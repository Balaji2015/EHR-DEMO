
top.window.document.addEventListener("visibilitychange", function () {
    VerifyScreenAndDownloadRcopia();
});

$(top.window).on("blur", function () {
    VerifyScreenAndDownloadRcopia();
});


function VerifyScreenAndDownloadRcopia() {
    //eRx for Menu,Tab,Task
    if ((top.window.$("#RadWindowWrapper_ctl00_ModalWindow")[0]?.innerHTML?.indexOf("frmRCopiaWebBrowser") != undefined
        && top.window.$("#RadWindowWrapper_ctl00_ModalWindow")[0]?.innerHTML?.indexOf("frmRCopiaWebBrowser") > -1
        && top.window.$("#RadWindowWrapper_ctl00_ModalWindow")[0].style.display.indexOf("none") == -1)
        ||
        (top.window.document?.getElementById('ctl00_C5POBody_EncounterContainer')?.contentWindow?.document?.querySelector('#myTabs .active')?.id?.indexOf("tbEPrescription") != undefined
            && top.window.document?.getElementById('ctl00_C5POBody_EncounterContainer').contentWindow.document.querySelector('#myTabs .active').id.indexOf("tbEPrescription") > -1)
        ||
        (top.window.document?.URL?.indexOf("frmRCopiaWebBrowser") != undefined
            && top.window.document?.URL?.indexOf("frmRCopiaWebBrowser") > -1)) {
        RcopiaDownload();
    }

    //eRx for ViewResult and MyQ>MyPriscription
    if ((top.window.document?.getElementById('TabViewResultFrame')?.contentWindow?.document?.getElementById("RadWindowWrapper_ctl00_ModalWindow")?.innerHTML?.indexOf("frmRCopiaWebBrowser") != undefined
        && top.window.document?.getElementById('TabViewResultFrame')?.contentWindow?.document?.getElementById("RadWindowWrapper_ctl00_ModalWindow")?.innerHTML?.indexOf("frmRCopiaWebBrowser") > -1)
        ||
        (top.window.document?.getElementById("RadWindowWrapper_MessageWindow")?.innerHTML?.indexOf("frmRCopiaWebBrowser") != undefined
            && top.window.document?.getElementById("RadWindowWrapper_MessageWindow")?.innerHTML?.indexOf("frmRCopiaWebBrowser") > -1
            && top.window.document?.getElementById("RadWindowWrapper_MessageWindow")?.style?.display?.indexOf("none") == -1)) {
        RcopiaDownloadOutSidePatientChart();

    }


}

function RcopiaDownload() {
    StartRcopiaStrip();
    $.ajax({
        type: "POST",
        url: "frmEncounter.aspx/DownloadRcoipa",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            //Jira CAP-1366
            StopRcopiaStrip();
            RcopiaErrorAlert(data.d);
            if (top?.window?.document?.URL?.indexOf("frmPatientChart") != undefined && top.window.document.URL.indexOf("frmPatientChart") > -1) {
                reloadSummaryEprescription();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        },
        error: function (result) {
            //Jira CAP-1366
            StopRcopiaStrip();
            var log = JSON.parse(result.responseText);
            console.log(log);
            if (result.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else
                alert("USER MESSAGE:\n" +
                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    "Message: " + log.Message);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}
function RcopiaDownloadOutSidePatientChart() {
    StartRcopiaStrip();
    $.ajax({
        type: "POST",
        url: "frmEncounter.aspx/DownloadRcoipaOutSidePatientChart",
        data: '{sHuman_id: "' + sessionStorage.getItem("eRxHumanID") + '"}',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            //Jira CAP-1366
            StopRcopiaStrip();
            RcopiaErrorAlert(data.d);
        },
        error: function (result) {
            //Jira CAP-1366
            StopRcopiaStrip();
            var log = JSON.parse(result.responseText);
            console.log(log);
            if (result.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else
                alert("USER MESSAGE:\n" +
                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    "Message: " + log.Message);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}
$(document).ready(function () {
    document.cookie = "CeRxFlag=true";
});
function reloadSummaryEprescription() {
    var enc_id = sessionStorage.getItem("EncId_PatSummaryBar");
    var enc_DOS = sessionStorage.getItem("Enc_DOS");
    //sessionStorage.removeItem("EncId_PatSummaryBar");
    //sessionStorage.removeItem("Enc_DOS");
    $.ajax({
        type: "POST",
        url: "frmRCopiaToolbar.aspx/LoadPatientSummaryBar",
        // data: JSON.stringify({ EncID: "", Enc_DOS: "" }),
        data: JSON.stringify({ EncID: enc_id, Enc_DOS: enc_DOS }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessSummaryBarEprescription,
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

    //LoadNotification("Notify");
}
function OnSuccessSummaryBarEprescription(response) {
    var regex = /<BR\s*[\/]?>/gi;

    if (response != null) {
        //CAP-795  Cannot read properties of null
        if (top?.window?.document?.getElementById("ctl00_C5POBody_lblAllergies") != undefined && top?.window?.document?.getElementById("ctl00_C5POBody_lblAllergies") != null) {
            top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = response.d[0];
        }
        if (top?.window?.document?.getElementById("ctl00_C5POBody_lblCheifComplaints") != undefined && top?.window?.document?.getElementById("ctl00_C5POBody_lblCheifComplaints") != null) {
            top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = response.d[1];
        }
        if (top?.window?.document?.getElementById("ctl00_C5POBody_lblProblemList") != undefined && top?.window?.document?.getElementById("ctl00_C5POBody_lblProblemList") != null) {
            top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = response.d[2];
        }
        if (top?.window?.document?.getElementById("ctl00_C5POBody_lblVitals") != undefined && top?.window?.document?.getElementById("ctl00_C5POBody_lblVitals") != null) {
            top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = response.d[3];
        }
        if (top?.window?.document?.getElementById("ctl00_C5POBody_lblMedication") != undefined && top?.window?.document?.getElementById("ctl00_C5POBody_lblMedication") != null) {
            top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = response.d[4];
        }
        if (response.d[5].replace("Allergies :<br/>", "").length != 0)
            top.window.document.getElementById("Allergies_tooltp").innerText = response.d[5].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Allergies_tooltp").innerText = "";
        //CAP-1614
        if (top?.window?.document?.getElementById("CheifComplaints_tooltp") != undefined && top?.window?.document?.getElementById("CheifComplaints_tooltp") != null) {
            if (response.d[6].replace("Chief Complaints :<br/><br/>", "").length != 0)
                top.window.document.getElementById("CheifComplaints_tooltp").innerText = response.d[6].replace(regex, "\n").split("&#xA;").join("\n") + "\n";
            else
                top.window.document.getElementById("CheifComplaints_tooltp").innerText = "";
        }
        if (response.d[7].replace("Problem List :<br/>", "").length != 0)
            top.window.document.getElementById("ProblemList_tooltp").innerText = response.d[7].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("ProblemList_tooltp").innerText = "";
        if (response.d[8].replace("Vitals :<br/>", "").length != 0)
            top.window.document.getElementById("Vitals_tooltp").innerText = response.d[8].replace(regex, "\n") + "\n";
        else
            //CAP-1463
            if (top?.window?.document?.getElementById("Vitals_tooltp") != undefined && top?.window?.document?.getElementById("Vitals_tooltp") != null) {
                top.window.document.getElementById("Vitals_tooltp").innerText = "";
            }

        if (response.d[9].replace("Medication :<br/>", "").length != 0)
            top.window.document.getElementById("Medication_tooltp").innerText = response.d[9].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Medication_tooltp").innerText = "";
        RefreshOverallSummaryTooltip();

    }
    //CAP-1463
    var sDtls = window?.parent?.parent?.document?.getElementsByName('lblPatientStrip')[0]?.innerText;
    document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
        "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
        window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
        window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];

}