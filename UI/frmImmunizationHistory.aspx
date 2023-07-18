<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmImmunizationHistory.aspx.cs"
    Inherits="Acurus.Capella.UI.frmImmunizationHistory" EnableEventValidation="false"  ValidateRequest="false" %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Src="~/UserControls/CustomDateTimePicker.ascx" TagName="CustomDatePicker"
    TagPrefix="UC" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Immunization History</title>
  
<%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <style type="text/css">
        
        #DLC_listDLC
        {
            position:static !important;
            width:314px;
        }
        .RadGrid_Default {
            border: 1px solid #828282;
            background: #fff;
            color: #333;
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }
        .displayNone {
            display: none;
        }

        .style1 {
            width: 244px;
        }

        .underline {
            text-decoration: underline;
        }

        body {
            zoom: 1.0 !important;
            -moz-transform: scale(1) !important;
            -moz-transform-origin: 0 0 !important;
        }
         #tag:hover {
            text-decoration: underline;
        }
            #pnlDLC{
            display: -webkit-inline-box!important;
        }
    </style>

    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>

<%--Commented for bug id:23169--%>

<%--<body onkeydown="cancelBack()" oncontextmenu="return false">--%>
<body onload="ImmuHist_Load();OpenNotificationPopUp('PFSH-IMMUNIZATION_HISTORY');">


    <form id="frmImmunizationHistory" style="font-family: Microsoft Sans Serif; font-size: 8.5pt; background-color: White;"
        runat="server">
        <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server"
            IconUrl="Resources/16_16.ico">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Overlay="true"
                    Title="Immunization History" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlImmunizationHistory" BackColor="White" runat="server">

            <div style="height:525px;overflow-x:auto;overflow-y:auto;">
                <table style="width: 100%; height: 100%">
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" ScrollBars="None" runat="server" Font-Bold="true"
                                BackColor="White" GroupingText="Frequently used Immunization Procedures" Height="412 px"  CssClass="Editabletxtbox"
                                Width="500px" Style="position: relative;">
                                <table style="height: 170px;">
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="lblSelectImmunProcedure" EnableViewState="false" Font-Bold="false"  CssClass="Editabletxtbox"
                                                runat="server" Text="Select An Immunization Procedure"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <telerik:RadListBox ID="chklstImmunizationHistory" runat="server" Height="325px"
                                                Style="position: relative;" Width="470px" BackColor="White" Font-Bold="False" CssClass="Editabletxtbox"
                                                CheckBoxes="false"  AutoPostBack="True" AutoPostBackOnReorder="True" OnClientSelectedIndexChanged="chklstImmunizationHistory_SelectedIndexChanged">
                                                <ButtonSettings TransferButtons="All" />
                                            </telerik:RadListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <telerik:RadButton ID="btnManageFrequentlyUsedImmunProc" runat="server" Text="Manage Frequently used Immunizations"  

                                               

                                                Width="445px" Style="top: -1px; height: 26px !important; left: 0px; position: relative; padding: 4px 12px !important; font-size:12px !important" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">

                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="Panel2" Font-Bold="true" GroupingText="Immunization History Details"
                                BackColor="White" runat="server" Height="414px"  CssClass="Editabletxtbox">
                                <asp:Panel ID="Panel3" GroupingText="Vaccine Details" runat="server" Height="400px"
                                    BackColor="White" Width="615px"  CssClass="Editabletxtbox">
                                    <table style="height: 368px; width: 400px;">
                                        <tr>
                                            <td valign="top">
                                                <%--<asp:Label ID="lblImmunizationProcedure" EnableViewState="false" Width="75px" Font-Bold="false"  CssClass="Editabletxtbox"
                                                    runat="server" Text="Immunization Procedure*" mand="Yes"></asp:Label>--%>
                                                <span class="MandLabelstyle">Immunization Procedure</span><span class="manredforstar">*</span>
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadTextBox ID="txtImmunizationProcedure" CssClass="nonEditabletxtbox" EnableViewState="false" Font-Bold="false"
                                                    runat="server" Height="44px" Width="492px" ReadOnly="True" TextMode="MultiLine">
                                                    <ReadOnlyStyle   BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                        ForeColor="Black"  />
                                                    <HoveredStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                        ForeColor="Black" CssClass="nonEditabletxtbox" />
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblCVXcode" Font-Bold="false" EnableViewState="false" runat="server"  CssClass="Editabletxtbox"
                                                    Text="CVX code"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtCVXcode" EnableViewState="false"  CssClass="nonEditabletxtbox"  Font-Bold="false" runat="server"
                                                    Width="190px" Height="23px" ReadOnly="True">
                                                    <ReadOnlyStyle  BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                        />
                                                    <HoveredStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                         CssClass="nonEditabletxtbox" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="lblRouteOfAdminstration" Width="65px" EnableViewState="false" Font-Bold="false"  CssClass="Editabletxtbox"
                                                    runat="server" Text="Route of Adminstration"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cboRouteOfAdminstration" Font-Bold="false" runat="server"
                                                    Height="75px" Width="185px" OnClientSelectedIndexChanged="EnableSave"  CssClass="Editabletxtbox"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblVISGiven" Font-Bold="false" EnableViewState="false" runat="server"
                                                    Text="VIS Given"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkVisgiven" Font-Bold="false" runat="server" AutoPostBack="true" OnClick="ShowLoadingWaitCursor();"
                                                    Text=" " OnCheckedChanged="chkVisgiven_CheckedChanged" />
                                                <telerik:RadDatePicker ID="dpVisgiven" runat="server" Font-Bold="false" Height="25px" CssClass="Editabletxtbox"
                                                    DateInput-ReadOnly="true" Width="170px" Culture="English (United States)" DateInput-MinDate="1/1/1900 12:00:00 AM">
                                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x" RangeMinDate="1/1/1900 12:00:00 AM">
                                                    </Calendar>
                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                    <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                        Height="25px" LabelWidth="40%" type="text" value="" MinDate="1/1/1900 12:00:00 AM" CssClass="Editabletxtbox">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="lblDateOnVIS" Font-Bold="false" EnableViewState="false" runat="server" CssClass="Editabletxtbox"
                                                    Text="Date on VIS"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dpDateOnVIS" Font-Bold="false" runat="server" Width="190px"
                                                    DateInput-ReadOnly="true" Height="25px" Culture="English (United States)" MinDate="1/1/1900 12:00:00 AM">
                                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x" RangeMinDate="1/1/1900 12:00:00 AM">
                                                    </Calendar>
                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                    <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                        Height="25px" LabelWidth="40%" type="text" value="" MinDate="1/1/1900 12:00:00 AM">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblLotNumber" Font-Bold="false" EnableViewState="false" CssClass="Editabletxtbox"
                                                    runat="server" Text="Lot Number"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtLotNumber" Font-Bold="false" runat="server" Width="190px"
                                                    Height="25px" onkeypress="EnableSave();" EnableViewState="false" MaxLength="10" CssClass="Editabletxtbox" />
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="lblManufacturer" Font-Bold="false" EnableViewState="false" runat="server" CssClass="Editabletxtbox"
                                                    Text="Manufacturer"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<telerik:RadTextBox ID="txtManufacturer" Font-Bold="false" EnableViewState="false"
                                                runat="server" Height="25px" Width="185px" onkeypress="EnableSave();" MaxLength="1024" />--%>
                                                <telerik:RadComboBox ID="cboManufacturer" runat="server" OnClientSelectedIndexChanged="cboManufacturer_SelectedIndexChanged" Height="75px" Width="185px" CssClass="Editabletxtbox">
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblExpiryDate" Font-Bold="false" EnableViewState="false" runat="server" CssClass="Editabletxtbox"
                                                    Text="Expiration  Date"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dpExpiryDate" runat="server" EnableViewState="false" Width="195px" CssClass="Editabletxtbox"
                                                    Height="25px" DateInput-ReadOnly="true" Culture="English (United States)" MinDate="1/1/1900 12:00:00 AM">
                                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x" RangeMinDate="1/1/1900 12:00:00 AM">
                                                    </Calendar>
                                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                    <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                        Height="25px" LabelWidth="10px" type="text" value="" MinDate="1/1/1900 12:00:00 AM" ClientEvents-OnValueChanged="EnableSave">
                                                    </DateInput>
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="lblImmunizationSource" Font-Bold="false" runat="server" EnableViewState="false" CssClass="Editabletxtbox"
                                                    Text="Immunization Source "></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cboImmunizationSource" Font-Bold="false" runat="server" CssClass="Editabletxtbox"
                                                    Height="75px" Width="185px" OnClientSelectedIndexChanged="EnableSave" />
                                            </td>
                                        </tr>
                                           <tr>
                                            <td>
                                                 <asp:Label ID="lblAdminsteredAmount" Font-Bold="false" runat="server" EnableViewState="false" CssClass="Editabletxtbox"
                                                    Text="Administered Amount "></asp:Label>
                                            </td>
                                            <td>
                                                 <telerik:RadNumericTextBox ID="txtAdminAmt" runat="server" Width="190px" MaxLength="7" ClientEvents-OnKeyPress="AllowNumbers" CssClass="Editabletxtbox">
                                                                <NumberFormat ZeroPattern="n" DecimalDigits="2" DecimalSeparator="." KeepNotRoundedValue="True"
                                                                    KeepTrailingZerosOnFocus="True" />
                                                            </telerik:RadNumericTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAdminsteredUnit" Font-Bold="false" runat="server" EnableViewState="false" CssClass="Editabletxtbox"
                                                    Text="Administered Unit "></asp:Label>
                                            </td>
                                            <td>
                                               <telerik:RadComboBox ID="cboAdminUnit" runat="server"  Height="75px" Width="185px"  OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblAdminsteredDate" runat="server" EnableViewState="false" Width="20px" CssClass="Editabletxtbox"
                                                    Font-Bold="false" Text="Administered Date"></asp:Label>
                                            </td>
                                            <td valign="top">
                                                <UC:CustomDatePicker ID="cdtAdministeredDate"  runat="server"  />
                                                &nbsp;
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="Label2" Font-Bold="false" EnableViewState="false" runat="server" Text="Protection State" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cboProtectionState" Font-Bold="false" runat="server" Height="75px"
                                                    Style="position: static;" Width="185px" OnClientSelectedIndexChanged="EnableSave"  CssClass="Editabletxtbox"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblLocation" Font-Bold="false" EnableViewState="false" runat="server" CssClass="Editabletxtbox"
                                                    Text="Location"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="cboLocation" Font-Bold="false" runat="server" Height="75px" CssClass="Editabletxtbox"
                                                    Style="position: static;" Width="185px" OnClientSelectedIndexChanged="EnableSave" />
                                            </td>
                                            <td valign="top">
                                                <asp:Label ID="Label14" runat="server" Font-Bold="false" EnableViewState="false" CssClass="Editabletxtbox"
                                                    Text="Dose# to Total Dose#"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            <telerik:RadNumericTextBox ID="txtDose" runat="server" Width="90px" Height="25px" CssClass="Editabletxtbox"
                                                EnableViewState="false" ClientEvents-OnKeyPress="AllowNumbers" MaxLength="2">
                                                <NumberFormat DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True" KeepTrailingZerosOnFocus="True"
                                                    ZeroPattern="n" />
                                            </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblDose" runat="server" EnableViewState="false" Text="/"></asp:Label>
                                                <telerik:RadComboBox ID="cboDosetotal" runat="server" Height="75px" Width="75px" CssClass="Editabletxtbox"
                                                    OnClientSelectedIndexChanged="EnableSave" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblNotes" Font-Bold="false" runat="server" EnableViewState="false" CssClass="Editabletxtbox"
                                                    Text="Notes"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White" CssClass="Editabletxtbox"
                                                    Font-Size="Small" Font-Bold="false">
                                                    <DLC:DLC ID="DLC" runat="server" TextboxHeight="44px" TextboxWidth="310px" Value="IMMUNIZATIONHISTORY NOTES" TextboxMaxLength="300" />
                                                </asp:Panel>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td align="right">
                                                <%--//cap-632 : Design Changes--%>
                                                <telerik:RadButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClicked="saveEnabled" 
                                                    Text="Add"  AccessKey="A" CssClass="greenbutton teleriknormalbuttonstyle" Style="margin-left: 0px; top: -48px; left: 0px; position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; height: 29px !important; font-size: 13px !important; width: 58px;" ButtonType="LinkButton" >
                                                        <ContentTemplate>
                                                        <span id="SpanAdd" runat="server">A</span><span id="SpanAdditionalword" runat="server" class="changebuttoncolor">dd</span>
                                                    </ContentTemplate>
                                                     </telerik:RadButton>
                                                 <%--//cap-632 : Design Changes--%>
                                                <telerik:RadButton ID="btnClearAll" runat="server" AutoPostBack="false" OnClientClicked="btnClearAll_Clicked"
                                                    EnableViewState="false"
                                                     AccessKey="C" CssClass="redbutton teleriknormalbuttonstyle" Style="position: static; text-align: center; top: -48px; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;  height: 29px !important; font-size: 13px !important;" ButtonType="LinkButton" >
                                                    <ContentTemplate>
                                                        <span id="SpanClear" runat="server" >C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                                    </ContentTemplate>
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width: 100%; height: 160px;">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlImmunizationHistoryDetails" runat="server" BorderWidth="1px" GroupingText="Immunization"
                                            Height="200px" Width="100%" Style="margin-left: 1px; margin-right: 1px; margin-top: 1px; margin-bottom: 1px;"
                                            BackColor="White" Font-Size="8.5pt" Font-Bold="true">
                                            <telerik:RadGrid ID="grdImmunization" runat="server" AutoGenerateColumns="False"
                                                EnableTheming="False" CellSpacing="0" GridLines="Both" OnItemCommand="grdImmunization_ItemCommand"
                                                Width="99%"  Height="150px" CssClass="Gridbodystyle">
                                                <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle" />
                                                <ItemStyle CssClass="Gridbodystyle" />
                                                <%--BackColor="#a4d9ff"--%>
                                                <FilterMenu EnableImageSprites="False">
                                                </FilterMenu>
                                                <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="false">
                                                    <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                                    <Selecting AllowRowSelect="true" />
                                                    <ClientEvents OnCommand="grdImmunization_OnCommand" />
                                                </ClientSettings>
                                                <MasterTableView>
                                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="EditRows" ImageUrl="~/Resources/edit.gif"
                                                            FilterControlAltText="Filter EditRows column" HeaderText="Edit" UniqueName="EditRows"
                                                            DataTextField="EditRows" Resizable="False">
                                                            <HeaderStyle Width="5%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle  BorderStyle="Dotted" CssClass="Gridbodystyle"  />

                                                        </telerik:GridButtonColumn>
                                                        <telerik:GridButtonColumn ButtonType="ImageButton" ConfirmText="Are you sure you want to delete this Immunization History?"
                                                            ConfirmDialogType="RadWindow" ConfirmTitle="Immunization History" CommandName="DeleteRows"
                                                            ImageUrl="~/Resources/close_small_pressed.png" FilterControlAltText="Filter DeleteRows column"  ConfirmDialogHeight="155px"
                                                            HeaderText="Del" UniqueName="DeleteRows" DataTextField="DeleteRows" Resizable="False">
                                                            <HeaderStyle Width="7%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle BorderStyle="Dotted" CssClass="Gridbodystyle"/>
                                                        </telerik:GridButtonColumn>
                                                        <telerik:GridBoundColumn DataField="Immunization Procedure" FilterControlAltText="Filter ImmunizationProcedure column"
                                                            HeaderText="Immunization Procedure" UniqueName="ImmunizationProcedure" Resizable="False">
                                                            <HeaderStyle Width="40%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle  BorderStyle="Dotted" CssClass="Gridbodystyle"  />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Administered Date" FilterControlAltText="Filter AdministeredDate column"
                                                            HeaderText="Administered Date" UniqueName="AdministeredDate" Resizable="False">
                                                            <HeaderStyle Width="20%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle  BorderStyle="Dotted" CssClass="Gridbodystyle"  />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Dose" FilterControlAltText="Filter Dose column"
                                                            HeaderText="Dose" UniqueName="Dose" Resizable="False">
                                                            <HeaderStyle Width="18%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle BorderStyle="Dotted"  CssClass="Gridbodystyle" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Route of Administration" FilterControlAltText="Filter SurgeryNotes column"
                                                            HeaderText="Route of Administration" UniqueName="RouteofAdministration" Resizable="False">
                                                            <HeaderStyle Width="30%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle BorderStyle="Dotted" CssClass="Gridbodystyle" />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Immunization Source" FilterControlAltText="Filter ImmunizationSource column"
                                                            HeaderText="Immunization Source" UniqueName="ImmunizationSource" Resizable="False">
                                                            <HeaderStyle Width="30%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle  BorderStyle="Dotted" CssClass="Gridbodystyle"  />
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="Id" FilterControlAltText="Filter Id column" HeaderText="Id"
                                                            UniqueName="Id" Display="false" Resizable="False">
                                                            <HeaderStyle Width="5%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle  BorderStyle="Dotted" CssClass="Gridbodystyle" />
                                                        </telerik:GridBoundColumn>
                                                         <telerik:GridBoundColumn DataField="Immunization_Order_ID" FilterControlAltText="Filter Id column" HeaderText="Id"
                                                            UniqueName="Id" Display="false" Resizable="False">
                                                            <HeaderStyle Width="5%" CssClass="Gridheaderstyle" />
                                                            <ItemStyle  BorderStyle="Dotted" CssClass="Gridbodystyle" />
                                                        </telerik:GridBoundColumn>
                                                    </Columns>
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <%-- <PageNavigator:PageNavigator ID="mpnImmunizationHistory" runat="server" OnFirst="FirstPageNavigator" />--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="height:60px;margin-left:-8px;">
               <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Immunization_Injection');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                </div>
                <div id="tag" style="margin-top: 13px; margin-left: -99px; font-size: 19px; height: 48px; width: 303px; font-weight: bold; color: #6504d0; border-radius: 7px; cursor: pointer; font-family: source sans pro;" onclick="OpenMeasurePopup('Immunization_Injection');">
                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                </div>
            </div>
            <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel4" runat="server">
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
        </div>    --%>
            <telerik:RadScriptManager EnableViewState="false" ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
        </telerik:RadAjaxPanel>
        <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
        <asp:HiddenField ID="HiddenField1" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="HiddenField2" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" EnableViewState="false" runat="server" />
        <asp:Button ID="InvisibleClearAllButton" runat="server" CssClass="displayNone" OnClick="InvisibleClearAllButton_Click" />
        <asp:HiddenField ID="Hiddenupdate" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSImmunizationHistory.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDateTimePicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDateTimePicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <script type="text/javascript">
            Telerik.Web.UI.RadListBox.prototype.saveClientState = function () {
                return "{" +
                            "\"isEnabled\":" + this._enabled +
                            ",\"logEntries\":" + this._logEntriesJson +
                           ",\"selectedIndices\":" + this._selectedIndicesJson +
                           ",\"checkedIndices\":" + this._checkedIndicesJson +
                           ",\"scrollPosition\":" + Math.round(this._scrollPosition) +
                       "}";
            }
        </script>
    </form>
</body>
</html>
