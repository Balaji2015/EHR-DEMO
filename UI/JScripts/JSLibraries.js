function OpenPlanWindow() {


    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.openModal("frmPayerLibrary.aspx", 720, 1050, null, "ModalWindow");
            childWindow.add_close(refreshSelectPayer)
        }, 0);


    return false;
}

function refreshSelectPayer() {
    document.getElementById(GetClientId("btnSearch")).click();
}

function closeWindow() {
    var grid = document.getElementById("grdPayerInformation");
    if (grid != null) {
        var index = parseInt(document.getElementById("hdnSelectedIndex").value) + 1;
        row = grid.rows[index];
        if (row) {
            planid = row.cells[1].innerHTML;
            planname = row.cells[3].innerHTML;
            CarrierId = row.cells[10].innerHTML;
            CarrierName = row.cells[2].innerHTML;
            var Result = new Object();
            Result.PlanId = planid;
            Result.planName = planname;
            Result.CarrierId = CarrierId;
            Result.CarrierName = CarrierName;
            if (window.opener) {
                window.opener.returnValue = Result;
            }
            window.returnValue = Result;
        }
    }

    self.close(); return false;
}
function cancel() {
    if ($(top.window.document).find("#txtPatientInformation") != null) {
        $(top.window.document).find("#txtPatientInformation")[0].value = "";
        $(top.window.document).find("#btnFindPatientClose").click();
    }
    var Result = new Object();
    returnToParent(null);
}
function CancelForPatientCommunication(sender, args) {
    if ((document.getElementById(GetClientId("btnSaveForMenu")).disabled == false) || (document.getElementById(GetClientId("btnSaveForMyQ")).disabled == false)) {
        if (document.getElementById(GetClientId("hdnToFindScreen")).value == "Menu") {
            if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                DisplayErrorMessage('7580013');
                $find("btnCancelForMenu").set_autoPostBack(false);
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                __doPostBack('btnSaveForMenu');
                document.getElementById(GetClientId("btnSaveForMenu")).disabled = false;
                return false;
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                document.getElementById(GetClientId("hdnMessageType")).value = "";
                self.close();
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                document.getElementById(GetClientId("hdnMessageType")).value = "";
                $find("btnSaveForMenu").set_enabled(true);
                $find("btnCancelForMenu").set_autoPostBack(false);
            }
        }
        else if (document.getElementById(GetClientId("hdnToFindScreen")).value != "Menu") {
            if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                document.getElementById(GetClientId("hdnMessageType")).value == "Yes";
                __doPostBack('btnSaveForMyQ');
                document.getElementById(GetClientId("btnSaveForMyQ")).disabled = false;
                return false;
                //DisplayErrorMessage('9093040');
                //$find("btnCancelForMyQ").set_autoPostBack(false);
            }
            //else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
            //    __doPostBack('btnSaveForMyQ');
            //    document.getElementById(GetClientId("btnSaveForMyQ")).disabled = false;
            //    return false;
            //}
            //else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
            //    document.getElementById(GetClientId("hdnMessageType")).value = "";
            //    self.close();
            //}
            //else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
            //    document.getElementById(GetClientId("hdnMessageType")).value = "";
            //    $find("btnSaveForMyQ").set_enabled(true);
            //    $find("btnCancelForMyQ").set_autoPostBack(false);
            //}
        }
    }
    else {
        self.close();

    }
}
function OpenCarrierLibrary() {
    setTimeout(
       function () {
           var oWindow = GetRadWindow();
           var childWindow = oWindow.BrowserWindow.radopen("frmCarrierLibrary.aspx", "PayerModalWindow");
           setRadWindowProperties(childWindow, 450, 600);
           childWindow.add_close(refreshCarrier)
       }, 0);


    return false;
}
function refreshCarrier(oWindow, args) {
    document.getElementById(GetClientId("btnRefresh")).click();
}
function CloseCarrierWindow() {

    if (document.getElementById(GetClientId('btnAdd')).disabled == false) {
        if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
            DisplayErrorMessage('660014');
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
            document.getElementById('btnAdd').click();
            document.getElementById(GetClientId('btnAdd')).disabled = true;
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            self.close();
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            return false;
        }
    }
    else {
        returnToParent(null);
    }
}


function AutoSave() {
    var dt = new Date(); var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    var save = $find("btnSaveForMenu");
    save.set_enabled(true);
    var SavrForQ = $find("btnSaveForMyQ");
    SavrForQ.set_enabled(true);
    var Complete = $find("btnCompleteForMyQ");
    Complete.set_enabled(true);
    document.getElementById("hdnForSaveEnable").value = "ToEnable";
}
function Enable() {
    var send = $find("btnSendForMyQ");
    send.set_enabled(true);
    var saveMenu = $find("btnSaveForMenu");
    saveMenu.set_enabled(true);
    var saveMyQ = $find("btnSaveForMyQ");
    saveMyQ.set_enabled(true);
    var completeQ = $find("btnCompleteForMyQ");
    completeQ.set_enabled(true);
    document.getElementById("hdnForSaveandSendEnable").value = "ToEnable";
}

function closeReferelWindow() {

    var Result = JSON.parse(document.getElementById('txtProviderSearch').attributes['data-phy-details'].value);
    if (window.opener) {
        window.opener.returnValue = Result;
    }
    window.returnValue = Result;
    returnToParent(Result);
    self.close();
}
function CloseFindchargeline() {
    window.showModalDialog("frmCodingAndChargePosting.aspx?ScreenName=View Charge Posting", null, "center:yes;title:no;resizable:no;dialogHeight:700px;dialogWidth:1040px");
}
function CloseFindcharge() {
    window.showModalDialog("frmCodingAndChargePosting.aspx?ScreenName=Update Charge Posting", null, "center:yes;title:no;resizable:no;dialogHeight:700px;dialogWidth:1040px");

}

function OpenViewMessage() {

    if (document.getElementById("txtAccount").value == "") {
        DisplayErrorMessage('7580010');
        return false;
    }
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var svalue = document.getElementById("txtAccount").value;
            var encounterId = document.getElementById("hdnEncounterId").value;
            var childWindow = oWnd.BrowserWindow.radopen("frmViewMessage.aspx?AccountNum=" + svalue + "&EncounterId=" + encounterId, "ModalWndPatientTask");//No need to show the new view message screen here. Since this screen is used for FO.
            setRadWindowProperties(childWindow, 500, 1210);
            childWindow.add_close(openViewMessag);

        }, 0);

    return false;
}

function openViewMessag(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("txtAccount")).value = Result.HumanId;
    }

}
function OpenPatientCommunication() {
    var Result = openModal("frmPatientCommunication.aspx", 810, 1050, obj, "ModalWindow");
    var WindowName = $find('ModalWindow');
    WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);


}

function CloseFindPatient() {
    if (document.getElementById("hdnFromScreen").value == "Menu View Claims") {
        var obj = new Object();
        // obj.HumanID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
        if (document.getElementById("txtPatientSearch").value != null && document.getElementById("txtPatientSearch").value != undefined && document.getElementById("txtPatientSearch").value != "") {

            obj.HumanID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
        }
        else if (document.getElementById("txtPatientSearchQuick").value != null && document.getElementById("txtPatientSearchQuick").value != undefined && document.getElementById("txtPatientSearchQuick").value != "") {

            obj.HumanID = document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value;
        }

        window.dialogArgument = obj;
        var result = window.showModalDialog("frmViewClaims.aspx?AccNo=" + obj.HumanID, obj, "scroll:yes;center:yes;resizable:yes;dialogHeight:350px;dialogWidth:950px");
        if (result != null) {
            document.getElementById(GetClientId('txtAccNo')).value = result.HumanID;
            return true;
        }
        else {
            return false;
        }
    }

    if (document.getElementById("hdnselected").value == "Patient Communication") {
        return true;
    }
    else {

        //For Bug Id : 74044
        var DataHumanDetails = "";
        if (document.getElementById("txtPatientSearch").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != "") {
            DataHumanDetails = document.getElementById("txtPatientSearch").attributes["data-human-details"].value;
        }
        else if (document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != "") {
            DataHumanDetails = document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value;
        }
        // var result = JSON.parse(document.getElementById("txtPatientSearch").attributes["data-human-details"].value);
        var result = "";
        if (DataHumanDetails != "")
            result = DataHumanDetails;

        if (document.getElementById("hdnFromScreen").value == "Appointments" && row.cells[13].innerHTML == "DECEASED") {
            DisplayErrorMessage("140007");
            return false;
        }
        if (window.opener) { window.opener.returnValue = result; }
        window.returnValue = result;
        returnToParent(result);
        return false;
    }
}

function GetDate()
{ var dt = new Date(); var LocalDate = dt.toLocaleDateString(); document.getElementById("hdnLocalDate").value = LocalDate; }
function OpenQuickPatient() {
    var oWnd = GetRadWindow();
    if (oWnd != null) {
        setTimeout(
            function () {

                var childWindow = oWnd.BrowserWindow.radopen("frmQuickpatientcreate.aspx?EncounterID=" + 0 + "&humanID=" + 0 + "&EncStatus=" + "" + "&bShowPat=" + false + "&sScreenMode=FIND PATIENT" + "&sPatlastname=&sFirstName=&sExtAccNo=&DOB=&ParentScreen=" + document.getElementById("hdnFromScreen").value, "ModalWindow");
                setRadWindowProperties(childWindow, 360, 930);
                childWindow.add_close(QuickPatientClick);

            }, 0);
    }
    else {
        $(top.window.document).find("#txtQuickPatientInformation")[0].value = "";
        $(top.window.document).find("#TabQuickPatient").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalQuickPatientTitle")[0].textContent = "Quick Patient Create";
        $(top.window.document).find("#TabmdldlgQuickPatient")[0].style.width = "1165px";
        $(top.window.document).find("#TabmdldlgQuickPatient")[0].style.height = "360px";
        var sPath = ""
        sPath = "frmQuickpatientcreate.aspx?EncounterID=" + 0 + "&humanID=" + 0 + "&EncStatus=" + "" + "&bShowPat=" + false + "&sScreenMode=FIND PATIENT" + "&sPatlastname=&sFirstName=&sExtAccNo=&DOB=&ParentScreen=" + document.getElementById("hdnFromScreen").value;
        $(top.window.document).find("#TabQuickPatientFrame")[0].style.height = "360px";
        $(top.window.document).find("#TabQuickPatientFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabQuickPatient").on('hide.bs.modal', function (e) {

            $(top.window.document).find("#TabQuickPatient").modal({ backdrop: "", keyboard: false }, 'hide');
        });
    }

    return false;
}
function openDemographicswindow() {
    if (document.getElementById("hdnFromScreen").value == "Appointments" || document.getElementById("hdnFromScreen").value == "" || document.getElementById("hdnFromScreen").value == "PAYMENTPOSTING" || document.getElementById("hdnFromScreen").value == "Demographics") {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        // var Humanid = document.getElementById('txtPatientSearch').attributes["data-human-id"].value;
        //For Bug Id : 74044
        var DataHumanID = "";
        if (document.getElementById("txtPatientSearch").attributes["data-human-id"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "" && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "0") {
            DataHumanID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
        }
        else if (document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != null && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != undefined && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != "" && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "0") {
            DataHumanID = document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value;
        }

        var Humanid = "0";
        if (DataHumanID != "" && DataHumanID != "0")
            Humanid = DataHumanID

        var Result = window.showModalDialog("frmPatientDemographics.aspx?HumanId=" + Humanid, null, "center:yes;resizable:yes;dialogHeight:1300px;dialogWidth:1140px");
        return false;
    }
    else { return false; }
}
function OpenDemoForAdd() {
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            RemoveSessions();
            var childWindow = oWnd.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + 0 + "&DisableFindPat=TRUE&Functionality=ADDPATIENT", "ModalWindow");
            setRadWindowProperties(childWindow, 1230, 1130);
            childWindow.add_close(QuickPatientClickAddPatient);

        }, 0);
    return false;
}

function OpenRereralPhysician() {
    var oWnd = GetRadWindow();
    var oManager = oWnd.get_windowManager();
    var childWindow = oManager.BrowserWindow.radopen("frmFindReferralPhysician.aspx", "ModalWindow");
    SetRadWindowProperties(childWindow, 256, 930);

    childWindow.add_close(RereralPhysicianClose)
    {
        function RereralPhysicianClose(oWindow, args) {
            var Result = args.get_argument();
            if (Result != null) {
                document.getElementById("txtDoctorName").value = Result.sPhyName;
                document.getElementById("txtDoctorNo").value = Result.ulPhyId;
            }
        }
    }
    return false;
}
function OpenFindPatient() {

    setTimeout(
        function () {

            var oWnd = GetRadWindow();
            var svalue = document.getElementById("txtAccount").value;
            var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx?AccountNum=" + svalue, "ModalWndPatientTask");
            setRadWindowProperties(childWindow, 251, 1200);
            childWindow.add_close(FindPatientClick)
            {
                function FindPatientClick(oWindow, args) {
                    var Result = args.get_argument();
                    if (Result) {
                        document.getElementById(GetClientId("hdnAccNo")).value = Result.HumanId;
                        document.getElementById("btnGetAccNo").click();
                    }

                }
            }
        }, 0);
}
function OpenFindPatientFrmViewMessage() {

    setTimeout(
        function () {

            var oWnd = GetRadWindow();
            var svalue = document.getElementById("txtAccount").value;
            var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx?AccountNum=" + svalue, "ModalWndViewMessage");
            setRadWindowProperties(childWindow, 251, 1200);
            childWindow.add_close(FindPatientClick)
            {
                function FindPatientClick(oWindow, args) {
                    var Result = args.get_argument();
                    if (Result) {
                        document.getElementById(GetClientId("hdnAccNo")).value = Result.HumanId;
                        document.getElementById("btnGetAccNo").click();
                    }

                }
            }
        }, 0);
}
function refresh(oWindow, args) {
    document.getElementById(GetClientId("btnGetAccNo")).click();
}

function OpenPop(objevent) {

    if (event.keyCode == 121) {

        document.onhelp = function () { return (false); }
        window.onhelp = function () { return (false); }

        var result = window.showModalDialog("frmAllDiagnosis.aspx", null, "center:yes;title:no;resizable:yes;dialogHeight:700px;dialogWidth:500px");

    }
}

function radioMe(e) {
    if (!e) e = window.event;
    var sender = e.target || e.srcElement;

    if (sender.nodeName != 'INPUT') return;
    var checker = sender;
    var chkBox = document.getElementById("chklstAllprocedure");
    var chks = chkBox.getElementsByTagName('INPUT');
    for (i = 0; i < chks.length; i++) {
        if (chks[i] != checker)
            chks[i].checked = false;

    }
}

function checkAll(objRef) {
    var GridView = objRef.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked) {
                inputList[i].checked = true;
            }
            else {
                inputList[i].checked = false;
            }
        }
    }
}
function Check_Click(objRef) {
    var row = objRef.parentNode.parentNode;
    var GridView = row.parentNode;
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        var headerCheckBox = inputList[0];
        var checked = true;
        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
            if (!inputList[i].checked) {
                checked = false;
                break;
            }
        }
    }
    headerCheckBox.checked = checked;

}

function OpenAddUpdate() {

    var svalue = "MESSAGE DESCRIPTION";
    var Result = window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + svalue + "&PhyID=" + document.getElementById("hdnPhysicianId").value, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:650px;scroll:yes;");
    if (Result == null)
        return false;
    return true;
}
function OpenAddUpdateForMessageNotes() {

    var svalue = "MESSAGE NOTES";
    var Result = window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + svalue + "&PhyID=" + document.getElementById("hdnPhysicianId").value, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:650px;scroll:yes;");
    if (Result == null)
        return false;
    return true;
}
function AddMessage() {


    if (document.getElementById("txtAccount").value == "") {
        DisplayErrorMessage('7580010');
        return false;
    }
    setTimeout(
     function () {
         var obj = new Array();
         var oWnd = GetRadWindow();
         var svalue = document.getElementById("txtAccount").value;
         obj.push("AccountNum=" + svalue);
         var childWindow = oWnd.BrowserWindow.radopen("frmPatientCommunication.aspx?AccountNum=" + svalue, "ModalWndPatientTask");
         setRadWindowProperties(childWindow, 810, 1050);
            //CAP-1442
            childWindow.remove_close(RefreshScreen);
         childWindow.add_close(RefreshScreen);
     }, 0);

    return false;
}
function RefreshScreen(oWindow, args) {

    document.getElementById("hdnAccNo").value = document.getElementById("txtAccount").value;
    document.getElementById("btnGetAccNo").click();
}
function OpenPatienTask() {

    var humanId = document.getElementById("txtAccount").value;
    var dob = document.getElementById("txtDOB").value;
    var status = document.getElementById("txtPatientStatus").value;
    var patientname = document.getElementById("txtPatientName").value;

    var obj = new Array();
    obj.push("patientdob=" + dob);
    obj.push("AccountNum=" + humanId);
    obj.push("status=" + status);
    obj.push("patientname=" + patientname);
    setTimeout(
     function () {
         var oWnd = GetRadWindow();
         var childWindow = oWnd.BrowserWindow.radopen("frmViewPatientTask.aspx?patientdob=" + dob + "&AccountNum=" + humanId + "&status=" + status + "&patientname=" + patientname, "ModalWndViewMessage");
         setRadWindowProperties(childWindow, 930, 1080);

     }, 0);

}
function OpenAddMessage() {
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var humanId = document.getElementById("txtAccount").value;

            var childWindow = oWnd.BrowserWindow.radopen("frmPatientCommunication.aspx?AccountNum=" + humanId, "ModalWndViewMessage");
            setRadWindowProperties(childWindow, 810, 1050);
            //CAP-1442
            childWindow.remove_close(RefreshScreen);
            childWindow.add_close(RefreshScreen);
        }, 0);

    return false;

}

function CancelLibrary() {
    var PhysicianID = document.getElementById("hdnPhysicianId").value;
    var fieldName = "CANCEL REASON CODE";
    window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + fieldName + "&PhyID=" + PhysicianID, null, "center:yes;resizable:yes;dialogHeight:400px;dialogWidth:650px;scroll:yes;");
}

function openFindPhysician(obj) {
    var Result = window.showModalDialog("frmFindReferralPhysician.aspx", null, "center:yes;resizable:no;dialogHeight:256px;dialogWidth:930px");
    if (Result != null) {
        if (obj.name == "ImgLibSelectProvider") {
            document.getElementById("txtSelectProvider").value = Result.sPhyName;
            document.getElementById("chkAllPhysician").checked = false;
        }
        else if (obj.name == "LibAddUpdateSelectProvider") {
            document.getElementById("txtAddUpdateSelectProvider").value = Result.sPhyName;
            document.getElementById("chkAll").checked = false;
        }

        document.getElementById("hdnPhysicianID").value = Result.ulPhyId;

    }
    return false;

}

function CheckboxValidation(obj) {
    if (obj.innerHTML.indexOf("chkAllPhysician") != -1) {
        if (document.getElementById("chkAllPhysician").checked == true) {
            document.getElementById("txtSelectProvider").value = "ALL";
        }
        else {
            document.getElementById("txtSelectProvider").value = "";
        }
    }
    else if (obj.innerHTML.indexOf("chkAll") != -1) {
        if (document.getElementById("chkAll").checked == true) {
            document.getElementById("txtAddUpdateSelectProvider").value = "ALL";
        }
        else {
            document.getElementById("txtAddUpdateSelectProvider").value = "";
        }
    }
    return false;
}

function AddOrUpdateRules() {
    if (document.getElementById("hdnAddUpdate").value != "0") {

        if (window.confirm("Rules Already Exits for Selected Provider!. Do you want to continue?")) {
            document.getElementById("btnAddUpdate").click();
            return true;
        }
        else {
            return false;
        }
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
        return false;


    return true;
}

function ClearAllFiels() {
    if (window.confirm("Are you sure you want to clear all without saving?")) {
        document.getElementById("txtAddUpdateSelectProvider").value = "";
        document.getElementById("chkAll").checked = false;
        document.getElementById("txtMultipleFactor").value = "";
        return true;
    }
    else {
        return false;
    }
}

function confirmMessage() {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;

    if (window.confirm("Are you sure you want to delete?")) {
        return true;
    }
    else {
        return false;
    }

}
function Validation() {
    if (document.getElementById("txtMultipleFactor").value == "") {

        DisplayErrorMessage('110076');
        document.getElementById("txtMultipleFactor").focus();
        return false;
    }
}
function GetRulsValidation() {
    if (document.getElementById("txtSelectProvider").value == "") {

        DisplayErrorMessage('110077');
        document.getElementById("ImgLibSelectProvider").focus();
        return false;
    }
}

function AllowNumbers(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function LettersWithSpaceOnly(evt) {
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
    ((evt.which) ? evt.which : 0));
    if (charCode > 32 && (charCode < 65 || charCode > 90) &&
        (charCode < 97 || charCode > 122)) {
        return false;
    }
    return true;
}

function returnToParent(args) {
    var oArg = new Object();
    oArg.result = args;
    var oWnd = GetRadWindow();
    if (oWnd != null && oArg != null) {
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

function CloseFindPatientRadGrid() {

    if (document.getElementById("hdnFromScreen").value == "Menu View Claims") {

        //For Bug Id : 74044
        var DataHumanID = "";
        if (document.getElementById("txtPatientSearch").attributes["data-human-id"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "" && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "0") {
            DataHumanID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
        }
        else if (document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != null && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != undefined && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != "" && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "0") {
            DataHumanID = document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value;
        }

        var obj = new Object();
        //obj.HumanID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
        if (DataHumanID != "" && DataHumanID != "0")
            obj.HumanID = DataHumanID;
        window.dialogArgument = obj;
        var result = window.showModalDialog("frmViewClaims.aspx?AccNo=" + obj.HumanID, obj, "scroll:yes;center:yes;resizable:yes;dialogHeight:350px;dialogWidth:950px");
        if (result != null) {
            document.getElementById(GetClientId('txtAccNo')).value = result.HumanID;
            return true;
        }
        else {
            return false;
        }
    }

    if (document.getElementById("hdnselected").value == "Patient Communication") {
        return true;
    }
    else {
        //For Bug Id : 74044
        var DataHumanDetails = "";
        if (document.getElementById("txtPatientSearch").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != "") {
            DataHumanDetails = document.getElementById("txtPatientSearch").attributes["data-human-details"].value;
        }
        else if (document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != "") {
            DataHumanDetails = document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value;
        }


        //if (document.getElementById("txtPatientSearch").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != "") 
        if (DataHumanDetails != "") {
            // var result = JSON.parse(document.getElementById("txtPatientSearch").attributes["data-human-details"].value);
            var result = JSON.parse(DataHumanDetails);
            result.IsFindPatient = "TRUE";
            CreateAuditLogEntry();
            var HumanID = result.HumanId;
            if (document.getElementById("hdnFromScreen").value == "Appointments" && result.Status.toUpperCase() == "DECEASED") {
                DisplayErrorMessage("140007");
                return false;
            }

            if (window.opener) { window.opener.returnValue = result; }
            window.returnValue = result;

            $(top.window.document).find("#txtPatientInformation")[0].value = JSON.stringify(result);
            $(top.window.document).find("#btnFindPatientClose").click();
            returnToParent(result);
            return true;
        }
        else {
            self.close();
        }
    }
}

function openDemographicsRadGrid() {
    var IsQuickorSlow;
    //For Bug Id : 74044
    var DataHumanID = "";
    if (document.getElementById("txtPatientSearch").attributes["data-human-id"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "" && document.getElementById("txtPatientSearch").attributes["data-human-id"].value != "0") {
        DataHumanID = document.getElementById("txtPatientSearch").attributes["data-human-id"].value;
        IsQuickorSlow = "txtPatientSearch";
    }
    else if (document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != null && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != undefined && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != "" && document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value != "0") {
        DataHumanID = document.getElementById("txtPatientSearchQuick").attributes["data-human-id"].value;
        IsQuickorSlow = "txtPatientSearchQuick";
    }

    var Humanid = "0";
    if (DataHumanID != "" && DataHumanID != "0")
        Humanid = DataHumanID;
    //var Humanid = document.getElementById("txtPatientSearch").attributes['data-human-id'].value;
    RemoveSessions();
    setTimeout(
       function () {

           var oWnd = GetRadWindow();
           { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
           var childWindow = oWnd.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + Humanid, 1230, 1130, "ModalWindow");
           //AuditLog Entry when opening Patient (MyQ) Modify/View Patient
           CreateAuditLogEntryForTransactions("ACCESS", "Human", Humanid);//BugID:49685
           setRadWindowProperties(childWindow, 1230, 1130);

           childWindow.add_close(FindPatientClick)
           {
               function FindPatientClick(oWindow, args) {
                   var WSData = {
                       //HumanID: document.getElementById("txtPatientSearch").attributes['data-human-id'].value
                       HumanID: Humanid
                   }
                   $.ajax({
                       type: "POST",
                       contentType: "application/json;RemoveSessions charset=utf-8",
                       url: "./frmFindPatient.aspx/GetHumanDetails",
                       data: JSON.stringify(WSData),
                       dataType: "json",
                       async: true,
                       success: function (data) {
                           var SelectedPatient = JSON.parse(data.d);
                           // $("#txtPatientSearch").val(SelectedPatient.DisplayString).attr("data-human-details", JSON.stringify(SelectedPatient.HumanDetails));
                           if (IsQuickorSlow != undefined && IsQuickorSlow == "txtPatientSearch") {

                               $("#txtPatientSearch").val(SelectedPatient.DisplayString).attr("data-human-details", JSON.stringify(SelectedPatient.HumanDetails));
                           }
                           else if (IsQuickorSlow != undefined && IsQuickorSlow == "txtPatientSearchQuick") {

                               $("#txtPatientSearchQuick").val(SelectedPatient.DisplayString).attr("data-human-details", JSON.stringify(SelectedPatient.HumanDetails));
                           }
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
           }
       }, 0);

    return false;
}

function grdPayerInformation_OnRowClick(sender, args) {
    document.getElementById('hdnSelectedIndex').value = args._itemIndexHierarchical;
    document.getElementById("btnOk").disabled = false;
}

function closeWindowRadGrid() {
    var grid = $find("grdPayerInformation");
    var MasterTable = grid.get_masterTableView();

    if (grid != null) {
        var index = parseInt(document.getElementById("hdnSelectedIndex").value);
        row = MasterTable.get_dataItems()[index];
        if (row) {
            var Result = new Object();
            Result.CarrierId = MasterTable.getCellByColumnUniqueName(row, "CarrierId").innerHTML;
            Result.CarrierName = MasterTable.getCellByColumnUniqueName(row, "CarrierName").innerHTML;
            Result.PlanId = MasterTable.getCellByColumnUniqueName(row, "Plan#").innerHTML;
            Result.planName = MasterTable.getCellByColumnUniqueName(row, "InsPlanName").innerHTML;
            if (window.opener) {
                window.opener.returnValue = Result;
            }
            window.returnValue = Result;
            returnToParent(Result);
        }
    }

    return false;
}

function grdPayerInformation_OnRowDblClick(sender, args) {
    document.getElementById('hdnSelectedIndex').value = args._itemIndexHierarchical;
    closeWindowRadGrid();

}

function QuickPatientClickAddPatient(oWindow, args) {

    var result = args.get_argument();
    oWindow.remove_close(QuickPatientClickAddPatient);
    if (result) {
        var Result = new Object();
        Result.HumanId = result.Human_id;
        Result.PatientName = result.PatientName;
        Result.PatientDOB = result.PatientDOB;
        Result.Encounter_Provider_ID = result.Encounter_Provider_ID;
        Result.Cell_Phone = result.Cell_Phone;
        Result.Home_Phone = result.Home_Phone;
        Result.HumanType = result.PatientType;
        Result.IsNewPatient = result.IsNewPatient;
        Result.IsQuickPatient = result.IsQuickPatient;
        Result.PatientGender = result.PatientGender;
        returnToParent(Result);
    }
}

function QuickPatientClick(oWindow, args) {

    var result = args.get_argument();
    oWindow.remove_close(QuickPatientClick);
    if (result) {
        var Result = new Object();
        Result.HumanId = result.Human_id;
        Result.PatientName = result.PatientName;
        Result.PatientDOB = result.PatientDOB;
        Result.Encounter_Provider_ID = result.Encounter_Provider_ID;
        Result.Cell_Phone = result.Cell_Phone;
        Result.Home_Phone = result.Home_Phone;
        Result.HumanType = result.HumanType;
        Result.IsNewPatient = result.IsNewPatient;
        Result.IsQuickPatient = result.IsQuickPatient;
        Result.PatientGender = result.PatientGender;
        returnToParent(Result);
    }
}

function closeReferelWindowRadGrid() {

    var Result = JSON.parse(document.getElementById('txtProviderSearch').attributes['data-phy-details'].value);
    if (window.opener) {
        window.opener.returnValue = Result;
    }
    window.returnValue = Result;
    returnToParent(Result);
    return false;
}

function confirmMessage() {
    if (window.confirm("Are you sure you want to delete?")) {
        return true;
    }
    else {
        return false;
    }

}
function ConfirmClearAll() {
    var sCheck = window.confirm("Do You Want to Clear All with out saving?");
    if (sCheck == true) {
        return true;
    }
    else {
        return false;
    }
}

function RereralPhysicianClickCarrierLibrary(oWindow, args) {


    document.getElementById(GetClientId("btnRefresh")).click();


}

function NumberOnly(txtbox) {
    var e = event || evt;
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;

}

function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

}

function OnValueChanged(sender, args) {
    var ControlId = sender.get_id().split('_')[0];
    var value = document.getElementById('hdnSelectedDate').value;
    if (value == "") {
        value = $find(ControlId).get_selectedDate().format('dd-MMM-yyyy');
    }
    var monthAndYear = value.split("-");
    if (monthAndYear[1] != undefined) {
        switch (monthAndYear[1].toUpperCase()) {
            case "JAN":
                x = 1;
                break;
            case "FEB":
                x = 1;
                break;
            case "MAR":
                x = 1;
                break;
            case "APR":
                x = 1;
                break;
            case "MAY":
                x = 1;
                break;
            case "JUN":
                x = 1;
                break;
            case "JUL":
                x = 1;
                break;
            case "AUG":
                x = 1;
                break;
            case "SEP":
                x = 1;
                break;
            case "OCT":
                x = 1;
                break;
            case "NOV":
                x = 1;
                break;
            case "DEC":
                x = 1;
                break;
            case monthAndYear[1]:
                x = 0;
                break;
        }

        if (x == 0) {
            alert("Invalid Date");
            $find(ControlId).set_selectedDate(null);
            $find(ControlId).get_dateInput().focus();
        }
    }
    if (monthAndYear[0] == undefined || monthAndYear[1] == undefined || monthAndYear[2] == undefined) {
        alert("Invalid Date");
        document.getElementById('hdnSelectedDate').value = "True";
        $find(ControlId).set_selectedDate(null);
        $find(ControlId).get_dateInput().focus();
        return false;
    }
}

function OnValueChanging(sender, args) {
    var ControlId = sender.get_id().split('_')[0];
    document.getElementById('hdnSelectedDate').value = $find(ControlId).get_dateInput()._element.value;
}

function dateInput_OnError(sender, args) {
    var ControlId = sender.get_id().split('_')[0];
    alert("Invalid Date");
    $find(ControlId).set_selectedDate(null);
    $find(ControlId).get_dateInput().focus();
}

function msktxtDOB_OnValueChanged(sender, args) {
    var EnteredDateLength = parseInt(args._newValue.replace("-", "").replace("-", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").length);
    if (EnteredDateLength != 9 && EnteredDateLength > 0) {
        alert("Please Enter the DOB Fully.")
        $find(sender.get_id().split('_')[0]).clear();
        $find(sender.get_id().split('_')[0]).focus(true);
        return false;
    }
    if (EnteredDateLength == 9) {
        validatedate($find(sender.get_id().split('_')[0])._value, sender.get_id().split('_')[0]);

    }

}

function validatedate(inputText, ControlId) {
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;
    if (inputText.match(FormatDDMMMYYYY)) {
        document.getElementById(GetClientId('hdnDateValidation')).value = 'true';
        var DateMonthYear = inputText.split('-');
        lopera2 = DateMonthYear.length;
        var DateInput = parseInt(DateMonthYear[0]);
        var Year = parseInt(DateMonthYear[2]);
        var Month = "";
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
        if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {

            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;

            if (Month == 1 || Month > 2) {
                if (DateInput > ListofDays[Month - 1]) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    document.getElementById(GetClientId('hdnDateValidation')).value = 'false';
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
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    document.getElementById(GetClientId('hdnDateValidation')).value = 'false';
                    return false;
                }
                if ((lyear == true) && (DateInput > 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    document.getElementById(GetClientId('hdnDateValidation')).value = 'false';
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();
            if (Year > CurrentYear) {
                alert("DOB cannot be future date. Please Enter the Valid Year.");
                $find(GetClientId(ControlId)).clear();
                $find(GetClientId(ControlId)).focus(true);
                document.getElementById(GetClientId('hdnDateValidation')).value = 'false';
                return false;
            }
        }

        else {
            alert('Invalid date format!');
            $find(GetClientId(ControlId)).clear();
            $find(GetClientId(ControlId)).focus(true);
            document.getElementById(GetClientId('hdnDateValidation')).value = 'false';
            return false;
        }
    }
    else {
        document.getElementById(GetClientId('hdnDateValidation')).value = 'false';
        return false;
    }
}

function AssignUTCTime() {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var utcnew = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utcnew;
    return true;
}

function showTimeCancel() {

    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}
function CloseWindow() {

    if (window.confirm("Are you sure you want to close?")) {
        if (window.opener) {
            window.opener.returnValue = null;
        }
        window.returnValue = null;
        returnToParent(null);

        return false;
    }
    else {
        return false;
    }
}

function CancelFromSelectPayer() {
    var is_all_text_boxes_empty = true;
    var pnlcontrol = document.getElementById('pnlSearchParameter');
    var allcontrols = pnlcontrol.getElementsByTagName('*');
    for (var i = 0; i < allcontrols.length; i++) {
        if (allcontrols[i].type == 'text') {
            if (allcontrols[i].value != "")
                is_all_text_boxes_empty = false;
        }
    }

    if (is_all_text_boxes_empty == false) {
        if (window.confirm("Are you sure you want to close?")) {
            if (window.opener) {
                window.opener.returnValue = null;
            }
            window.returnValue = null;
            returnToParent(null);

            return false;
        }
        else {
            return false;
        }
    }
    else
        self.close();
}


function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function confirmMessage() {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;

    if (window.confirm("Are you sure you want to delete?")) {
        return true;
    }
    else {
        return false;
    }

}

function ConfirmClearAll() {
    if (window.confirm("Do you want to clear all without saving?")) {
        return true;
    }
    else {
        return false;
    }
}

function RefreshMessageGrid(oWindow, args) {
    document.getElementById("btnSearch").click();
}
function LoadGrid(oWindow, args) {
    document.getElementById("btnSearch").click();
}
function grdLevel_OnRowClick(sender, args) {

    var grdLevel = $find('grdLevel');
    var MasterTable = grdLevel.get_masterTableView();

    document.getElementById('hdnSelectedIndex').value = args._itemIndexHierarchical;
    var index = parseInt(document.getElementById('hdnSelectedIndex').value);
    row = MasterTable.get_dataItems()[index];
    if (row)
        var obj = new Object();
    obj.ChargeheaderId = MasterTable.getCellByColumnUniqueName(row, "Chrg Header id").innerHTML;
    obj.ChargeLineItem = MasterTable.getCellByColumnUniqueName(row, "Chrg Line ID").innerHTML;
    window.dialogArgument = obj;
    document.getElementById('hdnChargeHeader').value = obj.ChargeheaderId;
    document.getElementById('hdnChargeLineId').value = obj.ChargeLineItem;
    document.getElementById('txtMessageNotes').value = null;
    document.getElementById('ddlMessageDescription').value = null;
    document.getElementById(GetClientId("btnLoadGrid")).click();
}
function RestrictSpecial(e) {
    var charInp = window.event.keyCode;
    if (charInp == 44 || charInp == 47 || charInp == 92) {
        return false;
    }
    else {
        document.getElementById(GetClientId("btnAdd")).disabled = false;
    }
}
function CloseCarrier() {
    var grid = document.getElementById("grdCarrierLibrary");
    if (grid != null) {
        var index = parseInt(document.getElementById("hdnSelectedIndex").value) + 1;
        row = grid.rows[index];
        if (row) {
            Carrierid = row.cells[2].innerHTML;
            CarrierName = row.cells[3].innerHTML;

            var Result = new Object();
            Result.Carrierid = Carrierid;
            Result.CarrierName = CarrierName;

            if (window.opener) {
                window.opener.returnValue = Result;
            }
            window.returnValue = Result;
        }
    }

    self.close(); return false;
}

function closeForPatientCommunication() {
    DisplayErrorMessage('7580006');
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    oWindow.close();
}
function EnableAddForPayer() {
    document.getElementById(GetClientId('btnAdd')).disabled = false;
}
function EnableAddInKeypress(e) {
    var charInp = window.event.keyCode;
    if (charInp >= 48 && charInp <= 57) {
        document.getElementById(GetClientId('btnAdd')).disabled = false;
    }
}
function FindPatientLoad() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function showTimeCancelForCancelAppointment() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}

var UI_Time_Start;
var UI_Time_Stop;
var intPatientlen = -1;
var arrPatient = [];
function PatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtPatientSearch = document.getElementById("txtPatientSearch");

    var WSData = {
        HumanID: ui.item.value,
        FullDetails: ui.item.label
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/GetHumanDetails",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtPatientSearch = document.getElementById('txtPatientSearch');
            txtPatientSearch.value = SelectedPatient.DisplayString;
            txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
            document.getElementById('btnModifiyPatient').disabled = false;
            document.getElementById('btnOk').disabled = false;

            document.getElementById('btnPrintFaceSheet').disabled = false;

            //$('#txtPatientSearchQuick').val('');
            //$("#txtPatientSearchQuick").attr({ "data-human-id": "0", "data-human-details": "" });


        }
    });
    txtPatientSearch.value = ui.item.label;
    txtPatientSearch.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    document.getElementById('hdnHumanID').value = ui.item.value;
    return false;
}



function PatientSelectedQuick(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtPatientSearch = document.getElementById("txtPatientSearchQuick");
    var WSData = {
        HumanID: ui.item.value,
        FullDetails: ui.item.label
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/GetHumanDetails",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtPatientSearch = document.getElementById('txtPatientSearchQuick');
            txtPatientSearch.value = SelectedPatient.DisplayString;
            txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
            document.getElementById('btnModifiyPatient').disabled = false;
            document.getElementById('btnOk').disabled = false;
            document.getElementById('btnPrintFaceSheet').disabled = false;
            //$('#txtPatientSearch').val('');
            //$("#txtPatientSearch").attr({ "data-human-id": "0", "data-human-details": "" });
        }
    });
    txtPatientSearchQuick.value = ui.item.label;
    txtPatientSearchQuick.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    document.getElementById('hdnHumanID').value = ui.item.value;
    return false;
}


function TriggerAutocomplete() {
    intPatientlen = 0;
    //$("#txtPatientSearch").autocomplete("search");//For Bug ID : 74213
    if (document.getElementById("txtPatientSearch").value != null && document.getElementById("txtPatientSearch").value != undefined && document.getElementById("txtPatientSearch").value != "") {

        $("#txtPatientSearch").autocomplete("search");
    }
    else if (document.getElementById("txtPatientSearchQuick").value != null && document.getElementById("txtPatientSearchQuick").value != undefined && document.getElementById("txtPatientSearchQuick").value != "") {

        $("#txtPatientSearchQuick").autocomplete("search");
    }
}

function CreateAuditLogEntry() {

    //For Bug Id : 74044
    var DataHumanDetails = "";
    if (document.getElementById("txtPatientSearch").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearch").attributes["data-human-details"].value != "") {
        DataHumanDetails = document.getElementById("txtPatientSearch").attributes["data-human-details"].value;
    }
    else if (document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != null && document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != undefined && document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value != "") {
        DataHumanDetails = document.getElementById("txtPatientSearchQuick").attributes["data-human-details"].value;
    }

    var WSData = {
        //HumanID: (JSON.parse(document.getElementById('txtPatientSearch').attributes['data-human-details'].value)).HumanId,
        HumanID: (JSON.parse(DataHumanDetails)).HumanId,
        startTime: (new Date()).toUTCString().split('G')[0]
    }
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/CreateAuditLogEntry",
        data: JSON.stringify(WSData),
        dataType: "json",
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

function LogTimeString(time_string) {
    UI_Time_Stop = new Date();

    var WS_Time = parseFloat(time_string.split(';')[0].split(':')[1].replace('s', ''));
    var DB_Time = parseFloat(time_string.split(';')[1].split(':')[1].replace('s', ''));
    var UI_Time = ((UI_Time_Stop.getTime() - UI_Time_Start.getTime()) / 1000) - WS_Time - DB_Time;
    console.log(time_string + " UI_Time :" + UI_Time + "s; Total_Time :" + (WS_Time + DB_Time + UI_Time).toString() + "s;");
    if (document.getElementById("hdnFromScreen").value != "" && document.getElementById("hdnFromScreen").value == "Indexing") {
        document.getElementById('btnAddpatient').disabled = true;
        document.getElementById('btnModifiyPatient').disabled = true;
        document.getElementById('btnPrintFaceSheet').disabled = true;
    }
    else
        document.getElementById('btnAddpatient').disabled = false;

    document.getElementById('btnQuickPatientCreate').disabled = false;
}
function FaceSheetClick() {
    //CAP-1231
    if (document.getElementById('hdnDataHumanDetails')?.value != null && document.getElementById('hdnDataHumanDetails').value != undefined && document.getElementById('hdnDataHumanDetails')?.value != undefined) {
        document.getElementById('hdnDataHumanDetails').value = document.getElementById("txtPatientSearch").attributes["data-human-details"].value;
    }

    if (window?.parent?.document?.body != undefined && window?.parent?.document?.body != null) {
            $(window.parent.document.body).css({ overflow: "hidden" });
    }

    document.getElementById('btnface').click();
}
$(document).ready(function () {

    var curleft = curtop = 0;
    var current_element = document.getElementById('txtPatientSearch');
    if (current_element == null) {
        current_element = document.getElementById('txtProviderSearch');
        curtop = 5;
    }


    window.setTimeout(function () {
        $('#txtPatientSearch').focus();
    }, 50);

    //window.setTimeout(function () {
    //    $('#txtPatientSearchQuick').focus();
    //}, 50);

    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    if (document.getElementById('btnAddpatient') != null && document.getElementById('btnAddpatient')!=undefined)
        document.getElementById('btnAddpatient').disabled = true;




    $("#imgClearPatientText").css({
        "position": "absolute",
        "right": "390px",        
        "top": (curtop + 10).toString() + "px",
        "cursor": "pointer",
        "width": "10px",
        "height": "10px"
    }).on("click", function () {
        document.getElementById('btnQuickPatientCreate').disabled = true;
        document.getElementById('btnAddpatient').disabled = true;
        document.getElementById('btnModifiyPatient').disabled = true;
        document.getElementById('btnPrintFaceSheet').disabled = true;
        document.getElementById('btnOk').disabled = true;
        $('#txtPatientSearch').val('').focus();
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });

    //For Quick Search
    var curleftQuick = curtopQuick = 0;
    var current_elementQuick = document.getElementById('txtPatientSearchQuick');
    if (current_elementQuick == null) {
        current_elementQuick = document.getElementById('txtProviderSearch');
        curtopQuick = 5;
    }

    if (current_elementQuick && current_elementQuick.offsetParent) {
        do {
            curleftQuick += current_elementQuick.offsetLeft;
            curtopQuick += current_elementQuick.offsetTop;
        } while (current_elementQuick = current_elementQuick.offsetParent);
    }
    $("#imgClearPatientTextQuick").css({
        "position": "absolute",
        "right": "15px",
        "top": (curtop + 10).toString() + "px",
        "cursor": "pointer",
        "width": "10px",
        "height": "10px"
    }).on("click", function () {
        document.getElementById('btnQuickPatientCreate').disabled = true;
        document.getElementById('btnAddpatient').disabled = true;
        document.getElementById('btnModifiyPatient').disabled = true;
        document.getElementById('btnPrintFaceSheet').disabled = true;
        document.getElementById('btnOk').disabled = true;
        $('#txtPatientSearchQuick').val('').focus();
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    });

    if (window.location.search.indexOf('Scan=Yes') > -1)
        $("#trAddOrModifyPatient").css("display", "none");
    if (document.URL.indexOf('frmOrdersPatientBar.aspx') < 0) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $(window.parent.document).find("[id='resultLoading']").hide();
    }


    //Current Search 
    if ($("#txtPatientSearch").length) {
        $("#txtPatientSearch").autocomplete({
            source: function (request, response) {
                if ($("#txtPatientSearch").val().trim().length > 2) {
                    if (intPatientlen == 0) {
                        UI_Time_Start = new Date();
                        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                        this.element.on("keydown", PreventTyping);
                        arrPatient = [];
                        var strkeyWords = $("#txtPatientSearch").val().split(' ');
                        var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                        var account_status = document.getElementById('chkIncludeInactive').checked ? "INACTIVE" : "ACTIVE";
                        var patient_status = document.getElementById("chkIncludeDeceased").checked ? "DECEASED" : "ALIVE";
                        var patient_type = document.getElementById("rdbRegular").checked ? "REGULAR" : (document.getElementById("rdbWC").checked ? "WC" : (document.getElementById("rdbAuto").checked ? "AUTO" : "ALL"));
                        var WSData = {
                            text_searched: strkeyWords[0],
                            account_status: account_status,
                            patient_status: patient_status,
                            human_type: patient_type
                        };

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "./frmFindPatient.aspx/GetPatientDetailsByTokens",
                            data: JSON.stringify(WSData),
                            dataType: "json",
                            success: function (data) {
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                $("#txtPatientSearch").off("keydown", PreventTyping);
                                var jsonData = $.parseJSON(data.d);
                                if (jsonData.Error != undefined) {
                                    alert(jsonData.Error);
                                    return;
                                }
                                if (jsonData.Time_Taken != undefined)
                                    LogTimeString(jsonData.Time_Taken);
                                if (jsonData.Result != undefined) {
                                    var no_matches = [];
                                    no_matches.push(jsonData.Result);
                                    response($.map(no_matches, function (item) {
                                        return {
                                            label: item,
                                            val: "0"
                                        }
                                    }));
                                }
                                else {
                                    var results;
                                    if (bMoreThanOneKeyword)
                                        results = Filter(jsonData.Matching_Result, request.term);
                                    else
                                        results = jsonData.Matching_Result;

                                    arrPatient = jsonData.Matching_Result;
                                    response($.map(results, function (item) {
                                        return {
                                            label: item.label,
                                            val: JSON.stringify(item.value),
                                            value: item.value.HumanId
                                        }
                                    }));
                                }
                            },
                            error: function OnError(xhr) {
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                if (xhr.status == 999)
                                    //CAP-502
                                    //window.location = xhr.statusText;
                                    window.location = "/frmSessionExpired.aspx";
                                else {
                                    //CAP-792
                                    if (isValidJSON(xhr.responseText)) {
                                        var log = JSON.parse(xhr.responseText);
                                        console.log(log);
                                        alert("USER MESSAGE:\n" +
                                            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                            "Message: " + log.Message);
                                    }
                                    else {
                                        alert("USER MESSAGE:\n" +
                                            ". Cannot process request. Please Login again and retry.");
                                    }
                                }
                            }

                        });
                    }
                    else if (intPatientlen != -1) {

                        var results = Filter(arrPatient, request.term);
                        response($.map(results, function (item) {
                            return {
                                label: item.label,
                                val: JSON.stringify(item.value),
                                value: item.value.HumanId
                            }
                        }));
                    }
                }
            },
            minlength: 0,
            multiple: true,
            mustMatch: false,
            select: PatientSelected,
            open: function () {
                // $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearch').width());
                $('.ui-autocomplete.ui-menu.ui-widget').width("1050px");
                

                $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
                $('#txtPatientSearch').focus();
            },
            focus: function () { return false; }
        }).on("paste", function (e) {
            intPatientlen = -1;
            arrPatient = [];
            $(".ui-autocomplete").hide();
        }).on("input", function (e) {
            $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
            if ($("#txtPatientSearch").val().charAt(e.currentTarget.value.length - 1) == " ") {
                if (e.currentTarget.value.split(" ").length > 2)
                    intPatientlen = intPatientlen + 1;
                else
                    intPatientlen = 0;
            }
            else {
                if ($("#txtPatientSearch").val().length != 0 && intPatientlen != -1) {
                    intPatientlen = intPatientlen + 1;
                }

                if ($("#txtPatientSearch").val().length == 0 || $("#txtPatientSearch").val().indexOf(" ") == -1) {
                    intPatientlen = -1;
                    arrPatient = [];
                    $(".ui-autocomplete").hide();
                }
            }
        }).on("click", function (e) {
            $('#txtPatientSearchQuick').val('');
            $("#txtPatientSearchQuick").attr({ "data-human-id": "0", "data-human-details": "" });
        }).on("focus", function (e) {
            $('#txtPatientSearchQuick').val('');
            $("#txtPatientSearchQuick").attr({ "data-human-id": "0", "data-human-details": "" });
        })

        $("#txtPatientSearch").data("ui-autocomplete")._renderItem = function (ul, item) {
            if (item.label != "No matches found.") {
                var HumanDetails = $.parseJSON(item.val);
                var list_item = $("<li>")
                  .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                  .append(item.label)
                  .appendTo(ul);
                if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                    list_item.addClass("inactive");
                if (HumanDetails.Status.toUpperCase() == "DECEASED")
                    list_item.addClass("deceased");
                return list_item;
            }
            else
                return $("<li>")
                  .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                  .addClass("disabled")
                  .append(item.label)
                  .appendTo(ul).on("click", function (e) {
                      e.preventDefault();
                      e.stopImmediatePropagation();
                  });
        };
    }

    //Quick Search
    if ($("#txtPatientSearchQuick").length) {
        $("#txtPatientSearchQuick").autocomplete({
            source: function (request, response) {
                if ($("#txtPatientSearchQuick").val().trim().length > 6) {

                    var PatientDOBYear = $("#txtPatientSearchQuick").val().trim().substring(0, 4);
                    // var LastNamelength = $("#txtPatientSearchQuick").val().trim().length;
                    var PatientlastName = $("#txtPatientSearchQuick").val().trim().substring(4, 7);
                    var numbers = /^[0-9]+$/;
                    if (intPatientlen == 0) {
                        //if (PatientDOBYear.match(numbers) && PatientlastName.match("^[a-zA-Z]*$")) {
                        if (PatientDOBYear.match(numbers) &&(PatientlastName.match("^[a-zA-Z'!@#\$%\^\&*\)\(+=._-]*$") != null && PatientlastName.match("^[a-zA-Z'!@#\$%\^\&*\)\(+=._-]*$") != false)) {
                            UI_Time_Start = new Date();
                            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                            this.element.on("keydown", PreventTyping);
                            arrPatient = [];
                            var strkeyWords = $("#txtPatientSearchQuick").val().split(' ');
                            var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                            var account_status = document.getElementById('chkIncludeInactive').checked ? "INACTIVE" : "ACTIVE";
                            var patient_status = document.getElementById("chkIncludeDeceased").checked ? "DECEASED" : "ALIVE";
                            var patient_type = document.getElementById("rdbRegular").checked ? "REGULAR" : (document.getElementById("rdbWC").checked ? "WC" : (document.getElementById("rdbAuto").checked ? "AUTO" : "ALL"));

                            // if (PatientDOBYear.match(numbers)) {
                            var WSData = {
                                text_searched: strkeyWords[0],
                                account_status: account_status,
                                patient_status: patient_status,
                                human_type: patient_type
                            };

                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "./frmFindPatient.aspx/GetPatientDetailsByTokens",
                                data: JSON.stringify(WSData),
                                dataType: "json",
                                success: function (data) {
                                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                    $("#txtPatientSearchQuick").off("keydown", PreventTyping);
                                    var jsonData = $.parseJSON(data.d);
                                    if (jsonData.Error != undefined) {
                                        alert(jsonData.Error);
                                        return;
                                    }
                                    if (jsonData.Time_Taken != undefined)
                                        LogTimeString(jsonData.Time_Taken);
                                    if (jsonData.Result != undefined) {
                                        var no_matches = [];
                                        no_matches.push(jsonData.Result);
                                        response($.map(no_matches, function (item) {
                                            return {
                                                label: item,
                                                val: "0"
                                            }
                                        }));
                                    }
                                    else {
                                        var results;
                                        if (bMoreThanOneKeyword)
                                            results = Filter(jsonData.Matching_Result, request.term);
                                        else
                                            results = jsonData.Matching_Result;

                                        arrPatient = jsonData.Matching_Result;
                                        response($.map(results, function (item) {
                                            return {
                                                label: item.label,
                                                val: JSON.stringify(item.value),
                                                value: item.value.HumanId
                                            }
                                        }));
                                    }
                                },
                                error: function OnError(xhr) {
                                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                    if (xhr.status == 999)
                                        //CAP-502
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
                            //}
                            //else {
                            //    //Show Alert search format was wrong
                            //    var no_matches = [];
                            //    no_matches.push("Please Enter 3 Character of Last Name and Year of DOB.");
                            //    response($.map(no_matches, function (item) {
                            //        return {
                            //            label: item,
                            //            val: "0"
                            //        }
                            //    }));
                            //}
                        }
                        //else {
                        //    $('#txtPatientSearchQuick').focus();
                        //    DisplayErrorMessage('1011189');
                        //}
                    }
                    else if (intPatientlen != -1) {

                        var results = Filter(arrPatient, request.term);
                        response($.map(results, function (item) {
                            return {
                                label: item.label,
                                val: JSON.stringify(item.value),
                                value: item.value.HumanId
                            }
                        }));
                    }
                }

            },
            minlength: 0,
            multiple: true,
            mustMatch: false,
            select: PatientSelectedQuick,
            open: function () {
                // $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearchQuick').width());
                $('.ui-autocomplete.ui-menu.ui-widget').width("1050px");
                $('.ui-autocomplete.ui-menu.ui-widget').css("left", "117px");
                $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
                $('#txtPatientSearchQuick').focus();
            },
            focus: function () { return false; }
        }).on("paste", function (e) {
            intPatientlen = -1;
            arrPatient = [];
            $(".ui-autocomplete").hide();
        }).on("input", function (e) {
            $("#txtPatientSearchQuick").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
            if ($("#txtPatientSearchQuick").val().charAt(e.currentTarget.value.length - 1) == " ") {
                if (e.currentTarget.value.split(" ").length > 6)
                    intPatientlen = intPatientlen + 1;
                else
                    intPatientlen = 0;
            }
            else {
                if ($("#txtPatientSearchQuick").val().length != 0 && intPatientlen != -1) {
                    intPatientlen = intPatientlen + 1;
                }

                if ($("#txtPatientSearchQuick").val().length == 0 || $("#txtPatientSearchQuick").val().indexOf(" ") == -1) {
                    intPatientlen = -1;
                    arrPatient = [];
                    $(".ui-autocomplete").hide();
                }
            }
        }).on("click", function (e) {
            $('#txtPatientSearch').val('');
            $("#txtPatientSearch").attr({ "data-human-id": "0", "data-human-details": "" });
        }).on("focus", function (e) {
            $('#txtPatientSearch').val('');
            $("#txtPatientSearch").attr({ "data-human-id": "0", "data-human-details": "" });
        })

        $("#txtPatientSearchQuick").data("ui-autocomplete")._renderItem = function (ul, item) {
            if (item.label != "No matches found.")// && item.label != "Please Enter 3 Character of Last Name and Year of DOB.")
            {
                var HumanDetails = $.parseJSON(item.val);
                var list_item = $("<li>")
                  .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                  .append(item.label)
                  .appendTo(ul);
                if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                    list_item.addClass("inactive");
                if (HumanDetails.Status.toUpperCase() == "DECEASED")
                    list_item.addClass("deceased");
                return list_item;
            }
                //else if (item.label == "Please Enter 3 Character of Last Name and Year of DOB.") {
                //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                //    return $("<li>")
                //     .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                //     .addClass("disabled")
                //     .append(item.label)
                //     .appendTo(ul).on("click", function (e) {
                //         e.preventDefault();
                //         e.stopImmediatePropagation();
                //     });
                //}
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return $("<li>")
                  .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                  .addClass("disabled")
                  .append(item.label)
                  .appendTo(ul).on("click", function (e) {
                      e.preventDefault();
                      e.stopImmediatePropagation();
                  });
            }
        };
    }
});
function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}
function Filter(array, terms) {
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 1 && arrayOfTerms[1].trim() != "") {
        var first_resultant = array;
        var resultant;
        for (var i = 1; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {
                return item.label.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
            });
            first_resultant = resultant;
        }
        return first_resultant;
    }
    else {
        return array;
    }
}
//Jira CAP-579
function FilterWithdelimiter(array, terms, checkdelimiter, SearchIndex) {
    var arrayindex = parseInt(SearchIndex);
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 0 && checkdelimiter != "") {
        var first_resultant = array;
        var resultant;

        for (var i = 0; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {

                if (item != undefined && checkdelimiter != "") {
                    return item.split(checkdelimiter)[arrayindex].toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
                }

            });
            first_resultant = resultant;
        }

        return first_resultant;
    }
    else {
        return array;
    }
}
//Jira CAP-579 - End
function RemoveSessions() {

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/RemoveSessions",
        dataType: "json",
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
function warningmethod() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}

function Reasoncode() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function PrintPDF(sFaxSubject) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: "static", keyboard: false }, 'show');

    //CAP-3689
    if ($(top.window.document).find("#PrintPDFModalTitle")[0] != undefined && $(top.window.document).find("#PrintPDFModalTitle")[0] != null) {
        $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Print FaceSheet";
    }

    //CAP-1578
    $(top.window.document).find("#PrintPDFModal")[0].style.overflow = "scroll";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('SelectedItem').value + "&Location=DYNAMIC&FaxSubject=" + sFaxSubject;
    document.getElementById('btnAddpatient').disabled = false;
    document.getElementById('btnQuickPatientCreate').disabled = false;
    document.getElementById('btnModifiyPatient').disabled = false;
    document.getElementById('btnPrintFaceSheet').disabled = false;
    document.getElementById('btnOk').disabled = false;
    // txtPatientSearch.attributes['data-human-id'].value = document.getElementById('hdnHumanID').value;//For Bug Id 62392

    //For Bug Id : 74044
    //CAP-1231
    //document.getElementById("txtPatientSearch").value = "";
    //document.getElementById("txtPatientSearchQuick").value = "";

    if (document.getElementById("txtPatientSearch").value != null && document.getElementById("txtPatientSearch").value != undefined && document.getElementById("txtPatientSearch").value != "") {

        txtPatientSearch.attributes['data-human-id'].value = document.getElementById('hdnHumanID').value;
    }
    else if (document.getElementById("txtPatientSearchQuick").value != null && document.getElementById("txtPatientSearchQuick").value != undefined && document.getElementById("txtPatientSearchQuick").value != "") {

        txtPatientSearchQuick.attributes['data-human-id'].value = document.getElementById('hdnHumanID').value;
    }
    //CAP-1231
    if (document.getElementById('hdnDataHumanDetails').value != null && document.getElementById('hdnDataHumanDetails').value != undefined && document.getElementById('hdnDataHumanDetails').value != "") {
        document.getElementById("txtPatientSearch").attributes["data-human-details"].value = document.getElementById('hdnDataHumanDetails').value;
    }
}