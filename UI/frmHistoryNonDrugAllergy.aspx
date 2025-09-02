<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmHistoryNonDrugAllergy.aspx.cs"
    Inherits="Acurus.Capella.UI.frmHistoryNonDrugAllergy" EnableEventValidation="false" ValidateRequest="false"  %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Non Drug Allergy History</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
  <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>


    <style type="text/css">
        #frmNonDrugAllergy {
            height: 575px;
            width: 788px;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .riSingle .riTextBox {
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadInput textarea {
            vertical-align: bottom;
            overflow: auto;
        }

        .style1 {
            width: 562px;
        }

        .style3 {
            width: 174px;
        }

        .style16 {
            width: 122px;
        }

        .style17 {
            width: 5px;
        }

        .displayNone {
            display: none;
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .underline {
            text-decoration: underline;
        }
        #tag:hover {
            text-decoration: underline;
        }
    </style>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
     <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload="HistoryNonDrug_Load();OpenNotificationPopUp('PFSH-NON_DRUG_ALLERGY');">
    <form id="frmNonDrugAllergy" runat="server" style="width: 100%;   margin-bottom: 5px;">
        <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Non Drug Allergy"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" EnableViewState="false" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <div>
                <table style="width: 100%; height: 97%">
                    <tr style="width: 100%;">
                        <td style="height: 20%;" valign="top">
                            <%-- <asp:Panel ID="Panel2" runat="server" Width="100%" Height="100%">
                            <table style="width: 100%; height: 87%">
                                <tr style="height: 39%;">
                                    <td valign="top">
                                        <asp:Panel ID="Panel4" runat="server" Width="100%" Height="65%">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 17.2%" valign="top">
                                                    </td>
                                                    <td valign="top" align="center" style="width: 5%">
                                                        <asp:Label ID="lblAllOthers" runat="server" Font-Size="8.5pt" Font-Bold="true" EnableViewState="false"
                                                            Text="All Others"></asp:Label>
                                                    </td>
                                                    <td style="width:64%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr style="height: 30%;">
                                    <td valign="top">
                                        <asp:Panel ID="Panel5" runat="server" Width="100%" Height="76%">
                                            <table style="width: 100%; height: 16px;">
                                                <tr>
                                                    <td style="width:17.2%">
                                                    </td>
                                                    <td align="left" width="2.5%" valign="top">
                                                        <asp:Label ID="lblYes" runat="server" EnableViewState="false" Font-Size="8.5pt" Font-Bold="true"
                                                            Text="Yes"></asp:Label>
                                                    </td>
                                                    <td style="width: 2.5%;" align="right" valign="top">
                                                        <asp:Label ID="lblNo" runat="server" EnableViewState="false" Font-Size="8.5pt" Font-Bold="true"
                                                            Text="No"></asp:Label>
                                                    </td>
                                                    <td style="width:64%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr style="height: 30%;">
                                    <td valign="top">
                                        <asp:Panel ID="Panel6" runat="server" Width="100%" Height="78%">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 17.2%">
                                                    </td>
                                                    <td align="center" style="width: 2.5%" valign="top">
                                                        <asp:CheckBox ID="chkAllYes" runat="server" Text=" " EnableViewState="false" OnClick="AllOthersYesOrNo(this.id);" />
                                                    </td>
                                                    <td style="width: 3.5%" align="center" valign="top">
                                                        <asp:CheckBox ID="chkAllNo" runat="server" EnableViewState="false" OnClick="AllOthersYesOrNo(this.id);"
                                                            Text=" " />
                                                    </td>
                                                    <td style="width: 64%;" align="center" valign="top">
                                                        <asp:Label ID="Label7" runat="server" Font-Size="8.5pt" Font-Bold="true" EnableViewState="false"
                                                            Text="Description"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>--%>


                            <div id="divNonDrugAllergyHeaderControls" runat="server" style="height: 40%; width: 100%; position: relative; background-color: White; border-width: thin; font-weight: bold;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 50%;" valign="top">
                            <div id="divNonDrugAllergy" runat="server" style="border-style: inset; height: 390px; background-color: White; border-width: thin; overflow: scroll; position: relative; top: 0px; left: 0px;">
                            </div>
                        </td>
                    </tr>
                    <tr style="width: 100%;">
                        <td style="height: 30;" valign="top">
                            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%" BackColor="#BFDBFF">
                                <table style="width: 100%; background-color: White;">
                                    <tr>
                                        <td valign="top" style="width: 10%;">
                                            <asp:CheckBox  ID="chkShowAll" runat="server"  Text="Show All" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" OnCheckedChanged="chkShowAll_CheckedChanged"
                                                AutoPostBack="True" CssClass="spanstyle" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20px;" valign="top">
                                            <asp:Label ID="lblGeneralNotes" EnableViewState="false" runat="server" Text="Notes" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td colspan="2" rowspan="2" valign="top">
                                            <DLC:DLC ID="DLC" runat="server" TextboxHeight="50px" Value="NON DRUG ALLERGY NOTES"
                                                TextboxWidth="660px" Enable="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td style="width: 100px;" valign="top">
                                            <asp:Panel ID="pnlSaveControls" runat="server" Width="100%" Height="100%">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td align="right" valign="top"></td>
                                                        <td align="right" valign="top" style="width: 5%;">
                                                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                                                OnClientClicked="btnSave_Clicked" AccessKey="S" CssClass="greenbutton teleriknormalbuttonstyle"
                                                                Style="position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: -12px; height: 29px !important;"
                                                                Width="58px">
                                                                <ContentTemplate>
                                                                    <span>S</span>ave
                                                                </ContentTemplate>
                                                            </telerik:RadButton>
                                                        </td>
                                                        <td align="right" valign="top" style="width: 8%;">
                                                            <telerik:RadButton ID="btnClearAll" runat="server" AutoPostBack="false" Text="Clear All" OnClientClicked="btnClearAll_Clicked"
                                                                AccessKey="l" Style="position: static; text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-right: 23px; height: 29px !important; top: -13px;"
                                                                Width="79px" CssClass="redbutton teleriknormalbuttonstyle">
                                                                <ContentTemplate>
                                                                    C<span>l</span>ear All
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
                </table>
            </div>
            <div style="width: 100%; margin-top: -40px; margin-left: -8px; height: 50px;">
                <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Non_Drug_Allergy_History');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                </div>
                <div id="tag" class="boosterIconstyle" onclick="OpenMeasurePopup('Non_Drug_Allergy_History');">
                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                </div>
            </div>
            <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel3" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label4" Text=" " EnableViewState="false" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" enableviewstate="false" title="[Please wait while the page is loading...]"
                    alt="Loading..." />
                <br />
            </asp:Panel>
        </div>--%>
        </telerik:RadAjaxPanel>
        <asp:Button ID="InvisibleButton" runat="server" OnClick="InvisibleButton_Click" CssClass="displayNone" />
        <asp:HiddenField ID="Hidden1" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnLibraryIcon" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnLocalTime" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnTabSelected" runat="server" EnableViewState="false" Value="false" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSHistoryNonDrugAllergy.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
