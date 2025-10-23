<%@ Page Async="true" ValidateRequest="false" EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="frmCancelAppointment.aspx.cs"
    Inherits="Acurus.Capella.UI.frmCancelAppointment" %>

<%--<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cancel Appointment</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <%--<title>Cancel Appointment</title>--%>
    <base target="_self"></base>
    <style type="text/css">
        .MultiLineTextBox {
            resize: none;
        }

        .style1 {
            width: 210px;
        }

        #frmCancelAppointment {
            width: 452px;
        }

        .style2 {
        }

        .style3 {
        }

        .style4 {
            width: 154px;
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

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 999;
        }

        .underline {
            text-decoration: underline;
        }
    </style>
    <link href="/CSS/CommonStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body onload=" {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();warningmethod();}">
    <form id="frmCancelAppointment" runat="server">
        <%--<telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>--%>
        <div>
            <div>
                <asp:Panel ID="pnlCancelAppointment" runat="server" Width="452px" Height="160px" CssClass="Editabletxtbox">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblReasonCode" runat="server" Text="Reason Code*" mand="Yes" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="style3" colspan="4">
                                <asp:DropDownList ID="ddlReasonCode" runat="server" Width="266px" AutoPostBack="True" onchange="Reasoncode();"
                                    OnSelectedIndexChanged="ddlReasonCode_SelectedIndexChanged" Height="23px" CssClass="Editabletxtbox">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblReasonText" runat="server" Text="Reason Text" CssClass="Editabletxtbox"></asp:Label>
                            </td>
                            <td class="style2" colspan="4">
                                <asp:TextBox ID="txtReasonText" runat="server" Height="86px" TextMode="MultiLine" style="resize: none;"
                                    Width="259px" CssClass="nonEditabletxtbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:HiddenField ID="hdnLocalTime" runat="server" />
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td class="style4">&nbsp;
                            </td>
                            <td class="style3">

                                <input type="button" runat="server" id="btnOkForAppt" name="btnOk"  value="Ok" width="60px" onserverclick="btnOkForAppt_Click" onclick="showTimeCancelForCancelAppointment();" class="aspresizedgreenbutton"/>
                            </td>
                            <td class="style3">
                                <input type="button" runat="server" id="btnCancelForAppt" name="btnCancelForAppt" width="80px"  onclick="CloseCancelAppmntWindow()" value="Cancel"  class="aspresizedredbutton"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:HiddenField ID="hdnEncounterID" runat="server" />
                                <asp:HiddenField ID="hdnPhysicianId" runat="server" />
                                <asp:HiddenField ID="hdnPhoneEncounter" runat="server" />
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td class="style4">&nbsp;
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                            <td class="style3">&nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div id="divLoading" class="modal" runat="server" style="text-align: center; display: none">
                    <asp:Panel ID="Panel2" runat="server">
                        <br />
                        <br />
                        <br />
                        <br />
                        <center>
                            <asp:Label ID="Label2" Text="" runat="server"></asp:Label></center>
                        <br />
                        <img src="Resources/wait.ico" title="[Please wait while the page is loading...]"
                            alt="Loading..." />
                        <br />
                    </asp:Panel>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSEditAppointment.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
