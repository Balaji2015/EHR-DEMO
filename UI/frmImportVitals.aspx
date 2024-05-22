<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmImportVitals.aspx.cs"
    Inherits="Acurus.Capella.UI.frmImportVitals" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enter Vitals</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style1
        {
            height: 23px;
        }
        .style2
        {
            height: 23px;
            width: 129px;
        }
        .style3
        {
            height: 23px;
            width: 466px;
        }
      
        .style4
        {
            height: 23px;
            width: 74px;
        }
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="warningmethod();">
    <form id="frmImportVitals" runat="server">
     <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
     <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="ImportVitals"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
            <telerik:RadWindow ID="radWin" runat="server" Behaviors="Close" Title="ImportVitals"
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
        </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="RadAjaxPnlImportVitals" CssClass="bodybackground" runat="server" Height="16px" Width="1200px">
        <div style="height: 700px; width: 1200px;">
            <telerik:RadPanelBar ID="lblPatientStrip" runat="server" Width="100%">              
                <Items>
                    <telerik:RadPanelItem runat="server" Text="Root RadPanelItem1" BackColor="White" style="background-image:none;border:none;"   CssClass="pnlBarGroup">
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
            <asp:Panel ID="pnlImportVitals" runat="server" >
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblPhysicianName" runat="server"   mand="Yes" Text="Physician Name*" EnableViewState="false"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPhysicianName" runat="server" CssClass="nonEditabletxtbox" Width="440px" AutoCompleteType="Disabled" EnableViewState="false">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                        </td>
                        <td align="right">
                            <telerik:RadButton ID="btnGetVitals" runat="server" Style="top: 0px; left: -232px"
                                Text="Get Vitals" Width="100px" OnClick="btnGetVitals_Click" 
                                onclientclicked="btnGetVitals_Clicked">
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <telerik:RadSplitter ID="splVitals" runat="server" Width="1150px" Height="730px">
                <telerik:RadPane ID="pnlVitalsContainer" runat="server" Height="730px" Width="1200px" Scrolling="None">
                  <div>
                    <iframe id="VitalsContainer" runat="server" height="730px" width="1200px"></iframe>
                    </div>
                </telerik:RadPane>  
           </telerik:RadSplitter>
        </div>
         <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return closeForVitals();"/>
          <asp:HiddenField ID="hdnMessageType" runat="server" Value="" />
            <asp:HiddenField ID="hdnToEnableSave" runat="server" />
    </telerik:RadAjaxPanel>
 <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false"/>
 <asp:HiddenField ID="hdnVitalTime" runat="server" EnableViewState="false"/>
 <asp:PlaceHolder ID="PlaceHolder1" runat="server">
     <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
      <script src="Jscripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
      <link rel="stylesheet" href="CSS/jquery-ui.min.css">
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSVitals.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
