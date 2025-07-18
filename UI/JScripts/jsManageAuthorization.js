var IsShowAll = "false";
var DeleteCPTID = new Array();
$(document).ready(function () {

    $("#txtauthValidfrom").datetimepicker({ timepicker: false, format: 'd-M-Y' });

    $("#txtauthvalidTo").datetimepicker({ timepicker: false, format: 'd-M-Y' });

    //CPT
    $("#imgClearCPTText").on("click", function () {
        $('#txtCPT').attr("disabled", false);
        $('#txtCPT').val('').focus();
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });
    var curleft = curtop = 0;
    var current_element = document.getElementById('txtCPT');
    var left = current_element.offsetWidth
    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    left = left + curleft;
    $("#imgClearCPTText").css({
        "right": "0px !important;",
        "left": (left - 10) + "px !important;",
        "top": (curtop + 3).toString() + "px",
        "cursor": "pointer",
    }).on("click", function () {
        $('#txtCPT').val('').focus();
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });

    //ICD

    var curleft = curtop = 0;
    var current_element = document.getElementById('txtDiagnosis');
    var left = current_element.offsetWidth
    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    left = left + curleft;
    $("#imgClearDiagnosisText").css({
        "right": "0px !important;",
        "left": (left - 10) + "px !important;",
        "top": (curtop + 3).toString() + "px",
        "cursor": "pointer",
    }).on("click", function () {
        $('#txtDiagnosis').val('').focus();
        intICDPatientlen = -1;
        arrPatientICD = [];
        $(".ui-autocomplete").hide();
    });

    $("#selCategory").combobox({ placeholder: "", containerWidth: $('#selCategory').parent().width() });
    var ROA = $("#selCategory");

    var option = '<option>Standard</option>';
    ROA.append(option);
    option = '<option>Routine</option>';
    ROA.append(option);
    option = '<option>Urgent</option>';
    ROA.append(option);
});

function ClearDiagnosis(e) {
    var idtxt = e.id.replace('imgClearDiagnosisText', 'txtDiagnosis');

    $("#" + idtxt).attr("disabled", false);
    $("#" + idtxt).val('').focus();
    intICDPatientlen = -1;
    arrPatientICD = [];
    $(".ui-autocomplete").hide();
}

var bErrorMsg = false;
function loadAuth() {
    $("#txtauthValidfrom").datetimepicker({ timepicker: false, format: 'd-M-Y' });
    $("#txtauthvalidTo").datetimepicker({ timepicker: false, format: 'd-M-Y' });
    //CPT

    var curleft = curtop = 0;
    var current_element = document.getElementById('txtCPT');

    var left = current_element.offsetWidth
    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    left = left + curleft;
    $("#imgClearCPTText").css({

        "right": "0px !important;",
        "left": (left - 10) + "px !important;",
        "top": (curtop + 3).toString() + "px",
        "cursor": "pointer",
        "width": "15px",
        "height": "15px"
    }).on("click", function () {
        $('#txtCPT').val('').focus();
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });

    //ICD
    var curleft = curtop = 0;
    var current_element = document.getElementById('txtDiagnosis');

    var left = current_element.offsetWidth
    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    left = left + curleft;
  
    if (localStorage.getItem("IsEdit") == "true") {
        document.getElementById("btnAdd").innerText = "Update";
        document.getElementById("btnClearAll").innerText = "Cancel";

    }
    else {
        DisableAdd();
    }
    if ($('#chkshowall')[0].checked) {
        IsShowAll = "true";
    }
    LoadAuthorization(IsShowAll);
}

var deleteID = null;
function LoadAuthorization(IsShowAll) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var Data = [IsShowAll]
    var humanId = "";
    var appdate = "";
    if ($('#hdnhumanidauth').val() == "")
        humanId = "0";
    else
        humanId = $('#hdnhumanidauth').val();

    Data.push(humanId);

    Data.push(appdate);

    $.ajax({
        type: "POST",
        url: "frmAuthorization.aspx/LoadAuthorizationData",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            "data": Data,
        }),
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d).Authlist;
            var objProdata = $.parseJSON(data.d).Proclist;
            var objProdataID = $.parseJSON(data.d).ProclistID;
            var objProDesc = $.parseJSON(data.d).ProcDescription;
            CreateAuthorizationGrid(objdata, objProdata, objProdataID, objProDesc);
            if ($('#hdnselect').val() != "") {

                $('#thsel').css('display', '');
                $('[id^=radio]').css('display', '');
                $('#btnok').css('display', 'block');

            }
            else {

                $('#thsel').css('display', 'none');
                $('[id^=rad]').css('display', 'none');
                $('#btnok').css('display', 'none');

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

function btnokclick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var flag = 0;
    var authNo = "";
    var authNoflag = 0;
    var authCPT = "";
    //To chk all selected items having same auth number
    $('#tblManageAuth > tbody  > tr').each(function () {
        if ($(this)[0].childNodes[0].childNodes[0].checked) {

            if ($(this)[0].childNodes[5].textContent != null) {
                if (authNo == "") {
                    authNo = $(this)[0].childNodes[5].textContent;
                    authCPT = $(this)[0].childNodes[6].textContent.trim() + "-" + $(this)[0].childNodes[7].textContent.trim();
                }
                else if (authNo != $(this)[0].childNodes[5].textContent) {
                    authNofdlag = 1;
                    authCPT = "";
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('1011162');
                    return false;
                }
                else {
                    authCPT += ", " + $(this)[0].childNodes[6].textContent.trim() + "-" + $(this)[0].childNodes[7].textContent.trim();
                }
            }
        }
    });
    if (authNoflag == 0) {
        $('#tblManageAuth > tbody  > tr').each(function () {
            if ($(this)[0].childNodes[0].childNodes[0].checked) {

                flag = 1;

                if ($(this)[0].childNodes[4].textContent.trim().toUpperCase() != "VALID") {

                    if (confirm("You are about to select an expired authorization as of current date. Are you sure you want to continue?")) {
                        var Result = JSON.stringify({
                            "Authnumber": $(this)[0].childNodes[5].textContent.trim(),
                            "ValidFrom": $(this)[0].childNodes[10].textContent.trim(),
                            "ValidTo": $(this)[0].childNodes[11].textContent.trim(),
                            "TestAppear": authCPT,
                            "PayerName": $(this)[0].childNodes[10].textContent.trim(),
                            "PlanName": $(this)[0].childNodes[11].textContent.trim(),

                            "PlanID": $(this)[0].childNodes[20].textContent.trim()

                        })
                        if (window.opener) {
                            window.opener.returnValue = Result;
                        }
                        returnToParent(Result);
                        return false;

                    } else {
                        return false;
                    }
                }
                else {
                    var Result = JSON.stringify({
                        "Authnumber": $(this)[0].childNodes[5].textContent.trim(),
                        "ValidFrom": $(this)[0].childNodes[12].textContent.trim(),
                        "ValidTo": $(this)[0].childNodes[13].textContent.trim(),
                        "TestAppear": authCPT,//$(this)[0].childNodes[6].textContent.trim() + "-" + $(this)[0].childNodes[7].childNodes[0].textContent.trim(),
                        "PayerName": $(this)[0].childNodes[10].textContent.trim(),
                        "PlanName": $(this)[0].childNodes[11].textContent.trim(),
                        "PlanID": $(this)[0].childNodes[20].textContent.trim()
                    })
                    if (window.opener) {
                        window.opener.returnValue = Result;
                    }
                    //window.returnValue = Result;
                    returnToParent(Result);
                    return false;
                }
            }

        });

        if (flag == 0) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please Select Authorization.");
        }
    }
}
function CreateAuthorizationGrid(gData, gProdata, gProdataID, gProDescription) {
  
    var objdata = gData;
    var objProceduredata = gProdata;

    $('#dvMessageAuth').empty();

    if (objdata.length > 0) {
        for (var i = 0; i < objdata.length; i++) {

            var CreatedTime = "";
            if (objdata[i].Created_Date_And_Time != '0001-01-01T00:00:00') {

                CreatedTime = DatatimeConvert(objdata[i].Created_Date_And_Time.replace("T", " "));
            }
            var ModifiedTime = "";
            if (objdata[i].Modified_Date_And_Time != '0001-01-01T00:00:00') {
                ModifiedTime = DatatimeConvert(objdata[i].Modified_Date_And_Time.replace("T", " "))
            }
            var Validfrom = "";
            if (objdata[i].Valid_From_Date != '0001-01-01T00:00:00') {
                Validfrom = DateConvert(objdata[i].Valid_From_Date.replace("T00:00:00", ""))
            }

            var ValidTo = "";
            if (objdata[i].Valid_To_Date != '0001-01-01T00:00:00') {
                ValidTo = DateConvert(objdata[i].Valid_To_Date.replace("T00:00:00", ""))
            }
            var now = new Date();
            var CurrentUTCDate = now.getFullYear() + '-' + (now.getMonth() + 1) + '-' + now.getDate();
            var CurDt = CurrentUTCDate.split('G')[0];

            var CurDtFinal = DateConvert(CurDt);
            var statustxt = "";
            if (new Date(ValidTo) >= new Date(CurDtFinal)) {
                statustxt = "<td style='width:7%;color: green;'> Valid </td>";
            } else {
                statustxt = "<td style='width:7%;;color: red;'> Invalid </td>";
            }
            var ApprovedQuantity = "";
            if (objdata[i].Approved_Quantity != '0') {
                ApprovedQuantity = objdata[i].Approved_Quantity;
            }
            var ICD = "";
            if (objdata[i].ICD1 != "") {
                ICD += objdata[i].ICD1 + ";</br>";
            }
            if (objdata[i].ICD2 != "") {
                ICD += objdata[i].ICD2 + ";</br>";;
            }
            if (objdata[i].ICD3 != "") {
                ICD += objdata[i].ICD3 + ";</br>";;
            }
            if (objdata[i].ICD4 != "") {
                ICD += objdata[i].ICD4 + ";</br>";;
            }
            if (objdata[i].ICD5 != "") {
                ICD += objdata[i].ICD5 + ";</br>";;
            }
            if (objdata[i].ICD6 != "") {
                ICD += objdata[i].ICD6 + ";</br>";;
            }
            if (objdata[i].ICD7 != "") {
                ICD += objdata[i].ICD7 + ";</br>";;
            }
            if (objdata[i].ICD8 != "") {
                ICD += objdata[i].ICD8 + ";</br>";;
            }
            if (objdata[i].ICD9 != "") {
                ICD += objdata[i].ICD9 + ";</br>";;
            }
            if (objdata[i].ICD10 != "") {
                ICD += objdata[i].ICD10 + ";</br>";;
            }
            if (objdata[i].ICD11 != "") {
                ICD += objdata[i].ICD11 + ";</br>";;
            }
            if (objdata[i].ICD12 != "") {
                ICD += objdata[i].ICD12 + ";</br>";;
            }
            var FinalProQty = "";
            if (gProdata.length > 0) {
                FinalProQty = gProdata[i];
            }
            if (i == 0)
                tabContents = "<tr id='" + objdata[i].Id + "'>" +
            "<td style='width:5%' id='radio" + objdata[i].Id + "'>" +
            "<input type='checkbox' name='select' title='Select' /></td>" +
            "<td style='width:5%'><img  src='Resources/edit.gif' onclick='Edit(this);' title='Edit' /></td>" +
            "<td style='width:5%'><img  src='Resources/Delete-Blue.png' onclick='Delete(this);' title='Delete' /></td>" +
            statustxt +
            "<td style='width:15%'>" + objdata[i].Authorization_No + "</td>" +
            "<td style='width:45%'>" + FinalProQty + "</td>" +
            "<td style='width:10%'>" + ICD + "</td>" +
            "<td style='width:13%'>" + Validfrom + "</td>" +
            "<td style='width:13%'>" + ValidTo + "</td>" +
            "<td style='width:15%;'>" + objdata[i].Referred_From_Provider + "</td>" +
            "<td style='width: 15%;'>" + objdata[i].Referred_To_Provider + "</td>" +
            "<td style='width:20%;'>" + objdata[i].Referred_To_Facility + "</td>" +
            "<td style='width: 15%;'>" + objdata[i].Payer_Name + "</td>" +
            "<td style='width:15%'>" + objdata[i].Insurance_Plan_Name + "</td>" +
            "<td style='width:20%'>" + objdata[i].PCP_Name + "</td>" +
            "<td style='width:20%'>" + objdata[i].POS + "</td>" +
            "<td style='width:20%'>" + objdata[i].Authorization_Category + "</td>" +
            "<td style='width:25%'>" + objdata[i].Authorization_Notes + "</td>" +
            "<td style='width:15%'>" + objdata[i].Created_By + "</td>" +
            "<td style='width:20%'>" + CreatedTime + "</td>" +
            "<td style='width:15%'>" + objdata[i].Modified_By + "</td>" +
            "<td style='width:20%'>" + ModifiedTime + "</td>" +
             "<td style='display:none'>" + objdata[i].Insurance_Plan_ID + "</td>" +
             "<td style='display:none'>" + gProdataID[i] + "</td>" +
              "<td style='display:none'>" + gProDescription[i] + "</td>" +
            "</tr>";
            else
                tabContents = tabContents + "<tr id='" + objdata[i].Id + "'>" +
          "<td style='width:5%' id='radio" + objdata[i].Id + "'>" +
         "<input type='checkbox' name='select' title='Select' /></td>" +
         "<td style='width:5%'><img  src='Resources/edit.gif' onclick='Edit(this);' title='Edit' /></td>" +
         "<td style='width:5%'><img  src='Resources/Delete-Blue.png' onclick='Delete(this);' title='Delete' /></td>" +
         statustxt +
         "<td style='width:15%'>" + objdata[i].Authorization_No + "</td>" +
         "<td style='width:45%'>" + FinalProQty + "</td>" +
         "<td style='width:10%'>" + ICD + "</td>" +
         "<td style='width:13%'>" + Validfrom + "</td>" +
         "<td style='width:13%'>" + ValidTo + "</td>" +
         "<td style='width:15%;'>" + objdata[i].Referred_From_Provider + "</td>" +
         "<td style='width: 15%;'>" + objdata[i].Referred_To_Provider + "</td>" +
         "<td style='width:20%;'>" + objdata[i].Referred_To_Facility + "</td>" +
         "<td style='width: 15%;'>" + objdata[i].Payer_Name + "</td>" +
         "<td style='width:15%'>" + objdata[i].Insurance_Plan_Name + "</td>" +
         "<td style='width:20%'>" + objdata[i].PCP_Name + "</td>" +
         "<td style='width:20%'>" + objdata[i].POS + "</td>" +
         "<td style='width:20%'>" + objdata[i].Authorization_Category + "</td>" +
         "<td style='width:25%'>" + objdata[i].Authorization_Notes + "</td>" +
         "<td style='width:15%'>" + objdata[i].Created_By + "</td>" +
         "<td style='width:20%'>" + CreatedTime + "</td>" +
         "<td style='width:15%'>" + objdata[i].Modified_By + "</td>" +
         "<td style='width:20%'>" + ModifiedTime + "</td>" +
          "<td style='display:none'>" + objdata[i].Insurance_Plan_ID + "</td>" +
          "<td style='display:none'>" + gProdataID[i] + "</td>" +
          "<td style='display:none'>" + gProDescription[i] + "</td>" +
            "</tr>";
        }
        $("#dvMessageAuth").append("<table id=tblManageAuth class='table table-bordered' style='table-layout: fixed;width: 3000px;' >" +
            "<thead style='border: 0px;'>" +
            "<tr class='header'>" +
            "<th id='thsel' style='border: 1px solid #909090;width: 5%!important;text-align: center'>Sel</th>" +
            "<th style='border: 1px solid #909090;width: 5%!important;text-align: center'>Edit</th>" +
            "<th style='border: 1px solid #909090;width: 5%!important;text-align: center'>Del</th>" +
            "<th style='border: 1px solid #909090;width: 7%!important;text-align: center'>Status</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Authorization #</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 45%!important;'>CPT Code - Description - (Used Qty./Approved Qty.)</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 10%!important;'>ICD</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 13%!important;'>Valid From</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 13%!important;'>Valid To</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Referred From</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Referred To</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 20%!important;'>Facility</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Payer Name</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Ins. Plan Name</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>PCP Name</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Place of Service</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Category</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:25%!important;'>Comments</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:15%!important;'>Created By</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Created Date and Time</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:15%!important;'>Modified By</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Modified Date and Time</th>" +
            "</tr>" +
            "</thead>" +
            "<tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
    }
    else {
        $("#dvMessageAuth").append("<table id=tblManageAuth class='table table-bordered' style='table-layout: fixed;width: 3000px;' >" +
            "<thead style='border: 0px;'>" +
            "<tr class='header'>" +
            "<th id='thsel' style='border: 1px solid #909090;width: 5%!important;text-align: center'>Sel</th>" +
            "<th style='border: 1px solid #909090;width: 5%!important;text-align: center'>Edit</th>" +
            "<th style='border: 1px solid #909090;width: 5%!important;text-align: center'>Del</th>" +
            "<th style='border: 1px solid #909090;width: 7%!important;text-align: center'>Status</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Authorization #</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 45%!important;'>CPT Code - Description - (Used Qty./Approved Qty.)</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 10%!important;'>ICD</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 13%!important;'>Valid From</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 13%!important;'>Valid To</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Referred From</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Referred To</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 20%!important;'>Facility</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Payer Name</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width: 15%!important;'>Ins. Plan Name</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>PCP Name</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Place of Service</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Category</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:25%!important;'>Comments</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:15%!important;'>Created By</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Created Date and Time</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:15%!important;'>Modified By</th>" +
            "<th style='border: 1px solid #909090;text-align: center;width:20%!important;'>Modified Date and Time</th>" +
            "</tr>" +
            "</thead>" +
            "<tbody style='word-wrap: break-word;'></tbody></table>");
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $('#dvMessageAuth').css('cursor', 'default');

    if ($('#hdnselect').val() != "") {
        $('#thsel').css('display', '');
        $('[id^=radio]').css('display', '');
        $('#btnok').css('display', 'block');

    }
    else {
        $('#thsel').css('display', 'none');
        $('[id^=rad]').css('display', 'none');
        $('#btnok').css('display', 'none');

    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;
    else if (charCode == 46)
        return false;
    return true;
}

function SaveUpdateAuthorization() {
    if (document.getElementById('txtauthnumber').value.trim() == "") {
        bErrorMsg = true;
        DisplayErrorMessage('1011150');
        return false;
    }
    if (document.getElementById("txtauthvalidTo").value != "" && document.getElementById("txtauthValidfrom").value == "") {
        bErrorMsg = true;
        DisplayErrorMessage('1011154');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("txtauthvalidTo").value == "" && document.getElementById("txtauthValidfrom").value != "") {

        bErrorMsg = true;
        DisplayErrorMessage('1011160');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }

    if (ValidateEffStartDate("txtauthValidfrom", "txtauthvalidTo") == false && document.getElementById("txtauthValidfrom").value != ""
      && document.getElementById("txtauthvalidTo").value != "") {

        bErrorMsg = true;
        DisplayErrorMessage('1011153');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if ((document.getElementById("txtauthValidfrom").value != undefined && document.getElementById("txtauthValidfrom").value == "") &&
      (document.getElementById("txtauthvalidTo").value != undefined && document.getElementById("txtauthvalidTo").value == "")) {
        bErrorMsg = true;
        DisplayErrorMessage('1011151');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("txtauthnumber").value != "" && (document.getElementById('txtauthPayer').value == "" || document.getElementById("txtauthinsplan").value == "")) {
        bErrorMsg = true;
        DisplayErrorMessage('1011152');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById('txtReferredFrom').value.trim() == "") {
        bErrorMsg = true;
        DisplayErrorMessage('1011163');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById('txtReferredTo').value.trim() == "" && document.getElementById('ddFacility').value.trim() == "") {

        bErrorMsg = true;
        DisplayErrorMessage('1011164');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("tblCPT").rows.length == 1) {
        bErrorMsg = true;
        DisplayErrorMessage('1011156');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    //Save
    var ICDDesc = "";
    if (document.getElementById("tblCPT").rows.length > 1) {
        for (var i = 1; i < document.getElementById("tblCPT").rows.length; i++) {
            if (document.getElementById("tblCPT").rows[i].cells[3].children[0].value.trim() == "" || document.getElementById("tblCPT").rows[i].cells[3].children[0].value == "0") {
                bErrorMsg = true;
                alert("Please enter the approved quantity for the CPT " + document.getElementById("tblCPT").rows[i].cells[1].innerText + " as atleast 1.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            if (document.getElementById("tblCPT").rows[i].cells[4].children[0].value.trim() != "" && document.getElementById("tblCPT").rows[i].cells[4].children[0].value.trim() != "0") {
                if (document.getElementById("tblCPT").rows[i].cells[4].children[0].value.trim() > document.getElementById("tblCPT").rows[i].cells[3].children[0].value.trim()) {
                    bErrorMsg = true;
                    alert("The entered approved quantity is greater than the used quantity for this CPT " + document.getElementById("tblCPT").rows[i].cells[1].innerText + ".");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }
            }
            if (ICDDesc == "")
                ICDDesc = document.getElementById("tblCPT").rows[i].cells[1].innerText + "~" + document.getElementById("tblCPT").rows[i].cells[2].innerText + "~" + document.getElementById("tblCPT").rows[i].cells[3].children[0].value + "~" + document.getElementById("tblCPT").rows[i].cells[4].children[0].value + "~" + document.getElementById("tblCPT").rows[i].cells[5].innerText;
            else
                ICDDesc += "|" + document.getElementById("tblCPT").rows[i].cells[1].innerText + "~" + document.getElementById("tblCPT").rows[i].cells[2].innerText + "~" + document.getElementById("tblCPT").rows[i].cells[3].children[0].value + "~" + document.getElementById("tblCPT").rows[i].cells[4].children[0].value + "~" + document.getElementById("tblCPT").rows[i].cells[5].innerText;
        }
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var AuthNo = document.getElementById('txtauthnumber').value.replace(' ', '').trim();
    var ValidFrom = document.getElementById('txtauthValidfrom').value;
    var ValidTo = document.getElementById('txtauthvalidTo').value;
    var PayerName = document.getElementById('txtauthPayer').value;
    var InsPlanName = document.getElementById('txtauthinsplan').value;
    var PCPName = document.getElementById('txtPCPName').value;
    var ReferredFrom = document.getElementById('txtReferredFrom').value;
    var ReferredTo = document.getElementById('txtReferredTo').value;
    var POS = $("#ddPlcOfService option:selected")[0].text;
    var category = document.getElementById('selCategory').value;
    var facility = $("#ddFacility option:selected")[0].text;
    var Comments = document.getElementById('txtComments').value;
    var Diagnosis1 = document.getElementById('txtDiagnosis').value;
    var Diagnosis2 = document.getElementById('txtDiagnosis2').value;
    var Diagnosis3 = document.getElementById('txtDiagnosis3').value;
    var Diagnosis4 = document.getElementById('txtDiagnosis4').value;
    var Diagnosis5 = document.getElementById('txtDiagnosis5').value;
    var Diagnosis6 = document.getElementById('txtDiagnosis6').value;
    var Diagnosis7 = document.getElementById('txtDiagnosis7').value;
    var Diagnosis8 = document.getElementById('txtDiagnosis8').value;
    var Diagnosis9 = document.getElementById('txtDiagnosis9').value;
    var Diagnosis10 = document.getElementById('txtDiagnosis10').value;
    var Diagnosis11 = document.getElementById('txtDiagnosis11').value;
    var Diagnosis12 = document.getElementById('txtDiagnosis12').value;

    var btnMode = null;
    var PrimaryId = null;
    if (document.getElementById("btnAdd").innerText.trim() == "Add")
        btnMode = "Add";
    if (document.getElementById("btnAdd").innerText.trim() == "Update") {
        btnMode = "Update";
        PrimaryId = localStorage.getItem("UpdateId");
    }
    IsShowAll = "false";
    if ($('#chkshowall')[0].checked) {
        IsShowAll = "true";
    }

    if ($('#hdnhumanidauth').val() == "")
        humanId = "0";
    else
        humanId = $('#hdnhumanidauth').val();

    var planID = "";
    if (document.getElementById('txtauthinsplan') != undefined && document.getElementById('txtauthinsplan') != null && document.getElementById('txtauthinsplan').getAttribute('planid') != undefined && document.getElementById('txtauthinsplan').getAttribute('planid') != null)
        planID = document.getElementById('txtauthinsplan').getAttribute('planid');

    var PCPID = "";
    if (document.getElementById('txtPCPName') != undefined && document.getElementById('txtPCPName') != null && document.getElementById('txtPCPName').getAttribute('pcpid') != undefined && document.getElementById('txtPCPName').getAttribute('pcpid') != null)
        PCPID = document.getElementById('txtPCPName').getAttribute('pcpid');

    var Data = [AuthNo, ValidFrom, ValidTo, PayerName, InsPlanName, planID, PCPID, PCPName, ReferredFrom, ReferredTo, POS, category, facility, Comments, Diagnosis1, Diagnosis2, Diagnosis3, Diagnosis4, Diagnosis5, Diagnosis6, Diagnosis7, Diagnosis8, Diagnosis9, Diagnosis10, Diagnosis11, Diagnosis12, btnMode, PrimaryId, IsShowAll, humanId];
    var CPTData = ICDDesc;
    $.ajax({
        type: "POST",
        url: "frmAuthorization.aspx/SaveAuthorizationData",
        data: JSON.stringify({
            "data": Data,
            "cptdata": CPTData,
            "deletecptid": DeleteCPTID
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var objdata = $.parseJSON(data.d).Authlist;
            var Validation = $.parseJSON(data.d).ValidationMessage;
            var objProdata = $.parseJSON(data.d).Proclist;
            var objProdataID = $.parseJSON(data.d).ProclistID;
            var objProDesc = $.parseJSON(data.d).ProcDescription;
            if (Validation != "") {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                alert("Authorization number '" + AuthNo + "' have already captured.Please edit the same to continue.")
                return false;
            } else {
                ClearAll();
                CreateAuthorizationGrid(objdata, objProdata, objProdataID, objProDesc);
                DisableAdd();
                if (IsShowAll == "true")
                    $('#chkshowall')[0].checked = true;
                localStorage.setItem("IsEdit", "false");
                localStorage.setItem("UpdateId", "");
                DisplayErrorMessage('1011155');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
function Edit(event) {
    ClearAll();
    //For Bug Id59450.Insurance part in not cleared in clear all functionality.
    document.getElementById('txtauthPayer').value = "";
    document.getElementById('txtauthPayer').title = "";
    document.getElementById('txtauthinsplan').value = "";
    document.getElementById('txtauthinsplan').title = "";
    document.getElementById('txtPCPName').value = "";
    document.getElementById('txtPCPName').title = "";
    //
    localStorage.setItem("IsEdit", "true");
    document.getElementById("btnAdd").innerText = "Update";
    document.getElementById("btnClearAll").innerText = "Cancel";
    document.getElementById('btnSelectPlan').disabled = false;
    document.getElementById('txtauthvalidTo').disabled = false;
    document.getElementById('txtauthValidfrom').disabled = false;
    $('#txtDiagnosis').attr("disabled", false);
    $('#imgClearDiagnosisText').bind('click');

    //Assign Edit row values
    localStorage.setItem("UpdateId", $(event).parent().parent()[0].id);
    localStorage.setItem("DeleteId", $(event).parent().parent()[0].children[22].textContent);
    document.getElementById('txtauthnumber').value = $(event).parent().parent()[0].children[4].textContent;
    $('#tbodyCPT').empty();
    if ($(event).parent().parent()[0].children[5].textContent != "") {
        var CPTarry = $(event).parent().parent()[0].children[5].innerText.split('\n');
        var CPTid = $(event).parent().parent()[0].children[23].textContent.split(';');//document.getElementById('txtComments').value = 
        var CPTDesc = $(event).parent().parent()[0].children[24].textContent.split('^~^');
        if (CPTarry.length > 0) {
            for (var i = 0; i < CPTarry.length - 1; i++) {
                $("#tbodyCPT").append("<tr>" +
                          "<td style='width:2%;'><img src='Resources/Delete-Blue.png' onclick='deleteCPT(this)'/></td>" +
                          "<td style='width:6%;' class='spanstyle'>" + CPTarry[i].split('-')[0] + "</td>" +
                          "<td style='width:70%;' class='spanstyle'>" + CPTDesc[i] + "</td>" +//CPTarry[i].split('-')[1] 
                          "<td style='width:10%;' ><input type='text' style='width: 100%;'  class='Editabletxtbox'  id='txtapproved value='" + $(event).parent().parent()[0].id + "'  onkeypress='EnableAdd(); return isNumberKey(event);' MaxLength='3'  value='" + CPTarry[i].split('- (')[1].split(' / ')[1].trim().replace(")", "") + "'/></td>" +
                          "<td style='width:7%;'><input type='text' style='width: 100%;'  id='txtused" + $(event).parent().parent()[0].id + "' class='nonEditabletxtbox'  onkeypress='EnableAdd(); return isNumberKey(event);' MaxLength='3'  readonly='readonly' value='" + CPTarry[i].split('- (')[1].split(' / ')[0].trim() + "'/></td>" +
                           "<td style='display:none'>" + CPTid[i] + "</td>"
                           + "</tr>");
            }
        }
    }
    if ($(event).parent().parent()[0].children[6].textContent != "") {
        var ICDarry = $(event).parent().parent()[0].children[6].textContent.split(';');
        if (ICDarry.length > 0) {
            if (ICDarry[0] != null && ICDarry[0] != undefined && ICDarry[0] != "") {
                document.getElementById('txtDiagnosis').value = ICDarry[0];
                document.getElementById('txtDiagnosis').title = ICDarry[0];
                document.getElementById('txtDiagnosis').disabled = true;
            }
            if (ICDarry[1] != null && ICDarry[1] != undefined && ICDarry[1] != "") {
                document.getElementById('txtDiagnosis2').value = ICDarry[1];
                document.getElementById('txtDiagnosis2').title = ICDarry[1];
                document.getElementById('txtDiagnosis2').disabled = true;
            }
            if (ICDarry[2] != null && ICDarry[2] != undefined && ICDarry[2] != "") {
                document.getElementById('txtDiagnosis3').value = ICDarry[2];
                document.getElementById('txtDiagnosis3').title = ICDarry[2];
                document.getElementById('txtDiagnosis3').disabled = true;
            }
            if (ICDarry[3] != null && ICDarry[3] != undefined && ICDarry[3] != "") {
                document.getElementById('txtDiagnosis4').value = ICDarry[3];
                document.getElementById('txtDiagnosis4').title = ICDarry[3];
                document.getElementById('txtDiagnosis4').disabled = true;
            }
            if (ICDarry[4] != null && ICDarry[4] != undefined && ICDarry[4] != "") {
                document.getElementById('txtDiagnosis5').value = ICDarry[4];
                document.getElementById('txtDiagnosis5').title = ICDarry[4];
                document.getElementById('txtDiagnosis5').disabled = true;
            }
            if (ICDarry[5] != null && ICDarry[5] != undefined && ICDarry[5] != "") {
                document.getElementById('txtDiagnosis6').value = ICDarry[5];
                document.getElementById('txtDiagnosis6').title = ICDarry[5];
                document.getElementById('txtDiagnosis6').disabled = true;
            }
            if (ICDarry[6] != null && ICDarry[6] != undefined && ICDarry[6] != "") {
                document.getElementById('txtDiagnosis7').value = ICDarry[6];
                document.getElementById('txtDiagnosis7').title = ICDarry[6];
                document.getElementById('txtDiagnosis7').disabled = true;
            }
            if (ICDarry[7] != null && ICDarry[7] != undefined && ICDarry[7] != "") {
                document.getElementById('txtDiagnosis8').value = ICDarry[7];
                document.getElementById('txtDiagnosis8').title = ICDarry[7];
                document.getElementById('txtDiagnosis8').disabled = true;
            }
            if (ICDarry[8] != null && ICDarry[8] != undefined && ICDarry[8] != "") {
                document.getElementById('txtDiagnosis9').value = ICDarry[8];
                document.getElementById('txtDiagnosis9').title = ICDarry[8];
                document.getElementById('txtDiagnosis9').disabled = true;
            }
            if (ICDarry[9] != null && ICDarry[9] != undefined && ICDarry[9] != "") {
                document.getElementById('txtDiagnosis10').value = ICDarry[9];
                document.getElementById('txtDiagnosis10').title = ICDarry[9];
                document.getElementById('txtDiagnosis10').disabled = true;
            }
            if (ICDarry[10] != null && ICDarry[10] != undefined && ICDarry[10] != "") {
                document.getElementById('txtDiagnosis11').value = ICDarry[10];
                document.getElementById('txtDiagnosis11').title = ICDarry[10];
                document.getElementById('txtDiagnosis11').disabled = true;
            }
            if (ICDarry[11] != null && ICDarry[11] != undefined && ICDarry[11] != "") {
                document.getElementById('txtDiagnosis12').value = ICDarry[11];
                document.getElementById('txtDiagnosis12').title = ICDarry[11];
                document.getElementById('txtDiagnosis12').disabled = true;
            }
        }
    }
    if ($(event).parent().parent()[0].children[7].textContent != null)
        document.getElementById('txtauthValidfrom').value = $(event).parent().parent()[0].children[7].textContent;
    if ($(event).parent().parent()[0].children[8].textContent != null)
        document.getElementById('txtauthvalidTo').value = $(event).parent().parent()[0].children[8].textContent;

    document.getElementById('txtReferredFrom').value = $(event).parent().parent()[0].children[9].textContent;
    document.getElementById('txtReferredTo').value = $(event).parent().parent()[0].children[10].textContent;
    document.getElementById('txtauthPayer').value = $(event).parent().parent()[0].children[12].textContent;
    document.getElementById('txtauthPayer').title = $(event).parent().parent()[0].children[12].textContent;
    document.getElementById('txtauthinsplan').value = $(event).parent().parent()[0].children[13].textContent;
    document.getElementById('txtauthinsplan').title = $(event).parent().parent()[0].children[13].textContent;
    document.getElementById('txtPCPName').value = $(event).parent().parent()[0].children[14].textContent;
    document.getElementById('txtPCPName').title = $(event).parent().parent()[0].children[14].textContent;
    document.getElementById('selCategory').value = $(event).parent().parent()[0].children[16].textContent;
    document.getElementById('txtComments').value = $(event).parent().parent()[0].children[17].textContent;
    $('.custom-combobox-input.ui-autocomplete-input')[0].value = $(event).parent().parent()[0].children[16].textContent;

    var ddlPOS = document.getElementById("ddPlcOfService")
    for (var x = 0; x < ddlPOS.length - 1 ; x++) {
        if ($(event).parent().parent()[0].children[15].textContent.toUpperCase() == ddlPOS.options[x].text.toUpperCase())
            ddlPOS.selectedIndex = x;
    }

    var ddlFacilty = document.getElementById("ddFacility")
    for (var x = 0; x < ddlFacilty.length - 1 ; x++) {
        if ($(event).parent().parent()[0].children[11].textContent.toUpperCase() == ddlFacilty.options[x].text.toUpperCase())
            ddlFacilty.selectedIndex = x;
    }
    EnableAdd();

}

function Delete(event) {
    if (confirm("Are you sure you want to delete?")) {
        if ($(event).parent().parent()[0].children[5].textContent != "") {
            var CPTarry = $(event).parent().parent()[0].children[5].innerText.split('\n');
            if (CPTarry.length > 0) {
                for (var i = 0; i < CPTarry.length - 1; i++) {
                    if (CPTarry[i].split('- (')[1].split(' / ')[0].trim() > 0) {
                        DisplayErrorMessage('1011172');
                        return false;
                    }
                }
            }
        }
        deleteID = $(event).parent().parent()[0].id;
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var IsShowAll = "false";
        if ($('#chkshowall')[0].checked) {
            IsShowAll = "true";
        }
        if ($('#hdnhumanidauth').val() == "")
            humanId = "0";
        else
            humanId = $('#hdnhumanidauth').val();
        var Data = [deleteID, IsShowAll, humanId];
        $.ajax({
            type: "POST",
            url: "frmAuthorization.aspx/DeleteAuthorizationData",
            data: JSON.stringify({
                "data": Data,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                var objdata = $.parseJSON(data.d).Authlist;
                var objProdata = $.parseJSON(data.d).Proclist;
                var objProdataID = $.parseJSON(data.d).ProclistID;
                var objProDesc = $.parseJSON(data.d).ProcDescription;
                CreateAuthorizationGrid(objdata, objProdata, objProdataID, objProDesc);
                DisplayErrorMessage('1011159');
                ClearAll();

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
    else {
        return false;
    }
}

function showall() {
    if ($('#chkshowall')[0].checked) {
        LoadAuthorization('true');
        IsShowAll = "true";
    }
    else {
        LoadAuthorization('false');
        IsShowAll = "false";
    }
}

Date.prototype.toShortFormat = function () {

    var month_names = ["Jan", "Feb", "Mar",
                      "Apr", "May", "Jun",
                      "Jul", "Aug", "Sep",
                      "Oct", "Nov", "Dec"];

    var day = this.getDate();
    if (day < 10) {
        day = '0' + day;
    }
    var month_index = this.getMonth();
    var year = this.getFullYear();

    return "" + day + "-" + month_names[month_index] + "-" + year;
}

function ClearAll() {
    DeleteCPTID = new Array();
    document.getElementById('txtauthnumber').value = "";
    var today = new Date();
    document.getElementById('txtauthValidfrom').value = today.toShortFormat();
    document.getElementById('txtauthvalidTo').value = "";

    document.getElementById('txtReferredFrom').value = "";
    document.getElementById('txtReferredTo').value = "";
    var ddlPOS = document.getElementById("ddPlcOfService")

    for (var x = 0; x < ddlPOS.length - 1 ; x++) {
        if (document.getElementById('hdnDefaultPlaceOfService').value.toUpperCase() == ddlPOS.options[x].text.toUpperCase())
            ddlPOS.selectedIndex = x;
    }
    $(":input").not(':input[type=button]').not(':input[type=hidden]').val("");
    document.getElementById('ddFacility').selectedIndex = 0;
    document.getElementById('txtComments').value = "";
    document.getElementById('txtComments').title = "";
    document.getElementById('txtDiagnosis').value = "";
    document.getElementById('txtDiagnosis2').value = "";
    document.getElementById('txtDiagnosis3').value = "";
    document.getElementById('txtDiagnosis4').value = "";
    document.getElementById('txtDiagnosis5').value = "";
    document.getElementById('txtDiagnosis6').value = "";
    document.getElementById('txtDiagnosis7').value = "";
    document.getElementById('txtDiagnosis8').value = "";
    document.getElementById('txtDiagnosis9').value = "";
    document.getElementById('txtDiagnosis10').value = "";
    document.getElementById('txtDiagnosis11').value = "";
    document.getElementById('txtDiagnosis12').value = "";
    document.getElementById('txtDiagnosis').title = "";
    document.getElementById('txtDiagnosis2').title = "";
    document.getElementById('txtDiagnosis3').title = "";
    document.getElementById('txtDiagnosis4').title = "";
    document.getElementById('txtDiagnosis5').title = "";
    document.getElementById('txtDiagnosis6').title = "";
    document.getElementById('txtDiagnosis7').title = "";
    document.getElementById('txtDiagnosis8').title = "";
    document.getElementById('txtDiagnosis9').title = "";
    document.getElementById('txtDiagnosis10').title = "";
    document.getElementById('txtDiagnosis11').title = "";
    document.getElementById('txtDiagnosis12').title = "";
    document.getElementById('txtDiagnosis').disabled = false;
    document.getElementById('txtDiagnosis2').disabled = false;
    document.getElementById('txtDiagnosis3').disabled = false;
    document.getElementById('txtDiagnosis4').disabled = false;
    document.getElementById('txtDiagnosis5').disabled = false;
    document.getElementById('txtDiagnosis6').disabled = false;
    document.getElementById('txtDiagnosis7').disabled = false;
    document.getElementById('txtDiagnosis8').disabled = false;
    document.getElementById('txtDiagnosis9').disabled = false;
    document.getElementById('txtDiagnosis10').disabled = false;
    document.getElementById('txtDiagnosis11').disabled = false;
    document.getElementById('txtDiagnosis12').disabled = false;
    document.getElementById('txtReferredFrom').disabled = false;
    document.getElementById('txtReferredTo').disabled = false;
    $('#tbodyCPT').empty();
    localStorage.setItem("IsEdit", "false");
    localStorage.setItem("UpdateId", "");
    document.getElementById("btnAdd").innerText = "Add";
    document.getElementById("btnClearAll").innerText = "Clear All";
    document.getElementById('btnAdd').disabled = true;
    $('#imgClearDiagnosisText').bind('click');

    $('input:checkbox').prop('checked', false);
    if (IsShowAll != null && IsShowAll != "" && IsShowAll != "false") {
        $('#chkshowall').prop('checked', true);
    }
    else {
        $('#chkshowall').prop('checked', false);
    }

}
function clearAllButton() {
    var IsClearAll = null;
    if (document.getElementById("btnClearAll").innerText == "Clear All") {
        IsClearAll = DisplayErrorMessage('1011157');
    }
    else {
        IsClearAll = DisplayErrorMessage('1011158');
    }
    if (IsClearAll == true) {
        ClearAll();
    } else {
        return false;
    }
}



function EnableAdd() {
    document.getElementById('btnAdd').disabled = false;
}

function DisableAdd() {
    document.getElementById('btnAdd').disabled = true;
}


function Closepopup() {
    if (!document.getElementById('btnAdd').disabled) {
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
                    sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
                    document.getElementById('btnAdd').click();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (bErrorMsg == false) {
                        returnToParent(null);
                    }
                    bErrorMsg = false;
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    returnToParent(null);
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
        returnToParent(null);
        return false;
    }
}

function DateConvert(DOB) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
}

function DatatimeConvert(utcDate) {
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

function ValidateEffStartDate(EffDate, TermDate) {
    var splitEffDatedate = document.getElementById(EffDate).value//$find(EffDate)._text;//._dateInput._text;
    var splitTermDate = document.getElementById(TermDate).value//$find(TermDate)._text;//._dateInput._text;
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


function OpenUploadDocument() {
    var obj = new Array();
    var screen = "OnlineDocuments";
    obj.push("Screen=" + screen);
    obj.push("HumanId=" + $('#hdnhumanidauth').val());
    var dateonclient = new Date;
    var Tz = (dateonclient.getTimezoneOffset());
    document.cookie = "Tz=" + Tz;
    var result = openModal("frmIndexing.aspx", 800, 1130, obj, "ctl00_ModalWindow");
}

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
var bBool = false;
var bcheck = true;
var arrPatient = [];
var intCPTLength = -1;



function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}

$('#btnPrint').click(function () {
    var strHumanId = 0;
    if ($('#lblPatientStrip')[0].innerText.split('|')[4].split('#:')[1] != undefined && $('#lblPatientStrip')[0].innerText.split('|')[4].split('#:')[1] != "") {
        strHumanId = $('#lblPatientStrip')[0].innerText.split('|')[4].split('#:')[1];
    }

    
    var ShowAll = "N";
    if ($('#chkshowall').prop('checked') == true) {
        ShowAll = "Y";
    }
    $.ajax({
        type: "POST",
        url: "frmAuthorization.aspx/PrintManageAuthorization",
        data: JSON.stringify({
            "strHumanId": strHumanId,
            "ShowAll": ShowAll,
        }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (path) {
            
            $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', "");
            $(top.window.document).find("#ModalReport").modal({ backdrop: 'static', keyboard: false }, 'show');
            $(top.window.document).find("#mdlcontentReport")[0].style.width = "100%";
            $(top.window.document).find("#ProcessiFrameReport")[0].style.border = "1px solid #D0D0D0";
            $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', path.d);
            $(top.window.document).find("#ModalReportTtle")[0].textContent = "";
            event.stopPropagation();
            event.stopImmediatePropagation();
            return false;
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
});


function btnSelectPlanclick() {

    var obj = new Array();
    var result = openModal("frmSelectPayer.aspx", 600, 900, obj, "MessageWindow");
    var WindowName = $find('MessageWindow');
    WindowName.add_close(closeplanauth);
   
}


function closeplanauth(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        if (Result.PlanId != "" && Result.PlanId != null) {
            var Data = [Result.PlanId];
            $.ajax({
                type: "POST",
                url: "frmAuthorization.aspx/GetInsurancebyId",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    var Plandata = $.parseJSON(data.d);
                    if (Plandata.InsuranceList != null && Plandata.InsuranceList != undefined && Plandata.InsuranceList.length > 0) {
                        document.getElementById('txtauthinsplan').value = Plandata.InsuranceList[0].Ins_Plan_Name;
                        document.getElementById('txtauthinsplan').title = Plandata.InsuranceList[0].Ins_Plan_Name;
                        document.getElementById("txtauthinsplan").attributes("planid").value = Plandata.InsuranceList[0].Id;
                        if (Plandata.InsuranceList[0].PCP_Name != null && Plandata.InsuranceList[0].PCP_Name != "") {
                            document.getElementById('txtPCPName').value = Plandata.InsuranceList[0].PCP_Name;
                            document.getElementById('txtPCPName').title = Plandata.InsuranceList[0].PCP_Name;

                        }
                    }

                    if (Plandata.CarrierLisr != null && Plandata.CarrierLisr != undefined) {
                        document.getElementById('txtauthPayer').value = Plandata.CarrierLisr.Carrier_Name;
                        document.getElementById('txtauthPayer').title = Plandata.CarrierLisr.Carrier_Name;
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

    }
}





var bBoolICD = false;
var bcheckICD = true;
var arrPatientICD = [];
var intICDLength = -1;
var intICDPatientlen = 0;
function ICDAutocomplete(evnt) {
    $(evnt).autocomplete({
        source: function (request, response) {
            if ($(evnt).val().trim().length > 0 && $(evnt).val().trim().length >= 3) {
                if (intICDPatientlen == 0) {
                    UI_Time_Start = new Date();
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    this.element.on("keydown", PreventTyping);
                    arrPatientICD = [];
                    var strkeyWords = $(evnt).val().trim().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var WSData = {
                        text_searched: strkeyWords[0]
                    };
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "./frmAuthorization.aspx/SearchICDDescription",
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            $(evnt).off("keydown", PreventTyping);
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
                                    results = FilterAuth(jsonData.Matching_Result, request.term);
                                else
                                    results = jsonData.Matching_Result;

                                arrPatientICD = jsonData.Matching_Result;
                                response($.map(results, function (item) {
                                    return {
                                        label: item,
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
                else if (intICDPatientlen != -1) {

                    var results = FilterAuth(arrPatientICD, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item,
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
            if (ui.item.label != "No matches found.") {
                var flag = true;
                for (var i = 0; i < $("input[autosearch='Disgnosis']").length ; i++) {
                    if ($("input[autosearch='Disgnosis']")[i].disabled && $($("input[autosearch='Disgnosis']")[i]).val().indexOf(ui.item.label.split('-')[0]) >= 0) {
                        flag = false;
                    }
                }
                if (flag) {
                    bBoolICD = false;
                    $(evnt).val(ui.item.label.split('-')[0]);
                    $(evnt).attr("title", ui.item.label);
                    $(evnt).attr("disabled", true);
                }
                else {
                    bBoolICD = false;
                    $(evnt).val("");
                    $(evnt).attr("title", "");

                }

            }
            else {
                bBoolICD = false;
                $(evnt).val("");
                $(evnt).attr("title", "");
            }
        },
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width($(this).width());
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $(evnt).focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intICDPatientlen = -1;
        arrPatientICD = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        if ($(evnt).val().length == 0) {
            intICDPatientlen = -1;
            arrPatientICD = [];
            $(".ui-autocomplete").hide();
        }
        else {
            intICDPatientlen = 0;
        }
    });
}
function FilterAuth(array, terms) {
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

var arrCPTs = [];
var bBool = false;
var bcheck = true;
var bDuplicatecheck = false;


$("#txtCPT").autocomplete({
    source: function (request, response) {
        if (intCPTLength == 0 && bcheck && bBool == false) {
            arrCPTs = [];
            bBool = true;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "WebServices/EandMCodingService.asmx/SearchICDDescrptionAuthText",
                data: "{\"text\":\"" + document.getElementById("txtCPT").value + "|" + "txtCPT" + "\"}",
                dataType: "json",
                async: true,
                success: function (data) {
                    var jsonData = $.parseJSON(data.d);
                    if (jsonData.length == 0) {
                        jsonData.push('No matches found.')
                        response($.map(jsonData, function (item) {
                            return {
                                label: item
                            }
                        }));
                    }
                    else {
                        response($.map(jsonData, function (item) {
                            arrCPTs.push(item);
                            return {
                                label: item
                            }
                        }));
                    }

                    $("#txtCPT").focus();
                    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
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
                    }
                }
            });
        }
        if ($("#txtCPT").val().length > 1) {
            if (arrCPTs.length != 0) {
                var results = PossibleCombination(arrCPTs, request.term);
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
    minlength: 0,
    multiple: true,
    mustMatch: false,
    open: function () {
        $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtCPT').width());
        $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
        });
    },
    select: function (event, ui) {
        event.preventDefault();
        if (ui.item.label != "No matches found.") {
            bDuplicatecheck = false;
            $('#tblCPT > tbody  > tr').each(function () {
                if ($(this)[0].childNodes[1].innerText.trim() == ui.item.label.split('~')[0]) {
                    bDuplicatecheck = true;

                }
            });
            if (bDuplicatecheck != true) {

                $("#tbodyCPT").append("<tr><td style='width:2%;'><img src='Resources/Delete-Blue.png' onclick='deleteCPT(this)'/></td>" +
                    "<td style='width:6%;' class='spanstyle'>" + ui.item.label.split('~')[0] + "</td>" +
                    "<td style='width:70%;' class='spanstyle'>" + ui.item.label.split('~')[1] + "</td>" +
                    "<td style='width:10%;' ><input type='text' style='width: 100%;' class='Editabletxtbox' id='txtapproved" + ui.item.label.split('~')[0] + "'  onkeypress='EnableAdd(); return isNumberKey(event);' MaxLength='3'/></td>" +
                    "<td style='width:7%;'><input type='text' style='width: 100%;'  id='txtused" + ui.item.label.split('~')[0] + "' class='nonEditabletxtbox' readonly='readonly'  onkeypress='EnableAdd(); return isNumberKey(event);' MaxLength='3' value='0'/></td>" +
                     "<td style='display:none'>0</td>"
                     + "</tr>");
            }

        }
        $('#txtCPT').val("");
        bBool = false;
    }
}).on("paste", function (e) {
    intCPTLength = -1;
    arrCPTs = [];
    $(".ui-autocomplete").hide();
}).on("keydown", function (e) {

    if (e.which == 8) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtCPT").val().length <= 1)
            bBool = false;
        else
            bBool = true;
        $("#txtCPT").focus();
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
    document.getElementById('txtCPTDescription').value = "";
    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') != "undefined" && jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
    { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } }

    if ($("#txtCPT").val().length >= 1) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') != "undefined" && jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } }

        if (!bBool)
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        intCPTLength = 0;
    }
    else if ($("#txtCPT").val().length != 0 && intCPTLength != -1) {
        intCPTLength = intCPTLength + 1;
    }
    if ($("#txtCPT").val().length < 1) {
        arrCPTs = [];
        $(".ui-autocomplete").hide();
        bBool = false;
    }
    if ($("#txtCPT").val().length == 1) {
        bBool = false;
    }
});

$("#txtCPTDescription").autocomplete({
    source: function (request, response) {
        if (intCPTLength == 0 && bcheck && bBool == false) {
            arrCPTs = [];
            bBool = true;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "WebServices/EandMCodingService.asmx/SearchICDDescrptionAuthText",
                data: "{\"text\":\"" + document.getElementById("txtCPTDescription").value + "|" + "txtCPTDescription" + "\"}",
                dataType: "json",
                async: true,
                success: function (data) {
                    var jsonData = $.parseJSON(data.d);
                    if (jsonData.length == 0) {
                        jsonData.push('No matches found.')
                        response($.map(jsonData, function (item) {
                            return {
                                label: item
                            }
                        }));
                    }
                    else {
                        response($.map(jsonData, function (item) {
                            arrCPTs.push(item);
                            return {
                                label: item
                            }
                        }));
                    }
                    $("#txtCPTDescription").focus();
                    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
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
                    }
                }
            });
        }
        if ($("#txtCPTDescription").val().length > 3) {
            if (arrCPTs.length != 0) {
                var results = PossibleCombination(arrCPTs, request.term);
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
        $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtCPTDescription').width());
        $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
        });
    },
    select: function (event, ui) {
        event.preventDefault();
        bDuplicatecheck = false;
        if (ui.item.label != "No matches found.") {
            $('#tblCPT > tbody  > tr').each(function () {
                if ($(this)[0].childNodes[1].innerText.trim() == ui.item.label.split('~')[0]) {
                    bDuplicatecheck = true;

                }
            });
            if (bDuplicatecheck != true) {
                $("#tbodyCPT").append("<tr><td style='width:2%;'><img src='Resources/Delete-Blue.png' onclick='deleteCPT(this)'/></td>" +
                    "<td style='width:6%;' class='spanstyle'>" + ui.item.label.split('~')[0] + "</td><td style='width:70%;' class='spanstyle'>" + ui.item.label.split('~')[1] + "</td>" +
                    "<td style='width:10%;' ><input type='text' style='width: 100%;' class='Editabletxtbox'  onkeypress='EnableAdd(); return isNumberKey(event);' MaxLength='3' id='txtapproved" + ui.item.label.split('~')[0] + "'/></td>" +
                    "<td style='width:7%;'><input type='text' style='width: 100%;'  id='txtused" + ui.item.label.split('~')[0] + "' class='nonEditabletxtbox'  readonly='readonly' onkeypress='EnableAdd(); return isNumberKey(event);' MaxLength='3'  value='0'/></td>" +
                    "<td style='display:none'>0</td></tr>");
            }
        }
        $('#txtCPTDescription').val("");
        bBool = false;
    }
}).on("paste", function (e) {
    intCPTLength = -1;
    arrCPTs = [];
    $(".ui-autocomplete").hide();
}).on("keydown", function (e) {

    if (e.which == 8) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtCPTDescription").val().length <= 3)
            bBool = false;
        else
            bBool = true;
        $("#txtCPTDescription").focus();
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

    document.getElementById('txtCPT').value = "";
    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if ($("#txtCPTDescription").val().length >= 3) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (!bBool)
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        intCPTLength = 0;
    }
    else if ($("#txtCPTDescription").val().length != 0 && intCPTLength != -1) {
        intCPTLength = intCPTLength + 1;
    }
    else if ($("#txtCPTDescription").val().length == 0 || $("#txtCPTDescription").val().length == 1) {
        intCPTLength = -1;
        arrICDs = [];
        $(".ui-autocomplete").hide();
    }
    if ($("#txtCPTDescription").val().length < 3) {
        arrCPTs = [];
        $(".ui-autocomplete").hide();
        bBool = false;
    }
});
function deleteCPT(evnt) {
    if ($(evnt)[0].parentElement.parentElement.cells[5].innerText == 0)
        $($(evnt)[0].parentElement.parentElement).remove();
    else {
        if (confirm("Are you sure you want to delete?")) {
            if ($(evnt)[0].parentElement.parentElement.cells[4].children[0].value == "0") {
                $($(evnt)[0].parentElement.parentElement).remove();
                DeleteCPTID.push($(evnt)[0].parentElement.parentElement.cells[5].innerText);
            }
            else {
                //If used quantity is filled ,,"User" not able to delete the CPT
                DisplayErrorMessage('1011172');
                return false;
            }
        }
        else {
            return false;
        }
    }

}


//Referred From
var intProviderlenFrom = -1;
var arrProviderFrom = [];

var flagFrom = 0;

$("#imgClearProviderFromText").on("click", function () {
    $('#txtReferredFrom').val('').focus();
    intProviderlenFrom = -1;
    arrProviderFrom = [];
    $(".ui-autocomplete").hide();
});
$("#txtReferredFrom").autocomplete({
    source: function (request, response) {
        if ($("#txtReferredFrom").val().trim().length > 2) {
            if (intProviderlenFrom == 0) {

                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                this.element.on("keydown", PreventTyping);
                arrProviderFrom = [];
                var strkeyWords = $("#txtReferredFrom").val().split(' ');
                var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                var sIsMenulevel = "";
                var WSData = {
                    text_searched: strkeyWords[0],
                    IsMenulevel: sIsMenulevel,
                };
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "./frmFindReferralPhysician.aspx/GetProviderDetailsByTokens",
                    data: JSON.stringify(WSData),
                    dataType: "json",
                    success: function (data) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        flagFrom = 0;
                        $("#txtReferredFrom").off("keydown", PreventTyping);
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
                                results = FilterAuth(jsonData.Matching_Result, request.term);
                            else
                                results = jsonData.Matching_Result;

                            arrProviderFrom = jsonData.Matching_Result;
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
            else if (intProviderlenFrom != -1) {

                var results = FilterAuth(arrProviderFrom, request.term);
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
    select: ProviderSelectedFrom,
    open: function () {
        $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtReferredFrom').width());
        $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
        $('#txtReferredFrom').focus();
    },
    focus: function () { return false; }
}).on("paste", function (e) {
    intProviderlenFrom = -1;
    arrProviderFrom = [];
    $(".ui-autocomplete").hide();
}).on("input", function (e) {
    $("#txtReferredFrom").css("color", "black").attr({ "data-phy-id": "0", "data-phy-details": "" });

    if ($("#txtReferredFrom").val().charCodeAt(e.currentTarget.value.length - 1) == 10) {
        e.preventDefault();
        $("#txtReferredFrom").val(e.currentTarget.value.substr(0, e.currentTarget.value.length - 1));
        return false;
    }
    if ($("#txtReferredFrom").val().charAt(e.currentTarget.value.length - 1) == " ") {
        if (e.currentTarget.value.split(" ").length > 2)
            intProviderlenFrom = intProviderlenFrom + 1;
        else
            intProviderlenFrom = 0;
    }
    else {
        if ($("#txtReferredFrom").val().length != 0 && intProviderlenFrom != -1) {
            intProviderlenFrom = intProviderlenFrom + 1;
        }

        if ($("#txtReferredFrom").val().length == 0 || $("#txtReferredFrom").val().indexOf(" ") == -1) {
            intProviderlenFrom = -1;
            arrProviderFrom = [];
            $(".ui-autocomplete").hide();
        }
    }
}).data("ui-autocomplete")._renderItem = function (ul, item) {
    if (item.label != "No matches found.") {
        if (flagFrom == 0) {
            flagFrom = 1;
            $("<li class='alinkstyle'>")
             .attr({ "data-value": "Add", "data-val": "Add" }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
             .append("Click to add Physician").addClass('alinkstyle').css("font-style", "italic")
             .appendTo(ul).on("click", function (e) {
                 e.preventDefault();
                 e.stopImmediatePropagation();
                 var obj = new Array();

                 localStorage.removeItem("IsEFax");
                 localStorage.setItem("IsEnableGrid", "false");
                 $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
                 $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px";
                 var sPath = "frmPhysicianLibray.aspx";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
                 $(top.window.document).find("#TabPhysicianLibrary").modal("show");
                 $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
                 });
                 return false;
             });;
            return $("<li>")
             .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
             .append(item.label)
             .appendTo(ul);
        }
        if (flagFrom != 0) {
            return $("<li>")
              .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
              .append(item.label)
              .appendTo(ul);
        }
    }
    else {
        if (flagFrom == 0) {
            flagFrom = 1;
            $("<li class='alinkstyle'>")
             .attr({ "data-value": "Add", "data-val": "Add" }).css({ "border-bottom": "1px solid #ccc", "font-style": "italic", "margin-bottom": "3px", "padding-bottom": "3px" })
             .append("Click to add Physician").addClass('alinkstyle')
             .appendTo(ul).on("click", function (e) {
                 var obj = new Array();

                 localStorage.removeItem("IsEFax");
                 localStorage.setItem("IsEnableGrid", "false");
                 $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
                 $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px";
                 var sPath = "frmPhysicianLibray.aspx";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
                 $(top.window.document).find("#TabPhysicianLibrary").modal("show");
                 $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
                 });
                 return false;
             });;;
            return $("<li>")
             .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
             .addClass("disabled")
             .append(item.label)
             .appendTo(ul).on("click", function (e) {
                 e.preventDefault();
                 e.stopImmediatePropagation();
             });
        }
        if (flagFrom != 0) {
            return $("<li>")
              .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
              .addClass("disabled")
              .append(item.label)
              .appendTo(ul).on("click", function (e) {
                  e.preventDefault();
                  e.stopImmediatePropagation();
              });
        }
    }
};
function ProviderSearchFromclear() {
    $("#txtReferredFrom").attr("disabled", false);
    $("#txtReferredFrom").val("");
    return false;
}
function ProviderSelectedFrom(event, ui) {
    var ProviderDetails = JSON.parse(ui.item.val);
    var txtProviderSearch = document.getElementById("txtReferredFrom");

    txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
    txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(ProviderDetails);
    txtProviderSearch.value = ui.item.label;
   
    $("#txtReferredFrom").attr("disabled", "disabled");
    return false;
}

//Referred to
var intProviderlenTo = -1;
var arrProviderTo = [];
var flagTo = 0



$("#imgClearProviderToText").on("click", function () {
    $('#txtReferredTo').val('').focus();
    intProviderlenTo = -1;
    arrProviderTo = [];
    $(".ui-autocomplete").hide();
});
$("#txtReferredTo").autocomplete({
    source: function (request, response) {
        if ($("#txtReferredTo").val().trim().length > 2) {
            if (intProviderlenTo == 0) {

                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                this.element.on("keydown", PreventTyping);
                arrProviderTo = [];
                var strkeyWords = $("#txtReferredTo").val().split(' ');
                var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                var sIsMenulevel = "";
                var WSData = {
                    text_searched: strkeyWords[0],
                    IsMenulevel: sIsMenulevel,
                };
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "./frmFindReferralPhysician.aspx/GetProviderDetailsByTokens",
                    data: JSON.stringify(WSData),
                    dataType: "json",
                    success: function (data) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        $("#txtReferredTo").off("keydown", PreventTyping);
                        flagTo = 0;
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
                                results = FilterAuth(jsonData.Matching_Result, request.term);
                            else
                                results = jsonData.Matching_Result;

                            arrProviderTo = jsonData.Matching_Result;
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
            else if (intProviderlenTo != -1) {

                var results = FilterAuth(arrProviderTo, request.term);
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
    select: ProviderSelectedTo,
    open: function () {
        $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtReferredTo').width());
        $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
        $('#txtReferredTo').focus();
    },
    focus: function () { return false; }
}).on("paste", function (e) {
    intProviderlenTo = -1;
    arrProviderTo = [];
    $(".ui-autocomplete").hide();
}).on("input", function (e) {
    $("#txtReferredTo").css("color", "black").attr({ "data-phy-id": "0", "data-phy-details": "" });

    if ($("#txtReferredTo").val().charCodeAt(e.currentTarget.value.length - 1) == 10) {
        e.preventDefault();
        $("#txtReferredTo").val(e.currentTarget.value.substr(0, e.currentTarget.value.length - 1));
        return false;
    }
    if ($("#txtReferredTo").val().charAt(e.currentTarget.value.length - 1) == " ") {
        if (e.currentTarget.value.split(" ").length > 2)
            intProviderlenTo = intProviderlenTo + 1;
        else
            intProviderlenTo = 0;
    }
    else {
        if ($("#txtReferredTo").val().length != 0 && intProviderlenTo != -1) {
            intProviderlenTo = intProviderlenTo + 1;
        }

        if ($("#txtReferredTo").val().length == 0 || $("#txtReferredTo").val().indexOf(" ") == -1) {
            intProviderlenTo = -1;
            arrProviderTo = [];
            $(".ui-autocomplete").hide();
        }
    }
}).data("ui-autocomplete")._renderItem = function (ul, item) {
    if (item.label != "No matches found.") {
        if (flagTo == 0) {
            flagTo = 1;
            $("<li class='alinkstyle'>")
             .attr({ "data-value": "Add", "data-val": "Add" }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
             .append("Click to add Physician").addClass('alinkstyle').css("font-style", "italic")
             .appendTo(ul).on("click", function (e) {
                 e.preventDefault();
                 e.stopImmediatePropagation();
                 var obj = new Array();

                 localStorage.removeItem("IsEFax");
                 localStorage.setItem("IsEnableGrid", "false");
                 $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
                 $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px";
                 var sPath = "frmPhysicianLibray.aspx";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
                 $(top.window.document).find("#TabPhysicianLibrary").modal("show");
                 $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
                 });
                 return false;
             });;
            return $("<li>")
             .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
             .append(item.label)
             .appendTo(ul);
        }
        if (flagTo != 0) {
            return $("<li>")
              .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
              .append(item.label)
              .appendTo(ul);
        }
    }
    else {
        if (flagTo == 0) {
            flagTo = 1;
            $("<li class='alinkstyle'>")
             .attr({ "data-value": "Add", "data-val": "Add" }).css({ "border-bottom": "1px solid #ccc", "font-style": "italic", "margin-bottom": "3px", "padding-bottom": "3px" })
             .append("Click to add Physician").addClass('alinkstyle')
             .appendTo(ul).on("click", function (e) {
                 var obj = new Array();

                 localStorage.removeItem("IsEFax");
                 localStorage.setItem("IsEnableGrid", "false");
                 $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
                 $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
                 $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px";
                 var sPath = "frmPhysicianLibray.aspx";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
                 $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
                 $(top.window.document).find("#TabPhysicianLibrary").modal("show");
                 $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
                 });
                 return false;
             });;;
            return $("<li>")
             .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
             .addClass("disabled")
             .append(item.label)
             .appendTo(ul).on("click", function (e) {
                 e.preventDefault();
                 e.stopImmediatePropagation();
             });
        }
        if (flagTo != 0) {
            return $("<li>")
              .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
              .addClass("disabled")
              .append(item.label)
              .appendTo(ul).on("click", function (e) {
                  e.preventDefault();
                  e.stopImmediatePropagation();
              });
        }
    }
};



function ProviderSearchToclear() {
    $("#txtReferredTo").attr("disabled", false);
    $("#txtReferredTo").val("");
    return false;
}

function ProviderSelectedTo(event, ui) {
    var ProviderDetails = JSON.parse(ui.item.val);
    var txtProviderSearch = document.getElementById("txtReferredTo");

    txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
    txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(ProviderDetails);
    txtProviderSearch.value = ui.item.label;
   
    $("#txtReferredTo").attr("disabled", "disabled");
    return false;
}



function OpenInsuranceScreen() {

    if (document.getElementById("hdnHumanDetails").value != null) {
        var sHumandatails = new Array();
        sHumandatails = document.getElementById("hdnHumanDetails").value.split('~');
        setTimeout(
           function () {
               var oWnd = GetRadWindow();
               var oManager = oWnd.get_windowManager();
               var childWindow = oManager.BrowserWindow.radopen("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + sHumandatails[0] + "&InsuranceType=true&LastName=" + sHumandatails[1] + "&FirstName=" + sHumandatails[2] + "&ExAccountNo=" + sHumandatails[3] + "&PatientType=" + sHumandatails[4] + "&EncounterId=" + sHumandatails[5] + "&CurrentProcess=" + sHumandatails[6], "ctl00_DemographicsModalWindow");
               SetRadWindowProperties(childWindow, 575, 1160);
               childWindow.add_close(ClosePatIns);
           }, 0);

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
    var Result = args.get_argument();
    if (Result) {
        if (Result.InsPlanID != "" && Result.InsPlanID != null) {
            var Data = [Result.InsPlanID, Result.id];
            $.ajax({
                type: "POST",
                url: "frmAuthorization.aspx/GetInsurancebyId",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    var Plandata = $.parseJSON(data.d);
                    if (Plandata.InsuranceList != null && Plandata.InsuranceList != undefined && Plandata.InsuranceList.length > 0) {
                        document.getElementById('txtauthinsplan').value = Plandata.InsuranceList[0].Ins_Plan_Name;
                        document.getElementById('txtauthinsplan').title = Plandata.InsuranceList[0].Ins_Plan_Name;
                        document.getElementById("txtauthinsplan").setAttribute("planid", Result.InsPlanID);

                    }
                    if (Plandata.PatInsurance != null && Plandata.PatInsurance != undefined && Plandata.PatInsurance.length > 0) {
                        document.getElementById('txtPCPName').value = Plandata.PatInsurance[0].PCP_Name;
                        document.getElementById('txtPCPName').title = Plandata.PatInsurance[0].PCP_Name;
                        document.getElementById("txtPCPName").setAttribute("pcpid", Plandata.PatInsurance[0].PCP_ID);

                    }
                    if (Plandata.CarrierLisr != null && Plandata.CarrierLisr != undefined) {
                        document.getElementById('txtauthPayer').value = Plandata.CarrierLisr.Carrier_Name;
                        document.getElementById('txtauthPayer').title = Plandata.CarrierLisr.Carrier_Name;
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

    }
}

$('#txtauthnumber').keypress(function (e) {
    var regex = new RegExp("^[a-zA-Z0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
});
