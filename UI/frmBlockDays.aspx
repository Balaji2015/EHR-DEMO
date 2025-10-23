<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmBlockDays.aspx.cs" Inherits="Acurus.Capella.UI.frmBlockDays"  EnableEventValidation="false"%>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Block Days</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <base target="_self" />
    <style type="text/css">
        .Panel legend {
            font-weight: bold;
        }
        .RadButton_Default.rbSkinnedButton, .RadButton_Default .rbDecorated, .RadButton_Default.rbVerticalButton, .RadButton_Default.rbVerticalButton .rbDecorated, .RadButton_Default .rbSplitRight, .RadButton_Default .rbSplitLeft

{
    background-image:none !important;
}
        .style1 {
            width: 466px;
            height: 37px;
        }

        .style2 {
            height: 37px;
        }

        .modal {
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
</head>

<body   class="bodybackground" onload="BlockDays_Load()">
    <form id="form1" runat="server"
        style="background-color: #ffffff;">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Title="DLC"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" >
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </ajaxToolkit:ToolkitScriptManager>
        <telerik:RadAjaxPanel ID="rapBlockDays" runat="server">
            <asp:Panel ID="pnlBlockDays" runat="server" Height="485px" Width="1083px" CssClass="Editabletxtbox">
                <table style="height: 400px; width: 1051px;">
                    <tr>
                        <td style="height: 400px">
                            <asp:Panel ID="pnlFacilityList" runat="server"  CssClass="Editabletxtbox">
                                <table style="width: 380px; height: 373px;">
                                    <tr >
                                        <td style="width: 209px">
                                            <asp:Panel ID="pnlFacility" runat="server" Font-Size="Small" GroupingText="Facility"
                                                BackColor="White" Height="45px" Width="370px" CssClass="Panel">
                                                <telerik:RadComboBox ID="ddlFacilityName" runat="server"  OnSelectedIndexChanged="ddlFacilityName_SelectedIndexChanged"
                                                    AutoPostBack="True" Width="350px" OnClientSelectedIndexChanged="facilityChange" CssClass="Editabletxtbox">
                                                </telerik:RadComboBox>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 209px"> <%--GroupingText="Provider"--%>
                                            <asp:Panel ID="pnlProvider" runat="server" Font-Size="Small"
                                                BackColor="White" Height="385px" Width="377px" CssClass="Editabletxtbox">
                                                <div style="overflow: auto; height: 348px; width: 340px;">
                                                    <asp:CheckBoxList ID="chklstboxProvider" runat="server" RepeatLayout="Flow"
                                                        Width="248px" CssClass="Editabletxtbox">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td colspan="3"
                            style="margin-left: 36px; height: 397px; width: 856px;"
                            valign="top">
                            <telerik:RadTabStrip ID="tabBlockDays" runat="server" MultiPageID="rdmpBlockDays"
                                OnTabClick="tabGeneralQ_TabClick" OnClientTabSelecting="BlockDaysTabChange"
                                SelectedIndex="0">
                                <Tabs>
                                    <telerik:RadTab Text="Block Recurring Days" PageViewID="rpvBlockRecurring" Font-Bold="true"
                                        Selected="True" CssClass="spanstyle">
                                    </telerik:RadTab>
                                    <telerik:RadTab Text="Block Non-Recurring Days" PageViewID="rpvBlockNonRecurring" Font-Bold="true" CssClass=" spanstyle">
                                    </telerik:RadTab>
                                    <telerik:RadTab Text="Block Type of Visit" PageViewID="rpvBlockTypeOfVisit" Font-Bold="true" Visible="false">
                                    </telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="rdmpBlockDays" runat="server" SelectedIndex="0" Height="380px">
                                <telerik:RadPageView ID="rpvBlockRecurring" runat="server" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlRecurBlockDays" runat="server" Font-Size="Small" GroupingText="Block Days"
                                                    Height="364px" Width="800px" BackColor="White" CssClass="Editabletxtbox">
                                                    <asp:Panel ID="pnlRecurSelectDates" runat="server">
                                                        <table style="width: 777px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text="From Date" Width="75px" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="dtpRecurFromDate" runat="server" Culture="en-US"
                                                                        EnableTyping="False" Height="21px" Width="147px" onchange="dtpRecurToTime_SelectedDateChanged()" CssClass="Editabletxtbox">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" Height="18px"
                                                                            LabelWidth="40%" ReadOnly="True" Width="">
                                                                            <EmptyMessageStyle Resize="None" CssClass="Editabletxtbox"></EmptyMessageStyle>
                                                                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                                            <FocusedStyle Resize="None"></FocusedStyle>
                                                                            <DisabledStyle Resize="None"></DisabledStyle>
                                                                            <InvalidStyle Resize="None"></InvalidStyle>
                                                                            <HoveredStyle Resize="None"></HoveredStyle>
                                                                            <EnabledStyle Resize="None"></EnabledStyle>
                                                                        </DateInput>
                                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label2" runat="server" Text="(Eg. 01-Jan-2010)" Width="147px" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" Width="53px" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="dtpRecurToDate" runat="server" Culture="en-US"
                                                                        EnableTyping="False" Height="18px" Width="147px" onchange="dtpRecurToTime_SelectedDateChanged()" CssClass="Editabletxtbox">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" CssClass="Editabletxtbox">
                                                                        </Calendar>
                                                                        <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" Height="18px"
                                                                            LabelWidth="40%" ReadOnly="True" Width="" CssClass="Editabletxtbox">
                                                                            <EmptyMessageStyle Resize="None" CssClass="Editabletxtbox"></EmptyMessageStyle>
                                                                            <ReadOnlyStyle Resize="None"></ReadOnlyStyle>
                                                                            <FocusedStyle Resize="None"></FocusedStyle>
                                                                            <DisabledStyle Resize="None"></DisabledStyle>
                                                                            <InvalidStyle Resize="None"></InvalidStyle>
                                                                            <HoveredStyle Resize="None"></HoveredStyle>
                                                                            <EnabledStyle Resize="None"></EnabledStyle>
                                                                        </DateInput>
                                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Text="(Eg. 01-Jan-2010)" Width="136px" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlSelectDate" runat="server" GroupingText="Select Day(s)" BackColor="White"
                                                        Width="758px" CssClass="Editabletxtbox" >
                                                        <table style="width: 758px;">
                                                            <tr style="height:30px;">
                                                                <td colspan="3.5" style="text-align: center;">
                                                                    <asp:CheckBox ID="chkAlternateWeeks" runat="server" onclick="ChecksAlternateWeeks(this);" Text="Alternate Weeks" Width="120px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td colspan="3.5" style="text-align: center;">
                                                                    <asp:CheckBox ID="chkAlternateMonths" runat="server" onclick="ChecksAlternateMonths(this);" Text="Alternate Months" Width="120px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                            </tr>
                                                            <tr style="height:30px;">
                                                                <td>
                                                                    <asp:CheckBox ID="chkSunday" runat="server" onclick="EnableSave();" Text="Sunday" Checked="true" Width="80px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkMonday" runat="server" onclick="EnableSave();" Text="Monday"
                                                                        Width="80px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkTuesday" runat="server" onclick="EnableSave();" Text="Tuesday"
                                                                        Width="80px" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkWednesday" runat="server" onclick="EnableSave();" Text="Wednesday"
                                                                        Width="100px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkThursday" runat="server" onclick="EnableSave();" Text="Thursday"
                                                                        Width="100px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkFriday" runat="server" onclick="EnableSave();" Text="Friday"
                                                                        Width="80px" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkSaturday" runat="server" onclick="EnableSave();" Text="Saturday" Checked="true"
                                                                        Width="80px" CssClass="Editabletxtbox"/>
                                                                </td>                                                                
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlRecurBlockTime" runat="server" GroupingText="Block Time" BackColor="White"
                                                        Width="758px" CssClass="Editabletxtbox">
                                                        <table style="width: 758px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkRecurSelecttime" runat="server" Text="Select Time" Width="100px"
                                                                        OnCheckedChanged="chkRecurSelecttime_CheckedChanged" AutoPostBack="True" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRecurFromTime" runat="server" Text="From Time" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTimePicker ID="dtpRecurFromTime" runat="server" Culture="en-US"
                                                                        Height="25px" Skin="Web20" Width="160px"
                                                                        onchange="dtpRecurToTime_SelectedDateChanged()">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" CssClass="Editabletxtbox">
                                                                        </Calendar>
                                                                        <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                                        </TimeView>
                                                                        <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <%--<DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" AutoPostBack="True"
                                                                            Width="">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>--%>
                                                                    </telerik:RadTimePicker>
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRecurToTime" runat="server" Text="To Time" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTimePicker ID="dtpRecurToTime" runat="server" Culture="en-US"
                                                                        Height="25px" Skin="Web20" Width="160px" onchange="dtpRecurToTime_SelectedDateChanged()">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                                                        <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                                        </TimeView>
                                                                        <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <%-- <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" Width=""
                                                                            AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>--%>
                                                                    </telerik:RadTimePicker>
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlRecDescription" runat="server" CssClass="Editabletxtbox">
                                                        <table>
                                                            <tr>
                                                                <td></td>
                                                                <td rowspan="3">
                                                                    <DLC:DLC ID="txtRecurringDescription" runat="server" Enable="True" TextboxHeight="60px"
                                                                        TextboxWidth="550px" Value="BLOCK DAYS DESCRIPTION"/>
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblRecurringDescription" runat="server"  CssClass="Editabletxtbox" mand="Yes"></asp:Label>
                                                                    <span class="MandLabelstyle">Description</span><span class="manredforstar">*</span>
                                                                </td>
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlRecurSave" runat="server" CssClass="Editabletxtbox">
                                                        <table style="width: 100%; height: 38px;">
                                                            <tr>
                                                                <td class="style2">
                                                                    <asp:HiddenField ID="hdnSelectedIndex" runat="server" />
                                                                </td>
                                                                <td class="style2"></td>
                                                                <td align="right" class="style1">
                                                                    <telerik:RadButton ID="btnSaveForRecurring" runat="server" OnClientClicked="SaveClick"
                                                                        Text="Save" OnClick="btnSaveForRecurring_Click"  Height="22px"  ><%--Font-Size="12px" CssClass="greenbutton teleriknormalbuttonstyle"  ButtonType="LinkButton"--%>
                                                                    </telerik:RadButton>
                                                                </td>
                                                                <td align="right" class="style2" style="width: 10%">
                                                                    <telerik:RadButton ID="btnClearForRecurring" runat="server" Text="Clear All" OnClientClicked="ClearAll" AutoPostBack="true" style="height: 31px !important;" 
                                                                        CssClass="redbutton teleriknormalbuttonstyle" ButtonType="LinkButton" font-size="12px" OnClick="btnClearForRecurring_Click">
                                                                    </telerik:RadButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>
                                <br />
                                <telerik:RadPageView ID="rpvBlockNonRecurring" runat="server" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlNonRecurBlockDays" runat="server" Font-Size="Small" GroupingText="Block Days"
                                                    Height="364px" Width="800px" BackColor="White" CssClass="Editabletxtbox">
                                                    <asp:Panel ID="pnlNonBlockDate" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblBlockDate" runat="server" Text="Block Date" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="dtpNonRecurDate" runat="server" Culture="en-US"
                                                                        EnableTyping="False" onchange="dtpNonRecurDate_SelectedDateChanged()" CssClass="Editabletxtbox">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" CssClass="Editabletxtbox">
                                                                        </Calendar>
                                                                        <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" LabelWidth="40%"
                                                                            ReadOnly="True" Width="" CssClass="Editabletxtbox">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>
                                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDateFormat" runat="server" Text="(Eg. 01-Jan-2010)" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlNonRecurBlockTime" runat="server" GroupingText="Block Time" BackColor="White"
                                                        Width="758px" CssClass="Panel">
                                                        <table style="width: 758px">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkNonRecurSelectTime" runat="server" Text="Select Time" AutoPostBack="True"
                                                                        Width="110px" OnCheckedChanged="chkNonRecurSelectTime_CheckedChanged" CssClass="Editabletxtbox"/>
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td></td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblNonRecurFromTime" runat="server" Text="From Time" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTimePicker ID="dtpNonRecurFromTime" runat="server"
                                                                        Culture="en-US" Height="25px" Skin="Web20" Width="160px" onchange="dtpNonRecurDate_SelectedDateChanged()" CssClass="Editabletxtbox">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                                                        <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                                        </TimeView>
                                                                        <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <%-- <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" Width=""
                                                                            AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>--%>
                                                                    </telerik:RadTimePicker>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblNonRecurToTime" runat="server" Text="To Time" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTimePicker ID="dtpNonRecurToTime" runat="server"
                                                                        Culture="en-US" Height="25px" Skin="Web20" Width="160px" onchange="dtpNonRecurDate_SelectedDateChanged()">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                                                        <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                                        </TimeView>
                                                                        <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <%-- <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" Width=""
                                                                            AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>--%>
                                                                    </telerik:RadTimePicker>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlNonRecDescription" runat="server">
                                                        <table style="width: 777px; margin-top: 0px;">
                                                            <tr>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td rowspan="3">
                                                                    <DLC:DLC ID="txtDescription" runat="server" Enable="True" TextboxHeight="180px" TextboxWidth="550px"
                                                                        Value="BLOCK DAYS DESCRIPTION"/>
                                                                </td>
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>

                                                                    <span class="MandLabelstyle">Description</span><span class="manredforstar">*</span>
                                                                  <%--  <asp:Label ID="lblDescription" runat="server" Text="Description*" CssClass="Editabletxtbox" mand="Yes"></asp:Label>--%>
                                                                </td>

                                                                   
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlNonRecButtons" runat="server">
                                                        <table style="width: 777px; height: 34px;">
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                                <td align="right">
                                                                    <telerik:RadButton ID="btnSaveForNonRecur" runat="server" Text="Save" OnClientClicked="SaveClick"
                                                                        OnClick="btnSaveForNonRecur_Click" Height="22px"> 
   
                                                                    </telerik:RadButton>


                                                                </td>
                                                                <td align="right" style="width: 10%">
                                                                    <telerik:RadButton ID="btnCancelForNonRecur" runat="server" Text="Clear All" OnClientClicked="ClearAll" AutoPostBack="true" CssClass="redbutton teleriknormalbuttonstyle" ButtonType="LinkButton" style="height: 30px !important;">
                                                                    </telerik:RadButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>
                                <br />
                                <telerik:RadPageView ID="rpvBlockTypeOfVisit" runat="server" Width="100%" Visible="false">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlBlockTypeofVisit" runat="server" Font-Size="Small" GroupingText="Block Days"
                                                    Height="367px" Width="777px" BackColor="White" CssClass="Panel">
                                                    <asp:Panel ID="pnlBlockTypeofVisitDate" runat="server">
                                                        <table style="width: 758px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFromDateTOF" runat="server" Text="From Date" Width="75px"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="dtpFromdateTOF" runat="server" AutoPostBack="True" Culture="en-US"
                                                                        EnableTyping="False" Height="21px" Width="147px" OnSelectedDateChanged="dtpFromdateTOF_SelectedDateChanged">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" LabelWidth="40%"
                                                                            ReadOnly="True" Width="" AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>
                                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTOVFromDateFormat" runat="server" Text="(Eg. 01-Jan-2010)"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblToDateTOF" runat="server" Text="To Date" Width="75px"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="dtpTodateTOF" runat="server" AutoPostBack="True" Culture="en-US"
                                                                        Height="21px" Width="147px" EnableTyping="False" OnSelectedDateChanged="dtpTodateTOF_SelectedDateChanged">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" LabelWidth="40%"
                                                                            ReadOnly="True" Width="" AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>
                                                                        <DatePopupButton HoverImageUrl="" ImageUrl="" CssClass="" />
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label6" runat="server" Text="(Eg. 01-Jan-2010)"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlTOVSelectDays" runat="server" GroupingText="Select Day(s)" BackColor="White"
                                                        Width="758px" CssClass="Panel">
                                                        <table style="width: 758px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkSundayTOF" runat="server" onclick="EnableSave();" Text="Sunday" Checked="true" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkMondayTOF" runat="server" onclick="EnableSave();" Text="Monday" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkTuesdayTOF" runat="server" onclick="EnableSave();" Text="Tuesday" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkWednesdayTOF" runat="server" onclick="EnableSave();" Text="Wednesday" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkThursdayTOF" runat="server" onclick="EnableSave();" Text="Thursday" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkFridayTOF" runat="server" onclick="EnableSave();" Text="Friday" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkSaturdayTOF" runat="server" onclick="EnableSave();" Text="Saturday" Checked="true" />
                                                                </td>                                                                
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlTOVSelectTime" runat="server" GroupingText="Block Time" BackColor="White"
                                                        Width="758px" CssClass="Panel">
                                                        <table style="width: 758px; height: 57px;">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chktypeofvisitSelectTime" runat="server" Text="Select Time" Width="100px"
                                                                        onclick="EnableSave();" AutoPostBack="True" OnCheckedChanged="chktypeofvisitSelectTime_CheckedChanged" />
                                                                </td>
                                                                <td></td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFromTimeTOF" runat="server" Text="From Time"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTimePicker ID="dtpFromTimeTOF" runat="server" AutoPostBack="True" Culture="en-US"
                                                                        Height="25px" Skin="Web20" Width="160px" OnSelectedDateChanged="dtpFromTimeTOF_SelectedDateChanged"
                                                                        AutoPostBackControl="TimeView">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                                                        <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                                        </TimeView>
                                                                        <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <%--<DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" Width=""
                                                                            AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>--%>
                                                                    </telerik:RadTimePicker>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblToTimeTOF" runat="server" Text="To Time"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTimePicker ID="dtpToTimeTOF" runat="server" AutoPostBack="True" Culture="en-US"
                                                                        Height="25px" Skin="Web20" Width="160px" OnSelectedDateChanged="dtpToTimeTOF_SelectedDateChanged"
                                                                        AutoPostBackControl="TimeView">
                                                                        <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False">
                                                                        </Calendar>
                                                                        <DatePopupButton CssClass="" HoverImageUrl="" ImageUrl="" Visible="False" />
                                                                        <TimeView CellSpacing="-1" Columns="7" Interval="00:10:00" TimeFormat="HH:mm">
                                                                        </TimeView>
                                                                        <TimePopupButton CssClass="" HoverImageUrl="" ImageUrl="" />
                                                                        <%-- <DateInput DateFormat="M/d/yyyy" DisplayDateFormat="M/d/yyyy" LabelWidth="64px" Width=""
                                                                            AutoPostBack="True">
                                                                            <EmptyMessageStyle Resize="None" />
                                                                            <ReadOnlyStyle Resize="None" />
                                                                            <FocusedStyle Resize="None" />
                                                                            <DisabledStyle Resize="None" />
                                                                            <InvalidStyle Resize="None" />
                                                                            <HoveredStyle Resize="None" />
                                                                            <EnabledStyle Resize="None" />
                                                                        </DateInput>--%>
                                                                    </telerik:RadTimePicker>
                                                                </td>
                                                                <td>&#160;&#160;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlTOVCatagory" runat="server">
                                                        <table style="width: 758px;">
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td align="right">
                                                                    <asp:Label ID="lblTypeofVisit" runat="server" ForeColor="Red" Text="Category for Block*"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadComboBox ID="cboTypeOfVisit" runat="server" Width="183px" OnClientSelectedIndexChanged="EnableSave">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlTOVDescription" runat="server">
                                                        <table>
                                                            <tr>
                                                                <td></td>
                                                                <td rowspan="3">
                                                                    <DLC:DLC ID="txtDescriptionTOF" runat="server" Enable="True" TextboxHeight="60px"
                                                                        TextboxWidth="550px" Value="BLOCK DAYS DESCRIPTION" />
                                                                </td>
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label9" runat="server" ForeColor="Black" Text="Description"></asp:Label>
                                                                </td>
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>&#160;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Panel ID="pnlTOVSave" runat="server" Height="60px" Style="margin-left: 3px"
                                                                        Visible="False" Width="758px">
                                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="Panel9" runat="server">
                                                        <table style="width: 758px;">
                                                            <tr>
                                                                <td></td>
                                                                <td></td>
                                                                <td></td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnSaveTOF" runat="server" OnClick="btnSaveTOF_Click" Text="Save"
                                                                        Width="74px" />
                                                                </td>
                                                                <td align="right" width="50">
                                                                    <asp:Button ID="btnClearTOF" runat="server" OnClick="btnClearTOF_Click" OnClientClick="return ClearAll();"
                                                                        Text="Clear All" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </td>
                    </tr>
                    <tr >
                        <td>
                            <asp:Button ID="btnShowBlockDetails" runat="server" Text="Show Block Details" Width="380px"
                                OnClientClick="return ShowResume();" CssClass="aspresizedbluebutton"/>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblId" runat="server" Visible="False" Style="display: none"></asp:Label>
                        </td>
                        <%--<td align="right">
                        </td>--%>
                        <td align="right"></td>
                        <td align="right">
                            <%--<telerik:RadButton ID="btnCancelForBlockDays" runat="server" Text="Cancel" OnClientClicked="CloseForBlockDays" AutoPostBack="false">
                            </telerik:RadButton>--%>

                            <button type="button" id="btnClose" value="Close" onclick="CloseForBlockDays();" class=" aspresizedredbutton">Close</button>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return CloseForBlockDays();" />
                <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                    <asp:Panel ID="Panel2" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="Label1" Text="" EnableViewState="false" runat="server"></asp:Label></center>
                        <br />
                        <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                            alt="Loading..." />
                        <br />
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdnToFindSource" runat="server" Value="" />
            <asp:HiddenField ID="hdnForTabClick" runat="server" />
            <asp:HiddenField ID="hdnMessageType" runat="server" Value="" />
            <asp:HiddenField ID="hdnNonRecur" runat="server" />
            <asp:HiddenField ID="hdnRecDescription" runat="server" />
            <asp:HiddenField ID="hdnNonRecurDescription" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnPhyID" runat="server" />
            <asp:HiddenField ID="hdnShowBlockDetaills" runat="server" />
            <asp:HiddenField runat="server" ID="hdnGruopId"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnTabName"></asp:HiddenField>
            <asp:HiddenField ID="hdnLocalTime" runat="server" />
            <asp:HiddenField ID="hdnBlockDayType" runat="server" />
            <asp:HiddenField runat="server" ID="hdnRecurFromDate"></asp:HiddenField>
            <asp:HiddenField ID="hdnBlockdaysId" runat="server" />
            <asp:HiddenField ID="hdnNonRecBlockDaysId" runat="server" />
            <asp:HiddenField ID="hdnRecur" runat="server" />
            <asp:HiddenField ID="hdnIndex" runat="server" />
            <asp:HiddenField ID="hdnFromTime" runat="server" />
            <asp:HiddenField ID="hdnToTime" runat="server" />
            <asp:HiddenField ID="hdnDays" runat="server" />
            <asp:HiddenField ID="hdnAlternateWeeks" runat="server" />
            <asp:HiddenField ID="hdnAlternateMonths" runat="server" />
            <asp:HiddenField ID="hdnFromDate" runat="server" />
            <asp:HiddenField ID="hdnToDate" runat="server" />
            <asp:HiddenField ID="hdnForMedicalAssistant" runat="server" />
            <asp:HiddenField ID="hdnSaveForDlc" runat="server" />
            <%--<asp:Button ID="btnAutosave" Style="display: none" runat="server" OnClick="btnAutosave_Click"
        Text="Autosave" />--%>
            <telerik:RadButton ID="btnHiddenForBlockDays" runat="server" Style="display: none" OnClick="btnHiddenForBlockDays_Click">
            </telerik:RadButton>
            <telerik:RadButton ID="btnInvisibleForBlockDays" runat="server" Style="display: none" OnClick="btnInvisibleForBlockDays_Click">
            </telerik:RadButton>
                <telerik:RadButton ID="btnInvisibleclear" runat="server" Style="display: none" OnClick="btnInvisibleclear_Click" >
                    </telerik:RadButton>
            <asp:HiddenField ID="hdnPhySelected" runat="server" />
            <asp:HiddenField ID="hdnTabChange" runat="server" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <%--  <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>--%>

            <script src="JScripts/jquery-1.11.3.min.js"></script>
            <script src="JScripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
            <script src="JScripts/bootstrap.min.js"></script>
            <script src="JScripts/JSBlockDays.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
