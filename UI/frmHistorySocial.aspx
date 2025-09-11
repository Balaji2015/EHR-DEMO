<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmHistorySocial.aspx.cs"
    Inherits="Acurus.Capella.UI.frmHistorySocial" EnableEventValidation="false"  ValidateRequest="false" %>

<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Social History</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
  <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>


    <style type="text/css">
        .displayNone {
            display: none;
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
         #tag:hover {
            text-decoration: underline;
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
            height: 155px !important;
            border: 2px solid !important;
            border-radius: 13px !important;
            top: 504px !important;
            left: 568px !important;
        }

        .ui-dialog .ui-dialog-content {
            min-height: 0px !important;
        }

        .ui-dialog .ui-dialog-buttonpane {
            margin-top: -10px !important;
            font-size: 14px !important;
            /*padding: 0px !important ;*/
        }

        .ui-widget-content {
            border: 0px !important;
        }

        .ui-widget-header, .ui-state-default, ui-button {
            font-weight: bold !important;
            font-size: 12px !important;
            font-family: sans-serif !important;
        }


        .ui-widget {
            border: 1px solid #adadad !important;
            background-color: #F7F7F7 !important;
        }
          .ui-autocomplete {
                -webkit-margin-before: 3px !important;
                max-height: 150px;
                overflow-y: auto;
            }

            .ui-state-focus {
                color: #808080;
                background-color: #bbe2f1 !important;
                outline: none;
                border: 0px !important;
            }
    </style>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
     <link href="CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();LoadSocialHistory();OpenNotificationPopUp('PFSH-SOCIAL_HISTORY');}">
    <form id="form1" style="font-family: Microsoft Sans Serif; font-size: 8.5pt; background-color: White; font-family: Microsoft Sans Serif; width: 100%;" runat="server">
        <telerik:RadWindowManager EnableViewState="false" ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="Social History"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlSocialHistory" runat="server" BackColor="White"
            Font-Bold="false" Font-Size="Small">
            <div id="SummaryAlert" runat="server" class="alert alert-info" style="height: 550px; padding: 10px; display: none; border-color: lightblue; color: black; margin: 3px; font-weight: bolder; background-color: aliceblue; font-family: Helvetica Neue,Helvetica,Arial,sans-serif; font-size: 14px;">
                Xml is not found for this encounter. Please contact support.
            </div>
            <div style="height: 110px;" class="Editabletxtbox" id="divSocialHistory">
                <div id="divSocialHistoryHeaderControls" runat="server" style="height: 40%; width: 100%; position: relative; background-color: White; border-width: thin; font-weight: bold;">
                </div>
                <div id="divSocialHistoryControls" runat="server" style="border-style: inset; height: 391px; width: 98%; position: relative; background-color: White; border-width: thin; overflow: scroll;">
                </div>
                <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" Text="Show All" CssClass="Editabletxtbox" />
                <table style="width: 100%; height: 115px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblNotes" runat="server" Text="Notes" CssClass="Editabletxtbox"  EnableViewState="false" ></asp:Label>
                            
                        </td>
                        <td rowspan="3">
                            <DLC:DLC ID="DLC" runat="server" TextboxHeight="55px" TextboxWidth="790px" Value="Social History Notes"  />
                            &nbsp; &nbsp; &nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td align="right">
                            <telerik:RadButton ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" Width="80px" OnClientClicked="btnSave_Clicked"
                                AccessKey="S" Style="top: -11px; text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative; height: 29px !important; padding: 4px 12px !important; font-size: 13px !important;"  ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle ">
                                <ContentTemplate>
                                    <span >S</span>ave
                                </ContentTemplate>
                            </telerik:RadButton>
                            <telerik:RadButton ID="btnClearAll" runat="server" AutoPostBack="true"
                                Text="Clear All" OnClientClicked="btnClearAll_Clicked" AccessKey="l" Width="80px"
                                Style="top: -10px; text-align: center; position: static; -moz-border-radius: 3px; -webkit-border-radius: 3px; position: relative;  height: 30px !important; margin-right: 32px; padding: 4px 12px !important; font-size: 13px !important;"  ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                <ContentTemplate>
                                    C<span >l</span>ear All
                                </ContentTemplate>
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
                <table>
                </table>

                <telerik:RadScriptManager ID="RadScriptManager1" EnableViewState="false" runat="server">
                    <Scripts>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                        <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
                    </Scripts>
                </telerik:RadScriptManager>
            </div>
        </telerik:RadAjaxPanel>
        <div style="width: 100%; margin-top: 435px; margin-left:-8px;height: 50px;">
                <div style="float: left; height: 47px; padding-left: 5px; color: white; margin-top: -5px; text-align: center;">
                    <img src="Resources/measure_logo.png" alt="" onclick="OpenMeasurePopup('Social_History');" style="height: 48px; width: 45px; padding-left: 2px; margin-left: -8px;" />
                </div>
                <div id="tag" class="boosterIconstyle"  onclick="OpenMeasurePopup('Social_History');">
                    Measure Booster<img src="Resources/measure_info.png" alt="" style="width: 16px; height: 15px; margin-left: 4px;" />
                </div>
            </div>
        <asp:Button ID="InvisibleButton" runat="server" OnClick="InvisibleButton_Click" CssClass="displayNone" />
        <asp:HiddenField ID="hdnDivPosition" EnableViewState="false" runat="server" />
        <asp:HiddenField ID="hdnreason"  runat="server" />
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
                <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
                <link href="CSS/jquery-ui.css?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" rel="stylesheet" />
              <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
            <script src="JScripts/JSocialHistory.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <link href="CSS/style.css" rel="stylesheet" type="text/css" />
            <script type="text/javascript">
                //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                //function EndRequestHandler(sender, args) {
                //    if (args.get_error() != undefined) {
                //        args.set_errorHandled(true);
                //    }
                //}
            </script>
        </asp:PlaceHolder>
    </form>

</body>

</html>
