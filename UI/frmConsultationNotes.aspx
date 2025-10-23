<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmConsultationNotes.aspx.cs" Inherits="Acurus.Capella.UI.frmConsultationNotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
  
   <script src="JScripts/bootstrap.min.css.3.1.1.js" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min3.1.1.js" type="text/javascript"></script>
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSSummaryNew.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JsHighlight.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

</head>
<body>
    <form id="frmConsultationNotes" runat="server">
  <asp:Button ID="btnword" runat="server" OnClick="btnword_Click"  style="display:none"/>
         <asp:Button ID="btnpdf" runat="server" OnClick="btnpdf_click"  style="display:none"/>
         <asp:Button ID="btnfax" runat="server" OnClick="btnFax_Click"  style="display:none"/>
        <asp:HiddenField runat="server" ID="hdnFilePath" />
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">

    <script src="JScripts/HTMLAdvDocViewer.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
     <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
     <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
