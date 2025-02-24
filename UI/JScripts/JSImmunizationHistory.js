
function btnClearAll_Clicked(sender, args) {

    var ClearAll = null;

    if (sender == undefined || sender._text.trim() == "Clear All") {
        ClearAll = DisplayErrorMessage('295002');
    }
    else {

        ClearAll = DisplayErrorMessage('180049');
    }

    if (ClearAll == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        document.getElementById("Hiddenupdate").value = "ADD";
        $find("txtImmunizationProcedure").clear();
        $find("txtCVXcode").clear();
        $find("txtLotNumber").clear();
        $find("cboManufacturer").clearSelection();
        $find("cboRouteOfAdminstration").clearSelection();
        $find("cboImmunizationSource").clearSelection();
        $find("txtAdminAmt").clear();
        $find("cboLocation").clearSelection();
        $find("cboDosetotal").clearSelection();
        $find("cboProtectionState").clearSelection();
        $find("txtDose").clear();
        document.getElementById("DLC_txtDLC").value = "";
        
        $find("chklstImmunizationHistory").clearSelection();
        $find("dpExpiryDate").clear();
        document.getElementById("chkVisgiven").checked = false;
        $find("dpVisgiven").set_enabled(false);
        var combo = $find("cdtAdministeredDate_cboYear");
        combo.clearSelection();
        var combo = $find("cdtAdministeredDate_cboMonth");
        combo.clearSelection();
        var combo = $find("cdtAdministeredDate_cboDate");
        combo.clearSelection();
        $find("dpDateOnVIS").set_enabled(false);
        $find("dpVisgiven").clear();
        $find("dpDateOnVIS").clear();
        $find("btnSave")._text = "Add";
        $find("btnSave").set_text("Add");
        $find("btnSave").set_enabled(false);
        document.getElementById("HiddenField2").value = "false";
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;

        document.getElementById('InvisibleClearAllButton').click();
    }
    else {
        return false;
    }
}

function EnableSave() {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}

function GridEditAutoSaveEnable() {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)

        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;


}
function clickManageFrequentlyUser() {
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false });
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "85%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "73%";
    $(top.window.document).find("#TabModalTitle")[0].textContent = "Manage Frequently Used Procedures";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmLabProcedureManage.aspx?procedureType=" + "IMMUNIZATION PROCEDURE" + "&IsImmunizationHistory=Y";
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        OnClientCloseLabprocedure();
    });
}
function EnablePFSH(val) {
    if ($(window.parent.document).find('#btnPFSHVerified') != null)
        $(window.parent.document).find('#btnPFSHVerified')[0].disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    var bValue = true;
    var PFSHVerified = localStorage.getItem("PFSHVerified");
    if (PFSHVerified != "") {
        var PFSH = PFSHVerified.split('|');
        for (var i = 0; i < PFSH.length; i++) {
            if (PFSH[i].split('-')[0] == val) {
                PFSHVerified = PFSHVerified.replace(PFSH[i], val + "-" + "TRUE");
                bValue = false;
            }
        }
    }
    if (bValue == true)
        PFSHVerified = PFSHVerified + "|" + val + "-" + "TRUE";
    localStorage.setItem("PFSHVerified", PFSHVerified);
}
function OnClientCloseLabprocedure(oWindow, args) {
   
    document.getElementById("InvisibleButton").click();
}
function keyValues() {
    if ($find("DLC_txtDLC") != null) {
        if ($find("DLC_txtDLC")._element.value.length >= 32767) {
            return false;
        }
    }
}



function chklstImmunizationHistory_SelectedIndexChanged(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $find('txtImmunizationProcedure').set_value("");
    $find('txtImmunizationProcedure').set_value(sender._element.value);
    document.getElementById("HiddenField2").value = "true";
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    $find('btnSave').set_enabled(true);
    //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}


function saveEnabled() {
   
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
}


function cancelBack() {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}

function ShowLoading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}
function ImmuHist_Load() {
    
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $('#chklstImmunizationHistory').find('li').addClass('Editabletxtbox');

    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}

function cboManufacturer_SelectedIndexChanged(sender, args) {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}
function DeleteSuccessfully() {
    RefreshNotification('ImmunizationHistory');
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
    //DisplayErrorMessage('295001');
}
function SavedSuccessfully() {
    RefreshNotification('ImmunizationHistory');
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
    DisplayErrorMessage('295001');
    //CAP-2678
    localStorage.setItem("IsSaveCompleted", true);
}
function ShowLoadingWaitCursor() {
    EnableSave();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    return true;

}
function grdImmunization_OnCommand(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function AllowNumbers(sender, args)
{
    EnableSave();
    var keyCharCode = args.get_domEvent().rawEvent.keyCode;

    if (keyCharCode == sender.get_numberFormat().NegativeSign.charCodeAt())
    {
        args.set_cancel(true);
    }

}