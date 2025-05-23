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
Imports System.Web
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Collections.Specialized
Imports System.Security.Cryptography

Namespace DotNetNuke.UI.WebControls

    ''' -----------------------------------------------------------------------------
    ''' Project:    DotNetNuke
    ''' Namespace:  DotNetNuke.UI.WebControls
    ''' Class:      CaptchaHandler
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The CaptchaHandler control provides a validator to validate a CAPTCHA Challenge
    ''' </summary>
    ''' <history>
    '''     [cnurse]	03/16/2006	created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class CaptchaHandler
        Implements IHttpHandler

        Private Const MAX_IMAGE_WIDTH As Integer = 600
        Private Const MAX_IMAGE_HEIGHT As Integer = 600

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest

            Dim queryString As NameValueCollection = context.Request.QueryString
            Dim text As String = queryString(CaptchaControl.KEY)
            Dim response As HttpResponse = context.Response
            Dim bmp As Bitmap = CaptchaControl.GenerateImage(text)
            If bmp IsNot Nothing Then
                bmp.Save(response.OutputStream, ImageFormat.Jpeg)
            End If

        End Sub

    End Class

End Namespace

