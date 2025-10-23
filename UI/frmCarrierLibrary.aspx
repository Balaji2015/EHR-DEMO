<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmCarrierLibrary.aspx.cs"
    Inherits="Acurus.Capella.UI.frmCarrierLibrary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Carrier Library</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self"></base>
    <style type="text/css">
        #frmCarrierLibrary
        {
            width: 588px;
        }
        .style12
        {
            width: 12%;
        }
        .style13
        {
            width: 49px;
        }
        .style14
        {
            width: 11%;
        }
        .style15
        {
            width: 25%;
        }
        .style16
        {
            width: 29px;
        }
        .style17
        {
            width: 27px;
        }
    </style>
</head>
<body bgcolor="bfdbff">
    <form id="frmCarrierLibrary" runat="server">
    <div style="width: 587px">
        <aspx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
        </aspx:ToolkitScriptManager>
        <div>
            <asp:UpdatePanel ID="updtpnlCarrierInfo" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlCarrierInfo" runat="server" Font-Size="Small" BackColor="White">
                        <table style="width: 100%;" enableviewstate="false">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCarrierID" runat="server" EnableViewState="false" Text="Carrier #"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarrierID" runat="server" Width="245px" BackColor="#BFDDFF" BorderColor="Black"
                                        BorderWidth="1px" EnableViewState="false" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCarrierName" EnableViewState="false" runat="server" Text="Carrier Name*"
                                        ForeColor="Red" Width="100px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCarrierName" runat="server" Width="245px" OnTextChanged="txtCarrierName_TextChanged"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNACID" EnableViewState="false" runat="server" Text="NACID"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNAICID" EnableViewState="false" runat="server" Width="245px"
                                        onkeypress="Enable();"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="70px" OnClientClick="showTime();"
                                        AccessKey="S" OnClick="btnSave_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" Width="70px" OnClientClick="return ConfirmClearAll();"
                                        AccessKey="L" OnClick="btnClearAll_Click" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:UpdatePanel ID="updtPnlGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlGrid" runat="server" Font-Size="Small" ScrollBars="Vertical" BackColor="White"
                        Height="350px">
                        <asp:GridView ID="grdCarrierLibrary" runat="server" Width="100%" AutoGenerateColumns="False"
                            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                            CellPadding="3" EmptyDataText="No records" Height="300px" CssClass="Radcss_Vista"
                            OnRowCommand="grdCarrierLibrary_RowCommand" OnRowDataBound="grdCarrierLibrary_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="EditGridRow" CommandName="EditC" ImageUrl="~/Resources/edit.gif" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="DeleteGridRow" CommandName="DeleteRow" ImageUrl="~/Resources/close_small_pressed.png"
                                            OnClientClick="return confirmMessage();" visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Carrier ID" HeaderText="Carrier #" />
                                <asp:BoundField DataField="Carrier Name" HeaderText="Carrier Name" />
                                <asp:BoundField DataField="NAIC ID" HeaderText="NAIC ID" />
                            </Columns>
                            <SelectedRowStyle CssClass="gridSelectedRow" />
                            <HeaderStyle CssClass="GridHeaderRow" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:HiddenField ID="hdnUpdateCarrierID" runat="server" EnableViewState="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 588px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlButtons" runat="server" Font-Size="Small">
                        <table style="width: 100%;">
                            <tr>
                                <td class="style17">
                                    <asp:LinkButton ID="btnFirst" runat="server" CommandArgument="First" OnCommand="PageChangeEventHandler">First</asp:LinkButton>
                                </td>
                                <td class="style13">
                                    <asp:LinkButton ID="btnPrevious" runat="server" CommandArgument="Previous" OnCommand="PageChangeEventHandler">Previous</asp:LinkButton>
                                </td>
                                <td class="style16">
                                    <asp:LinkButton ID="btnNext" runat="server" CommandArgument="Next" OnCommand="PageChangeEventHandler">Next</asp:LinkButton>
                                </td>
                                <td class="style14">
                                    <asp:LinkButton ID="btnLast" runat="server" CommandArgument="Last" OnCommand="PageChangeEventHandler">Last</asp:LinkButton>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblShowing" runat="server" Text="Label" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="style15">
                                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="70px" OnClientClick="return CloseWindow();"
                                        AccessKey="C" EnableViewState="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:HiddenField EnableViewState="false" ID="hdnTotalCount" runat="server" />
                                </td>
                                <td class="style12">
                                </td>
                                <td class="style15">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:HiddenField EnableViewState="false" ID="hdnLastPageNo" runat="server" />
                                    <asp:HiddenField EnableViewState="false" ID="hdnLocalTime" runat="server" />
                                </td>
                                <td class="style12">
                                </td>
                                <td class="style15">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">

    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
    <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>
</body>
</html>
