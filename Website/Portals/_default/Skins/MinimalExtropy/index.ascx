<%@ Control language="vb" CodeBehind="~/admin/Skins/skin.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="LANGUAGE" Src="~/Admin/Skins/Language.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGO" Src="~/Admin/Skins/Logo.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SEARCH" Src="~/Admin/Skins/Search.ascx" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TEXT" Src="~/Admin/Skins/Text.ascx" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LINKS" Src="~/Admin/Skins/Links.ascx" %>
<%@ Register TagPrefix="dnn" TagName="PRIVACY" Src="~/Admin/Skins/Privacy.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TERMS" Src="~/Admin/Skins/Terms.ascx" %>
<%@ Register TagPrefix="dnn" TagName="COPYRIGHT" Src="~/Admin/Skins/Copyright.ascx" %>
<%@ Register TagPrefix="dnn" TagName="STYLES" Src="~/Admin/Skins/Styles.ascx" %>

	<div>
		<div class="template_style">
		<div class="cpanel_left">
			<div class="cpanel_right">
				<div id="ControlPanel" runat="server" />
			</div>
		</div>
			<div class="top_space">
				<div class="lang_pad">
					<dnn:LANGUAGE runat="server" id="dnnLANGUAGE" showMenu="False" showLinks="True" />
				</div>
			</div>
			<div class="logo_top">
				<div class="logo_top_left">
					<div class="logo_top_right">
						<div class="logo_top_bg">
						</div>
					</div>
				</div>
				<div class="logo_left">
					<div class="logo_right">
						<div class="logo_pad">
							<dnn:LOGO runat="server" id="dnnLOGO" />
						</div>
					</div>
				</div>
			</div>
			<div class="menu_left">
				<div class="menu_right">
					<div class="menu_bg">
						<div class="search_style">
							<div class="search_bg">
								<dnn:SEARCH runat="server" id="dnnSEARCH" CssClass="ServerSkinWidget" UseDropDownList="True" submit="<img src=&quot;images/search.gif&quot; border=&quot;0&quot; alt=&quot;Search&quot; /&gt;" />
							</div>
						</div>
						<div class="menu_style">
							<dnn:NAV runat="server" id="dnnNAV" ProviderName="DNNMenuNavigationProvider" IndicateChildren="False" ControlOrientation="Horizontal" CSSNodeRoot="main_dnnmenu_rootitem" CSSNodeHoverRoot="main_dnnmenu_rootitem_hover" CSSNodeSelectedRoot="main_dnnmenu_rootitem_selected" CSSBreadCrumbRoot="main_dnnmenu_rootitem_selected" CSSContainerSub="main_dnnmenu_submenu" CSSNodeHoverSub="main_dnnmenu_itemhover" CSSNodeSelectedSub="main_dnnmenu_itemselected" CSSContainerRoot="main_dnnmenu_container" CSSControl="main_dnnmenu_bar" CSSBreak="main_dnnmenu_break" />
						</div>
						<div class="clear_float">
						</div>
					</div>
				</div>
			</div>
			<div class="bread_left">
				<div class="bread_right">
					<div class="bread_bg">
						<div id="bread_style">
							<dnn:TEXT runat="server" id="dnnTEXT" CssClass="breadcrumb_text" Text="You are here >" ResourceKey="Breadcrumb" />&nbsp;<span>
							<dnn:BREADCRUMB runat="server" id="dnnBREADCRUMB" CssClass="Breadcrumb" RootLevel="0" Separator="&nbsp;&gt;&nbsp;" />
							</span>
						</div>
						<div id="login_style" class="user">
							<dnn:USER runat="server" id="dnnUSER" CssClass="user" />&nbsp;&nbsp;|&nbsp;&nbsp;<dnn:LOGIN runat="server" id="dnnLOGIN" CssClass="user" />
						</div>
						<div class="clear_float">
						</div>
					</div>
				</div>
			</div>
			<div class="center_bg">
				<div class="left_bg">
					<div class="right_bg">
						<div class="content_pad">
							<div class="content_width">
								<div id="TopPane" class="TopPane" runat="server">
								</div>
								<table width="100%" border="0" cellspacing="0" cellpadding="0">
									<tr>
										<td valign="top" id="LeftPane" class="LeftPane" runat="server"></td>
										<td valign="top" id="ContentPane" class="ContentPane" runat="server"></td>
										<td valign="top" id="RightPane" class="RightPane" runat="server"></td>
									</tr>
								</table>
								<div id="BottomPane" class="BottomPane" runat="server"></div>
							</div>
							<div class="linkscontainer">
								<dnn:LINKS runat="server" id="dnnLINKS" CssClass="links" Level="Root" Separator="&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;" />
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="bot_left">
				<div class="bot_right">
					<div class="bot_bg">
					</div>
				</div>
			</div>
			<div class="bot_pad">
				<div id="terms_style" class="footer">
					<dnn:PRIVACY runat="server" id="dnnPRIVACY" CssClass="footer" />&nbsp;&nbsp;|&nbsp;&nbsp;<dnn:TERMS runat="server" id="dnnTERMS" CssClass="footer" />
				</div>
				<div id="copy_style" class="footer">
					<dnn:COPYRIGHT runat="server" id="dnnCOPYRIGHT" CssClass="footer" />
				</div>
				<div class="clear_float">
				</div>
			</div>
		</div>
	</div>
	<dnn:STYLES runat="server" id="dnnSTYLES" Name="IE6Minus" StyleSheet="css/ie6skin.css" Condition="LT IE 7" UseSkinPath="True" />

