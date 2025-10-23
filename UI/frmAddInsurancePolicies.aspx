<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAddInsurancePolicies.aspx.cs"
    Inherits="Acurus.Capella.UI.frmAddInsurancePolicies" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <base target="_self"></base>
    <title>Add Insurance Policy</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
  <%--  <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        #form1 {
            height: 853px;
        }

        .style13 {
            height: 16px;
        }

        #frmAddInsurancePolicies {
            height: 643px;
            width: 1079px;
            margin-right: 0px;
        }

        .style39 {
            height: 16px;
            width: 174px;
        }

        .style43 {
            height: 16px;
            width: 180px;
        }

        .style47 {
            height: 16px;
            width: 151px;
        }

        .style147 {
            width: 57px;
        }

        .style202 {
            width: 173px;
        }

        .style205 {
            width:8- 174px;
        }

        .style206 {
            width: 180px;
        }

        .style207 {
            width: 151px;
        }

        .style208 {
            width: 62px;
        }

        .style209 {
            width: 95px;
        }

        .panel_with_padding {
            padding-top: 5px;
            padding-left: 0px;
            padding-right: 0px;
            padding-bottom: 5px;
        }

        .Insurancepanel_with_padding {
            -webkit-padding-before: 10px;
            -webkit-padding-end: 0px;
            -webkit-padding-after: 20px;
            -webkit-padding-start: 0px;
        }

        .style213 {
            width: 81px;
        }

        .style214 {
            width: 98px;
        }

        .style216 {
            width: 129px;
        }

        .style217 {
            width: 76px;
        }

        .style218 {
            width: 138px;
        }

        .style219 {
            width: 136px;
        }

        .style220 {
            width: 114px;
        }

        .style221 {
            width: 140px;
        }

        .style222 {
            width: 124px;
        }

        .style224 {
            width: 106px;
        }

        .style227 {
            width: 103px;
        }

        .style228 {
            width: 72px;
        }

        .style229 {
            width: 101px;
        }

        .style230 {
            width: 75px;
        }

        .style231 {
            width: 8px;
        }

        .style232 {
            width: 80px;
        }

        .style233 {
            width: 11px;
        }

        .style235 {
            width: 88px;
        }

        .style236 {
            height: 16px;
            width: 57px;
        }

        .style238 {
            height: 16px;
            width: 81px;
        }

        .style239 {
            height: 16px;
            width: 94px;
        }

        .style240 {
            width: 94px;
        }

        .style241 {
            width: 141px;
        }

        .style245 {
            height: 16px;
            width: 8px;
        }

        .style246 {
            height: 16px;
            width: 9px;
        }

        .style247 {
            width: 9px;
        }

        .Panel legend {
            font-weight: bold;
        }

        .MultiLineTextBox {
            resize: none;
        }

        .style248 {
            width: 130px;
        }
    </style>
</head>
<body  onload = "loadAddinsurance();" class="bodybackground">
    <form id="frmAddInsurancePolicies" runat="server">
        <telerik:RadWindowManager ID="AddInsuredModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="AddInsuredModalWindow" runat="server" VisibleOnPageLoad="false"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div style="height: 642px; width: 1080px;">
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <div>
                        <asp:Panel ID="pnlPatientInformation" runat="server" CssClass="Editabletxtbox" GroupingText="Patient Information   "
                            Width="1076px" Font-Size="Small" TabIndex="0" BackColor="White">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPateintRelationshipToInsured" runat="server" Text="Rel.To Insured"
                                            EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style220">
                                        <asp:DropDownList ID="ddlPatientRelation" runat="server" TabIndex="1" Height="20px"
                                            Width="111px" OnSelectedIndexChanged="ddlPatientRelation_SelectedIndexChanged1"
                                            AutoPostBack="True" CssClass="Editabletxtbox">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPatientLastName" runat="server" Text="Last Name   " EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style219">
                                        <asp:TextBox ID="txtPatientLastName" runat="server" Width="140px" ReadOnly="True"
                                            BackColor="#BFDBFF" TabIndex="2" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPatientFirstName" runat="server" Text="First Name   " EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style221">
                                        <asp:TextBox ID="txtPatientFirstName" runat="server" Width="140px" ReadOnly="True"
                                            BackColor="#BFDBFF"  TabIndex="3" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style208">
                                        <asp:Label ID="lblPolicyHolderAccountno" runat="server" Text="Acc. #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style222">
                                        <asp:TextBox ID="txtPolicyHolderAccountNo" runat="server" onchange="AutoSave();"
                                            Width="123px"  TabIndex="4" ReadOnly="True" BackColor="#BFDBFF"
                                             CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblExternalAccountNo" runat="server" Text="Ext.Acc #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExternalAccountNo" runat="server" TabIndex="5" Width="139px" 
                                            ReadOnly="True" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:Panel ID="pnlPolicyDemoInfo" runat="server" GroupingText="Policy Holder Demographics Information    "
                            CssClass="Editabletxtbox" Width="1076px" TabIndex="6" Font-Size="Small" BackColor="White">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style214">
                                        <asp:Label ID="lblPolicyHolderLastName" runat="server" Text="Last Name   " EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style202">
                                        <asp:TextBox ID="txtPolicyHolderLastName" TabIndex="7" runat="server" ReadOnly="True"
                                            Width="172px" Style="margin-left: 0px" BackColor="#BFDBFF"
                                            CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style232">
                                        <asp:Label ID="lblPolicyHolderFirstName" runat="server" Text="First Name" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style216">
                                        <asp:TextBox ID="txtPolicyHolderFirstName" ReadOnly="True" TabIndex="8" runat="server"
                                            BackColor="#BFDBFF"  Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style233">&nbsp;
                                    </td>
                                    <td class="style217">
                                        <asp:Label ID="lblPolicyHolderMiddleInitial" runat="server" Text="Middle Name" Width="80px"
                                            EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style218">
                                        <asp:TextBox ID="txtPolicyHolderMiddleName" ReadOnly="True" TabIndex="9" runat="server"
                                            Width="150px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Button ID="btnAddNewInsured" runat="server" OnClick="btnAddNewInsured_Click"
                                            AccessKey="I" TabIndex="10" OnClientClick="return openDemographicswindow();"
                                            Text="Add New Insured" Width="121px" CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSelectExistingInsured" runat="server" Text="Select Existing Insured"
                                            AccessKey="E" Width="165px" TabIndex="11" OnClick="btnSelectExistingInsured_Click"
                                            OnClientClick="return OpenFindPatient();" CssClass="aspresizedbluebutton"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style214">
                                        <asp:Label ID="lblPolicyHolderDateOfBirth" runat="server" Text="DOB" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyHolderDateOfBirth" runat="server" TabIndex="12" ReadOnly="True"
                                            Width="172px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style232">
                                        <asp:Label ID="lblPolicyHolderSex" runat="server" Text="Sex" EnableViewState="False" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style216">
                                        <asp:TextBox ID="txtPolicyHolderSex" runat="server" TabIndex="13" ReadOnly="True"
                                            BackColor="#BFDBFF"  Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style233">&nbsp;
                                    </td>
                                    <td class="style217">
                                        <asp:Label ID="lblPolicyHolderMedicalRecordNo" runat="server" Text="Med Rec.#" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style218">
                                        <asp:TextBox ID="txtPolicyHolderMedicalRecordNo" TabIndex="14" runat="server" ReadOnly="True"
                                            Width="150px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblPolicyHolderSSN" runat="server" Text="SSN" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="msktxtPolicyHolderSSN" runat="server" TabIndex="15" ReadOnly="True"
                                            BackColor="#BFDBFF" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style214">
                                        <asp:Label ID="lblPolicyHolderAddress" runat="server" Text="Address" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyHolderAddress" runat="server" TabIndex="16" ReadOnly="True"
                                            Width="172px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style232">
                                        <asp:Label ID="lblPolicyHolderCity" runat="server" Text="City" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style216">
                                        <asp:TextBox ID="txtPolicyHolderCity" runat="server" TabIndex="17" ReadOnly="True"
                                            BackColor="#BFDBFF"  Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style217">
                                        <asp:Label ID="lblPolicyHolderState" runat="server" Text="State" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style218">
                                        <asp:TextBox ID="txtPolicyHolderState" runat="server" TabIndex="18" ReadOnly="True"
                                            Width="150px" BackColor="#BFDBFF"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblPolicyHolderZipcode" runat="server" Text="Zip Code" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyHolderZipCode" runat="server" TabIndex="19" ReadOnly="True"
                                            Width="150px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style214">
                                        <asp:Label ID="lblPolicyHolderMaritalStatus" runat="server" Text="Marital Status"
                                            EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyHolderMaritalStatus" runat="server" TabIndex="20" ReadOnly="True"
                                            Width="172px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style232">
                                        <asp:Label ID="lblPolicyHolderCellPhno" runat="server" Text="Cell Ph#" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style216">
                                        <asp:TextBox ID="msktxtPolicyHolderCellPhno" runat="server" TabIndex="21" ReadOnly="True"
                                            BackColor="#BFDBFF" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style233">&nbsp;
                                    </td>
                                    <td class="style217">
                                        <asp:Label ID="lblPolicyHolderHomePhno" runat="server" Text="Home Ph#" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style218">
                                        <asp:TextBox ID="msktxtPolicyHolderHomePhno" runat="server" TabIndex="22" ReadOnly="True"
                                            Width="150px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblPolicyHolderEmail" runat="server" Text="E-Mail" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyHolderEmail" runat="server" TabIndex="23" ReadOnly="True"
                                            BackColor="#BFDBFF" Width="150px"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style214">
                                        <asp:Label ID="lblPatEmploymentStatus" runat="server" Text="Emp.Status" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPatEmploymentStatus" runat="server" ReadOnly="True" TabIndex="24"
                                            Width="172px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style232">
                                        <asp:Label ID="lblPatEmployerName" runat="server" Text="Emp. Name" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style216">
                                        <asp:TextBox ID="txtPatEmployerName" runat="server" TabIndex="25" ReadOnly="True"
                                            BackColor="#BFDBFF"  Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style233">&nbsp;
                                    </td>
                                    <td class="style217">
                                        <asp:Label ID="lblPatWorkPhone" runat="server" Text="Work Ph#" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style218">
                                        <asp:TextBox ID="msktxtPatWorkPhone" runat="server" ReadOnly="True" Width="150px"
                                            TabIndex="26" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblPatWorkExtn" runat="server" Text="Extn" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPatWorkExtn" runat="server" ReadOnly="True" BackColor="#BFDBFF"
                                            TabIndex="27" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style214">
                                        <asp:Label ID="lblPolicyHolderDriverLicenseno" runat="server" Text="Driver License"
                                            EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyHolderDriverLicenseno" runat="server" ReadOnly="True" Width="172px"
                                            TabIndex="28" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style232">
                                        <asp:Label ID="lblPolicyHolderLicenseState" runat="server" Text="License State" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style216">
                                        <asp:TextBox ID="txtPolicyHolderLicenseState" runat="server" TabIndex="29" ReadOnly="True"
                                            BackColor="#BFDBFF" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style233" >&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAccNo" runat="server" Text="Acc. #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccNo" runat="server" BackColor="#BFDBFF" 
                                            TabIndex="30"  Height="22px" onchange="AutoSave();" ReadOnly="True"
                                            Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox ID="chkAddAsGuarantor" runat="server" TabIndex="31" onclick="AutoSave(this);" style="margin-left: -5px; word-break: break-all;"
                                            Text="Add As Guarantor" EnableViewState="false" CssClass="Editabletxtbox" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:Panel ID="pnlInsuranceDetails" runat="server" CssClass="Editabletxtbox" TabIndex="32"
                            GroupingText="Insurance Plan Details  " Width="1075px" Font-Size="Small" BackColor="White">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style235">&nbsp;
                                    <asp:Button ID="btnSelectPlan" runat="server" TabIndex="33" OnClick="btnSelectPlan_Click"
                                        OnClientClick="return Open();" AccessKey="P" Text="Select Plan" Width="80px" CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td class="style39"></td>
                                    <td class="style246">&nbsp;
                                    </td>
                                    <td class="style236"></td>
                                    <td class="style43"></td>
                                    <td class="style246">&nbsp;
                                    </td>
                                    <td class="style238"></td>
                                    <td class="style47"></td>
                                    <td class="style245">&nbsp;
                                    </td>
                                    <td class="style239"></td>
                                    <td class="style13">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style235">
                                        <asp:Label ID="lblInsPlanID" runat="server" Text="Plan #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style205">
                                        <asp:TextBox ID="txtInsPlanID" runat="server" TabIndex="34" BackColor="#BFDBFF" 
                                            Width="169px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style147">
                                        <asp:Label ID="lblInsPlanName" runat="server" Text="Ins. Plan Name" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style206">
                                        <asp:TextBox ID="txtInsPlanName" runat="server" TabIndex="35" BackColor="#BFDBFF"
                                           ReadOnly="True" Width="185px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblInsCarrierName" runat="server" Text="Carrier Name" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style207">
                                        <asp:TextBox ID="txtInsCarrierName" runat="server" TabIndex="36" BackColor="#BFDBFF"
                                            ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style240">
                                        <asp:Label ID="lblInsFinancialClass" runat="server" Text="Fin. Class" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInsFinancialClass" runat="server" TabIndex="37" BackColor="#BFDBFF"
                                             ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style235">
                                        <asp:Label ID="lblInsAddress" runat="server" Text="Address" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style205">
                                        <asp:TextBox ID="txtInsAddress" runat="server" TabIndex="38" Width="169px" BackColor="#BFDBFF"
                                            ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style147">
                                        <asp:Label ID="lblInsCity" runat="server" Text="City" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style206">
                                        <asp:TextBox ID="txtInsCity" runat="server" TabIndex="39" ReadOnly="True" Width="185px"
                                            BackColor="#BFDBFF"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblInsState" runat="server" Text="State" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style207">
                                        <asp:TextBox ID="txtInsState" runat="server" TabIndex="40" ReadOnly="True" Width="160px"
                                            BackColor="#BFDBFF"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style240">
                                        <asp:Label ID="lblInsZip" runat="server" Text="Zip Code" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInsZipCode" runat="server" TabIndex="41" BackColor="#BFDBFF"
                                            ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style235">
                                        <asp:Label ID="lblInsPhone" runat="server" Text="Phone #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style205">
                                        <asp:TextBox ID="mskInsPhone" runat="server" TabIndex="42" ReadOnly="True" Width="169px"
                                            BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style147">
                                        <asp:Label ID="lblInsFax" runat="server" Text="Fax #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style206">
                                        <asp:TextBox ID="msktxtInsFax" runat="server" TabIndex="43" ReadOnly="True" Width="185px"
                                            BackColor="#BFDBFF"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblInsAssignment" runat="server" Text="Assignment" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style207">
                                        <asp:DropDownList ID="cboInsAssignment" runat="server" TabIndex="44" OnSelectedIndexChanged="cboInsAssignment_SelectedIndexChanged"
                                            Width="160px" BackColor="#BFDBFF" Enabled="False" CssClass="nonEditabletxtbox">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style240">
                                        <asp:Label ID="lblInsGovtType" runat="server" Text="Govt.Type" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInsGovtType" runat="server" TabIndex="45" BackColor="#BFDBFF"
                                            ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style235">
                                        <asp:Label ID="lblClaimMailingAddress" runat="server" Text="Claim Address" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style205">
                                        <asp:TextBox ID="txtClaimAddress" runat="server" TabIndex="46" ReadOnly="True" Width="169px"
                                            BackColor="#BFDBFF"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style147">
                                        <asp:Label ID="lblClaimCity" runat="server" Text="City" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style206">
                                        <asp:TextBox ID="txtClaimCity" runat="server" TabIndex="47" ReadOnly="True" Width="185px"
                                            BackColor="#BFDBFF"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style247">&nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblClaimState" runat="server" Text="State" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style207">
                                        <asp:TextBox ID="txtClaimState" runat="server" TabIndex="48" BackColor="#BFDBFF"
                                             ReadOnly="True" Width="163px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style231">&nbsp;
                                    </td>
                                    <td class="style240">
                                        <asp:Label ID="lblClaimZipCode" runat="server" Text="Zip Code" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZipCode" runat="server" BackColor="#BFDBFF" TabIndex="49" 
                                            ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="width: 1080px;">
                        <asp:Panel ID="pnlPolicyHolderInfo" runat="server" CssClass="Editabletxtbox" TabIndex="50"
                            GroupingText="Policy Holder Information" Width="1075px" Font-Size="Small" BackColor="White">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style227">
                                       <%-- <asp:Label ID="lblPolicyHolderId" runat="server" Text="Policy Holder ID *"
                                            EnableViewState="false"  mand="Yes"></asp:Label>--%>
                                        <span id="sPolicyHolderId" runat="server" class="MandLabelstyle">Policy Holder ID</span><span id="sPolicyMan" runat="server" class="manredforstar">*</span>
                                    </td>
                                    <td class="style241">
                                         <asp:TextBox ID="txtPolicyHolderID" runat="server" TabIndex="51" OnKeyPress="AutoSave(this);"
                                        Width="150px" AutoPostBack="True" OnTextChanged="txtPolicyHolderID_TextChanged"
                                        BorderWidth="1px" BackColor="#BFDBFF" MaxLength="25" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                        <%--<asp:TextBox ID="txtPolicyHolderID" runat="server" TabIndex="51" onchange="AutoSave(this);"
                                            Width="150px" AutoPostBack="True" OnTextChanged="txtPolicyHolderID_TextChanged"
                                           MaxLength="25" onblur="return Validatealphanumaeric(this);" ></asp:TextBox>--%>
                                    </td>
                                    <td class="style228">
                                        <asp:Label ID="lblGroupNumber" runat="server" Text="Group #" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style224">
                                        <%--<asp:TextBox ID="txtGroupNumber" runat="server" TabIndex="52" OnKeyPress="AutoSave(this);"
                                        Width="150px" BackColor="#BFDBFF" BorderWidth="1px" MaxLength="25" ReadOnly="true"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtGroupNumber" runat="server" TabIndex="52" onchange="AutoSave(this);"
                                            OnTextChanged="txtGroupNumber_TextChanged" Width="150px" BackColor="#BFDBFF"
                                         MaxLength="25" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style229">
                                        <asp:Label ID="lblPCP" runat="server" Text="PCP" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="3">
                    
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class="style248"> <%-- Width="130px"--%>
                                                        <asp:TextBox ID="txtPCP" TabIndex="53" runat="server" AccessKey="R" OnKeyPress="ClearPcpIdTag();"
                                                            Width="147px" ReadOnly="true" CssClass="nonEditabletxtbox" style="margin-left:-3px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPCPNPI" runat="server" Text="PCP NPI" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPCPNPI" TabIndex="54" runat="server" Width="60px" MaxLength="10"
                                                            onkeypress="return isNumberKey(event);" ReadOnly="true" CssClass="nonEditabletxtbox" style="margin-right:-9px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnFindPCP" runat="server" Enabled="False" TabIndex="55" OnClientClick="OpenRereralPhysician();"
                                                            Text="Find PCP" CssClass="aspresizedbluebutton" style="margin-left:13px"/>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPerformEligibilityVerification" runat="server" AccessKey="V" Enabled="False"
                                                            OnClick="btnPerformEligibilityVerification_Click" TabIndex="56" OnClientClick="return OpenEligibilityWindow();"
                                                            Text="Manual EV" CssClass="aspresizedbluebutton"  style="margin-right:-9px" /> <%-- style="margin-right:-14px"--%>
                                                    </td>
                                                </tr>
                                            </table>
                                      
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style227">
                                        <asp:Label ID="lblEffectiveStartDate" runat="server" Text="Eff. Start Date" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style241">
                                        <asp:TextBox ID="dtpEffectiveStartDate" runat="server" TabIndex="57" BackColor="#BFDBFF"
                                            Width="150px"  ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style228">
                                        <asp:Label ID="lblTerminationDate" runat="server" Text="Term. Date" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style224">
                                        <asp:TextBox ID="dtpTerminationDate" runat="server" TabIndex="58" BackColor="#BFDBFF"
                                            Width="150px" ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style229">
                                        <asp:Label ID="lblPCPCoPay" runat="server" Text="PCP Copay           $" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td atomicselection="False" colspan="0">
                                        <asp:TextBox ID="txtPCPCopay" runat="server" TabIndex="59" BackColor="#BFDBFF" ReadOnly="True"
                                            Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style230">
                                        <asp:Label ID="lblSPCCoPay" runat="server" Text="SPC Copay                 $" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSPCCopay" runat="server" TabIndex="60" BackColor="#BFDBFF" ReadOnly="True"
                                            Width="150px"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style227">
                                        <asp:Label ID="lblDeductible" runat="server" Text="Deduct. $" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style241">
                                        <asp:TextBox ID="txtDeductible" runat="server" TabIndex="61" BackColor="#BFDBFF"
                                            ReadOnly="True" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style228">
                                        <asp:Label ID="lblDeductMet" runat="server" Text="Deduct Met $" Width="80px" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style224">
                                        <asp:TextBox ID="txtDeductibleMet" runat="server" BackColor="#BFDBFF" TabIndex="62"
                                             ReadOnly="True" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style229">
                                        <asp:Label ID="lblCoInsurance" runat="server" EnableViewState="false" Text="Co Ins. %" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCoInsurance" runat="server" BackColor="#BFDBFF" TabIndex="63"
                                             ReadOnly="True" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style230">
                                        <asp:Label ID="lblVerificationMode" runat="server" EnableViewState="false" Text="EV. Mode" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVerificationMode" runat="server" BackColor="#BFDBFF" TabIndex="64"
                                            ReadOnly="True" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style227">
                                        <asp:Label ID="lblCallRepName" runat="server" Text="Call Rep Name" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style241">
                                        <asp:TextBox ID="txtCallRepName" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                            TabIndex="65" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style228">
                                        <asp:Label ID="lblCallRefereneceNo" runat="server" Text="Call Ref.#" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style224">
                                        <asp:TextBox ID="txtCallRefNumber" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                            TabIndex="66" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style229">
                                        <asp:Label ID="lblEligibilityStatus" runat="server" Text="EV.Status" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEligibilityStatus" runat="server" TabIndex="67" BackColor="#BFDBFF"
                                             ReadOnly="True" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style230">
                                        <asp:Label ID="lblAttention" runat="server" EnableViewState="false" Text="Attn." CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAttention" runat="server" BackColor="#BFDBFF" TabIndex="68" 
                                            ReadOnly="True" Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style227">
                                        <asp:Label ID="lblEligibilityVerifiedBy" runat="server" Text="EV by" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style241">
                                        <asp:TextBox ID="txtEligibilityVerifiedBy" runat="server" TabIndex="69" BackColor="#BFDBFF"
                                            ReadOnly="True"  Width="150px" CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td style="margin-left: 40px" class="style228">
                                        <asp:Label ID="lblEligibilityVerificationDate" runat="server" Text="EV.date" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style224">
                                        <asp:TextBox ID="txtEligibilityVerificationDate" runat="server" TabIndex="70" BackColor="#BFDBFF"
                                            ReadOnly="True" Width="150px"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style229">
                                        <asp:Label ID="lblCarrierNameFromEligVerification" runat="server" Text="Carrier from EV."
                                            EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCarrierNameEligVerification" runat="server" TabIndex="71" BackColor="#BFDBFF"
                                            ReadOnly="True" Width="150px"  CssClass="nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                    <td class="style230">&nbsp;
                                    <asp:Label ID="lblFileName" runat="server" EnableViewState="false" Text="File Name" CssClass="Editabletxtbox"
                                        style="margin-left:-7px"></asp:Label>
                                    </td>
                                    <td>&nbsp;
                                    <asp:TextBox ID="txtFileName" runat="server" BackColor="#BFDBFF" TabIndex="72" 
                                        ReadOnly="True" Width="151px" CssClass="nonEditabletxtbox" style="margin-left:-8px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style227">
                                        <asp:Label ID="lblComments" runat="server" Text="Comments" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="7">
                                        <asp:TextBox ID="txtComments" runat="server" BackColor="#BFDBFF" 
                                            TabIndex="73" CssClass="nonEditabletxtbox" ReadOnly="True" TextMode="MultiLine" style="resize: none;"
                                            Width="943px" ></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <asp:Panel ID="PnlButton" runat="server" TabIndex="74" Width="1073px">
                        <table style="width: 100%; height: 10px">
                            <tr>
                                <td class="style209" style="display: none">
                                    <asp:Button ID="btnViewImage" runat="server" TabIndex="75" Text="View Image" EnableViewState="false" />
                                </td>
                                <td></td>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSelectPlanForSelf" runat="server" TabIndex="76" OnClick="btnSelectPlanForSelf_Click"
                                        Style="display: none" Text="Button" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style147" dir="rtl" unselectable="off" valign="middle">
                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" TabIndex="77" OnClientClick="return Validation();"
                                        AccessKey="S" Style="margin-left: 0px" Text="Save" Width="119px" CssClass="aspresizedgreenbutton"/>
                                </td>
                                <td class="style147" dir="rtl" unselectable="off" valign="middle">
                                    <asp:Button ID="btnClose" runat="server" OnClientClick="return CloseWindows();" Text="Close"
                                        TabIndex="78" AccessKey="C" Width="119px" EnableViewState="false" CssClass="aspresizedredbutton"/>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Button ID="btnAddNewInsuredRefresh" runat="server"  OnClick="btnAddNewInsuredRefresh_Click" style="display:none !important"/>
                    <asp:Button ID="btnSelectPatientRefresh" runat="server" OnClick="btnSelectPatientRefresh_Click" style="display:none !important"/>
                    <asp:Button ID="btnLoadPlan" runat="server"  OnClick="btnLoadPlan_Click" style="display:none !important"/>
                    <asp:Button ID="btnPerformEligibility" runat="server"  OnClick="btnPerformEligibility_Click" style="display:none !important"/>
                    <asp:HiddenField ID="txtHolderTag" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnSelfPlan" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="txtPcpTag" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnMyHumanID" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnbUpdatePolicy" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnSaveFlag" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnPatientType" runat="server" EnableViewState="false" />
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
                        <Scripts>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                        </Scripts>
                    </asp:ToolkitScriptManager>
                    <asp:HiddenField ID="hdnFindPatientID" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnInsuredHumanID" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnPatInusredPlanId" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnCurrentProcess" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
                    <asp:HiddenField ID="hdnTxtPCP" runat="server" EnableViewState="false" />
                     <asp:HiddenField ID="hdnCarrierID" runat="server" EnableViewState="false" />
                    <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
                        OnClientClick=" return CloseWindows();" />
                    <asp:HiddenField ID="hdnMessageType" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSAddInsPolicy.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
