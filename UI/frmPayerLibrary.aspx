<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmPayerLibrary.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPayerLibrary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plan Library</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self"></base>
    <style type="text/css">
        .style2
        {
            width: 146px;
        }
        .style4
        {
            width: 123px;
        }
        .style5
        {
            width: 988px;
        }
        .style6
        {
            width: 134px;
        }
        .style7
        {
        }
        .style8
        {
            width: 93px;
        }
        .style9
        {
            width: 43px;
        }
        .style10
        {
            width: 134px;
            height: 26px;
        }
        .style11
        {
            width: 136px;
            height: 26px;
        }
        .style12
        {
            width: 43px;
            height: 26px;
        }
        .style13
        {
            height: 26px;
        }
        .style14
        {
            width: 100px;
        }
        .style15
        {
            width: 100px;
            height: 26px;
        }
        .style16
        {
        }
        .style17
        {
            height: 26px;
            width: 16px;
        }
        .style18
        {
            width: 791px;
        }
        #frmPayerLibrary
        {
            width: 1031px;
        }
        .style19
        {
            width: 544px;
        }
        .style20
        {
            width: 396px;
        }
        .style21
        {
            width: 23px;
        }
    </style>
</head>
<body bgcolor="bfdbff" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmPayerLibrary" runat="server">
    <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="PayerModalWindow" runat="server" VisibleOnPageLoad="false"
                Height="625px" IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div>
        <aspx:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableViewState="false">
        </aspx:ToolkitScriptManager>
        <div>
            <asp:UpdatePanel ID="updtpnlNavigation" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlInformationsRequired" runat="server" Font-Size="Small" Width="1026px"
                        BackColor="White">
                        <table style="width: 100%;">
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblCarrierName" EnableViewState="false" runat="server" ForeColor="Red"
                                        Text="Carrier Name"></asp:Label>
                                </td>
                                <td class="style7" colspan="3">
                                    <asp:DropDownList ID="ddlCarrierName" runat="server" Width="360px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCarrierName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblInsPlanName" EnableViewState="false" runat="server" ForeColor="Red"
                                        Text="Ins Plan Name *"></asp:Label>
                                </td>
                                <td class="style16" colspan="4">
                                    <asp:TextBox ID="txtInsPlanName" runat="server" OnTextChanged="TextBox6_TextChanged"
                                        Style="margin-top: 0px" Width="360px" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style10">
                                    <asp:Label ID="lblStreetAddress1" EnableViewState="false" runat="server" Text="Address1"></asp:Label>
                                </td>
                                <td class="style11" colspan="3">
                                    <asp:TextBox ID="txtStreetAddress1" EnableViewState="false" runat="server" Width="360px" onkeypress="return RestrictSpecial(event)"></asp:TextBox>
                                </td>
                                <td class="style12">
                                </td>
                                <td class="style15">
                                    <asp:Label ID="lblOfficeNum" EnableViewState="false" runat="server" Text="Office #"></asp:Label>
                                </td>
                                <td class="style17" colspan="3">
                                    <asp:TextBox ID="msktxtOfficeNum" runat="server" OnTextChanged="TextBox6_TextChanged"
                                        Style="margin-top: 0px" Width="360px" onkeypress="return EnableAddInKeypress(event);" ></asp:TextBox>
                                </td>
                                <td class="style13">
                                    &nbsp;
                                </td>
                                <td class="style13">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblStreetAddress2" EnableViewState="false" runat="server" Text="Address 2"></asp:Label>
                                </td>
                                <td class="style7" colspan="3">
                                    <asp:TextBox ID="txtStreetAddress2" EnableViewState="false" runat="server" Width="360px" onkeypress="return RestrictSpecial(event)"></asp:TextBox>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblPlanNo" EnableViewState="false" runat="server" Text="Ext. Plan #"></asp:Label>
                                </td>
                                <td class="style16" colspan="3">
                                    <asp:TextBox ID="txtPlanNo" runat="server" OnTextChanged="TextBox6_TextChanged" Style="margin-top: 0px"
                                        Width="360px" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblCityStateZip" EnableViewState="false" runat="server" Text="City/State/Zip"></asp:Label>
                                </td>
                                <td class="style7">
                                    <asp:TextBox ID="txtCity" runat="server" OnTextChanged="TextBox6_TextChanged" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                </td>
                                <td class="style21">
                                    <asp:DropDownList ID="ddlState" runat="server" onchange="EnableAddForPayer();">
                                    </asp:DropDownList>
                                </td>
                                <td class="style8">
                                    <asp:TextBox ID="msktxtZip" runat="server" OnTextChanged="TextBox6_TextChanged" Style="margin-top: 0px" onkeypress="return EnableAddInKeypress(event);"></asp:TextBox>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblFinancialClassId" EnableViewState="false" runat="server" Text="Fin.Class ID"></asp:Label>
                                </td>
                                <td class="style16" colspan="3">
                                    <asp:DropDownList ID="ddlFinancialClassId" runat="server" Width="360px" onchange="EnableAddForPayer();">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblTelephone" EnableViewState="false" runat="server" Text="Phone #"></asp:Label>
                                </td>
                                <td class="style7" colspan="3">
                                    <asp:TextBox ID="msktxtTelephone" runat="server" Width="360px" OnTextChanged="TextBox6_TextChanged" onkeypress="return EnableAddInKeypress(event);"></asp:TextBox>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblPayerId" EnableViewState="false" runat="server" Text="Plan ID"></asp:Label>
                                </td>
                                <td class="style16" colspan="3">
                                    <asp:TextBox ID="txtPayerId" runat="server" OnTextChanged="TextBox6_TextChanged"
                                        Style="margin-top: 0px" BackColor="#BFDBFF" BorderColor="Black" BorderWidth="1px"
                                        ReadOnly="True" Width="360px" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblGovtType" EnableViewState="false" runat="server" Text="Govt.Type"></asp:Label>
                                </td>
                                <td class="style7" colspan="3">
                                    <asp:DropDownList ID="ddlGovtType" runat="server" Width="360px" onchange="EnableAddForPayer();">
                                    </asp:DropDownList>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblClaimAttention" EnableViewState="false" runat="server" Text="Claim Attn."></asp:Label>
                                </td>
                                <td class="style16" colspan="3">
                                    <asp:TextBox ID="txtClaimAttention" runat="server" Width="360px" OnTextChanged="TextBox6_TextChanged" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblClaimAddress" EnableViewState="false" runat="server" Text="Claim Address"></asp:Label>
                                </td>
                                <td class="style7" colspan="3">
                                    <asp:TextBox ID="txtClaimAddress" runat="server" Width="360px" OnTextChanged="TextBox6_TextChanged" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblClaimCity" EnableViewState="false" runat="server" Text="Claim City"></asp:Label>
                                </td>
                                <td class="style16" colspan="3">
                                    <asp:TextBox ID="txtClaimCity" runat="server" Width="360px" OnTextChanged="TextBox6_TextChanged" onkeypress="EnableAddForPayer();"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblClaimState" EnableViewState="false" runat="server" Text="Claim State"></asp:Label>
                                </td>
                                <td class="style7" colspan="3">
                                    <asp:DropDownList ID="ddlClaimState" runat="server" Width="360px" onchange="EnableAddForPayer();">
                                    </asp:DropDownList>
                                </td>
                                <td class="style9">
                                    &nbsp;
                                </td>
                                <td class="style14">
                                    <asp:Label ID="lblClaimZipCode" EnableViewState="false" runat="server" Text="Claim Zip Code"></asp:Label>
                                </td>
                                <td class="style16">
                                    <asp:TextBox ID="txtClaimZipCode" runat="server" Width="360px"
                                        Height="22px" OnTextChanged="TextBox6_TextChanged" onkeypress="return EnableAddInKeypress(event);"></asp:TextBox>
                                </td>
                                <td class="style16">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="style6">
                                    <asp:Label ID="lblPayerNotes" EnableViewState="false" runat="server" Text="Plan Notes"></asp:Label>
                                </td>
                                <td class="style7" colspan="10">
                                    <asp:TextBox ID="txtPayerNotes" OnTextChanged="TextBox6_TextChanged" runat="server" TextMode="MultiLine"
                                        Width="874px" onkeypress="EnableAddForPayer();" style="resize: none;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <aspx:MaskedEditExtender ID="msktxtZipCodeExtender" EnableViewState="false" Mask="99999-9999"
                        ClearMaskOnLostFocus="false" TargetControlID="msktxtZip" runat="server">
                    </aspx:MaskedEditExtender>
                    <aspx:MaskedEditExtender ID="msktxtHomeExtender" EnableViewState="false" Mask="(999) 999-9999"
                        ClearMaskOnLostFocus="false" TargetControlID="msktxtTelephone" runat="server">
                    </aspx:MaskedEditExtender>
                    <aspx:MaskedEditExtender ID="msktxtOfficeExtender" EnableViewState="false" Mask="(999) 999-9999"
                        ClearMaskOnLostFocus="false" TargetControlID="msktxtOfficeNum" runat="server">
                    </aspx:MaskedEditExtender>
                    <aspx:MaskedEditExtender ID="MaskedEditExtender1" EnableViewState="false" Mask="99999-9999"
                        ClearMaskOnLostFocus="false" TargetControlID="txtClaimZipCode" runat="server">
                    </aspx:MaskedEditExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:UpdatePanel ID="updtPnlButton" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlButton" runat="server" Font-Size="Small" Width="1026px">
                        <table style="width: 100%;" enableviewstate="false">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="style18">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="style2">
                                             <asp:Button ID="btnAdd" runat="server" Text="Add" Width="153px" CssClass="aspresizedgreenbutton" OnClick="btnAdd_Click" OnClientClick="return AssignUTCTime();"  />
                                </td>
                                <td>
                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" Width="153px"  CssClass="aspresizedredbutton" OnClientClick="return ConfirmClearAll();"
                                        OnClick="btnClearAll_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:UpdatePanel ID="updtpnlGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlGrid" runat="server" Font-Size="Small" Width="1026px" Height="300px"
                        CssClass="Radcss_Vista" BackColor="White" ScrollBars="Vertical" >
                        <asp:GridView ID="grdPayer" runat="server" AutoGenerateColumns="False" Width="1028px"
                            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px"
                            CellPadding="3" EmptyDataText="No Records" OnRowCommand="grdPayer_RowCommand">
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
                                <asp:BoundField DataField="Ins Plan Name" HeaderText="Ins. Plan Name" />
                                <asp:BoundField DataField="Address1" HeaderText="Address1" />
                                <asp:BoundField DataField="Address2" HeaderText="Address2" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="State" HeaderText="State" />
                                <asp:BoundField DataField="Zip Code" HeaderText="Zip Code" />
                                <asp:BoundField DataField="PayerID" HeaderText="Plan #" />
                                <asp:BoundField DataField="InsTypeCode" HeaderText="Ext. Plan #" />
                                <asp:BoundField DataField="Carrier ID" HeaderText="Carrier #" />
                            </Columns>
                            <SelectedRowStyle CssClass="gridSelectedRow" />
                            <HeaderStyle CssClass="GridHeaderRow" />
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlBottomButton" runat="server" Font-Size="Small" Width="1029px">
                        <table style="width: 100%;">
                            <tr>
                                <td class="style20">
                                    &nbsp;
                                    <asp:LinkButton ID="btnFirst" runat="server" CommandArgument="First" OnCommand="PageChangeEventHandler">First</asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="btnPrevious" runat="server" CommandArgument="Previous"
                                        OnCommand="PageChangeEventHandler">Previous</asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="btnNext" runat="server" CommandArgument="Next" OnCommand="PageChangeEventHandler">Next</asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="btnLast" runat="server" CommandArgument="Last" OnCommand="PageChangeEventHandler">Last</asp:LinkButton>
                                </td>
                                <td class="style19">
                                    &nbsp;
                                    <asp:Label ID="lblShowing" EnableViewState="false" runat="server"></asp:Label>
                                </td>
                                <td class="style5">
                                    &nbsp;
                                </td>
                                <td class="style4">
                                    <asp:Button ID="btnCarrierLibrary" runat="server" Text="Carrier Library" Width="153px" CssClass="aspresizedbluebutton"
                                        OnClientClick=" return OpenCarrierLibrary();" EnableViewState="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancel" EnableViewState="false" runat="server" Text="Cancel" Width="153px" CssClass="aspresizedredbutton" 
                                        OnClientClick="CloseCarrierWindow();" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style20">
                                    &nbsp;
                                    <asp:HiddenField ID="hdnLastPageNo" EnableViewState="false" runat="server" />
                                </td>
                                <td class="style19">
                                    &nbsp;
                                    <asp:HiddenField ID="hdnTotalCount" EnableViewState="false" runat="server" />
                                </td>
                                <td colspan="3">
                                    &nbsp;
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="displayNone" OnClick="btnRefresh_Click"
                                        Text="Refresh" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style20">
                                    &nbsp;
                                </td>
                                <td class="style19">
                                    <asp:HiddenField ID="hdnLocalTime" EnableViewState="false" runat="server" />
                                </td>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <p>
    </p>
     <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return CloseCarrierWindow();"/>
       <asp:UpdatePanel ID="updatePanel2" runat="server">
           <ContentTemplate>
                 <asp:HiddenField ID="hdnMessageType" runat="server" />
           </ContentTemplate>
       </asp:UpdatePanel>
     
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
    <link href="CSS/ElementStyles.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
   
   </asp:PlaceHolder>
    </form>
</body>
</html>
