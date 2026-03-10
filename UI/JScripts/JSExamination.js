function clearall(sender, args) {
    if (DisplayErrorMessage('270002') == true) {
        $find('btnSave').set_enabled(true);
        sender.set_autoPostBack(true);
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null) {
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
        }
        var now = new Date();
        var utc = now.toUTCString();
        document.getElementById(GetClientId("hdnLocalTime")).value = utc;

        var cntrls = document.getElementById("divExam").getElementsByTagName("span");;
        for (var i = 0; i < cntrls.length; i++) {
            cntrls[i].style.color = "black";
            var cbo = document.getElementById(cntrls[i].id.replace("lbl", "cbo"));
            if (cbo != null && cbo != undefined)
                cbo.selectedIndex = 0;
            var dlc = document.getElementById(cntrls[i].id.replace("lbl", "DLC") + "_txtDLC");
            if (dlc != null && dlc != undefined)
                dlc.value = "";
        }
    }
    else
        sender.set_autoPostBack(false);
}
function clearcombo() {
    var cntrls = document.getElementById("divExam").getElementsByTagName("td");;
    for (var i = 0; i < cntrls.length; i++) {
        if (cntrls[i].childNodes.length != 0) {
            if (cntrls[i].childNodes[0].id != undefined) {
                if (cntrls[i].childNodes[0].id.startsWith("lbl") == true) {
                    var color = cntrls[i].childNodes[0].style.color;
                }
                if (cntrls[i].childNodes[0].id.startsWith("colorlbl") == true) {
                    document.getElementById(cntrls[i].childNodes[0].id.replace("colorlbl", "lbl")).style.color = cntrls[i].childNodes[0].style.color;
                }
            }
        }
    }
}
function AbnormalValidation(button, args) {

    AssignUTCTime();
    var cntrls = document.getElementById("divExam").getElementsByTagName("td");

    for (var i = 0; i < cntrls.length; i++) {
        if (cntrls[i].childNodes.length != 0) {
            if (cntrls[i].childNodes[0].id != undefined) {
                if (cntrls[i].childNodes[0].id.startsWith("cbo") == true) {
                    var ID = cntrls[i].childNodes[0].id;
                    if (ID != null) {
                        if (cntrls[i].childNodes[0].value != "Not Examined" && cntrls[i].childNodes[0].value != "Normal" &&
                            cntrls[i].childNodes[0].value != "No" && cntrls[i].childNodes[0].value != "No Abnormality Detected(NAD)" &&
                            cntrls[i].childNodes[0].value != "Examined") {
                            if (ID.replace("cbo", "DLC") != null) {
                                var txtValue = ID.replace("cbo", "DLC");
                                if (document.getElementById(txtValue.replace("_ClientState", "") + "_txtDLC").value.trim() == "") {
                                    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
                                    button.set_autoPostBack(false);
                                    SaveUnsuccessful();
                                    AutoSaveUnsuccessful();
                                    DisplayErrorMessage("270005");
                                    document.getElementById(txtValue.replace("_ClientState", "") + "_txtDLC").focus();
                                    break;
                                } else {

                                    button.set_autoPostBack(true);

                                }
                            }
                        }

                    }
                }
            }
        }
    }

    return false;
}
function EnableSave() {
    $("#btnSave")[0].disabled = false;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
}
function ChangedGreenIndex(cbo) {
    var lblctrl = document.getElementById(cbo.id.replace("cbo", "lbl"));
    if (cbo.selectedIndex == 3) {
        document.getElementById(lblctrl.id).style.color = "red !important";
    } else {
        document.getElementById(lblctrl.id).style.color = "green !important";
    }
    $find('btnSave').set_enabled(true);
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    ChangedIndex(cbo);
    EnableSave();
}
function ChangedIndex(cbo) {
    var Obj;
    var dlc = document.getElementById(cbo.id.replace("cbo", "DLC") + "_txtDLC");
    var sMyTabName = document.getElementById('hdnCategory').value;
    var taglist = document.getElementById('hdnDLCTag').value.split("|~|");
    if (sMyTabName.toLowerCase().indexOf("general") > -1) {
        if (cbo.selectedOptions[0].value.indexOf("NAD") > -1) {
            if (dlc.value.trim() == "") {
                for (var j = 0; j < taglist.length; j++) {
                    var tagsplit = taglist[j].split(",__");
                    if ((dlc.id.indexOf(tagsplit[1]) > -1) && (tagsplit[0].trim() != "")) {
                        dlc.value = tagsplit[0].trim();
                        break;
                    }
                }
            }

        } else if (cbo.selectedOptions[0].value.indexOf("NAD") == -1) {
            for (var j = 0; j < taglist.length; j++) {
                var tagsplit = taglist[j].split(",__");
                if (dlc.id.indexOf(tagsplit[1]) > -1) {
                    if (dlc.value.trim().length == tagsplit[0].trim().length) {
                        if (dlc.value.trim() != "") {
                            if (tagsplit[0].trim() != "") {
                                if (dlc.value.trim().indexOf(tagsplit[0].trim()) > -1) {
                                    dlc.value = dlc.value.trim().replace(tagsplit[0].trim(), "");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    var cntrls = document.getElementById("divExam").getElementsByTagName("td");
    for (var i = 0; i < cntrls.length; i++) {
        if (cntrls[i].childNodes.length != 0) {
            if (cntrls[i].childNodes[0].id != undefined) {
                if (cntrls[i].childNodes[0].id.startsWith("lbl") == true) {
                    var color = cntrls[i].childNodes[0].style.color;
                }
                if (cntrls[i].childNodes[0].id.startsWith("cbo") == true) {
                    if (cntrls[i].childNodes[0].value != "Not Examined" && cntrls[i].childNodes[0].value != "Normal" &&
                        cntrls[i].childNodes[0].value != "No" && cntrls[i].childNodes[0].value != "No Abnormality Detected(NAD)" &&
                        cntrls[i].childNodes[0].value != "Examined") {
                        var txtValue = cntrls[i].childNodes[0].id;
                        if (color == "green") {
                            document.getElementById("hdnGreenLabel").value = cntrls[i].childNodes[0].id;
                        }
                        document.getElementById(txtValue.replace("cbo", "lbl")).style.color = "red !important";
                    } else {
                        var txtValue = cntrls[i].childNodes[0].id;
                        if (color == "red" && cntrls[i].childNodes[0].value != "Abnormal") {
                            if (document.getElementById("hdnGreenLabel").value == txtValue) {
                                document.getElementById(txtValue.replace("cbo", "lbl")).style.color = "green !important";
                            } else {

                                document.getElementById(txtValue.replace("cbo", "lbl")).classList.remove('manredforstar');
                                document.getElementById(txtValue.replace("cbo", "lbl")).classList.add('spanstyle');


                            }
                        } else if (color != "green") {

                            document.getElementById(txtValue.replace("cbo", "lbl")).classList.remove('manredforstar');


                            document.getElementById(txtValue.replace("cbo", "lbl")).classList.add('spanstyle');



                        }

                    }
                }
            }
        }
    }

    $find('btnSave').set_enabled(true);
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    EnableSave();
}
function Clear(a) {
    document.getElementById("txtDLC" + a).value = "";
}
function OnClientClose(oWnd, args) {
    var arg = args.get_argument();
    if (arg) {
        var Physician_ID = arg.Physician_ID;
        if (Physician_ID != "0") {
            document.getElementById('hdnCopyPreviousPhysicianId').value = Physician_ID;
            document.getElementById('btnFloatingSummary').click();
            $find('btnSave').set_enabled(true);
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
            EnableSave();
        }
    }
}
function ShowLoading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}
function RefreshFloatingSummary() {
    var dox = window.parent.window.parent.window.document;
    var iFrame = dox.getElementsByTagName("iframe");
    if (iFrame.length > 0) {
        var str = iFrame[0].src;
        var n = str.indexOf("frmFollowUpEncounter.aspx");
        if (n >= 0) {
            iFrame[0].src = iFrame[0].src;
        } else {

        }
    } else {
        // console.log("Error in finding the Floating Summary");
    }
}

function SavedSuccessfully() {

    var splitvalues = window.parent.parent.theForm.hdnTabClick.value.split('$#$');
    var which_tab = splitvalues[0];

    var screen_name;
    if (which_tab.indexOf('btn') > -1) {
        screen_name = 'MoveToButtonsClick';
    }
    else if (which_tab == 'first') {
        screen_name = '';
    }
    else if (which_tab != "first" && which_tab != "CC / HPI" && which_tab != "QUESTIONNAIRE" && which_tab != "PFSH" && which_tab != "ROS" && which_tab != "VITALS" && which_tab != "EXAM" && which_tab != "TEST" && which_tab != "ASSESSMENT" && which_tab != "ORDERS" && which_tab != "eRx" && which_tab != "SERV./PROC. CODES" && which_tab != "PLAN" && which_tab != "SUMMARY")
        screen_name = "ExamTabClick";
    else
        screen_name = "EncounterTabClick";

    if (splitvalues.length == 3 && splitvalues[2] == "Node")
        screen_name = 'PatientChartTreeViewNodeClick';

    DisplayErrorMessage("270001");
    localStorage.setItem("bSave", "true");

    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    if ($('#btnSave') != null && $('#btnSave')[0].disabled == false)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    else
        if ($('#btnSave') != null && $('#btnSave')[0].disabled == true)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        else
            if (document.getElementById('btnSave_ClientState').disabled == false)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            else
                if (document.getElementById('btnSave_ClientState').disabled == true)
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";


}
function Loading() { top.window.document.getElementById('ctl00_Loading').style.display = "none"; }
function Autosave() {
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    DisplayErrorMessage('270001');


}
function AssignUTCTime() {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var utcnew = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utcnew;
    return true;
}
function btnCopyPrevious_Clicked(sender, args) {
    var btnSave = $find('btnSave');
    if (btnSave._enabled) {
        checkSavebutton();
        sender.set_autoPostBack(false);
    }
    else {
        sender.set_autoPostBack(true);
    }



}
function checkSavebutton() {
    var button2 = $find('btnSave');
    var btnCopyCheck = $find('btnPastEncounter');
    var button1 = document.getElementById('Button2');
    document.getElementById("HdnCopyButton").value = "trueValidate";
    document.getElementById("hdnTrueCheck").value = "true";
    testCopy = "true";
    button2.click();
    document.getElementById("HdnCopyButton").value = "";
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
    var button1 = document.getElementById('Button2');

    var arg = args.get_argument();
    if (arg) {
        var result = arg;
        if (result == "Yes") {
            document.getElementById("HdnCopyButton").value = "trueValidate";
            document.getElementById("hdnTrueCheck").value = "true";
            testCopy = "true";
            button2.click();
            oWindow.remove_close(OnClientCloseValidation);
            oWindow.close();
            document.getElementById("HdnCopyButton").value = "";

        } else if (result == "Cancel") {
            document.getElementById("HdnCopyButton").value = "CheckSave";
            oWindow.remove_close(OnClientCloseNavigation);
            oWindow.close();
            document.getElementById("HdnCopyButton").value = "";
        }
        else if (result == "No") {
            document.getElementById("HdnCopyButton").value = "";
            button2.set_enabled(false);

            document.getElementById("btnCopyPrevious").click();

            oWindow.remove_close(OnClientCloseValidation);
            oWindow.close();

        }
    }
}
function CallBack(e) {
    if (e.ctrlKey && e.keyCode == 86)
        $find('btnSave').set_enabled(true);
    EnableSave();
}
function SetLabelColorAfterSave() {
    if ($find('btnSave') != null && $find('btnSave').get_enabled() == true)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    else
        if (document.getElementById('btnSave_ClientState').disabled == false)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    var cboList = $('select');
    for (var i = 0; i < cboList.length; i++) {
        if (cboList[i].selectedIndex == 3) {
            var lblctrl = document.getElementById(cboList[i].id.replace("cbo", "lbl"));
            document.getElementById(lblctrl.id).style.color = "red !important";
        }
    }
}
var enc_id;
var phy_id;
var human_id;
function Exam_Load() {


    
    $("textarea").bind("keydown", function (e) {//BUGID:45541

        insertTab(this, event);
    });
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    $('#btnSave')[0].disabled = true;

    var type = document.title;
    if (type == "General With Specialty") {

        SetPhysicianSpecificVisibility();
    }
    else {

        SetPhysicianSpecificFocusedVisibility();

    }

    $("textarea").addClass('spanstyle');
    //Jira CAP-1194
    //$("label").addClass('spanstyle');
    //Jira CAP-1493
    if (type != "General With Specialty") {
        $("label").addClass('spanstyle');
    }
    $("select").addClass('spanstyle');
    $('#tblPlan tr td').addClass('spanstyle');

    $("option").addClass('spanstyle');
}

function SetPhysicianSpecificFocusedVisibility() {

    var rows = $("#tblExam tbody tr");
    var list = '';
    var cookieLst = document.cookie.split(';');
    for (var l = 0; l < cookieLst.length; l++) {
        if (cookieLst[l].indexOf("Human_Details") > -1)
            list = cookieLst[l].split("=")[1];

    }
    var value = window.location.search.toString();
    if (value != undefined)
        enc_id = value.split('&')[0];

    var gender = '';
    if (list.indexOf("|") > -1 && list.split('|').length > 3)
        gender = list.split('|')[4].split(':')[1];
    var type = document.title;
    physician_id = value.split('&')[2];
    var Process = value.split('&')[1];
    human_id = value.split('&')[3];
    if (gender.trim() == "F") {
        gender = "Female";
    }
    else if (gender.trim() == "M") {
        gender = "Male";
    }
    for (var i = 0; i < rows.length; i++) {
        if ($(rows[i]).attr("data-physician-ids") != undefined) {
            var arry = new Array();
            for (var n = 0; n < $(rows[i]).attr("data-physician-ids").split(',').length; n++) {
                arry[n] = $(rows[i]).attr("data-physician-ids").split(',')[n].trim();

            }
            if ($.inArray(physician_id, arry) < 0) {
                rows[i].style.display = "none";
            }
            if (Process.toUpperCase() != 'SCRIBE_PROCESS' && Process.toUpperCase() != 'AKIDO_SCRIBE_PROCESS' && Process.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && Process.toUpperCase() != "SCRIBE_CORRECTION" && Process.toUpperCase() != 'DICTATION_REVIEW' && Process.toUpperCase() != 'CODER_REVIEW_CORRECTION' && Process.toUpperCase() != 'PROVIDER_PROCESS' && Process.toUpperCase() != "PROVIDER_REVIEW_CORRECTION" && Process.toUpperCase() != 'TRANSCRIPT_PROCESS' && Process.toUpperCase() != 'TRANSCRIPT_QC_PROCESS' && Process.toUpperCase() != 'AKIDO_SCRIBE_QC_PROCESS') {
                $('select').attr('disabled', 'disabled');
                $('select').css('backgroundColor', '#ebebe4');
                var len = $("#tblExam").find("textarea").length;
                var x = new Date();

                var y = new Date();
                $('textarea').attr('disabled', true);
                $('input').attr('disabled', true);
                $('#btnSave')[0].disabled = true;
                $('#btnCopyPrevious')[0].disabled = true;
                $('#btnClearAll')[0].disabled = true;
                $("a").attr('disabled', true);
                $("a").attr('onclick', false);
                $("a").css('backgroundColor', '#6D7777');
            }
            else {
                $('select').removeAttr('disabled');
                $('textarea').removeAttr('disabled');
                $('input').removeAttr('disabled');
                $('#btnSave')[0].disabled = true;
                $('#btnCopyPrevious')[0].disabled = false;
                $('#btnClearAll')[0].disabled = false;
                var len = $("#tblExam").find("textarea").length;
                var x = new Date();

                var y = new Date();
            }

            //Jira CAP-1600
            //if ($(rows[i]).attr("gender") != undefined) {
            if ($(rows[i]).attr("gender") != undefined && (gender.toUpperCase() == "MALE" || gender.toUpperCase() == "FEMALE")) {
                if ($(rows[i]).attr("gender").toUpperCase() != gender.toUpperCase() && $(rows[i]).attr("gender") != "") {
                    rows[i].style.display = "none";
                }
            }

        }

    }

    LoadControl("", type);

}
function SetPhysicianSpecificVisibility() {
    var rows = $("#tblExam tbody tr");
    var list = '';
    var cookieLst = document.cookie.split(';');
    for (var l = 0; l < cookieLst.length; l++) {
        if (cookieLst[l].indexOf("Human_Details") > -1)
            list = cookieLst[l].split("=")[1];
    }
    var value = window.location.search.toString();
    if (value != undefined)
        enc_id = value.split('&')[0].trim('?');

    var gender = '';
    if (list.indexOf("|") > -1 && list.split('|').length > 3)
        gender = list.split('|')[4].split(':')[1];
    var type = document.title;
    physician_id = value.split('&')[2];
    var Process = value.split('&')[1];
    human_id = value.split('&')[3];
    if (gender.trim() == "F") {
        gender = "Female";
    }
    else if (gender.trim() == "M") {
        gender = "Male";
    }
    for (var i = 0; i < rows.length; i++) {
        if ($(rows[i]).attr("data-physician-ids") != undefined) {
            var arry = new Array();

            for (var n = 0; n < $(rows[i]).attr("data-physician-ids").split(',').length; n++) {
                arry[n] = $(rows[i]).attr("data-physician-ids").split(',')[n].trim();
            }
            if ($.inArray(physician_id, arry) < 0) {
                rows[i].style.display = "none";
            }
            if (Process.toUpperCase() != 'SCRIBE_PROCESS' && Process.toUpperCase() != 'AKIDO_SCRIBE_PROCESS' && Process != "SCRIBE_REVIEW_CORRECTION" && Process.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && Process.toUpperCase() != "SCRIBE_CORRECTION" && Process.toUpperCase() != 'DICTATION_REVIEW' && Process.toUpperCase() != 'CODER_REVIEW_CORRECTION' && Process.toUpperCase() != 'PROVIDER_PROCESS' && Process.toUpperCase() != 'PROVIDER_REVIEW_CORRECTION' && Process.toUpperCase() != 'TRANSCRIPT_PROCESS' && Process.toUpperCase() != 'TRANSCRIPT_QC_PROCESS' && Process.toUpperCase() != 'AKIDO_SCRIBE_QC_PROCESS') {
                $('select').attr('disabled', 'disabled');
                $('select').css('backgroundColor', '#ebebe4');
                $('input').attr('disabled', true);
                $('#btnSave')[0].disabled = true;
                $('#btnCopyPrevious')[0].disabled = true;
                $('#btnClearAll')[0].disabled = true;
                var len = $("#tblExam").find("textarea").length;

                $('textarea').attr('disabled', true);
                $("a").attr('disabled', true);
                $("a").attr('onclick', false);
                $("a").css('backgroundColor', '#6D7777');
            }
            else {
                $('select').removeAttr('disabled');
                $('textarea').removeAttr('disabled');
                $('input').removeAttr('disabled');
                var len = $("#tblExam").find("textarea").length;

                $('#btnSave')[0].disabled = true;
                $('#btnCopyPrevious')[0].disabled = false;
                $('#btnClearAll')[0].disabled = false;
            }
            //Jira CAP-1600
            //if ($(rows[i]).attr("gender") != undefined) {
            if ($(rows[i]).attr("gender") != undefined && (gender.toUpperCase() == "MALE" || gender.toUpperCase() == "FEMALE")) {
                if ($(rows[i]).attr("gender").toUpperCase() != gender.toUpperCase() && $(rows[i]).attr("gender") != "") {
                    rows[i].style.display = "none";

                }
            }
        }
    }
    LoadControl("", type);

    //Jira CAP-1194 -  also adding a new attribute (phyname_withcolor) in label element for GeneralWithSpecialty.html in the htmlgenerate program

    var Changelabelcolor = $("#tblExam tbody tr:not([style='display: none;'])");

    for (var i = 0; i < Changelabelcolor.length; i++) {

        if ($(Changelabelcolor[i]).attr("data-physician-ids") != undefined) {
            if ($(Changelabelcolor[i])[0].children[0].children[0].tagName.toUpperCase() == "LABEL" && $(Changelabelcolor[i])[0].children[0].children[0].attributes[3] != undefined) {
                var temp_phyname_withcolor = $(Changelabelcolor[i])[0].children[0].children[0].attributes[3].value;

                var phyname_withcolor = temp_phyname_withcolor.slice(temp_phyname_withcolor.indexOf(physician_id), temp_phyname_withcolor.length).split(",")[0];

                if (phyname_withcolor != undefined && phyname_withcolor != null) {
                    $(Changelabelcolor[i])[0].children[0].children[0].style.removeProperty("color");
                    $(Changelabelcolor[i])[0].children[0].children[0].classList.remove("spanstyle");
                    $(Changelabelcolor[i])[0].children[0].children[0].classList.remove("lblcolor");

                    if (phyname_withcolor.split("~")[1] == "BLACK") {
                        $(Changelabelcolor[i])[0].children[0].children[0].style.setProperty("color","black");
                        $(Changelabelcolor[i])[0].children[0].children[0].classList.add('spanstyle');
                    }
                    else if (phyname_withcolor.split("~")[1] == "GREEN") {
                        $(Changelabelcolor[i])[0].children[0].children[0].style.setProperty("color", "green");
                        $(Changelabelcolor[i])[0].children[0].children[0].classList.add('lblcolor');
                    }
                }
            }
        }
    }

}
function SaveExam() {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var now = new Date();
    var type = document.title;
    var utc = now.toUTCString();
    var rows = $("#tblExam tbody tr");
    var value = window.location.search.toString();
    if (value != undefined)
        enc_id = value.split('&')[0];
    phy_id = value.split('&')[4];
    user_name = value.split('&')[5];
    human_id = value.split('&')[3];

    var arry = new Array();
    var ExamList = function () {
        this.System = "";
        this.Status = "";
        this.Category = type;
        this.Notes = "";
        this.Time = new Date(utc);
        this.Version = 0;
        this.Condition = "";
        this.ExamId = "";
        this.createdby = "";
        this.createddandt = new Date();
    }

    if (type == "General With Specialty") {
        for (var i = 0; i < rows.length; i++) {
            var objExamlist = new ExamList();
            if (($(rows[i])[0].style.display) != "none") {
                if ($(rows[i])[0].className != "System_name") {
                    if ($(rows[i])[0].attributes[1] != undefined && $(rows[i])[0].attributes[1].name == "system_name") {

                        objExamlist.System = $(rows[i])[0].attributes[1].value.replace("\n", "").replace("\n", "").trim();
                        if ($(rows[i])[0].children[1].firstElementChild != null) {
                            objExamlist.Status = $(rows[i])[0].children[1].firstElementChild.value;
                        }
                        else {
                            objExamlist.Status = "";
                        }
                        objExamlist.Notes = $($(rows[i])[0].children[2].firstElementChild.parentNode).find('textarea')[0].value;
                        if (objExamlist.Status == "Abnormal" && objExamlist.Notes.trim() == "") {

                            alert("Please enter the notes if the status is Abnormal or Yes");
                            $(rows[i])[0].children[2].firstElementChild.focus();
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            AutoSaveUnsuccessful();
                            return;
                        }
                        if ($(rows[i])[0].children[3].textContent.trim() != "") {
                            objExamlist.Version = $(rows[i])[0].children[3].textContent.split('~')[0].trim();
                            objExamlist.ExamId = $(rows[i])[0].children[3].textContent.split('~')[1].trim();
                            objExamlist.createdby = $(rows[i])[0].children[3].textContent.split('~')[2].trim();
                            objExamlist.createddandt = $(rows[i])[0].children[3].textContent.split('~')[3].trim();
                        }
                        else {
                            objExamlist.Version = 0;
                            objExamlist.ExamId = 0;
                            objExamlist.createdby = "";
                            objExamlist.createddandt = new Date();
                        }
                        objExamlist.Time = new Date(utc);
                        objExamlist.Condition = $(rows[i])[0].children[0].innerText.replace("\n", "").replace("\n", "").trim();
                    }
                    else {
                        objExamlist.System = $(rows[i])[0].children[0].innerText.replace("\n", "").replace("\n", "").trim();
                        if ($(rows[i])[0].children[1].firstElementChild != null) {
                            objExamlist.Status = $(rows[i])[0].children[1].firstElementChild.value;
                        }
                        else {
                            objExamlist.Status = "";
                        }
                        objExamlist.Notes = $($(rows[i])[0].children[2].firstElementChild.parentNode).find('textarea')[0].value;
                        if (objExamlist.Status == "Abnormal" && objExamlist.Notes.trim() == "") {
                            alert("Please enter the notes if the status is Abnormal or Yes");
                            $(rows[i])[0].children[2].firstElementChild.focus();
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            AutoSaveUnsuccessful();
                            return;
                        }
                        if ($(rows[i])[0].children[3].textContent.trim() != "") {
                            objExamlist.Version = $(rows[i])[0].children[3].textContent.split('~')[0].trim();
                            objExamlist.ExamId = $(rows[i])[0].children[3].textContent.split('~')[1].trim();
                            objExamlist.createdby = $(rows[i])[0].children[3].textContent.split('~')[2].trim();
                            objExamlist.createddandt = $(rows[i])[0].children[3].textContent.split('~')[3].trim();
                        }
                        else {
                            objExamlist.Version = 0;
                            objExamlist.ExamId = 0;
                            objExamlist.createdby = "";
                            objExamlist.createddandt = new Date();
                        }
                        objExamlist.Time = new Date(utc);
                        objExamlist.Condition = "";

                    }
                    arry[arry.length++] = objExamlist;
                }
            }

        }
        localStorage.setItem("bSave", "true");
        $.ajax({
            type: "POST",
            url: "WebServices/ExamService.asmx/SaveExamination",
            data: JSON.stringify({
                "data": arry,
            }),
            contentType: "application/json;charset=utd-8",
            async: true,
            success: function (data) {

                var objdata = $.parseJSON(data.d);
                if (objdata != null) {
                    for (var i = 0; i < objdata.length; i++) {
                        if ($("#tblExam").find("label:contains('" + objdata[i].System_Name + "')")[0] != undefined && objdata[i].Condition_Name == "") {
                            var lbl = $("#tblExam").find("label:contains('" + objdata[i].System_Name + "')");
                            for (var k = 0; k < lbl.length; k++) {
                                if (lbl[k].textContent == objdata[i].System_Name) {
                                    controlname = lbl[k];
                                }
                            }
                            if (controlname.parentNode.nextElementSibling.children[0] != undefined) {
                                controlname.parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                                if (objdata[i].Status == "Not Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                                }
                                else if (objdata[i].Status == "Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                                }
                                else if (objdata[i].Status == "No Abnormality Detected(NAD)") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                                }
                                else if (objdata[i].Status == "Abnormal") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 3;
                                    document.getElementById(controlname.id).classList.remove('spanstyle');
                                    document.getElementById(controlname.id).classList.add('manredforstar');



                                }
                            }
                            $(controlname.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Examination_Notes;
                            controlname.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~' + objdata[i].Id + '~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                        }
                        else {
                            var lbl = $("#tblExam").find("label:contains('" + objdata[i].Condition_Name + "')");
                            var controlname;
                            for (var k = 0; k < lbl.length; k++) {
                                if (lbl[k].parentElement.parentElement.attributes.getNamedItem('system_name') != null) {
                                    if (lbl[k].parentElement.parentElement.attributes.getNamedItem('system_name').value == objdata[i].System_Name) {
                                        controlname = lbl[k];
                                    }
                                }
                            }
                            if (controlname.parentNode.nextElementSibling.children[0] != undefined) {
                                controlname.parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                                if (objdata[i].Status == "Not Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                                }
                                else if (objdata[i].Status == "Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                                }
                                else if (objdata[i].Status == "No Abnormality Detected(NAD)") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                                }
                                else if (objdata[i].Status == "Abnormal") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 3;

                                    document.getElementById(controlname.id).classList.remove('spanstyle');
                                    document.getElementById(controlname.id).classList.add('manredforstar');



                                }
                            }
                            $(controlname.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Examination_Notes;
                            controlname.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~' + objdata[i].Id + '~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                        }
                    }
                }
                $('#btnSave')[0].disabled = true;
                SavedSuccessfully();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                AutoSaveSuccessful();
                //CAP-2678
                localStorage.setItem('IsSaveCompleted', true);
            },
            error: function OnError(xhr) {
                AutoSaveUnsuccessful();
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    //CAP-798 Unexpected end of JSON input
                    if (isValidJSON(xhr.responseText)) {
                        var log = JSON.parse(xhr.responseText);

                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //               "Message: " + log.Message);

                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;
                    }
                    else {
                         alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry.");
                    }

                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }

        });
    }
    else {
        for (var i = 0; i < rows.length; i++) {
            var objExamlist = new ExamList();
            if (($(rows[i])[0].style.display) != "none") {
                if ($(rows[i])[0].className == "System_name") {
                    var System_name = $(rows[i])[0].children[0].innerText.replace("\n", "").replace("\n", "").trim();
                }
                else {
                    objExamlist.System = System_name.replace("\n", "").replace("\n", "").trim();
                    objExamlist.Condition = $(rows[i])[0].children[0].innerText.replace("\n", "").replace("\n", "").trim();
                    if ($(rows[i])[0].children[1].firstElementChild != null) {
                        objExamlist.Status = $(rows[i])[0].children[1].firstElementChild.value;
                    }
                    else {
                        objExamlist.Status = "";
                    }
                    objExamlist.Notes = $($(rows[i])[0].children[2].firstElementChild.parentNode).find('textarea')[0].value;
                    if (objExamlist.Status == "Abnormal" && objExamlist.Notes.trim() == "") {

                        alert("Please enter the notes if the status is Abnormal or Yes");
                        $(rows[i])[0].children[2].firstElementChild.focus();
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        AutoSaveUnsuccessful();
                        return;
                    }
                    if ($(rows[i])[0].children[3].textContent.trim() != "") {
                        objExamlist.Version = $(rows[i])[0].children[3].textContent.split('~')[0].trim();
                        objExamlist.ExamId = $(rows[i])[0].children[3].textContent.split('~')[1].trim();
                        objExamlist.createdby = $(rows[i])[0].children[3].textContent.split('~')[2].trim();
                        objExamlist.createddandt = $(rows[i])[0].children[3].textContent.split('~')[3].trim();

                    }
                    else {
                        objExamlist.Version = 0;
                        objExamlist.ExamId = 0;
                        objExamlist.createdby = "";
                        objExamlist.createddandt = new Date();
                    }
                    objExamlist.Time = new Date(utc);

                    arry[arry.length++] = objExamlist;
                }
            }

        }
        localStorage.setItem("bSave", "true");
        $.ajax({
            type: "POST",
            url: "WebServices/ExamService.asmx/SaveExamination",
            data: JSON.stringify({
                "data": arry,
            }),
            contentType: "application/json;charset=utd-8",
            async: true,
            success: function (data) {
                var objdata = $.parseJSON(data.d);
                if (objdata != "[]") {
                    var i = 0;
                    var tblExambody = $("#tblExam tr");
                    for (var i = 0; i < objdata.length; i++) {
                        var j = 0;
                        objdata[i].Condition_Name = objdata[i].Condition_Name.replace(/'/g, "\\'");

                        var lbl = $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')");
                        if (lbl.length > 1) {
                            for (var k = 0; k < lbl.length; k++) {
                                if (lbl[k].parentElement.parentElement.attributes.getNamedItem('system_name').value == objdata[i].System_Name) {
                                    if (objdata[i].Condition_Name == lbl[k].innerText)//Added by naveena for bug_id=36763
                                    {
                                        j = k;
                                    }
                                }
                            }
                        }
                        else { j = 0; }
                        if ($("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.children[0] != undefined) {
                            $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                            if (objdata[i].Status == "Not Examined") {
                                $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.children[0].selectedIndex = 0;
                            }
                            else if (objdata[i].Status == "Examined") {
                                $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.children[0].selectedIndex = 1;
                            }
                            else if (objdata[i].Status == "No Abnormality Detected(NAD)") {
                                $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.children[0].selectedIndex = 2;
                            }
                            else if (objdata[i].Status == "Abnormal") {
                                $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.children[0].selectedIndex = 3;


                                document.getElementById(lbl[j].id).classList.remove('spanstyle');
                                document.getElementById(lbl[j].id).classList.add('manredforstar');




                            }
                        }
                        $($("#tblExam").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Examination_Notes;
                        $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~' + objdata[i].Id + '~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                    }
                }
                $('#btnSave')[0].disabled = true;
                SavedSuccessfully();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                AutoSaveSuccessful();
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
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                AutoSaveUnsuccessful();
            }

        });

    }


}

function CopyPrevious() {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
        event.preventDefault();
        event.stopPropagation();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        disableAutoSave();
        SaveExam();
        CopyPreviousExam();
        return;
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogFocused');

        //if (!dvdialog) {
        //    dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogExam');
        //}

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
        //            disableAutoSave();
        //            SaveExam();
        //            CopyPreviousExam();
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            disableAutoSave();
        //            CopyPreviousExam();
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
        CopyPreviousExam();
    }
}


function CopyPreviousExam() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var type = document.title;
    var LoadType = "CopyPrevious";
    LoadControl(LoadType, type);
}
function chkSystem_CheckedChanged() {
    $('#btnSave')[0].disabled = false;
    var lfckv = document.getElementById("chkSystem").checked;
    if (lfckv) {

        var combobox = $('select.combo');
        var classname = "";
        for (var i = 0; i <= combobox.length; i++) {
            classname = "";
            if ($('select.combo')[i] != undefined) {
                if (document.getElementById($('select.combo')[i].id.replace("cbo", "lbl")).classList != null) {
                    if (document.getElementById($('select.combo')[i].id.replace("cbo", "lbl")).classList[0] != undefined) {
                        classname = document.getElementById($('select.combo')[i].id.replace("cbo", "lbl")).classList[0];
                    }
                }
                if (classname != "lblcolor") {
                    var combo = document.getElementById($('select.combo')[i].id);
                    if ($(combo).find('option:selected').val() != 'Examined' && $(combo).find('option:selected').val() != 'Abnormal') {
                        // $($(combo).find('option')[2]).prop('selected', true); For Bug Id: 64799
                        //Status
                        if (combo.attributes.getNamedItem("normal_system_status").value != undefined) {
                            var Status = "";
                            var countList = combo.attributes.getNamedItem("normal_system_status").value.split('|').length;
                            for (var j = 0; j < countList; j++) {
                                if (combo.attributes.getNamedItem("normal_system_status").value.split('|')[j].indexOf(physician_id) > -1) {
                                    //if (combo.attributes.getNamedItem("normal_system_status").value.split('|')[j].indexOf("MTAMA") > -1) {
                                    if (combo.attributes.getNamedItem("normal_system_status").value.split('|')[j].split('$')[1] != undefined) {
                                        Status = combo.attributes.getNamedItem("normal_system_status").value.split('|')[j].split('$')[1];
                                        $(combo)[0].value = Status;
                                        break;
                                    }
                                    else {
                                        $(combo)[0].value = $(combo).find('option')[2].value;
                                    }
                                }
                                else {
                                    $(combo)[0].value = $(combo).find('option')[2].value;
                                }
                            }
                        }
                        else {
                            $(combo)[0].value = $(combo).find('option')[2].value;
                        }
                        //Notes
                        if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden') != undefined) {
                            var lstPhysician;
                            var OtherDescription = "";
                            var countList = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|').length;
                            for (var j = 0; j < countList; j++) {
                                if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].indexOf(physician_id) > -1) {
                                    if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1] != undefined) {
                                        OtherDescription = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1];
                                        if ($(combo.parentElement.nextElementSibling).find('textarea')[0].value != "") {
                                            if (OtherDescription == $(combo.parentElement.nextElementSibling).find('textarea')[0].value) {
                                                $(combo.parentElement.nextElementSibling).find('textarea')[0].value = OtherDescription;
                                            }
                                        } else {
                                            $(combo.parentElement.nextElementSibling).find('textarea')[0].value = OtherDescription;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

    }
    else {
        var combobox = $('select.combo');
        var classname = "";
        for (var i = 0; i <= combobox.length; i++) {
            if ($('select.combo')[i] != undefined) {
                classname = "";
                if (document.getElementById($('select.combo')[i].id.replace("cbo", "lbl")).classList != null) {
                    if (document.getElementById($('select.combo')[i].id.replace("cbo", "lbl")).classList[0] != undefined) {
                        classname = document.getElementById($('select.combo')[i].id.replace("cbo", "lbl")).classList[0];
                    }
                }

                if (classname != "lblcolor") {
                    var combo = document.getElementById($('select.combo')[i].id);
                    if ($(combo).find('option:selected').val() == 'No Abnormality Detected(NAD)') {
                        //$($(combo).find('option')[0]).prop('selected', true);
                        $(combo)[0].value = $(combo).find('option')[0].value;
                        if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden') != undefined) {
                            var lstPhysician;
                            var OtherDescription = "";
                            var countList = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|').length;
                            for (var j = 0; j < countList; j++) {
                                if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].indexOf(physician_id) > -1) {
                                    if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1] != undefined) {
                                        OtherDescription = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1];
                                        if ($(combo.parentElement.nextElementSibling).find('textarea')[0].value != "") {
                                            if (OtherDescription == $(combo.parentElement.nextElementSibling).find('textarea')[0].value) {
                                                $(combo.parentElement.nextElementSibling).find('textarea')[0].value = "";
                                            }
                                        } else {
                                            $(combo.parentElement.nextElementSibling).find('textarea')[0].value = "";
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    EnableSave();
}
function ClearSystem() {

    if (DisplayErrorMessage('270002') == true) {
        $('select').prop('selectedIndex', 0);
        document.getElementById("chkSystem").checked = false;
        $('textarea').val("");
        for (var i = 0; i < $("select").length ; i++) {
            if (document.getElementById($('select')[i].id.replace("cbo", "lbl")).style != null) {

                //Jira CAP-1194
                if (!document.getElementById($('select')[i].id.replace("cbo", "lbl")).classList.contains("lblcolor")) {
                    document.getElementById($('select')[i].id.replace("cbo", "lbl")).classList.add('spanstyle');
                }
                    document.getElementById($('select')[i].id.replace("cbo", "lbl")).classList.remove('manredforstar');
                
            }
        }
        $('#btnSave')[0].disabled = false;
        EnableSave();
    }

}
$("textarea").bind("click", function (e) {
    $('#btnSave')[0].disabled = false;
    EnableSave();
});


$("select").change(function () {

    var optionSelected = $("option:selected", this);
    var lbl = $("select").context.activeElement.id.replace("cbo", "lbl");
    var valueSelected = this.value;
    var combo = document.getElementById($("select").context.activeElement.id);
    if (valueSelected == 'Abnormal') {
        document.getElementById(lbl).classList.remove('spanstyle');
        document.getElementById(lbl).classList.add('manredforstar');
        PopulateNADValue(combo);
    } else if (valueSelected == 'No Abnormality Detected(NAD)') {


        document.getElementById(lbl).classList.remove('manredforstar');
        //Jira CAP-1194
        if (!document.getElementById(lbl).classList.contains('lblcolor')) {
            document.getElementById(lbl).classList.add('spanstyle');
        }


        document.getElementById(lbl).style.color = "";
        if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden') != undefined) {
            var lstPhysician;
            var OtherDescription = "";
            var countList = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|').length;
            for (var j = 0; j < countList; j++) {
                if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].indexOf(physician_id) > -1) {
                    if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1] != undefined) {
                        OtherDescription = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1];
                        if ($(combo.parentElement.nextElementSibling).find('textarea')[0].value != "") {
                            if (OtherDescription == $(combo.parentElement.nextElementSibling).find('textarea')[0].value) {
                                $(combo.parentElement.nextElementSibling).find('textarea')[0].value = OtherDescription;
                            }
                        } else {
                            $(combo.parentElement.nextElementSibling).find('textarea')[0].value = OtherDescription;
                        }

                        break;
                    }
                }
            }
        }
    }

    else {
        document.getElementById(lbl).classList.remove('manredforstar');
        //Jira CAP-1194
        if (!document.getElementById(lbl).classList.contains('lblcolor')) {
            document.getElementById(lbl).classList.add('spanstyle');
        }
        PopulateNADValue(combo);
    }
    $('#btnSave')[0].disabled = false;
    EnableSave();
});

function PopulateNADValue(combo) {
    if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden') != undefined) {
        var lstPhysician;
        var OtherDescription = "";
        var countList = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|').length;
        for (var j = 0; j < countList; j++) {
            if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].indexOf(physician_id) > -1) {
                if ($(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1] != undefined) {
                    OtherDescription = $(combo.parentElement.nextElementSibling).find('textarea')[0].attributes.getNamedItem('data-hidden').value.split('|')[j].split('$')[1];
                    if (OtherDescription == $(combo.parentElement.nextElementSibling).find('textarea')[0].value) {
                        $(combo.parentElement.nextElementSibling).find('textarea')[0].value = "";
                    }
                    break;
                }
            }
        }
    }
}

var hdnFieldName = null;
function callweb(icon, List, id) {

    $('#btnSave')[0].disabled = false;
    EnableSave();
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");

        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#btnSave')[0].disabled = false;
                var values = response.d.split("|");
                var targetControlValue = id;
                var innerdiv = '';
                var pos = $('#' + targetControlValue).position();
              //  $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BUGID:45541
                innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"fun('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";//BUGID:45541
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
                          top: $(".actcmpt").height() + 8,
                          left: pos.left,
                          width: pos.width,
                          height: '150px',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '385px',
                          border: '1px solid #8e8e8e',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto'
                          //CAP-804 Syntax error, unrecognized expression
                      }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                }
                EnableSave();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    //CAP-4174 Unexpected end of JSON input
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

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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

function fun(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BUGID:45541
    var sugglistval;
    var control;
    var value = agrulist.split("^");
    if (value.length > 2) {
        //CAP-804 Syntax error, unrecognized expression
        control = value[5]?.trim();
        sugglistval = $("#" + control + ".actcmpt").val().trim();
        var selectedvalue = value[0] + ',' + value[1] + ',' + value[2] + ',' + value[3] + ',' + value[4];
        if (sugglistval != " " && sugglistval != "") {
            var subsugglistval = sugglistval;
            var len = subsugglistval.length;
            var flag = 0;
            if (subsugglistval == selectedvalue) {
                flag++;
            }
            if (flag == 0) {
                $("#" + control + ".actcmpt").val(sugglistval + "," + selectedvalue);
            }
        }
        else {
            $("#" + control + ".actcmpt").val(selectedvalue);
        }
    }
    else {
        //CAP-804 Syntax error, unrecognized expression
        sugglistval = $("#" + value[1]?.trim() + ".actcmpt").val().trim();
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
    }


}
function LoadControl(LoadType, type) {
    var url;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (LoadType != "CopyPrevious") {
        url = "WebServices/ExamService.asmx/LoadExamination";
    } else {
        url = "WebServices/ExamService.asmx/CopyPreviousExamination";
    }
    if (type == "General With Specialty") {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify({
                data: type,
            }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                var objdataDTO = $.parseJSON(data.d);
                var objdata;
                if (LoadType == "CopyPrevious") {

                    if (objdataDTO.length == 0) {
                        onCopyPrevious('210010');
                        return;
                    }
                    else if (objdataDTO[0].Physician_Process == false) {
                        onCopyPrevious('210016');//not seen by physician..
                        return;
                    }
                    else {

                        objdata = objdataDTO[0].CopypreviousEncounterList;
                        if (objdata != null) {
                            if (objdata.length == 0) {
                                onCopyPrevious('170014');//no exam found
                                return;
                            }
                            else {
                                onCopyPrevious('');
                            }
                        }
                    }

                } else {
                    objdata = objdataDTO;
                    $('#btnSave')[0].disabled = true;
                }
                if (objdata != null) {
                    for (var i = 0; i < objdata.length; i++) {
                        objdata[i].System_Name = objdata[i].System_Name.replace(/[a-z]\)\s+/g, '');
                        if ($("#tblExam").find("label:contains('" + objdata[i].System_Name + "')")[0] != undefined && objdata[i].Condition_Name == "") {
                            var lbl = $("#tblExam").find("label:contains('" + objdata[i].System_Name + "')");
                            for (var k = 0; k < lbl.length; k++) {
                                if (lbl[k].textContent == objdata[i].System_Name && lbl[k].parentNode.parentNode.attributes.getNamedItem("system_name") == null) {
                                    controlname = lbl[k];
                                }
                            }
                            if (controlname.parentNode.nextElementSibling.children[0] != undefined) {
                                controlname.parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                                if (objdata[i].Status == "Not Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                                }
                                else if (objdata[i].Status == "Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                                }
                                else if (objdata[i].Status == "No Abnormality Detected(NAD)") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                                }
                                else if (objdata[i].Status == "Abnormal") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 3;

                                    document.getElementById(controlname.id).classList.remove('spanstyle');
                                    document.getElementById(controlname.id).classList.add('manredforstar');


                                }
                            }
                            $(controlname.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Examination_Notes;
                            if (LoadType == "CopyPrevious") {
                                controlname.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~0~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                            }
                            else
                                controlname.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~' + objdata[i].Id + '~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                        }
                        else {
                            var lbl = $("#tblExam").find("label:contains('" + objdata[i].Condition_Name + "')");
                            var controlname;
                            objdata[i].System_Name = objdata[i].System_Name.replace(/[a-z]\)\s+/g, '');
                            for (var k = 0; k < lbl.length; k++) {
                                if (lbl[k].parentElement.parentElement.attributes.getNamedItem('system_name') != null) {
                                    if (lbl[k].parentElement.parentElement.attributes.getNamedItem('system_name').value == objdata[i].System_Name) {
                                        controlname = lbl[k];
                                    }
                                }
                            }
                            if (controlname.parentNode.nextElementSibling.children[0] != undefined) {
                                controlname.parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                                if (objdata[i].Status == "Not Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                                }
                                else if (objdata[i].Status == "Examined") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                                }
                                else if (objdata[i].Status == "No Abnormality Detected(NAD)") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                                }
                                else if (objdata[i].Status == "Abnormal") {
                                    controlname.parentNode.nextElementSibling.children[0].selectedIndex = 3;


                                    document.getElementById(controlname.id).classList.remove('spanstyle');
                                    document.getElementById(controlname.id).classList.add('manredforstar');

                                }
                            }
                            $(controlname.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Examination_Notes;
                            if (LoadType == "CopyPrevious") {
                                controlname.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~0~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                            }
                            else
                                controlname.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~' + objdata[i].Id + '~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                        }
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },

            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    if (xhr.responseText != null && xhr.responseText != undefined && xhr.responseText != "") {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //           "Message: " + log.Message);
                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify({
                data: type,
            }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {

                var objdataDTO = $.parseJSON(data.d);
                var objdata;

                if (LoadType == "CopyPrevious") {
                    if (objdataDTO.length == 0) {
                        onCopyPrevious('210010');
                        return;
                    }
                    else if (objdataDTO[0].Physician_Process == false) {
                        onCopyPrevious('210016');//not seen by physician..
                        return;
                    }
                    else {

                        objdata = objdataDTO[0].CopypreviousEncounterList;
                        if (objdata != null) {
                            if (objdata.length == 0) {
                                onCopyPrevious('170014');//no exam found
                                return;
                            }
                            else {
                                onCopyPrevious('');
                            }
                        }
                    }
                } else {
                    objdata = objdataDTO;
                }

                var i = 0;
                var tblExambody = $("#tblExam tr");
                for (var i = 0; i < objdata.length; i++) {
                    var j = 0;
                    objdata[i].Condition_Name = objdata[i].Condition_Name.replace(/'/g, "\\'");
                    objdata[i].System_Name = objdata[i].System_Name.replace(/[a-z]\)\s+/g, '');
                    var lbl = $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')");
                    if (lbl.length > 1) {
                        for (var k = 0; k < lbl.length; k++) {
                            if (lbl[k].parentElement.parentElement.attributes.getNamedItem('system_name').value == objdata[i].System_Name) {
                                if (objdata[i].Condition_Name == lbl[k].innerText)//Added by naveena for bug_id=36763
                                {
                                    j = k;
                                }
                            }
                        }
                    }
                    else { j = 0; }
                    //CAP-3542
                    if (objdata[i].Condition_Name != "") {
                        var element = $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j]?.parentNode?.nextElementSibling?.children;
                        if (element?.length > 0 && element[0] != undefined) {
                            element[0].value = objdata[i].Status;
                            if (objdata[i].Status == "Not Examined") {
                                element[0].selectedIndex = 0;
                            }
                            else if (objdata[i].Status == "Examined") {
                                element[0].selectedIndex = 1;
                            }
                            else if (objdata[i].Status == "No Abnormality Detected(NAD)") {
                                element[0].selectedIndex = 2;
                            }
                            else if (objdata[i].Status == "Abnormal") {
                                element[0].selectedIndex = 3;

                                document.getElementById(lbl[j].id).classList.remove('spanstyle');
                                document.getElementById(lbl[j].id).classList.add('manredforstar');

                            }
                        }
                        $($("#tblExam").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Examination_Notes;

                        if (LoadType == "CopyPrevious") {
                            $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~0~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                        } else
                            $("#tblExam tr").find("label:contains('" + objdata[i].Condition_Name + "')")[j].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '~' + objdata[i].Id + '~' + objdata[i].Created_By + '~' + objdata[i].Created_Date_And_Time;
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);
                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }


}


function onCopyPrevious(errorCode) {

    if (errorCode == "") {
        enableAutoSave();
    }
    else {
        DisplayErrorMessage(errorCode);
        disableAutoSave();
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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


function OpenPopup(Keyword) {
    var focused = Keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}

