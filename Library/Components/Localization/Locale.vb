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
Imports System.IO
Imports System.Web.Caching
Imports System.Threading
Imports System.Resources
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.Xml
Imports System.Xml.Serialization

Namespace DotNetNuke.Services.Localization

	''' <summary>
	''' <para>The Locale class is a custom business object that represents a locale, which is the language and country combination.</para>
	''' </summary>
	Public Class Locale
		Private _Code As String
		Private _Text As String
		Private _Fallback As String

		<XmlAttributeAttribute("Code")> _
		Public Property Code() As String
			Get
				Return _Code
			End Get
			Set(ByVal Value As String)
				_Code = Value
			End Set
		End Property

		<XmlAttributeAttribute("DisplayName")> _
		Public Property Text() As String
			Get
				Return _Text
			End Get
			Set(ByVal Value As String)
				_Text = Value
			End Set
		End Property

		<XmlAttributeAttribute("Fallback")> _
		Public Property Fallback() As String
			Get
				Return _Fallback
			End Get
			Set(ByVal Value As String)
				_Fallback = Value
			End Set
		End Property
	End Class

End Namespace
