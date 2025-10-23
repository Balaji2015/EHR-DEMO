<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientPayment.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPatientPayment" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Patient Payment Account</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
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
    <script type="text/javascript">
        function checkLength() {
            var sender = document.getElementById('txtPaymentNote');
            if (sender.value.length > 10) {
                sender.value = sender.value.substr(0, 1024);
            }
        }
        </script>	


    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
    <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>

   

</head>
<body onload="setCPtvalue();warningmethod();setcurrentdate();" onchange="setcurrentdate();">

     <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">

            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" DestroyOnClose="true">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>

    <form id="frmQuickPatientCreate" runat="server" style="overflow-x: hidden;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
                    <Windows>
                        <telerik:RadWindow ID="PaymentMessageWindow" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
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
                <asp:Button ID="btnRefresh" runat="server" OnClientClick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" 
                    CssClass="aspresizedbluebutton" visible="false"/>
                <div>
                    <asp:Label ID="lblSave" runat="server" Style="display: none"></asp:Label>
                    <table style="width: 100.5%" id="tbl" runat="server">
                        <tr>
                            <td colspan="5">
                                <div id="divPatientstrip" runat="server" class=" pnlBarGroup Editabletxtbox " style="width:180%; height: 21px; margin-bottom: 6px; margin-top: 0px; vertical-align: middle; padding-top: 2px; position: relative; padding-left: 8px; border: 0px !important"></div>
                            </td>
                        </tr>
                         <tr style="width:100%">
                              <td style="width: 1%;">
                                <asp:RadioButton ID="rdbNewCollection" runat="server"  AutoPostBack="true" GroupName="rdbbutton"  OnCheckedChanged="rdbNewCollection_CheckedChanged" 
                                    onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" Checked="true" />
                            </td>
                             <td style="width:18%"><%--Facility - Appt. Date & Time*--%>
                                <asp:Label ID="lblcollection" runat="server" EnableViewState="false" Text="New Collection(Today)" CssClass="Editabletxtbox" ></asp:Label>
                           </td>
                            <td style="width: 1%;">
                                <asp:RadioButton ID="rdbFacilityName" runat="server"  AutoPostBack="true" GroupName="rdbbutton"  OnCheckedChanged="rdbFacilityName_CheckedChanged" Checked="false"
                                     style="margin-left: 56px;" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"/>
                            </td>
                            <td style="width:9%"><%--Facility - Appt. Date & Time*--%>
                                <asp:Label ID="lblFacilityName" runat="server" EnableViewState="false" Text="Facility / DOS" CssClass="Editabletxtbox" ></asp:Label>
                           </td>
                            <td style="width:5%">
                                <asp:DropDownList ID="ddlFacilitywithDOS" runat="server" Width="268px" AutoPostBack="true" style="margin-left: 4px;"
                                    OnSelectedIndexChanged="ddlFacilityWithDOS_SelectedIndexChanged" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" >
                                </asp:DropDownList>
                            </td>
                            <td style="width: 1%;">
                                <asp:RadioButton ID="rdbVoucher" runat="server" AutoPostBack="true" GroupName="rdbbutton"  OnCheckedChanged="rdbVoucher_CheckedChanged" 
                                     Checked="false" style="margin-left: 75px;" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"/>
                            </td>
                            <td style="width:8%;">
                                <asp:Label ID="lblVoucher" runat="server" EnableViewState="false" Text="Voucher #" CssClass="Editabletxtbox "></asp:Label>
                            </td>
                              <td style="width:5%">
                                <asp:DropDownList ID="ddlVoucher" runat="server" AutoPostBack="true" Width="120px" Style="margin-left: -10px; margin-right: -1px;"
                                      OnSelectedIndexChanged="ddlVoucher_SelectedIndexChanged"
                                   onchange="enableAdd(); { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" >
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblStatus" Font-Bold="true" runat="server" EnableViewState="false"  CssClass="Editabletxtbox  manredforstar !important;" style="margin-left: 13px;" ></asp:Label>
                            </td>
                            
                           <%-- <td style="width:13%"></td>--%>
                           
                        </tr>
                         <tr style="height:10px">
</tr>
                         <tr style="width:100%">
                             <td colspan="12">
                                 <asp:Label ID="lblnote" runat="server"  style="font-style:italic" Text="Note: By default Facility/DOS shows the most recent DOS for the patient,if needed the user can select a specific DOS and work on the collected payment entries.The listed DOS are neither batched nor archived."></asp:Label>
                             </td>
                             </tr>
                       
                       
                         <tr style="height:10px">
</tr>
                        <tr>
                            <td>
                    <span id="lblFromDate" class="fromdate" style="display:none;">From Date</span>
                </td>
                <td>
                    <input type="text" id="dtpFromDate" class="form-control fromdate" style="display:none;" />
                </td>
                <td>
                    <span id="lblToDate" class="todate" style="display:none;">To Date</span>
                </td>
                <td>
                    <input type="text" id="dtpToDate" class="form-control todate" style="display:none;" />
                </td>
                        </tr>
                        </table>
                    <table style="width:100%">
                        <tr style="width:100%">
                            <td style="width: 100%">
                                <asp:Panel ID="gbPaymentInformation" runat="server" ScrollBars="Auto" Font-Size="Small" style="margin-top: 9px;"
                                    GroupingText="Payment Information" CssClass="LabelStyleBold">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width:15%">
                                                <span class="MandLabelstyle" runat="server" id="spanpayment">Method of Payment</span><span class="manredforstar" runat="server" id="spanpayment2">*</span>

                                            </td>
                                            <td  style="width:15%">
                                                <asp:DropDownList ID="cboMethodOfPayment" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cboMethodOfPayment_SelectedIndexChanged" Width="93%" onchange="enableAdd();">
                                                </asp:DropDownList>
                                            </td>
                                            <td  style="width:20%">
                                                <span class="spanstyle" runat="server" id="spanCheck" style="margin-left: 12px;">Check # \Last 4 Digits of Card #</span><span class="manredforstar" runat="server" id="spanCheckStar" visible="false">*</span>
                                            </td>
                                            <td style="width:15%">
                                                <asp:TextBox ID="txtCheckNo" runat="server" Width="90%" OnKeyPress="return isNumberKey(this);enableAdd();"
                                                    MaxLength="4" Style="margin-left: 3px; margin-right: -1px;" onchange="enableAdd();"></asp:TextBox>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:Label ID="lblAuthNo" CssClass="Editabletxtbox" runat="server" Text="CC Auth#" Style="margin-left: 15px;"></asp:Label>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:TextBox ID="txtAuthNo" runat="server" Width="93%" OnKeyPress="return isNumberKey(this);enableAdd();"
                                                    MaxLength="25" Style="margin-left: -2px;" onchange="enableAdd();"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  style="width:15%">
                                                <asp:Label ID="lblPaymentAmount" CssClass="Editabletxtbox" runat="server" Text="Copay $  "
                                                    Width="100px"></asp:Label>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:TextBox ID="txtPaymentAmount" onchange="CalAmt(this);enableAdd();" OnKeyPress="return AllowAmount(this);enableAdd();"
                                                    runat="server" Width="90%" MaxLength="7" Text="0.00" onblur="DefaultCopay()" Style="margin-top: 6px"></asp:TextBox>
                                            </td>
                                            <td  style="width:20%">
                                                <asp:Label ID="lblRecOnAcc" CssClass="Editabletxtbox" runat="server" Text="Rec'd on Acc $  "
                                                    Width="100px" Style="margin-top: -1px; margin-left: 13px;"></asp:Label>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:TextBox ID="txtRecOnAcc" runat="server" OnKeyPress="return AllowAmount(this);enableAdd();" onchange="CalAmt(this);enableAdd();" MaxLength="7" Text="0.00" onblur="DefaultCopayforrec()" Width="90%"
                                                    Style="margin-top: 4px; margin-left: 3px;"></asp:TextBox>
                                            </td>

                                            <td  style="width:15%">
                                                <asp:Label ID="lblPastDue" CssClass="Editabletxtbox" runat="server" Text="Past Due $" Style="margin-top: 12px; margin-left: 16px;"></asp:Label>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:TextBox ID="txtPastDue" onchange="AutoSave();" OnKeyPress="return AllowAmount(this);"
                                                    runat="server" Width="93%" CssClass="nonEditabletxtbox" BorderWidth="1px" ReadOnly="True" Style="margin-top: 3px; margin-left: -2px;"></asp:TextBox>
                                        </tr>
                                        <tr>
                                            <td  style="width:15%">
                                                <asp:Label ID="lblRefundAmount" CssClass="Editabletxtbox" Text="Refund  Amt. $" runat="server"></asp:Label>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:TextBox ID="txtRefundAmount" Width="90%" onchange="CalAmt(this);" OnKeyPress="return AllowAmount(this);enableAdd();" onblur="DefaultCopayforrefund();enableAdd();" runat="server" MaxLength="7" Text="0.00"
                                                    Style="margin-top: -8px"></asp:TextBox>
                                            </td>
                                            <td  style="width:20%">
                                                <asp:Label ID="lblCheckDate" CssClass="Editabletxtbox" runat="server" Text="Check Date" Width="90%" Style="margin-top: -12px; margin-left: 12px;"></asp:Label>
                                            </td>
                                            <td  style="width:15%">
                                                <telerik:RadMaskedTextBox ID="dtpCheckDate" runat="server" Mask="##-Lll-####" Width="94%" Style="margin-top: -9px; margin-left: 2px;">
                                                    <ClientEvents OnValueChanged="QPCDateValidation" />
                                                    <InvalidStyle Resize="None" />
                                                    <FocusedStyle Resize="None" BackColor="White" CssClass="Editabletxtbox" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <HoveredStyle Resize="None" BackColor="White" CssClass="Editabletxtbox" />
                                                    <DisabledStyle Resize="None" />
                                                    <EnabledStyle Resize="None" BackColor="White" />
                                                    <ReadOnlyStyle Resize="None" />
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                            <td  style="width:15%">
                                                <%--<asp:Label ID="lblPaymentNote" CssClass="Editabletxtbox" runat="server" Text="Payment Note" Style="margin-top: 10px; margin-left: 12px;"></asp:Label>--%>
                                             <span id="spanPaymentNotes" runat="server" class="spanstyle" style="margin-top: 10px; margin-left: 15px;">Payment Note</span><span id="spanPatientNotestar" runat="server" class="manredforstar" style="margin-top:-15px;margin-left:2px;" visible="false">*</span> </td>
                                            </td>
                                             <td colspan="3"  style="width:15%">
                                                <asp:TextBox ID="txtPaymentNote" runat="server" onchange="AutoSave();enableAdd();" Style="margin-left: -2px; margin-top: 3px; resize: none;"
                                                    CssClass="MultiLineTextBox" TextMode="MultiLine" Width="94%" onkeyup="checkLength();"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td  style="width:15%">
                                                <span class="spanstyle" runat="server" id="spanRelation">Relationship</span><span class="manredforstar" runat="server" id="spanRelationstar" visible="false">*</span>

                                            </td>
                                            <td  style="width:15%">
                                                <asp:DropDownList ID="cboRelation" runat="server" onchange="enableAdd();" Width="94%" AutoPostBack="True" OnSelectedIndexChanged="cboRelation_SelectedIndexChanged" Style="margin-top: -8px">
                                                </asp:DropDownList>
                                            </td>

                                            <td  style="width:20%">
                                                <span class="spanstyle span1" runat="server" id="spanPaidBy" style="margin-top: 10px; margin-left: 13px;">Paid By</span><span class="manredforstar" runat="server" id="spanPaidStar" visible="false">*</span>
                                            </td>
                                            <td  style="width:15%">
                                                <asp:TextBox ID="txtpaidBy" runat="server" Style="margin-top: -8px; width: 90%; margin-left: 2px;" onchange="enableAdd();"></asp:TextBox>
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
                                                                Width="100px" Style="margin-top: -10px" OnClientClick="Loading()"  Enabled="false"/>
                                                            &nbsp;&nbsp;
                                                        <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" CssClass="aspresizedredbutton" OnClientClick="return ShowMessageforPatientPayment();"
                                                            AccessKey="C" Text="Clear All" Width="100px" Style="margin-top: -9px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" style="max-width: 100%;">
                                                <asp:GridView ID="grdPaymentInformation" runat="server"  AutoGenerateColumns="False" CellPadding="3" CssClass="Gridbodystyle" EmptyDataText="No Records Found" HeaderStyle-CssClass="Gridheaderstyle header"
                                                    Style="margin-top: 6px;" OnRowCommand="grdPaymentInformation_RowCommand" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField  HeaderText="Edit" HeaderStyle-Width="3%">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="EditGridRow" runat="server" CommandName="EditC" ImageUrl="~/Resources/edit.gif" ToolTip="Edit"/>
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
                                                         <asp:BoundField AccessibleHeaderText="VoucherNo" DataField="VoucherNo" HeaderText="Voucher No" Visible="false">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="gridSelectedRow" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <asp:HiddenField ID="hdnTotalPayment" runat="server" />
                                        <asp:HiddenField ID="hdnBatchStatus" runat="server" />
                                        <asp:HiddenField ID="hdnStatus" runat="server" />
                                    </table>
                                </asp:Panel>
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
                <asp:HiddenField ID="hdnPatientName" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnPastDue" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnEncid" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnVoucherNo" runat="server" EnableViewState="false" />
                <asp:HiddenField ID="hdnFacilityRole" runat="server" EnableViewState="false" />
                  <asp:HiddenField ID="hdndate" runat="server" EnableViewState="false" />
                 <asp:Button ID="hdnbtngeneratepaymentxml" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

                <%-- // <asp:HiddenField ID="hdnBtnLoadPatientDetails" Style="display: none" runat="server"/>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnendwaitcursor" runat="server" Text="Button" Style="display: none"
            OnClick="btnendwaitcursor_Click" />

        <asp:Button ID="btnSave" runat="server" Text="Save" Style="display: none" />
        <br />
       <%-- <div>
           

        </div>--%>
        <div style="text-align:right; margin-top: 48px;">
             <table>
                <tr>
                     <td> <%--style="text-align: right!important;margin-left: -76px;"--%>
                                <asp:Label ID="lblTotalAmount" CssClass="Editabletxtbox"  runat="server" Text="Total Payment $"></asp:Label>
                           </td>
                            <td >
                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="nonEditabletxtbox" ReadOnly="true" onchange="return AllowAmount(this);enableAdd();"  
                                    style="margin-left: 10%;"></asp:TextBox> <%--style="margin-left: -349px;margin-right: -9px;"--%>
                            </td>
                </tr>
            </table>
            <asp:Button ID="btnPatientTransaction" runat="server" CssClass="aspresizedbluebutton" OnClientClick="return OpenFinancialReport();"  style="margin-top: -38px;"
            AccessKey="C" Text="Patient Transactions"  EnableViewState="false" />
         <asp:Button ID="btnPrintRecipt" runat="server" CssClass="aspresizedbluebutton"   style="margin-top: -37px;"
            AccessKey="C" Text="Print Receipt" Width="104px"  EnableViewState="false" OnClick="btnPrintRecipt_Click"
             OnClientClick="btnPrintRecipt_Window();" />
             <asp:Button ID="btnClose" runat="server" CssClass="aspresizedredbutton" OnClientClick="return ClosesaveWindow();" style="margin-top: -38px;"
            AccessKey="C" Text="Close" Width="100px"  EnableViewState="false" />

        </div>
          
       





        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
            OnClientClick="return ClosesaveWindow();" />
       
         <asp:HiddenField ID="hdnMessageType" runat="server" />
         <asp:HiddenField ID="hdnBSave" runat="server" />
         <asp:HiddenField ID="hdnFileName" runat="server" EnableViewState="false" />
         <asp:HiddenField ID="hdnLaptopView" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnHumID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncounterProviderID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEncounterProviderName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnAppointmentProviderID" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnAppointmentProviderName" runat="server" EnableViewState="false" />
        <asp:Button ID="hdnRefresh" runat="server" Style="display: none" OnClick="Button2_Click1"
                Text="Refrashs" />

         <asp:PlaceHolder ID="PlaceHolder1" runat="server">
             <script type="text/javascript" src="JScripts/jquery-2.1.3.js"></script>
        <script type="text/javascript" src="JScripts/jquery-ui.min.js"></script>
           
            <link href="CSS/jquery-ui.css" rel="stylesheet" />

            <script src="JScripts/bootstrap.min.js"></script>
            <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />

            <%--<script src="JScripts/JSPatientPayment.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>--%>
             <script type="text/javascript" src="JScripts/jquery.datetimepicker.js"></script>

            <script src="JScripts/JSQuickPatient.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>


            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
