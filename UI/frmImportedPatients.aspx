<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImportedPatients.aspx.cs" Inherits="Acurus.Capella.UI.frmImportedPatients" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Abstracted</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <style type="text/css">
        .header {
            background-color: #bfdbff;
        }

        table tr td {
            font-size: 12px;
        }

        table th td {
            font-size: 12px;
        }
    </style>
      <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
</head>
<body onload="warningmethod();">
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
                <telerik:RadWindow ID="OpenPDFWindow" runat="server" Behaviors="Close" Title="PDF"
                    Height="90%" IconUrl="Resources/16_16.ico" Width="100%">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close" Title="ConsultationNotes"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
                <telerik:RadWindow ID="RadWindow4" runat="server" Behaviors="Close" Title="ProgressNotes"
                    Height="10px" IconUrl="Resources/16_16.ico" Width="10px">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="WindowEdit" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="AppointmentWindow" runat="server" Behaviors="Close" Title=""
                    IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableViewState="false"
            AsyncPostBackTimeout="400">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <div class="col-md-12 rowspace" style="overflow-y: auto; height: 530px; width: 100%; padding: 0px;" id="scrollID">
            <div class="table-responsive" style="height: 100%" id="dvImport">
            </div>
        </div>
        <div>
            <table style="width: 100%">
                <tr style="width: 100%">
                    <td align="left" style="width: 10%">
                        <asp:CheckBox ID="chkshowall" runat="server" Text="Show All" CssClass="chkitems " onchange="showall()" />
                    </td>
                    <td align="right" style="width: 90%">
                        <button id="btnScheduleAppointment" type="button" class="aspbluebutton" onclick="ScheduleAppointment();">Schedule Appointment</button>
                        <button id="btnsave" type="button" class="aspgreenbutton " onclick="SaveAppointmentStatus();">Save</button>
                        <button id="btnclose" type="button" class="aspredbutton" onclick="Closepopup();">Close</button>

                    </td>
                </tr>
            </table>


        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/jquery-1.11.3.min.js"></script>
            <script src="JScripts/jquery-ui.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"></script>
            <link href="CSS/jquery-ui.css" rel="stylesheet" />
            <link href="CSS/bootstrap.min.css" rel="stylesheet" />
            <script src="JScripts/bootstrap.min.js"></script>
            <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
            <script src="CSS/moment.js"></script>
            <link href="CSS/style.css" rel="stylesheet" type="text/css" />
            <script src="JScripts/JsImportedPatients.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSC5PO.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>

        </asp:PlaceHolder>
    </form>
</body>
</html>
