var PastMedicalList;
var GeneralNotesList;
function AllOthersYesOrNo(ChkValue) {
    var pcontrol = document.getElementById(ChkValue);
    var dcontrol = document.getElementById("tblTest");
    if (pcontrol.name.indexOf("Yes") != -1) {
        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
        EnableSave('test');
        for (i = 0; i < dcontrol.rows.length; i++) {
            if (dcontrol.rows[i].cells.length > 1) {
                if (pcontrol.checked == true) {
                    var NoChkBox = document.getElementById("chkAllNo");
                    NoChkBox.checked = false;
                    if (dcontrol.rows[i].cells[4].children[0].checked == false) {
                        dcontrol.rows[i].cells[2].children[0].checked = true;
                        MaskDateEnable(dcontrol.rows[i].cells[2].children[0], "chkYes", "txtFromDate");
                        MaskDateEnable(dcontrol.rows[i].cells[2].children[0], "chkYes", "txtToDate");
                        dcontrol.rows[i].cells[9].children[0].disabled = false;
                        var valu = dcontrol.rows[i].cells[2].children[0];
                        var CCurrentControl = valu.id.replace("chkYes", "chkCurrent");

                        if (document.getElementById(CCurrentControl).checked) {
                            document.getElementById(CCurrentControl).checked = true;
                            MaskDateDisable(dcontrol.rows[i].cells[2].children[0], "chkYes", "txtToDate");
                        }
                        else
                            document.getElementById(CCurrentControl).checked = false;

                        var a = document.getElementById(CCurrentControl);
                        a.disabled = false;
                        if (a.parentElement.tagName == 'SPAN' && a.parentElement.disabled == true)
                            a.parentElement.disabled = false;

                        var DLCcontrol = dcontrol.rows[i].cells[2].children[0];

                        var TControl = DLCcontrol.id.replace("chkYes", "DLCHistoryProblem");

                        //changes  
                        var DControl = DLCcontrol.id.replace("chkYes", "DLCHistoryProblem");
                        document.getElementById(DControl + "_pbDropdown").disabled = false;
                        document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: col-6-btn margintop5px;");

                        var CControl = DLCcontrol.id.replace("chkYes", "DLCHistoryProblem");
                        document.getElementById(CControl + "_pbClear").disabled = false;
                        document.getElementById(CControl + "_pbClear").setAttribute("style", "background-color: col-6-btn margintop5px;");


                        var TControl = DLCcontrol.id.replace("chkYes", "DLCHistoryProblem");
                        document.getElementById(TControl + "_txtDLC").disabled = false;

                        if (document.getElementById("hdnLibraryIcon").value == "Physician") {
                            var CControl = DLCcontrol.id.replace("chkYes", "DLCHistoryProblem");
                            document.getElementById(CControl + "_pbLibrary").disabled = false;
                            document.getElementById(CControl + "_pbLibrary").setAttribute("style", "background-color: col-6-btn margintop5px;");
                        }
                        //changes
                    }

                } else {
                    if (dcontrol.rows[i].cells[4].children[0].checked == false) {
                        dcontrol.rows[i].cells[2].children[0].checked = false;
                        MaskDateDisable(dcontrol.rows[i].cells[2].children[0], "chkYes", "txtFromDate");
                        MaskDateDisable(dcontrol.rows[i].cells[2].children[0], "chkYes", "txtToDate");
                        dcontrol.rows[i].cells[9].children[0].disabled = true;

                        dcontrol.rows[i].cells[9].children[0].checked = false;
                        var valu = dcontrol.rows[i].cells[2].children[0];
                        var CCurrentControl = valu.id.replace("chkYes", "chkCurrent");

                        document.getElementById(CCurrentControl).checked = false;
                        var a = document.getElementById(CCurrentControl);
                        a.disabled = true;
                        if (a.parentElement.tagName == 'SPAN' && a.parentElement.disabled == true)
                            a.parentElement.disabled = true;

                        var DLCcontrol = dcontrol.rows[i].cells[2].children[0];
                        var TControl = DLCcontrol.id.replace("chkYes", "DLCHistoryProblem");
                        document.getElementById(TControl + "_txtDLC").disabled = true;
                        document.getElementById(TControl + "_txtDLC").value = "";
                        document.getElementById(TControl + "_pbDropdown").disabled = true;
                        document.getElementById(TControl + "_pbClear").disabled = true;
                        document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: #808080;");
                        document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color: #808080;");
                        document.getElementById(TControl + "_pbLibrary").disabled = true;
                        document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: #808080;");
                    }

                }
            }
        }
    } else {

        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
        EnableSave('test');
        for (i = 0; i < dcontrol.rows.length; i++) {
            if (dcontrol.rows[i].cells.length > 1) {
                if (pcontrol.checked == true) {
                    var YesChkBox = document.getElementById("chkAllYes");
                    YesChkBox.checked = false;
                    if (dcontrol.rows[i].cells[2].children[0].checked == false) {
                        dcontrol.rows[i].cells[4].children[0].checked = true;

                        dcontrol.rows[i].cells[9].children[0].checked = false;
                        dcontrol.rows[i].cells[9].children[0].disabled = false;
                        var CCurrentControl = dcontrol.rows[i].cells[9].children[0];
                        CCurrentControl.disabled = true;
                        if (CCurrentControl.parentElement.tagName == 'SPAN' && CCurrentControl.parentElement.disabled == false)
                            CCurrentControl.parentElement.disabled = true;

                        var DLCcontrol = dcontrol.rows[i].cells[4].children[0];
                        var TControl = DLCcontrol.id.replace("chkNo", "DLCHistoryProblem");
                        document.getElementById(TControl + "_txtDLC").disabled = false;
                        document.getElementById(TControl + "_pbClear").disabled = false;
                        document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: col-6-btn margintop5px;");
                        document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color: col-6-btn margintop5px;");
                        if (document.getElementById("hdnLibraryIcon").value == "Physician") {
                            document.getElementById(TControl + "_pbLibrary").disabled = false;
                            document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: col-6-btn margintop5px;");
                        }
                    }
                } else {
                    dcontrol.rows[i].cells[4].children[0].checked = false;
                    if (dcontrol.rows[i].cells[2].children[0].checked == false) {

                        dcontrol.rows[i].cells[9].children[0].checked = false;
                        dcontrol.rows[i].cells[9].children[0].disabled = true;

                        var CCurrentControl = dcontrol.rows[i].cells[9].children[0];
                        CCurrentControl.disabled = true;

                        if (CCurrentControl.parentElement.tagName == 'SPAN' && CCurrentControl.parentElement.disabled == false)
                            CCurrentControl.parentElement.disabled = true;

                        var DLCcontrol = dcontrol.rows[i].cells[4].children[0];
                        var TControl = DLCcontrol.id.replace("chkNo", "DLCHistoryProblem");
                        document.getElementById(TControl + "_txtDLC").disabled = true;
                        document.getElementById(TControl + "_txtDLC").value = "";
                        document.getElementById(TControl + "_pbDropdown").disabled = true;
                        document.getElementById(TControl + "_pbClear").disabled = true;
                        document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: #808080;");
                        document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color: #808080;");
                        document.getElementById(TControl + "_pbLibrary").disabled = true;
                        document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: #808080;");
                    }
                }

            }

        }
    }
}

function enableField(ChkValue) {
    var pcontrol = document.getElementById(ChkValue);
    var YesChkBox = document.getElementById("chkAllYes");
    YesChkBox.checked = false;
    var NoChkBox = document.getElementById("chkAllNo");
    NoChkBox.checked = false;
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    EnableSave('test');
    if (pcontrol.name.indexOf("Yes") != -1) {
        if (pcontrol.checked == true) {

            var CControl = pcontrol.id.replace("chkYes", "chkNo");
            document.getElementById(CControl).checked = false;
            var CCurrentControl = pcontrol.id.replace("chkYes", "chkCurrent");
            document.getElementById(CCurrentControl).checked = false;
            var a = document.getElementById(CCurrentControl);
            a.disabled = false;
            if (a.parentElement.tagName == 'SPAN' && a.parentElement.disabled == true)
                a.parentElement.disabled = false;

            MaskDateEnable(pcontrol, "chkYes", "txtFromDate");
            MaskDateEnable(pcontrol, "chkYes", "txtToDate");

            var TControl = pcontrol.id.replace("chkYes", "DLCHistoryProblem");
            document.getElementById(TControl + "_txtDLC").disabled = false;
            document.getElementById(TControl + "_pbDropdown").disabled = false;
            document.getElementById(TControl + "_pbClear").disabled = false;
            document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: col-6-btn margintop5px;");
            document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color: col-6-btn margintop5px;");

            if (document.getElementById("hdnLibraryIcon").value == "Physician") {
                document.getElementById(TControl + "_pbLibrary").disabled = false;
                document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: col-6-btn margintop5px;");
            }
        } else {

            var CCurrentControl = pcontrol.id.replace("chkYes", "chkCurrent");
            document.getElementById(CCurrentControl).checked = false;
            var a = document.getElementById(CCurrentControl);
            a.disabled = true;
            if (a.parentElement.tagName == 'SPAN' && a.parentElement.disabled == false)
                a.parentElement.disabled = true;

            MaskDateDisable(pcontrol, "chkYes", "txtFromDate");
            MaskDateDisable(pcontrol, "chkYes", "txtToDate");

            var TControl = pcontrol.id.replace("chkYes", "DLCHistoryProblem");
            document.getElementById(TControl + "_txtDLC").disabled = true;
            document.getElementById(TControl + "_txtDLC").value = "";
            document.getElementById(TControl + "_pbDropdown").disabled = true;
            document.getElementById(TControl + "_pbClear").disabled = true;
            document.getElementById(TControl + "_pbClear").disabled = true;
            document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: #808080;");
            document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color:#808080;");
            document.getElementById(TControl + "_pbLibrary").disabled = true;
            document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: #808080;");
        }
    } else {
        if (pcontrol.checked == true) {
            var CControl = pcontrol.id.replace("chkNo", "chkYes");
            document.getElementById(CControl).checked = false;

            var CCurrentControl = pcontrol.id.replace("chkNo", "chkCurrent");
            document.getElementById(CCurrentControl).checked = false;
            var a = document.getElementById(CCurrentControl);
            a.disabled = true;
            if (a.parentElement.tagName == 'SPAN' && a.parentElement.disabled == false)
                a.parentElement.disabled = true;

            MaskDateDisable(pcontrol, "chkNo", "txtFromDate");
            MaskDateDisable(pcontrol, "chkNo", "txtToDate");

            var TControl = pcontrol.id.replace("chkNo", "DLCHistoryProblem");
            document.getElementById(TControl + "_txtDLC").disabled = false;
            document.getElementById(TControl + "_pbDropdown").disabled = false;
            document.getElementById(TControl + "_pbClear").disabled = false;
            document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: col-6-btn margintop5px;");
            document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color: col-6-btn margintop5px;");

            if (document.getElementById("hdnLibraryIcon").value == "Physician") {
                document.getElementById(TControl + "_pbLibrary").disabled = false;
                document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: col-6-btn margintop5px;");
            }

        } else {
            var CCurrentControl = pcontrol.id.replace("chkNo", "chkCurrent");
            document.getElementById(CCurrentControl).checked = false;
            var a = document.getElementById(CCurrentControl);
            a.disabled = true;
            if (a.parentElement.tagName == 'SPAN' && a.parentElement.disabled == false)
                a.parentElement.disabled = true;

            MaskDateDisable(pcontrol, "chkNo", "txtFromDate");
            MaskDateDisable(pcontrol, "chkNo", "txtToDate");

            var TControl = pcontrol.id.replace("chkNo", "DLCHistoryProblem");
            document.getElementById(TControl + "_txtDLC").disabled = true;
            document.getElementById(TControl + "_txtDLC").value = "";
            document.getElementById(TControl + "_pbDropdown").disabled = true;
            document.getElementById(TControl + "_pbClear").disabled = true;
            document.getElementById(TControl + "_pbDropdown").setAttribute("style", "background-color: #808080;");
            document.getElementById(TControl + "_pbClear").setAttribute("style", "background-color: #808080;");
            document.getElementById(TControl + "_pbLibrary").disabled = true;
            document.getElementById(TControl + "_pbLibrary").setAttribute("style", "background-color: #808080;");

        }

    }

}

function DateEnable(ctrl, Value, ctrlName) {
    var pcontrol = ctrl;
    var name = ctrlName;
    var chkValue = Value;

    var FromDateControl = pcontrol.id.replace(chkValue, name);
    document.getElementById(FromDateControl + "_RadButton1").disabled = false;
    document.getElementById(FromDateControl + "_RadButton1").src = "Resources/calenda2.bmp";

    var FromDateYearControl = pcontrol.id.replace(chkValue, name);
    var combo = $find(FromDateControl + "_cboYear");
    combo.enable();
    combo.clearSelection();
    var FromDateMonthControl = pcontrol.id.replace(chkValue, name);
    var combo = $find(FromDateControl + "_cboMonth");
    combo.enable();
    combo.clearSelection();
    var FromDateDateControl = pcontrol.id.replace(chkValue, name);
    var combo = $find(FromDateControl + "_cboDate");
    combo.enable();
    combo.clearSelection();
    var objDTP = FromDateControl + "_clbCalendar";
    var AllDiv = document.getElementsByTagName("div");
    for (var i = 0; i < AllDiv.length; i++) {
        if (AllDiv[i].id != objDTP && AllDiv[i].id.indexOf("clbCalendar") > 0) {
            var temp = $find(AllDiv[i].id.replace("_wrapper", ""));
            if (temp != null && temp._element.id != objDTP) {
                temp._element.style.display = "none";
            }
        }
    }
    $find(objDTP)._element.style.display = "none";
}

function DateDisable(ctrl, Value, ctrlName) {
    var pcontrol = ctrl;
    var name = ctrlName;
    var chkValue = Value;
    var FromDateControl = pcontrol.id.replace(chkValue, name);
    document.getElementById(FromDateControl + "_RadButton1").disabled = true;
    document.getElementById(FromDateControl + "_RadButton1").src = "Resources/calenda2_Disabled.bmp";
    var FromDateYearControl = pcontrol.id.replace(chkValue, name);
    var combo = $find(FromDateControl + "_cboYear");
    combo.disable();
    combo.clearSelection();
    var FromDateMonthControl = pcontrol.id.replace(chkValue, name);
    var combo = $find(FromDateControl + "_cboMonth");
    combo.disable();
    combo.clearSelection();
    var FromDateDateControl = pcontrol.id.replace(chkValue, name);
    var combo = $find(FromDateControl + "_cboDate");
    combo.disable();
    combo.clearSelection();
    var objDTP = FromDateControl + "_clbCalendar";
    var AllDiv = document.getElementsByTagName("div");
    for (var i = 0; i < AllDiv.length; i++) {
        if (AllDiv[i].id != objDTP && AllDiv[i].id.indexOf("clbCalendar") > 0) {
            var temp = $find(AllDiv[i].id.replace("_wrapper", ""));
            if (temp != null && temp._element.id != objDTP) {
                temp._element.style.display = "none";
            }
        }
    }
    $find(objDTP)._element.style.display = "none";
}


function TextBoxClear(IdValue) {
    var pbcontrol = document.getElementById(IdValue);
    var name = pbcontrol.id.replace("pbC", "txtNotes");
    document.getElementById(name).value = "";
    return false;
}

function GeneralNotesClear() {
    document.getElementById("txtGeneralNotes").value = "";
    $find("txtGeneralNotes").clear();
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function btnClearAll_Clicked(sender, args) {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {

        if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        else
            window.parent.theForm.hdnSaveEnable.value = false;
        document.getElementById('InvisibleButton').click();
        return true;
    }
    else {
        sender.set_autoPostBack(false);
        if ($find('btnSave').get_enabled() == true) {
            if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            else
                window.parent.theForm.hdnSaveEnable.value = true;
            localStorage.setItem("bSave", "false");
        }
        else {
            if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            else
                window.parent.theForm.hdnSaveEnable.value = false;
        }
        return false;
    }
}

function ChkCurrent(value) {
    var pcontrol = document.getElementById(value);

    if (pcontrol.checked)
        MaskDateDisable(pcontrol, "chkCurrent", "txtToDate");
    else
        MaskDateEnable(pcontrol, "chkCurrent", "txtToDate");

    $find('btnSave').set_enabled(true);

    if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
}

function setWaitCursor(onoff) {
    if (onoff)
        document.body.style.cursor = "wait";
    else
        document.body.style.cursor = 'default';
}

function hourglass() {
    document.body.style.cursor = "wait";
}

function EnableSave() {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)

        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
}
function SaveEnabled() {
    $('#btnSave').removeProp('disabled');
    EnableSave();
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

    // CAP-3316 - Applying null safety check
    if (PFSHVerified) {
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

function refreshSummaryBar() {

    var cboValue = window.parent.$find('cboSourceOfInformation').get_text();
    window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=Past Medical History&cboValue=" + cboValue;
}

function LaodWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function EndWaitCursorForPage() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}

function LaodWaitCursorForShowAll() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function SetDivPosition() {

    var ctrl = document.getElementsByTagName('INPUT');
    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].id.indexOf('cbo') != -1) {
            if ($find(ctrl[i].id.replace("_Input", "").replace("_ClientState", "")) != null && $find(ctrl[i].id.replace("_Input", "").replace("_ClientState", "")) != undefined)
                $find(ctrl[i].id.replace("_Input", "").replace("_ClientState", "")).hideDropDown();
        }
    }
}

function DisabledDateTimeCalender(value) {
    var objDTP = value + "_clbCalendar";
    var AllDiv = document.getElementsByTagName("div");
    for (var i = 0; i < AllDiv.length; i++) {
        if (AllDiv[i].id != objDTP && AllDiv[i].id.indexOf("clbCalendar") > 0) {
            var temp = $find(AllDiv[i].id.replace("_wrapper", ""));
            if (temp != null && temp._element.id != objDTP) {
                temp._element.style.display = "none";
            }
        }
    }
    $find(objDTP)._element.style.display = "none";
}

function Enable_OR_Disable() {
    if ($find('btnSave').get_enabled())
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";

    if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;

}

function DateSaveEnable() {
    $find('btnSave').set_enabled(true);
    document.getElementById("Hidden1").value = "True";

    if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    return false;
}

function CCTextChanged() {
    $find('btnSave').set_enabled(true);
    document.getElementById("Hidden1").value = "True";

    if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    return false;
}

function btnSaveClicked() {
    var now = new Date();
    var utc = now.toUTCString();
    var chkYesBoxes = $('input[id*="Other"][class="checkYes"]');
    var chkNoBoxes = $('input[id*="Other"][class="checkNo"]');
    var txtDLCs = $('textarea[id*="Other"][class="textDLC"]');
    var list = '', focus_on;
    for (var i = 0; i < chkYesBoxes.length; i++) {
        if ((chkYesBoxes[i].checked == true || chkNoBoxes[i].checked == true) && txtDLCs[i].value == '') {
            if (list == '')
                focus_on = txtDLCs[i];
            list += chkYesBoxes[i].id.replace("chkYes", "") + ', ';
        }
    }
    document.getElementById("hdnLocalTime").value = utc;
    if (list != '') {
        list = list.replace(list, list.substr(0, list.length - 2));
        $find('btnSave').set_autoPostBack(false);
        DisplayErrorMessage('180050', '', list);
        focus_on.focus();
    }
    else {
        $find('btnSave').set_autoPostBack(true);
        top.window.document.getElementById('ctl00_Loading').style.display = 'block';
    }
}

function EndWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}


function cancelBack() {
    $find('btnSave').set_enabled(false);
    if (window.parent.theForm.hdnMenuLevelAutoSave.value != "Menu" && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
}

function SaveEnable() {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
}

function CallMe(sender, args) {
    var inputText = sender._validationText;
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;

    if (inputText.match(FormatDDMMMYYYY)) {
        var DateMonthYear = inputText.split('-');
        if (DateMonthYear[0].length < 4) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }

        if (DateMonthYear[1].length < 3) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }
        if (DateMonthYear[2].length < 2) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }
        if (DateMonthYear[0].length != 0 && DateMonthYear[1].length != 0 && DateMonthYear[2].length != 0) {
            if (DateMonthYear[2] == "00") {
                DisplayErrorMessage('460011');
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;

            }
        }

        lopera2 = DateMonthYear.length;
        var DateInput = parseInt(DateMonthYear[2]);
        var Year = parseInt(DateMonthYear[0]);
        var Month = "";
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
        if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {
            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;
            if (Month == 1 || Month > 2) {
                if (DateInput > ListofDays[Month - 1]) {
                    DisplayErrorMessage('460011');
                    $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }
            }

            if (Month == 2) {
                var lyear = false;
                if ((!(Year % 4) && Year % 100) || !(Year % 400)) {
                    lyear = true;
                }
                if ((lyear == false) && (DateInput >= 29)) {
                    DisplayErrorMessage('460011');
                    $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }
                if ((lyear == true) && (DateInput > 29)) {
                    DisplayErrorMessage('460011');
                    $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase());
            if (Year > CurrentYear) {
                DisplayErrorMessage('460012');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            } else if (Year == CurrentYear && Month > CurrentDate.getMonth()) {
                DisplayErrorMessage('460012');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            } else if (Year == CurrentYear && Month == CurrentDate.getMonth() && DateInput > CurrentDate.getDate()) {
                DisplayErrorMessage('460012');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }
        } else {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }
    } else {

        if (inputText.split('-')[0].length == 0 && (inputText.split('-')[1].length != 0 || inputText.split('-')[0].length != 0)) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[2].length == 1) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[1].length == 0 && inputText.split('-')[0].length == 0) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[2].length != 0 && (inputText.split('-')[1].length == 0 || inputText.split('-')[0].length == 0)) {
            DisplayErrorMessage('460011');
            $find(GetClientId(sender._clientID)).clear();
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[1].length != 0 && inputText.split('-')[0].length != 0) {
            var DateMonthYear = inputText.split('-');
            if (DateMonthYear[0].length < 4) {
                DisplayErrorMessage('460011');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }

            if (DateMonthYear[1].length < 3) {
                DisplayErrorMessage('460011');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }

            var DateMonthYear = inputText.split('-');
            lopera2 = DateMonthYear.length;
            var Year = parseInt(DateMonthYear[0]);
            var Month = "";
            var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
            if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {

                var CurrentDate = new Date();
                var CurrentYear = CurrentDate.getFullYear();
                Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase());
                if (Year > CurrentYear) {
                    DisplayErrorMessage('460012');
                    $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                } else if (Year == CurrentYear && Month > CurrentDate.getMonth()) {
                    DisplayErrorMessage('460012');
                    $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }

            } else {
                DisplayErrorMessage('460011');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }
        } else if (inputText.split('-')[0].length != 0) {
            var DateMonthYear = inputText.split('-');
            if (DateMonthYear[0].length < 4) {
                DisplayErrorMessage('460011');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }

            var DateMonthYear = inputText.split('-');
            var Year = parseInt(DateMonthYear[0]);
            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            if (Year > CurrentYear) {
                DisplayErrorMessage('460012');
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }
        }
    }
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
}


function MaskDateEnable(ctrl, Value, ctrlName) {
    var pcontrol = ctrl;
    var name = ctrlName;
    var chkValue = Value;

    var DateControl = pcontrol.id.replace(chkValue, name);

    var txtMaskDate = $(DateControl);
    if (txtMaskDate != 'undefined') {
        document.getElementById(DateControl).disabled = false;
    }
}

function MaskDateDisable(ctrl, Value, ctrlName) {
    var pcontrol = ctrl;
    var name = ctrlName;
    var chkValue = Value;
    var DateControl = pcontrol.id.replace(chkValue, name);

    var txtMaskDate = $(DateControl);
    if (txtMaskDate != 'undefined') {
        document.getElementById(DateControl).disabled = true;
    }
}

function ShowAll(value) {
    LaodWaitCursor();
    document.getElementById("InvisibleCheckBox").click();
}

function Care() {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
}

function refreshSummaryBar(tabname) {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var clickedtab = tabname.split('@#');
    if (clickedtab != "first") {
        if (clickedtab != 'CC / HPI' && clickedtab != 'QUESTIONNAIRE' && clickedtab != 'PFSH' && clickedtab != 'ROS' && clickedtab != 'VITALS' && clickedtab != 'EXAM' && clickedtab != 'TEST' && clickedtab != 'ASSESSMENT' && clickedtab != 'ORDERS' && clickedtab != 'eRx' && clickedtab != 'SERV./PROC. CODES' && clickedtab != 'PLAN' && clickedtab != 'SUMMARY')
            window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=" + tabname + "&hdnLocalTime=" + utc;
        else {
            window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=" + tabname + "&hdnLocalTime=" + utc;
        }
    } else {
        window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=Past Medical History&hdnLocalTime=" + utc;
    }
}

function LoadTest() {
    var dcontrol = document.getElementById("tblTest");
    for (i = 0; i < dcontrol.rows.length; i++) {
        if (dcontrol.rows[i].cells[4].children[0].checked == false) {
            var DLCcontrol = dcontrol.rows[i].cells[2].children[0];

        }
    }

}

function HistoryProblem_Load() {
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";

}

function TextBoxFocus(va) {
    document.getElementById(va + '_txtDLC').focus();
}

function SetProblemList(ProbLstText, SummaryToolTip) {
    var dox = window.parent.window.parent.window.parent.window.document;

    top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = ProbLstText;
    var regex = /<BR\s*[\/]?>/gi;
    ProbLstText = ProbLstText.replace(regex, "\n");
    regex = /<[\/]{0,1}(span|SPAN)[^><]*>/g;
    ProbLstText = ProbLstText.replace(regex, "");
    top.window.document.getElementById("ProblemList_tooltp").innerText = ProbLstText + "\n";
    RefreshOverallSummaryTooltip();

}

function SuccessFn(data) {
    var value = window.location.search.toString();
    var physician_id = value.split('&')[3];
    var currentprocess = value.split('&')[4].split('~')[0];
    var JData = $.parseJSON(data.d);
    if (JData != "[]" && JData != "") {
        var jsonData = JData[0];
        PastMedicalList = jsonData;
        var GnrlNotes = JData[1];
        GeneralNotesList = GnrlNotes;
        var Prblmlst = JData[2];
        var ScreenMode = JData[4];
        if (jsonData != null && jsonData.length > 0) {
            for (var i = 0; i < jsonData.length; i++) {
                if ($("#tblPastMedical").find("label:contains('" + jsonData[i].description + "')").length > 1) {
                    var len = $("#tblPastMedical").find("label:contains('" + jsonData[i].description + "')").length;
                    for (var j = 0; j < len; j++) {
                        if ($("#tblPastMedical").find("label:contains('" + jsonData[i].description + "')")[j] != undefined && $("#tblPastMedical").find("label:contains('" + jsonData[i].description + "')")[j].innerText.trim() == jsonData[i].description)
                            var lbl = $("#tblPastMedical").find("label:contains('" + jsonData[i].description + "')")[j];
                    }
                }
                else
                    var lbl = $("#tblPastMedical").find("label:contains('" + jsonData[i].description + "')")[0];
                if (lbl != undefined) {
                    $(lbl).parent().parent().removeClass('displayNone');
                    $(lbl).parent().parent().removeAttr("style");
                    if (jsonData[i].ispresent == 'Y') {
                        lbl.parentNode.nextElementSibling.children[0].checked = true;
                        lbl.parentNode.nextElementSibling.nextElementSibling.children[0].checked = false;
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0]).removeProp("disabled");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0]).removeProp("disabled");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0]).removeProp("disabled");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0].children[0]).removeProp("disabled");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeAttr("disabled");
                        var Control = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0])[0];
                        Control.removeAttribute("disabled");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).attr("onclick", "NotesChanged(this, '" + Control.name + "', '" + Control.id + "')");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeClass('pbDropdownBackgrounddisable');
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).addClass('pbDropdownBackground');
                    }
                    else {
                        lbl.parentNode.nextElementSibling.nextElementSibling.children[0].checked = true;
                        lbl.parentNode.nextElementSibling.children[0].checked = false;
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0]).removeProp("disabled");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeAttr("disabled");
                        var Control = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0])[0];
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).attr("onclick", "NotesChanged(this, '" + Control.name + "', '" + Control.id + "')");
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeClass('pbDropdownBackgrounddisable');
                        $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).addClass('pbDropdownBackground');

                    }
                    if (jsonData[i].frmdate != "") {
                        var txtfrmDate = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0];
                        txtfrmDate.value = jsonData[i].frmdate;
                        $(txtfrmDate).removeProp("disabled");
                    }
                    if (jsonData[i].todate == 'Current') {
                        var chktoDate = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0];
                        chktoDate.checked = true;
                        $(chktoDate).removeProp("disabled");
                    }
                    else {
                        var txttoDate = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0];
                        if (jsonData[i].todate != "") {
                            txttoDate.value = jsonData[i].todate;
                            $(txttoDate).removeProp("disabled");
                        }
                        else if (jsonData[i].ispresent == 'Y') {
                            $(txttoDate).removeProp("disabled");
                        }
                    }
                    if (jsonData[i].notes != "") {
                        var tareaNotes = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0];
                        tareaNotes.value = jsonData[i].notes;
                        $(tareaNotes).removeProp("disabled");

                    }
                    $(lbl).parent().parent().find("td:last").find('label')[0].textContent = jsonData[i].version + '-' + jsonData[i].Id;

                }
            }

        }
        else {
            $("#chkShowAll").prop('disabled', 'true');
            var rows = $("#tblPastMedical").find("> tbody > tr");
            for (var i = 0; i < rows.length; i++) {
                var phyID = $(rows[i]).attr("data-physician-ids");
                if (phyID != undefined && phyID.indexOf(physician_id) > -1)
                    $(rows[i]).removeClass('displayNone');
            }
        }
        if (ScreenMode != "" && ScreenMode.toUpperCase() != "MENU") {
            if (GnrlNotes != "" && GnrlNotes != undefined)
                document.getElementById("txtNotes").value = GnrlNotes;
            else
                document.getElementById("txtNotes").value = "";
        }
        else {
            $("[id='txtNotes']").addClass('displayNone');
            $("[id='lblGenNotes']").addClass('displayNone');
            $("[id='NotesPlus']").addClass('displayNone');
        }
        if (Prblmlst != "" && Prblmlst != undefined) {
            SetProblemList(Prblmlst, null);
        }
    }
    else {
        $("#chkShowAll").prop('disabled', 'true')[0].checked = true;

        var rows = $("#tblPastMedical").find("> tbody > tr");
        for (var i = 0; i < rows.length; i++) {
            var phyID = $(rows[i]).attr("data-physician-ids");
            if (phyID != undefined && phyID.indexOf(physician_id) > -1)
                $(rows[i]).removeClass('displayNone');
        }

    }
    if (currentprocess != "SCRIBE_PROCESS" && currentprocess != "AKIDO_SCRIBE_PROCESS" && currentprocess.toUpperCase() != "SCRIBE_CORRECTION" && currentprocess.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && currentprocess != "DICTATION_REVIEW" && currentprocess != "CODER_REVIEW_CORRECTION" && currentprocess != "PROVIDER_PROCESS" && currentprocess != "MA_REVIEW" && currentprocess != "MA_PROCESS" && currentprocess != "PROVIDER_REVIEW_CORRECTION" && currentprocess != "TRANSCRIPT_PROCESS" && currentprocess != "TRANSCRIPT_QC_PROCESS" && currentprocess != "AKIDO_SCRIBE_QC_PROCESS" && currentprocess != "") {
        $('#btnSave')[0].disabled = true;
        $('#btnClearAll')[0].disabled = true;
        $('#mainContainer')[0].disabled = true;
        $('#mainContainer ').find(':input').prop('disabled', true);
        $('#mainContainer ').find('textarea').prop('disabled', true);
        $('a').attr("disabled", true);
        $('a').attr("onclick", "return false;");
        $('a').removeClass('pbDropdownBackground');
        $('a').addClass('pbDropdownBackgrounddisable');
        $("#txtNotes")[0].disabled = true;
        $('#mainContainer ').find('select').prop('disabled', true);
        $('#divHistoryProblem ').find('select').prop('disabled', true);
        $('#chkSelectAllYes')[0].disabled = true;
        $('#chkSelectAllNo')[0].disabled = true;
        //CAP-1882
        $('#cboSourceOfInformation', window.parent.document).prop("disabled", true);
        $('#btnPFSHVerified', window.parent.document).prop("disabled", true);
    }
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}

function CheckAllYes(Control) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($('#chkSelectAllYes')[0].checked) {
        $("[id*=pbDropdown]").removeClass(' pbDropdownBackgrounddisable');
        $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    }
    else {
        $("[id*=pbDropdown]").removeClass(' pbDropdownBackground');
        $("[id*=pbDropdown]").addClass(' pbDropdownBackgrounddisable');
    }

    if ($("#chkSelectAllNo")[0].checked) {
        $("#chkSelectAllNo")[0].checked = false;
        $('#btnSave').removeProp('disabled');
    }
    else {

        var lstchk = $('.chkYN').find("input[type='checkbox']");
        for (var i = 0; i < lstchk.length; i++) {
            if (lstchk[i].id.indexOf('chkYes') > -1 && document.getElementById(lstchk[i].id.replace('chkYes', 'chkNo')).checked == false && lstchk[i].parentNode.parentNode.className != "displayNone") {
                $(lstchk[i])[0].checked = Control.checked;
                $(lstchk[i]).change();
            }

        }
    }

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function CheckAllNo(Control) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    if ($('#chkSelectAllNo')[0].checked) {
        $("[id*=pbDropdown]").removeClass(' pbDropdownBackgrounddisable');
        $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    }
    else {
        $("[id*=pbDropdown]").removeClass(' pbDropdownBackground');
        $("[id*=pbDropdown]").addClass(' pbDropdownBackgrounddisable');
    }
    if ($("#chkSelectAllYes")[0].checked) {
        $("#chkSelectAllYes")[0].checked = false;
        $('#btnSave').removeProp('disabled');
    }
    else {
        var lstchk = $('.chkYN').find("input[type='checkbox']");
        for (var i = 0; i < lstchk.length; i++) {
            if (lstchk[i].id.indexOf('chkNo') > -1 && document.getElementById(lstchk[i].id.replace('chkNo', 'chkYes')).checked == false && lstchk[i].parentNode.parentNode.className != "displayNone") {
                $(lstchk[i])[0].checked = Control.checked;
                $(lstchk[i]).change();
            }
        }
    }


    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function LoadHistoryProblem() {

    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    $('#tblPastMedical').addClass('Editabletxtbox');
    $('span').addClass('Editabletxtbox');
    $('label').addClass('Editabletxtbox');
    $('#pbDropdownNotes').addClass('pbDropdownBackground');
    SetPhysicianSpecificVisibility();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

}
function StopLoadFromPatChart() {
    jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').height('100%');
    jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').fadeOut(300);
    jQuery(top.window.parent.parent.parent.parent.document.body).css('cursor', 'default');

}
function SetPhysicianSpecificVisibility() {
    var rows = $("#tblPastMedical tbody tr");
    var value = window.location.search.toString();
    var physician_id = value.split('&')[3];
    var len = $('textarea').length;
    for (var i = 0; i < rows.length; i++) {

        $(rows[i]).find("td:eq(0)").addClass("lbltd");
        $(rows[i]).find("td:eq(1)").addClass("chkYN");
        $(rows[i]).find("td:eq(2)").addClass("chkYN");
        var tbls = $(rows[i]).find('table');
        $(tbls[0]).parent().addClass('dttd').find("input[type='text']").prop("disabled", "true");
        $(tbls[1]).parent().addClass('dttd').find("input[type='text']").prop("disabled", "true");
        $(tbls[2]).parent().addClass('notestd').find("textarea").prop("disabled", "true");
        $(tbls[2]).parent().addClass('notestd').find("a").attr("disabled", true);
        $(tbls[2]).parent().addClass('notestd').find("a").attr("onclick", "return false;");
        $(tbls[2]).parent().addClass('notestd').find("a").removeClass('pbDropdownBackground');
        $(tbls[2]).parent().addClass('notestd').find("a").addClass('pbDropdownBackgrounddisable');
        $($(rows[i]).find("input[type='checkbox']")[2]).prop("disabled", "true").parent().addClass('chkcrttd');

        if ($(rows[i]).attr("data-physician-ids") != undefined) {
            var arry = new Array();
            for (var n = 0; n < $(rows[i]).attr("data-physician-ids").split(',').length; n++) {
                arry[n] = $(rows[i]).attr("data-physician-ids").split(',')[n].trim();

            }
            if ($.inArray(physician_id, arry) < 0) {
                rows[i].style.display = "none";
            }
        }
    }
    var lstchk = $('.chkYN').find("input[type='checkbox']");
    for (var i = 0; i < lstchk.length; i++)
        if (lstchk[i].id.indexOf('chkYes') > -1)
            $(lstchk[i]).addClass('checkYes');
        else
            if (lstchk[i].id.indexOf('chkNo') > -1)
                $(lstchk[i]).addClass('checkNo');
    $.ajax({
        type: "POST",
        url: "WebServices/PastMedicalHistoryService.asmx/LoadPastMedicalHistory",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            SuccessFn(data);
            $('#btnSave')[0].disabled = true;
        },

        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


        }
    });
    $('.chkcrttd').children().change(function (e) {
        $('#btnSave').removeProp('disabled');
        EnableSave();
        if (this.checked == true) {
            $(this).parent().next().find("input[type='text']").prop('disabled', 'true')[0].value = "";
        }
        else {
            $(this).parent().next().find("input[type='text']").removeProp("disabled");
        }
        Addto_ModifiedControls(e);
    });

    $("input[type='text']").change(function (e) {
        $('#btnSave').removeProp('disabled');
        EnableSave();
        Addto_ModifiedControls(e);
    });

    $("#chkShowAll").click(function () {
        if (this.checked == true) {
            var rows = $("#tblPastMedical > tbody > tr");
            for (var i = 0; i < rows.length; i++) {
                if ($(rows[i]).attr("data-physician-ids") != undefined) {
                    if ($(rows[i]).attr("data-physician-ids").search(physician_id) > -1)
                        $(rows[i]).removeClass('displayNone');
                }
            }
        }
        else {
            var lstYes = $('.checkYes'); var lstNo = $('.checkNo');
            for (var i = 0; i < lstYes.length; i++) {
                var phyID = $(lstYes[i]).parent().parent().attr("data-physician-ids");
                if (lstYes[i].checked == false && lstNo[i].checked == false && phyID != undefined && phyID.indexOf(physician_id) > -1)
                    $(lstYes[i]).parent().parent().addClass('displayNone');
            }

        }
    });

    $("#btnClearAll").click(function () {
        if (DisplayErrorMessage('200005')) {
            $('#btnSave')[0].disabled = true;
            localStorage.setItem("bSave", "false");
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            else
                window.parent.theForm.hdnSaveEnable.value = false;
            var rows = $("#tblPastMedical").find("> tbody > tr");
            for (var i = 0; i < rows.length; i++) {
                $(rows[i]).find('td:eq(1)').find("input[type='checkbox']")[0].checked = false;
                $(rows[i]).find('td:eq(2)').find("input[type='checkbox']")[0].checked = false;
                var tbls = $(rows[i]).find('table');
                $(tbls[0]).parent().addClass('dttd').find("input[type='text']").prop("disabled", "true")[0].value = "";
                $(tbls[1]).parent().addClass('dttd').find("input[type='text']").prop("disabled", "true")[0].value = "";
                $(tbls[2]).parent().addClass('notestd').find("textarea").prop("disabled", "true")[0].value = "";
                $(tbls[2]).parent().addClass('notestd').find("a").attr("disabled", true);
                if ($(tbls[2]).parent().addClass('notestd').find("a")[0].className.indexOf('minus') > -1) {
                    $(tbls[2]).parent().addClass('notestd').find("a").click();
                }
                $(tbls[2]).parent().addClass('notestd').find("a").attr("onclick", "return false;");
                $(tbls[2]).parent().addClass('notestd').find("a").removeClass('pbDropdownBackground')
                $(tbls[2]).parent().addClass('notestd').find("a").addClass('pbDropdownBackgrounddisable')
                $($(rows[i]).find("input[type='checkbox']")[2]).prop("disabled", "true")[0].checked = false;
            }
            $("#txtNotes")[0].value = "";
            $("#chkSelectAllNo")[0].checked = false;
            $("#chkSelectAllYes")[0].checked = false;
            if (PastMedicalList != null && PastMedicalList.length > 0) {
                for (var k = 0; k < PastMedicalList.length; k++) {
                    if ($("#tblPastMedical").find("label:contains('" + PastMedicalList[k].description + "')").length > 1) {
                        var len = $("#tblPastMedical").find("label:contains('" + PastMedicalList[k].description + "')").length;
                        for (var j = 0; j < len; j++) {
                            if ($("#tblPastMedical").find("label:contains('" + PastMedicalList[k].description + "')")[j] != undefined && $("#tblPastMedical").find("label:contains('" + PastMedicalList[k].description + "')")[j].innerText.trim() == PastMedicalList[k].description)
                                var lbl = $("#tblPastMedical").find("label:contains('" + PastMedicalList[k].description + "')")[j];
                        }
                    }
                    else
                        var lbl = $("#tblPastMedical").find("label:contains('" + PastMedicalList[k].description + "')")[0];
                    if (lbl != undefined) {
                        $(lbl).parent().parent().removeClass('displayNone');
                        $(lbl).parent().parent().removeAttr("style");
                        if (PastMedicalList[k].ispresent == 'Y') {
                            lbl.parentNode.nextElementSibling.children[0].checked = true;
                            lbl.parentNode.nextElementSibling.nextElementSibling.children[0].checked = false;
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0]).removeProp("disabled");
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0]).removeProp("disabled");
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0].nextElementSibling).removeProp("disabled");
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0].children[0]).removeProp("disabled");
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeAttr("disabled");
                            var Control = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0])[0];
                            if (Control != undefined) {
                                Control.removeAttribute("disabled");
                                $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).attr("onclick", "NotesChanged(this, '" + Control.name + "', '" + Control.id + "')");
                                $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeClass('pbDropdownBackgrounddisable ');
                                $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).addClass('pbDropdownBackground');
                            }
                        }
                        else {
                            lbl.parentNode.nextElementSibling.nextElementSibling.children[0].checked = true;
                            lbl.parentNode.nextElementSibling.children[0].checked = false;
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0].nextElementSibling).removeProp("disabled");
                            $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeAttr("disabled");
                            var Control = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0])[0];
                            if (Control != undefined) {
                                Control.removeAttribute("disabled");
                                $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).attr("onclick", "NotesChanged(this, '" + Control.name + "', '" + Control.id + "')");
                                $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).removeClass('pbDropdownBackgrounddisable ');
                                $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[1].children[0].children[0]).addClass('pbDropdownBackground');
                            }

                        }
                        if (PastMedicalList[k].frmdate != "") {
                            var txtfrmDate = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0];
                            txtfrmDate.value = PastMedicalList[k].frmdate;
                            $(txtfrmDate).removeProp("disabled");
                        }
                        if (PastMedicalList[k].todate == 'Current') {
                            var chktoDate = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0];
                            chktoDate.checked = true;
                            $(chktoDate).removeProp("disabled");
                        }
                        else {

                            var txttoDate = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0];
                            if (PastMedicalList[k].todate != "") {
                                txttoDate.value = PastMedicalList[k].todate;
                                $(txttoDate).removeProp("disabled");
                            }
                            else if (PastMedicalList[k].ispresent == 'Y') {
                                $(txttoDate).removeProp("disabled");
                            }
                        }
                        if (PastMedicalList[k].notes != "") {
                            var tareaNotes = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].children[0].children[0].children[0].children[0];
                            tareaNotes.value = PastMedicalList[k].notes;
                            $(tareaNotes).removeProp("disabled");
                        }
                        $(lbl).parent().parent().find("td:last").find('label')[0].textContent = PastMedicalList[k].version + '-' + PastMedicalList[k].Id;

                    }
                }

            }
            if (GeneralNotesList != null && GeneralNotesList != undefined && GeneralNotesList != "")
                document.getElementById("txtNotes").value = GeneralNotesList;
            else
                document.getElementById("txtNotes").value = "";
        }
    });

    $('.chkYN').children().change(function (e) {
        $('#btnSave').removeProp('disabled');
        EnableSave();
        if (this.checked == true) {
            if (this.id.indexOf('chkYes') > -1) {
                $(this).parent().next().children()[0].checked = false;
                $($(this).parent().siblings()[2]).find("input[type='text']").removeProp("disabled");
                $($(this).parent().siblings()[3]).find("input[type='checkbox']").removeProp("disabled");
                if (!$($(this).parent().siblings()[3]).find("input[type='checkbox']")[0].checked)
                    $($(this).parent().siblings()[4]).find("input[type='text']").removeProp("disabled");
                $($(this).parent().siblings()[5]).find("textarea").removeProp("disabled");
                var Control = $($(this).parent().siblings()[5]).find("textarea")[0];
                $($(this).parent().siblings()[5]).find("a").removeAttr("disabled");
                $($(this).parent().siblings()[5]).find("a").attr("onclick", "NotesChanged(this, '" + Control.name + "', '" + Control.id + "')");

                $($(this).parent().siblings()[5]).find("a").removeClass('pbDropdownBackgrounddisable');
                $($(this).parent().siblings()[5]).find("a").addClass('pbDropdownBackground');
                $("#chkSelectAllNo")[0].checked = false;
            }
            else
                if (this.id.indexOf('chkNo') > -1) {
                    $(this).parent().prev().children()[0].checked = false;
                    $($(this).parent().siblings()[2]).find("input[type='text']").prop('disabled', 'true')[0].value = "";
                    $($(this).parent().siblings()[3]).find("input[type='checkbox']").prop('disabled', 'true')[0].checked = false;
                    $($(this).parent().siblings()[4]).find("input[type='text']").prop('disabled', 'true')[0].value = "";
                    $($(this).parent().siblings()[5]).find("textarea").removeProp("disabled");
                    $($(this).parent().siblings()[5]).find("a").removeAttr("disabled");
                    var Control = $($(this).parent().siblings()[5]).find("textarea")[0];
                    $($(this).parent().siblings()[5]).find("a").removeAttr("disabled");
                    $($(this).parent().siblings()[5]).find("a").attr("onclick", "NotesChanged(this, '" + Control.name + "', '" + Control.id + "')");
                    $($(this).parent().siblings()[5]).find("a").removeClass('pbDropdownBackgrounddisable');
                    $($(this).parent().siblings()[5]).find("a").addClass('pbDropdownBackground');
                    $("#chkSelectAllYes")[0].checked = false;

                }
        }
        else {
            $($(this).parent().siblings()[2]).find("input[type='text']").prop('disabled', 'true')[0].value = "";
            $($(this).parent().siblings()[3]).find("input[type='checkbox']").prop('disabled', 'true')[0].checked = false;
            $($(this).parent().siblings()[4]).find("input[type='text']").prop('disabled', 'true')[0].value = "";
            $($(this).parent().siblings()[5]).find("textarea").prop('disabled', 'true')[0].value = "";
            if ($($(this).parent().siblings()[5]).find('a')[0].className.indexOf('minus') > -1) {
                $($(this).parent().siblings()[5]).find('a').click();
            }

            $($(this).parent().siblings()[5]).find('a').attr("disabled", true);
            $($(this).parent().siblings()[5]).find('a').attr("onclick", "return false;");
            $($(this).parent().siblings()[5]).find('a').removeClass('pbDropdownBackground');
            $($(this).parent().siblings()[5]).find('a').addClass('pbDropdownBackgrounddisable');
            if (this.id.indexOf('chkNo') > -1) { $("#chkSelectAllNo")[0].checked = false; }
            else if (this.id.indexOf('chkYes') > -1) { $("#chkSelectAllYes")[0].checked = false; }
        }
        Addto_ModifiedControls(e);
    });
    //CAP-1283
    setTimeout(function () {
        $("input:text").mask("9999?-aaa-99");
    },500);
}

function SaveProblemHistory() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var mod_ctrls = [];
    var now = new Date();
    var utc = now.toUTCString();
    var cookies = document.cookie.split(';');
    var dob, DOS;
    for (var i = 0; i < cookies.length; i++) {
        if (cookies[i].split("=")[0].trim() == "Human_Details") {
            dob = cookies[i].split("=")[1].split('|')[3].split(':')[1].trim();
            DOS = cookies[i].split("=")[1].split('|')[5];
        }
    }
    DOS = window.parent.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1];
    var versionYr = "ICD_10";
    if (new Date(DOS) < new Date("2015-10-01")) {
        versionYr = "ICD_9";
    }

    var lstchked = $('.chkYN input:checked');
    var objLst = [];
    for (var i = 0; i < lstchked.length; i++) {
        var p_id = $($(lstchked[i]).parent().siblings()[0]).find("label")[0].innerText;
        if (versionYr == "ICD_9") {
            var icd = $($(lstchked[i]).parent().siblings()[0]).find("label")[0].attributes[1].value;
            var ICD9 = $($(lstchked[i]).parent().siblings()[0]).find("label")[0].attributes[0].value;
        }
        else {
            var ICD9 = $($(lstchked[i]).parent().siblings()[0]).find("label")[0].attributes[0].value;
            var icd = $($(lstchked[i]).parent().siblings()[0]).find("label")[0].attributes[1].value;
        }

        var YN = (lstchked[i].id.indexOf('chkYes') > -1) ? 'Y' : 'N';
        var frmDT = $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value;
        var toDT = $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value;
        var TareaNotes = $($(lstchked[i]).parent().siblings()[5]).find("textarea")[0].value;
        var chkCRT = ($($(lstchked[i]).parent().siblings()[3]).find("input[type='checkbox']")[0].checked) ? 'Current' : '';
        var version = ($($(lstchked[i]).parent().siblings()[6]).find("label")[0].textContent.split('-')[0] == '') ? 0 : $($(lstchked[i]).parent().siblings()[6]).find("label")[0].textContent.split('-')[0];
        var id = ($($(lstchked[i]).parent().siblings()[6]).find("label")[0].textContent.split('-')[1] == '' || $($(lstchked[i]).parent().siblings()[6]).find("label")[0].textContent.split('-')[1] == undefined) ? 0 : $($(lstchked[i]).parent().siblings()[6]).find("label")[0].textContent.split('-')[1];
        var now = new Date();
        var currentdate = now.getUTCFullYear() + '-' + (now.getUTCMonth() + 1) + '-' + now.getUTCDate();
        currentDT = Date.parse(currentdate);
        frmDaTe = Date.parse(frmDT);
        toDaTe = Date.parse(toDT);
        DOB = Date.parse(dob);

        if (frmDT != "" && frmDT.split('-')[1] != undefined)

            var Fdate = frmDT.split('-')[1].toUpperCase();
        else
            var Fdate = "";

        if (toDT != "" && toDT.split('-')[1] != undefined)
            var Tdate = toDT.split('-')[1].toUpperCase();
        else
            var Tdate = "";


        if ((frmDT != "" && frmDT.split('-')[0] != undefined) && (frmDT.split('-')[0] < 1900))// (frmDT.split('-')[0] == "0000" || frmDT.split('-')[0] == "0001"))
        {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            return;
        }
        else if ((toDT != "" && toDT.split('-')[0] != undefined) && (toDT.split('-')[0] < 1900))//(toDT.split('-')[0] == "0000" || toDT.split('-')[0] == "0001")) 
        {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].focus();
            return;
        }


        if ((Fdate != "") && (Fdate != 'JAN' && Fdate != 'FEB' && Fdate != 'MAR' && Fdate != 'APR' && Fdate != 'MAY' && Fdate != 'JUN' && Fdate != 'JUL' && Fdate != 'AUG' && Fdate != 'SEP' && Fdate != 'OCT' && Fdate != 'NOV' && Fdate != 'DEC')) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            return;
        }
        else if ((Tdate != "") && (Tdate != 'JAN' && Tdate != 'FEB' && Tdate != 'MAR' && Tdate != 'APR' && Tdate != 'MAY' && Tdate != 'JUN' && Tdate != 'JUL' && Tdate != 'AUG' && Tdate != 'SEP' && Tdate != 'OCT' && Tdate != 'NOV' && Tdate != 'DEC')) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].focus();
            return;
        }
        else if ((frmDT.split('-')[2] != undefined) && (((Fdate == 'FEB') && (frmDT.split('-')[0] % 4 == 0) && (frmDT.split('-')[2] > 29)) || ((Fdate == 'FEB') && (frmDT.split('-')[0] % 4 != 0) && (frmDT.split('-')[2] > 28)) || (((Fdate == 'SEP') || (Fdate == 'APR') || (Fdate == 'JUN') || (Fdate == 'NOV')) && (frmDT.split('-')[2] > 30)) || ((frmDT.split('-')[2] > 31)))) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            return;

        }
        else if ((toDT.split('-')[2] != undefined) && (((Tdate == 'FEB') && (toDT.split('-')[0] % 4 == 0) && (toDT.split('-')[2] > 29)) || ((Tdate == 'FEB') && (toDT.split('-')[0] % 4 != 0) && (toDT.split('-')[2] > 28)) || (((Tdate == 'SEP') || (Tdate == 'APR') || (Tdate == 'JUN') || (Tdate == 'NOV')) && (toDT.split('-')[2] > 30)) || ((toDT.split('-')[2] > 31)))) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].focus();
            return;

        }
        else if ((frmDaTe > toDaTe) && (toDaTe != "")) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('180002');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            localStorage.setItem("bSave", "false");
            return;
        }
        else if ((frmDaTe < DOB) && (frmDaTe != "")) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('180012');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            localStorage.setItem("bSave", "false");
            return;
        }
        else if ((frmDaTe > currentDT) && (frmDaTe != "")) {

            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('180001');
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            return;
        }

        else if ((toDaTe < DOB) && (toDaTe != "")) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('180013');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].focus();
            localStorage.setItem("bSave", "false");
            return;
        }

        else if ((toDaTe > currentDT) && (toDaTe != "")) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('180011');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].focus();
            localStorage.setItem("bSave", "false");
            return;
        }
        else if ((frmDT.split('-')[2] != undefined) && (frmDT.split('-')[2] == "00" || frmDT.split('-')[2] == "0")) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[2]).find("input[type='text']")[0].focus();
            return;

        }
        else if ((toDT.split('-')[2] != undefined) && (toDT.split('-')[2] == "00" || toDT.split('-')[2] == "0")) {
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('460008');
            { sessionStoragef.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "false");
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].value = "";
            $($(lstchked[i]).parent().siblings()[4]).find("input[type='text']")[0].focus();
            return;

        }
        var obj = { ID: p_id, ICDcode: icd, IsPresent: YN, IsCurrent: chkCRT, FDate: frmDT, TDate: toDT, Notes: TareaNotes, version: version, PMH_id: id, ICD9Code: ICD9 };
        objLst.push(obj);


    }
    var GNotes = { GeneralNotes: $("#txtNotes")[0].value };
    objLst.push(GNotes);

    var LocalTime = { LocalTime: utc };
    objLst.push(LocalTime);

    var VrsnYr = { VersionYr: versionYr };
    objLst.push(VrsnYr);


    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";

    $.ajax({
        type: "POST",
        url: "WebServices/PastMedicalHistoryService.asmx/SavePastMedicalHistory",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify({
            data: objLst,
            Mod_lst: Modified_controls
        }),
        dataType: "json",
        async: true,
        success: function (data) {
            //$('#chkShowAll').removeProp("disabled"); //Commented By Manimaran (18-11-2015 12:43:58 PM) for after saved data show all is enabled. 
            if (data == "") {
                $('#btnSave')[0].disabled = true;
                if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)

                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
                else
                    window.parent.theForm.hdnSaveEnable.value = false;
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            SuccessFn(data);
            SavedSuccessfully();
            $('#btnSave')[0].disabled = true;
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)

                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            else
                window.parent.theForm.hdnSaveEnable.value = false;
            var JData = $.parseJSON(data.d);
            EnablePFSH(JData[3]);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //CAP-2678
            localStorage.setItem('IsSaveCompleted', true);
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;



            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}

function validate(frmDT, toDT, dob, utc) {
    var now = new Date();
    var currentdate = now.getUTCFullYear() + '-' + (now.getUTCMonth() + 1) + '-' + now.getUTCDate();
    currentdate = Date.parse(currentdate);
    var frmDT = Date.parse(frmDT);
    var toDT = Date.parse(toDT);
    var dob = Date.parse(dob);

    if ((frmDT > toDT) && (toDT != "")) {
        DisplayErrorMessage('180002');
        return;
    }
    else if ((frmDT < dob) && (frmDT != "")) {
        DisplayErrorMessage('180012'); return;
    }
    else if ((frmDT > currentdate) && (frmDT != "")) {
        DisplayErrorMessage('180001'); return;
    }

    else if ((toDT < dob) && (toDT != "")) {
        DisplayErrorMessage('180013'); return;
    }

    else if ((toDT > currentdate) && (toDT != "")) {
        DisplayErrorMessage('180011'); return;
    }

    else
        return 1;
}

function SavedSuccessfully() {

    DisplayErrorMessage('180602');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
}

function Filter(array, terms) {

    arrayOfTerms = terms.split(",");
    var first_resultant = array;
    var resultant;
    resultant = $.grep(first_resultant, function (item) {
        return item.toLowerCase().indexOf(arrayOfTerms[arrayOfTerms.length - 1].trim().toLowerCase()) > -1;

    });
    first_resultant = resultant;
    return first_resultant;
}

var Modified_controls = [];
var Mod_ctrl_Count = 0;
$(document).ready(function () {
    LoadHistoryProblem();
    $("textarea").bind("keydown", function (e) {//BUGID:45541

        insertTab(this, event);
    });
    if ((window.parent.document.title == "PFSH" && document.URL.indexOf('OpeingFrom=Menu') > -1 && window.parent.parent.document.URL.indexOf("PatientChart") > -1)) {
        var winName = "PFSHWindow";
        if ($(top.window.parent.document).find("iframe[name='ctl00_PFSHWindow']").length > 0)
            winName = "ctl00_PFSHWindow";
        if ($($(top.window.parent.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').find("#Modal").length == 0) {
            $("<div id='Modal' class='modal fade' style='background-color: transparent'><div class='modal-dialog' style='margin-top: 7%;'>" +
                "<div class='modal-content' style='width:90%;'><div class='modal-header' style='padding-top: 0px; padding-bottom: 0px;'>" +
                "<button type='button' id='btnClosewindow' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>" +
                "<h5 id='ModalTtle' style='font-weight:bold;'></h5></div><div class='modal-body' style='height:560px;'>" +
                "<iframe style='width: 100%; height: 100%; border: none' id='ProcessiFrame'></iframe></div></div></div></div>").insertAfter($($(top.window.parent.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body div:last'));
        }
    }

    var UserRole = "";
    if (window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole != null && window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole != undefined) {
        UserRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value.split('&')[1];
    }
    if (UserRole == "Physician" || UserRole == "Physician Assistant") {
        $("textarea").on('change keypress', function (e) {
            $('#btnSave').removeProp('disabled');
            EnableSave();
            Addto_ModifiedControls(e);
        });
        //CAP-1434
        $("textarea").on('change keyup', function (e) {
            if (e.key === "Backspace") {
                $('#btnSave').removeProp('disabled');
                EnableSave();
                Addto_ModifiedControls(e);
            }
        });
    }
});

function Addto_ModifiedControls(e) {
    if (e.currentTarget.type == "checkbox")
        var control = $(e.currentTarget.parentNode.parentNode).find("label")[0].attributes[1].value;
    else
        var control = $(e.currentTarget.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode).find("label")[0].attributes[1].value;

    if (Modified_controls.indexOf(control) == -1) {
        Modified_controls.push(control);
    }

}

function OpenPopup(keyword) {
    if ((window.parent.document.title == "PFSH" && document.URL.indexOf('OpeingFrom=Menu') > -1 && window.parent.parent.document.URL.indexOf("PatientChart") > -1)) {
        var winName = "PFSHWindow";
        if ($(top.window.parent.document).find("iframe[name='ctl00_PFSHWindow']").length > 0)
            winName = "ctl00_PFSHWindow";
        var focused = keyword;
        $($(top.window.parent.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').find("#Modal").modal({ backdrop: 'static', keyboard: false });
        $($(top.window.parent.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
        $($(top.window.parent.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
    }
    else {
        var focused = keyword;
        $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
        $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
        $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
    }
}
function ValidateMonth(sender, args) {
    var inputText = sender.value;
    if (inputText != "____-___-__" && inputText != "") {
        var CurrentDate = new Date();
        var CurrentYear = CurrentDate.getFullYear();
        var SplitDate = inputText.split('-');
        var Year = parseInt(SplitDate[0]);
        if (SplitDate.length >= 2) {
            if (SplitDate[0] == "0000") {
                DisplayErrorMessage('460011');
                $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                $("#" + sender.id)[0].focus();
                return false;

            }
            if (SplitDate[1].length < 3) {
                DisplayErrorMessage('460011');
                $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                $("#" + sender.id)[0].focus();
                return false;
            }

            var Month = "";
            var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
            if (ListofMonth.indexOf(SplitDate[1].toUpperCase()) != -1) {
                if (SplitDate.length == 2) {
                    Month = ListofMonth.indexOf(SplitDate[1].toUpperCase());
                    if (Year == CurrentYear && Month > CurrentDate.getMonth()) {
                        DisplayErrorMessage('460012');
                        $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                        $("#" + sender.id)[0].focus();
                        return false;
                    }
                }
                if (SplitDate.length >= 3) {
                    var DateInput = parseInt(SplitDate[2]);
                    Month = ListofMonth.indexOf(SplitDate[1].toUpperCase()) + 1;
                    if (Month == 1 || Month > 2) {
                        if (DateInput > ListofDays[Month - 1]) {
                            alert('Cannot be future date. Please Enter a Valid Date.');
                            $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                            $("#" + sender.id)[0].focus();
                            return false;
                        }
                    }

                    if (Month == 2) {
                        var lyear = false;
                        if ((!(Year % 4) && Year % 100) || !(Year % 400)) {
                            lyear = true;
                        }
                        if ((lyear == false) && (DateInput >= 29)) {
                            DisplayErrorMessage('460011');
                            $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                            $("#" + sender.id)[0].focus();
                            return false;
                        }
                        if ((lyear == true) && (DateInput > 29)) {
                            DisplayErrorMessage('460011');
                            $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                            $("#" + sender.id)[0].focus();
                            return false;
                        }
                    }
                    Month = ListofMonth.indexOf(SplitDate[1].toUpperCase());
                    if (Year > CurrentYear) {
                        DisplayErrorMessage('460012');
                        $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                        $("#" + sender.id)[0].focus();
                        return false;
                    } else if (Year == CurrentYear && Month > CurrentDate.getMonth()) {
                        DisplayErrorMessage('460012');
                        $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                        $("#" + sender.id)[0].focus();
                        return false;
                    } else if (Year == CurrentYear && Month == CurrentDate.getMonth() && DateInput > CurrentDate.getDate()) {
                        DisplayErrorMessage('460012');
                        $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                        $("#" + sender.id)[0].focus();
                        return false;
                    }
                    if (SplitDate[2] == "00") {
                        DisplayErrorMessage('460011');
                        $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                        $("#" + sender.id)[0].focus();
                        return false;

                    }
                }

            }
            else {
                DisplayErrorMessage('460011');
                $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                $("#" + sender.id)[0].focus();
                return false;
            }
        }
        else if (SplitDate.length == 1) {
            if (Year > CurrentYear) {
                DisplayErrorMessage('460012');
                $("#" + sender.id).val($("#" + sender.id).attr('placeholder'));
                $("#" + sender.id)[0].focus();
                return false;
            }
        }
        $('#btnSave').removeProp('disabled');
        localStorage.setItem("bSave", "false");
    }
}
var hdnFieldName = null;
function NotesChanged(icon, List, id) {

    if (icon.className.indexOf("plus") > -1) {
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");
        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var values = response.d.split("|");
                var targetControlValue = id;
                var innerdiv = '';
                var pos = $('#' + targetControlValue).position();
                $('#' + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BUGID:45541
                var Height = "120px";
                if (List == "Problem History Notes") {
                    pos.top = pos.top + 20;
                    Height = "55px";
                }
                if (window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole != null && window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole != undefined) {
                    UserRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value.split('&')[1];
                }
                if (UserRole == "Physician" || UserRole == "Physician Assistant") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default'  class='alinkstyle' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                if (values.length > 1) {
                    for (var i = 0; i < values.length - 1; i++) {

                        innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black' onclick=\"SelectedNotes('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i] + "</li>";
                    }
                }
                else {
                    values = "";
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black' onclick=\"SelectedNotes('" + values.split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values + "</li>";//BUGID:45541
                }

                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                        .css({
                            top: pos.top + 37,
                            left: pos.left,
                            width: $("#" + targetControlValue).width() + 5 + "px",
                            height: Height,
                            overflow: 'scroll',
                            position: 'absolute',
                            background: 'white',
                            bottom: '0',
                            floating: 'top',
                            border: '1px solid #8e8e8e',
                            background: '#FFF',
                            fontFamily: 'Segoe UI",Arial,sans-serif',
                            fontSize: '12px',
                            zIndex: '17',
                            overflowX: 'auto'

                        })
                        .focusout(function () {


                            $(this).css("display", "none");
                        })
                        //CAP-804 Syntax error, unrecognized expression
                        .insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                }
                // EnableSave();

            },
            failure: function (response) {
                alert(response.responseJSON.Message + ". Please Contact Support!");
            }
        });
    }
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");

    }

    if (hdnFieldName != null && hdnFieldName != icon) {

        $(hdnFieldName).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFieldName = icon;
}
function SelectedNotes(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BUGID:45541
    $('#btnSave')[0].disabled = false;
    EnableSave();
    var value = agrulist.split(",");
    //CAP-804 Syntax error, unrecognized expression
    //CAP-1471
    var sugglistval = $("#" + value[1]?.trim() + ".actcmpt")?.val()?.trim()??"";

    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split(",")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value[0]) {
                flag++;
            }
        }
        if (flag == 0) {
            $("#" + value[1]?.trim() + ".actcmpt").val(sugglistval + "," + value[0]);
        }
    }
    else {
        $("#" + value[1]?.trim() + ".actcmpt").val(value[0]);
    }

    EnableSave();
}
$("textarea").bind("keypress", function (e) {
    $('#btnSave').removeProp('disabled');
    EnableSave();
});
//CAP-1434
$("textarea").bind("keyup", function (e) {
    if (e.key === "Backspace") {
        $('#btnSave').removeProp('disabled');
        EnableSave();
    }
});


