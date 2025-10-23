<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImportCQM.aspx.cs" Inherits="Acurus.Capella.UI.frmImportCQM" Async="true"   ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import</title>
  <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <script type="text/javascript">
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(requestEndHandler );

// This function will handle the end request event
function requestEndHandler(sender, args) {
   if( args.get_error() ){
      //document.getElementById("errorMessageLabel").innerText = 
      //   args.get_error().description;
       args.set_errorHandled(true);
   }
}

    </script>

    <style type="text/css">
        .style3,.style4{height:20px}.style3,.style5,.style7{width:254px}.style5,.style6{height:32px}.style7,.style9{height:62px }.style10{height:32px;width:398px}.style11{height:20px;width:398px}.style17,.style19,.style20{height:22px}.RadPicker{vertical-align:middle}.RadPicker .rcTable{table-layout:auto}.RadPicker .RadInput{vertical-align:baseline}.RadInput_Default{font:12px "segoe ui",arial,sans-serif}.riSingle{box-sizing:border-box;-moz-box-sizing:border-box;-ms-box-sizing:border-box;-webkit-box-sizing:border-box;-khtml-box-sizing:border-box;position:relative;display:inline-block;white-space:nowrap;text-align:left}.RadInput{vertical-align:middle}.riSingle .riTextBox{box-sizing:border-box;-moz-box-sizing:border-box;-ms-box-sizing:border-box;-webkit-box-sizing:border-box;-khtml-box-sizing:border-box}.RadPicker_Default .rcCalPopup{background-position:0 0;background-image:url('mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif')}.RadPicker .rcCalPopup{display:block;overflow:hidden;width:22px;height:22px;background-color:transparent;background-repeat:no-repeat;text-indent:-2222px;text-align:center}div.RadPicker table.rcSingle .rcInputCell{padding-right:0}.RadPicker table.rcTable .rcInputCell{padding:0 4px 0 0}.style19{width:98px}.style20{width:89px}.displayNone{display:none}.modal{position:fixed;top:0;left:0;background-color:#fff;z-index:99;opacity:.8;filter:alpha(opacity=80);-moz-opacity:.8;min-height:100%;width:100%}.underline{text-decoration:underline}
#cboPhysicianName {
            font-size: 12px !important;
        }
    </style>
     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
  </head>
<body >
    <form id="frmImportCQM" runat="server" style="background-color: White;">
       
    <telerik:RadWindowManager ID="WindowMngr" runat="server" >
        <Windows>
            <telerik:RadWindow ID="RadViewer" runat="server" Behaviors="Close" Title="Image Viewer"
                VisibleStatusbar="false" IconUrl="Resources/16_16.ico" VisibleOnPageLoad="false" >
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadAddWindow" runat="server" Behaviors="Close" Title="Add Or Update KeyWords"
                VisibleStatusbar="false"  IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadResultWindow" runat="server" Behaviors="Close" Title="Result Viewer"
                VisibleStatusbar="false" IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" AsyncPostBackTimeout="1800">
    
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
   <%--<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout ="360000">
</asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <Triggers>
    <asp:PostBackTrigger ControlID="btnClearAll" />
    </Triggers>
        <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" Font-Size="Small" GroupingText="Upload CAT I ZIP File"
                            Height="125px" Width="100%" ScrollBars="Auto" CssClass="Editabletxtbox">
                            <table style="width:100%">
                                <tr>
                                    <td >
                                       <%-- <asp:Label ID="lblSelectFile" runat="server"  Text="Select File(s) to Upload *" mand="Yes"></asp:Label>--%>

                                        <span class="MandLabelstyle">Select a ZIP File to Upload </span><span class="manredforstar">*</span>
                                    </td>
                                    <td class="style9" >
                                        <telerik:RadAsyncUpload ID="UploadImage" runat="server" 
                                            MultipleFileSelection="Automatic" 
                                            onclientfileuploading="UploadImage_FileUploading"
                                            onclientfileuploadremoved="UploadImage_FileUploadRemoved" 
                                            InitialFileInputsCount="0"   CssClass="bluebutton"
                                            MaxFileInputsCount="1"
                                            >
                                            <Localization Select="Browse"/>
                                        </telerik:RadAsyncUpload>
                                        <%--  --%>
                                    </td>
                             
                                    
                                </tr>
                                <tr>
                                   <td colspan="2" align="right" >
                                        <asp:Button runat="server" ID="btnImport" OnClick="btnImport_Click"  CssClass="btn  aspresizedgreenbutton" Text="Import" />
                                    
                                         <asp:Button runat="server" ID="btnFileClear" OnClick="btnFileClear_Click"  CssClass="btn aspresizedredbutton" Text="Clear"/>
                                    </td>

                                </tr>
                              
                            </table>
                            </asp:Panel>
                     <asp:Button ID="btnClearAll" runat="server"  Style="display: none;"  />  
        </ContentTemplate>
        
    </asp:UpdatePanel>
                    <br />
                   


    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript" ></script>
        <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>   
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSImportCQM.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
