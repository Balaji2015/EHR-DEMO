<%@ Page  Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmCreatePatientRemainder.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmCreatePatientRemainder" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Reminder Rule</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .displayNone
        {
            display: none;
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
        .rgDataDiv{
            height:155px!important;
        }
    </style>
<link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    </head>
<%--<body class="bodybackground" onload= "onloadcreatepatientremainder(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">--%>
    <body class="bodybackground" onload= " onloadcreatepatient();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="background-color: White; font-family: Microsoft Sans Serif;
    font-size: smaller; width: 900px; height: 669px">    
        <telerik:RadScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
            <Windows>
                <telerik:RadWindow ID="CreatePatientModalWindow" runat="server" OnClientClose="OnOpendPatientChartClick">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow1" runat="server" OnClientClose="OnOpendPatientChartClick">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%; height: 100%;">
                    <tr style="height: 60%">
                        <td width="100%">
                            <asp:Panel ID="pnlPatientReminderRule" runat="server" Width="100%" Height="100%"
                                GroupingText="Create Patient Reminder Rule" Font-Bold="True">
                                <table style="width: 100%; height: 98%;">
                                    <tr style="height: 70%">
                                        <td>
                                            <asp:Panel ID="pnlCreateRule" runat="server" Width="100%" Height="100%" GroupingText="Create Rule" CssClass="LabelStyleBold"
                                                Font-Bold="True">
                                                <table style="width: 100%; height: 100%;">
                                                    <tr style="height: 12%">
                                                        <td width="20%">
                                                            <asp:Label ID="lblRuleName" runat="server" Text="Rule Name*" Width="100%" Font-Bold="False" CssClass="MandLabelstyle"  mand="Yes"></asp:Label>
                                                                
                                                        </td>
                                                        <td width="32%">
                                                            <telerik:RadTextBox ID="txtRuleName" runat="server" AutoPostBack="false" Width="100%"
                                                                Font-Bold="False">
                                                                <DisabledStyle Resize="None" />
                                                                <InvalidStyle Resize="None" />
                                                                <HoveredStyle Resize="None" />
                                                                <ReadOnlyStyle Resize="None" />
                                                                <EmptyMessageStyle Resize="None" />
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                                <FocusedStyle Resize="None" />
                                                                <EnabledStyle Resize="None" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td width="15%" align="right">
                                                            <asp:Label ID="lblDescription" runat="server" Text="Description" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td width="33%" colspan="3">
                                                            <telerik:RadTextBox ID="txtDescription" runat="server" Width="100%" Font-Bold="False">
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 12%">
                                                        <td width="20%">
                                                            <asp:Label ID="lblProblem" runat="server" Text="Problem" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td width="60%" colspan="3">
                                                            <telerik:RadTextBox ID="txtProblem" runat="server" Width="100%" Height="98%" ReadOnlyStyle-BackColor="#BFDBFF"
                                                                TextMode="MultiLine" Font-Bold="False" ReadOnly="True"  ReadOnlyStyle-BorderColor="Black" CssClass="nonEditabletxtbox">
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td width="17%">
                                                            <telerik:RadButton ID="btnFindDx" runat="server" Text="Find Dx" AccessKey="D" Width="83%"
                                                                Font-Bold="False" AutoPostBack="false" OnClientClicked="DxClicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" Style="padding:0px !important;font-size: 14px !important;">
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td width="3%">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" Width="87%" ImageUrl="~/Resources/close_small_pressed.png"
                                                                OnClientClick="txtProblemClear();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 12%">
                                                        <td width="20%">
                                                            <asp:Label ID="lblMedication" runat="server" Text="Medication" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td width="60%" colspan="3">
                                                            <telerik:RadTextBox ID="txtMedication" runat="server" Width="100%" Height="98%" ReadOnlyStyle-BackColor="#BFDBFF"
                                                                TextMode="MultiLine" ReadOnly="True" Font-Bold="False" ReadOnlyStyle-BorderColor="Black"  CssClass="nonEditabletxtbox">
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td width="17%">
                                                            <telerik:RadButton ID="btnFindRx" runat="server" Text="Find Rx" Width="83%" AccessKey="R"
                                                                AutoPostBack="false" Font-Bold="False" OnClientClicked="RxClicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" Style="padding:0px !important;font-size: 14px !important;">
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td width="3%">
                                                            <asp:ImageButton ID="ImageButton2" runat="server" Width="87%" ImageUrl="~/Resources/close_small_pressed.png"
                                                                OnClientClick="txtMedicationClear();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 12%">
                                                        <td width="20%">
                                                            <asp:Label ID="lblMedicationAllergy" runat="server" Text="Medication Allergy" Width="100%" CssClass="spanstyle"
                                                                Font-Bold="False"></asp:Label>
                                                        </td>
                                                        <td width="60%" colspan="3">
                                                            <telerik:RadTextBox ID="txtMedicationAllergy" runat="server" ReadOnlyStyle-BackColor="#BFDBFF"
                                                                Width="100%" Height="98%" ReadOnly="True" TextMode="MultiLine" Font-Bold="False"
                                                                 ReadOnlyStyle-BorderColor="Black"  CssClass="nonEditabletxtbox">
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td width="17%">
                                                            <telerik:RadButton ID="btnRxAllergy" runat="server" Text="Find Rx" AccessKey="x"
                                                                Width="83%" AutoPostBack="false" Font-Bold="False" OnClientClicked="RAndXClicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" Style="padding:0px !important;font-size: 14px !important;">
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td width="3%">
                                                            <asp:ImageButton ID="ImageButton3" runat="server" Width="87%" ImageUrl="~/Resources/close_small_pressed.png"
                                                                OnClientClick="txtMedicationAllergyClear();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 12%">
                                                        <td width="18%">
                                                            <asp:Label ID="lblLabTestResult" runat="server" Text="LabTest Result" Width="100%" CssClass="spanstyle"
                                                                Font-Bold="False"></asp:Label>
                                                        </td>
                                                        <td width="60%" colspan="3">
                                                            <telerik:RadTextBox ID="txtLabTestResult" runat="server" Width="100%" ReadOnlyStyle-BackColor="#BFDBFF"
                                                                Height="98%" TextMode="MultiLine" ReadOnly="True" Font-Bold="False"  CssClass="nonEditabletxtbox"
                                                                ReadOnlyStyle-BorderColor="Black">
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td width="17%">
                                                            <telerik:RadButton ID="btnFindLabResult" runat="server" Text="Find Lab Result" AccessKey="L"
                                                                Width="83%" AutoPostBack="false" Font-Bold="False" OnClientClicked="LabResultClicked" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" Style="padding:0px !important;font-size: 14px !important;">
                                                            </telerik:RadButton >
                                                        </td>
                                                        <td width="3%">
                                                            <asp:ImageButton ID="ImageButton4" runat="server" Width="87%" ImageUrl="~/Resources/close_small_pressed.png"
                                                                OnClientClick="txtLabTestResultClear();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 16%">
                                                        <td width="100%" colspan="6">
                                                            <asp:Panel ID="Panel3" runat="server" Width="100%" Height="100%" CssClass="LabelStyleBold">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr style="height: 100%">
                                                                        <td width="7%">
                                                                            <asp:Label ID="lblGender" runat="server" Text="Gender" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                                        </td>
                                                                        <td width="10%" align="left">
                                                                            <telerik:RadComboBox ID="cboGender" runat="server" Width="100%" Font-Bold="False"
                                                                                OnClientSelectedIndexChanged="cboGender_SelectedIndexChanged">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td width="8%" align="right">
                                                                            <asp:Label ID="lblRace" runat="server" Text="Race" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                                        </td>
                                                                        <td width="12%">
                                                                            <telerik:RadComboBox ID="cboRace" runat="server" Width="100%" Font-Bold="False" OnClientSelectedIndexChanged="cboRace_SelectedIndexChanged">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td width="8%" align="right">
                                                                            <asp:Label ID="lblEthnicity" runat="server" Text="Ethnicity" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                                        </td>
                                                                        <td width="10%">
                                                                            <telerik:RadComboBox ID="cboEthnicity" runat="server" Width="100%" Font-Bold="False"
                                                                                OnClientSelectedIndexChanged="cboEthnicity_SelectedIndexChanged">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td width="8%" align="right">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="Status" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                                        </td>
                                                                        <td width="8%">
                                                                            <telerik:RadComboBox ID="cboStatus" runat="server" Width="100%" Font-Bold="False"
                                                                                OnClientSelectedIndexChanged="cboStatus_SelectedIndexChanged">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td width="15%" align="right">
                                                                            <asp:Label ID="lblCommunicat" runat="server" Text="Communication Preference" Width="100%" CssClass="spanstyle"
                                                                                Font-Bold="False"></asp:Label>
                                                                        </td>
                                                                        <td width="15%">
                                                                            <telerik:RadComboBox ID="cboCommunication" runat="server" Width="100%" Font-Bold="False"
                                                                                OnClientSelectedIndexChanged="cboCommunication_SelectedIndexChanged">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 12%">
                                                        <td width="100%" colspan="6">
                                                            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%" CssClass="LabelStyleBold">
                                                                <table style="width: 100%; height: 100%">
                                                                    <tr>
                                                                        <td width="50%">
                                                                            <asp:Panel ID="PnlValue" runat="server" Width="100%" Height="100%" GroupingText="Value" CssClass="LabelStyleBold">
                                                                                <table style="width: 100%; height: 100%">
                                                                                    <tr>
                                                                                        <td width="50%">
                                                                                            <telerik:RadComboBox ID="cboRange" runat="server" Width="100%" Font-Bold="False"
                                                                                                OnClientSelectedIndexChanged="cboRange_SelectedIndexChanged">
                                                                                            </telerik:RadComboBox>
                                                                                        </td>
                                                                                        <td width="50%">
                                                                                            <telerik:RadNumericTextBox ID="txtValueFrom" runat="server" MaxLength="4" Width="100%">
                                                                                                <NumberFormat ZeroPattern="n" DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True"
                                                                                                    KeepTrailingZerosOnFocus="True" />
                                                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                        <td width="33%">
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                        <td width="50%">
                                                                            <asp:Panel ID="Panel5" runat="server" Width="100%" Height="100%" GroupingText="Age" CssClass="LabelStyleBold">
                                                                                <table style="width: 100%; height: 100%">
                                                                                    <tr>
                                                                                        <td width="15%" align="right">
                                                                                            <asp:Label ID="lblAgeFrom" runat="server" Text="From" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                                                        </td>
                                                                                        <td width="23%">
                                                                                            <telerik:RadNumericTextBox ID="txtAgeFrom" runat="server" MaxLength="4" Width="100%">
                                                                                                <NumberFormat ZeroPattern="n" DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True"
                                                                                                    KeepTrailingZerosOnFocus="True" />
                                                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                        <td width="10%" align="right">
                                                                                            <asp:Label ID="lblAgeTo" runat="server" Text="To" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                                                        </td>
                                                                                        <td width="18%">
                                                                                            <telerik:RadNumericTextBox ID="txtAgeTo" runat="server" MaxLength="4" Width="100%">
                                                                                                <NumberFormat ZeroPattern="n" DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True"
                                                                                                    KeepTrailingZerosOnFocus="True" />
                                                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                        <td width="18%">
                                                                                            <telerik:RadComboBox ID="cboAgeRange" runat="server" Width="100%" Font-Bold="False"
                                                                                                OnClientSelectedIndexChanged="cboAgeRange_SelectedIndexChanged">
                                                                                            </telerik:RadComboBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 12%">
                                                        <td width="20%">
                                                            <asp:Label ID="lblExpectedAction" runat="server" Text="Expected Action" Width="100%"
                                                                Font-Bold="False"></asp:Label>
                                                        </td>
                                                        <td width="80%" colspan="5" style="padding: 19px">
                                                            <DLC:DLC ID="txtExpectedResult" runat="server" TextboxHeight="31px" TextboxWidth="650px" 
                                                                Value="CHIEF_COMPLAINTS" />
                                                            <%--<asp:PlaceHolder ID="phlCheifComplaints" runat="server" EnableViewState="false" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr style="height: 20%">
                                        <td>
                                            <asp:Panel ID="pnlCreateAlert" runat="server" Width="100%" Height="100%" GroupingText="Create Alert" CssClass="LabelStyleBold"
                                                Font-Bold="True">
                                                <table style="width: 100%; height: 98%;">
                                                    <tr>
                                                        <td width="20%">
                                                            <asp:Label ID="lblAlert" runat="server" Text="Alert" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td width="15%">
                                                            <telerik:RadNumericTextBox ID="txtAlertDay" runat="server" MaxLength="4">
                                                                <NumberFormat ZeroPattern="n" DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True"
                                                                    KeepTrailingZerosOnFocus="True" />
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td width="17%">
                                                            <asp:Label ID="lblDueBack" runat="server" Text="Day(s) Before Due " Width="100%" CssClass="spanstyle"
                                                                Font-Bold="False"></asp:Label>
                                                        </td>
                                                        <td width="13%">
                                                            <asp:Label ID="lblFrequencyInterval" runat="server" Text="Frequency Interval" Width="100%" CssClass="spanstyle"
                                                                Font-Bold="False"></asp:Label>
                                                        </td>
                                                        <td width="15%" align="right">
                                                            <asp:Label ID="lblEvery" runat="server" Text="Every" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td width="15%">
                                                            <telerik:RadNumericTextBox ID="txtFrequency" runat="server" MaxLength="4" Width="100%">
                                                                <NumberFormat ZeroPattern="n" DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True"
                                                                    KeepTrailingZerosOnFocus="True" />
                                                                <ClientEvents OnKeyPress="KeyPress" />
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td width="5%">
                                                            <asp:Label ID="lblEveryDays" runat="server" Text="Day(s)" Width="100%" Font-Bold="False" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr style="height: 10%">
                                        <td width="100%">
                                            <asp:Panel ID="pnlButton" runat="server" Width="100%" Height="100%" CssClass="LabelStyleBold">
                                                <table style="width: 100%; height: 98%;">
                                                    <tr>
                                                        <td width="80%">
                                                        </td>
                                                        <td width="10%">
                                                            <%--<telerik:RadButton ID="btnAdd" runat="server" Text="Add" Width="100%" OnClick="btnAdd_Click" Enabled="false">
                                                            </telerik:RadButton>--%>
                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" Width="100%" OnClick="btnAdd_Click"  CssClass="aspresizedgreenbutton"
                                                                Enabled="true" />
                                                        </td>
                                                        <td width="10%">
                                                            <asp:Button ID="btnClearAll" runat="server" Text="Clear All" Width="100%" OnClientClick="btnClearAll_Clicked();return false;" CssClass="aspresizedredbutton" />
                                                            <%-- <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All" Width="100%"
                                                                AutoPostBack="false" OnClientClicked="btnClearAll_Clicked">--%>
                                                            <%-- </telerik:RadButton>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 35%">
                        <td width="100%">
                            <asp:Panel ID="Panel2" runat="server" Width="100%" GroupingText="Patient Reminder" CssClass="LabelStyleBold"
                                Font-Bold="true" Height="100%">
                                <telerik:RadGrid ID="grdPatientRemainder" runat="server" AutoGenerateColumns="false"
                                    CellSpacing="0" Height="185px" GridLines="Both" Style="margin-bottom: 0px; overflow: auto;"
                                    Font-Bold="False" Width="100%"  OnItemCommand="grdPatientRemainder_ItemCommand" CssClass="Gridbodystyle">
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                    <HeaderStyle Font-Bold="true"  CssClass="Gridheaderstyle"/>
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="True" />
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                        <Resizing AllowColumnResize="false" />
                                        
                                    </ClientSettings>
                                    <SelectedItemStyle CssClass="highlight" />
                                    <MasterTableView>
                                        <Columns>
                                            <telerik:GridButtonColumn FilterControlAltText="Filter column column" UniqueName="Edit"
                                                ButtonType="ImageButton" ImageUrl="~/Resources/edit.gif" CommandName="EditRow"
                                                DataTextField="Edit" HeaderText="Edit" Resizable="false">
                                                <HeaderStyle Wrap="true" Width="5%" CssClass="Gridheaderstyle" />
                                                </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn FilterControlAltText="Filter column1 column" UniqueName="Del"
                                                ImageUrl="~/Resources/close_small_pressed.png" CommandName="DeleteRow" ButtonType="ImageButton"
                                                CommandArgument="Del" DataTextField="Del" HeaderText="Del" Resizable="false">
                                                <HeaderStyle Wrap="true" Width="5%" CssClass="Gridheaderstyle" />
                                               
                                                </telerik:GridButtonColumn>
                                            <telerik:GridBoundColumn DataField="RuleID" EmptyDataText="" FilterControlAltText="Filter RuleID column"
                                                HeaderText="RuleID" Display="false" UniqueName="RuleID" Resizable="false" >
                                                <HeaderStyle CssClass="Gridheaderstyle" />
                                                  <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="RuleName" EmptyDataText="" FilterControlAltText="Filter RuleName column"
                                                HeaderText="Rule Name" UniqueName="RuleName" Resizable="false">
                                                <HeaderStyle Wrap="true" Width="30%" />
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Description" EmptyDataText="" FilterControlAltText="Filter Description column"
                                                HeaderText="Description" UniqueName="Description" Resizable="false">
                                                <HeaderStyle Wrap="true" Width="30%" />
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                 
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="ExpectedAction" EmptyDataText="" FilterControlAltText="Filter ExpectedAction column"
                                                HeaderText="Expected Action" UniqueName="ExpectedAction" Resizable="false">
                                                <HeaderStyle Wrap="true" Width="29.9%" />
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter Alert column" DataField="Alert"
                                                EmptyDataText="" HeaderText="Alert" UniqueName="Alert" Display="False" Resizable="false">
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                 
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Freq.Interval" FilterControlAltText="Filter Freq.Interval column"
                                                HeaderText="Freq.Interval" EmptyDataText="" UniqueName="Freq.Interval" Display="False"
                                                Resizable="false">
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                               
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="PrimaryKey" FilterControlAltText="Filter PrimaryKey column"
                                                HeaderText="PrimaryKey" UniqueName="PrimaryKey" Display="False" Resizable="false">
                                                 <HeaderStyle CssClass="Gridheaderstyle" />
                                                  
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
                    <tr style="height: 5%; right: 20px">
                        <td width="100%" align="right">
                            <telerik:RadButton ID="btnClose" runat="server" AutoPostBack="true" Text="Close" Width="10%" OnClientClicked="btnClose_Clicked" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle"
                                 Style="margin-left:782px;margin-top: 6px;padding:0px !important;">
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <asp:Button ID="btnInvisible" runat="server" Text="Invisible" CssClass="displayNone"
                        OnClick="btnInvisible_Click" />
                    <asp:Button ID="btnInvisibleDx" runat="server" Text="Invisible" CssClass="displayNone"
                        OnClick="btnInvisibleDx_Click" />
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <input id="txtProblemTag" type="hidden" runat="server" />
        <input id="txtMedicationTag" type="hidden" runat="server" />
        <input id="txtMedicationAllergyTag" type="hidden" runat="server" />
        <input id="txtLabTestResultTag" type="hidden" runat="server" />
        <input id="typeButtton" type="hidden" runat="server" value="" />
        <asp:HiddenField ID="hdnMessageType" runat="server" />
        <asp:Button ID="btnMessageType" runat="server" Text="Button"  CssClass="displayNone"
        OnClientClick="btnClose_Clicked();" />
        <asp:CheckBox ID="chkActiveRule" runat="server" Visible="false" AutoPostBack="true"
            Font-Bold="false" Text="Show Active Rules" OnCheckedChanged="chkActiveRule_CheckedChanged" />
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel4" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label8" Text="" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                    alt="Loading..." />
                <br />
            </asp:Panel>
        </div>
   
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSCreatePatientRemainder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
