<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPatientInsurancePolicyMaintenance.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPatientInsurancePolicyMaintenance" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <base target="_self" />

    <script language="javascript" type="text/javascript">
        function ClosePatInsuredMaintanence() {
            if (CloseWithWarning() == false) {
                return false;
            }
            else {
                var grdPolicyInformation = document.getElementById("grdPolicyInformation");
                var index = parseInt(document.getElementById("hdnSelectedIndex").value) + 1;
                var row = grdPolicyInformation.rows[index];
                var result = new Object();

                result.PlanName = row.cells[2].innerHTML;
                result.PolicyHolderId = row.cells[5].innerHTML;
                result.id = row.cells[14].innerHTML;
                result.InsPlanID = row.cells[15].innerHTML;
                result.CarrierID = row.cells[16].innerHTML;
                result.PlanType = row.cells[17].innerHTML;

                if (window.opener) {
                    window.opener.returnValue = result;
                }
                window.returnValue = result;
                returnToParent(result);

                //self.close();
            }

        }

        function CloseWithWarning() {
            var IsTrue = false;
            var grid = document.getElementById("grdPolicyInformation");
            var rowCount = grid.rows;
            for (var i = 0; i < rowCount.length; i++) {
                if ((grid.rows[i].cells[1].innerHTML == "&nbsp;" || grid.rows[i].cells[1].innerText == " ") && grid.rows[i].cells[4].innerText.toUpperCase() == "YES") {
                    IsTrue = true;
                    break;
                }
            }
            if (IsTrue == true) {
                DisplayErrorMessage('420039', '', grid.rows[i].cells[2].innerText);
                return false;
                returnToParent(null);
            }
            else {
                if (document.getElementById("hdnSelectedIndex").value != "") {
                    var index = parseInt(document.getElementById("hdnSelectedIndex").value) + 1;
                    var row = grid.rows[index];
                    var result = new Object();
                    if (row != undefined) {
                        result.PlanName = row.cells[2].innerHTML;
                        result.PolicyHolderId = row.cells[5].innerHTML;
                        result.id = row.cells[14].innerHTML;
                        result.InsPlanID = row.cells[15].innerHTML;
                        result.CarrierID = row.cells[16].innerHTML;
                        result.PlanType = row.cells[17].innerHTML;
                    }
                    returnToParent(result);
                }
                else {
                    returnToParent(null);
                }
            }

        }

        function returnToParent(args) {
            var oArg = new Object();
            oArg.result = args;
            var oWnd = GetRadWindow();
            if (oWnd != null) {
                if (oArg.result) {
                    oWnd.close(oArg.result);
                }
                else {

                    oWnd.close(oArg.result);
                }
            }
            else {
                self.close();
            }
        }

    </script>


    <title>Patient Insurance Policy Maintenance</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style31 {
            height: 33px;
        }

        .style32 {
            width: 258px;
        }

        .style33 {
            width: 243px;
        }

        .style38 {
            width: 500px;
        }

        .panel_with_padding {
            padding-top: 5px;
            padding-left: 0px;  
            padding-right: 0px;
            padding-bottom: 5px;
        }

        .DisplayNone {
            display: none;
        }

        .style39 {
            width: 71px;
        }

        .style40 {
            width: 57px;
        }

        .style41 {
            width: 55px;
        }

        .Panel legend {
            font-weight: bold;
        }
    </style>
   <%-- <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <telerik:RadWindowManager ID="PatInsuredMaintanenceodalWindowMngt" runat="server"
        EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="PatInsuredMaintanenceModalWindow" runat="server" VisibleOnPageLoad="false"
                Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
         <Windows>
            <telerik:RadWindow ID="PerformEVWindow" runat="server" VisibleOnPageLoad="false"
                Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <form id="frmPatientInsurancePolicyMaintenance" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptMngr" runat="server" EnableViewState="false">
            </asp:ScriptManager>

              <div>
                 <asp:Panel ID="pnlPatientStrip" runat="server" Width="1000px"
                    CssClass="LabelStyleBold">
                 
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <%--<asp:TextBox ID="txtPatientstrip" runat="server" Text="" CssClass="nonEditabletxtbox"></asp:TextBox>--%>
                                 <div id="divPatientstrip" runat="server" class=" pnlBarGroup Editabletxtbox " style="height:10px; margin-bottom: 22px; margin-top: -7px; vertical-align: middle; padding-top: 2px; position: relative; padding-left: 8px; border: 0px !important "></div>
                            </td>
                        </tr>
                     </table>
                     </asp:Panel>
               </div>

            <div runat="server" visible="false">
                <asp:Panel ID="Panel1" runat="server" GroupingText="Patient Information " BorderStyle="ridge" CssClass="Editabletxtbox"
                    Font-Size="Small" BackColor="White">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblPatientLastName" runat="server" Text="Last Name   " EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientLastName" runat="server" ReadOnly="True" Width="256px"
                                    BackColor="#BFDBFF" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPatientFirstName" runat="server" Text="First Name   " Width="75px"
                                    EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientFirstName" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="256px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblPatientDOB" runat="server" Text="DOB" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientDOB" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="256px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPatientSex" runat="server" Text="Sex" EnableViewState="False" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientSex" runat="server" BackColor="#BFDBFF" OnTextChanged="txtPatientSex_TextChanged"
                                    ReadOnly="True" Width="256px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblAccountno" runat="server" Text="Acc.#" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountNo" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="256px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblExternalAccountNo" runat="server" Text="Ext Acc.#" Width="55px"
                                    EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientExternalAccNo" runat="server" BackColor="#BFDBFF" ReadOnly="True"
                                    Width="256px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPatientType" runat="server" Text="Patient Type" EnableViewState="false" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPateintType" runat="server" BackColor="#BFDBFF" OnTextChanged="txtPatientSex_TextChanged"
                                    ReadOnly="True" Width="256px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td colspan="2"></td>

                            <td></td>
                            
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div style="height: 4px; width: 100%"></div>
            <div>
                <asp:UpdatePanel ID="updtPnl" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server" CssClass="Editabletxtbox" GroupingText="Existing Insurance Policies  "
                            ScrollBars="Horizontal" Font-Size="Small" BorderStyle="ridge" Height="300px"
                            Width="1120px" BackColor="White">
                            <table style="width: 100%">
                                <tr>
                                    <td valign="top">
                                        <asp:GridView ID="grdPolicyInformation" runat="server" OnSelectedIndexChanged="grdPolicyInformation_SelectedIndexChanged"
                                            AutoGenerateSelectButton="True" OnRowDataBound="grdPolicyInformation_RowDataBound"
                                            EmptyDataText="No Records Found" BackColor="White" BorderColor="#E7E7FF" BorderStyle="None"
                                            Width="1600px" BorderWidth="1px" CellPadding="3"  OnRowDeleting="grdPolicyInformation_RowDeleting"
                                            AutoGenerateColumns="False" OnRowCommand="grdPolicyInformation_RowCommand" CssClass="Gridbodystyle" >
                                            <Columns>
                                                <asp:BoundField DataField="Insurance Type" HeaderText="Ins. Type" HeaderStyle-Width="185px">
                                                    <HeaderStyle Width="185px" CssClass="Gridheaderstyle" />
                                                    <ItemStyle Wrap="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Plan Name" HeaderText="Plan Name" HeaderStyle-CssClass="Gridheaderstyle" />
                                                <asp:BoundField DataField="Carrier Name" HeaderText="Carrier Name" HeaderStyle-CssClass="Gridheaderstyle" />
                                                <asp:BoundField DataField="Active" HeaderText="Active" HeaderStyle-CssClass="Gridheaderstyle" />
                                                <asp:BoundField DataField="Policy Holder ID" HeaderText="Policy Holder ID" />
                                                <asp:BoundField DataField="Group Number" HeaderText="Group #" HeaderStyle-CssClass="Gridheaderstyle" />
                                                <asp:BoundField DataField="Relationship" HeaderText="Relationship" HeaderStyle-CssClass="Gridheaderstyle" />
                                                <asp:BoundField DataField="Effective Start Date" HeaderText="Eff. Start Date" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="Termination Date" HeaderText="Term. Date" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="Insured Name" HeaderText="Insured Name" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="Insured Human ID" HeaderText="Acc. #" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="Insured DOB" HeaderText="DOB" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="Insured Sex" HeaderText="Sex" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Wrap="true" ItemStyle-CssClass="DisplayNone"
                                                    HeaderStyle-CssClass="DisplayNone">
                                                    <HeaderStyle CssClass="DisplayNone Gridheaderstyle" />
                                                    <ItemStyle CssClass="DisplayNone" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Plan ID" HeaderText="Plan #" HeaderStyle-CssClass="Gridheaderstyle" />
                                                <asp:ButtonField CommandName="SingleClick" Text="SingleClick" Visible="False" HeaderStyle-CssClass="Gridheaderstyle"/>
                                                <asp:BoundField DataField="CarrierID" HeaderText="CarrierID" ItemStyle-Wrap="true"
                                                    ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone">
                                                    <HeaderStyle CssClass="DisplayNone" />
                                                    <ItemStyle CssClass="DisplayNone" Wrap="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PlanType" HeaderText="PlanType" ItemStyle-Wrap="true"
                                                    ItemStyle-CssClass="DisplayNone" HeaderStyle-CssClass="DisplayNone ">
                                                    <HeaderStyle CssClass="DisplayNone Gridheaderstyle" />
                                                    <ItemStyle CssClass="DisplayNone" Wrap="True" />
                                                </asp:BoundField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="highlight" />
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                <%-- <div>
                     <td>
                     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                     <ContentTemplate>
                     <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowActiveOnly_CheckedChanged"
                      Text="Show Active Only" CssClass="Editabletxtbox"/>
                     </ContentTemplate>
                     </asp:UpdatePanel>
                     </td>
                  </div>--%>

                        <asp:HiddenField ID="hdnHumanID" runat="server" EnableViewState="false" />
                        <asp:HiddenField ID="hdnSelectedIndex" runat="server" EnableViewState="false" />
                        <asp:HiddenField ID="hdnInsuredHumanID" runat="server" EnableViewState="false" />
                        <asp:HiddenField ID="hdnEncounterID" runat="server" EnableViewState="false" />
                        <asp:HiddenField ID="hdnfileupload" runat="server" EnableViewState="false" />
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
                 </div>
                            <td style="padding-top: -10px">
                                <asp:UpdatePanel ID="updtShowActive" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="chkShowActiveOnly" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowActiveOnly_CheckedChanged"
                                            Text="Show Active Only" CssClass="Editabletxtbox"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
              <div>
                <asp:UpdatePanel ID="updtButtons" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Buttons" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td rowspan="4">
                                        <asp:Panel ID="Panel3" runat="server" GroupingText="Coverage Priority  " CssClass="Editabletxtbox"
                                            Height="129px" Width="256px" Font-Size="Small">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSetPriorityAsPrimary" runat="server" OnClick="btnSetPriorityAsPrimary_Click"
                                                            AccessKey="P" OnClientClick="return IsPlanSelected();" Text="Primary" Width="218px" CssClass="aspresizedbluebutton"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style31">
                                                        <asp:Button ID="btnSetPriorityAsSecondary" runat="server" Height="23px" OnClick="btnSetPriorityAsSecondary_Click"
                                                            AccessKey="S" OnClientClick="return IsPlanSelected();" Text="Secondary" Width="219px"  CssClass="aspresizedbluebutton"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSetPriorityAsTertiary" runat="server" OnClick="btnSetPriorityAsTertiary_Click"
                                                            AccessKey="T" OnClientClick="return IsPlanSelected();" Text="Tertiary " Width="218px"  CssClass="aspresizedbluebutton"/>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td width="200">&nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnAddInsurancePolicy" runat="server" OnClick="btnAddInsurancePolicy_Click"
                                            OnClientClick="return openAddinswindow();" Text="Add Insurance Policy" AccessKey="A"
                                            Width="200px"  CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td width="150px">
                                        <%--<asp:Button ID="btnEligibilityHistory" runat="server" AccessKey="E" OnClientClick="OpenEligibilityHistory();"
                                            Text="Show Eligibility History" Width="200px" EnableViewState="false"  CssClass="aspresizedbluebutton"/>--%>
                                        <asp:Button ID="btnPerformEV" runat="server" Text="Perform EV" Width="200px" OnClientClick="OpenPerformEV();" CssClass="aspresizedbluebutton" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td align="right">
                                        <asp:Button ID="btnAddInsuranceForSelf" runat="server" OnClick="btnAddInsuranceForSelf_Click"
                                            OnClientClick="return openAddinswindowForSelf();" Text="Add Insurance Policy-Self"
                                            AccessKey="L" Width="200px"  CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnViewUpdatePolicyInformation" runat="server" OnClick="btnViewUpdatePolicyInformation_Click"
                                            AccessKey="U" OnClientClick="return openwindow();" Text="View/Update Policy Information"
                                            Width="200px"  CssClass="aspresizedbluebutton"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnUpdateInformationRefresh" runat="server" CssClass="DisplayNone"
                                            OnClick="btnUpdateInformationRefresh_Click" />
                                        <asp:Button ID="btnAddInsSelfRefresh" runat="server" CssClass="DisplayNone" OnClick="btnAddInsSelfRefresh_Click" />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnUpdateInsuredDemographics" runat="server" OnClick="btnUpdateInsuredDemographics_Click"
                                            OnClientClick="return openDemographicswindow();" Text="Update Insured Demographics"
                                            AccessKey="D" Width="200px"  CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnMakeInactive" runat="server" OnClick="btnMakeInactive_Click" Text="Make Active/Inactive"
                                            AccessKey="I" OnClientClick="return IsPlanSelected();" Width="200px"  CssClass="aspresizedbluebutton"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnMakeActive" runat="server" AccessKey="M" OnClick="btnMakeActive_Click"
                                            OnClientClick=" return IsPlanSelected();" Text="Make Active" Width="200px" Visible="False"  CssClass="aspresizedbluebutton"/>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnOK" runat="server"
                                            OnClientClick="ClosePatInsuredMaintanence();" Text="OK" Width="200px" EnableViewState="false"  CssClass="aspresizedgreenbutton"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return CloseWithWarning();"
                                            AccessKey="C" Width="200px" EnableViewState="false" CssClass="aspresizedredbutton"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:HiddenField ID="hdnCurrentProcess" runat="server" EnableViewState="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSPatInsMaintenance.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
