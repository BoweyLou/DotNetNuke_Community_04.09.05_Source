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

Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Xml.XPath

Imports ICSharpCode.SharpZipLib.Zip

Namespace DotNetNuke.Services.Packages

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The FileInstaller installs File Components to a DotNetNuke site
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	07/24/2007  created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class FileInstaller
        Inherits ComponentInstallerBase

#Region "Private Members"

        Private _BasePath As String
        Private _Files As New List(Of InstallFile)

#End Region

#Region "Protected Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the BasePath for the files
        ''' </summary>
        ''' <remarks>The Base Path is relative to the WebRoot</remarks>
        ''' <value>A String</value>
        ''' <history>
        ''' 	[cnurse]	07/25/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property BasePath() As String
            Get
                Return _BasePath
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the name of the Collection Node ("files")
        ''' </summary>
        ''' <value>A String</value>
        ''' <history>
        ''' 	[cnurse]	08/07/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable ReadOnly Property CollectionNodeName() As String
            Get
                Return "files"
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a Dictionary of Files that are included in this component
        ''' </summary>
        ''' <value>A Dictionary(Of String, InstallFile)</value>
        ''' <history>
        ''' 	[cnurse]	07/25/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property Files() As List(Of InstallFile)
            Get
                Return _Files
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the default Path for the file - if not present in the manifest
        ''' </summary>
        ''' <value>A String</value>
        ''' <history>
        ''' 	[cnurse]	08/10/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable ReadOnly Property DefaultPath() As String
            Get
                Return Null.NullString
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the name of the Item Node ("file")
        ''' </summary>
        ''' <value>A String</value>
        ''' <history>
        ''' 	[cnurse]	08/07/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable ReadOnly Property ItemNodeName() As String
            Get
                Return "file"
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the PhysicalBasePath for the files
        ''' </summary>
        ''' <value>A String</value>
        ''' <history>
        ''' 	[cnurse]	07/25/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable ReadOnly Property PhysicalBasePath() As String
            Get
                Dim _PhysicalBasePath As String = PhysicalSitePath + "\" + BasePath
                If Not _PhysicalBasePath.EndsWith("\") Then
                    _PhysicalBasePath += "\"
                End If
                Return _PhysicalBasePath.Replace("/", "\")
            End Get
        End Property

#End Region

#Region "Protected Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The CommitFile method commits a single file.
        ''' </summary>
        ''' <param name="insFile">The InstallFile to commit</param>
        ''' <history>
        ''' 	[cnurse]	08/24/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Sub CommitFile(ByVal insFile As InstallFile)
            If insFile.Type = InstallFileType.Resources Then
                Log.AddInfo(Util.FILES_Expanding)
                Try
                    Dim objZipInputStream As New ZipInputStream(New FileStream(PhysicalBasePath + insFile.FullName, FileMode.Open, FileAccess.Read))
                    FileSystemUtils.UnzipResources(objZipInputStream, PhysicalBasePath)
                Catch ex As Exception

                End Try

                Log.AddInfo(Util.FILES_CreatedResources)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The DeleteFile method deletes a single file.
        ''' </summary>
        ''' <param name="insFile">The InstallFile to delete</param>
        ''' <history>
        ''' 	[cnurse]	08/01/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Sub DeleteFile(ByVal insFile As InstallFile)
            Util.DeleteFile(insFile, PhysicalBasePath, Log)
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a flag that determines what type of file this installer supports
        ''' </summary>
        ''' <param name="type">The type of file being processed</param>
        ''' <history>
        ''' 	[cnurse]	08/07/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Function IsCorrectType(ByVal type As InstallFileType) As Boolean
            Return True
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The InstallFile method installs a single file.
        ''' </summary>
        ''' <param name="insFile">The InstallFile to install</param>
        ''' <history>
        ''' 	[cnurse]	08/01/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Function InstallFile(ByVal insFile As InstallFile) As Boolean
            Try
                If File.Exists(PhysicalBasePath + insFile.FullName) Then
                    Util.BackupFile(insFile, PhysicalBasePath, Log)
                End If

                'Copy file from temp location
                Util.CopyFile(insFile, PhysicalBasePath, Log)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The ProcessFile method determines what to do with parsed "file" node
        ''' </summary>
        ''' <param name="file">The file represented by the node</param>
        ''' <param name="nav">The XPathNavigator representing the node</param>
        ''' <history>
        ''' 	[cnurse]	08/07/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Sub ProcessFile(ByVal file As InstallFile, ByVal nav As XPathNavigator)
            If file IsNot Nothing AndAlso IsCorrectType(file.Type) Then
                Files.Add(file)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The ReadCustomManifest method reads the custom manifest items (that subclasses
        ''' of FileInstaller may need)
        ''' </summary>
        ''' <param name="nav">The XPathNavigator representing the node</param>
        ''' <history>
        ''' 	[cnurse]	08/22/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Sub ReadCustomManifest(ByVal nav As XPathNavigator)

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The ReadManifestItem method reads a single node
        ''' </summary>
        ''' <param name="nav">The XPathNavigator representing the node</param>
        ''' <param name="checkFileExists">Flag that determines whether a check should be made</param>
        ''' <history>
        ''' 	[cnurse]	08/07/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Function ReadManifestItem(ByVal nav As XPathNavigator, ByVal checkFileExists As Boolean) As InstallFile
            Dim fileName As String = Null.NullString

            'Get the path
            Dim pathNav As XPathNavigator = nav.SelectSingleNode("path")
            If pathNav Is Nothing Then
                fileName = DefaultPath
            Else
                fileName = pathNav.Value + "\"
            End If

            'Get the name
            Dim nameNav As XPathNavigator = nav.SelectSingleNode("name")
            If nameNav IsNot Nothing Then
                fileName += nameNav.Value
            End If

            Dim file As InstallFile = New InstallFile(fileName, Package.InstallerInfo)

            If file IsNot Nothing Then
                If InstallMode = InstallMode.Install AndAlso checkFileExists Then
                    If System.IO.File.Exists(file.TempFileName) Then
                        Log.AddInfo(String.Format(Util.FILE_Found, file.Path, file.Name))
                    Else
                        Log.AddFailure(Util.FILE_NotFound + " - " + fileName)
                    End If
                End If

                'Get the Version
                file.SetVersion(XmlUtils.GetNodeValue(nav, "version"))
            End If

            Return file
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The RollbackFile method rolls back the install of a single file.
        ''' </summary>
        ''' <remarks>For new installs this removes the added file.  For upgrades it restores the
        ''' backup file created during install</remarks>
        ''' <param name="installFile">The InstallFile to commit</param>
        ''' <history>
        ''' 	[cnurse]	08/01/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Overridable Sub RollbackFile(ByVal installFile As InstallFile)
            'Check for Backups
            If File.Exists(installFile.BackupFileName) Then
                Util.RestoreFile(installFile, PhysicalBasePath, Log)
            Else
                DeleteFile(installFile)
            End If
        End Sub

#End Region

#Region "Public Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The Commit method finalises the Install and commits any pending changes.
        ''' </summary>
        ''' <remarks>In the case of Files this is not neccessary</remarks>
        ''' <history>
        ''' 	[cnurse]	08/01/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub Commit()
            Try
                For Each file As InstallFile In Files
                    CommitFile(file)
                Next
                Completed = True
            Catch ex As Exception
                Log.AddFailure(Util.EXCEPTION + " - " + ex.Message)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The Install method installs the file component
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/25/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub Install()
            Try
                Dim bSuccess As Boolean = True
                For Each file As InstallFile In Files
                    bSuccess = InstallFile(file)
                    If Not bSuccess Then
                        Exit For
                    End If
                Next
                Completed = bSuccess
            Catch ex As Exception
                Log.AddFailure(Util.EXCEPTION + " - " + ex.Message)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The ReadManifest method reads the manifest file for the file compoent.
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/25/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub ReadManifest(ByVal manifestNav As XPathNavigator)
            Dim rootNav As XPathNavigator = manifestNav.SelectSingleNode(CollectionNodeName)

            'Get the Base path
            Dim baseNav As XPathNavigator = rootNav.SelectSingleNode("basePath")
            If baseNav IsNot Nothing Then
                _BasePath = baseNav.Value
            End If

            ReadCustomManifest(rootNav)

            'Parse the file nodes
            For Each nav As XPathNavigator In rootNav.Select(ItemNodeName)
                ProcessFile(ReadManifestItem(nav, True), nav)
            Next
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The Rollback method undoes the installation of the file component in the event 
        ''' that one of the other components fails
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/31/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub Rollback()
            Try
                For Each file As InstallFile In Files
                    RollbackFile(file)
                Next
                Completed = True
            Catch ex As Exception
                Log.AddFailure(Util.EXCEPTION + " - " + ex.Message)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' The UnInstall method uninstalls the file component
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/31/2007  created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UnInstall()
            Try
                For Each file As InstallFile In Files
                    DeleteFile(file)
                Next
                Completed = True
            Catch ex As Exception
                Log.AddFailure(Util.EXCEPTION + " - " + ex.Message)
            End Try
        End Sub

#End Region

    End Class

End Namespace
