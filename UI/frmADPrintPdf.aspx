<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmADPrintPdf.aspx.cs" Inherits="Acurus.Capella.UI.frmADPrintPdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <iframe id="PDFLOAD" runat="server" width="100%" style="height:600px;"></iframe>
    </div>
    </form>
</body>
</html>
