<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmTest.aspx.cs" EnableEventValidation="false" Inherits="Acurus.Capella.UI.frmTest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <%--jira new branch commit 1--%>
<head runat="server">
    <title>Test</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .style3 {
            width: 571px;
            height: 33px;
        }

        .style4 {
            height: 33px;
        }

        .modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }

        .underline {
            text-decoration: underline;
        }
    </style>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
       <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onkeydown="cancelBack()" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="form1" style="font-family: Microsoft Sans Serif; font-size: small; width: 100%;"
        runat="server">
        <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Test"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlTest" Font-Size="Small" runat="server">
            <div>
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: center; width: 43%;" class="Editabletxtbox">
                                <b>Condition</b>
                            </td>
                            <td style="text-align: center; width: 16%;" class="Editabletxtbox">
                                <b>Score</b>
                            </td>
                            <td style="text-align: center; width: 40%;" class="Editabletxtbox">
                                <b>Notes</b>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divTest" runat="server" style="height: 550px; width: 100%; z-index: auto; overflow: scroll; border-style: inset; background-color: White; border-width: thin; position: relative;"
                    onscroll="SetDivPosition()">
                </div>
                <div style="float:right;">
                    <telerik:RadButton ID="btnSave" runat="server" Text="Save" ButtonType="LinkButton" CssClass="greenbutton"  Style="position: static;text-align:center; -moz-border-radius: 3px; -webkit-border-radius: 3px; width: 92px"
                        AccessKey="s" OnClick="btnSave_Click" OnClientClicked="saveEnabled">
                        <ContentTemplate>
                            <span class="underline">S</span>ave                                              
                        </ContentTemplate>
                    </telerik:RadButton>

                    <telerik:RadButton ID="btnClearAll" runat="server" ButtonType="LinkButton" CssClass="redbutton" AutoPostBack="false" EnableViewState="false"
                        AccessKey="c" OnClientClicked="clearall" Style="position: static;text-align:center; -moz-border-radius: 3px; -webkit-border-radius: 3px; width: 90px"
                        Text="Clear All" Width="80px">
                        <ContentTemplate>
                            <span class="underline">C</span>lear All                                             
                        </ContentTemplate>
                    </telerik:RadButton>

                </div>
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
            </div>
            <input id="Hidden1" type="hidden" runat="server" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" />
            <input id="hdnSave" type="hidden" runat="server" />
            <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                <asp:Panel ID="Panel3" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center>
                        <asp:Label ID="Label4" Text="" runat="server"></asp:Label></center>
                    <br />
                    <img src="Resources/wait.ico" title="" alt="Loading..." />
                    <br />
                </asp:Panel>
            </div>
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JTest.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
