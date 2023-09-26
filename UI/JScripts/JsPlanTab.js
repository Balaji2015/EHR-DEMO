var plan_tab = [];
var plan_tab_to_disable;
var Sessionvalues;
$(document).ready(function () {
    localStorage.setItem('QueryStr', "");
    localStorage.setItem('DOS_Plan', "");
    if ($("ul#myTabs li.active").length != 0) {
        if (localStorage.getItem("notification") == 'CarePlan') {
            $('#myTabs a')[1].click();
            paneID = $(target).attr('href');
            var target = $('#myTabs li:eq(1) a').tab('show')
            localStorage.setItem("PrevSubTab", target[0].innerText);
            localStorage.setItem("bSave", "true");
        }
        else if (localStorage.getItem("bSave") == "true") {
            var target = $('#myTabs a:first').tab('show');
            localStorage.setItem("PrevSubTab", target[0].innerText);
            paneID = $(target).attr('href');
        }
        else {
            if (localStorage.getItem("PrevSubTab") == "General Plan") {
                var target = $('#myTabs a:first').tab('show');
                localStorage.setItem("PrevSubTab", target[0].innerText);
                localStorage.setItem("bSave", "true");
                paneID = $(target).attr('href');
            }
            else if (localStorage.getItem("PrevSubTab") == "Individualized CarePlan") {
                var target = $('#myTabs li:eq(1) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                localStorage.setItem("bSave", "true");
                paneID = $(target).attr('href');
            }
            else if (localStorage.getItem("PrevSubTab") == "Preventive Screen Plan") {
                var target = $('#myTabs li:eq(2) a').tab('show')
                localStorage.setItem("PrevSubTab", target[0].innerText);
                localStorage.setItem("bSave", "true");
                paneID = $(target).attr('href');
            }
            else {
                var target = $('#myTabs a:first').tab('show');
                localStorage.setItem("PrevSubTab", target[0].innerText);
                localStorage.setItem("bSave", "true");
                paneID = $(target).attr('href');
            }
        }
        $.ajax({
            type: "POST",
            url: "WebServices/PlanService.asmx/LoadPlanTab",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                src = $(paneID).attr('data-src') + "?" + (data.d).split('-')[0];
                var tab_disable = (data.d).split('-')[2].split('=')[1];
                if (tab_disable != "") {
                        plan_tab_to_disable = JSON.parse((data.d).split('-')[2].split('=')[1]);
                        for (var i = 0; i < plan_tab_to_disable.length; i++) {
                            plan_tab.push(plan_tab_to_disable[i].tab.split('_')[1].replace("tb", ""));
                            $("#myTabs a[href*='" + plan_tab[i] + "']").addClass("disableTab");
                        }
                }
                $(paneID + " iframe").attr("src", src);
                localStorage.setItem('QueryStr', "?&" + data.d);//BugID:47526
                if (target[0].innerText == "General Plan") {
                    src += "?&" + data.d;
                }
                $(paneID).attr('data-src', src);
                $(paneID + " iframe").attr("src", src);
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    //CAP-798 Unexpected end of JSON input
                    if (isValidJSON(xhr.responseText)) {
                        var log = JSON.parse(xhr.responseText);
                    
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        "Message: " + log.Message);
                    } else {
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry.");
                    }

                }
                 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            }
        });

    }
});
$('#myTabs li a').click(function () {
    
    var is_disabled = false;
    if (plan_tab != undefined && plan_tab.length > 0) {
        for (var t = 0; t < plan_tab.length; t++) {
            if (this.hash == "#" + plan_tab[t]) {
                event.stopPropagation();
                is_disabled = true;
            }
        }
        if (is_disabled == false) {
            localStorage.setItem("notification", "");
            if ($("ul#myTabs li.active a")[0].innerText != this.innerText)
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
    }
    else {
        localStorage.setItem("notification", "");
        if ($("ul#myTabs li.active a")[0].innerText != this.innerText)
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }
})
var PrevTab;
var CurTab;
var bPlanCancel = false;
var dvdialog;

$('.nav-tabs a').on('shown.bs.tab', function (event) {
    
    var is_disabled = false;
    if (plan_tab != undefined && plan_tab.length > 0) {
        for (var t = 0; t < plan_tab.length; t++) {
            if (this.hash == "#" + plan_tab[t])//to disable BodyImage Tab
            {
                event.stopPropagation();
                is_disabled = true;
            }
        }
        if (is_disabled == false) {
            loadplanTabs(event);
        }
    }
    else
        loadplanTabs(event);

});
function loadplanTabs(event) {
    CurTab = $(event.target);         // active tab
    PrevTab = $(event.relatedTarget);  // previous tab     
    localStorage.setItem("PrevSubTab", CurTab[0].innerText);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            event.preventDefault();
            event.stopPropagation();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //$(dvdialog).dialog("close");
            //$(dvdialog).remove();
            if (PrevTab[0].innerText == "General Plan") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[0].all.namedItem('btnSave').click();
                if (localStorage.getItem("bSave") == "true") {
                    paneID = $(event.target).attr('href');
                    src = $(paneID).attr('data-src');
                    $(paneID + " iframe").attr("src", src);
                }
                else {
                    bPlanCancel = true;
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    PrevTab.tab('show');
                    return;
                }
            }
            else if (PrevTab[0].innerText == "Individualized CarePlan") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[1].all.namedItem('btnSave').click();
                if (localStorage.getItem("bSave") == "true") {
                    paneID = $(event.target).attr('href');
                    src = $(paneID).attr('data-src');
                    $(paneID + " iframe").attr("src", src);
                }
                else {
                    bPlanCancel = true;
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    PrevTab.tab('show');
                    return;
                }
            }
            else if (PrevTab[0].innerText == "Preventive Screen Plan") {
                event.preventDefault();
                event.stopPropagation();
                $('.clsIframe').contents()[2].all.namedItem('btnSave').click();
                if (localStorage.getItem("bSave") == "true") {
                    paneID = $(event.target).attr('href');
                    src = $(paneID).attr('data-src');
                    $(paneID + " iframe").attr("src", src);
                }
                else {
                    bPlanCancel = true;
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    PrevTab.tab('show');
                    return;
                }
            }

            return;

            //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            if (PrevTab[0].innerText == "General Plan") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[0].all.namedItem('btnSave').click();
        //                if (localStorage.getItem("bSave") == "true") {
        //                    paneID = $(event.target).attr('href');
        //                    src = $(paneID).attr('data-src');
        //                    $(paneID + " iframe").attr("src", src);
        //                }
        //                else {
        //                    bPlanCancel = true;
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    PrevTab.tab('show');
        //                    return;
        //                }
        //            }
        //            else if (PrevTab[0].innerText == "Individualized CarePlan") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[1].all.namedItem('btnSave').click();
        //                if (localStorage.getItem("bSave") == "true") {
        //                    paneID = $(event.target).attr('href');
        //                    src = $(paneID).attr('data-src');
        //                    $(paneID + " iframe").attr("src", src);
        //                }
        //                else {
        //                    bPlanCancel = true;
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    PrevTab.tab('show');
        //                    return;
        //                }
        //            }
        //            else if (PrevTab[0].innerText == "Preventive Screen Plan") {
        //                event.preventDefault();
        //                event.stopPropagation();
        //                $('.clsIframe').contents()[2].all.namedItem('btnSave').click();
        //                if (localStorage.getItem("bSave") == "true") {
        //                    paneID = $(event.target).attr('href');
        //                    src = $(paneID).attr('data-src');
        //                    $(paneID + " iframe").attr("src", src);
        //                }
        //                else {
        //                    bPlanCancel = true;
        //                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //                    PrevTab.tab('show');
        //                    return;
        //                }
        //            }

        //            return;
        //        },
        //        "No": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            paneID = $(event.target).attr('href');
        //            var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
        //            if (HtmlVersion.indexOf('?') > -1) {
        //                if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) {
        //                    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
        //                }
        //            }
        //            else
        //                var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");

        //            $(paneID).attr('data-src', HtmlVersion);
        //            src = $(paneID).attr('data-src');
        //            $(paneID + " iframe").attr("src", src);

        //        },
        //        "Cancel": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
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
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
            $(dvdialog).remove();
        }
        if (bPlanCancel == false) {
            paneID = $(event.target).attr('href');
            var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
            if (HtmlVersion.indexOf('?') > -1) {
                //Jira - #CAP-80
                //if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) {
                if (HtmlVersion.split('?')[1].split("=")[1] != localStorage.getItem("ScriptVersion")) {
                    //Jira - #CAP-80
                    //var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
                    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + localStorage.getItem("ScriptVersion");
                }
            }
            else {
                //Jira - #CAP-80
                //var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");
                var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + localStorage.getItem("ScriptVersion");
            }
            $(paneID).attr('data-src', HtmlVersion);
            src = $(paneID).attr('data-src');
            $(paneID + " iframe").attr("src", src);
        } else {
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
            bPlanCancel = false;
        }

    }
    var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue;
    if (HtmlVersion.indexOf('?') > -1) {
        //Jira - #CAP-80
        //if (HtmlVersion.split('?')[1].split("=")[1] != sessionStorage.getItem("ScriptVersion")) 
        if (HtmlVersion.split('?')[1].split("=")[1] != localStorage.getItem("ScriptVersion")) {
            //Jira - #CAP-80
            //var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + sessionStorage.getItem("ScriptVersion");
            var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue.split('?')[0] + "?version=" + localStorage.getItem("ScriptVersion");
        
        }
    }
    else {
        //Jira - #CAP-80
        //var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + sessionStorage.getItem("ScriptVersion");
        var HtmlVersion = $(paneID)[0].attributes.getNamedItem('data-src').nodeValue + "?version=" + localStorage.getItem("ScriptVersion");
    }
    $(paneID).attr('data-src', HtmlVersion);
}


