'
' DotNetNukeŽ - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports System


Namespace DotNetNuke.Services.Scheduling


    Public Enum EventName
        'do not add APPLICATION_END
        'it will not reliably complete
        APPLICATION_START
    End Enum

    Public Enum ScheduleSource
        NOT_SET
        STARTED_FROM_SCHEDULE_CHANGE
        STARTED_FROM_EVENT
        STARTED_FROM_TIMER
        STARTED_FROM_BEGIN_REQUEST
    End Enum

    Public Enum ScheduleStatus
        NOT_SET
        WAITING_FOR_OPEN_THREAD
        RUNNING_EVENT_SCHEDULE
        RUNNING_TIMER_SCHEDULE
        RUNNING_REQUEST_SCHEDULE
        WAITING_FOR_REQUEST
        SHUTTING_DOWN
        STOPPED
    End Enum

    Public Enum SchedulerMode
        DISABLED = 0
        TIMER_METHOD = 1
        REQUEST_METHOD = 2
    End Enum


    ' set up our delegates so we can track and react to events of the scheduler clients
    Public Delegate Sub WorkStarted(ByRef objSchedulerClient As SchedulerClient)
    Public Delegate Sub WorkProgressing(ByRef objSchedulerClient As SchedulerClient)
    Public Delegate Sub WorkCompleted(ByRef objSchedulerClient As SchedulerClient)
    Public Delegate Sub WorkErrored(ByRef objSchedulerClient As SchedulerClient, ByRef objException As Exception)


    Public MustInherit Class SchedulingProvider

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration("scheduling")
        Private _providerPath As String
        Private Shared _Debug As Boolean
        Private Shared _MaxThreads As Integer
        Public EventName As Scheduling.EventName

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public Shared ReadOnly Property Debug() As Boolean
            Get
                Return _Debug
            End Get
        End Property

        Public Shared ReadOnly Property MaxThreads() As Integer
            Get
                Return _MaxThreads
            End Get
        End Property

        Public Shared ReadOnly Property Enabled() As Boolean
            Get
                If SchedulerMode <> SchedulerMode.DISABLED Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Shared Property ScheduleLastPolled() As Date
            Get
                If Not DataCache.GetCache("ScheduleLastPolled") Is Nothing Then
                    Return Convert.ToDateTime(DataCache.GetCache("ScheduleLastPolled"))
                Else
                    Return Date.MinValue
                End If
            End Get
            Set(ByVal Value As Date)
                Dim ns As Date
                Dim s As Scheduling.ScheduleItem
                s = Instance.GetNextScheduledTask(Common.Globals.ServerName)
                If Not s Is Nothing Then
                    Dim NextStart As Date = s.NextStart
                    If NextStart >= Now Then
                        ns = NextStart
                    Else
                        ns = Now.AddMinutes(1)
                    End If
                Else
                    ns = Now.AddMinutes(1)
                End If
                DataCache.SetCache("ScheduleLastPolled", Value, ns)
            End Set
        End Property

        Public Shared ReadOnly Property ReadyForPoll() As Boolean
            Get
                If DataCache.GetCache("ScheduleLastPolled") Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Shared ReadOnly Property SchedulerMode() As Services.Scheduling.SchedulerMode
            Get
                Dim s As String = Convert.ToString(Common.Globals.HostSettings("SchedulerMode"))
                If s.Length = 0 Then
                    Return SchedulerMode.TIMER_METHOD
                Else
                    Return CType(Common.Globals.HostSettings("SchedulerMode"), Services.Scheduling.SchedulerMode)
                End If
            End Get
        End Property

        Public Sub New()
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            _providerPath = objProvider.Attributes("providerPath")

            If Not objProvider.Attributes("debug") Is Nothing Then
                _Debug = Convert.ToBoolean(objProvider.Attributes("debug"))
            End If
            If Not objProvider.Attributes("maxThreads") Is Nothing Then
                _MaxThreads = Convert.ToInt32(objProvider.Attributes("maxThreads"))
            Else
                _MaxThreads = 1
            End If

        End Sub

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As SchedulingProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("scheduling"), SchedulingProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As SchedulingProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract Methods"

        Public MustOverride Function GetProviderPath() As String

        Public MustOverride Sub Start()
        Public MustOverride Sub ExecuteTasks()
        Public MustOverride Sub ReStart(ByVal SourceOfRestart As String)
        Public MustOverride Sub StartAndWaitForResponse()
        Public MustOverride Sub Halt(ByVal SourceOfHalt As String)
        Public MustOverride Sub PurgeScheduleHistory()
        Public MustOverride Sub RunEventSchedule(ByVal objEventName As Scheduling.EventName)
        Public MustOverride Function GetSchedule() As ArrayList
        Public MustOverride Function GetSchedule(ByVal Server As String) As ArrayList
        Public MustOverride Function GetSchedule(ByVal ScheduleID As Integer) As ScheduleItem
        Public MustOverride Function GetSchedule(ByVal TypeFullName As String, ByVal Server As String) As ScheduleItem
        Public MustOverride Function GetNextScheduledTask(ByVal Server As String) As ScheduleItem
        Public MustOverride Function GetScheduleHistory(ByVal ScheduleID As Integer) As ArrayList
        Public MustOverride Function GetScheduleItemSettings(ByVal ScheduleID As Integer) As Hashtable
        Public MustOverride Sub AddScheduleItemSetting(ByVal ScheduleID As Integer, ByVal Name As String, ByVal Value As String)
        Public MustOverride Function GetScheduleQueue() As Collection
        Public MustOverride Function GetScheduleProcessing() As Collection
        Public MustOverride Function GetFreeThreadCount() As Integer
        Public MustOverride Function GetActiveThreadCount() As Integer
        Public MustOverride Function GetMaxThreadCount() As Integer
        Public MustOverride Function GetScheduleStatus() As ScheduleStatus
        Public MustOverride Function AddSchedule(ByVal objScheduleItem As ScheduleItem) As Integer
        Public MustOverride Sub UpdateSchedule(ByVal objScheduleItem As ScheduleItem)
        Public MustOverride Sub DeleteSchedule(ByVal objScheduleItem As ScheduleItem)

#End Region

    End Class

End Namespace
