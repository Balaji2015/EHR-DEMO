<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmEligibilityVerification.aspx.cs"
    Inherits="Acurus.Capella.UI.frmEligibilityVerification" %>


<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self"></base>
    <title>Eligibility Verification</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style2 {
            width: 739px;
        }

        .style3 {
            width: 105px;
        }

        #pnlManualEV fieldset{
            height: 373px;
        }

        #frmEligibilityVerification {
            width: 784px;
        }

        .style11 {
            width: 149px;
        }

        .style14 {
            width: 143px;
        }

        .style17 {
            height: 36px;
        }

        .style18 {
            width: 75px;
            height: 36px;
        }

        .style20 {
            width: 98px;
            height: 36px;
        }

        .style22 {
            width: 232px;
        }

        .style23 {
            width: 98px;
        }

        .style25 {
            width: 54px;
        }

        .style26 {
            width: 104px;
        }

        .style29 {
            width: 75px;
        }

        .style31 {
        }

        .style32 {
            height: 36px;
            width: 161px;
        }

        .style35 {
            height: 36px;
            width: 89px;
        }

        .style36 {
            width: 89px;
        }

        .style37 {
            width: 91px;
        }

        .style38 {
            width: 10px;
        }

        .Panel legend {
            font-weight: bold;
        }

        .style40 {
            width: 73px;
        }

        .style41 {
            width: 70px;
        }

        .MultiLineTextBox {
            resize: none;
        }

        .style42 {
            height: 97px;
        }
    </style>

    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
   <%-- <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />--%>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>

    <%-- bgcolor="bfdbff"--%>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmEligibilityVerification" runat="server" >
        <telerik:RadWindowManager ID="WindowMngr" runat="server" VisibleStatusbar="false" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Eligibility Verification">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
      <%--  <div style="width: 785px; height: 438px;">
            <div>
                <asp:Panel ID="pnlPatientInfo" runat="server" Font-Size="Small" CssClass="Panel"
                    BackColor="White" GroupingText="Patient/Carrier info" Width="783px">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style37">
                                <asp:Label ID="lblPatientAccountNo" runat="server" Text="Acc. #" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style11">
                                <asp:TextBox ID="txtPatientAccountNo" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td class="style38">&nbsp;
                            </td>
                            <td class="style25">
                                <asp:Label ID="lblPatient" runat="server" Text="Name" EnableViewState="false"></asp:Label>
                            </td>--%>
                           <%-- <td class="style14">
                                <asp:TextBox ID="txtPatientName" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td class="style38">&nbsp;
                            </td>
                            <td class="style26">
                                <asp:Label ID="lblPolicyHolderID" runat="server" Text="Policy Holder ID" EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPolicyHolderID" runat="server" ReadOnly="True" Style="margin-left: 2px"
                                    Width="150px" BackColor="#BFDBFF"></asp:TextBox>
                            </td>--%>
                        <%--</tr>
                        <tr>
                            <td class="style37">
                                <asp:Label ID="lblInsPlan" runat="server" Text="Ins Plan ID" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style11">
                                <asp:TextBox ID="txtInsPlanID" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td class="style38">&nbsp;
                            </td>
                            <td class="style25">
                                <asp:Label ID="lblGroupNo" runat="server" Text="Group #" EnableViewState="false"></asp:Label>
                            </td>--%>
                            <%--<td class="style14">
                                <asp:TextBox ID="txtGroupNumber" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td class="style38">&nbsp;
                            </td>
                            <td class="style26">
                                <asp:Label ID="lblCarrerName" runat="server" Text="Carrier Name" EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCarrierName" runat="server" Width="150px" BackColor="#BFDBFF"
                                    ReadOnly="True"></asp:TextBox>
                            </td>--%>
                       <%-- </tr>
                        <tr>
                            <td class="style37">
                                <asp:Label ID="lblPatientType" runat="server" Text="Patient Type" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style11">
                                <asp:TextBox ID="txtPatientType" runat="server" onchange="AutoSave(this);" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td class="style38">&nbsp;
                            </td>--%>
                          <%--  <td class="style25">
                                <asp:Label ID="lblClaimMailingAddress" runat="server" Text="Claim Address" EnableViewState="false"></asp:Label>
                            </td>--%>
                            <%--<td class="style14">
                                <asp:TextBox ID="txtClaimAddress" runat="server" OnKeyPress="AutoSave(this);" Width="150px"></asp:TextBox>--%>
                                <%--<asp:TextBox ID="txtClaimAddress" runat="server" OnTextChanged="txtClaimAddress_TextChanged" onchange="AutoSave(this);" Width="150px"></asp:TextBox>--%>
                        <%--    </td>
                            <td class="style38">&nbsp;
                            </td>--%>
                           <%-- <td class="style26">
                                <asp:Label ID="lblCity" runat="server" Text="City" EnableViewState="false"></asp:Label>
                            </td>--%>
                            <%--<td>
                                <asp:TextBox ID="txtClaimCity" runat="server" OnKeyPress="AutoSave(this);" Width="150px"></asp:TextBox>
                                <%--<asp:TextBox ID="txtClaimCity" runat="server" OnTextChanged="txtClaimCity_TextChanged" onchange="AutoSave(this);" Width="150px"></asp:TextBox>--%>
<%--<%--                            </td>--%>
                        <%--</tr>
                        <tr>
                            <td class="style37">
                                <asp:Label ID="lblZipCode" runat="server" Text="Zip Code" EnableViewState="false"></asp:Label>
                            </td>--%>
                           <%-- <td class="style11">
                                <telerik:RadMaskedTextBox ID="txtZipCode" runat="server" Mask="#####-####" OnKeyPress="AutoSave(this);"
                                    Width="150px">--%>
                                <%--</telerik:RadMaskedTextBox>--%>
                                <%--<telerik:RadMaskedTextBox ID="txtZipCode" runat="server" Mask="#####-####" onchange="AutoSave(this);"
                                Width="150px">
                            </telerik:RadMaskedTextBox>--%>
                           <%-- </td>
                            <td class="style38">&nbsp;
                            </td>
                            <td class="style25">&nbsp;
                            <asp:Label ID="lblClaimState" runat="server" Text="State" EnableViewState="false"></asp:Label>
                            </td>--%>
                           <%-- <td class="style14">

                                <asp:DropDownList ID="ddlState" runat="server" Onchange="AutoSave(this);" Width="150px">
                                </asp:DropDownList>
                            </td>--%>
                          <%--  <td class="style38">&nbsp;
                            </td>
                            <td class="style26">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>--%>
                        <%--</tr>
                    </table>
                </asp:Panel>
            </div>--%>
          <%--  <div>
                <asp:Panel ID="pnlEligibilityDetails" runat="server" Font-Size="Small" CssClass="Panel"
                    BackColor="White" GroupingText="Eligibility Details" Width="782px">
                    <table style="width: 100%;">
                        <tr>--%>
                            <%--<td class="style23">
                                <asp:Label ID="lblStartDate" runat="server" Text="Eff. Start Date*" ForeColor="Red"
                                    EnableViewState="false"></asp:Label>
                            </td>--%>
                            <%--<td class="style31">
                                <telerik:RadMaskedTextBox ID="dtpEffectiveStartDate" runat="server" Mask="##-Lll-####"
                                    Width="160px">
                                    <ClientEvents OnValueChanged="EVDateValidation" />
                                    <InvalidStyle Resize="None" />
                                    <FocusedStyle Resize="None" BackColor="White" />
                                    <EmptyMessageStyle Resize="None" />
                                    <HoveredStyle Resize="None" BackColor="White" />
                                    <DisabledStyle Resize="None" />
                                    <EnabledStyle Resize="None" BackColor="White" />
                                    <ReadOnlyStyle Resize="None" />
                                </telerik:RadMaskedTextBox>--%>
                                <%--<telerik:RadDatePicker ID="dtpEffectiveStartDate" runat="server" onkeypress="AutoSave(this);"
                                Culture="English (United States)" MinDate="1900-01-01" Width="150px" EnableViewState="false">
                                <ClientEvents OnDateSelected="dtpEffectiveStartDate_OnDateSelected" />
                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                                    ShowRowHeaders="False">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today">
                                            <ItemStyle CssClass="rcToday" />
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" LabelWidth="40%">
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </DateInput>
                            </telerik:RadDatePicker>--%>
                           <%-- </td>
                            <td class="style29">
                                <asp:Label ID="lblTerminationDate" runat="server" Text="Term.Date " EnableViewState="false"></asp:Label>
                            </td>
                            <td colspan="2">
                                <telerik:RadMaskedTextBox ID="dtpTerminationDate" runat="server" Mask="##-Lll-####"
                                    Width="160px">
                                    <ClientEvents OnValueChanged="EVDateValidation" />
                                    <InvalidStyle Resize="None" />
                                    <FocusedStyle Resize="None" BackColor="White" />
                                    <EmptyMessageStyle Resize="None" />
                                    <HoveredStyle Resize="None" BackColor="White" />
                                    <DisabledStyle Resize="None" />
                                    <EnabledStyle Resize="None" BackColor="White" />
                                    <ReadOnlyStyle Resize="None" />
                                </telerik:RadMaskedTextBox>--%>
                                <%--<telerik:RadDatePicker ID="dtpTerminationDate" runat="server" onkeypress="AutoSave(this);"
                                Culture="English (United States)" MinDate="1900-01-01" Width="160px" EnableViewState="false">
                                <ClientEvents OnDateSelected="dtpTerminationDate_OnDateSelected" />
                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x"
                                    ShowRowHeaders="False">
                                    <SpecialDays>
                                        <telerik:RadCalendarDay Repeatable="Today">
                                            <ItemStyle CssClass="rcToday" />
                                        </telerik:RadCalendarDay>
                                    </SpecialDays>
                                </Calendar>
                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" LabelWidth="40%">
                                    <EmptyMessageStyle Resize="None" />
                                    <ReadOnlyStyle Resize="None" />
                                    <FocusedStyle Resize="None" />
                                    <DisabledStyle Resize="None" />
                                    <InvalidStyle Resize="None" />
                                    <HoveredStyle Resize="None" />
                                    <EnabledStyle Resize="None" />
                                </DateInput>
                            </telerik:RadDatePicker>--%>
                           <%-- </td>
                            <td class="style36">
                                <asp:Label ID="lblPCPCopay" runat="server" Text="PCP Copay $" EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPCPCopay" runat="server" OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtPCPCopay_TextChanged" Width="150px" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <%--<tr>
                            <td class="style20">
                                <asp:Label ID="lblSPCCopay" runat="server" Text="SPC Copay $" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style32">
                                <asp:TextBox ID="txtSPCCopay" runat="server" OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtSPCCopay_TextChanged" Width="150px" MaxLength="10"></asp:TextBox>
                            </td>
                            <td class="style18">
                                <asp:Label ID="lblDectible" runat="server" Text="Deductible $" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style17" colspan="2">
                                <asp:TextBox ID="txtDeductibleforThePlan" runat="server" OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtDeductibleforThePlan_TextChanged" Width="160px" MaxLength="10"></asp:TextBox>
                            </td>--%>
                            <%--<td class="style35">
                                <asp:Label ID="Label1" runat="server" Text="Deductible Met $"></asp:Label>
                            </td>
                            <td class="style17">
                                <asp:TextBox ID="txtDeductibleMet" runat="server" Width="150px" OnKeyPress="return AllowAmount(this);" MaxLength="9"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>--%>
                            <%--<td class="style23">
                                <asp:Label ID="lblCoInsurance" runat="server" EnableViewState="false"
                                    Text="Co Ins. %"></asp:Label>
                            </td>
                            <td class="style31">
                                <asp:TextBox ID="txtCoInsurance" runat="server" MaxLength="9"
                                    OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtCoInsurance_TextChanged" Width="150px"></asp:TextBox>
                            </td>--%>
                            <%--<td class="style29">
                                <asp:Label ID="lblEVType" runat="server" EnableViewState="false" Text="EV Type"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlEVType" runat="server" AutoPostBack="True"
                                    onchange="AutoSave(this);"
                                    OnSelectedIndexChanged="ddlEVType_SelectedIndexChanged"
                                    OnTextChanged="ddlEVType_TextChanged" Width="160px">
                                </asp:DropDownList>
                            </td>--%>
                           <%-- <td class="style36">
                                <asp:Label ID="lblEligibilityStatus" runat="server" EnableViewState="false"
                                    Text="EV Status"></asp:Label>
                            </td>
                            <td>--%>
                              <%--  <asp:DropDownList ID="ddlEligibilityStatus" runat="server"
                                    onchange="AutoSave(this);" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>--%>
                            <%--<td class="style23">
                                <asp:Label ID="lblCallRep" runat="server" EnableViewState="false"
                                    Text="Call Rep.Name"></asp:Label>
                            </td>
                            <td class="style31">
                                --%><%--  <asp:TextBox ID="txtCallRepresentative" runat="server" MaxLength="25" 
                                OnKeyPress="AutoSave(this);" OnTextChanged="txtCallRepresentative_TextChanged" 
                                Width="150px"></asp:TextBox>--%>
                               <%-- <asp:TextBox ID="txtCallRepresentative" runat="server" MaxLength="25"
                                    onchange="AutoSave(this);" OnTextChanged="txtCallRepresentative_TextChanged"
                                    Width="150px"></asp:TextBox>
                            </td>
                            <td class="style29">--%>
                               <%-- <asp:Label ID="lblCallRepNo" runat="server" Text="Call Ref. #"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCallReference" runat="server" MaxLength="25"
                                    onchange="AutoSave(this);" OnTextChanged="txtCallReference_TextChanged"
                                    Width="160px"></asp:TextBox>
                            </td>--%>
                           <%-- <td class="style36">

                                <asp:Label ID="lblHyperlink" runat="server" EnableViewState="false"
                                    Text="Hyperlink"></asp:Label>
                            </td>
                            <td>--%>

                            <%--    <%--<asp:TextBox ID="txtHyperlinkToWeb" runat="server" MaxLength="100" 
                                OnKeyPress="AutoSave(this);" OnTextChanged="txtHyperlinkToWeb_TextChanged" 
                                Width="150px"></asp:TextBox>--%>
                               <%-- <asp:TextBox ID="txtHyperlinkToWeb" runat="server" MaxLength="100"
                                    onchange="AutoSave(this);" OnTextChanged="txtHyperlinkToWeb_TextChanged"
                                    Width="150px"></asp:TextBox>--%>
                           <%-- </td>--%>
                       <%-- </tr>
                        <tr>
                            <td class="style23">
                                <asp:Label ID="lblComments" runat="server" Text="Message"
                                    EnableViewState="False"></asp:Label>
                            </td>--%>
                            <%--<td class="style31" colspan="6">
                                <asp:TextBox ID="txtComments" runat="server" Width="98%" OnKeyPress="AutoSave(this);"
                                    CssClass="MultiLineTextBox" TextMode="MultiLine" EnableViewState="false"></asp:TextBox>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>--%>
                       <%-- </tr>
                        <tr>
                            <td class="style23">
                                <asp:Label ID="lblAttchEv" runat="server" Text="Attach EV Sheet" EnableViewState="false"></asp:Label>
                            </td>
                            <td colspan="2">--%>

                               <%-- <telerik:RadAsyncUpload ID="UploadEV" runat="server"
                                    MultipleFileSelection="Automatic" InitialFileInputsCount="0"
                                    OnClientFileUploading="UploadImage_FileUploading" Height="22px" Width="230px">
                                    <Localization Select="Browse" />
                                </telerik:RadAsyncUpload>--%>
                           <%-- </td>
                            <td class="style41">

                                <asp:Label ID="lblFileName" runat="server" Text="File Name" CssClass="displayNone"
                                    EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style40">
                                <asp:TextBox ID="txtFileName" runat="server" BackColor="#BFDBFF" BorderWidth="1px"
                                    ReadOnly="True" OnKeyPress="AutoSave(this);" OnTextChanged="txtFileName_TextChanged"
                                    Style="margin-bottom: 2px" CssClass="displayNone" Width="100px"></asp:TextBox>
                            </td>--%>
                         <%--   <td colspan="2">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" class="style42"></td>
                        </tr>
                    </table>
                </asp:Panel>--%>
           <%-- </div>
            <div>
                <asp:Panel ID="Buttons" runat="server" Width="780px">
                    <table>
                        <tr>--%>
                            <%--<td class="style2">&nbsp;
                            <asp:HiddenField ID="hdnSaveFlag" runat="server" EnableViewState="false" />
                            </td>
                            <td class="style22">&nbsp;
                            </td>
                            <td class="style3">
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" OnClientClick="return ValidateSAve();"
                                    AccessKey="S" Text="Save" Width="95px" CssClass="aspresizedgreenbutton"  />
                            </td>--%>
                           <%-- <td>
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return CloseWindow();"
                                    AccessKey="C" Width="95px" EnableViewState="false" CssClass="aspresizedredbutton"  />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return CloseWindow();" />
            </div>--%>
           <%-- <aspx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
            </aspx:ToolkitScriptManager>
            <asp:HiddenField ID="hdnMessageType" runat="server" />
            <asp:HiddenField ID="hdnmyCarrierName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyHumanID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyPatientName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyGroupNumber" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyPolicyHolderId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnInsPlanId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnInsuranceEdited" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdndtMyTerminationDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdndtMyEffectiveDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
             <asp:HiddenField ID="hdnDOB" runat="server" EnableViewState="false" />
        </div>--%>        <%--height="385px"--%>
        <%--re-design--%>

           <div>
                 <asp:Panel ID="pnlPatientStrip" runat="server" Width="800px">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <%--<asp:TextBox ID="txtPatientstrip" runat="server" Text="" CssClass="nonEditabletxtbox"></asp:TextBox>--%>
                                 <div id="divPatientstrip" runat="server" class=" pnlBarGroup Editabletxtbox " style="height:21px; margin-bottom: 6px; margin-top: -7px; vertical-align: middle; padding-top: 2px; position: relative; padding-left: 8px; border: 0px !important "></div>
                            </td>
                        </tr>
                     </table>
                     </asp:Panel>
           </div>
         <div style="width: 785px; height: 438px;">
            <div style="margin-top: 9px; height: 368px;">
                <asp:Panel ID="pnlManualEV" runat="server" Font-Size="Small" CssClass="Panel"
                    BackColor="White" GroupingText="Manual EV" Width="800px" Height="380px" >
                    
                    <table style="width: 100%;">
                    <tr>   
                       <td class="style29">
                            <asp:Label ID="lblEVType" runat="server" EnableViewState="false" Text="EV Type"></asp:Label>
                      </td>
                      <td colspan="1">
                            <asp:DropDownList ID="ddlEVType" runat="server"   
                                  Width="160px" OnTextChanged="ddlEVType_TextChanged" AutoPostBack="false"  onchange="RefreshddlEVType(this);">
                              <%--AutoPostBack="True" OnSelectedIndexChanged="ddlEVType_SelectedIndexChanged"--%>
                            </asp:DropDownList>
                       </td>
                       <td class="style29">
                            <asp:Label ID="lblCallRepNo" runat="server" Text="Call Ref. #"></asp:Label>
                       </td>
                       <td colspan="0">
                             <asp:TextBox ID="txtCallReference" runat="server" MaxLength="25"
                                  onchange="AutoSave(this);" OnTextChanged="txtCallReference_TextChanged"
                                  Width="160px" BackColor="#BFDBFF"></asp:TextBox>
                       </td>
                       <td class="style23">
                              <asp:Label ID="lblCallRep" runat="server" EnableViewState="false"
                                   Text="Call Rep.Name"></asp:Label>
                        </td>
                        <td class="style31" colspan="1">
                                <%--  <asp:TextBox ID="txtCallRepresentative" runat="server" MaxLength="25" 
                                OnKeyPress="AutoSave(this);" OnTextChanged="txtCallRepresentative_TextChanged" 
                                Width="150px"></asp:TextBox>--%>
                               <asp:TextBox ID="txtCallRepresentative" runat="server" MaxLength="25"
                                    onchange="AutoSave(this);" OnTextChanged="txtCallRepresentative_TextChanged"
                                    Width="150px" BackColor="#BFDBFF"></asp:TextBox>
                         </td>
                       </tr>
                        <tr>
                          <td class="style25">
                                <asp:Label ID="lblPayerName" runat="server" Text="Payer Name* " ForeColor="Red"></asp:Label>
                          </td>
                           <td class="style14">
                                 <%--<asp:DropDownList ID="ddlPayerName" runat="server" AutoPostBack="True" onchange="AutoSave(this);" Width="160px"
                                      OnSelectedIndexChanged="ddlPayerName_SelectedIndexChanged" >
                                 </asp:DropDownList>--%>
                                <asp:TextBox ID="txtCarrierName" runat="server" Width="160px" BackColor="#BFDBFF"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                            
                            <td class="style25">
                                  <asp:Label ID="lblPlanName" runat="server" Text="Ins. Plan Name*" ForeColor="Red"></asp:Label>
                            </td>
                            <td class="style14">
                                  <%--<asp:DropDownList ID="ddlPlanName" runat="server" AutoPostBack="True" onchange="AutoSave(this);"
                                                    OnSelectedIndexChanged="ddlPlanName_SelectedIndexChanged" Width="160px">
                                  </asp:DropDownList>--%>
                                  <asp:TextBox ID="txtInsPlanName" runat="server" Width="160px" BackColor="#BFDBFF"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                             
                             <td class="style25">
                                  <asp:Label ID="lblClaimMailingAddress" runat="server" Text="Claim Address" EnableViewState="false"></asp:Label>
                              </td>
                            <td class="style14">
                                <asp:TextBox ID="txtClaimAddress" runat="server" OnKeyPress="AutoSave(this);" Width="150px" ReadOnly="True" BackColor="#BFDBFF"></asp:TextBox>
                                <%--<asp:TextBox ID="txtClaimAddress" runat="server" OnTextChanged="txtClaimAddress_TextChanged" onchange="AutoSave(this);" Width="150px"></asp:TextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style26">
                                <asp:Label ID="lblCity" runat="server" Text="City" EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtClaimCity" runat="server" OnKeyPress="AutoSave(this);" Width="157px" ReadOnly="True" BackColor="#BFDBFF"></asp:TextBox>
                                <%--<asp:TextBox ID="txtClaimCity" runat="server" OnTextChanged="txtClaimCity_TextChanged" onchange="AutoSave(this);" Width="150px"></asp:TextBox>--%>
                            </td>
                            <td class="style25">
                                <asp:Label ID="lblClaimState" runat="server" Text="State" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style14">
                                <asp:DropDownList ID="ddlState" runat="server" Onchange="AutoSave(this);" Width="158px" OnTextChanged="ddlState_TextChanged"  ReadOnly="True" BackColor="#BFDBFF">
                                </asp:DropDownList>
                            </td>
                            <td class="style37">
                                <asp:Label ID="lblZipCode" runat="server" Text="Zip Code" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style11">
                                <telerik:RadMaskedTextBox ID="txtZipCode" runat="server" Mask="#####-####" OnKeyPress="AutoSave(this);" OnTextChanged="txtZipCode_TextChanged"
                                    Width="151px"  ReadOnly="True" BackColor="#BFDBFF">
                                </telerik:RadMaskedTextBox>
                                <%--<telerik:RadMaskedTextBox ID="txtZipCode" runat="server" Mask="#####-####" onchange="AutoSave(this);"
                                Width="150px">
                            </telerik:RadMaskedTextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="style26">
                                <span  class="MandLabelstyle">Policy Holder Id<span class="manredforstar">*</span></span>
                               <%-- <asp:Label ID="lblPolicyHolder" runat="server" Text="Policy Holder Id*" EnableViewState="false"></asp:Label>--%>
                            </td>
                            <td>
                                 <asp:TextBox ID="txtPolicyHolderId" runat="server" OnKeyPress="AutoSave(this);" Width="157px" BackColor="#BFDBFF" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td class="style23">
                                <asp:Label ID="lblStartDate" runat="server" Text="Eff. Start Date*" ForeColor="Red"
                                    EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style31">
                                <telerik:RadMaskedTextBox ID="dtpEffectiveStartDate" runat="server" Mask="##-Lll-####" Width="160px" OnTextChanged="dtpEffectiveStartDate_TextChanged" OnKeyPress="AutoSave(this);">
                                        <ClientEvents OnValueChanged="QPCDateValidation" />
                                        <InvalidStyle Resize="None" />
                                        <FocusedStyle Resize="None" BackColor="White" />
                                        <EmptyMessageStyle Resize="None" />
                                        <HoveredStyle Resize="None" BackColor="White" />
                                        <DisabledStyle Resize="None" />
                                        <EnabledStyle Resize="None" BackColor="White" />
                                        <ReadOnlyStyle Resize="None" />
                                        </telerik:RadMaskedTextBox>
                            </td>
                            <td class="style29">
                                <asp:Label ID="lblTerminationDate" runat="server" Text="Term.Date " EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadMaskedTextBox ID="dtpTerminationDate" runat="server" Mask="##-Lll-####" OnTextChanged="dtpTerminationDate_TextChanged"
                                    Width="152px" OnKeyPress="AutoSave(this);">
                                    <ClientEvents OnValueChanged="EVDateValidation" />
                                    <InvalidStyle Resize="None" />
                                    <FocusedStyle Resize="None" BackColor="White" />
                                    <EmptyMessageStyle Resize="None" />
                                    <HoveredStyle Resize="None" BackColor="White" />
                                    <DisabledStyle Resize="None" />
                                    <EnabledStyle Resize="None" BackColor="White" />
                                    <ReadOnlyStyle Resize="None" />
                                </telerik:RadMaskedTextBox>
                            </td>
                        </tr>
                        <tr>
                             <td class="style25">
                                <asp:Label ID="lblGroupNo" runat="server" Text="Group #" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style14">
                                <asp:TextBox ID="txtGroupNumber" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="157px"></asp:TextBox>
                            </td>
                             <td class="style36">
                                <asp:Label ID="lblPCPCopay" runat="server" Text="PCP Copay $" EnableViewState="false"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPCPCopay" runat="server" OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtPCPCopay_TextChanged" Width="156px" MaxLength="7"></asp:TextBox>
                            </td>
                            <td class="style20">
                                <asp:Label ID="lblSPCCopay" runat="server" Text="SPC Copay $" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style32">
                                <asp:TextBox ID="txtSPCCopay" runat="server" OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtSPCCopay_TextChanged" Width="150px" MaxLength="7"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                             <td class="style18">
                                <asp:Label ID="lblDectible" runat="server" Text="Deduct.per Plan $" EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style17" colspan="">
                                <asp:TextBox ID="txtDeductibleforThePlan" runat="server" OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtDeductibleforThePlan_TextChanged" Width="160px" MaxLength="7"></asp:TextBox>
                            </td>
                            <td class="style35">
                                <asp:Label ID="Label1" runat="server" Text="Deduct.Met $"></asp:Label>
                            </td>
                            <td class="style17">
                                <asp:TextBox ID="txtDeductibleMet" runat="server" Width="158px" OnKeyPress="return AllowAmount(this);" MaxLength="7" OnTextChanged="txtDeductibleMet_TextChanged"></asp:TextBox>
                            </td>
                            <td class="style23">
                                <asp:Label ID="lblCoInsurance" runat="server" EnableViewState="false"
                                    Text="Co Ins. %"></asp:Label>
                            </td>
                            <td class="style31">
                                <asp:TextBox ID="txtCoInsurance" runat="server" MaxLength="7"
                                    OnKeyPress="return AllowAmount(this);"
                                    OnTextChanged="txtCoInsurance_TextChanged" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style23">
                                <asp:Label ID="lblComments" runat="server" Text="Message"
                                    EnableViewState="False"></asp:Label>
                            </td>
                            <td class="style31" colspan="5">
                                <asp:TextBox ID="txtComments" runat="server" Width="98%" OnKeyPress="AutoSave(this);"
                                    CssClass="MultiLineTextBox" TextMode="MultiLine" EnableViewState="false" OnTextChanged="txtComments_TextChanged"></asp:TextBox>
                                &nbsp; &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style23">
                               <%-- <asp:Label ID="lblAttchEv" runat="server" Text="Attach EV Sheet" EnableViewState="false"></asp:Label>--%>
                               <span id="lblAttachEV" runat="server" class="Editabletxtbox">Attach EV Sheet <i class="icon-ok-sign" style="color:blue">(File Formats supported:*.TIFF , *.PDF , *.PNG , *.JPG , *.GIF)</i></span>

                            </td>
                             <td colspan="2">
                                <div id="divUploadFile" style="height:80px;overflow-y:auto;">
                                    <telerik:RadAsyncUpload ID="UploadEV" runat="server" 
                                        MultipleFileSelection="Automatic" InitialFileInputsCount="0"
                                        OnClientFileUploading="UploadImage_FileUploading" Height="22px" Width="230px" UploadedFilesRendering="BelowFileInput">
                                        <Localization Select="Browse"  />
                                    </telerik:RadAsyncUpload>
                                </div>
                            </td>
                            <td class="style41">
                                <asp:Label ID="lblFileName" runat="server" Text="File Name" CssClass="displayNone"
                                    EnableViewState="false"></asp:Label>
                            </td>
                            <td class="style40">
                                <asp:TextBox ID="txtFileName" runat="server" BackColor="#BFDBFF" BorderWidth="1px"
                                    ReadOnly="True" OnKeyPress="AutoSave(this);" OnTextChanged="txtFileName_TextChanged"
                                    Style="margin-bottom: 2px" CssClass="displayNone" Width="100px"></asp:TextBox>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                       
                    </table>
                </asp:Panel>
            </div>

             <div>
                <asp:Panel ID="Buttons" runat="server" Width="780px">
                    <table>
                        <tr>
                            <td class="style2">&nbsp;
                                 <asp:HiddenField ID="hdnSaveFlag" runat="server" EnableViewState="false" />
                            </td>
                            <td class="style22">&nbsp;
                            </td>
                            <td class="style3" >
                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" OnClientClick="return ValidateSAve();"
                                    AccessKey="S" Text="Save" Width="95px"  style="margin-top:35px"  CssClass="aspresizedgreenbutton"  />
                            </td>
                            <td >
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return CloseWindow();"
                                    AccessKey="C" Width="95px" style="margin-top: 35px"    EnableViewState="false" CssClass="aspresizedredbutton" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return CloseWindow();" />
            </div>
             <table>
                  <tr>
                      <td class="style37">
                          <asp:Label ID="lblPatientAccountNo" runat="server" Text="Acc. #" EnableViewState="false" Visible="false"></asp:Label>
                      </td>
                      <td class="style11">
                          <asp:TextBox ID="txtPatientAccountNo" runat="server" BackColor="#BFDBFF" ReadOnly="True" Visible="false"
                               Width="150px"></asp:TextBox>
                      </td>
                      <td class="style38">&nbsp;
                      </td>
                      <td class="style25">
                          <asp:Label ID="lblPatient" runat="server" Text="Name" EnableViewState="false" Visible="false"></asp:Label>
                      </td>
                      <td class="style14">
                          <asp:TextBox ID="txtPatientName" runat="server" BackColor="#BFDBFF" ReadOnly="True" Visible="false"
                                Width="150px"></asp:TextBox>
                      </td>
                      <td class="style38">&nbsp;
                      </td>
                  </tr>
                   <tr>
                      <td class="style37">
                             <asp:Label ID="lblPatientType" runat="server" Text="Patient Type" EnableViewState="false" Visible="false"></asp:Label>
                      </td>
                      <td class="style11">
                              <asp:TextBox ID="txtPatientType" runat="server" onchange="AutoSave(this);" BackColor="#BFDBFF" ReadOnly="True" Visible="false"
                                   Width="150px"></asp:TextBox>
                       </td>
                       <td class="style38">&nbsp;
                       </td>
                       <td class="style38">&nbsp;
                       </td>
                    </tr>
                 </table>
                       <%-- <tr>
                            
                            <td class="style38">&nbsp;
                            </td>

                            <td class="style38">&nbsp;
                            </td>
                            <td class="style26">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>--%>
                       <%-- <tr>
                            <td colspan="7" class="style42"></td>
                        </tr>--%>
                
            
            <aspx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
            </aspx:ToolkitScriptManager>
            <asp:HiddenField ID="hdnMessageType" runat="server" />
            <asp:HiddenField ID="hdnmyCarrierName" runat="server" EnableViewState="false" />
             <asp:HiddenField ID="hdnmyInsPlanName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyHumanID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyPatientName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyGroupNumber" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnmyPolicyHolderId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnInsPlanId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnInsuranceEdited" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdndtMyTerminationDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdndtMyEffectiveDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
             <asp:HiddenField ID="hdnDOB" runat="server" EnableViewState="false" />
              <asp:HiddenField ID="hdnmyClaimCity" runat="server" EnableViewState="false" />
              <asp:HiddenField ID="hdnmymyState" runat="server" EnableViewState="false" />
              <asp:HiddenField ID="hdnmyZipCode" runat="server" EnableViewState="false" />
             <asp:HiddenField ID="hdnmyClaimAddress" runat="server" EnableViewState="false" />
             <%-- <asp:HiddenField ID="hdnmyPolicyHolderID" runat="server" EnableViewState="false" />--%>


        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSEligibility.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
