<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLabResult.aspx.cs" Inherits="Acurus.Capella.UI.frmResultLab" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Result</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #PatientDetails input[type="text"]
        {
        	background-color:#BFDBFF;
        }
        .pnlPatientDetails
        {
            font-weight:bold;
            font-size:10;
        }
        .style2
        {
            height: 250px;
        }
                
        #PatientDetails
        {
            height: 202px;
            width: 1005px;
        }
        
        #form1
        {
            height: 559px;
        }
        
    </style>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

   
</head>
<body bgcolor="#BFDBFF">
    <form id="form1" runat="server">
      <telerik:RadWindowManager ID="rdwResulst" runat="server" EnableViewState="false">
      <Windows>
      <telerik:RadWindow ID="MessageWindow" runat="server" VisibleOnPageLoad="false" Modal="true" Behaviors="Close"
      IconUrl="Resources/16_16.ico" EnableViewState="false">
      </telerik:RadWindow>
      </Windows>
      
      </telerik:RadWindowManager>
    <div id="pageViewLabcorp" runat="server">

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
       
        <table style="width:100%">
            <tr>
                <td style="vertical-align: top" class="style2">
                    <telerik:RadMultiPage ID="multiPageResult" Runat="server" Width="100%" 
                        Height="660px">
                        <telerik:RadPageView ID="RadPageView1" runat="server" Height="641px">
                            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" bgcolor="White">
                        <tr>
                         <td style="width: 50px;">
                       
                        </td>
                            <td style="width: 150px;">
                               
                            </td>
                            <td style="width: 90px;">
                                
                            </td>
                              <td style="width: 90px;">
                                
                            </td>
                            <td style="width: 490px;" align="right">
                                
                            </td>
                            <td style="width: 60px;">
                                
                            </td>
                            <td style="width: 50px;">
                                <telerik:RadButton ID="btnClose" runat="server" Text="Close" OnClientClicked="RadWindowClose" CssClass="redbutton" ButtonType="LinkButton"
                                    width="55px" Height="25px" Font-Size="13px">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </telerik:RadAjaxPanel>
    </div>
  <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
         <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
        </telerik:RadScriptManager>
       <asp:HiddenField ID="hdnSelectedItem" runat="server" EnableViewState="false" />
   <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/JSResult.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>         

        <script src="JScripts/JSC5PO.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>  
        <script src="JScripts/JSOrderException.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
