<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmClinicalSummary.aspx.cs"
    Inherits="Acurus.Capella.UI.frmClinicalSummary" EnableEventValidation="false" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
     <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <title>Clinical Summary</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style2
        {
            width: 245px;
        }
        .style3
        {
            width: 204px;
        }
        .style4
        {
            width: 99px;
        }
        .style5
        {
            width: 56px;
        }
        .Displaynone
        {
        	display:none
        }
        .SendSummary_td
        {
        	width:25%;
        }
        .ButtonStyle
        {
        	font-family: "Segoe UI";
            font-size: 12px;
            color: #303030;
        }
    </style>

</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmClinicalSummary" runat="server">
    <telerik:RadWindowManager ID="OrderManagementWindowManager" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="ModalWindow" runat="server" Modal="true" VisibleOnPageLoad="false"
                Behaviors="Close" IconUrl="Resources/16_16.ico" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <div>
        <asp:Panel ID="pnlClinicalSummary" Font-Size="Small" GroupingText="Data To Be Displayed"
            runat="server" CssClass="Editabletxtbox">
            <div>
             <asp:Label ID="Label3" runat="server" Text="  Automatically Displayed" CssClass="Editabletxtbox"></asp:Label>
             <br />
             
                <asp:Panel ID="pnlAccountInformation" runat="server" BorderColor="Black" BorderWidth="1px">
                
                    <asp:Label ID="Label1" runat="server" Text="  Patient details and Demographics" CssClass="Editabletxtbox"></asp:Label>
                    <br>
                    <asp:Label ID="Label2" runat="server" Text="  Provider name and contact Information" CssClass="Editabletxtbox"></asp:Label>
                    <br>
                    <%--The following line has been commented for BugID:30532 by Pujhitha --%>
                    <%--<asp:Label ID="Label4" runat="server" Text="  Vitals"></asp:Label>--%>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pnlEncounterDetails" GroupingText="Encounter Details" runat="server" CssClass="Editabletxtbox">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style3">
                                <asp:CheckBox ID="chkReasonOfVisit" runat="server" Text="Reason Of Visit" CssClass="Editabletxtbox"/>
                                <asp:CheckBox ID="chkVitals" runat="server" Text="Vitals" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkClinicalInstruction" runat="server" Text="Clinical Instruction" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkImmunization" runat="server" Text="Immunizations" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkMentalStatus" runat="server" Text="Mental Status" CssClass="Editabletxtbox"/>

                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:CheckBox ID="chkCarePlan" runat="server" Text="Care Plan" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkLaboratoryTest" runat="server" Text="Laboratory Test(s)"  CssClass="Editabletxtbox"/>
                                <asp:CheckBox ID="chkSmokingStatus" runat="server" Text="Smoking Status" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkAllergies" runat="server" Text="Allergy" CssClass="Editabletxtbox"/>
                            </td>
                              <td>
                                <asp:CheckBox ID="chkFunctionalStatus" runat="server" Text="Functional Status" CssClass="Editabletxtbox"/>

                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:CheckBox ID="chkProcedures" runat="server" Text="Procedure(s)" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkLaboratoryResultValues" runat="server" Text="Laboratory Values/Results" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                 <asp:CheckBox ID="chkEncounter" runat="server" Text="Encounter" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkGoals" runat="server" Text="Goals" CssClass="Editabletxtbox"/>

                            </td>
                        </tr>
                     
                        <tr>
                            <td class="style3">
                               <asp:CheckBox ID="chkChiefComplaints" runat="server" Text="Assessment" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkMedication" runat="server" Text="Medication" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkMedicationAdministrative" runat="server" Text="Medications Administered During visit" CssClass="Editabletxtbox"/>
                            </td>
                             <td>
                                <asp:CheckBox ID="chkTreatmentPlan" runat="server" Text="Treatment Plan" CssClass="Editabletxtbox"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:CheckBox ID="chkProblemList" runat="server" Text="Problem List" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkReasonforReferral" runat="server" Text="Reason for Referral" CssClass="Editabletxtbox"/>
                            
                            </td>
                            <td>
                                 <asp:CheckBox ID="chkImplant" runat="server" Text="Implants" CssClass="Editabletxtbox"/>
                                 <asp:CheckBox ID="chkFutureAppointment" runat="server" CssClass="Displaynone" 
                                    Text="Future Appointment" />
                            </td>
                            
                             <td>
                                <asp:CheckBox ID="chkHealthConcern" runat="server" Text="Health Concern" CssClass="Editabletxtbox"/>
                            </td>                            
                        </tr>
                          <tr>
                            <td class="style3">
                                <asp:CheckBox ID="chkLabTest" runat="server" Text="Lab Test" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkLab" runat="server" Text="Laboratory Information" CssClass="Editabletxtbox"/>
                            </td>
                        </tr>
                           <tr>
                            <td class="style3">
                                <asp:CheckBox ID="chkDiagnosticTestPending" runat="server" Text="Diagnostics Tests Pending" CssClass="Editabletxtbox" />
                            </td>
                            <td class="style2">
                                <asp:CheckBox ID="chkFutureScheduledTest" runat="server" Text="Future Scheduled Tests" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPatientAids" runat="server" Text="Patient Decision Aids" CssClass="Editabletxtbox" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPayer" runat="server" Text="Payer" CssClass="Editabletxtbox"/>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pnlButtons" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style4">
                                &nbsp;
                                <asp:CheckBox ID="chkCheckAll" runat="server" Text="Check All" onclick="Select();" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td class="style5">                                 
                            </td>
                              <td>
                                &nbsp;</td>
                            <td class ="style5">
                                <asp:Button ID="btnLaunchInferno" runat ="server" Visible="true" OnClientClick="return btnLaunch_Inferno();"
                                    Text="Launch API" CssClass="aspbluebutton" />
                            </td>
                            <td class="style5">
                                <asp:Button ID="btnSendCerner" runat="server" Visible="false" OnClick="btnSendCerner_Click"
                                    Text="Send to Cerner" CssClass="aspbluebutton" OnClientClick="return SendCerner();" />
                            </td>                          
                            <td class="style5">
                                 <asp:Button ID="btnCaregenerate" runat="server" OnClick="btnCaregenerate_Click" OnClientClick="return GenerateXML();"
                                    Text="Generate Care Note" CssClass="aspbluebutton"/>
                            </td>
                            <td class="style5">
                                 <%--<asp:Button ID="btnGenerate" runat="server" onclick="btnGenerate_Click" OnClientClick="return GenerateXML();"
                                    Text="Generate CCD" CssClass="aspgreenbutton"/>--%>
                                 <asp:Button ID="btnGenAtltovaCCD" runat="server" OnClick="btnGenAtltovaCCD_Click" OnClientClick="return GenerateXMLAltova();"
                                    Text="Generate CCD" CssClass="aspgreenbutton"/>
                            </td>
                            <td class="style5">                               
                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" CssClass="aspredbutton" OnClientClick="return ClearAll();" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="pnlSendSummary" Font-Size="Small" GroupingText="Send Summary" runat="server" CssClass="Editabletxtbox">
        <table width="100%">
      
        <tr>
            <td>
                <table width="100%">
                      <tr>
        <td style="width:15%;">
        <asp:Label ID="lblRecpAddress" runat="server" Text = "Recipient" Width="60px" CssClass="Editabletxtbox"></asp:Label>
        </td>
        <td style="width:35%;">
       <%-- <telerik:RadTextBox ID="txtRecAdd" runat="server" TextMode="MultiLine" Width="390px">
            <DisabledStyle Resize="None" />
            <InvalidStyle Resize="None" />
            <HoveredStyle Resize="None" />
            <ReadOnlyStyle Resize="None" />
            <EmptyMessageStyle Resize="None" />
            <FocusedStyle Resize="None" />
            <EnabledStyle Resize="None" />
            </telerik:RadTextBox>--%>
               <DLC:DLC ID="DLCRecAdd" runat="server" TextboxHeight="60px" TextboxWidth="330px"
                                                               Value="EMAIL" />
        </td>
        <td style="width:15%; text-align:center;">
        <asp:Label ID="lblMailText" runat="server" Text = "Message" Width="65px" CssClass="Editabletxtbox"></asp:Label>
        </td>
        <td style="width:35%;">
        <telerik:RadTextBox id="txtMailText" runat="server" TextMode="MultiLine" Width="390px"></telerik:RadTextBox>
        </td>
        </tr>
                </table>
            </td>
        </tr>
        <tr>
        <td  class="SendSummary_td">
        
        </td>
        <td  class="SendSummary_td">
        
        </td>
        <td  class="SendSummary_td">
        
        </td>
        <td  class="SendSummary_td">
        
        </td>
        </tr>
        
        <tr>
        <td  class="SendSummary_td">
        
        </td>
        <td  class="SendSummary_td">
        
        </td>
        <td  class="SendSummary_td">
        
        </td>
        <td  class="SendSummary_td">
        
        </td>
        </tr>
        
        </table>
        <div>
        <asp:LinkButton ID="lblAttachment" runat="server" Text="Attachment" CssClass="alinkstyle"></asp:LinkButton>
        </div>
        </asp:Panel>
        
        <table width="100%">
        <tr style="width:100%;">
        <td align="right"  style="width:100%;">
        <%--<telerik:RadButton ID="btnSendSummary" runat="server" Text ="Send" 
                onclick="btnSendSummary_Click" onclientclicked="btnSendSummary_Clicked" ButtonType="LinkButton" CssClass="greenbutton"></telerik:RadButton>--%>
        <asp:Button ID="btnSendSummary" runat="server" Text="Send"
            OnClick="btnSendSummary_Click" OnClientClick="btnSendSummary_Clicked" CssClass="aspgreenbutton" />
        </td>
        </tr>
        </table>
        
         <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
           <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
    </telerik:RadScriptManager>
        <asp:HiddenField ID="hdnPrintFilePath" runat="server" />
         <asp:HiddenField ID="hdnXmlPath" runat="server" />
        <asp:HiddenField ID="hdnHuman" runat="server" />
        <asp:Button ID="btnDownloadXML" runat="server" CssClass="Displaynone" 
            onclick="btnDownloadXML_Click" Text="Button" />
    </div>
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSClinicalSummary.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
