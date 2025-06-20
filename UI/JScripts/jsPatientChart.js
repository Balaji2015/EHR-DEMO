
var summarcheck = "";


function OpenReviewCodingException() {

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
    return false;
}

function OpenCreateCodingException() {

    $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Create Coding Exception";
    $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
    $(top.window.document).find("#TabmdldlgException")[0].style.height = "680px";
    var sPath = ""
    var patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
    sPath = "frmException.aspx?formName=" + "Create Coding Exception" + "&PatientName=" + patientName;
    $(top.window.document).find("#TabExceptionFrame")[0].style.height = "605px";
    $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
    });

    return false;
}


function openResultViewer() {
    { sessionStorage.setItem('StartLoading', 'true'); if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1) StartLoadFromPatChart(); }
    var obj = new Array();
    obj.push("Type=" + "Results");
    obj.push("HumanId=" + document.getElementById('ctl00_C5POBody_hdnHumanNo').value);
    obj.push("OrderSubmitId=" + document.getElementById('ctl00_C5POBody_hdnOrderSubmitId').value);
    obj.push("ResultId=" + document.getElementById('ctl00_C5POBody_hdnResultId').value);
    var result = openModal("frmViewResult.aspx", 900, 1200, obj, "ViewResult");
    return false;
}

function openViewer() {
    { sessionStorage.setItem('StartLoading', 'true'); if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1) StartLoadFromPatChart(); }
    var obj = new Array();
    obj.push("Type=" + "File");
    obj.push("HumanId=" + document.getElementById('ctl00_C5POBody_hdnHumanNo').value);
    obj.push("IndexId=" + document.getElementById('ctl00_C5POBody_hdnIndexId').value);
    var result = openModal("frmViewResult.aspx", 900, 1200, obj, "ViewResult");
    return false;
}

function OpenPhoneEncounter() {
    var obj = new Array();
    obj.push("openingfrom=" + "Menu");
    obj.push("MyHumanID=" + document.getElementById('ctl00_C5POBody_hdnHumanNo').value);
    //CAP-601 - validate encounter for phone encounter.
    var src = $('#ctl00_C5POBody_EncounterContainer').attr('src')
    if (src?.indexOf("frmEncounter.aspx") >= 0) {
        DisplayErrorMessage('1011198');
    } else {
        var result = openModal("HtmlPhoneEncounter.html", 800, 1200, obj, "PhoneEncounter");
    }
}

function trvPatientChart_NodeClicking(evt) {
    var EncounterDocument = document.getElementById('ctl00_C5POBody_EncounterContainer').contentDocument;
    if (EncounterDocument.getElementById('pageContainer') != null && EncounterDocument.getElementById('pageContainer') != undefined) {
        var EncounterMultiPage = EncounterDocument.getElementById('pageContainer').control;
        var TabClick = EncounterDocument.getElementById('hdnTabClick');
        if (TabClick.value == "first") {
            if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
                TabClick.value = evt.currentTarget.textContent.trim() + "$#$";
                evt.preventDefault();
                evt.stopPropagation();
                DisplayErrorMessage('1100000', 'PatientChartTreeViewNodeClick');
                return;
            }
        }
        else {
            var splitvalue = TabClick.value.split('$#$');
            var clicked_tab = splitvalue[0];
            var switchcase = splitvalue[1];
            if (switchcase == "second,true") {
                var IDs = EncounterDocument.getElementById('hdnSaveButtonID').value.split(',');
                if (IDs.length == 1) {
                    var save_button = window.frames[0].frameElement.contentDocument.getElementById(IDs[0]);
                    if (save_button == null)
                        save_button = EncounterMultiPage.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById(IDs[0]);
                    if (save_button != undefined || save_button != null) {
                        evt.preventDefault();
                        evt.stopPropagation();
                        TabClick.value = clicked_tab + "$#$third$#$Node";
                        save_button.click();
                        return;
                    }
                }
                else if (IDs.length == 2) {
                    var childControlsofParentContainer = EncounterMultiPage.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
                    for (var count = (childControlsofParentContainer.length - 1) ; count >= 0; count--) {
                        if (childControlsofParentContainer[count]._element.id == IDs[1]) {
                            var MultiPage = childControlsofParentContainer[count];
                            break;
                        }
                    }
                    var childControlsofChildContainer = MultiPage.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow.$telerik.radControls;
                    for (var count = (childControlsofChildContainer.length - 1) ; count >= 0; count--) {
                        if (childControlsofChildContainer[count]._element.id == IDs[0]) {
                            var save_button = childControlsofChildContainer[count];
                            if (MultiPage.get_selectedPageView()._contentUrl.indexOf('frmOtherHistory.aspx') > -1)
                                var add_button = MultiPage.get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentDocument.getElementById('btnSave');
                            if (save_button != undefined && save_button != null) {
                                evt.preventDefault();
                                evt.stopPropagation();
                                TabClick.value = clicked_tab + "$#$third$#$Node";
                                save_button.click();
                                if (add_button != undefined && add_button != null) {
                                    if (add_button.control._enabled)
                                        add_button.click();
                                }
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            else if (switchcase == "second,false") {
                window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            }
            else if (switchcase == "second,cancel") {
                evt.preventDefault();
                evt.stopPropagation();
                TabClick.value = "first";
                return;
            }

            TabClick.value = "first";
            evt.stopPropagation();
        }
    }
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
}

function lblPatientStrip_ItemClicked(sender, args) {
    var tableid = document.getElementById('divpanel');
    if (tableid.style.display == '')
    { tableid.style.display = 'none'; document.cookie = "ToggleMode=invis"; }
    else { tableid.style.display = ''; document.cookie = "ToggleMode=vis"; }
}

function load() {
    var tableid = document.getElementById('divpanel');
    var Mode = getCookie("ToggleMode");
    if (Mode == "invis") { tableid.style.display = 'none'; }
    if (Mode == "vis") { tableid.style.display = '' }
}

function getCookie(k) { var v = document.cookie.match('(^|;) ?' + k + '=([^;]*)(;|$)'); return v ? v[2] : null }

function trvPatinetChart_ContextMenuItemClicking(sender, args) {
    setTimeout(
   function () {
       var oWnd = $find('ctl00_C5POBody_RadWindowSummary');
       var childWindow = oWnd.BrowserWindow.radopen("frmSummary.aspx?Visit=True&Mode=ContextMenu&Encounter_id=" + args.get_node()._getNodeData().Value, "ctl00_C5POBody_RadWindowSummary");
       childWindow.SetModal(false);
       childWindow.set_visibleStatusbar(false);
       childWindow.setSize(1100, 800);
       childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Minimize);
       childWindow.set_iconUrl("Resources/16_16.ico");
       childWindow.set_keepInScreenBounds(true);
       childWindow.set_centerIfModal(true);
       childWindow.center();
   }, 0);
    var item = args.get_menuItem();
    var menu = item.get_menu();
    menu.hide();
}
function getQueryStrings() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = location.search.substring(1);
    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}
$(document).ready(function () {
    if (sessionStorage.getItem("nav") == null) {
        $('nav').attr('style', sessionStorage.getItem("nav"));
        $('li  div').css({ "background-color": sessionStorage.getItem("navhover") });
    }
    //BugID:52521 
    if (sessionStorage.getItem("Enc_Tree") != null)
        sessionStorage.removeItem("Enc_Tree");
    if (sessionStorage.getItem("Enc_SelectDOS") != null)
        sessionStorage.removeItem("Enc_SelectDOS");
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.cookie = "VitalCurrentDate=" + utc;
    localStorage.setItem("bSave", "true");
    $('#ctl00_C5POBody_trvPatinetChart li').on("click", trvPatientChart_NodeClicking);
    if (screen.width > 1280)
        diff = 11;
    else
        diff = 2;
    //Jira CAP-1237
    //$('#jqxSplitter').css({ "width": "100%", "height": "739" });
    $('#jqxSplitter').css({ "width": "100%", "height": "750" });
    var queryString = window.location.search.toString().split('?')[1];
    if (queryString != undefined && queryString != "") {


        if (queryString.split("&") != undefined && queryString.split("&").length > 1) {
            summarcheck = queryString.split("&")[1].split('=')[1];
            if (queryString.split("&")[3] != undefined && queryString.split("&")[3].indexOf("PSBEncID=") > -1) {
                sessionStorage.setItem("EncId_PatSummaryBar", queryString.split("&")[3].split("=")[1]);
                //Jira CAP-1587
                //sessionStorage.setItem("Enc_DOS", queryString.split("&")[4].split("=")[1].replace("%20", " ").replace("%20", " "));
                sessionStorage.setItem("Enc_DOS", queryString.split("&")[4].split("=")[1].replace("%20", " ").replace("%20", " ").replaceAll("%3A", ":").replaceAll("%3a", ":"));
            }
            //Jira #CAP-337
            if (queryString.split("&")[3] != undefined && queryString.split("&")[3].indexOf("EncounterID") > -1) {
                sessionStorage.setItem("EncId_PatSummaryBar", queryString.split("&")[3].split("=")[1]);
            }
            if (queryString.split("&")[2] != undefined && queryString.split("&")[2].indexOf("EncounterDate") > -1) {
                //Jira CAP-1587
                //sessionStorage.setItem("Enc_DOS", queryString.split("&")[2].split("=")[1].replace("T", " ").replace("%20", " ").replace("%20", " "));
                sessionStorage.setItem("Enc_DOS", queryString.split("&")[2].split("=")[1].replace("T", " ").replace("%20", " ").replace("%20", " ").replaceAll("%3A", ":").replaceAll("%3a", ":"));
            }

        }
        else
            summarcheck = "";


    }
    if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1) {
        //CAP-1518
        var encounter_Id = parseInt(document.getElementById(GetClientId('hdnSummaryEncID'))?.value) ?? 0;
        if (encounter_Id > 0) {
            sessionStorage.setItem("EncId_PatSummaryBar", encounter_Id);
        }

        reloadSummary();
    }
    else {
        $('#resultLoading').css("display", "none");
        $("#divpanel").css("display", "none")
        $("#trmaintab").css("display", "none");
        $("#trgeneral").css("display", "none");
        $("#MainMenu").css("display", "none");
        $("#tbGeneral").css("display", "none");
        $(".notify").css("display", "none")

    }
    var qs = getQueryStrings();
    var myParam = qs["openingfrom"];
    if (myParam == "Menu") {
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").css("background-color", "rgb(109, 119, 119)");
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").addClass("not-active");
    }


    $("div.bhoechie-tab-menu>div.list-group>a").click(function (e) {
        e.preventDefault();
        $(this).siblings('a.active').removeClass("active");
        $(this).addClass("active");
        var index = $(this).index();
        $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
        $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
    });
    if (document.getElementById(GetClientId('hdnOwnerEncMismatch')).value.toUpperCase() == "TRUE") {
          //$('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + document.getElementById(GetClientId('hdnOwnerEncMismatchEncID')).value;
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + document.getElementById(GetClientId('hdnOwnerEncMismatchEncID')).value + "&TabMode=true";
        DisplayErrorMessage('5001');
    }  
    //CAP-1167
    //CAP-1086
    var enc_Id = parseInt(document.getElementById(GetClientId('hdnSummaryEncID'))?.value) ?? 0;
    if (document.getElementById(GetClientId('hdnSummaryPageFlag')).value.toUpperCase() == "TRUE" && enc_Id > 0) {
        sessionStorage.setItem("EncId_PatSummaryBar", enc_Id);
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + enc_Id + "&TabMode=true";
    }
    else if (document.getElementById(GetClientId('hdnSummaryPageFlag')).value.toUpperCase() == "FALSE" && enc_Id > 0) {
        var now = new Date();
        var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
        document.getElementById(GetClientId("hdnLocalTime")).value = utc;

        sessionStorage.setItem("EnablePFSHMenu", "true");
        localStorage.setItem("CodingException", enc_Id);
        localStorage.setItem("CurrentProcess", $("#hdnEncCurrentProcess").val());
        sessionStorage.setItem("EncId_PatSummaryBar", enc_Id);
        sessionStorage.setItem("Enc_DOS", document.getElementById(GetClientId('hdnEncDos'))?.value);
        //CAP-1511
        var encounterURL = "frmEncounter.aspx?Date=" + document.getElementById(GetClientId("hdnLocalTime")).value + "&EncounterID=" + enc_Id;
        var currentTabNameId = document.getElementById(GetClientId('hdnScreen'))?.value;
        var currentSubTabNameId = document.getElementById(GetClientId('hdnSubScreen'))?.value;
        if (currentTabNameId != undefined && currentTabNameId != null && currentTabNameId != "") {
            encounterURL += "&Screen=" + currentTabNameId;
        }
        if (currentSubTabNameId != undefined && currentSubTabNameId != null && currentSubTabNameId != "") {
            encounterURL += "&SubScreen=" + currentSubTabNameId;
        }
        $('#ctl00_C5POBody_EncounterContainer')[0].src = encounterURL;
    }
});

function OnClientCloseWindow() {   //BugID:42368
}

function reloadSummary() {
    var enc_id = sessionStorage.getItem("EncId_PatSummaryBar");
    var enc_DOS = sessionStorage.getItem("Enc_DOS");
    //CAP-2596
    var encounterId = parseInt(enc_id);
    if ((encounterId??0) > 0) {
        //sessionStorage.removeItem("EncId_PatSummaryBar");
        //sessionStorage.removeItem("Enc_DOS");
        $.ajax({
            type: "POST",
            url: "frmRCopiaToolbar.aspx/LoadPatientSummaryBar",
            data: JSON.stringify({ EncID: enc_id, Enc_DOS: enc_DOS }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessSummaryBar,
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    //CAP-792
                    if (isValidJSON(xhr.responseText)) {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //Jira CAP-1587
                        //if (log.Message.indexOf("Unexpected end of file") > 0 && log.Message.indexOf("There is an unclosed literal string") > 0 &&
                        //    log.Message.indexOf("is an unexpected token") > 0) {
                        if (log.Message.indexOf("Human XML is invalid") == -1 && log.Message.indexOf("Human XML is not found") == -1 && log.Message.indexOf("Encounter XML is invalid") == -1 && log.Message.indexOf("Encounter XML is not found") == -1) {
                            alert("USER MESSAGE:\n" +
                                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                "Message: " + log.Message);
                        }
                        //}
                        //Jira CAP-1587
                        //else {
                        //    alert("USER MESSAGE:\n" +
                        //        ". Cannot process request. Please Login again and retry.");
                        //}
                    }
                    //Jira CAP-1587
                    else {

                        //CAP-3319 - Applying null safety check
                        if (xhr.responseText?.indexOf("Human XML is invalid") == -1 && xhr.responseText?.indexOf("Human XML is not found") == -1 && xhr.responseText?.indexOf("Encounter XML is invalid") == -1 && xhr.responseText?.indexOf("Encounter XML is not found") == -1) {
                            alert("USER MESSAGE:\n" + ". Cannot process request. Please Login again and retry.");
                        }
                        else if (xhr.responseText == null) {
                            alert("USER MESSAGE:\n" + ". Cannot process request. Please Login again and retry.");
                        }
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }

        });
    }
    
}
var list = "";
var iCount = 0;
var activemenu = "";
function CheckMe(MenuName, e) {
    if (MenuName != 'Exam') {
        if ($("#ctl00_C5POBody_dvTest")[0].style.display == "block") {

            $("#ctl00_C5POBody_dvTest").css("display", "none");
            $("#divPatChartContainer").css("display", "none");
            $("#Encountersimg")[0].style.color = "#3275B1";
            $("#EncountersText")[0].style.color = "#000000";
            $("#Resultsimg")[0].style.color = "#3275B1";
            $("#ResultsText")[0].style.color = "#000000";
            $("#PatientTaskimg")[0].style.color = "#3275B1";
            $("#PatientTaskText")[0].style.color = "#000000";
            $("#Examimg")[0].style.color = "#3275B1";
            $("#ExamText")[0].style.color = "#000000";
            $("#Documentimg")[0].style.color = "#3275B1";
            $("#DocumentText")[0].style.color = "#000000";
            $("#SummaryofCareimg")[0].style.color = "#3275B1";
            $("#SummaryofCareText")[0].style.color = "#000000";
            $("#Analyticsimg")[0].style.color = "#3275B1";
            $("#AnalyticsText")[0].style.color = "#000000";

        }
        else {
            $("#ctl00_C5POBody_dvTest").css("display", "block");
            $("#divPatChartContainer").css("display", "block");
            $("#Resultsimg")[0].style.color = "#3275B1";
            $("#ResultsText")[0].style.color = "#000000";
            activemenu = "";
        }

    }
    else {
        if ($("#ctl00_C5POBody_dvTest")[0].style.display == "block") {

            $("#ctl00_C5POBody_dvTest").css("display", "none");
            $("#divPatChartContainer").css("display", "none");
            $("#Encountersimg")[0].style.color = "#3275B1";
            $("#EncountersText")[0].style.color = "#000000";
            $("#Resultsimg")[0].style.color = "#3275B1";
            $("#ResultsText")[0].style.color = "#000000";
            $("#PatientTaskimg")[0].style.color = "#3275B1";
            $("#PatientTaskText")[0].style.color = "#000000";
            $("#Examimg")[0].style.color = "#3275B1";
            $("#ExamText")[0].style.color = "#000000";
            $("#Documentimg")[0].style.color = "#3275B1";
            $("#DocumentText")[0].style.color = "#000000";
            $("#SummaryofCareimg")[0].style.color = "#3275B1";
            $("#SummaryofCareText")[0].style.color = "#000000";
            $("#Analyticsimg")[0].style.color = "#3275B1";
            $("#AnalyticsText")[0].style.color = "#000000";

        }
    }
    if (activemenu != MenuName) {
        $(e)[0].style.color = "#fe5c00";
        $(e)[0].nextElementSibling.style.color = "#fe5c00";
        if (MenuName != 'Exam') {
            $("#divPatChartContainer").css("display", "block");
        }
        $("#ctl00_C5POBody_dvTest").css("display", "block");

        activemenu = MenuName;

        jQuery(top.window.parent.document).find('#divLoadingPatChart').css('display', "block");
        var WSData = "{\"text\":\"" + document.getElementById('ctl00_C5POBody_hdnHumanNo').value + "|" + MenuName + "\"}";
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/SearchDescrption",
            data: WSData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#divTreeview').empty();
                makeUL(data.d, MenuName)
                $('#divTreeview')[0].appendChild(list);
                list = "";
                jQuery(top.window.parent.document).find('#divLoadingPatChart').css('display', "none");
                $("#ctl00_C5POBody_dvCheck").find("li").removeClass("collapsed");
                $("#liEncounters").removeClass("collapsed");

            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    //CAP-792
                    if (isValidJSON(xhr.responseText)) {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                            ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                            log.ExceptionType + " \nMessage: " + log.Message);
                    }
                    else {
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry.");
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
        iCount++;


        $('#ctl00_C5POBody_dvCheck')[0].style.display = "block";

    }

}

var backcolor = '';

function makeUL(array, MenuName) {
    list = document.createElement('ul');
    if (MenuName == 'Encounters') {
        var item = document.createElement('li');
        item.className = "node";
        item.id = "liEncounters";
        var span1 = document.createElement('span');
        span1.id = "sEncounterID";
        span1.className = "leaf";
        span1.innerHTML = "Encounters";
        item.appendChild(span1);
        var span2 = document.createElement('span');
        span2.className = "node-toggle";
        item.appendChild(span2);

        var list1 = document.createElement('ul');
        var item1 = document.createElement('li');
        item1.className = "active";
        var sCheck = '';
        var nodepresent = false;

        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Encounters") {
                if (array[i].split('^')[2].toUpperCase().indexOf("01-JAN-0001") >= 0) {
                    window.location.href = "frmSessionExpired.aspx"
                    break;

                }
                var sStrinh = "";

                if (window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument != null && window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument != undefined) {
                    if ((window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs') != null) && window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs') != undefined) {

                        if (window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs').innerHTML.split('|').length > 1)

                            sStrinh = window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs').innerHTML.split('|')[1].trim()
                    }
                }



                var ulistEnc = document.createElement('ul');
                var listEnc = document.createElement('li');
                var spaninner = document.createElement('p');
                spaninner.className = "para";
                spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[1];
                //spaninner.title = array[i].split('^')[2].split(';')[0] + " - " + array[i].split('^')[2].split(';')[1];
                spaninner.currentId = array[i].split('^')[3];
                spaninner.From = array[i].split('^')[4];
                spaninner.innerHTML = array[i].split('^')[2].split(';')[0] + " - " + array[i].split('^')[2].split(';')[1] + " - " + array[i].split('^')[5];
                var sClickNode = sessionStorage.getItem("Enc_Tree");
                var sClickDos = sessionStorage.getItem("Enc_SelectDOS");
                if (document.getElementById(GetClientId('hdnOwnerEncMismatch')).value.toUpperCase() == "TRUE") {//changed for carePointe
                    if (document.getElementById(GetClientId('hdnOwnerEncMismatchEncID')).value == array[i].split('^')[1]) {
                        $(spaninner).addClass("colored");
                        backcolor = spaninner;
                    }
                }
                else {
                    if (sStrinh == array[i].split('^')[2].split(';')[0] || (sStrinh == "" && sClickDos == array[i].split('^')[2].split(';')[0])) {
                        $(spaninner).addClass("colored");
                        backcolor = spaninner;
                    }
                }
                listEnc.appendChild(spaninner);
                ulistEnc.appendChild(listEnc);
                item1.appendChild(ulistEnc);
                list1.appendChild(item1);
            }


        }
        item.appendChild(list1);
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "EncountersSub"){// && array[i].split('^')[8]!="0") {
                //if ($(item).children().children().find("p:contains('" + array[i].split('^')[2].split(' ')[0] + "')").length > 0) {
                //    var itemsub = $(item).children().children().find("p:contains('" + array[i].split('^')[2].split(' ')[0] + "')").parent()[0];
                //    itemsub.id = "subEncounters";
                //    nodepresent = true;
                //}
                if (array[i].split('^')[8]!="0" && $(item).children().children().find("p[id='Encounters^" + array[i].split('^')[8]+ "']").length > 0) {
                    var itemsub = $(item).children().children().find("p[id='Encounters^" + array[i].split('^')[8] + "']").parent()[0];
                    itemsub.id = "subEncounters";
                    nodepresent = true;
                }
                else if (sCheck != array[i].split('^')[2].split(' ')[0]) {
                    if ($(item).children().children().find("span:contains('" + array[i].split('^')[2].split(' ')[0] + "')").length > 0) {
                        var itemsub = $(item).children().children().find("span:contains('" + array[i].split('^')[2].split(' ')[0] + "')").parent()[0];
                        nodepresent = true;
                    }
                    else {
                        var itemsub = document.createElement('li');
                        itemsub.className = "node";
                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = array[i].split('^')[2].split(' ')[0];
                        sCheck = span1.innerHTML;
                        itemsub.appendChild(span1);

                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        itemsub.appendChild(span2);
                        nodepresent = false;
                    }

                }
                itemsub.className = "node";
                var spanEnc = document.createElement('span');
                spanEnc.className = "node-toggle";
                var list1sub = document.createElement('ul');
                var item1sub = document.createElement('li');
                var spaninner = document.createElement('p');
                spaninner.className = "para";
                spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[1] + "^" + array[i].split('^')[6] + "^" + array[i].split('^')[7];
                spaninner.innerHTML = array[i].split('^')[3];

                item1sub.appendChild(spaninner);
                list1sub.appendChild(item1sub);
                itemsub.appendChild(spanEnc);
                itemsub.appendChild(list1sub);
                if (nodepresent == false) {
                    list1.appendChild(itemsub);
                }
            }
        }
        item.appendChild(list1);
        list.appendChild(item);

        var itemPhoneEncounter = document.createElement('li');
        itemPhoneEncounter.id = "liPhoneEncounter";
        itemPhoneEncounter.className = "node";


        var span1 = document.createElement('span');

        span1.className = "leaf";
        span1.innerHTML = "Phone Encounter";
        itemPhoneEncounter.appendChild(span1);

        var span2 = document.createElement('span');
        span2.className = "node-toggle";
        itemPhoneEncounter.appendChild(span2);

        var list1 = document.createElement('ul');
        var item1 = document.createElement('li');
        item1.className = "active";
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Phone Encounter") {
                var spaninner = document.createElement('p');
                spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[1] + "^" + array[i].split('^')[2];
                spaninner.className = "para";
                spaninner.innerHTML = array[i].split('^')[1];
                item1.appendChild(spaninner);
                list1.appendChild(item1);
            }
        }
        if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
        else { span2.classList.add("node-toggle"); }
        itemPhoneEncounter.appendChild(list1);
        list.appendChild(itemPhoneEncounter);
    }

    if (MenuName == 'Analytics') {
        var itemGrowthChart = document.createElement('li');
        var spanGrowthChart = document.createElement('span');
        spanGrowthChart.className = "leaf";
        spanGrowthChart.innerHTML = "Growth Chart";
        spanGrowthChart.id = "GrowthChart";
        spanGrowthChart.onclick = function () { testme() };

        itemGrowthChart.appendChild(spanGrowthChart);
        list.appendChild(itemGrowthChart);

        var itemFlowSheet = document.createElement('li');
        var spanFlowsheet = document.createElement('span');
        spanFlowsheet.className = "leaf";
        spanFlowsheet.innerHTML = "Flow Sheet";
        spanFlowsheet.id = "FlowSheet";
        spanFlowsheet.onclick = function () { FlowsheetCheck() };
        itemFlowSheet.appendChild(spanFlowsheet);
        list.appendChild(itemFlowSheet);

        var itemClinicalTrend = document.createElement('li');
        var spanClinicalTrend = document.createElement('span');
        spanClinicalTrend.className = "leaf";
        spanClinicalTrend.innerHTML = "Clinical Trend";
        spanClinicalTrend.id = "ClinicalTrend";
        spanClinicalTrend.onclick = function () { ClinicalTrendCheck() };
        itemClinicalTrend.appendChild(spanClinicalTrend);
        list.appendChild(itemClinicalTrend);

    }




    if (MenuName == 'Exam') {
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmExamPhotos.aspx";
        hidePatChart();

        //var itemUltraSound = document.createElement('li');
        //itemUltraSound.id = "liexamEncounter";
        //itemUltraSound.className = "node";

        //var spanUltraSound = document.createElement('span');
        //spanUltraSound.className = "leaf";
        //spanUltraSound.innerHTML = "Exam Photos / Images";//BugID:52897
        //spanUltraSound.id = "Exaphotos";


        //spanUltraSound.onclick = function () { Ultrasound() };
        //itemUltraSound.appendChild(spanUltraSound);

        //var span2 = document.createElement('span');
        //span2.className = "node-toggle";
        //itemUltraSound.appendChild(span2);


        //var list1 = document.createElement('ul');
        //var item1 = document.createElement('li');
        //item1.className = "active";
        //var resultsHeader = [];
        //var resultsDate = [];
        //var resultSubarray = [];
        //var resltHeaderarr = [];
        //var resltDatearr = [];
        //var header = [];

        //var mainResults = [];
        //var mainResultArray = [];
        //for (var i = 0; i < array.length; i++) {
        //    if (array[i].split('^')[0] == "Exam") {
        //        if (mainResults.indexOf(array[i].split('^')[2]) == -1) {
        //            mainResults.push(array[i].split('^')[2]);
        //            mainResultArray.push(array[i]);
        //        }
        //    }
        //}
        //mainResults.sort(function (a, b) {
        //    return Date.parse(a) - Date.parse(b);
        //});
        //mainResults.reverse();
        //for (var k = 0; k < mainResults.length; k++) {
        //    for (var i = 0; i < mainResultArray.length; i++) {
        //        if (mainResultArray[i].split('^')[0] == "Exam" && mainResultArray[i].split('^')[2] == mainResults[k]) {
        //            var spaninner = document.createElement('p');
        //            if (mainResultArray[i].split('^')[1] == '0') {
        //                spaninner.id = mainResultArray[i].split('^')[0] + "^" + mainResultArray[i].split('^')[1] + "^" + mainResultArray[i].split('^')[3] + "^" + mainResultArray[i].split('^')[5] + "^" + mainResultArray[i].split('^')[7];
        //                //spaninner.title = mainResultArray[i].split('^')[6];
        //            }
        //            else {
        //                spaninner.id = mainResultArray[i].split('^')[0] + "^" + mainResultArray[i].split('^')[1] + "^" + mainResultArray[i].split('^')[4] + "^" + mainResultArray[i].split('^')[5];
        //                //spaninner.title = mainResultArray[i].split('^')[5];
        //            }
        //            spaninner.className = "para";
        //            spaninner.innerHTML = mainResultArray[i].split('^')[2];
        //            item1.appendChild(spaninner);
        //            list1.appendChild(item1);
        //        }
        //    }
        //}
        //for (var i = 0; i < array.length; i++) {
        //    if (array[i].split('^')[0] == "Exam") {
        //        resultSubarray.push(array[i]);
        //        if (resultsHeader.indexOf(array[i].split('^')[3].split("_")[0]) == -1) {
        //            resultsHeader.push(array[i].split('^')[3].split("_")[0]);
        //            header.push(0);
        //        }
        //        if (resultsDate.indexOf(array[i].split('^')[2].split(' ')[0]) == -1) {
        //            resultsDate.push(array[i].split('^')[2].split(' ')[0]);
        //        }
        //    }
        //}


        //resultsDate.sort(function (a, b) {
        //    return Date.parse(a) - Date.parse(b);
        //});
        //resultsDate.reverse();


        //for (var k = 0; k < resultsDate.length; k++) {
        //    for (var l = 0; l < resultSubarray.length; l++) {
        //        if (resultSubarray[l].split('^')[2].split(' ')[0] == resultsDate[k] && resltDatearr.indexOf(resultSubarray[l]) == -1)
        //            resltDatearr.push(resultSubarray[l]);
        //    }
        //}
        //for (var k = 0; k < resultsHeader.length; k++) {
        //    for (var l = 0; l < resltDatearr.length; l++) {
        //        if (resltDatearr[l].split('^')[3].split('_')[0] == resultsHeader[k]) {
        //            if (header[k] == 0) {
        //                var list1 = document.createElement('ul');
        //                var item1 = document.createElement('li');
        //                item1.className = "active";
        //                header[k] = 1;

        //                var itemsub = document.createElement('li');
        //                itemsub.className = "node";
        //                var span1 = document.createElement('span');
        //                span1.className = "leaf";
        //                span1.innerHTML = resltDatearr[l].split('^')[3].split('_')[0];
        //                sCheck = span1.innerHTML;
        //                itemsub.appendChild(span1);

        //                var span2 = document.createElement('span');
        //                span2.className = "node-toggle";
        //                itemsub.appendChild(span2);
        //            }


        //            var list1sub = document.createElement('ul');
        //            var item1sub = document.createElement('li');
        //            var spaninner = document.createElement('p');
        //            spaninner.className = "para";
        //            if (resltDatearr[l].split('^').length == 9) {
        //                spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[6] + "^" + resltDatearr[l].split('^')[0] + "^" + resltDatearr[l].split('^')[7];
        //                //spaninner.title = resltDatearr[l].split('^')[4];
        //            }
        //            else {
        //                spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[2].split(' ')[0] + "^" + resltDatearr[l].split('^')[0];
        //                //if (resltDatearr[l].split('^').length == 9)
        //                //    spaninner.title = resltDatearr[l].split('^')[8];
        //            }
        //            spaninner.innerHTML = resltDatearr[l].split('^')[2].replace("07:30 AM", "00:00 AM");

        //            item1sub.appendChild(spaninner);
        //            list1sub.appendChild(item1sub);
        //            itemsub.appendChild(list1sub);
        //            list1.appendChild(itemsub);

        //            if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
        //            else { span2.classList.add("node-toggle"); }
        //            itemUltraSound.appendChild(list1);
        //        }
        //    }
        //}
        //if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
        //else { span2.classList.add("node-toggle"); }
        //itemUltraSound.appendChild(list1);
        //list.appendChild(itemUltraSound);

    }

    if (MenuName == 'Results' || MenuName == 'Document') {

        var itemResults = document.createElement('li');
        itemResults.id = "liResults";
        itemResults.className = "node";
        var span1 = document.createElement('span');
        span1.className = "leaf";
        if (MenuName == 'Results') {
            span1.innerHTML = "Results";
        }
        else if (MenuName == 'Document') {
            span1.innerHTML = 'Document';
        }
        itemResults.appendChild(span1);

        var span2 = document.createElement('span');
        span2.className = "node-toggle";
        itemResults.appendChild(span2);



        var list1 = document.createElement('ul');
        var item1 = document.createElement('li');
        item1.className = "active";

        var resultsHeader = [];
        var resultsDate = [];
        var resultSubarray = [];
        var resltHeaderarr = [];
        var resltDatearr = [];
        var header = [];

        var mainResults = [];
        var mainResultArray = [];
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Results") {
                if (mainResults.indexOf(array[i].split('^')[2]) == -1) {
                    mainResults.push(array[i].split('^')[2]);
                    mainResultArray.push(array[i]);
                }
            }
        }
        mainResults.sort(function (a, b) {
            return Date.parse(a) - Date.parse(b);
        });
        mainResults.reverse();
        for (var k = 0; k < mainResults.length; k++) {
            for (var i = 0; i < mainResultArray.length; i++) {
                if (mainResultArray[i].split('^')[0] == "Results" && mainResultArray[i].split('^')[2] == mainResults[k]) {
                    var spaninner = document.createElement('p');
                    if (mainResultArray[i].split('^')[1] == '0') {
                        spaninner.id = mainResultArray[i].split('^')[0] + "^" + mainResultArray[i].split('^')[1] + "^" + mainResultArray[i].split('^')[3] + "^" + mainResultArray[i].split('^')[5] + "^" + mainResultArray[i].split('^')[7];
                        //spaninner.title = mainResultArray[i].split('^')[6];
                    }
                    else {
                        spaninner.id = mainResultArray[i].split('^')[0] + "^" + mainResultArray[i].split('^')[1] + "^" + mainResultArray[i].split('^')[4] + "^" + mainResultArray[i].split('^')[5];
                        //spaninner.title = mainResultArray[i].split('^')[5];
                    }
                    spaninner.className = "para";
                    spaninner.innerHTML = mainResultArray[i].split('^')[2];
                    item1.appendChild(spaninner);
                    list1.appendChild(item1);
                }
            }
        }
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "ResultsSub") {
                resultSubarray.push(array[i]);
                if (resultsHeader.indexOf(array[i].split('^')[3]) == -1) {
                    resultsHeader.push(array[i].split('^')[3]);
                    header.push(0);
                }
                if (resultsDate.indexOf(array[i].split('^')[2].split(' ')[0]) == -1) {
                    resultsDate.push(array[i].split('^')[2].split(' ')[0]);
                }
            }
            //Jira Cap-468 - Echo and Vascular test templates required Signed
           // if (array[i].split('^')[3] == "Stress Test" || array[i].split('^')[3] == "Nuclear Medicine") {
                for (var z = 0; z < array.length; z++) {
                    if (array[z].split('^')[0] == "StressTestReport" && array[z].split('^')[3] == array[i].split('^')[3]+" Signed Report") {
                        resultSubarray.push(array[z]);
                        if (resultsHeader.indexOf(array[z].split('^')[3]) == -1) {
                            resultsHeader.push(array[z].split('^')[3]);
                            header.push(0);
                        }
                        if (resultsDate.indexOf(array[z].split('^')[2].split(' ')[0]) == -1) {
                            resultsDate.push(array[z].split('^')[2].split(' ')[0]);
                        }
                    }
                //}
            }
        }

        //for (var i = 0; i < array.length; i++) {
        //    if (array[i].split('^')[0] == "StressTestReport") {
        //        resultSubarray.push(array[i]);
        //        if (resultsHeader.indexOf(array[i].split('^')[3]) == -1) {
        //            resultsHeader.push(array[i].split('^')[3]);
        //            header.push(0);
        //        }
        //        if (resultsDate.indexOf(array[i].split('^')[2].split(' ')[0]) == -1) {
        //            resultsDate.push(array[i].split('^')[2].split(' ')[0]);
        //        }
        //    }
        //}

        resultsDate.sort(function (a, b) {
            return Date.parse(a) - Date.parse(b);
        });
        resultsDate.reverse();


        for (var k = 0; k < resultsDate.length; k++) {
            for (var l = 0; l < resultSubarray.length; l++) {
                if (resultSubarray[l].split('^')[2].split(' ')[0] == resultsDate[k] && resltDatearr.indexOf(resultSubarray[l]) == -1)
                    resltDatearr.push(resultSubarray[l]);
            }
        }
        for (var k = 0; k < resultsHeader.length; k++) {
            for (var l = 0; l < resltDatearr.length; l++) {
                if (resltDatearr[l].split('^')[3] == resultsHeader[k]) {
                    if (header[k] == 0) {
                        var list1 = document.createElement('ul');
                        var item1 = document.createElement('li');
                        item1.className = "active";
                        header[k] = 1;

                        var itemsub = document.createElement('li');
                        itemsub.className = "node";
                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = resltDatearr[l].split('^')[3].split('_')[0];
                        sCheck = span1.innerHTML;
                        itemsub.appendChild(span1);

                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        itemsub.appendChild(span2);
                    }


                    var list1sub = document.createElement('ul');
                    var item1sub = document.createElement('li');
                    var spaninner = document.createElement('p');
                    spaninner.className = "para";
                    if (resltDatearr[l].split('^').length == 9) {
                        spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[6] + "^" + resltDatearr[l].split('^')[0] + "^" + resltDatearr[l].split('^')[7];
                        //spaninner.title = resltDatearr[l].split('^')[4];
                    }
                    else if (resltDatearr[l].split('^').length == 10) {
                        spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[6] + "^" + resltDatearr[l].split('^')[0] + "^" + resltDatearr[l].split('^')[7] + "^" + resltDatearr[l].split('^')[9];
                        //spaninner.title = resltDatearr[l].split('^')[4];
                    }
                    else {
                        spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[6] + "^" + resltDatearr[l].split('^')[0];
                        //if (resltDatearr[l].split('^').length == 9)
                        //    spaninner.title = resltDatearr[l].split('^')[8];
                    }
                    // To show the time as 00:00:00 in the left side patient chart when time is either (Nov-Mar) 7:30 AM or (Apr-Oct) 8:30 AM  for daylight savings
                    spaninner.innerHTML = resltDatearr[l].split('^')[2].replace("07:30 AM", "00:00 AM").replace("08:30 AM", "00:00 AM");

                    item1sub.appendChild(spaninner);
                    list1sub.appendChild(item1sub);
                    itemsub.appendChild(list1sub);
                    list1.appendChild(itemsub);

                    if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
                    else { span2.classList.add("node-toggle"); }
                    itemResults.appendChild(list1);
                }
            }
        }

        if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
        else { span2.classList.add("node-toggle"); }
        if (MenuName == 'Results' || MenuName == 'Document') {
            itemResults.appendChild(list1);
            list.appendChild(itemResults);
        }
    }

    if (MenuName == 'Patient Task') {

        var item12 = document.createElement('li');
        item12.className = "node";
        item12.id = "liPatientTask";
        var span1 = document.createElement('span');
        span1.className = "leaf";
        span1.innerHTML = "Patient Task";
        item12.appendChild(span1);



        var span2 = document.createElement('span');
        span2.className = "node-toggle";
        item12.appendChild(span2);

        var list1 = document.createElement('ul');
        var item1 = document.createElement('li');
        item1.className = "active";

        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Patient Task") {
                var spaninner = document.createElement('p');
                spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[2];
                spaninner.className = "para";
                spaninner.innerHTML = array[i].split('^')[1];
                item1.appendChild(spaninner);
                list1.appendChild(item1);
            }
        }
        if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
        else { span2.classList.add("node-toggle"); }
        item12.appendChild(list1);
        list.appendChild(item12);
    }









    //}

    if (MenuName == 'Document') {

        var headers = [];
        var k = [];
        var datelst = [];
        var arrayOthers = [];
        var arrayOthersNew = [];
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Others") {
                if (headers.indexOf(array[i].split('^')[5]) == -1) {
                    headers.push(array[i].split('^')[5]);
                    k.push(0);
                }

            }
        }
        for (var i = 0; i < headers.length; i++) {
            for (var j = 0; j < array.length; j++) {
                if (array[j].split('^')[0] == "Others") {
                    if (array[j].split('^')[5] == headers[i]) {
                        arrayOthers.push(array[j]);
                    }
                }
            }
        }
        for (var t = 0; t < arrayOthers.length; t++) {
            if (datelst.indexOf(arrayOthers[t].split('^')[2].split(' ')[0]) == -1) {
                datelst.push(arrayOthers[t].split('^')[2].split(' ')[0]);
            }
        }
        datelst.sort(function (a, b) {
            return Date.parse(a) - Date.parse(b);
        });

        for (var t = 0; t < datelst.length; t++) {
            for (var i = 0; i < arrayOthers.length; i++) {
                if (datelst[t] == arrayOthers[i].split('^')[2].split(' ')[0]) {
                    arrayOthersNew.push(arrayOthers[i]);
                }
            }
        }

        for (var j = 0; j < headers.length; j++) {
            var date = [];
            for (var i = 0; i < arrayOthersNew.length; i++) {
                if (headers[j] == arrayOthersNew[i].split('^')[5]) {
                    if (k[j] == 0) {
                        var iteminner = document.createElement('li');
                        iteminner.className = "node";
                        iteminner.id = headers[j].replace(" ", "").replace(" ", "").replace(" ", "");

                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = headers[j];

                        iteminner.appendChild(span1);


                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        iteminner.appendChild(span2);

                        var list1 = document.createElement('ul');
                        var item1 = document.createElement('li');
                        item1.className = "active";
                        k[j] = 1;
                    }

                    if (date.indexOf(arrayOthersNew[i].split('^')[2].split(' ')[0]) == -1) {
                        var itemsub = document.createElement('li');
                        itemsub.className = "node";
                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = arrayOthersNew[i].split('^')[2].split(' ')[0];
                        sCheck = span1.innerHTML;
                        itemsub.appendChild(span1);

                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        itemsub.appendChild(span2);
                        date.push(arrayOthersNew[i].split('^')[2].split(' ')[0]);
                    }



                    var list1sub = document.createElement('ul');
                    var item1sub = document.createElement('li');
                    var spaninner = document.createElement('p');
                    spaninner.className = "para";
                    spaninner.id = headers[j] + "^" + arrayOthersNew[i].split('^')[1] + "^" + arrayOthersNew[i].split('^')[6] + "^" + arrayOthersNew[i].split('^')[0] + "^" + arrayOthersNew[i].split('^')[7];
                    spaninner.innerHTML = arrayOthersNew[i].split('^')[3];

                    item1sub.appendChild(spaninner);
                    list1sub.appendChild(item1sub);
                    itemsub.appendChild(list1sub);

                    list1.appendChild(itemsub);

                    if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
                    else { span2.classList.add("node-toggle"); }
                    iteminner.appendChild(list1);

                    list.appendChild(iteminner);
                }

            }

        }
    }

    if (MenuName == 'Summary of Care') {
        var item13 = document.createElement('li');
        item13.className = "node";

        var span1 = document.createElement('span');
        span1.className = "leaf";
        span1.innerHTML = "Summary Of Care";
        item13.appendChild(span1);

        var span2 = document.createElement('span');
        span2.className = "node-toggle";
        item13.appendChild(span2);

        var list1 = document.createElement('ul');
        var item1 = document.createElement('li');
        item1.className = "active";

        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Summary of Care") {
                var spaninner = document.createElement('p');
                spaninner.className = "para";
                spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[2].split(';')[1];
                //spaninner.title = array[i].split('^')[2].split(';')[1];
                spaninner.innerHTML = array[i].split('^')[2].split(';')[0];
                item1.appendChild(spaninner);
                list1.appendChild(item1);
            }
        }
        if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
        else { span2.classList.add("node-toggle"); }
        item13.appendChild(list1);
        list.appendChild(item13);
    }
    //}
    return list;
}

/*BugID:45808 - Changes:
                    * View Result openened inside Patient Chart, not as a modal window
                    * Through Open PatientChart of View Result screen, now the entire Patient Summary will be shown (earlier only Encounters are shown)*/
var IsDisaster = null;

function tree_add_leaf_example_click(leaf, node, pnode, tree) {

    document.getElementById(GetClientId("hdnQencounterId")).value = leaf[0].id.split('^')[1];
    $("#ctl00_C5POBody_dvCheck li .colored").removeClass("colored");
    $(leaf[0]).addClass("colored");
    if (leaf[0].id.split('^')[0] == "Encounters") {
        var PrevTab = $($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active");
        if (PrevTab.length != 0) {
            var prvTab = PrevTab[0].attributes.id.value;

            if (prvTab.match("tbEPrescription") != null) {

                //Jira CAP-1366
                StartRcopiaStrip();

                $.ajax({
                    type: "POST",
                    url: "frmEncounter.aspx/DownloadRcoipa",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        //Jira CAP-1567
                        document.cookie = "CeRxFlag=false";
                        document.cookie = "CeRxHumanID=";
                        //Jira CAP-1366
                        StopRcopiaStrip();
                        RcopiaErrorAlert(data.d);
                    },
                    error: function OnError(xhr) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        //Jira CAP-1366
                        StopRcopiaStrip();
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
        var now = new Date();
        var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
        document.getElementById(GetClientId("hdnLocalTime")).value = utc;
        //CAP-778 Cannot read properties of null
        //var sStrinh = (window.frames["ctl00_C5POBody_EncounterContainer"]?.contentDocument?.getElementById('pnlBarGroupTabs') != null) ? window.frames["ctl00_C5POBody_EncounterContainer"]?.contentDocument?.getElementById('pnlBarGroupTabs')?.innerHTML?.split('|')[1]?.trim() : "";
        //CAP-1321
        if (leaf[0].id.split('^')[1] == leaf[0].currentId && (leaf[0].From != "Menu" || window.top.location.href.indexOf('ScreenMode=Menu') == -1)) {
            //CAP-1511
            var encounterURL = "frmEncounter.aspx?Date=" + document.getElementById(GetClientId("hdnLocalTime")).value + "&EncounterID=" + leaf[0].id.split('^')[1] + "&leftpane=Y";
            var currentTabNameId = document.getElementById(GetClientId('hdnScreen'))?.value;
            var currentSubTabNameId = document.getElementById(GetClientId('hdnSubScreen'))?.value;
            if (currentTabNameId != undefined && currentTabNameId != null && currentTabNameId != "") {
                encounterURL += "&Screen=" + currentTabNameId;
            }
            if (currentSubTabNameId != undefined && currentSubTabNameId != null && currentSubTabNameId != "") {
                encounterURL += "&SubScreen=" + currentSubTabNameId;
            }

            $('#ctl00_C5POBody_EncounterContainer')[0].src = encounterURL;
            sessionStorage.setItem("EncId_PatSummaryBar", leaf[0].id.split('^')[1]);
            sessionStorage.setItem("Enc_DOS", leaf[0].innerText.split(' - ')[0]);
            sessionStorage.setItem("EncId_PatSummaryBar_XMl_Regenerate", leaf[0].id.split('^')[1]);
            sessionStorage.setItem("Enc_DOS_XMl_Regenerate", leaf[0].innerText.split(' - ')[0]);
            if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1)
                reloadSummary();
        }
        else {
            //inserLog(leaf[0].id.split('^')[1], "0", "Opening Encounter from Leftside PatientChart");
            //Jira CAP-1379 - start
            //CAP-1463
            if (document?.getElementById('WaitingMessage') != undefined && document?.getElementById('WaitingMessage') != null) {
                document.getElementById('WaitingMessage').style.display = 'block';
            }
            if (document?.getElementById('jqxSplitter') != undefined && document?.getElementById('jqxSplitter') != null) {
                document.getElementById('jqxSplitter').style.height = '80px';
            }
            if (document?.getElementById('ctl00_C5POBody_EncounterContainer') != undefined && document?.getElementById('ctl00_C5POBody_EncounterContainer') != null) {
                document.getElementById('ctl00_C5POBody_EncounterContainer').style.display = 'none';
            }
           
            //Jira CAP-1379 -end
            $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + leaf[0].id.split('^')[1]+"&TabMode=true";
            document.getElementById(GetClientId("hdnEncounterId")).value = leaf[0].id.split('^')[1];
            sessionStorage.setItem("EncId_PatSummaryBar", leaf[0].id.split('^')[1]);
            sessionStorage.setItem("Enc_DOS", leaf[0].innerText.split(' - ')[0]);
            sessionStorage.setItem("EncId_PatSummaryBar_XMl_Regenerate", leaf[0].id.split('^')[1]);
            sessionStorage.setItem("Enc_DOS_XMl_Regenerate", leaf[0].innerText.split(' - ')[0]);
            sessionStorage.setItem("Enc_Tree", "true");
            sessionStorage.setItem("Enc_SelectDOS", leaf[0].innerText.split(' - ')[0]);
            localStorage.setItem("SummaryTab", "false");
            if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1)
                reloadSummary();
        }

        $(leaf[0]).addClass("colored");
        if (backcolor != "") {
            backcolor.style.backgroundColor = "";
        }
        backcolor = leaf[0];
        if (window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs') != null) {
            window.frames["ctl00_C5POBody_EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs').innerText = "";
        }
    }
    else if (leaf[0].id.split('^')[0] == "Patient Task") {
        var sScreenName = "PatientChart";
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmPatientCommunication.aspx?parentscreen=" + sScreenName + "&MessageID=" + leaf[0].id.split('^')[1];
    }
    else if (leaf[0].id.split('^')[0] == "Summary of Care") {
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmPrintPDF.aspx?ParentForm=PatientChart&FilePath=" + leaf[0].id.split('^')[1];
    }
    else if (leaf[0].id.split('^')[3] == "Others" || leaf[0].id.split('^')[0] == "EncountersSub") {

        if (leaf[0].id.split('^')[0] == "Disaster Recovery Encounters") {
            var url = "frmSummaryNew.aspx?DisasterRecovery=True&EncounterId=" + leaf[0].id.split('^')[1];
            OpenDRsummary(url);
            IsDisaster = "true";

        }
        else {
            var QueryString = "?HumanID=" + leaf[0].id.split('^')[2] + "&Key_id=" + leaf[0].id.split('^')[1] + "&Opening_from=" + "Patient_Pane";
            if (leaf[0].id.split('^')[0] == "EncountersSub")
                QueryString += "&Doc_type=" + $(node.parents()).find("li[id=liEncounters]")[0].childNodes[0].innerHTML + "&ResultMasterID=" + leaf[0].id.split('^')[3];
            else
                QueryString += "&Doc_type=" + $(node.offsetParent().offsetParent()).find(".leaf")[0].innerHTML + "&ResultMasterID=" + leaf[0].id.split('^')[4];
            $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmViewResult.aspx" + QueryString;
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
    }
    else if (leaf[0].id.split('^')[3] == "ResultsSub") {


        var QueryString = "?HumanID=" + leaf[0].id.split('^')[2] + "&Key_id=" + leaf[0].id.split('^')[1] + "&Doc_type=" + $(node.offsetParent().offsetParent()).find(".leaf")[0].innerHTML + "&Opening_from=" + "Patient_Pane" + "&Result_OBR_Date=" + leaf[0].innerText.split(' ')[0];
        if (leaf[0].id.split('^').length == 5)
            QueryString += "&OrderSubmitId=" + leaf[0].id.split('^')[4];
        else if (leaf[0].id.split('^').length == 6)
            QueryString += "&OrderSubmitId=" + leaf[0].id.split('^')[5];
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmViewResult.aspx" + QueryString;
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }
    else if (leaf[0].id.split('^')[0] == "Phone Encounter") {
        var sScreenName = "PatientChart";
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?PhoneEncounter=True&EncounterId=" + leaf[0].id.split('^')[2] + "&TabMode=true";
        localStorage.setItem("SummaryTab", "false");
    }
    else if (leaf[0].id.split('^')[3] == "Exam") {
        $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmExamPhotos.aspx?frompatientChart=yes&DocDate=" + leaf[0].id.split('^')[2] + "&DocType=" + leaf[0].id.split('^')[0] + "&FileMngID=" + leaf[0].id.split('^')[1];

    }
    //else if (leaf[0].id.split('^')[0] == "Stress Test Signed Report") {
    else if (leaf[0].id.split('^')[0].indexOf("Signed Report") >0){
         var IntNotes = "";
        var WSData = JSON.stringify({
            sResultMasterId: leaf[0].id.split('^')[2]
        });

        $.ajax({
            type: "POST",
            url: "frmResultInterpretation.aspx/PrintIntrepretationNotesData",
            contentType: "application/json;charset=utf-8",
            data: WSData,
            dataType: "json",
            async: true,
            success: function (data) {
                var result = $.parseJSON(data.d);
                //Cap - 878
                //$('#ctl00_C5POBody_EncounterContainer')[0].src = "frmPrintPDF.aspx?InterpretationNotes=PatientChart&IntNotes=" + JSON.stringify(result.ResultReviewComments.replaceAll("<br/>", "") + "&PhySigDate=" + JSON.stringify(result.SignedDate) + "&PhySigName=" + JSON.stringify(result.SignedPhyName) + "&FacAddress=" + encodeURIComponent(result.FacilityAddress);
                //Cap - 1054               
                //$('#ctl00_C5POBody_EncounterContainer')[0].src = "frmPrintPDF.aspx?InterpretationNotes=PatientChart&IntNotes=" + JSON.stringify(result.ResultReviewComments.replaceAll("<br/>", "").replaceAll("&", "$|$|$|$|").replaceAll("#", "!^!^!^!^").replaceAll("+", "~|~|~|~|")) + "&PhySigDate=" + JSON.stringify(result.SignedDate) + "&PhySigName=" + JSON.stringify(result.SignedPhyName) + "&FacAddress=" + encodeURIComponent(result.FacilityAddress);
                $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmPrintPDF.aspx?InterpretationNotes=PatientChart&IntNotes="+IntNotes+"&ResultMasterId=" + leaf[0].id.split('^')[2] + "&PhySigDate=" + JSON.stringify(result.SignedDate) + "&PhySigName=" + JSON.stringify(result.SignedPhyName) + "&FacAddress=" + encodeURIComponent(result.FacilityAddress);
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
    $(leaf[0]).addClass("colored");
    hidePatChart();
    if (IsDisaster == "true") {
        IsDisaster = null;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}


function testme() {
    hidePatChart();
    $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmGrowthChart.aspx";
}

function FlowsheetCheck() {
    hidePatChart();
    $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmFlowSheet.aspx";

}

function ClinicalTrendCheck() {
    hidePatChart();
    $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmClinicalTrend.aspx";

}

function Ultrasound() {
    hidePatChart();
    $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmExamPhotos.aspx?Category=UPLOAD & VIEW PHOTOS";
}

function hidePatChart() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#ctl00_C5POBody_dvTest")[0].style.display == "block") {
        $("#ctl00_C5POBody_dvTest").css("display", "none");
        $("#divPatChartContainer").css("display", "none");
        $("#ctl00_C5POBody_dvTest").css("display", "none");
        $("#divPatChartContainer").css("display", "none");
        $("#Encountersimg")[0].style.color = "#3275B1";
        $("#EncountersText")[0].style.color = "#000000";
        $("#Resultsimg")[0].style.color = "#3275B1";
        $("#ResultsText")[0].style.color = "#000000";
        $("#PatientTaskimg")[0].style.color = "#3275B1";
        $("#PatientTaskText")[0].style.color = "#000000";
        $("#Examimg")[0].style.color = "#3275B1";
        $("#ExamText")[0].style.color = "#000000";
        $("#Documentimg")[0].style.color = "#3275B1";
        $("#DocumentText")[0].style.color = "#000000";
        $("#SummaryofCareimg")[0].style.color = "#3275B1";
        $("#SummaryofCareText")[0].style.color = "#000000";
        $("#Analyticsimg")[0].style.color = "#3275B1";
        $("#AnalyticsText")[0].style.color = "#000000";
    }
}

function getInsuranceDetails() {
    var obj = new Array();
    var Humanid = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|')[4].split(':')[1].trim();
    obj.push("EncounterID=0");
    obj.push("humanID=" + Humanid);
    obj.push("EncStatus=''");
    obj.push("bShowPat=true");
    obj.push("sScreenMode=EVSUMMARY");
    openModal("frmQuickpatientcreate.aspx", 730, 1020, obj, "ctl00_ModalWindow");


}


function openRadWindow(fromname, height, width, inputargument, RadWindowName) {
    var Argument = "";
    var PageName = fromname;
    if (inputargument != undefined) {
        for (var i = 0; i < inputargument.length; i++) {
            if (i != 0) {
                Argument = Argument + "&" + inputargument[i];
            } else {
                Argument = inputargument[i];
            }
        }
        if (inputargument.lenght != 0 && inputargument.lenght != undefined) {
            PageName = PageName + "?";
        }
    }
    var result = radopen(PageName + Argument, RadWindowName);
    result.SetModal(true);
    result.set_visibleStatusbar(false);
    result.setSize(width, height);
    result.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
    result.set_iconUrl("Resources/16_16.ico");
    result.set_keepInScreenBounds(true);
    result.set_centerIfModal(true);
    result.center();

}
function LoadRAFfromXML() {
    var RAF_Score = "RAF Score :";
    var regex = /<BR\s*[\/]?>/gi;

    var HUMAN_ID = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|')[4].split(':')[1].trim();
    var ProjectName = top.window.document.getElementById("ctl00_hdnProjectName").value;
    var WSData = JSON.stringify({

        human_id: HUMAN_ID

    });
    $.ajax({
        type: "POST",
        url: "frmRcopiaToolbar.aspx/GetRAFScore",
        data: WSData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var jsonData = $.parseJSON(data.d);
            if (jsonData.length != 0) {

                if (jsonData.RAF != null && jsonData.RAF != undefined && jsonData.RAF != "" && jsonData.RAF != "|") {
                    var Raf = jsonData.RAF.split("|")
                    for (var i = 0; i < Raf.length; i++) {
                        if (Raf[i] != "" && Raf[i].indexOf('HPN') <= -1)
                            if (Raf[i].split(":")[1] != undefined && Raf[i].split(":")[1].trim() == "") {
                                RAF_Score += Raf[i].split(":")[0] + " : " + "NA" + "<br/>";
                            }
                            else
                                RAF_Score += Raf[i] + "<br/>";
                    }
                    if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblRAF") != null) {
                        top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = RAF_Score.replace("RAF Score :", "").replace("/n", "");
                        if (RAF_Score.length != 0)
                            top.window.document.getElementById("RAF_tooltp").innerText = RAF_Score.replace(regex, "\n") + "\n";
                        else
                            top.window.document.getElementById("RAF_tooltp").innerText = "";
                    }
                }
                else {
                    if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblRAF") != null) {
                        top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = (new Date()).getFullYear().toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 1).toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 2).toString() + ": " + "NA" + "<br/>" + "HPN " + ((new Date()).getFullYear()).toString() + ": " + "NA"; //+ "<br/>" + "HPN " + ((new Date()).getFullYear() - 1).toString() + ": " + "NA" + "<br/>" + "HPN " + ((new Date()).getFullYear() - 1).toString() + ": " + "NA";
                        top.window.document.getElementById("RAF_tooltp").innerText = ((new Date()).getFullYear().toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 1).toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 2).toString() + ": " + "NA").replace(regex, "\n");
                    }
                }
            }
            else {
                if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblRAF") != null) {
                    top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = (new Date()).getFullYear().toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 1).toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 2).toString() + ": " + "NA";// + "<br/>" + "HPN " + ((new Date()).getFullYear()).toString() + ": " + "NA" + "<br/>" + "HPN " + ((new Date()).getFullYear() - 1).toString() + ": " + "NA" + "<br/>" + "HPN " + ((new Date()).getFullYear() - 1).toString() + ": " + "NA";
                    top.window.document.getElementById("RAF_tooltp").innerText = ((new Date()).getFullYear().toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 1).toString() + ": " + "NA" + "<br/>" + ((new Date()).getFullYear() - 2).toString() + ": " + "NA").replace(regex, "\n");
                }
            }

        },
        error: function OnError(data, textStatus, jqXHR) {
            console.log('error!', data, textStatus, jqXHR);
        }
    });
}
function test() {
    $.ajax({
        type: "POST",
        url: "frmAssessmentNew.aspx/GetEncounterDetailsforRaf",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Human_id: HUMAN_ID }),
        dataType: "json",
        async: true,
        success: function (data) {
            var Getdata = data.d.split('~');
            var HUMAN_ID = '';
            var DOB = '';
            var Gender = '';
            var icdlist = '';
            var sIsMedicaid = '';
            var sIsDisabled = '';
            var sIsCommunity = '';
            var sOriginallyDisabled = '';
            var sEnrollmentStatus = '';
            var PrimaryCarrier = '';
            if (Getdata.length > 0) {
                icdlist = Getdata[0];
                sIsMedicaid = Getdata[1].split(':')[1];
                sOriginallyDisabled = Getdata[2].split(':')[1];
                sIsCommunity = Getdata[3].split(':')[1];
                sIsDisabled = Getdata[4].split(':')[1];
                sEnrollmentStatus = Getdata[5].split(':')[1];
                PrimaryCarrier = Getdata[6].split(':')[1];
            }

            if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined) {
                var sHumanDetails = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
                HUMAN_ID = sHumanDetails.split('|')[4].split(':')[1].trim();
                if (sHumanDetails.split('|')[3].trim() == 'F')
                    Gender = 'FEMALE';
                else if (sHumanDetails.split('|')[3].trim() == 'M')
                    Gender = 'MALE'
                DOB = sHumanDetails.split('|')[1];
            }

            var now = new Date();
            var Current = now.getUTCFullYear() + "|Y";
            var Previous = now.getUTCFullYear() - 1 + "|N";
            var year = Current + '~' + Previous;
            var icdlist = icdlist;
            var is_store_Value = 'Y';
            var ProjectName = '';
            var surl = '';

            if (top.window.document.getElementById("ctl00_hdnProjectName").value != '') {
                ProjectName = top.window.document.getElementById("ctl00_hdnProjectName").value;
            }
            if (top.window.document.getElementById("ctl00_hdnProjectIPAddress").value != '')
                surl = top.window.document.getElementById("ctl00_hdnProjectIPAddress").value;
            if (rafcalc != undefined && rafcalc != "") {
                HUMAN_ID = rafcalc.split('^')[0];
                DOB = rafcalc.split('^')[1];
                Gender = rafcalc.split('^')[2];
            }

            var WSData = JSON.stringify({
                ProjectName: ProjectName,
                human_id: HUMAN_ID,
                Gender: Gender,
                DOB: DOB,
                year: year,
                icdlist: icdlist,
                is_store_Value: is_store_Value,
                sIsMedicaid: sIsMedicaid,
                sIsDisabled: sIsDisabled,
                sIsCommunity: sIsCommunity,
                sOriginallyDisabled: sOriginallyDisabled,
                sEnrollmentStatus: sEnrollmentStatus,
                PrimaryCarrier: PrimaryCarrier
            });
            $.ajax({
                type: "POST",
                url: surl,
                data: WSData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    var jsonData = $.parseJSON(data.d);
                    if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined) {
                        if (jsonData.length != 0) {
                            for (var i = 0; i < jsonData.length; i++)
                                if (jsonData[i] != "")
                                    RAF_Score += jsonData[i] + "<br/>";

                            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = RAF_Score;
                            if (RAF_Score.length != 0)
                                top.window.document.getElementById("RAF_tooltp").innerText = RAF_Score.replace(regex, "\n") + "\n";
                            else
                                top.window.document.getElementById("RAF_tooltp").innerText = "";
                        }
                        else {
                            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                            top.window.document.getElementById("RAF_tooltp").innerText = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                        }
                    }
                },
                error: function OnError(data, textStatus, jqXHR) {
                    console.log('error!', data, textStatus, jqXHR);
                }
            });
        },
        error: function OnError(xhr) {
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
function loadRAF(sHumanDetails, Is_store) {
    var regex = /<BR\s*[\/]?>/gi;
    var RAF_Score = "RAF Score :";

    var HUMAN_ID = sHumanDetails.split('|')[4].split(':')[1].trim();
    var DOS = "";
    $.ajax({
        type: "POST",
        url: "frmAssessmentNew.aspx/GetEncounterDetailsforRaf",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ Human_id: HUMAN_ID }),
        dataType: "json",
        async: true,
        success: function (data) {
            var Getdata = data.d.split('~');
            var HUMAN_ID = '';
            var DOB = '';
            var Gender = '';
            var icdlist = '';
            var sIsMedicaid = '';
            var sIsDisabled = '';
            var sIsCommunity = '';
            var sOriginallyDisabled = '';
            var sEnrollmentStatus = '';
            var PrimaryCarrier = '';
            var DOS = '';
            if (Getdata.length > 0) {
                icdlist = Getdata[0];
                sIsMedicaid = Getdata[1].split(':')[1];
                sOriginallyDisabled = Getdata[2].split(':')[1];
                sIsCommunity = Getdata[3].split(':')[1];
                sIsDisabled = Getdata[4].split(':')[1];
                sEnrollmentStatus = Getdata[5].split(':')[1];
                PrimaryCarrier = Getdata[6].split(':')[1];
                DOS = Getdata[7].split(':')[1].replace('"', '');
            }
            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = "Calculating....."
            top.window.document.getElementById("RAF_tooltp").innerText = "Calculating.....";
            if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined) {
                var sHumanDetails = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
                HUMAN_ID = sHumanDetails.split('|')[4].split(':')[1].trim();
                if (sHumanDetails.split('|')[3].trim() == 'F')
                    Gender = 'FEMALE';
                else if (sHumanDetails.split('|')[3].trim() == 'M')
                    Gender = 'MALE'
                DOB = sHumanDetails.split('|')[1];
            }

            var Current = "", Previous = "", SPrevious = ""

            if (DOS == "0001") {
                DOS = "2018";
            }
            var now = new Date();
            if (DOS == "") {
                Current = now.getUTCFullYear() + "|Y";
                Previous = now.getUTCFullYear() - 1 + "|Y";
                SPrevious = now.getUTCFullYear() - 2 + "|Y";
            }
            else {
                if (now.getUTCFullYear().toString() == DOS) {
                    Current = now.getUTCFullYear() + "|Y";
                    Previous = now.getUTCFullYear() - 1 + "|N";
                    SPrevious = now.getUTCFullYear() - 2 + "|N";
                }
                else if ((now.getUTCFullYear() - 1).toString() == DOS) {
                    Current = now.getUTCFullYear() + "|N";
                    Previous = now.getUTCFullYear() - 1 + "|Y";
                    SPrevious = now.getUTCFullYear() - 2 + "|N";
                }
                else {
                    Current = now.getUTCFullYear() + "|N";
                    Previous = now.getUTCFullYear() - 1 + "|N";
                    SPrevious = now.getUTCFullYear() - 2 + "|Y";

                }
            }

            var year = Current + '~' + Previous + "~" + SPrevious;
            var icdlist = icdlist;
            var is_store_Value = 'Y';
            var ProjectName = '';
            var surl = '';

            if (top.window.document.getElementById("ctl00_hdnProjectName").value != '') {
                ProjectName = top.window.document.getElementById("ctl00_hdnProjectName").value;
            }
            if (top.window.document.getElementById("ctl00_hdnProjectIPAddress").value != '')
                surl = top.window.document.getElementById("ctl00_hdnProjectIPAddress").value;
            if (rafcalc != undefined && rafcalc != "") {
                HUMAN_ID = rafcalc.split('^')[0];
                DOB = rafcalc.split('^')[1];
                Gender = rafcalc.split('^')[2];
            }

            var WSData = JSON.stringify({
                ProjectName: ProjectName,
                human_id: HUMAN_ID,
                Gender: Gender,
                DOB: DOB,
                year: year,
                icdlist: icdlist,
                is_store_Value: is_store_Value,
                sIsMedicaid: sIsMedicaid,
                sIsDisabled: sIsDisabled,
                sIsCommunity: sIsCommunity,
                sOriginallyDisabled: sOriginallyDisabled,
                sEnrollmentStatus: sEnrollmentStatus,
                PrimaryCarrier: PrimaryCarrier
            });
            $.get(surl + '/RafCalculator?ProjectName=' + ProjectName + '&human_id=' + HUMAN_ID + '&Gender=' + Gender + '&DOB=' + DOB + '&year=' + year + '&icdlist=' + icdlist + '&is_store_Value=' + is_store_Value + '&sIsMedicaid=' + sIsMedicaid + '&sIsDisabled=' + sIsDisabled + '&sIsCommunity=' + sIsCommunity + '&sOriginallyDisabled=' + sOriginallyDisabled + '&sEnrollmentStatus=' + sEnrollmentStatus + '&PrimaryCarrier=' + PrimaryCarrier, null, function (data) {
                console.log(data);
                var jsonData = $.parseJSON(data);
                if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined) {
                    if (jsonData.length != 0) {
                        for (var i = 0; i < jsonData.length; i++) {
                            if (jsonData[i] != "" && jsonData[i].indexOf('HPN') <= -1)
                                if (jsonData[i].split(":")[1] != undefined && jsonData[i].split(":")[1].trim() == "") {

                                    RAF_Score += jsonData[i].split(":")[0] + " : " + "NA" + "<br/>";
                                }

                                else
                                    RAF_Score += jsonData[i] + "<br/>";
                        }
                        top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = RAF_Score.replace("RAF Score :", "").replace("/n", "");
                        if (RAF_Score.length != 0)
                            top.window.document.getElementById("RAF_tooltp").innerText = RAF_Score.replace(regex, "\n") + "\n";
                        else
                            top.window.document.getElementById("RAF_tooltp").innerText = "";
                    }
                    else {
                        top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                        top.window.document.getElementById("RAF_tooltp").innerText = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                    }
                }
            });


        },
        error: function OnError(xhr) {
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
function RAFRefreshCLick() {
    loadRAF(window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText, "N");
}
function OnSuccessSummaryBar(response) {
    var regex = /<BR\s*[\/]?>/gi;
    if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText == undefined || window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText == "") {
        return;
    }
    LoadRAFfromXML();
    if (response != null) {
        if (top.window.document.getElementById("ctl00_C5POBody_lblAllergies") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblAllergies") != null)
            top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = response.d[0];
        if (top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints") != null)

            top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = response.d[1];
        if (top.window.document.getElementById("ctl00_C5POBody_lblProblemList") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblProblemList") != null)

            top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = response.d[2];
        if (top.window.document.getElementById("ctl00_C5POBody_lblVitals") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblVitals") != null)

            top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = response.d[3];
        if (top.window.document.getElementById("ctl00_C5POBody_lblMedication") != undefined && top.window.document.getElementById("ctl00_C5POBody_lblMedication") != null)

            top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = response.d[4];

        if (top.window.document.getElementById("Allergies_tooltp") != null && top.window.document.getElementById("Allergies_tooltp") != undefined) {
            if (response.d[5].replace("Allergies :<br/>", "").length != 0)
                top.window.document.getElementById("Allergies_tooltp").innerText = response.d[5].replace(regex, "\n") + "\n";
            else
                top.window.document.getElementById("Allergies_tooltp").innerText = "";
        }

        if (top.window.document.getElementById("CheifComplaints_tooltp") != null && top.window.document.getElementById("CheifComplaints_tooltp") != undefined) {

            if (response.d[6].replace("Chief Complaints :<br/><br/>", "").length != 0)
                top.window.document.getElementById("CheifComplaints_tooltp").innerText = response.d[6].replace(regex, "\n") + "\n";
            else
                top.window.document.getElementById("CheifComplaints_tooltp").innerText = "";

        }

        if (top.window.document.getElementById("ProblemList_tooltp") != null && top.window.document.getElementById("ProblemList_tooltp") != undefined) {

            if (response.d[7].replace("Problem List :<br/>", "").length != 0)
                top.window.document.getElementById("ProblemList_tooltp").innerText = response.d[7].replace(regex, "\n") + "\n";
            else
                top.window.document.getElementById("ProblemList_tooltp").innerText = "";
        }
        if (top.window.document.getElementById("Vitals_tooltp") != null && top.window.document.getElementById("Vitals_tooltp") != undefined) {

            if (response.d[8].replace("Vitals :<br/>", "").length != 0)
                top.window.document.getElementById("Vitals_tooltp").innerText = response.d[8].replace(regex, "\n") + "\n";
            else
                top.window.document.getElementById("Vitals_tooltp").innerText = "";
        }
        if (top.window.document.getElementById("Medication_tooltp") != null && top.window.document.getElementById("Medication_tooltp") != undefined) {

            if (response.d[9].replace("Medication :<br/>", "").length != 0)
                top.window.document.getElementById("Medication_tooltp").innerText = response.d[9].replace(regex, "\n") + "\n";
            else
                top.window.document.getElementById("Medication_tooltp").innerText = "";
        }
        RefreshOverallSummaryTooltip();
    }
    if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined && window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText != null && window.parent.parent.document.all("ctl00_C5POBody_lblVitals") != undefined && window.parent.parent.document.all("ctl00_C5POBody_lblVitals") != null) {
        var sDtls = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;

        document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
            "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
           window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
            window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];

    }
}
function CreateCodingException() {
    $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show'); if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1)
        reloadSummary();
    $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Create Coding Exception";
    $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
    $(top.window.document).find("#TabmdldlgException")[0].style.height = "680px";
    var sPath = ""
    var patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
    sPath = "frmException.aspx?formName=" + "Create Coding Exception" + "&PatientName=" + patientName;
    $(top.window.document).find("#TabExceptionFrame")[0].style.height = "605px";
    $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
    });
    return false;
}
function FeedbackCodingException(Addendumid) {
    //CAP-2202
    if (localStorage.getItem("OpenFeedbackCoding") != "NO") {
        $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Feedback for Coding Exception";
        $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
        $(top.window.document).find("#TabmdldlgException")[0].style.height = "800px";
        var sPath = ""
        var patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
        sPath = "frmException.aspx?formName=" + "Feedback for Coding Exception" + "&PatientName=" + patientName + "&AddendumID=" + Addendumid;
        $(top.window.document).find("#TabExceptionFrame")[0].style.height = "725px";
        $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
        });
    }
    return false;
}
function OpenDRsummary(url) {

    $('#resultLoading').css("display", "none");
    Result = openWindowNonModalDRsumm(url, 720, 950, obj);

    if (Result == null)
        return false;
}
function openWindowNonModalDRsumm(fromname, height, width, inputargument) {

    var Argument = "";
    var PageName = fromname;
    if (inputargument != undefined) {
        for (var i = 0; i < inputargument.length; i++) {
            if (i != 0) {
                Argument = Argument + "&" + inputargument[i];
            }
            else {
                Argument = inputargument[i];
            }
        }
        if (inputargument.indexOf('?') == -1 && inputargument.length != 0) {
            PageName = PageName + "?";
        }
    }


    var result = window.open(PageName + Argument, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes,titlebar=no,toolbar=no");
    if (result != null)
        result.moveTo(((screen.width - width) / 2), ((screen.height - height) / 2));

    if (result == undefined) { result = window.returnValue; }
    return result;
}
$('#resultLoading').css("display", "none");

function showTip(lbl) {
    document.getElementById(GetClientId(lbl.id)).title = document.getElementById('ctl00_C5POBody_hdnPatientStrip').value;
}

function invisibleResult() {
    $('#Resultsimg').css("display", "none");
    $("#ResultsText").css("display", "none")
    $("#ResultLine").css("display", "none");
}