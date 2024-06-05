function changeReload() {   
    //CAP-1911
    //window.parent.location.href = window.parent.location.href;
    window.parent.parent.location.reload();
}
//CAP-2007
function cboLegalOrg_SelectedIndexChanged(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function CheckUserFields() {
    //CAP-2007

    var isCheckLegalOrg = document.getElementById(GetClientId("hdnIsLegaLOrgValidation"))?.value ?? "";
    if (isCheckLegalOrg == "Y") {
        if ($("#cboLegalOrg").val() == undefined || $("#cboLegalOrg").val() == null || $("#cboLegalOrg").val() == "") {
            DisplayErrorMessage("010026");
            return false;
        }
    }
    // Legal organization selection is not required, or the validation passed
    //return true;
    if ($("#cboFacilityName :selected").val() == undefined || $("#cboFacilityName :selected").val() == null || $("#cboFacilityName :selected").val() == "")
    {
        DisplayErrorMessage("010005");
        return false;
    }
    //CAP-2007
    setTimeZone();
    return true;
}

function RadWindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null) {
        oWindow.close();
        //location.href = location.href;
    }       
    return false;
}

$(document).ready(function () {
    //document.getElementById('cboFacilityName').focus();
    //CAP-2007
    var isCheckLegalOrg = document.getElementById(GetClientId("hdnHumanID"))?.value ?? "";
    if (isCheckLegalOrg == "Y") {
        document.getElementById('cboLegalOrg').focus();
    }
    else {
        document.getElementById('cboFacilityName').focus();
    }
});

function CloseWindow() {
    self.close();
}

//CAP-2007
function AlertUser() {
    var Continue = DisplayErrorMessage('010021');
    if (Continue == true) {
        document.getElementById("hdnbtnLogOutAndLogIn").click();
    }
    else if (Continue == false) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //window.location.reload();
    }

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
//var input = document.getElementById("cboUserName");
//input.addEventListener("keypress", function (event) {
//    if (event.key === "Enter") {
//        event.preventDefault();
//        document.getElementById("btnOk").click();
//    }
//});

//var input = document.getElementById("txtPassword");
//input.addEventListener("keypress", function (event) {
//    if (event.key === "Enter") {
//        event.preventDefault();
//        document.getElementById("btnOk").click();
//    }
//});

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


    top.window.location = FileName;
}

//Jira CAP-1787
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