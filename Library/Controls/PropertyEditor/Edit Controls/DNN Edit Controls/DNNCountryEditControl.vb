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

Imports DotNetNuke.Common.Lists
Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.UI.WebControls

    ''' -----------------------------------------------------------------------------
    ''' Project:    DotNetNuke
    ''' Namespace:  DotNetNuke.UI.WebControls
    ''' Class:      DNNCountryEditControl
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The DNNCountryEditControl control provides a standard UI component for editing
    ''' Countries
    ''' </summary>
    ''' <history>
    '''     [cnurse]	05/03/2006	created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    <ToolboxData("<{0}:DNNCountryEditControl runat=server></{0}:DNNCountryEditControl>")> _
    Public Class DNNCountryEditControl
        Inherits DNNListEditControl

#Region "Constructors"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Constructs a DNNCountryEditControl
        ''' </summary>
        ''' <history>
        '''     [cnurse]	05/03/2006	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Sub New()
            AutoPostBack = True
            ListName = "Country"
            ParentKey = ""
            TextField = ListBoundField.Text
            ValueField = ListBoundField.Text
        End Sub

#End Region

    End Class

End Namespace

