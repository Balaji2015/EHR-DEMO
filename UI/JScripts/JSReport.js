var now = new Date()
var duedate = new Date(now);
var d = duedate.getDate();
var m = duedate.getMonth();
var y = duedate.getFullYear();
var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
var date = d + "-" + arr[m] + "-" + y;

var PhysicianList;
var PhysicianList1;
var AllPhysicianList;
var UserRole;
var UserID;
var UserName;
var ReportNameList;
var FacilityList;
var FacilityList1 = "";
var CurrentOwnerList;
var CurrentProcessList;
var ApptCurrentProcessList;
var FacilityName;
var sPSFacilityList;
var sBIRTReportUrl;
var sDBConnection;
var intPatientlen = -1;
var UI_Time_Start;
var UI_Time_Stop;
var intPatientlen = -1;
var arrPatient = [];
function RadWindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}
function btnOpen_Clicked(sender, args) {
    var cboItem = document.getElementById("cboSelectCategory").value;
    if (cboItem == "EHR REPORTS") {
        var list = $find("lstReportList");
        var items = list.get_items();
        var index = items.indexOf(list.get_selectedItem());
        var firstItem = items.getItem(index);
        var SelectItem = firstItem._textElement.innerText;
        switch (SelectItem) {
            case "PRODUCTIVITY REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmProductivityReport.aspx', 250, 470, null, "MenuWindow"); }, 0);
                    break;
                }
            case "APPOINTMENTS SUMMARY REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmAppointmentSummaryReport.aspx', 295, 480, null, "MenuWindow"); }, 0);
                    break;
                }
            case "AUDIT LOG REPORT":
                {

                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmAuditLogs.aspx', 820, 1200, null, "MenuWindow"); }, 0);
                    break;
                }
            case "CODERS REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmCodersReport.aspx', 480, 960, null, "MenuWindow"); }, 0);
                    break;
                }
            case "eRx REFILL REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmeRxRefillReport.aspx', 260, 675, null, "MenuWindow"); }, 0);
                    break;
                }
            case "OFFICE MANAGER SUMMARY REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmOfficeManagerStatusReport.aspx', 300, 420, null, "MenuWindow"); }, 0);
                    break;
                }
            case "CMG ANCILLARY DOCUMENTATION REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmCMGAncillaryDocumentationReport.aspx', 545, 930, null, "MenuWindow"); }, 0);
                    break;
                }
            case "PATIENT LIST":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmGeneratePatientLists.aspx', 920, 1190, null, "MenuWindow"); }, 0);
                    break;
                }
            case "APPOINTMENTS REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmAppointmentsReport.aspx', 580, 820, null, "MenuWindow"); }, 0);
                    break;
                }
            case "COPD REPORT":
                {
                    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmCATReport.aspx', 810, 1075, null, "MenuWindow"); }, 0);
                    break;
                }
        }
    }
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function OpenFindPhysician() {
    var oBrowserWnd = GetRadWindow().BrowserWindow;
    var oWin = oBrowserWnd.radopen("frmFindReferralPhysician.aspx", "AppointmentWindow");
    setTimeout(
    function () {
        oWin.remove_close(OnClientCloseFindPhysician);
        oWin.SetModal(true);
        oWin.set_visibleStatusbar(false);
        oWin.setSize(930, 256);
        oWin.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
        oWin.set_iconUrl("Resources/16_16.ico");
        oWin.set_keepInScreenBounds(true);
        oWin.set_centerIfModal(true);
        oWin.center();
        oWin.set_showContentDuringLoad(false);
        oWin.set_reloadOnShow(true);
        oWin.add_close(OnClientCloseFindPhysician);
        oWin.remove_close(OnClientCloseFindPatient);
    }, 0);
    return false;
}
function FindPatient() {
    var oBrowserWnd = GetRadWindow().BrowserWindow;
    var childWindow = oBrowserWnd.radopen("frmFindPatient.aspx", "AppointmentWindow");
    setTimeout(
    function () {
        childWindow.SetModal(true);
        childWindow.remove_close(OnClientCloseFindPatient);
        childWindow.set_visibleStatusbar(false);
        childWindow.setSize(1200, 251);
        childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
        childWindow.set_iconUrl("Resources/16_16.ico");
        childWindow.set_keepInScreenBounds(true);
        childWindow.set_centerIfModal(true);
        childWindow.set_showContentDuringLoad(false);
        childWindow.set_reloadOnShow(true);
        childWindow.center();
        childWindow.add_close(OnClientCloseFindPatient);
        childWindow.remove_close(OnClientCloseFindPhysician);

    }, 0);
    return false;
}
function OnClientCloseFindPhysician(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined) {
        document.getElementById("txtPCPname").value = arg.sPhyName;
        document.getElementById("hdnTransferVaraible").value = arg.ulPhyId;
    }
}
function OnClientCloseFindPatient(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined) {
        document.getElementById("txtPatientName").value = arg.PatientName;
        document.getElementById("hdnPatientValues").value = arg.HumanId;
    }
}
function btnClearAll_Clicked(sender, args) {
    var ArguOne = "clear all";
    if (sender.get_text() == "Clear All") {
        ArguOne = "clear all";
    }
    else {
        ArguOne = "cancel";
    }

    if (DisplayErrorMessage("759004", '', ArguOne)) {
        sender.set_autoPostBack(true);
    }
    else {
        sender.set_autoPostBack(false);
    }
}
function GetUTCTime() {
    var dt = new Date();
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + ("0" + now.getUTCDate()).slice(-2) + '/' + now.getUTCFullYear();
    var minutes;
    var seconds;
    if (now.getUTCMinutes() < 10) {
        minutes = '0' + now.getUTCMinutes();
    }
    else {
        minutes = now.getUTCMinutes();
    }
    if (now.getUTCSeconds() < 10) {
        seconds = '0' + now.getUTCSeconds();
    }
    else {
        seconds = now.getUTCSeconds();
    }
    utc += ' ' + now.getUTCHours() + ':' + minutes + ':' + seconds;
    document.getElementById("hdnLocalTime").value = utc;
}
function cboFacilityName_SelectedIndexChanged(sender, args) {
    var SelectItem = $find("cboFacilityName")._text;
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}
function OpenPDF() {
    var obj = new Array();
    obj.push("SI=" + document.getElementById('SelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 835, 900, obj, "CMGAncillaryWindow"); }, 0);
}
function dateInput_OnKeyDown(sender, e) {
    e = e || window.event;
    if (e.keyCode == 8 || e.keyCode == 46)
        e.preventDefault();
}
function pbFindPlan_Click() {
    setTimeout(
       function () {
           var oWnd = GetRadWindow();
           var childWindow = oWnd.BrowserWindow.radopen("frmSelectPayer.aspx", "AddInsuredModalWindow");
           SetRadWindowProperties(childWindow, 480, 880);
           childWindow.add_close(SelectPatientClick);
       }, 0);
    return false;
}
function SelectPatientClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById("hdnPlanName").value = Result.planName;
        document.getElementById("btnSelectPlan").click();

    }

}
function SetRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}
function btnExportToExcel_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
function btnRunReport_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
function downloadURI(uri, name) {
    DisplayErrorMessage('750002');
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var link = document.createElement('a');
    link.download = name;
    link.href = uri; link.click();
}
function showAllPhy(checkStatus) {
    $("#cboSelectedPhysician")[0].selectedIndex = 0;
    if (checkStatus.firstChild.checked) {
        $("#cboSelectedPhysician > option").each(
            function () {
                $(this).css('display', 'block');
            });
    }
    else {
        $("#cboSelectedPhysician > option").each(function () {
            var option = $(this);
            if (option.attr('default') == 'true' || option.attr('default') == '') {
                option.css('display', 'block');
            }
            else {
                option.css('display', 'none');
            }
        });
    }
}
function btnLog_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var frmdt = document.getElementById('dtpFromDate_dateInput').value;
    var Todt = document.getElementById('dtpTodate_dateInput').value;
    if (frmdt == "" || frmdt == null) {
        DisplayErrorMessage('765010');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
        return;
    }
    if (Todt == "" || Todt == null) {
        DisplayErrorMessage('765008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
        return;
    }
    if (frmdt >= Todt) {
        DisplayErrorMessage('765007');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
        return;
    }
    sender.set_autoPostBack(true);
}
function OpenReport(path) {

    $(top.window.document).find('#ProcessFrame')[0].src = "about:blank";

    $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#mdldlg")[0].style.width = "970px";
    $(top.window.document).find("#ProcessModal")[0].style.width = "";
    $(top.window.document).find("#ProcessModal")[0].style.zIndex = "6000";
    $(top.window.document).find("#ProcessModal")[0].style.background = "transparent";
    $(top.window.document).find('#ProcessFrame')[0].src = path;
    $(top.window.document).find("#ModalTitle")[0].textContent = "Appointment Report";
}
function btnClearAll_Clicking(sender, args) {
    var IsClearAll;
    if (sender == undefined || sender._text == "Clear All")
        IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        document.getElementById("btnClear").click();
        GetUTCTime();
    }
    else {
        if (args != undefined)
            args._cancel = true;
    }
}
function EnableGeneratebtn(sender, args) {
    window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable.value = "true";
    document.getElementById("btnExportToExcel").disabled = false;
}
function Report_Type() {

    $('#dtpFromDate')[0].value = date;
    $('#dtpToDate')[0].value = date;

    $('#sltCurrentProcess option[value="0"]').prop("selected", true);
    $('#sltCurrentOwner option[value="0"]').prop("selected", true);

    $('#sltReportFor option[value="0"]').prop("selected", true);
    $("#chkIncludeVisit")[0].checked = false;
    $("#chkShowActive")[0].checked = false;

    $('#txtPatientSearch').val('');
    $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });

    var report_type = $("#sltReportType option:selected").val();
    if (report_type == "Audit_Log_Report" || report_type == "Coders_Report" || report_type == "Transportation_Report") {
        $('.report_batch').css("display", "none");
        $('#divReport').css("display", "block");
        $("#iReportFrame").attr("src", "about:blank");
        $(".fromdate").css({ "display": "block" });
        $(".todate").css({ "display": "block" });
        $(".facility").css({ "display": "none" });
        $(".facility").attr("disabled", false);
        $(".physician").css({ "display": "none" });
        $(".currentprocess").css({ "display": "none" });
        $(".currentowner").css({ "display": "none" });
        $(".reportfor").css({ "display": "none" });
        $(".includevisit").css({ "display": "none" });
        $(".showactive").css({ "display": "none" });
        $(".patientname").css({ "display": "none" });
        $(".runreport").css({ "display": "block" });
        $(".clearall").css({ "display": "block" });
    }
    else if (report_type == "Appointment_Report" || report_type == "COPD_Report") {
        $('.report_batch').css("display", "none");
        $('#divReport').css("display", "block");
        $("#iReportFrame").attr("src", "about:blank");
        $(".fromdate").css({ "display": "block" });
        $(".todate").css({ "display": "block" });
        $(".facility").css({ "display": "block" });
        $(".facility").attr("disabled", false);
        $(".physician").css({ "display": "block" });
        $(".currentprocess").css({ "display": "none" });
        $(".currentowner").css({ "display": "none" });
        $(".reportfor").css({ "display": "none" });
        $(".includevisit").css({ "display": "none" });
        $(".showactive").css({ "display": "none" });
        $(".patientname").css({ "display": "block" });
        $(".runreport").css({ "display": "block" });
        $(".clearall").css({ "display": "block" });
    }
    else if (report_type == "eRx_Refill_Report") {
        $('.report_batch').css("display", "none");
        $('#divReport').css("display", "block");
        $("#iReportFrame").attr("src", "about:blank");
        $(".fromdate").css({ "display": "block" });
        $(".todate").css({ "display": "block" });
        $(".facility").css({ "display": "block" });
        $(".facility").attr("disabled", true);
        $(".physician").css({ "display": "block" });
        $(".currentprocess").css({ "display": "none" });
        $(".currentowner").css({ "display": "none" });
        $(".reportfor").css({ "display": "none" });
        $(".includevisit").css({ "display": "none" });
        $(".showactive").css({ "display": "none" });
        $(".patientname").css({ "display": "none" });
        $(".runreport").css({ "display": "block" });
        $(".clearall").css({ "display": "block" });
    }
    else if (report_type == "Patient_Seen_and_Not_Seen_Report") {
        $('.report_batch').css("display", "none");
        $('#divReport').css("display", "block");
        $("#iReportFrame").attr("src", "about:blank");
        $(".fromdate").css({ "display": "block" });
        $(".todate").css({ "display": "block" });
        $(".facility").css({ "display": "none" });
        $(".facility").attr("disabled", false);
        $(".physician").css({ "display": "none" });
        $(".currentprocess").css({ "display": "none" });
        $(".currentowner").css({ "display": "none" });
        $(".reportfor").css({ "display": "block" });
        $(".includevisit").css({ "display": "block" });
        $(".showactive").css({ "display": "block" });
        $(".patientname").css({ "display": "none" });
        $(".runreport").css({ "display": "block" });
        $(".clearall").css({ "display": "block" });
    }
    else if (report_type == "Office_Manager_Summary_Report") {
        $('.report_batch').css("display", "none");
        $('#divReport').css("display", "block");
        $("#iReportFrame").attr("src", "about:blank");
        $(".fromdate").css({ "display": "block" });
        $(".todate").css({ "display": "block" });
        $(".facility").css({ "display": "none" });
        $(".facility").attr("disabled", false);
        $(".physician").css({ "display": "none" });
        $(".currentprocess").css({ "display": "block" });
        $(".currentowner").css({ "display": "block" });
        $(".reportfor").css({ "display": "none" });
        $(".includevisit").css({ "display": "none" });
        $(".showactive").css({ "display": "none" });
        $(".patientname").css({ "display": "none" });
        $(".runreport").css({ "display": "block" });
        $(".clearall").css({ "display": "block" });
    }
    else if (report_type == "0") {
        $(".fromdate").css({ "display": "none" });
        $(".todate").css({ "display": "none" });
        $(".facility").css({ "display": "none" });
        $(".facility").attr("disabled", false);
        $(".physician").css({ "display": "none" });
        $(".currentprocess").css({ "display": "none" });
        $(".currentowner").css({ "display": "none" });
        $(".reportfor").css({ "display": "none" });
        $(".includevisit").css({ "display": "none" });
        $(".showactive").css({ "display": "none" });
        $(".patientname").css({ "display": "none" });
        $(".runreport").css({ "display": "none" });
        $(".clearall").css({ "display": "none" });
    }
    else if (report_type == "Phone_Encounter_Report") {
        $('.report_batch').css("display", "none");
        $('#divReport').css("display", "block");
        $("#iReportFrame").attr("src", "about:blank");
        $(".fromdate").css({ "display": "block" });
        $(".todate").css({ "display": "block" });
        $(".facility").css({ "display": "none" });
        $(".facility").attr("disabled", false);
        $(".physician").css({ "display": "none" });
        $(".currentprocess").css({ "display": "none" });
        $(".currentowner").css({ "display": "none" });
        $(".reportfor").css({ "display": "none" });
        $(".includevisit").css({ "display": "none" });
        $(".showactive").css({ "display": "none" });
        $(".patientname").css({ "display": "block" });
        $(".runreport").css({ "display": "block" });
        $(".clearall").css({ "display": "block" });
    }
}
function Report_Click() {
    //Parameter Declaration
    var sParameters = "";
    var pFromDate, pToDate, pFacility, pPhysician, pCurrentProcess, pCurrentOwner, pReportFor, pIncludeVisit, pShowActive, pPatientName;;

    if (parseInt($("#sltReportType option:selected").val()) == 0) {
        DisplayErrorMessage('9004');
        return true;
    }

    var ReportTempalteName = $("#sltReportType option:selected").val();
    var sReportName = $("#sltReportType option:selected").text();


    var sFromDate = "";
    var sToDate = "";
    if ($('#dtpFromDate').val() != "") {
        sFromDate = new Date($('#dtpFromDate').val()).dateFormat("Y-m-d");
        pFromDate = "From Date: " + new Date($('#dtpFromDate').val()).dateFormat("d-M-Y");
    }
    else {
        sFromDate = "0001-01-01";
        pFromDate = "From Date: 01-Jan-0001";
    }
    if ($('#dtpToDate').val() != "") {
        sToDate = new Date($('#dtpToDate').val()).dateFormat("Y-m-d");
        pToDate = "To Date: " + new Date($('#dtpToDate').val()).dateFormat("d-M-Y");
    }
    else {
        sToDate = "9999-12-31";
        pToDate = "To Date: 31-Dec-9999";
    }

    if ($('#dtpFromDate')[0].validationMessage == "Please enter a valid value. The field is incomplete or has an invalid date.") {
        DisplayErrorMessage('9001');
        return true;
    }
    else if ($('#dtpToDate')[0].validationMessage == "Please enter a valid value. The field is incomplete or has an invalid date.") {
        DisplayErrorMessage('9002');
        return true;
    }
    else if (new Date(sFromDate) > new Date(sToDate)) {
        DisplayErrorMessage('9003');
        return true;
    }

    if ($("#sltReportType option:selected").val() == "Patient_Seen_and_Not_Seen_Report") {
        if (parseInt($("#sltReportFor option:selected").val()) == 0) {
            DisplayErrorMessage('9005');
            return true;
        }
    }

    var sFacilityName = "%25";
    if (parseInt($("#sltFacility option:selected").val()) > 0) {
        sFacilityName = $("#sltFacility option:selected").val().replace("#", "%23");
    }
    if (sFacilityName == "%25") {
        pFacility = "Facility Name: ALL";
    }
    else {
        pFacility = "Facility Name: " + sFacilityName;
    }

    var sPhysicianID = "%25";
    if (parseInt($("#sltPhysician option:selected").val()) > 0) {
        sPhysicianID = $("#sltPhysician option:selected").val();
    }
    if (sPhysicianID == "%25") {
        pPhysician = "Physician Name: ALL";
    }
    else {
        pPhysician = "Physician Name: " + $("#sltPhysician option:selected").text();
    }


    var sCurrentProcess = "%25";
    if (parseInt($("#sltCurrentProcess option:selected").val()) != 0) {
        sCurrentProcess = $("#sltCurrentProcess option:selected").val().replace("#", "%23");
    }
    if (sCurrentProcess == "%25") {
        pCurrentProcess = "Current Process: ALL";
    }
    else {
        pCurrentProcess = "Current Process: " + sCurrentProcess;
    }

    var sCurrentOwner = "%25";
    if (parseInt($("#sltCurrentOwner option:selected").val()) != 0) {
        sCurrentOwner = $("#sltCurrentOwner option:selected").val().replace("#", "%23");
    }
    if (sCurrentOwner == "%25") {
        pCurrentOwner = "Current Process: ALL";
    }
    else {
        pCurrentOwner = "Current Process: " + sCurrentOwner;
    }

    var sReportFor = "";
    if (parseInt($("#sltReportFor option:selected").val()) != 0) {
        sReportFor = $("#sltReportFor option:selected").val();
    }
    pReportFor = "Report For: " + sReportFor;

    var sIncludeVisit = "";
    sIncludeVisit = $("#chkIncludeVisit")[0].checked;
    if (sIncludeVisit == true) {
        pIncludeVisit = "Home Visit is Included";
    }
    else {
        pIncludeVisit = "Home Visit is Excluded";
    }

    var sShowActive = "%25";
    if ($("#chkShowActive")[0].checked == true) {
        pShowActive = "Only For Active Patients";
        sShowActive = "ACTIVE";
    }
    else {
        pShowActive = "For All Patients";
    }

    var sPatientID = "%25";
    if (parseInt(document.getElementById("txtPatientSearch").attributes["data-human-id"].value) > 0) {
        sPatientID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
    }
    if (sPatientID == "%25") {
        pPatientName = "Patient Name: ALL";
    }
    else {
        pPatientName = "Patient Name: " + document.getElementById("txtPatientSearch").value.split('|')[0];
    }

    
    var ReportUrl;
    if (ReportTempalteName != "Patient_Seen_and_Not_Seen_Report") {
        ReportUrl = sBIRTReportUrl + "_" + ReportTempalteName.toUpperCase() + ".rptdesign" + sDBConnection;
    }
    else {
        ReportUrl = sBIRTReportUrl + "_" + sReportFor.toUpperCase() + "_REPORT.rptdesign" + sDBConnection;
    }

    if (ReportTempalteName == "Audit_Log_Report" || ReportTempalteName == "Coders_Report" || ReportTempalteName == "Transportation_Report") {
        sParameters = pFromDate + ", " + pToDate;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&UserName=" + UserName + "&__title=" + sReportName;
    }
    else if (ReportTempalteName == "Appointment_Report") {
        sParameters = pFromDate + ", " + pToDate + ", " + pFacility + ", " + pPhysician + ", " + pPatientName;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&FacilityName=" + sFacilityName + "&PhysicianID=" + sPhysicianID + "&UserName=" + UserName + "&CurrentList=" + ApptCurrentProcessList + "&PatientID=" + sPatientID + "&__title=" + sReportName;
    }
    else if (ReportTempalteName == "COPD_Report") {
        sParameters = pFromDate + ", " + pToDate + ", " + pFacility + ", " + pPhysician + ", " + pPatientName;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&FacilityName=" + sFacilityName + "&PhysicianID=" + sPhysicianID + "&UserName=" + UserName + "&PatientID=" + sPatientID + "&__title=" + sReportName;
    }
    else if (ReportTempalteName == "eRx_Refill_Report") {
        sParameters = pFromDate + ", " + pToDate + ", " + pPhysician;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&PhysicianID=" + sPhysicianID + "&UserName=" + UserName + "&__title=" + sReportName;
    }
    else if (ReportTempalteName == "Office_Manager_Summary_Report") {
        sParameters = pFromDate + ", " + pToDate + ", " + pCurrentProcess + ", " + pCurrentOwner;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&CurrentProcess=" + sCurrentProcess + "&CurrentOwner=" + sCurrentOwner + "&UserName=" + UserName + "&__title=" + sReportName;
    }
    else if (ReportTempalteName == "Patient_Seen_and_Not_Seen_Report") {
        sParameters = pFromDate + ", " + pToDate + ", " + pReportFor + ", " + pIncludeVisit + ", " + pShowActive;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&ReportFor=" + sReportFor + "&IncludeVisit=" + sIncludeVisit + "&ShowActive=" + sShowActive + "&FacilityName=" + FacilityList1.replace(/,\s*$/, "") + "&UserName=" + UserName + "&__title=" + sReportName;
    }
    else if (ReportTempalteName == "Phone_Encounter_Report") {
        sParameters = pFromDate + ", " + pToDate + ", " + pPatientName;
        document.getElementById("iReportFrame").src = ReportUrl + "&ReportName=" + sReportName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&PatientID=" + sPatientID + "&UserName=" + UserName + "&__title=" + sReportName;
    }
    return false;
}
function btnClearAll_Clicked(sender, args) {
    var ArguOne = "clear all";
    if (sender.get_text() == "Clear All") {
        ArguOne = "clear all";
    }
    else {
        ArguOne = "cancel";
    }

    if (DisplayErrorMessage("759004", '', ArguOne)) {
        sender.set_autoPostBack(true);
    }
    else {
        sender.set_autoPostBack(false);
    }
}
$(document).ready(function () {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        url: "WebServices/ReportService.asmx/LoadReport",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function success(data) {
            
            var ReportLoadList = JSON.parse(data.d);

            ReportNameList = ReportLoadList.ReportNameList;
            FacilityList = ReportLoadList.FacilityList;
            PhysicianList = ReportLoadList.PhysicianList;
            AllPhysicianList = ReportLoadList.AllPhysicianList;
            ApptCurrentProcessList = ReportLoadList.ApptCurrentProcessList;

            UserRole = ReportLoadList.UserRole;
            UserID = ReportLoadList.UserID;
            UserName = ReportLoadList.UserName;
            FacilityName = ReportLoadList.FacilityName;
            sPSFacilityList = ReportLoadList.sPSFacilityList;

            CurrentOwnerList = ReportLoadList.CurrentOwnerList;
            CurrentProcessList = ReportLoadList.CurrentProcessList;

            sBIRTReportUrl = ReportLoadList.BIRTUrl;
            sDBConnection = ReportLoadList.DBConnection;

            $("#sltReportType option").remove();
            $("#sltReportType").append('<option value="0">' + "--Select Report Name--" + '</option>');
            for (i = 0; i < ReportNameList.length; i++) {
                var arrReportName = ReportNameList[i].Value;
                var arrReportValue = ReportNameList[i].Description;
                $("#sltReportType").append('<option value="' + arrReportValue + '">' + arrReportName + '</option>');
            }


            $("#sltFacility option").remove();
            $("#sltFacility").append('<option value="' + '0' + '">' + "--Select Facility --" + '</option>');
            for (i = 0; i < FacilityList.length; i++) {
                var arrFacName = FacilityList[i].Fac_Name;
                FacilityList1 += "'" + FacilityList[i].Fac_Name + "',";
                $("#sltFacility").append('<option value="' + arrFacName + '">' + arrFacName + '</option>');
            }
            $('#sltFacility option[value="' + FacilityName + '"]').prop("selected", true);

            $("#sltPhysician option").remove();
            $("#sltPhysician").append('<option value="' + '0' + '">' + "--Select Physician --" + '</option>');
            for (i = 0; i < PhysicianList.length; i++) {
                var arrPhyName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
                var arrPhyID = PhysicianList[i].Id;
                $("#sltPhysician").append('<option value="' + arrPhyID + '">' + arrPhyName + '</option>');
            }
            if (UserRole == "Physician") {
                $('#sltPhysician option[value="' + UserID + '"]').prop("selected", true);
            }

            $("#sltCurrentProcess option").remove();
            $("#sltCurrentProcess").append('<option value="' + '0' + '">' + "--Select Current Process --" + '</option>');
            for (i = 0; i < CurrentProcessList.length; i++) {
                var arrCPName = CurrentProcessList[i].To_Process;
                $("#sltCurrentProcess").append('<option value="' + arrCPName + '">' + arrCPName + '</option>');
            }

            $("#sltCurrentOwner option").remove();
            $("#sltCurrentOwner").append('<option value="' + '0' + '">' + "--Select Current Owner --" + '</option>');
            for (i = 0; i < CurrentOwnerList.length; i++) {
                var arrCOName = CurrentOwnerList[i];
                $("#sltCurrentOwner").append('<option value="' + arrCOName + '">' + arrCOName + '</option>');
            }

            $("#dtpFromDate").datetimepicker({ closeOnDateSelect: true, timepicker: false, format: 'd-M-Y' });
            $("#dtpToDate").datetimepicker({ closeOnDateSelect: true, timepicker: false, format: 'd-M-Y' });
            $('#dtpFromDate')[0].value = date;
            $('#dtpToDate')[0].value = date;
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        },
        error: function onerror(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
    var curleft = curtop = 0;
    var current_element = document.getElementById('txtPatientSearch');
    if (current_element == null) {
        current_element = document.getElementById('txtProviderSearch');
        curtop = 5;
    }
    window.setTimeout(function () {
        $('#txtPatientSearch').focus();
    }, 50);

    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    $("#imgClearPatientText").css({
        "cursor": "pointer",
        "width": "20px",
        "height": "20px"
    }).on("click", function () {
        $('#txtPatientSearch').val('').focus();
        $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });
    if ($("#txtPatientSearch").length) {
        $("#txtPatientSearch").autocomplete({
            source: function (request, response) {
                if ($("#txtPatientSearch").val().trim().length > 2) {
                    if (intPatientlen == 0) {
                        UI_Time_Start = new Date();
                        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                        this.element.on("keydown", PreventTyping);
                        arrPatient = [];
                        var strkeyWords = $("#txtPatientSearch").val().split(' ');
                        var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                        var account_status = "ACTIVE";
                        var patient_status = "ALIVE";
                        var patient_type = "ALL";
                        var WSData = {
                            text_searched: strkeyWords[0],
                            account_status: account_status,
                            patient_status: patient_status,
                            human_type: patient_type
                        };

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "WebServices/ReportService.asmx/GetPatientDetailsByTokens",
                            data: JSON.stringify(WSData),
                            dataType: "json",
                            success: function (data) {
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                $("#txtPatientSearch").off("keydown", PreventTyping);
                                var jsonData = $.parseJSON(data.d);
                                if (jsonData.Error != undefined) {
                                    alert(jsonData.Error);
                                    return;
                                }
                                if (jsonData.Time_Taken != undefined)
                                    LogTimeString(jsonData.Time_Taken);
                                if (jsonData.Result != undefined) {
                                    var no_matches = [];
                                    no_matches.push(jsonData.Result);
                                    response($.map(no_matches, function (item) {
                                        return {
                                            label: item,
                                            val: "0"
                                        }
                                    }));
                                }
                                else {
                                    var results;
                                    if (bMoreThanOneKeyword)
                                        results = Filter(jsonData.Matching_Result, request.term);
                                    else
                                        results = jsonData.Matching_Result;

                                    arrPatient = jsonData.Matching_Result;
                                    response($.map(results, function (item) {
                                        return {
                                            label: item.label,
                                            val: JSON.stringify(item.value),
                                            value: item.value.HumanId
                                        }
                                    }));
                                }
                            },
                            error: function OnError(xhr) {
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                if (xhr.status == 999)
                                    window.location = "/frmSessionExpired.aspx";
                                else {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                                }
                            }

                        });
                    }
                    else if (intPatientlen != -1) {

                        var results = Filter(arrPatient, request.term);
                        response($.map(results, function (item) {
                            return {
                                label: item.label,
                                val: JSON.stringify(item.value),
                                value: item.value.HumanId
                            }
                        }));
                    }
                }
            },
            minlength: 0,
            multiple: true,
            mustMatch: false,
            select: PatientSelected,
            open: function () {
                $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearch').width());
                $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
                $('#txtPatientSearch').focus();
            },
            focus: function () {
                return false;
            }
        }).on("paste", function (e) {
            intPatientlen = -1;
            arrPatient = [];
            $(".ui-autocomplete").hide();
        }).on("input", function (e) {
            $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
            if ($("#txtPatientSearch").val().charAt(e.currentTarget.value.length - 1) == " ") {
                if (e.currentTarget.value.split(" ").length > 2)
                    intPatientlen = intPatientlen + 1;
                else
                    intPatientlen = 0;
            }
            else {
                if ($("#txtPatientSearch").val().length != 0 && intPatientlen != -1) {
                    intPatientlen = intPatientlen + 1;
                }

                if ($("#txtPatientSearch").val().length == 0 || $("#txtPatientSearch").val().indexOf(" ") == -1) {
                    intPatientlen = -1;
                    arrPatient = [];
                    $(".ui-autocomplete").hide();
                }
            }
        })

        $("#txtPatientSearch").data("ui-autocomplete")._renderItem = function (ul, item) {
            if (item.label != "No matches found.") {
                var HumanDetails = $.parseJSON(item.val);
                var list_item = $("<li>")
                  .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                  .append(item.label)
                  .appendTo(ul);
                if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                    list_item.addClass("inactive");
                if (HumanDetails.Status.toUpperCase() == "DECEASED")
                    list_item.addClass("deceased");
                return list_item;
            }
            else
                return $("<li>")
                  .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                  .addClass("disabled")
                  .append(item.label)
                  .appendTo(ul).on("click", function (e) {
                      e.preventDefault();
                      e.stopImmediatePropagation();
                  });
        };
    }
});
function PatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtPatientSearch = document.getElementById("txtPatientSearch");

    var WSData = {
        HumanID: ui.item.value
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "WebServices/ReportService.asmx/GetHumanDetails",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtPatientSearch = document.getElementById('txtPatientSearch');
            txtPatientSearch.value = SelectedPatient.DisplayString;
            txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
        }
    });
    txtPatientSearch.value = ui.item.label;
    txtPatientSearch.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    return false;
}
function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}
function chkSelectAll_Click() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#chkSelectAll")[0].checked == true) {
        $("#sltPhysician option").remove();
        $("#sltPhysician").append('<option value="' + '0' + '">' + "--Select Physician--" + '</option>');
        for (i = 0; i < AllPhysicianList.length; i++) {
            var arrName = AllPhysicianList[i].PhyPrefix + " " + AllPhysicianList[i].PhyFirstName + " " + AllPhysicianList[i].PhyMiddleName + " " + AllPhysicianList[i].PhyLastName + " " + AllPhysicianList[i].PhySuffix;
            var arrID = AllPhysicianList[i].Id;
            $("#sltPhysician").append('<option value="' + arrID + '">' + arrName + '</option>');
        }
        if (UserRole == "Physician") {
            $('#sltPhysician option[value="' + UserID + '"]').prop("selected", true);
        }
    }
    else {

        $("#sltPhysician option").remove();
        $("#sltPhysician").append('<option value="' + '0' + '">' + "--Select Physician--" + '</option>');
        for (i = 0; i < PhysicianList.length; i++) {
            var arrName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            var arrID = PhysicianList[i].Id;
            $("#sltPhysician").append('<option value="' + arrID + '">' + arrName + '</option>');
        }
        if (UserRole == "Physician") {
            $('#sltPhysician option[value="' + UserID + '"]').prop("selected", true);
        }
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function Facility() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    var facil = $("#sltFacility option:selected").val();
    if (facil != "0") {
        $("#chkSelectAll")[0].checked = false;
        $.ajax({
            type: "POST",
            url: "WebServices/ReportService.asmx/PhysicaianNameList",
            data: JSON.stringify({
                "sFacilityName": facil,
            }),
            contentType: "application/json;charset=utf-8",
            datatype: "json",
            success: function success(data) {
                
                var ReportLoadList = JSON.parse(data.d);

                PhysicianList1 = ReportLoadList.PhysicianList;
                PhysicianList = ReportLoadList.PhysicianList;

                $("#sltPhysician option").remove();
                $("#sltPhysician").append('<option value="' + '0' + '">' + "--Select Physician --" + '</option>');
                for (i = 0; i < PhysicianList1.length; i++) {
                    var arrPhyName = PhysicianList1[i].PhyPrefix + " " + PhysicianList1[i].PhyFirstName + " " + PhysicianList1[i].PhyMiddleName + " " + PhysicianList1[i].PhyLastName + " " + PhysicianList1[i].PhySuffix;
                    var arrPhyID = PhysicianList1[i].Id;
                    $("#sltPhysician").append('<option value="' + arrPhyID + '">' + arrPhyName + '</option>');
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function onerror(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        $("#sltPhysician option").remove();
        $("#sltPhysician").append('<option value="' + '0' + '">' + "--Select Physician--" + '</option>');
        for (i = 0; i < PhysicianList.length; i++) {
            var arrName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
            var arrID = PhysicianList[i].Id;
            $("#sltPhysician").append('<option value="' + arrID + '">' + arrName + '</option>');
        }
        if (UserRole == "Physician") {
            $('#sltPhysician option[value="' + UserID + '"]').prop("selected", true);
        }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}
function btnClearAll_Click() {

    $('#dtpFromDate')[0].value = date;
    $('#dtpToDate')[0].value = date;

    $('#sltFacility option[value="0"]').prop("selected", true);

    $("#sltPhysician option").remove();
    $("#sltPhysician").append('<option value="' + '0' + '">' + "--Select Physician--" + '</option>');
    for (i = 0; i < PhysicianList.length; i++) {
        var arrName = PhysicianList[i].PhyPrefix + " " + PhysicianList[i].PhyFirstName + " " + PhysicianList[i].PhyMiddleName + " " + PhysicianList[i].PhyLastName + " " + PhysicianList[i].PhySuffix;
        var arrID = PhysicianList[i].Id;
        $("#sltPhysician").append('<option value="' + arrID + '">' + arrName + '</option>');
    }
    $("#chkSelectAll")[0].checked = false;

    $('#sltCurrentProcess option[value="0"]').prop("selected", true);

    $('#sltCurrentOwner option[value="0"]').prop("selected", true);

    $('#sltReportFor option[value="0"]').prop("selected", true);
    $("#chkIncludeVisit")[0].checked = false;
    $("#chkShowActive")[0].checked = false;
    $('#txtPatientSearch').val('');
    $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
    $("#iReportFrame").attr("src", "about:blank");
}
function LogTimeString(time_string) {
    UI_Time_Stop = new Date();

    var WS_Time = parseFloat(time_string.split(';')[0].split(':')[1].replace('s', ''));
    var DB_Time = parseFloat(time_string.split(';')[1].split(':')[1].replace('s', ''));
    var UI_Time = ((UI_Time_Stop.getTime() - UI_Time_Start.getTime()) / 1000) - WS_Time - DB_Time;
    console.log(time_string + " UI_Time :" + UI_Time + "s; Total_Time :" + (WS_Time + DB_Time + UI_Time).toString() + "s;");

}