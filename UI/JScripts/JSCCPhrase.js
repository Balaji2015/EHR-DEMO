

$(document).ready(function () {
    localStorage.setItem("CCAndEandMAutosave", "false");
    StopLoadingImage();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    RefreshNotification('Notify');
   
    });
function OnSuccessSummaryBar(response) {
    var regex = /<BR\s*[\/]?>/gi;

    if (response != null) {

        top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = response.d[0];
        top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = response.d[1];
        top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = response.d[2];
        top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = response.d[3];
        top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = response.d[4];
        if (response.d[5].replace("Allergies :<br/>", "").length != 0)
            top.window.document.getElementById("Allergies_tooltp").innerText = response.d[5].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Allergies_tooltp").innerText = "";
        if (response.d[6].replace("Chief Complaints :<br/><br/>", "").length != 0)
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = response.d[6].replace(regex, "\n").split("&#xA;").join("\n") + "\n";
        else
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = "";
        if (response.d[7].replace("Problem List :<br/>", "").length != 0)
            top.window.document.getElementById("ProblemList_tooltp").innerText = response.d[7].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("ProblemList_tooltp").innerText = "";
        if (response.d[8].replace("Vitals :<br/>", "").length != 0)
            top.window.document.getElementById("Vitals_tooltp").innerText = response.d[8].replace(regex, "\n").replace("<br/><br/><br/>", "<br/>") + "\n";
        else
            top.window.document.getElementById("Vitals_tooltp").innerText = "";
        if (response.d[9].replace("Medication :<br/>", "").length != 0)
            top.window.document.getElementById("Medication_tooltp").innerText = response.d[9].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Medication_tooltp").innerText = "";
        RefreshOverallSummaryTooltip();
      

    }
    var sDtls = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
    document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
        "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
        window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
        window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];


}

function SavedSuccessfully() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    var splitvalues = window.parent.theForm.hdnTabClick.value.split('$#$');
    var which_tab = splitvalues[0];
    var screen_name;
    if (which_tab.indexOf('btn') > -1) {
        screen_name = 'MoveToButtonsClick';
    }
    else if (which_tab == 'first') {
        screen_name = '';
    }
    else {
        screen_name = 'EncounterTabClick';
    }
    if (splitvalues.length == 3 && splitvalues[2] == "Node")
        screen_name = 'PatientChartTreeViewNodeClick';
    SavedSuccessfully_NowProceed(screen_name);
    DisplayErrorMessage('170006');
    Autosave();
    localStorage.setItem("CCAndEandMAutosave","true");
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    AutoSaveSuccessful();
    DisableChartLevelAutoSave(); //BugID:52795
    if (JSON.parse(sessionStorage.getItem("EncCancel")) == true)
        sessionStorage.setItem("EncCancel", "false");
    //CAP-2678
    localStorage.setItem("IsSaveCompleted", true);
}
function PurposeOfVisit() {
    //CAP-783 Cannot read properties of null
    if (window?.parent?.document?.getElementById('hdnPurposeOfVisit_SaveRejected')?.value == "false") {
        $("#btnAdd")[0].disabled = false;
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    }
}

function ClearAll() {
    $find("cctmDLCChief_Complaints_txtDLC").clear();
    $find("ctmDLCHPI_Notes_txtDLC").clear();
    return false;
}

function EnableSave(e) {
    $("#btnAdd")[0].disabled = false;
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value = "CC / HPI";
    document.getElementById("Hidden1").value = "True";
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnccAutosave.value = true;
}

function Enable_OR_Disable() {
    var button2 = $("#btnAdd")[0].disabled;
    if (!button2)
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";
    localStorage.setItem("bSave", "false");
}

function FollowUpFormOpen() {
    window.showModalDialog('frmFollowUpEncounter.aspx', '');
}

function btnClear_Clicked(sender, args) {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        document.getElementById("ctmDLCChief_Complaints_txtDLC").value = "";
        document.getElementById("ctmDLCHPI_Notes_txtDLC").value = "";
            document.getElementById("txtcount").value = "0";
            $("#chkCurrentMedicationDocumented")[0].checked = true;
            chkCurrentMedicationDocumented_changed();
      
        $("#btnAdd")[0].disabled = false;
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        //drawChart();
        //CAP-1334
        setTimeout(function () {
            drawChart();
        }, 1000);
    } else {
       //drawChart();
         //CAP-1334
        setTimeout(function () {
            drawChart();
        }, 1000);
        return false;
    }
}

function Autosave() {
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    localStorage.setItem("bSave", "true");
}

function saveCC() {
    
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var now = new Date();
    var utc = now.toUTCString();
    var count = document.getElementById("txtcount").value;
    if ($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == "") {
        DisplayErrorMessage('170004');
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        AutoSaveUnsuccessful();
        return false;
    }
    else if ($("#chkCurrentMedicationDocumented")[0].checked == false && $("#cboCurrentMedicationDocumented")[0].selectedIndex < 1)
    {
        DisplayErrorMessage('210018');
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        AutoSaveUnsuccessful();
        return false;
    }
    if (count.trim() == "") {
        DisplayErrorMessage('210020');
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        AutoSaveUnsuccessful();
        return false;
    }
    document.getElementById("hdnLocalTime").value = utc;
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    return true;
}

function SetUpdatedCC(ccContent, ToolTipText) {
    
    var dox = window.parent.window.parent.window.document;
    top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = ccContent;

    var pnl = dox.all.ctl00_C5POBody_pnlCheifComplaints;
    var regex = /<BR\s*[\/]?>/gi;
    ccContent = ccContent.replace(regex, "\n").split("&#xA;").join("\n");
    top.window.document.getElementById("CheifComplaints_tooltp").innerText = ccContent + "\n";
    RefreshOverallSummaryTooltip();
   

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
    }
}


function OnClientClose(oWnd, args) {
    var arg = args.get_argument();
    if (arg) {
        var Physician_ID = arg.Physician_ID;
        if (Physician_ID != "0") {
            document.getElementById('hdnCopyPreviousPhysicianId').value = Physician_ID;
            document.getElementById('btnFloatingSummary').click();
            $("#btnAdd")[0].disabled = false;
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true
        }
    }
}

document.onkeyup = KeyCheck;

function KeyCheck(e) {
    var KeyID = (window.event) ? event.keyCode : e.keyCode;
    if (KeyID == 8 || KeyID == 46) {
        $("#btnAdd")[0].disabled = false;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true
    }

}

function CCPhraseLoad() {
   
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("textarea[id *= txtDLC]").removeClass('DlcClass');
    $('#cboFlowSheetType').addClass('Editabletxtbox');
    $('#cboFlowSheetPeriod').addClass('Editabletxtbox');
    
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
   
    window.parent.theForm.hdnSaveButtonID.value = "btnAdd";
}

function btnCopyCC_Clicked() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var btnCopyCC = $("#btnCopyCC")[0];
    var btnSave = $("#btnAdd")[0].disabled;
    if ($('#ctmDLCChief_Complaints_txtDLC')[0].value.trim() == "") {
        $("#btnAdd")[0].disabled = true;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
        drawChart();
    }
    if (!$("#btnAdd")[0].disabled) {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
         CopyPrevious();
         drawChart();
        return false;
    } else {
        drawChart();
        return true;
    }
}


function checkSavebutton() {
    document.getElementById("HdnCopyButton").value = "";
    var button2 = $("#btnAdd")[0];
    var btnCopyCheck = $("#btnCopyCC")[0];
    document.getElementById("HdnCopyButton").value = "trueValidate";
    document.getElementById("hdnTrueCheck").value = "true";
    testCopy = "true";
    button2.click();
    //var obj = new Array();
    //obj.push("Title=" + "ChiefComplaints");
    //obj.push("ErrorMessages=" + "There are unsaved changes.Do you want to save them?");
    //var result = openModal("frmValidationArea.aspx", 100, 300, obj, "MessageWindow");
    //var WindowName = $find('MessageWindow');
    //WindowName.add_close(OnClientCloseValidation);
}

function OnClientCloseValidation(oWindow, args) {
    document.getElementById("HdnCopyButton").value = "";
    var button2 = $("#btnAdd")[0];
    var btnCopyCheck = $("#btnCopyCC")[0];
    var arg = args.get_argument();
    if (arg) {
        var result = arg;
        if (result == "Yes") {
            document.getElementById("HdnCopyButton").value = "trueValidate";
            document.getElementById("hdnTrueCheck").value = "true";
            testCopy = "true";
            button2.click();
        } else if (result == "Cancel") {

        } else {
            document.getElementById("HdnCopyButton").value = "ClickNo";
            button2.disabled = true;
            document.getElementById("btnCopyCC").click();
        }
    }
}

function CheckTestClick() {
    document.getElementById("HdnCopyButton").value = "CheckSave";
    document.getElementById("Button2").click();
    return true;
}
function HideAllControls() {
    document.getElementById("CCPharse").style.display = 'none';
    document.getElementById("SummaryAlert").style.display = '';
}

function onCopyPrevious(errorCode) {

    if (errorCode == "") {
        PurposeOfVisit();
        $("#btnAdd")[0].disabled = false;

        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    }
    else {
        DisplayErrorMessage(errorCode);
        $("#btnAdd")[0].disabled = true;

        localStorage.setItem("bSave", "true");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }
    drawChart();
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}


function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
}
function enableAutoSave() {
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
}

function CopyPrevious() {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" &&
        localStorage.getItem("bSave") == "false") {
        event.preventDefault();
        event.stopPropagation();

        document.getElementById("HdnCopyButton").value = "";
        var btnAdd = $("#btnAdd")[0];
        var btnSave = $find('btnSave');
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        document.getElementById("HdnCopyButton").value = "trueValidate";
        document.getElementById("hdnTrueCheck").value = "true";
        testCopy = "true";
        disableAutoSave();
        btnAdd.click();
        return;

        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogCC');

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
        //            disableAutoSave();
        //            btnAdd.click();
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            document.getElementById("HdnCopyButton").value = "ClickNo";
        //            btnAdd.disabled = true;
        //            disableAutoSave();
        //            document.getElementById("btnCopyCC").click();
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
function AllowNumbers(evt) {
    evt = (evt) ? evt : window.event;
    $("#btnAdd")[0].disabled = false;
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    window.parent.parent.parent.parent.theForm.ctl00_hdnCurrentTab.value = "CC / HPI";
    document.getElementById("Hidden1").value = "True";
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function chkCurrentMedicationDocumented_changed(){
    if ($("#chkCurrentMedicationDocumented")[0].checked) {
        $("#cboCurrentMedicationDocumented").attr('disabled', 'disabled');
        $("#lblCurrentMedicationDocumented").removeClass("MandLabelstyle");
        $("#lblCurrentMedicationDocumented")[0].innerText = $("#lblCurrentMedicationDocumented")[0].innerText.replace("*", " ").trim();
        $("#lblCurrentMedicationDocumented")[0].style.color = "black";
        $("#cboCurrentMedicationDocumented")[0].selectedIndex = 0;
    }
    else {
        $("#cboCurrentMedicationDocumented").attr('disabled', false);
      
        $("#lblCurrentMedicationDocumented")[0].innerText += "*";
        $("#lblCurrentMedicationDocumented").addClass("MandLabelstyle");
        $("#lblCurrentMedicationDocumented").html($("#lblCurrentMedicationDocumented").html().replace("*", "<span class='manredforstar'>*</span>"));
    }
    $("#btnAdd")[0].disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    var chkCurrentMedicationDocumented = document.getElementById("chkCurrentMedicationDocumented");
    __doPostBack('chkCurrentMedicationDocumented', chkCurrentMedicationDocumented);
    //CAP-2135
    setTimeout(function () {
        drawChart();
    },500);
}
function cboCurrentMedicationDocumented_Changed() {
    $("#btnAdd")[0].disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
}
//function EnableAdd() {
//    if ($("#btnAdd")[0] != undefined) {
//        if ($("#btnAdd")[0].disabled == true) {
//            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
//                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
//                localStorage.setItem("bSave", "false");
//            }
//        }
//    }
//}
$(document).ready(function () {
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnccAutosave.value = false;
    var disable_autosave = false;
    if (sessionStorage.getItem("bCCSave") != null && sessionStorage.getItem("bCCSave") != undefined && sessionStorage.getItem("bCCSave") == "false") {//BugID:48506
        disable_autosave = true;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
    }
    
    if ($("#btnAdd")[0] != undefined) {
        if (document.getElementById("CheckSave").value == "true" && !disable_autosave) {
            $("#btnAdd")[0].disabled = false;
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
                localStorage.setItem("bSave", "false");
            }
        }
        else {
            if (disable_autosave) {
                sessionStorage.removeItem("bCCSave");
            }
            $("#btnAdd")[0].disabled = true;
           
        }
         
    }
   
    //if (document.getElementById("hdnDisableCurrentProcess").value == "false") {
    //    $("#btnAdd")[0].disabled = false;
    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    //    localStorage.setItem("bSave", "false");
    //}
    //else {
    //    $("#btnAdd")[0].disabled = true;
    //    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    //    localStorage.setItem("bSave", "true");
    //}
    if (document.getElementById("hdnDisableCurrentProcess").value == "true") {
        $("#btnAdd")[0].disabled = true;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
    }

    if (document.getElementById("hdnTechnician").value == "true") {
        $("#btnAdd")[0].disabled = true;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
    }
   
   
});