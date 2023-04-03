function SummaryAlert()
{
    document.getElementById('summarydiv').style.display = "block";
    document.getElementById('divSummary').style.display = "none";
    document.getElementById('xslFrame').style.display = "none";
    
}

function SummaryHumanIDAlert(sMessage) {
    document.getElementById('summarydiv').style.display = "block";
    document.getElementById('divSummary').style.display = "none";
    document.getElementById('xslFrame').style.display = "none";
    //GitLab #3933  
    document.getElementById('summarydiv').innerText = sMessage + " is not found. Please contact support team to regenerate the XML.";
    
}


function SummaryTimeStamp(totaltime, transormtime) {
    console.log("TotalTime:" + totaltime + "ms");
    console.log("TransormTime:" + transormtime + "ms");
}
function OnSuccessSummaryBar(response) {
    var regex = /<BR\s*[\/]?>/gi;

    if (response != null) {

        top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = response.d[0];
        top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = response.d[1];
        top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = response.d[2];
        top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = response.d[3];
        top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = response.d[4];
        if (response.d[5].replace("Allergies :<br/>", "").length != 0)
            top.window.document.getElementById("Allergies_tooltp").innerText = response.d[5].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Allergies_tooltp").innerText = "";
        if (response.d[6].replace("Chief Complaints :<br/><br/>", "").length != 0)
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = response.d[6].replace(regex, "\n").split("&#xA;").join("\n") + "\n";
        else
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = "";
        if (response.d[7].replace("Problem List :<br/>", "").length != 0)
            top.window.document.getElementById("ProblemList_tooltp").innerText = response.d[7].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("ProblemList_tooltp").innerText = "";
        if (response.d[8].replace("Vitals :<br/>", "").length != 0)
            top.window.document.getElementById("Vitals_tooltp").innerText = response.d[8].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Vitals_tooltp").innerText = "";
        if (response.d[9].replace("Medication :<br/>", "").length != 0)
            top.window.document.getElementById("Medication_tooltp").innerText = response.d[9].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Medication_tooltp").innerText = "";
        RefreshOverallSummaryTooltip();

    }
    var sDtls = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
    document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
        "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
        window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
        window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];

}



function OpenEfax(sFaxSubject, sRefProvider)
{
    $(top.window.document).find("#TabFax").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabFax").css({ "z-index:": "5001" });
    $(top.window.document).find("#TabModalEFaxTitle")[0].textContent = "Efax";
    $(top.window.document).find("#TabmdldlgEFax")[0].style.width = "1050px";
    $(top.window.document).find("#TabmdldlgEFax")[0].style.height = "963px";
    $(top.window.document).find("#TabmdldlgEFax").css({ "margin-left": "100px" });
    var sPath = ""
    sPath = "frmEFax.aspx?ProgressNotes=" + document.getElementById('hdnFilePath').value + "&RefProvider=" + sRefProvider;
    $(top.window.document).find("#TabEFaxFrame")[0].style.height = "659px";
    $(top.window.document).find("#TabEFaxFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabFax").one("hidden.bs.modal", function (e) {
    });
    
    localStorage['FaxSubject'] = "";
    localStorage['FaxSubject'] = sFaxSubject.replace("__", "_");
    return false;
}
$(document).ready(function () {
    document.title = "Summary";
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
     var $target = $('#xslFrame');

     if (localStorage.getItem("SummaryTab") == "true") {
         document.getElementById('btnServiceProcedureCode').style.display = "none";
         document.getElementById('btnServiceProcedureCode').removeAttribute("class");
     }
    $("#txtSearch").on('input', function (e) {

        $("#xslFrame").unhighlight();
        $("#xslFrame").highlight($("#txtSearch").val());
    });
   
       document.onkeydown = function (e) {
        e = e || window.event;//Get event
        if (e.ctrlKey) {
            var c = e.which || e.keyCode;//Get key code
            switch (c) {
                case 70://Block Ctrl+F
                    e.preventDefault();
                    e.stopPropagation();
                    break;
            }
        }
    };
});


function downloadFile() {
  
    $("#btnword")[0].click();
    document.title = "Progress Notes";
}
function downloadFilePDF() {

    $("#btnpdf")[0].click();
    document.title = "Progress Notes";
}
function downloadFileFax() {

    $("#btnFax")[0].click();
    document.title = "Progress Notes";
    return false;
}
function downloadPatientdocuments() {
   
    $("#btnPrint").click();
    document.title = "Patient Documents";
}
function OpenPhoneEncounterCancelAppt() {
    if (document.getElementById('hdnBatchStatus').value == "OPEN") {
        var obj = new Array();
        obj.push("EncounterID=" + document.getElementById('hdnEncounterId').value);
        obj.push("PhoneEncounter=Y")
        openModal("frmCancelAppointment.aspx", 240, 520, obj, "RadViewer");
        var WindowName = $find('RadViewer');
        WindowName.add_close(function ClosePhoneEnc(oWindow, args) {
           self.close();
           });
    }
    else {
        DisplayErrorMessage('7430012');
    }


   //  WindowName.add_close(CancelRefreshSchedular);//checked
    //$(top.window.document).find("#TabFax").modal({ backdrop: "static", keyboard: false }, 'show');
    //$(top.window.document).find("#TabFax").css({ "z-index:": "5001" });
    //$(top.window.document).find("#TabModalEFaxTitle")[0].textContent = "Phone Encounter Cancel Appointment";
    //$(top.window.document).find("#TabmdldlgEFax")[0].style.width = "1050px";
    //$(top.window.document).find("#TabmdldlgEFax")[0].style.height = "963px";
    //$(top.window.document).find("#TabmdldlgEFax").css({ "margin-left": "100px" });
    //var sPath = ""
    //sPath = "frmCancelAppointment.aspx?EncounterID=0";//+ Encounterid;
    //$(top.window.document).find("#TabEFaxFrame")[0].style.height = "659px";
    //$(top.window.document).find("#TabEFaxFrame")[0].contentDocument.location.href = sPath;
    //$(top.window.document).find("#TabFax").one("hidden.bs.modal", function (e) {
    //});
    //return false;
}

function OpenServiceProcedureCode() {
    $.ajax({
        type: "POST",
        url: "frmSummaryNew.aspx/CheckServiceProcedureCodeStatus",
        data: JSON.stringify({
            "Encounter": '',
        }),
        contentType: "application/json;charset=utd-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var Output = objdata.Return;

            if (Output == "Success") {
                var obj = new Array();
                $(top.window.document).find("#TabModalEandM").modal({ backdrop: 'static', keyboard: false });
                $(top.window.document).find("#TabmdldlgEandM")[0].style.width = "90%";
                $(top.window.document).find("#TabmdldlgEandM")[0].style.height = "85%";
                $(top.window.document).find("#TabModalTitleEandM")[0].textContent = "Service Procedure Code";
                $(top.window.document).find("#TabFrameEandM")[0].style.height = "100%";
                $(top.window.document).find("#TabFrameEandM")[0].contentDocument.location.href = "htmlEandMCoding.html?EnableScreen=True";
                $(top.window.document).find("#TabModalEandM").one("hidden.bs.modal", function () {
                    location.reload();
                });
                //$(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
                //    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                //    //OnClientCloseServiceProcedure();
                //});
                //var WindowName = $find('RadViewer');
                //WindowName.add_close(function CloseServiceProcedure(oWindow, args) {
                //    self.close();
                //    window.top.location.href = "frmPatientChart.aspx";
                //});
                
            }
            else {
                DisplayErrorMessage(Output);
                var obj = new Array();
                $(top.window.document).find("#TabModalEandM").modal({ backdrop: 'static', keyboard: false });
                $(top.window.document).find("#TabmdldlgEandM")[0].style.width = "90%";
                $(top.window.document).find("#TabmdldlgEandM")[0].style.height = "85%";
                $(top.window.document).find("#TabModalTitleEandM")[0].textContent = "Service Procedure Code";
                $(top.window.document).find("#TabFrameEandM")[0].style.height = "100%";
                $(top.window.document).find("#TabFrameEandM")[0].contentDocument.location.href = "htmlEandMCoding.html?EnableScreen=False";
                $(top.window.document).find("#TabModalEandM").one("hidden.bs.modal", function () {
                    location.reload();
                });

            }
        },
        error: function OnError(xhr) {
            AutoSaveUnsuccessful();
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }

    });
}

function PrintWellnessNote() {
    StartLoadingImage();
    $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=WELLNESS NOTES" + "&Menu=True";
    $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Wellness Notes";
    var DateTime = new Date();
    var strYear = DateTime.getFullYear();
    var strMonth = DateTime.getMonth() + 1;
    var strDay = DateTime.getDate();
    var strHours = DateTime.getHours();
    var strMinutes = DateTime.getMinutes();
    var strSeconds = DateTime.getSeconds();
    if (strMonth.toString().length == 1)
        strMonth = "0" + strMonth;
    if (strDay.toString().length == 1)
        strDay = "0" + strDay;
    if (strMinutes.toString().length == 1)
        strMinutes = "0" + strMinutes;
    var testStieng = strHours.toString() + ":" + strMinutes.toString() + ":" + strSeconds.toString();
    var timeString = testStieng.toString();
    var H = +timeString.substr(0, 2);
    var h = H % 12 || 12;
    var ampm = H < 12 ? "AM" : "PM";
    if (h.toString().length == 1)
        h = "0" + h;
    timeString = h + timeString.substr(2, 6) + ampm;
    // document.getElementById(GetClientId("hdnLocalTime")).value = strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", "");

    var obj = new Array();
    // obj.push("Date=" + document.getElementById(GetClientId("hdnLocalTime")).value);//document.getElementById(GetClientId("hdnLocalTime")).value
    obj.push("Date=" + strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", ""));

    StopLoadingImage();
}

function PrintTreatmentNote() {
    StartLoadingImage();
    $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=TREATMENT NOTES" + "&Menu=True";
    $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Treatment Notes";
    var DateTime = new Date();
    var strYear = DateTime.getFullYear();
    var strMonth = DateTime.getMonth() + 1;
    var strDay = DateTime.getDate();
    var strHours = DateTime.getHours();
    var strMinutes = DateTime.getMinutes();
    var strSeconds = DateTime.getSeconds();
    if (strMonth.toString().length == 1)
        strMonth = "0" + strMonth;
    if (strDay.toString().length == 1)
        strDay = "0" + strDay;
    if (strMinutes.toString().length == 1)
        strMinutes = "0" + strMinutes;
    var testStieng = strHours.toString() + ":" + strMinutes.toString() + ":" + strSeconds.toString();
    var timeString = testStieng.toString();
    var H = +timeString.substr(0, 2);
    var h = H % 12 || 12;
    var ampm = H < 12 ? "AM" : "PM";
    if (h.toString().length == 1)
        h = "0" + h;
    timeString = h + timeString.substr(2, 6) + ampm;
    //document.getElementById(GetClientId("hdnLocalTime")).value = strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", "");

    var obj = new Array();
    //obj.push("Date=" + document.getElementById(GetClientId("hdnLocalTime")).value);//document.getElementById(GetClientId("hdnLocalTime")).value
    obj.push("Date=" + strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", ""));
    StopLoadingImage();
}
function AkidoNoteClickSum(sAkidoURL) {

    Result = openNonModal(sAkidoURL, 780, 1250, obj);

    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;

    return false;
}