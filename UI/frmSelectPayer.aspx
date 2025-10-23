<%@ Page  Async="true" Language="C#" CodeBehind="frmSelectPayer.aspx.cs" Inherits="Acurus.Capella.UI.frmSelectPayer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Select Payer</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #frmSelectPayer
        {
            width: 831px;
            height: 485px;
            margin-bottom: 4px;
        }
        .style19
        {
            width: 446px;
        }
        .style21
        {
            width: 210px;
        }
        .style22
        {
            width: 279px;
        }
    </style>
   <%-- <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />--%>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodybackground" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmSelectPayer" runat="server">
    <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div>
        <asp:ScriptManager ID="ScriptMngr" runat="server" EnableViewState="false">
        </asp:ScriptManager>
        <div>
            <asp:Panel ID="pnlSearchParameter" runat="server" Font-Size="Small">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCarrierName" EnableViewState="false" class="spanstyle" runat="server" Text="Carrier Name"
                                Width="90px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCarrierName" EnableViewState="false" Class="Editabletxtbox" runat="server" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblPanID" EnableViewState="false" runat="server" Class="spanstyle" Text="Plan #"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPlanID" EnableViewState="false" runat="server" Class="Editabletxtbox" Width="200px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblPlanName" EnableViewState="false" runat="server" Class="spanstyle" Text="Plan Name"
                                Width="70px"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPlanName" EnableViewState="false" Class="Editabletxtbox" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPayerCity" EnableViewState="false" runat="server" class="spanstyle" Text="City"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCityName" EnableViewState="false"  runat="server" Class="Editabletxtbox" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblZipCode" EnableViewState="false" runat="server" Class="spanstyle" Text="Zip Code"
                                Width="60px"></asp:Label>
                        </td>
                        <td dir="ltr">
                            <asp:TextBox ID="txtZipCode" EnableViewState="false" runat="server" Class="Editabletxtbox" Width="200px"></asp:TextBox>
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
                            <asp:Label ID="lblError" EnableViewState="false" class="spanstyle" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td dir="ltr">
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Class="aspbluebutton" OnClick="btnSearch_Click" Text="Search"
                                AccessKey="S" Width="100px" />
                        </td>
                        <td>
                            <asp:Button ID="btnClearAll" runat="server" Class="aspredbutton" OnClick="btnClearAll_Click" Text="Clear All"
                                AccessKey="A" Width="100px" OnClientClick="return ConfirmClearAll();" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div style="width: 821px">
            <asp:HiddenField ID="hdnSelectedIndex" runat="server" EnableViewState="false" />
            <asp:Panel ID="pnlPayerGrid" runat="server" GroupingText="Payer Information" CssClass="LabelStyleBold" BackColor="White"
                BorderStyle="Double">
                <asp:UpdatePanel ID="updtPnlPayer" runat="server">
                    <ContentTemplate>
                        <telerik:RadGrid ID="grdPayerInformation" runat="server" AutoGenerateColumns="False"
                            CellSpacing="0" GridLines="None" Height="230px" 
                            ClientSettings-EnablePostBackOnRowClick="false" EnableViewState="False" CssClass="Gridbodystyle">
                            <HeaderStyle Font-Bold="true"  />
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="true" />
                                <ClientEvents OnRowClick="grdPayerInformation_OnRowClick" OnRowDblClick="grdPayerInformation_OnRowDblClick" />
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                <KeyboardNavigationSettings EnableKeyboardShortcuts="true"></KeyboardNavigationSettings>
                                <Resizing AllowResizeToFit="true" />
                            </ClientSettings>
                            <MasterTableView NoMasterRecordsText="" ShowHeadersWhenNoRecords="true" CssClass="Gridbodystyle">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="Carrier Id" FilterControlAltText="Filter ZipCode column"
                                        HeaderText="Carrier Id" UniqueName="CarrierId" ItemStyle-Width="100%" 
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%"   />
                                        </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Carrier Name" FilterControlAltText="Filter CarrierName column"
                                        HeaderText="Carrier Name" UniqueName="CarrierName" ItemStyle-Width="100%" HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%"  />
                                        </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Plan ID" FilterControlAltText="Filter Plan# column"
                                        HeaderText="Plan #" UniqueName="Plan#" ItemStyle-Width="100%" 
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Ins Plan Name" FilterControlAltText="Filter InsPlanName column"
                                        HeaderText="Ins Plan Name" UniqueName="InsPlanName" ItemStyle-Width="100%" 
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%"  />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Financial Class" FilterControlAltText="Filter FinancialClass column"
                                        HeaderText="Financial Class" UniqueName="FinancialClass" 
                                        ItemStyle-Width="100%" HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%"  />
                                   </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Address" FilterControlAltText="Filter Address column"
                                          HeaderText="Address" UniqueName="Address" ItemStyle-Width="100%" 
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%" />
                                                 </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="City" FilterControlAltText="Filter City column"
                                        HeaderText="City" UniqueName="City" ItemStyle-Width="100%" 
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%"  />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="State" FilterControlAltText="Filter State column"
                         HeaderText="State" UniqueName="State" ItemStyle-Width="100%" 
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Zip Code" FilterControlAltText="Filter ZipCode column"
           HeaderText="Zip Code" UniqueName="ZipCode" ItemStyle-Width="100%"
                                        HeaderStyle-Width="100%">
                                        <HeaderStyle Width="100%"  />
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div style="width: 808px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlNavigation" runat="server" Font-Size="Small" Width="808px">
                        <table>
                            <tr>
                                <td class="style22">
                                <asp:LinkButton ID="btnFirst" runat="server" OnCommand="PageChangeEventHandler" CommandArgument="First"
                                        OnClientClick="ShowLoadingFindPatient(this);">First</asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btnPrevious" runat="server" OnCommand="PageChangeEventHandler"
                                        OnClientClick="ShowLoadingFindPatient(this);" CssClass="Editabletxtbox" CommandArgument="Previous">Previous</asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btnNext" runat="server" CssClass="Editabletxtbox" OnCommand="PageChangeEventHandler" OnClientClick="ShowLoadingFindPatient(this);"
                                        CommandArgument="Next">Next</asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="btnLast" runat="server" CssClass="Editabletxtbox" OnCommand="PageChangeEventHandler" OnClientClick="ShowLoadingFindPatient(this);"
                                        CommandArgument="Last">Last</asp:LinkButton>
                                </td>
                                <td class="style21">
                                    <asp:Label ID="lblShowing" runat="server" EnableViewState="false"></asp:Label>
                                </td>
                                <td class="style19">
                                </td>
                                <td>
                                    <asp:Button ID="btnPlanLibrary" runat="server" Class="aspbluebutton" Text="Plan Library" OnClientClick="return OpenPlanWindow();"
                                        AccessKey="P" EnableViewState="false" Enabled ="false"/>
                                </td>
                                <td>
                                    <asp:Button ID="btnOk" Class="aspbluebutton" runat="server" OnClick="btnOk_Click" OnClientClick="return closeWindow();"
                                        AccessKey="O" Text="OK" Width="60px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancel" runat="server" Class="aspredbutton" Text="Cancel" OnClientClick="return CancelFromSelectPayer();"
                                        AccessKey="C" EnableViewState="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="hdnLastPageNo" EnableViewState="false" runat="server" />
    <asp:HiddenField ID="hdnCarrierID" EnableViewState="false" runat="server" />
    <asp:HiddenField ID="hdnTotalCount" EnableViewState="false" runat="server" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server">
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    </asp:PlaceHolder>
    </form>
</body>
</html>
