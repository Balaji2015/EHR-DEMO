<%@ Page Async="true" Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="frmEditAppointment.aspx.cs"
    Inherits="Acurus.Capella.UI.frmEditAppointment"  %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Appointment</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <%--    <link href="CSS/style.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <base target="_self"></base>
    <style type="text/css">
        #form1 {
        }

        .style12 {
            width: 202px;
        }

        .style13 {
            width: 383px;
        }

        .style21 {
            width: 128px;
        }

        .style22 {
        }

        .style23 {
            width: 125px;
        }

        .style25 {
            width: 159px;
        }

        .style26 {
            width: 118px;
        }

        .style27 {
            width: 150px;
        }

        .style28 {
            width: 29px;
        }

        .style29 {
            width: 114px;
        }

        .Panel legend {
            font-weight: bold;
        }

        .style30 {
            width: 77px;
        }

        .style31 {
        }

        .style33 {
            width: 170px;
        }

        .style20 {
            width: 120px;
        }

        /*.modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }*/


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

        .ui-dialog-titlebar-close {
            display: none;
        }


        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }

        #frmEditAppointment {
            margin-bottom: 0px;
        }

        .style {
            width: 614px;
        }

        .AuthBtn {
            font-family: Arial;
            font-style: inherit;
            font-weight: bolder;
        }

        .resize {
            resize: none;
        }

        #Update {
            height: 590px;
        }

        #ddlReasonCode.element.style {
            background-color: lightgray !important;
        }
    </style>
    <%--<link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" /> --%>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="EditAppointmentLoad();">
    <form id="frmEditAppointment" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="DLC"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                   <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Title="DLC"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <div>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnablePartialRendering="false">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                 <asp:Panel ID="pnlPatientStrip" runat="server" Width="800px"
                    CssClass="LabelStyleBold">
                 
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
            <div runat="server" visible="false">
                <asp:Panel ID="pnlPatientDetails" GroupingText="Patient Details" runat="server" Width="800px"
                    CssClass="LabelStyleBold ">

                    <table style="width: 100%;" class="bodybackground">
                        <tr>
                            <td class="style26">
                                <asp:Label ID="lblPatientAccountNumber" CssClass="Editabletxtbox" runat="server" Text="Acc. #"></asp:Label>
                            </td>
                            <td class="style12">
                                <asp:TextBox ID="txtPatientAccountNumber" CssClass="nonEditabletxtbox" runat="server" ReadOnly="True" Width="231px"></asp:TextBox>
                            </td>
                            <td class="style30">
                                <asp:Label ID="lblpatientName" runat="server" CssClass="Editabletxtbox" Text="Patient Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientName" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="195px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style26">
                                <asp:Label ID="lblDOB" runat="server" CssClass="Editabletxtbox" Text="Patient DOB"></asp:Label>
                            </td>
                            <td class="style12">
                                <asp:TextBox ID="txtPatientDOB" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="231px"></asp:TextBox>
                            </td>
                            <td class="style30">
                                <asp:Label ID="lblHumanType" runat="server" CssClass="Editabletxtbox" Text="Patient Type"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHumanType" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="195px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style26">
                                <asp:Label ID="lblHomePhoneNumber" runat="server" CssClass="Editabletxtbox" Text="Home Ph#"></asp:Label>
                            </td>
                            <td class="style12">
                                <asp:TextBox ID="txtHomePhoneNumber" runat="server" ReadOnly="True" Width="228px"
                                    CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td class="style30">
                                <asp:Label ID="lblCellPhoneNumber" runat="server" CssClass="Editabletxtbox" Text="Cell Ph#"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCellPhoneNumber" runat="server" ReadOnly="True" Width="194px"
                                    CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:UpdatePanel ID="Update" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlScheduleAppointment" runat="server" Width="800px" Height="590px" Margin-top="5px !important"
                            GroupingText="Schedule Appointment" CssClass="LabelStyleBold">
                            <table style="width: 66%;">
                                 
                                 <asp:Panel ID="pnlAppointmentDetails" runat="server">
                                <tr>
                                    <td class="style21">
                                        <asp:Label ID="lblApptDate" runat="server" CssClass="Editabletxtbox" Text="Appt Date"></asp:Label>
                                    </td>
                                    <td class="style27">
                                        <telerik:RadDatePicker ID="dtpApptDate" runat="server" Culture="English (United States)"
                                            Skin="Web20" onkeypress="EnableSaveButton(this);" EnableTyping="False" Height="21px"
                                            Width="175px">
                                            <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False"
                                                ShowRowHeaders="false" Skin="Web20">
                                                <SpecialDays>
                                                    <telerik:RadCalendarDay Repeatable="Today">
                                                        <ItemStyle CssClass="rcToday" />
                                                    </telerik:RadCalendarDay>
                                                </SpecialDays>
                                            </Calendar>
                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                            <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" LabelWidth="40%"
                                                ReadOnly="True" Height="21px" CssClass="Editabletxtbox">
                                                <EmptyMessageStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </DateInput>
                                            <ClientEvents OnPopupOpening="EnableSaveButton" OnDateSelected="EnableSaveButton" />
                                        </telerik:RadDatePicker>
                                    </td>
                                    <td class="style23">
                                        <asp:Label ID="lblStartTime" runat="server" CssClass="Editabletxtbox" Text="Start Time"></asp:Label>
                                    </td>
                                    <td class="style33">
                                        <telerik:RadTimePicker ID="dtpStartTime" runat="server" Culture="English (United States)"
                                            Height="25px" onkeypress="EnableSaveButton(this);" Skin="Web20" Width="160px"  CssClass="Editabletxtbox">
                                            <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                            <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                            </TimeView>
                                            <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                            </Calendar>
                                            <%--   <dateinput dateformat="M/d/yyyy" displaydateformat="M/d/yyyy" labelwidth="64px" 
                                            width="">
                                            <emptymessagestyle resize="None" />
                                            <readonlystyle resize="None" />
                                            <focusedstyle resize="None" />
                                            <disabledstyle resize="None" />
                                            <invalidstyle resize="None" />
                                            <hoveredstyle resize="None" />
                                            <enabledstyle resize="None" />
                                        </dateinput>--%>
                                            <ClientEvents OnPopupOpening="EnableSaveButton" />
                                            <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                        </telerik:RadTimePicker>
                                    </td>
                                    <td class="style36">
                                        <asp:CheckBox ID="chkReschedule" runat="server" AutoPostBack="True" OnCheckedChanged="chkReschedule_CheckedChanged"
                                            Text="Reschedule" Width="88px" CssClass="Editabletxtbox" onclick="EnableSaveButton(this)" />
                                    </td>
                                    <td class="style37">
                                        <asp:Button ID="btnFindAvailableSlot" runat="server" AccessKey="F" Text="Find Available Slot"
                                            Visible="false" Width="118px" OnClientClick="return OpenFindAvailableSlot()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style21">
                                        <asp:Label ID="lblFacilityName" runat="server" CssClass="Editabletxtbox" Text="Facility"></asp:Label>
                                    </td>
                                    <td class="style27">
                                        <telerik:RadComboBox ID="cboFacility" runat="server" AutoPostBack="True" onchange="EnableSaveButton(this);" OnSelectedIndexChanged="cboFacility_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style23">
                                        <asp:Label ID="lblProviderName" runat="server" CssClass="Editabletxtbox" Text="Prov. Name"></asp:Label>
                                    </td>
                                    <td class="style33">
                                        <telerik:RadComboBox ID="ddlPhysicianName" runat="server" AutoPostBack="True" onchange="enablecontrol();"
                                            OnSelectedIndexChanged="ddlPhysicianName_SelectedIndexChanged" Width="177px"
                                            Height="200px">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style25">
                                        <asp:CheckBox ID="chkShowAllPhysicians" runat="server" AutoPostBack="True" onchange="EnableSaveButton(this);{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"
                                            OnCheckedChanged="chkShowAllPhysicians_CheckedChanged" Text="Show All Providers " CssClass="Editabletxtbox"
                                            Width="130px" />
                                    </td>
                                </tr>
                                </asp:Panel>
                                <tr>
                                    <td colspan="6" style="width: auto">
                                        <asp:Panel ID="pnlReferringProvAndPCP" runat="server" CssClass="LabelStyleBold" GroupingText="Referring Details">

                                            <telerik:RadTabStrip ID="tabReferringProvAndPCP" runat="server" MultiPageID="rdmpReferringProvAndPCP"
                                                SelectedIndex="1" OnTabClick="tabReferringProvAndPCP_TabClick" OnClientTabSelected="tabReferringProvAndPCP_TabSelected">
                                                <Tabs>
                                                    <telerik:RadTab Text="Referring Provider Info" CssClass="LabelStyleBold" PageViewID="rpvReferringProvider">
                                                    </telerik:RadTab>
                                                    <telerik:RadTab Text="PCP info" PageViewID="rpvPcp" CssClass="LabelStyleBold" Selected="True">
                                                    </telerik:RadTab>

                                                </Tabs>
                                            </telerik:RadTabStrip>
                                            <%--    <telerik:RadMultiPage ID="rdmpBlockDays" runat="server" SelectedIndex="0">
                                                <telerik:RadPageView ID="rdmpReferringProvAndPCP" runat="server" Width="100%">
                                                </telerik:RadPageView>
                                            </telerik:RadMultiPage>--%>
                                             
                                             <asp:Panel ID="pnlReferringDetails" runat="server">
                                            <table>
                                                <tr>
                                                    <td style="width: 125px"></td>
                                                    <td>
                                                        <asp:CheckBox ID="rdoReferringProvider" runat="server" AutoPostBack="true" Text="Enter Ref provider details" Visible="false" CssClass="LabelStyleBold" OnCheckedChanged="rdoReferringProvider_CheckedChanged" />
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="rdoPcp" runat="server" Text="Enter PCP info" AutoPostBack="true" Visible="false" CssClass="LabelStyleBold" OnCheckedChanged="rdoPcp_CheckedChanged" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOrder" runat="server" ForeColor="Red" Text="Outstanding Order*" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <telerik:RadComboBox ID="cboOrder" runat="server" Width="600px" CssClass="Editabletxtbox" onchange="EnableSaveButton(this);" AutoPostBack="True" OnSelectedIndexChanged="cboOrder_SelectedIndexChanged">
                                                        </telerik:RadComboBox>

                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnOrder" runat="server" Text="Create Order" CssClass="aspresizedbluebutton"
                                                            OnClientClick="return OpenImageAndLaborder();" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td>
                                                        <asp:Button ID="btnFindPhysician" runat="server" AccessKey="F" OnClientClick="return OpenRereralPhysician();" CssClass="aspresizedbluebutton"
                                                            Text="Find Provider" />
                                                    </td>--%>
                                                    <td style="width: 100%">
                                                        <asp:CheckBox ID="chkSelfReferred" runat="server" AutoPostBack="true" onclick="chkchange();{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" OnCheckedChanged="chkSelfReferred_CheckedChanged" CssClass="Editabletxtbox" Text="Self Referred" />
                                                    </td>
                                                </tr>
                                                <%--   <tr>
                                                    <td class="style20">
                                                        <asp:Label ID="lblReferringName" runat="server" Text="Ref. Provider" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style27">
                                                        <asp:TextBox ID="txtReferringProvider" runat="server"
                                                            Width="173px" MaxLength="100" onchange="EnableSaveButton(this);" onkeypress="EnableSaveButton(this);" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                    <td class="style23">
                                                        <asp:Label ID="lblProviderNPI" runat="server" Text="Provider NPI" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style33">
                                                        <asp:TextBox ID="txtProviderNPI" runat="server" MaxLength="10"
                                                            onchange="EnableSaveButton(this);" onkeypress="EnableSaveButton(this);" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                    <td class="style33"></td>

                                                </tr>

                                                <tr>
                                                    <td class="style20">
                                                        <asp:Label ID="lblReferingFacility" runat="server" Text="Ref. Facility" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style27">
                                                        <asp:TextBox ID="txtReferringFacility" runat="server"
                                                            Width="173px" MaxLength="50" onkeypress="EnableSaveButton(this);" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                    <td class="style23">
                                                        <asp:Label ID="lblAuthorization" Visible="false" runat="server" Text="Authorization#" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style33" colspan="2">
                                                        <asp:TextBox ID="txtAuthorizationNo" runat="server" BackColor="#BFDBFF" BorderColor="Black"
                                                            BorderWidth="1px" Width="180px" TextMode="MultiLine" CssClass="MultiLineTextBox Editabletxtbox"
                                                            Visible="false" onkeypress="EnableSaveButton(this);"></asp:TextBox>
                                                        <%-- <table>
                                <tr>
                                <td>  <asp:TextBox ID="txtAuthorizationNo" runat="server" BackColor="#BFDBFF" BorderColor="Black"
                                        BorderWidth="1px" Width="180px" TextMode="MultiLine" 
                                        CssClass="MultiLineTextBox" onkeypress="EnableSaveButton(this);"></asp:TextBox>
                                </td>
                                <td>
                                <div class="col-6-btn margintop5px">
		                                    <a runat="server" id="pbView" align="centre" font-bold="false" > <i class ="AuthBtn">V</i></a>
		                                 </div>
		                        </td>
                                <td>
                                 <div class="col-6-btn margintop5px">
		                                    <a runat="server" id="pbClear" align="centre" font-bold="false" > <i class ="AuthBtn">X</i></a>
		                         </div>
                                </td>
                                </tr>
                                </table>--%>
                                                <%-- <asp:Button ID="btnViewAuthorization" runat="server" Text="V" ToolTip ="View Authorization"/>
                                         <asp:Button ID="btnClearAuthorization" runat="server" Text="X" ToolTip="Clear"/>--%>
                                                <%--</td>
                                                    <td class="style25">
                                                        <asp:Button ID="btnFindAuthorization" runat="server" OnClientClick="return OpenFindAuth();"
                                                            Visible="false" Text="Find Authorization" CssClass="aspresizedbluebutton" Width="117px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style21">
                                                        <asp:Label ID="lblReferingAddress" runat="server" Text="Ref. Address" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style27" colspan="5">
                                                        <asp:TextBox ID="txtReferingAddress" runat="server" Width="610px" onkeypress="EnableSaveButton(this);"
                                                            MaxLength="225" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style20">
                                                        <asp:Label ID="lblReferingPhoneNo" runat="server" Text="Ref. Phone #" Width="118px" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style27">
                                                        <telerik:RadMaskedTextBox ID="msktxtReferingPhoneNo" Width="173px" runat="server" Mask="(###) ###-####" onkeypress="EnableSaveButton(this);" ReadOnly="true" CssClass="nonEditabletxtbox">
                                                            <InvalidStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                    <td class="style23">
                                                        <asp:Label ID="lblReferingFaxNo" runat="server" Text="Ref. Fax #" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style33">
                                                        <%--<asp:MaskedEditExtender ID="msktxtReferingFaxNoExtender" runat="server" ClearMaskOnLostFocus="false"
                                        Mask="(999) 999-9999" TargetControlID="msktxtReferingFaxNo"></asp:MaskedEditExtender>--%>
                                                <%--   <telerik:RadMaskedTextBox ID="msktxtReferingFaxNo" runat="server" Mask="(###) ###-####" onkeypress="EnableSaveButton(this);" ReadOnly="true" CssClass="nonEditabletxtbox">
                                                            <InvalidStyle Resize="None" />
                                                            <FocusedStyle Resize="None" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <DisabledStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                    <td class="style33" colspan="2">
                                                        <asp:CheckBox ID="chkWillingnessInCancellation" Visible="false" runat="server" Text="Willing to come on cancellation" CssClass="Editabletxtbox"
                                                            onclick="EnableSaveButton(this);" OnCheckedChanged="chkWillingnessInCancellation_CheckedChanged" />
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td style="width: 8%;">
                                                        <span class="spanstyle">Provider Search</span>
                                                    </td>
                                                    <td style="width: 88%;">
                                                        <asp:TextBox ID="txtProviderSearch" runat="server"  data-phy-id="0"  data-phy-details="" Rows="3" TextMode="MultiLine" placeholder="Type minimum 3 characters of Last or First name or Facility and follow it by a space.."  style="width:110.5%;resize:none" ></asp:TextBox> 
                                                    <td style="width:3%;">

                                                        <img id="imgClearProviderText" runat="server" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." onclick="return ProviderSearchclear();" style="position: absolute; margin-left: 65px; top: 246px !important; cursor: pointer; width: 10px; height: 15px;" />
                                                       </td>
                                                        <td style="width:12%;">
                                                        <img id="imgEditProvider" runat="server" src="Resources/edit.gif" alt="X" title="Click to edit the text field." onclick="return EditProviderDetails();" style="position: absolute; margin-left: 65px; top: 235px !important; cursor: pointer; width: 13px; height: 15px;" />


                                                    </td>
                                                </tr>
                                            </table>
                                                 </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                 
                                <asp:Panel ID="pnlVisit" runat="server">
                                <tr>
                                    <td class="style21">
                                        <asp:Label ID="lblPurposeofVisit" runat="server" Text="Type of Visit" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style27">
                                        <%--<telerik:RadComboBox ID="ddlVisitType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVisitType_SelectedIndexChanged"
                                            Width="173px" OnClientSelectedIndexChanged="ddlVisitType_onChange">
                                        </telerik:RadComboBox>--%>
                                        <telerik:RadComboBox ID="ddlVisitType" runat="server" AutoPostBack="True"
                                            OnClientSelectedIndexChanged="ddlVisitType_onChange" OnSelectedIndexChanged="ddlVisitType_SelectedIndexChanged"
                                            Width="173px">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style23">
                                        <asp:Label ID="lblTimeDuration" runat="server" CssClass="Editabletxtbox" Text="Time Duration"></asp:Label>
                                    </td>
                                    <td class="style33">
                                        <asp:TextBox ID="ddlDuration" runat="server" MaxLength="3" onkeypress="return isNumberKey(this);" CssClass="Editabletxtbox"
                                            Rows="5" Width="164px"></asp:TextBox>
                                    </td>
                                    <td class="style25" colspan="2">
                                        <asp:Label ID="Label1" runat="server" Text="Min" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style21">
                                        <asp:Label ID="radLabel2" runat="server" Text="Visit Description" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style22" colspan="5">
                                        <asp:TextBox ID="txtVisitDescription" runat="server" ReadOnly="True" TextMode="MultiLine"
                                            Width="617px" CssClass="MultiLineTextBox resize nonEditabletxtbox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style31">
                                        <asp:Label ID="lblPatientNote" runat="server" Text="Purpose of Visit" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style31" colspan="5" rowspan="2">
                                        <DLC:DLC ID="txtPurposeofVisit" runat="server" Enable="True" TextboxHeight="60px"
                                            onkeypress="EnableSaveButton(this);" TextboxWidth="550px" Value="PURPOSE OF VISIT" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style21"></td>
                                </tr>
                                <%--vasanth
                                <tr>
                                    <td class="style21">
                                        <asp:Label ID="lblTest" runat="server" Text="Lab and Ancillary Test"></asp:Label>
                                    </td>
                                    -Nijanthan
                                    <td colspan="5">
                                        <table style="width: 610px; height: 45px;">
                                            <tr>
                                                <td class="style38" rowspan="2">
                                                    <asp:TextBox ID="txtTest" runat="server" BackColor="#BFDBFF" BorderColor="Black"
                                                        Height="40px" BorderWidth="1px" CssClass="MultiLineTextBox resize" ReadOnly="True"
                                                        TextMode="MultiLine" Width="614px"></asp:TextBox>
                                                </td>
                                                <td style="width: 20px; height: 23px">
                                                    <asp:ImageButton ID="pbTestDropDown" runat="server" ImageUrl="~/Resources/pbAdd.png"
                                                        align="centre" font-bold="false" OnClick="pbTestDropDown_Click" Width="22px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 20px; height: 23px">
                                                    <asp:ImageButton ID="pbTestClear" runat="server" ImageUrl="~/Resources/pbClear.png"
                                                        align="centre" font-bold="false" OnClick="pbTestClear_Click" Width="22px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td class="style21"></td>
                                    <td class="style22" colspan="5">
                                        <asp:Panel ID="pnlTest" runat="server" Visible="False">
                                            <asp:ListBox ID="lstTest" runat="server" AutoPostBack="True" onchange="EnableSaveButton(this);"
                                                OnSelectedIndexChanged="lstTest_SelectedIndexChanged" Width="613px"></asp:ListBox>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                -----%>
                                <tr>
                                    <td class="style21">
                                        <asp:Label ID="lblNotes" runat="server" Text="Notes" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style22" colspan="5" rowspan="2">
                                        <DLC:DLC ID="txtNotes" runat="server" Enable="True" TextboxHeight="60px" TextboxWidth="550px"
                                            onkeypress="EnableSaveButton(this);" Value="APPOINTMENT NOTES" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style21"></td>
                                </tr>
                                <tr>
                                    <td class="style32" colspan="6">
                                        <asp:Panel ID="pnlReschedule" runat="server" Font-Size="Small" GroupingText="Reschedule Appointment" CssClass="LabelStyleBold"
                                            Width="758px">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td class="style29">
                                                        <asp:Label ID="lblReasonCode" runat="server" Text="Reason Code" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadComboBox ID="ddlReasonCode" runat="server" onchange="EnableSaveButton(this);" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlReasonCode_SelectedIndexChanged" Width="315px">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="style29">
                                                        <asp:Label ID="lblReasonText" runat="server" Text="Reason Text" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtReasonCode" runat="server" Width="611px" onkeypress="EnableSaveButton(this);" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                </tr>                                                
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                 </asp:Panel>
                            </table>
                        </asp:Panel>
                        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                            <asp:Panel ID="Panel2" runat="server">
                                <br />
                                <br />
                                <br />
                                <br />
                                <center>
                                    <asp:Label ID="Label2" Text="" runat="server"></asp:Label></center>
                                <br />
                                <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                                    alt="Loading..." />
                                <br />
                            </asp:Panel>
                        </div>
                        <%--<asp:MaskedEditExtender ID="msktxtReferingPhoneNoExtender" TargetControlID="msktxtReferingPhoneNo"
                        Mask="(999) 999-9999" ClearMaskOnLostFocus="false" runat="server">
                    </asp:MaskedEditExtender>--%>
                        <asp:HiddenField ID="hdnPbClick" runat="server" />
                        <asp:HiddenField ID="hdnpbNotesClick" runat="server" />
                        <asp:HiddenField ID="hdnHumanID" runat="server" />
                        <asp:HiddenField ID="hdnEncounterID" runat="server" />
                        <asp:HiddenField ID="hdnLocalTime" runat="server" OnValueChanged="hdnLocalTime_ValueChanged" />
                        <asp:HiddenField ID="hdnPhysicianID" runat="server" />
                        <asp:HiddenField ID="hdnSelectedDateTime" runat="server" />
                        <asp:Button ID="btnConfirmAppointment" Style="display: none" runat="server" OnClick="btnConfirmAppointment_Click"
                            Text="ConfirmAppointment" />
                        <asp:Button ID="btnAppointmentSlot" runat="server" OnClick="btnAppointmentSlot_Click"
                            Text="AppointmentSlot" Style="display: none" />
                        <asp:HiddenField ID="hdnFacilityName" runat="server" />
                        <asp:HiddenField ID="hdnCurrentProcess" runat="server" />
                        <asp:HiddenField ID="hdnTestClick" runat="server" />
                        <asp:HiddenField ID="hdnPatientSex" runat="server" />
                        <asp:HiddenField ID="hdnPatientStatus" runat="server" />
                        <asp:HiddenField ID="hdnOrderList" runat="server" />
                        <asp:HiddenField ID="hdnIsPopUp" runat="server" />
                        <asp:HiddenField ID="hdnParentscreen" runat="server" />
                        <asp:HiddenField ID="hdnAuthId" runat="server" />
                        <asp:HiddenField ID="hdnAuthSelectId" runat="server" />
                        <asp:HiddenField ID="hdnApptDate" runat="server" />
                        <asp:HiddenField ID="hdnApptTime" runat="server" />
                        <asp:HiddenField ID="hdnProviderName" runat="server" />
                        <asp:HiddenField ID="hdnEditApptPhyID" runat="server" />
                        <asp:HiddenField ID="hdnPOS" runat="server" />
                        <asp:HiddenField ID="hdnEncounter_Physician_id" runat="server" EnableViewState="false" />
                        <asp:HiddenField ID="HdnRefPhy" runat="server" />
                        <asp:HiddenField ID="HdnPcpPhy" runat="server" />
                        <asp:HiddenField ID="HdnPCPID" runat="server" />
                        <asp:HiddenField ID="HdnEditVisit" runat="server" />
                        <asp:HiddenField ID="hdnrenprovider" runat="server" />
                        <asp:HiddenField ID="hdnpcpprovider" runat="server" />
                        <asp:HiddenField ID="hdnEditPhysicianId" runat="server" />
                        <asp:HiddenField ID="hdnCategory" runat="server" />
                           <asp:HiddenField ID="hdnpcpprovidersearch" runat="server" />
                         <asp:HiddenField ID="hdbselref" runat="server" />
                             <asp:HiddenField ID="hdnbtnsave" runat="server" />

                        
                           <asp:HiddenField ID="hdnrenprovidersearch" runat="server" />
                        <br />
                        <asp:HiddenField ID="hdnWillingGridIndex" runat="server" />
                        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
                            OnClientClick="return btnClose_Clicked();" />
                        <asp:HiddenField ID="hdnMessageType" runat="server" />
                        <asp:HiddenField ID="hdnConfirmBlockAppointment" runat="server" />
                        <asp:HiddenField ID="hdnTimeTaken" runat="server" />
                        <asp:HiddenField ID="hdnOrderSubmitdata" runat="server" Value="" />
                        <asp:HiddenField ID="hdnFacility" runat="server" Value="" />
                        <asp:HiddenField ID="hdnTabRefPcpChange" runat="server" />
                        <asp:Button ID="btnReferralandPCP" runat="server" Style="display: none" OnClick="btnReferralandPCP_Click" />
                        <asp:Button ID="btnOrderCreate" runat="server" Style="display: none" OnClick="btnOrderCreate_Click" />
                   <asp:Button ID="hdnbtngeneratexmlAppointment" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

                         </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="margin-top: 10px;">
                <asp:Button ID="btnHumanDetailUpdate" Style="display: none" runat="server" Text="HumanDetailUpdate"
                    OnClick="btnHumanDetailUpdate_Click" />
                <asp:Panel ID="pnlButtons" runat="server" Width="775px">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnPatientTask" runat="server" Text="Patient Task" OnClientClick="return OpenPatientTask();" CssClass="aspresizedbluebutton"
                                    AccessKey="T" />
                            </td>
                            <td>

                                <asp:Button ID="btnPatientDemographics" runat="server" Text="Patient Demographics"
                                    OnClientClick="return OpenPatientDemographics();" CssClass="aspresizedbluebutton" AccessKey="D" />
                            </td>
                            <td class="style13"></td>
                            <td>
                                <%--OnClientClick="return showTime();"--%>
                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="58px"
                                    OnClick="btnSave_Click" AccessKey="S" CssClass="aspresizedgreenbutton" />
                            </td>
                            <td>

                                <button type="button" id="btnClose" style="width: 58px" value="Close" onclick="btnClose_Clicked();" class="aspresizedredbutton">Close</button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>

            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
             <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSEditAppointment.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <%--            <script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>--%>
        </asp:PlaceHolder>
    </form>
</body>
</html>
