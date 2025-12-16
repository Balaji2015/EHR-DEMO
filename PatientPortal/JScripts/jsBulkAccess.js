document.oncontextmenu = function () {
    return !1
},$(document).ready(function () {
    $("#dtpFromDt").datetimepicker({
        closeOnDateSelect: !0,
        timepicker: !1,
        format: "d-M-Y",
        onShow: function (e) {
            this.setOptions({
                minDate: "1900/01/01",
                maxDate: '0'
            })
        },
        onSelect: function (dateText, inst) {
            alert('select!');
        }
    });
    $("#dtpToDt").datetimepicker({
        closeOnDateSelect: !0,
        timepicker: !1,
        format: "d-M-Y",
        onShow: function (e) {
            this.setOptions({
                minDate: "2000/01/01",
                maxDate: '0'
            })
        },
        onSelect: function (dateText, inst) {
            alert('select!');
        }
    });
   
    if (document.getElementById('hdntodate').value == "") {
        $("#dtpToDt").datetimepicker({
            value: getLocalTime()
        });
    }
    else {
        $("#dtpToDt").datetimepicker({
            value: document.getElementById('hdntodate').value
        });
    }
     

    if (document.getElementById('hdnfromdate').value == "") {
        $("#dtpFromDt").datetimepicker({
            value: getfromDate()
        });
    }
    else {
        $("#dtpFromDt").datetimepicker({
            value: document.getElementById('hdnfromdate').value
        });
    }

   
});



//$('#bigImagePDF').hide();
//enableDates(document.getElementById("chckShowOldFiles").checked), "" == $("#dtpScannedDate").val() && $("#dtpScannedDate").datetimepicker({
//    value: getLocalTime()
//});
var e = 0;
jQuery.fn.rotate = function (e) {
    $("#bigImg").css({
        "-webkit-transform": "rotate(" + e + "deg)"
    })
}, $("#leftrotate").click(function () {
    e -= 90, $("#leftrotate").rotate(e)
}), $("#rightrotate").click(function () {
    e += 90, $("#rightrotate").rotate(e)
}), $("#zoomin").click(function () {
    var e = 10,
        t = parseInt($("#bigImg").width());
    $("#bigImg").width(t + e + "px");
    var n = parseInt($("#bigImg").height());
    $("#bigImg").height(n + e + "px")
}), $("#zoomout").click(function () {
    var e = 10,
        t = parseInt($("#bigImg").width());
    $("#bigImg").width(t - e + "px");
    var n = parseInt($("#bigImg").height());
    $("#bigImg").height(n - e + "px")
})


function btnSend_ClientClick() {
    var val = getCheckedItemsList();
    if (val == 1) {
        $(top.window.document).find("#hdnIsZip")[0].value = "true";
        $(top.window.document).find("#hdnbulkaccess")[0].value = "Yes";
        $(top.window.document).find("#hdnPatientPortal")[0].value = "Yes";//BugID:49606
        
        
        $(top.window.document).find("#btnSend").click();
    }
}

function btnDownloadClick() {
    var val = getCheckedItemsList();
    if (val == 1) {
        $(top.window.document).find("#hdnIsZip")[0].value = "true";
        $(top.window.document).find("#btnDownload").click();
    }
}
function getLocalTime() {
    var e = new Date,
        t = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"),
        n = e.getDate(),
        i = t[e.getMonth()],
        o = e.getFullYear(),
        e = n + "-" + i.toString() + "-" + o;
    return e
}
function getfromDate() {
    var e = new Date;
    e.setMonth(e.getMonth() -3);
       t = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"),
       n = e.getDate(),
       i = t[e.getMonth()],
       o = e.getFullYear(),
       e = n + "-" + i.toString() + "-" + o;
    return e
}
function ClearBulKAccessScreen() {
    $($("#SummaryList")[0].children).remove();
}
function getCheckedItemsList() {
    $(top.window.document).find("#hdnEncList")[0].value = "";
    var fileEncIds = [];
    var chckcnt = $("#SummaryList input[type=checkbox]:checked").length;
    if (chckcnt > 0) {
        for (var i = 0; i < chckcnt; i++) {
            fileEncIds.push($("#SummaryList input[type=checkbox]:checked")[i].parentNode.attributes.getNamedItem("hdnId").value);
        }
        $(top.window.document).find("#hdnEncList")[0].value = fileEncIds;
        return 1;
    }
    else {
        alert('Kindly select files to Send/Download');
        return 0;
    }

}
$("#SummaryList input[type=checkbox]").change(function () {
    if (this.checked) {
        $("#SummaryList label").css("background-color", "#fff");
        $(this)[0].nextElementSibling.style.backgroundColor = "#bddbff";
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        debugger;
        var EncId = $(this)[0].parentNode.attributes.getNamedItem("hdnId").value;
        var WSData = "{\"encounter_id\":\"" + EncId + "\", \"isFromBulkAccess\":\"" + true + "\"}";
        //document.getElementById('hdnEncounterId').value = leaf[0].id.split('^')[1];
        $.ajax({
            type: "POST",
            url: "webfrmPatientPortal.aspx/showreport",
            data: WSData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#bigImagePDF iframe')[0].src = "frmPrintPDF.aspx?SI=" + data.d + "&Location=DYNAMIC";
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                        log.ExceptionType + " \nMessage: " + log.Message);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        $(this)[0].nextElementSibling.style.backgroundColor = "#fff";
    }
});

function bulkload()
{
}
function btnClose_Clicked() {
    window.GetRadWindowManager().BrowserWindow.close();
    $(top.window.document).find('#EncounterContainer')[0].src = "";
    $(top.window.document).find("btnDownload").disabled = true;
    $(top.window.document).find("btnSend").disabled = true;
    $(top.window.document).find("btndelete").disabled = true;
}
function btn_generateClick() {
    if ($("#SummaryList")[0] != undefined && $("#SummaryList")[0].children.length > 0)
        $($("#SummaryList")[0].children).remove();
    var now = new Date();
    var currentdate = now.getUTCFullYear() + '-' + (now.getUTCMonth() + 1) + '-' + now.getUTCDate();
    currentDT = Date.parse(currentdate);
    var frmDT = $("#dtpFromDt")[0].value;
    var toDT = $("#dtpToDt")[0].value;
    frmDaTe = Date.parse(frmDT);
    toDaTe = Date.parse(toDT);
    if (frmDaTe > toDaTe) {
        alert('From date cannot be greater than to date');
        return false;
    }
    //else if (toDaTe > currentDT) {
    //    alert('To date cannot be greater than current date');
    //    return false;
    //}
    else {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return true;
    }
}