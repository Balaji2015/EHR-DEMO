function LoadPartialDuplicatesMedicationGrid(humanID) {
    var ID = { ulHuman_id: humanID };
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
                    if (MedicationName != checkMedicationName) {

                        MedicationName = checkMedicationName;
                        colorIndex = colorIndex + 1;
                    }
                    var start_date = ConvertDate(result[iCount].Start_Date.replace("T", " "));
                    if (start_date.indexOf("0001-01-01") > -1) {
                        start_date = "";
                    }
                    var Stop_date = ConvertDate(result[iCount].Stop_Date.replace("T", " "));
                    if (Stop_date.indexOf("0001-01-01") > -1) {
                        Stop_date = "";
                    }
                    var Created_date_time = ConvertDate(result[iCount].Created_Date_And_Time.replace("T", " "));
                    

                    if (result[iCount].Status == "ACTIVE") {
                        TableContent = TableContent + "<tr style='color: " + ColorList[colorIndex] + ";'><td style='width:65px;'><input type='checkbox' onclick='EnableSave()' /></td><td style='width: 335px;'>"
                            + MedicationName + "</td><td>" + result[iCount].Patient_Notes + "</td><td>"
                            + result[iCount].Other_Notes + "</td><td>" + start_date + "</td><td>" + Stop_date
                            + "</td><td>" + result[iCount].Created_By + "</td><td>" + Created_date_time + "</td><td>" + result[iCount].Status
                            + "</td><td style='display:none' RcopiaMedicationId=" + result[iCount].Id + " >" + result[iCount].Id + "</td></tr>";
                    }
                    else {
                        TableContent = TableContent + "<tr style='display:none; color: " + ColorList[colorIndex] + ";'><td style='width:65px;'><input type='checkbox'/></td><td style='width: 335px;'>"
                            + MedicationName + "</td><td>" + result[iCount].Patient_Notes + "</td><td>"
                            + result[iCount].Other_Notes + "</td><td>" + ConvertDate(result[iCount].Start_Date.replace("T", " ")) + "</td><td>" + ConvertDate(result[iCount].Stop_Date.replace("T", " "))
                            + "</td><td>" + result[iCount].Created_By + "</td><td>" + ConvertDate(result[iCount].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + result[iCount].Status
                            + "</td><td style='display:none' RcopiaMedicationId=" + result[iCount].Id + " >" + result[iCount].Id + "</td></tr>";
                    }
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
        alert("Please select a row to delete");
    }
    else {
        var SelectMedicationCount = $('#RCopiaDuplicateMediationsTable  td').find('input[type=checkbox]:checked').length;
        var check = confirm("There are " + SelectMedicationCount +" medication(s) selected for delete. Do you want to remove duplicates?");
        if (check == true) {
            var RcopiamedicationIds = [];
            $('#RCopiaDuplicateMediationsTable  td').find('input[type=checkbox]:checked').each(function () {
                if ($($(this)?.parent()?.parent())?.find("td[RcopiaMedicationId]")[0]?.innerText != undefined && $($(this)?.parent()?.parent())?.find("td[RcopiaMedicationId]")[0]?.innerText != null) {
                    RcopiamedicationIds.push(parseInt($($(this).parent().parent()).find("td[RcopiaMedicationId]")[0].innerText));
                }
            });
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
                        top.window.document.getElementById("TabPatientMergeFrame").contentWindow.document.getElementById("btnMedication").click();
                        StartGenericStrip("Deleted Successfully.");
                        
                    }
                    else {
                        StopGenericStrip();
                        if (result.indexOf("DownloadRCopiaInfo") > -1) {
                            RcopiaErrorAlert(result.replace("DownloadRCopiaInfo-"));
                        }
                        else {
                            alert(result);
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
    var bCheckShowall = document.getElementById("chkShowAll").checked;
    $("#RCopiaDuplicateMediationsTableBody tr").find("td:nth-child(9)").each(function () {

        if (bCheckShowall == true) {
            $(this).parent()[0].style.display = "";
        }
        else if ($(this)[0].innerText == "INACTIVE") {
            $(this).parent()[0].style.display = "none";
        }
        else if (bCheckShowall == false && $(this)[0].innerText == "ACTIVE") {
            $(this).parent()[0].style.display = "";
        }

    });

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
    VisibleAllRows();
}

function EnableSave() {
    if ($("#btnDelete")[0]?.disabled != undefined && $("#btnDelete")[0]?.disabled != null) {
        $("#btnDelete")[0].disabled = false;
    }
}
              