$(document).ready(function () {

    LoadPatientDetails();
});
function Closepopup()
{
    if (!document.getElementById('btnsave').disabled) {
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
                    sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
                    SaveAppointmentStatus();
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btncloseImport").click();
                    if (sessionStorage.getItem("importCount") != null && sessionStorage.getItem("importCount") != undefined)
                        sessionStorage.removeItem("importCount");
                    loadImpportedPatient();
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btncloseImport").click();
                    if (sessionStorage.getItem("importCount") != null && sessionStorage.getItem("importCount") != undefined)
                        sessionStorage.removeItem("importCount");
                    loadImpportedPatient();
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
        $(top.window.document).find("#btncloseImport").click();
        if (sessionStorage.getItem("importCount") != null && sessionStorage.getItem("importCount") != undefined)
            sessionStorage.removeItem("importCount");
        loadImpportedPatient();
        return false;
    }
}
function enablesave()
{
    $('#btnsave').prop('disabled', false);
}
function LoadPatientDetailsshowall() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var tabContents = '';
    $.ajax({
        type: "POST",
        url: "frmImportedPatients.aspx/LoadPatientshowall",

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.length > 0) {

                for (var i = 0; i < objdata.length; i++) {
                    var capturedtime = '';
                    if (objdata[i].Modified_Date_And_Time != '0001-01-01T00:00:00')
                        capturedtime = DatatimeConvert(objdata[i].Modified_Date_And_Time.replace("T", " "));
                    else if (objdata[i].Created_Date_And_Time != '0001-01-01T00:00:00')
                        capturedtime = DatatimeConvert(objdata[i].Created_Date_And_Time.replace("T", " "));

                    var phoneno = "";
                    phoneno = objdata[i].Home_Phone_No;

                    if (objdata[i].Work_Phone_No != "" && phoneno != "")
                        phoneno = phoneno + ",<br/>" + objdata[i].Work_Phone_No;
                    else if (objdata[i].Work_Phone_No == "" && phoneno != "")
                        phoneno = phoneno
                    else
                        phoneno = objdata[i].Work_Phone_No;
                    if (objdata[i].Cell_Phone_Number != "" && phoneno != "")
                        phoneno = phoneno + ",<br/>" + objdata[i].Cell_Phone_Number;
                    else if (objdata[i].Cell_Phone_Number == "" && phoneno != "")
                        phoneno = phoneno;
                    else {
                        phoneno = objdata[i].Cell_Phone_Number;
                    }
                    var caregivername = "";
                    if (objdata[i].Care_Giver_Last_Name != "" && objdata[i].Care_Giver_First_Name != "")
                        caregivername = objdata[i].Care_Giver_Last_Name + "," + objdata[i].Care_Giver_First_Name
                    else if (objdata[i].Care_Giver_Last_Name != "")
                        caregivername = objdata[i].Care_Giver_Last_Name
                    else if (objdata[i].Care_Giver_First_Name != "")
                        caregivername = objdata[i].Care_Giver_First_Name


                    if (i == 0)
                        tabContents = "<tr id='" + objdata[i].Id + "'></td><td style='width:12%'><a onclick=opensummary(this)>" + objdata[i].Last_Name + "," + objdata[i].First_Name + "</a></td><td style='width:6%'>" + DOBConvert(objdata[i].Birth_Date.replace("T00:00:00", "")) + "</td><td style='width:7%'>" + objdata[i].Sex + "</td><td style='width: 7%;'>" + phoneno + "</td><td style='width: 7%;'>" + caregivername + "</td><td style='width: 7%;'>" + objdata[i].Care_Giver_Phone_Number + "</td><td style='width:12%'>" + capturedtime + "</td><td style='width:12%'><select Class='Editabletxtbox' style='width:162px' id='sel" + objdata[i].Id + "' onchange='enablesave()'> </select></td><td style='width:8%;display:none;'> <button type='button' class='btn btn-success'style='font-size:12px' onclick=OpenAppointment(this)>Schedule Appointment</button></td><td style='display:none'>" + objdata[i].Id + "</td><td style='display:none'>" + objdata[i].Appointment_Status + "</td></tr>";
                    else
                        tabContents = tabContents + "<tr id='" + objdata[i].Id + "'></td><td style='width:12%'><a onclick=opensummary(this)>" + objdata[i].Last_Name + "," + objdata[i].First_Name + "</a></td><td style='width:6%'>" + DOBConvert(objdata[i].Birth_Date.replace("T00:00:00", "")) + "</td><td style='width:7%'>" + objdata[i].Sex + "</td><td style='width: 7%;'>" + phoneno + "</td><td style='width: 7%;'>" + caregivername + "</td><td style='width: 7%;'>" + objdata[i].Care_Giver_Phone_Number + "</td><td style='width:12%'>" + capturedtime + "</td><td style='width:12%'><select Class='Editabletxtbox' style='width:162px' id='sel" + objdata[i].Id + "' onchange='enablesave()'> </select></td><td style='width:8%;display:none;'> <button type='button' class='btn btn-success'style='font-size:12px' onclick=OpenAppointment(this)>Schedule Appointment</button></td><td style='display:none'>" + objdata[i].Id + "</td><td style='display:none'>" + objdata[i].Appointment_Status + "</td></tr>";
                }
                $("#tblImportedPatients").append(tabContents);

                var selectValues = { "1": "Waiting for Call", "2": "Patient Refused", "3": "Unable to reach", "4": "Appointment Created" };
                $('[id^=sel] option').remove();
                $.each(selectValues, function (key, value) {
                    $('[id^=sel]')
                        .append($("<option></option>")
                                   .attr("value", key)
                                   .text(value));
                });


                $("select").each(function () {
                    $(this).find('option').filter(function () {
                        return this.text == $(this)[0].parentElement.parentElement.parentElement.childNodes[10].textContent;
                    }).attr('selected', true);

                })

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

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
function LoadPatientDetails()
{
    var tabContents = '';
    $.ajax({
        type: "POST",
        url: "frmImportedPatients.aspx/LoadPatient",

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            $('#dvImport').empty();

            if (objdata.length > 0) {

                for (var i = 0; i < objdata.length; i++) {
                    var capturedtime = '';
                    if (objdata[i].Modified_Date_And_Time != '0001-01-01T00:00:00')
                        capturedtime = DatatimeConvert(objdata[i].Modified_Date_And_Time.replace("T", " "));
                    else if (objdata[i].Created_Date_And_Time != '0001-01-01T00:00:00')
                        capturedtime = DatatimeConvert(objdata[i].Created_Date_And_Time.replace("T", " "));

                    var phoneno = "";
                    phoneno = objdata[i].Home_Phone_No;

                    if (objdata[i].Work_Phone_No != "" && phoneno != "")
                        phoneno = phoneno + ",<br/>" + objdata[i].Work_Phone_No;
                    else if (objdata[i].Work_Phone_No == "" && phoneno != "")
                        phoneno = phoneno
                    else
                        phoneno = objdata[i].Work_Phone_No;
                    if (objdata[i].Cell_Phone_Number != "" && phoneno != "")
                        phoneno = phoneno + ",<br/>" + objdata[i].Cell_Phone_Number;
                    else if (objdata[i].Cell_Phone_Number == "" && phoneno != "")
                        phoneno = phoneno;
                    else {
                        phoneno = objdata[i].Cell_Phone_Number;
                    }
                    var caregivername = "";
                    if (objdata[i].Care_Giver_Last_Name != "" && objdata[i].Care_Giver_First_Name != "")
                        caregivername = objdata[i].Care_Giver_Last_Name + "," + objdata[i].Care_Giver_First_Name
                    else if (objdata[i].Care_Giver_Last_Name != "")
                        caregivername = objdata[i].Care_Giver_Last_Name
                    else if (objdata[i].Care_Giver_First_Name != "")
                        caregivername = objdata[i].Care_Giver_First_Name


                    if (i == 0)
                        tabContents = "<tr id='" + objdata[i].Id + "'></td><td style='width:12%'><a onclick=opensummary(this)>" + objdata[i].Last_Name + "," + objdata[i].First_Name + "</a></td><td style='width:6%'>" + DOBConvert(objdata[i].Birth_Date.replace("T00:00:00", "")) + "</td><td style='width:7%'>" + objdata[i].Sex + "</td><td style='width: 7%;'>" + phoneno + "</td><td style='width: 7%;'>" + caregivername + "</td><td style='width: 7%;'>" + objdata[i].Care_Giver_Phone_Number + "</td><td style='width:12%'>" + capturedtime + "</td><td style='width:12%'><select Class='Editabletxtbox' style='width:162px' id='sel" + objdata[i].Id + "' onchange='enablesave()'> </select></td><td style='width:8%;display:none;'> <button type='button' class='btn btn-success'style='font-size:12px' onclick=OpenAppointment(this)>Schedule Appointment</button></td><td style='display:none'>" + objdata[i].Id + "</td><td style='display:none'>" + objdata[i].Appointment_Status + "</td></tr>";
                    else
                        tabContents = tabContents + "<tr id='" + objdata[i].Id + "'></td><td style='width:12%'><a onclick=opensummary(this)>" + objdata[i].Last_Name + "," + objdata[i].First_Name + "</a></td><td style='width:6%'>" + DOBConvert(objdata[i].Birth_Date.replace("T00:00:00", "")) + "</td><td style='width:7%'>" + objdata[i].Sex + "</td><td style='width: 7%;'>" + phoneno + "</td><td style='width: 7%;'>" + caregivername + "</td><td style='width: 7%;'>" + objdata[i].Care_Giver_Phone_Number + "</td><td style='width:12%'>" + capturedtime + "</td><td style='width:12%'><select Class='Editabletxtbox' style='width:162px' id='sel" + objdata[i].Id + "' onchange='enablesave()'> </select></td><td style='width:8%;display:none;'> <button type='button' class='btn btn-success'style='font-size:12px' onclick=OpenAppointment(this)>Schedule Appointment</button></td><td style='display:none'>" + objdata[i].Id + "</td><td style='display:none'>" + objdata[i].Appointment_Status + "</td></tr>";
                }
                $("#dvImport").append("<table id=tblImportedPatients class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>DOB</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Gender</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>Phone#</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Care Giver</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Care Giver Phone #</th><th style='border: 1px solid #909090;text-align: center;width:12%;'>Captured Date Time</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appointment status</th><th style='border: 1px solid #909090;text-align: center;width: 13%;display:none;'>Schedule</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");


                var selectValues = { "1": "Waiting for Call", "2": "Patient Refused", "3": "Unable to reach", "4": "Appointment Created" };
                $.each(selectValues, function (key, value) {
                    $('[id^=sel]')
                        .append($("<option></option>")
                                   .attr("value", key)
                                   .text(value));
                });

            }
            else {
                $("#dvImport").append("<table id=tblImportedPatients class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>DOB</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Gender</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>Phone#</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Care Giver</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Care Giver Phone #</th><th style='border: 1px solid #909090;text-align: center;width:12%;'>Captured Date Time</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appointment status</th><th style='border: 1px solid #909090;text-align: center;width: 13%;display:none;'>Schedule</th></tr></thead><tbody style='word-wrap: break-word;'></tbody></table>");
            }
            sessionStorage.setItem('StartLoading', 'false');
            $('#dvImport').css('cursor', 'default')
               
              
                $('#resultLoading')[0].style.display = "none";
               
                $('#resultLoading.bg').height('100%');

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

function Openimportedsummary(url) {
    
    Result = openWindowNonModalsumm(url, 720, 860, obj);
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;
}
function openWindowNonModalsumm(fromname, height, width, inputargument) {
    
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
    if (result!=null)
    result.moveTo(((screen.width - width) / 2), ((screen.height - height) / 2));

    if (result == undefined) { result = window.returnValue; }
    return result;
}
function opensummary(e) {
    var human_id = e.parentElement.parentElement.childNodes[9].textContent
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        url: "frmImportedPatients.aspx/GetEncounterId",
        data: "{'human_id':'" + human_id + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var obj = new Array();
            obj.push("EncounterId=" + objdata);
          
            obj.push("IsImported=" + "Y");
            //var url = "frmSummaryNew.aspx?EncounterId=" + objdata + "&IsImported=" + 'Y';
            var url = "frmSummaryNew.aspx?EncounterId=" + objdata + "&IsImported=" + 'Y' +"&TabMode=true";
                           
            Openimportedsummary(url);

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
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }
    });
}
function OpenAppointment(e)
{
  
    var obj = new Array();
    obj.push("Human_id=" + e.parentElement.parentElement.childNodes[9].textContent);
    obj.push("PatientName=" + e.parentElement.parentElement.childNodes[0].textContent);
    obj.push("PatientDOB=" + e.parentElement.parentElement.childNodes[1].textContent);
    obj.push("HumanType=" + "");
    obj.push("Home_Phone=" + e.parentElement.parentElement.childNodes[3].textContent.split(',')[0]);
    obj.push("Cell_Phone=" + "");
    obj.push("Encounter_Provider_ID=" +0);
    obj.push("EncounterID=" + 0);
    obj.push("facility=" + "");
    obj.push("PhysicianName=" + '');
    obj.push("PhysicianID=" + 0);
    obj.push("Imported=" + "Y");
    var Datetime = new Date();

    var appointmentdate = Datetime.getFullYear() + "-" + (parseInt(Datetime.getMonth()) + 1) + "-" + Datetime.getDate() + " " + Datetime.getHours() + ":" + Datetime.getMinutes() + ":" + Datetime.getSeconds();
    obj.push("SelectedDate=" + appointmentdate);
    obj.push("CurrentProcess=" + "");
    window.setTimeout(function () {
     
        sessionStorage.setItem("EditAppointmentTransfer", new Date());
        openModal("frmEditAppointment.aspx", 720, 860, obj, "ctl00_ModalWindow");
   
    }, 50);
    return false;
    }
function DOBConvert(DOB) {

   
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
}

function DatatimeConvert(utcDate)
{
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
 "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate + ' UTC');
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
function showall()
{
    if ($('#chkshowall')[0].checked)
    {
        LoadPatientDetailsshowall();
    }
    else {
        LoadPatientDetails();

    }
}
function SaveAppointmentStatus() {
    var tabContents = '';
    var output = "";
    var humanId = new Array();;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $("#tblImportedPatients tbody tr").each(function () {
      
        var textval = $(this).find("td").eq(9)[0].textContent;
        var ddlval = $(this).find("td").eq(7).find("select option:selected").text();
        if (!($('#chkshowall')[0].checked)) {
            if (ddlval.toUpperCase() != "WAITING FOR CALL") {
                humanId.push(textval);

                if (output != "")
                    output = output + "|" + textval + "~" + ddlval;
                else
                    output = textval + "~" + ddlval;
            }
        }
        else {
            var temp = $(this).find("td").eq(10)[0].textContent;
            if (temp.toUpperCase() != ddlval.toUpperCase()) {
                humanId.push(textval);

                if (output != "")
                    output = output + "|" + textval + "~" + ddlval;
                else
                    output = textval + "~" + ddlval;

                $(this).find("td").eq(10)[0].textContent = ddlval;
            }

        }

    })
    if (output != '') {
        $.ajax({
            type: "POST",
            url: "frmImportedPatients.aspx/SaveAppointmentStatus",
            data: "{'HumanDetails':'" + output + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                var objdata = $.parseJSON(data.d);
                if (!($('#chkshowall')[0].checked)) {
                    for (var i = 0; i < humanId.length; i++) {
                        $("tr[id='" + humanId[i] + "']").remove();
                    }
                }
                DisplayErrorMessage('110020');
                $('#btnsave').prop('disabled', true);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

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
function ScheduleAppointment(e) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var url = "";
    url = "frmappointments.aspx?hdnSourceScreen=Menu";
    
    OpenAppointments(url);

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    
}
function OpenAppointments(url) {
    
    Result = openWindowNonModalsumm(url, 900, 1250, obj);
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;
}