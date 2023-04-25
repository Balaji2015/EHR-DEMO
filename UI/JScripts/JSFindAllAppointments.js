function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function OpenFindPatinet() {
    setTimeout(
    function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        var oWindow = GetRadWindow();
        var childWindow = $find('ModalWindowAppmnt')._browserWindow.GetRadWindow().BrowserWindow.radopen("frmFindPatient.aspx?ScreenName=Appointments", "ModalWindowAppmnt");
        setRadWindowProperties(childWindow, 251, 1200);
        childWindow.add_close(function refrehCarrier(oWindow, args) {
            var Result = args.get_argument();
            if (Result) {

                document.getElementById("hdnHumanID").value = Result.HumanId;
                document.getElementById("btnFindPatientRefresh").click();
            }
        });
    }, 0);
    return false;
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
function OpenCancelAppt() {
    WaitCursor();
    var index = parseInt(document.getElementById('hdnSelectedIndex').value) + 1;
    if (isNaN(index)) {
        DisplayErrorMessage("110027");
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return false;
    }
    var grid = $find("grdAppointment");
    var MasterTable = grid.get_masterTableView();
    var selectedRows = MasterTable.get_selectedItems();
    if (grid != null) {
        for (var i = 0; i < selectedRows.length; i++) {
            var row = selectedRows[i];
            Encounterid = MasterTable.getCellByColumnUniqueName(row, "Appt_ID").innerHTML;
            var is_archieve = MasterTable.getCellByColumnUniqueName(row, "Is_Archieve").innerHTML;
            if (is_archieve == "Main") {

                setTimeout(
       function () {
           var oWindow = GetRadWindow();
           var childWindow = $find('ModalWindowAppmnt')._browserWindow.GetRadWindow().BrowserWindow.radopen("frmCancelAppointment.aspx?EncounterID=" + Encounterid, "ModalWindowMngt");
           setRadWindowProperties(childWindow, 240, 520);
           childWindow.add_close(function refrehCarrier(oWindow, args) {
               document.getElementById("btnRefresh").click();
           });
       }, 0);
            }
            else {
                DisplayErrorMessage("110093");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            return false;
        }
    }
}
function OpenEditAppointment() {
    WaitCursor();
    var index = parseInt(document.getElementById('hdnSelectedIndex').value) + 1;
    if (isNaN(index)) {
        DisplayErrorMessage("110027");
        {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return false;
    }
    var grid1 = $find("grdAppointment");
    var MasterTable1 = grid1.get_masterTableView();
    var selectedRows = MasterTable1.get_selectedItems();
    if (grid1 != null) {
        for (var i = 0; i < selectedRows.length; i++) {
            var row = selectedRows[i];
            var humanID = document.getElementById("hdnHumanID").value;
            var fac = MasterTable1.getCellByColumnUniqueName(row, "FacilityName").innerHTML;
            var encounterId = MasterTable1.getCellByColumnUniqueName(row, "Appt_ID").innerHTML;
            var currentProcess = MasterTable1.getCellByColumnUniqueName(row, "CurrentProcess").innerHTML;
            var selectedDate = MasterTable1.getCellByColumnUniqueName(row, "AppointmentDate").innerHTML + " " + MasterTable1.getCellByColumnUniqueName(row, "AppointmentTime").innerHTML;
            var physicianName = MasterTable1.getCellByColumnUniqueName(row, "ProviderName").innerHTML;
            var physicianId = MasterTable1.getCellByColumnUniqueName(row, "Appt_Provider_Id").innerHTML;
            var is_archieve = MasterTable1.getCellByColumnUniqueName(row, "Is_Archieve").innerHTML;
            //Jira #CAP-68
            //if (is_archieve == "Main") {
                var Facility;
                if (fac.indexOf("#") != -1) {
                    Facility = fac.replace("#", "_");
                }
                else {
                    Facility = fac;
                }

                if (humanID != undefined) {
                    setTimeout(
    function () {
        var oWindow = GetRadWindow();
        var childWindow = $find('ModalWindowAppmnt')._browserWindow.GetRadWindow().BrowserWindow.radopen("frmEditAppointment.aspx?Human_id=" + humanID + "&EncounterID=" + encounterId + "&facility=" + Facility + "&PhysicianName=" + physicianName + "&PhysicianID=" + physicianId + "&SelectedDate=" + selectedDate + "&CurrentProcess=" + currentProcess, "ModalWindowMngt");
        setRadWindowProperties(childWindow, 800, 840);
        childWindow.add_close(function refrehCarrier(oWindow, args) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            document.getElementById("btnRefresh").click();
        });
    }, 0);
            }
//Jira #CAP-68
            //}
            //else {
            //    DisplayErrorMessage("110092");
            //    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            //    return false;
            //}
            return false;
        }
    }
   



}
function CloseWindow() {
   
    self.close();
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
function WaitCursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function grdAppointment_OnRowClick(sender, args) {
    var grdFindPatient = $find('grdAppointment');
    document.getElementById('hdnSelectedIndex').value = args._itemIndexHierarchical;
    var index = parseInt(document.getElementById("hdnSelectedIndex").value);
    var MasterTable = grdFindPatient.get_masterTableView();
    row = MasterTable.get_dataItems()[index];
    if (MasterTable.getCellByColumnUniqueName(row, "CurrentProcess").innerHTML == "SCHEDULED") {
        document.getElementById("btnCancelAppointment").disabled = false;
    }
    else {
        document.getElementById("btnCancelAppointment").disabled = true;
    }
    if (MasterTable.getCellByColumnUniqueName(row, "CurrentProcess").innerHTML == "MA_PROCESS") {
        document.getElementById("btnEditAppointment").disabled = true;
    }
    else {
        document.getElementById("btnEditAppointment").disabled = false;
    }
}