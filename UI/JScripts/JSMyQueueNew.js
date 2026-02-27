var Role;

var isproviderReview = false;

function loadGeneralQueue() {
    document.getElementById("divGeneralQ").style.display = "";
    document.getElementById("divMyQ").style.display = "none";
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $('#RefreshQ').css("background-color", "");
    $('#btnChkOut').css("background-color", "");
    $('#MoveTo').css("background-color", "");
    $('#MoveTo')[0].innerText = "Move To My Encounters";
    $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
    if (Role == "Medical Assistant" || Role == "Office Manager") {
        $('#Exam')[0].style.visibility = "visible";
        $('#lblEr')[0].style.visibility = "visible";
    }
    else {
        $('#Exam')[0].style.visibility = "hidden";
        $('#lblEr')[0].style.visibility = "hidden";
    }
    $('#btnChkOut')[0].style.visibility = "visible";
    $('#Processenc')[0].style.display = "none";
    $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

    $("#btnEnc").removeClass("default");
    $("#btnEnc").addClass("btncolorMyQ");
    $("#btnGeneral").removeClass("default");
    $("#btnGeneral").addClass("btncolorMyQ");
    $("#btnMyQ").removeClass("btncolorMyQ");
    $("#btnMyQ").addClass("default");

    $("#btnAmendmnt").removeClass("btncolorMyQ");
    $("#btnAmendmnt").addClass("default");
    $("#btnOrder").removeClass("btncolorMyQ");
    $("#btnOrder").addClass("default");

    $('#btnChkOut')[0].style.display = "";//BugID:48827
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.setTimeout(GenQLoad, 300);
}
$(document).ready(function () {
    localStorage.setItem("PrevSubTab", "");
    //CAP-2202
    localStorage.setItem("OpenFeedbackCoding", "YES");

    if ($("#MovetoNxtProcess") != null)
        $("#MovetoNxtProcess")[0].disabled = true;
    if (sessionStorage.getItem('Next_Order_Index') != undefined) { sessionStorage.removeItem('Next_Order_Index'); }
    if (sessionStorage.getItem('CloseNotification') != undefined) { sessionStorage.removeItem('CloseNotification'); }
    if (sessionStorage.getItem('bCCSave') != undefined) { sessionStorage.removeItem('bCCSave'); }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById("divMyQ").style.display = "";
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $('#MyQTable').empty();
    $("#ProcessModal").modal('hide');
    var MyShowAllmyQueue = localStorage.getItem('MyShowAll');
    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    var TabName = "";
    if (MyShowAllmyQueue == "Checked") {
        TabName = "MyQueue";
    }
    else if (MyShowAll == "Checked") {
        TabName = "GenQueue";
    }
    if (MyShowAll == "Checked" || MyShowAllmyQueue == "Checked") {
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/AllTabCount",
            data: JSON.stringify({
                "sTabName": TabName
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                var objdata = $.parseJSON(data.d);

                sessionStorage.setItem("My_Task_Count", objdata.count[0].My_Task_Count);
                sessionStorage.setItem("My_Order_Count", objdata.count[0].My_Order_Count);
                sessionStorage.setItem("My_Scan_Count", objdata.count[0].My_Scan_Count);
                sessionStorage.setItem("My_Presc_Count", objdata.count[0].My_Presc_Count);
                sessionStorage.setItem("My_Amendmnt_Count", objdata.count[0].My_Amendmnt_Count);

                sessionStorage.setItem("Order_Count", objdata.count[0].Order_Count);
                sessionStorage.setItem("Amendmnt_Count", objdata.count[0].Amendmnt_Count);
                sessionStorage.setItem("Task_Count", objdata.count[0].Task_Count);
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    //CAP-792
                    if (isValidJSON(xhr.responseText)) {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                    else {
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry.");
                    }
                }
            }
        });

    }
    if (MyShowAllmyQueue == "Checked") {
        $("#chkMyShowAll")[0].checked = true;
        Showall = "Checked";
        LoadMyEncounter();
    }
   else if (MyShowAll == "Checked") {
        $("#chkShowAll")[0].checked = true;
        Showall = "Checked";
        loadGeneralQueue();
    }
    else {
        var role = $('#ctl00_hdnuserrole').val();
        if (role != "Medical Assistant" && role != "Front Office" && role != "Surgery Coordinator" && role != "Scribe") {
            LoadMyEncounter('MyEncounterLoad');
        } else {
            loadGeneralQueue();
        }
    }

    $('#btnChkOut').click(function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 1) {
            $('#GeneralQTable tr').removeClass("highlight");
            $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#GeneralQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            alert("Please Select one encounter to check out");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else if ($('#GeneralQTable').children().find('.highlight').length > 0) {
            var currentProcess = $('#EncounterTable > tbody > tr.highlight > td:nth-child(8)')[0].outerText.trim();
            if (currentProcess == 'CHECK_OUT') {
                var HumanId = $('#EncounterTable > tbody > tr.highlight > td:nth-child(3)')[0].outerText.trim();
                var EncounterId = $('#EncounterTable > tbody > tr.highlight > td:nth-child(14)')[0].outerText.trim();
                var PhysicianId = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].outerText.trim();
                if (HumanId != undefined && HumanId != '' && EncounterId != undefined && EncounterId != '' && PhysicianId != undefined && PhysicianId != '') {
                    var obj = new Array();
                    $('#GeneralQTable td').find('input[type=checkbox]:checked')[0].checked = false;;
                    $('#GeneralQTable tr').removeClass("highlight");
                    var Result = openRadWindow("frmCheckOut.aspx?HumanID=" + HumanId + "&EncounterID=" + EncounterId + "&PhysicianId=" + PhysicianId, 800, 1260, obj, 'RadWindowCheckout');

                    return false;
                }
            }
            else {
                alert('Selected status does not permit Check Out');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        }
        else {
            alert("Select an encounter");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
    $('#Processenc').click(function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 1) {
            $('#GeneralQTable tr').removeClass("highlight");
            $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#GeneralQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            if ((this).innerText.indexOf('Task') > -1) {
                alert("Please select one Task to process");
            }
            else if ((this).innerText.indexOf('Encounter') > -1) {
                alert("Please select one encounter to process");
            }
            //alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else if ($('#GeneralQTable').children().find('.highlight').length > 0) {
            var Showall = '';
            $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
            if ((this).innerText.indexOf('Task') > -1) {
                if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Task Q") > -1 && $('#RefreshQ').is(":visible")) {
                    var currRow = $('#GeneralQTable').children().find('.highlight');
                    var human_id = $(currRow)[0].children[2].innerText.trim();
                    var Facility = $(currRow)[0].children[6].innerText.trim();
                    var MessageId = $(currRow)[0].children[8].innerText.trim();
                    var Data = [human_id, Facility, 'GeneralQ', 'TASK', MessageId];
                    if (human_id == 'Acct. #' || Facility == 'Facility Name' || MessageId == 'Message_ID') {
                        alert("Please select one encounter to process");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }

                    var sPersonname = '';
                    for (var l = 0; l < cookies.length; l++) {
                        if (cookies[l].indexOf("CPersonName") > -1) {
                            sPersonname = cookies[l].split("=")[1];
                        }
                    }
                    //if (sPersonname != $(currRow)[0].children[5].innerText.trim()) {
                    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    //    return false;
                    //}
                    $.ajax({
                        type: "POST",
                        url: "frmMyQueueNew.aspx/ProcessTask",
                        data: JSON.stringify({
                            "data": Data,
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var obj = new Array();
                            jQuery('body').css('cursor', 'default');
                            //for BugID:38984- to open task with patientchart as background.

                            var Result = openRadWindow("frmPatientCommunication.aspx?AccountNum=" + human_id + "&openingfrom=GenQTask&parentscreen=" + "MyQ" + "&MessageID=" + MessageId, 550, 1050, obj, 'ModalWindow');
                            var windowName = $find('ModalWindow');
                            windowName.add_close(OnClientCloseWindow);
                            windowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        },
                        error: function OnError(xhr) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            if (xhr.status == 999)
                                window.location = "/frmSessionExpired.aspx";
                            else {
                                //CAP-792
                                if (isValidJSON(xhr.responseText)) {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    //alert("USER MESSAGE:\n" +
                                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                    //    "Message: " + log.Message);
                                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                                }
                                else {
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry.");
                                }
                            }
                        }
                    });
                }
            }
            else if ((this).innerText.indexOf('Encounter') > -1) {
                var currentProcess = $('#EncounterTable > tbody > tr.highlight > td:nth-child(8)')[0].outerText.trim();
                //Jira #CAP-706
                //if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS') {
                if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS' || currentProcess == "AKIDO_SCRIBE_PROCESS" || currentProcess == 'AKIDO_REVIEW_CODING' || currentProcess == 'TRANSCRIPT_PROCESS' || currentProcess == 'TRANSCRIPT_QC_PROCESS' || currentProcess == "AKIDO_SCRIBE_QC_PROCESS" || currentProcess == 'AKIDO_REVIEW_CODING_QC' || currentProcess == 'TECHNICIAN_PROCESS') {
                    var objType = $('#EncounterTable > tbody > tr.highlight > td:nth-child(16)')[0].outerText.trim();
                    //Jira #CAP-706
                    //if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'TECHNICIAN_PROCESS' || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS') {
                    if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'TECHNICIAN_PROCESS' || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS' || currentProcess == "AKIDO_SCRIBE_PROCESS" || currentProcess == 'AKIDO_REVIEW_CODING' || currentProcess == 'TRANSCRIPT_PROCESS' || currentProcess == 'TRANSCRIPT_QC_PROCESS' || currentProcess == "AKIDO_SCRIBE_QC_PROCESS" || currentProcess == 'AKIDO_REVIEW_CODING_QC' || currentProcess == 'TECHNICIAN_PROCESS') {
                        var encounterID = $('#EncounterTable > tbody > tr.highlight > td:nth-child(14)')[0].outerText.trim();
                    }
                    var apptdt = $('#EncounterTable > tbody > tr.highlight > td:nth-child(2)')[0].outerText.trim();
                    var humanid = $('#EncounterTable > tbody > tr.highlight > td:nth-child(3)')[0].outerText.trim();
                    var physcianid = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].outerText.trim();
                    var date = $('#EncounterTable > tbody > tr.highlight > td:nth-child(17)')[0].outerText.trim();
                    var ExamRoom = document.getElementById("Exam").value;
                    var now = new Date();
                    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
                    if (currentProcess == "CHECK_OUT") {
                        if (humanid != undefined && humanid != '' && encounterID != undefined && encounterID != '' && physcianid != undefined && physcianid != '') {
                            var obj = new Array();
                            $('#GeneralQTable td').find('input[type=checkbox]:checked')[0].checked = false;
                            $('#GeneralQTable tr').removeClass("highlight");
                            var Result = openRadWindow("frmCheckOut.aspx?HumanID=" + humanid + "&EncounterID=" + encounterID + "&PhysicianId=" + physcianid, 800, 1260, obj, 'RadWindowCheckout');
                            return false;
                        }
                    }
                    else {
                        var Data = [humanid, encounterID, physcianid, currentProcess, objType, date, utc, ExamRoom, Showall, apptdt];

                        $.ajax({
                            type: "POST",
                            url: "frmMyQueueNew.aspx/ProcessGenEncounter",
                            data: JSON.stringify({
                                "data": Data,
                            }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                //jira #cap190 - Workflow issue - Current process PROVIDER_PROCESS current owner MA
                                if (data.d == 'UNKNOWN') {
                                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                    DisplayErrorMessage('1400013');
                                    return false;
                                }
                                sessionStorage.setItem("EnablePFSHMenu", "true");
                                localStorage.setItem("CodingException", encounterID);
                                //Jira #CAP-709
                                localStorage.setItem("CurrentProcess", currentProcess);
                                sessionStorage.setItem("EncId_PatSummaryBar", encounterID);
                                //sessionStorage.setItem("Enc_DOS", date);
                                sessionStorage.setItem("Enc_DOS", data.d);
                                window.location = "frmPatientChart.aspx?hdnLocalTime=" + utc + "&Notification_Type=All";//BugID:52556,52627
                                $('#GeneralQTable td').find('input[type=checkbox]:checked').removeAttr('checked');
                                $('#GeneralQTable tr').removeClass("highlight");

                            },
                            error: function OnError(xhr) {
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                if (xhr.status == 999)
                                    window.location = "/frmSessionExpired.aspx";
                                else {
                                    //CAP-3829
                                    if (isValidJSON(xhr.responseText)) {
                                        var log = JSON.parse(xhr.responseText);
                                        console.log(log);
                                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                                    }
                                    else {
                                        alert(`USER MESSAGE:\nCannot process request. Please Login again and retry.\nEXCEPTION DETAILS:\nMessage: ${xhr.responseText}`);
                                    }
                                }
                            }
                        });
                    }
                }
                else {
                    alert('Selected encounter cannot be processed.');
                    $('#GeneralQTable td').find('input[type=checkbox]:checked').removeAttr('checked');
                    $('#GeneralQTable tr').removeClass("highlight");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }
            }
        }
        else {
            if ((this).innerText.indexOf('Task') > -1) {
                alert("Please select an Task to process.");
            }
            else if ((this).innerText.indexOf('Encounter') > -1) {
                alert("Please select an encounter to process.");
            }
            //alert("Please select an encounter to process.");
            $('#GeneralQTable td').find('input[type=checkbox]:checked').removeAttr('checked');
            $('#GeneralQTable tr').removeClass("highlight");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        return false;
    });
    $('#Processenctr').click(function () {

        //Jira #CAP-889
        sessionStorage.setItem('MyQRemoveIdList', '');

        //Jira CAP-1354
        if ($('#MyQTable').children().find('.highlight').length > 1) {
            alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable tr').removeClass("highlight");
            //Jira CAP-1444
            if ($('#MovetoNxtProcess') != undefined && $('#MovetoNxtProcess')[0]?.disabled != undefined) {
                $('#MovetoNxtProcess')[0].disabled = true;
            }
            return false;
        }


        if ($('#MyQTable').children().find('.highlight').length > 0 && $('#MyQTable').children().find('.highlight')[0].classList.length == 1) {
            var currentProcessscnt = 0;
            $('#MyQTable tr.highlight').each(function (i, row) {
                var $row = $(row);
                if ($row.find('td:nth-child(8)')[0] != undefined) {
                    var currentProcesss = $row.find('td:nth-child(8)')[0].outerText.trim();
                    if (currentProcesss == "PROVIDER_REVIEW" || currentProcesss == "PROVIDER_REVIEW_2")
                        currentProcessscnt++;
                }
            });
            if (currentProcessscnt > 1) {
                alert("Please select one encounter to process");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable tr').removeClass("highlight");
                return false;
            }
            else if ($('#EncounterTable tr.highlight')[0].innerText == "No data available in table") {
                return false;
            }
            else {
                MyQclick();
            }
        }
        else {
            var currentProcessscnt = 0;
            $('#MyQTable tr.highlight').each(function (i, row) {
                var $row = $(row);
                if ($row.find('td:nth-child(8)')[0] != undefined) {
                    var currentProcesss = $row.find('td:nth-child(8)')[0].outerText.trim();
                    if (currentProcesss == "PROVIDER_REVIEW" || currentProcesss == "PROVIDER_REVIEW_2")
                        currentProcessscnt++;
                }
            });
            if (currentProcessscnt > 1) {
                alert("Please select one encounter to process");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable tr').removeClass("highlight");
                return false;
            }
            else {
                alert("Please select an encounter to process.");
            }
        }
    });
    $("th").hover(function () {
        $("th").css('cursor', 'pointer');
    });
});
function checkboxclick(evt) {
    if (evt.checked)
        $(evt.parentNode.parentElement).addClass("highlight");
    else
        $(evt.parentNode.parentElement).removeClass("highlight");
}
function selectAll(evt) {
    if (evt.checked) {
        $('#GeneralQTable td').find('input[type=checkbox]').prop('checked', 'checked');
        $('#GeneralQTable tbody tr').addClass("highlight");
        if ($("#EncounterTable tbody tr").length == 1 && $("#EncounterTable tbody tr td").length == 1 && $("#EncounterTable tbody tr td.dataTables_empty").length == 1) {
            $("#EncounterTable tbody tr").removeClass("highlight");
        }
        //return false;       
    }
    else {
        $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('#GeneralQTable tr').removeClass("highlight")
        return false;
    }
}
function MyQclick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    localStorage.setItem('PhysicianCPT', '');
    localStorage.setItem('PhysicianICD', '');
    if ($('#MyQTable').children().find('.highlight').length > 0) {
        var currRow = $('#MyQTable').children().find('.highlight');
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").css("background-color", "#4f94cd");
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").removeClass("not-active");
        if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Encounters") > -1 && $('#RefreshMyQ').is(":visible")) {
            var Showall = '';
            $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
            var apptdt = $(currRow)[0]?.children[1]?.innerText.trim();
            //var Curprocess = $(currRow)[0].children[7].innerText.trim();
            var Curprocess = '';
            var human_id = $(currRow)[0]?.children[2]?.innerText.trim();
            var PhyID = '';
            var date = '';
            var encounter_id = '';
            var objtype = '';
            //CAP 314  Uncaught Type Error: Cannot read properties of undefined (reading inner Text)    
            if ($(currRow)[0]?.children[7]?.innerText.trim() == 'TECHNICIAN_PROCESS' && $(currRow)[0]?.children[7]?.innerText.trim() != undefined && $(currRow)[0]?.children[7]?.innerText.trim() != null) {
                Curprocess = $(currRow)[0]?.children[7]?.innerText.trim()
                PhyID = $(currRow)[0]?.children[12]?.innerText.trim();
                date = $(currRow)[0]?.children[14]?.innerText.trim();
                //Jira CAP-2809
                //encounter_id = $(currRow)[0]?.children[11]?.innerText.trim();
                if ($("#EncounterTable thead tr")[0]?.children[11]?.children[0]?.innerHTML != undefined && $("#EncounterTable thead tr")[0]?.children[11]?.children[0]?.innerHTML == "Encounter ID") {
                    encounter_id = $(currRow)[0]?.children[11]?.innerText.trim();
                } else if ($("#EncounterTable thead tr")[0]?.children[11]?.children[0]?.innerHTML != undefined && $("#EncounterTable thead tr")[0]?.children[11]?.children[0]?.innerHTML != "Encounter ID") {
                    DisplayErrorMessage('1400014');
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }
                //Jira CAP-2809 - End
                objtype = $(currRow)[0]?.children[13]?.innerText.trim();
            }
            else {
                Curprocess = $(currRow)[0]?.children[7]?.innerText.trim();
                PhyID = $(currRow)[0]?.children[14]?.innerText.trim();
                date = $(currRow)[0]?.children[16]?.innerText.trim();
                encounter_id = $(currRow)[0]?.children[13]?.innerText.trim();
                objtype = $(currRow)[0]?.children[15]?.innerText.trim();
            }

            //CAP-618 find all headers from the table.
            var headerList = [];
            $("#MyQTable thead tr th").each(function () {
                headerList.push($(this).text());
            });

            //CAP-314 alert message if any parameter missing
            if (headerList?.includes('Test Details') == false && (Curprocess == undefined || Curprocess == null || Curprocess == '')) {
                alert("Curprocess is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            //CAP-618 alert message if has Test Details.
            else if (headerList?.includes('Test Details') == true && (Curprocess == undefined || Curprocess == null || Curprocess == '')) {
                alert("Please relogin using default facility to access these encounters in My Q.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (PhyID == undefined || PhyID == null || PhyID == '') {
                alert("PhyID is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (date == undefined || date == null || date == '') {
                alert("Date is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (encounter_id == undefined || encounter_id == null || encounter_id == '') {
                alert("Encounter_ID is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (objtype == undefined || objtype == null || objtype == '') {
                alert("ObjType is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }

            var now = new Date();
            var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();


            var Data = [human_id, encounter_id, Facility, PhyID, Curprocess, objtype, date, utc, apptdt];
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessEncounter",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    localStorage.setItem("CodingException", encounter_id);
                    //J/ira #CAP-709
                    localStorage.setItem("CurrentProcess", Curprocess);
                    sessionStorage.setItem("EnablePFSHMenu", "true");
                    sessionStorage.setItem("EncId_PatSummaryBar", encounter_id);
                    sessionStorage.setItem("Enc_DOS", date);
                    window.location = "frmPatientChart.aspx?hdnLocalTime=" + utc + "&Process=" + "MA_Process_Encounter" + "&EncounterDate=" + date + "&EncounterID=" + encounter_id + "&HumanID=" + human_id + "";
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            });
        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Tasks") > -1 && $('#RefreshMyQ').is(":visible")) {
            if ($("#chkMyTask14")[0].checked) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            var human_id = $(currRow)[0].children[1].innerText.trim();
            var Facility = $(currRow)[0].children[6].innerText.trim();
            var MessageId = $(currRow)[0].children[8].innerText.trim();
            var Data = [human_id, Facility];
            var sPersonname = '';
            for (var l = 0; l < cookies.length; l++) {
                if (cookies[l].indexOf("CPersonName") > -1) {
                    sPersonname = cookies[l].split("=")[1];
                }
            }
            if (sPersonname != $(currRow)[0].children[5].innerText.trim()) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessTask",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    var obj = new Array();
                    jQuery('body').css('cursor', 'default');
                    //for BugID:38984- to open task with patientchart as background.

                    var Result = openRadWindow("frmPatientCommunication.aspx?AccountNum=" + human_id + "&openingfrom=Task&parentscreen=" + "MyQ" + "&MessageID=" + MessageId, 550, 1050, obj, 'ModalWindow');
                    var windowName = $find('ModalWindow');
                    windowName.add_close(OnClientCloseWindow);
                    windowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            });
        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Scan") > -1 && $('#RefreshMyQ').is(":visible")) {
            var now = new Date();
            var utc = now.getTimezoneOffset();
            var sPath = "";
            var ScanID = $(currRow)[0].children[5].innerText;
            if (ScanID != null && ScanID != "") {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                var obj = new Array();
                openRadWindow("frmIndexing.aspx?ScanId=" + ScanID + "&CurrentZone=" + utc, 700, 1225, obj, 'IndexWindow');
                $find('IndexWindow').set_behaviors(Telerik.Web.UI.WindowBehaviors.None);
                //$find('IndexWindow').add_close(function () { $('#RefreshMyQ').click(); });
                $find('IndexWindow').add_close(OnClientCloseWindow);
                return false;
            }
        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Orders") > -1 && $('#RefreshMyQ').is(":visible")) {
            var CurrentProcess = $(currRow)[0].children[8].innerText.trim();
            sessionStorage.setItem("Next_Order_Index", $('#MyQTable tr').index($(currRow)));//BugID:41027 -- move to next result
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessOrder",
                data: JSON.stringify({
                    "CurrentProcess": CurrentProcess,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            });
            var sPath = "";
            var orderType = $(currRow)[0].children[14].innerText.replace("INTERNAL", "").trim();
            var ObjType = $(currRow)[0].children[14].innerText;

            // ---region BugID:42368---
            var fromMyOrderQ = "true";
            var OrderSubmitID = "";
            var File_Ref_ID = "";//BugID:43099
            var HumanID = $(currRow)[0].children[2].innerText.trim();
            var EncounterID = $(currRow)[0].children[11].innerText.trim();
            var Physician_ID = $(currRow)[0].children[12].innerText.trim();
            if (orderType == "IMMUNIZATION ORDER" || orderType == "REFERRAL ORDER")
                OrderSubmitID = $(currRow)[0].children[13].innerText.trim();
            else
                OrderSubmitID = $(currRow)[0].children[17].innerText.trim();
            var ResultMasterID = $(currRow)[0].children[19].innerText.trim();
            File_Ref_ID = $(currRow)[0].children[20].innerText.trim();
            var LabID = $(currRow)[0].children[15].innerText.trim();
            var vDescription = $(currRow)[0].children[6].innerText.trim();

            if (CurrentProcess == "MA_REVIEW" && (orderType == "DIAGNOSTIC ORDER" || orderType == "IMAGE ORDER" || orderType == "DME ORDER")) {
                var obj = new Array();
                var Result;
                if (orderType == "DIAGNOSTIC ORDER" || orderType == "IMAGE ORDER")
                    Result = openRadWindow("frmOrdersList.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[17].innerText + "&ScreenMode=MyQ&Openingfrom=MyorderQueue", 665, 1232, obj, 'MessageWindow');
                else if (orderType == "DME ORDER")
                    Result = openRadWindow("frmDMEOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[17].innerText + "&ScreenMode=MyQ&Openingfrom=MyorderQueue", 665, 1232, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');

                windowName.add_close(OnClientCloseWindow);
                {
              //Cap - 1393
             //sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                }
                return false;
            }
            else if (ObjType.toUpperCase() == "DIAGNOSTIC_RESULT") {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "95%";//BugID:47602
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&ObjType=" + ObjType + "&ResultMasterID=" + ResultMasterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&CurrentProcess=" + CurrentProcess + "&Opening_from=OrdersQ&Openingfrom=MyorderQueue&Result_OBR_Date=" + $(currRow)[0].children[0].innerText.trim().split(' ')[0];//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "RESULT_REVIEW" || orderType == "DICTATION_REVIEW") {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "97%";//BugID:47602
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&OrderSubmitId=" + OrderSubmitID + "&EncounterId=" + EncounterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&Opening_from=OrdersQ&Openingfrom=MyorderQueue&CurrentProcess=" + CurrentProcess + "&File_Ref_ID=" + File_Ref_ID + "&Description=" + vDescription;//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "RESULT_REVIEW" || orderType == "IMMUNIZATION ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmImmunization.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianID=" + $(currRow)[0].children[12].innerText + "&LabId=" + $(currRow)[0].children[15].innerText + "&Openingfrom=MyorderQueue&Screen=MyQ", 800, 1135, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (CurrentProcess == "MA_RESULTS") {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "97%";//BugID:47602
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&OrderSubmitId=" + OrderSubmitID + "&EncounterId=" + EncounterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&Opening_from=OrdersQ" + "&MA=True&Openingfrom=MyorderQueue&CurrentProcess=" + CurrentProcess + "&File_Ref_ID=" + File_Ref_ID;//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "MA_REVIEW" && orderType == "REFERRAL ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmReferralOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText + "&Openingfrom=MyorderQueue&ScreenMode=Myqueue", 750, 1230, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (CurrentProcess == "PHYSICIAN_VERIFY" && orderType == "REFERRAL ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmReferralOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText + "&Openingfrom=MyorderQueue&ScreenMode=Myqueue", 830, 1230, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }

            // ---end region BugID:42368---
        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Amendment") > -1 && $('#RefreshMyQ').is(":visible")) {
            var human_id = $(currRow)[0].children[2].innerText.trim();
            var encounter_id = $(currRow)[0].children[9].innerText.trim();
            var PhyID = $(currRow)[0].children[10].innerText.trim();
            var Curprocess = $(currRow)[0].children[5].innerText.trim();
            var objtype = $(currRow)[0].children[11].innerText.trim();
            var AddendumID = $(currRow)[0].children[12].innerText.trim();
            var Data = [human_id, encounter_id, PhyID, Curprocess, objtype];
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessAddendum",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    localStorage.setItem("CodingException", encounter_id);
                    //Jira #CAP-709
                    localStorage.setItem("CurrentProcess", Curprocess);
                    sessionStorage.setItem("EncId_PatSummaryBar", encounter_id);
                    sessionStorage.setItem("Enc_DOS", "");
                    window.location = "frmPatientChart.aspx?isOpenAddendum=true" + "&currentAddendumId=" + AddendumID;
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            });

        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Prescription") > -1 && $('#RefreshMyQ').is(":visible")) {
            var human_id = $(currRow)[0].children[1].innerText.trim();
            var encounter_id = $(currRow)[0].children[6].innerText.trim();
            var prescriptionId = $(currRow)[0].children[7].innerText.trim();
            var Data = [human_id, encounter_id];
            var obj = new Array();
            //Jira CAP-1567
            sessionStorage.setItem("eRxHumanID", human_id);
            var Result = openRadWindow("frmRCopiaWebBrowser.aspx?MyType=GENERAL&HumanID=" + human_id + "&EncID=" + encounter_id + "&PrescriptionID=" + prescriptionId +
                "&IsMoveButton=true&IsMoveCheckbox=false&IsPrescriptiontobePushed=N&openingFrom=Queue&IsSentToRCopia=Y", 630, 1350, obj, 'MessageWindow');
            var windowName = $find('MessageWindow');
            windowName.add_close(OnClientCloseWindow);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    else
        return false;
}
function OnClientCloseWindow() {
    //Jira CAP-1567
    if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Prescription") > -1 && $('#RefreshMyQ').is(":visible")) {
        StartRcopiaStrip();

        $.ajax({
            type: "POST",
            url: "frmEncounter.aspx/DownloadRcoipaOutSidePatientChart",
            data: '{sHuman_id: "' + sessionStorage.getItem("eRxHumanID") + '"}',
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                document.cookie = "CeRxFlag=false";
                document.cookie = "CeRxHumanID=";
                //Jira CAP-1366
                StopRcopiaStrip();
                RcopiaErrorAlert(data.d);
            },
            error: function (result) {
                //Jira CAP-1366
                StopRcopiaStrip();
                alert(result.d);
            }
        });
    }

    //Jira #CAP-889
    //chkShowAllClick();
    //CAP-1471
    var removeList = sessionStorage.getItem('MyQRemoveIdList');
    var btnid = '';
    if ($('#btnMyQ')[0].className.indexOf('btncolorMyQ') > -1) {
        btnid = $('#divMyQTab .btncolorMyQ')[0]?.id;
    }
    else {
        btnid = $('#divGeneralQTabs .btncolorMyQ')[0]?.id;
    }
    
    if (removeList != "" && removeList !=null) {
        var removearry = removeList.split(",");
        for (let i = 0; i < removearry.length; i++) {
            if (btnid.indexOf("Order") > -1) {
                var table = new DataTable('#EncounterTable');
                table.row($('#MyQTable tr').find('td:eq(' + removearry[i].split("~")[1] + '):contains(' + removearry[i].split("~")[0] + ')').parent()).remove().draw(false);
            }
            else if (btnid == "btnMyTask") {
                var table = new DataTable('#EncounterTable');
                table.row($('#MyQTable tr').find('td:eq(8):contains(' + removearry[i].split("~")[0] + ')').parent()).remove().draw(false);
            }
            else if (btnid == "btnTask") {
                var table = new DataTable('#EncounterTable');
                table.row($('#GeneralQTable tr').find('td:eq(8):contains(' + removearry[i].split("~")[0] + ')').parent()).remove().draw(false);
               // $('#GeneralQTable tr').find('td:eq(8):contains(' + removearry[i].split("~")[0] + ')').parent().remove();
            }
            else if (btnid == "btnMyScan") {
                var table = new DataTable('#EncounterTable');
                table.row($('#MyQTable tr').find('td:eq(5):contains(' + removearry[i].split("~")[0] + ')').parent()).remove().draw(false);
                //$('#MyQTable tr').find('td:eq(5):contains(' + removearry[i].split("~")[0] + ')').parent().remove();
            }
            else if (btnid == 'btnMyPres') {
                var table = new DataTable('#EncounterTable');
                table.row($('#MyQTable tr').find('td:eq(7):contains(' + removearry[i].split("~")[0] + ')').parent()).remove().draw(false);
            }
            else {
                chkShowAllClick();
            }
        }

        var numberofEncounters = "";
        if (btnid == "btnTask" && $('#GeneralQTable').find("#EncounterTable tbody").length > 0) {
          //  numberofEncounters = $('#GeneralQTable').find("#EncounterTable tbody").children().length;
            var table = new DataTable('#EncounterTable');
            numberofEncounters = table.data().count();
        }
        else if ($('#dvAdd').find("#EncounterTable tbody").length > 0) {
            numberofEncounters = $('#dvAdd').find("#EncounterTable tbody").children().length;
        }
        else if (btnid == "btnMyTask" || btnid == "btnMyScan" || btnid == "btnMyOrder" || btnid == "btnMyPres" ) {
            var table = new DataTable('#EncounterTable');
            numberofEncounters = table.data().count();
        }
        else {
            numberofEncounters = 0;
        }
        if (btnid != undefined && numberofEncounters != undefined) {
            document.getElementById(btnid).innerText = document.getElementById(btnid).innerText.split("(")[0] + " (" + numberofEncounters + ")";
        }
    }
   
}

function GenQLoad() {
    var sShowall = '';
    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    if (MyShowAll == "Checked") {
        $("#chkShowAll")[0].checked = true;
        LoadGeneralEncounter();
    }
    else {
        $("#chkShowAll")[0].checked = false;

        var ViewAllFacilities = "";
        if ($("#ctl00_C5POBody_chkViewAllFacilities")[0] != undefined && $("#ctl00_C5POBody_chkViewAllFacilities")[0] != null) {
            $("#ctl00_C5POBody_chkViewAllFacilities")[0].checked ? ViewAllFacilities = "Checked" : ViewAllFacilities = "Unchecked";
        }
        LoadGeneralEncounter('LoadEncounter');
    }
}
function MyQLoad() {
    var MyShowAll = localStorage.getItem('MyShowAll');
    if (MyShowAll == "Checked") {
        Showall = "Checked";
        $("#chkMyShowAll")[0].checked = true;
        LoadMyEncounter();
    }
    else {
        Showall = "Unchecked";
        $("#chkMyShowAll")[0].checked = false;
        LoadMyEncounter('EncounterLoad');
    }


}
function ShowMyQTabs(sender) {
    $("#chkMyShowAll")[0].checked = false;
    $("#chkShowAll")[0].checked = false;
    $(":button:not(#btnGeneralQcount):not(#btnMyQcount)").css("background-color", "transparent");
    if (sender.innerText == "General Q") {
        document.getElementById("divGeneralQ").style.display = "";
        document.getElementById("divMyQ").style.display = "none";
        $('#MyQTable').empty();
        $('#GeneralQTable').empty();
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#MoveTo')[0].innerText = "Move To My Encounters";
        $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
        if (Role == "Medical Assistant" || Role == "Office Manager") {
            $('#Exam')[0].style.visibility = "visible";
            $('#lblEr')[0].style.visibility = "visible";
        }
        else {
            $('#Exam')[0].style.visibility = "hidden";
            $('#lblEr')[0].style.visibility = "hidden";
        }
        $('#btnChkOut')[0].style.visibility = "visible";
        $('#Processenc')[0].style.display = "none";
        $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

        $("#btnEnc").removeClass("default");
        $("#btnEnc").addClass("btncolorMyQ");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $("#btnMyQ").removeClass("btncolorMyQ");
        $("#btnMyQ").addClass("default");

        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");
        $("#btnTask").removeClass("btncolorMyQ");
        $("#btnTask").addClass("default");

        $('#btnChkOut')[0].style.display = "";//BugID:48827
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        window.setTimeout(GenQLoad, 300);
    }
    else {
        document.getElementById("divGeneralQ").style.display = "none";
        document.getElementById("divMyQ").style.display = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Encounter";
        $('#MyQTable').empty();
        $("#btnMyEnc").removeClass("default");
        $("#btnMyEnc").addClass("btncolorMyQ");
        $("#btnMyQ").removeClass("default");
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnGeneral").removeClass("btncolorMyQ");
        $("#btnGeneral").addClass("default");


        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");


        $('#btnChangeExamRoom').css("display", "inline-block");
        $('#btnChangeExamRoom').css("background-color", "");
        $('#MovetoNxtProcess').css("display", "inline-block");
        $('#MovetoNxtProcess').css("background-color", "");
        $('#RefreshMyQ').css("background-color", "");
        $('#RefreshMyQ')[0].innerText = "Refresh My Encounters";//BugID:48827
        $('#chkMyTask14').css("display", "none");
        $('#ctl00_C5POBody_btnMyAkidoTasks').css("display", "none");
        $('#lbl14days').css("display", "none");
        $('#chkOpenTask').css("display", "none");
        $('#lblOpenTask').css("display", "none");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        window.setTimeout(MyQLoad, 300);
    }
}

function LoadMyEncounter(ajaxUrl) {
    if ($('#hdnIsShowAllMyEncountersQueue').val() == 'Y') {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "none");
    } else {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "");
    }
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $("#MyQTable").append(`
    <table id="EncounterTable" class="table table-bordered Gridbodystyle" style="table-layout: fixed;width:100%;">
    <thead class="header" style="border: 0px;width:96.7%;">
        <tr class="header">
            <th style="border: 1px solid #909090;text-align: center;width: 4%;">Select<input type="checkbox" class="myQChkbxAll" onclick="MyQselectAll(this)"></th>
            <th style="border: 1px solid #909090;text-align: center;">Appt. Date & Time</th>
            <th style="border: 1px solid #909090;text-align: center;">Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;">Ext. Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;">Patient Name</th>
            <th style="border: 1px solid #909090;text-align: center;">Patient DOB</th>
            <th style="border: 1px solid #909090;text-align: center;">Type of Visit</th>
            <th style="border: 1px solid #909090;text-align: center;">Current Process</th>
            <th style="border: 1px solid #909090;text-align: center;">Test Details</th>
            <th style="border: 1px solid #909090;text-align: center;">Ordering Physician</th>
            <th style="border: 1px solid #909090;text-align: center;">Facility Name</th>
            <th style="border: 1px solid #909090;text-align: center;">Assigned Physician</th>
            <th style="border: 1px solid #909090;text-align: center;">Pri. Carrier</th>
            <th style="border: 1px solid #909090;text-align: center;">Pri. Plan</th>
            <th style="border: 1px solid #909090;text-align: center;">eSuperbill Status</th>
            <th style="border: 1px solid #909090;text-align: center;">Encounter ID</th>
            <th style="border: 1px solid #909090;text-align: center;">Physician_ID</th>
            <th style="border: 1px solid #909090;text-align: center;">EHR_Obj_Type</th>
            <th style="border: 1px solid #909090;text-align: center;">Date_of_Service</th>
            <th style="border: 1px solid #909090;text-align: center;">QR Code</th>
        </tr>
    </thead>
</table>`);

    var ViewAllFacilities = "";
    if ($("#ctl00_C5POBody_chkViewAllFacilities")[0] != undefined && $("#ctl00_C5POBody_chkViewAllFacilities")[0] != null) {
        $("#ctl00_C5POBody_chkViewAllFacilities")[0].checked ? ViewAllFacilities = "Checked" : ViewAllFacilities = "Unchecked";
    }

    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    var Ancillary = $('#ctl00_C5POBody_hdnAncillary').val();
    ajaxUrl = ajaxUrl == '' || ajaxUrl == undefined ? 'chkShowAllMyEncounter' : ajaxUrl;
    var extra_search = ajaxUrl == 'MyEncounterLoad' ? ViewAllFacilities : '';
    extra_search = ajaxUrl == 'chkShowAllMyEncounter' ? Showall : '';

    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        processing: false,
        scrollCollapse: true,
        scrollY: '420px',
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. # or Encounter ID",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmMyQueueNew.aspx/' + ajaxUrl,
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = extra_search;
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);

                if (ajaxUrl == 'MyEncounterLoad') {
                    MyQBind1(objdata)
                }
                if (ajaxUrl == 'EncounterLoad') {
                    MyQBind2(objdata)
                }
                if (ajaxUrl == 'chkShowAllMyEncounter') {
                    MyQBind3(objdata)
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                json.data = objdata.data;
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: '', render: function (data, type, row) {
                    let disabled = "disabled='true'";
                    if (row.Current_Process.toUpperCase() == "PROVIDER_REVIEW" || row.Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
                        disabled = "";
                        disableOverallSelect = false;
                    }
                    if (Role != "Medical Assistant" && Role != "Front Office" && Role != "Surgery Coordinator" && Role != "Scribe") {
                        return "<input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/>";
                    } else {
                        return "<input type='checkbox' onclick='checkboxclick(this)' />";
                    }
                },
                sClass: "text-align-center",
                searchable: false,
                orderable: false,
                sWidth: '3%',
            },
            {
                data: 'Appt_Date_Time', render: function (data, type, row) {
                    return ConvertDate(data.replace("T", " "));
                },
                searchable: false,
                type: 'date',
                sWidth: '12%'
            },
            { data: 'Human_ID', sWidth: '6%' },
            { data: 'External_Account_Number', searchable: false, sWidth: '7%' },
            {
                data: 'Last_Name', render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                },
                sClass: 'word-break-all',
                sWidth: '10%'
            },
            {
                data: 'DOB', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                },
                searchable: false,
                type: 'date',
                sWidth: '8%'
            },
            { data: 'Type_Of_Visit', sClass: (Ancillary == 'false' ? '' : 'hide_column') +' word-break-all', searchable: false, sWidth: '8%' },
            { data: 'Current_Process', searchable: false, sWidth: '12%', sClass: 'process-word-wrap' },
            { data: 'Test_Details', visible: (Ancillary == 'true'), searchable: false, sWidth: '12%' },
            { data: 'Ordering_Physician', visible: (Ancillary == 'true'), searchable: false, sWidth: '12%' },
            { data: 'Facility_Name', visible: (Ancillary == 'false'), searchable: false, sWidth: '8%' },
            { data: 'PhyName', visible: (Ancillary == 'false'), searchable: false, sWidth: '12%' },
            { data: 'Carrier_Name', visible: (Ancillary == 'false'), searchable: false, sWidth: '8%' },
            { data: 'Insurance_Plan_Name', visible: (Ancillary == 'false'), searchable: false, sWidth: '8%' },
            {
                data: 'Is_EandM_Submitted', render: function (data, type, row) {
                    return data.toUpperCase() == 'Y' ? "Submitted" : "Not Submitted";
                },
                searchable: false,
                sWidth: '11%'
            },
            { data: 'Encounter_ID', sWidth: '7%' },
            { data: 'Physician_ID', sClass: 'hide_column', searchable: false },
            { data: 'EHR_Obj_Type', sClass: 'hide_column', searchable: false },
            { data: 'Date_of_Service', sClass: 'hide_column', searchable: false },
            {
                data: '', render: function (data, type, row) {
                    var dt1 = row.Date_of_Service.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    var dos = ConvertDate(new Date(parseInt(dt1)));
                    //return `<i class="fa fa-qrcode" style="font-size: xx-large;" onclick="QRCodeClick(${row.Human_ID},${row.Encounter_ID},'${dos}',${row.Physician_ID})"></i>`;
                    return `<i class="fa fa-qrcode" style="font-size: xx-large;" onclick="QRCodeClick(this)"></i>`;
                },
                sClass: "text-align-center",
                searchable: false,
                orderable: false,
                visible: (sessionStorage.getItem('IsAkidoPhysician') != null && sessionStorage.getItem('IsAkidoPhysician') == "YES"),
                sWidth: '6%'
            },
        ],
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }
    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

    dataTable.on('draw', function () {
        var info = dataTable.page.info();
        if (info.page !== info.previousPage) {
            $('.myQChkbxAll').prop('disabled', true);
            $('.myQChkbx').each(function () {
                if (!$(this).prop('disabled')) {
                    $('.myQChkbxAll').prop('disabled', false);
                    return;
                }
            });
        }
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        $('.myQChkbx,.myQChkbxAll').prop('checked', false);
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        $('.myQChkbx').prop('checked', false);
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        var existingSelectedItem = $("#MyQTable tr.highlight");
        for (var i = 0; i < existingSelectedItem.length; i++) {
            var processes = "NoCurrentProcess";
            if (existingSelectedItem[i]?.children[7]?.childNodes[0]?.data != undefined && existingSelectedItem[i]?.children[7]?.childNodes[0]?.data != null) {
                processes = existingSelectedItem[i].children[7].childNodes[0].data;
            }
            var isproviderReviewMyQ = processes;
            if (isproviderReviewMyQ != "PROVIDER_REVIEW" && isproviderReviewMyQ != "PROVIDER_REVIEW_2") {
                existingSelectedItem[i].classList.remove("highlight");
            }
        }

        $(this)[0].classList.add('highlight');
        var NewRowprocesses = "NoCurrentProcess";
        if ($(this)[0]?.children[7]?.childNodes[0]?.data != undefined && $(this)[0]?.children[7]?.childNodes[0]?.data != null) {
            NewRowprocesses = $(this)[0].children[7].childNodes[0].data;
        }
        var isproviderReviewMyQNewRow = NewRowprocesses;
        if (isproviderReviewMyQNewRow == "PROVIDER_REVIEW" || isproviderReviewMyQNewRow == "PROVIDER_REVIEW_2") {
            if ($(this)[0].children[0].children[0].checked == false) {
                $(this)[0].children[0].children[0].checked = true;
            }
            else {
                $(this)[0].children[0].children[0].checked = false;
                $(this).removeClass("highlight");
            }
            MyQcheckboxclickAction($(this)[0].children[0].children[0]);
        }
    });

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        if ($('#MyQTable').children().find('.highlight').length > 1) {
            alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable tr').removeClass("highlight");

            if ($('#MovetoNxtProcess') != undefined && $('#MovetoNxtProcess')[0]?.disabled != undefined) {
                $('#MovetoNxtProcess')[0].disabled = true;
            }
            return false;
        }

        if (event.target.tagName != 'TH' && event.target.type != 'checkbox') {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                $(this)[0].children[0].children[0].checked = true;
                $(this)[0].classList.add('highlight');
            }
            MyQclick();
        }
        sessionStorage.setItem('MyQRemoveIdList', '');
    });
}

function MyQBind1(objdata) {
    //MyEncounterLoad
    let disableOverallSelect = true;
    if (objdata.role != "Medical Assistant" && objdata.role != "Front Office" && objdata.role != "Surgery Coordinator" && objdata.role != "Scribe") {
        for (var i = 0; i < objdata.data.length; i++) {
            if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
                disableOverallSelect = false;
            }
        }

        $("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";
        $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.count[0].My_Task_Count + ")";
        $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.count[0].My_Order_Count + ")";
        $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.count[0].My_Scan_Count + ")";
        $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.count[0].My_Presc_Count + ")";
        $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.count[0].My_Amendmnt_Count + ")";

        localStorage.setItem("Myorderscount", objdata.count[0].My_Order_Count);
        if ($('#hdnIsShowAllMyEncountersQueue').val() == 'Y') {
            $('#chkMyShowAll,#lblMyShowAll').css("display", "none");
        } else {
            $('#chkMyShowAll,#lblMyShowAll').css("display", "");
        }
        if (objdata.EncounterCount != null && objdata.EncounterCount != undefined && $('#hdnIsShowAllMyEncountersQueue').val() != 'Y') {
            $("#ctl00_C5POBody_lblcount").css('font-size', '11px');
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = 'Total encounters to be signed are<span style="color:red;"> ' + objdata.EncounterCount + '</span>. To view current as well as more than 21 days old encounters, click on "ShowAll".'
        }
        else
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";

        $("#btnMyEnc").removeClass("default");
        $("#btnMyEnc").addClass("btncolorMyQ");
        $("#btnMyQ").removeClass("default");
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#MovetoNxtProcess").css("display", "inline-block");
        if (disableOverallSelect) {
            disableSelectAllMove();
        }
    }
    sessionStorage.setItem("My_Task_Count", objdata.count[0].My_Task_Count);
    sessionStorage.setItem("My_Order_Count", objdata.count[0].My_Order_Count);
    sessionStorage.setItem("My_Scan_Count", objdata.count[0].My_Scan_Count);
    sessionStorage.setItem("My_Presc_Count", objdata.count[0].My_Presc_Count);
    sessionStorage.setItem("My_Amendmnt_Count", objdata.count[0].My_Amendmnt_Count);
    sessionStorage.setItem("Order_Count", objdata.count[0].Order_Count);
    sessionStorage.setItem("Amendmnt_Count", objdata.count[0].Amendmnt_Count);
    sessionStorage.setItem("Task_Count", objdata.count[0].Task_Count);
}

function MyQBind2(objdata) {
    //EncounterLoad
    let disableOverallSelect = true;
    for (var i = 0; i < objdata.data.length; i++) {
        if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
            disableOverallSelect = false;
        }
    }
    $("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";
    $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.count[0].My_Task_Count + ")";
    $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.count[0].My_Order_Count + ")";
    $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.count[0].My_Scan_Count + ")";
    $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.count[0].My_Presc_Count + ")";
    $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.count[0].My_Amendmnt_Count + ")";

    sessionStorage.setItem("My_Task_Count", objdata.count[0].My_Task_Count);
    sessionStorage.setItem("My_Order_Count", objdata.count[0].My_Order_Count);
    sessionStorage.setItem("My_Scan_Count", objdata.count[0].My_Scan_Count);
    sessionStorage.setItem("My_Presc_Count", objdata.count[0].My_Presc_Count);
    sessionStorage.setItem("My_Amendmnt_Count", objdata.count[0].My_Amendmnt_Count);

    localStorage.setItem("Myorderscount", objdata.count[0].My_Order_Count);
    if (objdata.EncounterCount != null && objdata.EncounterCount != undefined && $('#hdnIsShowAllMyEncountersQueue').val() != 'Y') {
        $("#ctl00_C5POBody_lblcount").css('font-size', '11px');
        $("#ctl00_C5POBody_lblcount")[0].innerHTML = 'Total encounters to be signed are <span style="color:red;">' + objdata.EncounterCount + '</span>. To view current as well as more than 21 days old encounters, click on "ShowAll".'
    }
    else
        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    $("#btnMyEnc").removeClass("default");
    $("#btnMyEnc").addClass("btncolorMyQ");
    $("#btnMyQ").removeClass("default");
    $("#btnMyQ").addClass("btncolorMyQ");
    if (disableOverallSelect) {
        disableSelectAllMove();
    }
}

function MyQBind3(objdata) {
    //chkShowAllMyEncounter
    let disableOverallSelect = true;
    for (var i = 0; i < objdata.data.length; i++) {
        if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
            disableOverallSelect = false;
        }
    }
    $("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";
    if (sessionStorage.getItem("My_Task_Count") != null && sessionStorage.getItem("My_Task_Count") != undefined) {
        $("#btnMyTask")[0].innerText = "My Tasks " + "(" + sessionStorage.getItem("My_Task_Count") + ")";
    }
    if (sessionStorage.getItem("My_Order_Count") != null && sessionStorage.getItem("My_Order_Count") != undefined) {
        $("#btnMyOrder")[0].innerText = "My Orders " + "(" + sessionStorage.getItem("My_Order_Count") + ")";
    }
    if (sessionStorage.getItem("My_Scan_Count") != null && sessionStorage.getItem("My_Scan_Count") != undefined) {
        $("#btnMyScan")[0].innerText = "My Scan " + "(" + sessionStorage.getItem("My_Scan_Count") + ")";
    }
    if (sessionStorage.getItem("My_Presc_Count") != null && sessionStorage.getItem("My_Presc_Count") != undefined) {
        $("#btnMyPres")[0].innerText = "My Prescription " + "(" + sessionStorage.getItem("My_Presc_Count") + ")";
    }
    if (sessionStorage.getItem("My_Amendmnt_Count") != null && sessionStorage.getItem("My_Amendmnt_Count") != undefined) {
        $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + sessionStorage.getItem("My_Amendmnt_Count") + ")";
    }

    if (objdata.EncounterCount != null && objdata.EncounterCount != undefined && $('#hdnIsShowAllMyEncountersQueue').val() != 'Y') {
        $("#ctl00_C5POBody_lblcount").css('font-size', '11px');
        $("#ctl00_C5POBody_lblcount")[0].innerHTML = 'Total encounters to be signed are <span style="color:red;">' + objdata.EncounterCount + '</span>. To view current as well as more than 21 days old encounters, click on "ShowAll".'
    }
    else
        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    if (disableOverallSelect) {
        disableSelectAllMove();
    }
}

function loadMytask() {
    var myOpenTask = localStorage.getItem('MyOpenTask');
    if (myOpenTask == "Checked") {
        $("#chkOpenTask")[0].checked = true;
        $("#chkMyTask14")[0].checked = false;
        $("#chkMyTask14")[0].disabled = true;
    } else {
        $("#chkOpenTask")[0].checked = false;
    }

    var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";

    var url = "LoadMyTask";
    var data = JSON.stringify({ "sShowall": Showall, "sOpenTask": OpenTask });
    var myTask14 = localStorage.getItem('MyTask14');
    if (myTask14 == "Checked") {
        $("#chkMyTask14")[0].checked = true;
        $("#chkMyShowAll")[0].checked = false;
        $("#chkOpenTask")[0].checked = false;
        $("#chkMyShowAll")[0].disabled = true;
        $("#chkOpenTask")[0].disabled = true;
        url = "LoadMyTaskCompleted";
        data = JSON.stringify({ "sShowall": Showall });
        Showall = "";
    }

    var showallchecked = localStorage.getItem('MyShowAllMyTask');
//Jira #CAP-1051 
    //if (showallchecked == "Checked") {
    //    Showall = "Checked";
    //    $("#chkMyShowAll")[0].checked = true;
    //    $("#chkMyTask14")[0].checked = false;
    //    $("#chkMyTask14")[0].disabled = true;
    //} else {
    //    Showall = "Unchecked"
    //}
    if ($("#chkMyShowAll")[0].checked == true) {
        Showall = "Checked";
        $("#chkMyShowAll")[0].checked = true;
        $("#chkMyTask14")[0].checked = false;
        $("#chkMyTask14")[0].disabled = true;
    }
    else {
        Showall = "Unchecked"
    }
    //Jira #CAP-912
    data = JSON.stringify({ "sShowall": Showall, "sOpenTask": OpenTask });
    LoadMyTask();
    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/" + url,
    //    data: data,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#MyQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);
    //        if (data.d != "[]") {
    //            for (var i = 0; i < objdata.length; i++) {
    //                if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
    //                    Msg_Date_And_Time = "";
    //                else
    //                    Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
    //                if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
    //                    Modified_Date_Time = "";
    //                else
    //                    Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
    //                if (i == 0) {
    //                    //Jira #CAP-119
    //                    //tabContents = "<tr  style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                    tabContents = "<tr  style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '').replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                }
    //                else {
    //                    //Jira #CAP-119
    //                    //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                    tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '').replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                }
    //            }
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-break: break-all;' >" + tabContents + "</tbody></table>");
    //        }
    //        else {
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'  ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
    //        }
    //        //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)"; 
    //        $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";

    //        if (Showall != "Checked") {
    //            sessionStorage.setItem("My_Task_Count", objdata.length);
    //        }

    //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    //        SortTableHeader('MyQTask');
    //        //$('#EncounterTable th').addClass('header');
    //        RowClick();
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            //CAP-792
    //            if (isValidJSON(xhr.responseText)) {
    //                var log = JSON.parse(xhr.responseText);
    //                console.log(log);
    //                alert("USER MESSAGE:\n" +
    //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                    "Message: " + log.Message);
    //            }
    //            else {
    //                alert("USER MESSAGE:\n" +
    //                    ". Cannot process request. Please Login again and retry.");
    //            }
    //        }
    //    }
    //});

}

function LoadMyTask() {

    if ($('#hdnIsShowAllMyTasksQueue').val() == 'Y') {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "none");
        $("#chkMyShowAll").prop('checked', true);
    } else {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "");
    }

    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $("#MyQTable").append(`
    <table id="EncounterTable" class='table table-bordered Gridbodystyle' style=''>
    <thead class='header' style='border: 0px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th>
    <th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th>
    <th style='border: 1px solid #909090;display:none;'>TaskID</th>
    <th style='border: 1px solid #909090;display:none;'>Version</th>
    </tr>
    </thead>
</table>`);
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    if ($("#chkMyTask14")[0].checked) {
        var url = "LoadMyTaskCompleted";
    }
    else {
        var url = "LoadMyTask";
    }
    var Last14 = $("#chkMyTask14")[0].checked ? "Checked" : "Unchecked";
    var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";
    data = JSON.stringify({ "sShowall": Showall, "sOpenTask": OpenTask });
    var titleval;
    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        processing: false,
        scrollCollapse: true,
        scrollY: '420px',
        ordering: true,
        autowidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. #",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top

        ajax: {
            url: '/frmMyQueueNew.aspx/' + url,
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = data;
                return d;
            },
            dataSrc: function (json) {
                //var objdata = json.d;
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);
                $("#btnMyTask")[0].innerText = "My Tasks  " + "(" + objdata.data.length + ")";
                if (Showall != "Checked") {
                    sessionStorage.setItem("My_Task_Count", objdata.data.length);
                }
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                //return objdata;
                json.data = objdata.data;
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            { data: 'Priority', searchable: false, sWidth:'6%' },
            { data: 'Human_ID', sWidth: '7%' },
            {
                data: 'Last_Name', render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                },
                sClass: 'word-break-all', sWidth: '10%'
            },
            {
                data: 'Msg_Date_And_Time', render: function (data, type, row) {
                    if (row.Msg_Date_And_Time == "0001-01-01T00:00:00")
                        return "";
                    else
                        return ConvertDate(row.Msg_Date_And_Time.replace("T", " ")).split(' ')[0];
                }, searchable: false,
                type: 'date', sWidth: '11%'
            },
            {
                data: 'Message_Description', render: function (data, type, row) {
                    if (row.Message_Notes != "") {
                        titleval = row.Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, "").replace(/'/g, "");
                    }
                    else {
                        titleval = "";
                    }
                    return `<span title="${titleval}">${row.Message_Description}</span>`;
                }, searchable: false, sWidth: '11%'
            },
            { data: 'Assigned_To', searchable: false, sWidth: '11%' },
            { data: 'Created_By', searchable: false, sWidth: '11%' },
            {
                data: 'Modified_Date_Time', render: function (data, type, row) {
                    //CAP-3822
                    if (row.Modified_Date_Time == "0001-01-01T00:00:00" || !$("#chkMyTask14")[0]?.checked)
                        return "";
                    else
                        return ConvertDate(row.Modified_Date_Time.replace("T", " "));
                }, searchable: false,
                type: 'date', sWidth: '11%'
            },
            { data: 'Message_ID', sClass: "hide_column", searchable: false },
            { data: 'Version', sClass: "hide_column", searchable: false },
        ],
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }

    });
    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
        'width': '500px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });
   
    //$('#EncounterTable_filter input').unbind();

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        dataTable.$('tr.highlight').removeClass('highlight');
        $(this)[0].classList.add('highlight');
        if ($('#EncounterTable tr.highlight')[0].innerText == "No data available in table") {
            return false;
        } else {
            MyQclick();
        }
        sessionStorage.setItem('MyQRemoveIdList', '');
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        dataTable.$('tr.highlight').removeClass('highlight');
        $(this)[0].classList.add('highlight');
    });
}

function loadMyorder() {
    if ($('#hdnIsShowAllMyOrdersQueue').val() == 'Y') {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "none");
    } else {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "");
    }
    if ($('#hdnMyOrdersQueueVersion').val() == 'V2') {
        $('#cboYears').css("display", "");
    }
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $("#MyQTable").append(`
    <table id="EncounterTable" class="table table-bordered Gridbodystyle" style="table-layout: fixed;">
    <thead class="header" style="border: 0px;width:96.7%;">
        <tr class="header">
            <th style="border: 1px solid #909090;text-align: center;width:9%" title="Click here to sort">Order Date</th>
            <th style="border: 1px solid #909090;display:none;">Test Date</th>
            <th style="border: 1px solid #909090;text-align: center;width:6%;" title="Click here to sort">Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;width:6%" title="Click here to sort">Ext. Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;width:10%" title="Click here to sort">Patient Name</th>
            <th style="border: 1px solid #909090;text-align: center;width:8%" title="Click here to sort">Patient DOB</th>
            <th style="border: 1px solid #909090;text-align: center;width:10%" title="Click here to  sort">Description</th>
            <th style="border: 1px solid #909090;text-align: center;width:10%" title="Click here to sort">Ordering Physician</th>
            <th style="border: 1px solid #909090;text-align: center;width:10%" title="Click here to sort">Current Process</th>
            <th style="border: 1px solid #909090;text-align: center;width:10%" title="Click here to sort">Lab/Referred to</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">Lab Location</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">Encounter_ID</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">Physician_ID</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">Order_ID</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">ObjType</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">LabID</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">LocationID</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">Order_Submit_ID</th>
            <th style="border: 1px solid #909090;display:none;text-align: center;" title="Click here to sort">Referred to Facility</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">Result_Master_ID</th>
            <th style="border: 1px solid #909090;display:none;" title="Click here to sort">File_Reference_No</th>
            <th style="border: 1px solid #909090;text-align: center;width:10%" title="Click here to sort">Narrative Interpretation</th>
            <th style="border: 1px solid #909090;text-align: center;width:4%" title="Click here to sort">Abnormal</th>
        </tr>
    </thead>
    </table>`);

    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        scrollCollapse: true,
        scrollY: '420px',
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. #",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmMyQueueNew.aspx/LoadMyOrder',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({ Showall: Showall, Year: $("#cboYears").val() });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);
                if ($('#hdnMyOrdersQueueVersion').val() == 'V2') {
                    var year = $("#cboYears").val();
                    $("#cboYears").empty();
                    $.each(objdata.yearList, function (index, value) {
                        if (year == value) {
                            $("#cboYears").append($("<option selected></option>").val(value).text(value));
                        } else {
                            $("#cboYears").append($("<option></option>").val(value).text(value));
                        }
                    });
                }
                
                $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.data.length + ")";
                if (Showall != "Checked") {
                    sessionStorage.setItem("My_Order_Count", objdata.data.length);
                }
                localStorage.setItem("Myorderscount", objdata.data.length);
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "Note:All abnormal results are in <span style='color:red'> RED</span> color font.";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return objdata.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: 'Created_Date_And_Time', render: function (data, type, row) {
                    return ConvertDate(data.replace("T", " "));
                },
                searchable: false,
                type: 'date', sWidth:'9%'
            },
            { data: 'Test_Date', searchable: false, sClass: 'hide_column' },
            { data: 'Human_ID', sWidth: '6%' },
            { data: 'External_Account_Number', searchable: false, sWidth: '6%' },
            {
                data: 'Last_Name', render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                },
                sClass: 'word-break-all', sWidth: '10%'
            },
            {
                data: 'DOB', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                },
                searchable: false,
                type: 'date', sWidth: '8%'
            },
            {
                data: 'Reason_For_Referral', render: function (data, type, row) {
                    var description = "";
                    if (row.Reason_For_Referral != "") {
                        if (row.Referred_to != "") {
                            if (row.Is_Abnormal == "Yes")
                                description = row.Reason_For_Referral
                            else
                                description = row.Reason_For_Referral
                        }
                        else {
                            if (row.Is_Abnormal == "Yes")
                                description = row.Reason_For_Referral
                            else
                                description = row.Reason_For_Referral
                        }
                    }
                    else {
                        if (row.Referred_to != "") {
                            if (row.Is_Abnormal == "Yes")
                                description = row.Procedure_Ordered
                            else
                                description = row.Procedure_Ordered
                        }
                        else {
                            if (row.Is_Abnormal == "Yes")
                                description = row.Procedure_Ordered
                            else
                                description = row.Procedure_Ordered
                        }
                    }
                    return description;
                },
                searchable: false, sWidth: '10%'
            },
            { data: 'PhyName', searchable: false, sWidth: '10%' },
            { data: 'Current_Process', searchable: false, sClass: 'process-word-wrap', sWidth: '10%' },
            {
                data: 'Referred_to', render: function (data, type, row) {
                    var referredTo = "";
                    if (row.Referred_to != "") {
                        if (row.Is_Abnormal == "Yes")
                            referredTo = row.Referred_to
                        else
                            referredTo = row.Referred_to
                    }
                    else {
                        if (row.Is_Abnormal == "Yes")
                            referredTo = row.Lab_Name
                        else
                            referredTo = row.Lab_Name
                    }
                    return referredTo;
                },
                searchable: false, sWidth: '10%'
            },
            { data: 'Lab_Loc_Name', sClass: 'hide_column', searchable: false },
            { data: 'Encounter_ID', sClass: 'hide_column', searchable: false },
            { data: 'Physician_ID', sClass: 'hide_column', searchable: false },
            { data: 'Order_ID', sClass: 'hide_column', searchable: false },
            { data: 'EHR_Obj_Type', sClass: 'hide_column', searchable: false },
            { data: 'Lab_ID', sClass: 'hide_column', searchable: false },
            { data: 'Lab_Location_ID', sClass: 'hide_column', searchable: false },
            { data: 'Order_Submit_ID', sClass: 'hide_column', searchable: false },
            { data: 'Referred_to_Facility', sClass: 'hide_column', searchable: false },
            { data: 'ResultMasterID', sClass: 'hide_column', searchable: false },
            { data: 'File_Reference_No', sClass: 'hide_column', searchable: false },
            { data: 'Is_Narrative', searchable: false, sWidth: '10%' },
            { data: 'Is_Abnormal', render: function (data, type, row) { if (data != "") { return data.toUpperCase(); } else { return "NO"; } }, searchable: false, sWidth: '4%' },
        ],
        createdRow: function (row, data, dataIndex) {
            if (data.Is_Abnormal == "Yes") {
                $(row).css('color', 'red');
            }
        },
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }
    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        var existingSelectedItem = $("#MyQTable tr.highlight");
        for (var i = 0; i < existingSelectedItem.length; i++) {
            var processes = "NoCurrentProcess";
            if (existingSelectedItem[i]?.children[8]?.childNodes[0]?.data != undefined && existingSelectedItem[i]?.children[8]?.childNodes[0]?.data != null) {
                processes = existingSelectedItem[i].children[8].childNodes[0].data;
            }
            var isproviderReviewMyQ = processes;
            if (isproviderReviewMyQ != "PROVIDER_REVIEW" && isproviderReviewMyQ != "PROVIDER_REVIEW_2") {
                existingSelectedItem[i].classList.remove("highlight");
            }
        }

        $(this)[0].classList.add('highlight');
        var NewRowprocesses = "NoCurrentProcess";
        if ($(this)[0]?.children[8]?.childNodes[0]?.data != undefined && $(this)[0]?.children[8]?.childNodes[0]?.data != null) {
            NewRowprocesses = $(this)[0].children[8].childNodes[0].data;
        }
        var isproviderReviewMyQNewRow = NewRowprocesses;
        if (isproviderReviewMyQNewRow == "PROVIDER_REVIEW" || isproviderReviewMyQNewRow == "PROVIDER_REVIEW_2") {
            if ($(this)[0].children[0].children[0].checked == false) {
                $(this)[0].children[0].children[0].checked = true;
            }
            else {
                $(this)[0].children[0].children[0].checked = false;
                $(this).removeClass("highlight");
            }
            MyQcheckboxclickAction($(this)[0].children[0].children[0]);
        }
    });

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        if ($('#MyQTable').children().find('.highlight').length > 1) {
            alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable tr').removeClass("highlight");

            if ($('#MovetoNxtProcess') != undefined && $('#MovetoNxtProcess')[0]?.disabled != undefined) {
                $('#MovetoNxtProcess')[0].disabled = true;
            }
            return false;
        }

        if (event.target.tagName != 'TH' && event.target.type != 'checkbox') {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                $(this)[0].children[0].children[0].checked = true;
                $(this)[0].classList.add('highlight');
            }
            MyQclick();
        }
        sessionStorage.setItem('MyQRemoveIdList', '');
    });

    $("#cboYears").change(function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        dataTable.ajax.reload();
    });
}
function loadMyscan() {
    if ($("#hdnIsShowAllMyScanQueue")[0].value == "Y") {
        Showall = "Checked";
        document.getElementById("chkMyShowAll").style.display = "none";
        $("label[for|='chkMyShowAll']")[0].style.display = "none"
    }
    else {
        $("#chkMyShowAll")[0].disabled = false;
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

    }
    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/LoadMyScan",
    //    data: JSON.stringify({
    //        "sShowall": Showall,
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#MyQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);
    //        if (data.d != "[]") {
    //            for (var i = 0; i < objdata.length; i++) {
    //                if (i == 0)
    //                    tabContents = "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + + "</td></tr>";
    //                else
    //                    tabContents = tabContents + "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + "</td></tr>";
    //            }
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //            //Jira #CAP-938
    //            //$("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.length + ")";
    //        }
    //        else
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead></table>");
    //        //Jira #CAP-938
    //        $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.length + ")";

    //        if (Showall != "Checked") {
    //            sessionStorage.setItem("My_Scan_Count", objdata.length);
    //        }
    //        //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
    //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    //        SortTableHeader('MyQScan');
    //        RowClick();
    //        //$('#EncounterTable th').addClass('header');
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});
    $('#MyQTable').empty();
    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style=''><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead></table>");

    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        scrollCollapse: true,
        scrollY: '420px',
        processing: false,
        ordering: true,
        autowidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by File Name",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: "frmMyQueueNew.aspx/LoadMyScan",
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "sShowall": Showall
                });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);

                $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.data.length + ")";

                if (Showall != "Checked") {
                    sessionStorage.setItem("My_Scan_Count", objdata.data.length);
                }
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";

                json.draw = objdata.draw;
                json.recordsTotal = objdata.recordsTotal;
                json.recordsFiltered = objdata.recordsFiltered;
                json.data = objdata.data;


                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


                return json.data;

            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: 'Scanned_File_Name',
                sWidth: '16%',
                sClass: "word-break-all"

            },
            {
                data: 'No_of_Pages', sWidth: '16%', searchable: false
            },
            {
                data: 'Scanned_Date', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    dt1 = ConvertDate(dt1.replaceAll("T", " "));
                    var dt2 = dt1.split(' ');
                    if (dt2.length > 0) {
                        if (dt2.indexOf("01-01-0001") > -1) {
                            return "";
                        }
                    }
                    return dt1;
                }, type: 'date', sWidth: '16%', searchable: false, sClass: "process-word-wrap"
            },
            { data: 'Facility_Name', sWidth: '16%', sClass: "word-break-all", searchable: false },
            { data: 'Current_Process', sWidth: '16%', searchable: false, sClass: 'process-word-wrap' },
            { data: 'Scan_ID', sClass: "hide_column", sWidth: '5%', searchable: false },
            { data: 'Human_ID', sClass: "hide_column", sWidth: '5%', searchable: false }

        ],
        createdRow: function (row, data, dataIndex) {

        },
        "fnDrawCallback": function (oSettings) {
            RowClick();
        },
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }

    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

    $("#EncounterTable thead").click(function () {
        $("#EncounterTable thead tr").removeClass('highlight');
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });



}
function loadMyprescription() {
    if ($('#hdnIsShowAllMyPrescriptionQueue').val() == 'Y') {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "none");
    } else {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "");
    }
    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $("#MyQTable").append(`
    <table id="EncounterTable" class="table table-bordered Gridbodystyle" style="table-layout: fixed;">
    <thead class="header" style="border: 0px;width:96.7%;">
        <tr class="header">
            <th style="border: 1px solid #909090;text-align: center;width:20%">Prescription Date & Time</th>
            <th style="border: 1px solid #909090;text-align: center;width:6%">Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;width:7%">Ext. Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;width:20%">Patient Name</th>
            <th style="border: 1px solid #909090;text-align: center;width:20%">Patient DOB</th>
            <th style="border: 1px solid #909090;text-align: center;width:20%">Current Process</th>
            <th style="border: 1px solid #909090;display:none;">EncounterID</th>
            <th style="border: 1px solid #909090;display:none;">PrescriptionID</th>
            <th style="border: 1px solid #909090;display:none;">ObjType</th>
        </tr>
    </thead>
</table>`);
    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        scrollCollapse: true,
        scrollY: '420px',
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. #",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmMyQueueNew.aspx/LoadMyPrescription',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = Showall;
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);

                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.data.length + ")";
                if (Showall != "Checked") {
                    sessionStorage.setItem("My_Presc_Count", objdata.data.length);
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                json.data = objdata.data;
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: 'Prescription_Date', render: function (data, type, row) {
                    return ConvertDate(data.replace("T", " "));
                },
                searchable: false,
                type: 'date', sWidth:'20%'
            },
            { data: 'Human_ID', sWidth: '6%' },
            { data: 'External_Account_Number', searchable: false, sWidth: '7%' },
            {
                data: 'Last_Name', render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                },
                sClass: 'word-break-all', sWidth: '20%'
            },
            {
                data: 'DOB', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                },
                searchable: false,
                type: 'date', sWidth: '20%'
            },
            { data: 'Current_Process', searchable: false, sClass: 'process-word-wrap', sWidth: '20%' },
            { data: 'Encounter_ID', sClass: 'hide_column', searchable: false },
            { data: 'Prescription_Id', sClass: 'hide_column', searchable: false },
            { data: 'EHR_Obj_Type', sClass: 'hide_column', searchable: false },
        ],
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }
    });
    
    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        var existingSelectedItem = $("#MyQTable tr.highlight");
        for (var i = 0; i < existingSelectedItem.length; i++) {
            var processes = "NoCurrentProcess";
            if (existingSelectedItem[i]?.children[5]?.childNodes[0]?.data != undefined && existingSelectedItem[i]?.children[5]?.childNodes[0]?.data != null) {
                processes = existingSelectedItem[i].children[5].childNodes[0].data;
            }
            var isproviderReviewMyQ = processes;
            if (isproviderReviewMyQ != "PROVIDER_REVIEW" && isproviderReviewMyQ != "PROVIDER_REVIEW_2") {
                existingSelectedItem[i].classList.remove("highlight");
            }
        }

        $(this)[0].classList.add('highlight');
        var NewRowprocesses = "NoCurrentProcess";
        if ($(this)[0]?.children[5]?.childNodes[0]?.data != undefined && $(this)[0]?.children[5]?.childNodes[0]?.data != null) {
            NewRowprocesses = $(this)[0].children[5].childNodes[0].data;
        }
        var isproviderReviewMyQNewRow = NewRowprocesses;
        if (isproviderReviewMyQNewRow == "PROVIDER_REVIEW" || isproviderReviewMyQNewRow == "PROVIDER_REVIEW_2") {
            if ($(this)[0].children[0].children[0].checked == false) {
                $(this)[0].children[0].children[0].checked = true;
            }
            else {
                $(this)[0].children[0].children[0].checked = false;
                $(this).removeClass("highlight");
            }
            MyQcheckboxclickAction($(this)[0].children[0].children[0]);
        }
    });

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        if ($('#MyQTable').children().find('.highlight').length > 1) {
            alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable tr').removeClass("highlight");

            if ($('#MovetoNxtProcess') != undefined && $('#MovetoNxtProcess')[0]?.disabled != undefined) {
                $('#MovetoNxtProcess')[0].disabled = true;
            }
            return false;
        }

        if (event.target.tagName != 'TH' && event.target.type != 'checkbox') {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                $(this)[0].children[0].children[0].checked = true;
                $(this)[0].classList.add('highlight');
            }
            MyQclick();
        }
        sessionStorage.setItem('MyQRemoveIdList', '');
    });
}
function loadMyAmendment() {

    if ($('#hdnIsShowAllMyAmendmentQueue').val() == 'Y') {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "none");
        $("#chkMyShowAll").prop('checked', true);
    } else {
        $('#chkMyShowAll,#lblMyShowAll').css("display", "");
    }

    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

    $('#GeneralQTable').empty();
    $("#MyQTable").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 0px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th>
    <th style='border: 1px solid #909090;display:none;'>EncounterID</th>
    <th style='border: 1px solid #909090;display:none;'>PhysicianID</th>
    <th style='border: 1px solid #909090;display:none;'>ObjType</th>
    <th style='border: 1px solid #909090;display:none;'>AddendumID</th>
    <th style='border: 1px solid #909090;display:none;'>Current Owner</th>
    </tr></thead>
    </table>`);
    data = JSON.stringify({ "sShowall": Showall });
    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        scrollCollapse: true,
        scrollY: '420px',
        processing: false,
        ordering: true,
        autowidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. #",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top
        ajax: {
            url: '/frmMyQueueNew.aspx/LoadMyAmendment',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = Showall;
                return d;
            },
            dataSrc: function (json) {
                //var objdata = json.d;
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);
                tempObjectMyQTask = objdata.data;
                $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.data.length + ")";
                if (Showall != "Checked") {
                    sessionStorage.setItem("My_Amendmnt_Count", objdata.data.length);
                }
                localStorage.setItem("Myorderscount", objdata.data.length);
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return objdata.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: 'Appt_Date_Time', render: function (data, type, row) {
                    if (row.Appt_Date_Time == "0001-01-01T00:00:00")
                        return "";
                    else
                        return ConvertDate(row.Appt_Date_Time.replace("T", " "));
                }, searchable: false,
                type: 'date'
            },
            {
                data: 'Addendum_Created_Date_Time', render: function (data, type, row) {
                    if (row.Addendum_Created_Date_Time == "0001-01-01T00:00:00")
                        return "";
                    else
                        return ConvertDate(row.Addendum_Created_Date_Time.replace("T", " "));
                }, searchable: false,
                type: 'date'
            },
            { data: 'Human_ID' },
            { data: 'External_Account_Number', searchable: false },
            {
                data: 'Last_Name', render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                },
                sClass: 'word-break-all'
            },
            { data: 'Current_Process', searchable: false, sClass: 'process-word-wrap' },
            {
                data: 'Addendum_Created_Date_Time', render: function (data, type, row) {
                    if (row.Addendum_Created_Date_Time == "0001-01-01T00:00:00")
                        return "";
                    else
                        return ConvertDate(row.Addendum_Created_Date_Time.replace("T", " "));
                }, searchable: false,
                type: 'date'
            },
            { data: 'Addendum_Created_By', searchable: false },
            { data: 'Addendum_Signed_By', searchable: false },
            { data: 'Encounter_ID', sClass: "hide_column", searchable: false },
            { data: 'Physician_ID', sClass: "hide_column", searchable: false },
            { data: 'EHR_Obj_Type', sClass: "hide_column", searchable: false },
            { data: 'Addendum_ID', sClass: "hide_column", searchable: false },
            { data: 'Current_Owner', sClass: "hide_column", searchable: false },
        ],
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }

    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
        'width': '500px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });
    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });

    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });
    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        dataTable.$('tr.highlight').removeClass('highlight');
        $(this).addClass("highlight");
        if ($('#EncounterTable tr.highlight')[0].innerText == "No data available in table") {
            return false;
        } else {
            MyQclick();
        }
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        dataTable.$('tr.highlight').removeClass('highlight');
        $(this).addClass("highlight");
    });

    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/LoadMyAmendment",
    //    data: JSON.stringify({
    //        "sShowall": Showall,
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#MyQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);
    //        if (data.d != "[]") {
    //            for (var i = 0; i < objdata.length; i++) {
    //                if (i == 0) {
    //                    tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
    //                } else {
    //                    tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
    //                }
    //            }
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //        }
    //        else
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");

    //        //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
    //        $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.length + ")";
    //        if (Showall != "Checked") {
    //            sessionStorage.setItem("My_Amendmnt_Count", objdata.length);
    //        }
    //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    //        SortTableHeader('MyQAmendment');
    //        //$('#EncounterTable th').addClass('header');
    //        RowClick();
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});

}
function LoadGeneralEncounter(ajaxUrl) {
    if ($('#hdnIsShowAllGeneralEncountersQueue').val() == 'Y') {
        $('#lblShowAll,#chkShowAll').css("display", "none");
    } else {
        $('#lblShowAll,#chkShowAll').css("display", "");
    }
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $("#GeneralQTable").append(`
    <table id="EncounterTable" class="table table-bordered Gridbodystyle" style="table-layout: fixed;">
    <thead class="header" style="border: 0px;width:96.7%;">
        <tr class="header">
            <th style="border: 1px solid #909090;text-align: center;">Select<input type="checkbox" onclick="selectAll(this)" class="myQChkbxAll" /></th>
            <th style="border: 1px solid #909090;text-align: center;">Appt. Date & Time</th>
            <th style="border: 1px solid #909090;text-align: center;">Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;">Ext. Acct. #</th>
            <th style="border: 1px solid #909090;text-align: center;">Patient Name</th>
            <th style="border: 1px solid #909090;text-align: center;">Patient DOB</th>
            <th style="border: 1px solid #909090;text-align: center;">Type of Visit</th>
            <th style="border: 1px solid #909090;text-align: center;">Current Process</th>
            <th style="border: 1px solid #909090;text-align: center;">Facility Name</th>
            <th style="border: 1px solid #909090;text-align: center;">Assigned Physician</th>
            <th style="border: 1px solid #909090;text-align: center;">Pri. Carrier</th>
            <th style="border: 1px solid #909090;text-align: center;">Pri. Plan</th>
            <th style="border: 1px solid #909090;text-align: center;">eSuperbill Status</th>
            <th style="border: 1px solid #909090;text-align: center;">Encounter ID</th>
            <th style="display:none;">Physician_ID</th>
            <th style="display:none;">EHR_Obj_Type</th>
            <th style="display:none;">Date_of_Service</th>
        </tr>
    </thead>
    </table>`);

    $("#chkMyShowAll")[0].disabled = false;
    var sShowall = '';
    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    if (MyShowAll == "Checked") {
        sShowall = MyShowAll;
        $("#chkShowAll")[0].checked = true;
    }
    else {
        sShowall = "Unchecked";
        $("#chkShowAll")[0].checked = false;
    }
    var ViewAllFacilities = "";
    if ($("#ctl00_C5POBody_chkViewAllFacilities")[0] != undefined && $("#ctl00_C5POBody_chkViewAllFacilities")[0] != null) {
        $("#ctl00_C5POBody_chkViewAllFacilities")[0].checked ? ViewAllFacilities = "Checked" : ViewAllFacilities = "Unchecked";
    }
    ajaxUrl = ajaxUrl == '' || ajaxUrl == undefined ? 'LoadEncounterTabClick' : ajaxUrl;
    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        scrollCollapse: true,
        scrollY: '413px',
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. # or Encounter ID",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmMyQueueNew.aspx/' + ajaxUrl,
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "sShowall": sShowall,
                    "sViewAllFacilities": ViewAllFacilities
                });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);
                if (objdata.role == 'Medical Assistant' || objdata.role == 'Physician' || objdata.role == 'Coder' || objdata.role == 'Office Manager') {
                    $('#Processenc').css("background-color", "");
                    $('#Processenc')[0].style.display = "inline-block";
                    $('#Processenc')[0].style.visibility = "visible";
                }
                $("#btnEnc")[0].innerText = "Encounters Q " + "(" + objdata.data.length + ")";
                if (ajaxUrl == 'LoadEncounter') {
                    $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.count[0].Order_Count + ")";
                    $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.count[0].Amendmnt_Count + ")";
                    $("#btnTask")[0].innerText = "Tasks Q " + "(" + objdata.count[0].Task_Count + ")";

                    sessionStorage.setItem("Order_Count", objdata.count[0].Order_Count);
                    sessionStorage.setItem("Amendmnt_Count", objdata.count[0].Amendmnt_Count);
                    sessionStorage.setItem("Task_Count", objdata.count[0].Task_Count);
                    localStorage.setItem("GenralOrderCount", objdata.count[0].Order_Count);

                    $("#btnEnc").addClass("btncolorMyQ");
                    $("#btnGeneral").addClass("btncolorMyQ");
                    if (objdata.dataEroom.length > 0) {
                        if ($('select#Exam option').length == 0) { $.each(objdata.dataEroom, function (i, item) { $('#Exam').append($('<option>', { value: objdata.dataEroom[i], text: objdata.dataEroom[i] })); }); }
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                json.data = objdata.data;
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: '', render: function (data, type, row) {
                    return "<input type='checkbox' class='myQChkbx' onclick='checkboxclick(this)' />";
                },
                sClass: "text-align-center",
                searchable: false,
                orderable: false,
                sWidth: '4%',
            },
            {
                data: 'Appt_Date_Time', render: function (data, type, row) {
                    return ConvertDate(data.replace("T", " "));
                },
                searchable: false,
                type: 'date',
                sWidth: '11.5%'
            },
            { data: 'Human_ID', sWidth: '6%' },
            { data: 'External_Account_Number', searchable: false, sWidth: '7%' },
            {
                data: 'Last_Name', render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                },
                sClass: 'word-break-all process-word-wrap',
                sWidth: '10%'
            },
            {
                data: 'DOB', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                },
                searchable: false,
                type: 'date',
                sWidth: '6.7%'
            },
            { data: 'Type_Of_Visit', searchable: false, sWidth: '8%', sClass: ' word-break-all' },
            { data: 'Current_Process', searchable: false, sWidth: '11%', sClass: 'process-word-wrap' },
            {
                data: 'Facility_Name', searchable: false, sWidth: '8%'
            },
            { data: 'PhyName', searchable: false, sWidth: '12%' },
            { data: 'Carrier_Name', searchable: false, sWidth: '7%' },
            { data: 'Insurance_Plan_Name', searchable: false, sWidth: '7%' },
            {
                data: 'Is_EandM_Submitted', render: function (data, type, row) {
                    return data.toUpperCase() == 'Y' ? "Submitted" : "Not Submitted";
                },
                searchable: false,
                sWidth: '9%'
            },
            { data: 'Encounter_ID', sWidth: '7%' },
            { data: 'Physician_ID', sClass: 'hide_column', searchable: false },
            { data: 'EHR_Obj_Type', sClass: 'hide_column', searchable: false },
            { data: 'Date_of_Service', sClass: 'hide_column', searchable: false },
        ],
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }
    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        $('.myQChkbx,.myQChkbxAll').prop('checked', false);
    });

    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        $('.myQChkbx,.myQChkbxAll').prop('checked', false);
    });

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        if (event?.target?.tagName != undefined && document.getElementById("RefreshQ")?.innerText?.indexOf("Refresh Encounters Q") != undefined) {
            if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Encounters Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
            else if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Task Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
        }
        sessionStorage.setItem('MyQRemoveIdList', '');
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        if (event.target.tagName != 'INPUT') {
            if ($(this)[0].children[0].children[0].checked) {
                $(this).removeClass("highlight");
                $(this)[0].children[0].children[0].checked = false;
            }
            else {
                $(this).toggleClass("highlight");
                $(this)[0].children[0].children[0].checked = true;
            }
        }
    });
}
function loadTask() {
    $("#chkShowAll")[0].disabled = false;
    var sShowall = '';
    if ($('#hdnIsShowAllGeneralTasksQueue').val() == 'Y') {
        $('#chkShowAll,#lblShowAll').css("display", "none");
        $("#chkShowAll").prop('checked', true);
    } else {
        $('#chkShowAll,#lblShowAll').css("display", "");
    }

    $("#chkShowAll")[0].disabled = false;
    var sShowall = '';
    $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $("#GeneralQTable").append(`
        <table id=EncounterTable class= 'table table-bordered Gridbodystyle' style = 'table-layout: fixed;' >
        <thead class='header' style='border: 0px;width:96.7%;'>
        <tr class='header'>
        <th style='border: 1px solid #909090;text-align: center;width: 1%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='selectAll(this)'/></th>
        <th style='border: 1px solid #909090;text-align: center;width: 7%;'>Priority</th>
        <th style='border: 1px solid #909090;text-align: center;width: 5%;'>Acct. #</th>
        <th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient Name</th>
        <th style='border: 1px solid #909090;text-align: center;width: 7%;'>Message Date</th>
        <th style='border: 1px solid #909090;text-align: center;width: 8%;'>Message Description</th>
        <th style='border: 1px solid #909090;text-align: center;width: 7%;'>Facility Name</th>
        <th style='border: 1px solid #909090;text-align: center;width: 7%;'>Created By</th>
        <th style='border: 1px solid #909090; display: none;'>Message_ID</th></tr>
        </thead>
        </table> `);

    data = JSON.stringify({ "sShowall": Showall });
    var titleval;
    var dataTable =
        new DataTable('#EncounterTable', {
            serverSide: false,
            lengthChange: false,
            searching: true,
            scrollCollapse: true,
            scrollY: '410px',
            processing: false,
            ordering: true,
            autoWidth: false,
            order: [],
            pageLength: 15,
            language: {
                search: "",
                searchPlaceholder: "Search by Name or Acct. #",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top


            ajax: {
                url: '/frmMyQueueNew.aspx/LoadGeneralTask',
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                deferRender: true,
                data: function (d) {
                    d.extra_search = data;
                    return d;
                },
                dataSrc: function (json) {
                    //var objdata = json.d;
                    var objdata = json.d;
                    objdata.data = Decompress(objdata.data);
                    tempObjectMyTask = objdata.data;
                    $("#btnTask")[0].innerText = "Tasks Q " + "(" + objdata.data.length + ")";
                    if (Showall != "Checked") {
                        sessionStorage.setItem("Task_Count", objdata.data.length);
                    }
                    $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    json.data = objdata.data;
                    return json.data;
                },
                error: function (xhr, error, code) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            },
            columns: [
                {
                    data: '', render: function (data, type, row) {
                        return " <td style='width:5%'><input type = 'checkbox' class='myQChkbx' onclick = 'checkboxclick(this)' /></td >";
                    },
                    searchable: false,
                    orderable: false, sWidth:'1%'
                },
                { data: 'Priority', searchable: false, sWidth: '7%' },
                { data: 'Human_ID', sWidth: '5%' },
                {
                    data: 'Last_Name', render: function (data, type, row) {
                        return row.Last_Name + "," + row.First_Name + " " + row.MI;
                    },
                    sClass: 'word-break-all', sWidth: '7%'
                },
                {
                    data: 'Msg_Date_And_Time', render: function (data, type, row) {
                        if (row.Msg_Date_And_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Msg_Date_And_Time.replace("T", " ")).split(' ')[0];
                    }, searchable: false,
                    type: 'date', sWidth: '7%'
                },
                {
                    data: 'Message_Description', render: function (data, type, row) {
                        if (row.Message_Notes != "") {
                            titleval = row.Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, "").replace(/'/g, "");
                        }
                        else {
                            titleval = "";
                        }
                        return `<span title="${titleval}">${row.Message_Description}</span>`;
                    }, searchable: false, sWidth: '8%'
                },
                { data: 'Facility_Name', searchable: false, sWidth: '7%' },
                { data: 'Created_By', searchable: false, sWidth: '7%' },
                { data: 'Message_ID', sClass: "hide_column", searchable: false },
            ],
            initComplete: function (settings, json) {
                $("#EncounterTable_filter input")[0].classList.add('searchicon');
                SetHeightForTabelBasedOnScreenSize();
            }
        });
    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

  
    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        $('.myQChkbx,.myQChkbxAll').prop('checked', false);
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        $('.myQChkbx').prop('checked', false);
    });

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        $('#EncounterTable tr').removeClass("highlight");
        $(this)[0].classList.add('highlight');
        if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Task Q") > -1) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                $(this)[0].children[0].children[0].checked = true;
                checkboxclick($(this)[0].children[0].children[0]);
            }
            $('#Processenc').click();
        }

        sessionStorage.setItem('MyQRemoveIdList', '');
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        if (event.target.tagName != 'INPUT') {
            if ($(this)[0].children[0].children[0].checked) {
                $(this).removeClass("highlight");
                $(this)[0].children[0].children[0].checked = false;
            }
            else {
                $(this).toggleClass("highlight");
                $(this)[0].children[0].children[0].checked = true;
            }

        }
    });



    //var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    //$("#chkShowAll")[0].checked ? sShowall = "Checked" : sShowall = "Unchecked";
    //if (Showall == "Checked")
    //    sShowall = MyShowAll;
    //else
    //    sShowall = "Unchecked";
    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/LoadGeneralTask",
    //    data: JSON.stringify({
    //        "sShowall": sShowall,
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#GeneralQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);

    //        if (objdata.length > 0) {
    //            var Msg_Date_And_Time = '';
    //            for (var i = 0; i < objdata.length; i++) {

    //                if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
    //                    Msg_Date_And_Time = "";
    //                else
    //                    Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
                    

    //                if (i == 0)
    //                    tabContents = "<tr><td style='width:2%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:7%'>" + objdata[i].Priority + "</td><td style='width:5%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:7%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:7%'>" + objdata[i].Facility_Name + "</td><td style='width:7%'>" + objdata[i].Created_By + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td></tr>";
    //                else
    //                    tabContents = tabContents + "<tr><td style='width:2%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:7%'>" + objdata[i].Priority + "</td><td style='width:5%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:7%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:7%'>" + objdata[i].Facility_Name + "</td><td style='width:7%'>" + objdata[i].Created_By + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td></tr>";
    //            }

    //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 2%;'>Select<input type='checkbox' onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Priority</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Created By</th><th style='border: 1px solid #909090; display: none;'>Message_ID</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //        }
    //        else {
    //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 2%;'>Select<input type='checkbox' onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Priority</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Created By</th><th style='border: 1px solid #909090; display: none;'>Message_ID</th></tr></thead></table>");
    //        }
    //        $("#btnTask")[0].innerText = "Tasks Q " + "(" + objdata.length + ")";
    //        if (sShowall != "Checked") {
    //            sessionStorage.setItem("Task_Count", objdata.length);
    //        }

    //        SortTableHeader('GeneralQTask');
    //        RowClick();
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});

}
function loadorder() {

    //$("#chkMyShowAll")[0].disabled = false;
    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/LoadOrder",
    //    data: JSON.stringify({
    //        "sShowall": "Unchecked",
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#GeneralQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);
    //        if (data.d != null && data.d != "[]") {
    //            for (var i = 0; i < objdata.length; i++) {
    //                var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
    //                if (i == 0) {
    //                    if (objdata[i].Reason_For_Referral != "") {
    //                        if (objdata[i].Referred_to != "")
    //                            //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                        else
    //                            //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                    }
    //                    else {
    //                        if (objdata[i].Referred_to != "")
    //                            //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                        else
    //                            //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                    }
    //                }
    //                else {
    //                    if (objdata[i].Reason_For_Referral != "") {
    //                        if (objdata[i].Referred_to != "")
    //                            //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                        else
    //                            //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                    }
    //                    else {
    //                        if (objdata[i].Referred_to != "")
    //                            //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                        else
    //                            //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
    //                    }
    //                }
    //            }
    //            //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //        }
    //        else
    //            //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");
    //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");
    //        $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.length + ")";

    //        sessionStorage.setItem("Order_Count", objdata.length);

    //        localStorage.setItem("GenralOrderCount", objdata.length);

    //        SortTableHeader('GeneralQOrder');;
    //        //$('#EncounterTable th').addClass('header');
    //        RowClick();
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#hdnIsShowAllGeneralOrdersQueue")[0].value == "Y") {
        Showall = "Checked";
        document.getElementById("chkShowAll").style.display = "none";
        document.getElementById("lblShowAll").style.display = "none"
    }
    else {
        $("#chkShowAll")[0].disabled = false;
        $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

    }
    $('#GeneralQTable').empty();
    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style=''><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox' class='GenQChkbxAll'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");


    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        scrollCollapse: true,
        scrollY: '395px',
        processing: false,
        ordering: true,
        autowidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. #",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: "frmMyQueueNew.aspx/LoadOrder",
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "sShowall": Showall
                });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);

                $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.data.length + ")";

                sessionStorage.setItem("Order_Count", objdata.data.length);

                localStorage.setItem("GenralOrderCount", objdata.data.length);

                json.draw = objdata.draw;
                json.recordsTotal = objdata.recordsTotal;
                json.recordsFiltered = objdata.recordsFiltered;
                json.data = objdata.data;


                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


                return json.data;

            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: '', render: function (data, type, row) { return "<input type='checkbox'  onclick='checkboxclick(this)'/>"; },
                sWidth: '3%',
                searchable: false,
                orderable: false
            }
            ,
            {
                data: 'Created_Date_And_Time', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    dt1 = ConvertDate(dt1.replaceAll("T", " "));
                    var dt2 = dt1.split(' ');
                    if (dt2.length > 0) {
                        if (dt2.indexOf("01-01-0001") > -1) {
                            return "";
                        }
                    }
                    return dt1;
                }, type: 'date', sWidth: '10%', searchable: false, sClass:"process-word-wrap"
            },
            {
                data: 'Test_Date', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    dt1 = ConvertDate(dt1.replaceAll("T", " "));
                    var dt2 = dt1.split(' ');
                    if (dt2.length > 0) {
                        if (dt2.indexOf("01-01-0001") > -1) {
                            return "";
                        }
                    }
                    return dt1;
                }, type: 'date', sWidth: '10%', searchable: false, sClass: "hide_column"
            },
            { data: "Human_ID", sWidth: '10%', sClass: "process-word-wrap" },
            { data: "External_Account_Number", sWidth: '10%', searchable: false, sClass: "process-word-wrap" },
            {
                data: "Last_Name", render: function (data, type, row) {
                    return row.Last_Name + "," + row.First_Name + " " + row.MI;
                }, sWidth: '10%', sClass: "process-word-wrap"
            },
            {
                data: 'DOB', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                },
                searchable: false,
                type: 'date',
                sWidth: '10%',
                 sClass: "process-word-wrap"
            },
            {
                data: '', render: function (data, type, row) {
                    if (row.Reason_For_Referral != "") {
                        return row.Reason_For_Referral;
                    }
                    else {
                        return row.Procedure_Ordered;
                    }
                }, sWidth: '10%', searchable: false, sClass: "process-word-wrap"
            },
            {
                data: "PhyName", searchable: false, sWidth: '10%', sClass: "process-word-wrap"
            },
            {
                data: "Current_Process", searchable: false, sWidth: '10%', sClass: "process-word-wrap"
            },
            {
                data: "", render: function (data, type, row) {
                    if (row.Referred_to != "") {
                        return row.Referred_to;
                    }
                    else {
                        return row.Lab_Name;
                    }
                }, searchable: false, sWidth: '10%', sClass: "process-word-wrap"
            },
            { data: "Lab_Loc_Name", sClass: "hide_column", searchable: false },
            { data: "Encounter_ID", sClass: "hide_column", searchable: false },
            { data: "Physician_ID", sClass: "hide_column", searchable: false },
            { data: "Order_ID", sClass: "hide_column", searchable: false },
            { data: "EHR_Obj_Type", sClass: "hide_column", searchable: false },
            { data: "Lab_ID", sClass: "hide_column", searchable: false },
            { data: "Lab_Location_ID", sClass: "hide_column", searchable: false },
            { data: "Order_Submit_ID", sClass: "hide_column", searchable: false },
            { data: "Referred_to_Facility", sClass: "hide_column", searchable: false }
            
        ],
        createdRow: function (row, data, dataIndex) {

        },
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
            SetHeightForTabelBasedOnScreenSize();
        }

    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });
    
    $("#EncounterTable thead").click(function () {
        $("#EncounterTable thead tr").removeClass('highlight');
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').find('input[type=checkbox]').prop('checked', false);
        $('.GenQChkbxAll').prop('checked', false);
        dataTable.$('tr.highlight').removeClass('highlight');
       
        
    });

    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').find('input[type=checkbox]').prop('checked', false);
        $('.GenQChkbxAll').prop('checked', false);
        dataTable.$('tr.highlight').removeClass('highlight');
       ``
    });
   
    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");

        if (event.target.tagName != 'INPUT' && event.target.tagName != 'TH') {
            if ($(this)[0].children[0].children[0].checked) {
                $(this).removeClass("highlight");
                $(this)[0].children[0].children[0].checked = false;
            }
            else {
                $(this).toggleClass("highlight");
                $(this)[0].children[0].children[0].checked = true;
            }

        }

    });
}
function loadamend() {
    $("#chkMyShowAll")[0].disabled = false;
    if ($('#hdnIsShowAllGeneralAmendmentQueue').val() == 'Y') {
        $('#chkShowAll,#lblShowAll').css("display", "none");
        $('#chkShowAll').prop('checked', true);
    } else {
        $('#chkShowAll,#lblShowAll').css("display", "");
    }
    $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";

    $("#GeneralQTable").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 0px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th>
    <th style='border: 1px solid #909090;display:none;'>EncounterID</th>
    <th style='border: 1px solid #909090;display:none;'>PhysicianID</th>
    <th style='border: 1px solid #909090;display:none;'>ObjType</th>
    <th style='border: 1px solid #909090;display:none;'>AddendumID</th>
    <th style='border: 1px solid #909090;display:none;'>Current Owner</th>
    </tr>
    </thead>
    </table>`);

    data = JSON.stringify({ "sShowall": ShowallGeneral });
    var dataTable =
        new DataTable('#EncounterTable', {
            serverSide: false,
            lengthChange: false,
            searching: true,
            scrollCollapse: true,
            scrollY: '420px',
            processing: false,
            ordering: true,
            autoWidth: false,
            order: [],
            pageLength: 15,
            language: {
                search: "",
                searchPlaceholder: "Search by Name or Acct. #",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top
            ajax: {
                url: '/frmMyQueueNew.aspx/LoadAmend',
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                deferRender: true,
                data: function (d) {
                    d.extra_search = data;
                    return d;
                },
                dataSrc: function (json) {
                    var objdata = json.d;
                    objdata.data = Decompress(objdata.data);
                    $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.data.length + ")";
                    if (Showall != "Checked") {
                        sessionStorage.setItem("Amendmnt_Count", objdata.data.length);
                    }
                    $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return objdata.data;
                },
                error: function (xhr, error, code) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            },
            columns: [
                {
                    data: '', render: function (data, type, row) {
                        return " <td style='width:3%'><input type = 'checkbox' onclick = 'checkboxclick(this)' /></td >";
                    }, 
                    searchable: false,
                    orderable: false, sWidth:'3%'
                },
                {
                    data: 'Appt_Date_Time', render: function (data, type, row) {
                        if (row.Appt_Date_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Appt_Date_Time.replace("T", " "));
                    }, searchable: false,
                    type: 'date', sWidth: '9%'
                },
                {
                    data: 'Addendum_Created_Date_Time', render: function (data, type, row) {
                        if (row.Addendum_Created_Date_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Addendum_Created_Date_Time.replace("T", " "));
                    }, searchable: false,
                    type: 'date', sWidth: '9%'
                },
                { data: 'Human_ID', sWidth: '6%' },
                { data: 'External_Account_Number', searchable: false, sWidth: '7%' },
                {
                    data: 'Last_Name', render: function (data, type, row) {
                        return row.Last_Name + "," + row.First_Name + " " + row.MI;
                    },
                    sClass: 'word-break-all', sWidth: '9%'
                },
                { data: 'Current_Process', searchable: false, sWidth: '9%' },
                {
                    data: 'Addendum_Created_Date_Time', render: function (data, type, row) {
                        if (row.Addendum_Created_Date_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Addendum_Created_Date_Time.replace("T", " "));
                    }, searchable: false,
                    type: 'date', sWidth: '9%'
                },
                { data: 'Addendum_Created_By', searchable: false, sWidth: '9%' },
                { data: 'Addendum_Signed_By', searchable: false, sWidth: '9%' },
                { data: 'Encounter_ID', sClass: "hide_column", searchable: false },
                { data: 'Physician_ID', sClass: "hide_column", searchable: false },
                { data: 'EHR_Obj_Type', sClass: "hide_column", searchable: false },
                { data: 'Addendum_ID', sClass: "hide_column", searchable: false },
                { data: 'Current_Owner', sClass: "hide_column", searchable: false },
            ],
            initComplete: function (settings, json) {
                $("#EncounterTable_filter input")[0].classList.add('searchicon');
                SetHeightForTabelBasedOnScreenSize();
            }
        });
    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
        'width': '500px',
    });

    $('#EncounterTable_info').css({
        'min-width': '180px'
    });

    $('#EncounterTable tbody').on('dblclick', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        if (event?.target?.tagName != undefined && document.getElementById("RefreshQ")?.innerText?.indexOf("Refresh Encounters Q") != undefined) {
            if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Encounters Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
            else if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Task Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
        }
        sessionStorage.setItem('MyQRemoveIdList', '');
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        if (event.target.tagName != 'INPUT') {
            if ($(this)[0].children[0].children[0].checked) {
                $(this).removeClass("highlight");
                $(this)[0].children[0].children[0].checked = false;
            }
            else {
                $(this).toggleClass("highlight");
                $(this)[0].children[0].children[0].checked = true;
            }

        }
    });
    $("#EncounterTable_filter").children("label").children("input").css("width", "300px")



    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/LoadAmend",
    //    data: JSON.stringify({
    //        "sShowall": "Unchecked",
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#GeneralQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);
    //        if (data.d != "[]") {
    //            for (var i = 0; i < objdata.length; i++) {
    //                if (i == 0) {
    //                    //GitLab#2246
    //                    //tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
    //                    tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
    //                } else {
    //                    //GitLab#2246
    //                    //tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
    //                    tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
    //                }
    //            }
    //            //GitLab#2246
    //            /// $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //        }
    //        else
    //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
    //        //GitLab#2246
    //        //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
    //        //$("#btnAmendmnt")[0].innerText = "Amendment Q " + "(*)";
    //        $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";

    //        sessionStorage.setItem("Amendmnt_Count", objdata.length);

    //        SortTableHeader('GeneralQAmendment');
    //        //$('#EncounterTable th').addClass('header');
    //        RowClick();
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});

}
function chkMyTask14Click(sender) {
    $("#chkOpenTask")[0].checked = false;
    $("#chkMyShowAll")[0].checked = false;
    localStorage.setItem('MyOpenTask', "");
    localStorage.setItem('MyShowAllMyTask', "");
    localStorage.setItem('MyTask14', $("#chkMyTask14")[0].checked ? "Checked" : "Unchecked");
    LoadMyTask();

    //if ($("#chkMyShowAll") != null) {
    //    if ($("#chkMyTask14")[0].checked) {
    //        $("#chkOpenTask")[0].disabled = true;
    //        $("#chkMyShowAll")[0].disabled = true;
    //        $.ajax({
    //            type: "POST",
    //            url: "frmMyQueueNew.aspx/LoadMyTaskCompleted",
    //            data: JSON.stringify({
    //                "sShowall": "",
    //            }),
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            async: true,
    //            success: function (data) {
    //                $('#MyQTable').empty();
    //                var tabContents;
    //                var objdata = $.parseJSON(data.d);
    //                if (data.d != "[]") {
    //                    for (var i = 0; i < objdata.length; i++) {
    //                        if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
    //                            Msg_Date_And_Time = "";
    //                        else
    //                            Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
    //                        if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
    //                            Modified_Date_Time = "";
    //                        else
    //                            Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
    //                        if (i == 0) {
    //                            //Jira #CAP-119
    //                            //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                            tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                        }
    //                        else {
    //                            //Jira #CAP-119
    //                            //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                            tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                        }
    //                    }
    //                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //                }
    //                else
    //                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
    //                //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
    //                $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
    //                sessionStorage.setItem("My_Task_Count", objdata.length);

    //                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    //                SortTableHeader('MyQTask');
    //                //$('#EncounterTable th').addClass('header');
    //                RowClick();
    //                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //            },
    //            error: function OnError(xhr) {
    //                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //                if (xhr.status == 999)
    //                    window.location = "/frmSessionExpired.aspx";
    //                else {
    //                    var log = JSON.parse(xhr.responseText);
    //                    console.log(log);
    //                    alert("USER MESSAGE:\n" +
    //                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                        "Message: " + log.Message);
    //                }
    //            }
    //        });
    //    }
    //    else {
    //        $("#chkOpenTask")[0].disabled = false;
    //        $("#chkMyShowAll")[0].disabled = false;
    //        $.ajax({
    //            type: "POST",
    //            url: "frmMyQueueNew.aspx/LoadMyTask",
    //            data: JSON.stringify({
    //                "sShowall": "",
    //                "sOpenTask": ""
    //            }),
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            async: true,
    //            success: function (data) {
    //                $('#MyQTable').empty();
    //                var tabContents;
    //                var objdata = $.parseJSON(data.d);
    //                if (data.d != "[]") {
    //                    for (var i = 0; i < objdata.length; i++) {
    //                        if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
    //                            Msg_Date_And_Time = "";
    //                        else
    //                            Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
    //                        if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
    //                            Modified_Date_Time = "";
    //                        else
    //                            Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
    //                        if (i == 0) {
    //                            //Jira #CAP-119
    //                            //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                            tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";

    //                        } else {
    //                            //Jira #CAP-119
    //                            //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                            tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";

    //                        }
    //                    }
    //                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //                }
    //                else
    //                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
    //                //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
    //                $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
    //                sessionStorage.setItem("My_Task_Count", objdata.length);
    //                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    //                SortTableHeader('MyQTask');
    //                //$('#EncounterTable th').addClass('header');
    //                RowClick();
    //                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //            },
    //            error: function OnError(xhr) {
    //                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //                if (xhr.status == 999)
    //                    window.location = "/frmSessionExpired.aspx";
    //                else {
    //                    var log = JSON.parse(xhr.responseText);
    //                    console.log(log);
    //                    alert("USER MESSAGE:\n" +
    //                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                        "Message: " + log.Message);
    //                }
    //            }
    //        });

    //    }
    //}
}

function chkOpenTaskClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $("#chkMyTask14")[0].checked = false;
    $("#chkMyTask14")[0].disabled = false;
    var myOpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";
    localStorage.setItem('MyOpenTask', myOpenTask);
    if ($("#chkOpenTask")[0].checked) {
        $("#chkMyTask14")[0].disabled = true;
    }

    var Showall = $("#chkMyShowAll")[0].checked ? "Checked" : "Unchecked";
    var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";
    LoadMyTask();

    //$.ajax({
    //    type: "POST",
    //    url: "frmMyQueueNew.aspx/LoadMyTask",
    //    data: JSON.stringify({
    //        "sShowall": Showall,
    //        "sOpenTask": OpenTask,
    //    }),
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: true,
    //    success: function (data) {
    //        $('#MyQTable').empty();
    //        var tabContents;
    //        var objdata = $.parseJSON(data.d);
    //        if (data.d != "[]") {
    //            for (var i = 0; i < objdata.length; i++) {
    //                if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
    //                    Msg_Date_And_Time = "";
    //                else
    //                    Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
    //                if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
    //                    Modified_Date_Time = "";
    //                else
    //                    Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
    //                if (i == 0) {
    //                    //Jira #CAP-119
    //                    //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                    tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                }
    //                else {
    //                    //Jira #CAP-119
    //                    //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                    tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
    //                }
    //            }
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //        }
    //        else
    //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
    //        //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
    //        $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
    //        if (Showall != "Checked") {
    //            sessionStorage.setItem("My_Task_Count", objdata.length);
    //        }
    //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
    //        SortTableHeader('MyQTask');
    //        //$('#EncounterTable th').addClass('header');
    //        RowClick();
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});
}

function ChangeTableForTabs(sender) {

    //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";

    //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
    //$("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
    //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
    //$("#btnAmendmnt")[0].innerText = "Amendment Q" + "(*)";
    $(":button:not(#btnGeneralQcount):not(#btnMyQcount)").css("background-color", "transparent");
    var Showall = "";
    $("#chkMyShowAll")[0].checked = false;
    $("#chkShowAll")[0].checked = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $('#RefreshMyQ').css("background-color", "");
    $('#Processenctr').css("background-color", "");
    $('#btnChangeExamRoom').css("display", "none");
    $('#chkMyTask14').css("display", "none");
    $('#ctl00_C5POBody_btnMyAkidoTasks').css("display", "none");
    $('#lbl14days').css("display", "none");
    $('#chkOpenTask').css("display", "none");
    $('#lblOpenTask').css("display", "none");
    $('#cboYears').css("display", "none");
    if (sender.innerText.indexOf("My Encounter") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyEnc").removeClass("default");
        $("#btnMyEnc").addClass("btncolorMyQ");

        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");

        $('#btnChangeExamRoom').css("display", "inline-block");
        $('#btnChangeExamRoom').css("background-color", "");
        $('#MovetoNxtProcess').css("display", "inline-block");
        $('#MovetoNxtProcess').css("background-color", "");
        $('#RefreshMyQ')[0].innerText = "Refresh My Encounters";
        $('#Processenctr')[0].innerText = "Process Encounter";
        window.setTimeout(LoadMyEncounter, 300);
    }
    else if (sender.innerText.indexOf("My Task") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        $("#btnMyTask").removeClass("default");
        $("#btnMyTask").addClass("btncolorMyQ");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");

        $('#chkMyTask14').css("display", "");
        $('#ctl00_C5POBody_btnMyAkidoTasks').css("display", "");
        $('#lbl14days').css("display", "");
        $('#chkOpenTask').css("display", "");
        $('#lblOpenTask').css("display", "");
        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Tasks";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Task";
        window.setTimeout(LoadMyTask, 300);
    }
    else if (sender.innerText.indexOf("My Order") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyOrder").removeClass("default");
        $("#btnMyOrder").addClass("btncolorMyQ");

        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");


        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Orders";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Order";
        window.setTimeout(loadMyorder, 300);
    }
    else if (sender.innerText.indexOf("My Scan") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyScan").removeClass("default");
        $("#btnMyScan").addClass("btncolorMyQ");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");
        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");

        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Scan";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Scan";
        window.setTimeout(loadMyscan, 300);
    }
    else if (sender.innerText.indexOf("My Prescription") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyPres").removeClass("default");
        $("#btnMyPres").addClass("btncolorMyQ");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");
        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");

        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Prescription";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Prescription";
        window.setTimeout(loadMyprescription, 300);
    }
    else if (sender.innerText.indexOf("My Amendment") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyAmendmnt").removeClass("default");
        $("#btnMyAmendmnt").addClass("btncolorMyQ");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");
        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");

        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Amendment";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Amendment";
        window.setTimeout(loadMyAmendment, 300);
    }
    else if ((sender.innerText.indexOf("Encounter")) > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnEnc").removeClass("default");
        $("#btnEnc").addClass("btncolorMyQ");
        $("#btnTask").removeClass("btncolorMyQ");
        $("#btnTask").addClass("default");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
        if (Role == "Medical Assistant" || Role == "Office Manager") {
            $('#Exam')[0].style.visibility = "visible";
            $('#lblEr')[0].style.visibility = "visible";
        }
        else {
            $('#Exam')[0].style.visibility = "hidden";
            $('#lblEr')[0].style.visibility = "hidden";
        }
        $('#btnChkOut')[0].style.visibility = "visible";
        $('#btnChkOut')[0].style.display = "";
        $('#MoveTo')[0].innerText = "Move To My Encounters";
        $('#Processenc')[0].style.display = "none";
        $('#Processenc')[0].innerText = "Process Encounter";
        window.setTimeout(LoadGeneralEncounter, 300);
    }
    else if (sender.innerText.indexOf("Task") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnTask").removeClass("default");
        $("#btnTask").addClass("btncolorMyQ");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");
        $("#btnEnc").removeClass("btncolorMyQ");
        $("#btnEnc").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Task Q";
        $('#MoveTo')[0].innerText = "Move To My Task";
        $('#btnChkOut')[0].style.display = "none";
        $('#lblEr')[0].style.display = "none";
        $('#Exam')[0].style.display = "none";
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "inline-block";
        $('#Processenc')[0].style.visibility = "visible";
        $('#Processenc')[0].innerText = "Process Task";
        window.setTimeout(loadTask, 300);
    }
    else if (sender.innerText.indexOf("Order") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnOrder").removeClass("default");
        $("#btnOrder").addClass("btncolorMyQ");
        $("#btnEnc").removeClass("btncolorMyQ");
        $("#btnEnc").addClass("default");
        $("#btnTask").removeClass("btncolorMyQ");
        $("#btnTask").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Orders Q";
        $('#MoveTo')[0].innerText = "Move To My Orders";
        $('#btnChkOut')[0].style.display = "none";
        $('#lblEr')[0].style.display = "none";
        $('#Exam')[0].style.display = "none";
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        window.setTimeout(loadorder, 300);
    }
    else if (sender.innerText.indexOf("Amendment") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnAmendmnt").removeClass("default");
        $("#btnAmendmnt").addClass("btncolorMyQ");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");
        $("#btnEnc").removeClass("btncolorMyQ");
        $("#btnEnc").addClass("default");
        $("#btnTask").removeClass("btncolorMyQ");
        $("#btnTask").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Amendment Q";
        $('#MoveTo')[0].innerText = "Move To My Amendment";
        $('#btnChkOut')[0].style.display = "none";
        $('#lblEr')[0].style.display = "none";
        $('#Exam')[0].style.display = "none";
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        window.setTimeout(loadamend, 300);
    }
}

$('#ProcessModal').on('hidden.bs.modal', function () {
    chkShowAllClick();
});

function shwllclck() {
    $('#MyQTable').empty();
    //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";

    //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
    //$("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
    //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
    //$("#btnAmendmnt")[0].innerText = "Amendment Q" + "(*)";
    $(":button:not(#btnGeneralQcount):not(#btnMyQcount)").css("background-color", "transparent");
    $('#btnChangeExamRoom').css("display", "none");
    $("#chkMyTask14")[0].checked = false;
    $("#chkMyShowAll")[0].disabled = false;
    if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Encounters") > -1 && $('#RefreshMyQ').is(":visible")) {

        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Encounter";
        $('#btnChangeExamRoom').css("display", "inline-block");
        $('#btnChangeExamRoom').css("background-color", "");
        $('#MovetoNxtProcess').css("display", "inline-block");
        $('#MovetoNxtProcess').css("background-color", "");
        $("#btnMyEnc").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyTask").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        if ($("#MovetoNxtProcess") != null)
            $("#MovetoNxtProcess")[0].disabled = true;
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        localStorage.setItem("MyShowAll", Showall);
        LoadMyEncounter();


    }
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Tasks") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyTask").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $('#RefreshMyQ')[0].innerText = "Refresh My Tasks";
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Task";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        localStorage.setItem('MyShowAllMyTask', Showall);

        var myOpenTask = localStorage.getItem('MyOpenTask');
        if (myOpenTask == "Checked") {
            $("#chkOpenTask")[0].checked = true;
        }

        var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";

        if ($("#chkMyShowAll")[0].checked) {
            $("#chkMyTask14")[0].checked = false;
            $("#chkMyTask14")[0].disabled = true;
        } else {
            $("#chkMyTask14")[0].checked = false;
            $("#chkMyTask14")[0].disabled = false;
        }
        LoadMyTask();

        //$.ajax({
        //    type: "POST",
        //    url: "frmMyQueueNew.aspx/LoadMyTask",
        //    data: JSON.stringify({
        //        "sShowall": Showall,
        //        "sOpenTask": OpenTask
        //    }),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    success: function (data) {
        //        $('#MyQTable').empty();
        //        var tabContents;
        //        var objdata = $.parseJSON(data.d);
        //        if (data.d != "[]") {
        //            for (var i = 0; i < objdata.length; i++) {
        //                if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
        //                    Msg_Date_And_Time = "";
        //                else
        //                    Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
        //                if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
        //                    Modified_Date_Time = "";
        //                else
        //                    Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
        //                if (i == 0) {
        //                    //Jira #CAP-119
        //                    //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
        //                    //CAP-1536
        //                    tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";

        //                }
        //                else {
        //                    //Jira #CAP-119
        //                    //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
        //                    //CAP-1536
        //                    tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "&#013;").replace(/'/g, '') + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
        //                }
        //            }
        //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
        //            $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
        //        }
        //        else
        //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
        //        //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
        //        $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
        //        if (Showall != "Checked") {
        //            sessionStorage.setItem("My_Task_Count", objdata.length);
        //        }
        //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
        //        SortTableHeader('MyQTask');
        //        //$('#EncounterTable th').addClass('header');
        //        RowClick();
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    },
        //    error: function OnError(xhr) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        if (xhr.status == 999)
        //            window.location = "/frmSessionExpired.aspx";
        //        else {
        //            var log = JSON.parse(xhr.responseText);
        //            console.log(log);
        //            alert("USER MESSAGE:\n" +
        //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
        //                "Message: " + log.Message);
        //        }
        //    }
        //});

    }
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Scan") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyScan").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Scan";
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        $('#RefreshMyQ').css("background-color", "");
        //$.ajax({
        //    type: "POST",
        //    url: "frmMyQueueNew.aspx/LoadMyScan",
        //    data: JSON.stringify({
        //        "sShowall": Showall,
        //    }),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    success: function (data) {
        //        $('#MyQTable').empty();
        //        var tabContents;
        //        var objdata = $.parseJSON(data.d);
        //        if (data.d != "[]") {
        //            for (var i = 0; i < objdata.length; i++) {
        //                if (i == 0)
        //                    tabContents = "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + + "</td></tr>";
        //                else
        //                    tabContents = tabContents + "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + "</td></tr>";
        //            }
        //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
        //            //Jira #CAP-938
        //            //$("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.length + ")";
        //        }
        //        else
        //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead></table>");
        //        //Jira #CAP-938
        //        $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.length + ")";
        //        if (Showall != "Checked") {
        //            sessionStorage.setItem("My_Scan_Count", objdata.length);
        //        }
        //        //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
        //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
        //        SortTableHeader('MyQScan');
        //        //$('#EncounterTable th').addClass('header');
        //        RowClick();
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    },
        //    error: function OnError(xhr) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        if (xhr.status == 999)
        //            window.location = "/frmSessionExpired.aspx";
        //        else {
        //            var log = JSON.parse(xhr.responseText);
        //            console.log(log);
        //            alert("USER MESSAGE:\n" +
        //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
        //                "Message: " + log.Message);
        //        }
        //    }
        //});
        loadMyscan();

    }
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Orders") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyOrder").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $('#RefreshMyQ')[0].innerText = "Refresh My Orders";
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Order";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        loadMyorder();

    }
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Amendment") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyAmendmnt").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Amendment";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        loadMyAmendment();
        //$.ajax({
        //    type: "POST",
        //    url: "frmMyQueueNew.aspx/LoadMyAmendment",
        //    data: JSON.stringify({
        //        "sShowall": Showall,
        //    }),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    success: function (data) {
        //        $('#MyQTable').empty();
        //        var tabContents;
        //        var objdata = $.parseJSON(data.d);
        //        if (data.d != "[]") {
        //            for (var i = 0; i < objdata.length; i++) {
        //                if (i == 0)
        //                    tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
        //                else
        //                    tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
        //            }
        //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
        //            //Jira #CAP-938
        //            //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.length + ")";
        //        }
        //        else
        //            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
        //         //Jira #CAP-938
        //        $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.length + ")";

        //        if (Showall != "Checked") {
        //            sessionStorage.setItem("My_Amendmnt_Count", objdata.length);
        //        }

        //        //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
        //        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
        //        SortTableHeader('MyQAmendment');
        //        //$('#EncounterTable th').addClass('header');
        //        RowClick();
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    },
        //    error: function OnError(xhr) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        if (xhr.status == 999)
        //            window.location = "/frmSessionExpired.aspx";
        //        else {
        //            var log = JSON.parse(xhr.responseText);
        //            console.log(log);
        //            alert("USER MESSAGE:\n" +
        //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
        //                "Message: " + log.Message);
        //        }
        //    }
        //});

    }
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Prescription") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyPres").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Prescription";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        loadMyprescription();

    }
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Encounters Q") > -1 && $('#RefreshQ').is(":visible")) {
        document.getElementById("divGeneralQ").style.display = "";
        $('#GeneralQTable').empty();
        $("#btnEnc").addClass("btncolorMyQ");
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
        var ShowallGeneral = "";
        $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
        var MyShowAll = localStorage.setItem('ShowallGeneralqueue', ShowallGeneral);
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#Processenc')[0].style.display = "none";

        var ViewAllFacilities = "";
        if ($("#ctl00_C5POBody_chkViewAllFacilities")[0] != undefined && $("#ctl00_C5POBody_chkViewAllFacilities")[0] != null) {
            $("#ctl00_C5POBody_chkViewAllFacilities")[0].checked ? ViewAllFacilities = "Checked" : ViewAllFacilities = "Unchecked";
        }
        LoadGeneralEncounter();

    }
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Orders Q") > -1 && $('#RefreshQ').is(":visible")) {
        document.getElementById("divGeneralQ").style.display = "";
        $('#GeneralQTable').empty();
        $("#btnOrder").css({ "background-color": "#bfdbff" });
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ')[0].innerText = "Refresh Orders Q";
        var ShowallGeneral = "";
        $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        //$.ajax({
        //    type: "POST",
        //    url: "frmMyQueueNew.aspx/LoadOrder",
        //    data: JSON.stringify({
        //        "sShowall": ShowallGeneral,
        //    }),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    success: function (data) {
        //        $('#GeneralQTable').empty();
        //        var tabContents;
        //        var objdata = $.parseJSON(data.d);
        //        if (data.d != "[]") {
        //            for (var i = 0; i < objdata.length; i++) {
        //                var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
        //                if (i == 0) {
        //                    if (objdata[i].Reason_For_Referral != "") {
        //                        if (objdata[i].Referred_to != "")
        //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                        else
        //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                    }
        //                    else {
        //                        if (objdata[i].Referred_to != "")
        //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                        else
        //                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                    }
        //                }
        //                else {
        //                    if (objdata[i].Reason_For_Referral != "") {
        //                        if (objdata[i].Referred_to != "")
        //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                        else
        //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                    }
        //                    else {
        //                        if (objdata[i].Referred_to != "")
        //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                        else
        //                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
        //                    }
        //                }
        //            }
        //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;width:10%'>Acct. #</th><th style='border: 1px solid #909090;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;width:10%'>Patient Name</th><th style='border: 1px solid #909090;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;width:10%'>Description</th><th style='border: 1px solid #909090;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;width:10%'>Current Process</th><th style='border: 1px solid #909090;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
        //        }
        //        else
        //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;width:10%'>Acct. #</th><th style='border: 1px solid #909090;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;width:10%'>Patient Name</th><th style='border: 1px solid #909090;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;width:10%'>Description</th><th style='border: 1px solid #909090;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;width:10%'>Current Process</th><th style='border: 1px solid #909090;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none'>Referred to Facility</th></tr></thead></table>");
        //        $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.length + ")";

        //        if (ShowallGeneral != "Checked") {
        //            sessionStorage.setItem("Order_Count", objdata.length);
        //        }

        //        localStorage.setItem("GenralOrderCount", objdata.length);
        //        SortTableHeader('GeneralQOrder');
        //        //$('#EncounterTable th').addClass('header');
        //        RowClick();

        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    },
        //    error: function OnError(xhr) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        if (xhr.status == 999)
        //            window.location = "/frmSessionExpired.aspx";
        //        else {
        //            var log = JSON.parse(xhr.responseText);
        //            console.log(log);
        //            alert("USER MESSAGE:\n" +
        //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
        //                "Message: " + log.Message);
        //        }
        //    }
        //});

        loadorder();

    }
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Amendment Q") > -1 && $('#RefreshQ').is(":visible")) {
        $('#GeneralQTable').empty();
        loadamend();
        //$("#btnAmendmnt").css({ "background-color": "#bfdbff" });
        //$("#btnGeneral").addClass("btncolorMyQ");
        //$('#RefreshQ')[0].innerText = "Refresh Amendment Q";
        //var ShowallGeneral = "";
        //$("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
        //$('#RefreshQ').css("background-color", "");
        //$('#btnChkOut').css("background-color", "");
        //$('#MoveTo').css("background-color", "");
        //$('#Processenc').css("background-color", "");
        //$('#Processenc')[0].style.display = "none";
        //$.ajax({
        //    type: "POST",
        //    url: "frmMyQueueNew.aspx/LoadAmend",
        //    data: JSON.stringify({
        //        "sShowall": ShowallGeneral,
        //    }),
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: true,
        //    success: function (data) {
        //        $('#GeneralQTable').empty();
        //        var tabContents;
        //        var objdata = $.parseJSON(data.d);
        //        if (data.d != "[]") {
        //            for (var i = 0; i < objdata.length; i++) {
        //                if (i == 0) {
        //                    //GitLab#2246
        //                    //tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
        //                    tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
        //                }
        //                else {
        //                    //GitLab#2246
        //                    //tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
        //                    tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
        //                }
        //            }
        //            //GitLab#2246
        //            //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
        //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
        //            //Jira #CAP-938
        //            //$("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";
        //        }
        //        else
        //            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
        //        //Jira #CAP-938
        //        $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";
        //        if (ShowallGeneral != "Checked") {
        //            sessionStorage.setItem("Amendmnt_Count", objdata.length);
        //        }
        //         //GitLab#2246
        //        //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
        //        //$("#btnAmendmnt")[0].innerText = "Amendment Q " + "(*)";
        //        SortTableHeader('GeneralQAmendment');
        //        //$('#EncounterTable th').addClass('header');
        //        RowClick();
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    },
        //    error: function OnError(xhr) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        if (xhr.status == 999)
        //            window.location = "/frmSessionExpired.aspx";
        //        else {
        //            var log = JSON.parse(xhr.responseText);
        //            console.log(log);
        //            alert("USER MESSAGE:\n" +
        //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
        //                "Message: " + log.Message);
        //        }
        //    }
        //});

    }
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Task Q") > -1 && $('#RefreshQ').is(":visible")) {
        $('#GeneralQTable').empty();
        loadTask();
    }
}

function chkShowAllClick(sender) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.setTimeout(shwllclck, 2000);
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
$('#btnChangeExamRoom').click(function (evt) {

    //Jira CAP-1354
    if ($('#MyQTable').children().find('.highlight').length > 1) {
        alert("Please select one encounter to process");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('#MyQTable tr').removeClass("highlight");
        //Jira CAP-1444
        if ($('#MovetoNxtProcess') != undefined && $('#MovetoNxtProcess')[0]?.disabled != undefined) {
            $('#MovetoNxtProcess')[0].disabled = true;
        }
        return false;
    }

    var cells = $('#MyQTable').children().find('.highlight').children();
    if (cells.length > 0) {
        openRadWindow("frmRoomIn.aspx?AccountNumber=" + cells[2].innerText + "&PatientName=" + cells[4].innerText + "&PatientDOB=" + cells[5].innerText +
            "&AssignedPhysician=" + cells[9].innerText + "&AppointmentDateTime=" +
            cells[1].innerText + "&TypeOfVisit=" + '' + "&ExamRoom=" + cells[14].innerText + "&EnounterID=" + cells[13].innerText, 255, 1050, "", 'MessageWindow');
        var windowName = $find('MessageWindow');
        return false;
    }
    else {
        alert("Please select an encounter to process.");
    }
});


function RefreshMyQueue() {
    $('#RefreshMyQ').click();
}

function RowClick() {
    $('#EncounterTable tr').removeClass("odd");
    $('#EncounterTable tr').removeClass("even");

    $("#MyQTable tr").click(function () {
        //Jira CAP-1201
        ////OldCode start
        //var existingSelectedItem = $("#MyQTable tr.highlight");
        //var currentprocesscnt = 0;
        //if (isproviderReview == false) {
        //    if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
        //}
        ////$('#MyQTable tr.highlight').each(function (i, row) {
        ////    var $row = $(row);
        ////    var currentProcesss = '';
        ////    if ($row.find('td:nth-child(7)')[0] != undefined) {
        ////        currentProcesss = $row.find('td:nth-child(7)')[0].outerText.trim();
        ////        if (currentProcesss != "PROVIDER_REVIEW" || currentProcesss != "PROVIDER_REVIEW_2")
        ////            currentprocesscnt++;
        ////    }
        ////});
        ////if (currentprocesscnt == 0) {
        ////    if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
        ////}
        //isproviderReview = false;
        //$(this).toggleClass("highlight");

        ////if ($(this).find('input[type="checkbox"]:enabled').length > 0) {
        ////    var existingSelectedItem = $("#MyQTable tr.highlight");
        ////    if (existingSelectedItem.length > 0) {
        ////        if ($(this).find('input[type="checkbox"]:enabled')[0].checked == true) {
        ////            $(this).removeClass("highlight");
        ////            $(this).find('input[type="checkbox"]:enabled')[0].checked = false;
        ////        }
        ////        else {
        ////            $(this).addClass("highlight");
        ////            $(this).find('input[type="checkbox"]:enabled')[0].checked = true;
        ////        }
        ////    }
        ////}
        ////OldCode End

        //Jira CAP-1201
        var existingSelectedItem = $("#MyQTable tr.highlight");

        for (var i = 0; i < existingSelectedItem.length; i++) {
            //Jira CAP-1413
            //var processes = processes = existingSelectedItem[i].children[7].childNodes[0].data;
            var processes = "NoCurrentProcess";
            if (existingSelectedItem[i]?.children[7]?.childNodes[0]?.data != undefined && existingSelectedItem[i]?.children[7]?.childNodes[0]?.data != null) {
                processes = existingSelectedItem[i].children[7].childNodes[0].data;
            }

            var isproviderReviewMyQ = processes;
            if (isproviderReviewMyQ != "PROVIDER_REVIEW" && isproviderReviewMyQ != "PROVIDER_REVIEW_2") {
                existingSelectedItem[i].classList.remove("highlight");
            }
        }

        $(this)[0].classList.add('highlight');
        //Jira CAP-1413
        //var NewRowprocesses = $(this)[0].children[7].childNodes[0].data;
        var NewRowprocesses = "NoCurrentProcess";
        if ($(this)[0]?.children[7]?.childNodes[0]?.data != undefined && $(this)[0]?.children[7]?.childNodes[0]?.data != null) {
            NewRowprocesses = $(this)[0].children[7].childNodes[0].data;
        }
        var isproviderReviewMyQNewRow = NewRowprocesses;

        if (isproviderReviewMyQNewRow == "PROVIDER_REVIEW" || isproviderReviewMyQNewRow == "PROVIDER_REVIEW_2") {
            if ($(this)[0].children[0].children[0].checked == false) {
                $(this)[0].children[0].children[0].checked = true;
            }
            else {
                $(this)[0].children[0].children[0].checked = false;
                $(this).removeClass("highlight");
            }
            MyQcheckboxclickAction($(this)[0].children[0].children[0]);
        }

    });
    $("#MyQTable tr").dblclick(function () {
        //Jira CAP-1354
        if ($('#MyQTable').children().find('.highlight').length > 1) {
            alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#MyQTable tr').removeClass("highlight");

            //Jira CAP-1444
            if ($('#MovetoNxtProcess') != undefined && $('#MovetoNxtProcess')[0]?.disabled != undefined) {
                $('#MovetoNxtProcess')[0].disabled = true;
            }
            return false;
        }


        if (event.target.tagName != 'TH' && event.target.type != 'checkbox') {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                $(this)[0].children[0].children[0].checked = true;
                $(this)[0].classList.add('highlight');
            }

            MyQclick();
        }
//Jira #CAP-889
        sessionStorage.setItem('MyQRemoveIdList', '');
    });
    $("#GeneralQTable tr").dblclick(function () {
        //CAP-346 - Prevent Undefined.
        if (event?.target?.tagName != undefined && document.getElementById("RefreshQ")?.innerText?.indexOf("Refresh Encounters Q") != undefined) {
            if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Encounters Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
            else if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Task Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
        }
        
//Jira #CAP-889
        sessionStorage.setItem('MyQRemoveIdList', '');
    });
    $("#GeneralQTable tr").click(function () {

        if (event.target.tagName != 'INPUT') {
            if ($(this)[0].children[0].children[0].checked) {
                $(this).removeClass("highlight");
                $(this)[0].children[0].children[0].checked = false;
            }
            else {
                $(this).toggleClass("highlight");
                $(this)[0].children[0].children[0].checked = true;
            }

        }

    });
}
function movetoEnc() {

    var currentProcess = "";
    var inputData = new Array();
    var flag = 0;
    var tablelength = $('#GeneralQTable tr:has(td)').length;
    if ($('#GeneralQTable tr.highlight').length > 0) {
        $('#GeneralQTable tr.highlight').each(function (i, row) {


            // reference all the stuff you need first
            var $row = $(row);


            if ($row.find('td:nth-child(8)')[0] != undefined) {
                currentProcess = $row.find('td:nth-child(8)')[0].outerText.trim();


                if (currentProcess == 'MA_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'AKIDO_REVIEW_CODING' || currentProcess == 'TECHNICIAN_PROCESS'
                    || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'SURGERY_COORDINATOR_PROCESS' || currentProcess == 'SCRIBE_PROCESS' || currentProcess == 'AKIDO_SCRIBE_PROCESS' || currentProcess == 'TRANSCRIPT_PROCESS' || currentProcess == 'TRANSCRIPT_QC_PROCESS' || currentProcess == 'AKIDO_REVIEW_CODING_QC' || currentProcess == 'AKIDO_SCRIBE_QC_PROCESS') {
                    var objType = $row.find('td:nth-child(16)')[0].outerText.trim();
                    var ExamRoom = document.getElementById("Exam").value;
                    var now = new Date();
                    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
                    if (currentProcess == 'MA_PROCESS' || currentProcess == 'TECHNICIAN_PROCESS' || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'SCRIBE_PROCESS' || currentProcess == 'AKIDO_SCRIBE_PROCESS' || currentProcess == 'TRANSCRIPT_PROCESS' || currentProcess == 'TRANSCRIPT_QC_PROCESS' || currentProcess == 'AKIDO_SCRIBE_QC_PROCESS') {
                        var encounterID = $row.find('td:nth-child(14)')[0].outerText.trim();
                        var humanID = $row.find('td:nth-child(3)')[0].outerText.trim();//BugID:52865
                        inputData.push(Showall + "~" + currentProcess + "~" + encounterID + "~" + utc + "~" + objType + "~" + ExamRoom + "~" + humanID);
                    }
                    else {
                        var encounterID = $row.find('td:nth-child(14)')[0].outerText.trim();
                        inputData.push(Showall + "~" + currentProcess + "~" + encounterID + "~" + utc + "~" + objType);
                    }
                }

                else {

                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    alert('Selected encounter cannot be moved from General Queue.');
                    flag = 1;
                    $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
                        $(this).prop('checked', false);
                    });
                    $('#GeneralQTable th').find('input[type=checkbox]:checked').each(function () {
                        $(this).prop('checked', false);
                    });
                    $('#GeneralQTable tr').removeClass("highlight")
                    return false;
                }

            }
        });
        if (inputData.length > 0 && flag == 0) {
            var ViewAllFacilities = "";
            if ($("#ctl00_C5POBody_chkViewAllFacilities")[0] != undefined && $("#ctl00_C5POBody_chkViewAllFacilities")[0] != null) {
                $("#ctl00_C5POBody_chkViewAllFacilities")[0].checked ? ViewAllFacilities = "Checked" : ViewAllFacilities = "Unchecked";
            }
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/MoveToMyEncounters",
                data: JSON.stringify({
                    "data": inputData,
                    "sViewAllFacilities": ViewAllFacilities
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    LoadGeneralEncounter();
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //    "Message: " + log.Message);
                        ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                    }
                }
            });


        }
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        alert('Selected encounter cannot be moved from General Queue.');
    }
}
function movetoamend() {
    //GitLab#2246
    //var objID = $('#EncounterTable > tbody > tr.highlight > td:nth-child(16)')[0].innerText;
    //var objtype = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].innerText;
    //GitLab#2246
    var inputData = new Array();
    $('#EncounterTable > tbody > tr.highlight').each(function (i, row) {

        var $row = $(row);

        inputData.push($row.find('td:nth-child(13)')[0].innerText + "~" + $row.find('td:nth-child(14)')[0].innerText);
    });
    //GitLab#2246
    //var Data = [objtype, objID];
    // this parameter is change in GitLab#2246 data: JSON.stringify({
    //"data": inputData,
    //    }),
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/MoveToMyAmendment",
        data: JSON.stringify({
            "data": inputData,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
           // if (data.d != "[]") {
                //for (var i = 0; i < objdata.length; i++) {
                //    if (i == 0) {
                //        //GitLab#2246
                //        //tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                //        tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                //    } else {
                //        //GitLab#2246
                //        //tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                //        tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                //    }
                //}
                ////GitLab#2246
                ////$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
           // }
            //else
                //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
            //GitLab#2246
            //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
            // $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(*)";
           // $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";
            //sessionStorage.setItem("Amendmnt_Count", objdata.length);

           // $("#GeneralQTable tr").click(function () {
             //   var existingSelectedItem = $("#GeneralQTable tr.highlight"); if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
               // $(this).toggleClass("highlight");
            //});
            //$('#EncounterTable th').addClass('header');
            loadamend();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //    "Message: " + log.Message);
                ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
            }
        }
    });
}
function movetoorder() {
    var inputData = new Array();
    var ShowallGeneral = "";
    $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
    //Jira CAP-1123
    $('#EncounterTable > tbody > tr.highlight').each(function (i, row) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var $row = $(row);
        $row.find('td:nth-child(3)')
        var EncounterID = $row.find('td:nth-child(13)')[0].innerText;
        var OrderID = $row.find('td:nth-child(15)')[0].innerText;
        var ObjectType = $row.find('td:nth-child(16)')[0].innerText;
        var CurrentProcess = $row.find('td:nth-child(10)')[0].innerText;
        sShowall = ShowallGeneral;
        inputData.push(EncounterID + "~" + OrderID + "~" + ObjectType + "~" + sShowall + "~" + CurrentProcess);
    });
    if (inputData.length > 0) {
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/MoveToMyOrder",
            data: JSON.stringify({
                "data": inputData,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                
                //$('#GeneralQTable').empty();
                //var tabContents;
                //var objdata = $.parseJSON(data.d);
                //if (data.d != "[]") {
                //    for (var i = 0; i < objdata.length; i++) {
                //        var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
                //        if (i == 0) {
                //            if (objdata[i].Reason_For_Referral != "") {
                //                if (objdata[i].Referred_to != "")
                //                    tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                //                else
                //                    tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                //            }
                //            else {
                //                if (objdata[i].Referred_to != "")
                //                    tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                //                else
                //                    tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                //            }
                //        }
                //        else {
                //            if (objdata[i].Reason_For_Referral != "") {
                //                if (objdata[i].Referred_to != "")
                //                    tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                //                else
                //                    tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                //            }
                //            else {
                //                if (objdata[i].Referred_to != "")
                //                    tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                //                else
                //                    tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                //            }
                //        }
                //    }
                //    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;'>Order Date</th><th style='border: 1px solid #909090;'>Test Date</th><th style='border: 1px solid #909090;'>Acct. #</th><th style='border: 1px solid #909090;'>Ext. Acct. #</th><th style='border: 1px solid #909090;'>Patient Name</th><th style='border: 1px solid #909090;'>Patient DOB</th><th style='border: 1px solid #909090;'>Description</th><th style='border: 1px solid #909090;'>Ordering Physician</th><th style='border: 1px solid #909090;'>Current Process</th><th style='border: 1px solid #909090;'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                //}
                //else
                //    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;'>Order Date</th><th style='border: 1px solid #909090;'>Test Date</th><th style='border: 1px solid #909090;'>Acct. #</th><th style='border: 1px solid #909090;'>Ext. Acct. #</th><th style='border: 1px solid #909090;'>Patient Name</th><th style='border: 1px solid #909090;'>Patient DOB</th><th style='border: 1px solid #909090;'>Description</th><th style='border: 1px solid #909090;'>Ordering Physician</th><th style='border: 1px solid #909090;'>Current Process</th><th style='border: 1px solid #909090;'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");
                //$("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.length + ")";
                //if (ShowallGeneral != "Checked") {
                //    sessionStorage.setItem("Order_Count", objdata.length);
                //}

                //localStorage.setItem("GenralOrderCount", objdata.length);
                ////$('#EncounterTable th').addClass('header');
                ////Jira CAP-1431
                //RowClick();

                loadorder();
                //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }

        });
    }
}
function movetotask() {
    var inputData = new Array();
    $('#EncounterTable > tbody > tr.highlight').each(function (i, row) {

        var $row = $(row);

        inputData.push("TASK" + "~" + $row.find('td:nth-child(9)')[0].innerText);
    });
    
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/MoveToMyTask",
        data: JSON.stringify({
            "data": inputData,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            //var tabContents;
            //var objdata = $.parseJSON(data.d);
            loadTask();
            //$('#EncounterTable th').addClass('header');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //    "Message: " + log.Message);
                ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
            }
        }
    });
}
function MoveToClick(sender) {

    var Showall = '';
    $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    if (document.getElementById("MoveTo").innerText.indexOf("Move To My Encounters") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {
            window.setTimeout(movetoEnc, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an encounter to process.");
        }
    }
    else if (document.getElementById("MoveTo").innerText.indexOf("Move To My Amendment") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {

            window.setTimeout(movetoamend, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an encounter to process.");
        }
    }
    else if (document.getElementById("MoveTo").innerText.indexOf("Move To My Orders") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {

            window.setTimeout(movetoorder, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an encounter to process.");
        }
    }
    else if (document.getElementById("MoveTo").innerText.indexOf("Move To My Task") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {

            window.setTimeout(movetotask, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an task to process.");
        }
    }
}
function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    //CAP-3378
    //var now = new Date(utcDate + ' UTC');
    var now = new Date(utcDate.toString().replace(' ', 'T') + 'Z');
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-01-0001';
    else
        then = ('0' + now.getDate().format("dd")).slice(-2) + '-' + monthNames[now.getMonth()] + '-' + now.getFullYear();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = ('0' + hours).slice(-2) + ':' + minutes + ' ' + ampm;
    if (utcDate != '0001-01-01 00:00:00')
        then += ' ' + strTime;
    return then;
}
function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    oTbl.attr("data-item-original-width", oTbl.width());
    oTbl.find('thead tr td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    newTbl.width(newTbl.attr('data-item-original-width'));
    newTbl.find('thead tr td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    oTbl.width(oTbl.attr('data-item-original-width'));
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    if (tblAsJQueryObject[0] != undefined) {
        if (tblAsJQueryObject[0].parentElement.parentElement.id == "GeneralQTable") {
            $("#ScrollIDGeneral").css('height', '');
            $("#ScrollIDGeneral").css('overflow-y', '');
        }
        else {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
        }
    }
}

function DOBConvert(DOB) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
}
function DateConversion(date) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = date.split('-');
    return SplitDOB[2] + "" + ((monthNames.indexOf(SplitDOB[1]) < 10) ? (0 + "" + monthNames.indexOf(SplitDOB[1])) : (monthNames.indexOf(SplitDOB[1]))) + "" + SplitDOB[0];
}
function SortCol(Col, tdArray) {
    var isSame = true;
    if (Col == 'd') {
        for (i = 0; i < tdArray.length; i++) {
            if (tdArray[i].innerText != "") {
                var date = tdArray[i].innerText.split(' ')[0];
                date = DateConversion(date);
                if (tdArray[i].innerText.split(' ')[1] != undefined) {
                    //CAP-281 - null handling in array
                    var t1 = tdArray[i]?.innerText?.split(' ')[1]?.replace(':', '');
                    var t2 = tdArray[i]?.innerText?.split(' ')[2]?.replace('A', 0).replace('P', 1).replace('M', '');
                    tdArray[i].innerText = date + "" + t1 + "" + t2;
                }
                else
                    tdArray[i].innerText = date;
            }
            else
                tdArray[i].innerText = "0000000000000";
        }
        tdArray.sort(function (p, n) {
            var pData = parseInt($(p).text().trim());
            var nData = parseInt($(n).text().trim());
            if (pData != nData)
                isSame = false;
            return pData - nData;
        });
        for (i = 0; i < tdArray.length; i++) {
            if (tdArray[i].innerText != "0000000000000") {
                var y = tdArray[i].innerText.substring(0, 4);
                var m = tdArray[i].innerText.substring(4, 6);
                var d = tdArray[i].innerText.substring(6, 8);
                var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                if (m.substring(0, 1) == "0")
                    m = m.substring(1, 2);
                m = monthNames[m];
                if (tdArray[i].innerText.substring(8, 10) != "") {
                    var t1 = tdArray[i].innerText.substring(8, 10) + ":" + tdArray[i].innerText.substring(10, 12);
                    if (tdArray[i].innerText.substring(12, 13) == "0")
                        var t2 = "AM";
                    else
                        if (tdArray[i].innerText.substring(12, 13) == "1")
                            var t2 = "PM";
                    tdArray[i].innerText = d + "-" + m + "-" + y + " " + t1 + " " + t2;
                }
                else
                    tdArray[i].innerText = d + "-" + m + "-" + y;
            }
            else
                tdArray[i].innerText = "";
        }
    }
    if (Col == 'i') {
        tdArray.sort(function (p, n) {
            var pData = parseInt($(p).text().trim());
            var nData = parseInt($(n).text().trim());
            if (pData != nData)
                isSame = false;
            return pData - nData;
        });
    }
    else
        if (Col == 's') {
            tdArray.sort(function (p, n) {
                var pData = $(p).text().trim().replace(/<[^>]+>/g, '');;
                var nData = $(n).text().trim().replace(/<[^>]+>/g, '');;
                if (pData != nData)
                    isSame = false;
                if (pData < nData)
                    return 1;
                else if (pData > nData)
                    return -1;
                else
                    return 0;
            });
        }
    var sArray = [tdArray, isSame];

    return sArray;
}
function SortTableHeader(s) {
    var Header;
    if (s == 'MyQ' || s == 'MyQTask' || s == 'MyQOrder' || s == 'MyQScan' || s == 'MyQPrescription' || s == 'MyQAmendment')
        Header = $("#MyQTable th");
    else
        if (s == 'GeneralQOrder' || s == 'GeneralQ' || s == 'GeneralQScan' || s == 'GeneralQAmendment' || s == 'GeneralQTask')
            Header = $("#GeneralQTable th");
    for (var i = 0; i < Header.length; i++)
        Header[i].title = "Click here to sort";

    scrolify($('#EncounterTable'), 600);
    $("#MyQTable th").click(function () {
        var table = document.createElement("table");
        switch (s) {
            case 'MyQ': billQ = ['', 'd', 'i', 'i', 's', 'd', 's', 's', 's', 's', 's', 's','s','i']; break;
            case 'MyQTask': billQ = ['i', 'i', 's', 'd', 'd', 's', 's', 's', 's', 'd']; break;
            case 'MyQOrder': billQ = ['d', 'n', 'i', 'i', 's', 'd', 's', 's', 's', 's', 's', 's', 'n', 'n', 'n', 'n', 'n', 'n', 'n', 'n', 'n', 's']; break;
            case 'MyQScan': billQ = ['s', 'i', 'd', 's', 's', 's']; break;
            case 'MyQPrescription': billQ = ['d', 's', 'i', 'i', 's', 'd', 's']; break;
            case 'MyQAmendment': billQ = ['d', 'd', 's', 'i', 'i', 's', 'd', 's', 's', 'd', 's', 's']; break;
        }

        var columnIndex = $(this).index();
        var Col = billQ[columnIndex];
        var tdArray = $("#MyQTable tbody tr td:nth-child(" + (columnIndex + 1) + ")");
        var isSame = true;
        sArray = SortCol(Col, tdArray);
        tdArray = sArray[0];
        isSame = sArray[1];
        if ($(this)[0].className == "rgSortedAsc") {
            if (isSame == false) {
                for (i = tdArray.length - 1; i >= 0; i--) {
                    var row = tdArray[i].parentElement;
                    if (row[0] == undefined)
                        table.appendChild(row);
                    else
                        table.appendChild(row[0]);
                }
                $("#MyQTable tbody").empty();
                $("#dvAdd table tbody").append(table.rows);
            }

            $("#MyQTable").find("tr th").removeClass('rgSortedDesc');
            $("#MyQTable").find("tr th").removeClass('rgSortedAsc');
            $("#MyQTable").find("tr td").removeClass('ColSorted');
            $(this).addClass('rgSortedDesc');
        }
        else if ($(this)[0].className == "") {
            tdArray.each(function () {
                var row = $(this).parent();
                if (row[0] == undefined)
                    table.appendChild(row);
                else
                    table.appendChild(row[0]);
            });
            $("#MyQTable tbody").empty();
            $("#dvAdd table tbody").append(table.rows);
            $("#MyQTable").find("tr th").removeClass('rgSortedDesc');
            $("#MyQTable").find("tr th").removeClass('rgSortedAsc');
            $("#MyQTable").find("tr td").removeClass('ColSorted');
            $(this).addClass('rgSortedAsc');
        }
        else {
            if (isSame == false) {
                tdArray.each(function () {
                    var row = $(this).parent();
                    if (row[0] == undefined)
                        table.appendChild(row);
                    else
                        table.appendChild(row[0]);
                });
                $("#MyQTable tbody").empty();
                $("#dvAdd table tbody").append(table.rows);
            }
            $("#MyQTable").find("tr th").removeClass('rgSortedDesc');
            $("#MyQTable").find("tr th").removeClass('rgSortedAsc');
            $("#MyQTable").find("tr td").removeClass('ColSorted');
            $(this).addClass('rgSortedAsc');
        }
        $("#MyQTable").find("tbody tr td:nth-child(" + (columnIndex + 1) + ")").addClass('ColSorted');
    });
    $("#GeneralQTable th").click(function () {
        if (event.target.tagName != 'INPUT') {
            var table = document.createElement("table");
            switch (s) {
                case 'GeneralQ': billQ = ['', 'd', 'i', 'i', 's', 'd', 's', 's', 's', 's', 's', 's','s','i']; break;
                case 'GeneralQOrder': billQ = ['d', 'd', 'i', 'i', 's', 'd', 's', 's', 's', 's']; break;
                case 'GeneralQScan': billQ = ['s', 'i', 'd', 's', 's', 's']; break;
                case 'GeneralQAmendment': billQ = ['d', 'd', 's', 'i', 'i', 's', 'd', 's', 's', 'd', 's', 's']; break;
                case 'GeneralQTask': billQ = ['','s', 'i', 's', 'd', 's', 's', 's']; break;
            }
            var columnIndex = $(this).index();
            var Col = billQ[columnIndex];
            var tdArray = $("#GeneralQTable tbody tr td:nth-child(" + (columnIndex + 1) + ")");
            var isSame = true;
            sArray = SortCol(Col, tdArray);
            tdArray = sArray[0];
            isSame = sArray[1];
            if ($(this)[0].className == "rgSortedAsc") {
                if (isSame == false) {
                    for (i = tdArray.length - 1; i >= 0; i--) {
                        var row = tdArray[i].parentElement;
                        if (row[0] == undefined)
                            table.appendChild(row);
                        else
                            table.appendChild(row[0]);
                    }
                    $("#GeneralQTable tbody").empty();
                    $("#dvAdd table tbody").append(table.rows);
                }
                $("#GeneralQTable").find("tr th").removeClass('rgSortedDesc');
                $("#GeneralQTable").find("tr th").removeClass('rgSortedAsc');
                $("#GeneralQTable").find("tr td").removeClass('ColSorted');
                $(this).addClass('rgSortedDesc');
            }
            else if ($(this)[0].className == "") {
                tdArray.each(function () {
                    var row = $(this).parent();
                    if (row[0] == undefined)
                        table.appendChild(row);
                    else
                        table.appendChild(row[0]);
                });
                $("#GeneralQTable tbody").empty();
                $("#dvAdd table tbody").append(table.rows);
                $("#GeneralQTable").find("tr th").removeClass('rgSortedDesc');
                $("#GeneralQTable").find("tr th").removeClass('rgSortedAsc');
                $("#GeneralQTable").find("tr td").removeClass('ColSorted');
                $(this).addClass('rgSortedAsc');
            }
            else {
                if (isSame == false) {
                    for (i = tdArray.length - 1; i >= 0; i--) {
                        var row = tdArray[i].parentElement;
                        if (row[0] == undefined)
                            table.appendChild(row);
                        else
                            table.appendChild(row[0]);
                    }
                    $("#GeneralQTable tbody").empty();
                    $("#dvAdd table tbody").append(table.rows);
                }
                $("#GeneralQTable").find("tr th").removeClass('rgSortedDesc');
                $("#GeneralQTable").find("tr th").removeClass('rgSortedAsc');
                $("#GeneralQTable").find("tr td").removeClass('ColSorted');
                $(this).addClass('rgSortedAsc');
            }
            $("#GeneralQTable").find("tbody tr td:nth-child(" + (columnIndex + 1) + ")").addClass('ColSorted');
        }
    });


}
function MyQcheckboxclick(evt) {
    //Jira CAP-1201
    //if (evt.checked) {
    //    //$(evt).closest("tr").addClass("highlight");
    //    $("#MovetoNxtProcess")[0].disabled = false;
    //    // isproviderReview = true;
    //}
    //if ($("input:checkbox[class=myQChkbx]:enabled:checked").length == 0) {
    //    //$(evt).closest("tr").removeClass("highlight");
    //    $("#MovetoNxtProcess")[0].disabled = true;
    //    //isproviderReview = false;
    //}

    //Jira CAP-1201
    if (evt.checked) {
        evt.checked = false;
    }
    else {
        evt.checked = true;
    }

}
//Jira CAP-1201
function MyQcheckboxclickAction(evt) {
    if (evt.checked) {
        //$(evt).closest("tr").addClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = false;
        // isproviderReview = true;
    }
    if ($("input:checkbox[class=myQChkbx]:enabled:checked").length == 0) {
        //$(evt).closest("tr").removeClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = true;
        //isproviderReview = false;
    }
}
function MyQselectAll(chkbxAll) {
    if (chkbxAll.checked) {
        $("input:checkbox[class=myQChkbx]:enabled").prop("checked", "true");
        //$('#MyQTable td').find('input[type=checkbox]').prop('checked', 'checked');
        $("input:checkbox[class=myQChkbx]:enabled").each(function () {
            $(this).prop('checked', true);
            $(this).closest("tr").addClass("highlight");
        });
        //$('#MyQTable tr').addClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = false;
        isproviderReview = true;

        //return false;
    }
    else {
        //$("input:checkbox[class=myQChkbx]:enabled").removeProp("checked");
        // $("input:checkbox[class=myQChkbx]:enabled").removeProp("checked");

        $("input:checkbox[class=myQChkbx]:enabled").each(function () {
            $(this).prop('checked', false);
            $(this).closest("tr").removeClass("highlight");
        });
        //$('#MyQTable tr').removeClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = true;
        isproviderReview = false;

        return false;
    }
}
function PerformMovetoNextProcess() {
    let lstHumanID = [];
    let lstEncounterID = [];

    if ($("input:checkbox[class=myQChkbx]:enabled:checked").length > 0) {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        let chkLstenctoMove = $("input:checkbox[class=myQChkbx]:enabled:checked");
        for (let i = 0; i < chkLstenctoMove.length; i++) {
            lstHumanID.push($(chkLstenctoMove[i].parentElement.parentElement)[0].cells[2].innerText);
            lstEncounterID.push($(chkLstenctoMove[i].parentElement.parentElement)[0].cells[13].innerText);
        }
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/PerformMovetoNextProcess",
            data: "{ 'HumanIDlst': '" + lstHumanID + "','EncIDlst': '" + lstEncounterID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                RefreshMyQueue();
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        });
    }
    else {
        alert("Please select an encounter to process.");
    }
}
function disableSelectAllMove() {
    //CAP-3406: Capturing the element in a separate variable for the sanity check to avoid the undefined reference exception
    var checkboxElements = $('.myQChkbxAll');

    if (checkboxElements?.length > 0 && checkboxElements[0]?.disabled != null && checkboxElements[0]?.disabled != undefined) {
        checkboxElements[0].disabled = true;
    }
}

function QRCodeClick(evt) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var Human_id = "";
    var Encounter_id = "";
    var DOS = "";
    var Physician_id = "";

    var node = evt?.parentElement?.parentElement?.childNodes;
    if (node[6] != undefined && node[6] != null && node[6].innerText.trim() == 'TECHNICIAN_PROCESS') {

        Human_id = node[2].innerText;
        Encounter_id = node[10].innerText;
        DOS = node[13].innerText;
        Physician_id = node[11].innerText;
    }
    else {
        Human_id = node[2].innerText;
        Encounter_id = node[13].innerText;
        DOS = node[16].innerText;
        Physician_id = node[14].innerText;
    }

    if (window.top.document.getElementById("notificationpopup").innerText != "NOTIFICATION : Loading...") {
        if ($(top.window.document).find("#QRCodeInfo") != undefined) {
            var locatn = "frmQRCodeGenerator.aspx?HumanID=" + Human_id + "&EncounterID=" + Encounter_id + "&PhysicianID=" + Physician_id + "&DOS=" + DOS;
            $(top.window.document).find('#QRCode_Modal')[0].contentDocument.location.href = locatn;


            $(top.window.document).find("#QRCodeInfo")[0].style.display = "block";
            $(top.window.document).find("#QRCodeInfo").css('background-color', '#00000036');
            $(top.window.document).find("#QRCodeInfo").css('opacity', '1');
            $(top.window.document).find("#QRCodeHeader")[0].innerHTML = "Scan QR code with your mobile device to dictate";
            $(top.window.document).find("#btnCloseQRCode").css("display", "block");
            $('#btnCloseQRCode').removeClass('btn btn-danger');
            $('#btnCloseQRCode').addClass('aspresizedredbutton');

        }
    }
}
function Decompress(data) {
    // Decode the Base64 string
    const binaryString = window.atob(data);
    // Convert binary string to byte array
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    // Use pako to decompress the byte array
    const decompressed = pako.inflate(bytes, { to: 'string' });
    return JSON.parse(decompressed);
}

function SetHeightForTabelBasedOnScreenSize() {
    window.setTimeout(function () {
        //CAP-2990
        var launcher = document?.getElementById("launcher");
        var dataTableBody = document?.getElementsByClassName("dataTables_scrollBody")[0];
        if (launcher && dataTableBody) {
            var launcherRect = launcher?.getBoundingClientRect();
            var dataTableRect = dataTableBody?.getBoundingClientRect();
            if (launcherRect?.top != undefined && launcherRect?.height != undefined && dataTableRect?.top != undefined) {
                var MaxHeightTable = (launcherRect.top - dataTableRect.top) + (launcherRect.height - 10);
                if (MaxHeightTable != undefined) {
                    if ($(".dataTables_scrollBody")?.css("max-height") != undefined
                        && parseInt($(".dataTables_scrollBody").css("max-height")) < MaxHeightTable) {
                        $(".dataTables_scrollBody").css({ "max-height": MaxHeightTable + "px" });
                    }
                }
            }
        }
    }, 1000);
}
//CAP-4025
function OpenMyAkidoTasksClick() {
    var myAkidoTasksURL = document.getElementById('hdnMyAkidoTasksURL').value;

    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();

    window.open(myAkidoTasksURL, '_blank');

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}