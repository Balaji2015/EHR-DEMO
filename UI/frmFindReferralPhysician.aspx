<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="frmFindReferralPhysician.aspx.cs"
    Inherits="Acurus.Capella.UI.frmFindReferralPhysician" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Find/Add Provider</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        #frmFindReferralPhysician {
            width: 100%;
            height: 100%;
        }

        .ui-autocomplete {
            -webkit-margin-before: 3px !important;
            max-height: 130px;
            overflow-y: auto;
        }

        .ui-state-focus {
            color: #808080;
            background-color: #bbe2f1 !important;
            outline: none;
            border: 0px !important;
        }

        .disabled, .disabled.ui-state-focus {
            background-color: white !important;
        }
    </style>
    <link href="CSS/bootstrap.min3.1.1.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="JScripts/jquery-ui.min1.10.2.js" type="text/javascript"></script>
    <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
</head>
<body class="bodybackground" style="margin-top: 15px;" onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmFindReferralPhysician" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="FindReferralPhysician"
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="radscriptmngr" runat="server"></telerik:RadScriptManager>
        <div style="width: 100%">

            <div>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 13%;" class="Editabletxtbox">
                            <span><b>Provider Search**</b></span>
                        </td>
                        <td style="width: 80%;">
                            <asp:TextBox ID="txtProviderSearch" runat="server" Width="750px" data-phy-id="0" data-phy-details="" Rows="3" TextMode="MultiLine" placeholder="Type minimum 3 characters of Last or First Name or City or Company or Facility and follow it by a space.." CssClass="Editabletxtbox" style="resize:none"></asp:TextBox>
                            </td>
                        <td style="width: 1%;">
                            <img id="imgClearProviderText" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field."/>
                        </td>
                         <%--<td style="width: 4%;">
                           <img id="imgEditProvider" runat="server" src="Resources/edit.gif" alt="X" title="Click to edit the text field." onclick="return EditProviderDetails();" style="position: absolute;top: 38px!important;cursor: pointer;width: 15px;height: 15px;" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="float: right; margin-top: 2%; margin-right: 1%;">
                                <tr>
                                    <td style="width: 125px !important;">
                                        <input type="button" id="btnAddPhysician" value="Add Provider" onclick="return OpenAddPhysician();" style="width: 120px; left: 2000px !important;" disabled="disabled" class="aspresizedbluebutton" />
                                    </td>
                                    <td style="width:27px;"></td>
                                    <td></td>
                                    <td>
                                        <input type="button" id="EditProvider" value="Modifiy/View Provider" title="Click to edit the text field." onclick="return EditProviderDetails();" class="aspresizedbluebutton" style="width: 137px;"/>
                                    </td>
                                    </tr>
                                <tr style="height:5px;"></tr>
                                <tr>
                                    <td class="style9" style="width:44px !important;">
                                        <input type="button" id="btnOk" value="Ok" onclick="return closeReferelWindowRadGrid();" style="width: 120px; left: 1000px !important;" disabled="disabled" class="aspresizedgreenbutton" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <input type="button" id="btnCancel" value="Cancel" onclick="cancel();" class="aspresizedredbutton" style="width: 137px;"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height:20px"></tr>
                    <tr></tr>
                    <tr>
                        <td>
                            <label style="width:626%" class="LabelStyleBold">** Type minimum 3 characters of Last or First Name or City or Company or Facility and follow it by a space..</label>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hdnLastPageNo" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnTotalCount" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnAddPhysician" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnEditPhysicianId" runat="server" EnableViewState="false" />

            <asp:HiddenField ID="hdnCategory" runat="server" EnableViewState="false" />
            <%--<aspx:MaskedEditExtender ID="msktxtZipExtender" TargetControlID="msktxtZip" Mask="99999-9999"
            ClearMaskOnLostFocus="false" runat="server">
        </aspx:MaskedEditExtender>--%>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
           <%-- <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>--%>
            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAuthorization.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSFindReferralPhysician.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
