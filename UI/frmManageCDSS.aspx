<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmManageCDSS.aspx.cs"
    Inherits="Acurus.Capella.UI.frmManageCDSS" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage CDS</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            height: 45px;
        }
        .style2
        {
            height: 438px;
        }
        .style3
        {
            width: 108px;
        }
        .displayNone
        {
            display: none;
        }
        .modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="CDSLoad();">
    <form id="form1" runat="server" style="width: 417px; height: 539px; margin-bottom: 5px;">
    <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false"
        IconUrl="Resources/16_16.ico">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Surgical History"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <div style="width: 407px; height: 540px">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                    </asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                    </asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <table style="width: 100%; height: 100%;">
                <tr>
                    <td class="style1">
                        <asp:Panel ID="pnl" runat="server" Height="100%" Width="100%" BackColor="White" Font-Size="Small"
                            Font-Bold="false">
                            <table style="width: 100%; height: 100%;">
                                <tr>
                                    <td class="style3">
                                        <asp:Label ID="lblPhysicianName" runat="server" Text="User Name" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cboPhysicianName" runat="server" AutoPostBack="true" Style="margin-left: 0px"
                                            OnSelectedIndexChanged="cboPhysicianName_SelectedIndexChanged" Width="270px" CssClass="Editabletxtbox">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style2" valign="top">
                        <asp:Panel ID="pnlActivateDeactivate" GroupingText="Activate/Deactivate CDS Notification"
                            Width="380px" Height="460px" runat="server" Cssclass="LabelStyleBold">
                            <%-- <telerik:RadListBox ID="lstManageCDSS" runat="server" CheckBoxes="true"    
                        ShowCheckAll="false" Width="380px" Height="400px"  Font-Bold="false"
                        onclientitemchecked="lstManageCDSS_ItemChecked">
                            <ButtonSettings TransferButtons="All" />
                        </telerik:RadListBox>--%>
                            <asp:CheckBoxList ID="chklstFrequentlyUsedProcedures" runat="server" Width="380px"
                                Font-Bold="false" onclick="return Validate();" Height="400px" CssClass="Editabletxtbox">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Height="100%" Width="100%" BackColor="White"
                            Font-Size="Small" Font-Bold="false">
                            <table style="width: 409px; height: 100%;">
                                <tr style="width: 100%;">
                                    <td style="width: 60%;">
                                    </td>
                                    <td style="width: 20%;">
                                        <%-- <telerik:RadButton ID="btnAdd" runat="server" Text="Save" 
                                         Style="position: static" AccessKey="s"  Width="100%" Height="75%" OnClientClicked="btnAdd_Clicked"
                                         onclick="btnAdd_Click" >
                                                    </telerik:RadButton>--%>
                                        <input id="btnAdd" name="btnAdd" type="button" value="Save" runat="server" onclick="btnSaveClicked();"
                                            style="width: 100%; height: 100%;" onserverclick="btnAdd_Click" accesskey="s" class="aspgreenbutton"  />
                                    </td>
                                    <td style="width: 20%;">
                                        <%-- <telerik:RadButton ID="btnClose" runat="server" 
                                         Text="Close" Style="position: static" 
                                         AccessKey="c" Width="100%" Height="75%" onclick="btnClose_Click">
                                                    </telerik:RadButton>--%>
                                        <input id="btnClose" name="btnClose" type="button" value="Close" accesskey="c" runat="server"
                                            onclick="btnClose_Clicked();" style="width: 100%; height: 100%;" class="aspredbutton" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text=" " runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                    alt="Loading..." />
                <br />
            </asp:Panel>
        </div>
    </telerik:RadAjaxPanel>
    <asp:HiddenField ID="hdnLocalTime" runat="server" />
    <asp:HiddenField ID="hdnClose" runat="server" />
    <asp:HiddenField ID="hdnMessageType" runat="server" />
    <asp:Button ID="btnMessageType" runat="server" Text="Button" CssClass="displayNone" OnClientClick="return btnClose_Clicked();" />
   <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSManageCDSS.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
