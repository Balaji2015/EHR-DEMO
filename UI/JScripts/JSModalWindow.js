function openModal(formname, height, width, inputargument, RadWindowName) {
    try{
        var Argument = ""; var PageName = formname; if (inputargument != undefined) {
            for (var i = 0; i < inputargument.length; i++)
            { if (i != 0) { Argument = Argument + "&" + inputargument[i]; } else { Argument = inputargument[i]; } }
            if (inputargument.length != 0) { PageName = PageName + "?"; }
        }
        if (formname.indexOf("frmFindPatient.aspx") > -1) {
            height = 251;
            width = 1200;
        }
        var result = radopen(PageName + Argument, RadWindowName); result.SetModal(true); result.set_visibleStatusbar(false); result.setSize(width, height);
        result.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close); result.set_iconUrl("Resources/16_16.ico"); result.set_keepInScreenBounds(true);
        result.set_centerIfModal(true); result.add_close(clearURL); result.center();
        removeAllCloseHandlers($find(RadWindowName));
    }
    catch(exception){
    }
}
function openModalPerformEV(formname, height, width, inputargument, RadWindowName) {
    try {
        var Argument = ""; var PageName = formname; if (inputargument != undefined) {
            for (var i = 0; i < inputargument.length; i++)
            { if (i != 0) { Argument = Argument + "&" + inputargument[i]; } else { Argument = inputargument[i]; } }
            if (inputargument.length != 0) { PageName = PageName + "?"; }
        }
        if (formname.indexOf("frmFindPatient.aspx") > -1) {
            height = 250;
            width = 1200;
        }
        var result = radopen(PageName + Argument, RadWindowName); result.SetModal(true); result.set_visibleStatusbar(false); result.setSize(width, height);
        result.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move); result.set_iconUrl("Resources/16_16.ico"); result.set_keepInScreenBounds(true);
        result.remove_close();
        result.set_centerIfModal(true);
        result.center();
    }
    catch (exception) {
    }
}
function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName;
    for (i = 0; i < count; i++) { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}
function openNonModal(fromname, height, width, inputargument) {
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
    var result = window.open(PageName + Argument, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes");
    if (result == undefined) { result = window.returnValue; }
    return result;
}
function openModalProgress(fromname, height, width, inputargument, RadWindowName) {
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
        if (inputargument.lenght != 0) {
            PageName = PageName + "?";
        }
    }
    var result = radopen(PageName + Argument, RadWindowName);
    result.set_visibleStatusbar(false);
    result.setSize(width, height);
    result.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move + Telerik.Web.UI.WindowBehaviors.Close);
    result.set_iconUrl("Resources/16_16.ico");
    result.set_keepInScreenBounds(false);
    result.set_centerIfModal(false);
    result.center();

}
function clearURL(oWindow, args) { oWindow.setUrl("about:blank"); }
function removeAllCloseHandlers(oWindow) {
    
    if (oWindow != null)
    {
        var attachedCloseHandlers = oWindow.get_events()._getEvent("close");
        for (var i = 0; i < attachedCloseHandlers.length; i++) {
            if (attachedCloseHandlers[i].name != "" && attachedCloseHandlers[i].name != "clearURL")
                oWindow.remove_close(attachedCloseHandlers[0]);
        }
    }
   
}

function Blockdays()
{
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("span[mand=Yes]").addClass('MandLabelstyle');

    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
