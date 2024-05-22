<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmHealthQuestionnaire.aspx.cs" EnableEventValidation="false" Inherits="Acurus.Capella.UI.frmHealthQuestionnaire"  ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HealthQuestionnaire</title>
  
<%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>

    <style type="text/css">
        .style3 {
            width: 870px;
            height: 33px;
        }

        .style7 {
            width: 300px;
        }

        .style8 {
            width: 230px;
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

        .radioWithProperWrap input {
            float: left;
        }

        .radioWithProperWrap label {
            margin-left: 0px;
            display: block;
            margin-top: 3px;
            display:table-cell;
            padding: 2px;
        }
        
        .AlignPanelCenter {
            margin-left: 70px;
        }

        .underline {
            text-decoration: underline;
        }

        .displaynone {
            display: none;
        }
           .ui-dialog-titlebar-close {
            display: none;
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

        .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px;
            border: 2px solid;
            border-radius: 13px;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -16px !important;
            padding: 0px !important;
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            font-weight: bold !important;
            font-size: 12px !important;
            font-family: sans-serif;
        }
    </style>
    <link href="~/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onkeydown="cancelBack(window.event.keyCode)" onload="Questionnaire_Load()">
    <form id="frmHealthQuestionnaire" style="background-color: White; font-family: Microsoft Sans Serif; height: 610px; margin-bottom: 0px; width: 1200px;"
        runat="server">
        <asp:Panel ID="pnlHealthQuestionnaires"  runat="server" Height="500px" Width="100%"
            Font-Size="Small">
               <div>
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <%-- <td>
                            &nbsp;
                        </td>--%>
                            <td style="width:35%;" class="LabelStyleBold"  align="center" id="tdQuestion" runat="server">
                                <b>Question</b>
                            </td>
                            <td style="width:35%;" class="LabelStyleBold" align="center" id="tdOption" runat="server">
                                <b>Option</b>
                            </td>
                            <td style="width:29.9%;display:none;" class="LabelStyleBold" id="tdNotes" runat="server" align="center" >
                                <b>Notes</b>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divHealthQuestionnaire" runat="server" style="height: 555px; width: 1190px; z-index: auto; overflow: auto; border-style: inset; background-color: White; border-width: thin; position: relative;"
                    onscroll="SetDivPosition()">
                </div>
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td class="style3" valign="top">
                                <asp:CheckBox ID="chkQuestion" runat="server" CssClass="Editabletxtbox" Style="font-family: Microsoft Sans Serif; font-size: 8.5pt;"
                                    Text="All other questions are discussed and found to be &quot;Normal&quot;"
                                    AutoPostBack="true"
                                    Visible="true" OnCheckedChanged="chkQuestion_CheckedChanged" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" />
                            </td>
                            <td align="center">

                                <input type="button" runat="server" class="btn aspresizedbluebutton" id="btnCopyPrevious" accesskey="p" value="Copy Previous Screening"
                                    onclick="if (!btnCopyPrevious_Clicked()) return;" onserverclick="btnCopyPrevious_Click" />
                            </td>
                            <td align="center">
                                <input type="button" runat="server" class="btn  aspresizedgreenbutton" id="btnSave" accesskey="s"
                                    disabled="disabled" value="Save"
                                    onclick="if (!SaveEnabled()) return;" onserverclick="btnSave_Click" />
                            </td>
                            <td align="center">
                                <input type="button" runat="server" class="btn aspresizedredbutton " id="btnClearAll" accesskey="c"
                                                                    value="Clear All" onclick="clearall();return;" />
                            </td>
                            <td align="center">
                                 <input type="button" runat="server" class="btn aspresizedbluebutton " id="btnPrint" 
                                                                    value="Print" onclick="btnPrint_Clicked(); return;" />
                            </td>
                        </tr>
                      <tr>
                            <td class="style3" valign="top">
                               
                            </td>
                            <td align="center">
                                
                            </td>
                            <td align="center">
                                
                            </td>
                            <td align="center">
                                
                            </td>
                            <td align="center">
                                 
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
             <input id="Hidden1" type="hidden" enableviewstate="false" runat="server" />
            <asp:HiddenField ID="hdnLocalTime" EnableViewState="false" runat="server" />
                <input id="hdnPrint" type="hidden" enableviewstate="false" runat="server" />		
             <asp:HiddenField ID="hdnTotalScore" Value="" EnableViewState="false" runat="server" />
            <asp:HiddenField ID="hdnTotalScoreDescription" Value="" EnableViewState="false" runat="server" />
            <asp:HiddenField ID="hdnPercentage" Value="" EnableViewState="false" runat="server" />
             <asp:HiddenField ID="hdnScorelevel" Value="" EnableViewState="false" runat="server" />
             <asp:HiddenField ID="hdnPercentageLevel" Value="" EnableViewState="false" runat="server" />
            <asp:Button ID="btnCopyPrevHidden" runat="server" OnClick="btnCopyPrevHidden_Click" style="display:none;" />
            <asp:HiddenField ID="HdnCopyButton" runat="server" Value="false" />
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
               <script src="JScripts/jquery-1.11.3.min.js" type="text/javascript"></script>
               <script src="JScripts/jquery-ui.min1.10.2.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
                  <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
                <script src="JScripts/JSHealthQuestionnaire.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

                <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
           
               
            </asp:PlaceHolder>
        </asp:Panel>
    </form>
</body>
</html>
