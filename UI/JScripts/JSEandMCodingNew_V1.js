//If any changes are made in this file increase value of version in the query string in all files referencing this javascript file 
//Only for AngularJS scripts

var DeleteArray = [];
var DeleteArrayICD = [];
var UserRole;
//var BillingWFProcess;
var EnableScreen = "";
var EnablePriRbtn = "";
var EnableScreenAdditinalICD = "";
var CheckboxValue;
var BillingInstrlen = -1;
var lstSelectedCPT = new Array();
var arrPatient = [];
var bSubmitted = false;
var bSaveCheck = false;
var intCPTLength = -1;
var arrICD10Codes = [];
var cptlist;
var isclosemodal = 0;
var Esuperbillclicked = "N";
$(document).ready(function () {
    //CAP-1534
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $('#txtUnits').val("6");
    localStorage.setItem("CCAndEandMAutosave", "false");
    EnableSaveButton();
    
   
    $(top.window.document).find("#btnMinimizeViewResultCPT").css({ "display": "block" });
    $(top.window.document).find("#btnMinimizeViewResultICD").css({ "display": "block" });
});

function EnableSaveButton() {

    $('#btnSave').removeAttr("disabled");
    $('#btnSubAllForSuperbill').removeAttr("disabled");
    if (UserRole != undefined && UserRole.toUpperCase() == 'SCRIBE') {
        $("#btnSubAllForSuperbill").attr("disabled", "disabled");
        bSubmitted = false;
    }
    //if (BillingWFProcess != undefined && BillingWFProcess != "BATCHING_WAIT") {
    //    $("#btnSubAllForSuperbill").attr("disabled", "disabled");
    //    bSubmitted = false;
    //}
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}

function mouseDown(n) {

    try {
        if (2 == event.button || 3 == event.button)
            return !1
    }
    catch (n) {
        if (3 == n.which)
            return !1
    }
}
document.oncontextmenu = function () {

    return !1
},
    document.onmousedown = mouseDown;
var ValEnableScreen = ""; // window.location.search.toString().split('?')[1];
var myapp = angular.module('EandMCodingapp', []);
/*=====new add angular Global function =======*/
myapp.config(function ($provide) {
    $provide.decorator('$exceptionHandler', function ($delegate) {
        return function (exception, cause) {
            HandlerAngularjsError(exception);
        };
    });
});
myapp.controller('EandMCodingCtrl', function ($scope, $http) {
    localStorage.setItem("MovetofromEandM", "False");
    ValEnableScreen = window.location.search.toString().split('?')[1];
    //Cap - 1623
    if (ValEnableScreen == undefined && ValEnableScreen == null) {
        ValEnableScreen = "";
    }
    // $scope.sortColumn = 'Order';
    $http({
        url: "WebServices/EandMCodingService.asmx/LoadEandMCodingCPTTable",
        dataType: 'json',
        method: 'POST',
        data: '{strEandMCodingCPT: "Load", sEnableScreen:"'+ ValEnableScreen +'"}',
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "X-Requested-With": "XMLHttpRequest"
        }
    }).success(function (response, status, headers, config) {
        $("span[mand=Yes]").addClass('MandLabelstyle');
        $("span[mand=Yes]").each(function () {
            $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
        });
        var str = response.d;
        var test = JSON.parse(str);
        $scope.Modifiers1 = JSON.parse(test.ModifierList);
        $scope.Modifiers2 = JSON.parse(test.ModifierList);
        $scope.Modifiers3 = JSON.parse(test.ModifierList);
        $scope.Modifiers4 = JSON.parse(test.ModifierList);
        $scope.EandMCodingCPTTable = test.ProcedureList;
        $scope.EandMCodingICDTable = test.ICDList;
        $scope.AssEandMCodingICDTable = test.AssICDlist;
        cptlist = test.ProcedureList;
        UserRole = test.UserRole.toUpperCase();
        var CloseVisible = test.CloseVisible;

        //BillingWFProcess = test.BillingWFProcess;

        //  $scope.setOrder('Amount');
        // $scope.orderByFieldICD = 'ICDCode';
        //$scope.reverseSort = false;
        if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT' && UserRole.toUpperCase() != 'OFFICE MANAGER' && UserRole.toUpperCase() != 'CODER' && UserRole.toUpperCase() != 'TECHNICIAN') {
            $('#divICDPanel *').attr("disabled", "disabled").off('click');
        }
        EnablePriRbtn = test.EnablePriRbtn;
        //Jira - #CAP-80
        //sessionStorage.setItem("Is_CMG_Ancillary", test.IsCMGAncillary);
        if (test.IsCMGAncillary != undefined && test.IsCMGAncillary != null)
        localStorage.setItem("Is_CMG_Ancillary", test.IsCMGAncillary);
        if (test.BillingInstruction != "")
            $('#txtBillingInstruction').val(test.BillingInstruction);

        EnableScreen = test.EnableScreen;

        var saveEnable = test.SaveEnable;
        if (EnableScreen != "") {
            $('#AngularDiv').find('input').attr("disabled", "disabled");
           // $('#AngularDiv').find('button').attr("disabled", "disabled");
            $('#AngularDiv').find('button').not('[id*="btnClose"]').attr("disabled", "disabled");
            $('#AngularDiv').find('textarea').attr("disabled", "disabled");
            $('#AngularDiv').find('select').attr("disabled", "disabled");
            $("#btnSubAllForSuperbill").attr("disabled", "disabled");
            localStorage.setItem("bSave", "true");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        }
        if (saveEnable == "true") {
            if (EnableScreen == "")
                $('#btnSubAllForSuperbill').removeAttr("disabled");
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
        else if (saveEnable == "false") {

            localStorage.setItem("bSave", "true");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        }
        if (UserRole.toUpperCase() == 'SCRIBE') {
            $("#btnSubAllForSuperbill").attr("disabled", "disabled");
            bSubmitted = false;
        }
        //if (BillingWFProcess != "BATCHING_WAIT")
        //{
        //    $("#btnSubAllForSuperbill").attr("disabled", "disabled");
        //    bSubmitted = false;
        //}
        //BugID:46020 - to not enable Save and Save&Submit by default - END -
        if (CloseVisible == "false") {
            document.getElementById('btnClose').style.display = "none";
            document.getElementById('btnClose').removeAttribute("class");
        }
        //Cap - 1301
        $scope.RefershGrid();
       { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    })
        .error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            //else
            //    alert(error.Message + ".Please Contact Support!");

            else {
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            }
        });

    var arrCPTs = [];
    var bBool = false;
    var bcheck = true;
    $("#txtCPT").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "WebServices/EandMCodingService.asmx/SearchCPTDescrptionText",
                    //CAP-1585
                   // data: "{\"text\":\"" + document.getElementById("txtCPT").value + "|" + "txtCPT" + "\"}",
                    data: JSON.stringify({ text: document.getElementById("txtCPT").value + "|txtCPT" }),
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
                                arrCPTs.push(item.label + "~" + item.value);
                                return {
                                    label: item.label,
                                    value: item.value
                                }
                            }));
                        }

                        $("#txtCPT").focus();
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    },
                    error: function OnError(xhr) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        if (xhr.status == 999)
                            window.location = "/frmSessionExpired.aspx";
                        else {
                            //CAP-798
                            if (isValidJSON(xhr.responseText)) {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);
                            alert("USER MESSAGE:\n" +
                                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                "Message: " + log.Message);
                        }
                            else {
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry.");
                            }
                        }
                    }
                });
            }
            if ($("#txtCPT").val().length > 1) {
                if (arrCPTs.length != 0) {
                    var results = $scope.PossibleCombination(arrCPTs, request.term);
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
                                label: item.split('~')[0] + '~' + item.split('~')[1],
                                //Cap - 1301
                                //value: item.split('~')[2] + '~' + item.split('~')[3]
                                value: item.split('~')[2] + '~' + item.split('~')[3] + '~' + item.split('~')[4]
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
                //if (JSON.stringify($scope.EandMCodingCPTTable).indexOf(ui.item.label.split('~')[0]) == -1) {

                var CPTCode = "";
                for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                    if (i == 0)
                        CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
                    else

                        CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
                }

                var Modifier = "";
                var MaxOrderList = new Array();
                var iOrder;

                var flag = 0;
                var iMax = 0;
                //Cap - 1301
                //if ($scope.EandMCodingCPTTable.length != 0) {
                //    for (iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                //        if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) <= parseFloat(ui.item.value.split('~')[1])) {

                //            flag = 1;
                //            //$scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat((iMax + 1)), 'Amount': ui.item.value, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                //            $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                //            $scope.EandMCodingCPTTable.join();
                //            //push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iMax + 1, 'Amount': ui.item.label.split('~')[2] });
                //            break;
                //        }

                //    }

                if ($scope.EandMCodingCPTTable.length != 0) {
                    for (iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                        iOrder = parseFloat($scope.EandMCodingCPTTable[iMax].Order);
                        if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) > parseFloat(ui.item.value.split('~')[1])) {

                            flag = 1;
                            $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                            $scope.EandMCodingCPTTable.join();
                            $scope.EandMCodingCPTTable = $scope.EandMCodingCPTTable;
                            break;
                        }
                        else if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) == parseFloat(ui.item.value.split('~')[1])) {


                            if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) < parseFloat(ui.item.value.split('~')[2])) {
                                flag = 1;
                                // $scope.productAttributes.Products.indexOf(site)
                                $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                $scope.EandMCodingCPTTable.join();
                                // $scope.$apply();
                                break;

                            }
                            else if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) == parseFloat(ui.item.value.split('~')[2])) {


                                if (($scope.EandMCodingCPTTable[iMax].CPTCode) > ui.item.label.split('~')[0]) {
                                    flag = 1;
                                    $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                    $scope.EandMCodingCPTTable.join();
                                    break;
                                }
                                else {
                                    continue;
                                }



                            }

                            //else if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) > parseFloat(ui.item.value.split('~')[2]) &&
                            //    iMax + 1 < $scope.EandMCodingCPTTable.length && parseFloat($scope.EandMCodingCPTTable[iMax + 1].Order) != iOrder) {
                            //    flag = 1;
                            //    $scope.EandMCodingCPTTable.splice(iMax + 1, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                            //    $scope.EandMCodingCPTTable.join();
                            //    break;

                            //}
                        }
                        else {

                            if (iMax == $scope.EandMCodingCPTTable.length - 1) {
                                flag = 1;
                                // $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                $scope.EandMCodingCPTTable.splice(iMax + 1, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                $scope.EandMCodingCPTTable.join();
                                break;
                            }
                            else {
                                continue;
                            }

                        }

                    }

                    if (flag == 0) {
                        //Cap - 1301
                        //$scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                        $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                    }
                    else {
                        var flagcpt = 0;
                        var flagx = 0;
                        for (var j = 0; j < $scope.EandMCodingCPTTable.length; j++) {
                            flagx = 0;
                            if ($scope.EandMCodingCPTTable[j].CPTCode.indexOf(ui.item.label.split('~')[0]) >= 0) {
                                flagcpt = 1;
                                flagx = 1;
                            }
                            //if (flagcpt == 1 && flagx == 0)
                            //    $scope.EandMCodingCPTTable[j].Order = (parseFloat($scope.EandMCodingCPTTable[j].Order) + 1);


                        }
                    }

                }
                else {
                    //Cap - 1301
                    //$scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                    $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                }


                if ($scope.EandMCodingCPTTable.length != 0) {
                    for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                        MaxOrderList.push(parseInt($scope.EandMCodingCPTTable[iMax].Order));
                    }
                }
                else {
                    MaxOrderList.push("0");
                }
                MaxOrderList.sort();
                // iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                //  $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder, 'Amount': ui.item.label.split('~')[2] });
                lstSelectedCPT.push(ui.item.label);
                $scope.RefershGrid();
                BindCPTtable();
                $scope.EnableSaveButton();
                /*$scope.$apply();*/

                //$.ajax({
                //    type: "POST",
                //    url: "WebServices/EandMCodingService.asmx/GetModifierforCPT",
                //    contentType: "application/json; charset=utf-8",
                //    data: JSON.stringify({
                //        "CPT": ui.item.label.split('~')[0],
                //        "CPTList": CPTCode
                //    }),
                //    dataType: "json",
                //    async: false,
                //    success: function (data) {
                //        var Modifier = (data.d);
                //        var MaxOrderList = new Array();
                //        var iOrder;
                //        //if ($scope.EandMCodingCPTTable.length != 0) {
                //        //    for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                //        //        MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                //        //    }
                //        //}
                //        //else {
                //        //    MaxOrderList.push("0");
                //        //}
                //        //MaxOrderList.sort();
                //        //iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;

                //        //order by charge amount 
                //        var flag = 0;
                //        var iMax = 0
                //        cptlist = $scope.EandMCodingCPTTable;
                //        if (cptlist.length != 0) {
                //            for (iMax = 0; iMax < cptlist.length; iMax++) {
                //                if (parseFloat(cptlist[iMax].Amount) <= parseFloat(ui.item.value)) {

                //                    flag = 1;
                //                    cptlist.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat((iMax + 1)), 'Amount': ui.item.value })
                //                    cptlist.join();
                //                    //push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iMax + 1, 'Amount': ui.item.label.split('~')[2] });
                //                    break;
                //                }

                //            }
                //            if (flag == 0) {
                //                cptlist.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat((iMax + 1)), 'Amount': ui.item.value });

                //            }
                //            else {
                //                var flagcpt = 0;
                //                var flagx = 0;
                //                for (var j = 0; j < cptlist.length; j++) {
                //                    flagx = 0;
                //                    if (cptlist[j].CPTCode.indexOf(ui.item.label.split('~')[0]) >= 0) {
                //                        flagcpt = 1;
                //                        flagx = 1;
                //                    }
                //                    if (flagcpt == 1 && flagx == 0)
                //                        cptlist[j].Order = (parseFloat(cptlist[j].Order) + 1);


                //                }

                //            }
                //        }
                //        else {
                //            cptlist.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat('1'), 'Amount': ui.item.value });

                //        }
                //        //if ($scope.EandMCodingCPTTable.length != 0) {
                //        //    for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                //        //        MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                //        //    }
                //        //}
                //        //else {
                //        //    MaxOrderList.push("0");
                //        //}
                //        //MaxOrderList.sort();
                //        //iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                //        var datatable = cptlist;
                //        var emptydata = [];



                //        lstSelectedCPT.push(ui.item.label);
                //        //  $scope.setOrder('Amount');


                //        $scope.EandMCodingCPTTable = emptydata;

                //        $scope.EandMCodingCPTTable = datatable;

                //        // BindCPTtable();
                //        $scope.EnableSaveButton();
                //        // $scope.$apply();
                //        $scope.RefershGrid();

                //    },
                //    error: function OnError(xhr) {
                //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //        if (xhr.status == 999)
                //            window.location = xhr.statusText;
                //        else {
                //            var log = JSON.parse(xhr.responseText);
                //            console.log(log);
                //            alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);
                //            return false;
                //        }
                //    }
                //});


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
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtCPT").val().length <= 1)
                bBool = false;
            else
                bBool = true;
            $("#txtCPT").focus();
            bcheck = false;
        }
        else if (e.which == 46) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bBool = false;
            bcheck = false;
        }
        else {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }
    }).on("input", function (e) {
        document.getElementById('txtCPTDescription').value = "";
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') != "undefined" && jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } }

        if ($("#txtCPT").val().length >= 1) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') != "undefined" && jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } }

            if (!bBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
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

    function BindCPTtable() {
        var CPTCode = "";
        for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
            if (i == 0)
                CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
            else

                CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
        }
        //$.ajax({
        //    type: "POST",
        //    url: "WebServices/EandMCodingService.asmx/SetModifierforCPT",
        //    contentType: "application/json; charset=utf-8",
        //    data: JSON.stringify({

        //        "CPTList": CPTCode
        //    }),
        //    dataType: "json",
        //    async: false,
        //    success: function (data) {
        //        if (data.d != "") {
        //            var list = $.parseJSON(data.d);
        //            for (var i = 0; i < list.length; i++) {
        //                for (var j = 0; j < $('#tblEandMCodingCPT > tbody  > tr').length; j++) {

        //                    if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[7].innerText.trim() == list[i].split('~')[0]) {

        //                        for (var k = 0; k < $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children.length; k++) {

        //                            if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value == "") {

        //                                $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value = list[i].split('~')[1];
        //                                break;
        //                            }
        //                            else if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value != "" && $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value == list[i].split('~')[1]) {
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }

        //    },
        //    error: function OnError(xhr) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        if (xhr.status == 999)
        //            window.location = xhr.statusText;
        //        else {
        //            var log = JSON.parse(xhr.responseText);
        //            console.log(log);
        //            alert("USER MESSAGE:\n" +
        //                            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
        //                           "Message: " + log.Message);
        //            return false;
        //        }
        //    }
        //});
    }
    /*function BindDeleteCPTtable() {
        var CPTCode = "";
        for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
            if (i == 0)
                CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
            else

                CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
        }
        $.ajax({
            type: "POST",
            url: "WebServices/EandMCodingService.asmx/DeleteModifierforCPT",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({

                "CPTList": CPTCode
            }),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.d != "") {
                    var list = $.parseJSON(data.d);
                    for (var i = 0; i < list.length; i++) {
                        for (var j = 0; j < $('#tblEandMCodingCPT > tbody  > tr').length; j++) {

                            if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[7].innerText.trim() == list[i].split('~')[0]) {

                                for (var k = 0; k < $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children.length; k++) {

                                    if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value == list[i].split('~')[1]) {

                                        $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value = "";
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }

            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = xhr.statusText;
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
    }*/
    $("#txtCPTDescription").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "WebServices/EandMCodingService.asmx/SearchCPTDescrptionText",
                    //CAP-1585
                    data: JSON.stringify({ text: document.getElementById("txtCPTDescription").value + "|txtCPTDescription" }),
                   // data: "{\"text\":\"" + document.getElementById("txtCPTDescription").value + "|" + "txtCPTDescription" + "\"}",
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
                                arrCPTs.push(item.label + "~" + item.value);
                                return {
                                    label: item.label,
                                    value: item.value
                                }
                            }));
                        }
                        $("#txtCPTDescription").focus();
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    },
                    error: function OnError(xhr) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        if (xhr.status == 999)
                            window.location = "/frmSessionExpired.aspx";
                        else {
                            if (isValidJSON(xhr.responseText)) {
                                var log = JSON.parse(xhr.responseText);
                                console.log(log);
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                    "Message: " + log.Message);
                            }
                            else {
                                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry.");
                            }
                        }     
                    }
                });
            }
            if ($("#txtCPTDescription").val().length > 3) {
                if (arrCPTs.length != 0) {
                    var results = $scope.PossibleCombination(arrCPTs, request.term);
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
                                label: item.split('~')[0] + '~' + item.split('~')[1],
                                //Cap - 1301
                                //value: item.split('~')[2] + '~' + item.split('~')[3]
                                value: item.split('~')[2] + '~' + item.split('~')[3] + '~' + item.split('~')[4]
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

            if (ui.item.label != "No matches found.") {

                var CPTCode = "";
                for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                    if (i == 0)
                        CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
                    else

                        CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
                }

                var Modifier = "";
                var MaxOrderList = new Array();
                var iOrder;

                var flag = 0;
                var iMax = 0
                if ($scope.EandMCodingCPTTable.length != 0) {
                    for (iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                        //if (parseFloat($scope.EandMCodingCPTTable[iMax].Amount) <= parseFloat(ui.item.value)) {
                            //Cap - 1301
                        iOrder = parseFloat($scope.EandMCodingCPTTable[iMax].Order);
                        //if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) <= parseFloat(ui.item.value.split('~')[1])) {

                        //    flag = 1;
                        //    $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                        //    $scope.EandMCodingCPTTable.join();
                        //    //push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iMax + 1, 'Amount': ui.item.label.split('~')[2] });
                        //    break;
                        //}

                        if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) > parseFloat(ui.item.value.split('~')[1])) {

                            flag = 1;
                            $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                            $scope.EandMCodingCPTTable.join();
                            $scope.EandMCodingCPTTable = $scope.EandMCodingCPTTable;
                            break;;
                        }
                        else if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) == parseFloat(ui.item.value.split('~')[1])) {


                            if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) < parseFloat(ui.item.value.split('~')[2])) {
                                flag = 1;
                                // $scope.productAttributes.Products.indexOf(site)
                                $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                $scope.EandMCodingCPTTable.join();
                                // $scope.$apply();
                                break;

                            }
                            else if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) == parseFloat(ui.item.value.split('~')[2])) {


                                if (($scope.EandMCodingCPTTable[iMax].CPTCode) > ui.item.label.split('~')[0]) {
                                    flag = 1;
                                    $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                    $scope.EandMCodingCPTTable.join();
                                    break;
                                }
                                else {
                                    continue;
                                }



                            }

                            //else if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) > parseFloat(ui.item.value.split('~')[2]) &&
                            //    iMax + 1 < $scope.EandMCodingCPTTable.length && parseFloat($scope.EandMCodingCPTTable[iMax + 1].Order) != iOrder) {
                            //    flag = 1;
                            //    $scope.EandMCodingCPTTable.splice(iMax + 1, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                            //    $scope.EandMCodingCPTTable.join();
                            //    break;

                            //}
                        }
                        else {

                            if (iMax == $scope.EandMCodingCPTTable.length - 1) {
                                flag = 1;
                                $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });

                                break;
                            }
                            else {
                                continue;
                            }

                        }

                    }
                    if (flag == 0) {
                        //Cap - 1301
                        //$scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                        $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });

                    }
                    else {
                        var flagcpt = 0;
                        var flagx = 0;
                        for (var j = 0; j < $scope.EandMCodingCPTTable.length; j++) {
                            flagx = 0;
                            if ($scope.EandMCodingCPTTable[j].CPTCode.indexOf(ui.item.label.split('~')[0]) >= 0) {
                                flagcpt = 1;
                                flagx = 1;
                            }
                            //if (flagcpt == 1 && flagx == 0)
                            // $scope.EandMCodingCPTTable[j].Order = (parseFloat($scope.EandMCodingCPTTable[j].Order) + 1);


                        }
                    }

                }
                else {
                    //Cap - 1301
                   // $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                    $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': ui.item.value.split('~')[1], 'RVU': ui.item.value.split('~')[2], 'Amount': ui.item.value.split('~')[0], 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });

                }


                if ($scope.EandMCodingCPTTable.length != 0) {
                    for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                        MaxOrderList.push(parseInt($scope.EandMCodingCPTTable[iMax].Order));
                    }
                }
                else {
                    MaxOrderList.push("0");
                }
                MaxOrderList.sort();
                iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                //  $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder, 'Amount': ui.item.label.split('~')[2] });
                lstSelectedCPT.push(ui.item.label);
                $scope.RefershGrid();
                BindCPTtable();
                $scope.EnableSaveButton();
                /*$scope.$apply();*/

                //$.ajax({
                //    type: "POST",
                //    url: "WebServices/EandMCodingService.asmx/GetModifierforCPT",
                //    contentType: "application/json; charset=utf-8",
                //    data: JSON.stringify({
                //        "CPT": ui.item.label.split('~')[0],
                //        "CPTList": CPTCode
                //    }),
                //    dataType: "json",
                //    async: false,
                //    success: function (data) {
                //        var Modifier = (data.d);
                //        var MaxOrderList = new Array();
                //        var iOrder;

                //        var flag = 0;
                //        var iMax = 0
                //        if ($scope.EandMCodingCPTTable.length != 0) {
                //            for (iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                //                if (parseFloat($scope.EandMCodingCPTTable[iMax].Amount) <= parseFloat(ui.item.value)) {

                //                    flag = 1;
                //                    $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat((iMax + 1)), 'Amount': ui.item.value })
                //                    $scope.EandMCodingCPTTable.join();
                //                    //push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iMax + 1, 'Amount': ui.item.label.split('~')[2] });
                //                    break;
                //                }

                //            }
                //            if (flag == 0) {
                //                $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat((iMax + 1)), 'Amount': ui.item.value });

                //            }
                //            else {
                //                var flagcpt = 0;
                //                var flagx = 0;
                //                for (var j = 0; j < $scope.EandMCodingCPTTable.length; j++) {
                //                    flagx = 0;
                //                    if ($scope.EandMCodingCPTTable[j].CPTCode.indexOf(ui.item.label.split('~')[0]) >= 0) {
                //                        flagcpt = 1;
                //                        flagx = 1;
                //                    }
                //                    if (flagcpt == 1 && flagx == 0)
                //                        $scope.EandMCodingCPTTable[j].Order = (parseFloat($scope.EandMCodingCPTTable[j].Order) + 1);


                //                }
                //            }

                //        }
                //        else {
                //            $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': parseFloat('1'), 'Amount': ui.item.value });

                //        }


                //        if ($scope.EandMCodingCPTTable.length != 0) {
                //            for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                //                MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                //            }
                //        }
                //        else {
                //            MaxOrderList.push("0");
                //        }
                //        MaxOrderList.sort();
                //        iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                //        //  $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder, 'Amount': ui.item.label.split('~')[2] });
                //        lstSelectedCPT.push(ui.item.label);
                //        $scope.RefershGrid();
                //        BindCPTtable();
                //        $scope.EnableSaveButton();
                //        $scope.$apply();

                //    },
                //    error: function OnError(xhr) {
                //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //        if (xhr.status == 999)
                //            window.location = xhr.statusText;
                //        else {
                //            var log = JSON.parse(xhr.responseText);
                //            console.log(log);
                //            alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);
                //            return false;
                //        }
                //    }
                //});



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
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtCPTDescription").val().length <= 3)
                bBool = false;
            else
                bBool = true;
            $("#txtCPTDescription").focus();
            bcheck = false;
        }
        else if (e.which == 46) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bBool = false;
            bcheck = false;
        }
        else {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }
    }).on("input", function (e) {

        document.getElementById('txtCPT').value = "";
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtCPTDescription").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
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
    function FilterCodes(array, terms) {
        arrayOfTerms = terms.split(" ");
        if (arrayOfTerms.length > 0 && arrayOfTerms[0].trim() != "") {
            var first_resultant = array;
            var resultant;
            for (var i = 0; i < arrayOfTerms.length; i++) {
                if (i == 0) {
                    resultant = $.grep(first_resultant, function (item) {
                        return item.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) == 0;
                    });
                }
                else {
                    resultant = $.grep(first_resultant, function (item) {
                        return item.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
                    });
                }
                first_resultant = resultant;
            }
            return first_resultant;
        }
        else {
            return array;
        }
    }

    function FilterCodesdescription(array, terms) {
        arrayOfTerms = terms.split(" ");
        if (arrayOfTerms.length > 0 && arrayOfTerms[0].trim() != "") {
            var first_resultant = array;
            var resultant;
            for (var i = 0; i < arrayOfTerms.length; i++) {
                if (i == 0) {
                    resultant = $.grep(first_resultant, function (item) {
                        return item.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) >= 0;
                    });
                }
                else {
                    resultant = $.grep(first_resultant, function (item) {
                        return item.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
                    });
                }
                first_resultant = resultant;
            }
            return first_resultant;
        }
        else {
            return array;
        }
    }
    //Jira - #CAP-80
    // $('#dlstICD10').load("htmICD10.html?version=" + sessionStorage.getItem("ScriptVersion").split('|')[0].trim(), function () {
    $('#dlstICD10').load("htmICD10.html?version=" + localStorage.getItem("ScriptVersion").split('|')[0].trim(), function () {
        arrICD10Codes = $.map($('#dlstICD10 option'), function (li) {
            return $(li).attr("value");
        });
    });
    $("#txtICD10").autocomplete({
        source:
            function (request, response) {
                if (arrICD10Codes == null) {
                    //Jira - #CAP-80
                    //$.get("htmICD10.html?version" + sessionStorage.getItem("ScriptVersion").split('|')[0].trim()).done(function (file) {
                    $.get("htmICD10.html?version" + localStorage.getItem("ScriptVersion").split('|')[0].trim()).done(function (file) {
                        arrICD10Codes = $.map(file, function (li) {
                            return $(li).attr("value");
                        });
                    });
                }
                if ($("#txtICD10").val().length > 2) {
                    var results = FilterCodes(arrICD10Codes, request.term);

                    response($.map(results, function (item) {
                        return {
                            label: item,
                            value: item
                        }
                    }));
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
        minlength: 2,
        multiple: true,
        mustMatch: false,
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtICD10').width());
            $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            if (ui.item.label != "No matches found.") {
                if (JSON.stringify($scope.EandMCodingICDTable).indexOf(ui.item.label.split('~')[0]) == -1) {
                    //const sNoSeq = parseInt($scope.EandMCodingICDTable.length) + 1;
                    var max = 0;
                    $('.maxseq').each(function () {
                        $this = parseInt($(this).text().replace('B', ''));
                        if ($this > max) max = $this;
                    });
                    max = parseInt(max) + 1;

                    $scope.EandMCodingICDTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'ICDVersion': '0', 'btnDelete': 'Resources/Delete-Blue.png', 'IsPrimary': 'N', 'EandMICDID': '0', 'EnablePriRbtn': EnablePriRbtn, 'Sequence': 'B' + max });
                    $scope.RefershGrid();
                }
                $scope.EnableSaveButton();
            }
            bBool = false;
            $('#txtICD10').val("");
        }
    }).on("paste", function (e) {
        intCPTLength = -1;
        arrCPTs = [];
        $(".ui-autocomplete").hide();
    }).on("keydown", function (e) {

        if (e.which == 8) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtICD10").val().length <= 3)
                bBool = false;
            else
                bBool = true;
            $("#txtICD10").focus();
            bcheck = false;
        }
        else if (e.which == 46) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bBool = false;
            bcheck = false;
        }
        else {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }
    }).on("input", function (e) {

        document.getElementById('txtDescription').value = "";
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtICD10").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intCPTLength = 0;
        }
        else if ($("#txtICD10").val().length != 0 && intCPTLength != -1) {
            intCPTLength = intCPTLength + 1;
        }
        if ($("#txtICD10").val().length < 3) {
            intCPTLength = -1;
            arrCPTs = [];
            $(".ui-autocomplete").hide();
            bBool = false;
        }
    });
    $("#txtDescription").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "WebServices/EandMCodingService.asmx/SearchICDDescrptionText",
                    data: "{\"text\":\"" + document.getElementById("txtDescription").value + "|" + "txtDescription" + "|" + "ICD10" + "\"}",
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
                        $("#txtDescription").focus();
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
            if ($("#txtDescription").val().length > 3) {
                if (arrCPTs.length != 0) {
                    var results = $scope.PossibleCombination(arrCPTs, request.term);
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
            $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtDescription').width());
            $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            if (ui.item.label != "No matches found.") {
                if (JSON.stringify($scope.EandMCodingICDTable).indexOf(ui.item.label.split('~')[0]) == -1) {
                    //  $scope.EandMCodingICDTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'ICDVersion': 0, 'btnDelete': 'Resources/Delete-Blue.png', 'IsPrimary': 'N', 'EandMICDID': 0, 'Sequence': '6', 'EnablePriRbtn': EnablePriRbtn });
                    //const sNoSeq = parseInt($scope.EandMCodingICDTable.length) + 1;
                    var max = 0;
                    $('.maxseq').each(function () {
                        $this = parseInt($(this).text().replace('B', ''));
                        if ($this > max) max = $this;
                    });
                    max = parseInt(max) + 1;
                    $scope.EandMCodingICDTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'ICDVersion': '0', 'btnDelete': 'Resources/Delete-Blue.png', 'IsPrimary': 'N', 'EandMICDID': '0', 'Sequence': 'B' + max, 'EnablePriRbtn': EnablePriRbtn });
                    lstSelectedCPT.push(ui.item.label);
                    $scope.RefershGrid();
                }
                $scope.EnableSaveButton();
            }
            bBool = false;
            $('#txtDescription').val("");
        }
    }).on("paste", function (e) {
        intCPTLength = -1;
        arrCPTs = [];
        $(".ui-autocomplete").hide();
    }).on("keydown", function (e) {

        if (e.which == 8) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtDescription").val().length <= 3)
                bBool = false;
            else
                bBool = true;
            $("#txtDescription").focus();
            bcheck = false;
        }
        else if (e.which == 46) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bBool = false;
            bcheck = false;
        }
        else {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }
    }).on("input", function (e) {

        document.getElementById('txtICD10').value = "";
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtDescription").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intCPTLength = 0;
        }
        else if ($("#txtDescription").val().length != 0 && intCPTLength != -1) {
            intCPTLength = intCPTLength + 1;
        }
        else if ($("#txtDescription").val().length == 0 || $("#txtDescription").val().length == 1) {
            intCPTLength = -1;
            arrICDs = [];
            $(".ui-autocomplete").hide();
        }
        if ($("#txtDescription").val().length < 3) {
            arrCPTs = [];
            $(".ui-autocomplete").hide();
            bBool = false;
        }
    });



    $scope.LoadFavouriteCPTs = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var arrListCPTs = [];
        var type_Visit = "";
        if ($(top.window.document).find('#select_Type_Of_Visit').find(':selected')[0] != undefined)
            type_Visit = $(top.window.document).find('#select_Type_Of_Visit').find(':selected')[0].text;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",

            url: "WebServices/EandMCodingService.asmx/GetFavouriteCPTsByTypeOfVisit",
            data: "{\"type_of_visit\":\"" + type_Visit + "\"}",
            dataType: "json",
            success: function (data) {
                if (data.d != undefined && data.d != "") {
                    var jsonData = $.parseJSON(data.d);
                    for (var i = 0; i < jsonData.length; i++) {// Previously it was jsonData - 1
                        var arrCPT = jsonData[i];
                        arrListCPTs.push(arrCPT.CPTCode + "~" + arrCPT.CPTDesc + "~" + arrCPT.Category)
                    }
                }
                if (arrListCPTs.length != 0)
                    localStorage.setItem("PhysicianCPT", JSON.stringify(arrListCPTs));
                else
                    localStorage.setItem("PhysicianCPT", "");
                $scope.FillFavoriteCPTs();
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
    $scope.LoadFavouriteICDs = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var arrListICDs = [];
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "WebServices/EandMCodingService.asmx/GetFavouriteICDS",
            data: '',
            dataType: "json",
            success: function (data) {
                if (data.d != undefined && data.d != "") {
                    var jsonData = $.parseJSON(data.d);

                    for (var i = 0; i < jsonData.length; i++) {// Previously it was jsonData - 1
                        var arricd = jsonData[i];
                        arrListICDs.push(arricd.ICD10Code + "~" + arricd.ICD10Desc + "~" + arricd.Category)
                    }
                }
                localStorage.setItem("PhysicianICD", JSON.stringify(arrListICDs));
                $scope.FillFavoriteICDs();
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
    $scope.PossibleCombination = function (array, txtValue) {

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




    $scope.OpenFormViewCPT = function () {
        //BugID:51562 
        if ($(top.window.document).find('#divCPTFormView.in').length > 0) {
            $(top.window.document).find("#btnMaximizeViewResultCPT").click();
        }
        else {
            if ($(top.window.document).find('#divFormView.in').length > 0) {
                $(top.window.document).find("#btnICDClose").click();
                $(top.window.document).find("#btnMaximizeViewResultICD").click();
                $(top.window.document).find("#divFormView").css({ "position": "static" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "absolute", "height": "108%!important" });
                //$(top.window.document).find("#divFormView").css({ "position": "static" });
                $(top.window.document).find("#divFormView").css({ "z-index": "10000" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divCPTFormView").css({ "z-index": "10000" });
            }
            else {
                $(top.window.document).find("#main").css({ "position": "relative" });
                //$(top.window.document).find("#divCPTFormView").css({ "position": "absolute", "height": "108%!important" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divCPTFormView").css({ "z-index": "10000" });
            }
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "WebServices/EandMCodingService.asmx/fillTypeOfVisit",
                data: '',
                async: false,
                dataType: "json",
                success: function (data) {

                    var jsonData = $.parseJSON(data.d);
                    var visit = "";
                    $(top.window.document).find("#select_Type_Of_Visit option").remove();

                    $.each(jsonData[0], function (key, value) {
                        $(top.window.document).find("#select_Type_Of_Visit")
                            .append($("<option></option>")
                                .attr("value", key)
                                .text(value));
                    });
                    if (jsonData[1] != undefined && jsonData[1] != null) {
                        $(top.window.document).find("#select_Type_Of_Visit").each(function () {
                            $(this).find('option').filter(function () {
                                return this.text.toUpperCase() == jsonData[1].toUpperCase();
                            }).attr('selected', true);

                        })
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

            localStorage.setItem("PhysicianCPT", "");
            $scope.LoadFavouriteCPTs();
        }
    }
    $scope.FillFavoriteCPTs = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var PhysicianCPTs = localStorage.getItem("PhysicianCPT");
        if (PhysicianCPTs != undefined && PhysicianCPTs != "")
            PhysicianCPTs = $.parseJSON(PhysicianCPTs);

        if (PhysicianCPTs.length != 0) {
            var catlist = [];
            for (i = 0; i < PhysicianCPTs.length; i++) {
                if (i == 0 || PhysicianCPTs[i].split('~')[2] != PhysicianCPTs[i - 1].split('~')[2])
                    catlist.push(PhysicianCPTs[i].split('~')[2]);
            }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('230134');
            return;
        }

        $(top.window.document).find("#tbFavCPTsContainer #dynTr").remove();
        $(top.window.document).find("#CPTok")[0].disabled = true;
        $(top.window.document).find('#divCPTFormView').modal({ backdrop: 'static', "height": "108%!important", keyboard: false }, 'show');

        var table = $(top.window.document).find('#tbFavCPTsContainer');
        if (PhysicianCPTs.length != 0 && PhysicianCPTs.length <= 2 && catlist.length == 1) { RowCount = 2; }
        else {
            var favCPTCount = PhysicianCPTs.length;
            favCPTCount = favCPTCount + catlist.length;
            var RowCount = parseInt(favCPTCount / 3);
            var EmptyCount = favCPTCount % 3;
            RowCount = EmptyCount != 0 ? RowCount + 1 : RowCount;
        }
        var catIndex = 0;
        var icount = 0;
        var jcount = RowCount;
        var j = 0;
        var icase = 0;
        var switchno = 1;
        for (k = 0; k < 3; k++) {
            switch (switchno) {
                case 1:
                    for (j = 0; j < jcount; j++) {
                        var row = $('<tr id="dynTr"></tr>');
                        if (catlist[catIndex] == PhysicianCPTs[j].split('~')[2] && j == 0 && icount == 0) {
                            var cell1 = $('<td colspan="3" align="center" style="font-weight:bold;"></td>').text(PhysicianCPTs[j].split('~')[2]);
                            var cell2 = $('<td style="display:none;"></td>');
                            var cell3 = $('<td style="display:none;"></td>');
                            var cell4 = $('<td align="center"></td>');
                            var cell5 = $('<td></td>');
                            var cell6 = $('<td></td>');
                            var cell7 = $('<td align="center"></td>');
                            var cell8 = $('<td></td>');
                            var cell9 = $('<td></td>');
                            row.append(cell1);
                            row.append(cell2);
                            row.append(cell3);
                            row.append(cell4);
                            row.append(cell5);
                            row.append(cell6);
                            row.append(cell7);
                            row.append(cell8);
                            row.append(cell9);
                            table.append(row);
                            j--;
                            jcount--;
                            icount++;
                        }
                        else if (catlist[catIndex] == PhysicianCPTs[j].split('~')[2] && icount != 0) {
                            var id = "chkCPT" + (j + 1);
                            var cell1 = $('<td align="center"></td>');
                            cell1.append("<input type='checkbox' class='CPT'  id='" + id + "' onclick='chkFavCPT_Click();'/>");
                            var cell2 = $('<td></td>').text(PhysicianCPTs[j].split('~')[0]);
                            var cell3 = $('<td></td>').text(PhysicianCPTs[j].split('~')[1]);
                            var cell4 = $('<td align="center"></td>');
                            var cell5 = $('<td></td>');
                            var cell6 = $('<td></td>');
                            var cell7 = $('<td align="center"></td>');
                            var cell8 = $('<td></td>');
                            var cell9 = $('<td></td>');
                            row.append(cell1);
                            row.append(cell2);
                            row.append(cell3);
                            row.append(cell4);
                            row.append(cell5);
                            row.append(cell6);
                            row.append(cell7);
                            row.append(cell8);
                            row.append(cell9);
                            table.append(row);
                        }
                        else if (catlist[catIndex] != PhysicianCPTs[j].split('~')[2]) {
                            ++catIndex;
                            if (catlist[catIndex] == PhysicianCPTs[j].split('~')[2]) {
                                var cell1 = $('<td colspan="3" align="center" style="font-weight:bold;"></td>').text(PhysicianCPTs[j].split('~')[2]);
                                var cell2 = $('<td style="display:none;"></td>');
                                var cell3 = $('<td style="display:none;"></td>');
                                var cell4 = $('<td align="center"></td>');
                                var cell5 = $('<td></td>');
                                var cell6 = $('<td></td>');
                                var cell7 = $('<td align="center"></td>');
                                var cell8 = $('<td></td>');
                                var cell9 = $('<td></td>');
                                row.append(cell1);
                                row.append(cell2);
                                row.append(cell3);
                                row.append(cell4);
                                row.append(cell5);
                                row.append(cell6);
                                row.append(cell7);
                                row.append(cell8);
                                row.append(cell9);
                                table.append(row);
                                j--;
                                jcount--;
                            }
                        }
                    }
                    icase = jcount;
                    switchno++;
                    jcount = RowCount + RowCount;
                    break;
                case 2:
                    table.find('tr').each(function (rowIndex, r) {
                        if (rowIndex != 0) {
                            var cols = [];
                            if (PhysicianCPTs.length > 1) {
                                if (!(icase == PhysicianCPTs.length && PhysicianCPTs.length == 2)) {
                                    if (catlist[catIndex] == PhysicianCPTs[icase].split('~')[2] && j != 0) {
                                        $(this).find('td').each(function (colIndex, c) {

                                            if (colIndex == 3) {
                                                var id = "chkCPT" + (icase + 1);
                                                c.innerHTML = "<input type='checkbox' class='CPT'  id='" + id + "' onclick='chkFavCPT_Click();' />";
                                            }
                                            if (colIndex == 4) {
                                                c.textContent = PhysicianCPTs[icase].split('~')[0];
                                            }
                                            if (colIndex == 5) {
                                                c.textContent = PhysicianCPTs[icase].split('~')[1];
                                            }

                                        });
                                        icase++;
                                    }


                                    else if (catlist[catIndex] != PhysicianCPTs[icase].split('~')[2]) {
                                        ++catIndex;
                                        if (catlist[catIndex] == PhysicianCPTs[icase].split('~')[2]) {
                                            $(this).find('td').each(function (colIndex, c) {
                                                if (colIndex == 3) {
                                                    c.outerHTML = "<td colspan='3' align='center' style='font-weight:bold;'>" + PhysicianCPTs[icase].split('~')[2] + "</td>"
                                                    c.innerHTML = PhysicianCPTs[icase].split('~')[2];
                                                }
                                                if (colIndex == 4) {
                                                    c.outerHTML = "<td style='display:none;'></td>";
                                                }
                                                if (colIndex == 5) {
                                                    c.outerHTML = "<td style='display:none;'></td>";
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        }

                    });
                    switchno++;
                    jcount = RowCount + RowCount + RowCount;
                    break;
                case 3:
                    var max = RowCount - (PhysicianCPTs.length - (RowCount + RowCount))
                    table.find('tr').each(function (rowIndex, r) {

                        if (icase < jcount - max && rowIndex != 0) {
                            var cols = [];
                            if (catlist[catIndex] == PhysicianCPTs[icase].split('~')[2] && j != 0) {
                                $(this).find('td').each(function (colIndex, c) {

                                    if (colIndex == 6) {
                                        var id = "chkCPT" + (icase + 1);
                                        c.innerHTML = "<input type='checkbox' class='CPT'  id='" + id + "' onclick='chkFavCPT_Click();' />";
                                    }
                                    if (colIndex == 7) {
                                        c.textContent = PhysicianCPTs[icase].split('~')[0];
                                    }
                                    if (colIndex == 8) {
                                        c.textContent = PhysicianCPTs[icase].split('~')[1];
                                    }

                                });
                                icase++;
                            }
                            else if (catlist[catIndex] != PhysicianCPTs[icase].split('~')[2]) {
                                ++catIndex;
                                if (catlist[catIndex] == PhysicianCPTs[icase].split('~')[2]) {
                                    $(this).find('td').each(function (colIndex, c) {
                                        if (colIndex == 6) {
                                            c.outerHTML = "<td colspan='3' align='center' style='font-weight:bold;'>" + PhysicianCPTs[icase].split('~')[2] + "</td>"
                                            c.innerHTML = PhysicianCPTs[icase].split('~')[2];
                                        }
                                        if (colIndex == 7) {
                                            c.outerHTML = "<td style='display:none;'></td>";
                                        }
                                        if (colIndex == 8) {
                                            c.outerHTML = "<td style='display:none;'></td>";
                                        }
                                    });
                                }
                            }
                        }

                    });
                    break;
            }
        }
        $(top.window.document).find('#divCPTBody').append(table);
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    $scope.OpenFormViewICD = function () {
        if ($(top.window.document).find('#divFormView.in').length > 0) {
            $(top.window.document).find("#btnMaximizeViewResultICD").click();
        }
        else {
            if ($(top.window.document).find('#divCPTFormView.in').length > 0) {
                $(top.window.document).find("#btnCPTClose").click();
                $(top.window.document).find("#btnMaximizeViewResultCPT").click();
                //$(top.window.document).find("#divFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "static", "height": "108%!important" });
                $(top.window.document).find("#divFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divFormView").css({ "z-index": "10000" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "static" });
                $(top.window.document).find("#divCPTFormView").css({ "z-index": "10000" });
            }
            else {
                $(top.window.document).find("#main").css({ "position": "relative" });
                $(top.window.document).find("#divFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divFormView").css({ "z-index": "10000" });
            }
            if (localStorage.getItem("PhysicianICD") == "")
                $scope.LoadFavouriteICDs();
            else
                $scope.FillFavoriteICDs();
        }

    }


    $scope.FillFavoriteICDs = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        var PhysicianICDs = localStorage.getItem("PhysicianICD");
        PhysicianICDs = $.parseJSON(PhysicianICDs);
        var catlist = [];
        if (PhysicianICDs.length != 0) {
            for (i = 0; i < PhysicianICDs.length; i++) {
                if (i == 0 || PhysicianICDs[i].split('~')[2] != PhysicianICDs[i - 1].split('~')[2])
                    catlist.push(PhysicianICDs[i].split('~')[2]);
            }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('230155');
            return;
        }


        $(top.window.document).find("#tbFavICDsContainer #dynTr").remove();
        $(top.window.document).find("#ok")[0].disabled = true;
        $(top.window.document).find('#divFormView').modal({ backdrop: 'static', keyboard: false }, 'show');

        var table = $(top.window.document).find('#tbFavICDsContainer');
        var favICDCount = PhysicianICDs.length;
        favICDCount = favICDCount + catlist.length;
        var RowCount = parseInt(favICDCount / 3);
        var EmptyCount = favICDCount % 3;
        RowCount = EmptyCount != 0 ? RowCount + 1 : RowCount;
        var catIndex = 0;
        var icount = 0;
        var jcount = RowCount;
        var j = 0;
        var icase = 0;
        var switchno = 1;
        for (k = 0; k < 3; k++) {
            switch (switchno) {
                case 1:
                    for (j = 0; j < jcount; j++) {
                        var row = $('<tr id="dynTr"></tr>');
                        if (catlist[catIndex] == PhysicianICDs[j].split('~')[2] && j == 0 && icount == 0) {
                            var cell1 = $('<td colspan="3" align="center" style="font-weight:bold;"></td>').text(PhysicianICDs[j].split('~')[2]);
                            var cell2 = $('<td style="display:none;"></td>');
                            var cell3 = $('<td style="display:none;"></td>');
                            var cell4 = $('<td align="center"></td>');
                            var cell5 = $('<td></td>');
                            var cell6 = $('<td></td>');
                            var cell7 = $('<td align="center"></td>');
                            var cell8 = $('<td></td>');
                            var cell9 = $('<td></td>');
                            row.append(cell1);
                            row.append(cell2);
                            row.append(cell3);
                            row.append(cell4);
                            row.append(cell5);
                            row.append(cell6);
                            row.append(cell7);
                            row.append(cell8);
                            row.append(cell9);
                            table.append(row);
                            j--;
                            jcount--;
                            icount++;
                        }
                        else if (catlist[catIndex] == PhysicianICDs[j].split('~')[2] && icount != 0) {
                            var id = "chkicd" + (j + 1);
                            var cell1 = $('<td align="center"></td>');
                            cell1.append("<input type='checkbox' class='icd' id='" + id + "' onclick='chkFavICD_Click();' />");
                            var cell2 = $('<td></td>').text(PhysicianICDs[j].split('~')[0]);
                            var cell3 = $('<td></td>').text(PhysicianICDs[j].split('~')[1]);
                            var cell4 = $('<td align="center"></td>');
                            var cell5 = $('<td></td>');
                            var cell6 = $('<td></td>');
                            var cell7 = $('<td align="center"></td>');
                            var cell8 = $('<td></td>');
                            var cell9 = $('<td></td>');
                            row.append(cell1);
                            row.append(cell2);
                            row.append(cell3);
                            row.append(cell4);
                            row.append(cell5);
                            row.append(cell6);
                            row.append(cell7);
                            row.append(cell8);
                            row.append(cell9);
                            table.append(row);
                        }
                        else if (catlist[catIndex] != PhysicianICDs[j].split('~')[2]) {
                            ++catIndex;
                            if (catlist[catIndex] == PhysicianICDs[j].split('~')[2]) {
                                var cell1 = $('<td colspan="3" align="center" style="font-weight:bold;"></td>').text(PhysicianICDs[j].split('~')[2]);
                                var cell2 = $('<td style="display:none;"></td>');
                                var cell3 = $('<td style="display:none;"></td>');
                                var cell4 = $('<td align="center"></td>');
                                var cell5 = $('<td></td>');
                                var cell6 = $('<td></td>');
                                var cell7 = $('<td align="center"></td>');
                                var cell8 = $('<td></td>');
                                var cell9 = $('<td></td>');
                                row.append(cell1);
                                row.append(cell2);
                                row.append(cell3);
                                row.append(cell4);
                                row.append(cell5);
                                row.append(cell6);
                                row.append(cell7);
                                row.append(cell8);
                                row.append(cell9);
                                table.append(row);
                                j--;
                                jcount--;
                            }
                        }
                    }
                    icase = jcount;
                    switchno++;
                    jcount = RowCount + RowCount;
                    break;
                case 2:
                    table.find('tr').each(function (rowIndex, r) {
                        if (rowIndex != 0) {

                            var cols = [];
                            if (catlist[catIndex] == PhysicianICDs[icase].split('~')[2] && j != 0) {
                                $(this).find('td').each(function (colIndex, c) {

                                    if (colIndex == 3) {
                                        var id = "chkicd" + (icase + 1);
                                        c.innerHTML = "<input type='checkbox' class='icd' id='" + id + "' onclick='chkFavICD_Click();' />";
                                    }
                                    if (colIndex == 4) {
                                        c.textContent = PhysicianICDs[icase].split('~')[0];
                                    }
                                    if (colIndex == 5) {
                                        c.textContent = PhysicianICDs[icase].split('~')[1];
                                    }

                                });
                                icase++;
                            }
                            else if (catlist[catIndex] != PhysicianICDs[icase].split('~')[2]) {
                                ++catIndex;
                                if (catlist[catIndex] == PhysicianICDs[icase].split('~')[2]) {
                                    $(this).find('td').each(function (colIndex, c) {
                                        if (colIndex == 3) {
                                            c.outerHTML = "<td colspan='3' align='center' style='font-weight:bold;'>" + PhysicianICDs[icase].split('~')[2] + "</td>"
                                            c.innerHTML = PhysicianICDs[icase].split('~')[2];
                                        }
                                        if (colIndex == 4) {
                                            c.outerHTML = "<td style='display:none;'></td>";
                                        }
                                        if (colIndex == 5) {
                                            c.outerHTML = "<td style='display:none;'></td>";
                                        }
                                    });
                                }
                            }
                        }

                    });
                    switchno++;
                    jcount = RowCount + RowCount + RowCount;
                    break;
                case 3:
                    var max = RowCount - (PhysicianICDs.length - (RowCount + RowCount))
                    table.find('tr').each(function (rowIndex, r) {

                        if (icase < jcount - max && rowIndex != 0) {
                            var cols = [];
                            if (catlist[catIndex] == PhysicianICDs[icase].split('~')[2] && j != 0) {
                                $(this).find('td').each(function (colIndex, c) {

                                    if (colIndex == 6) {
                                        var id = "chkicd" + (icase + 1);
                                        c.innerHTML = "<input type='checkbox' class='icd' id='" + id + "' onclick='chkFavICD_Click();' />";
                                    }
                                    if (colIndex == 7) {
                                        c.textContent = PhysicianICDs[icase].split('~')[0];
                                    }
                                    if (colIndex == 8) {
                                        c.textContent = PhysicianICDs[icase].split('~')[1];
                                    }

                                });
                                icase++;
                            }
                            else if (catlist[catIndex] != PhysicianICDs[icase].split('~')[2]) {
                                ++catIndex;
                                if (catlist[catIndex] == PhysicianICDs[icase].split('~')[2]) {
                                    $(this).find('td').each(function (colIndex, c) {
                                        if (colIndex == 6) {
                                            c.outerHTML = "<td colspan='3' align='center' style='font-weight:bold;'>" + PhysicianICDs[icase].split('~')[2] + "</td>"
                                            c.innerHTML = PhysicianICDs[icase].split('~')[2];
                                        }
                                        if (colIndex == 7) {
                                            c.outerHTML = "<td style='display:none;'></td>";
                                        }
                                        if (colIndex == 8) {
                                            c.outerHTML = "<td style='display:none;'></td>";
                                        }
                                    });
                                }
                            }
                        }

                    });
                    break;
            }
        }
        $(top.window.document).find('#divBody').append(table);
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }

    $(top.window.document).find("#CPTok").click(function () {

        $scope.CPTokClick();
        return false;
    });
    var CheckedCPTs;
    $scope.CPTokClick = function () {


        if (sessionStorage != null && sessionStorage != undefined) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
        if ($(top.window) != null && $(top.window) != undefined) {
            CheckedCPTs = $.grep($(top.window.document).find('#tbFavCPTsContainer tr:not(:first)'), function (row) {
                for (var i = 0; i < $(row).find("input[type=checkbox]").length; i++) {
                    if ($(row).find("input[type=checkbox]")[i].checked)
                        return ($(row));
                }
            });

        }


        var MaxOrderList = new Array();
        var iOrder;
        if ($scope.EandMCodingCPTTable.length != 0) {
            for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                MaxOrderList.push(parseInt($scope.EandMCodingCPTTable[iMax].Order));
            }
        }
        else {
            MaxOrderList.push("0");
        }
        MaxOrderList.sort();
        iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;

        var sResultCPTs = "";
        for (var i = 0; i < CheckedCPTs.length; i++) {
            var cells = $(CheckedCPTs[i]).find('td');

            if (cells[0].textContent == "") {
                if (cells[0].childNodes.length != 0) {
                    if (cells[0].childNodes[0].checked) {
                        if (sResultCPTs == "")
                            sResultCPTs += cells[1].innerText.trim() + "~" + cells[2].innerText.trim() + "~" + iOrder;
                        else
                            sResultCPTs += "|" + cells[1].innerText.trim() + "~" + cells[2].innerText.trim() + "~" + iOrder;
                    }
                }
            }
            if (cells[3].textContent == "") {
                if (cells[3].childNodes.length != 0) {
                    if (cells[3].childNodes[0].checked) {
                        {
                            if (sResultCPTs == "")
                                sResultCPTs += cells[4].innerText.trim() + "~" + cells[5].innerText.trim() + "~" + iOrder;
                            else
                                sResultCPTs += "|" + cells[4].innerText.trim() + "~" + cells[5].innerText.trim() + "~" + iOrder;
                        }
                    }
                }
            }

            if (cells[6].textContent == "") {
                if (cells[6].childNodes.length != 0) {
                    if (cells[6].childNodes[0].checked) {
                        {
                            if (sResultCPTs == "")
                                sResultCPTs += cells[7].innerText.trim() + "~" + cells[8].innerText.trim() + "~" + iOrder;
                            else
                                sResultCPTs += "|" + cells[7].innerText.trim() + "~" + cells[8].innerText.trim() + "~" + iOrder;
                        }
                    }
                }
            }
            iOrder = iOrder + 1;
        }
        if (sResultCPTs == "") {
            DisplayErrorMessage('530001');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return;
        }

        var max = 0;
        $('.maxseq').each(function () {
            $this = parseInt($(this).text().replace('B', ''));
            if ($this > max) max = $this;
        });
        max = parseInt(max) + 1;


        var FinalCPTs = sResultCPTs.replace(/\"/g, "$");
        $http({
            url: "WebServices/EandMCodingService.asmx/GetFormviewCPTs",
            dataType: 'json',
            method: 'POST',
            data: '{sFormviewCPT: "' + FinalCPTs + '"}',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }

        }).success(function (response, status, headers, config) {
            if (response.d != "") {
                var test = JSON.parse(response.d);
                //Cap - 1634
                var flag = 0;
                var iMax = 0
                //Cap - 1301
                //if (test.ListofCPTs.length > 0) {


                //    for (var i = 0; i < test.ListofCPTs.length; i++) {
                //        var flag = 0;
                //        var iMax = 0
                //        // $scope.EandMCodingCPTTable.push(test.ListofCPTs[i]);

                //        for (iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                //            if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) <= parseFloat(test.ListofCPTs[i].Order)) {

                //                flag = 1;
                //                $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                //                $scope.EandMCodingCPTTable.join();
                //                //push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iMax + 1, 'Amount': ui.item.label.split('~')[2] });
                //                break;
                //            }

                //        }

                if (test.ListofCPTs.length > 0) {
                    for (var i = 0; i < test.ListofCPTs.length; i++) {
                        for (iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                            iOrder = parseFloat($scope.EandMCodingCPTTable[iMax].Order);
                            if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) > parseFloat(test.ListofCPTs[i].Order)) {

                                flag = 1;
                                $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'RVU': test.ListofCPTs[i].RVU, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                $scope.EandMCodingCPTTable.join();
                                $scope.EandMCodingCPTTable = $scope.EandMCodingCPTTable;
                                break;;
                            }
                            else if (parseFloat($scope.EandMCodingCPTTable[iMax].Order) == parseFloat(test.ListofCPTs[i].Order)) {


                                if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) < parseFloat(test.ListofCPTs[i].RVU)) {
                                    flag = 1;

                                    $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'RVU': test.ListofCPTs[i].RVU, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                    $scope.EandMCodingCPTTable.join();
                                    // $scope.$apply();
                                    break;

                                }
                                else if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) == parseFloat(test.ListofCPTs[i].RVU)) {


                                    if (($scope.EandMCodingCPTTable[iMax].CPTCode) > (test.ListofCPTs[i].CPTCode)) {
                                        flag = 1;
                                        $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'RVU': test.ListofCPTs[i].RVU, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                        $scope.EandMCodingCPTTable.join();
                                        break;
                                    }
                                    else {
                                        continue;
                                    }



                                }
                                //else if (parseFloat($scope.EandMCodingCPTTable[iMax].RVU) > parseFloat(test.ListofCPTs[i].RVU) &&
                                //    iMax + 1 < $scope.EandMCodingCPTTable.length && parseFloat($scope.EandMCodingCPTTable[iMax + 1].Order) != iOrder) {
                                //    flag = 1;
                                //    $scope.EandMCodingCPTTable.splice(iMax+1, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'RVU': test.ListofCPTs[i].RVU, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                //    $scope.EandMCodingCPTTable.join();
                                //    break;

                                //}
                            }
                            else {

                                if (iMax == $scope.EandMCodingCPTTable.length - 1) {
                                    flag = 1;
                                    $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'RVU': test.ListofCPTs[i].RVU, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                                    break;
                                }
                                else {
                                    continue;
                                }

                            }

                        }


                        if (flag == 0) {
                            //$scope.EandMCodingCPTTable.push({ 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });
                            $scope.EandMCodingCPTTable.splice(iMax, 0, { 'CPTCode': test.ListofCPTs[i].CPTCode, 'CPTDesc': test.ListofCPTs[i].CPTDesc, 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': test.ListofCPTs[i].Order, 'RVU': test.ListofCPTs[i].RVU, 'Amount': test.ListofCPTs[i].Amount, 'DiaPointer1': '', 'DiaPointer2': '', 'DiaPointer3': '', 'DiaPointer4': '', 'DiaPointer5': '', 'DiaPointer6': '' });


                        }
                        else {
                            var flagcpt = 0;
                            var flagx = 0;
                            for (var j = 0; j < $scope.EandMCodingCPTTable.length; j++) {
                                flagx = 0;
                                if ($scope.EandMCodingCPTTable[j].CPTCode.indexOf(test.ListofCPTs[i].CPTCode) >= 0) {
                                    flagcpt = 1;
                                    flagx = 1;
                                }
                                //if (flagcpt == 1 && flagx == 0)
                                //    $scope.EandMCodingCPTTable[j].Order = parseFloat(parseFloat($scope.EandMCodingCPTTable[j].Order) + 1);


                            }

                        }






                    }


                }


                BindCPTtable();
                $scope.RefershGrid();
                $scope.EnableSaveButton();
                /*$scope.$apply();*/
                $(top.window.document).find("#tbFavCPTsContainer #dynTr").remove();
                $(top.window.document).find('#divCPTFormView').modal('hide');
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            // alert(error.Message + ".Please Contact Support!");
        });
    }
    $(top.window.document).find("#ok").click(function () {

        $scope.okClick();
        return false;
    });
    $scope.okClick = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var CheckedICDs;
        if ($(top.window) != null && $(top.window) != undefined) {
            CheckedICDs = $.grep($(top.window.document).find('#tbFavICDsContainer tr:not(:first)'), function (row) {

                for (var i = 0; i < $(row).find("input[type=checkbox]").length; i++) {
                    if ($(row).find("input[type=checkbox]")[i].checked)
                        return ($(row));
                }
            });
        }
        var max = 0;
        $('.maxseq').each(function () {
            $this = parseInt($(this).text().replace('B', ''));
            if ($this > max) max = $this;
        });
        max = parseInt(max) + 1;

        var sResultICDs = "";
        for (var i = 0; i < CheckedICDs.length; i++) {
            var cells = $(CheckedICDs[i]).find('td');

            if (cells[0].textContent == "") {
                if (cells[0].childNodes.length != 0) {
                    if (cells[0].childNodes[0].checked) {
                        if (sResultICDs == "")
                            sResultICDs += cells[1].innerText.trim() + "~" + cells[2].innerText.trim() + "~" + 'N' + "~" + max;
                        else {
                            sResultICDs += "|" + cells[1].innerText.trim() + "~" + cells[2].innerText.trim() + "~" + 'N' + "~" + max;
                        }
                        max = parseInt(max) + 1;
                    }
                }
            }
            if (cells[3].textContent == "") {
                if (cells[3].childNodes.length != 0) {
                    if (cells[3].childNodes[0].checked) {
                        {
                            if (sResultICDs == "")
                                sResultICDs += cells[4].innerText.trim() + "~" + cells[5].innerText.trim() + "~" + 'N' + "~" + max;
                            else {
                                sResultICDs += "|" + cells[4].innerText.trim() + "~" + cells[5].innerText.trim() + "~" + 'N' + "~" + max;
                            }
                            max = parseInt(max) + 1;
                        }
                    }
                }
            }

            if (cells[6].textContent == "") {
                if (cells[6].childNodes.length != 0) {
                    if (cells[6].childNodes[0].checked) {
                        {
                            if (sResultICDs == "")
                                sResultICDs += cells[7].innerText.trim() + "~" + cells[8].innerText.trim() + "~" + 'N' + "~" + max;
                            else {
                                sResultICDs += "|" + cells[7].innerText.trim() + "~" + cells[8].innerText.trim() + "~" + 'N' + "~" + max;
                            }
                            max = parseInt(max) + 1;
                        }
                    }
                }
            }


        }
        if (sResultICDs == "") {
            DisplayErrorMessage('220004');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return;
        }
        var sFinalICDs = sResultICDs.replace(/\"/g, "$");
        $http({
            url: "WebServices/EandMCodingService.asmx/GetFormviewICDs",
            dataType: 'json',
            method: 'POST',
            data: '{sFormviewICD: "' + sFinalICDs + '" }',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }

        }).success(function (response, status, headers, config) {
            if (response.d != "") {
                var test = JSON.parse(response.d);

                if (test.ListofICDs.length > 0) {
                    for (var i = 0; i < test.ListofICDs.length; i++) {
                        if (JSON.stringify($scope.EandMCodingICDTable).indexOf(test.ListofICDs[i].ICDCode) == -1)
                            $scope.EandMCodingICDTable.push(test.ListofICDs[i]);
                    }
                }
                $scope.EnableSaveButton();
                $(top.window.document).find("#tbFavICDsContainer #dynTr").remove();
                $(top.window.document).find('#divFormView').modal('hide');
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            // alert(error.Message + ".Please Contact Support!");
        });
    }
    $(top.window.document).find("#CPTcancel").click(function () {

        $scope.CPTcancelClick();
        return false;
    });
    $scope.CPTcancelClick = function () {

        if ($(top.window) != null && $(top.window) != undefined) {
            if ($(top.window.document).find("#CPTok")[0].disabled == false) {
                if (DisplayErrorMessage('220019') == true) {
                    $(top.window.document).find('#divCPTFormView').modal('hide');
                    return false;
                }
                else
                    return;
            }
            else {
                $(top.window.document).find('#divCPTFormView').modal('hide');
                return false;
            }
        }
    }
    $(top.window.document).find("#cancel").click(function () {

        $scope.cancelClick();
        return false;
    });
    $scope.cancelClick = function () {

        if ($(top.window.document).find("#ok")[0].disabled == false) {
            if (DisplayErrorMessage('220019') == true) {
                $(top.window.document).find('#divFormView').modal('hide');
                return false;
            }
            else
                return;
        }
        else {
            $(top.window.document).find('#divFormView').modal('hide');
            return false;
        }
    }



    $(top.window.document).find("#select_Type_Of_Visit").change(function () {
        localStorage.setItem("PhysicianCPT", "");
        $scope.LoadFavouriteCPTs();
        return true;

    });


    var iIndex = -1;
    $scope.CPTDelete = function (index) {
         //Cap - 1573
        //DeleteArray = new Array();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (index != undefined) {
            iIndex = index;
        }
        if (DisplayErrorMessage('530007') == true) {
            var DelList = [];
            var table = $('#tblEandMCodingCPT');
            var iCheck = iIndex + 1;
            iIndex = -1;
            //for (var i = 1; i <= 6; i++) {
            //    if (table.find('tr')[iCheck].children[i].children[0].checked == true) {
            //        CheckboxValue = i;
            //        if (i != 6) {
            //            for (var unCheck = 0; unCheck < $('#tblEandMCodingICD tr td input.chkICD' + CheckboxValue).length ; unCheck++) {
            //                $('#tblEandMCodingICD tr td input.chkICD' + CheckboxValue)[unCheck].checked = false;
            //            }
            //        }
            //    }
            //}


            var sCPTCode = table.find('tr')[iCheck].cells[1].innerText.trim();
            var sCPTDesc = table.find('tr')[iCheck].cells[2].innerText.trim();
            var sUnits = table.find('tr')[iCheck].cells[3].children[0].value;
            var sModi1 = table.find('tr')[iCheck].cells[4].children[0].selectedOptions[0].innerText.trim();
            var sModi2 = table.find('tr')[iCheck].cells[4].children[1].selectedOptions[0].innerText.trim();
            var sModi3 = table.find('tr')[iCheck].cells[4].children[2].selectedOptions[0].innerText.trim();
            var sModi4 = table.find('tr')[iCheck].cells[4].children[3].selectedOptions[0].innerText.trim();
            var sDiaPointer1 = table.find('tr')[iCheck].cells[5].children[0].value;
            var sDiaPointer2 = table.find('tr')[iCheck].cells[5].children[1].value;
            var sDiaPointer3 = table.find('tr')[iCheck].cells[5].children[2].value;
            var sDiaPointer4 = table.find('tr')[iCheck].cells[5].children[3].value;
            var sDiaPointer5 = table.find('tr')[iCheck].cells[5].children[4].value;
            var sDiaPointer6 = table.find('tr')[iCheck].cells[5].children[5].value;
            var sSortOrder = table.find('tr')[iCheck].cells[8].innerText.trim();
            //Cap - 1301
            var RVU = table.find('tr')[iCheck].cells[10].innerText.trim();
            //DeleteArray.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + table.find('tr')[iCheck].children[11].innerText.trim() + "~" + table.find('tr')[iCheck].children[12].innerText.trim() + "~" + table.find('tr')[iCheck].children[13].innerText.trim() + "~" + table.find('tr')[iCheck].children[14].innerText.trim());
            //Cap - 1301
            //DeleteArray.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + table.find('tr')[iCheck].cells[6].innerText.trim() + "~" + table.find('tr')[iCheck].cells[7].innerText.trim() + "~" + table.find('tr')[iCheck].cells[8].innerText.trim() + "~" + table.find('tr')[iCheck].cells[9].innerText.trim() + "~" + sDiaPointer1 + "~" + sDiaPointer2 + "~" + sDiaPointer3 + "~" + sDiaPointer4 + "~" + sDiaPointer5 + "~" + sDiaPointer6 + "~" + sSortOrder);
            DeleteArray.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + table.find('tr')[iCheck].cells[6].innerText.trim() + "~" + table.find('tr')[iCheck].cells[7].innerText.trim() + "~" + table.find('tr')[iCheck].cells[8].innerText.trim() + "~" + table.find('tr')[iCheck].cells[9].innerText.trim() + "~" + sDiaPointer1 + "~" + sDiaPointer2 + "~" + sDiaPointer3 + "~" + sDiaPointer4 + "~" + sDiaPointer5 + "~" + sDiaPointer6 + "~" + sSortOrder + "~" + RVU);

            for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                for (var j = 0; j < DeleteArray.length; j++) {
                    if ($scope.EandMCodingCPTTable[i] != null && $scope.EandMCodingCPTTable[i] != undefined) {
                        //cap - 1301
                        //if (DeleteArray[j].split('~')[10] == $scope.EandMCodingCPTTable[i].Order) {
                        if (DeleteArray[j].split('~')[10] == $scope.EandMCodingCPTTable[i].Order && DeleteArray[j].split('~')[19] == $scope.EandMCodingCPTTable[i].RVU && DeleteArray[j].split('~')[0] == $scope.EandMCodingCPTTable[i].CPTCode) {
                            $scope.EandMCodingCPTTable.splice(i, 1);
                        }
                    }
                }
            }
            $scope.RefershGrid();
            //BindDeleteCPTtable();
            $scope.EnableSaveButton();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return true;
        }
    }
    $scope.ICDDelete = function (index) {
        if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT' && UserRole.toUpperCase() != 'OFFICE MANAGER' && (UserRole.toUpperCase() != 'CODER') && UserRole.toUpperCase() != 'TECHNICIAN') {
            return false;
        }
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (index != undefined)
            iIndex = index;
        if (DisplayErrorMessage('530006') == true) {
            var DelList = [];
            var table = $('#tblEandMCodingICD');
            var iCheck = iIndex + 1;
            iIndex = -1;
            //DeleteArrayICD.push(table.find('tr')[iCheck].children[8].innerText.trim());
            DeleteArrayICD.push(table.find('tr')[iCheck].children[3].innerText.trim());
            for (var i = 0; i < $scope.EandMCodingICDTable.length; i++) {
                for (var j = 0; j < DeleteArrayICD.length; j++) {
                    //CAP-1471
                    if (DeleteArrayICD[j]?.trim() == $scope?.EandMCodingICDTable[i]?.ICDCode?.trim()) {
                        $scope.EandMCodingICDTable.splice(i, 1);

                        //Remove diagnosis pointer mapping from CPT table if ICD deleted
                        if ($('#tblEandMCodingCPT tr').length > 0) {
                            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.trim() == table.find('tr')[iCheck].children[2].innerText.trim()) {
                                        $($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value = "";
                                    }
                                }
                            }
                        }
                        //
                    }
                }
            }
            $scope.EnableSaveButton();
            $scope.RefershGrid();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return true;
        }
    }

    $scope.chkCPT1_Checked = function (index) {

        if (document.getElementById("chkcpt1$" + index.a.Order + "$" + index.a.CPTCode).checked == true) {
            $('#tblEandMCodingCPT tr td input.chkCPT1').each(function () {
                $(this).prop('checked', false);
            });
            $('#tblEandMCodingCPT tr td input.chkCPTRow' + index.a.Order + index.a.CPTCode).each(function () {
                $(this).prop('checked', false);
            });
            document.getElementById("chkcpt1$" + index.a.Order + "$" + index.a.CPTCode).checked = true;
        }
        else {
            document.getElementById("chkcpt1$" + index.a.Order + "$" + index.a.CPTCode).checked = false;
        }
        $scope.EnableSaveButton();
    }
    $scope.chkCPT2_Checked = function (index) {

        if (document.getElementById("chkcpt2$" + index.a.Order + "$" + index.a.CPTCode).checked == true) {
            $('#tblEandMCodingCPT tr td input.chkCPT2').each(function () {
                $(this).prop('checked', false);
            });
            $('#tblEandMCodingCPT tr td input.chkCPTRow' + index.a.Order + index.a.CPTCode).each(function () {
                $(this).prop('checked', false);
            });
            document.getElementById("chkcpt2$" + index.a.Order + "$" + index.a.CPTCode).checked = true;
        }
        else {
            document.getElementById("chkcpt2$" + index.a.Order + "$" + index.a.CPTCode).checked = false;
        }
        $scope.EnableSaveButton();
    }
    $scope.chkCPT3_Checked = function (index) {

        if (document.getElementById("chkcpt3$" + index.a.Order + "$" + index.a.CPTCode).checked == true) {
            $('#tblEandMCodingCPT tr td input.chkCPT3').each(function () {
                $(this).prop('checked', false);
            });
            $('#tblEandMCodingCPT tr td input.chkCPTRow' + index.a.Order + index.a.CPTCode).each(function () {
                $(this).prop('checked', false);
            });
            document.getElementById("chkcpt3$" + index.a.Order + "$" + index.a.CPTCode).checked = true;
        }
        else {
            document.getElementById("chkcpt3$" + index.a.Order + "$" + index.a.CPTCode).checked = false;
        }
        $scope.EnableSaveButton();
    }
    $scope.chkCPT4_Checked = function (index) {

        if (document.getElementById("chkcpt4$" + index.a.Order + "$" + index.a.CPTCode).checked == true) {
            $('#tblEandMCodingCPT tr td input.chkCPT4').each(function () {
                $(this).prop('checked', false);
            });
            $('#tblEandMCodingCPT tr td input.chkCPTRow' + index.a.Order + index.a.CPTCode).each(function () {
                $(this).prop('checked', false);
            });
            document.getElementById("chkcpt4$" + index.a.Order + "$" + index.a.CPTCode).checked = true;
        }
        else {
            document.getElementById("chkcpt4$" + index.a.Order + "$" + index.a.CPTCode).checked = false;
        }
        $scope.EnableSaveButton();
    }
    $scope.chkCPT5_Checked = function (index) {

        if (document.getElementById("chkcpt5$" + index.a.Order + "$" + index.a.CPTCode).checked == true) {
            $('#tblEandMCodingCPT tr td input.chkCPT5').each(function () {
                $(this).prop('checked', false);
            });
            $('#tblEandMCodingCPT tr td input.chkCPTRow' + index.a.Order + index.a.CPTCode).each(function () {
                $(this).prop('checked', false);
            });
            document.getElementById("chkcpt5$" + index.a.Order + "$" + index.a.CPTCode).checked = true;
        }

        else {
            document.getElementById("chkcpt5$" + index.a.Order + "$" + index.a.CPTCode).checked = false;
        }
        $scope.EnableSaveButton();
    }
    $scope.chkCPT6_Checked = function (index) {

        if (document.getElementById("chkcpt6$" + index.a.Order + "$" + index.a.CPTCode).checked == true) {
            $('#tblEandMCodingCPT tr td input.chkCPTRow' + index.a.Order + index.a.CPTCode).each(function () {
                $(this).prop('checked', false);
            });
            document.getElementById("chkcpt6$" + index.a.Order + "$" + index.a.CPTCode).checked = true;
        }
        else {
            document.getElementById("chkcpt6$" + index.a.Order + "$" + index.a.CPTCode).checked = false;
        }
        $scope.EnableSaveButton();
    }

    $scope.EnableSaveButton = function () {

        $('#btnSave').removeAttr("disabled");
        if (UserRole != undefined && UserRole.toUpperCase() != 'SCRIBE') {
            $('#btnSubAllForSuperbill').removeAttr("disabled");
        }
        localStorage.setItem("bSave", "false");
        //CAP-1577
        localStorage.setItem("CCAndEandMAutosave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }

    $scope.SaveEandMCoding = function (index) {
        /*Start For Git Lab Id: 1666*/
        var icdcount = false;
        // CAP  1571;
        var sICDCode='';
            //End
        if (index == "Submit") {
            //Save and submit
            Esuperbillclicked = "Y";
        }
        else {
            //Only save
            Esuperbillclicked = "N";
        }
        /*Stop For Git Lab Id: 1666*/
        var arrlstAssICD = [];
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        // if (index == "Submit") {
        setTimeout(function () { 
        if ($('#tblEandMCodingCPT tr td').length == 0) { //any cpt not mapped
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('530003');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
        else if (($('#tblEandMCodingICD tr td').length == 0 && $('#tblAssEandMCodingICD tr td').length == 0)) { //save service procedure code without icd.

            if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT' && UserRole.toUpperCase() != 'OFFICE MANAGER' && UserRole.toUpperCase() != 'CODER' && UserRole.toUpperCase() != 'TECHNICIAN' && ValEnableScreen.indexOf('EnableScreen')<0) {
              //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                icdcount = true;

                //$($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                //return;


            }
            
            else {
                if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT' && UserRole.toUpperCase() != 'OFFICE MANAGER') {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('530004');
                    bSaveCheck = true;
                    AutoSaveUnsuccessful();

                    //$($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                    return;
                }
            }

        }

        //else if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT') {
        //    for (var j = 1; j < 7; j++) {
        //        if ($('#tblEandMCodingCPT tr td input.chkCPT' + j + ':checked').length > 0 && ($('#tblEandMCodingICD tr td input.chkICD' + j + ':checked').length == 0 && $('#tblAssEandMCodingICD tr td input.chkICD' + j + ':checked').length == 0)) {//mapping not complete i.e 6 markd in CPT but 6 not marked in ICD
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            sessionStorage.setItem("EncCancel", "false");
        //            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //            DisplayErrorMessage('530014', "", "'" + j + "'"); No need for now
        //            bSaveCheck = true;
        //            AutoSaveUnsuccessful();
        //            return;
        //        }
        //    }
        //}
        else if ($('#tblEandMCodingICD tr td').length == 0 && $('#tblAssEandMCodingICD tr td').length == 0) {  //any icd not mapped userRole:coder//BugID:47922 -- ServProc REVAMP
            if (bbtnSubAllForSuperbilltted == true || UserRole.toUpperCase() == 'CODER') {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('530004');
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }
        }
        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                // Units c=validation
                if ($($('#tblEandMCodingCPT tr'))[cptlength].children[3].children[0].value == "") {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('530016');
                    bSaveCheck = true;
                    return;
                }
                else if ($($('#tblEandMCodingCPT tr'))[cptlength].children[3].children[0].value == "0" || $($('#tblEandMCodingCPT tr'))[cptlength].children[3].children[0].value == "00" || $($('#tblEandMCodingCPT tr'))[cptlength].children[3].children[0].value == "000") {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('530017');
                    bSaveCheck = true;
                    return;

                }
            }
        }
        //// }
        var isprimary = false;
        if (UserRole.toUpperCase() == 'CODER' || UserRole.toUpperCase() == 'PHYSICIAN' ) {
            var bIsPrimary = false, bIsAssPrimary = false, bPriICDSelected = false, bPriICDAssSelected = false;

            if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').length > 0) {
                bIsPrimary = true;
                //for (var i = 1; i < 7; i++) {
                //    if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').parent().parent()[0].children[i].children[0].checked == true) {
                //        bPriICDSelected = true;
                //    }
                //}
            }
            var AssICDrows_PrimaryCol = $('#tblAssEandMCodingICD tr td:first-child');
            for (var i = 0; i < AssICDrows_PrimaryCol.length; i++) {
                if (AssICDrows_PrimaryCol[i].firstElementChild.firstElementChild.innerText.trim() == "Pri") {
                    bIsAssPrimary = true;
                    //for (var j = 1; j < 7; j++) {
                    //    if ($(AssICDrows_PrimaryCol[i]).parent()[0].children[j].children[0].checked == true) {
                    //        bPriICDAssSelected = true;
                    //    }
                    //}
                }
            }
            //if (!(bPriICDSelected || bPriICDAssSelected)) {//primary not marked
            //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //    DisplayErrorMessage('530018'); // No need for now
            //    bSaveCheck = true;
            //    AutoSaveUnsuccessful();
            //    return;
            //}

           
            if (!(bIsPrimary || bIsAssPrimary)) {
                if ((UserRole.toUpperCase() == 'CODER')&& ValEnableScreen.indexOf('EnableScreen') >= 0) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('530005');
                    bSaveCheck = true;
                    AutoSaveUnsuccessful();
                    return;
                }
            }
           
            
            else {
                isprimary = true;
            }

        }
        /*
        for (var j = 1; j < 7; j++) {
            if ($('#tbleandmcodingcpt tr td input.chkcpt' + j + ':checked').length > 0 && ($('#tbleandmcodingicd tr td input.chkicd' + j + ':checked').length == 0 && $('#tblasseandmcodingicd tr td input.chkicd' + j + ':checked').length == 0)) {//mapping not complete i.e 6 markd in cpt but 6 not marked in icd
                { sessionstorage.setitem('startloading', 'false'); stoploadfrompatchart(); }
                displayerrormessage('530014', "", "'" + j + "'"); no need for now
                bsavecheck = true;
                autosaveunsuccessful();
                return;
            }
        }
    }

    //For Bug Id 55636
    for (var iCPTChk = 1; iCPTChk < $('#tblEandMCodingCPT tr').length; iCPTChk++) { //Please select the sequence
        if ($($('#tblEandMCodingCPT tr')[iCPTChk]).find("input:checked").length == 0) {
            DisplayErrorMessage('530023', "", "'" + $('#tblEandMCodingCPT tr')[iCPTChk].cells[7].innerText + "'"); No need for now
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
    }
    for (var iRowCPT = 1; iRowCPT < $('#tblEandMCodingCPT tr').length; iRowCPT++) {
        var SelectedRow = $('#tblEandMCodingCPT tr')[iRowCPT];//CPT is not selected but the modifier is added
        if (SelectedRow.children[10].children[0].selectedOptions[0].innerText.trim() != "" || SelectedRow.children[10].children[1].selectedOptions[0].innerText.trim() != ""
            || SelectedRow.children[10].children[2].selectedOptions[0].innerText.trim() != "" || SelectedRow.children[10].children[3].selectedOptions[0].innerText.trim() != "") {
            if ($('#tblEandMCodingCPT tr input.chkCPTRow' + SelectedRow.children[13].innerText.trim() + SelectedRow.children[7].innerText.trim() + ':checked').length == 0) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('530015'); -- No need for now 
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }
        }
        if (SelectedRow.children[9].children[0].value == "") {//units not entered for selected CPT
            if ($('#tblEandMCodingCPT tr input.chkCPTRow' + SelectedRow.children[13].innerText.trim() + SelectedRow.children[7].innerText.trim() + ':checked').length > 0) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('530016'); *
                bSaveCheck = true;
                return;
            }
        }
        if (SelectedRow.children[9].children[0].value == "0" || SelectedRow.children[9].children[0].value == "00" || SelectedRow.children[9].children[0].value == "000") {//units to be between 1 to 999//BugID:49446
            if ($('#tblEandMCodingCPT tr input.chkCPTRow' + SelectedRow.children[13].innerText.trim() + SelectedRow.children[7].innerText.trim() + ':checked').length > 0) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('530017'); *
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }
        }
    }

    for (var i = 1; i < 7; i++) {
        if ($('#tblEandMCodingCPT tr td input.chkCPT' + i + ':checked').length == 0 && ($('#tblEandMCodingICD tr td input.chkICD' + i + ':checked').length > 0 || $('#tblAssEandMCodingICD tr td input.chkICD' + i + ':checked').length > 0)) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sessionStorage.setItem("EncCancel", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            DisplayErrorMessage('530013', "", "'" + i + "'");    NO need for now // incomplete mapping icd mapped to 6, no CPT mapped to 6
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
    }

    for (var jICD = 1; jICD < $('#tblAssEandMCodingICD tr').length; jICD++) {
        if ($($('#tblAssEandMCodingICD tr')[jICD]).find("input:checked").length == 0) {
            DisplayErrorMessage('530022');  NO need for now // Seq not marked for an ICD
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
    }
    for (var jICD = 1; jICD < $('#tblEandMCodingICD tr').length; jICD++) {
        if ($($('#tblEandMCodingICD tr')[jICD]).find("input:checked").length == 0) {
            DisplayErrorMessage('530022');  NO need for now// Seq not marked for an ICD
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
    }*/

        //Validation

        var DiagnosisPointerCPTList = [];
        var DiagnosisPointerICDList = [];

        DiagnosisPointerCPTList = new Array();
        DiagnosisPointerICDList = new Array();

        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim() != "") {
                        if (DiagnosisPointerCPTList.length == 0) {
                            DiagnosisPointerCPTList.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim());
                        }
                        else if (DiagnosisPointerCPTList.indexOf($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim()) == -1) {
                            DiagnosisPointerCPTList.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim());
                        }
                    }
                }
            }
        }

        if (DiagnosisPointerCPTList.length > 0 && $('#tblAssEandMCodingICD tr').length > 0) {
            for (var IcdAsslength = 1; IcdAsslength < $('#tblAssEandMCodingICD tr').length; IcdAsslength++) {
                if ($($('#tblAssEandMCodingICD tr'))[IcdAsslength].children[1].innerText.toUpperCase().trim() != "") {
                    if (DiagnosisPointerICDList.length == 0) {
                        DiagnosisPointerICDList.push($($('#tblAssEandMCodingICD tr'))[IcdAsslength].children[1].innerText.toUpperCase().trim());
                    }
                    else if (DiagnosisPointerICDList.indexOf($($('#tblAssEandMCodingICD tr'))[IcdAsslength].children[1].innerText.toUpperCase().trim()) == -1) {
                        DiagnosisPointerICDList.push($($('#tblAssEandMCodingICD tr'))[IcdAsslength].children[1].innerText.toUpperCase().trim());
                    }
                }
            }
        }
        if (DiagnosisPointerCPTList.length > 0 && $('#tblEandMCodingICD tr').length > 0) {
            for (var IcdEandMlength = 1; IcdEandMlength < $('#tblEandMCodingICD tr').length; IcdEandMlength++) {
                if ($($('#tblEandMCodingICD tr'))[IcdEandMlength].children[2].innerText.toUpperCase().trim() != "") {
                    if (DiagnosisPointerICDList.length == 0) {
                        DiagnosisPointerICDList.push($($('#tblEandMCodingICD tr'))[IcdEandMlength].children[2].innerText.toUpperCase().trim());
                    }
                    else if (DiagnosisPointerICDList.indexOf($($('#tblEandMCodingICD tr'))[IcdEandMlength].children[2].innerText.toUpperCase().trim()) == -1) {
                        DiagnosisPointerICDList.push($($('#tblEandMCodingICD tr'))[IcdEandMlength].children[2].innerText.toUpperCase().trim());
                    }
                }
            }
        }

        if (DiagnosisPointerCPTList.length > 0 && DiagnosisPointerICDList.length > 0) // Diagosis pointers are not mandatory
        {
            var DiagnosisPointerdiff = [];
            jQuery.grep(DiagnosisPointerCPTList, function (el) {
                if (jQuery.inArray(el, DiagnosisPointerICDList) == -1) DiagnosisPointerdiff.push(el);
            });
            if (DiagnosisPointerdiff.length > 0) {
                DisplayErrorMessage('530025', "", "'" + DiagnosisPointerdiff[0] + "'");
                // DisplayErrorMessage('530025', "", "'" + DiagnosisPointerdiff + "'");
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }
        }
        else if (DiagnosisPointerCPTList.length > 0 && DiagnosisPointerICDList.length == 0) {
            DisplayErrorMessage('530025', "", "'" + DiagnosisPointerCPTList[0] + "'");
            // DisplayErrorMessage('530025', "", "'" + DiagnosisPointerdiff + "'");
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }

        //Duplicate Diagnosis Pointer Validation
        var DuplicateDiagnosis = [];
        DuplicateDiagnosis = new Array();

        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {

                DuplicateDiagnosis = new Array();
                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.trim() != "") {
                        if (DuplicateDiagnosis.length == 0) {
                            DuplicateDiagnosis.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase());
                        }
                        else if (DuplicateDiagnosis.indexOf($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim()) > -1) {

                            DisplayErrorMessage('530026', "", "'" + $($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim() + "'-'" + $($('#tblEandMCodingCPT tr'))[cptlength].children[1].innerText.toUpperCase().trim() + "'-'" + parseInt(DiaPointer + 1) + "'");
                            bSaveCheck = true;
                            AutoSaveUnsuccessful();
                            return;
                        }
                        else {
                            DuplicateDiagnosis.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase());
                        }
                    }
                }
            }
        }

        //Cap - 1553
        var DuplicateModifier = [];
        DuplicateModifier = new Array();

        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {

                DuplicateModifier = new Array();
                for (var DiaPointer = 0; DiaPointer < 4; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.trim() != "") {
                        if (DuplicateModifier.length == 0) {
                            DuplicateModifier.push($($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.toUpperCase());
                        }
                        else if (DuplicateModifier.indexOf($($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.toUpperCase().trim()) > -1) {

                            DisplayErrorMessage('530029', "", "'" + $($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.toUpperCase().trim() + "'-'" + $($('#tblEandMCodingCPT tr'))[cptlength].children[1].innerText.toUpperCase().trim() + "'-'" + parseInt(DiaPointer + 1) + "'");
                            bSaveCheck = true;
                            AutoSaveUnsuccessful();
                            return;
                        }
                        else {
                            DuplicateModifier.push($($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.toUpperCase());
                        }
                    }
                }
            }
        }

        var bSerial = false;
        var bSeqValidation = false;
        var iDiaPointerPosition = 0;
        var iDiaPointerPosition = false;

        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                bSerial = false;
                bSeqValidation = false;
                bPosition = false;
                iDiaPointerPosition = 0;
                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim() == "") {
                        if (bSeqValidation != true) {
                            iDiaPointerPosition = parseInt(DiaPointer) + 1;
                        }
                        if (bPosition == false) {
                            bPosition = true;
                            iDiaPointerPosition = parseInt(DiaPointer) + 1;
                        }

                        bSerial = true;
                    }
                    if (bSerial == true && $($('#tblEandMCodingCPT tr'))[cptlength].children[5].children[DiaPointer].value.toUpperCase().trim() != "") {
                        var Cpt = $($('#tblEandMCodingCPT tr'))[cptlength].children[1].innerText.toUpperCase().trim();
                        if (iDiaPointerPosition == 0) {
                            iDiaPointerPosition = parseInt(iDiaPointerPosition) + 1;
                        }
                        DisplayErrorMessage('530027', "", "'" + Cpt + "'-'" + (parseInt(DiaPointer) + 1) + "'-'" + iDiaPointerPosition + "'");
                        bSaveCheck = true;
                        AutoSaveUnsuccessful();
                        return;
                    }
                    else {
                        bSeqValidation = true;
                        // iDiaPointerPosition = parseInt(iDiaPointerPosition) + 1;
                    }
                }
            }
        }

        //Cap - 1553
        var bModifiersSerial = false;
        var bModifiersSeqValidation = false;
        var iModifiersPosition = 0;

        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                bModifiersSerial = false;
                bModifiersSeqValidation = false;
                bPosition = false;
                iModifiersPosition = 0;
                for (var DiaPointer = 0; DiaPointer < 4; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.toUpperCase().trim() == "") {
                        if (bModifiersSeqValidation != true) {
                            iModifiersPosition = parseInt(DiaPointer) + 1;
                        }
                        if (bPosition == false) {
                            bPosition = true;
                            iModifiersPosition = parseInt(DiaPointer) + 1;
                        }

                        bModifiersSerial = true;
                    }
                    if (bModifiersSerial == true && $($('#tblEandMCodingCPT tr'))[cptlength].children[4].children[DiaPointer].value.toUpperCase().trim() != "") {
                        var Cpt = $($('#tblEandMCodingCPT tr'))[cptlength].children[1].innerText.toUpperCase().trim();
                        if (iModifiersPosition == 0) {
                            iModifiersPosition = parseInt(iModifiersPosition) + 1;
                        }
                        DisplayErrorMessage('530028', "", "'" + Cpt + "'-'" + (parseInt(DiaPointer) + 1) + "'-'" + iModifiersPosition + "'");
                        bSaveCheck = true;
                        AutoSaveUnsuccessful();
                        return;
                    }
                    else {
                        bModifiersSeqValidation = true;
                        // iDiaPointerPosition = parseInt(iDiaPointerPosition) + 1;
                    }
                }
            }
        }

       

        var aryCPTList = [];
        var aryICDList = [];
        if ($('#tblEandMCodingCPT tr').length > 0) {
            var chkCPTContainer = $('#tblEandMCodingCPT tr');
            for (var iCheck = 1; iCheck < chkCPTContainer.length; iCheck++) {
                var sCPTCode = chkCPTContainer[iCheck].cells[1].innerText.trim();
                var sCPTDesc = chkCPTContainer[iCheck].cells[2].innerText.trim();
                var sUnits = chkCPTContainer[iCheck].cells[3].children[0].value;
                var sModi1 = chkCPTContainer[iCheck].cells[4].children[0].selectedOptions[0].innerText.trim();
                var sModi2 = chkCPTContainer[iCheck].cells[4].children[1].selectedOptions[0].innerText.trim();
                var sModi3 = chkCPTContainer[iCheck].cells[4].children[2].selectedOptions[0].innerText.trim();
                var sModi4 = chkCPTContainer[iCheck].cells[4].children[3].selectedOptions[0].innerText.trim();
                var sDiaPointer1 = chkCPTContainer[iCheck].cells[5].children[0].value.toUpperCase().trim();
                var sDiaPointer2 = chkCPTContainer[iCheck].cells[5].children[1].value.toUpperCase().trim();
                var sDiaPointer3 = chkCPTContainer[iCheck].cells[5].children[2].value.toUpperCase().trim();
                var sDiaPointer4 = chkCPTContainer[iCheck].cells[5].children[3].value.toUpperCase().trim();
                var sDiaPointer5 = chkCPTContainer[iCheck].cells[5].children[4].value.toUpperCase().trim();
                var sDiaPointer6 = chkCPTContainer[iCheck].cells[5].children[5].value.toUpperCase().trim();
                var sSortOrder = chkCPTContainer[iCheck].cells[8].innerText.trim();


                //aryCPTList.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + $('#tblEandMCodingCPT tr td')[iCheck].className.replace("chkCPT", "")[0] + "~" + chkCPTContainer.parent().parent()[iCheck].cells[11].innerText.trim() + "~" + chkCPTContainer.parent().parent()[iCheck].cells[12].innerText.trim() + "~" + chkCPTContainer.parent().parent()[iCheck].cells[13].innerText.trim() + "~" + chkCPTContainer.parent().parent()[iCheck].cells[14].innerText.trim());
                aryCPTList.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + chkCPTContainer[iCheck].cells[6].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[7].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[8].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[9].innerText.trim() + "~" + sDiaPointer1 + "~" + sDiaPointer2 + "~" + sDiaPointer3 + "~" + sDiaPointer4 + "~" + sDiaPointer5 + "~" + sDiaPointer6 + "~" + sSortOrder);

            }
        }
        for (var jICD = 1; jICD < $('#tblAssEandMCodingICD tr').length; jICD++) {
            var chkICDContainer = $('#tblAssEandMCodingICD tr')[jICD];
            var chkICD1 = ""; var chkICD2 = ""; var chkICD3 = ""; var chkICD4 = ""; var chkICD5 = ""; var chkICD6 = "";
            var IsPrimary = 'N';
            if (chkICDContainer.cells[0].firstElementChild.firstElementChild.innerText.trim() == "Pri") {
                IsPrimary = 'Y';
            }
            //if (chkICDContainer.cells[1].children[0].checked == true) {
            //    chkICD1 = '1';
            //}
            //if (chkICDContainer.cells[2].children[0].checked == true) {
            //    chkICD2 = '2';
            //}
            //if (chkICDContainer.cells[3].children[0].checked == true) {
            //    chkICD3 = '3';
            //}
            //if (chkICDContainer.cells[4].children[0].checked == true) {
            //    chkICD4 = '4';
            //}
            //if (chkICDContainer.cells[5].children[0].checked == true) {
            //    chkICD5 = '5';
            //}
            //if (chkICDContainer.cells[6].children[0].checked == true) {
            //    chkICD6 = '6';
            //}
            var sSequence = chkICDContainer.cells[1].innerText.trim();
            var sICDCode = chkICDContainer.cells[2].innerText.trim();
            var sICDDesc = chkICDContainer.cells[3].innerText.trim();
            var Source = "ASSESSMENT";
            //Jira - #CAP-80
            //if (sessionStorage.getItem("Is_CMG_Ancillary") != null && sessionStorage.getItem("Is_CMG_Ancillary").toUpperCase() == "TRUE") {//BugID:52857
            if (localStorage.getItem("Is_CMG_Ancillary") != null && localStorage.getItem("Is_CMG_Ancillary").toUpperCase() == "TRUE") {
                Source = "ORDERS_ASSESSMENT";
            }
            // if (chkICD1 != "" || chkICD2 != "" || chkICD3 != "" || chkICD4 != "" || chkICD5 != "" || chkICD6 != "")
            aryICDList.push(sICDCode + "~" + sICDDesc + "~" + IsPrimary + "~" + chkICD1 + "~" + chkICD2 + "~" + chkICD3 + "~" + chkICD4 + "~" + chkICD5 + "~" + chkICD6 + "~" + chkICDContainer.cells[4].innerText.trim() + "~" + chkICDContainer.cells[5].innerText.trim() + "~" + Source + "~" + sSequence);
            arrlstAssICD.push(sICDCode);
        }

        for (var jICD = 1; jICD < $('#tblEandMCodingICD tr').length; jICD++) {
            var chkICDContainer = $('#tblEandMCodingICD tr')[jICD];
            var chkICD1 = ""; var chkICD2 = ""; var chkICD3 = ""; var chkICD4 = ""; var chkICD5 = ""; var chkICD6 = "";
            var IsPrimary = 'N';
            if (chkICDContainer.cells[1].children[0].checked == true) {
                IsPrimary = 'Y';
            }
            //if (chkICDContainer.cells[2].children[0].checked == true) {
            //    chkICD1 = '1';
            //}
            //if (chkICDContainer.cells[3].children[0].checked == true) {
            //    chkICD2 = '2';
            //}
            //if (chkICDContainer.cells[4].children[0].checked == true) {
            //    chkICD3 = '3';
            //}
            //if (chkICDContainer.cells[5].children[0].checked == true) {
            //    chkICD4 = '4';
            //}
            //if (chkICDContainer.cells[6].children[0].checked == true) {
            //    chkICD5 = '5';
            //}
            //if (chkICDContainer.cells[7].children[0].checked == true) {
            //    chkICD6 = '6';
            //}3
            var sSequence = chkICDContainer.cells[2].innerText.trim();
            sICDCode = chkICDContainer.cells[3].innerText.trim();
            var sICDDesc = chkICDContainer.cells[4].innerText.trim();
            var dupicd = false;
            var icddup = '';
            // if (chkICD1 != "" || chkICD2 != "" || chkICD3 != "" || chkICD4 != "" || chkICD5 != "" || chkICD6 != "")
            aryICDList.push(sICDCode + "~" + sICDDesc + "~" + IsPrimary + "~" + chkICD1 + "~" + chkICD2 + "~" + chkICD3 + "~" + chkICD4 + "~" + chkICD5 + "~" + chkICD6 + "~" + chkICDContainer.cells[5].innerText.trim() + "~" + chkICDContainer.cells[6].innerText.trim() + "~" + "EMICD" + "~" + sSequence);

            if (arrlstAssICD.indexOf(sICDCode) != -1 && (UserRole.toUpperCase() == "MEDICAL ASSISTANT" || UserRole.toUpperCase() == "OFFICE MANAGER"  || UserRole.toUpperCase() == "CODER" || UserRole.toUpperCase() == 'TECHNICIAN'  || ValEnableScreen.indexOf('EnableScreen') >= 0)) { 
                DisplayErrorMessage('530021', "", "'" + sICDCode + "'");
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }

           else  if (arrlstAssICD.indexOf(sICDCode) != -1) {
               // bSaveCheck = true;
               // AutoSaveUnsuccessful();
                dupicd = true;
                break;
                // CAP-1571

             //   if (!alert('ICD' + sICDCode + 'has already been added under Assessment ICDs.Please remove it ')) {
                    //CAP-980
                  //  localStorage.setItem("bSave", "true");
                   // window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    //$($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                    //$($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[7].click();

                  //  return;


               // }
            }
           


        }

        localStorage.setItem("bSave", "true");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";

        $http({
            url: "WebServices/EandMCodingService.asmx/SaveEandMCodingTable",
            dataType: 'json',
            method: 'POST',
            async: false,
            data: JSON.stringify({ arylstCPT: aryCPTList, arylstICD: aryICDList, sBillingInstruction: $('#txtBillingInstruction').val(), arylstDelCPT: DeleteArray, arylstDelICD: DeleteArrayICD, sESuperbillSubmitted: Esuperbillclicked }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {
            var str = response.d;
            var test = JSON.parse(str);
            if (icdcount) {

                if (!alert('Please select at least one ICD')) {

                    localStorage.setItem("bSave", "true");
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
                    localStorage.setItem("CCAndEandMAutosave", "true");
                    bSaveCheck = false;
                    DisableChartLevelAutoSave();//BugID:52795
                    AutoSaveSuccessful();
                    DeleteArray = [];
                    RefreshNotification('ServiceAndProcedureCode');

                    if (localStorage.getItem("MovetofromEandM") == "False") {

                        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[7].click();
                        return;
                    }
                    else {
                        localStorage.setItem("MovetofromEandM", "False");
                        $scope.EandMCodingCPTTable = test.ProcedureList;
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return;


                    }
                }
             
                //DisplayErrorMessage('530004');

                // bSaveCheck = true;
                //AutoSaveUnsuccessful();

            }
            if (!isprimary && UserRole.toUpperCase() == 'PHYSICIAN') {

                localStorage.setItem("bSave", "true");
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
                localStorage.setItem("CCAndEandMAutosave", "true");
                bSaveCheck = false;
                DisableChartLevelAutoSave();//BugID:52795
                AutoSaveSuccessful();
                DeleteArray = [];
                RefreshNotification('ServiceAndProcedureCode');
                if (!alert('Please mark the Primary ICD') ) {
                    if (localStorage.getItem("MovetofromEandM") == "False") {

                        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[7].click();
                        return;
                    }
                    else {
                        localStorage.setItem("MovetofromEandM", "False");
                        $scope.EandMCodingCPTTable = test.ProcedureList;
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return;


                    }
                }
                //BugID: 49118
                // bSaveCheck = true;
                //AutoSaveUnsuccessful();


            }

            // CAP-1571
            if (dupicd) {

                if (!alert('ICD ' + sICDCode + ' has already been added under Assessment ICDs.Please remove it ')) {
                    $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                    $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[7].click();
                    return;

                }
            }
            //GitLab #3038
            //Jira CAP-998
            //if (test.IsBillableNo == "180045") {
            if (test.IsBillableNo.split('~')[0] == "180045") {

                localStorage.setItem("bSave", "true");
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
                localStorage.setItem("CCAndEandMAutosave", "true");
                bSaveCheck = false;
                DisableChartLevelAutoSave();//BugID:52795
                AutoSaveSuccessful();
                DeleteArray = [];
                RefreshNotification('ServiceAndProcedureCode');
                //Jira CAP-998 - Start
                var alertMessage;
                if (test.IsBillableNo.split('~')[1] == "180057") {
                    alertMessage = 'Please select the ICD Z00.121 or Z00.129 for Gcodes G0438 or G0439';
                }
                else {
                    alertMessage = 'Please select the ICD Z00.00 or Z00.01 for Gcodes G0438 or G0439';
                }
                //Jira CAP-998 - End
                if (UserRole.toUpperCase() == 'PHYSICIAN' && ValEnableScreen.indexOf('EnableScreen') < 0) {

                    //Jira CAP-998
                    //if (!alert('Please select the ICD Z00.00 or Z00.01 for Gcodes G0438 or G0439'))   {
                    if (!alert(alertMessage)) {
                        if (localStorage.getItem("MovetofromEandM") == "False") {

                            $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
                            $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[7].click();
                            return;
                        }
                        else {
                            localStorage.setItem("MovetofromEandM", "False");
                            $scope.EandMCodingCPTTable = test.ProcedureList;
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return;


                        }
                    }
                }
                else {
                    DisplayErrorMessage(test.IsBillableNo.split('~')[1]);
                }
            }
            $scope.EandMCodingCPTTable = test.ProcedureList;
            $scope.EandMCodingICDTable = test.ICDList;
            $scope.AssEandMCodingICDTable = test.AssICDlist;
            if (test.BillingInstruction != "")
                $('#txtBillingInstruction').val(test.BillingInstruction);


            if (test.IsBillableNo == "530024") {
                DisplayErrorMessage('530024');
                sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                return;
            }
            UserRole = test.UserRole.toUpperCase();
            if (UserRole != undefined && UserRole.toUpperCase() == 'SCRIBE') {
                $("#btnSubAllForSuperbill").attr("disabled", "disabled");
                bSubmitted = false;
            }
            //  $scope.orderByFieldCPT = 'Amount';
            // $scope.orderByFieldICD = 'ICDCode';
            // $scope.reverseSort = false;
            EnablePriRbtn = test.EnablePriRbtn;
            $('#btnSave').attr("disabled", "disabled");
            if (bSubmitted == false) { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (Esuperbillclicked != 'Y') {
                DisplayErrorMessage('110019');
            }
            else {
                DisplayErrorMessage('230150');
                $("#btnSubAllForSuperbill").attr("disabled", "disabled");
                bSubmitted = false;
            }
            localStorage.setItem("bSave", "true");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            localStorage.setItem("CCAndEandMAutosave", "true");
            bSaveCheck = false;
            DisableChartLevelAutoSave();//BugID:52795
            AutoSaveSuccessful();
            DeleteArray = []; //BugID: 49118
            RefreshNotification('ServiceAndProcedureCode');
            if (isclosemodal == 1) {
                $(top.window.document).find("#btnClosedEandM").click();
                isclosemodal = 0;
                self.close();
            }
            return;
        })
                .error(function (error, status, headers, config) {

                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (status == 999)
                        window.location = "frmSessionExpired.aspx";
                    //else
                    //    alert(error.Message + ".Please Contact Support!");

                    else {
                        window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                    }
                });
        }, 100);
    }
    /* For Git Lab Id: 1666
    $scope.btnSubAllForSuperbill_Click = function (index) {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#btnSave')[0].disabled == false) {
            bSubmitted = true;
            bSaveCheck = false;

            //if ($('#tblEandMCodingICD tr td input[tdype=checkbox]:checked').length == 0 && $('#tblAssEandMCodingICD tr td input[type=checkbox]:checked').length == 0) {//BugID:48668 -- ServProc REVAMP
            //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //    DisplayErrorMessage('530004');

            //    return;
            //}

            //$scope.SaveEandMCoding();



            if (bSaveCheck) {
                bSubmitted = false;
                return false;
            }



            //var bIsPrimary = false, bIsAssPrimary = false, bPriICDSelected = false, bPriICDAssSelected = false;

            //if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').length > 0) {
            //    bIsPrimary = true;
            //    for (var i = 1; i < 7; i++) {
            //        if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').parent().parent()[0].children[i].children[0].checked == true) {
            //            bPriICDSelected = true;
            //        }
            //    }
            //}
            // var AssICDrows_PrimaryCol = $('#tblAssEandMCodingICD tr td:first-child');
            //for (var i = 0; i < AssICDrows_PrimaryCol.length; i++) {
            //    if (AssICDrows_PrimaryCol[i].firstElementChild.firstElementChild.innerText.trim() == "Pri") {
            //        bIsAssPrimary = true;
            //        for (var j = 1; j < 7; j++) {
            //            if ($(AssICDrows_PrimaryCol[i]).parent()[0].children[j].children[0].checked == true) {
            //                bPriICDAssSelected = true;
            //            }
            //        }
            //    }
            //}
            //if (!(bPriICDSelected || bPriICDAssSelected)) {//primary not marked
            //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //    DisplayErrorMessage('530018');
            //    bSaveCheck = true;
            //    AutoSaveUnsuccessful();
            //    return;
            //}


            //if (!(bIsPrimary || bIsAssPrimary)) {
            //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //    DisplayErrorMessage('530005');
            //    bSaveCheck = true;
            //    AutoSaveUnsuccessful();
            //    return;
            //}


        }
        //else
        //{
        //    if ($('#tblEandMCodingICD tr td input[type=checkbox]:checked').length == 0 && $('#tblAssEandMCodingICD tr td input[type=checkbox]:checked').length == 0) {//BugID:48668 -- ServProc REVAMP
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        DisplayErrorMessage('530004');

        //        return;
        //    }

        //    var bIsPrimary = false, bIsAssPrimary = false, bPriICDSelected = false, bPriICDAssSelected = false;

        //    if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').length > 0) {
        //        bIsPrimary = true;
        //        for (var i = 1; i < 7; i++) {
        //            if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').parent().parent()[0].children[i].children[0].checked == true) {
        //                bPriICDSelected = true;
        //            }
        //        }
        //    }
        //    var AssICDrows_PrimaryCol = $('#tblAssEandMCodingICD tr td:first-child');
        //    for (var i = 0; i < AssICDrows_PrimaryCol.length; i++) {
        //        if (AssICDrows_PrimaryCol[i].firstElementChild.firstElementChild.innerText.trim() == "Pri") {
        //            bIsAssPrimary = true;
        //            for (var j = 1; j < 7; j++) {
        //                if ($(AssICDrows_PrimaryCol[i]).parent()[0].children[j].children[0].checked == true) {
        //                    bPriICDAssSelected = true;
        //                }
        //            }
        //        }
        //    }
        //    if (!(bPriICDSelected || bPriICDAssSelected)) {//primary not marked
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        DisplayErrorMessage('530018'); //No need
        //        bSaveCheck = true;
        //        AutoSaveUnsuccessful();
        //        return;
        //    }


        //    if (!(bIsPrimary || bIsAssPrimary)) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        DisplayErrorMessage('530005');
        //        bSaveCheck = true;
        //        AutoSaveUnsuccessful();
        //        return;
        //    }
        //}

        $http({
            url: "WebServices/EandMCodingService.asmx/ESuperbillSubmitted",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({ sESuperbillSubmitted: 'Y' }),
            async: false,
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {
            var str = JSON.parse(response.d);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (str == "530024") {
                DisplayErrorMessage('530024');
            }
            else {
                DisplayErrorMessage('230150');
            }
            $("#btnSubAllForSuperbill").attr("disabled", "disabled");
            bSubmitted = false;
            //BillingWFProcess = "BATCHING_READY";
            return;

        })
        .error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            //alert(error.Message + ".Please Contact Support!");
        });
    }
    */
    function autoSave(val) {
        if (document.getElementById("btnSave").disabled == false) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(dvdialog).dialog({
                modal: true,
                title: "Capella EHR",
                position: {
                    my: 'center',
                    at: 'center + 100px'
                },
                buttons: {
                    "Yes": function () {
                        $(dvdialog).dialog("close");
                        if (val == 0) {
                            //isload = 1;
                            $("#btnSave").click();

                        }
                        else
                            if (val == 1) {
                                isclosemodal = 1;
                                $("#btnSave").click();
                            }
                    },
                    "No": function () {
                        $(dvdialog).dialog("close");
                        //setSaveDisabled();
                        //if (val == 0) {
                        //    loadprblmlist();
                        //}
                        //if (val == 1) {
                        $(top.window.document).find("#btnClosedEandM").click();
                        self.close();
                        //}
                    },
                    "Cancel": function () {
                        $(dvdialog).dialog("close");
                        self.close();
                    }
                }
            });
        }
        else
            loadprblmlist();

    }
    $scope.CloseModal = function () {
        if (document.getElementById("btnSave").disabled == false) {
            event.preventDefault();
            autoSave(1);
        }
        else {
            $(top.window.document).find("#btnClosedEandM").click();
            self.close();
        }
        return 1;
    }
    //$scope.btnClose_Click = function (index) {
    //    if (document.getElementById("btnSave").value != null && document.getElementById("btnSave").value != undefined) {
    //        if (document.getElementById("btnSave").disabled == false) {
    //            if (document.getElementById("hdnMessageType").value == "") {
    //                DisplayErrorMessage('1100000');   //('380049'); //('7430006');
    //                return false;
    //            }
    //            else if (document.getElementById("hdnMessageType").value == "Yes") {
    //                btnSave_Clicked(sender, args);
    //                __doPostBack('btnSave', "true");
    //                self.close();
    //            }
    //            else if (document.getElementById("hdnMessageType").value == "No") {
    //                document.getElementById("hdnMessageType").value = ""
    //                self.close();
    //            }
    //            else if (document.getElementById("hdnMessageType").value == "Cancel") {
    //                document.getElementById("hdnMessageType").value = "";
    //                EnableSave();
    //                args.set_cancel(true);
    //            }
    //        }
    //        else {
    //            self.close();
    //        }
    //    }
    //    else {
    //        self.close();
    //    }
    //}


    $scope.RefershGrid = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $http({
            url: "WebServices/EandMCodingService.asmx/RefershGrid",
            dataType: 'json',
            method: 'POST',
            data: '{}',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response, status, headers, config) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            // alert(error.Message + ".Please Contact Support!");
        });
    }
    $scope.txtUnits_KeyPress = function (keyCode) {

        if (event.keyCode < 48 || event.keyCode > 57) {
            event.returnValue = false;
            $scope.EnableSaveButton();
        }
    }
    $scope.validation = function (keyCode) {
        var Units = document.getElementById("txtUnits").value;
        $scope.EnableSaveButton();
    }
    $scope.txtDiaPointer_KeyPress = function (keyCode) {
        var keyCode1 = event.keyCode || event.which;
        var regex = /^[A-Za-z0-9]+$/;
        var isValid = regex.test(String.fromCharCode(keyCode1));
        if (!isValid) {
            event.returnValue = false;
            $scope.EnableSaveButton();
        }
    }



});

myapp.filter('orderObjectBy', function () {
    return function (items, field, reverse) {
        var filtered = [];
        angular.forEach(items, function (item) {
            filtered.push(item);
        });
        filtered.sort(function (a, b) {
            return (parseInt(a[field]) > parseInt(b[field]) ? 1 : -1);
        });
        if (reverse) filtered.reverse();
        return filtered;
    };
});





myapp.filter('orderSequenceBy', function () {
    return function (items, field, reverse) {
        var filtered = [];
        angular.forEach(items, function (item) {
            filtered.push(item);
        });
        filtered.sort(function (a, b) {
            return (parseInt(a[field].toUpperCase().replace("A", "").replace("B", "")) > parseInt(b[field].toUpperCase().replace("A", "").replace("B", "")) ? 1 : -1);
        });
        if (reverse) filtered.reverse();
        return filtered;
    };
});


window.addEventListener("contextmenu",
    function (e) {
        e.stopPropagation()
    }, true);
function followupKeypress() {
    event.preventDefault();

}
