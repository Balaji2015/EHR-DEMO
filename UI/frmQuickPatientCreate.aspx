 <%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmQuickPatientCreate.aspx.cs"
    Inherits="Acurus.Capella.UI.frmQuickPatientCreate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Quick Patient Create</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
        <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
        
    <style type="text/css">
        .Policypanel_with_padding {
        }

        table {
            border-spacing: 0;
            border-collapse: collapse;
        }

        td, th {
            padding: 0;
        }

        .table {
            border-collapse: collapse !important;
        }

        #Panel1 fieldset {
            height: 130px;
        }


        #gbEligibilityVerification fieldset {
            height: 425px;
        }

        .style218 {
            width: 8px;
        }

        .table-bordered td, .table-bordered th {
            border: 1px solid #ddd !important;
        }

        .Paymentpanel_with_padding {
            -webkit-padding-before: 65px;
            -webkit-padding-end: 0px;
            -webkit-padding-after: 0px;
            -webkit-padding-start: 0px;
            -moz-padding-end: 0px;
            -moz-padding: 65px;
            -moz-padding-bottom: 65px;
            -moz-padding-start: 0px;
        }

        .Panel legend {
            font-weight: bold !important;
        }

        .MultiLineTextBox {
            resize: none;
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

        .auto-style1 {
            width: 141px;
        }

        .tdwidth {
            width: 10%;
        }

        .tdwidth2 {
            width: 15%;
        }

        .span1 {
            margin-top: 10px;
        }
    </style>

    <%--<link href="CSS/style.css" rel="stylesheet" type="text/css" />
        <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
        <link href="CSS/jquery-ui.min.css" rel="stylesheet" />
          <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
        <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>--%>

</head>
<body onload="setCPtvalue();warningmethod();">
    <form id="frmQuickPatientCreate" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSendMail" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" />
            </Triggers>--%>
            <ContentTemplate>
                <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
                    <Windows>
                        <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                        </telerik:RadWindow>
                         <telerik:RadWindow ID="RadOnlineWindow" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
                <aspx:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnableViewState="false" AsyncPostBackTimeout="36000">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </aspx:ToolkitScriptManager>
                <div>
                    <asp:Label ID="lblSave" runat="server" Style="display: none"></asp:Label>
                    <table style="width: 100%" id="tbl" runat="server">
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="gbPatientInformation" runat="server" GroupingText="Patient Information"
                                    CssClass="LabelStyleBold">
                                    <table style="width: 100%;table-layout:fixed">
                                        <tr style="width:100%">
                                            <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblPatientAccountNo" class="Editabletxtbox" runat="server" Text="Acc. #"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:TextBox ID="txtPatientAccountNo" runat="server"  CssClass="nonEditabletxtbox"
                                                    BorderWidth="1px" MaxLength="20" Width="98%"></asp:TextBox>
                                            </td>
                                            <td style="padding-bottom:5px;" >
                                                <asp:Label ID="lblMedicalRecordNo" class="Editabletxtbox" runat="server" Text="Med Rec. #" style="margin-left:10px"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:TextBox ID="txtMedicalRecordNo" runat="server" class="Editabletxtbox" onkeypress="AutoSave(this);"
                                                    Width="116%" MaxLength="25"></asp:TextBox>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblExternalAccountNo" class="Editabletxtbox" runat="server" Text="Ext. Acc #" style="margin-left:30%"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;" rowspan="1">
                                                <asp:TextBox ID="txtExternalAccNo" runat="server" class="Editabletxtbox" onkeypress="AutoSave(this);" Width="156px"
                                                    MaxLength="15" style="margin-left: 10px; margin-bottom:-7px"></asp:TextBox>&nbsp;&nbsp;
                                            
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                 <asp:Button ID="btnEditName" runat="server" Text="Edit Name" OnClick="btnEditName_Click" Class="aspresizedbluebutton"
                                                 AccessKey="E" Width="100px" style=" margin-left:61px; margin-top:-10px "/>
                                            </td>
                                        </tr>
                                        <tr  style="width:100%">
                                            <td style="padding-bottom:5px;">
                                                <%-- <asp:Label ID="lblPatientLastName" mand="Yes" runat="server" Text="Last Name*"  
                                                    Width="75px"></asp:Label>--%>
                                                <span class="MandLabelstyle" runat="server">Last Name</span><span class="manredforstar">*</span>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:TextBox ID="txtPatientLastName" runat="server" onchange="AutoSave(this);" Width="98%"
                                                    onkeypress="AutoSave(this);" MaxLength="35"></asp:TextBox>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <%-- <asp:Label ID="lblPatientFirstName" mand="Yes" runat="server"  Text="First Name   *"></asp:Label>--%>
                                                <span class="MandLabelstyle" runat="server" style="margin-left:10px">First Name</span><span class="manredforstar">*</span>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:TextBox ID="txtPatientFirstName" runat="server" onkeypress="AutoSave(this);"
                                                    Width="116%" MaxLength="25"></asp:TextBox>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblPatientMI" runat="server" CssClass="Editabletxtbox" Text="Middle Name" style="margin-left:28%"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:Panel ID="Panel3" runat="server">
                                                    <table style="width: 70%;">
                                                        <tr>
                                                            <td style="width: 40%;padding-bottom:5px;">
                                                                <asp:TextBox ID="txtPatientMI" runat="server" onkeypress="AutoSave(this);" Width="105px" ReadOnly="false" 
                                                                    CssClass="Editabletxtbox" style="margin-top:-2px;margin-left:10px; width:80px; margin-right: 2px;" MaxLength="5"
                                                                   ></asp:TextBox>
                                                                <%-- MaxLength="25"--%>
                                                            </td>
                                                            <td style="width: 18%;padding-bottom:5px;">
                                                                <asp:Label ID="lblPatientSuffix" runat="server" CssClass="Editabletxtbox" Text="Suffix" style="margin-right: 10px;"
                                                                    EnableViewState="false"></asp:Label>
                                                            </td>
                                                            <td style="padding-bottom:5px;">
                                                                <asp:DropDownList ID="cboPatientSuffix" CssClass="Editabletxtbox" runat="server" onchange="AutoSave(this);" style="margin-left: -7px; margin-top: -1px;">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                       
                                             <td  style="width:10%;padding-bottom:5px;"; rowspan="5">
                                            <asp:Image ID="imgOverAllSummary" Width="139px" onmouseover="showtooltip(this)" Height="121px" ImageAlign="Top" runat="server" data-sqre="imgOverAllSummary_sqre" data-tooltp="imgOverAllSummary_tooltp"  
                                                 CssClass="displayInline boxModel DockImage" ImageUrl="~/Resources/person.gif" 
                                                style="margin-left:81px; margin-top:4px" />
                                                <%-- NCQADiabetes.gif--%>
                                     <%--style=" margin-left:-353px ;margin-bottom:-71px;" Width="212%"Height="138px" 97, 102 --%> 
                                  </td>
                                        
                                        </tr>
                                        <tr  style="width:100%">
                                            <td style="padding-bottom:5px;">
                                                <%--<asp:Label ID="lblPatientDOB" mand="Yes" runat="server" Text="DOB   * (Format: 01-Jan-1987)" ></asp:Label>--%>
                                                <span class="MandLabelstyle" runat="server">DOB</span><span class="manredforstar">*</span><span class="MandLabelstyle" runat="server">(Format: 01-Jan-1987)</span>
                                            </td>
                                            <td style="padding-bottom:5px;"> 
                                              <%--  <telerik:RadMaskedTextBox ID="dtpPatientDOB" runat="server" Mask="##-Lll-####"  Width="98%">
                                                    <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>--%>
                                                <telerik:RadMaskedTextBox ID="dtpPatientDOB" runat="server" Mask="##-Lll-####" Width="98%" >
                                                    <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <%-- <asp:Label ID="lblPatientSex" runat="server" mand="Yes" Text="Sex *"></asp:Label>--%>
                                                <span class="MandLabelstyle" runat="server" style="margin-left:10px">Sex</span><span class="manredforstar">*</span>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:DropDownList ID="cboPatientSex" runat="server" onchange="AutoSave(this);" Width="118%">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="padding-bottom:5px;"> 
                                                <asp:Label ID="lblCellPhno" CssClass="Editabletxtbox" runat="server" Text="Cell Ph#" style="margin-left:30%" Width="72px"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <telerik:RadMaskedTextBox ID="msktxtCellPhno" runat="server" onkeypress="AutoSave(this);"
                                                    autocomplete="off" Mask="(###) ###-####" Width="155px" style="margin-left:10px">
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            
                                             
                                            </td>
                                        </tr>
                                        <tr  style="width:100%">
                                            <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblHomePhno" CssClass="Editabletxtbox" runat="server" Text="Home Ph#"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <telerik:RadMaskedTextBox ID="msktxtHomePhno" CssClass="Editabletxtbox" runat="server" autocomplete="off" onkeypress="AutoSave(this);"
                                                    Mask="(###) ###-####" Width="98%">
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </t>
                                            <td style="padding-bottom:5px;">
                                                <%--<asp:Label ID="lblZip" runat="server" mand="Yes" Text="Zip Code  *"></asp:Label>--%>
                                                <span class="MandLabelstyle" runat="server" style="margin-left:10px">Zip Code</span><span class="manredforstar">*</span>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <telerik:RadMaskedTextBox ID="msktxtZipcode" autocomplete="off" runat="server" Mask="#####-####"
                                                    onkeypress="AutoSave(this);" Width="119%" >
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" CssClass="Editabletxtbox" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" CssClass="Editabletxtbox" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />

                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblPreferredConfidentialCorrespondenceMode" CssClass="Editabletxtbox" runat="server" Text="Pref.  Corres" style="margin-left:30%"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:DropDownList ID="cboPreferredConfidentialCoreespondenceMode" CssClass="Editabletxtbox" onchange="AutoSave(this);"
                                                    runat="server" Width="156px" style="margin-left:10px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                 <tr style="width: 100%;">

                                     <td style="padding-bottom:5px;"><%--<asp:Label ID="lblEMail"  runat="server" Text="E-Mail" mand="Yes"></asp:Label>--%><span id="spanemail" runat="server" class="spanstyle">E-Mail</span><span id="spanemailstar" runat="server" class="manredforstar" style="margin-top:-15px;margin-left:2px;" visible="false">*</span> </td>
                                     <td style="padding-bottom:5px;">
                                         <asp:TextBox ID="txtMail" runat="server" class="nonEditabletxtbox" Enabled="false" MaxLength="100" onkeypress="AutoSave(this);" onkeyup="" Width="98%"></asp:TextBox>
                                     </td>
                                     <td colspan="4" style="word-wrap:break-word;padding-bottom:5px;">
                                         <asp:CheckBox ID="chkOnlineAccess" runat="server" AutoPostBack="true" CssClass="chkitems" OnCheckedChanged="chkOnlineAccess_CheckedChanged" style="margin-left:7px;" Text="Enroll in Online Access" />
                                         <%-- onclick="EnableSendEmail(this);"--%>
                                         <asp:Button ID="btnSendMail" runat="server" AccessKey="M" CssClass="aspresizedbluebutton" OnClick="btnSendMail_Click" OnClientClick="showTime();" style="margin-left:-2px;font-size:11px !important;" Text="Send E-Mail" Width="19%" />
                                     </td>
                                     <td style="padding-bottom:5px;">
                                         <asp:Label ID="Label3" runat="server" CssClass="spanstyle" style="margin-left:-269%" Text="PatientPhoto"> </asp:Label>
                                         <asp:FileUpload ID="fileupload" runat="server" ClientIDMode="Static" onchange="this.form.submit()" style="margin-left:9%;display:none;" />
                                         <input type="button" id="btnUpload" runat="server" class="btn btn-primary btn-sm" value="Choose File" onclick="ClickUploadControl(); AutoSave(this);" style="margin-left:12px"/>
                                         <asp:Label ID="lblFileUpload" runat="server" Font-Bold="false"></asp:Label>
                                     </td>

                                 </tr>

                                        <tr  style="width:100%">
                                            <td  style="padding-bottom:5px;">
                                                <asp:Label ID="lblHumanType" runat="server" CssClass="Editabletxtbox" Text="Patient Type"></asp:Label>
                                            </td>

                                            <td style="padding-bottom:5px;"> 
                                                <asp:DropDownList ID="cboHumanType" runat="server" Width="98%">
                                                </asp:DropDownList>
                                            </td>
                                               <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblSSN" runat="server" CssClass="Editabletxtbox" Text="SSN" style="margin-left:10px"></asp:Label>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <telerik:RadMaskedTextBox ID="msktxtSSN" CssClass="Editabletxtbox" runat="server" Mask="###-##-####" autocomplete="off"
                                                    onkeypress="AutoSave(this);" Width="119%">
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td style="padding-bottom:5px;">
                                                <asp:Label ID="lblFileFormat" runat="server" Text="(File Format supported:*.jpg,*.png)" style="color:blue;margin-left:37px; white-space:nowrap"  CssClass="Editabletxtbox"></asp:Label>
<%--                                              <asp:Label ID="lblFileFormat" runat="server" style="color:blue;margin-left:37px" class="icon-ok-sign" Text="Formats supported:*.TIFF & *.PDF"></asp:Label>--%>
                                            </td>
                                        </tr>
                                    
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="gbExistingPolicies" runat="server" Width="975px" Height="130px" GroupingText="Existing Policies"
                                    CssClass="LabelStyleBold" ScrollBars="auto">
                                    <asp:GridView ID="grdExistingPolicies" runat="server" Width="1500px" EnableViewState="true" 
                                        CssClass="Gridbodystyle" EmptyDataText="No Records"
                                        CellPadding="3" AutoGenerateSelectButton="True"
                                        OnSelectedIndexChanged="grdExistingPolicies_SelectedIndexChanged" AutoGenerateColumns="False" HeaderStyle-CssClass="Gridheaderstyle header"
                                        OnRowCommand="grdExistingPolicies_RowCommand" OnRowDataBound="grdExistingPolicies_RowDataBound"  
                                        onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"> 

                                        <%-- onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"--%>
                                        <Columns>
                                            <asp:BoundField DataField="Policy Holder Id" HeaderText="Policy Holder Id" />
                                            <asp:BoundField DataField="Plan #" HeaderText="Plan #" />
                                            <asp:BoundField DataField="Plan Name" HeaderText="Plan Name" />
                                            <asp:BoundField DataField="Group #" HeaderText="Group #" />
                                            <asp:BoundField DataField="Ins. Type" HeaderText="Ins. Type" />
                                            <asp:BoundField DataField="Insured Party" HeaderText="Insured Party" />
                                            <asp:BoundField DataField="Relationship" HeaderText="Relationship" />
                                            <asp:BoundField DataField="Active" HeaderText="Active" />
                                            <asp:BoundField DataField="Eff. Start Date" HeaderText="Eff. Start Date" />
                                            <asp:BoundField DataField="Term. Date" HeaderText="Term. Date" />
                                            <asp:BoundField DataField="Carrier #" HeaderText="Carrier #" />
                                            <asp:BoundField DataField="Insured Name" HeaderText="Insured Name" />
                                            <asp:BoundField DataField="Insured #" HeaderText="Insured #" />
                                            <asp:BoundField DataField="SPC CoPay $" HeaderText="SPC CoPay $" />
                                            <asp:BoundField DataField="PCP CoPay $" HeaderText="PCP CoPay $" />
                                            <asp:BoundField DataField="Deduct. $" HeaderText="Deduct. $" />
                                            <asp:BoundField DataField="Deduct. Met $" HeaderText="Deduct. Met $" />
                                            <asp:BoundField DataField="Co ins. %" HeaderText="Co ins. %" />
                                            <asp:BoundField DataField="Eligibility_Type" HeaderText="Eligibility_Type" />
                                            <asp:ButtonField CommandName="SingleClick" Text="SingleClick" Visible="false" />
                                        </Columns>
                                        <SelectedRowStyle CssClass="aspSelectedRow" />
                                        <%--<SelectedRowStyle CssClass="gridSelectedRow" />--%>
                                        <%--<HeaderStyle CssClass="GridHeaderRow" />--%>
                                    </asp:GridView>
                                    <asp:HiddenField ID="hdnSelectedIndex" runat="server" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="pnlAuthorization" runat="server" GroupingText="Authorization Information**"
                                    BackColor="White" Height="80px" CssClass="LabelStyleBold">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label" CssClass="spanstyle" runat="server" Text="Carrier Name "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlauthPayer" CssClass="Editabletxtbox" runat="server" AutoPostBack="True" onchange="AutoSave(this);"
                                                    OnSelectedIndexChanged="ddlAuthourPayerName_SelectedIndexChanged" Width="160px">
                                                </asp:DropDownList>
                                                 <asp:CheckBox ID="chkShowAllAuth" runat="server" CssClass="chkitems" AutoPostBack="True" onclick="funEV();" 
                                                    OnCheckedChanged="chkShowAllAuth_CheckedChanged" Text="Show All" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" CssClass="Editabletxtbox" runat="server" Text="Ins. Plan Name"></asp:Label>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlauthinsplan" CssClass="Editabletxtbox" runat="server" AutoPostBack="True" onchange="AutoSave(this);"
                                                    Width="160px" OnSelectedIndexChanged="ddlAuthourPlanName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkShowAllAuthPlan" runat="server" CssClass="chkitems" AutoPostBack="true" onclick="funEV();"
                                                     Text="Show All" OnCheckedChanged="chkShowAllAuthPlan_CheckedChanged"/>

                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" CssClass="Editabletxtbox" Text="Authorization #"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtauthnumber" CssClass="Editabletxtbox" runat="server"
                                                    Width="160px" MaxLength="25" onkeypress="AutoSave(this);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label16" CssClass="Editabletxtbox" runat="server" Text="Valid from"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="txtauthValidfrom" CssClass="Editabletxtbox" runat="server" Mask="##-Lll-####"
                                                    Width="162px" autocomplete="off" style="margin-top: 4px;">

                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" CssClass="Editabletxtbox" Text="Valid To"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="txtauthvalidTo" CssClass="Editabletxtbox" runat="server" Mask="##-Lll-####"
                                                    Width="162px" style="margin-top: 4px;">

                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" CssClass="Editabletxtbox" Text="Test Appeared for"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txttestappear" runat="server"
                                                    Width="160px" Enabled="false" CssClass="nonEditabletxtbox"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>
                                <%--<div style="text-align: right;
    padding-right: 40px;font-size:12px;   font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif;"  >** Remaining CPT Qty with two or below are highlighted in red color</div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; height: 75%;">
                                <%-- <asp:Panel ID="gbPolicyInformation" runat="server" Font-Size="Small" GroupingText="Policy Information"--%>
                                <asp:Panel ID="gbEligibilityVerification" runat="server" Font-Size="Small" GroupingText="Eligibility Verification"
                                    CssClass="LabelStyleBold" >
                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="chkEligibilityVerified" runat="server" CssClass="chkitems" AutoPostBack="True" onclick="funEV();"
                                                    OnCheckedChanged="chkEligibilityVerified_CheckedChanged" Text="Eligibility Verified Manually" />
                                            </td>
                                          
                                             <%--<td>
                                                <asp:TextBox ID="txtEligibilityVerificationBy" runat="server" onkeypress="AutoSave(this);"
                                                    Width="160px" MaxLength="50" Visible="false"></asp:TextBox>
                                            </td>--%>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td>
                                                <a  href="" id="Viewpdf" runat="server" class="alinkstyle" style="cursor: pointer;text-decoration:underline;margin-left: 82px;">View Details</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 5px;">
                                                <asp:Label ID="lblVerificationType" CssClass="Editabletxtbox" runat="server" Text="EV Type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboVerificationType" runat="server" AutoPostBack="True" onchange="AutoSave(this);"
                                                    OnSelectedIndexChanged="cboVerificationType_SelectedIndexChanged" Width="162px">
                                                </asp:DropDownList>
                                            </td>
                                             <td>
                                                <asp:Label ID="lblEligibilityVerificationDate" CssClass="Editabletxtbox" runat="server" Text="EV Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEligibilityVerificationDate" runat="server" Width="160px"  Enabled="false"  style="margin-top: -5px"></asp:TextBox>
                                               <%-- <a  href="" id="Viewpdf" runat="server" class="alinkstyle" style="cursor: pointer;text-decoration:underline">View Details</a>--%>
                                            </td>
                                             <td>
                                                <asp:Label ID="lblClaimMailingName" CssClass="Editabletxtbox" runat="server" Text="Claim Mailing Name"></asp:Label>
                                            </td>
                                             <td>
                                                  <asp:TextBox ID="txtClaimMailingName" runat="server" Width="160px"  style="margin-top: -5px" onkeypress="AutoSave(this);"  CssClass="nonEditabletxtbox" ReadOnly="True"></asp:TextBox>
                                            </td>
                                         </tr>

                                        <tr>
                                             <td>
                                                <span class="spanstyle" runat="server" id="spanPayername">Carrier Name </span><span class="manredforstar" runat="server" id="spanPayerstar" visible="false">*</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPayerName" runat="server" AutoPostBack="True" onchange="Payer(this);" OnSelectedIndexChanged="ddlPayerName_SelectedIndexChanged"
                                                    Width="140px" style="margin-top: 4px;">
                                                </asp:DropDownList>
                                                  <asp:CheckBox ID="chkShowAllEV" runat="server" CssClass="chkitems" AutoPostBack="True" onclick="funEV();" 
                                                    OnCheckedChanged="chkShowAllEV_CheckedChanged"/>
                                                <asp:Label ID="lblShowAll" Text="Show All" runat="server" style="display:inline-block;width:21px;font-size: 11px;font-weight: 100;"/>
                                            </td>
                                             <td>
                                                  <span class="spanstyle" runat="server" id="spanEffectiveDate">Coverage Eff.Date</span><span class="manredforstar" runat="server" id="spanEffDatestar" visible="false">*</span>
                                            </td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="dtpEffectiveStartDate" runat="server" Mask="##-Lll-####" onkeypress="AutoSave(this);"
                                                    Width="166px" autocomplete="off" style="margin-top:2px">
                                                    <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" CssClass="Editabletxtbox" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" CssClass="Editabletxtbox" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStreet" runat="server" Text="PO Box / Street" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStreet" runat="server"  CssClass="Editabletxtbox" Width="160px" onkeypress="AutoSave(this);"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCity1" runat="server" CssClass="Editabletxtbox" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClaimCity" runat="server" Width="160px" CssClass="nonEditabletxtbox" ReadOnly="True" style="margin-top:0px" onkeypress="AutoSave(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTerminationDate" CssClass="Editabletxtbox" runat="server" Text="Coverage End Date"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="dtpTerminationDate" runat="server" Mask="##-Lll-####" onkeypress="AutoSave(this);"
                                                    Width="167px" style="margin-top:5px">
                                                    <ClientEvents OnValueChanged="QPCTermDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" CssClass="Editabletxtbox" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" CssClass="Editabletxtbox" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCity2" runat="server" CssClass="Editabletxtbox" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClaimCity2" runat="server" Width="160px" CssClass="nonEditabletxtbox" ReadOnly="True" onkeypress="AutoSave(this);" style="margin-top:0px"></asp:TextBox>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td>
                                                <span class="spanstyle" runat="server" id="spanPolicyID">Policy Holder ID</span><span class="manredforstar" runat="server" id="spanPolicyIDstar" visible="false">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPolicyHolderID" runat="server" CssClass="Editabletxtbox" Width="160px" onkeypress="AutoSave(this);"
                                                    MaxLength="20" style="margin-top: -2px" ReadOnly="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInsuranceType" runat="server" Text="Plan Type" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td>
                                                 <asp:TextBox ID="txtInsurancetype" runat="server"  Width="160px" CssClass="Editabletxtbox" onkeypress="AutoSave(this);" style="margin-top:4px;"  MaxLength="85"> <%-- onSelectedValueChanged="ddlPayerName_SelectedIndexChanged"--%>
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblState" runat="server" CssClass="Editabletxtbox" Text="State"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClaimState" runat="server" Width="160px" CssClass="nonEditabletxtbox" onkeypress="AutoSave(this);"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPlanNumber" runat="server" CssClass="Editabletxtbox" Text="Plan Number"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPlanNumber" runat="server" CssClass="Editabletxtbox" Width="160px" onkeypress="return onlyNumbers(event)" MaxLength="10"
                                                     onpaste="return false"></asp:TextBox>
                                            </td>
                                              <td>
                                                <span class="spanstyle" runat="server" id="spanPlanName">Ins. Plan Name</span><span class="manredforstar" runat="server" id="spanPlanstar" visible="false">*</span>
                                            </td>
                                              <td>
                                                <asp:DropDownList ID="ddlPlanName" runat="server" AutoPostBack="True" onchange="AutoSave(this);"
                                                     Width="140px" style="margin-top: 4px;" OnSelectedIndexChanged="ddlPlanName_SelectedIndexChanged" >
                                                 </asp:DropDownList>

                                                   <asp:CheckBox ID="chkShowAllEVPlan" runat="server" CssClass="chkitems" AutoPostBack="True"
                                                        onclick="funEV();" OnCheckedChanged="chkShowAllEVPlan_CheckedChanged" /> <%--OnCheckedChanged="chkShowAllEVPlan_CheckedChanged"--%>
                                                <asp:Label ID="lblShowAllEVPlan" Text="Show All" runat="server" style="display:inline-block;width:21px;font-size: 11px;font-weight: 100;"/>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblOrganization" runat="server" CssClass="Editabletxtbox" Text="Organization"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOrganization" runat="server" Width="160px" CssClass="Editabletxtbox" onkeypress="AutoSave(this);" style="margin-top:4px;"  MaxLength="75"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSubscriberName" runat="server" CssClass="Editabletxtbox" Text="Subscriber Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSubscriberName" runat="server" CssClass="Editabletxtbox" onkeypress="AutoSave(this);" Width="160px" MaxLength="75"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPCPName" runat="server" CssClass="Editabletxtbox" Text="PCP Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPCPName" runat="server" CssClass="Editabletxtbox" Width="160px" onkeypress="AutoSave(this);" style="margin-top: 3px;"  MaxLength="75"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStatus" CssClass="Editabletxtbox" runat="server" Text="Status"></asp:Label>
                                            </td>
                                            <td rowspan="2">
                                               <%-- <asp:TextBox ID="txtStaus" runat="server" CssClass="Editabletxtbox" Width="160px"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtErrorMessage" runat="server" onkeypress="AutoSave(this);" BackColor="#BFDBFF"
                                                    Width="160px" TextMode="MultiLine" MaxLength="9" ReadOnly="true"  style="resize:none; margin-top: -3px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRelationship" runat="server" CssClass="Editabletxtbox" Text="Relationship to Sub"></asp:Label> 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRelationship" runat="server" CssClass="Editabletxtbox" onkeypress="AutoSave(this);" Width="160px"  MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPCP_NPI" runat="server" CssClass="Editabletxtbox" Text="PCP NPI"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPCP_NPI" runat="server" CssClass="Editabletxtbox" Width="160px" onkeypress="return onlyNumbers(event)" MaxLength="10"
                                                    onpaste="return false"  style="margin-top: 3px;"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                           <td>
                                                <asp:Label ID="lblGroupNumber" CssClass="Editabletxtbox" runat="server" Text="Group #"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGroupNumber" runat="server" OnKeyPress="AutoSave(this);" Width="160px"
                                                    MaxLength="25" style="margin-top:4px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPCP_EffDate" CssClass="Editabletxtbox" runat="server" Text="PCP Eff.Date"></asp:Label>
                                            </td>
                                            <td>
                                              <telerik:RadMaskedTextBox ID="dtpPCPEffectiveDate" runat="server" Mask="##-Lll-####" onkeypress="AutoSave(this);"
                                                    Width="166px" style="margin-top:5px">
                                                    <ClientEvents OnValueChanged="QPCTermDateValidation"  />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White"  CssClass="Editabletxtbox" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None"  CssClass="Editabletxtbox"/> 
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                           <td>
                                                <asp:Label ID="lblDemoNote" CssClass="Editabletxtbox" runat="server" Text="Comments" ></asp:Label>
                                            </td>
                                            <td rowspan="2">
                                                <asp:TextBox ID="txtDemoNote" runat="server"   onkeypress="AutoSave(this);"
                                                    TextMode="MultiLine" CssClass="Editabletxtbox" Width="160px" MaxLength="255"  style="margin-top:-7px; resize: none;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <asp:Label ID="lblGroupName" CssClass="Editabletxtbox" runat="server" Text="Group Name"></asp:Label>
                                            </td>
                                            <td>
                                                   <asp:TextBox ID="txtGroupName" CssClass="Editabletxtbox" runat="server" Width="160px" onkeypress="AutoSave(this);" style="margin-top: 3px;"  MaxLength="100"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIPAName" runat="server" CssClass="Editabletxtbox" Text="IPA Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIPAName" runat="server" onkeypress="AutoSave(this);" CssClass="Editabletxtbox" Width="160px" 
                                                    style="margin-top: 5px;"  MaxLength="75"></asp:TextBox>
                                            </td>
                                        </tr>

                                         <tr>
                                            <td colspan="6">
                                                <table style="width: 100%;">
                                                    <tr >
                                                        <td style="width: 19%;"></td>
                                                        <td style="width: 16%;"></td>
                                                        <td style="width: 18%;"></td>
                                                        <td style="width: 13%;"></td>
                                                        <td style="width: 20%;"></td>
                                                        <td style="width: 20%;"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td colspan="6">
                                                <table style="width: 100%;">
                                                    <tr >
                                                        <td style="width: 19%;"></td>
                                                        <td style="width: 16%;">In-Network</td>
                                                        <td style="width: 18%;">Out-of-Network</td>
                                                        <td style="width: 13%;"></td>
                                                        <td style="width: 20%;">In-Network</td>
                                                        <td style="width: 20%;">Out-of-Network</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                       
                                        <tr>
                                            <td colspan="6">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                     <td style="width:20%; padding: 7px;">
                                                                        <asp:Label ID="lblempty" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%;">
                                                                         <asp:Label ID="lblPCPOfficevisit" runat="server" CssClass="Editabletxtbox" Text="PCP Office Visit" style="margin-left: -10%;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                     <td style="width:20%; padding: 2%;">
                                                                        <asp:Label ID="lblspace" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                     <td style="width:20%;padding: 4%;"> 
                                                                        <asp:Label ID="lblSpecialityVisit" runat="server" CssClass="Editabletxtbox" Text="Splty Office Visit" style="margin-left: -13px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                     <td style="width:20%; padding: -7px;height: -1px;">
                                                                        <asp:Label ID="lblspace2" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                     <td style="width:20%;padding: 6%;">
                                                                        <asp:Label ID="lblMedication" runat="server" CssClass="Editabletxtbox" Text="Inj./ Medication" style="margin-left: -13px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%;padding: -23%; ">
                                                                        <asp:Label ID="lblUrgentCare" runat="server" CssClass="Editabletxtbox" Text="Urgent Care" style="margin-left: -8px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%;padding: 9px;">  
                                                                        <asp:Label ID="lblMessage" runat="server" CssClass="Editabletxtbox" Text="Message" style="margin-left: -16px; padding-top: 23%;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                         <td>
                                                             <table style="border:ridge;"> <%--margin-top: -45px;--%>
                                                                 <thead>
                                                                     <tr>
                                                                         <th>CoPay</th>
                                                                         <th>Co.Ins</th>
                                                                     </tr>
                                                                 </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPCPVisitInCopay" runat="server" CssClass="nonEditabletxtbox" Width="85px" onkeypress="return onlyDotsAndNumbers(event)"
                                                                                 onpaste="return false"  style="margin-left:2px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPCPVisitInCoIns" runat="server" CssClass="nonEditabletxtbox" Width="85px"
                                                                                onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtSpecialityVisitInCopay" runat="server" CssClass="nonEditabletxtbox" Width="85px" 
                                                                                 onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px; margin-top: 3px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                              <asp:TextBox ID="txtSpecialityVisitInCoIns" runat="server" CssClass="nonEditabletxtbox" Width="85px" 
                                                                                  onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px; margin-top: 3px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtMedicationInCopay" runat="server" CssClass="nonEditabletxtbox" Width="85px" 
                                                                                 onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px;margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMedicationInCoIns" runat="server" CssClass="nonEditabletxtbox" Width="85px" 
                                                                                onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px;margin-top: 3px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtUrgentCareInCopay" runat="server" CssClass="nonEditabletxtbox" Width="85px" 
                                                                                onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px; margin-top: 3px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtUrgentCareCoInIns" runat="server" CssClass="nonEditabletxtbox" Width="85px" 
                                                                                onpaste="return false"  onkeypress="return onlyDotsAndNumbers(event)" style="margin-left:2px; margin-top: 3px;" MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtPCPInNetworkMessage" runat="server" CssClass="nonEditabletxtbox" Width="174px" style="margin-left:2px;  margin-top: 3px; resize: none;"
                                                                                TextMode="MultiLine" MaxLength="100" OnKeyPress="AutoSave(this);"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                             </table>
                                                        </td>

                                                         <td>
                                                             <table style="border: ridge;"> <%-- margin-top: -46px;--%>
                                                                 <thead>
                                                                     <tr>
                                                                         <th>CoPay</th>
                                                                         <th>Co.Ins</th>
                                                                     </tr>
                                                                 </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPCPVisitOutCopay" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)"
                                                                                onpaste="return false"  Width="85px" style="margin-left:2px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPCPVisitOutCoIns" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" 
                                                                                onpaste="return false"  Width="85px" style="margin-left:2px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtSpecialityVisitOutCopay" runat="server" CssClass="nonEditabletxtbox"  onkeypress="return onlyDotsAndNumbers(event)" 
                                                                                 onpaste="return false"  Width="85px" style="margin-left:2px; margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                              <asp:TextBox ID="txtSpecialityVisitOutCoIns" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)"
                                                                                  onpaste="return false"  Width="85px" style="margin-left:2px; margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtMedicationOutCopay" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" 
                                                                                 onpaste="return false"  Width="85px" style="margin-left:2px;margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMedicationOutCoIns" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" 
                                                                                onpaste="return false"  Width="85px" style="margin-left:2px;margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtUrgentCareOutCopay" runat="server" CssClass="nonEditabletxtbox"  onkeypress="return onlyDotsAndNumbers(event)" 
                                                                                onpaste="return false"  Width="85px" style="margin-left:2px;margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtUrgentCareOutCoIns" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" style="margin-left:2px; margin-top: 3px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtPCPOutNetworkMessage" runat="server" CssClass="nonEditabletxtbox" Width="174px" style="margin-left:2px; margin-top: 3px; resize: none;"
                                                                                TextMode="MultiLine" MaxLength="100" OnKeyPress="AutoSave(this);"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                             </table>
                                                        </td>

                                                         <td>
                                                             <table style="margin-left: -5px;">  <%--style="margin-top: -48px;"--%>
                                                               <tr>
                                                                    <td style="width:20%; padding: 3px;">
                                                                        <asp:Label ID="lblempty2" runat="server" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%; padding: -4px;">
                                                                        <asp:Label ID="lblIndperPlan" runat="server" CssClass="Editabletxtbox"  Text="Ind.per Plan$" style="margin-left:10px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%; padding: 5px;">
                                                                        <asp:Label ID="lblIndMet" runat="server" CssClass="Editabletxtbox" Text="Ind. met $" style="margin-left:5px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%; padding: 4px;">
                                                                        <asp:Label ID="lblFamilyperPlan" runat="server" CssClass="Editabletxtbox" Text="Fam.perplan$"  style="margin-left:5px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%; padding: 4px;">
                                                                        <asp:Label ID="lblFamilyMet" runat="server" CssClass="Editabletxtbox" Text="Fam.met $" style="margin-left:5px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:20%; padding: 3px;">
                                                                        <asp:Label ID="lblMessage2" runat="server" CssClass="Editabletxtbox" Text="Message" style="margin-left:5px;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                         <td>
                                                              <table style="border: ridge;"> <%-- margin-top: -40px;--%>
                                                                 <thead>
                                                                     <tr>
                                                                         <th>Deductible</th>
                                                                         <th>Out-of-Pocket</th>
                                                                     </tr>
                                                                 </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInDeductiblePlan" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" style="margin-left:2px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInPockot" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" style="margin-left:2px;"  MaxLength="9"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtInDeductiblemet" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px"  MaxLength="9" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                             <asp:TextBox ID="txtInpocketmet" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" MaxLength="9" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtInFamilyDeductible" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" MaxLength="9" style="margin-left:2px;margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInFamilypocket" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" MaxLength="9" style="margin-left:2px;margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                           <asp:TextBox ID="txtInFamilyDeductiblemet" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" MaxLength="9" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtInFamilymetpocket" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px" MaxLength="9" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtDeductibleInNetworkMessage" runat="server" CssClass="nonEditabletxtbox" Width="174px" style="margin-left:2px; margin-top: 3px; resize: none;"
                                                                                TextMode="MultiLine" MaxLength="100" OnKeyPress="AutoSave(this);"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                             </table>
                                                        </td>

                                                         <td>
                                                               <table style="border: ridge;"><%--  margin-top: -36px;--%>
                                                                 <thead>
                                                                     <tr>
                                                                         <th>Deductible</th>
                                                                         <th>Out-of-Pocket</th>
                                                                     </tr>
                                                                 </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOutDeductiblePlan" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event);" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOutPocket" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtOutDeductiblemet" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOutpocketmet" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                             <asp:TextBox ID="txtOutFamilyDeductible" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px;margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOutFamilypocket" runat="server" CssClass="nonEditabletxtbox"  onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px;margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                           <asp:TextBox ID="txtOutFamilyDeductiblemet" runat="server" CssClass="nonEditabletxtbox"  onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  Width="85px"  MaxLength="9" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtOutFamilymetpocket" runat="server" CssClass="nonEditabletxtbox" onkeypress="return onlyDotsAndNumbers(event)" onpaste="return false"  MaxLength="9" Width="85px" style="margin-left:2px; margin-top: 3px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtDeductibleOutNetworkMessage" runat="server" CssClass="nonEditabletxtbox" Width="174px" OnKeyPress="AutoSave(this);"
                                                                                TextMode="MultiLine" MaxLength="100" style="margin-left:2px; margin-top: 3px; resize: none;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                             </table>
                                                        </td>


                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        </table>
                                       </asp:Panel>
                                     
                                    </table>
                                <%--</asp:Panel>--%>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 100%">
                                <asp:Panel ID="gbPaymentInformation" runat="server" ScrollBars="Auto" Font-Size="Small"
                                    GroupingText="Payment Information" CssClass="LabelStyleBold">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <span class="MandLabelstyle" runat="server" id="spanpayment">Method of Payment</span><span class="manredforstar" runat="server" id="spanpayment2">*</span>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboMethodOfPayment" onchange="AutoSave();" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cboMethodOfPayment_SelectedIndexChanged" Width="124px">
                                                </asp:DropDownList>
                                            </td>
                                        
                                            <td>
                                         <span class="spanstyle" runat="server" id="spanCheck" style="margin-left:12px;">Check # \Last 4 Digits of Card #</span><span class="manredforstar" runat="server" id="spanCheckStar" visible="false">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCheckNo" runat="server" Width="120px" OnKeyPress="return isNumberKey(this);"
                                                    MaxLength="4" style="margin-left: 3px; margin-right: -1px;"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAuthNo" CssClass="Editabletxtbox" runat="server" Text="CC Auth#" style="margin-left: 15px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAuthNo" runat="server" Width="120px" OnKeyPress="return isNumberKey(this);"
                                                    MaxLength="25" style="margin-left: 15px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <asp:Label ID="lblPaymentAmount" CssClass="Editabletxtbox" runat="server" Text="Copay $  "
                                                    Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPaymentAmount" onchange="CalAmt(this);" OnKeyPress="return AllowAmount(this);"
                                                    runat="server" Width="120px" MaxLength="7" Text="0.00" onblur="DefaultCopay()" style="margin-top:4px"></asp:TextBox>
                                            </td>
                                           <%-- <td> 
                                                 &nbsp;
                                            </td>--%>
                                            <td>
                                                <asp:Label ID="lblRecOnAcc" CssClass="Editabletxtbox" runat="server" Text="Rec'd on Acc $  "
                                                    Width="100px" style="margin-top:-1px; margin-left: 13px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRecOnAcc" runat="server" OnKeyPress="return AllowAmount(this);" onchange="CalAmt(this);" MaxLength="7" Text="0.00" onblur="DefaultCopayforrec()" Width="120px"
                                                    style="margin-top:2px; margin-left:3px;"></asp:TextBox>
                                            </td>

                                            <td>
                                                <asp:Label ID="lblPastDue" CssClass="Editabletxtbox" runat="server" Text="Past Due $" style="margin-top:12px; margin-left: 13px;"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPastDue" onchange="AutoSave();" OnKeyPress="return AllowAmount(this);"
                                                    runat="server" Width="120px" CssClass="nonEditabletxtbox" BorderWidth="1px" ReadOnly="True" style="margin-top:2px; margin-left: 15px;"></asp:TextBox>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRefundAmount" CssClass="Editabletxtbox" Text="Refund  Amt. $" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRefundAmount" Width="120px" onchange="CalAmt(this);" OnKeyPress="return AllowAmount(this);" onblur="DefaultCopayforrefund()" runat="server" MaxLength="7" Text="0.00"
                                                    style="margin-top:-8px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCheckDate" CssClass="Editabletxtbox" runat="server" Text="Check Date" Width="100px" style="margin-top:-12px; margin-left: 12px;"></asp:Label>
                                            </td>
                                            <td>
                                                <telerik:RadMaskedTextBox ID="dtpCheckDate" runat="server" Mask="##-Lll-####" Width="122px" style="margin-top:-9px; margin-left: 2px;">
                                                    <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" CssClass="Editabletxtbox"/>
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" CssClass="Editabletxtbox"/>
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                          <td>
                                             <span id="spanPaymentNotes" runat="server" class="spanstyle" style="margin-top: 10px; margin-left: 12px;">Payment Note</span><span id="spanPatientNotestar" runat="server" class="manredforstar" style="margin-top:-15px;margin-left:2px;" visible="false">*</span> </td>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtPaymentNote" runat="server" onchange="AutoSave();" Style="margin-left: 15px;margin-top:3px; resize:none;"
                                                    CssClass="MultiLineTextBox" TextMode="MultiLine" Width="244px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>
                                                <span class="spanstyle" runat="server" id="spanRelation">Relationship</span><span class="manredforstar" runat="server" id="spanRelationstar" visible="false">*</span>

                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboRelation" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="cboRelation_SelectedIndexChanged" style="margin-top:-8px">
                                                </asp:DropDownList>
                                            </td>

                                            <td>
                                                <span class="spanstyle span1" runat="server" id="spanPaidBy" style="margin-top: 10px; margin-left: 13px;">Paid By</span><span class="manredforstar" runat="server" id="spanPaidStar" visible="false">*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtpaidBy" runat="server" style="margin-top:-8px; width: 120px; margin-left: 2px;"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td style="width: 100%" colspan="7">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 70%">
                                                            <asp:Label ID="lblPaymentInCollection" runat="server" CssClass="Editabletxtbox" Text="Patient In Collection : "></asp:Label>
                                                            &nbsp;&nbsp;
                                                        <asp:Label ID="lblDeclaredBankruptcy" runat="server" CssClass="Editabletxtbox" Text="Declared Bankruptcy  :"></asp:Label>
                                                            &nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkMultiplePayments" runat="server" onclick="CheckMultiPayment();"
                                                            Text="Multiple Payment Mode" Visible="False" />
                                                        </td>
                                                        <td align="right" style="width: 30%">
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="aspresizedgreenbutton" OnClick="btnAdd_Click" AccessKey="A" Text="Add"
                                                                Width="100px" style="margin-top:-10px"/>
                                                            &nbsp;&nbsp;
                                                        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="aspresizedredbutton" OnClientClick="return ShowMessage();"
                                                            AccessKey="C" Text="Clear All" Width="100px" style="margin-top:-9px"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="width: 100%">
                                                <asp:GridView ID="grdPaymentInformation" runat="server" AutoGenerateColumns="False" CellPadding="3" CssClass="Gridbodystyle" EmptyDataText="No Records Found" HeaderStyle-CssClass="Gridheaderstyle header"
                                                      style="margin-top: 6px;" OnRowCommand="grdPaymentInformation_RowCommand" Width="945px" ><%-- Width="870px"--%>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="3%">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="EditGridRow" runat="server" CommandName="EditC" ImageUrl="~/Resources/edit.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Del">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="DeleteGridRow" runat="server" CommandName="DeleteRow" ImageUrl="~/Resources/close_small_pressed.png" ToolTip="Delete"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField AccessibleHeaderText="MethodofPayment" DataField="MethodofPayment" HeaderText="Method of Payment">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="CheckCardNo" DataField="CheckCardNo" HeaderText="Check /Card #">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="AuthNo" DataField="AuthNo" HeaderText="Confirmation ID">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="PastDue" DataField="PastDue" HeaderText="Past Due $">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="PatientPayment" DataField="PatientPayment" HeaderText="Copay $">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="RecOnAcc" DataField="RecOnAcc" HeaderText="Rec'd on Acc. $">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="RefundAmount" DataField="RefundAmount" HeaderText="Refund Amt. $">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="CheckDate" DataField="CheckDate" HeaderText="Check Date">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="PaymentNotes" DataField="PaymentNotes" HeaderStyle-CssClass="displayNone" HeaderText="PaymentNotes" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="VisitID" DataField="VisitID" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="PPHeaderID" DataField="PPHeaderID" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="PPLineID" DataField="PPLineID" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="CheckID" DataField="CheckID" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="Realationship" DataField="relationship" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField AccessibleHeaderText="Amount Paid By" DataField="Amtpaidby" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                         <asp:BoundField AccessibleHeaderText="Receipt Date" DataField="receiptdate" HeaderText="Receipt Date" HeaderStyle-CssClass="displayNone" ItemStyle-CssClass="displayNone">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                          <asp:BoundField AccessibleHeaderText="PaymentNote" DataField="PaymentNote" HeaderText="Payment Note">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                          <asp:BoundField AccessibleHeaderText="TransactionDate&Time" DataField="TransactionDate&Time" HeaderText="Transaction Date & Time">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                                    <%--<HeaderStyle CssClass="GridHeaderRow" />--%>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTotalAmount" CssClass="Editabletxtbox" runat="server" Text="Total Payment $"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="nonEditabletxtbox" ReadOnly="true" Width="150px" onchange="return AllowAmount(this);" style="margin-top: 7px;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <asp:HiddenField ID="hdnTotalPayment" runat="server" />
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                  <table>
                                    <tr>
                                          <td style="width:1%"> <%--align="right" style="width: 100%"--%>
                                <asp:Label ID="lblAuthTip" runat="server" Style="display: none!important; font-family: Helvetica Neue,Helvetica,Arial,sans-serif !important; font-size: 11px !important; font-weight: bold; font-style: italic;">** Remaining CPT Qty with two or below are highlighted in red color</asp:Label>&nbsp;&nbsp;
                                <asp:Label ID="lblLoad" runat="server" Style="display: none"></asp:Label>&nbsp;&nbsp;
                            </td>   
                            <td style="width:1%">                                
                                <asp:Button ID="btnAuthorization" runat="server" Text="Capture Authorization" Width="150px" Class="aspresizedbluebutton"
                                    OnClientClick="return OpenAuthorization();" Style="display: none!important" />&nbsp;&nbsp;
                            </td>
                            <td style="width:1%">
                                <asp:UpdatePanel runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                </Triggers>
                                </asp:UpdatePanel>
                             </td>
                            <td style="width:1%">                            
                                <asp:Button ID="btnCheckDuplicate" runat="server" OnClick="btnCheckDuplicate_Click"
                                Text="Button" Style="display: none" />
                            </td>
                            <td style="width:1%;"> <%--text-align:right;--%>
                                 <asp:Button ID="Button2" runat="server" Text="RefreshButton"  Class="aspresizedbluebutton" Width="28px"
                                   OnClick="Button2_Click1" style="margin-top: 3px;visibility:hidden;display:none" />&nbsp;&nbsp; <%--Width="120px"--%>
                            </td>

                            <td style="width:24%;text-align:right;">
                               <asp:Button ID="Button1" runat="server" Text="Perform EV" Width="120px" Class="aspresizedbluebutton"
                                    OnClientClick="return OpenEV();" style="margin-top: -2%; margin-right: -1%; " Visible="false" />&nbsp;&nbsp;
                            </td>
                                       
                            <td align="right" style="width: 2%"> 
                            <asp:Button ID="btnViewUpdateInsurance" runat="server" AccessKey="U"  CssClass="aspresizedbluebutton"    OnClientClick="return openPatInsurancewindowScreen();"                        
                                  Text="Patient Demographics"  Width="158px" style="margin-top: -3%;" />
                             </td>
                          
                            <td style="width:2%;text-align:right;">                                 
                                 <asp:Button ID="btnUploadDocuments" runat="server" Text="Upload Documents" Width="140px" Class="aspresizedbluebutton"
                                     OnClientClick ="return OpenUploadDocuments();" style="margin-top: 7%; margin-left: 3%;" />&nbsp;&nbsp;
                            </td>   
                            <td style="width:10%;text-align:right;">                                
                                <asp:Button ID="btnSave" runat="server" Text="Check In to MA" Width="94%" Class="aspresizedgreenbutton" OnClientClick="return ValidatePatientInformation();"
                                    AccessKey="S" OnClick="btnSave_Click" Visible="false" style="margin-top:-6%;margin-right: -3%; " />&nbsp;&nbsp;
                            </td>
                            <td style="width:1%;text-align:right;">
                                 <asp:Button ID="btnQuit" runat="server" Text="Close" Width="100px" margin-left="2%" OnClientClick="return CloseWindow();" Class="aspresizedredbutton"
                                AccessKey="Q" style="margin-top:8%"/>&nbsp;&nbsp;
                             </td> 
                            
                            <%-- <td style="width:24%;text-align:right;">
                                 <asp:Button ID="Button1" runat="server" Text="Perform EV" Width="120px" Class="aspresizedbluebutton"
                                    OnClientClick="return OpenEV();" style="margin-top: 4%; " />&nbsp;&nbsp;
                            </td>
                                       
                            <td align="left" style="width: 2%"> 
                           <asp:Button ID="btnViewUpdateInsurance" runat="server" AccessKey="U"  CssClass="aspresizedbluebutton"    OnClientClick="return openPatInsurancewindowScreen();"                        
                                  Text="View /Update Insurance Policies"  Width="220px" style="margin-top: 4%;" />
                             </td>
                          
                            <td style="width:2%;text-align:right;">                                 
                                 <asp:Button ID="btnUploadDocuments" runat="server" Text="Upload Documents" Width="140px" Class="aspresizedbluebutton"
                                     OnClientClick ="return OpenUploadDocuments();" style="margin-top: 6%;" />&nbsp;&nbsp;
                             </td>   
                            <td style="width:10%;text-align:right;">                                
                                <asp:Button ID="btnSave" runat="server" Text="Check In to MA" Width="94%" Class="aspresizedgreenbutton" OnClientClick="return ValidatePatientInformation();"
                                    AccessKey="S" OnClick="btnSave_Click" Visible="false" style="margin-top:8%; " />&nbsp;&nbsp;
                            </td>
                             <td style="width:1%;text-align:right;">
                                 <asp:Button ID="btnQuit" runat="server" Text="Close" Width="100px" margin-left="2%" OnClientClick="return CloseWindow();" Class="aspresizedredbutton"
                                AccessKey="Q" style="margin-top:5%"/>&nbsp;&nbsp;
                              </td> --%>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                    <asp:Panel ID="Panel2" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="Label1" Text="" runat="server"></asp:Label></center>
                        <br />
                        <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                            alt="Loading..." />
                        <br />
                    </asp:Panel>
                </div>
                <asp:HiddenField ID="hdnHumanID" runat="server" />
                <asp:HiddenField ID="hdnEncounterID" runat="server" />
                <asp:HiddenField ID="hdnScreenMode" runat="server" />
                <asp:HiddenField ID="hdnbShowPatInfo" runat="server" />
                <asp:HiddenField ID="hdnParentScreen" runat="server" />
                <asp:HiddenField ID="hdnPCTime" runat="server" />
                <asp:HiddenField ID="hdnPhyName" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPhyID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnFacility" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnValidation" runat="server" />
                <asp:HiddenField ID="hdnCarrierName" runat="server" />
                <asp:HiddenField ID="hdnCarrierId" runat="server" />
                <br />
                <asp:HiddenField ID="hdnEncStatus" runat="server" />
                <asp:HiddenField ID="hdnLocalTime" runat="server" />
                <asp:HiddenField ID="hdnPPLineItemID" runat="server" />
                <asp:HiddenField ID="hdnPPHeaderID" runat="server" />
                <asp:HiddenField ID="hdnVisitID" runat="server" />
                <asp:HiddenField ID="hdnCheckID" runat="server" />
                <asp:HiddenField ID="hdnIsmailsend" runat="server" />
                <asp:HiddenField ID="hdnDupsendmail" runat="server" />
                <asp:HiddenField ID="hdnfacilityanc" runat="server" />
                <asp:HiddenField ID="hdnCPT" runat="server" />
                <asp:HiddenField ID="hdnUploadFile" runat="server" />

                 <asp:HiddenField ID="hdnPatientType" runat="server" EnableViewState="false" />
                 <asp:HiddenField ID="hdnPatientID" runat="server" EnableViewState="false" />
               <%-- // <asp:HiddenField ID="hdnBtnLoadPatientDetails" Style="display: none" runat="server"/>--%>

                <asp:HiddenField ID="hdnPastDue" runat="server" EnableViewState="false" />


            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnendwaitcursor" runat="server" Text="Button" Style="display: none"
            OnClick="btnendwaitcursor_Click" />

        <asp:Button ID="btnloadgrid" runat="server" Text="Button" Style="display: none"
            OnClick="btnloadgrid_Click" />

          

        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
            OnClientClick="return CloseWindow();" />
        <asp:HiddenField ID="hdnMessageType" runat="server" />
        <asp:HiddenField ID="hdnBSave" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <link href="CSS/jquery-ui.min.css" rel="stylesheet" />
          <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
        <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />

            <script src="JScripts/bootstrap.min.js"></script>
            <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />

            <script src="JScripts/JSQuickPatient.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
           
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            
                <%--<link href="CSS/style.css" rel="stylesheet" type="text/css" />--%>

        </asp:PlaceHolder>
    </form>
</body>
</html>
