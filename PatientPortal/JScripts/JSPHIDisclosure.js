$(document).ready(function () {
    //Applay click event for Telrick close button
    {
        var Closebutton = top?.window?.$("#RadWindowWrapper_RadWindowPhiDisclosure")?.find(".rwCloseButton")[0];
        const clone = Closebutton.cloneNode(true);
        clone.addEventListener('click', (e) => {
            CloseClick();
            return false;
        }, true);
        Closebutton.parentNode.replaceChild(clone, Closebutton);
    }
    document.getElementById("btnSave").disabled = true;
    $('#btnSave').attr("Autosave", "");
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    LoadPHIDetails();

    $("#lblSignedText")[0].innerText = "Electronically signed by " + $("#lblSubScreeenPatientStrip")[0].innerText.split('|')[0].trim();
});
function LoadPHIDetails() {

    var HumanId = new URLSearchParams(document.URL.split("?")[1]).get('PatientID');
    $.ajax({
        type: "POST",
        url: "frmPHIDisclosure.aspx/OnPageLoad",
        contentType: "application/json;charset=utf-8",
        data: '{sHumanId : "' + HumanId + '"}',
        datatype: "json",
        success: function success(data) {

            var PHIDetails = JSON.parse(data.d);
            PHIDetails = PHIDetails.PHIDetails;
            if (PHIDetails != "") {
                if (PHIDetails.Is_Disclose_All_Information == 'Y') {
                    $("#chkAll")[0].checked = true;
                } else if (PHIDetails.Is_Disclose_All_Information == 'N') {
                    $("#chkSelected")[0].checked = true;
                }

                if (PHIDetails.Disclosure_Details != "") {
                    var arrySavedPhiDetails = PHIDetails.Disclosure_Details.split('|');
                    $("#tblEncounterDetails input[type='checkbox']").each(function (Element) {
                        var selectediteam = arrySavedPhiDetails.filter(x => x == this.nextElementSibling.innerText);
                        if (selectediteam.length > 0) { this.checked = true; }
                        if (PHIDetails.Is_Disclose_All_Information == 'Y') { this.disabled = true; }
                    });
                }
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        },
        error: function onerror(xhr) {
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

function EnableSave(IsUncheckSign) {
    if (IsUncheckSign != 'N') {
        $("#chkSign")[0].checked = false;
    }
    document.getElementById("btnSave").disabled = false;
}

function CheckedChange(event) {
    if (event.id == "chkAll") {
        $("#tblEncounterDetails input[type='checkbox']").each(function (Element) {
            this.checked = true;
            this.disabled = true;
        });
    }
    else {
        $("#tblEncounterDetails input[type='checkbox']").each(function (Element) {
            this.checked = false;
            this.disabled = false;
        });
    }
    document.getElementById("tblEncounterDetails");
}

function CloseClick() {

    if (!$('#btnSave').attr("disabled")) {

        dvdialog = $("#dvdialogPHIDisclosure");
        myPos = "center center";
        atPos = 'center center';

        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: myPos,
                at: atPos
            },
            buttons: {
                "Yes": function () {

                    sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
                    $('#btnSave').attr("Autosave", "true");
                    $('#btnSave').click();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    $(dvdialog).dialog("close");
                    return false;
                },
                "No": function () {

                    self.close();
                    $(dvdialog).dialog("close");
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    return false;
                }
            }
        });
        $(".ui-dialog-titlebar-close")[0].innerText = "X";
        $(".ui-dialog-titlebar-close")[0].style.padding = "0px";
    }
    else {
        self.close();
        return false;
    }
    
}

function btnSaveClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("input[name='RadioGroups']:checked").length == 0) {
        $('#btnSave').attr("Autosave", "");
        alert("Please select atleast one Disclosure option.");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    var selectedList = $("#tblEncounterDetails input[type='checkbox']:checked")?.length;
    if (selectedList == 0) {
        $('#btnSave').attr("Autosave", "");
        alert("Please select atleast one Encounter details.");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    var PatientSign = document.getElementById("chkSign")?.checked;
    if (!PatientSign) {
        $('#btnSave').attr("Autosave", "");
        alert("Please sign the PHI Use and Disclosure.");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    var HumanId = new URLSearchParams(document.URL.split("?")[1]).get('PatientID');
    var IsSigned = $("#chkSign")[0].checked.toString();
    var Is_Disclose_All_Information = $("input[name='RadioGroups']:checked")[0].id == "chkAll" ? "Y" : "N";
    var SeletedPhiDetailsFunction = Array.from($("#tblEncounterDetails input[type='checkbox']:checked"))
        .map(checkbox => checkbox.nextElementSibling.innerText)
        .join("|");
    
    var sSignedText = $("#lblSignedText")[0].innerText;
    $.ajax({
        type: "POST",
        url: "frmPHIDisclosure.aspx/SavePHIDetails",
        contentType: "application/json;charset=utf-8",
        data: '{sHumanId : "' + HumanId + '",sIsSigned : "' + IsSigned + '",sSignedText : "' + sSignedText +'",sIs_Disclose_All_Information : "' + Is_Disclose_All_Information + '",sSeletedPhiDetails : "' + SeletedPhiDetailsFunction +'"}',
        datatype: "json",
        success: function success(data) {

            var FaxLoadList = JSON.parse(data.d);
            DisplayErrorMessage('1011301');
            document.getElementById("btnSave").disabled = true;

            if (document.getElementById("btnSave").getAttribute("Autosave") == "true") {
                $('#btnSave').attr("Autosave", "");
                self.close();
                $(dvdialog).dialog("close");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        },
        error: function onerror(xhr) {
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
    return false;
}

