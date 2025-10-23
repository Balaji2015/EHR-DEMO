<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmProviderValidation.aspx.cs" Inherits="Acurus.Capella.UI.frmProviderValidation" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Provider Validation</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style3
        {
            width: 474px;
        }
        .style4
        {
            width: 452px;
        }
        
        .style5
        {
            height: 20px;
        }
        
        #form1
        {
            height: 138px;
            width: 1019px;
            margin-bottom: 1px;
           
        }
        
    </style>
    
    
 <link href="CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
    
</head>
<body class="bodybackground">
    <form id="form1" runat="server">
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="62px" 
        Width="507px">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
    <div style="height: 89px; width: 1015px">
    
        <asp:Panel ID="Panel1" runat="server" Height="85%" Width="100%" CssClass="LabelStyleBold">
            <table style="width: 100%; height: 86px;">
                <tr>
                    <td class="style5">
                        <asp:Label ID="lblMsg" runat="server" Text="The appointment was initially fixed with " CssClass="spanstyle"></asp:Label>
                    </td>
                    
                   
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel3" runat="server" Height="53%" Width="100%" CssClass="LabelStyleBold">
                            <table style="width: 100%; height:41%;">
                                <tr>
                                    <td class="style4">
                                         
                                        <telerik:RadButton ID="btnOption1" runat="server" onclick="btnOption1_Click" ButtonType="LinkButton" CssClass=" bluebutton teleriknormalbuttonstyle"
                                            Width="98%" onclientclicked="btnOption1_Clicked">
                                        </telerik:RadButton>
                                         
                                    </td>
                                    <td class="style3">
                                         
                                        <telerik:RadButton ID="btnOption2" runat="server" onclick="btnOption2_Click" ButtonType="LinkButton" CssClass=" bluebutton teleriknormalbuttonstyle"
                                            Width="98%" onclientclicked="btnOption2_Clicked">
                                        </telerik:RadButton>
                                         
                                    </td>
                                    <td>
                                         
                                        <telerik:RadButton ID="btnCancel" runat="server" onclick="btnCancel_Click" ButtonType="LinkButton" CssClass=" redbutton teleriknormalbuttonstyle"
                                            Text="Cancel" onclientclicked="btnCancel_Clicked">
                                        </telerik:RadButton>
                                         
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    
                </tr>
                
            </table>
        </asp:Panel>
    
    </div>
    
    
     </telerik:RadAjaxPanel>
    <asp:HiddenField ID="hdnButton1" runat="server" />
    <asp:HiddenField ID="hdnButton2" runat="server" />
    <asp:HiddenField ID="hdnButton3" runat="server" />
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <scripts>
<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
</scripts>
    </telerik:RadScriptManager>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
     <script src="JScripts/JSProviderValidation.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
