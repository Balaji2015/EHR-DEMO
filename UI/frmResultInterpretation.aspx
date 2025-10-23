<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmResultInterpretation.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmResultInterpretation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Interpretation Notes</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
      <style type="text/css">
        .ui-dialog-titlebar-close {
            display: none !important;
        }
      </style>
     <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>

<body onload="LoadResultInterpretation()">
    
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">

            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" DestroyOnClose="true">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <aspx:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableViewState="false" AsyncPostBackTimeout="36000">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </aspx:ToolkitScriptManager>
        <div>
            <table>
                <tr>
                    <td colspan="2">
                        <input type="text" runat="server" readonly="readonly" style="width: 840px; border: 1px solid black;" id="txtPatientInformation" class="nonEditabletxtbox" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="text" runat="server" readonly="readonly" style="width: 840px; border: 1px solid black;" id="txtFileInformation" class="nonEditabletxtbox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTemplate" runat="server" CssClass="spanstyle" Text="Template" Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTemplate" CssClass="Editabletxtbox" runat="server" AutoPostBack="true" Width="600px" EnableViewState="true" onmousedown="return ddlTemplate_Alert();" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged"  onchange="ddlTemplate_Onchange();">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtSummary" runat="server" TextMode="MultiLine" Style="width: 840px; height: 448px; resize: none"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                    </td>
                    <td style="float: right;">
                         <asp:Button ID="btnDelete" runat="server" OnClientClick="return DeleteClick();" Text="Delete" Width="58px" CssClass="aspresizedredbutton" OnClick="btnDelete_Click"  />
                        <%--<input type="button" id="btnPrintInterpretation" runat="server" value="Print" class="aspresizedbluebutton" onserverclick="btnPrintInt_ServerClick"/>--%>
                        <asp:Button ID="btnReset" runat="server" CssClass="aspresizedbluebutton" Text="Reset" Width="53px" OnClientClick="return btnResetClick();" />
                        <%--<asp:Button ID="btnPrintInt" runat="server" Text="Print" Width="60px" CssClass="aspresizedbluebutton" OnClick="btnPrint_Click" />--%>
                        <input type="button" id="btnSaveInt" runat="server" value="OK" class="aspresizedgreenbutton" onclick="return btnOkClick();"/>
                        <%--<asp:Button ID="btnSave" runat="server" Text="OK" Width="53px" CssClass="aspresizedgreenbutton" OnClientClick="return btnOkClick();" />--%>
                        <asp:Button ID="btnClose" runat="server" OnClientClick="return btnInterpretationClose_Clicked();" Text="Close" Width="53px" CssClass="aspresizedredbutton" />
                    </td>
                </tr>
            </table>
        </div>

        <asp:HiddenField ID="hdnHumanId" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPatientfirstname" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPatientlastname" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPatientmiddlename" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnDOB" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPatientType" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFileName" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
             <asp:HiddenField ID="hdnProviderNotes" runat="server" EnableViewState="false" />

        <asp:HiddenField ID="hdnPostback" runat="server" EnableViewState="false" />

            <%--<script src="JScripts/jquery-1.11.3.min.js"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js"></script>--%>
            <%--<script src="JScripts/bootstrap.min.js"></script>
            <link href="CSS/bootstrap.min3.4.0.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min.css" rel="stylesheet" />
            <link href="CSS/jquery-ui.css" rel="Stylesheet" />
            <script src="JScripts/bootstrap.min3.4.0.js" type="text/javascript"></script>
            <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
            <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>--%>
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSResultInterpretation.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
