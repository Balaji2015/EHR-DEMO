var AssignedTo = "";
var vRowID = "";
let bOldCheck = false;
var vOldPriority = "";

function OpenGuarantor() {
    if (document.getElementById(GetClientId('txtAccountNo')).value.length != 0) {
        var obj = new Array();
        var HumanID = document.getElementById(GetClientId('txtAccountNo')).value;
        var PatType = document.getElementById(GetClientId('hdnPatientType')).value;
        obj.push("HumanID=" + HumanID);
        obj.push("Patype=" + PatType);

        setTimeout(
            function () {
                var oWnd = GetRadWindow();
                var childWindow = oWnd.BrowserWindow.radopen("frmViewGuarantor.aspx?HumanID=" + HumanID + "&Patype=" + PatType, "ctl00_DemographicsModalWindow");
                SetRadWindowProperties(childWindow, 650, 850);
                //CAP-1441 - In Testing & Production: Selected Guarantor Information through View guarantor is not displayed in Demographics screen
                childWindow.remove_close(ViewGaurantorClick);
                childWindow.add_close(ViewGaurantorClick);
                childWindow.remove_close(AddGuarantorClick);
                childWindow.remove_close(OpenPatIns);
                childWindow.remove_close(OpenAddInsForNewPatient);
                childWindow.remove_close(SelectGaurantorClick);
                childWindow.remove_close(FindPatientClick);
                childWindow.remove_close(CloseWorksetClick);
            }, 0);



    }
    else {
        DisplayErrorMessage('80011');
        return false;
    }
}
function showTime() { var dt = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById(GetClientId("hdnLocalTime")).value = utc; }
function openDemographicswindow() {
    var obj = new Array();
    obj.push("HumanId=" + 0);
    obj.push("bInsurance=" + true);
    obj.push("ScreenName=Demographics");
    obj.push("FromAddGuarantor=TRUE");

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + 0 + "&bInsurance=" + true + "&ScreenName=Demographics&FromAddGuarantor=TRUE", "ctl00_DemographicsModalWindow");
            SetRadWindowProperties(childWindow, 1230, 1130);
            childWindow.add_close(AddGuarantorClick);
            childWindow.remove_close(ViewGaurantorClick);
            childWindow.remove_close(OpenPatIns);
            childWindow.remove_close(OpenAddInsForNewPatient);
            childWindow.remove_close(SelectGaurantorClick);
            childWindow.remove_close(FindPatientClick);
            childWindow.remove_close(CloseWorksetClick);
        }, 0);


    return false;

}
function AutoSave() {
    document.getElementById(GetClientId("btnSave")).disabled = false;
    document.getElementById(GetClientId("hdnSaveFlag")).value = true; var dt = new Date();
    var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById(GetClientId("hdnLocalTime")).value = utc;

}

function ddlSexualOrientation_change() {
    AutoSave();
    if (document.getElementById(GetClientId("ddlSexualOrientation")).value.indexOf("please describe") > -1) {
        document.getElementById(GetClientId("TxtSexualOrientationSpecify")).style.backgroundColor = "";
        document.getElementById(GetClientId("TxtSexualOrientationSpecify")).readOnly = false;
        document.getElementById(GetClientId("TxtSexualOrientationSpecify")).value = '';
        $('#ctl00_C5POBody_TxtSexualOrientationSpecify').removeClass('nonEditabletxtbox');
        $('#ctl00_C5POBody_TxtSexualOrientationSpecify').addClass('Editabletxtbox');
    }
    else {
        document.getElementById(GetClientId("TxtSexualOrientationSpecify")).value = "";
        document.getElementById(GetClientId("TxtSexualOrientationSpecify")).readOnly = true;
        $('#ctl00_C5POBody_TxtSexualOrientationSpecify').removeClass('Editabletxtbox');
        $('#ctl00_C5POBody_TxtSexualOrientationSpecify').addClass('nonEditabletxtbox');
    }
    document.getElementById(GetClientId("hdnSexualOrientationSpecify")).value = "";
    document.getElementById(GetClientId("hdnGenderIdentity")).value = "";

}


function ddlGenderIdentity_change() {
    AutoSave();
    if (document.getElementById(GetClientId("ddlGenderIdentity")).value.indexOf("please specify") > -1) {
        document.getElementById(GetClientId("TxtGenderIdentity")).readOnly = false;
        $('#ctl00_C5POBody_TxtGenderIdentity').removeClass('nonEditabletxtbox');
        $('#ctl00_C5POBody_TxtGenderIdentity').addClass('Editabletxtbox');
    }
    else {
        document.getElementById(GetClientId("TxtGenderIdentity")).value = "";
        document.getElementById(GetClientId("TxtGenderIdentity")).readOnly = true;
        $('#ctl00_C5POBody_TxtGenderIdentity').removeClass('Editabletxtbox');
        $('#ctl00_C5POBody_TxtGenderIdentity').addClass('nonEditabletxtbox');
    }
}

function CloseWindow() {
    //if (document.getElementById(GetClientId("btnViewUpdateInsurance")) != undefined && document.getElementById(GetClientId("btnViewUpdateInsurance")).disabled == false) {
    //    if (document.getElementById(GetClientId("txtNoofPolicies")).value != "") {
    //        if (parseInt(document.getElementById(GetClientId("txtNoofPolicies")).value) < 1) {
    //            if (window.confirm("Insurance Policies not added. Do you want to add?")) {
    //                HumanId = document.getElementById(GetClientId("txtAccountNo")).value;
    //                ulpatientid = document.getElementById(GetClientId("hdnPatientID")).value
    //                objhumanid = document.getElementById(GetClientId("hdnPatientID")).value
    //                txtPatientlastname = document.getElementById(GetClientId("txtPatientlastname")).value
    //                txtPatientfirstname = document.getElementById(GetClientId("txtPatientfirstname")).value
    //                txtExternalAccNo = document.getElementById(GetClientId("txtExternalAccNo")).value
    //                if (parseInt(ulpatientid) == 0) {
    //                    var obj = new Array();
    //                    obj.push("HumanId=" + objhumanid);
    //                    obj.push("InsuranceType=" + true);
    //                    obj.push("LastName=" + txtPatientlastname);
    //                    obj.push("FirstName=" + txtPatientfirstname);
    //                    obj.push("ExAccountNo=" + txtExternalAccNo);
    //                    obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
    //                    setTimeout(
    //                        function () {
    //                            var oWnd = GetRadWindow();
    //                            var childWindow = oWnd.BrowserWindow.radopen("frmAddInsurancePolicies.aspx?HumanId=" + objhumanid + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname + "&FirstName=" + txtPatientfirstname + "&ExAccountNo=" + txtExternalAccNo + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value, "ctl00_DemographicsModalWindow");
    //                            SetRadWindowProperties(childWindow, 850, 1140);
    //                            childWindow.remove_close(OpenPatIns);
    //                            childWindow.remove_close(AddGuarantorClick);
    //                            childWindow.remove_close(ViewGaurantorClick);
    //                            childWindow.add_close(OpenAddInsForNewPatient);
    //                            childWindow.remove_close(SelectGaurantorClick);
    //                            childWindow.remove_close(FindPatientClick);
    //                            childWindow.remove_close(CloseWorksetClick);
    //                        }, 0);
    //                    return false;
    //                }
    //                else {
    //                    var obj = new Array();
    //                    obj.push("HumanId=" + HumanId);
    //                    obj.push("InsuranceType=" + true);
    //                    obj.push("LastName=" + txtPatientlastname);
    //                    obj.push("FirstName=" + txtPatientfirstname);
    //                    obj.push("ExAccountNo=" + txtExternalAccNo);
    //                    obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
    //                    setTimeout(
    //                        function () {
    //                            var oWnd = GetRadWindow();
    //                            var childWindow = oWnd.BrowserWindow.radopen("frmAddInsurancePolicies.aspx?HumanId=" + HumanId + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname + "&FirstName=" + txtPatientfirstname + "&ExAccountNo=" + txtExternalAccNo + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value, "ctl00_DemographicsModalWindow");
    //                            SetRadWindowProperties(childWindow, 850, 1140);
    //                            childWindow.remove_close(OpenPatIns);
    //                            childWindow.remove_close(AddGuarantorClick);
    //                            childWindow.remove_close(ViewGaurantorClick);
    //                            childWindow.add_close(OpenAddInsForNewPatient);
    //                            childWindow.remove_close(SelectGaurantorClick);
    //                            childWindow.remove_close(FindPatientClick);
    //                            childWindow.remove_close(CloseWorksetClick);
    //                        }, 0);
    //                    return false;
    //                }
    //                return false;
    //            }
    //            else {
    //            }
    //        }
    //    }
    //}
    if (document.getElementById(GetClientId("hdnSaveFlag")).value == "true" && document.getElementById(GetClientId("btnSave")).disabled == false) {
        if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
            DisplayErrorMessage('420087');
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
            document.getElementById(GetClientId("hdnYesNoMessage")).value = document.getElementById(GetClientId("hdnMessageType")).value;
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            var obj = new Object();
            obj.HumanID = document.getElementById(GetClientId("txtAccountNo")).value;
            obj.PatientName = document.getElementById(GetClientId("txtPatientlastname")).value + " " + document.getElementById(GetClientId("txtPatientfirstname")).value + " " + document.getElementById(GetClientId("txtPatientmiddlename")).value;
            obj.DOB = document.getElementById(GetClientId("dtpPatientDOB")).value;
            obj.SSN = document.getElementById(GetClientId("msktxtSSN")).value;
            obj.ExternalAccNo = document.getElementById(GetClientId("txtExternalAccNo")).value;
            obj.Sex = document.getElementById(GetClientId("ddlPatientsex")).value;
            obj.PriInsPlanID = document.getElementById(GetClientId("hdnPrimInsPlanID")).value;
            obj.PriInsPlanName = document.getElementById(GetClientId("txtPrimaryInsPlanName")).value;
            obj.SecInsPlanID = document.getElementById(GetClientId("hdnSecInsPlanID")).value;
            obj.SecInsPlanName = document.getElementById(GetClientId("txtSecInsPlanName")).value;
            obj.EVStatus = document.getElementById(GetClientId("txtRecentVerificationStatus")).value;
            obj.ScannedStatus = document.getElementById(GetClientId("txtRecentScannedStatus")).value;
            obj.PatientType = document.getElementById(GetClientId("hdnPatientType")).value;
            obj.HomePhoneNo = document.getElementById(GetClientId("msktxtHomePhno")).value;
            obj.CellPhoneNo = document.getElementById(GetClientId("msktxtCellPhno")).value;
            document.getElementById(GetClientId("btnSave")).click();
            document.getElementById(GetClientId("btnSave")).disabled = false;
            return true;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
            document.getElementById(GetClientId("hdnMessageType")).value = ""
            self.close();
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            return false;
        }
    }
    else {
        var obj = new Object();
        obj.HumanID = document.getElementById(GetClientId("txtAccountNo")).value;
        obj.PatientName = document.getElementById(GetClientId("txtPatientlastname")).value + " " + document.getElementById(GetClientId("txtPatientfirstname")).value + " " + document.getElementById(GetClientId("txtPatientmiddlename")).value;
        obj.DOB = document.getElementById(GetClientId("dtpPatientDOB")).value;
        obj.SSN = document.getElementById(GetClientId("msktxtSSN")).value;
        obj.ExternalAccNo = document.getElementById(GetClientId("txtExternalAccNo")).value;
        obj.Sex = document.getElementById(GetClientId("ddlPatientsex")).value;
        obj.PriInsPlanID = document.getElementById(GetClientId("hdnPrimInsPlanID")).value;
        obj.PriInsPlanName = document.getElementById(GetClientId("txtPrimaryInsPlanName")).value;
        obj.SecInsPlanID = document.getElementById(GetClientId("hdnSecInsPlanID")).value;
        obj.SecInsPlanName = document.getElementById(GetClientId("txtSecInsPlanName")).value;
        obj.EVStatus = document.getElementById(GetClientId("txtRecentVerificationStatus")).value;
        obj.ScannedStatus = document.getElementById(GetClientId("txtRecentScannedStatus")).value;
        obj.PatientType = document.getElementById(GetClientId("hdnPatientType")).value;
        obj.HomePhoneNo = document.getElementById(GetClientId("msktxtHomePhno")).value;
        obj.CellPhoneNo = document.getElementById(GetClientId("msktxtCellPhno")).value;
        if (window.opener) { window.opener.returnValue = obj; }
        window.returnValue = obj;

        if (obj.HumanID == 0 || obj.HumanID == "") {
            self.close();
        }
        else {
            returnToParent(obj);
        }
    }
}
function jsFormatSSN(asSSNControl) {
    var re = /\D/g; var lvCurrentSSNControlID = asSSNControl.id; var lvSSNControl = lvCurrentSSNControlID.substring(lvCurrentSSNControlID.lastIndexOf("_") + 1); var lvParent; var lvCompare; var lvRequired; if (lvSSNControl == "msktxtSSN") { lvParent = document.getElementById(GetClientId("msktxtSSN")); }
    var lvNumber = lvParent.value.replace(re, ""); var lvLength = lvNumber.length; if (lvLength > 3 && lvLength < 6) { var lvSegmentA = lvNumber.slice(0, 3); var lvSegmentB = lvNumber.slice(3, 5); lvParent.value = lvSegmentA + "-" + lvSegmentB; }
    else {
        if (lvLength > 5) { var lvSegmentA = lvNumber.slice(0, 3); var lvSegmentB = lvNumber.slice(3, 5); var lvSegmentC = lvNumber.slice(5, 9); lvParent.value = lvSegmentA + "-" + lvSegmentB + "-" + lvSegmentC; }
        else {
            if (lvLength < 1) { lvParent.value = ""; }
            lvParent.value = lvNumber;
        }
    }
}
function FormatZipCode(txtbox) {
    var e = event || evt; if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false; return true;
}
function FormatPhone(event, txtbox) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false; else {
        if (txtbox.value.length == 0)
            txtbox.value += "("; else if (txtbox.value.length == 4)
            txtbox.value += ")"; else if (txtbox.value.length == 5)
            txtbox.value += " "
        else if (txtbox.value.length == 8)
            txtbox.value += "-"; else if (txtbox.value.length == 11)
            return
    }
}
function ChangeDeathStatus(control) {
    var control = document.getElementById(GetClientId("ddlPatientStatus")); if ((control.value == "ALIVE") || (control.value == "DECEASED")) {
        if (window.confirm("Do you want to change the patient’s status?")) {
            document.getElementById(GetClientId("hdnstatus")).value = "True";
        }
        else {
            var e = document.getElementById(GetClientId("ddlPatientStatus"));
            control.value = e.options[e.selectedIndex].value
        }
    }
    if (control.value == "ALIVE") {
        if (document.getElementById(GetClientId("hdnstatus")).value == "True") {
            {
                document.getElementById(GetClientId("btnSave")).disabled = false;
                document.getElementById(GetClientId("hdnstatus")).value = "";
                //Cap - 1529
                document.getElementById(GetClientId("hdncancel")).value = "";
            }
        }
        else {
            document.getElementById(GetClientId("btnSave")).disabled = false; var Dt = new Date();
            document.getElementById(GetClientId('dtpDateOfDeath')).value = Dt.format("dd-MMM-yyyy");
            document.getElementById(GetClientId("hdncancel")).value = "True";
        }
    }

    else if (control.value == "DECEASED")
        
        if (document.getElementById(GetClientId("hdnstatus")).value == "True") {
            document.getElementById(GetClientId("btnSave")).disabled = false; var Dt = new Date(); document.getElementById(GetClientId('dtpDateOfDeath')).value = Dt.format("dd-MMM-yyyy");
            document.getElementById(GetClientId("hdnstatus")).value = "";
            //Cap - 1529
            document.getElementById(GetClientId("hdncancel")).value = "";
        }
        else {
            document.getElementById(GetClientId("btnSave")).disabled = false;
            document.getElementById(GetClientId("hdncancel")).value = "True";
            return false;
        }
}
function changelabel() {
    var control = document.getElementById(GetClientId("ddlPatientStatus"));
    if (control.value == "DECEASED") {
        document.getElementById(GetClientId("dtpDateOfDeath")).disabled = false;
        document.getElementById(GetClientId("ddlReasonForDeath")).disabled = false;
        $('#ctl00_C5POBody_dtpDateOfDeath').removeClass('nonEditabletxtbox');
        $('#ctl00_C5POBody_dtpDateOfDeath').addClass('Editabletxtbox');
        $('#ctl00_C5POBody_ddlReasonForDeath').removeClass('nonEditabletxtbox');
        $('#ctl00_C5POBody_ddlReasonForDeath').addClass('Editabletxtbox');
        $("#ctl00_C5POBody_lblDateOfDeath").removeClass('spanstyle');
        $("#ctl00_C5POBody_lblDateOfDeath").addClass('MandLabelstyle');
        $('#ctl00_C5POBody_lblDateOfDeath').html($('#ctl00_C5POBody_lblDateOfDeath').html().replace("*", "<span class='manredforstar'>*</span>"));
        $("#ctl00_C5POBody_lblReasonForDeath").removeClass('spanstyle');
        $("#ctl00_C5POBody_lblReasonForDeath").addClass('MandLabelstyle');
        $('#ctl00_C5POBody_lblReasonForDeath').html($('#ctl00_C5POBody_lblReasonForDeath').html().replace("*", "<span class='manredforstar'>*</span>"));
        document.getElementById(GetClientId("btnSave")).disabled = false; var Dt = new Date(); document.getElementById(GetClientId('dtpDateOfDeath')).value = Dt.format("dd-MMM-yyyy");
        document.getElementById(GetClientId("hdnstatus")).value = "";
    }
    else {
        document.getElementById(GetClientId("dtpDateOfDeath")).disabled = true;
        document.getElementById(GetClientId("dtpDateOfDeath")).value = "";
        $('#ctl00_C5POBody_dtpDateOfDeath').removeClass('Editabletxtbox');
        $('#ctl00_C5POBody_dtpDateOfDeath').addClass('nonEditabletxtbox');
        $('#ctl00_C5POBody_ddlReasonForDeath').removeClass('Editabletxtbox');
        $('#ctl00_C5POBody_ddlReasonForDeath').addClass('nonEditabletxtbox');
        document.getElementById(GetClientId("ddlReasonForDeath")).disabled = true;
        document.getElementById(GetClientId("ddlReasonForDeath")).value = "";
        $('#ctl00_C5POBody_lblDateOfDeath').html($('#ctl00_C5POBody_lblDateOfDeath').html().replace("*", ""));
        $('#ctl00_C5POBody_lblReasonForDeath').html($('#ctl00_C5POBody_lblReasonForDeath').html().replace("*", ""));
        document.getElementById(GetClientId("ctl00_C5POBody_lblReasonForDeath")).style.color = "Black";
        document.getElementById(GetClientId("ctl00_C5POBody_lblReasonForDeath")).style.color = "Black";
        document.getElementById(GetClientId("btnSave")).disabled = false;
        document.getElementById(GetClientId("hdnstatus")).value = "";

    }
}
function AllowAlphabet(e) {
    document.getElementById(GetClientId("btnSave")).disabled = false; isIE = document.all ? 1 : 0
    keyEntry = !isIE ? e.which : event.keyCode; if (((keyEntry >= '65') && (keyEntry <= '90')) || ((keyEntry >= '97') && (keyEntry <= '122')) || (keyEntry == '46') || (keyEntry == '32') || keyEntry == '45')
        return true; else { return false; }
}
function Copy(data) {
    document.getElementById(GetClientId("btnSave")).disabled = false; document.getElementById(GetClientId("hdnSaveFlag")).value = true;
    if (document.getElementById(GetClientId("chkGuarantorIsPatient")).checked == true) {
        if (data == 'Lastname') { var LastName = document.getElementById(GetClientId("txtPatientlastname")).value; document.getElementById(GetClientId("txtGuarantorLastName")).value = LastName }
        if (data == 'Firstname') { var LastName = document.getElementById(GetClientId("txtPatientfirstname")).value; document.getElementById(GetClientId("txtGuarantorFirstName")).value = LastName }
        if (data == 'MiddleName') { var LastName = document.getElementById(GetClientId("txtPatientmiddlename")).value; document.getElementById(GetClientId("txtGuarantorMiddleName")).value = LastName }
        if (data == 'Address') { var LastName = document.getElementById(GetClientId("txtPatientAddress")).value; document.getElementById(GetClientId("txtGuarantorAddress")).value = LastName }
        if (data == 'Address Line') { var LastName = document.getElementById(GetClientId("txtPatientAddressLine2")).value; document.getElementById(GetClientId("txtGuarantorAddressLine2")).value = LastName }
        if (data == 'Zipcode') {
            //CAP-1463 
            var LastName = $find(GetClientId("msktxtZipcode"))._text; $find(GetClientId("msktxtGuarantorZipCode"))?.set_value(LastName);
        }
        if (data == 'HomePhone') { var LastName = document.getElementById(GetClientId("msktxtHomePhno")).value; $find(GetClientId("msktxtGuarantorHomeNo")).set_value(LastName) }

        if (data == 'Email') { var Email = document.getElementById(GetClientId("txtEmail")).value; document.getElementById(GetClientId("txtGuaEmail")).value = Email }

        if (data == 'CellPhone') { var LastName = document.getElementById(GetClientId("msktxtCellPhno")).value; $find(GetClientId("msktxtGuarantorCellNo")).set_value(LastName) }
        if (data == 'City') { var LastName = document.getElementById(GetClientId("txtCity")).value; document.getElementById(GetClientId("txtGuarantorCity")).value = LastName }
        if (data == "SEX") { var textData = document.getElementById(GetClientId("ddlPatientsex")).value; document.getElementById(GetClientId("ddlGuarantorSex")).value = textData; }

        if (data == "DOB") { var textData = document.getElementById(GetClientId("dtpPatientDOB")).value; document.getElementById(GetClientId("dtpGuarantorDOB")).value = textData; }
        if (data == "PatientDOB") {
            $find(GetClientId("dtpGuarantorDOB")).set_value($find(GetClientId("dtpPatientDOB")).get_value());
            AutoSave();
        }

        if (data == "StartDate") { var textData = document.getElementById(GetClientId("txtStartdate")).value; }
        if (data == "PatientStartDate") {
            $find(GetClientId("dtpGuarantorDOB")).set_value($find(GetClientId("txtStartdate")).get_value());
            AutoSave();
        }

        if (data == "EndDate") { var textData = document.getElementById(GetClientId("txtEnddate")).value; }
        if (data == "PatientEndDate") {
            $find(GetClientId("dtpGuarantorDOB")).set_value($find(GetClientId("txtEnddate")).get_value());
            AutoSave();
        }

        if (data == "State") { var textData = document.getElementById(GetClientId("ddlState")).value; document.getElementById(GetClientId("ddlGuarantorState")).value = textData; }
    }
}
function change(btn) {
    document.getElementById(GetClientId("btnSave")).disabled = false;
    if (document.getElementById(GetClientId("txtAccountNo")).value != "") {
        document.getElementById(GetClientId("btnAddMessage")).disabled = false;
    }
}
function OpenRereralPhysician() {
    openModal("frmFindReferralPhysician.aspx", 150, 860, null, "ctl00_DemographicsModalWindow");
    var WindowName = $find('ctl00_DemographicsModalWindow');
    WindowName.remove_close(OpenPatIns);
    WindowName.remove_close(AddGuarantorClick);
    WindowName.remove_close(ViewGaurantorClick);
    WindowName.remove_close(OpenAddInsForNewPatient);
    WindowName.remove_close(SelectGaurantorClick);
    WindowName.remove_close(FindPatientClick);
    WindowName.remove_close(CloseWorksetClick);
    WindowName.add_close(function ReferalPhyClick(ownd, args) {
        var result = args.get_argument();
        if (result) {
            document.getElementById(GetClientId("txtPCPProvider")).value = result.sPhyName; document.getElementById(GetClientId("txtPCPProviderTag")).value = result.ulPhyId;
            if (result.sPhyNPI != "&nbsp;")
                document.getElementById(GetClientId("txtNPI")).value = result.sPhyNPI.replace("&nbsp;", "");
            else
                document.getElementById(GetClientId("txtNPI")).value = '';
            AutoSave();
        }
    });
    return false;
}
function openPatInsurancewindow() {
    var NoofPolicy = document.getElementById(GetClientId("txtNoofPolicies")).value;
    HumanId = document.getElementById(GetClientId("txtAccountNo")).value;
    if (parseInt(NoofPolicy) > 0) {
        if (HumanId) {
            var obj = new Array();
            obj.push("HumanId=" + HumanId);
            obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
            obj.push("CurrentProcess=" + document.getElementById(GetClientId("txtCurrentProcess")).value);

            setTimeout(
                function () {
                    var oWnd = GetRadWindow();
                    var oManager = oWnd.get_windowManager();
                    var childWindow = oManager.BrowserWindow.radopen("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + HumanId + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&CurrentProcess=" + document.getElementById(GetClientId("txtCurrentProcess")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
                    SetRadWindowProperties(childWindow, 590, 1160);
                    childWindow.add_close(OpenPatIns);
                    childWindow.remove_close(AddGuarantorClick);
                    childWindow.remove_close(ViewGaurantorClick);
                    childWindow.remove_close(OpenAddInsForNewPatient);
                    childWindow.remove_close(SelectGaurantorClick);
                    childWindow.remove_close(FindPatientClick);
                    childWindow.remove_close(CloseWorksetClick);
                }, 0);
        }
    }
    else {
        ulpatientid = document.getElementById(GetClientId("hdnPatientID")).value
        objhumanid = document.getElementById(GetClientId("hdnPatientID")).value
        txtPatientlastname = document.getElementById(GetClientId("txtPatientlastname")).value
        txtPatientfirstname = document.getElementById(GetClientId("txtPatientfirstname")).value
        txtExternalAccNo = document.getElementById(GetClientId("txtExternalAccNo")).value
        if (parseInt(ulpatientid) == 0) {
            var obj = new Array();
            obj.push("HumanId=" + objhumanid);
            obj.push("InsuranceType=" + true);
            obj.push("LastName=" + txtPatientlastname);
            obj.push("FirstName=" + txtPatientfirstname);
            obj.push("ExAccountNo=" + txtExternalAccNo);
            obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
            setTimeout(
                function () {
                    var oWnd = GetRadWindow();
                    var oManager = oWnd.get_windowManager();
                    var childWindow = oManager.BrowserWindow.radopen("frmAddInsurancePolicies.aspx?HumanId=" + objhumanid + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname + "&FirstName=" + txtPatientfirstname + "&ExAccountNo=" + txtExternalAccNo + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
                    SetRadWindowProperties(childWindow, 850, 1140);
                    childWindow.add_close(OpenAddInsForNewPatient);
                    childWindow.remove_close(OpenPatIns);
                    childWindow.remove_close(AddGuarantorClick);
                    childWindow.remove_close(ViewGaurantorClick);
                    childWindow.remove_close(SelectGaurantorClick);
                    childWindow.remove_close(FindPatientClick);
                    childWindow.remove_close(CloseWorksetClick);
                }, 0);
        }
        else {
            var obj = new Array();
            obj.push("HumanId=" + HumanId);
            obj.push("InsuranceType=" + true);
            obj.push("LastName=" + txtPatientlastname);
            obj.push("FirstName=" + txtPatientfirstname);
            obj.push("ExAccountNo=" + txtExternalAccNo);
            obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
            setTimeout(
                function () {
                    var oWnd = GetRadWindow();
                    var oManager = oWnd.get_windowManager();
                    var childWindow = oManager.BrowserWindow.radopen("frmAddInsurancePolicies.aspx?HumanId=" + HumanId + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname + "&FirstName=" + txtPatientfirstname + "&ExAccountNo=" + txtExternalAccNo + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
                    SetRadWindowProperties(childWindow, 850, 1140);
                    childWindow.add_close(OpenAddInsForNewPatient);
                    childWindow.remove_close(OpenPatIns);
                    childWindow.remove_close(AddGuarantorClick);
                    childWindow.remove_close(ViewGaurantorClick);
                    childWindow.remove_close(SelectGaurantorClick);
                    childWindow.remove_close(FindPatientClick);
                    childWindow.remove_close(CloseWorksetClick);
                }, 0);
        }
    }
    return false;
}
function OpenFindPatient() {
    var obj = new Array();

    obj.push("ScreenName=Demographics");

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx?ScreenName=Demographics", "ctl00_DemographicsAddInsured");
            SetRadWindowProperties(childWindow, 251, 1200);
            childWindow.add_close(FindPatientClick);
            childWindow.remove_close(OpenAddInsForNewPatient);
            childWindow.remove_close(OpenPatIns);
            childWindow.remove_close(AddGuarantorClick);
            childWindow.remove_close(ViewGaurantorClick);
            childWindow.remove_close(SelectGaurantorClick);
            childWindow.remove_close(CloseWorksetClick);
        }, 0);
    return false;
}
function OpenFindPatientForGuarantor() {
    var obj = new Array();
    obj.push("ScreenName=Demographics");


    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx?ScreenName=Demographics", "ctl00_DemographicsModalWindow");
            SetRadWindowProperties(childWindow, 251, 1200);
            childWindow.add_close(SelectGaurantorClick);
            childWindow.remove_close(FindPatientClick);
            childWindow.remove_close(OpenAddInsForNewPatient);
            childWindow.remove_close(OpenPatIns);
            childWindow.remove_close(AddGuarantorClick);
            childWindow.remove_close(ViewGaurantorClick);
            childWindow.remove_close(CloseWorksetClick);
        }, 0);
    return false;
}
function ShowConfirmMessage() {
    var Result = window.showModalDialog("Script/frmMessageBox.aspx", null, "center:yes;resizable:yes;dialogHeight:98px;dialogWidth:236px;location=no"); if (Result != null) {
        if (Result.OkCancel == 1) { document.getElementById(GetClientId("txtPolicyholderid")).value = Result.OkCancel; }
        else { document.getElementById(GetClientId("txtPolicyholderid")).value = "Clicked Cancel:" + Result.OkCancel; return false; }
    }
}
function OpenCloseWorkset() {
    var dt = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById(GetClientId("hdnLocalTime")).value = utc; if (document.getElementById(GetClientId("txtWorkSetID")).value == "" || document.getElementById(GetClientId("hdnParentScreen")).value == "Review Exception" || document.getElementById(GetClientId("hdnParentScreen")).value == "Update Call Log") { CloseWindow(); }
    else {

        var dt = new Date();
        var now = new Date();
        var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
        then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
        utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
        document.getElementById(GetClientId('hdnLocalTime')).value = utc;

        var obj = new Object();
        obj.WFObjectID = document.getElementById(GetClientId('txtWorkSetID')).value;
        obj.CompletedList = document.getElementById(GetClientId('hdnCurrentCompletedList')).value;
        obj.WFObjIDNotStarted = "0";
        obj.CompAmt = "0";
        obj.PostedAmount = "0";
        obj.ParentScreen = document.getElementById(GetClientId("hdnParentScreen")).value;
        obj.BatchType = document.getElementById(GetClientId('hdnBatchType')).value;
        obj.BillDestination = "";
        obj.HumanID = document.getElementById(GetClientId('txtHumanID')).value;
        obj.CloseWorksetStartTime = document.getElementById(GetClientId('hdnCloseWorksetStartTime')).value;
        setTimeout(
            function () {
                var oWnd = GetRadWindow();
                var childWindow = oWnd.BrowserWindow.radopen("frmCloseWorkset.aspx?WFObjectID=" + obj.WFObjectID + "&DOPStart=" + obj.CloseWorksetStartTime + "&CompletedList=" + obj.CompletedList + "&WFObjIDNotStarted=" + obj.WFObjIDNotStarted + "&CompAmt=" + obj.CompAmt + "&PostedAmt=" + obj.PostedAmount + "&ParentScreen=" + obj.ParentScreen + "&BatchType=" + obj.BatchType + "&BillDestination=" + obj.BillDestination + "&HumanID=" + obj.HumanID, "ctl00_DemographicsViewReportModalWindow");
                SetRadWindowProperties(childWindow, 700, 850);
                childWindow.remove_close(FindPatientClick);
                childWindow.remove_close(OpenAddInsForNewPatient);
                childWindow.remove_close(OpenPatIns);
                childWindow.remove_close(AddGuarantorClick);
                childWindow.remove_close(ViewGaurantorClick);
                childWindow.remove_close(SelectGaurantorClick);
                childWindow.remove_close(ViewReportClick);
                childWindow.add_close(CloseWorksetClick);
            }, 0);
        return false;
    }
}
function CloseWorksetClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != null) {
        returnToParent(args);
    }
}
function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName; for (i = 0; i < count; i++) { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}
function ViewReportClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != null) {
        if (Result.HumanID != "") {
            document.getElementById(GetClientId("hdnAccNoFromViewReport")).value = Result.HumanID;
            document.getElementById(GetClientId("txtDOOS")).value = Result.DOOS;
            document.getElementById(GetClientId("txtBatchName")).value = Result.BatchName;
            document.getElementById(GetClientId("btnViewReportRefresh")).click();
        }
        else if (Result.WFObjectID != "") {
            document.getElementById(GetClientId("btnCloseRefresh")).click();
        }
    }
}
function PatientInformationValidation() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;

    if (document.getElementById(GetClientId("txtPatientlastname")).value.length == 0) {
        DisplayErrorMessage('420001');
        document.getElementById(GetClientId("txtPatientlastname")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("txtPatientfirstname")).value.length == 0) {
        DisplayErrorMessage('420002');
        document.getElementById(GetClientId("txtPatientfirstname")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("dtpPatientDOB")).value == "__-___-____") {
        DisplayErrorMessage('420003');
        document.getElementById(GetClientId("dtpPatientDOB")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (DOBValidation(GetClientId("dtpPatientDOB")) == false) {
        DisplayErrorMessage('420031');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("ddlPatientsex")).value.length == 0) {
        DisplayErrorMessage('420004');
        document.getElementById(GetClientId("ddlPatientsex")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("msktxtCGPhNo")).value.length != 0 && PhNoValid(GetClientId("msktxtCGPhNo")) == false && document.getElementById(GetClientId("msktxtCGPhNo")).value != "(___) ___-____") {

        DisplayErrorMessage('420090');
        document.getElementById(GetClientId("msktxtCGPhNo")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("msktxtZipcode")).value.length != 0 && document.getElementById(GetClientId("msktxtZipcode")).value != "_____-____") {
        var str = document.getElementById(GetClientId("msktxtZipcode")).value;
        if (str.replace(/_/gi, "").length != 6 && str.replace(/_/gi, "").length != 10) {

            DisplayErrorMessage('420050');
            document.getElementById(GetClientId("msktxtZipcode")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }



    if (document.getElementById(GetClientId("msktxtHomePhno")).value.length != 0 && PhNoValid(GetClientId("msktxtHomePhno")) == false && document.getElementById(GetClientId("msktxtHomePhno")).value != "(___) ___-____") {
        DisplayErrorMessage('420006');
        document.getElementById(GetClientId("msktxtHomePhno")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("msktxtWorkPhoneNo")).value.length != 0 && PhNoValid(GetClientId("msktxtWorkPhoneNo")) == false && document.getElementById(GetClientId("msktxtWorkPhoneNo")).value != "(___) ___-____") {

        DisplayErrorMessage('420007');
        document.getElementById(GetClientId("msktxtWorkPhoneNo")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("msktxtSSN")).value.length != 0 && document.getElementById(GetClientId("msktxtSSN")).value != "___-__-____") {
        var str = document.getElementById(GetClientId("msktxtSSN")).value;
        if (str.replace(/_/gi, "").length < 11) {
            DisplayErrorMessage('380039');
            window.setTimeout(function () {
                document.getElementById(GetClientId("msktxtSSN")).focus();
            }, 0);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById(GetClientId("txtPatientAddress")).value.length == 0) {

        DisplayErrorMessage('420085');
        document.getElementById(GetClientId("txtPatientAddress")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("txtCity")).value.length == 0) {

        DisplayErrorMessage('420076');
        document.getElementById(GetClientId("txtCity")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById(GetClientId("ddlState")).value.length == 0) {

        DisplayErrorMessage('420077');
        document.getElementById(GetClientId("ddlState")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (document.getElementById(GetClientId("msktxtZipcode")).value == "_____-____") {

        DisplayErrorMessage('420054');
        document.getElementById(GetClientId("msktxtZipcode")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (document.getElementById(GetClientId("msktxtCellPhno")).value.length != 0 && PhNoValid(GetClientId("msktxtCellPhno")) == false && document.getElementById(GetClientId("msktxtCellPhno")).value != "(___) ___-____") {
        DisplayErrorMessage('420005');
        document.getElementById(GetClientId("msktxtCellPhno")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (document.getElementById(GetClientId("msktxtCellPhno")).value == "(___) ___-____" || document.getElementById(GetClientId("msktxtCellPhno")).value == "") {
        DisplayErrorMessage('420091');
        document.getElementById(GetClientId("msktxtCellPhno")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (document.getElementById(GetClientId("chkGuarantorIsPatient")).disabled == false && document.getElementById(GetClientId("chkGuarantorIsPatient")).checked == false) {
        if (document.getElementById(GetClientId("msktxtGuarantorCellNo")).value.length != 0 && PhNoValid(GetClientId("msktxtGuarantorCellNo")) == false && document.getElementById(GetClientId("msktxtGuarantorCellNo")).value != "(___) ___-____") {

            DisplayErrorMessage('420005');
            document.getElementById(GetClientId("msktxtGuarantorCellNo")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById(GetClientId("msktxtGuarantorHomeNo")).value.length != 0 && PhNoValid(GetClientId("msktxtGuarantorHomeNo")) == false && document.getElementById(GetClientId("msktxtGuarantorHomeNo")).value != "(___) ___-____") {

            DisplayErrorMessage('420006');
            document.getElementById(GetClientId("msktxtGuarantorHomeNo")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById(GetClientId("dtpEmerDOB")).disabled == false) {
        if (document.getElementById(GetClientId("dtpEmerDOB")).value != "__-___-____" && DOBValidation(GetClientId("dtpEmerDOB")) == false) {

            DisplayErrorMessage('420074');
            document.getElementById(GetClientId("dtpEmerDOB")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById(GetClientId("msktxtEmerCell")).value.length != 0 && PhNoValid(GetClientId("msktxtEmerCell")) == false && document.getElementById(GetClientId("msktxtEmerCell")).value != "(___) ___-____") {

            DisplayErrorMessage('420005');
            document.getElementById(GetClientId("msktxtEmerCell")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById(GetClientId("msktxtEmerHome")).value.length != 0 && PhNoValid(GetClientId("msktxtEmerHome")) == false && document.getElementById(GetClientId("msktxtEmerHome")).value != "(___) ___-____") {

            DisplayErrorMessage('420006');
            document.getElementById(GetClientId("msktxtEmerHome")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById(GetClientId("msktxtFaxNumber")).value.length != 0 && PhNoValid(GetClientId("msktxtFaxNumber")) == false && document.getElementById(GetClientId("msktxtFaxNumber")).value != "(___) ___-____") {

        DisplayErrorMessage('420013');
        document.getElementById(GetClientId("msktxtFaxNumber")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if ((document.getElementById(GetClientId("chkEnrollOnlineAccess")).checked == true)) {
        if (document.getElementById(GetClientId("txtEmail")).value.length == 0) {
            DisplayErrorMessage('420056');
            document.getElementById(GetClientId("txtEmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById(GetClientId("txtEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtEmail")).value) == false) {

            DisplayErrorMessage('320010');
            document.getElementById(GetClientId("txtEmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        if (document.getElementById(GetClientId("txtReptEmail")).value.length != 0 && document.getElementById(GetClientId("txtEmail")).value.length == 0) {
            DisplayErrorMessage('1007005');
            document.getElementById(GetClientId("txtEmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById(GetClientId("txtReptEmail")).value == document.getElementById(GetClientId("txtEmail")).value) {
            DisplayErrorMessage('1007006');
            document.getElementById(GetClientId("txtReptEmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById(GetClientId("txtReptEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtReptEmail")).value) == false) {
            DisplayErrorMessage('420030');
            document.getElementById(GetClientId("txtReptEmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById(GetClientId("msktxtEmerZipCode")).value.length != 0 && document.getElementById(GetClientId("msktxtEmerZipCode")).value != "_____-____") {
        var str = document.getElementById(GetClientId("msktxtEmerZipCode")).value;
        if (str.replace(/_/gi, "").length != 6 && str.replace(/_/gi, "").length != 10) {

            DisplayErrorMessage('420050');
            document.getElementById(GetClientId("msktxtEmerZipCode")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }



    if (document.getElementById(GetClientId("txtEmail")).value.length != 0) {

        if (IsEmail(document.getElementById(GetClientId("txtEmail")).value) == false) {
            DisplayErrorMessage('320010');
            document.getElementById(GetClientId("txtEmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

    }
    if (document.getElementById(GetClientId("ddlPatientStatus")).value == "DECEASED") {
        //Cap - 1570
        var DateDeath = parseMyDate(document.getElementById(GetClientId("dtpDateOfDeath")).value);
        var dtDeath = new Date();

        if (document.getElementById(GetClientId("dtpDateOfDeath")).value == "__-___-____") {

            DisplayErrorMessage('420078');
            document.getElementById(GetClientId("dtpDateOfDeath")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }       
        //Cap - 1570
              
        if (DeathValidation(GetClientId("dtpDateOfDeath")) == false && document.getElementById(GetClientId("dtpDateOfDeath")).value != "__-___-____") {
            DisplayErrorMessage('420093');
            document.getElementById(GetClientId("dtpDateOfDeath")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        if (DateDeath > dtDeath) {
            DisplayErrorMessage('420080');
            document.getElementById(GetClientId("dtpDateOfDeath")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }  

        if (document.getElementById(GetClientId("ddlReasonForDeath")).value.length == 0) {

            DisplayErrorMessage('420079');
            document.getElementById(GetClientId("ddlReasonForDeath")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        

        
        

    }
    var enterDate = document.getElementById(GetClientId("dtpGuarantorDOB")).value;  // $find(GetClientId("dtpGuarantorDOB"))._value
    var today = new Date();
    var birthDate = new Date(enterDate.split('-')[2], x, enterDate.split('-')[0]);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    document.getElementById(GetClientId('hdnAge')).value = age;
    if (document.getElementById(GetClientId('hdnAge')).value < 18) {
        document.getElementById(GetClientId("chkGuarantorIsPatient")).checked = false; //false;
        document.getElementById(GetClientId("btnUncheckGurantor")).click();
        document.getElementById(GetClientId("txtGuarantorLastName")).value = "";
        document.getElementById(GetClientId("txtGuarantorFirstName")).value = "";
        document.getElementById(GetClientId("txtGuarantorMiddleName")).value = "";
        document.getElementById(GetClientId("dtpGuarantorDOB")).value = "";
        document.getElementById(GetClientId("ddlGuarantorSex")).value = "";
        document.getElementById(GetClientId("txtGuarantorAddress")).value = "";
        document.getElementById(GetClientId("txtGuarantorAddressLine2")).value = "";
        document.getElementById(GetClientId("txtGuarantorCity")).value = "";
        document.getElementById(GetClientId("ddlGuarantorState")).value = "";
        document.getElementById(GetClientId("msktxtGuarantorZipCode")).value = "";
        document.getElementById(GetClientId("msktxtGuarantorHomeNo")).value = "";
        document.getElementById(GetClientId("msktxtGuarantorCellNo")).value = "";
        document.getElementById(GetClientId("ddlGuarantorRelationship")).value = "";
        document.getElementById(GetClientId("txtGuaEmail")).value = "";
        DisplayErrorMessage('420081');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        return false;
    }

    if (document.getElementById(GetClientId("ddlImmunizationRegStatus")).value.length == 0) {

        DisplayErrorMessage('420089');
        document.getElementById(GetClientId("ddlImmunizationRegStatus")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    document.getElementById(GetClientId("hdnSexualOrientationSpecify")).value = document.getElementById(GetClientId("TxtSexualOrientationSpecify")).value;
    document.getElementById(GetClientId("hdnGenderIdentity")).value = document.getElementById(GetClientId("TxtGenderIdentity")).value;

    setplanDetailssession();
}

function DOBValidation(dateToValidate) {
    var splitdate = document.getElementById(GetClientId(dateToValidate.split('_')[2])).value
    var dt1 = new Date();
    var dd = new Date();
    var month = new Array();
    switch (splitdate.split('-')[1]) {
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
        case splitdate.split('-')[1]:
            return false;
            break;

    }
    if (isNaN(Date.parse(splitdate))) {
        return false;
    }
    if (splitdate.split('-')[0] == "00") {
        return false;
    }
    dd.setFullYear(splitdate.split('-')[2], x, splitdate.split('-')[0]);
    if (isNaN(dd)) {
        return false;
    }
    if (parseInt(splitdate.split('-')[0]) > 31) {
        return false;
    }
    if ((dd.getFullYear() > dt1.getFullYear())) {
        return false;
    }
    else if (dd.getMonth() > dt1.getMonth() && (dd.getFullYear() >= dt1.getFullYear())) {
        return false;
    }
    else if (dd.getDate() > dt1.getDate() && (dd.getMonth() >= dt1.getMonth()) && (dd.getFullYear() >= dt1.getFullYear())) {
        return false;
    }
    else {
        return true;
    }
}
//Cap - 1582
function DeathValidation(dateToValidate) {
    var splitdate = document.getElementById(GetClientId(dateToValidate.split('_')[2])).value
    var dt1 = new Date();
    var dd = new Date();
    var month = new Array();
    switch (splitdate.split('-')[1]) {
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
        case splitdate.split('-')[1]:
            return false;
            break;

    }
    if (isNaN(Date.parse(splitdate))) {
        return false;
    }
    if (splitdate.split('-')[0] == "00") {
        return false;
    }
    dd.setFullYear(splitdate.split('-')[2], x, splitdate.split('-')[0]);
    if (isNaN(dd)) {
        return false;
    }
    if (parseInt(splitdate.split('-')[0]) > 31) {
        return false;
    }
    else {
        return true;
    }
}


function PhNoValid(sphno) {
    var s = document.getElementById(sphno).value;
    sReplace = s.replace(/_/gi, "");
    if (sReplace.length < 14) {
        return false;
    }
    else {
        return true;
    }
}

function IsEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);

}

function EnableSendEmail(ctl) {
    if (ctl.checked == true) {
        document.getElementById(GetClientId("btnSendEmail")).disabled = false;

        $('#spanemail').addClass('MandLabelstyle').removeClass('spanstyle');
        document.getElementById(GetClientId("spanemailstar")).style.display = "block";
    }
    else {
        document.getElementById(GetClientId("btnSendEmail")).disabled = true;
        $('#spanemail').addClass('spanstyle').removeClass('MandLabelstyle');
        document.getElementById(GetClientId("spanemailstar")).style.display = "none";
    }
    AutoSave();
}
function OpenAddUpdate() {

    var svalue = "MESSAGE DESCRIPTION";
    var Result = window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + svalue + "&PhyID=" + document.getElementById(GetClientId("hdnPhysicianID")).value, null, "center:yes;resizable:yes;dialogHeight:645px;dialogWidth:1025px;scroll:yes;");
    if (Result == null)
        return false;
    return true;
}
function OpenAddUpdateForMessageNotes() {

    var svalue = "MESSAGE NOTES";
    var Result = window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + svalue + "&PhyID=" + document.getElementById(GetClientId("hdnPhysicianID")).value, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:650px;scroll:yes;");
    if (Result == null)
        return false;
    return true;
}

function OpenUpload() {

    var obj = new Array();
    obj.push("HumanId=" + document.getElementById(GetClientId("txtAccountNo")).value);
    obj.push("Screen=Demographic");
    openNonModal("frmViewResult.aspx", 1000, 1200, obj);
}
function OpenViewMessage() {
    var obj = new Array();
    var svalue = document.getElementById(GetClientId("txtAccountNo")).value;
    var EncId = document.getElementById(GetClientId("hdnEncounterID")).value;
    obj.push("AccountNum=" + svalue);
    obj.push("EncounterId=" + EncId);
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var result = oWnd.BrowserWindow.openModal("frmViewMessage.aspx", 1000, 1190, obj);
        }, 0);

    return false;
}
function OpenDisplayObject() {
    if (document.getElementById(GetClientId("hdnObjectType")).value != "WORKSET") {
        var obj = new Array();
        var sWorksetID = document.getElementById(GetClientId("txtWorkSetID")).value;
        var sObjType = document.getElementById(GetClientId("hdnObjectType")).value;
        var sObjSystemID = document.getElementById(GetClientId("hdnObjSystemID")).value;
        var HumanID = document.getElementById(GetClientId("txtAccountNo")).value;
        obj.push("WorksetID=" + sWorksetID);
        obj.push("ObjType=" + sObjType);
        obj.push("ObjSystemID=" + sObjSystemID);
        obj.push("HumanID=" + HumanID);
        var Result = openNonModal("frmNewDisplayObject.aspx", 850, 1500, obj);

        if (Result == null)
            return false;
    }
}
function OpenNewDisplayObject() {
    if (document.getElementById(GetClientId("hdnObjectType")).value != "WORKSET") {
        var obj = new Array();
        var sWorksetID = document.getElementById(GetClientId("txtWorkSetID")).value;
        var sHumanID = document.getElementById(GetClientId("txtAccountNo")).value;
        obj.push("WorksetID=" + sWorksetID);
        obj.push("HumanID=" + sHumanID);
        var Result = openNonModal("frmNewDisplayObject.aspx", 900, 1200, obj, "ctl00_DemographicsModalWindow");
        if (Result == null)
            return false;
    }
}
function OpenViewBatch() {
    if (document.getElementById(GetClientId("hdnFilePath")).value.length != 0) {
        var obj = new Array();
        obj.push("FilePath=" + document.getElementById(GetClientId("hdnFilePath")).value);
        obj.push("HumanID=" + document.getElementById(GetClientId("txtAccountNo")).value);
        obj.push("BatchName=" + document.getElementById(GetClientId("txtBatchName")).value);
        obj.push("DOOS=" + document.getElementById(GetClientId("txtDOOS")).value);
        var Result = openNonModal("frmViewBatch.aspx", 940, 1170, obj);
        if (Result == null)
            return false;
    }
}
function GetAddPatientGuarantor(yesnocancelmessage, human_details) {
    if (yesnocancelmessage == "Yes") {
        DisplayErrorMessage('420020');
        document.getElementById(GetClientId("hdnMessageType")).value = "";
    }
    if (document.getElementById(GetClientId("hdnFromAddPatient")).value == "TRUE") {
        var obj = new Object();
        obj.HumanID = document.getElementById(GetClientId("txtAccountNo")).value;
        obj.PatientName = document.getElementById(GetClientId("txtPatientlastname")).value + " " + document.getElementById(GetClientId("txtPatientfirstname")).value;
        obj.DOB = document.getElementById(GetClientId("dtpPatientDOB")).value;
        obj.SSN = document.getElementById(GetClientId("msktxtSSN")).value;
        obj.ExternalAccNo = document.getElementById(GetClientId("txtExternalAccNo")).value;
        obj.Sex = document.getElementById(GetClientId("ddlPatientsex")).value;
        obj.PriInsPlanID = document.getElementById(GetClientId("hdnPrimInsPlanID")).value;
        obj.PriInsPlanName = document.getElementById(GetClientId("txtPrimaryInsPlanName")).value;
        obj.SecInsPlanID = document.getElementById(GetClientId("hdnSecInsPlanID")).value;
        obj.SecInsPlanName = document.getElementById(GetClientId("txtSecInsPlanName")).value;
        obj.EVStatus = document.getElementById(GetClientId("txtRecentVerificationStatus")).value;
        obj.ScannedStatus = document.getElementById(GetClientId("txtRecentScannedStatus")).value;
        if (window.opener) { window.opener.returnValue = obj; } window.returnValue = obj; self.close();
    }
    else {

        if (window.location.search.indexOf("Functionality=ADDPATIENT") > -1) {
            var obj = new Object();
            human_details = human_details.replace("&apos", "'");
            var selectedPatient = human_details != null ? JSON.parse(human_details) : null;
            if (selectedPatient != null) {
                obj.Human_id = selectedPatient.Id.toString();
                obj.PatientName = selectedPatient.Last_Name + ", " + selectedPatient.First_Name + " " + selectedPatient.MI;
                var date_components = selectedPatient.Birth_Date.split('T');
                if (date_components.length > 0)
                    obj.PatientDOB = date_components[0];
                else
                    obj.PatientDOB = "";
                obj.Status = selectedPatient.Patient_Status;
                obj.PCP = selectedPatient.PCP_Name;
                obj.HumanType = selectedPatient.Human_Type;
                obj.PatientGender = selectedPatient.Sex;
                obj.Aco_Eligible = selectedPatient.ACO_Is_Eligible_Patient;
                obj.SSN = selectedPatient.SSN;
                obj.Account_Status = selectedPatient.Account_Status;
                obj.Home_Phone = selectedPatient.Home_Phone_No;
                obj.Cell_Phone = selectedPatient.Cell_Phone_Number;
                obj.Encounter_Provider_ID = selectedPatient.Encounter_Provider_ID;
                obj.PolicyHolderID = "";
                obj.IsNewPatient = "TRUE";
                obj.IsQuickPatient = "TRUE";
            }
            returnToParent(obj);
        }
        else
            GetRadWindow().close();
    }
}

function GetLocalTime() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    return utc;
}
function GetEndLocalTime() {
    document.getElementById(GetClientId('hdnEndLocalTime')).value = GetLocalTime();
    document.getElementById(GetClientId('hdnNextDateAndTime')).value = GetLocalTime();
}
function CloseWindow1() {
    window.close();
}

function OpenPatIns(oWindow, args) {
    document.getElementById(GetClientId("hdnBtnLoadInsurance")).click();
}
function OpenAddInsForNewPatient(oWindow, args) {
    document.getElementById(GetClientId("hdnBtnLoadInsurance")).click();
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    if (oWindow == null) {
        oWindow = $find(ModalWndw);
    }
    return oWindow;
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

function FindPatientClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("hdnPatientID")).value = Result.HumanId;
        document.getElementById(GetClientId("btnFindpatientClick")).click();
    }
}
function AddGuarantorClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("hdnGuarantorID")).value = Result.HumanID;
        document.getElementById(GetClientId("btnSave")).disabled = false; 
        document.getElementById(GetClientId("hdnSaveFlag")).value = true;
        document.getElementById(GetClientId("btnAddGuarantorRefresh")).click();
    }
}
function SelectGaurantorClick(oWindow, args) {
    var Result = args.get_argument();
    
    if (Result) {
        var name = Result.PatientName;
        var dob = Result.PatientDOB;
        var sex = Result.PatientGender;
        var status = Result.Status;
        var cell_phone = Result.Cell_Phone;
        var home_phone = Result.Home_Phone;
        var zipcode = Result.ZipCode;
        var email = Result.EMail;
        if (Result.Address != null)
            var address = Result.Address;

        document.getElementById(GetClientId("hdnGuarantorID")).value = Result.HumanId;
        if (name != undefined && name.split(' ')[0].split(',')[1] != undefined)
            document.getElementById(GetClientId("txtGuarantorFirstName")).value = name.split(' ')[0].split(',')[1]
        if (name != undefined && name.split(' ')[0] != undefined)
            document.getElementById(GetClientId("txtGuarantorLastName")).value = name.split(',')[0];
        if (name != undefined && name.split(' ')[1] != undefined)
            document.getElementById(GetClientId("txtGuarantorMiddleName")).value = name.split(' ')[1];
        if (dob != undefined && dob.split(' ')[0] != undefined)
            document.getElementById(GetClientId("dtpGuarantorDOB")).value = dob.split(' ')[0];
        if (sex != undefined)
            document.getElementById(GetClientId("ddlGuarantorSex")).value = Result.PatientGender;
        if (status != undefined)
            document.getElementById(GetClientId("ddlPatientStatus")).value = Result.Status;
        if (cell_phone != undefined)
            document.getElementById(GetClientId("msktxtGuarantorCellNo")).value = Result.Cell_Phone;
        if (home_phone != undefined)
            document.getElementById(GetClientId("msktxtGuarantorHomeNo")).value = Result.Home_Phone;
        if (address != undefined && address.split(',')[0] != undefined)
            document.getElementById(GetClientId("txtGuarantorAddress")).value = address.split(',')[0];
        if (address != undefined && address.split(',')[0] != undefined)
            document.getElementById(GetClientId("txtGuarantorAddressLine2")).value = address.split(',')[0];
        if (address != undefined && address.split(',')[1] != undefined)
            document.getElementById(GetClientId("txtGuarantorCity")).value = address.split(',')[1];
        if (address != undefined && address.split(',')[2] != undefined)
            document.getElementById(GetClientId("ddlGuarantorState")).value = address.split(',')[2];
        if (zipcode != undefined)
            document.getElementById(GetClientId("msktxtGuarantorZipCode")).value = Result.ZipCode;
        if (email != undefined)
            document.getElementById(GetClientId("txtGuaEmail")).value = Result.EMail;


        document.getElementById(GetClientId("btnSave")).disabled = false;
        document.getElementById(GetClientId("hdnSaveFlag")).value = true;
        document.getElementById(GetClientId("btnAddGuarantorRefresh")).click();

    }

}

function ViewGaurantorClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("hdnGuarantorIdForView")).value = Result.GuarantorId;
    }
    //CAP-1750 - Guarantor information displayed as blank in Demographics screen
    var activeAnyGuarantor = sessionStorage.getItem("ActiveAnyGuarantor");
    if (activeAnyGuarantor == true || activeAnyGuarantor == 'true') {
        document.getElementById(GetClientId("hdnBtnLoadInsurance")).click();
    }
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
function dtpPatientDOB_OnDateSelected(sender, args) {
    if (document.getElementById(GetClientId("chkGuarantorIsPatient")).checked == true) {
        $find(GetClientId("dtpGuarantorDOB")).set_selectedDate(args._newDate);
    }
    AutoSave();
}

function showTip(ddl) {

    document.getElementById(GetClientId(ddl.id)).title = document.getElementById(GetClientId(ddl.id)).options[document.getElementById(GetClientId(ddl.id)).selectedIndex].text;
    if (document.getElementById("ctl00_C5POBody_ddlPreferredLanguage").value == "" || document.getElementById("ctl00_C5POBody_ddlPreferredLanguage").value == "Patient Refuses to answer" || document.getElementById("ctl00_C5POBody_ddlPreferredLanguage").value == "Declined to specify" || document.getElementById("ctl00_C5POBody_ddlPreferredLanguage").value == "Preferred Language not indicated") {
        document.getElementById("ctl00_C5POBody_chkReqTranslator").checked = false;
        document.getElementById("ctl00_C5POBody_chkReqTranslator").disabled = true;
    }
    else {
        document.getElementById("ctl00_C5POBody_chkReqTranslator").disabled = false;
    }
    AutoSave();
}

function OnMouseHover(ddl) {

    document.getElementById(GetClientId(ddl.id)).title = document.getElementById(GetClientId(ddl.id)).options[document.getElementById(GetClientId(ddl.id)).selectedIndex].text;
}
function ShowLoading() {
}


function PatientDemographicsDateVlidation(sender, args) {
    var EnteredDateLength = parseInt(args._newValue.replace("-", "").replace("-", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").length);
    if (EnteredDateLength != 9 && EnteredDateLength > 0) {
        alert("Please Enter the Date Fully.")
        $find(GetClientId(sender.get_id().split('_')[2])).focus(true);
        return false;
    }
    if (EnteredDateLength == 9) {
        validatedate($find(GetClientId(sender.get_id().split('_')[2]))._value, sender.get_id().split('_')[2]);
    }

}


function validatedate(inputText, ControlId) {
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;
    if (inputText.match(FormatDDMMMYYYY)) {
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
                    return false;
                    return false;
                }
                if ((lyear == true) && (DateInput > 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId)).clear();
                    $find(GetClientId(ControlId)).focus(true);
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();

            if (document.getElementById(GetClientId("dtpPatientDOB")).value != "__-___-____" || document.getElementById(GetClientId("dtpEmerDOB")).value != "__-___-____") {
                if (ControlId == 'dtpPatientDOB') {
                    Dobdate = parseMyDate(document.getElementById(GetClientId("dtpPatientDOB")).value);
                }
                else if (ControlId == 'dtpEmerDOB') {
                    Dobdate = parseMyDate(document.getElementById(GetClientId("dtpEmerDOB")).value);
                }
                else {
                    Dobdate = parseMyDate(document.getElementById(GetClientId("dtpPatientDOB")).value);
                }
            }
            if (Dobdate > CurrentDate) {
                alert("The Date of Birth you have entered is in the future. Please enter a valid day, month, and year.");
                if (ControlId == 'dtpPatientDOB') {
                    $find(GetClientId('dtpPatientDOB')).clear();
                    $find(GetClientId('dtpPatientDOB')).focus(true);
                    return false;
                }
                else if (ControlId == 'dtpEmerDOB') {

                    $find(GetClientId('dtpEmerDOB')).clear();
                    $find(GetClientId('dtpEmerDOB')).focus(true);
                    return false;
                    return false;
                }

            }
        }
        else {
            alert('Invalid date format!');
            $find(GetClientId(ControlId)).clear();
            $find(GetClientId(ControlId)).focus(true);
            return false;
        }
    }
}

function AddMessageDemo() {
    //CAP-726 - add loader
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    return true;
}
function ValidationOnSaveAndNext() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    document.getElementById(GetClientId('hdnEndLocalTime')).value = utc;

    PatientInformationValidation();
}
function ViewGuarantorshowTime() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}

function CloseViewGuarantorWindow() {
    //CAP-1750 - Guarantor information displayed as blank in Demographics screen
    sessionStorage.setItem("ActiveAnyGuarantor", 'true');
    self.close();
}
function CloseEligibilityHistoryWindow() {
    self.close();
}
function OpenIndexing() {
    if (document.getElementById(GetClientId('txtAccountNo')).value == "") {
        DisplayErrorMessage('420083');
        return false;
    }
    var obj = new Object();
    obj.WFObjectID = document.getElementById(GetClientId('txtWorkSetID')).value;
    obj.HumanID = document.getElementById(GetClientId('txtAccountNo')).value;

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmIndexing.aspx?WFObjectID=" + obj.WFObjectID + "&HumanId=" + obj.HumanID, "ctl00_DemographicsModalWindow");
            SetRadWindowProperties(childWindow, 500, 1250);
            childWindow.add_close(function IndexClick(oWindow, args) {
                var Result = args.get_argument();
                if (Result != null) {
                }
            });
        }, 0);
    return false;
}
function ClosePatientDemographics() {
    var IsClearAll = DisplayErrorMessage('420069');
    if (IsClearAll == true) {
        OpenCloseWorkset();
    }
    else {
        return false;
    }
}
function CloseRefreshClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("btnCloseRefresh")).click();
    }
}
function ClearPCPTag() {
    AutoSave();
    document.getElementById(GetClientId("txtPCPProviderTag")).value = "";

}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false
    }
    ClearPCPTag();
    return true;
}
function OpenDemographics() {

    var AccNO1 = document.getElementById(GetClientId("txtAccountNo")).value;
    var EncounterID = document.getElementById(GetClientId("hdnEncounterID")).value;
    if (document.getElementById(GetClientId("hdnIsPopUp")).value == "Y") {
        var obj = new Array();
        obj.push("HumanId=" + AccNO1);
        obj.push("EncounterId=" + EncounterID);
        setTimeout(
            function () {
                var oWnd = GetRadWindow();
                var childWindow = oWnd.BrowserWindow.openModal("frmPopup.aspx", 460, 1000, obj, "ctl00_DemographicsModalWindow");

            }, 0);

    }

}

function OpenAddressHistory() {

    var AccNO1 = document.getElementById(GetClientId("txtAccountNo")).value;
    var obj = new Array();
    obj.push("HumanId=" + AccNO1);
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.openModal("frmAddressHistory.aspx", 600, 1030, obj, "ctl00_DemographicsModalWindow");

        }, 0);
}
function PreventDot(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 46) {
        return false
    }
    AutoSave();
}
var RaceTag = "", Tag = "";
function listRaceChange(listId) {
    var ID;
    var ID1;
    ID = listId.id;
    ID1 = ID.replace("_listRace", "_txtRace");
    //CAP-1471
    var txtbox = document?.getElementById(ID1)?.value;
    var txtValue = listId[listId.selectedIndex]?.text;
    Tag = listId[listId.selectedIndex]?.value;
    if (txtValue == "[Empty]") {
        RaceTag = ""
        ClearRace();
    }
    else {
        if (RaceTag == "") {
            RaceTag = Tag;
        }
        else {
            RaceTag = RaceTag + "," + Tag;
        }
        if (txtbox == '' && txtbox.indexOf(txtValue) == -1) {
            txtbox = txtValue;
            document.getElementById(ID1).value = txtbox;
        }
        else if (txtbox.indexOf(txtValue) == -1) {
            txtbox = txtbox + "," + txtValue;
            document.getElementById(ID1).value = txtbox;
        }
    }
    document.getElementById(GetClientId("btnSave")).disabled = false;
    document.getElementById(GetClientId("hdnRaceTag")).value = RaceTag;
}
var RaceTag = "", Tag = "";
function listGranularityChange(listId) {
    var ID;
    var ID1;
    ID = listId.id;
    ID1 = ID.replace("_ListGranularity", "_txtGranularity");
    var txtbox = document.getElementById(ID1).value;
    var txtValue = listId[listId.selectedIndex].text;
    Tag = listId[listId.selectedIndex].value;
    if (txtValue == "[Empty]") {
        RaceTag = ""
        ClearGranularity();
    }
    else {
        if (RaceTag == "") {
            RaceTag = Tag;
        }
        else {
            RaceTag = RaceTag + "," + Tag;
        }
        if (txtbox == '' && txtbox.indexOf(txtValue) == -1) {
            txtbox = txtValue;
            document.getElementById(ID1).value = txtbox;
        }
        else if (txtbox.indexOf(txtValue) == -1) {
            txtbox = txtbox + "," + txtValue;
            document.getElementById(ID1).value = txtbox;
        }
    }
    document.getElementById(GetClientId("btnSave")).disabled = false;
    document.getElementById(GetClientId("hdnGranularTag")).value = RaceTag;
}
var lstCtrl = null;
function btnDropDown() {
    var pbDropDownValue = document.getElementById(GetClientId("btnDropdown")).value;
    var Button = document.getElementById(GetClientId("btnDropdown"));
    if (pbDropDownValue == "+") {
        document.getElementById(GetClientId("listRace")).style.display = "block";
        Button.value = "-";
    }
    else if (pbDropDownValue == "-") {
        Button.value = "+"
        document.getElementById(GetClientId("listRace")).style.display = "none";

    }

    return false;
}
function ClearRace() {
    document.getElementById(GetClientId("txtRace")).value = "";
}
function ClearGranularity() {
    document.getElementById(GetClientId("txtGranularity")).value = "";
}
function textboxReleave(ctrl, e) {
    document.getElementById(GetClientId("listRace")).style.display = "none";
}
function textboxReleaveGranular(ctrl, e) {
    document.getElementById(GetClientId("ListGranularity")).style.display = "none";
}
function SaveEnable() {
    document.getElementById(GetClientId("btnSave")).disabled = false;
}
function OpenDemographicsFromViewReport() {
    var obj = new Object();
    obj.HumanID = document.getElementById(GetClientId("hdnDemoAccNumber")).value;
    obj.DOOS = document.getElementById(GetClientId("txtDOOS")).value;
    obj.BatchName = document.getElementById(GetClientId("txtBatchName")).value;
    obj.WFObjectID = "";
    returnToParent(obj);
}
function NewCloseWindow() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    if (document.getElementById(GetClientId("txtWorkSetID")).value == "" || document.getElementById(GetClientId("hdnParentScreen")).value == "Review Exception" || document.getElementById(GetClientId("hdnParentScreen")).value == "Update Call Log") {
        CloseWindow();
        return false;
    }
}
function GetRadWindowNew() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    if (oWindow == null) {
        oWindow = $find(ModalWndw);
    }
    return oWindow;
}
function RaceImageButton(sender, args) {
    var ImageType = document.getElementById(GetClientId("hdnimgurl")).value;
    if (ImageType != "DropDown") {
        document.getElementById(GetClientId("listRace")).style.display = "block";
        document.getElementById(GetClientId("listRace")).style.position = "absolute";
        document.getElementById(GetClientId("listRace")).style.width = "155px";
        document.getElementById(GetClientId("hdnimgurl")).value = "DropDown";
    }
    else {
        document.getElementById(GetClientId("listRace")).style.display = "none";
        document.getElementById(GetClientId("listRace")).style.position = "absolute";
        document.getElementById(GetClientId("listRace")).style.width = "155px";
        document.getElementById(GetClientId("hdnimgurl")).value = "";
    }
    SaveEnable();
    return false;
}
function GranularityImageButton(sender, args) {
    var ImageType = document.getElementById(GetClientId("HdnGranular")).value;
    if (ImageType != "DropDown") {
        document.getElementById(GetClientId("ListGranularity")).style.display = "block";
        document.getElementById(GetClientId("ListGranularity")).style.position = "absolute";
        document.getElementById(GetClientId("ListGranularity")).style.width = "120px";
        document.getElementById(GetClientId("HdnGranular")).value = "DropDown";
    }
    else {
        document.getElementById(GetClientId("ListGranularity")).style.display = "none";
        document.getElementById(GetClientId("ListGranularity")).style.position = "absolute";
        document.getElementById(GetClientId("ListGranularity")).style.width = "120px";
        document.getElementById(GetClientId("HdnGranular")).value = "";
    }
    SaveEnable();
    return false;
}
function YesNoCancel() {
    DisplayErrorMessage('420020');
    document.getElementById(GetClientId("hdnMessageType")).value = "";
    saveplanDetails();
    GetRadWindow().close();
}
function WaitCursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function ConfirmHumanDuplicate() {
    if (window.confirm("Patient with the same Name , Date of Birth and Sex exists in the system. Do you want to create a new patient?") == true) {
        document.getElementById(GetClientId("btnsaveDuplicate")).click();
    }
    else { return false; }
}

function QPCDateValidation(sender, args) {
    var EnteredDateLength = parseInt(args._newValue.replace("-", "").replace("-", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").length);
    if (EnteredDateLength != 9 && EnteredDateLength > 0) {
        alert("Please Enter the Date Fully.")
        sender.clear();
        document.getElementById(sender._clientID).focus();
        return false;
    }
    if (EnteredDateLength == 9) {
        if (isNaN(Date.parse($find(GetClientId(sender.get_id().split('_')[2]))._value))) {
            alert('Enter Valid Date!');
            $find(GetClientId(sender.get_id().split('_')[2])).clear();
            $find(GetClientId(sender.get_id().split('_')[2])).focus(true);
            return false;
        }
        if ($find(GetClientId(sender.get_id().split('_')[2]))._value.split('-')[0] == "00") {
            alert('Enter Valid Date!');
            $find(GetClientId(sender.get_id().split('_')[2])).clear();
            $find(GetClientId(sender.get_id().split('_')[2])).focus(true);
            return false;
        }
        validatedate($find(GetClientId(sender.get_id().split('_')[2]))._value, sender.get_id().split('_')[2]);

    }
    $(document.getElementById(GetClientId(sender._clientID))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId(sender._clientID)).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId(sender._clientID))).click(function () {
        $(document.getElementById(GetClientId(sender._clientID))).focus();
    });



}

function parseMyDate(s) {
    var m = ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec'];
    var match = s.match(/(\d+)-([^.]+)-(\d+)/);
    var date = match[1];
    var monthText = match[2];
    var year = match[3];
    var month = m.indexOf(monthText.toLowerCase());
    return new Date(year, month, date);
}

var uPatientId = "";
$(document).ready(function () {
    vRowID = "";
    if (document.getElementById('ctl00_C5POBody_rdbPRI')?.disabled == true) {
        document.getElementById('btnAdd').disabled = true;
        document.getElementById('btnClearAll').disabled = true;
    }
    if (document.getElementById("ctl00_C5POBody_ddlPreferredLanguage")?.value == "" || document.getElementById("ctl00_C5POBody_ddlPreferredLanguage")?.value == "Patient Refuses to answer" || document.getElementById("ctl00_C5POBody_ddlPreferredLanguage")?.value == "Declined to specify" || document.getElementById("ctl00_C5POBody_ddlPreferredLanguage")?.value == "Preferred Language not indicated") {
        document.getElementById("ctl00_C5POBody_chkReqTranslator").checked = false;
        document.getElementById("ctl00_C5POBody_chkReqTranslator").disabled = true;
    }
    if (document.getElementById('ctl00_C5POBody_txtSpecify')?.style?.backgroundColor) {
        document.getElementById('ctl00_C5POBody_txtSpecify').style.backgroundColor = "#BFDBFF";
    }
    //Jira Cap-634
    if (document.getElementById('ctl00_C5POBody_txtSelectinsured')?.disabled) {
        document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = true;
    }
    if (document.getElementById('imginsuredText')?.style?.visibility) {
        document.getElementById('imginsuredText').style.visibility = "hidden";
    }
    if (document.getElementById('ctl00_C5POBody_txtSpecify')?.disabled) {
        document.getElementById('ctl00_C5POBody_txtSpecify').disabled = true;
    }
    document.getElementById('ctl00_C5POBody_txtPlanSearch')?.setAttribute("data-plan-id", "0");
    document.getElementById('ctl00_C5POBody_txtPlanSearch')?.setAttribute("data-carrier-id", "0");
    if (document.getElementById(GetClientId("btnViewUpdateInsurance"))?.style?.display) {
        document.getElementById(GetClientId("btnViewUpdateInsurance")).style.display = "none";
    }
    if (document.getElementById('ctl00_C5POBody_btnaddins')?.disabled) {
        document.getElementById('ctl00_C5POBody_btnaddins').disabled = true;
    }
    document.getElementById("ctl00_C5POBody_txtProviderSearch")?.setAttribute("data-phy-id", "0");
    document.getElementById('ctl00_C5POBody_txtProviderSearch')?.setAttribute("data-phy-details", "0");
    document.getElementById('ctl00_C5POBody_txtProviderSearch')?.setAttribute("data-phy-gridname", "");
    document.getElementById('ctl00_C5POBody_txtProviderSearch')?.setAttribute("data-phy-npi", "");

    $(document.getElementById(GetClientId("dtpPatientDOB"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, maxDate: new Date(), yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("dtpPatientDOB")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("dtpPatientDOB"))).click(function () {
        //$(document.getElementById(GetClientId("dtpPatientDOB"))).focus();
    });

    $(document.getElementById(GetClientId("txtStartdate"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, yearRange: "-120:+100",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("txtStartdate")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("txtStartdate"))).click(function () {
        // $(document.getElementById(GetClientId("txtStartdate"))).focus();
    });

    $(document.getElementById(GetClientId("txtEnddate"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, yearRange: "-120:+100",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("txtEnddate")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("txtEnddate"))).click(function () {
        // $(document.getElementById(GetClientId("txtEnddate"))).focus();
    });

    $("[id*=pbDropdown]").addClass('displaynonestyle');
    uPatientId = document.getElementById("ctl00_C5POBody_HiddenPatientName")?.value?.split('&')[1];
    loadgrid();
    //  scrolify($('#tblpolicyinfo'), 50);

    //Cap - 1583
    if (document.getElementById(GetClientId("ctl00_C5POBody_ddlPatientStatus"))?.value == "ALIVE") {
        $('#ctl00_C5POBody_dtpDateOfDeath').removeClass('Editabletxtbox');
        $('#ctl00_C5POBody_dtpDateOfDeath').addClass('nonEditabletxtbox');
    }
    else {
        $('#ctl00_C5POBody_dtpDateOfDeath').removeClass('nonEditabletxtbox');
        $('#ctl00_C5POBody_dtpDateOfDeath').addClass('Editabletxtbox');
        if (document.getElementById('ctl00_C5POBody_dtpDateOfDeath')?.disabled) {
            document.getElementById('ctl00_C5POBody_dtpDateOfDeath').disabled = false;
        }
    }

});

function loadgrid() {
    //$('#tbodupolicyinfo tr').remove();
    $.ajax({

        type: "POST",
        url: "./frmPatientDemographics.aspx/loadGrid",
        //CAP 319 Bad control character in string literal in JSON 
        //data: JSON.stringify({ "uPatientId": uPatientId }),
        data: JSON.stringify({ "uPatientId": uPatientId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            document.getElementById("tbodupolicyinfo").innerHTML = "";          
            if (data != undefined && data != null)
            {
               //CAP-831 Bad escaped character in JSON at position 353
                if (isValidJSON(data.d))
                {
                    var objdata = JSON.parse(data.d);
               
                     var j = 0;
                    if (objdata.length > 0)
                    {
                        var index = 0;

                        for (var i = 0; i < objdata.length; i++) {
                        j = parseInt(j) + parseInt("1");
                        var vStatus = objdata[i].Active;
                        if (vStatus !== undefined && vStatus.toUpperCase() == "YES") {
                            var vFinalStatus = "Active";
                        }
                        else {
                            var vFinalStatus = "Inactive";
                        }
                        if (objdata[i].Termination_Date == null || objdata[i].Termination_Date == undefined) {
                            var Termination_Date = "";
                        }
                        else {
                            var Termination_Date = objdata[i].Termination_Date;
                        }
                        if (objdata[i].Effective_Start_Date == null || objdata[i].Effective_Start_Date == undefined) {
                            var Effective_Start_Date = "";
                        }
                        else {
                            var Effective_Start_Date = objdata[i].Effective_Start_Date;
                        }
                        if (objdata[i].PCP_Name == null || objdata[i].PCP_Name == undefined) {
                            var PCP_Name = "";
                        }
                        else {
                            var PCP_Name = objdata[i].PCP_Name;
                        }

                        var vsrc;
                        if (document.getElementById('ctl00_C5POBody_rdbPRI').disabled == true) {
                            vsrc = "Resources/editdisabled.png";
                        }
                        else {
                            vsrc = "Resources/edit.gif";
                        }
                        var newRow = document.getElementById('tbodupolicyinfo').insertRow();
                        newRow.innerHTML = "<tr><td style='width: 5%;text-align: center'><img src=" + vsrc + " onclick='Edit(this);'/></td><td style='width: 10%;text-align: center'>" + objdata[i].Insurance_Type + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Plan_Name + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Policy_Holder_ID + "</td><td style='width: 5 %;text-align: center'>" + objdata[i].Relationship + "</td ><td style='width: 10 %;text-align: center'>" + objdata[i].Insured_Name + "</td ><td style='width: 10 %;text-align: center'>" + objdata[i].PCP_Grid_Name + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Specify_Other + "</td><td style='width: 7 %;text-align: center'> " + Effective_Start_Date + "</td><td style='width: 7 %;text-align: center'>" + Termination_Date + "</td><td style='width: 7 %;text-align: center'>" + vFinalStatus + "</td><td style='display:none'>" + objdata[i].Sortorder + "</td><td style='display:none'>" + objdata[i].Plan_ID + "</td><td style='display:none'>" + objdata[i].Id + "</td><td style='display:none'>" + objdata[i].Insured_Human_ID + "</td><td style='display:none'>" + parseInt(j) + "</td><td style='display:none'>" + objdata[i].Relationship_Number + "</td><td style='display:none'>" + objdata[i].Insured_Details + "</td><td style='display:none'>" + objdata[i].CarrierID + "</td><td style='display:none'>" + objdata[i].PCP_ID + "</td><td style='display:none'>" + objdata[i].PCP_Name + "</td><td style='display:none'>" + objdata[i].PCP_Textbox_Name + "</td><td style='display:none'>" + objdata[i].PCP_NPI + "</td><tr>";
                        document.getElementById(GetClientId("txtNoofPolicies")).value = $('#tbodupolicyinfo tr').length;

                    }

                         DisplayActiveInsurance();
                         sortTable();


                    }
                }
                else
                {
                    alert("An error occured while loading patient insurance grid. Please reload the page and try again.");
                }
            }
           

        }

    });
}

function DisplayActiveInsurance() {

    if (document.getElementById('ctl00_C5POBody_chkActiveStatus').checked) {
        for (var k = 0; k < $('#tbodupolicyinfo  tr').length; k++) {

            if ($('#tbodupolicyinfo tr')[k].childNodes[10].innerText.toUpperCase() != "ACTIVE") {
                $('#tbodupolicyinfo tr')[k].style.display = "none";
            }

            else {
                $('#tbodupolicyinfo tr')[k].style.display = "";
            }
        }


    }

    else {
        for (var k = 0; k < $('#tbodupolicyinfo  tr').length; k++) {

            $('#tbodupolicyinfo tr')[k].style.display = "";

        }
    }
}
function chkShowAllChange() {
    Facility = document.getElementById(GetClientId("hdnFacilityName")).value;

    document.getElementById("ctl00_C5POBody_ddlAssignedTo").options.length = 0;
    var checked = "false";
    var vfacilitys = "";
    if (document.getElementById("ctl00_C5POBody_chkshowall").checked)
        checked = "true";
    $.ajax({
        type: "POST",
        url: "frmPatientCommunication.aspx/laodAssigned",
        data: JSON.stringify({
            "chkshowall": checked,
            "facility": Facility
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            if (objdata.AssignedTo.length > 0) {
                var vAssignedTo = objdata.AssignedTo;
                if (vAssignedTo != null && vAssignedTo.length > 0) {
                    $('#ctl00_C5POBody_ddlAssignedTo').append("<option value=''>" + "" + "</option>");
                    var vddlAssignedTo = document.getElementById("ctl00_C5POBody_ddlAssignedTo");
                    var opt = document.createElement("option");
                    for (var i = 0; i < vAssignedTo.length; i++) {
                        var opt = document.createElement("option");
                        opt.text = vAssignedTo[i].split('|')[1];
                        opt.value = vAssignedTo[i].split('|')[0];
                        opt.title = vAssignedTo[i].split('|')[1];
                        vddlAssignedTo.options.add(opt);
                    }
                }
            }
        },
        failure: function (data) {
            alert(data.d);
        }
    });

}

//Add Insurance Policy Change.

function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    oTbl.attr("data-item-original-width", oTbl.width());
    oTbl.find('thead tr td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    newTbl.width(newTbl.attr('data-item-original-width'));
    newTbl.find('thead tr td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    oTbl.width(oTbl.attr('data-item-original-width'));
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    if (tblAsJQueryObject[0] != undefined) {
        if (tblAsJQueryObject[0].parentElement.parentElement.id == "GeneralQTable") {
            $("#ScrollIDGeneral").css('height', '');
            $("#ScrollIDGeneral").css('overflow-y', '');
        }
        else {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
        }
    }
}

function OpenAddinsured() {

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            var childWindow = oWnd.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + 0 + "&DisableFindPat=TRUE&Functionality=ADDPATIENT", "ctl00_DemographicsAddInsured");
            setRadWindowProperties(childWindow, 1230, 1130);
            childWindow.add_close(QuickPatientClickAddPatient);

        }, 0);
    return false;
}
function QuickPatientClickAddPatient(oWindow, args) {

    var result = args.get_argument();
    oWindow.remove_close(QuickPatientClickAddPatient);
    if (result) {
        var Result = new Object();

        document.getElementById("ctl00_C5POBody_txtSelectinsured").value = result.PatientName + "|DOB: " + result.PatientDOB + "|" + result.PatientGender + "| ACC#:" + result.Human_id + "| PATIENT TYPE:" + result.HumanType;
        document.getElementById('ctl00_C5POBody_txtSelectinsured').attributes['data-human-id'].value = result.Human_id;
        document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = true;
        document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#BFDBFF"




        //Result.HumanId = result.Human_id;
        //Result.PatientName = result.PatientName;
        //Result.PatientDOB = result.PatientDOB;
        //Result.Encounter_Provider_ID = result.Encounter_Provider_ID;
        //Result.Cell_Phone = result.Cell_Phone;
        //Result.Home_Phone = result.Home_Phone;
        //Result.HumanType = result.PatientType;
        //Result.IsNewPatient = result.IsNewPatient;
        //Result.IsQuickPatient = result.IsQuickPatient;
        //Result.PatientGender = result.PatientGender;

    }
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

function setplanDetailssession() {
    var data = [];

    $('#tbodupolicyinfo').find('tr').each(function (rowIndex, r) {
        var cols = [];
        $(this).find('td').each(function (colIndex, c) {
            if (parseInt(colIndex) > 0) {
                cols.push(c.textContent);
            }
        });
        data.push(cols);
    });
    localStorage.setItem("PlanDetails", JSON.stringify(data));

}
function saveplanDetails() {


    $.ajax({
        type: "POST",
        url: "./frmPatientDemographics.aspx/SavePlanData",
        data: JSON.stringify({
            name: JSON.parse(localStorage.getItem("PlanDetails")),
            Human_id: document.getElementById(GetClientId("txtAccountNo")).value

        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {

            document.getElementById("ctl00_C5POBody_txtSelectinsured").value = document.getElementById("ctl00_C5POBody_HiddenPatientName").value.split('&')[0];
            document.getElementById('ctl00_C5POBody_txtSelectinsured').attributes['data-human-id'].value = document.getElementById("ctl00_C5POBody_HiddenPatientName").value.split('&')[1];
            document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = true;
            document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#BFDBFF"
            document.getElementById('imginsuredText').display = "none";
            document.getElementById('ctl00_C5POBody_btnaddins').disabled = true;
            document.getElementById('imginsuredText').style.visibility = "hidden";// $('#tbodupolicyinfo tr').remove();

            //  $('#tbodupolicyinfo tr').remove();
            loadgrid();
            document.getElementById(GetClientId("txtNoofPolicies")).value = $('#tbodupolicyinfo tr').length;
        },
        failure: function (data) {
            alert(data.d);
        }
    });
}
var editinsurancetype = '';
function btnaddinsured(e) {
    //Cap - 1369
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); };

    var PriChecked = document.getElementById("ctl00_C5POBody_rdbPRI").checked;
    var SecChecked = document.getElementById("ctl00_C5POBody_rdbSEC").checked;
    var TerChecked = document.getElementById("ctl00_C5POBody_rdbTER").checked;

    //Cap - 1359 - Commented for this bug
    //Cap - 883
    //if (document.getElementById("btnAdd").value == 'Update') {
    //    if (document.getElementById("ctl00_C5POBody_rdStatusactive").checked == true && !PriChecked && !SecChecked && !TerChecked) {
    //        if (bOldCheck == true && vOldPriority != null) {
    //            if (vOldPriorit.includes("PRIMARY")) {
    //                document.getElementById("ctl00_C5POBody_rdbPRI").checked = true;
    //            }
    //            if (vOldPriorit.includes("SECONDARY")) {
    //                document.getElementById("ctl00_C5POBody_rdbSEC").checked = true;
    //            }
    //            if (vOldPriorit.includes("TERTIARY")) {
    //                document.getElementById("ctl00_C5POBody_rdbTER").checked = true;
    //            }
    //        }
    //    }
    //}

    //Cap - 182
    if (PriChecked && document.getElementById("ctl00_C5POBody_txtProviderSearch") && document.getElementById("ctl00_C5POBody_txtProviderSearch") != '' && document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'].value != null) {
        document.getElementById(GetClientId("txtPCPProviderTag")).value = document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'].value;
    }
    else {
        document.getElementById(GetClientId("txtPCPProviderTag")).value = 0;
    }


    if (document.getElementById("ctl00_C5POBody_txtPlanSearch").attributes['data-plan-id']) {
        var PlanVal = document.getElementById("ctl00_C5POBody_txtPlanSearch").attributes['data-plan-id'].value;
        var carrierVal = document.getElementById("ctl00_C5POBody_txtPlanSearch").attributes['data-carrier-id'].value;
    }
    else {
        var PlanVal = 0;
        var carrierVal = 0;
    }
    if (document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'] && document.getElementById("ctl00_C5POBody_txtProviderSearch").value != "") {
        var vPcpId = document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'].value;
    }
    else {
        var vPcpId = 0;
    }

    if (vPcpId != '0' && document.getElementById("ctl00_C5POBody_txtProviderSearch").value != "") {
        var vPCPName = document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-gridname'].value;
    }
    else {
        var vPCPName = "";
    }
    if (vPcpId != '0' && document.getElementById("ctl00_C5POBody_txtProviderSearch").value != "") {
        var vPcpnpi = document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-npi'].value;
    }
    else {
        var vPcpnpi = "";
    }
    if (vPcpId != '0' && document.getElementById("ctl00_C5POBody_txtProviderSearch").value != "") {
        var vProviderName = document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-details'].value;
    }
    else {
        var vProviderName = "";
    }

    var vProviderFullName = document.getElementById("ctl00_C5POBody_txtProviderSearch").value;
    var planname = document.getElementById("ctl00_C5POBody_txtPlanSearch").value;
    var insurehumanid = document.getElementById("ctl00_C5POBody_txtSelectinsured").attributes['data-human-id'].value;

    var insurename = document.getElementById("ctl00_C5POBody_txtSelectinsured").value.split('|')[0];
    var PolicyVal = document.getElementById("ctl00_C5POBody_txtPolicyholderid").value;
    var SpecificVal = document.getElementById("ctl00_C5POBody_txtSpecify").value;
    var EffStartDate = document.getElementById("ctl00_C5POBody_txtStartdate").value;
    var EffEndDate = document.getElementById("ctl00_C5POBody_txtEnddate").value;
    var InsuredFullname = document.getElementById("ctl00_C5POBody_txtSelectinsured").value;
    var RelationVal = document.getElementById("ctl00_C5POBody_ddlPatientRelation");
    var insuranceType = '';
    if (document.getElementById("ctl00_C5POBody_rdbPRI").checked)
        insuranceType = "PRIMARY";
    else if (document.getElementById("ctl00_C5POBody_rdbSEC").checked)
        insuranceType = "SECONDARY";

    else if (document.getElementById("ctl00_C5POBody_rdbTER").checked)
        insuranceType = "TERTIARY";
    //else {
    //    insuranceType = editinsurancetype;
    //}

    var status = "";
    //Jira #Cap-255 - old Primary is changed as Primary, Status change active
    //if (document.getElementById("ctl00_C5POBody_rdStatusactive").checked )
    if (document.getElementById("ctl00_C5POBody_rdStatusactive").checked ||
        (bOldCheck == true && (document.getElementById("ctl00_C5POBody_rdbPRI").checked == true
            || document.getElementById("ctl00_C5POBody_rdbSEC").checked == true
            || document.getElementById("ctl00_C5POBody_rdbTER").checked == true))) {
        status = "Active";
    }
    else if (document.getElementById("ctl00_C5POBody_rdStatusinactive").checked)
        status = "Inactive";
    var human_id = document.getElementById(GetClientId("txtAccountNo")).value;

    var patientname = document.getElementById(GetClientId("txtPatientlastname")).value + "," + document.getElementById(GetClientId("txtPatientfirstname")).value
    //Cap - 1359
    //if (document.getElementById("btnAdd").value == 'Add')
    if (document.getElementById("btnAdd").value == 'Add' || document.getElementById("ctl00_C5POBody_rdStatusactive").checked == true) {
        if (PriChecked == false && SecChecked == false && TerChecked == false) {
            DisplayErrorMessage('410006');
            //CAP-Demographics loading
            setTimeout(function () { { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); } }, 1000);
            return false;
        }
    }

    if (PlanVal == "0") {
        DisplayErrorMessage('380028');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }


    if (document.getElementById("ctl00_C5POBody_txtPlanSearch").value == "PAYER NOT FOUND" && document.getElementById("ctl00_C5POBody_txtSpecify").value == "") {
        DisplayErrorMessage('410030');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (PolicyVal == "") {
        DisplayErrorMessage('410031');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }



    if (RelationVal.options[RelationVal.selectedIndex].text == "") {
        DisplayErrorMessage('380051');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }


    if (RelationVal.options[RelationVal.selectedIndex].text.toUpperCase() != "SELF" && insurehumanid == "0") {
        DisplayErrorMessage('420043');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (EffStartDate == "" && EffEndDate != "") {
        DisplayErrorMessage('380016');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (Date.parse(EffStartDate) > Date.parse(EffEndDate)) {
        DisplayErrorMessage('410033');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }


    if (document.getElementById("ctl00_C5POBody_txtStartdate").value.length != 0) {
        if (document.getElementById("ctl00_C5POBody_txtStartdate").value != "__-___-____") {
            if (DateValidattion("ctl00_C5POBody_txtStartdate") == false) {
                DisplayErrorMessage('350010');
                //Cap - 1369
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        else {
            EffStartDate = "";
        }
    }


    if (document.getElementById("ctl00_C5POBody_txtEnddate").value.length != 0) {
        if (document.getElementById("ctl00_C5POBody_txtEnddate").value != "__-___-____") {
            if (DateValidattion("ctl00_C5POBody_txtEnddate") == false) {

                DisplayErrorMessage('350011');
                //Cap - 1369
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        else {
            EffEndDate = "";
        }
    }
    if (vProviderFullName != "" && vPcpId == 0) {
        DisplayErrorMessage('350013');
        //Cap - 1369
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("btnAdd").value == 'Add') {
        vRowID = "";
    }
    for (var k = 0; k < $('#tbodupolicyinfo  tr').length; k++) {
        //Jira #Cap-264 - If the Payer not found plan is added with 2 different "specify other" it should not be considered as duplicate
         if (document.getElementById("ctl00_C5POBody_txtPlanSearch").value != "PAYER NOT FOUND") {
            if (vRowID != $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[11].innerText) {
                if (RelationVal.options[RelationVal.selectedIndex].text == "SELF") {
                    //Jira #CAP-141 - Remove Relationship to Patient from the Matching criteria && Jira #CAP-146 - Able to add duplicate insurance
                    //if ($('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[2].innerText == planname.trim() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[3].innerText == PolicyVal.trim() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[4].innerText == RelationVal.options[RelationVal.selectedIndex].text) {
                    if ($('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[2].innerText.toUpperCase() == planname.trim().toUpperCase() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[3].innerText.toUpperCase() == PolicyVal.trim().toUpperCase() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[14].innerText.trim() == insurehumanid.trim()) {
                        DisplayErrorMessage('350014');
                        //Cap - 1369
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                }
                else {
                    //Jira #CAP-141 - Remove Relationship to Patient from the Matching criteria  && Jira #CAP-146 - Able to add duplicate insurance
                    //if ($('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[2].innerText == planname.trim() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[3].innerText == PolicyVal.trim() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[4].innerText == RelationVal.options[RelationVal.selectedIndex].text && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[5].innerText == insurename.trim()) {
                    if ($('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[2].innerText.toUpperCase() == planname.trim().toUpperCase() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[3].innerText.toUpperCase() == PolicyVal.trim().toUpperCase() && $('#tbodupolicyinfo tr')[k].getElementsByTagName('td')[14].innerText.trim() == insurehumanid.trim()) {
                        DisplayErrorMessage('350014');
                        //Cap - 1369
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                }
            }
        }
    }

    var id = "0";

    $.ajax({
        type: "POST",
        url: "./frmPatientDemographics.aspx/SetInsuranceType",
        data: JSON.stringify({
            "humanid": human_id,
            "insuranceType": insuranceType,
            "id": id,
            "policyholderid": PolicyVal,
            "active": "Yes",
            "PatientName": patientname,
            "insurehumanid": insurehumanid,
            "insid": PlanVal,
            "Effective_Start_Date": EffStartDate,
            "Termination_Date": EffEndDate,
            "relationship": RelationVal.options[RelationVal.selectedIndex].text

        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);

            //if ($('#tblpolicyinfo tbody tr').length == 1) {
            //    $('#tblpolicyinfo tbody tr').remove();
            //}
            //else {
            var sortordernew = 0;
            var sortorderexists = 0;
            if (objdata.ValidationError != undefined && objdata.ValidationError == "Success") {

                if (objdata.Sortorderlookup != undefined && objdata.Sortorderlookup.length > 0) {
                    for (var j = 0; j < objdata.Sortorderlookup.length; j++) {
                        if (objdata.Sortorderlookup[j].Value == insuranceType) {
                            sortordernew = objdata.Sortorderlookup[j].Sort_Order;
                            break;
                        }
                    }
                    for (var j = 0; j < objdata.Sortorderlookup.length; j++) {
                        if (objdata.Sortorderlookup[j].Value == "OLD " + insuranceType) {
                            sortorderexists = objdata.Sortorderlookup[j].Sort_Order;
                            break;
                        }
                    }

                }
                var tablelength = $('#tbodupolicyinfo  tr').length;
                if (e.value.toUpperCase() == "ADD") {

                    var maxvalue = 0;
                    // if (document.getElementById('ctl00_C5POBody_chkActiveStatus').checked && status.toUpperCase() == "ACTIVE") {
                    for (var j = 0; j < tablelength; j++) {

                        if (parseInt($('#tbodupolicyinfo tr')[j].childNodes[15].innerText) > parseInt(maxvalue))

                            maxvalue = parseInt($('#tbodupolicyinfo tr')[j].childNodes[15].innerText);

                        if ($('#tbodupolicyinfo tr')[j].childNodes[1].innerText == insuranceType) {

                            $('#tbodupolicyinfo tr')[j].childNodes[1].innerText = "OLD " + $('#tbodupolicyinfo tr')[j].childNodes[1].innerText
                            $('#tbodupolicyinfo tr')[j].childNodes[10].innerText = "Inactive"
                            $('#tbodupolicyinfo tr')[j].childNodes[11].innerText = sortorderexists;

                        }
                    }
                    // }

                    var newRow = document.getElementById('tbodupolicyinfo').insertRow();
                    newRow.innerHTML = "<tr><td style='width: 5%;text-align: center'><img src='Resources/edit.gif' onclick='Edit(this);'/></td><td style='width: 10%;text-align: center'>" + insuranceType + "</td><td style='width: 10 %;text-align: center'>" + planname + "</td><td style='width: 10 %;text-align: center'>" + PolicyVal + "</td><td style='width: 5 %;text-align: center'>" + RelationVal.options[RelationVal.selectedIndex].text + "</td ><td style='width: 10%;text-align: center'>" + insurename + "</td ><td style='width: 10 %;text-align: center'>" + vPCPName + "</td><td style='width: 10 %;text-align: center'>" + SpecificVal + "</td><td style='width: 7 %;text-align: center'> " + EffStartDate + "</td><td style='width: 7 %;text-align: center'>" + EffEndDate + "</td><td style='width: 7 %;text-align: center'>" + status + "</td><td style='display:none'>" + sortordernew + "</td><td style='display:none'>" + PlanVal + "</td><td style='display:none'>" + id + "</td><td style='display:none'>" + insurehumanid + "</td><td style='display:none'>" + parseInt(parseInt(maxvalue) + parseInt("1")) + "</td><td style='display:none'>" + RelationVal.options[RelationVal.selectedIndex].value + "</td><td style='display:none'>" + InsuredFullname + "</td><td style='display:none'>" + carrierVal + "</td><td style='display:none'>" + vPcpId + "</td><td style='display:none'>" + vProviderName + "</td><td style='display:none'>" + vProviderFullName + "</td><td style='display:none'>" + vPcpnpi + "</td><tr>";
                    document.getElementById(GetClientId("txtNoofPolicies")).value = $('#tbodupolicyinfo tr').length;
                    //Cap - 1369
                    //btnclearinsured(false);
                    setTimeout(btnclearinsured(false), 1000);
                    //}
                    //else {
                    //   // btnclearinsured(false);
                    //}
                    DisplayActiveInsurance();
                    sortTable();
                }
                else {


                    for (var j = 0; j < tablelength; j++) {
                        //if ($('#tblpolicyinfo tr')[j].childNodes[11].innerText != "0") {

                        //    $('#tblpolicyinfo tr')[j].remove();
                        //}



                        if ($('#tbodupolicyinfo tr')[j].childNodes[1].innerText == insuranceType && $('#tbodupolicyinfo tr')[j].childNodes[15].innerText != document.getElementById("ctl00_C5POBody_hdnpatinsuredid").value) {

                            $('#tbodupolicyinfo tr')[j].childNodes[1].innerText = "OLD " + $('#tbodupolicyinfo tr')[j].childNodes[1].innerText
                            $('#tbodupolicyinfo tr')[j].childNodes[10].innerText = "Inactive"
                            $('#tbodupolicyinfo tr')[j].childNodes[11].innerText = sortorderexists;

                        }

                        if ($('#tbodupolicyinfo tr')[j].childNodes[15].innerText == document.getElementById("ctl00_C5POBody_hdnpatinsuredid").value) {
                            if (insuranceType != "")
                                $('#tbodupolicyinfo tr')[j].childNodes[1].innerText = insuranceType;
                            $('#tbodupolicyinfo tr')[j].childNodes[2].innerText = planname;
                            $('#tbodupolicyinfo tr')[j].childNodes[3].innerText = PolicyVal;
                            $('#tbodupolicyinfo tr')[j].childNodes[4].innerText = RelationVal.options[RelationVal.selectedIndex].text;
                            $('#tbodupolicyinfo tr')[j].childNodes[5].innerText = insurename;
                            $('#tbodupolicyinfo tr')[j].childNodes[6].innerText = vPCPName;
                            $('#tbodupolicyinfo tr')[j].childNodes[7].innerText = SpecificVal;

                            $('#tbodupolicyinfo tr')[j].childNodes[8].innerText = EffStartDate;
                            $('#tbodupolicyinfo tr')[j].childNodes[9].innerText = EffEndDate;
                            $('#tbodupolicyinfo tr')[j].childNodes[10].innerText = status;
                            if (parseInt(sortordernew) != 0)
                                $('#tbodupolicyinfo tr')[j].childNodes[11].innerText = sortordernew;
                            $('#tbodupolicyinfo tr')[j].childNodes[12].innerText = PlanVal;
                            $('#tbodupolicyinfo tr')[j].childNodes[14].innerText = insurehumanid;
                            $('#tbodupolicyinfo tr')[j].childNodes[16].innerText = RelationVal.options[RelationVal.selectedIndex].value;
                            $('#tbodupolicyinfo tr')[j].childNodes[17].innerText = InsuredFullname;
                            $('#tbodupolicyinfo tr')[j].childNodes[18].innerText = carrierVal;
                            $('#tbodupolicyinfo tr')[j].childNodes[19].innerText = vPcpId;
                            $('#tbodupolicyinfo tr')[j].childNodes[20].innerText = vProviderName;
                            $('#tbodupolicyinfo tr')[j].childNodes[21].innerText = vProviderFullName;
                            $('#tbodupolicyinfo tr')[j].childNodes[22].innerText = vPcpnpi;
                            //Cap - 1369
                            //btnclearinsured(false);
                            setTimeout(btnclearinsured(false), 1000);
                        }

                    }
                    DisplayActiveInsurance();
                    sortTable();
                }
            }

            else if (objdata.ValidationError != undefined) {
                if (objdata.ValidationError.indexOf("Policy Holder ID is Invalid") > -1) {
                    var errmsgnumber = objdata.ValidationError.slice(objdata.ValidationError.indexOf('$@') + 2, objdata.ValidationError.length);

                    DisplayErrorMessage('380057', '', errmsgnumber);
                    //Cap - 1369
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }

                //alert(objdata.ValidationError);
            }
            //if (objdata.length > 0) {
            //    for (var i = 0; i < objdata.length; i++) {
            //        var vStatus = objdata[i].Active;
            //        if (vStatus == "Yes") {
            //            var vFinalStatus = "Active";
            //        }
            //        else {
            //            var vFinalStatus = "Inactive";
            //        }
            //        if (objdata[i].Termination_Date == null || objdata[i].Termination_Date != undefined) {
            //            var Termination_Date = "";
            //        }
            //        else {
            //            var Termination_Date = objdata[i].Termination_Date;
            //        }
            //        if (objdata[i].Effective_Start_Date == null || objdata[i].Effective_Start_Date != undefined) {
            //            var Effective_Start_Date = "";
            //        }
            //        else {
            //            var Effective_Start_Date = objdata[i].Effective_Start_Date;
            //        }
            //        if (document.getElementById('ctl00_C5POBody_chkActiveStatus').checked && vFinalStatus == "Active") {
            //            var newRow = document.getElementById('tblpolicyinfo').insertRow();
            //            newRow.innerHTML = "<tr><td style='width: 5%;text-align: center'></td><td style='width: 10%;text-align: center'>" + objdata[i].Insurance_Type + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Plan_Name + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Policy_Holder_ID + "</td><td style='width: 5 %;text-align: center'>" + objdata[i].Relationship + "</td ><td style='width: 15 %;text-align: center'>" + objdata[i].Insured_Name + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Insured_Name + "</td><td style='width: 7 %;text-align: center'> " + Effective_Start_Date + "</td><td style='width: 7 %;text-align: center'>" + Termination_Date + "</td><td style='width: 7 %;text-align: center'>" + vFinalStatus + "</td><td style='display:none'>" + objdata[i].Plan_ID + "</td><td style='display:none'>" + objdata[i].Id + "</td><tr>";
            //        }
            //        else if (!document.getElementById('ctl00_C5POBody_chkActiveStatus').checked && vFinalStatus != "Active") {
            //            var newRow = document.getElementById('tblpolicyinfo').insertRow();
            //            newRow.innerHTML = "<tr><td style='width: 5%;text-align: center'></td><td style='width: 10%;text-align: center'>" + objdata[i].Insurance_Type + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Plan_Name + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Policy_Holder_ID + "</td><td style='width: 5 %;text-align: center'>" + objdata[i].Relationship + "</td ><td style='width: 15 %;text-align: center'>" + objdata[i].Insured_Name + "</td><td style='width: 10 %;text-align: center'>" + objdata[i].Insured_Name + "</td><td style='width: 7 %;text-align: center'> " + Effective_Start_Date + "</td><td style='width: 7 %;text-align: center'>" + Termination_Date + "</td><td style='width: 7 %;text-align: center'>" + vFinalStatus + "</td><td style='display:none'>" + objdata[i].Plan_ID + "</td><td style='display:none'>" + objdata[i].Id + "</td><tr>";
            //        }
            //    }
            //}
            //else {
            //    var newRow = document.getElementById('tblpolicyinfo').insertRow();
            //    newRow.innerHTML = "<tr>No Data Found</tr>";
            //}

            document.getElementById("ctl00_C5POBody_txtProviderSearch").setAttribute("data-phy-id", "0");
            document.getElementById('ctl00_C5POBody_txtProviderSearch').setAttribute("data-phy-details", "0");
            document.getElementById('ctl00_C5POBody_txtProviderSearch').setAttribute("data-phy-gridname", "");
            document.getElementById('ctl00_C5POBody_txtProviderSearch').setAttribute("data-phy-npi", "");
        },
        failure: function (data) {
            alert(data.d);
        }
    });
}

function Edit(e) {

    bOldCheck = false;
    if (document.getElementById('ctl00_C5POBody_rdbPRI').disabled == true) {
        return false;
    }
    //Jira #Cap-255 - old Primary is changed as Primary, Status change active
    if (e.parentElement.parentElement.childNodes[1].innerText.includes("OLD")) {
        bOldCheck = true;
        //Cap - 883
        vOldPriorit = e.parentElement.parentElement.childNodes[1].innerText;
    }

    // editinsurancetype = e.parentElement.parentElement.childNodes[1].innerText;
    if (e.parentElement.parentElement.childNodes[1].innerText == "PRIMARY")
        document.getElementById("ctl00_C5POBody_rdbPRI").checked = true;
    else if (e.parentElement.parentElement.childNodes[1].innerText == "SECONDARY")
        document.getElementById("ctl00_C5POBody_rdbSEC").checked = true;
    else if (e.parentElement.parentElement.childNodes[1].innerText == "TERTIARY")
        document.getElementById("ctl00_C5POBody_rdbTER").checked = true;
    else {
        document.getElementById("ctl00_C5POBody_rdbPRI").checked = false;
        document.getElementById("ctl00_C5POBody_rdbSEC").checked = false;
        document.getElementById("ctl00_C5POBody_rdbTER").checked = false;
    }

    vRowID = e.parentElement.parentElement.childNodes[11].innerText;
    document.getElementById("ctl00_C5POBody_txtPlanSearch").attributes['data-plan-id'].value = e.parentElement.parentElement.childNodes[12].innerText;
    document.getElementById("ctl00_C5POBody_txtPlanSearch").attributes['data-carrier-id'].value = e.parentElement.parentElement.childNodes[18].innerText;
    document.getElementById("ctl00_C5POBody_txtPlanSearch").value = e.parentElement.parentElement.childNodes[2].innerText;
    document.getElementById('ctl00_C5POBody_txtPlanSearch').style.backgroundColor = "#BFDBFF";
    document.getElementById('ctl00_C5POBody_txtPlanSearch').disabled = true;
    document.getElementById('ctl00_C5POBody_txtPolicyholderid').style.backgroundColor = "#BFDBFF";
    document.getElementById('ctl00_C5POBody_txtPolicyholderid').disabled = true;
    document.getElementById('ctl00_C5POBody_ddlPatientRelation').style.backgroundColor = "#BFDBFF";
    document.getElementById('ctl00_C5POBody_ddlPatientRelation').disabled = true;
    document.getElementById('ctl00_C5POBody_imgClearplanText').style.visibility = "hidden";
    document.getElementById("ctl00_C5POBody_txtSelectinsured").attributes['data-human-id'].value = e.parentElement.parentElement.childNodes[14].innerText
    document.getElementById("ctl00_C5POBody_txtSelectinsured").value = e.parentElement.parentElement.childNodes[17].innerText;
    document.getElementById("ctl00_C5POBody_txtPolicyholderid").value = e.parentElement.parentElement.childNodes[3].innerText;
    document.getElementById("ctl00_C5POBody_txtSpecify").value = e.parentElement.parentElement.childNodes[7].innerText;

    document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = true;
    document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#BFDBFF";
    document.getElementById('imginsuredText').display = "none";
    document.getElementById('ctl00_C5POBody_btnaddins').disabled = true;
    document.getElementById('imginsuredText').style.visibility = "hidden";

    if (e.parentElement.parentElement.childNodes[6].innerText != "") {
        document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'].value = e.parentElement.parentElement.childNodes[19].innerText;
        document.getElementById('ctl00_C5POBody_txtProviderSearch').attributes['data-phy-gridname'].value = e.parentElement.parentElement.childNodes[6].innerText;
        document.getElementById('ctl00_C5POBody_txtProviderSearch').attributes['data-phy-details'].value = e.parentElement.parentElement.childNodes[20].innerText
        document.getElementById('ctl00_C5POBody_txtProviderSearch').attributes['data-phy-npi'].value = e.parentElement.parentElement.childNodes[22].innerText;
        document.getElementById("ctl00_C5POBody_txtProviderSearch").value = e.parentElement.parentElement.childNodes[21].innerText;
        document.getElementById('ctl00_C5POBody_txtProviderSearch').disabled = true;
        document.getElementById('ctl00_C5POBody_txtProviderSearch').style.backgroundColor = "#BFDBFF";
    }
    //jira - #Cap200 - PCP of the Previous edit is populated
    else {
        document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'].value = "";
        document.getElementById('ctl00_C5POBody_txtProviderSearch').attributes['data-phy-gridname'].value = "";
        document.getElementById('ctl00_C5POBody_txtProviderSearch').attributes['data-phy-details'].value = "";
        document.getElementById('ctl00_C5POBody_txtProviderSearch').attributes['data-phy-npi'].value = "";
        document.getElementById("ctl00_C5POBody_txtProviderSearch").value = "";
        document.getElementById('ctl00_C5POBody_txtProviderSearch').disabled = false;
        document.getElementById('ctl00_C5POBody_txtProviderSearch').style.backgroundColor = "#FFFFFF";
    }
    if (e.parentElement.parentElement.childNodes[8].innerText != "") {
        document.getElementById("ctl00_C5POBody_txtStartdate").value = e.parentElement.parentElement.childNodes[8].innerText;
    }
    else {
        document.getElementById("ctl00_C5POBody_txtStartdate").value = "__-___-____";
    }
    if (e.parentElement.parentElement.childNodes[9].innerText != "") {
        document.getElementById("ctl00_C5POBody_txtEnddate").value = e.parentElement.parentElement.childNodes[9].innerText;
    }
    else {
        document.getElementById("ctl00_C5POBody_txtEnddate").value = "__-___-____";
    }

    //var RelationVal = document.getElementById("ctl00_C5POBody_ddlPatientRelation");
    //RelationVal.options[RelationVal.selectedIndex].text = e.parentElement.parentElement.childNodes[4].innerText;
    var dd = document.getElementById("ctl00_C5POBody_ddlPatientRelation");
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text == e.parentElement.parentElement.childNodes[4].innerText) {
            dd.selectedIndex = i;
            break;
        }
    }

    if (e.parentElement.parentElement.childNodes[4].innerText == "SELF") {
        $('#lblSelectInsured').removeClass('manredforstar');
        $('#lblSelectInsured').addClass('spanstyle')
        document.getElementById("lblSelectInsured").innerHTML = "Select Insured";
        document.getElementById('ctl00_C5POBody_txtSelectinsured').value = "";
    }
    else {
        $('#lblSelectInsured').removeClass('spanstyle');
        $('#lblSelectInsured').addClass('manredforstar');
        document.getElementById("lblSelectInsured").innerHTML = "Select Insured*";

    }
    document.getElementById("ctl00_C5POBody_hdnpatinsuredid").value = e.parentElement.parentElement.childNodes[15].innerText;
    document.getElementById("ctl00_C5POBody_Hdnsortorder").value = e.parentElement.parentElement.childNodes[11].innerText;
    var status = e.parentElement.parentElement.childNodes[10].innerText;

    if (status.toUpperCase() == "ACTIVE")
        document.getElementById("ctl00_C5POBody_rdStatusactive").checked = true;
    else if (status.toUpperCase() == "INACTIVE")
        document.getElementById("ctl00_C5POBody_rdStatusinactive").checked = true;


    DisplayActiveInsurance();

    $('#btnAdd').val("Update");

    $('#btnClearAll').val("Cancel");


}
function btnclearinsured(btnclearAll) {
    if (btnclearAll == true) {
        var clearall;
        if (document.getElementById("btnClearAll").value == "Clear All") {
            clearall = window.confirm('Are you sure you want to clear all the fields?');
        }
        else {
            clearall = window.confirm('Would you like to Cancel without saving?');
        }
        if (clearall == false) {
            return false;
        }

    }

    document.getElementById("ctl00_C5POBody_ddlPatientRelation").selectedIndex = 0;
    document.getElementById("ctl00_C5POBody_txtSelectinsured").value = document.getElementById("ctl00_C5POBody_HiddenPatientName").value.split('&')[0];
    document.getElementById("ctl00_C5POBody_txtSelectinsured").setAttribute("data-human-id", document.getElementById("ctl00_C5POBody_HiddenPatientName").value.split('&')[1]);
    document.getElementById('ctl00_C5POBody_rdbPRI').checked = false;
    document.getElementById('ctl00_C5POBody_rdbSEC').checked = false;
    document.getElementById('ctl00_C5POBody_rdbTER').checked = false;
    document.getElementById('ctl00_C5POBody_rdStatusactive').checked = true;
    document.getElementById('ctl00_C5POBody_rdStatusinactive').checked = false;
    document.getElementById("ctl00_C5POBody_txtPlanSearch").value = "";
    document.getElementById('ctl00_C5POBody_txtPlanSearch').style.backgroundColor = "#FFFFFF";
    document.getElementById('ctl00_C5POBody_txtPlanSearch').setAttribute("data-plan-id", "0");
    document.getElementById('ctl00_C5POBody_txtPlanSearch').setAttribute("data-carrier-id", "0");
    document.getElementById('ctl00_C5POBody_txtPlanSearch').disabled = false;
    document.getElementById('ctl00_C5POBody_txtProviderSearch').disabled = false;
    document.getElementById('ctl00_C5POBody_txtProviderSearch').value = "";
    document.getElementById('ctl00_C5POBody_txtProviderSearch').style.backgroundColor = "#FFFFFF";
    document.getElementById("ctl00_C5POBody_txtPolicyholderid").value = "";
    document.getElementById("ctl00_C5POBody_txtStartdate").value = "__-___-____";
    document.getElementById("ctl00_C5POBody_txtEnddate").value = "__-___-____";
    document.getElementById("ctl00_C5POBody_txtSpecify").value = "";
    document.getElementById('ctl00_C5POBody_txtSelectinsured').value = "";
    document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = true;
    document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#BFDBFF"
    document.getElementById('ctl00_C5POBody_txtSpecify').disabled = true;
    document.getElementById('ctl00_C5POBody_txtSpecify').style.backgroundColor = "#BFDBFF"
    document.getElementById('imginsuredText').display = "none";
    document.getElementById('ctl00_C5POBody_imgClearplanText').style.visibility = "visible";
    document.getElementById('ctl00_C5POBody_btnaddins').disabled = true;
    $('#lblSelectInsured').removeClass('manredforstar');
    $('#lblSelectInsured').addClass('spanstyle');
    document.getElementById("lblSelectInsured").innerHTML = "Select Insured";
    $('#lblSpecifyOther').removeClass('manredforstar');
    $('#lblSpecifyOther').addClass('spanstyle');
    document.getElementById('ctl00_C5POBody_txtPolicyholderid').style.backgroundColor = "#FFFFFF";
    document.getElementById('ctl00_C5POBody_txtPolicyholderid').disabled = false;
    document.getElementById('ctl00_C5POBody_ddlPatientRelation').style.backgroundColor = "#FFFFFF";
    document.getElementById('ctl00_C5POBody_ddlPatientRelation').disabled = false;
    document.getElementById("lblSpecifyOther").innerHTML = "Specify Other";
    $('#btnAdd').val("Add");
    $('#btnClearAll').val("Clear All");
    //Cap - 1369
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function sortTable() {
    var table, rows, switching, i, x, y, shouldSwitch;
    table = document.getElementById("tbodupolicyinfo");
    switching = true;
    /*Make a loop that will continue until
    no switching has been done:*/
    while (switching) {
        //start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /*Loop through all table rows (except the
        first, which contains table headers):*/
        for (i = 0; i < (rows.length - 1); i++) {
            //start by saying there should be no switching:
            shouldSwitch = false;
            /*Get the two elements you want to compare,
            one from current row and one from the next:*/
            x = rows[i].getElementsByTagName("TD")[11];
            y = rows[i + 1].getElementsByTagName("TD")[11];
            //check if the two rows should switch place:
            if (parseInt(x.innerHTML) > parseInt(y.innerHTML)) {
                //if so, mark as a switch and break the loop:
                shouldSwitch = true;
                break;
            }
        }
        if (shouldSwitch) {
            /*If a switch has been marked, make the switch
            and mark that a switch has been done:*/
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}
function PatientRelationchange() {
    var RelationVal = document.getElementById("ctl00_C5POBody_ddlPatientRelation");

    if (RelationVal.options[RelationVal.selectedIndex].text == "SELF") {
        document.getElementById("ctl00_C5POBody_txtSelectinsured").value = document.getElementById("ctl00_C5POBody_HiddenPatientName").value.split('&')[0];
        document.getElementById('ctl00_C5POBody_txtSelectinsured').attributes['data-human-id'].value = document.getElementById("ctl00_C5POBody_HiddenPatientName").value.split('&')[1];
        document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = true;
        document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#BFDBFF"
        document.getElementById('imginsuredText').display = "none";
        document.getElementById('ctl00_C5POBody_btnaddins').disabled = true;
        document.getElementById('imginsuredText').style.visibility = "hidden";
        $('#lblSelectInsured').removeClass('manredforstar');
        $('#lblSelectInsured').addClass('spanstyle')
        document.getElementById("lblSelectInsured").innerHTML = "Select Insured";
    }
    else {
        $('#ctl00_C5POBody_txtSelectinsured').val('');
        document.getElementById('ctl00_C5POBody_txtSelectinsured').attributes['data-human-id'].value = "0";
        document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = false;
        document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#FFFFFF"
        document.getElementById('imginsuredText').display = "block";
        document.getElementById('ctl00_C5POBody_btnaddins').disabled = false;
        $('#ctl00_C5POBody_txtSelectinsured').removeClass('nonEditabletxtbox');
        document.getElementById('imginsuredText').style.visibility = "visible";

        $('#lblSelectInsured').removeClass('spanstyle');
        $('#lblSelectInsured').addClass('manredforstar');
        document.getElementById("lblSelectInsured").innerHTML = "Select Insured*";


    }
}

var intPatientlen = -1;

function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}
function LogTimeString(time_string) {
    UI_Time_Stop = new Date();

    var WS_Time = parseFloat(time_string.split(';')[0].split(':')[1].replace('s', ''));
    var DB_Time = parseFloat(time_string.split(';')[1].split(':')[1].replace('s', ''));
    var UI_Time = ((UI_Time_Stop.getTime() - UI_Time_Start.getTime()) / 1000) - WS_Time - DB_Time;
    console.log(time_string + " UI_Time :" + UI_Time + "s; Total_Time :" + (WS_Time + DB_Time + UI_Time).toString() + "s;");

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

function PatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    var txtPatientSearch = document.getElementById("ctl00_C5POBody_txtSelectinsured");

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
            var txtPatientSearch = document.getElementById('ctl00_C5POBody_txtSelectinsured');
            var human_type = SelectedPatient.DisplayString.slice(SelectedPatient.DisplayString.indexOf("PATIENT TYPE"), SelectedPatient.DisplayString.indexOf("EMAIL:") - 3);
            var human_tocken = SelectedPatient.DisplayString.split('|')[0] + "|" + SelectedPatient.DisplayString.split('|')[1] + "|" + SelectedPatient.DisplayString.split('|')[2] + "|" + SelectedPatient.DisplayString.split('|')[3] + "| " + human_type;
            txtPatientSearch.value = human_tocken;
            txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');

        }
    });
    txtPatientSearch.value = ui.item.label;
    txtPatientSearch.attributes['data-human-id'].value = ui.item.value;
    txtPatientSearch.style.backgroundColor = "#BFDBFF";
    txtPatientSearch.disabled = true;
    //HumanDetails.HumanId;

    return false;
}
$("#imginsuredText").on("click", function () {

    $('#ctl00_C5POBody_txtSelectinsured').val('').focus();
    intPatientlen = 0;
    arrPatient = [];
    $(".ui-autocomplete").hide();

    document.getElementById('ctl00_C5POBody_txtSelectinsured').style.backgroundColor = "#FFFFFF";
    document.getElementById('ctl00_C5POBody_txtSelectinsured').attributes['data-human-id'].value = "0";
    document.getElementById('ctl00_C5POBody_txtSelectinsured').disabled = false;

});
$("#ctl00_C5POBody_imgClearplanText").on("click", function () {

    $('#ctl00_C5POBody_txtPlanSearch').val('').focus();
    intplanlength = 0;
    arrPatient = [];
    $(".ui-autocomplete").hide();

    document.getElementById('ctl00_C5POBody_txtPlanSearch').style.backgroundColor = "#FFFFFF";
    document.getElementById('ctl00_C5POBody_txtPlanSearch').attributes['data-plan-id'].value = "0";
    document.getElementById('ctl00_C5POBody_txtPlanSearch').attributes['data-carrier-id'].value = "0";
    document.getElementById('ctl00_C5POBody_txtPlanSearch').disabled = false;
    document.getElementById('ctl00_C5POBody_txtSpecify').value = "";
    document.getElementById('ctl00_C5POBody_txtSpecify').disabled = true;
    document.getElementById('ctl00_C5POBody_txtSpecify').style.backgroundColor = "#BFDBFF"
    $('#lblSpecifyOther').removeClass('manredforstar');
    $('#lblSpecifyOther').addClass('spanstyle');
    document.getElementById("lblSpecifyOther").innerHTML = "Specify Other";


});
function PlanSelected(event, ui) {

    var txtPatientSearch = document.getElementById("ctl00_C5POBody_txtPlanSearch");
    txtPatientSearch.value = ui.item.label.split("|")[0];

    if (ui.item.label.toUpperCase() == "NO MATCHES FOUND.") {
        txtPatientSearch.value = "";
    }
    else {
        if (txtPatientSearch.value.toUpperCase() == "PAYER NOT FOUND") {
            document.getElementById('ctl00_C5POBody_txtSpecify').style.backgroundColor = "#FFFFFF";
            document.getElementById('ctl00_C5POBody_txtSpecify').disabled = false;
            $('#lblSpecifyOther').removeClass('spanstyle');
            $('#lblSpecifyOther').addClass('manredforstar');
            document.getElementById("lblSpecifyOther").innerHTML = "Specify Other*";
        }
        else {
            document.getElementById('ctl00_C5POBody_txtSpecify').style.backgroundColor = "#BFDBFF";
            document.getElementById('ctl00_C5POBody_txtSpecify').disabled = true;
            $('#lblSpecifyOther').removeClass('manredforstar');
            $('#lblSpecifyOther').addClass('spanstyle');
            document.getElementById("lblSpecifyOther").innerHTML = "Specify Other";
        }



        txtPatientSearch.attributes['data-plan-id'].value = ui.item.value.split('|')[0];
        txtPatientSearch.attributes['data-carrier-id'].value = ui.item.value.split('|')[1];
        txtPatientSearch.style.backgroundColor = "#BFDBFF";
        txtPatientSearch.disabled = true;
    }

    if (ui.item.label.toUpperCase() == "PAYER NOT FOUND") {
        $('#lblSpecifyOther').removeClass('spanstyle');
        $('#lblSpecifyOther').addClass('manredforstar');
        document.getElementById("lblSpecifyOther").innerHTML = "Specify Other*";
    }
    else {
        $('#lblSpecifyOther').removeClass('manredforstar');
        $('#lblSpecifyOther').addClass('spanstyle');
        document.getElementById("lblSpecifyOther").innerHTML = "Specify Other";
    }

    //Cap - 1521
    if (document.getElementById("ctl00_C5POBody_txtSpecify").disabled == true) {
        $("#ctl00_C5POBody_txtPolicyholderid").focus();
    }
    else {
        $("#ctl00_C5POBody_txtSpecify").focus();
    }

    return false;
}

var intPatientlen = -1;
if ($("#ctl00_C5POBody_txtSelectinsured").length) {
    $("#ctl00_C5POBody_txtSelectinsured").autocomplete({
        source: function (request, response) {
            if ($("#ctl00_C5POBody_txtSelectinsured").val().trim().length > 2) {
                if (intPatientlen == 0) {
                    UI_Time_Start = new Date();
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

                    arrPatient = [];
                    var strkeyWords = $("#ctl00_C5POBody_txtSelectinsured").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var account_status = "ACTIVE"; //document.getElementById('chkIncludeInactive').checked ? "INACTIVE" : "ACTIVE";
                    var patient_status = "ALIVE";//document.getElementById("chkIncludeDeceased").checked ? "DECEASED" : "ALIVE";
                    var patient_type = "REGULAR";//document.getElementById("rdbRegular").checked ? "REGULAR" : (document.getElementById("rdbWC").checked ? "WC" : (document.getElementById("rdbAuto").checked ? "AUTO" : "ALL"));
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
                                    var human_type = item.label.slice(item.label.indexOf("PATIENT TYPE"), item.label.length);
                                    var human_tocken = item.label.split('|')[0] + "|" + item.label.split('|')[1] + "|" + item.label.split('|')[2] + "|" + item.label.split('|')[3] + "|" + human_type;
                                    return {
                                        label: human_tocken,
                                        val: JSON.stringify(item.value),
                                        value: item.value.HumanId
                                    }
                                }));
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
                else if (intPatientlen != -1) {

                    var results = Filter(arrPatient, request.term);
                    response($.map(results, function (item) {
                        var human_type = item.label.slice(item.label.indexOf("PATIENT TYPE"), item.label.length);
                        var human_tocken = item.label.split('|')[0] + "|" + item.label.split('|')[1] + "|" + item.label.split('|')[2] + "|" + item.label.split('|')[3] + "|" + human_type;
                        return {
                            label: human_tocken,
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
            $('.ui-autocomplete.ui-menu.ui-widget').width("249px");
            $('.ui-autocomplete.ui-menu.ui-widget').css("left", "59%");
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $('#ctl00_C5POBody_txtSelectinsured').focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#ctl00_C5POBody_txtSelectinsured").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        if ($("#ctl00_C5POBody_txtSelectinsured").val().charAt(e.currentTarget.value.length - 1) == " ") {
            if (e.currentTarget.value.split(" ").length > 2)
                intPatientlen = intPatientlen + 1;
            else
                intPatientlen = 0;
        }
        else {
            if ($("#ctl00_C5POBody_txtSelectinsured").val().length != 0 && intPatientlen != -1) {
                intPatientlen = intPatientlen + 1;
            }

            if ($("#ctl00_C5POBody_txtSelectinsured").val().length == 0 || $("#ctl00_C5POBody_txtSelectinsured").val().indexOf(" ") == -1) {
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

    $("#ctl00_C5POBody_txtSelectinsured").data("ui-autocomplete")._renderItem = function (ul, item) {
        var human_tocken = "";
        if (item.label != "No matches found.") {
            var human_type = item.label.slice(item.label.indexOf("PATIENT TYPE"), item.label.length);
            human_tocken = item.label.split('|')[0] + "|" + item.label.split('|')[1] + "|" + item.label.split('|')[2] + "|" + item.label.split('|')[3] + "|" + human_type;
        }
        else {
            human_tocken = item.label;
        }

        if (item.label != "No matches found.") {
            var HumanDetails = $.parseJSON(item.val);
            var list_item = $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .append(human_tocken)
                .appendTo(ul);
            //Jira #CAP-774 - Check the undefind and null to the HumanDetails.Account_Status
            //if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
            if (HumanDetails.Account_Status != undefined && HumanDetails.Account_Status != null && HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                list_item.addClass("inactive");
            //Jira #CAP-774 - Check the undefind and null to the HumanDetails.Status
            //if (HumanDetails.Status.toUpperCase() == "DECEASED")
            if (HumanDetails.Status != undefined && HumanDetails.Status != null && HumanDetails.Status.toUpperCase() == "DECEASED")
                list_item.addClass("deceased");
            return list_item;
        }
        else
            return $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .addClass("disabled")
                .append(human_tocken)
                .appendTo(ul).on("click", function (e) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                });
    };
}

var intplanlength = 0;


$("#ctl00_C5POBody_txtPlanSearch").autocomplete({
    source: function (request, response) {

        if ($("#ctl00_C5POBody_txtPlanSearch").val().length > 2) {
            UI_Time_Start = new Date();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            arrPatient = [];
            var strkeyWords = $("#ctl00_C5POBody_txtPlanSearch").val();

            //var account_status = "ACTIVE"; //document.getElementById('chkIncludeInactive').checked ? "INACTIVE" : "ACTIVE";
            //var patient_status = "ALIVE";//document.getElementById("chkIncludeDeceased").checked ? "DECEASED" : "ALIVE";
            //var patient_type = "REGULAR";//document.getElementById("rdbRegular").checked ? "REGULAR" : (document.getElementById("rdbWC").checked ? "WC" : (document.getElementById("rdbAuto").checked ? "AUTO" : "ALL"));
            var WSData = {
                text_searched: strkeyWords
                //account_status: account_status,
                //patient_status: patient_status,
                //human_type: patient_type
            };

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./frmFindPatient.aspx/GetPlanDetailsByTokens",
                data: JSON.stringify(WSData),
                dataType: "json",
                success: function (data) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

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

                        results = jsonData.Matching_Result;

                        arrPatient = jsonData.Matching_Result;
                        response($.map(results, function (item) {
                            return {
                                label: item.label,
                                val: JSON.stringify(item.value),
                                value: item.value.PlanId
                            }
                        }));
                    }
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
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
        else if (intplanlength != -1) {

            //var results = Filter(arrPatient, request.term);
            //response($.map(results, function (item) {
            //    return {
            //        label: item.label,
            //        val: JSON.stringify(item.value),
            //        value: item.value.PlanId
            //    }
            //}));
        }

    },
    minlength: 2,
    multiple: true,
    mustMatch: false,
    select: PlanSelected,
    open: function () {
        // $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearch').width());
        $('.ui-autocomplete.ui-menu.ui-widget').width("290px");
        $('.ui-autocomplete.ui-menu.ui-widget').css("left", "338px");
        $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
        $('#ctl00_C5POBody_txtPlanSearch').focus();


    },
    focus: function () { return false; }
}).on("paste", function (e) {
    intplanlength = -1;
    arrPatient = [];
    $(".ui-autocomplete").hide();
}).on("input", function (e) {
    $("#ctl00_C5POBody_txtPlanSearch").css("color", "black").attr({ "data-plan-id": "0" });

    $("#ctl00_C5POBody_txtPlanSearch").css("color", "black").attr({ "data-carrier-id": "0" });



}).on("click", function (e) {
    // $('#txtPlanSearch').val('');
    //$("#txtPlanSearch").attr({ "data-human-id": "0", "data-human-details": "" });
}).on("focus", function (e) {
    //$('#txtPlanSearch').val('');
    //$("#txtPlanSearch").attr({ "data-human-id": "0", "data-human-details": "" });
})
$("#ctl00_C5POBody_txtPlanSearch").data("ui-autocomplete")._renderItem = function (ul, item) {

    if (item.label != "No matches found.") {

        var list_item = $("<li>")
            .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
            .append(item.label)
            .appendTo(ul);

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


function ProviderSelected(event, ui) {
    AutoSave();
    var ProviderDetails = JSON.parse(ui.item.val);
    var txtProviderSearch = document.getElementById("ctl00_C5POBody_txtProviderSearch");
    document.getElementById('ctl00_C5POBody_txtProviderSearch').disabled = true;
    document.getElementById('ctl00_C5POBody_txtProviderSearch').style.backgroundColor = "#BFDBFF";

    txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
    txtProviderSearch.attributes['data-phy-details'].value = ProviderDetails.sPhyName;
    txtProviderSearch.attributes['data-phy-gridname'].value = ProviderDetails.sPhyshortName;
    txtProviderSearch.attributes['data-phy-npi'].value = ProviderDetails.sPhyNPI;
    txtProviderSearch.value = ui.item.label;
    var provider = "";

    provider = JSON.parse(ui.item.val).ulPhyId + "|" +
        JSON.parse(ui.item.val).sPhyName + "|" +
        JSON.parse(ui.item.val).sPhyshortName + "|" +
        JSON.parse(ui.item.val).sPhyNPI + "|" +
        JSON.parse(ui.item.val).sPhySpecialty + "|" +
        JSON.parse(ui.item.val).sPhyFacility + "|" +
        JSON.parse(ui.item.val).sPhyAddress + "|" +
        JSON.parse(ui.item.val).sPhyFax + "|" +
        JSON.parse(ui.item.val).sPhyPhone

    document.getElementById('ctl00_C5POBody_txtProviderSearch').disabled = true;

    return false;
}
function ProviderSearchclear() {
    $("#ctl00_C5POBody_txtProviderSearch").attr("disabled", false);
    $("#ctl00_C5POBody_txtProviderSearch").val("");
    document.getElementById('ctl00_C5POBody_txtProviderSearch').style.backgroundColor = "#FFFFFF";

}

$("#ctl00_C5POBody_txtProviderSearch").autocomplete({
    source: function (request, response) {

        if ($("#ctl00_C5POBody_txtProviderSearch").val().length > 2) {
            UI_Time_Start = new Date();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

            arrPatient = [];
            var strkeyWords = $("#ctl00_C5POBody_txtProviderSearch").val();
            var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
            //var account_status = "ACTIVE"; //document.getElementById('chkIncludeInactive').checked ? "INACTIVE" : "ACTIVE";
            //var patient_status = "ALIVE";//document.getElementById("chkIncludeDeceased").checked ? "DECEASED" : "ALIVE";
            //var patient_type = "REGULAR";//document.getElementById("rdbRegular").checked ? "REGULAR" : (document.getElementById("rdbWC").checked ? "WC" : (document.getElementById("rdbAuto").checked ? "AUTO" : "ALL"));
            var WSData = {
                text_searched: strkeyWords
                //account_status: account_status,
                //patient_status: patient_status,
                //human_type: patient_type
            };

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "./frmFindReferralPhysician.aspx/GetProviderDetailsByTokens",
                data: JSON.stringify(WSData),
                dataType: "json",
                success: function (data) {
                    flag = 0;
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    $("#txtProviderSearch").off("keydown", PreventTyping);
                    var jsonData = $.parseJSON(data.d);
                    if (jsonData.Error != undefined) {
                        alert(jsonData.Error);
                        return;
                    }

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

                        arrProvider = jsonData.Matching_Result;
                        response($.map(results, function (item) {
                            return {
                                label: item.label,
                                val: JSON.stringify(item.value),
                                value: item.value.ulPhyId
                            }
                        }));
                    }
                },
                error: function OnError(xhr) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    if (xhr.status == 999)
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
        //else if (intProviderlen != -1) {

        //    var results = Filter(arrProvider, request.term);
        //    response($.map(results, function (item) {
        //        return {
        //            label: item.label,
        //            val: JSON.stringify(item.value),
        //            value: item.value.ulPhyId
        //        }
        //    }));
        //}
    },
    minlength: 2,
    multiple: true,
    mustMatch: false,
    select: ProviderSelected,
    open: function () {
        // $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearch').width());
        $('.ui-autocomplete.ui-menu.ui-widget').width("525px");
        $('.ui-autocomplete.ui-menu.ui-widget').css("left", "138px");
        $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
        $('#ctl00_C5POBody_txtProviderSearch').focus();


    },
    focus: function () { return false; }
}).on("paste", function (e) {
    intplanlength = -1;
    arrPatient = [];
    $(".ui-autocomplete").hide();
}).on("input", function (e) {
    $("#ctl00_C5POBody_txtProviderSearch").css("color", "black").attr({ "data-phy-id": "0" });
    $("#ctl00_C5POBody_txtProviderSearch").css("color", "black").attr({ "data-phy-details": "0" });
    $("#ctl00_C5POBody_txtProviderSearch").css("color", "black").attr({ "data-phy-gridname": "" });
    $("#ctl00_C5POBody_txtProviderSearch").css("color", "black").attr({ "data-phy-npi": "" });

}).on("click", function (e) {
    // $('#txtPlanSearch').val('');
    //$("#txtPlanSearch").attr({ "data-human-id": "0", "data-human-details": "" });
}).on("focus", function (e) {
    //$('#txtPlanSearch').val('');
    //$("#txtPlanSearch").attr({ "data-human-id": "0", "data-human-details": "" });
})
$("#ctl00_C5POBody_txtProviderSearch").data("ui-autocomplete")._renderItem = function (ul, item) {

    if (item.label != "No matches found.") {

        var list_item = $("<li>")
            .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
            .append(item.label)
            .appendTo(ul);

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




function DateValidattion(dateToValidate) {
    var splitdate = document.getElementById(dateToValidate).value;
    var dt1 = new Date();
    var dd = new Date();
    var month = new Array();
    switch (splitdate.split('-')[1]) {
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
        case splitdate.split('-')[1]:
            return false;
            break;

    }


    dd.setFullYear(splitdate.split('-')[2], x, splitdate.split('-')[0]);
    if (isNaN(dd)) {
        return false;
    }
    if (splitdate.split('-')[0] > 31) {
        return false;
    }
    return true;
}

function OpenAddPhysician() {
    localStorage.setItem("IsEnableGrid", "false");
    $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
    $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
    $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px"; 
    //$(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.left = "150px";
    var sPath = "frmPhysicianLibray.aspx";
    $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
    $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabPhysicianLibrary").modal("show");
    $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
        //CAP-291 - Preventing undefined
        var PhyDetails = localStorage.getItem("PhyDetails") ?? "";
        var PhyID = PhyDetails.split("&")[0];
        if (PhyID != "") {
            document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-id'].value = PhyID;
            document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-gridname'].value = PhyDetails.split("&")[1];
            document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-details'].value = PhyDetails.split("&")[2];
            document.getElementById("ctl00_C5POBody_txtProviderSearch").attributes['data-phy-npi'].value = PhyDetails.split("&")[3];
            document.getElementById('ctl00_C5POBody_txtProviderSearch').value = PhyDetails.split("&")[4];
            document.getElementById('ctl00_C5POBody_txtProviderSearch').disabled = true;
            document.getElementById('ctl00_C5POBody_txtProviderSearch').style.backgroundColor = "#BFDBFF";
            localStorage.setItem("PhyDetails", "");
        }
    });
    return false;
}