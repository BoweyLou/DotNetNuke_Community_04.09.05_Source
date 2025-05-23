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

Imports System.Collections.Specialized
Imports System.Text
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Security.Permissions
Imports System.Web
Imports System.Reflection
Imports System.Diagnostics
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions


Namespace DotNetNuke.Services.Exceptions

    Public Module Exceptions

        Public Function GetExceptionInfo(ByVal e As Exception) As ExceptionInfo
            Dim objExceptionInfo As New ExceptionInfo

            Do Until e.InnerException Is Nothing
                e = e.InnerException
            Loop

            Dim st As New StackTrace(e, True)
            Dim sf As StackFrame = st.GetFrame(0)

            Try
                'Get the corresponding method for that stack frame.
                Dim mi As MemberInfo = sf.GetMethod
                ' Get the namespace where that method is defined.
                Dim res As String = mi.DeclaringType.Namespace & "."
                ' Append the type name.
                res &= mi.DeclaringType.Name & "."
                ' Append the name of the method.
                res &= mi.Name
                objExceptionInfo.Method = res
            Catch ex As Exception
                objExceptionInfo.Method = "N/A - Reflection Permission required"
            End Try

            If sf.GetFileName <> "" Then
                objExceptionInfo.FileName = sf.GetFileName
                objExceptionInfo.FileColumnNumber = sf.GetFileColumnNumber
                objExceptionInfo.FileLineNumber = sf.GetFileLineNumber
            End If

            Return objExceptionInfo
        End Function

        Private Function ThreadAbortCheck(ByVal exc As Exception) As Boolean
            If TypeOf exc Is Threading.ThreadAbortException Then
                Threading.Thread.ResetAbort()
                Return True
            Else : Return False
            End If

        End Function

#Region "Module Load Exceptions"

        Public Sub ProcessModuleLoadException(ByVal FriendlyMessage As String, ByVal ctrlModule As Entities.Modules.PortalModuleBase, ByVal exc As Exception, ByVal DisplayErrorMessage As Boolean)
            If ThreadAbortCheck(exc) Then Exit Sub
            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
            Try
                If Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "N" Then
                    Throw New ModuleLoadException(FriendlyMessage, exc)
                Else
                    Dim lex As New ModuleLoadException(exc.Message.ToString, exc, ctrlModule.ModuleConfiguration)
                    'publish the exception
                    Dim objExceptionLog As New Services.log.EventLog.ExceptionLogController
                    objExceptionLog.AddLog(lex)


                    'Some modules may want to suppress an error message
                    'and just log the exception.
                    If DisplayErrorMessage Then

                        Dim ErrorPlaceholder As PlaceHolder = Nothing
                        If Not ctrlModule.Parent Is Nothing Then
                            ErrorPlaceholder = CType(ctrlModule.Parent.FindControl("MessagePlaceHolder"), PlaceHolder)
                        End If
                        If Not ErrorPlaceholder Is Nothing Then

                            'hide the module
                            ctrlModule.Visible = False
                            ErrorPlaceholder.Visible = True
                            ErrorPlaceholder.Controls.Add(New ErrorContainer(_portalSettings, FriendlyMessage, lex).Container)         'add the error message to the error placeholder
                        Else
                            'there's no ErrorPlaceholder, add it to the module's control collection
                            ctrlModule.Controls.Add(New ErrorContainer(_portalSettings, FriendlyMessage, lex).Container)
                        End If
                    End If
                End If
            Catch exc2 As Exception When Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "Y"
                ProcessPageLoadException(exc2)
            End Try
        End Sub

        Public Sub ProcessModuleLoadException(ByVal ctrlModule As Entities.Modules.PortalModuleBase, ByVal exc As Exception)
            ProcessModuleLoadException(ctrlModule, exc, True)
        End Sub

        Public Sub ProcessModuleLoadException(ByVal ctrlModule As Entities.Modules.PortalModuleBase, ByVal exc As Exception, ByVal DisplayErrorMessage As Boolean)
            If ThreadAbortCheck(exc) Then Exit Sub
            Dim moduleTitle As String = Null.NullString
            If ctrlModule IsNot Nothing AndAlso ctrlModule.ModuleConfiguration IsNot Nothing Then
                moduleTitle = ctrlModule.ModuleConfiguration.ModuleTitle
            End If
            ProcessModuleLoadException(String.Format(Localization.Localization.GetString("ModuleUnavailable"), moduleTitle), ctrlModule, exc, DisplayErrorMessage)
        End Sub


        Public Sub ProcessModuleLoadException(ByVal FriendlyMessage As String, ByVal UserCtrl As Control, ByVal exc As Exception)

            If ThreadAbortCheck(exc) Then Exit Sub

            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
            Try

                If Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "N" Then
                    Throw New ModuleLoadException(FriendlyMessage, exc)
                Else
                    Dim lex As New ModuleLoadException(exc.Message.ToString, exc)
                    'publish the exception
                    Dim objExceptionLog As New Services.Log.EventLog.ExceptionLogController
                    objExceptionLog.AddLog(lex)
                    'add the error message to the user control
                    UserCtrl.Controls.Add(New ErrorContainer(_portalSettings, FriendlyMessage, lex).Container)
                End If
            Catch When Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "Y"
                ProcessPageLoadException(exc)
            End Try
        End Sub

        Public Sub ProcessModuleLoadException(ByVal FriendlyMessage As String, ByVal ctrlModule As Control, ByVal exc As Exception, ByVal DisplayErrorMessage As Boolean)
            If ThreadAbortCheck(exc) Then Exit Sub
            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
            Try
                If Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "N" Then
                    Throw New ModuleLoadException(FriendlyMessage, exc)
                Else
                    Dim lex As New ModuleLoadException(exc.Message.ToString, exc)
                    'publish the exception
                    Dim objExceptionLog As New Services.Log.EventLog.ExceptionLogController
                    objExceptionLog.AddLog(lex)
                    'Some modules may want to suppress an error message
                    'and just log the exception.
                    If DisplayErrorMessage Then
                        Dim ErrorPlaceholder As PlaceHolder = CType(ctrlModule.Parent.FindControl("MessagePlaceHolder"), PlaceHolder)
                        If Not ErrorPlaceholder Is Nothing Then

                            'hide the module
                            ctrlModule.Visible = False
                            ErrorPlaceholder.Visible = True

                            ErrorPlaceholder.Controls.Add(New ErrorContainer(_portalSettings, FriendlyMessage, lex).Container)         'add the error message to the error placeholder
                        Else
                            'there's no ErrorPlaceholder, add it to the module's control collection
                            ctrlModule.Controls.Add(New ErrorContainer(_portalSettings, FriendlyMessage, lex).Container)
                        End If
                    End If
                End If
            Catch exc2 As Exception When Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "Y"
                ProcessPageLoadException(exc2)
            End Try
        End Sub

        Public Sub ProcessModuleLoadException(ByVal UserCtrl As Control, ByVal exc As Exception)
            If ThreadAbortCheck(exc) Then Exit Sub
            ProcessModuleLoadException(Services.Localization.Localization.GetString("ErrorOccurred"), UserCtrl, exc)
        End Sub

        Public Sub ProcessModuleLoadException(ByVal UserCtrl As Control, ByVal exc As Exception, ByVal DisplayErrorMessage As Boolean)
            If ThreadAbortCheck(exc) Then Exit Sub
            ProcessModuleLoadException(Services.Localization.Localization.GetString("ErrorOccurred"), UserCtrl, exc, DisplayErrorMessage)
        End Sub

#End Region

#Region "Page Load Exceptions"

        Public Sub ProcessPageLoadException(ByVal exc As Exception)
            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
            Dim appURL As String = ApplicationURL()
            If appURL.IndexOf("?") = Null.NullInteger Then
                appURL += "?def=ErrorMessage"
            Else
                appURL += "&def=ErrorMessage"
            End If
            ProcessPageLoadException(exc, appURL)
        End Sub

        Public Sub ProcessPageLoadException(ByVal exc As Exception, ByVal URL As String)
            If ThreadAbortCheck(exc) Then Exit Sub

            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings

            Dim useCustomMessages As Boolean = True
            If Convert.ToString(Common.Globals.HostSettings("UseCustomErrorMessages")) = "N" Then
                useCustomMessages = False
            End If

            If Not useCustomMessages Then
                Throw New PageLoadException(exc.Message, exc)
            Else
                Dim lex As New PageLoadException(exc.Message.ToString, exc)
                'publish the exception
                Dim objExceptionLog As New Services.Log.EventLog.ExceptionLogController
                objExceptionLog.AddLog(lex)
                ' redirect

                If URL <> "" Then
                    If URL.IndexOf("error=terminate") <> -1 Then
                        HttpContext.Current.Response.Clear()
                        HttpContext.Current.Server.Transfer("~/ErrorPage.aspx")
                    Else
                        HttpContext.Current.Response.Redirect(URL, True)
                    End If
                End If
            End If
        End Sub

#End Region

#Region "General Exceptions"

        Public Sub LogException(ByVal exc As ModuleLoadException)
            Dim objExceptionLog As New Services.Log.EventLog.ExceptionLogController
            objExceptionLog.AddLog(exc, Services.Log.EventLog.ExceptionLogController.ExceptionLogType.MODULE_LOAD_EXCEPTION)
        End Sub

        Public Sub LogException(ByVal exc As PageLoadException)
            Dim objExceptionLog As New Services.Log.EventLog.ExceptionLogController
            objExceptionLog.AddLog(exc, Services.Log.EventLog.ExceptionLogController.ExceptionLogType.PAGE_LOAD_EXCEPTION)
        End Sub

        Public Sub LogException(ByVal exc As SchedulerException)
            Dim objExceptionLog As New Log.EventLog.ExceptionLogController
            objExceptionLog.AddLog(exc, Log.EventLog.ExceptionLogController.ExceptionLogType.SCHEDULER_EXCEPTION)
        End Sub

        Public Sub LogException(ByVal exc As SecurityException)
            Dim objExceptionLog As New Log.EventLog.ExceptionLogController
            objExceptionLog.AddLog(exc, Log.EventLog.ExceptionLogController.ExceptionLogType.SECURITY_EXCEPTION)
        End Sub

        Public Sub LogException(ByVal exc As Exception)
            Dim objExceptionLog As New Log.EventLog.ExceptionLogController
            objExceptionLog.AddLog(exc, Log.EventLog.ExceptionLogController.ExceptionLogType.GENERAL_EXCEPTION)
        End Sub

#End Region

#Region "Scheduler Exception"

        Public Sub ProcessSchedulerException(ByVal exc As Exception)
            Dim objExceptionLog As New Log.EventLog.ExceptionLogController
            objExceptionLog.AddLog(exc, Log.EventLog.ExceptionLogController.ExceptionLogType.SCHEDULER_EXCEPTION)
        End Sub

#End Region

    End Module
End Namespace
