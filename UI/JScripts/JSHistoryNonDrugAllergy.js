function AllOthersYesOrNo(ChkValue) {
    var pcontrol = document.getElementById(ChkValue);
    var dcontrol = document.getElementById("tblTest");

    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    if ( window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;

    if (pcontrol.name.indexOf("Yes") != -1)
    {
        for (i = 0; i < dcontrol.rows.length; i++)
        {
            if (dcontrol.rows[i].cells.length > 1)
            {
                if (pcontrol.checked == true)
                {
                    var NoChkBox = document.getElementById("chkAllNo");
                    NoChkBox.checked = false;

                    if (dcontrol.rows[i].cells[2].children[0].checked == false && dcontrol.rows[i].cells[1].children[0].checked == false) {
                        dcontrol.rows[i].cells[1].children[0].checked = true;
                        var DLCcontrol = dcontrol.rows[i].cells[1].children[0];
                        var TControl = DLCcontrol.id.replace("chkYes", "DLC");
                        //CAP-276 - Replace All space from TControl value
                        document.getElementById(TControl.replaceAll(" ","") + "_txtDLC").disabled = false;
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";                        
                        document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = false;                       
                        //changes  
                        var DControl = DLCcontrol.id.replace("chkYes", "DLC");
                        document.getElementById(DControl.replaceAll(" ", "") + "_pbDropdown").disabled = false;
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackground');
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackgrounddisable');
                    }
                }
                else
                {
                    dcontrol.rows[i].cells[1].children[0].checked = false;
                    if (dcontrol.rows[i].cells[2].children[0].checked == false)
                    {
                        var DLCcontrol = dcontrol.rows[i].cells[1].children[0];
                        var TControl = DLCcontrol.id.replace("chkYes", "DLC");
                        //CAP-276 - Replace All space from TControl value
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = true;
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";
                        document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = true;
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").css("background", "rgb(128, 128, 128) !important");
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackgrounddisable');
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackground');
                        var TNonDrugAllergy = TControl.replaceAll(" ", "") + "_listDLC";
                        document.getElementById(TNonDrugAllergy).style.display = "none";
                        var listcontrolNonDrugAllergy = document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown");
                        if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].className != null)
                            listcontrolSocialHistory.childNodes[0].className = "fa fa-plus margin2";
                        else if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].nextSibling.className != null)
                            listcontrolNonDrugAllergy.childNodes[0].nextSibling.className = "fa fa-plus margin2";
                    }
                }
            }
        }
    }
    else
    {
        for (i = 0; i < dcontrol.rows.length; i++) {

            if (dcontrol.rows[i].cells.length > 1)
            {
                if (pcontrol.checked == true) {
                    var YesChkBox = document.getElementById("chkAllYes");
                    YesChkBox.checked = false;

                    if (dcontrol.rows[i].cells[1].children[0].checked == false && dcontrol.rows[i].cells[2].children[0].checked == false) {
                        dcontrol.rows[i].cells[2].children[0].checked = true;

                        var DLCcontrol = dcontrol.rows[i].cells[1].children[0];
                        var TControl = DLCcontrol.id.replace("chkYes", "DLC");
                        //CAP-276 - Replace All space from TControl value
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = false;
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";                     
                        document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = false;                       

                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackground');
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackgrounddisable');
                    }

                }
                else
                {
                    dcontrol.rows[i].cells[2].children[0].checked = false;
                    if (dcontrol.rows[i].cells[1].children[0].checked == false)
                    {
                        var DLCcontrol = dcontrol.rows[i].cells[1].children[0];
                        var TControl = DLCcontrol.id.replace("chkYes", "DLC");
                        //CAP-276 - Replace All space from TControl value
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = true;
                        document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";                      
                        document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = true;
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").css("background", "rgb(128, 128, 128) !important");
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackgrounddisable');
                        $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackground');
                        var TNonDrugAllergy = TControl.replaceAll(" ", "") + "_listDLC";
                        document.getElementById(TNonDrugAllergy).style.display = "none";
                        var listcontrolNonDrugAllergy = document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown");
                        if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].className != null)
                            listcontrolSocialHistory.childNodes[0].className = "fa fa-plus margin2";
                        else if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].nextSibling.className != null)
                            listcontrolNonDrugAllergy.childNodes[0].nextSibling.className = "fa fa-plus margin2";
                    }
                }
            }
        }
    }
}

function enableField(chkValue) {
    var pcontrol = document.getElementById(chkValue);
    var YesChkBox = document.getElementById("chkAllYes");
    YesChkBox.checked = false;
    var NoChkBox = document.getElementById("chkAllNo");
    NoChkBox.checked = false;
    if (pcontrol.name.indexOf("Yes") != -1)
    {
        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
        if (pcontrol.checked == true)
        {
            var CControl = pcontrol.id.replace("chkYes", "chkNo");
            document.getElementById(CControl).checked = false;
            var TControl = pcontrol.id.replace("chkYes", "DLC");
            //CAP-276 - replace all spaces in the tcontrol name
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = false
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";
            document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = false;
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").css({ 'background': '', 'opacity': '' });
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackground');
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackgrounddisable');
        }
        else
        {
            var TControl = pcontrol.id.replace("chkYes", "DLC");
            //CAP-276 - Replace All space from TControl value
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = true;
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";
            document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = true;
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").css("background", "rgb(128, 128, 128) !important");
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackgrounddisable');
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackground');
            var TNonDrugAllergy = TControl.replaceAll(" ", "") + "_listDLC";
            document.getElementById(TNonDrugAllergy).style.display = "none";
            var listcontrolNonDrugAllergy = document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown");
            if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].className != null)
                listcontrolSocialHistory.childNodes[0].className = "fa fa-plus margin2";
            else if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].nextSibling.className != null)
                listcontrolNonDrugAllergy.childNodes[0].nextSibling.className = "fa fa-plus margin2";
        }
    }
    else
    {
        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;
        if (pcontrol.checked == true)
        {
            var CControl = pcontrol.id.replace("chkNo", "chkYes");
            document.getElementById(CControl).checked = false;
            var TControl = pcontrol.id.replace("chkNo", "DLC");
            //CAP-276 - Replace All space from TControl value
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = false;
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";
            document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = false;
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").css({ 'background': '', 'opacity': '' });
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackground');
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackgrounddisable');
        }
        else
        {
            var TControl = pcontrol.id.replace("chkNo", "DLC");
            //CAP-276 - Replace All space from TControl value
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").disabled = true;
            document.getElementById(TControl.replaceAll(" ", "") + "_txtDLC").value = "";
            document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown").disabled = true;
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").css("background", "rgb(128, 128, 128) !important");
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").addClass('pbDropdownBackgrounddisable');
            $('#' + TControl.replaceAll(" ", "") + "_pbDropdown").removeClass('pbDropdownBackground');
            var TNonDrugAllergy = TControl.replaceAll(" ", "") + "_listDLC";
            document.getElementById(TNonDrugAllergy).style.display = "none";
            var listcontrolNonDrugAllergy = document.getElementById(TControl.replaceAll(" ", "") + "_pbDropdown");
            if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].className != null)
                listcontrolSocialHistory.childNodes[0].className = "fa fa-plus margin2";
            else if (listcontrolNonDrugAllergy.childNodes[0] != undefined && listcontrolNonDrugAllergy.childNodes[0].nextSibling.className != null)
                listcontrolNonDrugAllergy.childNodes[0].nextSibling.className = "fa fa-plus margin2";
        }
    }
}

function btnClearAll_Clicked(sender, args) {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        if ( window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        else
            window.parent.theForm.hdnSaveEnable.value = false;
        document.getElementById('InvisibleButton').click();
        return true;
    } else {
        $find('btnSave').set_enabled(true);
        localStorage.setItem("bSave", "false");

        if ( window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
        else
            window.parent.theForm.hdnSaveEnable.value = true;

        return false;
    }

}

function EnableSave(e) {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function refreshSummaryBar() {    

    top.window.document.getElementById('ctl00_Loading').style.display = 'none';    

    var TabClick = top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick'); 
    var tabname = TabClick.value.split('$#$')[0];
    var clickedtab = tabname.split('@#');
    
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    
    if (clickedtab != "first") {
        if (clickedtab != 'CC / HPI' && clickedtab != 'QUESTIONNAIRE' && clickedtab != 'PFSH' && clickedtab != 'ROS' && clickedtab != 'VITALS' && clickedtab != 'EXAM' && clickedtab != 'TEST' && clickedtab != 'ASSESSMENT' && clickedtab != 'ORDERS' && clickedtab != 'eRx' && clickedtab != 'SERV./PROC. CODES' && clickedtab != 'PLAN' && clickedtab != 'SUMMARY')
            window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=" + tabname + "&hdnLocalTime=" + utc;
        else {
            window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=" + tabname + "&hdnLocalTime=" + utc;
        }
    } else {
        window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=Non Drug Allergy&hdnLocalTime=" + utc;
    }
    
   
}

function EnablePFSH(val) {
    if ($(window.parent.document).find('#btnPFSHVerified') != null)
        $(window.parent.document).find('#btnPFSHVerified')[0].disabled = false;
    

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        localStorage.setItem("bSave", "true");
    }
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

function EndWaitCursor() {
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
}


function cancelBack() {

    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");

    if ( window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}

function btnSave_Clicked(sender, args) {
    
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
   
}

function Enable_OR_Disable() {
    if ($find("btnSave").get_enabled())
        document.getElementById("Hidden1").value = "True";
    else
        document.getElementById("Hidden1").value = "";
}

function CCTextChanged() {
    $find("btnSave").set_enabled(true);
    if ( window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;

    document.getElementById("Hidden1").value = "True";
    return false;
}

function refreshSummaryBar(tabname) {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var clickedtab = tabname.split('@#');
    if (clickedtab != "first") {
        if (clickedtab != 'CC / HPI' && clickedtab != 'QUESTIONNAIRE' && clickedtab != 'PFSH' && clickedtab != 'ROS' && clickedtab != 'VITALS' && clickedtab != 'EXAM' && clickedtab != 'TEST' && clickedtab != 'ASSESSMENT' && clickedtab != 'ORDERS' && clickedtab != 'eRx' && clickedtab != 'SERV./PROC. CODES' && clickedtab != 'PLAN' && clickedtab != 'SUMMARY')
            window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=" + tabname + "&hdnLocalTime=" + utc;
        else {
            window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=" + tabname + "&hdnLocalTime=" + utc;
        }
    } else {
        window.parent.parent.parent.location.href = "frmPatientChart.aspx?tabName=PFSH&ChildTabName=Non Drug Allergy&hdnLocalTime=" + utc;
    }
}

function HistoryNonDrug_Load() {

    

    $("textarea[id *= txtDLC]").addClass('spanstyle');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined)
    window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}


function DuplicateNotesError(){
    top.window.document.getElementById('ctl00_Loading').style.display = 'none';
    PFSH_SaveUnsuccessful();
    DisplayErrorMessage('180042');
}

function RefreshFloatingSummary() {    
    var dox = window.parent.window.parent.window.document;    
    var iFrame = dox.getElementsByTagName("iframe");
    if (iFrame.length > 0) {
        var str = iFrame[0].src;
        var n = str.indexOf("frmFollowUpEncounter.aspx");
        if (n >= 0) {
            iFrame[0].src = iFrame[0].src;            
        }else {
        
        }
    } else {
        //console.log("Error in finding the Floating Summary");
    }
} 

function SetAllergySummary(allergyText, SummaryToolTip) {
    
    localStorage.setItem("bSave", "true");
    var dox = window.parent.window.parent.window.parent.window.document;
    var AllergyText = dox.all.ctl00_C5POBody_lblAllergies;

    var pnl = dox.all.ctl00_C5POBody_pnlAllergies;
    var regex = /<BR\s*[\/]?>/gi;
    
    top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = allergyText;
    top.window.document.getElementById("Allergies_tooltp").innerText = allergyText.replace(regex, "\n")+"\n";
    RefreshOverallSummaryTooltip();
    
}


function SavedSuccessfully() {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    DisplayErrorMessage('180602');  
    localStorage.setItem("bSave", "true");
    RefreshNotification('NonDrugAllergy');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
}


function SavedSuccesfullyNDA(){
     
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
    DisplayErrorMessage('180602');
}

function SetUpdatedNDA(NDAContent) {
    var dox = window.parent.window.parent.window.parent.window.document; 
    var NDAText = dox.getElementById("ctl00_C5POBody_lblAllergies");        
    NDAText.innerHTML = NDAContent;   
}
function checkLength(el) {
    el.maxLength = 255;
    if (el.value.length > 255) {
        var val = el.value.substring(0, 255);
        el.value = val;
        return false;       
    }
    EnableSave();
}