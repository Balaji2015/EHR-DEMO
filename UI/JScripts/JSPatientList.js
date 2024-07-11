$(document).ready(function () {
    $("#falogout").css("display", "block");
    $(document.getElementById(GetClientId("dtpPatientDOB"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, maxDate: new Date(), yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("dtpPatientDOB")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("dtpPatientDOB"))).click(function () {
        $(document.getElementById(GetClientId("dtpPatientDOB"))).focus();
    });
    $(document.getElementById(GetClientId("dtFromDOS"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, maxDate: new Date(), yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("dtFromDOS")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("dtFromDOS"))).click(function () {
        $(document.getElementById(GetClientId("dtFromDOS"))).focus();
    });
    $(document.getElementById(GetClientId("dtToDOS"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, maxDate: new Date(), yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("dtToDOS")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("dtToDOS"))).click(function () {
        $(document.getElementById(GetClientId("dtToDOS"))).focus();
    });
});

function AutoSave() {
    var dt = new Date();
    var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById(GetClientId("hdnLocalTime")).value = utc;

}

function DateValidattion(dateToValidate) {

    var datePicker = $find(dateToValidate);
    var splitdate = datePicker.get_dateInput().get_selectedDate().format("dd-MMM-yyyy");
    var dt1 = new Date();
    var dd = new Date();
    var month = new Array();
    switch (splitdate.split('-')[1]) {
        case "Jan":
            x = 0;
            break;
        case "Feb":
            x = 1;
            break;
        case "Mar":
            x = 2;
            break;
        case "Apr":
            x = 3;
            break;
        case "May":
            x = 4;
            break;
        case "Jun":
            x = 5;
            break;
        case "Jul":
            x = 6;
            break;
        case "Aug":
            x = 7;
            break;
        case "Sep":
            x = 8;
            break;
        case "Oct":
            x = 9;
            break;
        case "Nov":
            x = 10;
            break;
        case "Dec":
            x = 11;
            break;
        case splitdate.split('-')[1]:
            return false;
            break;

    }
    dd.setFullYear(splitdate.split('-')[2], x, splitdate.split('-')[0]);
    if (isNaN(dd)) {
        return false;
    }
    if (splitdate.split('-')[0] > 31) {
        return false;
    }
}

function isNumberKey(evt) {
    //var charCode = (evt.which) ? evt.which : event.keyCode
    //if ((charCode > 47 && charCode < 58) || (charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
    //    return true;
    //}


    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function OpenModal(data) {
    var itemValue = data;
    if (itemValue == "Logout") {
        StartLoadingImage();
        if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
            if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {

                DisplayErrorMessage('1100000', 'LogOut');
            }
        }
        document.getElementById(GetClientId("btnlogout")).click();
    }
}

function ValidateDate(sender, args) {
    var datePicker = $find("dtpPatientDOB_dateInput");
    var todayDate = new Date();
    todayDate.setHours(0, 0, 0, 0);

    if (args.get_newDate() > todayDate) {
        datePicker.set_selectedDate(args.get_oldDate());
        alert("Can not enter future dates.");
    }
}

function ViewSummary(Human_ID, enc_id) {
    // var EncounterID = e.parentNode.parentNode.cells[9].innerText; //document.getElementById(GetClientId("hdnEncID")).value;
    var obj = new Array();
    var result = openModal("frmSummaryNew.aspx?" + "&HumanID=" + Human_ID + "&EncounterID=" + enc_id + "&IsPatientList=Y" +"&TabMode=true", 600, 1020, obj, "PatListModalWindow");
    var result = $find('PatListModalWindow');
}


//function viewImage(e) {
//    var EncounterID = e.parentNode.parentNode.cells[9].innerText; //document.getElementById(GetClientId("hdnEncID")).value;
//    var HumanID = e.parentNode.parentNode.cells[2].innerText;
//        var obj = new Array();
//        obj.push("EncounterID=" + EncounterID);
//        obj.push("HumanID=" + HumanID);
//        obj.push("IsPatientList=Y");
//        var result = openModal("frmSummaryNew.aspx", 600, 1020, obj, "PatListModalWindow");
//        var result = $find('PatListModalWindow');
//}

function QPCDateValidation(sender, args) {
    var EnteredDateLength = parseInt(args._newValue.replace("-", "").replace("-", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").length);
    if (EnteredDateLength != 9 && EnteredDateLength > 0) {
        alert("Please Enter the Date Fully.")
        sender.clear();
        document.getElementById(sender._clientID).focus();
        return false;
    }
    if (EnteredDateLength == 9) {
        if (isNaN(Date.parse($find(GetClientId(sender.get_id().split('_')[2]))._value))) {
            alert('Enter Valid Date!');
            $find(GetClientId(sender.get_id().split('_')[2])).clear();
            $find(GetClientId(sender.get_id().split('_')[2])).focus(true);
            return false;
        }
        if ($find(GetClientId(sender.get_id().split('_')[2]))._value.split('-')[0] == "00") {
            alert('Enter Valid Date!');
            $find(GetClientId(sender.get_id().split('_')[2])).clear();
            $find(GetClientId(sender.get_id().split('_')[2])).focus(true);
            return false;
        }
        validatedate($find(GetClientId(sender.get_id().split('_')[2]))._value, sender.get_id().split('_')[2]);
    }
}


function validatedate(inputText, ControlId) {
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;
    if (inputText.match(FormatDDMMMYYYY)) {
        var DateMonthYear = inputText.split('-');
        lopera2 = DateMonthYear.length;
        var DateInput = parseInt(DateMonthYear[0]);
        var Year = parseInt(DateMonthYear[2]);
        var Month = "";
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
        if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {
            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;
            if (Month == 1 || Month > 2) {
                if (DateInput > ListofDays[Month - 1]) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    return false;
                }
            }

            if (Month == 2) {
                var lyear = false;
                if ((!(Year % 4) && Year % 100) || !(Year % 400)) {
                    lyear = true;
                }
                if ((lyear == false) && (DateInput >= 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    return false;
                    return false;
                }
                if ((lyear == true) && (DateInput > 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();

            if (document.getElementById(GetClientId("dtpPatientDOB")).value != "__-___-____") {
                if (ControlId == 'dtpPatientDOB') {
                    Dobdate = parseMyDate(document.getElementById(GetClientId("dtpPatientDOB")).value);
                }


                if (Dobdate != undefined && Dobdate > CurrentDate) {
                    alert("The Date of Birth you have entered is in the future. Please enter a valid day, month, and year.");
                    if (ControlId == 'dtpPatientDOB') {
                        $find(GetClientId('dtpPatientDOB')).clear();
                        $find(GetClientId('dtpPatientDOB')).focus(true);
                        return false;
                    }

                }
                if (Dobdate != undefined && Dobdate.getFullYear() < (CurrentYear - 120)) {
                    alert("The Date of Birth you have entered is way in the past. Please enter a valid day, month, and year.");
                    if (ControlId == 'dtpPatientDOB') {
                        $find(GetClientId('dtpPatientDOB')).clear();
                        $find(GetClientId('dtpPatientDOB')).focus(true);
                        return false;
                    }

                }
            }

        }
        else {
            alert('Invalid date format!');
            $find(GetClientId(ControlId)).clear();
            $find(GetClientId(ControlId)).focus(true);
            return false;
        }
    }
}

function parseMyDate(s) {
    var m = ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec'];
    var match = s.match(/(\d+)-([^.]+)-(\d+)/);
    var date = match[1];
    var monthText = match[2];
    var year = match[3];
    var month = m.indexOf(monthText.toLowerCase());
    return new Date(year, month, date);
}

function EnableSaveButton(ctrl) {

}
//CAP-2241
function PayerNameChange() {
    var cbolab = document.getElementById("ctl00_C5POBody_ddlPayerName");
    __doPostBack('ctl00_C5POBody_ddlPayerName', cbolab);
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function LoadingImage() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //var fromDate = document.getElementById('dtpFromDOS_dateInput');
    //var fromDateValue = fromDate.value;

    //var toDate = document.getElementById('dtpToDOS_dateInput');
    //var toDateValue = toDate.value;


    //var DOBDate = document.getElementById('dtpPatientDOB_dateInput');
    //var DOBDateValue = DOBDate.value;


    //fromDateValue = new Date(fromDateValue);
    //toDateValue = new Date(toDateValue);
    //DOBDateValue = new Date(DOBDateValue);

    //if (fromDateValue == "Invalid Date")
    //{
    //    DisplayErrorMessage('103010');
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    sender.set_autoPostBack(false);
    //    return false;
    //}

    //if (toDateValue == "Invalid Date")
    //{
    //    DisplayErrorMessage('103011');
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    sender.set_autoPostBack(false);
    //    return false;
    //}


    //if (fromDateValue != "Invalid Date"&&toDateValue != "Invalid Date"&& toDateValue < fromDateValue) {
    //    DisplayErrorMessage('103005');
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    sender.set_autoPostBack(false);
    //    return false;
    //}

  //  GetUTCTime();

    //var today = new Date();
    //sender.set_autoPostBack(true);
}

function CloseResultPage(oWindow, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && DisplayErrorMessage('1100000') == true)
            args.set_cancel(true);
        else
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }

}

function RefreshCloseResultPage() {

}

function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}