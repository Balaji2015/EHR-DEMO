var chkToggleState = false;

function txtSystemNotes_ValueChanged() {
    btnSaveEnabled(false);
}

function checkStateWhenAllSymptomsCheckBoxChecked(chkBoxId) {
    var chkList = new Array();
    var chkBox = document.getElementById(chkBoxId);
    var splitter = chkBoxId.split("_");
    var table = document.getElementById("table_" + splitter[1]);

    if (document.getElementById("hdnChkToggleState").value == "false") {
        for (i = 0; i < table.rows.length; i++) {
            for (j = 0; j < table.rows[i].cells.length; j++) {
                if (table.rows[i].cells[j].children[0] != null && table.rows[i].cells[j].children[0].id.indexOf(splitter[0]) != -1 && !table.rows[i].cells[j].children[0].id.endsWith("All")) {
                    document.getElementById("hdnChkToggleState").value = true;
                    var nameFind1;

                    if (splitter[0] == "chkYes")
                        nameFind1 = "chkNo";
                    else if (splitter[0] == "chkNo")
                        nameFind1 = "chkYes";

                    chkList.push(table.rows[i].cells[j].children[0]);

                    var chkctrl = document.getElementById(nameFind1 + "_" + splitter[1] + "_All");

                    if (chkctrl.checked)
                        chkctrl.checked = false;
                }
            }
        }

        for (k = 0; k < chkList.length; k++) {
            document.getElementById("hdnChkToggleState").value = true;

            if (chkBox.checked) {
                if (!chkList[k].checked) {
                    var splitName = chkList[k].id.split('_');

                    var nameFind;

                    if (splitName[0] == "chkYes")
                        nameFind = "chkNo";
                    else if (splitName[0] == "chkNo")
                        nameFind = "chkYes";

                    var chkctrl = document.getElementById(nameFind + "_" + splitName[1] + "_" + splitName[2]);

                    if (!chkctrl.checked)
                        chkList[k].checked = true;
                }
            } else if (!chkBox.checked) {
                if (chkList[k].checked)
                    chkList[k].checked = false;
            }
        }
    }

    document.getElementById("hdnChkToggleState").value = false;
    btnSaveEnabled(false);
}

function chkYesNoToggleStateChanged(chkBoxId) {
    btnSaveEnabled(false);

    var chk = document.getElementById(chkBoxId);
    var splitter = chkBoxId.split("_");
    var table = document.getElementById("table_" + splitter[1]);

    if (chk.checked)
        checkYOrN(chk);
    else {
        chk.checked = false;
        var split = chk.id.split('_');

        var chkCtrl = document.getElementById(split[0] + "_" + split[1]);

        if (chkCtrl != null) {
            if (chkCtrl.checked == true) {
                document.getElementById("hdnChkToggleState").value = true;
                chkCtrl.checked = false;
            }
        }

        document.getElementById("hdnChkToggleState").value = false;

        if (chk.id.indexOf("chkYes_") != -1)
            document.getElementById("chkAllOtherSystemsNormal").checked = false;
    }
}

function checkYOrN(chk) {
    var splitter = chk.id.split('_');
    var table = document.getElementById("table_" + splitter[1]);

    if (document.getElementById("hdnChkToggleState").value == "false") {
        for (i = 0; i < table.rows.length; i++) {
            for (j = 0; j < table.rows[i].cells.length; j++) {
                if (table.rows[i].cells[j].children[0] != null && table.rows[i].cells[j].children[0].id.endsWith("All")) {
                    document.getElementById("hdnChkToggleState").value = true;
                    if (table.rows[i].cells[j].children[0].checked)
                        table.rows[i].cells[j].children[0].checked = false;
                }

                if (table.rows[i].cells[j].children[0] != null && table.rows[i].cells[j].children[0].id == chk.id && table.rows[i].cells[j].children[0].id.indexOf(splitter[splitter.length - 1]) != -1) {
                    table.rows[i].cells[j].children[0].checked = true;
                    document.getElementById("table_" + splitter[1]).checked = false;
                } else if (table.rows[i].cells[j].children[0] != null && table.rows[i].cells[j].children[0].id.indexOf(splitter[splitter.length - 1]) != -1)
                    table.rows[i].cells[j].children[0].checked = false;
            }
        }
    }

    document.getElementById("hdnChkToggleState").value = false;
}

function chkAllOtherSystemsNormalClick(systemNames) {
    btnSaveEnabled(false);

    var systemNamesArray = new Array();
    var systemNamesSplitter = systemNames.split('|');

    for (i = 0; i < systemNamesSplitter.length; i++)
        systemNamesArray.push(systemNamesSplitter[i]);

    var chkBox = document.getElementById("chkAllOtherSystemsNormal");

    if (chkBox != null)
        checkStateWhenAllOtherAreNormalIsChecked(chkBox.checked, systemNamesArray);
}

function checkStateWhenAllOtherAreNormalIsChecked(chkState, systemNamesArray) {
    for (k = 0; k < systemNamesArray.length; k++) {
        var table = document.getElementById("table_" + systemNamesArray[k]);
        if (table != null) {
            for (i = 0; i < table.rows.length; i++) {
                var isChkYesChecked = false;

                for (j = 0; j < table.rows[i].cells.length; j++) {
                    if (table.rows[i].cells[j].children[0] == null || table.rows[i].cells[j].children[0].id.indexOf("chk") == -1)
                        continue;

                    if (table.rows[i].cells[j].children[0].id.indexOf("Yes") != -1 && table.rows[i].cells[j].children[0].id.endsWith("All")) {
                        if (table.rows[i].cells[j].children[0].checked)
                            break;
                    }

                    if (table.rows[i].cells[j].children[0].id.indexOf("No") != -1 && table.rows[i].cells[j].children[0].id.endsWith("All")) {
                        document.getElementById("hdnChkToggleState").value = false;
                        table.rows[i].cells[j].children[0].checked = chkState ? true : false;
                    }

                    if (table.rows[i].cells[j].children[0].id.indexOf("chkYes") != -1 && !table.rows[i].cells[j].children[0].id.endsWith("All")) {
                        if (table.rows[i].cells[j].children[0].checked)
                            isChkYesChecked = true;
                    }

                    if (table.rows[i].cells[j].children[0].id.indexOf("chkNo") != -1 && !table.rows[i].cells[j].children[0].id.endsWith("All")) {
                        table.rows[i].cells[j].children[0].checked = chkState && !isChkYesChecked ? true : false;
                        isChkYesChecked = false;
                    }
                }
            }
        }
    }
}

function btnSaveEnabled(val) {
    if (document.getElementById("btnSave").control != undefined)
        document.getElementById("btnSave").control.set_enabled((val != true) ? true : false);
    else
        document.getElementById(GetClientId("btnSave")).disabled = val;
    if (!val) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    else {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
    }
}


function cancelBack() {
    if (document.getElementById('dlcROS_txtDLC').disabled == false) {
        document.getElementById("btnSave").disabled = true;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
}

function resetClick() {
    var systemNames = document.getElementById("systemNamesForEventHF").value;
    var isReset = false;

    if (DisplayErrorMessage('190002') == true) {
        clearAll(systemNames);
        document.getElementById("btnSave").disabled = true;
        document.getElementById("isResetOK").value = "true";
        reLoadROS();
        return true;
    } else {
        isReset = !document.getElementById("btnSave").disabled ? false : true;
        document.getElementById("isResetOK").value = "false";
        return false;
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}

function clearAll(systemNames) {
    var systemNamesArray = new Array();
    var systemNamesSplitter = systemNames.split('|');

    for (i = 0; i < systemNamesSplitter.length; i++)
        systemNamesArray.push(systemNamesSplitter[i]);

    for (k = 0; k < systemNamesArray.length; k++) {
        var table = document.getElementById("table_" + systemNamesArray[k]);

        for (i = 0; i < table.rows.length; i++) {
            for (j = 0; j < table.rows[i].cells.length; j++) {
                if (table.rows[i].cells[j].children[0] == null)
                    continue;

                if (table.rows[i].cells[j].children[0] != null && table.rows[i].cells[j].children[0].id.indexOf("chk") != -1)
                    table.rows[i].cells[j].children[0].checked = false;

                if (table.rows[i].cells[j].children[5] != null && table.rows[i].cells[j].children[5].id.indexOf("dlc_") != -1)
                    document.getElementById(table.rows[i].cells[j].children[5].id).innerHTML = "";
            }
        }
    }

    document.getElementById("chkAllOtherSystemsNormal").checked = false;
    document.getElementById("dlcROS_txtDLC").innerHTML = "";
    btnSaveEnabled(false);
}

function clearTextBox(imageButtonId) {
    if (imageButtonId == "ibClear")
        document.getElementById("dlcROS_txtDLC").innerHTML = "";
    else {
        var splitter = imageButtonId.split('_');
        document.getElementById("dlc_" + splitter[1]).innerHTML = "";
    }
}

var bCheckNotes = true;

function lst_SelectedIndexChanged(listBoxId) {
    var groupName = listBoxId.split('_');
    var clearText = document.getElementById("txtSystemNotes_" + groupName[1]);
    var listBox = document.getElementById("lst_" + groupName[1]);
    var selectedTextLength = 0;
    var totalLength = 0;
    var item = listBox.selectedItem;

    if ((item != null) && (bCheckNotes == true)) {
        selectedTextLength = item.Text.Length;
        totalLength = selectedTextLength + clearText.Text.Length + 2;

        if (totalLength <= clearText.MaxLength) {
            bCheckNotes = false;
            if (clearText.Text == string.Empty || clearText.Text.StartsWith(" "))
                clearText.Text = item.Text;
        }

        clearText.Focus();
    }
}

function saveClientClick() {
    //Cap - 1765
    document.getElementById("HdnCopyButton").value = "";

    var btnSave = $find('btnSave');
    if (btnSave._enabled) {
        var now = new Date();
        var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
        then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
        utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
        document.getElementById(GetClientId("hdnLocalTime")).value = utc;
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    }
}

function OnClientClose(oWnd, args) {
    var arg = args.get_argument();
    if (arg) {
        var Physician_ID = arg.Physician_ID;
        if (Physician_ID != "0") {
            document.getElementById('hdnCopyPreviousPhysicianId').value = Physician_ID;
            document.getElementById('btnFloatingSummary').click();
            $find('btnSave').set_enabled(true);
        }
    }
}

function enableOrDiasble() {
    if (document.getElementById("btnSave").disabled == false) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        document.getElementById("isbtnSaveOrDisable").value = true;
        localStorage.setItem("bSave", "false");
    } else {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        document.getElementById("isbtnSaveOrDisable").value = "";
        localStorage.setItem("bSave", "true");
    }
}

function savedSuccessfully() {
   
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    DisplayErrorMessage('190001');
    top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    AutoSaveSuccessful();
    //CAP-2678
    localStorage.setItem("IsSaveCompleted", true);
}

function RefreshFloatingSummary() {
    var dox = window.parent.window.document;
    var iFrame = dox.getElementsByTagName("iframe");
    if (iFrame.length > 0) {
        var str = iFrame[0].src;
        var n = str.indexOf("frmFollowUpEncounter.aspx");
        if (n >= 0) {
            iFrame[0].src = iFrame[0].src;
        } else {

        }
    } else {
        //console.log("Error in finding the Floating Summary");
    }
}

function reLoadROS() {
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    window.location.reload(false);
}


function PreviousEncoutner() {
    DisplayErrorMessage('190005');
}

function OnROSLoad() {
    window.parent.theForm.hdnSaveButtonID.value = "btnSave";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    
}

function btnPastEncounter_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var button2 = $find("btnPastEncounter");
    var btnSave = $find('btnSave');
    if (btnSave._enabled) {
        CopyPrevious();
        sender.set_autoPostBack(false);
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    } else {
        button2.set_autoPostBack(true);
    }
}


function Loading() {
    document.getElementById('divLoading').style.display = "none";
}

function checkSavebutton() {
    var button2 = $find('btnSave');
    var btnCopyCheck = $find('btnPastEncounter');
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById("HdnCopyButton").value = "trueValidate";
    document.getElementById("hdnTrueCheck").value = "true";
    testCopy = "true";
    button2.click();
    LoadPastEncounter();
    button2.set_enabled(true);
    //var obj = new Array();
    //obj.push("Title=" + "Message");
    //obj.push("ErrorMessages=" + "There are unsaved changes.Do you want to save them?");
    //var result = openModal("frmValidationArea.aspx", 100, 300, obj, "MessageWindow");
    //var WindowName = $find('MessageWindow');
    //WindowName.add_close(OnClientCloseValidation);
}

function OnClientCloseValidation(oWindow, args) {

    var button2 = $find('btnSave');
    var btnCopyCheck = $find('btnPastEncounter');
    var arg = args.get_argument();
    if (arg) {
        var result = arg;
        if (result == "Yes") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            document.getElementById("HdnCopyButton").value = "trueValidate";
            document.getElementById("hdnTrueCheck").value = "true";
            testCopy = "true";
            button2.click();
            LoadPastEncounter();
            button2.set_enabled(true);

        } else if (result == "Cancel") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
        else {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            button2.set_enabled(false);
            document.getElementById("HdnCopyButton").value = "";
            document.getElementById("btnPastEncounter").click();
        }
    }
}

function LoadPastEncounter() {
    document.getElementById("HdnCopyButton").value = "CheckSave";
    document.getElementById("btnCopyPrevHidden").click();
    return true;
}

function onCopyPrevious(errorCode) {

    if (errorCode == "") {
        btnSaveEnabled(false);
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    }
    else {
        DisplayErrorMessage(errorCode);
        btnSaveEnabled(true);
        localStorage.setItem("bSave", "true");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}

function enableAutoSave() {
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    $('#btnSave')[0].disabled = false;
}
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnSave')[0].disabled = true;
}

function CopyPrevious() {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
        event.preventDefault();
        event.stopPropagation();
        var btnSave = $find('btnSave');
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        document.getElementById("HdnCopyButton").value = "trueValidate";
        document.getElementById("hdnTrueCheck").value = "true";
        testCopy = "true";
        btnSave.click();
        enableAutoSave();
        btnSave.set_enabled(true);
        return;
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogROS');

        //if (!dvdialog) {
        //    dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogFocused');
        //}

        //var btnSave = $find('btnSave');

        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            document.getElementById("HdnCopyButton").value = "trueValidate";
        //            document.getElementById("hdnTrueCheck").value = "true";
        //            testCopy = "true";
        //            btnSave.click();
        //            enableAutoSave();
        //            btnSave.set_enabled(true);
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            btnSave.set_enabled(false);
        //            disableAutoSave();
        //            document.getElementById("HdnCopyButton").value = "";
        //            document.getElementById("btnPastEncounter").click();
        //            $(dvdialog).dialog("close");
        //        },
        //        "Cancel": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            $(dvdialog).dialog("close");
        //             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //            return;
        //        }
        //    }
        //});
    }
    else {
        LoadPastEncounter();
    }
}