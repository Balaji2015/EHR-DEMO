function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function OpenFindPatinet() {
    setTimeout(
    function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        var oWindow = GetRadWindow();
        var childWindow = $find('ModalWindowAppmnt')._browserWindow.GetRadWindow().BrowserWindow.radopen("frmFindPatient.aspx?ScreenName=Appointments", "ModalWindowAppmnt");
        setRadWindowProperties(childWindow, 251, 1200);
        childWindow.add_close(function refrehCarrier(oWindow, args) {
            var Result = args.get_argument();
            if (Result) {

                document.getElementById("hdnHumanID").value = Result.HumanId;
                document.getElementById("btnFindPatientRefresh").click();
            }
        });
    }, 0);
    return false;
}
function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

}
function OpenCancelAppt() {
    WaitCursor();
    //var index = parseInt(document.getElementById('hdnSelectedIndex').value) + 1;
    //if (isNaN(index)) {
    //    DisplayErrorMessage("110027");
    //     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    //    return false;
    //}
    if ($('#grdAppointment tbody tr.rgSelectedRow').length <= 0) {
        DisplayErrorMessage("110027");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    //var grid = $find("grdAppointment");
    //var MasterTable = grid.get_masterTableView();
    //var selectedRows = MasterTable.get_selectedItems();
    if ($('#grdAppointment') != null) {
        // for (var i = 0; i < selectedRows.length; i++) {
        //var row = selectedRows[i];
        //Encounterid = MasterTable.getCellByColumnUniqueName(row, "Appt_ID").innerHTML;
        //var is_archieve = MasterTable.getCellByColumnUniqueName(row, "Is_Archieve").innerHTML;

        Encounterid = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[4].innerText;
        var is_archieve = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[10].innerText;
        if (is_archieve == "Main") {

            setTimeout(
                function () {
                    var oWindow = GetRadWindow();
                    var childWindow = $find('ModalWindowAppmnt')._browserWindow.GetRadWindow().BrowserWindow.radopen("frmCancelAppointment.aspx?EncounterID=" + Encounterid, "ModalWindowMngt");
                    setRadWindowProperties(childWindow, 240, 520);
                    childWindow.add_close(function refrehCarrier(oWindow, args) {
                        document.getElementById("btnRefresh").click();
                    });
                }, 0);
        }
        else {
            DisplayErrorMessage("110093");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        return false;
        //}
    }
}
function OpenEditAppointment() {
    WaitCursor();
    //var index = parseInt(document.getElementById('hdnSelectedIndex').value) + 1;
    //if (isNaN(index)) {
    //    DisplayErrorMessage("110027");
    //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    return false;
    //}
    if ($('#grdAppointment tbody tr.rgSelectedRow').length <= 0) {
        DisplayErrorMessage("110027");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }


    //var grid1 = $find("grdAppointment");
    //var MasterTable1 = grid1.get_masterTableView();
    //var selectedRows = MasterTable1.get_selectedItems();
    if ($('#grdAppointment') != null) {
        //for (var i = 0; i < selectedRows.length; i++) {
        //    var row = selectedRows[i];
        //    var humanID = document.getElementById("hdnHumanID").value;
        //    var fac = MasterTable1.getCellByColumnUniqueName(row, "FacilityName").innerHTML;
        //    var encounterId = MasterTable1.getCellByColumnUniqueName(row, "Appt_ID").innerHTML;
        //    var currentProcess = MasterTable1.getCellByColumnUniqueName(row, "CurrentProcess").innerHTML;
        //    var selectedDate = MasterTable1.getCellByColumnUniqueName(row, "AppointmentDate").innerHTML + " " + MasterTable1.getCellByColumnUniqueName(row, "AppointmentTime").innerHTML;
        //    var physicianName = MasterTable1.getCellByColumnUniqueName(row, "ProviderName").innerHTML;
        //    var physicianId = MasterTable1.getCellByColumnUniqueName(row, "Appt_Provider_Id").innerHTML;
        //    var is_archieve = MasterTable1.getCellByColumnUniqueName(row, "Is_Archieve").innerHTML;


        var humanID = document.getElementById("hdnHumanID").value;
        var fac = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[3].innerText;
        var encounterId = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[4].innerText;
        var currentProcess = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[5].innerText;
        var selectedDate = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[0].innerText + " " + $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[1].innerText;
        var physicianName = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[2].innerText;
        var physicianId = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[6].innerText;
        var is_archieve = $('#grdAppointment tbody tr.rgSelectedRow')[0].cells[10].innerText;




        //Jira #CAP-68
        //if (is_archieve == "Main") {
        var Facility;
        if (fac.indexOf("#") != -1) {
            Facility = fac.replace("#", "_");
        }
        else {
            Facility = fac;
        }

        if (humanID != undefined) {
            setTimeout(
                function () {
                    var oWindow = GetRadWindow();
                    var childWindow = $find('ModalWindowAppmnt')._browserWindow.GetRadWindow().BrowserWindow.radopen("frmEditAppointment.aspx?Human_id=" + humanID + "&EncounterID=" + encounterId + "&facility=" + Facility + "&PhysicianName=" + physicianName + "&PhysicianID=" + physicianId + "&SelectedDate=" + selectedDate + "&CurrentProcess=" + currentProcess, "ModalWindowMngt");
                    setRadWindowProperties(childWindow, 800, 840);
                    childWindow.add_close(function refrehCarrier(oWindow, args) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        document.getElementById("btnRefresh").click();
                    });
                }, 0);
        }
        //Jira #CAP-68
        //}
        //else {
        //    DisplayErrorMessage("110092");
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}
        return false;
        //}
    }

}
function CloseWindow() {
   
    self.close();
}
function returnToParent(args) {
    var oArg = new Object();
    oArg.result = args;
    var oWnd = GetRadWindow();
    if (oWnd != null) {
        if (oArg.result) {
            oWnd.close(oArg.result);
        }
        else {
            oWnd.close(oArg.result);
        }
    }
    else {
        self.close();
    }
}
function WaitCursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function grdAppointment_OnRowClick(sender, args) {
    var grdFindPatient = $find('grdAppointment');
    document.getElementById('hdnSelectedIndex').value = args._itemIndexHierarchical;
    var index = parseInt(document.getElementById("hdnSelectedIndex").value);
    var MasterTable = grdFindPatient.get_masterTableView();
    row = MasterTable.get_dataItems()[index];
    if (MasterTable.getCellByColumnUniqueName(row, "CurrentProcess").innerHTML == "SCHEDULED") {
        document.getElementById("btnCancelAppointment").disabled = false;
    }
    else {
        document.getElementById("btnCancelAppointment").disabled = true;
    }
    if (MasterTable.getCellByColumnUniqueName(row, "CurrentProcess").innerHTML == "MA_PROCESS") {
        document.getElementById("btnEditAppointment").disabled = true;
    }
    else {
        document.getElementById("btnEditAppointment").disabled = false;
    }
}
function FillResult() {
    loadFillResult();
    
}
function loadFillResult() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $("#hdnHumanID")[0].value;
    $("#divTable").empty();
    $("#divTable").append(`
     <table id="grdAppointment" runat="server" class="table table-bordered Gridbodystyle" style="width:100%;table-layout:fixed;empty-cells:show;">
    <thead class="header" style="border: 0px;width:96.7%;">
        <tr class="header">
            <th style="border: 1px solid #909090;text-align: center;width: 6%;">Appointment Date Time</th>
            <th style="border: 1px solid #909090;text-align: center;display:none;">AppointmentTime</th>
            <th style="border: 1px solid #909090;text-align: center;width: 8%;">Provider Name</th>
            <th style="border: 1px solid #909090;text-align: center;width: 7%;">Facility Name</th>
            <th style="border: 1px solid #909090;text-align: center;display:none;">Appt ID</th>
            <th style="border: 1px solid #909090;text-align: center;width: 11%;">Current Process</th>
            <th style="border: 1px solid #909090;text-align: center;display:none;">Appt Provider Id</th>
            <th style="border: 1px solid #909090;text-align: center;display:none;">Test Ordered</th>
            <th style="border: 1px solid #909090;text-align: center;width: 8%;">Rescheduled Appointment Date</th>
            <th style="border: 1px solid #909090;text-align: center;width: 9%;">Reason for Cancelation</th>
            <th style="border: 1px solid #909090;text-align: center;display:none;">Is Archieve</th>
        </tr>
    </thead>
    </table>`);

    var facilityLibrary = "";
    var LoginfacilityName = "";
    var isAncilaryOfLoginFacility = "";
    var dataTable = new DataTable('#grdAppointment', {
        serverSide: false,
        lengthChange: false,
        searching: false,
        scrollCollapse: true,
        scrollY: '331px',
        processing: false,
        ordering: true,
        autoWidth: false,
        order: [],
        pageLength: 15,
        dom: '<"top"ipf>rt<"bottom"l><"clear">',
        ajax: {
            url: '/frmFindAllAppointments.aspx/FindResult',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = JSON.stringify({
                    "sHumanID": $("#hdnHumanID")[0].value,
                    "chkShowOldAppointments": document.getElementById("chkShowOldAppointments").checked
                });
                return d;
            },
            dataSrc: function (json) {
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);
                facilityLibrary = objdata.facilityLibrary;

                LoginfacilityName = document.cookie.split(";").filter(f => f.indexOf("CFacilityName") > -1);

                if (LoginfacilityName != undefined && LoginfacilityName != null && LoginfacilityName.length > 0) {
                    LoginfacilityName = LoginfacilityName[0].split("=")[1];
                }
                isAncilaryOfLoginFacility = facilityLibrary.filter(fl => fl.Fac_Name == LoginfacilityName);
                isAncilaryOfLoginFacility = isAncilaryOfLoginFacility[0].Is_Ancillary;
                if (isAncilaryOfLoginFacility == 'Y') {
                    if ($(top?.window?.document)?.find('#RadWindowWrapper_ctl00_ModalWindow')?.[0] != undefined) {
                        $(top.window.document).find('#RadWindowWrapper_ctl00_ModalWindow')[0].style.width = "1010px";
                        $("#divTable").parent()[0].style.width = "983px";
                        $("#pnlButtons")[0].style.width = "935px";
                    }
                    else if (top?.window?.document?.getElementsByName("ModalWindow")[0]?.contentWindow?.document?.getElementById("RadWindowWrapper_ctl00_ModalWindow") != undefined) {
                        $(top?.window?.document?.getElementsByName("ModalWindow")[0]?.contentWindow?.document?.getElementById("RadWindowWrapper_ctl00_ModalWindow"))[0].style.width = "1010px";
                        $(top?.window?.document?.getElementsByName("ModalWindow")[0]?.contentWindow?.document?.getElementsByName("ctl00_ModalWindow")[0].contentWindow?.document.getElementById("divTable")).parent()[0].style.width = "983px";
                        $(top?.window?.document?.getElementsByName("ModalWindow")[0]?.contentWindow?.document?.getElementsByName("ctl00_ModalWindow")[0].contentWindow?.document.getElementById("pnlButtons"))[0].style.width = "935px";
                    }
                    else if (top?.window?.document?.getElementById("ctl00_C5POBody_EncounterContainer")?.contentWindow?.document?.getElementById("RadWindowWrapper_ctl00_ModalWindow") != undefined) {
                        $(top?.window?.document?.getElementById("ctl00_C5POBody_EncounterContainer")?.contentWindow?.document?.getElementById("RadWindowWrapper_ctl00_ModalWindow"))[0].style.width = "1010px";
                        $(top?.window?.document?.getElementById("ctl00_C5POBody_EncounterContainer")?.contentWindow?.document?.getElementsByName("ctl00_ModalWindow")[0].contentWindow?.document.getElementById("divTable")).parent()[0].style.width = "983px";
                        $(top?.window?.document?.getElementById("ctl00_C5POBody_EncounterContainer")?.contentWindow?.document?.getElementsByName("ctl00_ModalWindow")[0].contentWindow?.document.getElementById("pnlButtons"))[0].style.width = "935px";
                    }
                }
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
                data: 'Appointment_Date_Time', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    return ConvertDate(dt1.replaceAll("T", " "));
                },
                sWidth: '6% !important', type: 'date', sClass:'TableCellBorder process-word-wrap'
            },
            {
                data: 'Appointment_Date_Time', render: function (data, type, row) { return ""; },
                type:'date', sClass: "hide_column"
            },
            { data: 'Physician_Name', sWidth: '8% !important', sClass: 'TableCellBorder process-word-wrap' },
            { data: 'Facility_Name', sWidth: '7% !important', sClass: 'TableCellBorder process-word-wrap' },
            { data: 'Encounter_ID', sClass: "hide_column",  },
            { data: 'Current_Process', sWidth:'11% !important', sClass: 'TableCellBorder process-word-wrap' },
            {
                data: 'Physician_ID', render: function (data, type, row) {
                    var fac = facilityLibrary.filter(fl => fl.Fac_Name == row.Facility_Name);
                    if (fac.length == 0 || (fac.length > 0 && fac[0].Is_Ancillary != "Y")) {
                        return row.Physician_ID;
                    }
                    else {
                        return row.Machine_Technician_Library_ID;
                    }
                }, sClass: "hide_column", 
            },
            {
                data: 'Test_Ordered', sWidth: '6% !important', sClass: 'TableCellBorder process-word-wrap'
            },
            {
                data: 'Rescheduled_Appointment_Date', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    dt1 = ConvertDate(dt1.replaceAll("T", " "));
                    var dt2 = dt1.split(' ');
                    if (dt2.length > 0) {
                        if (dt2.indexOf("01-01-0001") > -1) {
                            return "";
                        }
                    }
                    return dt1;
                }, sWidth: '8% !important', type: 'date', sClass: 'TableCellBorder process-word-wrap'
            },
            { data: 'Reason_for_Cancelation', sWidth: '9% !important', sClass: 'TableCellBorder process-word-wrap' },
            { data: 'Human_Type', sClass: "hide_column" }
                
        ],
        createdRow: function (row, data, dataIndex) {

            if (isAncilaryOfLoginFacility != "Y") {

                row.children[7].classList.add("hide_column");
            }
            else {
                row.children[7].classList.remove("hide_column");
            }
        },
        initComplete: function (settings, json) {

            if (isAncilaryOfLoginFacility != "Y") {
                $(".dataTables_scrollHead thead tr th:nth-child(8)")[0].style.display = "none";
            }
            else {
                $(".dataTables_scrollHead thead tr th:nth-child(8)")[0].style.display = "";
            }

            //Always select first iteam
            if ($("#grdAppointment tbody tr").length > 0 && $("#grdAppointment tbody tr td").length > 1) {
                $($("#grdAppointment tbody tr")[0]).click();
            }
        }
    });

    $("#grdAppointment_paginate").on('click', function () {

        $('#grdAppointment tr').removeClass("rgSelectedRow");
        document.getElementById("btnCancelAppointment").disabled = true;
    });

    $(".dataTables_scrollHead thead tr").on('click', function () {
        $('#grdAppointment tr').removeClass("rgSelectedRow");
        document.getElementById("btnCancelAppointment").disabled = true;
        document.getElementById("divTable").scrollTop = 0;
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.rgSelectedRow').removeClass('rgSelectedRow');
        document.getElementById("btnCancelAppointment").disabled = true;
        document.getElementById("divTable").scrollTop = 0;
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.rgSelectedRow').removeClass('rgSelectedRow');
        document.getElementById("btnCancelAppointment").disabled = true;
        document.getElementById("divTable").scrollTop = 0;
    });

    $('#grdAppointment tbody').on('click', 'tr', function () {

        if ($(this)[0].cells[0].innerText != 'No data available in table') {
            $('#grdAppointment tr').removeClass("rgSelectedRow");
            $(this).addClass("rgSelectedRow");

            if ($("#grdAppointment tbody tr").length > 0) {

                if ($("#grdAppointment tbody tr").length == 1 && $('#grdAppointment tbody tr')[0].cells.length > 1 &&
                    ($('#grdAppointment tbody tr')[0].cells[3].innerText == '' && $('#grdAppointment tbody tr')[0].cells[3].innerText == '&nbsp;')) {

                    document.getElementById("btnCancelAppointment").disabled = true;
                    document.getElementById("btnEditAppointment").disabled = true;
                }

                if ($(this)[0].cells.length > 4) {

                    if ($(this)[0].cells[5].innerText == "MA_PROCESS") {
                        document.getElementById("btnEditAppointment").disabled = true;
                    }
                    else {
                        document.getElementById("btnEditAppointment").disabled = false;
                    }

                    if ($(this)[0].cells[5].innerText != "SCHEDULED") {
                        document.getElementById("btnCancelAppointment").disabled = true;
                    }
                    else if ($(this)[0].cells[5].innerText == "SCHEDULED") {
                        document.getElementById("btnCancelAppointment").disabled = false;
                    }

                }
            }



        }
    });


}

function ShowAllAppoinmentsClick() {
    WaitCursor();
    FillResult();
    //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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

function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate + ' UTC');
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-01-0001';
    else
        then = ('0' + now.getDate().format("dd")).slice(-2) + '-' + monthNames[now.getMonth()] + '-' + now.getFullYear();
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