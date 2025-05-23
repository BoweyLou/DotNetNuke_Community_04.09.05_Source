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
Imports System.Data
Imports DotNetNuke


Namespace DotNetNuke.Entities.Portals


#Region "PortalAliasInfo"
	Public Class PortalAliasInfo

#Region "Private Variables"
		Private _PortalID As Integer
		Private _PortalAliasID As Integer
		Private _HTTPAlias As String
#End Region
#Region "Public Properties"
		Public Property PortalID() As Integer
			Get
				Return _PortalID
			End Get
			Set(ByVal Value As Integer)
				_PortalID = Value
			End Set
		End Property
		Public Property PortalAliasID() As Integer
			Get
				Return _PortalAliasID
			End Get
			Set(ByVal Value As Integer)
				_PortalAliasID = Value
			End Set
		End Property
		Public Property HTTPAlias() As String
			Get
				Return _HTTPAlias
			End Get
			Set(ByVal Value As String)
				_HTTPAlias = Value
			End Set
		End Property
#End Region

	End Class
#End Region

End Namespace
