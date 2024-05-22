<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSelectLabLocation.aspx.cs"
    Inherits="Acurus.Capella.UI.frmSelectLabLocation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="PageNavigator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <title>Select Lab Location</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #frmSelectLabLocation {
            width: 695px;
            height: 533px;
        }

        .style1 {
            width: 100%;
            height: 530px;
        }

        .style2 {
            height: 160px;
        }

        .style3 {
            width: 100%;
            height: 157px;
        }

        .style4 {
        }

        .style5 {
            width: 195px;
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

        .RadInput {
            vertical-align: middle;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
        }

        .RadInput {
            vertical-align: middle;
        }

        .riSingle {
            position: relative;
            display: inline-block;
            white-space: nowrap;
            text-align: left;
        }

        .riSingle {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -ms-box-sizing: border-box;
            -webkit-box-sizing: border-box;
            -khtml-box-sizing: border-box;
        }

        .RadInput_Default {
            font: 12px "segoe ui",arial,sans-serif;
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

        .style6 {
            width: 71px;
        }

        .style7 {
            width: 107px;
            height: 21px;
        }

        .style8 {
            height: 21px;
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
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
        }

        .RadButton {
            font-size: 12px;
            font-family: "Segoe UI",Arial,Helvetica,sans-serif;
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

        .rbSkinnedButton {
            vertical-align: top;
        }

        .RadButton {
            cursor: pointer;
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

        .style15 {
            height: 357px;
        }

        .style16 {
            width: 445px;
        }

        .style17 {
            width: 33px;
        }

        .style18 {
            width: 107px;
            height: 33px;
        }

        .style19 {
            width: 195px;
            height: 33px;
        }

        .style20 {
            width: 71px;
            height: 33px;
        }

        .style21 {
            height: 33px;
        }

        .auto-style1 {
            width: 107px;
            height: 8px;
        }

        .auto-style2 {
            height: 8px;
        }

        .auto-style3 {
            width: 107px;
            height: 25px;
        }

        .auto-style4 {
            width: 195px;
            height: 25px;
        }

        .auto-style5 {
            width: 71px;
            height: 25px;
        }

        .auto-style6 {
            height: 25px;
        }

        .auto-style7 {
            width: 107px;
            height: 19px;
        }

        .auto-style8 {
            width: 195px;
            height: 19px;
        }

        .auto-style9 {
            width: 71px;
            height: 19px;
        }

        .auto-style10 {
            height: 19px;
        }

        .auto-style11 {
            width: 107px;
            height: 9px;
        }

        .auto-style12 {
            width: 195px;
            height: 9px;
        }

        .auto-style13 {
            width: 71px;
            height: 9px;
        }

        .auto-style14 {
            height: 9px;
        }

        .auto-style17 {
            height: 180px;
        }

        .auto-style18 {
            height: 39px;
        }

        .auto-style20 {
            height: 126px;
        }
    </style>
    <base target="_self" />
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmSelectLabLocation" runat="server">
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div style="height: 534px; width: 691px">
                <asp:Panel ID="pnlSelectLabLocation" runat="server" BorderWidth="1px" Height="530px"
                    DefaultButton="btnSearch">
                    <table class="style1">
                        <tr>
                            <td class="auto-style20" colspan="5">
                                <asp:Panel ID="Panel1" runat="server" BorderWidth="1px" Height="155px" DefaultButton="btnSearch">
                                    <table class="style3">
                                        <tr>
                                            <td class="auto-style1">
                                                <asp:Label ID="lblLabName" runat="server" Text="Lab or Imaging Center Name" Width="120px" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td class="auto-style2" colspan="4">
                                                <telerik:RadTextBox ID="txtLabName" runat="server" onkeyup="txtkeyUp()" Height="18px" Width="550px" ReadOnly="True" CssClass="nonEditabletxtbox">
                                                    <ReadOnlyStyle BorderWidth="1px" ForeColor="Black"/>
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                <asp:Label ID="lblAddress" runat="server" Text="Address" Width="120px" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td class="auto-style4">
                                                <telerik:RadTextBox ID="txtAddress" runat="server" onkeyup="txtkeyUp()" Height="18px" Width="100%" CssClass="Editabletxtbox">
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ClientEvents OnKeyPress="txtAddress_OnKeyPress" />
                                                    <FocusedStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="auto-style5">&nbsp;
                                        <asp:Label ID="lblCity" runat="server" Text="City" Width="60px" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td class="auto-style6" colspan="2">
                                                <telerik:RadTextBox ID="txtCity" runat="server" onkeyup="txtkeyUp()" Height="18px" Width="100%" MaxLength="35" CssClass="Editabletxtbox">
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ClientEvents OnKeyPress="txtCity_OnKeyPress" />
                                                    <FocusedStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style7">
                                                <asp:Label ID="lblState" runat="server" Text="State" Width="120px" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td class="auto-style8">
                                                <telerik:RadTextBox ID="txtState" runat="server" onkeyup="txtkeyUp()" Height="18px" Width="100%" MaxLength="2" CssClass="Editabletxtbox">
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ClientEvents OnKeyPress="txtState_OnKeyPress" />
                                                    <FocusedStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="auto-style9">&nbsp;
                                        <asp:Label ID="lblZip" runat="server" Text="Zip" Width="60px" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td class="auto-style10" colspan="2">
                                                <telerik:RadMaskedTextBox ID="txtZip" runat="server" Mask="#####-####" onkeyup="Copy('Zipcode');" 
                                                    Width="99%" EnableViewState="false" CssClass="Editabletxtbox">
                                                    <ClientEvents OnKeyPress="txtZip_OnKeyPress"/>
                                                </telerik:RadMaskedTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style11">
                                                <asp:Label ID="lblNPI" runat="server" Text="NPI" Width="120px" CssClass="Editabletxtbox"></asp:Label>
                                            </td>
                                            <td class="auto-style12">
                                                <telerik:RadTextBox ID="txtNPI" runat="server" onkeyup="txtkeyUp()" Height="18px" Width="100%" CssClass="Editabletxtbox">
                                                    <DisabledStyle Resize="None" />
                                                    <InvalidStyle Resize="None" />
                                                    <HoveredStyle Resize="None" />
                                                    <ReadOnlyStyle Resize="None" />
                                                    <EmptyMessageStyle Resize="None" />
                                                    <ClientEvents OnKeyPress="txtNPI_OnKeyPress" />
                                                    <FocusedStyle Resize="None" />
                                                    <EnabledStyle Resize="None" />
                                                </telerik:RadTextBox>
                                            </td>
                                            <td class="auto-style13"></td>
                                            <td class="auto-style14" colspan="2"></td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style10" colspan="2">
                                                <asp:Label ID="lblResult" runat="server" Width="100%" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td class="auto-style9">&nbsp;
                                            </td>
                                            <td class="auto-style10">
                                                <%--<telerik:RadButton ID="btnSearch" runat="server" OnClientClicked="btnSearchClient" Style="top: -4px; left: 104px; height: 10px; width: 55px;  padding: 4px 12px !important;  font-size: 12px !important;"
                                                    Text="Search" Width="100%" OnClick="btnSearch_Click" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle">
                                                </telerik:RadButton>--%>
                                                <telerik:RadButton ID="btnSearch" runat="server" CssClass="aspresizedbluebutton" OnClientClicked="btnSearchClient" Text="Search" Width="100%" OnClick="btnSearch_Click" ButtonType="LinkButton">
                                                </telerik:RadButton>
                                            </td>
                                            <td class="auto-style10">
                                                <%--<telerik:RadButton ID="btnClearAll" runat="server" Style="top: -4px; left:45px; height: 10px; width: 50px;  padding: 4px 12px !important;  font-size: 12px !important;"
                                                    Text="Clear All" Width="100%" OnClientClicked="btnSearchClient" OnClick="btnClearAll_Click" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle"> 
                                                </telerik:RadButton>--%>
                                                <telerik:RadButton ID="btnClearAll" runat="server" CssClass="aspresizedredbutton" Text="Clear All" Width="100%" OnClientClicked="btnSearchClient" OnClick="btnClearAll_Click" ButtonType="LinkButton"> 
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style17" colspan="5">
                                <asp:Panel ID="pnlLabLocation" runat="server" GroupingText="Lab Location" Height="316px" CssClass="Editabletxtbox"> 
                                    <telerik:RadGrid ID="grdLabLocations" runat="server" CellSpacing="0" GridLines="None" ClientSettings-EnablePostBackOnRowClick="true"
                                        Height="289px" Width="650px" AllowSorting="True" AutoGenerateColumns="False" OnSelectedIndexChanged="grdLabLocations_SelectedIndexChanged">
                                        <FilterMenu EnableImageSprites="False"></FilterMenu>
                                        <ClientSettings>
                                            <Selecting AllowRowSelect="True" />
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                        </ClientSettings>
                                        <ClientSettings EnableAlternatingItems="false"></ClientSettings>
                                        <MasterTableView>
                                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True"></RowIndicatorColumn>
                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True"></ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="Address" FilterControlAltText="Filter Address column"
                                                    HeaderText="Address" UniqueName="Address">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="City" FilterControlAltText="Filter City column"
                                                    HeaderText="City" UniqueName="City">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="State" FilterControlAltText="Filter State column"
                                                    HeaderText="State" UniqueName="State">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Zip" FilterControlAltText="Filter Zip column"
                                                    HeaderText="Zip" UniqueName="Zip">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="NPI" FilterControlAltText="Filter NPI column"
                                                    HeaderText="NPI" UniqueName="NPI">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Location ID" FilterControlAltText="Filter LocationID column"
                                                    HeaderText="Location ID" UniqueName="LocationID" Visible="False">
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                            <EditFormSettings>
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                                            </EditFormSettings>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" width="370px" colspan="3" class="auto-style18">
                                <PageNavigator:PageNavigator ID="mpnLabLocation" runat="server" OnFirst="FirstPageNavigator" />
                            </td>
                            <%--<td width="100px" class="auto-style18">&nbsp;
                        </td>
                        <td class="auto-style18">&nbsp;
                        </td>--%>
                            <td valign="middle" align="right" class="auto-style18">
                                <%--<telerik:RadButton ID="btnOk" runat="server" Style="top: -4px; left: 10px; height: 10px; width: 50px; padding: 4px 12px !important;  font-size: 12px !important;"
                                    Text="OK" Width="50%" OnClick="btnOk_Click" OnClientClicked="btnOk_Clicked" ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle">  
                                </telerik:RadButton>--%>
                                <telerik:RadButton ID="btnOk" runat="server" Text="OK" Width="100%" OnClick="btnOk_Click" OnClientClicked="btnOk_Clicked" ButtonType="LinkButton" CssClass="aspresizedgreenbutton">  
                                </telerik:RadButton>

                            </td>
                            <td valign="middle" align="right" class="auto-style18">
                               <%-- <telerik:RadButton ID="btnCancel" runat="server" Style="top: -4px; left: -15px; height: 10px; width:50px; padding: 4px 12px !important;  font-size: 12px !important;"
                                    Text="Cancel" Width="50%" OnClientClicked="btnSearchClient" OnClick="btnCancel_Click" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                </telerik:RadButton>--%>
                                 <telerik:RadButton ID="btnCancel" runat="server" Text="Cancel" Width="100%" OnClientClicked="btnSearchClient" OnClick="btnCancel_Click" ButtonType="LinkButton" CssClass="aspresizedredbutton">
                                </telerik:RadButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="hdnSelectedLabText" runat="server" />
            <asp:HiddenField ID="hdnSelectedLocID" runat="server" />
            <asp:HiddenField ID="hdnSelectedLabAddress" runat="server" />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSImageAndLabOrder.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
             <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="JScripts/jquery.datetimepicker.js" type="text/javascript"></script>  
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
