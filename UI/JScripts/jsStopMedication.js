$(document).ready(function () {
    $(".datecontrol").mask("9999?-aaa-99");
    $("#RxHistoryNotes").on("keypress", function () {
        EnableSave();
    });
    $("#dtpStopDate").on("keypress", function () {
        EnableSave();
    });
    $("#selStopReason").on("change", function () {
        EnableSave();
    });
    $("#dtpStopDate").val(getLocalTime());
    $(top.window.document).find('#btnstopmedClose').hide();
    $("#DrugName").val(" "+sessionStorage.getItem("DrugName").toString());
});
function getLocalTime() {
    var e = new Date,
        t = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"),
        n = e.getDate(),
        i = t[e.getMonth()],
        o = e.getFullYear()

    if (n < 10)
        n = '0' + n;
    var ve = o.toString() + "-" + i.toString() + "-" + n.toString();
    return ve
}
function btnSave() {
    var encID = "";
    var Id = "";
    var vertion = "";
    if (document.baseURI.split("HtmlStopMedication.html?")[1].length > -1) {
        encID = document.baseURI.split("HtmlStopMedication.html?")[1].split("&")[1].split("encID=")[1];
        Id = document.baseURI.split("HtmlStopMedication.html?")[1].split("&")[2].split("Id=")[1];
        vertion = document.baseURI.split("HtmlStopMedication.html?")[1].split("&")[3].split("versionid=")[1]
    }
    var type = "del";
    var stopDate = $("#dtpStopDate").val();
    var stopReason = $("#selStopReason").val();
    var stopNotes = $("#RxHistoryNotes").val();
    if (stopNotes.trim() != "")
    {
        var obj = { Type: type, ID: Id, EncID: encID, StopDate: stopDate, StopReason: stopReason, StopNotes: stopNotes, Version: vertion };
        $.ajax({
            type: "POST",
            url: "frmRxHistory.aspx/StopRxHistory",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ data: obj }),
            datatype: "json",
            success: function success(data) {
                var result = JSON.parse(data.d);
                var MedTooltip = result.Medtooltip;
                var regex = /<BR\s*[\/]?>/gi;
                top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = MedTooltip[0];
                top.window.document.getElementById("Medication_tooltp").innerText = MedTooltip[1].replace(regex, "\n") + "\n";
                RefreshOverallSummaryTooltip();
                RefreshNotification('RxHistory');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                SavedSuccessfully();
                loadRx_History();
                $("#btnSave").prop("disabled", true);
                btnclose();
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
    else
    {
        DisplayErrorMessage('3006');
    }
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
function btnclose() {
    if ($("#btnSave").prop("disabled") == true)
    { 
        $(top.window.document).find('#btnstopmedClose').click();
    }
    else
    {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $(divStopMedication).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'top',
                at: 'center + 100px'
            },
            buttons: {
                "Yes": function () {
                    $(divStopMedication).dialog("close");
                    btnSave();
                },
                "No": function () {
                    $(divStopMedication).dialog("close");
                    $(top.window.document).find('#btnstopmedClose').click();
                },
                "Cancel": function () {
                    EnableSave();
                    $(divStopMedication).dialog("close");
                }
            }
        });
        $(".ui-dialog-titlebar-close").css({ "display": "none" });
    }
}

function loadRx_History() {
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
            $($(tbody)[0].parentNode.parentNode.nextElementSibling).remove();

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
function EnableSave() {
    $("#btnSave").prop("disabled", false);
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
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:44642
                if (sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN" || sessionStorage.getItem("Rx_UserRole").toUpperCase() == "PHYSICIAN ASSISTANT") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick=\"openAddorUpdate('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"fun('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";
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
                          top: '132px',
                          left: pos.left,
                          width: '250px',
                          height: '64px',
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