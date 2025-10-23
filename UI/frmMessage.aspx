<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmMessage.aspx.cs" Inherits="Acurus.Capella.UI.frmMessage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="CSS/CommonStyle.css" rel="stylesheet" />
<head runat="server">
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmMessage" runat="server">
    <div>
        <asp:Panel ID="Panel1" runat="server" Height="28px" Font-Size="Small" Font-Names="Microsoft Sans Serif"
            Width="200px">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Height="20px" Width="300px" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
        <scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </scripts>
    </telerik:RadScriptManager>
    <asp:HiddenField ID="hdnMessages" runat="server" EnableViewState="false"/>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
