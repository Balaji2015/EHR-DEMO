var vversion;

$(document).ready(function () {
    localStorage.removeItem("MyShowAll");
    localStorage.removeItem("MyOpenTask");
    localStorage.removeItem("MyTask14");
    localStorage.removeItem("MyShowAllMyTask");
    localStorage.removeItem("ShowallGeneralqueue");
    //localStorage.removeItem("ClientIpAddress");
    var version = document.getElementById('hdnVersion').value;
    var ProjectName = document.getElementById('hdnProjectName').value;
    //Jira - #CAP-80
    //sessionStorage.setItem("Projname", ProjectName.trim().toUpperCase());
    localStorage.setItem("Projname", ProjectName.trim().toUpperCase());
    var ReportPath = document.getElementById('hdnreportPath').value;
    sessionStorage.setItem("ReportPath", ReportPath);
    var LoginHeader = document.getElementById('hdnLoginheader').value;

    var versionkey = document.getElementById('hdnVersionKey').value;
    sessionStorage.setItem("versionkey", versionkey);

    var vEVServiceLink = document.getElementById('hdnServiceLink').value;
    sessionStorage.setItem("EVWebServiceLink", vEVServiceLink);

    var vEVProjectName = document.getElementById('hdnEvProjectName').value;
    sessionStorage.setItem("EVProjectName", vEVProjectName);
    var ReportPathhttp = document.getElementById('hdnReportPathhttp').value;
    sessionStorage.setItem("ReportPathhttp", ReportPathhttp);
    //document.getElementById('lblProduct').innerHTML = "EHR <span style='color:black;font-size:13px;font-weight:500;'> - " + version.replace('Capella - ', '') + "</span>";
    ////Jira - #CAP-80
    //Use localstorage insted of Sessionstorage and currently use sessionstorage also because of sessionstorage use more page
    if (version.split('-')[1].trim() != undefined && version.split('-')[1].trim() != null) {
        sessionStorage.setItem("ScriptVersion", version.split('-')[1].trim());
        localStorage.setItem("ScriptVersion", version.split('-')[1].trim());
    }
    //document.getElementById('lblProduct').innerHTML = "EHR <span style='color:black;font-size:13px;font-weight:500;'> - " + version.replace('Capella - ', '') + "</span>";
/*    $($('#ulSystemMessages')[0].parentElement).css('overflow', 'auto');*/
    if (sessionStorage.getItem("MailClinicalCnt") != null && sessionStorage.getItem("MailClinicalCnt") != undefined)//BugID:48547
        sessionStorage.removeItem("MailClinicalCnt");
    if (sessionStorage.getItem("importCount") != null && sessionStorage.getItem("importCount") != undefined)
        sessionStorage.removeItem("importCount");
    if (sessionStorage.getItem("RxCount") != null && sessionStorage.getItem("RxCount") != undefined)//BugID:48547
        sessionStorage.removeItem("RxCount");
    LoadSystemMessagesKnowledgeCenterdetails();
    $('[id^=dvpanelheading]').addClass("panel-heading");

    $('[id^=divpanelsucess]').addClass("Loginboder");


    //$('.logocolor').attr('style', sessionStorage.getItem("logocolor"));
    sessionStorage.removeItem("logocolor");
    // $('#lblProduct').addClass("logoEHR");
    sessionStorage.removeItem("logoEHR");
    /*$('#spnlogo').addClass("logoLogin");*/

    vversion = sessionStorage.getItem("versionkey");
    sessionStorage.removeItem("versionkey");

    $('#imgright').addClass("logoRight");
    $('#imgleft').addClass("logoleft");
    $('#ulKnowledgecenter').find('a').addClass('Editabletxtbox')
    //if (vversion != undefined && vversion != "" && vversion.toUpperCase() == "ENABLE") {
    //    $("#lblProduct").show();
    //}
    //else {
    //    $("#lblProduct").hide();
    //}
});

function LoadSystemMessagesKnowledgeCenterdetails() {
    localStorage.setItem("PFSHVerified", "");
    $.ajax({
        type: "GET",
        url: "ConfigXML/LoginMessage.xml",
        dataType: "xml",
        async: false,
        cache: false,
        success: function (xml) {

            $(xml).find('KeyFeatures').each(function () {
                var name_text = $(this)[0].attributes[0].nodeValue;
                var description = $(this)[0].attributes[1].nodeValue;
                var html = "";

                if ($(this)[0].attributes[1].nodeValue != null && $(this)[0].attributes[1].nodeValue != '') {
                    html = "<li style='padding-top: 5px;color:green;' class='fa fa-check'><a href='" + description + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + name_text + "</a></li></br>";
                    $('#ulFeatures').append($(html));
                }
                else {
                    html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + name_text + "</h10></li></br>";
                    $('#ulFeatures').append($(html));
                }
            });
            $(xml).find('SystemMessage').each(function () {
                var html = "";
                var name_text = $(this)[0].attributes[0].nodeValue;
                var descrip = $(this)[0].attributes[1].nodeValue;

                if ($(this)[0].attributes[1].nodeValue != null && $(this)[0].attributes[1].nodeValue != '') {
                    html = "<li style='color:green;' class='fa fa-check'><a href='" + descrip + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + name_text + "</a></li></br>";
                    $('#ulSystemMessages').append($(html));
                }
                else {
                    html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + name_text + "</h10></li>";
                    $('#ulSystemMessages').append($(html));
                }
            });
            $(xml).find('KnowledgeCentre').each(function () {

                var $book = $(this);
                var title = $book.attr("Name");
                var description = $book.attr("link");

                if ($book.attr("link") != null && $book.attr("link") != '') {
                    html = "<li style='color:green;' class='fa fa-check'><a href='" + description + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + title + "</a></li></br>";
                    $('#ulKnowledgecenter').append($(html));
                }
                else {
                    html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + title + "</h10></li>";
                    $('#ulKnowledgecenter').append($(html));
                }
            });
            $(xml).find('Address').each(function () {
                var $Addressdetails = $(this)[0].outerHTML;
                $('#pContactDetails').append($Addressdetails);
            });
        }
    });

}

function CheckMandatory(txtbx) {
    localStorage.setItem('PhysicianCPT', '');
    localStorage.setItem('PhysicianICD', '');
    document.getElementById('txtPassword').focus();

    if ((document.getElementById('txtUserName').value.indexOf('!') != -1) || (document.getElementById('txtUserName').value.indexOf('{') != -1) || (document.getElementById('txtUserName').value.indexOf('}') != -1) || (document.getElementById('txtUserName').value.indexOf('\\') != -1) || (document.getElementById('txtUserName').value.indexOf('[') != -1) || (document.getElementById('txtUserName').value.indexOf(']') != -1) || (document.getElementById('txtUserName').value.indexOf('<') != -1) || (document.getElementById('txtUserName').value.indexOf('>') != -1) || (document.getElementById('txtUserName').value.indexOf('?') != -1) || (document.getElementById('txtUserName').value.indexOf(';') != -1) || (document.getElementById('txtUserName').value.indexOf(',') != -1) || (document.getElementById('txtUserName').value.indexOf('|') != -1) || (document.getElementById('txtUserName').value.indexOf(':') != -1) || (document.getElementById('txtUserName').value.indexOf('/') != -1)) {
        document.getElementById('hdnOkButton').value = "false";
        DisplayErrorMessage('000019');
        document.getElementById('txtUserName').value = "";
        document.getElementById('txtPassword').value = "";
        document.getElementById('txtUserName').focus();

        return false;
    }
    else if ((document.getElementById('txtPassword').value.indexOf('{') != -1) || (document.getElementById('txtPassword').value.indexOf('}') != -1) || (document.getElementById('txtPassword').value.indexOf('\\') != -1) || (document.getElementById('txtPassword').value.indexOf('[') != -1) || (document.getElementById('txtPassword').value.indexOf(']') != -1) || (document.getElementById('txtPassword').value.indexOf('<') != -1) || (document.getElementById('txtPassword').value.indexOf('>') != -1) || (document.getElementById('txtPassword').value.indexOf('?') != -1) || (document.getElementById('txtPassword').value.indexOf(';') != -1) || (document.getElementById('txtPassword').value.indexOf(',') != -1) || (document.getElementById('txtPassword').value.indexOf('|') != -1) || (document.getElementById('txtPassword').value.indexOf(':') != -1) || (document.getElementById('txtPassword').value.indexOf('/') != -1)) {
        document.getElementById('hdnOkButton').value = "false";
        DisplayErrorMessage('000019');
        //CAP-325 - remove $find and add document.getElementById
        document.getElementById('txtPassword').value = "";
        document.getElementById('txtUserName').value = "";
        document.getElementById('txtUserName').focus();
        return false;
    }
    else if (document.getElementById('txtUserName').value == "") {
        document.getElementById('hdnOkButton').value = "false";
        DisplayErrorMessage('010002');
        document.getElementById('txtUserName').focus();
        return false;
    }
    else if (document.getElementById('txtPassword').value == "") {
        document.getElementById('hdnOkButton').value = "false";
        DisplayErrorMessage('010004');
        document.getElementById('txtPassword').focus();
        return false;
    }
    else if (document.getElementById('ddlFacility').value == "" || document.getElementById('ddlFacility').value == "0") {
        document.getElementById('hdnOkButton').value = "false";
        DisplayErrorMessage('010005');
        document.getElementById('ddlFacility').focus();
        return false;
    }
    else {
        document.getElementById('hdnOkButton').value = "true";
        document.getElementById('hdnFacltyName').value = document.getElementById('ddlFacility').value;
        //setTimeZone();
        //ShowLoading();
        //getIpAddress();
        return true;
    }

}

//function getIpAddress() {
//    $.ajax({
//        type: "POST",
//        url: "frmRCopiaToolbar.aspx/GetIPAddress",
//        dataType: "json",
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            localStorage.setItem("ClientIpAddress", data.d);
//        },
//        error: function OnError(xhr) {
//        }
//    });
//}

//function setTimeZone() {
//    //var hiddenObj = document.getElementById('hdnLocalTime');
//    //hiddenObj.value = showTime().toString();


//}
//function showTime() {
//    var dt = new Date();
//    var LocalTime = dt.stdTimezoneOffset();
//    var LocalDate = dt.toLocaleDateString("en-US");

//    if (LocalDate.indexOf("/") != -1) {
//        var LocalDatenew = LocalDate.split('/');
//        var day = ("0" + (LocalDatenew[1])).slice(-2);
//        LocalDate = LocalDatenew[0] + "/" + day + "/" + LocalDatenew[2];
//    }
//    if (LocalDate.indexOf("-") != -1) {
//        var LocalDatenew = LocalDate.split('-');
//        var day = ("0" + (LocalDatenew[1])).slice(-2);
//        LocalDate = LocalDatenew[0] + "/" + day + "/" + LocalDatenew[2];
//    }
//    document.getElementById('hdnUniversaloffset').value = createOffset(dt);
//    var dt1 = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalDateAndTime").value = dt1.toLocaleTimeString();

//    document.getElementById('hdnLocalDate').value = LocalDate;
//    document.getElementById('hdnFollowsDayLightSavings').value = (dt.dst()).toString().toLowerCase();
//    return LocalTime;

//}
//function pad(value) {
//    return value < 10 ? '0' + value : value;
//}
//function createOffset(date) {
//    var sign = (date.stdTimezoneOffset() > 0) ? "-" : "+";
//    var offset = Math.abs(date.stdTimezoneOffset());
//    var hours = pad(Math.floor(offset / 60));
//    var minutes = pad(offset % 60);
//    return sign + hours + "." + minutes;
//}
function SessionExpired() {
    window.location.href = "frmLoginNew.aspx";
}
function Validations(sender, args) {
    if ($find('txtUserName')._value == "") {
        DisplayErrorMessage('010002');
        sender.set_autoPostBack(false);
        document.getElementById('txtUserName').focus();
    }
    else if ($find('txtOldPassword')._value == "") {
        if ($find('txtOldPassword').get_enabled() == true) {
            DisplayErrorMessage('010007');
            sender.set_autoPostBack(false);
            document.getElementById('txtOldPassword').focus();
        }
    }
    else if ($find('txtNewPassword')._value == "") {
        DisplayErrorMessage('010008');
        sender.set_autoPostBack(false);
        document.getElementById('txtNewPassword').focus();
    }
    else if (($find('txtNewPassword')._value.indexOf('{') != -1) || ($find('txtNewPassword')._value.indexOf('}') != -1) || ($find('txtNewPassword')._value.indexOf('\\') != -1) || ($find('txtNewPassword')._value.indexOf('[') != -1) || ($find('txtNewPassword')._value.indexOf(']') != -1) || ($find('txtNewPassword')._value.indexOf('<') != -1) || ($find('txtNewPassword')._value.indexOf('>') != -1) || ($find('txtNewPassword')._value.indexOf('?') != -1) || ($find('txtNewPassword')._value.indexOf(';') != -1) || ($find('txtNewPassword')._value.indexOf(',') != -1) || ($find('txtNewPassword')._value.indexOf('|') != -1) || ($find('txtNewPassword')._value.indexOf(':') != -1) || ($find('txtNewPassword')._value.indexOf('/') != -1)) {
        DisplayErrorMessage('000019');
        sender.set_autoPostBack(false);

        $find('txtNewPassword').clear();
        document.getElementById('txtNewPassword').focus();
    }

    else if ($find('txtConfirmPassword')._value == "") {
        DisplayErrorMessage('010009');
        sender.set_autoPostBack(false);
        document.getElementById('txtConfirmPassword').focus();
    }
    else if ($find('txtNewPassword')._value != $find('txtConfirmPassword')._value) {
        DisplayErrorMessage('010010');
        sender.set_autoPostBack(false);
        $find('txtNewPassword').clear();
        $find('txtConfirmPassword').clear();
        document.getElementById('txtNewPassword').focus();
    }
    else if ($find('txtOldPassword')._value == $find('txtNewPassword')._value) {
        DisplayErrorMessage('010016');
        sender.set_autoPostBack(false);
        $find('txtOldPassword').clear();
        $find('txtNewPassword').clear();
        $find('txtConfirmPassword').clear();
        document.getElementById('txtOldPassword').focus();
    }
    else {
        sender.set_autoPostBack(true);
    }
}
function CheckCapsLock(e, object) {
    var myKeyCode = 0;
    var myShiftKey = e.shiftKey;

    if (document.all) {
        myKeyCode = e.keyCode;
    } else if (document.getElementById) {
        myKeyCode = e.which;
    }

    if ((myKeyCode >= 65 && myKeyCode <= 90) || (myKeyCode >= 97 && myKeyCode <= 122)) {
        if (

            ((myKeyCode >= 65 && myKeyCode <= 90) && !myShiftKey)

            ||


            ((myKeyCode >= 97 && myKeyCode <= 122) && myShiftKey)
        ) {
            if (object.id == "txtUserName") {
                var toolTip = $find('UserNameTooltip');
                toolTip.show();
            }
            else if (object.id == "txtPassword") {
                var toolTip = $find('PassWordToolTip');
                toolTip.show();
            }

        }
        else {
            if (object.id == "txtUserName") {
                var toolTip = $find('UserNameTooltip');
                toolTip.hide();
            }
            else if (object.id == "txtPassword") {
                var toolTip = $find('PassWordToolTip');
                if (toolTip != null)
                    toolTip.hide();
            }


        }
    }
}
$('#txtUserName').focusout(function () {
    GetUserName();
});

function GetUserName() {
    //if (document.getElementById('ddlFacility').value == '0' || document.getElementById('ddlFacility').value == '') {
    if (document.getElementById('txtUserName').value != '') {
        var User_name = document.getElementById('txtUserName').value.trim();
        $.ajax({
            type: "POST",
            url: "frmLogin.aspx/LoadFacility",
            data: '{UserName: "' + User_name + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessRCopia,
            error: function OnError(xhr) {
                if (xhr.status == 999) {
                    window.location = "/frmSessionExpired.aspx";
                    //if (xhr.statusText == "Session Expired")
                    //{
                    //    window.location = "frmSessionExpired.aspx";
                    //}
                    //else
                    //{
                    //    window.location = xhr.statusText;
                    //}
                }
                else {
                    //CAP-798 Unexpected end of JSON input
                    if (isValidJSON(xhr.responseText)) {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                            "Message: " + log.Message);
                    } else {
                        alert("USER MESSAGE:\n" +
                            ". Cannot process request. Please Login again and retry.");
                    }

                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    //}
}


function Focus(textbox, event) {
    if (document.getElementById('ddlFacility').value == '') {
        if (document.getElementById('txtUserName').value != '') {
            var User_name = document.getElementById('txtUserName').value;
            $.ajax({
                type: "POST",
                url: "frmLogin.aspx/LoadFacility",
                data: '{UserName: "' + User_name + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessRCopia,
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
}

function OnSuccessRCopia(response) {

    //document.getElementById('ddlFacility').value = response.d;
    var sResult = JSON.parse(response.d);
    $('#ddlFacility').find('option').remove().end().append('<option value="0">Select Facility</option>').val('Select Facility');
    if (sResult.FacilityList != null) {
        $.each(sResult.FacilityList, function (key, value) {
            $('#ddlFacility').append($("<option></option>").attr("value", value).text(value));
        });
    }
    document.getElementById('ddlFacility').value = sResult.FacilityName;


}

//function ShowLoading() {
//    if (localStorage["phyIDList"] != undefined) {
//        var phyIDList = JSON.parse(localStorage["phyIDList"]);
//        var list = Object.keys(phyIDList).map(function (key) { return phyIDList[key] });
//        var strList = "";
//        for (var i = 0; i < list.length; i++) {
//            strList += list[i] + "#";
//        }
//        strList = strList.substr(0, strList.length);
//        document.cookie = "LocalStorage=" + strList + ";path=/;";
//    }
//}

//Date.prototype.stdTimezoneOffset = function () {
//    var jan = new Date(this.getFullYear(), 0, 1);
//    var jul = new Date(this.getFullYear(), 6, 1);
//    return Math.max(jan.getTimezoneOffset(), jul.getTimezoneOffset());
//}
//Date.prototype.dst = function () {
//    var jan = new Date(this.getFullYear(), 0, 1);
//    var jul = new Date(this.getFullYear(), 6, 1);
//    var Jan_Offset = jan.getTimezoneOffset();
//    var July_Offset = jul.getTimezoneOffset();
//    var difference = Jan_Offset - July_Offset;
//    if (difference != 0)
//        return true;
//    else
//        return false;
//}

function CheckLastModified(sDates) {
    sessionStorage.clear();
    if ((localStorage.length == 0) || (localStorage.length == 1 && localStorage["phyIDList"] != undefined)) {
        document.cookie = 'LocalStorage=; expires=Thu, 01 Jan 1970 00:00:01 GMT;path=/;';
        localStorage.clear();
    }
    var modList = sDates.split('%');
    if (localStorage["phyIDList"] != undefined) {
        var phyIDList = JSON.parse(localStorage["phyIDList"]);
        for (var i = 0; i < modList.length; i++) {
            var field = modList[i].split("|")[0];
            var lastmodified = new Date(modList[i].split("|")[1]);
            for (var j = 0; j < phyIDList.length; j++) {
                if (phyIDList[j] != "") {
                    var phyId = phyIDList[j].split('^')[0].replace("*", "");
                    var item = phyIDList[j].split('^')[1].split('%')[0];
                    var local = new Date(phyIDList[j].split('^')[1].split('%')[1]);
                    if ((field == item || field == phyId) && (lastmodified >= local)) {
                        if (item == "E AND M PROCEDURE" && phyId != "POS MODIFIER") {
                            localStorage.removeItem("CPTArrayE&M" + phyId);
                            localStorage.removeItem("RootNodesE&M" + phyId);
                        }
                        else
                            if (item == "FREQUENTLY USED ICD") {
                                localStorage.removeItem("ICDArray" + phyId);
                                localStorage.removeItem("ICDRootNodes" + phyId);
                            }
                            else
                                if (item == "E AND M PROCEDURE" && phyId == "POS MODIFIER") {
                                    localStorage.removeItem("POS");
                                    localStorage.removeItem("MODIFIER");
                                }
                                else
                                    if (item == "ENCOUNTER" && phyId == "FACILITY_LIST") {
                                        localStorage.removeItem("FACILITY_LIST");
                                    }
                        phyIDList.splice(j, 1);
                        j--;
                    }
                }
                else {
                    phyIDList.splice(j, 1);
                    j--;
                }
            }
        }
        if (phyIDList.length > 0)
            localStorage.setItem("phyIDList", JSON.stringify(phyIDList));
        else
            localStorage.removeItem("phyIDList");
    }//phyIDList may become null. So before setting cookie, check not null condition again.
    if (localStorage["phyIDList"] != undefined) {
        phyIDList = JSON.parse(localStorage["phyIDList"]);
        var list = Object.keys(phyIDList).map(function (key) { return phyIDList[key] });
        var strList = "";
        for (var i = 0; i < list.length; i++) {
            strList += list[i] + "#";
        }
        strList = strList.substr(0, strList.length);
        document.cookie = "LocalStorage=" + strList + ";path=/;";
    }
    if (localStorage["FACILITY_LIST"] != undefined) {
        var Faclist = JSON.parse(localStorage["FACILITY_LIST"]);
        var list = Object.keys(Faclist).map(function (key) { return Faclist[key] });
        var strList = "";
        for (var i = 0; i < list.length; i++) {
            strList += list[i] + "!";
        }
        strList = strList.substr(0, strList.length - 1);
        document.cookie = "AllFacility=" + strList + ";path=/;";
    }
}

function AlertUser() {
    //CAP-2504
    if ((window.location?.origin??"") == "https://chart-stage.akidolabs.com") {
        document.getElementById("hdnbtnLogin").click();
    }
    else {
    var Continue = DisplayErrorMessage('010021');
    if (Continue == true) {
        document.getElementById("hdnbtnLogin").click();
    }
    else if (Continue == false) {
        window.location.href = "frmLoginNew.aspx";
    }
}
}


function EHRLanding(FileName) {
    localStorage.removeItem("MyShowAll");
    localStorage.removeItem("MyOpenTask");
    localStorage.removeItem("MyTask14");
    localStorage.removeItem("MyShowAllMyTask");
    localStorage.removeItem("ShowallGeneralqueue");
    //localStorage.removeItem("ClientIpAddress");
    var version = document.getElementById('hdnVersion').value;
    var ProjectName = document.getElementById('hdnProjectName').value;
    //Jira - #CAP-80
    //sessionStorage.setItem("Projname", ProjectName.trim().toUpperCase());
    if (ProjectName != undefined && ProjectName != null) {
        localStorage.setItem("Projname", ProjectName.trim().toUpperCase());
    }
    var ReportPath = document.getElementById('hdnreportPath').value;
    sessionStorage.setItem("ReportPath", ReportPath);
    var LoginHeader = document.getElementById('hdnLoginheader').value;

    var versionkey = document.getElementById('hdnVersionKey').value;
    sessionStorage.setItem("versionkey", versionkey);

    var vEVServiceLink = document.getElementById('hdnServiceLink').value;
    sessionStorage.setItem("EVWebServiceLink", vEVServiceLink);

    var vEVProjectName = document.getElementById('hdnEvProjectName').value;
    sessionStorage.setItem("EVProjectName", vEVProjectName);
    var ReportPathhttp = document.getElementById('hdnReportPathhttp').value;
    sessionStorage.setItem("ReportPathhttp", ReportPathhttp);
    //document.getElementById('lblProduct').innerHTML = "EHR <span style='color:black;font-size:13px;font-weight:500;'> - " + version.replace('Capella - ', '') + "</span>";
    //Jira - #CAP-80
    //Use localstorage insted of Sessionstorage and currently use sessionstorage also because of sessionstorage use more page
    if (version.split('-')[1].trim() != undefined && version.split('-')[1].trim() != null) {
        sessionStorage.setItem("ScriptVersion", version.split('-')[1].trim());
        localStorage.setItem("ScriptVersion", version.split('-')[1].trim());
    }

    //document.getElementById('lblProduct').innerHTML = "EHR <span style='color:black;font-size:13px;font-weight:500;'> - " + version.replace('Capella - ', '') + "</span>";
    // $($('#ulSystemMessages')[0].parentElement).css('overflow', 'auto');
    //if (sessionStorage.getItem("MailClinicalCnt") != null && sessionStorage.getItem("MailClinicalCnt") != undefined)//BugID:48547
    //    sessionStorage.removeItem("MailClinicalCnt");
    //if (sessionStorage.getItem("importCount") != null && sessionStorage.getItem("importCount") != undefined)
    //    sessionStorage.removeItem("importCount");
    //if (sessionStorage.getItem("RxCount") != null && sessionStorage.getItem("RxCount") != undefined)//BugID:48547
    //    sessionStorage.removeItem("RxCount");
    //$('[id^=dvpanelheading]').addClass("panel-heading");

    //$('[id^=divpanelsucess]').addClass("Loginboder");


    //$('.logocolor').attr('style', sessionStorage.getItem("logocolor"));
    //sessionStorage.removeItem("logocolor");
    //$('#lblProduct').addClass("logoEHR");
    //sessionStorage.removeItem("logoEHR");
    //$('#spnlogo').addClass("logoLogin");

    //vversion = sessionStorage.getItem("versionkey");
    //sessionStorage.removeItem("versionkey");

    //$('#imgright').addClass("logoRight");
    //$('#imgleft').addClass("logoleft");
    //$('#ulKnowledgecenter').find('a').addClass('Editabletxtbox')
    //if (vversion != undefined && vversion != "" && vversion.toUpperCase() == "ENABLE") {
    //    $("#lblProduct").show();
    //}
    //else {
    //    $("#lblProduct").hide();
    //}

    //localStorage.setItem("PFSHVerified", "");
    //CAP-2504
    if ((window.location?.origin ?? "") == "https://chart-stage.akidolabs.com") {
        $("iframe").contentWindow.location.href = FileName;
    }
    else {
    top.window.location = FileName;
    }
}

$(top.window.document).find('#btnErrorCancel').unbind("click");
$(top.window.document).find('#btnErrorCancel').on("click", function () {
    //CAP-2379 & CAP-2389
    if ($('#pErrorMsg').text() == 'Invalid User Name and/or Password.'
        || $('#pErrorMsg').text() == 'Capella is undergoing scheduled downtime. The application will now exit.'
        || ($('#pErrorMsg').text()).includes('You are not a permitted user. Please contact the System Administrator.')) {
        location.href = 'frmLoginNew.aspx?IsLoginRequired=true';
    }
});

$(top.window.document).find('#btnErrorOkCancel').unbind("click");
$(top.window.document).find('#btnErrorOkCancel').on("click", function () {
    //CAP-2379 & CAP-2389
    if ($('#pErrorMsg').text() == 'Invalid User Name and/or Password.'
        || $('#pErrorMsg').text() == 'Capella is undergoing scheduled downtime. The application will now exit.'
        || ($('#pErrorMsg').text()).includes('You are not a permitted user. Please contact the System Administrator.')) {
        location.href = 'frmLoginNew.aspx?IsLoginRequired=true';
    }
});