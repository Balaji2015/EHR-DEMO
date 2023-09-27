function btnInterpretationClose_Clicked() {
    if (document.getElementById("btnSaveInt").disabled == false) {
        if (!$(document).find('body').is('#dvdialogMenu'))
            $(document).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">Do you want to carry the changes?</p></div>');
        dvdialog = $(document).find('body').find('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';
        $(dvdialog).dialog({
            modal: true,
            title: "Capella -EHR",
            closeOnEscape: false,
            open: function (event, ui) {
                $(".ui-dialog-titlebar-close", ui.dialog || ui).hide();
            },
            position: {
                my: myPos,
                at: atPos
            },
            buttons: {
                "Yes": function () {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    $(dvdialog).dialog("close");
                    document.getElementById("btnSaveInt").click();
                },
                "No": function () {

                    $(dvdialog).dialog("close");
                    $(top.window.document).find("#btnResultInterpretations").click();
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                }
            }
        });
        return false;
    }
    else {
        $(top.window.document).find("#btnResultInterpretations").click();
    }
}

function btnResetClick() {
    if (document.getElementById("ddlTemplate").value.length != '' && document.getElementById("txtSummary").value != '') {
        var ErrorMessage1 = window.confirm("Are you sure you want to Reset?"); //DisplayErrorMessage('1011190');  //window.confirm("Are you sure you want to clear all the fields in Payment information?");
        if (ErrorMessage1 == true) {
            document.getElementById('ddlTemplate').selectedIndex = 0;
            document.getElementById("txtSummary").value = '';
            return true;
        } else {
            return false;
        }
    }
}

$("#txtSummary").keypress(function (e) {
    document.getElementById("btnSaveInt").disabled = false;
    if (e.key == ";")
        return false;
});


function btnOkClick() {
    var summaryvalue = document.getElementById("txtSummary").value;
    var SelectedTemplate = document.getElementById("ddlTemplate").value;
    var Notes = '';
    if (summaryvalue != '')
        Notes = "Test Reviewed: " + SelectedTemplate + ";\n" + summaryvalue;
    $(top.window.document).find("#txtResultInterpretationsInformation")[0].value = JSON.stringify(Notes);
    $(top.window.document).find("#btnResultInterpretations").click();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}



function ddlTemplate_Onchange() {
    if (document.getElementById('ddlTemplate').value == '') {
        document.getElementById("btnSaveInt").disabled = true;
        //document.getElementById("btnPrintInterpretation").disabled = true;
        document.getElementById("btnReset").disabled = true;
    }
    else {
        document.getElementById("btnSaveInt").disabled = false;
        //document.getElementById("btnPrintInterpretation").disabled = false;
        document.getElementById("btnReset").disabled = false;
    }
    return false;
}


function PrintInterpretation() {
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Print Interpretation Notes";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "685px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?&SI=" + document.getElementById("hdnFileName").value + "&Location=DYNAMIC&FaxSubject=''";
    return false;
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

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

//Cap - 686
function DeleteClick() {
    var ErrorMessage = window.confirm("Are you sure you want to delete?");
    if (ErrorMessage == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return true;
    }
    else {
        return false;
    }
}

function DeletedInterpretationNotes() {

    var SelectedTemplate = document.getElementById("ddlTemplate").value;
    var Notes = $(top.window.document).find("#txtDeletedInterpretationsInformation")[0].value;
    if (Notes != '') {
        Notes += "|$|" + "Test Reviewed: " + SelectedTemplate + "$$$" + Notes;
    }
    else {
        Notes = "Test Reviewed: " + SelectedTemplate + "$$$" + Notes;
    }

    $(top.window.document).find("#txtResultInterpretationsInformation")[0].value = "DeleteProviderNotes";
    $(top.window.document).find("#txtDeletedInterpretationsInformation")[0].value = JSON.stringify(Notes);
    document.getElementById("btnSaveInt").disabled = true;
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

}

function LoadResultInterpretation(){
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}











//function btnInterpretationClose_Clicked() {
//    var ErrorMessage1 = window.confirm("Do you want to carry the changes?");


//        if (document.getElementById('btnSave').disabled == false) {
//            //if (document.getElementById("hdnMessageType").value == "") {
//            //    DisplayErrorMessage('1105001');
//            //}
//             if (ErrorMessage1 == "Yes") {
//                document.getElementById('btnSave').disabled = true;
//                document.getElementById("btnSave").click();
//                //self.close();
//            }
//            else if (ErrorMessage1 == "No") {
//                //document.getElementById("hdnMessageType").value = "";
//                //$(top.window.document).find('#btnCloseViewResult')[0].click();
//                self.close();
//            }
//            else if (ErrorMessage1 == "Cancel") {
//                //document.getElementById("hdnMessageType").value = "";
//            }
//        }


//}
