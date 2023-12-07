function showTime() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}

function dtpRecurToTime_SelectedDateChanged() {
    $find("btnSaveForRecurring").set_enabled(true);
    $find("btnCancelForNonRecur").set_enabled(true);

    document.getElementById("hdnSaveForDlc").value = "true";
    document.getElementById("hdnToFindSource").value = "ToEnable";

    return false;
}
function facilityChange() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
function ChecksAlternateWeeks(evt) {
    var AlternateWeeks = document.getElementById("chkAlternateWeeks");
    var AlternateMoths = document.getElementById("chkAlternateMonths");
    if (AlternateWeeks.checked)
        AlternateMoths.checked = false;
    EnableSave();
}
function ChecksAlternateMonths(evt) {
    var AlternateWeeks = document.getElementById("chkAlternateWeeks");
    var AlternateMoths = document.getElementById("chkAlternateMonths");
    if (AlternateMoths.checked)
        AlternateWeeks.checked = false;
    EnableSave();
}
function dtpNonRecurDate_SelectedDateChanged() {
    $find("btnSaveForNonRecur").set_enabled(true);
    $find("btnCancelForNonRecur").set_enabled(true);

    document.getElementById("hdnSaveForDlc").value = "true";
    document.getElementById("hdnToFindSource").value = "ToEnableNon";

    return false;
}

function ShowResume() {
    var ddlFacility = $find("ddlFacilityName");
    if (ddlFacility._text.indexOf("#") != -1) {
        var ddlFacilityText = ddlFacility._text.replace("#", "_");
    }
    else {
        var ddlFacilityText = ddlFacility._text;
    }
    if (document.getElementById('hdnForMedicalAssistant').value != "MEDICAL ASSISTANT") {
        var checkedValues = '';

        var elementRef = document.getElementById('chklstboxProvider');
        //CAP-1463 
        var checkBoxArray = elementRef?.getElementsByTagName('input');

        for (var i = 0; i < checkBoxArray.length; i++) {
            var checkBoxRef = checkBoxArray[i];

            if (checkBoxRef.checked == true) {
                var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');
                if (labelArray.length > 0) {
                    if (checkedValues.length > 0)
                        checkedValues += ',';

                    checkedValues += labelArray[i].innerHTML;
                    break;
                }
            }
        }

        var obj = new Array();
        obj.push("Facility_Name=" + ddlFacilityText);
        obj.push("Physician_Name=" + checkedValues);
        document.getElementById('hdnShowBlockDetaills').value = "Yes";
        var Result = openModal("frmBlockDaysDetails.aspx", 480, 1120, obj, "ModalWindow");
        var WindowName = $find('ModalWindow');
        WindowName.add_close(ShowBlockDaysClick)
        return false;
    }
    else {
        var obj = new Array();
        obj.push("Facility_Name=" + ddlFacilityText);
        obj.push("hdnForMedicalAssistant=" + "MEDICAL ASSISTANT");
        var Result = openModal("frmBlockDaysDetails.aspx", 460, 1120, obj, "ModalWindow");
        return false;
    }
}
function ShowBlockDaysClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != null) {
        document.getElementById('hdnIndex').value = Result.index;
        document.getElementById('hdnFromTime').value = Result.fromtime;
        document.getElementById('hdnToTime').value = Result.totime;
        document.getElementById('hdnBlockDayType').value = Result.blocktype;
        document.getElementById('hdnPhySelected').value = Result.phyname.trim();
        document.getElementById('hdnAlternateWeeks').value = Result.AlternateWeeks;
        document.getElementById('hdnAlternateMonths').value = Result.AlternateMonths;
    }
    if (Result != null) {
        if (Result.blocktype == "RECURSIVE") {
            document.getElementById('hdnBlockdaysId').value = Result.blockid;
            document.getElementById('hdnFromDate').value = Result.fromdate;
            document.getElementById('hdnToDate').value = Result.todate;
            document.getElementById('hdnDays').value = Result.selectedday;
            document.getElementById('hdnRecDescription').value = Result.description;
        }
        else if (Result.blocktype == "NON RECURSIVE") {
            document.getElementById('hdnNonRecBlockDaysId').value = Result.blockid;
            document.getElementById('hdnFromDate').value = Result.fromdate;
            document.getElementById('hdnNonRecurDescription').value = Result.description;
        }
        else {
            document.getElementById('hdnBlockdaysId').value = Result.blockid;
            document.getElementById(GetClientId('dtpRecurFromDate')).value = Result.fromdate;
            document.getElementById(GetClientId('dtpRecurToDate')).value = Result.todate;
            document.getElementById('hdnDays').value = Result.selectedday;
            document.getElementById('hdnRecDescription').value = Result.description;
        }
        $find("btnInvisibleForBlockDays").click(true);
    }


}

function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName; for (i = 0; i < count; i++)
    { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}

function ConfirmtoSave() {
    if (document.getElementById('<%=btnSaveForRecurring.ClientID%>').disabled == false || document.getElementById('<%=btnSaveForNonRecur.ClientID%>').disabled == false) {
        if (window.confirm("Do you want to save the Changes")) {
            document.getElementById('<%=hdnTabChange.ClientID%>').value = "True";
            return false;
        }
        else {
            document.getElementById('<%=hdnTabChange.ClientID%>').value = "False";
            return true;
        }
    }
}


function ClearAll(sender, args) {
    if (($find("btnSaveForRecurring").get_enabled() == true) && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Recurring Days")) {
        if ($find("btnSaveForRecurring").get_text() == "Save") {
            ClearAllFields();
        }
        else {
            CloseForBlockDays(sender, args);
        }
    }
    if (($find("btnSaveForNonRecur").get_enabled() == true) && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Non-Recurring Days")) {
        if ($find("btnSaveForNonRecur").get_text() == "Save") {
            ClearAllFields();
        }
        else {
            CloseForBlockDays(sender, args);
        }
    }
}
function btnSave(e) {
    $find("btnSaveForRecurring").set_enabled(true);
}
function BlockDaysValidation() {
    if (document.getElementById("ddlFacilityName").value.length == 0) {

        DisplayErrorMessage('110110');
        document.getElementById("ddlFacilityName").focus();
        return false;
    }

    if (document.getElementById("txtRecurringDescription").value.length == 0) {

        DisplayErrorMessage('110124');
        document.getElementById("txtRecurringDescription").focus();
        return false;
    }

    if (ValidationFromDateAndToDate("dtpRecurFromDate", "dtpRecurToDate", ">") == false) {

        DisplayErrorMessage('110125');
        document.getElementById("dtpRecurFromDate").focus();
        return false;
    }
}

function ValidationFromDateAndToDate(FromDate, ToDate, Check) {
    var splitEffDatedate = document.getElementById(FromDate).value;
    var splitTermDate = document.getElementById(TermDate).value;
    var EffDatedate = new Date();
    var TermDate = new Date();
    var m = getMonth(splitEffDatedate.split('-')[1]);
    EffDatedate.setFullYear(splitEffDatedate.split('-')[2], m, splitEffDatedate.split('-')[0]);
    var n = getMonth(splitTermDate.split('-')[1]);
    TermDate.setFullYear(splitTermDate.split('-')[2], n, splitTermDate.split('-')[0]);
    if (Check == ">") {
        if ((EffDatedate.getFullYear() > TermDate.getFullYear())) {
            return false;
        }
        else if (EffDatedate.getMonth() > TermDate.getMonth() && (EffDatedate.getFullYear() >= TermDate.getFullYear())) {
            return false;
        }
        else if (EffDatedate.getDate() > TermDate.getDate() && (EffDatedate.getMonth() >= TermDate.getMonth()) && (EffDatedate.getFullYear() >= TermDate.getFullYear())) {
            return false;
        }
        else {
            return true;
        }

    }

    if (Check == "==") {
        if ((EffDatedate.getFullYear() == TermDate.getFullYear())) {
            return false;
        }
        else if (EffDatedate.getMonth() == TermDate.getMonth() && (EffDatedate.getFullYear() == TermDate.getFullYear())) {
            return false;
        }
        else if (EffDatedate.getDate() == TermDate.getDate() && (EffDatedate.getMonth() == TermDate.getMonth()) && (EffDatedate.getFullYear() == TermDate.getFullYear())) {
            return false;
        }
        else {
            return true;
        }
    }
    ClearAll();
}

function getMonth(Month) {
    var month = new Array();
    switch (Month) {
        case "Jan":
            x = 0;
            break;
        case "Feb":
            x = 1;
            break;
        case "Mar":
            x = 2;
            break;
        case "Apr":
            x = 3;
            break;
        case "May":
            x = 4;
            break;
        case "Jun":
            x = 5;
            break;
        case "Jul":
            x = 6;
            break;
        case "Aug":
            x = 7;
            break;
        case "Sep":
            x = 8;
            break;
        case "Oct":
            x = 9;
            break;
        case "Nov":
            x = 10;
            break;
        case "Dec":
            x = 11;
            break;
    }
    return x;
}
function CloseForBlockDays(sender, args) {
    if (document.getElementById(GetClientId('hdnToFindSource')).value != "Tab") {
        if ((document.getElementById(GetClientId('btnSaveForRecurring')).disabled == false) || (document.getElementById(GetClientId('btnSaveForNonRecur')).disabled == false) || (document.getElementById(GetClientId('hdnToFindSource')).value == "Error Message")) {
            if (((document.getElementById(GetClientId('btnSaveForRecurring')).disabled == false) && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Recurring Days")) || ((document.getElementById(GetClientId('hdnToFindSource')).value == "Error Message") && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Recurring Days"))) {
                if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                    DisplayErrorMessage('110126');
                    document.getElementById(GetClientId('hdnToFindSource')).value = "Error Message";
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    __doPostBack('btnSaveForRecurring');
                    $find('btnSaveForRecurring').set_enabled(true);
                    document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
                    document.getElementById("hdnToFindSource").value = "ToEnable";
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    document.getElementById(GetClientId('hdnToFindSource')).value = "No";
                    self.close();
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
                    document.getElementById(GetClientId('hdnToFindSource')).value = "No";
                    $find('btnSaveForRecurring').set_enabled(true);
                }
            }
            if (((document.getElementById(GetClientId('btnSaveForNonRecur')).disabled == false) && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Non-Recurring Days")) || ((document.getElementById(GetClientId('hdnToFindSource')).value == "Error Message") && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Non-Recurring Days"))) {
                if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                    DisplayErrorMessage('9093040');
                    document.getElementById(GetClientId('hdnToFindSource')).value = "Error Message";
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    __doPostBack('btnSaveForNonRecur');
                    $find('btnSaveForNonRecur').set_enabled(true);
                    document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
                    document.getElementById("hdnToFindSource").value = "ToEnable";
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                    document.getElementById(GetClientId("hdnMessageType")).value = ""
                    document.getElementById(GetClientId('hdnToFindSource')).value = "No";
                    self.close();
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
                    document.getElementById(GetClientId('hdnToFindSource')).value = "No";
                    $find('btnSaveForNonRecur').set_enabled(true);
                    //CAP-1471
                    args?.set_cancel(true);
                }
            }
            if ((document.getElementById(GetClientId('btnSaveTOF')).disabled == false) && ($find('tabBlockDays').get_selectedTab().get_text() == "Block Type of Visit")) {
                if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                    DisplayErrorMessage('9093040');
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                    document.getElementById(GetClientId("btnSaveTOF")).click();
                    document.getElementById(GetClientId("btnSaveTOF")).disabled = false;
                    document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    self.close();
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
                    return false;
                }
            }
        }
        else {

            var o = new Object(); o = true; returnToParent(o);
        }
    }
}





function libraryBlockRecurring() {

    var PhysicianID = document.getElementById("hdnPhyID").value;
    var fieldName = "BLOCK DAYS DESCRIPTION";
    window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + fieldName + "&PhyID=" + PhysicianID, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:645px;scroll:yes;");
}
function libraryBlockNonRecurring() {
    var PhysicianID = document.getElementById("hdnPhyID").value;
    var fieldName = "BLOCK DAYS DESCRIPTION";
    window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + fieldName + "&PhyID=" + PhysicianID, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:645px;scroll:yes;");
}
function libraryTOF() {
    var PhysicianID = document.getElementById("hdnPhyID").value;
    var fieldName = "BLOCK DAYS DESCRIPTION";
    window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + fieldName + "&PhyID=" + PhysicianID, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:645px;scroll:yes;");
}
function TimeValidation(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
}
function EnableSave() {
    if ($find('tabBlockDays') != null) {
        var ActiveIndex = $find('tabBlockDays').get_selectedTab().get_text();
        if (ActiveIndex == "Block Recurring Days") {
            $find("btnSaveForRecurring").set_enabled(true);
            document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
        }
        else if (ActiveIndex == "Block Non-Recurring Days") {
            $find("btnSaveForNonRecur").set_enabled(true);
            document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
        }
        else if (ActiveIndex == "Block Type of Visit") {
            document.getElementById(GetClientId('btnSaveTOF')).disabled = false;
            document.getElementById(GetClientId('hdnSaveForDlc')).value = "true";
        }
    }
    showTime();
}
function validation() {
    var ActiveIndex = $find('tabBlockDays').get_selectedTab().get_text();
    if (document.getElementById("chklstboxProvider").length == 0) {

    }
    if (ActiveIndex == "Block Recurring Days") {
        if (document.getElementById("chklstboxProvider").length == 0) {

        }
    }
    else if (ActiveIndex == "Block Non-Recurring Days") {

    }
    else if (ActiveIndex == "Block Type of Visit") {

    }

}
function returnToParent(args) {
    var oArg = new Object();
    oArg.result = args;
    var oWnd = GetRadWindow();
    if (oWnd != null) {
        if (oArg.result) {
            oWnd.close(oArg.result);
        }
        else {
            oWnd.close(oArg.result);
        }
    }
    else {
        self.close();
    }
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function clearAllForTabandSave() {
    if ($find('tabBlockDays').get_selectedTab().get_text() == "Block Recurring Days") {
        $find('btnSaveForRecurring').set_enabled(false);
        document.getElementById("hdnToFindSource").value = "Not";
        var time = new Date();
        var hours = time.getHours();
        var minutes = time.getMinutes();
        minutes = ((minutes < 10) ? "0" : "") + minutes;
        var clock = hours + ":" + minutes; //  + ":" + seconds
        var time = new Date();
        var timePicker = $find("dtpRecurFromTime");
        timePicker.get_dateInput().set_value(hours + ':' + minutes);
        hours = hours + 1;
        var timePicker1 = $find("dtpRecurToTime");
        timePicker1.get_dateInput().set_value(hours + ':' + minutes);
        document.getElementById("chkMonday").checked = false;
        document.getElementById("chkTuesday").checked = false;
        document.getElementById("chkWednesday").checked = false;
        document.getElementById("chkThursday").checked = false;
        document.getElementById("chkFriday").checked = false;
        document.getElementById("chkSaturday").checked = false;
        document.getElementById("chkSunday").checked = false;
        document.getElementById("chkAlternateWeeks").checked = false;
        document.getElementById("chkAlternateMonths").checked = false;
        document.getElementById("chkRecurSelecttime").checked = true;
        document.getElementById("hdnBlockDayType").value = "";
        document.getElementById("hdnNonRecBlockDaysId").value = "";
        document.getElementById("hdnBlockdaysId").value = "";
        document.getElementById("hdnSaveForDlc").value = "false";
        var datepicker = $find("dtpRecurFromDate");
        var dat = datepicker.get_selectedDate();
        datepicker.set_selectedDate(dat);
        var datepicker = $find("dtpRecurToDate");
        datepicker.set_selectedDate(dat);
        document.getElementById("txtRecurringDescription_txtDLC").value = "";
    }
    if ($find('tabBlockDays').get_selectedTab().get_text() == "Block Non-Recurring Days") {
        $find('btnSaveForNonRecur').set_enabled(false);
        document.getElementById("hdnToFindSource").value = "Not";
        document.getElementById("chkNonRecurSelectTime").checked = false;
        document.getElementById("hdnBlockDayType").value = "";
        document.getElementById("hdnNonRecBlockDaysId").value = "";
        document.getElementById("hdnBlockdaysId").value = "";
        document.getElementById("hdnSaveForDlc").value = "false";
        var datepicker = $find("dtpNonRecurDate");
        var dat = datepicker.get_selectedDate();
        datepicker.set_selectedDate(dat);
        var time = new Date();
        var hours = time.getHours();
        var minutes = time.getMinutes();
        minutes = ((minutes < 10) ? "0" : "") + minutes;
        var clock = hours + ":" + minutes; //  + ":" + seconds
        var time = new Date();
        var timePicker = $find("dtpNonRecurFromTime");
        timePicker.get_dateInput().set_value(hours + ':' + minutes);
        hours = hours + 1;
        var timePicker1 = $find("dtpNonRecurToTime");
        timePicker1.get_dateInput().set_value(hours + ':' + minutes);
        document.getElementById("txtDescription_txtDLC").value = "";
    }
}
function ClearAllFields() {
    if (DisplayErrorMessage('180010') == true) {
        // clearAllForTabandSave();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $find("btnInvisibleclear").click(true);
    }
    else {
        if ($find('tabBlockDays').get_selectedTab().get_text() == "Block Recurring Days") {
            $find('btnSaveForRecurring').set_enabled(true);
        }
        if ($find('tabBlockDays').get_selectedTab().get_text() == "Block Non-Recurring Days") {
            $find('btnSaveForNonRecur').set_enabled(true);
        }
    }
}
function closeForYesClick() {
    self.close();
}
function BlockDaysTabChange(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (window.parent.document.getElementById('ctl00_hdnTabClick') != undefined && window.parent.document.getElementById('ctl00_hdnTabClick') != null)
        document.getElementById("hdnToFindSource").value = "";
    var TabClick = window.parent.document.getElementById('ctl00_hdnTabClick');
    if (TabClick.value == "Block Non-Recurring Days$#$third") {
        if (document.getElementById(GetClientId('btnSaveForRecurring')).disabled == false) {
            TabClick.value = "first";
        }
    }
    else if (TabClick.value == "Block Recurring Days$#$third") {
        if (document.getElementById(GetClientId('btnSaveForNonRecur')).disabled == false) {
            TabClick.value = args._tab._element.textContent + "$#$";
            args?.set_cancel(true);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('1100000', 'BlockDaysTabClick');
            return false;
        }
    }
    if (TabClick.value == "first") {
        if (args._tab._element.textContent == "Block Non-Recurring Days") {
            if (document.getElementById(GetClientId('btnSaveForRecurring')).disabled == false) {
                TabClick.value = args._tab._element.textContent + "$#$";
                args?.set_cancel(true);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('1100000', 'BlockDaysTabClick');
                return false;
            }
        }
        else {
            if (document.getElementById(GetClientId('btnSaveForNonRecur')).disabled == false) {
                TabClick.value = args._tab._element.textContent + "$#$";
                args?.set_cancel(true);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                DisplayErrorMessage('1100000', 'BlockDaysTabClick');
                return false;
            }
        }
    }
    else {
        var splitvalue = TabClick.value.split('$#$');
        var clicked_tab = splitvalue[0];
        var switchcase = splitvalue[1];
        if (switchcase == "second,true") {
            if (window.parent.document.getElementById('ctl00_hdnSaveButtonID') != undefined && window.parent.document.getElementById('ctl00_hdnSaveButtonID') != null)
                var hdnSaveButtonID = window.parent.document.getElementById('ctl00_hdnSaveButtonID');
            var IDs = hdnSaveButtonID.value.split(',');
            if (clicked_tab == "Block Recurring Days")
                IDs[0] = 'btnSaveForNonRecur';
            var save_button = $find(IDs[0]);
            if (save_button != undefined || save_button != null) {
                args?.set_cancel(true);
                TabClick.value = clicked_tab + "$#$third";
                document.getElementById("hdnToFindSource").value = "ToEnable";
                if (IDs[0] == 'btnSaveForNonRecur') {
                    document.getElementById("hdnToFindSource").value = "ToEnableNon";
                }
                save_button.click(true);
                return;
            }

        }
        else if (switchcase == "second,false") {
            clearAllForTabandSave();
        }
        else if (switchcase == "second,cancel") {
            args?.set_cancel(true);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
        TabClick.value = "first";
    }
}

function BlockDays_Load() {
    if (window.parent.document.getElementById('ctl00_hdnSaveButtonID') != null && window.parent.document.getElementById('ctl00_hdnSaveButtonID') != undefined)
        window.parent.document.getElementById('ctl00_hdnSaveButtonID').value = "btnSaveForRecurring,rdmpBlockDays";

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    $("[id*=pbDropdown]").addClass('pbDropdownBackground');



}
function GetRadNewWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    oWindow.close();
}
function ShowLoading() {

    document.getElementById('divLoading').style.display = "block";
}
function SaveClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function OpenBlockDetails() {
    var obj = new Array();
    var faclity = document.getElementById('hdnFacilityName').value;
    obj.push("Facility_Name=" + faclity);
    var physicanName = document.getElementById('hdnPhysician').value;
    obj.push("Physician_Name=" + physicanName);

    var Physician_ID = document.getElementById('hdnPhysicianID').value;
    obj.push("Physician_ID=" + Physician_ID);

    var GroupId = document.getElementById('hdnGroupID').value;
    obj.push("GroupId=" + GroupId);
    var UniqueBlockID = document.getElementById('hdnUniqueBLockID').value;
    obj.push("UniqueBlockID=" + UniqueBlockID);
    obj.push("Is_Edit_All_Occurrence=True");
    var Result = openModal("frmBlockDaysDetails.aspx", 450, 1100, obj, "ModalWindow");
    return false;
}


