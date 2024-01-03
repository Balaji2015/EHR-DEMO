<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOrderManagement.aspx.cs"
    Inherits="Acurus.Capella.UI.frmOrder" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Order Management</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .RadPicker
        {
            vertical-align: middle;
        }
        .RadPicker
        {
            vertical-align: middle;
        }
        .RadPicker .rcTable
        {
            table-layout: auto;
        }
        .RadPicker .rcTable
        {
            table-layout: auto;
        }
        .RadPicker_Default .rcCalPopup
        {
            background-position: 0 0;
        }
        .RadPicker_Default .rcCalPopup
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }
        .RadPicker .rcCalPopup
        {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }
        .RadPicker_Default .rcCalPopup
        {
            background-position: 0 0;
        }
        .RadPicker_Default .rcCalPopup
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }
        .RadPicker .rcCalPopup
        {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }
        .RadPicker_Default .rcTimePopup
        {
            background-position: 0 -100px;
        }
        .RadPicker_Default .rcTimePopup
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }
        .RadPicker .rcTimePopup
        {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }
        .RadPicker_Default .rcTimePopup
        {
            background-position: 0 -100px;
        }
        .RadPicker_Default .rcTimePopup
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }
        .RadPicker .rcTimePopup
        {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }
        .RadComboBox_Default
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox
        {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }
        .RadComboBox
        {
            text-align: left;
        }
        .RadComboBox_Default
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox
        {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }
        .RadComboBox
        {
            text-align: left;
        }
        .RadComboBox_Default
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox
        {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }
        .RadComboBox
        {
            text-align: left;
        }
        .RadComboBox_Default
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox
        {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }
        .RadComboBox
        {
            text-align: left;
        }
        .RadComboBox *
        {
            margin: 0;
            padding: 0;
        }
        .RadComboBox *
        {
            margin: 0;
            padding: 0;
        }
        .RadComboBox *
        {
            margin: 0;
            padding: 0;
        }
        .RadComboBox *
        {
            margin: 0;
            padding: 0;
        }
        .RadComboBox .rcbReadOnly .rcbInput
        {
            cursor: default;
        }
        .RadComboBox .rcbReadOnly .rcbInput
        {
            cursor: default;
        }
        .RadComboBox .rcbReadOnly .rcbInput
        {
            cursor: default;
        }
        .RadComboBox .rcbReadOnly .rcbInput
        {
            cursor: default;
        }
        .RadComboBox_Default .rcbInput
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox .rcbInput
        {
            text-align: left;
        }
        .RadComboBox_Default .rcbInput
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox .rcbInput
        {
            text-align: left;
        }
        .RadComboBox_Default .rcbInput
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox .rcbInput
        {
            text-align: left;
        }
        .RadComboBox_Default .rcbInput
        {
            font: 12px "Segoe UI" ,Arial,sans-serif;
            color: #333;
        }
        .RadComboBox .rcbInput
        {
            text-align: left;
        }
        .rcSingle .riSingle
        {
            white-space: normal;
        }
        .rcSingle .riSingle
        {
            white-space: normal;
        }
        .rcSingle .riSingle
        {
            white-space: normal;
        }
        .RadPicker .RadInput
        {
            vertical-align: baseline;
        }
        .rcSingle .riSingle
        {
            white-space: normal;
        }
        .RadPicker .RadInput
        {
            vertical-align: baseline;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .RadInput_Default
        {
            font: 12px "segoe ui" ,arial,sans-serif;
        }
        .riSingle
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle
        {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }
        .RadInput
        {
            vertical-align: middle;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .riSingle .riTextBox
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }
        .modal
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
        .Panel legend
        {
            font-weight: bold;
        }
        .style22
        {
            width: 650px;
            height: 34px;
        }
        .style10
        {
            width: 11px;
            height: 34px;
        }
        .style11
        {
            width: 4px;
            height: 34px;
        }
        .style26
        {
            width: 456px;
            height: 34px;
        }
        .style20
        {
            height: 34px;
        }
        .style23
        {
            width: 361px;
            height: 34px;
        }
        .style7
        {
            width: 11px;
        }
        .style8
        {
            width: 4px;
        }
        .style9
        {
            width: 456px;
            height: 27px;
        }
        .style25
        {
            width: 352px;
        }
        .style15
        {
            width: 602px;
            height: 28px;
        }
        .style13
        {
            width: 160px;
            height: 28px;
        }
        .style14
        {
            width: 11px;
            height: 28px;
        }
        .TextWrapAll
        {
            overflow-wrap: break-word !important;
        }
    </style>
     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body class="bodybackground" onload="OrderMgmtLoad();">
    <form id="form1" runat="server">
    <telerik:RadWindowManager ID="OrderManagementWindowManager" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="OrderManagementWindow" runat="server" Modal="true" VisibleOnPageLoad="false"
                Behaviors="Close" IconUrl="Resources/16_16.ico" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
   
    <div>
        <asp:Panel ID="pnlOrderManagementDetails" runat="server" GroupingText="Order Management Details"
            CssClass="Panel LabelStyleBold">
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel1" runat="server">
                            <table bgcolor="White">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlSearchParameters" runat="server" GroupingText="Search Parameters"
                                            CssClass="Panel LabelStyleBold">
                                            <table bgcolor="White">
                                                <tr>
                                                    <td class="style22">
                                                        <asp:Label ID="lblOrderType" runat="server" Text="Order Type*" Width="100%" CssClass="Editabletxtbox"
                                                            EnableViewState="false"  mand="Yes"></asp:Label>
                                                    </td>
                                                    <td class="style10" colspan="2">
                                                        <telerik:RadComboBox ID="cboOrderType" runat="server" AutoPostBack="True" Height="75px" CssClass="Editabletxtbox"
                                                            OnSelectedIndexChanged="cboOrderType_SelectedIndexChanged" Width="100%" OnClientSelectedIndexChanged="cboOrderType_SelectedIndexChanged">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td class="style11">
                                                    </td>
                                                    <td class="style26">
                                                        <asp:Label ID="lblOrderStatus" runat="server" Text="Order Status" Width="100%" EnableViewState="false"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style10" colspan="3">
                                                        <telerik:RadComboBox ID="cboOrderStatus" runat="server" Height="90px" Width="100%"  CssClass="Editabletxtbox">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td class="style20">
                                                        <asp:Label ID="lblFacility" runat="server" Text="Facility Name" Width="100%"  CssClass="Editabletxtbox"
                                                            EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td class="style20">
                                                        <telerik:RadComboBox ID="cboFacilityName" runat="server" Height="150px"  CssClass="Editabletxtbox">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style22">
                                                        <asp:CheckBox ID="chkDate" runat="server" Text="Arrival Date Range" Width="100%"  CssClass="Editabletxtbox"
                                                            onclick="return CheckedChange();" AutoPostBack="True" />
                                                    </td>
                                                    <td class="style23">
                                                        <asp:Label ID="lblOrderDate" runat="server" Text="From Date" Width="100%" EnableViewState="false"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style10">
                                                        <telerik:RadDateTimePicker ID="dtpFromDate" runat="server" Culture="English (United States)"
                                                            Width="150px" Enabled="False" Height="22px"  CssClass="Editabletxtbox">
                                                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                            </Calendar>
                                                            <TimeView CellSpacing="-1">
                                                            </TimeView>
                                                            <TimePopupButton HoverImageUrl="" ImageUrl="" Visible="False" />
                                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                            <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                LabelWidth="40%" type="text" value="" Height="22px" MaxLength="11">
                                                                <EmptyMessageStyle Resize="None" />
                                                                <ReadOnlyStyle Resize="None" />
                                                                <FocusedStyle Resize="None" />
                                                                <DisabledStyle Resize="None" />
                                                                <InvalidStyle Resize="None" />
                                                                <HoveredStyle Resize="None" />
                                                                <EnabledStyle Resize="None" />
                                                            </DateInput></telerik:RadDateTimePicker>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td class="style26">
                                                        <asp:Label ID="lblToDate" runat="server" Text="To Date" Width="100%" EnableViewState="false"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style10" colspan="2">
                                                        <telerik:RadDateTimePicker ID="dtpToDate" runat="server" Culture="English (United States)"
                                                            Height="22px" Width="150px"  CssClass="Editabletxtbox">
                                                            <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                            </Calendar>
                                                            <TimeView CellSpacing="-1">
                                                            </TimeView>
                                                            <TimePopupButton HoverImageUrl="" ImageUrl="" Visible="False" />
                                                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                            <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                Height="22px" LabelWidth="40%" type="text" value="" MaxLength="11">
                                                                <EmptyMessageStyle Resize="None" />
                                                                <ReadOnlyStyle Resize="None" />
                                                                <FocusedStyle Resize="None" />
                                                                <DisabledStyle Resize="None" />
                                                                <InvalidStyle Resize="None" />
                                                                <HoveredStyle Resize="None" />
                                                                <EnabledStyle Resize="None" />
                                                            </DateInput></telerik:RadDateTimePicker>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style27">
                                                        <asp:Label ID="lblPatientName" runat="server" Height="19px" Text="Patient Name" Width="100%"
                                                            EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style7" colspan="2">
                                                        <telerik:RadTextBox ID="txtPatientName" runat="server" Height="20px" tag="tagPatientName"
                                                            Width="100%" BorderWidth="1px" EnableViewState="false" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td class="style8">
                                                        <asp:ImageButton ID="pbClearPatientName" runat="server" ImageUrl="~/Resources/close_small_pressed.png"
                                                            OnClick="pbClearPatientName_Click" OnClientClick="LoadIcon();" />
                                                    </td>
                                                    <td class="style9">
                                                        <asp:Button ID="btnFindPatient" runat="server" OnClientClick="return FindPatient();"
                                                            Text="Find Patient" EnableViewState="false" CssClass="aspresizedbluebutton" />
                                                    </td>
                                                    <td class="style25">
                                                        <asp:Label ID="lblProviderName" runat="server" Text="Provider Name" Width="100%"
                                                            EnableViewState="false"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style15">
                                                        <telerik:RadTextBox ID="txtProviderName" runat="server" Height="20px" Style="margin-left: 0px"
                                                            tag="tagProviderName" Width="100%"  BorderWidth="1px"
                                                            EnableViewState="false" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td class="style13">
                                                        <asp:ImageButton ID="pbClearProviderName" runat="server" ImageUrl="~/Resources/close_small_pressed.png"
                                                            OnClick="pbClearProviderName_Click" OnClientClick="LoadIcon();" />
                                                    </td>
                                                    <td class="style14">
                                                        <asp:Button ID="btnFindProvider" runat="server" OnClientClick="return OpenFindPhysician();"
                                                            Text="Find Provider" Width="100px" EnableViewState="false" CssClass="aspresizedbluebutton" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style22">
                                                        <asp:Label ID="lblLabCenter" runat="server" Text="Lab/Imaging Center" Width="100%"
                                                            EnableViewState="false"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td class="style10" colspan="2">
                                                        <telerik:RadComboBox ID="cboLabCenter" runat="server" AutoPostBack="false" Height="150px"  CssClass="Editabletxtbox"
                                                            Width="100%">
                                                        </telerik:RadComboBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td colspan="2">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style27" colspan="3">
                                                        <asp:Label ID="lblResultsFound" runat="server" Text="Results" Width="100%" Font-Bold="true"
                                                            EnableViewState="false"  CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="style14">
                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="Load();"
                                                            Text="Search" Width="100px" CssClass="aspresizedbluebutton" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClearAll" runat="server" OnClientClick="return Clear();" Text="Clear All"
                                                            Width="100px" CssClass="aspresizedredbutton"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlReport" runat="server" GroupingText="Search Results" CssClass="Panel LabelStyleBold">
                                            <telerik:RadGrid ID="grdReport" runat="server" Height="235px" AutoGenerateColumns="False"
                                                CellSpacing="0"  GridLines="None" OnNeedDataSource="grdReport_NeedDataSource"
                                                OnItemCommand="grdReport_ItemCommand" CssClass="Gridbodystyle">
                                                <ClientSettings EnablePostBackOnRowClick="True">
                                                    <ClientEvents OnRowClick="grdReport_OnRowClick"/>
                                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                                    <Selecting AllowRowSelect="True" />
                                                </ClientSettings>
                                                <MasterTableView DataKeyNames="GroupID" GridLines="Both">
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="Control" FilterControlAltText="Filter FormRequired column"
                                                            HeaderText="Control#" UniqueName="Control" Display="False">
                                                            <HeaderStyle Width="6%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle Width="6%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Arrival Date" FilterControlAltText="Filter FormAvailable column"
                                                            HeaderText="Arrival Date" UniqueName="ArrivalDate">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Order Type" FilterControlAltText="Filter AppointmentDate column"
                                                            HeaderText="Order Type" UniqueName="OrderType">
                                                            <HeaderStyle Width="6%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="6%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Order Status" FilterControlAltText="Filter PatientA/CNo column"
                                                            HeaderText="Order Status" UniqueName="OrderStatus">
                                                            <HeaderStyle Width="6%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="6%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Patient Acc" FilterControlAltText="Filter PatientName column"
                                                            HeaderText="Patient Acc" UniqueName="PatientAcc">
                                                            <HeaderStyle Width="4%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="4%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Patient Name" FilterControlAltText="Filter PatientDOB column"
                                                            HeaderText="Patient Name" UniqueName="PatientName">
                                                            <HeaderStyle Width="6%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="6%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Patient DOB" FilterControlAltText="Filter HomePhoneNo column"
                                                            HeaderText="Patient DOB" UniqueName="PatientDOB">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Procedure/Rx/Reason for referral" FilterControlAltText="Filter TestID column"
                                                            HeaderText="Procedure/Rx/ Reason for referral" UniqueName="Procedure/Rx/Reasonforreferral">
                                                            <HeaderStyle Width="8%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="ICD" FilterControlAltText="Filter CPT column"
                                                            HeaderText="ICD" UniqueName="ICD">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Ordering Provider" FilterControlAltText="Filter OrderingPhysician column"
                                                            HeaderText="Ordering Provider" UniqueName="OrderingProvider">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Attending Provider" FilterControlAltText="Filter OrderingClinic column"
                                                            HeaderText="Attending Provider" UniqueName="AttendingProvider">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Ordering Facility" FilterControlAltText="Filter OrderingFacility column"
                                                            HeaderText="Ordering Facility" UniqueName="OrderingFacility">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Ref to Facility/Lab/Pharmacy" FilterControlAltText="Filter ReftoFacility/Pharmacy column"
                                                            HeaderText="Ref to Facility/Lab/ Pharmacy" UniqueName="ReftoFacility/Pharmacy">
                                                            <HeaderStyle Width="6%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="6%" CssClass="TextWrapAll" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Specimen" FilterControlAltText="Filter Specimen column"
                                                            HeaderText="Specimen" UniqueName="Specimen" Display="False">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="LabId" FilterControlAltText="Filter LabId column"
                                                            HeaderText="LabId" UniqueName="LabId" Display="False">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" 
                                                            FilterControlAltText="Filter Print Requisition column" Text="Print Requisition"
                                                            UniqueName="PrintEReq" ImageUrl="~/Resources/PrintReq.png" Display="False" HeaderText="Print Req."
                                                            CommandArgument="PrintRequisition" CommandName="PrintEReq" ButtonCssClass="loaderClass">
                                                            <HeaderStyle Width="3%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="2%" />
                                                        </telerik:GridButtonColumn>
                                                        <telerik:GridBoundColumn DataField="Physician ID" FilterControlAltText="Filter PhysicianID column"
                                                            HeaderText="Physician ID" UniqueName="PhysicianID" Display="False">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle" />
                                                            <ItemStyle Width="5%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Encounter ID" FilterControlAltText="Filter EncounterID column"
                                                            HeaderText="Encounter ID" UniqueName="EncounterID" Display="False">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="5%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Specimen Collected Date Time" FilterControlAltText="Filter SpecimenCollectedDateTime column"
                                                            HeaderText="Specimen Collected Date Time" UniqueName="SpecimenCollectedDateTime"
                                                            Display="False">
                                                            <HeaderStyle Width="5%"  CssClass="Gridheaderstyle" />
                                                            <ItemStyle Width="5%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" FilterControlAltText="Filter ViewResult column"
                                                            Text="View Result" UniqueName="ViewResult" ImageUrl="~/Resources/Down.bmp" HeaderText="View Result"
                                                            CommandArgument="ViewResult" CommandName="ViewResults" ButtonCssClass="loaderClass">
                                                            <HeaderStyle Width="3%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="3%" />
                                                        </telerik:GridButtonColumn>
                                                        <telerik:GridBoundColumn DataField="Index Order ID" FilterControlAltText="Filter IndexOrderID column"
                                                            HeaderText="Index Order ID" UniqueName="IndexOrderID" Display="False">
                                                            <HeaderStyle Width="3%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="3%" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="IsElectronic Signature" FilterControlAltText="Filter IsElectronicSignature column"
                                                            HeaderText="IsElectronic Signature" UniqueName="IsElectronicSignature" Display="False">
                                                            <HeaderStyle Width="3%"  CssClass="Gridheaderstyle"/>
                                                            <ItemStyle Width="3%" />
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="Editabletxtbox">
                        <PageNavigator:PageNavigator ID="mpnOrderManagement" runat="server" OnFirst="FirstPageNavigator" />
                    </td>
                    <td align="right">
                        <asp:Panel ID="pan" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddResult" runat="server" OnClick="btnAddResult_Click" Text="Import Result"
                                            Width="100px" CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrintToPDF" runat="server" OnClientClick="return btnExcelClick();"
                                            OnClick="btnPrintToPDF_Click" Text="Print To PDF" Width="100px" CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExportToExcel" runat="server" OnClick="btnExportToExcel_Click"
                                            OnClientClick="return btnExcelClick();" Text="Export To Excel" Width="133px" CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" Width="100px" CssClass="aspresizedredbutton"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>

        <asp:Button ID="btnClear" runat="server" style="display:none;" OnClick="btnClear_Click" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableViewState="false">
    </asp:ScriptManager>
    <asp:HiddenField ID="hdnTransferVaraible" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnPatientValues" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="SelectedItem" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnSelectedItem" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnResultsLabel" runat="server" EnableViewState="false" />
    <asp:HiddenField ID="hdnCurrentProcess" runat="server" />
     <asp:HiddenField ID="hdnOrder" runat="server" />
    <br />
    <asp:HiddenField ID="hdnOrderStatus" runat="server" EnableViewState="false" />
    <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
        <asp:Panel ID="Panel2" runat="server">
            <br />
            <br />
            <br />
            <br />
            <center>
                <asp:Label ID="Label1" Text="" runat="server" EnableViewState="false"></asp:Label></center>
            <br />
            <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                alt="Loading..." />
            <br />
        </asp:Panel>
    </div>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <link href="CSS/jquery-ui.css" rel="stylesheet" />
     <script src="JScripts/jquery-1.11.3.min.js"></script>
            <script src="JScripts/jquery-ui.js"></script>
            <script src="JScripts/bootstrap.min.js"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSC5PO.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSOrderManagement.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   
   </asp:PlaceHolder>
     
    </form>
</body>
</html>
