function changeReload() {   
    //CAP-1911
    //window.parent.location.href = window.parent.location.href;
    window.parent.parent.location.reload();
}


function CheckUserFields() {

    if ($("#cboFacilityName :selected").val() == undefined || $("#cboFacilityName :selected").val() == null || $("#cboFacilityName :selected").val() == "")
    {
        DisplayErrorMessage("010005");
        return false;
    }
    return true;
}

function RadWindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null) {
        oWindow.close();
        //location.href = location.href;
    }       
    return false;
}

$(document).ready(function () {
    document.getElementById('cboFacilityName').focus();
});

function CloseWindow() {
    self.close();
}