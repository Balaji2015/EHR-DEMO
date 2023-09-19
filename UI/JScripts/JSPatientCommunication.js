var FacilityName = "";
var DiffUser = "";
var AssignedTo = "";
var Result;
var obj = new Array();
function btnpatientChart_Click() {
    var queryString = window.location.search.toString().split('?')[1];
    if (queryString != undefined && queryString != "") {
        humanid = queryString.split("&")[0].split('=')[1];
    }
    //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 780, 1250, obj);//BugID:45892
    //Cap - 891
    Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 780, 1250, obj);//BugID:45892
    //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&ScreenMode=Menu&openingfrom=Menu&from=viewresult", 780, 1250, obj);

    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;

    return false;
}
function openNonModal(fromname, height, width, inputargument) {
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
        if (inputargument.lenght != 0) {
            PageName = PageName + "?";
        }
    }
    //Jira #CAP-903
    if (PageName != undefined && PageName != null) {
        if (PageName.indexOf('?') > -1) {
            PageName = PageName + Argument + "&allowmultipletab=true";
        }
        else {
            PageName = PageName + "?allowmultipletab=true";
        }
    }

    var windowop = window.open(PageName, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes,titlebar=no,toolbar=no");
    if (windowop != null)
        windowop.moveTo(30, 150);

    if (windowop == undefined) { windowop = window.returnValue; }
    return windowop;
}
function btnPrint_Clicked() {
    var strHumanId = 0;
    if ($('#txtAccount').val() != undefined && $('#txtAccount').val() != "") {
        strHumanId = $('#txtAccount').val();
    }
    var QuryString = window.location.search;
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/PrintGrid",
        data: JSON.stringify({
            "strHumanId": strHumanId
        }),
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
            event.stopPropagation();
            event.stopImmediatePropagation();
            return false;
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = xhr.statusText;
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

function btnePrescribe_Click() {
    var obj = new Array();
    obj.push("MyType=" + "GENERAL");
    obj.push("HumanID=" + document.getElementById("txtAccount").value);
    obj.push("EncID=" + "0");
    obj.push("PrescriptionID=" + "0");
    obj.push("IsMoveButton=" + "true");
    obj.push("IsMoveCheckbox=" + "false");
    obj.push("IsPrescriptiontobePushed=" + "N");
    obj.push("openingFrom=" + "Menu");
    obj.push("IsSentToRCopia=" + "Y");
    obj.push("LocalTime=" + document.getElementById(GetClientId('hdnLocalTime')).value);
    //Jira #CAP-903
    Result = openNonModal("frmRCopiaWebBrowser.aspx", 535, 860, obj, 'ctl00_ModalWindow');
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;

    return false;
}

function SaveMenuClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("[id*='lblCallerName']")[0].innerHTML == "Spoken To*" && $("[id*='txtCallerName']")[0].value.trim() == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580012');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if (document.getElementById("ddlRelationship").value != "Self" && document.getElementById("ddlRelationship").value != "" && document.getElementById("txtCallerName").value == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580012');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if ($("#ddlType option:selected").text() == "TASK" && $("#ddlAssignedTo option:selected").text() == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580007');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    else if ($("#ddlMessageType option:selected").text() == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if (document.getElementById("DLC_txtDLC").value == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580009');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if ($("#ddlRelationship option:selected").text() == "Self" && $("#ddlRelationship option:selected").text() == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580012');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if ($("[id*='lblCallerName']")[0].innerHTML == "Spoken To*" && $("[id*='txtCallerName']")[0].value.trim() == "") {
        localStorage.setItem("bSaveSuccess", "");
        localStorage.setItem("bSave", "false");
        DisplayErrorMessage('7580012');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        var chkPatientChart = "";
        if (document.getElementById("ChkPatientChart") != null && document.getElementById("ChkPatientChart").checked == true)
            chkPatientChart = "Y";
        else
            chkPatientChart = "N";
        var ParentScr = "";
        if (GetParameterValues('parentscreen') != null)
            ParentScr = GetParameterValues('parentscreen');
        var StartTime = "";
        if (GetParameterValues('StartTime') != null)
            StartTime = GetParameterValues('StartTime');
        $.ajax({
            type: "POST",
            url: "frmPatientCommunication.aspx/SaveMenuClick",
            data: JSON.stringify({
                "AccNo": document.getElementById("txtAccount").value,
                "AssignedTo": $("#ddlAssignedTo option:selected").val(),
                "RelationShip": $("#ddlRelationship option:selected").text(),
                "FacilityName": $("#ddlFacilityName option:selected").text(),
                "CallerName": document.getElementById("txtCallerName").value,
                "MessageOrigin": $("#ddlMessageOrigin option:selected").text(),
                "Priority": $("#ddlPriority option:selected").text(),
                "MessageType": $("#ddlMessageType option:selected").text(),
                "DLC": document.getElementById("DLC_txtDLC").value,
                "Type": $("#ddlType option:selected").text(),
                "MessageDate": $('#txtMessageDate')[0].value,
                "PatientChart": chkPatientChart,
                "ParentScreen": ParentScr,
                "StartTime": StartTime,
                "sBtnText": $('#btnSaveMenu').val(),//$("[id*='btnSaveMenu']")[0].innerText,
                "sMessageID": sessionStorage.getItem('messageid'),
                "WFOBJID": sessionStorage.getItem('WFOBJID'),
                "DateTime": new Date()
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                DisplayErrorMessage('7580006');
                FillMessageGrid();
                AfterSaveClear();
                Assingnmethod();
                localStorage.setItem('bSave', 'true');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = xhr.statusText;
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
    return false;
}
function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}
function SaveClick(sender) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#ddlAssignedTo option:selected").text() == "" && sender.defaultValue != "Task Complete") {
        DisplayErrorMessage('390009');
        localStorage.setItem("bSaveSuccess", "");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if ($("#ddlMessageType option:selected").text() == "") {
        DisplayErrorMessage('390016');
        localStorage.setItem("bSaveSuccess", "");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if ($("#DLC_txtDLC").val() == "" && sender.defaultValue != "Task Complete") {
        alert("Please enter Message Notes");
        localStorage.setItem("bSaveSuccess", "");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        var chkPatientChart = "";
        if (document.getElementById("ChkPatientChart") != null && document.getElementById("ChkPatientChart").checked == true)
            chkPatientChart = "Y";
        else
            chkPatientChart = "N";
        var MessageId = "";
        if (GetParameterValues('MessageID') != null)
            MessageId = GetParameterValues('MessageID');
        var StartTime = "";
        if (GetParameterValues('StartTime') != null)
            StartTime = GetParameterValues('StartTime');
        $.ajax({
            type: "POST",
            url: "frmPatientCommunication.aspx/SaveMyQClick",
            data: JSON.stringify({
                "AccNo": document.getElementById("txtAccount").value,
                "AssignedTo": $("#ddlAssignedTo option:selected").val(),
                "RelationShip": $("#ddlRelationship option:selected").text(),
                "FacilityName": $("#ddlFacilityName option:selected").text(),
                "CallerName": document.getElementById("txtCallerName").value,
                "MessageOrigin": $("#ddlMessageOrigin option:selected").text(),
                "Priority": $("#ddlPriority option:selected").text(),
                "MessageType": $("#ddlMessageType option:selected").text(),
                "DLC": document.getElementById("DLC_txtDLC").value,
                "Type": $("#ddlType option:selected").text(),
                "MessageDate": $('#txtMessageDate')[0].value,
                "PatientChart": chkPatientChart,
                "Button": sender.defaultValue,
                "MessageID": MessageId,
                "StartTime": StartTime,
                "HistoryNotes": document.getElementById("txtmsghistory").value,
                "datetime": new Date()
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (sender.defaultValue == "Task Complete" || sender.defaultValue == "Send") {
                    //Jira #CAP-889
                    var sCheckAssigned = data.d;
                    if (sender.id == 'btnSaveSendMyQ' && document.URL.indexOf('GenQTask')>-1) {
                        RemoveItem(document.URL, "MessageID");
                    }
                    else if (sender.id == 'btnSaveSendMyQ' && sCheckAssigned == '"true"') {
                        RemoveItem(document.URL, "MessageID");
                    }
                    else if (sender.id == 'btnSaveCompletedMyQ') {
                        RemoveItem(document.URL, "MessageID");
                    }
                    

                    if (Result != undefined) {
                        if (false == Result.closed) {

                            Result.close();
                        }
                    }
                    DisplayErrorMessage('7580006');
                    self.close();
                }
                else {
                    DisplayErrorMessage('7580006');
                    if (data.d != "") {
                        var objdata = $.parseJSON(data.d);
                        document.getElementById("DLC_txtDLC").value = "";
                        if (objdata.indexOf("@") <= -1) {
                            var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
                            var vCreated_Date = objdata[0].Created_Date_And_Time.replace("T", " ").split('-');
                            if (vCreated_Date[1].substring(0, 1) == "0")
                                vCreated_Date[1] = vCreated_Date[1].slice(-1);

                            var hours = vCreated_Date[2].split(' ')[1].split(':')[0];
                            var ampm = hours >= 12 ? "PM" : "AM"
                            hours = hours % 12;
                            hours = hours > 0 ? hours : 12; // the hour '0' should be '12'
                            var minutes = '';
                            if (vCreated_Date[2].split(' ')[1].split(':').length > 1)
                                minutes = vCreated_Date[2].split(' ')[1].split(':')[1];

                            var strTime = hours + ':' + minutes + ' ' + ampm;

                            var vCRT = vCreated_Date[2].split(' ')[0] + "-" + arr[parseInt(vCreated_Date[1]) - 1] + "-" + vCreated_Date[0] + " " + strTime;
                            document.getElementById("txtmsghistory").value = "@" + objdata[0].Created_By + "(" + vCRT + "): " + objdata[0].Notes;

                        }
                        else {
                            document.getElementById("txtmsghistory").value = objdata.replace("<br />", "\n");
                        }
                    }

                    if (localStorage.getItem("bSaveSuccess") == "true") {
                        if (Result != undefined) {
                            if (false == Result.closed) {

                                Result.close();
                            }
                        }
                        self.close();
                        localStorage.setItem('bSaveSuccess', '');
                    }
                    else {
                        $("[id*='btnSaveMyQ']")[0].disabled = true;
                        localStorage.setItem("bSave", "")
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = xhr.statusText;
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

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    return false;
}
function CancelForPatientCommunication(sender) {
    if (document.getElementById(GetClientId("btnSaveMenu")).disabled == false && document.getElementById("RowForHide").style.display == "none") {
        if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
            DisplayErrorMessage('7580013');
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
            document.getElementById("btnSaveMenu").click();
            document.getElementById(GetClientId("btnSaveMenu")).disabled = false;
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            self.close();
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            return false;
        }
    }
    else if (document.getElementById(GetClientId("btnSaveMyQ")).disabled == false) {
        if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
            DisplayErrorMessage('9093040');
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
            document.getElementById(GetClientId("btnSaveMyQ")).disabled = false;
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            self.close();
            window.location.reload();
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            return false;
        }
    }
    else {
        self.close();
    }
}
function TypeChange() {
    if ($("#ddlType option:selected").text() == "TASK") {
        document.getElementById("ddlRelationship").disabled = false;
        document.getElementById("ddlRelationship").style.backgroundColor = 'White !important';
        document.getElementById("txtCallerName").disabled = false;
        document.getElementById("txtCallerName").style.backgroundColor = 'White !important';
        document.getElementById("ddlMessageOrigin").disabled = false;
        document.getElementById("ddlMessageOrigin").style.backgroundColor = 'White !important';
        document.getElementById("ddlAssignedTo").disabled = false;
        document.getElementById("ddlAssignedTo").style.backgroundColor = 'White !important';
        document.getElementById("lblassignedto").innerText = "Assigned To*";
        document.getElementById("ddlMessageType").disabled = false;
        document.getElementById("ddlMessageType").style.backgroundColor = 'White !important';
        document.getElementById("chkshowall").disabled = true;
        $('#lblMessageType').removeClass('spanstyle');
        $('#lblMessageType').addClass('MandLabelstyle');
        document.getElementById("lblMessageTypeStar").style.display = 'block';
    }
    else if ($("#ddlType option:selected").text() == "MESSAGE") {
        document.getElementById("ddlRelationship").disabled = true;
        $('#ddlRelationship').addClass('nonEditabletxtbox');
        document.getElementById("txtCallerName").disabled = true;
        $('#ddlRelationship').addClass('nonEditabletxtbox');
        document.getElementById("ddlMessageOrigin").disabled = true;
        $('#txtCallerName').addClass('nonEditabletxtbox');
        document.getElementById("ddlAssignedTo").disabled = true;
        $('#ddlMessageOrigin').addClass('nonEditabletxtbox');
        $('#ddlAssignedTo').addClass('nonEditabletxtbox');
        document.getElementById("lblassignedto").innerText = "Assigned To";
        document.getElementById("lblassignedto").style.color = 'Black';
        document.getElementById("ddlMessageType").disabled = true;
        $('#ddlRelationship').addClass('ddlMessageType');
        document.getElementById("chkshowall").disabled = true;
        $('#lblMessageType').removeClass('MandLabelstyle');
        $('#lblMessageType').addClass('spanstyle');
        document.getElementById("lblMessageTypeStar").style.display = 'none';
    }
    if (($("#ddlRelationship option:selected").text() != "Self") && ($("#ddlRelationship option:selected").text() != "")) {
        document.getElementById("lblCallerName").style.color = "Red";
        $('#lblCallerName').html($('#lblCallerName').html().replace("*", "<span class='manredforstar'>*</span>"));
    }
    else {
        document.getElementById("lblCallerName").style.color = "Black";
        $(this).html('Spoken To');
    }
    document.getElementById("btnSaveMenu").disabled = false;
}
function RelationChange() {
    document.getElementById("lblCallerName").innerHTML = "Spoken To*";
    if ($("#ddlRelationship option:selected").text() != "" && $("#ddlRelationship option:selected").text() != "Self") {
        $('#lblCallerName').removeClass('spanstyle');
        $('#lblCallerName').addClass('MandLabelstyle');
        $('#lblCallerName').html($('#lblCallerName').html().replace("*", "<span class='manredforstar'>*</span>"));


        document.getElementById("txtCallerName").disabled = false;
        $('#txtCallerName').removeClass('nonEditabletxtbox');
        $('#txtCallerName').addClass('Editabletxtbox');


        document.getElementById("txtCallerName").value = "";
    }
    else {
        $('#txtCallerName').removeClass('Editabletxtbox');
        $('#lblCallerName').removeClass('MandLabelstyle');
        $('#txtCallerName').addClass('nonEditabletxtbox');
        document.getElementById("lblCallerName").style.color = "Black";
        $('#lblCallerName').html('Spoken To');
        document.getElementById("lblCallerName").innerHTML = "Spoken To";
        document.getElementById("txtCallerName").disabled = true;
        if ($("#ddlRelationship option:selected").text() != "" && document.getElementById('divPatientstrip').outerText.includes('|') == true) {
            document.getElementById("txtCallerName").value = document.getElementById('divPatientstrip').outerText.split('|')[0];
        }
        else
            document.getElementById("txtCallerName").value = "";
    }
    document.getElementById("btnSaveMenu").disabled = false;
    localStorage.setItem("bSave", "false");
}
function FacilityLoad() {
    var markers = null;
    $.get("Facility_Library.xml", {}, function (xml) {
        $('Facility', xml).each(function (i) {
            markers = $(this);
        });
    });
}
var Facility = "";
function FacilityChange(sender) {
    Facility = sender.selectedOptions[0].innerText;
    document.getElementById("ddlAssignedTo").options.length = 0;
    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
    var checked = "false";
    var vfacilitys = "";
    if (document.getElementById("chkshowall").checked)
        checked = "true";
    var vfacility = $("[id*='ddlFacilityName']");
    document.getElementById("ddlAssignedTo").options.length = 0;
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/laodAssigned",
        data: JSON.stringify({
            "chkshowall": checked,
            "facility": Facility,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.AssignedTo.length > 0) {
                var vAssignedTo = objdata.AssignedTo;
                if (vAssignedTo != null && vAssignedTo.length > 0) {
                    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
                    var vddlAssignedTo = document.getElementById("ddlAssignedTo");
                    var opt = document.createElement("option");
                    for (var i = 0; i < vAssignedTo.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = vAssignedTo[i].split('|')[1];
                        opt.value = vAssignedTo[i].split('|')[0];
                        opt.title = vAssignedTo[i].split('|')[1];
                        vddlAssignedTo.options.add(opt);
                    }
                }
            }
        },
        failure: function (data) {
            alert(data.d);
        }
    });

    document.getElementById("btnSaveMenu").disabled = false;
    localStorage.setItem("bSave", "false");
    return false;
}
function FacilityChanges(Facility) {
    document.getElementById("ddlAssignedTo").options.length = 0;
    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
    $.get("ConfigXML/PhysicianFacilityMapping.xml", {}, function (xml) {
        $("PhyList", xml).each(function (i) {
            $(this).find("Facility").each(function (l) {
                if ($(this).attr("name") == Facility) {
                    $(this).find('Physician').each(function (k) {
                        if ($(this).attr('username') != undefined)
                            $('#ddlAssignedTo').append("<option value=''>" + $(this).attr('username') + "</option>");
                    });
                }
            });
        });


        var vddlAssignedTo = $("[id*='ddlAssignedTo']")[0].options;
        if (vddlAssignedTo.length > 0) {
            for (var i = 0; i < vddlAssignedTo.length; i++) {
                if (vddlAssignedTo[i].innerText == row.cells[7].innerHTML) {
                    vddlAssignedTo.selectedIndex = i;
                    break;
                }
            }
        }
    });
    document.getElementById("btnSaveMenu").disabled = false;
}
function SearchClick() {
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/SearchClick",
        data: JSON.stringify({
            "Description": $("#ddlMessageDescription option:selected").text(),
            "Notes": document.getElementById("txtMessageNotes").value,
            "AccNo": document.getElementById("txtAccount").value,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var GridView = document.getElementById('grdMessages');
            $("#grdMessages").find("tr:gt(0)").remove();
            if (objdata != null && objdata.length > 0) {
                for (var i = 0; i < objdata.length; i++) {
                    var newGridViewRow = GridView.insertRow(GridView.rows.length);
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[0].innerHTML = objdata[i].Created_Date_And_Time;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[1].innerHTML = objdata[i].Type;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[2].innerHTML = objdata[i].Message_Description;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[3].innerHTML = objdata[i].Notes;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[4].innerHTML = objdata[i].Priority;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[5].innerHTML = objdata[i].Source;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[6].innerHTML = objdata[i].Assigned_To;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[7].innerHTML = objdata[i].Caller_Name;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[8].innerHTML = objdata[i].Relationship;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[9].innerHTML = objdata[i].Message_Date_And_Time;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[10].innerHTML = objdata[i].Created_By;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[11].innerHTML = objdata[i].Encounter_ID;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[12].innerHTML = objdata[i].Line_ID;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[13].innerHTML = objdata[i].Line_Type;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[14].innerHTML = objdata[i].Header_ID;
                    newGridViewRow.insertCell();
                    newGridViewRow.cells[15].innerHTML = objdata[i].Header_Type;
                    if (objdata[i].Priority.toUpperCase() == "HIGH")
                        newGridViewRow.style.color = "Red";
                }
            }
            return false;
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = xhr.statusText;
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
function ClearClick() {
    $("#ddlMessageDescription option:first").attr('selected', 'selected');
    document.getElementById("txtMessageNotes").value = "";
    $("#grdMessages").find("tr:gt(0)").remove();
    SearchClick();
    return false;
}
$(document).ready(function () {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    sessionStorage.setItem('MYQ', '');
    sessionStorage.setItem('PatientCommunication', '');
    sessionStorage.setItem('sFacility', '');
    sessionStorage.setItem('AccountNum', '');
    if (window.location.href.indexOf("MyQ") > -1 || window.location.href.indexOf("parentscreen") > -1) {
        sessionStorage.setItem('MYQ', 'true');
    }
    else
        if (window.location.href.indexOf('PatientCommunication') > -1 || window.location.href.indexOf('AccountNum') > -1) {
            sessionStorage.setItem('PatientCommunication', 'true');
            if (window.location.href.indexOf('AccountNum') > -1) {
                sessionStorage.setItem('AccountNum', window.location.href.split('?')[1].split('AccountNum=')[1]);
            }
        }
    if (document.getElementById("btnSaveMenu") != null)
        document.getElementById("btnSaveMenu").disabled = true;
    if (document.getElementById("btnSaveMyQ") != null)
        document.getElementById("btnSaveMyQ").disabled = true;
    //if (document.getElementById("btnSaveSendMyQ") != null)
    //    document.getElementById("btnSaveSendMyQ").disabled = true;
    //if (document.getElementById("btnSaveCompletedMyQ") != null)
    //    document.getElementById("btnSaveCompletedMyQ").disabled = true;
    var messageId = "";
    var parentscr = "";
    if (GetParameterValues('MessageID') != undefined)
        messageId = GetParameterValues('MessageID');
    if (GetParameterValues('parentscreen') != undefined)
        parentscr = GetParameterValues('parentscreen');
    $("#txtMessageDate").datetimepicker({ timepicker: false, format: 'd-M-Y' });
    var now = new Date()
    var duedate = new Date(now);
    var d = duedate.getDate();
    var m = duedate.getMonth();
    var y = duedate.getFullYear();
    var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var date = d + "-" + arr[m] + "-" + y;
    $('#txtMessageDate')[0].value = date;
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/LoadPatientCommunication",
        data: JSON.stringify({
            "MessageID": messageId,
            "ParentScreen": parentscr,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.DropDown.length > 0) {
                var returnedData = $.grep(objdata.DropDown, function (element, index) {
                    return element.Field_Name == "MESSAGE ORIGIN";
                });
                if (returnedData != null && returnedData.length > 0) {
                    var MessageOrigin = document.getElementById("ddlMessageOrigin");
                    var opt = document.createElement("option");
                    opt.text = "";
                    opt.value = "";
                    MessageOrigin.options.add(opt);
                    for (var i = 0; i < returnedData.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = returnedData[i].Value;
                        opt.value = i;
                        opt.title = returnedData[i].Value;
                        MessageOrigin.options.add(opt);
                    }
                }
                returnedData = $.grep(objdata.DropDown, function (element, index) {
                    return element.Field_Name == "Type";
                });
                if (returnedData != null && returnedData.length > 0) {
                    var Type = document.getElementById("ddlType");
                    for (var i = 0; i < returnedData.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = returnedData[i].Value;
                        opt.value = i;
                        opt.title = returnedData[i].Value;
                        Type.options.add(opt);
                    }
                }
                returnedData = $.grep(objdata.DropDown, function (element, index) {
                    return element.Field_Name == "MESSAGE DESCRIPTION";
                });
                if (returnedData != null && returnedData.length > 0) {
                    var MessageType = document.getElementById("ddlMessageType");
                    var MessageDesc = document.getElementById("ddlMessageDescription");
                    var opt = document.createElement("option");
                    opt.text = "";
                    opt.value = "";
                    opt.title = "";
                    MessageType.options.add(opt);
                    if (MessageDesc != undefined) {
                        var opts = document.createElement("option");
                        opts.text = "";
                        opts.value = "";
                        opts.title = "";
                        MessageDesc.options.add(opts);
                    }
                    for (var i = 0; i < returnedData.length; i++) {
                        var oppt = document.createElement("option");
                        oppt.text = returnedData[i].Value;
                        oppt.value = i;
                        oppt.title = returnedData[i].Value;
                        MessageType.options.add(oppt);
                        if (MessageDesc != undefined) {
                            var optt = document.createElement("option");
                            optt.text = returnedData[i].Value;
                            optt.value = i;
                            optt.title = returnedData[i].Value;
                            MessageDesc.options.add(optt);
                        }
                    }
                }
                returnedData = $.grep(objdata.DropDown, function (element, index) {
                    return element.Field_Name == "RELATIONSHIP";
                });
                if (returnedData != null && returnedData.length > 0) {
                    var Relationship = document.getElementById("ddlRelationship");
                    var opt = document.createElement("option");
                    opt.text = "";
                    opt.value = "";
                    opt.title = "";
                    Relationship.options.add(opt);
                    for (var i = 0; i < returnedData.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = returnedData[i].Value;
                        opt.value = i;
                        opt.title = returnedData[i].Value;
                        Relationship.options.add(opt);
                    }
                }
                returnedData = $.grep(objdata.DropDown, function (element, index) {
                    return element.Field_Name == "PRIORITY FOR MESSAGE";
                });
                if (returnedData != null && returnedData.length > 0) {
                    var Priority = document.getElementById("ddlPriority");
                    var opt = document.createElement("option");
                    opt.text = "";
                    opt.value = "";
                    opt.title = "";
                    Priority.options.add(opt);
                    for (var i = 0; i < returnedData.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = returnedData[i].Value;
                        opt.value = i;
                        opt.title = returnedData[i].Value;
                        Priority.options.add(opt);
                    }
                }
                FacilityName = objdata.Facility;
                sessionStorage.setItem('sFacility', FacilityName);
                if (objdata.Message.length > 0) {
                    AssignedTo = objdata.Message[0].Assigned_To;
                    FacilityName = objdata.Message[0].Facility_Name;
                }

                $("#ddlPriority option:eq(2)").attr('selected', 'selected');

                document.getElementById("ddlAssignedTo").options.length = 0;
                var vAssignedTo = objdata.AssignedTo;
                if (vAssignedTo != null && vAssignedTo.length > 0) {
                    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
                    var vddlAssignedTo = document.getElementById("ddlAssignedTo");
                    var opt = document.createElement("option");
                    for (var i = 0; i < vAssignedTo.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = vAssignedTo[i].split('|')[1];
                        opt.value = vAssignedTo[i].split('|')[0];
                        opt.title = vAssignedTo[i].split('|')[1];
                        vddlAssignedTo.options.add(opt);
                    }
                    $('#ddlAssignedTo option').each(function () {
                        if (AssignedTo.indexOf(this.value) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                }

                var cookies = document.cookie.split(';');
                var CLegalOrg = "";
                for (var l = 0; l < cookies.length; l++) {
                    if (cookies[l].indexOf("CLegalOrg") > -1)
                        CLegalOrg = cookies[l].split("=")[1];
                }
                $.get("ConfigXML/Facility_Library.xml", {}, function (xml) {
                    $("FacilityList", xml).each(function (i) {
                        $(this).find("Facility").each(function (l) {
                            if (CLegalOrg.toUpperCase() == $(this).attr("Legal_Org").toUpperCase()) {
                                $('#ddlFacilityName').append("<option value=''>" + $(this).attr("Name") + "</option>");
                            }
                        });
                    });
                    var vFacilityNames = sessionStorage.getItem('sFacility');
                    var vddlFacilityName = $("[id*='ddlFacilityName']")[0].options;
                    if (vddlFacilityName.length > 0) {
                        for (var i = 0; i < vddlFacilityName.length; i++) {
                            if (vddlFacilityName[i].innerText == vFacilityNames) {
                                vddlFacilityName.selectedIndex = i;
                                break;
                            }
                        }
                    }
                });
                if (objdata.Message.length > 0) {
                    $('#ddlType option').each(function () {
                        if (objdata.Message[0].Type.indexOf(this.text) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                    $('#ddlRelationship option').each(function () {
                        if (objdata.Message[0].Relationship.indexOf(this.text) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                    $('#ddlMessageOrigin option').each(function () {
                        if (objdata.Message[0].Message_Orign.indexOf(this.text) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                    $('#ddlPriority option').each(function () {
                        if (objdata.Message[0].Priority.indexOf(this.text) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                    $('#ddlMessageType option').each(function () {
                        if (objdata.Message[0].Message_Description.indexOf(this.text) > -1) {
                            option = this;
                            option.selected = true;
                        }
                    });
                    document.getElementById("txtCallerName").value = objdata.Message[0].Caller_Name;
                    document.getElementById("txtCreatedBy").value = objdata.Message[0].Created_By;
                    document.getElementById("DLC_txtDLC").value = "";
                    if (objdata.Message[0].Notes.indexOf("@") <= -1) {
                        var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
                        var vCreated_Date;
                        if (objdata.Message[0].Message_Date_And_Time != "0001-01-01T00:00:00")
                            vCreated_Date = objdata.Message[0].Message_Date_And_Time.replace("T", " ").split('-');
                        else

                            vCreated_Date = objdata.Message[0].Created_Date_And_Time.replace("T", " ").split('-');

                        if (vCreated_Date[1].substring(0, 1) == "0")
                            vCreated_Date[1] = vCreated_Date[1].slice(-1);

                        var hours = vCreated_Date[2].split(' ')[1].split(':')[0];
                        var ampm = hours >= 12 ? "PM" : "AM"
                        hours = hours % 12;
                        hours = hours > 0 ? hours : 12; // the hour '0' should be '12'
                        var minutes = '';
                        if (vCreated_Date[2].split(' ')[1].split(':').length > 1)
                            minutes = vCreated_Date[2].split(' ')[1].split(':')[1];

                        var strTime = hours + ':' + minutes + ' ' + ampm;

                        var vCRT = vCreated_Date[2].split(' ')[0] + "-" + arr[parseInt(vCreated_Date[1]) - 1] + "-" + vCreated_Date[0] + " " + strTime;
                        document.getElementById("txtmsghistory").value = "@" + objdata.Message[0].Created_By + "(" + vCRT + "): " + objdata.Message[0].Notes;


                    }
                    else {
                        document.getElementById("txtmsghistory").value = objdata.Message[0].Notes.replace("<br />", "\n");
                    }
                    if (objdata.Message[0].Message_Date_And_Time != "0001-01-01T00:00:00") {
                        var Messagedate = new Date(objdata.Message[0].Message_Date_And_Time.substring(0, 11) + objdata.Message[0].Message_Date_And_Time.split('T')[1].split(':')[0].replace('00', '12') + objdata.Message[0].Message_Date_And_Time.substring(13, 19));
                        //var Messagedate = new Date(objdata.Message[0].Message_Date_And_Time);
                        var d = Messagedate.getDate();
                        var m = Messagedate.getMonth();
                        var y = Messagedate.getFullYear();
                        var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
                        var date = d + "-" + arr[m] + "-" + y;
                        $("#txtMessageDate")[0].value = date;
                        //Gitlab #2823 - Fill Add to Patient Chart Checkbox
                        if (objdata.Message[0].Is_PatientChart == "Y") {
                            document.getElementById("ChkPatientChart").checked = true;
                        }
                        else {
                            document.getElementById("ChkPatientChart").checked = false;
                        }
                    }
                }
            }
            if (GetParameterValues('parentscreen') == "MyQ") {
                document.getElementById("ddlType").disabled = true;
                $('#ddlType').addClass('nonEditabletxtbox');
                document.getElementById("ddlRelationship").disabled = true;
                $('#ddlRelationship').addClass('nonEditabletxtbox');
                document.getElementById("txtCallerName").disabled = true;
                $('#txtCallerName').addClass('nonEditabletxtbox');
                document.getElementById("ddlMessageOrigin").disabled = true;
                $('#ddlMessageOrigin').addClass('nonEditabletxtbox');
                document.getElementById("ddlFacilityName").disabled = true;
                $('#ddlFacilityName').addClass('nonEditabletxtbox');
                document.getElementById("ddlPriority").disabled = true;
                $('#ddlPriority').addClass('nonEditabletxtbox');
                $("#txtMessageDate")[0].disabled = true;
                $("#txtMessageDate")[0].readOnly = true;
                $("#txtMessageDate").addClass('nonEditabletxtbox');
            }
            if ($("#ddlType option:selected").text() == "TASK") {
                document.getElementById("lblassignedto").innerText = "Assigned To*";
            }
            if ($("#ddlType option:selected").text() == "MESSAGE") {
                document.getElementById("lblassignedto").style.color = 'Black';
                document.getElementById("lblassignedto").innerText = "Assigned To";
            }
            document.getElementById("txtCreatedBy").disabled = true;
            $('#txtCreatedBy').addClass('nonEditabletxtbox');
            document.getElementById("txtCallerName").disabled = true;
            $('#txtCallerName').addClass('nonEditabletxtbox');
            if (GetParameterValues('parentscreen') == "PatientChart") {
                $("[id*='lblassignedto']")[0].style.color = 'Black';
                $("[id*='lblassignedto']")[0].innerText = "Assigned To";
                $('#lblMessageType').removeClass('MandLabelstyle');
                $('#lblMessageType').addClass('spanstyle');
                document.getElementById("lblMessageTypeStar").style.display = 'none';
                $('#lblMessageNotes').removeClass('MandLabelstyle');
                $('#lblMessageNotes').addClass('spanstyle');
                document.getElementById("lblMessageNotesStar").style.display = 'none';
                $("[id*='chkshowall']")[0].disabled = true;
                $("[id*='chkshowall']").addClass('nonEditabletxtbox')
                $("[id*='txtCallerName']")[0].disabled = true;
                $("[id*='txtCallerName']").addClass('nonEditabletxtbox')
                $("[id*='ChkPatientChart']")[0].disabled = true;

                $('#DLC_txtDLC').addClass('nonEditabletxtbox')
                $("[id*='ChkPatientChart']").addClass('nonEditabletxtbox')
                $("[id*='ChkPatientChart']")[0].checked = true;
                document.getElementById("DLC_txtDLC").disabled = true;

                $('#DLC_txtDLC').addClass('nonEditabletxtbox')
                $("[id*='ddlType']")[0].disabled = true;
                $("[id*='ddlType']").addClass('nonEditabletxtbox')
                $("[id*='ddlRelationship']")[0].disabled = true;
                $("[id*='ddlRelationship']").addClass('nonEditabletxtbox');
                $("[id*='ddlMessageOrigin']")[0].disabled = true;
                $("[id*='ddlMessageOrigin']").addClass('nonEditabletxtbox');
                $("[id*='ddlFacilityName']")[0].disabled = true;
                $("[id*='ddlFacilityName']").addClass('nonEditabletxtbox');
                $("[id*='ddlAssignedTo']")[0].disabled = true;
                $("[id*='ddlAssignedTo']").addClass('nonEditabletxtbox');
                $("[id*='ddlPriority']")[0].disabled = true;
                $("[id*='ddlPriority']").addClass('nonEditabletxtbox');
                $("[id*='ddlMessageType']")[0].disabled = true;
                $("[id*='ddlMessageType']").addClass('nonEditabletxtbox');
                $("[id*='txtMessageDate']")[0].disabled = true;
                $("[id*='txtMessageDate']").addClass('nonEditabletxtbox');
                $("[id*='RowForHide']")[0].className = "displayNone";
            }
            $("span[mand=Yes]").addClass('MandLabelstyle');

            $("span[mand=Yes]").each(function () {
                $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
            });
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    "Message: " + log.Message);
            }
        }
    });

    if (sessionStorage.getItem('MYQ') != "true" || sessionStorage.getItem('PatientCommunication') == "true") {

        FillMessageGrid();
    }
    else
        if ((sessionStorage.getItem('MYQ') == "true")) {
            if ($("[id*='grdMessages']")[0] != undefined) {
                $("[id*='grdMessages']")[0].className = "displayNone";
            }
            if ($("[id*='pnlMessageInfo']")[0] != undefined) {
                $("[id*='pnlMessageInfo']")[0].className = "displayNone";
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
});
var vhumanid = "";
function FillMessageGrid() {
    if (sessionStorage.getItem('AccountNum') != undefined && sessionStorage.getItem('AccountNum') != "") {
        vhumanid = sessionStorage.getItem('AccountNum');
    }
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/FillMessageGrid",
        data: "{\"sHumanId\":\"" + vhumanid + "\"}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $("[id*='grdMessages']")[0].innerHTML = "";
            var objdata = $.parseJSON(data.d);
            if (objdata.ilstPatientNotes.length > 0) {
                var TableContents = "";
                var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
                for (var i = 0; i < objdata.ilstPatientNotes.length; i++) {
                    var vCreated_Date = objdata.ilstPatientNotes[i].Created_Date_And_Time.replace("T", " ").split('-');
                    if (vCreated_Date[1].substring(0, 1) == "0")
                        vCreated_Date[1] = vCreated_Date[1].slice(-1);

                    var hours = vCreated_Date[2].split(' ')[1].split(':')[0];
                    var ampm = hours >= 12 ? "PM" : "AM"
                    hours = hours % 12;
                    hours = hours > 0 ? hours : 12; // the hour '0' should be '12'
                    var minutes = '';
                    if (vCreated_Date[2].split(' ')[1].split(':').length > 1)
                        minutes = vCreated_Date[2].split(' ')[1].split(':')[1];

                    var strTime = hours + ':' + minutes + ' ' + ampm;
                    var vCRT = vCreated_Date[2].split(' ')[0] + "-" + arr[parseInt(vCreated_Date[1]) - 1] + "-" + vCreated_Date[0] + " " + strTime;//vCreated_Date[2].split(' ')[1];
                    var vMessage_Date = new Date(objdata.ilstPatientNotes[i].Message_Date_And_Time.substring(0, 11) + objdata.ilstPatientNotes[i].Message_Date_And_Time.split('T')[1].split(':')[0].replace('00', '12') + objdata.ilstPatientNotes[i].Message_Date_And_Time.substring(13, 19));
                    var MSG = new Array();
                    MSG = vMessage_Date.toString().split(' ');
                    var vMSG;
                    if (MSG[2] + "-" + MSG[1] + "-" + MSG[3] == '01-Jan-0001') {
                        vMSG = "";
                    }
                    else {
                        vMSG = MSG[2] + "-" + MSG[1] + "-" + MSG[3];
                    }
                    if (objdata.ilstPatientNotes[i].Modified_Date_And_Time != "0001-01-01T00:00:00") {
                        var vModified = objdata.ilstPatientNotes[i].Modified_Date_And_Time.replace("T", " ").split('-');
                        if (vModified[1].substring(0, 1) == "0")
                            vModified[1] = vModified[1].slice(-1);
                        var hours = vModified[2].split(' ')[1].split(':')[0];
                        var ampm = hours >= 12 ? "PM" : "AM"
                        hours = hours % 12;
                        hours = hours > 0 ? hours : 12; // the hour '0' should be '12'
                        var minutes = '';
                        if (vModified[2].split(' ')[1].split(':').length > 1)
                            minutes = vModified[2].split(' ')[1].split(':')[1];

                        var strTime = hours + ':' + minutes + ' ' + ampm;
                        var vMDY = vModified[2].split(' ')[0] + "-" + arr[parseInt(vModified[1]) - 1] + "-" + vModified[0] + " " + strTime;//vModified[2].split(' ')[1];
                    }
                    else
                        vMDY = "";
                    var CurrentProcess = "";
                    if (objdata.ilstWFObj[i] != null && objdata.ilstWFObj[i].Current_Process != null && objdata.ilstWFObj[i].Current_Process != "")
                        CurrentProcess = objdata.ilstWFObj[i].Current_Process;
                    var AssigntoPerson = "";
                    if (objdata.ilstPatientNotes[i] != null && objdata.ilstPatientNotes[i].Assigned_To != undefined && objdata.ilstPatientNotes[i].Assigned_To != null && objdata.ilstPatientNotes[i].Assigned_To != "")
                        AssigntoPerson = objdata.ilstPatientNotes[i].Assigned_To;
                    //var AssigntoPerson = "";
                    //if (objdata.ilstUser[i] != null && objdata.ilstUser[i].person_name != undefined && objdata.ilstUser[i].person_name != null && objdata.ilstUser[i].person_name != "")
                    //    AssigntoPerson = objdata.ilstUser[i].person_name;

                    //Gitlab #2823 - Fill Add to Patient Chart Checkbox
                    if (i == 0) {
                        if (CurrentProcess == "OPEN_TASK") {
                            TableContents = "<tr><td style='width:30px'><img id=img" + i + " src='Resources/edit.gif' alt='Submit' onclick='img(this);' " + (CurrentProcess.toUpperCase() != "OPEN_TASK" ? "disabled='disabled'" : "") + " /></td><td style='width:60px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Type + "</td><td style='width:100px;white-space: normal;'>" + AssigntoPerson + "</td><td style='width:60px'>" + objdata.ilstPatientNotes[i].Priority + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Description + "</td><td style='width:250px;white-space: normal;white-space: pre-wrap;'>" + objdata.ilstPatientNotes[i].Notes + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Caller_Name + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Relationship + "</td><td style='width:200px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Created_By + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Orign + "</td><td style='width:100px;white-space: normal;'>" + vMSG + "</td><td style='width:150px'>" + vCRT + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Facility_Name + "</td><td style='width:150;white-space: normal;'>" + objdata.ilstPatientNotes[i].Modified_By + "</td><td style='width:150px;white-space: normal;'>" + vMDY + "</td><td style='display:none;' >" + objdata.ilstPatientNotes[i].Source + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Encounter_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_Type + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_Type + "</td><td style='display:none'>" + CurrentProcess + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Id + "</td><td style='display:none'>" + objdata.ilstWFObj[i].Id + "</td><td>Open</td><td></td><td style='display:none'>" + objdata.ilstPatientNotes[i].Is_PatientChart + "</td></tr>";
                        }
                        else {
                            TableContents = "<tr><td style='width:30px'><img id=img" + i + " src='Resources/edit disabled.png' alt='Submit' onclick='img(this);' " + (CurrentProcess.toUpperCase() != "OPEN_TASK" ? "disabled='disabled'" : "") + " /></td><td style='width:60px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Type + "</td><td style='width:100px;white-space: normal;'>" + AssigntoPerson + "</td><td style='width:60px'>" + objdata.ilstPatientNotes[i].Priority + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Description + "</td><td style='width:250px;white-space: normal;white-space: pre-wrap;'>" + objdata.ilstPatientNotes[i].Notes + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Caller_Name + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Relationship + "</td><td style='width:200px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Created_By + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Orign + "</td><td style='width:100px;white-space: normal;'>" + vMSG + "</td><td style='width:150px'>" + vCRT + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Facility_Name + "</td><td style='width:150;white-space: normal;'>" + objdata.ilstPatientNotes[i].Modified_By + "</td><td style='width:150px;white-space: normal;'>" + vMDY + "</td><td style='display:none;'>" + objdata.ilstPatientNotes[i].Source + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Encounter_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_Type + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_Type + "</td><td style='display:none'>" + CurrentProcess + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Id + "</td><td style='display:none'>" + objdata.ilstWFObj[i].Id + "</td><td>Completed</td><td>" + vMDY + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Is_PatientChart + "</td></tr>";
                        }
                    }
                    else {
                        if (CurrentProcess == "OPEN_TASK") {
                            TableContents = TableContents + "<tr><td style='width:30px'><img id=img" + i + " src='Resources/edit.gif' alt='Submit' onclick='img(this);' " + (CurrentProcess.toUpperCase() != "OPEN_TASK" ? "disabled='disabled'" : "") + " /></td><td style='width:60px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Type + "</td><td style='width:100px;white-space: normal;'>" + AssigntoPerson + "</td><td style='width:60px'>" + objdata.ilstPatientNotes[i].Priority + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Description + "</td><td style='width:250px;white-space: normal;white-space: pre-wrap;'>" + objdata.ilstPatientNotes[i].Notes + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Caller_Name + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Relationship + "</td><td style='width:200px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Created_By + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Orign + "</td><td style='width:100px;white-space: normal;'>" + vMSG + "</td><td style='width:150px'>" + vCRT + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Facility_Name + "</td><td style='width:150;white-space: normal;'>" + objdata.ilstPatientNotes[i].Modified_By + "</td><td style='width:150px;white-space: normal;'>" + vMDY + "</td><td style='display:none;' >" + objdata.ilstPatientNotes[i].Source + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Encounter_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_Type + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_Type + "</td><td style='display:none'>" + CurrentProcess + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Id + "</td><td style='display:none'>" + objdata.ilstWFObj[i].Id + "</td><td>Open</td><td></td><td style='display:none'>" + objdata.ilstPatientNotes[i].Is_PatientChart + "</td></tr>";
                        }
                        else {
                            TableContents = TableContents + "<tr><td style='width:30px'><img id=img" + i + " src='Resources/edit disabled.png' alt='Submit' onclick='img(this);' " + (CurrentProcess.toUpperCase() != "OPEN_TASK" ? "disabled='disabled'" : "") + " /></td><td style='width:60px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Type + "</td><td style='width:100px;white-space: normal;'>" + AssigntoPerson + "</td><td style='width:60px'>" + objdata.ilstPatientNotes[i].Priority + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Description + "</td><td style='width:250px;white-space: normal;white-space: pre-wrap;'>" + objdata.ilstPatientNotes[i].Notes + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Caller_Name + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Relationship + "</td><td style='width:200px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Created_By + "</td><td style='width:100px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Message_Orign + "</td><td style='width:100px;white-space: normal;'>" + vMSG + "</td><td style='width:150px'>" + vCRT + "</td><td style='width:150px;white-space: normal;'>" + objdata.ilstPatientNotes[i].Facility_Name + "</td><td style='width:150;white-space: normal;'>" + objdata.ilstPatientNotes[i].Modified_By + "</td><td style='width:150px;white-space: normal;'>" + vMDY + "</td><td style='display:none;'>" + objdata.ilstPatientNotes[i].Source + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Encounter_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Line_Type + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_ID + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Header_Type + "</td><td style='display:none'>" + CurrentProcess + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Id + "</td><td style='display:none'>" + objdata.ilstWFObj[i].Id + "</td><td>Completed</td><td>" + vMDY + "</td><td style='display:none'>" + objdata.ilstPatientNotes[i].Is_PatientChart + "</td></tr>";
                        }

                    }

                }
                $("[id*='grdMessages']").append("<table id=grdMessagestable class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:100%'><thead style='border: 0px;width:96.7%;' ><tr class='Gridheaderstyle'><th  style='border: 1px solid #909090;width:30px'>Edit</th><th style='border: 1px solid #909090;width:60px'>Type</th><th style='border: 1px solid #909090;width:100px'>Assigned To</th><th style='border: 1px solid #909090;width:60px'>Priority</th><th style='border: 1px solid #909090;width:150px'>Msg.Description</th><th style='border: 1px solid #909090;width:250px'>Msg.Notes</th><th style='border: 1px solid #909090;width:100px'>Spoken To</th><th style='border: 1px solid #909090;width:100px'>Relationship</th><th style='border: 1px solid #909090;width:150px'>Created By</th><th style='border: 1px solid #909090;width:100px'>Message Origin</th><th style='border: 1px solid #909090;width:100px'>Message Date</th><th style='border: 1px solid #909090;width:150px;white-space: normal;'>Task Created Date and Time</th><th style='border: 1px solid #909090;width:150px'>Facility Name</th><th style='border: 1px solid #909090;width:150px'>Modified By</th><th style='border: 1px solid #909090;width:150px;white-space: normal'>Task Modified Date and Time</th><th style='border: 1px solid #909090;display:none;'>Source</th><th style='border: 1px solid #909090;display:none;'>Enc Id</th><th style='border: 1px solid #909090;display:none;'>Line Id</th><th style='border: 1px solid #909090;display:none;'>Line Type</th><th style='border: 1px solid #909090;display:none;'>Header Id</th><th style='border: 1px solid #909090;display:none;'>Header Type</th><th style='border: 1px solid #909090;display:none;'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Message ID</th><th style='border: 1px solid #909090;display:none;'>WfobjectID</th><th style='border: 1px solid #909090;width:150px;white-space: normal;'>Status</th><th style='border: 1px solid #909090;width:150px;white-space: normal;'>Task Complete Date and Time</th></thead><tbody style='word-wrap: break-word;'></thead><tbody style='word-wrap: break-word;'>" + TableContents + "</tbody></table>");
            }
            else {
                $("[id*='grdMessages']").append("<table id=grdMessagestable class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:100%'><thead style='border: 0px;width:96.7%;' class='Gridheaderstyle'><tr class='Gridheaderstyle'><th  style='border: 1px solid #909090;width:30px'>Edit</th><th style='border: 1px solid #909090;width:60px'>Type</th><th style='border: 1px solid #909090;width:100px'>Assigned To</th><th style='border: 1px solid #909090;width:60px'>Priority</th><th style='border: 1px solid #909090;width:150px'>Msg.Description</th><th style='border: 1px solid #909090;width:250px'>Msg.Notes</th><th style='border: 1px solid #909090;width:100px'>Spoken To</th><th style='border: 1px solid #909090;width:100px'>Relationship</th><th style='border: 1px solid #909090;width:150px'>Created By</th><th style='border: 1px solid #909090;width:100px'>Message Origin</th><th style='border: 1px solid #909090;width:100px'>Message Date</th><th style='border: 1px solid #909090;width:150px;white-space: normal;'>Task Created Date and Time</th><th style='border: 1px solid #909090;width:150px'>Facility Name</th><th style='border: 1px solid #909090;width:150px'>Modified By</th><th style='border: 1px solid #909090;width:150px;white-space: normal'>Task Modified Date and Time</th><th style='border: 1px solid #909090;display:none;'>Source</th><th style='border: 1px solid #909090;display:none;'>Enc Id</th><th style='border: 1px solid #909090;display:none;'>Line Id</th><th style='border: 1px solid #909090;display:none;'>Line Type</th><th style='border: 1px solid #909090;display:none;'>Header Id</th><th style='border: 1px solid #909090;display:none;'>Header Type</th><th style='border: 1px solid #909090;display:none;'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Message ID</th><th style='border: 1px solid #909090;display:none;'>WfobjectID</th><th style='border: 1px solid #909090;width:150px;white-space: normal;'>Status</th><th style='border: 1px solid #909090;width:150px;white-space: normal;'>Task Complete Date and Time</th></thead><tbody style='word-wrap: break-word;'></tbody></table>");
            }

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $("[id*='btnSaveMenu']")[0].disabled = true;
            if (localStorage.getItem("bSaveSuccess") == "true") {
                localStorage.setItem('bSaveSuccess', '');
                self.close();
            }
            return false;
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = xhr.statusText;
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
function img(e) {

    var row = e.parentNode.parentNode;
    if (row.cells[21].innerHTML == "OPEN_TASK") {

        $("#btnSaveMenu").val("Update");
        $("#btnClearAll").val("Cancel");
        var vddlType = $("[id*='ddlType']")[0].options;
        $("#ddlType option:first").attr('selected', 'selected');
        if (vddlType.length > 0) {
            for (var i = 0; i < vddlType.length; i++) {
                if (vddlType[i].innerText == row.cells[1].innerHTML) {
                    vddlType.selectedIndex = i;
                    break;
                }
            }
        }
        var vddlPriority = $("[id*='ddlPriority']")[0].options;
        $("#ddlPriority option:first").attr('selected', 'selected');
        if (vddlPriority.length > 0) {
            for (var i = 0; i < vddlPriority.length; i++) {
                if (vddlPriority[i].innerText == row.cells[3].innerHTML) {
                    vddlPriority.selectedIndex = i;
                    break;
                }
            }
        }

        var vddlMessageOrigin = $("[id*='ddlMessageOrigin']")[0].options;
        $("#ddlMessageOrigin option:first").attr('selected', 'selected');
        if (vddlMessageOrigin.length > 0) {
            for (var i = 0; i < vddlMessageOrigin.length; i++) {
                if (vddlMessageOrigin[i].innerText == row.cells[9].innerHTML) {
                    vddlMessageOrigin.selectedIndex = i;
                    break;
                }
            }
        }

        var vddlRelationship = $("[id*='ddlRelationship']")[0].options;
        $("#ddlRelationship option:first").attr('selected', 'selected');
        if (vddlRelationship.length > 0) {
            for (var i = 0; i < vddlRelationship.length; i++) {
                if (vddlRelationship[i].innerText == row.cells[7].innerHTML) {
                    vddlRelationship.selectedIndex = i;
                    break;
                }
            }
            RelationChange();
        }

        var vddlFacilityName = $("[id*='ddlFacilityName']")[0].options;
        if (vddlFacilityName.length > 0) {
            for (var i = 0; i < vddlFacilityName.length; i++) {
                if (vddlFacilityName[i].innerText == row.cells[12].innerHTML) {
                    vddlFacilityName.selectedIndex = i;
                    break;
                }
            }
        }

        EditAssingnmethod(row);
        document.getElementById("btnSaveMenu").disabled = false;
        var vddlMessageType = $("[id*='ddlMessageType']")[0].options;
        $("#ddlMessageType option:first").attr('selected', 'selected');
        if (vddlMessageType.length > 0) {
            for (var i = 0; i < vddlMessageType.length; i++) {
                if (vddlMessageType[i].innerText == row.cells[4].innerHTML) {
                    vddlMessageType.selectedIndex = i;
                    break;
                }
            }
        }

        $("[id*='txtMessageDate']")[0].value = row.cells[10].innerHTML;
        $("[id*='txtCallerName']")[0].value = row.cells[6].innerHTML;
        document.getElementById("txtmsghistory").value = row.cells[5].innerHTML;
        sessionStorage.setItem("messageid", row.cells[22].innerHTML);
        sessionStorage.setItem("WFOBJID", row.cells[23].innerHTML);
        //Gitlab #2823 - Fill Add to Patient Chart Checkbox
        if (row.cells[26].innerHTML == "Y") {
            document.getElementById("ChkPatientChart").checked = true;
        }
        else {
            document.getElementById("ChkPatientChart").checked = false;
        }
    }


}

function AfterSaveClear() {
    $("#ddlType option:first").attr('selected', 'selected');

    $('#ddlRelationship option:selected').attr("selected", null);
    $("#ddlRelationship option:first").attr('selected', 'selected');
    $('#ddlMessageOrigin option:selected').attr("selected", null);
    $("#ddlMessageOrigin option:first").attr('selected', 'selected');
    $('#ddlMessageType option:selected').attr('selected', null);
    $("#ddlMessageType option:first").attr('selected', 'selected');
    $('#ddlPriority option:selected').attr('selected', null);
    $("#ddlPriority option:eq(2)").attr('selected', 'selected');
    $('#ddlMessageDescription option:selected').attr('selected', null);
    $("#ddlMessageDescription option:first").attr('selected', 'selected');
    document.getElementById("lblassignedto").innerText = "Assigned To*";
    document.getElementById("lblCallerName").style.color = "Black";
    $('#lblCallerName').html('Spoken To"');
    $("#lblCallerName").removeClass("MandLabelstyle");

    document.getElementById("lblCallerName").innerHTML = "Spoken To";
    if ($("#ddlRelationship option:selected").text() != "Self" && $("#ddlRelationship option:selected").text() != "") {
        document.getElementById("txtCallerName").disabled = false;
        $("#txtCallerName").removeClass('nonEditabletxtbox');
        $("#txtCallerName").addClass('Editabletxtbox');
        $("#ddlRelationship option:first").attr('selected', 'selected');
    }
    else {
        document.getElementById("txtCallerName").disabled = true;
        $("#txtCallerName").removeClass('Editabletxtbox');
        $("#txtCallerName").addClass('nonEditabletxtbox');

        $("#ddlRelationship option:first").attr('selected', 'selected');
    }
    document.getElementById("txtCallerName").value = "";
    document.getElementById("txtCreatedBy").disabled = true;
    $("#txtCreatedBy").addClass('nonEditableTextbox');
    document.getElementById("chkshowall").checked = false;
    document.getElementById("DLC_txtDLC").value = "";
    document.getElementById("txtmsghistory").value = "";
    document.getElementById("ddlRelationship").disabled = false;
    document.getElementById("ddlMessageOrigin").disabled = false;
    document.getElementById("ddlAssignedTo").disabled = false;
    document.getElementById("ChkPatientChart").checked = false;
    $('#btnSaveMenu').val('Add');
    $('#btnClearAll').val('Clear All');
    $('#btnSaveMenu').attr("disabled", true);
    var now = new Date()
    var duedate = new Date(now);
    var d = duedate.getDate();
    var m = duedate.getMonth();
    var y = duedate.getFullYear();
    var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var date = d + "-" + arr[m] + "-" + y;
    $('#txtMessageDate')[0].value = date;

    var vFacilityNames = sessionStorage.getItem('sFacility');
    var vddlFacilityName = $("[id*='ddlFacilityName']")[0].options;
    if (vddlFacilityName.length > 0) {
        for (var i = 0; i < vddlFacilityName.length; i++) {
            if (vddlFacilityName[i].innerText == vFacilityNames) {
                vddlFacilityName.selectedIndex = i;
                break;
            }
        }
    }
}
function chkShowAllChange() {
    var vfacility = $("[id*='ddlFacilityName']");
    Facility = $("[id*='ddlFacilityName']")[0].selectedOptions[0].innerText;
    document.getElementById("ddlAssignedTo").options.length = 0;
    var checked = "false";
    var vfacilitys = "";
    if (document.getElementById("chkshowall").checked)
        checked = "true";
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/laodAssigned",
        data: JSON.stringify({
            "chkshowall": checked,
            "facility": Facility,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.AssignedTo.length > 0) {
                var vAssignedTo = objdata.AssignedTo;
                if (vAssignedTo != null && vAssignedTo.length > 0) {
                    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
                    var vddlAssignedTo = document.getElementById("ddlAssignedTo");
                    var opt = document.createElement("option");
                    for (var i = 0; i < vAssignedTo.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = vAssignedTo[i].split('|')[1];
                        opt.value = vAssignedTo[i].split('|')[0];
                        opt.title = vAssignedTo[i].split('|')[1];
                        vddlAssignedTo.options.add(opt);
                    }
                }
            }
        },
        failure: function (data) {
            alert(data.d);
        }
    });

}
function EditAssingnmethod(row) {
    var vfacility = $("[id*='ddlFacilityName']");
    document.getElementById("ddlAssignedTo").options.length = 0;
    var checked = "false";
    var vfacilitys = "";
    var varFacilitys = $("[id*='ddlFacilityName']")[0].selectedOptions[0].innerText;
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/laodAssigned",
        data: JSON.stringify({
            "chkshowall": checked,
            "facility": varFacilitys,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.AssignedTo.length > 0) {
                var vAssignedTo = objdata.AssignedTo;
                if (vAssignedTo != null && vAssignedTo.length > 0) {
                    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
                    var vddlAssignedTo = document.getElementById("ddlAssignedTo");
                    var opt = document.createElement("option");
                    for (var i = 0; i < vAssignedTo.length; i++) {
                        var opt = document.createElement("option");

                        opt.text = vAssignedTo[i].split('|')[1];
                        opt.value = vAssignedTo[i].split('|')[0];
                        opt.title = vAssignedTo[i].split('|')[1];
                        vddlAssignedTo.options.add(opt);
                    }
                }
            }
            var vvddlAssignedTo = $("[id*='ddlAssignedTo']")[0].options;
            if (vvddlAssignedTo.length > 0) {
                for (var i = 0; i < vvddlAssignedTo.length; i++) {
                    if (vvddlAssignedTo[i].innerText == row.cells[2].innerHTML) {
                        vvddlAssignedTo.selectedIndex = i;
                        break;
                    }
                }
            }
        },
        failure: function (data) {
            alert(data.d);
        }
    });
}
function Assingnmethod() {

    var vfacility = $("[id*='ddlFacilityName']");
    document.getElementById("ddlAssignedTo").options.length = 0;
    var checked = "false";
    var vfacilitys = "";
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/laodAssigned",
        data: JSON.stringify({
            "chkshowall": checked,
            "facility": vfacilitys,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.AssignedTo.length > 0) {
                var vAssignedTo = objdata.AssignedTo;
                if (vAssignedTo != null && vAssignedTo.length > 0) {
                    $('#ddlAssignedTo').append("<option value=''>" + "" + "</option>");
                    var vddlAssignedTo = document.getElementById("ddlAssignedTo");
                    var opt = document.createElement("option");
                    for (var i = 0; i < vAssignedTo.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = vAssignedTo[i].split('|')[1];
                        opt.value = vAssignedTo[i].split('|')[0];
                        opt.title = vAssignedTo[i].split('|')[1];
                        vddlAssignedTo.options.add(opt);
                    }
                }
            }
        },
        failure: function (data) {
            alert(data.d);
        }
    });

}
function ClearAllMenu() {
    if ($("#btnSaveMenu").val() == "Add") {
        if (DisplayErrorMessage('180010')) {
            AfterSaveClear();
            Assingnmethod();
            $('#btnSaveMenu').attr("disabled", true);
            localStorage.setItem('bSave', 'true');
        }
    }
    else if ($("#btnSaveMenu").val() == "Update") {
        if (DisplayErrorMessage('180049')) {
            AfterSaveClear();
            Assingnmethod();
            $("#btnSaveMenu").val("Add");
            $("#btnClearAll").val("Clear All");
            $('#btnSaveMenu').attr("disabled", true);
            localStorage.setItem('bSave', 'true');
        }

    }
    return false;
}
function AutoSave() {
    document.getElementById("btnSaveMenu").disabled = false;
    localStorage.setItem("bSave", "false");
    //documen.get
}
function EnableAll() {
    if (document.getElementById("btnSaveMenu") != null)
        document.getElementById("btnSaveMenu").disabled = false;
    if (document.getElementById("btnSaveMyQ") != null)
        document.getElementById("btnSaveMyQ").disabled = false;
    //if (document.getElementById("btnSaveSendMyQ") != null && AssignedTo != "" && $("#ddlAssignedTo option:selected").text() != AssignedTo)
    //    document.getElementById("btnSaveSendMyQ").disabled = false;
    //else
    //    document.getElementById("btnSaveSendMyQ").disabled = true;
    //if (document.getElementById("btnSaveCompletedMyQ") != null)
    //    document.getElementById("btnSaveCompletedMyQ").disabled = false;
    localStorage.setItem("bSave", "false");
}
function CancelMenu() {
    if ($("[id*='btnSaveMenu']")[0] != undefined && $("[id*='btnSaveMenu']")[0].disabled == false) {
        localStorage.setItem("bSave", "false");
    }
    else
        localStorage.setItem("bSave", "true");
    if (window.GetRadWindow() != null)
        var winName = window.GetRadWindow()._name;
    localStorage.setItem("bSaveSuccess", "");
    if (localStorage.getItem("bSave") == "false") {
        if ($(top.window.document).find("iframe[name='" + winName + "']").length != 0) {
            if (!$($(top.window.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').is('#dvdialogMenu'))
                $($(top.window.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                    '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
            dvdialog = $($(top.window.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').find('#dvdialogMenu');
        }
        else if ($(top.window.document).find("iframe[name='ctl00_ModalWindow']").length != 0) {
            if (!$($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').is('#dvdialogMenu'))
                $($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                    '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
            dvdialog = $($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').find('#dvdialogMenu');
        }
        else if ($(top.window.document).find("iframe").length != 0) {
            if (!$($(top.window.document).find("iframe")[0].contentDocument).find('body').is('#dvdialogMenu'))
                $($(top.window.document).find("iframe")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                    '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
            dvdialog = $($(top.window.document).find("iframe")[0].contentDocument).find('body').find('#dvdialogMenu');
        }
        myPos = "center center";
        atPos = 'center center';
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            buttons: {
                "Yes": function () {

                    localStorage.setItem("bSaveSuccess", "true");
                    $("[id*='btnSaveMenu']")[0].click();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    localStorage.setItem("bSave", "true");
                    $(dvdialog).dialog("close");
                    return false;
                },
                "No": function () {
                    localStorage.setItem("bSave", "true");
                    localStorage.setItem("bSaveSuccess", "");
                    $(dvdialog).dialog("close");
                    self.close();
                    return false;
                },
                "Cancel": function () {
                    localStorage.setItem("bSaveSuccess", "");
                    $(dvdialog).dialog("close");
                    localStorage.setItem("bSave", "false");
                    //localStorage.setItem("bSave", "true");
                    return false;
                }
            }
        });
        return false;
    }
    else {
        window.close();
        return false;
    }


}
function CancelMyQ() {
    if (document.URL.indexOf('GenQTask') > -1) {
        RemoveItem(document.URL, "MessageID");
    }
    if (Result != undefined) {
        if (false == Result.closed) {

            Result.close();
        }
    }
    localStorage.setItem("bSaveSuccess", "");
    if (localStorage.getItem("bSave") == "false") {
        if (!$($(top.window.document).find("iframe[name='ModalWindow']")[0].contentDocument).find('body').is('#dvdialogMenu'))
            $($(top.window.document).find("iframe[name='ModalWindow']")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save and send?</p></div>');
        dvdialog = $($(top.window.document).find("iframe[name='ModalWindow']")[0].contentDocument).find('body').find('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            buttons: {
                "Yes": function () {
                    $(dvdialog).dialog("close");
                    localStorage.setItem("bSaveSuccess", "true");
                    //$("[id*='btnSaveMyQ']")[0].click();
                    $("[id*='btnSaveSendMyQ']")[0].click();
                    localStorage.setItem("bSave", "true");
                    return false;
                },
                "No": function () {
                    localStorage.setItem("bSaveSuccess", "");
                    localStorage.setItem("bSave", "true");
                    $(dvdialog).dialog("close");
                    self.close();
                    return false;
                },
                "Cancel": function () {
                    localStorage.setItem("bSaveSuccess", "");
                    $(dvdialog).dialog("close");
                    return false;
                }
            }
        });
        return false;
    }
    else {
        window.close();
        return false;
    }
}

function OpenFindAllAppointments() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var FindHumanID = document.getElementById(GetClientId("txtAccount")).value
    if (FindHumanID == "") {
        OpenFindPatient(TimeSlotFindAllAppointmentsClick);//checked
    }
    else {
        var obj = new Array();
        obj.push("HumanID=" + FindHumanID);
        obj.push("IsFindPatientRequired=N");
        openModal("frmFindAllAppointments.aspx", 460, 900, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.add_close(FindAllAppointmentClick);//checked
    }
    return false;
}



function TimeSlotFindAllAppointmentsClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != null) {
        if (Result.HumanID == "") {
            return false;
        }
        var obj = new Array();
        obj.push("HumanID=" + Result.HumanId);
        obj.push("IsFindPatientRequired=N");
        window.setTimeout(function () {
            openModal("frmFindAllAppointments.aspx", 460, 900, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(FindAllAppointmentClick);
        }, 50);
    }
}



function FindAllAppointmentClick(oWindow, args) {
    oWindow.close();
}

function OpenFindPatient(AddCloseMethod) {
    var obj = new Array();
    obj.push("ScreenName=Appointments");
    StartLoadingImage();
    openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.add_close(AddCloseMethod);
}