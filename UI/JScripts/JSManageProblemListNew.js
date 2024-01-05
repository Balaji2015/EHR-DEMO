//If any changes are made in this file increase value of version in the query string in all files referencing this javascript file
//Only for AngularJS scripts

$(document).ready(function () {

    $('#chkShowActiveOnly')[0].checked = true;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";

});
var iLoadCount = 0;
var toload = 0;

$(top.window.document).find("#btnClose")[0].hidden = true;


jQuery.ui.autocomplete.prototype._resizeMenu = function () {
    var ul = this.menu.element;
    ul.outerWidth(this.element.outerWidth());
}
var DeleteArray = [];
var iCountBack = 0;
var QuestionnaireArray = [];
var ICDCheckNode = "";
var sMutilSelectICD = "";
var sMulti = 0;
var bcolorcoding = false;
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
var sSingleICD = "";
var iCheckSubLevelQuestionnaire = -1;
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
var sAssociatedSelectedICD = "";
var bSetICD9Code = false;
var saved = 0;
var isclosemodal = 0;
var isload = 0;
var loadicdconv = 0;
function validate(data) {
    var cookieList = document.cookie.split(';');
    var DOB;
    for (var l = 0; l < cookieList.length; l++) {
        if (cookieList[l].indexOf("Human_Details") > -1)
            DOB = cookieList[l].split("=")[1].split('|')[3].split(':')[1].trim();
    }
    var date = new Date();
    var currDate = date.toDateString().split(' ')[2] + "-" + date.toDateString().split(' ')[1] + "-" + date.toDateString().split(' ')[3];
    for (var j = 0; j < data.length; j++) {
        var obj = data[j];
        var Y = obj[5];
        var M = obj[6];
        var D = obj[7];

        var YR = obj[8];
        var MR = obj[9];
        var DR = obj[10];

        if (Y.indexOf('?') != -1)
            Y = "";
        if (M.indexOf('?') != -1)
            M = "";
        if (D.indexOf('?') != -1)
            D = "";
        if (YR.indexOf('?') != -1)
            YR = "";
        if (MR.indexOf('?') != -1)
            MR = "";
        if (DR.indexOf('?') != -1)
            DR = "";

        if ((Y == "" && D != "" && M !== "") || (Y == "" && M == "" && D != "") || (Y == "" && D == "" && M != "") || (Y != "" && D != "" && M == "")) {
            DisplayErrorMessage('180006');
            return 0;
        }
        else if ((YR == "" && DR != "" && MR !== "") || (YR == "" && MR == "" && DR != "") || (YR == "" && DR == "" && MR != "") || (YR != "" && DR != "" && MR == "")) {
            DisplayErrorMessage('180006');
            return 0;
        }
        else {
            if (Y != "" && M != "" && D != "")
                var Diag_Date = D + "-" + M + "-" + Y;
            else if (Y !== "" && M != "" && D == "")
                var Diag_Date = M + "-" + Y;
            else
                var Diag_Date = Y;
            if (YR != "" && MR != "" && DR != "")
                var rDiag_Date = DR + "-" + MR + "-" + YR;
            else if (YR !== "" && MR != "" && DR == "")
                var rDiag_Date = MR + "-" + YR;
            else
                var rDiag_Date = YR;

            var cur_Date = Date.parse(currDate);
            var dob = Date.parse(DOB);
            var diag_Date = Date.parse(Diag_Date);
            var rdiag_Date = Date.parse(rDiag_Date);

            if (dob > diag_Date) {
                DisplayErrorMessage('182215');
                return 0;
            }
            if (diag_Date > cur_Date) {
                DisplayErrorMessage('182218');
                return 0;
            }
            if (rdiag_Date > cur_Date) {
                DisplayErrorMessage('182227');
                return 0;
            }
        }
    }

    return 1;
}

function setSaveDisabled() {
    $('#btnSave')[0].disabled = true;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    localStorage.setItem("bSave", "true");
}

function SetIndexChanged(statusdrpDown) {
    SetSaveEnabled();
    if ($(statusdrpDown)[0].value == "Active") {
        document.getElementById("chkNoKnownActiveProblem").checked = false;
    }
    if ($(statusdrpDown)[0].value == "Resolved") {
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EndYear")[0].disabled = false;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EndMonth")[0].disabled = false;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EnddateDrpDwn")[0].disabled = false;

    }
    else {
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EndYear")[0].disabled = true;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EndMonth")[0].disabled = true;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EnddateDrpDwn")[0].disabled = true;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EndYear")[0].selectedIndex = 0;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EndMonth")[0].selectedIndex = 0;
        $($(statusdrpDown)[0].parentElement.parentElement).find("#EnddateDrpDwn")[0].selectedIndex = 0;
    }
}

function SetSaveEnabled() {
    $('#btnSave')[0].disabled = false;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
}

function SetStatus() {
    var bActive = false;
    if (document.getElementById("chkNoKnownActiveProblem").checked == true) {
        var table = $('#tblProblemList');
        var data = [];
        if (table != undefined) {
            table.find('tr').each(function (rowIndex, r) {
                if (rowIndex != 0) {
                    var cols = [];
                    $(this).find('td').each(function (colIndex, c) {
                        if (colIndex == 4) {
                            var select = c.childNodes;
                            for (var i = 0; i < select.length; i++) {
                                if ($(select)[i].value != undefined)
                                    var Slctstatus = $(select)[i].value;
                            }
                            if (Slctstatus == "Active") {
                                document.getElementById("chkNoKnownActiveProblem").checked = false;
                                bActive = true;
                            }
                        }
                    });
                }
            });
            if (bActive == true)
                DisplayErrorMessage('220018');
        }
        if (bActive == false) {
            SetSaveEnabled();
        }
    }
}

var ProblemApp = angular.module('AppManageProblem', []);
ProblemApp.config(function ($provide) {
    $provide.decorator('$exceptionHandler', function ($delegate) {
        return function (exception, cause) {
            HandlerAngularjsError(exception);
        };
    });
});
ProblemApp.controller('ControllerManageProblem', function ($scope, $http) {

    var yrlst = [];
    for (var i = new Date().getFullYear() ; i >= 1900; i--) {
        yrlst.push(i);
    }
    var datelst = [];
    for (var i = 1; i <= 31; i++) {
        datelst.push(i);
    }

    $http({
        url: "WebServices/ManagePrblmListService.asmx/LoadManageProblemList",
        dataType: 'json',
        method: 'POST',
        data: JSON.stringify({ data: "true" }),
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "X-Requested-With": "XMLHttpRequest"
        }
    }).success(function (response, status, headers, config) {
        var Icdcode = [];
        var str = response.d;
        var test12 = JSON.parse(str);
        $scope.items = yrlst;
        $scope.dates = datelst;

        var unsavedDataPresent = test12.UnsavedData;
        $scope.ProblemListTable = test12.ICDList;
        test = test12.ICD10Tool;
        if ($scope.ProblemListTable.length > 0) {
            for (var i = 0; i < $scope.ProblemListTable.length; i++) {
                if ($scope.ProblemListTable[i].ICDCode == "0000" && $scope.ProblemListTable[i].isActive == "Y") {
                    if ($scope.ProblemListTable.length == 1)
                        $("#chkNoKnownActiveProblem")[0].checked = true;
                    else {
                        $("#chkNoKnownActiveProblem")[0].checked = false;
                        $scope.ProblemListTable.splice(i, 1);
                    }
                }
            }
        }
        if (unsavedDataPresent == true)
            $scope.SaveEnableDisable(false);
        else
            if (unsavedDataPresent == false)
                $scope.SaveEnableDisable(true);
        $scope.orderByField = 'ICDCode';
        $scope.ColorCoding();
        $scope.reverseSort = false;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    })
            .error(function (error, status, headers, config) {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else
                    alert(error.Message + ".Please Contact Support!");
            });
    function load() {

        if (saved == 1) {
            loadprblmlist();
            saved = 0;
        }
    }
    function close() {
        if (saved == 1) {
            $(top.window.document).find("#btnClose").click();
            self.close();
            saved = 0;
            redirectToCC();
        }
    }
    function redirectToCC() {
        $($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active")).removeClass("active");
        $($($(top.window.document).find("iframe")[0].contentDocument).find('#myTabs li.active')).removeClass("active");
        $($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane")[0]).addClass("active");
        $($($(top.window.document).find("iframe")[0].contentDocument).find('#myTabs li:eq(0)')).addClass("active");
    }
    function autoSave(val) {
        if ($("#btnSave")[0].disabled == false) {

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
                            isload = 1;
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
                        setSaveDisabled();
                        if (val == 0) {
                            loadprblmlist();
                        }
                        if (val == 1) {
                            $(top.window.document).find("#btnClose").click();
                            self.close();
                            redirectToCC();
                        }

                    },
                    "Cancel": function () {
                        SetSaveEnabled();
                        $(dvdialog).dialog("close");
                    }
                }
            });
        }
        else
            loadprblmlist();

    }
    $scope.CloseModal = function () {
        if (($("#btnSave")[0].disabled == false) && (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true")) {
            event.preventDefault();
            autoSave(1);
        }
        else {
            $(top.window.document).find("#btnClose").click();
            self.close();
            redirectToCC();
        }
        return 1;
    }
    $scope.LoadFavouriteICDS = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var arrListICDs = [];
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "frmAssessmentNew.aspx/GetFavouriteICDS",
            data: '',
            dataType: "json",
            success: function (data) {
                var jsonData = $.parseJSON(data.d);
                for (var i = 0; i < jsonData.length; i++) {
                    var arricd = jsonData[i];
                    arrListICDs.push(arricd.ICD10Code + "~" + arricd.ICD10Desc + "~" + arricd.Category)
                }
                localStorage.setItem("PhysicianICD", JSON.stringify(arrListICDs));
                $scope.FillFavorite();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }

        });
    }
    $scope.ClearAllLocalVariables = function () {

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

        $('#lblQuestionnaire').html("Please select the Diagnosis related to:");
    }

    $scope.OpenFormView = function () {

        if (localStorage.getItem("PhysicianICD") == "")
            $scope.LoadFavouriteICDS();
        else
            $scope.FillFavorite();

    }
    $scope.FillFavorite = function () {
        bClickcount = false;
        $(top.window.document).find("#tbFavICDsContainer #dynTr").remove();

        $(top.window.document).find('#ok')[0].disabled = true;
        $(top.window.document).find('#divFormView').modal({ backdrop: 'static', keyboard: false }, 'show');//.find('#ok').unbind().click(function (e) {

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
                            cell1.append("<input type='checkbox' class='icd'  id='" + id + "'/>");
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
                                        c.innerHTML = "<input type='checkbox' class='icd'  id='" + id + "'/>";
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
                                        c.innerHTML = "<input type='checkbox' class='icd'  id='" + id + "'/>";
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
        $(top.window.document).find("#tbFavICDsContainer .icd").change(function () {
            $(top.window.document).find('#ok')[0].disabled = false;
        });
    }
    $(top.window.document).find("#ok").click(function () {

        $scope.okClick();
        return false;
    });

    $(top.window.document).find("#cancel").click(function () {

        $scope.cancelClick();
        return false;
    });
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

    $scope.cancelClick = function () {
        if ($(top.window.document).find("#tbFavICDsContainer input:checked ").length > 0) {
            var ShowAlert = DisplayErrorMessage('220019');
            if (ShowAlert == true) {
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
                if (cells[0].childNodes.length != 0) {
                    if (cells[0].childNodes[0].checked) {
                        if (sResultICDs == "")
                            sResultICDs += cells[1].innerText + "~" + cells[2].innerText;
                        else
                            sResultICDs += "|" + cells[1].innerText + "~" + cells[2].innerText;
                    }
                }
            }
            if (cells[3].textContent == "" && cells[3].childNodes[0] != undefined) {
                if (cells[3].childNodes.length != 0) {
                    if (cells[3].childNodes[0].checked) {
                        {
                            if (sResultICDs == "")
                                sResultICDs += cells[4].innerText + "~" + cells[5].innerText;
                            else
                                sResultICDs += "|" + cells[4].innerText + "~" + cells[5].innerText;
                        }
                    }
                }
            }

            if (cells[6].textContent == "" && cells[6].childNodes[0] != undefined) {
                if (cells[6].childNodes.length != 0) {
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
        }

        if (sResultICDs == "") {
            DisplayErrorMessage('220004');
            return;
            //return false;
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
                        if (JSON.stringify($scope.ProblemListTable).indexOf(test.NoQuestionnaire[i].ICDCode) == -1)
                            $scope.ProblemListTable.push({ 'ICDCode': test.NoQuestionnaire[i].ICDCode, 'ICDDescription': test.NoQuestionnaire[i].ICDDescription, 'ProblemListID': test.NoQuestionnaire[i].ProblemListID, 'iProblemListVersion': test.NoQuestionnaire[i].iProblemListVersion, 'Version_Yr': "ICD_10" });
                        $scope.SaveEnableDisable(false);
                    }

                    $scope.ColorCoding();
                }

                $(top.window.document).find("#tbFavICDsContainer #dynTr").remove();

                $(top.window.document).find('#divFormView').modal('hide');


                if (test.Questionnaire != undefined && test.Questionnaire.length > 0) {
                    $scope.Questionnaire = test.Questionnaire;
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
                    QuestionnaireArray = response.d;
                    if (test.Questionnaire[0].MutuallyExclusive == "Y")
                        $scope.ColumnName = singlecolumn.ColumnName;
                    else
                        $scope.ColumnName = Multiplecolumn.ColumnName;

                    var labelText = $("#lblQuestionnaire").text();
                    $('#lblQuestionnaire').html(labelText + "" + test.Questionnaire[0].ICDCodeDesc + "--->");
                    bSetICD9Code = true;
                    $('#divMultipleICD9ModalQuestionnaire').modal('show');


                }

            }

        })
 .error(function (error, status, headers, config) {

     { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
     if (status == 999)
         window.location = "frmSessionExpired.aspx";
     else
         alert(error.Message + ".Please Contact Support!");
 });


        return false;
    }
    $scope.ShowActivChkd = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        autoSave(0);

    }
    function loadprblmlist() {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var value;

        if (document.getElementById("chkShowActiveOnly").checked == true)
            value = "true";
        else
            value = "false";

        $http({
            url: "WebServices/ManagePrblmListService.asmx/LoadManageProblemList",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({ data: value }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response, status, headers, config) {
            var Icdcode = [];
            var str = response.d;
            var test12 = JSON.parse(str);
            test = test12.ICD10Tool;

            var unsavedDataPresent = test12.UnsavedData;
            $scope.ProblemListTable = test12.ICDList;
            if ($scope.ProblemListTable.length > 0) {
                for (var i = 0; i < $scope.ProblemListTable.length; i++) {
                    if ($scope.ProblemListTable[i].ICDCode == "0000" && $scope.ProblemListTable[i].isActive == "Y") {
                        if ($scope.ProblemListTable.length == 1)
                            $("#chkNoKnownActiveProblem")[0].checked = true;
                        else {
                            $("#chkNoKnownActiveProblem")[0].checked = false;
                            $scope.ProblemListTable.splice(i, 1);
                        }
                    }
                }
            }
            $scope.orderByField = 'ICDCode';
            $scope.ColorCoding();
            $scope.reverseSort = false;
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (unsavedDataPresent == true)
                $scope.SaveEnableDisable(false);
            else
                if (unsavedDataPresent == false)
                    $scope.SaveEnableDisable(true);

            if (toload == 2) {
                IcdConversion();
            }
        })
      .error(function (error, status, headers, config) {

          { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
          if (status == 999)
              window.location = "frmSessionExpired.aspx";
          else
              alert(error.Message + ".Please Contact Support!");
      });

    }
    $scope.LoadICD10toProblemLIstTable = function () {


        if (bICD9CodeCheck) {
            bICD9CodeCheck = false;
            sSelectedICD = "";
            var Selected = $('#divtblContainer').find("input:checked");
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
                            for (var j = 0; j < $scope.ProblemListTable.length; j++) {
                                if ($scope.ProblemListTable[i].ICDCode == test.NoQuestionnaire[i].ICDCode)
                                    $scope.ProblemListTable.remove($scope.ProblemListTable[i]);

                            }
                        }

                        for (var i = 0; i < test.NoQuestionnaire.length; i++) {
                            if (JSON.stringify($scope.ProblemListTable).indexOf(test.NoQuestionnaire[i].ICDCode) == -1) {
                                $scope.ProblemListTable.push(test.NoQuestionnaire[i]);
                                $scope.SaveEnableDisable(false);
                            }
                        }
                        $scope.ColorCoding();

                    }




                    sAssociatedSelectedICD = $(Selected[0]).parent().next()[0].innerText + "~" + $(Selected[0]).parent().next().next()[0].innerText + "|" + "Y";



                    $('#divdetails').modal('hide');

                    if (test.Questionnaire != undefined && test.Questionnaire.length > 0) {
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
                        QuestionnaireArray = response.d;
                        if (test.Questionnaire[0].MutuallyExclusive == "Y")
                            $scope.ColumnName = singlecolumn.ColumnName;
                        else
                            $scope.ColumnName = Multiplecolumn.ColumnName;

                        var labelText = $("#lblQuestionnaire").text();
                        $('#lblQuestionnaire').html(labelText + "" + test.Questionnaire[0].ICDCodeDesc + "--->");

                        $('#divMultipleICD9ModalQuestionnaire').modal('show');


                    }

                }

            })
  .error(function (error, status, headers, config) {

      { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
      if (status == 999)
          window.location = "frmSessionExpired.aspx";
      else
          alert(error.Message + ".Please Contact Support!");
  });



        }

        else {
            var selIds = [];
            var ICD_Cnvrsns = $('#divtblContainer').find("details");
            for (var k = 0; k < ICD_Cnvrsns.length; k++) {
                var Selected = $(ICD_Cnvrsns[k]).find("input:checked");
                var ICD_Details = $('#divtblContainer').find("details");

                var sSelectedICD = "";
                var SelectedObj = [];
                var SelectedId;
                var SelectedVersion;
                for (var i = 0; i < Selected.length; i++) {
                    if (JSON.stringify($scope.ProblemListTable).indexOf($(Selected[i]).parent().next()[0].innerText) == -1) {
                        if (i == 0)
                            $scope.ProblemListTable.push({ 'ICDCode': $(Selected[i]).parent().next()[0].innerText, 'ICDDescription': $(Selected[i]).parent().next().next()[0].innerText, 'iProblemListVersion': $(Selected[i]).parent().next().next().next().next()[0].innerText, 'ProblemListID': $(Selected[i]).parent().next().next().next()[0].innerText, 'Version_Yr': "ICD_10", 'Status': $(Selected[i]).parent().next().next().next().next().next()[0].innerText });
                        else
                            $scope.ProblemListTable.push({ 'ICDCode': $(Selected[i]).parent().next()[0].innerText, 'ICDDescription': $(Selected[i]).parent().next().next()[0].innerText, 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10", 'Status': $(Selected[i]).parent().next().next().next().next().next()[0].innerText });

                        $("textarea").unbind();
                        iRightClickMenuCheck = false;


                        $scope.SaveEnableDisable(false);

                        for (var l = test.length - 1; l >= 0; l--) {

                            if (test[l].split('~')[0] == $(Selected[i]).parent().next().next().next().next().next().next().next().next()[0].innerText) {
                                test.splice(l, 1);

                                bICD10Conversioncheck = false;
                            }
                        }
                    }


                }
            }
            $scope.ColorCoding();
            $('#divdetails').modal('hide');
        }
    }

    $scope.ColorCoding = function () {

        var sICDCodes = $scope.ProblemListTable.map(function (item) { return item.ICDCode; });
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


            var test = JSON.parse(response.d);

            var table = $('#tblProblemList');
            table.find('tr').each(function (rowIndex, r) {
                if (rowIndex != 0) {
                    var cols = [];
                    var iColor = "";
                    $(this).find('td').each(function (colIndex, c) {


                        if (colIndex == "1") {
                            if (response.d.indexOf(c.textContent) > -1) {
                                for (var k = 0; k < test.ColorCoding.length; k++) {
                                    if (test.ColorCoding[k].ICDCode == c.textContent) {
                                        c.style.color = test.ColorCoding[k].Color;
                                        iColor = c.style.color;
                                        bcolorcoding = true;
                                    }

                                }

                            }
                            else
                                c.style.color = "Black";
                        }
                        else if (colIndex == "2" || colIndex == "3" || colIndex == "4" || colIndex == "5" || colIndex == "6") {
                            c.style.color = iColor;
                        }


                    });

                }
            });






        })
 .error(function (error, status, headers, config) {

     { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
     if (status == 999)
         window.location = "frmSessionExpired.aspx";
     else
         alert(error.Message + ".Please Contact Support!");
 });

    }

    $scope.SetDateDropDown = function (event) {


        var monthval = $(event)[0].event.currentTarget.parentNode.previousElementSibling.firstElementChild.value;
        var yearval = $(event)[0].event.currentTarget.parentNode.previousElementSibling.previousElementSibling.firstElementChild.value;

        var noofdays;
        var datelist = [];

        if (yearval == "? string: ?" || yearval == "") {
            $($("select #dateDrpDwn option").context.activeElement).empty();
            DisplayErrorMessage('180006');
            return;
        }
        if (monthval == "") {
            $($("select #dateDrpDwn option").context.activeElement).empty();
            DisplayErrorMessage('180006');
            return;
        }

        if (monthval.toUpperCase() == "SEP" || monthval.toUpperCase() == "APR" || monthval.toUpperCase() == "JUN" || monthval.toUpperCase() == "NOV")
            noofdays = 30;
        else
            if (monthval.toUpperCase() == "FEB") {
                if (yearval !== "") {
                    if (yearval % 4 == 0)
                        noofdays = 29;
                    else
                        noofdays = 28;
                }
            }
            else
                noofdays = 31;
        if (noofdays > 0) {
            $($("select #dateDrpDwn option").context.activeElement).empty();
            var option = $('<option />');
            option.attr('value', "").text("");
            $($("select #dateDrpDwn option").context.activeElement).append(option);

            for (var i = 1; i <= noofdays; i++) {
                datelist.push(i);
                var option = $('<option />');
                option.attr('value', i).text(i);
                $($("select #dateDrpDwn option").context.activeElement).append(option);
            }
        }
        SetSaveEnabled();
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
                alert(error.Message + ".Please Contact Support!");
        });
    }


    $("#txtICD10").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmAssessmentNew.aspx/SearchDescrptionText",
                    data: "{\"text\":\"" + document.getElementById("txtICD10").value + "|" + "txtICD10" + "|" + "ICD_10" + "\"}",
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

                        $("#txtICD10").focus();
                        //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
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
            if ($("#txtICD10").val().length > 3) {
                if (arrCPTs.length != 0) {
                    var results = $scope.PossibleCombination(arrCPTs, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item
                        }
                    }));
                }
            }
        },
        minlength: 2,
        multiple: true,
        mustMatch: false,
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

                            if (JSON.stringify($scope.ProblemListTable).indexOf(ui.item.label.split('~')[0]) == -1) {

                                $scope.ProblemListTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10" });

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
                $('#txtICD10').val("");
                bBool = false;
            }



        }
    }).on("paste", function (e) {
        intCPTLength = -1;
        arrCPTs = [];
        $(".ui-autocomplete").hide();
        //CAP-1617
    }).on("keydown", function (e) {
        if (e.which == 8) {
            //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtICD10").val().length <= 3)
                bBool = false;
            $("#txtICD10").focus();
            bcheck = false;
        }
        else {
            //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }

    }).on("input", function (e) {
        document.getElementById('txtDescription').value = "";
        //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtICD10").val().length >= 3) {
            //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //if (!bBool)
            //{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intCPTLength = 0;
        }
        else if ($("#txtICD10").val().length != 0 && intCPTLength != -1) {
            intCPTLength = intCPTLength + 1;
        }
        if ($("#txtICD10").val().length < 3) {
            intCPTLength = -1;
            arrCPTs = [];
            $(".ui-autocomplete").hide();
        }
    });




    $("#txtDescription").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "frmAssessmentNew.aspx/SearchDescrptionText",
                    data: "{\"text\":\"" + document.getElementById("txtDescription").value + "|" + "txtDescription" + "|" + "ICD_10" + "\"}",
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
                        //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
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
            if ($("#txtDescription").val().length > 4) {
                if (arrCPTs.length != 0) {
                    var results = $scope.PossibleCombination(arrCPTs, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item
                        }
                    }));
                }
            }
        },
        minlength: 3,
        multiple: true,
        mustMatch: false,
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

                            if (JSON.stringify($scope.ProblemListTable).indexOf(ui.item.label.split('~')[0]) == -1) {

                                $scope.ProblemListTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10" });

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
                $('#txtICD10').val("");
                bBool = false;
            }



        }
    }).on("paste", function (e) {
        intCPTLength = -1;
        arrCPTs = [];
        $(".ui-autocomplete").hide();
        //CAP-1617
    }).on("keydown", function (e) {
        if (e.which == 8) {
            //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtDescription").val().length <= 4)
                bBool = false;
            $("#txtDescription").focus();
            bcheck = false;
        }
        else {
            //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }

    }).on("input", function (e) {
        document.getElementById('txtICD10').value = "";
        //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtDescription").val().length >= 4) {
            //if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //if (!bBool)
            //{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intCPTLength = 0;
        }
        else if ($("#txtDescription").val().length != 0 && intCPTLength != -1) {
            intCPTLength = intCPTLength + 1;
        }
        if ($("#txtDescription").val().length < 3) {
            intCPTLength = -1;
            arrCPTs = [];
            $(".ui-autocomplete").hide();
        }
    });

    var Indx;
    var iIndex = -1;
    $scope.Delete = function (index) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (index != undefined)
            iIndex = index;
        if (DisplayErrorMessage('220002') == true) {
            var DelList = [];
            var table = $('#tblProblemList');
            var iCheck = iIndex + 1;
            iIndex = -1;
            table.find('tr').each(function (rowIndex, r) {
                if (rowIndex == iCheck) {
                    var colsDel = [];
                    $(this).find('td').each(function (colIndex, c) {
                        if (colIndex == 0) {
                            colsDel.push("Del");
                        }
                        else if (c.textContent == "" && colIndex == "1") {
                            colsDel.push(c.childNodes[0].checked);
                        }
                        else if (colIndex == 4 || colIndex == 5 || colIndex == 6 || colIndex == 7 || colIndex == 8 || colIndex == 9 || colIndex == 10) {
                            var select = c.childNodes;

                            for (var i = 0; i < select.length; i++) {
                                if ($(select)[i].value != undefined)
                                    colsDel.push($(select)[i].value);
                            }

                        }
                        else {
                            colsDel.push(c.textContent);
                        }
                    });
                    DeleteArray.push(colsDel);
                    DelList.push(colsDel);
                }
            });

            for (var i = 0; i < $scope.ProblemListTable.length; i++) {
                if (DelList[0][2] == $scope.ProblemListTable[i].ICDCode) {
                    $scope.ProblemListTable.splice(i, 1);
                }
            }
            $scope.SaveEnableDisable(false);
            $scope.ColorCoding();
        }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    $scope.OpenQuestionnaire = function () {

        var dataChecked = "";

        sMainlevelCheck = false;
        dataChecked = sMainlevelQuestionnaire.split('~')[0];
        sMainlevelQuestionnaire = sMainlevelQuestionnaire.replace(sMainlevelQuestionnaire.split('~')[0] + "~", "").replace(sMainlevelQuestionnaire.split('~')[0], "");
        $('#lblQuestionnaire').html("Please select the Diagnosis related to:" + sLabelText.split('~')[0] + "--->");

        sLabelText = sLabelText.replace(sLabelText.split('~')[0] + "~", "");


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
            })
     .error(function (error, status, headers, config) {

         { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
         if (status == 999)
             window.location = "frmSessionExpired.aspx";
         else
             alert(error.Message + ".Please Contact Support!");
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
                    if (colIndex == 4 && c.textContent != "") {
                        ICDCheckNode = ICDCheckNode.replace(c.textContent, "").replace(c.textContent + ",", "");
                        c.textContent = "";

                    }
                });

            }
        });



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
        else if (sMainlevelQuestionnaire != "")
        { sMainlevelCheck = true; $scope.OpenQuestionnaire(); }

        else {
            sMainlevelCheck = false;
            var AddSelectedICD = sSelectedICD.split('^');
            var iCheckColor = 0;
            for (var i = 0; i < AddSelectedICD.length; i++) {
                if (JSON.stringify($scope.ProblemListTable).indexOf(AddSelectedICD[i].split('-')[0]) == -1) {
                    $scope.ProblemListTable.push({ 'ICDCode': AddSelectedICD[i].split('-')[0], 'ICDDescription': AddSelectedICD[i].split('-')[1].split('|')[0], 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10" });
                    iCheckColor++;
                    $("textarea").unbind();
                    iRightClickMenuCheck = false;
                }
            }
            if (iCheckColor != 0)
                $scope.ColorCoding();
            $('#divMultipleICD9ModalQuestionnaire').modal('hide');
            $scope.ClearAllLocalVariables();
        }

    }
    $scope.OpenBackQuestionnaire = function () {


        QuestionnaireArray = QuestionnaireArray.slice(0, QuestionnaireArray.length - 1);
        if (QuestionnaireArray.length == 0) {

            if (sSingleICD != "") {
                sSelectedICD = "";
                sSelectedICD = "";
            }


            var AddSelectedICD = sSelectedICD.split('^');
            for (var i = 0; i < AddSelectedICD.length; i++) {
                if (JSON.stringify($scope.ProblemListTable).indexOf(AddSelectedICD[i].split('~')[0]) == -1) {
                    $scope.ProblemListTable.push({ 'ICDCode': AddSelectedICD[i].split('~')[0], 'ICDDescription': AddSelectedICD[i].split('~')[1].split('|')[0], 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10" });
                    $("textarea").unbind();
                    iRightClickMenuCheck = false;
                }
            }
            $scope.ColorCoding();
            $('#divMultipleICD9ModalQuestionnaire').modal('hide');
            $scope.ClearAllLocalVariables();
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

                if (test.Questionnaire[0].MutuallyExclusive == "Y")
                    $scope.ColumnName = singlecolumn.ColumnName;
                else
                    $scope.ColumnName = Multiplecolumn.ColumnName;
                QuestionnaireArray.push(test.Questionnaire);  //+= "|" + response.d;
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
                for (var i = 0; i < AddSelectedICD.length; i++) {
                    if (JSON.stringify($scope.ProblemListTable).indexOf(AddSelectedICD[i].split('~')[0]) == -1) {
                        if (bSetICD9Code)
                            $scope.ProblemListTable.push({ 'ICDCode': AddSelectedICD[i].split('~')[0], 'ICDDescription': AddSelectedICD[i].split('~')[1].split('|')[0], 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10" });
                        else
                            $scope.ProblemListTable.push({ 'ICDCode': AddSelectedICD[i].split('~')[0], 'ICDDescription': AddSelectedICD[i].split('~')[1].split('|')[0], 'iProblemListVersion': 0, 'ProblemListID': 0, 'Version_Yr': "ICD_10" });

                        $("textarea").unbind();
                        iRightClickMenuCheck = false;
                        $scope.SaveEnableDisable(false);

                    }
                }
                $scope.ColorCoding();
                bSetICD9Code = false;

                $('#divMultipleICD9ModalQuestionnaire').modal('hide');
                $scope.ClearAllLocalVariables();
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
                alert(error.Message + ".Please Contact Support!");
        });
    }

    $scope.SaveProblemList = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var table = $('#tblProblemList');
        var data = [];
        var bDate = "false";
        table.find('tr').each(function (rowIndex, r) {
            var dYear = '';
            var dMonth = '';
            var dDay = '';
            var rYear = '';
            var rMonth = '';
            var rDay = '';
            var status = '';
            if (rowIndex != 0) {
                var cols = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (c.textContent == "" && colIndex == "1") {
                        cols.push(c.childNodes[0].checked);
                    }
                    else if (colIndex == 4 || colIndex == 5 || colIndex == 6 || colIndex == 7 || colIndex == 8 || colIndex == 9 || colIndex == 10) {
                        var select = c.childNodes;

                        for (var i = 0; i < select.length; i++) {
                            if ($(select)[i].value != undefined) {
                                cols.push($(select)[i].value);
                                if (colIndex == 4)
                                    status = $(select)[i].value;
                                else if (colIndex == 5)
                                    dYear = $(select)[i].value;
                                else if (colIndex == 6)
                                    dMonth = $(select)[i].value;
                                else if (colIndex == 7)
                                    dDay = $(select)[i].value;
                                else if (colIndex == 8)
                                    rYear = $(select)[i].value;
                                else if (colIndex == 9)
                                    rMonth = $(select)[i].value;
                                else if (colIndex == 10)
                                    rDay = $(select)[i].value;
                            }

                        }

                    }
                    else {
                        cols.push(c.textContent);
                    }
                });
                if (status == "Resolved") {
                    var DiagnosedDate = new Date(dDay + '/' + dMonth + '/' + dYear);
                    var ResolvedDate = new Date(rDay + '/' + rMonth + '/' + rYear);
                    if (DiagnosedDate > ResolvedDate) {
                        bDate = "true";
                        return;
                    }
                }

                data.push(cols);
            }
        });
        var val = validate(data);
        if (bDate == "true") {
            val = 0;
            DisplayErrorMessage('220024');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
        for (var i = 0; i < data.length; i++) {
            for (var j = 0; j < DeleteArray.length; j++) {
                if (data[i][2] == DeleteArray[j][2])
                    DeleteArray.splice(j, 1);
            }
        }
        if (DeleteArray.length != 0)
            data = data.concat(DeleteArray);
        if (val == 1) {
            if (bcolorcoding) {

                var iChek = DisplayErrorMessage('220016');
                if (iChek == undefined) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return;
                }

                if (!iChek) {
                    toload = 0;
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return;
                }

            }
            for (var i = 0; i < data.length; i++) {
                if (data[i][1] != "0000")
                    document.getElementById("chkNoKnownActiveProblem").checked = false;
            }

            var NoKnwmActvPblm, chkShowActive;

            var NoKnwmActvPblm = $("#chkNoKnownActiveProblem")[0].checked;
            var chkShowActive = $("#chkShowActiveOnly")[0].checked;
            if (chkShowActive == true)
                chkShowActive = "true";
            else
                chkShowActive = "false";


            if (NoKnwmActvPblm == true)
                NoKnwmActvPblm = "true";
            else
                NoKnwmActvPblm = "false";

            $http({
                url: "WebServices/ManagePrblmListService.asmx/SaveProblemList",
                dataType: 'json',
                method: 'POST',
                data: JSON.stringify({ name: data, NoKnwmActvPblm: NoKnwmActvPblm, chkShowActive: chkShowActive }),
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                }

            }).success(function (response) {
                var Icdcode = [];
                var str = response.d;
                var test = JSON.parse(str);

                $scope.ProblemListTable = test.ProblemList;
                if ($scope.ProblemListTable.length > 0) {
                    for (var i = 0; i < $scope.ProblemListTable.length; i++) {
                        if ($scope.ProblemListTable[i].ICDCode == "0000" && $scope.ProblemListTable[i].isActive == "Y") {
                            if ($scope.ProblemListTable.length == 1)
                                $("#chkNoKnownActiveProblem")[0].checked = true;
                            else {
                                $("#chkNoKnownActiveProblem")[0].checked = false;
                                $scope.ProblemListTable.splice(i, 1);
                            }
                        }
                    }
                }
                var iSpliceCount = -1;
                bcolorcoding = false;
                if (isload == 0) {
                    $scope.ColorCoding();
                }
                DeleteArray = [];
                $scope.SaveEnableDisable(true);
                DisplayErrorMessage('600001');
                saved = 1;

                var ProblemListTooltip = test.ToolTip;
                var regex = /<BR\s*[\/]?>/gi;
                top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = ProblemListTooltip[0];
                top.window.document.getElementById("ProblemList_tooltp").innerText = ProblemListTooltip[1].replace(regex, "\n") + "\n";
                RefreshOverallSummaryTooltip();

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (isclosemodal == 1) {
                    close();
                    isclosemodal = 0;
                }
                if (isload == 1) {
                    load();
                    isload = 0;
                }
                if (isload == 2) {
                    load();
                    isload = 0;
                }

                RefreshNotification('ProblemList');
            })

            .error(function (error, status, headers, config) {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (status == 999)
                    window.location = "frmSessionExpired.aspx";
                else
                    alert(error.Message + ".Please Contact Support!");
            });
        }
        else {
            saved = 0;
            isclosemodal = 0;
            isload = 0;
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }

    }


    function IcdConversion() {
        toload = 0;
        if (!bICD10Conversioncheck) {
            if (test.length == 0) {
                DisplayErrorMessage('220021');
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
                    table += '<tr><td class="chkbx"><input class="' + test[j].split('^')[0].split('~')[0].replace('.', '_') + '" type="checkbox" /></td><td class="ValueICD">' + ICD_10lst[i].split('-')[0] + '</td><td class="DescpICD">' + ICD_10lst[i].split('-')[2] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[2] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[3] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[4] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[5] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[7] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[0] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[1] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[8] + '</td><td class="ICDValue">' + test[j].split('^')[0].split('~')[9] + '</td></tr>';
                }
                table += '</table>';
                $("#divtblContainer").append(details);
                $(details).attr('open', '');
                $(details).append(table);

            }

            $('#divdetails').modal({ backdrop: 'static', keyboard: false }, 'show');

            bICD10Conversioncheck = true;
            bICD9CodeCheck = false;
        }
        else {
            if (test.length == 0) {
                DisplayErrorMessage('220021');
                return;
            }
            else {
                $('#divdetails').modal({ backdrop: 'static', keyboard: false }, 'show');
            }

        }
    }
    $scope.CheckAllHealthConcern = function () {

        var chkHealthConcern = $('#chkHealthConcern');
        var table = $('#tblProblemList');
        table.find('tr').each(function (rowIndex, r) {
            if (rowIndex != 0) {
                var cols = [];
                $(this).find('td').each(function (colIndex, c) {
                    if (c.textContent == "") {
                        if (chkHealthConcern[0].checked && colIndex == 1)
                            c.childNodes[0].checked = true;
                        else if (colIndex == 1)
                            c.childNodes[0].checked = false;

                    }

                });

            }
        });

        $scope.SaveEnableDisable(false);

    }
});
