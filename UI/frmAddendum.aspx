<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAddendum.aspx.cs" Inherits="Acurus.Capella.UI.frmAddendum" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UserControls/CustomDLCNew.ascx" TagName="CustomDLC" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Amendment</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
   <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
<link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {
            width: 33px;
        }

        .RightAligned {
            text-align: right;
        }

        .ui-dialog-titlebar-close {
            display: none !important;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            width: 60px !important;
        }

        .ui-dialog .ui-dialog-titlebar {
            padding: 0px !important;
        }

        .ui-dialog .ui-dialog-title {
            font-size: 12px !important;
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px !important;
            border: 2px solid !important;
            border-radius: 13px !important;
            top: 504px !important;
            left: 568px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
            /*padding: 0px !important ;*/
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            font-weight: bold !important;
            font-size: 12px !important;
            font-family: sans-serif !important;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7 !important;
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

        
    </style>

    
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
</head>
<body onload="LoadAddendum(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart(); } ">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" VisibleStatusbar="False">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Amendment" VisibleStatusbar="False">
                </telerik:RadWindow>
                <telerik:RadWindow ID="WindowException" runat="server" Behaviors="Close" Title="Exception" VisibleStatusbar="False">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="1100px">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                <asp:Panel ID="pnlEncounterDetails" runat="server" GroupingText="Encounter Details"
                    Font-Bold="false" Font-Size="Small" CssClass="Editabletxtbox">
                    <table id="tblEncounterDetails">
                        <tr>
                            <td bgcolor="White">
                                <asp:Label ID="lblPatientName" runat="server" Text="Patient Name" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientNameText" runat="server" 
                                    ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblDateOfBirth" runat="server" Text="Date Of Birth" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDOBText" runat="server"  ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblSex" runat="server" Text="Sex" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSexText" runat="server" ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblAccountNo" runat="server" Text="Account #" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtAccountNoText" runat="server" 
                                    ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <%--<td class="style43" bgcolor="White">
                                        <asp:Label ID="lblMedicalRecNo" runat="server" Text="Medical Rec #"></asp:Label>
                                    </td>
                                    <td bgcolor="#BFDBFF" class="style69">
                                        <asp:Label ID="lblMedicalRecNoText" runat="server"></asp:Label>
                                    </td>--%>
                        </tr>
                        <tr>
                            <td bgcolor="White">
                                <asp:Label ID="lblFacilityName" runat="server" Text="Facility Name" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFacilityNameText" runat="server" 
                                    ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblDateOfService" runat="server" Text="Date Of Service" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDOSText" runat="server" ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblEncounterProvider" runat="server" Text="Encounter Provider" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:TextBox ID="txtEncounterProviderText" runat="server" Width="407px"
                                    ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <%--<td align="right" colspan="2" bgcolor="White" class="style40">
                                        <asp:Label ID="lblMedicalAssistant" runat="server" Text="Medical Assistant"></asp:Label>
                                    </td>
                                    <td bgcolor="#BFDBFF" class="style68">
                                        <asp:Label ID="lblMedicalAssistantText" runat="server"></asp:Label>
                                    </td>--%>
                        </tr>
                        <tr>
                            <td bgcolor="White">
                                <asp:Label ID="lblAddendumTypedBy" runat="server" Text="Amendment typed by" ForeColor="Blue"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddendumTypedByText" runat="server" ReadOnly="True"
                                    BackColor="#BFDBFF"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblAddendumTypedDateTime" runat="server" Text="Amendment typed date and time"
                                    ForeColor="Blue"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddendumTypedDateTimeText" runat="server" ReadOnly="True"
                                    BackColor="#BFDBFF"></asp:TextBox>
                            </td>
                            <td bgcolor="White">
                                <asp:Label ID="lblAddendumSignedBy" runat="server" Text="Amendment signed by" ForeColor="Blue"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddendumSignedByText" runat="server" AutoPostBack="true"
                                    ReadOnly="True" BackColor="#BFDBFF"></asp:TextBox>
                            </td>
                            <td align="right" colspan="3" bgcolor="White">
                                <asp:Label ID="lblAddendumSignedDateAndTime" runat="server" Text="Amendment signed date and time"
                                    ForeColor="Blue"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddendumSignedDateAndTimeText" runat="server"
                                    AutoPostBack="true" ReadOnly="True" BackColor="#BFDBFF"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlAmendmentSource" runat="server" Font-Size="Small" GroupingText="Amendment Source" CssClass="Editabletxtbox">
                    <table style="width: 600px">
                        <tr>
                            <td>
                                <%--<asp:Label ID="Label1" runat="server" Font-Size="Small" Text="Source*"  mand="Yes"></asp:Label>--%>
                                 <span class="manredforstar" runat="server" id="spanPayername">Source* </span><%--<span class="manredforstar" runat="server" id="spanPayerstar" visible="false">*</span>--%>


                            </td>
                            <td>
                                <asp:RadioButton ID="rdPatient" runat="server" Font-Size="Small" GroupName="AmendmentSource"
                                    Text="Patient" AutoPostBack="false" OnCheckedChanged="rdPatient_CheckedChanged" onchange="rdPat_rdPro_Change();" CssClass="Editabletxtbox"/>
                            </td>
                            <td>
                                <asp:RadioButton ID="rdProvider" runat="server" Font-Size="Small" GroupName="AmendmentSource"
                                    Text="Provider" OnCheckedChanged="rdProvider_CheckedChanged" AutoPostBack="false" onchange="rdPat_rdPro_Change();" CssClass="Editabletxtbox"/>
                            </td>
                            <td class="style1"></td>
                            <td align="right">
                                <%--<asp:Label ID="lblAcceptOrDeny" runat="server"  Text="Accept or Deny ?*"
                                  <%--mand="Yes"></asp:Label>--%>
                                  <span class="manredforstar" runat="server" id="spanAcceptOrDeny">Accept or Deny ?* </span>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cboAcceptOrDeny" runat="server" OnSelectedIndexChanged="cboAcceptOrDeny_SelectedIndexChanged"
                                    AutoPostBack="false" OnClientSelectedIndexChanged="cboAcceptOrDeny_IndexChanged" CssClass="nonEditabletxtbox">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlAddendumNotes" runat="server" Font-Size="Small" GroupingText="Amendment Notes" CssClass="Editabletxtbox">
                    <uc1:CustomDLC ID="txtAddendumNotes" runat="server" TextboxHeight="100"
                        TextboxWidth="930" Value="Amendment Notes"/>

                </asp:Panel>
                <asp:Panel ID="pnlAttestation" runat="server" Font-Bold="false" Font-Size="Small"
                    GroupingText="Attestation" CssClass="Editabletxtbox">
                    <asp:CheckBox ID="chkElectronicDigitalSignature" runat="server" AutoPostBack="false"
                        OnCheckedChanged="chkElectronicDigitalSignature_CheckedChanged" onchange="EnableButtons();" />
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" Font-Bold="false" Font-Size="Small">
                    <table align="right">
                        <tr>
                            <td>
                                <asp:Label ID="lblSelectReviewPhysician" runat="server" Text="Select Review Physician"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cboShowAllPhysicians" runat="server" Height="100px" Width="500px" AutoPostBack="true"
                                    OnSelectedIndexChanged="cboShowAllPhysicians_SelectedIndexChanged" OnClientSelectedIndexChanged="EnableButtons">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkShowAllPhysicians" runat="server" Text="Show All Physicians" AutoPostBack="true"
                                    OnCheckedChanged="chkShowAllPhysicians_CheckedChanged" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" />
                            </td>
                            <td align="right">
                                <telerik:RadButton ID="btnMoveToProviderReview" runat="server" Text="Move To Next Process"
                                    Width="175px" OnClick="btnMoveToProviderReview_Click" OnClientClicked="OnbtnSaveAndCloseClick" ButtonType="LinkButton"  CssClass="bluebutton">
                                </telerik:RadButton>
                            </td>
                            <td align="right">
                                <telerik:RadButton ID="btnSaveAndClose" runat="server" Text="Save and Close" OnClick="btnSaveAndClose_Click" OnClientClicked="OnbtnSaveAndClose" ButtonType="LinkButton" CssClass="greenbutton">
                                </telerik:RadButton>
                            </td>
                            <td align="right">
                                <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" OnClientClicked="OnClientbtnCancelClick" ButtonType="LinkButton" CssClass="redbutton">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="hdnIsLoad" runat="server" Value="true" />
            <asp:HiddenField ID="hdnUserRole" runat="server" Value="" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" />
            <asp:HiddenField ID="hdnIsAssistant" runat="server" />
            <asp:Button ID="btnIsReview" runat="server" Text="Button" Style="display: none" OnClick="btnIsReview_Click" />
               <asp:Button ID="btnReview" runat="server" Text="Button" Style="display: none" OnClick="btnReview_Click" />
            <asp:Button ID="btnIsClose" runat="server" Text="Button" CssClass="dispaly" OnClick="btnIsClose_Click"
                Visible="False" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAddendum.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
