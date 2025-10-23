<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmMailMessage.aspx.cs" Inherits="Acurus.Capella.UI.frmMailMessage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mail Message</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .btnAlignment {
            float: right;
            width: 103px;
        }

        .Forward {
            width: 103px;
        }

        .Labellink {
            color: blue;
            text-decoration: underline;
            cursor: pointer;
        }
    </style>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 420px; overflow: auto">
            <table>
                <tr>
                    <td>
                        <telerik:RadButton ID="btnReply" runat="server" Text="Reply" CssClass="btnAlignment bluebutton" OnClientClicked="Reply"></telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnForward" runat="server" Text="Forward" CssClass="Forward bluebutton" OnClientClicked="Forward"></telerik:RadButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%-- <telerik:RadTextBox ID="txtMessage" runat="server" Width="601px" Height="407px" 
            TextMode="MultiLine" BorderColor="White">--%>

                        <asp:Label ID="lblcontent" runat="server" Class="Editabletxtbox" Width="601px" Height="407px"
                            TextMode="MultiLine" BorderColor="White"></asp:Label>
                        <disabledstyle resize="None"></disabledstyle>

                        <invalidstyle resize="None"></invalidstyle>

                        <hoveredstyle resize="None"></hoveredstyle>

                        <readonlystyle resize="None"></readonlystyle>

                        <emptymessagestyle resize="None"></emptymessagestyle>

                        <focusedstyle resize="None"></focusedstyle>

                        <enabledstyle resize="None"></enabledstyle>
                        </telerik:RadTextBox></td>
                </tr>
            </table>

            <asp:Button ID="btndownload" runat="server" OnClick="btndownload_Click" Style="display: none" />
            <asp:HiddenField ID="hdnpath" runat="server" />
            <asp:HiddenField ID="hdnmailPath" runat="server" />
            <telerik:RadScriptManager ID="RadScriptManager1" EnableViewState="false" runat="server">
            </telerik:RadScriptManager>
        </div>

        <asp:Panel ID="pnlAttachment" runat="server" Width="100%" CssClass="Editabletxtbox" GroupingText="Attachments">
            <div id="dvattachment" runat="server" style="height: 180px; overflow: auto">
            </div>
        </asp:Panel>
        <asp:HiddenField ID="hdnPatientID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEmailID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnBodyMessage" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnRole" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsPatientPortal" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSMailBox.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
