$(document).ready(function () {
    $(top.window.document).find("#btnClosewindow")[0].hidden = true;
    $('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
               '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
});

var iDelIndex = -1;
function gridCellDeleteClick(btn) {
    var vhdndelid = $("[id*='hdndelid']")[0];
    if (btn != undefined) {
        vhdndelid.value = btn.id;
    }
    var check = DisplayErrorMessage('020001');
    if (check==undefined)
    {
        iDelIndex = btn.parentNode.parentNode.rowIndex - 1;
    }
    if (check != undefined && check) {
        

        document.getElementById("hdmRowIndex").value = iDelIndex;
        iDelIndex = -1;
        document.getElementById("btnDelete").click();
        document.getElementById("isGridDelete").value = true;
        return true;
    }
    else if (check != undefined && check == false) {
        document.getElementById("isGridDelete").value = false;
        return false;
    }
    return false;
}

function btnAddEnabled() {
    localStorage.setItem("bSaveAddorUpdate", "false");
    document.getElementById("hdnEnableYesNo").value = "true";

    if (document.getElementById('btnAdd').value != null)
        document.getElementById('btnAdd').disabled = false;
}
function enabledfunction() {
    localStorage.setItem("bSaveAddorUpdate", "false");
    document.getElementById("hdnEnableYesNo").value = "true";

    if (document.getElementById('btnAdd').value != null)
        document.getElementById('btnAdd').disabled = false;
}
function OnFormClose() {
    if ($find('btnAdd') != null)
    {
        var isAddEnabled = $find('btnAdd').get_enabled();
        localStorage.setItem("bSaveAddorUpdate", "false");
        document.getElementById("hdnEnableYesNo").value = "true";
        if (isAddEnabled == true && DisplayErrorMessage('020006') == true) {
        }
    }
    
}

function btnClearAll_ClientClicked(button, args) {
    //CAP-1912
    var btnText = $('#btnClearAll').val();
    if (btnText != undefined) {
        if (btnText == "Clear All") {
            if (DisplayErrorMessage('020007') == true) {
                document.getElementById("isClearAll").value = true;
                document.getElementById('btnClear').click();
                //button.set_autoPostBack(true);
            }
            //else
            //    button.set_autoPostBack(false);
            //Cap - 1913
            else {
                document.getElementById("btnAdd").disabled = false;
            }
        }
        else
            if (btnText == "Cancel") {
                if (DisplayErrorMessage('020008') == true) {
                    document.getElementById("isClearAll").value = true;
                    document.getElementById('btnClear').click();
                    //button.set_autoPostBack(true);
                }
                //else
                //    button.set_autoPostBack(false);
                //Cap - 1913
                else {
                    document.getElementById("btnAdd").disabled = false;
                }
                
            }
    }
    else
    {
        document.getElementById('btnAdd').disabled = false;
        if (DisplayErrorMessage('020007') == true) {
            document.getElementById("isClearAll").value = true;
            document.getElementById('btnClear').click();
        }
    }
}

function btnClearAll_ClientClick() {
    if (DisplayErrorMessage('020007') == true)
        document.getElementById("isClearAll").value = true;
    else
        document.getElementById("isClearAll").value = false;
}
function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName; for (i = 0; i < count; i++)
    { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}
function bSaveAddorUpdate() {
    localStorage.setItem("bSaveAddorUpdate","true");
}
// Modified By Suvarnni for YesNoCancel
function btnClose_Clicked() {
    if (document.getElementById('btnAdd').value != null)

        dvdialog = $('#dvdialogMenu');
    myPos = "center center";
    atPos = 'center center';

    if (document.getElementById('btnAdd').value != null) {
        if (document.getElementById('btnAdd').disabled == true) {
            localStorage.setItem("bSaveAddorUpdate", "true");
        }
        if (document.getElementById('btnAdd').disabled == false) {
            localStorage.setItem("bSaveAddorUpdate", "false");
        }
    }

    if (localStorage.getItem("bSaveAddorUpdate") == "false") {
        event.preventDefault();
        $(dvdialog).dialog({
            modal: true,
            id: "dvModal",
            title: "Capella EHR",
            buttons: {
                "Yes": function () {
                    $(dvdialog).dialog("close");
                    $("[id*='hdnMessageType']")[0].value = "Yes"
                    localStorage.setItem("bSaveAddorUpdate", "true");
                    if (document.getElementById('btnAdd').value != null) {
                        //Cap - 1913
                       // __doPostBack('btnAdd', true);
                        document.getElementById('btnAdd').click();
                    }
                },
                "No": function () {
                    localStorage.setItem("bSaveAddorUpdate", "true");
                    $("[id*='hdnMessageType']")[0].value = ""
                    $(dvdialog).dialog("close");
                    UpdateNotesInHistory();
                    $(top.window.document).find("#btnClosewindow")[0].click();
                },
                "Cancel": function () {
                    $("[id*='hdnMessageType']")[0].value = ""
                    $(dvdialog).dialog("close");
                    return;
                }
            }
        });
    }
    else {
        UpdateNotesInHistory();
        $(top.window.document).find("#btnClosewindow")[0].click();
    }
    var len = window.parent.parent.parent.parent.document.getElementsByClassName('ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable').length;
    if (len > 1) {
        window.parent.parent.parent.parent.document.getElementsByClassName('ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable')[0].remove();
    }
    if (window.parent.parent.parent.parent.document.getElementsByClassName('ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable').length > 0) {
        window.parent.parent.parent.parent.document.getElementsByClassName('ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable')[0].style.top = '475px';
        window.parent.parent.parent.parent.document.getElementsByClassName('ui-dialog ui-widget ui-widget-content ui-corner-all ui-front ui-dialog-buttons ui-draggable ui-resizable')[0].style.left = '570px';

    }
}

function YesNoUpdateSaveAddorUpdate() {
    UpdateNotesInHistory();
    $(top.window.document).find("#btnClosewindow")[0].click();
    self.close();
}

function UpdateNotesInHistory() {
    if ($('#lblKeyword').text() == 'Surgery Name' || $('#lblKeyword').text() == 'Reason For Hospitalization') {
        var cells = $($('#grdAddOrUpdateKeyword').find('td.displayNone').prev());
        var lstArray = [];
        for (var i = 0; i < cells.length; i++)
            lstArray.push($(cells[i]).text());
        sessionStorage.setItem("Updated_Surgery_List", JSON.stringify(lstArray.sort()));
    }
}

function btnAdd_ClientClicked(button, args)
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
