var payerList = new Array();
var payerListID = new Array();
var ServiceCodeList = new Array();
var AutoIncList = new Array();
var EVWebServiceLink = null;
var ProjectName = null;
$(document).ready(function () {
    $("#txtdatefrom").datetimepicker({ timepicker: false, format: 'd-M-Y' });
    $("#txtdateto").datetimepicker({ timepicker: false, format: 'd-M-Y' });
    EVWebServiceLink = sessionStorage.getItem("EVWebServiceLink");
    ProjectName = sessionStorage.getItem("EVProjectName");
    $.get(EVWebServiceLink + '/evfieldlookup?customerName=' + ProjectName + '&fieldname=EB03ServiceTypeCode', null, function (data) {
        console.log(data);
        for (var iCount = 0; iCount < data.length; iCount++) {
            ServiceCodeList.push(data[iCount]);
        }
    });

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
})

function OpenInsurance() {
    if (document.getElementById("hdnHumanDetails").value != null) {
        var sHumandatails = new Array();
        sHumandatails = document.getElementById("hdnHumanDetails").value.split('~');
        var Insurance_Plan_ID;
        if ($("#tbodupolicyinfo tr.highlight").length > 0) {
            Insurance_Plan_ID = $("#tbodupolicyinfo tr.highlight")[0].childNodes[12].innerText.trim();
        }
        setTimeout(
           function () {
               var oWnd = GetRadWindow();
               var oManager = oWnd.get_windowManager();
               var childWindow = oManager.BrowserWindow.radopen("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + sHumandatails[0] + "&InsuranceType=true&LastName=" + sHumandatails[1] + "&FirstName=" + sHumandatails[2] + "&ExAccountNo=" + sHumandatails[3] + "&PatientType=" + sHumandatails[4] + "&EncounterId=" + sHumandatails[5] + "&CurrentProcess=" + sHumandatails[6] + "&Insurance_Plan_ID=" + Insurance_Plan_ID + "&ScreenMode=PerformEv", "ctl00_DemographicsModalWindow");
               SetRadWindowProperties(childWindow, 575, 1160);
               childWindow.add_close(InsuranceClose);
           }, 0);
        return false;
    }
}

function CloseEv() {
    self.close();
}
function SetRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}
function ClosePatIns(oWindow, args) {
    $('#seldateNextApp').val("---Select---");
    $('#seldateNextApp').prop("disabled", true);
    alert("Success");
    $('#txtdatefrom').prop("disabled", false);
    $('#txtdateto').prop("disabled", false);
}

function InsuranceClose(oWindow, args) {
    bindEVTable();
}

function disablecontrols() {
    if ($('#radionextApp')[0].checked) {
        $('#txtdatefrom').val("");
        $('#txtdatefrom').prop("disabled", true);
        $('#txtdateto').val("");
        $('#txtdateto').prop("disabled", true);
        $('#seldateNextApp').prop("disabled", false);
        $('#spandatefrom').removeClass('MandLabelstyle');
        $('#spandatefrom').addClass('spanstyle');
        $('#spnstardatefrom').css('display', 'none');
        $('#spannxtappt').removeClass('spanstyle');
        $('#spannxtappt').addClass('MandLabelstyle');
        $('#spannxtapptstar').css('display', 'inline');
    }
}

function disablecontrolsDate() {
    if ($('#radioDate')[0].checked) {
        $('#seldateNextApp').val("---Select---");
        $('#seldateNextApp').prop("disabled", true);
        $('#txtdatefrom').prop("disabled", false);
        $('#txtdateto').prop("disabled", false);
        $('#spandatefrom').removeClass('spanstyle');
        $('#spandatefrom').addClass('MandLabelstyle');
        $('#spnstardatefrom').css('display', 'inline');
        $('#spannxtappt').removeClass('MandLabelstyle');
        $('#spannxtappt').addClass('spanstyle');
        $('#spannxtapptstar').css('display', 'none');

        var month_names = ["Jan", "Feb", "Mar",
                    "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep",
                    "Oct", "Nov", "Dec"];
        var todaydate = new Date();
        var day = todaydate.getDate();
        var month = todaydate.getMonth();
        var year = todaydate.getFullYear();
        if (day.toString().length == 1)
            day = "0" + day;
        var datestring = day + "-" + month_names[month] + "-" + year;

        document.getElementById("txtdatefrom").value = datestring;
        document.getElementById("txtdateto").value = datestring;
    }
}
function bindEVTable() {

    localStorage.setItem("ServiceTypeCode", document.getElementById('ServiceTypeCode').value);
    localStorage.setItem("ServiceTypeDescription", document.getElementById('ServiceTypeCodeDesc').value);

    document.getElementById("txtServieType").value = localStorage.getItem("ServiceTypeDescription");
    $("#txtServieType").attr("title", document.getElementById("txtServieType").value);
    if (EVWebServiceLink == null) {
        EVWebServiceLink = sessionStorage.getItem("EVWebServiceLink");
    }
    jQuery.ajax({
        url: EVWebServiceLink + '/EVPayers',
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                payerList.push(data[i].PayerNAICCode + "|" + data[i].AllowedSTCcodes);
                payerListID.push(data[i].PayerName.toUpperCase() + "~" + data[i].PayerID);
            }
        }
    });
    var sHumandatails = new Array();
    var planid = document.getElementById("hdnInsPlan_Id").value;
    var highlightclass = "";
    sHumandatails = document.getElementById("hdnHumanDetails").value.split('~');
    $.ajax({
        type: "POST",
        url: "frmPerformEV.aspx/LoadEV",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            "HumanID": sHumandatails[0],
        }),
        dataType: "json",
        async: true,
        success: function (data) {
            var tabContents = "";
            var objdata = $.parseJSON(data.d);
            $("#tbodupolicyinfo").empty();
            if (objdata.EV != null && objdata.EV != undefined) {
                for (var i = 0; i < objdata.EV.length; i++) {
                    var Evmode = "";
                    var Status = "";
                    var servicetypeCount = ""
                    var naiccode = ""
                    var flage = 0;
                    for (var h = 0; h < payerList.length; h++) {
                        if (payerList[h].split('|')[0] == objdata.EV[i][16]) {
                            flage = 1;
                            Evmode = "E/M";
                            break;
                        }
                    }
                    if (flage == 0) {
                        Evmode = "M";
                    }
                   
                    naiccode = objdata.EV[i][16];
                    var flag = 0;
                    for (var j = 0; j < payerList.length; j++) {
                        if (payerList[j].split('|')[0] == objdata.EV[i][16]) {
                            flag = 1
                            servicetypeCount = payerList[j].split('|')[1];

                        }
                        if (flag == 1)
                            break;

                    }


                    //bug id:66771

                    if (objdata.EV[i][6].trim() != "") {
                        Status = objdata.EV[i][6];

                    }
                    else if (objdata.EV[i][6].toUpperCase().indexOf("CONTACT SUPPORT") >= 0) {
                        Status = "Error";
                    }
                   else  if (objdata.EV[i][22] != "" && objdata.EV[i][22] == "MANUAL") {//For Bug Id 61512
                        Status = "EV-PERFORMED MANUALLY";

                    }
                    else {
                        Status = "EV-NOT PERFORMED";
                    }


                    var checkfor = "";
                    if (objdata.EV[i][3] == objdata.EV[i][14])
                        checkfor = objdata.EV[i][3]
                    else
                        checkfor = objdata.EV[i][3] + " To " + objdata.EV[i][14]
                    if (tabContents == "") {

                        var hdnIsSendEVEnable = document.getElementById("hdnIsSendEVEnable").value;
                        if (hdnIsSendEVEnable == "true")
                            document.getElementById("btnev").disabled = true;
                        else {
                            if (Evmode == "M") {
                                document.getElementById("btnev").disabled = true;

                            }
                            else {
                                document.getElementById("btnev").disabled = false;
                            }
                        }
                        var planid = document.getElementById("hdnInsPlan_Id").value;
                        if (i == 0) {
                            if (document.getElementById("hdnInsPlan_Id").value == undefined || document.getElementById("hdnInsPlan_Id").value == "") {
                                highlightclass = "<tr onclick='select(this)' class='highlight'>";
                            }
                            else if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                highlightclass = "<tr onclick='select(this)' class='highlight'>";
                            }
                            else {
                                highlightclass = "<tr onclick='select(this)'>";
                            }
                        }
                        else {
                            if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                highlightclass = "<tr onclick='select(this)' class='highlight'>";
                            }
                            else {
                                highlightclass = "<tr onclick='select(this)'>";
                            }
                        }


                        tabContents = highlightclass + "<td style='width: 10%'>" + objdata.EV[i][0] + "</td>" +
                            "<td  style='width: 20%' >" + objdata.EV[i][1] + "</td>" +
                            "<td style='width: 10%'>" + objdata.EV[i][2] + "</td>" +
                            "<td style='width: 18%'>" + checkfor + "</td>" +
                            "<td style='width: 1%;display:none'>" + objdata.EV[i][14] + "</td>" +
                            "<td style='width: 5%'>  <i class='glyphicon glyphicon-eye-open'  onclick='ViewFile(this);' ></i> </td>" +
                            "<td style='width: 3%'>" + Evmode + "</td>" +
                            "<td style='width: 5%'>" + Status.toUpperCase()+ "</td>" +
                            "<td style='width: 17%'>" + objdata.EV[i][7] + "</td>" +
                            "<td style='width: 20%'>" + objdata.EV[i][8] + "</td>" +
                            "<td style='width: 12%'>" + objdata.EV[i][15] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][9] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][10] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][11] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][12] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][13] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][18] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][19] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][20] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][21] + "</td>" +
                            "<td style='display:none'>" + servicetypeCount + "</td>" +
                            "<td style='display:none'>" + naiccode + "</td>" +
                            "</tr>";
                    }
                    else {

                        var planid = document.getElementById("hdnInsPlan_Id").value;
                        if (i == 0) {
                            if (document.getElementById("hdnInsPlan_Id").value == undefined || document.getElementById("hdnInsPlan_Id").value == "") {
                                highlightclass = "<tr onclick='select(this)' class='highlight'>";
                            }
                            else if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                highlightclass = "<tr onclick='select(this)' class='highlight'>";
                            }
                            else {
                                highlightclass = "<tr onclick='select(this)'>";
                            }
                        }
                        else {
                            if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                highlightclass = "<tr onclick='select(this)' class='highlight'>";
                            }
                            else {
                                highlightclass = "<tr onclick='select(this)'>";
                            }
                        }
                        tabContents = tabContents + highlightclass + "<td style='width: 10%'>" + objdata.EV[i][0] + "</td>" +
                           "<td  style='width: 20%' >" + objdata.EV[i][1] + "</td>" +
                            "<td style='width: 10%'>" + objdata.EV[i][2] + "</td>" +
                            "<td style='width: 18%'>" + checkfor + "</td>" +
                            "<td style='width: 1%;display:none'>" + objdata.EV[i][14] + "</td>" +
                            "<td style='width: 5%'>  <i class='glyphicon glyphicon-eye-open'  onclick='ViewFile(this);' ></i> </td>" +
                            "<td style='width: 3%'>" + Evmode + "</td>" +
                            "<td style='width: 5%'>" + Status.toUpperCase() + "</td>" +
                            "<td style='width: 17%'>" + objdata.EV[i][7] + "</td>" +
                            "<td style='width: 20%'>" + objdata.EV[i][8] + "</td>" +
                            "<td style='width: 12%'>" + objdata.EV[i][15] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][9] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][10] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][11] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][12] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][13] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][18] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][19] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][20] + "</td>" +
                            "<td style='display:none'>" + objdata.EV[i][21] + "</td>" +
                            "<td style='display:none'>" + servicetypeCount + "</td>" +
                            "<td style='display:none'>" + naiccode + "</td>" +
                            "</tr>";
                    }
                    $('#btnmEV').attr('disabled', false);
                    $('#btnrefresh').attr('disabled', false);

                }
            }

            var hdnIsSendEVEnable = document.getElementById("hdnIsSendEVEnable").value;
            if (hdnIsSendEVEnable == "true")
                document.getElementById("btnev").disabled = true;
            else
                document.getElementById("btnev").disabled = false;

            if (tabContents == "") {
                tabContents = "<tr><td colspan='12'>No Data Found</td></tr>"
                $('#btnev').attr('disabled', true);
                $('#btnmEV').attr('disabled', true);
                $('#btnrefresh').attr('disabled', true);

            }

            $("#tbodupolicyinfo").append(tabContents);

            if ($("#tbodupolicyinfo tr.highlight")[0] != null &&
                $("#tbodupolicyinfo tr.highlight")[0] != undefined && $("#tbodupolicyinfo tr.highlight")[0].childNodes[21] != null &&
                $("#tbodupolicyinfo tr.highlight")[0].childNodes[21] != undefined)
                localStorage.setItem("NaicCode", $("#tbodupolicyinfo tr.highlight")[0].childNodes[21].innerText.trim());

            else
                localStorage.setItem("NaicCode", "");

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
                return false;
            }
        }
    });
}
function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    oTbl.attr("data-item-original-width", oTbl.width());
    oTbl.find('thead tr td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    newTbl.width(newTbl.attr('data-item-original-width'));
    newTbl.find('thead tr td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    oTbl.width(oTbl.attr('data-item-original-width'));
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    if (tblAsJQueryObject[0] != undefined) {
        if (tblAsJQueryObject[0].parentElement.parentElement.id == "GeneralQTable") {
            $("#ScrollIDGeneral").css('height', '');
            $("#ScrollIDGeneral").css('overflow-y', '');
        }
        else {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
        }
    }
}

function ValidateEffStartDate(EffDate, TermDate) {
    var splitEffDatedate = document.getElementById(EffDate).value;
    var splitTermDate = document.getElementById(TermDate).value;
    var EffDatedate = new Date();
    var TermDate = new Date();
    var m = getMonth(splitEffDatedate.split('-')[1]);
    if (m == 55) {
        return false;
    }
    EffDatedate.setFullYear(splitEffDatedate.split('-')[2], m, splitEffDatedate.split('-')[0]);
    if (isNaN(EffDatedate)) {
        return false;
    }
    var n = getMonth(splitTermDate.split('-')[1]);
    if (n == 55) {
        return false;
    }
    TermDate.setFullYear(splitTermDate.split('-')[2], n, splitTermDate.split('-')[0]);
    if (isNaN(TermDate)) {
        return false;
    }
    if (parseInt(splitEffDatedate.split('-')[0]) > 31) {
        return false;
    }
    if (parseInt(splitTermDate.split('-')[0]) > 31) {
        return false;
    }
    if ((EffDatedate.getFullYear() > TermDate.getFullYear())) {
        return false;
    }
    else if (EffDatedate.getMonth() > TermDate.getMonth() && (EffDatedate.getFullYear() >= TermDate.getFullYear())) {
        return false;
    }
    else if (EffDatedate.getDate() > TermDate.getDate() && (EffDatedate.getMonth() >= TermDate.getMonth()) && (EffDatedate.getFullYear() >= TermDate.getFullYear())) {
        return false;
    }
    else {
        return true;
    }
}

function getMonth(Month) {
    var month = new Array();
    switch (Month) {
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
        case Month:
            x = 55;
            break;
    }
    return x;
}
function formatDate() {
    var d = new Date(),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('');
}
function startTime() {
    var today = new Date();
    var h = today.getHours();
    if (h < 10)
        h = h + 10;

    var m = today.getMinutes();
    if (m < 10)
        m = m + 10;

    var s = today.getSeconds();
    if (s < 10)
        s = s + 10;
    return h.toString() + m.toString() + s.toString();
}


function format(inputDate) {
    var date = new Date(inputDate);
    if (!isNaN(date.getTime())) {
        return date.getMonth() + 1 + '/' + date.getDate() + '/' + date.getFullYear();
    }
}

function SendEvRequest() {
    if ($('#radionextApp')[0].checked && $('#seldateNextApp option:selected').val() == "---Select---") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        DisplayErrorMessage('1011181');
        return false;
    }
    else if ($('#radioDate')[0].checked && $('#txtdatefrom').val() == "" && $('#txtdateto').val() == "") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        DisplayErrorMessage('1011182');
        return false;
    }
    else if ($("#tbodupolicyinfo tr.highlight").length == 0) {
        return false;
    }
    if ($('#radioDate')[0].checked && document.getElementById("txtdateto").value == "" && document.getElementById("txtdatefrom").value != "") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        DisplayErrorMessage('1011178');
        return;
    }

    if ($('#radioDate')[0].checked && ValidateEffStartDate("txtdatefrom", "txtdateto") == false && document.getElementById("txtdatefrom").value != ""
      && document.getElementById("txtdateto").value != "") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        DisplayErrorMessage('1011176');
        return false;
    }
    if ($('#radioDate')[0].checked && (document.getElementById("txtdatefrom").value != undefined && document.getElementById("txtdatefrom").value == "") &&
      (document.getElementById("txtdateto").value != undefined && document.getElementById("txtdateto").value == "")) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        DisplayErrorMessage('1011177');
        return false;
    }

    if ($('#txtServieType')[0].value.split('|').count > $("#tbodupolicyinfo tr.highlight")[0].childNodes[20].innerText.trim()) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        DisplayErrorMessage('1011198', '', $("#tbodupolicyinfo tr.highlight")[0].childNodes[20].innerText.trim());
        return false;
    }

    else {
        document.getElementById("btnev").disabled = true;
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        var sHumandatails = document.getElementById("hdnHumanDetails").value.split('~');
        var Insurance_Plan_ID = $("#tbodupolicyinfo tr.highlight")[0].childNodes[12].innerText.trim();
        var Policy_Holder_ID = $("#tbodupolicyinfo tr.highlight")[0].childNodes[9].innerText.trim();
        var Carrier_ID = $("#tbodupolicyinfo tr.highlight")[0].childNodes[13].innerText.trim();

        var Data = [sHumandatails[0], Carrier_ID, Insurance_Plan_ID, Policy_Holder_ID];
        var objPerformEVDetails = null;

        $.ajax({
            type: "POST",
            url: "frmPerformEV.aspx/GetPerformEVDetails",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "data": Data,
            }),
            dataType: "json",
            async: false,
            success: function (data) {
                objPerformEVDetails = $.parseJSON(data.d);
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
                    return false;
                }
            }
        });

        var EligibilityRequestByClient = new Object();
        EligibilityRequestByClient.EligibilityReqByClientID = 1;
        ProjectName = sessionStorage.getItem("EVProjectName");
        EligibilityRequestByClient.CustomerName = ProjectName;
        EligibilityRequestByClient.NM109PayerNAICCodePayer = objPerformEVDetails.Payerid_NAIC_ID;
        EligibilityRequestByClient.BatchID = 0;
        EligibilityRequestByClient.PatientId = objPerformEVDetails.HumanDetails[0].Id;
        EligibilityRequestByClient.BHT04Date = formatDate();
        EligibilityRequestByClient.BHT05Time = startTime();
        EligibilityRequestByClient.DMG01DatetimePeriodFormatQualifierSubscriber = "D8";
        EligibilityRequestByClient.DMG02DatetimePeriodSubscriber = objPerformEVDetails.HumanDetails[0].Birth_Date.replace("T00:00:00", "").replace("-", "").replace("-", "");

        if (objPerformEVDetails.HumanDetails[0].Sex != "")
            EligibilityRequestByClient.DMG03GenderCodeSubscriber = objPerformEVDetails.HumanDetails[0].Sex.substr(0, 1);
        else
            EligibilityRequestByClient.DMG03GenderCodeSubscriber = "";
        if (objPerformEVDetails.InsuredHumanDetails[0].Sex != "" && objPerformEVDetails.InsuredRelationship.toUpperCase() != "" && objPerformEVDetails.InsuredRelationship.toUpperCase() != "SELF")
            EligibilityRequestByClient.DMG03GenderCodeDependent = objPerformEVDetails.InsuredHumanDetails[0].Sex.substr(0, 1);
        else
            EligibilityRequestByClient.DMG03GenderCodeDependent = "";
        EligibilityRequestByClient.EQ0201ProductIDQualifierSubscriber = "IP";
        EligibilityRequestByClient.EQ03CoverageLevelCodeSubscriber = "IND";
        EligibilityRequestByClient.EQ06NetworkIndicatorCodeSubscriber = "I";
        EligibilityRequestByClient.GS04Date = formatDate();
        EligibilityRequestByClient.GS05Time = startTime();
        EligibilityRequestByClient.NM108IdentificationcodeQualifierSubscriber = "MI";
        EligibilityRequestByClient.ReqBy = objPerformEVDetails.CurrentUserName;
        //Relationship
        if (objPerformEVDetails.InsuredRelationship != "" && objPerformEVDetails.InsuredRelationship.toUpperCase() != "SELF") {
            EligibilityRequestByClient.HL04HierarchicalChildCodeDependent = "1";
            EligibilityRequestByClient.HL04HierarchicalChildCodeSubscriber = "1";
            EligibilityRequestByClient.INS01ResponseCodeSubscriber = "Y";
            EligibilityRequestByClient.INS01ResponseCodeDependent = "Y";
            EligibilityRequestByClient.NM101EntityIdentifierCodeDependent = "03";
            EligibilityRequestByClient.NM102EntityTypequalifierDependent = "1";
            if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "SPOUSE")
                EligibilityRequestByClient.INS02IndividualRelationshipCodeSubscriber = "01";
            else if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "CHILD")
                EligibilityRequestByClient.INS02IndividualRelationshipCodeSubscriber = "19";
            else if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "OTHER")
                EligibilityRequestByClient.INS02IndividualRelationshipCodeSubscriber = "34";
            else if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "LIFE PARTNER")
                EligibilityRequestByClient.INS02IndividualRelationshipCodeSubscriber = "53";
            //Dependent
            EligibilityRequestByClient.EQ0201ProductIDQualifierDependent = "IP";
            EligibilityRequestByClient.EQ03CoverageLevelCodeDependent = "IND";
            EligibilityRequestByClient.NM103OrganizationNameDependent = objPerformEVDetails.InsuredHumanDetails[0].Last_Name;
            EligibilityRequestByClient.NM104NameFirstDependent = objPerformEVDetails.InsuredHumanDetails[0].First_Name;
            EligibilityRequestByClient.NM105NameMiddleDependent = objPerformEVDetails.InsuredHumanDetails[0].MI;
            EligibilityRequestByClient.NM107NameSuffixDependent = objPerformEVDetails.InsuredHumanDetails[0].Suffix;
            EligibilityRequestByClient.N301AddressLineDependent = objPerformEVDetails.InsuredHumanDetails[0].Street_Address1;
            EligibilityRequestByClient.N302AddressLineDependent = objPerformEVDetails.InsuredHumanDetails[0].Street_Address2;
            EligibilityRequestByClient.N401CityNameDependent = objPerformEVDetails.InsuredHumanDetails[0].City;
            EligibilityRequestByClient.N402StateCodeDependent = objPerformEVDetails.InsuredHumanDetails[0].State;
            EligibilityRequestByClient.N403PostalCodeDependent = objPerformEVDetails.InsuredHumanDetails[0].ZipCode;
            EligibilityRequestByClient.INS17NumberDependent = objPerformEVDetails.InsuredHumanDetails[0].Birth_Order;
            EligibilityRequestByClient.DTP01DateSubscriber = "291";
            EligibilityRequestByClient.DMG01DatetimePeriodFormatQualifierDependent = "D8";
            if (objPerformEVDetails.InsuredHumanDetails[0].Birth_Date != "1/1/0001 12:00:00 AM")
                EligibilityRequestByClient.DMG02DatetimePeriodDependent = objPerformEVDetails.InsuredHumanDetails[0].Birth_Date.replace("T00:00:00", "").replace("-", "").replace("-", "");
            else
                EligibilityRequestByClient.DMG02DatetimePeriodDependent = "";
            if ($('#radionextApp')[0].checked) {
                EligibilityRequestByClient.DTP02DateTimeSubscriber = "D8";
                EligibilityRequestByClient.DTP02DateTimeDependent = "D8";
                EligibilityRequestByClient.DTP03DateTimePeriodSubscriber = DateFormat($('#seldateNextApp option:selected').val().split(' ')[0]);
            }
            else {
                EligibilityRequestByClient.DTP02DateTimeSubscriber = "RD8";
                EligibilityRequestByClient.DTP02DateTimeDependent = "RD8";
                EligibilityRequestByClient.DTP03DateTimePeriodSubscriber = DateFormat($('#txtdatefrom').val()) + "-" + DateFormat($('#txtdateto').val());
            }

            EligibilityRequestByClient.DTP01DateDependent = "291";
            if ($('#seldateNextApp option:selected').val() != "---Select---")
                EligibilityRequestByClient.DTP03DateTimePeriodDependent = DateFormat($('#seldateNextApp option:selected').val().split(' ')[0]);
        }
        else {
            EligibilityRequestByClient.HL04HierarchicalChildCodeDependent = "";
            EligibilityRequestByClient.HL04HierarchicalChildCodeSubscriber = "0";
            EligibilityRequestByClient.INS01ResponseCodeSubscriber = "N";
            EligibilityRequestByClient.INS01ResponseCodeDependent = "";
            EligibilityRequestByClient.INS02IndividualRelationshipCodeSubscriber = "18";
            EligibilityRequestByClient.NM101EntityIdentifierCodeDependent = "";
            EligibilityRequestByClient.NM102EntityTypequalifierDependent = "";
            //Dependent
            EligibilityRequestByClient.NM103OrganizationNameDependent = "";
            EligibilityRequestByClient.NM104NameFirstDependent = "";
            EligibilityRequestByClient.NM105NameMiddleDependent = "";
            EligibilityRequestByClient.NM107NameSuffixDependent = "";
            EligibilityRequestByClient.N301AddressLineDependent = "";
            EligibilityRequestByClient.N302AddressLineDependent = "";
            EligibilityRequestByClient.N401CityNameDependent = "";
            EligibilityRequestByClient.N402StateCodeDependent = "";
            EligibilityRequestByClient.N403PostalCodeDependent = "";
            EligibilityRequestByClient.INS17NumberDependent = "";
            EligibilityRequestByClient.DTP01DateSubscriber = "291";
            EligibilityRequestByClient.DMG02DatetimePeriodDependent = "";
            EligibilityRequestByClient.EQ0201ProductIDQualifierDependent = "";
            EligibilityRequestByClient.EQ03CoverageLevelCodeDependent = "";
            if ($('#radionextApp')[0].checked) {
                EligibilityRequestByClient.DTP02DateTimeSubscriber = "D8";
                EligibilityRequestByClient.DTP03DateTimePeriodSubscriber = DateFormat($('#seldateNextApp option:selected').val().split(' ')[0]);
            }
            else {
                EligibilityRequestByClient.DTP02DateTimeSubscriber = "RD8";
                EligibilityRequestByClient.DTP03DateTimePeriodSubscriber = DateFormat($('#txtdatefrom').val()) + "-" + DateFormat($('#txtdateto').val());
            }
            EligibilityRequestByClient.DTP01DateDependent = "";
            EligibilityRequestByClient.DTP03DateTimePeriodDependent = "";
        }
        EligibilityRequestByClient.IEA01NumberofIncludedFunctionalGroups = "1";
        EligibilityRequestByClient.INS17NumberSubscriber = objPerformEVDetails.InsuredHumanDetails[0].Birth_Order;

        if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "SPOUSE")
            EligibilityRequestByClient.INS02IndividualRelationshipCodeDependent = "01";
        else if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "CHILD")
            EligibilityRequestByClient.INS02IndividualRelationshipCodeDependent = "19";
        else if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "OTHER")
            EligibilityRequestByClient.INS02IndividualRelationshipCodeDependent = "34";
        else if (objPerformEVDetails.InsuredRelationship.toUpperCase() == "LIFE PARTNER")
            EligibilityRequestByClient.INS02IndividualRelationshipCodeDependent = "53";
        else
            EligibilityRequestByClient.INS02IndividualRelationshipCodeDependent = "";

        EligibilityRequestByClient.ISA09InterchangeDate = formatDate().substr(2, 6);
        EligibilityRequestByClient.ISA10InterchangeTime = startTime().substr(0, 4);
        EligibilityRequestByClient.NM103OrganizationNameSubscriber = objPerformEVDetails.HumanDetails[0].Last_Name;
        EligibilityRequestByClient.NM104NameFirstSubscriber = objPerformEVDetails.HumanDetails[0].First_Name;
        EligibilityRequestByClient.NM105NameMiddleSubscriber = objPerformEVDetails.HumanDetails[0].MI;
        EligibilityRequestByClient.NM107NameSuffixSubscriber = objPerformEVDetails.HumanDetails[0].Suffix;
        EligibilityRequestByClient.N301AddressLineSubscriber = objPerformEVDetails.HumanDetails[0].Street_Address1;
        EligibilityRequestByClient.N302AddressLineSubscriber = objPerformEVDetails.HumanDetails[0].Street_Address2;
        EligibilityRequestByClient.N401CityNameSubscriber = objPerformEVDetails.HumanDetails[0].City;
        EligibilityRequestByClient.N402StateCodeSubscriber = objPerformEVDetails.HumanDetails[0].State;
        EligibilityRequestByClient.N403PostalCodeSubscriber = objPerformEVDetails.HumanDetails[0].ZipCode;
        EligibilityRequestByClient.NM103OrganizationNamePayer = $("#tbodupolicyinfo tr.highlight")[0].childNodes[0].innerText.trim();
        EligibilityRequestByClient.NM103OrganizationNameCustomer = objPerformEVDetails.ProjectName;
        EligibilityRequestByClient.NM104FirstNameCustomer = objPerformEVDetails.PhysicianDetails.PhyFirstName;
        EligibilityRequestByClient.NM105MiddleNameCustomer = objPerformEVDetails.PhysicianDetails.PhyMiddleName;
        EligibilityRequestByClient.NM107NameSuffixCustomer = objPerformEVDetails.PhysicianDetails.PhySuffix;
        EligibilityRequestByClient.NM109IdentificationcodeSubscriber = objPerformEVDetails.PolicyHolderID;
        if ($('#txtServieType')[0].value != "") {
            var sService = new Array();
            sService = $('#txtServieType')[0].value.split('|');
            var sEQ01ServiceType = "";
            if (ProjectName == null) {
                ProjectName = sessionStorage.getItem("EVProjectName");
            }
            if (ServiceCodeList == null) {
                $.get(EVWebServiceLink + '/evfieldlookup?customerName=' + ProjectName + '&fieldname=EB03ServiceTypeCode', null, function (data) {
                    console.log(data);
                    for (var iCount = 0; iCount < data.length; iCount++) {
                        ServiceCodeList.push(data[iCount]);
                    }
                });
            }
            if (ServiceCodeList != null) {
                for (var i = 0; i < sService.length; i++) {
                    for (var j = 0; j < ServiceCodeList.length; j++) {
                        if (sService[i] == ServiceCodeList[j].Description) {
                            if (sEQ01ServiceType == "") {
                                sEQ01ServiceType = ServiceCodeList[j].Value;
                            }

                            else
                                sEQ01ServiceType = sEQ01ServiceType + "," + ServiceCodeList[j].Value;
                        }
                    }
                }
                EligibilityRequestByClient.EQ01ServiceTypeCodeSubscriber = sEQ01ServiceType;
            }
        }
        if (EVWebServiceLink == null) {
            EVWebServiceLink = sessionStorage.getItem("EVWebServiceLink");
        }
        $.post(EVWebServiceLink + "/evservice", EligibilityRequestByClient, function (data) {
            console.log(data);
            document.getElementById("btnev").disabled = false;
            var human_id = $("#tbodupolicyinfo tr.highlight")[0].childNodes[11].innerText.trim();
            var Payer_Name = $("#tbodupolicyinfo tr.highlight")[0].childNodes[0].innerText.trim();
            var plan_name = $("#tbodupolicyinfo tr.highlight")[0].childNodes[1].innerText.trim();
            var TRN02TraceNumber1 = "";
            if (data.TRN02TraceNumberSubscriber != null && data.TRN02TraceNumberSubscriber != undefined)
                TRN02TraceNumber1 = data.TRN02TraceNumberSubscriber
            var Service_Type_Code = localStorage.getItem("ServiceTypeCode");
            var Service_Type = $('#txtServieType').val();
            var Insurance_Type = $("#tbodupolicyinfo tr.highlight")[0].childNodes[2].innerText.trim();
            var Requested_For_From_Date = "";
            var status = "";
            if (data.Status != null && data.Status != undefined)
                Status = "EV-IN PROGRESS";// data.Status; bugid:67076 
            var Requested_For_To_Date = "";
            if ($('#radionextApp')[0].checked) {

                Requested_For_From_Date = $('#seldateNextApp option:selected').val();
                Requested_For_To_Date = $('#seldateNextApp option:selected').val();
            }
            else if ($('#radioDate')[0].checked) {
                Requested_For_From_Date = $('#txtdatefrom').val();
                Requested_For_To_Date = $('#txtdateto').val();
            }
            var Mode = $("#tbodupolicyinfo tr.highlight")[0].childNodes[6].innerText.trim();

            var Data = [human_id, Payer_Name, Insurance_Plan_ID, plan_name, Policy_Holder_ID, TRN02TraceNumber1, Service_Type_Code, Service_Type, Insurance_Type, Requested_For_From_Date, Requested_For_To_Date, Status, Mode];
            $.ajax({
                type: "POST",
                url: "frmPerformEV.aspx/SaveEVDetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    "data": Data,
                }),
                dataType: "json",
                async: true,
                success: function (data) {
                    if (data.d == "Success")
                        bindEVTable();
                    else
                        alert(data.d);
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
                        return false;
                    }
                }
            });
        }).fail(function (xhr, status, error) {

            if ($("#tbodupolicyinfo tr.highlight").length > 0) {
                $("#tbodupolicyinfo tr.highlight")[0].childNodes[8].innerText = "Could not Connect to EV Service. Please TryAgain Later";
                $("#tbodupolicyinfo tr.highlight")[0].childNodes[7].innerText = "Error";
                document.getElementById("btnev").disabled = false;
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        });
    }
    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
    return false;
}

function openManualEV() {
    if ($("#tbodupolicyinfo tr.highlight").length > 0) {
        var dt = new Date();
        var now = new Date();
        var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
        var GroupNumber = "";
        var EffectiveStartDate = "";
        var TerminationDate = "";
        var LastName = "";
        var ClaimCity = $("#tbodupolicyinfo tr.highlight")[0].childNodes[17].innerText.trim();
        var ClaimState = $("#tbodupolicyinfo tr.highlight")[0].childNodes[18].innerText.trim();
        var ClaimAddress = $("#tbodupolicyinfo tr.highlight")[0].childNodes[16].innerText.trim().replace("#","!@");
        var Zipcode = $("#tbodupolicyinfo tr.highlight")[0].childNodes[19].innerText.trim();
        setTimeout(
                function () {
                    var oWnd = GetRadWindow();
                    var oManager = oWnd.get_windowManager();
                    var childWindow = oManager.BrowserWindow.radopen("frmEligibilityVerification.aspx?humanID=" +
                        $("#tbodupolicyinfo tr.highlight")[0].childNodes[11].innerText.trim() + "&patientName=" + LastName + "&groupNumber=" + GroupNumber +
                        "&policyHolderId=" + $("#tbodupolicyinfo tr.highlight")[0].childNodes[9].innerText.trim() +
                        "&insPlanId=" + $("#tbodupolicyinfo tr.highlight")[0].childNodes[12].innerText.trim() +
                        "&carrierName=" + $("#tbodupolicyinfo tr.highlight")[0].childNodes[0].innerText.trim() +
                        "&dtEffectiveDate=" + EffectiveStartDate + "&dtTerminationDate=" + TerminationDate
                        + "&insplanname=" + $("#tbodupolicyinfo tr.highlight")[0].childNodes[1].innerText.trim()
                        + "&ClaimCity=" + ClaimCity
                        + "&ClaimState=" + ClaimState
                           + "&ClaimAddress=" + ClaimAddress
                              + "&zipcode=" + Zipcode
                                 + "&UTCTime=" + utc
                        , "ctl00_DemographicsModalWindow");
                    SetRadWindowProperties(childWindow, 575, 900);
                }, 0);
        return false;
    }
}

function showall() {
    if ($('#chkshowall')[0].checked) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var sHumandatails = new Array();
        sHumandatails = document.getElementById("hdnHumanDetails").value.split('~');
        var highlightclass = "";
        $.ajax({
            type: "POST",
            url: "frmPerformEV.aspx/LoadEVshowall",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "HumanID": sHumandatails[0],
            }),
            dataType: "json",
            async: true,
            success: function (data) {



                var tabContents = "";
                var objdata = $.parseJSON(data.d);
                $("#tbodupolicyinfo").empty();
                if (objdata.EV != null && objdata.EV != undefined) {
                    for (var i = 0; i < objdata.EV.length; i++) {

                        var Evmode = "";
                        var Status = "";
                        var servicetypeCount = ""
                        var naiccode = ""
                        var flage = 0;
                        for (var h = 0; h < payerList.length; h++) {
                            if (payerList[h].split('|')[0] == objdata.EV[i][16]) {
                                flage = 1;
                                Evmode = "E/M";
                                break;
                            }
                        }
                        if (flage == 0) {
                            Evmode = "M";
                        }
                       
                        naiccode = objdata.EV[i][16];
                        var flag = 0;
                        for (var j = 0; j < payerList.length; j++) {
                            if (payerList[j].split('|')[0] == objdata.EV[i][16]) {
                                flag = 1
                                servicetypeCount = payerList[j].split('|')[1];

                            }
                            if (flag == 1)
                                break;

                        }

                        //bug id:66771

                        if (objdata.EV[i][6].trim()!="") {
                            Status = objdata.EV[i][6];

                        }
                        else if (objdata.EV[i][6].toUpperCase().indexOf("CONTACT SUPPORT") >= 0) {
                            Status = "Error";
                        }
                       else if (objdata.EV[i][22] != "" && objdata.EV[i][22] == "MANUAL") {//For Bug Id 61512
                            Status = "EV-PERFORMED MANUALLY";

                        }
                        else  {
                           Status = "EV-NOT PERFORMED";
                        }
                       

                        var checkfor = "";
                        if (objdata.EV[i][3] == objdata.EV[i][14])
                            checkfor = objdata.EV[i][3]
                        else
                            checkfor = objdata.EV[i][3] + " To " + objdata.EV[i][14]
                        if (tabContents == "") {

                            var hdnIsSendEVEnable = document.getElementById("hdnIsSendEVEnable").value;
                            if (hdnIsSendEVEnable == "true")
                                document.getElementById("btnev").disabled = true;
                            else {
                                if (Evmode == "M") {
                                    document.getElementById("btnev").disabled = true;

                                }
                                else {
                                    document.getElementById("btnev").disabled = false;
                                }
                            }
                            var planid = document.getElementById("hdnInsPlan_Id").value;
                            if (i == 0) {
                                if (document.getElementById("hdnInsPlan_Id").value == undefined || document.getElementById("hdnInsPlan_Id").value == "") {
                                    highlightclass = "<tr onclick='select(this)' class='highlight'>";
                                }
                                else if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                    highlightclass = "<tr onclick='select(this)' class='highlight'>";
                                }
                                else {
                                    highlightclass = "<tr onclick='select(this)'>";
                                }
                            }
                            else {
                                if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                    highlightclass = "<tr onclick='select(this)' class='highlight'>";
                                }
                                else {
                                    highlightclass = "<tr onclick='select(this)'>";
                                }
                            }

                            tabContents = highlightclass + "<td style='width: 10%'>" + objdata.EV[i][0] + "</td>" +
                                "<td  style='width: 20%' >" + objdata.EV[i][1] + "</td>" +
                                "<td style='width: 10%'>" + objdata.EV[i][2] + "</td>" +
                                "<td style='width: 18%'>" + checkfor + "</td>" +
                                "<td style='width: 1%;display:none'>" + objdata.EV[i][14] + "</td>" +
                                "<td style='width: 5%'>  <i class='glyphicon glyphicon-eye-open'  onclick='ViewFile(this);' ></i> </td>" +
                                "<td style='width: 3%'>" + Evmode + "</td>" +
                                "<td style='width: 5%'>" + Status.toUpperCase() + "</td>" +
                                "<td style='width: 17%'>" + objdata.EV[i][7] + "</td>" +
                                "<td style='width: 20%'>" + objdata.EV[i][8] + "</td>" +
                                "<td style='width: 12%'>" + objdata.EV[i][15] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][9] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][10] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][11] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][12] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][13] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][18] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][19] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][20] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][21] + "</td>" +
                                 "<td style='display:none'>" + servicetypeCount + "</td>" +
                                   "<td style='display:none'>" + naiccode + "</td>" +


                                "</tr>";

                        }
                        else {
                            var planid = document.getElementById("hdnInsPlan_Id").value;
                            if (i == 0) {
                                if (document.getElementById("hdnInsPlan_Id").value == undefined || document.getElementById("hdnInsPlan_Id").value == "") {
                                    highlightclass = "<tr onclick='select(this)' class='highlight'>";
                                }
                                else if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                    highlightclass = "<tr onclick='select(this)' class='highlight'>";
                                }
                                else {
                                    highlightclass = "<tr onclick='select(this)'>";
                                }
                            }
                            else {
                                if (planid != null && planid != "" && planid == objdata.EV[i][10]) {
                                    highlightclass = "<tr onclick='select(this)' class='highlight'>";
                                }
                                else {
                                    highlightclass = "<tr onclick='select(this)'>";
                                }
                            }




                            tabContents = tabContents + highlightclass + "<td style='width: 10%'>" + objdata.EV[i][0] + "</td>" +
                               "<td  style='width: 20%' >" + objdata.EV[i][1] + "</td>" +
                                "<td style='width: 10%'>" + objdata.EV[i][2] + "</td>" +
                                "<td style='width: 18%'>" + checkfor + "</td>" +
                                "<td style='width: 1%;display:none'>" + objdata.EV[i][14] + "</td>" +
                                "<td style='width: 5%'>  <i class='glyphicon glyphicon-eye-open'  onclick='ViewFile(this);' ></i> </td>" +
                                "<td style='width: 3%'>" + Evmode + "</td>" +
                                "<td style='width: 5%'>" + Status.toUpperCase() + "</td>" +
                                "<td style='width: 17%'>" + objdata.EV[i][7] + "</td>" +
                                "<td style='width: 20%'>" + objdata.EV[i][8] + "</td>" +
                                "<td style='width: 12%'>" + objdata.EV[i][15] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][9] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][10] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][11] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][12] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][13] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][18] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][19] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][20] + "</td>" +
                                "<td style='display:none'>" + objdata.EV[i][21] + "</td>" +
                                "<td style='display:none'>" + servicetypeCount + "</td>" +
                                "<td style='display:none'>" + naiccode + "</td>" +
                                "</tr>";
                        }
                        $('#btnmEV').attr('disabled', false);
                        $('#btnrefresh').attr('disabled', false);

                    }
                }

                var hdnIsSendEVEnable = document.getElementById("hdnIsSendEVEnable").value;
                if (hdnIsSendEVEnable == "true")
                    document.getElementById("btnev").disabled = true;
                else
                    document.getElementById("btnev").disabled = false;

                if (tabContents == "") {
                    tabContents = "<tr><td colspan='12'>No Data Found</td></tr>"
                    $('#btnev').attr('disabled', true);
                    $('#btnmEV').attr('disabled', true);
                    $('#btnrefresh').attr('disabled', true);

                }

                $("#tbodupolicyinfo").append(tabContents);

                if ($("#tbodupolicyinfo tr.highlight")[0] != null &&
                    $("#tbodupolicyinfo tr.highlight")[0] != undefined && $("#tbodupolicyinfo tr.highlight")[0].childNodes[21] != null &&
                    $("#tbodupolicyinfo tr.highlight")[0].childNodes[21] != undefined)
                    localStorage.setItem("NaicCode", $("#tbodupolicyinfo tr.highlight")[0].childNodes[21].innerText.trim());

                else
                    localStorage.setItem("NaicCode", "");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
                    return false;
                }
            }
        });
    }
    else {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        bindEVTable();
    }
}

function select(e) {
    var existingSelectedItem = $("#tbodupolicyinfo tr.highlight");
    if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
    $(e).toggleClass("highlight");

    var hdnIsSendEVEnable = document.getElementById("hdnIsSendEVEnable").value;
    if (hdnIsSendEVEnable == "true")
        document.getElementById("btnev").disabled = true;
    else {
        if ($("#tbodupolicyinfo tr.highlight")[0].childNodes[6].innerText.trim() == "M") {
            document.getElementById("btnev").disabled = true;
        }
        else {
            document.getElementById("btnev").disabled = false;
        }
    }
}
function ViewFile(e) {

    var fillocation = e.parentElement.parentElement.children[15].innerText.trim();
    var filecount = fillocation.split('|');
    var filePath = "";
    for (var i = 0; i < filecount.length ; i++) {
        if (i == 0)
            filePath = filecount[i];
        else {
            if (filecount[i] != "")
                filePath = filePath + "|" + filecount[i];
        }
    }
    var sHumaName = null;
    if (document.getElementById("hdnHumanDetails").value.split('~').length > 0)
        sHumaName = document.getElementById("hdnHumanDetails").value.split('~')[1] + "_" + document.getElementById("hdnHumanDetails").value.split('~')[2];
    if (filePath != "") {
        var obj = new Array();
        obj.push("SI=" + filePath);
        obj.push("Location=EV");
        obj.push("PageTitle=Eligibility Verification - Response File");
        obj.push("HumanName=" + sHumaName)
        var result = openModal("frmPrintPDF.aspx", 750, 1000, obj, "MessageWindow");
    }
    else {
        DisplayErrorMessage('1011183');
    }
}
function SetRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}
function CloseServiceType() {

    if ($('input[type="checkbox"]:checked').length > 0) {
        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
                               "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';
        event.preventDefault();

        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'left' + " " + 'center',
                at: 'center' + " " + 'center + 100px'

            },
            buttons: {
                "Yes": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    serviceOKclick();
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    self.close();
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    return false;
                }
            }
        });
    }
    else {
        self.close();
        return false;
    }
}
function CloseServiceTypepopup(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById('txtServieType').value = Result.ServiceType;
        $("#txtServieType").attr("title", Result.ServiceType);

        localStorage.setItem("ServiceTypeCode", Result.ServiceCode);
        document.getElementById('ServiceTypeCode').value = Result.ServiceCode;
        localStorage.setItem("ServiceTypeDescription", document.getElementById('txtServieType').value);
        document.getElementById('ServiceTypeCodeDesc').value = document.getElementById('txtServieType').value;
    }
}
function serviceOKclick() {
    var servicetype = "";
    var ServiceCode = "";
    for (var i = 0; i < $('input[type="checkbox"]:checked').length; i++) {
        if (i == 0) {
            servicetype = $('#' + $('input[type="checkbox"]:checked')[i].id.replace("chk", "td"))[0].innerText.trim();
            ServiceCode = $('#' + $('input[type="checkbox"]:checked')[i].id)[0].attributes['code'].value;
        }
        else {
            servicetype = servicetype + "|" + $('#' + $('input[type="checkbox"]:checked')[i].id.replace("chk", "td"))[0].innerText.trim();
            ServiceCode = ServiceCode + "|" + $('#' + $('input[type="checkbox"]:checked')[i].id)[0].attributes['code'].value;
        }
    }
    var result = new Object();
    result.ServiceType = servicetype;
    result.ServiceCode = ServiceCode;
    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    returnToParent(result);
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
function LoadServiceType() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //CAP-2787
    $.ajax({
        type: "GET",
        url: "ConfigXML/staticlookup.json",
        dataType: "json",
        async: false,
        success: function (xml) {

            var content = "";
            var servicetypecnt = xml.ServiceTypeList.length;
            if (parseInt(servicetypecnt) > 0) {
                var fixedlength = parseInt(servicetypecnt) % 20;
                var columnscount = 3;
                var width = 100 / columnscount;
                var typewidth = parseInt(width) - 5;
                var i = 0;
                var header = "";
                for (var j = 0; j < columnscount; j++) {
                    if (j == 0)
                        header = "<thead class='Gridheaderstyle'><th style='width:5%' class='Gridheaderstyle'>Select</th><th class='Gridheaderstyle'  style='width:" + typewidth + "%'>Service Type</th>";
                    else if (j == columnscount - 1)
                        header = header + "<th class='Gridheaderstyle' style='width:5%'>Select</th><th class='Gridheaderstyle' style='width:" + typewidth + "%'>Service Type</th></thead>";
                    else
                        header = header + "<th class='Gridheaderstyle' style='width:5%' >Select</th><th class='Gridheaderstyle'  style='width:" + typewidth + "%'>Service Type</th>";
                }
                for (i = 0 ; i < servicetypecnt; i = i + columnscount) {
                    if (i == 0)
                        content = "<tbody><tr>"
                    else
                        content = content + "<tr style='border: 1px dotted;'>"
                    for (var j = 0; j < columnscount; j++) {
                        if (xml.ServiceTypeList[parseInt(i) + parseInt(j)] != undefined) {
                            content = content + "<td style='width:5%'><input code='" + xml.ServiceTypeList[parseInt(i) + parseInt(j)].code + "'type='checkbox' id='chk" + xml.ServiceTypeList[parseInt(i) + parseInt(j)].code + "' onclick='EnableSave(this);checkboxchange(this);'/></td><td  id='td" + xml.ServiceTypeList[parseInt(i) + parseInt(j)].code + "'style='width:" + typewidth + "%'>" + xml.ServiceTypeList[parseInt(i) + parseInt(j)].type + "</td>";
                            if (j == columnscount - 1)
                                content = content + "</tr>"
                        }
                    }
                }
            }
            $('#tblserviceType').append(header + content + "</tbody>")

            scrolify($('#tblserviceType'), 500);

            if (localStorage.getItem("ServiceTypeCode") != null && localStorage.getItem("ServiceTypeCode") != undefined && localStorage.getItem("ServiceTypeCode") != "") {
                var servicrtypecode = localStorage.getItem("ServiceTypeCode").split('|');
                for (var i = 0; i < servicrtypecode.length; i++) {
                    $('#chk' + servicrtypecode[i]).attr("checked", true);
                }

                if (localStorage.getItem("Servicetypecodecount") != null &&
                    localStorage.getItem("Servicetypecodecount") > 1 && localStorage.getItem("NaicCode") != null && localStorage.getItem("NaicCode") != "") {
                    //CAP-2787
                    $.ajax({
                        type: "GET",
                        url: "ConfigXML/staticlookup.json",
                        dataType: "json",
                        async: false,
                        cache: false,
                        success: function (xml) {


                            var ServiceTypeSelectionList = xml.ServiceTypeSelectionList;


                            for (var i = 0; i < ServiceTypeSelectionList.length; i++) {
                                if (localStorage.getItem("ServiceTypeCode") == ServiceTypeSelectionList[i].ServiceTypeCode && ServiceTypeSelectionList[i].Payer.indexOf(localStorage.getItem("NaicCode")) >= 0) {
                                    $('#chk' + ServiceTypeSelectionList[i].Code).attr("checked", false);
                                    $('#chk' + ServiceTypeSelectionList[i].Code).attr("disabled", true);
                                }

                            }

                        }
                    });


                }
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }, error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);

                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                return false;
            }
        }

    });
}


function checkboxchange(e) {
    if (localStorage.getItem("Servicetypecodecount") != null && localStorage.getItem("Servicetypecodecount") > 1 && e.id.indexOf('chk' + localStorage.getItem("ServiceTypeCode")) >= 0) {
        //CAP-2787
        $.ajax({
            type: "GET",
            url: "ConfigXML/staticlookup.json",
            dataType: "json",
            async: false,
            cache: false,
            success: function (xml) {


                var ServiceTypeSelectionList = xml.ServiceTypeSelectionList;



                $('input:checkbox').attr("disabled", false);

                for (var i = 0; i < ServiceTypeSelectionList.length; i++) {
                    if (e.checked && localStorage.getItem("NaicCode") != "" && ServiceTypeSelectionList[i].Payer.indexOf(localStorage.getItem("NaicCode")) >= 0) {
                        $('#chk' + ServiceTypeSelectionList[i].Code).attr("checked", false);
                        $('#chk' + ServiceTypeSelectionList[i].Code).attr("disabled", true);
                    }

                }

            }, error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);

                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                    return false;
                }
            }
        });
    }
}
function EnableSave(e) {
    document.getElementById('btnOK').disabled = false;
    if (e.checked) {
       
        if (localStorage.getItem("Servicetypecodecount") != null && localStorage.getItem("Servicetypecodecount") != "") {
            if ($('input[type="checkbox"]:checked').length > localStorage.getItem("Servicetypecodecount")) {
                DisplayErrorMessage('1011198', '', localStorage.getItem("Servicetypecodecount"));
                $(e).attr("checked", false);
                return false;
            }
        }
        else {
            if ($('input[type="checkbox"]:checked').length > 99) {
                DisplayErrorMessage('1011184');
                $(e).attr("checked", false);
                return false;
            }
        }
    }
}
function OpenServiceType() {
    if ($("#tbodupolicyinfo tr.highlight").length > 0) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        var obj = new Array();
        localStorage.setItem("Servicetypecodecount", $("#tbodupolicyinfo tr.highlight")[0].childNodes[20].innerText.trim());
        var result = openModal("frmServicetype.aspx", 600, 1000, obj, "MessageWindow");
        $("frmServicetype").find("Telerik.Web.UI.WindowBehaviors").css("visibility", "hidden");
        var WindowName = $find('MessageWindow');
        WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);

        WindowName.add_close(CloseServiceTypepopup);
        return false;
    }
    else {

    }
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    if (oWindow == null) {
        oWindow = $find(ModalWndw);
    }
    return oWindow;
}

//dd-MMM-yyyy to yyyyMMdd
function DateFormat(dtDate) {
    var date = dtDate.split("-");
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    for (var j = 0; j < months.length; j++) {
        if (date[1] == months[j]) {
            date[1] = months.indexOf(months[j]) + 1;
        }
    }
    if (date[1] < 10) {
        date[1] = '0' + date[1];
    }
    var formattedDate = date[2] + date[1] + date[0];
    return formattedDate;
}