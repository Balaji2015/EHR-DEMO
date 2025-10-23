<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPhoneEncounter.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmPhoneEncounter"  ValidateRequest="false"  %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Phone Encounter</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <style type="text/css">
        #frmPhoneEncounters {
        }

        .displayNone {
            display: none;
        }

        /*.BackcolorDisable {
            background-color: #BFDBFF;
        }*/

        /*input[disabled], textarea[disabled], select[disabled] {
            border: 1px solid;
            background-color: #BFDBFF !important;
        }*/
    </style>
 <%--   <link href="CSS/style.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="Stylesheet" />
    <link rel="stylesheet" href="CSS/datetimepicker.css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="GetWin();">
    <form id="frmPhoneEncounters" runat="server" style="background-color: White">
            <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="YesNoCancelWindow" runat="server" Behaviors="Close" Title=""
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
              </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%" Width="98%">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%">
                           <table style="width: 100%" id="tblPhEnc" runat="server">
                                <tr>
                                    <td style="width: 100%" align="center" valign="top" id="tdPhEnc" runat="server"><%--   <asp:Panel ID="pnlPhoneEncounter" runat="server" GroupingText="Phone Encounter" Font-Size="Small"
                                            BackColor="White">--%>
                                          <fieldset class="panel panel-success"  style="border-color: black !important;text-align:left;padding:10px;vertical-align:top; width: 966px; height: 290px;" id="fldsetPhEnc" runat="server">
                                            <legend style="font-size: small; font-weight: bold; width: 120px;height:11px; border-bottom: none;vertical-align:top;">Phone Encounter</legend>
                                   
                                            <table style="width: 100%;  height: 220px;"  id="tbl" runat="server">
                                                <tr>
                                                    <td style="font-size: small">&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblPatientName" runat="server" Text="Patient Name" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                       <%-- <telerik:RadTextBox ID="txtPatientName1" runat="server" BorderColor="Black" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>--%>
                                                         <asp:TextBox ID="txtPatientName" Width="162px" runat="server" ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                       
                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblPatientDOB" runat="server" Text="Patient DOB" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <%--<telerik:RadTextBox ID="txtPatientDOB1" runat="server" BorderColor="Black" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>--%>
                                                        <asp:TextBox ID="txtPatientDOB" Width="162px" runat="server"  ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblPatientSex" runat="server" Text="Patient Sex" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <%--<telerik:RadTextBox ID="txtPatientSex1" runat="server" BorderColor="Black" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>--%>
                                                        <asp:TextBox ID="txtPatientSex" Width="162px" runat="server" ReadOnly="True" CssClass="nonEditabletxtbox" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small">&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblAccountNumber" runat="server" Text="Account#" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <%--<telerik:RadTextBox ID="txtAccountNumber1" runat="server" BorderColor="Black" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>--%>
                                                        <asp:TextBox ID="txtAccountNumber" Width="162px" runat="server"  ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>
                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblHomePhno" runat="server" Text="Home Phone #" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadMaskedTextBox ID="mskHomePhone" runat="server"  Mask="(###) ###-####" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblCellPhno" runat="server" Text="Cell Phone #" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadMaskedTextBox ID="mskCellPhone" runat="server" Mask="(###) ###-####" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small">&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblWorkPhone" runat="server" Text="Work Phone#" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadMaskedTextBox ID="mskWorkPhno" runat="server"  Mask="(###) ###-####" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadMaskedTextBox>
                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblExtensionNumber" runat="server" Text="Extension #" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                      <%--  <telerik:RadTextBox ID="txtExtension1" runat="server"  BorderColor="Black" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                        </telerik:RadTextBox>--%>
                                                        <asp:TextBox ID="txtExtension" Width="162px" runat="server"  ReadOnly="True" CssClass="nonEditabletxtbox"></asp:TextBox>


                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblcabture" runat="server" Text="Call Date*"  mand="Yes">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <%--<input type="text" id="txtCalldate" runat="server" onclick ="EnableSave();" onkeyup="EnableSave();" onkeydown="EnableSave();" onkeypress="EnableSave();" diabled="disabled" style="width: 73%;" />--%>
                                                        <input type="text" id="txtCalldate" runat="server"  style="width: 160px;"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="font-size: small">&nbsp;&nbsp;&nbsp;
                                                            <%--<asp:Label ID="lblCallSpokenTo" runat="server" Text="Call Spoken To"></asp:Label>--%>
                                                        <asp:Label ID="lblCallSpokenTo" runat="server" Text="Relationship" CssClass="spanstyle" ></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <select id="cboCallSpokenTo" runat="server" onchange="cboCallSpokenTo_SelectedIndexChanged();" style="width: 160px;" >
                                                            <%--style="border: 1px solid #ccc; border-radius: 3px; margin-left: 4px;width:156px;"--%>
                                                            <option value=""></option>
                                                            <option value="Self">Self</option>
                                                            <option value="Spouse">Spouse</option>
                                                            <option value="Father">Father</option>
                                                            <option value="Mother">Mother</option>
                                                            <option value="Brother">Brother</option>
                                                            <option value="Sister">Sister</option>
                                                            <option value="Son">Son</option>
                                                            <option value="Daughter">Daughter</option>
                                                            <option value="Aunt">Aunt</option>
                                                            <option value="Uncle">Uncle</option>
                                                            <option value="Grand Father">Grand Father</option>
                                                            <option value="Grand Mother">Grand Mother</option>
                                                            <option value="Other">Other</option>
                                                            <option value="Nephew">Nephew</option>
                                                            <option value="Niece">Niece</option>
                                                            <option value="Grand Son">Grand Son</option>
                                                            <option value="Grand Daughter">Grand Daughter</option>
                                                            <option value="Friend">Friend</option>
                                                            <option value="PCP">PCP</option>
                                                            <option value="SPL">SPL</option>
                                                        </select>
                                                    </td>
                                                    <td style="font-size: small">
                                                        <%--<asp:Label ID="lblCallerName" runat="server" Text="Caller Name"></asp:Label>--%>
                                                        <asp:Label ID="lblCallerName" runat="server" Text="Call Spoken To*" mand="Yes"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <telerik:RadTextBox ID="txtCallerName" runat="server" BackColor="#BFDBFF"  CssClass="Editabletxtbox">
                                                            <DisabledStyle Resize="None" />
                                                            <InvalidStyle Resize="None" />
                                                            <HoveredStyle Resize="None" />
                                                            <ReadOnlyStyle Resize="None" />
                                                            <EmptyMessageStyle Resize="None" />
                                                            <ClientEvents OnFocus="txtCallerName_OnFocus" OnMouseOut="txtCallerName_OnMouseOut" OnMouseOver="txtCallerName_OnMouseOver" />
                                                            <FocusedStyle Resize="None" />
                                                            <EnabledStyle Resize="None" />
                                                        </telerik:RadTextBox>
                                                    </td>
                                                    <td style="font-size: small">
                                                        <asp:Label ID="lblCallDuration" runat="server" Text="Call Duration" CssClass="Editabletxtbox"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadTextBox ID="txtCallHrs" runat="server" MaxLength="2" Width="50px" AutoCompleteType="Disabled" oncopy="return false" onpaste="return false"  oncut="return false" CssClass="Editabletxtbox">
                                                                        <DisabledStyle Resize="None" />
                                                                        <InvalidStyle Resize="None" />
                                                                        <HoveredStyle Resize="None" />
                                                                        <ReadOnlyStyle Resize="None" />
                                                                        <EmptyMessageStyle Resize="None" />
                                                                        <ClientEvents OnKeyPress="txtCallHrs_OnKeyPress" />
                                                                        <FocusedStyle Resize="None" />
                                                                        <EnabledStyle Resize="None" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCallHrs" runat="server" Text="Hrs" CssClass="Editabletxtbox" ></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTextBox ID="txtCallMins" runat="server"  MaxLength="2" Width="37px" AutoCompleteType="Disabled" oncopy="return false" onpaste="return false" oncut="return false" CssClass="Editabletxtbox">
                                                                        <DisabledStyle Resize="None" />
                                                                        <InvalidStyle Resize="None" />
                                                                        <HoveredStyle Resize="None" />
                                                                        <ReadOnlyStyle Resize="None" />
                                                                        <EmptyMessageStyle Resize="None" />
                                                                        <ClientEvents OnKeyPress="txtCallMins_OnKeyPress" />
                                                                        <FocusedStyle Resize="None" />
                                                                        <EnabledStyle Resize="None" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCallMins" runat="server" Text="Min" CssClass="Editabletxtbox"></asp:Label>
                                                                </td>
                                                                <%-- Do not delte the below element with id="donotDisplaytxt" BUGID:44922 --%>
                                                                 <td>
                                                                     <input type="text" id="donotDisplaytxt" style="width: 0px;height:0px;border-color:white;border-top-width:0px;border-bottom-width:0px;border-left-width: 0px;" onclick="return false;"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td style="font-size: small;">&nbsp;&nbsp;&nbsp;
                                                                                    <asp:Label ID="lblPlan" runat="server" Text="Plan" Width="100px" CssClass="Editabletxtbox" ></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <DLC:DLC ID="DLC" runat="server" Enable="True" style="resize: none; padding: 2px; width: 25px; " 
                                                                                    TextboxHeight="95px" TextboxWidth="756px" TextboxMaxLength="40000" Value="TREATMENT PLAN NOTES" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <%--  </asp:Panel>--%></td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%" align="center">
                                        <asp:Label ID="lblPassword" Width="105px" runat="server" Text="Password*"  CssClass="MandLabelstyle" mand="Yes"></asp:Label>
                                    </td>
                                    <td style="width: 5%">
                                        <telerik:RadTextBox ID="txtPassword" Width="162px" runat="server" TextMode="Password" CssClass="Editabletxtbox">
                                            <%-- Style="margin-right: 1px"--%>
                                            <ClientEvents OnKeyPress="txtPassword_OnKeyPress" />
                                            <FocusedStyle Resize="None" />
                                        </telerik:RadTextBox>
                                    </td>
                                     <%-- Changed OnClientClicked="btnsaved_Clicked" to OnClientClicked="btnsave_Clicked"-- validation missing in btnsaved_clicked BUGID:44922 --%>
                                    <td align="right" style="width: 90%" >&nbsp;<telerik:RadButton ID="btnSave" OnClientClicked="btnSave_Clicked" runat="server" OnClick="btnSave_Click" ButtonType="LinkButton" CssClass="greenbutton"
                                        Text="Save">
                                    </telerik:RadButton>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    <telerik:RadButton ID="btnClose" runat="server" OnClientClicked="btnClose_Clicked" AutoPostBack="false" ButtonType="LinkButton" CssClass="redbutton"
                                        Text="Close">
                                    </telerik:RadButton>
                                        &nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <%--  <table style="width: 100%;height:100%">
                <tr  style="width: 100%;height:88%">
                    <td valign="top">
                        <asp:Panel ID="pnlPhoneEncounter" runat="server" GroupingText="Phone Encounter" Width="100%" Height="100%" Font-Size="Small" BackColor="White">
                            <table style="width: 100%;height:100%;">
                                <tr>
                                    <td class="style1" >
                                         
                                        <asp:Label ID="lblPatientName" runat="server" Text="Patient Name"></asp:Label>
                                         
                                    </td>
                                    <td class="style2">
                                         
                                        <telerik:RadTextBox ID="txtPatientName" Runat="server" BackColor="#BFDBFF" BorderColor="Black"
                                            ReadOnly="True">
                                            
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblPatientDOB" runat="server" Text="Patient DOB"></asp:Label>
                                         
                                    </td>
                                    <td >
                                         
                                        <telerik:RadTextBox ID="txtPatientDOB" Runat="server" BackColor="#BFDBFF" 
                                            ReadOnly="True" BorderColor="Black">
                                     
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblPatientSex" runat="server" Text="Patient Sex"></asp:Label>
                                         
                                    </td>
                                    <td  colspan="4">
                                         
                                        <telerik:RadTextBox ID="txtPatientSex" Runat="server" BackColor="#BFDBFF" 
                                            ReadOnly="True" BorderColor="Black">
                                       </telerik:RadTextBox>
                                         
                                    </td>
                                </tr>
                                <tr>
                                   <td class="style1" >
                                         
                                       <asp:Label ID="lblAccountNumber" runat="server" Text="Account#"></asp:Label>
                                         
                                    </td>
                                    <td class="style2" >
                                         
                                        <telerik:RadTextBox ID="txtAccountNumber" Runat="server" BackColor="#BFDBFF" 
                                            ReadOnly="True" BorderColor="Black">
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblHomePhno" runat="server" Text="Home Phone #"></asp:Label>
                                         
                                    </td>
                                    <td >
                                         
                                        <telerik:RadMaskedTextBox ID="mskHomePhone" Runat="server" 
                                            Mask="(###) ###-####" BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black">
                                        </telerik:RadMaskedTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblCellPhno" runat="server" Text="Cell Phone #"></asp:Label>
                                         
                                    </td>
                                    <td colspan="4">
                                         
                                        <telerik:RadMaskedTextBox ID="mskCellPhone" Runat="server" 
                                            Mask="(###) ###-####" BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black">
                                            
                                        </telerik:RadMaskedTextBox>
                                         
                                    </td>
                                </tr>
                                <tr>
                                   <td class="style1" >
                                         
                                       <asp:Label ID="lblWorkPhone" runat="server" Text="Work Phone#"></asp:Label>
                                         
                                    </td>
                                    <td class="style2" >
                                         
                                        <telerik:RadMaskedTextBox ID="mskWorkPhno" Runat="server" 
                                            Mask="(###) ###-####" BackColor="#BFDBFF" ReadOnly="True" BorderColor="Black">
                                       </telerik:RadMaskedTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblExtensionNumber" runat="server" Text="Extension #"></asp:Label>
                                         
                                    </td>
                                    <td >
                                         
                                        <telerik:RadTextBox ID="txtExtension" Runat="server" BackColor="#BFDBFF" 
                                            ReadOnly="True" BorderColor="Black">
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        </td>
                                    <td >
                                         
                                    </td>
                                    <td >
                                         
                                    </td>
                                    <td>
                                         
                                    </td>
                                    <td >
                                         
                                    </td>
                                </tr>
                                <tr>
                                   <td class="style1" >
                                         
                                       <asp:Label ID="lblCallSpokenTo" runat="server" Text="Call Spoken To"></asp:Label>
                                         
                                    </td>
                                    <td class="style2" >
                                         
                                        <telerik:RadComboBox ID="cboCallSpokenTo" Runat="server" 
                                            onclientselectedindexchanged="cboCallSpokenTo_SelectedIndexChanged">
                                        </telerik:RadComboBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblCallerName" runat="server" Text="Caller Name"></asp:Label>
                                         
                                    </td>
                                    <td >
                                         
                                        <telerik:RadTextBox ID="txtCallerName" Runat="server" BackColor="#BFDBFF" BorderColor="Black"  >
                                            <DisabledStyle Resize="None" />
                                            <InvalidStyle Resize="None" />
                                            <HoveredStyle Resize="None" />
                                            <ReadOnlyStyle Resize="None" />
                                            <EmptyMessageStyle Resize="None" />
                                            <ClientEvents OnFocus="txtCallerName_OnFocus" 
                                                OnMouseOver="txtCallerName_OnMouseOver" 
                                                OnMouseOut="txtCallerName_OnMouseOut" />
                                            <FocusedStyle Resize="None" />
                                            <EnabledStyle Resize="None" />
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblCallDuration" runat="server" Text="Call Duration"></asp:Label>
                                         
                                    </td>
                                    <td >
                                         
                                        <telerik:RadTextBox ID="txtCallHrs" Runat="server" Height="16px" Width="50px">
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblCallHrs" runat="server" Text="Hrs"></asp:Label>
                                         
                                    </td>
                                    <td >
                                         
                                        <telerik:RadTextBox ID="txtCallMins" Runat="server" Height="16px" Width="37px">
                                        </telerik:RadTextBox>
                                         
                                    </td>
                                    <td >
                                         
                                        <asp:Label ID="lblCallMins" runat="server" Text="Min"></asp:Label>
                                         
                                    </td>
                                </tr>
                                <tr>
                                   <td  colspan="9" valign="top">
                                         
                                       <asp:Panel ID="Panel1" runat="server" Height="100px" BackColor="White">
                                           <table style="width:100%;height:100%;">
                                               <tr>
                                                   <td >
                                                       <asp:Label ID="lblPlan" runat="server" Text="Plan"></asp:Label>
                                                       </td>
                                                   <td  rowspan="2">
                                                      <DLC:DLC ID="DLC" runat="server" Enable="True" TextboxHeight="95px" Value="TREATMENT PLAN NOTES"
                                                           TextboxWidth="800px" />
                                                       </td>
                                                   
                                               </tr>
                                               <tr>
                                                   <td>
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
                <tr  style="width: 100%;height:12%">
                    <td valign="top">
                        <asp:Panel ID="pnlControls" runat="server" Width="100%" Height="40%" BackColor="White">
                            <table style="width: 100%; height:40%;">
                                <tr>
                                    <td class="style4" >
                                        <asp:Label ID="lblPassword" runat="server" Text="Password*" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td class="style3" align="left" >
                                        <telerik:RadTextBox ID="txtPassword" runat="server" style="margin-right: 1px" 
                                            TextMode="Password">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td>
                                        
                                    </td>
                                    <td class="style8" >
                                        
                                    </td>
                                    <td class="style7" align="right" >
                                        <telerik:RadButton ID="btnSave" runat="server" Text="Save" 
                                            onclick="btnSave_Click">
                                        </telerik:RadButton>
                                    </td>
                                    <td>
                                        <telerik:RadButton ID="btnClose" runat="server" Text="Close" 
                                            onclientclicked="btnClose_Clicked"  >
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                               
                            </table>
                        </asp:Panel>
                    </td>
                   
                </tr>
               
            </table>--%>
            </div>
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnHumanDetails" runat="server" EnableViewState="false" />
            <%--<asp:HiddenField ID="hdnEnableSave" runat="server" />--%>
            <asp:HiddenField ID="hdnMessageType" runat="server" />
            <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="btnClose_Clicked();" />
            <asp:Button ID="InvisibleCloseButton" runat="server" CssClass="displayNone" OnClientClick="SetDateTime();" OnClick="InvisibleCloseButton_Click" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSPhoneEncounter.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <%--<script src="JScripts/JSErrorReport.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
            
        </asp:PlaceHolder>
    </form>
</body>
</html>