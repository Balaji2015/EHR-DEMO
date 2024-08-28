var human_id = document.URL.slice(document.URL.indexOf("HumanID")).split("&")[0].split("=")[1];
var myapp = angular.module('Allergieapp', []);
var AllergieListnew = [];

myapp.config(function ($provide) {
    $provide.decorator('$exceptionHandler', function ($delegate) {
        return function (exception, cause) {
            HandlerAngularjsError(exception);
        };
    });
});


myapp.controller('AllergieCtrl', function ($scope, $http) {

    var status = "Active";

    $http({
        url: "frmRCopiaDuplicateAllergy.aspx/GetAllergyWithPartialDuplicates",
        dataType: 'json',
        method: 'POST',
        data: '{HumanId:"' + human_id + '",status:"' + status + '"}',
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "X-Requested-With": "XMLHttpRequest"
        }
    }).success(function (response) {

        var test = JSON.parse(response.d);

        if (test != undefined && test != "") {
            $scope.AllergieList = test;
            AllergieListnew = test;
        }
    })


    $scope.DeleteClick = function () {

        var checkboxValues = [];
        var CheckboxList;
        $('input[name="DeleteCheck"]:checked').each(function () {
            checkboxValues.push(this.value);
            CheckboxList = checkboxValues.join(',');
        });

        if (CheckboxList != undefined && CheckboxList != '') {

            var Checkboxcount = checkboxValues.length;

            var DeleteCheck = DisplayErrorMessage('10113609', '', Checkboxcount.toString());
            if (DeleteCheck != undefined && DeleteCheck) {

                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

                $http({
                    url: "frmRCopiaDuplicateAllergy.aspx/DeleteRCopiaAllergy",
                    dataType: 'json',
                    method: 'POST',
                    data: '{RCopiaId:"' + CheckboxList + '"}',
                    headers: {
                        "Content-Type": "application/json; charset=utf-8",
                        "X-Requested-With": "XMLHttpRequest"
                    }
                }).success(function (response) {

                    var successmsg = response.d;

                    if (successmsg == "Success") {

                    $http({
                        url: "frmRCopiaDuplicateAllergy.aspx/GetAllergyWithPartialDuplicates",
                        dataType: 'json',
                        method: 'POST',
                        data: '{HumanId:"' + human_id + '",status:"' + status + '"}',
                        headers: {
                            "Content-Type": "application/json; charset=utf-8",
                            "X-Requested-With": "XMLHttpRequest"
                        }
                    }).success(function (response) {

                        var test = JSON.parse(response.d);

                        if (test != undefined) {
                            $scope.AllergieList = test;
                            AllergieListnew = test;
                        }
                    })

                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('10113610');
                    }
                    else {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('10113607', '', successmsg);
                    }
                })
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }

        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('10113605');
        }



    }

    $scope.ShowActive = function () {

        if (document.getElementById("ShowAll").checked == true) {
            status = "All";
        }
        else {
            status = "Active";
        }

        $http({
            url: "frmRCopiaDuplicateAllergy.aspx/GetAllergyWithPartialDuplicates",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({ HumanId: human_id, status: status }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {

            var test = JSON.parse(response.d);

            if (test != undefined && test != "") {
                $scope.AllergieList = test;
            }
        })

    }

    $scope.AllergySearch = function () {

        var Indexval = document.getElementById("txtAllergy").value;
        var FinalList = [];
        if (Indexval != undefined && Indexval != "") {

            var list = $scope.AllergieList;

            for (var i = 0; i < list.length; i++) {
                if (list[i].AlergyName.toUpperCase().indexOf(Indexval.toUpperCase()) == 0) {
                    FinalList.push(list[i]);
                }
            }

            $scope.AllergieList = FinalList;
        }
        else {
            $scope.AllergieList = AllergieListnew;
        }

    }

    $scope.AllergySearchDown = function () {

        var Indexval = document.getElementById("txtAllergy").value;
        var FinalList = [];
        if (Indexval != undefined && Indexval != "") {

            var list = $scope.AllergieList;

            for (var i = 0; i < list.length; i++) {
                if (list[i].AlergyName.toUpperCase().indexOf(Indexval.toUpperCase()) == 0) {
                    FinalList.push(list[i]);
                }
            }

            $scope.AllergieList = FinalList;
        }
        else {
            $scope.AllergieList = AllergieListnew;
        }

    }


    $scope.AllergyClear = function () {

        document.getElementById("txtAllergy").value = "";
        $scope.AllergieList = AllergieListnew;
    }

    $scope.CheckChange = function () {

        var checkboxValues = [];
        var CheckboxList;
        $('input[name="DeleteCheck"]:checked').each(function () {
            checkboxValues.push(this.value);
            CheckboxList = checkboxValues.join(',');
        });

        if (CheckboxList != undefined && CheckboxList.length > 0) {
            document.getElementById("btnDelete").disabled = false;
        }
        else {
            document.getElementById("btnDelete").disabled = true;
        }

    }


});


