<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmLabProcedureManage.aspx.cs"
    Inherits="Acurus.Capella.UI.frmLabProcedureManage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Frequently Used Procedures</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
       

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

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default.rbSkinnedButton {
            background-image: url( 'mvwres://Telerik.Web.UI,%20Version=2012.2.607.35,%20Culture=neutral,%20PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
            top: 1px;
            left: 0px;
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

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
        }

        .RadButton_Default .rbDecorated {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Button.ButtonSprites.png' );
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

        * {
            font-size: small;
        }

        .style1 {
            width: 334px;
        }

        .style2 {
            width: 100%;
            height: 25px;
        }

        .style3 {
            width: 219px;
        }

        legend {
            font-weight: bold;
        }

        * {
            font-size: 11.5px;
            font-family: Microsoft Sans Serif;
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

        .DisplayNone {
            display: none;
        }

        .underline {
            text-decoration: underline;
        }

        .style4 {
            top: 0px;
            position: relative;
            left: 0px;
            height: 18px;
        }

        ::-webkit-scrollbar {
    width: 8px;
}

::-webkit-scrollbar-track {
    background-color: #c3bfbf;
}

::-webkit-scrollbar-thumb {
    background-color: #707070;
}

    ::-webkit-scrollbar-thumb:hover {
        background-color: #3d3c3a;
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
}

.ui-dialog .ui-dialog-buttonpane button {
    width: 70px !important;
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
    left: 180px !important;
}

.ui-dialog .ui-dialog-content {
    min-height: 0px !important;
}

.ui-dialog .ui-dialog-buttonpane {
    margin-top: -10px !important;
    /*padding: 0px !important ;*/
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
    </style>
    
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
    <base target="_self" />
</head>
<body onload="OnLabProcedureLoad();" class="bodybackground">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" VisibleStatusbar="false"
            EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Move" Title="Manage Frequently Used Procedures">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="StopRefresh" runat="server">
            <div>
                <table style="width: 100%">
                    <%--width="800px;"--%>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Panel ID="Panel5" runat="server" BorderWidth="1px">
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="4" style="width: 100%;">
                                            <asp:Panel ID="pnlGroupDetail" runat="server" GroupingText="Category Detail" CssClass="LabelStyleBold">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 47%;">
                                                            <asp:Label ID="lblPanelName" runat="server" Font-Names="Microsoft Sans Serif" Font-Size="Small"
                                                                Text="Select a Category Name" CssClass="spanstyle"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="cboPanelName" runat="server" Width="100%" AutoPostBack="True" AllowCustomText="true" OnClientSelectedIndexChanged="StartLoadFromPatChart"
                                                                OnSelectedIndexChanged="cboPanelName_SelectedIndexChanged" Font-Bold="False">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 40%">
                                            <asp:Label ID="lblEnterDescription" runat="server" Font-Names="Microsoft Sans Serif"
                                                Font-Size="Small" Text="Enter Description" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td colspan="3" style="width: 53%">
                                            <telerik:RadTextBox ID="txtEnterDescription" runat="server" Width="100%" CssClass="Editabletxtbox">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style15" style="width: 47%">
                                            <asp:Label ID="lblEnterCPTCode" runat="server" Font-Names="Microsoft Sans Serif"
                                                Font-Size="Small" Text="Enter Code" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td class="style17 Editabletxtbox" colspan="3" style="width: 53%">
                                            <telerik:RadTextBox ID="txtEnterCPTCode" runat="server" Width="100%">
                                                <DisabledStyle Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <EmptyMessageStyle Resize="None" />
                                                <ClientEvents OnKeyPress="txtEnterCPTCode_OnKeyPress" />
                                                <FocusedStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 175px;">
                                            <asp:Label ID="lblResult" runat="server" Text="Label" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <telerik:RadButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Context Search"
                                                OnClientClicked="btnSearch_Clicked" Width="100%"
                                                ButtonType="LinkButton"
                                                CssClass="aspresizedbluebutton">
                                                <%--<ContentTemplate>
                                                    C<span>o</span>ntext Search
                                                </ContentTemplate>--%>
                                            </telerik:RadButton>
                                        </td>
                                        <td align="right">
                                            <telerik:RadButton ID="btnSearchAll" runat="server" OnClick="btnSearch_Click" Text="Search All"
                                                OnClientClicked="btnSearch_Clicked" Width="100%" AccessKey="A"
                                                ButtonType="LinkButton"
                                                CssClass="aspresizedbluebutton">
                                               <%-- <ContentTemplate>
                                                    Search <span>A</span>ll
                                                </ContentTemplate>--%>
                                            </telerik:RadButton>
                                        </td>
                                        <td align="right">
                                            <telerik:RadButton ID="btnClearAll" runat="server" OnClientClicked="btnClearAll_Click" Text="Clear All" AccessKey="C" OnClick="btnClearAll_Clicked"
                                                ButtonType="LinkButton" Width="100%"
                                                 CssClass="aspresizedredbutton">
                                               <%-- <ContentTemplate>
                                                   <span>C</span>lear All
                                                </ContentTemplate>--%>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td></td>
                        <td class="style1" rowspan="3">
                            <asp:Panel ID="gbFrequentlyUsedProcedures" runat="server" GroupingText="Frequently Used Procedures"
                                ScrollBars="Auto" BorderWidth="1px" BackColor="White" Width="495px" CssClass="LabelStyleBold">
                                <asp:CheckBoxList ID="lstLabProcudurePhysician" runat="server" Width="100%" Height="100%" Font-Bold="false" font-weight="normal" 
                                    BackColor="White">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2" style="vertical-align: top">
                            <asp:Panel ID="gbProcedureCodeLibrary" runat="server" GroupingText="Procedure Code Library"
                                ScrollBars="Auto" BorderWidth="1px" BackColor="White" CssClass="LabelStyleBold">
                                <asp:CheckBoxList ID="lstLabProcedureAll" runat="server" Width="100%" Height="100%"
                                    BackColor="White" font-weight="normal" Font-Bold="false">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
                            </telerik:RadScriptManager>
                        </td>
                        <td>
                            <asp:Panel ID="Panel4" runat="server" Height="84px">
                                <table style="width: 100%; height: 84px;">
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnMove" runat="server" Width="100%" OnClick="btnMove_Click" Text="&gt;" OnClientClicked="StartLoadFromPatChart" ButtonType="LinkButton" CssClass="aspresizedbluebutton">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadButton ID="btnBack" runat="server"  Width="100%" Text="&lt;" OnClientClicked="btnBack_Clicked" AutoPostBack="false" ButtonType="LinkButton" CssClass="aspresizedbluebutton">
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td class="style1">
                            <asp:Panel ID="Panel6" runat="server" Height="29px">
                                <table class="style2">
                                    <tr>
                                        <td class="style3">&nbsp;
                                        </td>
                                        <td></td>
                                        <td align="right">
                                            <telerik:RadButton ID="btnSave" runat="server" Text="Save" Width="100%" OnClientClicked="btnSave_Clicked"
                                                OnClick="btnSave_Click" AccessKey="S"
                                                ButtonType="LinkButton"
                                                CssClass="aspresizedgreenbutton">
                                             <%--<ContentTemplate>
                                                   <span>S</span>ave
                                                </ContentTemplate>--%>
                                            </telerik:RadButton>
                                        </td>
                                        <td align="right" style="width: 120px">
                                            <telerik:RadButton ID="btnQuitFrequentlyDiagnosis" runat="server" Width="100%"
                                                OnClientClicking="closeWindow" AccessKey="l"
                                                ButtonType="LinkButton"
                                                Text="Close" CssClass="aspresizedredbutton">
                                                <%--<ContentTemplate>
                                                    C<span >l</span>ose
                                                </ContentTemplate>--%>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="LabID" runat="server" />
            <asp:HiddenField ID="hdnSelectedCPT" runat="server" />
            <asp:HiddenField ID="hdnIsDirty" runat="server" Value="false" />
            <asp:HiddenField ID="hdnMoveClick" runat="server" Value="0" />
            <asp:HiddenField ID="hdnEandMCode" runat="server" />
            <asp:HiddenField ID="hdnDeletedCPT" runat="server" />
            <asp:HiddenField ID="hdnRefresh" runat="server" />
            <asp:HiddenField ID="hdnMessageType" runat="server" />
            <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none" OnClientClick="return closeWindow();" />
            <asp:Button ID="btnInvisibleBack" runat="server" Style="display: none" OnClick="btnInvisibleBack_Click" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSLabProcedureManager.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel1" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <br />
                    <img src="Resources/wait.ico" title="[Please wait while the page is saving...]" alt="saving..." />
                    <br />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
