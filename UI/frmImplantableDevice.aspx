<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmImplantableDevice.aspx.cs" Inherits="Acurus.Capella.UI.frmImplantableDevice" EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomPhrases.ascx" TagName="Phrases" TagPrefix="Phrases" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Implantable Device</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        ::-webkit-scrollbar {
            width: 6px;
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

        .displayNone {
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

        .underline {
            text-decoration: underline;
        }

        #pnlDLC {
            display: -webkit-inline-box !important;
        }

        .fa fa-question-circle .tooltip:hover .tooltiptext {
            visibility: visible;
        }

        .fa fa-question-circle .tooltip {
            position: relative;
            display: inline-block;
            border-bottom: 1px dotted black;
        }

        .tooltip {
            position: relative;
            display: inline-block;
        }

            .tooltip .tooltiptext {
                visibility: hidden;
                width: 200px;
                background-color: black;
                color: #fff;
                text-align: center;
                border-radius: 6px;
                padding: 5px 0;
                position: absolute;
                z-index: 1;
                top: -5px;
                left: 105%;
            }

        #tipforfindbtn:hover .tooltiptext {
            visibility: visible;
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
    </style>
    <link href="CSS/jquery-ui.css" rel="Stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="OnDLCLoad();">
    <form id="frmImplantableDevice" runat="server" style="background-color: White; font-family: Microsoft Sans Serif; font-size: smaller; width: 100%; height: 100%"
        scrollbars="Auto">
        <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
        </telerik:RadStyleSheetManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" HorizontalAlign="NotSet"
            Width="100%">
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            </telerik:RadAjaxManager>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                </Scripts>
            </telerik:RadScriptManager>
            <telerik:RadWindowManager ID="WindowMngr1" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Height="640px" VisibleStatusbar="false" Width="950px" VisibleOnPageLoad="false"
                        Behaviors="None" Modal="true" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindow2" runat="server" Title="ViewProcedure" Height="640px"
                        Width="950px" VisibleOnPageLoad="false" Behaviors="Resize,Move,Close" Modal="true"
                        IconUrl="Resources/16_16.ico" VisibleStatusbar="false">
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="RadWindow3" runat="server" Height="900px" Width="900px" VisibleStatusbar="false" VisibleOnPageLoad="false"
                        Behaviors="Resize,Move,Close" Modal="true" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="WindowMngr" runat="server">
                <Windows>
                    <telerik:RadWindow ID="MessageWindow" runat="server" Title="InhouseProcedure" IconUrl="Resources/16_16.ico">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <table style="width: 100%; height: 100%">
                <tr style="width: 100%; height: 40%">
                    <td>
                        <asp:Panel ID="pnlSelected" runat="server" GroupingText="Selected Procedure" Height="100%" class="LabelStyleBold" >
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td style="width: 10%">
                                        <asp:Label ID="lblProcedure" runat="server" class="Editabletxtbox MandLabelstyle"  mand="Yes" Text="Procedure*"></asp:Label>
                                    </td>
                                    <td style="width: 90%">
                                        <telerik:RadTextBox ID="txtProcedure" runat="server" Width="95%" Height="60px" 
                                            ReadOnly="True" TextMode="MultiLine" CssClass="nonEditabletxtbox">
                                        </telerik:RadTextBox>
                                        <asp:ImageButton ID="pictureBox2" runat="server" Height="17px" ToolTip="Clear" ImageUrl="~/Resources/close_small_pressed.png"
                                            OnClientClick="return procedureClear();" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                        <asp:Label ID="lblNotes" runat="server" Text="Notes" class="Editabletxtbox" ></asp:Label>
                                    </td>
                                    <td style="width: 90%">
                                        <asp:Panel ID="pnlDLC" runat="server" Height="100%" Width="100%" BackColor="White"
                                            Font-Size="Small" Font-Bold="false">

                                            <DLC:DLC ID="ctmDLC_procedure" TextboxWidth="925px" runat="server" TextboxHeight="140px"
                                                Value="OTHER PROCEDURE NOTES" />

                                            <Phrases:Phrases ID="pbProcedure" runat="server" MyFieldName="PROCEDURE" Height="12px"
                                                Width="12px" />

                                            <asp:PlaceHolder ID="phlProce" runat="server" />

                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="width: 100%; height: 40%">
                    <td>
                        <asp:Panel ID="pnlImplantable" runat="server" Height="100%" GroupingText="Implantable Device" class="LabelStyleBold">
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td style="width: 13%">
                                        <asp:Label ID="DevIdentifier" runat="server" Text="Device Identifier(UDI/DI)*"
                                           class="Editabletxtbox MandLabelstyle"></asp:Label>
                                        <i id="tipforfindbtn" class="fa fa-question-circle tooltip"><span class="tooltiptext">Please enter the Device Indentifier(UDI/DI) and click find button.</span></i></td>
                                    <td style="width: 60%">
                                        <asp:TextBox ID="txtDeviceIdentifier" runat="server" Width="88%" class="Editabletxtbox" onclick="EnableFind();" onchange="EnableFind();"></asp:TextBox>
                                        <asp:Button ID="btnFind" runat="server" class="aspbluebutton"  Width="10%" Text="Find" OnClick="btnFind_Click" />
                                    </td>
                                    <td style="width: 5%; font-weight: 100;">
                                        <telerik:RadButton ID="btnClearAllImplantable" ButtonType="LinkButton" CssClass="redbutton" runat="server" ToolTip="Clear All" Text="Clear All" AutoPostBack="false"
                                            OnClientClicked="btnClearAllImplantable_Clicked"
                                            AccessKey="C" Style="text-align: center; position: static; left: 144px; ">
                                            <ContentTemplate>
                                                <span id="SpanClear" runat="server">C</span><span id="SpanClearAdditional" runat="server">lear All</span>
                                            </ContentTemplate>
                                        </telerik:RadButton>
                                    </td>
                                    <td style="width: 7%">
                                        <asp:DropDownList ID="ddActive" runat="server" Width="100%" class="Editabletxtbox" onchange="GetKeyPress();"></asp:DropDownList></td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr style="width: 100%">
                                    <td style="width: 15%">
                                        <asp:Label ID="lblSerialNumber" Text="Serial Number" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtSerialNumber" runat="server" Width="95%" class="nonEditabletxtbox"  ReadOnly="true"></asp:TextBox></td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lblLotorBatch" Text="Lot or Batch" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtLotorBatch" runat="server" Width="100%" class="nonEditabletxtbox"  ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr style="width: 100%">
                                    <td style="width: 15%">
                                        <asp:Label ID="lblManufacturedDate" Text="Manufactured Date" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtManufacturedDate" runat="server" Width="95%" class="nonEditabletxtbox"  ReadOnly="true"></asp:TextBox></td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lblExpirationDate" Text="Expiration Date" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtExpirationDate" runat="server" Width="100%" class="nonEditabletxtbox"  ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr style="width: 100%">
                                    <td style="width: 15%">
                                        <asp:Label ID="lblDistinctID" Text="Distinct ID(HCT/P)" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 33%">
                                        <asp:TextBox ID="txtDistinctID" runat="server" Width="95%" class="nonEditabletxtbox" ReadOnly="true"></asp:TextBox></td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lblIssuingAgency" Text="Issuing Agency" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtIssuingAgency" runat="server" class="nonEditabletxtbox" Width="100%"  ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr style="width: 100%">
                                    <td style="width: 15%">
                                        <asp:Label ID="lblBrandName" Text="Brand Name" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtBrandName" runat="server" class="nonEditabletxtbox" Width="95%"  ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lblVersionModel" Text="Version/Model" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtVersionModel" runat="server" class="nonEditabletxtbox" Width="100%"  ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr style="width: 100%">
                                    <td style="width: 15%">
                                        <asp:Label ID="lblCompanyName" Text="Company Name" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtCompanyName" runat="server" class="nonEditabletxtbox" Width="95%"  ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lblMRISafetyStatus" Text="MRI Safety Status" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtMRISafetyStatus" runat="server" class="nonEditabletxtbox" Width="100%"  ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                </tr>
                                <tr style="width: 100%">

                                    <td style="width: 15%">
                                        <asp:Label ID="lblGMDNPTName" Text="GMDN PT Name" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtGMDNPTName" runat="server" class="nonEditabletxtbox" Width="95%"  ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lblrubberContent" Text="Rubber Content" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtRubberContent" runat="server" class="nonEditabletxtbox" Width="100%"  ReadOnly="true" Wrap="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="width: 100%">
                                    <td style="width: 15%">
                                        <asp:Label ID="lblDescription" Text="Description" runat="server" class="Editabletxtbox"></asp:Label></td>
                                    <td style="width: 55%" colspan="3">
                                        <asp:TextBox ID="txtDescription" runat="server" class="nonEditabletxtbox" Width="100%"  ReadOnly="true" Wrap="true"></asp:TextBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="width: 100%; height: 20%">
                    <td style="width: 100%">
                        <table style="width: 100%">
                            <tr style="width: 100%">
                                <td style="width: 70%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 10%; text-align: right;">
                                    <telerik:RadButton ID="btnAdd" ButtonType="LinkButton" CssClass="greenbutton" runat="server" ToolTip="Save"
                                        OnClientClicked="btnAddImplantable" OnClick="btnAdd_Click"
                                        Style="text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px;">
                                        <ContentTemplate>
                                            <span id="SpanAdd" runat="server" >S</span><span id="SpanAdditionalword" runat="server">ave</span>
                                        </ContentTemplate>
                                    </telerik:RadButton>
                                </td>
                                <td style="width: 5%">
                                    <telerik:RadButton ID="btnClose" ButtonType="LinkButton" CssClass="redbutton" runat="server" ToolTip="Close" OnClientClicked="CloseImplantable"
                                        Style="text-align: center; -moz-border-radius: 3px; -webkit-border-radius: 3px;">
                                        <ContentTemplate>
                                            <span id="SpanCancle1" runat="server">C</span><span id="SpanCancle2" runat="server">l</span><span id="Span1" runat="server">ose</span>
                                        </ContentTemplate>
                                    </telerik:RadButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnBtnFind" runat="server" />
        </telerik:RadAjaxPanel>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
            <script src="Jscripts/jquery-ui.js" type="text/javascript"></script>
            <link rel="stylesheet" href="CSS/jquery-ui.min.css">
            <script src="JScripts/JSInhouseProcedure.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomPhrases.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
