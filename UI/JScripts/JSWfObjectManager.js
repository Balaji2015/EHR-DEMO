function GetObjects() {
    document.getElementById("divLoading").style.display = "block";
}
function cboObjectTypeUpdateProcess() {
    var SelectItem = document.getElementById("cboFirstObjType");
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}

function cboObjectTypeUpdateOwner() {
    var SelectItem = document.getElementById("cboFirstObjType");

    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}
function WindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}
function Clear() {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        return true;
    }
    return false;
}

function cboUpdateCurrentProcess() {
    var SelectItem = document.getElementById("cboFirstCurrentProcess");
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}

function cboOwnerCurrentProcess() {
    var SelectItem = document.getElementById("cboCurrentProcess");
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}


function Checking(args) {

    var rdprocess = document.getElementById("rbUpdateProcess");
    var rdOwner = document.getElementById("rbUpdateOwner");
    if (rdprocess.checked) {
        var controls = document.getElementById("<%=pnlUpdateProcess.ClientID%>").getElementsByTagName("input");
        for (var i = 0; i < controls.length; i++)
            controls[i].disabled = false;
        var controls1 = document.getElementById("<%=pnlUpdateOwner.ClientID%>").getElementsByTagName("input");
        for (var i = 0; i < controls1.length; i++)
            controls1[i].disabled = true;
    }
}
 
function CheckingOwner(args) {

    var rdprocess = document.getElementById("rbUpdateProcess");
    var rdOwner = document.getElementById("rbUpdateOwner");
    if (rdOwner.checked) {
        var controls = document.getElementById("<%=pnlUpdateOwner.ClientID%>").getElementsByTagName("input");
        for (var i = 0; i < controls.length; i++)
            controls[i].disabled = false;
        var controls1 = document.getElementById("<%=pnlUpdateProcess.ClientID%>").getElementsByTagName("input");

        for (var i = 0; i < controls1.length; i++)
            controls1[i].disabled = true;
    }
}


function btnClose_Clicked()
{
    if (document.getElementById("btnUpdateProcess").disabled == false) {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        if (!$($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').is('#dvdialogMenu'))
            $($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
        dvdialog = $($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').find('#dvdialogMenu');
            myPos = "center center";
            atPos = 'center center';
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
                    if (document.getElementById("txtReasonForChange").value=="")
                    {
                        DisplayErrorMessage('700003');
                         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                        return false;
                    }
                    else {
                        document.getElementById("hdnMessageType").value = "Yes";
                        $('#btnUpdateProcess').trigger('click');
                    }
                },
                "No": function () {
                    
                    $(dvdialog).dialog("close");
                    self.close();
                },
                "Cancel": function () {
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
        self.close();
    }

   
}
function loadwfobject() {
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}
function btnGetobjectsClick()
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}

function btnUpdateOwnerClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function btnUpdateProcessClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    return true;
}

function SaveSuccess() {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    self.close();
}
function NotSaved() {
    document.getElementById("btnUpdateProcess").disabled == false;
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
function grdAdminModule_OnCommand(sender, args)
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    return true;
}

function rbUpdateProcessOnClick()
{
    if (document.getElementById("txtReasonForChange").disabled == true)
    {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    }
}
function rbUpdateOwnerOnClick()
{
    if (document.getElementById("btnUpdateOwner").disabled == true)
    {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    }
}
 //Jira #CAP-195
function chkShowAllOwner() {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}