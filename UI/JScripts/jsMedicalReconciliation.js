var interaction_colors = ["#00A27B", "#0863B2", "#FF8444", "#B5D163", "#FF4E44", "#B23A4C", "#FFB812", "#FF91AC", "#976CFF", "#BF7630", "#00CCCB", "#3F00BF", "#B200B2"];
var i;
var Messagenotes = new Array();
$(document).ready(function () {
   
    i = 0;
    
        var tblDrugBody = $("#tblDrugBody tbody");
        tblDrugBody.append(sessionStorage.getItem("RowstoAppend"));
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        setSaveDisabled();
        $('textarea').attr('disabled', true);
        $('a').attr("disabled", true).attr("onclick", "return false;").css("background-color", "#6D7777 !important");
        $(".datecontrol").mask("9999?-aaa-99");
});

$(top.window.document).find("#btnClose")[0].hidden = true;
function SavetoRxHistory() {

    if ($("#tblDrugInteraction tbody tr").length > 0) {
        DisplayErrorMessage('5600');
    }
    else if ($("#tblDrugDuplicate tbody tr").length != 0) {
        DisplayErrorMessage('5601');
    }
    else {
        var reloadtable = 0;
        var Medlst;
        if ($("#tblDrugList tbody tr").length > 0) {//only for notes enabled fields
            var tr = $("#tblDrugList tbody tr").find("span:enabled");
            var namelst = [];

            var data = [];

            for (var j = 0; j < tr.length; j++) {
                var name = tr[0].parentNode.parentNode.attributes.getNamedItem("name").value;
                if (namelst.indexOf(name) == -1)
                    namelst.push(name);
            }
            for (var i = 0; i < namelst.length; i++) {
                var trnode = document.getElementsByName(namelst[i]);
                for (var k = 0; k < trnode.length; k++) {
                    var rows = [];
                    if (k == 0) {
                        rows.push(trnode[k].childNodes[3].innerText);
                        rows.push(trnode[0].childNodes[2].firstChild.value);
                    }
                    else {
                        rows.push(trnode[k].childNodes[1].innerText);
                        rows.push(trnode[0].childNodes[2].firstChild.value);
                    }
                    data.push(rows);
                }

            }

            $.ajax({
                type: "POST",
                url: "frmRxHistory.aspx/SavetoRxHistory",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({ data: data }),
                datatype: "json",
                success: function success(data) {
                    Medlst = JSON.parse(data.d);
                    DisplayErrorMessage('220005');

                    $(top.window.document).find("#btnClose").click();
                    reloadTable(Medlst);
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
            $(top.window.document).find("#btnClose").click();
        }
    }

}
var hdnFieldName = null;
function callweb(icon, List, id) {
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
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:44642
                if (sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN" || sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN ASSISTANT") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default;text-align: left;' onclick=\"openAddorUpdate('" + ListValue + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default;text-align: left;' onclick=\"fun('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";
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
                          top: pos.top + 51,
                          left: pos.left,
                          height: '64px',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '164px',
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
function openAddorUpdate(field_name) {
    var field_name = field_name;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + field_name;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}
function fun(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BugID:44641
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
function StopRetainDetails(id, EncounterId, StopNotes, RetainNotes, StopDate) {
    this.id = id;
    this.EncounterId = EncounterId;
    this.StopNotes = StopNotes;
    this.RetainNotes = RetainNotes;
    this.StopDate = StopDate;
    this.Type = "del";

}
function Cancel() {
    if ($("#btnSave")[0].disabled == false) {
        autoSave();
    }
    else {
        $(top.window.document).find("#btnClose").click();
        reloadTableRx_History();
    }
}

function submit() {
    var stoparray = new Array();
    var retainarray = new Array();
    var CalloutAnnotation_Collection = new Array();
    var flag = 0;
    $('#tblDrugBody > tbody  > tr').each(function () {
        $this = $(this);
        if ($this[0].childNodes.length > 1) {
            if ($this[0].childNodes[0].childNodes.length != 0 && !($this[0].childNodes[0].childNodes[0].checked || $this[0].childNodes[1].childNodes[0].checked)) {
                DisplayErrorMessage('3001');
                flag = flag + 1;
                return false;
            }
            if ($this[0].childNodes[0].childNodes.length != 0 && $this[0].childNodes[0].childNodes[0].checked && $this[0].childNodes[5].childNodes[0].value == "") {
                DisplayErrorMessage('3002','', $this[0].childNodes[2].textContent.toString());
                $this[0].childNodes[5].childNodes[0].focus();
                flag = flag + 1;
                return false;
            }
            if ($this[0].childNodes[0].childNodes.length != 0 && $this[0].childNodes[0].childNodes[0].checked && $this[0].childNodes[6].childNodes[0].value == "") {
                DisplayErrorMessage('3003','', $this[0].childNodes[2].textContent.toString());
                $this[0].childNodes[6].childNodes[0].focus();
                flag = flag + 1;
                return false;
            }
            if ($this[0].childNodes[1].childNodes.length != 0 && $this[0].childNodes[1].childNodes[0].checked && $this[0].childNodes[6].childNodes[0].value == "") {
                //alert("Please Enter Stop/Retain Notes " + $this[0].childNodes[2].textContent);
                DisplayErrorMessage('3003','', $this[0].childNodes[2].textContent.toString());
                $this[0].childNodes[6].childNodes[0].focus();
                flag = flag + 1;
                return false;
            }
        }
    });
   
    if (flag == 0) {
        $('#tblDrugBody > tbody  > tr').each(function () {
            $this = $(this);
            if ($this[0].childNodes.length > 1) {
                if ($this[0].childNodes[0].childNodes.length != 0 && $this[0].childNodes[0].childNodes[0].checked) {
                    var StopRetainDetailsarray = new StopRetainDetails($this[0].attributes.getNamedItem("medhis_ID").value, sessionStorage.getItem("Rx_CurrEncID"), $this[0].childNodes[6].childNodes[0].value, "", $this[0].childNodes[5].childNodes[0].value);

                    stoparray.push(StopRetainDetailsarray);

                }
                else {
                    var StopRetainDetailsarray = new StopRetainDetails($this[0].attributes.getNamedItem("medhis_ID").value, sessionStorage.getItem("Rx_CurrEncID"), "", $this[0].childNodes[6].childNodes[0].value, "");

                    retainarray.push(StopRetainDetailsarray);

                }
            }

        });


        if (stoparray.length > 0 || retainarray.length > 0) {
            var WS_Data = {
                StopHistory: stoparray,
                RetainHistory: retainarray

            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "frmRxHistory.aspx/StopRetainRXConsile",
                data: JSON.stringify(WS_Data),
                dataType: "json",
                success: function (data) {
                    var jsonData = data.d;
                    SavedSuccessfully();
                    RefreshNotification('RxHistory');
                    Cancel();
                    reloadTableRx_History();
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
    }

}
function reloadTableRx_History() {
    $.ajax({
        type: "POST",
        url: "frmRxHistory.aspx/LoadRxHistory",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        success: function success(data) {
            
            var Result = JSON.parse(data.d);
            var Medlst = Result.Medlst;
            var tRxbody = $($($($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active")).find("iframe")[0].contentDocument).find(".tab-pane.active").find("iframe")[0].contentDocument.activeElement).find(".table-bordered");
            $(tRxbody).find("tr").remove();
            var tbody = $($($($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active")).find("iframe")[0].contentDocument).find(".tab-pane.active").find("iframe")[0].contentDocument.activeElement).find("#tRxMed");
            var tr = '<thead><tr><th id="thDelete" style="width: 4%;">Del.</th><th id="thClose" style="width: 4.5%;">Stop</th>' +
          '<th id="thDrugName" style="width: 15%;">Drug Name</th><th id="thStrength" style="width: 14%;">Strength</th>' +
          '<th id="thFrequency" style="width: 13%;">Frequency</th> <th id="thFrmDt" style="width: 10%;">From Date</th>' +
          '<th id="thToDt" style="width: 10%;">To Date</th><th id="thRoa" style="width: 15%;">Route of Administration</th>' +
          '<th id="thNotes" style="width: 15%;">Notes</th><th id="thID" style="width: 1%; display: none"></th>' +
          '<th id="thVersion" style="width: 1%; display: none"></th><th id="thEncID" style="width: 1%; display: none"></th>' +
          '<th id="thStopNotes" style="width: 1%; display: none"></th><th id="thRetainNotes" style="width: 1%; display: none">' +
          '<th id="thStatus" style="width: 1%; display: none"></th><th id="thCreatedBy" style="width: 1%; display: none">' +
          '<th id="thCreatedDateTime" style="width: 1%; display: none"></th></tr></thead>';
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
                    var tr = '<tr style="height: 25px!important;font-size: 8.9pt!important;background-color:#D4FBA8;" class=' + Convert_to_Date(Medlst[i].To_Dt) + ' onclick="rowclick(this)"><td style="width:4%;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:red;font-size:23px;width:23px;" onclick="StopDetailMedication(this);"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                else
                    var tr = '<tr style="height: 25px!important;font-size: 8.9pt!important;background-color:#e0dede;" class=' + Convert_to_Date(Medlst[i].To_Dt) + '><td style="width:4%;"><img src="Resources/Delete-Grey.png"/></td><td style="width:4.5%;"><span class="glyphicon glyphicon-ban-circle" style="color:grey;font-size:23px;width:23px;"></span></td><td style="width: 15%;">' + Medlst[i].DrugName + '</td><td style="width: 14%;">' + Medlst[i].Strength + '</td><td style="width: 13%;">' + Medlst[i].Frequency + '</td><td style="width: 10%;">' + fromdate + '</td><td style="width: 10%;">' + todate + '</td><td style="width: 15%;">' + Medlst[i].Route_of_admin + '</td><td style="width: 15%;word-break: break-word;">' + Medlst[i].Notes + '</td></td> <td style="width: 1%;display:none">' + Medlst[i].AdditionalNotes + ' </td>  <td style="width: 1%;display:none"> ' + Medlst[i].Quantity + '</td><td style="display:none">' + Medlst[i].Quantity_Unit + '</td> <td style="display:none">' + Medlst[i].dose + '</td> <td style="display:none">' + Medlst[i].dose_unit + '</td><td style="display:none">' + Medlst[i].Direction + '</td> <td style="display:none">' + Medlst[i].Refill + '</td> <td style="display:none"> ' + Medlst[i].dayssupply + '</td><td style="display:none">' + Fill_Date + '</td> <td style="display:none">' + Medlst[i].Id + '</td><td style="display:none">' + Medlst[i].Version + '</td><td style="display:none">' + Medlst[i].EncID + '</td><td style="display:none">' + Medlst[i].Pharmacy + '</td><td style="display:none">' + Medlst[i].Pharmacy_Notes + '</td><td style="display:none">' + Medlst[i].StopNotes + '</td><td style="display:none">' + Medlst[i].RetainNotes + '</td><td style="display:none">' + Medlst[i].Status + '</td><td style="display:none">' + Medlst[i].CreatedBy + '</td><td style="display:none">' + Medlst[i].CreatedDateTime + '</td></tr>';
                tbody.append(tr);
            }
            tbody.append('</tbody>');
            $(tbody).css('width', '99%');
            scrolify_RxHis($(tbody), "275px");
            if ($(tbody)[0].parentNode != null && $(tbody)[0].parentNode != undefined) {
                if ($(tbody)[0].parentNode.parentNode != null && $(tbody)[0].parentNode.parentNode != undefined && $(tbody)[0].parentNode.parentNode.nextElementSibling != null && $(tbody)[0].parentNode.parentNode.nextElementSibling!=undefined)
                $($(tbody)[0].parentNode.parentNode.nextElementSibling).remove();
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
}

//new design
function SavedSuccessfully() {
    $("#btnSave")[0].disabled = true;
    localStorage.setItem("bSave", "true");
    DisplayErrorMessage('180602');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
}
function StopMedication(item) {

    var drugid;
    var name = $(item).parent().parent()[0].attributes.getNamedItem("name").value;
    var textareaID = "RxStopNotes" + name;
    if (item.checked) {
        $(item).parent().parent()[0].childNodes[5].childNodes[0].disabled = false;//Re Activate enable
        $(item).parent().parent()[0].childNodes[1].childNodes[0].disabled = true;//retain disable
        $(item).parent().parent()[0].childNodes[6].childNodes[0].disabled = false;
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).removeAttr("disabled");
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).css("background-color", "#6DABF7 !important");
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).attr("onclick", "callweb(this, \'Stop or Retain Notes\', '" + textareaID + "')");

        drugid = $(item).parent().parent()[0].attributes.getNamedItem("name").value;
        $("td[name='" + drugid + "'] span").css("display", 'none');

    }
    else {
        drugid = $(item).parent().parent()[0].attributes.getNamedItem("name").value;
        $(item).parent().parent()[0].childNodes[5].childNodes[0].value = "";
        $(item).parent().parent()[0].childNodes[5].childNodes[0].disabled = true;
        $(item).parent().parent()[0].childNodes[6].childNodes[0].value = "";
        $(item).parent().parent()[0].childNodes[1].childNodes[0].disabled = false;
        $(item).parent().parent()[0].childNodes[6].childNodes[0].disabled = true;
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).attr("disabled", true);
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).attr("onclick", "return false;");
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).css("background-color", "#6D7777 !important");
        $("td[name='" + drugid + "'] span").css("display", '');
    }
    i = i + 1;
    SetSaveEnabled();
}

function RetainMedication(item) {

    var name = $(item).parent().parent()[0].attributes.getNamedItem("name").value;
    var textareaID = "RxStopNotes" + name;
    if (item.checked) {
        $(item).parent().parent()[0].childNodes[5].childNodes[0].disabled = true;//Change color of stop btn
        $(item).parent().parent()[0].childNodes[0].childNodes[0].disabled = true;
        $(item).parent().parent()[0].childNodes[6].childNodes[0].disabled = false;
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).removeAttr("disabled");
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).css("background-color", "#6DABF7 !important");
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).attr("onclick", "callweb(this, \'Stop or Retain Notes\', '" + textareaID + "')");

        $tr = $(item).parent()[0].closest('tr');

        rowspan = $(item).parent()[0].rowSpan;
        Messagenotes = new Array(parseInt(rowspan));
        index = $('tr').index($tr);
        tot = parseInt(index) + parseInt(rowspan);
        for (var i = index, len = tot, j = 0; i < len; i++, j++) {
            Messagenotes[j] = $(item).parent().parent()[0].attributes.getNamedItem("name").value + "~" + $('tr:eq(' + i + ') td:eq(3)').text();
          
        }
    }
    else {
        var n = 0;
        var message = "";
        $(item).parent().parent()[0].childNodes[5].childNodes[0].value = "";
        $(item).parent().parent()[0].childNodes[5].childNodes[0].disabled = true;
        $(item).parent().parent()[0].childNodes[6].childNodes[0].value = "";
        $(item).parent().parent()[0].childNodes[0].childNodes[0].disabled = false;
        $(item).parent().parent()[0].childNodes[6].childNodes[0].disabled = true;
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).attr("disabled", true);
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).attr("onclick", "return false;");
        $($(item).parent().parent()[0].childNodes[6].childNodes[2].childNodes[1]).css("background-color", "#6D7777 !important");

        var id = $(item).parent().parent()[0].attributes.getNamedItem("name").value;
        rowspan = $(item).parent()[0].rowSpan;

        index = $('tr').index($tr);
        tot = parseInt(index) + parseInt(rowspan);
        for (var i = index, len = tot; i < len; i++) {

            for (var y = n; y < Messagenotes.length; y++) {
                n = y + 1;
                if (Messagenotes[y].split('~')[0] == id) {
                    message = (Messagenotes[y].split('~')[1]);
                    break;
                }
            }
           
        }

    }
    SetSaveEnabled();
}

function ActivateMed(item) {
    var name = $(item).parent().parent()[0].attributes.getNamedItem("name").value;
    document.getElementsByName(name)[0].childNodes[5].childNodes[0].disabled = true;//ReActivate disable
    document.getElementsByName(name)[0].childNodes[1].childNodes[0].disabled = false;//retain enable
    document.getElementsByName(name)[0].childNodes[0].childNodes[0].style.color = "red";//Change color of stop btn
    document.getElementsByName(name)[0].childNodes[0].childNodes[0].attributes.getNamedItem("onclick").value = "StopMedication(this)";//Stop disable onclick
    SetSaveEnabled();
}
function ClearAllInReconcilation() {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        $('#tblDrugBody > tbody  > tr').each(function () {
            $this = $(this);
            if ($this[0].childNodes.length > 1) {
                if ($this[0].childNodes[0].childNodes.length != 0 && ($this[0].childNodes[0].childNodes[0].checked || $this[0].childNodes[1].childNodes[0].checked)) {
                    $this[0].childNodes[6].childNodes[0].value = "";
                    $this[0].childNodes[5].childNodes[0].value = "";
                    var drugid = $this[0].attributes.getNamedItem("name").value;
                    $("tr[name='" + drugid + "'] span").css("display", '');
                    if ($this[0].childNodes[0].childNodes[0].checked)
                        $this[0].childNodes[0].childNodes[0].click();
                    if ($this[0].childNodes[1].childNodes[0].checked)
                        $this[0].childNodes[1].childNodes[0].click();
                }
                else {
                    $this[0].childNodes[6].childNodes[0].value = "";
                }
            }
        });
        setSaveDisabled();
    }
    else {
        return false;
    }
}
function setSaveDisabled() {
    if ($('#btnSave')[0] != undefined) {
        $('#btnSave')[0].disabled = true;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        localStorage.setItem("bSave", "true");
    }
   
}
function SetSaveEnabled() {
    if ($('#btnSave')[0] != undefined) {
        $('#btnSave')[0].disabled = false;
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
        localStorage.setItem("bSave", "false");
    }
}

function autoSave() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $(dvdialogMedReconcile).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'center',
                at: 'center + 100px'
            },
            buttons: {
                "Yes": function () {
                    $(dvdialogMedReconcile).dialog("close");
                    $("#btnSave").click();
                },
                "No": function () {
                    $(dvdialogMedReconcile).dialog("close");
                    setSaveDisabled();
                    $(top.window.document).find("#btnClose").click();
                    reloadTableRx_History();
                },
                "Cancel": function () {
                    SetSaveEnabled();
                    $(dvdialogMedReconcile).dialog("close");
                }
            }
        });
    }
