<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmOtherHistory.aspx.cs"
    Inherits="Acurus.Capella.UI.frmOtherHistory" EnableEventValidation="false" ValidateRequest="false"  %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Other History</title>
  <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>


    <style type="text/css">
        .displayNone {
            display: none;
        }

        .underline {
            text-decoration: underline;
        }

        .style64 {
            width: 480px;
        }
    </style>

    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="loadotherHistory();{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server" style="background-color: White; height:584px; font-size: small; margin-bottom: 0px; font-family: Microsoft Sans Serif; width: 100%;">
       
         <telerik:RadWindowManager ID="WindowMngr" runat="server" IconUrl="Resources/16_16.ico" EnableViewState="False">
            <Windows>
                <telerik:RadWindow ID="MessageWindowAD" runat="server" Behaviors="Close" EnableViewState="False"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableTheming="True" EnableViewState="False">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                <table style="width: 100%; height: 100%">
                    <tr style="height: 25%">
                        <td style="width: 100%">
                            <asp:Panel ID="pnlAdvancedDirectives" runat="server" GroupingText="Advanced Directives"
                                Height="100%" Width="100%" Font-Bold="true" Font-Size="Small" CssClass="Editabletxtbox">
                                <table style="height: 100%; width: 100%; font-family: Microsoft Sans Serif; font-size: small;">
                                    <tr>
                                        <td style="font-family: Microsoft Sans Serif;">
                                            <asp:Label ID="lblDiscussionOfAdvancedDirectives" runat="server" Text="Discussion of Advanced Directives?" Font-Bold="False"  EnableViewState="False" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="cboAdvancedDirectived" runat="server" OnClientSelectedIndexChanged="cboAdvancedDirectived_SelectedIndexChanged" CssClass="Editabletxtbox">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value="0" />
                                                    <telerik:RadComboBoxItem Text="YES" Value="1" />
                                                    <telerik:RadComboBoxItem Text="NO" Value="2" />
                                                </Items >
                                            </telerik:RadComboBox>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td><asp:Label ID="lblFilestatus" runat="server"  Font-Bold="False" CssClass="Editabletxtbox" >File Status: </asp:Label> </td>
                                                    <td><asp:Label ID="lblStaus" runat="server" Text="Not Available" ForeColor="Red" Font-Bold="False" CssClass="Editabletxtbox"></asp:Label> 
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <a runat="server" id="FileLink" onserverclick="FileLink_ServerClick" style="pointer-events:none;color:gray;">View</a> <%-- style="pointer-events:none;color:gray;"--%>
                                                       </td>
                                                    </tr>
                                            </table></td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="font-family: Microsoft Sans Serif;">
                                            <asp:Label ID="lblComments" runat="server" Text="Comments" Font-Bold="False" EnableViewState="False" CssClass="Editabletxtbox"></asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                                Font-Size="Small" Font-Bold="false">
                                                <DLC:DLC runat="server" ID="DLC" Enable="True" TextboxHeight="45px" TextboxWidth="520px" TextboxMaxLength="1000"
                                                    Value="ADVANCE DIRECTIVE COMMENTS" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td valign="top">
                                            <asp:Panel ID="pnlsaveDelete" runat="server" Width="100%" Height="100%" Font-Bold="false">
                                                <table style="width: 100%; height: 100%">
                                                    <tr>
                                                        <td style="width: 90%;" valign="top" class="style64" align="right">
                                                            <telerik:RadButton ID="btnSave" runat="server" Text="Save"
                                                                OnClick="btnSave_Click" OnClientClicked="btnSave_Clicked"
                                                                AccessKey="S" Style="position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px; border-radius: 6px;  font-size: 12px; height: 31px !important;"  ButtonType="LinkButton" CssClass="greenbutton"
                                                                Width="60px">
                                                                <ContentTemplate>
                                                                    <span>S</span>ave
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td style="width: 10%;" valign="top" align="right">
                                                            <telerik:RadButton ID="btnClearAll" runat="server" Text="Clear All" OnClientClicked="btnClearAll_Clicked"
                                                                AutoPostBack="false" AccessKey="C" Style="left: 0px; top: 0px; width: 76px; position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px; border-radius: 6px;  font-size: 12px; height: 31px !important;" ButtonType="LinkButton" CssClass="redbutton" 
                                                                Width="86px">
                                                                <ContentTemplate>
                                                                    <span >C</span>lear All
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
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
                    <tr style="height: 65%">
                        <td style="width: 100%" valign="top">
                            <asp:Panel ID="pnlProvider" runat="server" GroupingText="List of Specialists involved in member's medical care"
                                Height="100%" Width="100%" Font-Bold="true" Font-Size="Small">
                                <table style="width: 100%; height: 100%; font-family: Microsoft Sans Serif; font-size: small;">
                                    <tr style="font-family: Microsoft Sans Serif; font-size: small;">
                                        <td style="width: 10%;" >
                                            <asp:Label ID="lblProviderName" runat="server" Text="Provider Name*" Font-Bold="False" EnableViewState="False"  mand="Yes" CssClass="MandLabelstyle"  >
                                               </asp:Label>
                                          <%--  <span id="spanProviderName" runat="server" class="MandLabelstyle">Provider Name*</span>--%>
                                        </td>
                                        <td style="width: 40%;" >
                                            <telerik:RadTextBox ID="txtProviderName" runat="server" Height="36px" Style="margin-right: 1px" EnableViewState="False" CssClass="Editabletxtbox"
                                                MaxLength="50" Width="440px">
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <EmptyMessageStyle Resize="None" />
                                                <ClientEvents OnKeyPress="txtProviderName_OnKeyPress"
                                                    OnValueChanged="txtProviderName_OnValueChanged" />
                                                <FocusedStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <telerik:RadButton ID="btnFindProvider" runat="server" Text="Find Provider" OnClientClicked="OpenReferralPhysician"
                                                AccessKey="F" Style="position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px; border-radius: 6px;  font-size: 12px; height: 32px !important;"  AutoPostBack="false"  ButtonType="LinkButton" CssClass="bluebutton">
                                                <ContentTemplate>
                                                    <span >F</span>ind Provider
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                        <td style="width: 40%;" ></td>
                                    </tr>
                                    <tr>
                                        <td style="font-family: Microsoft Sans Serif; font-size: small; width: 10%;">
                                            <asp:Label ID="lblSpecialty" runat="server" Text="Specialty" Font-Bold="False" CssClass="Editabletxtbox" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td style="width: 40%;">
                                            <telerik:RadTextBox ID="txtSpecialty" runat="server" Height="34px" Style="margin-right: 0px" EnableViewState="False" CssClass="Editabletxtbox"
                                                MaxLength="50" Width="437px">
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <EmptyMessageStyle Resize="None" />
                                                <ClientEvents OnKeyPress="txtSpecialty_OnKeyPress" />
                                                <FocusedStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Label ID="lblPhoneNumber" runat="server" Text="Phone Number" Font-Bold="False" CssClass="Editabletxtbox" EnableViewState="False"></asp:Label>
                                        </td>
                                        <td style="width: 40%;">
                                            <telerik:RadMaskedTextBox ID="msktxtTelephone" runat="server" Mask="(###) ###-####" EnableViewState="False"
                                                Height="28px" Style="margin-right: 0px; margin-left: 0px;" Font-Bold="False" CssClass="Editabletxtbox"
                                                Width="147px">
                                                <ClientEvents OnKeyPress="msktxtTelephone_OnKeyPress" />
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
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnClearAllAdvancedDirectivedHidden" runat="server" CssClass="displayNone"
                                                Text="Button" OnClick="btnClearAllAdvancedDirectivedHidden_Click" />
                                        </td>
                                        <td></td>
                                        <td align="right">
                                            <asp:Panel ID="pnlPhysicianSaveandClearAll" runat="server" Width="100%" Height="100%" Font-Bold="False" CssClass="Editabletxtbox">
                                                <table style="width: 99%;">
                                                    <tr>
                                                        <td style="width: 80%;" valign="top" align="right">
                                                            <telerik:RadButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click"
                                                                OnClientClicked="btnAdd_Clicked"
                                                                AccessKey="A" Style="position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px; border-radius: 6px;  font-size: 12px; height: 31px !important; width: auto !important"  ButtonType="LinkButton" CssClass="greenbutton">
                                                                <ContentTemplate>
                                                                    <span id="SpanAdd" runat="server" >A</span><span id="SpanAddAddtionalText" runat="server">dd</span>
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td style="width: 20%;" valign="top" align="right">
                                                            <telerik:RadButton ID="btnPhysicianClearAll" runat="server" Text="Clear All"
                                                                OnClientClicked="btnPhysicianClearAll_Clicked"
                                                                AccessKey="l" Style="top: 0px; width: 77px; position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; padding: 4px 12px; border-radius: 6px;  font-size: 12px; height: 31px !important;" ButtonType="LinkButton" CssClass="redbutton" 
                                                                Width="82px">
                                                                <ContentTemplate>
                                                                    <span id="SpanPhysicianClearAllAddtionalTextOne" runat="server">C</span><span id="SpanPhysicianClearAll" runat="server" >l</span><span id="SpanPhysicianClearAllAddtionalTextTwo" runat="server">ear All</span>
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" valign="top">
                                            <asp:Panel ID="pnlGrid" runat="server" BackColor="White" 
                                                Font-Size="Small" Font-Bold="true" Height="100%" Width="100%" SkinID="">
                                                <telerik:RadGrid ID="grdProviderDeatils" runat="server" AutoGenerateColumns="False"  
                                                    CellSpacing="0" GridLines="Both" Font-Bold="False" Width="100%"
                                                    Height="250px" EnableTheming="False" OnItemCommand="grdProviderDeatils_ItemCommand" CssClass="Gridbodystyle" >
                                                    <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle" />
                                                     <ItemStyle CssClass="Gridbodystyle" />
                                                    <FilterMenu EnableImageSprites="False">
                                                    </FilterMenu>
                                                    <ClientSettings>
                                                        <ClientEvents OnCellSelected="grdProviderDeatils_OnCellSelected" />
                                                        <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                                    </ClientSettings>
                                                    <MasterTableView>
                                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                            <HeaderStyle Width="20px" CssClass="Gridheaderstyle"/>
                                                            </RowIndicatorColumn>
                                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                            <HeaderStyle Width="20px" CssClass="Gridheaderstyle"/>
                                                            </ExpandCollapseColumn>
                                                        <Columns>
                                                            <telerik:GridButtonColumn HeaderText="Edit" HeaderStyle-Width="50px" ButtonType="ImageButton"
                                                                CommandName="Edt"  Text="Edit" UniqueName="EditRows" ImageUrl="~/Resources/edit.gif"  >
                                                                <HeaderStyle Width="8px" CssClass="Gridheaderstyle" />
                                                                <ItemStyle  CssClass="Editabletxtbox" />
                                                                </telerik:GridButtonColumn>
                                                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this line item?"
                                                                ConfirmDialogType="RadWindow" ConfirmTitle="Delete" HeaderText="Delete" HeaderStyle-Width="50px"
                                                                ButtonType="ImageButton" CommandName="DeleteRows" Text="Delete" UniqueName="DeleteRows"  ConfirmDialogHeight="155px"
                                                                ImageUrl="~/Resources/close_small_pressed.png">
                                                                <HeaderStyle Width="8px" CssClass="Gridheaderstyle" />
                                                                <ItemStyle  CssClass="Editabletxtbox" />
                                                            </telerik:GridButtonColumn>
                                                            <telerik:GridBoundColumn DataField="ProviderName" FilterControlAltText="Filter ProviderName column"
                                                                HeaderText="Provider Name"  UniqueName="ProviderName" >
                                                                <HeaderStyle Width="50px" CssClass="Gridheaderstyle" />
                                                                <ItemStyle  Font-Bold="false" CssClass="Editabletxtbox"  />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="Specialty" FilterControlAltText="Filter Specialty column"
                                                                HeaderText="Specialty" UniqueName="Specialty">
                                                                <HeaderStyle Width="50px" CssClass="Gridheaderstyle" />
                                                                <ItemStyle Font-Bold="false" CssClass="Editabletxtbox" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="PhoneNumber" FilterControlAltText="Filter PhoneNumber column"
                                                                HeaderText="Phone Number" UniqueName="PhoneNumber">
                                                                <HeaderStyle Width="50px" CssClass="Gridheaderstyle" />
                                                                <ItemStyle  Font-Bold="false" CssClass="Editabletxtbox" />
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn DataField="ID" FilterControlAltText="Filter ID column" HeaderText="ID"
                                                                UniqueName="ID" Display="false">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                        <EditFormSettings>
                                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                            </EditColumn>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hdnAddEnable" runat="server" EnableViewState="False" />
            <asp:HiddenField ID="hdnAddorUpdate" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSaveEnable" runat="server" EnableViewState="false" />
            <asp:Button ID="btnPFSHAutoSave" runat="server" OnClientClick="return PFSHAutoSave();" CssClass="displayNone" />
            <asp:Button ID="InvisibleButton" runat="server" CssClass="displayNone" OnClick="InvisibleButton_Click" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSOtherHistory.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript" enableviewstate="False"></script>
            <script type="text/javascript">
                //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                //function EndRequestHandler(sender, args) {
                //    if (args.get_error() != undefined) {
                //        args.set_errorHandled(true);
                //    }
                //}
            </script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
