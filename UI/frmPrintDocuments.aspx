<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPrintDocuments.aspx.cs"
    EnableEventValidation="false" Inherits="Acurus.Capella.UI.frmPrintDocuments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<base target="_self" />
<head runat="server">
    <title>Print Documents</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .verticalscroll {
            overflow-x: hidden;
            overflow-y: auto;
            height: 150px;
        }

       
        .style4 {
            width: 30px;
        }

        .MultilineText {
            D: \cmg2021\EHR-Capella5.0\UI\frmPrintDocuments.aspx resize: none;
        }
         body {
            resize: none;
        }
    </style>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top:0px!important;" onload="DisableButtons();">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Add/Update Keywords"
                    VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                <telerik:RadWindow ID="WellnessWindow" runat="server" Behaviors="Close" Title="Wellness Note"
                    VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close" Title="ProgressNotes"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadwindowPDF" runat="server" Behaviors="Close" Title="PDF"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close" Title="ConsultationNotes"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
                <telerik:RadWindow ID="PlanWindow" runat="server" Behaviors="Close" Title="Plan"
                    VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                 <telerik:RadWindow ID="RadWindow4" runat="server" Behaviors="Close" Title="CareNote"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
                  <telerik:RadWindow ID="RadWindow5" runat="server" Behaviors="Close" Title="TreatmentPlanNote"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div>
            <aspx:ToolkitScriptManager ID="toolkitScriptMngr" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </aspx:ToolkitScriptManager>
            <telerik:RadAjaxPanel ID="refreshPanel" CssClass="LabelStyleBold" runat="server" Width="100%"
                Font-Size="Small" Height="16px">
                <table width="100%">
                    <tr>
                        <td style="width:44.9%">
                            <asp:Panel ID="pnlFollowUp" runat="server" GroupingText="Follow up Appointment"
                                CssClass="LabelStyleBold" BackColor="White">
                                <table width="100%" style="height:155px;">
                                    <%--<tr>
                                        <td>
                                            <asp:CheckBox ID="chkDueOn" runat="server" Text="Due On" Font-Size="Small" onchange="DueonClick(this);"
                                                AutoPostBack="false" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtDueon" runat="server"  DateInput-DateFormat="dd-MMM-yyyy"
                                                onkeypress="EnableSave();" Style="padding-left: 2px;">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkReturnIn" runat="server" Text="Return In" Cssclass="spanstyle"
                                                onchange="return ReturnInClick(this);" AutoPostBack="false" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <%--<telerik:RadTextBox ID="txtReturnIn1" runat="server" Height="20px" Width="30px" 
                                                            nospell="true" MaxLength="2" Style="position: static" onkeypress="return AllowNumbers(event)">
                                                        </telerik:RadTextBox>--%>

                                                        <asp:TextBox  ID="txtReturnIn" runat="server" Height="20px" Width="30px" CssClass="nonEditabletxtbox"
                                                            MaxLength="2"  Style="position: static" onkeypress="return AllowNumbers(event)" ></asp:TextBox>
                                                    </td>
                                                    <td class="spanstyle">Months
                                                    </td>
                                                    <td >
                                                       <%-- <telerik:RadTextBox ID="txtRetrunWeeks" runat="server" Height="20px" Width="30px" CssClass="nonEditabletxtbox"
                                                            nospell="true" MaxLength="2" Style="position: static" onkeypress="return AllowNumbers(event)">
                                                        </telerik:RadTextBox>--%>
                                                        <asp:TextBox  ID="txtRetrunWeeks" runat="server" Height="20px" Width="30px" CssClass="nonEditabletxtbox"
                                                            MaxLength="2"  Style="position: static" onkeypress="return AllowNumbers(event)" ></asp:TextBox>
                                                    </td>
                                                        
                                                    </td>
                                                    <td class="spanstyle" >Weeks
                                                    </td>
                                                    <td >
                                                        <%--<telerik:RadTextBox ID="txtRetrunDays" runat="server" Height="20px" Width="30px" CssClass="nonEditabletxtbox"
                                                            nospell="true" MaxLength="2" Style="position: static" onkeypress="return AllowNumbers(event)">
                                                        </telerik:RadTextBox>--%>
                                                        <asp:TextBox  ID="txtRetrunDays" runat="server" Height="20px" Width="30px" CssClass="nonEditabletxtbox"
                                                            MaxLength="2"  Style="position: static" onkeypress="return AllowNumbers(event)" ></asp:TextBox>
                                                    </td>
                                                    </td>
                                                    <td class="spanstyle">Days
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNotes" runat="server" Text="Notes" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White" CssClass="spanstyle">
                                                <DLC:DLC ID="txtNotes" runat="server" TextboxHeight="62px" TextboxWidth="205px" Value="FOLLOW_UP_REASON_NOTES"></DLC:DLC>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                             <input type='checkbox' runat="server"  disabled="disabled" id='chkSurgeryDeclaration' onclick='ChkElectronicDeclaration_Checked();'>
                                             <span id='ProceedwithSurgeryasPlanned' runat="server" style='font-size:13px!important;width: -19%;word-wrap: break-word;'>Proceed with Surgery as Planned</span>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width:55%">
                            <asp:Panel ID="pnlDocuments" runat="server" GroupingText="Select required documents to be printed"
                                 CssClass="LabelStyleBold" >
                                <table style="width: 100%; height: 150px;">
                                    <tr>
                                        <td style="width: 100%; height: 90%">
                                            <div class="verticalscroll">
                                                <asp:CheckBoxList ID="chklstSelectDocuments" runat="server" Height="16px" Width="100%"
                                                    onchange="EnableDocuments();" AutoPostBack="false" CssClass="spanstyle">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <asp:ImageButton ID="imgDocumentsLibrary" CssClass="displayNone" runat="server" ImageUrl="~/Resources/Database Inactive.jpg"
                                                OnClientClick="return OpenAddUpdateKeywords('PATIENT EDUCATION DOCUMENTS');" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr runat="server" id="trAlignment">
                        <td colspan="2">
                            <asp:Panel ID="Alignment" runat="server" Width="100%" cssclass="bodybackground">
                                <table width="100%" style="height:30px;">
                                    <tr>
                                        <td style="width: 14%">
                                            <asp:Label ID="lblRelationShip" runat="server" Text="Relationship" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <telerik:RadComboBox ID="cboRelationship" onclientselectedindexchanged="OnClientSelectedIndexChanged"  runat="server" Width="100%" AutoPostBack="True"
                                                MaxHeight="100px" OnSelectedIndexChanged="cboRelationship_SelectedIndexChanged" CssClass="spanstyle">
                                                
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value="" />
                                                    <telerik:RadComboBoxItem Text="Self" Value="Self" />
                                                    <telerik:RadComboBoxItem Text="Spouse" Value="Spouse" />
                                                    <telerik:RadComboBoxItem Text="Father" Value="Father" />
                                                    <telerik:RadComboBoxItem Text="Mother" Value="Mother" />
                                                    <telerik:RadComboBoxItem Text="Brother" Value="Brother" />
                                                    <telerik:RadComboBoxItem Text="Sister" Value="Sister" />
                                                    <telerik:RadComboBoxItem Text="Son" Value="Son" />
                                                    <telerik:RadComboBoxItem Text="Daughter" Value="Daughter" />
                                                    <telerik:RadComboBoxItem Text="Aunt" Value="Aunt" />
                                                    <telerik:RadComboBoxItem Text="Uncle" Value="Uncle" />
                                                    <telerik:RadComboBoxItem Text="Grand Father" Value="Grand Father" />
                                                    <telerik:RadComboBoxItem Text="Grand Mother" Value="Grand Mother" />
                                                    <telerik:RadComboBoxItem Text="Other" Value="Other" />
                                                    <telerik:RadComboBoxItem Text="Nephew" Value="Nephew" />
                                                    <telerik:RadComboBoxItem Text="Niece" Value="Niece" />
                                                    <telerik:RadComboBoxItem Text="Grand Son" Value="Grand Son" />
                                                    <telerik:RadComboBoxItem Text="Grand Daughter" Value="Grand Daughter" />
                                                    <telerik:RadComboBoxItem Text="Friend" Value="Friend" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                        <td style="width: 15%">
                                            <asp:Label ID="lblGivenTo" runat="server" Text="Documents given to" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <telerik:RadTextBox ID="txtGivenTo" runat="server" Width="100%" Height="19px" CssClass="Editabletxtbox">
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" CssClass="Editabletxtbox" />
                                                <ReadOnlyStyle Resize="None" />
                                                <EmptyMessageStyle Resize="None" />
                                                <FocusedStyle Resize="None" CssClass="Editabletxtbox" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td style="width: 14%">
                                            <asp:Label ID="lblIsDocumentGiven" runat="server" Text="Is Document Given" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td style="width: 15%">
                                            <telerik:RadComboBox ID="cboIsDocumentGiven" runat="server" Width="100%" CssClass="Editabletxtbox">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value="" />
                                                    <telerik:RadComboBoxItem Text="Yes" Value="Yes" />
                                                    <telerik:RadComboBoxItem Text="No" Value="No" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                   
                    <tr runat="server" id="trpnlAddendum">
                        <td  colspan="2">
                            <asp:Panel ID="pnlAddendum" runat="server" GroupingText="Amendment to Plan" Width="100%"
                                CssClass="LabelStyleBold" >
                                <table width="100%" style="height: 35px">
                                    <tr>
                                        <td style="width:18%;">
                                            <asp:RadioButton ID="radbtnAgreePlan" Width="100%" runat="server" onclick="ChangeRadio();"
                                                GroupName="radGroupValidation" Text="Agree with Plan" CssClass="spanstyle" />
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="radbtnAgreewithChanges" Width="100%" runat="server" onclick="ChangeRadio();"
                                                GroupName="radGroupValidation" Text="Agree with Amendments" Cssclass="spanstyle"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtAddendumToPlan" runat="server" Width="100%" Height="50px" TextMode="MultiLine" CssClass="MultilineText">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td >
                                            <asp:Button ID="btnPlan" runat="server" Text="Plan" Style="display:none !important"  CssClass="displaynonestyle"  OnClientClick="return OpenAddOrUpdatePlan();" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2">
                     <asp:RadioButton ID="radbtnCorrection" Width="100%" runat="server" onclick="ChangeRadio();" GroupName="radGroupValidation" Text="Corrections to be Made" CssClass="spanstyle" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtCorrectionToPlan" runat="server" Width="100%" Height="170px" TextMode="MultiLine" CssClass="MultilineText" Enabled="false">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                     <tr runat="server" id="trPanel1">
                        <td  colspan="2">
                            <asp:Panel ID="Panel1" runat="server" Width="100%" CssClass="bodybackground">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2" style="width: 40%">
                                            <asp:CheckBox ID="chkPFSHVerified" runat="server" Text=" " />
                                            <asp:Label ID="lblPFSH" runat="server" Text="Is PFSH Verified *" mand="Yes" CssClass="manredforstar"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr runat="server" id="trpnlElectronicSignature">
                        <td  colspan="2">
                            <asp:Panel ID="pnlElectronicSignature" runat="server" GroupingText="Electronic Signature Declaration"
                                CssClass="LabelStyleBold" Height="66px" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkElectronicDeclaration" runat="server" Width="100%" Height="30px" onclick="ForSaveEnable();" CssClass="spanstyle"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="2">
                            <asp:Panel ID="pnlButtons" runat="server" Width="100%" cssclass="bodybackground">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 80%">
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" EnableViewState="False"
                                                OnClientClick="Printwaitcursor();" CssClass="aspresizedbluebutton" />
                                        </td>
                                        <%--<td style="width: 12%">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100%" OnClick="btnSave_Click" />
                                        </td>--%>
                                        <td style="width: 12%">
                                            <asp:Button ID="btnSave" runat="server" Text="Sign and Move to Next Process" Width="100%"  OnClick="btnSignMove_Click" CssClass="aspresizedgreenbutton"/>
                                        </td>
                                        <td style="width: 12%" runat="server" id="tdBtnMovetoPhyAsst">
                                            <asp:Button ID="btnMovetoPhyAsst" runat="server" Text="Move to Physician Assistant" Width="100%" OnClick="btnMovetoPhyAsst_Click" CssClass="aspresizedbluebutton"/>
                                        </td>

                                        <td style="width: 12%" runat="server" id="td1">
                                            <asp:Button ID="btnmovetoscribe" runat="server" Text="Move to Scribe" Width="100%" OnClick="btnmovetoscribe_Click" CssClass="aspresizedbluebutton"  Enabled="false"/>
                                        </td>
                                        <td style="width: 8%">
                                            <asp:Button ID="btnClose" runat="server" Text="Cancel" OnClientClick="return Close_printDocument();" CssClass="aspresizedredbutton"
                                                Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnClearDrpdwn" runat="server" OnClick="btnClearDrpdwn_Click" CssClass="displaynonestyle"  Style="display:none !important" /><%--//BugID:40876--%>
                <asp:Button ID="btnInvisible" runat="server" OnClick="btnInvisible_Click" CssClass="displaynonestyle"  Style="display:none !important" />
                <asp:HiddenField ID="MySignID" runat="server" />
                <asp:HiddenField ID="SignedDateandTime" runat="server" />
                <asp:HiddenField ID="DigitalSign" runat="server" />
                <asp:HiddenField ID="userName" runat="server" />
                <asp:HiddenField ID="hdnSelectedItem" runat="server" />
                <asp:HiddenField ID="hdnLocalTime" runat="server" />
                <asp:HiddenField ID="hdnMessageType" runat="server" />
                <asp:HiddenField ID="hdnSaveDetails" runat="server" />
                <asp:HiddenField ID="hdnEnableYesNo" runat="server" />
                <asp:HiddenField ID="hdnPrintFilePath" runat="server" />
                <asp:HiddenField ID="hdnXmlPath" runat="server" />
                <asp:HiddenField ID="hdnfilename" runat="server" />
                <asp:HiddenField ID="hdnscribe" runat="server" />
                <asp:HiddenField ID="hdnChkElecDeclaration" runat="server" />
                <asp:Button ID="btnMessageType" runat="server" Text="Button" CssClass="displaynonestyle"
                    OnClientClick="Close_printDocument();" />
                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                    <script src="JScripts/JSPlan.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <%--<link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>

                    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                </asp:PlaceHolder>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
