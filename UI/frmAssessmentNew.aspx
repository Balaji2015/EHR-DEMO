<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAssessmentNew.aspx.cs" Inherits="Acurus.Capella.UI.frmAssessmentNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
     <script src="JScripts/jquery-1.9.1.js"></script>
    <script src="JScripts/jquery-1.8.3.min.js"></script>
    <script src="JScripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
