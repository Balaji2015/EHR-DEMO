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
        if (document.getElementById('lblProduct') != undefined && document.getElementById('lblProduct') != null) {
            document.getElementById('lblProduct').innerHTML = "EHR <span style='color:black;font-size:13px;font-weight:500;'> - " + version.replace('Capella - ', '') + "</span>";
        }
        //Jira - #CAP-80
        //Use localstorage insted of Sessionstorage and currently use sessionstorage also because of sessionstorage use more page
        if (version?.split('-')[1]?.trim() != undefined && version?.split('-')[1]?.trim() != null) {
            sessionStorage.setItem("ScriptVersion", version.split('-')[1].trim());
            localStorage.setItem("ScriptVersion", version.split('-')[1].trim());
        }
        if (document.getElementById('lblProduct') != undefined && document.getElementById('lblProduct') != null) {
            document.getElementById('lblProduct').innerHTML = "EHR <span style='color:black;font-size:13px;font-weight:500;'> - " + version.replace('Capella - ', '') + "</span>";
        }

        if ($('#ulSystemMessages')[0] != undefined) {
            $($('#ulSystemMessages')[0].parentElement).css('overflow', 'auto');
        }

        if (sessionStorage.getItem("MailClinicalCnt") != null && sessionStorage.getItem("MailClinicalCnt") != undefined)//BugID:48547
            sessionStorage.removeItem("MailClinicalCnt");
        if (sessionStorage.getItem("importCount") != null && sessionStorage.getItem("importCount") != undefined)
            sessionStorage.removeItem("importCount");
        if (sessionStorage.getItem("RxCount") != null && sessionStorage.getItem("RxCount") != undefined)//BugID:48547
            sessionStorage.removeItem("RxCount");
        LoadSystemMessagesKnowledgeCenterdetails();
        $('[id^=dvpanelheading]').addClass("panel-heading");

        $('[id^=divpanelsucess]').addClass("Loginboder");


        $('.logocolor').attr('style', sessionStorage.getItem("logocolor"));
        sessionStorage.removeItem("logocolor");
    if ($('#lblProduct').val() != undefined)
    {
            $('#lblProduct').addClass("logoEHR");
        }
        sessionStorage.removeItem("logoEHR");
        $('#spnlogo').addClass("logoLogin");

        vversion = sessionStorage.getItem("versionkey");
        sessionStorage.removeItem("versionkey");

        $('#imgright').addClass("logoRight");
        $('#imgleft').addClass("logoleft");
        $('#ulKnowledgecenter').find('a').addClass('Editabletxtbox')
        if (vversion != undefined && vversion != "" && vversion.toUpperCase() == "ENABLE") {
            $("#lblProduct").show();
        }
        else {
            $("#lblProduct").hide();
        }
        setTimeZone();
        ShowLoading();
        getIpAddress();

        //CAP-2041
        var sharedSessionUrl = document.getElementById('hdnOktaSharedSessionURL');
        var stateParams = document.getElementById('hdnStateParam');
        var localTimeParams = document.getElementById('hdnLocalTime');
        var localDateParams = document.getElementById('hdnLocalDate');
        var universaloffsetParams = document.getElementById('hdnUniversaloffset');
        var localDateAndTimeParams = document.getElementById('hdnLocalDateAndTime');
        var dayLightSavingsParams = document.getElementById('hdnFollowsDayLightSavings');
        var dateParams = (localTimeParams?.value ?? "") + "|" + (localDateParams?.value ?? "") + "|" + (universaloffsetParams?.value ?? "") + "|" + (localDateAndTimeParams?.value ?? "") + "|" + (dayLightSavingsParams?.value ?? "false");
        if (sharedSessionUrl?.value ?? "" != "") {
            //CAP-2921
            var iframe = window.document.createElement("iframe");
            iframe.id = "ifrmLogin";
            iframe.src = sharedSessionUrl.value + "" + btoa(((stateParams?.value ?? "") + "" + dateParams));
            iframe.style.display = "none";
            iframe.onload = function () {
                sessionStorage.setItem('StartLoading', 'false');
                StopLoadFromPatChart();
                if ($('#ifrmLogin')[0]?.contentWindow?.location?.href != null && $('#ifrmLogin')[0]?.contentWindow?.location?.href.includes("frmLoginNew.aspx")) {
                    location.href = $('#ifrmLogin')[0].contentWindow.location.href;
                }
            };
            window.document.body.appendChild(iframe);

            //location.href = sharedSessionUrl.value + "" + btoa(((stateParams?.value ?? "") + "" + dateParams));
        }
});

function LoadSystemMessagesKnowledgeCenterdetails() {
    localStorage.setItem("PFSHVerified", "");

    //Jira CAP-2775
    //$.ajax({
    //    type: "GET",
    //    url: "ConfigXML/LoginMessage.xml",
    //    dataType: "xml",
    //    async: false,
    //    cache: false,
    //    success: function (xml) {

    //        $(xml).find('KeyFeatures').each(function () {
    //            var name_text = $(this)[0].attributes[0].nodeValue;
    //            var description = $(this)[0].attributes[1].nodeValue;
    //            var html = "";

    //            if ($(this)[0].attributes[1].nodeValue != null && $(this)[0].attributes[1].nodeValue != '') {
    //                html = "<li style='padding-top: 5px;color:green;' class='fa fa-check'><a href='" + description + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + name_text + "</a></li></br>";
    //                $('#ulFeatures').append($(html));
    //            }
    //            else {
    //                html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + name_text + "</h10></li></br>";
    //                $('#ulFeatures').append($(html));
    //            }
    //        });
    //        $(xml).find('SystemMessage').each(function () {
    //            var html = "";
    //            var name_text = $(this)[0].attributes[0].nodeValue;
    //            var descrip = $(this)[0].attributes[1].nodeValue;

    //            if ($(this)[0].attributes[1].nodeValue != null && $(this)[0].attributes[1].nodeValue != '') {
    //                html = "<li style='color:green;' class='fa fa-check'><a href='" + descrip + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + name_text + "</a></li></br>";
    //                $('#ulSystemMessages').append($(html));
    //            }
    //            else {
    //                html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + name_text + "</h10></li>";
    //                $('#ulSystemMessages').append($(html));
    //            }
    //        });
    //        $(xml).find('KnowledgeCentre').each(function () {

    //            var $book = $(this);
    //            var title = $book.attr("Name");
    //            var description = $book.attr("link");

    //            if ($book.attr("link") != null && $book.attr("link") != '') {
    //                html = "<li style='color:green;' class='fa fa-check'><a href='" + description + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + title + "</a></li></br>";
    //                $('#ulKnowledgecenter').append($(html));
    //            }
    //            else {
    //                html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + title + "</h10></li>";
    //                $('#ulKnowledgecenter').append($(html));
    //            }
    //        });
    //        $(xml).find('Address').each(function () {
    //            var $Addressdetails = $(this)[0].outerHTML;
    //            $('#pContactDetails').append($Addressdetails);
    //        });
    //    }
    //});

    //Jira CAP-2775
    $.get("ConfigXML/LoginMessage.json", {}, function (jsonobject) {

        if (jsonobject?.Login?.KeyFeatures != null) {
            jsonobject?.Login?.KeyFeatures.forEach((item) => {
                var name_text = item.Name;
                var description = item.link;
                var html = "";

                if (description != null && description != '') {
                    html = "<li style='padding-top: 5px;color:green;' class='fa fa-check'><a href='" + description + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + name_text + "</a></li></br>";
                    $('#ulFeatures').append($(html));
                }
                else {
                    html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + name_text + "</h10></li></br>";
                    $('#ulFeatures').append($(html));
                }

            });
        }
        if (jsonobject?.Login?.SystemMessage != null) {
            jsonobject?.Login?.SystemMessage.forEach((item) => {
                var html = "";
                var name_text = item.Name;
                var descrip = item.link;

                if (descrip != null && descrip != '') {
                    html = "<li style='color:green;' class='fa fa-check'><a href='" + descrip + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + name_text + "</a></li></br>";
                    $('#ulSystemMessages').append($(html));
                }
                else {
                    html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + name_text + "</h10></li>";
                    $('#ulSystemMessages').append($(html));
                }
            });
        }
        if (jsonobject?.Login?.KnowledgeCentre != null) {
            jsonobject?.Login?.KnowledgeCentre.forEach((item) => {
                var title = item.Name;
                var description = item.link;
                var html = "";

                if (description != null && description != '') {
                    html = "<li style='color:green;' class='fa fa-check'><a href='" + description + "' target='_blank' class='alinkstyle'>&nbsp;&nbsp;&nbsp;" + title + "</a></li></br>";
                    $('#ulKnowledgecenter').append($(html));
                }
                else {
                    html = "<li style='padding-top: 5px;color:green;'  class='fa fa-check'>&nbsp;&nbsp;&nbsp;<h10 class='pagerLink'>" + title + "</h10></li>";
                    $('#ulKnowledgecenter').append($(html));
                }
            });
        }
        if (jsonobject?.Login?.ContactDeatilsList?.Address != null) {
            var $Addressdetails = jsonobject?.Login?.ContactDeatilsList?.Address;
            $('#pContactDetails').append($Addressdetails);
        }
    });
}

function ValidateNext() {
    localStorage.setItem('PhysicianCPT', '');
    localStorage.setItem('PhysicianICD', '');
    $('#txtUserName').focus(); 

    if ($("#txtUserName").val() == undefined || $("#txtUserName").val() == "") {
        DisplayErrorMessage('010002');
        return false;
    }
    else {
        setTimeZone();
        ShowLoading();
        getIpAddress();
        __doPostBack("btnNext", "true");
        return true;
    }
}

function CheckMandatoryPassword() {
    if ($('#txtPassword').val() == undefined || $('#txtPassword').val() == "") {
        DisplayErrorMessage('010004');
        $('#txtPassword').focus();
        return false;
    }
    else {       
        __doPostBack("btnSignin", "true");
        return true;
    }
}
function getIpAddress() {
    $.ajax({
        type: "POST",
        url: "frmRCopiaToolbar.aspx/GetIPAddress",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            localStorage.setItem("ClientIpAddress", data.d);
        },
        error: function OnError(xhr) {
        }
    });
}

function setTimeZone() {
    var hiddenObj = document.getElementById('hdnLocalTime');
    hiddenObj.value = showTime().toString();


}
function showTime() {
    var dt = new Date();
    var LocalTime = dt.stdTimezoneOffset();
    var LocalDate = dt.toLocaleDateString("en-US");

    if (LocalDate.indexOf("/") != -1) {
        var LocalDatenew = LocalDate.split('/');
        var day = ("0" + (LocalDatenew[1])).slice(-2);
        LocalDate = LocalDatenew[0] + "/" + day + "/" + LocalDatenew[2];
    }
    if (LocalDate.indexOf("-") != -1) {
        var LocalDatenew = LocalDate.split('-');
        var day = ("0" + (LocalDatenew[1])).slice(-2);
        LocalDate = LocalDatenew[0] + "/" + day + "/" + LocalDatenew[2];
    }
    document.getElementById('hdnUniversaloffset').value = createOffset(dt);
    var dt1 = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalDateAndTime").value = dt1.toLocaleTimeString();

    document.getElementById('hdnLocalDate').value = LocalDate;
    document.getElementById('hdnFollowsDayLightSavings').value = (dt.dst()).toString().toLowerCase();
    return LocalTime;

}
function pad(value) {
    return value < 10 ? '0' + value : value;
}
function createOffset(date) {
    var sign = (date.stdTimezoneOffset() > 0) ? "-" : "+";
    var offset = Math.abs(date.stdTimezoneOffset());
    var hours = pad(Math.floor(offset / 60));
    var minutes = pad(offset % 60);
    return sign + hours + "." + minutes;
}
function SessionExpired() {
    window.location.href = "frmLoginNew.aspx";
}

function ShowLoading() {
    if (localStorage["phyIDList"] != undefined) {
        var phyIDList = JSON.parse(localStorage["phyIDList"]);
        var list = Object.keys(phyIDList).map(function (key) { return phyIDList[key] });
        var strList = "";
        for (var i = 0; i < list.length; i++) {
            strList += list[i] + "#";
        }
        strList = strList.substr(0, strList.length);
        document.cookie = "LocalStorage=" + strList + ";path=/;";
    }
}

Date.prototype.stdTimezoneOffset = function () {
    var jan = new Date(this.getFullYear(), 0, 1);
    var jul = new Date(this.getFullYear(), 6, 1);
    return Math.max(jan.getTimezoneOffset(), jul.getTimezoneOffset());
}
Date.prototype.dst = function () {
    var jan = new Date(this.getFullYear(), 0, 1);
    var jul = new Date(this.getFullYear(), 6, 1);
    var Jan_Offset = jan.getTimezoneOffset();
    var July_Offset = jul.getTimezoneOffset();
    var difference = Jan_Offset - July_Offset;
    if (difference != 0)
        return true;
    else
        return false;
}

$(top.window.document).find('#btnErrorCancel').unbind("click");
$(top.window.document).find('#btnErrorCancel').on("click", function () {
    if ($('#pErrorMsg').text() == 'Invalid User Name and/or Password.') {
        location.href = location.href;
    }
});

$(top.window.document).find('#btnErrorOkCancel').unbind("click");
$(top.window.document).find('#btnErrorOkCancel').on("click", function () {
    if ($('#pErrorMsg').text() == 'Invalid User Name and/or Password.') {
        location.href = location.href;
    }
});