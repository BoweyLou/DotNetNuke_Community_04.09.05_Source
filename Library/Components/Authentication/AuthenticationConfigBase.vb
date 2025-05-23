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
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Globalization

Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Services.Authentication

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The AuthenticationConfigBase class provides base configuration class for the 
    ''' Authentication providers
    ''' </summary>
    ''' <history>
    ''' 	[cnurse]	07/16/2007  Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class AuthenticationConfigBase

#Region "Private Members"

        Private _AuthenticationModuleID As Integer = Null.NullInteger
        Private _PortalID As Integer = Null.NullInteger
        Private _Settings As Hashtable

#End Region

#Region "Constructor(s)"

        Protected Sub New(ByVal portalID As Integer)

            _PortalID = portalID
            Dim controller As New ModuleController()
            _Settings = controller.GetModuleSettings(AuthenticationModuleID)

        End Sub

#End Region

#Region "Protected Properties"

        Protected ReadOnly Property AuthenticationModuleID() As Integer
            Get
                If _AuthenticationModuleID = Null.NullInteger Then
                    'Get the ModuleID of the Portals Authentication Module
                    Dim controller As New ModuleController()
                    Dim authModule As ModuleInfo = controller.GetModuleByDefinition(PortalID, "Authentication")
                    _AuthenticationModuleID = authModule.ModuleID
                End If
                Return _AuthenticationModuleID
            End Get
        End Property

#End Region

#Region "Public Properties"

        <Browsable(False)> _
        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(ByVal value As Integer)
                _PortalID = value
            End Set
        End Property

        <Browsable(False)> _
        Public ReadOnly Property ModuleSettings() As Hashtable
            Get
                Return _Settings
            End Get
        End Property

#End Region

    End Class

End Namespace
