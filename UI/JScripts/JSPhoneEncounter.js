//If any changes are made in this file increase value of version in the query string in all files referencing this javascript file 
//Only for AngularJS scripts

var DeleteArray = [];
var DeleteArrayICD = [];
var arrICD10Codes = [];
var lstSelectedCPT = new Array();
var lstSelectedICD = new Array();
var humanid;
var bSubmitted = false;
var intCPTLength = -1;
var CallDurationStaticlookup = [];

var CallDurationAutosuggestLoad = [];

function cboCallSpokenTo_SelectedIndexChanged(sender, args) {
    var cbovalue = document.getElementById("cboCallSpokenTo").value;
    //$("span[mand=Yes]").addClass('MandLabelstyle');
    //$("span[mand=Yes]").each(function () {
    //    $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    //});
    //$("[id*=pbDropdown]").addClass('pbDropdownBackground');
    if (cbovalue == "Self") {
        var txtValue = $("#hdnPatientName").val();//$find("txtPatientName")._text;
        document.getElementById("txtCallerName").value = txtValue;
        // $find("txtCallerName").set_value(txtValue);
        $('#txtCallerName').addClass('nonEditabletxtbox');
        $('#txtCallerName').removeClass('Editabletxtbox');
        document.getElementById("txtCallerName").readOnly = true;
    }
    else {
        document.getElementById("txtCallerName").value = "";
        $('#txtCallerName').addClass('Editabletxtbox');
        $('#txtCallerName').removeClass('nonEditabletxtbox');
        document.getElementById("txtCallerName").readOnly = false;
    }
    EnableSave();
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function txtCallerName_OnFocus(sender, args) {
    if (document.getElementById("cboCallSpokenTo").value == "Self") {
        document.getElementById("txtCallerName").readOnly = true;
        $('#txtCallerName').addClass('nonEditabletxtbox');
        $('#txtCallerName').removeClass('Editabletxtbox');
    }
}


function txtCallerName_OnMouseOver(sender, args) {
    if (document.getElementById("cboCallSpokenTo").value == "Self") {
        document.getElementById("txtCallerName").readOnly = true;
        $('#txtCallerName').addClass('nonEditabletxtbox');
        $('#txtCallerName').removeClass('Editabletxtbox');
    }
}

function txtCallerName_OnMouseOut(sender, args) {
    if (document.getElementById("cboCallSpokenTo").value == "Self") {
        document.getElementById("txtCallerName").readOnly = true;
        $('#txtCallerName').addClass('nonEditabletxtbox');
        $('#txtCallerName').removeClass('Editabletxtbox');
    }
}
function Test() {
    document.getElementById("DLC_pbLibrary").disabled = true;
}

function EnableSave() {
    document.getElementById("btnSave").disabled = false;
    document.getElementById("btnSaveOnly").disabled = false;
}

function datetime() {
    if (!($('#txtCalldate').prop('disabled'))) {
        $("#txtCalldate").datetimepicker({
            maxDate: '0',
            beforeShow: function () {
                jQuery(this).datepicker('option', 'maxDate', jQuery('#txtCalldate').val());
            },
            timepicker: true,
            closeOnDateSelect: true, format: 'd-M-Y h:i A',
            onSelect: function () {
                EnableSave();

            }, ampm: true
        });
    }
}
function GetWin() {
    var wind = GetRadWindow();
    wind.set_behaviors();
    // { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var date = new Date();
    $("#txtCalldate").datetimepicker({
        maxDate: '0',
        beforeShow: function () {
            jQuery(this).datepicker('option', 'maxDate', jQuery('#txtCalldate').val());
        },
        timepicker: true,
        closeOnDateSelect: true, format: 'd-M-Y h:i A',
        onSelect: function () {
            EnableSave();

        }, ampm: true
    });

    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}


function pageLoad() {
    $("#txtCalldate").datetimepicker({
        maxDate: '0',
        beforeShow: function () {
            jQuery(this).datepicker('option', 'maxDate', jQuery('#txtCalldate').val());
        },
        timepicker: true,
        closeOnDateSelect: true, format: 'd-M-Y h:i A',
        onSelect: function () {
            EnableSave();

        }, ampm: true
    });
}

function CalldateMethod() {
    $("#txtCalldate")[0].value = getLocalTime();
}
function getLocalTime() {
    var e = new Date,
        t = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"),
        n = e.getDate(),
        i = t[e.getMonth()],
        o = e.getFullYear(),

     ampm = 'am',
     h = e.getHours(),
     m = e.getMinutes(),
     s = e.getSeconds();
    if (h >= 12) {
        if (h >= 13) {
            h -= 12;
        }
        ampm = 'pm';
    }
    if (n < 10) n = '0' + n;
    if (m < 10) m = '0' + m;
    if (h < 10) h = '0' + h;
    if (s < 10) s = '0' + s;
    var ve = n.toString() + "-" + i.toString() + "-" + o.toString() + ' ' + h.toString() + ':' + m.toString() + ' ' + ampm.toUpperCase();
    return ve
}
function DisabledCall() {
    $("#txtCalldate")[0].disabled = true;
    $("#txtCalldate").datepicker('disable');
}

//function txtCallHrs_OnKeyPress(sender, args) {
//    EnableSave();
//    //var text = sender.get_value() + args.get_keyCharacter();
//    //var text = $("#txtCallHrs")[0].value
//    //if (!text.match('^[0-9]+$'))
//    //    args.set_cancel(true);

//    setInputFilter(document.getElementById("txtCallHrs"), function (value) {
//        return /^-?\d*$/.test(value);
//    });
//}

//function txtCallMins_OnKeyPress(sender, args) {
//    EnableSave();
//    // var text = sender.get_value() + args.get_keyCharacter();
//    //var text = $("#txtCallMins")[0].value
//    //if (!text.match('^[0-9]+$'))
//    //    args.set_cancel(true);

//    setInputFilter(document.getElementById("txtCallMins"), function (value) {
//        return /^-?\d*$/.test(value);
//    });
//}

//function btnSave_Clicked(sender, args) {
//    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
//    var now = new Date();
//    var utc = now.toUTCString();
//    document.getElementById("hdnLocalTime").value = utc;
//    if (($("#txtCallHrs")[0].value != "" && isNaN(parseFloat($("#txtCallHrs")[0].value))) || ($("#txtCallMins")[0].value != "" && isNaN(parseFloat($("#txtCallMins")[0].value)))) {
//        DisplayErrorMessage('000038');
//        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
//        if (isNaN(parseFloat($("#txtCallHrs")[0].value)))
//            $find("txtCallHrs").clear();
//        if (isNaN(parseFloat($("#txtCallMins")[0].value)))
//            $find("txtCallMins").clear();
//        sender.set_autoPostBack(false);
//    }
//    if ($("#txtCallMins")[0].value > 59) {
//        DisplayErrorMessage('7430007');
//        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
//        if (isNaN(parseFloat($("#txtCallMins")[0].value)))
//            $find("txtCallMins").clear();
//        sender.set_autoPostBack(false);
//    }
//}
function SetDateTime() {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
}
function parseMyDate(s) {
    var m = ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec'];
    var match = s.match(/(\d+)-([^.]+)-(\d+)/);
    if (match != '' && match != null) {
        var date = match[1];
        var monthText = match[2];
        var year = match[3];
        var month = m.indexOf(monthText.toLowerCase());
        return new Date(year, month, date);
    }

}


// Modified By Suvarnni- YesNoCancel Message
function btnClose_Clicked(sender, args) {
    var dob = parseMyDate(document.getElementById("hdnPatientDOB").value);
    var calldateonly = parseMyDate(document.getElementById("txtCalldate").value.split(' ')[0]);
    //if ($find("btnSave") != null && $find("btnSave") != undefined) {
    if (document.getElementById("btnSave").value != null && document.getElementById("btnSave").value != undefined) {
        if (document.getElementById("btnSave").disabled == false) {
            if (document.getElementById("hdnMessageType").value == "") {
                DisplayErrorMessage('1100000');   //('380049'); //('7430006');
                return false;
            }
            else if (document.getElementById("hdnMessageType").value == "Yes") {
                if (document.getElementById("txtCallerName").value == "") {
                    document.getElementById("txtCallerName").focus();
                    EnableSave();
                    document.getElementById("hdnMessageType").value = "";
                    DisplayErrorMessage('7430000');
                    return false;
                }
                else if (calldateonly < dob) {
                    document.getElementById("txtCallerName").focus();
                    EnableSave();
                    document.getElementById("hdnMessageType").value = "";
                    DisplayErrorMessage('7430009');
                    return false;
                }
                //CAP-713 - Add Electronically Signed
                else if (document.getElementById("chkElectronicallySigned").checked == false) {
                    document.getElementById("hdnMessageType").value = "";
                    DisplayErrorMessage('7430004');
                    return false;
                }
                else if (document.getElementById("txtCallerName").value != "" && document.getElementById("chkElectronicallySigned").checked == true) {
                    btnSave_Clicked(sender, args);
                    __doPostBack('btnSave', "true");
                    document.getElementById("hdnMessageType").value = "";
                    self.close();
                }
            }
            else if (document.getElementById("hdnMessageType").value == "No") {
                document.getElementById("hdnMessageType").value = ""
                self.close();
            }
            else if (document.getElementById("hdnMessageType").value == "Cancel") {
                document.getElementById("hdnMessageType").value = "";
                EnableSave();
                args.set_cancel(true);
            }
        }
        else {
            self.close();
        }
    }
    else {
        self.close();
    }
    self.close();
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
                    $("#btnSave").click();
                },
                "No": function () {
                    self.close();
                    $(dvdialog).dialog("close");

                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                }
            }
        });
    }
    else
        self.close();
}






function redirectToCC() {
    $($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active")).removeClass("active");
    $($($(top.window.document).find("iframe")[0].contentDocument).find('#myTabs li.active')).removeClass("active");
    $($($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane")[0]).addClass("active");
    $($($(top.window.document).find("iframe")[0].contentDocument).find('#myTabs li:eq(0)')).addClass("active");
}


function SaveEnable() {
    EnableSave();
}
function Close() {
    var win = GetRadWindow();
    win.close();
    DisplayErrorMessage('160001');
    return false;
}
var UserRole;
var myapp = angular.module('PhoneEncounterapp', []);
myapp.config(function ($provide) {
    $provide.decorator('$exceptionHandler', function ($delegate) {
        return function (exception, cause) {
            HandlerAngularjsError(exception);
        };
    });
});
var myappload = angular.module('EandMCodingapp', []);
var QuerystringValues = window.location.search;
var OpeningFrom = QuerystringValues.split('&')[0].replace("?", "");  // QuerystringValues.split('|')[0];//  //
var HumanID = QuerystringValues.split('&')[1].split('=')[1]; //QuerystringValues.split('|')[1];//; //
//var EncounterID = QuerystringValues.split('|')[2];

myapp.controller('PhoneEncounterCtrl', function ($scope, $http) {
    //{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChartPhoneEncounter(); } //change
    //jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css("display", "block");
    CallDurationAutosuggestLoad = [];
    cboSelectPhysician.addEventListener('change', CboPhysicianChange);
    cboCallDuration.addEventListener('change', cboCalldurationselectedindex);
    $http({
        url: "WebServices/PhoneEncounterService.asmx/LoadPhoneEncounter",
        dataType: 'json',
        method: 'POST',
        data: JSON.stringify({
            "sHumanID": HumanID
            //"sEncounterID": EncounterID
        }),
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            "X-Requested-With": "XMLHttpRequest"
        }
    }).success(function (response, status, headers, config) {
        lstSelectedICD = new Array();
        var str = response.d;
        var result = JSON.parse(str);
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } //change
        //CAP-713 - Add Electronically Signed
        $('#lblElectronicallySigned').text("Electronically Signed by " + result.PersonName)
        CalldateMethod();
        document.getElementById("hdnFaciltyName").value = result.FacilityName;
        //document.getElementById("hdnProvID").value = result.ApptProvIDORLoginID;
        UserRole = result.UserRole.toUpperCase();
        //loadPhysicianCombo(document.getElementById("hdnFaciltyName").value, document.getElementById("hdnProvID").value);
        loadPhysicianCombo(result.EncDOSPhyIDList);
        CallDurationStaticlookup = result.StaticCallDuration;
        //document.getElementById("txtAccountNumber").value = HumanID;
        var patientname;
        if (result.HumanDetails[0].Last_Name != "")
            patientname = result.HumanDetails[0].Last_Name
        if (result.HumanDetails[0].First_Name != "")
            patientname += "," + result.HumanDetails[0].First_Name
        if (result.HumanDetails[0].MI != "")
            //CAP-2168
            //patientname += "," + result.HumanDetails[0].MI
            patientname += " " + result.HumanDetails[0].MI
        if (result.HumanDetails[0].Suffix != "")
            //CAP-2168
            //patientname += "," + result.HumanDetails[0].Suffix
            patientname += " " + result.HumanDetails[0].Suffix
        document.getElementById("divPatientstrip").innerText = result.PatientDetails;

        var sContactDetails = "Contact Details : ";
        if (result.HumanDetails[0].Home_Phone_No != "")
            sContactDetails += "Home Phone# : " + result.HumanDetails[0].Home_Phone_No;
        if (result.HumanDetails[0].Cell_Phone_Number != "")
            sContactDetails += " | Cell Phone# : " + result.HumanDetails[0].Cell_Phone_Number;
        if (result.HumanDetails[0].Work_Phone_No != "")
            sContactDetails += " | Work Phone# : " + result.HumanDetails[0].Work_Phone_No;
        if (result.HumanDetails[0].Work_Phone_Ext != "")
            sContactDetails += " | Extension# : " + result.HumanDetails[0].Work_Phone_Ext;

        document.getElementById("divContactDetailstrip").innerText = sContactDetails;

        document.getElementById("hdnPatientName").value = patientname;
        document.getElementById("hdnPatientDOB").value = result.Birth_Date;
        //document.getElementById("txtPatientName").value = patientname;
        //document.getElementById("txtPatientDOB").value = result.Birth_Date;
        //document.getElementById("txtPatientSex").value = result.HumanDetails[0].Sex;
        //document.getElementById("txtHomePhone").value = result.HumanDetails[0].Home_Phone_No;
        //document.getElementById("txtCellPhone").value = result.HumanDetails[0].Cell_Phone_Number;
        //document.getElementById("txtWorkPhno").value = result.HumanDetails[0].Work_Phone_No;
        //document.getElementById("txtExtension").value = result.HumanDetails[0].Work_Phone_Ext;
        //if (result.UserRole.toUpperCase() == "PHYSICIAN") {
        //    document.getElementById("txtFacilityDOS").innerHTML = result.FacilityDOS;
        //    //document.getElementById("txtFacilityDOS").title = result.FacilityDOS;
        //}
        //if (result.EncounterDetails.split('|')[11] != '') {
        //    document.getElementById("cboCallSpokenTo").value = result.EncounterDetails.split('|')[11];
        //}
        //else {
        //    document.getElementById("cboCallSpokenTo").value = '';
        //}

        //if (result.EncounterDetails.split('|')[9] != '') {
        //    document.getElementById("txtCallHrs").value = result.EncounterDetails.split('|')[9];
        //}
        //else {
        //    document.getElementById("txtCallHrs").value = '';
        //}

        //if (result.EncounterDetails.split('|')[10] != '') {
        //    document.getElementById("txtCallMins").value = result.EncounterDetails.split('|')[10];
        //}
        //else {
        //    document.getElementById("txtCallMins").value = '';
        //}
        //document.getElementById("txtCalldate").value = result.EncounterDetails.split('|')[12];
        //document.getElementById("txtNotes").value = result.EncounterDetails.split('|')[13];



        if (OpeningFrom.trim() == "Menu") {
            document.getElementById("lblcabture").innerHTML = "Call Date*";
            document.getElementById("lblcabture").style.color = 'red';
            document.getElementById("lblCallerName").innerHTML = "Call Spoken To*";
            document.getElementById("lblCallerName").style.color = 'red';
            // document.getElementById("txtAccountNumber").value = HumanID;
        }
        else {
            // document.getElementById("txtAccountNumber").value = HumanID;
        }

        if (OpeningFrom.trim() != "Menu") {
            //DisabledCall();
            //document.getElementById("txtCalldate").value = "";
            //document.getElementById("txtCalldate").readOnly = true;
            //document.getElementById("txtCallHrs").readOnly = true;
            //document.getElementById("txtCallMins").readOnly = true;
            //document.getElementById("txtExtension").readOnly = true;
            // document.getElementById("cboCallSpokenTo").disabled = true;
            // document.getElementById("txtCallerName").disabled = true;
            //CAP-713 - Add Electronically Signed
            document.getElementById("lblElectronicallySigned").visible = false;
            document.getElementById("chkElectronicallySigned").visible = false;
            document.getElementById("btnSave").visible = false;
            //document.getElementById("txtCallHrs").style.color = "#BFDBFF";
            //document.getElementById("txtCallHrs").style.borderColor = "Black";
            //document.getElementById("txtCallMins").style.color = "#BFDBFF";
            //document.getElementById("txtCallMins").style.borderColor = "Black";
            //document.getElementById("txtNotes").disabled = true;
            //document.getElementById("txtCallerName").value = result.split('|')[8];
            //document.getElementById("txtCallHrs").value = result.split('|')[9];
            //document.getElementById("txtCallMins").value = result.split('|')[10];
            //document.getElementById("cboCallSpokenTo").value = result.split('|')[11];
            //document.getElementById("txtCalldate").value = result.split('|')[12];
            //document.getElementById("txtNotes").value = result.split('|')[13];
            //document.getElementById("txtCallerName").value = result.EncounterDetails.split('|')[8];
            //document.getElementById("txtCallHrs").value = result.EncounterDetails.split('|')[9];
            //document.getElementById("txtCallMins").value = result.EncounterDetails.split('|')[10];
            //document.getElementById("cboCallSpokenTo").value = result.EncounterDetails.split('|')[11];
            //document.getElementById("txtCalldate").value = result.EncounterDetails.split('|')[12];
            //document.getElementById("txtNotes").value = result.EncounterDetails.split('|')[13];
        }
        else {

            document.getElementById("txtCalldate").disabled = false;
            document.getElementById("txtCalldate").value = getLocalTime();
        }
        //  { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }  // not defind

        // { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } //change
        //BugID:51570
        $scope.Modifiers1 = JSON.parse(result.ModifierList);
        $scope.Modifiers2 = JSON.parse(result.ModifierList);
        $scope.Modifiers3 = JSON.parse(result.ModifierList);
        $scope.Modifiers4 = JSON.parse(result.ModifierList);
        $scope.EandMCodingCPTTable = result.ProcedureList;
        $scope.EandMCodingICDTable = result.ICDList;
        $scope.AssEandMCodingICDTable = result.AssICDlist;

        $scope.orderByFieldCPT = 'CPTCode';
        $scope.orderByFieldICD = 'ICDCode';
        $scope.reverseSort = false;
        EnablePriRbtn = result.EnablePriRbtn;
        // sessionStorage.setItem("Is_CMG_Ancillary", result.IsCMGAncillary);
        cboCalldurationselectedindex();
        EnableScreen = result.EnableScreen;
        var saveEnable = result.SaveEnable;
        if (EnableScreen != "") {
            $('#AngularDiv').find('input').attr("disabled", "disabled");
            $('#AngularDiv').find('button').attr("disabled", "disabled");
            $('#AngularDiv').find('textarea').attr("disabled", "disabled");
            $('#AngularDiv').find('select').attr("disabled", "disabled");
            //localStorage.setItem("bSave", "true");
            //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        }

        if (saveEnable == "true") {
            $('#btnSubAllForSuperbill').removeAttr("disabled");
            //localStorage.setItem("bSave", "false");
            //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
        else if (saveEnable == "false") {
            //localStorage.setItem("bSave", "true");
            //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }//change
        StopLoadingImage();
    })
        .error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                alert(error.Message + ".Please Contact Support!");
        });
    //{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } //change


    //below code for CPT and ICD Load:
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
                    url: "WebServices/PhoneEncounterService.asmx/SearchCPTDescrptionText",
                    // data: "{\"text\":\"" + document.getElementById("txtCPT").value + "|" + "txtCPT" + "\"}",
                    data: JSON.stringify({
                        "text": document.getElementById("txtCPT").value + "|" + "txtCPT",
                        "sCallDate": document.getElementById("txtCalldate").value
                    }),
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
                                // arrCPTs.push(item);
                                arrCPTs.push(item.label);
                                return {
                                    label: item.label,
                                    value: item.value
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
                            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                                log.ExceptionType + " \nMessage: " + log.Message);
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
                                label: item.label,
                                value: item.value
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
                var CPTCode = "";
                for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                    if (i == 0)
                        CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
                    else

                        CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
                }

                //Modified by balaji.TJ
                var Modifier = '';
                // (data.d);
                var MaxOrderList = new Array();
                var iOrder;
                if ($scope.EandMCodingCPTTable.length != 0) {
                    for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                        MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                    }
                }
                else {
                    MaxOrderList.push("0");
                }
                MaxOrderList.sort();
                iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;

                $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
                lstSelectedCPT.push(ui.item.label);
                $scope.RefershGrid();
                BindCPTtable();
                //}
                $scope.EnableSaveButton();

                //Comment by balaji.TJ
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

                //        $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
                //        lstSelectedCPT.push(ui.item.label);
                //        $scope.RefershGrid();
                //        BindCPTtable();
                //        //}
                //        $scope.EnableSaveButton();

                //    },
                //    error: function OnError(xhr) {
                //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //        if (xhr.status == 999)
                //            window.location = xhr.statusText;
                //        else {
                //            var log = JSON.parse(xhr.responseText);
                //            console.log(log);
                //            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                //                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                //                log.ExceptionType + " \nMessage: " + log.Message);
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
        //            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
        //                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
        //                log.ExceptionType + " \nMessage: " + log.Message);
        //            return false;
        //        }
        //    }
        //});
    }

    //function BindDeleteCPTtable() {
    //    var CPTCode = "";
    //    for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
    //        if (i == 0)
    //            CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
    //        else

    //            CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
    //    }
    //    $.ajax({
    //        type: "POST",
    //        url: "WebServices/EandMCodingService.asmx/DeleteModifierforCPT",
    //        contentType: "application/json; charset=utf-8",
    //        data: JSON.stringify({

    //            "CPTList": CPTCode
    //        }),
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            if (data.d != "") {
    //                var list = $.parseJSON(data.d);
    //                for (var i = 0; i < list.length; i++) {
    //                    for (var j = 0; j < $('#tblEandMCodingCPT > tbody  > tr').length; j++) {

    //                        if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[7].innerText.trim() == list[i].split('~')[0]) {

    //                            for (var k = 0; k < $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children.length; k++) {

    //                                if ($('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value == list[i].split('~')[1]) {

    //                                    $('#tblEandMCodingCPT > tbody  > tr')[j].cells[10].children[k].value = "";
    //                                    break;
    //                                }
    //                            }
    //                        }
    //                    }

    //                }
    //            }

    //        },
    //        error: function OnError(xhr) {
    //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //            if (xhr.status == 999)
    //                window.location = xhr.statusText;
    //            else {
    //                var log = JSON.parse(xhr.responseText);
    //                console.log(log);
    //                alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
    //                    ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
    //                    log.ExceptionType + " \nMessage: " + log.Message);
    //                return false;
    //            }
    //        }
    //    });
    //}


    $("#txtCPTDescription").autocomplete({
        source: function (request, response) {
            if (intCPTLength == 0 && bcheck && bBool == false) {
                arrCPTs = [];
                bBool = true;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    //url: "WebServices/EandMCodingService.asmx/SearchCPTDescrptionText",
                    //data: "{\"text\":\"" + document.getElementById("txtCPTDescription").value + "|" + "txtCPTDescription" + "\"}",
                    url: "WebServices/PhoneEncounterService.asmx/SearchCPTDescrptionText",
                    data: JSON.stringify({
                        "text": document.getElementById("txtCPTDescription").value + "|" + "txtCPTDescription",
                        "sCallDate": document.getElementById("txtCalldate").value
                    }),
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
                                arrCPTs.push(item.label);
                                return {
                                    label: item.label,
                                    value: item.value
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
                            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                                log.ExceptionType + " \nMessage: " + log.Message);
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

            if (ui.item.label != "No matches found.") {
                var CPTCode = "";
                for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                    if (i == 0)
                        CPTCode = $scope.EandMCodingCPTTable[i].CPTCode;
                    else

                        CPTCode = CPTCode + "|" + $scope.EandMCodingCPTTable[i].CPTCode;
                }

                //Modified by balaji.TJ

                var Modifier = ''; //(data.d);
                var MaxOrderList = new Array();
                var iOrder;
                if ($scope.EandMCodingCPTTable.length != 0) {
                    for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                        MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                    }
                }
                else {
                    MaxOrderList.push("0");
                }
                MaxOrderList.sort();
                iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': '', 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
                lstSelectedCPT.push(ui.item.label);
                $scope.RefershGrid();
                //}
                BindCPTtable();
                $scope.EnableSaveButton();

                //Comment by balaji.TJ

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
                //        $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
                //        lstSelectedCPT.push(ui.item.label);
                //        $scope.RefershGrid();
                //        //}
                //        BindCPTtable();
                //        $scope.EnableSaveButton();

                //    },
                //    error: function OnError(xhr) {
                //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //        if (xhr.status == 999)
                //            window.location = xhr.statusText;
                //        else {
                //            var log = JSON.parse(xhr.responseText);
                //            console.log(log);
                //            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                //                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                //                log.ExceptionType + " \nMessage: " + log.Message);
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
    $('#dlstICD10').load("htmICD10.html", function () {
        arrICD10Codes = $.map($('#dlstICD10 option'), function (li) {
            return $(li).attr("value");
        });
    });
    $("#txtICD10").autocomplete({
        source:
                function (request, response) {
                    if (arrICD10Codes == null) {
                        $.get("htmICD10.html").done(function (file) {
                            arrICD10Codes = $.map(file, function (li) {
                                return $(li).attr("value");
                            });
                        });
                    }
                    var results = FilterCodes(arrICD10Codes, request.term);

                    response($.map(results, function (item) {
                        return {
                            label: item,
                            value: item
                        }
                    }));

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
                    $scope.EandMCodingICDTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'ICDVersion': '0', 'btnDelete': 'Resources/Delete-Blue.png', 'IsPrimary': 'N', 'EandMICDID': '0', 'Sequence': '6', 'EnablePriRbtn': EnablePriRbtn });
                    lstSelectedICD.push(ui.item.label);
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
                    url: "WebServices/PhoneEncounterService.asmx/SearchICDDescrptionText",
                    // data: "{\"text\":\"" + document.getElementById("txtDescription").value + "|" + "txtDescription" + "|" + "ICD10" + "\"}",

                    data: JSON.stringify({
                        "text": document.getElementById("txtDescription").value + "|" + "txtDescription" + "|" + "ICD10",
                        "sCallDate": document.getElementById("txtCalldate").value
                    }),
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
                                    label: item
                                }
                            }));
                        }
                        else {
                            response($.map(jsonData, function (item) {
                                //Jira CAP-2140
                                //arrCPTs.push(item);
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
                            window.location = "/frmSessionExpired.aspx";
                        else {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);
                            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                                log.ExceptionType + " \nMessage: " + log.Message);
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
                    $scope.EandMCodingICDTable.push({ 'ICDCode': ui.item.label.split('~')[0], 'ICDDescription': ui.item.label.split('~')[1], 'ICDVersion': 0, 'btnDelete': 'Resources/Delete-Blue.png', 'IsPrimary': 'N', 'EandMICDID': 0, 'Sequence': '6', 'EnablePriRbtn': EnablePriRbtn });
                    lstSelectedICD.push(ui.item.label);
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
                // { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }//change
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
                    for (var i = 0; i < jsonData.length; i++) {
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
                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                        log.ExceptionType + " \nMessage: " + log.Message);
                }
            }
        });
    }
    $scope.LoadFavouriteICDs = function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var vselectPhyId = $('#cboSelectPhysician option:selected').attr('id');
        //if (vselectPhyId != undefined && vselectPhyId != "") { }
        var arrListICDs = [];
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "WebServices/PhoneEncounterService.asmx/GetFavouriteICDS",
            data: "{'uPhysicianID':'" + vselectPhyId + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d != undefined && data.d != "") {
                    var jsonData = $.parseJSON(data.d);

                    for (var i = 0; i < jsonData.length; i++) {
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
                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                        log.ExceptionType + " \nMessage: " + log.Message);
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
        if ($(top.window.document).find('#divCPTFormView.in').length > 0) {
            $(top.window.document).find("#btnMaximizeViewResultCPT").click();
        }
        else {
            if ($(top.window.document).find('#divFormView.in').length > 0) {
                $(top.window.document).find("#btnICDClose").click();
                $(top.window.document).find("#btnMaximizeViewResultICD").click();
                $(top.window.document).find("#divFormView").css({ "position": "static" });
                $(top.window.document).find("#divFormView").css({ "z-index": "10000" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divCPTFormView").css({ "z-index": "10000" });
            }
            else {
                $(top.window.document).find("#main").css({ "position": "relative" });
                $(top.window.document).find("#divCPTFormView").css({ "position": "absolute" });
                $(top.window.document).find("#divCPTFormView").css({ "z-index": "10000" });
            }
            /*Add only phone encounter in type of visit*/
            $(top.window.document).find("#select_Type_Of_Visit option").remove();

            $(top.window.document).find("#select_Type_Of_Visit")
                    .append($("<option></option>").attr("value", "Phone Encounter").text("Phone Encounter"));

            //$(top.window.document).find("#select_Type_Of_Visit").each(function () {
            //    $(this).find('option').filter(function () {
            //        return this.text.toUpperCase() == jsonData[1].toUpperCase();
            //    }).attr('selected', true);

            //})
            // }


            /*  $.ajax({
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
                          window.location = xhr.statusText;
                      else {
                          var log = JSON.parse(xhr.responseText);
                          console.log(log);
                          alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                              ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                              log.ExceptionType + " \nMessage: " + log.Message);
                      }
                  }
              });*/

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
        $(top.window.document).find('#divCPTFormView').modal({ backdrop: 'static', keyboard: false }, 'show');

        var table = $(top.window.document).find('#tbFavCPTsContainer');
        if (PhysicianCPTs.length != 0 && PhysicianCPTs.length <= 2 && catlist.length == 1)
        { RowCount = 2; }
        else
        {
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
            // if (localStorage.getItem("PhysicianICD") == "")
            $scope.LoadFavouriteICDs();
            //else
            //    $scope.FillFavoriteICDs();
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
                MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
            }
        }
        else {
            MaxOrderList.push("0");
        }
        MaxOrderList.sort();
        // iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
        var sResultCPTs = "";
        for (var i = 0; i < CheckedCPTs.length; i++) {
            var cells = $(CheckedCPTs[i]).find('td');

            if (i == 0) {
                for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                    MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                }
                iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                if (iOrder == 0)
                    iOrder = 1;
            }
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

        var FinalCPTs = sResultCPTs.replace(/\"/g, "$");
        $http({
            url: "WebServices/EandMCodingService.asmx/GetFormviewCPT",
            dataType: 'json',
            method: 'POST',
            data: '{sFormviewCPT: "' + FinalCPTs + '" }',
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }

        }).success(function (response, status, headers, config) {
            if (response.d != "") {
                var test = JSON.parse(response.d);

                if (test.ListofCPTs.length > 0) {
                    for (var i = 0; i < test.ListofCPTs.length; i++) {
                        $scope.EandMCodingCPTTable.push(test.ListofCPTs[i]);
                        lstSelectedCPT.push(test.ListofCPTs[i].CPTCode + "~" + test.ListofCPTs[i].CPTDesc);
                    }
                }
                $scope.EnableSaveButton();
                $(top.window.document).find("#tbFavCPTsContainer #dynTr").remove();
                $(top.window.document).find('#divCPTFormView').modal('hide');
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }).error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                alert(error.Message + ".Please Contact Support!");
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
                        else
                            sResultICDs += "|" + cells[1].innerText.trim() + "~" + cells[2].innerText.trim() + "~" + 'N' + "~" + max;
                    }
                    max = parseInt(max) + 1;
                }
            }
            if (cells[3].textContent == "") {
                if (cells[3].childNodes.length != 0) {
                    if (cells[3].childNodes[0].checked) {
                        {
                            if (sResultICDs == "")
                                sResultICDs += cells[4].innerText.trim() + "~" + cells[5].innerText.trim() + "~" + 'N' + "~" + max;
                            else
                                sResultICDs += "|" + cells[4].innerText.trim() + "~" + cells[5].innerText.trim() + "~" + 'N' + "~" + max;
                        }
                        max = parseInt(max) + 1;
                    }
                }
            }

            if (cells[6].textContent == "") {
                if (cells[6].childNodes.length != 0) {
                    if (cells[6].childNodes[0].checked) {
                        {
                            if (sResultICDs == "")
                                sResultICDs += cells[7].innerText.trim() + "~" + cells[8].innerText.trim() + "~" + 'N' + "~" + max;
                            else
                                sResultICDs += "|" + cells[7].innerText.trim() + "~" + cells[8].innerText.trim() + "~" + 'N' + "~" + max;
                        }
                        max = parseInt(max) + 1;
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
                        if (JSON.stringify($scope.EandMCodingICDTable).indexOf(test.ListofICDs[i].ICDCode) == -1) {
                            $scope.EandMCodingICDTable.push(test.ListofICDs[i]);
                            lstSelectedICD.push(test.ListofICDs[i].ICDCode + "~" + test.ListofICDs[i].ICDDescription);
                        }
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
                alert(error.Message + ".Please Contact Support!");
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
        DeleteArray = new Array();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (index != undefined)
            iIndex = index;
        if (DisplayErrorMessage('530007') == true) {
            var DelList = [];
            var table = $('#tblEandMCodingCPT');
            var iCheck = iIndex + 1;
            iIndex = -1;
            //BugID:49118
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

            var chkCPTContainer = $('#tblEandMCodingCPT').find('tr');
            var sCPTCode = chkCPTContainer[iCheck].cells[1].innerText.trim();
            var sCPTDesc = chkCPTContainer[iCheck].cells[2].innerText.trim();
            var sUnits = $(chkCPTContainer[iCheck].cells[3].children[0]).val().trim();
            var sModi1 = $(chkCPTContainer[iCheck].cells[4].children[0]).val().trim();
            var sModi2 = $(chkCPTContainer[iCheck].cells[4].children[1]).val().trim();
            var sModi3 = $(chkCPTContainer[iCheck].cells[4].children[2]).val().trim();
            var sModi4 = $(chkCPTContainer[iCheck].cells[4].children[3]).val().trim();
            var sdPot1 = $(chkCPTContainer[iCheck].cells[5].children[0]).val().trim();
            var sdPot2 = $(chkCPTContainer[iCheck].cells[6].children[0]).val().trim();
            var sdPot3 = $(chkCPTContainer[iCheck].cells[7].children[0]).val().trim();
            var sdPot4 = $(chkCPTContainer[iCheck].cells[8].children[0]).val().trim();
            var sdPot5 = $(chkCPTContainer[iCheck].cells[9].children[0]).val().trim();
            var sdPot6 = $(chkCPTContainer[iCheck].cells[10].children[0]).val().trim();

            DeleteArray.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + sdPot1 + "~" + sdPot2 + "~" + sdPot3 + "~" + sdPot4 + "~" + sdPot5 + "~" + sdPot6 + "~" + chkCPTContainer[iCheck].cells[11].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[12].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[13].innerText.trim());

            //var sCPTCode = table.find('tr')[iCheck].children[1].innerText.trim();
            //var sCPTDesc = table.find('tr')[iCheck].children[2].innerText.trim();
            //var sUnits = table.find('tr')[iCheck].children[3].children[0].value;
            //var sModi1 = table.find('tr')[iCheck].cells[4].children[0].selectedOptions[0].innerText.trim();
            //var sModi2 = table.find('tr')[iCheck].cells[4].children[1].selectedOptions[0].innerText.trim();
            //var sModi3 = table.find('tr')[iCheck].cells[4].children[2].selectedOptions[0].innerText.trim();
            //var sModi4 = table.find('tr')[iCheck].cells[4].children[3].selectedOptions[0].innerText.trim();
            //var sDiaPointer1 = table.find('tr')[iCheck].cells[5].children[0].value;
            //var sDiaPointer2 = table.find('tr')[iCheck].cells[5].children[1].value;
            //var sDiaPointer3 = table.find('tr')[iCheck].cells[5].children[2].value;
            //var sDiaPointer4 = table.find('tr')[iCheck].cells[5].children[3].value;
            //var sDiaPointer5 = table.find('tr')[iCheck].cells[5].children[4].value;
            //var sDiaPointer6 = table.find('tr')[iCheck].cells[5].children[5].value;
            //DeleteArray.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + CheckboxValue + "~" + table.find('tr')[iCheck].children[11].innerText.trim() + "~" + table.find('tr')[iCheck].children[12].innerText.trim() + "~" + table.find('tr')[iCheck].children[13].innerText.trim());
            //DeleteArray.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + table.find('tr')[iCheck].cells[6].innerText.trim() + "~" + table.find('tr')[iCheck].cells[7].innerText.trim() + "~" + table.find('tr')[iCheck].cells[8].innerText.trim() + "~" + table.find('tr')[iCheck].cells[9].innerText.trim() + "~" + sDiaPointer1 + "~" + sDiaPointer2 + "~" + sDiaPointer3 + "~" + sDiaPointer4 + "~" + sDiaPointer5 + "~" + sDiaPointer6);

            for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                for (var j = 0; j < DeleteArray.length; j++) {
                    if ($scope.EandMCodingCPTTable[i] != null && $scope.EandMCodingCPTTable[i] != undefined) {
                        if (DeleteArray[j].split('~')[15] == $scope.EandMCodingCPTTable[i].Order) {
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
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (index != undefined)
            iIndex = index;
        if (DisplayErrorMessage('530006') == true) {
            var DelList = [];
            var table = $('#tblEandMCodingICD');
            var iCheck = iIndex + 1;
            iIndex = -1;
            DeleteArrayICD.push(table.find('tr')[iCheck].children[3].innerText.trim());
            for (var i = 0; i < $scope.EandMCodingICDTable.length; i++) {
                for (var j = 0; j < DeleteArrayICD.length; j++) {
                    if (DeleteArrayICD[j].trim() == $scope.EandMCodingICDTable[i].ICDCode.trim()) {
                        $scope.EandMCodingICDTable.splice(i, 1);

                        //Remove diagnosis pointer mapping from CPT table if ICD deleted
                        if ($('#tblEandMCodingCPT tr').length > 0) {
                            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.trim().toUpperCase() == table.find('tr')[iCheck].children[2].innerText.trim().toUpperCase()) {
                                        $($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value = "";
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
        $('#btnSaveOnly').removeAttr("disabled");
        $('#btnSubAllForSuperbill').removeAttr("disabled");
        //localStorage.setItem("bSave", "false");
        //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }

    $scope.SaveEandMCoding = function (index, submitmode) {
        var arrlstAssICD = [];
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (document.getElementById("txtCalldate").value != null && document.getElementById("txtCalldate").value == '') {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('7430008');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
        if (document.getElementById("txtCallerName").value != null && document.getElementById("txtCallerName").value == '') {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('7430000');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
        /*if ($('#tblEandMCodingCPT tr td input:checked').length == 0) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('530003');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }*/
        if ($('#tblEandMCodingCPT tr td').length == 0) { //any cpt not mapped
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('530003');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }


        var bcheckNoChrg = false;
        for (var iRowCPT = 1; iRowCPT < $('#tblEandMCodingCPT tr').length; iRowCPT++) {
            if ($('#tblEandMCodingCPT tr')[iRowCPT].cells[1].innerText.trim().toUpperCase() == "NCVST" || $('#tblEandMCodingCPT tr')[iRowCPT].cells[1].innerText.trim().toUpperCase() == "NO CHARGE" ||
                $('#tblEandMCodingCPT tr')[iRowCPT].cells[1].innerText.trim().toUpperCase() == "NOBILL" || $('#tblEandMCodingCPT tr')[iRowCPT].cells[1].innerText.trim().toUpperCase() == "NO CHRG") {
                bcheckNoChrg = true;//For Bug Id : 72339
            }
        }


        var aryCPTList = [];
        var aryICDList = [];
        //comment by balaji.TJ
        /*if (($('#tblEandMCodingICD tr td input[type=checkbox]:checked').length == 0 && bcheckNoChrg == false)) {
            //if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT') {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('530004');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
            // }
        }*/
        if (($('#tblEandMCodingICD tr td').length == 0 && $('#tblAssEandMCodingICD tr td').length == 0 && bcheckNoChrg == false)) { //save service procedure code without icd.
            //if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT') {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('530004');
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            //}
        }
        else if ($('#tblEandMCodingICD tr td input.IsPrimary:checked').length == 0 && bcheckNoChrg == false) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('530005');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }
        else if ($('#tblEandMCodingCPT tr').length > 0) {
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
        //// else if (UserRole.toUpperCase() != 'MEDICAL ASSISTANT') {
        //for (var j = 1; j < 7; j++) {
        //    if ($('#tblEandMCodingCPT tr td input.chkCPT' + j + ':checked').length > 0 && ($('#tblEandMCodingICD tr td input:checked').length > 0 && $('#tblEandMCodingICD tr td input.chkICD' + j + ':checked').length == 0)) {//mapping not complete i.e 6 markd in CPT but 6 not marked in ICD
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        sessionStorage.setItem("EncCancel", "false");
        //        //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //        DisplayErrorMessage('530014', "", "'" + j + "'");
        //        bSaveCheck = true;
        //        AutoSaveUnsuccessful();
        //        return;
        //    }
        //}
        //////}



        //comment by balaji.TJ sequence
        //for (var iCPTChk = 1; iCPTChk < $('#tblEandMCodingCPT tr').length; iCPTChk++) {
        //    if ($($('#tblEandMCodingCPT tr')[iCPTChk]).find("input:checked").length == 0) {
        //        DisplayErrorMessage('530023', "", "'" + $('#tblEandMCodingCPT tr')[iCPTChk].cells[7].innerText + "'");
        //        bSaveCheck = true;
        //        AutoSaveUnsuccessful();
        //        return;
        //    }
        //}

        //  comment by balaji.TJ
        //sequenceCPT is not selected but the modifier is added.Please check.
        //for (var iRowCPT = 1; iRowCPT < $('#tblEandMCodingCPT tr').length; iRowCPT++) {
        //    var SelectedRow = $('#tblEandMCodingCPT tr')[iRowCPT];
        //    if (SelectedRow.children[10].children[0].selectedOptions[0].innerText.trim() != "" || SelectedRow.children[10].children[1].selectedOptions[0].innerText.trim() != ""
        //        || SelectedRow.children[10].children[2].selectedOptions[0].innerText.trim() != "" || SelectedRow.children[10].children[3].selectedOptions[0].innerText.trim() != "") {
        //        if ($('#tblEandMCodingCPT tr input.chkCPTRow' + SelectedRow.children[13].innerText.trim() + SelectedRow.children[7].innerText.trim() + ':checked').length == 0) {
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            DisplayErrorMessage('530015');
        //            bSaveCheck = true;
        //            AutoSaveUnsuccessful();
        //            return;
        //        }
        //    }
        //    if (SelectedRow.children[9].children[0].value == "") {
        //        if ($('#tblEandMCodingCPT tr input.chkCPTRow' + SelectedRow.children[13].innerText.trim() + SelectedRow.children[7].innerText.trim() + ':checked').length > 0) {
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            DisplayErrorMessage('530016');
        //            bSaveCheck = true;
        //            return;
        //        }
        //    }
        //    if (SelectedRow.children[9].children[0].value == "0" || SelectedRow.children[9].children[0].value == "00" || SelectedRow.children[9].children[0].value == "000") {//units to be between 1 to 999//BugID:49446
        //        if ($('#tblEandMCodingCPT tr input.chkCPTRow' + SelectedRow.children[13].innerText.trim() + SelectedRow.children[7].innerText.trim() + ':checked').length > 0) {
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            DisplayErrorMessage('530017');
        //            bSaveCheck = true;
        //            AutoSaveUnsuccessful();
        //            return;
        //        }
        //    }
        //}

        //for (var i = 1; i < 7; i++) {
        //    if ($('#tblEandMCodingCPT tr td input.chkCPT' + i + ':checked').length == 0 && ($('#tblEandMCodingICD tr td input.chkICD' + i + ':checked').length > 0)) {
        //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        sessionStorage.setItem("EncCancel", "false");
        //        //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        //        DisplayErrorMessage('530013', "", "'" + i + "'");
        //        bSaveCheck = true;
        //        AutoSaveUnsuccessful();
        //        return;
        //    }
        //}


        //if ($('#tblEandMCodingICD tr td input:checked').length > 0) {
        //    for (var jICD = 1; jICD < $('#tblEandMCodingICD tr').length; jICD++) {
        //        if ($($('#tblEandMCodingICD tr')[jICD]).find("input:checked").length == 0) {
        //            DisplayErrorMessage('530022');
        //            bSaveCheck = true;
        //            AutoSaveUnsuccessful();
        //            return;
        //        }
        //    }
        //}

        //Validation
        var DuplicateDiagnosis = [];
        DuplicateDiagnosis = new Array();
        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                DuplicateDiagnosis = new Array();
                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.trim() != "") {
                        if (DuplicateDiagnosis.length == 0) {
                            DuplicateDiagnosis.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase());
                        }
                        else if (DuplicateDiagnosis.indexOf($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim()) > -1) {
                            DisplayErrorMessage('530026', "", "'" + $($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim() + "'-'" + $($('#tblEandMCodingCPT tr'))[cptlength].children[1].innerText.toUpperCase().trim() + "'-'" + parseInt(DiaPointer + 1) + "'");
                            bSaveCheck = true;
                            AutoSaveUnsuccessful();
                            return;
                        }
                        else {
                            DuplicateDiagnosis.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + +DiaPointer].children[0].value.toUpperCase());
                        }
                    }
                }
            }
        }
        var bSerial = false;
        var bSeqValidation = false;
        var bPosition = false;
        var iDiaPointerPosition = 0;
        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                bSerial = false;
                bSeqValidation = false;
                bPosition = false;
                iDiaPointerPosition = 0;
                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim() == "") {
                        if (bSeqValidation != true) {
                            iDiaPointerPosition = parseInt(DiaPointer) + 1;
                        }
                        if (bPosition == false) {
                            bPosition = true;
                            iDiaPointerPosition = parseInt(DiaPointer) + 1;
                        }
                        bSerial = true;
                    }
                    if (bSerial == true && $($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim() != "") {
                        var Cpt = $($('#tblEandMCodingCPT tr'))[cptlength].children[1].innerText.toUpperCase().trim();
                        if (iDiaPointerPosition == 0)
                        {
                            iDiaPointerPosition = parseInt(iDiaPointerPosition) + 1;
                        }
                        DisplayErrorMessage('530027', "", "'" + Cpt + "'-'" + (parseInt(DiaPointer) + 1) + "'-'" + iDiaPointerPosition + "'");
                        bSaveCheck = true;
                        AutoSaveUnsuccessful();
                        return;
                    }
                    else {
                        bSeqValidation = true;
                    }
                }
            }
        }

        var DiagnosisPointerCPTList = [];
        var DiagnosisPointerICDList = [];

        DiagnosisPointerCPTList = new Array();
        DiagnosisPointerICDList = new Array();

        if ($('#tblEandMCodingCPT tr').length > 0) {
            for (var cptlength = 1; cptlength < $('#tblEandMCodingCPT tr').length; cptlength++) {
                for (var DiaPointer = 0; DiaPointer < 6; DiaPointer++) {
                    if ($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim() != "") {
                        if (DiagnosisPointerCPTList.length == 0) {
                            DiagnosisPointerCPTList.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim());
                        }
                        else if (DiagnosisPointerCPTList.indexOf($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim()) == -1) {
                            DiagnosisPointerCPTList.push($($('#tblEandMCodingCPT tr'))[cptlength].children[5 + DiaPointer].children[0].value.toUpperCase().trim());
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


        //

        //if ($('#tblEandMCodingCPT tr td input:checked').length > 0) {
        //    var chkCPTContainer = $('#tblEandMCodingCPT tr td input:checked');
        //    for (var iCheck = 0; iCheck < chkCPTContainer.length; iCheck++) {
        //        var sCPTCode = chkCPTContainer.parent().parent()[iCheck].cells[7].innerText.trim();
        //        var sCPTDesc = chkCPTContainer.parent().parent()[iCheck].cells[8].innerText.trim();
        //        var sUnits = chkCPTContainer.parent().parent()[iCheck].cells[9].children[0].value;
        //        var sModi1 = chkCPTContainer.parent().parent()[iCheck].cells[10].children[0].selectedOptions[0].innerText.trim();
        //        var sModi2 = chkCPTContainer.parent().parent()[iCheck].cells[10].children[1].selectedOptions[0].innerText.trim();
        //        var sModi3 = chkCPTContainer.parent().parent()[iCheck].cells[10].children[2].selectedOptions[0].innerText.trim();
        //        var sModi4 = chkCPTContainer.parent().parent()[iCheck].cells[10].children[3].selectedOptions[0].innerText.trim();
        //        aryCPTList.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + $('#tblEandMCodingCPT tr td input:checked')[iCheck].className.replace("chkCPT", "")[0] + "~" + chkCPTContainer.parent().parent()[iCheck].cells[11].innerText.trim() + "~" + chkCPTContainer.parent().parent()[iCheck].cells[12].innerText.trim() + "~" + chkCPTContainer.parent().parent()[iCheck].cells[13].innerText.trim());
        //    }
        //}


        //Modified  by balaji.TJ 
        var chkCPTContainer = $('#tblEandMCodingCPT tr.ng-scope');
        
        for (var iCheck = 0; iCheck < chkCPTContainer.length; iCheck++) {
            var sCPTCode = chkCPTContainer[iCheck].cells[1].innerText.trim();
            var sCPTDesc = chkCPTContainer[iCheck].cells[2].innerText.trim();
            var sUnits = $(chkCPTContainer[iCheck].cells[3].children[0]).val().trim();
            var sModi1 = $(chkCPTContainer[iCheck].cells[4].children[0]).val().trim();
            var sModi2 = $(chkCPTContainer[iCheck].cells[4].children[1]).val().trim();
            var sModi3 = $(chkCPTContainer[iCheck].cells[4].children[2]).val().trim();
            var sModi4 = $(chkCPTContainer[iCheck].cells[4].children[3]).val().trim();
            var sdPot1 = $(chkCPTContainer[iCheck].cells[5].children[0]).val().trim();
            var sdPot2 = $(chkCPTContainer[iCheck].cells[6].children[0]).val().trim();
            var sdPot3 = $(chkCPTContainer[iCheck].cells[7].children[0]).val().trim();
            var sdPot4 = $(chkCPTContainer[iCheck].cells[8].children[0]).val().trim();
            var sdPot5 = $(chkCPTContainer[iCheck].cells[9].children[0]).val().trim();
            var sdPot6 = $(chkCPTContainer[iCheck].cells[10].children[0]).val().trim();
            
            aryCPTList.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + sdPot1 + "~" + sdPot2 + "~" + sdPot3 + "~" + sdPot4 + "~" + sdPot5 + "~" + sdPot6 + "~" + chkCPTContainer[iCheck].cells[11].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[12].innerText.trim() + "~" + chkCPTContainer[iCheck].cells[13].innerText.trim());
        }
        

        //comment  by balaji.TJ 
        ////for (var jICD = 1; jICD < $('#tblEandMCodingICD tr').length; jICD++) {
        ////    var chkICDContainer = $('#tblEandMCodingICD tr')[jICD];
        ////    var chkICD1 = ""; var chkICD2 = ""; var chkICD3 = ""; var chkICD4 = ""; var chkICD5 = ""; var chkICD6 = "";
        ////    var IsPrimary = 'N';
        ////    if (chkICDContainer.cells[1].children[0].checked == true) {
        ////        IsPrimary = 'Y';
        ////    }
        ////    if (chkICDContainer.cells[2].children[0].checked == true) {
        ////        chkICD1 = '1';
        ////    }
        ////    if (chkICDContainer.cells[3].children[0].checked == true) {
        ////        chkICD2 = '2';
        ////    }
        ////    if (chkICDContainer.cells[4].children[0].checked == true) {
        ////        chkICD3 = '3';
        ////    }
        ////    if (chkICDContainer.cells[5].children[0].checked == true) {
        ////        chkICD4 = '4';
        ////    }
        ////    if (chkICDContainer.cells[6].children[0].checked == true) {
        ////        chkICD5 = '5';
        ////    }
        ////    if (chkICDContainer.cells[7].children[0].checked == true) {
        ////        chkICD6 = '6';
        ////    }

        ////    var sICDCode = chkICDContainer.cells[8].innerText.trim();
        ////    var sICDDesc = chkICDContainer.cells[9].innerText.trim();
        ////    if (chkICD1 != "" || chkICD2 != "" || chkICD3 != "" || chkICD4 != "" || chkICD5 != "" || chkICD6 != "")
        ////        aryICDList.push(sICDCode + "~" + sICDDesc + "~" + IsPrimary + "~" + chkICD1 + "~" + chkICD2 + "~" + chkICD3 + "~" + chkICD4 + "~" + chkICD5 + "~" + chkICD6 + "~" + chkICDContainer.cells[10].innerText.trim() + "~" + chkICDContainer.cells[11].innerText.trim() + "~" + "EMICD");

        ////    if (arrlstAssICD.indexOf(sICDCode) != -1) {
        ////        DisplayErrorMessage('530021', "", "'" + sICDCode + "'");
        ////        bSaveCheck = true;
        ////        AutoSaveUnsuccessful();
        ////        return;
        ////    }
        ////}


        //Modified  by balaji.TJ 
        for (var jICD = 0; jICD < $('#tblEandMCodingICD > tbody  > tr').length; jICD++) {
            var chkICDContainer = $('#tblEandMCodingICD > tbody  > tr')[jICD];
            //var chkICD1 = ""; var chkICD2 = ""; var chkICD3 = ""; var chkICD4 = ""; var chkICD5 = ""; var chkICD6 = "";
            var IsPrimary = 'N';
            if (chkICDContainer.cells[1].children[0].checked == true) {
                IsPrimary = 'Y';
            }
            
            var ssequence = chkICDContainer.cells[2].innerText.trim();
            var sICDCode = chkICDContainer.cells[3].innerText.trim();
            var sICDDesc = chkICDContainer.cells[4].innerText.trim();            
            aryICDList.push(sICDCode + "~" + sICDDesc + "~" + IsPrimary + "~" + chkICDContainer.cells[5].innerText.trim() + "~" + chkICDContainer.cells[6].innerText.trim() + "~" + "EMICD" + "~" + ssequence);

            if (arrlstAssICD.indexOf(sICDCode) != -1) {
                DisplayErrorMessage('530021', "", "'" + sICDCode + "'");
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }
        }
        //CAP-713 - Add Electronically Signed
        if (document.getElementById("chkElectronicallySigned").checked == false) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('7430004');
            bSaveCheck = true;
            AutoSaveUnsuccessful();
            return;
        }


        //localStorage.setItem("bSave", "true");
        //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";

        $http({
            url: "WebServices/PhoneEncounterService.asmx/SavePhoneEncounterPlanEandMCoding",
            dataType: 'json',
            method: 'POST',
            data: JSON.stringify({
                arylstCPT: aryCPTList, arylstICD: aryICDList, arylstDelCPT: DeleteArray,
                arylstDelICD: DeleteArrayICD, ulHumanID: HumanID,
                //sCallHrs: document.getElementById('txtCallHrs').value,
                //sCallMins: document.getElementById('txtCallMins').value,
                sCallMins: $('#cboCallDuration option:selected').val(),
                sCallerName: document.getElementById('txtCallerName').value,
                sCallSpokenTo: document.getElementById('cboCallSpokenTo').value,
                sCallDate: document.getElementById('txtCalldate').value,
                sNotes: document.getElementById('txtNotes').value,
                sSubmitMode: submitmode,
                sPhyID: $('#cboSelectPhysician option:selected').attr('id'),
                sDOSPhyName: $('#cboSelectPhysician option:selected').text()
            }),
            headers: {
                "Content-Type": "application/json; charset=utf-8",
                "X-Requested-With": "XMLHttpRequest"
            }
        }).success(function (response) {
            var str = response.d;
            //var test = JSON.parse(str);

            if (str == '7430005') {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('7430005');
                bSaveCheck = true;
                AutoSaveUnsuccessful();
                return;
            }
            //$scope.EandMCodingCPTTable = test.ProcedureList;
            //$scope.EandMCodingICDTable = test.ICDList;
            //$scope.AssEandMCodingICDTable = test.AssICDlist;

            //UserRole = test.UserRole.toUpperCase();
            //$scope.orderByFieldCPT = 'CPTCode';
            //$scope.orderByFieldICD = 'ICDCode';
            //$scope.reverseSort = false;
            //EnablePriRbtn = test.EnablePriRbtn;


            $('#btnSave').attr("disabled", "disabled");
            $('#btnSaveOnly').attr("disabled", "disabled");
            //if (bSubmitted == false)
            //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('110095');
            //localStorage.setItem("bSave", "true");
            //window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            bSaveCheck = false;
            //DisableChartLevelAutoSave();//BugID:52795
            //AutoSaveSuccessful();
            //DeleteArray = []; //BugID: 49118
            //self.close();
            // return;
            $("#btnClose").click();
            //Jira Cap-409 - Phone encounter duplicating
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        })
        .error(function (error, status, headers, config) {

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (status == 999)
                window.location = "frmSessionExpired.aspx";
            else
                alert(error.Message + ".Please Contact Support!");
        });
    }

    $scope.btnSubAllForSuperbill_Click = function (index, submitmode) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#btnSave')[0].disabled == false) {
            bSubmitted = true;
            bSaveCheck = false;
            $scope.SaveEandMCoding(index, submitmode);
            if (bSaveCheck) {
                bSubmitted = false;
                return false;
            }
        }
        else {
            if ($('#tblEandMCodingICD tr td input[type=checkbox]:checked').length == 0) {//BugID:48668 -- ServProc REVAMP
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('530004');

                return;
            }
        }
        //$http({
        //    url: "WebServices/EandMCodingService.asmx/ESuperbillSubmitted",
        //    dataType: 'json',
        //    method: 'POST',
        //    data: JSON.stringify({ sESuperbillSubmitted: 'Y' }),
        //    headers: {
        //        "Content-Type": "application/json; charset=utf-8",
        //        "X-Requested-With": "XMLHttpRequest"
        //    }
        //}).success(function (response) {
        //    var str = response.d;
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    //DisplayErrorMessage('230150');//save&submitalert
        //    //DisplayErrorMessage('230164');
        //    //$("#btnSubAllForSuperbill").attr("disabled", "disabled");
        //    //bSubmitted = false;
        //    if (submitmode == 'N') {
        //        DisplayErrorMessage('230165');
        //        //$("#btnSubAllForSuperbill").attr("disabled", "disabled");
        //        bSubmitted = false;
        //        document.getElementById('txtPassword').value = '';
        //        document.getElementById('btnSaveOnly').disabled = true;

        //    }
        //    else {
        //        DisplayErrorMessage('230164');
        //        // $("#btnSubAllForSuperbill").attr("disabled", "disabled");
        //        bSubmitted = false;
        //        document.getElementById('txtPassword').value = '';
        //    }

        //    return;
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //})
        //.error(function (error, status, headers, config) {

        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    if (status == 999)
        //        window.location = "frmSessionExpired.aspx";
        //    else
        //        alert(error.Message + ".Please Contact Support!");
        //});
        //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
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
                alert(error.Message + ".Please Contact Support!");
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

    $scope.txtCallHrs_OnKeyPress = function () {
        $scope.EnableSaveButton();
        setInputFilter(document.getElementById("txtCallHrs"), function (value) {
            return /^-?\d*$/.test(value);
        });
    }
    $scope.txtCallMins_OnKeyPress = function () {
        $scope.EnableSaveButton();
        setInputFilter(document.getElementById("txtCallMins"), function (value) {
            return /^-?\d*$/.test(value);
        });
    }
    function setInputFilter(textbox, inputFilter) {
        ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
            textbox.addEventListener(event, function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                } else {
                    this.value = "";
                }
            });
        });
    }
    function CheckCPTValue(CPT) {
        var iresult = false;
        for (var j = 0; j < CallDurationAutosuggestLoad.length; j++) {
            //if (lstSelectedCPT[j].split('~')[0].trim() == CPT.trim()) {
            if (CallDurationAutosuggestLoad[j].split('-')[0].trim() == CPT.trim()) {

                iresult = true;
            }
        }
        return iresult;
    }
    function CheckICDValue(ICD) {
        var iresult = true;
        for (var j = 0; j < lstSelectedICD.length; j++) {
            if (lstSelectedICD[j].split('~')[0].trim() == ICD.trim()) {
                iresult = false;
            }
        }
        return iresult;
    }




    //function AddCptCallduration() {
    //    jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display', 'block');
    //    if ($scope.EandMCodingCPTTable.length > 0 && CallDurationAutosuggestLoad != null && CallDurationAutosuggestLoad.length > 0 && lstSelectedCPT != null && lstSelectedCPT.length > 0) {
    //        var DeleteArrayTemp = [];
    //        for (var i = 1; i < $('#tblEandMCodingCPT tr').length; i++) {
    //            if (CheckCPTValue($('#tblEandMCodingCPT tr')[i].children[7].innerText.trim()) == true) {
    //                var sCPTCode = $('#tblEandMCodingCPT tr')[i].children[7].innerText.trim();
    //                var sCPTDesc = $('#tblEandMCodingCPT tr')[i].children[8].innerText.trim();
    //                var sUnits = $('#tblEandMCodingCPT tr')[i].children[9].children[0].value;
    //                var sModi1 = "";
    //                var sModi2 = "";
    //                var sModi3 = "";
    //                var sModi4 = "";
    //                DeleteArrayTemp.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + $('#tblEandMCodingCPT tr')[i].children[11].innerText.trim() + "~" + $('#tblEandMCodingCPT tr')[i].children[12].innerText.trim() + "~" + $('#tblEandMCodingCPT tr')[i].children[13].innerText.trim());

    //            }
    //        }
    //        for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
    //            for (var j = 0; j < DeleteArrayTemp.length; j++) {
    //                if ($scope.EandMCodingCPTTable[i] != null && $scope.EandMCodingCPTTable[i] != undefined) {
    //                    if (DeleteArrayTemp[j].split('~')[10] == $scope.EandMCodingCPTTable[i].Order) {
    //                        let dellength = DeleteArrayTemp.length;
    //                        $scope.EandMCodingCPTTable.splice(i, dellength);//Index,length
    //                    }
    //                }
    //            }
    //        }

    //        $scope.RefershGrid();
    //    }
    //    else if ($scope.EandMCodingCPTTable.length > 0 && CallDurationAutosuggestLoad != null && CallDurationAutosuggestLoad.length > 0) {
    //        var gridlen = $scope.EandMCodingCPTTable.length;
    //        for (var i = 0; i < gridlen ; i++) {
    //            $scope.EandMCodingCPTTable.splice(gridlen[i], 1);
    //        }
    //        $scope.RefershGrid();
    //    }
    //    var minutes;// = hours * 60;
    //    CallDurationAutosuggestLoad = [];

    //    //if (document.getElementById("txtCallHrs").value != "" && document.getElementById("txtCallMins").value == "") {
    //    //    minutes = document.getElementById("txtCallHrs").value * 60;
    //    //}
    //    //else if (document.getElementById("txtCallHrs").value == "" && document.getElementById("txtCallMins").value != "") {
    //    //    minutes = document.getElementById("txtCallMins").value;
    //    //}
    //    //else if (document.getElementById("txtCallHrs").value != "" && document.getElementById("txtCallMins").value != "") {
    //    //    minutes = (document.getElementById("txtCallHrs").value * 60) + document.getElementById("txtCallMins").value;
    //    //}

    //    if (CallDurationStaticlookup != null && CallDurationStaticlookup.length > 0) {

    //        for (var i = 0; i < CallDurationStaticlookup.length; i++) {
    //            if (CallDurationStaticlookup[i].Description.indexOf('-') > -1) {
    //                var min = CallDurationStaticlookup[i].Description.split('-')[0];
    //                var max = CallDurationStaticlookup[i].Description.split('-')[1];
    //                if (parseInt(minutes) >= parseInt(min) && parseInt(minutes) <= parseInt(max)) {
    //                    CallDurationAutosuggestLoad.push(CallDurationStaticlookup[i].Value);
    //                }
    //            }
    //            else if (CallDurationStaticlookup[i].Description.indexOf('>=') > -1) {
    //                if (parseInt(minutes) >= parseInt(CallDurationStaticlookup[i].Description.replace('>=', ""))) {
    //                    CallDurationAutosuggestLoad.push(CallDurationStaticlookup[i].Value);
    //                }
    //            }
    //            else if (CallDurationStaticlookup[i].Description.indexOf('<=') > -1) {
    //                if (parseInt(minutes) <= parseInt(CallDurationStaticlookup[i].Description.replace('<=', ""))) {
    //                    CallDurationAutosuggestLoad.push(CallDurationStaticlookup[i].Value);
    //                }
    //            }
    //            else if (parseInt(minutes) == parseInt(CallDurationStaticlookup[i].Description)) {
    //                CallDurationAutosuggestLoad.push(CallDurationStaticlookup[i].Value);
    //            }


    //        }

    //        var unique = CallDurationAutosuggestLoad.filter(onlyUnique);

    //        if (unique != null && unique.length > 0) {
    //            var CPTCode = "";
    //            for (var j = 0; j < unique.length; j++) {
    //                if (j == 0)
    //                    CPTCode = unique[j].split('-')[0];
    //                else

    //                    CPTCode = CPTCode + "|" + unique[j].split('-')[0];
    //            }

    //            $.ajax({
    //                type: "POST",
    //                url: "WebServices/EandMCodingService.asmx/GetModifierforCPT",
    //                contentType: "application/json; charset=utf-8",
    //                data: JSON.stringify({
    //                    "CPT": unique[0].split('-')[0],
    //                    "CPTList": CPTCode
    //                }),
    //                dataType: "json",
    //                async: false,
    //                success: function (data) {
    //                    var Modifier = (data.d);
    //                    var MaxOrderList = new Array();
    //                    var iOrder;
    //                    if ($scope.EandMCodingCPTTable.length != 0) {
    //                        for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
    //                            MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
    //                        }
    //                    }
    //                    else {
    //                        MaxOrderList.push("0");
    //                    }
    //                    MaxOrderList.sort();
    //                    for (var k = 0; k < unique.length; k++) {
    //                        for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
    //                            MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
    //                        }
    //                        iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
    //                        if (iOrder == 0)
    //                            iOrder = 1;

    //                        $scope.EandMCodingCPTTable.push({ 'CPTCode': unique[k].split('-')[0], 'CPTDesc': unique[k].split('-')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
    //                        // lstSelectedCPT.push(ui.item.label);

    //                    }
    //                    $scope.RefershGrid();
    //                    BindCPTtable();
    //                    $scope.EnableSaveButton();
    //                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //                },
    //                error: function OnError(xhr) {
    //                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //                    if (xhr.status == 999)
    //                        window.location = xhr.statusText;
    //                    else {
    //                        var log = JSON.parse(xhr.responseText);
    //                        console.log(log);
    //                        alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
    //                            ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
    //                            log.ExceptionType + " \nMessage: " + log.Message);
    //                        return false;
    //                    }
    //                }
    //            });

    //        }




    //    }
    //    else {
    //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    //    }

    //}
    function onlyUnique(value, index, self) {
        return self.indexOf(value) === index;
    }
    var removeByAttr = function (arr, attr, value) {
        var i = arr.length;
        while (i--) {
            if (arr[i]
                && arr[i].hasOwnProperty(attr)
                && (arguments.length > 2 && arr[i][attr] === value)) {
                arr.splice(i, 1);
            }
        }
        return arr;
    }

    function cboCalldurationselectedindex() {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display', 'block');
        if ($scope.EandMCodingCPTTable.length > 0 && CallDurationAutosuggestLoad != null && CallDurationAutosuggestLoad.length > 0 && lstSelectedCPT != null && lstSelectedCPT.length > 0) {
            var DeleteArrayTemp = [];
            for (var i = 1; i < $('#tblEandMCodingCPT tr').length; i++) {
                if (CheckCPTValue($('#tblEandMCodingCPT tr')[i].children[7].innerText.trim()) == true) {
                    var sCPTCode = $('#tblEandMCodingCPT tr')[i].children[7].innerText.trim();
                    var sCPTDesc = $('#tblEandMCodingCPT tr')[i].children[8].innerText.trim();
                    var sUnits = $('#tblEandMCodingCPT tr')[i].children[9].children[0].value;
                    var sModi1 = "";
                    var sModi2 = "";
                    var sModi3 = "";
                    var sModi4 = "";
                    DeleteArrayTemp.push(sCPTCode + "~" + sCPTDesc + "~" + sUnits + "~" + sModi1 + "~" + sModi2 + "~" + sModi3 + "~" + sModi4 + "~" + "" + "~" + $('#tblEandMCodingCPT tr')[i].children[11].innerText.trim() + "~" + $('#tblEandMCodingCPT tr')[i].children[12].innerText.trim() + "~" + $('#tblEandMCodingCPT tr')[i].children[13].innerText.trim());

                }
                else {


                }
            }
            for (var i = 0; i < $scope.EandMCodingCPTTable.length; i++) {
                for (var j = 0; j < DeleteArrayTemp.length; j++) {
                    if ($scope.EandMCodingCPTTable[i] != null && $scope.EandMCodingCPTTable[i] != undefined) {
                        if (DeleteArrayTemp[j].split('~')[10] == $scope.EandMCodingCPTTable[i].Order) {
                            let dellength = DeleteArrayTemp.length;
                            $scope.EandMCodingCPTTable.splice(i, dellength);//Index,length
                        }
                    }
                }
            }

            $scope.RefershGrid();
        }
        else if ($scope.EandMCodingCPTTable.length > 0 && CallDurationAutosuggestLoad != null && CallDurationAutosuggestLoad.length > 0) {
            var gridlen = $scope.EandMCodingCPTTable.length;
            for (var i = 0; i < gridlen ; i++) {
                $scope.EandMCodingCPTTable.splice(gridlen[i], 1);
            }
            $scope.RefershGrid();
        }
        var minutes;
        CallDurationAutosuggestLoad = [];

        if (document.getElementById("cboCallDuration").value != "") {
            minutes = document.getElementById("cboCallDuration").value;
        }
        if (CallDurationStaticlookup != null && CallDurationStaticlookup.length > 0) {

            for (var i = 0; i < CallDurationStaticlookup.length; i++) {
                if (CallDurationStaticlookup[i].Description == minutes) {
                    CallDurationAutosuggestLoad.push(CallDurationStaticlookup[i].Value);
                }
            }
        }

        var unique = CallDurationAutosuggestLoad.filter(onlyUnique);

        if (unique != null && unique.length > 0) {
            var CPTCode = "";
            for (var j = 0; j < unique.length; j++) {
                if (j == 0)
                    CPTCode = unique[j].split('-')[0];
                else

                    CPTCode = CPTCode + "|" + unique[j].split('-')[0];
            }
            //Modified by balaji.TJ

            var Modifier =''; //(data.d);
            var MaxOrderList = new Array();
            var iOrder;
            if ($scope.EandMCodingCPTTable.length != 0) {
                for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                    MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                }
            }
            else {
                MaxOrderList.push("0");
            }
            MaxOrderList.sort();
           
            for (var k = 0; k < unique.length; k++) {
                
                for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
                    MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
                }
                iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
                if (iOrder == 0)
                    iOrder = 1;

                $scope.EandMCodingCPTTable.push({ 'CPTCode': unique[k].split('-')[0], 'CPTDesc': unique[k].split('-')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
                
            }
            $scope.RefershGrid();
            BindCPTtable();
            $scope.EnableSaveButton();

            //Comment by balaji.TJ

            //$.ajax({
            //    type: "POST",
            //    url: "WebServices/EandMCodingService.asmx/GetModifierforCPT",
            //    contentType: "application/json; charset=utf-8",
            //    data: JSON.stringify({
            //        "CPT": unique[0].split('-')[0],
            //        "CPTList": CPTCode
            //    }),
            //    dataType: "json",
            //    async: false,
            //    success: function (data) {
            //        var Modifier = (data.d);
            //        var MaxOrderList = new Array();
            //        var iOrder;
            //        if ($scope.EandMCodingCPTTable.length != 0) {
            //            for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
            //                MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
            //            }
            //        }
            //        else {
            //            MaxOrderList.push("0");
            //        }
            //        MaxOrderList.sort();
            //        //iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + k;
            //        //var icount = 0;
            //        // $scope.EandMCodingCPTTable.push({ 'CPTCode': ui.item.label.split('~')[0], 'CPTDesc': ui.item.label.split('~')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
            //        for (var k = 0; k < unique.length; k++) {
            //            //if (k == 0) {
            //            //    icount = 1;
            //            //}
            //            //else {
            //            //    icount++;
            //            //}
            //            //iOrder =  parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + icount ;
            //            for (var iMax = 0; iMax < $scope.EandMCodingCPTTable.length; iMax++) {
            //                MaxOrderList.push($scope.EandMCodingCPTTable[iMax].Order);
            //            }
            //            iOrder = parseInt(MaxOrderList.sort()[MaxOrderList.length - 1]) + 1;
            //            if (iOrder == 0)
            //                iOrder = 1;

            //            $scope.EandMCodingCPTTable.push({ 'CPTCode': unique[k].split('-')[0], 'CPTDesc': unique[k].split('-')[1], 'EandMCPTID': '', 'Units': '1', 'Modifier1': Modifier, 'Modifier2': '', 'Modifier3': '', 'Modifier4': '', 'CPTCheck': '6', 'CPTVersion': '', 'btnDelete': 'Resources/Delete-Blue.png', 'Order': iOrder });
            //            // lstSelectedCPT.push(ui.item.label);

            //        }
            //        $scope.RefershGrid();
            //        BindCPTtable();
            //        $scope.EnableSaveButton();
            //        // { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //    },
            //    error: function OnError(xhr) {
            //        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //        if (xhr.status == 999)
            //            window.location = xhr.statusText;
            //        else {
            //            var log = JSON.parse(xhr.responseText);
            //            console.log(log);
            //            alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
            //                ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
            //                log.ExceptionType + " \nMessage: " + log.Message);
            //            return false;
            //        }
            //    }
            //});

            //}




        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }

    }
    function CboPhysicianChange() {
        EnableSave();
        //var vHumanID = document.getElementById('txtAccountNumber').value;
        var vselectPhyId = $('#cboSelectPhysician option:selected').attr('value');
        if (vselectPhyId != undefined && vselectPhyId != "") {
            $.ajax({
                type: "POST",
                async: true,
                url: "WebServices/PhoneEncounterService.asmx/GetAssessmentDetails",
                data: "{'uEncounterID':'" + vselectPhyId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (sdata) {
                    //Delete last autosuggest ICD
                    if ($scope.EandMCodingICDTable.length > 0 && lstSelectedICD != null && lstSelectedICD.length > 0) {
                        var DeleteArrayTempICD = [];
                        var iindex;
                        for (var i = 0; i < $scope.EandMCodingICDTable.length; i++) {
                            //CAP-1471
                            if (CheckICDValue($('#tblEandMCodingICD tbody tr')[i]?.children[8]?.innerText?.trim()) == true) {
                                DeleteArrayTempICD.push($('#tblEandMCodingICD tbody tr')[i]?.children[8]?.innerText?.trim());
                                iindex = i;
                            }
                        }
                        for (var i = 0; i < $scope.EandMCodingICDTable.length; i++) {
                            for (var j = 0; j < DeleteArrayTempICD.length; j++) {
                                if (DeleteArrayTempICD[j] == $('#tblEandMCodingICD tbody tr')[i].children[8].innerText.trim()) {
                                    // $('#tblEandMCodingICD tbody tr')[i].remove();

                                    $("#tblEandMCodingICD tbody tr").find('td#ICD').each(function () {
                                        if (DeleteArrayTempICD[j] == $(this)[0].innerText.trim()) {
                                            //$(this).parents("tr")[0].remove();
                                            //// const val=  $scope.EandMCodingICDTable.filter(item=>item.ICDCode.trim() !== $(this)[0].innerText.trim());
                                            //$scope.EandMCodingICDTable.splice($(this).parents("tr")[0], 1);
                                            // $scope.EandMCodingICDTable.splice(iindex, 1);
                                            var vindex = removeByAttr($scope.EandMCodingICDTable, "ICDCode", $(this)[0].innerHTML);
                                            // console.log(vindex);
                                        }

                                    });



                                    //let dellength = $scope.EandMCodingICDTable.length;
                                    //let arrylen = $scope.EandMCodingICDTable.length;
                                    //$scope.EandMCodingICDTable.splice(dellength[i], arrylen);//Index,length
                                }
                                //  break;
                            }
                            // break;
                        }
                        $scope.RefershGrid();
                    }
                    else if ($scope.EandMCodingICDTable.length > 0) {
                        var gridlen = $scope.EandMCodingICDTable.length;
                        for (var i = 0; i < gridlen ; i++) {
                            $scope.EandMCodingICDTable.splice(gridlen[i], 1);
                        }
                        $scope.RefershGrid();
                    }
                    if (sdata.d.length > 0) {
                        //Add Autosuggest primary ICD from assessment
                        $scope.EandMCodingICDTable.push({ 'ICDCode': sdata.d[0].split('~')[1], 'ICDDescription': sdata.d[0].split('~')[2], 'ICDVersion': '0', 'btnDelete': 'Resources/Delete-Blue.png', 'IsPrimary': sdata.d[0].split('~')[4], 'EandMICDID': '0', 'Sequence': '6', 'EnablePriRbtn': sdata.d[0].split('~')[7] });
                        $scope.RefershGrid();
                    }
                    $scope.EnableSaveButton();
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
            // document.getElementById("txtFacilityDOS").innerHTML = "";
            //document.getElementById("txtFacilityDOS").title = "";
        }

    }

});

window.addEventListener("contextmenu",
  function (e) {
      e.stopPropagation()
  }, true);
function followupKeypress() {
    event.preventDefault();

}


function btnpatientChart_Click() {
    var queryString = window.location.search.toString().split('?')[1];
    if (HumanID != "") {
        humanid = HumanID;
    }
    else if (queryString != undefined && queryString != "") {
        humanid = queryString.split('|')[1];
    }

    //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    //Cap - 891
    //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&ScreenMode=Menu&openingfrom=Menu&from=viewresult", 1000, 1500, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;


}
//function EnableSaveandSubmit() {
//    $find('btnSave').set_enabled(true);
//    $find('btnSaveOnly').set_enabled(true);

//}



/*function loadPhysicianCombo(FacilityName, CurrentUser) {
    $("#cboSelectPhysician option").remove();
    $('#cboSelectPhysician').append("<option value=''>" + "" + "</option>");
    $.get("ConfigXML/PhysicianFacilityMapping.xml", {}, function (xml) {
        $("PhyList", xml).each(function (i) {
            $(this).find("Facility").each(function (l) {
                if (FacilityName != "") {
                    if ($(this).attr("name") == FacilityName) {
                        $(this).find('Physician').sort(function (a, b) {
                            return $(a)[0].attributes[5].value > $(b)[0].attributes[5].value
                        }).each(function (l) {

                            //$(result).find('car').sort(function(a,b){
                            //  return $(a).find('marca').text() > $(b).find('marca').text()
                            // }).each(function () {


                            if ($(this).attr("username").trim() != "") {
                                var physicianname = "";
                                if ($(this).attr('username') != undefined && $(this).attr('username').trim() != "")
                                    physicianname = $(this).attr('username') + " - ";
                                if ($(this).attr('prefix') != undefined && $(this).attr('prefix').trim() != "")
                                    physicianname += $(this).attr('prefix') + " ";
                                if ($(this).attr('firstname') != undefined && $(this).attr('firstname').trim() != "")
                                    physicianname += $(this).attr('firstname') + " ";
                                if ($(this).attr('middlename') != undefined && $(this).attr('middlename').trim() != "")
                                    physicianname += $(this).attr('middlename') + " ";
                                if ($(this).attr('lastname') != undefined && $(this).attr('lastname').trim() != "")
                                    physicianname += $(this).attr('lastname') + " ";
                                if ($(this).attr('suffix') != undefined && $(this).attr('suffix').trim() != "")
                                    physicianname += $(this).attr('suffix') + " ";

                                if (physicianname.trim() != "") {
                                    $('#cboSelectPhysician').append("<option id=" + $(this).attr('ID') + " value=" + $(this).attr('username') + ">" + physicianname + "</option>");

                                    //Select latest appt PHYSICIANID or login physician
                                    if (UserRole.toUpperCase() == "PHYSICIAN" && $('#hdnProvID').val() != null && $('#hdnProvID').val() != "" && $('#hdnProvID').val() == $(this).attr('ID')) {
                                        $('#cboSelectPhysician').val($(this).attr('username'));
                                    }
                                }
                            }
                        });
                    }
                }
                else {
                    $(this).find('Physician').sort(function (a, b) {
                        return $(a)[0].attributes[5].value > $(b)[0].attributes[5].value
                    }).each(function (l) {
                        if ($(this).attr("username").trim() != "") {
                            var physicianname = "";
                            if ($(this).attr('username') != undefined && $(this).attr('username').trim() != "")
                                physicianname = $(this).attr('username') + " - ";
                            if ($(this).attr('prefix') != undefined && $(this).attr('prefix').trim() != "")
                                physicianname += $(this).attr('prefix') + " ";
                            if ($(this).attr('firstname') != undefined && $(this).attr('firstname').trim() != "")
                                physicianname += $(this).attr('firstname') + " ";
                            if ($(this).attr('middlename') != undefined && $(this).attr('middlename').trim() != "")
                                physicianname += $(this).attr('middlename') + " ";
                            if ($(this).attr('lastname') != undefined && $(this).attr('lastname').trim() != "")
                                physicianname += $(this).attr('lastname') + " ";
                            if ($(this).attr('suffix') != undefined && $(this).attr('suffix').trim() != "")
                                physicianname += $(this).attr('suffix') + " ";

                            if (physicianname.trim() != "") {
                                $('#cboSelectPhysician').append("<option id=" + $(this).attr('ID') + " value=" + $(this).attr('username') + ">" + physicianname + "</option>");
                                //Select latest appt PHYSICIANID or login physician
                                if (UserRole.toUpperCase() == "PHYSICIAN" && $('#hdnProvID').val() != null && $('#hdnProvID').val() != "" && $('#hdnProvID').val() == $(this).attr('ID')) {
                                    $('#cboSelectPhysician').val($(this).attr('username'));
                                }
                            }
                        }
                    });
                }
            });
        });
    });


    var sel = $('#cboSelectPhysician');
    var selected = sel.val(); // cache selected value, before reordering
    var opts_list = sel.find('option');

    opts_list.sort(function (a, b) { return a.text == b.text ? 0 : a.text < b.text ? -1 : 1; });
    sel.html('').append(opts_list);
    sel.val(selected);
}*/





$.fn.getType = function () { return this[0].tagName == "INPUT" ? this[0].type.toLowerCase() : this[0].tagName.toLowerCase(); }
var hdnFollowup = null;
function FollowupNotes(icon, List, id) {
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
    if (icon.className.indexOf("plus") > -1) {
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
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
                var targetControlValue = id;
                var innerdiv = '';
                var pos = $('#' + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:45541
                if (UserRole.toUpperCase() == 'PHYSICIAN' || UserRole.toUpperCase() == 'PHYSICIAN ASSISTANT') {
                    innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].attributes.getNamedItem('data-src').value + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none;cursor:default; list-style-type: none;color:black' onclick=\"FollowUp('" + values[i].replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45541
                }

                var listlength = innerdiv.length;
                if (listlength > 0) {

                    if (document.getElementById(id).value != "")
                        var txtValue = document.getElementById(id).value;
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    //BugID:49036 - Decreased height for follow-up notes
                    $("<div  class='Listdiv' id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                      .css({
                          top: pos.top + $("#" + targetControlValue).height() + 7,
                          left: pos.left + 1,
                          height: '106px',
                          overflow: 'scroll',
                          position: 'fixed',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '451px',
                          border: '1px solid #8e8e8e',
                          background: '#FFF',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto'

                      })

                        .insertAfter($("#" + targetControlValue));
                    // }
                }
                //EnableSave();
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

    if (hdnFollowup != null && hdnFollowup != icon) {

        $(hdnFollowup).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFollowup = icon;
    // if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
    $('#btnSave')[0].disabled = false;
    // EnableSave();
}
function FollowUp(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var value = agrulist.split("^");
    var sugglistval = $("#" + value[1]).val().trim();

    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split(",\t")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value[0]) {
                flag++;
            }
        }
        if (flag == 0) {
            $("#" + value[1]).val(sugglistval + ",\t" + value[0]);
        }
    }
    else {
        $("#" + value[1]).val(value[0]);
    }
}

function OpenPopup(KeyWord) {
    var focused = KeyWord;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}









function StartLoadFromPatChartPhoneEncounter() {
    if (sessionStorage.getItem('StartLoading') == 'true') {
        if (jQuery(window.top.parent.parent.parent.parent.document.body).find('#resultLoading').attr('id') != 'resultLoading') {
            jQuery(window.top.parent.parent.parent.parent.document.body).append('<div id="resultLoading" class="masterLoad" style="display:none"><div><img src="./Resources/loadimage.gif" style="opacity:0.7;height:30px;width:30px;"><div style="font-size:16px;padding-top:5px;padding-left:15px;">Loading...</div></div><div class="bg"></div></div>');
        }
        else {
            jQuery(window.top.parent.parent.parent.parent.document.body).find('#resultLoading').remove();
            jQuery(window.top.parent.parent.parent.parent.document.body).append('<div id="resultLoading" class="masterLoad" style="display:none"><div><img src="./Resources/loadimage.gif" style="opacity:0.7;height:30px;width:30px;"><div style="font-size:16px;padding-top:5px;padding-left:15px;">Loading...</div></div><div class="bg"></div></div>');
        }
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css({
            'display': 'block',
            'width': '100%',
            'height': '100%',
            'position': 'fixed',
            'z-index': '10000000',
            'top': '0',
            'left': '0',
            'right': '0',
            'bottom': '0',
            'margin': 'auto'
        });
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').css({
            'background': '#ffffff',
            'opacity': '0.7',
            'width': '100%',
            'height': '100%',
            'position': 'absolute',
            'top': '0'
        });
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading>div:first').css({
            'width': '250px',
            'height': '75px',
            'text-align': 'center',
            'position': 'fixed',
            'top': '0',
            'left': '0',
            'right': '0',
            'bottom': '0',
            'margin': 'auto',
            'font-size': '16px',
            'z-index': '10',
            'color': '#000000'
        });
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading .bg').height('100%');
        jQuery(window.top.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').fadeIn(300);
        //jQuery(window.top.parent.parent.parent.parent.document.body).css('cursor', 'wait');
    }
}


function loadPhysicianCombo(data) {
    $("#cboSelectPhysician option").remove();
    $('#cboSelectPhysician').append("<option id='0' value='0'></option>");
    if (data.length > 0) {
        for (var i = 0; i < data.length ; i++) {
            $('#cboSelectPhysician').append("<option id=" + data[i].split('|')[2] + " value=" + data[i].split('|')[0] + ">" + data[i].split('|')[1] + "</option>");
        }
        //if (UserRole.toUpperCase() == "PHYSICIAN") {
        $('#cboSelectPhysician').val(data[0].split('|')[0]);
        // }
    }
}

function GetEncdetails(isShowAll) {
    var vHumanID = HumanID; //document.getElementById('txtAccountNumber').value;
    $.ajax({
        type: "POST",
        async: true,
        url: "WebServices/PhoneEncounterService.asmx/GetAllEncountersFromHumanXml",
        data: "{'ulHumanID':'" + vHumanID + "','sNoOfDays':'" + isShowAll + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (sdata) {

            loadPhysicianCombo(sdata.d);

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




function PhysicianShowall() {
    if ($('input[id="chkPhyShowall"]:checked').length > 0) {
        GetEncdetails("");
    } else {
        GetEncdetails("90");
    }
}