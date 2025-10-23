<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPQRIBirtViewDetail.aspx.cs" EnableEventValidation="false" inherits="Acurus.Capella.UI.frmPQRIBirtViewDetail" %>


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
    <iframe  runat="server" style="width: 100%; height: 600px; border: none" id="ProcessiFrame"></iframe>
    </div>
    </form>
</body>
</html>
