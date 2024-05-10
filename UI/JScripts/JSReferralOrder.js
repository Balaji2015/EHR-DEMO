function OpenFindReferralPhysician(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var obj = new Array();
    var ResultFindReferralPhysician = openModal("frmFindReferralPhysician.aspx", 256, 930, obj, "RadWindow1");
    var objWindow = $find("RadWindow1");
    objWindow.add_close(OnclientCloseFindPhysician);
    sender.set_autoPostBack(false);
}

function OpenSpecialityDiagonsis()
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    $(top.window.document).find("#TabModal").modal({ backdrop: "static", keyboard: false }, 'show');
    if (document.getElementById('hdnUserName') != null)
        $(top.window.document).find("#TabModalTitle")[0].textContent = "All Diagnosis" + " - " + document.getElementById('hdnUserName').value;
    else
        $(top.window.document).find("#TabModalTitle")[0].textContent = "All Diagnosis";
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "54%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "67%";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find(".modal-body")[8].style.height = "92.5%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmSpecialityDiagnosis.aspx?sourceScreen=" + "ORDERS";
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function (e) {
        OnClientCloseDiagnosisRef(null, e);
    });
    return false;
}

function SetRadWindowProperties(childWindow, height, width)
{
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}


function AllowalphabetsReferralOrder(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if ((charCode < 65 || charCode > 90) && (charCode < 97 || charCode > 123) && charCode != 32)
        return false;
    return true;

}

function OpenAddOrUpdatePlan() {
    var obj = new Array();
    var id = document.getElementById("hdnhumanid").value;
    var Encid = document.getElementById("hdnEncID").value;
    var Phyid = document.getElementById("hdnPhyId").value;

    obj.push("humanid=" + id);
    obj.push("EncId=" + Encid);
    obj.push("PhyId=" + Phyid);
    var ResultSpecialityDiagonsis = openModal("frmAddorUpdatePlan.aspx", 400, 900, obj, "RadWindow1");
    $find('btnPlan').set_autoPostBack(false);
}

function OpenPDF() {
    var obj = new Array();
    obj.push("SI=" + document.getElementById('SelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () {
        $find('RadWindow1').BrowserWindow.openModal('frmPrintPDF.aspx', 750, 900, obj, 'RadWindow1');
    }, 0);
}
function PrintReferralOrderPDF()
{
    if ($("#btnAddRefOrder") != null && $("#btnAddRefOrder") != undefined && document.getElementById('btnAddRefOrder').attributes["disabled"] !=undefined &&  document.getElementById('btnAddRefOrder').attributes["disabled"].value == "disabled")
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: "static", keyboard: false }, 'show');
    //CAP-1689
    $(top.window.document).find("#PrintPDFModal")[0].style.top = "-30px";
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Print Referral Orders";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.maxWidth = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "100%";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.maxHeight = "680px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "100%";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "100%";
    $(top.window.document).find("#PrintPDFFrame")[0].style.maxHeight = "600px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('SelectedItem').value + "&Location=DYNAMIC";
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}


function OpenFindPhysician() {
    var obj = new Array();
    var ResultFindReferralPhysician = openModal("frmFindReferralPhysician.aspx", 256, 930, obj);
    if (ResultFindReferralPhysician != undefined) {
        var elementRef = document.getElementById(GetClientId("hdnTransferVaraible"));
        elementRef.value = "sPhyName=" + ResultFindReferralPhysician.sPhyName + "$" + "sPhySpecialty=" + ResultFindReferralPhysician.sPhySpecialty + "$" + "sPhyFacility=" + ResultFindReferralPhysician.sPhyFacility + "$" + "ulPhyId=" + ResultFindReferralPhysician.ulPhyId;
    }
}

function AllowNumbers(evt) {
    EnableSaveReferralOrder(event);
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function LettersWithSpaceOnly(evt) {
    EnableSaveReferralOrder(event);
    evt = (evt) ? evt : event;
    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
        ((evt.which) ? evt.which : 0));
    if (charCode > 32 && (charCode < 65 || charCode > 90) &&
        (charCode < 97 || charCode > 122)) {
        return false;
    }
    return true;
}

function OnclientCloseFindPhysician(oWindow, args) {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}   
    var arg = args.get_argument();
    if (arg) {
        if (document.getElementById('btnAddRefOrder') != null) {
            document.getElementById('btnAddRefOrder').disabled = false;
            localStorage.setItem("bSave", "false");
        }
        EnableSaveReferralOrder(event);
        var ResultFindReferralPhysician = arg;
        if (ResultFindReferralPhysician != undefined) {
            var elementRef = document.getElementById("hdnTransferVaraible");
            elementRef.value = "sPhyName=" + ResultFindReferralPhysician.sPhyName + "$" + "sPhySpecialty=" + ResultFindReferralPhysician.sPhySpecialty + "$" + "sPhyFacility=" + ResultFindReferralPhysician.sPhyFacility + "$" + "ulPhyId=" + ResultFindReferralPhysician.ulPhyId;
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            __doPostBack($find('btnFindPhysician')._uniqueID);

        }
    }
}

function refOrderValidation(sender, args) {
    //CAP-1435
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //CAP-1176 - remove all the return statement.
    var isSuccess = true;
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    if ($find('txtProviderName')._text.trim() == '' && isSuccess) {
        DisplayErrorMessage('720011');
        sender.set_autoPostBack(false);
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        Order_SaveUnsuccessful();
        isSuccess = false;
    } else if ($find('cboSpecialty')._text.trim() == '' && isSuccess) {
        DisplayErrorMessage('720003');
        sender.set_autoPostBack(false);
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        Order_SaveUnsuccessful();
        isSuccess = false;
    } else if (document.getElementById('rbtnYes').enabled == true && $find('txtAuthorizationNumber')._text.trim() == '' && isSuccess) {
        DisplayErrorMessage('720010');
        sender.set_autoPostBack(false);
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        Order_SaveUnsuccessful();
        isSuccess = false;
    }
    if (document.getElementById('txtReasonForReferral_txtDLC').value.trim() == '' && isSuccess) {
        DisplayErrorMessage('720009');
        sender.set_autoPostBack(false);
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        Order_SaveUnsuccessful();
        isSuccess = false;
    }
    if (document.getElementById(GetClientId("msktxtFacilityPhoneNumber")).value.length != 0 && PhNoValid(GetClientId("msktxtFacilityPhoneNumber")) == false && document.getElementById(GetClientId("msktxtFacilityPhoneNumber")).value != "(___) ___-____" && isSuccess) {

        DisplayErrorMessage('420005');
        sender.set_autoPostBack(false);
        document.getElementById(GetClientId("msktxtFacilityPhoneNumber")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        isSuccess = false;
    }
    if (document.getElementById(GetClientId("msktxtFacilityFaxNumber")).value.length != 0 && PhNoValid(GetClientId("msktxtFacilityFaxNumber")) == false && document.getElementById(GetClientId("msktxtFacilityFaxNumber")).value != "(___) ___-____" && isSuccess) {

        DisplayErrorMessage('420013');
        sender.set_autoPostBack(false);
        document.getElementById(GetClientId("msktxtFacilityFaxNumber")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        isSuccess = false;
    }
    if (!DateValidattion("dtpValidTill") && isSuccess)
    {
        DisplayErrorMessage('380006');
        sender.set_autoPostBack(false);
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        isSuccess = false;
    }
    if (document.getElementById(GetClientId("msktxtFacilityZipCode")).value.length != 0 && document.getElementById(GetClientId("msktxtFacilityZipCode")).value != "_____-____") {
        var str = document.getElementById(GetClientId("msktxtFacilityZipCode")).value;
        if (str.replace(/_/gi, "").length != 6 && str.replace(/_/gi, "").length != 10 && isSuccess) {

            DisplayErrorMessage('420050');
            sender.set_autoPostBack(false);
            document.getElementById(GetClientId("msktxtFacilityZipCode")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            isSuccess = false;
        }
        else {
            sender.set_autoPostBack(true);
            //ShowLoading();
        }
    }
    else {
        sender.set_autoPostBack(true);
        //ShowLoading();
    }
}

//function StartLoadFromPatChart(){
//    $("#btnClearAllRefOrder").prop('disabled', false);
//    localStorage.setItem("btnClearAllRefOrder", "false");
//}

function EnableSaveReferralOrder() {    
   //  
    if ($find("btnAddRefOrder")==null)
        $("#btnAddRefOrder").prop('disabled', false);//For menu level order
    else
        $find("btnAddRefOrder").set_enabled(true);//For Tab level order
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;

}
function ImageEnableSaveReferralOrder() {   
    $("#btnAddRefOrder").prop('disabled', false)
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;

}

function ShowLoading() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'block';
}

function EnableAddorder()
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById('btnAddRefOrder') != null)
        document.getElementById('btnAddRefOrder').disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    }
    localStorage.setItem("bSave", "false");
}

function OnClientCloseDiagnosisRef(oWindow, args)
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (document.getElementById('btnAddRefOrder') != null)
        document.getElementById('btnAddRefOrder').disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
    {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    }
    localStorage.setItem("bSave", "false");
    __doPostBack('imgDiagnosis');
}

function EnableSave() {
    $("#btnSave").prop('disabled', false);    
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    }
    window.parent.theForm.children.hdnIsSaveEnableOrders.value = "true";
}

function btnClearAllRefOrder_Clicked(sender, args) {
    var ArguOne = "clear all";
    if (sender==undefined|| sender.get_text() == "Clear All") {
        ArguOne = "clear all";
    } else {
        ArguOne = "cancel";
    }

    if (DisplayErrorMessage("720004", '', ArguOne))
    {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        }
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        document.getElementById('btnClear').click();
        
    } else {
    }
}

function btnFindPhysician_Clicked(sender, args) {

    OpenFindReferralPhysician(sender, args);
}
function DateValidattion(dateToValidate) {

    var datePicker = $find(dateToValidate);
    var splitdate;
    if (datePicker.get_dateInput().get_selectedDate() != null) {
        splitdate = datePicker.get_dateInput().get_selectedDate().format("dd-MMM-yyyy");
    }
    else
    {
        splitdate = datePicker.get_dateInput().get_textBoxValue();
    }
    if (splitdate == "") { return true; }
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
        return true;
    }
    if (parseInt(splitdate.split('-')[0]) > 31) {
        return false;
    }
  
    return true;
}
function onCheckListBoxClick(chkbox) {
    var checked = chkbox.checked;
    if (checked) {
        $("#btnAddRefOrder").prop('disabled', false);
        
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        }
    }
    localStorage.setItem("bSave", "false");
}

function btnPlan_Clicked(sender, args) {
    OpenAddOrUpdatePlan();
}

function btnPrint_Clicked(sender, args) {
    if ($("#btnAddRefOrder") != null && $("#btnAddRefOrder") != undefined && document.getElementById('btnAddRefOrder').attributes["disabled"].value == "disabled")
        //CAP-636 - Handle exception
        if (window?.parent?.parent?.parent?.parent?.theForm?.ctl00_C5POBody_hdnIsSaveEnable?.value != undefined) {
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}

var delIndex = -1;
function grdReferralOrders_OnCommand(sender, args) {

    if (sender == undefined && args == undefined) { 
        if (DisplayErrorMessage("720005")) {
            document.getElementById("hdnRowIndex").value = delIndex;
            delIndex = -1;
            {sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
            //CAP-1436 //CAP-1777-My-Orders screen getting crashed and closed automatically when click on EDIT button
            if (window?.parent?.parent?.parent?.parent?.theForm?.ctl00_C5POBody_hdnIsSaveEnable?.value != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            }
            document.getElementById("btnDelete").click();

        }
    }
    else {
        var CommanArgs = args.get_commandName();
        if (CommanArgs == "Del") {
            delIndex = args.get_commandArgument();
            if (DisplayErrorMessage("720005")) {
                args.set_cancel(false);
            } else {
                args.set_cancel(true);
            }
        }
        else if (CommanArgs == "EditC") { { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();} }
    }
}

function txtProviderName_OnKeyPress(sender, args) {    
    EnableSaveReferralOrder(event);
    //Add JavaScript handler code here
    var reg = /[^-\sa-zA-Z]/;
    if (reg.test(args.get_keyCharacter())) {
        args.set_cancel(true);
    } else {
        args.set_cancel(false);
    }


}

function OnAuthorizationYesClicked() {
    if (document.getElementById('rbtnYes').checked == true) {
        document.getElementById('txtAuthorizationNumber').disabled = false;
        document.getElementById('txtAuthorizationNumber').removeAttribute("backgroundColor");
        document.getElementById('txtAuthorizationNumber').style.backgroundColor = "White";
        document.getElementById('rbtnNo').checked = false;
    }
}

function OnAuthorizationNoClicked() {
    if (document.getElementById('rbtnNo').checked == true) {
        document.getElementById('txtAuthorizationNumber').disabled = true;
        document.getElementById('txtAuthorizationNumber').value = "";
        document.getElementById('txtAuthorizationNumber').style.backgroundColor = "rgb(191, 219, 255)";
        document.getElementById('rbtnYes').checked = false;
    }
}


function CheckBoxListSelect() {
    EnableSaveReferralOrder(event);
    $("#btnAddRefOrder").prop('disabled', false);
    var chkBoxList = document.getElementById('chklstAssessment');
    if (chkBoxList != null) {
        var chkBoxCount = chkBoxList.getElementsByTagName('input');
        if (document.getElementById('ChkSelectAll').checked == true) {
            for (var i = 0; i < chkBoxCount.length; i++) {
                chkBoxCount[i].checked = true;
            }
        } else {
            for (var i = 0; i < chkBoxCount.length; i++) {
                chkBoxCount[i].checked = false;
            }
        }

    }
    return false;
}

function onSplInstructionChecked() {
    var chkBoxList = document.getElementById('chklstAssessment');
    var chkBoxCount = chkBoxList.getElementsByTagName('input');
    if ($('#chklstAssessment input:checkbox[disabled]').length > 0)
    {
        return false;
    }
        var bool = 0;
        for (var i = 0; i < chkBoxCount.length; i++) {
            if (chkBoxCount[i].checked == false) {
                bool++;
                document.getElementById('ChkSelectAll').checked = false;
            }

        }
        if (bool == 0) {
            document.getElementById('ChkSelectAll').checked = true;
        }
        $("#btnAddRefOrder").prop('disabled', false);
        EnableSaveReferralOrder(event);
    
}

function Autosave() {

    document.getElementById('btnAddRefOrder').disabled = true;
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    }
   
    localStorage.setItem("bSave", "true");
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    DisplayErrorMessage('720001');
    Order_AfterAutoSave();
}

function btnMoveToNextProcess_Clicked(sender, args) {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;

    //Jira #CAP-889
    RemoveItem(document.URL, "Orders");
}

function WindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
    //window.top.location.href = "frmMyQueueNew.aspx";// BugID:42368
    
}

function OnLoadReferral() {
    
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined && window.parent.parent.theForm.hdnSaveButtonID != null)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnAddRefOrder,mulPageOrders";
    if (window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != undefined && window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != null)
        window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID.value = "btnAddRefOrder,mulPageOrders";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";

    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}

function btnClose_Clicked(sender, args) {
    var iframe = document.getElementById("iframeOrderPatientBar");

    PrevTab = $($('iframe')[0].contentDocument).find("li.active a");  // previous tab
  if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
      { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
      sessionStorage.setItem("AutoSave_Order", "false");
        if (window.document.title == 'Orders') {
            if (!$($(window.document).find('iframe')[0].contentDocument).find("body").is('#dvdialogMenu'))
                $($(window.document).find('iframe')[0].contentDocument).find("body").append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
            dvdialog = $($(window.document).find('iframe')[0].contentDocument).find("body").find('#dvdialogMenu');
            myPos = "center center";
            atPos = 'center center';
        }
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            position: {
                my: myPos,
                at: atPos

            },
            buttons: {
                "Yes": function () {
                    
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
                    $(dvdialog).dialog("close");
                    sessionStorage.setItem("AutoSave_OrderMenu", "true");
                    if (PrevTab[0].innerText == "Diagnostic Order") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('Order_PrevTabTextMenu', prevTabtxt);
                        $($($($(window.document).children().children()[1]).find('#iframeOrderPatientBar')[0].contentDocument).find("#dvcontainer div.active iframe")[0].contentDocument).find("#btnOrderSubmit")[0].click();
                    }
                    else if (PrevTab[0].innerText == "Referral Order") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('Order_PrevTabTextMenu', prevTabtxt);
                        $($($($(window.document).children().children()[1]).find('#iframeOrderPatientBar')[0].contentDocument).find("#dvcontainer div.active iframe")[0].contentDocument).find("#btnAddRefOrder")[0].click();
                    }
                    else if (PrevTab[0].innerText == "Immunization/Injection") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('Order_PrevTabTextMenu', prevTabtxt);
                        $($($($(window.document).children().children()[1]).find('#iframeOrderPatientBar')[0].contentDocument).find("#dvcontainer div.active iframe")[0].contentDocument).find("#btnAdd")[0].click();

                    }
                    else if (PrevTab[0].innerText == "Procedures") {
                        var prevTabtxt = $(PrevTab).text();
                        sessionStorage.setItem('Order_PrevTabTextMenu', prevTabtxt);
                        $($($($(window.document).children().children()[1]).find('#iframeOrderPatientBar')[0].contentDocument).find("#dvcontainer div.active iframe")[0].contentDocument).find("#btnAdd")[0].click();

                    }
                },
                "No": function () {
                    
                    sessionStorage.setItem("AutoSave_OrderMenu", "false");
                    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                    $(dvdialog).dialog("close");
                    self.close();
                    if (window.parent.parent.location.href.indexOf('Encounter') < 0) {
                        return false;
                    }
                    else {
                        window.parent.parent.location.reload();
                    }
                },
                "Cancel": function () {
                    sessionStorage.setItem("AutoSave_OrderMenu", "false");
                    $(dvdialog).dialog("close");
                    return;

                }
            }
        });
    }
    else {
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
        }
        sessionStorage.setItem("AutoSave_OrderMenu", "false");
        self.close();
        if (window.parent.parent.location.href.indexOf('Encounter') < 0) {
            return false;
        }
        else {
            window.parent.parent.location.reload();
        }
    }

}

function btnCloseyes() {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    window.parent.parent.close();
}

function RefreshParent() {
    window.parent.location.href = window.parent.location.href;
}

//frmOrdersPatientBar
function CreateOrderLoad() {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
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

function OnLoad_CPOE() {
    if (document.getElementById(GetClientId('cboPhysician')).value == "" || document.getElementById(GetClientId('cboPhysician')).textContent == "")
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
//BugID:53431
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnAddRefOrder')[0].disabled = true;
}