$(document).ready(function () {
    $(".loaderClass").click(function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    });
});

﻿
function OpenFindPhysicianOld() {
    var oWnd = GetRadWindow();
    var oBrowserWnd = GetRadWindow().BrowserWindow;
    var oWin = oBrowserWnd.radopen("frmFindReferralPhysician.aspx", "OrderManagementWindow");
    setTimeout(
        function () {
            oWin.remove_close(OnClientCloseFindProvider);
            oWin.SetModal(true);
            oWin.set_visibleStatusbar(false);
            oWin.setSize(930, 256);
            oWin.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
            oWin.set_iconUrl("Resources/16_16.ico");
            oWin.set_keepInScreenBounds(true);
            oWin.set_centerIfModal(true);
            oWin.center();
            oWin.set_reloadOnShow(true);
            oWin.remove_close(OnClientCloseFindPatient);
            oWin.add_close(OnClientCloseFindProvider);
        }, 0);
    return false;
}
function OpenFindPhysician() {
    setTimeout(function () {
        var oWnd = GetRadWindow();
        var childWindow = oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx", "OrderManagementWindow");
        setRadWindowProperties(childWindow, 256, 930);
        childWindow.remove_close(OnClientCloseFindPatient)
        childWindow.add_close(OnClientCloseFindProvider);
    }, 0);
    return false;
}
function FindPatient() {
    setTimeout(function () {
      var oWnd = GetRadWindow();
      var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx", "OrderManagementWindow");
      setRadWindowProperties(childWindow, 251, 1200);
      childWindow.add_close(OnClientCloseFindPatient)
      childWindow.remove_close(OnClientCloseFindProvider);
  }, 0);
    return false;
}


function FindPatientOld() {
    var oBrowserWnd = GetRadWindow().BrowserWindow;
    var childWindow = oBrowserWnd.radopen("frmFindPatient.aspx", "OrderManagementWindow");
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
            childWindow.set_reloadOnShow(true);
            childWindow.center();
            childWindow.remove_close(OnClientCloseFindProvider);
            childWindow.add_close(OnClientCloseFindPatient);
        }, 0);
    return false;
}


function btnPrintToPDF_Click(sender, args) {
    var obj = new Array();
    obj.push("Location=" + "DYNAMIC");
    var result = openModal("frmPrintPDF.aspx", 750, 900, obj, "OrderManagementWindow");
}

function ExamPhotos(Submit_Id, CurrentProcess) {

    var obj = new Array();
    obj.push("type=Result Upload");
    obj.push("CurrentProcess=" + CurrentProcess);
    obj.push("OrderSubmit_ID=" + Submit_Id);
    obj.push("IsCmg=IsCmg");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmExamPhotos.aspx', 400, 850, obj, "OrderManagementWindow"); }, 0);
}

function ViewResults(Human_ID, order_submit_id, enc_id, Physician_ID, lab_id) {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var vhdnCurrentProcess = "";
    if (document.getElementById("hdnCurrentProcess") != null) {
        vhdnCurrentProcess = document.getElementById("hdnCurrentProcess").value;
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
    $(top.window.document).find("#TabViewResultdlg")[0].style.width = "90%";
    $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
    $(top.window.document).find("#TabViewResultdlg").css("margin-top", "1%");
    var sPath = "frmViewResult.aspx?HumanID=" + Human_ID + "&OrderSubmitId=" + order_submit_id + "&EncounterId=" + enc_id + "&PhysicianId=" + Physician_ID + "&LabId=" + lab_id + "&Screen=OrderManagement" + "&CurrentProcess=" + vhdnCurrentProcess + "&Opening_from=OrderManagementScreen&File_Ref_ID=" + order_submit_id;//BugID:43099
    $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
    $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
    });
    return false;
}

function RadWindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}

function Clear() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        var FromDate = $find("dtpFromDate");
        var ToDate = $find("dtpToDate");
        FromDate.clear();
        ToDate.clear();
        document.getElementById("btnClear").click();


    }
    else if (IsClearAll == false) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

}
function Load() {
    GetUTCTime();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

}
function PatientClear() {
    document.getElementById("txtPatientName").value = "";
    document.getElementById("txtPatientName").removeAttribute("tagPatientName");
    return false;
}
function ProviderClear() {
    document.getElementById("txtProviderName").value = '';
    return false;
}
function OnClientCloseFindProvider(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined) {
        document.getElementById("txtProviderName").value = arg.sPhyName;
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
function OnRowSelected(sender, eventArgs) {
    var btnAdd = document.getElementById("btnAddResult");
    var rowindex = eventArgs.get_itemIndexHierarchical();
    var value16 = eventArgs.get_gridDataItem()._element.cells[14].innerHTML;
    var value5 = eventArgs.get_gridDataItem()._element.cells[3].innerHTML;
    if (eventArgs.get_gridDataItem()._element.cells[14].innerHTML != '' && eventArgs.get_gridDataItem()._element.cells[14].innerHTML == '32' && eventArgs.get_gridDataItem()._element.cells[3].innerHTML != "BILLING_WAIT" && eventArgs.get_gridDataItem()._element.cells[3].innerHTML != "RESULT_REVIEW") {
        btnAdd.disabled = true;
    }
    else {
        btnAdd.disabled = false;
    }
}
function CheckedChange() {
    var Chk = document.getElementById("chkDate");
    if (Chk.checked == true) {
        var FromDate = $find("dtpFromDate");
        var ToDate = $find("dtpToDate");
        var Currentdate = new Date();
        var sval = Currentdate.format("dd-MMM-yyyy");

        FromDate.get_dateInput().set_value(sval);
        ToDate.get_dateInput().set_value(sval);

        FromDate.set_enabled(true);
        ToDate.set_enabled(true);
    }
    else {
        var FromDate = $find("dtpFromDate");
        var ToDate = $find("dtpToDate");
        FromDate.clear();
        ToDate.clear();
        FromDate._dateInput._displayText = null;
        ToDate._dateInput._displayText = null;
        FromDate.set_enabled(false);
        ToDate.set_enabled(false);
    }
}
function OpenPDFImage() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var obj = new Array();
    obj.push("SI=" + document.getElementById('hdnSelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 720, 900, obj, "OrderManagementWindow"); }, 0);
}
function OpenPDF() {
    var obj = new Array();
    obj.push("SI=" + document.getElementById('SelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 720, 900, obj, "OrderManagementWindow"); }, 0);
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function btnExcelClick() {
    GetUTCTime();
    var grid = $find("grdReport");
    var MasterTable = grid.get_masterTableView();
    var Rows = MasterTable.get_dataItems();
    if (Rows.length == 0) {
        DisplayErrorMessage('7090007');
        return false;
    }
    else {
        return true;
    }
}

function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}

function grdReport_OnRowClick(sender, args) {

    var lblResult = document.getElementById('lblResultsFound');
    document.getElementById("hdnResultsLabel").value = lblResult.textContent;
}
function CloseImportResult() {
    document.getElementById("btnSearch").click();
}
function OrderMgmtLoad() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}


function cboOrderType_SelectedIndexChanged(sender, args) {
    //Add JavaScript handler code here

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function LoadIcon() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
