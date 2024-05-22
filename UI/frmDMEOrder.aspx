<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmDMEOrder.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmDMEOrder"  ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="UC" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="MKB" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .InvisibleButton, .hiddenVisible {
            visibility: hidden;
        }

        *, .modal {
            top: 0;
            left: 0;
        }

        .VerticalAlginTop {
            vertical-align: top;
        }

        .ui-widget-overlay {
            opacity: .5 !important;
        }

        .Scrollable {
            overflow: auto;
        }

        input[Tag="!!"] + label[for] {
            color: #00f;
            font-weight: bolder;
        }

        * {
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-size: 11.5px;
            font-family: Microsoft Sans Serif;
        }

        .ui-dialog .ui-dialog-title, .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        legend {
            font-weight: 700;
        }

        #gbOrderDetails fieldset {
            height: 100%;
        }

        #gbSelectICD fieldset{
             height: 100%;
        }

        #chklstFrequentlyUsedProcedures > tbody > tr > td > span {
            display: block;
            white-space: nowrap;
            padding: 0;
        }

            #chklstFrequentlyUsedProcedures > tbody > tr > td > span[IsHeader=true] {
                color: #00f;
                font-weight: 700;
            }

        .noWrapText {
            white-space: nowrap;
        }

        #txtOrderNotes_listDIAGNOSTIC ORDER CLINICAL INFORMATION {
            height: 75px;
            font-weight: 400;
            margin-left: 3px;
            position: static;
        }

        .style1, .style2, .style3, .style4, .style5 {
            height: 38px;
        }

        .MultiLineTextBox {
            resize: none;
        }

        .modal {
            position: fixed;
            background-color: #fff;
            z-index: 99;
            opacity: .8;
            filter: alpha(opacity=80);
            -moz-opacity: .8;
            min-height: 100%;
            width: 100%;
        }

        .underline {
            text-decoration: underline;
        }

        .ui-dialog-titlebar-close {
            display: none !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
        }

        .chkLabel input, .chkLabel1 input, .chkLabelleft input {
            float: left;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            width: 60px !important;
        }

        .ui-dialog .ui-dialog-titlebar {
            padding: 0 !important;
        }

        .ui-dialog .ui-dialog-title {
            font-size: 12px !important;
        }

        .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px !important;
            border: 2px solid !important;
            border-radius: 13px !important;
            top: 504px !important;
            left: 568px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0 !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
        }

        .ui-widget-content {
            border: 0 !important;
        }

        .ui-state-default, .ui-widget-header, ui-button {
            font-weight: 700 !important;
            font-size: 12px !important;
            font-family: sans-serif !important;
        }

        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7 !important;
        }

        .btn-primary, .btn-success {
            border: 1px solid transparent;
            color: #fff;
        }
        
        .btnbackcolor {
            background-color: #80ff9f;
        }

        .btn-primary {
            background-color: #337ab7;
            border-radius: 4px;
            padding: 4px 0;
        }
          .btn-primary:disabled {
            border: 1px solid transparent;
            background-color: #8bb8df;
        }
         .btn-primary:active {
            border: 1px solid transparent;
            background-color: #337ab7;
        }
        .btn-success {
            background-color: #5cb85c;
            border-radius: 4px;
            padding: 4px;
        }
        .btn-success:disabled {
            border: 1px solid transparent;
            background-color: #9ed69e;
        }
         .btn-success:active {
            border: 1px solid transparent;
            background-color: #398439;
        }
        .btn-danger {
            color: #fff;
            background-color: #d9534f;
            border: 1px solid transparent;
            border-radius: 4px;
            padding: 4px;
        }

        .verti {
            transform: rotate(315deg);
        }

        .chkProcedurelist {
            height: 283px !important;
            width: 700px;
        }

        .chkLabel label {
            display: block;
            margin-left: -1.5em;
        }

        .chkLabel1 label {
            display: block;
            margin-left: 1.3em;
        }

        .chkLabelleft label {
            display: block;
            margin-left: .3em;
        }

        #tag:hover {
            text-decoration: underline;
        }
        .Macra_field legend{
            color:#fe0003!important;
        }
        .rgDataDiv{
            height:130px!important;
        }
        #grdOrders{
            overflow: auto!important;
        }
        .chkLabel1 label {
            display: block;
            margin-left: 0.2em;
        }
    </style>
    <base target="_self" />
 
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    
   
    <link rel="stylesheet" href="CSS/datetimepicker.css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="FormLoad();warningmethod();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="DME Order"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>

            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" Title="" Modal="true"
                    VisibleOnPageLoad="false" IconUrl="Resources/16_16.ico" EnableViewState="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table>
                <tr>
                    <td>
                        <table style="margin-left: 2px;width:1185px;">
                            <tr style="width:100%;">
                                <td colspan="2" style="width:100%;">
                                    <asp:Panel runat="server" GroupingText="Center Details" ID="gbCenterDetail" CssClass="LabelStyleBold" Width="100%"
                                        BackColor="White">
                                        <table width="100%;">
                                            <tr>
                                                <td style="width: 80px;">
                                                    
                                                     <span id="lblLab" runat="server" style="width: 100%;" class="MandLabelstyle" mand="Yes">Vendor Name</span><span class="manredforstar">*</span>
                                                </td>
                                                <td style="width: 230px;">
                                                    
                                                    <select id="cboLab" runat="server" style="width: 100%; height: 20px;" class="Editabletxtbox"  onchange="EnableWaitCursorcbolab();" onserverchange="cboLab_SelectedIndexChanged">
                                                    </select>
                                                </td>
                                                <td style="width: 100px;">
                                                    <label id="lblCenterName" runat="server" class="Editabletxtbox" style="width: 100%;">Center Name</label>
                                                </td>
                                                <td style="width: 160px;">
                                                    
                                                    <input type="text" id="txtCenterName" class="Editabletxtbox" runat="server" style="width: 100%; height: 100%; border-color: black; border-width: 1px; color: black;" />
                                                </td>
                                                <td style="width: 80px;">
                                                    <label id="lblLocation" runat="server" class="Editabletxtbox" style="width: 100%;">Location</label>
                                                </td>
                                                <td style="width: 200px;">
                                                    <input type="text" id="txtLocation" runat="server" class="Editabletxtbox" style="width: 100%; height: 100%; border-color: black; border-width: 1px; color: black;" />
                                                </td>
                                                <td style="width: 100px;">
                                                   
                                                    <input type="button" id="btnSelectLocation" runat="server" value="Select Location" class="aspresizedbluebutton" 
                                                        onclick="btnSelectLocations();" onserverclick="btnSelectLocation_Click" 
                                                        accesskey="O" style="margin-right: 2px; width: 100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="width:100%;">
                                <td colspan="2">
                                    <asp:Panel runat="server" GroupingText="" ID="gbDetails" Width="100%"
                                        BackColor="White">
                                    <table width="100%">
                                        <tr>
                                             <td class="chkLabel1">
                                                    <label style="width: 100%; display: inline-block;" id="MoveToMA" runat="server">
                                                        <input type="checkbox" id="chkMoveToMA" class="chkitems" runat="server" value="Move to MA" onclick="chkMoveToMA_Click();" />Move to MA
                                                    </label>
                                                </td>
                                            <td>
                                                 <span id="lblDatelastseen" runat="server" style="width: 87%;" class="MandLabelstyle" mand="Yes">Date last seen by Physician*</span>
                                            </td>
                                            <td>
                                                <input type="text" id="dtpLastseenbyPhysician" class="Editabletxtbox" runat="server" value="" style="width: 100%;" onclick="dtpLastseenbyPhysician_OnDateSelected();"/>
                                            </td>
                                            <td>
                                                  <span id="lblDurationofneedforDME" runat="server" style="width: 87%;" class="MandLabelstyle" mand="Yes">Duration of need for DME*</span>
                                            </td>
                                            <td>
                                                 <input type="text" id="txtDurationofneedforDME" class="Editabletxtbox" runat="server" style="width: 30px; border-color: black; border-width: 1px; color: black;" maxlength="2" onkeypress="return Numeric_OnKeyPress(event);" />
                                            </td>
                                             <td>
                                                   <label id="Label4" runat="server" class="Editabletxtbox" style="width: 100%;padding-left: 10px !important;">Month(s)</label>
                                            </td>
                                            <td>
                                                  <span id="lblDurationofneedforsupplies" runat="server" style="width: 87%;" class="MandLabelstyle" mand="Yes">Duration of need for supplies*</span>
                                            </td>
                                            <td>
                                                <input type="text" id="txtDurationofneedforsupplies" runat="server" class="Editabletxtbox" style="width: 30px;  border-color: black; border-width: 1px; color: black;" maxlength="2" onkeypress="return Numeric_OnKeyPress(event);" />
                                            </td>
                                            <td>
                                                 <label id="Label6" runat="server" class="Editabletxtbox" style="width: 100%;padding-left: 10px !important;">Month(s)</label>
                                            </td>
                                            <td>
                                                <label id="Label7" runat="server" class="Editabletxtbox" style="width: 87%;">99 months * Lifetime Duration</label>
                                            </td>
                                        </tr>
                                    </table>
                                        </asp:Panel>
                                </td>
                                <td>

                                </td>
                            </tr>
                            <tr style="height: 260px">
                                <td style="height: 100%; vertical-align: top; width: 80%; padding-left: 2px !important;">
                                    <asp:Panel ID="gbProcedures" runat="server" GroupingText="Lab Procedures" Height="100%"
                                        BackColor="White" Width="972px" CssClass="LabelStyleBoldMand" >
                                        <table width="100%" style="height: 250px;">
                                            <tr style="height: 200px;width:100%;">
                                                <td style="vertical-align: top; width: 100%; height: 100%;">
                                                    <asp:Panel ID="containerFrequentlyUsedProcedures" runat="server" Width="964px" Height="97%"
                                                        ScrollBars="Horizontal">
                                                        <asp:CheckBoxList ID="chklstFrequentlyUsedProcedures" runat="server" CssClass="chkProcedurelist chkitems"
                                                            AutoPostBack="false" OnPreRender="chklstFrequentlyUsedProcedures_PreRender" onchange="chklstFrequentlyUsedProcedures_Changed();">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                             <tr style="height: 20px">
                                                 <td>
                                                     <table  width="100%">
                                                         <tr style="float:right;">
                                                             <td>
                                                                 <input type="button" id="btninvisibleDiagnosis" class="btn-primary" runat="server" value="Invisible" onserverclick="btninvisibleDiagnosis_Click"
                                                        style="margin-right: 2px; font-size: 13px; visibility: hidden;" />
                                                             </td>
                                                             <td>
                                                             </td>
                                                             <td>
                                                             </td>
                                                             <td>
                                                             </td>
                                                <td style="width: 100px">
                                                    <input type="button" id="btnEditQuantity" class="aspresizedbluebutton" runat="server" value="Edit Quantity"
                                                        accesskey="Q" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 120px;" onclick="OpenMedication_dosageDME();" />
                                                </td>
                                                <td style="width: 100px">
                                                    <input type="button" id="tblSelectProcedure" class="aspresizedbluebutton" runat="server" value="Manage Frequently Used Procedures"
                                                        accesskey="Q" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 250px;" onclick="tblSelectProcedure_Clicked();" onserverclick="tblSelectProcedure_Click" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnAllProcedures" runat="server" class="aspresizedbluebutton" value="All Procedures" 
                                                        onclick="btnAllProcedures_Clicked();" onserverclick="btnAllProcedures_Click" 
                                                        accesskey="A" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 120px;" />
                                                </td>
                                                         </tr>
                                                     </table>
                                                 </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td style="height:100%;width:20%;">
                                    <table style="height:100%">
                                        <tr style="height:60%">
                                            <td style="height:100%">
                                                 <asp:Panel ID="gbSelectICD" runat="server" GroupingText="Diagnosis" Height="195px" CssClass="LabelStyleBold"
                                        Width="200px" BackColor="White">
                                        <table style="width:100%;height:100%;">
                                            <tbody style="height:100%;">
                                            <tr style="width: 100%;height:15%;">
                                                <td class="chkLabel1">
                                                    <label style="width: 100%; white-space: nowrap; display: inline-block;margin-left:0px!important;">
                                                        <input type="checkbox" id="chkSelectALLICD"  runat="server" value="Select All" class="chkitems" onserverchange="chkSelectALLICD_CheckedChanged"  onclick="return EnableWaitCursor();"/>Select All</label>
                                                </td>
                                                <td align="right" style="width: 100%">
                                                    <input type="button" id="pbSelectICD" runat="server" class="aspresizedbluebutton" value="All Diagnosis" accesskey="N"
                                                         onclick="OpenSpecialityDiagonsisImageandLab();" 
                                                        style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 100px;" />
                                                </td>
                                            </tr>
                                            <tr style="width: 100%;height:85%;">
                                                <td colspan="2" style="vertical-align: top;height:100%;">
                                                    <asp:Panel runat="server" Width="100%" Height="90%" CssClass="Scrollable VerticalAlginTop chkitems">
                                                        <asp:CheckBoxList ID="chklstAssessment" runat="server" Width="97%" 
                                                            onclick="ChklstAssessmentEnable();">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                                </tbody>
                                        </table>
                                    </asp:Panel>
                                            </td>
                                          
                                        </tr>
                                        <tr style="height:40%">
                                              <td>
                                                <asp:Panel ID="pnlDlc" GroupingText="Notes/Instructions" runat="server" Width="200px" BackColor="White" CssClass="LabelStyleBold">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                  <UC:DLC ID="txtOrderNotes" runat="server" TextboxHeight="40px" TextboxWidth="150px"
                                                        ListboxPosition="990px" ListboxTopPosition="332px" ListboxHeight="75px" Value="DIAGNOSTIC ORDER CLINICAL INFORMATION"></UC:DLC>
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
                                <td colspan="3" align="right" style="padding-top: 25px;">
                                    <asp:Panel ID="Panel5" runat="server" Style="margin-top: -10px">
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                
                                                <td>
                                                     <input type="button" id="InvisibleButton" class="btn-primary" runat="server" value="Invisible" onserverclick="InvisibleButton_Click"
                                                        style="margin-right: 2px; font-size: 13px; visibility: hidden;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnMoveToNextProcess" class="btn-primary aspbluebutton" runat="server" style="margin-right: 2px; font-size: 13px; width: 170px;" value="Move To Next Process" onclick="if (!MoveToNextProcessClicked()) { return false; }" onserverclick="btnMoveToNextProcess_Click" visible="false" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPrintRequestion" runat="server" value="Print Requisition" class="aspresizedbluebutton" accesskey="P" style="margin-right:2px;font-size: 13px; position: relative; width: 130px;" onclick="if (!btnPrintRequsition_Clicked()) { return false; }" onserverclick="btnPrintRequestion_ServerClick"/>
                                                </td>
                                                <td>
                                                    <input type="button" id="btnAdd" runat="server" value="Add" class="aspresizedgreenbutton" onclick="btnAdd_Clicked(this);"  onserverclick="btnAdd_Click"
                                                        accesskey="S" style="margin-right: 2px; font-size: 13px; position: relative; width: 100px;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnClearAll" runat="server" value="Clear All" class="aspresizedredbutton"  onclick="if (!btnClearAll_Clicked()) { return false; }" 
                                                        accesskey="C" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 100px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                           <tr>
            <td colspan="2" style="width:100%;height:100%;">
                    
                     <asp:Panel ID="Panel1" runat="server" GroupingText="List Of Orders"
                        Width="100%" Height="230px">
                        <telerik:RadGrid ID="grdOrders" runat="server" Width="1174px" AutoGenerateColumns="False"
                            CellSpacing="0" GridLines="None" OnItemCommand="grdOrders_ItemCommand"

                             OnItemCreated="grdOrders_ItemCreated" Height="190px" CssClass="Gridbodytyle">

                            <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle"  />
                             <ItemStyle CssClass="Gridbodystyle" />
                            <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <ClientEvents OnCommand="grdOrders_OnCommand"/>
                            </ClientSettings>
                            <MasterTableView ToolTip="">
                                <Columns>
                                    <telerik:GridButtonColumn DataTextField="Edit" ButtonType="ImageButton" FilterControlAltText="Filter Edit column"
                                        HeaderText="Edit" UniqueName="Edit" ImageUrl="~/Resources/edit.GIF" 
                                        CommandName="EditC">
                                     <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" />   
                                    </telerik:GridButtonColumn>
                                    <telerik:GridButtonColumn  DataTextField="Del" ButtonType="ImageButton" FilterControlAltText="Filter Del column"
                                        HeaderText="Del" UniqueName="Del" ImageUrl="~/Resources/close_small_pressed.png" 
                                        CommandName="Del">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" />
                                    </telerik:GridButtonColumn>
                                    <telerik:GridBoundColumn DataField="Procedure" ItemStyle-CssClass="Editabletxtbox" FilterControlAltText="Filter Procedure column"
                                        HeaderText="Lab Procedure" UniqueName="Procedure">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Diagnosis"  FilterControlAltText="Filter Assessment column"
                                        HeaderText="Diagnosis" UniqueName="Diagnosis">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"  />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="VendorName" FilterControlAltText="Filter VendorName column"
                                        HeaderText="Vendor Name" UniqueName="VendorName">
                                       <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" /> 
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                        HeaderText="Location" UniqueName="Location">
                                    <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />    
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LastSeenbyPhysician" FilterControlAltText="Filter LastSeenbyPhysician column"
                                        HeaderText="Last Seen by Physician " UniqueName="Unit">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NeedforDMEDurationinMonths" FilterControlAltText="Filter NeedforDMEDurationinMonths column"
                                        HeaderText="DME Duration (Months)" UniqueName="NeedforDMEDurationinMonths">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NeedforSuppliesDurationinMonths" FilterControlAltText="Filter NeedforSuppliesDurationinMonths column"
                                        HeaderText="Supplies Duration (Months)" UniqueName="NeedforSuppliesDurationinMonths">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="NotesInstructions" FilterControlAltText="Filter NotesInstructions column"
                                        HeaderText="Notes/Instructions" UniqueName="NotesInstructions">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="OrderID" FilterControlAltText="Filter OrderID column"
                                        HeaderText="OrderID" UniqueName="OrderID" Display="false">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="OrderSubmitID" FilterControlAltText="Filter OrderSubmitID column"
                                        HeaderText="OrderSubmitID" UniqueName="OrderSubmitID" Display="false">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="LabID" FilterControlAltText="Filter LabID column"
                                        HeaderText="LabID" UniqueName="LabID" Display="false">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                     <telerik:GridBoundColumn DataField="CurrentProcess" FilterControlAltText="Filter CurrentProcess column"
                                        HeaderText="Current Process" UniqueName="CurrentProcess">
                                        <ItemStyle BorderColor="#CCFFFF" BorderStyle="Dotted" CssClass="Editabletxtbox"/>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </asp:Panel>
            </td>
        </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            

             <asp:HiddenField ID="hdnListViewArray" runat="server" />
             <asp:HiddenField ID="hdnTransferVaraible" runat="server" />
             <asp:HiddenField ID="hdnSelectedItem" runat="server" />
             <asp:HiddenField ID="hdnOrderSubmitID" runat="server" />
             <asp:HiddenField ID="hdnProcedureType" runat="server" />
             <asp:HiddenField ID="hdnPhysicianID" runat="server" />
             <asp:HiddenField ID="hdnManageFreqUsed" runat="server" Value="false" />
             <asp:HiddenField ID="hdnRowIndex" runat="server" EnableViewState="false"/>
             <asp:HiddenField ID="hdnCommandField" runat="server" EnableViewState="false"/>
            <asp:HiddenField ID="hdnMovetoNextProcess" runat="server" Value="false" />
            <asp:HiddenField ID="hdnType" runat="server" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <asp:Button ID="btnDelete" style="display:none;" runat="server" Text="DisableLoad" onclick="btnDelete_Click" />
            <asp:Button ID="btnClear" style="display:none;" runat="server"  OnClick="btnClear_Click" />
            
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSDMEOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>
         
             </asp:PlaceHolder>
         
    </form>
    <script type="text/javascript">
       
    </script>
</body>

</html>