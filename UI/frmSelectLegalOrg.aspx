<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmSelectLegalOrg.aspx.cs"
    Inherits="Acurus.Capella.UI.frmSelectLegalOrg" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title runat="server" id="titleLabel">Select LegalOrg and Facility</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <base target="_self" />
    <style type="text/css">
        .RadPicker {
            vertical-align: middle;
        }

        .RadPicker {
            vertical-align: middle;
        }

            .RadPicker .rcTable {
                table-layout: auto;
            }

            .RadPicker .rcTable {
                table-layout: auto;
            }

        .RadPicker_Default .rcCalPopup {
            background-position: 0 0;
        }

        .RadPicker_Default .rcCalPopup {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }

        .RadPicker .rcCalPopup {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }

        .RadPicker_Default .rcCalPopup {
            background-position: 0 0;
        }

        .RadPicker_Default .rcCalPopup {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }

        .RadPicker .rcCalPopup {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }

        .RadPicker_Default .rcTimePopup {
            background-position: 0 -100px;
        }

        .RadPicker_Default .rcTimePopup {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }

        .RadPicker .rcTimePopup {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }

        .RadPicker_Default .rcTimePopup {
            background-position: 0 -100px;
        }

        .RadPicker_Default .rcTimePopup {
            background-image: url( 'mvwres://Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Calendar.sprite.gif' );
        }

        .RadPicker .rcTimePopup {
            display: block;
            overflow: hidden;
            width: 22px;
            height: 22px;
            background-color: transparent;
            background-repeat: no-repeat;
            text-indent: -2222px;
            text-align: center;
        }

        .RadComboBox_Default {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }

        .RadComboBox {
            text-align: left;
        }

        .RadComboBox_Default {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }

        .RadComboBox {
            text-align: left;
        }

        .RadComboBox_Default {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }

        .RadComboBox {
            text-align: left;
        }

        .RadComboBox_Default {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox {
            vertical-align: middle;
            display: -moz-inline-stack;
            display: inline-block;
        }

        .RadComboBox {
            text-align: left;
        }

            .RadComboBox * {
                margin: 0;
                padding: 0;
            }

            .RadComboBox * {
                margin: 0;
                padding: 0;
            }

            .RadComboBox * {
                margin: 0;
                padding: 0;
            }

            .RadComboBox * {
                margin: 0;
                padding: 0;
            }

            .RadComboBox .rcbReadOnly .rcbInput {
                cursor: default;
            }

            .RadComboBox .rcbReadOnly .rcbInput {
                cursor: default;
            }

            .RadComboBox .rcbReadOnly .rcbInput {
                cursor: default;
            }

            .RadComboBox .rcbReadOnly .rcbInput {
                cursor: default;
            }

        .RadComboBox_Default .rcbInput {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox .rcbInput {
            text-align: left;
        }

        .RadComboBox_Default .rcbInput {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox .rcbInput {
            text-align: left;
        }

        .RadComboBox_Default .rcbInput {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox .rcbInput {
            text-align: left;
        }

        .RadComboBox_Default .rcbInput {
            font: 12px "Segoe UI",Arial,sans-serif;
            color: #333;
        }

        .RadComboBox .rcbInput {
            text-align: left;
        }

        .rcSingle .riSingle {
            white-space: normal;
        }

        .rcSingle .riSingle {
            white-space: normal;
        }

        .rcSingle .riSingle {
            white-space: normal;
        }

        .RadPicker .RadInput {
            vertical-align: baseline;
        }

        .rcSingle .riSingle {
            white-space: normal;
        }

        .RadPicker .RadInput {
            vertical-align: baseline;
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

        .Panel legend {
            font-weight: bold;
        }

        .style22 {
            width: 113px;
            height: 34px;
        }

        .style10 {
            width: 11px;
            height: 34px;
        }

        .style11 {
            width: 4px;
            height: 34px;
        }

        .style26 {
            width: 456px;
            height: 34px;
        }

        .style20 {
            height: 34px;
        }

        .style23 {
            width: 361px;
            height: 34px;
        }

        .style7 {
            width: 11px;
        }

        .style8 {
            width: 4px;
        }

        .style9 {
            width: 456px;
            height: 27px;
        }

        .style25 {
            width: 352px;
        }

        .style15 {
            width: 200px;
            height: 28px;
        }

        .style13 {
            width: 160px;
            height: 28px;
        }

        .style14 {
            width: 11px;
            height: 28px;
        }

        .TextWrapAll {
            overflow-wrap: break-word !important;
        }

        .flexible-table {
            display: flex;
            padding: 16px;
        }

            .flexible-table > tbody {
                width: 100%;
            }

                .flexible-table > tbody > tr {
                    width: 100%;
                    display: flex;
                }

                    .flexible-table > tbody > tr td:first-child {
                        width: 150px;
                        flex-shrink: 0;
                    }

                    .flexible-table > tbody > tr td {
                        width: 100%;
                    }

                    .flexible-table > tbody > tr + tr {
                        margin-top: 16px;
                    }

                    .flexible-table > tbody > tr:last-child {
                        justify-content: flex-end;
                    }

        .topSpace {
            margin-top: 30px;
        }

        .warningnotes {
            color: red;
            margin-left: 15px;
        }

        .warninglabelnotes {
            color: red;
        }
    </style>
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />

    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="~/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
    <form id="frmSelectLegalOrg" runat="server" name="frmSelectLegalOrg" style="margin-top: 2%">
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false" DestroyOnClose="true">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="radscriptmngr" runat="server"></telerik:RadScriptManager>
        <div style="margin-left: 15px" class="Editabletxtbox"><span runat="server" id="spnNotes">* Please ensure you have saved your work before proceeding to a different LegalOrg and Facility.</span></div>
        <table bgcolor="White " class="flexible-table">
            <tr runat="server" id="rowLegalOrg">
                <td style="width: 110px">
                    <asp:Label ID="lblLegalOrg" runat="server" Text="Organization" Width="100%" CssClass="Editabletxtbox"
                        EnableViewState="false" mand="Yes"></asp:Label>
                </td>
                <td class="" colspan="2">
                    <%--<select id="cboLegalOrg" runat="server" style="height:28px" width="100%"  class="form-control Editabletxtbox"  onserverchange="cboLegalOrg_Change"  >
                          <option value="0">Select Organization</option>
                    </select>--%>
                    <asp:DropDownList ID="cboLegalOrg" runat="server" Height="28px" Width="100%" CssClass="form-control Editabletxtbox" AutoPostBack="True" OnSelectedIndexChanged="cboLegalOrg_Change">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 110px">
                    <asp:Label ID="lblFacilityName" runat="server" Text="Facility Name" Width="110px" CssClass="Editabletxtbox"
                        EnableViewState="false"></asp:Label>
                </td>
                <td>
                    <select id="cboFacilityName" runat="server" style="height: 28px" width="100%" class="form-control Editabletxtbox">
                    </select>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" OnClientClick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); CheckUserFields();}"
                        Text="OK" Width="100px" CssClass="aspresizedbluebutton" />
                    <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" Width="100px" CssClass="aspresizedredbutton" />
                </td>
            </tr>
        </table>


        <button id="hdnbtnLogin" runat="server" style="display: none;" onserverclick="hdnbtnLogin_ServerClick">hdnLogin</button>
        <button id="hdnMultiUserLogin" runat="server" style="display: none;" onserverclick="hdnMultiUserLogin_Click">hdnMultiUserLogin</button>
        <button id="hdnbtnLogOutAndLogIn" runat="server" style="display: none;" onserverclick="hdbbtnLogOutAndLogIn_ServerClick">hdnMultiUserLogin</button>
        <asp:HiddenField ID="hdnLocalTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalDate" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnUniversaloffset" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLocalDateAndTime" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnFollowsDayLightSavings" runat="server" Value="false" />
        <asp:HiddenField ID="hdnroleLanding" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnRCopia_User_NameLanding" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIs_RCopia_Notification_RequiredLanding" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnPhysician_Library_IDLanding" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLanding_Screen_IDLanding" runat="server" Value="false" />
        <asp:HiddenField ID="hdnEMailAddress" runat="server" Value="false" />
        <asp:HiddenField ID="hdnPersonName" runat="server" EnableViewState="false" />

        <asp:HiddenField ID="hdnChangedFacilityName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnVersion" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnProjectName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnreportPath" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnLoginheader" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnVersionKey" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnServiceLink" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="HiddenField1" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnEvProjectName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnReportPathhttp" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnGroupId" runat="server" EnableViewState="false" />

        <asp:HiddenField ID="hdnFacltyName" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="HiddenField2" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnChangedLegalOrg" runat="server" EnableViewState="false" />
        <asp:HiddenField ID="hdnIsLegaLOrgValidation" runat="server" EnableViewState="false" />
        <%--<asp:HiddenField ID="hdnEmailAddress" runat="server" EnableViewState="false" />--%>

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSSelectLegalOrg.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
