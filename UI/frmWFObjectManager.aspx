<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmWFObjectManager.aspx.cs"
    Inherits="Acurus.Capella.UI.frmWFObjectManager" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WFObjectManager</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        #frmWFObjectManager {
            width: 782px;
            height: 589px;
        }

        .style1 {
            width: 100%;
            height: 575px;
        }

        .style5 {
            height: 225px;
        }

        .style7 {
            height: 172px;
        }

        .style9 {
            height: 90px;
        }

        .style12 {
            height: 27px;
        }

        .style13 {
            width: 100%;
            height: 22px;
        }

        .style14 {
            height: 18px;
        }

        .style15 {
            height: 56px;
        }

        .style16 {
            width: 118px;
        }

        .style17 {
            width: 76px;
        }

        .RadGrid_Default {
            border: 1px solid #828282;
            background-color: white;
            color: #333;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
            font-size: 12px;
            line-height: 16px;
        }

        .RadGrid_Default {
            border: 1px solid #828282;
            background-color: white;
            color: #333;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
            font-size: 12px;
            line-height: 16px;
        }

        .RadGrid_Default {
            font: 12px/16px "segoe ui",arial,sans-serif;
        }

        .RadGrid_Default {
            border: 1px solid #828282;
            background: #fff;
            color: #333;
        }

        .RadGrid_Default {
            border: 1px solid #828282;
            background: #fff;
            color: #333;
        }

        .RadGrid_Default {
            font: 12px/16px "segoe ui",arial,sans-serif;
        }

            .RadGrid_Default .rgHeaderWrapper {
                border: 0;
                border-bottom: 1px solid #828282;
                background: #eaeaea 0 -2300px repeat-x url('mvwres://Telerik.Web.UI, Version=2013.3.1114.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif');
            }

            .RadGrid_Default .rgHeaderWrapper {
                border: 0;
                border-bottom: 1px solid #828282;
                background: #eaeaea 0 -2300px repeat-x url('mvwres://Telerik.Web.UI, Version=2013.3.1114.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif');
            }

            .RadGrid_Default .rgHeaderDiv {
                background: #eee 0 -7550px repeat-x url('mvwres://Telerik.Web.UI, Version=2013.3.1114.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif');
            }

            .RadGrid_Default .rgHeaderDiv {
                background: #eee 0 -7550px repeat-x url('mvwres://Telerik.Web.UI, Version=2013.3.1114.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif');
            }

            .RadGrid_Default .rgHeaderDiv {
                background: #eee 0 -7550px repeat-x url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif' );
            }

            .RadGrid_Default .rgHeaderDiv {
                background: #eee 0 -7550px repeat-x url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif' );
            }

            .RadGrid_Default .rgMasterTable {
                font-family: "Segoe UI",Arial,Helvetica,sans-serif;
                font-size: 12px;
                line-height: 16px;
            }

        .RadGrid .rgMasterTable {
            border-collapse: separate;
            border-spacing: 0;
        }

        .RadGrid_Default .rgMasterTable {
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
            font-size: 12px;
            line-height: 16px;
        }

        .RadGrid .rgMasterTable {
            border-collapse: separate;
            border-spacing: 0;
        }

        .RadGrid_Default .rgMasterTable {
            font: 12px/16px "segoe ui",arial,sans-serif;
        }

        .RadGrid .rgMasterTable {
            border-collapse: separate;
            border-spacing: 0;
        }

        .RadGrid .rgMasterTable {
            border-collapse: separate;
            border-spacing: 0;
        }

        .RadGrid_Default .rgMasterTable {
            font: 12px/16px "segoe ui",arial,sans-serif;
        }

        .RadGrid table.rgMasterTable tr .rgExpandCol {
            padding-left: 0;
            padding-right: 0;
            text-align: center;
        }

        .RadGrid table.rgMasterTable tr .rgExpandCol {
            padding-left: 0;
            padding-right: 0;
            text-align: center;
        }

        .RadGrid .rgClipCells .rgHeader {
            overflow: hidden;
        }

        .RadGrid .rgClipCells .rgHeader {
            overflow: hidden;
        }

        .RadGrid .rgClipCells .rgHeader {
            overflow: hidden;
        }

        .RadGrid .rgClipCells .rgHeader {
            overflow: hidden;
        }

        .RadGrid_Default .rgHeader {
            color: #333;
        }

        .RadGrid_Default .rgHeader {
            border: 0;
            border-bottom: 1px solid #828282;
            background: #eaeaea 0 -2300px repeat-x url('mvwres://Telerik.Web.UI, Version=2013.3.1114.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif');
        }

        .RadGrid .rgHeader {
            padding-top: 5px;
            padding-bottom: 4px;
            text-align: left;
            font-weight: normal;
        }

        .RadGrid .rgHeader {
            padding-left: 7px;
            padding-right: 7px;
        }

        .RadGrid .rgHeader {
            cursor: default;
        }

        .RadGrid_Default .rgHeader {
            color: #333;
        }

        .RadGrid_Default .rgHeader {
            border: 0;
            border-bottom: 1px solid #828282;
            background: #eaeaea 0 -2300px repeat-x url('mvwres://Telerik.Web.UI, Version=2013.3.1114.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif');
        }

        .RadGrid .rgHeader {
            padding-top: 5px;
            padding-bottom: 4px;
            text-align: left;
            font-weight: normal;
        }

        .RadGrid .rgHeader {
            padding-left: 7px;
            padding-right: 7px;
        }

        .RadGrid .rgHeader {
            cursor: default;
        }

        .RadGrid_Default .rgHeader {
            color: #333;
        }

        .RadGrid_Default .rgHeader {
            border: 0;
            border-bottom: 1px solid #828282;
            background: #eaeaea 0 -2300px repeat-x url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif' );
        }

        .RadGrid .rgHeader {
            padding-top: 5px;
            padding-bottom: 4px;
            text-align: left;
            font-weight: normal;
        }

        .RadGrid .rgHeader {
            padding-left: 7px;
            padding-right: 7px;
        }

        .RadGrid .rgHeader {
            cursor: default;
        }

        .RadGrid .rgHeader {
            cursor: default;
        }

        .RadGrid .rgHeader {
            padding-left: 7px;
            padding-right: 7px;
        }

        .RadGrid .rgHeader {
            padding-top: 5px;
            padding-bottom: 4px;
            text-align: left;
            font-weight: normal;
        }

        .RadGrid_Default .rgHeader {
            border: 0;
            border-bottom: 1px solid #828282;
            background: #eaeaea 0 -2300px repeat-x url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.sprite.gif' );
        }

        .RadGrid_Default .rgHeader {
            color: #333;
        }

        .style26 {
            width: 208px;
        }

        .style32 {
            width: 441px;
        }

        div.AddBorders .rgHeader, div.AddBorders th.rgResizeCol, div.AddBorders .rgFilterRow td, div.AddBorders .rgRow td, div.AddBorders .rgAltRow td, div.AddBorders .rgEditRow td, div.AddBorders .rgFooter td {
            border-style: solid;
            border-color: #aaa;
            border-width: 0 0 1px 1px; /*top right bottom left*/
        }

            div.AddBorders .rgHeader:first-child, div.AddBorders th.rgResizeCol:first-child, div.AddBorders .rgFilterRow td:first-child, div.AddBorders .rgRow td:first-child, div.AddBorders .rgAltRow td:first-child, div.AddBorders .rgEditRow td:first-child, div.AddBorders .rgFooter td:first-child {
                border-left-width: 0;
            }

        .style36 {
            width: 96px;
        }

        .style37 {
            width: 117px;
        }

        .style38 {
            width: 155px;
        }

        .style39 {
            width: 50px;
        }

        .displayNone {
            display: none;
        }

        .ui-dialog-titlebar-close {
            display: none !important;
        }

        .ui-widget {
            font-family: Verdana,Arial,sans-serif !important;
        }

        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset {
            float: none !important;
            margin-left: 45px !important;
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
            left: 568px !important;
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
            font-weight: bold !important;
            font-size: 12px !important;
            font-family: sans-serif;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7;
        }
    </style>
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body class="bodybackground" onload="loadwfobject(); {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmWFObjectManager" runat="server">
        <div style="width: 1000px;">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlAdmin" runat="server" Width="900px">

                        <table class="style1">
                            <tr>
                                <td class="style9">
                                    <asp:Panel ID="pnlPatientInformations" runat="server" Height="64px" Font-Size="Small" 
                                        Width="1050px" GroupingText="Patient Information" CssClass="LabelStyleBold">
                                        <table bgcolor="white">
                                            <tr>
                                                <td class="style36">
                                                    <asp:Label ID="lblAccountNo" runat="server" Text="Account No #" Width="120" Style="margin-left: 10px" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <telerik:RadTextBox ID="txtAccountNo" runat="server" ReadOnly="true" BackColor="#BFDBFF"  AutoPostBack="false" DisabledStyle-Font-Bold="True"
                                                         BorderWidth="1px" Style="margin-left: 10px" CssClass="nonEditabletxtbox">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td class="style38">
                                                    <asp:Label ID="lblPatientName" runat="server" Text="Patient Name" Style="margin-left: 10px" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtPatientName" runat="server" ReadOnly="true" BackColor="#BFDBFF"   AutoPostBack="false"
                                                        BorderWidth="1px" Style="margin-left: 10px" CssClass="nonEditabletxtbox">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td class="style39">
                                                    <asp:Label ID="lblDOB" runat="server" Text="DOB" CssClass="spanstyle"></asp:Label>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox ID="txtDOB" runat="server" ReadOnly="true" BackColor="#BFDBFF"   AutoPostBack="false"
                                                         BorderWidth="1px" Style="margin-left: 10px" CssClass="nonEditabletxtbox">
                                                    </telerik:RadTextBox>
                                                </td>
                                                <td>

                                                    <asp:Button ID="btnGetObjects" runat="server" OnClick="btnGetObjects_Click" Text="Get Objects" OnClientClick="return btnGetobjectsClick();"
                                                        Style="margin-left: 30px" CssClass="aspresizedbluebutton " />

                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClearAll" runat="server" Text="Clear All" OnClientClick="return Clear();"
                                                        OnClick="btnClearAll_Click" Style="margin-left: 30px" CssClass="aspresizedredbutton "  />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="style5">

                                    <asp:Panel ID="Panel1" runat="server" Font-Size="Small" Height="247px" Width="1050px" CssClass="LabelStyleBold"
                                        GroupingText="Patient Process Details">
                                        <table class="style13" bgcolor="white">
                                            <tr>
                                                <td>
                                                    <telerik:RadGrid ID="grdAdminModule" runat="server" CellSpacing="0" GridLines="None"
                                                        Height="217px" OnSelectedIndexChanged="grdAdminModule_SelectedIndexChanged" AllowSorting="True"
                                                        OnItemCommand="grdAdminModule_ItemCommand"
                                                        OnSortCommand="grdAdminModule_SortCommand" OnPreRender="grdAdminModule_PreRender"
                                                         CssClass="AddBorders">
                                                        <ClientSettings EnablePostBackOnRowClick="True">
                                                            <Selecting AllowRowSelect="True" />
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="170px" />
                                                            <Resizing AllowColumnResize="true" ClipCellContentOnResize="true" />
                                                            <ClientMessages ColumnResizeTooltipFormatString="" />
                                                            <ClientEvents OnRowClick="grdAdminModule_OnCommand" />
                                                        </ClientSettings>
                                                        <MasterTableView PageSize="8" TableLayout="Fixed">
                                                            <RowIndicatorColumn Visible="False">
                                                            </RowIndicatorColumn>
                                                            <ExpandCollapseColumn Created="True">
                                                            </ExpandCollapseColumn>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                </td>
                            </tr>
                            <tr>
                                <td class="style7">
                                    <asp:Panel ID="rbPanel" runat="server" Font-Size="Small" Width="900px" CssClass="LabelStyleBold">
                                        <table style="width: 1050px">
                                            <tr>
                                                <td class="style12">
                                                    <asp:RadioButton ID="rbUpdateProcess" runat="server" AutoPostBack="True" GroupName="rbbutton" onClick="rbUpdateProcessOnClick();"
                                                        OnCheckedChanged="rbUpdateProcess_CheckedChanged" Text="Update Process" />
                                                </td>
                                                <td class="style12"></td>
                                            </tr>
                                            <tr>
                                                <td class="style15" colspan="2">
                                                    <asp:Panel ID="pnlUpdateProcess" runat="server" Height="72px" Font-Size="Small" GroupingText=" Process" CssClass="LabelStyleBold"
                                                        Width="1050px">
                                                        <table bgcolor="white" style="width: 1022px">
                                                            <tr>
                                                                <td class="style16">
                                                                    <span class="MandLabelstyle">Reason for Change</span><span class="manredforstar">*</span>
                                                                   
                                                                </td>
                                                                <td>
                                                                    <telerik:RadTextBox ID="txtReasonForChange" runat="server" TextMode="MultiLine" CssClass="Editabletxtbox">
                                                                        <DisabledStyle Resize="None" />
                                                                        <InvalidStyle Resize="None" />
                                                                        <HoveredStyle Resize="None" />
                                                                        <ReadOnlyStyle Resize="None" />
                                                                        <EmptyMessageStyle Resize="None" />
                                                                        <FocusedStyle Resize="None" />
                                                                        <EnabledStyle Resize="None" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPreviousProcess" runat="server" Text="Previous Process" CssClass="spanstyle"></asp:Label>
                                                                </td>
                                                                <td class="style32">
                                                                    <telerik:RadComboBox ID="cboPreviousProcess" runat="server" AutoPostBack="false"
                                                                        Height="100px" Width="200px" CssClass="Editabletxtbox">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnUpdateProcess" runat="server" Text="Update Process" OnClientClick="btnUpdateProcessClick();"
                                                                        Style="margin-left: 0px" OnClick="btnUpdateProcess_Click" CssClass="aspresizedbluebutton" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style14">
                                                    <asp:RadioButton ID="rbUpdateOwner" runat="server" AutoPostBack="True" GroupName="rbbutton" onClick="rbUpdateOwnerOnClick();"
                                                        OnCheckedChanged="rbUpdateOwner_CheckedChanged" Text="Update Owner" />
                                                </td>
                                                <td class="style14"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlUpdateOwner" runat="server" Height="60px" Font-Size="Small" GroupingText="Owner" CssClass="LabelStyleBold">
                                                        <table bgcolor="white" style="width: 1022px">
                                                            <tr>
                                                                <td class="style17">
                                                                    <asp:Label ID="lblUpdateOwner" runat="server" Text="Owner" CssClass="spanstyle"></asp:Label>
                                                                </td>
                                                                <td class="style26">
                                                                    <telerik:RadComboBox ID="cboUpdateOwner" runat="server" AutoPostBack="false" Height="100px"
                                                                        Width="200px" CssClass="Editabletxtbox">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <td class="style26">
                                                                    <asp:CheckBox ID="checkkShowAllOwner" runat="server" OnCheckedChanged="checkkShowAllOwner_CheckedChanged" onclick="chkShowAllOwner();" Text="Show All" AutoPostBack="true" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnUpdateOwner" runat="server" Text="Update Owner" OnClientClick="return btnUpdateOwnerClick();" OnClick="btnUpdateOwner_Click" CssClass="aspresizedbluebutton" />
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td>&nbsp;
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
                            <tr>
                                <td align="right">
                                    <button type="button" id="btnClose" value="Close" onclick="btnClose_Clicked();" class="aspresizedredbutton">Close</button>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </ContentTemplate>
               
            </asp:UpdatePanel>
        </div>
        <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
            <asp:Panel ID="Panel4" runat="server">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="Label1" Text="" runat="server" CssClass="LabelStyleBold"></asp:Label></center>
                <br />
                <img src="Resources/wait.ico" title="" alt="saving..." />
                <br />
            </asp:Panel>
        </div>
        <asp:HiddenField ID="hdnMessageType" runat="server" />
        <asp:Button ID="btnMessageType" runat="server" Text="Button" CssClass="displayNone"
            OnClientClick="return btnClose_Clicked();" />

        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="WfObject"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSWfObjectManager.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
