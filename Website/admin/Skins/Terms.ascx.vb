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

Namespace DotNetNuke.UI.Skins.Controls
    ''' -----------------------------------------------------------------------------
    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 	[cniknet]	10/15/2004	Replaced public members with properties and removed
    '''                             brackets from property names
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Partial  Class Terms

        Inherits UI.Skins.SkinObjectBase

        ' private members
        Private _text As String
        Private _cssClass As String

        Const MyFileName As String = "Terms.ascx"
        ' protected controls

#Region "Public Members"
        Public Property Text() As String
            Get
                Return _text
            End Get
            Set(ByVal Value As String)
                _text = Value
            End Set
        End Property

        Public Property CssClass() As String
            Get
                Return _cssClass
            End Get
            Set(ByVal Value As String)
                _cssClass = Value
            End Set
        End Property
#End Region

#Region " Web Form Designer Generated Code "


        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        '*******************************************************
        '
        ' The Page_Load server event handler on this page is used
        ' to populate the role information for the page
        '
        '*******************************************************

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                ' public attributes
                If CssClass <> "" Then
                    hypTerms.CssClass = CssClass
                End If

                If Text <> "" Then
                    hypTerms.Text = Text
                Else
                    hypTerms.Text = Services.Localization.Localization.GetString("Terms", Services.Localization.Localization.GetResourceFile(Me, MyFileName))
                End If

                If PortalSettings.HomeTabId <> -1 Then
                    hypTerms.NavigateUrl = NavigateURL(PortalSettings.HomeTabId, "Terms")
                Else
                    hypTerms.NavigateUrl = NavigateURL("Terms")
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

    End Class

End Namespace
