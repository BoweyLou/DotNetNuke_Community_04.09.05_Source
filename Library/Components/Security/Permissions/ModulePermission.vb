'
' DotNetNuke� - http://www.dotnetnuke.com
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
Imports System.Xml.Serialization

Namespace DotNetNuke.Security.Permissions

#Region "ModulePermissionInfo"
    Public Class ModulePermissionInfo
        Inherits PermissionInfo

        ' local property declarations
        Dim _modulePermissionID As Integer
        Dim _moduleID As Integer
        Dim _roleID As Integer
        Dim _AllowAccess As Boolean
        Dim _RoleName As String
        Dim _userID As Integer
        Dim _Username As String
        Dim _DisplayName As String


        ' initialization
        Public Sub New()
            MyBase.New()
            _modulePermissionID = Null.NullInteger
            _moduleID = Null.NullInteger
            _roleID = Integer.Parse(glbRoleNothing)
            _AllowAccess = False
            _RoleName = Null.NullString
            _userID = Null.NullInteger
            _Username = Null.NullString
            _DisplayName = Null.NullString
        End Sub 'New

        Public Sub New(ByVal permission As PermissionInfo)
            Me.New()

            Me.ModuleDefID = permission.ModuleDefID
            Me.PermissionCode = permission.PermissionCode
            Me.PermissionID = permission.PermissionID
            Me.PermissionKey = permission.PermissionKey
            Me.PermissionName = permission.PermissionName
        End Sub

#Region "Public Properties"
        <XmlElement("modulepermissionid")> Public Property ModulePermissionID() As Integer
            Get
                Return _modulePermissionID
            End Get
            Set(ByVal Value As Integer)
                _modulePermissionID = Value
            End Set
        End Property

        <XmlElement("moduleid")> Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        <XmlElement("roleid")> Public Property RoleID() As Integer
            Get
                Return _roleID
            End Get
            Set(ByVal Value As Integer)
                _roleID = Value
            End Set
        End Property

        <XmlElement("rolename")> Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal Value As String)
                _RoleName = Value
            End Set
        End Property

        <XmlElement("allowaccess")> Public Property AllowAccess() As Boolean
            Get
                Return _AllowAccess
            End Get
            Set(ByVal Value As Boolean)
                _AllowAccess = Value
            End Set
        End Property

        <XmlElement("userid")> Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        <XmlElement("username")> Public Property Username() As String
            Get
                Return _Username
            End Get
            Set(ByVal Value As String)
                _Username = Value
            End Set
        End Property

        <XmlElement("displayname")> Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal Value As String)
                _DisplayName = Value
            End Set
        End Property

#End Region

#Region "Public Methods"
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Compares if two ModulePermissionInfo objects are equivalent/equal
        ''' </summary>
        ''' <param name="obj">a ModulePermissionObject</param>
        ''' <returns>true if the permissions being passed represents the same permission
        ''' in the current object
        ''' </returns>
        ''' <remarks>
        ''' This function is needed to prevent adding duplicates to the ModulePermissionCollection.
        ''' ModulePermissionCollection.Contains will use this method to check if a given permission
        ''' is already included in the collection.
        ''' </remarks>
        ''' <history>
        ''' 	[Vicen�]	09/01/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj Is Nothing Or Not Me.GetType() Is obj.GetType() Then
                Return False
            End If
            Dim perm As ModulePermissionInfo = CType(obj, ModulePermissionInfo)
            Return (Me.AllowAccess = perm.AllowAccess) And (Me.ModuleID = perm.ModuleID) And _
                (Me.RoleID = perm.RoleID) And (Me.PermissionID = perm.PermissionID)
        End Function
#End Region

    End Class
#End Region

End Namespace
