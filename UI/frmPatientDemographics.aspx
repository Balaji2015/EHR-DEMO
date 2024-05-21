<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientDemographics.aspx.cs"
    MasterPageFile="~/DemoGraphicsEmpty.Master" EnableEventValidation="false" Inherits="Acurus.Capella.UI.frmPatientDemographics" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script runat="server">
</script>

<asp:Content ID="Demographics" ContentPlaceHolderID="C5POBody" runat="server">
    <head>
        
        <title>Patient Demographics</title>
        <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
        <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
        <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
        <%--<link href="CSS/jquery-ui.min.css" rel="stylesheet" />--%>
          <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" href="CSS/datetimepicker.css" />
       <%-- <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
        <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>--%>
      <base target="_self" />
        <style type="text/css">
            .style155 {
                width: 75px;
            }

            .style159 {
                width: 33px;
            }

            .style163 {
                width: 118px;
            }

            .panel_with_padding {
                -webkit-padding-before: 5px;
                -webkit-padding-end: 0px;
                -webkit-padding-after: 5px;
                -webkit-padding-start: 0px;
            }

            .style164 {
                width: 70px;
            }

            .style165 {
                width: 127px;
            }

            .style166 {
                width: 91px;
            }

            .style167 {
                width: 179px;
            }

            .style168 {
                width: 132px;
            }

            .style170 {
                width: 82px;
            }

            .style171 {
                width: 129px;
            }

            .style174 {
            }

            .style182 {
                width: 111px;
            }

            .style185 {
                width: 146px;
            }

            .style188 {
                width: 137px;
            }

            .style194 {
                width: 79px;
            }

            .style199 {
                width: 77px;
            }

            .style205 {
                width: 43px;
            }

            .style206 {
                width: 68px;
            }

            .style208 {
                width: 438px;
            }

            .style209 {
                width: 148px;
            }

            .style212 {
                width: 113px;
            }

            .style213 {
                width: 69px;
            }

            .style214 {
                width: 236px;
            }

            .style218 {
                width: 8px;
            }

            .style222 {
                width: 182px;
                height: 38px;
            }

            .style223 {
                width: 88px;
                height: 38px;
            }

            .style224 {
                width: 165px;
                height: 38px;
            }

            .style227 {
                width: 190px;
                font-family: Verdana;
                font-size: 10pt;
                font-weight: normal;
                color: Black;
                height: 38px;
            }

            .style231 {
                width: 5px;
            }

            .style232 {
                width: 83px;
            }

            .style233 {
                width: 11px;
            }

            .style237 {
                height: 38px;
                width: 190px;
            }

            .style239 {
                width: 82px;
                height: 38px;
            }



            .style240 {
                width: 72px;
            }

            .style241 {
                width: 10px;
            }

            .style242 {
                width: 125px;
            }

            .style246 {
            }

            .style250 {
                height: 38px;
                width: 76px;
            }

            .style266 {
                width: 74px;
            }

            .style268 {
                width: 313px;
            }

            .style269 {
                width: 209px;
            }

            .style270 {
                width: 71px;
            }

            .style272 {
                width: 203px;
                margin-left: 40px;
            }

            .style274 {
                width: 3px;
            }

            .style276 {
                width: 248px;
                margin-left: 40px;
            }

            .style277 {
                height: 20px;
            }

            .style278 {
                height: 20px;
                width: 93px;
            }

            .style279 {
                height: 20px;
                width: 80px;
            }

            .style280 {
                height: 20px;
                width: 30px;
            }

            .style282 {
                width: 77px;
            }

            .style288 {
                height: 38px;
                width: 77px;
            }

            .style293 {
            }

            .style294 {
                height: 38px;
                width: 10px;
            }

            .style296 {
                width: 165px;
            }

            .style298 {
                width: 182px;
            }

            .style299 {
                width: 76px;
            }

            .style300 {
                width: 190px;
            }

            .style301 {
                width: 131px;
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

            .style302 {
                width: 41px;
            }

            .style303 {
                width: 150px;
            }

            .style304 {
                width: 19px;
            }

            .style305 {
                width: 16px;
            }

            .style306 {
                width: 78px;
            }

            .style307 {
                width: 156px;
            }

            .style308 {
                width: 195px;
            }

            .style310 {
                width: 77px;
                height: 41px;
            }

            .style311 {
                width: 182px;
                height: 41px;
            }

            .style312 {
                width: 10px;
                height: 41px;
            }

            .style313 {
                height: 41px;
            }

            .style315 {
                width: 82px;
                height: 41px;
            }

            .style318 {
                width: 46px;
            }

            .auto-style1 {
                width: 335px;
            }

            .auto-style6 {
                width: 77px;
                height: 28px;
            }

            .auto-style7 {
                width: 182px;
                height: 28px;
            }

            .auto-style8 {
                width: 10px;
                height: 28px;
            }

            .auto-style9 {
                height: 28px;
            }

            .auto-style10 {
                width: 82px;
                height: 28px;
            }

            .auto-style11 {
                height: 20px;
                width: 139px;
            }

            .auto-style16 {
                width: 165px;
                height: 8px;
            }

            .auto-style17 {
                width: 76px;
                height: 8px;
            }

            .auto-style18 {
                width: 77px;
                height: 8px;
            }

            .auto-style19 {
                width: 182px;
                height: 8px;
            }

            .auto-style20 {
                width: 10px;
                height: 8px;
            }

            .auto-style21 {
                height: 8px;
            }

            .auto-style22 {
                width: 156px;
                height: 38px;
            }

            .auto-style23 {
                width: 195px;
                height: 38px;
            }

            .auto-style24 {
                height: 38px;
            }

            #pnlPatientInfo fieldset {
                height: 600px;
            }

            .ui-autocomplete {
                -webkit-margin-before: 3px !important;
                max-height: 150px;
                overflow-y: auto;
            }

            .ui-state-focus {
                color: #808080;
                background-color: #bbe2f1 !important;
                outline: none;
                border: 0px !important;
            }
            #tbodupolicyinfo tr td {
                word-break: break-word;
            }
        </style>
    </head>
    <body>
        
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server">
            <Windows>
                <telerik:RadWindow ID="DemographicsModalWindow" runat="server" VisibleOnPageLoad="false"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" DestroyOnClose="true">
                </telerik:RadWindow>

                <telerik:RadWindow ID="DemographicsAddInsured" runat="server" VisibleOnPageLoad="false"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" DestroyOnClose="true">
                </telerik:RadWindow>

                <telerik:RadWindow ID="DLCWindow" runat="server" VisibleOnPageLoad="false" Title="DLC"
                    Height="625px" IconUrl="Resources/16_16.ico" Width="1225px">
                </telerik:RadWindow>
            </Windows>
             <Windows>
                <telerik:RadWindow ID="MessageWindowAuth" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>

        <script language="javascript" type="text/javascript">
            var DLCWindow = "<%=DLCWindow.ClientID %>";
            var ModalWndw = "<%=DemographicsModalWindow.ClientID %>";
        </script>

        <form id="frmPatientDemographics">
            <div>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
                <table style="display:none;">
                     <td class="style164" style="visibility:hidden;">
                                    <asp:Label ID="lblWorksetID" runat="server" Text="WorkSet ID" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                     </td>
                     <td class="style165">
                                    <asp:TextBox ID="txtWorkSetID" runat="server" Cssclass="nonEditabletxtbox" ReadOnly="True"
                                        BorderWidth="1px" Width="126px"></asp:TextBox>
                     </td>
                </table>
                <div>
                    <asp:Panel ID="pnlPatientInfo" runat="server" CssClass="LabelStyleBold" GroupingText="Patient/Insured Information"
                        >
                        <table style="width: 100%; table-layout:fixed">
                            <tr style="width:100%" >
                                <td style="width:11%">
                                    <asp:Label ID="lblAccountno" runat="server" Text="Acc. #" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td  align="left" style="width:11%">
                                    <asp:TextBox ID="txtAccountNo" runat="server" CssClass="nonEditabletxtbox" 
                                        BorderWidth="1px" Width="92%" ReadOnly="True"></asp:TextBox>
                                </td>
                               <%-- <td  align="left">&nbsp;
                                </td>--%>

                                <td style="width:11%">
                                    <asp:Label ID="lblMedicalrecordno" runat="server" Text="Med Rec.#" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtMedicalRecordno" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" autocomplete="off" CssClass="Editabletxtbox"
                                        Width="93%" MaxLength="25" EnableViewState="false"></asp:TextBox>
                                </td>
                                                               
                                <td style="width:11%">
                                    <asp:Label ID="lblExternalAccountNo" runat="server" Text="Ext.Acc #" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:17%">
                                    <asp:TextBox ID="txtExternalAccNo" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        Width="93%" MaxLength="15" EnableViewState="false"></asp:TextBox>
                                </td>
                                <%--<td></td>--%>

                                <td style="width:11%">
                                    <asp:Label ID="lblDynamicsNumber" runat="server" Text="Dynamics #" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                 <td  style="width:11%">
                                    <asp:TextBox ID="txtDynamicsNumber" onchange="AutoSave();" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        autocomplete="off" EnableViewState="false" width="160px" MaxLength="15"></asp:TextBox>
                                </td>
                                <td style="width:11%"></td>
                                
                                <%--<td width="150">
                                    <asp:Button ID="btnFindPatient" runat="server" OnClick="btnFindPatient_Click" CssClass="aspresizedbluebutton" OnClientClick="return OpenFindPatient();"
                                        Text="Find Patient" />
                                </td>--%>
                               <%-- <td class="style274">&nbsp;
                                </td>--%>

                                
                               
                                
                                
                            </tr>
                            <tr>
                                 <td style="width:11%">
                                    <asp:Label ID="Label7" runat="server" Text="Previous Name" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                 <td  style="width:11%">
                                    <asp:TextBox ID="TxtPreviousName" onchange="AutoSave();" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        autocomplete="off" EnableViewState="false" width="92px" ></asp:TextBox>
                                   <%--  Width="72%" style="margin-left:-65px"--%>
                                </td>
                                
                                <td style="width:11%">
                                    <asp:Button ID="btnEditName" runat="server" OnClick="btnEditName_Click" Text="Edit Name" CssClass="aspresizedbluebutton"
                                        AccessKey="E" Style="margin-right: 1px; margin-bottom: 0px;" Width="80px" />
                                </td>
                            </tr>
                            <tr style="width:100%" >
                                <td style="width:11%">

                                     <span class="MandLabelstyle">Last Name</span><span class="manredforstar">*</span> 

                                    <%--<asp:Label ID="lblPatientlastname" runat="server" Text="Last Name*" mand="Yes" 
                                        EnableViewState="false"></asp:Label>--%>
                                </td>
                                <td style="width:11%">
                                    <asp:TextBox ID="txtPatientlastname" runat="server" onchange="AutoSave();" autocomplete="off" 
                                        onkeypress="return PreventDot(this);" onkeyup="Copy('Lastname');" Width="91%"
                                        MaxLength="35" ></asp:TextBox>
                                </td>
                                <td style="width:11%">

                                     <span class="MandLabelstyle">First Name</span><span class="manredforstar">*</span>


                                    <%--<asp:Label ID="lblPatientfirstname" runat="server" mand="Yes" Text="First Name*"
                                        EnableViewState="false"></asp:Label>--%>
                                </td>
                                <td style="width:11%" >
                                    <asp:TextBox ID="txtPatientfirstname" runat="server" onchange="AutoSave();" onkeypress="return PreventDot(this);" 
                                        autocomplete="off" onkeyup="Copy('Firstname');" Width="93%" MaxLength="35"></asp:TextBox>
                                </td>
                                <td style="width:11%">
                                    <asp:Label ID="lblPatientmiddlename" runat="server" Text="Middle Name" Width="75px" CssClass="spanstyle"
                                        EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                     <asp:Panel ID="Panel3" runat="server">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td >
                                                    <asp:TextBox ID="txtPatientmiddlename" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" 
                                        autocomplete="off" onkeyup="Copy('MiddleName');" Width="95%" MaxLength="20"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSuffix" runat="server" Text="Suffix" Width="30px" CssClass="spanstyle"
                                        EnableViewState="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSuffix" runat="server" onchange="AutoSave();" >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                
                                <td align="left" style="width:11%">

                                     <span class="MandLabelstyle">DOB</span><span class="manredforstar">*</span><span class="MandLabelstyle">(Format: 01-Jan-1987)</span>

                                    <%--<asp:Label ID="lblPatientdob" runat="server" mand="Yes" 
                                        Text="DOB* (Format: 01-Jan-1987)" EnableViewState="False"></asp:Label>--%>
                                </td>
                                <td width="150" style="width:11%">
                                   <%-- <telerik:RadMaskedTextBox ID="dtpPatientDOB" runat="server" Mask="##-Lll-####" Width="99%"
                                        OnTextChanged="dtpPatientDOB_TextChanged" onchange="Copy('PatientDOB');">
                                        <ClientEvents OnValueChanged=""  OnBlur="PatientDemographicsDateVlidation"/>
                                        <InvalidStyle Resize="None" />
                                        <FocusedStyle Resize="None" />
                                        <EmptyMessageStyle Resize="None" />
                                        <HoveredStyle Resize="None" />
                                        <DisabledStyle Resize="None" />
                                        <EnabledStyle Resize="None" />
                                        <ReadOnlyStyle Resize="None" />
                                    </telerik:RadMaskedTextBox>--%>
                                    <%-- Added onchange="Copy('PatientDOB');" for BUGID:32770 --%>
                                    <telerik:RadMaskedTextBox ID="dtpPatientDOB" runat="server" Mask="##-Lll-####" Width="171px" onchange="Copy('PatientDOB'); " onkeyup="Copy('DOB');" >
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
                            </tr>
                            <tr style="width:100%" >
                                <td style="width:11%">
                                     <span class="MandLabelstyle">Sex</span><span class="manredforstar">*</span>
                                    <%--<asp:Label ID="lblPatientSex" runat="server" Text="Sex *" mand="Yes"  EnableViewState="False"></asp:Label>--%>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlPatientsex" runat="server" onchange="patientSexChanged();" Width="99%">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="HiddenPatientSex" runat="server" />
                                </td>
                                
                                <td style="width:11%">
                                    
                                     <span class="MandLabelstyle">Address Line 1</span><span class="manredforstar">*</span>

                                    <%--<asp:Label ID="lblPatientaddress" runat="server" mand="Yes" Text="Address Line 1*" 
                                        EnableViewState="false"></asp:Label>--%>
                                </td>
                                <td style="width:11%">
                                    <asp:TextBox ID="txtPatientAddress" runat="server" onchange="AutoSave();" onkeypress="AutoSave();"
                                        onkeyup="Copy('Address');" autocomplete="off" Width="92%" MaxLength="55" EnableViewState="false" CssClass="Editabletxtbox"></asp:TextBox>
                                    <%-- <asp:ImageButton ID="ImageButton1" runat="server" 
                                    ImageUrl="~/Resources/H_Img.jpg" Height="19px" 
                                    onclick="ImageButton1_Click" />--%>
                                </td>
                               
                                <td style="width:11%">
                                    <asp:Label ID="lblPatientAddressLine2" runat="server" CssClass="spanstyle" Text="Address Line 2" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:TextBox ID="txtPatientAddressLine2" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        onkeyup="Copy('Address Line');" autocomplete="off" Width="93%" MaxLength="55"
                                        EnableViewState="false"></asp:TextBox>
                                </td>
                                
                                <td align="left" style="width:11%">

                                     <span class="MandLabelstyle">City</span><span class="manredforstar">*</span>

                                    <%--<asp:Label ID="lblCity" runat="server" mand="Yes" Text="City*"  EnableViewState="false"></asp:Label>--%>
                                </td>
                                <td style="width:11%">  <%--width="150"--%>
                                    <asp:TextBox ID="txtCity" runat="server" AutoPostBack="false" onchange="AutoSave();"
                                        autocomplete="off" onkeypress="AutoSave();" onkeyup="Copy('City');" Width="160%" CssClass="Editabletxtbox"
                                        MaxLength="30" EnableViewState="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="width:100%" >
                                <td style="width:11%">
                                    <%--<asp:Label ID="lblState" runat="server" mand="Yes" Text="State*"  EnableViewState="false"></asp:Label>--%>
                                    <span class="MandLabelstyle">State</span><span class="manredforstar">*</span>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlState" runat="server" onchange="Copy('State');" Width="99%" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                               
                                <td style="width:11%">
                                    <%--<asp:Label ID="Label3" runat="server" Text="Birth Place"></asp:Label>--%>

                                     <span class="MandLabelstyle">Zip Code</span><span class="manredforstar">*</span>


                                    <%--<asp:Label ID="lblZipcode" runat="server" Text="Zip Code*" mand="Yes" EnableViewState="false" ></asp:Label>--%>
                                </td>
                                <td style="width:11%">
                                    <asp:Panel ID="Panel4" runat="server">
                                        <table style="width: 100%;">
                                            <tr>
                                                <%--<td class="style306">
                                                <asp:DropDownList ID="ddlBirthPlace" runat="server">
                                                </asp:DropDownList>
                                            </td>--%>
                                                <td>
                                                    <telerik:RadMaskedTextBox ID="msktxtZipcode" runat="server"  Mask="#####-####" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                                        onkeyup="Copy('Zipcode');" Width="99%" EnableViewState="false">
                                                    </telerik:RadMaskedTextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPatientMaritalStatus" runat="server" CssClass="spanstyle" EnableViewState="false" Text="Marital Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPatientMaritalStatus" runat="server" onchange="AutoSave();" CssClass="Editabletxtbox" Width="59px"
                                                        style="margin-right: 7px;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                
                                <td align="left" style="width:11%">
                                     <span class="MandLabelstyle">Cell Ph#</span><span class="manredforstar">*</span>                                    
                                </td>
                                <td width="150" style="width:11%">
                                    <telerik:RadMaskedTextBox ID="msktxtCellPhno" runat="server" onkeyup="Copy('CellPhone');" CssClass="Editabletxtbox"
                                        onkeypress="AutoSave();" Mask="(###) ###-####" Width="99%" EnableViewState="false">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                 <td style="width:11%">
                                    <asp:Label ID="lblHomePhno" runat="server" Text="Home Ph#" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <telerik:RadMaskedTextBox ID="msktxtHomePhno" runat="server" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        onkeyup="Copy('HomePhone');" Mask="(###) ###-####" Width="167%" EnableViewState="false">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                
                            </tr>


                             <tr style="width:100%" >
                                <td style="width:11%">
                                    <asp:Label ID="Label1" runat="server" Text="Sexual Orientation" CssClass="spanstyle"  EnableViewState="False"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlSexualOrientation" runat="server"  Width="99%" onchange="ddlSexualOrientation_change()" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                                <td style="width:11%">
                                    <asp:Label ID="Label3" runat="server"  Text="Specify"  CssClass="spanstyle"
                                        EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:TextBox ID="TxtSexualOrientationSpecify" runat="server" onchange="AutoSave();"  onkeypress="AutoSave();"  CssClass="nonEditabletxtbox"
                                        
                                        Width="93%"  ReadOnly="true" EnableViewState="false"></asp:TextBox>
                                    <%-- <asp:ImageButton ID="ImageButton1" runat="server" 
                                    ImageUrl="~/Resources/H_Img.jpg" Height="19px" 
                                    onclick="ImageButton1_Click" />--%>
                                </td>
                                <td style="width:11%">
                                    <asp:Label ID="Label4" runat="server" Text="Gender Identity" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                     <asp:DropDownList ID="ddlGenderIdentity" runat="server"  Width="99%" onchange="ddlGenderIdentity_change()" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                               
                                <td align="left" style="width:11%">
                                    <asp:Label ID="Label5" runat="server"  Text="Specify" CssClass="spanstyle"
                                        EnableViewState="false"></asp:Label>
                                </td>
                                <td width="150" style="width:11%">
                                    <asp:TextBox ID="TxtGenderIdentity" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="nonEditabletxtbox"
                                       
                                        Width="160%" ReadOnly="true" EnableViewState="true"></asp:TextBox>
                                </td>
                            </tr>


                            <tr style="width:100%">
                               
                                <td style="width:11%">
                                    <asp:Label ID="lblEthnicity" runat="server" Text="Ethnicity" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                    <%--<asp:Label ID="lblEthnicity" runat="server" ForeColor="Red" Text="Ethnicity*" EnableViewState="false"></asp:Label>--%>
                                   
                                </td>
                                 <td style="width:11%">
                                           <asp:DropDownList ID="ddlEthnicity" runat="server" onchange="showTip(this);" onmouseover="OnMouseHover(this);" CssClass="Editabletxtbox"
                                        Width="100%">
                                    </asp:DropDownList>  
                                     </td>
                                <td style="width:11%">
                                    <asp:Label ID="lblRace" runat="server" CssClass="spanstyle" Text="Race" EnableViewState="false"></asp:Label>
                                    <%--<asp:Label ID="lblRace" runat="server" ForeColor="Red" Text="Race*" EnableViewState="false"></asp:Label>--%>
                                </td>
                                <td style="width:11%">
                                    <%--<asp:DropDownList ID="ddlRace" runat="server" onchange="showTip(this);" onmouseover="OnMouseHover(this);"
                                    Width="99%">
                                </asp:DropDownList>--%>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 95%">
                                                <asp:TextBox ID="txtRace" runat="server" TextMode="MultiLine" Style="position: static; resize: none;" CssClass="Editabletxtbox"
                                                    MaxLength="32767" nospell="true"
                                                    Width="150px" oncopy="return false" onpaste="return false" oncut="return false"
                                                    onblur="return textboxReleave(this,event);" />
                                                <asp:ListBox ID="listRace" runat="server" onblur="return textboxReleave(this,event);" CssClass="Editabletxtbox"
                                                    Style="display: none; position: absolute; width: 155px;" Font-Bold="false" onclick="return listRaceChange(this);"></asp:ListBox>
                                            </td>
                                            <td style="width: 5%;">
                                                <%-- <asp:Button ID ="btnDropdown" runat ="server" Text ="+" OnClientClick=" return btnDropDown();" />--%>
                                                <asp:ImageButton ID="ImgbtnDropdown" runat="server" AutoPostBack="false" OnClientClick="return RaceImageButton();"
                                                    ImageUrl="~/Resources/Dropdownimg.jpg" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                              
                              <td style="width:11%">
                                    <asp:Label ID="lblgra" runat="server" Text="Granularity" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                    <%--<asp:Label ID="lblRace" runat="server" ForeColor="Red" Text="Race*" EnableViewState="false"></asp:Label>--%>
                                </td>
                                <td style="width:11%">
                                   
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 95%">
                                                <asp:TextBox ID="txtGranularity" runat="server" TextMode="MultiLine" Style="position: static; resize: none;"
                                                    MaxLength="32767" nospell="true" CssClass="Editabletxtbox"
                                                    Width="120px" oncopy="return false" onpaste="return false" oncut="return false"
                                                    onblur="return textboxReleaveGranular(this,event);" />
                                                <asp:ListBox ID="ListGranularity" runat="server" onblur="return textboxReleaveGranular(this,event);" CssClass="Editabletxtbox"
                                                    Style="display: none; position: absolute; width: 120px;" Font-Bold="false" onclick="return listGranularityChange(this);"></asp:ListBox>
                                            </td>
                                            <td style="width: 5%;">
                                                <%-- <asp:Button ID ="btnDropdown" runat ="server" Text ="+" OnClientClick=" return btnDropDown();" />--%>
                                                <asp:ImageButton ID="ImageGranularity" runat="server" AutoPostBack="false" OnClientClick="return GranularityImageButton();"
                                                    ImageUrl="~/Resources/Dropdownimg.jpg" />
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                                                                   
                                 <td style="width:11%">
                                    <asp:Label ID="lblPreferredLanguage" runat="server" Text="Pref.Lang" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                    <%--<asp:Label ID="lblPreferredLanguage" runat="server" Text="Pref.Lang*" ForeColor="Red"
                                    EnableViewState="false"></asp:Label>--%>
                                                
                                </td>
                                          
                                 
                               <td style="width:11%">
                                    <table width="220%">
                                        <tr>
                                            <td style="width:50%">
                                    <asp:DropDownList ID="ddlPreferredLanguage" onchange="showTip(this);" runat="server" CssClass="Editabletxtbox"
                                        onmouseover="OnMouseHover(this);" Width="115%">
                                         </asp:DropDownList>
                                   </td>
                                       <td style="width:28%; text-align:right;">
                                     <asp:CheckBox ID="chkReqTranslator" runat="server"  onclick="AutoSave();"
                                                 CssClass="spanstyle" />
                                     <asp:Label ID="Translator" runat="server" Text="Req.</br>Translator" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                        </td>
                                      </tr>
                                        </table>
                                                                                        
                                           </tr>
                            <tr style="width:100%">
                                <td style="width:11%">
                                    <asp:Label ID="lblLicenseState" runat="server" Text="License State" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlLicenseState" onchange="AutoSave();" CssClass="Editabletxtbox" runat="server" Width="99%">
                                    </asp:DropDownList>
                                </td>
                               
                                <td style="width:11%">
                                    <asp:Label ID="lblCorrespondenceMode" runat="server" CssClass="spanstyle" Text="Pref.  Corres" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlPreferredCorrespondenceMode" runat="server" onchange="showTip(this);" CssClass="Editabletxtbox"
                                        onmouseover="OnMouseHover(this);" Width="99%">
                                    </asp:DropDownList>
                                </td>
                                <td style="width:11%">
                                    <asp:Label ID="lblEmploymentStatus" runat="server" CssClass="spanstyle" Text="Emp.Status" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlEmploymentStatus" runat="server" onchange="showTip(this);" CssClass="Editabletxtbox"
                                        onmouseover="OnMouseHover(this);" Width="99%">
                                    </asp:DropDownList>
                                </td>
                                                                 
                                 <td style="width:15%;">
                                      <asp:Label ID="Label8" runat="server" CssClass="spanstyle" Text="Patient Photo"></asp:Label>
                                     <asp:Label ID="Label9" runat="server" CssClass="Editabletxtbox" Text="(File Format supported:*.jpg,*.png)" style="width:2%; color:blue; font-size:2%" Font-Size="20px"></asp:Label>
                          
                                   </td>
                                 <td  colspan="4">
                                     <asp:FileUpload ID="fileupload" runat="server" onchange="AutoSave();" accept="image/*"/>
                                </td>
                                
                               <td>

                               </td>
                                 <td  style="width:10%"; rowspan="5"  >
                                            <asp:Image ID="imgOverAllSummary" Width="139px" onmouseover="showtooltip(this)" Height="121px" ImageAlign="Top" runat="server" data-sqre="imgOverAllSummary_sqre" data-tooltp="imgOverAllSummary_tooltp"  
                                                 CssClass="displayInline boxModel DockImage" ImageUrl="~/Resources/person.gif" 
                                                style="margin-left:-201px; margin-top:46px" />
                                     <%--style=" margin-left:-353px ;margin-bottom:-71px;" Width="212%"Height="138px" 97, 102 --%> 
                                 </td>
                                 <%--<td  style="width:10%"; rowspan="5"  >
                                            <asp:Image ID="imgOverAllSummary" Width="139px" onmouseover="showtooltip(this)" Height="121px" ImageAlign="Top" runat="server" data-sqre="imgOverAllSummary_sqre" data-tooltp="imgOverAllSummary_tooltp"  
                                                 CssClass="displayInline boxModel DockImage" ImageUrl="~/Resources/prasad.jpg" 
                                                style="margin-left:-139px; margin-top:31px" />
                                     <%--style=" margin-left:-353px ;margin-bottom:-71px;" Width="212%"Height="138px" 97, 102 --%> 
                                  <%--</td>--%>
                            </tr>
                            <tr style="width:100%">
                                <td align="left" style="width:11%">
                                    <asp:Label ID="lblEmployerName" runat="server" CssClass="spanstyle" Text="Emp.Name" EnableViewState="false"></asp:Label>
                                </td>
                                <td width="150" style="width:11%">
                                    <asp:TextBox ID="txtEmployerName" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        autocomplete="off" Width="92%" MaxLength="100" EnableViewState="false"></asp:TextBox>
                                </td>
                                                                 
                                <td style="width:11%">
                                    <asp:Label ID="lblWorkPhoneno" runat="server" Text="Work Ph#" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <telerik:RadMaskedTextBox ID="msktxtWorkPhoneNo" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        onkeyup="Copy('Zipcode');" Mask="(###) ###-####" Width="98%" EnableViewState="false">
                                    </telerik:RadMaskedTextBox>
                                 
                                </td>
                                <%-- <td ></td>--%>
                                <td style="width:11%">
                                    <asp:Label ID="lblExtensionNumber" runat="server" Text="Extn" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                  <td style="width:11%" >
                                    <asp:TextBox ID="txtExtensionNumber" runat="server" MaxLength="13" onchange="AutoSave();" CssClass="Editabletxtbox"
                                        autocomplete="off" OnKeyPress="change(this);" Width="94%" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td>

                                </td>
                                <td></td>
                                 <%--<td style="width:30px">
                                      
<%--                                     <asp:Label ID="lblFileFormat" runat="server" Text="(File Format supported:*.jpg)" style="color:blue;margin-left:-203px; white-space:nowrap"  CssClass="Editabletxtbox"></asp:Label>--%>
                            <%--     </td>--%>
                               
                            </tr>
                            <tr style="width:100%">
                                
                                <%--<td class="style274">&nbsp;
                                </td>--%>
                               <%-- <td style="width:11%">
                                    <asp:Label ID="lblEmail" runat="server" Text="E-Mail" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>--%>
                                <td>
                                    <span class="spanstyle" id="spanemail" runat="server">E-Mail</span><span class="manredforstar" runat="server" id="spanemailstar" style="margin-top:-15px;margin-left:2px;" visible="false">*</span>
                                </td>
                                <td style="width:11%">
                                   <%-- <asp:TextBox ID="txtEmail" onchange="AutoSave();" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        autocomplete="off" Width="99%" MaxLength="100" EnableViewState="false" onkeyup="Copy('Email');"></asp:TextBox>--%>
                                    <%--CAP-1975 - done--%>
                                     <%--<asp:TextBox ID="txtEmail" runat="server" onkeypress="AutoSave(this);" Width="91%" EnableViewState="false"
                                             MaxLength="100" onkeyup="" class="Editabletxtbox"  autocomplete="off"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtEmail" runat="server" onkeypress="AutoSave(this);" Width="91%" EnableViewState="false"
                                             MaxLength="100" onkeyup="Copy('Email');" class="Editabletxtbox"  autocomplete="off"></asp:TextBox>
                                </td>
                                <%-- <td></td>--%>
                                  <td style="width:11%">
                                    <asp:Label ID="lblReptEmail" runat="server" CssClass="spanstyle" Text="Representative E-Mail"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:TextBox ID="txtReptEmail" onchange="AutoSave();" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        autocomplete="off" Width="93%" MaxLength="100" EnableViewState="false"></asp:TextBox>
                                </td>
                                
                               <%-- <td class="style218">&nbsp;
                                </td>--%>
<%--                                 <td ></td>--%>
                                  <td style="width:11%">
                                    <asp:CheckBox ID="chkEnrollOnlineAccess" runat="server"  
                                          CssClass="spanstyle" onclick="EnableSendEmail(this);" onchange="AutoSave();"
                                        Text="Enroll Online Access" AutoPostBack="true" Width="100px" OnCheckedChanged="chkEnrollOnlineAccess_CheckedChanged"/>  
                                            
                                </td>
                                <td style="width:11%">
                                    <asp:Button ID="btnSendEmail" runat="server" OnClick="btnSendEmail_Click" OnClientClick="return PatientInformationValidation();" CssClass="aspresizedbluebutton"
                                        Text="Send Email" Width="86px" />
                                </td>

                            </tr>
                           
                           
                            <tr style="width:100%">
                                <td style="width:11%">
                                    <asp:Label ID="lblPatientStatus" runat="server" Text="Status" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <asp:DropDownList ID="ddlPatientStatus" runat="server" OnSelectedIndexChanged="ddlPatientStatus_SelectedIndexChanged" CssClass="Editabletxtbox"
                                        onchange="ChangeDeathStatus(this);" Width="97%" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                               <%-- <td >&nbsp;
                                </td>--%>
                                <td style="width:11%">
                                    <asp:Label ID="lblDateOfDeath" runat="server" Text="Date of Death" EnableViewState="false" mand="Yes" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <telerik:RadMaskedTextBox ID="dtpDateOfDeath" runat="server" Mask="##-Lll-####" Width="97%"   CssClass="nonEditabletxtbox"
                                        onkeypress="AutoSave();" EnableViewState="true">
                                      
                                        <InvalidStyle Resize="None" />
                                        <FocusedStyle Resize="None" CssClass="Editabletxtbox" />
                                        <EmptyMessageStyle Resize="None" />
                                        <HoveredStyle Resize="None" CssClass="Editabletxtbox" />
                                        <DisabledStyle Resize="None" />
                                        <EnabledStyle Resize="None" />
                                        <ReadOnlyStyle Resize="None" />
                                    </telerik:RadMaskedTextBox>
                                </td>
                              <%--  <td>&nbsp;
                                </td>--%>
                                <td style="width:11%">
                                    <asp:Label ID="lblReasonForDeath" runat="server"  Text="Reason For Death" EnableViewState="false" mand="Yes" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td style="width:11%" >
                                    <asp:DropDownList ID="ddlReasonForDeath" runat="server" onchange="showTip(this);" 
                                        onmouseover="OnMouseHover(this);" Width="99%">
                                    </asp:DropDownList>
                                </td>
                                </tr>
                            <tr style="width:100%">
                                 <td align="left" style="width:11%">
                                    <asp:Label ID="Label6" runat="server" Text="Driver License" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td width="150" style="width:11%">
                                    <asp:TextBox ID="txtDriverLicenseno" runat="server" onchange="AutoSave();" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        autocomplete="off" Width="91%" MaxLength="15" EnableViewState="false"></asp:TextBox>
                                </td>
                                <%-- <td >&nbsp;
                                </td>--%>
                                <td style="width:11%">
                                    <asp:Label ID="lblSSN" runat="server" Text="SSN" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="width:11%">
                                    <telerik:RadMaskedTextBox ID="msktxtSSN" runat="server" Mask="###-##-####" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        onkeyup="Copy('Zipcode');" Width="96%" EnableViewState="false">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                <%-- <td>&nbsp;
                                </td>--%>

                                 <td style="width:11%">
                                    <asp:Label ID="lblFax" runat="server" Text="Fax #" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <%-- <td class="style199"></td>--%>
                                <td style="width:11%">
                                    <telerik:RadMaskedTextBox ID="msktxtFaxNumber" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        onkeyup="Copy('Zipcode');" Mask="(###) ###-####" Width="99%" EnableViewState="false">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                 <%--<td>&nbsp;
                                </td>--%>

                              <%--  <td class="style199">
                                    <asp:Label ID="lblFax" runat="server" Text="Fax #" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="style182">
                                    <telerik:RadMaskedTextBox ID="msktxtFaxNumber" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        onkeyup="Copy('Zipcode');" Mask="(###) ###-####" Width="99%" EnableViewState="false">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                 <td>&nbsp;
                                </td>--%>
                            </tr>
                            <%--<tr>
                                <td class="style199">
                                    <asp:Label ID="Label7" runat="server" Text="Previous Name" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                 <td class="style199">

                                    <asp:TextBox ID="TxtPreviousName" onchange="AutoSave();" runat="server" OnKeyPress="change(this);" CssClass="Editabletxtbox"
                                        autocomplete="off" Width="99%" MaxLength="100" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                </td>--%>
                            <%--</tr>--%>
                           
                            
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:Panel ID="pnlCareGiverInfo" runat="server" CssClass="LabelStyleBold" GroupingText="Care Giver Information">
                        <table style="width: 100%">
                            <tr>
                                <td class="style270">
                                    <asp:Label ID="lblCGLastName" runat="server" Text="Last Name" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="style300" align="left">
                                    <asp:TextBox ID="txtCGLastName" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" autocomplete="off" CssClass="Editabletxtbox"
                                        Width="99%" MaxLength="25" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td class="style274" align="left">&nbsp;
                                </td>
                                <td class="style155">
                                    <asp:Label ID="lblCGFirstName" runat="server" Text="First Name" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td class="style300">
                                    <asp:TextBox ID="txtCGFirstName" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" autocomplete="off" CssClass="Editabletxtbox"
                                        Width="99%" MaxLength="25" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td class="style218">&nbsp;
                                </td>
                                <td class="style199">
                                    <asp:Label ID="lblCGRelation" runat="server" Text="Relation" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td class="style300">
                                    <asp:DropDownList ID="ddlCGRelation" runat="server" onchange="showTip(this);" CssClass="Editabletxtbox"
                                        onmouseover="OnMouseHover(this);" Width="99%">
                                    </asp:DropDownList>
                                </td>
                                <td class="style231">&nbsp;
                                </td>
                               <td class="style199">
                                    <asp:Label ID="lblCGPhNumber" runat="server" Text="Ph#" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td class="style300">
                                   <telerik:RadMaskedTextBox ID="msktxtCGPhNo" runat="server" onkeypress="AutoSave();" Mask="(###) ###-####" Width="99%" EnableViewState="false" CssClass="Editabletxtbox">
                                    </telerik:RadMaskedTextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:UpdatePanel ID="updtpnlGuarantor" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlGuarantorInfo" runat="server" CssClass="LabelStyleBold" GroupingText="Guarantor Information "
                                Font-Size="Small">
                                <table style="width: 100%">
                                    <tr>
                                        <td class="style209">
                                            <asp:CheckBox ID="chkGuarantorIsPatient" runat="server" AutoPostBack="True" onclick="AutoSave();"
                                                OnCheckedChanged="chkGuarantorIsPatient_CheckedChanged" Text="Guarantor Is Patient" CssClass="spanstyle" />
                                        </td>
                                        <td class="style242">
                                            <asp:Button ID="btnAddGuarantor" runat="server" OnClick="btnAddGuarantor_Click" OnClientClick="return openDemographicswindow();"
                                                Text="Add Guarantor" CssClass="aspresizedbluebutton" />
                                            <asp:Button ID="btnAddGuarantorRefresh" runat="server" CssClass="displaynonestyle" OnClick="btnAddGuarantorRefresh_Click" />
                                            <%--<asp:Button ID="btnUncheckGurantor" runat="server" CssClass="displaynonestyle" OnClick="btnUncheckGurantor_Click" />--%>
                                        </td>
                                        <td class="style209">
                                            <asp:Button ID="btnSelectGaurantor" runat="server" OnClick="btnSelectGaurantor_Click"
                                                OnClientClick="return OpenFindPatientForGuarantor();" Text="Select Guarantor" CssClass="aspresizedbluebutton" />
                                        </td>
                                        <td class="style188">
                                            <asp:Button ID="btnViewGaurantor" runat="server" OnClientClick="return OpenGuarantor();"
                                                Text="View Guarantor" EnableViewState="false" CssClass="aspresizedbluebutton" />
                                        </td>
                                        <td>&nbsp;<asp:HiddenField ID="hdnGuarantorID" runat="server" EnableViewState="false" />
                                            &nbsp;
                                        </td>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblGuarantorLastName" runat="server" Text="Last Name  " Width="60px"
                                                EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td class="style206">
                                            <asp:TextBox ID="txtGuarantorLastName" runat="server" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                onkeypress="return AllowAlphabet(event)" Width="190px" EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td class="style206">&nbsp;
                                        </td>
                                        <td class="style212">
                                            <asp:Label ID="lblGuarantorFirstName" runat="server" Text="First Name" Width="80px"
                                                EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td class="style185">
                                            <asp:TextBox ID="txtGuarantorFirstName" runat="server" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                onkeypress="return AllowAlphabet(event)" Width="190px" EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td class="style185">&nbsp;
                                        </td>
                                        <td class="style212">
                                           <asp:Label ID="lblGuarantorMiddleName" runat="server" Text="Middle Name" Width="45px"
                                                EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td class="style194">
                                            <asp:TextBox ID="txtGuarantorMiddleName" onchange="AutoSave();" onkeypress="return AllowAlphabet(event)"
                                                runat="server" Cssclass="nonEditabletxtbox" Width="190px"  EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td class="style194">&nbsp;
                                        </td>
                                        <td class="style213">
                                            <asp:Label ID="lblGuarantorDOB" runat="server" Text="DOB" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td class="style208" width="170">
                                            <telerik:RadMaskedTextBox ID="dtpGuarantorDOB" runat="server" Mask="##-Lll-####" Cssclass="nonEditabletxtbox"
                                                Width="150px">
                                                <ClientEvents OnValueChanged="PatientDemographicsDateVlidation" />
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
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblGuarantorSex" runat="server" Text="Sex" EnableViewState="False" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGuarantorSex" runat="server" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                Width="190px"  EnableViewState="true">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdnGuarantorSex" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="style212">
                                            <asp:Label ID="lblGuarantorAddress" runat="server" Text="Address Line1" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGuarantorAddress" runat="server" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                Width="190px"  EnableViewState="true"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <%--<td class="style212">
                                        <asp:Label ID="lblGuarantorCity" runat="server" Text="City" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGuarantorCity" runat="server" BackColor="#BFDBFF" Width="190px"
                                            ></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="style213">
                                        <asp:Label ID="lblGuarantorState" runat="server" Text="State" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td width="170">
                                        <asp:DropDownList ID="ddlGuarantorState" runat="server" BackColor="#BFDBFF" onchange="AutoSave();"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>--%>
                                        <td colspan="5">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblGuarantorAddressLine2" runat="server" Text="Address Line2" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGuarantorAddressLine2" runat="server" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                            Width="125px"  EnableViewState="true"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGuarantorCity" runat="server" Text="City" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtGuarantorCity" runat="server" Cssclass="nonEditabletxtbox" Width="110px"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblGuarantorState" runat="server" Text="State" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlGuarantorState" runat="server" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                            Width="100px"  EnableViewState="true">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnGuarantorState" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblGuarantorZipCode" runat="server" Text="Zip Code" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadMaskedTextBox ID="msktxtGuarantorZipCode" Cssclass="nonEditabletxtbox" runat="server"
                                                onchange="AutoSave();" Width="190px" Mask="#####-####">
                                            </telerik:RadMaskedTextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="style212">
                                            <asp:Label ID="lblGuarantorHome" runat="server" Text="Home Ph #" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadMaskedTextBox ID="msktxtGuarantorHomeNo" runat="server" Cssclass="nonEditabletxtbox"
                                                onchange="AutoSave();" Width="190px" Mask="(###) ###-####">
                                                <DisabledStyle BackColor="#BFDBFF" />
                                            </telerik:RadMaskedTextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="style212">
                                            <asp:Label ID="lblGuarantorCellPhone" runat="server" Text="Cell Ph#" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadMaskedTextBox ID="msktxtGuarantorCellNo" Cssclass="nonEditabletxtbox" onchange="AutoSave();"
                                                Width="190px" runat="server" Mask="(###) ###-####">
                                                <ReadOnlyStyle BackColor="#BFDBFF" />
                                            </telerik:RadMaskedTextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td class="style213">
                                            <asp:Label ID="lblGuarantorRelationship" runat="server" Text="Rel.to Patient" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td width="170">
                                            <asp:DropDownList ID="ddlGuarantorRelationship" runat="server" CssClass="Editabletxtbox"
                                                onmouseover="OnMouseHover(this);" onchange="showTip(this);" Width="150px"  EnableViewState="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">
                                            <asp:Label ID="lblGuaEmail" runat="server" Text="Guarantor Email" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGuaEmail" onchange="AutoSave();" runat="server" OnKeyPress="change(this);"
                                                autocomplete="off" Width="190px" MaxLength="100" EnableViewState="true" Cssclass="nonEditabletxtbox" Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div>
                    <asp:Panel ID="pnlEmergencyInfo" runat="server" CssClass="LabelStyleBold" GroupingText="Emergency Contact Information ">
                        <table style="width: 100%">
                            <tr>
                                <td class="style199">
                                    <asp:Label ID="lblEmerLastName" runat="server" Text="Last Name  " Width="70px" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td width="197">
                                    <asp:TextBox ID="txtEmerLastName" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        Width="197px" MaxLength="75" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="style164">
                                    <asp:Label ID="lblEmerFirstName" runat="server" Text="First Name" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td width="197">
                                    <asp:TextBox ID="txtEmerFirstName" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        Width="197px" MaxLength="75" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td class="style218">&nbsp;
                                </td>
                                <td class="style194">
                                    <asp:Label ID="lblEmerMiddleName" runat="server" Text="Middle Name  " Width="75px" CssClass="spanstyle"
                                        EnableViewState="false"></asp:Label>
                                </td>
                                <td width="197">
                                    <asp:TextBox ID="txtEmerMiddleName" runat="server" onchange="AutoSave();" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                        Width="197px" MaxLength="25" EnableViewState="false"></asp:TextBox>
                                </td>
                                <td class="style233">&nbsp;
                                </td>
                                <td class="style240">
                                    <asp:Label ID="lblEmerDOB" runat="server" Text="DOB" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadMaskedTextBox ID="dtpEmerDOB" runat="server" Mask="##-Lll-####" onchange="AutoSave();" CssClass="Editabletxtbox"
                                        onkeypress="AutoSave();" Width="166px">
                                        <ClientEvents OnValueChanged="PatientDemographicsDateVlidation" />
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
                            <tr>
                                <td class="style199">
                                    <asp:Label ID="lblSex" runat="server" Text="Sex" EnableViewState="False" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td width="197">
                                    <asp:DropDownList ID="ddlEmerSex" onchange="AutoSave();" runat="server" Width="197px" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="style164">
                                    <asp:Label ID="lblEmerAddress" runat="server" Text="Address" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td width="197">
                                    <asp:TextBox ID="txtEmerAddress" onchange="AutoSave();" runat="server" onkeypress="AutoSave();"
                                        Width="197px" MaxLength="55" EnableViewState="false" CssClass="Editabletxtbox"></asp:TextBox>
                                </td>
                                <td class="style218">&nbsp;
                                </td>
                                <td class="style194">
                                    <asp:Label ID="lblEmerCity" runat="server" Text="City" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmerCity" runat="server" onchange="AutoSave();" onkeypress="AutoSave();"
                                        Width="197px" MaxLength="35" EnableViewState="false" CssClass="Editabletxtbox"></asp:TextBox>
                                </td>
                                <td class="style233">&nbsp;
                                </td>
                                <td class="style240">
                                    <asp:Label ID="lblEmerState" runat="server" Text="State" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEmerState" runat="server" onchange="AutoSave();" Width="166px" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="style199">
                                    <asp:Label ID="lblEmerZipCode" runat="server" Text="Zip Code" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td width="197">
                                    <telerik:RadMaskedTextBox ID="msktxtEmerZipCode" runat="server" onkeypress="AutoSave();"
                                        Width="197px" Mask="#####-####" EnableViewState="false" CssClass="Editabletxtbox">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td class="style164">
                                    <asp:Label ID="lblEmerHomeNo" runat="server" Text="Home Ph #" Width="70px" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td width="197">
                                    <telerik:RadMaskedTextBox ID="msktxtEmerHome" runat="server" onkeypress="AutoSave();"
                                        Mask="(###) ###-####" Width="197px" EnableViewState="false" CssClass="Editabletxtbox">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                <td class="style218">&nbsp;
                                </td>
                                <td class="style164">
                                    <asp:Label ID="lblEmerCellNo" runat="server" Text="Cell Ph #" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadMaskedTextBox ID="msktxtEmerCell" runat="server" onchange="AutoSave();"
                                        onkeypress="AutoSave();" Width="197px" Mask="(###) ###-####" EnableViewState="false" CssClass="Editabletxtbox">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                <td class="style233">&nbsp;
                                </td>
                                <td class="style240">
                                    <asp:Label ID="lblRelation" runat="server" Text="Rel.to Patient" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRelation" runat="server" onchange="showTip(this);" onmouseover="OnMouseHover(this);" CssClass="Editabletxtbox"
                                        Width="166px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <asp:Panel ID="pnlAccountInfo" runat="server" CssClass="LabelStyleBold" GroupingText="Account Information ">
                        <table>
                            <tr>
                                <td class="auto-style6">
                                    <asp:Label ID="lblAccCreationDate" runat="server" Text="Acc.Creation Date" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="auto-style7">
                                    <asp:TextBox ID="dtpAccCreationDate" runat="server" CssClass="nonEditabletxtbox" 
                                         ReadOnly="True" Width="150px"></asp:TextBox>
                                </td>
                               
                                <td class="auto-style8">
                                    <asp:Label ID="lblNoofPolicies" runat="server" Text="# of Ins Policies" CssClass="spanstyle" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="auto-style9">
                                   <asp:TextBox ID="txtNoofPolicies" runat="server" CssClass="nonEditabletxtbox" ReadOnly="True" Width="85px"></asp:TextBox>
                                                </td>
                                                <td class="auto-style11">
                                                    <asp:Label ID="lblPatientSignature" runat="server" CssClass="spanstyle" Text="Sign on File" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPatientSignature" runat="server" onchange="AutoSave();" CssClass="Editabletxtbox"
                                                        Width="60px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="style302">
                                                    <asp:Label ID="lblDemoStatus" runat="server" CssClass="spanstyle" Text="Demo Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlDemoStatus" runat="server" onchange="AutoSave();" Width="160px" CssClass="Editabletxtbox">
                                                    </asp:DropDownList>
                                                </td>
                                            
                                <td class="auto-style10">
                                    <asp:Label ID="lblAccountStatus" runat="server" CssClass="spanstyle" Text="Acc. Status" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="auto-style9">
                                    <asp:DropDownList ID="ddlAccountStatus" runat="server" onchange="AutoSave();" Width="180px" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReferredToCollection" CssClass="spanstyle" runat="server" EnableViewState="false" Text="Ref to Collection"></asp:Label>
                                </td>
                                <td class="style298">
                                    <asp:DropDownList ID="ddlReferredToCollection" runat="server" onchange="AutoSave();" Width="80px" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                                
                                     <td class="style270">
                                                <asp:Label ID="lblHumanType" CssClass="spanstyle" runat="server" EnableViewState="false" Text="Patient Type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboHumanType" runat="server" onchange="AutoSave();" Width="95px" CssClass="Editabletxtbox">
                                                </asp:DropDownList>
                                            </td>
                                
                                                <td class="auto-style11">
                                                    <asp:Label ID="lblDeclaredBankruptcy" CssClass="spanstyle" runat="server" EnableViewState="false" Text="Declared Bankruptcy"></asp:Label>
                                                </td>
                                                <td class="style279">
                                                    <asp:DropDownList ID="ddlDeclaredBankruptcy" runat="server" onchange="AutoSave();" CssClass="Editabletxtbox" style="width:60px;">
                                                    </asp:DropDownList>
                                                </td>
                                               
                                                <td class="style277">
                                                    <asp:Label ID="lblPatientStatementFormat" CssClass="spanstyle" runat="server" Text="Pat. Stmt" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td class="style277">
                                                    <asp:TextBox ID="txtPatientStatementFormat" runat="server" MaxLength="50" onkeypress="AutoSave();" CssClass="Editabletxtbox"
                                                        onchange="AutoSave();" Width="155px" EnableViewState="false"></asp:TextBox>
                                                </td>
                                            
                               
                                <td class="style170">
                                    <asp:Label ID="lblDefaultFacility" CssClass="spanstyle" runat="server" Text="Def. Fac" EnableViewState="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDefaultFacility" runat="server" onchange="AutoSave();" Width="180px" CssClass="Editabletxtbox">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                          
                            <tr>
                        <td class="style4" colspan="7" valign="bottom" style="margin: 0px; padding: 0px">
                            <asp:Panel ID="pnlPatientInsuranceList" runat="server" GroupingText="Patient Insurance List" Height="320px"
                                Width="164%" Font-Bold="True" CssClass="LabelStyleBold">
                              <table>
                                  <tr>
                                    <td style="width: 25%;">
                                        <span  class="MandLabelstyle">Priority</span><span class="manredforstar">* &nbsp;</span> 
                                        <span  class="MandLabelstyle">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                        <asp:RadioButton ID="rdbPRI" Checked="false" Text="Pri." runat="server" GroupName="InsuranceType"  CssClass="Editabletxtbox" onchange="AutoSave();" />
                                        <asp:RadioButton ID="rdbSEC" Checked="false" Text="Sec." runat="server" GroupName="InsuranceType"  CssClass="Editabletxtbox" onchange="AutoSave();" />
                                        <asp:RadioButton ID="rdbTER" Checked="false" Text="Ter." runat="server" GroupName="InsuranceType"  CssClass="Editabletxtbox" onchange="AutoSave();" />
                                        
                                    </td>
                                      <td style="width: 40%; text-align: left;">
                                        <span  class="MandLabelstyle">Plan</span><span class="manredforstar">* &nbsp;</span> 
                                       <asp:TextBox ID="txtPlanSearch" CssClass="spanstyle" runat="server" Width="72%" onchange="AutoSave();" ></asp:TextBox>
                                       <img id="imgClearplanText" src="Resources/Delete-Blue.png" runat="server" alt="X" title="Click to clear the text field." style="width:10px;margin-top:-5px;"/>
                                       </td>
                                      <td>
                                          </td>
                                            <td style="width: 35%;text-align: left;">
                                        <span  class="spanstyle" id="lblSpecifyOther">Specify Other</span> 
                                       <asp:TextBox ID="txtSpecify" CssClass="spanstyle" runat="server" Width="70%" onchange="AutoSave();" ></asp:TextBox>
                                       </td>                          
                                   </tr>
                                  </table>
                                <table>
                                  <tr>
                                       <td style="width: 25%;">
                                        <span  class="MandLabelstyle">Policy holder Id</span><span class="manredforstar">*</span> 
                                       <asp:TextBox ID="txtPolicyholderid" CssClass="spanstyle" runat="server" Width="55%" MaxLength="25" onchange="AutoSave();" ></asp:TextBox>
                                       </td>
                                    <td style="width: 25%;">
                                          <span  class="MandLabelstyle">Rel.to Patient</span><span class="manredforstar">*</span> 
                                       <asp:DropDownList ID="ddlPatientRelation" onchange="PatientRelationchange();AutoSave();" runat="server" style="width:146px"></asp:DropDownList>
                                    </td>
                                   <td style="width: 36%;text-align: left;">
                                        <span id="lblSelectInsured"  class="spanstyle">Select Insured</span>
                                       <asp:TextBox ID="txtSelectinsured" CssClass="spanstyle" runat="server" Width="68%" onchange="AutoSave();" ></asp:TextBox>
                                       <img id="imginsuredText" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="width:10px;margin-top:-5px;"/>
                                       </td>
                                   <td style="width: 15%;text-align: center;">
                                         <input type="button" id="btnaddins" runat="server" value="Add New Insured" class="aspresizedbluebutton" onclick="OpenAddinsured();"/>
                
                                      </td>
                                    
                                   </tr>
                                  <tr>
                                    <td style="width: 25%;">
                                          <span  class="spanstyle">Eff. Start Date&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                         <span style="width: 146px;">
                                         <telerik:RadMaskedTextBox ID="txtStartdate" runat="server" Mask="##-Lll-####" Width="145px" onchange="AutoSave(); "  >
                                                    <ClientEvents OnValueChanged="PatientDemographicsDateVlidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox></span>
                                    </td>
                                   <td style="width: 20%; text-align: left; ">
                                        <span  class="spanstyle">Eff. End Date &nbsp;</span>
                                         
                                       <telerik:RadMaskedTextBox ID="txtEnddate" runat="server" Mask="##-Lll-####" Width="146px" onchange="AutoSave(); "  >
                                                    <ClientEvents OnValueChanged="PatientDemographicsDateVlidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                             
                                   </td>
                                      <td>
                                       <span  class="spanstyle">Status </span>
                                        <asp:RadioButton ID="rdStatusactive" Checked="true" Text="Active" runat="server" GroupName="Status" CssClass="Editabletxtbox"  onchange="AutoSave();"/>
                                        <asp:RadioButton ID="rdStatusinactive" Checked="false" Text="Inactive" runat="server" GroupName="Status"  CssClass="Editabletxtbox" onchange="AutoSave();" />
                                      
                                       </td>
                                                                     
                                   </tr>
                                    </table>
                                <table>
                                    <tr>
                                        <td style="width: 10%;">
                                         <span class="spanstyle">PCP</span>
                                         </td>
                                        <td style="width:51%; text-align: left;">
                                        <asp:TextBox ID="txtProviderSearch" runat="server"  data-phy-id="0"  data-phy-details="" Rows="2" TextMode="MultiLine" placeholder="Type minimum 3 characters of Last or First name or Facility and follow it by a space.."  style="width:110%;resize:none"  ></asp:TextBox> 
                                        </td>
                                        <td style="width: 1%;text-align: right;">
                                        <img id="imgClearProviderText" runat="server" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." onclick="return ProviderSearchclear();" style="position: absolute; margin-left: 47px; cursor: pointer; width: 10px; height: 12px;margin-top: -6px;" />
                                        </td>
                                         <td style="width: 25%;text-align: left;">
                                            <img id="imgEditProvider" runat="server" src="Resources/edit.gif" alt="X" title="Click to edit the text field." onclick="return EditProviderDetails();" style="position: absolute; margin-left: 60px; margin-top: -8px ; cursor: pointer; width: 13px; height: 15px;" />
                                        <%--<input type="button" id="btnAddPhysician" value="Add Provider" onclick="return OpenAddPhysician();" style="width: 120px; left: 2000px !important;" class="aspresizedbluebutton" />--%>
                                        </td>
                                        <td style="width: 10%;text-align: right;">
                                         <input type="button" id="btnAdd" onclick="btnaddinsured(this)" value="Add"   class="aspresizedgreenbutton" />
                                            </td>
                                             <td style="width: 10%;text-align: center;">
                                         <input type="button" id="btnClearAll" onclick="btnclearinsured(true)" value="Clear All" style="margin-right: 13px;"  class="aspresizedredbutton" />
                
                                      </td>
                                        </tr>
                                      </table>
                                 <table>
                                  <tr>
                                            <td style="width:20%;" class="style24">
                                                <asp:CheckBox ID="chkActiveStatus" runat="server"
                                                    EnableViewState="false" CssClass="spanstyle"
                                                    Text="Show Active Only."  onchange="DisplayActiveInsurance();" Checked="true"/>
                                            </td>
                                      </tr>
                                    <tr></tr>
                                  </table>
                                <div style="width: 100%;height:120px;overflow:scroll;">
                                   <table id="tblpolicyinfo" class="table table-bordered Gridbodystyle" >
                        <thead class="Gridheaderstyle">
                            <tr style="position:sticky;top:0;">
                                <th class="Gridheaderstyle" style="width: 5%; text-align: center">Edit</th>
                                <th class="Gridheaderstyle" style="width: 10%; text-align: center">Priority</th>
                                <th class="Gridheaderstyle" style="width: 10%; text-align: center">Plan Name</th>
                                <th class="Gridheaderstyle" style="width: 7%; text-align: center">Policy Holder Id</th>
                                <th class="Gridheaderstyle" style="width: 5%; text-align: center">Rel.To Patient</th>
                                <th class="Gridheaderstyle" style="width: 12%; text-align: center">Insured Name</th>
                                <th class="Gridheaderstyle" style="width: 10%; text-align: center">PCP</th>
                                <th class="Gridheaderstyle" style="width: 7%; text-align: center">Specify Other</th>
                                <th class="Gridheaderstyle" style="width: 7%; text-align: center">Eff.Start Date</th>
                                <th class="Gridheaderstyle" style="width: 7%; text-align: center">Eff.End Date</th>
                                <th class="Gridheaderstyle" style="width: 5%; text-align: center">Status</th>
                            </tr>
                        </thead>
                        <tbody id="tbodupolicyinfo" class="Gridbodystyle">
                        </tbody>
                    </table>
                                    </div>
                            </asp:Panel>
                        </td>
                    </tr>
                           
                            <tr>
                                <td class="style282">
                                    <%--<asp:Label ID="lblRecentVerification" runat="server" Text="Recent EV Status" EnableViewState="false"></asp:Label>--%>
                                    <asp:Label ID="lblRecentScannedStatus0" CssClass="spanstyle" runat="server" EnableViewState="false" Text="Recent Scan Status"></asp:Label>
                                </td>
                                <td colspan="4">
                                   <%-- <asp:TextBox ID="txtRecentVerificationStatus" runat="server" BackColor="#BFDBFF"
                                        BorderColor="Black" BorderWidth="1px" ReadOnly="True" TextMode="MultiLine" CssClass="MultiLineTextBox"
                                        Width="98%" Height="80px"></asp:TextBox>--%>
                                    <asp:TextBox ID="txtRecentScannedStatus" runat="server"   CssClass="MultiLineTextBox nonEditabletxtbox" Height="80px" ReadOnly="True" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                </td>
                                <td class="style241">&nbsp;
                                </td>
                                <td class="style299">
                                    <asp:Label ID="lblRecentVerification" CssClass="spanstyle" runat="server" EnableViewState="false" Text="Recent EV Status"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtRecentVerificationStatus" runat="server"  CssClass="MultiLineTextBox nonEditabletxtbox" Height="80px" ReadOnly="True" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <asp:Panel ID="pnlAdditionalInformation" CssClass="LabelStyleBold" runat="server" GroupingText="Immunization Registry Information"
                     Width="1067px">
                    <table style="width: 100%;">
                        <tr>
                            <td class="auto-style22">
                                <asp:Label ID="lblMothersMaidenName" runat="server" Text="Mother’s maiden name " CssClass="spanstyle"
                                    Width="166px"></asp:Label>
                            </td>
                            <td class="auto-style24">
                                <asp:TextBox ID="txtMothersMaidenName" runat="server" Style="margin-left: 0px" Width="178px" CssClass="Editabletxtbox"
                                    onkeypress="SaveEnable();"></asp:TextBox>
                            </td>
                            <td class="auto-style23">
                                <asp:Label ID="lblImmunizationRegistryStatus" runat="server" Text="Immunization registry status " CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="auto-style24">
                                <asp:DropDownList ID="ddlImmunizationRegStatus" runat="server" Height="22px" Width="187px" CssClass="Editabletxtbox"
                                    onchange="SaveEnable();">
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style24">
                                <asp:Label ID="lblPublicityCode" runat="server" Text="Reminder/Recall" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="auto-style24">
                                <asp:DropDownList ID="ddlPublicityCode" runat="server" Height="22px" Width="170px" CssClass="Editabletxtbox"
                                    onchange="SaveEnable();">
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td class="style307">
                                <asp:Label ID="lblDataSharingPreference" runat="server" Text="Data Protection Required? " CssClass="spanstyle"
                                    Width="166px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDataSharingPreference" runat="server" Height="22px" Width="180px" CssClass="Editabletxtbox"
                                    onchange="SaveEnable();">
                                </asp:DropDownList>
                            </td>
                            <td class="style308">
                                <asp:Label ID="lblBirthIndicator" runat="server" Text="Is Patient a Single Child? " CssClass="spanstyle"></asp:Label>
                            </td>
                            <td>
                                <%--<asp:DropDownList ID="ddlBirthIndicator" runat="server" Height="22px" Width="187px" CssClass="Editabletxtbox"
                                    onchange="ddlBirthIndicator_Change(this);">
                                </asp:DropDownList>--%>
                                <asp:DropDownList ID="ddlBirthIndicator" runat="server" Height="22px" Width="187px" CssClass="Editabletxtbox"
                                    onchange="SaveEnable();">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblBirthOrder" runat="server" Text="Birth Order " CssClass="spanstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBirthOrder" runat="server" Height="22px" Width="170px" CssClass="Editabletxtbox"
                                    onchange="SaveEnable();">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                    <asp:UpdatePanel ID="UpdatePanelForMessageInfo" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMessageInfo" runat="server" GroupingText="Patient Communication Details"
                                CssClass="LabelStyleBold" >
                                <table>
                                    <tr>
                                        <td rowspan="2" class="style301">
                                            <asp:Label ID="lblMessageDescription" runat="server" Text="Message Description" EnableViewState="false" CssClass="spanstyle" ></asp:Label>
                                        </td>
                                        <td rowspan="2">&nbsp;<asp:DropDownList ID="ddlMessageDescription" onchange="change(this);" runat="server" CssClass="Editabletxtbox"
                                            Width="176px" style="margin-top: -13px;">
                                        </asp:DropDownList>
                                        </td>
                                        <td class="style302" rowspan="2">
                                            <asp:Label ID="lblMessageNotes" runat="server" Text="Message" EnableViewState="False" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td class="style268" rowspan="2">
                                            <DLC:DLC ID="txtNotes" runat="server" Enable="True" TextboxHeight="60px" TextboxWidth="360px" TextboxMaxLength="300"
                                                Value="MESSAGE NOTES" EnableViewState="false"  />
                                            &nbsp;
                                        </td>
                                         <td rowspan="2" class="style302"> 
                                            <asp:Label ID="lblassignedto" runat="server" Text="Assigned To" mand="Yes" style="font-weight: normal;"></asp:Label>
                                        </td>
                                        <td rowspan="2">&nbsp;<asp:DropDownList ID="ddlAssignedTo" onchange="change(this);" runat="server" CssClass="Editabletxtbox"  style="margin-top: -20px;"
                                            Width="119px">
                                        </asp:DropDownList>
                                        </td>


                                        <td rowspan="2">
                                            <asp:CheckBox ID="chkshowall" runat="server" Text="Show All" onclick="chkShowAllChange();" Width="71px" CssClass="Editabletxtbox"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style266">
                                            <asp:Button ID="btnAddMessage" runat="server" OnClick="btnAdd_Click" Text="Add" Width="52px" CssClass="aspresizedgreenbutton"
                                                OnClientClick="return AddMessageDemo();" style="margin-left: 12px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style168" colspan="2">&nbsp;
                                        </td>
                                        <td class="style302">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div>
                    <asp:Panel ID="Buttons" runat="server">
                        <table style="width: 100%; margin-right: 0px;">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="style269">&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnUploadDocuments" runat="server" Text="Upload Documents" Width="100px" CssClass="aspresizedbluebutton"
                                        OnClientClick="return OpenIndexing();" EnableViewState="false" Visible="false" />
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btnViewMessage" runat="server" Text="View Message" Width="100px"  CssClass="aspresizedbluebutton"
                                        OnClientClick="return OpenViewMessage();" EnableViewState="false" Style="display: none ! important;" />
                                </td>
                                 <td>
                                    <asp:Button ID="btnAuthorization" runat="server"  Text="Manage Authorization" OnClientClick="openAuthorization();" Width="200px" CssClass="aspresizedbluebutton"   Style="display: none!important;" />
                                    </td>
                                <td></td>
                                <td align="left" colspan="2">
                                    <asp:Button ID="btnViewUpdateInsurance" runat="server" AccessKey="U" OnClick="btnViewUpdateInsurance_Click" CssClass="aspresizedbluebutton"
                                        OnClientClick="return openPatInsurancewindow();" Text="View /Update Insurance Policies"
                                        Width="239px" style="margin-left:357px; " Visible ="false"/>
                                </td>
                                <td class="style159">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="150px" OnClientClick="return PatientInformationValidation();" CssClass="aspresizedgreenbutton"
                                        AccessKey="S" OnClick="btnSave_Click" />
                                </td>
                                <td class="style188">
                                    <asp:Button ID="btnClose" runat="server" AccessKey="C" Text="Close" Width="150px" CssClass="aspresizedredbutton"
                                        OnClientClick="return NewCloseWindow();" EnableViewState="false" />
                                </td>
                            </tr>
                            
                        </table>
                       
                    </asp:Panel>
                </div>
                
                
                <asp:HiddenField ID="txtPCPProviderTag" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPatientID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnHumanId" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPatientType" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnSaveFlag" runat="server" EnableViewState="false" />
                <asp:Button ID="hdnBtnLoadInsurance" Style="display: none" runat="server" OnClick="hdnBtnLoadInsurance_Click" />
                <asp:HiddenField ID="hdnCurrentCompletedCount" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnCurrentCompletedList" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="LocalDate" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="LocalTimeZone" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnType" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnParentScreen" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPrimInsPlanID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnSecInsPlanID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnbInsuredHuman" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnHumanIDList" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPbClick" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPhysicianID" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnAge" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnFilePath" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnFromAddPatient" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnFirstDateAndTime" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnNextDateAndTime" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnEndLocalTime" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnRaceTag" runat="server" EnableViewState="false" />
                 <asp:HiddenField ID="hdnGranularTag" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnimgurl" runat="server" EnableViewState="false" />
                 <asp:HiddenField ID="HdnGranular" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnDemoAccNumber" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnAccNoFromViewReport" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnGuarantorIdForView" runat="server" EnableViewState="false" />
                  <asp:HiddenField ID="hdnSexualOrientationSpecify" runat="server" EnableViewState="false" />
                  <asp:HiddenField ID="hdnGenderIdentity" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="HiddenPatientName" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnpatinsuredid" runat="server" EnableViewState="false" />
                 <asp:HiddenField ID="Hdnsortorder" runat="server" EnableViewState="false" />
                <asp:Button ID="btnFindpatientClick" runat="server" Text="Button" Style="display: none"
                    OnClick="btnFindpatientClick_Click" />
                <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return NewCloseWindow();" />
                <asp:HiddenField ID="hdnIsPopUp" runat="server" />
                <asp:HiddenField ID="hdnstatus" runat="server" />
                <asp:HiddenField ID="hdncancel" runat="server" />
                <asp:HiddenField ID="hdnMessageType" runat="server" />
                <asp:HiddenField ID="hdnYesNoMessage" runat="server" />
                <asp:HiddenField ID="hdnBirthOrder" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnFacilityName" runat="server" />
                 <asp:HiddenField ID="hdnProviderId" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnCategory" runat="server" EnableViewState="false" />               
                <br />
            </div>
            <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                <asp:Panel ID="Panel2" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="Label2" Text="" runat="server"></asp:Label></center>
                    <br />
                    <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                        alt="Loading..." />
                    <br />
                </asp:Panel>
            </div>
            <asp:Button ID="btnsaveDuplicate" runat="server" OnClick="btnCheckDuplicate_Click"
                                Text="Button" style="display: none" />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                
               <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
              <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
                <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSDemographics.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                 <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                
                  <link href="CSS/style.css" rel="stylesheet" type="text/css" />
            </asp:PlaceHolder>
        </form>
        <script type="text/javascript">
            //CAP-70 Create a function for storing a value in the hidden field.
            function patientSexChanged() {
                $('#ctl00_C5POBody_HiddenPatientSex').val($('#ctl00_C5POBody_ddlPatientsex').val());
                Copy('SEX');
            }
        </script>
    </body>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
