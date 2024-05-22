<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSpecialityDiagnosis.aspx.cs"
    Inherits="Acurus.Capella.UI.frmSpecialityDiagnosis" EnableEventValidation="false"%>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>All Diagnosis</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .invisibleDisplay {
            display: none;
        }
        
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body  onload="OnSpecialityLoad();">
    <form id="form1" runat="server" defaultbutton="btnSearch">
        <telerik:RadWindowManager ID="WindowMngr1" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Assessment"
                    IconUrl="Resources/16_16.ico" KeepInScreenBounds="False">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                <asp:Panel ID="pnlMain" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlSearch" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblDescription" class="Editabletxtbox" runat="server"  Text="Enter Description"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="Editabletxtbox" Width="350px">
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ClientEvents OnKeyPress="txtDescription_OnKeyPress" />
                                                    <FocusedStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblICDCode" class="Editabletxtbox" runat="server"  Text="Enter ICD Code"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtICDCode" runat="server" CssClass="Editabletxtbox" Width="350px" MaxLength="10">
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadButton ID="btnSearch" runat="server" Text="Search" ButtonType="LinkButton" CssClass="bluebutton"
                                                    OnClick="btnSearch_Click" OnClientClicked="btnSearch_ClientClicked">
                                                </telerik:RadButton>
                                            </td>
                                            <td>
                                                <telerik:RadButton ID="btnClearAll" runat="server" ButtonType="LinkButton" CssClass="redbutton" Text="Clear All" OnClick="btnClearAll_Click">
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblError1" class="Editabletxtbox" runat="server"  ></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pntTree" runat="server" Width="100%"  CssClass="classpntTree LabelStyleBold"
                                    GroupingText="All" Style="overflow: auto;" >
                                    <telerik:RadTreeView ID="trvCodeLibrary" runat="server" CheckBoxes="True" BackColor="White" 
                                        OnNodeCheck="trvCodeLibrary_NodeCheck" OnClientNodeClicking="trvCodeLibrary_NodeClickingClient" OnClientNodeChecking="trvCodeLibrary_NodeCheckingClient" CssClass="trvHeight chkitems">
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlButton" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnInvisible" runat="server" Text="InVisible" OnClick="btnInvisible_Click"
                                                    CssClass="invisibleDisplay" />
                                            </td>
                                            <td align="right">
                                                <telerik:RadButton ID="btnMoveToSelectedAssessment" ButtonType="LinkButton" CssClass="greenbutton" runat="server" OnClick="btnMoveToSelectedAssessment_Click"
                                                    Text="Move To Selected Assessment"
                                                    OnClientClicked="btnMoveToSelectedAssessment_Clicked" style="font-size: 13px !important;" >
                                                </telerik:RadButton>
                                            </td>
                                            <td style="width: 60px">
                                                <telerik:RadButton ID="btnQuit" runat="server" Text="Close" ButtonType="LinkButton" CssClass ="redbutton" AutoPostBack="true" OnClick="btnQuit_Click"
                                                    OnClientClicking="btnQuit_Clicked"  style="font-size: 13px !important; " >
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="finalProblemList" runat="server" />
            <asp:HiddenField ID="hdnEandMCode" runat="server" />
            <asp:HiddenField ID="hdnEandMIcd" runat="server" />
            <asp:HiddenField ID="hdnMessageType" runat="server" />
            <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="btnQuit_Clicked();" />
            <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                <asp:Panel ID="Panel4" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <br />
                        <img src="Resources/wait.ico" title="[Please wait while the page is searching...]"
                            alt="searching..." />
                        <br />
                </asp:Panel>
            </div>
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSSpecialityDiagnosis.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
