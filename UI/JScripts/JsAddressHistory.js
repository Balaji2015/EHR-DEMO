
let months = {
    jan: "00", feb: "01", mar: "02", apr: "03", may: "04", jun: "05",
    jul: "06", aug: "07", sep: "08", oct: "09", nov: "10", dec: "11"
};

$(document).ready(function () { 
    document.getElementById("btnAdd").disabled = true;
    $('#btnAdd').attr("Autosave", "");
    loadPatientStrip();
    $("input.DateInput").inputmask({
        //mask: ["99-aaa-9999", "99-aaa-____", "__-aaa-9999", "99-___-9999", "99-___-____", "__-aaa-____", "__-__-9999"]
        mask: ["9999-aaa-99", "9999-___-99", "9999-aaa-__", "9999-___-__", "____-aaa-99", "____-___-99"],
        placeholder: "____-___-__",
        casing: "lower",
        clearMaskOnLostFocus: false
    });
    $("input.ZipCode").inputmask({
        mask: ["99999-9999", "99999"]
    });
    loadStates();
    LoadGrid();
});

function loadStates() {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/WebServices/AddressHistoryService.asmx/GetAllStats",
        data: JSON.stringify({}),
        dataType: "json",
        async: true,
        success: function (data) {
            var cboEmptyOption = document.createElement("option");
            cboEmptyOption.value = "";
            cboEmptyOption.text = "";
            document.getElementById("cboState").appendChild(cboEmptyOption);
            var data = $.parseJSON(data.d);
            data.states.forEach(function (option) { 
                
                var cboOption = document.createElement("option");
                cboOption.value = option;
                cboOption.text = option;
                document.getElementById("cboState").appendChild(cboOption);
            });

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });

}

function LoadGrid() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $("#tbLoadGrid").empty();
    var sHumanID = new URLSearchParams(document.URL.split("?")[1]).get('HumanID'); 
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/WebServices/AddressHistoryService.asmx/GetAddress_HistoryByHumanID",
        data: JSON.stringify({ "sHumanID": sHumanID }),
        dataType: "json",
        async: true,
        success: function (data) {
            var tbLoadGrid = document.getElementById("tbLoadGrid");
            var data = $.parseJSON(data.d);
            data.Address_History.forEach(function (option) {
                var tr = document.createElement("tr");
                var TableData = "<td  style='width: 40px;'><img src='Resources/edit.gif'  id='bntEdite' onclick='EditeClick(this)'/></td>" +
                    "<td  style='width: 40px;'><img src='Resources/close_small_pressed.png'  id='bntDelete' onclick='DeleteClick(this)'/></td>" +
                    "<td name='Street_Address1'>" + option.Street_Address1 + "</td>" +
                    "<td name='Street_Address2'>" + option.Street_Address2 + "</td>" +
                    "<td name='City'>" + option.City + "</td>" +
                    "<td name='State' style='width: 42px;'>" + option.State + "</td>" +
                    "<td name='ZipCode' value='" + option.ZipCode + "'>" + option.ZipCode.replaceAll("_", "").replace(/-+$/, "") + "</td>" +
                    "<td name='Start_Date' value='" + option.Start_Date + "'>" + option.Start_Date.replaceAll("_", "").replace(/-+$/, "").replace(/^-/, "").replace(/^-/, "").replace("--", "-") + "</td>" +
                    "<td name='End_Date' value='" + option.End_Date + "'>" + option.End_Date.replaceAll("_", "").replace(/-+$/, "").replace(/^-/, "").replace(/^-/, "").replace("--", "-") + "</td>" +
                    "<td name='Human_ID' style='display:none;'>" + option.Human_ID + "</td>" +
                    "<td name='Human_Address_ID' style='display:none;'>" + option.Id + "</td>";

                tr.innerHTML = TableData;
                tbLoadGrid.appendChild(tr);
            });

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}
function Validation() {
    var Address = document.getElementById("txtAddressLineOne");

    var StartDate = document.getElementById("dtStartDate").value;
    var EndDate = document.getElementById("dtEndDate").value;
    let [startyear, startmon, startday] = StartDate.replaceAll("_", "").split("-");
    let [endyear, endmon, endday] = EndDate.replaceAll("_", "").split("-");
    var sDOB = new URLSearchParams(document.URL.split("?")[1]).get('DOB');
    let [DOBDate, DOBMonth, DOBYear ] = sDOB.replaceAll("_", "").split("-");

    if (Address?.value != undefined && Address.value == "") {
        DisplayErrorMessage("10113702");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    var City = document.getElementById("txtCity");
    if (City?.value != undefined && City.value == "") {
        DisplayErrorMessage("10113703");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    var State = document.getElementById("cboState");
    if (State?.value != undefined && State.value == "") {
        DisplayErrorMessage("10113704");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    var ZipCode = document.getElementById("txtZipCode");
    if (ZipCode?.value != undefined && ZipCode.value == "") {
        DisplayErrorMessage("10113705");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (ZipCode.value.replace(/_/gi, "").length != 6 && ZipCode.value.replace(/_/gi, "").length != 10) {

        DisplayErrorMessage('10113706');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    //if (document.getElementById("dtStartDate").value == "" && document.getElementById("dtEndDate").value != "") {
    //    alert("Please fill the start date first.");
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    return false;
    //}

    //Check Valid Date
    if (!IsValidDate(StartDate)) {
        DisplayErrorMessage("10113709");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (!IsValidDate(EndDate)) {
        DisplayErrorMessage("10113710");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }


    //Check Less then DOB
    if (startyear != undefined && startyear != "" &&
        DOBYear != undefined && DOBYear != "") {
        if (parseInt(startyear) < parseInt(DOBYear)) {
            DisplayErrorMessage("10113707");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (startyear != undefined && startyear != "" &&
        DOBYear != undefined && DOBYear != "" &&
        startmon != undefined && startmon != "" &&
        DOBMonth != undefined && DOBMonth != "") {
        if (parseInt(startyear + months[startmon]) < parseInt(DOBYear + months[DOBMonth.toLowerCase()])) {
            DisplayErrorMessage("10113707");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (startyear != undefined && startyear != "" &&
        DOBYear != undefined && DOBYear != "" &&
        startmon != undefined && startmon != "" &&
        DOBMonth != undefined && DOBMonth != "" &&
        startday != undefined && startday != "" &&
        DOBDate != undefined && DOBDate != "") {
        if (parseInt(startyear + months[startmon] + startday) < parseInt(DOBYear + months[DOBMonth.toLowerCase()] + DOBDate)) {
            DisplayErrorMessage("10113707");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }



    if (endyear != undefined && endyear != "" &&
        DOBYear != undefined && DOBYear != "") {
        if (parseInt(endyear) < parseInt(DOBYear)) {
            DisplayErrorMessage("10113708");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (endyear != undefined && endyear != "" &&
        DOBYear != undefined && DOBYear != "" &&
        endmon != undefined && endmon != "" &&
        DOBMonth != undefined && DOBMonth != "") {
        if (parseInt(endyear + months[endmon]) < parseInt(DOBYear + months[DOBMonth.toLowerCase()])) {
            DisplayErrorMessage("10113708");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (endyear != undefined && endyear != "" &&
        DOBYear != undefined && DOBYear != "" &&
        endmon != undefined && endmon != "" &&
        DOBMonth != undefined && DOBMonth != "" &&
        endday != undefined && endday != "" &&
        DOBDate != undefined && DOBDate != "") {
        if (parseInt(endyear + months[endmon] + endday) < parseInt(DOBYear + months[DOBMonth.toLowerCase()] + DOBDate)) {
            DisplayErrorMessage("10113708");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }


    
    var CurrentDateAndTime = new Date();
    var CurrentDate = parseInt(CurrentDateAndTime.getFullYear().toString() + ((CurrentDateAndTime.getMonth().toString().length == 1) ? "0" + CurrentDateAndTime.getMonth().toString() : CurrentDateAndTime.getMonth().toString()) + ((CurrentDateAndTime.getDate().toString().length == 1) ? "0" + CurrentDateAndTime.getDate().toString() : CurrentDateAndTime.getDate().toString()));

    //let [startyear, startmon, startday] = StartDate.replaceAll("_", "").split("-");
    //let [endyear, endmon, endday] = EndDate.replaceAll("_", "").split("-");

    

    //Check Less then future date
    if (startyear != undefined && startyear != "") {
        if (startyear > CurrentDateAndTime.getFullYear()) {
            DisplayErrorMessage("10113712");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (endyear != undefined && endyear != "") {
        if (endyear > CurrentDateAndTime.getFullYear()) {
            DisplayErrorMessage("10113713");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (startday != undefined && startmon != undefined
        && startyear != undefined && startyear != "") {
        if (parseInt(startyear.toString() + (months[startmon]?.toString() ?? "00") + ((startday != undefined && startday != "") ? startday.toString() : "00")) > CurrentDate) {
            DisplayErrorMessage("10113712");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (endday != undefined && endmon != undefined &&
        endyear != undefined && endyear != "") {
        if (parseInt(endyear.toString() + (months[endmon]?.toString() ?? "00") + ((endday != undefined && endday != "") ? endday.toString() : "00")) > CurrentDate) {
            DisplayErrorMessage("10113713");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    //Check start date is less then end date
    if (startyear != undefined && endyear != undefined && startyear != "" && endyear != "") {
        if (startyear > endyear) {
            DisplayErrorMessage("10113711");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (startday != undefined && endday != undefined && startday != "" && endday != "" &&
        startmon != undefined && endmon != undefined && startmon != "" && endmon != "" &&
        startyear != undefined && startyear != "" && endyear != undefined && endyear != "") {

        if (new Date(startday + "-" + startmon + "-" + startyear) > new Date(endday + "-" + endmon + "-" + endyear)) {
            DisplayErrorMessage("10113711");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }


    if (startday != undefined && endday != undefined && startday != "" && endday != "" &&
        startmon != undefined && endmon != undefined && startmon != "" && endmon != "" &&
        startyear != undefined && startyear == "" && endyear != undefined && endyear == "") {
        var startdaystartmon = parseInt(startday + months[startmon]);
        var enddayendmon = parseInt(endday.toString() + months[endmon].toString());
        if (startdaystartmon > enddayendmon) {
            DisplayErrorMessage("10113711");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (startyear != undefined && endyear != undefined && startyear != "" && endyear != "" &&
        startmon != undefined && endmon != undefined && startmon != "" && endmon != "" &&
        startday != undefined && endday != undefined && startday == "" && endday == "") {
        var startmonstartyear = parseInt(months[startmon] + startyear);
        var endmonendyear = parseInt(months[endmon].toString() + endyear.toString());
        if (startmonstartyear > endmonendyear) {
            DisplayErrorMessage("10113711");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
}

function IsValidDate(vDate) {
    if (vDate == "") { return true; }
    vDate = vDate.replaceAll("_", "");
    let [year, mon, day] = vDate.split("-");

    var aryMonths = ["jan", "feb", "mar", "apr", "may", "jun",
        "jul", "aug", "sep", "oct", "nov", "dec"];

    if (day != "" && day.length == 1) {
        return false;
    }
    if (day != "" && day.length != 2) {
        return false;
    }
    if (day != "") {
        day = parseInt(day, 10);
    }
    if (day != "" && day > 31) {
        return false;
    }
    if (mon != "" && aryMonths.indexOf(mon) == -1) {
        return false;
    }
    if (year != "" && year.length != 4) {
        return false;
    }
    if (day != "" && mon != "") {
        var date = new Date(vDate);

        if (day != "" && day != undefined && date.getDate() != day.toString()) { return false; }
        if (mon != "" && mon != undefined && date.getMonth() != months[mon]) { return false; }
    }
    if (mon != "" && year != "") {
        var date = new Date(vDate);

        if (mon != "" && mon != undefined && date.getMonth() != months[mon]) { return false; }
        if (year != "" && year != undefined && date.getFullYear() != year) { return false; }
    }

    return true;
}
function AddClick() {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var validation = Validation();
    if (validation == false) {
        $('#btnAdd').attr("Autosave", "");
        return false;
    }

    var btnAdd = document.getElementById("btnAdd").value;
    var sHumanID = new URLSearchParams(document.URL.split("?")[1]).get('HumanID'); 
    var Street_Address1 = document.getElementById("txtAddressLineOne").value;
    var Street_Address2 = document.getElementById("txtAddressLineTwo").value;
    var City = document.getElementById("txtCity").value;
    var State = document.getElementById("cboState").value;
    var ZipCode = document.getElementById("txtZipCode").value;
    var Start_Date = document.getElementById("dtStartDate").value;
    var End_Date = document.getElementById("dtEndDate").value;
    var Human_Address_ID = (document?.getElementById("btnAdd")?.getAttribute("Human_Address_ID")) ?? "";
    //Transcation
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/WebServices/AddressHistoryService.asmx/SaveHumanHistoryAddress",
        data: JSON.stringify({
            "sHumanID": sHumanID, "sStreet_Address1": Street_Address1,
            "sStreet_Address2": Street_Address2, "sCity": City,
            "sState": State, "sZipCode": ZipCode, "sStart_Date": Start_Date,
            "sEnd_Date": End_Date, "sHuman_Address_ID": Human_Address_ID, "sInsertOrUpdate": btnAdd
        }),
        dataType: "json",
        async: true,
        success: function (data) {
            
            var data = $.parseJSON(data.d);
           
            DisplayErrorMessage("10113715");
            document.getElementById("btnAdd").disabled = true;
            if (document.getElementById("btnAdd").getAttribute("Autosave") == "true") {
                $('#btnAdd').attr("Autosave", "");
                $(top.window.document).find("#btnAdressHistoryClose").attr("aria-hidden", "true");
                $(top.window.document).find("#btnAdressHistoryClose").attr("data-dismiss", "modal");
                $(top.window.document).find("#TabAdressHistory").hide();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return true;
            }
            ClearALL();
            LoadGrid();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}

function EditeClick(row) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById("btnAdd").disabled = false;
    document.getElementById("btnAdd").value = 'Update';
    document.getElementById("btnClearAll").value = 'Cancel';
    $('#btnAdd').attr("Autosave", "");
    document.getElementById("txtAddressLineOne").value = $(row).parent().parent().children('td[name="Street_Address1"]').text() ?? "";
    document.getElementById("txtAddressLineTwo").value = $(row).parent().parent().children('td[name="Street_Address2"]').text() ?? "";
    document.getElementById("txtCity").value = $(row).parent().parent().children('td[name="City"]').text() ?? "";
    document.getElementById("cboState").value = $(row).parent().parent().children('td[name="State"]').text() ?? "";
    document.getElementById("txtZipCode").value = $(row).parent().parent().children('td[name="ZipCode"]').attr('value') ?? "";
    document.getElementById("dtStartDate").value = $(row).parent().parent().children('td[name="Start_Date"]').attr('value') ?? "";
    document.getElementById("dtEndDate").value = $(row).parent().parent().children('td[name="End_Date"]').attr('value') ?? "";
    document.getElementById("btnAdd").setAttribute("Human_Address_ID", $(row).parent().parent().children('td[name="Human_Address_ID"]').text() ?? "");
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function DeleteClick(delrow)
{
    var vHuman_Address_ID = $(delrow).parent().parent().children('td[name="Human_Address_ID"]').text();
    if (!confirm("Are you sure you want to delete?")) { return false; }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/WebServices/AddressHistoryService.asmx/DeleteHumanHistoryAddress",
        data: JSON.stringify({ "sHuman_Address_ID": vHuman_Address_ID ?? "" }),
        dataType: "json",
        async: true,
        success: function (data) {

            var data = $.parseJSON(data.d);
            ClearALL();
            LoadGrid();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });

}
function ClearALLClick() {
    if (!DisplayErrorMessage("10113714")) { return false; }
    ClearALL();
}
function ClearALL() {
    document.getElementById("btnAdd").value = 'Add';
    document.getElementById("btnClearAll").value = 'Clear All';
    document.getElementById("txtAddressLineOne").value = '';
    document.getElementById("txtAddressLineTwo").value = '';
    document.getElementById("txtCity").value = '';
    document.getElementById("cboState").value = '';
    document.getElementById("txtZipCode").value = '';
    document.getElementById("dtStartDate").value = '';
    document.getElementById("dtEndDate").value = '';
    document.getElementById("btnAdd").setAttribute("Human_Address_ID", "");
    document.getElementById("btnAdd").setAttribute("Autosave", "");
}

function Autosave() {
    document.getElementById("btnAdd").disabled = false;
}

function FillCurrentAddress() {
    document.getElementById("btnAdd").disabled = false;
    if (top?.window?.document?.getElementsByName("ctl00_ModalWindow")[0]?.contentWindow.document != undefined
        && top?.window?.document?.getElementsByName("ctl00_ModalWindow")[0]?.contentWindow.document != null) {
        document.getElementById("btnAdd").value = 'Add';
        document.getElementById("btnClearAll").value = 'Clear All';
        document.getElementById("btnAdd").setAttribute("Human_Address_ID", "");
        document.getElementById("btnAdd").setAttribute("Autosave", "");
        document.getElementById("txtAddressLineOne").value = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtPatientAddress").value;
        document.getElementById("txtAddressLineTwo").value = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtPatientAddressLine2").value;
        document.getElementById("txtCity").value = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtCity").value;
        document.getElementById("cboState").value = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_ddlState").value;
        document.getElementById("txtZipCode").value = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_msktxtZipcode").value;
    }
}

function closepopup() {
    $(top.window.document).find("#btnAdressHistoryClose").removeAttr("data-dismiss");
    $(top.window.document).find("#btnAdressHistoryClose").removeAttr("aria-hidden");
    if (!$('#btnAdd').attr("disabled")) {

        dvdialog = $("#dvdialogAddressHistory");
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
                    $('#btnAdd').attr("Autosave", "true");
                    $('#btnAdd').click();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    $(dvdialog).dialog("close");
                    return false;
                },
                "No": function () {
                    
                    $(top.window.document).find("#btnAdressHistoryClose").attr("aria-hidden", "true");
                    $(top.window.document).find("#btnAdressHistoryClose").attr("data-dismiss", "modal");
                    $(top.window.document).find("#TabAdressHistory").hide();
                    $(dvdialog).dialog("close");
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    return false;
                }
            }
        });
    }
    else {
        $(top.window.document).find("#btnAdressHistoryClose").attr("aria-hidden", "true");
        $(top.window.document).find("#btnAdressHistoryClose").attr("data-dismiss", "modal");
        $(top.window.document).find("#TabAdressHistory").hide();
        return false;
    }
    
}

function loadPatientStrip() {
    if (top?.window?.document?.getElementsByName("ctl00_ModalWindow")[0]?.contentWindow.document != undefined
        && top?.window?.document?.getElementsByName("ctl00_ModalWindow")[0]?.contentWindow.document != null) {

        var patFirstName = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtPatientfirstname").value;
        var PatLastName = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtPatientlastname").value;
        var PatDOB = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_dtpPatientDOB").value;
        var PatSex = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_HiddenPatientSex").value;
        var PatHumanID = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtAccountNo").value;
        var PatMedRecNumber = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_txtMedicalRecordno").value;
        var PatHomePhoneNumber = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_msktxtHomePhno").value;
        var PatCellPhoneNumber = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_msktxtCellPhno").value;
        var PatType = top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_cboHumanType").value;
        var PatAge = 0;
        var aryDOB = [date, month, year] = PatDOB.replaceAll("_","").split("-");
        var aryCurrentDate = [cDate, cMonth, cYear] = [new Date().getDate().toString(), new Date().getMonth().toString(), new Date().getFullYear().toString()];
        if (top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_msktxtHomePhno").value.replaceAll("_", '').replaceAll("()", "").trim().length == 1) {
            PatHomePhoneNumber = "";
        }
        if (top.window.document.getElementsByName("ctl00_ModalWindow")[0].contentWindow.document.getElementById("ctl00_C5POBody_msktxtCellPhno").value.replaceAll("_", '').replaceAll("()", "").trim().length == 1) {
            PatCellPhoneNumber = "";
        }

        if (PatDOB.replaceAll("_","") != "") { PatAge = cYear - year; }
        if (cMonth < months[month] || (cMonth == months[month] && cDate < date)) { --PatAge; }

        if (PatSex != undefined && PatSex != "") {
            if (PatSex.toUpperCase() == "UNKNOWN") { PatSex = "UNK"; }
            else if (PatSex.toUpperCase() == "UNDIFFERENTIATED") { PatSex = "UN"; }
            else { PatSex = PatSex.slice(0, 1); }
        }
        else { PatSex = " "; }

        var patientStrip = `${PatLastName},${patFirstName} | ${PatDOB} | ${PatAge} year(s) | ${PatSex} | Acct #:${PatHumanID} | Med Rec #:${PatMedRecNumber} | Home Phone #:${PatHomePhoneNumber} | Cell Phone #:${PatCellPhoneNumber} | Patient Type:${PatType} |`;

        document.getElementById("lblPatientStrip").innerText = patientStrip;
    }
}