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

Imports System.Web
Imports System.Text.RegularExpressions

Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotnetNuke.Entities.Portals
Imports DotNetNuke.Services.Url.FriendlyUrl
Imports DotNetNuke.Framework.Providers
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.HttpModules

Namespace DotNetNuke.Services.Url.FriendlyUrl

    Public Class DNNFriendlyUrlProvider

        Inherits FriendlyUrlProvider

#Region " Constants "

        Private Const ProviderType As String = "friendlyUrl"
        Private Const RegexMatchExpression As String = "[^a-zA-Z0-9 ]"

#End Region

#Region " Private Members "

        Private _providerConfiguration As ProviderConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _includePageName As Boolean
        Private _regexMatch As String
        Private _fileExtension As String
        Private _urlFormat As UrlFormatType = UrlFormatType.SearchFriendly

#End Region

#Region " Constructors "

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Provider)

            ' Read the attributes for this provider

            If Convert.ToString(objProvider.Attributes("includePageName")) <> "" Then
                _includePageName = Boolean.Parse(objProvider.Attributes("includePageName"))
            Else
                _includePageName = True
            End If

            If Convert.ToString(objProvider.Attributes("regexMatch")) <> "" Then
                _regexMatch = objProvider.Attributes("regexMatch")
            Else
                _regexMatch = RegexMatchExpression
            End If

            If Convert.ToString(objProvider.Attributes("fileExtension")) <> "" Then
                _fileExtension = objProvider.Attributes("fileExtension")
            Else
                _fileExtension = ".aspx"
            End If

            If Convert.ToString(objProvider.Attributes("urlFormat")) <> "" Then
                Select Case objProvider.Attributes("urlFormat").ToLower()
                    Case "searchfriendly"
                        _urlFormat = UrlFormatType.SearchFriendly

                    Case "humanfriendly"
                        _urlFormat = UrlFormatType.HumanFriendly

                    Case Else
                        _urlFormat = UrlFormatType.SearchFriendly
                End Select
            End If

        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property FileExtension() As String
            Get
                Return _fileExtension
            End Get
        End Property

        Public ReadOnly Property IncludePageName() As Boolean
            Get
                Return _includePageName
            End Get
        End Property

        Public ReadOnly Property RegexMatch() As String
            Get
                Return _regexMatch
            End Get
        End Property

        Public ReadOnly Property UrlFormat() As UrlFormatType
            Get
                Return _urlFormat
            End Get
        End Property

#End Region

#Region " Public Methods "

        Public Overloads Overrides Function FriendlyUrl(ByVal tab As TabInfo, ByVal path As String) As String
            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
            Return FriendlyUrl(tab, path, glbDefaultPage, _portalSettings)
        End Function

        Public Overloads Overrides Function FriendlyUrl(ByVal tab As TabInfo, ByVal path As String, ByVal pageName As String) As String
            Dim _portalSettings As PortalSettings = PortalController.GetCurrentPortalSettings
            Return FriendlyUrl(tab, path, pageName, _portalSettings)
        End Function

        Public Overloads Overrides Function FriendlyUrl(ByVal tab As TabInfo, ByVal path As String, ByVal pageName As String, ByVal settings As PortalSettings) As String

            Return FriendlyUrl(tab, path, pageName, settings.PortalAlias.HTTPAlias)

        End Function

        Public Overloads Overrides Function FriendlyUrl(ByVal tab As TabInfo, ByVal path As String, ByVal pageName As String, ByVal portalAlias As String) As String

            Dim friendlyPath As String = path
            Dim matchString As String = ""

            'Call GetFriendlyAlias to get the Alias part of the url
            friendlyPath = GetFriendlyAlias(path, portalAlias)
            'Call GetFriendlyQueryString to get the QueryString part of the url
            friendlyPath = GetFriendlyQueryString(tab, friendlyPath, pageName)

            If (UrlFormat = UrlFormatType.HumanFriendly) Then
                If (Regex.IsMatch(friendlyPath, "[^?]*/Tab[Ii]d/(\d+)/default.aspx$", RegexOptions.IgnoreCase)) Then
                    If Not (tab Is Nothing) Then
                        Return AddHTTP(portalAlias & "/" & tab.TabPath.Replace("//", "/").TrimStart(Convert.ToChar("/")) + ".aspx")
                    End If
                Else
                    Dim re As Regex = New Regex("[^?]*/Tab[Ii]d/(\d+)/ctl/([A-Z][a-z]+)/default.aspx(\?returnurl=([^>]+))?$", RegexOptions.IgnoreCase)
                    If (re.IsMatch(friendlyPath)) Then
                        Dim sesMatch As Match = re.Match(friendlyPath)

                        If (sesMatch.Groups.Count > 2) Then
                            Select Case sesMatch.Groups(2).Value.ToLower
                                Case "terms"
                                    Return AddHTTP(portalAlias & "/" & sesMatch.Groups(2).Value & ".aspx")
                                Case "privacy"
                                    Return AddHTTP(portalAlias & "/" & sesMatch.Groups(2).Value & ".aspx")
                                Case "login"
                                    If (sesMatch.Groups(4).Value.ToLower() <> "") Then
                                        Return AddHTTP(portalAlias & "/" & sesMatch.Groups(2).Value & ".aspx?ReturnUrl=" & sesMatch.Groups(4).Value)
                                    Else
                                        Return AddHTTP(portalAlias & "/" & sesMatch.Groups(2).Value & ".aspx")
                                    End If
                                Case "register"
                                    If (sesMatch.Groups(4).Value.ToLower() <> "") Then
                                        Return AddHTTP(portalAlias & "/" & sesMatch.Groups(2).Value & ".aspx?ReturnUrl=" & sesMatch.Groups(4).Value)
                                    Else
                                        Return AddHTTP(portalAlias & "/" & sesMatch.Groups(2).Value & ".aspx")
                                    End If
                                Case Else
                                    Return friendlyPath
                            End Select
                        Else
                            Return friendlyPath
                        End If
                    Else
                        Return friendlyPath
                    End If
                End If
            End If

            friendlyPath = CheckPathLength(friendlyPath, path)

            Return friendlyPath

        End Function
#End Region

#Region " Private Methods "


        Private Function CheckPathLength(ByVal friendlyPath As String, ByVal originalpath As String) As String

            If friendlyPath.Length >= 260 Then '248 + "/default.aspx"
                Return ResolveUrl(originalpath)
            Else
                Return friendlyPath
            End If

        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' AddPage adds the page to the friendly url
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="path">The path to format.</param>
        ''' <param name="pageName">The page name.</param>
        ''' <returns>The formatted url</returns>
        ''' <history>
        '''		[cnurse]	12/16/2004	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function AddPage(ByVal path As String, ByVal pageName As String) As String
            Dim friendlyPath As String = path

            If (friendlyPath.EndsWith("/")) Then
                friendlyPath = friendlyPath & pageName
            Else
                friendlyPath = friendlyPath & "/" & pageName
            End If

            Return friendlyPath
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetFriendlyAlias gets the Alias root of the friendly url
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="path">The path to format.</param>
        ''' <param name="portalAlias">The portal alias of the site.</param>
        ''' <returns>The formatted url</returns>
        ''' <history>
        '''		[cnurse]	12/16/2004	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetFriendlyAlias(ByVal path As String, ByVal portalAlias As String) As String
            Dim friendlyPath As String = path
            Dim matchString As String = ""

            If Not (portalAlias = Null.NullString) Then
                If Not (HttpContext.Current.Items("UrlRewrite:OriginalUrl") Is Nothing) Then
                    Dim originalUrl As String = HttpContext.Current.Items("UrlRewrite:OriginalUrl").ToString()

                    'For Each entry As String In arrAlias
                    Dim portalMatch As Match = Regex.Match(originalUrl, "^" & AddHTTP(portalAlias), RegexOptions.IgnoreCase)
                    If Not (portalMatch Is Match.Empty) Then
                        matchString = AddHTTP(portalAlias)
                    End If

                    If (matchString = "") Then
                        'Manage the special case where original url contains the alias as
                        'http://www.domain.com/Default.aspx?alias=www.domain.com/child"
                        portalMatch = Regex.Match(originalUrl, "^?alias=" & portalAlias, RegexOptions.IgnoreCase)
                        If Not (portalMatch Is Match.Empty) Then
                            matchString = AddHTTP(portalAlias)
                        End If
                    End If
                End If
            End If

            If (matchString <> "") Then
                If (path.IndexOf("~") <> -1) Then
                    friendlyPath = friendlyPath.Replace("~", matchString)
                Else
                    friendlyPath = matchString & friendlyPath
                End If
            Else
                friendlyPath = ResolveUrl(friendlyPath)
            End If

            Return friendlyPath
        End Function

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' GetFriendlyQueryString gets the Querystring part of the friendly url
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <param name="tab">The tab whose url is being formatted.</param>
        ''' <param name="path">The path to format.</param>
        ''' <returns>The formatted url</returns>
        ''' <history>
        '''		[cnurse]	12/16/2004	created
        '''		[smcculloch]10/10/2005	Regex update for rewritten characters
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Function GetFriendlyQueryString(ByVal tab As TabInfo, ByVal path As String, ByVal pageName As String) As String
            Dim friendlyPath As String = path
            Dim queryStringMatch As Match = Regex.Match(friendlyPath, "(.[^\\?]*)\\?(.*)", RegexOptions.IgnoreCase)
            Dim queryStringSpecialChars As String = ""

            If Not (queryStringMatch Is Match.Empty) Then
                friendlyPath = queryStringMatch.Groups(1).Value
                friendlyPath = Regex.Replace(friendlyPath, glbDefaultPage, "", RegexOptions.IgnoreCase)

                Dim queryString As String = queryStringMatch.Groups(2).Value.Replace("&amp;", "&")
                If (queryString.StartsWith("?")) Then
                    queryString = queryString.TrimStart(Convert.ToChar("?"))
                End If

                Dim nameValuePairs As String() = queryString.Split(Convert.ToChar("&"))
                For i As Integer = 0 To nameValuePairs.Length - 1
                    Dim pathToAppend As String = ""
                    Dim pair As String() = nameValuePairs(i).Split(Convert.ToChar("="))

                    'Add name part of name/value pair
                    If (friendlyPath.EndsWith("/")) Then
                        pathToAppend = pathToAppend & pair(0)
                    Else
                        pathToAppend = pathToAppend & "/" & pair(0)
                    End If

                    If (pair.Length > 1) Then
                        If (pair(1).Length > 0) Then

                            If (Regex.IsMatch(pair(1), _regexMatch) = False) Then

                                ' Contains Non-AlphaNumeric Characters
                                If (pair(0).ToLower() = "tabid") Then
                                    If (IsNumeric(pair(1))) Then
                                        If Not (tab Is Nothing) Then
                                            Dim tabId As Integer = Convert.ToInt32(pair(1))
                                            If (tab.TabID = tabId) Then
                                                If (tab.TabPath <> Null.NullString) And IncludePageName Then
                                                    pathToAppend = tab.TabPath.Replace("//", "/").TrimStart("/"c) & "/" & pathToAppend
                                                End If
                                            End If
                                        End If
                                    End If
                                End If

                                pathToAppend = pathToAppend & "/" & System.Web.HttpUtility.UrlPathEncode(pair(1))

                            Else

                                ' Rewrite into URL, contains only alphanumeric and the % or space
                                If (queryStringSpecialChars.Length = 0) Then
                                    queryStringSpecialChars = pair(0) & "=" & pair(1)
                                Else
                                    queryStringSpecialChars = queryStringSpecialChars & "&" & pair(0) & "=" & pair(1)
                                End If

                                pathToAppend = ""

                            End If
                        Else
                            pathToAppend = pathToAppend & "/" & System.Web.HttpUtility.UrlPathEncode((Chr(32)).ToString())
                        End If
                    End If

                    friendlyPath = friendlyPath & pathToAppend
                Next
            End If

            If (queryStringSpecialChars.Length > 0) Then
                Return AddPage(friendlyPath, pageName) & "?" & queryStringSpecialChars
            Else
                Return AddPage(friendlyPath, pageName)
            End If
        End Function

#End Region

    End Class

End Namespace

