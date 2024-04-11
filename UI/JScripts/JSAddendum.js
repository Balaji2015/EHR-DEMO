function enableSave(e) {
    if (document.getElementById("hdnIsLoad").value == "false")
        $find('btnSaveAndClose').set_enabled(true);

    $find('btnMoveToProviderReview').set_enabled(true);

    if (e.srcElement.value.length >= 32767)
        return false;
}

function isReview() {

    var txt;
    var r = confirm("Would you like to send a copy to provider for review?");
    if (r == true) {
        document.getElementById("btnIsReview").click();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    } else {
        document.getElementById("btnReview").click();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }


}

function closeMessage() {
    if (DisplayErrorMessage('7490008') == true) {
        self.close();
    }
    refreshChart();
}
function showTime(sender, args) {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    if (document.getElementById("hdnIsAssistant").value == "true") {
        if (sender._text == "Move To Next Process") {
            sender.set_autoPostBack(false);
            isReview();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            return true;
        }
        else {
            sender.set_autoPostBack(true);
        }
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    return false;
}
function refreshChart(encID) {
    //$(top.window.document).find('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + encID;
    $(top.window.document).find('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + encID +"&TabMode=true";
}

function OpenException() {
    var obj = new Array();
    $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Feedback for Coding Exception";
    $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
    $(top.window.document).find("#TabmdldlgException")[0].style.height = "800px";
    var sPath = ""
    var patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
    sPath = "frmException.aspx?formName=" + "Feedback for Coding Exception" + "&PatientName=" + patientName;
    $(top.window.document).find("#TabExceptionFrame")[0].style.height = "725px";
    $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
    });
}

function rdPat_rdPro_Change() {

    if (event.target.id.indexOf("rdProvider") > -1 && event.target.checked == true) {
        $find('cboAcceptOrDeny').set_enabled(true);
        $find('cboAcceptOrDeny').clearSelection();
        //Jira #CAP-700
       // $('#spanAcceptOrDeny').css('color', 'black').text("Accept or Deny ?");
        $('#spanAcceptOrDeny').text("Accept or Deny ?");
        document.getElementById("spanAcceptOrDeny").classList.remove("manredforstar");
        document.getElementById("spanAcceptOrDeny").style.color = "black";
    }
    else if (event.target.id.indexOf("rdPatient") > -1 && event.target.checked == true) {
        $find('cboAcceptOrDeny').set_enabled(true);
        $('#spanAcceptOrDeny').css('color', 'red').text("Accept or Deny ?*");
        $(spanAcceptOrDeny).html($(spanAcceptOrDeny).html().replace("*", "<span class='manredforstar'>*</span>"));
    }
    EnableButtons();
}

function cboAcceptOrDeny_IndexChanged(sender, eventArgs) {
    EnableButtons();
}

function EnableButtons() {
    $find('btnSaveAndClose').set_enabled(true);
    $find('btnMoveToProviderReview').set_enabled(true);
}
var v1 = 0;
function OnbtnSaveAndCloseClick(sender, eventArgs) {

    var userRole = document.getElementById('hdnUserRole').value;
    if (document.getElementById('rdProvider').checked == false && document.getElementById('rdPatient').checked == false && userRole != "CODER") {
        DisplayErrorMessage('7490010');
        sender.set_autoPostBack(false);
        NotSaved();
        v1 = 1;
        return;
    }
    //Jira #CAP-725
    //if (document.getElementById('rdPatient').checked == true && $find('cboAcceptOrDeny').get_selectedItem().get_text().trim() == "" && userRole != "CODER") {
    if (document.getElementById('rdPatient').checked == true && document.getElementById("cboAcceptOrDeny").value.trim() == "" && userRole != "CODER") {
        DisplayErrorMessage('7490011');
        sender.set_autoPostBack(false);
        NotSaved();
        v1 = 1;
        return;
    }
    if (document.getElementById('txtAddendumNotes_txtDLC').value.trim() == '' && userRole != "CODER") {
        DisplayErrorMessage('7490012');
        sender.set_autoPostBack(false);
        NotSaved();
        v1 = 1;
        return;
    }
    if ((userRole == "MEDICAL ASSISTANT" || userRole == "PHYSICIAN ASSISTANT") && $find('cboShowAllPhysicians').get_text() == "") {
        DisplayErrorMessage('7490003');
        sender.set_autoPostBack(false);
        NotSaved();
        v1 = 1;
        return;
    }
    if (sender._text == "Move To Next Process" && (userRole == "PHYSICIAN" || userRole == "PHYSICIAN ASSISTANT")) {
        if (document.getElementById('chkElectronicDigitalSignature').checked == false) {
            DisplayErrorMessage('7490001');
            sender.set_autoPostBack(false);
            NotSaved();
            v1 = 1;
            return;
        }
    }
    
    var check = showTime(sender, eventArgs);
//  if (v1 == 0)
//Jira Cap - 600
   // sender.set_autoPostBack(true);
    //else {
    //    v1 == 0;

    //Jira #CAP-656 and Jira #CAP-659 and Jira #CAP-658
    ////Jira Cap - 600
       // __doPostBack('btnMoveToProviderReview');
    sender.set_autoPostBack(false);
//Jira CAP-787 and Jira CAP-748 intriduced the condtion
    if (check == false) {
        __doPostBack('btnMoveToProviderReview', "true");
    }
    //}
}
var v = 0;
function OnbtnSaveAndClose(sender, eventArgs) {

    var userRole = document.getElementById('hdnUserRole').value;
    if (document.getElementById('rdProvider').checked == false && document.getElementById('rdPatient').checked == false && userRole != "CODER") {
        DisplayErrorMessage('7490010');
        sender.set_autoPostBack(false);
        NotSaved();
        v = 1;
        return;
    }
    //Jira #CAP-725
    //if (document.getElementById('rdPatient').checked == true && $find('cboAcceptOrDeny').get_selectedItem().get_text().trim() == "" && userRole != "CODER") {
    if (document.getElementById('rdPatient').checked == true && document.getElementById("cboAcceptOrDeny").value.trim() == "" && userRole != "CODER") {
        DisplayErrorMessage('7490011');
        sender.set_autoPostBack(false);
        NotSaved();
        v = 1;
        return;
    }
    if (document.getElementById('txtAddendumNotes_txtDLC').value.trim() == '' && userRole != "CODER") {
        DisplayErrorMessage('7490012');
        sender.set_autoPostBack(false);
        NotSaved();
        v = 1;
        return;
    }
    if ((userRole == "MEDICAL ASSISTANT" || userRole == "PHYSICIAN ASSISTANT") && $find('cboShowAllPhysicians').get_text() == "") {
        DisplayErrorMessage('7490003');
        sender.set_autoPostBack(false);
        NotSaved();
        v = 1;
        return;
    }
    if (sender._text == "Move To Next Process" && (userRole == "PHYSICIAN" || userRole == "PHYSICIAN ASSISTANT")) {
        if (document.getElementById('chkElectronicDigitalSignature').checked == false) {
            DisplayErrorMessage('7490001');
            sender.set_autoPostBack(false);
            NotSaved();
            v = 1;
            return;
        }
    }
    showTime(sender, eventArgs);
    if(v==0)
      sender.set_autoPostBack(true);
    else
    {
        v == 0;
        __doPostBack('btnSaveAndClose');
    }
}


function OnClientbtnCancelClick(sender, eventArgs) {

    sender.set_autoPostBack(false);
    if ($find('btnSaveAndClose').get_enabled() == true) {
        if (!$($(top.window.document).find('iframe')[0].contentDocument).find("body").is('#dvdialogMenu'))
            $($(top.window.document).find('iframe')[0].contentDocument).find("body").append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
            '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
        dvdialog = $($(top.window.document).find('iframe')[0].contentDocument).find("body").find('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            position: {
                my: myPos,
                at: atPos

            },
            buttons: {
                "Yes": function () {

                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    $(dvdialog).dialog("close");
                    sessionStorage.setItem("Addendum_AutoSave", "true");
                    $find('btnSaveAndClose').click();
                },
                "No": function () {

                    $(dvdialog).dialog("close");
                    self.close();
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                }
            }
        });
    }
    else {
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
        }
        self.close();
    }
}

function Saved() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (sessionStorage.getItem("Addendum_AutoSave") != undefined && JSON.parse(sessionStorage.getItem("Addendum_AutoSave")) == true) {
        sessionStorage.setItem("Addendum_AutoSave", "false");
        self.close();
    }
}

function NotSaved() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function LoadAddendum() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}
