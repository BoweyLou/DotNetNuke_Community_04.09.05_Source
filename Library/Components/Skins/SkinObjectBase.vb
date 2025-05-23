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
Imports System.IO
Imports System.ComponentModel

Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security

Namespace DotNetNuke.UI.Skins

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The SkinObject class defines a custom base class inherited by all
	''' skin and container objects within the Portal.
	''' </summary>
    ''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	''' -----------------------------------------------------------------------------
    Public Class SkinObjectBase

        Inherits System.Web.UI.UserControl

#Region "Public Properties"

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public ReadOnly Property PortalSettings() As PortalSettings
            Get
                PortalSettings = PortalController.GetCurrentPortalSettings
            End Get
        End Property

        Public ReadOnly Property AdminMode() As Boolean
            Get
                Return PortalSecurity.IsInRoles(PortalSettings.AdministratorRoleName) Or PortalSecurity.IsInRoles(PortalSettings.ActiveTab.AdministratorRoles.ToString)
            End Get
        End Property

#End Region


    End Class

End Namespace
