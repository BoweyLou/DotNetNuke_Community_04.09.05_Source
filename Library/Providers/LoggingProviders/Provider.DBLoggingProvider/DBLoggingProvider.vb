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
Imports System.Data
Imports System.Threading
Imports DotNetNuke.Services.Log.EventLog
Imports DotNetNuke.Services.Log.EventLog.DBLoggingProvider.Data
Imports System.Text
Imports System.Xml.Serialization
Imports System.Web
Imports System.Xml
Imports System.io


Namespace DotNetNuke.Services.Log.EventLog.DBLoggingProvider

    Public Class DBLoggingProvider
        Inherits DotNetNuke.Services.Log.EventLog.LoggingProvider

        Public Shared LogQueue As New Collection

        Private Shared lockNotif As New ReaderWriterLock
        Private Shared lockPurgeLog As New ReaderWriterLock
        Private Const ReaderLockTimeout As Integer = 10000    'milliseconds
        Private Const WriterLockTimeout As Integer = 10000    'milliseconds

#Region "Private Methods"

        Private Function FillLogTypeConfigInfoByKey(ByVal arr As ArrayList) As Hashtable
            Dim ht As New Hashtable
            Dim i As Integer
            For i = 0 To arr.Count - 1
                Dim obj As LogTypeConfigInfo
                obj = CType(arr(i), LogTypeConfigInfo)
                If obj.LogTypeKey = "" Then obj.LogTypeKey = "*"
                If obj.LogTypePortalID = "" Then obj.LogTypePortalID = "*"
                ht.Add(obj.LogTypeKey + "|" + obj.LogTypePortalID, obj)
            Next
            DataCache.SetCache("GetLogTypeConfigInfoByKey", ht)
            Return ht
        End Function

        Private Function GetLogTypeConfigInfoByKey(ByVal LogTypeKey As String, ByVal LogTypePortalID As String) As LogTypeConfigInfo
            Dim ht As Hashtable
            ht = CType(DataCache.GetCache("GetLogTypeConfigInfoByKey"), Hashtable)
            If ht Is Nothing Then
                ht = FillLogTypeConfigInfoByKey(GetLogTypeConfigInfo())
            End If
            Dim objLogTypeConfigInfo As LogTypeConfigInfo
            objLogTypeConfigInfo = CType(ht(LogTypeKey + "|" + LogTypePortalID), LogTypeConfigInfo)
            If objLogTypeConfigInfo Is Nothing Then
                objLogTypeConfigInfo = CType(ht("*|" + LogTypePortalID), LogTypeConfigInfo)
                If objLogTypeConfigInfo Is Nothing Then
                    objLogTypeConfigInfo = CType(ht(LogTypeKey + "|*"), LogTypeConfigInfo)
                    If objLogTypeConfigInfo Is Nothing Then
                        objLogTypeConfigInfo = CType(ht("*|*"), LogTypeConfigInfo)
                    Else
                        Return objLogTypeConfigInfo
                    End If
                Else
                    Return objLogTypeConfigInfo
                End If
            Else
                Return objLogTypeConfigInfo
            End If

            Return objLogTypeConfigInfo
        End Function

        Private Function FillLogInfo(ByVal dr As IDataReader) As LogInfo
            Dim obj As New LogInfo()
            Try
                Dim LogGUID As String
                LogGUID = Convert.ToString(dr("LogGUID"))

                obj.LogCreateDate = Convert.ToDateTime(dr("LogCreateDate"))
                obj.LogGUID = Convert.ToString(dr("LogGUID"))
                If Not IsDBNull(dr("LogPortalID")) Then
                    obj.LogPortalID = Convert.ToInt32(dr("LogPortalID"))
                End If
                If Not IsDBNull(dr("LogPortalName")) Then
                    obj.LogPortalName = Convert.ToString(dr("LogPortalName"))
                End If
                If Not IsDBNull(dr("LogServerName")) Then
                    obj.LogServerName = Convert.ToString(dr("LogServerName"))
                End If
                If Not IsDBNull(dr("LogUserID")) Then
                    obj.LogUserID = Convert.ToInt32(dr("LogUserID"))
                End If
                obj.LogTypeKey = Convert.ToString(dr("LogTypeKey"))
                obj.LogUserName = Convert.ToString(dr("LogUserName"))
                obj.LogConfigID = Convert.ToString(dr("LogConfigID"))
                obj.LogProperties.Deserialize(Convert.ToString(dr("LogProperties")))
            Catch
            End Try
            Return obj
        End Function

        Private Sub WriteLog(ByVal objLogQueueItem As LogQueueItem)

            Dim objLogTypeConfigInfo As LogTypeConfigInfo = Nothing
            Try
                objLogTypeConfigInfo = objLogQueueItem.LogTypeConfigInfo
                If Not objLogTypeConfigInfo Is Nothing Then
                    Dim objLogInfo As LogInfo = objLogQueueItem.LogInfo
                    Dim LogProperties As String = objLogInfo.LogProperties.Serialize()

                    DataProvider.Instance.AddLog(objLogInfo.LogGUID, objLogInfo.LogTypeKey, objLogInfo.LogUserID, objLogInfo.LogUserName, objLogInfo.LogPortalID, objLogInfo.LogPortalName, objLogInfo.LogCreateDate, objLogInfo.LogServerName, LogProperties, Convert.ToInt32(objLogInfo.LogConfigID))

                    If objLogTypeConfigInfo.EmailNotificationIsActive = True Then
                        Try
                            lockNotif.AcquireWriterLock(ReaderLockTimeout)
                            If objLogTypeConfigInfo.NotificationThreshold = 0 Then
                                Dim str As String = objLogQueueItem.LogInfo.Serialize()
                                Mail.Mail.SendMail(objLogTypeConfigInfo.MailFromAddress, _
                                    objLogTypeConfigInfo.MailToAddress, "", "Event Notification", str, _
                                    "", "", "", "", "", "")
                            ElseIf objLogTypeConfigInfo.LogTypeKey <> "LOG_NOTIFICATION_FAILURE" Then
                                'pending log notifications go here
                            End If
                        Finally
                            lockNotif.ReleaseWriterLock()
                        End Try
                    End If
                End If

                If objLogTypeConfigInfo.EmailNotificationIsActive = True Then
                    If objLogTypeConfigInfo.NotificationThreshold = 0 Then
                        'SendNotification(objLogTypeConfigInfo.MailFromAddress, objLogTypeConfigInfo.MailToAddress, "", "Event Notification", xmlDoc.InnerXml)
                    ElseIf objLogTypeConfigInfo.LogTypeKey <> "LOG_NOTIFICATION_FAILURE" Then

                    End If
                End If

            Catch exc As Exception
                If Not HttpContext.Current Is Nothing Then
                    Dim response As HttpResponse = HttpContext.Current.Response
                    HtmlUtils.WriteHeader(response, "Unhandled Error")

                    Dim strMessage As String = exc.Message
                    HtmlUtils.WriteError(response, objLogTypeConfigInfo.LogFileNameWithPath, strMessage)

                    HtmlUtils.WriteFooter(response)
                    response.End()
                End If
            End Try

        End Sub

#End Region

#Region "Public Methods"

        Public Overrides Sub AddLog(ByVal objLogInfo As LogInfo)

            Dim ConfigPortalID As String
            If objLogInfo.LogPortalID <> Null.NullInteger Then
                ConfigPortalID = objLogInfo.LogPortalID.ToString
            Else
                ConfigPortalID = "*"
            End If

            Dim objLogTypeConfigInfo As LogTypeConfigInfo
            objLogTypeConfigInfo = GetLogTypeConfigInfoByKey(objLogInfo.LogTypeKey, ConfigPortalID)

            If objLogTypeConfigInfo.LoggingIsActive = False Then
                Exit Sub
            End If

            Dim UseEventLogBuffer As Boolean = True
            If Common.Globals.HostSettings.ContainsKey("EventLogBuffer") Then
                If Convert.ToString(HostSettings("EventLogBuffer")) <> "Y" Then
                    UseEventLogBuffer = False
                End If
            Else
                UseEventLogBuffer = False
            End If

            objLogInfo.LogConfigID = objLogTypeConfigInfo.ID

            Dim objLogQueueItem As New LogQueueItem
            objLogQueueItem.LogInfo = objLogInfo
            objLogQueueItem.LogTypeConfigInfo = objLogTypeConfigInfo

            Dim scheduler As Scheduling.SchedulingProvider = Scheduling.SchedulingProvider.Instance()
            If objLogInfo.BypassBuffering Or Scheduling.SchedulingProvider.Enabled = False Or scheduler.GetScheduleStatus = Scheduling.ScheduleStatus.STOPPED Or UseEventLogBuffer = False Then
                WriteLog(objLogQueueItem)
            Else
                LogQueue.Add(objLogQueueItem)
            End If

        End Sub

        Public Overrides Sub AddLogType(ByVal LogTypeKey As String, ByVal LogTypeFriendlyName As String, ByVal LogTypeDescription As String, ByVal LogTypeCSSClass As String, ByVal LogTypeOwner As String)
            DataProvider.Instance.AddLogType(LogTypeKey, LogTypeFriendlyName, LogTypeDescription, LogTypeCSSClass, LogTypeOwner)
        End Sub

        Public Overrides Sub AddLogTypeConfigInfo(ByVal ID As String, ByVal LoggingIsActive As Boolean, ByVal LogTypeKey As String, ByVal LogTypePortalID As String, ByVal KeepMostRecent As String, ByVal LogFileName As String, ByVal EmailNotificationIsActive As Boolean, ByVal Threshold As String, ByVal ThresholdTime As String, ByVal ThresholdTimeType As String, ByVal MailFromAddress As String, ByVal MailToAddress As String)
            Dim intThreshold As Integer = -1
            Dim intThresholdTime As Integer = -1
            Dim intThresholdTimeType As Integer = -1
            Dim intKeepMostRecent As Integer = -1

            If IsNumeric(Threshold) Then intThreshold = Convert.ToInt32(Threshold)
            If IsNumeric(ThresholdTime) Then intThresholdTime = Convert.ToInt32(ThresholdTime)
            If IsNumeric(ThresholdTimeType) Then intThresholdTimeType = Convert.ToInt32(ThresholdTimeType)
            If IsNumeric(KeepMostRecent) Then intKeepMostRecent = Convert.ToInt32(KeepMostRecent)

            DataProvider.Instance.AddLogTypeConfigInfo(LoggingIsActive, LogTypeKey, LogTypePortalID, intKeepMostRecent, EmailNotificationIsActive, intThreshold, intThresholdTime, intThresholdTimeType, MailFromAddress, MailToAddress)
            DataCache.RemoveCache("GetLogTypeConfigInfo")
            DataCache.RemoveCache("GetLogTypeConfigInfoByKey")
        End Sub

        Public Overrides Sub ClearLog()
            DataProvider.Instance.ClearLog()
        End Sub

        Public Overrides Sub DeleteLog(ByVal LogInfo As LogInfo)
            DataProvider.Instance.DeleteLog(LogInfo.LogGUID)
        End Sub

        Public Overrides Sub DeleteLogType(ByVal LogTypeKey As String)
            DataProvider.Instance.DeleteLogType(LogTypeKey)
        End Sub

        Public Overrides Sub DeleteLogTypeConfigInfo(ByVal ID As String)
            DataProvider.Instance.DeleteLogTypeConfigInfo(ID)
            DataCache.RemoveCache("GetLogTypeConfigInfo")
            DataCache.RemoveCache("GetLogTypeConfigInfoByKey")
        End Sub

        Public Overloads Overrides Function GetLog(ByVal PageSize As Integer, ByVal PageIndex As Integer, ByRef TotalRecords As Integer) As LogInfoArray
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(PageSize, PageIndex)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
                dr.NextResult()
                While dr.Read
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr
        End Function

        Public Overloads Overrides Function GetLog(ByVal LogType As String, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByRef TotalRecords As Integer) As LogInfoArray
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(LogType, PageSize, PageIndex)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
                dr.NextResult()
                While dr.Read
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr
        End Function

        Public Overloads Overrides Function GetLog(ByVal PortalID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByRef TotalRecords As Integer) As LogInfoArray
            If PortalID = -1 Then
                Return GetLog()
            End If
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(PortalID, PageSize, PageIndex)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
                dr.NextResult()
                While dr.Read
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr

        End Function

        Public Overloads Overrides Function GetLog(ByVal PortalID As Integer, ByVal LogType As String, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByRef TotalRecords As Integer) As LogInfoArray
            If PortalID = -1 Then
                Return GetLog(LogType)
            End If
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(PortalID, LogType, PageSize, PageIndex)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
                dr.NextResult()
                While dr.Read
                    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr

        End Function

        Public Overloads Overrides Function GetLog() As LogInfoArray
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog()
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr
        End Function

        Public Overloads Overrides Function GetLog(ByVal LogType As String) As LogInfoArray
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(LogType)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr
        End Function

        Public Overloads Overrides Function GetLog(ByVal PortalID As Integer) As LogInfoArray
            If PortalID = -1 Then
                Return GetLog()
            End If
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(PortalID)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr

        End Function

        Public Overloads Overrides Function GetLog(ByVal PortalID As Integer, ByVal LogType As String) As LogInfoArray
            If PortalID = -1 Then
                Return GetLog(LogType)
            End If
            Dim objArr As LogInfoArray = New LogInfoArray
            Dim dr As IDataReader = DataProvider.Instance.GetLog(PortalID, LogType)
            Try
                Dim objLogInfo As LogInfo
                While dr.Read
                    objLogInfo = FillLogInfo(dr)
                    objArr.Add(objLogInfo)
                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            Return objArr

        End Function

        Public Overrides Function GetLogTypeConfigInfo() As ArrayList
            Dim arr As ArrayList
            arr = CType(DataCache.GetCache("GetLogTypeConfigInfo"), ArrayList)
            If arr Is Nothing Then
                Dim dr As IDataReader = Nothing
                Try
                    dr = DataProvider.Instance.GetLogTypeConfigInfo()
                    arr = CBO.FillCollection(dr, GetType(LogTypeConfigInfo))
                    DataCache.SetCache("GetLogTypeConfigInfo", arr)
                    FillLogTypeConfigInfoByKey(arr)
                Finally
                    If dr Is Nothing Then
                        arr = New ArrayList
                    Else
                        dr.Close()
                    End If
                End Try
            End If
            Return arr
        End Function

        Public Overrides Function GetLogTypeConfigInfoByID(ByVal ID As String) As LogTypeConfigInfo
            Return CType(CBO.FillObject(DataProvider.Instance.GetLogTypeConfigInfoByID(Convert.ToInt32(ID)), GetType(LogTypeConfigInfo)), LogTypeConfigInfo)
        End Function

        Public Overrides Function GetLogTypeInfo() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.GetLogTypeInfo(), GetType(LogTypeInfo))
        End Function

        Public Overrides Function GetSingleLog(ByVal LogInfo As LogInfo, ByVal objReturnType As LoggingProvider.ReturnType) As Object
            Dim dr As IDataReader = DataProvider.Instance.GetSingleLog(LogInfo.LogGUID)
            Dim obj As LogInfo = Nothing
            Try
                If Not dr Is Nothing Then
                    dr.Read()
                    obj = FillLogInfo(dr)
                End If
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
            If objReturnType = LoggingProvider.ReturnType.LogInfoObjects Then
                Return obj
            Else
                Dim xmlDoc As New XmlDocument
                xmlDoc.LoadXml(obj.Serialize())
                Return CType(xmlDoc.DocumentElement, XmlNode)
            End If
        End Function

        Public Overrides Function LoggingIsEnabled(ByVal LogType As String, ByVal PortalID As Integer) As Boolean
            Dim ConfigPortalID As String = PortalID.ToString
            If PortalID = -1 Then
                ConfigPortalID = "*"
            End If
            Dim obj As LogTypeConfigInfo
            obj = GetLogTypeConfigInfoByKey(LogType, ConfigPortalID)
            If obj Is Nothing Then
                Return False
            End If
            Return obj.LoggingIsActive
        End Function

        Public Overrides Sub PurgeLogBuffer()
            Try
                lockPurgeLog.AcquireWriterLock(WriterLockTimeout)
                Dim i As Integer
                Dim c As Collection = LogQueue
                Dim j As Integer = c.Count
                For i = 1 To c.Count
                    'in case the log was removed
                    'by another thread simultaneously
                    If Not c(j) Is Nothing Then
                        Dim objLogQueueItem As LogQueueItem
                        objLogQueueItem = CType(c(j), LogQueueItem)
                        WriteLog(objLogQueueItem)
                    End If
                    'in case the log was removed
                    'by another thread simultaneously
                    If Not c(j) Is Nothing Then
                        c.Remove(j)
                    End If

                    'use "j" instead of "i" so we
                    'can iterate in reverse, taking
                    'items out of the rear of the
                    'collection
                    j -= 1
                Next

                DataProvider.Instance.PurgeLog()

            Finally
                lockPurgeLog.ReleaseWriterLock()
            End Try
        End Sub

        Public Overrides Sub SendLogNotifications()
            Dim arrLogConfig As ArrayList

            arrLogConfig = CBO.FillCollection(DataProvider.Instance.GetEventLogPendingNotifConfig(), GetType(LogTypeConfigInfo))

            Dim i As Integer
            For i = 0 To arrLogConfig.Count - 1
                Dim objLogConfig As LogTypeConfigInfo
                objLogConfig = CType(arrLogConfig(i), LogTypeConfigInfo)

                Dim dr As IDataReader = DataProvider.Instance.GetEventLogPendingNotif(Convert.ToInt32(objLogConfig.ID))
                Dim strLog As String = ""
                Try
                    While dr.Read
                        Dim objLogInfo As LogInfo = Me.FillLogInfo(dr)
                        strLog += objLogInfo.Serialize() + vbCrLf + vbCrLf
                    End While
                Finally
                    If Not dr Is Nothing Then
                        dr.Close()
                    End If
                End Try
                dr = Nothing
                Mail.Mail.SendMail(objLogConfig.MailFromAddress, objLogConfig.MailToAddress, "", _
                    "Event Notification", strLog, "", "", "", "", "", "")
                DataProvider.Instance.UpdateEventLogPendingNotif(Convert.ToInt32(objLogConfig.ID))
            Next

        End Sub

        Public Overrides Function SupportsEmailNotification() As Boolean
            Return True
        End Function

        Public Overrides Function SupportsInternalViewer() As Boolean
            Return True
        End Function

        Public Overrides Function SupportsSendToCoreTeam() As Boolean
            Return False
        End Function

        Public Overrides Function SupportsSendViaEmail() As Boolean
            Return True
        End Function

        Public Overrides Sub UpdateLogType(ByVal LogTypeKey As String, ByVal LogTypeFriendlyName As String, ByVal LogTypeDescription As String, ByVal LogTypeCSSClass As String, ByVal LogTypeOwner As String)
            DataProvider.Instance.UpdateLogType(LogTypeKey, LogTypeFriendlyName, LogTypeDescription, LogTypeCSSClass, LogTypeOwner)
        End Sub

        Public Overrides Sub UpdateLogTypeConfigInfo(ByVal ID As String, ByVal LoggingIsActive As Boolean, ByVal LogTypeKey As String, ByVal LogTypePortalID As String, ByVal KeepMostRecent As String, ByVal LogFileName As String, ByVal EmailNotificationIsActive As Boolean, ByVal Threshold As String, ByVal ThresholdTime As String, ByVal ThresholdTimeType As String, ByVal MailFromAddress As String, ByVal MailToAddress As String)
            Dim intThreshold As Integer = -1
            Dim intThresholdTime As Integer = -1
            Dim intThresholdTimeType As Integer = -1
            Dim intKeepMostRecent As Integer = -1

            If IsNumeric(Threshold) Then intThreshold = Convert.ToInt32(Threshold)
            If IsNumeric(ThresholdTime) Then intThresholdTime = Convert.ToInt32(ThresholdTime)
            If IsNumeric(ThresholdTimeType) Then intThresholdTimeType = Convert.ToInt32(ThresholdTimeType)
            If IsNumeric(KeepMostRecent) Then intKeepMostRecent = Convert.ToInt32(KeepMostRecent)

            DataProvider.Instance.UpdateLogTypeConfigInfo(ID, LoggingIsActive, LogTypeKey, LogTypePortalID, intKeepMostRecent, LogFileName, EmailNotificationIsActive, intThreshold, intThresholdTime, intThresholdTimeType, MailFromAddress, MailToAddress)
            DataCache.RemoveCache("GetLogTypeConfigInfo")
            DataCache.RemoveCache("GetLogTypeConfigInfoByKey")
        End Sub

#End Region

    End Class

End Namespace
