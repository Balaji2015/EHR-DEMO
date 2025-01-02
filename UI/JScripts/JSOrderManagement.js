$(document).ready(function () {
    //$(".loaderClass").click(function () {
    //    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //});
    document.getElementById("btnAddResult").disabled = true;
    cboOrderType_SelectedIndexChanged();
    if (document.getElementById('hdnPostback').value == "True") {
        //Load();
    }
    else {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Control#</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>View Result</th>
    <th style='border: 1px solid #909090; display: none;'>Encounter_ID</th>
    <th style='border: 1px solid #909090; display: none;'>Physician_ID</th>
    <th style='border: 1px solid #909090; display: none;'>Lab_ID</th>
    <th style='border: 1px solid #909090; display: none;'>IsElectronic Signature</th>
    <th style='border: 1px solid #909090; display: none;'>Index Order ID</th>
    </tr>
    </thead>
</table>`);
    }

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

});


function OpenFindPhysicianOld() {
    var oWnd = GetRadWindow();
    var oBrowserWnd = GetRadWindow().BrowserWindow;
    var oWin = oBrowserWnd.radopen("frmFindReferralPhysician.aspx", "OrderManagementWindow");
    setTimeout(
        function () {
            oWin.remove_close(OnClientCloseFindProvider);
            oWin.SetModal(true);
            oWin.set_visibleStatusbar(false);
            oWin.setSize(930, 256);
            oWin.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
            oWin.set_iconUrl("Resources/16_16.ico");
            oWin.set_keepInScreenBounds(true);
            oWin.set_centerIfModal(true);
            oWin.center();
            oWin.set_reloadOnShow(true);
            oWin.remove_close(OnClientCloseFindPatient);
            oWin.add_close(OnClientCloseFindProvider);
        }, 0);
    return false;
}
function OpenFindPhysician() {
    setTimeout(function () {
        var oWnd = GetRadWindow();
        var childWindow = oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx", "OrderManagementWindow");
        setRadWindowProperties(childWindow, 256, 930);
        childWindow.remove_close(OnClientCloseFindPatient)
        childWindow.add_close(OnClientCloseFindProvider);
    }, 0);
    return false;
}
function FindPatient() {
    setTimeout(function () {
        var oWnd = GetRadWindow();
        var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx", "OrderManagementWindow");
        setRadWindowProperties(childWindow, 251, 1200);
        childWindow.add_close(OnClientCloseFindPatient)
        childWindow.remove_close(OnClientCloseFindProvider);
    }, 0);
    return false;
}


function FindPatientOld() {
    var oBrowserWnd = GetRadWindow().BrowserWindow;
    var childWindow = oBrowserWnd.radopen("frmFindPatient.aspx", "OrderManagementWindow");
    setTimeout(
        function () {

            childWindow.SetModal(true);
            childWindow.remove_close(OnClientCloseFindPatient);
            childWindow.set_visibleStatusbar(false);
            childWindow.setSize(1200, 251);
            childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
            childWindow.set_iconUrl("Resources/16_16.ico");
            childWindow.set_keepInScreenBounds(true);
            childWindow.set_centerIfModal(true);
            childWindow.set_reloadOnShow(true);
            childWindow.center();
            childWindow.remove_close(OnClientCloseFindProvider);
            childWindow.add_close(OnClientCloseFindPatient);
        }, 0);
    return false;
}


function btnPrintToPDF_Click(sender, args) {
    var obj = new Array();
    obj.push("Location=" + "DYNAMIC");
    var result = openModal("frmPrintPDF.aspx", 750, 900, obj, "OrderManagementWindow");
}

function ExamPhotos(Submit_Id, CurrentProcess) {

    var obj = new Array();
    obj.push("type=Result Upload");
    obj.push("CurrentProcess=" + CurrentProcess);
    obj.push("OrderSubmit_ID=" + Submit_Id);
    obj.push("IsCmg=IsCmg");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmExamPhotos.aspx', 400, 850, obj, "OrderManagementWindow"); }, 0);
}

function ViewResults(Human_ID, order_submit_id, enc_id, Physician_ID, lab_id) {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var vhdnCurrentProcess = "";
    if (document.getElementById("hdnCurrentProcess") != null) {
        vhdnCurrentProcess = document.getElementById("hdnCurrentProcess").value;
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
    $(top.window.document).find("#TabViewResultdlg")[0].style.width = "90%";
    $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
    $(top.window.document).find("#TabViewResultdlg").css("margin-top", "1%");
    var sPath = "frmViewResult.aspx?HumanID=" + Human_ID + "&OrderSubmitId=" + order_submit_id + "&EncounterId=" + enc_id + "&PhysicianId=" + Physician_ID + "&LabId=" + lab_id + "&Screen=OrderManagement" + "&CurrentProcess=" + vhdnCurrentProcess + "&Opening_from=OrderManagementScreen&File_Ref_ID=" + order_submit_id;//BugID:43099
    $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
    $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
    });
    return false;
}

function ResultsNew() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    var hdnCurrentProc = document.getElementById("hdnCurrentProc").value;
    var hdnOrderSubId = document.getElementById("hdnOrderSubId").value;
    var hdnIndexOrderID = document.getElementById("hdnIndexOrderID").value;
    var hdnElectronicSign = document.getElementById("hdnElectronicSign").value;

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find("#TabAddResult").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabAddResultTitle")[0].textContent = "Upload Photos";
    $(top.window.document).find("#TabAddResultdlg")[0].style.width = "60%";
    $(top.window.document).find("#TabAddResultdlg")[0].style.height = "50%";
    $(top.window.document).find("#TabAddResultdlg").css("margin-top", "9%");
    var sPath = "frmExamPhotos.aspx?type=Result Upload" + "&CurrentProcess=" + hdnCurrentProc + "&OrderSubmit_ID=" + hdnOrderSubId + "&hdnOrder=false" + "&IsCmg= IsCmg" + "&IndexOrderID=" + hdnIndexOrderID + "&ElectronicSign=" + hdnElectronicSign;//BugID:43099
    $(top.window.document).find("#TabAddResultFrame")[0].style.height = "100%";
    $(top.window.document).find("#TabAddResultFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabAddResult").one("hidden.bs.modal", function (e) {
    });
}


function RadWindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}

function Clear() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        var FromDate = $find("dtpFromDate");
        var ToDate = $find("dtpToDate");
        FromDate.clear();
        ToDate.clear();
        document.getElementById("btnClear").click();


    }
    else if (IsClearAll == false) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

}
function Load() {
    GetUTCTime();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var cboOrderType = document.getElementById('cboOrderType').value;
    var chkDate = document.getElementById("chkDate").checked;
    var dtpFromDate = document.getElementById("dtpFromDate");
    var dtpToDate = document.getElementById("dtpToDate");
    var cboLabCenter = document.getElementById("cboLabCenter").value;
    var txtPatientName = document.getElementById('txtPatientName');
    var hdnPatientVal = document.getElementById('hdnPatientValues').value;
    var txtProviderName = document.getElementById('txtProviderName')
    var hdnTransferVal = document.getElementById('hdnTransferVaraible').value
    var cboFacilityName = document.getElementById('cboFacilityName').value;
    var cboOrderStatus = document.getElementById('cboOrderStatus').value;
    var dtpFromDateNew = $find("dtpFromDate");
    var dtpToDateNew = $find("dtpToDate");
    var cbolabItem = 0;
    if ($find("cboLabCenter").get_selectedItem()!= null) {
        cbolabItem = $find("cboLabCenter").get_selectedItem().get_value();
    }
    
    var FacilityName;
    setTimeout(
        function () {
            document.getElementById('btnAddResult').disabled = true;
        }, 2);

    if (chkDate) {
        dtpFromDate.disabled = false;
        dtpToDate.disabled = false;

        if (dtpFromDate.value == '') {
            DisplayErrorMessage('7090011');
            return false;
        }
        if (dtpToDate.value == '') {
            DisplayErrorMessage('7090012')
            return false;
        }

    }
    document.getElementById("btnAddResult").disabled = false;
    if (chkDate && dtpFromDate.value > dtpToDate.value) {
        DisplayErrorMessage('7090008');
        return false;
    }

    if (txtPatientName.value == '') {
        txtPatientName.setAttribute('tagPatientName', '');
    }
    else {
        if (hdnPatientVal != undefined && hdnPatientVal != '') {
            txtPatientName.setAttribute('tagPatientName', hdnPatientVal);
        }
    }

    if (txtProviderName.value == '') {
        txtProviderName.setAttribute('tagProviderName', '');
    }
    else {
        if (hdnTransferVal != undefined && hdnTransferVal != '') {
            txtProviderName.setAttribute('tagProviderName', hdnTransferVal);
        }
    }

    if (cboFacilityName != "ALL") {
        FacilityName = cboFacilityName;
    }
    else {
        FacilityName = '%';
    }


    if (cboOrderType != undefined && (cboOrderType == "DIAGNOSTIC ORDER" || cboOrderType == "DME ORDER")) {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 0px;width:96.7%;position: sticky;top: 0;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: left;width:6%'>Control#</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>View Result</th>
    <th style='border: 1px solid #909090; display: none;'>Encounter_ID</th>
    <th style='border: 1px solid #909090; display: none;'>Physician_ID</th>
    <th style='border: 1px solid #909090; display: none;'>Lab_ID</th>
    <th style='border: 1px solid #909090; display: none;'>IsElectronic Signature</th>
    <th style='border: 1px solid #909090; display: none;'>Index Order ID</th>
    </tr>
    </thead>
</table>`);

        data = JSON.stringify({
            "cboOrderType": cboOrderType,
            "cboOrderStatus": cboOrderStatus,
            "sFacilityName": FacilityName,
            "fromdate": dtpFromDateNew.get_dateInput()._text,
            "todate": dtpToDateNew.get_dateInput()._text,
            "sHumanID": hdnPatientVal,
            "tagProviderName": hdnTransferVal,
            "labItem": cbolabItem

        });
        var dataTable = new DataTable('#EncounterTable', {
            serverSide: false,
            lengthChange: false,
            searching: true,
            processing: false,
            ordering: true,
            autoWidth: false,
            scrollCollapse: true,
            scrollY: '180px',
            order: [],
            pageLength: 15,
            language: {
                search: "",
                searchPlaceholder: "Search by Name or Acct. # or Procedure",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top


            ajax: {
                url: '/frmOrderManagement.aspx/LoadDiagnostic',
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                deferRender: true,
                data: function (d) {
                    d.extra_search = data;
                    return d;
                },
                dataSrc: function (json) {
                    //var objdata = json.d;
                    var objdata = json.d;   
                    objdata.data = Decompress(objdata.data);
                    json.data = objdata.data;                   
                    setTimeout(
                        function () {
                            $(".dataTables_scrollBody").find(".header").hide();
                        }, 10);
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
                    data: 'Group_ID', render: function (data, type, row) {
                        if (row.Group_ID != "")
                            return "ACUR" + row.Group_ID;
                        else
                            return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                {
                    data: 'Ordered_Date_And_Time', render: function (data, type, row) {
                        if (row.Ordered_Date_And_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Ordered_Date_And_Time.replace("T", " ")).split(' ')[0];
                    }, sClass: 'TableCellBorder', searchable: false,
                    type: 'date'
                },
                {
                    date: '', render: function (data, type, row) {
                        return cboOrderType;
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Current_Process', sClass: 'TableCellBorder', searchable: false },
                { data: 'Human_Id', sClass: 'TableCellBorder' },
                { data: 'Human_Name', sClass: 'TableCellBorder' },
                {
                    data: 'Date_Of_Birth', render: function (data, type, row) {
                        return DOBConvert(data.replace("T00:00:00", ""))
                    }, sClass: 'TableCellBorder', searchable: false,
                    type: 'date'

                },
                { data: 'Procedures', sClass: 'TableCellBorder word-break-all' },
                { data: 'Assessment', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'Ordering_Provider', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder', searchable: false
                },
                { data: 'Facility_Name', sClass: 'TableCellBorder', searchable: false },
                { data: 'Lab_Name', sClass: 'TableCellBorder', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "<td style='width:2%'><input type = 'image' name='grdPrintEReq' id='grdPrintEReq' src = '/Resources/PrintReq.png' class= 'loaderClass'  style = 'border-width:0px;'/></td>";
                    }, sClass: 'TableCellBorder text-align-center', searchable: false
                },
                {
                    data: '', render: function (data, type, row) {
                        return "<td style='width:2%'><input type = 'image' name='grdViewResult' id ='grdViewResult' src = '/Resources/Down.bmp' class= 'loaderClass'  style = 'border-width:0px;'/></td>";
                    }, sClass: 'TableCellBorder text-align-center', searchable: false
                },
                { data: 'Encounter_ID', sClass: "hide_column", searchable: false },
                { data: 'Physician_ID', sClass: "hide_column", searchable: false },
                { data: 'Lab_ID', sClass: "hide_column", searchable: false },
                { data: 'Is_Electronic_Result_Available', sClass: "hide_column", searchable: false },
                { data: 'File_Management_Index_Order_ID', sClass: "hide_column", searchable: false },
            ],
            initComplete: function (settings, json) {
                $("#EncounterTable_filter input")[0].classList.add('searchicon');
            }
        });
        $('#EncounterTable_filter').css({
            'float': 'left',
            'text-align': 'left',
            'margin-left': '30px',
        });

        $('#EncounterTable_info').css({
            'min-width': '180px'
        });

        $(".dataTables_scrollHeadInner").find(".header").on('click', function () {
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });

        dataTable.on('search.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            $('.myQChkbx').prop('checked', false);
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('page.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        setTimeout(
            function () {
                $(".dataTables_scrollBody").find(".header").hide();
            }, 10);
        dataTable.on('click', 'tr', function () {
            $('#EncounterTable tr').removeClass("odd");
            $('#EncounterTable tr').removeClass("even");
            dataTable.$('tr.highlight').removeClass('highlight');
            $(this)[0].classList.add('highlight');

            var data = dataTable.row($(this)).data();
            if (data.Lab_ID != undefined && data.Lab_ID == "32" && data.Current_Process != undefined && data.Current_Process.toUpperCase() != "BILLING_WAIT" && data.Current_Process.toUpperCase() != "RESULT_REVIEW") {

                document.getElementById('btnAddResult').disabled = false;
                document.getElementById('hdnOrderSubId').value = data.Group_ID;
                document.getElementById('hdnCurrentProc').value = data.Current_Process;
                document.getElementById('hdnIndexOrderID').value = data.File_Management_Index_Order_ID;
                document.getElementById('hdnElectronicSign').value = data.Is_Electronic_Result_Available;

                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "False",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });

            }
            else {
                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "True",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }

        });

        dataTable.on("click", '#grdViewResult', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            var dataval = dataTable.row($(this).parents('tr')).data();
            if (dataval.File_Management_Index_Order_ID == "" || dataval.File_Management_Index_Order_ID == 0 && dataval.Is_Electronic_Result_Available == "False") {
                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadViewResult",
                    data: JSON.stringify({
                        "sIdCheck": "False",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        DisplayErrorMessage('7090010');
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadViewResult",
                    data: JSON.stringify({
                        "sIdCheck": "True",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        var Human_ID = dataval.Human_Id;
                        var order_submit_id = dataval.Group_ID;
                        var enc_id = dataval.Encounter_ID;
                        var Physician_ID = dataval.Physician_ID;
                        var lab_id = dataval.Lab_ID;
                        ViewResults(Human_ID, order_submit_id, enc_id, Physician_ID, lab_id);
                        return false;
                    }, error: function (xhr, error, code) {
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
                });
            }
            return false;
        });
        dataTable.on("click", '#grdPrintEReq', function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var now = new Date();
            var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();

            dataTable.$('tr.highlight').removeClass('highlight');
            var dataval = dataTable.row($(this).parents('tr')).data();

            setTimeout(
                function () {
                    $.ajax({
                        type: "POST",
                        url: "./frmOrderManagement.aspx/Loadprintreq",
                        data: JSON.stringify({
                            "cboOrderType": cboOrderType,
                            "hdnRefPhar": dataval.Lab_Name,
                            "hdnLabID": dataval.Lab_ID,
                            "hdnICd": dataval.Assessment,
                            "hdnControl": dataval.Group_ID,
                            "hdnEncounterID": dataval.Encounter_ID,
                            "hdnProRXReason": dataval.Procedures,
                            "hdnPatientACC": dataval.Human_Id,
                            "hdnPhysicianID": dataval.Physician_ID,
                            "hdnLocalTime": utc
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var result = $.parseJSON(data.d);
                            if (result.split("~")[0] == "OpenPDFImage") {
                                document.getElementById('SelectedItem').value = result.split("~")[1]
                                OpenPDF();
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }
                            else {
                                DisplayErrorMessage(result);
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }
                        }, error: function (xhr, error, code) {
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

                    });
                }, 1);
            return false;
        });
        
    }
    else if (cboOrderType != undefined && cboOrderType == "IMMUNIZATION ORDER") {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 0px;width:96.7%;position: sticky;top: 0;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>
    <th style='border: 1px solid #909090; display: none;'>Group_ID</th>
    </tr>
    </thead>
</table>`);

        data = JSON.stringify({
            "cboOrderType": cboOrderType,
            "cboOrderStatus": cboOrderStatus,
            "sFacilityName": FacilityName,
            "fromdate": dtpFromDateNew.get_dateInput()._text,
            "todate": dtpToDateNew.get_dateInput()._text,
            "sHumanID": hdnPatientVal,
            "tagProviderName": hdnTransferVal,
            "labItem": cbolabItem

        });
        var dataTable = new DataTable('#EncounterTable', {
            serverSide: false,
            lengthChange: false,
            searching: true,
            processing: false,
            ordering: true,
            scrollCollapse: true,
            scrollY: '180px',
            autoWidth: false,
            order: [],
            pageLength: 15,
            language: {
                search: "",
                searchPlaceholder: "Search by Name or Acct. # or Procedure",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top


            ajax: {
                url: '/frmOrderManagement.aspx/LoadImmunization',
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                deferRender: true,
                data: function (d) {
                    d.extra_search = data;
                    return d;
                },
                dataSrc: function (json) {
                    //var objdata = json.d;
                    var objdata = json.d;
                    objdata.data = Decompress(objdata.data);
                    json.data = objdata.data;
                    setTimeout(
                        function () {
                            $(".dataTables_scrollBody").find(".header").hide();
                        }, 10);
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
                    data: 'Ordered_Date_And_Time', render: function (data, type, row) {
                        if (row.Ordered_Date_And_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Ordered_Date_And_Time.replace("T", " ")).split(' ')[0];
                    }, searchable: false, sClass: 'TableCellBorder word-break-all',
                    type: 'date'
                },
                {
                    date: '', render: function (data, type, row) {
                        return cboOrderType;
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Current_Process', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'Human_Id', sClass: 'TableCellBorder word-break-all' },
                { data: 'Human_Name', sClass: 'TableCellBorder word-break-all' },
                {
                    data: 'Date_Of_Birth', render: function (data, type, row) {
                        return DOBConvert(data.replace("T00:00:00", ""))
                    }, searchable: false, sClass: 'TableCellBorder word-break-all',
                    type: 'date'
                },
                { data: 'Procedures', sClass: 'TableCellBorder word-break-all' },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Ordering_Provider', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Facility_Name', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                {
                    data: '', render: function (data, type, row) {
                        return "<td style='width:2%'><input type = 'image' name = 'grdPrintEReq' id = 'grdPrintEReq' src = '/Resources/PrintReq.png' class= 'loaderClass'  style = 'border-width:0px;'/></td>";
                    }, sClass: 'TableCellBorder word-break-all text-align-center', searchable: false
                },
                { data: 'Group_ID', sClass: "hide_column", searchable: false },
            ],
            initComplete: function (settings, json) {
                $("#EncounterTable_filter input")[0].classList.add('searchicon');
            }
        });
        $('#EncounterTable_filter').css({
            'float': 'left',
            'text-align': 'left',
            'margin-left': '30px',
        });

        $('#EncounterTable_info').css({
            'min-width': '180px'
        });
        $(".dataTables_scrollHeadInner").find(".header").on('click', function () {
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('search.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            $('.myQChkbx').prop('checked', false);
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('page.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        setTimeout(
            function () {
                $(".dataTables_scrollBody").find(".header").hide();
            }, 10);
        dataTable.on('click', 'tr', function () {
            $('#EncounterTable tr').removeClass("odd");
            $('#EncounterTable tr').removeClass("even");
            dataTable.$('tr.highlight').removeClass('highlight');
            $(this)[0].classList.add('highlight');

            if (data.Lab_ID != undefined && data.Lab_ID == "32" && data.Current_Process != undefined && data.Current_Process.ToUpper() != "BILLING_WAIT" && data.Current_Process.ToUpper() != "RESULT_REVIEW") {

                document.getElementById('btnAddResult').disabled = true;
                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "False",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }
            else {
                document.getElementById('btnAddResult').disabled = false;
                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "True",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }
        });

        dataTable.on("click", '#grdPrintEReq', function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var now = new Date();
            var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();

            dataTable.$('tr.highlight').removeClass('highlight');
            var dataval = dataTable.row($(this).parents('tr')).data();
            setTimeout(
                function () {
                    $.ajax({
                        type: "POST",
                        url: "./frmOrderManagement.aspx/Loadprintreq",
                        data: JSON.stringify({
                            "cboOrderType": cboOrderType,
                            "hdnRefPhar": "",
                            "hdnLabID": "",
                            "hdnICd": "",
                            "hdnControl": "",
                            "hdnEncounterID": dataval.Group_ID,
                            "hdnProRXReason": dataval.Procedures,
                            "hdnPatientACC": dataval.Human_Id,
                            "hdnPhysicianID": "",
                            "hdnLocalTime": utc
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var result = $.parseJSON(data.d);
                            if (result.split("~")[0] == "OpenPDFImage") {
                                document.getElementById('SelectedItem').value = result.split("~")[1]
                                OpenPDF();
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }
                            else {
                                DisplayErrorMessage(result);
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }

                        }, error: function (xhr, error, code) {
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

                    });
                }, 1);
            return false;
        });
        
    }
    else if (cboOrderType != undefined && cboOrderType == "REFERRAL ORDER") {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 0px;width:96.7%;position: sticky;top: 0;'>
     <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>  
    <th style='border: 1px solid #909090; display: none;'>Group_ID</th>
    </tr>
    </thead>
</table>`);

        data = JSON.stringify({
            "cboOrderType": cboOrderType,
            "cboOrderStatus": cboOrderStatus,
            "sFacilityName": FacilityName,
            "fromdate": dtpFromDateNew.get_dateInput()._text,
            "todate": dtpToDateNew.get_dateInput()._text,
            "sHumanID": hdnPatientVal,
            "tagProviderName": hdnTransferVal,
            "labItem": cbolabItem

        });
        var dataTable = new DataTable('#EncounterTable', {
            serverSide: false,
            lengthChange: false,
            searching: true,
            processing: false,
            ordering: true,
            scrollCollapse: true,
            scrollY: '180px',
            autoWidth: false,
            order: [],
            pageLength: 15,
            language: {
                search: "",
                searchPlaceholder: "Search by Name or Acct. # or Procedure",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top


            ajax: {
                url: '/frmOrderManagement.aspx/LoadReferral',
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                deferRender: true,
                data: function (d) {
                    d.extra_search = data;
                    return d;
                },
                dataSrc: function (json) {
                    //var objdata = json.d;
                    var objdata = json.d;
                    objdata.data = Decompress(objdata.data);
                    json.data = objdata.data;
                    setTimeout(
                        function () {
                            $(".dataTables_scrollBody").find(".header").hide();
                        }, 10);
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
                    data: 'Ordered_Date_And_Time', render: function (data, type, row) {
                        if (row.Ordered_Date_And_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Ordered_Date_And_Time.replace("T", " ")).split(' ')[0];
                    }, searchable: false, sClass: 'TableCellBorder word-break-all',
                    type: 'date'
                },
                {
                    date: '', render: function (data, type, row) {
                        return cboOrderType;
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Current_Process', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'Human_Id', sClass: 'TableCellBorder word-break-all' },
                { data: 'Human_Name', sClass: 'TableCellBorder word-break-all' },
                {
                    data: 'Date_Of_Birth', render: function (data, type, row) {
                        return DOBConvert(data.replace("T00:00:00", ""))
                    }, searchable: false, sClass: 'TableCellBorder word-break-all',
                    type: 'date'
                },
                { data: 'Procedures', sClass: 'TableCellBorder word-break-all' },
                { data: 'Assessment', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'Ordering_Provider', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'To_Physician_Name', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'To_Facility_Name', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "<td style='width:2%'><input type = 'image' name = 'grdPrintEReq' id = 'grdPrintEReq' src = '/Resources/PrintReq.png' class= 'loaderClass'  style = 'border-width:0px;'/></td>";
                    }, sClass: 'TableCellBorder word-break-all text-align-center', searchable: false
                },
                { data: 'Group_ID', sClass: "hide_column", searchable: false },
            ],
            initComplete: function (settings, json) {
                $("#EncounterTable_filter input")[0].classList.add('searchicon');
            }
        });
        $('#EncounterTable_filter').css({
            'float': 'left',
            'text-align': 'left',
            'margin-left': '30px',
        });

        $('#EncounterTable_info').css({
            'min-width': '180px'
        });
        $(".dataTables_scrollHeadInner").find(".header").on('click', function () {
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('search.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            $('.myQChkbx').prop('checked', false);
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('page.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        setTimeout(
            function () {
                $(".dataTables_scrollBody").find(".header").hide();
            }, 10);
        dataTable.on('click', 'tr', function () {
            $('#EncounterTable tr').removeClass("odd");
            $('#EncounterTable tr').removeClass("even");
            dataTable.$('tr.highlight').removeClass('highlight');
            $(this)[0].classList.add('highlight');

            if (data.Lab_ID != undefined && data.Lab_ID == "32" && data.Current_Process != undefined && data.Current_Process.ToUpper() != "BILLING_WAIT" && data.Current_Process.ToUpper() != "RESULT_REVIEW") {
                document.getElementById('btnAddResult').disabled = true;
                document.getElementById('hdnOrderSubId').value = data.Group_ID;
                document.getElementById('hdnCurrentProc').value = data.Current_Process;
                document.getElementById('hdnIndexOrderID').value = data.File_Management_Index_Order_ID;
                document.getElementById('hdnElectronicSign').value = data.Is_Electronic_Result_Available;

                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "False",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });

            }
            else {
                document.getElementById('btnAddResult').disabled = false;

                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "True",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }

        });

        dataTable.on("click", '#grdPrintEReq', function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            var now = new Date();
            var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();

            dataTable.$('tr.highlight').removeClass('highlight');
            var dataval = dataTable.row($(this).parents('tr')).data();
            setTimeout(
                function () {
                    $.ajax({
                        type: "POST",
                        url: "./frmOrderManagement.aspx/Loadprintreq",
                        data: JSON.stringify({
                            "cboOrderType": cboOrderType,
                            "hdnRefPhar": dataval.To_Facility_Name,
                            "hdnLabID": "",
                            "hdnICd": dataval.Assessment,
                            "hdnControl": dataval.Group_ID,
                            "hdnEncounterID": dataval.Group_ID,
                            "hdnProRXReason": dataval.Procedures,
                            "hdnPatientACC": dataval.Human_Id,
                            "hdnPhysicianID": "",
                            "hdnLocalTime": utc
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var result = $.parseJSON(data.d);
                            if (result.split("~")[0] == "OpenPDFImage") {
                                document.getElementById('SelectedItem').value = result.split("~")[1]
                                OpenPDF();
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }
                            else {
                                DisplayErrorMessage(result);
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            }

                        }, error: function (xhr, error, code) {
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

                    });
                }, 1);
            return false;
        });
    }
    else if (cboOrderType != undefined && cboOrderType == "PROCEDURES") {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 0px;width:96.7%;position: sticky;top: 0;'>
     <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Ref to Facility/Lab/ Pharmacy</th>
    </tr>
    </thead>
</table>`);

        data = JSON.stringify({
            "cboOrderType": cboOrderType,
            "cboOrderStatus": cboOrderStatus,
            "sFacilityName": FacilityName,
            "fromdate": dtpFromDateNew.get_dateInput()._text,
            "todate": dtpToDateNew.get_dateInput()._text,
            "sHumanID": hdnPatientVal,
            "tagProviderName": hdnTransferVal,
            "labItem": cbolabItem

        });
        var dataTable = new DataTable('#EncounterTable', {
            serverSide: false,
            lengthChange: false,
            searching: true,
            processing: false,
            ordering: true,
            scrollCollapse: true,
            scrollY: '180px',
            autoWidth: false,
            order: [],
            pageLength: 15,
            language: {
                search: "",
                searchPlaceholder: "Search by Name or Acct. # or Procedure",
                infoFiltered: ""
            },
            dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top


            ajax: {
                url: '/frmOrderManagement.aspx/LoadProcedure',
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                deferRender: true,
                data: function (d) {
                    d.extra_search = data;
                    return d;
                },
                dataSrc: function (json) {
                    //var objdata = json.d;
                    var objdata = json.d;
                    objdata.data = Decompress(objdata.data);
                    json.data = objdata.data;
                    setTimeout(
                        function () {
                            $(".dataTables_scrollBody").find(".header").hide();
                        }, 10);
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
                    data: 'Ordered_Date_And_Time', render: function (data, type, row) {
                        if (row.Ordered_Date_And_Time == "0001-01-01T00:00:00")
                            return "";
                        else
                            return ConvertDate(row.Ordered_Date_And_Time.replace("T", " ")).split(' ')[0];
                    }, searchable: false, sClass: 'TableCellBorder word-break-all',
                    type: 'date'
                },
                {
                    date: '', render: function (data, type, row) {
                        return cboOrderType;
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Current_Process', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'Human_Id', sClass: 'TableCellBorder word-break-all' },
                { data: 'Human_Name', sClass: 'TableCellBorder word-break-all' },
                {
                    data: 'Date_Of_Birth', render: function (data, type, row) {
                        return DOBConvert(data.replace("T00:00:00", ""))
                    }, searchable: false, sClass: 'TableCellBorder word-break-all',
                    type: 'date'
                },
                { data: 'Procedures', sClass: 'TableCellBorder word-break-all' },
                { data: 'Assessment', sClass: 'TableCellBorder word-break-all', searchable: false },
                { data: 'To_Physician_Name', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
                { data: 'Facility_Name', sClass: 'TableCellBorder word-break-all', searchable: false },
                {
                    data: '', render: function (data, type, row) {
                        return "";
                    }, sClass: 'TableCellBorder word-break-all', searchable: false
                },
            ],
            initComplete: function (settings, json) {
                $("#EncounterTable_filter input")[0].classList.add('searchicon');
            }
        });
        $('#EncounterTable_filter').css({
            'float': 'left',
            'text-align': 'left',
            'margin-left': '30px',
        });

        $('#EncounterTable_info').css({
            'min-width': '180px'
        });
        $(".dataTables_scrollHeadInner").find(".header").on('click', function () {
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('search.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            $('.myQChkbx').prop('checked', false);
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        dataTable.on('page.dt', function () {
            dataTable.$('tr.highlight').removeClass('highlight');
            setTimeout(
                function () {
                    $(".dataTables_scrollBody").find(".header").hide();
                }, 10);
        });
        setTimeout(
            function () {
                $(".dataTables_scrollBody").find(".header").hide();
            }, 10);
        dataTable.on('click', 'tr', function () {
            $('#EncounterTable tr').removeClass("odd");
            $('#EncounterTable tr').removeClass("even");
            dataTable.$('tr.highlight').removeClass('highlight');
            $(this)[0].classList.add('highlight');

            if (data.Lab_ID != undefined && data.Lab_ID == "32" && data.Current_Process != undefined && data.Current_Process.ToUpper() != "BILLING_WAIT" && data.Current_Process.ToUpper() != "RESULT_REVIEW") {
                document.getElementById('btnAddResult').disabled = true;
                document.getElementById('hdnOrderSubId').value = data.Group_ID;
                document.getElementById('hdnCurrentProc').value = data.Current_Process;
                document.getElementById('hdnIndexOrderID').value = data.File_Management_Index_Order_ID;
                document.getElementById('hdnElectronicSign').value = data.Is_Electronic_Result_Available;

                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "False",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }
            else {
                document.getElementById('btnAddResult').disabled = false;
                $.ajax({
                    type: "POST",
                    url: "./frmOrderManagement.aspx/LoadImportResult",
                    data: JSON.stringify({
                        "sIdCheck": "True",
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var result = $.parseJSON(data.d);
                        return false;
                    }, error: function (xhr, error, code) {
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

                });
            }

        });
    }
   

    return false;
}

function PatientClear() {
    document.getElementById("txtPatientName").value = "";
    document.getElementById("txtPatientName").removeAttribute("tagPatientName");
    return false;
}
function ProviderClear() {
    document.getElementById("txtProviderName").value = '';
    return false;
}
function OnClientCloseFindProvider(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined) {
        document.getElementById("txtProviderName").value = arg.sPhyName;
        document.getElementById("hdnTransferVaraible").value = arg.ulPhyId;
    }
}
function OnClientCloseFindPatient(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined) {
        document.getElementById("txtPatientName").value = arg.PatientName;
        document.getElementById("hdnPatientValues").value = arg.HumanId;
    }
}
function OnRowSelected(sender, eventArgs) {
    var btnAdd = document.getElementById("btnAddResult");
    var rowindex = eventArgs.get_itemIndexHierarchical();
    var value16 = eventArgs.get_gridDataItem()._element.cells[14].innerHTML;
    var value5 = eventArgs.get_gridDataItem()._element.cells[3].innerHTML;
    if (eventArgs.get_gridDataItem()._element.cells[14].innerHTML != '' && eventArgs.get_gridDataItem()._element.cells[14].innerHTML == '32' && eventArgs.get_gridDataItem()._element.cells[3].innerHTML != "BILLING_WAIT" && eventArgs.get_gridDataItem()._element.cells[3].innerHTML != "RESULT_REVIEW") {
        btnAdd.disabled = true;
    }
    else {
        btnAdd.disabled = false;
    }
}
function CheckedChange() {
    var Chk = document.getElementById("chkDate");
    if (Chk.checked == true) {
        var FromDate = $find("dtpFromDate");
        var ToDate = $find("dtpToDate");
        var Currentdate = new Date();
        var sval = Currentdate.format("dd-MMM-yyyy");

        FromDate.get_dateInput().set_value(sval);
        ToDate.get_dateInput().set_value(sval);

        FromDate.set_enabled(true);
        ToDate.set_enabled(true);
    }
    else {
        var FromDate = $find("dtpFromDate");
        var ToDate = $find("dtpToDate");
        FromDate.clear();
        ToDate.clear();
        FromDate._dateInput._displayText = null;
        ToDate._dateInput._displayText = null;
        FromDate.set_enabled(false);
        ToDate.set_enabled(false);
    }
}
function OpenPDFImage() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var obj = new Array();
    obj.push("SI=" + document.getElementById('hdnSelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 720, 900, obj, "OrderManagementWindow"); }, 0);
}
function OpenPDF() {
    var obj = new Array();
    obj.push("SI=" + document.getElementById('SelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 720, 900, obj, "OrderManagementWindow"); }, 0);
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function btnExcelClick() {
    GetUTCTime();
    //var grid = $find("grdReport");
    //var MasterTable = grid.get_masterTableView();
    //var Rows = MasterTable.get_dataItems();
    var table = new DataTable('#EncounterTable');
    if (table.row($('#ResultTableNew tr')).length == 0) {
        DisplayErrorMessage('7090007');
        return false;
    }
    else {
        return true;

    }
}
function PrintToPDF(sender, args) {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var cboOrderType = document.getElementById('cboOrderType').value;
    var cboOrderStatus = document.getElementById('cboOrderStatus').value;
    var dtpFromDate = document.getElementById('dtpFromDate').value;
    var dtpToDate = document.getElementById('dtpToDate').value;
    var cboFacilityName = document.getElementById('cboFacilityName').value;
    var txtPatientName = document.getElementById('txtPatientName').value;
    var txtProviderName = document.getElementById('txtProviderName').value;
    var cboLabCenter = document.getElementById('cboLabCenter').value;
    var chkDate = document.getElementById('chkDate').checked;

    var table = new DataTable('#EncounterTable');
    if (table.row($('#ResultTableNew tr')).length == 0) {
        DisplayErrorMessage('7090007');
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: "./frmOrderManagement.aspx/LoadPrintToPDF",
            data: JSON.stringify({
                "cboOrderType": cboOrderType,
                "hdnLocalTime": utc,
                "cboOrderStatus": cboOrderStatus,
                "dtpFromDate": dtpFromDate,
                "dtpToDate": dtpToDate,
                "cboFacilityName": cboFacilityName,
                "txtPatientName": txtPatientName,
                "txtProviderName": txtProviderName,
                "cboLabCenter": cboLabCenter,
                "chkDate": chkDate
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                var result = $.parseJSON(data.d);
                if (result.split("~")[0] = "OpenPDFImage") {
                    document.getElementById('SelectedItem').value = result.split("~")[1]
                    OpenPDF();
                }
                else {
                    //DisplayErrorMessage(result);
                    return false;
                }

            }, error: function (xhr, error, code) {
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

        });
    }
    //sender.set_autoPostBack(false);
    //set_cancel(true);
}

function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}

function grdReport_OnRowClick(sender, args) {

    var lblResult = document.getElementById('lblResultsFound');
    document.getElementById("hdnResultsLabel").value = lblResult.textContent;
}
function CloseImportResult() {
    document.getElementById("btnSearch").click();
}
function OrderMgmtLoad() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    return false;
}


function cboOrderType_SelectedIndexChanged(sender, args) {
    //Add JavaScript handler code here
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var cboOrderType = document.getElementById('cboOrderType').value;

    if (cboOrderType != undefined && (cboOrderType == "DIAGNOSTIC ORDER" || cboOrderType == "DME ORDER")) {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Control#</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>View Result</th>
    <th style='border: 1px solid #909090; display: none;'>Encounter_ID</th>
    <th style='border: 1px solid #909090; display: none;'>Physician_ID</th>
    <th style='border: 1px solid #909090; display: none;'>Lab_ID</th>
    <th style='border: 1px solid #909090; display: none;'>IsElectronic Signature</th>
    <th style='border: 1px solid #909090; display: none;'>Index Order ID</th>
    </tr>
    </thead>
</table>`);
    }
    else if (cboOrderType != undefined && cboOrderType == "IMMUNIZATION ORDER") {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>
    <th style='border: 1px solid #909090; display: none;'>Group_ID</th>
    </tr>
    </thead>
</table>`);
    }
    else if (cboOrderType != undefined && cboOrderType == "REFERRAL ORDER") {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
     <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ref to Facility/Lab/ Pharmacy</th>
    <th style='border: 1px solid #909090;text-align: center;width:3%'>Print Req.</th>  
    <th style='border: 1px solid #909090; display: none;'>Group_ID</th>
    </tr>
    </thead>
</table>`);
    }
    else if (cboOrderType != undefined && cboOrderType == "PROCEDURES") {
        $("#ResultTableNew").empty();
        $("#ResultTableNew").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
     <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Arrival Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Order Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Order Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Procedure/Rx/ Reason for referral</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>ICD</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ordering Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Attending Provider</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Ordering Facility</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Ref to Facility/Lab/ Pharmacy</th>
    </tr>
    </thead>
</table>`);
    }


    var combo = window.$find("cboOrderStatus");
   // combo.trackChanges();
    for (var i = 0; i < combo.get_items().get_count();) {
        combo.get_items().remove(combo.get_items().getItem(i));
    }
    combo.commitChanges();

    var combo = window.$find("cboLabCenter");
    //combo.trackChanges();
    for (var i = 0; i < combo.get_items().get_count();) {
        combo.get_items().remove(combo.get_items().getItem(i));
    }
    combo.commitChanges();

    $.ajax({
        type: "POST",
        url: "./frmOrderManagement.aspx/LoadcboOrderType",
        data: JSON.stringify({
            "cboOrderType": cboOrderType,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var result = $.parseJSON(data.d);

            var combo = $find("cboOrderStatus");
            var comboItem = new Telerik.Web.UI.RadComboBoxItem();
            comboItem.set_text("");
            combo.get_items().add(comboItem);
            combo.commitChanges();

            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    var combo = $find("cboOrderStatus");
                    var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                    comboItem.set_text(result[i]);
                    combo.get_items().add(comboItem);
                    combo.commitChanges();
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }


        }, error: function (xhr, error, code) {
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

    });

  

    if (cboOrderType == "DIAGNOSTIC ORDER" || cboOrderType == "DME ORDER") {
        $find("cboLabCenter").enable()
        $.get("ConfigXML/LabList.json", {}, function (jsonobject) {
            var LabList = null;
            var combo = $find("cboLabCenter");
            var comboItem = new Telerik.Web.UI.RadComboBoxItem();
            comboItem.set_text("");
            comboItem.set_value("");
            combo.get_items().add(comboItem);
            combo.commitChanges();  
            if (jsonobject != null) {
                for (var l = 0; l < jsonobject.Lab.length; l++) {  
                    var combo = $find("cboLabCenter");
                    var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                    comboItem.set_text(jsonobject.Lab[l].name);
                    comboItem.set_value(jsonobject.Lab[l].id);
                    combo.get_items().add(comboItem);
                    combo.commitChanges();                
                }
            }
        });
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    }
    else {
        $find("cboLabCenter").disable()
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }


}

function LoadIconPatient() {
    document.getElementById('txtPatientName').value = "";
    document.getElementById('hdnPatientValues').value = "";
}
function LoadIconProvider() {
    document.getElementById('txtProviderName').value = "";
    document.getElementById('hdnTransferVaraible').value = "";
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
function DOBConvert(DOB) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
}