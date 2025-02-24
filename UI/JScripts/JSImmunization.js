
function chklstImmunizationProcedures_ItemChecked(sender, args) {
    $find('btnTestArea').set_enabled(true);
    var elementRef = document.getElementById("chklstImmunizationProcedures");
    var checkBoxArray = elementRef.getElementsByTagName('input');
    document.getElementById("txtImmunizationProcedure").value = "";
    for (var i = 0; i < checkBoxArray.length; i++) {
        var checkBoxRef = checkBoxArray[i];
        if (checkBoxRef.checked == true) {
            document.getElementById("txtImmunizationProcedure").value = checkBoxRef.parentElement.innerText;
        }
    }
}

function DateVisible(chk) {
    EnableSave();
    if (chk.name == "chkYes" && chk.checked == true)
        document.getElementById("chkNo").checked = false;
    else if (chk.name == "chkYes" && chk.checked == false)
        document.getElementById("chkNo").checked = true;
    else if (chk.name == "chkNo" && chk.checked == false)
        document.getElementById("chkYes").checked = true;
    else if (chk.name == "chkNo" && chk.checked == true)
        document.getElementById("chkYes").checked = false;

    var chkBox = document.getElementById("chkNo").checked;

    var div_to_disable = document.getElementById("pnlVaccineAdminDetails").getElementsByTagName("input");
    var children = div_to_disable; //.childNodes;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = chkBox;
    }

    document.getElementById("TbDis").disabled = chkBox;
    document.getElementById("pnlVaccineAdminDetails").disabled = chkBox;
    if (chkBox == true) {
        $find('cboRefused').disable(true);
        $find('cboEligibility').disable(true);
        $find('cboInformationSource').disable(true);
        $find('cboObservation').disable(true);
        $find('cboManufacturer').disable(true);
        $find('cboImmunizationSource').disable(true);
        $find('cboAdminUnit').disable(true);
        $find('cboLocation').disable(true);
        $find('cboDoseno').disable(true);
        $find("cboEligibility").clearSelection();
        $find("cboInformationSource").clearSelection();
        $find("cboObservation").clearSelection();
        $find("cboManufacturer").clearSelection();
        $find("cboImmunizationSource").clearSelection();
        $find("cboAdminUnit").clearSelection();
        $find("cboLocation").clearSelection();
        $find("cboDoseno").clearSelection();
        $find('txtNDC').disable();
        $find('txtLotNumber').disable();
        $find('txtAdminAmt').disable();
        $find('txtGivenBy').disable();
        $find('txtDose').disable();
        $find("txtAdminAmt").clear();
        $find("txtGivenBy").clear();
        $find("txtLotNumber").clear();
        $find("txtNDC").clear();
        $find("txtDose").clear();
        $find("dtpExpiryDate").clear();
        //$find("dtpGivenDate").clear();
        $find('dtpExpiryDate').set_enabled(false);
        $find('dtpGivenDate').set_enabled(false);
        document.getElementById("chkRefused").checked = false;
    }
    else {
        $find('cboRefused').disable(true);
        $find('cboEligibility').enable(true);
        $find('cboInformationSource').enable(true);
        $find('cboObservation').enable(true);
        $find('cboManufacturer').enable(true);
        $find('cboImmunizationSource').enable(true);
        $find('cboAdminUnit').enable(true);
        $find('cboLocation').enable(true);
        $find('cboDoseno').enable(true);
        $find("cboRefused").clearSelection();
        $find('txtNDC').enable();
        $find('txtLotNumber').enable();
        $find('txtAdminAmt').enable();
        $find('txtGivenBy').enable();
        $find('txtDose').enable();
        $find('dtpExpiryDate').set_enabled(true);
        $find('dtpGivenDate').set_enabled(true);
        document.getElementById("chkRefused").checked = false;
    }
    return false;
}

function DateDisplay(chkBox) {

    GetUTCTime();
    var raddatepickerDateOnVis = $find("dtpDateOnVis");
    var raddatepickerVisGiven = $find("dtpVisGiven");
    if (chkBox.checked == true) {
        var today = new Date();
        raddatepickerDateOnVis.set_enabled(true);
        setTimeout(function () { raddatepickerDateOnVis.set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
        raddatepickerVisGiven.set_enabled(true);
        setTimeout(function () { raddatepickerVisGiven.set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
    }
    else {

        var today = new Date();
        raddatepickerDateOnVis.set_enabled(true);
        setTimeout(function () { raddatepickerDateOnVis.set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
        raddatepickerVisGiven.set_enabled(true);
        setTimeout(function () { raddatepickerVisGiven.set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
    }
    return false;
}

function grdImmunizations_OnCommand(sender, args) {
    if (args.get_commandName() == "Del") {
        var IsClearAll = DisplayErrorMessage('280004');
        if (IsClearAll == true) {
            document.getElementById("grdImmunizations").click();
        }
        else {
            args._cancel = true;
        }
    }
}

function DeleteGrid() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var delete_row = DisplayErrorMessage('280004');
    if (delete_row == true) {
        document.getElementById("btnInvisible").click();
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}

function btnClearAll_Clicked(sender, args) {
    var IsClearAll;
    if (sender == undefined || sender._text == "Clear All")
        IsClearAll = DisplayErrorMessage('200005');
    else
        IsClearAll = DisplayErrorMessage('290020');
    if (IsClearAll == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $find("txtImmunizationProcedure").clear();
        $find("txtCVXCode").clear();
        $find("txtLotNumber").clear();
        $find("cboVFC").clearSelection();
        $find("txtAdminAmt").clear();
        $find("cboRouteOfAdministration").clearSelection();
        $find("cboImmunizationSource").clearSelection();
        $find("cboLocation").clearSelection();
        $find("cboAdminUnit").clearSelection();
        $find("cboImmunizationSource").clearSelection();
        $find("cboManufacturer").clearSelection();
        $find("cboDoseno").clearSelection();
        $find("txtDose").clear();
        $find("txtGivenBy").clear();
        $find("txtNDC").clear();
        $find("txtAdminAmt").clear();
        var elementRef = document.getElementById("chklstImmunizationProcedures");
        var checkBoxArray = elementRef.getElementsByTagName('input');
        for (var i = 0; i < checkBoxArray.length; i++) {
            checkBoxArray[i].checked = false;
        }

        document.getElementById("txtNotes_txtDLC").value = "";
        document.getElementById("txtDLCDocumentType_txtDLC").value = "";
        $find("dtpExpiryDate").clear();

        $find("dtpDateOnVis").clear();
        $find("dtpGivenDate").clear();
        $find("dtpExpiryDate").clear();
        $find("dtpVisitDate").clear();
        setTimeout(function () { $find('dtpVisitDate').set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
        $find("dtpVisGiven").clear();
        setTimeout(function () { $find('dtpVisGiven').set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
        $find("dtpDateOnVis").clear();
        setTimeout(function () { $find('dtpDateOnVis').set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }, 0);
        document.getElementById("chkRefused").checked = false;
        $find("cboRefused").clearSelection();
        $find("cboRefused").set_enabled(false);
        $find("cboEligibility").clearSelection();
        $find("cboInformationSource").clearSelection();
        $find("cboObservation").clearSelection();
        $find("cboImmunizationEve").clearSelection();
        $find("cboEligibility").set_enabled(true);
        $find("cboInformationSource").set_enabled(true);
        $find("cboObservation").set_enabled(true);

        document.getElementById('btnAdd_SpanAdd').textContent = "A";
        document.getElementById('btnAdd_SpanAdditionalword').textContent = "dd";
        $find("btnAdd")._text = "Add";
        document.getElementById('btnClearAll_SpanClear').textContent = "C";
        document.getElementById('btnClearAll_SpanClearAdditional').textContent = "lear All";
        $find("btnClearAll")._text = "Clear All";
        $find("btnTestArea").set_enabled(false);
        document.getElementById("btnClear").click();
        GetUTCTime();
    }
    else {
        if (args != undefined)
            args._cancel = true;
    }
}

function RefreshImmunization() {
    document.getElementById("btnRefreshImm_proce").click();
    //CAP-2930
    disableAutoSave();
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    return oWindow;
}

function ClickSaveButton() {
    var oWnd = GetRadWindow();
    oWnd.close();
}

function EnableSaveDiagnosticOrder(IsEnable) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = IsEnable;
    if (IsEnable != "true") {
        localStorage.setItem("bSave", "true");
    }
    else {
        localStorage.setItem("bSave", "false");
    }

}

function Closed(oWnd, args) {
    var arg = args.get_argument();
    alert("1");
    if (arg) {
        var result = arg.IsEnabled;
        if (result == "TRUE") {
            alert($find("btnAdd"));
            $find("btnAdd").click();
        }
    }
}

function BeforeCloseForTestArea(oWnd, args) {
    var arg = args.get_argument();
    if (arg) {
        var result = arg.IsEnabled;
        if (result == "TRUE") {
            GetUTCTime();
            document.getElementById("InvisibleButton").click();
        }
    }
}

function EnableSave() {
    if (document.getElementById("hdnLoad") != null) {
        if (document.getElementById("hdnLoad").value == "true") {
            document.getElementById("hdnLoad").value = "false";
            //Jira Cap-456 - JS error fixed
            if ($find('btnAdd') != null) {
                $find('btnAdd').set_enabled(false);
            }
            else {
                if ($("#btnAdd") != null) {
                    $("#btnAdd").prop("disabled", true);
                }
            }
            EnableSaveDiagnosticOrder('false');
        }
        else {
            //Jira Cap-456 - JS error fixed
            if ($find('btnAdd') != null) {
                $find('btnAdd').set_enabled(true);
            }
            else {
                if ($("#btnAdd") != null) {
                    $("#btnAdd").prop("disabled", false);
                }
            }
            EnableSaveDiagnosticOrder('true');
        }
    }
    else {
        //Jira Cap-456 - JS error fixed
        if ($find('btnAdd') != null) {
            $find('btnAdd').set_enabled(true);
        }
        else {
            if ($("#btnAdd") != null) {
                $("#btnAdd").prop("disabled", false);
            }
        }
        EnableSaveDiagnosticOrder('true');
    }
}

function btnManageFrequentlyUsedImmunProc_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    EnableSave();
    if (document.getElementById("hdnCurrentProcess").value != "MA_REVIEW") {
        $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false });
        $(top.window.document).find("#Tabmdldlg")[0].style.width = "85%";
        $(top.window.document).find("#Tabmdldlg")[0].style.height = "73%";
        $(top.window.document).find("#TabModalTitle")[0].textContent = "Manage Frequently Used Procedures";
        $(top.window.document).find("#TabFrame")[0].style.height = "100%";
        $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmLabProcedureManage.aspx?procedureType=IMMUNIZATION PROCEDURE";
        $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            RefreshImmunization();
        });
    }
}
var v1 = 0;
function btnAdd_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();
    localStorage.setItem("bSave", "false");
    var dtpExpiryDate = (new Date($find("dtpExpiryDate")._element.value).getMonth() + 1) + '/' + (new Date($find("dtpExpiryDate")._element.value).getDate() + 1) + '/' + new Date($find("dtpExpiryDate")._element.value).getFullYear();
    var dtpVisitDate = (new Date($find("dtpVisitDate")._element.value).getMonth() + 1) + '/' + (new Date($find("dtpVisitDate")._element.value).getDate() + 1) + '/' + new Date($find("dtpVisitDate")._element.value).getFullYear();
    var dtpGivenDate = (new Date($find("dtpGivenDate")._element.value).getMonth() + 1) + '/' + (new Date($find("dtpGivenDate")._element.value).getDate() + 1) + '/' + new Date($find("dtpGivenDate")._element.value).getFullYear();
    var DOS = (new Date(document.getElementById("hdnDOS").value).getMonth() + 1) + '/' + new Date(document.getElementById("hdnDOS").value).getDate() + '/' + new Date(document.getElementById("hdnDOS").value).getFullYear();

    if ($find("chklstImmunizationProcedures")._checkedIndices.length == 0) {
        DisplayErrorMessage('290010');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $find("txtImmunizationProcedure").focus();
        Order_SaveUnsuccessful();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if (document.getElementById("txtDLCDocumentType_txtDLC").value == "") {
        DisplayErrorMessage('290023');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if (document.getElementById("lblVFC").innerHTML.indexOf('*') != -1 && $find("cboVFC")._text == "") {
        DisplayErrorMessage('290014');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if ($find("txtDose")._text != "") {
        var f = 0;
        if (parseInt($find("txtDose")._text) > 20) {
            f = 1;
        }
        else if ($find("cboDoseno")._text != "") {
            if (parseInt($find("txtDose")._text) > parseInt($find("cboDoseno")._text)) {
                f = 1;
            }
        }
        if (f == 1) {
            DisplayErrorMessage('290008');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            EnableSave();
            Order_SaveUnsuccessful();
            $find("txtDose").focus();
            sender.set_autoPostBack(false);
            v1 = 1;
            return false;
        }
        else {
            if (v1 == 0)
                sender.set_autoPostBack(true);
            else {
                v1 == 0;
                __doPostBack('btnAdd');
            }
        }
    }
    else if (document.getElementById("chkRefused").checked && document.getElementById("cboRefused").value == '' && document.getElementById("lblAllergy").innerText != '') {
        DisplayErrorMessage('290021');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        document.getElementById('cboRefused').focus();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if (document.getElementById("chkRefused").checked && document.getElementById("cboRefused").value == '') {
        DisplayErrorMessage('290021');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        document.getElementById('cboRefused').focus();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if (new Date(dtpExpiryDate) < new Date(DOS) && $find("dtpExpiryDate")._element.value != "") {
        DisplayErrorMessage('290015');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        $find("dtpExpiryDate").get_dateInput().focus();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if (new Date(dtpGivenDate) < new Date(DOS) && $find("dtpGivenDate")._element.value != "") {
        DisplayErrorMessage('290016');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        Order_SaveUnsuccessful();
        $find('btnAdd').set_enabled(true);
        $find("dtpGivenDate").get_dateInput().focus();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if (new Date(dtpVisitDate) < new Date(DOS) && $find("dtpVisitDate")._element.value != "") {
        DisplayErrorMessage('290017');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        $find("dtpVisitDate").get_dateInput().focus();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else if ($find("txtAdminAmt")._text != "" && $find("txtAdminAmt")._text.includes(".") != true) {
        DisplayErrorMessage('290022');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        EnableSave();
        Order_SaveUnsuccessful();
        $find("txtAdminAmt").focus();
        sender.set_autoPostBack(false);
        v1 = 1;
        return false;
    }
    else {
        //sender.set_autoPostBack(true);
        $find('btnAdd').set_enabled(false);
        EnableSaveDiagnosticOrder('false');
        if (v1 == 0)
            sender.set_autoPostBack(true);
        else {
            v1 == 0;
            __doPostBack('btnAdd');
        }
    }
}

function checkRefused(chk) {
    if (chk.checked == true) {
        $find('cboRefused').enable(true);
        $find('cboEligibility').disable(true);
        $find('cboInformationSource').disable(true);
        $find('cboObservation').disable(true);
        $find('cboManufacturer').disable(true);
        $find('cboImmunizationSource').disable(true);
        $find('cboAdminUnit').disable(true);
        $find('cboLocation').disable(true);
        $find('cboDoseno').disable(true);
        $find("cboEligibility").clearSelection();
        $find("cboInformationSource").clearSelection();
        $find("cboObservation").clearSelection();
        $find("cboManufacturer").clearSelection();
        $find("cboImmunizationSource").clearSelection();
        $find("cboAdminUnit").clearSelection();
        $find("cboLocation").clearSelection();
        $find("cboDoseno").clearSelection();
        $find('txtNDC').disable();
        $find('txtLotNumber').disable();
        $find('txtAdminAmt').disable();
        $find('txtGivenBy').disable();
        $find('txtDose').disable();
        $find("txtAdminAmt").clear();
        $find("txtGivenBy").clear();
        $find("txtLotNumber").clear();
        $find("txtNDC").clear();
        $find("txtDose").clear();
        $find("dtpExpiryDate").clear();
        $find("dtpGivenDate").clear();
        $find('dtpExpiryDate').set_enabled(false);
        $find('dtpGivenDate').set_enabled(false);
        var cmbbx = $find("cboRefused");
        if (cmbbx._itemData.length > 0) {
            var val = cmbbx._itemData[1].value.split('|')[1];
            var cItem = cmbbx.findItemByText(val);
            cItem.select();
        }

    }
    else {
        $find('cboRefused').disable(true);
        $find('cboEligibility').enable(true);
        $find('cboInformationSource').enable(true);
        $find('cboObservation').enable(true);
        $find('cboManufacturer').enable(true);
        $find('cboImmunizationSource').enable(true);
        $find('cboAdminUnit').enable(true);
        $find('cboLocation').enable(true);
        $find('cboDoseno').enable(true);
        $find("cboRefused").clearSelection();
        $find('txtNDC').enable();
        $find('txtLotNumber').enable();
        $find('txtAdminAmt').enable();
        $find('txtGivenBy').enable();
        $find('txtDose').enable();
        $find('dtpExpiryDate').set_enabled(true);
        $find('dtpGivenDate').set_enabled(true);
        setTimeout(function () { if ($find('dtpGivenDate') != null) { $find('dtpGivenDate').set_selectedDate(new Date(document.getElementById("hdnDOS").value)); } }, 0);

    }
    EnableSave();
}



function OnLoadImmunization() {

    if (window.parent.parent.theForm.hdnSaveButtonID != undefined && window.parent.parent.theForm.hdnSaveButtonID != null)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnAdd,mulPageOrders";
    if (window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != undefined && window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != null)
        window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID.value = "btnAdd,mulPageOrders";
    GetUTCTime();
    //setTimeout(function () { if($find('dtpVisitDate')!=null){ $find('dtpVisitDate').set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }}, 0);
    //setTimeout(function () {if($find('dtpVisGiven')!=null){ $find('dtpVisGiven').set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); }}, 0);
    setTimeout(function () { if ($find('dtpDateOnVis') != null) { $find('dtpDateOnVis').set_selectedDate(new Date(document.getElementById("hdnLocalTime").value)); } }, 0);
    //setTimeout(function () { if($find('dtpVisGiven')!=null){$find('dtpVisGiven').set_maxDate(new Date(document.getElementById("hdnLocalTime").value)); }}, 0);
    setTimeout(function () { if ($find('dtpDateOnVis') != null) { $find('dtpDateOnVis').set_maxDate(new Date(document.getElementById("hdnLocalTime").value)); } }, 0);
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}

function SavedSuccessfully() {


    DisplayErrorMessage('290001');
    //SavedSuccessfully_NowProceed(screen_name);
    if (top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable') != undefined && top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable') != null)
        top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    Order_AfterAutoSave();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    RefreshNotification('ImmunizationOrder');
    //CAP-2678
    localStorage.setItem("IsSaveCompleted", true);
}

function DeleteSuccessfully() {


    // DisplayErrorMessage('290001');
    //SavedSuccessfully_NowProceed(screen_name);
    if (top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable') != undefined && top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable') != null)
        top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    Order_AfterAutoSave();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    RefreshNotification('ImmunizationOrder');
}

function chklstImmunizationProcedures_Load() {
    if (document.getElementById('txtAdminAmt').disabled == true) {
        return;
    }
    EnableSave();
    checkRefused(document.getElementById('chkRefused'));
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }


}

function HideAllControls() {
    document.getElementById("divImmunization").style.display = 'none';
    document.getElementById("SummaryAlert").style.display = '';
}

function WindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
    window.top.location.href = "frmMyQueueNew.aspx";//BugID:42368
}

function PrintImmunizationOrderPDF() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Immunization / Injection";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('SelectedItem').value + "&Location=DYNAMIC";
    document.getElementById('SelectedItem').value = "";
    $(top.window.document).find("#PrintPDFModal").one("hidden.bs.modal", function (e) {
        RefreshImmunization();
    });
}
function PrintVIS() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Immunization / Injection";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('hdnSelectedItem').value + "&Location=STATIC";
    document.getElementById('SelectedItem').value = "";
    $(top.window.document).find("#PrintPDFModal").one("hidden.bs.modal", function (e) {
        RefreshImmunization();
    });
}
function GetUTCTime() {
    dt = new Date();
    document.getElementById('hdnLocalTime').value = dt.getFullYear() + "/" + (dt.getMonth() + 1) + "/" + dt.getDate() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
}

function grdImmunizations_OnCommand(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
//BugID:53431
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnAdd')[0].disabled = true;
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}