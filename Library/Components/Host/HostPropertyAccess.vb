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

Imports DotNetNuke.Services.Tokens

Namespace DotNetNuke.Entities.Host
    Public Class HostPropertyAccess
        Inherits DictionaryPropertyAccess
        Public Sub New()
            MyBase.New(HostSettings.GetSecureHostSettings)
        End Sub

        Public Overrides Function GetProperty(ByVal strPropertyName As String, ByVal strFormat As String, ByVal formatProvider As System.Globalization.CultureInfo, ByVal AccessingUser As Users.UserInfo, ByVal CurrentScope As Services.Tokens.Scope, ByRef PropertyNotFound As Boolean) As String

            If strPropertyName.ToLower = "hosttitle" OrElse CurrentScope = Scope.Debug Then
                Return MyBase.GetProperty(strPropertyName, strFormat, formatProvider, AccessingUser, CurrentScope, PropertyNotFound)
            Else
                PropertyNotFound = True : Return PropertyAccess.ContentLocked
            End If

        End Function

    End Class
End Namespace
