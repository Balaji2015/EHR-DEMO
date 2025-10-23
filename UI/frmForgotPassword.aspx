<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmForgotPassword.aspx.cs"
    Inherits="Acurus.Capella.UI.frmForgotPassword" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>CAPELLA 5.0</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
    <link id="Link1" runat="server" rel="shortcut icon" href="Resources/16_16.ico" type="image/x-icon" />
    <style type="text/css">
        .style1
        {
            width: 162px;
            height: 44px;
        }
        .style2
        {
            height: 44px;
        }
        .style3
        {
            height: 42px;
        }
        .style5
        {
            height: 45px;
        }
        .style6
        {
            height: 52px;
        }
        .style7
        {
            height: 28px;
        }
        .style8
        {
            width: 125px;          
        }
        .style9
        {
            width: 75px;            
        }
         .centered
        {
            border: 1px solid #ddd;
            position: absolute;
            left: 50%;
            top: 50%;
            width: 478px;
            height:358px;
            margin-left: -190px;
            margin-top: -180px;
            background-color:#BFDBFF;
            font-family: Microsoft Sans Serif;
            font-size: 8.5pt;
            Font-Size:small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close">
            </telerik:RadWindow>
        </windows>
        <windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" VisibleOnPageLoad="false" Behaviors="Close"
                Title="Change Password">
            </telerik:RadWindow>
        </windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="pnlForgotPassword" runat="server"  >
            <telerik:RadScriptManager ID="RadScriptManager1" EnablePageMethods="true" runat="server"
                AsyncPostBackTimeOut="36000">
                <scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
                </scripts>
            </telerik:RadScriptManager>
            <asp:Panel ID="Panel1" runat="server" Font-Bold="true" Height="358px" Width="478px">
                <table id="tblForgotPassword" runat="server" style="height: 330px">
                    <tr>
                        <td class="style1 Editabletxtbox">
                            <asp:Label ID="lblUserName" Font-Bold="false" Text="User Name" runat="server" EnableViewState="false" />
                        </td>
                        <td class="style2 Editabletxtbox">
                            <telerik:RadTextBox ID="txtUserName" runat="server" Height="24px" Width="300px" >
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <%--<tr id="rowDOB" runat="server">
                        <td class="style3">
                            <asp:Label ID="lblDateofBirth" Font-Bold="false" Text="Date of Birth" runat="server"
                                EnableViewState="false" />
                        </td>
                        <td class="style3">
                            <telerik:RadTextBox ID="txtBirthDate" ReadOnly="true" runat="server" Height="24px"
                                Width="250px" />
                        </td>
                    </tr>
                    <tr id="rowEMail" runat="server">
                        <td class="style7">
                            <asp:Label ID="lblEmail" Font-Bold="false" Text="E-Mail ID" runat="server" EnableViewState="false" />
                        </td>
                        <td class="style7">
                            <telerik:RadTextBox ID="txtEmailId" ReadOnly="true" runat="server" Height="24px"
                                Width="250px" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="style5 Editabletxtbox">
                            <asp:Label ID="lblSecurityQuestion1" Font-Bold="false" Text="Security Question1"
                                runat="server" EnableViewState="false" />
                        </td>
                        <td class="style5 Editabletxtbox">
                            <telerik:RadComboBox ID="cboSecurityQuestion1" runat="server" Width="300px" Height="100px" OnClientSelectedIndexChanged="EnableForgotPwd"
                                AutoPostBack="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAnswer1" Font-Bold="false" Text="Answer" runat="server" EnableViewState="false" CssClass="Editabletxtbox"/>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAnswer1" runat="server" Height="24px" Width="300px" onkeypress="EnableForgotPwd();" onchange="EnableForgotPwd();" CssClass="Editabletxtbox"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="style6 Editabletxtbox">
                            <asp:Label ID="lblSecurityQuestion2" Font-Bold="false" Text="Security Question2"
                                runat="server" EnableViewState="false"/>
                        </td>
                        <td class="style6 Editabletxtbox">
                            <telerik:RadComboBox ID="cboSecurityQuestion2" runat="server" Width="300px" Height="100px"
                                AutoPostBack="false" OnClientSelectedIndexChanged="EnableForgotPwd"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAnswer2" Text="Answer" Font-Bold="false" runat="server" EnableViewState="false" CssClass="Editabletxtbox"/>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAnswer2" runat="server" Height="24px" Width="300px" onkeypress="EnableForgotPwd();" onchange="EnableForgotPwd();" CssClass="Editabletxtbox"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <input type="button" runat="server" id="btnSave" name="btnSave" value="Save" onclick="ForgotPwdValidation();"
                                onserverclick="btnSave_Click" class="btn btn-success"/>
                            <%--<telerik:RadButton ID="btnSave" runat="server" Width="125px" OnClick="btnSave_Click" OnClientClicked="ForgotPwdValidation"/>--%>
                            <%--<telerik:RadButton ID="btnClearAll" AutoPostBack="false" runat="server" Width="75px" onclientclicked="btnClearAll_Clicked"/>--%>
                            <input type="button" runat="server" id="btnClearAll" name="btnClearAll" value="ClearAll"
                                 onclick="btnClearAll_Clicked();"
                                class="btn btn-danger" />
                        </td>

                    </tr>
                </table>
            </asp:Panel>
         <asp:HiddenField ID="hdnIsSSOLogin" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnValidation" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnScreenMode" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEmailID" runat="server" EnableViewState="false" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/JSPatientPortal.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder> </telerik:RadAjaxPanel>
    </div>
    </form>
</body>
</html>
