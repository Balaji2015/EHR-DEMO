<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPFSHVerficationYesNo.aspx.cs" Inherits="Acurus.Capella.UI.frmPFSHVerficationYesNo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Encounter</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target=_self />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:200px;position:relative;" >
    <br />
        <div  class="Editabletxtbox">
            Do you want to verify PFSH?
        </div>
        <div style="display:block;float:right">
            <asp:Button ID="btnOK" class="aspgreenbutton" runat="server" Text="Ok" onclick="btnOK_Click" />
            <asp:Button ID="btnCancel" class="aspredbutton" runat="server" Text="Cancel" 
                onclick="btnCancel_Click" />
        </div>
        <br />
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
     <script src="JScripts/JSPFSHVerificationYesNo.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="false"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
  
  </asp:PlaceHolder>
    </form>
</body>
</html>
