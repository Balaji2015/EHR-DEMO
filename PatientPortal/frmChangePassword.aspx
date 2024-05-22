<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmChangePassword.aspx.cs"
    Inherits="Acurus.Capella.PatientPortal.frmChangePassword" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">


<head runat="server">
    <title>CAPELLA 5.0</title>
    <link id="Link1" runat="server" rel="shortcut icon" href="Resources/16_16.ico" type="image/x-icon" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        .Panel {
            background-color: White;
        }

            .Panel legend {
                font-weight: bold;
            }
           
    </style>

</head>
<body onload="loadchangepassword();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server">
        <div id="centered">
            <telerik:RadWindowManager ID="WindowMngr" runat="server">
                <Windows>
                    <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close">
                    </telerik:RadWindow>
                </Windows>
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" VisibleOnPageLoad="false" Behaviors="Close"
                        Title="Change Password">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <asp:Panel ID="pnlChangePassword" runat="server" GroupingText="Change Password"
                BackColor="#bfdbff" CssClass="Editabletxtbox" Font-Size="Small">
                <asp:UpdatePanel ID="updtpnl" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server"  Text="User Name"  mand="Yes"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtUserName" Width="100%" runat="server" ReadOnly="true"
                                       CssClass="nonEditabletxtbox">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <asp:Panel ID="pnlOldPassword" runat="server" >
                                    <td>
                                        <asp:Label ID="lblOldPassword" runat="server"  Text="Old Password*"  mand="Yes"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtOldPassword" runat="server" Width="100%" MaxLength="50" TextMode="Password" onchange="tochangeValues" ClientEvents-OnBlur="deleting" CssClass="Editabletxtbox">
                                        </telerik:RadTextBox>
                                    </td>
                                </asp:Panel>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNewPassword" runat="server"  Text="New Password*" mand="Yes"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtNewPassword" runat="server"
                                        Width="100%" onkeypress="SpacebarValidation(this);" MaxLength="50"
                                        TextMode="Password" onchange="tochangeValues" ClientEvents-OnBlur="deleting" >
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password*"  mand="Yes"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtConfirmPassword" runat="server"
                                        Width="100%" onkeypress="SpacebarValidation(this);" MaxLength="50"
                                        TextMode="Password" onchange="tochangeValues" ClientEvents-OnBlur="deleting" CssClass="Editabletxtbox">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="right" style="height: 40px;">
                                    <input type="button" runat="server" id="btnOk" name="btnOk" style="color: white;" value="Ok" width="60px" onserverclick="btnOk_Click" onclick="changeValues();" class="aspbluebutton"/>
                                    &nbsp;&nbsp;
                                    <input type="button" runat="server" id="btnClose" name="btnClose" width="80px" style="color: white;" value="Close" onserverclick="btnClose_Click" class="aspredbutton"/>&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnOld" runat="server" />
                        <asp:HiddenField ID="hdnNew" runat="server" />
                        <asp:HiddenField ID="hdnConfirm" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:HiddenField ID="IsLoginOpen" runat="server" />
            <asp:HiddenField ID="hdnToFindPostback" runat="server" />
            <asp:HiddenField ID="hdnScreenMode" runat="server" />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
                <%--<script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>--%>
                <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
                <script src="JScripts/JSPatientPortal.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                    type="text/javascript"></script>
                <script src="JScripts/JSMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                    type="text/javascript"></script>
                <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                    type="text/javascript"></script>
               
                <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
                    type="text/javascript"></script>
            </asp:PlaceHolder>
        </div>
    </form>
</body>
</html>
