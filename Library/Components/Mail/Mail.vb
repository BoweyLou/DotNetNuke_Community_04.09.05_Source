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

Imports Localize = DotNetNuke.Services.Localization.Localization

Namespace DotNetNuke.Services.Mail

    Public Enum MailFormat
        Text
        Html
    End Enum

    Public Enum MailPriority
        Normal
        Low
        High
    End Enum

    Public Enum MessageType
        PasswordReminder
        ProfileUpdated
        UserRegistrationAdmin
        UserRegistrationPrivate
        UserRegistrationPublic
        UserRegistrationVerified
    End Enum


    Public Class Mail

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' <summary>Send an email notification</summary>
        ''' </summary>
        ''' <param name="user">The user to whom the message is being sent</param>
        ''' <param name="msgType">The type of message being sent</param>
        ''' <param name="settings">Portal Settings</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''     [cnurse]        09/29/2005  Moved to Mail class
        '''     [sLeupold]      02/07/2008 language used for admin mails corrected
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function SendMail(ByVal user As UserInfo, ByVal msgType As MessageType, ByVal settings As PortalSettings) As String
            'Send Notification to User
            Dim userEmail As String = user.Email
            Dim locale As String = user.Profile.PreferredLocale
            Dim subject As String = ""
            Dim body As String = ""
            Dim custom As ArrayList = Nothing
            Select Case msgType
                Case MessageType.UserRegistrationAdmin
                    subject = "EMAIL_USER_REGISTRATION_ADMINISTRATOR_SUBJECT"
                    body = "EMAIL_USER_REGISTRATION_ADMINISTRATOR_BODY"
                    userEmail = settings.Email
                    Dim admin As UserInfo = DotNetNuke.Entities.Users.UserController.GetUser(settings.PortalId, settings.AdministratorId, True)
                    locale = admin.Profile.PreferredLocale
                Case MessageType.UserRegistrationPrivate
                    subject = "EMAIL_USER_REGISTRATION_PRIVATE_SUBJECT"
                    body = "EMAIL_USER_REGISTRATION_PRIVATE_BODY"
                Case MessageType.UserRegistrationPublic
                    subject = "EMAIL_USER_REGISTRATION_PUBLIC_SUBJECT"
                    body = "EMAIL_USER_REGISTRATION_PUBLIC_BODY"
                Case MessageType.UserRegistrationVerified
                    subject = "EMAIL_USER_REGISTRATION_VERIFIED_SUBJECT"
                    body = "EMAIL_USER_REGISTRATION_VERIFIED_BODY"
                    If Not HttpContext.Current Is Nothing Then
                        custom = New ArrayList
                        custom.Add(HttpContext.Current.Server.UrlEncode(user.Username))
                    End If
                Case MessageType.PasswordReminder
                    subject = "EMAIL_PASSWORD_REMINDER_SUBJECT"
                    body = "EMAIL_PASSWORD_REMINDER_BODY"
                Case MessageType.ProfileUpdated
                    subject = "EMAIL_PROFILE_UPDATED_SUBJECT"
                    body = "EMAIL_PROFILE_UPDATED_BODY"
            End Select

            Return SendMail(settings.Email, userEmail, "", _
                            Localize.GetSystemMessage(locale, settings, subject, user, Localize.GlobalResourceFile, custom, "", settings.AdministratorId), _
                            Localize.GetSystemMessage(locale, settings, body, user, Localize.GlobalResourceFile, custom, "", settings.AdministratorId), _
                            "", "", "", "", "", "")
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' <summary>Send a simple email.</summary>
        ''' </summary>
        ''' <param name="MailFrom"></param>
        ''' <param name="MailTo"></param>
        ''' <param name="Bcc"></param>
        ''' <param name="Subject"></param>
        ''' <param name="Body"></param>
        ''' <param name="Attachment"></param>
        ''' <param name="BodyType"></param>
        ''' <param name="SMTPServer"></param>
        ''' <param name="SMTPAuthentication"></param>
        ''' <param name="SMTPUsername"></param>
        ''' <param name="SMTPPassword"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''     [cnurse]        09/29/2005  Moved to Mail class
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function SendMail(ByVal MailFrom As String, ByVal MailTo As String, ByVal Bcc As String, ByVal Subject As String, ByVal Body As String, ByVal Attachment As String, ByVal BodyType As String, ByVal SMTPServer As String, ByVal SMTPAuthentication As String, ByVal SMTPUsername As String, ByVal SMTPPassword As String) As String

            ' here we check if we want to format the email as html or plain text.
            Dim objBodyFormat As MailFormat
            If BodyType <> "" Then
                Select Case LCase(BodyType)
                    Case "html"
                        objBodyFormat = MailFormat.Html
                    Case "text"
                        objBodyFormat = MailFormat.Text
                End Select
            End If

            Return SendMail(MailFrom, MailTo, "", Bcc, MailPriority.Normal, _
                Subject, objBodyFormat, System.Text.Encoding.UTF8, Body, Attachment, SMTPServer, _
                SMTPAuthentication, SMTPUsername, SMTPPassword)

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>Send a simple email.</summary>
        ''' <param name="MailFrom"></param>
        ''' <param name="MailTo"></param>
        ''' <param name="Cc"></param>
        ''' <param name="Bcc"></param>
        ''' <param name="Priority"></param>
        ''' <param name="Subject"></param>
        ''' <param name="BodyFormat"></param>
        ''' <param name="BodyEncoding"></param>
        ''' <param name="Body"></param>
        ''' <param name="Attachment"></param>
        ''' <param name="SMTPServer"></param>
        ''' <param name="SMTPAuthentication"></param>
        ''' <param name="SMTPUsername"></param>
        ''' <param name="SMTPPassword"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 	[Nik Kalyani]	10/15/2004	Replaced brackets in member names
        '''     [cnurse]        09/29/2005  Moved to Mail class
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function SendMail(ByVal MailFrom As String, ByVal MailTo As String, _
            ByVal Cc As String, ByVal Bcc As String, ByVal Priority As MailPriority, _
            ByVal Subject As String, ByVal BodyFormat As MailFormat, _
            ByVal BodyEncoding As System.Text.Encoding, ByVal Body As String, _
            ByVal Attachment As String, ByVal SMTPServer As String, ByVal SMTPAuthentication As String, _
            ByVal SMTPUsername As String, ByVal SMTPPassword As String) As String

            Dim SMTPEnableSSL As Boolean = Null.NullBoolean
            If Convert.ToString(Common.Globals.HostSettings("SMTPEnableSSL")) = "Y" Then
                SMTPEnableSSL = True
            End If

            Return SendMail(MailFrom, MailTo, Cc, Bcc, Priority, Subject, BodyFormat, BodyEncoding, _
                Body, Attachment, SMTPServer, SMTPAuthentication, SMTPUsername, SMTPPassword, SMTPEnableSSL)

        End Function

        Public Shared Function SendMail(ByVal MailFrom As String, ByVal MailTo As String, _
            ByVal Cc As String, ByVal Bcc As String, ByVal Priority As MailPriority, _
            ByVal Subject As String, ByVal BodyFormat As MailFormat, _
            ByVal BodyEncoding As System.Text.Encoding, ByVal Body As String, _
            ByVal Attachment As String, ByVal SMTPServer As String, ByVal SMTPAuthentication As String, _
            ByVal SMTPUsername As String, ByVal SMTPPassword As String, ByVal SMTPEnableSSL As Boolean) As String

            Return SendMail(MailFrom, MailTo, Cc, Bcc, MailFrom, Priority, Subject, BodyFormat, BodyEncoding, Body, Split(Attachment, "|"), _
                            SMTPServer, SMTPAuthentication, SMTPUsername, SMTPPassword, SMTPEnableSSL)
        End Function

        Public Shared Function SendMail(ByVal MailFrom As String, ByVal MailTo As String, _
            ByVal Cc As String, ByVal Bcc As String, _
            ByVal Priority As MailPriority, ByVal Subject As String, _
            ByVal BodyFormat As MailFormat, ByVal BodyEncoding As System.Text.Encoding, ByVal Body As String, _
            ByVal Attachment() As String, ByVal SMTPServer As String, ByVal SMTPAuthentication As String, _
            ByVal SMTPUsername As String, ByVal SMTPPassword As String, ByVal SMTPEnableSSL As Boolean) As String

            Return SendMail(MailFrom, MailTo, Cc, Bcc, MailFrom, Priority, Subject, BodyFormat, BodyEncoding, Body, Attachment, _
                            SMTPServer, SMTPAuthentication, SMTPUsername, SMTPPassword, SMTPEnableSSL)
        End Function

        Public Shared Function SendMail(ByVal MailFrom As String, ByVal MailTo As String, _
            ByVal Cc As String, ByVal Bcc As String, ByVal ReplyTo As String, _
            ByVal Priority As MailPriority, ByVal Subject As String, _
            ByVal BodyFormat As MailFormat, ByVal BodyEncoding As System.Text.Encoding, ByVal Body As String, _
            ByVal Attachment() As String, ByVal SMTPServer As String, ByVal SMTPAuthentication As String, _
            ByVal SMTPUsername As String, ByVal SMTPPassword As String, ByVal SMTPEnableSSL As Boolean) As String

            SendMail = ""

            ' SMTP server configuration
            If SMTPServer = "" Then
                If Convert.ToString(Common.Globals.HostSettings("SMTPServer")) <> "" Then
                    SMTPServer = Convert.ToString(Common.Globals.HostSettings("SMTPServer"))
                End If
            End If
            If SMTPAuthentication = "" Then
                If Convert.ToString(Common.Globals.HostSettings("SMTPAuthentication")) <> "" Then
                    SMTPAuthentication = Convert.ToString(Common.Globals.HostSettings("SMTPAuthentication"))
                End If
            End If
            If SMTPUsername = "" Then
                If Convert.ToString(Common.Globals.HostSettings("SMTPUsername")) <> "" Then
                    SMTPUsername = Convert.ToString(Common.Globals.HostSettings("SMTPUsername"))
                End If
            End If
            If SMTPPassword = "" Then
                If Convert.ToString(Common.Globals.HostSettings("SMTPPassword")) <> "" Then
                    SMTPPassword = Convert.ToString(Common.Globals.HostSettings("SMTPPassword"))
                End If
            End If

            ' translate semi-colon delimiters to commas as ASP.NET 2.0 does not support semi-colons
            MailTo = MailTo.Replace(";", ",")
            Cc = Cc.Replace(";", ",")
            Bcc = Bcc.Replace(";", ",")

            Dim objMail As System.Net.Mail.MailMessage = Nothing
            Try
                objMail = New System.Net.Mail.MailMessage(MailFrom, MailTo)
                If Cc <> "" Then
                    objMail.CC.Add(Cc)
                End If
                If Bcc <> "" Then
                    objMail.Bcc.Add(Bcc)
                End If
                If ReplyTo <> String.Empty Then objMail.ReplyTo = New System.Net.Mail.MailAddress(ReplyTo)
                objMail.Priority = CType(Priority, Net.Mail.MailPriority)
                objMail.IsBodyHtml = CBool(IIf(BodyFormat = MailFormat.Html, True, False))

                For Each myAtt As String In Attachment
                    If myAtt <> "" Then objMail.Attachments.Add(New Net.Mail.Attachment(myAtt))
                Next

                ' message
                objMail.SubjectEncoding = BodyEncoding
                objMail.Subject = HtmlUtils.StripWhiteSpace(Subject, True)
                objMail.BodyEncoding = BodyEncoding


                'added support for multipart html messages
                'add text part as alternate view
                'objMail.Body = Body
                Dim PlainView As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(ConvertToText(Body), Nothing, "text/plain")
                objMail.AlternateViews.Add(PlainView)

                'if body contains html, add html part
                If IsHTMLMail(Body) Then
                    Dim HTMLView As System.Net.Mail.AlternateView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(Body, Nothing, "text/html")
                    objMail.AlternateViews.Add(HTMLView)
                End If

            Catch objException As Exception
                ' Problem creating Mail Object
                SendMail = MailTo + ": " + objException.Message
                LogException(objException)
            End Try

            If objMail IsNot Nothing Then

                ' external SMTP server alternate port
                Dim SmtpPort As Integer = Null.NullInteger
                Dim portPos As Integer = SMTPServer.IndexOf(":")
                If portPos > -1 Then
                    SmtpPort = Int32.Parse(SMTPServer.Substring(portPos + 1, SMTPServer.Length - portPos - 1))
                    SMTPServer = SMTPServer.Substring(0, portPos)
                End If

                Dim smtpClient As New Net.Mail.SmtpClient()

                If SMTPServer <> "" Then
                    smtpClient.Host = SMTPServer
                    If SmtpPort > Null.NullInteger Then
                        smtpClient.Port = SmtpPort
                    End If
                    Select Case SMTPAuthentication
                        Case "", "0" ' anonymous
                        Case "1" ' basic
                            If SMTPUsername <> "" And SMTPPassword <> "" Then
                                smtpClient.UseDefaultCredentials = False
                                smtpClient.Credentials = New System.Net.NetworkCredential(SMTPUsername, SMTPPassword)
                            End If
                        Case "2" ' NTLM
                            smtpClient.UseDefaultCredentials = True
                    End Select
                End If
                smtpClient.EnableSsl = SMTPEnableSSL

                Try
                    smtpClient.Send(objMail)
                    SendMail = ""
                Catch objException As Exception
                    ' mail configuration problem
                    If Not IsNothing(objException.InnerException) Then
                        SendMail = String.Concat(objException.Message, ControlChars.CrLf, objException.InnerException.Message)
                        LogException(objException.InnerException)
                    Else
                        SendMail = objException.Message
                        LogException(objException)
                    End If
                Finally
                    objMail.Dispose()
                End Try
            End If

        End Function

        Public Shared Function IsHTMLMail(ByVal Body As String) As Boolean
            Return System.Text.RegularExpressions.Regex.IsMatch(Body, "<[^>]*>")
        End Function

        Public Shared Function ConvertToText(ByVal sHTML As String) As String
            Dim sContent As String = sHTML
            sContent = sContent.Replace("<br />", vbCrLf)
            sContent = sContent.Replace("<br>", vbCrLf)
            sContent = HtmlUtils.FormatText(sContent, True)
            Return HtmlUtils.StripTags(sContent, True)
        End Function

    End Class

End Namespace

