function warningmethod() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
var date_diff_indays = function (date1, date2) {
    dt1 = new Date(date1);
    dt2 = new Date(date2);
    return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24));
}
var deleteVariable = "Del";
var editVariable = "EditRow";
var statusid = [];

var popup;

function btnopenpatient_Clicked() {
    popup = window.open("frmPatientChart.aspx?HumanID=140050", "mypopup", "width=500,height=300");
}

function myClickHandler() {
    $("div.bootstrap-datetimepicker-widget").css("display", "none");
}
function Display(e) {
    //Cap - 804
    if (e.currentTarget.id.indexOf('(') < 0 && e.currentTarget.id.indexOf(')')<0)
    {
        $("div.bootstrap-datetimepicker-widget").css("display", "block");
        var pos = $("#" + e.currentTarget.id).offsetParent();
        var posheight = $("#" + e.currentTarget.id).position();
        var postop = pos[0].scrollTop + posheight.top;

        $("div.bootstrap-datetimepicker-widget").css({ top: postop + 15 + "px", left: posheight.left + "px", height: 300 + "px", width: 440 + "px", bgcolor: "#FFFFFF" });
        $(".bootstrap-datetimepicker-widget .timepicker-hour").css("margin-left", "15px");
        $(".bootstrap-datetimepicker-widget .timepicker-minute").css("margin-left", "15px");
        $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
        $("div.bootstrap-datetimepicker-widget").css('z-index', 3000);
    }
}
function DisplayDate(e) {

    $("div.bootstrap-datetimepicker-widget").css("display", "block");
    var pos = $("#" + e.currentTarget.id).offsetParent();
    var posheight = $("#" + e.currentTarget.id).position();
    var postop = pos[0].scrollTop + posheight.top;
    $("div.bootstrap-datetimepicker-widget").css({ top: postop + 15 + "px", left: posheight.left + "px", height: 300 + "px", width: 440 + "px", bgcolor: "#FFFFFF" });
    $(".bootstrap-datetimepicker-widget .timepicker-minute").css("margin-left", "15px");
    $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
    $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
    $("div.bootstrap-datetimepicker-widget").css('z-index', 3000);
    localStorage.setItem("bSave", "false");
    $('#btnSaveVitals').prop('disabled', false);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;

}
function Displayalert(ctrlid) {

    $("div.bootstrap-datetimepicker-widget").css("display", "block");
    var pos = $("#" + ctrlid).offsetParent();
    var posheight = $("#" + ctrlid).position();
    var postop = pos[0].scrollTop + posheight.top;
    $("div.bootstrap-datetimepicker-widget").css({ top: postop + 15 + "px", left: posheight.left + "px", height: 300 + "px", width: 440 + "px", bgcolor: "#FFFFFF" });
    $(".bootstrap-datetimepicker-widget .timepicker-minute").css("margin-left", "15px");
    $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
    $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
    $("div.bootstrap-datetimepicker-widget").css('z-index', 3000);
}


function LoadValues() {
}
function ChangeTextSpinner() {
    $(".NumericUpDown").spinner({
        max: 300, min: 35, down: "custom-down-icon", up: "custom-up-icon", spin: function (event, ui) {
            $('#btnSaveVitals').prop('disabled', false);
        }
    });

}
function CheckMaxValue(minvalue, maxvalue, id) {
    var ctrl = document.getElementById(id);//"input[id *=" + id + "]";
    var textvalue = ctrl.value; 
    if (textvalue != "") {
        if (parseInt(textvalue) < parseInt(minvalue) || parseInt(textvalue) > parseInt(maxvalue)) {
            if (parseInt(textvalue) < parseInt(minvalue))
                ctrl.value = minvalue;
            else
                ctrl.value = maxvalue;

            if (id.indexOf('Diastolic') > 0) {
                DisplayErrorMessage('200030', '', minvalue + "-" + maxvalue + "-" + "BP Sitting Dia");
            } else {
                DisplayErrorMessage('200030', '', minvalue + "-" + maxvalue + "-" + "BP Sitting Sys");
            }
        }
    }
}
function OpenPastVitals() {
    $("#PastVitals").modal({ backdrop: "static" });
    //CAP-779 Cannot read properties of null
    if (document.getElementById('PastVitalsFrame') != undefined && document.getElementById('PastVitalsFrame') != null && $('#PastVitalsFrame')[0]?.contentDocument != undefined && $('#PastVitalsFrame')[0]?.contentDocument != null) {
        $('#PastVitalsFrame')[0].contentDocument.location.href = "frmVitalsHistory.aspx";
    }
    $("#PastVitalsTitle")[0].textContent = "Past Vitals";
}

function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    //  childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
}

function AllowNumbers(evt) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function ClearAll(e) {
    statusid = [];
    var buttontxt = $("#" + e).attr("value");
    if (buttontxt == undefined || buttontxt == "Clear All") {
        if (DisplayErrorMessage('200005') == true) {
            //CAP-2022
            sessionStorage.setItem("EncCancel", "false");
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var ctrl = document.getElementsByTagName('INPUT');
            var element;
            for (var i = 0; i < document.forms[0].elements.length; i++) {
                element = document.forms[0].elements[i];

                switch (element.type) {
                    case 'text':
                        if (element.value.length > 0) {
                            if (document.getElementById(element.id + " Status") != null) {
                                var Status = document.getElementById(element.id + " Status");
                                Status.value = "";
                            }
                            if (document.getElementById(element.id + "Status") != null) {
                                var Status = document.getElementById(element.id + "Status");
                                Status.value = "";
                            }
                            else if (document.getElementById(element.id.replace("Second", "") + " StatusSecond") != null) {
                                var Status = document.getElementById(element.id.replace("Second", "") + " StatusSecond");
                                Status.value = "";
                            }
                            if (element.id.indexOf("cbo") != -1) {
                                if (document.getElementById(element.id.replace("_Input", "")) != null) {
                                    var Combo = document.getElementById(element.id.replace("_Input", ""));
                                    Combo.clearSelection();
                                }
                            }
                        }
                        break;
                    case 'select-one':
                        if (element.selectedIndex > 0) {
                            element.selectedIndex = 0;
                        }
                        break;
                    case 'checkbox':
                        if (element.id.indexOf("Second") == -1) {
                            if (element.id.indexOf("Left") != -1 && element.id.indexOf("Second") == -1) {
                                element.checked = true;
                            } else {
                                element.checked = false;
                            }
                        } else {
                            if (element.id.indexOf("Second") != -1 && element.id.indexOf("Right") != -1) {
                                element.checked = true;
                            } else {
                                element.checked = false;
                            }
                        }
                        break;
                }
            }
            for (var i = 0; i < ctrl.length; i++) {
                try {
                    var textField = document.getElementById(ctrl[i].id.replace("_ClientState", ""));
                    if (textField != null) {
                        if (ctrl[i].id.indexOf("dtp") == -1)
                            textField.clear();
                    }
                } catch (Error) { }
            }
            // $find('btnSave').set_enabled(true);
            $('#btnSaveVitals').prop('disabled', false);
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
            $("[id*='lblVitalLatest']").css("display", "none");
            $("[id*='lblLatest ']").css("display", "none");
            document.getElementById('btnClear').click();
        }
    }
    else if (buttontxt == undefined || buttontxt == "Cancel") {
        if (DisplayErrorMessage('180049') == true) {
            //CAP-2022
            sessionStorage.setItem("EncCancel", "false");
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var ctrl = document.getElementsByTagName('INPUT');
            var element;

            for (var i = 0; i < document.forms[0].elements.length; i++) {
                element = document.forms[0].elements[i];

                switch (element.type) {
                    case 'text':
                        if (element.value.length > 0) {
                            if (document.getElementById(element.id + " Status") != null) {
                                var Status = document.getElementById(element.id + " Status");
                                Status.value = "";
                            } else if (document.getElementById(element.id.replace("Second", "") + " StatusSecond") != null) {
                                var Status = document.getElementById(element.id.replace("Second", "") + " StatusSecond");
                                Status.value = "";
                            }
                            if (element.id.indexOf("cbo") != -1) {
                                if (document.getElementById(element.id.replace("_Input", "")) != null) {
                                    var Combo = document.getElementById(element.id.replace("_Input", ""));
                                    Combo.clearSelection();
                                }
                            }
                        }
                        break;
                    case 'select-one':
                        if (element.selectedIndex > 0) {
                            element.selectedIndex = 0;
                        }
                        break;
                    case 'checkbox':
                        if (element.id.indexOf("Second") == -1) {
                            if (element.id.indexOf("Left") != -1 && element.id.indexOf("Second") == -1) {
                                element.checked = true;
                            } else {
                                element.checked = false;
                            }
                        } else {
                            if (element.id.indexOf("Second") != -1 && element.id.indexOf("Right") != -1) {
                                element.checked = true;
                            } else {
                                element.checked = false;
                            }
                        }
                        break;
                }
            }
            for (var i = 0; i < ctrl.length; i++) {
                try {
                    var textField = document.getElementById(ctrl[i].id.replace("_ClientState", ""));
                    if (textField != null) {
                        if (ctrl[i].id.indexOf("dtp") == -1)
                            textField.clear();
                    }
                } catch (Error) { }
            }
            $('#btnSaveVitals').prop('disabled', true);
            return true;

        } else {
            return false;

        }
    }


}
function clearLatestLabResult() {
    $("[id*='lblVitalLatest']").css("display", "none");

    $("[id*='lblLatest ']").css("display", "none");
}
function enableField(ChkValue) {
    var pcontrol = document.getElementById(ChkValue);
    if (pcontrol.name.indexOf("Left") != -1) {
        if (pcontrol.checked == true) {
            var CControl = pcontrol.id.replace("Left", "Right");
            document.getElementById(CControl).checked = false;
        }
    } else {
        if (pcontrol.checked == true) {
            var CControl = pcontrol.id.replace("Right", "Left");
            document.getElementById(CControl).checked = false;
        }
    }
}

// Added By Ponmozhi Vendan T
function RefreshFloatingSummary() {
    var dox = window.parent.window.document;
    var iFrame = dox.getElementsByTagName("iframe");
    if (iFrame.length > 0) {
        var str = iFrame[0].src;
        var n = str.indexOf("frmFollowUpEncounter.aspx");
        if (n >= 0) {
            iFrame[0].src = iFrame[0].src;
        } else {
        }
    } else {
        //console.log("Error in finding the Floating Summary");
    }
}

function hideLoading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}


function CellSelected() {


    var Continue = DisplayErrorMessage('102006');

    if (Continue == undefined) {
        return;
    }
    else if (Continue) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        document.getElementById('InvisibleButton').click();
    }
    else {
        return;
    }


}

function LoadDefaultValues(value) {
    if (DisplayErrorMessage('200011') == true) {
        document.getElementById('btnLoadDefaultValues').click();
    }
}


function grdPastVitals_OnCommandEdit(index, commandName, args) {
    document.getElementById("hdnEdit").value = "";

    if (commandName != undefined) {

        if (args == "EditRow" || args == "Del") {
            var itemIndex = index;

            var encID = commandName;
            if (encID == "0" && args == "EditRow") {
                document.getElementById("hdnEdit").value = index;
                if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
                return true;
            }
            else if (encID != "0") {
                document.getElementById("hdnEdit").value = ""
                DisplayErrorMessage('200012');
                return false;
            }

        }

    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

}
function grdPastVitals_OnCommandDelete(index, commandName, args) {
    document.getElementById("hdnEdit").value = "";
    var encID = commandName;

    if (encID != "0") {
        document.getElementById("hdnEdit").value = ""
        DisplayErrorMessage('200012');
        return false;
    }
    else {
        document.getElementById("hdnEdit").value = index;
        return true;
    }
}

function setWaitCursor(onoff) {
    if (onoff)
        document.body.style.cursor = "wait";
    else
        document.body.style.cursor = 'default';
}

function hourglass() {
    document.body.style.cursor = "wait";
}

function ShowLoading() {
    var openingfrom = document.getElementById('hdnOpeningFrom').value;
    if (openingfrom == 'Menu')
        document.getElementById('divLoading').style.display = "block";
    else
        top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function btnSave_Clicked() {
    //CAP-937, CAP-1823
    //sessionStorage.removeItem("EncAutoSave");
    var values = $('#tblVitalControls').find('input[id ^= "dtpBP"]').map(function () {
        return this.id + "~" + this.value
    }).get();
    sessionStorage.setItem("MandatoryCheck", "");
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    nowDate = new Date();
    nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();
    nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    document.getElementById(GetClientId("hdnSystemTime")).value = nowDate;
    var controlscount = $('#tblVitalControls').find('input[type="text"]').length;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById("hdnOpeningFrom").value.toUpperCase() != "MENU") {
        var scriptAdded = document.getElementById('hdnscript').value;
        var controlid = scriptAdded.split("~");
        for (var j = 1; j < controlid.length; j++) {
            if (controlid[j].indexOf("-spin") > -1) {
                var ctrlid = controlid[j].replace("-spin", "")
                var pairedControl = ctrlid.replace("SysDia", "Diastolic");
                if ((document.getElementById(ctrlid).value != undefined && document.getElementById(pairedControl).value != undefined)
                    && (document.getElementById(pairedControl).value == ""
                     && document.getElementById("txtNotes" + ctrlid + "_txtDLC").value == '' &&
                    document.getElementById(ctrlid).value == '') || (document.getElementById(pairedControl).value != ""
                    && document.getElementById("txtNotes" + ctrlid + "_txtDLC").value == '' &&
                    document.getElementById(ctrlid).value == '') || (document.getElementById(pairedControl).value == ""
                    && document.getElementById("txtNotes" + ctrlid + "_txtDLC").value == '' &&
                    document.getElementById(ctrlid).value != '')) {
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('200031', '', ctrlid);
                    sessionStorage.setItem("MandatoryCheck", "true");
                    return false;
                }
            }
            else {
                if (document.getElementById(controlid[j]).value != undefined &&
                    document.getElementById("txtNotes" + controlid[j] + "_txtDLC").value == ''
                    && document.getElementById(controlid[j]).value == '') {
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('200031', '', controlid[j]);
                    sessionStorage.setItem("MandatoryCheck", "true");
                    return false;
                }
            }
        }

    }

    for (var i = 0; i < controlscount; i++) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        var idname = $('#tblVitalControls').find('input[type="text"]')[i].id;
        var ValidationErrName;


        if (idname != undefined && idname != "" && !(idname.toUpperCase().startsWith("LBL"))) {
            if (idname.toUpperCase().indexOf("DTP") > -1 || ((idname.toUpperCase().indexOf("CDP") == 0) && (idname.indexOf("CDPLast Mammogram") <= -1) && (idname.indexOf("CDPLast Colonoscopy") <= -1))) {
                var date1 = new Date(document.getElementById(idname).value);
                if (document.getElementById(idname).value == "" && idname.toUpperCase().indexOf("DTP") > -1) {
                    ChangeStatusLabel();
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                    DisplayErrorMessage('200040', '', idname.replace(" ", "").replace("-", ""));
                    $(top.window.document).find('#btnErrorOkCancel').unbind("click");
                    $(top.window.document).find('#btnErrorOkCancel').on("click", function () {


                        document.getElementById(idname.replace(" ", "").replace("-", "")).focus();
                        Displayalert(idname.replace(" ", "").replace("-", ""));
                    });
                    return false;



                }

                else if (new Date(document.getElementById(idname).value) > new Date()) {
                    ChangeStatusLabel();
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('200018', '', idname.replace("dtp", " "));
                    $(top.window.document).find('#btnErrorOkCancel').unbind("click");
                    $(top.window.document).find('#btnErrorOkCancel').on("click", function () {


                        document.getElementById(idname.replace(" ", "").replace("-", "")).focus();
                        Displayalert(idname.replace(" ", "").replace("-", ""));
                    });
                    return false;
                }
                else if (document.getElementById('birthdate').value != "" && (new Date(document.getElementById(idname).value) < new Date(document.getElementById('birthdate').value))) {
                    ChangeStatusLabel();
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('200023');
                    $(top.window.document).find('#btnErrorOkCancel').unbind("click");
                    $(top.window.document).find('#btnErrorOkCancel').on("click", function () {


                        document.getElementById(idname.replace(" ", "").replace("-", "")).focus();
                        Displayalert(idname.replace(" ", "").replace("-", ""));
                    });
                    return false;
                }
                    //else if ((document.getElementById(idname).value).indexOf("000") > -1 ) {//For bug id:46827
                else if (((document.getElementById(idname).value).indexOf("0001") > -1) || ((document.getElementById(idname).value).indexOf("0000") > -1)) {

                    ChangeStatusLabel();
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    DisplayErrorMessage('460011');
                    $(top.window.document).find('#btnErrorOkCancel').unbind("click");
                    $(top.window.document).find('#btnErrorOkCancel').on("click", function () {
                        document.getElementById(idname.replace(" ", "").replace("-", "")).focus();
                        Displayalert(idname.replace(" ", "").replace("-", ""));
                    });
                    return false;
                }
            }

            else if (idname.toUpperCase().startsWith("BP") && idname.toUpperCase().indexOf("DIASTOLIC") <= -1 && idname.toUpperCase().indexOf("STATUS") <= -1 && idname.toUpperCase().indexOf("TXTDLC") <= -1) {
                var idnew = idname.replace("SysDia", "Diastolic");
                if ((document.getElementById(idname).value != "" && document.getElementById(idnew).value == "") || (document.getElementById(idname).value == "" && document.getElementById(idnew).value != "")) {

                    ChangeStatusLabel();
                    AutoSaveUnsuccessful();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    //CAP-3082
                    sessionStorage.setItem("MandatoryCheck", "true");
                    alert("Please Fill both the values of " + idname);
                    return false;

                }
                else {
                    var captureddate = document.getElementById("dtp" + idname.replace(" ", "").replace("-", "")).value;
                    if (idname.toUpperCase().indexOf("SECOND") >= 0) {

                        var captureddatesecond = document.getElementById("dtp" + idname.replace(" ", "").replace("-", "")).value;
                        var captureddate = document.getElementById("dtp" + idname.replace(" ", "").replace("-", "").replace("Second", "")).value;

                        var valuesecond = document.getElementById(idname.replace(" ", "").replace("-", "")).value;
                        var value = document.getElementById(idname.replace(" ", "").replace("-", "").replace("Second", "")).value;

                        if (valuesecond != "" && value != "" && captureddatesecond == captureddate) {
                            var controlnamesecond = $('#tblVitalControls').find('input[type=radio][id *="' + idname.replace("BP", "").replace("SysDia", "") + '"]:checked').map(function () {
                                return this.id
                            }).get();

                            var controlname = $('#tblVitalControls').find('input[type=radio][id *="' + idname.replace("BP", "").replace("SysDia", "").replace("Second", "") + '"]:checked').map(function () {
                                return this.id
                            }).get();
                        }

                        if (controlnamesecond != undefined && controlname != undefined && controlnamesecond.length > 0 && controlname.length > 0) {


                            if (controlnamesecond[0].replace("Second", "") == controlname[0]) {
                                ChangeStatusLabel();
                                AutoSaveUnsuccessful();
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                alert("Captured Date and Time cannot be same for two or more BP readings.");
                                return false;
                            }
                        }

                    }
                    //for (var j = 0; j < values.length; j++) {
                    //    if (captureddate == values[j].split('~')[1] && values[j].split('~')[0] != "dtp" + idname) {
                    //        if (document.getElementById(values[j].split('~')[0].replace("dtp", "")).value != "" && document.getElementById(idname).value != "" && document.getElementById(values[j].split('~')[0].replace("dtp", "").replace("SysDia", "Diastolic")).value != "") {
                    //            ChangeStatusLabel();
                    //            AutoSaveUnsuccessful();
                    //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    //            alert("Captured Date and Time cannot be same for two or more BP readings.");
                    //            return false;
                    //        }
                    //    }

                    //}
                }
            }
            else if (idname.toUpperCase().indexOf("TEMPERATURE") > -1 || idname.toUpperCase().indexOf("PULSE") > -1 ||
                idname.toUpperCase().indexOf("FASTING") > -1 || idname.toUpperCase().indexOf("EGFR") > -1) {
                if (idname.toUpperCase().indexOf("SECOND") > -1 && idname.toUpperCase().indexOf("TXTDLC") <= -1
                    && idname.toUpperCase().indexOf("STATUS") <= -1
                    && idname.toUpperCase().indexOf("DTP") <= -1) {
                    var idnew = idname.replace("Second", "");
                    if ((document.getElementById(idname).value != "" && document.getElementById(idnew).value != "")
                        && (document.getElementById("dtp" + idname.replace(" ", "").replace("-", "")).value == document.getElementById("dtp" + idnew.replace(" ", "").replace("-", "")).value)) {
                        ChangeStatusLabel();
                        AutoSaveUnsuccessful();
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('200027', '', idnew.replace(" ", "").replace("-", ""));
                        $(top.window.document).find('#btnErrorOkCancel').unbind("click");
                        $(top.window.document).find('#btnErrorOkCancel').on("click", function () {
                            Displayalert("dtp" + idname.replace(" ", "").replace("-", ""));
                            document.getElementById("dtp" + idnew.replace(" ", "").replace("-", "")).focus();
                        });
                        return false;
                    }
                }
            }
            else if (idname.toUpperCase().indexOf("PRANDIAL") > -1) {
                if (idname.toUpperCase().indexOf("SECOND") > -1 && idname.toUpperCase().indexOf("TXTDLC") <= -1 && idname.toUpperCase().indexOf("STATUS") <= -1
                   && idname.toUpperCase().indexOf("DTP") <= -1) {
                    var idnew = idname.replace("Second", "");
                    if (document.getElementById(idname).value != "" && document.getElementById(idnew).value != ""
                        && document.getElementById("dtp" + idname.replace(" ", "").replace("-", "").replace(" ", "")).value == document.getElementById("dtp" + idnew.replace(" ", "").replace("-", "").replace(" ", "")).value) {
                        ChangeStatusLabel();
                        AutoSaveUnsuccessful();
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('200027', '', idnew.replace(" ", "").replace("-", "").replace(" ", ""));

                        $(top.window.document).find('#btnErrorOkCancel').unbind("click");
                        $(top.window.document).find('#btnErrorOkCancel').on("click", function () {
                            Displayalert("dtp" + idname.replace(" ", "").replace("-", "").replace(" ", ""));
                            document.getElementById("dtp" + idnew.replace(" ", "").replace("-", "").replace(" ", "")).focus();
                        });
                        return false;

                    }

                }
            }
            else if (idname.toUpperCase().indexOf("VISION") > -1) {

                if ((document.getElementById('Vision-Left').value != "" &&
                 document.getElementById('Vision-Lefts').value == "") || (document.getElementById('Vision-Left').value == "" &&
                 document.getElementById('Vision-Lefts').value != "")) {
                    AutoSaveUnsuccessful();
                    alert("Please enter both value in Vision Left");
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();

                    return false;
                }
                else if ((document.getElementById('Vision-Right').value != "" &&
                 document.getElementById('Vision-Rights').value == "") || (document.getElementById('Vision-Right').value == "" &&
                 document.getElementById('Vision-Rights').value != "")) {
                    AutoSaveUnsuccessful();
                    alert("Please enter both value in Vision Right");
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    return false;
                }
                else if ((document.getElementById('Vision-Both').value != "" &&
                 document.getElementById('Vision-Boths').value == "") || (document.getElementById('Vision-Both').value == "" &&
                 document.getElementById('Vision-Boths').value != "")) {
                    AutoSaveUnsuccessful();
                    alert("Please enter both value in Vision Both");
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    return false;
                }
            }
        }
    }
    //BugID:47706
    var i = 0;
    $(".mask").each(function () {
        var inputText = $(this).val();
        if ($(this).val() == "")
            inputText = "____-___-__";
        else if ($(this).val().split('-').length == 1)
            inputText = $(this).val() + "-___-__";
        else if ($(this).val().split('-').length == 2)
            inputText = $(this).val() + "-__";
        var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;
        if (inputText != "____-___-__") {
            if (inputText.match(FormatDDMMMYYYY)) {
                var DateMonthYear = inputText.split('-');
                if (DateMonthYear[0].length < 4) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    // $(this)[0].value = "____-___-__";
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                }
                if (DateMonthYear[0].length = 4 && parseInt(DateMonthYear[0]) < 1900) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    // $(this)[0].value = "____-___-__";
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                }
                if (DateMonthYear[1].length < 3) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    //  $(this)[0].value.clear();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                }
                if (DateMonthYear[2].length < 2) {
                    alert('Invalid date format!');
                    this.focus();
                    $(this)[0].value = "____-___-__";
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                }

                if (document.getElementById('birthdate').value != "" && (new Date(inputText) < new Date(document.getElementById('birthdate').value))) {
                    alert('Date cannot be Less than Birth Date');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                }
                lopera2 = DateMonthYear.length;
                var DateInput = parseInt(DateMonthYear[2]);
                var Year = parseInt(DateMonthYear[0]);
                var Month = "";
                var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
                if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {
                    Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;
                    //check the entered date value is larger or not
                    if (Month == 1 || Month > 2) {
                        if (DateInput > ListofDays[Month - 1]) {
                            alert('Invalid date format!');
                            this.focus();
                            i = 1;
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                        }
                    }

                    //CHECK FOR THE LEAP YEAR
                    if (Month == 2) {
                        var lyear = false;
                        if ((!(Year % 4) && Year % 100) || !(Year % 400)) {
                            lyear = true;
                        }
                        if ((lyear == false) && (DateInput >= 29)) {
                            alert('Invalid date format!');
                            this.focus();
                            i = 1;
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                        }
                        if ((lyear == true) && (DateInput > 29)) {
                            alert('Invalid date format!');
                            this.focus();
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                            i = 1;
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                        }
                    }

                    var CurrentDate = new Date();
                    var CurrentYear = CurrentDate.getFullYear();
                    Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;
                    var CurrentYear = CurrentDate.getFullYear();
                    var curmonth = CurrentDate.getMonth() + 1;
                    var curday = CurrentDate.getDate();
                    if (Year > CurrentYear) {
                        alert("Cannot be future date. Please Enter a Valid Date.");
                        this.focus();
                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }
                    else if (parseInt(Month) > parseInt(curmonth) && Year >= CurrentYear) {
                        alert("Cannot be future date. Please Enter a Valid Date.");
                        this.focus();

                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }
                    else if (parseInt(DateMonthYear[2]) > parseInt(curday) && Year >= CurrentYear && parseInt(Month) >= parseInt(curmonth)) {
                        alert("Cannot be future date. Please Enter a Valid Date.");
                        this.focus();
                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }

                } else {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                }
            } else {

                if (inputText.split('-')[0].length == 0 && (inputText.split('-')[1].length != 0 || inputText.split('-')[0].length != 0)) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                } else if (inputText.split('-')[2].length == 1) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                } else if (inputText.split('-')[1].length == 0 && inputText.split('-')[0].length == 0) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                } else if (inputText.split('-')[2].length != 0 && (inputText.split('-')[1].length == 0 || inputText.split('-')[0].length == 0)) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                } else if (inputText.split('-')[1] != '___' && inputText.split('-')[1].length != 0 && inputText.split('-')[0].length != 0) {
                    var DateMonthYear = inputText.split('-');
                    if (DateMonthYear[0].length < 4) {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }

                    if (DateMonthYear[1].length < 3) {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }

                    var DateMonthYear = inputText.split('-');
                    lopera2 = DateMonthYear.length;

                    var Year = parseInt(DateMonthYear[0]);
                    var Month = "";
                    var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                    var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
                    if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {
                        Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;
                        var CurrentDate = new Date();
                        var CurrentYear = CurrentDate.getFullYear();
                        var curmonth = CurrentDate.getMonth() + 1;
                        var curday = CurrentDate.getDate();
                        if (Year > CurrentYear) {
                            alert("Cannot be future date. Please Enter a Valid Date.");
                            this.focus();
                            i = 1;
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                        }
                        else if (parseInt(Month) > parseInt(curmonth) && Year >= CurrentYear) {

                            alert("Cannot be future date. Please Enter a Valid Date.");
                            this.focus();
                            i = 1;
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                        }
                        else if (parseInt(DateMonthYear[2]) > parseInt(curday) && Year >= CurrentYear && parseInt(Month) >= parseInt(curmonth)) {
                            alert("Cannot be future date. Please Enter a Valid Date.");
                            this.focus();
                            i = 1;
                            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                        }

                    }
                        /*bugid :38668*/
                    else {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }
                } else if (inputText.split('-')[0].length != 0) {
                    var DateMonthYear = inputText.split('-');
                    if (DateMonthYear[0].length < 4) {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }

                    var DateMonthYear = inputText.split('-');
                    var Year = parseInt(DateMonthYear[0]);
                    var CurrentDate = new Date();
                    var CurrentYear = CurrentDate.getFullYear();
                    if (Year > CurrentYear) {
                        alert("Cannot be future date. Please Enter a Valid Date.");
                        this.focus();
                        i = 1;
                        $(this)[0].value = "____-___-__";
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); return false;
                    }
                }
            }
        }

    });
    if ($("#CDPLastMammogramDATEPICKER") != "undefined" && $("#CDPLastMammogramDATEPICKER").length != 0) {
        var alertError = false;
        var Cdp_Val = $("#CDPLastMammogramDATEPICKER")[0].value.trim();
        var Res_Val = $('select[name="Last Mammogram Result"]')[0].selectedOptions[0].innerText.trim();
        var Notes_Val = $("#txtNotesLastMammogram_txtDLC")[0].value.trim();
        var SelectedVal = "";
        var TextBoxVal = Notes_Val;
        var Type = "MandatoryAlert";
        if (Cdp_Val == "" && Res_Val == "" && Notes_Val == "") {
            sessionStorage.setItem("EncCancel", "false");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
            document.getElementById('btnsavehidden').click();
        }
        else if (Cdp_Val != "" && Res_Val != "" && Notes_Val != "") {
            $.ajax({
                type: "POST",
                url: "frmDLC.aspx/FindIfInMammogramTestList",

                data: "{'SelectedText':'" + SelectedVal + "','TextBoxValue':'" + TextBoxVal + "','Type':'" + Type + "'}",

                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var result = response.d;
                    if (result == "showAlert") {
                        alertError = true;
                    }
                    if (alertError) {
                        AutoSaveUnsuccessful();
                        DisplayErrorMessage('210019');
                        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                        return false;
                    }
                    else {
                        sessionStorage.setItem("EncCancel", "false");
                        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                        document.getElementById('btnsavehidden').click();
                    }
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
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
        }
        else {
            AutoSaveUnsuccessful();
            DisplayErrorMessage('210019');
            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
            return false;
        }
    }
    else if (i != 1) {
        sessionStorage.setItem("EncCancel", "false");
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        document.getElementById('btnsavehidden').click();
    }


}
//CAP-958 - Vitals data not reflect in Summary
function vitalsAutoSaveSuccessful() {
    var vitalAutoSave = sessionStorage.getItem('VitalsAutoSave');
    if (vitalAutoSave == true || vitalAutoSave == "true") {
        sessionStorage.setItem('EncAutoSave', true);
        sessionStorage.removeItem('VitalsAutoSave');
        AutoSaveSuccessful();
    }
    //CAP-2678
    localStorage.setItem('IsSaveCompleted', true);
}
function convertDate(inputFormat) {
    var d = new Date(inputFormat);
    return [d.getDate(), d.getMonth() + 1, d.getFullYear()].join('/');
}
Date.prototype.today = function () {
    return this.getFullYear() + "/" + (((this.getMonth() + 1) < 10) ? "0" : "") + (this.getMonth() + 1) + "/" + ((this.getDate() < 10) ? "0" : "") + this.getDate();
}
Date.prototype.timeNow = function () {
    return ((this.getHours() < 10) ? "0" : "") + ((this.getHours() > 12) ? (this.getHours() - 12) : this.getHours()) + ":" + ((this.getMinutes() < 10) ? "0" : "") + this.getMinutes() + ":" + ((this.getSeconds() < 10) ? "0" : "") + this.getSeconds() + ((this.getHours() > 12) ? ('PM') : 'AM');
}

function timeformat(inputformat) {
    var d = new Date(inputformat);
    return ((d.getHours() < 10) ? "0" : "") + ((d.getHours() > 12) ? (d.getHours() - 12) : d.getHours()) + ":" + ((d.getMinutes() < 10) ? "0" : "") + d.getMinutes() + ":" + ((d.getSeconds() < 10) ? "0" : "") + d.getSeconds() + ((d.getHours() > 12) ? ('PM') : 'AM');
}
String.prototype.insert = function (index, string) {
    if (index > 0)
        return this.substring(0, index) + string + this.substring(index, this.length);
    else
        return string + this;
};

function EnableSave(event) {

    localStorage.setItem("bSave", "false");
    if (event == undefined) {
        $('#btnSaveVitals').prop('disabled', false);
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    if (event != undefined) {
        var keyPressed = (event.which) ? event.which : event.keyCode;
        if (keyPressed == null || keyPressed != 9) {
            $('#btnSaveVitals').prop('disabled', false);
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
    }
    //Commentted for bugId:76114
    //if (event != null) {
    //    if (event.srcElement != null) {
    //        if (event.srcElement.value != undefined && event.srcElement.value.indexOf(".") != -1) {
    //            var txt = event.srcElement.value;
    //            var number = event.srcElement.value.split('.');
    //            if (number[1] > 1) {
    //                if (number[1].length >3) {
    //                    event.srcElement.value = number[0] + '.' + number[1].substring(0, 3);
    //                    return false;
    //                }
    //            } else {
    //                if (number[1].length > 3) {
    //                    event.srcElement.value = number[0] + '.' + number[1].substring(0, 3);
    //                    return false;
    //                }
    //            }
    //        }
    //    }
    //}
    //Commentted for bugId:76114 
    return true;
}

function onChangeDatePicker(event, args) {
    var today = new Date();
    var id = event._clientStateFieldID.split("_")[0];
    var cal = $find(id);
    if (args._newDate > today)
        cal.set_selectedDate(args._oldDate);
}

function ConvertInchtoFeetInch(strValue) {
    var returnValue = new Array();
    if (strValue == '') {
        return returnValue;
    }
    var inch = parseFloat(strValue);
    var defaultValue = parseFloat(12);
    var feet = Math.floor(inch / defaultValue);
    var remainInch = Math.round((inch % defaultValue));
    returnValue.push(feet);
    returnValue.push(remainInch);
    return returnValue;
}

function CalculateBMIOnFtInchAndLBS(HeightFt, HeightInch, Weight) {
    var BMI = 0;
    var HtVal = 0;
    var WtVal = 0;
    var sHtval = '';
    try {
        if ((HeightFt >= 1 && HeightFt <= 12) && (HeightInch >= 0 && HeightInch <= 11.99) && (Weight >= 2 && Weight <= 1000)) {
            if (HeightFt != '') {
                sHtval = ConvertFeetInchToInch(HeightFt, HeightInch);
            }
            if (sHtval != '') {
                HtVal = parseFloat(sHtval);
            } else {
                if (document.getElementById('BMI') != undefined)
                    document.getElementById('BMI').value = '';
            }
            if (Weight != '') {
                WtVal = parseFloat(Weight);
            } else {
                return '';
            }
            var DefaultValue = parseFloat(703);
            BMI = ((WtVal / (HtVal * HtVal)) * DefaultValue).toFixed(1);
            if (BMI != 0) {
                if (document.getElementById('BMI') != undefined)
                    document.getElementById('BMI').value = BMI;
            } else {
                if (document.getElementById('BMI') != undefined)
                    document.getElementById('BMI').value = '';
            }
        } else {
            if (document.getElementById('BMI') != undefined)
                document.getElementById('BMI').value = '';
        }

    } catch (Exception) {
        if (document.getElementById('BMI') != undefined)
            document.getElementById('BMI').value = '';
    }
}

function ConvertFeetInchToInch(s1, s2) {
    if (s1 == '') {
        return '';
    }
    var inValue = 0;
    if (s2 != '') {
        inValue = ((parseFloat(s1) * 12) + parseFloat(s2)).toFixed(2);
    } else {
        inValue = ((s1) * 12).toFixed(2);
    }
    if (inValue != 0) {
        return inValue;
    } else {
        return '';
    }
}

function SetBMIStatus(sBMI) {
    var statusLabel = document?.getElementById('BMI Status');
    var BMITextBox = document?.getElementById('BMI');
    var percentileIncrement = 0;
    var percentileListForBMI = '';
    var ColorListForBMI = '';
    //CAP-1463 
    if (statusLabel != undefined && statusLabel != null) {
        statusLabel.style.color = "Red";
    }
    
    if (SpercentileIncrement != null) {
        percentileListForBMI = SpercentileIncrement;
    }
    if (statusLabel != undefined && statusLabel.id != null) {
        if (sBMI != '') {
            var dbmi = sBMI;
            if (percentileListForBMI.length > 0) {
                var objFirst = percentileListForBMI[0];
                var objSecond = percentileListForBMI[1];
                var L = ((percentileIncrement * objFirst.L) + (1 - percentileIncrement) * objSecond.L);
                var M = ((percentileIncrement * objFirst.M) + (1 - percentileIncrement) * objSecond.M);
                var S = ((percentileIncrement * objFirst.S) + (1 - percentileIncrement) * objSecond.S);
                var X = sBMI;
                var z_score = (Math.pow((X / M), L) - 1) / (L * S);
                var normDist = Formula.NORMSDIST(z_score, true);
                var percentile = ((normDist * 1000) / 10).toFixed(2);
                statusLabel.title = "BMI %tile: " + percentile;
                statusLabel.value = "BMI %tile: " + percentile;
                if (BMI_PERCENTILE_STATUS.length > 0) {
                    for (var i = 0; i < BMI_PERCENTILE_STATUS.length; i++) {
                        var rangeText = BMI_PERCENTILE_STATUS[i].Value;
                        if (BMI_PERCENTILE_STATUS[i].LowerLimit != '' && BMI_PERCENTILE_STATUS[i].UpperLimit != '') {
                            if (parseFloat(percentile) >= parseFloat(BMI_PERCENTILE_STATUS[i].LowerLimit) && parseFloat(percentile) <= parseFloat(BMI_PERCENTILE_STATUS[i].UpperLimit)) {
                                statusLabel.title = statusLabel.value + " " + BMI_PERCENTILE_STATUS[i].Description;
                                statusLabel.value += " " + BMI_PERCENTILE_STATUS[i].Description;
                                statusLabel.title = statusLabel.value;
                                document.getElementById(GetClientId("hdnBMI")).value = statusLabel.value;
                                break;
                            }
                        } else {
                            if (parseFloat(percentile) >= parseFloat(BMI_PERCENTILE_STATUS[i].LowerLimit)) {
                                statusLabel.title = statusLabel.value + " " + BMI_PERCENTILE_STATUS[i].Description;
                                statusLabel.value += " " + BMI_PERCENTILE_STATUS[i].Description;
                                statusLabel.title = statusLabel.value;
                                document.getElementById(GetClientId("hdnBMI")).value = statusLabel.value;
                                break;
                            }
                        }
                    }
                }
            } else {
                percentileListForBMI = S_BMI_Stat;
                if (percentileListForBMI != null && percentileListForBMI.length > 0) {
                    for (var i = 0; i < percentileListForBMI.length; i++) {
                        var rangeText = percentileListForBMI[i].Value;
                        if (percentileListForBMI[i].LowerLimit != '' && percentileListForBMI[i].UpperLimit != '') {
                            if (parseFloat(dbmi) >= parseFloat(percentileListForBMI[i].LowerLimit) && parseFloat(dbmi) <= parseFloat(percentileListForBMI[i].UpperLimit)) {

                                statusLabel.title = " " + percentileListForBMI[i].Description;
                                statusLabel.value = percentileListForBMI[i].Description;
                                statusLabel.title = statusLabel.value;
                                document.getElementById(GetClientId("hdnBMI")).value = statusLabel.value;
                                break;
                            }
                        } else {
                            if (parseFloat(dbmi) >= parseFloat(percentileListForBMI[i].LowerLimit)) {
                                statusLabel.title = " " + percentileListForBMI[i].Description;
                                statusLabel.value = percentileListForBMI[i].Description;
                                statusLabel.title = statusLabel.value;
                                document.getElementById(GetClientId("hdnBMI")).value = statusLabel.value;
                                break;
                            }
                        }
                    }
                }

            }
        } else {
            statusLabel.value = " ";
            statusLabel.title = " ";
        }
        if (statusLabel.value != '') {
            ColorListForBMI = BMI_STATUS_COLOR;
            statusLabel.style.color = "red";
            statusid.push("BMI Status");
            if (statusLabel.value.length > 15) {
                statusLabel.style.height = "29px";
            } else {
                statusLabel.style.height = "18px";
            }
            if (statusLabel.value.toUpperCase().indexOf("NORMAL") > -1) {

                statusLabel.style.color = "black";

                statusid.remove("BMI Status");

            }
        }
    }
    document.getElementById(GetClientId("hdnBMI")).value += "+" + statusLabel.style.color;
}

function SetHBA1CStatus(hbValue, controlID, statusLabelId) {
    var HB = '';
    if (hbValue != '') {
        if (HBA1C_STATUS != undefined && HBA1C_STATUS.length > 0) {
            for (var i = 0; i < HBA1C_STATUS.length; i++) {
                if (HBA1C_STATUS[i].lowerLimit != undefined && HBA1C_STATUS[i].upperLimit != undefined) {
                    if (parseFloat(hbValue) >= parseFloat(HBA1C_STATUS[i].lowerLimit) && parseFloat(hbValue) <= parseFloat(HBA1C_STATUS[i].upperLimit)) {
                        HB = HBA1C_STATUS[i].description;
                        document.getElementById(GetClientId("hdnHbA1c")).value = HB;
                        break;
                    }
                } else {
                    if (parseFloat(hbValue) >= parseFloat(HBA1C_STATUS[i].lowerLimit)) {
                        HB = HBA1C_STATUS[i].description;
                        document.getElementById(GetClientId("hdnHbA1c")).value = HB;
                        break;
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {

        statusLabel.title = HB;
        statusLabel.value = HB;
        statusLabel.style.color = "red";
        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        } else {
            statusLabel.style.height = "18px";
        }
        if (HBA1C_COLOR != undefined && HBA1C_COLOR.length > 0) {
            for (var i = 0; i < HBA1C_COLOR.length; i++) {
                if (HBA1C_COLOR[i].description != undefined && HBA1C_COLOR[i].Value != undefined) {
                    if (HBA1C_COLOR[i].Value == statusLabelId) {
                        if (HB.toUpperCase().trim() == HBA1C_COLOR[i].description.toUpperCase().trim()) {
                            statusLabel.style.color = "black";
                            statusid.remove(statusLabelId);
                            break;
                        }
                    }
                }
            }
        }
    }

    document.getElementById(GetClientId("hdnHbA1c")).value += "+" + statusLabel.style.color;
}

function SetEGFRStatus(egfrValue, controlID, statusLabelId) {
    var eGFR = '';
    if (egfrValue != '') {
        if (EGFR_STATUS != undefined && EGFR_STATUS.length > 0) {
            for (var i = 0; i < EGFR_STATUS.length; i++) {
                if (EGFR_STATUS[i].lowerLimit != undefined && EGFR_STATUS[i].upperLimit != undefined) {
                    if (parseFloat(egfrValue) >= parseFloat(EGFR_STATUS[i].lowerLimit) && parseFloat(egfrValue) <= parseFloat(EGFR_STATUS[i].upperLimit)) {
                        eGFR = EGFR_STATUS[i].description;
                        if (statusLabelId.toString().indexOf("Second") != -1)
                            document.getElementById(GetClientId("hdnegfrSecond")).value = eGFR;
                        else
                            document.getElementById(GetClientId("hdnegfr")).value = eGFR;
                        break;
                    }
                } else {
                    if (parseFloat(egfrValue) >= parseFloat(EGFR_STATUS[i].lowerLimit)) {
                        eGFR = EGFR_STATUS[i].description;
                        if (statusLabelId.toString().indexOf("Second") != -1)
                            document.getElementById(GetClientId("hdnegfrSecond")).value = eGFR;
                        else
                            document.getElementById(GetClientId("hdnegfr")).value = eGFR;
                        break;
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {

        statusLabel.title = eGFR;
        statusLabel.value = eGFR;

        statusLabel.style.color = "red";
        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        } else {
            statusLabel.style.height = "18px";
        }
        if (EGFR_COLOR != undefined && EGFR_COLOR.length > 0) {
            for (var i = 0; i < EGFR_COLOR.length; i++) {
                if (EGFR_COLOR[i].description != undefined && EGFR_COLOR[i].Value != undefined) {
                    if (EGFR_COLOR[i].Value == statusLabelId) {
                        if (eGFR.toUpperCase().trim() == EGFR_COLOR[i].description.toUpperCase().trim()) {
                            statusLabel.style.color = "black";
                            statusid.remove(statusLabelId);
                            break;
                        }
                    }
                }
            }
        }
    }
    if (statusLabelId.toString().indexOf("Second") != -1)
        document.getElementById(GetClientId("hdnegfrSecond")).value += "+" + statusLabel.style.color;
    else
        document.getElementById(GetClientId("hdnegfr")).value += "+" + statusLabel.style.color;
}

function SetUrineforMicroalbuminStatus(UrineforMicroalbuminValue, controlID, statusLabelId) {
    var UrineforMicroalbumin = '';
    if (UrineforMicroalbuminValue != '') {
        if (URINE_FOR_MICROALBUMINSTATUS != undefined && URINE_FOR_MICROALBUMINSTATUS.length > 0) {
            for (var i = 0; i < URINE_FOR_MICROALBUMINSTATUS.length; i++) {
                if (URINE_FOR_MICROALBUMINSTATUS[i].lowerLimit != undefined && URINE_FOR_MICROALBUMINSTATUS[i].upperLimit != undefined) {
                    if (parseFloat(UrineforMicroalbuminValue) >= parseFloat(URINE_FOR_MICROALBUMINSTATUS[i].lowerLimit) && parseFloat(UrineforMicroalbuminValue) <= parseFloat(URINE_FOR_MICROALBUMINSTATUS[i].upperLimit)) {
                        UrineforMicroalbumin = URINE_FOR_MICROALBUMINSTATUS[i].description;
                        document.getElementById(GetClientId("hdnUrineforMicroalbumin")).value = UrineforMicroalbumin;
                        break;
                    }
                } else {
                    if (parseFloat(UrineforMicroalbuminValue) >= parseFloat(URINE_FOR_MICROALBUMINSTATUS[i].lowerLimit)) {
                        UrineforMicroalbumin = URINE_FOR_MICROALBUMINSTATUS[i].description;
                        document.getElementById(GetClientId("hdnUrineforMicroalbumin")).value = UrineforMicroalbumin;
                        break;
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {
        statusLabel.title = UrineforMicroalbumin
        statusLabel.value = UrineforMicroalbumin;
        statusLabel.style.color = "red";
        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        } else {
            statusLabel.style.height = "18px";
        }


        var BMIValue = statusLabel.value;

        if (UrineforMicroalbumin.toUpperCase().trim() == "NORMAL") {

            statusLabel.style.color = "black";
            statusid.remove(statusLabelId);

        }

    }
}

function SetABITestStatus(ABITestValue, controlID, statusLabelId) {
    var ABITest = '';
    if (ABITestValue != '') {
        if (ABI_TESTSTATUS != undefined && ABI_TESTSTATUS.length > 0) {
            for (var i = 0; i < ABI_TESTSTATUS.length; i++) {
                if (ABI_TESTSTATUS[i].lowerLimit != undefined && ABI_TESTSTATUS[i].upperLimit != undefined) {
                    if (parseFloat(ABITestValue) >= parseFloat(ABI_TESTSTATUS[i].lowerLimit) && parseFloat(ABITestValue) <= parseFloat(ABI_TESTSTATUS[i].upperLimit)) {
                        ABITest = ABI_TESTSTATUS[i].description;
                        document.getElementById(GetClientId("hdnABITest")).value = ABITest;
                        break;
                    }
                } else {
                    if (ABI_TESTSTATUS[i].lowerLimit != undefined) {
                        if (parseFloat(ABITestValue) <= parseFloat(ABI_TESTSTATUS[i].lowerLimit)) {
                            ABITest = ABI_TESTSTATUS[i].description;
                            document.getElementById(GetClientId("hdnABITest")).value = ABITest;
                            break;
                        }
                    }
                    if (ABI_TESTSTATUS[i].upperLimit != undefined) {
                        if (parseFloat(ABITestValue) >= parseFloat(ABI_TESTSTATUS[i].upperLimit)) {
                            ABITest = ABI_TESTSTATUS[i].description;
                            document.getElementById(GetClientId("hdnABITest")).value = ABITest;
                            break;
                        }
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {
        statusLabel.title = ABITest;
        statusLabel.value = ABITest;
        statusLabel.style.color = "red";

        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        } else {
            statusLabel.style.height = "18px";
        }

        if (ABITest.toUpperCase().trim() == "NORMAL RANGE") {

            statusLabel.style.color = "black";
            statusid.remove(statusLabelId);
        }
    }
}

function SetBloodSugarFastingStatus(FastingValue, controlID, statusLabelId) {
    var FASTING = '';
    if (FastingValue != '') {
        if (BLOOD_SUGAR_FASTING_STATUS != undefined && BLOOD_SUGAR_FASTING_STATUS.length > 0) {
            for (var i = 0; i < BLOOD_SUGAR_FASTING_STATUS.length; i++) {
                if (BLOOD_SUGAR_FASTING_STATUS[i].lowerLimit != undefined && BLOOD_SUGAR_FASTING_STATUS[i].upperLimit != undefined) {
                    if (parseFloat(FastingValue) >= parseFloat(BLOOD_SUGAR_FASTING_STATUS[i].lowerLimit) && parseFloat(FastingValue) <= parseFloat(BLOOD_SUGAR_FASTING_STATUS[i].upperLimit)) {
                        FASTING = BLOOD_SUGAR_FASTING_STATUS[i].description;
                        if (statusLabelId.toString().indexOf("Second") != -1) {
                            document.getElementById(GetClientId("hdnBloodFastingSecond")).value = FASTING;
                            document.getElementById(GetClientId("hdnBloodFastingSecond")).title = FASTING;
                        }
                        else {
                            document.getElementById(GetClientId("hdnBloodFasting")).value = FASTING;
                            document.getElementById(GetClientId("hdnBloodFasting")).title = FASTING;
                        }
                        break;
                    }
                } else {
                    if (parseFloat(FastingValue) >= parseFloat(BLOOD_SUGAR_FASTING_STATUS[i].lowerLimit)) {
                        FASTING = BLOOD_SUGAR_FASTING_STATUS[i].description;
                        if (statusLabelId.toString().indexOf("Second") != -1) {
                            document.getElementById(GetClientId("hdnBloodFastingSecond")).value = FASTING;
                            document.getElementById(GetClientId("hdnBloodFastingSecond")).title = FASTING;
                        }
                        else {
                            document.getElementById(GetClientId("hdnBloodFasting")).value.value = FASTING;
                            document.getElementById(GetClientId("hdnBloodFasting")).value.title = FASTING;
                        }
                        break;
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {
        statusLabel.title = FASTING;
        statusLabel.value = FASTING;

        statusLabel.style.color = "red";
        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        } else {
            statusLabel.style.height = "18px";
        }
        if (FASTING.toUpperCase().indexOf("NORMAL") > -1) {
            statusLabel.style.color = "black";
            statusid.remove(statusLabelId);
        }
    }
    if (statusLabelId.toString().indexOf("Second") != -1)
        document.getElementById(GetClientId("hdnBloodFastingSecond")).value += "+" + statusLabel.style.color;
    else
        document.getElementById(GetClientId("hdnBloodFasting")).value += "+" + statusLabel.style.color;
}

function SetBloodSugarPostPrandialStatus(PostValue, controlID, statusLabelId) {
    var POSTPRANDIAL = '';
    if (PostValue != '') {
        if (BLOOD_SUGAR_POST_PRANDIAL_STATUS != undefined && BLOOD_SUGAR_POST_PRANDIAL_STATUS.length > 0) {
            for (var i = 0; i < BLOOD_SUGAR_POST_PRANDIAL_STATUS.length; i++) {
                if (BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].lowerLimit != undefined && BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].upperLimit != undefined) {
                    if (parseFloat(PostValue) >= parseFloat(BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].lowerLimit) && parseFloat(PostValue) <= parseFloat(BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].upperLimit)) {
                        POSTPRANDIAL = BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].description;
                        if (statusLabelId.toString().indexOf("Second") != -1)
                            hdnBloodPostSecond.value = POSTPRANDIAL;
                        else
                            hdnBloodPost.value = POSTPRANDIAL;
                        break;
                    }
                } else {
                    if (parseFloat(PostValue) >= parseFloat(BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].lowerLimit)) {
                        POSTPRANDIAL = BLOOD_SUGAR_POST_PRANDIAL_STATUS[i].description;
                        if (statusLabelId.toString().indexOf("Second") != -1)
                            hdnBloodPostSecond.value = POSTPRANDIAL;
                        else
                            hdnBloodPost.value = POSTPRANDIAL;
                        break;
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {
        statusLabel.title == POSTPRANDIAL;
        statusLabel.value = POSTPRANDIAL;
        statusLabel.title = POSTPRANDIAL;
        statusLabel.style.color = "red";
        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        }
       
        if (POSTPRANDIAL.toUpperCase().indexOf("NORMAL") > -1) {
            statusLabel.style.color = "black";
            statusid.remove(statusLabelId);
        }
    }
    if (statusLabelId.toString().indexOf("Second") != -1)
        hdnBloodPostSecond.value += "+" + statusLabel.style.color;
    else
        hdnBloodPost.value += "+" + statusLabel.style.color;
}

function ConvertInchtoFeetInch(strValue) {
    var returnValue = new Array();
    if (strValue == '') {
        return returnValue;
    }
    var inch = parseFloat(strValue);
    var defaultValue = parseFloat(12);
    var feet = Math.floor(inch / defaultValue);
    var remainInch = Math.round((inch % defaultValue));
    returnValue.push(feet);
    returnValue.push(remainInch);
    return returnValue;
}

function CalculateBMIOnFtInchAndLBS(HeightFt, HeightInch, Weight) {
    var BMI = 0;
    var HtVal = 0;
    var WtVal = 0;
    var sHtval = '';
    try {
        if (HeightFt != '') {
            sHtval = ConvertFeetInchToInch(HeightFt, HeightInch);
        }
        if (sHtval != '') {
            HtVal = parseFloat(sHtval);
        } else {
            if (document.getElementById('BMI') != undefined)
                document.getElementById('BMI').value = '';
        }
        if (Weight != '') {
            WtVal = parseFloat(Weight);
        } else {
            return '';
        }
        var DefaultValue = parseFloat(703);
        BMI = ((WtVal / (HtVal * HtVal)) * DefaultValue).toFixed(1);
        if (BMI != 0) {
            if (document.getElementById('BMI') != undefined)
                document.getElementById('BMI').value = BMI;
        } else {
            if (document.getElementById('BMI') != undefined)
                document.getElementById('BMI').value = '';
        }
    } catch (Exception) {
        //CAP-290 Cannot read properties of null (reading value)
        //if (document.getElementById('BMI') != undefined && document.getElementById('BMI') != null)
            document.getElementById('BMI').value = '';
    }
}

function ConvertFeetInchToInch(s1, s2) {
    if (s1 == '') {
        return '';
    }
    var inValue = 0;
    if (s2 != '') {
        inValue = ((parseFloat(s1) * 12) + parseFloat(s2)).toFixed(2);
    } else {
        inValue = ((s1) * 12).toFixed(2);
    }
    if (inValue != 0) {
        return inValue;
    } else {
        return '';
    }
}

function EnableSaveButton(IsEnable) {
    if (IsEnable)

        $('#btnSaveVitals').prop('disabled', false);
    else
        $('#btnSaveVitals').prop('disabled', true);
    //$find('btnSave').set_enabled(IsEnable);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = IsEnable;
    localStorage.setItem("bSave", "false");
    if (event.srcElement != null) {
        if (event.srcElement.value != undefined && event.srcElement.value.indexOf(".") != -1) {
            var txt = event.srcElement.value;
            var number = event.srcElement.value.split('.');

            if (number[1] > 1) {
                if (number[1].length > 2) {
                    event.srcElement.value = number[0] + '.' + number[1].substring(0, 2);
                    return false;
                }
                var charCode = (event.which) ? event.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                    return false;
                else {
                    var len = event.srcElement.value.length;
                    var index = event.srcElement.value.indexOf('.');

                    if (index > 0 && charCode == 46) {
                        return false;
                    }
                    if (index > 0) {
                        var CharAfterdot = (len + 1) - index;
                        if (CharAfterdot > 3) {
                            if (number[1].length > 2) {
                                event.srcElement.value = number[0] + '.' + number[1].substring(0, 2);
                                return false;
                            }
                            if (!isNaN(txt) && isFinite(txt) && txt.length != 0) {
                                var rounded = Math.round(txt * 100) / 100;
                                event.srcElement.value = rounded.toFixed(2);
                            }
                            return false;
                        }
                    }

                }
            } else {
                if (number[1].length > 2) {
                    event.srcElement.value = number[0] + '.' + number[1].substring(0, 2);
                    return false;
                }
            }
        }
    }


}

function CheckIsBMIIsValid() {
    var txtBMI = document.getElementById('BMI');
    if (document.getElementById('BMI') != undefined) {
        if (document.getElementById('Weight').value == '' || document.getElementById('Height').value == '') {
            txtBMI.value = '';
            var statusLabel = document.getElementById('BMI Status');
            statusLabel.value = '';
            if (document.getElementById('txtNotesBMI_txtDLC') != undefined) {
                document.getElementById('txtNotesBMI_txtDLC').readOnly = true;
                $('#txtNotesBMI_txtDLC').removeClass('Editabletxtbox');
                $('#txtNotesBMI_txtDLC').addClass('nonEditabletxtbox');
            }
            if (document.getElementById('txtNotesBMI_pbDropdown') != undefined)
                document.getElementById('txtNotesBMI_pbDropdown').disabled = true;
            if (document.getElementById('txtNotesBMI_listDLC') != undefined) {
                $('#txtNotesBMI_pbDropdown')[0].firstElementChild.className = "fa fa-plus margin2";
                document.getElementById('txtNotesBMI_listDLC').style.display = "none";
            }

        }
        else {
            if (document.getElementById('txtNotesBMI_txtDLC') != undefined) {
                document.getElementById('txtNotesBMI_txtDLC').readOnly = false;
                $('#txtNotesBMI_txtDLC').removeClass('nonEditabletxtbox');
                $('#txtNotesBMI_txtDLC').addClass('Editabletxtbox');
                document.getElementById('txtNotesBMI_txtDLC').value = "";
            }
            if (document.getElementById('txtNotesBMI_pbDropdown') != undefined) {

                document.getElementById('txtNotesBMI_pbDropdown').disabled = false;
            }
        }
    }
    if (document.getElementById('Weight').value != "") {
        if (document.getElementById('txtNotesWeight_txtDLC').value != "") {

            var heightnotes = document.getElementById('txtNotesWeight_txtDLC').value.split(',');
            var resason = document.getElementById('hdnreason').value.split('~');
            for (var i = 0; i < heightnotes?.length; i++) {
                for (var j = 0; j < resason.length; j++) {
                    //CAP - 282 - Preventing undefined error.
                    if (resason[j]?.split('|')[0]?.indexOf("WEIGHT") != undefined && resason[j]?.split('|')[1] != undefined && heightnotes[i] != undefined) {
                        if (resason[j].split('|')[0].indexOf("WEIGHT") > -1 && resason[j].split('|')[1] == heightnotes[i].trim()) {
                            if ((heightnotes.length == 0 && i == 0) || i == heightnotes.length - 1)
                                document.getElementById('txtNotesWeight_txtDLC').value = document.getElementById('txtNotesWeight_txtDLC').value.replace(heightnotes[i].trim(), "");

                            else
                                document.getElementById('txtNotesWeight_txtDLC').value = document.getElementById('txtNotesWeight_txtDLC').value.replace(heightnotes[i].trim() + ", ", "");
                            //CAP-3680
                            if (document.getElementById('txtNotesBMI_txtDLC')?.value != undefined && document.getElementById('txtNotesBMI_txtDLC')?.value != null && document.getElementById('txtNotesBMI_txtDLC')?.value != "" && document.getElementById('txtNotesBMI_txtDLC')?.value?.indexOf(heightnotes[i]?.trim()) > -1 && document.getElementById('txtNotesHeight_txtDLC')?.value?.indexOf(heightnotes[i]?.trim()) <= -1)
                                document.getElementById('txtNotesBMI_txtDLC').value = document.getElementById('txtNotesBMI_txtDLC').value.replace(heightnotes[i].trim(), "");

                            var notes = document.getElementById('txtNotesBMI_txtDLC').value.split(',');
                            var ref = "";
                            for (var i = 0; i < notes.length; i++) {

                                if (ref == "") {
                                    if (notes[i].trim() != "")
                                        ref = notes[i];
                                }
                                else {
                                    if (notes[i].trim() != "")
                                        ref = ref + "," + notes[i];
                                }

                            }

                            document.getElementById('txtNotesBMI_txtDLC').value = ref;
                        }
                    }
                }

            }
        }

    }
    if (document.getElementById('Height').value != "") {
        if (document.getElementById('txtNotesHeight_txtDLC').value != "") {

            var heightnotes = document.getElementById('txtNotesHeight_txtDLC').value.split(',');
            var resason = document.getElementById('hdnreason').value.split('~');
            for (var i = 0; i < heightnotes?.length; i++) {
                for (var j = 0; j < resason.length; j++) {
                    //CAP - 282 - Preventing undefined error.
                    if (resason[j]?.split('|')[0]?.indexOf("HEIGHT") != undefined && resason[j]?.split('|')[1] != undefined && heightnotes[i] != undefined) {
                        if (resason[j].split('|')[0].indexOf("HEIGHT") > -1 && resason[j].split('|')[1] == heightnotes[i].trim()) {
                            //CAP-1463
                            if (document.getElementById('txtNotesHeight_txtDLC') != undefined && document.getElementById('txtNotesHeight_txtDLC') != null) {
                                if ((heightnotes.length == 0 && i == 0) || i == heightnotes.length - 1)
                                    document.getElementById('txtNotesHeight_txtDLC').value = document.getElementById('txtNotesHeight_txtDLC').value.replace(heightnotes[i].trim(), "");
                                else
                                    document.getElementById('txtNotesHeight_txtDLC').value = document.getElementById('txtNotesHeight_txtDLC').value.replace(heightnotes[i].trim() + ", ", "");
                            }
                            if (document.getElementById('txtNotesBMI_txtDLC').value != "" && document.getElementById('txtNotesBMI_txtDLC').value.indexOf(heightnotes[i].trim()) > -1 && document.getElementById('txtNotesHeight_txtDLC').value.indexOf(heightnotes[i].trim()) <= -1)
                            //CAP-1463 
                                document.getElementById('txtNotesBMI_txtDLC').value = document?.getElementById('txtNotesBMI_txtDLC')?.value?.replace(heightnotes[i]?.trim(), "");

                            var notes = document.getElementById('txtNotesBMI_txtDLC').value.split(',');
                            var ref = "";
                            for (var i = 0; i < notes.length; i++) {

                                if (ref == "") {
                                    if (notes[i].trim() != "")
                                        ref = notes[i];
                                }
                                else {
                                    if (notes[i].trim() != "")
                                        ref = ref + "," + notes[i];
                                }

                            }

                            document.getElementById('txtNotesBMI_txtDLC').value = ref;
                        }
                    }
                }

            }
        }



    }

}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function RangeError(input, args) {
    var inputText = args.get_inputText();
    if (Number(inputText) != NaN) {
        var message = "Value should be in the range of " + input.get_minValue() + " to " + input.get_maxValue();
        alert(message);
    }

}

function savedSuccessfully() {
    localStorage.setItem("bSave", "true");
    SavedSuccessfully_NowProceed('EncounterTabClick');
    DisplayErrorMessage('200008');
    //CAP-1463
    if (window?.parent?.parent?.parent?.parent?.theForm?.ctl00_C5POBody_hdnIsSaveEnable != undefined && window?.parent?.parent?.parent?.parent?.theForm?.ctl00_C5POBody_hdnIsSaveEnable != null){
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
    }
}

function btnGetVitals_Clicked(sender, args) {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    document.getElementById(GetClientId("hdnVitalTime")).value = now;
}

function textboxLeave(testValue) {
    var FeetHeight = document.getElementById(testValue.split('/')[0]);
    if (document.getElementById("Height").value >= 12) {
        document.getElementById("Height").focus();
        alert('Height value should be in the range of 1 to 12');
        return false;
    }
    if (document.getElementById("Height").value == "") {
        if (testValue.split('/')[1].split('-')[0] < FeetHeight.value && testValue.split('/')[1].split('-')[1].replace(",|", "") > FeetHeight.value != true) {
            var inch = ConvertInchtoFeetInch(FeetHeight.value);

            document.getElementById('Height').value = inch[0];
            FeetHeight.value = inch[1];
        }
    }
    var labelID = document.getElementById(testValue.split('/')[0] + " " + "Status");

    var checkValue = testValue.split('/')[1].split('|')[0].split(',')[0]

    var checkBoxValue = document.getElementById(testValue.split('/')[0]).value;

    for (var i = 0; i < testValue.split('/')[1].split('|').length; i++) {
        if (testValue.split('/')[1].split('|')[i] != "") {
            if (parseFloat(checkBoxValue) > parseFloat(testValue.split('/')[1].split('|')[i].split('-')[0]) && parseFloat(checkBoxValue) < parseFloat(testValue.split('/')[1].split('|')[i].split('-')[1].split(',')[0])) {
                labelID.innerHTML = testValue.split('/')[1].split('|')[i].split(',')[1];
            }
        }
    }
}


function CallMe(sender, args) {
    var inputText = sender._validationText;
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;
    if (inputText.match(FormatDDMMMYYYY)) {
        var DateMonthYear = inputText.split('-');
        if (DateMonthYear[0].length < 4) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        }
        if (DateMonthYear[1].length < 3) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        }
        if (DateMonthYear[2].length < 2) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        }
        if (DateMonthYear[0].length != 0 && DateMonthYear[1].length != 0 && DateMonthYear[2].length != 0) {
            if (DateMonthYear[2] == "00") {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;

            }
        }

        lopera2 = DateMonthYear.length;
        var DateInput = parseInt(DateMonthYear[2]);
        var Year = parseInt(DateMonthYear[0]);
        var Month = "";
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
        if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {
            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;
            if (Month == 1 || Month > 2) {
                if (DateInput > ListofDays[Month - 1]) {
                    alert('Invalid date format!');
                    $find(GetClientId(sender._clientID)).clear();
                    document.getElementById(sender._clientID).focus(true);
                    return false;
                }
            }

            if (Month == 2) {
                var lyear = false;
                if ((!(Year % 4) && Year % 100) || !(Year % 400)) {
                    lyear = true;
                }
                if ((lyear == false) && (DateInput >= 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(sender._clientID)).clear();
                    document.getElementById(sender._clientID).focus(true);
                    return false;
                }
                if ((lyear == true) && (DateInput > 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(sender._clientID)).clear();
                    document.getElementById(sender._clientID).focus(true);
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase());
            if (Year > CurrentYear) {
                alert("Cannot be future date. Please Enter a Valid Date.");
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;
            } else if (Year == CurrentYear && Month > CurrentDate.getMonth()) {
                alert("Cannot be future date. Please Enter a Valid Date.");
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;
            } else if (Year == CurrentYear && Month == CurrentDate.getMonth() && DateInput > CurrentDate.getDate()) {
                alert("Cannot be future date. Please Enter a Valid Date.");
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;
            }
        } else {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        }
    } else {

        if (inputText != null && inputText != "" && inputText.split('-')[0].length == 0 && (inputText.split('-')[1].length != 0 || inputText.split('-')[0].length != 0)) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        } else if (inputText != null && inputText != "" && inputText.split('-')[2].length == 1) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        } else if (inputText != null && inputText != "" && inputText.split('-')[1].length == 0 && inputText.split('-')[0].length == 0) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        } else if (inputText != null && inputText != "" && inputText.split('-')[2].length != 0 && (inputText.split('-')[1].length == 0 || inputText.split('-')[0].length == 0)) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).clear();
            document.getElementById(sender._clientID).focus(true);
            return false;
        } else if (inputText != null && inputText != "" && inputText.split('-')[1].length != 0 && inputText.split('-')[0].length != 0) {
            var DateMonthYear = inputText.split('-');
            if (DateMonthYear[0].length < 4) {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;
            }

            if (DateMonthYear[1].length < 3) {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;
            }

            var DateMonthYear = inputText.split('-');
            lopera2 = DateMonthYear.length;
            var Year = parseInt(DateMonthYear[0]);
            var Month = "";
            var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
            if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {

                var CurrentDate = new Date();
                var CurrentYear = CurrentDate.getFullYear();
                Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase());
                if (Year > CurrentYear) {
                    alert("Cannot be future date. Please Enter a Valid Date.");
                    $find(GetClientId(sender._clientID)).clear();
                    document.getElementById(sender._clientID).focus(true);
                    return false;
                } else if (Year == CurrentYear && Month > CurrentDate.getMonth()) {
                    alert("Cannot be future date. Please Enter a Valid Date.");
                    $find(GetClientId(sender._clientID)).clear();
                    document.getElementById(sender._clientID).focus(true);
                    return false;
                }

            } else {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).focus(true);
                return false;
            }
        } else if (inputText != null && inputText != "" && inputText.split('-')[0].length != 0) {
            var DateMonthYear = inputText.split('-');
            if (DateMonthYear[0].length < 4) {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).setSelectionRange(0, 0);
                return false;
            }

            var DateMonthYear = inputText.split('-');
            var Year = parseInt(DateMonthYear[0]);
            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            if (Year > CurrentYear) {
                alert("Cannot be future date. Please Enter a Valid Date.");
                $find(GetClientId(sender._clientID)).clear();
                document.getElementById(sender._clientID).setSelectionRange(0, 0);
                return false;
            }
        }
    }
}


function Saved(message) {
    if (message != null) {
        var contentArr = message.split(": ~");

        var messagearr = contentArr[1].split("!")[0].split('~');

        if (messagearr[0].trim() == "true") {

            var itemValue = messagearr[1];
            var toolTipText = messagearr[2];
            var ToolTip = messagearr[3];

            var sNotification = messagearr[0].trim();
            SetVitalsText(itemValue, ToolTip, toolTipText);
            $('#btnSaveVitals').prop('disabled', true);
            if (window.parent.theForm.hdnTabClick != null && window.parent.theForm.hdnTabClick != undefined) {
                var splitvalues = window.parent.theForm.hdnTabClick.value.split('$#$');
                var which_tab = splitvalues[0];
                var screen_name;
                if (which_tab.indexOf('btn') > -1) {
                    screen_name = 'MoveToButtonsClick';
                }
                else if (which_tab == 'first') {
                    screen_name = '';
                }
                else
                    screen_name = "EncounterTabClick";
                if (splitvalues.length == 3 && splitvalues[2] == "Node")
                    screen_name = 'PatientChartTreeViewNodeClick';
                SavedSuccessfully_NowProceed(screen_name);
            }
            DisplayErrorMessage('200008');
            top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
            var openingfrom = document.getElementById('hdnOpeningFrom').value;
            if (openingfrom == 'Menu')
                document.getElementById('divLoading').style.display = "none";
            else
                top.window.document.getElementById('ctl00_Loading').style.display = 'none';

            if (message.split(": ~")[1].split('!')[1] != null) {
                if (document.getElementById('lblVitalLatest Lab Results') != null) {
                    document.getElementById('lblVitalLatest Lab Results').style.visibility = "visible";
                    if (message.split(": ~")[1].split('!')[1] != '') {
                        document.getElementById('lblVitalLatest Lab Results').value = "Latest Lab Results";
                    } else if (message.split(": ~")[1].split('!')[1] == '') {
                        document.getElementById('lblVitalLatest Lab Results').value = "";
                    }
                }
                document.getElementById('lblLatest Lab Results').value = message.split(": ~")[1].split('!')[1];
                document.getElementById('lblLatest Lab Results').style.visibility = "visible";
            }

        } else if (message.split(": ~")[1].split('!')[0].trim() == "false") {
            alert(message.split(": ~")[1].split('!')[1].trim());
            var openingfrom = document.getElementById('hdnOpeningFrom').value;

            if (openingfrom == 'Menu')
                document.getElementById('divLoading').style.display = "none";
            else
                top.window.document.getElementById('ctl00_Loading').style.display = 'none';

            if ($find(message.split(':')[1].split('!')[2]) != null) {
                var DatePicker5 = $find(message.split(':')[1].split('!')[2]);
                DatePicker5.get_dateInput().focus();
            }
        }
    }
}

function SetVitalsText(itemValue, ToolTip, toolTipText) {

    var dox = window.parent.window.parent.window.document;
    var vitalsText = dox.all.ctl00_C5POBody_lblVitals;
    var pnl = dox.all.ctl00_C5POBody_pnlVitals;
    var regex = /<BR\s*[\/]?>/gi;
    ToolTip = ToolTip.replace("<br/><br/>", "<br/>").replace("<br/><br/><br/>", "<br/>");

    top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = itemValue;

    top.window.document.getElementById("Vitals_tooltp").innerText = ToolTip.replace(regex, "\n") + "\n";
    RefreshOverallSummaryTooltip();
    var sDtls = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
    document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
        "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
        window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
        window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];
    savedSuccessfully();
    RefreshNotification('Vitals');
}


function MakeStaticHeader(gridId, height, width, headerHeight, isFooter) {
    var tbl = document.getElementById(gridId);
    if (tbl) {
        var DivHR = document.getElementById('DivHeaderRow');
        var DivMC = document.getElementById('GridDiv');
        var DivFR = document.getElementById('DivFooterRow');

        //*** Set divheaderRow Properties ****
        DivHR.style.height = headerHeight + 'px';
        DivHR.style.width = (parseInt(width) - 350) + 'px';
        DivHR.style.position = 'relative';
        DivHR.style.top = '0px';
        DivHR.style.zIndex = '10';
        DivHR.style.verticalAlign = 'top';

        //*** Set divMainContent Properties ****
        DivMC.style.width = width + 'px';
        DivMC.style.height = height + 'px';
        DivMC.style.position = 'relative';
        DivMC.style.top = -headerHeight + 'px';
        DivMC.style.zIndex = '1';


        DivHR.appendChild(tbl.cloneNode(true));
        console.log($(".GridviewScrollHeader"))
    }
}
function OnScrollDiv(Scrollablediv) {
    document.getElementById('DivHeaderRow').scrollLeft = Scrollablediv.scrollLeft;
}



function OnVitalsLoad() {
    
    $('select option[type=ALL]').css("display", "none");
    $("input[Editable=No]").addClass('nonEditabletxtbox');
    $("input[Editable=Yes]").addClass('Editabletxtbox');
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=No]").addClass('LabelStyle');
    $("span[mand=Macra]").addClass('MacraLabelStyle');
    
    $("textarea[Editable=No]").removeClass('Editabletxtbox');
    $("textarea[Editable=No]").addClass('nonEditabletxtbox');
    
    $("textarea[id *= txtDLC]").removeClass('DlcClass');

    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    $("input[id *= dtp]").addClass('Editabletxtbox');
    $("input[mand=Yes]").addClass('MandLabelstyle');
    $("input[mand=No]").addClass('LabelStyle');
    $("input[mand=Macra]").addClass('MacraLabelStyle');

    $("div[id^=dvpbdropdown]").addClass("pbDropdownstyle")
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("input[GroupLabelstyle=Y]").addClass('grouplablestyle');

    $("[id*=pbDropdown]").addClass('pbDropdownBackground');

    $("[id*=Status]").addClass('Editabletxtbox');
    
    $('select').change(function () {

        if (event.currentTarget.value != undefined && event.currentTarget.value == "Click to View More") {
            var val = event.currentTarget.attributes.getNamedItem("selectType").value;
            var element = $('select[selectType=' + val + ']');
            $(event.currentTarget).find('option[type=SHOW_ALL]').css("display", "none");
            $(event.currentTarget).find('option[type=ALL]').css("display", "block");
            $(event.currentTarget).find('option:selected').removeAttr('selected');
            $('select[selectType=cboLastMammogramTest]').trigger('click');
        }

    });
    if (window.parent.theForm.hdnSaveButtonID != null && window.parent.theForm.hdnSaveButtonID != 'undefined')
        window.parent.theForm.hdnSaveButtonID.value = "btnSaveVitals";
    if (top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick') == null) {
        document.getElementById("hdnForMenuLevelCancel").value = "Null";
    } else {
        document.getElementById("hdnForMenuLevelCancel").value = "Not Null";

    }

    $(".mask").mask("9999?-aaa-99");
    $(".mask").each(function (ev) {

        if (!$(this).val()) {
            $(this).attr("placeholder", "yyyy-MMM-dd");
        }
    });
    $(".mask").blur(function (ev) {

        if ($(this).val() == "") {
            $(this).attr("placeholder", "yyyy-MMM-dd");
        }
    });
    var now = new Date();
    nowDate = new Date();
    nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();
    nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    document.getElementById(GetClientId("hdnSystemTime")).value = nowDate;

    $(".NumericUpDown").spinner({
        max: 300, min: 35, down: "custom-down-icon", up: "custom-up-icon", spin: function (event, ui) {
            $('#btnSaveVitals').prop('disabled', false);

        }

    });
    $(".VitalDateInput").datetimepicker({

        format: 'DD-MMM-YYYY hh:mm A',
        maxDate: new Date(),
        sideBySide: true

    }).on("dp.change", function (e) {
        $('#btnSaveVitals').prop('disabled', false);
        setBPStatus('', '');
    }).on('dp.show dp.update', function (e) {
        $(".datepicker-years .picker-switch").removeAttr('title')
            .on('click', function () {
                e.stopPropagation();
            });
            //Cap - 804
        if (e.currentTarget.id.indexOf('(') < 0 && e.currentTarget.id.indexOf(')') < 0) {
            $("div.bootstrap-datetimepicker-widget").css("display", "block");
            var pos = $("#" + e.currentTarget.id).offsetParent();
            var posheight = $("#" + e.currentTarget.id).position();
            var postop = pos[0].scrollTop + posheight.top;
            $("div.bootstrap-datetimepicker-widget").css({ top: postop + 15 + "px", left: posheight.left + "px", height: 300 + "px", width: 440 + "px", bgcolor: "#FFFFFF" });
            $(".bootstrap-datetimepicker-widget .timepicker-hour").css("margin-left", "15px");
            $(".bootstrap-datetimepicker-widget .timepicker-minute").css("margin-left", "15px");
            $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
            $("div.bootstrap-datetimepicker-widget").css('z-index', 3000);
        }
    });

    $(".CustomDate").datetimepicker({
        //format: 'DD-MMM-YYYY',
        format: 'YYYY-MMM-DD',
        maxDate: new Date()

    }).on("dp.change", function (e) {
        $('#btnSaveVitals').prop('disabled', false);
    }).on('dp.show dp.update', function () {
        $(".datepicker-years .picker-switch").removeAttr('title')
            .on('click', function (e) {
                e.stopPropagation();
            });
    });;

    var bmi_status_label = document.getElementById('BMI Status');
    if (bmi_status_label != null && bmi_status_label != "undefined")
        bmi_status_label.title = bmi_status_label.value;

    var blood_fasting = document.getElementById('Blood Sugar-Fasting Status');
    if (blood_fasting != null && blood_fasting != "undefined")
        blood_fasting.title = blood_fasting.value;

    var BloodFasting = document.getElementById('Blood Sugar-Fasting StatusSecond');
    if (BloodFasting != null && BloodFasting != "undefined")
        BloodFasting.title = BloodFasting.value;

    var blood_Sugar_post = document.getElementById('Blood Sugar-Post Prandial Status');
    if (blood_Sugar_post != null && blood_Sugar_post != "undefined")
        blood_Sugar_post.title = blood_Sugar_post.value;

    var blood_Sugar_post_prandial = document.getElementById('Blood Sugar-Post Prandial StatusSecond');
    if (blood_Sugar_post_prandial != null && blood_Sugar_post_prandial != "undefined")
        blood_Sugar_post_prandial.title = blood_Sugar_post_prandial.value;

    var Urine_for_Microalbumin_Status = document.getElementById('Urine for Microalbumin Status');
    if (Urine_for_Microalbumin_Status != null && Urine_for_Microalbumin_Status != "undefined")
        Urine_for_Microalbumin_Status.title = Urine_for_Microalbumin_Status.value;


    var ABI_Test_Status = document.getElementById('ABI Test Status');
    if (ABI_Test_Status != null && ABI_Test_Status != "undefined")
        ABI_Test_Status.title = ABI_Test_Status.value;

    var BP_Status_sitting = document.getElementById('BPSittingSysDiaStatus');
    if (BP_Status_sitting != null && BP_Status_sitting != "undefined") {
        BP_Status_sitting.title = BP_Status_sitting.value;
        if (BP_Status_sitting.value.toUpperCase().indexOf("NORMAL") > -1)
            BP_Status_sitting.style.color = "black";
    }

    var BPStandingSysDiaStatus = document.getElementById('BPStandingSysDiaStatus');
    if (BPStandingSysDiaStatus != null && BPStandingSysDiaStatus != "undefined") {
        BPStandingSysDiaStatus.title = BPStandingSysDiaStatus.value;
        if (BPStandingSysDiaStatus.value.toUpperCase().indexOf("NORMAL") > -1)
            BPStandingSysDiaStatus.style.color = "black";
    }

    var BPLyingSysDiaStatus = document.getElementById('BPLyingSysDiaStatus');
    if (BPLyingSysDiaStatus != null && BPLyingSysDiaStatus != "undefined") {
        BPLyingSysDiaStatus.title = BPLyingSysDiaStatus.value;
        if (BPLyingSysDiaStatus.value.toUpperCase().indexOf("NORMAL") > -1)
            BPLyingSysDiaStatus.style.color = "black";
    }

    var BPStandingSecondSysDiaStatus = document.getElementById('BPStandingSecondSysDiaStatus');
    if (BPStandingSecondSysDiaStatus != null && BPStandingSecondSysDiaStatus != "undefined") {
        BPStandingSecondSysDiaStatus.title = BPStandingSecondSysDiaStatus.value;
        if (BPStandingSecondSysDiaStatus.value.toUpperCase().indexOf("NORMAL") > -1)
            BPStandingSecondSysDiaStatus.style.color = "black";
    }

    var BPSittingSecondSysDiaStatus = document.getElementById('BPSittingSecondSysDiaStatus');
    if (BPSittingSecondSysDiaStatus != null && BPSittingSecondSysDiaStatus != "undefined") {
        BPSittingSecondSysDiaStatus.title = BPSittingSecondSysDiaStatus.value;
        if (BPSittingSecondSysDiaStatus.value.toUpperCase().indexOf("NORMAL") > -1)
            BPSittingSecondSysDiaStatus.style.color = "black";
    }

    var BPLyingSecondSysDiaStatus = document.getElementById('BPLyingSecondSysDiaStatus');
    if (BPLyingSecondSysDiaStatus != null && BPLyingSecondSysDiaStatus != "undefined") {
        BPLyingSecondSysDiaStatus.title = BPLyingSecondSysDiaStatus.value;
        if (BPLyingSecondSysDiaStatus.value.toUpperCase().indexOf("NORMAL") > -1)
            BPLyingSecondSysDiaStatus.style.color = "black";
    }





    showhidBP();
    enabledisableBMI();
    $("#txtNotesHeight_txtDLC,#txtNotesWeight_txtDLC").on('keyup', function (e) {
        var notes = "";
        //CAP-3972
        if (document?.getElementById('txtNotesBMI_txtDLC') != undefined && document?.getElementById('txtNotesBMI_txtDLC') != null) {
            document.getElementById('txtNotesBMI_txtDLC').value = "";
        }
        if (document.getElementById('Weight').value == "") {
            var heightnotes = document.getElementById('txtNotesWeight_txtDLC').value.split(',');
            var resason = document.getElementById('hdnreason').value.split('~');
            for (var i = 0; i < heightnotes.length; i++) {
                for (var j = 0; j < resason.length; j++) {
                    if (resason[j].split('|')[0].indexOf("WEIGHT") > -1 && resason[j].split('|')[1] == heightnotes[i].trim()) {
                        if (document.getElementById('txtNotesBMI_txtDLC').value == "") {
                            document.getElementById('txtNotesBMI_txtDLC').value = heightnotes[i].trim();
                            notes = heightnotes[i].trim();
                        }
                        else {
                            if (notes.indexOf(heightnotes[i].trim()) == -1) {

                                notes = notes + ", " + heightnotes[i].trim();
                                document.getElementById('txtNotesBMI_txtDLC').value = notes;

                            }

                        }
                    }
                }

            }
        }
        if (document.getElementById('Height').value == "") {
            var heightnotes = document.getElementById('txtNotesHeight_txtDLC').value.split(',');
            var resason = document.getElementById('hdnreason').value.split('~');
            for (var i = 0; i < heightnotes.length; i++) {
                for (var j = 0; j < resason.length; j++) {
                    if (resason[j].split('|')[0].indexOf("HEIGHT") > -1 && resason[j].split('|')[1] == heightnotes[i].trim()) {
                        if (notes == '' && notes.indexOf(heightnotes[i].trim()) == -1) {
                            document.getElementById('txtNotesBMI_txtDLC').value = heightnotes[i].trim();
                        }
                        else {
                            if (notes.indexOf(heightnotes[i].trim()) == -1) {

                                notes = notes + ", " + heightnotes[i].trim();
                                document.getElementById('txtNotesBMI_txtDLC').value = notes;

                            }

                        }
                    }
                }

            }
        }
    });
    //Jira #CAP-733
    var sVisitType = "";
    var sDefaulttext = "Not performed - Telemedicine";

    if ($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0] != undefined && $(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0] != null && $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument) != null &&
        $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument) != undefined) {
        if (($($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#pnlBarGroupTabs") != null) &&
            $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#pnlBarGroupTabs") != undefined) {

            if ($($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#pnlBarGroupTabs")[0] != undefined && $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#pnlBarGroupTabs")[0] != null &&  $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#pnlBarGroupTabs")[0].innerHTML.split('|').length > 2)

                sVisitType = $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("#pnlBarGroupTabs")[0].innerHTML.split('|')[2].trim()
        }
    }


    document.getElementById('hdnVisittype').value = sVisitType;
    //Jira #CAP-913
    //if (sVisitType.toUpperCase() == "TELEMEDICINE" )
    if (sVisitType.toUpperCase() == "TELEMEDICINE" && window.location.href.split('?')[1].split('&')[0].split('=')[1].toUpperCase() != "MENU") {

        if (document.getElementById('Height').value == "" && document.getElementById('HeightInch').value == "" && document.getElementById('txtNotesHeight_txtDLC').value=="") {
            document.getElementById('txtNotesHeight_txtDLC').value = sDefaulttext;
            EnableSave(true);

           // $('#btnSaveVitals').prop('disabled', false);
        }
        if (document.getElementById('Weight').value == "" && document.getElementById('txtNotesWeight_txtDLC').value == "" ) {
            document.getElementById('txtNotesWeight_txtDLC').value = sDefaulttext;
            EnableSave(true);
          //  $('#btnSaveVitals').prop('disabled', false);
        }
        if (document.getElementById('BMI').value == "" && document.getElementById('txtNotesBMI_txtDLC').value=="") {
            document.getElementById('txtNotesBMI_txtDLC').value = sDefaulttext;
          //  $('#btnSaveVitals').prop('disabled', false);
            EnableSave(true);

        }
        if (document.getElementById('BPSittingSysDia').value == "" && document.getElementById('txtNotesBPSittingSysDia_txtDLC').value =="" ) {
            document.getElementById('txtNotesBPSittingSysDia_txtDLC').value = sDefaulttext;
           // $('#btnSaveVitals').prop('disabled', false);
            EnableSave(true);
        }


    }

    $("#Height,#Weight,#BPSittingSysDia,#BPSittingDiastolic,#HeightInch").on('keyup', function (e) {
         //Jira #CAP-913
    //if (sVisitType.toUpperCase() == "TELEMEDICINE" )
        if (sVisitType.toUpperCase() == "TELEMEDICINE" && window.location.href.split('?')[1].split('&')[0].split('=')[1].toUpperCase() != "MENU") {

            if (document.getElementById('Height').value != "" || document.getElementById('HeightInch').value != "")
                document.getElementById('txtNotesHeight_txtDLC').value = document.getElementById('txtNotesHeight_txtDLC').value.replace(sDefaulttext, '');
            else
                document.getElementById('txtNotesHeight_txtDLC').value = sDefaulttext;

            if (document.getElementById('Weight').value != "")
                document.getElementById('txtNotesWeight_txtDLC').value = document.getElementById('txtNotesWeight_txtDLC').value.replace(sDefaulttext, '');
            else
                document.getElementById('txtNotesWeight_txtDLC').value = sDefaulttext;

            if (document.getElementById('BMI').value != "")
                document.getElementById('txtNotesBMI_txtDLC').value = document.getElementById('txtNotesBMI_txtDLC').value.replace(sDefaulttext, '');
            else
                document.getElementById('txtNotesBMI_txtDLC').value = sDefaulttext;


            if (document.getElementById('BPSittingSysDia').value != "")
                document.getElementById('txtNotesBPSittingSysDia_txtDLC').value = document.getElementById('txtNotesBPSittingSysDia_txtDLC').value.replace(sDefaulttext, '');
            else
                document.getElementById('txtNotesBPSittingSysDia_txtDLC').value = sDefaulttext;






        }
    });
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function test() {
    if (document.getElementById('spanplus') != undefined)
        document.getElementById('spanplus').style.display = "none";

    $('#trBPStandingSysDia').removeClass('displayprop');
    $('#tr1BPStandingSysDia').removeClass('displayprop');
    $('#trBPStandingSecondSysDia').removeClass('displayprop');
    $('#tr1BPStandingSecondSysDia').removeClass('displayprop');
    $('#trBPLyingSysDia').removeClass('displayprop');
    $('#tr1BPLyingSysDia').removeClass('displayprop');
    $('#trBPLyingSecondSysDia').removeClass('displayprop');
    $('#tr1BPLyingSecondSysDia').removeClass('displayprop');
    $('#lblVitalBPSittingSecondSysDia').css('padding-left', '7px');
    $('#lblVitalBPSittingSecondSysDia').css('width', '150px')



}
function showhidBP() {
    if ((document.getElementById('BPStandingSysDia') != undefined && document.getElementById('BPStandingSysDia').value == "")
        && (document.getElementById('BPStandingSecondSysDia') != undefined && document.getElementById('BPStandingSecondSysDia').value == "")
        && (document.getElementById('BPLyingSysDia') != undefined && document.getElementById('BPLyingSysDia').value == "")
        && (document.getElementById('BPLyingSecondSysDia') != undefined && document.getElementById('BPLyingSecondSysDia').value == "")) {

        $('#trBPStandingSysDia').addClass('displayprop');
        $('#tr1BPStandingSysDia').addClass('displayprop');
        $('#trBPStandingSecondSysDia').addClass('displayprop');
        $('#tr1BPStandingSecondSysDia').addClass('displayprop');
        $('#trBPLyingSysDia').addClass('displayprop');
        $('#tr1BPLyingSysDia').addClass('displayprop');
        $('#trBPLyingSecondSysDia').addClass('displayprop');
        $('#tr1BPLyingSecondSysDia').addClass('displayprop');

        if (document.getElementById('spanplus') != undefined)
            document.getElementById('spanplus').style.display = "inline-block";

    }
    else {

        if (document.getElementById('spanplus') != undefined)
            document.getElementById('spanplus').style.display = "none";
        $('#lblVitalBPSittingSecondSysDia').css('padding-left', '7px');
        $('#lblVitalBPSittingSecondSysDia').css('width', '150px')
    }
}
function displaysnomedalert(id) {

    alert('Please capture either Value or select a valid reason not performed for ' + id);

    return false;
}
function enabledisableBMI() {
    if (document.getElementById('BMI') != undefined) {
        if (document.getElementById('BMI').value == "") {
            if (document.getElementById('txtNotesBMI_txtDLC') != undefined) {
                $('#txtNotesBMI_txtDLC').removeClass('Editabletxtbox');
                $('#txtNotesBMI_txtDLC').addClass('nonEditabletxtbox');
               document.getElementById('txtNotesBMI_txtDLC').readOnly = true;
            }
            if (document.getElementById('txtNotesBMI_pbDropdown') != undefined)
                document.getElementById('txtNotesBMI_pbDropdown').disabled = true;

        }
        else {
            if (document.getElementById('txtNotesBMI_txtDLC') != undefined) {
                $('#txtNotesBMI_txtDLC').removeClass('nonEditabletxtbox');
                $('#txtNotesBMI_txtDLC').addClass('Editabletxtbox');
                $('#txtNotesBMI_txtDLC').addClass('Editabletxtbox');
                document.getElementById('txtNotesBMI_txtDLC').readOnly = false;
            }
            if (document.getElementById('txtNotesBMI_pbDropdown') != undefined)
                document.getElementById('txtNotesBMI_pbDropdown').disabled = false;
        }
    }
}
function NotAllowdecimal(evt) {

    $('#btnSaveVitals').prop('disabled', false);
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function SetFocusDateTime(valuess) {
    document.getElementById(valuess + '_txtHour').focus();
}

function CurrentSystemTime() {
    var now = new Date();
    nowDate = new Date();
    nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();
    nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    document.getElementById(GetClientId("hdnSystemTime")).value = nowDate;
}


function ForClosingVitalsForYes() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    if (oWindow == null || oWindow == undefined) {
        window.parent.document.getElementsByTagName("iFrame")[1].contentWindow.$telerik.radControls[0].BrowserWindow.parent.GetRadWindow().Close();
    } else {
        oWindow.close();
    }
}
function EnableSaveFocus(event) {
    if (event != null) {
        if (event.srcElement != null) {
            if (event.srcElement.id != null)
                event.srcElement.focus(true);
        }
    }
}
//CAP-967
function closePopup() {
    parent.window.close();
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function btnClose_Clicked() {
    //CAP-967
    if (document.getElementById('btnSaveVitals').disabled == false) {
        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
            "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');
        event.preventDefault();
        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'center center',
                at: 'center center'
            },
            buttons: {
                "Yes": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    //CAP-967
                    document.getElementById('hdnType').value = "Yes";
                    btnSave_Clicked();
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    parent.window.close();
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    return false;
                }
            }
        });
    } else {
        parent.window.close();
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }
}
function autosavevitals() {

    if (document.getElementById("hdnMessageType").value == "Yes") {
        var now = new Date();
        var utc = now.toUTCString();
        document.getElementById(GetClientId("hdnLocalTime")).value = utc;
        nowDate = new Date();
        nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear(); nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        document.getElementById(GetClientId("hdnSystemTime")).value = nowDate;
        DisplayErrorMessage('200002');
        document.getElementById(GetClientId("hdnMessageType")).value = "";
        parent.window.close();
    }
    else if (document.getElementById("hdnMessageType").value == "No") {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        else
            window.parent.theForm.hdnSaveEnable.value = "false";
        document.getElementById("hdnMessageType").value = "";
        parent.window.close();
    }
    else if (document.getElementById("hdnMessageType").value == "Cancel") {
        document.getElementById("hdnMessageType").value = "";
        return false;
    }
}

Array.prototype.remove = function () {
    var what, a = arguments, L = a.length, ax;
    while (L && this.length) {
        what = a[--L];
        while ((ax = this.indexOf(what)) !== -1) {
            this.splice(ax, 1);
        }
    }
    return this;
};


function ChangeStatusLabel() {
    $(".VitalDateInput").datetimepicker({
        format: 'DD-MMM-YYYY hh:mm A',
        sideBySide: true,
    }).on("dp.change", function (e) {
        $('#btnSaveVitals').prop('disabled', false);
    }).on('dp.show dp.update', function (e) {
        $(".datepicker-years .picker-switch").removeAttr('title')
            .on('click', function (e) {
                e.stopPropagation();
            });
        $("div.bootstrap-datetimepicker-widget").css("display", "block");
        var pos = $("#" + e.currentTarget.id).offsetParent();
        var posheight = $("#" + e.currentTarget.id).position();
        var postop = pos[0].scrollTop + posheight.top;
        $("div.bootstrap-datetimepicker-widget").css({ top: postop + 15 + "px", left: posheight.left + "px", height: 300 + "px", width: 440 + "px", bgcolor: "#FFFFFF" });
        $(".bootstrap-datetimepicker-widget .timepicker-hour").css("margin-left", "15px");
        $(".bootstrap-datetimepicker-widget .timepicker-minute").css("margin-left", "15px");
        $(".bootstrap-datetimepicker-widget .btn").css({ "width": "42px", "margin-left": "5px" });
        $("div.bootstrap-datetimepicker-widget").css('z-index', 3000);
    });
    $(".CustomDate").datetimepicker({
        format: 'YYYY-MMM-DD',
        maxDate: new Date()

    }).on("dp.change", function (e) {
        $('#btnSaveVitals').prop('disabled', false);
    }).on('dp.show dp.update', function () {
        $(".datepicker-years .picker-switch").removeAttr('title')
            
            .on('click', function (e) {
                e.stopPropagation();
            });
    });;

    $(".NumericUpDown").spinner({
        max: 300, min: 35, down: "custom-down-icon", up: "custom-up-icon", spin: function (event, ui) {
            $('#btnSaveVitals').prop('disabled', false);

        }
    });
    var statuslength = statusid.length;

    for (i = 0; i < statuslength; i++) {

        var statusLabel = document.getElementById(statusid[i]);
        if (statusLabel != undefined) {
            statusLabel.style.color = "red";
        }

    }
    if (document.getElementById("hdnLabResults") != undefined)
        $("[id*='lblLatest Lab Results']")[0].value = document.getElementById("hdnLabResults").value;

    if (document.getElementById("hdnLabResults") != undefined && document.getElementById("hdnLabResults").value != "")
        $("[id*='lblVitalLatest Lab Results']")[0].value = "Latest Lab Results";
    else
        $("[id*='lblVitalLatest Lab Results']").css("display", "none");


}

function setBPStatus(idsys, iddia) {
    EnableSave(event);
    var controlscount = $('#tblVitalControls').find('input[type="text"]').length;

    var default_date = "";
    var arraydate = [];
    var flag = 0;
    var flagdatecheck = 0;
    var a = "", b = "", c = "", d = "", e = "", f = "";
    if (document.getElementById('dtpBPSittingSysDia') != undefined)
        a = document.getElementById('dtpBPSittingSysDia').value;
    if (document.getElementById('dtpBPSittingSecondSysDia') != undefined)
        b = document.getElementById('dtpBPSittingSecondSysDia').value;
    if (document.getElementById('dtpBPStandingSysDia') != undefined)
        c = document.getElementById('dtpBPStandingSysDia').value;
    if (document.getElementById('dtpBPStandingSecondSysDia') != undefined)
        d = document.getElementById('dtpBPStandingSecondSysDia').value;
    if (document.getElementById('dtpBPLyingSysDia') != undefined)
        e = document.getElementById('dtpBPLyingSysDia').value;
    if (document.getElementById('dtpBPLyingSecondSysDia') != undefined)
        f = document.getElementById('dtpBPLyingSecondSysDia').value;


    var values = $('#tblVitalControls').find('input[id ^= "dtpBP"]').map(function () {
        return this.value
    }).get();
    for (var i = 0; i < values.length - 1; i++) {
        if (values[i] !== values[i + 1]) {
            flagdatecheck = 1
            break;
        }
    }

    for (var i = 0; i < controlscount; i++) {

        var idname = $('#tblVitalControls').find('input[type="text"]')[i].id;


        if (idname != undefined && idname != "" && (idname.toUpperCase().indexOf('BP') > -1) && (idname.toUpperCase().indexOf('STATUS') > -1)) {
            var sysvalue = document.getElementById(idname.replace('Status', ''));
            var diavalue = document.getElementById(idname.replace('Status', '').replace("SysDia", "Diastolic"));
            var sysvalye = sysvalue.value;
            var diaval = diavalue.value;

            //Jira Cap-430 - BP status changes when the time value of the second BP is modified
            //if ((parseFloat(sysvalye) >= 140 || parseFloat(diaval) >= 90) && idname.indexOf("Sitting") > -1) {
            if ((parseFloat(sysvalye) >= 120 || parseFloat(diaval) >= 80) && idname.indexOf("Sitting") > -1) {
                if (idname.indexOf("Second") > -1) {
                    arraydate.push(b);
                }// arraydate.splice(2);
                else {
                    arraydate.push(a);
                }


            }
            //Jira Cap-430 - BP status changes when the time value of the second BP is modified
            //if ((parseFloat(sysvalye) >= 140 || parseFloat(diaval) >= 90) && idname.indexOf("Standing") > -1) {
            if ((parseFloat(sysvalye) >= 120 || parseFloat(diaval) >= 80) && idname.indexOf("Standing") > -1) {
                if (idname.indexOf("Second") > -1)
                    arraydate.push(d);//arraydate.splice(4);
                else
                    arraydate.push(c);


            }
            //Jira Cap-430 - BP status changes when the time value of the second BP is modified
           // if ((parseFloat(sysvalye) >= 140 || parseFloat(diaval) >= 90) && idname.indexOf("Lying") > -1) {
            if ((parseFloat(sysvalye) >= 120 || parseFloat(diaval) >= 80) && idname.indexOf("Lying") > -1) {
                if (idname.indexOf("Second") > -1)
                    arraydate.push(f);
                else
                    arraydate.push(e);


            }
            arraydate.sort(function (a, b) {
                var dateA = new Date(a), dateB = new Date(b)
                return dateA - dateB
            })
            default_date = arraydate[0];
        }
    }
    var sHumanDetails;
    var age = 16;
    var dob=""
    //if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined) {
    //     sHumanDetails = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
    //     age = sHumanDetails.split('|')[2].trim().split(' ')[0];
    //     dob = sHumanDetails.split('|')[1].trim();

    //}
    for (var i = 0; i < controlscount; i++) {

        var idname = $('#tblVitalControls').find('input[type="text"]')[i].id;



        if (idname != undefined && idname != "" && (idname.toUpperCase().indexOf('BP') > -1) && (idname.toUpperCase().indexOf('STATUS') > -1)) {

            var statusLabel = document.getElementById(idname);
            var sysvalue = document.getElementById(idname.replace('Status', ''));
            var diavalue = document.getElementById(idname.replace('Status', '').replace("SysDia", "Diastolic"));
            if (statusLabel != undefined && sysvalue != undefined && diavalue != undefined) {

                var sysvalye = sysvalue.value;
                var diaval = diavalue.value;
                if (sysvalye != "" && diaval != "") {
                    //if (parseFloat(age) > 15) {
                        if (document.getElementById('hdnBPValue').value == "I10") {
                            statusLabel.value = " ";
                            statusLabel.title = " ";
                        }
                        else {
                            if (parseFloat(sysvalye) < 120 && (parseFloat(diaval) < 80)) {
                                statusLabel.title = "Normal"
                                statusLabel.value = "Normal";
                                default_date = "";

                            }
                            //if ((parseFloat(sysvalye) >= 120 && parseFloat(sysvalye) < 140)) {
                            //    if (parseFloat(diaval) < 90) {
                            //        statusLabel.title = "Pre-Hypertensive"
                            //        statusLabel.value = "Pre-Hypertensive";
                            //        default_date = "";

                            //    }
                            //}
                            if ((parseFloat(sysvalye) >= 120 && parseFloat(sysvalye) <= 129) && parseFloat(diaval) < 80) {
                               // if (parseFloat(diaval) < 80) {
                                    statusLabel.title = "Elevated";
                                    statusLabel.value = "Elevated";
                                    default_date = "";

                                //}
                            }




                            //if ((parseFloat(diaval) >= 80) && parseFloat(diaval) < 90 && parseFloat(sysvalye) < 140) {

                            //    statusLabel.title = "Pre-Hypertensive"
                            //    statusLabel.value = "Pre-Hypertensive";
                            //    default_date = "";

                            //}
                            //if (parseFloat(diaval) < 80 && parseFloat(sysvalye) < 129) {

                            //    statusLabel.title = "Elevated";
                            //    statusLabel.value = "Elevated";
                            //    default_date = "";

                            //}
                            //else if (parseFloat(sysvalye) >= 140 || parseFloat(diaval) >= 90) {
                            if (parseFloat(sysvalye) >= 130 || parseFloat(diaval) >= 80) {


                                if (document.getElementById('hdnBPValue').value != "" && flagdatecheck == 0) {
                                    var recentvalues = document.getElementById('hdnBPValue').value.split('|')

                                    for (var g = 0; g < recentvalues.length; g++) {
                                        var stat = recentvalues[g].split(':')[0].replace('/', '').replace('-', '').replace(' ', '')
                                        if (recentvalues[g].split(':')[1].split('/')[0] >= 130 || recentvalues[g].split(':')[1].split('/')[1] >= 80) {
                                            statusLabel.title = "Second Hypertensive";
                                            statusLabel.value = "Second Hypertensive";
                                            flag = 1;
                                            break;
                                        }
                                    }
                                    if (flag == 0) {
                                        for (var g = 0; g < recentvalues.length; g++) {
                                            //if (recentvalues[g].split(':')[1].split('/')[0] < 140 && recentvalues[g].split(':')[1].split('/')[1] < 90) {
                                            //    statusLabel.title = "First Hypertensive";
                                            //    statusLabel.value = "First Hypertensive";

                                            //    break;
                                            //}
                                            //else {
                                            //    //  g++;
                                            //    if (g == recentvalues.length - 1) {
                                            //        statusLabel.title = "First Hypertensive";
                                            //        statusLabel.value = "First Hypertensive";

                                            //    }
                                            //}
                                            if (recentvalues[g].split(':')[1].split('/')[0] < 130 || recentvalues[g].split(':')[1].split('/')[1] < 80) {
                                                statusLabel.title = "First Hypertensive";
                                                statusLabel.value = "First Hypertensive";

                                                break;
                                            }
                                            else {
                                                //  g++;
                                                if (g == recentvalues.length - 1) {
                                                    statusLabel.title = "First Hypertensive";
                                                    statusLabel.value = "First Hypertensive";

                                                }
                                            }
                                        }

                                    }
                                }
                                else if (document.getElementById('hdnBPValue').value == "" && flagdatecheck == 0) {
                                    statusLabel.value = "First Hypertensive";
                                    statusLabel.title = "First Hypertensive";
                                }

                                else if (document.getElementById('hdnBPValue').value == "" && flagdatecheck == 1) {
                                    if (default_date == "" || default_date == undefined) {
                                        arraydate.sort(function (a, b) { // sort object by retirement date
                                            var dateA = new Date(a), dateB = new Date(b)
                                            return dateA - dateB //sort by date ascending
                                        })
                                        default_date = arraydate[0];

                                    }

                                    if (new Date(document.getElementById("dtp" + idname.replace("Status", "")).value) <= new Date(default_date)) {
                                        statusLabel.value = "First Hypertensive";
                                        statusLabel.title = "First Hypertensive";
                                    }

                                    else {
                                        statusLabel.title = "Second Hypertensive";
                                        statusLabel.value = "Second Hypertensive";
                                    }


                                }



                                else {
                                    if (default_date == "") {
                                        arraydate.sort(function (a, b) { // sort object by retirement date
                                            var dateA = new Date(a), dateB = new Date(b)
                                            return dateA - dateB //sort by date ascending
                                        })
                                        default_date = arraydate[0];

                                    }

                                    var recentvalues = document.getElementById('hdnBPValue').value.split('|')

                                    for (var g = 0; g < recentvalues.length; g++) {
                                        var stat = recentvalues[g].split(':')[0].replace('/', '').replace('-', '').replace(' ', '')

                                        if (recentvalues[g].split(':')[1].split('/')[0] >= 130 || recentvalues[g].split(':')[1].split('/')[1] >= 80) {
                                            statusLabel.title = "Second Hypertensive";
                                            statusLabel.value = "Second Hypertensive";
                                            flag = 1;
                                            break;
                                        }
                                    }
                                    if (flag == 0 && new Date(document.getElementById("dtp" + idname.replace("Status", "")).value) <= new Date(default_date)) {
                                        statusLabel.value = "First Hypertensive";
                                        statusLabel.title = "First Hypertensive";
                                    }

                                    else {
                                        statusLabel.title = "Second Hypertensive";
                                        statusLabel.value = "Second Hypertensive";
                                    }
                                }



                            }


                        }
                  //  }
                    //else {
                    //     if (parseFloat(age) == 0  && parseFloat(diaval) >= 37 && parseFloat(diaval) <= 56 && parseFloat(sysvalye) >= 72 && parseFloat(sysvalye) <= 104) {

                    //            statusLabel.value = "Normal";
                    //            statusLabel.title = "Normal";
                            
                    //    }
                    //    else if ((parseFloat(age) ==1  || parseFloat(age) ==2 )  && parseFloat(diaval) >= 42 && parseFloat(diaval) <= 63 && parseFloat(sysvalye) >= 86 && parseFloat(sysvalye) <= 106) {
                    //          statusLabel.value = "Normal";
                    //            statusLabel.title = "Normal";
                            
                    //    }
                    //    else if (parseFloat(age) >= 3 && parseFloat(age) <= 5 && parseFloat(diaval) >= 46 && parseFloat(diaval) <= 72 && parseFloat(sysvalye) >= 89 && parseFloat(sysvalye) <= 112) {
                    //            statusLabel.value = "Normal";
                    //            statusLabel.title = "Normal";
                    //        }
                        
                    //    else if (parseFloat(age) >= 6 && parseFloat(age) <= 9   && parseFloat(diaval) >= 57 && parseFloat(diaval) <= 76 && parseFloat(sysvalye) >= 97 && parseFloat(sysvalye) <= 115) {
                    //            statusLabel.value = "Normal";
                    //            statusLabel.title = "Normal";
                    //        }
                        
                    //    else if (parseFloat(age) >= 10 && parseFloat(age) <= 11 && parseFloat(diaval) >= 61 && parseFloat(diaval) <= 80 && parseFloat(sysvalye) >= 100 && parseFloat(sysvalye) <= 120) {
                    //           statusLabel.value = "Normal";
                    //            statusLabel.title = "Normal";
                    //        }
                        
                    //        else if (parseFloat(age) >= 12 && parseFloat(age) <= 15 && parseFloat(diaval) >= 63 && parseFloat(diaval) <= 83 && parseFloat(sysvalye) >= 115 && parseFloat(sysvalye) <= 131) {
                           
                    //            statusLabel.value = "Normal";
                    //            statusLabel.title = "Normal";
                            
                    //    }
                    //    else
                    //    {
                    //        statusLabel.value = "Abnormal";
                    //        statusLabel.title = "Abnormal";
                    //    }

                    //}
                }
                else {
                    statusLabel.value = "";
                    statusLabel.title = "";
                }

                statusLabel.style.color = "red";

                if (statusLabel.value.length > 15) {
                    statusLabel.style.height = "29px";
                } else {
                    statusLabel.style.height = "18px";
                }
                if (statusLabel.value.toUpperCase().indexOf("ABNORMAL") < 0 &&  statusLabel.value.toUpperCase().indexOf("NORMAL") > -1) {

                    statusLabel.style.color = "black";

                }




            }

        }

    }

    if (idsys != "" && iddia != "")
        RemoveBPReason(idsys, iddia);
}

function RemoveBPReason(idsys, iddia) {
    var sysvalye = document.getElementById(idsys).value;
    var diaval = document.getElementById(iddia).value;

    if (sysvalye != "" && diaval != "") {
        if (document.getElementById('txtNotes' + idsys + '_txtDLC').value != "") {

            var heightnotes = document.getElementById('txtNotes' + idsys + '_txtDLC').value.split(',');
            var resason = document.getElementById('hdnreason').value.split('~');
            for (var i = 0; i < heightnotes.length; i++) {
                for (var j = 0; j < resason.length; j++) {
                    if (resason[j].split('|')[0].indexOf("BP-SITTING SYS/DIA") > -1 && resason[j].split('|')[1] == heightnotes[i].trim()) {
                        if ((heightnotes.length == 0 && i == 0) || i == heightnotes.length - 1)
                            document.getElementById('txtNotes' + idsys + '_txtDLC').value = document.getElementById('txtNotes' + idsys + '_txtDLC').value.replace(heightnotes[i].trim(), "");

                        else
                            document.getElementById('txtNotes' + idsys + '_txtDLC').value = document.getElementById('txtNotes' + idsys + '_txtDLC').value.replace(heightnotes[i].trim() + ", ", "");
                    }
                }

            }
        }
    }
}

function ShowAll() {

}
//BugID:48015
function DefaultTest(event) {
    if (event != null && event != undefined && event.currentTarget.id.indexOf("CDPLastMammogram") != -1 && $("#CDPLastMammogramDATEPICKER")[0].value.trim() != "") {
        var Notes_Val = $("#txtNotesLastMammogram_txtDLC")[0].value.trim();
        var SelectedVal = "";
        var TextBoxVal = Notes_Val;
        var Type = "DefaultTest";
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/FindIfInMammogramTestList",

            data: "{'SelectedText':'" + SelectedVal + "','TextBoxValue':'" + TextBoxVal + "','Type':'" + Type + "'}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var result = response.d;
                if (result != "") {
                    if ($("#txtNotesLastMammogram_txtDLC")[0].value.trim() != "")
                        $("#txtNotesLastMammogram_txtDLC")[0].value = $("#txtNotesLastMammogram_txtDLC")[0].value + ", " + result;
                    else
                        $("#txtNotesLastMammogram_txtDLC")[0].value = result;
                }
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
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
}

//BugID:51648-- hgb field
function SetHGBStatus(hbValue, controlID, statusLabelId) {
    var HB = '';
    var age = "", sex = "";
    if ($(top.window.document).find("#ctl00_C5POBody_lblPatientStrip")[0].textContent != undefined) {
        age = $(top.window.document).find("#ctl00_C5POBody_lblPatientStrip")[0].textContent.split("|")[2].trim().split(" ")[0];
        sex = $(top.window.document).find("#ctl00_C5POBody_lblPatientStrip")[0].textContent.split("|")[3].trim().split(" ")[0];
    }

    if (hbValue != '') {
        if (HGB_STATUS != undefined && HGB_STATUS.length > 0) {
            for (var i = 0; i < HGB_STATUS.length; i++) {
                if (HGB_STATUS[i].lowerLimit != undefined && HGB_STATUS[i].upperLimit != undefined) {
                    if (parseFloat(age) >= parseFloat(HGB_STATUS[i].description.split('$')[0].split('|')[0].split('-')[0]) && parseFloat(age) < parseFloat(HGB_STATUS[i].description.split('$')[0].split('|')[0].split('-')[1])) {
                        if (HGB_STATUS[i].description.split('$')[0].split('|')[1].length == 1) {
                            if (HGB_STATUS[i].description.split('$')[0].split('|')[1] == sex) {
                                if (parseFloat(hbValue) >= parseFloat(HGB_STATUS[i].lowerLimit) && parseFloat(hbValue) <= parseFloat(HGB_STATUS[i].upperLimit)) {
                                    HB = HGB_STATUS[i].description.split('$')[1].split('|')[0];
                                    document.getElementById(GetClientId("hdnHgb")).value = HB;
                                    break;
                                }
                                else {
                                    HB = HGB_STATUS[i].description.split('$')[1].split('|')[1];
                                    document.getElementById(GetClientId("hdnHgb")).value = HB;
                                    break;
                                }
                            }
                        }
                        else {
                            if (parseFloat(hbValue) >= parseFloat(HGB_STATUS[i].lowerLimit) && parseFloat(hbValue) <= parseFloat(HGB_STATUS[i].upperLimit)) {
                                HB = HGB_STATUS[i].description.split('$')[1].split('|')[0];
                                document.getElementById(GetClientId("hdnHgb")).value = HB;
                                break;
                            }
                            else {
                                HB = HGB_STATUS[i].description.split('$')[1].split('|')[1];
                                document.getElementById(GetClientId("hdnHgb")).value = HB;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    var statusLabel = document.getElementById(statusLabelId);
    if (statusLabel != undefined) {

        statusLabel.title = HB;
        statusLabel.value = HB;
        statusLabel.style.color = "red";
        statusid.push(statusLabelId);
        if (statusLabel.value.length > 15) {
            statusLabel.style.height = "29px";
        } else {
            statusLabel.style.height = "18px";
        }
        if (HGB_COLOR != undefined && HGB_COLOR.length > 0) {
            for (var i = 0; i < HGB_COLOR.length; i++) {
                if (HGB_COLOR[i].description != undefined && HGB_COLOR[i].Value != undefined) {
                    if (HGB_COLOR[i].Value == statusLabelId) {
                        if (HB.toUpperCase().trim() == HGB_COLOR[i].description.toUpperCase().trim()) {
                            statusLabel.style.color = "black";
                            statusid.remove(statusLabelId);
                            break;
                        }
                    }
                }
            }
        }
    }

    document.getElementById(GetClientId("hdnHgb")).value += "+" + statusLabel.style.color;
}