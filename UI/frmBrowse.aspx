<%@ Page Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmBrowse.aspx.cs" Inherits="Acurus.Capella.UI.frmBrowse" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Import</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #form1 {
            height: 100%;
            width: 100%;
        }

        .tr_Import {
            height: 30%;
        }

        .tr_Radiopanel {
            height: 40%;
        }

        .tr_Buttons {
            height: 30%;
        }

        .Receive_td {
            width: 25%;
        }
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="browseRadWindowManager" runat="server">
            <Windows>
                <telerik:RadWindow ID="browseRadWindow" runat="server" Behaviors="Close" Title="Summary" IconUrl="Resources/16_16.ico" ReloadOnShow="false">
                </telerik:RadWindow>
            </Windows>

        </telerik:RadWindowManager>
        <telerik:RadWindow ID="actLogRadWindow" IconUrl="~/Resources/16_16.ico" Height="217px"
            Width="527px" VisibleStatusbar="false" Behaviors="Close" Title="Activity Log"
            Style="display: none" Overlay="true" Modal="true" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtActivityLog" Wrap="true" TextMode="MultiLine"
                    Style="font-family: Microsoft Sans Serif; font-size: 8.5pt; resize:none" Height="168px" Width="503px"
                    runat="server" />

                <%--<asp:Button ID="btnClose" runat="server" Text="Close" />--%>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>

        <div style="height: 100%; width: 100%;">
            <asp:Panel ID="pnlMain" runat="server" Height="100%" Width="100%">
                <table style="width: 100%; height: 100%;">
                    <tr class="tr_Import">
                        <td class="tr_Import">
                            <asp:Panel ID="pnlSelectFile" runat="server" Height="100%" Width="100%" >
                                <table style="width: 100%; height: 100%;">
                                    <tr style="width: 100%; height: 100%; display: none;">
                                        <td style="width: 30%; height: 100%;">
                                            <telerik:RadTextBox ID="txtReceive" runat="server"></telerik:RadTextBox>
                                        </td>
                                        <td style="width: 20%; height: 100%;">
                                            <telerik:RadButton ID="btnReceive" runat="server" Text="Receive"
                                                OnClick="btnReceive_Click" OnClientClicked="btnReceive_Clicked">
                                            </telerik:RadButton>
                                        </td>
                                        <td style="width: 50%; height: 100%;">
                                            <input id="fuImport" runat="server" type="file" accept="application/xml" enableviewstate="true" />
                                        </td>
                                        <td style="width: 100%; height: 100%;">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <button id="btnReceiveMail" runat="server" name="btnReceiveMail" onclick="btnReceiveMail_Clicked();" onserverclick="btnReceiveMail_ServerClick" class="aspbluebutton">Refresh CCD Mail Box</button></td>
                                         <td>
                                            <button id="btnCernerDownload" runat="server" visible="false" name="btnCernerDownload" onserverclick="btnCernerDownload_ServerClick" onclick=" { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" class="aspbluebutton">Download from Cerner</button></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr class="tr_Radiopanel" style="display: none;">
                        <td class="tr_Radiopanel">
                            <asp:Panel ID="pnlStandard" runat="server" Font-Size="Small"
                                GroupingText="Standard" BackColor="#BFDBFF">
                                <table style="width: 100%; height: 100%;">
                                    <tr style="width: 100%; height: 100%;">
                                        <td style="height: 100%;">
                                            <asp:RadioButton ID="radCCDA_Ambulatory" runat="server"
                                                GroupName="standard" Text="CCDA_Ambulatory" />
                                        </td>
                                        <td style="height: 100%;">
                                            <asp:RadioButton ID="radCCDA_Inpatient" runat="server"
                                                GroupName="standard" Text="CCDA_Inpatient" />
                                        </td>
                                        <td style="height: 100%;">
                                            <asp:RadioButton ID="radC32" runat="server"
                                                GroupName="standard" Text="C32" />
                                        </td>
                                        <td style="height: 100%;">
                                            <asp:RadioButton ID="radCCR" runat="server"
                                                GroupName="standard" Text="CCR" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadGrid ID="grdImport" runat="server" AutoGenerateColumns="False"
                                EnableTheming="False" CellSpacing="0" GridLines="Both"
                                Width="100%" CssClass="Gridbodystyle" Height="150px">
                                <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle"  />
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                                <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="false">
                                    <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                    <Selecting AllowRowSelect="true" />
                                    <ClientEvents OnRowSelected="clearSelectedItemsgrdNegative" />
                                </ClientSettings>
                                <MasterTableView>
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <%-- <telerik:GridButtonColumn ButtonType="LinkButton" CommandName="FileName" UniqueName="FileName" HeaderStyle-Width="16%" HeaderText="Name" DataTextField="Name">
                      <ItemStyle Font-Bold="false" Font-Names="Microsoft Sans Serif" Font-Size="8.5pt" BorderColor="#CCFFFF" BorderStyle="Dotted" />
                  </telerik:GridButtonColumn>--%>
                                        <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter FileName column" HeaderText="Name" UniqueName="FileName" Resizable="False">
                                            <HeaderStyle Width="40%" />
                                            <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                                            HeaderText="Type" UniqueName="Type" Resizable="False">
                                            <HeaderStyle Width="16%" />
                                            <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel runat="server" GroupingText="Verified Invalid Files" CssClass="LabelStyle">
                                <telerik:RadGrid ID="grdVerifiedNegativeFiles" runat="server" AutoGenerateColumns="False"
                                    EnableTheming="False" CellSpacing="0" GridLines="Both"  OnItemCommand="grdVerifiedNegativeFiles_ItemCommand"
                                    Width="100%" Height="150px" CssClass="Gridbodystyle">
                                    <HeaderStyle Font-Bold="true" CssClass="Gridheaderstyle" />
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                    <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="false">
                                        <Scrolling AllowScroll="True" ScrollHeight="270px" UseStaticHeaders="True" />
                                        <Selecting AllowRowSelect="true"/>
                                         <ClientEvents OnRowSelected="clearSelectedItemsgrdImport" />
                                    </ClientSettings>
                                    <MasterTableView>
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="Name" FilterControlAltText="Filter FileName column" HeaderText="Name" UniqueName="FileName" Resizable="False">
                                                <HeaderStyle Width="60%" />
                                                <ItemStyle CssClass="Editabletxtbox" />
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="Type" FilterControlAltText="Filter Type column"
                                                HeaderText="Type" UniqueName="Type" Resizable="False">
                                                <HeaderStyle Width="16%" />
                                                <ItemStyle CssClass="Editabletxtbox"/>
                                            </telerik:GridBoundColumn>
                                             <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="DeleteRow" FilterControlAltText="Filter Remove column"
                                            HeaderText="Remove" ImageUrl="~/Resources/close_small_pressed.png" UniqueName="Remove" ConfirmDialogType="RadWindow" ConfirmTitle="Capella - Confirmation" ConfirmDialogWidth="300px" ConfirmDialogHeight="120px"  ConfirmText="Are you sure want to delete the file?">
                                                  <HeaderStyle Width="16%" />
                                        <ItemStyle CssClass="Editabletxtbox" />
                                        </telerik:GridButtonColumn>
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
                    <tr class="tr_Buttons">
                        <td>
                            <asp:Panel ID="pnlButton" runat="server" Height="70%" Width="100%">
                                <table style="width: 100%; height: 75%;">
                                    <tr style="width: 100%; height: 100%;">
                                        <td width="20%">
                                            <asp:LinkButton ID="lnkActiveHistory" runat="server" Style="font-weight: 700;" CssClass="alinkstyle">Activity History</asp:LinkButton>
                                        </td>
                                        <td width="55%"></td>
                                        <td width="15%">
                                            <%--  <telerik:RadButton ID="btnOk" runat="server" Text="Ok" 
                                              width="100%" OnClick="btnOk_Click" onclientclicked="btnOk_Clicked">
                                        </telerik:RadButton>--%>
                                            <button id="btnOk" runat="server" name="btnOk" onclick="return btnOk_Clicked();" class="aspgreenbutton" onserverclick="btnOk_Click" style="width: 100%;">View</button>
                                        </td>
                                        <td width="15%">
                                            <%--<telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" 
                                            onclientclicked="btnCancelClient_Clicked" width="100%">
                                        </telerik:RadButton>--%>

                                            <asp:Button ID="btnCance" runat="server" Text="Cancel" Style="width: 100%;" CssClass="aspredbutton"
                                                OnClientClick="return btnCancelClient_Clicked();" CausesValidation="False" />

                                            <%--<button id="btnCancel" runat="server" name="btnCancel" onclick="return btnCancelClient_Clicked();" style="width:100%;">Cancel</button>--%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </div>


        <asp:HiddenField ID="hdnFilePath" runat="server" />
        <asp:HiddenField ID="hdnCCR" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" runat="server" />
        <asp:HiddenField ID="hdnFileCnt" runat="server" />
        <%--BugID:48547--%>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSBrowse.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
