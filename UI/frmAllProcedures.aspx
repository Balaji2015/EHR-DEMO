<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmAllProcedures.aspx.cs"
    Inherits="Acurus.Capella.UI.frmAllProcedures" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>All Procedures</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .VerticalAlignTop {
            vertical-align: top;
        }

        #frmAllProcedures {
            width: 628px;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .RadInput {
            vertical-align: middle;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton {
            cursor: pointer;
        }

        .rbSkinnedButton {
            vertical-align: top;
        }

        .rbSkinnedButton {
            display: inline-block;
            position: relative;
            background-color: transparent;
            background-repeat: no-repeat;
            border: 0 none;
            height: 22px;
            text-align: center;
            text-decoration: none;
            white-space: nowrap;
            background-position: right 0;
            padding-right: 4px;
            vertical-align: top;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .rbDecorated {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .rbDecorated {
            display: block;
            height: 22px;
            padding-right: 6px;
            padding-left: 10px;
            border: 0;
            text-align: center;
            background-position: left -22px;
            overflow: visible;
            background-color: transparent;
            outline: none;
            cursor: pointer;
            -webkit-border-radius: 0;
        }

        .rbDecorated {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
        }

        .style17 {
            width: 133px;
            height: 28px;
        }

        .style18 {
            width: 216px;
            height: 28px;
        }

        .style19 {
            height: 28px;
        }

        .style20 {
            height: 26px;
        }

        .style21 {
            width: 268px;
        }

        .style22 {
            height: 26px;
            width: 110px;
        }

        * {
            font-size: small;
            font-family: Microsoft Sans Serif;
        }

        .scrollposition {
            overflow: auto;
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
            width: 100% !important;
        }
    
          .ui-dialog-titlebar-close {
            display: none !important;
        }

        .ui-autocomplete {
            -webkit-margin-before: 3px !important;
            max-height: 70px;
            overflow-y: auto;
        }

        .ui-dialog-titlebar-close {
            display: none;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
            font-size: 14px;
        }

        .ui-dialog .ui-dialog-buttonpane button {
            width: 60px !important;
        }

        .ui-dialog .ui-dialog-titlebar {
            padding: 0px !important;
        }

        .ui-dialog .ui-dialog-title {
            font-size: 12px !important;
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px;
            border: 2px solid;
            border-radius: 13px;
            top: 504px !important;
            left: 430px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            font-size: 11px !important;
            font-family: sans-serif;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7;
        }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto;
            overflow-x: hidden;
        }

        .paddingtop {
            padding-top: 11px;
        }
    </style>
    <base target="_self" />
</head>
<body onload="StopLoadFromPatChart()" >
    <form id="frmAllProcedures" runat="server">
        <telerik:RadAjaxPanel ID="AjaxPanel" runat="server">           
                <telerik:RadWindowManager ID="WindowMngr" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Message"
                            IconUrl="Resources/16_16.ico">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>

                <div>
                    <asp:Panel ID="Panel1" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblEnterDescription" runat="server" Text="Enter Description" Width="100%" CssClass="Editabletxtbox"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <telerik:RadTextBox ID="txtEnterDescription" runat="server" Width="200px" CssClass="Editabletxtbox">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEnterCPTCode" runat="server" Text="Enter CPT Code" Width="100%" CssClass="Editabletxtbox"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtEnterCPTCode" runat="server" Width="200px" CssClass="Editabletxtbox">
                                    </telerik:RadTextBox>
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnSearch" runat="server" ButtonType="LinkButton" style="height: 29px !important;"
                            Text="Search" OnClientClicked="OnClientSearchClick" OnClick="btnSearch_Click" CssClass="bluebutton teleriknormalbuttonstyle">
                        </telerik:RadButton>
                                    &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnClearAll" runat="server" ButtonType="LinkButton" style="height: 29px !important;"
                            Text="Clear All" OnClick="btnClearAll_Click"  CssClass="redbutton teleriknormalbuttonstyle">
                        </telerik:RadButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="lblError1" runat="server" Width="100%" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 290px" colspan="3" valign="top">
                                    <asp:Panel ID="Panel3" runat="server" Height="340px" GroupingText="All" Width="619px"
                                        CssClass="Editabletxtbox" BorderStyle="Solid" BorderWidth="1px">
                                        <asp:CheckBoxList ID="chklstcptanddesclist" runat="server"
                                            Width="580px" Height="300px" RepeatLayout="Flow" RepeatDirection="Vertical" CssClass="scrollposition" onchange="ChkBoxSelected();">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%" colspan="3" align="right">
                                    <%--  <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>--%>
                                    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                                        <Scripts>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                                            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                                        </Scripts>
                                    </telerik:RadScriptManager>
                                    <telerik:RadButton ID="btnMoveToProcedure" runat="server" Text="Move To Procedure" OnClientClicked="OnClientMoveToProcedureClick"
                                        OnClick="btnMoveToProcedure_Click" style="left:-15px;height: 29px !important;"
                                        AccessKey="M" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                    </telerik:RadButton>
                                    &nbsp;&nbsp;&nbsp;
                        <telerik:RadButton ID="btnClose" runat="server" Style="top: -1px; left: -20px; height: 29px !important;"
                            Text="Close" OnClick="btnClose_Click" OnClientClicking="Close" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">


                        </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField ID="hdnProcedureType" runat="server" />
                        <asp:HiddenField ID="hdnLabId" runat="server" />
                        <asp:HiddenField ID="hdnLabName" runat="server" />
                        <asp:HiddenField ID="hdnPreloadedICD" runat="server" />
                        <asp:HiddenField ID="hdnSelectedCPT" runat="server" />
                        <asp:HiddenField ID="hdnScreenName" runat="server" />
                        <asp:HiddenField ID="hdnMessageType" runat="server" />
                        <asp:HiddenField ID="hdnCPT" runat="server" />
                        <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="Close();" />
                    </asp:Panel>
                </div>
                <%--<div style="height: 507px; width: 627px">
        <asp:Panel ID="Panel1" runat="server" Height="509px" Width="624px">
            <table class="style1">
                <tr>
                    <td class="style5" colspan="3">
                        <asp:Panel ID="Panel2" runat="server" Height="86px" Width="616px">
                            <table class="style14">
                                <tr>
                                    <td class="style17">
                                        <asp:Label ID="lblEnterDescription" runat="server" Text="Enter Description" Width="100%"></asp:Label>
                                    </td>
                                    <td class="style18">
                                        <telerik:RadTextBox ID="txtEnterDescription" runat="server" Width="200px">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td colspan="2" class="style19">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style15">
                                        <asp:Label ID="lblEnterCPTCode" runat="server" Text="Enter CPT Code" Width="100%"></asp:Label>
                                    </td>
                                    <td class="style16">
                                        <telerik:RadTextBox ID="txtEnterCPTCode" runat="server" Width="200px">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td class="style22">
                                        <telerik:RadButton ID="btnSearch" runat="server" Style="top: -1px; left: 0px; height: 20px;
                                            width: 100px" Text="Search" Width="100%" OnClick="btnSearch_Click">
                                        </telerik:RadButton>
                                    </td>
                                    <td class="style20">
                                        <telerik:RadButton ID="btnClearAll" runat="server" Style="top: -1px; left: 0px; height: 20px;
                                            width: 100px" Text="Clear All" Width="100%" OnClick="btnClearAll_Click">
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblError1" runat="server" Width="100%" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style8" colspan="3" style="vertical-align: top;">
                        <asp:Panel ID="Panel3" runat="server" Height="372px" GroupingText="All" Width="619px"
                            CssClass="VerticalAlignTop" BorderStyle="Solid" BorderWidth="1px">
                            <asp:CheckBoxList ID="chklstcptanddesclist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstcptanddesclist_SelectedIndexChanged"
                                Width="580px"  Height="340px" RepeatLayout="Flow" RepeatDirection="Vertical" CssClass="scrollposition">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style21">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                    </td>
                    <td class="style12" align="right">
                        <telerik:RadButton ID="btnMoveToProcedure" runat="server" Text="Move To Procedure"
                            OnClick="btnMoveToProcedure_Click" OnClientClicked="GetCPTValueMoveToProcedure" AccessKey="M">
                        </telerik:RadButton>
                    </td>
                    <td>
                        <telerik:RadButton ID="btnClose" runat="server" Style="top: -1px; left: 0px; height: 20px;
                            width: 100px" Text="Close" Width="100%" OnClick="btnClose_Click" OnClientClicked="Close">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnProcedureType" runat="server" />
            <asp:HiddenField ID="hdnLabId" runat="server" />
            <asp:HiddenField ID="hdnLabName" runat="server" />
            <asp:HiddenField ID="hdnPreloadedICD" runat="server" />
            <asp:HiddenField ID="hdnSelectedCPT" runat="server" />
        </asp:Panel>
    </div>--%>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                    <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>

                    <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>

                    <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <script src="JScripts/JSAllProcedures.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                    <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                </asp:PlaceHolder>

           </telerik:RadAjaxPanel>
    </form>
</body>
</html>
