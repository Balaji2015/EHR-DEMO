<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmReviewOfQuestionnaire.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmReviewOfQuestionnaire" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Review Of Questionnaire</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        div legend {
            font-weight: bolder;
        }

        .style17 {
            width: 1115px;
            height: 291px;
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

        .style19 {
            width: 1200px;
            height: 95px;
        }
    </style>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
</head>
<body onkeydown="cancelBack()" onload="ReviewQuestionnaire_Load()">
    <form id="form1" style="font-family: Microsoft Sans Serif; font-size: 8.5pt; background-color: White;" runat="server" class="style19">
        <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" IconUrl="Resources/16_16.ico"
                    Title="Questionnaire">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="rapReviewOfQuestionnaire" runat="server" HorizontalAlign="NotSet">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <telerik:RadScriptManager EnableViewState="false" ID="rsmReviewOfQuestionnaire" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
            <div>
                <asp:Panel ID="Panel2" runat="server" Width="100%" BackColor="White">
                    <table style="width: 100%; height: 100%;">
                        <tr>
                            <td class="style17">
                                <asp:Panel ID="pnlReviewOfQuestionnaire" runat="server" BorderWidth="1px" ScrollBars="Auto"
                                    BackColor="White" Style="height: 555px; width: 1190px; z-index: auto; left: -8px; overflow: auto; border-style: inset; background-color: White; border-width: thin; position: relative;"
                                    onscroll="SetDivPosition()">
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 1182px;">
                                    <tr>
                                        <td style="width: 75%">
                                            <asp:CheckBox ID="chkAllOtherSystemsNormal" runat="server" CssClass="Editabletxtbox" Text="All other systems are Negative except for the problems presented." />
                                        </td>
                                        <td align="center">

                                <input type="button" runat="server" class="btn aspresizedbluebutton" id="btnCopyPrevious" accesskey="p" value="Copy Previous Screening"
                                    onclick="if (!btnCopyPrevious_Clicked()) return;" onserverclick="btnCopyPrevious_Click" />
                            </td>
                                        <td style="width: 6%;" align="right">
                                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" Width="80px" OnClick="btnSave_Click"
                                                OnClientClicked="SaveEnabled" AccessKey="s" Style="-moz-border-radius: 3px; -webkit-border-radius: 3px; width: 60px; text-align: center" CssClass="greenbutton">
                                                <ContentTemplate>
                                                    <span class="underline">S</span>ave
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                        <td style="width: 6%;" align="left">
                                            <telerik:RadButton ID="btnClearAll" AutoPostBack="false" CssClass="redbutton" runat="server" Text="Clear All"
                                                EnableViewState="false" OnClientClicked="ClearAllRoq" AccessKey="c" Style="-moz-border-radius: 3px; -webkit-border-radius: 3px; width: 80px; text-align: center">
                                                <ContentTemplate>
                                                    <span class="underline">C</span>lear All                                             
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                        <td align="center" style="width: 6%;">
                                            <telerik:RadButton ID="btnPrint" runat="server" CssClass="bluebutton" Text="Print" Width="80px" OnClientClicked="btnPrint_Clicked" Style="position: relative; -moz-border-radius: 3px; -webkit-border-radius: 3px;">
                                                <ContentTemplate>
                                                    <span class="underline">P</span>rint		
                                                </ContentTemplate>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <%--<td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>--%>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <%--<div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel3" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label4" Text="" EnableViewState="false" runat="server"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="" enableviewstate="false" alt="Loading..." />
                <br />
            </asp:Panel>
        </div>--%>
        </telerik:RadAjaxPanel>
        <asp:HiddenField ID="HdnCopyButton" runat="server" Value="false" />
        <input id="Hidden1" type="hidden" enableviewstate="false" runat="server" />
        <input id="hdnPrint" type="hidden" enableviewstate="false" runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSHealthQuestionnaire.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSReviewOfQuestionnaire.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
