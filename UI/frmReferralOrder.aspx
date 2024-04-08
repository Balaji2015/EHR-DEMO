<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmReferralOrder.aspx.cs"
    Inherits="Acurus.Capella.UI.frmReferralOrder" EnableEventValidation="false"  ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/CustomPhrases.ascx" TagName="Phrases" TagPrefix="Phrases" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
  


   <%-- <link href="CSS/style.css" rel="stylesheet" type="text/css" />--%>
  
    <%--<link href="CSS/bootstrap.min.css" rel="Stylesheet" />--%>
    <title>Referral Order</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .Panel {
            background-color: White;
        }

            .Panel legend {
                font-weight: bold !important;
            }

            /*margin-top: -14px !important;
        }*/
        /*.modal
        {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        .loading
        {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }*/
        .displayNone {
            display: none !important;
        }

        .radioWithProperWrap input {
            float: left;
        }

        .radioWithProperWrap label {
            margin-left: 0px;
            display: flex;
            margin-top: 3px;
        }

        .style2 {
            width: 18%;
        }

        .style3 {
            width: 19%;
        }

        .underline {
            text-decoration: underline;
        }

        .modal-open {
            overflow: hidden;
        }

        .modal {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1050;
            display: none;
            overflow: hidden;
            -webkit-overflow-scrolling: touch;
            outline: 0;
        }

            .modal.fade .modal-dialog {
                -webkit-transition: -webkit-transform .3s ease-out;
                -o-transition: -o-transform .3s ease-out;
                transition: transform .3s ease-out;
                -webkit-transform: translate(0, -25%);
                -ms-transform: translate(0, -25%);
                -o-transform: translate(0, -25%);
                transform: translate(0, -25%);
            }

            .modal.in .modal-dialog {
                -webkit-transform: translate(0, 0);
                -ms-transform: translate(0, 0);
                -o-transform: translate(0, 0);
                transform: translate(0, 0);
            }

        .modal-open .modal {
            overflow-x: hidden;
            overflow-y: auto;
        }

        .modal-dialog {
            position: relative;
            width: auto;
            margin: 10px;
        }

        .modal-content {
            position: relative;
            background-color: #fff;
            -webkit-background-clip: padding-box;
            background-clip: padding-box;
            border: 1px solid #999;
            border: 1px solid rgba(0, 0, 0, .2);
            border-radius: 6px;
            outline: 0;
            -webkit-box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
            box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
        }

        .modal-backdrop {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1040;
            background-color: #000;
        }

            .modal-backdrop.fade {
                filter: alpha(opacity=0);
                opacity: 0;
            }

            .modal-backdrop.in {
                filter: alpha(opacity=50);
                opacity: .5;
            }

        .modal-header {
            padding: 15px;
            border-bottom: 1px solid #e5e5e5;
        }

            .modal-header .close {
                margin-top: -2px;
            }

        .modal-title {
            margin: 0;
            line-height: 1.42857143;
        }

        .modal-body {
            position: relative;
            padding: 15px;
        }

        .modal-footer {
            padding: 15px;
            text-align: right;
            border-top: 1px solid #e5e5e5;
        }

            .modal-footer .btn + .btn {
                margin-bottom: 0;
                margin-left: 5px;
            }

            .modal-footer .btn-group .btn + .btn {
                margin-left: -1px;
            }

            .modal-footer .btn-block + .btn-block {
                margin-left: 0;
            }

        .modal-scrollbar-measure {
            position: absolute;
            top: -9999px;
            width: 50px;
            height: 50px;
            overflow: scroll;
        }

        @media (min-width: 768px) {
            .modal-dialog {
                width: 600px;
                margin: 30px auto;
            }

            .modal-content {
                -webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
                box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
            }

            .modal-sm {
                width: 300px;
            }
        }

        @media (min-width: 992px) {
            .modal-lg {
                width: 900px;
            }
        }

        .modal-header:before,
        .modal-header:after,
        .modal-footer:before,
        .modal-footer:after {
            display: table;
            content: " ";
        }

        .modal-header:after,
        .modal-footer:after {
            clear: both;
        }
        .close {
  float: right;
  font-size: 21px;
  font-weight: bold;
  line-height: 1;
  color: #000;
  text-shadow: 0 1px 0 #fff;
  filter: alpha(opacity=20);
  opacity: .2;
}
.close:hover,
.close:focus {
  color: #000;
  text-decoration: none;
  cursor: pointer;
  filter: alpha(opacity=50);
  opacity: .5;
}
button.close {
  -webkit-appearance: none;
  padding: 0;
  cursor: pointer;
  background: transparent;
  border: 0;
}
    </style>
<link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />

</head>
<body onload="OnLoadReferral()" style="margin: 0px; padding: 0px;" >
    <form id="form1" runat="server" style="height: 765px;">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Title="" Modal="true"
                    VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico" EnableViewState="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="WindowMngr1" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="RadWindow11" runat="server" Height="577" Behaviors="Close" Title="" Modal="true"
                    VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div style="height: 100%; width: 100%;">
            <telerik:RadAjaxPanel ID="pnlRefresh" runat="server" Width="100%">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 15%">
                            <asp:Panel ID="pnlDiagnosis" runat="server" Height="469px" GroupingText="Diagnosis"
                                CssClass="LabelStyleBold" Width="100%" Font-Size="Small" BorderColor="White" >
                                <table width="100%" style="height: 392px">
                                    <tr>
                                        <td style="width: 100px">
                                            <asp:Label ID="lblSelect" runat="server" Width="150px" Text="Select One or More ICD's" Font-Bold="false" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td style="width: 27%">
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" Text="Select All" Width="100%" Height="22px" Font-Bold="false"  CssClass="Editabletxtbox"
                                                AutoPostBack="false"  EnableViewState="false" onchange=" CheckBoxListSelect();" />
                                        </td>
                                        <td style="width: 10%">
                                            <asp:ImageButton ID="imgDiagnosis" runat="server" ToolTip="All Diagnosis" ImageUrl="~/Resources/Database Inactive.jpg"
                                                OnClientClick="return OpenSpecialityDiagonsis();" Width="88%" Height="20px" OnClick="imgDiagnosis_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlAssessmentProblemList" runat="server" Height="345px" GroupingText="Assessment/ProblemList"  CssClass="LabelStyleBold"
                                                BackColor="White" Style="margin-top: 0px" Width="250px" Font-Size="Small" ScrollBars="Auto">
                                                <asp:CheckBoxList ID="chklstAssessment" runat="server" Width="100%" Font-Bold="false" onclick="onSplInstructionChecked();"
                                                    Height="19px">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width: 85%">
                            <table style="width: 100%; height: 351px;">
                                <tr>
                                    <td style="width: 50%">
                                        <asp:Panel ID="pnlReferredTo" runat="server" GroupingText="Referred To" Width="100%"
                                            Height="221px" Font-Size="Small" CssClass="LabelStyleBold" BackColor="White">
                                            <table style="width: 100%; height: 187px;">
                                                <tr>
                                                    <td style="width: 15%">
                                                        <asp:Label ID="lblProviderName" runat="server" Text="Provider Name*" Width="100%"
                                                            Font-Size="Small"  mand="Yes"></asp:Label>
                                                    </td>
                                                    <td style="width: 8%">
                                                        <telerik:RadTextBox ID="txtProviderName" runat="server" Width="165px" Font-Size="Small"
                                                            MaxLength="100" onkeypress="return LettersWithSpaceOnly(event)" CssClass="Editabletxtbox">
                                                            <ClientEvents OnKeyPress="txtProviderName_OnKeyPress" />
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td style="width: 5%">
                                                        <telerik:RadButton ID="btnFindPhysician" runat="server" Text="Find Provider" Width="100%" 
                                                            Font-Size="Small" OnClick="btnFindPhysician_Click" OnClientClicked="btnFindPhysician_Clicked"
                                                            AccessKey="F" Style="top: 0px; left: 0px; text-align: center; height: 32px !important; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                                            <ContentTemplate>
                                                                <span >F</span>ind Provider
                                                            </ContentTemplate>
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td class="style3">&nbsp;
                                                    </td>
                                                    <td style="width: 10%">&nbsp;
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class="style3">
                                                        <asp:Label ID="lblSpeciality" runat="server" Text="Specialty*" Width="38%" Font-Size="Small"
                                                            mand="Yes"></asp:Label>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <telerik:RadComboBox ID="cboSpecialty" runat="server" Font-Size="Small" Width="100%"
                                                            AllowCustomText="True" MaxHeight="150px" onkeypress="EnableSaveReferralOrder"
                                                            MaxLength="50" OnClientSelectedIndexChanged="EnableSaveReferralOrder" CssClass="Editabletxtbox" >
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:Label ID="lblFacilityName" runat="server" Text="Facility Name" Width="100%" CssClass="Editabletxtbox"
                                                            Font-Size="Small" Height="16px"></asp:Label>
                                                    </td>
                                                    <td style="width: 8%">
                                                        <telerik:RadComboBox ID="cboFacilityName" runat="server" Width="120px" Font-Size="Small" OnClientSelectedIndexChanged="StartLoadFromPatChart" CssClass="Editabletxtbox"
                                                            AutoPostBack="True" OnSelectedIndexChanged="cboFacilityName_SelectedIndexChanged"
                                                            MaxLength="50" AllowCustomText="true">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td style="width: 8%">
                                                        <asp:Label ID="lblFacilityAddress" runat="server" Text="Facility Address" Width="100%" CssClass="Editabletxtbox"
                                                            Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td style="width: 20%">
                                                        <telerik:RadTextBox ID="txtReferredToFacilityAddress" runat="server" Width="100%" CssClass="Editabletxtbox"
                                                            Font-Size="Small" MaxLength="1000" onkeypress="EnableSaveReferralOrder">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style3">
                                                        <asp:Label ID="lblFacilitycity" runat="server" Text="Facility City" Width="100%" CssClass="Editabletxtbox"
                                                            Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <telerik:RadTextBox ID="txtReferredToFacilityCity" runat="server" Width="100%" MaxLength="35" CssClass="Editabletxtbox">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:Label ID="lblFacilityState" runat="server" Text="Facility State" Width="100%" CssClass="Editabletxtbox"
                                                            Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td style="width: 8%">
                                                        <telerik:RadTextBox ID="txtReferredToFacilityState" runat="server" Width="120px" Font-Size="Small"  CssClass="Editabletxtbox"
                                                            MaxLength="2" onkeypress="return AllowalphabetsReferralOrder(event)">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <%-- <td style="width: 17%">
                                                    <asp:Label ID="lblCountry" runat="server" Text="Country" Width="100%" Font-Size="Small"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboCountry" runat="server" Width="100%" Font-Size="Small"
                                                        MaxLength="2">
                                                    </telerik:RadComboBox>
                                                </td>--%>
                                                    <td style="width: 12%">
                                                        <asp:Label ID="lblFacilityZip" runat="server" Text="Facility Zip" Width="100%" Font-Size="Small"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td style="width: 12%">
                                                        <telerik:RadMaskedTextBox ID="msktxtFacilityZipCode" runat="server" Width="100%"  CssClass="Editabletxtbox"
                                                            Mask="#####-####" DisplayText="" LabelWidth="40%" 
                                                            Text='<%# DataBinder.Eval( Container, "DataItem.ZipCode" ) %>' value="">
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style3">
                                                        <asp:Label ID="lblPhonenumber" runat="server" Text="Phone #" Width="100%" Font-Size="Small"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td style="width: 20%">
                                                        <telerik:RadMaskedTextBox ID="msktxtFacilityPhoneNumber" runat="server" Width="100%"  CssClass="Editabletxtbox"
                                                            Mask="(###) ###-####" DisplayText="" LabelWidth="40%" 
                                                            Text='<%# DataBinder.Eval( Container, "DataItem.Phone" ) %>' value="">
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:Label ID="lblFaxNo" runat="server" Text="Fax #" Width="100%" Font-Size="Small"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadMaskedTextBox ID="msktxtFacilityFaxNumber" runat="server" Width="120px"  CssClass="Editabletxtbox"
                                                            Font-Size="Small" Mask="(###) ###-####" DisplayText="" LabelWidth="40%" 
                                                            Text='<%# DataBinder.Eval( Container, "DataItem.Fax" ) %>' value="">
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style3">
                                                        <asp:Label ID="lblReferralDate" runat="server" Text="Referral Date (Valid From) "
                                                            Width="74%" Height="33px" Font-Size="Small"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <telerik:RadTextBox ID="txtReferralDate" runat="server" Width="100%" Font-Size="Small"
                                                            ForeColor="Black" ReadOnly="True" onkeypress="EnableSaveReferralOrder">
                                                            <ReadOnlyStyle BackColor="#BFDBFF" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                ForeColor="Black"  CssClass="nonEditabletxtbox" />
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:Label ID="lblValidTill" runat="server" Text="Valid Till" Width="100%" Height="19px" CssClass="Editabletxtbox"
                                                            Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td style="width: 8%">
                                                        <telerik:RadDatePicker ID="dtpValidTill" runat="server" Width="120px" CssClass="Editabletxtbox">
                                                            <DateInput ID="DateInput1" runat="server" DateFormat="dd-MMM-yyyy">
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:Label ID="lblNumberOfVisits" runat="server" Text="Number of Visit" Width="100%" CssClass="Editabletxtbox"
                                                            Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <telerik:RadTextBox ID="txtNumberofVisit" runat="server" Width="100%" Font-Size="Small" CssClass="Editabletxtbox"
                                                            onkeypress="return AllowNumbers(event)" MaxLength="2">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style3">
                                                        <asp:Label ID="lblAuthorizationRequired" runat="server" Text="Authorization Required?" CssClass="Editabletxtbox"
                                                            Width="71%" Height="32px" Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButton ID="rbtnYes" runat="server" Text="Yes" CssClass="Editabletxtbox" onclick="OnAuthorizationYesClicked();" /></td>
                                                                <td>
                                                                    <asp:RadioButton ID="rbtnNo" runat="server" Text="No" CssClass="Editabletxtbox" onclick="OnAuthorizationNoClicked();" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:Label ID="lblAuthorizationNumber" runat="server" Text="Authorization #" Width="100%" CssClass="Editabletxtbox"
                                                            Height="19px" Font-Size="Small"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <telerik:RadTextBox ID="txtAuthorizationNumber" runat="server" Width="120px" Font-Size="Small" CssClass="Editabletxtbox"
                                                            onkeypress="return AllowNumbers(event)">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkMoveToMA" runat="server" Text="Move To MA" Width="100%" Font-Size="Small" CssClass="Editabletxtbox"
                                                            onclick="EnableSaveReferralOrder();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%">
                                        <asp:Panel ID="pnlReferredDetails" runat="server" GroupingText="Referred Details"
                                            Width="100%" Height="200px" Font-Size="Small" CssClass="LabelStyleBold">
                                            <div id="divpnlReferredDetails" runat="server" style="height: 170px; width: 100%; position: relative; margin-top: 10px; background-color: White; border-width: thin;"
                                                enableviewstate="false">
                                                <table style="height: 180px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblReasonForReferral" runat="server" Width="100%" Text="Reason for Referral*"
                                                                Height="19px"  mand="Yes"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <DLC:DLC ID="txtReasonForReferral" runat="server" TextboxHeight="25px" TextboxWidth="600px" ListboxHeight="75px"  Value="REASON FOR REFERRALS" />
                                                            <%--<asp:PlaceHolder ID="pnlReasonForReferral" runat="server" EnableViewState="false"/>--%>
                                                        </td>
                                                        <%-- <td>
                                                    <Phrases:Phrases ID="pbReasonForReferral" runat="server" MyFieldName="REASON FOR REFERRALS"
                                                        Height="12px" Width="12px" />
                                                </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblServiceRequested" runat="server" Text="Service Requested" Width="100%" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <DLC:DLC ID="txtServiceRequested" runat="server" TextboxHeight="25px" TextboxWidth="600px"  ListboxHeight="75px" CssClass="Editabletxtbox"
                                                                Value="SERVICE REQUESTED" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSpecialNeeds" runat="server" Text="Special Needs" Width="100%" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <DLC:DLC ID="txtSpecialNeeds" runat="server" TextboxHeight="25px" TextboxWidth="600px"  ListboxHeight="75px" CssClass="Editabletxtbox" 
                                                                Value="SPECIAL NEEDS" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblOtherComments" runat="server" Text="Other Comments" Width="100%" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <DLC:DLC ID="txtOtherComments" runat="server" TextboxHeight="25px" TextboxWidth="600px"  ListboxHeight="75px" CssClass="Editabletxtbox" 
                                                                Value="OTHER COMMENTS" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlbtn" runat="server">
                                            <table width="100%">
                                                <tr>
                                                    <td style="width: 70%;">
                                                        <telerik:RadButton ID="btnPlan" runat="server" Text="Plan" OnClick="btnPlan_Click"
                                                            OnClientClicked="btnPlan_Clicked" AccessKey="L"
                                                            Style="position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; display: none;"
                                                            Width="40px">
                                                            <ContentTemplate>
                                                                P<span >l</span>an
                                                            </ContentTemplate>
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td style="width: 5%;">
                                                        <telerik:RadButton ID="btnAddRefOrder" runat="server" Text="Add" OnClick="btnAddRefOrder_Click"
                                                            OnClientClicked="refOrderValidation" AccessKey="A" Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: 10px;" ButtonType="LinkButton" CssClass="greenbutton"
                                                            Width="75px">
                                                            <ContentTemplate>
                                                                <span id="SpanAdd" runat="server" >A</span><span id="SpanAdditionalword" runat="server">dd</span>
                                                            </ContentTemplate>
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td style="width: 5%;">
                                                        <telerik:RadButton ID="btnClearAllRefOrder" runat="server" Text="Clear All" AutoPostBack="false" 
                                                            OnClientClicked="btnClearAllRefOrder_Clicked" AccessKey="C" Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: 10px;" ButtonType="LinkButton" CssClass="redbutton"
                                                            Width="82px">
                                                            <ContentTemplate>
                                                                <span id="SpanClear" runat="server" >C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                                            </ContentTemplate>
                                                        </telerik:RadButton>
                                                    </td>
                                                    <td style="width: 5%;">
                                                        <telerik:RadButton ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click"
                                                            OnClientClicked="btnPrint_Clicked" AccessKey="P" Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: 10px;" ButtonType="LinkButton" CssClass="bluebutton"
                                                            Width="67px">
                                                            <ContentTemplate>
                                                                <span>P</span>rint
                                                            </ContentTemplate>
                                                        </telerik:RadButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlReferralDetails" runat="server" Width="100%" Height="200px" GroupingText="Referral Details"
                                CssClass="LabelStyleBold" Font-Size="Small" ScrollBars="None">
                                <telerik:RadGrid ID="grdReferralOrders" runat="server" AutoGenerateColumns="False"  CssClass="Gridbodystyle"
                                    Height="180px"  CellSpacing="0" GridLines="Both" Style="margin-bottom: 0px; overflow: auto;"
                                    OnItemCommand="grdReferralOrders_ItemCommand" OnItemCreated="grdReferralOrders_ItemCreated">
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                    <HeaderStyle Font-Bold="true" BackColor="#a4d9ff" CssClass="Gridheaderstyle" />
                                    <ClientSettings EnablePostBackOnRowClick="False">
                                        <Selecting AllowRowSelect="true" />
                                        <ClientEvents OnCommand="grdReferralOrders_OnCommand" />
                                        <Scrolling AllowScroll="false" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                        <%--<Resizing AllowColumnResize="true"  ResizeGridOnColumnResize="true" />--%>
                                    </ClientSettings>
                                    <MasterTableView TableLayout="Fixed" ToolTip="">
                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                            </EditColumn>
                                        </EditFormSettings>
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="False">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn Created="True" FilterControlAltText="Filter ExpandColumn column"
                                            Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="EditC" FilterControlAltText="Filter Edit column"
                                                HeaderStyle-Width="100%" HeaderText="Edit" ImageUrl="~/Resources/edit.GIF" ItemStyle-Width="100%" ItemStyle-Font-Bold="false"
                                                UniqueName="Edit">
                                                <HeaderStyle Width="50px" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="100%"  />
                                                <ItemStyle  BorderStyle="Dotted"  />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Del" FilterControlAltText="Filter Del column"
                                                HeaderStyle-Width="100%" HeaderText="Del" ImageUrl="~/Resources/close_small_pressed.png" ItemStyle-Font-Bold="false"
                                                ItemStyle-Width="100%" UniqueName="Del" >
                                                <HeaderStyle Width="50px" CssClass="Gridheaderstyle" />
                                                <ItemStyle Width="100%" />
                                                <ItemStyle BorderStyle="Dotted" />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridBoundColumn DataField="ReferringToProvider" FilterControlAltText="Filter ReferringToProvider column"
                                                HeaderStyle-Width="120%" HeaderText="Referring to Provider" ItemStyle-Width="120%" ItemStyle-Font-Bold="false"
                                                UniqueName="ReferringToProvider">
                                                <HeaderStyle Width="120%" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="120%" />
                                                <ItemStyle  BorderStyle="Dotted"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ReferringToFacility" FilterControlAltText="Filter ReferringToFacility column"
                                                HeaderStyle-Width="120%" HeaderText="Referring to Facility" ItemStyle-Width="120%" ItemStyle-Font-Bold="false"
                                                UniqueName="ReferringToFacility">
                                                <HeaderStyle Width="120%" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="120%" />
                                                <ItemStyle  BorderStyle="Dotted"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Speciality" FilterControlAltText="Filter Speciality column"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" HeaderText="Specialty" ItemStyle-Font-Bold="false"
                                                ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px" UniqueName="Speciality">
                                                <HeaderStyle HorizontalAlign="Left" CssClass="Gridheaderstyle" />
                                                <ItemStyle HorizontalAlign="Left"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Diagnosis" FilterControlAltText="Filter Diagnosis column"
                                                HeaderStyle-Width="200px" HeaderText="Diagnosis" ItemStyle-Width="200px" UniqueName="Diagnosis" ItemStyle-Font-Bold="false">
                                                <HeaderStyle Width="200px" CssClass="Gridheaderstyle" />
                                                <ItemStyle Width="200px"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="AuthReq" FilterControlAltText="Filter AuthReq column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="100%" HeaderText="Auth Req" ItemStyle-Width="100%" UniqueName="AuthReq">
                                                <HeaderStyle Width="100%" CssClass="Gridheaderstyle" />
                                                <ItemStyle Width="100%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="NoOfVisit" FilterControlAltText="Filter NoOfVisit column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="100%" HeaderText="No. of Visit" ItemStyle-Width="100%" UniqueName="NoOfVisit">
                                                <HeaderStyle Width="100%" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="100%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ReasonForReferral" FilterControlAltText="Filter ReasonForReferral column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="120%" HeaderText="Reason for Referral" ItemStyle-Width="120%"
                                                UniqueName="ReasonForReferral">
                                                <HeaderStyle Width="120%" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="120%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RefDate" FilterControlAltText="Filter RefDate column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="100%" HeaderText="Ref Date" ItemStyle-Width="100%" UniqueName="RefDate">
                                                <HeaderStyle Width="100%" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="100%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ServiceRequested" FilterControlAltText="Filter ServiceRequested column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="100%" HeaderText="Service Requested" ItemStyle-Width="100%"
                                                UniqueName="ServiceRequested">
                                                <HeaderStyle Width="98px" CssClass="Gridheaderstyle"/>
                                                <ItemStyle Width="100%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="SpecialNeeds" FilterControlAltText="Filter SpecialNeeds column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="120%" HeaderText="Special Needs" ItemStyle-Width="120%" UniqueName="SpecialNeeds">
                                                <HeaderStyle Width="120%" CssClass="Gridheaderstyle" />
                                                <ItemStyle Width="120%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="OtherComments" FilterControlAltText="Filter OtherComments column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="100%" HeaderText="Other Comments" ItemStyle-Width="100%" UniqueName="OtherComments">
                                                <HeaderStyle Width="100px" CssClass="Gridheaderstyle" />
                                                <ItemStyle Width="100%"  />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ID" Display="false" FilterControlAltText="Filter ID column" ItemStyle-Font-Bold="false"
                                                HeaderStyle-Width="100%" HeaderText="ID" ItemStyle-Width="100%" UniqueName="ID">
                                                <HeaderStyle Width="100%" CssClass="Gridheaderstyle" />
                                                <ItemStyle Width="100%"  />
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlMovetonextProcess" runat="server" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="padding-top: 10px;">
                                            <%--<telerik:RadButton ID="btnMoveToNextProcess" runat="server" Text="Move To Next Process" OnClientClicked="btnMoveToNextProcess_Clicked"
                                                OnClick="btnMoveToNextProcess_Click" AccessKey="M" Style="text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;"
                                               ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">--%>
                                            <telerik:RadButton ID="btnMoveToNextProcess" runat="server" Text="Move To Next Process" OnClientClicked="btnMoveToNextProcess_Clicked"
                                                OnClick="btnMoveToNextProcess_Click" AccessKey="M" Font-Size="13px" 
                                               ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                                <ContentTemplate>
                                                    <span class="underline">M</span>ove To Next Process
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="InvisibleButton" runat="server" OnClick="InvisibleButton_Click" style="display:none;" />
                <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                <asp:Panel ID="Panel2" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="Label1" Text="" runat="server"></asp:Label></center>
                    <br />
                    <img src="Resources/wait.ico" title="" alt="Loading..." />
                    <br />
                </asp:Panel>
            </div>--%>
            </telerik:RadAjaxPanel>
        </div>
        <asp:ToolkitScriptManager ID="toolkitScriptMngr" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </asp:ToolkitScriptManager>
        <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>
        <%--  <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
        <scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </scripts>
    </telerik:RadScriptManager>--%>
         <asp:Button ID="btnClear" runat="server" style="display:none;" OnClick="btnClear_Click" />
        <asp:HiddenField ID="SelectedItem" runat="server" />
        <asp:HiddenField ID="hdnTransferVaraible" runat="server" />
        <asp:HiddenField ID="hdnEncID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPhyId" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnhumanid" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnRowIndex" runat="server" EnableViewState="false"/>
         <asp:HiddenField ID="hdnUserName" runat="server" EnableViewState="false"/>
        <asp:Button ID="btnDelete" style="display:none;" runat="server" Text="DisableLoad" CssClass="DisplayNone"
                onclick="btnDelete_Click" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSReferralOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomPhrases.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <%--<link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
