<%@ Page Async="true" Language="C#" AutoEventWireup="True" CodeBehind="frmViewResult.aspx.cs" EnableEventValidation="false"
    Inherits="Acurus.Capella.UI.frmViewResult" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/UserControls/CustomDLCNew.ascx" TagName="DLC" TagPrefix="DLC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Result</title>
    <%--<script src="https://logrocket.acurussolutions.io/LogRocket.js"; crossorigin="anonymous"></script> <script>window.LogRocket && window.LogRocket.init('akido/akido-test', { mergeIframes: true }, { enableVerboseLogging: true });</script>--%>
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <base target="_self" />
    <script type="text/javascript" id="telerikClientEvents1">

        
        function txtMedicalAssistantNotes_OnKeyPress(sender, args) {
            //Cap - 1268
            //document.getElementById(GetClientId("hdnSave")).value = "true";
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
            document.getElementById(GetClientId("btnSave")).disabled = false;
        }

        function AutosaveDisable(IsEnable) {
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = IsEnable;
            }
        }
        function EnableSave() {
            document.getElementById(GetClientId("btnSave")).disabled = false;
        }


        function txtMedicalAssistantNotes_OnValueChanged(sender, args) {
            document.getElementById('btnSave').disabled = false;
        }
        function EnableSave() {
            document.getElementById('btnSave').disabled = false;
        }
        function btnMoveToMa_ClientClicked(sender, args) {

            //if (document.getElementById('DLC_txtDLC') != null && document.getElementById('DLC_txtDLC').disabled == false && document.getElementById('cboMoveToMA') != null && document.getElementById('cboMoveToMA').value == "") {
            if (document.getElementById('DLC_txtDLC') != null && document.getElementById('DLC_txtDLC').disabled == false && document.getElementById('txtAssignedTo')?.attributes["val"] != null && document.getElementById('txtAssignedTo')?.attributes["val"] != undefined && document.getElementById('txtAssignedTo').attributes["val"] == "") {
                DisplayErrorMessage('115046');
                return false;
            }
            if (document.getElementById('DLC_txtDLC').disabled == false && document.getElementById('DLC_txtDLC').value == "") {
                //var Continue = DisplayErrorMessage('115060');
                //if (Continue != undefined && Continue == true) {
                    StartLoadingImage();
                    __doPostBack('btnMoveToMa', "true");
                //}
                //else {
                //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //    return false;
                //}
            }
            if (document.getElementById('txtMedicalAssistantNotes').disabled == false && document.getElementById('txtMedicalAssistantNotes').value == "") {


                //var Continue = DisplayErrorMessage('115060');
                //if (Continue != undefined && Continue == true) {
                    StartLoadingImage();
                    __doPostBack('btnMoveToMa', "true");
                //}
                //else {
                //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //    return false;
                //}
            }
            else {
                StartLoadingImage();//BugID:41027 -- move to next result
                __doPostBack('btnMoveToMa', "true");
            }
        }
        function btnMoveToNextProcess_ClientClicked(sender, args) {

            if (document.getElementById('chkPhyName') != null && document.getElementById('chkPhyName').disabled == false && document.getElementById('chkPhyName').checked == false) {
                DisplayErrorMessage('115034');
                return false;
            }

            if (document.getElementById('DLC_txtDLC').disabled == false && document.getElementById('DLC_txtDLC').value == "") {
                //var Continue = DisplayErrorMessage('115060');
                //if (Continue != undefined && Continue == true) {
                    StartLoadingImage();
                    __doPostBack('btnMoveToNextProcess', "true");
                //}
                //else {
                //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //    return false;
                //}
            }
            if (document.getElementById('txtMedicalAssistantNotes').disabled == false && document.getElementById('txtMedicalAssistantNotes').value == "") {
                //var Continue = DisplayErrorMessage('115060');
                //if (Continue != undefined && Continue == true) {
                    StartLoadingImage();
                    __doPostBack('btnMoveToNextProcess', "true");
                //}
                //else {
                //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //    return false;
                //}

            }
            else {
                StartLoadingImage();
                __doPostBack('btnMoveToNextProcess', "true");
            }
        }
        function tabView_TabSelected(sender, args) {
            sender.set_enabled(false);
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
        function SaveViewResults() {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            AutosaveDisable('false');
            DisplayErrorMessage('115009');
        }

        function RegenerateXML(Humanid, xmlType, page) {

            // DisplayErrorMessage('1011190');
            if ($(top.window.document).find("#CheckAlertxml") != undefined)
                $(top.window.document).find("#CheckAlertxml")[0].style.display = "block";
            if ($(top.window.document).find("#innerMsgTextxml") != undefined)
                $(top.window.document).find("#innerMsgTextxml")[0].innerText = "We apologize for the inconvenience. XML regeneration is in progress. Please contact capella support, if the page does not refresh within 2 minutes.";

            $.ajax({
                type: "POST",
                url: "frmPatientChart.aspx/RegenerateXMLbyTalend",
                data: JSON.stringify({ id: Humanid, XMLType: xmlType }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                asyn: false,
                success: function (data) {
                    var ProtocolLst = data.d;
                    if (ProtocolLst == "Success") {
                        ToolStripAlertHidexml();
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        if (page == "patientchart")
                            document.getElementById('ctl00_C5POBody_hdnbtngeneratexml').click();
                        else if (page == "summary")
                            document.getElementById('hdnbtngeneratexmlsummary').click();

                        else if (page == "encounter")
                            document.getElementById('hdnbtngeneratexmlEncounter').click();
                        else if (page == "Appointment")
                            document.getElementById('hdnbtngeneratexmlAppointment').click();

                        else if (page == "result")
                            document.getElementById('hdnbtngenerateresultxml').click();

                        else if (page == "payment")
                            document.getElementById('hdnbtngeneratepaymentxml').click();
                        else if (page == "task")
                            document.getElementById('hdnbtngeneratetaskxml').click();
                        else if (page == "indexing")
                            document.getElementById('hdnbtngenerateindexingxml').click();
                        else if (page == "ev")
                            document.getElementById('hdnbtngenerateevxml').click();

                        else if (page == "phoneencounter")
                            document.getElementById('hdnbtngeneratephoneencounterxml').click();


                        return false;
                    }
                    else {
                        alert(ProtocolLst);
                    }

                },
                error: function OnError(xhr) {
                    ToolStripAlertHidexml();
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        //CAP-798
                        if (isValidJSON(xhr.responseText)) {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);

                            alert("USER MESSAGE:\n" +
                                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                "Message: " + log.Message);
                        }
                        else {
                            alert("USER MESSAGE:\n" +
                                ". Cannot process request. Please Login again and retry.");
                        }

                    }
                }

            });

        }

        //function btnClose_Clicked(sender, args) {
        //    if (Result != undefined) {
        //        if (false == Result.closed) {

        //            Result.close();
        //        }
        //    }
        //    if (document.getElementById('hdnTab').value == "true") {
        //        if (document.getElementById("hdnMessageType").value == "") {
        //            document.getElementById("hdnMessageType").value == "Yes";
        //            document.getElementById("btnSave").click();
        //            DisplayErrorMessage('115009');
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false";
        //            document.getElementById(GetClientId("hdnMessageType")).value = "";
        //            $(top.window.document).find('#btnCloseViewResult')[0].click();
        //            self.close();
        //            //DisplayErrorMessage('1105001');
        //        }
        //        //else if (document.getElementById("hdnMessageType").value == "Yes") {
        //        //    document.getElementById("btnSave").click();
        //        //    DisplayErrorMessage('115009');
        //        //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false";
        //        //    document.getElementById(GetClientId("hdnMessageType")).value = "";
        //        //    $(top.window.document).find('#btnCloseViewResult')[0].click();
        //        //    self.close();
        //        //}
        //        //else if (document.getElementById("hdnMessageType").value == "No") {
        //        //    document.getElementById("hdnMessageType").value = "";
        //        //    $(top.window.document).find('#btnCloseViewResult')[0].click();
        //        //    self.close();
        //        //}
        //        //else if (document.getElementById("hdnMessageType").value == "Cancel") {
        //        //    document.getElementById("hdnMessageType").value = "";
        //        //}
        //    }
        //    if (document.getElementById('btnSave') != null) {
        //        if (document.getElementById('btnSave').disabled == false) {
        //            if (document.getElementById("hdnMessageType").value == "") {
        //                document.getElementById("hdnMessageType").value == "Yes";
        //                document.getElementById("btnSave").click();
        //                DisplayErrorMessage('115009');
        //                document.getElementById('btnSave').disabled = true;
        //                document.getElementById(GetClientId("hdnMessageType")).value = "";
        //                $(top.window.document).find('#btnCloseViewResult')[0].click();
        //                self.close();
        //                //DisplayErrorMessage('1105001');
        //            }
        //            //else if (document.getElementById("hdnMessageType").value == "Yes") {
        //            //    document.getElementById("btnSave").click();
        //            //    DisplayErrorMessage('115009');
        //            //    document.getElementById('btnSave').disabled = true;
        //            //    document.getElementById(GetClientId("hdnMessageType")).value = "";
        //            //    $(top.window.document).find('#btnCloseViewResult')[0].click();
        //            //    self.close();
        //            //}
        //            //else if (document.getElementById("hdnMessageType").value == "No") {
        //            //    document.getElementById("hdnMessageType").value = "";
        //            //    $(top.window.document).find('#btnCloseViewResult')[0].click();
        //            //    self.close();
        //            //}
        //            //else if (document.getElementById("hdnMessageType").value == "Cancel") {
        //            //    document.getElementById("hdnMessageType").value = "";
        //            //}
        //        }
        //        else {
        //            var win = GetRadWindow();
        //            if (win != null) {
        //                win.close();
        //            }
        //            else {
        //                $(top.window.document).find('#btnCloseViewResult')[0].click();
        //            }
        //        }
        //    }
        //    else if (document.getElementById('btnSave') == null) {
        //        var win = GetRadWindow();
        //        if (win != null) {
        //            win.close();
        //        }
        //        else {
        //            $(top.window.document).find('#btnCloseViewResult')[0].click();
        //        }
        //    }
        //    else {
        //        var win = GetRadWindow();
        //        if (win != null) {
        //            win.close();
        //        }
        //        else {
        //            $(top.window.document).find('#btnCloseViewResult')[0].click();
        //        }
        //    }
        //}

        function btnSave_ClientNodeClick() {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
        function btnSave_ClientClicked(sender, args) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            __doPostBack('btnSave', "true");
        }


    </script>
    <style type="text/css">
        .style1 {
            height: 12px;
        }

        .elements {
            height: 10px;
            font-size: small;
        }

        .DisplayNone {
            display: none;
        }

        .modal {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1050;
            display: none;
            overflow: hidden;
            -webkit-overflow-scrolling: touch;
            outline: 0;
        }

            .modal.fade .modal-dialog {
                -webkit-transition: -webkit-transform .3s ease-out;
                -o-transition: -o-transform .3s ease-out;
                transition: transform .3s ease-out;
                -webkit-transform: translate(0, -25%);
                -ms-transform: translate(0, -25%);
                -o-transform: translate(0, -25%);
                transform: translate(0, -25%);
            }

            .modal.in .modal-dialog {
                -webkit-transform: translate(0, 0);
                -ms-transform: translate(0, 0);
                -o-transform: translate(0, 0);
                transform: translate(0, 0);
            }

        .modal-open .modal {
            overflow-x: hidden;
            overflow-y: auto;
        }

        .modal-dialog {
            position: relative;
            width: auto;
            margin: 10px;
        }

        .modal-content {
            position: relative;
            background-color: #fff;
            -webkit-background-clip: padding-box;
            background-clip: padding-box;
            border: 1px solid #999;
            border: 1px solid rgba(0, 0, 0, .2);
            border-radius: 6px;
            outline: 0;
            -webkit-box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
            box-shadow: 0 3px 9px rgba(0, 0, 0, .5);
        }

        .modal-backdrop {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1040;
            background-color: #000;
        }

            .modal-backdrop.fade {
                filter: alpha(opacity=0);
                opacity: 0;
            }

            .modal-backdrop.in {
                filter: alpha(opacity=50);
                opacity: .5;
            }

        .modal-header {
            padding: 15px;
            border-bottom: 1px solid #e5e5e5;
        }

            .modal-header .close {
                margin-top: -2px;
            }

        .modal-title {
            margin: 0;
            line-height: 1.42857143;
        }

        .modal-body {
            position: relative;
            padding: 15px;
        }

        .modal-footer {
            padding: 15px;
            text-align: right;
            border-top: 1px solid #e5e5e5;
        }

            .modal-footer .btn + .btn {
                margin-bottom: 0;
                margin-left: 5px;
            }

            .modal-footer .btn-group .btn + .btn {
                margin-left: -1px;
            }

            .modal-footer .btn-block + .btn-block {
                margin-left: 0;
            }

        .modal-scrollbar-measure {
            position: absolute;
            top: -9999px;
            width: 50px;
            height: 50px;
            overflow: scroll;
        }

        @media (min-width: 768px) {
            .modal-dialog {
                width: 600px;
                margin: 30px auto;
            }

            .modal-content {
                -webkit-box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
                box-shadow: 0 5px 15px rgba(0, 0, 0, .5);
            }

            .modal-sm {
                width: 300px;
            }
        }

        @media (min-width: 992px) {
            .modal-lg {
                width: 900px;
            }
        }

        .modal-header:before,
        .modal-header:after,
        .modal-footer:before,
        .modal-footer:after {
            display: table;
            content: " ";
        }

        .modal-header:after,
        .modal-footer:after {
            clear: both;
        }

        .close {
            float: right;
            font-size: 21px;
            font-weight: bold;
            line-height: 1;
            color: #000;
            text-shadow: 0 1px 0 #fff;
            filter: alpha(opacity=20);
            opacity: .2;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
                filter: alpha(opacity=50);
                opacity: .5;
            }

        button.close {
            -webkit-appearance: none;
            padding: 0;
            cursor: pointer;
            background: transparent;
            border: 0;
        }

        .rtsUL, .rtsScroll {
            width: 935px !important;
        }

        .fa-plus {
            content: "\f067";
        }
         .ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable {
            height: 155px !important;
            border: 2px solid !important;
            border-radius: 13px !important;
            top: 504px !important;
            left: 568px !important;
        }
        .ui-menu {
            list-style: none;
            padding: 2px;
            margin: 0;
            display: block;
            outline: none;
        }
         .ui-autocomplete {
            -webkit-margin-before: 3px !important;
            max-height: 150px;
            overflow-y: auto !important;
        }
        

    </style>
    <link href="CSS/jquery-ui.css" rel="stylesheet" />
    <link href="CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="CSS/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.7.2/font/bootstrap-icons.css" />
</head>
<body onload="{sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}">
    <form id="frmViewResult" runat="server" style="background-color: White;">
        <telerik:RadWindowManager ID="Resulst" runat="server" EnableViewState="false">
            <Windows>
                <telerik:RadWindow ID="MessageWindow" runat="server" VisibleOnPageLoad="false" Modal="true"
                    Behaviors="Close" IconUrl="Resources/16_16.ico" EnableViewState="false">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div id="summaryIndexdiv" runat="server" style="height: 660px; display: none; padding: 15px; font-weight: bolder; background-color: aliceblue">The selected document has been deleted and removed from the patient chart.</div>
            <div id="divViewAll" runat="server">
                <table id="tblView" runat="server" style="width: 100%; height: 100%;">
                    <tr id="trPatInfo" runat="server">
                        <td width="100%" colspan="2">
                            <input type="text" runat="server" readonly="readonly" style="width: 100%; border: 1px solid black;" id="txtPatientInformation" class="nonEditabletxtbox" />
                        </td>
                    </tr>
                    <tr style="background-color: #BFDBFF;">
                        <td width="100%" colspan="2">
                            <input type="text" runat="server" readonly="readonly" style="width: 100%; border: 1px solid black;" id="txtFileInformation" class="nonEditabletxtbox" />
                        </td>
                    </tr>
                    <tr id="test">
                        <td id="c1" style="width: 20%;" valign="top" runat="server">
                            <asp:Panel ID="pnlTree" runat="server" Width="100%" BorderStyle="Solid"
                                BorderWidth="1px">
                                <table id="tblTree" runat="server" style="width: 100%;">
                                    <tr>
                                        <td colspan="2" height="3%">
                                            <asp:Label ID="lblDocumentType" runat="server" Text="Document Type" Width="100%" CssClass="spanstyle"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" height="5%"><%-- Added ClientSelectedIndexChanged for Document Type--%><%-- onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}"--%>
                                            <telerik:RadComboBox ID="cboDocumentType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboDocumentType_SelectedIndexChanged"
                                                onchange="{ sessionStorage.setItem('StartLoading', 'false '); StopLoadFromPatChart();}" Width="100%">
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <%-- BUGID:43099 --%>
                                        <td colspan="2" height="3%">
                                            <asp:Label ID="lblDocumentSubType" runat="server" Text="Document Sub Type" Width="100%" CssClass="spanstyle"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- BUGID:43099 --%>
                                        <td height="12%" valign="top" width="90%">
                                            <telerik:RadTextBox ID="txtDocumentSubType" runat="server" Height="100%" ReadOnly="False"
                                                TextMode="MultiLine" Width="100%" Visible="false">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td height="12%" width="10%">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="pbPlus" runat="server" class="button" ImageUrl="~/Resources/plus_new.gif"
                                                            OnClick="pbPlus_Click" ToolTip="Select Document Subtype" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="pbClear" runat="server" class="button" ImageUrl="~/Resources/close_small_pressed.png"
                                                            OnClientClick="Clear();" ToolTip="Clear All" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="pbFilter" runat="server" class="button" ImageUrl="~/Resources/Filter.bmp"
                                                            OnClick="pbFilter_Click" ToolTip="Filter" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" height="62%" valign="top" width="20%">
                                            <telerik:RadTreeView ID="tvViewIndex" runat="server" Height="540px" OnNodeClick="tvViewIndex_NodeClick1"
                                                Width="100%" OnClientNodeClicked="btnSave_ClientNodeClick" CssClass="spanstyle">
                                            </telerik:RadTreeView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td id="c2" style="width: 80%;" valign="top">

                            <telerik:RadTabStrip ID="tabView" runat="server" MultiPageID="RadMultiPage1" OnClientTabSelecting="tabView_TabSelected"
                                OnTabClick="tabView_TabClick" ScrollChildren="True" SelectedIndex="0" Width="100%">
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="100%">
                                <telerik:RadPageView ID="PageViewScan" runat="server" Height="583px">
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="PageViewResultFiles" runat="server" Height="583px">
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="PageViewResult" runat="server" Height="583px">
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="PageViewABIResults" runat="server" Height="583px">
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="PageViewSpirometryResults" runat="server" Height="583px">
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="PageViewMessageLog" runat="server" Height="583px">
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>

                        </td>
                    </tr>
                    <tr style="height: 15%;">
                        <td width="100%" colspan="2" valign="top">
                            <asp:Panel ID="pnlTextbox" runat="server">
                                <table style="width: 100%; height: 117px;">
                                    <tr>
                                        <td height="65%" width="15%">
                                            <asp:Label ID="lblProviderNotes" runat="server" Text="Provider Notes" Width="100%" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td height="65%" width="35%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <DLC:DLC ID="DLC" runat="server" TextboxHeight="48px" TextboxWidth="400px" Value="PROVIDER NOTES" />
                                                    </td>
                                                    <td>
                                                        <i runat="server" id="imgCopyPrevious" class="glyphicon glyphicon-file" title="Interpretation Notes" style="color: #6DABF7; cursor: pointer; width: 22px; height: 25px; font-size: 20px;" visible="false"></i>
                                                    </td>
                                                    <%-- <td>
                                                    <button runat="server" id="imgPrintNotes"  onserverclick="btnPrintInt_ServerClick" title="Print Interpretation Notes" visible="false" style="border:aliceblue;">
                                                        <i class="fa fa-print" style="color:#6DABF7;"></i>
                                                    </button>
                                                </td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="3%"></td>
                                        <td height="65%" width="15%">
                                            <asp:Label ID="lblMedicalAssistantNotes" runat="server" Text="Medical Asst Notes" Width="100%" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td height="65%" width="32%">
                                            <telerik:RadTextBox ID="txtMedicalAssistantNotes" runat="server" DisabledStyle-CssClass="nonEditabletxtbox" EnabledStyle-CssClass="Editabletxtbox" Height="50px" TextMode="MultiLine" Width="100%" Style="margin-top: 1px;">
                                                <%--BugID:46406--%>
                                                <ClientEvents OnValueChanged="txtMedicalAssistantNotes_OnValueChanged" OnKeyPress="txtMedicalAssistantNotes_OnKeyPress" />
                                                <DisabledStyle ForeColor="Black" Resize="None" />
                                                <InvalidStyle Resize="None" />
                                                <HoveredStyle Resize="None" />
                                                <ReadOnlyStyle Resize="None" />
                                                <EmptyMessageStyle Resize="None" />
                                                <FocusedStyle Resize="None" />
                                                <EnabledStyle Resize="None" />
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="35%" width="15%">
                                            <asp:Label ID="Label2" runat="server" Text="Provider Notes History"
                                                Width="100%" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td height="35%" width="35%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtProvNoteshistory" runat="server" ReadOnly="true" Style="background-color: #BFDBFF; width: 440px; height: 55px; resize: none;" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <button runat="server" id="imgPrintNotes" onserverclick="btnPrintInt_ServerClick" title="Print Interpretation Notes" visible="false" style="border: aliceblue;" onclick="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}">
                                                            <i class="fa fa-print" style="color: #6DABF7;"></i>
                                                        </button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="3%"></td>
                                        <td height="35%" width="15%">
                                            <asp:Label ID="Label3" runat="server" Text="Medical Asst Notes History"
                                                Width="100%" CssClass="spanstyle"></asp:Label>
                                        </td>
                                        <td height="35%" width="32%">
                                            <asp:TextBox TextMode="MultiLine" ID="txtMedNoteshistory" runat="server" Style="width: 100%; height: 50px; resize: none;" ReadOnly="true" CssClass="nonEditabletxtbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Move To:" CssClass="spanstyle"></asp:Label>
                                            <asp:RadioButton ID="rdbMA" runat="server" GroupName="MAProvider" Text="MA" AutoPostBack="true" OnCheckedChanged="rdbMA_CheckedChanged" CssClass="Editabletxtbox" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" />
                                            <asp:RadioButton ID="rdbProvider" runat="server" GroupName="MAProvider" Text="Provider" AutoPostBack="true" OnCheckedChanged="rdbProvider_CheckedChanged" CssClass="Editabletxtbox" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" />

                                        </td>
                                        <td height="25%" style="display: none;">
                                            <telerik:RadComboBox ID="cboMoveToMA" Width="307px" Height="150px" OnClientSelectedIndexChanged="cboMoveToMA_valchange"
                                                runat="server" />
                                            <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowAll_CheckedChanged" CssClass="spanstyle" onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" Text="Show All" />
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input id="txtAssignedTo" onchange="cboMoveToMA_valchange();" class="Editabletxtbox" style="width: 400px;" runat="server" />
                                           
                                                    </td>
                                                    <td style="width: 2px;"></td>
                                                    <td>
                                                        <img id="imgclearAssignTo" src="Resources/Delete-Blue.png" alt="X" title="Click to clear the text field." style="margin-top:-5px;" runat="server"/>
                                                    </td>
                                                </tr>
                                            </table>
                                           
                                            </td>
                                         
                                        <td colspan="3" style="text-align: right;">
                                            <table style="width:100% !important">
                                                <tr>
                                                    <td runat="server" id="DelIndexDiv" ></td>
                                                    <td>
                                                        <%-- <div><asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnDeleteIndexing" runat="server" OnClientClick="DeleteFilesIndexing();" Text="Delete Document" CssClass="aspresizedredbutton" Visible="false"  style="margin-left:145px" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel></div>--%>
                                                    </td>
                                                    <td style="display:flex;float:right;">
                                                         <button id="btnAkidoInterpretationNote" runat="server" value="Akido Interpretation Report" class="aspresizedbluebutton" style="margin-right: 4px;" onclick="if(StartLoadingcursor()); AkidoInterpretationNoteClick();">Akido Interpretation Report <i class="bi bi-box-arrow-up-right" aria-hidden="true" style="margin-left: 3px;"></i></button>
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnDeleteIndexing" runat="server" OnClientClick="DeleteFilesIndexing();" Text="Delete Document" CssClass="aspresizedredbutton" Visible="false"  Style="margin-right: 4px;" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                                        <input type="button" id="btnSave" runat="server" value="Save" onclick="return btnSave_ClientClicked();" onserverclick="btnSave_Click1" class="aspresizedgreenbutton" style="height: 26px !important;margin-right: 4px;" />
                                                    <asp:Button ID="btnTask" runat="server" CssClass="aspresizedbluebutton" Text="Task" OnClientClick="OpenPatientCommunication();"  Style="margin-right: 4px;" />
                                                        <asp:Button ID="btnFindAppointments" runat="server" OnClientClick="return OpenFindAllAppointments();"
                                                Text="Find All Appointments" CssClass="aspresizedbluebutton"  Style="margin-right: 4px;" />
                                                        <asp:Button ID="btnpatientChart1" runat="server" OnClientClick="return btnpatientChart_Click();" Text="OpenPatientChart"
                                                Visible="false" CssClass="aspresizedbluebutton"  Style="margin-right: 4px;" />
                                                        <asp:Button ID="btnePrescribe" runat="server" OnClientClick="return ClickPrescription();" Text="eRx" Width="60px" Visible="false" CssClass="aspresizedbluebutton"  Style="margin-right: 4px;" />
                                                        <asp:Button ID="btnEfax" runat="server" Text="Send Fax" OnClientClick="funEFax();" CssClass="aspresizedbluebutton" Style="margin-right: 4px;" />
                                                    </td>
                                               </tr></table>
                                        </td>
                                    </tr>

                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="height: 5%;">
                        <td colspan="2" width="100%">
                            <asp:Panel ID="pnlButton" runat="server">
                                <table style="width: 100%;">
                                    <tr>
                                        <td width="3%">
                                            <telerik:RadButton ID="btnPrintEducatnMaterial" runat="server" Height="32px" Image-ImageUrl="~/Resources/images_info.jpg" Image-IsBackgroundImage="true" OnClick="btnPrintEducatnMaterial_Click" Visible="false" Width="32px">
                                                <Image EnableImageButton="True" ImageUrl="~/Resources/images_info.jpg" />
                                                <%--BugID:48550 --%>
                                            </telerik:RadButton>
                                        </td>
                                        <td width="50%">
                                            <asp:CheckBox ID="chkPhyName" runat="server" onclick="EnableSave();" Width="100%" CssClass="Editabletxtbox" />
                                        </td>
                                        <%--<td width="5%">
                                           <telerik:RadButton ID="btnSave" AutoPostBack="false" runat="server" OnClick="btnSave_Click1"  Text="Save" OnClientClicked="btnSave_ClientClicked"  Font-Size="13px" style="margin-top:-51px; margin-left: 675px;height:12px!important;text-align:center;" ButtonType="LinkButton"  CssClass="greenbutton teleriknormalbuttonstyle">
                                        </telerik:RadButton>
                                    </td>--%><%--OnClientClick="btnSave_ClientClicked(); return false;"--%>
                                        <td></td>
                                        <td width="10%">
                                            <telerik:RadButton ID="btnMoveToMa" AutoPostBack="false" runat="server" OnClick="btnMoveToMa_Click" OnClientClicked="ClickMovetoma" Text="Save & Move To MA" Width="100%" Font-Size="13px" ButtonType="LinkButton" CssClass="bluebutton teleriknormalbuttonstyle"
                                                onchange="{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}" Style="height: 26px !important; margin-left: 52px;">
                                            </telerik:RadButton>
                                        </td>
                                        <td width="2%"></td>
                                        <td width="15%">
                                            <telerik:RadButton ID="btnMoveToNextProcess" AutoPostBack="false" runat="server" OnClick="btnMoveToNextProcess_Click" OnClientClicked="ClickMovetonextProcess" Text="Save & Move To Next Process" Width="100%" Font-Size="13px" ButtonType="LinkButton"
                                                CssClass="bluebutton teleriknormalbuttonstyle" Style="height: 26px !important; margin-left: 34px;">
                                            </telerik:RadButton>
                                        </td>

                                        <td width="2%"></td>
                                        <td width="5%">
                                            <asp:Button ID="btnClose" runat="server" Text="Close" Width="92%" Style="margin-left: 14px; height: 26px !important;" OnClientClick="return ClickClose();" Class="aspresizedredbutton"
                                                AccessKey="Q" />
                                            <%--<telerik:RadButton ID="btnClose" runat="server" OnClientClicked="return ClickClose();" Text="Close" Width="92%" Style="margin-left: 14px; height: 26px !important;"
                                            Font-Size="13px" ButtonType="LinkButton" CssClass="redbutton teleriknormalbuttonstyle">
                                        </telerik:RadButton>--%>
                                        </td>
                                        <td width="2%"></td>
                                        <td>
                                            <telerik:RadButton ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Print" Width="100%" Visible="false" ButtonType="LinkButton" CssClass="greenbutton teleriknormalbuttonstyle">
                                            </telerik:RadButton>
                                        </td>

                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hdnSave" runat="server" />
            <asp:HiddenField ID="hdnSelectedItem" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnMessageType" runat="server" />
            <asp:HiddenField ID="hdnEnableYesNo" runat="server" />
            <asp:HiddenField ID="hdnTab" runat="server" />
            <asp:HiddenField ID="hdnpath" runat="server" />
            <asp:HiddenField ID="hdnfileindexid" runat="server" />
            <asp:HiddenField ID="hdnFaxpath" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnHumanId" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPatientfirstname" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPatientlastname" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPatientmiddlename" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnDOB" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnPatientType" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnFileName" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLeftPaneOrderSubmitID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLeftPaneResultMasterID" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLeftPaneCurrentProcess" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnLeftPaneObjType" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnSubDocumentType" runat="server" EnableViewState="false" />

             <asp:HiddenField ID="hdnNewProviderNotes" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnNewProviderNotesHistory" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnNewProviderhistoryattribute" runat="server" EnableViewState="false" />

            <asp:HiddenField ID="hdnHumanText" runat="server" EnableViewState="false" />

            <asp:Button ID="hdnSetvalue" OnClick="btn_SetValue"  runat="server" Style="display: none;"/>

             <asp:HiddenField ID="hdncurrentProcess" runat="server" EnableViewState="false" />

            <asp:Button ID="hdnbtngenerateresultxml" runat="server" OnClick="hdnbtngeneratexml_Click" Style="display: none" />

            <asp:HiddenField ID="hdnFaxSubject" runat="server" EnableViewState="false" />
            <asp:Button ID="hdnSaveEnable" runat="server" Style="display: none;" />
            <%--OnClick="hdnSaveEnable_Click"--%>
            <asp:HiddenField ID="hdnAssignTo" runat="server" EnableViewState="false" />
            <asp:HiddenField ID="hdnAkidoInterpretationNote" runat="server" />
            <asp:Button ID="btnMessageType" runat="server" Text="Button" Style="display: none"
                OnClientClick="return ClickClose();" />
        </telerik:RadAjaxPanel>
    </form>
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="JScripts/jquery-2.1.3.js" type="text/javascript"></script>
        <script type="text/javascript">jQuery.noConflict();</script>
        <script src="JScripts/JSErrorMessage.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella-","") %>" type="text/javascript"></script>
        <script src="JScripts/JSResult.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>
        <script src="JScripts/JSViewResult.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>
        <script src="JScripts/JSCustomDLC.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>" type="text/javascript"></script>
        <script src="JScripts/JSAvoidRightClick.js?version=<%=ConfigurationManager.AppSettings["VersionConfiguration"].ToString().Replace("Capella - ","") %>"
            type="text/javascript"></script>
        <script src="JScripts/bootstrap.min.js" type="text/javascript"></script>
    </asp:PlaceHolder>
</body>
</html>
