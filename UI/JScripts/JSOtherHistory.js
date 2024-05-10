function btnPhysicianClearAll_Clicked(sender, args) {
    if (sender != undefined)
        sender.set_autoPostBack(false);
    var IsClearAll = null;
    if (document.getElementById('hdnAddorUpdate').value.trim().toUpperCase() == "ADD") {
        IsClearAll = DisplayErrorMessage('180010');
    } else {
        IsClearAll = true;//DisplayErrorMessage('180035');
    }
    if (IsClearAll == true)
    {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        else
            window.parent.theForm.hdnAddEnable.value = false;

        document.getElementById('btnAdd').set_enabled = false;
        document.getElementById('txtProviderName').value = "";
        document.getElementById('txtSpecialty').value = "";
        document.getElementById('msktxtTelephone').value = "";
        document.getElementById('hdnAddEnable').value = "false";
        document.getElementById('InvisibleButton').click();
        return true;
    }
    else
    {
        $find('btnAdd').set_enabled(true);
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnAddEnable.value = true;
        localStorage.setItem("bSave", "false");
        return false;
    }
}


function btnClearAll_Clicked(sender, args) {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        else
            window.parent.theForm.hdnSaveEnable.value = false;

        document.getElementById('btnClearAllAdvancedDirectivedHidden').click();
        return true;
    } else {
        $find('btnSave').set_enabled(true);
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
        localStorage.setItem("bSave", "false");
        return false;
    }
}

function cboAdvancedDirectived_SelectedIndexChanged(sender, args) {
    $find('btnSave').set_enabled(true);
    document.getElementById("hdnSaveEnable").value = "true";
    localStorage.setItem("bSave", "false");    
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function EnableSave(e) {
    $find("btnSave").set_enabled(true);
    document.getElementById("hdnSaveEnable").value = "true";
    localStorage.setItem("bSave", "false");   
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function EnablePFSH(val) {
    if ($(window.parent.document).find('#btnPFSHVerified') != null)
        $(window.parent.document).find('#btnPFSHVerified')[0].disabled = false;
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    var bValue = true;
    var PFSHVerified = localStorage.getItem("PFSHVerified");
    if (PFSHVerified != "") {
        var PFSH = PFSHVerified.split('|');
        for (var i = 0; i < PFSH.length; i++) {
            if (PFSH[i].split('-')[0] == val) {
                PFSHVerified = PFSHVerified.replace(PFSH[i], val + "-" + "TRUE");
                bValue = false;
            }
        }
    }
    if (bValue == true)
        PFSHVerified = PFSHVerified + "|" + val + "-" + "TRUE";
    localStorage.setItem("PFSHVerified", PFSHVerified);
}

function CCTextChanged() {
    if($find("btnSave")!=null)
        $find("btnSave").set_enabled(true);
    document.getElementById("hdnSaveEnable").value = "true";
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;    
    return false;
}

function btnAdd_Clicked(sender, args) {
    if ($find('btnAdd').get_enabled() == true) {
        if (document.getElementById("txtProviderName").value.trim() == "") {
            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('845002');
            $find('btnAdd').set_autoPostBack(false);
        }
        else {
            $find('btnAdd').set_autoPostBack(true);
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        }
    }
}

function btnSave_Clicked(sender, args) {
    if ($find('btnSave').get_enabled() == true) {
        if (document.getElementById("DLC_txtDLC").value == "" && $find("cboAdvancedDirectived").get_selectedItem()._text == "") {
            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('845006');
            $find("btnSave").set_autoPostBack(false);
        }
        else {
            $find("btnSave").set_autoPostBack(true);
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
          
        }               
    }
}
function loadotherHistory() {
    
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');

}
function OnClientClose(oWnd, args) {    
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    var arg = args.get_argument();
    if (arg != null) {
        if (arg.sPhyName != undefined)
            $find("txtProviderName").set_value(arg.sPhyName);;
        if (arg.sPhySpecialty != undefined) {
            if (arg.sPhySpecialty.indexOf("&amp;") != -1)
                $find("txtSpecialty").set_value(arg.sPhySpecialty.replace("&amp;", ""));
            else
                $find("txtSpecialty").set_value(arg.sPhySpecialty);
        }
        if (arg.sPhyPhone != undefined)
            $find("msktxtTelephone").set_value(arg.sPhyPhone);
        $find('btnAdd').set_enabled(true);
        document.getElementById('hdnAddEnable').value = "true";
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
    }
   
}

function grdProviderDeatils_OnCellSelected(sender, args) {
    $find('btnAdd').set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    document.getElementById('hdnAddEnable').value = "true";
}

function txtProviderName_OnKeyPress(sender, args) {
    $find('btnAdd').set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false"); 
    document.getElementById('hdnAddEnable').value = "true";
    //var c = args.get_keyCode();
    //if ((c < 37) || (c > 40 && c < 45) || (c > 47 && c < 65) || (c > 90 && c < 97) || (c > 122) || (c == 32))
    //    args.set_cancel(true);
}

function txtProviderName_OnValueChanged(sender, args) {
    $find('btnAdd').set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    document.getElementById('hdnAddEnable').value = "true";
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;   
}

function txtSpecialty_OnKeyPress(sender, args) {
    $find('btnAdd').set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    document.getElementById('hdnAddEnable').value = "true";
}

function msktxtTelephone_OnKeyPress(sender, args) {
    $find('btnAdd').set_enabled(true);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    document.getElementById('hdnAddEnable').value = "true";
}

function SavedSuccessfully() {
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
    DisplayErrorMessage('180601');
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
function OpenReferralPhysician(sender,args) {
    var result = openModal("frmFindReferralPhysician.aspx", 256, 930, null, "MessageWindowAD");
    var WindowName = $find('MessageWindowAD');
    WindowName.add_close(OnClientClose);    
}
function PFSHAutoSave() {
    if ($find('btnSave').get_enabled() == true) {
        if (document.getElementById("DLC_txtDLC").value == "" && $find("cboAdvancedDirectived").get_selectedItem()._text == "") {
            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('845006');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else {
            $find("btnSave").click();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        }
    }
    if ($find('btnAdd').get_enabled() == true) {
        if (document.getElementById("txtProviderName").value.trim() == "") {
            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
            PFSH_SaveUnsuccessful();
            DisplayErrorMessage('845002');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else {
            $find('btnAdd').click();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        }
    }
}

function OpenADFiles()
{
    $(top.window.document).find("#PrintPDFModalAD").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitleAD")[0].textContent = "View AD Documents";
    $(top.window.document).find("#PrintPDFmdldlgAD")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlgAD")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrameAD")[0].style.height = "650px";
    $(top.window.document).find("#PrintPDFFrameAD")[0].contentDocument.location.href = "frmADPrintPdf.aspx?";
}

function OpenADTiffImage(FileName) {
   
    $(top.window.document).find("#PrintPDFModalAD").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitleAD")[0].textContent = "View AD Documents";
    $(top.window.document).find("#PrintPDFmdldlgAD")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlgAD")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrameAD")[0].style.height = "650px";
    $(top.window.document).find("#PrintPDFFrameAD")[0].contentDocument.location.href = "frmImageViewer.aspx?Source=ADINDEX";
}