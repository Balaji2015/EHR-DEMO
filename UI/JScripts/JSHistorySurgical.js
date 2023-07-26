function lstSurgeryName_SelectedIndexChanged(sender, args) {
    var txtValue = args.get_item()._element.innerText;
    $find("txtSurgeryName").set_value(txtValue);
    $find('btnAdd').set_enabled(true);
    document.getElementById("Hidden1").value = "True";

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}
function btnClearAll_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var IsClearAll = null;

    if (sender == undefined || sender._text.trim() == "Clear All") {

        var iSender = localStorage.getItem("Events");
        sender = iSender;
        IsClearAll = DisplayErrorMessage('180010');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
    else {
        IsClearAll = DisplayErrorMessage('180049');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
    if (IsClearAll != undefined) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        if (IsClearAll == true) {
            document.getElementById("Hiddenupdate").value = "ADD";
            ClearAll();
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            else
                window.parent.theForm.hdnSaveEnable.value = false;
        }
        else {
            $find("btnAdd").set_enabled(true);
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            else
                window.parent.theForm.hdnSaveEnable.value = true;
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
    }
}

function openAddorUpdate() {
    $(top.window.document).find("#Modal").modal({ backdrop: "static" });
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=Surgery Name";
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
    $(top.window.document).find("#Modal").on('hidden.bs.modal', function () {
        
        if (sessionStorage.getItem("Updated_Surgery_List") != undefined) {
            var lstBox = $('#lstSurgeryName')[0].control;
            var lstSurgery = JSON.parse(sessionStorage.getItem("Updated_Surgery_List"));
            var newItem = localStorage.getItem("New_Surgery_Item");
            lstBox.trackChanges();
            lstBox.get_items().clear();
            for (var i = 0; i < lstSurgery.length; i++) {
                var item = new Telerik.Web.UI.RadListBoxItem();
                item.set_text(lstSurgery[i]);
                lstBox.get_items().add(item);
            }
            lstBox.commitChanges();
            sessionStorage.removeItem("Updated_Surgery_List");
        }
    })
    
}

function ClearAll() {
   
    $("#btnAdd_SpanAdd").html("A");
    $("#btnAdd_SpanAdditionalword").html("dd");
    $("#btnClearAll_SpanClearAdditional").html("lear All");
    $find("txtSurgeryName").clear();
    document.getElementById("DLC_txtDLC").value = "";
    document.getElementById("dtpDateOfSurgery_RadButton1").disabled = false;
    var combo = $find("dtpDateOfSurgery_cboYear");
    combo.enable();
    combo.clearSelection();
    var combo = $find("dtpDateOfSurgery_cboMonth");
    combo.enable();
    combo.clearSelection();
    var combo = $find("dtpDateOfSurgery_cboDate");
    combo.enable();
    combo.clearSelection();
    $find('btnAdd').set_enabled(false);
    document.getElementById("btnAdd").disabled = false;
    $find("lstSurgeryName").clearSelection();
    $('select').css('display', 'none');
    $('.fa').removeClass('fa-minus').addClass('fa-plus');
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}

function CellSelected(value) {
    if (DisplayErrorMessage('180016') == true) {
        document.getElementById('hdnDelSurgicalId').value = value;
        document.getElementById('InvisibleButton').click();
    }
}

function EnableSave(e) {
    if ($find('btnAdd') != null && $find('btnAdd')!=undefined)
        $find('btnAdd').set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
}

function EnablePFSH(val) {
    document.getElementById("Hiddenupdate").value = "ADD";
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

function Enable_OR_Disable() {
    if ($find("btnAdd").get_enabled()) {
        document.getElementById("Hidden1").value = "True";
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    else
        document.getElementById("Hidden1").value = "";
}

function CCTextChanged() {
    if ($find("btnAdd") != null)
        $find("btnAdd").set_enabled(true);
    localStorage.setItem("bSave", "false");
    document.getElementById("Hidden1").value = "True";

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    return false;
}

function DateSelecting() {
    $find("btnAdd").set_enabled(true);
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function LaodWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function EndWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}

function SurgicalSave(sender, args) {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    if ($('#txtSurgeryName').val().trim() == "") {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        DisplayErrorMessage('180018');
        sender.set_autoPostBack(false);
        PFSH_SaveUnsuccessful();
    }
    else if ($('#dtpDateOfSurgery_cboDate').val().trim() != "" && $('#dtpDateOfSurgery_cboMonth').val().trim() == "" && $('#dtpDateOfSurgery_cboYear').val().trim() != "") {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        DisplayErrorMessage('180611');
        sender.set_autoPostBack(false);
        PFSH_SaveUnsuccessful();
    }
    else {
        //CAP-536 PFSH - Surgical History screen loading for long time
        sender.set_autoPostBack(false);
        __doPostBack('btnAdd', "true");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        $find("lstSurgeryName").clearSelection();
        $('#lstSurgeryName').find('li').addClass('ChangeLabel')
    }
}
function txtSurgeryName_OnKeyPress(sender, args) {
    $find('btnAdd').set_enabled(true);
    localStorage.setItem("bSave", "false");
}

function keyDownEvent(e) {
    if (e.onkeydown.arguments[0].keyCode == 17) {
        if (e.id = "txtdiv")
            $find('btnAdd').set_enabled(true);
        else if (e.id = "dlcDiv")
            $find('DLC').set_enabled(true);

        localStorage.setItem("bSave", "false");
        e.SuppressKeyPress = false;
    }
    else
        e.SuppressKeyPress = true;
}

function grdSurgeryHistoryDetails_OnCommand(sender, args) {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}

function ReloadOnClientClose(oWindow, args) {
    document.getElementById('LibraryButton').click();
}

function GridEditAutoSaveEnable() {
    $('#lstSurgeryName').find('li').addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").addClass('Editabletxtbox');
    document.getElementById("Hiddenupdate").value = "UPDATE";

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function HistorySurgical_Load() {
    

    if (window.parent.parent.theForm.hdnSaveButtonID != undefined)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnAdd,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $('#lstSurgeryName').find('li').addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").addClass('Editabletxtbox');
}

function grdSurgeryHistoryDetails_OnCommand(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
}

function SavedSuccessfully() {
    localStorage.setItem("bSave", "true");
   
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    DisplayErrorMessage('180015');
    PFSH_AfterAutoSave();
    $('#lstSurgeryName').find('li').addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
}