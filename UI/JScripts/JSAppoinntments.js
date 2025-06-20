//DECLARATIONS---------------------
var fac;
var timeslot;
var PhysicianName;
var PhysicianID;
var SelectedDate;
//---------------------------------

//METHODS--------------------------
window.top.setInterval(function () {
    //Jira #CAP-768
    //if ($(top.window.document).find("#CheckAlert") != undefined)
    if ($(top.window.document).find("#CheckAlert") != undefined && $(top.window.document).find("#CheckAlert") != null && $(top.window.document).find("#CheckAlert").length > 0 && $(top.window.document).find("#CheckAlert")[0] != undefined && $(top.window.document).find("#CheckAlert")[0] != null)
        $(top.window.document).find("#CheckAlert")[0].style.display = "none";
}, 6500);
function showTime() {
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
    then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
}

function OpenFindAllAppointments() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var FindHumanID = document.getElementById(GetClientId("hdnFindApptHumanID")).value
    if (FindHumanID == "") {
        OpenFindPatient(TimeSlotFindAllAppointmentsClick);//checked
    }
    else {
        var obj = new Array();
        obj.push("HumanID=" + FindHumanID);
        openModal("frmFindAllAppointments.aspx", 625, 910, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        //CAP-775 Cannot read properties of null - jsAppointments
        WindowName?.add_close(FindAllAppointmentClick);//checked
    }
    return false;
}

function OpenPhysicianFacility() {
    var ApptPhyID = document.getElementById(GetClientId("hdnApptPhyId")).value
    var ApptFacName = document.getElementById(GetClientId("hdnApptFacName")).value
    var sourcescreen = document.getElementById(GetClientId("hdnSourceScreen")).value
    var obj = new Array();
    obj.push("hdnApptPhyId=" + ApptPhyID);
    obj.push("hdnApptFacName=" + ApptFacName);
    obj.push("hdnSourceScreen=" + sourcescreen);
    openModal("frmAppointments.aspx", 1100, 1150, obj, "ctl00_ModalWindow");
    // openModal("frmAppointments.aspx", 500, 600, obj, "ctl00_ModalWindow");


    document.getElementById(GetClientId("hdnSourceScreen")).value = "";
}

function OpenBlockDays(FacName, SPhy) {
    var FacilityName = FacName;
    var PhysicianSelected = SPhy;
    var obj = new Array();
    obj.push("FacilityName=" + FacilityName);
    obj.push("PhysicianSelected=" + PhysicianSelected);
    openModal("frmBlockDays.aspx", 540, 1150, obj, "ctl00_ModalWindow");
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);

}

function schAppointmentScheduler_TimeSlotContextMenuItemClicked(sender, args) {
    var screenname = document.getElementById(GetClientId("hdnSourceScreen")).value;
    SelectedDate = document.getElementById(GetClientId("hdnSelectedDate")).value;
    var MenuName = args.get_item();
    if (MenuName._text == "Willing Patients List") {
        timeslot = args.get_slot();
        var Faclity = document.getElementById(GetClientId("cboFacilityName"));
        var Calendar = document.getElementById(GetClientId("Calendar1"));
        var str = Faclity.options[Faclity.selectedIndex].value;
        if (str.indexOf("#") != -1) {
            fac = str.replace("#", "_")
        } else {
            fac = str
        }
        var obj = new Array();
        obj.push("facility=" + fac);
        obj.push("SelectedDate=" + timeslot._startTime.format("dd-MMM-yyyy hh:mm:ss tt"));
        obj.push("PhysicianName=" + timeslot._resource.get_text());
        obj.push("EncounterID=" + 0);
        obj.push("humanID=" + 0);
        openModal("frmWillingPatientList.aspx", 800, 1000, obj, "ctl00_ModalWindow");

    }
    else {
        if (screenname == "AppointmentFacility") {
            var Physician = document.getElementById(GetClientId("cboFacilityName"));
            var Calendar = document.getElementById(GetClientId("Calendar1"));
            timeslot = args.get_slot();
            if (timeslot._resource.get_text().indexOf("#") != -1) {
                fac = timeslot._resource.get_text().replace("#", "_");
            } else {
                fac = timeslot._resource.get_text();
            }
            PhysicianName = Physician.options[Physician.selectedIndex].textContent;
            PhysicianID = document.getElementById(GetClientId("hdnApptPhyId")).value;
            SelectedDate = timeslot._startTime.format("dd-MMM-yyyy hh:mm:ss tt");

            if (AppointmentPastDateValidation(document.getElementById(GetClientId("hdnSelectedDate"))) == false) {
                if (confirm("Do you want to create an appointment in the past?") == true) {
                    OpenFindPatient(TimeSlotFindPatientClick);//checked
                }
                else {
                    return false;
                }
            }
            else {
                OpenFindPatient(TimeSlotFindPatientClick);//checked
                return false;
            }
        }
        else {
            timeslot = args.get_slot();
            var Faclity = document.getElementById(GetClientId("cboFacilityName"));
            var Calendar = document.getElementById(GetClientId("Calendar1"));
            var str = Faclity.options[Faclity.selectedIndex].value;
            if (str.indexOf("#") != -1) {
                fac = str.replace("#", "_")
            } else {
                fac = str
            }
            PhysicianName = timeslot._resource.get_text();
            PhysicianID = timeslot._resource.get_key();
            SelectedDate = timeslot._startTime.format("dd-MMM-yyyy hh:mm:ss tt");
            if (AppointmentPastDateValidation(document.getElementById(GetClientId("hdnSelectedDate"))) == false) {
                if (confirm("Do you want to create an appointment in the past?") == true) {
                    OpenFindPatient(TimeSlotFindPatientClick);//checked
                    return false;
                }
                else {
                    return false;
                }
            }
            else {
                OpenFindPatient(TimeSlotFindPatientClick);//checked
                return false;
            }

            return false;
        }
    }
}

function schAppointmentScheduler_AppointmentContextMenuItemClicked(sender, args) {
    var screenname = document.getElementById(GetClientId("hdnSourceScreen")).value;
    if (screenname == "AppointmentFacility") {
        var MenuName = args.get_item();
        var Appointment = args.get_appointment();
        document.getElementById(GetClientId("hdnFindApptHumanID")).value = Appointment._toolTip.split('-')[0];
        var physician = document.getElementById(GetClientId("cboFacilityName"))
        var ApptPhyID = document.getElementById(GetClientId("hdnApptPhyId")).value
        var ApptFacName = document.getElementById(GetClientId("hdnApptFacName")).value;
        if (ApptFacName.indexOf("#") != -1) {
            ApptFacName = ApptFacName.replace("#", "_")
        }
        if (MenuName._text == "Edit Appointment") {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("facility=" + ApptFacName);
            obj.push("PhysicianName=" + physician.value);
            obj.push("PhysicianID=" + ApptPhyID);
            obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
            obj.push("CurrentProcess=" + Appointment._description);
           
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            sessionStorage.setItem("EditAppointmentTransfer", new Date());
            openModal("frmEditAppointment.aspx", 720, 840, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(TimeSlotEditAppointmentClick);//checked
        }
        else if (MenuName._text == "View Appointment") {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("facility=" + ApptFacName);
            obj.push("PhysicianName=" + physician.value);
            obj.push("PhysicianID=" + ApptPhyID);
            obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
            obj.push("CurrentProcess=" + Appointment._description);
            
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            sessionStorage.setItem("EditAppointmentTransfer", new Date());
            openModal("frmEditAppointment.aspx", 750, 840, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(TimeSlotEditAppointmentClick);
        }
        else if (MenuName._text == "New Appointment") {
            fac = ApptFacName;
            var Calendar = document.getElementById(GetClientId("Calendar1"));
            PhysicianName = physician.value;
            PhysicianID = ApptPhyID;
            SelectedDate = Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt");

            OpenFindPatient(TimeSlotFindPatientClick);//checked
            return false;
        }
        else if (MenuName._text == "Check In") {
            var sProcedure = '';
            var obj = new Array();
            if (Appointment._toolTip.toString().split(';').length > 1) {

                var sValue = Appointment._toolTip.toString().split(';');
                for (var i = 1; i < sValue.length; i++) {
                    if (i == 1)
                        sProcedure = sValue[i].replace("\n", "").replace(" ", "").replace("#", "");
                    else
                        sProcedure += ";" + sValue[i].replace("\n", "").replace(" ", "").replace("#", "");
                }

            }
            localStorage.setItem("cpt", sProcedure)
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            obj.push("cpt=" + sProcedure);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("Is_General_Queue_Appoinment=" + Appointment._attributes._data.Is_General_Queue_Appoinment);
            //CAP-2601
            obj.push("Facility=" + encodeURIComponent(document.getElementById("ctl00_C5POBody_cboFacilityName").value));
         
            obj.push("bShowPat=false");
            obj.push("sScreenMode=CheckedIn");
            //CAP-2619
            if (/^[0-9]+$/.test(Appointment.get_resources()._array[0]._key)) {
                obj.push("PhysicianID=" + Appointment.get_resources()._array[0]._key);
            } else {
                obj.push("PhysicianID=" + ApptPhyID);
            }
            openModal("frmQuickpatientcreate.aspx", 920, 1020, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Cancel Appointment") {
            var obj = new Array();
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            openModal("frmCancelAppointment.aspx", 240, 520, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(CancelRefreshSchedular);
        }
        else if (MenuName._text == "No Show") {
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnMyEncounterStatus")).value = Appointment._description;
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Manage Authorization") {
            localStorage.setItem("IsEdit", "false");

            var obj = new Array();
            obj.push("Human_Id=" + Appointment._toolTip.split('-')[0]);
           
            openModal("frmAuthorization.aspx", 800, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
          
            return false;
        }
        else if (MenuName._text == "Walked Away") {
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnMyEncounterStatus")).value = Appointment._description;
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Undo") {
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnMyEncounterStatus")).value = Appointment._description;
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Authorization & EV") {
            var obj = new Array();
            var sProcedure = '';
            if (Appointment._toolTip.toString().split(';').length > 1) {

                var sValue = Appointment._toolTip.toString().split(';');
                for (var i = 1; i < sValue.length; i++) {
                    if (i == 1)
                        sProcedure = sValue[i].replace("\n", "").replace(" ", "").replace("#","");
                    else
                        sProcedure += ";" + sValue[i].replace("\n", "").replace(" ", "").replace("#","");
                }

            }
            localStorage.setItem("cpt", sProcedure)
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            obj.push("cpt=" + sProcedure);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=false");
            obj.push("sScreenMode=ELIGIBILITY");
            openModal("frmQuickpatientcreate.aspx", 710, 1020, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Electronic Super Bill") {
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnApptPhyId")).value = Appointment.get_resources()._array[0]._key;
            document.getElementById(GetClientId("hdnApptFacName")).value = document.getElementById(GetClientId("cboFacilityName")).value;
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Show Patient Summary") {
            var obj = new Array();
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=true");
            obj.push("sScreenMode=PATIENT SUMMARY");
            openModal("frmQuickpatientcreate.aspx", 920, 1020, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Generate Enc. Doc.") {
            var obj = new Array();
            obj.push("EncounterId=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            if (screenname == "AppointmentFacility") {               
                obj.push("PhysicianName=" + $("#ctl00_C5POBody_cboFacilityName option:selected").text());
                obj.push("FacilityName=" + ApptFacName);
            }
            else {
                obj.push("FacilityName=" + document.getElementById("ctl00_C5POBody_cboFacilityName").value);
                obj.push("PhysicianName=" + Appointment.get_resources()._array[0]._text);
            }
            obj.push("TempelateName=ENCOUNTERTEMPLATE");
            openModal("frmWellnessNotes.aspx", 920, 1000, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.hide();
           top.document.getElementsByClassName("TelerikModalOverlay")[0].attributes[2].value = "display: none;";
        }
        else if (MenuName._text == "Print Receipt") {
            //if (Appointment._id.toString().split('-')[1] == "Y") {
                document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
                document.getElementById(GetClientId("hdnHumanID")).value = Appointment._toolTip.split('-')[0];
                document.getElementById(GetClientId("hdnFileName")).value = '';
                if (screen.height < 800)
                    document.getElementById(GetClientId("hdnLaptopView")).value = "true";
                else
                    document.getElementById(GetClientId("hdnLaptopView")).value = "false";
                document.getElementById(GetClientId("btnPrintRecipt")).click();
            //}
            //else
            //    DisplayErrorMessage('110085');
        }
        else if (MenuName._text == "Collect/Edit Payment") {
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            var obj = new Array();
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=true");
            obj.push("sScreenMode=COLLECT COPAY");
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            openModal("frmQuickpatientcreate.aspx", 920, 1020, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Print Quality Measure Data Sheet") {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("formName=Measure");

            openModal("frmMeasureReport.aspx", 740, 840, obj, "ctl00_ModalWindow");
        }
        else if (MenuName._text == "Open Patient Chart") {
            humanid = Appointment._toolTip.split('-')[0];

            //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
            // Cap - 891
            //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
            Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&ScreenMode=Menu&openingfrom=Menu&from=viewresult", 1000, 1500, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
            $('#resultLoading').css("display", "none");
            if (Result == null)
                return false;
        }
    }
    else {
        var MenuName = args.get_item();
        var Appointment = args.get_appointment();
        var Faclity;
        fac = document.getElementById(GetClientId("cboFacilityName"));
        var str = fac.options[fac.selectedIndex].value;
        if (str.indexOf("#") != -1) {
            Faclity = str.replace("#", "_")
        }
        else {
            Faclity = str
        }

        document.getElementById(GetClientId("hdnFindApptHumanID")).value = Appointment._toolTip.split('-')[0];
        if (MenuName._text == "Edit Appointment") {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("facility=" + Faclity);
            obj.push("PhysicianName=" + Appointment.get_resources()._array[0]._text);
            obj.push("PhysicianID=" + Appointment.get_resources()._array[0]._key);
            obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
            obj.push("CurrentProcess=" + Appointment._description);
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            sessionStorage.setItem("EditAppointmentTransfer", new Date());

            openModal("frmEditAppointment.aspx", 720, 840, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(TimeSlotEditAppointment2Click);//checked

        } else if (MenuName._text == "View Appointment") {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("facility=" + Faclity);
            obj.push("PhysicianName=" + Appointment.get_resources()._array[0]._text);
            obj.push("PhysicianID=" + Appointment.get_resources()._array[0]._key);
            obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
            obj.push("CurrentProcess=" + Appointment._description);
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            sessionStorage.setItem("EditAppointmentTransfer", new Date());

            openModal("frmEditAppointment.aspx", 720, 840, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(TimeSlotEditAppointmentClick);//checked
        }
        else if (MenuName._text == "New Appointment") {
            var Calendar = document.getElementById(GetClientId("Calendar1"));

            fac = Faclity
            PhysicianName = Appointment.get_resources()._array[0]._text;
            PhysicianID = Appointment.get_resources()._array[0]._key;
            SelectedDate = Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt");

            OpenFindPatient(TimeSlotFindPatientClick);//checked
            return false;
        }
        else if (MenuName._text == "Check In") {
            var obj = new Array();
            var sProcedure = '';
            if (Appointment._toolTip.toString().split(';').length > 1) {

                var sValue = Appointment._toolTip.toString().split(';');
                for (var i = 1; i < sValue.length; i++) {
                    if (i == 1)
                        sProcedure = sValue[i].replace("\n", "").replace(" ", "").replace("#","");
                    else
                        sProcedure += ";" + sValue[i].replace("\n", "").replace(" ", "").replace("#", "");
                }

            }
            localStorage.setItem("cpt", sProcedure)
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            obj.push("cpt=" + sProcedure);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=false");
            obj.push("sScreenMode=CheckedIn");
            obj.push("Is_General_Queue_Appoinment=" + Appointment._attributes._data.Is_General_Queue_Appoinment);
            //CAP-2601
            obj.push("Facility=" + encodeURIComponent(document.getElementById("ctl00_C5POBody_cboFacilityName").value));
            //CAP-2619
            if (/^[0-9]+$/.test(Appointment.get_resources()._array[0]._key)) {
                obj.push("PhysicianID=" + Appointment.get_resources()._array[0]._key);
            } else {
                obj.push("PhysicianID=" + ApptPhyID);
            }
           
            openModal("frmQuickpatientcreate.aspx", 920, 1020, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);//checked
        }
        else if (MenuName._text == "Cancel Appointment") {
            var obj = new Array();
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            openModal("frmCancelAppointment.aspx", 240, 520, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(CancelRefreshSchedular);//checked
        }
        else if (MenuName._text == "No Show") {
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnMyEncounterStatus")).value = Appointment._description;
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Manage Authorization") {
            localStorage.setItem("IsEdit", "false");
           

            var obj = new Array();
            obj.push("Human_Id=" + Appointment._toolTip.split('-')[0]);

            openModal("frmAuthorization.aspx", 800, 1200, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');

            return false;
        }
        else if (MenuName._text == "Walked Away") {
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnMyEncounterStatus")).value = Appointment._description;
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Undo") {
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnMyEncounterStatus")).value = Appointment._description;
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Authorization & EV") {
            var obj = new Array();
            var sProcedure = '';
            if (Appointment._toolTip.toString().split(';').length > 1) {

                var sValue = Appointment._toolTip.toString().split(';');
                for (var i = 1; i < sValue.length; i++) {
                    if (i == 1)
                        sProcedure = sValue[i].replace("\n", "").replace(" ", "").replace("#","");
                    else
                        sProcedure += ";" + sValue[i].replace("\n", "").replace(" ", "").replace("#", "");
                }

            }
            localStorage.setItem("cpt", sProcedure)
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0])
            obj.push("cpt=" + sProcedure);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=false");
            obj.push("sScreenMode=ELIGIBILITY");
            openModal("frmQuickpatientcreate.aspx", 720, 1020, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);//checked
        }
        else if (MenuName._text == "Electronic Super Bill") {
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnApptPhyId")).value = Appointment.get_resources()._array[0]._key;
            document.getElementById(GetClientId("hdnApptFacName")).value = document.getElementById(GetClientId("cboFacilityName")).value;
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
        }
        else if (MenuName._text == "Willing Patients List") {
            var obj = new Array();
            obj.push("facility=" + Faclity);
            obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
            obj.push("PhysicianName=" + Appointment.get_resources()._array[0]._text);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            openModal("frmWillingPatientList.aspx", 800, 1000, obj, "ctl00_ModalWindow");

        }
        else if (MenuName._text == "Show Patient Summary") {
            var obj = new Array();
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=true");
            obj.push("sScreenMode=PATIENT SUMMARY");
            openModal("frmQuickpatientcreate.aspx", 900, 1020, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Generate Enc. Doc.") {
            var obj = new Array();
            obj.push("EncounterId=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            if (screenname == "AppointmentFacility") {
                obj.push("PhysicianName=" + document.getElementById("ctl00_C5POBody_cboFacilityName").value);
                obj.push("FacilityName=" + ApptFacName);
            }
            else {
                obj.push("FacilityName=" + document.getElementById("ctl00_C5POBody_cboFacilityName").value);
                obj.push("PhysicianName=" + Appointment.get_resources()._array[0]._text);
            }
            obj.push("TempelateName=ENCOUNTERTEMPLATE");
            openModal("frmWellnessNotes.aspx", 920, 1000, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.hide();
            top.document.getElementsByClassName("TelerikModalOverlay")[0].attributes[2].value = "display: none;";
        }
        else if (MenuName._text == "Print Receipt") {
           // if (Appointment._id.toString().split('-')[1] == "Y") {
                document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
                document.getElementById(GetClientId("hdnHumanID")).value = Appointment._toolTip.split('-')[0];
                document.getElementById(GetClientId("hdnFileName")).value = '';
                if (screen.height < 800)
                    document.getElementById(GetClientId("hdnLaptopView")).value = "true";
                else
                    document.getElementById(GetClientId("hdnLaptopView")).value = "false";
                document.getElementById(GetClientId("btnPrintRecipt")).click();
            //}
            //else
            //    DisplayErrorMessage('110085');
        }
        else if (MenuName._text == "Print Super Bill") {
            document.getElementById(GetClientId("hdnEncounterID")).value = Appointment._id.toString().split('-')[0];
            document.getElementById(GetClientId("hdnSelectedMenu")).value = MenuName._text;
            document.getElementById(GetClientId("hdnApptPhyId")).value = Appointment.get_resources()._array[0]._key;
            document.getElementById(GetClientId("hdnApptFacName")).value = document.getElementById(GetClientId("cboFacilityName")).value;
            document.getElementById(GetClientId("btnMoveToNextProcess")).click();
            document.getElementById(GetClientId("btnPrintSuperBill")).click();
        }
        else if (MenuName._text == "Attach Super Bill") {

            var obj = new Array();
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("HuamnId=" + Appointment._toolTip.split('-')[0]);
            obj.push("Screen=Appointments");
            openModal("frmOnlineDocuments.aspx", 900, 930, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Create New Auth") {
            var obj = new Array();
            obj.push("PatientName=" + Appointment._toolTip.split('-')[1]);
            obj.push("ScreenName=CaptureAuthorization");
            obj.push("PatientType=" + Appointment._id.toString().split('-')[4]);
            obj.push("PatientDOB=" + Appointment._id.toString().split('-')[3]);
            obj.push("AccNo=" + Appointment._toolTip.split('-')[0]);
            obj.push("AuthID=" + 0);
            obj.push("OpenMode=Create");
            obj.push("EncID=" + Appointment._id.toString().split('-')[0]);
            openModal("frmOnlineDocuments.aspx", 1000, 2000, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Print Quality Measure Data Sheet") {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("formName=Measure");

            openModal("frmMeasureReport.aspx", 740, 840, obj, "ctl00_ModalWindow");
        }
        else if (MenuName._text == "Collect/Edit Payment") {
            localStorage.setItem("EncounterAuth", Appointment._id.toString().split('-')[0]);
            var obj = new Array();
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("humanID=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncStatus=" + Appointment._description);
            obj.push("bShowPat=true");
            obj.push("sScreenMode=COLLECT COPAY");
            openModal("frmQuickpatientcreate.aspx", 900, 1020, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(RefreshSchedular);
        }
        else if (MenuName._text == "Open Patient Chart") {
            humanid = Appointment._toolTip.split('-')[0];

            //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=openpatientchart&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
            //Cap - 891
            //Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&from=viewresult&ScreenMode=Menu&openingfrom=Menu", 840, 1278, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
            Result = openNonModal("frmPatientchart.aspx?HumanID=" + humanid + "&ScreenMode=Menu&openingfrom=Menu&from=viewresult", 1000, 1500, obj);//BugID:45876,for BugID:45808 increased screen width to 1278px
            $('#resultLoading').css("display", "none");
            if (Result == null)
                return false;
        }
    }
}

function schAppointmentScheduler_AppointmentClick(sender, args) {
    var Appointment = args.get_appointment();
    if (Appointment._description.trim() != "")
        document.getElementById(GetClientId("hdnFindApptHumanID")).value = Appointment._toolTip.split('-')[0];
    else
        document.getElementById(GetClientId("hdnFindApptHumanID")).value = "";
}

function schAppointmentScheduler_AppointmentContextMenu(sender, args) {
    var date = new Date();
    var Appointment = args.get_appointment();
    Appointment._description
    var menu = $find(AppointmentMenu + "_AppointmentMenu");
    menu._postback();
    var Calendar = document.getElementById(GetClientId("Calendar1"));
    if (Appointment._description == "") {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").hide();
        menu.findItemByText("View Appointment").hide();
        menu.findItemByText("Check In").hide();
        menu.findItemByText("Cancel Appointment").hide();
        menu.findItemByText("Walked Away").hide();
        menu.findItemByText("No Show").hide();
        menu.findItemByText("Undo").hide();
        menu.findItemByText("Authorization & EV").hide();
        menu.findItemByText("Show Patient Summary").hide();
        menu.findItemByText("Print Receipt").hide();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Open Patient Chart").hide();

    } else if (Appointment._start.format("yyyy-MM-dd") > date.format("yyyy-MM-dd")) {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").show();
        menu.findItemByText("Cancel Appointment").show();
        menu.findItemByText("Authorization & EV").show();
        menu.findItemByText("Show Patient Summary").show();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Print Receipt").show();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("View Appointment").hide();
        menu.findItemByText("Check In").hide();
        menu.findItemByText("Walked Away").hide();
        menu.findItemByText("No Show").hide();
        menu.findItemByText("Undo").hide();
        menu.findItemByText("Open Patient Chart").show();


    } else if (Appointment._description == "SCHEDULED") {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").show();
        menu.findItemByText("Check In").show();
        menu.findItemByText("Cancel Appointment").show();
        menu.findItemByText("No Show").show();
        menu.findItemByText("Authorization & EV").show();
        menu.findItemByText("Show Patient Summary").show();
        //menu.findItemByText("Generate Enc. Doc.").show();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Print Receipt").show();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("View Appointment").hide();
        menu.findItemByText("Walked Away").hide();
        menu.findItemByText("Undo").hide();
        menu.findItemByText("Open Patient Chart").show();

    } 
    //else if (Appointment._description == "MA_PROCESS") {
    //    menu.findItemByText("New Appointment").show();
    //    menu.findItemByText("View Appointment").show();
    //    menu.findItemByText("Walked Away").show();
    //    menu.findItemByText("Authorization & EV").show();
    //    menu.findItemByText("Show Patient Summary").show();
    //    menu.findItemByText("Print Receipt").show();
    //    menu.findItemByText("Generate Enc. Doc.").hide();
    //    menu.findItemByText("Print Quality Measure Data Sheet").hide();
    //    menu.findItemByText("Edit Appointment").hide();
    //    menu.findItemByText("Cancel Appointment").hide();
    //    menu.findItemByText("No Show").hide();

    //    menu.findItemByText("Undo").hide();
    //    menu.findItemByText("Check In").hide();

    //    menu.findItemByText("Open Patient Chart").show();

    //}
    else if (Appointment._description == "NO_SHOW" || Appointment._description == "WALKED_AWAY") {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").show();
        menu.findItemByText("Check In").hide();
        menu.findItemByText("Undo").show();
        menu.findItemByText("Authorization & EV").show();
        menu.findItemByText("Show Patient Summary").show();
        menu.findItemByText("Print Receipt").show();
        //menu.findItemByText("Generate Enc. Doc.").show();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("View Appointment").hide();
        menu.findItemByText("Cancel Appointment").hide();
        menu.findItemByText("Walked Away").hide();
        menu.findItemByText("No Show").hide();
        menu.findItemByText("Open Patient Chart").show();

    } else if (Appointment._description == "CHECK_OUT_WAIT") {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").hide();
        menu.findItemByText("View Appointment").show();
        menu.findItemByText("Undo").hide();
        menu.findItemByText("Show Patient Summary").show();
        menu.findItemByText("Print Receipt").show();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("Cancel Appointment").hide();
        menu.findItemByText("Check In").hide();
        menu.findItemByText("Walked Away").show();
        menu.findItemByText("No Show").hide();
      
        menu.findItemByText("Authorization & EV").hide();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Open Patient Chart").show();
    } else {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").hide();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("View Appointment").hide();
        menu.findItemByText("Cancel Appointment").hide();
        menu.findItemByText("Check In").hide();
        menu.findItemByText("Walked Away").hide();
        menu.findItemByText("No Show").hide();
       // menu.findItemByText("Manage Authorization").hide();
        menu.findItemByText("Undo").hide();
        menu.findItemByText("Authorization & EV").hide();
        menu.findItemByText("Show Patient Summary").hide();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Open Patient Chart").hide();
    }
    if (Appointment._description == "CHECK_OUT_COMPLETE" || Appointment._description == "CHECK_OUT") {
        menu.findItemByText("New Appointment").show();
        menu.findItemByText("Edit Appointment").hide();
        menu.findItemByText("View Appointment").show();
        menu.findItemByText("Check In").hide();
        menu.findItemByText("Cancel Appointment").hide();
        menu.findItemByText("Walked Away").hide();
        menu.findItemByText("No Show").hide();

        menu.findItemByText("Undo").hide();
        menu.findItemByText("Authorization & EV").hide();
        menu.findItemByText("Show Patient Summary").show();
        menu.findItemByText("Print Receipt").show();
        menu.findItemByText("Generate Enc. Doc.").hide();
        menu.findItemByText("Print Quality Measure Data Sheet").hide();
        menu.findItemByText("Open Patient Chart").show();

    }
}

function GetClientId(strid) {
    var count = document.forms[0].length;
    var i = 0;
    var eleName;
    for (i = 0; i < count; i++) {
        eleName = document.forms[0].elements[i].id;
        pos = eleName.indexOf(strid);
        if (pos >= 0) break;
    }
    return eleName;
}

function schAppointmentScheduler_TimeSlotClick(sender, args) {
    document.getElementById(GetClientId("hdnFindApptHumanID")).value = "";
    var screenname = document.getElementById(GetClientId("hdnSourceScreen")).value;
    if (screenname == "AppointmentFacility") {
        timeslot = args.get_targetSlot();
        if (timeslot._resource.get_text().indexOf("#") != -1) {
            fac = timeslot._resource.get_text().replace("#", "_");
        } else {
            fac = timeslot._resource.get_text();
        }

        PhysicianName = document.getElementById(GetClientId("cboFacilityName")).value;
        PhysicianID = document.getElementById(GetClientId("hdnApptPhyId")).value;
        SelectedDate = timeslot._startTime.format("dd-MMM-yyyy hh:mm:ss tt");

        var Calendar = document.getElementById(GetClientId("Calendar1"));
        if (AppointmentPastDateValidation(document.getElementById(GetClientId("hdnSelectedDate"))) == false) {
            if (confirm("Do you want to create an appointment in the past?") == true) {
                OpenFindPatient(TimeSlotFindPatientClick);//checked
                return false;
            }

            else {
                return false;
            }
        }
        else {
            OpenFindPatient(TimeSlotFindPatientClick);//checked
        }
    }
    else {
        timeslot = args.get_targetSlot();
        var Faclity = document.getElementById(GetClientId("cboFacilityName"));
        var str = Faclity.options[Faclity.selectedIndex].value;
        if (str.indexOf("#") != -1) {
            fac = str.replace("#", "_")
        } else {
            fac = str
        }
        PhysicianName = timeslot._resource.get_text();
        PhysicianID = timeslot._resource.get_key();
        SelectedDate = timeslot._startTime.format("dd-MMM-yyyy hh:mm:ss tt");

        var Calendar = document.getElementById(GetClientId("Calendar1"));
        if (AppointmentPastDateValidation(document.getElementById(GetClientId("hdnSelectedDate"))) == false) {
            if (confirm("Do you want to create an appointment in the past?") == true) {
                OpenFindPatient(TimeSlotFindPatientClick);//checked
                return false;
            }
            else {
                return false;
            }
        }
        else {
            OpenFindPatient(TimeSlotFindPatientClick);//checked
            return false;
        }
        return false;
    }
}
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler)
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function BeginRequestHandler(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function EndRequestHandler(sender, args) {
    if (args._error != null) {
        if (args._error.name == "Sys.WebForms.PageRequestManagerParserErrorException") {
            alert("Your session has expired");
            args._error.message = "Your session has expired";
            args._errorHandled = true;
            //CAP-1752
            var IsSSOLogin = document.getElementById(GetClientId("hdnIsSSOLogin")).value;
            if (IsSSOLogin == "Y") {
                parent.window.location.href = 'frmloginNew.aspx';
            }
            else {
                parent.window.location.href = 'frmlogin.aspx';
            }
            
            return false;
        } else if (args._error.name == "Sys.WebForms.PageRequestManagerServerErrorException") {
            console.log(args._error.message);
            args._errorHandled = true;
            return false;
        }
    }
    StopLoadingImage();

    //Jira CAP-1953
    //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if ($("#ctl00_C5POBody_hdnStopLoading")[0]?.value != "true") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}

function AppointmentPastDateValidation(dateToValidate) {
    var splitdate = dateToValidate.value;
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
    if (parseInt(splitdate.split('-')[0]) > 31) {
        return false;
    }
    if ((dd.getFullYear() < dt1.getFullYear())) {
        return false;
    } else if (dd.getMonth() < dt1.getMonth() && (dd.getFullYear() <= dt1.getFullYear())) {
        return false;
    } else if (dd.getDate() < dt1.getDate() && (dd.getMonth() <= dt1.getMonth()) && (dd.getFullYear() <= dt1.getFullYear())) {
        return false;
    } else {
        return true;
    }
}

function OpenPDF() {
    var obj = new Array();
    var sSI = null;
    obj.push("SI=" + document.getElementById(GetClientId("hdnFileName")).value);
    obj.push("Location=" + "DYNAMIC");
    openModal("frmPrintPDF.aspx", 750, 900, obj, "ctl00_ModalWindow");
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.add_close(RefreshSchedular);
}
function PrintReciptClick() {
    document.getElementById(GetClientId("btnRefresh")).click();
}

function CloseWindow() {
    window.close();
}

function PrintSuperBillClick() {
    document.getElementById(GetClientId("btnRefresh")).click();
}

function calenderclick(sender, args) {
    document.getElementById(GetClientId("hdnFindApptHumanID")).value = "";
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    sender.set_autoPostBack(true);
}
function schAppoinmentScheduler_NavigationCommand(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function openModalFindAllAppointments(fromname, height, width, inputargument, RadWindowName) {
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
        if (inputargument.length != 0) {
            PageName = PageName + "?";
        }
    }
    var result = radopen(PageName + Argument, RadWindowName);
    result.SetModal(true);
    result.set_visibleStatusbar(false);
    result.setSize(width, height);
    result.set_behaviors(Telerik.Web.UI.WindowBehaviors.move);
    result.set_iconUrl("Resources/16_16.ico");
    result.set_keepInScreenBounds(true);
    result.set_centerIfModal(true);
    result.center();
}
function GetClientId(strid) {
    var count = document.forms[0].length;
    var i = 0; var eleName;
    for (i = 0; i < count; i++) {
        eleName = document.forms[0].elements[i].id;
        pos = eleName.indexOf(strid);
        if (pos >= 0) break;
    }
    return eleName;
}


function schAppoinmentScheduler_DoubleClick(sender, args) {
    var screenname = document.getElementById(GetClientId("hdnSourceScreen")).value;
    var Appointment = args.get_appointment();
    if (screenname == "AppointmentFacility") {
        document.getElementById(GetClientId("hdnFindApptHumanID")).value = Appointment._toolTip.split('-')[0];
        var physician = document.getElementById(GetClientId("cboFacilityName"))
        var ApptPhyID = document.getElementById(GetClientId("hdnApptPhyId")).value
        var ApptFacName = document.getElementById(GetClientId("hdnApptFacName")).value


        var str = ApptFacName;
        if (str.indexOf("#") != -1) {
            ApptFacName = str.replace("#", "_")
        }
        else {
            ApptFacName = str
        }
        var obj = new Array();
        obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
        obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
        obj.push("facility=" + ApptFacName);
        obj.push("PhysicianName=" + physician.value);
        obj.push("PhysicianID=" + ApptPhyID);
        obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
        obj.push("CurrentProcess=" + Appointment._description);
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        sessionStorage.setItem("EditAppointmentTransfer", new Date());

        openModal("frmEditAppointment.aspx", 720, 840, obj, "ctl00_ModalWindow");

        var WindowName = $find('ctl00_ModalWindow');
        WindowName.add_close(TimeSlotEditAppointmentClick);
    }

    else {
        var Faclity;
        fac = document.getElementById(GetClientId("cboFacilityName"));
        var str = fac.options[fac.selectedIndex].value;
        if (str.indexOf("#") != -1) {
            Faclity = str.replace("#", "_")
        }
        else {
            Faclity = str
        }
        if (Appointment?._id != undefined && Appointment?._id != null) {
            var obj = new Array();
            obj.push("Human_id=" + Appointment._toolTip.split('-')[0]);
            obj.push("EncounterID=" + Appointment._id.toString().split('-')[0]);
            obj.push("facility=" + Faclity);
            obj.push("PhysicianName=" + Appointment.get_resources()._array[0]._text);
            obj.push("PhysicianID=" + Appointment.get_resources()._array[0]._key);
            obj.push("SelectedDate=" + Appointment._start.format("dd-MMM-yyyy hh:mm:ss tt"));
            obj.push("CurrentProcess=" + Appointment._description);
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            sessionStorage.setItem("EditAppointmentTransfer", new Date());

            openModal("frmEditAppointment.aspx", 750, 840, obj, "ctl00_ModalWindow");

            openModal("frmEditAppointment.aspx", 720, 840, obj, "ctl00_ModalWindow");


            var WindowName = $find('ctl00_ModalWindow');

            //CAP-3318 - Applying null safety check
            WindowName?.add_close(TimeSlotEditAppointment2Click);
        }
    }
}
function CloseAppointmentModal() {
    //Jira CAP-1217
    $(top.window.document).find("#ctl00_C5POBody_btnRefresh")[0].click();
    self.close();
}
function AppmntLoad() {
    StopLoadingImage();
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}

$(document).ready(function () {
    $(document).keydown(function (event) {
        if (event.ctrlKey == true && (event.which == '107' || event.which == '109' || event.which == '187' || event.which == '189')) {
            event.preventDefault();
        }
    });

    $(window).bind('mousewheel DOMMouseScroll', function (event) {
        if (event.ctrlKey == true) {
            event.preventDefault();
        }
    });


});
function refreshbtnclick() {
    document.getElementById(GetClientId("btnRefresh")).click();
}

function btnToday_Clicked() {
    //CAP-775 Cannot read properties of null - jsAppointments
    var selected_date = $find('ctl00_C5POBody_Calendar1')?.get_selectedDates();
    var today = new Date();
    //Jira #CAP-768
    ////CAP-289 - Cannot read properties of undefined
    //if (selected_date != undefined && selected_date != null && (selected_date[0][0] != today.getFullYear() || selected_date[0][1] != (today.getMonth() + 1) || selected_date[0][2] != today.getDate())) {
    //CAP-1009 - Add missing word '.length' after selected_date.
    if (selected_date != undefined && selected_date != null && selected_date.length > 0 && (selected_date[0][0] != today.getFullYear() || selected_date[0][1] != (today.getMonth() + 1) || selected_date[0][2] != today.getDate())) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return true;
    }
    else
        return false;
}

function OpenFindPatient(AddCloseMethod) {
    var obj = new Array();
    obj.push("ScreenName=Appointments");
    StartLoadingImage();
    openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
    var WindowName = $find('ctl00_ModalWindow');
    //CAP-775 Cannot read properties of null - jsAppointments
    WindowName?.add_close(AddCloseMethod);
}
//---------------------------------

//ADD CLOSE METHODS--------------------------
function TimeSlotFindPatientClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != "undefined" && Result != null) {
        if (Result.HumanID == "") {
            return false;
        }
        if ((Result.IsQuickPatient != null && Result.IsQuickPatient != undefined && Result.IsQuickPatient == "TRUE") || (Result.IsFindPatient == "TRUE" && Result.IsQuickPatient == undefined)) {
            var obj = new Array();
            obj.push("Human_id=" + Result.HumanId);
            obj.push("PatientName=" + Result.PatientName);
            obj.push("PatientDOB=" + Result.PatientDOB);
            obj.push("HumanType=" + Result.HumanType);
            obj.push("Home_Phone=" + Result.Home_Phone);
            obj.push("Cell_Phone=" + Result.Cell_Phone);
            obj.push("Encounter_Provider_ID=" + Result.Encounter_Provider_ID);
            obj.push("EncounterID=" + 0);
            obj.push("facility=" + fac);
            obj.push("PhysicianName=" + PhysicianName);
            obj.push("PhysicianID=" + PhysicianID);
            obj.push("SelectedDate=" + SelectedDate);
            obj.push("CurrentProcess=" + "");
            window.setTimeout(function () {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
                sessionStorage.setItem("EditAppointmentTransfer", new Date());
                openModal("frmEditAppointment.aspx", 750, 840, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.remove_close(TimeSlotFindPatientClick);
                WindowName.add_close(TimeSlotEditAppointmentClick);
            }, 50);
            return false;
        }
        else {
            editappt = args.get_argument();
            if (editappt != null) {
                document.getElementById(GetClientId("hdnEditApptPhyID")).value = editappt.PhysicianID;
                document.getElementById(GetClientId("btnRefresh")).click();
            }
        }
    }
}

function TimeSlotEditAppointmentClick(oWindow, args) {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    editappt = args.get_argument();
    if (editappt != null) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        document.getElementById(GetClientId("hdnEditApptPhyID")).value = editappt.PhysicianID;
        document.getElementById(GetClientId("btnRefresh")).click();
    }
}

function TimeSlotFindAllAppointmentsClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != null) {
        if (Result.HumanID == "") {
            return false;
        }
        var obj = new Array();
        obj.push("HumanID=" + Result.HumanId);
        window.setTimeout(function () {
            openModal("frmFindAllAppointments.aspx", 625, 910, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(FindAllAppointmentClick);
        }, 50);
    }
    else
    {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}

function TimeSlotEditAppointment2Click(oWindow, args) {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    editappt = args.get_argument();
    if (editappt != null) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        document.getElementById(GetClientId("hdnEditApptPhyID")).value = editappt.PhysicianID;
        if (editappt.Selecteddate != "") {
            document.getElementById(GetClientId("hdnSelectedSlotDate")).value = editappt.Selecteddate;
            document.getElementById(GetClientId("hdnApptFacName")).value = editappt.Facility;
        }
        document.getElementById(GetClientId("btnRefresh")).click();
    }
}

function FindAllAppointmentClick(oWindow, args) {
    oWindow.close();
    document.getElementById(GetClientId("btnRefresh")).click();
}

function RefreshSchedular(oWindow, args) {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    document.getElementById(GetClientId("btnRefresh")).click();
}
function CancelRefreshSchedular(oWindow, args) {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    document.getElementById(GetClientId("btnRefresh")).click();
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.remove_close(CancelRefreshSchedular)
    if (oWindow._title != "Willing Patient List") {
        if (args._argument != null && args._argument.Close == "Y") {
            return false;
        }
        
    }
}
function LoadAppointment() {

    $('option').addClass('LabelStyleBold');
    $('label').addClass('LabelStyleBold');

    //Jira CAP-1953
    //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if ($("#ctl00_C5POBody_hdnStopLoading")[0]?.value != "true") {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}

//Jira CAP-1953
function ProcessMissMatchAlert(sAlertMessage) {
    //alert(sAlertMessage);
    var check = DisplayErrorMessage('10113405', '', sAlertMessage);
    $("#btnErrorOkCancel").click(function () {
        
        if ($("#pErrorMsg")[0]?.innerText != undefined && $("#pErrorMsg")[0]?.innerText.indexOf("The selected appointment is") > -1) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            if ($('#ctl00_C5POBody_btnRefresh')[0] != undefined) {

                $("#ctl00_C5POBody_hdnStopLoading")[0].value = "true"
                $(top.window.document).find("#ctl00_C5POBody_btnRefresh")[0].click();
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        }
       
    });
}




//-----------------