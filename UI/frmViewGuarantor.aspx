<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmViewGuarantor.aspx.cs"
    Inherits="Acurus.Capella.UI.frmViewGuarantor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Guarantor</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self"></base>
    <style type="text/css">
        .style1 {
            width: 82px;
        }

        .style2 {
            width: 172px;
        }

        .style5 {
            width: 82px;
            height: 30px;
        }

        .style6 {
            width: 172px;
            height: 30px;
        }

        .style9 {
            height: 30px;
        }

        .style11 {
            width: 998px;
        }

        .style12 {
            width: 101px;
        }

        .displayNone {
            display: none;
        }

        #frmViewGuarantor {
            width: 797px;
        }

        .Panel legend {
            font-weight: bold;
        }

        .style18 {
            width: 58px;
        }

        .style20 {
            width: 58px;
            height: 30px;
        }

        .style22 {
            width: 126px;
        }

        .style23 {
            height: 30px;
            width: 126px;
        }

        .style25 {
            height: 30px;
            width: 70px;
        }

        .style26 {
            width: 4px;
        }

        .style28 {
            height: 30px;
            width: 4px;
        }

        .style29 {
            width: 75px;
        }
    </style>
    <link href="CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body class="bodybackground">
    <form id="frmViewGuarantor" runat="server">
        <div style="width: 799px">
            <div>
                <asp:Panel ID="pnlPatientInfo" runat="server" Font-Size="Small" CssClass="Panel LabelStyleBold"
                    GroupingText="Patient Details" Width="793px">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblAccountNo" runat="server" Text="Acc. #" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="txtAccountNO" runat="server" BorderColor="Black" 
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td class="style18">
                                <asp:Label ID="lblPatientLastName" runat="server" Text="Last Name" Width="68px" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="style22">
                                <asp:TextBox ID="txtPatinetLastName" runat="server" BorderColor="Black" 
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td class="style26">&nbsp;
                            </td>
                            <td class="style29">&nbsp;
                            <asp:Label ID="lblPatientFirstName" runat="server" Text="First Name" Width="64px" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatinetFirstName" runat="server" BorderColor="Black"
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPatientDOB" runat="server" Text="DOB" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="txtPatientDOB" runat="server" BackColor="#BFDBFF" BorderColor="Black"
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td class="style18">
                                <asp:Label ID="lblPatientSex" runat="server" Text="Sex" EnableViewState="False" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="style22">
                                <asp:TextBox ID="txtPatientSex" runat="server" BorderColor="Black"
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td class="style26">&nbsp;
                            </td>
                            <td class="style29">&nbsp;
                            <asp:Label ID="lblExternalAccount" runat="server" Text="Ext. Acc.#" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExternalAccountNO" runat="server" BorderColor="Black"
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5">
                                <asp:Label ID="lblPatientType" runat="server" Text="Patient Type" EnableViewState="false" CssClass="spanstyle"></asp:Label>
                            </td>
                            <td class="style6">
                                <asp:TextBox ID="txtPatientType" runat="server" BorderColor="Black"
                                    BorderWidth="1px" ReadOnly="True" Width="160px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                            <td class="style20">&nbsp;
                            </td>
                            <td class="style23">&nbsp;
                            </td>
                            <td class="style28">&nbsp;
                            </td>
                            <td class="style25">&nbsp;
                            </td>
                            <td class="style9">&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pnlGuarantorList" runat="server" Font-Size="Small" GroupingText="Guarantor Details"
                    BackColor="White" CssClass="Panel LabelStyleBold" Width="790px" Height="400px" ScrollBars="Vertical">
                    <asp:GridView ID="grdGuarantorDetails" Width="100%" runat="server" AutoGenerateColumns="False"
                        AutoGenerateSelectButton="True" BackColor="White" BorderStyle="None"
                       BorderWidth="1px" CellPadding="3" OnSelectedIndexChanged="grdGuarantorDetails_SelectedIndexChanged"
                        OnRowDataBound="grdGuarantorDetails_RowDataBound" OnRowCommand="grdGuarantorDetails_RowCommand" CssClass="Gridbodystyle">
                   
                        <Columns>
                            <asp:BoundField AccessibleHeaderText="GuarantorID" DataField="GuarantorID" HeaderText="Acc. #" HeaderStyle-CssClass="Gridheaderstyle "/>
                            <asp:BoundField AccessibleHeaderText="GuarantorName" DataField="GuarantorName" HeaderText="Name" HeaderStyle-CssClass="Gridheaderstyle "/>
                            <asp:BoundField AccessibleHeaderText="GuarantorDOB" DataField="GuarantorDOB" HeaderText="DOB" HeaderStyle-CssClass="Gridheaderstyle " />
                            <asp:BoundField AccessibleHeaderText="HomePhone#" DataField="HomePhone#" HeaderText="Home Ph#" HeaderStyle-CssClass="Gridheaderstyle " />
                            <asp:BoundField AccessibleHeaderText="WorkPhone#" DataField="WorkPhone#" HeaderText="Work Ph#" HeaderStyle-CssClass="Gridheaderstyle " />
                            <asp:BoundField AccessibleHeaderText="Cell Phone#" DataField="Cell Phone#" HeaderText="Cell Ph#" HeaderStyle-CssClass="Gridheaderstyle " />
                            <asp:BoundField AccessibleHeaderText="RelationToPatient" DataField="RelationToPatient"
                                HeaderText="Rel. to Patient" HeaderStyle-CssClass="Gridheaderstyle "/>
                            <asp:BoundField AccessibleHeaderText="FromDate" DataField="FromDate" HeaderText="From Date" HeaderStyle-CssClass="Gridheaderstyle" />
                            <asp:BoundField AccessibleHeaderText="ToDate" DataField="ToDate" HeaderText="To Date" HeaderStyle-CssClass="Gridheaderstyle" />
                            <asp:BoundField AccessibleHeaderText="Active" DataField="Active" HeaderText="Active" HeaderStyle-CssClass="Gridheaderstyle" />
                            <asp:BoundField AccessibleHeaderText="ID" DataField="ID" HeaderText="ID" HeaderStyle-CssClass="displayNone"
                                ItemStyle-CssClass="displayNone">
                                
                            </asp:BoundField>
                            <asp:ButtonField CommandName="SingleClick" Text="SingleClick" Visible="False" />
                        </Columns>


                        <SelectedRowStyle CssClass="highlight" />
                        <HeaderStyle CssClass="Gridheaderstyle" />
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div>
                <asp:Panel ID="pnlButtons" runat="server" Font-Size="Small" Width="793px">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style11">&nbsp;
                            <asp:HiddenField ID="hdnLocalTime" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btnMakeInactive" runat="server" OnClick="btnMakeInactive_Click" Text="Make Inactive" CssClass="aspresizedbluebutton" />
                            </td>
                            <td class="style12">
                                <asp:Button ID="btnMakeActive" runat="server" Text="Make Active" OnClick="btnMakeActive_Click" CssClass="aspresizedbluebutton" />
                            </td>
                            <td>
                                <asp:Button ID="btnClose" runat="server" Text="OK" OnClientClick="CloseViewGuarantorWindow();" CssClass="aspresizedgreenbutton" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
        <asp:HiddenField ID="hdnGuaratorID" runat="server" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>

            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSDemographics.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
