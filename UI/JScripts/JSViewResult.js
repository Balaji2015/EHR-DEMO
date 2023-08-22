function Clear() {
    $find('txtDocumentSubType').clear();
    var imag = document.getElementById("pbPlus");
    if (imag != null) {
        imag.id = "pbPlus";
    }
    else {
        var image = document.getElementById("pbMinus");
        if (image != null)
            image.id = "pbPlus";
    }
}
function loaddlc() {
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("textarea").addClass('Editabletxtbox');
}
var Result;
var obj = new Array();
var humanid;
function btnpatientChart_Click() {
    var queryString = window.location.search.toString().split('?')[1];
    if (queryString != undefined && queryString != "") {
        humanid = queryString.split("&")[0].split('=')[1];
    }

    //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
    $('#resultLoading').css("display", "none");
    if (Result == null)
        return false;
}

function ClickPrescription() {
    var obj = new Array();
    obj.push("MyType=" + "GENERAL");

    obj.push("HumanID=" + window.location.search.toString().split('?')[1].split("&")[0].split('=')[1]);     //document.getElementById(GetClientId("hdnHumanId")).value);     //$('#txtPatientInformation').val().split('|')[4].split('#:')[1]);
    obj.push("EncID=" + "0");
    obj.push("PrescriptionID=" + "0");
    obj.push("IsMoveButton=" + "true");
    obj.push("IsMoveCheckbox=" + "false");
    obj.push("IsPrescriptiontobePushed=" + "N");
    //obj.push("openingFrom=" + "LabResult");
    obj.push("openingFrom" + "Menu");
    obj.push("IsSentToRCopia=" + "Y");
    obj.push("LabResult=" + "N");
    obj.push("LocalTime=" + document.getElementById(GetClientId('hdnLocalTime')).value);
    var result = openModal("frmRCopiaWebBrowser.aspx", 635, 1060, obj, 'ctl00_ModalWindow');
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
    WindowName.add_close(TriggerDownloadRcopia);
    return false;
}

function TriggerDownloadRcopia(oWindow, args) {

    $.ajax({
        type: "POST",
        url: "frmEncounter.aspx/DownloadRcoipa",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
        },
        error: function (result) {
        }
    });
    //loadRcopia();
}
function OnSuccessRCopia(response) {
    var responseValues = response.d.split('#$%');
    var rxValues = "";

    if (responseValues == "") {
        if (document.getElementById("tsRefill") != null)
            document.getElementById("tsRefill").style.display = "none";
        if (document.getElementById("tsRx_Pending") != null)
            document.getElementById("tsRx_Pending").style.display = "none";
        if (document.getElementById("tsRx_Need_Signing") != null)
            document.getElementById("tsRx_Need_Signing").style.display = "none";
        if (document.getElementById("tsRx_Change") != null)
            document.getElementById("tsRx_Change").style.display = "none";
    }
    else if (responseValues != null && responseValues.length >= 3) {
        if (document.getElementById("tsRefill") != null)
            document.getElementById("tsRefill").innerText = responseValues[0];
        if (document.getElementById("tsRx_Pending") != null)
            document.getElementById("tsRx_Pending").innerText = responseValues[1];
        if (document.getElementById("tsRx_Need_Signing") != null)
            document.getElementById("tsRx_Need_Signing").innerText = responseValues[2];
        if (document.getElementById("tsRx_Change") != null)
            document.getElementById("tsRx_Change").innerText = responseValues[3];
        if (document.getElementById("tsRefill") != null && document.getElementById("tsRx_Pending") != null && document.getElementById("tsRx_Need_Signing") != null && document.getElementById("tsRx_Change") != null && document.getElementById("tsRefill") != undefined && document.getElementById("tsRx_Pending") != undefined && document.getElementById("tsRx_Need_Signing") != undefined && document.getElementById("tsRx_Change") != undefined)
            rxValues = document.getElementById("tsRefill").innerText + "$:$" + document.getElementById("tsRx_Pending").innerText + "$:$" + document.getElementById("tsRx_Need_Signing").innerText + "$:$" + document.getElementById("tsRx_Change").innerText;
    }
    else {
        if (document.getElementById("tsRefill") != null)
            document.getElementById("tsRefill").innerText = "Refill : 0";
        if (document.getElementById("tsRx_Pending") != null)
            document.getElementById("tsRx_Pending").innerText = "Rx_Pending : 0";
        if (document.getElementById("tsRx_Need_Signing") != null)
            document.getElementById("tsRx_Need_Signing").innerText = "Rx_Need_Signing : 0";
        if (document.getElementById("tsRx_Change") != null)
            document.getElementById("tsRx_Change").innerText = "RxChange : 0";

        if (document.getElementById("tsRefill") != null && document.getElementById("tsRx_Pending") != null && document.getElementById("tsRx_Need_Signing") != null && document.getElementById("tsRx_Change") != null && document.getElementById("tsRefill") != undefined && document.getElementById("tsRx_Pending") != undefined && document.getElementById("tsRx_Need_Signing") != undefined && document.getElementById("tsRx_Change") != undefined)
            rxValues = document.getElementById("tsRefill").innerText + "$:$" + document.getElementById("tsRx_Pending").innerText + "$:$" + document.getElementById("tsRx_Need_Signing").innerText + "$:$" + document.getElementById("tsRx_Change").innerText;
    }
    sessionStorage.setItem("RxCount", rxValues);
}
function loadRcopia() {

    $.ajax({
        type: "POST",
        url: "frmRCopiaToolbar.aspx/LoadRCopiaNotification",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessRCopia,
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
//function LoadRcopiaCount(oWindow, args) {
//    loadRcopia();
//    oWindow.remove_close(LoadRcopiaCount);
//}
function openNonModal(fromname, height, width, inputargument) {
    var Argument = "";
    var PageName = fromname;
    if (inputargument != undefined) {
        for (var i = 0; i < inputargument.length; i++) {
            if (i != 0) {
                Argument = Argument + "&" + inputargument[i];
            }
            else {
                Argument = inputargument[i];
            }
        }
        if (inputargument.lenght != 0) {
            PageName = PageName + "?";
        }
    }


    var result = window.open(PageName + Argument, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes,titlebar=no,toolbar=no");
    if (result != null)
        result.moveTo(30, 150);

    if (result == undefined) { result = window.returnValue; }
    return result;
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function SelectedIndexChanged() {
    var text = $find('txtDocumentSubType')._text;
    if (text.indexOf($find('lstDynamic')._element.value) == -1) {
        if (text == "")
            $find('txtDocumentSubType').set_value($find('lstDynamic')._element.value);
        else
            $find('txtDocumentSubType').set_value(text + "," + $find('lstDynamic')._element.value);
    }
}
$(document).ready(function () {
    $('#dvpbdropdown').css('display', 'block');
    //pageload();
})

function pageload() {
    document.getElementById("btnSave").disabled = true;
    //document.addEventListener("DOMContentLoaded", function (event) {
    //    document.getElementById("btnSave").disabled = true;
    //});

}

function ClickClose() {
    if (Result != undefined) {
        if (false == Result.closed) {

            Result.close();
        }
    }
    if (document.getElementById('hdnTab').value == "true") {
        if (document.getElementById("hdnMessageType").value == "") {
            DisplayErrorMessage('1105001');
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            document.getElementById("btnSave").click();
            DisplayErrorMessage('115009');
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "false";
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            $(top.window.document).find('#btnCloseViewResult')[0].click();
            self.close();
        }
        else if (document.getElementById("hdnMessageType").value == "No") {
            document.getElementById("hdnMessageType").value = "";
            $(top.window.document).find('#btnCloseViewResult')[0].click();
            self.close();
        }
        else if (document.getElementById("hdnMessageType").value == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
        }
    }
    if (document.getElementById('btnSave') != null) {

        if (document.getElementById('btnSave').disabled == false) {
            if (document.getElementById("hdnMessageType").value == "") {
                DisplayErrorMessage('1105001');
            }
            else if (document.getElementById("hdnMessageType").value == "Yes") {
                document.getElementById("btnSave").click();
                DisplayErrorMessage('115009');
                document.getElementById('btnSave').disabled = true;
                document.getElementById(GetClientId("hdnMessageType")).value = "";
                $(top.window.document).find('#btnCloseViewResult')[0].click();
                self.close();
            }
            else if (document.getElementById("hdnMessageType").value == "No") {
                document.getElementById("hdnMessageType").value = "";
                $(top.window.document).find('#btnCloseViewResult')[0].click();
                self.close();
            }
            else if (document.getElementById("hdnMessageType").value == "Cancel") {
                document.getElementById("hdnMessageType").value = "";
            }
        }
        else {
            var win = GetRadWindow();
            if (win != null) {
                win.close();
            }
            else {
                $(top.window.document).find('#btnCloseViewResult')[0].click();
            }
        }
    }
    else if (document.getElementById('btnSave') == null) {
        var win = GetRadWindow();
        if (win != null) {
            win.close();
        }
        else {
            $(top.window.document).find('#btnCloseViewResult')[0].click();
        }
    }
    else {
        var win = GetRadWindow();
        if (win != null) {
            win.close();
        }
        else {
            $(top.window.document).find('#btnCloseViewResult')[0].click();
        }
    }
}
function End() {
    var win = GetRadWindow();
    if (win != null) {
        win.close();
    }
    else {
        $(top.window.document).find('#btnCloseViewResult')[0].click();
    }
    self.close();
    return false;
}

function openLink(urls) {
    var temp = urls.split(';');
    var IsLonic = 0;
    if (temp.length > 1) {
        for (var i = 0; i < temp.length ; i++) {
            if (temp[i] != "") {
                window.open("http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=" + temp[i] + "&informationRecipient.languageCode.c = en", "_blank");
            }
            else if (i != (temp.length - 1)) {
                window.open("http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=&informationRecipient.languageCode.c = en", "_blank");
            }

        }
    }
    else {
        window.open("http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=&informationRecipient.languageCode.c = en", "_blank");
    }

}
function txtProviderNotes_OnValueChanged(e) {
    //document.getElementById('btnSave').disabled = false;
    document.getElementById(GetClientId("btnSave")).disabled = false;
}

function txtMedicalAssistantNotes_OnValueChanged(sender, args) {
    //document.getElementById('btnSave').disabled = false;
    document.getElementById(GetClientId("btnSave")).disabled = false;
}
function EnableSave() {
    //document.getElementById('btnSave').disabled = false;
    document.getElementById(GetClientId("btnSave")).disabled = false;
}
function ClickMovetoma(sender, args) {
    //For Bug Id 56084-4.9.18
    //StartLoadingImage();
    //if (document.getElementById('DLC_txtDLC')!=null&&document.getElementById('DLC_txtDLC').disabled == false &&document.getElementById('cboMoveToMA')!=null&& document.getElementById('cboMoveToMA').value == "") {//For Bug Id: 74955 
    if (document.getElementById('cboMoveToMA') != null && document.getElementById('cboMoveToMA').value == "") {
        DisplayErrorMessage('115046');
        return false;
    }
    if (document.getElementById('DLC_txtDLC').disabled == false && document.getElementById('DLC_txtDLC').value == "") {
        //var Continue = DisplayErrorMessage('115060');
        //if (Continue != undefined && Continue == true) {
            StartLoadingImage();
            __doPostBack('btnMoveToMa', "true");
        //}
        //else {
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}
    }
    if (document.getElementById('txtMedicalAssistantNotes').disabled == false && document.getElementById('txtMedicalAssistantNotes').value == "") {


        //var Continue = DisplayErrorMessage('115060');
        //if (Continue != undefined && Continue == true) {
            StartLoadingImage();
            __doPostBack('btnMoveToMa', "true");
        //}
        //else {
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}
    }
    else {
        StartLoadingImage();//BugID:41027 -- move to next result
        __doPostBack('btnMoveToMa', "true");
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function ClickMovetonextProcess(sender, args) {
    //For Bug Id 56084-4.9.18
    if (document.getElementById('chkPhyName') != null && document.getElementById('chkPhyName').disabled == false && document.getElementById('chkPhyName').checked == false) {
        DisplayErrorMessage('115034');
        return false;
    }

    if (document.getElementById('DLC_txtDLC').disabled == false && document.getElementById('DLC_txtDLC').value == "") {
        //var Continue = DisplayErrorMessage('115060');
        //if (Continue != undefined && Continue == true) {
            StartLoadingImage();
            __doPostBack('btnMoveToNextProcess', "true");
        //}
        //else {
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}
    }
    if (document.getElementById('txtMedicalAssistantNotes').disabled == false && document.getElementById('txtMedicalAssistantNotes').value == "") {
        //var Continue = DisplayErrorMessage('115060');
        //if (Continue != undefined && Continue == true) {
            StartLoadingImage();
            __doPostBack('btnMoveToNextProcess', "true");
        //}
        //else {
        //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //    return false;
        //}

    }
    else {
        StartLoadingImage();
        __doPostBack('btnMoveToNextProcess', "true");
    }
    //End For BUg Id 56084-4.9.18
    //BugID:41027 -- move to next result

}

function btnSave_ClientClicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    __doPostBack('btnSave', "true");
    // return true;
}

function btnSave_ClientNodeClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}


function SaveViewResults() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    AutosaveDisable('false');
    DisplayErrorMessage('115009');
}
//BugID:47602

function onload() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function funEFax() {

    localStorage["datetime"] = "";
    var vdatetime = "";
    var vTabstrip = $($($($("#tabView").find('.rtsLevel1')).find('ul>li>a')));//.length;//[1].className
    if (vTabstrip != undefined > 0 && vTabstrip.length != 0) {
        for (var i = 0; i < vTabstrip.length; i++) {
            if (vTabstrip[i].className == "rtsLink rtsSelected") {
                localStorage["datetime"] = vTabstrip[i].innerText

                break;
            }
        }
    }

    else if (vTabstrip.length == 0) {
        vTabstrip = $($($($("#tvViewIndex").find('.rtLines')).find('ul>li>div')));

        if (vTabstrip != undefined > 0) {
            for (var i = 0; i < vTabstrip.length; i++) {
                if (vTabstrip[i].className == "rtBot rtSelected") {
                    localStorage["datetime"] = vTabstrip[i].innerText
                    break;
                }
            }
        }
    }

    localStorage.setItem("IsMenuEFax", "N");
    $(top.window.document).find("#TabFax").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabFax").css({ "z-index:": "5001" });
    $(top.window.document).find("#TabModalEFaxTitle")[0].textContent = "Efax";
    $(top.window.document).find("#TabmdldlgEFax")[0].style.width = "1050px";
    $(top.window.document).find("#TabmdldlgEFax")[0].style.height = "963px";
    $(top.window.document).find("#TabmdldlgEFax").css({ "margin-left": "150px" });
    var sPath = ""
    if (localStorage["Result"] != undefined && localStorage["Result"].split('&')[0] != undefined && localStorage["Result"].split('&')[0] != "") {
        if ($('#hdnFaxpath').val().split("$")[1] != undefined) {
            sPath = "frmEFax.aspx?Result=" + localStorage["Result"].split('&')[0] + "|" + $('#hdnFaxpath').val().split("$")[0] + "|" + $('#hdnFaxpath').val().split("$")[2];
        }
        else {

            sPath = "frmEFax.aspx?Result=" + $('#hdnFaxpath').val();
        }
    }
    else
        sPath = "frmEFax.aspx?Result=" + $('#hdnFaxpath').val();


    localStorage['FaxSubject'] = "";
    localStorage['FaxSubject'] = $('#hdnFaxSubject').val().replace("__", "_");
    $(top.window.document).find("#TabEFaxFrame")[0].style.height = "659px";
    $(top.window.document).find("#TabEFaxFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabFax").one("hidden.bs.modal", function (e) {
    });
}

function OpenPatientCommunication() {
    var obj = new Array();
    var ID = new Object();
    ID = document.getElementById(GetClientId("hdnHumanId")).value;
    if (ID == undefined || ID == "") {
        var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.add_close(OnClientPatientCommunication);
    }
    else {
        var Id = new Object();
        Id = document.getElementById(GetClientId("hdnHumanId")).value;
        PatientF_name = document.getElementById(GetClientId("hdnPatientfirstname")).value;
        PatientL_name = document.getElementById(GetClientId("hdnPatientlastname")).value;
        PatientM_name = document.getElementById(GetClientId("hdnPatientmiddlename")).value;
        Dob = document.getElementById(GetClientId("hdnDOB")).value.split(" ")[0];
        PatientType = document.getElementById(GetClientId("hdnPatientType")).value;
        var result = openModal("frmPatientCommunication.aspx?IsMYQ=N" + "&AccountNum=" + Id + "&PatientName=" + PatientF_name + "," + PatientL_name + " " + PatientM_name + "&PatientDOB=" + Dob + "&HumanType=" + PatientType, 810, 1050, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
    }
    return false;
}

function txtProviderNotes_OnKeyPress(sender, args) {
    document.getElementById(GetClientId("hdnSave")).value = "true";
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    if (document.getElementById('btnSave') != null && document.getElementById('btnSave') != undefined)
        document.getElementById('btnSave').enabled = true;

    if (document.getElementById(GetClientId("btnSave")) != null && document.getElementById(GetClientId("btnSave")) != undefined && document.getElementById(GetClientId("btnSave")).disabled != null && document.getElementById(GetClientId("btnSave")).disabled != undefined)
        document.getElementById(GetClientId("btnSave")).disabled = "false"
}

function onLoad() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}


function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

}


function OpenResultInterpretation() {
    //var obj = new Array();
    //obj.push("HumanText=" + document.getElementById('txtPatientInformation').value.split("#").join("%23"));
    //obj.push("FileText=" + document.getElementById('txtFileInformation').value.split("#").join("%23"));
    //obj.push("DocumentSubType=" + document.getElementById(GetClientId('hdnSubDocumentType')).value);
    ////obj.push("ResultMasterID=" + document.getElementById(GetClientId('ResultMasterID')).value);

    //$("#RadWindowWrapper_ctl00_ModalWindow").css({ "margin-top": "2%", "margin-left": "2%" });
    //var result = openModal("frmResultInterpretation.aspx", 635, 1151, obj, 'ctl00_ModalWindow');

    //var WindowName = $find('ctl00_ModalWindow');
    //WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close + Telerik.Web.UI.WindowBehaviors.Move);
    //WindowName.add_close(FillProviderNotes);
    //return false;
    var notes = '';
    var ProvNoteshistory = document.getElementById('DLC_txtDLC').value;
    if (ProvNoteshistory != '') {
        //Cap - 747
        //notes = ProvNoteshistory;
        notes = ProvNoteshistory.replaceAll("&", "$|$|$|$|").replaceAll("#", "!^!^!^!^").replaceAll("+","~|~|~|~|");
    }
    else {
        //notes = document.getElementById("txtProvNoteshistory").value;

        // Cap - 747
        //notes = document.getElementById("txtProvNoteshistory").attributes[5].value.replaceAll("<br/>", "")
        notes = document.getElementById("txtProvNoteshistory").attributes[5].value.replaceAll("<br/>", "").replaceAll("&", "$|$|$|$|").replaceAll("#", "!^!^!^!^").replaceAll("+", "~|~|~|~|");

    }
    //if (notes != '') {
    //    if (!notes.includes("Test Reviewed: ")) {
    //        if (!DisplayErrorMessage('115061')) {
    //            return false;
    //        }
    //        else {
    //            notes = '';
    //        }
    //    }
    //}

    var PatientInformation = '';
    if (document.getElementById('txtPatientInformation') != null && document.getElementById('txtPatientInformation') != undefined && document.getElementById('txtPatientInformation') != '')
        PatientInformation = document.getElementById('txtPatientInformation').value.split("#").join("%23");
    $(top.window.document).find("#txtResultInterpretationsInformation")[0].value = "";
    $(top.window.document).find("#TabResultInterpretations").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalResultInterpretationsTitle")[0].textContent = "Interpretation Notes";
    $(top.window.document).find("#TabmdldlgResultInterpretations")[0].style.width = "900px";
    $(top.window.document).find("#TabmdldlgResultInterpretations")[0].style.height = "875px";
    var sPath = "frmResultInterpretation.aspx?HumanText=" + PatientInformation + "&FileText=" + document.getElementById('txtFileInformation').value.split("#").join("%23") + "&DocumentSubType=" + document.getElementById(GetClientId('hdnSubDocumentType')).value + "&ProviderNotes=" + JSON.stringify(notes) + "&ProviderNotesHistory=" + JSON.stringify(document.getElementById("txtProvNoteshistory").value); //+ "&ResultMasterID=" + document.getElementById(GetClientId('ResultMasterID')).value;
    $(top.window.document).find("#TabResultInterpretationsFrame")[0].style.height = "635px";
    $(top.window.document).find("#TabResultInterpretationsFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabResultInterpretations").on('hide.bs.modal', function (e) {
        if ($(top.window.document).find("#txtResultInterpretationsInformation")[0].value != null &&
            $(top.window.document).find("#txtResultInterpretationsInformation")[0].value != undefined
            && $(top.window.document).find("#txtResultInterpretationsInformation")[0].value != "") {
            var n = JSON.parse($(top.window.document).find("#txtResultInterpretationsInformation")[0].value);
            document.getElementById(GetClientId("hdnResultInterpretations")).value = n;
            document.getElementById('DLC_txtDLC').value = n;
            var dlc = document.getElementById('DLC_txtDLC');
            if (dlc.value != '') {
                $("#DLC_txtDLC").addClass('nonEditabletxtbox');
                $("#DLC_pbDropdown").addClass('pbDropdownBackgrounddisable');
                dlc.disabled = true;
                dlc.setAttribute("style", "resize:none;width:400px;height:55px;overflow-x: hidden;");
            }
            else {
                dlc.disabled = false;
                $("#DLC_txtDLC").removeClass('nonEditabletxtbox');
                $("#DLC_pbDropdown").removeClass('pbDropdownBackgrounddisable');
                $("#DLC_txtDLC").addClass('Editabletxtbox');
                $("#DLC_pbDropdown").addClass('pbDropdownBackground');
            }
            EnableSave();
        }
        $(top.window.document).find("#TabResultInterpretations").modal({ backdrop: "", keyboard: false }, 'hide');
    });
    return false;
}

function FillProviderNotes() {
    var TemplateDetails = localStorage.getItem("InterpretationNotes");
    if (TemplateDetails != undefined && TemplateDetails != null)
        document.getElementById('DLC_txtDLC').value = TemplateDetails;
}

//function OpenResultInterpretation() {
//    var oWnd = GetRadWindow();
//    var childWindow = oWnd.BrowserWindow.radopen("frmResultInterpretation.aspx?HumanText=" + document.getElementById('txtPatientInformation').value.split("#").join("%23") + "&Screen=Appointments" + "&Title=Upload Documents" + "&CurrentTime=" + utc + "&ScreenMode=Bulk Scanning and Fax", "RadOnlineWindow");
//    setRadWindowProperties(childWindow, 880, 1200);


//    var obj = new Array();
//    obj.push("HumanText=" + document.getElementById('txtPatientInformation').value.split("#").join("%23"));
//    obj.push("FileText=" + document.getElementById('txtFileInformation').value.split("#").join("%23"));
//    obj.push("DocumentSubType=" + document.getElementById(GetClientId('hdnSubDocumentType')).value);

//    Result = openNonModal("frmResultInterpretation.aspx", 700, 1200, obj, 'ctl00_ModalWindow');
//    $('#resultLoading').css("display", "none");
//    if (Result == null)
//        return false;
//}

//function OpenResultInterpretation() {
//    var obj = new Array();
//    obj.push("HumanText=" + document.getElementById('txtPatientInformation').value.split("#").join("%23"));
//    obj.push("FileText=" + document.getElementById('txtFileInformation').value.split("#").join("%23"));
//    obj.push("DocumentSubType=" + document.getElementById(GetClientId('hdnSubDocumentType')).value);

//    Result = openNonModal("frmResultInterpretation.aspx", 700, 1200, obj, 'ctl00_ModalWindow');
//    $('#resultLoading').css("display", "none");
//    if (Result == null)
//        return false;
//}

function PrintInterpretation() {
    var notes = '';
    var ProvNoteshistory = document.getElementById('DLC_txtDLC').value;
    if (ProvNoteshistory != '') {
        notes = ProvNoteshistory;
    }
    else {
        // notes = document.getElementById("txtProvNoteshistory").attributes[5].value;//document.getElementById("txtProvNoteshistory").value;
        notes = document.getElementById("txtProvNoteshistory").attributes.InterpretationText.value;
    }
    if (!notes.includes("Test Reviewed: ")) {
        DisplayErrorMessage('115059');
        return false;
    }
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Print Interpretation Notes";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "685px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?&SI=" + document.getElementById("hdnFileName").value + "&Location=DYNAMIC&FaxSubject=''";
    return false;
}

function SetRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}


function DeleteIndexing() {
    if (confirm("The selected Document will be removed from the Patient chart. Please confirm by clicking on OK?")) {
        StartLoadingImage();
        document.getElementById("hdnDeleteIndexing").click();
        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        DisplayErrorMessage('114021');
        self.close();
        window.top.location.href = "frmPatientChart.aspx";

    }
    else {
        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        return false;
    }
}


function RefreshPatChart() {
    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
    self.close();
    //returnToParent(null);
    window.top.location.href = "frmPatientChart.aspx";
}

function DeleteFilesIndexing(filename) {
    if (confirm("The selected document will be removed from the Patient Chart. Please confirm by clicking on OK.")) {
        //onLoad();
        StartLoadingImage();
        var vScanIndexConversionID = document.getElementById('hdnfileindexid').value;
        $.ajax({
            type: "POST",
            async: true,
            url: "frmViewResult.aspx/DeleteIndexImage",
            data: '{ScanIndexConversionID: "' + vScanIndexConversionID + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d == "Success") {
                    StopLoadingImage();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    document.getElementById('summaryIndexdiv').style.display = "block";
                    document.getElementById('divViewAll').style.display = "none";
                    return false;
                }
                else {
                    StopLoadingImage();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    var ErrorNo = data.d;
                    DisplayErrorMessage(ErrorNo);
                    //return false;
                }
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
            }
        });
    }
    else {
        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        return false;
    }
}


