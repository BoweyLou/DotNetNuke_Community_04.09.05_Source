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
Imports System.Configuration
Imports System.Data
Imports System.Windows.Forms

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Membership

Namespace DotNetNuke.Services.Authentication

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The AuthenticationLogoffBase class provides a base class for Authentiication 
    ''' Logoff controls
    ''' </summary>
    ''' <history>
    ''' 	[cnurse]	07/10/2007  Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class AuthenticationLogoffBase
        Inherits DotNetNuke.Entities.Modules.UserModuleBase


#Region "Public Events"

        Public Event LogOff As EventHandler
        Public Event Redirect As EventHandler

#End Region

#Region "Private Members"

        Private _AuthenticationType As String = Null.NullString
        Private _RedirectURL As String = Null.NullString

#End Region

#Region "Public Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and Sets the Type of Authentication associated with this control
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/10/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property AuthenticationType() As String
            Get
                Return _AuthenticationType
            End Get
            Set(ByVal value As String)
                _AuthenticationType = value
            End Set
        End Property

#End Region

#Region "Protected Event Methods"

        Protected Overridable Sub OnLogOff(ByVal a As EventArgs)
            RaiseEvent LogOff(Nothing, a)
        End Sub

        Protected Overridable Sub OnRedirect(ByVal a As EventArgs)
            RaiseEvent Redirect(Nothing, a)
        End Sub

#End Region


    End Class

End Namespace

