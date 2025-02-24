function lstReasonForHospitalization_SelectedIndexChanged(sender, args) {
    var check = false;
    var txtValue = args.get_item()._element.innerText;

    var txtControl = document.getElementById("txtReasonForHospitalization");
    if (txtControl.value == "") {
        $find("txtReasonForHospitalization").set_value(txtValue);
    }
    else {
        var value = $find("txtReasonForHospitalization")._value.split(', ');
        var index = value.indexOf(txtValue.trim());
        if (index >= 0) {
            check = true;
        }

        if (!check)
            $find("txtReasonForHospitalization").set_value(txtControl.value + ", " + txtValue);//Added space after comma for better readability .BugID:37929

    }
    document.getElementById("Hidden1").value = "True";

    $find('btnAdd').set_enabled(true);

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");

    var input = $find("txtReasonForHospitalization");
    input.focus();

    if (input._text.length > 0)
        input.set_caretPosition(input._text.length);
}


function TextBoxClear() {
    $find("txtReasonForHospitalization").clear();

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;

    $find("txtReasonForHospitalization").focus();

    return false;
}

function CurrentCheckBox() {
    if (document.getElementById("chkCurrentDate").checked == true) {
        dateDisable("dtpToDate");
        document.getElementById("ddlReadmitted").disabled = true;
        document.getElementById("ddlReadmitted").value = '';
        dateDisable("dtpReadmissionDate");
     
      }
    else {

        var combo = $find("dtpToDate_cboYear");
        combo.enable();
        combo.clearSelection();

        var combo = $find("dtpToDate_cboMonth");
        combo.enable();
        combo.clearSelection();

        var combo = $find("dtpToDate_cboDate");
        combo.enable();
        combo.clearSelection();


        if (document.getElementById("chkCurrentDate").checked == true || document.getElementById("ddlReadmitted").value == "Yes" ) {
            var combo = $find("dtpReadmissionDate_cboYear");
            combo.enable();
            combo.clearSelection();

            var combo = $find("dtpReadmissionDate_cboMonth");
            combo.enable();
            combo.clearSelection();

            var combo = $find("dtpReadmissionDate_cboDate");
            combo.enable();
            combo.clearSelection();
        }



        var calTodate = $find("dtpToDate_clbCalendar");
        if (calTodate != null) 
            calTodate.set_enabled(true);
        

        var calReadmitted = $find("ddlReadmitted_clbCalendar");
        if (calReadmitted != null)
            calReadmitted.set_enabled(true);

        document.getElementById("dtpToDate_clbCalendar_FooterTemplateContainer_Button1").disabled = false;
        document.getElementById("dtpToDate_RadButton1").disabled = false;
        document.getElementById("dtpToDate_RadButton1").src = "Resources/calenda2.bmp";
        document.getElementById("ddlReadmitted").disabled = false;          
        document.getElementById("dtpReadmissionDate_clbCalendar_FooterTemplateContainer_Button1").disabled = true;
        document.getElementById("dtpReadmissionDate_RadButton1").disabled = true;
        document.getElementById("dtpReadmissionDate_RadButton1").src = "Resources/calenda2.bmp"; 
       
       
    }
    $find('btnAdd').set_enabled(true);

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");

}

function btnClearAll_Clicked(sender, args) {
    var IsClearAll = null;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (sender==undefined|| sender._text.trim() == "Clear All") {
        IsClearAll = DisplayErrorMessage('180010');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
    else {
        IsClearAll = DisplayErrorMessage('180049');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
    if (IsClearAll != undefined) {
        if (IsClearAll == true) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            else
                window.parent.theForm.hdnSaveEnable.value = false;
            document.getElementById("Hiddenupdate").value = "ADD";
            $find("lstReasonForHospitalization").clearSelection();
            document.getElementById('InvisibleClearAllButton').click();
        }
        else {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            $find("btnAdd").set_enabled(true);

            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            else
                window.parent.theForm.hdnSaveEnable.value = true;
            localStorage.setItem("bSave", "false");
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
    }
}

function dateDisable(Value) {
    var dateValue = Value;
    var combo = $find(dateValue + "_cboYear");
    combo.disable();
    combo.clearSelection();
    var combo = $find(dateValue + "_cboMonth");
    combo.disable();
    combo.clearSelection();
    var combo = $find(dateValue + "_cboDate");
    combo.disable();
    combo.clearSelection();
   

    var cal = $find(dateValue + "_clbCalendar");
    if (cal != null)
        cal.set_enabled(false);
    
    document.getElementById(dateValue +"_clbCalendar_FooterTemplateContainer_Button1").disabled = true;
    document.getElementById(dateValue +"_RadButton1").disabled = true;
    document.getElementById(dateValue + "_RadButton1").src = "Resources/calenda2_Disabled.bmp";
    document.getElementById(dateValue +"_")

}

function pbLibrary_Click() {
    var obj = new Array();
    obj.push("FieldName=Reason For Hospitalization");
    openModal("frmAddorUpdateKeywords.aspx", 500, 630, obj);
    return false;


}

function openAddorUpdate() {
   
    var fieldName = "Reason For Hospitalization";
    $(top.window.document).find("#Modal").modal({ backdrop: "static" });
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + fieldName;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";


    $(top.window.document).find("#Modal").on('hidden.bs.modal', function () {
        
        if (sessionStorage.getItem("Updated_Surgery_List") != undefined) {
            var lstBox = $('#lstReasonForHospitalization')[0].control;
            var lstSurgery = JSON.parse(sessionStorage.getItem("Updated_Surgery_List"));
           
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
    $find("txtReasonForHospitalization").clear();
    $find("DLC_txtDLC").clear();
    document.getElementById("chkCurrentDate").checked = false;
    document.getElementById("dtpFromDate" + "_RadButton1").disabled = true;
    dateDisable("dtpFromDate");
    dateDisable("dtpToDate");
    $find("btnAdd")._text = "Add";
    $find("btnAdd")._value = "Add";
    $find("btnAdd").set_text("Add");
    $find("btnClearAll")._text = "Clear All";
    $find("btnClearAll")._value = "Clear All";
}

function CellSelected(value) {
    if (DisplayErrorMessage('180016') == true) {
        document.getElementById('hdnDelHospitalizationId').value = value;
        document.getElementById('InvisibleButton').click();
    }

}
function EnableSave(e) {

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

function btnAdd_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    $find("lstReasonForHospitalization").clearSelection();
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
   
}

function Enable_OR_Disable() {
    if ($find("btnAdd").get_enabled())
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";


}
function CCTextChanged() {

    $find("btnAdd").set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {

        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    else {

        window.parent.theForm.hdnSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    document.getElementById("Hidden1").value = "True";
    return false;
}

function CheckedChange(ID) {
    if (ID.checked) {
        document.getElementById('pbClear').src = "Resources/close_disabled.png";
        document.getElementById('pbLibrary').src = "Resources/Database Disable.png";
    }
}

function DateSelecting() {
    
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        if (document.getElementById("chkPatientDeniesHospitalization") != null) {
            if (!document.getElementById("chkPatientDeniesHospitalization").checked)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            else
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        }
    }
    else {

        window.parent.theForm.hdnSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    document.getElementById("Hidden1").value = "True";
    if (document.getElementById("chkCurrentDate").checked == true) {
        document.getElementById("ddlReadmitted").disabled = true;
        document.getElementById("ddlReadmitted").value = '';

    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}

}

function LaodWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function EndWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';

}

function OnClientclose() {

    document.getElementById('LibraryIconButton').click();

}

function ReloadOnClientClose(oWindow, args) {


    document.getElementById('LibraryIconButton').click();
}

function txtReasonForHospitalization_OnKeyPress(sender, args) {

    $find('btnAdd').set_enabled(true);
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {

        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    else {

        window.parent.theForm.hdnSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
}


function GridEditAutoSaveEnable() {
    $('#lstReasonForHospitalization').find('li').addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").addClass('Editabletxtbox');
    document.getElementById("Hiddenupdate").value = "UPDATE";
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
}
function HistoryHosp_Load() {
    
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnAdd,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $('#lstReasonForHospitalization').find('li').addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").addClass('Editabletxtbox');
    ;

}

function LoadingSymbol() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}

function clbCalendar_OnDateSelecting(sender, args) {
    $find('btnAdd').set_enabled(true);
    localStorage.setItem("bSave", "false");
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    //Add JavaScript handler code here
}
function grdHospitalizationHistory_OnCommand(sender, args) {
    //Add JavaScript handler code here
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
    PFSH_AfterAutoSave();
    $('#lstReasonForHospitalization').find('li').addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").addClass('Editabletxtbox');
    //CAP-2678
    localStorage.setItem("IsSaveCompleted", true);
}

function ChangeReadmitted() {
    if (document.getElementById("ddlReadmitted").value == "No" || document.getElementById("ddlReadmitted").value == "")
        dateDisable("dtpReadmissionDate");
    else {

        var combo = $find("dtpReadmissionDate_cboYear");
        combo.enable();
        combo.clearSelection();

        var combo = $find("dtpReadmissionDate_cboMonth");
        combo.enable();
        combo.clearSelection();

        var combo = $find("dtpReadmissionDate_cboDate");
        combo.enable();
        combo.clearSelection();

        var cal = $find("dtpReadmissionDate_clbCalendar");
        cal.set_enabled(true);
        document.getElementById("dtpReadmissionDate_clbCalendar_FooterTemplateContainer_Button1").disabled = false;
        document.getElementById("dtpReadmissionDate_RadButton1").disabled = false;
        document.getElementById("dtpReadmissionDate_RadButton1").src = "Resources/calenda2.bmp";
    }
    $find('btnAdd').set_enabled(true);

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");

}