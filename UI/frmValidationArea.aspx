<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmValidationArea.aspx.cs" Inherits="Acurus.Capella.UI.frmValidationArea" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Test Area</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        #form1
        {
        	width:262px;
        	height: 50px;
        }
        .style2
        {
            height: 30px;
            font-family: Sans-Serif;
            font-size: small;
        }
    </style>
    
  


   
   
</head>
<body>
    <form id="form1" runat="server" >
    <telerik:RadScriptManager Runat="server">
        <Scripts>
<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
</Scripts>
    </telerik:RadScriptManager>
    <table>
    <tr>
    <td colspan="3">
        <asp:Label ID="Label1" runat="server" Text="Do you want to save the Changes?" Width="100%"></asp:Label></td>
    </tr>
        <tr>
            <td width="35%" class="style2" align="right">
                <asp:Button ID="btnYes" runat="server" AutoPostBack="false" Text="Yes" Width="60px" 
                     OnClientClick="return Close(this);"/>
            </td>
            <td width="35%" class="style2" align="center">
                <asp:Button ID="btnNo" runat="server" AutoPostBack="false" Text="No" Width="60px" 
                     OnClientClick="return Close(this);"/>
            </td>
            <td width="35%" class="style2" align="left">
                <asp:Button ID="btncancel" runat="server" AutoPostBack="false" Text="Cancel" Width="60px" 
                       OnClientClick="return Close(this)"/>
            </td>
        </tr>

        
    </table>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/JSValidationArea.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
  
  </asp:PlaceHolder>
    </form>
</body>
</html>
