var humanid = null;
var tabContents = "";
var encounterid = "";
var objdata = "";
var lstchked = "";
var id = "";

$(document).ready(function () {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    document.getElementById("btnSubmit").disabled = true;
    $.ajax({
        type: "POST",
        url: "frmMyQueueArchive.aspx/LoadArchive",
        data: JSON.stringify({
            "sShowall": "t",
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            objdata = $.parseJSON(data.d);
            $('#MyQTable').empty();
            $("#tbodupolicyinfo").empty();
            let enabled = "enabled='true'";
            if (objdata.length > 0) {
                for (var i = 0; i < objdata.length; i++) {
                    var chiefcomplaints = "";
                    if (objdata[i].hpi_value != null && objdata[i].hpi_value != "") {
                        chiefcomplaints = objdata[i].hpi_value;
                    }
                    else {
                        chiefcomplaints = "";
                    }


                    if (objdata[i].encounter_id != null && objdata[i].encounter_id != "") {
                        id = objdata[i].encounter_id;
                    }
                    else {
                        id = "";
                    }

                    tabContents = "<tr><td style='width:13%'>" + ConvertDate(objdata[i].d.replace("T", " ")) + "</td>" +
                        "<td style='width:6%'>" + objdata[i].human_id + "</td>" +
                        "<td style='width:8%'>" + objdata[i].last_name + "," + objdata[i].first_name + " " + objdata[i].mi + "</td>" +
                        "<td style='width:8%'>" + DOBConvert(objdata[i].b.replace("T00:00:00", "")) + "</td>" +
                        "<td style='width:8%'>" + chiefcomplaints + "</td>" +
                        "<td style='width:12%'>" + objdata[i].Current_Process + "</td>" +
                        "<td style='width:3%;text-align: center;cursor: pointer;'><i class='glyphicon glyphicon-eye-open' id='" + objdata[i].human_id + "|" + id + "' onclick='ViewFile(this);' ></i></td>" +
                        "<td style='width:3%;text-align: center;'><input type='checkbox' class='myQChkbx' id='signencounter" + id + "' name='SignOnEncounter' onclick='signClick(" + id + ");' " + enabled + "/></td>" +
                        "<td style='width:3%;text-align: center;'><input type='checkbox' class='myQChkbx' id='workencounter" + id + "' name='WorkOnEncounter' onclick='workonClick(" + id + ")' " + enabled + "/></td>" +
                        "<td style='display:none'>" + id + "</td>" +
                        "</tr>";
                    $("#tbodyachiveinfo").append(tabContents);
                    StopLoadingImage();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            }
        }
    });
    StopLoadingImage();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
})


function ViewFile(e) {
    var humanid = e.id.split('|')[0];
    var encounterID = e.id.split('|')[1];
    if (humanid != null && humanid != "") {
        if (encounterID != null && encounterID != "") {
            var obj = new Array();
            obj.push("HumanID=" + humanid);
            obj.push("EncounterID=" + encounterID);
            
            var result = openModal("frmSummaryNew.aspx", 700, 1177, obj, "ctl00_RadWindow2");
            var result = $find('ctl00_RadWindow2');
        }
    }
}

function closeArchive() {

    if ($('input[type="checkbox"]:checked').length > 0) {
        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
                               "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';
        event.preventDefault();

        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'left' + " " + 'center',
                at: 'center' + " " + 'center + 100px'

            },
            buttons: {
                "Yes": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
                    document.getElementById('btnSubmit').click();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    btnSubmitButtonClicked()
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    self.close();
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    return false;
                }
            }
        });
    }
    else {
        self.close();
    }
}


function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate + ' UTC');
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-01-0001';
    else
        then = ('0' + now.getDate()).slice(-2) + '-' + monthNames[now.getMonth()] + '-' + now.getFullYear();
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


function DOBConvert(DOB) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
}


function signClick(evt) {
    document.getElementById("btnSubmit").disabled = false;
    var checkboxes = document.getElementsByName("SignOnEncounter");
    var workoncheckboxs = document.getElementsByName("WorkOnEncounter");

    for (var i = 0; i < checkboxes.length; i++) {
        var encid = objdata[i].encounter_id;

        if (evt == encid) {
            if (workoncheckboxs[i].type == 'checkbox' && workoncheckboxs[i].checked == true) {
                workoncheckboxs[i].checked = false;
                break;
            }
        }
    }
}


function workonClick(evt) {
    document.getElementById("btnSubmit").disabled = false;
    var checkboxes = document.getElementsByName("SignOnEncounter");
    var workoncheckboxs = document.getElementsByName("WorkOnEncounter");

    for (var i = 0; i < checkboxes.length; i++) {
        var encid = objdata[i].encounter_id;

        if (evt == encid) {
            if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
                checkboxes[i].checked = false;
                break;
            }
        }
    }
}


function btnSubmitButtonClicked() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    var checkboxes = document.getElementsByName("SignOnEncounter");
    var workoncheckboxs = document.getElementsByName("WorkOnEncounter");
    var inputData = new Array();
    for (var i = 0; i < checkboxes.length; i++) {
        var encid = objdata[i].encounter_id;
        var now = new Date();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();

        if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked == true) {
            inputData.push(encid + "~" + utc + "~" + "Sign");
        }
        else if (workoncheckboxs[i].type == 'checkbox' && workoncheckboxs[i].checked == true) {
            inputData.push(encid + "~" + utc + "~" + "Work");
        }

    }
    if (inputData.length > 0) {

        $.ajax({
            type: "POST",
            url: "frmMyQueueArchive.aspx/SubmitEncounters",
            data: JSON.stringify({
                "data": inputData,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                objdata = $.parseJSON(data.d);
                DisplayErrorMessage('230135');
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
    else {
        alert("Please select atleast one encounter for either sign-off or work-on.");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}

function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
   
    if (tblAsJQueryObject[0] != undefined) {
        $("#scrollID").css('height', '');
        $("#scrollID").css('overflow-y', '');
    }
}