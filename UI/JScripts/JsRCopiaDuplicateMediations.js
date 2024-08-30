function LoadPartialDuplicatesMedicationGrid(humanID, ShowAll) {
    $("#RCopiaDuplicateMediationsTableBody").children().remove();
    var ID = { ulHuman_id: humanID, sShowAll: ShowAll };
    $.ajax({
        type: "POST",
        url: "frmRCopiaDuplicateMediations.aspx/LoadMedicationGrid",
        data: JSON.stringify(ID),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var result = $.parseJSON(data.d).PartialDupicateMedications;
            ColorList = GetColors();
            var colorIndex = -1;
            var MedicationName = "";
            var TableContent = "";
            if (result.length > 0) {
                for (iCount = 0; iCount < result.length; iCount++) {
                    var checkMedicationName = result[iCount].Brand_Name + " " + result[iCount].Strength + " : " + result[iCount].Form + " "
                        + result[iCount].Dose + " " + result[iCount].Dose_Unit + " " + result[iCount].Route + " " + result[iCount].Dose_Timing;
                    //Set colour for Pairs
                    if (MedicationName != checkMedicationName) {

                        MedicationName = checkMedicationName;
                        colorIndex = colorIndex + 1;
                    }

                    var start_date = ConvertDate(result[iCount].Start_Date.replace("T", " "),"WithOutTime");
                    if (start_date.indexOf("0001-01-01") > -1) {
                        start_date = "";
                    }
                    var Stop_date = ConvertDate(result[iCount].Stop_Date.replace("T", " "), "WithOutTime");
                    if (Stop_date.indexOf("0001-01-01") > -1) {
                        Stop_date = "";
                    }
                    var Created_date_time = ConvertDate(result[iCount].Created_Date_And_Time.replace("T", " "), "WithTime");


                    TableContent = TableContent + "<tr style='color: " + ColorList[colorIndex] + ";'><td style='width:65px;'><input type='checkbox' onclick='EnableSave()' /></td><td style='width: 335px;'>"
                        + MedicationName + "</td><td>" + result[iCount].Patient_Notes + "</td><td>"
                        + result[iCount].Other_Notes + "</td><td>" + start_date + "</td><td>" + Stop_date
                        + "</td><td>" + result[iCount].Created_By + "</td><td>" + Created_date_time + "</td><td>" + result[iCount].Status
                        + "</td><td style='display:none' RcopiaMedicationId=" + result[iCount].Id + " >" + result[iCount].Id + "</td></tr>";

                }
                $("#RCopiaDuplicateMediationsTableBody").append(TableContent);
            }
            else {
                $("#RCopiaDuplicateMediationsTableBody").append("");
            }


            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                //CAP-792
                if (isValidJSON(xhr.responseText)) {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);

                    if (log.Message.indexOf("Human XML is invalid") == -1 && log.Message.indexOf("Human XML is not found") == -1 && log.Message.indexOf("Encounter XML is invalid") == -1 && log.Message.indexOf("Encounter XML is not found") == -1) {
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                            "Message: " + log.Message);
                    }
                }
                else {
                    if (xhr.responseText.indexOf("Human XML is invalid") == -1 && xhr.responseText.indexOf("Human XML is not found") == -1 && xhr.responseText.indexOf("Encounter XML is invalid") == -1 && xhr.responseText.indexOf("Encounter XML is not found") == -1) {
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry.");
                    }
                }
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }

    });
    $("#btnDelete")[0].disabled = true;
}

function DeleteMedication() {
    
    var SelectedMedication = $('#RCopiaDuplicateMediationsTable  td').find('input[type=checkbox]:checked').length;
    if (SelectedMedication == 0) {
        DisplayErrorMessage('10113605');
    }
    else {
        var SelectMedicationCount = $('#RCopiaDuplicateMediationsTable  td').find('input[type=checkbox]:checked').length;
        var isInActive = false;
        if (DisplayErrorMessage('10113606', '', SelectMedicationCount.toString()) == true) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var RcopiamedicationIds = [];
            $('#RCopiaDuplicateMediationsTable  td').find('input[type=checkbox]:checked').each(function () {
                if ($($(this)?.parent()?.parent())?.find("td[RcopiaMedicationId]")[0]?.innerText != undefined && $($(this)?.parent()?.parent())?.find("td[RcopiaMedicationId]")[0]?.innerText != null) {
                    RcopiamedicationIds.push(parseInt($($(this).parent().parent()).find("td[RcopiaMedicationId]")[0].innerText));
                }
                if ($($(this)?.parent()?.parent())?.find("td:nth-child(9)")[0]?.innerText != null && $($(this)?.parent()?.parent())?.find("td:nth-child(9)")[0]?.innerText != undefined) {
                    if ($($(this).parent().parent()).find("td:nth-child(9)")[0].innerText.toUpperCase() == "INACTIVE") {
                        isInActive = true;
                    }
                }
            });
            if (isInActive) {
                if (DisplayErrorMessage('10113611') == false) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return true;
                }

            }
            var DeleteList = { MedicationIds: RcopiamedicationIds };
            StartGenericStrip("Data is being deleted in DrFirst. Please wait.");
            $.ajax({
                type: "POST",
                url: "frmRCopiaDuplicateMediations.aspx/DeleteMedications",
                data: JSON.stringify(DeleteList),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

                    var result = $.parseJSON(data.d).status;
                    if (result == "Success") {
                        StopGenericStrip();
                        //top.window.document.getElementById("TabPatientMergeFrame").contentWindow.document.getElementById("btnMedication").click();
                        document.getElementById("txtSearcMedication").value = "";
                        document.getElementById("chkShowAll").checked = false;
                        var human_id = document.URL.slice(document.URL.indexOf("HumanID")).split("&")[0].split("=")[1];
                        LoadPartialDuplicatesMedicationGrid(human_id, "ACTIVE");
                        StartGenericStrip("Deleted Successfully.");
                        
                    }
                    else {
                        StopGenericStrip();
                        if (result.indexOf("DownloadRCopiaInfo") > -1) {
                            RcopiaErrorAlert(result.replace("DownloadRCopiaInfo-"));
                        }
                        else {
                            DisplayErrorMessage('10113607','', result);
                        }
                    }

                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                },
                error: function OnError(xhr) {
                    StopGenericStrip();
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        //CAP-792
                        if (isValidJSON(xhr.responseText)) {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);

                            if (log.Message.indexOf("Human XML is invalid") == -1 && log.Message.indexOf("Human XML is not found") == -1 && log.Message.indexOf("Encounter XML is invalid") == -1 && log.Message.indexOf("Encounter XML is not found") == -1) {
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                    "Message: " + log.Message);
                            }
                        }
                        else {
                            if (xhr.responseText.indexOf("Human XML is invalid") == -1 && xhr.responseText.indexOf("Human XML is not found") == -1 && xhr.responseText.indexOf("Encounter XML is invalid") == -1 && xhr.responseText.indexOf("Encounter XML is not found") == -1) {
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry.");
                            }
                        }
                    }
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }

            });
        }
    }

}
function ShowAllMedication() {
    document.getElementById("txtSearcMedication").value = "";
    var bCheckShowall = document.getElementById("chkShowAll").checked;
    if (bCheckShowall) {
        showllchk = "ALL";
    }
    else {
        showllchk = "ACTIVE";
    }
    var human_id = document.URL.slice(document.URL.indexOf("HumanID")).split("&")[0].split("=")[1];

    LoadPartialDuplicatesMedicationGrid(human_id, showllchk)

}

function SearchMedication() {
    var MedicationName = document.getElementById("txtSearcMedication").value;
    var bCheckShowall = document.getElementById("chkShowAll").checked;
    $("#RCopiaDuplicateMediationsTableBody tr").find("td:nth-child(2)").each(function () {

        if ($(this)[0].innerText.toLowerCase().indexOf(MedicationName.toLowerCase()) > -1 && (bCheckShowall == true || (bCheckShowall == false && $(this).parent().find("td:nth-child(9)")[0].innerText == 'ACTIVE'))) {
            $(this).parent()[0].style.display = "";
        }
        else {
            $(this).parent()[0].style.display = "none";
        }

    });
}

function VisibleAllRows() {
    ShowAllMedication();

}

function ClearAllSearch() {
    document.getElementById("txtSearcMedication").value = "";
    $("#RCopiaDuplicateMediationsTableBody tr").each(function () {
        $(this)[0].style.display = "";
    });
}

function EnableSave() {
    if ($("#btnDelete")[0]?.disabled != undefined && $("#btnDelete")[0]?.disabled != null) {
        $("#btnDelete")[0].disabled = false;
    }
}
              