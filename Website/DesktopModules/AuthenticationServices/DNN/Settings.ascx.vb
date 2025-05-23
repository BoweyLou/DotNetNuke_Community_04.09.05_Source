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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Authentication
Imports DotNetNuke.UI.WebControls

Namespace DotNetNuke.Modules.Admin.Authentication

    ''' -----------------------------------------------------------------------------
    ''' Project	 : DotNetNuke
    ''' Class	 : Authentication
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Manages the core DNN Authentication settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''     [cnurse]        06/29/2007   Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits DotNetNuke.Services.Authentication.AuthenticationSettingsBase


        Public Overrides Sub UpdateSettings()
            If SettingsEditor.IsValid AndAlso SettingsEditor.IsDirty Then
                Dim config As AuthenticationConfig = CType(SettingsEditor.DataSource, AuthenticationConfig)
                AuthenticationConfig.UpdateConfig(config)
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                Dim config As AuthenticationConfig = AuthenticationConfig.GetConfig(PortalId)

                SettingsEditor.DataSource = config
                SettingsEditor.DataBind()

            Catch exc As Exception
                Exceptions.ProcessModuleLoadException(Me, exc)
            End Try

        End Sub
    End Class

End Namespace

