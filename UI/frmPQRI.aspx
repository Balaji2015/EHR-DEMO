<%@ Page  Async="true" Language="C#" Title="PQRI" AutoEventWireup="true" CodeBehind="frmPQRI.aspx.cs"
    Inherits="Acurus.Capella.UI.frmPQRI" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .dropmenuScroll
        {
            height: 30px;
            max-height: 30px;
            overflow-y: scroll;
            position: static;
        }
    </style>
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmPQRI" runat="server" style="width: 781px; height: 165px;">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
        <Windows>
            <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div>
        <asp:UpdatePanel ID="updatePnl" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlPQRI" runat="server" Width="781px" Height="161px" Font-Size="Small"
                    GroupingText="PQRI"  CssClass="Editabletxtbox" Font-Bold="true">
                    <table style="width: 100%; table-layout: fixed;">
                        <tr>
                            <td style="width: 14%">
                            </td>
                            <td style="width: 3%; font-family: sans-serif; font-size: small;" align="center">
                                <asp:Label ID="lblY" runat="server" Text="Y" Width="3%" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td style="width: 3%; font-family: sans-serif; font-size: small;" align="center">
                                <asp:Label ID="lblN" runat="server" Text="N" Width="3%" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td style="width: 25%; font-family: sans-serif; font-size: small;" align="center">
                                <asp:Label ID="lblOptions" runat="server" Text="Options" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td style="width: 30%">
                            </td>
                            <td style="width: 25%; font-family: sans-serif; font-size: small;" align="center">
                                <asp:Label ID="lblNotes" runat="server" Text="Notes" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; font-family: sans-serif; font-size: small;">
                                <asp:Label ID="lblTobaccoUse" runat="server" Text="Tobacco Use" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td style="width: 5%">
                                <asp:CheckBox ID="chkTobaccoUseYes" runat="server" Text="" Width="3%" AutoPostBack="true"
                                    TabIndex="0" onclick="chkTobaccoUseYes_Clicked();" EnableViewState="false" OnCheckedChanged="chkTobaccoUseYes_CheckedChanged"
                                     />
                            </td>
                            <td style="width: 5%">
                                <asp:CheckBox ID="chkTobaccoUseNo" runat="server" AutoPostBack="true" Text="" Width="3%"
                                    TabIndex="1" EnableViewState="false" onclick="chkTobaccoUseNo_Clicked();" OnCheckedChanged="chkTobaccoUseNo_CheckedChanged" />
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="cboTobaccoUse" runat="server" Width="100%" CssClass="Editabletxtbox"/>
                            </td>
                            <td style="width: 30%">
                                <asp:CheckBox ID="chkTobaccoCessation" runat="server" EnableViewState="false" Text="Cessation Intervention Done"
                                    onchange="enableButton();" TabIndex="3" onclick="chkTobaccoCessation_Clicked();"
                                      CssClass="Editabletxtbox"/>
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="cboTobaccoCessationComments" runat="server" onchange="enableButton();"
                                    TabIndex="4" Width="100%" CssClass="Editabletxtbox"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; font-family: sans-serif; font-size: small;">
                                <asp:Label ID="lblSmokingHabit" runat="server" Text="Smoking Habit" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td style="width: 5%">
                                <asp:CheckBox ID="chkSmokingHabitYes" runat="server" Text="" Width="3%" TabIndex="5"
                                    onchange="enableButton();" onclick="chkSmokingHabitYes_Clicked();"  />
                            </td>
                            <td style="width: 5%">
                                <asp:CheckBox ID="chkSmokingHabitNo" runat="server" Text="" EnableViewState="false"
                                    Width="3%" TabIndex="6" onchange="enableButton();" onclick="chkSmokingHabitNo_Clicked();" />
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="cboSmokingHabit" runat="server" TabIndex="7" onchange="enableButton();"
                                    Width="100%" CssClass="Editabletxtbox"/>
                            </td>
                            <td style="width: 30%">
                                <asp:CheckBox ID="chkSmokingCessation" runat="server" EnableViewState="false" Text="Cessation Intervention Done"
                                    onchange="enableButton();" TabIndex="8" onclick="chkSmokingCessation_Clicked();"
                                      CssClass="'Editabletxtbox"/>
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="cboSmokingCessationComments" runat="server" TabIndex="9" onchange="enableButton();"
                                    Width="100%" CssClass="Editabletxtbox"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%; font-family: sans-serif; font-size: small;">
                                <asp:Label ID="lblBMI" runat="server" Text="BMI" CssClass-="Editabletxtbox"></asp:Label>
                            </td>
                            <td style="width: 5%" colspan="2">
                                <telerik:RadTextBox ID="txtBmi" runat="server" TabIndex="10" Width="100%" ReadOnly="True" CssClass="nonEditabletxtbox">
                                </telerik:RadTextBox>
                            </td>
                            <td style="width: 25%">
                                <asp:Label ID="lblBMIStatus" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:CheckBox ID="chkBMIFollowUp" runat="server" EnableViewState="false" Text="FollowUp Plan"
                                    TabIndex="11" onchange="enableButton();" onclick="chkBMIFollowUp_Clicked();" 
                                      CssClass="Editabletxtbox"/>
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="cboFollowUpComments" runat="server" TabIndex="12" Width="100%"
                                    onchange="enableButton();" CssClass="Editabletxtbox"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                            </td>
                            <td style="width: 5%">
                            </td>
                            <td style="width: 5%">
                            </td>
                            <td style="width: 25%">
                                &nbsp;
                            </td>
                            <td style="width: 30%">
                            </td>
                            <td style="width: 25%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 9%">
                                        </td>
                                        <td style="width: 45%; font-family: sans-serif; font-size: small;" align="right">
                                            <%--<telerik:RadButton ID="btnSave" runat="server" Text="Save" TabIndex="13" OnClick="btnSave_Click"
                                        AutoPostBack="true" Width="75px">
                                    </telerik:RadButton>--%>
                                            <%--<asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="13" OnClick="btnSave_Click"
                                                 Width="75px" OnClientClick="btnsave_click();"/>--%>
                                            <asp:Button Button ID="btnSave" runat="server" Text="Save" TabIndex="13" OnClick="btnSave_Click"
                                                Width="75px" OnClientClick="btnsave_click();" disabled="disabled" CssClass="aspresizedgreenbutton"/>
                                        </td>
                                        <td style="width: 45%; font-family: sans-serif; font-size: small;" align="right">
                                            <%--<telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="true"
                                        TabIndex="14" Width="75px" OnClientClicked="btnCancel_Clicked">
                                    </telerik:RadButton>--%>
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" TabIndex="14" Width="75px"
                                                OnClientClick="return btnCancel_Clicked();" CausesValidation="False" CssClass="aspresizedredbutton"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
            OnClientClick="return btnCancel_Clicked();" />
        <asp:HiddenField ID="hdnMessageType" runat="server" Value="" />
        <asp:HiddenField ID="chkTobaccoUseYesTag" runat="server" />
        <asp:HiddenField ID="chkSmokingHabbitYesTag" runat="server" />
        <asp:HiddenField ID="chkBMIFollowUpTag" runat="server" />
        <asp:HiddenField ID="Client_saveCheckingFlag" runat="server" />
        <asp:HiddenField ID="hdnCheckedChangeFlag" runat="server" />
    </div>
    </form>
</body>
</html>
<asp:PlaceHolder runat="server">
<script src="JScripts/JSPQRI.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
<script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
</asp:PlaceHolder>