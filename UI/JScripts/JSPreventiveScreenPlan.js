var first_time = null;
//CAP-324 - Declare variable for lbl
var lbl;
function checkboxClicked() {
    var chk = document.getElementById("chkPreventivePlan");
    if (chk.checked) {
        var cntrls = document.getElementById("divPreventivePlan").getElementsByTagName("td");;
        for (var i = 0; i < cntrls.length; i++) {
            if (cntrls[i].childNodes.length != 0) {
                if (cntrls[i].childNodes[0].id != undefined) {
                    if (cntrls[i].childNodes[0].id.startsWith("cbo") == true && cntrls[i].childNodes[0].id.indexOf("_Input") != -1) {
                        if (cntrls[i].childNodes[0].defaultValue != "Yes") {
                            var cboname = cntrls[i].childNodes[0].id.split("_")[0];
                            var combo = $find(cboname);
                            var item = combo.findItemByText("No");
                            if (item) {
                                item.select();
                            }
                        }
                    }
                }
            }
        }
    } else {
        var cntrls = document.getElementById("divPreventivePlan").getElementsByTagName("td");;
        for (var i = 0; i < cntrls.length; i++) {
            if (cntrls[i].childNodes.length != 0) {
                if (cntrls[i].childNodes[0].id != undefined) {
                    if (cntrls[i].childNodes[0].id.startsWith("cbo") == true && cntrls[i].childNodes[0].id.indexOf("_Input") != -1) {
                        if (cntrls[i].childNodes[0].defaultValue != "Yes") {
                            var cboname = cntrls[i].childNodes[0].id.split("_")[0];
                            var combo = $find(cboname);
                            var item = combo.findItemByText("");
                            if (item) {
                                item.select();
                            }
                        }
                    }
                }
            }
        }
    }
}

function EnableSave() {
    localStorage.setItem("bSave", "false");
    document.getElementById('btnSave').disabled = false;
    if (document.getElementById("hdnEnableYesNo") != null) {
        document.getElementById("hdnEnableYesNo").value = "true";
    }
    if (top.window.document.getElementById("ctl00_C5POBody_hdnIsSaveEnable") != undefined && top.window.document.getElementById("ctl00_C5POBody_hdnIsSaveEnable") != null) {
        top.window.document.getElementById("ctl00_C5POBody_hdnIsSaveEnable").value = "true";
    }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    else
        window.parent.theForm.hdnSaveEnable.value = "true";
}

function Clear() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
    var IsClearAll = DisplayErrorMessage('600005');
    if (IsClearAll == true) {
        $find('btnSave').set_enabled(true);
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
        else
            window.parent.theForm.hdnSaveEnable.value = true;

        var ctrl = document.getElementsByTagName('INPUT');
        var ctrlCmb = document.getElementsByTagName('SELECT');

        for (var i = 0; i < ctrl.length; i++) {
            if (ctrl[i].id.split('_')[1] == 'listDLC') {
                var ID = ctrl[i].id.replace("listDLC", "txtDLC");
                document.getElementById(ID.replace("_ClientState", "")).value = ""; //.clear();//changed by naveena for bug_id 26571
                document.getElementById("chkPreventivePlan").checked = false;
                if (ID.replace("txt", "lst") != null) {
                    if (document.getElementById(ID.replace("txt", "lst")) != null) {
                        if (document.getElementById(ID.replace("txt", "lst")).style.visibility == "") {
                            $find(ID.replace("_ClientState", "").replace("txt", "lst")).get_element().style.display = "none";
                            document.getElementById(ID.replace("_ClientState", "").replace("txt", "img1")).src = "Resources/plus_new.gif";
                        }
                    }
                }
            } else if (ctrl[i].id.split('_')[0] == 'cbo') {
                var ID = ctrl[i].id;
                if (ctrl[i].id.split('_')[2] == "ClientState")
                    $find(ID.replace("_ClientState", "")).clearSelection();
                else if (ctrl[i].id.split('_')[2] == "Input")
                    $find(ID.replace("_Input", "")).clearSelection();
            }
        }
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    } else {
        $find('btnSave').set_enabled(true);
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
        else
            window.parent.theForm.hdnSaveEnable.value = "true";
        return false;
    }
    for (var i = 0; i < ctrlCmb.length; i++) {
        if (ctrlCmb[i].id.indexOf('cbo') >= 0) {
            var ID = ctrlCmb[i].id;
            document.getElementById(ID).selectedIndex = 0;
        }
    }
}

function Enable_OR_Disable() {
    if (!$find('btnSave').get_enabled())
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";
}

function CCTextChanged() {
    $find('btnSave').set_enabled(true);
    document.getElementById("Hidden1").value = "True";
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    else
        window.parent.theForm.hdnSaveEnable.value = "true";
    return false;
}

function ShowLoading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function btnSaveClicked() {
    ShowLoading();
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
}

function savedSuccessfully() {
    localStorage.setItem("bSave", "true");
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
        screen_name = "PlanTabClick";
    else
        screen_name = "EncounterTabClick";
    if (splitvalues.length == 3 && splitvalues[2] == "Node")
        screen_name = 'PatientChartTreeViewNodeClick';
    DisplayErrorMessage("270001");
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
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
    }
}


function chkPlanCheckChange() {
    var bcheck = true;
    $find('btnSave').set_enabled(true);
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    var ctrl = document.getElementsByTagName('*');
    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].id.substring(0, 3) == 'cbo') {
            var name = ctrl[i].name;
            var ID = document.getElementsByName(name);

            for (var j = 0; j < ID.length; j++) {
                var value = ID[j].value.split(' ')[1];

                if (document.getElementById('chkPreventivePlan').checked == true) {
                    if (ctrl[i].value == "No") {

                    } else if (ctrl[i].value == "") {
                        ctrl[i].selected = true;
                        ctrl[i].value = "No";
                    } else if (ctrl[i].value == "Yes") { }
                } else {
                    if (ctrl[i].value == "No") {
                        ctrl[i].selected = false;
                        ctrl[i].value = "";
                    }
                }
            }
        }
    }
}

function PrevScreenPlan_Load() {
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,rdmpPlanTab";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    
}

function loading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}

function btnCopyPreviousEncounter_Clicked(sender, args) {

    var btnSave = $find('btnSave');
    if (btnSave._enabled) {
        args.set_cancel(true);
        first_time = "true";
        checkSavebutton(args);
        sender.set_autoPostBack(false);
    }

    else {
        sender.set_autoPostBack(true);
    }
}

function checkSavebutton() {
    if (first_time == "true") {
        var button2 = $find('btnSave');
        var btnCopyCheck = $find('btnPastEncounter');
        var button1 = document.getElementById('btnCopyPreviousEncounter');
        first_time = "false";
        var arg = args.get_argument();
                var now = new Date();
                var utc = now.toUTCString();
                document.getElementById(GetClientId("hdnLocalTime")).value = utc;
                document.getElementById("HdnCopyButton").value = "trueValidate";
                document.getElementById("hdnTrueCheck").value = "true";
                testCopy = "true";
                __doPostBack('btnSave');

    }

    document.getElementById("HdnCopyButton").value = "";
    //var obj = new Array();
    //obj.push("Title=" + "Message");
    //obj.push("ErrorMessages=" + "There are unsaved changes.Do you want to save them?");
    //var result = openModal("frmValidationArea.aspx", 100, 300, obj, "MessageWindow");
    //var WindowName = $find('MessageWindow');
    //WindowName.add_close(OnClientCloseValidation);
}


function OnClientCloseValidation(oWindow, args) {
    if (first_time == "true") {
        var button2 = $find('btnSave');
        var btnCopyCheck = $find('btnPastEncounter');
        var button1 = document.getElementById('btnCopyPreviousEncounter');
        first_time = "false";
        var arg = args.get_argument();
        if (arg) {
            var result = arg;
            if (result == "Yes") {
                var now = new Date();
                var utc = now.toUTCString();
                document.getElementById(GetClientId("hdnLocalTime")).value = utc;
                document.getElementById("HdnCopyButton").value = "trueValidate";
                document.getElementById("hdnTrueCheck").value = "true";
                testCopy = "true";
                __doPostBack('btnSave');

            } else if (result == "Cancel") {
                document.getElementById("HdnCopyButton").value = "CheckSave";
            }
            else if (result == "No") {
                document.getElementById("HdnCopyButton").value = "No";
                button2.set_enabled(false);
                button1.click();
            }
        }
    }

    document.getElementById("HdnCopyButton").value = "";
}
/*HtmlLoad*/

function PrevntScreenPlanNew_Load() {
    $("#divPreventiveScreenPlan").load("preventivescreenplanhtml.html", LoadPage);
}
function LoadPage() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    $("textarea").addClass('spanstyle');
    $("label").addClass('spanstyle');
    $("select").addClass('spanstyle');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").bind("keydown", function (e) {//BugID:45541

        insertTab(this, event);
    });
    var rows = $("#mainContainer table label");
    $('#btnSave')[0].disabled = true;
    var lbl;
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/LoadPreventiveScreenPlan",
        data: '',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var currentprocess = objdata.currentProcess;
            if (objdata.PreventiveScreenLst != "[]") {
                for (var i = 0; i < objdata.PreventiveScreenLst.length; i++) {
                    for (var p = 0; p < rows.length; p++) {
                        if (rows[p].innerText == objdata.PreventiveScreenLst[i].Preventive_Service_Value) {
                            lbl = rows[p];
                            break;
                        }
                    }
                    if (lbl != "") {
                        lbl.parentNode.nextElementSibling.children[0].value = objdata.PreventiveScreenLst[i].Status;
                        if (objdata.PreventiveScreenLst[i].Status == "") {
                            lbl.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                        }
                        else if (objdata.PreventiveScreenLst[i].Status == "Yes") {
                            lbl.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                        }
                        else if (objdata.PreventiveScreenLst[i].Status == "No") {
                            lbl.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                        }
                        $(lbl.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata.PreventiveScreenLst[i].Preventive_Screening_Notes.trim();
                        lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata.PreventiveScreenLst[i].Version + '-' + objdata.PreventiveScreenLst[i].Id;
                    }
                }
            }

            if (objdata.Gender == "FEMALE") {
                $("div.panel-info.FEMALE").css({ "display": "block" });
            }
            else {
                $("div.panel-info.FEMALE").css({ "display": "none" });
            }

            var len = $("#mainContainer").find("textarea").length;
           
            if (currentprocess.toUpperCase() != "SCRIBE_PROCESS" && currentprocess.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "SCRIBE_CORRECTION" &&  currentprocess.toUpperCase() != "DICTATION_REVIEW" && currentprocess.toUpperCase() != "CODER_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "PROVIDER_PROCESS" && currentprocess.toUpperCase() != "MA_REVIEW" && currentprocess.toUpperCase() != "MA_PROCESS" && currentprocess.toUpperCase() != "PROVIDER_REVIEW_CORRECTION") {
                $('#btnSave')[0].disabled = true;
                $('#btnClearAll')[0].disabled = true;
                $('#btnCopyPrevious')[0].disabled = true;
                $('#mainContainer')[0].disabled = true;
                $('#mainContainer ').find(':input').prop('disabled', true);
                $('select').css('backgroundColor', '#ebebe4');
                $("a").attr('disabled', true);
                $("a").attr('onclick', false);
                $("a").css('backgroundColor', '#6D7777');
            }

             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        },
        error: function OnError(xhr) {
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
        }
    });
}


function SavePreventiveScreenPlan() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    
    var arry = new Array();
    var PreventiveScreenPlanList = function () {
        this.Preventive_Service_Value = "";
        this.Preventive_Service = "";
        this.Status = "";
        this.Preventive_Screening_Notes = "";
        this.version = "";
        this.Id = "";
    };
    var arry = new Array();
    var objPreventiveScreenPlanList = new PreventiveScreenPlanList();
    var rows = $("#mainContainer table label");
    if ($("#mainContainer div:visible table label") != null && $("#mainContainer div:visible table label")!=undefined && $("#mainContainer div:visible table label").length != 0) {
        rows = $("#mainContainer div:visible table label")
    } 
    for (var pos = 0; pos < rows.length ; pos++) {
        var objPreventiveScreenPlanList = new PreventiveScreenPlanList();
        lbl = rows[pos];
        if (lbl.className.indexOf("lblversion")<0) {
            objPreventiveScreenPlanList.Preventive_Service = lbl.parentElement.parentElement.parentElement.parentElement.parentNode.previousElementSibling.textContent;
            objPreventiveScreenPlanList.Preventive_Service_Value = lbl.innerText;
            objPreventiveScreenPlanList.Status = lbl.parentNode.nextElementSibling.children[0].value;
            objPreventiveScreenPlanList.Preventive_Screening_Notes = $(lbl.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value.trim();
            if (lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent != "") {
                objPreventiveScreenPlanList.version = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split('-')[0].trim();
                objPreventiveScreenPlanList.Id = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split('-')[1].trim();
            }
            else {
                objPreventiveScreenPlanList.version = 0;
                objPreventiveScreenPlanList.Id = 0;
            }
            arry[arry.length++] = objPreventiveScreenPlanList;
        }

    }
    localStorage.setItem("bSave", "true");
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/SavePreventiveScreenPlan",
        data: JSON.stringify({
            "data": arry,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var lbl;
            if (objdata != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    for (var p = 0; p < rows.length; p++) {
                        if (rows[p].innerText == objdata[i].Preventive_Service_Value) {
                            lbl = rows[p];
                            break;
                        }
                    }
                    if (lbl != "" && lbl.className != "lblversion") {
                        lbl.parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                        if (objdata[i].Status == "") {
                            lbl.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                        }
                        else if (objdata[i].Status == "Yes") {
                            lbl.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                        }
                        else if (objdata[i].Status == "No") {
                            lbl.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                        }
                        $(lbl.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Preventive_Screening_Notes.trim();
                        lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + objdata[i].Id;
                    }
                }
            }
            $('#btnSave')[0].disabled = true;
            savedSuccessfully();
            AutoSaveSuccessful();
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        },
        error: function OnError(xhr) {
            AutoSaveUnsuccessful();
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
        }
    });
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


function CopyPreviousPreventiveScreenPlan() {



    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
        event.preventDefault();
        event.stopPropagation();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        disableAutoSave();
        SavePreventiveScreenPlan();
        PreventiveScreenPlanCopyPrevious();
        return;
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogPreventivePlan');
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
        //            SavePreventiveScreenPlan();
        //            PreventiveScreenPlanCopyPrevious();
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            disableAutoSave();
        //            PreventiveScreenPlanCopyPrevious();
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
        PreventiveScreenPlanCopyPrevious();
    }
}
function PreventiveScreenPlanCopyPrevious() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var rows = $("#mainContainer table label");
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/CopyPreviousPreventiveScreenPlan",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            var objdata = $.parseJSON(data.d);

            var previouslst = objdata.data;
            var IsPreviousEncounter = objdata.IsPreviousEncounter;
            var IsPhyEnc = objdata.IsPhyEnc;

            if (!IsPreviousEncounter) {
                onCopyPrevious('210010');
                return;
            }
            else if (!IsPhyEnc) {
                onCopyPrevious('210016');
                return;
            }
            if (previouslst.length > 0) {
                if (previouslst.length == 1) {
                    onCopyPrevious('170014');
                    return;
                }
                if (previouslst != "[]") {
                    for (var i = 0; i < previouslst.length; i++) {
                        for (var p = 0; p < rows.length; p++) {
                            if (rows[p].innerText == previouslst[i].Preventive_Service_Value) {
                                lbl = rows[p];
                                break;
                            }
                        }
                        //CAP-324 - Handle null & undefined value
                        if ((lbl ?? "") != "" && lbl.className != "lblversion") {
                            lbl.parentNode.nextElementSibling.children[0].value = previouslst[i].Status;
                            if (previouslst[i].Status == "") {
                                lbl.parentNode.nextElementSibling.children[0].selectedIndex = 0;
                            }
                            else if (previouslst[i].Status == "Yes") {
                                lbl.parentNode.nextElementSibling.children[0].selectedIndex = 1;
                            }
                            else if (previouslst[i].Status == "No") {
                                lbl.parentNode.nextElementSibling.children[0].selectedIndex = 2;
                            }

                            $(lbl.parentNode.nextElementSibling.nextElementSibling).find('textarea')[0].value = previouslst[i].Preventive_Screening_Notes.trim();
                            lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = previouslst[i].Version + '-' + previouslst[i].Id;
                        }
                    }
                    onCopyPrevious('');
                }
            }
            else {
                onCopyPrevious('210010');
            }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);
                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;


            }
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
    });

}
function Clear() {
    

    if (DisplayErrorMessage('270002') == true) {
        $('select').prop('selectedIndex', 0);
        $('textarea').val("");
        $('#btnSave')[0].disabled = false;
        EnableSave();
    }

}

$("select").change(function () {
    
    var optionSelected = $("option:selected", this);
    $('#btnSave')[0].disabled = false;
    EnableSave();

});

function chkSystem_CheckedChanged() {
    
    $('#btnSave')[0].disabled = false;
    EnableSave();
    var lfckv = document.getElementById("chkSystem").checked;
    if (lfckv) {

        var combobox = $('select.combo');
        var classname = "";
        for (var i = 0; i < combobox.length; i++) {
            if ($(combobox[i]).find('option:selected').val() != 'Yes')
                $($(combobox[i]).find('option')[2]).prop('selected', true);

        }
    }
    else {
        var combobox = $('select.combo');
        for (var i = 0; i < combobox.length; i++) {
            if ($(combobox[i]).find('option:selected').val() == 'No')
                $($(combobox[i]).find('option')[0]).prop('selected', true);
        }
    }
}

var hdnFieldName = null;
function callweb(icon, List, id) {

    $('#btnSave')[0].disabled = false;
    EnableSave();
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
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
                var FacilityRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value;
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:45541
                if (FacilityRole != "") {
                    if (FacilityRole.split('&')[1] != "" && (FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN" || FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN ASSISTANT")) {
                        innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                    }
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black' onclick=\"fun('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45541
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
                          top: pos.top + $(".actcmpt").height() + 5,
                          left: pos.left,
                          width: "370px",
                          height: '150px',
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
                        .insertAfter($("#" + targetControlValue + ".actcmpt"));
                }
                EnableSave();
                 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }

                 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
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
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var sugglistval;
    var control;
    var value = agrulist.split(",");
    if (value.length > 2) {
        control = value[5];
        //CAP-283 - null handling if valur is null or undefined
        sugglistval = ($("#" + control + ".actcmpt").val()??"").trim();
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
        sugglistval = $("#" + value[1] + ".actcmpt").val().trim();
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
                $("#" + value[1] + ".actcmpt").val(sugglistval + "," + value[0]);
            }
        }
        else {
            $("#" + value[1] + ".actcmpt").val(value[0]);
        }
    }
}


function OpenPopup(Keyword) {
    var focused = Keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
  
}
function onCopyPrevious(errorCode) {

    if (errorCode == "") {
        enableAutoSave();
    }
    else {
        DisplayErrorMessage(errorCode);
        disableAutoSave();
    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
$("textarea").bind("click", function (e) {
    $('#btnSave')[0].disabled = false;
    EnableSave();
});
