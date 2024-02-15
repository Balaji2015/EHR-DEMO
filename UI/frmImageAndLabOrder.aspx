<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmImageAndLabOrder.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmImageAndLabOrder" ValidateRequest="false" %>

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

        .chkProcedurelist {
            height: 283px !important;
            width: 700px;
        }

        .chkLabel label {
            display: block;
            margin-left: -8.5em;
        }

        .chkLabel1 label {
            display: block;
            margin-left: 1.3em;
        }
        .chkLabel2 label {
            display: block;
            /*margin-left: 1em;*/
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
    </style>
    <base target="_self" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/datetimepicker.css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
  


</head>
<body onload="FormLoad();OpenNotificationPopUp('ORDER');">
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
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Diagnostic Order"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>

            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager2" runat="server">
            <Windows>
                <telerik:RadWindow ID="RadWindowImportResult" runat="server" Behaviors="Close,Move"
                    OnClientClose="ExamClose" Title="Diagnostic Order" IconUrl="Resources/16_16.ico">
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
                        <table style="width:100%;">
                            <tr>
                                <td>
                                    <p class="verti" style="height: 90px; margin-top: -22em; width: 22px; margin-left: -2px;">
                                        <asp:HyperLink ID="lnkDiagnosticOrder" Style="position: absolute; width: 78px; font-weight: bold;"
                                            CssClass="verti" runat="server" NavigateUrl="~/frmImageAndLabOrder.aspx"
                                            EnableViewState="false">Detailed Order</asp:HyperLink>
                                    </p>
                                    <p class="verti " style="height: 90px; margin-top: -1em; width: 20px; margin-left: -2px;">
                                        <asp:HyperLink Style="position: absolute; width: 75px; font-weight: bold;"
                                            CssClass="verti" ID="lnkOrderList" runat="server" NavigateUrl="~/frmOrdersList.aspx"
                                            EnableViewState="false">Order List</asp:HyperLink>
                                    </p>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <!--<table style="margin-left: -12px;width:1185px;">-->
                        <table style="margin-left: -12px;width:90%;"
                            <%--<tr>
                <td colspan="2">
                    <asp:HyperLink ID="lnkDiagnosticOrder" runat="server" NavigateUrl="~/frmImageAndLabOrder.aspx"
                        EnableViewState="false">Detailed Order</asp:HyperLink><span style="padding-left: 20px;">
                            <asp:HyperLink ID="lnkOrderList" runat="server" NavigateUrl="~/frmOrdersList.aspx"
                                EnableViewState="false">Order List</asp:HyperLink></span>
                    
                </td>
            </tr>--%>
                            <tr>
                                <td >
                                    <asp:Panel runat="server" GroupingText="Center Details" ID="gbCenterDetail" Width="100%" 
                                        BackColor="White" CssClass="spanstyle">
                                        <table width="100%;">
                                            <tr>
                                                <td style="width: 50px;">
                                                    <%--<asp:Label ID="lblLab" runat="server" Text="Lab *" Width="100%" ForeColor="Red"></asp:Label>--%>
                                                    <span id="lblLab" runat="server" style="width: 100%;" class="MandLabelstyle" mand="Yes">Lab</span><span class="manredforstar">*</span>
                                                </td>
                                                <td style="width: 230px;">
                                                    <%--<telerik:RadComboBox ID="cboLab" runat="server" Width="100%" Height="200px" OnClientSelectedIndexChanged="EnableWaitCursor"
                                        OnSelectedIndexChanged="cboLab_SelectedIndexChanged" AutoPostBack="True">
                                    </telerik:RadComboBox>--%>
                                                    <select id="cboLab" runat="server" style="width: 100%; height: 20px;" onchange="EnableWaitCursorcbolab();" onserverchange="cboLab_SelectedIndexChanged" class="Editabletxtbox">
                                                    </select>
                                                </td>
                                                <td style="width: 100px;">
                                                    <%--<asp:Label ID="lblCenterName" runat="server" Text="Center Name" Width="100%"> </asp:Label>--%>
                                                    <label id="lblCenterName" runat="server" style="width: 100%;" class="spanstyle">Center Name</label>
                                                </td>
                                                <td style="width: 160px;">
                                                    <%--<telerik:RadTextBox ID="txtCenterName" runat="server" Width="100%" ReadOnly="True">
                                        <ReadOnlyStyle BackColor="#BFDBFF" BorderColor="Black" BorderWidth="1px" ForeColor="Black" />
                                    </telerik:RadTextBox>--%>
                                                    <input type="text" id="txtCenterName" runat="server" style="width: 100%; height: 100%; border-width: 1px; color: black;" class="nonEditabletxtbox"/>
                                                </td>
                                                <td style="width: 80px;">
                                                    <%--<asp:Label ID="lblLocation" runat="server" Text="Location" Width="100%"></asp:Label>--%>
                                                    <label id="lblLocation" runat="server" style="width: 100%;" class="spanstyle">Location</label>
                                                </td>
                                                <td style="width: 200px;">
                                                    <%--<telerik:RadTextBox ID="txtLocation" runat="server" Width="100%" ReadOnly="True">
                                        <ReadOnlyStyle BackColor="#BFDBFF" BorderColor="Black" BorderWidth="1px" ForeColor="Black" />
                                    </telerik:RadTextBox>--%>
                                                    <input type="text" id="txtLocation" runat="server" style="width: 100%; height: 100%; border-width: 1px; color: black;" class="nonEditabletxtbox" />
                                                </td>
                                                <td style="width: 130px;">
                                                    <%-- <telerik:RadButton ID="btnSelectLocation" OnClientClicked="btnSelectLocations" runat="server" Text="Select Location" Width="100px"
                                        OnClick="btnSelectLocation_Click" AccessKey="O" Style="text-align: center; -moz-border-radius: 3px;
                                        -webkit-border-radius: 3px; position: relative;">
                                        <ContentTemplate>
                                            Select L<span class="underline">o</span>cation
                                        </ContentTemplate>
                                    </telerik:RadButton>--%>
                                                    <input type="button" id="btnSelectLocation" runat="server" value="Select Location" class="aspresizedbluebutton" onclick="btnSelectLocations(); return false;" onserverclick="btnSelectLocation_Click"
                                                        accesskey="O" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 92%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="overflow: scroll; height: 130px;">
                              <td  style="width: 25%; vertical-align: top; height: 130px;">
                                  <table>
                                      <tr>
                                  <td style="width: 20%; vertical-align: top;">
                                    <asp:Panel ID="gbSpecimenDetails" runat="server" GroupingText="Specimen Details"
                                        Height="100%" Width="100%" BackColor="White" CssClass="spanstyle">
                                        <table style="height: inherit; height: 130px;width:98%;">
                                            <tr>
                                                <td  colspan="2"  class="chkLabelleft">
                                                    <%--<asp:CheckBox ID="chkSpecimenInHouse" runat="server" Text="Specimen In House" Width="100%"
                                        onClick="ChkSpecimenInHouseCheck();" CssClass="noWrapText" />--%>
                                                    <label style="width: 100%; display: inline-block;">
                                                        <input type="checkbox" id="chkSpecimenInHouse" value="Specimen In House" runat="server" style="" onclick="ChkSpecimenInHouseCheck();" class="Editabletxtbox"/>Specimen In House</label>
                                                </td>
                                                <td  colspan="2" class="chkLabel1">
                                                    <%--<asp:CheckBox ID="chkMoveToMA" runat="server" Text="Move to MA" Width="100%" onClick="chkMoveToMA_Click();" />--%>
                                                    <label style="width: 100%; display: inline-block;" id="MoveToMA" runat="server">
                                                        <input type="checkbox" id="chkMoveToMA" runat="server" value="Move to MA" onclick="chkMoveToMA_Click();" class="Editabletxtbox"/>Move to MA
                                                    </label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td   colspan="2">
                                                    <%--<asp:Label ID="lblCollectionDate" runat="server" Text="Collection Date" Width="87%"></asp:Label>--%>
                                                    <label id="lblCollectionDate" runat="server" style="width: 87%;" class="spanstyle">Collection Date</label>
                                                </td>
                                                <td  colspan="2">
                                                    
                                                    <input type="text" id="dtpCollectionDate" runat="server" value="" style="width: 100%;" onclick="dtpCollectionDate_OnDateSelected();" class="Editabletxtbox"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td  colspan="2">
                                                    
                                                    <label id="lblSpecimen" runat="server" style="width: 100%;" class="spanstyle">Specimen</label>
                                                </td>
                                                <td  colspan="2">
                                                   
                                                    <select id="cbospecimen" runat="server" style="width: 100%;" onchange="EnableSaveDiagnosticOrder();" class="Editabletxtbox">
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 60px;">
                                                    <%--<asp:Label ID="lblQuantity" runat="server" Text="Quantity" Width="100%"></asp:Label>--%>
                                                    <label id="lblQuantity" runat="server" style="width: 100%;" class="spanstyle">Quantity</label>
                                                </td>
                                                <td  style="width: 60px;">
                                                    
                                                    <input type="text" id="txtQuantity" runat="server" maxlength="5" onblur="Quality_TextChanged();" onkeypress="return AllowNumbersImageAndLab(event);" style="width: 100%;" class="Editabletxtbox"/>
                                                </td>
                                                <td style="width: 60px;">
                                                    <%-- <asp:Label ID="lblUnits" runat="server" Text="Units" Width="100%"></asp:Label>--%>
                                                    <label id="lblUnits" runat="server" style="width: 100%;" class="spanstyle">Units</label>
                                                </td>
                                                <td style="width: 60px;">
                                                    <%--<telerik:RadComboBox ID="cboSpecimenUnits" runat="server" Width="100%" OnClientSelectedIndexChanged="EnableSaveDiagnosticOrder">
                                    </telerik:RadComboBox>--%>
                                                    <select id="cboSpecimenUnits" runat="server" style="width: 100%;" onchange="EnableSaveDiagnosticOrder(); " class="Editabletxtbox"></select>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td style="width: 25%; vertical-align: top;">
                                    <asp:Panel ID="gbBillingDetails" runat="server" GroupingText="Billing Details"
                                        Height="100%" Width="100%" BackColor="White" CssClass="spanstyle">
                                        <table style="height: inherit; height: 130px;width:98%;">
                                            <tr>
                                                 <td>
                                                    <span id="lblBillType" runat="server" style="width: 100%;" mand="Yes">Bill Type*</span>
                                                </td>
                                                <td class="style2" colspan="2">
                                                    <select id="cboBillType" runat="server" style="width: 183px;" onchange="EnableSaveDiagnosticOrderbilltype();" onserverchange="cboBillType_SelectedIndexChanged" class="Editabletxtbox"></select>
                                                    <telerik:RadButton ID="dbInsurancePlan" runat="server" Width="100%" Image-ImageUrl="~/Resources/Database Inactive.jpg" Style="display: none;" class="Editabletxtbox">
                                                    </telerik:RadButton>
                                                    <asp:ImageButton ID="dbInsurance" runat="server" ImageUrl="~/Resources/Database Inactive.jpg" Style="display: none;" class="Editabletxtbox"
                                                        OnClientClick="OpenInsurance();return false;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label id="lblClinicalInformation" runat="server" style="width: 100%;" class="spanstyle">Clinical Information</label>
                                                </td>
                                                <td>
                                                    <UC:DLC ID="txtOrderNotes" runat="server" TextboxHeight="77px" TextboxWidth="166px" TextboxMaxLength="1024"
                                                        ListboxPosition="322px" ListboxTopPosition="185px" ListboxHeight="75px" Value="DIAGNOSTIC ORDER CLINICAL INFORMATION" class="Editabletxtbox"></UC:DLC>
                                                </td>
                                            </tr>
                                            </table>
                                            </asp:Panel>
                                </td>
                                <td style="width: 30%; vertical-align: top; height: 130px;">
                                   <asp:Panel ID="gbOrderDetails" runat="server" GroupingText="Diagnostic Order Details" Style="position: relative;" 
                                        BackColor="White" Height="100%" CssClass="spanstyle" >
                                        <table id="temptbl" style="width: 100%; height: 130px;margin-left:-11px">
                                            <tr>
                                                <td>
                                                <table style="width:100%">
                                                    <tr>
                                                         <td class="chkLabel1">
                                                     <label style="width: 100%;  white-space: nowrap;">
                                                        <input type="checkbox" id="chkYesFasting" value="Fasting" runat="server" onclick="EnableSaveDiagnosticOrder();" class="spanstyle"/>Fasting
                                                    </label>

                                                </td>
                                                <td class="chkLabel1">
                                                    <label style="width: 100%; white-space: nowrap; ">
                                                        <input type="checkbox" id="chkYesABN" runat="server" style="" class="spanstyle"
                                                            onclick="EnableSaveDiagnosticOrder();" />Is ABN Signed</label>
                                                </td>
                                                <td class="chkLabel1" style="padding-left:15px!important;">
                                                     <label style="width: 100%; white-space: nowrap; ">
                                                        <input type="checkbox" id="chkAuthRequired" value="Auth Required" runat="server"
                                                            onclick="EnableSaveDiagnosticOrder();" class="spanstyle"/>Auth Required</label>
                                                </td>
                                                    </tr>
                                                </table>
                                                 </td>                                            


                                               
                                            </tr>
                                            <tr>
                                                <td>
                                                <table style="width:100%">
                                                    <tr>
                                                          <td class="chkLabel1">
                                                    <label style="width: 100%; white-space: nowrap; ">
                                                        <input type="checkbox" id="chkTestDateInDate" runat="server" value="In Date"
                                                            onclick="chkTestDataeInDateClick();" checked="checked" class="spanstyle"/>In Date</label>
                                                </td>
                                                <td>
                                                   <span id="lblTestDate" runat="server" style="width: 100%;white-space:nowrap; margin-left: 1em;" class="MandLabelstyle" mand="Yes">Test Date</span><span class="manredforstar">*</span>

                                                    <input type="text" id="cstdtpTestDate" runat="server" value="" style="width: 55%;padding-left: 11px !important;" onclick="dtpCollectionDate_OnDateSelected();" class="Editabletxtbox"/>
                                                </td>
                                               <td>
                                                    <label style="width: 100%; white-space: nowrap; ">
                                                        <input type="checkbox" id="chkStat" runat="server" value="Stat" onclick="chkStat_CheckedChanged();" class="spanstyle"/>Stat</label>

                                                </td>
                                                  <td>
                                                    <label style="width: 100%; white-space: nowrap;">
                                                        <input type="checkbox" id="chkUrgent" runat="server" value="Urgent" class="spanstyle" onclick="chkUrgent_CheckedChanged();"/>Urgent</label>
                                                </td>
                                                    </tr>

                                                </table>
                                                 </td>
                                                <%--<td>

                                                </td>--%>
                                               
                                            </tr>
                                            <tr>
                                                <td>
                                                <table style="width:100%">
                                                    <tr>
                                                                                                       
                                                <td class="chkLabel1" style="padding-right:5px">                                                 
                                                    <label style="width: 100%; white-space: nowrap; ">
                                                        <input type="checkbox" id="chkTestDateInWords" runat="server" value="In Days"
                                                            onclick="chkTestDataeInWordsClick();" class="spanstyle"/>In Days</label>
                                                </td>
                                                        <td>
                                                                
                                                                <label id="lblMonths" runat="server" style="width: 100%;padding-left: 10px !important;" class="Editabletxtbox">Month(s)</label>
                                                            </td>
                                                            <td style="width: 50px;">
                                                               
                                                                <input type="text" id="txtMonths" runat="server" style="width: 30px;  border-width: 1px; color: black;" maxlength="2" onkeypress="return Numeric_OnKeyPress(event);" class="nonEditabletxtbox" />
                                                            </td>
                                                            <td>
                                                                
                                                                <label id="lblWeeks" runat="server" style="width: 100%; white-space: nowrap;" class="Editabletxtbox">Week(s)</label>
                                                            </td>
                                                            <td style="width: 50px">
                                                                
                                                                <input type="text" id="txtWeeks" runat="server" style="width: 30px; border-width: 1px; color: black;" maxlength="2" onkeypress="return Numeric_OnKeyPress(event);" class="nonEditabletxtbox"/>
                                                            </td>
                                                            <td>
                                                                
                                                                <label id="lblDays" runat="server" style="width: 100%; white-space: nowrap;" class="Editabletxtbox">Day(s)</label>
                                                            </td>
                                                            <td>                                                               
                                                                <input type="text" id="txtDays" runat="server" style="width: 30px; border-width: 1px; color: black;" maxlength="2" onkeypress="return Numeric_OnKeyPress(event);" class="nonEditabletxtbox" />
                                                            </td>
                                                    </tr>

                                                </table>
                                                 </td>



                                               
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td style="width:25%; vertical-align:top;height:130px;">
                                      <asp:Panel ID="gbSelectICD" runat="server" GroupingText="Diagnosis" Height="100%"  
                                        Width="100%" BackColor="White" CssClass="spanstyle">
                                        <table style="width:100%;height:93%;">
                                            <tbody style="height:85%;">
                                            <tr style="width: 100%;height:20%;">
                                                <td class="chkLabel1">
                                                    <label style="width: 100%; white-space: nowrap; display: inline-block;margin-left:0px!important;">
                                                        <input type="checkbox" id="chkSelectALLICD" runat="server" value="Select All" onserverchange="chkSelectALLICD_CheckedChanged"
                                                            onclick="return EnableWaitCursor();" class="spanstyle" />Select All</label>
                                                </td>
                                                <td align="right" style="width: 100%">
                                                    <input type="button" id="pbSelectICD" runat="server" class="aspresizedbluebutton" value="All Diagnosis" accesskey="N"
                                                        onclick="OpenSpecialityDiagonsisImageandLab();" style="margin-right: 14px; font-size: 13px; text-align: center; position: relative; width: 120px;" />
                                                </td>
                                            </tr>
                                            <tr style="width: 100%;height:79.9%;">
                                                <td colspan="2" style="vertical-align: top;height:100%;">
                                                    <asp:Panel runat="server" Width="97%" Height="100%" CssClass="Scrollable VerticalAlginTop">
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
                                      </table>
                                  </td>
                            </tr>
                            <tr style="height: 350px">
                                <td colspan="3" style="height: 100%; vertical-align: top; width: 100%; padding-left: 2px !important;">
                                    <asp:Panel ID="gbProcedures" runat="server" GroupingText="Lab Procedures" Height="100%"
                                        BackColor="White" Width="100%" CssClass="LabelStyleBoldMand">
                                        <table width="100%" style="height: 340px;">
                                            <tr style="height: 260px;width:100%;">
                                                <td colspan="7" style="vertical-align: top; width: 100%; height: 100%;">
                                                    <asp:Panel ID="containerFrequentlyUsedProcedures" runat="server" Width="1150px" Height="97%"
                                                        ScrollBars="Horizontal" CssClass="spanstyle">
                                                        <asp:CheckBoxList ID="chklstFrequentlyUsedProcedures" runat="server" CssClass="chkProcedurelist spanstyle "
                                                            AutoPostBack="false" OnPreRender="chklstFrequentlyUsedProcedures_PreRender" onchange="chklstFrequentlyUsedProcedures_Changed();">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr style="height: 20px">
                                                 <td style="width: 100px">
                                                    <asp:RadioButton ID="rbtnSubmitAfter3Hour" runat="server" Text="Submit Later" GroupName="SubmitPriority" CssClass="spanstyle"
                                                        Checked="True" />
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:RadioButton ID="rbtnSubmitImmediately" runat="server" Text="Submit Now" GroupName="SubmitPriority" CssClass="spanstyle" />
                                                </td>
                                                <td style="width: 130px" class="chkLabelleft">
                                                    <label style="width: 100%; white-space: nowrap; display: inline-block; font-weight: normal ">
                                                        <input type="checkbox" id="chkpaperorder" runat="server" value="Filled in Paper Form" onclick="chkpaperorderChanged();"  visible="true" class="spanstyle"/>Filled in Paper Form</label>
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:RadioButton ID="rbLabOrder" runat="server" Text="Lab Order" GroupName="Paper Form" CssClass="spanstyle"/>
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:RadioButton ID="rbImageOrder" runat="server" Text="Image Order" GroupName="Paper Form" CssClass="spanstyle"/>
                                                </td>
                                                <td style="width: 100px">
                                                     <input type="button" id="tblSelectProcedure" class="aspresizedbluebutton" runat="server" value="Manage Frequently Used Procedures"
                                                        accesskey="Q" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 250px;" onclick="tblSelectProcedure_Clicked();" onserverclick="tblSelectProcedure_Click" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnAllProcedures" runat="server" class="aspresizedbluebutton" value="All Procedures" onclick="btnAllProcedures_Clicked();" onserverclick="btnAllProcedures_Click"
                                                        accesskey="A" style="margin-right: 2px; font-size: 13px; text-align: center; position: relative; width: 120px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                
                            </tr>
                            <tr>
                                <td colspan="3" align="right" style="padding-top: 25px;">
                                    <asp:Panel ID="Panel5" runat="server" Style="margin-top: -10px">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btninvisibleDiagnosis" class="aspresizedbluebutton" runat="server" value="Invisible" onserverclick="btninvisibleDiagnosis_Click"
                                                        style="margin-right: 2px; font-size: 13px; visibility: hidden;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="InvisibleButton" class="aspresizedbluebutton" runat="server" value="Invisible" onserverclick="InvisibleButton_Click"
                                                        style="margin-right: 2px; font-size: 13px; visibility: hidden;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btninvisible" runat="server" class="aspresizedbluebutton" value="Invisible" onserverclick="btninvisible_Click"
                                                        style="margin-right: 2px; font-size: 13px; visibility: hidden;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnMoveToNextProcess" class="aspresizedbluebutton" runat="server" style="margin-right: 2px; font-size: 13px; width: 170px;" value="Move To Next Process" onclick="if (!MoveToNextProcessClicked()) { return false; }" onserverclick="btnMoveToNextProcess_Click" />
                                                </td>
                                                <td>
                                                       <input type="button" id="btnEditQuantity" runat="server" class="aspresizedbluebutton" value="Edit Quantity" onclick="OpenMedication_dosage()" 
                                                        accesskey="E" style="margin-right: 3px; font-size: 13px; text-align: center; position: relative; width: 100px;" />
                                               </td>
                                                <td>
                                                    <input type="button" id="btnImportresult" runat="server" class="aspresizedbluebutton" visible="false" value="Import Result" onclick="btnImportresult_Clicked();" onserverclick="btnImportresult_Click"
                                                        accesskey="I" style="margin-right: -2px; font-size: 13px; text-align: center; position: relative; width: 100px;" />

                                                </td>
                                                <td>
                                                    <input type="button" id="btnPlan" runat="server" class="aspresizedbluebutton" value="Plan" onclick="ImageAndLabOrderPlan();"
                                                        accesskey="L" style="margin-right: 2px; font-size: 13px; text-align: center; display: none; position: relative; width: 40px;" visible="false" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnOrderList" runat="server" class="btn-primary" style="margin-right: 2px; font-size: 13px; width: 100px;" value="Order List" onclick="btnOrderList_Clicked();" onserverclick="btnOrderList_Click" visible="false" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPrintlabel" runat="server" value="Print label" class="aspresizedbluebutton" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }" onserverclick="btnPrintlabel_Click"
                                                        accesskey="R" style="margin-right: 1px; font-size: 13px; position: relative; width: 100px;" />
                                                </td>
                                                <td>
                                                     <input type="button" id="btnPrintRequsition" runat="server" value="Print Requisition" class="aspresizedbluebutton" onclick="if (!btnPrintRequsition_Clicked()) { return false; }" onserverclick="btnPrintRequsition_Click"
                                                        accesskey="P" style="margin-right: 3px; font-size: 13px; position: relative; width: 150px;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnGenerateABN" runat="server" value="Generate ABN" class="aspresizedbluebutton" onclick="btnGenerateABN_Clicked();" onserverclick="btnGenerateABN_Click"
                                                        accesskey="G" style="margin-right: 3px; font-size: 13px; position: relative; display: none; width: 100px;" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnOrderSubmit" runat="server" value="Save and Submit" class="aspresizedgreenbutton" onclick="if (!btnOrderSubmit_Clicked(this)) { return false; }" onserverclick="btnOrderSubmit_Click"
                                                        accesskey="S" style="margin-right: 2px; font-size: 13px; position: relative; width: 150px;" />
                                                </td>
                                                <td>
                                                     <input type="button" id="btnClearAll" runat="server" value="Clear All" class="aspresizedredbutton" onclick="if (!btnClearAll_Clicked()) { return false; }"
                                                        accesskey="C" style="margin-right: 11px; font-size: 13px; text-align: center; position: relative; width: 100px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            <div style="width: 100%; height: 47px;margin-top:-30px;">
                <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Diagnostic_Order');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                </div>
                <div id="tag" style="margin-top: -13px; margin-left: -99px; font-size: 19px; height: 48px; width: 301px; font-weight: bold; color: #6504d0; border-radius: 7px; cursor: pointer; font-family: source sans pro;" onclick="OpenMeasurePopup('Diagnostic_Order');">
                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                </div>
            </div>

            <asp:Button ID="hdnbuttonload" runat="server" OnClick="hdnbuttonload_Click" CssClass="displayNone"/>
            <asp:Button ID="btnClear" runat="server" CssClass="displayNone" OnClick="btnClear_Click" />
            <asp:HiddenField ID="hdnForEditErrorMsg" runat="server" />
            <asp:HiddenField ID="hdnToFindSource" runat="server" />
            <asp:HiddenField ID="hdnTransferVaraible" runat="server" />
            <asp:HiddenField ID="hdnOrderSubmitID" runat="server" />
            <asp:HiddenField ID="hdnProcedureType" runat="server" />
            <asp:HiddenField ID="hdnPhysicianID" runat="server" />
            <asp:HiddenField ID="hdnHumanID_EncounterID_PhysicianID" runat="server" />
            <asp:HiddenField ID="hdnListViewArray" runat="server" />
            <asp:HiddenField ID="hdnSelectedItem" runat="server" />
            <asp:HiddenField ID="hdnCollectionDateIsMand" runat="server" Value="false" />
            <asp:HiddenField ID="hdnUnitsIsMand" runat="server" Value="false" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" />
            <asp:HiddenField ID="hdnManageFreqUsed" runat="server" Value="false" />
            <asp:HiddenField ID="hdnType" runat="server" />
            <asp:HiddenField ID="hdnMoveToMA" runat="server" />
            <asp:HiddenField ID="hdnPrintLabel" runat="server" />
            <asp:HiddenField ID="hdnOrderSubmitClick" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnsaveEnable" runat="server" />
            <asp:HiddenField ID="hdnImportResult" Value="false" runat="server" />
            <%--Added for import result --%>
            <asp:Button ID="btnMessageType" AutoPostBack="false" runat="server" Text="Button"
                Style="display: none" OnClientClick="btnClose_Clicked();" />
            <asp:HiddenField ID="hdnLabLocation" runat="server" />
            <asp:HiddenField ID="hdnpaper" runat="server" />
            <asp:HiddenField ID="hdnMovetoNextProcess" runat="server" Value="false" />
            <asp:HiddenField ID="hdnSelectedOrder" runat="server" Value="" />
            <asp:HiddenField ID="hdnCMGAncillarySaveOrder" runat="server" Value="false" />
            <asp:HiddenField ID="hdnMovetoOrderSubmitId" runat="server" Value="false" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSImageAndLabOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSExamPhotos.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
          <%--  <script src="JScripts/JSReferralOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>
            <script type="text/javascript">
                SetReadOnly();
            </script>

        </asp:PlaceHolder>
    </form>
</body>

</html>
