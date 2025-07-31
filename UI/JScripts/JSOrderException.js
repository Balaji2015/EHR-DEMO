//Jira Cap - 3206
var grdUnassignedSelected = false;
var ReasonCode = "";
var GridToSelect = true;
var GridOutstandingSelect = true;
var OrderSubmitID = 0;
var ResultMasterID = 0;
var MatchingPatientID = 0;


function btn_Match_Clicked(sender, eventArgs) { { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } }
function OnMatch() { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } DisplayErrorMessage('7100008'); }


function enableField(ChkValue) {
    var pcontrol = document.getElementById(ChkValue);
    if (pcontrol.checked == true) {
        document.getElementById("chkSearchbyfacility").checked = false;
        document.getElementById("chkShowAllResults").checked = false;
        document.getElementById("pnlSearchbyfacility").disabled = true;
        document.getElementById("cboFacilityList").SelectedIndex = 0;
        document.getElementById("pnlFindbyPatient").enabled = true;
    }
}
    function Result(iResultMasterID)
    {

    var obj=new Array();
obj.push("Result_Master_ID="+iResultMasterID);
    obj.push("Order_ID=0");
    obj.push("strScreenName=ORDER EXCEPTION");
    obj.push("bMovetonextprocess=false");
    //Jira CAP-1144
    //setTimeout(function(){GetRadWindow().BrowserWindow.openModal("frmLabResult.aspx",750,845,obj,"MessageWindow");},0);
    setTimeout(openNonModal("frmLabResult.aspx", 780, 1250, obj), 0);
    return false;
}
      function FindPatient()
     {
         var obj=new Array();
    var result = openModal("frmFindPatient.aspx", 251, 1200, obj, 'MessageWindow');
         var winObj=$find('MessageWindow');
    winObj.add_close(OnClientCloseOrderManagement);
}
     function OnRowSelected(sender, eventArgs) 
     {
    var rowindex = eventArgs.get_itemIndexHierarchical();
       var accno=eventArgs.get_gridDataItem()._element.cells[0].innerHTML;
       var patName=eventArgs.get_gridDataItem()._element.cells[7].innerHTML;
       var DOB=eventArgs.get_gridDataItem()._element.cells[8].innerHTML;
       var txtGen=eventArgs.get_gridDataItem()._element.cells[9].innerHTML;
       if(accno!="&nbsp;")
       {
        $find('txtAccountNumber').set_value(accno);
        $find('txtPatientName').set_value(patName);
        $find('txtDOB').set_value(DOB);
        $find('txtGender').set_value(txtGen);
    }
}
//function OnClientCloseOrderManagement(oWindow, args) {
//    var arg = args.get_argument();
//    if (arg) {
//        document.getElementById(GetClientId("hdnHumanID")).value = arg.HumanId;
//        document.getElementById('InvisibleButton').click();

//    }

//}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function HandleOnCheck() {
    var target = event.target || event.srcElement;
    var chkLst = document.getElementById('chklPhysicianlist').getElementsByTagName('input');
    for (var item = 0; item < chkLst.length; item++) {
        if (target.id != chkLst[item].id)
            chkLst[item].checked = false;
    }
}

function RadWindowClose() {
    //Jira CAP-1144
    //var oWindow = null;
    //  if (window.radWindow)
    //       oWindow = window.radWindow;
    //  else if (window.frameElement.radWindow)
    //       oWindow = window.frameElement.radWindow;
    //  if(oWindow!=null)
    //   oWindow.close();
    window.close();
}

function txtToolTip(txtname) {
    var txtTool = document.getElementById(txtname);
    if (txtTool.value.length > 0) {
        txtTool.title = txtTool.value;
    }
}

function Clear() {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        return true;
    }
    return false;
}

function btnFindPatient_Clicked(sender, args) {
    var obj = new Array();
    var result = openModal("frmFindPatient.aspx", 251, 1200, obj, 'MessageWindow');
    var winObj = $find('MessageWindow');
    winObj.add_close(OnClientCloseOrderManagement);
}

function btnViewPendingOrder_Clicked(sender, args) {
    var obj = new Array();
    var result = openModal("frmOrderManagement.aspx", 650, 1130, obj, 'MessageWindow');
}

function checkRadioButton() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById('SearchClick').click();
}
function check() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function checkOnclcik() {
    $('#OutstandingTable tbody tr').removeClass('highlight');
    //var table = new DataTable('#OutstandingTable');
    //table.$('tr.highlight').removeClass('highlight');
    //$('#OutstandingTable_filter').hide();    
    //$('#OutstandingTable_paginate').hide(); 
    GridOutstandingSelect = true;

}

function deselect() {

    //Jira Cap - 3206
    //var masterTable = $find("grdOutstandingOrders").get_masterTableView();
    //var row = masterTable.get_dataItems();
    //for (var i = 0; i < row.length; i++)
    //{
    //  masterTable.get_dataItems()[i].set_selected(false);
    //}  

}

function btnSearch_Clicked(sender, args) {
    document.getElementById("chkNoOrders").checked = false;

    if ($telerik.findDatePicker("frmDate")._element.value != "" && $telerik.findDatePicker("toDate")._element.value == "") {//BugID:46054
        DisplayErrorMessage('7100015');
        sender.set_autoPostBack(false);
    }
    else {
        //Jira Cap - 3206
        //sender.set_autoPostBack(true);
        var table = new DataTable('#OutstandingTable');
        table.clear().draw();
        table.destroy();

        $find("cboUnmatchProvider").disable();
        $find("cboUnmatchProvider").get_items().getItem(0).select();
        $find("cboUnmatchProvider").set_text("");
        $find("cboUnmatchProvider").set_value("");
        document.getElementById("chkUnmatchedProvider").disabled = true;
        $('#txtPatientSearch').val('');
        $('#txtPatientSearch').prop('disabled', false);
        $find("btnMatchOrders").set_enabled(false);
        Load();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }

}
//Jira Cap - 3206
function btnMatchOrders_Clicked(sender, args) {
    var NPINumbers='';
    var HumanID = document.getElementById("hdnHumanID").value;
    var chkNoOrders = document.getElementById("chkNoOrders").checked;
    Order_Submit_ID = OrderSubmitID;
    Result_Master_ID = ResultMasterID;
    Matching_Patient_ID = MatchingPatientID;

    if (document.getElementById("txtPatientSearch").value == "" && HumanID == 0) {
        DisplayErrorMessage('7100013');
        return false;
    }

    if (grdUnassignedSelected) {

        var combo = window.$find("cboUnmatchProvider");
        var HumanID = document.getElementById("hdnHumanID").value;

        if (combo.get_value() != "") {

            NPINumbers = combo.get_value();
            if (NPINumbers == "") {
                DisplayErrorMessage('7100011');
                return false;
            }
        }
        else if (combo.get_enabled() && (ReasonCode == "ACUR_LAB_05" || ReasonCode == "ACUR_LAB_06")) {
            if (combo.get_text() == "") {
                DisplayErrorMessage('7100012');
                return false;
            }
            if (combo.get_value() == "") {
                DisplayErrorMessage('7100012');
                return false;
            }
            NPINumbers = combo.get_value();


        }
    }
    else {
        DisplayErrorMessage('7100001');
        return false;
    }
    if (!chkNoOrders) {
        if (GridOutstandingSelect == false ) {
            Order_Submit_ID = OrderSubmitID;
            Result_Master_ID = ResultMasterID;
        }
        else {
            DisplayErrorMessage('7100016');
            return false;
        }
    }


    $.ajax({
        type: "POST",
        url: "./frmLabException.aspx/MatchButtonClick",
        data: JSON.stringify({
            "Result_Master_ID": Result_Master_ID,
            "Order_Submit_ID": Order_Submit_ID,
            "chkNoOrders": chkNoOrders,
            "HumanID": HumanID,
            "NPINumbers": NPINumbers,
            "Matching_Patient_ID": Matching_Patient_ID
        }),
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            var objdata = JSON.parse(json.d);
            if (objdata != "") {
                DisplayErrorMessage(objdata);
                var table = new DataTable('#EncounterTable');
                table.rows('.highlight').remove().draw(false);
                document.getElementById("chkUnmatchedProvider").disabled = true
                $find("cboUnmatchProvider").disable();
                $find("cboUnmatchProvider").get_items().getItem(0).select();
                $find("cboUnmatchProvider").set_text("");
                $find("cboUnmatchProvider").set_value("");
                $('#txtPatientSearch').val('');
                $('#txtPatientSearch').prop('disabled', false);
                document.getElementById("hdnHumanID").value = 0;
                $find("btnMatchOrders").set_enabled(false);
                var table = new DataTable('#OutstandingTable');
                table.clear().draw();
                table.destroy();
                document.getElementById('chkNoOrders').checked = false;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

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
    })


    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

}



function grdUnassignedResults_OnRowClick(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}


Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
    initializeAutocomplete();
});

$(document).ready(function () {
    //Jira Cap - 3206
    $find("btnDemographics").set_enabled(false);    
    $("#UnassignedResults").empty();
    $("#UnassignedResults").append(`
    <table id=EncounterTable2 class='table table-bordered Gridbodystyle'  style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Acc # in Result</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Patient Name in Result</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient DOB In Result</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Patient Gender In Result</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Order ID</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Specimen</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Lab Procedure</th>
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Reason for not matching</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Result Message Date And Time</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Provider Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Lab Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Abnormal</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>View</th>
    <th style='border: 1px solid #909090; display: none;'>Order Type</th>
    <th style='border: 1px solid #909090; display: none;'>OrderID</th>
    <th style='border: 1px solid #909090; display: none;'>Lab ID</th>
    <th style='border: 1px solid #909090; display: none;'>Matching Patient ID</th>
    <th style='border: 1px solid #909090; display: none;'>Reason Code</th>
    <th style='border: 1px solid #909090; display: none;'>OrderingNPI</th>
    <th style='border: 1px solid #909090; display: none;'>ResultMasterID</th>
    </tr>
    </thead>
</table>`);


    $("#OutstandingOrders").empty();
    $("#OutstandingOrders").append(`
    <table id=OutstandingTable class='table table-bordered Gridbodystyle'  style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:8%'>Order#</th>
    <th style='border: 1px solid #909090;text-align: center;width:70%'>Order Description(Procedures)</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Lab Name</th>
    <th style='border: 1px solid #909090; display: none;'>Order_Submit_ID</th>
    </tr>
    </thead>
    </table>`);
    initializeAutocomplete();
    chkUnmatchedProvider_CheckedChanged();
    //chkProviderNameChecked();
    document.getElementById("chkUnmatchedProvider").disabled = true
    $find("cboUnmatchProvider").disable();
    $find("btnMatchOrders").set_enabled(false);
});
var intPatientlen = -1;
function initializeAutocomplete() {
    var curleft = curtop = 0;
    var current_element = document.getElementById('txtPatientSearch');

    if (current_element == null) {
        current_element = document.getElementById('txtProviderSearch');
        curtop = 5;
    }
    window.setTimeout(function () {
        $('#txtPatientSearch').focus();
    }, 50);

    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    $("#imgClearPatientText").css({
        "cursor": "pointer",
        "width": "20px",
        "height": "20px"
    }).on("click", function () {
        $('#txtPatientSearch').val('').focus();
        $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
        sessionStorage.setItem("valuepatientsearch", "");
        sessionStorage.setItem("labelpatientsearch", "");
        sessionStorage.setItem("HumanId", "");
        $("#imgClearPatientText").removeClass("disabled");
        $('#txtPatientSearch').prop('disabled', false);
        document.getElementById("hdnHumanID").value = 0;
        var table = new DataTable('#OutstandingTable');
        table.clear().draw();
        table.destroy();
    });
    if ($("#txtPatientSearch").length) {
        $("#txtPatientSearch").autocomplete({
            source: function (request, response) {
                if ($("#txtPatientSearch").val().trim().length > 2) {
                    if (intPatientlen == 0) {
                        UI_Time_Start = new Date();
                        //{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                        StartLoadingImage();
                        this.element.on("keydown", PreventTyping);
                        arrPatient = [];
                        var strkeyWords = $("#txtPatientSearch").val().split(' ');
                        var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                        var account_status = "ACTIVE";
                        var patient_status = "ALIVE";
                        var patient_type = "REGULAR";
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
                                // { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                StopLoadingImage();
                                $("#txtPatientSearch").off("keydown", PreventTyping);
                                var jsonData = $.parseJSON(data.d);
                                if (jsonData.Error != undefined) {
                                    alert(jsonData.Error);
                                    return;
                                }
                                if (jsonData.Time_Taken != undefined)
                                    LogTimeString(jsonData.Time_Taken);
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
                                //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                StopLoadingImage();
                                if (xhr.status == 999)
                                    window.location = xhr.statusText;
                                else {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                                        log.ExceptionType + " \nMessage: " + log.Message);
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
                $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearch').width());
                $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            },
            focus: function () {
                return false;
            }
        }).on("paste", function (e) {
            intPatientlen = -1;
            arrPatient = [];
            $(".ui-autocomplete").hide();
        }).on("input", function (e) {
            $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
            if ($("#txtPatientSearch").val().charAt(e.currentTarget.value.length - 1) == " ") {
                if (e.currentTarget.value.split(" ").length > 2)
                    intPatientlen = intPatientlen + 1;
                else
                    intPatientlen = 0;
            }
            else {
                if ($("#txtPatientSearch").val().length != 0 && intPatientlen != -1) {
                    intPatientlen = intPatientlen + 1;
                }

                if ($("#txtPatientSearch").val().length == 0 || $("#txtPatientSearch").val().indexOf(" ") == -1) {
                    intPatientlen = -1;
                    arrPatient = [];
                    $(".ui-autocomplete").hide();
                }
            }
        })

        $("#txtPatientSearch").data("ui-autocomplete")._renderItem = function (ul, item) {
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
}
function LogTimeString(time_string) {
    UI_Time_Stop = new Date();

    var WS_Time = parseFloat(time_string.split(';')[0].split(':')[1].replace('s', ''));
    var DB_Time = parseFloat(time_string.split(';')[1].split(':')[1].replace('s', ''));
    var UI_Time = ((UI_Time_Stop.getTime() - UI_Time_Start.getTime()) / 1000) - WS_Time - DB_Time;
    console.log(time_string + " UI_Time :" + UI_Time + "s; Total_Time :" + (WS_Time + DB_Time + UI_Time).toString() + "s;");

}

function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}

function PatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
   // $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtPatientSearch = document.getElementById("txtPatientSearch");

    var WSData = {
        HumanID: ui.item.value,
        FullDetails: ui.item.label
    }

    //$.ajax({
    //    type: "POST",
    //    contentType: "application/json; charset=utf-8",
    //    url: "./frmFindPatient.aspx/GetHumanDetails",
    //    data: JSON.stringify(WSData),
    //    dataType: "json",
    //    success: function (data) {
    //        var SelectedPatient = JSON.parse(data.d);
    //        var HumanDetails = SelectedPatient.HumanDetails;
    //        var txtPatientSearch = document.getElementById('txtPatientSearch');
    //        txtPatientSearch.value = SelectedPatient.DisplayString;
    //        txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //        $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
    //    }
    //});

    $.ajax({
        type: "GET",
        url: "./frmLabException.aspx/GetOutstandingOrdersList?HumanId=" + ui.item.value,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (json) {
            var objdata = JSON.parse(json.d);


            const dataval = objdata;


            const grouped = {};

            dataval.forEach(item => {
                const key = `${item.Order_Submit_ID}`;
                if (!grouped[key]) {
                    grouped[key] = {
                        Order_Submit_ID: item.Order_Submit_ID,
                        Created_Date_And_Time: item.Created_Date_And_Time,
                        Internal_Property_Lab_Name: item.Internal_Property_Lab_Name,
                        Lab_Procedure: []
                    };
                }
                grouped[key].Lab_Procedure.push(item.Lab_Procedure + "-" + item.Lab_Procedure_Description);
            });

            const result = Object.values(grouped).map(item => ({
                ...item,
                Lab_Procedure: item.Lab_Procedure.join('; ')
            })).sort((a, b) => b.Order_Submit_ID - a.Order_Submit_ID);

            $("#OutstandingOrders").empty();
            $("#OutstandingOrders").append(`
                            <table id=OutstandingTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
                            <thead class='header' style='border: 1px;width:96.7%;'>
                            <tr class='header' >
                            <th style='border: 1px solid #909090;text-align: center;width:8%'>Order#</th>
                            <th style='border: 1px solid #909090;text-align: center;width:70%'>Order Description(Procedures)</th>
                            <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Date</th>
                            <th style='border: 1px solid #909090;text-align: center;width:6%'>Lab Name</th>
                            <th style='border: 1px solid #909090; display: none;'>Order_Submit_ID</th>
                            </tr>
                            </thead>
                            </table>`);
            $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
                $('#OutstandingTable').DataTable().columns.adjust();
            });

            var dataTable = new DataTable('#OutstandingTable', {
                data: [],
                serverSide: false,
                lengthChange: false,
                processing: false,
                ordering: true,
                scrollCollapse: false,
                scrollY: '145px',
                autoWidth: false,
                paging: false,
                searching: false,
                info: false,
                dom: 'lrt',
                order: [],

                data: result,//objdata[0].OrdersList,                               

                columns: [

                    {
                        data: 'Order_Submit_ID', render: function (data, type, row) {
                            return "ACUR" + row.Order_Submit_ID;
                        },
                        sClass: 'TableCellBorder word-break-all', sWidth: '8%'
                    },
                    { data: 'Lab_Procedure', sClass: 'TableCellBorder word-break-all', sWidth: '70%' },

                    {
                        data: 'Created_Date_And_Time', render: function (data, type, row) {
                            if (row.Created_Date_And_Time == "0001-01-01T00:00:00")
                                return "";
                            else
                                return DOBConvert(row.Created_Date_And_Time.replace("T", " ").split(' ')[0]);
                        }, sClass: 'TableCellBorder', searchable: false, sWidth: '6% !important',
                        type: 'date'
                    },

                    { data: 'Internal_Property_Lab_Name', sClass: 'TableCellBorder', searchable: false, sWidth: '6% !important' },

                    { data: 'Order_Submit_ID', sClass: "hide_column", searchable: false },

                ],

            })


            $('#OutstandingTable tbody').on('click', 'tr', function () {
                $('#OutstandingTable tr').removeClass("odd");
                $('#OutstandingTable tr').removeClass("even");
                dataTable.$('tr.highlight').removeClass('highlight');
                $(this)[0].classList.add('highlight');

                document.getElementById('chkNoOrders').checked = false;

                var data = dataTable.row($(this)).data();
                if (data.Order_Submit_ID != 0) {
                    GridOutstandingSelect = false;
                    OrderSubmitID = data.Order_Submit_ID;
                } else {
                    GridOutstandingSelect = true;
                }

            });

            document.getElementById("chkUnmatchedProvider").enabled = false;

        }
    });


    txtPatientSearch.value = ui.item.label;
    txtPatientSearch.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    document.getElementById('hdnHumanID').value = ui.item.value;
    //$('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
    //debugger;
    //document.getElementById('hdnpatientsearch').value = ui.item.label;
    //document.getElementById('InvisibleButton').click();   
    sessionStorage.setItem("valuepatientsearch", ui.item.value);
    sessionStorage.setItem("labelpatientsearch", ui.item.label);
    //__doPostBack('ctl00$ContentPlaceHolder1$InvisibleButton', '');
   

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    return false;
}
function Filter(array, terms) {
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
function setpatientsearch(sAutosearch) {
    var txtPatientSearch = document.getElementById('txtPatientSearch');
    debugger;
    if (sessionStorage.getItem("labelpatientsearch") != undefined && sessionStorage.getItem("labelpatientsearch") != "") {
        txtPatientSearch.value = sessionStorage.getItem("labelpatientsearch");
        txtPatientSearch.attributes['data-human-id'].value = sessionStorage.getItem("valuepatientsearch");
        if (sAutosearch == "Y") {
            $('#txtPatientSearch').prop('disabled', false);
            //$('#imgClearPatientText').prop('disabled', false);
            $("#imgClearPatientText").removeClass("disabled");
            $('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
            $('#txtPatientSearch').focus()[0].scrollLeft = 0;
        }
    }
    else {
        txtPatientSearch.value = "";
        txtPatientSearch.attributes['data-human-id'].value = "";
        document.getElementById("hdnHumanID").value = 0;
    }
}
function FindPatientenabled(val, sPatientname) {
    debugger;
    $find('btnDemographics').set_enabled(true);
    var txtPatientSearch = document.getElementById('txtPatientSearch');
    debugger;
    if (val == "True") {
        $('#txtPatientSearch').prop('disabled', true);
        //$('#imgClearPatientText').prop('disabled', true);
        $("#imgClearPatientText").addClass("disabled");
        txtPatientSearch.value = sPatientname;
        txtPatientSearch.attributes['data-human-id'].value = sPatientname;
        document.getElementById('hdnHumanID').value = HumanId;
        sessionStorage.setItem("valuepatientsearch", sPatientname);
        sessionStorage.setItem("labelpatientsearch", sPatientname);
    }
    else if (val == "Success") {

        $('#txtPatientSearch').prop('disabled', true);
        $("#imgClearPatientText").addClass("disabled");
        sessionStorage.setItem("valuepatientsearch", "");
        sessionStorage.setItem("labelpatientsearch", "");        
        txtPatientSearch.value = "";
        txtPatientSearch.attributes['data-human-id'].value = "";
        sessionStorage.setItem('StartLoading', 'false');

    }
    else {
        debugger;
        $('#txtPatientSearch').prop('disabled', false);
        //$('#imgClearPatientText').prop('disabled', false);
        $("#imgClearPatientText").removeClass("disabled");
        sessionStorage.setItem("valuepatientsearch", "");
        sessionStorage.setItem("labelpatientsearch", "");       
        txtPatientSearch.value = sPatientname;
        txtPatientSearch.attributes['data-human-id'].value = sPatientname;
        document.getElementById('hdnHumanID').value = HumanId;
    }

}

//Cap - 3217
function btnClose_Clicked() {


    if (document.getElementById("btnMatchOrders") != undefined && document.getElementById("btnMatchOrders").hasAttribute("disabled") == false) {
        localStorage.setItem("bSave", "false");
    }
    else
        localStorage.setItem("bSave", "true");
    if (window.GetRadWindow() != null)
        var winName = window.GetRadWindow()._name;
    localStorage.setItem("bSaveSuccess", "");
    if (localStorage.getItem("bSave") == "false") {
        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
            "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');

        myPos = "center center";
        atPos = 'center center';
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            position: {
                my: myPos,
                at: atPos
            },
            buttons: {
                "Yes": function () {

                    localStorage.setItem("bSaveSuccess", "true");
                    document.getElementById(GetClientId("btnMatchOrders")).click();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    localStorage.setItem("bSave", "true");
                    $(dvdialog).dialog("close");
                    return false;
                },
                "No": function () {
                    localStorage.setItem("bSave", "true");
                    localStorage.setItem("bSaveSuccess", "");
                    $(dvdialog).dialog("close");
                    window.close();
                    return false;
                },
                "Cancel": function () {
                    localStorage.setItem("bSaveSuccess", "");
                    $(dvdialog).dialog("close");
                    localStorage.setItem("bSave", "false");
                    //localStorage.setItem("bSave", "true");
                    return false;
                }
            }
        });
        return false;
    }
    else {
        window.close();
    }

}



function LabExcepLoad() {

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function Load() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $("#UnassignedResults").empty();
    $("#UnassignedResults").append(`
    <table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
    <thead class='header' style='border: 1px;width:96.7%;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Acc # in Result</th>
    <th style='border: 1px solid #909090;word-break: normal;text-align: center;width:7%'>Patient Name in Result</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Patient DOB In Result</th>
    <th style='border: 1px solid #909090;text-align: center;width:4%'>Patient Gender In Result</th>
    <th style='border: 1px solid #909090;word-break: normal;text-align: center;width:4%'>Order ID</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%'>Specimen</th>
    <th style='border: 1px solid #909090;text-align: center;width:16%'>Lab Procedure</th>
    <th style='border: 1px solid #909090;word-break: normal;text-align: center;width:8.5%'>Reason for not matching</th>
    <th style='border: 1px solid #909090;word-break: normal;text-align: center;width:6%'>Result Message Date And Time</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Provider Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%'>Lab Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%'>Abnormal</th>
    <th style='border: 1px solid #909090;text-align: left;width:2%'>View</th>
    <th style='border: 1px solid #909090; display: none;'>Order Type</th>
    <th style='border: 1px solid #909090; display: none;'>OrderID</th>
    <th style='border: 1px solid #909090; display: none;'>Lab ID</th>
    <th style='border: 1px solid #909090; display: none;'>Matching Patient ID</th>
    <th style='border: 1px solid #909090; display: none;'>Reason Code</th>
    <th style='border: 1px solid #909090; display: none;'>OrderingNPI</th>
    <th style='border: 1px solid #909090; display: none;'>ResultMasterID</th>
    </tr>
    </thead>
</table>`);

    var sCriteria = "";
    var NPINumbers = "";
    var cboProviderName = document.getElementById('cboProviderName');
    var cboCategory = document.getElementById("cboCategory");

    if (cboCategory.options[cboCategory.selectedIndex].text != undefined && cboCategory.options[cboCategory.selectedIndex].text != null && cboCategory.options[cboCategory.selectedIndex].text != "") {
        sCriteria = cboCategory.options[cboCategory.selectedIndex].text;
    }

    if (cboProviderName.options[cboProviderName.selectedIndex].text != undefined && cboProviderName.options[cboProviderName.selectedIndex].text.toUpperCase() == "ALL") {
        sCriteria = cboProviderName.options[cboProviderName.selectedIndex].text.toUpperCase();
    }
    else if (cboProviderName.options[cboProviderName.selectedIndex].text.toUpperCase() == "PROVIDER NOT ASSIGNED") {
        sCriteria = cboProviderName.options[cboProviderName.selectedIndex].text.toUpperCase();
    }
    else if (cboProviderName.options[cboProviderName.selectedIndex].text == undefined || cboProviderName.options[cboProviderName.selectedIndex].text == null || cboProviderName.selectedIndex < 0 || cboProviderName.options[cboProviderName.selectedIndex].text == "") {
        DisplayErrorMessage('7100011');
        return false;
    }


    if (cboProviderName.selectedIndex >= 0) {

        if (cboProviderName.options[cboProviderName.selectedIndex].text != undefined && cboProviderName.options[cboProviderName.selectedIndex].text != "") {
            NPINumbers = cboProviderName.options[cboProviderName.selectedIndex].value;
        }
        else {
            NPINumbers = "";
        }
    }


    const FieldName = new Array();
    const FieldValue = new Array();
    $('#txtPatientSearch').prop('disabled', true);
    var cboErrorReason = document.getElementById('cboErrorReason');
    var cboLabName = document.getElementById('cboLabName');
    var fromdate = $find("frmDate").get_dateInput()._text;
    var toDate = $find("toDate").get_dateInput()._text;

    const fromdatenew = new Date(fromdate).format('yyyyMMdd')
    const toDatenew = new Date(toDate).format('yyyyMMdd')


    if (cboErrorReason.selectedIndex != 0) {
        FieldName.push('Reason_Code')
        if (cboErrorReason.options[cboErrorReason.selectedIndex].text != undefined && cboErrorReason.options[cboErrorReason.selectedIndex].text != null) {
            FieldValue.push(cboErrorReason.options[cboErrorReason.selectedIndex].text);
        }
    }
    if (cboLabName.selectedIndex != 0) {
        FieldName.push('Lab_Id')
        if (cboLabName.options[cboLabName.selectedIndex].text != undefined && cboLabName.options[cboLabName.selectedIndex].text != null) {
            FieldValue.push(cboLabName.options[cboLabName.selectedIndex].value);
        }
    }

    if (fromdate != undefined && fromdate != null && fromdate != "" && toDate != undefined && toDate != null && toDate != "") {
        FieldName.push('MSH_Date_And_Time_Of_Message');
        FieldValue.push(fromdatenew + "-" + toDatenew);
    }



    data = JSON.stringify({
        "sCriteria": sCriteria,
        "NPINumbers": NPINumbers,
        "FieldName": FieldName.toString(),
        "FieldValue": FieldValue.toString(),
    });
    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
        $('#EncounterTable').DataTable().columns.adjust();
    });

    var dataTable = new DataTable('#EncounterTable', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        processing: false,
        ordering: true,
        scrollCollapse: false,
        scrollY: '180px',
        autoWidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Search by Name or Acct. # or Result Message Date",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top


        ajax: {
            url: '/frmLabException.aspx/LoadResults',
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
                data: 'HumanId', render: function (data, type, row) {
                    if (row.HumanId == "" || row.HumanId == 0)
                        return "";
                    else
                        return row.HumanId;
                }, sClass: 'TableCellBorder', sWidth: '4% !important'
            },

            {
                data: 'Last_Name_In_Result', render: function (data, type, row) {
                    return row.Last_Name_In_Result + " " + row.First_Name_In_Result + " " + row.MI_In_Result;
                },
                sClass: 'TableCellBorder word-break-all', sWidth: '7%'
            },


            {
                data: 'DOB_In_Result', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                }, sClass: 'TableCellBorder', searchable: false, sWidth: '7% !important',
                type: 'date'
            },

            { data: 'Sex_In_Result', sClass: 'TableCellBorder', searchable: false, sWidth: '4% !important' },

            { data: 'Order_ID', sClass: 'TableCellBorder word-break-all', searchable: false, sWidth: '4% !important' },
            { data: 'Specimen', sClass: 'TableCellBorder word-break-all', searchable: false, sWidth: '7% !important' },
            { data: 'Lab_Procedure', sClass: 'TableCellBorder word-break-all', searchable: false, sWidth: '16% !important' },
            {
                data: 'Reason_Code', render: function (data, type, row) {
                    return row.Reason_Code + "-" + row.Reason_Description;
                },
                sClass: 'TableCellBorder word-break-all', sWidth: '8.5%'
            },
            {
                data: 'Result_Received_Date_And_Time', render: function (data, type, row) {
                    if (row.Result_Received_Date_And_Time == "")
                        return "";
                    else
                        return parseCustomDate(row.Result_Received_Date_And_Time.replace("T", " "));
                }, sClass: 'TableCellBorder word-break-all',  sWidth: '5% !important',
                type: 'date'
            },

            { data: 'Provider_Name', sClass: 'TableCellBorder word-break-all', searchable: false, sWidth: '5% !important' },
            { data: 'Lab_Name', sClass: 'TableCellBorder word-break-all', searchable: false, sWidth: '6% !important' },


            { data: 'Is_Abnormal', sClass: 'TableCellBorder', searchable: false, sWidth: '5% !important' },

            {
                data: '', render: function (data, type, row) {
                    return "<td style='width:2%'><input type = 'image' name='grdViewResult' id ='grdViewResult' AutoPostBack='false' src = '/Resources/Down.bmp' class= 'loaderClass'  style = 'border-width:0px;'/></td>";
                }, sClass: 'TableCellBorder', searchable: false, sWidth: '2% !important'
            },

            { data: 'Result_Master_ID', sClass: "hide_column", searchable: false },
            { data: 'Order_ID', sClass: "hide_column", searchable: false },
            { data: 'Lab_ID', sClass: "hide_column", searchable: false },
            { data: 'Matching_Patient_ID', sClass: "hide_column", searchable: false },
            { data: 'Reason_Code', sClass: "hide_column", searchable: false },
            { data: 'OrderingProviderNPI', sClass: "hide_column", searchable: false },
            { data: 'Result_Master_ID', sClass: "hide_column", searchable: false },

        ],
        createdRow: function (row, data, dataIndex) {
            if (data.Is_Abnormal == "Yes") {
                $(row).css('color', 'red');
            }
        },
        initComplete: function (settings, json) {
            $("#EncounterTable_filter input")[0].classList.add('searchicon');
        }
    });

    $('#EncounterTable_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#EncounterTable_paginate').css({
        'font-weight': 'normal'
    }); 

    $('#EncounterTable_info').css({
        'min-width': '180px',
        'font-weight': 'normal'
    });


    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        grdUnassignedSelected = false;
        ReasonCode = "";
        GridToSelect = true;
        ResultMasterID = 0;
        MatchingPatientID = 0;
    });
    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        grdUnassignedSelected = false;
        ReasonCode = "";
        GridToSelect = true;
        ResultMasterID = 0;
        MatchingPatientID = 0;
    });
    dataTable.on("click", '#grdViewResult', function () {
        var datavalue = dataTable.row($(this).parents('tr')).data();
        var ResultMasterIDVal = datavalue.Result_Master_ID;
        Result(ResultMasterIDVal);
        return false;
    });

    $('#EncounterTable tbody').on('click', 'tr', function () {
        $('#EncounterTable tr').removeClass("odd");
        $('#EncounterTable tr').removeClass("even");
        dataTable.$('tr.highlight').removeClass('highlight');
        $(this)[0].classList.add('highlight');
        var data = dataTable.row($(this)).data();

        document.getElementById("chkUnmatchedProvider").disabled = true
        $find("cboUnmatchProvider").disable();
        $find("cboUnmatchProvider").get_items().getItem(0).select();
        $('#txtPatientSearch').val('');
        $('#txtPatientSearch').prop('disabled', false);
        
        document.getElementById('chkNoOrders').checked = false;

        var table = new DataTable('#OutstandingTable');
        table.clear().draw();
        table.destroy();

        GridToSelect = false;
        grdUnassignedSelected = true;
        ReasonCode = data.Reason_Code;
        ResultMasterID = data.Result_Master_ID;
        MatchingPatientID = data.Matching_Patient_ID;
        OrderSubmitID = data.Order_ID;
        var ResultId = data.Result_Master_ID;

        if ($('#EncounterTable tr.highlight')[0].innerText == "No data available in table") {
            return false;
        }
        else {
            $find("btnMatchOrders").set_enabled(true);
            $find("btnDemographics").set_enabled(true);
            $.ajax({
                type: "GET",
                url: "./frmLabException.aspx/FillOutstandingorders?ResultMasterID=" + ResultId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (json) {
                    var objdata = JSON.parse(json.d);
                    var cboCheck = false;
                    if (objdata.length > 0) {
                        if (objdata[0].Reason_Code != "ACUR_LAB_05" || objdata[0].Reason_Code != "ACUR_LAB_06") {
                            var combo = window.$find("cboUnmatchProvider");
                            if (cboProviderName.options[cboProviderName.selectedIndex].text == "ALL") {   
                                for (let i = 0; i < combo.get_items().get_count(); i++) {
                                    if (data.OrderingProviderNPI == "") {
                                        break;
                                    }
                                    if (combo.get_items().getItem(i).get_value() == data.OrderingProviderNPI) {

                                        combo.disable();
                                        document.getElementById("chkUnmatchedProvider").disabled = true;
                                        combo.get_items().getItem(i).select();
                                        combo.set_text(combo.get_items().getItem(i).get_text());
                                        combo.set_value(combo.get_items().getItem(i).get_value());
                                        cboCheck = true;
                                        break;
                                    }
                                }
                                if (!cboCheck) {
                                    combo.get_items().getItem(0).select();
                                    combo.set_text("");
                                    combo.set_value("");
                                    combo.enable();
                                    document.getElementById("chkUnmatchedProvider").disabled = false
                                }

                            }
                            else if (objdata[0].Reason_Code == "ACUR_LAB_05" || objdata[0].Reason_Code == "ACUR_LAB_06") {
                                combo.enable();
                                document.getElementById("chkUnmatchedProvider").disabled = false
                            }
                            else {
                                combo.disable();
                                document.getElementById("chkUnmatchedProvider").disabled = true;
                            }

                        }
                    }
                    if (objdata[0].Matching_Patient_ID != 0) {


                        var sPatientname = objdata[0].Last_Name_In_Capella + "," + objdata[0].First_Name_In_Capella + " " + objdata[0].MI_In_Capella + " | DOB: " + new Date(objdata[0].DOB_In_Capella).format("dd-MMM-yyyy") + " | " + objdata[0].Sex_In_Capella + " | ACC#: " + objdata[0].Human_Id_In_Capella.toString() + " | EX.ACC#: " + objdata[0].Patient_Account_External.toString() + " | ADDR: " + objdata[0].Street_Address1.toString() + " | Ph: " + objdata[0].Home_Phone_No.toString() + " | PATIENT TYPE: " + objdata[0].Human_Type.toString();
                        var txtPatientSearch = document.getElementById('txtPatientSearch');

                        $('#txtPatientSearch').prop('disabled', true);
                        $("#imgClearPatientText").addClass("disabled");
                        txtPatientSearch.value = sPatientname;
                        txtPatientSearch.attributes['data-human-id'].value = objdata[0].Matching_Patient_ID;
                        document.getElementById("hdnHumanID").value = objdata[0].Matching_Patient_ID;
                        sessionStorage.setItem("valuepatientsearch", sPatientname);
                        sessionStorage.setItem("labelpatientsearch", sPatientname);

                        const dataval = objdata[0].OrdersList;


                        const grouped = {};

                        dataval.forEach(item => {
                            const key = `${item.Order_Submit_ID}`;
                            if (!grouped[key]) {
                                grouped[key] = {
                                    Order_Submit_ID: item.Order_Submit_ID,
                                    Created_Date_And_Time: item.Created_Date_And_Time,
                                    Internal_Property_Lab_Name: item.Internal_Property_Lab_Name,
                                    Lab_Procedure: []
                                };
                            }
                            grouped[key].Lab_Procedure.push(item.Lab_Procedure + "-" + item.Lab_Procedure_Description);
                        });

                        const result = Object.values(grouped).map(item => ({
                            ...item,
                            Lab_Procedure: item.Lab_Procedure.join('; ')
                        })).sort((a, b) => b.Order_Submit_ID - a.Order_Submit_ID);



                        $("#OutstandingOrders").empty();
                        $("#OutstandingOrders").append(`
                            <table id=OutstandingTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
                            <thead class='header' style='border: 1px;width:96.7%;'>
                            <tr class='header' >
                            <th style='border: 1px solid #909090;text-align: center;width:8%'>Order#</th>
                            <th style='border: 1px solid #909090;text-align: center;width:70%'>Order Description(Procedures)</th>
                            <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Date</th>
                            <th style='border: 1px solid #909090;text-align: center;width:6%'>Lab Name</th>
                            <th style='border: 1px solid #909090; display: none;'>Order_Submit_ID</th>
                            </tr>
                            </thead>
                            </table>`);
                        $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
                            $('#OutstandingTable').DataTable().columns.adjust();
                        });

                        var dataTable = new DataTable('#OutstandingTable', {
                            data: [],
                            serverSide: false,
                            lengthChange: false,
                            processing: false,
                            ordering: true,
                            scrollCollapse: false,
                            scrollY: '145px',
                            autoWidth: false,
                            paging: false,
                            searching: false,
                            info: false,
                            dom: 'lrt',
                            order: [],

                            data: result,//objdata[0].OrdersList,                               

                            columns: [

                                {
                                    data: 'Order_Submit_ID', render: function (data, type, row) {
                                        return "ACUR" + row.Order_Submit_ID;
                                    },
                                    sClass: 'TableCellBorder word-break-all', sWidth: '8%'
                                },
                                { data: 'Lab_Procedure', sClass: 'TableCellBorder word-break-all', sWidth: '70%' },

                                {
                                    data: 'Created_Date_And_Time', render: function (data, type, row) {
                                        if (row.Created_Date_And_Time == "0001-01-01T00:00:00")
                                            return "";
                                        else
                                            return DOBConvert(row.Created_Date_And_Time.replace("T", " ").split(' ')[0]);
                                    }, sClass: 'TableCellBorder', searchable: false, sWidth: '6% !important',
                                    type: 'date'
                                },

                                { data: 'Internal_Property_Lab_Name', sClass: 'TableCellBorder', searchable: false, sWidth: '6% !important' },

                                { data: 'Order_Submit_ID', sClass: "hide_column", searchable: false },

                            ],

                        })


                        $('#OutstandingTable tbody').on('click', 'tr', function () {
                            $('#OutstandingTable tr').removeClass("odd");
                            $('#OutstandingTable tr').removeClass("even");
                            dataTable.$('tr.highlight').removeClass('highlight');
                            $(this)[0].classList.add('highlight');

                            document.getElementById('chkNoOrders').checked = false;

                            var data = dataTable.row($(this)).data();
                            if (data.Order_Submit_ID != 0) {
                                GridOutstandingSelect = false;
                                OrderSubmitID = data.Order_Submit_ID;
                            } else {
                                GridOutstandingSelect = true;
                            }

                        });

                        document.getElementById("chkUnmatchedProvider").enabled = false;
                    }
                    else {
                        var txtPatientSearch = document.getElementById('txtPatientSearch');

                        $('#txtPatientSearch').prop('disabled', false);
                        $('#txtPatientSearch').focus();
                        $("#imgClearPatientText").removeClass("disabled");
                        document.getElementById("hdnHumanID").value = 0;
                        sessionStorage.setItem("valuepatientsearch", "");
                        sessionStorage.setItem("labelpatientsearch", "");                        
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

    });
    return false;
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

function chkUnmatchedProvider_CheckedChanged() {
    var checked = document.getElementById("chkUnmatchedProvider").checked

    $.ajax({
        type: "POST",
        url: "./frmLabException.aspx/chkUnmatchedProvidercheck",
        data: JSON.stringify({
            "check": checked
        }),
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            var objdata = JSON.parse(json.d);
            if (objdata != "") {
                var PhyList = objdata.PhyList;
                var combo = $find("cboUnmatchProvider");
                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text("");
                comboItem.set_value("");
                combo.clearItems();
                combo.get_items().add(comboItem);
                combo.commitChanges();
                if (PhyList.length > 0) {
                    for (var i = 0; i < PhyList.length; i++) {
                        var sPhyName = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                        var PhyNPI = PhyList[i].PhyNPI;
                        var combo = $find("cboUnmatchProvider");
                        var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                        comboItem.set_text(sPhyName);
                        comboItem.set_value(PhyNPI);
                        combo.get_items().add(comboItem);
                        combo.commitChanges();
                    }
                }

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

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
    })
}

function chkProviderNameChecked() {
    var checked = document.getElementById("chkProviderName").checked

    $.ajax({
        type: "POST",
        url: "./frmLabException.aspx/chkProviderNameCheckedChanged",
        data: JSON.stringify({
            "check": checked
        }),
        contentType: "application/json; charset=utf-8",
        success: function (json) {
            var objdata = JSON.parse(json.d);
            if (objdata != "") {
                var PhyList = objdata.PhyList;
                var ddl = document.getElementById('cboProviderName');
                ddl.innerHTML = ""; 

                
                var defaultOption = document.createElement("option");
                defaultOption.text = "ALL";
                defaultOption.value = "";
                ddl.add(defaultOption);
                
                if (PhyList.length > 0) {
                    for (var i = 0; i < PhyList.length; i++) {
                        var option = document.createElement("option");
                        option.text = PhyList[i].PhyPrefix + " " + PhyList[i].PhyFirstName + " " + PhyList[i].PhyMiddleName + " " + PhyList[i].PhyLastName + " " + PhyList[i].PhySuffix;
                        option.value = PhyList[i].PhyNPI;
                        ddl.add(option);
                    }
                }
                
                    var defaultOption = document.createElement("option");
                    defaultOption.text = "PROVIDER NOT ASSIGNED";
                    defaultOption.value = "";
                    ddl.add(defaultOption);
                
            }
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
    })
}


function parseCustomDate(input) {

    input = input.substring(0, 12);
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    if (input.length !== 14 && input.length !== 12) return "";

    const year = input.substring(0, 4);
    const month = input.substring(4, 6);
    const day = input.substring(6, 8);
    const hour = input.substring(8, 10);
    const minute = input.substring(10, 12);
    const second = input.substring(12, 14);

    const utcDate = `${year}-${month}-${day} ${hour}:${minute}:${second}`;


    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate);
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
function btnDemographics_Clicked(sender, args) {
    debugger;
    var Humanid = '';
    var txtPatientSearch = document.getElementById('txtPatientSearch');
    if (txtPatientSearch.value != '') {
        Humanid = document.getElementById('hdnHumanID').value;
    }
    var obj = new Array();
    obj.push("HumanId=" + Humanid);
    obj.push("ScreenName=Demographics");
    var result = openModal("frmPatientDemographics.aspx", 1230, 1182, obj, 'DemographicsModalWindow');
    var winObj = $find('DemographicsModalWindow');
    winObj.add_close(OnClientCloseOrderManagement);

}
function OnClientCloseOrderManagement(oWindow, args) {
    debugger;
    //var vHumanID = '895225';
    var vHumanID = '';
    var arg = args.get_argument();

    if (arg != null) {
        document.getElementById(GetClientId("hdnHumanID")).value = arg.HumanID;
        vHumanID = arg.HumanID.toString();
    }
    else {
        if (sessionStorage.getItem("HumanId") != null && sessionStorage.getItem("HumanId") != undefined) {
            document.getElementById(GetClientId("hdnHumanID")).value = sessionStorage.getItem("HumanId");
            vHumanID = sessionStorage.getItem("HumanId").toString();
            sessionStorage.setItem("HumanId", "");
        }
    }

    var txtPatientSearch = document.getElementById('txtPatientSearch');
    debugger;

    var WSData = {
        HumanID: vHumanID,
    }
    if (vHumanID != '' && vHumanID != undefined) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "./frmLabException.aspx/GenerateHumanDetails",
            data: JSON.stringify(WSData),
            dataType: "json",
            success: function (data) {
                debugger
                var SelectedPatient = JSON.parse(data.d);
                var HumanDetails = SelectedPatient.HumanDetails;
                var txtPatientSearch = document.getElementById('txtPatientSearch');
                txtPatientSearch.value = SelectedPatient.HumanDetails;
                txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
                sessionStorage.setItem("valuepatientsearch", SelectedPatient.HumanDetails);
                sessionStorage.setItem("labelpatientsearch", SelectedPatient.HumanDetails);
                //document.getElementById('InvisibleButton').click();

                {
                    //var objdata = JSON.parse(json.d);


                    const dataval = SelectedPatient.listOrders;


                    const grouped = {};

                    dataval.forEach(item => {
                        const key = `${item.Order_Submit_ID}`;
                        if (!grouped[key]) {
                            grouped[key] = {
                                Order_Submit_ID: item.Order_Submit_ID,
                                Created_Date_And_Time: item.Created_Date_And_Time,
                                Internal_Property_Lab_Name: item.Internal_Property_Lab_Name,
                                Lab_Procedure: []
                            };
                        }
                        grouped[key].Lab_Procedure.push(item.Lab_Procedure + "-" + item.Lab_Procedure_Description);
                    });

                    const result = Object.values(grouped).map(item => ({
                        ...item,
                        Lab_Procedure: item.Lab_Procedure.join('; ')
                    })).sort((a, b) => b.Order_Submit_ID - a.Order_Submit_ID);

                    $("#OutstandingOrders").empty();
                    $("#OutstandingOrders").append(`
                            <table id=OutstandingTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'>
                            <thead class='header' style='border: 1px;width:96.7%;'>
                            <tr class='header' >
                            <th style='border: 1px solid #909090;text-align: center;width:8%'>Order#</th>
                            <th style='border: 1px solid #909090;text-align: center;width:70%'>Order Description(Procedures)</th>
                            <th style='border: 1px solid #909090;text-align: center;width:6%'>Order Date</th>
                            <th style='border: 1px solid #909090;text-align: center;width:6%'>Lab Name</th>
                            <th style='border: 1px solid #909090; display: none;'>Order_Submit_ID</th>
                            </tr>
                            </thead>
                            </table>`);
                    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
                        $('#OutstandingTable').DataTable().columns.adjust();
                    });

                    var dataTable = new DataTable('#OutstandingTable', {
                        data: [],
                        serverSide: false,
                        lengthChange: false,
                        processing: false,
                        ordering: true,
                        scrollCollapse: false,
                        scrollY: '145px',
                        autoWidth: false,
                        paging: false,
                        searching: false,
                        info: false,
                        dom: 'lrt',
                        order: [],

                        data: result,//objdata[0].OrdersList,                               

                        columns: [

                            {
                                data: 'Order_Submit_ID', render: function (data, type, row) {
                                    return "ACUR" + row.Order_Submit_ID;
                                },
                                sClass: 'TableCellBorder word-break-all', sWidth: '8%'
                            },
                            { data: 'Lab_Procedure', sClass: 'TableCellBorder word-break-all', sWidth: '70%' },

                            {
                                data: 'Created_Date_And_Time', render: function (data, type, row) {
                                    if (row.Created_Date_And_Time == "0001-01-01T00:00:00")
                                        return "";
                                    else
                                        return DOBConvert(row.Created_Date_And_Time.replace("T", " ").split(' ')[0]);
                                }, sClass: 'TableCellBorder', searchable: false, sWidth: '6% !important',
                                type: 'date'
                            },

                            { data: 'Internal_Property_Lab_Name', sClass: 'TableCellBorder', searchable: false, sWidth: '6% !important' },

                            { data: 'Order_Submit_ID', sClass: "hide_column", searchable: false },

                        ],

                    })


                    $('#OutstandingTable tbody').on('click', 'tr', function () {
                        $('#OutstandingTable tr').removeClass("odd");
                        $('#OutstandingTable tr').removeClass("even");
                        dataTable.$('tr.highlight').removeClass('highlight');
                        $(this)[0].classList.add('highlight');

                        document.getElementById('chkNoOrders').checked = false;

                        var data = dataTable.row($(this)).data();
                        if (data.Order_Submit_ID != 0) {
                            GridOutstandingSelect = false;
                            OrderSubmitID = data.Order_Submit_ID;
                        } else {
                            GridOutstandingSelect = true;
                        }

                    });

                    document.getElementById("chkUnmatchedProvider").enabled = false;

                }

            }
        });
    }
}