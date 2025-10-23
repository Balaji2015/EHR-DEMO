<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmServicetype.aspx.cs" Inherits="Acurus.Capella.UI.frmServicetype" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Type</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/datetimepicker.css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
</head>
<body onload="LoadServiceType()">
    <form id="form1" runat="server">
        <div style="overflow:auto;height:500px;">
            <table id="tblserviceType" class="table table-bordered Gridbodystyle" style="width: 100%; overflow: auto;"></table>
            <br />
            <br />
           
        </div>
         <div style="width: 100%">
                <table style="width: 100%" >
                    <tr>
                        <td style="width:88%">
                            <span  style="color:red; visibility: hidden;">** Only upto 99 service types can be selected for EV Request</span>
                        </td>
                        <td >
                            <asp:Button ID="btnOK" runat="server" CssClass="aspresizedgreenbutton" Text="Ok"  OnClientClick="return serviceOKclick();" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="aspresizedredbutton" Text="Close" OnClientClick="return CloseServiceType();" />
                        </td>
                    </tr>

                </table>
            </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <%--<link href="CSS/jquery-ui.css" rel="stylesheet" />--%>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>

            <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JsPerformEV.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
