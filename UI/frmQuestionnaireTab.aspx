<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmQuestionnaireTab.aspx.cs"
    Inherits="Acurus.Capella.UI.frmQuestionnaireTab" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Questionnaire</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
<link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <telerik:RadScriptManager EnableViewState="false" ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadTabStrip ID="RadTabStrip2" runat="server" MultiPageID="RadMultiPage1"
            SelectedIndex="0" Width="100%" ScrollChildren="True" OnTabClick="RadTabStrip1_TabClick"
            OnClientTabSelecting="RadTabStrip2_TabSelecting">
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Width="100%" SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server">
            </telerik:RadPageView>
        </telerik:RadMultiPage>
        <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Questionnaire"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    </div>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="JScripts/JSHealthQuestionnaire.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
    </form>
</body>
</html>
