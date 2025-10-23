<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPerformEV.aspx.cs" Inherits="Acurus.Capella.UI.frmPerformEV" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Perform EV</title>
    <script type="text/javascript">document.write("<script src='JScripts/JsLogRocket.js?version=" + sessionStorage.getItem("ScriptVersion") + "'><\/script>")</script>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="CSS/datetimepicker.css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            width: inherit; /* Or auto */
            padding: 0 10px; /* To give a bit of padding on the left and right */
            border-bottom: none;
        }

        legend {
            font-size: 13px !important;
            font-weight: 700 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadWindowManager ID="WindowMngr" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="WindowEdit" runat="server" Behaviors="Close" Title="" IconUrl="Resources/16_16.ico">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadWindowManager ID="ModalWindowMngt" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="ModalWindow" runat="server" VisibleOnPageLoad="false" Height="625px"
                    IconUrl="Resources/16_16.ico" Width="1225px" EnableViewState="false">
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
        <div style="padding-left: 14px; padding-right: 14px;">
            <%--<fieldset class="scheduler-border" id="FieldsetEV" runat="server" style="height:600px">--%>
            <%--<legend class="scheduler-border">EV Request</legend>--%>

            <table style="width: 100%">
                <tr style="width: 100%">
                    <%-- <td style="width: 20%">
                            <span class="MandLabelstyle">Patient Details</span><span class="manredforstar">*</span>
                        </td>
                        <td style="width: 60%">
                            <input type="text" class="nonEditabletxtbox" id="txtpatientName" style="width:99%" runat="server"/>
                        </td>--%>
                    <%--<td >--%>
                    <div id="divPatientstrip" runat="server" class=" pnlBarGroup Editabletxtbox " style="height: 21px; margin-bottom: 0px; margin-top: 8px; vertical-align: middle; padding-top: 4px; position: relative; padding-left: 0px; border: 0px !important;"></div>
                    <%--</td>--%>
                    <%-- <td style="width: 20%">
                           <button id="btnSelectPlan" type="button" runat="server" class="aspbluebutton aspresizedbluebutton" onclick="OpenInsurance();" style="width: 70%">View/Update Insurance</button>
              
                        </td>--%>
                </tr>
                <tr style="height: 10px">
                </tr>
                <tr style="width: 100%">
                    <td style="width: 20%">
                        <span class="MandLabelstyle">Service Type</span><span class="manredforstar">*</span>
                    </td>
                    <td style="width: 60%">
                        <input type="text" class="nonEditabletxtbox" id="txtServieType" style="width: 99%" runat="server" disabled />
                    </td>
                    <td style="width: 20%">
                        <asp:Button Text="Service Type" runat="server" CssClass="aspresizedbluebutton" ID="btnservicetype" Width="70%" OnClientClick="return OpenServiceType()" />
                    </td>
                </tr>
                <tr style="height: 10px">
                </tr>
                <tr style="width: 100%">
                    <td style="width: 20%">
                        <input type="radio" name="radDate" id="radionextApp" checked="checked" onclick="disablecontrols();" />
                        <span id="spannxtappt" class="MandLabelstyle" style="width=10px">Next Appointment On</span><span id="spannxtapptstar" class="manredforstar" style="width=10px">*</span>
                    </td>
                    <td colspan="2">
                        <select id="seldateNextApp" class="Editabletxtbox" runat="server"></select>


                        <input type="radio" name="radDate" id="radioDate" onclick="disablecontrolsDate();" />
                        <span class="spanstyle" id="spandatefrom" style="width=10px">Date From</span><span id="spnstardatefrom" class="manredforstar" style="display: none; width=10px">*</span>
                        <input type="text" class="Editabletxtbox" id="txtdatefrom" autocomplete="off" disabled />


                        <span class="spanstyle">Date To</span>
                        <input type="text" class="Editabletxtbox" id="txtdateto" autocomplete="off" disabled />
                    </td>
                </tr>
            </table>

            <fieldset class="scheduler-border" id="FieldsetPolicy" runat="server" style="height: 400px">
                <legend class="scheduler-border">Policy Information</legend>
                <div style="width: 100%; height: 350px; overflow: auto;">
                    <table id="tblpolicyinfo" class="table table-bordered Gridbodystyle" style="width: 100%">
                        <thead class="Gridheaderstyle">
                            <tr>
                                <th class="Gridheaderstyle" style="width: 10%; text-align: center">Carrier</th>
                                <th class="Gridheaderstyle" style="width: 20%; text-align: center">Plan Name</th>
                                <th class="Gridheaderstyle" style="width: 10%; text-align: center">Ins.Type</th>
                                <th class="Gridheaderstyle" style="width: 18%; text-align: center">Checked For</th>
                                <th class="Gridheaderstyle" style="width: 1%; text-align: center; display: none;">Checked To</th>
                                <th class="Gridheaderstyle" style="width: 5%; text-align: center">View Details</th>
                                <th class="Gridheaderstyle" style="width: 3%; text-align: center">Mode</th>
                                <th class="Gridheaderstyle" style="width: 5%; text-align: center">Status</th>
                                <th class="Gridheaderstyle" style="width: 16%; text-align: center">Message</th>
                                <th class="Gridheaderstyle" style="width: 20%; text-align: center">Policy Holder Id</th>
                                <th class="Gridheaderstyle" style="width: 10%; text-align: center">Trace Number</th>

                            </tr>
                        </thead>
                        <tbody id="tbodupolicyinfo" class="Gridbodystyle">
                        </tbody>
                    </table>
                </div>

            </fieldset>
            <table style="width: 100%">
                <tr>
                    <td style="width: 7%">
                        <input type="checkbox" id="chkshowall" value="Show All" onclick="showall();" />&nbsp;Show All
                    </td>
                    <td style="text-align: right; width: 100%">
                        <button id="btnSelectPlan" type="button" runat="server" class="aspbluebutton aspresizedbluebutton" onclick="OpenInsurance();" style="width: 15%">View/Update Insurance</button>
                        <%--<button id="btnev" type="button" runat="server" class="aspbluebutton aspresizedbluebutton" onclick="SendEvRequest();">Send EV Request</button>--%>
                        <%--<asp:Button Text="Send EV Request" runat="server" CssClass="aspresizedgreenbutton"  OnClientClick="SendEvRequest();"/>--%>
                        <input type="button" id="btnev" runat="server" class="aspresizedbluebutton" onclick=" { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } SendEvRequest(); " value="Send EV Request" />
                        <asp:Button ID="btnmEV" Text="Manual EV" runat="server" CssClass="aspresizedbluebutton" OnClientClick="openManualEV();" />
                        <asp:Button Text="Refresh" runat="server" CssClass="aspresizedbluebutton" OnClientClick="bindEVTable();" ID="btnrefresh" />
                        <asp:Button Text="Close" runat="server" CssClass="aspresizedredbutton" OnClientClick="CloseEv();" />
                    </td>
                </tr>
            </table>
            <%--  </fieldset>--%>
        </div>
        <asp:HiddenField runat="server" ID="hdnHumanDetails" />
        <asp:HiddenField runat="server" ID="hdnIsSendEVEnable" Value="false"/>
        <asp:HiddenField runat="server" ID="ServiceTypeCode" />
        <asp:HiddenField runat="server" ID="ServiceTypeCodeDesc" />
        <asp:HiddenField runat="server" ID="hdnInsPlan_Id" />
         <asp:Button ID="hdnbtngenerateevxml" runat="server" OnClick="hdnbtngeneratexml_Click"  style="display:none" />

        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

            <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
            <script src="JScripts/jquery-ui.min1.11.4.js" type="text/javascript"></script>
            <%--<link href="CSS/jquery-ui.css" rel="stylesheet" />--%>
            <script src="JScripts/JSLibraries.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>

            <script src="JScripts/jquery.datetimepicker.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JsPerformEV.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
            <script src="JScripts/JSModalWindow.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        </asp:PlaceHolder>
    </form>
</body>
</html>
