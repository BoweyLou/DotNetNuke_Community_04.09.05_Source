<%@ Control Language="vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Admin.PortalManagement.Template" CodeFile="Template.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="1" cellpadding="1" border="0">
	<tr>
		<td class="SubHead" style="vertical-align:top; white-space:nowrap; width:150px;"><dnn:label id="plPortals" text="Portal:" controlname="cboPortals" runat="server" /></td>
		<td><asp:dropdownlist id="cboPortals" runat="server" width="300px" /></td>
	</tr>
	<tr>
		<td class="SubHead" style="vertical-align:top; white-space:nowrap; width:150px;"><dnn:label id="plTemplateName" text="Template Filename:" controlname="txtTemplateName" runat="server" /></td>
		<td>
			<asp:textbox id="txtTemplateName" runat="server" width="300px" enableviewstate="False" />
			<br/>
			<asp:requiredfieldvalidator id="valFileName" runat="server" controltovalidate="txtTemplateName" display="Dynamic" resourcekey="valFileName.ErrorMessage"/>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="vertical-align:top; white-space:nowrap; width:150px;"><dnn:label id="plDescription" text="Template Description:" controlname="txtDescription" runat="server" /></td>
		<td>
			<asp:textbox id="txtDescription" runat="server" width="300px" enableviewstate="False" TextMode="MultiLine" Height="150px" />
			<br/>
			<asp:requiredfieldvalidator id="valDescription" runat="server" controltovalidate="txtDescription" display="Dynamic" resourcekey="valDescription.ErrorMessage" />
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="vertical-align:top; white-space:nowrap; width:150px;"><dnn:label id="plContent" runat="server" controlname="chkContent" /></td>
		<td><asp:CheckBox id="chkContent" runat="server" /></td>
	</tr>
</table>
<p>
	<asp:linkbutton id="cmdExport" cssclass="CommandButton" runat="server" resourcekey="cmdExport" />
</p>
<asp:label id="lblMessage" runat="server" enableviewstate="False" CssClass="Normal" />
