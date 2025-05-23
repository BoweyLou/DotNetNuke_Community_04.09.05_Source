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

Namespace DotNetNuke.Modules.Admin.ResourceInstaller
	Public Class PaInstallInfo
		Private _Log As New PaLogger
		Private _Path As String = ""
		Private _FileTable As New Hashtable
		Private _dnn As PaFile

		Public Property Log() As PaLogger
			Get
				Return _Log
			End Get
			Set(ByVal Value As PaLogger)
				_Log = Value
			End Set
		End Property

		Public Property SitePath() As String
			Get
				Return _Path
			End Get
			Set(ByVal Value As String)
				_Path = Value
			End Set
		End Property

		Public Property FileTable() As Hashtable
			Get
				Return _FileTable
			End Get
			Set(ByVal Value As Hashtable)
				_FileTable = Value
			End Set
		End Property

		Public Property DnnFile() As PaFile
			Get
				Return _dnn
			End Get
			Set(ByVal Value As PaFile)
				_dnn = Value
			End Set
		End Property

	End Class
End Namespace
