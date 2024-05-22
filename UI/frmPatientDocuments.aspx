<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPatientDocuments.aspx.cs" Inherits="Acurus.Capella.UI.frmPatientDocuments" EnableEventValidation="false" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
    </style>
    <link href='CSS/bootstrap.min.css' rel='Stylesheet' />
    <link href='CSS/jquery-ui.css' rel='Stylesheet' />

    <link href='CSS/font-awesome.css' rel='stylesheet' type='text/css' />
    <link href='CSS/style.css' rel='stylesheet' type='text/css' />
       <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p style='margin-bottom: 3px; margin-top: 3px; margin-left: 2px; font-weight: bold; height: 27px; padding-top: 5px;' class="Editabletxtbox" ></p>
            <input type='button' value='Print' class='btn btn-xs btn-primary aspresizedbluebutton' style='float: right; margin-top: -9%; margin-right: 1%; margin-bottom: 1%; font-weight: bold;' id='btnPrint' /> <%--onclick="btnPrintDocument()"--%>
            <div id="divchkpatientdocument" style='width: 100%; height: 97.5%;border: 1px solid #ccc; display: inline-block;' class="chkitems">                            
                
            </div>
       
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <%--<script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>--%>            
            <script src="JScripts/JSPatientDocument.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
