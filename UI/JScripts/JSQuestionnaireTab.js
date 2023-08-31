var IS_Ros_Type = null;
$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "WebServices/QuestionnaireService.asmx/LoadQuestionnarieTab",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            if (data.d != '') {

                    for (var i = 0; i < data.d.split('^').length; i++) {
                        if (data.d.split('^')[i].split('$').length == 2)
                        {
                            var TabName = "#li" + data.d.split('^')[i].split('$')[0].replace(/ /g, '');
                            $("#" + data.d.split('^')[i].split('$')[0].replace(/ /g, '') + " iframe").attr("rostype", data.d.split('^')[i].split('$')[1]);
                            $(TabName)[0].attributes.style.value = "display:block";
                        }
                        else
                        {
                            var TabName = "#li" + data.d.split('^')[i].replace(/ /g, '');
                            $(TabName)[0].attributes.style.value = "display:block";
                        }
                            
                    }
                    var FirstName = $("ul#myTabs")[0].innerText.split(/\n/g)[0];
                    if (FirstName == "General") {
                        var target = $('#myTabs li:eq(0) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "TB Risk Assessment") {
                        var target = $('#myTabs li:eq(1) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Diabetic Foot Screening") {
                        var target = $('#myTabs li:eq(2) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Sleep Screening") {
                        var target = $('#myTabs li:eq(3) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Sleep") {
                        var target = $('#myTabs li:eq(4) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Pulmonary") {
                        var target = $('#myTabs li:eq(5) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Epworth Sleep Score") {
                        var target = $('#myTabs li:eq(6) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Pulmonary/Sleep Exam") {
                        var target = $('#myTabs li:eq(7) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Dermatology Questionnaire") {
                        var target = $('#myTabs li:eq(8) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Acne") {
                        var target = $('#myTabs li:eq(9) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Asthma Control Test") {
                        var target = $('#myTabs li:eq(10) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "PHQ-9 Screening") {
                        var target = $('#myTabs li:eq(11) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Depression Test") {
                        var target = $('#myTabs li:eq(12) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Neck Disability Index") {
                        var target = $('#myTabs li:eq(13) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Oswestry Disability Index") {
                        var target = $('#myTabs li:eq(14) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Development") {
                        var target = $('#myTabs li:eq(15) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    //Rjones
                    if (FirstName == "Chronic Cough Scale") {
                        var target = $('#myTabs li:eq(16) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Gynecological") {
                        var target = $('#myTabs li:eq(17) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Pediatric Sleep Questionnaire") {
                        var target = $('#myTabs li:eq(18) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Sleep Short") {
                        var target = $('#myTabs li:eq(19) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Sleep Tendency Scale") {
                        var target = $('#myTabs li:eq(20) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    //start
                    if (FirstName == "Pain Assessment") {
                        var target = $('#myTabs li:eq(21) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Functional Assessment") {
                        var target = $('#myTabs li:eq(22) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Karnofsky") {
                        var target = $('#myTabs li:eq(23) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Mini Mental") {
                        var target = $('#myTabs li:eq(24) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Nutritional Screening") {
                        var target = $('#myTabs li:eq(25) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Safety Guidelines") {
                        var target = $('#myTabs li:eq(26) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Support Needs") {
                        var target = $('#myTabs li:eq(27) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "CAT Screening") {
                        var target = $('#myTabs li:eq(28) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "COPD Breathe Well Program") {
                        var target = $('#myTabs li:eq(29) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Fall Risk Screening") {
                        var target = $('#myTabs li:eq(30) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Cognitive Screening") {
                        var target = $('#myTabs li:eq(31) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Pain Screening") {
                        var target = $('#myTabs li:eq(32) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Home Safety") {
                        var target = $('#myTabs li:eq(33) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Katz Index Screening") {
                        var target = $('#myTabs li:eq(34) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "ADL Screening") {
                        var target = $('#myTabs li:eq(35) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Get Up and Go") {
                        var target = $('#myTabs li:eq(36) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Lawton Screening") {
                        var target = $('#myTabs li:eq(37) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Anxiety Screening") {
                        var target = $('#myTabs li:eq(38) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Spine Intake") {
                        var target = $('#myTabs li:eq(39) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Monofilament Foot Exam") {
                        var target = $('#myTabs li:eq(40) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Cervical Spine") {
                        var target = $('#myTabs li:eq(41) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    } 
                    if (FirstName == "Lumbar Spine") {
                        var target = $('#myTabs li:eq(42) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Urinalysis") {
                        var target = $('#myTabs li:eq(43) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "Staying Healthy Assessment") {
                        var target = $('#myTabs li:eq(44) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    if (FirstName == "AUA BPH Symptom") {
                        var target = $('#myTabs li:eq(45) a').tab('show');
                        localStorage.setItem("PrevSubTab", target[0].innerText);
                        localStorage.setItem("bSave", "true");
                    }
                    //End

                    var Is_Ros_type = $("#" + FirstName + " iframe").attr('rostype');
                    if (FirstName == "Pulmonary/Sleep Exam") {
                        $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
                    }
                    else if (Is_Ros_type == "Y") {
                        $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
                    }
                    else {
                        $("#i" + FirstName.replace(/ /g, ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
                    }
                    target[0].innerText
                    localStorage.setItem("PrevSubTab", target[0].innerText);
            }
            else {
                DisplayErrorMessage('118504');
                { sessionStorage.setItem('StartLoading', 'true'); StopLoadFromPatChart(); }
            }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
    });
});


var bPlanCancel = false;
var PrevTab;
var CurTab;
var bPlanCancel = false;
var dvdialog;

$('.nav-tabs a').on('shown.bs.tab', function (event) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    CurTab = $(event.target);         // active tab
    PrevTab = $(event.relatedTarget);  // previous tab     
    localStorage.setItem("PrevSubTab", CurTab[0].innerText);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //$(dvdialog).dialog("close");
        //Jira #CAP-771 - check the PrevTab[0] is undefind or null
        if (PrevTab[0] != undefined && PrevTab[0] != null) {
            if (PrevTab[0].innerText == "General") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[0].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "TB Risk Assessment") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[1].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Diabetic Foot Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[2].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Sleep Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Sleep") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[4].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Pulmonary") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[5].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Epworth Sleep Score") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[6].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Pulmonary/Sleep Exam") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[7].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        //src = $(paneID).attr('data-src');
                        //$(paneID + " iframe").attr("src", src);
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Dermatology Questionnaire") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[8].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Acne") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[9].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Asthma Control Test") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[10].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "PHQ-9 Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Depression Test") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Neck Disability Index") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[13].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        $(dvdialog).dialog("close");
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Oswestry Disability Index") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[14].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Development") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[15].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Chronic Cough Scale") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[16].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Gynecological") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[17].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Pediatric Sleep Questionnaire") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[18].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Sleep Short") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[19].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Sleep Tendency Scale") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[20].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            //start
            else if (PrevTab[0].innerText == "Pain Assessment") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[21].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Functional Assessment") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[22].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Karnofsky") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[23].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Mini Mental") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[24].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Nutritional Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[25].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }

            else if (PrevTab[0].innerText == "Safety Guidelines") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[26].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Support Needs") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[27].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "CAT Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[28].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "COPD Breathe Well Program") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[29].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Fall Risk Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[30].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Cognitive Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[31].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Pain Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[32].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Home Safety") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[33].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Katz Index Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[34].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "ADL Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[35].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Get Up and Go") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[36].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Lawton Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[37].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Anxiety Screening") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[38].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Spine Intake") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[39].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Monofilament Foot Exam") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[40].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Cervical Spine") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[41].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Lumbar Spine") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[42].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Urinalysis") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[43].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "Staying Healthy Assessment") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[44].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
            else if (PrevTab[0].innerText == "AUA BPH Symptom") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[45].all.namedItem('btnSave').click();
                setTimeout(function () {
                    if (localStorage.getItem("bSave") == "true") {
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        paneID = $(event.target).attr('href');
                        ClickTab($(event.target)[0].innerText, paneID);
                    }
                    else {
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        //$(dvdialog).dialog("close");
                        //$(dvdialog).remove();
                        PrevTab.tab('show');
                        return;
                    }
                }, 1000);
            }
        }
            return;
        // {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //event.preventDefault();
        //event.stopPropagation();
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
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            $(dvdialog).dialog("close");
        //            if (PrevTab[0].innerText == "General") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[0].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText,paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }
        //            else if (PrevTab[0].innerText == "TB Risk Assessment") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[1].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }
        //            else if (PrevTab[0].innerText == "Diabetic Foot Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[2].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Sleep Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[3].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Sleep") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[4].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Pulmonary") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[5].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Epworth Sleep Score") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[6].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Pulmonary/Sleep Exam") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[7].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    //src = $(paneID).attr('data-src');
        //                    //$(paneID + " iframe").attr("src", src);
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Dermatology Questionnaire") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[8].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Acne") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[9].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Asthma Control Test") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[10].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "PHQ-9 Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[11].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Depression Test") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[12].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Neck Disability Index") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[13].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Oswestry Disability Index") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[14].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Development") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[15].all.namedItem('btnSave').click();
        //                setTimeout(function () {if (localStorage.getItem("bSave") == "true") {
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    paneID = $(event.target).attr('href');
        //                    ClickTab($(event.target)[0].innerText, paneID);
        //                }
        //                else {
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    $(dvdialog).dialog("close");
        //                    $(dvdialog).remove();
        //                    PrevTab.tab('show');
        //                    return;
        //                }},1000);
        //            }

        //            else if (PrevTab[0].innerText == "Chronic Cough Scale") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[16].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }

        //            else if (PrevTab[0].innerText == "Gynecological") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[17].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Pediatric Sleep Questionnaire") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[18].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Sleep Short") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[19].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Sleep Tendency Scale") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[20].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //                //start
        //            else if (PrevTab[0].innerText == "Pain Assessment") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[21].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Functional Assessment") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[22].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Karnofsky") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[23].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Mini Mental") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[24].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }

        //            else if (PrevTab[0].innerText == "Nutritional Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[25].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }

        //            else if (PrevTab[0].innerText == "Safety Guidelines") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[26].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Support Needs") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[27].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "CAT Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[28].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "COPD Breathe Well Program") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[29].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Fall Risk Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[30].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Cognitive Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[31].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Pain Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[32].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Home Safety") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[33].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Katz Index Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[34].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "ADL Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[35].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Get Up and Go") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[36].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Lawton Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[37].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Anxiety Screening") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[38].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Spine Intake") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[39].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Monofilament Foot Exam") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[40].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Cervical Spine") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[41].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Lumbar Spine") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[42].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Urinalysis") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[43].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "Staying Healthy Assessment") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[44].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            else if (PrevTab[0].innerText == "AUA BPH Symptom") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[45].all.namedItem('btnSave').click();
        //                setTimeout(function () {
        //                    if (localStorage.getItem("bSave") == "true") {
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        paneID = $(event.target).attr('href');
        //                        ClickTab($(event.target)[0].innerText, paneID);
        //                    }
        //                    else {
        //                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                        $(dvdialog).dialog("close");
        //                        $(dvdialog).remove();
        //                        PrevTab.tab('show');
        //                        return;
        //                    }
        //                }, 1000);
        //            }
        //            return;
        //        },
        //        "No": function () {
        //            var vv;
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            paneID = $(event.target).attr('href');
        //            var FirstName = $(event.target)[0].innerText;
        //            var Ros_type = $(paneID + " iframe").attr('rostype');
        //            if (FirstName == "Pulmonary/Sleep Exam") {
        //                $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
        //                vv = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
        //            }
        //            else if (Ros_type == "Y") {
        //                $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
        //                vv = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
        //            }
        //            else {
        //                $("#i" + FirstName.replace(/ /g, ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
        //                vv = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
        //            }
        //            src = $(paneID).attr('data-src');
        //            src = vv;
        //            $(paneID + " iframe").attr("src", src);

        //            $(paneID + " iframe").attr("src", src);

        //        },
        //        "Cancel": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            bPlanCancel = true;
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            PrevTab.tab('show');
        //            return;

        //        }
        //    }
        //});
    }
    else {
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
            $(dvdialog).remove();
        }
        if (bPlanCancel == false) {
            var vv;
            localStorage.setItem("bSave", "false");
            paneID = $(event.target).attr('href');
            var FirstName = $(event.target)[0].innerText;
            var Ros_type=$(paneID + " iframe").attr('rostype');
            if (FirstName == "Pulmonary/Sleep Exam") {
                $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
                vv = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
            }
            else if (Ros_type == "Y") {
                $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
                vv = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
            }
            else {
                $("#i" + FirstName.replace(/ /g, ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
                vv = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
            }
            src = $(paneID).attr('data-src');
            src = vv;
            $(paneID + " iframe").attr("src", src);
        } else {
            bPlanCancel = false;
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }

    }
});

function ClickTab(value, paneID) {
    var vv;
    var FirstName = value;
    localStorage.setItem("bSave", "false");
    var Is_Ros_type = $(paneID + " iframe").attr('rostype');
    if (FirstName == "Development") {
        var target = $('#myTabs li:eq(15) a').tab('show');
    }
    if (FirstName == "Pulmonary/Sleep Exam") {
        $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
        vv = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
    }
    else if (Is_Ros_type == "Y") {
        $("#i" + FirstName.replace(/ /g, '').replace('/', ''))[0].attributes[3].value = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
        vv = "frmReviewOfQuestionnaire.aspx?TabName=" + FirstName;
    }
    else {
        $("#i" + FirstName.replace(/ /g, ''))[0].attributes[3].value = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
        vv = "frmHealthQuestionnaire.aspx?TabName=" + FirstName;
    }
  
    if (vv != null)
    {
        src = $(paneID).attr('data-src');
        src = vv;
        $(paneID + " iframe").attr("src", src);
    }
   
}



