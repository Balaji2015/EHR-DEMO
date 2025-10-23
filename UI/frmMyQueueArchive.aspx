<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMyQueueArchive.aspx.cs" Inherits="Acurus.Capella.UI.frmMyQueueArchive" %>

<!DOCTYPE html>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MyQ - Archive</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style>
        .highlight {
    background-color: #F3C55A !important;
}
        .tableFixHead          { overflow-y: auto; height: 100px; }
        .tableFixHead thead th { position: sticky; top: 0; z-index: 5;}
        .tr{
            page-break-inside: auto;
        }
    </style>
   <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CssNotify.css" rel="stylesheet" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/CommonStyle.css" rel="stylesheet"  type="text/css"/>


</head>
<body>
    <form id="frmMyQueueArchive" runat="server">
        
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" Modal="false"
                    VisibleStatusbar="False" ShowContentDuringLoad="false">
                </telerik:RadWindow>

                <telerik:RadWindow ID="RadWindow1" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" Modal="false"
                    VisibleStatusbar="False" ShowContentDuringLoad="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
         <script language="javascript" type="text/javascript">
             var ModalWndw = "<%=ModalWindow.ClientID %>";
        </script>
           <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                        <Scripts>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                                        </Scripts>
                                    </telerik:RadScriptManager>
    <div style="width: 100%; height: 560px; overflow: auto; margin-top: 4px;" id="MyTable" class="tableFixHead">  
       
        <table id="tblMyQarchive" class="table table-bordered Gridbodystyle" style="width: 98%; margin-left: 4px;">
             <thead class="Gridheaderstyle">
              <tr>
                <th class="Gridheaderstyle" style="width: 13%; text-align: center;">Appt. Date & Time</th>
                <th class="Gridheaderstyle" style="width: 5%;  text-align: center; ">Acct. #</th>
                <th class="Gridheaderstyle" style="width: 12%; text-align: center">Patient Name</th>
                  <th class="Gridheaderstyle" style="width: 8%; text-align: center">Patient DOB</th>
                  <th class="Gridheaderstyle" style="width: 20%; text-align: center">Chief Complaints</th>
                  <th class="Gridheaderstyle" style="width: 10%; text-align: center">Current Process</th>
                  <th class="Gridheaderstyle" style="width: 5%; text-align: center">View Summary</th>
                  <th class="Gridheaderstyle" style="width: 5%; text-align: center" id="chkSigEncounter" >Sign-Off Encounter</th>
                  <th class="Gridheaderstyle" style="width: 5%; text-align: center" id="chkWorkEncounter">Work-on Encounter</th>
                  <th class="Gridheaderstyle" style="display:none;">EncounterID</th>
                
              </tr>
             </thead>
            <tbody id="tbodyachiveinfo" class="Gridbodystyle">
                        </tbody>
        </table>   
      </div>
        <div class="col-md-12 rowspace" style="overflow-y: auto; height: 85%; width: 100%; padding: 0px;  " id="scrollID">
                <div class="table-responsive" style="height: 100%;" id="MyQTable" >
                </div>
            </div>
      <div style="margin-left:86%;margin-top: 1%;">
           <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="aspgreenbutton" OnClientClick="return btnSubmitButtonClicked();" OnClick="SavedSuccessfully"/>
           <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="aspredbutton" OnClientClick="closeArchive();" />
      </div>

        <asp:HiddenField ID="hdnHumanID" runat="server" />
         <link href='CSS/bootstrap.min.css' rel='Stylesheet' />
    <link href='CSS/jquery-ui.css' rel='Stylesheet' />
    <link href='CSS/font-awesome.css' rel='stylesheet' type='text/css' />
    <link href='CSS/style.css' rel='stylesheet' type='text/css' />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>

            <script src="JScripts/JsMyQueueArchive.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </form>
</body>
</html>
