$(document).ready(function () {
    EFaxoutboxload();
});
////CAP-1831 - eFax Outbox - Introduce filter 
//$('#chkLast30daysTransactions').change(function () {
//    EFaxoutboxload();
//});
//CAP-1831 - eFax Outbox - Introduce filter 
function EFaxoutboxload() {
    $('#divEFaxTable').empty();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var last30daysTransaction = $('#chkLast30daysTransactions').prop('checked');
    last30daysTransaction = last30daysTransaction == undefined ? true : last30daysTransaction;
    //$.ajax({
    //    type: "POST",
    //    url: "frmEFax.aspx/EFaxoutboxload",
    //    contentType: "application/json;charset=utf-8",
    //    data: '{last30daysTransaction: "' + last30daysTransaction + '"}',
    //    datatype: "json",
    //    success: function success(data) {
    //        var objdata = $.parseJSON(data.d);
    //        var tabContents;
    //        $('#divEFaxTable').empty();
    //        if (objdata.ActivityLogList.length > 0) {
    //            for (var i = 0; i < objdata.ActivityLogList.length; i++) {
    //                if (objdata.ActivityLogList[i].Fax_File_Path == "" && objdata.ActivityLogList[i].Fax_Sent_File_Path == "") {
    //                    if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
    //                        if (i == 0)
    //                            tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
    //                        else
    //                            tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");'  title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

    //                    }
    //                    else {
    //                        if (i == 0)
    //                            tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
    //                        else
    //                            tabContents = tabContents + " <tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
    //                    }

    //                }
    //                else {
    //                    if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
    //                        if (i == 0)
    //                            tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
    //                        else
    //                            tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

    //                    }
    //                    else {
    //                        if (i == 0)
    //                            tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
    //                        else
    //                            tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
    //                    }
    //                }

    //            }
    //        }
    //        if (tabContents == undefined)
    //            tabContents = '';
    //        $("#divEFaxTable").append("<table id='EFaxTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:990px;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Recipient Company</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Recipient Fax</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'>Sent Date and Time </th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Status</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Description</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    //        $('#EFaxTable th').addClass('header');
    //        //CAP - 1802
    //        /*scrolify($('#EFaxTable'), 635);*/
    //        scrolify($('#EFaxTable'), 500);
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    },
    //    error: function OnError(xhr) {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        if (xhr.status == 999)
    //            window.location = "/frmSessionExpired.aspx";
    //        else {
    //            var log = JSON.parse(xhr.responseText);
    //            console.log(log);
    //            alert("USER MESSAGE:\n" +
    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
    //                "Message: " + log.Message);
    //        }
    //    }
    //});
    $("#divEFaxTable").append("<table id='EFaxTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:990px;'><thead class='header' style='border: 0px; width: 96.7 %;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Recipient Company</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Recipient Fax</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'>Sent Date and Time </th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Status</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Description</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>Encounter ID</th><th style='border: 1px solid #909090;text-align: center;width: 4%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'></tbody></table>");
    var datatable = new DataTable('#EFaxTable', {
        serverSide: false,
        lengthChange: false,
        scrollCollapse: true,
        scrollY: '450px',
        searching: true,
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 30,
        language: {
            search: "",
            searchPlaceholder: "Search by Subject or Status or Description",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmEFax.aspx/EFaxoutboxload',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "last30daysTransaction": last30daysTransaction
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
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            {
                data: 'Fax_Recipient_Name', render: function (data, type, row) {
                    return data.split("|")[0];
                },
                searchable: false
            },
            {
                data: 'Fax_Recipient_Company', searchable: false
            },
            {
                data: 'Fax_Recipient_Number', searchable: false
            },
            { data: 'Subject' },
            {
                data: "Activity_Date_And_Time", render: function (data, type, row) {
                    var date = data.replace("T", " ");
                    return date;
                },
                type: 'date', searchable: false
            },
            { data: 'Fax_Status' },
            { data: 'Error_Description' },
            { data: 'Encounter_ID', searchable: false },
            {
                data: "", render: function (data, type, row) {
                    if (row.Fax_File_Path == '' && row.Fax_Sent_File_Path == '') {
                        return "<i onclick='CloseOutboxImage(" + row.Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i>"
                    }
                    else {
                        return "<i onclick='OpenViewerforEFoxoutBox(" + row.Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i>"
                    }

                }, searchable: false
            },
            {
                data: "", render: function (data, type, row) {
                    if (row.Fax_Status.toUpperCase() == "FAILED") {
                        return "<i onclick='funRetry(" + row.Id + ");' class='glyphicon glyphicon-refresh'/>"
                    }
                    else { return "<i></i>"; }
                }, searchable: false
            }
        ], createdRow: function (row, data, dataIndex) {
            $(row)[0].id = data.Id;
        },
        initComplete: function (settings, data, json) {
            $("#EFaxTable_filter input")[0].classList.add('searchicon');
        }

    });


    $("#EFaxTable thead tr").on('click', function () {
        document.getElementById("divEFaxTable").scrollTop = 0;
        //$("#EFaxTable thead")[0].style.display = "none"
    });
    $('#EFaxTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '3px',
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
function funalert(e) {
    alert('ActivityLog Failed id:' + e);
}
function funRetry(e) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        url: "frmEFax.aspx/EFaxoutboxRetry",
        contentType: "application/json;charset=utf-8",
        data: '{ActivityId: "' + e + '"}',
        datatype: "json",
        success: function success(data) {
            //var objdata = $.parseJSON(data.d);
            //var tabContents;
            //$('#divEFaxTable').empty();
            //if (objdata.ActivityLogList.length > 0) {
            //    for (var i = 0; i < objdata.ActivityLogList.length; i++) {
            //        if (objdata.ActivityLogList[i].Fax_File_Path == "" && objdata.ActivityLogList[i].Fax_Sent_File_Path == "") {

            //            if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
            //                if (i == 0)
            //                    tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
            //                else
            //                    tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

            //            }
            //            else {
            //                if (i == 0)
            //                    tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
            //                else
            //                    tabContents = tabContents + " <tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");'  title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
            //            }

            //        }
            //        else {
            //            if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
            //                if (i == 0)
            //                    tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
            //                else
            //                    tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

            //            }
            //            else {
            //                if (i == 0)
            //                    tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
            //                else
            //                    tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
            //            }
            //        }

            //    }
            //    DisplayErrorMessage('1011134');
            //}
            //if (tabContents == undefined)
            //    tabContents = '';
            //$("#divEFaxTable").append("<table id='EFaxTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:990px;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Recipient Company</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Recipient Fax</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'>Sent Date and Time </th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Status</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Description</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            //$('#EFaxTable th').addClass('header');

            ////CAP - 1802
            ///*scrolify($('#EFaxTable'), 635);*/
            //scrolify($('#EFaxTable'), 500);
            //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            EFaxoutboxload();
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);
                ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
            }
        }
    });
}


function OpenViewerforEFoxoutBox(EFaxId) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var test = $(top.window.document).find('#ProcessModalExam').modal();
    test.modal({ backdrop: 'static', keyboard: false }, 'show');

    $(top.window.document).find('#btnCloseExam').css("display", "block");
    $(top.window.document).find('#ProcessModalExam')[0].style.marginLeft = "56px";
    $(top.window.document).find('#ProcessModalExam')[0].style.marginTop = "115px";
    $(top.window.document).find("#ProcessFrameExam")[0].style.height = "101%";
    $(top.window.document).find("#mdldlgExam")[0].style.height = "86.5%";
    $(top.window.document).find("#mdldlgExam")[0].style.width = "79%";
    $(top.window.document).find("#mdldlgExam")[0].style.marginLeft = "-8px";
    $(top.window.document).find("#mdldlgExam")[0].style.marginTop = "0px";
    $(top.window.document).find("#ProcessModalExam")[0].style.width = "121%";
    $(top.window.document).find("#ProcessModalExam")[0].style.zIndex = "50001";
    $(top.window.document).find('#ProcessFrameExam')[0].contentDocument.location.href = "frmImageViewer.aspx?Source=EFAX&ActivityId=" + EFaxId;
    $(top.window.document).find("#ModalTitleExam")[0].textContent = "Image Viewer -EFax";

    return false;
}





function CloseOutboxImage() {
    DisplayErrorMessage('1011135');
}
function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    
    if (tblAsJQueryObject[0] != undefined) {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
    }
}