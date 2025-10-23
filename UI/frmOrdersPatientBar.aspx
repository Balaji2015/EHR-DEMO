<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrdersPatientBar.aspx.cs"
    Title="Orders" Inherits="Acurus.Capella.UI.frmOrdersPatientBar" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <title>Orders Patient Bar</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1 {
            width: 188px;
        }

        .style2 {
            width: 605px;
        }

        .style3 {
            width: 439px;
        }

        #iframeOrderPatientBar {
            width: 1208px;
            height: 654px;
        }
    </style>
</head>
<body onload="OnLoad_CPOE();">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <div>
            <telerik:RadAjaxPanel ID="pnlOrdersPatientBar" runat="server">
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                </telerik:RadScriptManager>
                <telerik:RadPanelBar ID="lblPatientStrip" runat="server"  Width="100%">
                    <Items>
                        <telerik:RadPanelItem runat="server" Text="Root RadPanelItem1" Width="100%" CssClass="pnlBarGroup">
                        </telerik:RadPanelItem>
                    </Items>
                </telerik:RadPanelBar>
                <table width="100%">
                    <tr style="height: 35px !important;">
                        <td>
                            <asp:Label ID="lblSelectPhysician" runat="server" Text="Select Physician" CssClass="spanstyle"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="cboPhysician" runat="server" Width="400px" AutoPostBack="True"
                                OnSelectedIndexChanged="cboPhysician_SelectedIndexChanged" OnClientSelectedIndexChanged="StartLoadFromPatChart" CssClass="Editabletxtbox">
                            </telerik:RadComboBox>
                            &nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkShowAll" runat="server" OnCheckedChanged="chkShowAll_CheckedChanged"
                            AutoPostBack="True" />
                            <asp:Label ID="lblchkShowAll" runat="server" Text="Show All Physician" CssClass="Editabletxtbox"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadButton ID="btnFindPhysician" runat="server" Text="Find Physician" OnClientClicked="btnFindPhysician_Clicked"
                                OnClick="btnFindPhysician_Click" ButtonType="LinkButton" Visible="false" CssClass="bluebutton teleriknormalbuttonstyle" style="height: 12px !important; font-size: 13px !important;"  >
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <%--   <asp:UpdatePanel ID="m_DocFrame_UpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                            <iframe id="iframeOrderPatientBar" runat="server" scrolling="no"></iframe>
                            <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-right:30px;text-align:right">
                            <telerik:RadButton ID="btnOrder" runat="server" Text="Move To Next Process" OnClick="btnOrder_Click">
                            </telerik:RadButton>
                            &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnClose" AutoPostBack="false" runat="server" Text="Close"
                            OnClientClicking="btnClose_Clicked" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle" Font-Size="13px">
                        </telerik:RadButton>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnTransferVaraible" runat="server" />
                <asp:HiddenField ID="hdnMessageType" runat="server" />
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
                    OnClientClick="return btnClose_Clicked();" />
            </telerik:RadAjaxPanel>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSReferralOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
<%-- <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" /> Removed the button. Bug ID:25871--%>