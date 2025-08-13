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

//Cap - 1256
//$("#txtSummary").keypress(function (e) {
//    document.getElementById("btnSaveInt").disabled = false;
//    if (e.key == ";")
//        return false;
//});

function transformTypedChar(charStr) {
    return charStr == ";" ? "," : charStr;
}

document.getElementById("txtSummary").onkeypress = function (evt) {
    document.getElementById("btnSaveInt").disabled = false;
    var val = this.value;
    evt = evt || window.event;

    // Ensure we only handle printable keys, excluding enter and space
    var charCode = typeof evt.which == "number" ? evt.which : evt.keyCode;
    if (charCode && charCode > 32) {
        var keyChar = String.fromCharCode(charCode);

        // Transform typed character
        var mappedChar = transformTypedChar(keyChar);

        var start, end;
        if (typeof this.selectionStart == "number" && typeof this.selectionEnd == "number") {
            // Non-IE browsers and IE 9
            start = this.selectionStart;
            end = this.selectionEnd;
            this.value = val.slice(0, start) + mappedChar + val.slice(end);

            // Move the caret
            this.selectionStart = this.selectionEnd = start + 1;
        } else if (document.selection && document.selection.createRange) {
            // For IE up to version 8
            var selectionRange = document.selection.createRange();
            var textInputRange = this.createTextRange();
            var precedingRange = this.createTextRange();
            var bookmark = selectionRange.getBookmark();
            textInputRange.moveToBookmark(bookmark);
            precedingRange.setEndPoint("EndToStart", textInputRange);
            start = precedingRange.text.length;
            end = start + selectionRange.text.length;

            this.value = val.slice(0, start) + mappedChar + val.slice(end);
            start++;

            // Move the caret
            textInputRange = this.createTextRange();
            textInputRange.collapse(true);
            textInputRange.move("character", start - (this.value.slice(0, start).split("\r\n").length - 1));
            textInputRange.select();
        }

        return false;
    }
};

$("#txtSummary").bind('paste', function () {
    setTimeout(function () {
        var data = $('#txtSummary').val();
        var dataFull = data.replaceAll(";", ",");
        $('#txtSummary').val(dataFull);
    }), '3000';
    //Cap - 1636
    document.getElementById("btnSaveInt").disabled = false;
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
    //Cap - 3385
    var IsFilled = localStorage.getItem("IsInterpretationFilled");

    if (IsFilled == "Y") {
        DisplayErrorMessage('1105010');
        return false;
    }         

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
//Cap - 3385
function ddlTemplate_Alert() {
    var IsFilled = localStorage.getItem("IsInterpretationFilled");

    if (IsFilled == "Y") {
        DisplayErrorMessage('1105010');
        return false;
    }
    else {
        return true;
    }
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
        //Cap - 3385
        localStorage.setItem("IsInterpretationFilled", "N");
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
