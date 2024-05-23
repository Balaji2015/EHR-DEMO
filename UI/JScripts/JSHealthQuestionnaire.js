var intx;
function clearall() {
    if (DisplayErrorMessage('1180002') == true) {

        var ctrl = document.getElementsByTagName('INPUT');
        var ctrldlc = document.getElementsByTagName('textarea');
        if (ctrl.length > 0)
        {
            for (var i = 0; i < ctrl.length; i++) {
                if (ctrl[i].type == 'radio') {
                    var ID = ctrl[i].id;
                    document.getElementById(ID).checked = false;
                }
            }
           
        }
        if(ctrldlc.length>0)
        {
            for (var i = 0; i < ctrldlc.length; i++) {
                if (ctrldlc[i].id.split('_')[1] == 'txtDLC') {
                    var ID = ctrldlc[i].id.replace("listDLC", "txtDLC");
                    document.getElementById(ID.replace("_ClientState", "")).value = "";

                    if (ID.replace("txt", "lst") != null) {
                        if (document.getElementById(ID.replace("txt", "lst")) != null) {
                            if (document.getElementById(ID.replace("txt", "lst")).style.visibility == "") {
                                $find(ID.replace("_ClientState", "").replace("txt", "lst")).get_element().style.display = "none";
                                document.getElementById(ID.replace("_ClientState", "").replace("txt", "img1")).src = "Resources/plus_new.gif";
                            }
                        }
                    }

                }
            }
            if (document.getElementById("chkQuestion") != null) {
                if (document.getElementById("chkQuestion").checked) {
                    document.getElementById("chkQuestion").checked = false;
                    Checks();
                }
            }
        }
        if (document.getElementById("txtTotalScore") != undefined) {
            document.getElementById("txtTotalScore").value = '';
        }
        if (document.getElementById("hdnTotalScore") != null) {
            document.getElementById("hdnTotalScore").value = ' ';
        }
        if (document.getElementById("hdnTotalScoreDescription") != null) {
            document.getElementById("hdnTotalScoreDescription").value = '';
        }
        if (document.getElementById("txtPercentage") != undefined) {
            document.getElementById("txtPercentage").value = '';
        }
        if (document.getElementById("hdnPercentage") != null) {
            document.getElementById("hdnPercentage").value = ' ';
        }
        $("#btnSave")[0].disabled = false;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    else {

    }

}

function Checks()
{
    __doPostBack("'" + "chkQuestion" + "'", '');
}

function Questionnairechanged() {
    var sTotalScore = 0;
    var sPercentage = 0;
    var sChecked = 0;
    var ctrl = document.getElementsByTagName('INPUT');
    var Scorelevel = document.getElementById('hdnScorelevel').value;
    var PercentageLevel = document.getElementById('hdnPercentageLevel').value;
    var ScoreDescription = '';
    var FinalValue = '';
    if (Scorelevel != null && Scorelevel != '') {
        ScoreDescription = (Scorelevel).split('|');
    }
    var PercentageDescription = '';
    var PercentageValue = '';
    if (PercentageLevel != null && PercentageLevel != '') {
        PercentageDescription = (PercentageLevel).split('|');
    }
    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].type == 'radio') {
            var name = ctrl[i].name;
            var ID1 = document.getElementsByName(name);
            var ID = ctrl[i].id;
            if (document.getElementById(ID).checked) {
                var value = '';
                value = ID.split(' ')[1];
                if (value == undefined)
                    value = ID.split('-')[1]
                if (Number.isInteger(Number(value))) {
                    sChecked++;
                    sTotalScore = Number(sTotalScore) + Number(value);
                    sPercentage = Number(sTotalScore) * 2;
                }
            }
        }
    }
    if (ScoreDescription.length > 0) {
        for (var j = 0; j < ScoreDescription.length; j++) {
            var v = ScoreDescription[j].split('$')[1];
            var iStart = Number(v.split('-')[0]);
            var iEnd=Number(v.split('-')[1]);
            for (var k = iStart; k <= iEnd; k++) {
                if (k == sTotalScore)
                {
                    FinalValue = ScoreDescription[j].split('$')[0];
                }
            }
        }
    }

    if (PercentageDescription.length > 0) {
        for (var j = 0; j < PercentageDescription.length; j++) {
            var v = PercentageDescription[j].split('$')[1];
            var iStart = Number(v.split('-')[0]);
            var iEnd = Number(v.split('-')[1]);
            for (var k = iStart; k <= iEnd; k++) {
                if (k == sPercentage) {
                    PercentageValue = PercentageDescription[j].split('$')[0];
                }
            }
        }
    }
        var ctrldlc = document.getElementsByTagName('textarea');
        if (ctrldlc.length > 0) {
            for (var i = 0; i < ctrldlc.length; i++) {
                if (ctrldlc[i].id.split('_')[1] == 'txtDLC') {
                    var ID = ctrldlc[i].id.replace("listDLC", "txtDLC");
                    if (ID.includes('DLCTotalScore'))
                        document.getElementById(ID.replace("_ClientState", "")).value = FinalValue;
                    else if (ID.includes('DLCPercentage%'))
                        document.getElementById(ID.replace("_ClientState", "")).value = PercentageValue;
                }
            }
        }
    if (sChecked > 0) {
        if (document.getElementById("hdnTotalScore") != null)
            document.getElementById("hdnTotalScore").value = sTotalScore;
        if (document.getElementById("txtTotalScore") != null)
            document.getElementById("txtTotalScore").value = sTotalScore;
        if (document.getElementById("hdnTotalScoreDescription") != null)
            document.getElementById("hdnTotalScoreDescription").value = FinalValue;
        if (document.getElementById("hdnPercentage") != null)
            document.getElementById("hdnPercentage").value = sPercentage;
        if (document.getElementById("txtPercentage") != null)
            document.getElementById("txtPercentage").value = sPercentage;
    }
    if (sChecked == 0) {
        if (document.getElementById("hdnTotalScore") != null)
            document.getElementById("hdnTotalScore").value = ' ';
        if (document.getElementById("txtTotalScore") != null)
            document.getElementById("txtTotalScore").value = '';
        if (document.getElementById("hdnTotalScoreDescription") != null)
            document.getElementById("hdnTotalScoreDescription").value = '';
        if (document.getElementById("hdnPercentage") != null)
            document.getElementById("hdnPercentage").value = ' ';
        if (document.getElementById("txtPercentage") != null)
            document.getElementById("txtPercentage").value = '';
    }
}


function CheckChanged() {
    var bcheck = true;
    var ctrl = document.getElementsByTagName('INPUT');
    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].type == 'radio') {
            var name = ctrl[i].name;
            var ID = document.getElementsByName(name);

            for (var j = 0; j < ID.length; j++) {
                var value = ID[j].value.split(' ')[1];

                if (document.getElementById('chkQuestion').checked == true) {
                    if (value != "No") {
                        if (ID[j].checked == true) {
                            bcheck = false;
                        }
                        else {
                            bcheck = true;
                        }
                    }
                    if (bcheck) {
                        if (value == "No") {
                            ID[j].checked = true;
                        }
                    }
                }
                else {

                    if (value == "No") {
                        ID[j].checked = false;
                    }
                }


            }



        }


    }
    $("#btnSave")[0].disabled = false;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
}
function OpenAddorUpdate(ctrlID) {
    visiblelstbox()
    var obj = new Array();
    obj.push("FieldName=" + ctrlID);
    openModal("frmAddorUpdateKeywords.aspx", 800, 700, obj);
    return false;
}

function visiblelstbox() {
    var ctrl = document.getElementsByTagName('INPUT');

    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].id.substring(0, 3) == 'txt') {
            var ID = ctrl[i].id;
            if (ID.replace("txt", "lst") != null) {
                if (document.getElementById(ID.replace("txt", "lst")) != null) {
                    if (document.getElementById(ID.replace("txt", "lst")).style.visibility == "visible" || document.getElementById(ID.replace("txt", "lst")).style.visibility == "") {
                        $find(ID.replace("_ClientState", "").replace("txt", "lst")).get_element().style.display = "none";
                        document.getElementById(ID.replace("_ClientState", "").replace("txt", "img1")).src = "Resources/plus_new.gif";
                        document.getElementById(ID.replace("_ClientState", "").replace("txt", "img1")).setAttribute("src", "Resources/plus_new.gif");
                    }
                }
            }
        }
    }

}

function ClearAllRoq() {
    if (DisplayErrorMessage('1180002') == true) {
        var ctrl = document.getElementsByTagName('INPUT');
        var ctrldlc = document.getElementsByTagName('textarea');
        for (var i = 0; i < ctrl.length; i++) {
            if (ctrl[i].type == 'checkbox') {
                if (ctrl[i].checked == true) {
                    ctrl[i].checked = false;
                }
            }
        }
        if(ctrldlc.length>0)
        {
            for (var i = 0; i < ctrldlc.length; i++)
            {
                if (ctrldlc[i].id.split('_')[2] == 'txtDLC') {
                    document.getElementById(ctrldlc[i].id.replace("_ClientState", "")).value = "";
                }
            }
        }
        $("#btnSave")[0].disabled = false;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
    else {

    }
    return false;
}

function clearTextBox(txtID) {
    document.getElementById(txtID).value = "";
    $find(txtID).clear(); 1
    visiblelstbox()
    return false;
}




function EnableSave(e) {
    $("#btnSave")[0].disabled = false;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    document.getElementById("Hidden1").value = "True";
}



function Enable_OR_Disable(id) {
    var button2 = $("#btnSave");
    if (button2.disabled==false)
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";

    intx = document.getElementById("divHealthQuestionnaire").scrollTop;
}


function SetDivPosition() {
    if (intx != undefined) {
        document.getElementById("divHealthQuestionnaire").scrollTop = intx;
        intx = undefined;
    }
}


function SaveEnabled() {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    localStorage.setItem("bSave", "true");
    return true;
}


function cancelBack(e) {
    if (e == 0) {
        $("#btnSave")[0].disabled = false;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
    }
}



function RadTabStrip2_TabSelecting(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var TabClick = window.parent.theForm.hdnTabClick;
    if (TabClick.value == "first") {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            TabClick.value = args._tab._element.textContent + "$#$";
            args.set_cancel(true);
            DisplayErrorMessage('1100000', 'QuestionnaireTabClick');
            return;
        }
    }
    else {
        var splitvalue = TabClick.value.split('$#$');
        var clicked_tab = splitvalue[0];
        var switchcase = splitvalue[1];
        if (switchcase == "second,true") {
            var IDs = window.parent.theForm.hdnSaveButtonID.value.split(',');

            var childControlsofChildContainer = $find(IDs[1]).get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
            for (var count = (childControlsofChildContainer.length - 1) ; count >= 0; count--) {
                if (childControlsofChildContainer[count]._element.id == IDs[0]) {
                    var save_button = childControlsofChildContainer[count];
                    if (save_button != undefined || save_button != null) {
                        args.set_cancel(true);
                        TabClick.value = clicked_tab + "$#$third";
                        save_button.click();
                        return;
                    }
                    break;
                }
            }

        }
        else if (switchcase == "second,false") {
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        }
        else if (switchcase == "second,cancel") {
            args.set_cancel(true);
        }
        TabClick.value = "first";
    }
}

function Questionnaire_Load() {
   
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $("textarea[id *= txtDLC]").removeClass('DlcClass');
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
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
        screen_name = "QuestionnaireTabClick";
    else
        screen_name = "EncounterTabClick";
    if (splitvalues.length == 3 && splitvalues[2] == "Node")
        screen_name = 'PatientChartTreeViewNodeClick';
    SavedSuccessfully_NowProceed(screen_name);
    DisplayErrorMessage('118501');
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if ($("#btnSave") != null)
        $("#btnSave")[0].disabled = true;;
    AutoSaveSuccessful();
    $("textarea[id *= txtDLC]").removeClass('DlcClass');
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
}

function chkCheckedChange(NormalQuestionStatus) {
    var QuestionArry = NormalQuestionStatus.split('~');
    for (var k = 0; k < QuestionArry.length; k++) {
        var ques = QuestionArry[k].split('^');
        var bFirst = true;
        if (ques.length > 1) {
            var Question = ques[0].trim().replace(/:/g, "");
            var NormalAns = ques[1].replace(/#/g, "'");
            var PossbileAns = ques[2].replace(/#/g, "'").split('|');
            var QuesType = ques[3].trim();
            if (NormalAns != '') {
                var index = PossbileAns.indexOf(NormalAns);
                PossbileAns.splice(index, 1);
                PossbileAns;
                var sToBeChecked = 'rdbtn ' + NormalAns + ' ' + Question + '-' + QuesType + '-' + k;
                var sNotToBeChecked = [];
                for (var x = 0; x < PossbileAns.length; x++)
                    if (PossbileAns[x]!='')
                        sNotToBeChecked.push('rdbtn ' + PossbileAns[x] + ' ' + Question + '-' + QuesType + '-' + k);
                var rdbtnAns = document.getElementById(sToBeChecked);
                var IsOthersChecked=false;
                if (document.getElementById('chkQuestion')!=null && document.getElementById('chkQuestion').checked) {
                    for (var n = 0; n < sNotToBeChecked.length; n++) {
                        if (document.getElementById(sNotToBeChecked[n])!=null && document.getElementById(sNotToBeChecked[n]).checked == true)
                            IsOthersChecked = true;
                    }
                    if (rdbtnAns!=null && IsOthersChecked == false)
                        rdbtnAns.checked = true;
                }
                else
                    if (rdbtnAns != null)
                        rdbtnAns.checked = false;
            }
        }
    }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable!=null)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    localStorage.setItem('bSave', 'false');
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    Questionnairechanged();
}

    
function OpenReport(path)
{
    $($(top.window.document).find('#ProcessiFrame')[0]).attr('src', "");
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#mdlcontent")[0].style.width = "100%";
    $(top.window.document).find("#ProcessiFrame")[0].style.border = "1px solid #D0D0D0";
    $($(top.window.document).find('#ProcessiFrame')[0]).attr('src', path);
}



function btnPrint_Clicked() {
   var strDOS = window.parent.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1];
    var strProvider=window.parent.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[3];
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        document.getElementById("hdnPrint").value = "true";
        __doPostBack('btnSave', 'OnClick');

        //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }      
        //$(top.window.document).find("body").append("<div id='dvdialog' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
        //                    "<p style='font-family: Verdana,Arial,sans-serif; font-size: 13.5px;'>There are unsaved changes.Do you want to save them?</p></div>");
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialog');
        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            $(dvdialog).dialog("close");
        //            document.getElementById("hdnPrint").value = "true";
        //            __doPostBack('btnSave', 'OnClick');                 
        //        },

        //        "No": function () {
        //            localStorage.setItem("bSave", "true");
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            PrintQuestionnaire();
        //        },
        //        "Cancel": function () {
        //            { sessionStorage.setItem('StartLoading', 'false'); StartLoadFromPatChart(); }
        //            bPlanCancel = true;
        //            $("#btnSave")[0].disabled = false;
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        //            localStorage.setItem("bSave", "false");
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //        }

        //    }
        //});
    } else {
        document.getElementById("hdnPrint").value = "false";
        var QuryString = window.location.search;
        //CAP-2035
        if (QuryString == '?TabName=PHQ-9+Screening') {
            QuryString = "?TabName=PHQ-9%20Screening";
        }
        $.ajax({
            type: "POST",
            url: "frmHealthQuestionnaire.aspx/PrintQuestionnaire",
            data: '{strCategory: "' + QuryString.split('=')[1] + '",strDos:"'+strDOS+'",strProvider:"'+strProvider+'" }',
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: true,
            success: function (path) {
                $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', "");
                $(top.window.document).find("#ModalReport").modal({ backdrop: 'static', keyboard: false }, 'show');
                $(top.window.document).find("#mdlcontentReport")[0].style.width = "100%";
                $(top.window.document).find("#ProcessiFrameReport")[0].style.border = "1px solid #D0D0D0";
                $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', path.d);
                $(top.window.document).find("#ModalReportTtle")[0].textContent = "";
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                       ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                      "Message: " + log.Message);
                }
            }
        });
    }
}
function PrintQuestionnaire() {
    SavedSuccessfully();
    var strDOS = window.parent.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1];
    var strProvider = window.parent.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[3];
    document.getElementById("hdnPrint").value = "false";
    var QuryString = window.location.search;
    $.ajax({
        type: "POST",
        url: "frmHealthQuestionnaire.aspx/PrintQuestionnaire",
        data: '{strCategory: "' + QuryString.split('=')[1] + '",strDos:"' + strDOS + '",strProvider:"' + strProvider + '" }',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (path) {
            $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', "");
            $(top.window.document).find("#ModalReport").modal({ backdrop: 'static', keyboard: false }, 'show');
            $(top.window.document).find("#mdlcontentReport")[0].style.width = "100%";
            $(top.window.document).find("#ProcessiFrameReport")[0].style.border = "1px solid #D0D0D0";
            $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', path.d);
            $(top.window.document).find("#ModalReportTtle")[0].textContent = "";
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }
    });
}
//BugID:44324
function LoadPastEncounter() {
    document.getElementById("HdnCopyButton").value = "CheckSave";
    document.getElementById("btnCopyPrevHidden").click();
    return true;
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

function btnCopyPrevious_Clicked() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var button2 = $('#btnCopyPrevious');
    var btnSave = $('#btnSave');
    if ($('#btnSave')[0].disabled == false) {
        CopyPrevious();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    } else {
        return true;
    }
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
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function CopyPrevious() {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
        event.preventDefault();
        event.stopPropagation();
        var btnSave = $("#btnSave")[0];
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        document.getElementById("HdnCopyButton").value = "trueValidate";
        btnSave.click();
        enableAutoSave();
        $("#btnSave")[0].disabled = false;
        return;
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogROS');

        //if (!dvdialog) {
        //    dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogFocused');
        //}

        //var btnSave = $("#btnSave")[0];

        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            document.getElementById("HdnCopyButton").value = "trueValidate";
        //            btnSave.click();
        //            enableAutoSave();
        //            $("#btnSave")[0].disabled = false;
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            $("#btnSave")[0].disabled = true;
        //            disableAutoSave();
        //            document.getElementById("HdnCopyButton").value = "";
        //            document.getElementById("btnCopyPrevHidden").click();
        //            $(dvdialog).dialog("close");
        //        },
        //        "Cancel": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            $(dvdialog).dialog("close");
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            return;
        //        }
        //    }
        //});
    }
    else {
        LoadPastEncounter();
    }
}

function savedSuccessfully() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    DisplayErrorMessage('190001');
    top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    AutoSaveSuccessful();
}
function btnSaveEnabled(val) {
    if (document.getElementById("btnSave").control != undefined)
        document.getElementById("btnSave").control.disabled=((val == true) ? true : false);
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