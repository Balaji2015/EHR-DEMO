function AutoSave(btn)
{ document.getElementById("btnSave").disabled = false; document.getElementById("hdnSaveFlag").value = true; }
function SelectPlanClickForSelf()
{ document.getElementById("btnSelectPlanForSelf").click(); }
function Open() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    setTimeout(
       function () {
           var oWnd = GetRadWindow();
           var childWindow = oWnd.BrowserWindow.radopen("frmSelectPayer.aspx", "AddInsuredModalWindow");
           SetRadWindowProperties(childWindow, 480, 880);
           childWindow.add_close(SelectPlanClick);
           childWindow.remove_close(RereralPhysicianClick);
           childWindow.remove_close(SelectPatientClick);
           childWindow.remove_close(AddNewPatientClick);
           childWindow.remove_close(EligibilityVerification);

       }, 0);
    return false;
}

function OpenRereralPhysician() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var result = openModal("frmFindReferralPhysician.aspx", 256, 930, null, "AddInsuredModalWindow");
    var WindowName = $find('AddInsuredModalWindow');
    WindowName.add_close(RereralPhysicianClick);
    WindowName.remove_close(SelectPatientClick);
    WindowName.remove_close(AddNewPatientClick);
    WindowName.remove_close(SelectPlanClick);
    WindowName.remove_close(EligibilityVerification);
}
function SaveCloseWindow() {
    DisplayErrorMessage('420018');
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var Result = new Object();
    Result.PcpName = document.getElementById("txtPCP").value;
    Result.InsCarrierName = document.getElementById("txtInsCarrierName").value;
    Result.InsPlanName = document.getElementById("txtInsPlanName").value;
    Result.InsPlanID = document.getElementById("txtInsPlanID").value;
    Result.PCPID = document.getElementById("txtPCPNPI").value;
    if (window.opener) {
        window.opener.returnValue = Result;
    }
    window.returnValue = Result;
    openPatInsurancewindow();
}
function SelectInsurance(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        returnToParent(Result);
    }
}
function openPatInsurancewindow() {
    HumanId = document.getElementById(GetClientId("txtPolicyHolderAccountNo")).value;
    if (HumanId) {
        var obj = new Array();
        obj.push("HumanId=" + HumanId);
        obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
        obj.push("CurrentProcess=" + document.getElementById(GetClientId("hdnCurrentProcess")).value);

        setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var oManager = oWnd.get_windowManager();
            var childWindow = oManager.BrowserWindow.radopen("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + HumanId + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&CurrentProcess=" + document.getElementById(GetClientId("hdnCurrentProcess")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
            SetRadWindowProperties(childWindow, 650, 1160);
            childWindow.add_close(SelectInsurance);
        }, 0);
    }
}
function OpenFindPatient() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var obj = new Array();
    obj.push("ScreenName=Demographics");
    setTimeout(
      function () {
          var oWnd = GetRadWindow();
          var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx?ScreenName=Demographics", "AddInsuredModalWindow");
          SetRadWindowProperties(childWindow, 255, 1170);
          childWindow.add_close(SelectPatientClick);
          childWindow.remove_close(AddNewPatientClick);
          childWindow.remove_close(SelectPlanClick);
          childWindow.remove_close(RereralPhysicianClick);
          childWindow.remove_close(EligibilityVerification);

      }, 0);
    return false;

}
function openDemographicswindow() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById("ddlPatientRelation").value.length == 0) {
        DisplayErrorMessage('420025');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("ddlPatientRelation").focus();
        return false;
    }
    var obj = new Array();
    obj.push("HumanId=" + 0);
    obj.push("bInsurance=" + true);
    obj.push("DisableFindPat=TRUE");
    setTimeout(
function () {
    var oWnd = GetRadWindow();
    var childWindow = oWnd.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + 0 + "&bInsurance=" + true + "&DisableFindPat=TRUE", "AddInsuredModalWindow");
    SetRadWindowProperties(childWindow, 1230, 1130);
    childWindow.add_close(AddNewPatientClick);
    childWindow.remove_close(SelectPlanClick);
    childWindow.remove_close(RereralPhysicianClick);
    childWindow.remove_close(SelectPatientClick);
    childWindow.remove_close(EligibilityVerification);
}, 0);
    return false;

}
function OpenEligibilityWindow() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;

    if (document.getElementById("txtInsPlanID").value.length == 0) {

        DisplayErrorMessage('420072');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("btnSelectPlan").focus();
        return false;
    }
    if (document.getElementById("txtPolicyHolderID").value.length == 0) {

        DisplayErrorMessage('420073');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("txtPolicyHolderID").focus();
        return false;
    }
    var PolicyHolderID = document.getElementById("txtPolicyHolderID").value;
    if (PolicyHolderID) {
        var humanID = document.getElementById("txtPolicyHolderAccountNo").value;

        var LastName = document.getElementById("txtPatientLastName").value + "," + document.getElementById("txtPatientFirstName").value;
        var GroupNumber = document.getElementById("txtGroupNumber").value;
        var InsPlanID = document.getElementById("txtInsPlanID").value;
        var InsCarrierName = document.getElementById("txtInsCarrierName").value;
        var EffectiveStartDate = document.getElementById("dtpEffectiveStartDate").value;
        var TerminationDate = document.getElementById("dtpTerminationDate").value;
       
        var obj = new Array();
        obj.push("humanID=" + humanID);
        obj.push("patientName=" + LastName);
        obj.push("groupNumber=" + GroupNumber);
        obj.push("policyHolderId=" + PolicyHolderID);
        obj.push("insPlanId=" + InsPlanID);
        obj.push("carrierName=" + InsCarrierName);
        obj.push("dtEffectiveDate=" + EffectiveStartDate);
        obj.push("dtTerminationDate=" + TerminationDate);
        obj.push("PatientType=" + document.getElementById("hdnPatientType").value);

        obj.push("PatientDOB=" + document.getElementById("txtPolicyHolderDateOfBirth").value);
        obj.push("EncounterId=" + document.getElementById("hdnEncounterID").value);

        obj.push("PCPCopay=" + document.getElementById("txtPCPCopay").value);
        obj.push("SPCCopay=" + document.getElementById("txtSPCCopay").value);
        obj.push("Deductible=" + document.getElementById("txtDeductible").value);
        obj.push("CoInsurance=" + document.getElementById("txtCoInsurance").value);
        obj.push("EVMode=" + document.getElementById("txtVerificationMode").value);
        obj.push("EVStatus=" + document.getElementById("txtEligibilityStatus").value);
        obj.push("CallRepName=" + document.getElementById("txtCallRepName").value);
        obj.push("CallRefNo=" + document.getElementById("txtCallRefNumber").value);
        obj.push("Comments=" + document.getElementById("txtComments").value);
        //added
        obj.push("insplanname=" + document.getElementById("txtInsPlanName").value);
        obj.push("ClaimCity=" + document.getElementById("txtClaimCity").value);
       
        obj.push("ClaimState=" + document.getElementById(GetClientId("txtClaimState")).value);
        obj.push("ClaimAddress=" + document.getElementById("txtClaimAddress").value.replace("#","!@"));
        obj.push("zipcode=" + document.getElementById("txtZipCode").value);
        obj.push("UTCTime=" + utc);
        var result = openModal("frmEligibilityVerification.aspx", 560, 830, obj, "AddInsuredModalWindow");
        var WindowName = $find('AddInsuredModalWindow');
        WindowName.remove_close(SelectPlanClick);
        WindowName.remove_close(RereralPhysicianClick);
        WindowName.remove_close(SelectPatientClick);
        WindowName.remove_close(AddNewPatientClick);
        WindowName.add_close(EligibilityVerification);
        return false;
    }
    else {

        DisplayErrorMessage('420026');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}

function showTime() { var dt = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalTime").value = utc; }
function CloseWindow() {
    if (document.getElementById("hdnSaveFlag").value == "true" || document.getElementById(GetClientId("btnSave")).disabled == false) {
        if (DisplayErrorMessage('420101') == true) {
            return false;
        }
        else {
            self.close();
        }
    }
    else {
        self.close();
    }
}
function CloseWindows() {
    if (document.getElementById("hdnSaveFlag").value == "true" || document.getElementById(GetClientId("btnSave")).disabled == false) {
        if (document.getElementById("hdnMessageType").value == "") {
            DisplayErrorMessage('420102');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            document.getElementById("btnSave").click();
            SaveCloseWindow();
        }
        else if (document.getElementById("hdnMessageType").value == "No") {
            document.getElementById("hdnMessageType").value = "";
            self.close();
        }
        else if (document.getElementById("hdnMessageType").value == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
        }
    }
    else {
        self.close();
    }
}

function verifiyPlanId() {
    if (confirm("The plan number entered during eligibility verification did not match the plan number you have selected for this Policy ID. Do you want to continue adding new policy?") == true) {
        document.getElementById("txtPolicyHolderID").value = "";
        document.getElementById("txtPolicyHolderID").focus();

    }

}

function Validation() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById("txtInsPlanID").value.length == 0) {
        DisplayErrorMessage('420042');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("txtPolicyHolderID").focus();
        return false;
    }
    if (document.getElementById("txtPolicyHolderID").value.length == 0) {
        DisplayErrorMessage('420026');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("txtPolicyHolderID").focus();
        return false;
    }
    if (document.getElementById("ddlPatientRelation").value.length == 0) {
        DisplayErrorMessage('420025');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("ddlPatientRelation").focus();
        return false;
    }
    if (document.getElementById("txtPolicyHolderFirstName").value.length == 0) {

        DisplayErrorMessage('420043');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("txtPolicyHolderFirstName").focus();
        return false;
    }
    var dt = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalTime").value = utc;
}

function AddNewPatientClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {


        document.getElementById("hdnFindPatientID").value = Result.HumanID;
        document.getElementById("btnAddNewInsuredRefresh").click();


    }

}
function changelabelstyle() {
    $("input[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
function SelectPatientClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {


        document.getElementById("hdnFindPatientID").value = Result.HumanId;
        document.getElementById("btnSelectPatientRefresh").click();

    }

}
function SelectPlanClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {


        document.getElementById("txtInsPlanID").value = Result.PlanId;
        document.getElementById("btnLoadPlan").click();


    }

}
function RereralPhysicianClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {

        document.getElementById("txtPCP").value = Result.sPhyName;
        document.getElementById("hdnTxtPCP").value = Result.sPhyName;
        document.getElementById("txtPcpTag").value = Result.ulPhyId;
        document.getElementById("txtPCPNPI").value = Result.sPhyNPI.replace("&nbsp;", "");
        AutoSave();
    }

}

function EligibilityVerification(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {


        if (Result.IsEVSaved == "TRUE") {
            document.getElementById("btnPerformEligibility").click();

        }


    }

}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
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
function ClearPcpIdTag() {
    document.getElementById("txtPcpTag").value = "";
    AutoSave();
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false
    }
    ClearPcpIdTag();
    return true;
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
function loadAddinsurance() {
    $('#txtPolicyHolderID').addClass('nonEditabletxtbox');
    $('#txtGroupNumber').addClass('nonEditabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("span[mand=Yes]").addClass('MandLabelstyle');

    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}




function Validatealphanumaeric() {
    var textvalue = document.getElementById('txtPolicyHolderID').value;
    var myRegEx = /^[-0-9a-z]*$/i;
    var isValid = !(myRegEx.test(textvalue));


    if (!textvalue.match(myRegEx)) {

        DisplayErrorMessage('8008');
        document.getElementById('txtPolicyHolderID').value = "";
        document.getElementById('txtPolicyHolderID').focus();
        return false;
    }
    else {

        return true;
    }

}