<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmCheckOut.aspx.cs" Inherits="Acurus.Capella.UI.frmCheckOut" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CheckOut</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    
    <style type="text/css">
        .RadButton_Default.rbSkinnedButton
        {
            background-image: url('mvwres://Telerik.Web.UI,%20Version=2012.2.607.35,%20Culture=neutral,%20PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
            top: 0px;
            left: -3px;
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
        }
        .RadButton_Default .rbDecorated
        {
            background-image: url(                                                                   'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
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
        .displayNone
        {
            display: none;
        }
        #frmCheckOutt
        {
            width: 1122px;
            height: 747px;
        }
    </style>
     <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body class="bodybackground"  onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <%-- <form id="frmCheckOut" runat="server" target="_self" style="height:668px;">--%>
    <form id="frmCheckOut" runat="server" target="_self">
    <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
            <telerik:RadWindow ID="OpenPDFWindow" runat="server" Behaviors="Close" Title="PDF"
                Height="90%" IconUrl="Resources/16_16.ico" Width="100%" >
            </telerik:RadWindow>
            <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close" Title="ConsultationNotes"
                Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
            </telerik:RadWindow>
            <%--BugID:42305 --%>
            <telerik:RadWindow ID="RadWindow4" runat="server" Behaviors="Close" Title="ProgressNotes"
                Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
            </telerik:RadWindow>
          <%--  <telerik:RadWindow ID="RadWindowPatient" runat="server" Behaviors="Resize,Move,Close" IconUrl="Resources/16_16.ico" ShowContentDuringLoad="false">
            </telerik:RadWindow>--%>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="WindowEdit" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="AppointmentWindow" runat="server" Behaviors="Close" Title=""
                IconUrl="Resources/16_16.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false"
        AsyncPostBackTimeout="400">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="RadAjaxPnlCheckOut" runat="server">
        <div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <telerik:RadPanelBar ID="lblPatientStrip" runat="server" Width="100%">
                            <Items>
                                <telerik:RadPanelItem runat="server" Text="Root RadPanelItem1" BackColor="White" style="background-image:none" CssClass="pnlBarGroup">
                                </telerik:RadPanelItem>
                            </Items>
                        </telerik:RadPanelBar>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" height="280px"">
                        <asp:Panel ID="pnlgrdOrders" runat="server" Height="280px" GroupingText="Orders"
                            Width="1050px" Font-Names="Times New Roman" Font-Bold="True" CssClass="LabelStyleBold">
                            <telerik:RadGrid ID="grdorders" runat="server" AutoGenerateColumns="False" ScrollBars="Auto"
                                CellSpacing="0" GridLines="None" Width="1190px" EnableViewState="false" CssClass="Gridbodystyle">
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                                <ClientSettings Selecting-AllowRowSelect="true">
                                    <Selecting AllowRowSelect="True" />
                                    <Scrolling AllowScroll="True" ScrollHeight="250px" />
                                </ClientSettings>
                                <MasterTableView>
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="Order Type" FilterControlAltText="Filter OrderType column"
                                            HeaderText="Order Type" UniqueName="OrderType">
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                            <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Procedure/Rx/Referral" FilterControlAltText="Filter Procedure/Rx/Referral column"
                                            HeaderText="Procedure/Referral" UniqueName="Procedure/Rx/Referral">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Lab/Facility" FilterControlAltText="Filter Lab/Facility column"
                                            HeaderText="Lab/Facility" UniqueName="Lab/Facility">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Location" FilterControlAltText="Filter Location column"
                                            HeaderText="Location" UniqueName="Location">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Order ID" FilterControlAltText="Filter OrderID column"
                                            HeaderText="Order ID" UniqueName="OrderID" Visible="False">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Required Specimen" FilterControlAltText="Filter RequiredSpecimen column"
                                            HeaderText="Required Specimen" UniqueName="RequiredSpecimen">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Is Specimen In House" FilterControlAltText="Filter IsSpecimenInHouse column"
                                            HeaderText="Is Specimen In House" UniqueName="IsSpecimenInHouse" Visible="False">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Is Specimen Collected" FilterControlAltText="Filter IsSpecimenCollected column"
                                            HeaderText="Is Specimen Collected" UniqueName="IsSpecimenCollected">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Order Submit ID" FilterControlAltText="Filter OrderSubmitID column"
                                            HeaderText="Order Submit ID" UniqueName="OrderSubmitID" Visible="False">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ModifiedDateTime" FilterControlAltText="Filter ModifiedDateTime column"
                                            HeaderText="ModifiedDateTime" UniqueName="ModifiedDateTime" Visible="False">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Auth. Req." FilterControlAltText="Filter Auth.Req. column"
                                            HeaderText="Auth. Req." UniqueName="Auth.Req.">
                                             <HeaderStyle CssClass="Gridheaderstyle" />
                                             <ItemStyle CssClass="Editabletxtbox" />
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
                    <td style="width: 100%; height: 50px" align="right" valign="bottom">
                        <asp:Button ID="btnPrintLabOrder" runat="server" OnClick="btnPrintLabOrder_Click"
                            Text="Print Orders" OnClientClick="clientclick()"  CssClass="aspresizedbluebutton" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Panel ID="PnlNotes" runat="server" GroupingText="Notes" Font-Bold="True" Font-Names="Times New Roman"
                            BorderColor="Black" autoPostBack="false"  CssClass="LabelStyleBold">
                            <table style="width: 100%; height: 60px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCheckOutNotes" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                                            Width="120px" Font-Size="Small" Text="Check Out Notes" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                    </td>
                                    <td>
                                        <DLC:DLC ID="DLC" runat="server" TextboxHeight="50px" TextboxWidth="1011px"   
                                            Value="CHECK OUT NOTES" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 40%">
                                    <asp:Panel ID="pnlPrintDocuments" runat="server" Font-Bold="True" Font-Names="Times New Roman"
                                        GroupingText="Print Documents" Width="772px" BorderColor="Black" CssClass="LabelStyleBold">
                                        <table style="width: 100%; background-color: White;">
                                            <tr>
                                                <td rowspan="4">
                                                    <asp:Panel ID="pnlChklst" runat="server" Height="117px" Width="297px" ScrollBars="Auto">
                                                        <asp:CheckBoxList ID="chklstPrintDocuments" runat="server" Height="116px" Width="280px"
                                                            Font-Bold="False" onclick="TextChange();">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgLibrary" runat="server" ImageUrl="~/Resources/Database Inactive.jpg"
                                                        OnClick="imgLibrary_Click" OnClientClick="OpenAddUpdateKeywords('PATIENT EDUCATION DOCUMENTS');"
                                                        Visible="false" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRelationship" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                                                        Font-Size="Small" Text="Relationship" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboRelationship" runat="server" Height="75px" Width="212px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="cboRelationship_SelectedIndexChanged"
                                                        OnClientSelectedIndexChanged="cboRelationship_SelectedIndexChanged" CssClass="Editabletxtbox">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDocumentsgivento" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                                                        Font-Size="Small" Text="Documents given to" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDocumentsgivento" runat="server" ReadOnly="True" Width="212px"
                                                        AutoCompleteType="Disabled" EnableViewState="false" CssClass="nonEditabletxtbox ">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblIsDocumentgiven" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                                                        Font-Size="Small" Text="Is Document Given" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="cboIsDocumentGiven" runat="server" Height="40px" Width="212px"
                                                        OnClientSelectedIndexChanged="cboIsDocumentGiven_SelectedIndexChanged" CssClass="Editabletxtbox">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnPrintDocuments" runat="server" Text="Print Documents" Width="125px"
                                                        OnClick="btnPrintDocuments_Click" OnClientClick="GetUTCTime();"  CssClass="aspresizedbluebutton" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td style="width: 5%">
                                    <asp:Panel ID="pnlFollowupAppointment" runat="server" Font-Names="Times New Roman"
                                        GroupingText="Follow Up Appointment" Font-Bold="True" BorderColor="Black" Width="423px">
                                        <table style="height: 122px; background-color: White;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDueDate" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                                                        Font-Size="Small" Text="Due Date" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDueDate" runat="server" ReadOnly="True" Width="240px"
                                                        AutoCompleteType="Disabled" CssClass="nonEditabletxtbox ">
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFollowReasonNotes" runat="server" Font-Bold="False" Font-Names="Times New Roman"
                                                        Font-Size="Small" Text="Follow Reason Notes" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtFollowReasonNotes" runat="server" Height="70px" ReadOnly="True"
                                                        Width="240px" AutoCompleteType="Disabled" TextMode="MultiLine" CssClass="nonEditabletxtbox ">
                                                    </telerik:RadTextBox>
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
                    <td style="width: 100%;" align="right" valign="bottom">
                        <asp:Panel ID="pnlCheckOutButtons" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 100%" valign="bottom" align="right">
                                        <asp:Button ID="btnPatientSummary" runat="server" Text="View Patient Summary"
                                            Width="170px" Height="30px" OnClientClick="return ShowPatientSummary();"  CssClass="aspresizedbluebutton"/>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnFollowupAppointment" runat="server" Text="Follow Up Appointment"
                                            Width="170px" Height="30px" OnClick="btnFollowupAppointment_Click" OnClientClick="GetUTCTime();"  CssClass="aspresizedbluebutton"/>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnPaymentCollection" Height="30px" runat="server" Text="Payment Collection"
                                            Width="150px" OnClick="btnPaymentCollection_Click" CssClass="aspresizedbluebutton"/>
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnCheckOut" runat="server" Height="30px" Text="Check Out" Width="152px"
                                            OnClientClick=" return Checkout();" CssClass="aspresizedbluebutton" />
                                        &nbsp;&nbsp; &nbsp;<asp:Button ID="btnClose" Height="30px" runat="server" Text="Close"
                                            Width="100px" OnClientClick="return Checkout(event);" autoPostBack="false" CssClass="aspresizedredbutton" />
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        
        <asp:HiddenField ID="hdnSelectedItem" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnNotes" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncStatus" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFacility" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPhyName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPhyID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnDuedate" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsCheckout" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnXmlPath" runat="server" />
        <asp:HiddenField ID="hdnPrintFilePath" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsFormClosed" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnAppointment" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="HiddenDLC" runat="server" EnableViewState="false"  Value=""/>
       
       <%--  <asp:HiddenField ID="hdnnotes" runat="server" EnableViewState="false"  Value=""/>--%>
        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
            OnClientClick=" return Checkout();" />

     
        <asp:HiddenField ID="hdnButtonName" runat="server" />
        <asp:HiddenField ID="hdnMessageType" runat="server" />
           <asp:HiddenField ID="HiddenhumanDetails" runat="server" />
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel2" runat="server">
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
        </div>
    </telerik:RadAjaxPanel>

    <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server">

    <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSCheckOut.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSC5PO.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
         
    </form>
    <div id="ModalProgressNotesCheckout" class="modal fade" style="height: 800px;display:none;">
            <div class="modal-dialog" style="height: 100%; width: 1050px; margin-left: 7%">
                <div class="modal-content" style="height: 99%; width: 100%">
                    <div class="modal-header" style="padding-top: 0px; padding-bottom: 0px;">
                        <button type="button" id="btnClosewindowNotes" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h5 id="ModalTtleNotes" style="font-weight: bold;"></h5>
                    </div>
                    <div class="modal-body" style="height: 100%; width: 100%">
                        <iframe style="width: 100%; height: 100%; border: none; overflow: hidden; position: relative" id="ProcessiFrameNotesCheckout"></iframe>
                    </div>
                </div>
            </div>
        </div>
</body>
</html>
