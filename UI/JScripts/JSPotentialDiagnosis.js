
var intCPTLength = -1;
var bBool = false;
var bColumnHide;
var lstSelectedCPT = new Array();
var InsertList = [];
var UpdateList = [];
var DelList = [];
var bErrorMsg = false;
var ProblemApp = angular.module('AppPotentialDiagnosis', []);
ProblemApp.config(function ($provide) {
    $provide.decorator('$exceptionHandler', function ($delegate) {
        return function (exception, cause) {
            HandlerAngularjsError(exception);
        };
    });
});
ProblemApp.controller('CtrlPotentialDiagnosis', function ($scope, $http) {
    var sData = "";
    if (document.URL.indexOf("ASSESSMENT") == -1) {
        $('#btnSave')[0].style.display = "block";
        $('#btnMoveToAssessment')[0].style.display = "none !important";
    }
    else {
        $('#btnSave')[0].style.display = "none";
        $('#btnMoveToAssessment')[0].style.display = "block !important";
        $('#btnCloseModal')[0].style.display = "none";
        $('#tblPotentialFooter')[0].style.display = "none";
        $('#trSelectParameters')[0].style.display = "none";
        $(top.window.document).find("#btnAssessment")[0].disabled = true;
        if (window.screen.availHeight < 900) {
            $('#PotentialBody')[0].style.height = "330px";
        }
        sData = "true";
        $(top.window.document).find("#chkPDShowAll")[0].checked = false;
    }
    $http({
        url: "WebServices/PotentialDiagnosisService.asmx/LoadPotentialDiagnosis",
        dataType: 'json',
        method: 'POST',
        data: JSON.stringify({ data: sData }),
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "X-Requested-With": "XMLHttpRequest"
        }

    }).success(function (response, status, headers, config) {
        var Icdcode = [];
        var str = response.d;
        var ICDLst = JSON.parse(str);
        $scope.PotentialListTable = ICDLst.ICD10List;
        $scope.EnableColumn = ICDLst.EnableColumn;
        bColumnHide = ICDLst.EnableColumn;
        $scope.PotentialHeadListTable = [{ EnableColumn: bColumnHide }];
        var test = JSON.parse(response.d);

        if (test.ICD10List.length > 0) {
            for (var i = 0; i < test.ICD10List.length; i++) {
                $scope.PotentialListTable[i].ICD10Code = test.ICD10List[i].ICD_Code;
                $scope.PotentialListTable[i].ICD10Description = test.ICD10List[i].ICD_Description;
                $scope.PotentialListTable[i].ID = test.ICD10List[i].Id;
                $scope.PotentialListTable[i].Notes = test.ICD10List[i].Notes.trim();
                $scope.PotentialListTable[i].EnableColumn = bColumnHide;
                $scope.PotentialListTable[i].Version = test.ICD10List[i].Version;
                $scope.PotentialListTable[i].source = test.ICD10List[i].Source;
                $scope.PotentialListTable[i].SelectedValue = test.ICD10List[i].Source;
                $scope.PotentialListTable[i].MoveToAssessment = test.ICD10List[i].Move_To_Assessment;
                $scope.PotentialListTable[i].VersionYear = test.ICD10List[i].Version_Year;
            }
        }
       
        $('#btnSave')[0].disabled = true;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        
        

    })
.error(function (error, status, headers, config) {

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (status == 999)
        window.location = "frmSessionExpired.aspx";
    else
        alert(error.Message + ".Please Contact Support!");
});
    $("#txtICD10").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "WebServices/PotentialDiagnosisService.asmx/SearchICDDescrptionText",
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
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
                        }
                    }

                });
            }
            if ($("#txtICD10").val().length > 3) {
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
            $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtICD10').width());
            $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            if (ui.item.label != "No matches found.") {
                if (JSON.stringify($scope.PotentialListTable).indexOf(ui.item.label.split('~')[0]) == -1) {
                    $scope.PotentialListTable.push({ 'ICD10Code': ui.item.label.split('~')[0], 'ICD10Description': ui.item.label.split('~')[1], 'EnableColumn': bColumnHide, 'MoveToAssessment': 'N', 'VersionYear': 'ICD_10' });
                    $scope.RefershGrid();
                }
                $scope.EnableSaveButton();
                $("[id*=pbDropdown]").addClass('pbDropdownBackground');
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
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtICD10").val().length <= 3)
                bBool = false;
            else
                bBool = true;
            $("#txtICD10").focus();
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
        document.getElementById('txtDescription').value = "";
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtICD10").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool)
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
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
                    url: "WebServices/PotentialDiagnosisService.asmx/SearchICDDescrptionText",
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
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
                if (JSON.stringify($scope.PotentialListTable).indexOf(ui.item.label.split('~')[0]) == -1) {
                    $scope.PotentialListTable.push({ 'ICD10Code': ui.item.label.split('~')[0], 'ICD10Description': ui.item.label.split('~')[1], 'EnableColumn': bColumnHide, 'MoveToAssessment': 'N', 'VersionYear': 'ICD_10' });
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
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#txtDescription").val().length <= 3)
                bBool = false;
            else
                bBool = true;
            $("#txtDescription").focus();
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
        document.getElementById('txtICD10').value = "";
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#txtDescription").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool)
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
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
            url: "WebServices/PotentialDiagnosisService.asmx/RefershGrid",
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
    $scope.EnableSaveButton = function () {
        $('#btnSave').removeAttr("disabled");
    }
    $scope.SavePotentialDiagnosisList = function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var table = $('#PotentialBody tr');
        if (table.length > 0 || DelList != 0) {
            if (table.length > 0) {
                for (var i = 0; i < table.length; i++) {
                    if ($('#PotentialBody tr')[i].children[4].innerHTML == 0) {
                        var ICD10Code = $('#PotentialBody tr')[i].children[2].innerHTML;
                        var ICD10Desc = $('#PotentialBody tr')[i].children[3].innerHTML;
                        var Notes = $('#PotentialBody tr')[i].children[6].children[0].value.trim();
                        var MoveToAssessment = $('#PotentialBody tr')[i].children[8].innerHTML;
                        var VersionYear = $('#PotentialBody tr')[i].children[9].innerHTML;
                        if ($('#PotentialBody tr')[i].children[5].children[0].value == "") {
                            UpdateList = [];
                            InsertList = [];
                            bErrorMsg = true;
                            DisplayErrorMessage('1011073');
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return;
                        }
                        else {
                            var source = $('#PotentialBody tr')[i].children[5].children[0].value;
                            InsertList.push(ICD10Code + "~" + ICD10Desc + "~" + Notes + "~" + source + "~" + MoveToAssessment + "~" + VersionYear);
                        }
                    }
                    else {
                        var ICD10Code = $('#PotentialBody tr')[i].children[2].innerHTML;
                        var ICD10Desc = $('#PotentialBody tr')[i].children[3].innerHTML;
                        var ID = $('#PotentialBody tr')[i].children[4].innerHTML;
                        var Notes = $('#PotentialBody tr')[i].children[6].children[0].value.trim();
                        var Version = $('#PotentialBody tr')[i].children[7].innerHTML;
                        var MoveToAssessment = $('#PotentialBody tr')[i].children[8].innerHTML;
                        var VersionYear = $('#PotentialBody tr')[i].children[9].innerHTML;
                        if ($('#PotentialBody tr')[i].children[5].children[0].value == "") {
                            UpdateList = [];
                            InsertList = [];
                            DisplayErrorMessage('1011073')
                            bErrorMsg = true;
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return;
                        }
                        else {
                            var source = $('#PotentialBody tr')[i].children[5].children[0].value;
                            UpdateList.push(ICD10Code + "~" + ICD10Desc + "~" + Notes + "~" + ID + "~" + Version + "~" + source + "~" + MoveToAssessment + "~" + VersionYear);
                        }
                    }
                }
            }
            $http({
                url: "WebServices/PotentialDiagnosisService.asmx/SavePotentialDiagnosisList",
                dataType: 'json',
                method: 'POST',
                data: JSON.stringify({ aryInsert: InsertList, aryUpdate: UpdateList, aryDelete: DelList }),
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    "X-Requested-With": "XMLHttpRequest"
                }
            }).success(function (response) {
                UpdateList = [];
                InsertList = [];
                DelList = [];
                $('#btnSave')[0].disabled = true;
                DisplayErrorMessage('1011071');
                $("#PotentialBody tr").remove();
                var Icdcode = [];
                var str = response.d;
                var ICDLst = JSON.parse(str);
                $scope.PotentialListTable = ICDLst.ICD10List;
                $scope.EnableColumn = ICDLst.EnableColumn;
                bColumnHide = ICDLst.EnableColumn;
                $scope.PotentialHeadListTable = [{ EnableColumn: bColumnHide }];
                var test = JSON.parse(response.d);

                if (test.ICD10List.length > 0) {
                    for (var i = 0; i < test.ICD10List.length; i++) {
                        $scope.PotentialListTable[i].ICD10Code = test.ICD10List[i].ICD_Code;
                        $scope.PotentialListTable[i].ICD10Description = test.ICD10List[i].ICD_Description;
                        $scope.PotentialListTable[i].ID = test.ICD10List[i].Id;
                        $scope.PotentialListTable[i].Notes = test.ICD10List[i].Notes.trim();
                        $scope.PotentialListTable[i].EnableColumn = bColumnHide;
                        $scope.PotentialListTable[i].Version = test.ICD10List[i].Version;
                        $scope.PotentialListTable[i].SelectedValue = test.ICD10List[i].Source;
                        $scope.PotentialListTable[i].source = test.ICD10List[i].Source;
                        $scope.PotentialListTable[i].MoveToAssessment = test.ICD10List[i].Move_To_Assessment;
                        $scope.PotentialListTable[i].VersionYear = test.ICD10List[i].Version_Year;
                    }
                }
                {
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                   
                }
               
                return;
            })
      .error(function (error, status, headers, config) {
          {
              UpdateList = [];
              InsertList = [];
              DelList = [];
              sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
          }
          if (status == 999)
              window.location = "frmSessionExpired.aspx";
          else
              alert(error.Message + ".Please Contact Support!");
      });
        }
    }
    var iIndex = -1;
    $scope.ICDDelete = function (index) {
        if (index != undefined)
            iIndex = index.$index + 1;
        if (DisplayErrorMessage('1011072') == true) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var table = $('#tblPotential');
            var sICDCode = table.find('tr')[iIndex].children[2].innerText;
            var sICDDesc = table.find('tr')[iIndex].children[3].innerText;
            var sID = table.find('tr')[iIndex].children[4].innerText;
            var sSource = table.find('tr')[iIndex].children[5].children[0].value;
            var sNotes = table.find('tr')[iIndex].children[6].children[0].value.trim();
            var sVersion = table.find('tr')[iIndex].children[7].innerText;
            var sMoveToAssessment = table.find('tr')[iIndex].children[8].innerText;
            var sVersionYear = table.find('tr')[iIndex].children[9].innerText;
            if (sID != "")
                DelList.push(sICDCode + "~" + sICDDesc + "~" + sNotes + "~" + sID + "~" + sVersion + "~" + sSource + "~" + sMoveToAssessment + "~" + sVersionYear);
            for (var i = 0; i < $scope.PotentialListTable.length; i++) {
                if (table.find('tr')[iIndex].children[2].innerText == $scope.PotentialListTable[i].ICD10Code) {
                    $scope.PotentialListTable.splice(i, 1);
                }
            }
            $scope.EnableSaveButton();
            $scope.RefershGrid();
            {
                sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
               
            }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return true;
        }
    }
    $scope.CloseModal = function () {
        if ($("#btnSave")[0].disabled == false) {
            event.preventDefault();
            autoSave();
        }
        else {
            $(top.window.document).find("#btnClose").click();
            redirectToCC();
        }
        return 1;
    }
    $scope.chkSelect_Change = function (index) {

        var ICDList = [];
        if ($('#PotentialBody tr td input.chkSelect:checked').length > 0) {
            if ($(top.window.document).find("#btnAssessment")[0].disabled == true)
                $(top.window.document).find("#btnAssessment")[0].disabled = false;
            var chkICDContainer = $('#PotentialBody tr td input.chkSelect:checked');
            for (var iCheck = 0; iCheck < chkICDContainer.length; iCheck++) {
                var sICDCode = chkICDContainer.parent().parent()[iCheck].cells[2].innerText;
                var sICDDesc = chkICDContainer.parent().parent()[iCheck].cells[3].innerText;
                var sID = chkICDContainer.parent().parent()[iCheck].cells[4].innerText;
                var sVersionYear = chkICDContainer.parent().parent()[iCheck].cells[9].innerText;
                var sVersion = chkICDContainer.parent().parent()[iCheck].cells[7].innerText;
                if (ICDList.length >= 1) {
                    ICDList.push("|" + sICDCode + "~" + sICDDesc + "~" + "Potential Diagnosis" + "~" + sID + "~" + sVersionYear + "~" + sVersion);
                }
                else {
                    ICDList.push(sICDCode + "~" + sICDDesc + "~" + "Potential Diagnosis" + "~" + sID + "~" + sVersionYear + "~" + sVersion);
                }
            }
        }
        if ($('#PotentialBody tr td input.chkSelect:checked').length == 0) {
            ICDList = [];
            $(top.window.document).find("#btnAssessment")[0].disabled = true;
        }
        //Jira - #CAP-80
        //sessionStorage.setItem('PotentialICDList', ICDList);
        if (ICDList != undefined && ICDList != null){
    localStorage.setItem('PotentialICDList', ICDList);
}
    }
    $(top.window.document).find("#chkPDShowAll").change(function () {
        $scope.chkPDShowAllClick();
        return false;
    });

    $scope.chkPDShowAllClick = function () {
        delete $scope.PotentialListTable;
        if ($(top.window.document).find("#chkPDShowAll")[0].checked == false) {
            sData = "ShowAllFalse";
        }
        else {
            sData = "ShowAllTrue";
        }

        $http({
            url: "WebServices/PotentialDiagnosisService.asmx/LoadPotentialDiagnosis",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({ data: sData }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }

        }).success(function (response, status, headers, config) {
            var Icdcode = [];
            var str = response.d;
            var ICDLst = JSON.parse(str);
            $scope.PotentialListTable = ICDLst.ICD10List;
            $scope.EnableColumn = ICDLst.EnableColumn;
            bColumnHide = ICDLst.EnableColumn;
            $scope.PotentialHeadListTable = [{ EnableColumn: bColumnHide }];
            var test = JSON.parse(response.d);

            if (test.ICD10List.length > 0) {
                for (var i = 0; i < test.ICD10List.length; i++) {
                    $scope.PotentialListTable[i].ICD10Code = test.ICD10List[i].ICD_Code;
                    $scope.PotentialListTable[i].ICD10Description = test.ICD10List[i].ICD_Description;
                    $scope.PotentialListTable[i].ID = test.ICD10List[i].Id;
                    $scope.PotentialListTable[i].Notes = test.ICD10List[i].Notes.trim();
                    $scope.PotentialListTable[i].EnableColumn = bColumnHide;
                    $scope.PotentialListTable[i].Version = test.ICD10List[i].Version;
                    $scope.PotentialListTable[i].source = test.ICD10List[i].Source;
                    $scope.PotentialListTable[i].SelectedValue = test.ICD10List[i].Source;
                    $scope.PotentialListTable[i].MoveToAssessment = test.ICD10List[i].Move_To_Assessment;
                    $scope.PotentialListTable[i].VersionYear = test.ICD10List[i].Version_Year;
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

    $scope.SelectedValue = function (index) {
        var sourcevalue = $('#PotentialBody tr')[index.$index].children[5].children[0].value;
        if (sourcevalue == "") {
            sourcevalue = $('#PotentialBody tr')[index.$index].children[5].children[2].value;
        }
        else {
            if ($('#PotentialBody tr')[index.$index].children[5].children[0].value.indexOf($('#PotentialBody tr')[index.$index].children[5].children[2].value) == -1) {
                sourcevalue = sourcevalue + ',' + $('#PotentialBody tr')[index.$index].children[5].children[2].value;
            }
        }
        $('#PotentialBody tr')[index.$index].children[5].children[0].value = sourcevalue;
    }

});

function SetSaveEnabled() {
    $('#btnSave')[0].disabled = false;
}
function SetSaveDisabled() {
    $('#btnSave')[0].disabled = true;
}

function autoSave() {
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
                    $('#btnSave')[0].click();
                    if (bErrorMsg == false) {
                        $(top.window.document).find("#btnClose").click();
                        redirectToCC();
                    }
                    bErrorMsg = false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $('#btnSave')[0].disabled = true;
                    $(top.window.document).find("#btnClose").click();
                    redirectToCC();
                },
                "Cancel": function () {
                    SetSaveEnabled();
                    $(dvdialog).dialog("close");
                }
            }
        });
    }
}

function redirectToCC() {
    $($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active")).removeClass("active");
    $($($(top.window.document).find("iframe")[0].contentDocument).find('#myTabs li.active')).removeClass("active");
    $($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane")[0]).addClass("active");
    $($($(top.window.document).find("iframe")[0].contentDocument).find('#myTabs li:eq(0)')).addClass("active");
}

function OpenDropDown(icon) {
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");
        var index = $(icon).context.parentNode.id;
        $($('#PotentialBody tr')[index].children[5].children[2]).disabled = false;
        $($('#PotentialBody tr')[index].children[5].children[2]).show();
        $($('#PotentialBody tr')[index].children[5].children[2]).click();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    }
    else {
        var index = $(icon).context.parentNode.id;
        $($('#PotentialBody tr')[index].children[5].children[2]).disabled = true;
        $($('#PotentialBody tr')[index].children[5].children[2]).hide();
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");
    }
}

function GetNotesList(icon, List) {

    $('#btnSave')[0].disabled = false;
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
                $('#btnSave')[0].disabled = false;
                var values = response.d.split("|");
                var targetControlValue = $(icon).parent().siblings().prop('id');
                var innerdiv = '';
                var txtPos = $('#' + targetControlValue).position(), tdPos = $('#' + targetControlValue).parent().position(),
                    trPos = $('#' + targetControlValue).parent().parent().position(), tblPos = $('#' + targetControlValue).parent().parent().parent().position();
                var pos = $('#' + targetControlValue).position();
                var topPosition = txtPos.top;
                if (document.URL.indexOf("ASSESSMENT") == -1) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' disabled onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                else {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:grey;font-weight:bolder;font-style: italic;cursor:default;pointer-events:none;' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                if (document.URL.indexOf("ASSESSMENT") == -1) {
                    for (var i = 0; i < values.length ; i++) {
                        innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default;' onclick=\"AddItem('" + values[i] + "^" + targetControlValue + "');\">" + values[i] + "</li>";
                    }
                }
                else {
                    for (var i = 0; i < values.length ; i++) {
                        innerdiv += "<li style='text-decoration: none; list-style-type: none;color:grey;cursor:default;pointer-events:none;' onclick=\"AddItem('" + values[i] + "^" + targetControlValue + "');\">" + values[i] + "</li>";
                    }
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
                          top: topPosition + $(".actcmpt").height() + 5,
                          left: pos.left,
                          width: pos.width,
                          height: '120',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '179px',
                          border: '1px solid #8e8e8e',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto'

                      }).insertAfter($("#" + targetControlValue + ".actcmpt"));
                }
                SetSaveEnabled();
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
        control = value[5];
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
        sugglistval = $("#" + value[1] + ".actcmpt").val().trim();
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
                $("#" + value[1] + ".actcmpt").val(sugglistval + "," + value[0]);
            }
        }
        else {
            $("#" + value[1] + ".actcmpt").val(value[0]);
        }
    }


}

function OpenPopup(Keyword) {
    var focused = Keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}