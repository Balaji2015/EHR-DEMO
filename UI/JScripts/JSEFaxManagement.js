var intProviderlen = -1;
var arrProvider = [];
var intPatientlen = -1;
var arrPatient = [];
var getDate = new Date();
var beforeOneMonthFromCurrentDate = new Date();
beforeOneMonthFromCurrentDate.setMonth(beforeOneMonthFromCurrentDate.getMonth() - 1);
var beforedate = beforeOneMonthFromCurrentDate.getFullYear() + "-" + beforeOneMonthFromCurrentDate.toLocaleString('default', { month: 'short' })
    + "-" + beforeOneMonthFromCurrentDate.getDate().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });

var todayDate = getDate.getFullYear() + "-" + getDate.toLocaleString('default', { month: 'short' })
    + "-" + getDate.getDate().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });

$(document).ready(function () {
    SetAutoSearchRecipientName();
    SetAutoSearchFaxSenderName();
    SetAutoSearchPatientDetails();
    
    $("#dtFaxSentDateFrom").val(beforedate);
    $("#dtFaxSentDateTo").val(todayDate);
    $("#dtFaxSentDateFrom").datetimepicker({closeOnDateSelect: true, timepicker: false, format: 'Y-M-d', maxDate: 0 });
    $("#dtFaxSentDateTo").datetimepicker({ closeOnDateSelect: true, timepicker: false, format: 'Y-M-d', maxDate: 0 });
    var currentdate = new Date();
    var datetime = currentdate.getFullYear() + "-" + String((currentdate.getMonth() + 1)).padStart(2, '0') + "-" + String(currentdate.getDate()).padStart(2, '0');
    document.getElementById("dtFaxSentDateFrom").setAttribute("max", datetime);
    document.getElementById("dtFaxSentDateTo").setAttribute("max", datetime);
    
});
function SetTabelHeader() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.setTimeout(function () {
        $("#divEFAXManagement")[0].innerHTML = "";
        $("#divEFAXManagement").append("<table id='EFaxManagementTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Encounter ID</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Acc#</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'> Sender Name</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'> Sender Facility</th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Sender Fax #</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>Recipient Fax #</th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'> Status</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Reason</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'> Sent Date</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'> Sent By</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'/></table>");
        var datatable = new DataTable('#EFaxManagementTable', {
            serverSide: false,
            lengthChange: false,
            scrollCollapse: true,
            scrollY: '243px',
            searching: true,
            processing: false,
            ordering: true,
            autoWidth: false,
            order: [],
            pageLength: 30,
            language: {
                search: "",
                searchPlaceholder: "Search by Encounter ID or Sent By or Reason",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">',
            initComplete: function (settings, data, json) {
                $("#EFaxManagementTable_filter input")[0].classList.add('searchicon');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }, 1000);
}
function btnSearchClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //if (document.getElementById("txtPatientName").value == "" && document.getElementById("txtPatientName").disabled == false) {
    //    alert("Please select patient.");
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    return true;
    //}
    //else if (document.getElementById("txtRecipientName").value == "" && document.getElementById("txtRecipientName").disabled == false) {
    //    alert("Please select RecipientName.");
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    return true;
    //}
    //else if (document.getElementById("txtFaxSenderName").value == "" && document.getElementById("txtFaxSenderName").disabled == false) {
    //    alert("Please select FAX SenderName.");
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    return true;
    //}
    //else 
    if (document.getElementById("dtFaxSentDateFrom").value == "") {
        alert("Please select FAX Sent Date (From).");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return true;
    }
    else if (document.getElementById("dtFaxSentDateTo").value == "") {
        alert("Please select FAX Sent Date (To).");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return true;
    }
    var fromdate = document.getElementById("dtFaxSentDateFrom").value;
    var todate = document.getElementById("dtFaxSentDateTo").value;

    if (!ValidateDate(fromdate, todate)) { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } return true; }
    EFaxManagementload();

}
function ValidateDate(fromDate, toDate) {
    //From Date
    var objFromDate = new Date(fromDate);
    var finalFromdate = "";
    if (objFromDate != "Invalid Date") {
        finalFromdate = objFromDate.getFullYear() + "-" + objFromDate.toLocaleString('default', { month: 'short' }) + "-" + ((objFromDate.getDate().toString().length == 1) ? "0" + objFromDate.getDate() : objFromDate.getDate());
        if (fromDate.toUpperCase() != finalFromdate.toUpperCase())  {
            alert("Please select a valide date");
            return false;
        }
    }
    else {
        alert("Please select a valide date");
        return false;
    }

    //To Date
    var objToDate = new Date(toDate);
    var finalTodate = "";
    if (objToDate != "Invalid Date") {
        finalTodate = objToDate.getFullYear() + "-" + objToDate.toLocaleString('default', { month: 'short' }) + "-" + ((objToDate.getDate().toString().length == 1) ? "0" + objToDate.getDate() : objToDate.getDate());
        if (toDate.toUpperCase() != finalTodate.toUpperCase()) {
            alert("Please select a valide date");
            return false;
        }
    }
    else {
        alert("Please select a valide date");
        return false;
    }
    //Restict future date
    if (objFromDate > new Date()) {
        alert("Please select only past and present date.");
        return false;
    }
    if (objToDate > new Date()) {
        alert("Please select only past and present date.");
        return false;
    }
    //Always To date grater then or Equal the From date

    if (objFromDate > objToDate)  {
        alert("From date can not be greater than To date.");
        return false;
    }

    //Select between one month validation
    var numberofdays = Math.round((new Date(toDate) - new Date(fromDate)) / (1000 * 3600 * 24));
    if (numberofdays > 31)  {
        alert("Please select From Date and To Date between one month.");
        return false;
    }

    return true;

}
function EFaxManagementload() {
    $("#divEFAXManagement")[0].innerHTML = "";
    var FaxStatus = document.getElementById("cboFaxStatus").value;
    var HumanId = document.getElementById("txtPatientName").getAttribute("data-human-id");
    var RecipiantName = document.getElementById("txtRecipientName").value;
    var SenderName = document.getElementById("txtFaxSenderName").value;
    var FromDate = document.getElementById("dtFaxSentDateFrom").value;
    var ToDate = document.getElementById("dtFaxSentDateTo").value;

    $("#divEFAXManagement").append("<table id='EFaxManagementTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Encounter ID</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Acc#</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'> Sender Name</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'> Sender Facility</th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Sender Fax #</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>Recipient Fax #</th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'> Status</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Reason</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'> Sent Date</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'> Sent By</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'/></table>");
    var datatable = new DataTable('#EFaxManagementTable', {
        serverSide: false,
        lengthChange: false,
        scrollCollapse: true,
        scrollY: '243px',
        searching: true,
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 30,
        language: {
            search: "",
            searchPlaceholder: "Search by Encounter ID or Sent By or Reason",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmEFax.aspx/EFaxManagementload',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "FaxStatus": FaxStatus,
                    "HumanId": HumanId,
                    "RecipiantName": RecipiantName,
                    "SenderName": SenderName,
                    "FromDate": FromDate,
                    "ToDate": ToDate
                });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);

                json.draw = objdata.draw;
                json.recordsTotal = objdata.recordsTotal;
                json.recordsFiltered = objdata.recordsFiltered;
                json.data = objdata.data;
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
            {
                data: 'Encounter_ID', sWidth: '15%' },
            {
                data: 'Human_ID', searchable: false, sWidth: '10%' },
            {
                data: 'Patient_Name', searchable: false, sWidth: '11%' },
            {
                data: 'Fax_Sender_Name', sWidth: '18%'
            },
            {
                data: 'Fax_Sender_Company', render: function (data, type, row) {
                    return data.split("|")[0];
                },
                searchable: false, sWidth: '13%'
            },
            {
                data: 'Fax_Sender_Number', searchable: false, sWidth: '15%'
            },
            {
                data: 'Fax_Recipient_Name',  render: function (data, type, row) {
                    return data.split("|")[0];
                }, searchable: false, sWidth: '17%'
            },
            {
                data: 'Fax_Recipient_Number', searchable: false, sWidth: '12%'
            },
            { data: 'Subject', sWidth: '15%', searchable: false },
            { data: 'Fax_Status', sWidth: '10%', searchable: false },
            { data: 'Error_Description', sWidth: '11%' },
            {
                data: "Activity_Date_And_Time", render: function (data, type, row) {
                    var date = ConvertDate(data.replace("T", " "));
                    return date;
                },
                type: 'date', searchable: false, sWidth: '18%'
            },
            { data: 'Activity_By', sWidth: '15%' },
            {
                data: "", render: function (data, type, row) {
                    if (row.Fax_File_Path == '' && row.Fax_Sent_File_Path == '') {
                        return "<i onclick='CloseOutboxImage(" + row.Activity_Log_ID + ");' title='View' class='glyphicon glyphicon-eye-open'></i>"
                    }
                    else {
                        return "<i onclick='OpenViewerforEFoxoutBox(" + row.Activity_Log_ID + ");' title='View' class='glyphicon glyphicon-eye-open'></i>"
                    }

                }, searchable: false, sWidth: '6%', sClass: "text-align-center"
            },
            {
                data: "", render: function (data, type, row) {
                    if (row.Fax_Status.toUpperCase() == "FAILED") {
                        return "<i onclick='funRetryForEfaxManagement(" + row.Activity_Log_ID + ");' class='glyphicon glyphicon-refresh'/>"
                    }
                    else { return "<i></i>"; }
                }, searchable: false, sWidth: '6%', sClass: "text-align-center"
            }
        ], createdRow: function (row, data, dataIndex) {
            $(row)[0].id = data.Id;
        },
        initComplete: function (settings, data, json) {
            $("#EFaxManagementTable_filter input")[0].classList.add('searchicon');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });

    $('#EFaxManagementTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EFaxManagementTable_info').css({
        'min-width': '180px'
    });
}
function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate + ' UTC');
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-01-0001';
    else
        then = ('0' + now.getDate().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false })).slice(-2) + '-' + monthNames[now.getMonth()] + '-' + now.getFullYear();
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
function funRetryForEfaxManagement(e) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        url: "frmEFax.aspx/EFaxoutboxRetry",
        contentType: "application/json;charset=utf-8",
        data: '{ActivityId: "' + e + '"}',
        datatype: "json",
        success: function success(data) {
            
            EFaxManagementload();
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
function cleartextbox(ctrl) {
    if (ctrl.id == "imgClearRecipientNameText") {
        document.getElementById("txtRecipientName").disabled = false;
        document.getElementById("txtRecipientName").value = "";
        document.getElementById("txtRecipientName").title = "";
        var txtProviderSearch = document.getElementById("txtRecipientName");
        txtProviderSearch.attributes['data-phy-id'].value = "0";
        txtProviderSearch.attributes['data-phy-details'].value = "";
        txtProviderSearch.attributes['data-category'].value = "";
    }
    else if (ctrl.id == "imgClearFaxSenderNameText") {
        document.getElementById("txtFaxSenderName").disabled = false;
        document.getElementById("txtFaxSenderName").value = "";
        document.getElementById("txtFaxSenderName").title = "";

    }
    else if (ctrl.id == "imgClearPatientNameText") {
        document.getElementById("txtPatientName").disabled = false;
        document.getElementById("txtPatientName").value = "";
    }
}

function clearAll() {
    document.getElementById("txtRecipientName").disabled = false;
    document.getElementById("txtRecipientName").value = "";
    document.getElementById("txtRecipientName").title = "";
    var txtProviderSearch = document.getElementById("txtRecipientName");
    txtProviderSearch.attributes['data-phy-id'].value ="0";
    txtProviderSearch.attributes['data-phy-details'].value = "";
    txtProviderSearch.attributes['data-category'].value = "";


    document.getElementById("txtFaxSenderName").disabled = false;
    document.getElementById("txtFaxSenderName").value = "";
    document.getElementById("txtFaxSenderName").title = "";

    document.getElementById("txtPatientName").disabled = false;
    document.getElementById("txtPatientName").value = "";
    document.getElementById("imgClearPatientNameText").click();

    $("#dtFaxSentDateFrom").val(beforedate);
    $("#dtFaxSentDateTo").val(todayDate);

    document.getElementById("cboFaxStatus").selectedIndex = 0;
    $("#divEFAXManagement")[0].innerHTML = "";
    SetTabelHeader();
}
function closeclick() {
    $(top.window.document).find('#btnFaxManagementClose')[0].click();
}

function ExportToExcel() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#divEFAXManagement table").length > 0) {
        var table = new DataTable('#EFaxManagementTable');
        var rawtableHeading = table.$("tr").parent().parent().find("thead").children().children().children();
        var tableRow = table.$("tr");

        //Create header
        var tabelHeading = document.createElement("thead").appendChild(document.createElement("tr"));
        rawtableHeading.each(function (iteam) {
            if ($(this)[0].innerHTML.indexOf("View") == -1 && $(this)[0].innerHTML.indexOf("Retry") == -1) {
                var d = document.createElement("b");
                d.innerText = $(this)[0].innerHTML;
                var th = document.createElement("td");
                th.appendChild(d);
                tabelHeading.appendChild(th);
            }
        });


        var exceltable = document.createElement("table");
        exceltable.appendChild(tabelHeading);

        //Create Body
        var tabelbody = document.createElement("tbody");
        tableRow.each(function (iteam) {
            tabelbody.appendChild($(this)[0]);
        });

        
        exceltable.appendChild(tabelbody);

        //Create and Set Search Parameters
        var searchparametertabel = document.createElement("table");
        var searchparameterdatebody = document.createElement("tbody");


        var reportheadingtr = document.createElement("tr");
        var reportheadingtd1 = document.createElement("td");
        reportheadingtr.appendChild(reportheadingtd1);
        var reportheadingtd2 = document.createElement("td");
        reportheadingtr.appendChild(reportheadingtd2);
        var reportheadingtd3 = document.createElement("td");
        reportheadingtr.appendChild(reportheadingtd3);
        var reportheadingtd4 = document.createElement("td");
        reportheadingtd4.innerHTML = "<b>E-FAX</b>";
        reportheadingtr.appendChild(reportheadingtd4);
        searchparameterdatebody.appendChild(reportheadingtr);

        var emptyrow = document.createElement("tr");
        searchparameterdatebody.appendChild(emptyrow);

        var r1 = document.createElement("tr");
        var d1 = document.createElement("td");
        d1.innerHTML = "<b>Fax Status:</b>";
        r1.appendChild(d1);
        var d2 = document.createElement("td");
        d2.innerHTML = document.getElementById("cboFaxStatus").value;
        r1.appendChild(d2);
        searchparameterdatebody.appendChild(r1);

        
        var d3 = document.createElement("td");
        d3.innerHTML = "<b>FAX Sent Date (From):</b>";
        r1.appendChild(d3);
        var d4 = document.createElement("td");
        d4.innerHTML = document.getElementById("dtFaxSentDateFrom").value;
        r1.appendChild(d4);
        searchparameterdatebody.appendChild(r1);

        var d5 = document.createElement("td");
        d5.innerHTML = "<b>FAX Sent Date (To):</b>";
        r1.appendChild(d5);
        var d6 = document.createElement("td");
        d6.innerHTML = document.getElementById("dtFaxSentDateTo").value;
        r1.appendChild(d6);
        searchparameterdatebody.appendChild(r1);

        var r2 = document.createElement("tr");
        var r2d1 = document.createElement("td");
        r2d1.innerHTML = "<b>Recipient Name:</b>";
        r2.appendChild(r2d1);
        var r2d2 = document.createElement("td");
        r2d2.innerHTML = (document?.getElementById("txtRecipientName")?.getAttribute("data-phy-details") != undefined
            && document?.getElementById("txtRecipientName")?.getAttribute("data-phy-details") != "")
            ? JSON.parse(document.getElementById("txtRecipientName").getAttribute("data-phy-details")).sPhyName : "";

        r2.appendChild(r2d2);
        searchparameterdatebody.appendChild(r2);

        var r2d3 = document.createElement("td");
        r2d3.innerHTML = "<b>FAX Sender Name:</b>";
        r2.appendChild(r2d3);
        var r2d4 = document.createElement("td");
        r2d4.innerHTML = document.getElementById("txtFaxSenderName").value;
        r2.appendChild(r2d4);
        searchparameterdatebody.appendChild(r2);

        var r3 = document.createElement("tr");
        var r3d1 = document.createElement("td");
        r3d1.innerHTML = "<b>Patient Name:</b>";
        r3.appendChild(r3d1);
        var r3d2 = document.createElement("td");
        r3d2.innerHTML = (document?.getElementById("txtPatientName")?.getAttribute("data-human-details") != undefined
            && document?.getElementById("txtPatientName")?.getAttribute("data-human-details") != "")
            ? JSON.parse(document.getElementById("txtPatientName").getAttribute("data-human-details")).PatientName : "";
        r3.appendChild(r3d2);
        searchparameterdatebody.appendChild(r3);

        searchparametertabel.appendChild(searchparameterdatebody);

        //Create and download the Excel Report
        var divtag = document.createElement("div");
        divtag.appendChild(searchparametertabel);
        divtag.appendChild(exceltable);

        var nowdate = new Date();
        $(divtag).table2excel({
            filename: "EFax_Search_Report_" + nowdate.getFullYear() + nowdate.getMonth() + 1
                + nowdate.getDate().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + " "
                + nowdate.getHours().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + " "
                + nowdate.getMinutes().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + " "
                + nowdate.getSeconds().toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false }) + " "
                + nowdate.toLocaleString('en-US', { hour: 'numeric', hour12: true }).split(" ")[1] + ".xls",
            preserveColors: true
        });

        table.draw(false);
    }
    else {
        alert("Please search for export Excel.");
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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



var arrAssignto = [];
var bBool = false;
var bcheck = true;
var intAssigntoLength = -1;
function SetAutoSearchRecipientName() {

    //OldCode{
    //var a = "a";
    //a = "v";
    //$("#txtRecipientName").autocomplete({
    //    source: function (request, response) {
    //        if (intAssigntoLength == 0 && bcheck && bBool == false) {
    //            arrAssignto = [];
    //            bBool = true;
    //            var sUserRole = "";
    //            var sUrl = "";
    //            $.ajax({
    //                type: "POST",
    //                contentType: "application/json; charset=utf-8",
    //                url: "frmPatientCommunication.aspx/SearchAssigned",
    //                data: JSON.stringify({
    //                    "sUserName": document.getElementById("txtRecipientName").value.slice(0, 3),
    //                    "sUserRole": sUserRole,
    //                }),
    //                dataType: "json",
    //                async: true,
    //                success: function (data) {
    //                    var jsonData = $.parseJSON(data.d);
    //                    //Jira CAP-2140
    //                    arrAssignto = jsonData.AssignedTo;
    //                    jsonData.AssignedTo = jsonData.AssignedTo.slice(0, 100);
    //                    //Jira CAP-2140 End
    //                    if (jsonData.AssignedTo.length == 0) {
    //                        jsonData.AssignedTo.push('No matches found.')
    //                        response($.map(jsonData, function (item) {
    //                            return {
    //                                label: item

    //                            }
    //                        }));
    //                    }
    //                    else {
    //                        if (arrAssignto.length != 0) {
    //                            var results = FilterWithdelimiter(arrAssignto, request.term, "|", 1);
    //                            results = results.slice(0, 100);
    //                            if (results.length == 0) {
    //                                results.push('No matches found.')
    //                                response($.map(results, function (item) {
    //                                    return {
    //                                        label: item
    //                                    }
    //                                }));
    //                            }
    //                            else {
    //                                response($.map(results, function (item) {
    //                                    return {
    //                                        label: item.split('|')[1],
    //                                        val: item.split('|')[0]
    //                                    }
    //                                }));
    //                            }
    //                        }
    //                        else {
    //                            response($.map(jsonData.AssignedTo, function (item) {
    //                                //arrAssignto.push(item);
    //                                return {
    //                                    label: item.split('|')[1],
    //                                    val: item.split('|')[0]
    //                                }
    //                            }));
    //                        }
    //                    }

    //                    $("#txtRecipientName").focus();
    //                    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //                },
    //                error: function OnError(xhr) {
    //                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //                    if (xhr.status == 999)
    //                        window.location = "/frmSessionExpired.aspx";
    //                    else {
    //                        var log = JSON.parse(xhr.responseText);
    //                        console.log(log);
    //                        alert("USER MESSAGE:\n" +
    //                            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                            "Message: " + log.Message);
    //                    }
    //                }

    //            });

    //        }
    //        if ($("#txtRecipientName").val().length > 2) {
    //            if (arrAssignto.length != 0) {
    //                var results = FilterWithdelimiter(arrAssignto, request.term, "|", 1);
    //                results = results.slice(0, 100);
    //                if (results.length == 0) {
    //                    results.push('No matches found.')
    //                    response($.map(results, function (item) {
    //                        return {
    //                            label: item
    //                        }
    //                    }));
    //                }
    //                else {
    //                    response($.map(results, function (item) {
    //                        return {
    //                            label: item.split('|')[1],
    //                            val: item.split('|')[0]
    //                        }
    //                    }));
    //                }
    //            }
    //        }
    //    },
    //    minlength: 2,
    //    multiple: true,
    //    mustMatch: false,
    //    open: function () {
    //        $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtRecipientName').width());
    //        $('.ui-autocomplete.ui-menu.ui-widget').find('li').css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" });
    //        $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
    //        $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
    //            e.preventDefault();
    //            e.stopImmediatePropagation();
    //        });
    //    },
    //    select: function (event, ui) {
    //        event.preventDefault();

    //        if (ui.item.label != "No matches found.") {
    //            bBool = false;
    //            document.getElementById("txtRecipientName").value = ui.item.label;
    //            document.getElementById("txtRecipientName").attributes["val"] = ui.item.val;
    //            document.getElementById("txtRecipientName").title = ui.item.label;
    //            document.getElementById("txtRecipientName").disabled = true;
                
    //        }
    //        else {
    //            $('#txtRecipientName').val("");
    //            bBool = false;
    //        }
    //    }
    //}).on("paste", function (e) {
    //    intAssigntoLength = -1;
    //    arrAssignto = [];
    //    $(".ui-autocomplete").hide();
    //}).on("keydown", function (e) {
    //    if (e.which == 8) {
    //        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if ($("#txtRecipientName").val().length <= 3)
    //            bBool = false;
    //        else
    //            bBool = true;
    //        $("#txtRecipientName").focus();
    //        bcheck = false;
    //    }
    //    else if (e.which == 46) {
    //        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        bBool = false;
    //        bcheck = false;
    //    }
    //    else {
    //        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        bcheck = true;
    //    }

    //}).on("input", function (e) {
    //    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    if ($("#txtRecipientName").val().length >= 3) {
    //        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (!bBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //        intAssigntoLength = 0;
    //    }
    //    else if ($("#txtRecipientName").val().length != 0 && intAssigntoLength != -1) {
    //        intAssigntoLength = intAssigntoLength + 1;
    //    }
    //    if ($("#txtRecipientName").val().length < 2) {
    //        intAssigntoLength = -1;
    //        arrAssignto = [];
    //        $(".ui-autocomplete").hide();
    //        bBool = false;
    //    }
    //});
    //}OldCode End
    $("#txtRecipientName").attr({ "data-phy-id": "0", "data-phy-details": "", "data-category": "" });
    $('#txtRecipientName').keydown(
        function (event) {
            if (event.which == '13') {
                event.preventDefault();
            }
        });


    $("#txtRecipientName").autocomplete({
        source: function (request, response) {
            if ($("#txtRecipientName").val().trim().length > 2) {
                if (intProviderlen == 0) {

                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    this.element.on("keydown", PreventTyping);
                    arrProvider = [];
                    var strkeyWords = $("#txtRecipientName").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var WSData;


                    var vurl = "";
                    //var test = $("input:radio[name='chkProvider1']:checked").val()
                    //if (test == "Provider") {
                        WSData = {
                            text_searched: strkeyWords[0],
                        };
                        vurl = "./frmFindReferralPhysician.aspx/GetProviderDetailsFaxByTokens"
                    //}
                    //else if (test == "Patient") {
                    //    WSData = {
                    //        text_searched: strkeyWords[0],
                    //        account_status: "ACTIVE",
                    //        patient_status: "ALIVE",
                    //        human_type: "REGULAR"
                    //    };
                    //    vurl = "./frmFindPatient.aspx/GetPatientDetailsByTokens"
                    //}

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: vurl,
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            $("#txtRecipientName").off("keydown", PreventTyping);
                            var jsonData = $.parseJSON(data.d);
                            if (jsonData.Error != undefined) {
                                alert(jsonData.Error);
                                return;
                            }

                            if (jsonData.Result != undefined) {
                                var no_matches = [];
                                no_matches.push(jsonData.Result);
                                response($.map(no_matches, function (item) {
                                    return {
                                        label: item,
                                        val: "0"
                                    }
                                }));
                            }
                            else {
                                var results;
                                if (bMoreThanOneKeyword)
                                    results = FilterFax(jsonData.Matching_Result, request.term);
                                else
                                    results = jsonData.Matching_Result;

                                arrProvider = jsonData.Matching_Result;
                                response($.map(results, function (item) {
                                    return {
                                        label: item.label,
                                        val: JSON.stringify(item.value),
                                        value: item.value.ulPhyId
                                    }
                                }));
                            }
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
                else if (intProviderlen != -1) {

                    var results = FilterFax(arrProvider, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item.label,
                            val: JSON.stringify(item.value),
                            value: item.value.ulPhyId
                        }
                    }));
                }
            }
        },
        minlength: 0,
        multiple: true,
        mustMatch: false,
        select: function (event, ui) {
            event.preventDefault();
            var ProviderDetails = JSON.parse(ui.item.val);
            var txtProviderSearch = document.getElementById("txtRecipientName");

            txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
            txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(ProviderDetails);
            txtProviderSearch.attributes['data-category'].value = ProviderDetails.sCategory;
            txtProviderSearch.value = ui.item.label;
            txtProviderSearch.title = ui.item.label;
            document.getElementById("txtRecipientName").disabled = true;
        },
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtRecipientName').width());
            $('.ui-autocomplete.ui-menu.ui-widget').find('li').css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" });
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $('#txtRecipientName').focus();
            $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intProviderlen = -1;
        arrProvider = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#txtRecipientName").css("color", "black").attr({ "data-phy-id": "0", "data-phy-details": "","data-category":"" });
        if ($("#txtRecipientName").val().length == 0) {
            intProviderlen = -1;
            arrProvider = [];
            $(".ui-autocomplete").hide();
        }
        else {
            intProviderlen = 0;
        }

    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        if (item.label != "No matches found.")
            return $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .append(item.label)
                .appendTo(ul);
        else
            return $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .addClass("disabled")
                .append(item.label)
                .appendTo(ul).on("click", function (e) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                });
    };
    $("#txtRecipientName").keypress(function (e) {
        if (e.keyCode == 64)
            return false;
    });
    

}
var arrFaxSenderName = [];
var bFSBool = false;
var bFScheck = true;
var intFaxSenderNameLength = -1;
function SetAutoSearchFaxSenderName() { 
    //Set autocomplete for FaxSenderName

    $("#txtFaxSenderName").autocomplete({
        source: function (request, response) {
            if (intFaxSenderNameLength == 0 && bFScheck && bFSBool == false) {
                arrFaxSenderName = [];
                bFSBool = true;
                var sUserRole = "";
                var sUrl = "";
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmPatientCommunication.aspx/SearchAssigned",
                    data: JSON.stringify({
                        "sUserName": document.getElementById("txtFaxSenderName").value.slice(0, 3),
                        "sUserRole": sUserRole,
                    }),
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var jsonData = $.parseJSON(data.d);
                        //Jira CAP-2140
                        arrFaxSenderName = jsonData.AssignedTo;
                        jsonData.AssignedTo = jsonData.AssignedTo.slice(0, 100);
                        //Jira CAP-2140 End
                        if (jsonData.AssignedTo.length == 0) {
                            jsonData.AssignedTo.push('No matches found.')
                            response($.map(jsonData, function (item) {
                                return {
                                    label: item

                                }
                            }));
                        }
                        else {
                            if (arrFaxSenderName.length != 0) {
                                var results = FilterWithdelimiter(arrFaxSenderName, request.term, "|", 1);
                                results = results.slice(0, 100);
                                if (results.length == 0) {
                                    results.push('No matches found.')
                                    response($.map(results, function (item) {
                                        return {
                                            label: item
                                        }
                                    }));
                                }
                                else {
                                    response($.map(results, function (item) {
                                        return {
                                            label: item.split('|')[1],
                                            val: item.split('|')[0]
                                        }
                                    }));
                                }
                            }
                            else {
                                response($.map(jsonData.AssignedTo, function (item) {
                                    //arrFaxSenderName.push(item);
                                    return {
                                        label: item.split('|')[1],
                                        val: item.split('|')[0]
                                    }
                                }));
                            }
                        }

                        $("#txtFaxSenderName").focus();
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
            if ($("#txtFaxSenderName").val().length > 2) {
                if (arrFaxSenderName.length != 0) {
                    var results = FilterWithdelimiter(arrFaxSenderName, request.term, "|",1);
                    results = results.slice(0, 100);
                    if (results.length == 0) {
                        results.push('No matches found.')
                        response($.map(results, function (item) {
                            return {
                                label: item
                            }
                        }));
                    }
                    else {
                        response($.map(results, function (item) {
                            return {
                                label: item.split('|')[1],
                                val: item.split('|')[0]
                            }
                        }));
                    }
                }
            }
        },
        minlength: 2,
        multiple: true,
        mustMatch: false,
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtFaxSenderName').width());
            $('.ui-autocomplete.ui-menu.ui-widget').find('li').css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" });
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        select: function (event, ui) {
            event.preventDefault();

            if (ui.item.label != "No matches found.") {
                bFSBool = false;
                document.getElementById("txtFaxSenderName").value = ui.item.label;
                document.getElementById("txtFaxSenderName").attributes["val"] = ui.item.val;
                document.getElementById("txtFaxSenderName").title = ui.item.label;
                document.getElementById("txtFaxSenderName").disabled = true;
            }
            else {
                $('#txtFaxSenderName').val("");
                bFSBool = false;
            }
        }
    }).on("paste", function (e) {
        intFaxSenderNameLength = -1;
        arrFaxSenderName = [];
        $(".ui-autocomplete").hide();
    }).on("keydown", function (e) {
        if (e.which == 8) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtFaxSenderName").val().length <= 3)
                bFSBool = false;
            else
                bFSBool = true;
            $("#txtFaxSenderName").focus();
            bFScheck = false;
        }
        else if (e.which == 46) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bFSBool = false;
            bFScheck = false;
        }
        else {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bFScheck = true;
        }

    }).on("input", function (e) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtFaxSenderName").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bFSBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intFaxSenderNameLength = 0;
        }
        else if ($("#txtFaxSenderName").val().length != 0 && intFaxSenderNameLength != -1) {
            intFaxSenderNameLength = intFaxSenderNameLength + 1;
        }
        if ($("#txtFaxSenderName").val().length < 2) {
            intFaxSenderNameLength = -1;
            arrFaxSenderName = [];
            $(".ui-autocomplete").hide();
            bFSBool = false;
        }
    });

}
function SetAutoSearchPatientDetails() {
    $("#imgClearPatientNameText").on("click", function () {
        $('#txtPatientName').val('').focus();
        $('#txtPatientName').attr('data-human-id', '0');
        $('#txtPatientName').attr('data-human-details', '');
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });

    $("#txtPatientName").autocomplete({
        source: function (request, response) {
            if ($("#txtPatientName").val().trim().length > 2) {
                if (intPatientlen == 0) {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    this.element.on("keydown", PreventTyping);
                    arrPatient = [];
                    var strkeyWords = $("#txtPatientName").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var account_status = "ACTIVE";
                    var patient_status = "ALIVE";
                    var patient_type = "ALL";
                    var WSData = {
                        text_searched: strkeyWords[0],
                        account_status: account_status,
                        patient_status: patient_status,
                        human_type: patient_type
                    };

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "./frmFindPatient.aspx/GetPatientDetailsByTokens",
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            $("#txtPatientName").off("keydown", PreventTyping);
                            var jsonData = $.parseJSON(data.d);
                            if (jsonData.Error != undefined) {
                                alert(jsonData.Error);
                                return;
                            }
                            if (jsonData.Result != undefined) {
                                var no_matches = [];
                                no_matches.push(jsonData.Result);
                                response($.map(no_matches, function (item) {
                                    return {
                                        label: item,
                                        val: "0"
                                    }
                                }));
                            }
                            else {
                                var results;
                                if (bMoreThanOneKeyword)
                                    results = Filter(jsonData.Matching_Result, request.term);
                                else
                                    results = jsonData.Matching_Result;

                                arrPatient = jsonData.Matching_Result;
                                response($.map(results, function (item) {
                                    return {
                                        label: item.label,
                                        val: JSON.stringify(item.value),
                                        value: item.value.HumanId
                                    }
                                }));
                            }
                        },
                        error: function OnError(xhr) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            if (xhr.status == 999)
                                window.location = "/frmSessionExpired.aspx";
                            else {
                                if (isValidJSON(xhr.responseText)) {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                        "Message: " + log.Message);
                                }
                                else {
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry.");
                                }
                            }
                        }

                    });
                }
                else if (intPatientlen != -1) {
                    var results = Filter(arrPatient, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item.label,
                            val: JSON.stringify(item.value),
                            value: item.value.HumanId
                        }
                    }));
                }
            }
        },
        minlength: 0,
        multiple: true,
        mustMatch: false,
        select: PatientSelected,
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width("716px");
            $(".ui-autocomplete.ui-menu.ui-widget#ui-id-3").css({ "max-height": "320px" });
            $(".ui-autocomplete.ui-menu.ui-widget#ui-id-3").css({ "overflow": "auto" });
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "10px");
            $('#txtPatientName').focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#txtPatientName").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        if ($("#txtPatientName").val().charAt(e.currentTarget.value.length - 1) == " ") {
            if (e.currentTarget.value.split(" ").length > 2)
                intPatientlen = intPatientlen + 1;
            else
                intPatientlen = 0;
        }
        else {
            if ($("#txtPatientName").val().length != 0 && intPatientlen != -1) {
                intPatientlen = intPatientlen + 1;
            }
            if ($("#txtPatientName").val().length == 0 || $("#txtPatientName").val().indexOf(" ") == -1) {
                intPatientlen = -1;
                arrPatient = [];
                $(".ui-autocomplete").hide();
            }
        }
    })

    $("#txtPatientName").data("ui-autocomplete")._renderItem = function (ul, item) {
        if (item.label != "No matches found.") {
            var HumanDetails = $.parseJSON(item.val);
            var list_item = $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .append(item.label)
                .appendTo(ul);
            if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                list_item.addClass("inactive");
            if (HumanDetails.Status.toUpperCase() == "DECEASED")
                list_item.addClass("deceased");
            return list_item;
        }
        else
            return $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .addClass("disabled")
                .append(item.label)
                .appendTo(ul).on("click", function (e) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                });
    };

}
function FilterWithdelimiter(array, terms, checkdelimiter, SearchIndex) {
    var arrayindex = parseInt(SearchIndex);
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 0 && checkdelimiter != "") {
        var first_resultant = array;
        var resultant;

        for (var i = 0; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {

                if (item != undefined && checkdelimiter != "") {
                    return item.split(checkdelimiter)[arrayindex].toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
                }

            });
            first_resultant = resultant;
        }

        return first_resultant;
    }
    else {
        return array;
    }
}
function Filter(array, terms) {
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 1 && arrayOfTerms[1].trim() != "") {
        var first_resultant = array;
        var resultant;
        for (var i = 1; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {
                return item.label.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
            });
            first_resultant = resultant;
        }
        return first_resultant;
    }
    else {
        return array;
    }
}
function FilterFax(array, terms) {
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 1 && arrayOfTerms[1].trim() != "") {
        var first_resultant = array;
        var resultant;
        for (var i = 1; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {
                if (item.label != undefined)
                    return item.label.toUpperCase().indexOf(arrayOfTerms[i].toString().toUpperCase()) > -1;
                else if (item != undefined)
                    return item.toUpperCase().indexOf(arrayOfTerms[i].toString().toUpperCase()) > -1;
            });
            first_resultant = resultant;
        }
        return first_resultant;
    }
    else {
        return array;
    }
}
function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}
function PatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtPatientName = document.getElementById("txtPatientName");

    var WSData = {
        HumanID: ui.item.value,
        FullDetails: ui.item.label
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/GetHumanDetails",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtPatientName = document.getElementById('txtPatientName');
            txtPatientName.value = SelectedPatient.DisplayString;
            txtPatientName.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
        }
    });
    txtPatientName.value = ui.item.label;
    txtPatientName.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    txtPatientName.disabled = true;
    return false;
}