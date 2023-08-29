$(document).ready(function () {
  
    $("#dtpToDate").css("background-color", "white");
    $(".datecontrol").mask("9999?-aaa-99");
    $("#btnAdd").addClass("disabled");
    $("#btnAdd")[0].disabled = true;
    $("#txtDrugName").on("keypress", function () {
        EnableSave();
    });
    $("#RxHistoryNotes").on("keypress", function () {
        EnableSave();
    });
    $("#dtpFromDate").on("keypress", function () {
        EnableSave();
    });
    $("#dtpToDate").on("keypress", function () {
        EnableSave();
    });
    $("#selStrength").combobox({ placeholder: "", containerWidth: $('#selStrength').parent().width() });
    $("#selRouteOAdministration").combobox({ placeholder: "", containerWidth: $('#selRouteOAdministration').parent().width() });
   
});
function settooltip() {
    document.getElementById('selPharmacist').title = document.getElementById('selPharmacist').value;
}
function rowclick(e)
{
    $('.highlight').removeClass("highlight");
    $(e).toggleClass("highlight");
}
function EnableSave() {
    $("#btnAdd").removeClass("disabled");
    $("#btnAdd")[0].disabled = false;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
}
function openAddorUpdate(field_name) {
    var field_name = field_name;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + field_name;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}
function OpenFrequentUsedDrugs(field_name) {
    var field_name = field_name;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#Modal .modal-dialog").width(1040);
    $(top.window.document).find("#Modal .modal-content").width('100%');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmManageFrequentlyUsedDrugs.aspx";
    $(top.window.document).find("#ModalTtle")[0].textContent = "Manage Frequently Used Drugs";
    $(top.window.document).find("#Modal").one("hidden.bs.modal", function (e) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        drug_dts = [];
        drugs = [];
        $.ajax({
            type: "POST",
            url: "frmRxHistory.aspx/LoadDrugsDetails",
            contentType: "application/json;charset=utf-8",
            datatype: "json",
            success: function success(data) {
                var Druglist = JSON.parse(data.d);
                var DrugPanel = $("#DrugPanel");
                DrugPanel.empty();
                var j = 0;
                for (var i = 0; i < Druglist.length; i++) {
                    drug_dts[i] = "Drug_Name:" + Druglist[i].split('~')[0] + "$Route_of_Admin:" + Druglist[i].split('~')[1] + "$Strength:" + Druglist[i].split('~')[2];
                    if (drugs.indexOf(Druglist[i].split('~')[0].toUpperCase()) == -1) {
                        drugs[j] = Druglist[i].split('~')[0].toUpperCase();
                        j++;
                    }
                }
                var p = '<p onclick="OpenFrequentUsedDrugs(\'Drug Name\');" style="font-weight:bold;background-color:#BFDBFF !important" >Click here to open Frequently Used Drugs</p>';
                DrugPanel.append(p);

                for (var i = 0; i < drugs.length; i++) {
                    var p = '<p onclick="fillDrugROA(\'' + drugs[i] + '\');">' + drugs[i].toUpperCase() + '</p>';
                    DrugPanel.append(p);
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

            },
            error: function onerror(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }

        });
    });
}
function callweb(icon, List, id) {

    EnableSave();
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");

        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var values = response.d.split("|");
                var targetControlValue = id;
                var innerdiv = '';
                var pos = $('#' + targetControlValue).position();
                if (sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN" || sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN ASSISTANT") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;font-weight:bolder;cursor:default' class='alinkstyle' onclick=\"openAddorUpdate('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"fun('" + values[i] + "^" + targetControlValue + "');\">" + values[i] + "</li>";
                }
                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }

                    $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                      .css({
                          top: pos.top + 40,//(".actcmpt").height() + 250,
                          left: pos.left,
                          width: $('#' + targetControlValue).width() + 30 + "px",
                          height: '150px',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          border: '1px solid #8e8e8e',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto'
                          //CAP-804 Syntax error, unrecognized expression
                      }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");
    }

    if (hdnFieldName != null && hdnFieldName != icon) {

        $(hdnFieldName).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFieldName = icon;

}
function fun(agrulist) {

    var sugglistval;
    var control;
    var value = agrulist.split("^");
    if (value.length > 2) {
        //CAP-804 Syntax error, unrecognized expression
        control = value[5]?.trim();
        sugglistval = $("#" + control + ".actcmpt").val().trim();
        var selectedvalue = value[0] + ',' + value[1] + ',' + value[2] + ',' + value[3] + ',' + value[4];
        if (sugglistval != " " && sugglistval != "") {
            var subsugglistval = sugglistval;
            var len = subsugglistval.length;
            var flag = 0;
            if (subsugglistval == selectedvalue) {
                flag++;
            }
            if (flag == 0) {
                $("#" + control + ".actcmpt").val(sugglistval + "," + selectedvalue);
            }
        }
        else {
            $("#" + control + ".actcmpt").val(selectedvalue);
        }
    }
    else {
        //CAP-804 Syntax error, unrecognized expression
        sugglistval = $("#" + value[1]?.trim() + ".actcmpt").val().trim();
        if (sugglistval != " " && sugglistval != "") {
            var subsugglistval = sugglistval.split(",")
            var len = subsugglistval.length;
            var flag = 0
            for (var i = 0; i < len; i++) {
                if (subsugglistval[i] == value[0]) {
                    flag++;
                }
            }
            if (flag == 0) {
                $("#" + value[1]?.trim() + ".actcmpt").val(sugglistval + "," + value[0]);
            }
        }
        else {
            $("#" + value[1]?.trim() + ".actcmpt").val(value[0]);
        }
    }


}
function ClearValues() {
    var IsClearAll = null;
    if ($('#btnClear')[0].value == "Clear All")
        IsClearAll = DisplayErrorMessage('180010');
    else
        if ($('#btnClear')[0].value == "Cancel")
            IsClearAll = DisplayErrorMessage('180049');
    if (IsClearAll != undefined) {
        if (IsClearAll == true) {
            ClearAll();
        }
    }
}
function fillDrugROA(drugName) {
    $("#txtDrugName").val(drugName);
    var ROA = $("#selRouteOAdministration");
    var aryROA = [];
    for (var i = 0; i < drug_dts.length; i++) {
        if ((drug_dts[i].split(':')[1].split('$')[0]).toUpperCase() == drugName.toUpperCase()) {
            if (aryROA.indexOf(drug_dts[i].split(':')[2].split('$')[0]) == -1) {
                var option = '<option>' + drug_dts[i].split(':')[2].split('$')[0] + '</option>';
                ROA.append(option);
                $('.custom-combobox-input.ui-autocomplete-input')[1].value = $("#selRouteOAdministration option")[$("#selRouteOAdministration option").length - 1].text;
                aryROA.push(drug_dts[i].split(':')[2].split('$')[0]);
            }
        }
    }
    FillStrength();
    EnableSave();
}
function FillStrength() {
    var drugName = $("#txtDrugName")[0].value;
    if ($("#selRouteOAdministration option:selected")[0] != undefined)
        var ROA = $("#selRouteOAdministration option:selected")[0].value;
    $("#selStrength").empty();
    var selStrength = $("#selStrength")[0];
    for (var i = 0; i < drug_dts.length; i++) {
        if ((drug_dts[i].split(':')[1].split('$')[0]).toUpperCase() == drugName.toUpperCase() && (drug_dts[i].split(':')[2].split('$')[0].toUpperCase() == $('.custom-combobox-input.ui-autocomplete-input')[1].value.toUpperCase())) {
            selStrength.innerHTML += "<option value='" + drug_dts[i].split(':')[3].split('$')[0] + "'>" + drug_dts[i].split(':')[3].split('$')[0] + "</option>";
            $('.custom-combobox-input.ui-autocomplete-input')[0].value = $("#selStrength option")[$("#selStrength option").length - 1].text;
        }
    }
    EnableSave();
}
function ChkCurrent() {
    EnableSave();

    if ($("#chkCurntDt")[0].checked == true) {
        var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        var d = new Date();
        $("#dtpToDate").val("");
        $("#dtpToDate").prop("disabled", "disabled");
        $("#dtpToDate").css("background-color", "#E0E0E0");
    }
    else {
        $("#dtpToDate").val("");
        $("#dtpToDate").removeAttr("disabled");
        $("#dtpToDate").css("background-color", "white");
    }
}
function ClearAll() {
    $(":input").not(':input[type=button]').val("");
    $("#dtpToDate").removeAttr("disabled");
    $("#btnAdd")[0].disabled = true;
    $("#btnAdd").addClass("disabled").val("Add");
    $('#btnClear')[0].value = "Clear All";
    $("#selStrength").empty().combobox({ placeholder: "", containerWidth: $('#selStrength').parent().width() });
}
var drug_dts = [];
var drugs = [];
function loadRx_History() {
    
    var tbody = $("#tRxMed");
    $("#tRxMed").empty();
    var tr = '<thead class="Gridheaderstyle"><tr><th id="thDelete" style="width: 4%;">Del.</th><th id="thClose" style="width: 4.5%;">Stop</th>' +
             '<th id="thDrugName" style="width: 15%;">Drug Name</th><th id="thStrength" style="width: 14%;">Strength</th>' +
             '<th id="thFrequency" style="width: 13%;">Frequency</th> <th id="thFrmDt" style="width: 10%;">From Date</th>'+
             '<th id="thToDt" style="width: 10%;">To Date</th><th id="thRoa" style="width: 15%;">Route of Administration</th>'+
             '<th id="thNotes" style="width: 15%;">Notes</th><th id="thID" style="width: 1%; display: none"></th>'+
             '<th id="thVersion" style="width: 1%; display: none"></th><th id="thEncID" style="width: 1%; display: none"></th>'+
             '<th id="thStopNotes" style="width: 1%; display: none"></th><th id="thRetainNotes" style="width: 1%; display: none">' +
             '<th id="thStatus" style="width: 1%; display: none"></th><th id="thCreatedBy" style="width: 1%; display: none">' +
             '<th id="thCreatedDateTime" style="width: 1%; display: none"></th></tr></thead>';
    tbody.append(tr);

    sessionStorage.setItem("Rx_CurrEncID", hdnCurEncounterID.value);
    sessionStorage.setItem("Rx_UserRole", hdnUserRole.value);
    $.ajax({
        type: "POST",
        url: "frmRxHistory.aspx/LoadRxHistory",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function success(data) {
            
            var currentprocess = hdnCurProcess.value;

            var Result = JSON.parse(data.d);
            var Druglist = Result.DrugList;
            var FrqList = Result.FreqList;
            var Medlst = Result.Medlst;
            var DrugPanel = $("#DrugPanel");
            j = 0;
            for (var i = 0; i < Druglist.length; i++) {
                drug_dts[i] = "Drug_Name:" + Druglist[i].split('~')[0] + "$Route_of_Admin:" + Druglist[i].split('~')[1] + "$Strength:" + Druglist[i].split('~')[2];
                if (drugs.indexOf(Druglist[i].split('~')[0].toUpperCase()) == -1) {
                    drugs[j] = Druglist[i].split('~')[0].toUpperCase();
                    j++;
                }
            }
            if (sessionStorage.getItem("Rx_UserRole").toString().toUpperCase() == "PHYSICIAN" ||
                sessionStorage.getItem("Rx_UserRole").toString().toUpperCase() == "PHYSICIAN ASSISTANT") {
                var p = '<p onclick="OpenFrequentUsedDrugs(\'Drug Name\');" style="font-weight:bold;background-color:#BFDBFF !important" >Click here to open Frequently Used Drugs</p>';
                DrugPanel.append(p);
            }
          
            if (currentprocess != "DICTATION_REVIEW" && currentprocess != "PHYSICIAN_CORRECTION" && currentprocess != "PROVIDER_PROCESS" && currentprocess != "MA_REVIEW" && currentprocess != "MA_PROCESS" && currentprocess != "") {
                $('#btnAdd')[0].disabled = true;
                $('#btnClear')[0].disabled = true;
                $('#tblMedicationDetails')[0].disabled = true;
                $('#mainContainer ').find(':input').prop('disabled', true);
                $('#mainContainer ').find('textarea').prop('disabled', true);
                $('a').attr("disabled", true);
                $('a').attr("onclick", "return false;");
                $('a').css("background-color", "#6D7777 !important");

                $('#mainContainer ').find('select').prop('disabled', true);
                          
                if (Medlst.length > 0) {
                    if (tbody == undefined) {
                        var tbody = $("#tRxMed");
                        $("#tRxMed").empty();
                        var tr = '<thead class="Gridheaderstyle"><tr><th id="thDelete" style="width: 4%;">Del.</th><th id="thClose" style="width: 4.5%;">Stop</th><th id="thDrugName" style="width: 15%;">Drug Name</th><th id="thStrength" style="width: 14%;">Strength</th><th id="thFrequency" style="width: 13%;">Frequency</th> <th id="thFrmDt" style="width: 10%;">From Date</th><th id="thToDt" style="width: 10%;">To Date</th><th id="thRoa" style="width: 15%;">Route of Administration</th>  <th id="thNotes" style="width: 15%;">Notes</th><th id="thID" style="width: 1%; display: none"></th><th id="thVersion" style="width: 1%; display: none"></th><th id="thEncID" style="width: 1%; display: none"></th><th id="thStopNotes" style="width: 1%; display: none"></th><th id="thRetainNotes" style="width: 1%; display: none"></th><th id="thStatus" style="width: 1%; display: none"></th><th id="thCreatedBy" style="width: 1%; display: none"><th id="thCreatedDateTime" style="width: 1%; display: none"></th></tr></thead>';
                        tbody.append(tr);
                    }
                    tbody.append('<tbody style="word-wrap: break-word;">');
                    for (var i = 0; i < Medlst.length; i++) {
                        var fromdate = "";
                        var todate = "";
                        var Fill_Date = "";
                        if (Medlst[i].From_dt.indexOf("0001") <= -1) {
                            fromdate = Medlst[i].From_dt;
                        }

                        if (Medlst[i].To_Dt.indexOf("0001") <= -1) {
                            todate = Medlst[i].To_Dt;
                        }
                        if (Medlst[i].Fill_Date.indexOf("0001") <= -1) {
                            Fill_Date = Medlst[i].Fill_Date;
                        }
                        if (Convert_to_Date(Medlst[i].To_Dt) == "CURRENT")
                            var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + ' onclick="rowclick(this)"><td style="width:4%;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:red;font-size:23px;width:23px;" onclick="StopDetailMedication(this);"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                        else
                            var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + '><td style="width:4%;"><img src="Resources/Delete-Grey.png"/></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:grey;font-size:23px;width:23px;"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                        tbody.append(tr);
                    }
                    tbody.append('</tbody>');
                }
              
                $("#tRxMed").css('width', '99%');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                

            } else {
                var tbody = $("#tRxMed");
                $("#tRxMed").empty();
                var tr = '<thead  class="Gridheaderstyle"><tr><th id="thDelete" style="width: 4%;">Del.</th><th id="thClose" style="width: 4.5%;">Stop</th><th id="thDrugName" style="width: 15%;">Drug Name</th><th id="thStrength" style="width: 14%;">Strength</th><th id="thFrequency" style="width: 13%;">Frequency</th> <th id="thFrmDt" style="width: 10%;">From Date</th><th id="thToDt" style="width: 10%;">To Date</th><th id="thRoa" style="width: 15%;">Route of Administration</th>  <th id="thNotes" style="width: 15%;">Notes</th><th id="thID" style="width: 1%; display: none"></th><th id="thVersion" style="width: 1%; display: none"></th><th id="thEncID" style="width: 1%; display: none"></th><th id="thStopNotes" style="width: 1%; display: none"></th><th id="thRetainNotes" style="width: 1%; display: none"></th><th id="thStatus" style="width: 1%; display: none"></th><th id="thCreatedBy" style="width: 1%; display: none"><th id="thCreatedDateTime" style="width: 1%; display: none"></th></tr></thead>';
                tbody.append(tr);
                tbody.append('<tbody style="word-wrap: break-word;">');
                for (var i = 0; i < Medlst.length; i++) {
                    var fromdate = "";
                    var todate = "";
                    var Fill_Date = "";
                    if (Medlst[i].From_dt.indexOf("0001") <= -1) {
                        fromdate = Medlst[i].From_dt;
                    }
                    if (Medlst[i].Fill_Date.indexOf("0001") <= -1) {
                        Fill_Date = Medlst[i].Fill_Date;
                    }
                    if (Medlst[i].To_Dt.indexOf("0001") <= -1) {
                        todate = Medlst[i].To_Dt;
                    }
                    if (Convert_to_Date(Medlst[i].To_Dt) == "CURRENT")
                        var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + ' onclick="rowclick(this)"><td style="width:4%;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:red;font-size:23px;width:23px;" onclick="StopDetailMedication(this);"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                    else
                        var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + '><td style="width:4%;"><img src="Resources/Delete-Grey.png"/></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:grey;font-size:23px;width:23px;"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                    tbody.append(tr);
                }
                tbody.append('</tbody>');
                $("#tRxMed").css('width', '99%');
                scrolify_RxHis($('#tRxMed'), "100%");
                
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (sessionStorage.getItem("Rx_UserRole").toString().toUpperCase() == "MEDICAL ASSISTANT") {
                    $("#btnReconcile")[0].disabled = true;
                }
            }
            $("input[mand=Yes]").addClass('MandLabelstyle');
            $("span[mand=Yes]").each(function () {
                $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
            });
            $("textarea[id *= txtDLC]").removeClass('DlcClass');

            $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
            $("[id*=pbaddnotes]").addClass('pbDropdownBackground');
            $("[id*=pbnotes]").addClass('pbDropdownBackground');
        },
        error: function onerror(xhr) {
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }

    });
    $.ajax({
        type: "GET",
        url: "ConfigXML/staticlookup.xml",
        dataType: "xml",
        success: function (xml) {
            $("#selquantity").append("<option> </option>");
            $("#selfrequency").append("<option> </option>");
            $("#selPharmacist").append("<option> </option>");
            $("#selDirection").append("<option> </option>");
            $("#selrefill").append("<option> </option>");
            $("#selRouteOAdministration").append("<option> </option>");
            $("#selTotQuantity").append("<option> </option>");
            $("#selDayssupply").append("<option> </option>");

            $(xml).find('RxHistory').each(function () {

                if ($(this)[0].attributes[0].nodeValue == "RxHistory_Quantity") {
                    $("#selquantity").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                    $("#selTotQuantity").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }


                else if ($(this)[0].attributes[0].nodeValue == "RxHistory_Frequency") {
                    $("#selfrequency").append("<option value='" + $(this)[0].attributes[2].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }


                else if ($(this)[0].attributes[0].nodeValue == "RxHistory_Direction") {
                    $("#selDirection").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }
                else if ($(this)[0].attributes[0].nodeValue == "RxHistory_Days_Supply") {
                    $("#selDayssupply").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }

                else if ($(this)[0].attributes[0].nodeValue == "RxHistory_Refills") {
                    $("#selrefill").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }

                else if ($(this)[0].attributes[0].nodeValue == "RxHistory_Direction_to_Pharmacist") {
                    $("#selPharmacist").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }

                else if ($(this)[0].attributes[0].nodeValue == "RxHistory_Route_Admistration") {
                    $("#selRouteOAdministration").append("<option value='" + $(this)[0].attributes[1].nodeValue + "'>" + $(this)[0].attributes[1].nodeValue + "</option>");
                }
            });

        }
    });
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function validate(frmDT, toDT, FillDt) {
    var now = new Date();
    var currentdate = now.getUTCFullYear() + '-' + (now.getUTCMonth() + 1) + '-' + now.getUTCDate();
    currentDT = Date.parse(currentdate);
    frmDaTe = Date.parse(frmDT);
    toDaTe = Date.parse(toDT);

    Date.parse(FillDt)
    if (frmDT != "" && frmDT.split('-')[1] != undefined)

        var Fdate = frmDT.split('-')[1].toUpperCase();
    else
        var Fdate = "";

    if (toDT != "" && toDT.split('-')[1] != undefined)
        var Tdate = toDT.split('-')[1].toUpperCase();
    else
        var Tdate = "";

    if (FillDt != "" && FillDt.split('-')[1] != undefined)
        var Fildate = FillDt.split('-')[1].toUpperCase();
    else
        var Fildate = "";

    DOB = top.window.document.getElementsByName("lblPatientStrip")[0].textContent.split('|')[1];

    if (frmDT != "" && frmDT.split('-').length < 3)
    {
        PFSH_SaveUnsuccessful();
        $("#dtpFromDate").focus();
        alert('Please enter valid from date')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;

    }
    else if (toDT != "" && toDT.split('-').length < 3) {
        $("#dtpToDate").focus();
        PFSH_SaveUnsuccessful();
        alert('Please enter valid to date')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        
        return 0;
    }
    else if (FillDt != "" && FillDt.split('-').length < 3) {
        PFSH_SaveUnsuccessful();
        $("#txtlastfilled").focus();
        alert('Please enter valid Last Fill  date')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    else if ((Fdate != "") && (Fdate != 'JAN' && Fdate != 'FEB' && Fdate != 'MAR' && Fdate != 'APR' && Fdate != 'MAY' && Fdate != 'JUN' && Fdate != 'JUL' && Fdate != 'AUG' && Fdate != 'SEP' && Fdate != 'OCT' && Fdate != 'NOV' && Fdate != 'DEC')) {
        PFSH_SaveUnsuccessful();
        $("#dtpFromDate").focus();
        DisplayErrorMessage('460008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    else if ((Tdate != "") && (Tdate != 'JAN' && Tdate != 'FEB' && Tdate != 'MAR' && Tdate != 'APR' && Tdate != 'MAY' && Tdate != 'JUN' && Tdate != 'JUL' && Tdate != 'AUG' && Tdate != 'SEP' && Tdate != 'OCT' && Tdate != 'NOV' && Tdate != 'DEC')) {
        PFSH_SaveUnsuccessful();
        $("#dtpToDate").focus();
        DisplayErrorMessage('460008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    else if ((frmDT.split('-')[2] != undefined) && (((Fdate == 'FEB') && (frmDT.split('-')[0] % 4 == 0) && (frmDT.split('-')[2] > 29)) || ((Fdate == 'FEB') && (frmDT.split('-')[0] % 4 != 0) && (frmDT.split('-')[2] > 28)) || (((Fdate == 'SEP') || (Fdate == 'APR') || (Fdate == 'JUN') || (Fdate == 'NOV')) && (frmDT.split('-')[2] > 30)) || ((frmDT.split('-')[2] > 31)))) {
        PFSH_SaveUnsuccessful();
        $("#dtpFromDate").focus();
        DisplayErrorMessage('460008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;

    }
    else if ((toDT.split('-')[2] != undefined) && (((Tdate == 'FEB') && (toDT.split('-')[0] % 4 == 0) && (toDT.split('-')[2] > 29)) || ((Tdate == 'FEB') && (toDT.split('-')[0] % 4 != 0) && (toDT.split('-')[2] > 28)) || (((Tdate == 'SEP') || (Tdate == 'APR') || (Tdate == 'JUN') || (Tdate == 'NOV')) && (toDT.split('-')[2] > 30)) || ((toDT.split('-')[2] > 31)))) {
        PFSH_SaveUnsuccessful();
        $("#dtpToDate").focus();
        DisplayErrorMessage('460008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");

        return 0;

    }
    else if ((Fildate != "") && (Fildate != 'JAN' && Fildate != 'FEB' && Fildate != 'MAR' && Fildate != 'APR' && Fildate != 'MAY' && Fildate != 'JUN' && Fildate != 'JUL' && Fildate != 'AUG' && Fildate != 'SEP' && Fildate != 'OCT' && Fildate != 'NOV' && Fildate != 'DEC')) {
        PFSH_SaveUnsuccessful();
        $("#txtlastfilled").focus();
        DisplayErrorMessage('460008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    else if ((FillDt.split('-')[2] != undefined) && (((Fildate == 'FEB') && (FillDt.split('-')[0] % 4 == 0) && (FillDt.split('-')[2] > 29)) || ((Fildate == 'FEB') && (FillDt.split('-')[0] % 4 != 0) && (FillDt.split('-')[2] > 28)) || (((Fildate == 'SEP') || (Fildate == 'APR') || (Fildate == 'JUN') || (Fildate == 'NOV')) && (FillDt.split('-')[2] > 30)) || ((FillDt.split('-')[2] > 31)))) {
        PFSH_SaveUnsuccessful();
               $("#txtlastfilled").focus();
        DisplayErrorMessage('460008');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");

        return 0;
    }
    else if ((frmDaTe > toDaTe) && (toDaTe != "")) {
        PFSH_SaveUnsuccessful();
        $("#dtpFromDate").focus();
        DisplayErrorMessage('180002');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    else if ((frmDaTe < DOB) && (frmDaTe != "")) {
        PFSH_SaveUnsuccessful();
        $("#dtpFromDate").focus();
        DisplayErrorMessage('180012');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    else if ((frmDaTe > currentDT) && (frmDaTe != "")) {

        PFSH_SaveUnsuccessful();
        $("#dtpFromDate").focus();
        DisplayErrorMessage('180001');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }

    else if ((toDaTe < DOB) && (toDaTe != "")) {

        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180013');

        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $("#dtpToDate").focus();
        localStorage.setItem("bSave", "false");
        return 0;
    }

    else if ((Date.parse(FillDt) > currentDT) && (FillDt != "")) {
        PFSH_SaveUnsuccessful();
        $("#txtlastfilled").focus();
        alert('Last Filled date cannot be future Date');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        localStorage.setItem("bSave", "false");
        return 0;
    }
    return 1;
}

function saveRx_History(Type, Id, item) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var Stop_Notes = "";
    var Retain_Notes = "";
    var Status = "";
    var CreatedBy = "";
    var CreatedDateTime = "";
    if (Type == "save") {
        var Id = Id;
        var version = 0;
        var encID = 0;
        if ($("#btnAdd")[0].value == "Update") {
            Type = "Update";
            Id = hdnMedID.value;
            version = hdnVersion.value;
            encID = hdnEncID.value;
        }
        var drugName = $("#txtDrugName")[0].value;
        if (drugName == "") {
            DisplayErrorMessage('380054');
            localStorage.setItem("bSave", "false");
            PFSH_SaveUnsuccessful();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return;
        }

        else if (($('#txtlastfilled').val() != "" && $('#dtpFromDate').val() != "") && (new Date($('#txtlastfilled').val()) < new Date($('#dtpFromDate').val()))) {
            alert("Last Filled Date  cannot be lesser than start date")
            localStorage.setItem("bSave", "false");
            PFSH_SaveUnsuccessful();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return;
        }
        else if (($('#dtpToDate').val() != "" && $('#dtpFromDate').val() != "") && (new Date($('#dtpToDate').val()) < new Date($('#dtpFromDate').val()))) {
            alert('To Date cannot be lesser than start date');
            localStorage.setItem("bSave", "false");
            PFSH_SaveUnsuccessful();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return;
        }

        else if (($('#txtlastfilled').val() != "" && $('#dtpToDate').val() != "") && (new Date($('#txtlastfilled').val()) > new Date($('#dtpToDate').val()))) {
            alert('Last Filled Date  cannot be greater than To Date');
            localStorage.setItem("bSave", "false");
            PFSH_SaveUnsuccessful();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return;
        }

        var strength = $(".custom-combobox-input.ui-autocomplete-input.form-control")[0].value;//$("#selStrength option:selected").length > 0 ? $("#selStrength option:selected")[0].value : "";
        var direction = $('#selDirection').val();
        var pharm = $('#selPharmacist').val();

        var dose = $('#txtQuantity').val();
        var dosestren = $('#selquantity').val();

        var quant = $('#txttotalQuantity').val();
        var quantstr = $('#selTotQuantity').val();;
        var frequency = $('#selfrequency option:selected').text();
        var refill = $('#selrefill').val();
        var notes = $('#txtlastfilled').val();
        var lastfill = $('#txtlastfilled').val();
        var dayssupply = $('#selDayssupply').val();
        var additionalnotes = $('#txtAdditionalNotes').val();
        var Route_of_admin = $('.custom-combobox-input.ui-autocomplete-input')[1].value;//$("#selRouteOAdministration option:selected").length > 0 ? $("#selRouteOAdministration option:selected")[0].value : "";
        var fromDt = $("#dtpFromDate").val();
            var todt = $("#dtpToDate").val();
        var notes = $("#txtNotes").val();
        var type = Type;
        var val = validate(fromDt, todt, $('#txtlastfilled').val());
        var pharmacy = "";
        if (pharm.indexOf("Substitution Permitted") > -1) {
            pharmacy = "y";
        }
        else if (pharm.indexOf("Substitution Permitted") <= -1) {
            pharmacy = "n";
        }
        else {
            pharmacy = "";
        }
        var Pharmacy_Notes = $("#txtNotesPhar").val();
    }
    else if (Type == "del") {
        var encID = $(item).parent().parent()[0].children[20].textContent;


        var drugName = $(item).parent().parent()[0].children[2].textContent;
        var strength = $(item).parent().parent()[0].children[3].textContent;
        var frequency = $(item).parent().parent()[0].children[4].textContent;
        var Route_of_admin = $(item).parent().parent()[0].children[7].textContent;
        var fromDt = $(item).parent().parent()[0].children[5].textContent;
        var todt = $(item).parent().parent()[0].children[6].textContent;
        var notes = $(item).parent().parent()[0].children[8].textContent;
        var type = Type;
        var Id = $(item).parent().parent()[0].children[18].textContent;
        var version = $(item).parent().parent()[0].children[19].textContent;
        var val = 1;

        var direction = $(item).parent().parent()[0].children[14].textContent;
        var pharm = $(item).parent().parent()[0].childNodes[21].textContent;
        var refill = $(item).parent().parent()[0].children[15].textContent;
        var dose = $(item).parent().parent()[0].children[12].textContent;
        var dosestren = $(item).parent().parent()[0].children[13].textContent;

        var quant = $(item).parent().parent()[0].children[10].textContent;
        var quantstr = $(item).parent().parent()[0].children[11].textContent;
        var lastfill = $(item).parent().parent()[0].children[17].textContent;
        var dayssupply = $(item).parent().parent()[0].children[16].textContent;
        var additionalnotes = $(item).parent().parent()[0].children[9].textContent;
        var Pharmacy_Notes = $(item).parent().parent()[0].children[22].textContent;
        Stop_Notes = $(item).parent().parent()[0].children[23].textContent;
        Retain_Notes = $(item).parent().parent()[0].children[24].textContent;
        Status = $(item).parent().parent()[0].children[25].textContent;
        CreatedBy = $(item).parent().parent()[0].children[26].textContent;
        CreatedDateTime = $(item).parent().parent()[0].children[27].textContent;
    }
    var pharmacy = "";
    if (pharm.indexOf("Substitution Permitted") > -1) {
        pharmacy = "y";
    }
    else if (pharm.indexOf("Substitution Permitted") <= -1) {
        pharmacy = "n";
    }
    else {
        pharmacy = "";
    }
    if (val == 1) {

        if (todt == "")
            todt = "0001-01-01";
        var obj = {
            DrugName: drugName,
            ROA: Route_of_admin,
            Strength: strength,
            Frequency: frequency,
            From_Date: fromDt,
            To_Date: todt,
            direction: direction,
            dose: dose,
            dosestren: dosestren,
            quant: quant,
            quantstr: quantstr,
            refill: refill,
            additionalnotes: additionalnotes,
            Notes: notes,
            Type: type,
            dayssupply: dayssupply,
            lastfill: lastfill,
            ID: Id,
            pharmacy: pharmacy,
            Pharmacy_Notes: Pharmacy_Notes,
            Version: version,
            EncID: encID,
            stopnotes: Stop_Notes,
            retainnotes: Retain_Notes,
            status: Status,
            createdby: CreatedBy,
            createddatetime:CreatedDateTime
        };


        $.ajax({
            type: "POST",
            url: "frmRxHistory.aspx/SaveRxHistory",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ data: obj }),
            datatype: "json",
            success: function success(data) {
               
                var result = JSON.parse(data.d);
                var Medlst = result.Medlst;
                var MedTooltip = result.Medtooltip;
                $('#tRxMed tr').remove();
                var tbody = $("#tRxMed");

                var tr = '<thead class="Gridheaderstyle"><tr><th id="thDelete" style="width: 4%;">Del.</th><th id="thClose" style="width: 4.5%;">Stop</th><th id="thDrugName" style="width: 15%;">Drug Name</th><th id="thStrength" style="width: 14%;">Strength</th><th id="thFrequency" style="width: 13%;">Frequency</th> <th id="thFrmDt" style="width: 10%;">From Date</th><th id="thToDt" style="width: 10%;">To Date</th><th id="thRoa" style="width: 15%;">Route of Administration</th>  <th id="thNotes" style="width: 15%;">Notes</th><th id="thID" style="width: 1%; display: none"></th><th id="thVersion" style="width: 1%; display: none"></th><th id="thEncID" style="width: 1%; display: none"></th><th id="thStopNotes" style="width: 1%; display: none"></th><th id="thRetainNotes" style="width: 1%; display: none"></th><th id="thStatus" style="width: 1%; display: none"></th><th id="thCreatedBy" style="width: 1%; display: none"><th id="thCreatedDateTime" style="width: 1%; display: none"></th></tr></thead>';
                tbody.append(tr);
                tbody.append('<tbody style="word-wrap: break-word;">');
                for (var i = 0; i < Medlst.length; i++) {
                    var fromdate = "";
                    var todate = "";
                    var Fill_Date = "";
                    if (Medlst[i].From_dt.indexOf("0001") <= -1) {
                        fromdate = Medlst[i].From_dt;
                    }

                    if (Medlst[i].To_Dt.indexOf("0001") <= -1) {
                        todate = Medlst[i].To_Dt;
                    }
                    if (Medlst[i].Fill_Date.indexOf("0001") <= -1) {
                        Fill_Date = Medlst[i].Fill_Date;
                    }
                    if (Convert_to_Date(Medlst[i].To_Dt) == "CURRENT")
                        var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + ' onclick="rowclick(this)"><td style="width:4%;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:red;font-size:23px;width:23px;" onclick="StopDetailMedication(this);"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                    else
                        var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + '><td style="width:4%;"><img src="Resources/Delete-Grey.png"/></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:grey;font-size:23px;width:23px;"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                    tbody.append(tr);
                }
                tbody.append('</tbody>');
                $("#tRxMed").css('width', '99%');
                scrolify_RxHis($('#tRxMed'), "275px");
                $("#dtpToDate").removeAttr("disabled");
                $("#dtpToDate").css("background-color", "white");
                $('#btnAdd')[0].disabled = true;
                if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)

                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
                else
                    window.parent.theForm.hdnSaveEnable.value = false;

                EnablePFSH(result[0]);
                $("#btnAdd")[0].value = "Add";
                $("#btnClear")[0].value = "Clear All";
                ClearAll();
                var regex = /<BR\s*[\/]?>/gi;
                top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = MedTooltip[0];
                top.window.document.getElementById("Medication_tooltp").innerText = MedTooltip[1].replace(regex, "\n") + "\n";
                RefreshOverallSummaryTooltip();
                if (Type == "save" && sessionStorage.getItem("Rx_UserRole").toString().toUpperCase() != "MEDICAL ASSISTANT") {
                    loadMedReconcile('ADD');
                }
                else {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }

                SavedSuccessfully();
            },
            error: function onerror(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
       
    }
}

function setTolquantityunit() {

    $('#selTotQuantity').val($('#selquantity option:selected').val());
}
function CalculateTotal() {

    if ($('#selDayssupply').val() != "" && $('#selfrequency option:selected').val() != "" && $('#txtQuantity').val() != "") {

        var totalquant = parseFloat($('#selDayssupply').val().trim()) * parseFloat($('#selfrequency option:selected').val().trim()) * parseFloat($('#txtQuantity').val().trim());


        $('#txttotalQuantity').val(Math.ceil(parseFloat(totalquant)));
    }
    else {
        $('#txttotalQuantity').val("");

    }
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;
}



function Update(item) {
    
    $("#txtDrugName").val($(item).parent().parent()[0].children[2].textContent);
    fillDrugROA($("#txtDrugName").val());

    $(".custom-combobox-input.ui-autocomplete-input.form-control")[0].value = $(item).parent().parent()[0].children[3].textContent;
      
 $(".custom-combobox-input.ui-autocomplete-input.form-control")[1].value = $(item).parent().parent()[0].children[7].textContent;

   
    var dd = document.getElementById('selfrequency');
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text.toUpperCase() === $(item).parent().parent()[0].children[4].textContent.toUpperCase()) {
            dd.selectedIndex = i;
            break;
        }
    }

    $("#dtpFromDate").val($(item).parent().parent()[0].children[5].textContent);
    if ($(item).parent().parent()[0].children[6].textContent == "Current") {
        $("#dtpToDate").val("");
        $("#dtpToDate").attr("disabled", "disabled");
    }
    else {
        $("#dtpToDate").val($(item).parent().parent()[0].children[6].textContent);
        $("#dtpToDate").removeAttr("disabled");
    }

    $("#txtNotes").val($(item).parent().parent()[0].children[8].textContent);

    $("#selDirection").val($(item).parent().parent()[0].children[14].textContent);
    $("#selDayssupply").val($(item).parent().parent()[0].children[16].textContent.trim());
    $("#selrefill").val($(item).parent().parent()[0].children[15].textContent);
    $("#txtlastfilled").val($(item).parent().parent()[0].children[17].textContent);
    $("#txtQuantity").val($(item).parent().parent()[0].children[12].textContent);


    var dd = document.getElementById('selquantity');
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text.toUpperCase() === $(item).parent().parent()[0].children[13].textContent.toUpperCase()) {
            dd.selectedIndex = i;
            break;
        }
    }

    $("#txttotalQuantity").val($(item).parent().parent()[0].children[10].textContent);

  
    var dd = document.getElementById('selTotQuantity');
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text.toUpperCase() === $(item).parent().parent()[0].children[11].textContent.toUpperCase()) {
            dd.selectedIndex = i;
            break;
        }
    }

    $("#txtAdditionalNotes").val($(item).parent().parent()[0].children[9].textContent);
    hdnMedID.value = $(item).parent().parent()[0].children[18].textContent;
    hdnVersion.value = $(item).parent().parent()[0].children[19].textContent;
    hdnEncID.value = $(item).parent().parent()[0].children[20].textContent;
    $("#btnAdd")[0].value = "Update";
    $("#btnAdd").removeClass("disabled");
    $("#btnAdd")[0].disabled = false;
    $("#btnClear")[0].value = "Cancel";
    if ($(item).parent().parent()[0].children[21].textContent.toUpperCase() == "Y")
        $("#selPharmacist").val($("#selPharmacist option")[1].value);
    else if ($(item).parent().parent()[0].children[21].textContent.toUpperCase() == "")
        $("#selPharmacist").val("");
    else {
        $("#selPharmacist").val($("#selPharmacist option")[2].value);
    }

    $("#txtNotesPhar").val($(item).parent().parent()[0].children[22].textContent);
    EnableSave();
}
var delitem;
var cnt = 0;
function Delete(item) {
    
    if (cnt == 0) {
        delitem = item;
    }

    cnt++;
    var del = DisplayErrorMessage('110021');
    if (del == true) {
        cnt = 0;
        saveRx_History("del", 0, delitem);
    }
}
function Save() {
    saveRx_History("save", 0, null);
}
function EnablePFSH(val) {

    if ($(window.parent.document).find('#btnPFSHVerified') != null)
        $(window.parent.document).find('#btnPFSHVerified')[0].disabled = false;

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    var bValue = true;
    var PFSHVerified = localStorage.getItem("PFSHVerified");
    if (PFSHVerified != "") {
        var PFSH = PFSHVerified.split('|');
        for (var i = 0; i < PFSH.length; i++) {
            if (PFSH[i].split('-')[0] == val) {
                PFSHVerified = PFSHVerified.replace(PFSH[i], val + "-" + "TRUE");
                bValue = false;
            }
        }
    }
    if (bValue == true)
        PFSHVerified = PFSHVerified + "|" + val + "-" + "TRUE";
    localStorage.setItem("PFSHVerified", PFSHVerified);
}
function SavedSuccessfully() {
    localStorage.setItem("bSave", "true");
    DisplayErrorMessage('180602');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
}

function GetFrequencies(icon, List) {

    $('#btnAdd')[0].disabled = false;
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('.fa-minus').addClass('fa-plus').removeClass('fa-minus');
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");

        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#btnAdd')[0].disabled = false;
                var values = response.d.split("|");
                var targetControlValue = $(icon).parent().siblings().prop('id');
                var innerdiv = '';
                var txtPos = $('#' + targetControlValue).position();
                var pos = $('#' + targetControlValue).position();
                var topPosition = txtPos.top;
                if (sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN" || sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN ASSISTANT") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"AddItem('" + values[i] + "^" + targetControlValue + "');\">" + values[i] + "</li>";
                }
                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }

                    $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                      .css({
                          top: topPosition + $(".actcmpt").height() + 15,
                          left: pos.left,
                          width: pos.width,
                          height: '100px',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '283px',
                          border: '1px solid #8e8e8e',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto',
                          'border-radius': '2px'
                          //CAP-804 Syntax error, unrecognized expression
                      }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                }
                EnableSave();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");
    }
}

function AddItem(agrulist) {

    var sugglistval;
    var control;
    var value = agrulist.split("^");
    if (value.length > 2) {
        //CAP-804 Syntax error, unrecognized expression
        control = value[5]?.trim();
        sugglistval = $("#" + control + ".actcmpt").val().trim();
        var selectedvalue = value[0] + ',' + value[1] + ',' + value[2] + ',' + value[3] + ',' + value[4];
        if (sugglistval != " " && sugglistval != "") {
            var subsugglistval = sugglistval;
            var len = subsugglistval.length;
            var flag = 0;
            if (subsugglistval == selectedvalue) {
                flag++;
            }
            if (flag == 0) {
                $("#" + control + ".actcmpt").val(sugglistval + "," + selectedvalue);
            }
        }
        else {
            $("#" + control + ".actcmpt").val(selectedvalue);
        }
    }
    else {
        $("#" + value[1]?.trim() + ".actcmpt").val(value[0]);
      
    }


}

function OpenPopup(Keyword) {
    var focused = Keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}

var bBool = false;
var bcheck = true;
var intCPTLength = -1;
var drugarry = [];
var drugarryUnique = [];

$("#txtDrugName").autocomplete({
    source: function (request, response) {
        if (intCPTLength == 0 && bcheck && bBool == false) {
            bBool = true;
            drugarry = [];
            $.ajax({
                type: "POST",
                url: "frmRxHistory.aspx/LoadDrugsDetails",
                data: '{fieldName: "' + $("#txtDrugName").val() + '"}',
                contentType: "application/json;charset=utf-8",
                datatype: "json",
                success: function success(data) {
                    var jsonData = JSON.parse(data.d);
                    if (jsonData.length == 0) {
                        jsonData.push('No matches found.')
                        response($.map(jsonData, function (item) {
                            return {
                                label: item
                            }
                        }));
                    }
                    else {
                        for (var i = 0; i < jsonData.length; i++) {
                            drug_dts[i] = "Drug_Name:" + jsonData[i].split('~')[0] + "$Route_of_Admin:" + jsonData[i].split('~')[1] + "$Strength:" + jsonData[i].split('~')[2];
                            drugarry.push(jsonData[i].split('~')[0]);
                        }
                        for (i = 0; i < drugarry.length; i++) {
                            if (drugarryUnique.indexOf(drugarry[i].toUpperCase()) === -1) {
                                drugarryUnique.push(drugarry[i].toUpperCase());
                            }
                        }
                        if (drugarryUnique.length != 0) {
                            var results = PossibleCombination(drugarryUnique, request.term);
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
                                        label: item
                                    }
                                }));
                            }
                        }
                    }
                    $("#txtDrugName").focus();
                    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                },
                error: function onerror(xhr) {
                    if (xhr.status == 999)
                        window.location = xhr.statusText;
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                    }
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
        }
        if ($("#txtDrugName").val().length > 3) {
            if (drugarryUnique.length != 0) {
                var results = PossibleCombination(drugarryUnique, request.term);
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
                            label: item
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
        $('.ui-menu').width(295);
        $('.ui-menu').height(200);
        $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
        });
    },
    select: function (event, ui) {
        event.preventDefault();
        if (ui.item.label != "No matches found.") {
            bBool = false;
            $('#txtDrugName').val(ui.item.label);
            fillDrugROA(ui.item.label);

        }
        else {
            bBool = false;
            $('#txtDrugName').val("");
        }
    }
}).on("paste", function (e) {
    intCPTLength = -1;
    drugarry = [];
    drugarryUnique = [];
    $(".ui-autocomplete").hide();
}).on("keydown", function (e) {
    if (e.which == 8) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtDrugName").val().length <= 3)
            bBool = false;
        else
            bBool = true;
        $("#txtDrugName").focus();
        bcheck = false;
    }
    else if (e.which == 46) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        bBool = false;
        bcheck = false;
    }
    else {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        bcheck = true;
    }

}).on("input", function (e) {
    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if ($("#txtDrugName").val().length >= 3) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (!bBool)
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        intCPTLength = 0;
    }
    else if ($("#txtDrugName").val().length != 0 && intCPTLength != -1) {
        intCPTLength = intCPTLength + 1;
    }
    if ($("#txtDrugName").val().length < 3) {
        intCPTLength = -1;
        drugarry = [];
        drugarryUnique = [];
        $(".ui-autocomplete").hide();
        bBool = false;
    }
});

function PossibleCombination(array, txtValue) {
    aICDDesc = txtValue.split(" ");
    var ResultArray = array;
    var SearchValues;
    for (var i = 0; i < aICDDesc.length; i++) {
        SearchValues = $.grep(ResultArray, function (ResulttxtDesc) {
            return ResulttxtDesc.toLowerCase().indexOf(aICDDesc[i].toLowerCase()) > -1;
        });
        ResultArray = SearchValues;
    }
    return ResultArray;
}

function StopDetailMedication(item) {

    
    var encID = "";
    var Id = "";
    var versionid = "";
    if ($(item).parent().parent()[0].children[18] != undefined)
        Id = $(item).parent().parent()[0].children[18].textContent;
    if ($(item).parent().parent()[0].children[2] != undefined)
        sessionStorage.setItem("DrugName", $(item).parent().parent()[0].children[2].textContent);
    if ($(item).parent().parent()[0].children[20] != undefined)
        encID = $(item).parent().parent()[0].children[20].textContent;
    if ($(item).parent().parent()[0].children[19] != undefined)
        versionid = $(item).parent().parent()[0].children[19].textContent;
    $(top.window.document).find('#StopMedicationmodal').modal({ backdrop: 'static', keyboard: false }, 'show');
    var sPath = ""
    sPath = "HtmlStopMedication.html?version=" + sessionStorage.getItem("ScriptVersion") + "&encID=" + encID + "&Id=" + Id + "&versionid=" + versionid;
    $(top.window.document).find("#StopMedicationmodaldlg")[0].style.width = "403px";
    $(top.window.document).find("#StopMedicationFrame")[0].style.height = "197px";
    $(top.window.document).find("#StopMedicationmodal")[0].style.width = "";
    $(top.window.document).find('#StopMedicationFrame')[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#StopMedicationModalTitle")[0].textContent = "Stop Medication";
}

function OpenReconcile() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#btnAdd")[0].disabled == false) {
        autoSave();
    }
    else {
        loadMedReconcile('');
    }
}

function autoSave() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $("#btnAdd").click();
    loadMedReconcile('');
    //$(dvdialogRxHistory).dialog({
    //    modal: true,
    //    title: "Capella EHR",
    //    position: {
    //        my: 'center',
    //        at: 'top + 50px'
    //    },
    //    buttons: {
    //        "Yes": function () {
    //            $(dvdialogRxHistory).dialog("close");
    //            $("#btnAdd").click();
    //            loadMedReconcile('');
    //        },
    //        "No": function () {
    //            $(dvdialogRxHistory).dialog("close");
    //            ClearAll();
    //            loadMedReconcile('');
    //        },
    //        "Cancel": function () {
    //            EnableSave();
    //            $(dvdialogRxHistory).dialog("close");
    //        }
    //    }
    //});
}
function reloadTable(Medlst) {
    var tbody = $("#tRxMed");
    $("#tRxMed").empty();
    var tr = '<thead class="Gridheaderstyle" ><tr><th id="thDelete" style="width: 4%;">Del.</th><th id="thClose" class="Gridheaderstyle" style="width: 4.5%;">Stop</th>' +
            '<th id="thDrugName" style="width: 15%;">Drug Name</th><th id="thStrength" style="width: 14%;">Strength</th>' +
            '<th id="thFrequency" style="width: 13%;">Frequency</th> <th id="thFrmDt" style="width: 10%;">From Date</th>' +
            '<th id="thToDt" style="width: 10%;">To Date</th><th id="thRoa" style="width: 15%;">Route of Administration</th>' +
            '<th id="thNotes" style="width: 15%;">Notes</th><th id="thID" style="width: 1%; display: none"></th>' +
            '<th id="thVersion" style="width: 1%; display: none"></th><th id="thEncID" style="width: 1%; display: none"></th>' +
            '<th id="thStopNotes" style="width: 1%; display: none"></th><th id="thRetainNotes" style="width: 1%; display: none">' +
            '<th id="thStatus" style="width: 1%; display: none"></th><th id="thCreatedBy" style="width: 1%; display: none">' +
            '<th id="thCreatedDateTime" style="width: 1%; display: none"></th></tr></thead>';
    tbody.append(tr);
    for (var i = 0; i < Medlst.length; i++) {
        if (Convert_to_Date(Medlst[i].To_Dt) == "CURRENT")
            var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + ' onclick="rowclick(this)"><td style="width:4%;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:red;font-size:23px;width:23px;" onclick="StopDetailMedication(this);"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
        else
            var tr = '<tr style="height: 25px!important;" class=Gridbodystyle ' + Convert_to_Date(Medlst[i].To_Dt) + '><td style="width:4%;"><img src="Resources/Delete-Grey.png"/></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:grey;font-size:23px;width:23px;"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
        tbody.append(tr);
    }
}