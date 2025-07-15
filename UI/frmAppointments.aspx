<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAppointments.aspx.cs" 
    MasterPageFile="~/C5PO.Master" Inherits="Acurus.Capella.UI.frmAppointments" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">--%><asp:Content ID="Appointments" runat="server"
    ContentPlaceHolderID="C5POBody">
    <head>
        <title>Appointments</title>
        <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
        <style type="text/css">
            label{
                display:inline!important;
            }
            .scrollingControlContainer {
                overflow-x: hidden;
                overflow-y: scroll;
            }

            .MyClass {
                background-color: Grey !important;
            }

            .scrollingCheckBoxList {
                border: 1px #808080 solid;
                margin: 10px 10px 10px 10px;
                height: 300px;
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

            .RadScheduler_Vista .rsContentTable .rsNonWorkHour {
                background-color: #BCADAD;
            }

            .holder {
                min-height: 0;
                _zoom: 1;
            }

                .holder:after {
                    content: "";
                    height: 0;
                    clear: both;
                    display: block;
                }

            #scheduler-container {
                width: 600px;
                float: left;
            }

            #settings-container {
                width: 250px;
                float: right;
            }

            .ConfigurationPanel1 .ConfigSection li {
                line-height: 24px;
            }

                .ConfigurationPanel1 .ConfigSection li select {
                    margin-left: 20px;
                    float: right;
                    width: 80px;
                }

            .ConfigurationPanel1 .ConfigSection {
                margin-left: 10px;
                list-style: none;
                width: 220px;
                float: left;
                padding-left: 11px;
                border-left: solid 1px #b1d8eb;
                height: 220px;
                padding-right: 0;
                padding-top: 0;
                padding-bottom: 0;
            }
            /*Day, Week, and MonthView*/ div.rsContent .rsCell.MyCustomClass {
                background: orange !important;
            }
            /*Timeline view*/ div.rsContent .rsTimelineTable .MyCustomClass {
                background: orange !important;
            }

            .White {
                background: White !important;
            }

            .AliceBlue {
                background: AliceBlue !important;
            }

            .AntiqueWhite {
                background: AntiqueWhite !important;
            }

            .Aqua {
                background: Aqua !important;
            }

            .Bisque {
                background: Bisque !important;
            }

            .BlanchedAlmond {
                background: BlanchedAlmond !important;
            }

            .BurlyWood {
                background: BurlyWood !important;
            }

            .CadetBlue {
                background: CadetBlue !important;
            }

            .CornflowerBlue {
                background: CornflowerBlue !important;
            }

            .Crimson {
                background: Crimson !important;
            }

            .DarkGray {
                background: DarkGray !important;
            }

            .DarkSalmon {
                background: DarkSalmon !important;
            }

            .DimGray {
                background: DimGray !important;
            }

            .Firebrick {
                background: Firebrick !important;
            }

            .Fuchsia {
                background: Fuchsia !important;
            }

            .GhostWhite {
                background: GhostWhite !important;
            }

            .Gold {
                background: Gold !important;
            }

            .Goldenrod {
                background: Goldenrod !important;
            }

            .Honeydew {
                background: Honeydew !important;
            }

            .HotPink {
                background: HotPink !important;
            }

            .Indigo {
                background: Indigo !important;
            }

            .Khaki {
                background: Khaki !important;
            }

            .Lavender {
                background: Lavender !important;
            }

            .LemonChiffon {
                background: LemonChiffon !important;
            }

            .LightCyan {
                background: LightCyan !important;
            }

            .LightGoldenrodYellow {
                background: LightGoldenrodYellow !important;
            }

            .Lime {
                background: Lime !important;
            }

            .Maroon {
                background: Maroon !important;
            }

            .MediumOrchid {
                background: MediumOrchid !important;
            }

            .MintCream {
                background: MintCream !important;
            }

            .Moccasin {
                background: Moccasin !important;
            }

            .Navy {
                background: Navy !important;
            }

            .Olive {
                background: Olive !important;
            }

            .Orchid {
                background: Orchid !important;
            }

            .PaleTurquoise {
                background: PaleTurquoise !important;
            }

            .PeachPuff {
                background: PeachPuff !important;
            }

            .Peru {
                background: Peru !important;
            }

            .RoyalBlue {
                background: RoyalBlue !important;
            }

            .SeaShell {
                background: SeaShell !important;
            }

            .Tan {
                background: Tan !important;
            }

            .Tomato {
                background: Tomato !important;
            }

            .Turquoise {
                background: Turquoise !important;
            }

            .Wheat {
                background: Wheat !important;
            }

            .style1 {
                height: 20px;
            }

            .zindtex {
                position: relative;
                z-index: 1;
            }

            .RadScheduler .rsTopWrap {
                width: 966px;
                z-index: 1;
            }

            .RadScheduler .rsAptEditSizingWrapper {
                display: none;
                visibility: hidden;
            }

            .RadScheduler .rsMonthView .rsWrap {
                overflow: hidden;
                font-size: 0;
                line-height: 0;
            }

            .rowspace {
                margin-top: 0.5%;
                margin-bottom: 0.5%;
            }

            caption {
                padding: 0px !important;
            }

            .panel {
                margin-bottom: 0px !important;
                margin-left: 1%;
                margin-right: 1%;
            }

            .panel-headApp, .panel-footer {
                padding: 6px 10px !important;
            }

            #ctl00_C5POBody_updtPnlCalender {
                display: inline;
            }

            body {
                zoom: 1.0 !important;
                -moz-transform: scale(1) !important;
                -moz-transform-origin: 0 0 !important;
            }
        </style>
          
                <link href="CSS/CommonStyle.css?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
            Telerik.Web.UI.RadScheduler.prototype.saveClientState = function () {
                return '{"scrollTop":' + Math.round(this._scrollTop) + ',"scrollLeft":' + Math.round(this._scrollLeft) + ',"isDirty":' + this._isDirty + '}';
            }
        </script>
    </head>
    <body  onload="LoadAppointment();"> 
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">

            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" DestroyOnClose="true">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>

        <script language="javascript" type="text/javascript">
            var AppointmentMenu = "<%=schAppointmentScheduler.ClientID %>";
        </script>
        <form id="frmAppointments" >
            <div style="width: 100%; height: 100%;">
                <table style="width: 100%;">
                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
                    </telerik:RadScriptManager>
                    <tr style="height: 36%;">
                        <td style="width: 25%;">

                            <div class="panel panelborderbox" id="pnlCalender">
                                <div class="panel-headApprecolor">Calendar</div>
                                <div class="panel-body">
                                    <telerik:RadCalendar ID="Calendar1" runat="server" EnableMonthYearFastNavigation="true"
                                        Width="100%" ShowRowHeaders="false" AutoPostBack="false" OnSelectionChanged="Calendar1_SelectionChanged"
                                        FastNavigationStep="12">
                                        <%--<todaydaystyle backcolor="Beige" />--%>
                                        <%--<SelectedDayStyle BackColor="#0033CC" />--%>
                                        <%-- <selectorstyle backcolor="#0033CC" />--%>
                                        <ClientEvents OnDateSelected="calenderclick" />
                                        <SpecialDays>
                                            <telerik:RadCalendarDay Repeatable="Today">
                                                <ItemStyle CssClass="rcToday" />
                                            </telerik:RadCalendarDay>
                                        </SpecialDays>
                                    </telerik:RadCalendar>
                                </div>
                                <div class="panel-footer">
                                    <asp:UpdatePanel ID="updtPnlCalender" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh My Scheduler" OnClick="btnRefresh_Click"
                                                OnClientClick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" CssClass="aspresizedbluebutton" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Button ID="btnShowToday" runat="server" Text="Today" OnClick="btnShowToday_Click" OnClientClick="return btnToday_Clicked();"  CssClass="aspresizedbluebutton"/>
                                    <asp:Button ID="btnMoveToNextProcess" runat="server" Style="display: none !important"
                                        OnClick="btnMoveToNextProcess_Click" OnClientClick="showTime();" Text="Button" CssClass="aspresizedbluebutton" />

                                </div>
                            </div>
                            <asp:HiddenField ID="hdnSelectedDate" runat="server" EnableViewState="false" />

                        </td>
                        <td rowspan="7" valign="top" class="zindtex" style="width: 74.9%; padding-top: 0.95%;">
                            <asp:UpdatePanel ID="updpnlScheduler" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="chklstProviders" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="chkShowActive" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoActiveProviders" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoAllActiveProviders" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoInActiveProviders" EventName="CheckedChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="panel panelborderbox" id="pnlScheduler" >
                                        <div class="panel-headApprecolor">Scheduler</div>
                                        <div class="panel-body">
                                            <telerik:RadScheduler ID="schAppointmentScheduler" runat="server" AppointmentStyleMode="Default"
                                                OverflowBehavior="Auto" DataEndField="EndDate" DataKeyField="ID" DataStartField="StartDate"
                                                DataSubjectField="Subject" Height="760px" Width="100.5%" AllowDelete="False" AllowEdit="False"
                                                EnableAdvancedForm="False" NumberOfHoveredRows="1" OnClientTimeSlotContextMenuItemClicked="schAppointmentScheduler_TimeSlotContextMenuItemClicked"
                                                OnClientAppointmentContextMenuItemClicked="schAppointmentScheduler_AppointmentContextMenuItemClicked"
                                                ShowAllDayRow="False" OnClientAppointmentContextMenu="schAppointmentScheduler_AppointmentContextMenu"
                                                Culture="(Default)" OnClientAppointmentClick="schAppointmentScheduler_AppointmentClick"
                                                ShowNavigationPane="False" OnClientAppointmentInserting="schAppointmentScheduler_TimeSlotClick"
                                                CssClass="rsAptPai" OnTimeSlotCreated="schAppointmentScheduler_TimeSlotCreated"
                                                OnNavigationCommand="schAppointmentScheduler_NavigationCommand" OnClientNavigationCommand="schAppoinmentScheduler_NavigationCommand" OnClientAppointmentDoubleClick="schAppoinmentScheduler_DoubleClick"
                                                OnClientTimeSlotContextMenu="schAppointmentScheduler_TimeSlotContextMenu"
                                                Skin="Default" TimeLabelRowSpan="1" EnableExactTimeRendering="true">
                                                <TimelineView UserSelectable="False" />
                                                <TimeSlotContextMenus>
                                                    <telerik:RadSchedulerContextMenu ID="RadSchedulerContextMenu0" runat="server">
                                                        <Items>
                                                            <telerik:RadMenuItem runat="server" Text="New Appointment">
                                                            </telerik:RadMenuItem>
                                                            <%-- <telerik:RadMenuItem runat="server" Text="Willing Patients List">
                                                        </telerik:RadMenuItem>--%>
                                                        </Items>
                                                    </telerik:RadSchedulerContextMenu>
                                                </TimeSlotContextMenus>
                                                <ExportSettings>
                                                    <Pdf PageBottomMargin="1in" PageLeftMargin="1in" PageRightMargin="1in" PageTopMargin="1in" />
                                                </ExportSettings>
                                                <AppointmentContextMenus>
                                                    <telerik:RadSchedulerContextMenu ID="AppointmentMenu" runat="server">
                                                        <Items>
                                                            <telerik:RadMenuItem runat="server" Text="Check In">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="New Appointment">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="Edit Appointment">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="Cancel Appointment">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="No Show">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="View Appointment">
                                                            </telerik:RadMenuItem>
                                                            <%--<telerik:RadMenuItem runat="server" Text="Collect/Edit Payment">
                                                            </telerik:RadMenuItem>--%>
                                                            <telerik:RadMenuItem runat="server" Text="Authorization & EV"><%--Text="Eligibility Verification"--%>
                                                            </telerik:RadMenuItem>
                                                           <%-- <telerik:RadMenuItem runat="server" Text="Manage Authorization">
                                                            </telerik:RadMenuItem>--%>
                                                            <telerik:RadMenuItem runat="server" Text="Show Patient Summary">
                                                            </telerik:RadMenuItem>
                                                            <%-- <telerik:RadMenuItem runat="server" Text="Check In-NON EHR">
                                                        </telerik:RadMenuItem>--%>
                                                            <telerik:RadMenuItem runat="server" Text="Walked Away">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="Print Receipt">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem runat="server" Text="Undo">
                                                            </telerik:RadMenuItem>
                                                            <%--<telerik:RadMenuItem runat="server" Text="Electronic Super Bill">
                                                        </telerik:RadMenuItem>--%>
                                                            <%-- <telerik:RadMenuItem runat="server" Text="Print Super Bill">
                                                        </telerik:RadMenuItem>--%>
                                                            <%--<telerik:RadMenuItem runat="server" Text="Attach Super Bill">--%>
                                                            <%--</telerik:RadMenuItem><telerik:RadMenuItem 
                                                    runat="server" Text="Auth Request"></telerik:RadMenuItem><telerik:RadMenuItem 
                                                    runat="server" Text="Update Auth Response">--%><%--</telerik:RadMenuItem>--%>
                                                            <%--<telerik:RadMenuItem runat="server" Text="Create New Auth">
                                                        </telerik:RadMenuItem>--%>
                                                            <%--  <telerik:RadMenuItem runat="server" Text="Willing Patients List">
                                                        </telerik:RadMenuItem>--%>
                                                            <telerik:RadMenuItem runat="server" Text="Print Quality Measure Data Sheet">
                                                            </telerik:RadMenuItem>
                                                              <telerik:RadMenuItem runat="server" Text="Generate Enc. Doc.">
                                                            </telerik:RadMenuItem>
                                                               <telerik:RadMenuItem runat="server" Text="Open Patient Chart">
                                                            </telerik:RadMenuItem>
                                                        </Items>
                                                    </telerik:RadSchedulerContextMenu>
                                                </AppointmentContextMenus>
                                                <AdvancedForm Enabled="false" />
                                                <ResourceTypes>
                                                    <telerik:ResourceType KeyField="Appointment_Provider_ID" Name="Physician" />
                                                </ResourceTypes>
                                            </telerik:RadScheduler>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="height: 10%;">
                        <td style="width: 25%;">
                            <div class="panel panelborderbox" id="pnlFacility">
                                <div class="container-fluid panel-headApprecolor" style="padding-left: 0px!important;" id="pnlFacilityHeader" runat="server">Facility</div>
                                   <div class="panel-body ">
                                        <asp:DropDownList ID="cboFacilityName" runat="server" OnSelectedIndexChanged="cboFacilityName_SelectedIndexChanged"
                                            PostBackUrl="~/frmBlockDays.aspx" AutoPostBack="True" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" Style="width: 100%; white-space: pre-line;" CssClass="spanstyle">
                                        </asp:DropDownList>
                                    </div>
                                <%--<div class="panel-footer" id="divShowAllPhysicians" runat="server">
                                    <asp:CheckBox ID="chkShowAllPhysicians" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowAllPhysicians_CheckedChanged"
                                        onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" Text="Show All Providers"  CssClass="spanstyle"/>
                                </div>--%>
                                <div class="panel-footer" style="display: grid;" id="divShowAllPhysicians" runat="server">
                                    <div>
                                        <span>Show Providers</span>
                                        <asp:RadioButton ID="rdoActivePhysicians" runat="server" AutoPostBack="true" Text="Active" GroupName="Physicians" OnCheckedChanged="chkShowAllPhysicians_CheckedChanged" style="margin-left: 5px;"/>
                                        <asp:RadioButton ID="rdoAllActivePhysicians" runat="server" AutoPostBack="true" Text="All Active" GroupName="Physicians" OnCheckedChanged="chkShowAllPhysicians_CheckedChanged" style="margin-left: 3px;"/>
                                        <asp:RadioButton ID="rdoInActivePhysicians" runat="server" AutoPostBack="true" Text="InActive" GroupName="Physicians" OnCheckedChanged="chkShowAllPhysicians_CheckedChanged" style="margin-left: 3px;"/>
                                    </div>
                                </div>
                            </div>
                               
                        </td>
                    </tr>
                    <tr style="height: 43%;">
                        <td style="width: 25%;">
                            <asp:UpdatePanel ID="updpnlProvidersList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="chkShowActive" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoActiveProviders" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoAllActiveProviders" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rdoInActiveProviders" EventName="CheckedChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="panel panelborderbox" id="pnlProviders">
                                        <div class="panel-headApprecolor fontcolor" id="pnlProvidersHeader" runat="server">Providers</div>
                                        <div class="panel-body ">

                                            <div style="height: 262px; overflow-y: auto;" >

                                                <asp:CheckBoxList ID="chklstProviders" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="chklstProviders_SelectedIndexChanged" RepeatLayout="Flow" 
                                                    onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" CssClass="LabelStyleBold">
                                                </asp:CheckBoxList>

                                            </div>
                                        </div>
                                        <div class="panel-footer" id="divCheckBoxShowAllProviders" runat="server">
                                            <asp:CheckBox ID="chkShowActive" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowActive_CheckedChanged" Text="Show All Providers" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }" />
                                        </div>
                                        <div class="panel-footer" style="display: grid;" id="divShowAllProviders" runat="server">
                                            <div>
                                                <span>Show Providers</span>
                                                <asp:RadioButton ID="rdoActiveProviders" runat="server" AutoPostBack="true" Text="Active" GroupName="Providers" OnCheckedChanged="chkShowActive_CheckedChanged" Checked="true" style="margin-left: 5px;"/>
                                                <asp:RadioButton ID="rdoAllActiveProviders" runat="server" AutoPostBack="true" Text="All Active" GroupName="Providers" OnCheckedChanged="chkShowActive_CheckedChanged" style="margin-left: 3px;"/>
                                                <asp:RadioButton ID="rdoInActiveProviders" runat="server" AutoPostBack="true" Text="InActive" GroupName="Providers" OnCheckedChanged="chkShowActive_CheckedChanged" style="margin-left: 3px;"/>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="height: 1%;">
                        <td style="width: 25%;">
                            <asp:Button ID="btnPhysiciancalenderFacility" runat="server" Text="Physician Calendar All Facility"
                                Width="100%" OnClick="btnPhysiciancalenderFacility_Click" CssClass="aspresizedbluebutton" />
                        </td>
                    </tr>
                    <tr style="height: 3%;">
                        <td style="width: 25%;">
                            <asp:Button ID="btnFindAppointments" runat="server" OnClientClick="return OpenFindAllAppointments();"
                                Text="Find Appointments" Width="100%" EnableViewState="false"  CssClass="aspresizedbluebutton"/>
                        </td>
                    </tr>
                   
                    <tr style="height: 3%;">
                        <td style="width: 25%;">
                            <asp:Button ID="btnBlockDays" runat="server" Text="Block Days" Width="100%" OnClick="btnBlockDays_Click" 
                                 CssClass="aspresizedbluebutton"/>
                        </td>
                    </tr>
                
                     <tr style="height: 3%;">
                        <td style="width: 25%;">
                            <asp:Button ID="btnBulkEncTemplate" runat="server" Text="Download Bulk Encounter Templates" Width="100%" Visible="false"
                                OnClick="btnBulkEncTemplate_Click" CssClass="aspresizedbluebutton"/>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr style="width: 100%">
                        <td width="90%"></td>
                        <td>
                            <asp:Button ID="btnClose" Text="Close" CssClass="aspresizedredbutton" OnClientClick="return CloseAppointmentModal();" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
            <asp:Label ID="lblLoad" runat="server" Style="display: none" EnableViewState="false"></asp:Label>
            <br />
            <asp:Label ID="lblCalenderClick" runat="server" Style="display: none" EnableViewState="false"></asp:Label>
            <br />
            <asp:Label ID="lblFillAppointments" runat="server" Style="display: none" EnableViewState="false"></asp:Label>
            <asp:HiddenField ID="hdnIsSSOLogin" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSelectedMenu" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnMyEncounterStatus" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFindApptHumanID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEditApptPhyID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnApptPhyId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnApptFacName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhyIDColor" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhysisicanChecked" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSourceScreen" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSchedulerView" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLaptopView" runat="server" EnableViewState="false" />
            <asp:Button ID="btnPrintSuperBill" runat="server" Style="display: none" OnClick="btnPrintSuperBill_Click"
                Text="PrintSuperBill" />
            <asp:Button ID="btnPrintRecipt" runat="server" Style="display: none" OnClick="btnPrintRecipt_Click"
                Text="PrintRecipt" />
            <asp:HiddenField ID="hdnFileName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSelectedSlotDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnRole" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnTitle" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnStopLoading" runat="server" EnableViewState="false" Value="" />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
                <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                <script src="JScripts/JSAppoinntments.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            </asp:PlaceHolder>
        </form>
    </body>
</asp:Content>
<%--</html>--%>