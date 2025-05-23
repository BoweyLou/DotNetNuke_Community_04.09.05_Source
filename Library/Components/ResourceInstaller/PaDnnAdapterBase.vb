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

Imports DotNetNuke.Services.Localization

Namespace DotNetNuke.Modules.Admin.ResourceInstaller

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The PaDnnAdapterBase is a base class for all PaDnnAdaptors
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	10/29/2004	Added Localization Support by adding inheritance
    '''					from ResourceInstallerBase
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class PaDnnAdapterBase
        Inherits ResourceInstallerBase

#Region "Private Members"

        Private _installerInfo As PaInstallInfo

#End Region

#Region "Constructors"

        Public Sub New(ByVal InstallerInfo As PaInstallInfo)
            _installerInfo = InstallerInfo
        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property InstallerInfo() As PaInstallInfo
            Get
                Return _installerInfo
            End Get
        End Property

#End Region

#Region "Abstract Methods"

        Public MustOverride Function ReadDnn() As PaFolderCollection

#End Region

    End Class
End Namespace