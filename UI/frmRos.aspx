<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmRos.aspx.cs" Inherits="Acurus.Capella.UI.frmRos" EnableEventValidation="false"  ValidateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="UserControls/CustomDLCNew.ascx" TagName="CustomDLC" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ROS</title>
  <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>


    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
     <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #form1 {
            width: 1032px;
            height: 740px;
            margin-bottom: 4px;
        }

        div legend {
            font-weight: bolder;
        }

        .RadInput_Default {
            /*font: 12px "segoe ui",arial,sans-serif;*/
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
            margin-right: 0px;
        }

        .riSingle .riTextBox {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadInput textarea {
            vertical-align: bottom;
            overflow: auto;
            resize: none;
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url('mvwres://Telerik.Web.UI,%20Version=2012.2.607.35,%20Culture=neutral,%20PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png');
            top: 0px;
            left: 4px;
            width: 114px;
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
            /*font-family: "Segoe UI",Arial,Helvetica,sans-serif;*/
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
            width: 1115px;
            height: 291px;
        }

        .style24 {
            width: 637px;
        }

        .style25 {
            width: 104px;
        }

        .style26 {
            width: 80px;
        }

        .style27 {
            width: 1115px;
        }

        .style29 {
            width: 250px;
        }

        .style30 {
            width: 85px;
        }

        .dispaly {
            display: none;
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

        .dispaly {
            display: none;
        }

        .underline {
            text-decoration: underline;
        }
    </style>

</head>
<body onkeydown="cancelBack()" onload="OnROSLoad()">
    <form id="form1" runat="server" style="background-color: White; height: 677px; width: 100%; overflow: hidden; margin-top: 0px;">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="ROS" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="rapROS" runat="server" Height="550px" HorizontalAlign="NotSet"
            Width="16px">
            <telerik:RadScriptManager ID="rsmROS" runat="server" EnableViewState="false">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" OnClientClose="OnClientClose" Width="800px" Height="250px" NavigateUrl="frmProviderValidation.aspx" VisibleOnPageLoad="false" Behaviors="Close">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <div>
                <asp:Panel ID="Panel2" runat="server" Width="100%" BackColor="White" Style="margin-top: 0px;">
                    <table style="width: 100%; height: 425px;">
                        <tr>
                            <td class="style17">
                                <div id="pnlReviewOfSystems" runat="server" style="border-style: inset; height: 540px; width: 100%; position: relative; background-color: White; border-width: thin; overflow-x: auto;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" Height="74px" Width="1006px">
                                    <table style="width: 100%; height: 100%;">
                                        <tr>
                                            <td class="style25" valign="top">
                                                <asp:Label ID="lblNotes" runat="server" Font-Bold="True" Text="General Notes"
                                                    Width="101px"  EnableViewState="false" CssClass="spanstyle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                                    Font-Size="Small" Font-Bold="false">
                                                    <uc1:CustomDLC ID="dlcROS" runat="server" TextboxHeight="50" TextboxWidth="800" TextboxMaxLength="3000"
                                                        Value="ROS GENERAL NOTES" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <%--  <tr style="height:1px"></tr>--%>
                        <tr>
                            <td class="style27">
                                <asp:Panel ID="Panel3" runat="server">
                                    <table style="width:100%;margin-top:-10px">
                                        <tr>
                                            <td class="style24">
                                                <asp:CheckBox ID="chkAllOtherSystemsNormal" runat="server"
                                                    EnableViewState="false" CssClass="spanstyle"
                                                    Text="All other systems are Negative except for the problems presented." />
                                            </td>
                                            <td class="style29" align="right">
                                                <%--<asp:Button ID="btnPastEncounter" runat="server" 
                                                Text="Copy Previous ROS" Width="100%" AccessKey="C" 
                                                Font-Names="Microsoft Sans Serif" Font-Size="Small" 
                                                onclick="btnPastEncounter_Click">
                                            </asp:Button>--%>
                                                <telerik:RadButton ID="btnPastEncounter" runat="server" CssClass="bluebutton teleriknormalbuttonstyle"
                                                 
                                                    OnClick="btnPastEncounter_Click" ButtonType="LinkButton" Text="Copy Previous ROS" AccessKey="C" Style="-moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: -4px; height: 33px !important; margin-right: -8px;"
                                                    OnClientClicking="btnPastEncounter_Clicked">
                                                    <ContentTemplate>
                                                        <span class="">C</span>opy Previous ROS
                                                    </ContentTemplate>
                                                </telerik:RadButton>
                                            </td>
                                            <td class="style30" align="right">
                                                <%--<asp:Button ID="btnSave" runat="server" AccessKey="S" 
                                                Font-Names="Microsoft Sans Serif" Font-Size="Small" OnClick="btnSave_Click" 
                                                Text="Save" Value="30px" Width="100%" />--%>
                                                <telerik:RadButton ID="btnSave" runat="server" Text="Save"
                                                    Value="30px" CssClass="greenbutton teleriknormalbuttonstyle"
                                                    OnClick="btnSave_Click" Width="80px" AccessKey="S" Style="text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: -7px; height: 30px !important;">
                                                    <ContentTemplate>
                                                        <span class="">S</span>ave
                                                    </ContentTemplate>
                                                </telerik:RadButton>
                                            </td>
                                            <td class="style26">
                                                <%--<asp:Button ID="btnResetROS" runat="server" AccessKey="R" AutoPostBack="false" 
                                                Font-Names="Microsoft Sans Serif" Font-Size="Small" 
                                                OnClientClick="resetClick()" Text="Reset" Width="100%" />--%>
                                                <telerik:RadButton ID="btnResetROS" runat="server" Text="Reset" AutoPostBack="false" CssClass="redbutton teleriknormalbuttonstyle"
                                                   OnClientClicked="resetClick"
                                                    Width="80px" AccessKey="R" Style="text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; margin-top: -9px; height: 32px !important;">
                                                    <ContentTemplate>
                                                        <span class="">R</span>eset
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
            </div>
            <asp:HiddenField ID="HdnCopyButton" Value="false" runat="server" />
            <asp:HiddenField ID="hdnTrueCheck" Value="false" runat="server" />
            <asp:HiddenField ID="systemNamesForEventHF" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="isResetOK" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnChkToggleState" runat="server" Value="false" EnableViewState="false" />
            <asp:HiddenField ID="hdnCopyPreviousPhysicianId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="isbtnSaveOrDisable" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />

            <asp:Button ID="btnCopyPrevHidden" runat="server" OnClick="btnCopyPrevHidden_Click" Style="display: none;" />
            <asp:Button ID="btnFloatingSummary" runat="server" OnClick="btnFloatingSummary_Click" CssClass="dispaly" />

            <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">

                <asp:Panel ID="Panel4" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <center />
                    <br />
                    <img src="Resources/wait.ico" title="[Please wait while the page is saving...]"
                        alt="saving..." />
                    <br />
                </asp:Panel>
            </div>
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script type="text/javascript" src="JScripts/jquery-1.11.3.min.js"></script>
            <script type="text/javascript" src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
            <script type="text/javascript" src="JScripts/bootstrap.min.js"></script>
            <script type="text/javascript" src="JScripts/jsROS.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
