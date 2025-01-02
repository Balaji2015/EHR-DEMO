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

function LoadPatientList() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $('#PatientListTable').empty();
    $("#PatientListTable").append(`
    <table id="PatientTable" class="table table-bordered Gridbodystyle" style="table-layout: fixed;width:100%;">
    <thead class="header" style="border: 0px;width:96.7%;">
        <tr class="header">
            <th style="border: 1px solid #909090;text-align: center;">Patient Name</th>
            <th style="border: 1px solid #909090;text-align: center;">DOB</th>
            <th style="border: 1px solid #909090;text-align: center;">Pt. Acc. #</th>
            <th style="border: 1px solid #909090;text-align: center;">Member ID</th>
            <th style="border: 1px solid #909090;text-align: center;">DOS</th>
            <th style="border: 1px solid #909090;text-align: center;">Enc. Provider</th>
            <th style="border: 1px solid #909090;text-align: center;">Pri. Carrier</th>
            <th style="border: 1px solid #909090;text-align: center;">Pri. Plan</th>
            <th style="border: 1px solid #909090;text-align: center;">Type of Visit</th>
            <th style="border: 1px solid #909090;text-align: center;">Facility</th>
            <th style="border: 1px solid #909090;text-align: center;">Encounter ID</th>
            <th style="border: 1px solid #909090;text-align: center;">View</th>
        </tr>
    </thead>
</table>`);

    var dataTable = new DataTable('#PatientTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 30,
        language: {
            search: "",
            searchPlaceholder: "Search by Patient Name or Acct # or Member ID",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmPatientList.aspx/LoadPatientList',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "dtFromDOS": $('#ctl00_C5POBody_dtFromDOS').val(),
                    "dtToDOS": $('#ctl00_C5POBody_dtToDOS').val(),
                    "dtpPatientDOB": $('#ctl00_C5POBody_dtpPatientDOB').val(),
                    "ddlPayerName": $('#ctl00_C5POBody_ddlPayerName').val(),
                    "sPlanId": $('#ctl00_C5POBody_ddlPlan').val(),
                    "txtPatientAccNo": $('#ctl00_C5POBody_txtPatientAccNo').val(),
                    "txtMemberId": $('#ctl00_C5POBody_txtMemberId').val(),
                    "txtPatientLastName": $('#ctl00_C5POBody_txtPatientLastName').val(),
                    "txtPatientFirstName": $('#ctl00_C5POBody_txtPatientFirstName').val(),
                });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                if (objdata.errorCode) {
                    DisplayErrorMessage(objdata.errorCode);
                } else {
                    objdata.data = Decompress(objdata.data);
                }

                if (objdata?.data?.length > 0) {
                    $('#ctl00_C5POBody_lblNoofResults').text(objdata?.data?.length + ' record(s) found.');
                } else {
                    $('#ctl00_C5POBody_lblNoofResults').text('No record(s) found.');
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                json.data = objdata.data;
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        "Message: " + log.Message);
                }
            }
        },
        columns: [
            { data: 'Patient Name', sWidth: '9%' },
            {
                data: 'DOB', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                },
                searchable: false,
                type: 'date',
                sWidth: '6%'
            },
            { data: 'Patient_Account_Number', sWidth: '5%' },
            { data: 'Member ID', sWidth: '7%' },
            {
                data: 'DOS', render: function (data, type, row) {
                    return ConvertDate(data.replace("T", " "));
                },
                searchable: false,
                type: 'date',
                sWidth: '7%'
            },
            { data: 'Provider_Name', searchable: false, sWidth: '9%' },
            { data: 'Payer', searchable: false, sWidth: '9%' },
            { data: 'Plan_Name', searchable: false, sWidth: '9%' },
            { data: 'Type of Visit', searchable: false, sWidth: '7%' },
            { data: 'Facility', searchable: false, sWidth: '7%' },
            { data: 'Encounter ID', searchable: false, sClass: 'hide_column', sWidth: '7%' },
            {
                data: 'Carrier ID', render: function (data, type, row) {
                    var encounterId = row["Encounter ID"];
                    var humanId = row["Patient_Account_Number"];
                    return `<span style="cursor: pointer;" title="View" onclick="viewSummary(${encounterId},${humanId});"> <img src="Resources/Down.bmp" alt="View"> </span>`;
                },
                searchable: false,
                orderable: false,
                sClass: 'text-align-center',
                sWidth: '2%'
            },
        ],
        initComplete: function (settings, json) {
            $("#PatientTable_filter input")[0].classList.add('searchicon');
        }
    });

    $('#PatientTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
        'font-size': '13px',
    });

    $('#PatientTable_info').css({
        'min-width': '180px'
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });

    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
    });

    $('#PatientTable tbody').on('click', 'tr', function () {
        $('#PatientTable tr').removeClass("odd");
        $('#PatientTable tr').removeClass("even");
        $('#PatientTable tbody tr').removeClass('highlight');
        $(this)[0].classList.add('highlight');
    });
}

function viewSummary(encounterId, humanId) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    $.ajax({
        type: "POST",
        url: "frmPatientList.aspx/ViewSummary",
        data: JSON.stringify({
            "humanId": humanId,
            "encounterId": encounterId,
        }),
        contentType: "application/json;charset=utd-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var sPath = `frmSummaryNew.aspx?IsPatientList=Y&EncounterID=${encounterId}&HumanID=${humanId}&TabMode=true`;
            $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            //$(top.window.document).find("#mdldlg")[0].style.width = "1050px";
            $(top.window.document).find("#ProcessModal")[0].style.width = "";
            $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
            $(top.window.document).find("#ModalTitle")[0].textContent = "Summary";
        },
        error: function OnError(xhr) {
            AutoSaveUnsuccessful();
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

function Decompress(data) {
    // Decode the Base64 string
    const binaryString = window.atob(data);
    // Convert binary string to byte array
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    // Use pako to decompress the byte array
    const decompressed = pako.inflate(bytes, { to: 'string' });
    return JSON.parse(decompressed);
}

function DOBConvert(DOB) {
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[0] + "-" + (SplitDOB[1]).toString().padStart(2, '0') + "-" + SplitDOB[2];
}

function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate + ' UTC');
    var then = '';
    var month = (now.getMonth() + 1).toString().padStart(2, '0'); 
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-01-0001';
    else
        then = (now.getFullYear() + '-' + month + '-' + now.getDate().format("dd").slice(-2).toString().padStart(2, '0'));
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