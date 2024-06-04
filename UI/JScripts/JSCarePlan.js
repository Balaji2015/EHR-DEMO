function EnableSave() {
    document.getElementById('btnSave').disabled = false;
    localStorage.setItem("bSave", "false");
    if (document.getElementById("hdnEnableYesNo") != null) {
        document.getElementById("hdnEnableYesNo").value = "true";
    }
    if (top.window.document.getElementById("ctl00_C5POBody_hdnIsSaveEnable") != undefined && top.window.document.getElementById("ctl00_C5POBody_hdnIsSaveEnable") != null) {
        top.window.document.getElementById("ctl00_C5POBody_hdnIsSaveEnable").value = "true";
    }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    else
        window.parent.theForm.hdnSaveEnable.value = "true";
}

function displayall(id) {
    $('.' + id).css('display', 'block');
    $('.' + id + "test").css('display', 'none');
}

var hdnFieldName = null;
function callweb(icon, List, id) {

    var check = $('#' + id)[0].parentElement.parentElement.children[1].children[0].value;
    var bpstatus = "";
    var bmistatus = "";
    if (List.toUpperCase().indexOf("BP SYS/DIA") > -1) {
        if (check.toUpperCase().indexOf("NORMAL") > -1)
            bpstatus = "NORMAL"
    }
    if (List.toUpperCase() == "BMI") {
        if (check.toUpperCase().indexOf("NORMAL") > -1)
            bmistatus = "NORMAL"
    }
    $('#btnSave')[0].disabled = false;
    EnableSave();
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");

        var ListValue = List;
        if ((check == "" || (ListValue.toUpperCase().indexOf("TOBACCO") > -1 && localStorage.getItem('Tobacco').toString().toUpperCase() == "NO") || bpstatus == "NORMAL" || bmistatus == "NORMAL" )&&
            (ListValue.toUpperCase().indexOf("HOSPICE") < 0 && ListValue.toUpperCase().indexOf("PALLIATIVE")) < 0) {
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
                    var FacilityRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value;
                    if (FacilityRole != "") {
                        if (FacilityRole.split('&')[1] != "" && (FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN" || FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN ASSISTANT")) {
                            innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;font-weight:bolder;cursor:default' ;\" disabled>Comments</li>";
                            innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                        }
                    }
                    for (var i = 0; i < values.length ; i++) {

                        innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"fun('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45541
                    }

                    var listlength = innerdiv.length;
                    if (listlength > 0) {
                        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                                document.getElementsByTagName("div")[i].hidden = true;
                            }
                        }
                         //CAP-804 Syntax error, unrecognized expression
                        $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                          .css({
                              top: pos.top + $(".actcmpt").height() + 15,
                              left: pos.left,
                              width: pos.width,
                              height: '150px',
                              position: 'absolute',
                              background: 'white',
                              bottom: '0',
                              floating: 'top',
                              width: '361px',
                              border: '1px solid #8e8e8e',
                              background: '#FFF',
                              fontFamily: 'Segoe UI",Arial,sans-serif',
                              fontSize: '12px',
                              zIndex: '17',
                              overflowX: 'auto'

                          }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                    }
                    EnableSave();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                },
                error: function OnError(xhr) {
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //             ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //            "Message: " + log.Message);

                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                    }

                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
        }
        else {
            var flag = 0;

            var flagfollow = 0;
            var reason = 0;


            $.ajax({
                type: "POST",
                url: "frmDLC.aspx/GetListBoxcareplanReason",
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
                    var FacilityRole = window.parent.parent.parent.theForm.ctl00_C5POBody_hdnFacilityRole.value;
                    if (FacilityRole != "") {
                        if (FacilityRole.split('&')[1] != "" && (FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN" || FacilityRole.split('&')[1].toUpperCase() == "PHYSICIAN ASSISTANT")) {
                            innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;font-weight:bolder;cursor:default' ;\" disabled>Comments</li>";
                            innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                        }
                    }
                    for (var i = 0; i < values.length ; i++) {

                        if (values[i].split('$')[0] == "Reason for Not Performed for Follow Up") {
                            innerdiv += "<li  style='text-decoration: none; list-style-type: none;font-weight:bolder;color:black;cursor:default'>" + values[i].split('$')[0] + "</li>";//BugID:45541
                            flag = 1;
                            reason = 1;
                            continue;
                        }
                        else if (values[i].split('$')[0] == "Follow Up") {
                            innerdiv += "<li  style='text-decoration: none; list-style-type: none;font-weight:bolder;color:black;cursor:default'>" + values[i].split('$')[0] + "</li>";//BugID:45541
                            flag = 1;
                            flagfollow = 1;
                            continue;;
                        }
                        if (flag == 0) {
                            innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"fun('" + values[i].split('$')[0].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i].split('$')[0] + "</li>";
                        }
                        else if (flag == 1) {
                            if (values[i].split('$')[0] == "Click to view more" && flagfollow == 1 && reason == 0)
                                innerdiv += "<li class='followuptest' style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default'  onclick=displayall('followup');\>" + values[i].split('$')[0] + "</li>";

                            else if (values[i].split('$')[0] == "Click to view more" && reason == 1)
                                innerdiv += "<li  class='reasontest' style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default'   onclick=displayall('reason');\>" + values[i].split('$')[0] + "</li>";
                            else {
                                if (values[i].split('$')[1] != "FREQUENTLY USED" && flagfollow == 1 && reason == 0)
                                    innerdiv += "<li class='followup' style='text-decoration: none;display:none; list-style-type: none;color:rgb(101,4,208);cursor:default' onclick=\"fun('" + values[i].split('$')[0].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i].split('$')[0] + "</li>";
                                else if (values[i].split('$')[1] != "FREQUENTLY USED" && reason == 1)
                                    innerdiv += "<li class='reason' style='text-decoration: none;display:none; list-style-type: none;color:rgb(101,4,208);cursor:default' onclick=\"fun('" + values[i].split('$')[0].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i].split('$')[0] + "</li>";
                                else
                                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(101,4,208);cursor:default' onclick=\"fun('" + values[i].split('$')[0].split("\r\n").join("\n").split("\n").join("~") + "," + targetControlValue + "');\">" + values[i].split('$')[0] + "</li>";
                            }

                        }
                    }

                    var listlength = innerdiv.length;
                    if (listlength > 0) {
                        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                                document.getElementsByTagName("div")[i].hidden = true;
                            }
                        }
                         //CAP-804 Syntax error, unrecognized expression
                        $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                          .css({
                              top: pos.top + $(".actcmpt").height() + 15,
                              left: pos.left,
                              width: pos.width,
                              height: '150px',
                              position: 'absolute',
                              background: 'white',
                              bottom: '0',
                              floating: 'top',
                              width: '362px',
                              border: '1px solid #8e8e8e',
                              background: '#FFF',
                              fontFamily: 'Segoe UI",Arial,sans-serif',
                              fontSize: '12px',
                              zIndex: '17',
                              overflowX: 'auto'

                          }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                    }
                    EnableSave();
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                },
                error: function OnError(xhr) {
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        //alert("USER MESSAGE:\n" +
                        //            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        //           "Message: " + log.Message);
                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                    }

                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });


        }
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

function fun(agrulist) {
    EnableSave();
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var sugglistval;
    var control;
    var value = agrulist.split(",");
    if (value.length > 2) {
        //CAP-804 Syntax error, unrecognized expression
        control = value[5]?.trim();
        //CAP-284 - Null handling for trim function
        sugglistval = ($("#" + control + ".actcmpt").val()??"").trim();
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

function checkboxClicked() {
    var chk = document.getElementById("chkCarePlan");
    if (chk.checked) {
        var cntrls = document.getElementById("divCarePlan").getElementsByTagName("td");;
        for (var i = 0; i < cntrls.length; i++) {
            if (cntrls[i].childNodes.length != 0) {
                if (cntrls[i].childNodes[0].id != undefined) {
                    if (cntrls[i].childNodes[0].id.startsWith("cbo") == true && cntrls[i].childNodes[0].id.indexOf("_Input") != -1) {
                        if (cntrls[i].childNodes[0].defaultValue != "Yes") {
                            var cboname = cntrls[i].childNodes[0].id.split("_")[0];
                            var combo = $find(cboname);
                            var item = combo.findItemByText("No");
                            if (item) {
                                item.select();
                            }
                        }
                    }
                }
            }
        }
    } else {
        var cntrls = document.getElementById("divCarePlan").getElementsByTagName("td");;
        for (var i = 0; i < cntrls.length; i++) {
            if (cntrls[i].childNodes.length != 0) {
                if (cntrls[i].childNodes[0].id != undefined) {
                    if (cntrls[i].childNodes[0].id.startsWith("cbo") == true && cntrls[i].childNodes[0].id.indexOf("_Input") != -1) {
                        if (cntrls[i].childNodes[0].defaultValue != "Yes") {
                            var cboname = cntrls[i].childNodes[0].id.split("_")[0];
                            var combo = $find(cboname);
                            var item = combo.findItemByText("");
                            if (item) {
                                item.select();
                            }
                        }
                    }
                }
            }
        }
    }
}

function ClearAll(sender, args) {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
    sender.set_autoPostBack(false);
    var IsClearAll = DisplayErrorMessage('600005');
    if (IsClearAll == true) {
        $find("btnSave").set_enabled(true);
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
        var ctrl = document.getElementsByTagName('INPUT');

        var ctrlCmb = document.getElementsByTagName('SELECT');

        for (var i = 0; i < ctrl.length; i++) {
            if (ctrl[i].id.split('_')[1] == 'txtDLC') {
                var ID = ctrl[i].id;
                $find(ID.replace("_ClientState", "")).clear();
                document.getElementById("chkCarePlan").checked = false;
                if (ID.replace("txt", "lst") != null) {
                    if (document.getElementById(ID.replace("txt", "lst")) != null) {
                        if (document.getElementById(ID.replace("txt", "lst")).style.visibility == "") {
                            $find(ID.replace("_ClientState", "").replace("txt", "lst")).get_element().style.display = "none";
                            document.getElementById(ID.replace("_ClientState", "").replace("txt", "img1")).src = "Resources/plus_new.gif";
                        }
                    }
                }
            } else if (ctrl[i].id.indexOf('txtctrl') >= 0) {
                var ID = ctrl[i].id;
                document.getElementById(ID.replace("_ClientState", "")).value = "";
            } else if (ctrl[i].id.indexOf('cbo') >= 0) {
                var ID = ctrl[i].id;
                if (ctrl[i].id.split('_')[1] == "ClientState" || ctrl[i].id.split('_')[2] == "ClientState") {
                    document.getElementById(ID.replace("_ClientState", "")).selectedIndex = 0;
                } else if (ctrl[i].id.split('_')[1] == "Input" || ctrl[i].id.split('_')[2] == "Input") {
                    document.getElementById(ID.replace("_Input", "")).selectedIndex = 0;
                }
            } else if (ctrl[i].id.startsWith("dtp") == true) {
                var ID = ctrl[i].id;
                if (ctrl[i].id.split('_')[1] == "ClientState") {
                    $find(ID.replace("_ClientState", "")).clear();
                }
            }
        }
        var txtDLC = document.getElementsByTagName('textarea');
        for (var i = 0; i < txtDLC.length; i++) {
            txtDLC[i].textContent = "";
            txtDLC[i].value = "";
        }
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    } else {
        $find("btnSave").set_enabled(true);
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
    }
    for (var i = 0; i < ctrlCmb.length; i++) {
        if (ctrlCmb[i].id.indexOf('cbo') >= 0) {
            var ID = ctrlCmb[i].id;
            document.getElementById(ID).selectedIndex = 0;
        }
    }

}

function Enable_OR_Disable() {
    if ($find("btnSave").get_enabled(true) == true)
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";
}

function CCTextChanged() {
    $find("btnSave").set_enabled(true);
    document.getElementById("Hidden1").value = "True";
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;

    return false;
}

function RadComboSelectedIndexChanged() {
    EnableSave();
}

function ShowLoading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function CellSelected(value) {
    if (DisplayErrorMessage('600015') == false) {

    } else {
        document.getElementById('hdnValue').value = value;
        __doPostBack('btnSave');
    }
}

function DateSaveEnable() {
    $find("btnSave").set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}


function DateSaveEnable() {
    EnableSave();
}

function cancelBack() {
    if (event.keyCode != 9 && event.keyCode != 18) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
    }
}

function btnCopyPreviousEncounter_Clicked(sender, args) {
    document.getElementById("HdnCopyButton").value = "";
    var btnSave = $find('btnSave');
    if (btnSave._enabled) {
        checkSavebutton();
        sender.set_autoPostBack(false);
    }
    else {
        sender.set_autoPostBack(true);
    }
}

function btnSaveClicked() {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
}

function CallMe(sender, args) {
    var inputText = sender._validationText;
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;

    if (inputText.match(FormatDDMMMYYYY)) {
        var DateMonthYear = inputText.split('-');
        if (DateMonthYear[0].length < 4) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }

        if (DateMonthYear[1].length < 3) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }
        if (DateMonthYear[2].length < 2) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
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
                    // $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
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
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }
                if ((lyear == true) && (DateInput > 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            if (Year > CurrentYear) {
                alert("Cannot be future date. Please Enter a Valid Date.");
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }

        } else {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        }
    } else {

        if (inputText.split('-')[0].length == 0 && (inputText.split('-')[1].length != 0 || inputText.split('-')[0].length != 0)) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[2].length == 1) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[1].length == 0 && inputText.split('-')[0].length == 0) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[2].length != 0 && (inputText.split('-')[1].length == 0 || inputText.split('-')[0].length == 0)) {
            alert('Invalid date format!');
            $find(GetClientId(sender._clientID)).focus(true);
            return false;
        } else if (inputText.split('-')[1].length != 0 && inputText.split('-')[0].length != 0) {
            var DateMonthYear = inputText.split('-');
            if (DateMonthYear[0].length < 4) {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }

            if (DateMonthYear[1].length < 3) {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).focus(true);
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
                if (Year > CurrentYear) {
                    alert("Cannot be future date. Please Enter a Valid Date.");
                    $find(GetClientId(sender._clientID)).clear();
                    $find(GetClientId(sender._clientID)).focus(true);
                    return false;
                }

            } else {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }
        } else if (inputText.split('-')[0].length != 0) {
            var DateMonthYear = inputText.split('-');
            if (DateMonthYear[0].length < 4) {
                alert('Invalid date format!');
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }

            var DateMonthYear = inputText.split('-');
            var Year = parseInt(DateMonthYear[0]);
            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            if (Year > CurrentYear) {
                alert("Cannot be future date. Please Enter a Valid Date.");
                $find(GetClientId(sender._clientID)).clear();
                $find(GetClientId(sender._clientID)).focus(true);
                return false;
            }
        }
    }
}


function MaskDateEnable(ctrl, Value, ctrlName) {
    var pcontrol = ctrl;
    var name = ctrlName;
    var chkValue = Value;

    var DateControl = pcontrol.id.replace(chkValue, name);
    var txtMaskDate = $find(DateControl);
    if (txtMaskDate != null) {
        txtMaskDate.enable();
        txtMaskDate.clear();
    }
}

function MaskDateDisable(ctrl, Value, ctrlName) {
    var pcontrol = ctrl;
    var name = ctrlName;
    var chkValue = Value;
    var DateControl = pcontrol.id.replace(chkValue, name);
    var txtMaskDate = $find(DateControl);
    if (txtMaskDate != null) {
        txtMaskDate.disable();
        txtMaskDate.clear();
    }
}

function RefreshFloatingSummary() {
    var dox = window.parent.window.parent.window.document;
    var iFrame = dox.getElementsByTagName("iframe");
    if (iFrame.length > 0) {
        var str = iFrame[0].src;
        var n = str.indexOf("frmFollowUpEncounter.aspx");
        if (n >= 0) {
            iFrame[0].src = iFrame[0].src;
        } else {

        }
    }
}

function savedSuccessfully() {
    localStorage.setItem("bSave", "true");
    var splitvalues = window.parent.parent.theForm.hdnTabClick.value.split('$#$');
    var which_tab = splitvalues[0];
    var screen_name;
    if (which_tab.indexOf('btn') > -1) {
        screen_name = 'MoveToButtonsClick';
    }
    else if (which_tab == 'first') {
        screen_name = '';
    }
    else if (which_tab != "first" && which_tab != "CC / HPI" && which_tab != "QUESTIONNAIRE" && which_tab != "PFSH" && which_tab != "ROS" && which_tab != "VITALS" && which_tab != "EXAM" && which_tab != "TEST" && which_tab != "ASSESSMENT" && which_tab != "ORDERS" && which_tab != "eRx" && which_tab != "SERV./PROC. CODES" && which_tab != "PLAN" && which_tab != "SUMMARY")
        screen_name = "PlanTabClick";
    else
        screen_name = "EncounterTabClick";
    if (splitvalues.length == 3 && splitvalues[2] == "Node")
        screen_name = 'PatientChartTreeViewNodeClick';

    DisplayErrorMessage('600014');

    top.window.document.getElementById('ctl00_Loading').style.display = 'none';


    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
}

function chkPlanCheckChange() {
    var bcheck = true;
    $find("btnSave").set_enabled(true);
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    var ctrl = document.getElementsByTagName('*');
    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].id.substring(0, 3) == 'cbo') {
            var name = ctrl[i].name;
            var ID = document.getElementsByName(name);

            for (var j = 0; j < ID.length; j++) {
                var value = ID[j].value.split(' ')[1];

                if (document.getElementById('chkCarePlan').checked == true) {
                    if (ctrl[i].value == "No") {

                    } else if (ctrl[i].value == "") {
                        ctrl[i].selected = true;
                        ctrl[i].value = "No";
                    } else if (ctrl[i].value == "Yes") { }
                } else {
                    if (ctrl[i].value == "No") {
                        ctrl[i].selected = false;
                        ctrl[i].value = "";
                    }
                }
            }
        }
    }
}



function CarePlan_Load() {
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,rdmpPlanTab";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
}
function Loading() {

    document.getElementById('divLoading').style.display = "none";
}



function checkSavebutton() {

    var button2 = $find('btnSave');
    var btnCopyCheck = $find('btnCopyPreviousEncounter');
    var button1 = document.getElementById('Button2');
    document.getElementById("HdnCopyButton").value = "";
    document.getElementById("HdnCopyButton").value = "trueValidate";
    document.getElementById("hdnTrueCheck").value = "true";
    testCopy = "true";
    button2.click();
    //var obj = new Array();
    //obj.push("Title=" + "Message");
    //obj.push("ErrorMessages=" + "There are unsaved changes.Do you want to save them?");
    //var result = openModal("frmValidationArea.aspx", 100, 300, obj, "MessageWindow");
    //var WindowName = $find('MessageWindow');
    //WindowName.add_close(OnClientCloseValidation);
}


function OnClientCloseValidation(oWindow, args) {
    var button2 = $find('btnSave');
    var btnCopyCheck = $find('btnCopyPreviousEncounter');
    var button1 = document.getElementById('Button2');
    document.getElementById("HdnCopyButton").value = "";
    var arg = args.get_argument();
    if (arg) {
        var result = arg;
        if (result == "Yes") {
            document.getElementById("HdnCopyButton").value = "trueValidate";
            document.getElementById("hdnTrueCheck").value = "true";
            testCopy = "true";
            button2.click();

        } else if (result == "Cancel") {
            document.getElementById("HdnCopyButton").value = "CheckSave";
        }
        else if (result == "No") {
            button2.set_enabled(false);
            btnCopyCheck.click();
        }
    }

}
document.onkeyup = KeyCheck;    //added for Backspace and Delete key
function KeyCheck(e) {
    var KeyID = (window.event) ? event.keyCode : e.keyCode;
    if (KeyID == 8 || KeyID == 46) {
        EnableSave();
    }
}


/* code for html page*/

function CarePlan_Load() {
    $("#divCarePlan").load("CarePlan.html", LoadCarePlan);
}
var DOB;
function LoadCarePlan() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }


    $('#btnSave')[0].disabled = false;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    var lbl;
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/LoadeCarePlan",
        data: '',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {

            var objdata = $.parseJSON(data.d);
            DOB = objdata.DOB;
            var currentprocess = objdata.currentProcess;
            FillCarePlanTable(objdata.objCarePlanList, objdata.MasterID);
            var len = $("#mainContainer").find("textarea").length;
            if (currentprocess.toUpperCase() != "SCRIBE_PROCESS" && currentprocess.toUpperCase() != "AKIDO_SCRIBE_PROCESS" && currentprocess.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "SCRIBE_CORRECTION" && currentprocess.toUpperCase() != "DICTATION_REVIEW" && currentprocess.toUpperCase() != "CODER_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "PROVIDER_PROCESS" && currentprocess.toUpperCase() != "MA_REVIEW" && currentprocess.toUpperCase() != "MA_PROCESS" && currentprocess.toUpperCase() != "PROVIDER_REVIEW_CORRECTION") {
                $('#btnSave')[0].disabled = true;
                $('#btnClearAll')[0].disabled = true;
                if ($('#btnCopyPrevious')[0]?.disabled != undefined) {
                    $('#btnCopyPrevious')[0].disabled = true;
                }
                $('#mainContainer')[0].disabled = true;
                $('#mainContainer ').find(':input').prop('disabled', true);
                $('img').css("display", "none");
                $('select').css('backgroundColor', '#ebebe4');
                $("a").attr('disabled', true);
                $("a").attr('onclick', false);
                $("a").css('backgroundColor', '#6D7777');
            }

            $("[id*=pbDropdown]").addClass('pbDropdownBackground');
            $("textarea").addClass('spanstyle');
            $("label").addClass('spanstyle');
            $("select").addClass('spanstyle');

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
        }
    });
    $("input.DateInput").mask("9999?-aaa-99");
}

function SaveCarePlan() {

    var i = 0;
    $("input.DateInput").each(function () {



        localStorage.setItem("bSave", "false");

        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
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
                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }

                if (DateMonthYear[1].length < 3) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }
                if (DateMonthYear[2].length < 2) {
                    alert('Invalid date format!');
                    this.focus();
                    $(this)[0].value = "____-___-__";
                    i = 1;
                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
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
                            //CAP-1399
                            SaveUnsuccessful();
                            AutoSaveUnsuccessful();
                            localStorage.setItem("bSave", "false");
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
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
                            //CAP-1399
                            SaveUnsuccessful();
                            AutoSaveUnsuccessful();
                            localStorage.setItem("bSave", "false");
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        }
                        if ((lyear == true) && (DateInput > 29)) {
                            alert('Invalid date format!');
                            this.focus();
                            i = 1;
                            //CAP-1399
                            SaveUnsuccessful();
                            AutoSaveUnsuccessful();
                            localStorage.setItem("bSave", "false");
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
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
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                    else if (parseInt(Month) > parseInt(curmonth) && Year >= CurrentYear) {
                        alert("Cannot be future date. Please Enter a Valid Date.");
                        this.focus();
                        i = 1;
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                    else if (parseInt(DateMonthYear[2]) > parseInt(curday) && Year >= CurrentYear && parseInt(Month) >= parseInt(curmonth)) {
                        alert("Cannot be future date. Please Enter a Valid Date.");
                        this.focus();
                        i = 1;
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }

                } else {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;
                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }
            } else {

                if (inputText.split('-')[0].length == 0 && (inputText.split('-')[1].length != 0 || inputText.split('-')[0].length != 0)) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;

                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                } else if (inputText.split('-')[2].length == 1) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;

                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                } else if (inputText.split('-')[1].length == 0 && inputText.split('-')[0].length == 0) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;

                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                } else if (inputText.split('-')[2].length != 0 && (inputText.split('-')[1].length == 0 || inputText.split('-')[0].length == 0)) {
                    alert('Invalid date format!');
                    this.focus();
                    i = 1;

                    //CAP-1399
                    SaveUnsuccessful();
                    AutoSaveUnsuccessful();
                    localStorage.setItem("bSave", "false");
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                } else if (inputText.split('-')[1] != '___' && inputText.split('-')[1].length != 0 && inputText.split('-')[0].length != 0) {
                    var DateMonthYear = inputText.split('-');
                    if (DateMonthYear[0].length < 4) {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;

                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }

                    if (DateMonthYear[1].length < 3) {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
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
                            //CAP-1399
                            SaveUnsuccessful();
                            AutoSaveUnsuccessful();
                            localStorage.setItem("bSave", "false");
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        }
                        else if (parseInt(Month) > parseInt(curmonth) && Year >= CurrentYear) {

                            alert("Cannot be future date. Please Enter a Valid Date.");
                            this.focus();
                            i = 1;
                            //CAP-1399
                            SaveUnsuccessful();
                            AutoSaveUnsuccessful();
                            localStorage.setItem("bSave", "false");
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        }
                        else if (parseInt(DateMonthYear[2]) > parseInt(curday) && Year >= CurrentYear && parseInt(Month) >= parseInt(curmonth)) {
                            alert("Cannot be future date. Please Enter a Valid Date.");
                            this.focus();
                            i = 1;
                            //CAP-1399
                            SaveUnsuccessful();
                            AutoSaveUnsuccessful();
                            localStorage.setItem("bSave", "false");
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        }

                    }
                    else {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                } else if (inputText.split('-')[0].length != 0) {
                    var DateMonthYear = inputText.split('-');
                    if (DateMonthYear[0].length < 4) {
                        alert('Invalid date format!');
                        this.focus();
                        i = 1;
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
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
                        //CAP-1399
                        SaveUnsuccessful();
                        AutoSaveUnsuccessful();
                        localStorage.setItem("bSave", "false");
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                }
            }
        }


    });
    if (i != 1) {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var arry = new Array();
        var lbl;
        var bCheckDOB = Date.parse(DOB);
        var now = new Date();
        var d = now.getUTCDate();
        var m = (now.getUTCMonth() + 1);
        var y = now.getUTCFullYear();
        var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
        var date = d + "-" + arr[m] + "-" + y;
        var currentdate = Date.parse(date);

        var CarePlanList = function () {
            this.Care_Name = "";
            this.Care_Name_Value = "";
            this.Status = "";
            this.Care_Plan_Notes = "";
            this.Care_Plan_Lookup_ID = 0;
            this.Plan_Date = "";
            this.Status_Value = "";
            this.version = "";
            this.id = "";
        };

        var arry = new Array();
        var objCarePlanList = new CarePlanList();
        var rows = $("#mainContainer label");
        for (var pos = 0; pos < rows.length; pos++) {
            var objCarePlanList = new CarePlanList();
            var lbl = rows[pos];
            if (lbl.className.indexOf("lblversion") < 0 && $(lbl).parents("tr")[0].style.display == "block" && ($(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value.trim() == "") &&
                (lbl.parentNode.nextElementSibling.nextElementSibling.children[0].value == "") &&
                 (lbl.parentNode.nextElementSibling.children[0].value == "")
                ) {

            }
            else {
                if (lbl.className.indexOf("lblversion") < 0 && $(lbl).parents("tr")[0].style.display == "block") {
                    objCarePlanList.Care_Plan_Lookup_ID = lbl.attributes[0].value;
                    objCarePlanList.Care_Name = $(lbl)[0].closest('div').parentNode.children[0].innerText.trim();
                    objCarePlanList.Care_Name_Value = lbl.innerText.trim();
                    if (lbl.parentNode.nextElementSibling.nextElementSibling.children.length != 0) {
                        objCarePlanList.Plan_Date = lbl.parentNode.nextElementSibling.nextElementSibling.children[0].value;
                        if (objCarePlanList.Plan_Date != "") {
                            var plan_date = Date.parse(objCarePlanList.Plan_Date);
                            if (plan_date >= bCheckDOB) {
                                if (Date.parse(objCarePlanList.Plan_Date) > currentdate) {
                                    DisplayErrorMessage('600013');
                                    SaveUnsuccessful();
                                    AutoSaveUnsuccessful();
                                    localStorage.setItem("bSave", "false");
                                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                    return;
                                }
                            }
                            else {
                                DisplayErrorMessage('600010');
                                SaveUnsuccessful();
                                AutoSaveUnsuccessful();
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                return;
                            }
                        }
                        objCarePlanList.Care_Plan_Notes = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value.trim();

                        if (lbl.parentNode.nextElementSibling.children[0].className == "combo SelectStyle spanstyle") {
                            objCarePlanList.Status = lbl.parentNode.nextElementSibling.children[0].value.trim();
                        }
                        else if (lbl.parentNode.nextElementSibling.children[0].className == "stylTxtCtrl nonEditabletxtbox") {
                            if (lbl.innerText == "BMI") {
                                objCarePlanList.Status_Value = lbl.parentNode.nextElementSibling.children[0].value.trim();
                                if ($(lbl).parents("tr")[0].nextElementSibling != null) {
                                    if ($(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.style.display == "block") {
                                        objCarePlanList.Status = $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.value.trim();
                                    }
                                }
                            }
                            else if (lbl.innerText.toUpperCase() == "BP SYS/DIA") {
                                objCarePlanList.Status_Value = lbl.parentNode.nextElementSibling.children[0].value;

                            }
                            else if (lbl.innerText.toUpperCase().indexOf("TOBACCO") > -1) {
                                objCarePlanList.Status = lbl.parentNode.nextElementSibling.children[0].value.trim();

                            }
                            else {
                                objCarePlanList.Status_Value = lbl.parentNode.nextElementSibling.children[0].value.trim();
                            }
                        }
                        else if (lbl.parentNode.nextElementSibling.children[0].className == "stylTxtCtrl Editabletxtbox") //Git Id 1593 
                        {
                            objCarePlanList.Status_Value = lbl.parentNode.nextElementSibling.children[0].value.trim();
                        }
                        if (lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.trim() != "") {
                            objCarePlanList.version = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split("-")[0].trim();
                            objCarePlanList.id = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split("-")[1].trim();
                        }
                        else {
                            objCarePlanList.version = "0";
                            objCarePlanList.id = "0";
                        }

                    }
                    else if (lbl.parentNode.nextElementSibling.children.length != 0) {


                        objCarePlanList.Care_Plan_Notes = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value.trim();


                        if (lbl.parentNode.nextElementSibling.children[0].className == "combo SelectStyle spanstyle") {
                            objCarePlanList.Status = lbl.parentNode.nextElementSibling.children[0].value.trim();
                        }
                        else if (lbl.parentNode.nextElementSibling.children[0].className == "stylTxtCtrl nonEditabletxtbox") {
                            if (lbl.innerText == "BMI") {
                                objCarePlanList.Status_Value = lbl.parentNode.nextElementSibling.children[0].value.trim();
                                if ($(lbl).parents("tr")[0].nextElementSibling != null) {
                                    if ($(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.style.display == "block") {
                                        objCarePlanList.Status = $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.value.trim();
                                    }
                                }
                            }
                            else {
                                objCarePlanList.Status = lbl.parentNode.nextElementSibling.children[0].value.trim();
                            }
                        }
                        else if (lbl.parentNode.nextElementSibling.children[0].className == "stylTxtCtrl Editabletxtbox") //Git Id 1593 
                        {
                            objCarePlanList.Status_Value = lbl.parentNode.nextElementSibling.children[0].value.trim();
                        }
                        if (lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.trim() != "") {
                            objCarePlanList.version = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split("-")[0].trim();
                            objCarePlanList.id = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split("-")[1].trim();
                        }
                        else {
                            objCarePlanList.version = "0";
                            objCarePlanList.id = "0";
                        }

                    }
                    else {
                        objCarePlanList.Care_Plan_Notes = $(lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value.trim();
                        if (lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.trim() != "") {
                            objCarePlanList.version = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split("-")[0].trim();
                            objCarePlanList.id = lbl.parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent.split("-")[1].trim();
                        }
                        else {
                            objCarePlanList.version = "0";
                            objCarePlanList.id = "0";
                        }
                    }
                    arry[arry.length++] = objCarePlanList;
                }
            }
        }
        localStorage.setItem("bSave", "true");
        $.ajax({
            type: "POST",
            url: "WebServices/PlanService.asmx/SaveCarePlan",
            data: JSON.stringify({
                "data": arry,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                var dataList = $.parseJSON(data.d);
                AutoSave();
                AutoSaveSuccessful();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                RefreshNotification('Careplan');
            },
            error: function OnError(xhr) {
                AutoSaveUnsuccessful();
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }

}


var sCareName = "";
var rCareName = "";
var objCareName = ["Vaccination", "Screening Tests", "Patient Details", "Preventive Care", "Social", "Nutrition", "Functional Assessment", "Psychological Assessment", "Cognitive Functioning"];
function FillCarePlanTable(objdata, MasterID) {
    $("#mainContainer").find("tr").css("display", "none");
    if (objdata.length != 0) {
        for (var i = 0; i < objdata.length; i++) {

            sCareName = objdata[i].Care_Name;
            if (rCareName != sCareName) {
                if (objCareName.indexOf(sCareName) > -1) {
                    for (var p = objCareName.length - 1; p >= 0; p--) {

                        if (objCareName[p] == sCareName) {
                            rCareName = objCareName[p];
                            objCareName.splice(p, 1);

                        }

                    }
                }
            }

            var lbl = $("#mainContainer").find("label[LookUpID ='" + objdata[i].Care_Plan_Lookup_ID + "']");
            if (lbl != undefined && lbl[0] != undefined) {
                if (lbl[0].attributes[0].value == objdata[i].Care_Plan_Lookup_ID) {
                    $(lbl).parents("tr")[0].style.display = "block";
                    if ($(lbl).parents("tr").attr("gender") != undefined) {
                        if ($(lbl).parents("tr").attr("gender").toUpperCase() != objdata[i].Gender) {
                            $(lbl).parents("tr")[0].style.display = "block";
                        }
                        else {
                            $(lbl).parents("tr")[0].style.display = "none";
                        }
                    }

                }

                if ($(lbl).parents("tr")[0].style.display != "none") {
                    //CAP-1396
                    if (lbl[0].parentNode.nextElementSibling.children[0].className == "combo SelectStyle" || lbl[0].parentNode.nextElementSibling.children[0].className == "combo SelectStyle spanstyle") {
                        lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                        if (objdata[i].Care_Name_Value.toUpperCase().indexOf("SEXUAL ORIENTATION") > -1) {
                            if (objdata[i].Status.indexOf("please describe") <= -1)
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].disabled = "true"
                        }
                        if (objdata[i].Care_Name_Value.toUpperCase().indexOf("GENDER IDENTITY") > -1) {
                            if (objdata[i].Status.indexOf("please specify") <= -1)
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].disabled = "true"
                        }

                        //Jira CAP-447
                        //else if ((objdata[i].Care_Name_Value.toUpperCase().indexOf("BREAST") > -1)) {
                        //    lbl[0].parentNode.nextElementSibling.children[0].disabled = "true";
                        //    lbl[0].parentNode.nextElementSibling.children[0].style.backgroundColor = "rgba(191, 219, 255,1)";
                        //    $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                        //}
                        if (objdata[i].Status == "") {
                            lbl[0].parentNode.nextElementSibling.children[0].selectedIndex = 0;
                        }
                        else if (objdata[i].Status == "Yes") {
                            lbl[0].parentNode.nextElementSibling.children[0].selectedIndex = 1;
                        }
                        else if (objdata[i].Status == "No") {
                            lbl[0].parentNode.nextElementSibling.children[0].selectedIndex = 2;
                        }

                    } else if (lbl[0].parentNode.nextElementSibling.children[0].className.indexOf("stylTxtCtrl") >= 0) {

                        if (objdata[i].Care_Name_Value == "BMI") {
                            if (objdata[i].Vitals_BMI != undefined && objdata[i].Vitals_BMI != "") {
                                lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Vitals_BMI;
                            }
                            else if (objdata[i].Status_Value != "") {
                                lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                            }
                            if (objdata[i].Vitals_BMI_Status_Value != undefined && objdata[i].Vitals_BMI_Status_Value != "") {
                                if ($(lbl).parents("tr")[0].nextElementSibling != null) {
                                    $(lbl).parents("tr")[0].nextElementSibling.style.display = "block";
                                    $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.style.display = "block";
                                    $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.value = objdata[i].Vitals_BMI_Status_Value;
                                }
                            }

                            lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                        }
                        else if (objdata[i].Care_Name_Value.toUpperCase() == "HEMOGLOBIN") { //BugID:51648
                            lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                            if (lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0] != undefined)
                                lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                        }

                        else if (objdata[i].Care_Name_Value.toUpperCase().indexOf("TOBACCO") > -1) {
                            if (objdata[i].Status != undefined && objdata[i].Status != "") {
                                if (objdata[i].Status.split('|')[0] != "") {
                                    lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status.split('|')[0];
                                    $(lbl[0].parentNode.nextElementSibling.children[0]).attr('title', objdata[i].Status.split('|')[0]);
                                    if (objdata[i].Status.split('|')[1] != undefined && objdata[i].Status.split('|')[1] != null)
                                        localStorage.setItem('Tobacco', objdata[i].Status.split('|')[1]);
                                }


                            }



                            lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                        }

                        else if (objdata[i].Care_Name_Value.toUpperCase() == "BP SYS/DIA") {
                            if (objdata[i].Vitals_BP != undefined && objdata[i].Vitals_BP != "") {
                                lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Vitals_BP;
                            }
                            else if (objdata[i].Status_Value != "") {
                                lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                            }
                            if (objdata[i].Vitals_BP_Status_Value != undefined && objdata[i].Vitals_BP_Status_Value != "") {
                                if ($(lbl).parents("tr")[0].nextElementSibling != null) {
                                    $(lbl).parents("tr")[0].nextElementSibling.style.display = "block";
                                    $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.style.display = "block";
                                    $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.value = objdata[i].Vitals_BP_Status_Value;
                                }
                            }

                            lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');

                        }
                        else {
                            lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                        }
                    }
                    if (lbl[0].parentNode.nextElementSibling.nextElementSibling.children.length != 0) {
                        lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].value = objdata[i].Plan_Date.split(" ")[0];
                        $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Care_Plan_Notes;

                        if (objdata[i].Id == undefined) {
                            lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + "0";
                        }
                        else {
                            lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + objdata[i].Id;
                        }

                    }
                    else {
                        $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Care_Plan_Notes;
                        if (objdata[i].Id == undefined) {
                            lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + "0";
                        }
                        else {
                            lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + objdata[i].Id;
                        }
                    }
                    //BugID:47880
                    if (objdata[i].Care_Name_Value.toUpperCase().indexOf("TOBACCO") > -1 && objdata[i].Status != undefined && objdata[i].Status != "" && objdata[i].Status.split('|').length == 3) {
                        if (objdata[i].Care_Plan_Notes.trim() == "") {
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Status.split('|')[2];
                            EnableSave();
                        }

                    }
                    //BugID:47887
                    if (objdata[i].Care_Name_Value.toUpperCase() == "BP SYS/DIA" && objdata[i].Status != undefined && objdata[i].Status.trim() != "") {
                        if (objdata[i].Care_Plan_Notes.trim() == "" && objdata[i].Status.indexOf("|") != -1 && objdata[i].Status.split("|").length == 2) {
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Status.split('|')[1];
                            EnableSave();
                        }
                    }

                     //Jira CAP-339
                    if (MasterID[objdata[i].Care_Plan_Lookup_ID] != 0) {
                        if (lbl[0].parentNode.nextElementSibling.children[0] != null && lbl[0].parentNode.nextElementSibling.children[0] != undefined && lbl[0].parentNode.nextElementSibling.children[0].disabled == false) {
                            lbl[0].parentNode.nextElementSibling.children[0].disabled = "true";
                            lbl[0].parentNode.nextElementSibling.children[0].style.backgroundColor = "rgba(191, 219, 255,1)";
                        }
                        if (lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled != null && lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled != undefined)
                            lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                        if ($(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]) != null && $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]) != undefined)
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                        
                    }

                }

            }
        }
    }

    for (var m = 0; m < objCareName.length; m++) {
        $("[id*='" + objCareName[m] + "']").css("display", "none");
    }


}

function AutoSave() {
    var which_tab;
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.theForm.hdnTabClick != undefined && window.parent.parent.theForm.hdnTabClick != null) {
        var splitvalues = window.parent.parent.theForm.hdnTabClick.value.split('$#$');
        which_tab = splitvalues[0];

        var screen_name;
        if (which_tab.indexOf('btn') > -1) {
            screen_name = 'MoveToButtonsClick';
        }
        else if (which_tab == 'first') {
            screen_name = '';
        }
        else if (which_tab != "first" && which_tab != "CC / HPI" && which_tab != "QUESTIONNAIRE" && which_tab != "PFSH" && which_tab != "ROS" && which_tab != "VITALS" && which_tab != "EXAM" && which_tab != "TEST" && which_tab != "ASSESSMENT" && which_tab != "ORDERS" && which_tab != "eRx" && which_tab != "SERV./PROC. CODES" && which_tab != "PLAN" && which_tab != "SUMMARY")
            screen_name = "PlanTabClick";
        else
            screen_name = "EncounterTabClick";
        if (splitvalues.length == 3 && splitvalues[2] == "Node")
            screen_name = 'PatientChartTreeViewNodeClick';

    }
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    DisplayErrorMessage('600001');

    $('#btnSave')[0].disabled = true;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
}

function CopyPreviousCarePlan() {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
        event.preventDefault();
        event.stopPropagation();
        disableAutoSave();
        SaveCarePlan();
        CarePlanCopyPrevious();
        return;
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogCarePlan');

        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            disableAutoSave();
        //            SaveCarePlan();
        //            CarePlanCopyPrevious();
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            disableAutoSave();
        //            CarePlanCopyPrevious();
        //            $(dvdialog).dialog("close");

        //        },
        //        "Cancel": function () {
        //            $(dvdialog).dialog("close");
        //            return;
        //        }
        //    }
        //});
    }
    else {
        CarePlanCopyPrevious();
    }
}

function enableAutoSave() {
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    $('#btnSave')[0].disabled = false;
}
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnSave')[0].disabled = true;
}

function CarePlanCopyPrevious() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var lbl;
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/CopyPreviousCarePlan",
        data: '',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var objdata = $.parseJSON(data.d);

            if (objdata.objCarePlanList.length == 0) {
                onCopyPrevious('210010');
                return;
            }
            else if (objdata.CareplanListcount == "0") {
                onCopyPrevious('170014');

            }
            else if (objdata.objCarePlanList.length == 1 && objdata.objCarePlanList[0].Physician_Process == false) {
                onCopyPrevious('210016');
            }

            else {
                FillCarePlanTableForCopyPrevious(objdata.objCarePlanList, objdata.MasterID, objdata.ChangedMasterId);
                onCopyPrevious('');
                return;
            }
        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}




function Filter(array, terms) {

    arrayOfTerms = terms.split(",");
    var first_resultant = array;
    var resultant;
    resultant = $.grep(first_resultant, function (item) {
        return item.toLowerCase().indexOf(arrayOfTerms[arrayOfTerms.length - 1].trim().toLowerCase()) > -1;

    });
    first_resultant = resultant;
    return first_resultant;
}
//function chkSystem_CheckedChanged() {

//    $('#btnSave')[0].disabled = false;
//    var lfckv = document.getElementById("chkSystem").checked;
//    if (lfckv) {

//        var combobox = $('select.combo');
//        var classname = "";
//        for (var i = 0; i < combobox.length; i++) {
//            if ($(combobox[i]).find('option:selected').val() != 'Yes')
//                $($(combobox[i]).find('option')[2]).prop('selected', true);

//        }
//    }
//    else {
//        var combobox = $('select.combo');
//        for (var i = 0; i < combobox.length; i++) {
//            if ($(combobox[i]).find('option:selected').val() == 'No')
//                $($(combobox[i]).find('option')[0]).prop('selected', true);
//        }
//    }
//}

$("select").change(function () {

    var optionSelected = $("option:selected", this);
    $('#btnSave')[0].disabled = false;

    if (this.id.toUpperCase().indexOf("SEXUAL ORIENTATION") > -1) {
        if (optionSelected[0].innerText.indexOf("please describe") > -1)
            $(this)[0].parentNode.parentNode.children[3].childNodes[1].disabled = false;
        else {
            $(this)[0].parentNode.parentNode.children[3].childNodes[1].disabled = true;
            $(this)[0].parentNode.parentNode.children[3].childNodes[1].value = "";
        }
    }

    if (this.id.toUpperCase().indexOf("GENDER IDENTITY") > -1) {
        if (optionSelected[0].innerText.indexOf("please specify") > -1)
            $(this)[0].parentNode.parentNode.children[3].childNodes[1].disabled = false;
        else {
            $(this)[0].parentNode.parentNode.children[3].childNodes[1].disabled = true;
            $(this)[0].parentNode.parentNode.children[3].childNodes[1].value = "";
        }
    }



    EnableSave();

});

function Clear() {


    if (DisplayErrorMessage('270002') == true) {

        LoadCarePlan();
        EnableSave();
    }

}

$(document).ready(function () {
    $("input.DateInput").datepicker({
        dateFormat: 'yy-M-dd', changeYear: true, changeMonth: true, showOn: "button",
        buttonImage: "Resources/calendar_icon.png",
        buttonImageOnly: false,
        buttonText: "Select date",
        yearRange: "-100:+0"
    });

    $("input.DateInput").bind("keypress", function (e) {

        EnableSave();
    });
    $("textarea").bind("keypress", function (e) {

        EnableSave();
    });
    $("textarea").bind("keydown", function (e) {//BugID:45541

        insertTab(this, event);
    });
    var specialKeys = new Array();
    specialKeys.push(8);
    $(".stylTxtCtrl").bind("keypress", function (e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 40 && keyCode <= 65 || keyCode >= 123) || specialKeys.indexOf(keyCode) != -1);
        if (ret) {
            $('#btnSave')[0].disabled = false;
            EnableSave();
        }
        return ret;
    });
    $(".stylTxtCtrl").bind("paste", function (e) {
        return false;
    });
    $(".stylTxtCtrl").bind("drop", function (e) {
        return false;
    });
});
function OpenPopup(keyword) {
    var focused = keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}

function onCopyPrevious(errorCode) {

    if (errorCode == "") {
        enableAutoSave();
    }
    else {
        DisplayErrorMessage(errorCode);
        disableAutoSave();
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

//Jira CAP-1329
$(".hasDatepicker").change(function () {
    
    EnableSave();

});

function FillCarePlanTableForCopyPrevious(objdata, MasterID, ChangedMasterId)
{
    $("#mainContainer").find("tr").css("display", "none");
        if (objdata.length != 0) {
            for (var i = 0; i < objdata.length; i++) {
                var isChanged = ChangedMasterId.includes(objdata[i]?.Care_Plan_Lookup_ID);
                sCareName = objdata[i].Care_Name;
                if (rCareName != sCareName) {
                    if (objCareName.indexOf(sCareName) > -1) {
                        for (var p = objCareName.length - 1; p >= 0; p--) {

                            if (objCareName[p] == sCareName) {
                                rCareName = objCareName[p];
                                objCareName.splice(p, 1);

                            }

                        }
                    }
                }

                var lbl = $("#mainContainer").find("label[LookUpID ='" + objdata[i].Care_Plan_Lookup_ID + "']");
                if (lbl != undefined && lbl[0] != undefined) {
                    if (lbl[0].attributes[0].value == objdata[i].Care_Plan_Lookup_ID) {
                        $(lbl).parents("tr")[0].style.display = "block";
                        if ($(lbl).parents("tr").attr("gender") != undefined) {
                            if ($(lbl).parents("tr").attr("gender").toUpperCase() != objdata[i].Gender) {
                                $(lbl).parents("tr")[0].style.display = "block";
                            }
                            else {
                                $(lbl).parents("tr")[0].style.display = "none";
                            }
                        }

                    }

                    if ($(lbl).parents("tr")[0].style.display != "none") {
                        //CAP-1396
                        if (lbl[0].parentNode.nextElementSibling.children[0].className == "combo SelectStyle" || lbl[0].parentNode.nextElementSibling.children[0].className == "combo SelectStyle spanstyle") {
                            if (isChanged) {
                                lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status;
                            }
                            if (objdata[i].Care_Name_Value.toUpperCase().indexOf("SEXUAL ORIENTATION") > -1) {
                                if (objdata[i].Status.indexOf("please describe") <= -1)
                                    $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].disabled = "true"
                            }
                            if (objdata[i].Care_Name_Value.toUpperCase().indexOf("GENDER IDENTITY") > -1) {
                                if (objdata[i].Status.indexOf("please specify") <= -1)
                                    $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].disabled = "true"
                            }

                            //Jira CAP-447
                            //else if ((objdata[i].Care_Name_Value.toUpperCase().indexOf("BREAST") > -1)) {
                            //    lbl[0].parentNode.nextElementSibling.children[0].disabled = "true";
                            //    lbl[0].parentNode.nextElementSibling.children[0].style.backgroundColor = "rgba(191, 219, 255,1)";
                            //    $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                            //}
                            if (isChanged && objdata[i].Status == "") {
                                lbl[0].parentNode.nextElementSibling.children[0].selectedIndex = 0;
                            }
                            else if (isChanged != 0 && objdata[i].Status == "Yes") {
                                lbl[0].parentNode.nextElementSibling.children[0].selectedIndex = 1;
                            }
                            else if (isChanged != 0 && objdata[i].Status == "No") {
                                lbl[0].parentNode.nextElementSibling.children[0].selectedIndex = 2;
                            }

                        } else if (lbl[0].parentNode.nextElementSibling.children[0].className.indexOf("stylTxtCtrl") >= 0) {

                            if (objdata[i].Care_Name_Value == "BMI") {
                                if (isChanged && objdata[i].Vitals_BMI != undefined && objdata[i].Vitals_BMI != "") {
                                    lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Vitals_BMI;
                                }
                                else if (isChanged && objdata[i].Status_Value != "") {
                                    lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                                }
                                if (objdata[i].Vitals_BMI_Status_Value != undefined && objdata[i].Vitals_BMI_Status_Value != "") {
                                    if ($(lbl).parents("tr")[0].nextElementSibling != null) {
                                        $(lbl).parents("tr")[0].nextElementSibling.style.display = "block";
                                        $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.style.display = "block";
                                        if (isChanged) {
                                            $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.value = objdata[i].Vitals_BMI_Status_Value;
                                        }
                                    }
                                }

                                lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                            }
                            else if (objdata[i].Care_Name_Value.toUpperCase() == "HEMOGLOBIN") { //BugID:51648
                                if (isChanged) {
                                    lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                                }
                                if (lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0] != undefined)
                                    lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                            }

                            else if (isChanged && objdata[i].Care_Name_Value.toUpperCase().indexOf("TOBACCO") > -1) {
                                if (objdata[i].Status != undefined && objdata[i].Status != "") {
                                    if (objdata[i].Status.split('|')[0] != "") {
                                        lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status.split('|')[0];
                                        $(lbl[0].parentNode.nextElementSibling.children[0]).attr('title', objdata[i].Status.split('|')[0]);
                                        if (objdata[i].Status.split('|')[1] != undefined && objdata[i].Status.split('|')[1] != null)
                                            localStorage.setItem('Tobacco', objdata[i].Status.split('|')[1]);
                                    }


                                }



                                lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');
                            }

                            else if (objdata[i].Care_Name_Value.toUpperCase() == "BP SYS/DIA") {
                                if (isChanged) {
                                    if (objdata[i].Vitals_BP != undefined && objdata[i].Vitals_BP != "") {
                                        lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Vitals_BP;
                                    }
                                    else if (objdata[i].Status_Value != "") {
                                        lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                                    }
                                }
                                if (objdata[i].Vitals_BP_Status_Value != undefined && objdata[i].Vitals_BP_Status_Value != "") {
                                    if ($(lbl).parents("tr")[0].nextElementSibling != null) {
                                        $(lbl).parents("tr")[0].nextElementSibling.style.display = "block";
                                        $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.style.display = "block";
                                        if (isChanged) {
                                            $(lbl).parents("tr")[0].nextElementSibling.children[1].firstElementChild.value = objdata[i].Vitals_BP_Status_Value;
                                        }
                                    }
                                }

                                lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');

                            }
                            else {
                                if (isChanged) {
                                    lbl[0].parentNode.nextElementSibling.children[0].value = objdata[i].Status_Value;
                                }
                            }
                        }
                        if (isChanged && lbl[0].parentNode.nextElementSibling.nextElementSibling.children.length != 0) {
                            lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].value = objdata[i].Plan_Date.split(" ")[0];
                            $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Care_Plan_Notes;

                            if (objdata[i].Id == undefined) {
                                lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + "0";
                            }
                            else {
                                lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + objdata[i].Id;
                            }

                        }
                        else {
                            if (isChanged) {
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Care_Plan_Notes;
                                if (objdata[i].Id == undefined) {
                                    lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + "0";
                                }
                                else {
                                    lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling.nextElementSibling.children[0].textContent = objdata[i].Version + '-' + objdata[i].Id;
                                }
                            }
                        }
                        //BugID:47880
                        if (isChanged && objdata[i].Care_Name_Value.toUpperCase().indexOf("TOBACCO") > -1 && objdata[i].Status != undefined && objdata[i].Status != "" && objdata[i].Status.split('|').length == 3) {
                            if (objdata[i].Care_Plan_Notes.trim() == "") {
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Status.split('|')[2];
                                EnableSave();
                            }

                        }
                        //BugID:47887
                        if (isChanged && objdata[i].Care_Name_Value.toUpperCase() == "BP SYS/DIA" && objdata[i].Status != undefined && objdata[i].Status.trim() != "") {
                            if (objdata[i].Care_Plan_Notes.trim() == "" && objdata[i].Status.indexOf("|") != -1 && objdata[i].Status.split("|").length == 2) {
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.nextElementSibling).find('textarea')[0].value = objdata[i].Status.split('|')[1];
                                EnableSave();
                            }
                        }

                        //Jira CAP-339
                        if (MasterID[objdata[i].Care_Plan_Lookup_ID] != 0) {
                            if (lbl[0].parentNode.nextElementSibling.children[0] != null && lbl[0].parentNode.nextElementSibling.children[0] != undefined && lbl[0].parentNode.nextElementSibling.children[0].disabled == false) {
                                lbl[0].parentNode.nextElementSibling.children[0].disabled = "true";
                                lbl[0].parentNode.nextElementSibling.children[0].style.backgroundColor = "rgba(191, 219, 255,1)";
                            }
                            if (lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled != null && lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled != undefined)
                                lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0].disabled = "true";
                            if ($(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]) != null && $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]) != undefined)
                                $(lbl[0].parentNode.nextElementSibling.nextElementSibling.children[0]).datepicker('disable');

                        }

                    }

                }
            }
        }

        for (var m = 0; m < objCareName.length; m++) {
            $("[id*='" + objCareName[m] + "']").css("display", "none");
        }
}