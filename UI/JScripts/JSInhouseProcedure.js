
function OnDLCLoad() {
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
function chklstOtherProcedures_ItemChecked(sender, args) {


    var elementRef = document.getElementById("chklstOtherProcedures");
    var checkBoxArray = elementRef.getElementsByTagName('input');
    document.getElementById("Hidden1").value = "";

    $find('btnAdd').set_enabled(false);
    EnableSaveDiagnosticOrder('false');

    document.getElementById("txtProcedure").value = "";

    for (var i = 0; i < checkBoxArray.length; i++) {
        var checkBoxRef = checkBoxArray[i];


        if (checkBoxRef.checked == true) {
            $find('btnAdd').set_enabled(true);

            EnableSaveDiagnosticOrder('true');
            if (document.getElementById("txtProcedure").value == "") {
                document.getElementById("txtProcedure").value = checkBoxRef.parentElement.innerText;
                document.getElementById("Hidden1").value = checkBoxRef.parentElement.innerText
            }
            else {
                document.getElementById("Hidden1").value += "^" + checkBoxRef.parentElement.innerText;
                document.getElementById("txtProcedure").value += ", " + checkBoxRef.parentElement.innerText;
            }

            checkBoxRef.selected = false;
        }
    }







}

function procedureClear() {


    document.getElementById("txtProcedure").value = "";

    document.getElementById("Hidden1").value = "";

    var elementRef = document.getElementById("chklstOtherProcedures");
    var checkBoxArray = elementRef.getElementsByTagName('input');



    for (var i = 0; i < checkBoxArray.length; i++) {
        checkBoxArray[i].checked = false;

    }

    return false;

}

function clearall() {
    document.getElementById("txtProcedure").value = "";
    document.getElementById("Hidden1").value = "";



    var elementRef = document.getElementById("chklstOtherProcedures");
    var checkBoxArray = elementRef.getElementsByTagName('input');



    for (var i = 0; i < checkBoxArray.length; i++) {
        checkBoxArray[i].checked = false;

    }

    $find("ctmDLC_procedure_txtDLC").clear();
}







function grdProcedure_OnCommand(sender, args) {
    if (args.get_commandName() == "PDelete") {
        var IsClearAll = DisplayErrorMessage('280004');
        if (IsClearAll == true) {
            document.getElementById("grdProcedure").click();

        }
        else {

            args._cancel = true;
        }
    }
}

function btnClearAll_Clicked_Old(sender, args) {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); 
        $(lblProcedure).html($(lblProcedure).html().replace("*", "<span class='manredforstar'>*</span>"));
    }

    var IsClearAll;
    if (sender._text == "Clear All")
        IsClearAll = DisplayErrorMessage('200005');
    else
        IsClearAll = DisplayErrorMessage('290020');
    if (IsClearAll == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatC
            hart(); }
        document.getElementById("Hidden1").value = "";
        document.getElementById("txtProcedure").value = "";
        document.getElementById("ctmDLC_procedure_txtDLC").value = "";

        var elementRef = document.getElementById("chklstOtherProcedures");
        var checkBoxArray = elementRef.getElementsByTagName('input');

        for (var i = 0; i < checkBoxArray.length; i++) {
            checkBoxArray[i].checked = false;

        }

        if (sender._text != "Clear All") {
            $find('btnTestArea').set_enabled(false);
            $find('btnAdd').set_enabled(false);
            EnableSaveDiagnosticOrder('false');
        }

        $find("btnAdd").set_text("Add");
        $find("btnClearAll").set_text("Clear All");
        $find('btnAdd').set_enabled(false);
        EnableSaveDiagnosticOrder('false');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    else {
        args._cancel = true;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}





function chklstOtherProcedures_SelectedIndexChanged(sender, args) {
    var elementRef = document.getElementById("chklstOtherProcedures");
    var checkBoxArray = elementRef.getElementsByTagName('input');

    var checkBoxRef = checkBoxArray[sender._selectedIndices[0]];


    if (checkBoxRef.checked == false) {

        checkBoxRef.checked = true;
    }
    else {
        checkBoxRef.checked = false;
    }

    sender.clearSelection();
    chklstOtherProcedures_ItemChecked(sender, args);
}

function OpenTestArea() {
    Result = OpenModal("safds");
    if (Result != null) {
        Result.IsEnabled
    }
}

function RefreshProcedure() {
    document.getElementById("btnRefreshProce").click();
    ClickSaveButton();
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
    if (oWnd) {
        oWnd.close();
    }
}

function CellSelected(value, args) {
    {
        sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
    }

    if (value != undefined)
        document.getElementById('hdnDelImmuniztionId').value = value;

    var continuee = DisplayErrorMessage('180016');

    if (continuee == undefined) {
        document.getElementById('hdnDelImmuniztionId').value = value;
    }
    if (continuee == true) {
        document.getElementById('InvisibleButton').click();
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}

function CCTextChanged() {
    if ($find('btnAdd') != null) {
        $find('btnAdd').set_enabled(true);
        EnableSaveDiagnosticOrder('true');
    }
    else {
        document.getElementById('btnAdd').disabled = false;
        EnableSaveDiagnosticOrder('true');
    }
}




function GetKeyPress() {
    if ($find('btnAdd') != null) {
        $find('btnAdd').set_enabled(true);

        EnableSaveDiagnosticOrder('true');
    }
    else {
        document.getElementById('btnAdd').disabled = false;

        EnableSaveDiagnosticOrder('true');
    }
}

function OnClientCloseLabProcedureManager(oWnd, args) {

    RefreshProcedure();
}

function Closed(oWnd, args) {

    var arg = args.get_argument();

    if (arg) {
        if (arg.IsEnabled == "TRUE") {
            $find("btnAdd").click();

        }
    }
}

function EnableSaveDiagnosticOrder(IsEnable) {

    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = IsEnable;
    if (IsEnable != "true") {
        localStorage.setItem("bSave", "true");
    }
    else {
        localStorage.setItem("bSave", "false");
    }
    var div = document.getElementById("pnlCheckedList");
    var div_position = document.getElementById("div_position");
    if (document.getElementById("div_position") != null) {
        var position = parseInt(document.getElementById("div_position").value);
        if (isNaN(position)) {
            position = 0;
        }
        div.scrollTop = position;
    }
}
function btnManageFrequentlyUsed_Click() {
    
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false });
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "85%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "73%";
    $(top.window.document).find("#TabModalTitle")[0].textContent = "Manage Frequently Used Procedures";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmLabProcedureManage.aspx?procedureType=" + "OTHER PROCEDURE";
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
        OnClientCloseLabProcedureManager();
    });
    return false;
}


function btnAdd_Clicked(sender, args) {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;

  { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

}
function btnAddImplantable(sender, args) {
   sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
}

function btnClearAll_Clicked(sender, args) {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var IsClearAll;

    var errMessage;

    if (sender == undefined || sender._text == "Clear All")
        errMessage = '200005';
    else
        errMessage = '290020';

    if (DisplayErrorMessage(errMessage)) {

        document.getElementById("Hidden1").value = "";
        document.getElementById("txtProcedure").value = "";
        document.getElementById("ctmDLC_procedure_txtDLC").value = "";

        var elementRef = document.getElementById("chklstOtherProcedures");
        var checkBoxArray = elementRef.getElementsByTagName('input');

        for (var i = 0; i < checkBoxArray.length; i++) {
            checkBoxArray[i].checked = false;
        }

        $find('btnAdd').set_enabled(false);
       
        EnableSaveDiagnosticOrder('false');
        document.getElementById('btnClear').click();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}

function BeforeCloseForTestArea(oWnd, args) {


    var arg = args.get_argument();

    if (arg) {
        var result = arg.IsEnabled;
        if (result == "TRUE") {
            $find("btnAdd").click();


        }
    }

}
function OnLoadProcedure() {

    if (window.parent.parent.theForm.hdnSaveButtonID != undefined && window.parent.parent.theForm.hdnSaveButtonID != null)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnAdd,mulPageOrders";
    if (window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != undefined && window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != null)
        window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID.value = "btnAdd,mulPageOrders";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}


function txtProcedure_OnKeyPress(sender, args) {
    EnableSaveReferralOrder(event);
    //Add JavaScript handler code here
    var reg = /[^-\sa-zA-Z]/;
    if (reg.test(args.get_keyCharacter())) {
        args.set_cancel(true);
    } else {
        args.set_cancel(false);
    }


}

function btnPlan_Clicked(sender, args) {
    var obj = new Array();
    openModal("frmAddorUpdatePlan.aspx", 445, 860, obj, "MessageWindow");
}


function Savedsuccessfully() {

    DisplayErrorMessage('295001');
    top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    Order_AfterAutoSave();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}



function OnItemCommand(sender, eventArgs) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById('hdnSelectedIndex').value = eventArgs.get_commandArgument();
}
function Scrolling() {
    document.getElementById('div_position').value = document.getElementById('pnlCheckedList').scrollTop;
}

function EnableFind() {
    if (document.getElementById('btnFind') != undefined && document.getElementById('btnFind') != null) {
        document.getElementById('btnFind').disabled = false;
    }
    GetKeyPress();
}

function FindLoading() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
//BugID:53431
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnSave')[0].disabled = true;
}

function ImplantableScreenOpen(isUDI) {
    var Updatekey = document.getElementById("hdngroupkey").value;
    var saveupdate = document.getElementById("IsSaveOrUpdate").value;
    var EncId = document.getElementById("hEncID").value;
    var Prodecure = null;
    var ProNotes = null;
    if (isUDI=="true")
    {
         Prodecure = document.getElementById("hProcedure").value;
         ProNotes = document.getElementById("hNotes").value;
    }
    else
    { 
         Prodecure = document.getElementById("txtProcedure").value;
         ProNotes = document.getElementById("ctmDLC_procedure_txtDLC").value;
    }
    $(top.window.document).find("#TabModalImplantable").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#TabmdldlgImplantable")[0].style.width = "85%";
    $(top.window.document).find("#TabmdldlgImplantable")[0].style.height = "80%";
    $(top.window.document).find("#TabModalTitleImplantable")[0].textContent = "Manage Implantable Device";
    $(top.window.document).find("#TabFrameImplantable")[0].style.height = "95%";
    $(top.window.document).find(".modal-body")[8].style.height = "80%";
    var ScreenMode = '';
    //jira cap - 513
    if (document.URL.split('&')[3] != undefined && document.URL.split('&')[3].indexOf('ScreenMode') > -1) {
        ScreenMode = document.URL.split('&')[3].split('=')[1];
    }
    $(top.window.document).find("#TabFrameImplantable")[0].contentDocument.location.href = "frmImplantableDevice.aspx?ProcedureCode=" + Prodecure + "&Notes=" + ProNotes + "&Issaveorupdate=" + saveupdate + "&UpdateKeyValue=" + Updatekey + "&EncounterID=" + EncId + "&ScreenMode=" + ScreenMode;

    document.getElementById("hdngroupkey").value = "";
    document.getElementById("IsSaveOrUpdate").value = "";
    document.getElementById("hProcedure").value = "";
    document.getElementById("hNotes").value = "";
    document.getElementById("hEncID").value = "";
}

function OnClientCloseImplantableDevice(sender, eventArgs) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function EnableSaveImplantableDiagnosticOrder(IsEnable) {
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = IsEnable;
    if (IsEnable != "true") {
        localStorage.setItem("bSave", "true");
    }
    else {
        localStorage.setItem("bSave", "false");
    }
    $(top.window.document).find("#btnCloseImplantable").click();
    $(top.window.document).find("#TabModalImplantable").css('display', 'none');
    if ($(window.parent.document).find("#RadWindowWrapper_ctl00_ModalWindow") != undefined && $(window.parent.document).find("#RadWindowWrapper_ctl00_ModalWindow").length > 0 && $($(window.parent.document).find("#RadWindowWrapper_ctl00_ModalWindow")).find("iframe") != undefined) {
        var frame = $($($(window.parent.document).find("#RadWindowWrapper_ctl00_ModalWindow")).find("iframe")[0].contentDocument).find("iframe[id=iframeOrderPatientBar]");
        if (frame != undefined)
            $($($(frame[0].contentDocument).find(".tab-pane.active")).find("iframe")[0].contentDocument).find("#btnInvisibleImplant").click();
    }
    else
    {
    $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
    $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[8].click();
    localStorage.setItem("Implant", "Procedures")
    }

}


function btnClearAllImplantable_Clicked(sender, args) {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var IsClearAll;

    var errMessage;

    if (sender == undefined || sender._text == "Clear All")
        errMessage = '200005';
    else
        errMessage = '290020';

    if (DisplayErrorMessage(errMessage)) {
        document.getElementById("txtSerialNumber").value = "";
        document.getElementById("txtLotorBatch").value = "";
        document.getElementById("txtManufacturedDate").value = "";
        document.getElementById("txtExpirationDate").value = "";
        document.getElementById("txtDistinctID").value = "";
        document.getElementById("txtIssuingAgency").value = "";
        document.getElementById("txtBrandName").value = "";
        document.getElementById("txtVersionModel").value = "";
        document.getElementById("txtManufacturedDate").value = "";
        document.getElementById("txtCompanyName").value = "";
        document.getElementById("txtMRISafetyStatus").value = "";
        document.getElementById("txtGMDNPTName").value = "";
        document.getElementById("txtDescription").value = "";
        document.getElementById("txtRubberContent").value = "";
        document.getElementById("txtDeviceIdentifier").value = "";
        if (document.getElementById('btnFind') != undefined && document.getElementById('btnFind') != null) {
            document.getElementById('btnFind').disabled = true;
        }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}



function CloseImplantable() {
    if (!document.getElementById('btnAdd').disabled) {
        event.preventDefault();

        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
            "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';

        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'left' + " " + 'center',
                at: 'center' + " " + 'center + 100px'
            },
            buttons: {
                "Yes": function () {
                    if (document.getElementById('hdnBtnFind').value == "" && document.getElementById('txtDeviceIdentifier').value != "" && document.getElementById('txtDeviceIdentifier').readOnly == false) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('280021');
                        document.getElementById('btnFind').disabled = false;
                        return;
                    }
                    else if (document.getElementById('txtProcedure').value == "99999-Implantable Devices" && document.getElementById('txtDeviceIdentifier').value == "" && document.getElementById('hdnBtnFind').value == "") {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('280022');
                        return;
                    }
                    else if ($('#txtDescription').val() == "") {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        DisplayErrorMessage('280023');
                        return;
                    }
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $find('btnAdd').set_enabled(true);
                    EnableSaveDiagnosticOrder('true');
                    $find('btnAdd').click();
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btnCloseImplantable").click();
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btnCloseImplantable").click();
                    return false;
                }
            }
        });
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        $(top.window.document).find("#btnCloseImplantable").click();
        return false;
    }
}