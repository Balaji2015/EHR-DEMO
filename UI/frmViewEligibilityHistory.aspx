<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmViewEligibilityHistory.aspx.cs" Inherits="Acurus.Capella.UI.frmViewEligibilityHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Eligibility History</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self"></base>
    <style type="text/css">
        .style1 {
            width: 68px;
        }

        .style2 {
        }

        .style5 {
            width: 920px;
        }

        .style6 {
            width: 145px;
        }

        .style7 {
            width: 870px;
        }

        .style8 {
            width: 63px;
        }

        .Panel legend {
            font-weight: bold;
        } 
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodybackground">
    <form id="frmViewEligibilityHistory" runat="server" style="width: 100%; height: 100%">
        <aspx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </aspx:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div style="width: 100%">
                    <div style="width: 100%">
                        <asp:Panel ID="pnlPatientInfo" runat="server" Width="100%" Font-Size="Small">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblPatientLastName" EnableViewState="false" runat="server" Text="Last Name" CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style2" colspan="2">
                                        <asp:TextBox ID="txtLastName" runat="server" Width="300px" Cssclass="nonEditabletxtbox" BorderWidth="1px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="style8">
                                        <asp:Label ID="lblAccountNO" EnableViewState="false" runat="server" Text="Acc. #"  CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAccountNO" runat="server" Width="300px"  Cssclass="nonEditabletxtbox" BorderWidth="1px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblPatientFirstName" EnableViewState="false" runat="server" Text="First Name"  CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style2" colspan="2">
                                        <asp:TextBox ID="txtFirstName" runat="server" Width="300px"  Cssclass="nonEditabletxtbox" BorderWidth="1px"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="style8">
                                        <asp:Label ID="lblExternalAccoutNO" EnableViewState="false" runat="server" Text="Ext. Acc #"  CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtExternalAccountNO" runat="server" Width="300px"  Cssclass="nonEditabletxtbox"
                                            BorderWidth="1px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="lblPatientDOB" EnableViewState="false" runat="server" Text="DOB"  CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td class="style2" colspan="2">
                                        <asp:TextBox ID="txtPatientDOB" runat="server" Width="300px"  Cssclass="nonEditabletxtbox"
                                            BorderWidth="1px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td class="style8">
                                        <asp:Label ID="lblPatientSex" EnableViewState="False" runat="server" Text="Sex"  CssClass="Editabletxtbox"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPatientSex" runat="server" Width="300px"  Cssclass="nonEditabletxtbox"  BorderWidth="1px" ReadOnly="True"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="style1"></td>
                                    <td class="style2"></td>
                                    <td class="style6"></td>
                                    <td class="style8"></td>
                                    <td>
                                        <asp:CheckBox ID="chkShowAll" runat="server" Text="Show All" AutoPostBack="True" onclick="WaitCursor();"
                                            OnCheckedChanged="chkShowAll_CheckedChanged" CssClass="Editabletxtbox" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:Panel ID="pnlEligibilityHistory" runat="server" ScrollBars="Auto"  Width="840px" Height="285px">
                            <table style="width: 96%;">
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="grdEligibilityHIstory" runat="server" AutoGenerateColumns="False" Width="812px"
                                            EmptyDataText="No Records Found"
                                            CssClass="Gridbodystyle" CellPadding="3">
                                            <Columns>
                                                <asp:BoundField DataField="Policy Holder ID" HeaderText="Policy Holder Id" />
                                                <asp:BoundField DataField="Plan ID" HeaderText="Plan #" />
                                                <asp:BoundField DataField="Plan Name" HeaderText="Plan Name" />
                                                <asp:BoundField DataField="Group No" HeaderText="Group #" />
                                                <asp:BoundField DataField="Effective Start Date" HeaderText="Eff. Start Date" />
                                                <asp:BoundField DataField="Termination Date" HeaderText="Term. Date" />
                                                <asp:BoundField DataField="PCP Copay" HeaderText="PCP Copay $" />
                                                <asp:BoundField DataField="SPC Copay" HeaderText="SPC Copay $" />
                                                <asp:BoundField DataField="Eligibility Verified By" HeaderText="EV by" />
                                                <asp:BoundField DataField="Eligibility Verified Date" HeaderText="EV Date" />
                                            </Columns>
                                            <SelectedRowStyle CssClass="gridSelectedRow" />
                                            <HeaderStyle CssClass="Gridheaderstyle" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <table>
                            <tr>
                                <td colspan="3" style="width: 87%"></td>
                                <td>
                                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="99px" OnClientClick="CloseEligibilityHistoryWindow();"
                                        EnableViewState="false" CssClass="aspresizedredbutton" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:HiddenField ID="hdnInsPlanID" runat="server" EnableViewState="false" />
                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                    <script src="JScripts/JSDemographics.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                    <script src="JScripts/jquery-1.7.1.min.js" type="text/javascript"></script>
                    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
