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
Imports System.Configuration
Imports System.Data
Imports System.IO

Imports DotNetNuke.Entities.Modules.Actions
Imports DotNetNuke.Security.Membership
Imports DotNetNuke.Services.Mail
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage

Namespace DotNetNuke.Entities.Modules

#Region "Enums"

    Public Enum DisplayMode
        All = 0
        FirstLetter = 1
        None = 2
    End Enum

    Public Enum UsersControl
        Combo = 0
        TextBox = 1
    End Enum

#End Region


    ''' -----------------------------------------------------------------------------
    ''' Project	 :  DotNetNuke
    ''' Namespace:  DotNetNuke.Entities.Modules
    ''' Class	 :  UserModuleBase
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The UserModuleBase class defines a custom base class inherited by all
    ''' desktop portal modules within the Portal that manage Users.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    '''		[cnurse]	03/20/2006
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class UserModuleBase
        Inherits PortalModuleBase

#Region "Public Shared Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Default Settings for the Module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/02/2006
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function GetDefaultSettings() As Hashtable
            Return GetSettings(New Hashtable)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the Settings for the Module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	03/02/2006
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function GetSettings(ByVal settings As Hashtable) As Hashtable

            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings()

            If settings("Column_FirstName") Is Nothing Then
                settings("Column_FirstName") = False
            End If
            If settings("Column_LastName") Is Nothing Then
                settings("Column_LastName") = False
            End If
            If settings("Column_DisplayName") Is Nothing Then
                settings("Column_DisplayName") = True
            End If
            If settings("Column_Address") Is Nothing Then
                settings("Column_Address") = True
            End If
            If settings("Column_Telephone") Is Nothing Then
                settings("Column_Telephone") = True
            End If
            If settings("Column_Email") Is Nothing Then
                settings("Column_Email") = False
            End If
            If settings("Column_CreatedDate") Is Nothing Then
                settings("Column_CreatedDate") = True
            End If
            If settings("Column_LastLogin") Is Nothing Then
                settings("Column_LastLogin") = False
            End If
            If settings("Column_Authorized") Is Nothing Then
                settings("Column_Authorized") = True
            End If

            If settings("Display_Mode") Is Nothing Then
                settings("Display_Mode") = DisplayMode.None
            Else
                settings("Display_Mode") = CType(settings("Display_Mode"), DisplayMode)
            End If
            If settings("Display_SuppressPager") Is Nothing Then
                settings("Display_SuppressPager") = False
            End If
            If settings("Records_PerPage") Is Nothing Then
                settings("Records_PerPage") = 10
            End If

            If settings("Profile_DefaultVisibility") Is Nothing Then
                settings("Profile_DefaultVisibility") = UserVisibilityMode.AdminOnly
            Else
                settings("Profile_DefaultVisibility") = CType(settings("Profile_DefaultVisibility"), UserVisibilityMode)
            End If
            If settings("Profile_DisplayVisibility") Is Nothing Then
                settings("Profile_DisplayVisibility") = True
            End If
            If settings("Profile_ManageServices") Is Nothing Then
                settings("Profile_ManageServices") = True
            End If
            If settings("Redirect_AfterLogin") Is Nothing Then
                settings("Redirect_AfterLogin") = -1
            End If

            If settings("Redirect_AfterRegistration") Is Nothing Then
                settings("Redirect_AfterRegistration") = -1
            End If
            If settings("Redirect_AfterLogout") Is Nothing Then
                settings("Redirect_AfterLogout") = -1
            End If

            If settings("Security_CaptchaLogin") Is Nothing Then
                settings("Security_CaptchaLogin") = False
            End If

            If settings("Security_CaptchaRegister") Is Nothing Then
                settings("Security_CaptchaRegister") = False
            End If
            If settings("Security_EmailValidation") Is Nothing Then
                settings("Security_EmailValidation") = glbEmailRegEx
            End If
            'Forces a valid profile on registration
            If settings("Security_RequireValidProfile") Is Nothing Then
                settings("Security_RequireValidProfile") = False
            End If
            'Forces a valid profile on login
            If settings("Security_RequireValidProfileAtLogin") Is Nothing Then
                settings("Security_RequireValidProfileAtLogin") = True
            End If
            If settings("Security_UsersControl") Is Nothing Then
                If ((Not _portalSettings Is Nothing) AndAlso (UserController.GetUserCountByPortal(_portalSettings.PortalId) > 1000)) Then
                    settings("Security_UsersControl") = UsersControl.TextBox
                Else
                    settings("Security_UsersControl") = UsersControl.Combo
                End If
            Else
                settings("Security_UsersControl") = CType(settings("Security_UsersControl"), UsersControl)
            End If
            'Display name format
            If settings("Security_DisplayNameFormat") Is Nothing Then
                settings("Security_DisplayNameFormat") = ""
            End If

            Return settings

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets a Setting for the Module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	05/01/2006  Created
        '''     [cnurse]    02/07/2008  DNN-7003 fixed
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Function GetSetting(ByVal portalId As Integer, ByVal settingKey As String) As Object
            Dim settings As Hashtable = UserController.GetUserSettings(portalId)

            If settings Is Nothing Then
                settings = GetDefaultSettings()
                UpdateSettings(portalId, settings)
            ElseIf settings(settingKey) Is Nothing Then
                settings = GetSettings(settings)
                UpdateSettings(portalId, settings)
            End If

            Return settings(settingKey)
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Updates the Settings for the Module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	06/27/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shared Sub UpdateSettings(ByVal portalId As Integer, ByVal settings As Hashtable)

            Dim objModuleController As New ModuleController
            Dim key As String
            Dim setting As String

            'This settings page is loaded from two locations - so make sure we use the correct ModuleId
            Dim objModule As ModuleInfo = objModuleController.GetModuleByDefinition(portalId, "User Accounts")
            If Not objModule Is Nothing Then
                'Now save the values
                Dim settingsEnumerator As IDictionaryEnumerator = settings.GetEnumerator()
                While settingsEnumerator.MoveNext()
                    key = CType(settingsEnumerator.Key, String)
                    setting = CType(settingsEnumerator.Value, String)

                    objModuleController.UpdateModuleSetting(objModule.ModuleID, key, setting)
                End While
            End If

            'Clear the UserSettings Cache
            DataCache.RemoveCache(UserController.SettingsKey(portalId))

        End Sub

#End Region

#Region "Private Members"

        Private _User As UserInfo

#End Region

#Region "Protected Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether we are in Add User mode
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/06/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property AddUser() As Boolean
            Get
                Return (UserId = Null.NullInteger)
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether the current user is an Administrator (or SuperUser)
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/03/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsAdmin() As Boolean
            Get
                Return (UserInfo.IsInRole(Me.PortalSettings.AdministratorRoleName) Or UserInfo.IsSuperUser)
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether this control is in the Admin menu
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/13/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsAdminTab() As Boolean
            Get
                Return (PortalSettings.ActiveTab.ParentId = PortalSettings.AdminTabId)
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether this control is in the Host menu
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/13/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsHostTab() As Boolean
            Get
                Return (PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId)
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether the control is being called form the User Accounts module
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	07/07/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsEdit() As Boolean
            Get
                Dim _IsEdit As Boolean = False
                If Not (Request.QueryString("ctl") Is Nothing) Then
                    Dim ctl As String = Request.QueryString("ctl")
                    If ctl = "Edit" Then
                        _IsEdit = True
                    End If
                End If
                Return _IsEdit
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether the current user is modifying their profile
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsProfile() As Boolean
            Get
                Dim _IsProfile As Boolean = False
                If IsUser Then
                    If PortalSettings.UserTabId <> -1 Then
                        ' user defined tab
                        If PortalSettings.ActiveTab.TabID = PortalSettings.UserTabId Then
                            _IsProfile = True
                        End If
                    Else
                        ' admin tab
                        If Not (Request.QueryString("ctl") Is Nothing) Then
                            Dim ctl As String = Request.QueryString("ctl")
                            If ctl = "Profile" Then
                                _IsProfile = True
                            End If
                        End If
                    End If
                End If
                Return _IsProfile
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether an anonymous user is trying to register
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/21/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsRegister() As Boolean
            Get
                Dim _IsRegister As Boolean = False
                If Not IsAdmin And Not IsUser Then
                    _IsRegister = True
                End If
                Return _IsRegister
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets whether the User is editing their own information
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/03/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property IsUser() As Boolean
            Get
                Dim _IsUser As Boolean = False
                If Request.IsAuthenticated Then
                    _IsUser = (User.UserID = UserInfo.UserID)
                End If
                Return _IsUser
            End Get
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets the PortalId to use for this control
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	02/21/2007  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected ReadOnly Property UserPortalID() As Integer
            Get
                If IsHostTab Then
                    Return Null.NullInteger
                Else
                    Return PortalId
                End If
            End Get
        End Property

#End Region

#Region "Public Properties"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the User associated with this control
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/02/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Property User() As UserInfo
            Get
                If _User Is Nothing Then
                    If AddUser Then
                        _User = InitialiseUser()
                    Else
                        _User = UserController.GetUser(PortalId, UserId, False)
                    End If
                End If
                Return _User
            End Get
            Set(ByVal Value As UserInfo)
                _User = Value
                If Not _User Is Nothing Then
                    UserId = _User.UserID
                End If
            End Set
        End Property

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Gets and sets the UserId associated with this control
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/01/2006  Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Shadows Property UserId() As Integer
            Get
                Dim _UserId As Integer = Null.NullInteger
                If ViewState("UserId") Is Nothing Then
                    If Not (Request.QueryString("userid") Is Nothing) Then
                        Dim passedID As Integer = Int32.Parse(Request.QueryString("userid"))
                        Dim isAllowedUser As Boolean = False
                        'check if user is edited their own details
                        If UserInfo.UserID = passedID Then
                            isAllowedUser = True
                        ElseIf UserInfo.IsSuperUser = True Then
                            'superuser has access to all users
                            isAllowedUser = True
                        ElseIf UserInfo.IsInRole(Me.PortalSettings.AdministratorRoleName) = True Then
                            'need to check if the user is within the admins portal
                            Dim passedUser As UserInfo = DotNetNuke.Entities.Users.UserController.GetUser(PortalId, passedID, False)
                            If Not IsNothing(passedUser) Then
                                'ignore superuser's
                                If passedUser.IsSuperUser = False Then
                                    isAllowedUser = True
                                End If
                            End If
                            End If
                            If isAllowedUser = True Then
                                _UserId = Int32.Parse(Request.QueryString("userid"))
                                ViewState("UserId") = _UserId
                            Else
                                'attempt to access invalid user
                                Response.Redirect(NavigateURL("Access Denied"), True)
                            End If
                    End If
                Else
                    _UserId = CType(ViewState("UserId"), Integer)
                End If
                Return _UserId
            End Get
            Set(ByVal Value As Integer)
                ViewState("UserId") = Value
            End Set
        End Property

#End Region

#Region "Private Methods"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' InitialiseUser initialises a "new" user
        ''' </summary>
        ''' <history>
        ''' 	[cnurse]	03/13/2006
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function InitialiseUser() As UserInfo

            Dim newUser As New UserInfo

            If IsHostMenu Then
                newUser.IsSuperUser = True
            Else
                newUser.PortalID = PortalId
            End If

            ''Initialise the ProfileProperties Collection
            newUser.Profile.InitialiseProfile(PortalId)
            newUser.Profile.TimeZone = Me.PortalSettings.TimeZoneOffset

            Dim lc As String = New DotNetNuke.Services.Localization.Localization().CurrentCulture
            If String.IsNullOrEmpty(lc) Then lc = Me.PortalSettings.DefaultLanguage
            newUser.Profile.PreferredLocale = lc

            'Set default countr
            Dim country As String = Null.NullString
            country = LookupCountry()
            If country <> "" Then
                newUser.Profile.Country = country
            End If

            ''Set AffiliateId
            Dim AffiliateId As Integer = Null.NullInteger
            If Not Request.Cookies("AffiliateId") Is Nothing Then
                AffiliateId = Integer.Parse(Request.Cookies("AffiliateId").Value)
            End If
            newUser.AffiliateID = AffiliateId

            Return newUser

        End Function

        Private Function LookupCountry() As String

            Dim IP As String
            Dim IsLocal As Boolean = False
            Dim _CacheGeoIPData As Boolean = True
            Dim _GeoIPFile As String

            _GeoIPFile = "controls/CountryListBox/Data/GeoIP.dat"

            If Me.Page.Request.UserHostAddress = "127.0.0.1" Then
                'The country cannot be detected because the user is local.
                IsLocal = True
                'Set the IP address in case they didn't specify LocalhostCountryCode
                IP = Me.Page.Request.UserHostAddress
            Else
                'Set the IP address so we can find the country
                IP = Me.Page.Request.UserHostAddress
            End If

            'Check to see if we need to generate the Cache for the GeoIPData file
            If Context.Cache.Get("GeoIPData") Is Nothing And _CacheGeoIPData Then
                'Store it as	well as	setting	a dependency on	the	file
                Context.Cache.Insert("GeoIPData", DotNetNuke.UI.WebControls.CountryLookup.FileToMemory(Context.Server.MapPath(_GeoIPFile)), New System.Web.Caching.CacheDependency(Context.Server.MapPath(_GeoIPFile)))
            End If

            'Check to see if the request is a localhost request
            'and see if the LocalhostCountryCode is specified
            If IsLocal Then
                Return Null.NullString
            End If

            'Either this is a remote request or it is a local
            'request with no LocalhostCountryCode specified
            Dim _CountryLookup As DotNetNuke.UI.WebControls.CountryLookup

            'Check to see if we are using the Cached
            'version of the GeoIPData file
            If _CacheGeoIPData Then
                'Yes, get it from cache
                _CountryLookup = New DotNetNuke.UI.WebControls.CountryLookup(CType(Context.Cache.Get("GeoIPData"), MemoryStream))
            Else
                'No, get it from file
                _CountryLookup = New DotNetNuke.UI.WebControls.CountryLookup(Context.Server.MapPath(_GeoIPFile))
            End If

            'Get the country code based on the IP address
            Return _CountryLookup.LookupCountryName(IP)


        End Function

#End Region

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' AddLocalizedModuleMessage adds a localized module message
        ''' </summary>
        ''' <param name="message">The localized message</param>
        ''' <param name="type">The type of message</param>
        ''' <param name="display">A flag that determines whether the message should be displayed</param>
        ''' <history>
        ''' 	[cnurse]	03/14/2006
        ''' 	[cnurse]	07/03/2007  Moved to Base Class and changed to Protected
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub AddLocalizedModuleMessage(ByVal message As String, ByVal type As ModuleMessageType, ByVal display As Boolean)
            If display Then
                UI.Skins.Skin.AddModuleMessage(Me, message, type)
            End If
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' AddModuleMessage adds a module message
        ''' </summary>
        ''' <param name="message">The message</param>
        ''' <param name="type">The type of message</param>
        ''' <param name="display">A flag that determines whether the message should be displayed</param>
        ''' <history>
        ''' 	[cnurse]	03/14/2006
        ''' 	[cnurse]	07/03/2007  Moved to Base Class and changed to Protected
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Protected Sub AddModuleMessage(ByVal message As String, ByVal type As ModuleMessageType, ByVal display As Boolean)
            AddLocalizedModuleMessage(Localization.GetString(message, LocalResourceFile), type, display)
        End Sub

        Protected Function CompleteUserCreation(ByVal createStatus As UserCreateStatus, ByVal newUser As UserInfo, ByVal notify As Boolean, ByVal register As Boolean) As String
            Dim strMessage As String = ""

            If register Then
                ' send notification to portal administrator of new user registration
                Mail.SendMail(newUser, MessageType.UserRegistrationAdmin, PortalSettings)

                ' complete registration
                Select Case PortalSettings.UserRegistration
                    Case PortalRegistrationType.PrivateRegistration
                        Mail.SendMail(newUser, MessageType.UserRegistrationPrivate, PortalSettings)

                        'show a message that a portal administrator has to verify the user credentials
                        strMessage = String.Format(Localization.GetString("PrivateConfirmationMessage", Localization.SharedResourceFile), newUser.Email)

                    Case PortalRegistrationType.PublicRegistration
                        Mail.SendMail(newUser, MessageType.UserRegistrationPublic, PortalSettings)

                        Dim loginStatus As UserLoginStatus
                        UserController.UserLogin(PortalSettings.PortalId, newUser.Username, newUser.Membership.Password, "", PortalSettings.PortalName, "", loginStatus, False)
                    Case PortalRegistrationType.VerifiedRegistration
                        Mail.SendMail(newUser, MessageType.UserRegistrationVerified, PortalSettings)

                        'show a message that an email has been send with the registration details
                        strMessage = String.Format(Localization.GetString("VerifiedConfirmationMessage", Localization.SharedResourceFile), newUser.Email)
                End Select

                ' affiliate
                If Not Null.IsNull(User.AffiliateID) Then
                    Dim objAffiliates As New Services.Vendors.AffiliateController
                    objAffiliates.UpdateAffiliateStats(newUser.AffiliateID, 0, 1)
                End If

                'store preferredlocale in cookie
                Localization.SetLanguage(newUser.Profile.PreferredLocale)

                AddLocalizedModuleMessage(strMessage, ModuleMessageType.GreenSuccess, (strMessage.Length > 0))
            Else
                If notify Then
                    'Send Notification to User
                    If PortalSettings.UserRegistration = PortalRegistrationType.VerifiedRegistration Then
                        Mail.SendMail(newUser, MessageType.UserRegistrationVerified, PortalSettings)
                    Else
                        Mail.SendMail(newUser, MessageType.UserRegistrationPublic, PortalSettings)
                    End If
                End If
            End If

            'Log Event to Event Log
            Dim objEventLog As New Services.Log.EventLog.EventLogController
            objEventLog.AddLog(newUser, PortalSettings, UserId, newUser.Username, Services.Log.EventLog.EventLogController.EventLogType.USER_CREATED)

            Return strMessage

        End Function

    End Class

End Namespace
