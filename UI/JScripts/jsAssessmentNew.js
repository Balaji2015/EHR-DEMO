//If any changes are made in this file increase value of version in the query string in all files referencing this javascript file
//Only for AngularJS scripts
var arrICD10Codes = [];



function OpenPlanScreen() {

    $('#divPlanBodyProcessFrame')[0].contentDocument.location.href = "frmAddorUpdatePlan.aspx?Check=true";

    $('#divPlanScreen').modal({ backdrop: 'static', keyboard: false }, 'show').find('#btnClose').click(function () { alert("Hi"); });
}

function ChangeEnable(item) {
    item.parentNode.parentNode.children[21].innerText = "Y";
    if (item.name == "Primary") {
        if ($('input[data-primaryonload="true"]') != undefined && $('input[data-primaryonload="true"]').length > 0)
            $('input[data-primaryonload="true"]')[0].parentNode.parentNode.children[21].innerText = "Y";
    }

    $('#btnSave')[0].disabled = false;
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}

function chkStatusChange(cIDs) {

    //Jira - #CAP-80
    //if (sessionStorage.getItem("Projname") != undefined && sessionStorage.getItem("Projname").toUpperCase() == "WISH") {
    if (localStorage.getItem("Projname") != undefined && localStorage.getItem("Projname").toUpperCase() == "WISH") {
        cIDs.parentNode.parentNode.children[21].innerText = "Y";
        $('#btnSave')[0].disabled = false;
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    else {
        for (var i = 0; i < $(cIDs).parent().next().next().next()[0].children[0].length - 1; i++) {
            if ($(cIDs).parent().next().next().next()[0].children[0][i].selected && $(cIDs).parent().next().next().next()[0].children[0][i].value == "Suspected") { DisplayErrorMessage('220014'); cIDs.checked = false; }
            else {
                cIDs.parentNode.parentNode.children[21].innerText = "Y";
                $('#btnSave')[0].disabled = false;
                localStorage.setItem("bSave", "false");
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            }
        }
    }


}

function cboStatusChange(cIDs) {
    //Jira - #CAP-80
    //if (sessionStorage.getItem("Projname") != undefined && sessionStorage.getItem("Projname").toUpperCase() == "WISH") {
    if (localStorage.getItem("Projname") != undefined && localStorage.getItem("Projname").toUpperCase() == "WISH") {
        $('#btnSave')[0].disabled = false;
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    else {
        cIDs.parentNode.parentNode.children[21].innerText = "Y";
        if ($(cIDs).parent().prev().prev().prev()[0].children[0].checked && cIDs.value == "Suspected") { DisplayErrorMessage('220014'); cIDs.value = ""; }
        else {
            $('#btnSave')[0].disabled = false;
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
    }

}
function ClosePlanScreen() {

    $('#divPlanBodyProcessFrame')[0].contentDocument.location.href = "about:blank";
    $('#divPlanScreen').modal('hide');

}

function chkstatus_change(checkbox) {
    if (checkbox.checked) {
        for (var i = 0; i < $('select').length; i++) {

            if ($('select')[i].selectedIndex <= 1) {
                $('select')[i].selectedIndex = 1;
                $('select')[i].parentNode.parentNode.children[21].innerText = "Y";
            }


        }

    }

    else {
        for (var i = 0; i < $('select').length; i++) {

            if ($('select')[i].selectedIndex <= 1) {
                $('select')[i].selectedIndex = 0;
                $('select')[i].parentNode.parentNode.children[21].innerText = "Y";
            }


        }
    }



    $('#btnSave')[0].disabled = false;

    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;

}
var iRightClickMenuCheck = false;
var bTimeCheck = false;
function RightClickMenu() {
    if (!iRightClickMenuCheck) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        setTimeout(function () {

            if (!bTimeCheck) {
                bTimeCheck = true;
            }


            iRightClickMenuCheck = true;

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        }, 750);
    }

}

function OpenPopup(keyword) {
    var focused = keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}


function SaveEnable(item) {
    if (item != undefined)
        item.parentNode.parentNode.children[21].innerText = "Y";
    $('#btnSave')[0].disabled = false;
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}


var iDisable = "";
var hdnFieldName = null;

function callweb(icon, List, id) {

    if (iDisable == 'Disable') {
        return false;
    }

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
                var pos = "";
                if (id == "txtGeneralNotes")
                    pos = $('#' + targetControlValue).position();
                else
                    pos = $('#' + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:45541
                innerdiv += "<li class='alinkstyle'style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                for (var i = 0; i < values.length - 1; i++) {

                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black' onclick=\"fun('" + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45785 for special characters functionality 
                }

                var listlength = innerdiv.length;
                for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                    if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                        document.getElementsByTagName("div")[i].hidden = true;
                    }
                }
                var iTop = parseInt("0");
                if (id == "txtGeneralNotes")
                    iTop = pos.top + 101;
                else
                    iTop = pos.top + 45;
                $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                    .css({
                        top: iTop,
                        left: pos.left,
                        width: $('#' + targetControlValue)[0].style.width,
                        height: '150px',
                        overflow: 'scroll',
                        position: 'absolute',
                        background: 'white',
                        bottom: '0',
                        floating: 'top',
                        border: '1px solid #8e8e8e',
                        background: '#FFF',
                        fontFamily: 'Segoe UI",Arial,sans-serif',
                        fontSize: '12px',
                        'z-index': '150',
                        overflowX: 'auto'

                    })
                    .insertAfter($('#' + targetControlValue));



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
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");
    }
    var hdnFieldName = null;
    if (hdnFieldName != null && hdnFieldName != icon) {

        $(hdnFieldName).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFieldName = icon;
}


function fun(agrulist) {

    $('#btnSave')[0].disabled = false;


    var sugglistval = $("#" + agrulist).val().trim();
    var value = event.currentTarget.textContent;

    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split(",")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value) {
                flag++;
            }
        }
        if (flag == 0) {
            $("#" + agrulist).val(sugglistval + "," + value);
        }
    }
    else {
        $("#" + agrulist).val(value);
    }
}


var arrphysician;
var DeleteArray = [];
var iCountBack = 0;
var QuestionnaireArray = [];
var ICDCheckNode = "";
var sMutilSelectICD = "";
var sMulti = 0;
var sSelectedICD = "";
var sMainlevelQuestionnaire = "";
var iFavCheck = 0;
var sMainlevelCheck = false;
var bSublevelCheck = false;
var sSublevelQuestionnaire = "";
var sLabelText = "";
var sSublevelLabelText = "";
var QuetionnaireInnerArray = [];
var sAssociatedSelectedICD = "";
var bICD10Conversioncheck = false;
var bcolorcoding = false;
var iCheckSubLevelQuestionnaire = -1;
var sSingleICD = "";
var bProblmCheck = false;

var arrCPTs = [];
var bBool = false;
var bcheck = true;
var intCPTLength = -1;

var singlecolumn = {
    "ColumnName": [
        {
            "Select": "Select(One Diagnosis)",
            "ICD": "ICD 10",
            "ICDDesc": "ICD 10 Description",
            "LeafNode": "LeafNode",
            "CheckLeafNode": "CheckLeafNode",
            "MutullayExclusive": "Mutullay Exclusive"

        }
    ]
}
var Multiplecolumn = {
    "ColumnName": [
        {
            "Select": "Select(One or more Diagnosis)",
            "ICD": "ICD 10",
            "ICDDesc": "ICD 10 Description",
            "LeafNode": "LeafNode",
            "CheckLeafNode": "CheckLeafNode",
            "MutullayExclusive": "Mutullay Exclusive"
        }
    ]
}
var test = "";
var bSetICD9Code = false;
var statusDefaultLst = "";
var AssoICDQuestionnaire;
var bAssoICDOpen = false;
var myapp = angular.module('Assessmentapp', []);
//CAP-450: Error Handler for Angular Js
myapp.config(function ($provide) {
    $provide.decorator('$exceptionHandler', function ($delegate) {
        return function (exception, cause) {
            HandlerAngularjsError(exception);
        };
    });
});
myapp.controller('assessmentCtrl', function ($scope, $http) {
    //CAP-1656, CAP-1660
    $(top.window.document).find("#btnMinimizeViewResultICD").css({ "display": "block" }); //BugID:44399 
    $(top.window.document).find("#divFormView").css({ "position": "absolute" });
    //Jira - #CAP-80
    //if (sessionStorage.getItem("Projname") != undefined && sessionStorage.getItem("Projname").toUpperCase() == "WISH") {
    if (localStorage.getItem("Projname") != undefined && localStorage.getItem("Projname").toUpperCase() == "WISH") {
        $("#lblStatus").css({ "display": "none" });
    }
    $http({
        url: "frmAssessmentNew.aspx/LoadAssessmentTable",
        dataType: 'json',
        method: 'POST',
        data: '{strAssessment: "Load" }',
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "X-Requested-With": "XMLHttpRequest"
        }
    }).success(function (response, status, headers, config) {
        var str = response.d;
        var test12 = JSON.parse(str);

        $scope.AssessmentTable = test12.AssessmentList;

        $scope.PotentailDiagnosisList = test12.PotentialDiagnosisList;

        $scope.Statuses = test12.StatusList;

        statusDefaultLst = test12.StatusDefaultList;//BugID:49118

        $scope.orderByField = ['-IsPrimary', 'ICDCode'];

        $scope.reverseSort = false;

        $scope.FromProblemList = test12.FromProblemList;

        test = test12.ICD10Tool;

        $scope.EnableScreen = test12.DisableScreen;
        iDisable = $scope.EnableScreen;
        if (iDisable == "Disable") {
            $scope.ButtonBG = "#6D7777";
            $("a[class*='fa fa-plus'").addClass('pbDropdownBackgrounddisable');
        }
        else {
            $scope.ButtonBG = "#6DABF7";
            $("a[class*='fa fa-plus'").addClass('pbDropdownBackground');
        }
        if (test12.DisableScreen != "Disable") {
            $scope.SaveEnableDisable(test12.SaveEnableDisable);
            if ($scope.PotentailDiagnosisList.length > 0) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#divPotential").modal({ backdrop: 'static', keyboard: false }, 'show');
                //Jira - #CAP-80
                //$(top.window.document).find("#PotentialFrame")[0].contentDocument.location.href = "htmlPotentialDiagnosis.html?version=" + sessionStorage.getItem("ScriptVersion") + "&Screen=ASSESSMENT";
                $(top.window.document).find("#PotentialFrame")[0].contentDocument.location.href = "htmlPotentialDiagnosis.html?version=" + localStorage.getItem("ScriptVersion") + "&Screen=ASSESSMENT";
            }
            //Cap - 1566
            //if (test12.btnPotentialDiagnosis == true) {
            //    $('#btnPotentailDiagnosis')[0].disabled = false;
            //}
            //else {
            //    $('#btnPotentailDiagnosis')[0].disabled = true;
            //}
        }
        $scope.ColorCoding();
        $("textarea").bind("keydown", function (e) {

            insertTab(this, event);//BugID:45541
        });
        if (test12.ResultGeneralNotes != "")
            $('#txtGeneralNotes').val(test12.ResultGeneralNotes);
        if (iDisable == "Disable") {
            $scope.ButtonBG = "#6D7777";
            $("a[class*='fa fa-plus'").addClass('pbDropdownBackgrounddisable');
        }
        else {
            $scope.ButtonBG = "#6DABF7";
            $("a[class*='fa fa-plus'").addClass('pbDropdownBackground');
        }
        if (test12.SaveEnableXmlMismatch == true) {
            $('#btnSave')[0].disabled = false;
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    })
        .error(function (error, status, headers, config) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else {
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            }

            // alert(error.Message + ".Please Contact Support!");
        });

    var ResultAssesssment = [];
    var RefreshCheck = false;
    $scope.ColorCoding = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        var sICDCodes = $scope.AssessmentTable.map(function (item) { return item.ICDCode; });
        bcolorcoding = false;

        $http({
            url: "frmAssessmentNew.aspx/ColorCodingforMutuallyExclusive",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({ ICDCodes: sICDCodes }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response, status, headers, config) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            var test = JSON.parse(response.d);

            var table = $('#tblCurrICDs');
            table.find('tr').each(function (rowIndex, r) {
                if (rowIndex != 0) {
                    var cols = [];
                    var iColor = "";
                    $(this).find('td').each(function (colIndex, c) {


                        if (colIndex == "4") {
                            if (response.d.indexOf(c.textContent.replace(" ", "")) > -1) {
                                for (var k = 0; k < test.ColorCoding.length; k++) {
                                    if (test.ColorCoding[k].ICDCode.replace(" ", "") == c.textContent.replace(" ", "")) {
                                        c.style.color = test.ColorCoding[k].Color;
                                        iColor = c.style.color;
                                        bcolorcoding = true;
                                    }

                                }

                            }
                            else
                                c.style.color = "Black";
                        }
                        else if (colIndex == "5" || colIndex == "6" || colIndex == "7") {
                            c.style.color = iColor;
                        }


                    });

                }
            });
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else if (error.Message != null)
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            // alert(error.Message + ".Please Contact Support!");



        });

    }

    var iLoadCount = 0;

    $scope.OpenICD10Conversion = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        if (!bICD10Conversioncheck) {
            if (test.length == 0) {
                DisplayErrorMessage('220021');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return;
            }

            $('#divtblContainer').empty();
            for (var j = 0; j < test.length; j++) {

                var details = document.createElement('DETAILS');
                var summary = document.createElement('SUMMARY');
                summary.innerHTML = test[j].split('^')[0].split('~')[0] + "-" + test[j].split('^')[0].split('~')[1];
                $(details).append(summary);
                var table = '<table class="table table-bordered table-condensed" style="margin-bottom:0px;">';
                var ICD_10lst = test[j].split('^')[1].split('|');
                for (var i = 0; i < ICD_10lst.length - 1; i++) {
                    ICD_10lst[i] = ICD_10lst[i].replace('-', '').replace(',', '');
                    table += '<tr><td class="chkbx"><input class="' + test[j].split('^')[0].split('~')[0].replace('.', '_') + '" type="checkbox" /></td><td class="ValueICD">' + ICD_10lst[i].split('-')[0] + '</td><td class="DescpICD">' + ICD_10lst[i].split('-')[2] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[2] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[4] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[6] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[5] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[7] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[0] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[1] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[8] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[9] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[11] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[12] + '</td></tr>';
                }
                table += '</table>';
                $("#divtblContainer").append(details);
                $(details).attr('open', '');
                $(details).append(table);

            }

            $('#divdetails').modal({ backdrop: 'static', keyboard: false }, 'show');

            bICD10Conversioncheck = false;

            bICD9CodeCheck = false;
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        }
        else {
            $('#divdetails').modal({ backdrop: 'static', keyboard: false }, 'show');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    }

    var sAssociatedSelectedICD = "";
    $scope.LoadICD10toAssessmentTable = function () {
        if (bICD9CodeCheck) {
            bICD9CodeCheck = false;
            sSelectedICD = "";
            var Selected = $('#divtblContainer').find("input:checked");
            var ICD9SelectedICDS = "";
            for (var u = 0; u < Selected.length; u++) {
                if (ICD9SelectedICDS == "")
                    ICD9SelectedICDS = $(Selected[u]).parent().next()[0].innerText + "~" + $(Selected[u]).parent().next().next()[0].innerText;
                else
                    ICD9SelectedICDS += "|" + $(Selected[u]).parent().next()[0].innerText + "~" + $(Selected[u]).parent().next().next()[0].innerText;


            }
            $http({
                url: "frmAssessmentNew.aspx/GetFormviewICDs",
                dataType: 'json',
                method: 'POST',
                data: '{sFormviewICD: "' + $(Selected[0]).parent().next()[0].innerText + "~" + $(Selected[0]).parent().next().next()[0].innerText + '" }',
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                }

            }).success(function (response, status, headers, config) {
                if (response.d != "") {
                    var test = JSON.parse(response.d);

                    if (test.NoQuestionnaire.length > 0) {
                        for (var i = 0; i < test.NoQuestionnaire.length; i++) {
                            if (JSON.stringify($scope.AssessmentTable).indexOf(test.NoQuestionnaire[i].ICDCode) == -1) {
                                $scope.AssessmentTable.push(test.NoQuestionnaire[i]);
                                $("textarea").unbind();
                                iRightClickMenuCheck = false;
                                $scope.SaveEnableDisable(false);
                            }
                        }
                        $scope.ColorCoding();
                    }

                    sAssociatedSelectedICD = $(Selected[0]).parent().next()[0].innerText + "~" + $(Selected[0]).parent().next().next()[0].innerText + "|" + "Y";

                    $('#divdetails').modal('hide');

                    if (test.Questionnaire != undefined) {
                        if (test.Questionnaire.length > 0) {
                            $scope.Questionnaire = test.Questionnaire;

                            for (var i = 0; i < test.MainQuestionnaire.length; i++) {
                                if (sMainlevelQuestionnaire == "")
                                    sMainlevelQuestionnaire = test.MainQuestionnaire[i].split('~')[0] + "-" + test.MainQuestionnaire[i].split('-')[1];
                                else
                                    sMainlevelQuestionnaire = "~" + test.MainQuestionnaire[i].split('~')[0] + "-" + test.MainQuestionnaire[i].split('-')[1];

                                if (sLabelText == "")
                                    sLabelText = test.MainQuestionnaire[i].split('-')[0].replace("~", "-");
                                else
                                    sLabelText += "~" + test.MainQuestionnaire[i].split('-')[0].replace("~", "-");
                            }

                            QuestionnaireArray.push(test.Questionnaire);// = response.d;

                            if (test.Questionnaire[0].MutuallyExclusive == "Y")
                                $scope.ColumnName = singlecolumn.ColumnName;
                            else
                                $scope.ColumnName = Multiplecolumn.ColumnName;

                            var labelText = $("#lblQuestionnaire").text();

                            $('#lblQuestionnaire').html(labelText + "" + test.Questionnaire[0].ICDCodeDesc + "--->");
                            $('#divMultipleICD9ModalQuestionnaire').modal('show');
                        }
                    }

                }

            }).error(function (error, status, headers, config) {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else
                    window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                // alert(error.Message + ".Please Contact Support!");
            });
        }
        else {
            var Selected = $('#divtblContainer').find("input:checked");
            if ($(Selected[0]).parent().next().next().next()[0].innerText == "Potential Diagnosis") {
                var sAllICD10List = [];
                var AllICD10List = $('#divtblContainer').find("input[type=checkbox]");
                for (var iAll = 0; iAll < AllICD10List.length; iAll++) {
                    sAllICD10List.push(AllICD10List[iAll].className);
                }
                var distinctAll10List = removeDuplicates(sAllICD10List);

                var sSelected10List = [];

                for (var iSelect = 0; iSelect < Selected.length; iSelect++) {
                    sSelected10List.push(Selected[iSelect].className);
                }
                var distinctSel10List = removeDuplicates(sSelected10List);

                if (distinctAll10List.length != distinctSel10List.length) {
                    DisplayErrorMessage('1011074');
                    return;
                }
            }
            var ICD_Details = $('#divtblContainer').find("details");

            var sSelectedICD = "";
            var ProblemListID = parseInt("0");
            var assessmentID = parseInt("0");
            var cICD9Code = "";
            var cICD9Desc = "";
            var SelectedObj = [];
            var PrimaryCheck = "";
            for (var i = 0; i < Selected.length; i++) {
                if (JSON.stringify($scope.AssessmentTable).indexOf($(Selected[i]).parent().next()[0].innerText) == -1) {

                    if (ProblemListID != parseInt($(Selected[i]).parent().next().next().next().next()[0].innerText)) {
                        ProblemListID = $(Selected[i]).parent().next().next().next().next()[0].innerText;
                        cICD9Code = $(Selected[i]).parent().next().next().next().next().next().next().next().next()[0].innerText;
                        cICD9Desc = $(Selected[i]).parent().next().next().next().next().next().next().next().next().next()[0].innerText;
                    }
                    else {
                        ProblemListID = 0;
                        cICD9Code = "";
                        cICD9Desc = "";
                    }

                    if (assessmentID != parseInt($(Selected[i]).parent().next().next().next().next().next()[0].innerText)) {
                        assessmentID = $(Selected[i]).parent().next().next().next().next().next()[0].innerText;
                        cICD9Code = $(Selected[i]).parent().next().next().next().next().next().next().next().next()[0].innerText;
                        cICD9Desc = $(Selected[i]).parent().next().next().next().next().next().next().next().next().next()[0].innerText;
                        PrimaryCheck = $(Selected[i]).parent().next().next().next().next().next().next().next().next().next().next().next()[0].innerText;
                    }
                    else {
                        assessmentID = 0;
                        PrimaryCheck = "";

                    }
                    var Ass_status = "";
                    if (statusDefaultLst.ASSESSMENT != undefined) {
                        Ass_status = statusDefaultLst.ASSESSMENT;
                    }
                    $scope.AssessmentTable.push({
                        'ICDCode': $(Selected[i]).parent().next()[0].innerText,
                        'ICDDescription': $(Selected[i]).parent().next().next()[0].innerText,
                        'AssessmentID': assessmentID,
                        'iVersion': $(Selected[i]).parent().next().next().next().next().next().next().next()[0].innerText,
                        'iProblemListVersion': $(Selected[i]).parent().next().next().next().next().next().next()[0].innerText,
                        'ProblemListID': ProblemListID,
                        'Notes': $(Selected[i]).parent().next().next().next()[0].innerText,
                        'ICD9Code': cICD9Code,
                        'ICD9Desc': cICD9Desc,
                        'CheckBoxCheck': $(Selected[i]).parent().next().next().next().next().next().next().next().next().next().next()[0].innerText,
                        'IsPrimary': PrimaryCheck,
                        'Created_by': $(Selected[i]).parent().parent()[0].cells[12].innerText,
                        'Created_date': $(Selected[i]).parent().parent()[0].cells[13].innerText,
                        'Updated': "Y",
                        'StatusSelected': Ass_status,
                        'Orig_Status': Ass_status
                    });
                    $("textarea").unbind();
                    iRightClickMenuCheck = false;

                    ProblemListID = $(Selected[i]).parent().next().next().next().next()[0].innerText;
                    assessmentID = $(Selected[i]).parent().next().next().next().next().next()[0].innerText;

                    cICD9Code = "";
                    cICD9Desc = "";

                    $scope.SaveEnableDisable(false);

                    for (var k = test.length - 1; k >= 0; k--) {

                        if (test[k].split('~')[0] == $(Selected[i]).parent().next().next().next().next().next().next().next().next()[0].innerText) {
                            test.splice(k, 1);
                            bICD10Conversioncheck = false;
                        }
                    }
                }
            }

            $scope.ColorCoding();
            $('#divdetails').modal('hide');
        }
    }
    $scope.ClearAllLocalVariables = function () {
        sAssociatedSelectedICD = "";
        bProblmCheck = false;
        iCountBack = 0;
        QuestionnaireArray = [];
        ICDCheckNode = "";
        sMutilSelectICD = "";
        sMulti = 0;
        sSelectedICD = "";
        sMainlevelQuestionnaire = "";
        sMainlevelCheck = false;
        bSublevelCheck = false;
        sSublevelQuestionnaire = "";
        sLabelText = "";
        sSublevelLabelText = "";
        iSelectMoreDiagnosis = -1;
        $('#lblQuestionnaire').html("Please select the Diagnosis related to:");
    }

    $scope.LoadFavouriteICDS = function () {
        var arrListICDs = [];
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $http({
            url: "frmAssessmentNew.aspx/GetFavouriteICDS",
            dataType: 'json',
            method: 'POST',
            data: '',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }

        }).success(function (data) {
            if (data.d != "") {

                var jsonData = $.parseJSON(data.d);
                for (var i = 0; i < jsonData.length; i++) {// Previously it was jsonData - 1
                    var arricd = jsonData[i];
                    arrListICDs.push(arricd.ICD10Code + "~" + arricd.ICD10Desc + "~" + arricd.Category)
                }
                localStorage.setItem("PhysicianICD", JSON.stringify(arrListICDs));

                $scope.FillFavorite();


            }

        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            //  alert(error.Message + ".Please Contact Support!");
        });

        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }


    $scope.okClick = function () {

        var CheckedICDs = $.grep($(top.window.document).find('#tbFavICDsContainer tr:not(:first)'), function (row) {

            for (var i = 0; i < $(row).find("input[type=checkbox]").length; i++) {
                if ($(row).find("input[type=checkbox]")[i].checked)
                    return ($(row));
            }
        });

        var sResultICDs = "";
        for (var i = 0; i < CheckedICDs.length; i++) {

            var cells = $(CheckedICDs[i]).find('td');

            if (cells[0].textContent == "" && cells[0].childNodes[0] != undefined) {
                if (cells[0].childNodes[0].checked) {
                    if (sResultICDs == "")
                        sResultICDs += cells[1].innerText + "~" + cells[2].innerText;
                    else
                        sResultICDs += "|" + cells[1].innerText + "~" + cells[2].innerText;
                }
            }
            if (cells[3].textContent == "" && cells[3].childNodes[0] != undefined) {
                if (cells[3].childNodes[0].checked) {
                    {
                        if (sResultICDs == "")
                            sResultICDs += cells[4].innerText + "~" + cells[5].innerText;
                        else
                            sResultICDs += "|" + cells[4].innerText + "~" + cells[5].innerText;
                    }
                }
            }

            if (cells[6].textContent == "" && cells[6].childNodes[0] != undefined) {
                if (cells[6].childNodes[0].checked) {
                    {
                        if (sResultICDs == "")
                            sResultICDs += cells[7].innerText + "~" + cells[8].innerText;
                        else
                            sResultICDs += "|" + cells[7].innerText + "~" + cells[8].innerText;
                    }
                }
            }
        }


        if (sResultICDs == "") {
            DisplayErrorMessage('220004');
            return;
        }



        $http({
            url: "frmAssessmentNew.aspx/GetFormviewICDs",
            dataType: 'json',
            method: 'POST',
            data: '{sFormviewICD: "' + sResultICDs + '" }',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }

        }).success(function (response, status, headers, config) {
            if (response.d != "") {
                var test = JSON.parse(response.d);

                if (test.NoQuestionnaire.length > 0) {
                    for (var i = 0; i < test.NoQuestionnaire.length; i++) {
                        if (JSON.stringify($scope.AssessmentTable).indexOf(test.NoQuestionnaire[i].ICDCode) == -1) {

                            $scope.AssessmentTable.push(test.NoQuestionnaire[i]);
                            $("textarea").unbind();
                            iRightClickMenuCheck = false;
                        }


                    }
                    $scope.SaveEnableDisable(false);
                    $scope.ColorCoding();

                }

                $(top.window.document).find("#tbFavICDsContainer #dynTr").remove();

                $(top.window.document).find('#divFormView').modal('hide');
                if (test.Questionnaire != undefined) {
                    if (test.Questionnaire.length > 0) {
                        $scope.Questionnaire = test.Questionnaire;
                        bProblmCheck = true;
                        sSingleICD = "";
                        if (test.Questionnaire[0].MutuallyExclusive == "Y") {
                            sSelectedICD = test.Questionnaire[0].ICDCodeDesc + "|" + test.Questionnaire[0].MutuallyExclusive;
                            sSingleICD = sSelectedICD;
                        }
                        for (var i = 0; i < test.MainQuestionnaire.length; i++) {
                            if (sMainlevelQuestionnaire == "")
                                sMainlevelQuestionnaire = test.MainQuestionnaire[i].split('~')[0] + "-" + test.MainQuestionnaire[i].split('-')[1];
                            else
                                sMainlevelQuestionnaire = "~" + test.MainQuestionnaire[i].split('~')[0] + "-" + test.MainQuestionnaire[i].split('-')[1];

                            if (test.MainQuestionnaire[i].split('-')[1] == "Y") {
                                sSelectedICD += "^" + test.MainQuestionnaire[i].split('~')[0] + "-" + test.MainQuestionnaire[i].split('-')[0] + "|" + test.MainQuestionnaire[i].split('-')[1];
                                sSingleICD = "~" + test.MainQuestionnaire[i].split('~')[0];
                            }
                            if (sLabelText == "")
                                sLabelText = test.MainQuestionnaire[i].split('-')[0].replace("~", "-");
                            else
                                sLabelText += "~" + test.MainQuestionnaire[i].split('-')[0].replace("~", "-");



                        }
                        QuestionnaireArray.push(test.Questionnaire);// = response.d;
                        if (test.Questionnaire[0].MutuallyExclusive == "Y")
                            $scope.ColumnName = singlecolumn.ColumnName;
                        else
                            $scope.ColumnName = Multiplecolumn.ColumnName;

                        var labelText = $("#lblQuestionnaire").text();
                        $('#lblQuestionnaire').html(labelText + "" + test.Questionnaire[0].ICDCodeDesc + "--->");
                        bSetICD9Code = false;
                        $('#divMultipleICD9ModalQuestionnaire').modal('show');


                    }
                }

            }

        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            // alert(error.Message + ".Please Contact Support!");
        });

    }


    $(top.window.document).find("#cancel").click(function () {
        $scope.cancelClick();
        return false;
    });

    $scope.cancelClick = function () {
        if ($(top.window.document).find("#ok")[0].disabled == false) {
            if (DisplayErrorMessage('220019') == true) {
                //CAP-2152
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
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

    $(top.window.document).find("#ok").click(function () {
        $scope.okClick();
        return false;
    });

    $scope.FillFavorite = function () {
        if ($(top.window.document)?.find('#divFormView.in')?.length != undefined && $(top.window.document).find('#divFormView.in').length > 0) {
            $(top.window.document).find("#btnMaximizeViewResultICD").click();
        }
        else {
            $(top.window.document).find("#tbFavICDsContainer #dynTr").remove();
            $(top.window.document).find("#ok")[0].disabled = true;
            $(top.window.document).find('#divFormView').modal({ backdrop: 'static', keyboard: false }, 'show');
            $(top.window.document).find("#chkicd7").click(function () { alert("Hi"); });
            var PhysicianICDs = localStorage.getItem("PhysicianICD");
            PhysicianICDs = $.parseJSON(PhysicianICDs);
            var catlist = [];
            for (i = 0; i < PhysicianICDs.length; i++) {

                if (catlist.indexOf(PhysicianICDs[i].split('~')[2]) == -1)
                    catlist.push(PhysicianICDs[i].split('~')[2]);

            }
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
        }
    }

    $scope.OpenFormView = function () {

        if (localStorage.getItem("PhysicianICD") == "")
            $scope.LoadFavouriteICDS();
        else
            $scope.FillFavorite();

    }

    $scope.OpenSkipQuestionnaire = function () {


        if (sSingleICD != "") {
            sSelectedICD = "";
            sSelectedICD = "";
        }

        dataAllICD = sSublevelQuestionnaire.split('^')[0];

        if (sSublevelQuestionnaire != "") {
            bSublevelCheck = true;
            $scope.OpenNextQuestionnaire();
        }
        else if (sMainlevelQuestionnaire != "") { sMainlevelCheck = true; $scope.OpenQuestionnaire(); }

        else {
            sMainlevelCheck = false;
            var AddSelectedICD = sSelectedICD.split('^');
            var iCheckColor = 0;
            var Ass_status = "";
            if (statusDefaultLst.ASSESSMENT != undefined) {
                Ass_status = statusDefaultLst.ASSESSMENT;
            }
            for (var i = 0; i < AddSelectedICD.length; i++) {
                if (JSON.stringify($scope.AssessmentTable).indexOf(AddSelectedICD[i].split('~')[0]) == -1) {
                    $scope.AssessmentTable.push({ 'ICDCode': AddSelectedICD[i].split('~')[0], 'ICDDescription': AddSelectedICD[i].split('~')[1].split('|')[0], 'AssessmentID': 0, 'iVersion': 0, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Notes': '', 'IncompleteICDCode': AddSelectedICD[i].split('|')[2], 'Created_by': "", 'Created_date': "", 'Updated': "Y", 'StatusSelected': Ass_status, 'Orig_Status': Ass_status });
                    iCheckColor++;
                    $("textarea").unbind();
                    iRightClickMenuCheck = false;
                }
            }
            if (iCheckColor != 0)
                $scope.ColorCoding();
            if (AssoICDQuestionnaire != undefined && bAssoICDOpen != true) {
                if (AssoICDQuestionnaire.length > 0) {
                    $scope.OpenAssICDQuestionnaire();
                }
            }
            else {
                $('#divMultipleICD9ModalQuestionnaire').modal('hide');
                $scope.ClearAllLocalVariables();
            }
        }
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

    $scope.RefershGrid = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $http({
            url: "frmAssessmentNew.aspx/RefershTable",
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
            //alert(error.Message + ".Please Contact Support!");
        });
    }

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
                        if (item.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) >= 0)
                            return item.toLowerCase().split('|')[0];
                    });
                }
                else {
                    resultant = $.grep(first_resultant, function (item) {
                        if (item.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1)
                            return item.toLowerCase().split('|')[0];
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
    //$('#dlstICD10').load("htmICD10.html?version" + sessionStorage.getItem("ScriptVersion").split('|')[0].trim(), function () {
    $('#dlstICD10').load("htmICD10.html?version" + localStorage.getItem("ScriptVersion").split('|')[0].trim(), function () {
        arrICD10Codes = $.map($('#dlstICD10 option'), function (li) {
            return $(li).attr("value");
        });

    });

    $("#txtICD10").autocomplete({
        source:
            function (request, response) {
                if (intCPTLength == 0 && bcheck && bBool == false) {
                    arrCPTs = [];
                    bBool = true;
                }
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
        select: function (e, ui) {
            $("#txtICD10").val(ui.item.label).trigger('input');
            event.preventDefault();
            if (ui.item.label != "No matches found.") {

                bBool = false;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmAssessmentNew.aspx/GetAssociatedICDList",
                    data: '{sAssociateICD: "' + ui.item.label.split('~')[0] + '" }',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        if (data.d == "") {
                            var Ass_status = "";
                            if (statusDefaultLst.ASSESSMENT != undefined) {
                                Ass_status = statusDefaultLst.ASSESSMENT;
                            }
                            if (JSON.stringify($scope.AssessmentTable).indexOf(ui.item.label.split('~')[0].trim()) == -1) {

                                $scope.AssessmentTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'AssessmentID': 0, 'iVersion': 0, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Notes': '', 'Created_by': "", 'Created_date': "", 'Updated': "Y", 'StatusSelected': Ass_status, 'Orig_Status': Ass_status });
                                $("textarea").unbind();
                                iRightClickMenuCheck = false;
                                $scope.SaveEnableDisable(false);
                                $scope.ColorCoding();
                            }
                        }
                        else {

                            $scope.RefershGrid();
                            var str = data.d;
                            var test = JSON.parse(str);
                            bSetICD9Code = true;
                            bProblmCheck = true;
                            $scope.Questionnaire = test.Questionnaire;
                            if (test.Questionnaire[0].MutuallyExclusive == "Y")
                                $scope.ColumnName = singlecolumn.ColumnName;
                            else
                                $scope.ColumnName = Multiplecolumn.ColumnName;

                            sSingleICD = "";
                            sSelectedICD = ui.item.label.split('~')[0] + "~" + ui.item.label.split('~')[1] + "|" + "Y";
                            sSingleICD = ui.item.label.split('~')[0];
                            var labelText = $("#lblQuestionnaire").text();
                            $('#lblQuestionnaire').html(labelText + "" + ui.item.label.split('~')[0] + "-" + ui.item.label.split('~')[1] + "--->");
                            QuestionnaireArray.push(test.Questionnaire);// = str;
                            if ($.isReady) {
                                $('#divMultipleICD9ModalQuestionnaire').modal({ backdrop: 'static', keyboard: false }, 'show');
                            }
                        }
                        $('#txtICD10').val("");
                    }, error: function OnError(xhr) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        if (xhr.status == 999)
                            window.location = "/frmSessionExpired.aspx";
                        else {
                            //CAP-798 Unexpected end of JSON input
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
            else {
                $('#txtICD10').val("");
                bBool = false;
            }
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
                    url: "frmAssessmentNew.aspx/SearchDescrptionText",
                    //CAP-1667
                    data: JSON.stringify({ text: document.getElementById("txtDescription").value + "|txtDescription" + "|ICD_10" }),
                   /* data: "{\"text\":\"" + document.getElementById("txtDescription").value + "|" + "txtDescription" + "|" + "ICD_10" + "\"}",*/
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var jsonData = $.parseJSON(data.d);
                        //Jira CAP-2140
                        arrCPTs = jsonData;
                        jsonData = jsonData.slice(0, 100);
                        //Jira CAP-2140 End
                        if (jsonData.length == 0) {
                            jsonData.push('No matches found.')
                            response($.map(jsonData, function (item) {
                                return {
                                    label: item.split('|')[0]
                                }
                            }));
                        }
                        else {
                            response($.map(jsonData, function (item) {
                                //Jira CAP-2140
                                //arrCPTs.push(item);
                                return {
                                    label: item.split('|')[0]
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
                    //Jira CAP-2140
                    results = results.slice(0, 100);
                    if (results.length == 0) {
                        results.push('No matches found.')
                        response($.map(results, function (item) {
                            return {
                                label: item.split('|')[0]
                            }
                        }));
                    }
                    else {
                        response($.map(results, function (item) {
                            return {
                                label: item.split('|')[0]
                            }
                        }));
                    }
                }
            }
        },
        minlength: 3,
        multiple: true,
        mustMatch: false,
        open: function () {
            $('.ui-menu').width(610); $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        select: function (event, ui) {
            event.preventDefault();

            if (ui.item.label != "No matches found.") {
                bBool = false;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmAssessmentNew.aspx/GetAssociatedICDList",
                    data: '{sAssociateICD: "' + ui.item.label.split('~')[0] + '" }',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        if (data.d == "") {
                            var Ass_status = "";
                            if (statusDefaultLst.ASSESSMENT != undefined) {
                                Ass_status = statusDefaultLst.ASSESSMENT;
                            }
                            if (JSON.stringify($scope.AssessmentTable).indexOf(ui.item.label.split('~')[0].trim()) == -1) {

                                $scope.AssessmentTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'AssessmentID': 0, 'iVersion': 0, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Notes': '', 'Created_by': "", 'Created_date': "", 'Updated': "Y", 'StatusSelected': Ass_status, 'Orig_Status': Ass_status });
                                $("textarea").unbind();
                                iRightClickMenuCheck = false;
                                $scope.SaveEnableDisable(false);
                                $scope.ColorCoding();
                            }
                        }
                        else {

                            $scope.RefershGrid();
                            var str = data.d;
                            var test = JSON.parse(str);
                            bSetICD9Code = true;
                            bProblmCheck = true;
                            $scope.Questionnaire = test.Questionnaire;
                            if (test.Questionnaire[0].MutuallyExclusive == "Y")
                                $scope.ColumnName = singlecolumn.ColumnName;
                            else
                                $scope.ColumnName = Multiplecolumn.ColumnName;

                            sSingleICD = "";
                            sSelectedICD = ui.item.label.split('~')[0] + "~" + ui.item.label.split('~')[1] + "|" + "Y";
                            sSingleICD = ui.item.label.split('~')[0];
                            var labelText = $("#lblQuestionnaire").text();
                            $('#lblQuestionnaire').html(labelText + "" + ui.item.label.split('~')[0] + "-" + ui.item.label.split('~')[1] + "--->");
                            QuestionnaireArray.push(test.Questionnaire);
                            if ($.isReady) {
                                $('#divMultipleICD9ModalQuestionnaire').modal({ backdrop: 'static', keyboard: false }, 'show');
                            }

                        }
                        $('#txtDescription').val("");
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
                        }
                    }

                });

            }
            else {
                $('#txtDescription').val("");
                bBool = false;
            }
        }
    }).on("paste", function (e) {
        intCPTLength = -1;
        arrCPTs = [];
        $(".ui-autocomplete").hide();
    }).on("keydown", function (e) {
        if (e.which == 8) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtDescription").val().length <= 4)
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
        if ($("#txtDescription").val().length >= 4) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block') { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool) { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intCPTLength = 0;
        }
        else if ($("#txtDescription").val().length != 0 && intCPTLength != -1) {
            intCPTLength = intCPTLength + 1;
        }
        if ($("#txtDescription").val().length < 3) {
            intCPTLength = -1;
            arrCPTs = [];
            $(".ui-autocomplete").hide();
            bBool = false;
        }
    });


    $scope.sFavAutoComplete = function () {
        if (iFavCheck == 0) {
            var arrphysician = localStorage.getItem("PhysicianICD");
            arrphysician = $.parseJSON(arrphysician);
            iFavCheck++;
        }
    }

    $scope.CopyPreviousEncounter = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        $http({
            url: "frmAssessmentNew.aspx/LoadAssessmentTable",
            dataType: 'json',
            method: 'POST',
            data: '{strAssessment: "CopyPrevious" }',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {
            var obj = response.d;

            if (obj.indexOf("Message-") > -1) {
                DisplayErrorMessage(obj.split('-')[1]);
                $scope.SaveEnableDisable(true);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
            else {
                var objAssessmentDTO = JSON.parse(obj);

                $scope.AssessmentTable = objAssessmentDTO.AssessmentList;
                $scope.orderByField = ['-IsPrimary', 'ICDCode'];
                $scope.reverseSort = true;

                $scope.AddDeleteArrayOnCopyPrevious();
                test = objAssessmentDTO.ICD10Tool;

                if (objAssessmentDTO.ResultGeneralNotes != "")
                    $('#txtGeneralNotes').val(objAssessmentDTO.ResultGeneralNotes);

                if (test.length > 0) {
                    bICD10Conversioncheck = false;
                    $scope.OpenICD10Conversion();
                }
                else {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
                for (var i = 0; i < $scope.AssessmentTable.length; i++) {
                    $scope.AssessmentTable[i].Updated = "Y";
                }
                $scope.SaveEnableDisable(false);
                bcolorcoding = false;
                $scope.ColorCoding();
            }
        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            // alert(error.Message + ".Please Contact Support!");
        });
    }

    $scope.AddDeleteArrayOnCopyPrevious = function () {

        var table = $('#tblCurrICDs');
        table.find('tr').each(function (rowIndex, r) {
            var colsDel = [];
            $(this).find('td').each(function (colIndex, c) {
                if (colIndex == 0) {
                    colsDel.push("Del");
                }
                else if (c.textContent == "") {
                    colsDel.push(c.childNodes[0].checked);
                }
                else if (colIndex == 6) {
                    var select = c.childNodes;

                    for (var i = 0; i < select.length; i++) {
                        if ($(select)[i].value != undefined)
                            colsDel.push($(select)[i].value);
                    }
                }
                else if (colIndex == "8") {

                }
                else if (colIndex == "9") {
                    colsDel.push('0');
                }
                else {
                    colsDel.push(c.textContent);
                }
            });
            if (colsDel.length != 0) {
                DeleteArray.push(colsDel);
            }
        });
    }

    var iIndex = -1;
    $scope.Save = function (index) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        if (index != undefined)
            iIndex = index;
        var DelList = [];
        var table = $('#tblCurrICDs');
        var iCheck = iIndex + 1;
        $scope.AssessmentTable[iIndex].Updated = "Y";
        iIndex = -1;
        table.find('tr').each(function (rowIndex, r) {
            if (rowIndex == iCheck) {
                var colsDel = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (colIndex == 0) {
                        colsDel.push("Del");
                    }
                    else if (c.textContent == "") {
                        colsDel.push(c.childNodes[0].checked);
                    }
                    else if (colIndex == 6) {
                        var select = c.childNodes;

                        for (var i = 0; i < select.length; i++) {
                            if ($(select)[i].value != undefined)
                                colsDel.push($(select)[i].value);

                        }

                    }
                    else if (colIndex == "8") {


                    }
                    else {
                        colsDel.push(c.textContent);
                    }
                });
                DeleteArray.push(colsDel);
                DelList.push(colsDel);
            }
        });


        var bDeleteCheck = false;
        for (var i = 0; i < $scope.AssessmentTable.length; i++) {
            if (DelList[0][4] == $scope.AssessmentTable[i].ICDCode && !bDeleteCheck) {
                $scope.AssessmentTable.splice(i, 1);
                bDeleteCheck = true;
                $scope.SaveEnableDisable(false);
            }
        }

        $scope.ColorCoding();
        //   }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


    }
    $scope.OpenQuestionnaire = function () {
        var iCol = 0;
        var table = $('#ICDDescQuestionnaire');
        var dataChecked = "";
        if (!sMainlevelCheck) {
            table.find('tr').each(function (rowIndex, r) {

                var cols = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (c.childNodes[1].checked) {
                        if (dataChecked == "") {
                            dataChecked = c.childNodes[2].textContent.split('-')[0];
                            sLabelText = c.childNodes[2].textContent;
                        }
                        else {
                            dataChecked += "~" + c.childNodes[2].textContent.split('-')[0];
                            sLabelText += "~" + c.childNodes[2].textContent;
                        }
                    }
                });
            });

            if (dataChecked.indexOf('~') > -1)
                sMainlevelQuestionnaire = dataChecked.replace(dataChecked.split('~')[0].trim() + "~", "");
            dataChecked = dataChecked.split('~')[0].trim();

            var labelText = $("#lblQuestionnaire").text();
            $('#lblQuestionnaire').html(labelText + "" + sLabelText.split('~')[0] + "--->");

            sLabelText = sLabelText.replace(sLabelText.split('~')[0] + "~", "");

        }
        else {
            sMainlevelCheck = false;
            dataChecked = sMainlevelQuestionnaire.split('~')[0];
            sMainlevelQuestionnaire = sMainlevelQuestionnaire.replace(sMainlevelQuestionnaire.split('~')[0] + "~", "").replace(sMainlevelQuestionnaire.split('~')[0], "");
            $('#lblQuestionnaire').html("Please select the Diagnosis related to:" + sLabelText.split('~')[0] + "--->");

            sLabelText = sLabelText.replace(sLabelText.split('~')[0] + "~", "");
        }

        if (dataChecked.trim() == "") {
            DisplayErrorMessage('220020');
            return;
        }


        else {

            $http({
                url: "frmAssessmentNew.aspx/SearchQuestionnaire",
                dataType: 'json',
                method: 'POST',
                data: '{sICD: "' + dataChecked + '" }',
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                }
            }).success(function (response) {
                var str = response.d;
                bSetICD9Code = false;
                var test = JSON.parse(str);
                $scope.Questionnaire = test.Questionnaire;


                if (test.Questionnaire[0].MutuallyExclusive == "Y")
                    $scope.ColumnName = singlecolumn.ColumnName;
                else
                    $scope.ColumnName = Multiplecolumn.ColumnName;
                QuestionnaireArray = [];
                QuestionnaireArray.push(test.Questionnaire);// = str;
                if ($.isReady) {
                    $('#divMultipleICD9ModalQuestionnaire').modal({ backdrop: 'static', keyboard: false }, 'show');
                }
            }).error(function (error, status, headers, config) {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else
                    window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                //alert(error.Message + ".Please Contact Support!");
            });
        }


    }
    $scope.CheckedQuestionnaire = function (index) {


        var table = $('#tblQuestionnaire');
        table.find('tr').each(function (rowIndex, r) {
            if (rowIndex != 0) {
                var cols = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (c.textContent == "") {
                        if (c.childNodes[0].checked && rowIndex != index + 1)
                            c.childNodes[0].checked = false;


                    }
                    if (colIndex == "4" && c.textContent != "") {
                        ICDCheckNode = ICDCheckNode.replace(c.textContent, "").replace(c.textContent + ",", "");
                        c.textContent = "";

                    }
                });

            }
        });



    }
    $scope.OpenBackQuestionnaire = function () {


        QuestionnaireArray = QuestionnaireArray.slice(0, QuestionnaireArray.length - 1);
        if (QuestionnaireArray.length == 0) {

            if (sSingleICD != "") {
                sSelectedICD = "";
                sSelectedICD = "";
            }
            var Ass_status = "";
            if (statusDefaultLst.ASSESSMENT != undefined) {
                Ass_status = statusDefaultLst.ASSESSMENT;
            }

            var AddSelectedICD = sSelectedICD.split('^');
            for (var i = 0; i < AddSelectedICD.length; i++) {
                if (JSON.stringify($scope.AssessmentTable).indexOf(AddSelectedICD[i].split('~')[0]) == -1) {
                    $scope.AssessmentTable.push({ 'ICDCode': AddSelectedICD[i].split('~')[0], 'ICDDescription': AddSelectedICD[i].split('~')[1].split('|')[0], 'AssessmentID': 0, 'iVersion': 0, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Notes': '', 'Created_by': "", 'Created_date': "", 'Updated': "Y", 'StatusSelected': Ass_status, 'Orig_Status': Ass_status });
                    $("textarea").unbind();
                    iRightClickMenuCheck = false;
                }
            }
            $scope.ColorCoding();
            if (AssoICDQuestionnaire != undefined && bAssoICDOpen != true) {
                if (AssoICDQuestionnaire.length > 0) {
                    $scope.OpenAssICDQuestionnaire();
                }
            }
            else {
                $('#divMultipleICD9ModalQuestionnaire').modal('hide');
                $scope.ClearAllLocalVariables();
            }
        }
        else {
            var test12 = QuestionnaireArray[QuestionnaireArray.length - 1];
            var labelText = $("#lblQuestionnaire").text();
            for (var i = 0; i < test12.length; i++) {
                if (ICDCheckNode.indexOf(test12[i].ICDCode) > -1) {

                    var ResultCheckNode = ICDCheckNode.split(',');
                    var iResultCheckNode = 0;
                    for (var k = 0; k < ResultCheckNode.length; k++) {
                        if (ResultCheckNode[k].trim() == test12[i].ICDCode)
                            iResultCheckNode++;
                    }
                    if (iResultCheckNode > 0) {
                        test12[i].CheckLeafNode = test12[i].ICDCode;
                        sSublevelQuestionnaire = sSublevelQuestionnaire.replace(test12[i].ICDCode + "-" + test12[i].LeafNode + "^", "").replace(test12[i].ICDCode + "-" + test12[i].LeafNode, "");
                        if (labelText.indexOf(test12[i].ICDCode)) {
                            labelText = labelText.replace(test12[i].ICDCode + "-" + test12[i].ICDDescription + "--->", "");
                            $('#lblQuestionnaire').html(labelText);
                        }

                        ICDCheckNode = ICDCheckNode.replace(test12[i].ICDCode, "").replace(test12[i].ICDCode + ",", "");
                        if (sSelectedICD.indexOf(test12[i].ICDCode) > -1) {
                            sSelectedICD = sSelectedICD.replace(test12[i].ICDCode + "~" + test12[i].ICDDescription + "|" + test12[i].LeafNode + "|" + labelText.split(':')[1].split('-')[0] + "^", "").replace(test12[i].ICDCode + "-" + test12[i].ICDDescription + "|" + test12[i].LeafNode + "|" + labelText.split(':')[1].split('-')[0], "");
                        }
                    }
                    else
                        test12[i].CheckLeafNode = "";
                }
                else
                    test12[i].CheckLeafNode = "";
            }

            $scope.Questionnaire = test12;
            if (test12[0].MutuallyExclusive == "Y")
                $scope.ColumnName = singlecolumn.ColumnName;
            else
                $scope.ColumnName = Multiplecolumn.ColumnName;
        }
    }

    var iSelectMoreDiagnosis = -1;
    $scope.OpenNextQuestionnaire = function () {

        var iCheck = 0;
        var leafNode = 0;
        var iMultiselect = 0;
        var table = $('#tblQuestionnaire');
        var sLabelChange = "";
        var iLabelcheck = 0;
        var dataAllICD = "";
        var iValue = 0;
        sMutilSelectICD = "";


        if (sAssociatedSelectedICD != "") {
            if (sSelectedICD == "")
                sSelectedICD += sAssociatedSelectedICD;
            else
                sSelectedICD += "^" + sAssociatedSelectedICD;

            sAssociatedSelectedICD = "";
        }
        if (!bSublevelCheck) {
            table.find('tr').each(function (rowIndex, r) {

                if (rowIndex == 0) {
                    $(this).find('th').each(function (colIndex, c) {

                        if (c.textContent != "Select(One Diagnosis) ") {

                            for (var i = 0; i < iSelectMoreDiagnosis; i++) {

                                QuestionnaireArray = QuestionnaireArray.slice(0, QuestionnaireArray.length - 1);

                            }
                            iSelectMoreDiagnosis = 0;
                        }
                        else if (iSelectMoreDiagnosis > -1)
                            iSelectMoreDiagnosis++;


                    });
                }
                if (rowIndex != 0) {
                    var cols = [];
                    var tempSelectedValue = "";
                    $(this).find('td').each(function (colIndex, c) {
                        if (colIndex == 0) {
                            if (c.textContent == "") {
                                if (c.childNodes[0].checked) {
                                    iCheck++;
                                    iLabelcheck++;
                                    iMultiselect++;
                                }

                            }
                        }

                        else if (iCheck != 0) {
                            dataAllICD = c.textContent;

                            if (sSublevelLabelText == "")
                                sSublevelLabelText = c.textContent;
                            else
                                sSublevelLabelText += "|" + c.textContent;

                            if (iLabelcheck == 1)
                                sLabelChange += c.textContent;
                            tempSelectedValue = c.textContent;
                            if (ICDCheckNode == "")
                                ICDCheckNode = c.textContent;
                            else
                                ICDCheckNode += "," + c.textContent;

                            if (iMultiselect > 0) {
                                if (sMutilSelectICD == "")
                                    sMutilSelectICD = c.textContent;
                                else
                                    sMutilSelectICD += "^" + c.textContent;
                            }

                            iCheck = 0;
                            leafNode++;
                        }
                        else if (colIndex == 2) {
                            if (sSublevelLabelText != "" && leafNode != 0)
                                sSublevelLabelText += "-" + c.textContent;
                            tempSelectedValue += "~" + c.textContent;
                            if (iLabelcheck == 1 && leafNode != 0) {
                                sLabelChange += "-" + c.textContent;
                            }
                        }
                        else if (leafNode != 0 && colIndex == 3) {

                            if (c.textContent == "Y") {
                                if (sSelectedICD == "")
                                    sSelectedICD += tempSelectedValue + "|" + c.textContent + "|" + $("#lblQuestionnaire").text().split(':')[1].split('-')[0];
                                else
                                    sSelectedICD += "^" + tempSelectedValue + "|" + c.textContent + "|" + $("#lblQuestionnaire").text().split(':')[1].split('-')[0];
                            }

                            if (iMultiselect > 0) {
                                sMutilSelectICD += "-" + c.textContent;
                            }
                            dataAllICD += "-" + c.textContent;
                            leafNode = 0;

                        }


                    });

                }
            });


            if (sSublevelLabelText.indexOf('|') == -1)
                sSublevelLabelText = "";
            if (sMutilSelectICD.indexOf('^') > -1) {
                if (sSublevelQuestionnaire == "")
                    sSublevelQuestionnaire = sMutilSelectICD;
                else
                    sSublevelQuestionnaire += "^" + sMutilSelectICD;



                dataAllICD = sSublevelQuestionnaire.split('^')[0];
                sSublevelQuestionnaire = sSublevelQuestionnaire.replace(sSublevelQuestionnaire.split('^')[0] + "^", "").replace(sSublevelQuestionnaire.split('^')[0], "");

            }
            else
                sMutilSelectICD = "";


            var labelText = $("#lblQuestionnaire").text();
            $('#lblQuestionnaire').html(labelText + "" + sLabelChange.split('~')[0] + "--->");

        }
        else {
            bSublevelCheck = false;
            dataAllICD = sSublevelQuestionnaire.split('^')[0];
            sSublevelQuestionnaire = sSublevelQuestionnaire.replace(sSublevelQuestionnaire.split('^')[0] + "^", "").replace(sSublevelQuestionnaire.split('^')[0], "");

            var labelText = $("#lblQuestionnaire").text();

            var sCurrentText = sSublevelLabelText.split('|');

            for (var i = 0; i < sCurrentText.length; i++) {
                if (labelText.indexOf(sCurrentText[i]) > -1) {
                    labelText = labelText.replace(sCurrentText[i] + "--->", "");


                }
                if (dataAllICD.indexOf(sCurrentText[i].split('-')[0]) > -1)
                    labelText += sCurrentText[i] + "--->";

            }


            $('#lblQuestionnaire').html(labelText);
        }


        if (dataAllICD == "") {
            DisplayErrorMessage('550002');
            return;
        }

        $http({

            url: "frmAssessmentNew.aspx/SearchQuestionnairelIST",
            dataType: 'json',
            method: 'POST',
            async: true,
            data: '{sICDS: "' + dataAllICD + '" }',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {
            var str = response.d;
            if (response.d != "") {
                var test = JSON.parse(str);
                $scope.Questionnaire = test.Questionnaire;

                if (test.Questionnaire[0].MutuallyExclusive == "Y") {

                    $scope.ColumnName = singlecolumn.ColumnName;
                }
                else {
                    $scope.ColumnName = Multiplecolumn.ColumnName;
                }
                QuestionnaireArray.push(test.Questionnaire);
                if (response.d != "") {
                    if ($.isReady)
                        $('#divMultipleICD9ModalQuestionnaire').modal({ backdrop: 'static', keyboard: false }, 'show');
                    else {
                        $('#divMultipleICD9ModalQuestionnaire').modal('hide');
                        $scope.ClearAllLocalVariables();
                    }

                }
                else if (sMutilSelectICD == "" && sMainlevelQuestionnaire == "") {
                    $('#divMultipleICD9ModalQuestionnaire').modal('hide');
                    $scope.ClearAllLocalVariables();
                }
                else if (sMainlevelQuestionnaire != "") {
                    sMainlevelCheck = true;
                    $scope.OpenQuestionnaire();
                }

            }
            else if (sSublevelQuestionnaire == "" && sMainlevelQuestionnaire == "") {
                var AddSelectedICD = sSelectedICD.split('^');
                var Ass_status = "";
                if (statusDefaultLst.ASSESSMENT != undefined) {
                    Ass_status = statusDefaultLst.ASSESSMENT;
                }
                for (var i = 0; i < AddSelectedICD.length; i++) {
                    //CAP-1694
                    var icdCode = AddSelectedICD[i].split('~').length > 1 ? AddSelectedICD[i].split('~')[0] : AddSelectedICD[i].split('-')[0];
                    if (JSON.stringify($scope.AssessmentTable).indexOf(AddSelectedICD[i].split('~')[0].trim()) == -1) {
                        if (bSetICD9Code || bProblmCheck)
                            $scope.AssessmentTable.push({
                                'ICDCode': icdCode, 'ICDDescription': (AddSelectedICD[i].split('~').length > 1 ? AddSelectedICD[i].split('~')[1].split('|')[0] : AddSelectedICD[i].split('-')[1].split('|')[0]), 'ParentICD': icdCode, 'AssessmentID': 0, 'iVersion': 0, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Notes': '', 'IncompleteICDCode': ' ', 'Created_by': "", 'Created_date': "", 'Updated': "Y", 'StatusSelected': Ass_status, 'Orig_Status': Ass_status
                    });
                        else
                            $scope.AssessmentTable.push({ 'ICDCode': icdCode, 'ICDDescription': (AddSelectedICD[i].split('~').length > 1 ? AddSelectedICD[i].split('~')[1].split('|')[0] : AddSelectedICD[i].split('-')[1].split('|')[0]), 'ParentICD': icdCode, 'AssessmentID': 0, 'iVersion': 0, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Notes': '', 'IncompleteICDCode': AddSelectedICD[i].split('|').length > 2 ? AddSelectedICD[i].split('|')[2] : "", 'CheckBoxCheck': 'PROBLEM', 'Created_by': "", 'Created_date': "", 'Updated': "Y", 'StatusSelected': Ass_status, 'Orig_Status': Ass_status });

                        $("textarea").unbind();
                        iRightClickMenuCheck = false;
                        $scope.SaveEnableDisable(false);

                    }
                }
                $scope.ColorCoding();
                bSetICD9Code = false;

                if (AssoICDQuestionnaire != undefined && bAssoICDOpen != true) {
                    if (AssoICDQuestionnaire.length > 0) {
                        $scope.OpenAssICDQuestionnaire();
                    }
                }
                else {
                    $('#divMultipleICD9ModalQuestionnaire').modal('hide');
                    $scope.ClearAllLocalVariables();
                }
            }
            else if (sSublevelQuestionnaire != "") {

                bSublevelCheck = true;
                $scope.OpenNextQuestionnaire();
            }
            else if (sMainlevelQuestionnaire != "") {
                sMainlevelCheck = true;
                $scope.OpenQuestionnaire();
            }


        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
            //alert(error.Message + ".Please Contact Support!");
        });
    }

    $scope.SaveAssessment = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        var bCheck = false, bprimary = false, bpriSuspected = false;
        var table = $('#tblCurrICDs');
        var data = [];

        table.find('tr').each(function (rowIndex, r) {
            var cols = [];

            $(this).find('td').each(function (colIndex, c) {

                if (c.textContent == "" && colIndex <= "3") {
                    if (c.childNodes[0].checked && colIndex == "1") {
                        bCheck = true;
                        bprimary = true;
                    }
                    else if (!c.childNodes[0].checked && colIndex == "1") {
                        bprimary = false
                    }
                    cols.push(c.childNodes[0].checked);
                }

                else if (colIndex == "6") {

                    var select = c.childNodes;

                    for (var i = 0; i < select.length; i++) {
                        if ($(select)[i].value != undefined)
                            cols.push($(select)[i].value);
                        if ($(select)[i].value != undefined && $(select)[i].value.toLowerCase() == "suspected" && bprimary) {
                            bpriSuspected = true;
                        }
                    }
                }
                else if (colIndex == "7") {
                    cols.push(c.childNodes[0].nextElementSibling.value);
                }
                else if (colIndex == "8") {
                }
                else {
                    cols.push(c.textContent);
                }
            });
            //modified for integrum
            data.push(cols);
        });

        if (bpriSuspected) {//BugID:49118
            DisplayErrorMessage('220023');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("Assauto", "N");
            AutoSaveUnsuccessful();
            return "false";
        }
        if (!bCheck && $scope.AssessmentTable.length > 0) {
            localStorage.setItem("Assauto", "N");
            DisplayErrorMessage('220006');
            {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                AutoSaveUnsuccessful();
                return "false";
            }
        }

        if (bcolorcoding) {
            localStorage.setItem("Assauto", "N");
            if (!DisplayErrorMessage('220016')) {
                //CAP-2230
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                AutoSaveUnsuccessful();
                return "false";
            }
        }

        if (DeleteArray.length != 0)
            data = data.concat(DeleteArray);

        localStorage.setItem("bSave", "true");

        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        localStorage.setItem("Assauto", "Y");
        $http({
            url: "frmAssessmentNew.aspx/SaveAssessmentTable",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({ name: data, sGeneralNotes: $('#txtGeneralNotes').val() }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {

            var str = response.d;
            var test = JSON.parse(str);

            if (test.TruncatedICDList != undefined && test.TruncatedICDList != null) {
                alert("Please delete or update the following invalid/incomplete ICD codes: " + test.TruncatedICDList.toString() + " .");

                //DisplayErrorMessage('220025', "", test.TruncatedICDList.toString());
                sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                //Jira CAP-976 - Start
                //return;
                AutoSaveUnsuccessful();
                return false;
                //Jira CAP-976 - End
            }


            $scope.AssessmentTable = test.AssessmentList;

            var IncompleteList = test.InCompleteProblemList;

            var iSpliceCount = -1;


            for (var i = $scope.FromProblemList.length - 1; i >= 0; i--) {

                if (JSON.stringify(IncompleteList).indexOf($scope.FromProblemList[i].ICDCODE) > -1) {
                    $scope.FromProblemList.splice(i, 1);
                }
            }

            bcolorcoding = false;
            $scope.ColorCoding();

            DeleteArray = [];
            DisplayErrorMessage('220005');

            AutoSaveSuccessful();
            $scope.SaveEnableDisable(true);
            DisableChartLevelAutoSave();//BugID:52795
            var ProblemList = test.ProblemList;

            var regex = /<BR\s*[\/]?>/gi;
            top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = ProblemList[0];
            top.window.document.getElementById("ProblemList_tooltp").innerText = ProblemList[1].replace(regex, "\n") + "\n";
            RefreshOverallSummaryTooltip();

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            RefreshNotification('Assessment|ServiceAndProcedureCode');
            RefreshOverallSummaryTooltip();
            //RafCalculation();

            if (test.IsAssessmentRAFUpdate == "Y") {
                RAFRefreshCLick();
            }
        })
            .error(function (error, status, headers, config) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                AutoSaveUnsuccessful();
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                //else
                //    alert(error.Message + ".Please Contact Support!");
                else {
                    window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                }
            });
    }

    function RAFRefreshCLick() {
        loadRAF(window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText, "N");
    }

    function loadRAF(sHumanDetails, Is_store) {
        var regex = /<BR\s*[\/]?>/gi;
        var RAF_Score = "RAF Score :";

        var HUMAN_ID = sHumanDetails.split('|')[4].split(':')[1].trim();
        var DOS = "";
        $.ajax({
            type: "POST",
            url: "frmAssessmentNew.aspx/GetEncounterDetailsforRaf",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Human_id: HUMAN_ID }),
            dataType: "json",
            async: true,
            success: function (data) {
                var Getdata = data.d.split('~');
                var HUMAN_ID = '';
                var DOB = '';
                var Gender = '';
                var icdlist = '';
                var sIsMedicaid = '';
                var sIsDisabled = '';
                var sIsCommunity = '';
                var sOriginallyDisabled = '';
                var sEnrollmentStatus = '';
                var PrimaryCarrier = '';
                var DOS = '';
                if (Getdata.length > 0) {
                    icdlist = Getdata[0];
                    sIsMedicaid = Getdata[1].split(':')[1];
                    sOriginallyDisabled = Getdata[2].split(':')[1];
                    sIsCommunity = Getdata[3].split(':')[1];
                    sIsDisabled = Getdata[4].split(':')[1];
                    sEnrollmentStatus = Getdata[5].split(':')[1];
                    PrimaryCarrier = Getdata[6].split(':')[1];
                    DOS = Getdata[7].split(':')[1].replace('"', '');
                }
                top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = "Calculating....."
                top.window.document.getElementById("RAF_tooltp").innerText = "Calculating.....";
                if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined) {
                    var sHumanDetails = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
                    HUMAN_ID = sHumanDetails.split('|')[4].split(':')[1].trim();
                    if (sHumanDetails.split('|')[3].trim() == 'F')
                        Gender = 'FEMALE';
                    else if (sHumanDetails.split('|')[3].trim() == 'M')
                        Gender = 'MALE'
                    DOB = sHumanDetails.split('|')[1];
                }

                var Current = "", Previous = "", SPrevious = ""

                if (DOS == "0001") {
                    DOS = "2018";
                }
                var now = new Date();
                if (DOS == "") {
                    Current = now.getUTCFullYear() + "|Y";
                    Previous = now.getUTCFullYear() - 1 + "|Y";
                    SPrevious = now.getUTCFullYear() - 2 + "|Y";
                }
                else {
                    if (now.getUTCFullYear().toString() == DOS) {
                        Current = now.getUTCFullYear() + "|Y";
                        Previous = now.getUTCFullYear() - 1 + "|N";
                        SPrevious = now.getUTCFullYear() - 2 + "|N";
                    }
                    else if ((now.getUTCFullYear() - 1).toString() == DOS) {
                        Current = now.getUTCFullYear() + "|N";
                        Previous = now.getUTCFullYear() - 1 + "|Y";
                        SPrevious = now.getUTCFullYear() - 2 + "|N";
                    }
                    else {
                        Current = now.getUTCFullYear() + "|N";
                        Previous = now.getUTCFullYear() - 1 + "|N";
                        SPrevious = now.getUTCFullYear() - 2 + "|Y";

                    }
                }

                var year = Current + '~' + Previous + "~" + SPrevious;
                var icdlist = icdlist;
                var is_store_Value = 'Y';
                var ProjectName = '';
                var surl = '';

                if (top.window.document.getElementById("ctl00_hdnProjectName").value != '') {
                    ProjectName = top.window.document.getElementById("ctl00_hdnProjectName").value;
                }
                if (top.window.document.getElementById("ctl00_hdnProjectIPAddress").value != '')
                    surl = top.window.document.getElementById("ctl00_hdnProjectIPAddress").value;
                if (rafcalc != undefined && rafcalc != "") {
                    HUMAN_ID = rafcalc.split('^')[0];
                    DOB = rafcalc.split('^')[1];
                    Gender = rafcalc.split('^')[2];
                }

                var WSData = JSON.stringify({
                    ProjectName: ProjectName,
                    human_id: HUMAN_ID,
                    Gender: Gender,
                    DOB: DOB,
                    year: year,
                    icdlist: icdlist,
                    is_store_Value: is_store_Value,
                    sIsMedicaid: sIsMedicaid,
                    sIsDisabled: sIsDisabled,
                    sIsCommunity: sIsCommunity,
                    sOriginallyDisabled: sOriginallyDisabled,
                    sEnrollmentStatus: sEnrollmentStatus,
                    PrimaryCarrier: PrimaryCarrier
                });
                $.get(surl + '/RafCalculator?ProjectName=' + ProjectName + '&human_id=' + HUMAN_ID + '&Gender=' + Gender + '&DOB=' + DOB + '&year=' + year + '&icdlist=' + icdlist + '&is_store_Value=' + is_store_Value + '&sIsMedicaid=' + sIsMedicaid + '&sIsDisabled=' + sIsDisabled + '&sIsCommunity=' + sIsCommunity + '&sOriginallyDisabled=' + sOriginallyDisabled + '&sEnrollmentStatus=' + sEnrollmentStatus + '&PrimaryCarrier=' + PrimaryCarrier, null, function (data) {
                    console.log(data);
                    var jsonData = $.parseJSON(data);
                    if (top.window.document.getElementById("ctl00_C5POBody_lblRAF") != undefined) {
                        if (jsonData.length != 0) {
                            for (var i = 0; i < jsonData.length; i++) {
                                if (jsonData[i] != "" && jsonData[i].indexOf('HPN') <= -1)
                                    if (jsonData[i].split(":")[1] != undefined && jsonData[i].split(":")[1].trim() == "") {

                                        RAF_Score += jsonData[i].split(":")[0] + " : " + "NA" + "<br/>";
                                    }

                                    else
                                        RAF_Score += jsonData[i] + "<br/>";
                            }
                            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = RAF_Score.replace("RAF Score :", "").replace("/n", "");
                            if (RAF_Score.length != 0)
                                top.window.document.getElementById("RAF_tooltp").innerText = RAF_Score.replace(regex, "\n") + "\n";
                            else
                                top.window.document.getElementById("RAF_tooltp").innerText = "";
                        }
                        else {
                            top.window.document.getElementById("ctl00_C5POBody_lblRAF").innerHTML = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                            top.window.document.getElementById("RAF_tooltp").innerText = (new Date).getFullYear() + ": " + RAF_Score + "<br/>" + (new Date).getFullYear() - 1 + ": " + RAF_Score_Year + "<br/>" + "HPN" + ": " + Score;
                        }
                    }
                });


            },
            error: function OnError(xhr) {
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

    $scope.CheckAllChronic = function () {

        var chkCheckChronic = $('#chkChronic');
        var table = $('#tblCurrICDs');
        table.find('tr').each(function (rowIndex, r) {
            if (rowIndex != 0) {
                var cols = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (colIndex == 21)
                        c.childNodes[0].textContent = "Y";
                    if (c.textContent == "") {
                        if (chkCheckChronic[0].checked && colIndex == 2)
                            c.childNodes[0].checked = true;
                        else if (colIndex == 2)
                            c.childNodes[0].checked = false;

                    }

                });

            }
        });

        $scope.SaveEnableDisable(false);

    }
    $scope.CheckAllProblem = function () {

        var chkCheckProblem = $('#chkProblem');
        var table = $('#tblCurrICDs');
        table.find('tr').each(function (rowIndex, r) {
            if (rowIndex != 0) {
                var cols = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (colIndex == 21)
                        c.childNodes[0].textContent = "Y";
                    if (c.textContent == "") {
                        if (chkCheckProblem[0].checked && colIndex == 3)
                            c.childNodes[0].checked = true;
                        else if (colIndex == 3)
                            c.childNodes[0].checked = false;

                    }
                });

            }
        });

        $scope.SaveEnableDisable(false);

    }
    $scope.ClearAllAssessmentTable = function () {

        if (DisplayErrorMessage('220011') == true) {
            //Jira - #CAP-80
            //if (sessionStorage.getItem("Projname") != undefined && sessionStorage.getItem("Projname").toUpperCase() != "WISH") {
            if (localStorage.getItem("Projname") != undefined && localStorage.getItem("Projname").toUpperCase() != "WISH") {
                $('select').prop('selectedIndex', 0);
            }
            $('textarea').val('');
            $('input:checkbox').attr('checked', false);
            $('input:radio').attr('checked', false);

            $('input:text').val('');
            $scope.SaveEnableDisable(false);
            var no_rows = $('select').length;
            for (var i = 0; i < no_rows; i++) {
                $('select')[i].parentNode.parentNode.children[21].innerText = "Y";
                var options = $('select')[i].options;
                var default_val = $('select')[i].parentNode.parentNode.cells[22].innerText;
                for (var j = 0; j < options.length; j++) {
                    if ($('select')[i].options[j].innerText == default_val)
                        $($('select')[i])[0].selectedIndex = j;
                }
            }
        }
    }

    $scope.SaveEnableDisable = function (bSave) {

        $('#btnSave')[0].disabled = bSave;

        if (!bSave) {
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
        else {
            localStorage.setItem("bSave", "true");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;

        }
    }

    $scope.SaveEnable = function () {
        $('#btnSave')[0].disabled = false;
        localStorage.setItem("bSave", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }

    $scope.OpenCalculatorLink = function () {
        $('#divCalculator').modal({ backdrop: 'static', keyboard: false }, 'show');
    }

    $scope.CopyPrevious = function () {

        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" &&
            localStorage.getItem("bSave") == "false") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //Cap - 1022, 1626
           // var isSaved = $scope.SaveAssessment();
            //$scope.SaveEnableDisable(true);
            //if (isSaved != "false") {
                $scope.CopyPreviousEncounter();
            //}
            //else { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //return;


            //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogAssessment');
            //$(dvdialog).dialog({
            //    modal: true,
            //    title: "Capella -EHR",
            //    position: {
            //        my: 'left' + " " + 'center',
            //        at: 'center' + " " + 'center + 100px'

            //    },
            //    buttons: {
            //        "Yes": function () {
            //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //            var isSaved = $scope.SaveAssessment();
            //            $scope.SaveEnableDisable(true);
            //            $(dvdialog).dialog("close");
            //            if (isSaved != "false") {
            //                $scope.CopyPreviousEncounter();
            //            }
            //            else { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //            return;
            //        },
            //        "No": function () {
            //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //            $scope.SaveEnableDisable(true);
            //            $(dvdialog).dialog("close");
            //            $scope.CopyPreviousEncounter();
            //        },
            //        "Cancel": function () {
            //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            //            $(dvdialog).dialog("close");
            //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //            return;
            //        }
            //    }
            //});
        }
        else {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            $scope.CopyPreviousEncounter();
        }
    }
    $(top.window.document).find("#btnAssessment").click(function () {
        $scope.btnMoveToAssessmentClick();
        return false;
    });

    $scope.btnMoveToAssessmentClick = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        bAssoICDOpen = false;
        var ICDSplitList;
        //Jira - #CAP-80
        //if (sessionStorage.getItem("PotentialICDList") != null) {
        if (localStorage.getItem("PotentialICDList") != null) {
            //Jira - #CAP-80
            //var ICDList = sessionStorage.getItem("PotentialICDList");
            var ICDList = localStorage.getItem("PotentialICDList");
            if (ICDList.indexOf(',|') > -1) {
                ICDSplitList = ICDList.split(",|");
            }
            else {
                ICDSplitList = ICDList;
            }

            $http({
                url: "frmAssessmentNew.aspx/GetPotentialICDs",
                dataType: 'json',
                method: 'POST',
                data: '{sFormviewICD: "' + ICDList + '" }',
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                }

            }).success(function (response, status, headers, config) {
                if (response.d != "") {
                    var test = JSON.parse(response.d);

                    if (test.NoQuestionnaire.length > 0) {
                        for (var i = 0; i < test.NoQuestionnaire.length; i++) {
                            if (JSON.stringify($scope.AssessmentTable).indexOf(test.NoQuestionnaire[i].ICDCode) == -1) {
                                $scope.AssessmentTable.push(test.NoQuestionnaire[i]);
                                $("textarea").unbind();
                                iRightClickMenuCheck = false;
                            }
                        }
                        $scope.SaveEnableDisable(false);
                        $scope.ColorCoding();

                    }
                    $(top.window.document).find('#divPotential').modal('hide');
                    if (test.AllICDQuestionnaire != undefined) {
                        if (test.AllICDQuestionnaire.length > 0) {
                            $scope.OpenPotentialQuestionnaire(test.AllICDQuestionnaire);
                        }
                    }

                    if (test.AssoICDQuestionnaire != undefined) {
                        if (test.AssoICDQuestionnaire.length > 0) {
                            AssoICDQuestionnaire = test.AssoICDQuestionnaire;
                            if (test.AllICDQuestionnaire.length == 0) {
                                {
                                    $scope.OpenAssICDQuestionnaire();
                                }
                            }
                        }
                    }
                    if (test.MultiICD10Tool.length > 0) {
                        var MultiConvList = test.MultiICD10Tool;
                        $('#divtblContainer').empty();
                        for (var j = 0; j < MultiConvList.length; j++) {

                            var details = document.createElement('DETAILS');
                            var summary = document.createElement('SUMMARY');
                            summary.innerHTML = MultiConvList[j].split('^')[0].split('~')[0] + "-" + MultiConvList[j].split('^')[0].split('~')[1];
                            $(details).append(summary);
                            var table = '<table class="table table-bordered table-condensed" style="margin-bottom:0px;">';
                            var ICD_10lst = MultiConvList[j].split('^')[1].split('|');
                            for (var i = 0; i < ICD_10lst.length - 1; i++) {
                                ICD_10lst[i] = ICD_10lst[i].replace('-', '').replace(',', '');
                                table += '<tr><td class="chkbx"><input class="' + MultiConvList[j].split('^')[0].split('~')[0].replace('.', '_') + '" type="checkbox" /></td><td class="ValueICD">' + ICD_10lst[i].split('-')[0] + '</td><td class="DescpICD">' + ICD_10lst[i].split('-')[2] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[2] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[4] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[6] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[5] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[7] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[0] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[1] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[8] + '</td><td class="ICDValue">' + MultiConvList[j].split('^')[0].split('~')[9] + '</td></tr>';
                            }
                            table += '</table>';
                            $("#divtblContainer").append(details);
                            $(details).attr('open', '');
                            $(details).append(table);
                        }
                        $('#divdetails').modal({ backdrop: 'static', keyboard: false }, 'show');
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }).error(function (error, status, headers, config) {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else
                    window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                //alert(error.Message + ".Please Contact Support!");
            });
        }
    }

    $(top.window.document).find("#ClosePotentialModal").click(function () {
        $scope.ClosePotentialModalClick();
        return false;
    });

    $scope.ClosePotentialModalClick = function () {

        if ($(top.window.document).find("#btnAssessment")[0].disabled == false) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(top.window.document).find("#divPotential").modal({ backdrop: 'static', keyboard: false }, 'show');
            $scope.btnMoveToAssessmentClick();
            return false;
            //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogAssessment');
            //$(dvdialog).dialog({
            //    modal: true,
            //    title: "Capella EHR",
            //    position: {
            //        my: 'left' + " " + 'center',
            //        at: 'center' + " " + 'center + 100px'
            //    },
            //    buttons: {
            //        "Yes": function () {
            //            $(dvdialog).dialog("close");
            //            $scope.btnMoveToAssessmentClick();
            //            return false;
            //        },
            //        "No": function () {
            //            $(dvdialog).dialog("close");
            //            $(top.window.document).find('#divPotential').modal('hide');
            //        },
            //        "Cancel": function () {
            //            $(dvdialog).dialog("close");
            //        }
            //    }
            //});
        }
        else {
            $(top.window.document).find('#divPotential').modal('hide');
            return false;
        }


    }

    $scope.btnPotentailDiagnosis_Click = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $(top.window.document).find("#divPotential").modal({ backdrop: 'static', keyboard: false }, 'show');
        //Jira - #CAP-80
        //$(top.window.document).find("#PotentialFrame")[0].contentDocument.location.href = "htmlPotentialDiagnosis.html?version=" + sessionStorage.getItem("ScriptVersion") + "&Screen=ASSESSMENT";
        $(top.window.document).find("#PotentialFrame")[0].contentDocument.location.href = "htmlPotentialDiagnosis.html?version=" + localStorage.getItem("ScriptVersion") + "&Screen=ASSESSMENT";
    }

    $scope.OpenAssICDQuestionnaire = function () {
        if (AssoICDQuestionnaire != undefined) {
            if (AssoICDQuestionnaire.length > 0) {

                bAssoICDOpen = true;
                bSetICD9Code = true;
                bProblmCheck = true;
                $scope.Questionnaire = AssoICDQuestionnaire;
                if (AssoICDQuestionnaire[0].MutuallyExclusive == "Y")
                    $scope.ColumnName = singlecolumn.ColumnName;
                else
                    $scope.ColumnName = Multiplecolumn.ColumnName;

                sSingleICD = "";
                sSelectedICD = AssoICDQuestionnaire[0].ICDCodeDesc.split('-')[0] + "~" + AssoICDQuestionnaire[0].MainICDDesc + "|" + "Y";
                sSingleICD = AssoICDQuestionnaire[0].ICDCodeDesc.split('-')[0];
                var labelText = "Please select the Diagnosis related to:";
                $('#lblQuestionnaire').html(labelText + "" + AssoICDQuestionnaire[0].ICDCodeDesc.split('-')[0] + "-" + AssoICDQuestionnaire[0].MainICDDesc + "--->");
                QuestionnaireArray.push(AssoICDQuestionnaire);// = str;
                if ($.isReady) {
                    $('#divMultipleICD9ModalQuestionnaire').modal({ backdrop: 'static', keyboard: false }, 'show');
                }
            }
        }
    }

    $scope.OpenPotentialQuestionnaire = function (AllICDList) {
        var iCol = 0;
        if (AllICDList != null) {
            var table = AllICDList;
            var dataChecked = "";
            if (!sMainlevelCheck) {
                for (var iSplit = 0; iSplit < table.length; iSplit++) {
                    if (dataChecked == "") {
                        dataChecked = AllICDList[iSplit].ICDCode;
                        sLabelText = AllICDList[iSplit].ICDCode + "-" + AllICDList[iSplit].MainICDDesc;
                    }
                    else {
                        dataChecked += "~" + AllICDList[iSplit].ICDCode;
                        sLabelText += "~" + AllICDList[iSplit].ICDCode + "-" + AllICDList[iSplit].MainICDDesc;
                    }
                }
                if (dataChecked.indexOf('~') > -1)
                    sMainlevelQuestionnaire = dataChecked.replace(dataChecked.split('~')[0].trim() + "~", "");
                dataChecked = dataChecked.split('~')[0].trim();

                var labelText = $("#lblQuestionnaire").text();
                $('#lblQuestionnaire').html(labelText + "" + sLabelText.split('~')[0] + "--->");

                sLabelText = sLabelText.replace(sLabelText.split('~')[0] + "~", "");

            }
            else {
                sMainlevelCheck = false;
                dataChecked = sMainlevelQuestionnaire.split('~')[0];
                sMainlevelQuestionnaire = sMainlevelQuestionnaire.replace(sMainlevelQuestionnaire.split('~')[0] + "~", "").replace(sMainlevelQuestionnaire.split('~')[0], "");
                $('#lblQuestionnaire').html("Please select the Diagnosis related to:" + sLabelText.split('~')[0] + "--->");

                sLabelText = sLabelText.replace(sLabelText.split('~')[0] + "~", "");
            }
        }
        if (dataChecked.trim() == "") {
            DisplayErrorMessage('220020');
            return;
        }
        else {

            $http({
                url: "frmAssessmentNew.aspx/SearchQuestionnaire",
                dataType: 'json',
                method: 'POST',
                data: '{sICD: "' + dataChecked + '" }',
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                }
            }).success(function (response) {
                var str = response.d;
                bSetICD9Code = false;
                var test = JSON.parse(str);
                $scope.Questionnaire = test.Questionnaire;
                if (test.Questionnaire[0].MutuallyExclusive == "Y")
                    $scope.ColumnName = singlecolumn.ColumnName;
                else
                    $scope.ColumnName = Multiplecolumn.ColumnName;
                QuestionnaireArray = [];
                QuestionnaireArray.push(test.Questionnaire);// = str;
                if ($.isReady) {
                    $('#divMultipleICD9ModalQuestionnaire').modal({ backdrop: 'static', keyboard: false }, 'show');
                }
            }).error(function (error, status, headers, config) {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else
                    window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                //alert(error.Message + ".Please Contact Support!");
            });
        }


    }
    //Cap - 1566
    $scope.btnProblemList_Click = function (event) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        $http({
            url: "frmAssessmentNew.aspx/LoadProblemList",
            dataType: 'json',
            method: 'POST',
            data: '{strAssessment: "Load" }',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response, status, headers, config) {
            var str = response.d;
            var test12 = JSON.parse(str);          
            let AssList = [];
            var bCheck = true;
            if (test12.AssessmentList == '220026') {
                DisplayErrorMessage('220026');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            if (test12.AssessmentList == '220027') {
                DisplayErrorMessage('220027');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            var icd = test12.AssessmentList;
            //CAP-2128
            //$scope.FromProblemList = test12.FromProblemList;

            $.each(test12.FromProblemList, function (index, item) {
                var isExists = $scope.FromProblemList.some(function (x) { return x.ICDCODE == item.ICDCODE; });
                if (!isExists) {
                    $scope.FromProblemList.push(item);
                }
            });
            var oldicd = $scope.AssessmentTable;
            for (i = 0; i < icd.length; i++) {
                for (j = 0; j < oldicd.length; j++) {
                    if (icd[i].ICDCode.trim() === oldicd[j].ICDCode.trim()) {
                        bCheck = false;
                        break;
                    }
                    else {
                        bCheck = true;
                    }
                }
                if (bCheck== true)
                AssList.push(icd[i]);
            }
            var AssList1 = $scope.AssessmentTable.concat(AssList);
            $scope.AssessmentTable = AssList1;

            $scope.orderByField = ['-IsPrimary', 'ICDCode'];

            $scope.reverseSort = false;
            //Cap - 1624
            $('#btnSave')[0].disabled = false;
            //Cap - 1627
            localStorage.setItem("bSave", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        })
            .error(function (error, status, headers, config) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    window.location = "ErrorPage.aspx?Message=" + error.Message + "|$|" + error.StackTrace;
                }

                // alert(error.Message + ".Please Contact Support!");
            });

    }

});
function removeDuplicates(inputArray) {
    var i;
    var len = inputArray.length;
    var outputArray = [];
    var temp = {};

    for (i = 0; i < len; i++) {
        temp[inputArray[i]] = 0;
    }
    for (i in temp) {
        outputArray.push(i);
    }
    return outputArray;
}