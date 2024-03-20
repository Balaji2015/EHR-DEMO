$(document).ready(function () {
    
    $(top.window.document).find("#btnClosed")[0].hidden = true;
    document.getElementById("hdnDeletedCPT").value = "";
});


function btnBack_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var list = document.getElementById("lstLabProcudurePhysician").getElementsByTagName('input')
    var isChecked = false;
    var CheckedList = new Array();
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            isChecked = true;
            break;
        }
    }

    if (isChecked) {
        var DeletedICD = "";
        var bConfirmation = DisplayErrorMessage('230007');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        if (bConfirmation == undefined) {
            return;
        }
        if (bConfirmation == true) {
            for (i = 0; i < list.length; i++) {
                if (list[i].checked) {
                    if (DeletedICD != "") {
                        DeletedICD = DeletedICD + "|" + list[i].parentElement.parentElement.innerText;
                    }
                    else {
                        DeletedICD = list[i].parentElement.parentElement.innerText;
                    }
                    var trElement = list[i].parentElement.parentElement;
                    var tblElement = trElement.parentElement;
                    tblElement.removeChild(trElement);
                    i--;
                }
            }
            if (document.getElementById("hdnDeletedCPT").value != "") {
                document.getElementById("hdnDeletedCPT").value += "|" + DeletedICD;
            }
            else {
                document.getElementById("hdnDeletedCPT").value = DeletedICD;

            }
            $find('btnSave').set_enabled(true);
            document.getElementById('btnInvisibleBack').click();
        }
        else {
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
    }
    else {
        DisplayErrorMessage('9093037')
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
}

function btnSearch_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var txtDesc = $find("txtEnterDescription");
    var txtCode = $find("txtEnterCPTCode");
    if (txtDesc.get_textBoxValue() == "" && txtCode.get_textBoxValue() == "") {
        DisplayErrorMessage('230004');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        sender.set_autoPostBack(false);
    }
    else {

        sender.set_autoPostBack(true);
    }
}

function EnableSaveButton() {
    $find('btnSave').set_enabled(true);
}

function GetCPTValue() {
    var chkListModules = document.getElementById('<%= lstLabProcudurePhysician.ClientID %>');
    if (chkListModules == null) {
        return false;
    }
    var chkListinputs = chkListModules.getElementsByTagName("input");
    var Result = new Object();
    for (var i = 0; i < chkListinputs.length; i++) {
        var options = document.getElementById('<%=lstLabProcudurePhysician.ClientID%>');
        for (i = 0; i < options.cells.length; i++) {
            if (Result.SelectedICD == null) {
                Result.SelectedICD = options.cells[i].innerText;
            }
            else {
                Result.SelectedICD += "|" + options.cells[i].innerText;
            }
        }
        return true;
    }
}

function oncloseclick() {
    DisplayErrorMessage('220106');
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    var Result = new Object();
    if (document.getElementById('hdnRefresh').value != null && document.getElementById('hdnRefresh').value != undefined) {
        if (document.getElementById('hdnRefresh').value == "true") {
            var win = GetRadWindow();
            if (win != null) {
                win.set_behaviors(Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                win.close();
            }
            $(top.window.document).find('#btnClosed')[0].click();
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            self.close();
        }
    }
    else {
        Result.hdnSelectedCPT = document.getElementById("hdnSelectedCPT").value;
        Result.IsDirty = document.getElementById("hdnIsDirty").value;
        window.returnValue = Result;
        returnToParent(Result);
        return true;
    }

}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
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

function EnableSave() {
    document.getElementById('btnSave').disabled = false;
    DisplayErrorMessage('220106');
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    }
    else {
        window.parent.theForm.hdnSaveEnable.value = true;
    }
    if (document.getElementById('hdnRefresh').value != null && document.getElementById('hdnRefresh').value != undefined) {
        if (document.getElementById('hdnRefresh').value == "true") {
            var win = GetRadWindow();
            if (win != null) {
                win.set_behaviors(Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                win.close();
            }
            $(top.window.document).find('#btnClosed')[0].click();
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            self.close();
        }
    }
}

function CheckIfOTHERTEST() {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        document.getElementById('hdnMoveClick').value = "1,true";
        document.getElementById('btnMove').click();
}

function txtEnterCPTCode_OnKeyPress(sender, args) {
    if ((args._keyCode > 47 && args._keyCode < 58) || (args._keyCode > 64 && args._keyCode < 91) || (args._keyCode > 96 && args._keyCode < 123)) {
        args._cancel = false;
    }
    else {
        args._cancel = true;
    }
    if (args._keyCode == 13) {
        document.getElementById('btnSearch').click();
    }
}

function closeWindow(sender, args) {
    var win = GetRadWindow();
    var btnSave = $find('btnSave');
    if (btnSave._enabled == true) {
        if (document.getElementById("hdnMessageType").value == "") {
            //CAP-1770
            document.getElementById("hdnMessageType").value = "";
            document.getElementById('hdnRefresh').value = "true";

            $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'> <p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
            dvdialog = $('#dvdialogMenu');
            myPos = "center center";
            atPos = 'center center';

            $(dvdialog).dialog({
                modal: true,
                title: "Capella EHR",
                position: {
                    my: myPos,
                    at: atPos
                },
                buttons: {
                    "Yes": function () {
                        $find('btnSave').set_enabled(true);
                        $('#btnSave').trigger('click');
                        $(dvdialog).dialog("close");
                        $(dvdialog).remove();
                        return false;
                    },
                    "No": function () {
                        $find('btnSave').set_enabled(true);
                        $(dvdialog).dialog("close");
                        $(dvdialog).remove();
                        $(top.window.document).find('#btnClosed')[0].click();
                        return false;
                    },
                    "Cancel": function () {
                        $find('btnSave').set_enabled(true);
                        $(dvdialog).dialog("close");
                        $(dvdialog).remove();
                        return false;
                    }
                }
            });
            //args.set_cancel(true);
            //DisplayErrorMessage('220108');
            // {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            //sender.set_autoPostBack(false);
        }
        //else if (document.getElementById("hdnMessageType").value == "Yes") {
        //    __doPostBack('btnSave', "true");
        //    $find('btnSave').set_enabled(false);
        //    document.getElementById("hdnMessageType").value = "";
        //    document.getElementById('hdnRefresh').value = "true";
        //     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //    return false;
        //}
        //else if (document.getElementById("hdnMessageType").value == "No") {
        //    document.getElementById("hdnMessageType").value = ""
        //    self.close();
        //    $(top.window.document).find('#btnClosed')[0].click();
        //     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //}
        //else if (document.getElementById("hdnMessageType").value == "Cancel") {
        //    document.getElementById("hdnMessageType").value = "";
        //    $find('btnSave').set_enabled(true);
        //    $find('btnQuitFrequentlyDiagnosis').set_autoPostBack(false);
        //     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //    return false;
        //}
    }
    else {
        if (win != null) {
            win.set_behaviors(Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
            win.close();
        }
        $(top.window.document).find('#btnClosed')[0].click();
    }

}

function btnClearAll_Click(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    document.getElementById("txtEnterDescription").value = "";
    document.getElementById("txtEnterCPTCode").value = "";
    if (document.getElementById("lblResult") != null) {
        document.getElementById("lblResult").innerHTML = "";
        document.getElementById("lblResult").Visible = false;
    }
    if (document.getElementById("lstLabProcedureAll") != null) {
        document.getElementById("lstLabProcedureAll").innerHTML = "";
    }
    document.getElementById("txtEnterDescription").focus();
    sender.set_autoPostBack(true);
}
function btnSave_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function OnLabProcedureLoad() {
    
    if (window.innerHeight >= 620) {
        document.getElementById("gbFrequentlyUsedProcedures").style.height = "545px";
        document.getElementById("gbProcedureCodeLibrary").style.height = "400px";
        document.cookie = "FrequProcHgt=545";
        document.cookie = "ProcedureCodeHgt=400";
    }
    else if (window.innerHeight >= 530 && window.innerHeight <= 619) {
        document.getElementById("gbFrequentlyUsedProcedures").style.height = "445px";
        document.getElementById("gbProcedureCodeLibrary").style.height = "300px";
        document.cookie = "FrequProcHgt=445";
        document.cookie = "ProcedureCodeHgt=300";
    }
    else {
        document.getElementById("gbFrequentlyUsedProcedures").style.height = "345px";
        document.getElementById("gbProcedureCodeLibrary").style.height = "200px";
        document.cookie = "FrequProcHgt=345";
        document.cookie = "ProcedureCodeHgt=200";
    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}