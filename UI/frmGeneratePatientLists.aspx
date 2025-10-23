<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmGeneratePatientLists.aspx.cs"
    Inherits="Acurus.Capella.UI.frmGeneratePatientLists" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Generate Patient List</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #form1
        {
            width: 1143px;
            height: 862px;
        }
        .style7
        {
            height: 27px;
        }
        .style9
        {
            height: 191px;
            width: 438px;
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
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .RadButton
        {
            cursor: pointer;
        }
        .rbSkinnedButton
        {
            vertical-align: top;
        }
        .rbSkinnedButton
        {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }
        .RadButton
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
            top: 1px;
            left: 0px;
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }
        .rbDecorated
        {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }
        .rbDecorated
        {
            font-size: 12px;
            font-family: "Segoe UI" ,Arial,Helvetica,sans-serif;
        }
        .style29
        {
            width: 1036px;
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
        .RadComboBox_Default .rcbInputCellLeft
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.ComboBox.rcbSprite.png' );
        }
        .RadComboBox .rcbInputCellLeft
        {
            background-color: transparent;
            background-repeat: no-repeat;
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
        .RadComboBox_Default .rcbArrowCellRight
        {
            background-image: url(  'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.ComboBox.rcbSprite.png' );
        }
        .RadComboBox .rcbArrowCellRight
        {
            background-color: transparent;
            background-repeat: no-repeat;
        }
        .style30
        {
            width: 570px;
        }
        .style31
        {
            width: 370px;
        }
        .style32
        {
            width: 177px;
        }
        .style33
        {
            width: 588px;
        }
        .style36
        {
            height: 23px;
        }
        .style38
        {
            height: 23px;
            width: 407px;
        }
        .style40
        {
            height: 23px;
            width: 167px;
        }
        .style41
        {
            width: 167px;
        }
        .style42
        {
            width: 256px;
        }
        .style43
        {
            height: 23px;
            width: 256px;
        }
        .style44
        {
            width: 294px;
        }
        .style45
        {
            height: 23px;
            width: 294px;
        }
        .style46
        {
            width: 407px;
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
        .display
        {
            display:none; 
        }  
        .style20
        {
            height: 116px;
        }
        .style22
        {
            height: 49px;
            width: 246px;
        }
        .style21
        {
            height: 49px;
        }
        .style25
        {
            height: 49px;
            width: 29px;
        }
        .style35
        {
            width: 246px;
        }
        .style24
        {
            width: 191px;
        }
        .style26
        {
            width: 29px;
        }
        .style48
        {
            width: 78px;
        }
        .style47
        {
            width: 151px;
        }
        .style49
        {
            width: 180px;
        }
        .style8
        {
            height: 40px;
        }
        .style50
        {
            width: 83px;
        }
         .style51
        {
            height: 35px;
        }
         .style15
        {
            width: 110px;
            height: 29px;
        }
        .style16
        {
            width: 110px;
        }
        .style17
        {
            height: 29px;
        }
         </style>

     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
   </head>
<body class="bodybackground" onload ="{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" >
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
        <windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Patient List" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
        </windows>
    </telerik:RadWindowManager>
     <telerik:RadWindowManager ID="WindowMngr1" runat="server">
        <windows>
            <telerik:RadWindow ID="MessageWindow1" runat="server" Behaviors="Close" Title="Patient List" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
        </windows>
    </telerik:RadWindowManager>
   
   <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Width="96%" >
     
        <div style="height: 100%; width: 100%">
            <asp:Panel ID="pnlMain" runat="server" Height="100%" Width="100%">
                <table style="height: 100%; width: 100%;" >
                    <tr>
                        <td class="style9" colspan="1">
                            <asp:Panel ID="pnlDemoProbMed" runat="server" Height="100%" Width="1130px" DefaultButton="btnGenerateReport">
                                <table style="width: 100%; height: 100%;">
                                    <tr>
                                        <td class="style30" >
                                            <asp:Panel ID="pnlDemographics" runat="server" Font-Size="Small" CssClass="Editabletxtbox "
                                                GroupingText="Demographics"  Width="100%" DefaultButton="btnGenerateReport">
                                                <table bgcolor="White" style="width: 100%; height: 120%";>
                                                    <tr>
                                                        <td class="style20">
                                                            <asp:Panel ID="pnlAge" runat="server" GroupingText="Age" Height="100%" CssClass="Editabletxtbox"
                                                                Width="100%" >
                                                                <table bgcolor="White" style="width: 100%; height: 109%;">
                                                                    <tr>
                                                                        <td class="style22">
                                                                            <asp:CheckBox ID="chkAge" runat="server" Font-Size="Small" Text="Age" />
                                                                        </td>
                                                                        <td class="style21" colspan="2">
                                                                            <telerik:RadComboBox ID="cboAge" runat="server" Enabled="False" Width="220px">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td class="style25">
                                                                        </td>
                                                                        <td class="style21">
                                                                            <telerik:RadTextBox ID="txtAgeLevel" runat="server" Enabled="False" 
                                                                                Width="100px">
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style35">
                                                                            <asp:CheckBox ID="chkAgeRange" runat="server" Font-Size="Small" 
                                                                                Text="Age Range" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblFrom" runat="server" Font-Size="Small" Text="From"></asp:Label>
                                                                        </td>
                                                                        <td class="style24">
                                                                            <telerik:RadTextBox ID="txtAgeRangeFrom" runat="server" Width="100%">
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                        <td class="style26">
                                                                            <asp:Label ID="lblTo" runat="server" Font-Size="Small" Text="To"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="txtAgeRangeTo" runat="server" Enabled="False" 
                                                                                Width="100px">
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlGender" runat="server" Height="100%">
                                                                <table style="width:100%; height: 100%;">
                                                                    <tr>
                                                                        <td class="style48">
                                                                            <asp:CheckBox ID="chkGender" runat="server" Font-Size="Small" Text="Sex" />
                                                                        </td>
                                                                        <td class="style47">
                                                                            <telerik:RadComboBox ID="cboGender" runat="server" Enabled="False" 
                                                                                Width="140px">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td class="style49">
                                                                            <asp:Label ID="lblRace" runat="server" Font-Size="Small" Text="Race"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="cboRace" runat="server" Width="140px">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style48">
                                                                            <asp:Label ID="lblEthnicity" runat="server" Font-Size="Small" Text="Ethnicity"></asp:Label>
                                                                        </td>
                                                                        <td class="style47">
                                                                            <telerik:RadComboBox ID="cboEthnicity" runat="server" Width="140px">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td class="style49">
                                                                            <asp:Label ID="lblCommPreference" runat="server" Font-Size="Small" 
                                                                                Text="Communication Preference"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="cboCommPreference" runat="server" Width="140px">
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
                                        <td>
                                            <asp:Panel ID="pnlProblemMedication" runat="server" Height="100%" Width="100%">
                                                <table style="width: 100%; height: 100%">
                                                    <tr>
                                                        <td class="style51">
                                                            <asp:Panel ID="pnlEncounterDate" runat="server" Font-Size="Small" 
                                                                GroupingText="Encounter date" Height="100%" Width="100%" CssClass="LabelStyleBold">
                                                                <table style="width:100%; height: 74%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="Editabletxtbox"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDateTimePicker ID="rdtpFromDate" Runat="server" Width="180px" CssClass="Editabletxtbox"
                                                                                Culture="English (India)">
                                                                                <TimePopupButton HoverImageUrl="" ImageUrl="" />
                                                                                <TimeView CellSpacing="-1" >
                                                                                </TimeView>
                                                                                <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" 
                                                                                    UseRowHeadersAsSelectors="False">
                                                                                </Calendar>
                                                                                <DateInput DateFormat="dd-MMM-yyyy hh:mm:ss tt"  ReadOnly="true"
                                                                                    DisplayDateFormat="dd-MMM-yyyy hh:mm tt" LabelWidth="40%">
                                                                                    <EmptyMessageStyle Resize="None" />
                                                                                    <ReadOnlyStyle Resize="None" />
                                                                                    <FocusedStyle Resize="None" />
                                                                                    <DisabledStyle Resize="None" />
                                                                                    <InvalidStyle Resize="None" />
                                                                                    <HoveredStyle Resize="None" />
                                                                                    <EnabledStyle Resize="None" />
                                                                                </DateInput>
                                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                            </telerik:RadDateTimePicker>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblToDate" runat="server" Text="To Date" CssClass="Editabletxtbox"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDateTimePicker ID="rdtpToDate" Runat="server" 
                                                                                Width="170px" Culture="English (India)" >                                                                                
                                                                                <TimePopupButton HoverImageUrl="" ImageUrl="" />
                                                                                <TimeView CellSpacing="-1" >
                                                                                </TimeView>
                                                                                <Calendar EnableWeekends="True" UseColumnHeadersAsSelectors="False" 
                                                                                    UseRowHeadersAsSelectors="False">
                                                                                </Calendar>
                                                                                <DateInput DateFormat="dd-MMM-yyyy hh:mm:ss tt" DisplayDateFormat="dd-MMM-yyyy hh:mm tt"
                                                                                    LabelWidth="40%" ReadOnly="true">
                                                                                    <EmptyMessageStyle Resize="None" />
                                                                                    <ReadOnlyStyle Resize="None" />
                                                                                    <FocusedStyle Resize="None" />
                                                                                    <DisabledStyle Resize="None" />
                                                                                    <InvalidStyle Resize="None" />
                                                                                    <HoveredStyle Resize="None" />
                                                                                    <EnabledStyle Resize="None" />
                                                                                </DateInput>
                                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                            </telerik:RadDateTimePicker>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style8">
                                                            <asp:Panel ID="pnlProblemList" runat="server" Font-Size="Small" CssClass="LabelStyleBold"
                                                                GroupingText="Problem List" Height="100%" Width="100%">
                                                                <table bgcolor="White" style="width: 100%; height: 100%;">
                                                                    <tr>
                                                                        <td class="style15">
                                                                            <asp:Label ID="lblProblemList" runat="server" Font-Size="Small" CssClass="Editabletxtbox"
                                                                                Text="Problem List"></asp:Label>
                                                                        </td>
                                                                        <td class="style16" >
                                                                            <telerik:RadTextBox ID="txtProblemList" runat="server" Height="100%" 
                                                                                TextMode="MultiLine" Width="380px" BackColor="#BFDBFF" BorderColor="Black" CssClass="nonEditabletxtbox"
                                                                                BorderWidth="1px" ReadOnly="True">
                                                                                <disabledstyle resize="None" />
                                                                                <invalidstyle resize="None" />
                                                                                <hoveredstyle resize="None" />
                                                                                <readonlystyle resize="None" />
                                                                                <emptymessagestyle resize="None" />
                                                                                <focusedstyle resize="None" />
                                                                                <enabledstyle resize="None" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                        <td class="style17">
                                                                            <asp:ImageButton ID="pbFindProblemList" runat="server" 
                                                                                ImageUrl="~/Resources/Database Inactive.jpg" 
                                                                                OnClientClick="pbFindProblemList_Click()" ToolTip="ProblemList" />
                                                                        </td>
                                                                        <td class="style17">
                                                                            <asp:ImageButton ID="pbClearproblemlist" runat="server" 
                                                                                ImageUrl="~/Resources/close_small_pressed.png" 
                                                                                OnClientClick="pbClearproblemlist_click()" ToolTip="Clear" />
                                                                        </td>
                                                                    </tr>                                                                    
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlMedication" runat="server" Font-Size="Small" CssClass="LabelStyleBold"
                                                                GroupingText="Medication List" Height="100%" Width="100%">
                                                                <table  style="width:100%; height: 100%;background-color:White" >
                                                                    <tr>
                                                                        <td class="style50">
                                                                            <asp:Label ID="lblMedication" runat="server" Font-Size="Small" CssClass="Editabletxtbox"
                                                                                Text="Medication"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="txtMedication" runat="server" Height="100%" TextMode="MultiLine"
                                                                                Width="370px" BorderColor="Black" BorderWidth="1px" BackColor="#BFDBFF" CssClass="nonEditabletxtbox"
                                                                                ReadOnly="True">
                                                                                <DisabledStyle Resize="None" />
                                                                                <InvalidStyle Resize="None" />
                                                                                <HoveredStyle Resize="None" />
                                                                                <ReadOnlyStyle Resize="None" />
                                                                                <EmptyMessageStyle Resize="None" />
                                                                                <FocusedStyle Resize="None" />
                                                                                <EnabledStyle Resize="None" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="pbFindMedication" runat="server" 
                                                                                ImageUrl="~/Resources/Database Inactive.jpg"  ToolTip="MedicationManager" OnClientClick="pbFindMedication_Click()" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="pbClearmedication" runat="server" 
                                                                                ImageUrl="~/Resources/close_small_pressed.png" 
                                                                                OnClientClick="pbClearmedication_click()" ToolTip="Clear" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style50">
                                                                            <asp:Label ID="lblMedicationAllergy" runat="server" Font-Size="Small" CssClass="Editabletxtbox"
                                                                                Text="Medication Allergy"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="txtMedicationAllergy" runat="server" Height="100%" TextMode="MultiLine" CssClass="nonEditabletxtbox"
                                                                                Width="370px" BorderColor="Black" BackColor="#BFDBFF" ReadOnly="true">
                                                                                <DisabledStyle Resize="None" />
                                                                                <InvalidStyle Resize="None" />
                                                                                <HoveredStyle Resize="None" />
                                                                                <ReadOnlyStyle Resize="None" />
                                                                                <EmptyMessageStyle Resize="None" />
                                                                                <FocusedStyle Resize="None" />
                                                                                <EnabledStyle Resize="None" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="pbFindMedicationAllergy" runat="server" 
                                                                                ImageUrl="~/Resources/Database Inactive.jpg" 
                                                                                OnClientClick="pbFindMedicationAllergy_Click()"  ToolTip="MedicationManager"/>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="pbClearmedicationAllergy" runat="server" 
                                                                                ImageUrl="~/Resources/close_small_pressed.png" 
                                                                                OnClientClick="pbClearmedicationAllergy_click()" ToolTip="Clear" />
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
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                   
                    <tr><td style="height: 80px;">
                        <asp:Panel ID="pnlLabResults" runat="server" Height="100%" Width="100%" 
                            DefaultButton="btnGenerateReport">
                            <table style="width:100%; height: 100%;" bgcolor="White">
                                <tr>
                                    <td class="style46">
                                        <asp:Label ID="lblTestResultName1" runat="server" Font-Size="Small" CssClass="Editabletxtbox" 
                                            Text="Test/Result Name"></asp:Label>
                                    </td>
                                    <td class="style41">
                                        <asp:Label ID="lblCondition" runat="server" Font-Size="Small" Text="Condition" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style44" colspan="2">
                                        <asp:Label ID="lblValueRange" runat="server" Font-Size="Small" CssClass="Editabletxtbox"
                                            Text="Value/Value Range"></asp:Label>
                                    </td>
                                    <%--<td class="style42">
                                        <asp:Label ID="lblCombintion" runat="server" Text="Condition" Font-Size="Small"></asp:Label>
                                    </td>  --%>                                  
                                </tr>
                                <tr>
                                    <td class="style46">
                                       <telerik:RadTextBox ID="txtTestResultName1" Runat="server" Width="100%"  CssClass="Editabletxtbox" >
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style41">
                                        <telerik:RadComboBox ID="cboTestResultName1Range" Runat="server" 
                                            onselectedindexchanged="cboTestResultName1Range_SelectedIndexChanged" BackColor="White"    
                                            AutoPostBack="True">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style44">
                                        <telerik:RadTextBox ID="txtTestResultName1Units" Runat="server" Width="100%" >
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style42">
                                        <telerik:RadTextBox ID="txtbetween1" Runat="server" Width="100%" CssClass="nonEditabletxtbox"
                                            Enabled="False">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                   <%-- <td>
                                        <telerik:RadComboBox ID="cboTestResultName1Condition" Runat="server" 
                                            Width="100%" 
                                            onselectedindexchanged="cboTestResultName1Condition_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                        </telerik:RadComboBox>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td class="style46">                                        
                                         <telerik:RadTextBox ID="txtTestResultName2" Runat="server" Width="100%" Enabled="true"  CssClass="Editabletxtbox">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style41">
                                        <telerik:RadComboBox ID="cboTestResultName2Range" Runat="server" 
                                            onselectedindexchanged="cboTestResultName2Range_SelectedIndexChanged" BackColor="White" 
                                            AutoPostBack="True" Enabled="true">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style44">
                                        <telerik:RadTextBox ID="txtTestResultName2Units" Runat="server" Width="100%" 
                                            Enabled="true">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style42">
                                        <telerik:RadTextBox ID="txtbetween2" Runat="server" Width="100%" CssClass="nonEditabletxtbox"
                                            Enabled="False">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <%--<td>
                                        <telerik:RadComboBox ID="cboTestResultName2Condition" Runat="server" 
                                            Width="100%" 
                                            onselectedindexchanged="cboTestResultName2Condition_SelectedIndexChanged" 
                                            AutoPostBack="True" Enabled="False">
                                        </telerik:RadComboBox>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <td class="style38">
                                       
                                            <telerik:RadTextBox ID="txtTestResultName3" Runat="server" Width="100%" Enabled="true"  CssClass="Editabletxtbox">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        </td>
                                    <td class="style40">
                                        <telerik:RadComboBox ID="cboTestResultName3Range" Runat="server" 
                                            onselectedindexchanged="cboTestResultName3Range_SelectedIndexChanged"  BackColor="White" 
                                            AutoPostBack="True" Enabled="true">
                                        </telerik:RadComboBox>
                                        </td>
                                    <td class="style45">
                                        <telerik:RadTextBox ID="txtTestResultName3Units" Runat="server" Width="100%" 
                                            Enabled="true">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        </td>
                                    <td class="style43">
                                        <telerik:RadTextBox ID="txtbetween3" Runat="server" Width="100%" CssClass="nonEditabletxtbox"
                                            Enabled="False">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                        </td>
                                 <%--   <td class="style36">
                                        <telerik:RadComboBox ID="cboTestResultName3Condition" Runat="server" 
                                            Width="100%" 
                                            onselectedindexchanged="cboTestResultName3Condition_SelectedIndexChanged" 
                                            AutoPostBack="True" Enabled="False">
                                        </telerik:RadComboBox>
                                        </td>--%>
                                </tr>
                                <tr>
                                    <td class="style46">
                                        
                                        
                                            <telerik:RadTextBox ID="txtTestResultName4" Runat="server" Width="100%" Enabled="true" CssClass="Editabletxtbox">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style41">
                                        <telerik:RadComboBox ID="cboTestResultName4Range" Runat="server" 
                                            onselectedindexchanged="cboTestResultName4Range_SelectedIndexChanged"  BackColor="White" 
                                            AutoPostBack="True" Enabled="true">
                                        </telerik:RadComboBox>
                                    </td>
                                    <td class="style44">
                                        <telerik:RadTextBox ID="txtTestResultName4Units" Runat="server" Width="100%" 
                                            Enabled="true">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style42">
                                        <telerik:RadTextBox ID="txtbetween4" Runat="server" Width="100%" CssClass="nonEditabletxtbox"
                                            Enabled="false">
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                        </td></tr>
                    <tr>
                        <td class="style7">
                            <asp:Panel ID="pnlButton" runat="server" Height="100%" Width="100%">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="style29">
                                            &nbsp;
                                        </td>
                                        <td class="style31" align="right">
                                            <telerik:RadButton ID="btnGenerateReport" runat="server" 
                                                OnClick="btnGenerateReport_Click"  style="height: 34px !important;"
                                                onclientclicked="btnGenerateReport_ClientClicked" Text="Generate Report" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle"
                                                Width="150px">
                                            </telerik:RadButton>
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="btnClearAll" runat="server" OnClick="btnClearAll_Click" style="height: 34px !important;"
                                                onclientclicked="btnClearAll_ClientClicked" Text="Clear All" Width="82px" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td height="380">
                            <asp:Panel ID="pnlGrid" runat="server" Height="380px" Width="1120px" Font-Size="Small" CssClass="Editabletxtbox"
                                GroupingText="Patient List Report" ScrollBars="Auto" >     
                                  <telerik:RadGrid ID="grdConditionReport" runat="server" AutoGenerateColumns="False"
                                     CellSpacing="0" GridLines="Horizontal" Height="380px" Width="100%"  BorderWidth="1px"
                                      AllowSorting="true" onsortcommand="grdConditionReport_SortCommand" CssClass="Gridbodystyle"
                                      AutoPostBack="True" >
                                      <HeaderStyle CssClass="Gridheaderstyle " />
                                      <PagerStyle CssClass="Editabletxtbox" />
                                      <ItemStyle CssClass="Editabletxtbox" />
                                     <FilterMenu EnableImageSprites="False">
                                     </FilterMenu>
                                      <clientsettings enablepostbackonrowclick="True">
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                                      <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" ResizeGridOnColumnResize="true" />                                          
                                              
                                                    </clientsettings>
                                      <MasterTableView width="100%">
                                     <ItemStyle   wrap="True"  /> 
                                         <CommandItemSettings ExportToPdfText="Export to PDF" />
                                         <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                         </RowIndicatorColumn>
                                         <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                         </ExpandCollapseColumn>
                                         <Columns >                                                              
                                             <telerik:GridBoundColumn  DataField="PatientAccountNo" FilterControlAltText="Filter PatientAccountNo column" 
                                                 HeaderText="Patient<br />Account<br />No"   HeaderStyle-HorizontalAlign="Center" 
                                                  UniqueName="PatientAccountNo" AllowFiltering="False" Groupable="False" Resizable="false" >
                                                 <HeaderStyle Width="50px"  HorizontalAlign="Center" Wrap="true" CssClass="Gridheaderstyle"/>
                                                
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="PatientName" FilterControlAltText="Filter PatientName column"
                                                 HeaderText="PatientName" UniqueName="PatientName"  AllowFiltering="False" Groupable="False" Resizable="false" HeaderStyle-HorizontalAlign="Center">

                                                 <HeaderStyle Width="95px"  Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="DOB" FilterControlAltText="Filter DOB column"
                                                 HeaderText="DOB" UniqueName="DOB" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="48px" Wrap="true" CssClass="Gridheaderstyle" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="Age" FilterControlAltText="Filter Age column"
                                                 HeaderText="Age" UniqueName="Age" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="30px" Wrap="true" CssClass="Gridheaderstyle" />
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="Gender" FilterControlAltText="Filter Gender column"
                                                 HeaderText="Gender" UniqueName="Gender" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="55px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="Race" 

                                                 FilterControlAltText="Filter Race column" HeaderText="Race" UniqueName="Race" AllowFiltering="False" Groupable="False" Resizable="false">
                                                  <HeaderStyle Width="55px"  Wrap="true" CssClass="Gridheaderstyle"/>

                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="Ethnicity" 
                                                 FilterControlAltText="Filter Ethnicity column" HeaderText="Ethnicity" 
                                                 UniqueName="Ethnicity" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="55px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="CommunicationPreference" 
                                                 FilterControlAltText="Filter CommunicationPreference column" 
                                                 HeaderText="Communication<br />Preference" UniqueName="CommunicationPreference" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="100px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="EncounterDate" FilterControlAltText="Filte EncounterDate column"
                                                 HeaderText="Encounter <br/>Date" UniqueName="EncounterDate" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="70px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="Medication" FilterControlAltText="Filter Medication column"
                                                 HeaderText="Medication" UniqueName="Medication" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="70px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="MedicationDate" FilterControlAltText="Filter MedicationDate column"
                                                 HeaderText="Medication<br />Date" UniqueName="MedicationDate" Visible="true" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="70px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="MedicationAllergy" 
                                                 FilterControlAltText="Filter MedicationAllergy column" 
                                                 HeaderText="Medication<br />Allergy" UniqueName="MedicationAllergy" AllowFiltering="False" Groupable="False" Resizable="false">
                                                  <HeaderStyle Width="70px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                             <telerik:GridBoundColumn DataField="MedicationAllergyDate" FilterControlAltText="Filter MedicationAllergyDate column"
                                                 HeaderText="Medication<br />AllergyDate" UniqueName="MedicationAllergyDate" Visible="true" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="75px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>

                                             <telerik:GridBoundColumn DataField="ProblemList" FilterControlAltText="Filter ProblemList column" HeaderStyle-HorizontalAlign="Center"
                                                 HeaderText="ProblemList" UniqueName="ProblemList" AllowFiltering="False" Groupable="False" Resizable="False">
                                                 <HeaderStyle Width="110px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                               <telerik:GridBoundColumn DataField="ProblemListDate" FilterControlAltText="Filter ProblemListDate column"
                                                 HeaderText="ProblemList<br />Date" UniqueName="ProblemListDate" Visible="true" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="75px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                          
                                             <telerik:GridBoundColumn DataField="LabResult" FilterControlAltText="Filter LabResult column"
                                                 HeaderText="LabResult" UniqueName="LabResult" AllowFiltering="False" Groupable="False" Resizable="False">
                                                 <HeaderStyle Width="75px" Wrap="true" CssClass="Gridheaderstyle"/>
                                             </telerik:GridBoundColumn>
                                                 <telerik:GridBoundColumn DataField="LabResultDate" FilterControlAltText="Filter LabResultDate column"
                                                 HeaderText="LabResult<br />Date" UniqueName="LabResultDate" Visible="true" AllowFiltering="False" Groupable="False" Resizable="false">
                                                 <HeaderStyle Width="75px" Wrap="true" CssClass="Gridheaderstyle"/>
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
                    <td class="Editabletxtbox">
                     <PageNavigator:PageNavigator ID="mpnOrderManagement" runat="server"  OnFirst="FirstPageNavigator" />
                     </td>
                     
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlExport" runat="server" Height="100%" Width="100%">
                                <table style="width: 100%; height: 82%;">
                                    <tr>
                                        <td class="style33" valign="bottom">
                                            &nbsp;
                                            <asp:Label ID="lblResults" runat="server" ></asp:Label>
                                        </td>
                                        <td class="style32">
                                            &nbsp;
                                        </td>                                       
                                         <td valign="bottom" align="right">
                                            <telerik:RadButton ID="btnPrintPDF" runat="server" Text="Print" style="height: 33px !important;"
                                                 onclick="btnPrintPDF_Click" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle" >
                                            </telerik:RadButton>
                                        </td>
                                         <td valign="bottom">
                                            <telerik:RadButton ID="btnExportToExcel" runat="server" Text="Export To Excel" OnClick="btnExportToExcel_Click" ButtonType="LinkButton"  CssClass="bluebutton teleriknormalbuttonstyle" style="height: 34px !important;">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                   <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
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
        </telerik:RadScriptManager>
            </asp:Panel>
        </div>
         <asp:Button ID="InvisibleGenerateReport" runat="server" Text="Button" style="display:none" onclick="InvisibleGenerateReport_Click"/>
         <asp:HiddenField ID="iProblemList" runat="server" />
    <asp:HiddenField ID="searchedMedications" runat="server" />
      <asp:HiddenField ID="hdnMedAllergy" runat="server" />
    <asp:HiddenField ID="frequentProblemlist" runat="server" />
    <asp:HiddenField ID="probListText" runat="server" />
        <asp:HiddenField ID="isClearAll" runat="server" />
        <asp:HiddenField ID="hdnFilePath" runat="server" />
         <asp:HiddenField ID="hdnLocalTime" runat="server" />
           <asp:HiddenField ID="hdnSortOrder" runat="server" />
           
     <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
      <asp:Panel ID="Panel4" runat="server">
             <br />
             <br />
             <br />
             <br />
             <br />
             <img src="Resources/wait.ico" title="[Please wait while the page is saving...]"
                 alt="saving..." />
             <br />
         </asp:Panel>
     </div>
  
    </telerik:RadAjaxPanel>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="JScripts/JSGeneratePatientLists.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
   </asp:PlaceHolder>
    </form>
</body>
</html>
