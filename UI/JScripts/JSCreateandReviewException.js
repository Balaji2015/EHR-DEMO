
function CloseExceptionmovetab() {
    $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[10].click();

    $(top.window.document).find('#btnExceptionId')[0].click();

}
function btnMovetoProvider(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if ($find("btnAdd").get_enabled()) {
        if (document.getElementById("hdnMessageType").value == "") {
            document.getElementById(GetClientId("hdnsuccess")).value = "btnMovetoProvider";
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            DisplayErrorMessage('450014');
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            ShowLoading();
            if (document.getElementById("hdnSourceScreen").value == "Create Coding Exception") {
                if (document.getElementById('txtIssues').value == "") {
                    DisplayErrorMessage('450005');
                    document.getElementById('divLoading').style.display = "none";
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    return false;
                }
                if (document.getElementById('txtIssues').value != "") {
                    __doPostBack('btnAdd', "true");
                    $find('btnAdd').set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    DisplayErrorMessage('450002');
                    $(top.window.document).find('#btnExceptionId')[0].click();
                }
            }
            else if (document.getElementById("hdnSourceScreen").value == "Feedback for Coding Exception") {
                if (document.getElementById('txtIssues').value == "") {
                    DisplayErrorMessage('440007');
                    document.getElementById('DLCfeedback_txtDLC').value = ""
                    $find("btnAdd").set_enabled(true);
                    document.getElementById('divLoading').style.display = "none";
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    return false;
                }
                if (document.getElementById('DLCfeedback_txtDLC').value == "") {
                    DisplayErrorMessage('440006');
                    $find("btnAdd").set_enabled(true);
                    document.getElementById('divLoading').style.display = "none";
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    return false;
                }
                if (document.getElementById('txtIssues').value != "" && document.getElementById('DLCfeedback_txtDLC').value != "") {
                    __doPostBack('btnAdd', "true");
                    $find('btnAdd').set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    DisplayErrorMessage('450002');
                    
                    $(top.window.document).find('#btnExceptionId')[0].click();
                }
            }
        }
        else if (document.getElementById("hdnMessageType").value == "No") {
            document.getElementById("hdnMessageType").value = ""
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            $(top.window.document).find('#btnExceptionId')[0].click();
        }
        else if (document.getElementById("hdnMessageType").value == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
            $find("btnAdd").set_enabled(true);
            if (document.getElementById('txtIssues').value != "") {
                $find('btnAdd').set_enabled(true);
            }
            else {
                $find('btnAdd').set_enabled(false);
            }
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
        if (document.getElementById("hdnMessageType").value != "Yes") {
            sender.set_autoPostBack(false);
        }
    }
    else {
        sender.set_autoPostBack(true);
    }
}
function btnClose_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if ($find("btnAdd").get_enabled()) {
        if (document.getElementById("hdnMessageType").value == "") {
            DisplayErrorMessage('450014');
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            ShowLoading();
            if (document.getElementById("hdnSourceScreen").value == "Create Coding Exception") {
                if (document.getElementById('txtIssues').value == "") {
                    DisplayErrorMessage('450005');
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    return false;
                }
                if (document.getElementById('txtIssues').value != "") {
                    $find('btnAdd').set_enabled(true);
                    $find('btnAdd').click();
                    return true;
                }
            }
            else if (document.getElementById("hdnSourceScreen").value == "Feedback for Coding Exception") {
                if (document.getElementById('txtIssues').value == "") {
                    DisplayErrorMessage('440007');
                    document.getElementById('DLCfeedback_txtDLC').value = ""
                    $find("btnAdd").set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    return false;
                }
                if (document.getElementById('DLCfeedback_txtDLC').value == "") {
                    DisplayErrorMessage('440006');
                    $find("btnAdd").set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    return false;
                }
                if (document.getElementById('txtIssues').value != "" && document.getElementById('DLCfeedback_txtDLC').value != "") {
                    $find('btnAdd').set_enabled(true);
                    $find('btnAdd').click();
                    return true;
                }
            }
        }
        else if (document.getElementById("hdnMessageType").value == "No") {
            document.getElementById("hdnMessageType").value = "";
            document.getElementById(GetClientId("hdnsuccess")).value = "";
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
            $(top.window.document).find('#btnExceptionId')[0].click();
        }
        else if (document.getElementById("hdnMessageType").value == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
            document.getElementById(GetClientId("hdnsuccess")).value = "";
            $find("btnAdd").set_enabled(true);
            if (document.getElementById('txtIssues').value != "") {
                $find('btnAdd').set_enabled(true);
            }
            else {
                $find('btnAdd').set_enabled(false);
            }
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
    }
    else {
        $(top.window.document).find('#btnExceptionId')[0].click();
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
}
function Addsuccess() {
    DisplayErrorMessage('450002');
    self.close();
}
function CellSelected(value) {
    if (DisplayErrorMessage('450003') == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        document.getElementById('hdnDelExceptionId').value = value;
        document.getElementById('InvisibleButton').click();
    }

}
function CloseException() {
    self.parent.parent.parent.location.href = "frmMyQueueNew.aspx";
    self.close();
}
function Clearall() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if ($find('btnClear')._text == "Clear") {
        if (DisplayErrorMessage('450012') == true) {
            var txtboxes = document.getElementsByTagName('textarea');
            if (location.search.indexOf('Feedback') == -1) {
                if (txtboxes[0] != undefined) {
                    txtboxes[0].value = "";
                    $find('btnAdd').set_enabled(false);
                }
            }

            if (txtboxes[1] != undefined) {
                txtboxes[1].value = "";
                $find('btnAdd').set_enabled(false);
            }
            $find('btnAdd').set_enabled(false);
        }
    }
    else if ($find('btnClear')._text == "Cancel") {
        if (DisplayErrorMessage('440009') == true) {
            var txtboxes = document.getElementsByTagName('textarea');
            if (txtboxes[0] != undefined) {
                txtboxes[0].value = "";
                $find('btnClear').set_text("Clear");
                $find('btnAdd').set_text("Add");
                $find('btnAdd').set_enabled(false);
            }

            if (txtboxes[1] != undefined) {
                txtboxes[1].value = "";
                $find('btnClear').set_text("Clear");
                $find('btnAdd').set_text("Add");
                $find('btnAdd').set_enabled(false);
            }

        }
    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
function ShowLoading() {
   
    var dt = new Date();
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
}
function EnableSave(event) {
    $find('btnAdd').set_enabled(true);
}
function btnAdd_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (document.getElementById('txtIssues').value.trim() == '') {
        DisplayErrorMessage('440007');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        sender.set_autoPostBack(false);
    }
    else {
        ShowLoading();
        sender.set_autoPostBack(true);
    }

}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    if (oWindow == null) {
        oWindow = $find(ModalWndw);
    }
    return oWindow;
}
function EnableSavetxtFeedback(event) {
    if (document.getElementById('DLCfeedback_txtDLC').value != "") {
        $find('btnAdd').set_enabled(true);
    }
    else {
        $find('btnAdd').set_enabled(false);
    }

}

function EnableSavetxtIssues(event) {
    if (document.getElementById('txtIssues').value != "") {
        $find('btnAdd')?.set_enabled(true);
    }
    else {
        //CAP-1463 
        $find('btnAdd')?.set_enabled(false);
    }
}
function btnCloseClicked(sender, args) {
    if ($find("btnAdd").get_enabled()) {
        if (document.getElementById("hdnMessageType").value == "") {
            DisplayErrorMessage('450014');
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            ShowLoading();
            if (document.getElementById("hdnSourceScreen").value == "Create Coding Exception") {
                if (document.getElementById('txtIssues').value == "") {
                    DisplayErrorMessage('450005');
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    return false;
                }
                if (document.getElementById('txtIssues').value != "") {
                    __doPostBack('btnAdd', "true");
                    $find('btnAdd').set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    DisplayErrorMessage('450002');
                }
            }
            else if (document.getElementById("hdnSourceScreen").value == "Feedback for Coding Exception") {
                if (document.getElementById('txtIssues').value == "") {
                    DisplayErrorMessage('440007');
                    document.getElementById('DLCfeedback_txtDLC').value = ""
                    $find("btnAdd").set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                    return false;
                }
                if (document.getElementById('DLCfeedback_txtDLC').value == "") {
                    DisplayErrorMessage('440006');
                    $find("btnAdd").set_enabled(true);
                    document.getElementById('divLoading').style.display = "none";
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    return false;
                }
                if (document.getElementById('txtIssues').value != "" && document.getElementById('DLCfeedback_txtDLC').value != "") {
                    __doPostBack('btnAdd', "true");
                    $find('btnAdd').set_enabled(true);
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    DisplayErrorMessage('450002');
                }
            }
        }
        else if (document.getElementById("hdnMessageType").value == "No") {
            document.getElementById("hdnMessageType").value = "";
            document.getElementById(GetClientId("hdnsuccess")).value = "";
           
            $(top.window.document).find('#btnExceptionId')[0].click();
        }
        else if (document.getElementById("hdnMessageType").value == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
            document.getElementById(GetClientId("hdnsuccess")).value = "";
            $find("btnAdd").set_enabled(true);
            if (document.getElementById('txtIssues').value != "") {
                r
                $find('btnAdd').set_enabled(true);
            }
            else {
                $find('btnAdd').set_enabled(false);
            }
        }
    }
    else {

        $(top.window.document).find('#btnExceptionId')[0].click();
    }
}
function LoadException() {
    
        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        $("span[mand=Yes]").addClass('MandLabelstyle');

        $("span[mand=Yes]").each(function () {
            $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
        });
   
}