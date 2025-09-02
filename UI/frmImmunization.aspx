<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmImmunization.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmImmunization" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <title>Immunization</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
  <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>


    <style type="text/css">
        #frmImmunization {
            height: 925px;
            width: 100%;
        }

        .style4 {
            height: 300px;
        }

        .style5 {
            height: 580px;
        }

        .style6 {
            height: 223px;
        }

        .style7 {
            height: 277px;
        }

        .style8 {
            height: 580px;
            width: 250px;
        }

        .style9 {
            width: 250px;
        }

        .style10 {
            width: 237px;
        }

        .style12 {
            height: 36px;
        }

        .style15 {
            height: 36px;
            width: 169px;
        }

        .style19 {
            width: 169px;
        }

        .style22 {
        }

        .style26 {
            height: 36px;
            width: 169px;
        }

        .style27 {
            height: 36px;
            width: 189px;
        }

        .style28 {
            height: 36px;
            width: 144px;
        }

        .style29 {
            height: 36px;
        }

        .style30 {
            height: 32px;
            width: 169px;
        }

        .style31 {
            height: 32px;
            width: 189px;
        }

        .style32 {
            height: 32px;
            width: 144px;
        }

        .style33 {
            height: 32px;
        }

        .style38 {
            height: 33px;
            width: 169px;
        }

        .style39 {
            height: 33px;
            width: 189px;
        }

        .style40 {
            height: 33px;
            width: 144px;
        }

        .style41 {
            height: 33px;
        }

        .style42 {
            height: 23px;
            width: 169px;
        }

        .style43 {
            height: 23px;
            width: 189px;
        }

        .style44 {
            height: 23px;
        }

        .style51 {
            width: 224px;
        }

        .style55 {
            height: 36px;
            width: 178px;
        }

        .style56 {
            width: 165px;
        }

        .style57 {
            width: 165px;
            height: 36px;
        }

        .style58 {
            width: 161px;
        }

        .style59 {
            height: 36px;
            width: 161px;
        }

        .style60 {
            height: 434px;
        }

        #btnInvisible {
            display: none;
        }

        #btnRefreshImm_proce {
            display: none;
        }

        #btnVis {
            display: none;
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

        .underline {
            text-decoration: underline;
        }

        .rgNoRecords, .rgClipCells {
            width: 100% !important;
        }

        .displayNone {
            display: none;
        }

        body {
            zoom: 1.0 !important;
            -moz-transform: scale(1) !important;
            -moz-transform-origin: 0 0 !important;
        }

        #tag:hover {
            text-decoration: underline;
        }

        
        #pnlVaccineAdminDetails fieldset {
            height: 273px;
        }

    </style>

    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="Stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">

        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>

        <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>

        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>

        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>

        <script src="JScripts/JSImmunization.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>

    </asp:PlaceHolder>
</head>
<body onload="OnLoadImmunization();OpenNotificationPopUp('ORDER-IMMUNIZATION');" style="margin-left: 0px; margin-right: 0px; height: 650px; overflow-y: hidden;">
    <form id="frmImmunization" runat="server" style="background-color: White; font-family: Microsoft Sans Serif; font-size: smaller; margin: 0px; padding: 0px;"
        scrollbars="Vertical">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" HorizontalAlign="NotSet"
            Width="100%">
            <telerik:RadWindowManager ID="WindowMngr1" runat="server" VisibleStatusbar="false">
                <Windows>
                    <telerik:RadWindow ID="ModalWindowPrint" runat="server" VisibleOnPageLoad="false"
                        Height="625px" Modal="true" IconUrl="Resources/16_16.ico" Width="1225px" Behaviors="Move,Close">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadScriptManager ID="Scritp" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadWindowManager ID="WindowMngr" runat="server" VisibleStatusbar="false">
                <Windows>
                    <telerik:RadWindow ID="ModalWindow" runat="server" Behaviors="Close" VisibleOnPageLoad="false"
                        Height="625px" Modal="true" IconUrl="Resources/16_16.ico" Width="1225px" BackColor="#bfd7ff">
                    </telerik:RadWindow>

                    <telerik:RadWindow ID="MessageWindow" runat="server" Title="Immunization" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="PrintVISWindow" runat="server" Behaviors="Close" Title="Immunization Order" VisibleOnPageLoad="false"
                        IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindowPlan" runat="server" VisibleOnPageLoad="false" Height="625px"
                        Behaviors="Close" IconUrl="Resources/16_16.ico" Width="1225px" Modal="true">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindow1" runat="server" VisibleOnPageLoad="false" Height="650px"
                        Modal="true" IconUrl="Resources/16_16.ico" Width="900px"
                        Behaviors="None">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindow2" runat="server" VisibleOnPageLoad="true" Height="650px"
                        Modal="true" Behaviors="Close,Move" IconUrl="Resources/16_16.ico" Width="900px">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <div style="height: 548px; overflow-y: auto; margin: 0px; padding: 0px" runat="server" id="immGrid">
                <table style="width: 100%; height: 100%; margin: 0px; padding: 0px">
                    <tr>
                        <td class="style8" valign="top" style="margin: 0px; padding: 0px">
                            <asp:Panel ID="gbImmunization" runat="server" GroupingText="Frequently Used Immn./Inj. Procedures"
                                Height="610px" Font-Bold="True" Width="370px" CssClass="LabelStyleBold">
                                <table style="width: 100%; height: 565px; margin: 0px; padding: 0px">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSelectImmunProcedure" runat="server" Text="Select an Immn./Inj. Procedure"
                                                Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style60">
                                            <telerik:RadListBox ID="chklstImmunizationProcedures" runat="server" Height="550px" onclick="chklstImmunizationProcedures_Load();"
                                                Width="336px" Font-Bold="False" CheckBoxes="True" AutoPostBack="True" OnSelectedIndexChanged="chklstImmunizationProcedures_SelectedIndexChanged"
                                                OnItemCheck="chklstImmunizationProcedures_ItemCheck" SelectionMode="Single" CssClass="Editabletxtbox">
                                                <ButtonSettings TransferButtons="All" />
                                            </telerik:RadListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnManageFrequentlyUsedImmunProc" ButtonType="LinkButton" runat="server" Text="Manage Freq. Used Immn./Inj."
                                                OnClientClicked="btnManageFrequentlyUsedImmunProc_Clicked" Width="305px"
                                                Style="right: -13px;padding: 4px 12px !important; height: 23px !important;
    font-size: 12px !important;" CssClass="bluebutton teleriknormalbuttonstyle">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td class="style5" colspan="2" valign="top" style="margin: 0px; padding: 0px">
                            <asp:Panel ID="pnlmmunizationDetail" runat="server" GroupingText="Immn./Inj. Details    "
                                Height="586px" Width="100%" Font-Bold="True" CssClass="LabelStyleBold">
                                <table style="width: 100%; height: 600px; margin: 0px; padding: 0px">
                                    <tr>
                                        <td class="style7" colspan="2" valign="top" style="margin: 0px; padding: 0px">
                                            <asp:Panel ID="pnlOrderDetails" runat="server" GroupingText="Order Details" Height="325px" CssClass="LabelStyleBold">
                                                <table style="width: 100%; height: 282px;">
                                                    <tr>
                                                        <td colspan="4" style="text-align:right; padding-bottom:0px;">
                                                            <asp:Label id="lblAllergy" runat="server" ForeColor="Red" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style15">
                                                            <%--<asp:Label ID="lblImmunization" runat="server" Text="Immn./Inj. Procedure*" 
                                                                Font-Bold="False" CssClass="Editabletxtbox" mand="Yes"></asp:Label>--%>
                                                            <span class="MandLabelstyle">Immn./Inj. Procedure</span><span class="manredforstar">*</span>
                                                        </td>
                                                        <td class="style12" colspan="3">
                                                            <telerik:RadTextBox ID="txtImmunizationProcedure" runat="server" Height="30px" Width="568px"
                                                                Skin="Office2010Black" Font-Bold="False" ReadOnly="true" BackColor="#BFDBFF"
                                                                BorderColor="Black" TextMode="MultiLine" CssClass="nonEditabletxtbox">
                                                                <DisabledStyle Resize="None" />
                                                                <InvalidStyle Resize="None" />
                                                                <HoveredStyle Resize="None" />
                                                                <ReadOnlyStyle Resize="None" />
                                                                <EmptyMessageStyle Resize="None" />
                                                                <FocusedStyle Resize="None" />
                                                                <EnabledStyle Resize="None" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style26">
                                                            <asp:Label ID="Label2" runat="server" Text="CVX Code" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style27">
                                                            <telerik:RadTextBox ID="txtCVXCode" runat="server" Height="25px" Skin="Office2010Black"
                                                                Font-Bold="False" ReadOnly="true" BackColor="#BFDBFF" BorderColor="Black" CssClass="nonEditabletxtbox">
                                                                <DisabledStyle Resize="None" />
                                                                <InvalidStyle Resize="None" />
                                                                <HoveredStyle Resize="None" />
                                                                <ReadOnlyStyle Resize="None" />
                                                                <EmptyMessageStyle Resize="None" />
                                                                <FocusedStyle Resize="None" />
                                                                <EnabledStyle Resize="None" />
                                                                <ClientEvents OnValueChanged="EnableSave" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td class="style28">
                                                            <asp:Label ID="lblRouteOfAdministration" runat="server" Text="Route of Administration"
                                                                Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style29">
                                                            <telerik:RadComboBox ID="cboRouteOfAdministration" runat="server" Font-Bold="False"
                                                                OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style30">
                                                            <asp:Label ID="lblVaccinationGivenOutside" runat="server" Text="Immn./Inj. In House"
                                                                Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style31">
                                                            <asp:CheckBox ID="chkYes" runat="server" Text="Yes" Font-Bold="False" onclick="DateVisible(this);" CssClass="Editabletxtbox"/>
                                                            <asp:CheckBox ID="chkNo" runat="server" Text="No" Font-Bold="False" onclick="DateVisible(this);" CssClass="Editabletxtbox"/>
                                                        </td>
                                                        <td class="style32">
                                                            <asp:Label ID="lblOrderDate" runat="server" Text="Order Date" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style33">
                                                            <telerik:RadDatePicker ID="dtpVisitDate" runat="server" EnableTyping="false" CssClass="Editabletxtbox">
                                                                <ClientEvents OnPopupOpening="EnableSave" />
                                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                </Calendar>
                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                    LabelWidth="40%" type="text" value="" CssClass="Editabletxtbox">
                                                                    <EmptyMessageStyle Resize="None" />
                                                                    <ReadOnlyStyle Resize="None" />
                                                                    <FocusedStyle Resize="None" />
                                                                    <DisabledStyle Resize="None" />
                                                                    <InvalidStyle Resize="None" />
                                                                    <HoveredStyle Resize="None" />
                                                                    <EnabledStyle Resize="None" />
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                      <tr>
                                                        <td class="style19">
                                                            <%--<asp:Label ID="lblDocumentType" runat="server" Text="Document Type*" Font-Bold="False" 
                                                                CssClass="Editabletxtbox" mand="Yes"></asp:Label>--%>
                                                            <span ID="lblDocumentType" class="MandLabelstyle">Document Type</span><span class="manredforstar">*</span>
                                                        </td>
                                                        <td class="style22">
                                                            <asp:Panel ID="pnlDLCDocumentType" runat="server" Height="100%" Width="100%" BackColor="White"
                                                                Font-Size="Small" Font-Bold="false" CssClass="Editabletxtbox">
                                                                <DLC:DLC ID="txtDLCDocumentType" TextboxHeight="30px" TextboxWidth="200px" runat="server" Value="IMMUNIZATION DOCUMENTATION TYPE NOTES" ListboxHeight="100px" />
                                                                <%--<asp:PlaceHolder ID="phlCheifComplaints" runat="server" EnableViewState="false" />--%>
                                                            </asp:Panel>
                                                        </td>
                                                       <td class="style38">
                                                            <asp:Label ID="lblVISGiven" runat="server" Text="VIS Given" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style39">
                                                            <asp:CheckBox ID="chkVisgiven" runat="server" Text=" " onclick="DateDisplay(this);" Checked="true" Enabled="false"  CssClass="Editabletxtbox"/>
                                                            <telerik:RadDatePicker ID="dtpVisGiven" runat="server" EnableTyping="false">
                                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                </Calendar>
                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                    LabelWidth="40%" type="text" value="" CssClass="Editabletxtbox">
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                      </tr>
                                                    <tr>
                                                        
                                                        <td class="style40">
                                                            <asp:Label ID="lblDateOnVis" runat="server" Text="Date on VIS" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style41">
                                                            <telerik:RadDatePicker ID="dtpDateOnVis" runat="server" EnableTyping="false" CssClass="Editabletxtbox">
                                                                <ClientEvents OnDateSelected="EnableSave" />
                                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                </Calendar>
                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                    LabelWidth="40%" type="text" value="" CssClass="Editabletxtbox">
                                                                    <EmptyMessageStyle Resize="None" />
                                                                    <ReadOnlyStyle Resize="None" />
                                                                    <FocusedStyle Resize="None" />
                                                                    <DisabledStyle Resize="None" />
                                                                    <InvalidStyle Resize="None" />
                                                                    <HoveredStyle Resize="None" />
                                                                    <EnabledStyle Resize="None" />
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                         <td>
                                                            <asp:Label ID="Label1" runat="server" Text="Immunization Evidence" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="cboImmunizationEve" runat="server" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style42">
                                                            <asp:Label ID="lblVFC" runat="server" Text="VFC Status" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style43">
                                                            <telerik:RadComboBox ID="cboVFC" runat="server" Font-Bold="False" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td class="style44" colspan="2">
                                                            <asp:CheckBox ID="chkAuthorizationRequired" runat="server" Text="Authorization Required"
                                                                Font-Bold="False" CssClass="Editabletxtbox"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style19">
                                                            <asp:Label ID="lblNotes" runat="server" Text="Notes" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style22">
                                                            <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                                                Font-Size="Small" Font-Bold="false" CssClass="Editabletxtbox">
                                                                <DLC:DLC ID="txtNotes" TextboxHeight="30px" TextboxWidth="200px" runat="server" Value="IMMUNIZATION NOTES" ListboxHeight="100px" />
                                                                <%--<asp:PlaceHolder ID="phlCheifComplaints" runat="server" EnableViewState="false" />--%>
                                                            </asp:Panel>
                                                        </td>
                                                       
                                                    </tr>
                                                   
                                                </table>
                                            </asp:Panel>
                                    </tr>
                                    <tr>
                                        <td class="style6" colspan="2" valign="top">
                                            <asp:Panel ID="pnlVaccineAdminDetails" runat="server" GroupingText="Immn./Inj. Administration Details"
                                                Height="208px" Width="100%" CssClass="LabelStyleBold" style="margin-top: -1.5%;">
                                                <table style="width: 100%; height: 246px;" id="TbDis">
                                                    <tr>
                                                        <td colspan="4">
                                                            <asp:Panel ID="Panel1" runat="server" Height="100%" Width="100%">
                                                                <table style="width: 100%; height: 100%;" id="Table1">
                                                                    <tr>
                                                                        <td width="24%" align="left">
                                                                            <asp:CheckBox ID="chkRefused" runat="server" Text="Refused Administration" onclick="checkRefused(this);"
                                                                                Font-Bold="False" CssClass="Editabletxtbox"/>
                                                                        </td>
                                                                        <td width="24%" align="left">
                                                                            <telerik:RadComboBox ID="cboRefused" runat="server" Font-Bold="False" Width="150px" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td width="4%"></td>
                                                                        <td width="24%">
                                                                            <asp:Label ID="Label3" runat="server" Text="VFC Eligibility status captured at" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                                        </td>
                                                                        <td width="24%">
                                                                            <telerik:RadComboBox ID="cboEligibility" runat="server" Font-Bold="False" Width="158px" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="24%">
                                                                            <asp:Label ID="Label4" runat="server" Text="Immunization Information Source" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                                        </td>
                                                                        <td width="24%">
                                                                            <telerik:RadComboBox ID="cboInformationSource" runat="server" Font-Bold="False" Width="150px" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                        <td width="4%"></td>
                                                                        <td width="24%">
                                                                            <asp:Label ID="Label5" runat="server" Text="Observation" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label>
                                                                        </td>
                                                                        <td width="24%">
                                                                            <telerik:RadComboBox ID="cboObservation" runat="server" Font-Bold="False" Width="158px" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style58">
                                                            <asp:Label ID="lblLotNumber" runat="server" Font-Bold="False" Text="Lot Number" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style51">
                                                            <telerik:RadTextBox ID="txtLotNumber" runat="server" ClientEvents-OnKeyPress="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td class="style56">
                                                            <asp:Label ID="lblManufacturer" runat="server" Font-Bold="False" Text="Manufacturer" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="cboManufacturer" runat="server" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style58">
                                                            <asp:Label ID="lblExpiryDate" runat="server" Font-Bold="False" Text="Expiration Date" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style51">
                                                            <telerik:RadDatePicker ID="dtpExpiryDate" runat="server" EnableTyping="false" CssClass="Editabletxtbox">
                                                                <ClientEvents OnDateSelected="EnableSave" />
                                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                </Calendar>
                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                    LabelWidth="40%" type="text" value="" CssClass="Editabletxtbox">
                                                                    <EmptyMessageStyle Resize="None" />
                                                                    <ReadOnlyStyle Resize="None" />
                                                                    <FocusedStyle Resize="None" />
                                                                    <DisabledStyle Resize="None" />
                                                                    <InvalidStyle Resize="None" />
                                                                    <HoveredStyle Resize="None" />
                                                                    <EnabledStyle Resize="None" />
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                        <td class="style56">
                                                            <asp:Label ID="lblImmunizationSource" runat="server" Font-Bold="False" Text=" Immn./Inj. Source" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="cboImmunizationSource" runat="server" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style58">
                                                            <asp:Label ID="lblAdminAmt" runat="server" Font-Bold="False" Text="Administered Amount" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style51">
                                                            <telerik:RadNumericTextBox ID="txtAdminAmt" runat="server" MaxLength="12" ClientEvents-OnKeyPress="EnableSave" CssClass="Editabletxtbox">
                                                                <NumberFormat ZeroPattern="n" DecimalDigits="2" DecimalSeparator="." KeepNotRoundedValue="True"
                                                                    KeepTrailingZerosOnFocus="True" />
                                                            </telerik:RadNumericTextBox>
                                                        </td>
                                                        <td class="style56">
                                                            <asp:Label ID="lblAdminUnit" runat="server" Font-Bold="False" Text=" Administered Unit" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="cboAdminUnit" runat="server" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style59">
                                                            <asp:Label ID="lblGivenBy" runat="server" Font-Bold="False" Text="Administered By" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style55">
                                                            <telerik:RadTextBox ID="txtGivenBy" runat="server" Height="25px" ClientEvents-OnKeyPress="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td class="style57">
                                                            <asp:Label ID="lblGivenDate" runat="server" Font-Bold="False" Text="Administered Date" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style29">
                                                            <telerik:RadDatePicker ID="dtpGivenDate" runat="server" EnableTyping="false" CssClass="Editabletxtbox">
                                                                <ClientEvents OnDateSelected="EnableSave" />
                                                                <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                                                </Calendar>
                                                                <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                                                <DateInput DateFormat="dd-MMM-yyyy" DisplayDateFormat="dd-MMM-yyyy" DisplayText=""
                                                                    LabelWidth="40%" type="text" value="" CssClass="Editabletxtbox">
                                                                    <EmptyMessageStyle Resize="None" />
                                                                    <ReadOnlyStyle Resize="None" />
                                                                    <FocusedStyle Resize="None" />
                                                                    <DisabledStyle Resize="None" />
                                                                    <InvalidStyle Resize="None" />
                                                                    <HoveredStyle Resize="None" />
                                                                    <EnabledStyle Resize="None" />
                                                                </DateInput>
                                                            </telerik:RadDatePicker>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style58">
                                                            <asp:Label ID="lblLocation" runat="server" Font-Bold="False" Text="Location" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style51">
                                                            <telerik:RadComboBox ID="cboLocation" runat="server" Font-Bold="False" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td class="style56">
                                                            <asp:Label ID="lblDose" runat="server" Font-Bold="False" Text="Dose# to Total Dose#" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadNumericTextBox ID="txtDose" runat="server" Height="20px" Width="65px" ClientEvents-OnKeyPress="EnableSave"
                                                                MaxLength="2" CssClass="Editabletxtbox">
                                                                <NumberFormat DecimalDigits="0" DecimalSeparator=" " KeepNotRoundedValue="True" KeepTrailingZerosOnFocus="True"
                                                                    ZeroPattern="n"/>
                                                            </telerik:RadNumericTextBox>
                                                            <asp:Label ID="lblDoseNo" runat="server" Height="20px" Text="&nbsp;&nbsp;&nbsp;/"
                                                                Width="20px"></asp:Label>
                                                            <telerik:RadComboBox ID="cboDoseno" runat="server" Height="80px" Width="67px" OnClientSelectedIndexChanged="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style58">
                                                            <asp:Label ID="lblNDC" runat="server" Font-Bold="False" Text="NDC" CssClass="Editabletxtbox"></asp:Label>
                                                        </td>
                                                        <td class="style51">
                                                            <telerik:RadTextBox ID="txtNDC" runat="server" ClientEvents-OnKeyPress="EnableSave" CssClass="Editabletxtbox">
                                                            </telerik:RadTextBox>
                                                        </td>
                                                        <td class="style56"></td>
                                                        <td></td>
                                                    </tr>

                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style10" valign="bottom">
                                            <telerik:RadButton ID="btnPlan" runat="server" Text="Plan" AccessKey="L" Style="display: none; top: 9px; left: 9px; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;">
                                                <ContentTemplate>
                                                    P<span class="underline">l</span>an
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                            <%--OnClientClicked="btnPlan_Clicked" OnClick="btnPlan_Click"--%>
                                            <telerik:RadButton ID="btnPrintVIS" runat="server" Text="Print VIS" OnClick="btnPrintVIS_Click" OnClientClicked="StartLoadFromPatChart"
                                                AccessKey="V" Style="top: -1px; left: 406px;padding: 4px 12px !important;
    font-size: 12px !important; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; height: 24px !important;" ButtonType="LinkButton"  Width="72px" CssClass="bluebutton teleriknormalbuttonstyle">
                                                <ContentTemplate>
                                                    Print <span class="">V</span>IS
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                        <td valign="bottom">
                                            <telerik:RadButton ID="btnTestArea" runat="server" Text="Test Area" Enabled="False" AccessKey="T"
                                                Style="text-align: center; top: 9px; left: 187px; -moz-border-radius: 3px; -webkit-border-radius: 3px; display: none;padding: 4px 12px !important;
    font-size: 12px !important; position: relative;"
                                                Width="60px">
                                                <ContentTemplate>
                                                    <span class="">T</span>est Area
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                            <%--OnClientClicked="btnTestArea_Clicked" OnClick="btnTestArea_Click"--%>
                                            <telerik:RadButton ID="btnAdd" OnClientClicked="btnAdd_Clicked" runat="server" Text="Add"
                                                OnClick="btnAdd_Click" AccessKey="A" Style="text-align: center; top: -1px; left: 244px; font-size: 13px !important;  height: 25px !important; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px !important;"
                                                ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle">
                                                <ContentTemplate>
                                                    <span id="SpanAdd" runat="server" class="">A</span><span id="SpanAdditionalword" runat="server">dd</span>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All" AutoPostBack="false"
                                                OnClientClicking="btnClearAll_Clicked" AccessKey="C"
                                                Style="text-align: center; -moz-border-radius: 3px; font-size: 13px !important;  height: 25px !important; -webkit-border-radius: 3px; position: relative; top: -1px; left: 244px;padding: 4px 12px !important;"
                                                Width="73px" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                                <ContentTemplate>
                                                    <span id="SpanClear" runat="server" class="">C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                            <telerik:RadButton ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" OnClientClicked="StartLoadFromPatChart"
                                                AccessKey="P" Style="text-align: center; top: -1px; left: 245px; -moz-border-radius: 3px; -webkit-border-radius: 3px;padding: 4px 12px !important;
    font-size: 12px !important; position: relative; height: 25px !important;" 
                                                Width="56px" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                                <ContentTemplate>
                                                    <span class="">P</span>rint
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4" colspan="3" valign="bottom" style="margin: 0px; padding: 0px">
                            <asp:Panel ID="pnlImmunizations" runat="server" GroupingText="Immn./Inj. " Height="280px"
                                Width="100%" Font-Bold="True" CssClass="LabelStyleBold">
                                <telerik:RadGrid ID="grdImmunizations" runat="server" CellSpacing="0" Height="250px"
                                    Style="margin-bottom: 0px" Font-Bold="false" AutoGenerateColumns="False"
                                    GridLines="Both" OnItemCommand="grdImmunizations_ItemCommand"
                                    Width="100%" OnItemCreated="grdImmunizations_ItemCreated" CssClass="Gridbodystyle">
                                    <%--OnNeedDataSource="grdImmunizations_NeedDataSource"--%>
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                    <HeaderStyle Font-Bold="true"  />
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="true" />
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                        <ClientEvents OnCommand="grdImmunizations_OnCommand" />
                                    </ClientSettings>
                                    <MasterTableView>
                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridButtonColumn FilterControlAltText="Filter column1 column" HeaderText="Edit"
                                                UniqueName="Edit" ButtonType="ImageButton" DataTextField="Edit" Text="Edit" ImageUrl="~/Resources/edit.gif">
                                                <HeaderStyle Width="40px" />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn FilterControlAltText="Filter column2 column" HeaderText="Del"
                                                UniqueName="Del" ButtonType="ImageButton" DataTextField="Del" ImageUrl="~/Resources/close_small_pressed.png"
                                                CommandName="Del" Text="Delete">
                                                <HeaderStyle Width="40px" />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column column" HeaderText="Immn./Inj. Procedure"
                                                UniqueName="Immn/InjProcedure" DataField="Immn./Inj. Procedure">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridDateTimeColumn FilterControlAltText="Filter column22 column" HeaderText="Administered Date"
                                                UniqueName="AdministeredDate" DataField="Administered Date" DataFormatString="{0:dd/MMM/yyyy}">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column4 column" HeaderText="CVX Code"
                                                UniqueName="column4" Display="false" DataField="CVX Code">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column5 column" HeaderText="Dose"
                                                UniqueName="column5" DataField="Dose">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column6 column" HeaderText="Dose No"
                                                UniqueName="column6" Display="false" DataField="Dose No">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column7 column" HeaderText="Given By"
                                                UniqueName="column7" Display="false" DataField="Given By">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridDateTimeColumn FilterControlAltText="Filter column8 column" HeaderText="Given Date"
                                                UniqueName="column8" Display="false" DataField="Given Date">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column9 column" HeaderText="Location"
                                                UniqueName="column9" Display="false" DataField="Location">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column10 column" HeaderText="Lot Number"
                                                UniqueName="column10" Display="false" DataField="Lot Number">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column11 column" HeaderText="Manufacturer"
                                                UniqueName="column11" Display="false" DataField="Manufacturer">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridDateTimeColumn FilterControlAltText="Filter column12 column" HeaderText="Date on Vis"
                                                UniqueName="column12" Display="false" DataField="Date on Vis">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridDateTimeColumn FilterControlAltText="Filter column13 column" HeaderText="Vis Given"
                                                UniqueName="column13" Display="false" DataField="Vis Given">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridDateTimeColumn FilterControlAltText="Filter column14 column" HeaderText="Expiry Date"
                                                UniqueName="column14" Display="false" DataField="Expiry Date">
                                            </telerik:GridDateTimeColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column15 column" HeaderText="Route of Administration"
                                                UniqueName="column15" DataField="Route of Administration">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column16 column" HeaderText="VFC"
                                                UniqueName="column16" Display="false" DataField="VFC">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column17 column" HeaderText="Id"
                                                UniqueName="Id" Display="false" DataField="Id">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column18 column" HeaderText="Immn./Inj. Source"
                                                UniqueName="column18" DataField="Immn./Inj. Source">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column19 column" HeaderText="NDC"
                                                UniqueName="column19" DataField="NDC">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn FilterControlAltText="Filter column20 column" HeaderText="Notes"
                                                UniqueName="column20" DataField="Notes">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridButtonColumn FilterControlAltText="Filter column21 column" HeaderText="View" Visible="false"
                                                UniqueName="View" DataTextField="View" ButtonType="ImageButton" CommandName="View"
                                                ImageUrl="~/Resources/Down.bmp">
                                                <HeaderStyle Width="40px" />
                                            </telerik:GridButtonColumn>
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
                        <td>
                            <asp:Panel ID="pnlMovetonextProcess" runat="server" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td align="right" valign="top">
                                            <telerik:RadButton ID="btnMoveToNextProcess" runat="server" OnClientClicked="StartLoadFromPatChart" Text="Move To Next Process"
                                                OnClick="btnMoveToNextProcess_Click" AccessKey="M" Style="-moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; " ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                                <ContentTemplate>
                                                    <span>M</span>ove To Next Process
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style9"></td>
                        <td></td>
                        <td>
                            <asp:Button ID="btnRefreshImm_proce" runat="server" Text="RefreshImm_proce" OnClick="btnRefreshImm_proce_Click" />
                            <asp:Button ID="btnInvisible" runat="server" Text="Invisible" OnClick="Button1_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; margin-top: 28px; height: 50px;">
                <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Immunization_Injection');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                </div>
                <div id="tag" style="margin-top: -13px; margin-left: -99px; font-size: 19px; height: 48px; width: 303px; font-weight: bold; color: #6504d0; border-radius: 7px; cursor: pointer; font-family: source sans pro;" onclick="OpenMeasurePopup('Immunization_Injection');">
                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                </div>
            </div>
            <%-- <div style="height: 40px;">
                <div style="width: 100%; margin-top: 3px; height: 50px;">
                    <div style="float: left; width: 48px; height: 47px; padding: 4px; color: white; text-align: center; margin-top: 10px;">
                        <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Immunization_Injection');" style="height: 60px; width: 58px; padding: 2px; margin-top: -11px; border-radius: 40px;" />
                    </div>
                    <div id="tag" style="width: 33%; padding: 4px; padding-left: 47px; font-size: 17px; height: 48px; width: 313px; margin-top: -6px; color: #00c4f7; border-radius: 7px; cursor: pointer; font-family: source sans pro;" onclick="OpenMeasurePopup('Immunization_Injection');">
                        Measure Booster<img src="Resources/measure_info.png" alt="" style="width:16px;height:15px;"/>
                    </div>
                </div>
            </div>--%>
            <asp:Button ID="btnClear" runat="server" CssClass="displayNone" OnClick="btnClear_Click" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" />
            <asp:HiddenField ID="hdnEncID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPhyId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnhumanid" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSelectedItem" runat="server" />
            <asp:HiddenField ID="hdnDOS" runat="server" />
            <asp:HiddenField ID="hdnGroupID" runat="server" />
            <asp:HiddenField ID="hdnCurrentProcess" runat="server" />
            <asp:HiddenField ID="SelectedItem" runat="server" />
            <asp:HiddenField ID="hdnLoad" runat="server" Value="false" />
            <asp:Button ID="InvisibleButton" Style="display: none;" OnClick="InvisibleButton_Click" runat="server" EnableViewState="false" />
        </telerik:RadAjaxPanel>
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
