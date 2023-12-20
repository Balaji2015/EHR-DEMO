var procedure = "";
function OpenLabLocationScreen(LabID, LabName) {
    var objSelectLabLocation = new Array();
    objSelectLabLocation.push("LabName=" + LabName);
    objSelectLabLocation.push("LabID=" + LabID);
    var SelectLabLocationResult = openModal("frmSelectLabLocation.aspx", 590, 720, objSelectLabLocation, 'MessageWindow');
    var WindowName = $find('MessageWindow');
    WindowName.add_close(OnClientCloseLabLocation);


}
MaxCounts = new Array();
function OpenInsurance() {
    var obj = new Array();
    obj.push("HumanId=" + document.getElementById('hdnHumanID_EncounterID_PhysicianID').value.split(',')[0]);
    var Result = openModal("frmPatientInsurancePolicyMaintenance.aspx", 645, 1000, obj, 'MessageWindow');
    EnableSaveDiagnosticOrder();
    return false;
}
function FormateFrequentlyUsedList() {

    var tabletobeappende = document.getElementById("chklstFrequentlyUsedProcedures");
    if (tabletobeappende != null && tabletobeappende.cells != undefined) {
        var AllCells = tabletobeappende.cells;
        for (var i = 0; i < AllCells.length; i++) {
            if (AllCells[i].all.length > 2 && AllCells[i].all[0].getAttribute("IsHeader") == "true") {
                AllCells[i].all[2].style.setProperty("color", "blue", null);
                AllCells[i].all[2].style.setProperty("font-weight", "bold", null);
            }
        }
    }
}

function GetClientId(strid) {
    var count = document.forms[0].length;
    var i = 0; var eleName;
    for (i = 0; i < count; i++) {
        eleName = document.forms[0].elements[i].id;
        pos = eleName.indexOf(strid);
        if (pos >= 0) break;
    }
    return eleName;
}
function Blood() {
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("span[mand=Yes]").addClass('MandLabelstyle');

    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
function OpenInsurancePolicy() {
    var humanid = document.getElementById('humanID').value;
    var Result = window.showModalDialog("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + humanid, null, "center:yes;resizable:yes;dialogHeight:570px;dialogWidth:1220px;scroll:yes;");
}
function OpenSpecialityDiagonsisImageandLab() {
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    if (document.getElementById('hdnUserName') != null)
        $(top.window.document).find("#TabModalTitle")[0].textContent = "All Diagnosis" + " - " + document.getElementById('hdnUserName').value;
    else
        $(top.window.document).find("#TabModalTitle")[0].textContent = "All Diagnosis";
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "54%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "67%";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find(".modal-body")[8].style.height = "92.5%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmSpecialityDiagnosis.aspx?sourceScreen=" + "ORDERS";
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function (e) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        OnClientCloseDiagnosis(null, e);
    });
   // EnableSaveDiagnosticOrder();
    return false;
}
function OpenManagedFrequentlyUsedProcedures() {


    var btnOrderSubmit = document.getElementById("btnOrderSubmit") //$find("btnOrderSubmit");
    var btnOrderSubmitText = btnOrderSubmit.get_text();

    if (btnOrderSubmitText != undefined && btnOrderSubmitText.toUpperCase() == "UPDATE") {
        DisplayErrorMessage('230142');
        return false;
    }
    var cboLab = document.getElementById("cboLab");
    if (cboLab.Text == "") {
        DisplayErrorMessage('230140');
        return false;
    }



    var obj = new Array();
    var ResultManagedFrequentlyUsedProcedures = openModal("frmLabProcedureManage.aspx", 577, 620, obj, 'MessageWindow');
    if (ResultManagedFrequentlyUsedProcedures != undefined) {
        var elementRef = document.getElementById("hdnTransferVaraible");
        elementRef.value = ResultManagedFrequentlyUsedProcedures.SelectedCPT;
    }
}
function OpenPDFImage(FaxSubject) {
   
    if(FaxSubject!="")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
   
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Orders";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "685px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('hdnSelectedItem').value + "&Location=DYNAMIC" + "&FromOrder=Y";
    //CAP-1498
    document.getElementById('btnOrderSubmit').disabled = true;
}
function OnClientCloseDiagnosis(oWindow, args) {
    if (oWindow != null) {
        oWindow.remove_close(OnClientCloseSelectLabLocation);
    }
    else {
        $(top.window.document).find("#TabModal").on('hidden', function (e) {
            OnClientCloseSelectLabLocation(null, args);
        });
    }
    document.getElementById("btninvisibleDiagnosis").click();
      
}
function chkMoveToMA_Click() {

    var chkMoveToMA = document.getElementById('chkMoveToMA');
    if (!chkMoveToMA.checked) {
        var chkSpecimenInHouse = document.getElementById('chkSpecimenInHouse');
        var lblCollectionDate = document.getElementById('lblCollectionDate');
        if (chkSpecimenInHouse.checked) {
            if (lblCollectionDate.innerText != "Collection Date*")
            {
                lblCollectionDate.innerText += "*";
            }
                
            document.getElementById('hdnCollectionDateIsMand').value = "true";
            $(lblCollectionDate).html($(lblCollectionDate).html().replace("*", "<span class='manredforstar'>*</span>"));
            $('#lblCollectionDate').addClass('MandLabelstyle');
            $('#lblCollectionDate').removeClass('spanstyle');
        }
        else {
            lblCollectionDate.innerText = lblCollectionDate.innerText.replace('*', ' ').trim();
            document.getElementById('hdnCollectionDateIsMand').value = "false";
            lblCollectionDate.style.color = "black";
            $('#lblCollectionDate').addClass('spanstyle');
            $('#lblCollectionDate').removeClass('MandLabelstyle');

        }
    }
    else {
        var lblCollectionDate = document.getElementById('lblCollectionDate');
        if (UserRole != "Medical Assistant") {
            lblCollectionDate.innerText = lblCollectionDate.innerText.replace('*', ' ').trim();
            lblCollectionDate.style.color = "Black";
            document.getElementById('hdnCollectionDateIsMand').value = "false";
            $('#lblCollectionDate').addClass('spanstyle');
            $('#lblCollectionDate').removeClass('MandLabelstyle');
        }
    }
    EnableSaveDiagnosticOrder();
}
function ChkSpecimenInHouseCheck() {
    var lblCollectionDate = document.getElementById('lblCollectionDate');
    if ((UserRole == "Medical Assistant") || (UserRole == "Technician")) {
        var chkSpecimenInHouse = document.getElementById('chkSpecimenInHouse');

        if (chkSpecimenInHouse.checked) {
            lblCollectionDate.innerText = "Collection Date*";
            document.getElementById('hdnCollectionDateIsMand').value = "true";
            $(lblCollectionDate).html($(lblCollectionDate).html().replace("*", "<span class='manredforstar'>*</span>"));
            $('#lblCollectionDate').addClass('MandLabelstyle');
            $('#lblCollectionDate').removeClass('spanstyle');
        }
        else {
            lblCollectionDate.innerText = lblCollectionDate.innerText.replace('*', ' ').trim();
            document.getElementById('hdnCollectionDateIsMand').value = "false";
            lblCollectionDate.style.color = "black";
            $('#lblCollectionDate').addClass('spanstyle');
            $('#lblCollectionDate').removeClass('MandLabelstyle');
        }
    }
    else {
        var chkSpecimenInHouse = document.getElementById('chkSpecimenInHouse');
        var chkMoveToMA = document.getElementById('chkMoveToMA');
        if (chkSpecimenInHouse.checked) {
            chkMoveToMA.checked = chkSpecimenInHouse.checked;
        }
        else {
            if (!chkMoveToMA.checked) {
                lblCollectionDate.innerText = lblCollectionDate.innerText.replace('*', ' ').trim();
                document.getElementById('hdnCollectionDateIsMand').value = "false";
                lblCollectionDate.style.color = "black";
                $('#lblCollectionDate').addClass('spanstyle');
                $('#lblCollectionDate').removeClass('MandLabelstyle');
            }

        }
    }
    EnableSaveDiagnosticOrder();
}
function chkTestDataeInWordsClick() {
    var chktestDateInWords = document.getElementById('chkTestDateInWords');
    var chktestDateInDate = document.getElementById('chkTestDateInDate');
    var cstdtptestDate = document.getElementById('cstdtpTestDate');
    var chkstat = document.getElementById('chkStat');
    var chkUrgent = document.getElementById('chkUrgent');
    if (chktestDateInWords.checked) {
        chkstat.disabled = chktestDateInWords.checked;
        chkstat.checked = false;
        chkUrgent.disabled = chktestDateInWords.checked;
        chkUrgent.checked = false;
        chktestDateInDate.checked = false;
        document.getElementById('cstdtpTestDate').disabled = true; //$find("cstdtpTestDate").set_enabled(false);
        $('#cstdtpTestDate').addClass('nonEditabletxtbox');
        $('#cstdtpTestDate').removeClass('Editabletxtbox');
        document.getElementById('txtMonths').disabled = false;
        document.getElementById('txtWeeks').disabled = false;
        document.getElementById('txtDays').disabled = false;
        $('#txtMonths').removeClass('nonEditabletxtbox');
        $('#txtWeeks').removeClass('nonEditabletxtbox');
        $('#txtDays').removeClass('nonEditabletxtbox');
        $('#txtMonths').addClass('Editabletxtbox');
        $('#txtWeeks').addClass('Editabletxtbox');
        $('#txtDays').addClass('Editabletxtbox');
        document.getElementById('txtMonths').readOnly= false;
        document.getElementById('txtWeeks').readOnly = false;
        document.getElementById('txtDays').readOnly = false;
    }
    else {
        document.getElementById('cstdtpTestDate').disabled = false;
        $('#cstdtpTestDate').removeClass('nonEditabletxtbox');
        $('#cstdtpTestDate').addClass('Editabletxtbox');
        chkstat.disabled = true;
        chkUrgent.disabled = true;
        chktestDateInDate.checked = true;
    }
    if (chktestDateInDate.checked) {
        chkstat.disabled = false;
        chkUrgent.disabled = false;
        document.getElementById('txtMonths').value = '';
        document.getElementById('txtWeeks').value = ''; 
        document.getElementById('txtDays').value = '';
        document.getElementById('txtMonths').disabled = true;
        document.getElementById('txtWeeks').disabled = true;
        document.getElementById('txtDays').disabled = true;
        $('#txtMonths').removeClass('Editabletxtbox');
        $('#txtWeeks').removeClass('Editabletxtbox');
        $('#txtDays').removeClass('Editabletxtbox');
        $('#txtMonths').addClass('nonEditabletxtbox');
        $('#txtWeeks').addClass('nonEditabletxtbox');
        $('#txtDays').addClass('nonEditabletxtbox');
        document.getElementById('txtMonths').readOnly = true;
        document.getElementById('txtWeeks').readOnly = true;
        document.getElementById('txtDays').readOnly = true;
    }

    EnableSaveDiagnosticOrder();

}
function chkTestDataeInDateClick() {
    var chktestDateInDate = document.getElementById('chkTestDateInDate');
    var chktestDateInWords = document.getElementById('chkTestDateInWords');
    var chkstat = document.getElementById('chkStat');
    var chkUrgent = document.getElementById('chkUrgent');
    chktestDateInWords.checked = !chktestDateInDate.checked;
    if (chktestDateInDate.checked) {
        chkstat.disabled = false;
        chkUrgent.disabled = false;
    }
    else {
        chkstat.disabled = true;
        chkUrgent.disabled = true;
        document.getElementById('txtMonths').disabled = false;
        document.getElementById('txtWeeks').disabled = false;
        document.getElementById('txtDays').disabled = false;
        $('#txtMonths').removeClass('nonEditabletxtbox');
        $('#txtWeeks').removeClass('nonEditabletxtbox');
        $('#txtDays').removeClass('nonEditabletxtbox');
        $('#txtMonths').addClass('Editabletxtbox');
        $('#txtWeeks').addClass('Editabletxtbox');
        $('#txtDays').addClass('Editabletxtbox');
        document.getElementById('txtMonths').readOnly = false;//.disabled = false; //$find('txtMonths').enable();
        document.getElementById('txtWeeks').readOnly = false;//.disabled = false;//$find('txtWeeks').enable();
        document.getElementById('txtDays').readOnly = false;//.disabled = false;//$find('txtDays').enable();
    }

    chkstat.checked = false;
    chkUrgent.disabled = false;
    document.getElementById('cstdtpTestDate').disabled = (!chktestDateInDate.checked); //$find("cstdtpTestDate").set_enabled(chktestDateInDate.checked);
    if (chkstat.checked  == true) {
        $('#cstdtpTestDate').addClass('nonEditabletxtbox');
        $('#cstdtpTestDate').removeClass('Editabletxtbox');
    }
    else
    {
        $('#cstdtpTestDate').removeClass('nonEditabletxtbox');
        $('#cstdtpTestDate').addClass('Editabletxtbox');
    }

    if (chktestDateInDate.checked) {
        document.getElementById('txtMonths').value = '';//$find('txtMonths').set_value("");
        document.getElementById('txtWeeks').value = ''; //$find('txtWeeks').set_value("");
        document.getElementById('txtDays').value = '';//$find('txtDays').set_value("");
        document.getElementById('txtMonths').disabled = true;
        document.getElementById('txtWeeks').disabled = true;
        document.getElementById('txtDays').disabled = true;
        $('#txtMonths').removeClass('Editabletxtbox');
        $('#txtWeeks').removeClass('Editabletxtbox');
        $('#txtDays').removeClass('Editabletxtbox');
        $('#txtMonths').addClass('nonEditabletxtbox');
        $('#txtWeeks').addClass('nonEditabletxtbox');
        $('#txtDays').addClass('nonEditabletxtbox');
        document.getElementById('txtMonths').readOnly = true;//.disabled = true;//$find('txtMonths').disable();
        document.getElementById('txtWeeks').readOnly = true;//.disabled = true;//$find('txtWeeks').disable();
        document.getElementById('txtDays').readOnly = true;//.disabled = true;//$find('txtDays').disable();
    }
    EnableSaveDiagnosticOrder();
}
function Quality_TextChanged(sender, args) {
    var lblunits = document.getElementById("lblUnits");
    if (document.getElementById("txtQuantity").value != "" && lblunits.innerHTML.indexOf("*") == -1) {
        lblunits.innerHTML += "*";
        $(lblCollectionDate).html($(lblCollectionDate).html().replace("*", "<span class='manredforstar'>*</span>"));
        $(lblunits).html($(lblunits).html().replace("*", "<span class='manredforstar'>*</span>"));
        $('#lblUnits').removeClass('spanstyle');
        $('#lblUnits').addClass('MandLabelstyle');
    }
    else if (document.getElementById("txtQuantity").value == "") {
        lblunits.innerHTML = lblunits.innerHTML.replace('*', ' ');
        lblunits.style.color = "black";
        $('#lblUnits').removeClass('MandLabelstyle');
        $('#lblUnits').addClass('spanstyle');
    }

    EnableSaveDiagnosticOrder();
}
function AllowNumbersImageAndLab(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;

}
function ShowLoading() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function EnableImportResults() {
    var cboLab = false;
    if (document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].value == '32') { document.getElementById('btnImportresult').disabled = false; } else { document.getElementById('btnImportresult').disabled = true; }
    document.getElementById('btnOrderSubmit').disabled = false; //$find('btnOrderSubmit').set_enabled(true);
}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
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
        $(top.window.document).find('#btnClosed')[0].click(oArg.result);
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }
}
function OnClientCloseSelectLabLocation(oWindow, args) {
    if (oWindow != null) {
        oWindow.remove_close(OnClientCloseDiagnosis);
    }
    else {
        $(top.window.document).find("#TabModal").on('hidden', function (e) {
            OnClientCloseDiagnosis();
        });
    }
    if (args != null) {
        var arg = args;
        if (arg) {
            var elementRef = document.getElementById("txtLocation");
            elementRef.value = arg;
        }
    }

    if (oWindow != null) {
        oWindow.remove_close(OnClientCloseSelectLabLocation);
    }
    else {
        $(top.window.document).find("#TabModal").on('hidden', function (e) {
            OnClientCloseSelectLabLocation();
        });
    }
}
function EnableSaveDiagnosticOrder() {
    if (document.getElementById('btnOrderSubmit') != undefined) {
        document.getElementById('btnOrderSubmit').disabled = false;
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true;
    }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
    //CAP-1501
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function EnableSaveDiagnosticOrderbilltype() {
    if (document.getElementById('btnOrderSubmit') != undefined) {
        document.getElementById('btnOrderSubmit').disabled = false;
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true;
    }
    var cboBillType = document.getElementById("cboBillType");
    __doPostBack('cboBillType', cboBillType);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
}

function chkpaperorderChanged() {
    if (document.getElementById('btnOrderSubmit') != undefined) {
        document.getElementById('btnOrderSubmit').disabled = false;
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true;
    }
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
    var chkPaper = document.getElementById("chkpaperorder");
    if (chkPaper.checked) {
        document.getElementById("btnOrderSubmit").disabled = false;
        document.getElementById("rbImageOrder").disabled = false;
        document.getElementById("rbLabOrder").disabled = false;
        if (document.getElementById("rbImageOrder").checked)
            document.getElementById("rbLabOrder").checked = false;
        if (document.getElementById("rbLabOrder").checked)
            document.getElementById("rbImageOrder").checked = false;
    }
    else {
        document.getElementById("rbLabOrder").disabled = true;
        document.getElementById("rbImageOrder").disabled = true;
        document.getElementById("rbImageOrder").checked = false;
        document.getElementById("rbLabOrder").checked = false;
    }
}

function cboLab_SelectedIndexChanging(sender, args) {

    var chkBoxList = document.getElementById("chklstAssessment");
    var chkBoxCount = chkBoxList.getElementsByTagName("input");
    for (i = 0; i < chkBoxCount.length; i++) {
        if (chkBoxCount[i].checked) {

            var button2 = $find("btnImportresult");
            button2._enabled = true;
            break;
        }
    }

    ShowLoading();
}

function MoveToNextProcessClicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (document.getElementById('btnOrderSubmit').disabled == false) {
        event.preventDefault();
        event.stopPropagation();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        sessionStorage.setItem("autoMovetonxtProc", "true");
        document.getElementById('hdnType').value = "Yes";

        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        __doPostBack('btnMoveToNextProcess'); //document.getElementById("btnMoveToNextProcess").click(); //$find('btnMoveToNextProcess').click();
        //if ($(dvdialog) != undefined && $(dvdialog)!=null)
        //   $(dvdialog).dialog("close");

        //Jira #CAP-889
        RemoveItem(document.URL, "Orders");
        return true;
        //if (!$($(top.window.document).find('iframe')[0].contentDocument).find("body").is('#dvdialogMenu'))
        //    $($(top.window.document).find('iframe')[0].contentDocument).find("body").append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
        //    '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
        //dvdialog = $($(top.window.document).find('iframe')[0].contentDocument).find("body").find('#dvdialogMenu');
        //event.preventDefault();
        //event.stopPropagation();
        // {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}

        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        //            sessionStorage.setItem("autoMovetonxtProc", "true");
        //            document.getElementById('hdnType').value = "Yes";

        //            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        //            __doPostBack('btnMoveToNextProcess'); //document.getElementById("btnMoveToNextProcess").click(); //$find('btnMoveToNextProcess').click();
        //            $(dvdialog).dialog("close");
        //            return true;
        //        },
        //        "No": function () {
        //            document.getElementById('hdnType').value = "";
        //            __doPostBack('btnMoveToNextProcess');
        //            $(dvdialog).dialog("close");
        //            self.close();
        //             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //        },
        //        "Cancel": function () {
        //             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        //            document.getElementById('hdnType').value = "";
        //            $(dvdialog).dialog("close");

        //            return false;
        //        }
        //    }
        //});

        
    }
    else
    {
        //Jira #CAP-889
        RemoveItem(document.URL, "Orders");
        return true;
    }

}
function saveorder() {
    
    var rows = $('#tabMed tbody >tr');
    var columns;
    var inputdata = "";
    var quantity = 0;
    for (var i = 0; i < rows.length; i++) {
        columns = $(rows[i]).find('td');
        if (columns != undefined) {
            if ($(columns[1])[0].childNodes[0].value!="")
             quantity = $(columns[1])[0].childNodes[0].value;
           // if (quantity != "") {
                var order_id = $(columns[2]).html();
                var data = quantity + "|" + order_id;
                if (i == 0 && inputdata=="")
                    inputdata = data
                else
                    inputdata = inputdata + "~" + data;
           // }
        }

    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (inputdata != "") {
        $.ajax({
            type: "POST",
            url: "frmImageAndLabOrder.aspx/updateOrderQuantity",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({
                "Quantity": inputdata,
            }),
            datatype: "json",
            success: function success(data) {
                $('#btnsave').attr("disabled", "disabled");
                $(top.window.document).find("#btnCloseMed")[0].click();
             
                localStorage.setItem("ProcedureQuantity", inputdata);
                $("#hdnbuttonload").click();
               
            },
            error: function onerror(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
}
var OrderIdLst = [];
function OpenMedication_dosage() {
    
    //if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        //if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            //document.getElementById("btnOrderSubmit").click();
            //return true;
        //}
    //}
    //if (document.getElementById('btnOrderSubmit').disabled == false) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            DisplayErrorMessage('230145');
            return false;
        }
    }
    var order_id = 1544140;
    var order_id2 = 1544141;
    OrderIdLst.push(order_id);
    OrderIdLst.push(order_id2);
    procedure = "";
    $("input[type=checkbox]:checked").each(function () {

        if ($('#' + this.id)[0].labels[0].innerText.indexOf('x__') > -1) {
            if (procedure == "")
            //CAP-1471
                procedure = $('#' + this.id)[0]?.labels[0]?.innerText + "|" + $('#' + this.id)[0]?.parentElement?.attributes["orderid"]?.value;
            else {
                procedure = procedure + "~" + $('#' + this.id)[0]?.labels[0]?.innerText + "|" + $('#' + this.id)[0]?.parentElement?.attributes["orderid"]?.value;
            }
        }


    });

    localStorage.setItem("procedure", procedure);
    $(top.window.document).find("#ProcessModalMed")[0].style.height = "47%";
    $(top.window.document).find("#ProcessModalMed")[0].style.width = "800px";
    $(top.window.document).find("#ProcessModalMed").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#mdldlgMed")[0].style.width = "86%";
    $(top.window.document).find("#ProcessFrameMed")[0].style.height = "275px";
    $(top.window.document).find("#ModalTitleMed").html("Medication Dosage");
    $(top.window.document).find("#ModalTitleMed")[0].textContent = "Medication Dosage";
    $(top.window.document).find('#ProcessFrameMed')[0].src = "HtmlMedicineDosage.html?version=" + sessionStorage.getItem("ScriptVersion");
    return false;
}

function closepopup() {
    if (!$('#btnsave').attr("disabled")) 
    {
        sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
        saveorder();

        sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        return false;
        //$("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
        //                       "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        //dvdialog = $('#dvdialogMenu');
        //myPos = "center center";
        //atPos = 'center center';

        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
        //            saveorder();
                  
        //            sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        //            return false;
        //        },
        //        "No": function () {
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            $(top.window.document).find("#btnCloseMed").click();
        //            return false;
        //        },
        //        "Cancel": function () {
        //            $(dvdialog).dialog("close");
        //            $(dvdialog).remove();
        //            return false;
        //        }
        //    }
        //});
    }
    else {
        $(top.window.document).find("#btnCloseMed").click();
        if (localStorage.getItem("ProcedureQuantity") != "") {
            var datalist = localStorage.getItem("ProcedureQuantity").split('~');
            for (var i = 0; i < datalist.length; i++) {
                var quantity = datalist[i].split('|');
                var orderid = quantity[1];
                var outputtextQuantity = quantity[0];

                if ($("span[orderid='" + orderid + "']")[0] != undefined) {
                    var texquantity = $("span[orderid='" + orderid + "']")[0].childNodes[0].labels[0].innerText.split('x___')
                    if (outputtextQuantity != "" && outputtextQuantity != "0")
                        $("span[orderid='" + orderid + "']")[0].childNodes[0].labels[0].innerText = texquantity[0] + "x___" + outputtextQuantity + "___"
                    else {
                        $("span[orderid='" + orderid + "']")[0].childNodes[0].labels[0].innerText = texquantity[0] + "x___" + "___"

                    }
                }
            }
        }
     
        return false;
    }
}
$(top.window.document).find("#ProcessModalMed").on("hidden.bs.modal", function () {
    // put your default event here
  
        if (localStorage.getItem("ProcedureQuantity") != "") {
            var datalist = localStorage.getItem("ProcedureQuantity").split('~');
            for (var i = 0; i < datalist.length; i++) {
                var quantity = datalist[i].split('|');
                var orderid = quantity[1];
                var outputtextQuantity = quantity[0];
                //CAP-1471
                if ($("span[orderid='" + orderid + "']") != undefined && $("span[orderid='" + orderid + "']") != null && $("span[orderid='" + orderid + "']")[0] != undefined && $("span[orderid='" + orderid + "']")[0] != null)
                {
                    var texquantity = $("span[orderid='" + orderid + "']")[0]?.childNodes[0]?.labels[0]?.innerText?.split('x___')??"";
                    if (outputtextQuantity != "" && outputtextQuantity != "0")
                        $("span[orderid='" + orderid + "']")[0].childNodes[0].labels[0].innerText = texquantity[0] + "x___" + outputtextQuantity + "___"
                    else {
                        $("span[orderid='" + orderid + "']")[0].childNodes[0].labels[0].innerText = texquantity[0] + "x___" + "___"
                    }
                }
            }
        }
        $("#hdnbuttonload").click();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    
});
function isNumberKey(evt) {
  
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;
  
       
}
function enablesave() {
    $('#btnsave').attr("disabled", false);
}
function BindMedication() {
    var procedure = localStorage.getItem("procedure");
    var tbody = $("#tabMed");
    $("#tabMed").empty();
    var tr = '<thead class="Gridheaderstyle"><tr><th>Lab Procedure</th><th>Quantity</th>';
    tbody.append(tr);
    if (procedure != "") {
        var prolist = procedure.split('~');
        var Qty = "";
        var tr;
        for (var i = 0; i < prolist.length; i++) {
            if (prolist[i].split('|')[0].split("x___")[1].length > 3) {
               Qty = prolist[i].split('|')[0].split("x___")[1].split("___")[0];
            }
            if (Qty != "" && Qty != "0") {
                var procedure_item = prolist[i].split('|')[0].split('x___')[0] + "x______";
                tr = '<tr><td>' + procedure_item + '</td><td><input type="textbox" class="Editabletxtbox" onkeypress="return isNumberKey(event);" onkeyup="enablesave();" value="' + Qty + '"/></td><td style="display:none">' + prolist[i].split('|')[1] + '</td>';

            }
            else
                tr = '<tr><td>' + prolist[i].split('|')[0] + '</td><td><input type="textbox" class="Editabletxtbox"  value="1.00" onkeypress="return isNumberKey(event);" onkeyup ="enablesave();"/></td><td style="display:none">' + prolist[i].split('|')[1] + '</td>';
            tbody.append(tr);
        }
        $("[id*=pbDropdow]").addClass('pbDropdownBackground');
    }
    else {
        $('#btnsave').attr("disabled", "disabled");
    }
}
function btnOrderSubmit_Clicked(sender) {
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    document.getElementById("hdnOrderSubmitClick").value = "Yes";
    var lblCollectionDate = document.getElementById("lblCollectionDate")
    var dtpCollectionDate = document.getElementById("dtpCollectionDate").value.substring(0, 11)
    var dtpTestDate = document.getElementById("cstdtpTestDate").value
    if (lblCollectionDate.style.color == "red" && document.getElementById("dtpCollectionDate").value==""){//$find("dtpCollectionDate").get_dateInput()._textBoxElement.value == "") {
        DisplayErrorMessage('230153');
        Order_SaveUnsuccessful();
        return false;

    }
    if (new Date(dtpCollectionDate) > new Date(dtpTestDate))
    {
        DisplayErrorMessage('230163');
        Order_SaveUnsuccessful();
        return false;
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    return true;
}
function ImageAndLabOrderPlan(sender, args) {
    var obj = new Array();
    var values = document.getElementById('hdnHumanID_EncounterID_PhysicianID').value.split(',');
    obj.push("humanid=" + values[0]);
    obj.push("EncId=" + values[1]);
    obj.push("PhyId=" + values[2]);
    var ResultSpecialityDiagonsis = openModal("frmAddorUpdatePlan.aspx", 440, 900, obj, 'MessageWindow');
    resultwin = $find('MessageWindow');
    resultwin.add_close(OnClientClosePlan);
}
function btnPlan_Clicked(sender, args) {
    var obj = new Array();
    var ResultSpecialityDiagonsis = openModal("frmAddorUpdatePlan.aspx", 440, 900, obj, 'MessageWindow');
    resultwin = $find('MessageWindow');
    resultwin.add_close(OnClientClosePlan);
}
function btnOrderList_Clicked(sender, args) {
    var OrderListOpen = new Array();
    var ResultOrderList = openModal("frmOrdersList.aspx", 900, 1000, OrderListOpen, 'MessageWindow');
    if (ResultOrderList != undefined) {
        var elementRef = document.getElementById("hdnTransferVaraible");
        elementRef.value = ResultOrderList.OrderSubmitID;
    }
}
function btnAllProcedures_Clicked(sender, args) {
   
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var selectedLab = document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text;
    var cboLab = document.getElementById("cboLab");//$find("cboLab");
    if (document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text==undefined) { //if (cboLab.get_selectedItem() == undefined) {
        DisplayErrorMessage('230140');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return;
    }
    if ( document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text.trim()==""){//cboLab.get_selectedItem().get_text().trim() == "") {
        DisplayErrorMessage('230140');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return;
    }
    var phyID = document.getElementById("hdnPhysicianID").value;
    var proType = document.getElementById("hdnProcedureType").value;
    var selectedLabID = cboLab.value;//cboLab.get_selectedItem().get_value();
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "680px";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "530px";
    $(top.window.document).find("#TabModalTitle")[0].textContent = "All Procedures";
    $(top.window.document).find("#TabFrame")[0].style.height = "485px";
    $(top.window.document).find(".modal-body")[8].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmAllProcedures.aspx?ulMyPhysicianID=" + phyID + "&procedureList=LabCorp" + "&procedureType=LAB PROCEDURE" + "&selectedLabID=" + selectedLabID + "&SelectedLab=" + selectedLab + "&IsAllProcedure=" + true;
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
        CloseAllProcedures();
    });
    return;
}
function CloseAllProcedures(oWindow, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var arg = sessionStorage.getItem("AllProc_SelectCPT");
    if (arg != undefined) {
        var elementRef = document.getElementById("hdnTransferVaraible");
        elementRef.value = arg;
        __doPostBack('btnAllProcedures');
    }
    else
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}

function tblSelectProcedure_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var cboLab = document.getElementById("cboLab"); //$find("cboLab");
    if (cboLab.options[cboLab.selectedIndex].text == undefined) {
        DisplayErrorMessage('230140');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return;
    }
    if (cboLab.options[cboLab.selectedIndex].text=="") {//(cboLab.get_selectedItem().get_text().trim() == "") {
        DisplayErrorMessage('230140');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return;
    }
    var btnOrderSubmit = document.getElementById('btnOrderSubmit');
    if (btnOrderSubmit.value != undefined && btnOrderSubmit.value.toUpperCase() == "UPDATE") {
        DisplayErrorMessage('230142');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return;
    }

    var phyID = document.getElementById("hdnPhysicianID").value;
    var proType = document.getElementById("hdnProcedureType").value;
    var phyID = document.getElementById("hdnPhysicianID").value;

    var selectedLabID = cboLab.value;//cboLab.get_selectedItem().get_value();
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "85%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "73%";
    $(top.window.document).find("#TabModalTitle")[0].textContent = "Manage Frequently Used Procedures";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find(".modal-body")[8].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmLabProcedureManage.aspx?ulMyPhysicianID=" + phyID + "&procedureList=LabCorp" + "&procedureType=LAB PROCEDURE" + "&selectedLabID=" + selectedLabID + "&SelectedLab=" + cboLab.options[cboLab.selectedIndex].text + "&IsAllProcedure=" + true;
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
        OnClientCloseLabProcedureManager();
    });

}
function OnClientCloseLabProcedureManager(sender, eventArgs) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var hdnvariable = document.getElementById("hdnManageFreqUsed");
    hdnvariable.value = "true";
    document.getElementById('InvisibleButton').click();
}
function btnSelectLocation_Clicked(sender, args) {
    var cboLab = document.getElementById("cboLab");// $find("cboLab");
    var selectedLabID = cboLab.value;// cboLab.get_selectedItem().get_value();
    var objSelectLabLocation = new Array();
    objSelectLabLocation.push("LabName=" + cboLab.options[cboLab.selectedIndex].text);//cboLab.get_selectedItem().get_text()
    objSelectLabLocation.push("LabID=" + selectedLabID);
    var SelectLabLocationResult = openModal("frmSelectLabLocation.aspx", 618, 691, objSelectLabLocation, 'MessageWindow');
    var WindowName = $find('MessageWindow');
    WindowName.add_close(OnClientCloseSelectLabLocation);
}
function FormLoad() {
    
    var objgbSpecimenDetails = document.getElementById("gbSpecimenDetails");
    var objgbOrderDetails = document.getElementById("gbOrderDetails");
    objgbOrderDetails.style.height = objgbSpecimenDetails.offsetHeight;
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined && window.parent.parent.theForm.hdnSaveButtonID != null)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnOrderSubmit,mulPageOrders";
    if (window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != undefined && window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID != null)
        window.parent.parent.parent.theForm.ctl00_C5POBody_hdnSaveButtonID.value = "btnOrderSubmit,mulPageOrders";
    //GetUTCTime();
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    if (document.getElementById('cstdtpTestDate') != null) {
        if (document.getElementById("hdnsaveEnable").value == "Load")
            document.getElementById("hdnsaveEnable").value = '';
        else {
            setTimeout(function () { document.getElementById('cstdtpTestDate').value = new Date(document.getElementById("hdnLocalTime").value).format('dd-MMM-yyyy');},0);
            document.getElementById("hdnsaveEnable").value = "Load";
        }

    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
   $("span[mand=Yes]").addClass('MandLabelstyle');
          $("span[mand=Yes]").each(function () {
              $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
          });
          $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}
function SelectItemsUnderHeader(controlInstance) {
    Array.prototype.contains = function (k) {
        for (var p in this)
            if (this[p] === k)
                return true;
        return false;
    }

    var target = event.target || event.srcElement;
    var chks = controlInstance.getElementsByTagName("input");
    var checkedHeaders = new Array();
    var targetState;
    if (target.type == 'checkbox') {
        if (target.parentNode.getAttribute("IsHeader") == "true") {
            checkedHeaders.push(target.parentNode.getAttribute("RespectiveHeader"));
            targetState = target.checked;
        }
    }

    for (var i = 0; i < chks.length; i++) {
        if (checkedHeaders.contains(chks[i].parentNode.getAttribute("RespectiveHeader"))) {
            chks[i].checked = targetState;
        }
    }
}
function Testw(chkBox) {
    if (chkBox.checked)
        EnableSaveDiagnosticOrder();
}
function btnFillQuestionSets_Clicking(sender, args) {


    var SaveSubmit = document.getElementById('btnOrderSubmit'); //$find("<%=btnOrderSubmit.ClientID%>");
    if (SaveSubmit.get_visible() === true) {
        alert("Kindly Do Save And Submit Before Filling QuestionSet");
    }
    else {
        var obj = new Array();
        var Result = openModal("frmOrdersQuestionSetsBloodLead.aspx", 515, 630, obj, 'MessageWindow');
    }
}
function Test() {

    window.self.location.assign("www.google.com");
    window.self.location.reload();
}

function Numeric_OnKeyPress(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
        return false;
    else if(charCode == 46)
        return false;

    return true;
}
function cboBillType_SelectedIndexChanging(sender, args) {
    if (eventArgs.get_item().get_text() == "Third Party") {
        $find('dbInsurancePlan').set_enabled(true);
    }
    else {
        $find('dbInsurancePlan').set_enabled(false);
    }
}

var editIndex = -1;
function grdOrders_OnCommand(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (args != undefined)
        editIndex = args.get_commandArgument();

    if (args != undefined) {
        var CommanArgs = args.get_commandName();
        document.getElementById("hdnCommandField").value = CommanArgs;
        var masterTable = $find("grdOrders").get_masterTableView();
        if (CommanArgs == "FillQuestionSet") {
            var rowItems = masterTable.get_dataItems()[args.get_commandArgument()];
            var CellContent = rowItems.get_element().cells[18].innerText;
            var SubmitID = rowItems.get_element().cells[15].innerText;
            if (CellContent !== undefined && CellContent !== "") {
                var IsQuestionSetRequired = false;
                if (CellContent.indexOf("AOE") >= 0) {
                    IsQuestionSetRequired = true;
                    var result = window.showModalDialog("frmOrderQuestionSetAOE.aspx?OrderSubmitID=" + SubmitID, null, "center:yes;resizable:no;dialogHeight:400px;dialogWidth:600px");
                }
                if (CellContent.indexOf("AFP") >= 0) {
                    IsQuestionSetRequired = true;
                    var result = window.showModalDialog("frmOrdersQuestionSetsAFP.aspx?OrderSubmitID=" + SubmitID, null, "center:yes;resizable:no;dialogHeight:400px;dialogWidth:600px");
                }
                if (CellContent.indexOf("CYT") >= 0) {
                    IsQuestionSetRequired = true;
                    var result = window.showModalDialog("frmOrdersQuestionSetsCytology.aspx?OrderSubmitID=" + SubmitID, null, "center:yes;resizable:no;dialogHeight:400px;dialogWidth:600px");
                }
                if (CellContent.indexOf("ZCY") >= 0) {
                    IsQuestionSetRequired = true;

                    var result = window.showModalDialog("frmOrdersQuestionSetsCytology.aspx?OrderSubmitID=" + SubmitID, null, "center:yes;resizable:no;dialogHeight:400px;dialogWidth:600px");
                }
                if (IsQuestionSetRequired == false) {
                    alert("Question Set is not required for this order");
                }
                args.set_cancel(true);
            }


        }
        else if (CommanArgs == "View") {
            var rowItems = masterTable.get_dataItems()[args.get_commandArgument()];
            var CellContent = rowItems.get_element().cells[18].innerText;
            if (rowItems.get_element().cells[17].innerHTML.includes('Resources/Down_Disabled.bmp') == true) {
                args.set_cancel(true);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
            else {
                args.set_cancel(false);
            }
        }
        else if (CommanArgs == "Del") {
            if (DisplayErrorMessage("230105")) {
                args.set_cancel(false);
            }
            else {
                args.set_cancel(true);
            }
        }
    }

    else {
        if (DisplayErrorMessage("230105")) {
            document.getElementById("hdnRowIndex").value = editIndex;
            editIndex = -1;
            document.getElementById("btnDelete").click();
            //return true;
        }
        else
             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    }

}
function OnloadOrderList() {
    if (document.getElementById("hdnCommandField").value != "View") {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        document.getElementById("hdnCommandField").value = "";
    }
}
function btnPlan_Clicking(sender, args) {
    var obj = new Array();
    var ResultSpecialityDiagonsis = openModal("frmAddorUpdatePlan.aspx", 450, 700, obj);
}


function SetReadOnly() {
    if (typeof (Telerik) != "undefined" && typeof (Telerik.Web.UI.RadInputControl) != "undefined") {
        Telerik.Web.UI.RadInputControl.prototype.updateCssClass_Old = Telerik.Web.UI.RadInputControl.prototype.updateCssClass;
        Telerik.Web.UI.RadInputControl.prototype.updateCssClass = function () {
            this._hovered = false;
            this.updateCssClass_Old();
        }
    }
}

function ExamClose(radWindow, args) {

    var obtn = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
    if (obtn != null) {
        if (obtn._enabled) {
            var butt = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
            butt.click();
            //var IsClearAll = DisplayErrorMessage('280014');
            //if (IsClearAll) {
            //    var butt = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
            //    butt.click();
               
            //}
            //else {
            //    var Result = new Object();
            //    var WindowName = $find('RadWindowImportResult');
            //    WindowName.Close();
              
            //}
        }
    }
}

function OnClientBeforeClose(radWindow, args) {


    var obtn = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
    if (obtn != null) {
        if (obtn._enabled) {
            var butt = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
            butt.click();
            //var IsClearAll = DisplayErrorMessage('280014');
            //if (IsClearAll) {
            //    var butt = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
            //    butt.click();
            //}
            //else {
            //    var Result = new Object();
            //    var oWnd = GetRadWindow();
            //    oWnd.close(Result);
            //}
        }

    }

  

}


function OnOpendPatientChartClick(radWindow, args) {

    var Result = new Object();
    Result.IsEnabled = "false";

    var arg = args.get_argument();
    if (arg) {
        if (arg == "Yes") {

            var butt = radWindow.get_contentFrame().contentWindow.$find('btnUpload');
            butt.click();
            var WindowName = $find('RadWindowImportResult');
            WindowName.remove_beforeClose(OnClientBeforeClose);
            WindowName.close();
        }
        else if (arg == "No") {
            var WindowName = $find('RadWindowImportResult');
            WindowName.remove_beforeClose(OnClientBeforeClose);
            WindowName.close();
           
        }
        else {
            // return true;
        }
    }

}


function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function OpenPlanCpt(insuranceId, sTempCpts, Home_Phone_No, SaveOrUpdate, ulMyHumanID)
{
    PlanCptClose();
}

function PlanCptClose() {
    var now = new Date();
    var utc = now.toUTCString();
    var screen_name;
    document.getElementById("hdnLocalTime").value = utc;
    document.getElementById("btninvisible").click();

    var splitvalues = null;
    if (window.parent.theForm != undefined && window.parent.theForm.hdnTabClick != undefined && window.parent.theForm.hdnTabClick != null)
        splitvalues = window.parent.theForm.hdnTabClick.value.split('$#$');
    else if (window.parent.parent.theForm != undefined && window.parent.parent.theForm.ctl00_C5POBody_hdnTabClick != undefined && window.parent.parent.theForm.ctl00_C5POBody_hdnTabClick != null)
        splitvalues = window.parent.parent.theForm.ctl00_C5POBody_hdnTabClick.value.split('$#$');
    else if (top.window.frames[0].frameElement.contentDocument != undefined && top.window.frames[0].frameElement.contentDocument != null && top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick') != undefined && top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick') != null)
        splitvalues = top.window.frames[0].frameElement.contentDocument.getElementById('hdnTabClick').value.split('$#$');
    else if (window.parent.parent.parent.parent.theForm != undefined && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null)
        splitvalues = window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value;
        // var splitvalues = window.parent.parent.theForm.hdnTabClick.value.split('$#$');

    if (splitvalues!=null && splitvalues.length == 3 && splitvalues[2] == "Node")
        screen_name = 'PatientChartTreeViewNodeClick';
    SavedSuccessfully_NowProceed(screen_name);
}
function OpenPatInsurancePolicy(HumanId, sMyPayerName, ulPayerID, ulCarrierID, PatInsId, sRelationShip, sInsType, sPreferred_Reading_Physician_ID) {
    var obj = new Array();
    obj.push("HumanId=" + HumanId);
    obj.push("sMyPayerName=" + sMyPayerName);
    obj.push("ulPayerID=" + ulPayerID);
    obj.push("ulCarrierID=" + ulCarrierID);
    obj.push("PatInsId=" + PatInsId);
    obj.push("sRelationShip=" + sRelationShip);
    obj.push("sInsType=" + sInsType);
    obj.push("sPreferred_Reading_Physician_ID=" + sPreferred_Reading_Physician_ID);
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal("frmPatientInsurancePolicyMaintenance.aspx", 630, 850, obj, "FormRequiredWindow"); }, 0);
}

function FormAvailable_CheckedChanged(checkBox) {
    var grid = $find("grdCPTAndRequiredForms");
    var masterTable = grid.get_masterTableView();

    for (var i = 0; i < masterTable.get_dataItems().length; i++) {
        var gridItemElement = masterTable.get_dataItems()[i].findElement("FormAvailableForpatient");
        gridItemElement.checked = false;
    }

    checkBox.checked = true;
    enableSave(false);
}

function OpenPlanCptUpdate(Home_Phone_No, SaveOrUpdate, sTempCpts, ulMyHumanID, OrderSubmitID, sPreferred_Reading_Physician_ID) {
    PlanCptClose();
}

function RadWindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}

function SaveValidation() {
    if (document.getElementById(GetClientId("txtPhoneNumber")).value == "(___) ___-____") {
        DisplayErrorMessage('230146');
        document.getElementById(GetClientId("txtPhoneNumber")).focus();
        return false;
    }
    if (PhNoValid(GetClientId("txtPhoneNumber")) == false && document.getElementById(GetClientId("txtPhoneNumber")).value != "(___) ___-____") {
        DisplayErrorMessage('230147');
        document.getElementById(GetClientId("txtPhoneNumber")).focus();
        return false;
    }
}
function PhNoValid(sphno) {
    var s = document.getElementById(sphno).value;
    sReplace = s.replace(/_/gi, "");
    if (sReplace.length < 13) {
        return false;
    }
    else {
        return true;
    }



}

function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
}
function openDiagnosisFormViews(CurrentProcess, Submit_Id, hdnMoveToMA) {
    var obj = new Array();
    obj.push("type=Result Upload");
    obj.push("CurrentProcess=" + CurrentProcess);
    obj.push("OrderSubmit_ID=" + Submit_Id);
    obj.push("hdnMoveToMA=" + hdnMoveToMA);
    obj.push("IsCmg=IsCmg");

    var Result = openModal("frmExamPhotos.aspx", 370, 800, obj, 'MessageWindow');
    
}
function openDiagnosisFormView(CurrentProcess, Submit_Id) {
    var obj = new Array();
    obj.push("type=Result Upload");
    obj.push("CurrentProcess=" + CurrentProcess);
    obj.push("OrderSubmit_ID=" + Submit_Id);
    obj.push("IsCmg=IsCmg");

    var Result = openModal("frmExamPhotos.aspx", 400, 800, obj, 'MessageWindow');
    
}
function ChklstAssessmentEnable() {
    if (document.getElementById('chkSelectALLICD').disabled == false) {
        document.getElementById('btnOrderSubmit').disabled = false;//$find('btnOrderSubmit').set_enabled(true);
        EnableSaveDiagnosticOrder();
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true; // $find('btnOrderSubmit').set_enabled(false);
    }

}
//added by balaji.TJ
function ChklstFrequentlyEnable() {
    if (document.getElementById('chklstFrequentlyUsedProcedures').disabled == false) {
        document.getElementById('btnOrderSubmit').disabled = false; //$find('btnOrderSubmit').set_enabled(true);
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true; //$find('btnOrderSubmit').set_enabled(false);
    }

    var selectedLab = document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text;//cboLab.get_selectedItem().get_text();
    if (selectedLab != null && selectedLab == "CMG Anc.-In House") {
        document.getElementById('btnImportresult').disabled = false; //$find("btnImportresult").set_enabled(true);
    }

}
function OnClientCloseLabLocation(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var SelectedLab = arg.selectedLabText;
        var elementRef = document.getElementById("txtLocation");
        if (SelectedLab != undefined) {
            elementRef.value = SelectedLab;
            __doPostBack('btnSelectLocation');
        }
    }
    oWindow.remove_close(OnClientCloseLabLocation);
}
function openLink(urls) {
    var temp = urls.split(';');
    var IsLonic = 0;
    if (temp.length > 1) {
        for (var i = 0; i < temp.length; i++) {
            if (temp[i] != "") {
                window.open("http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=" + temp[i] + "&informationRecipient.languageCode.c = en", "_blank");
            }
            else if (i != (temp.length - 1)) {
                window.open("http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=&informationRecipient.languageCode.c = en", "_blank");
            }
        }
    }
    else {
        window.open("http://apps.nlm.nih.gov/medlineplus/services/mpconnect.cfm?mainSearchCriteria.v.cs=2.16.840.1.113883.6.1&mainSearchCriteria.v.c=&informationRecipient.languageCode.c = en", "_blank");
    }

}

function btnInsurancePolicyClick() {
    if (save.disabled == false) {
        return true;
    }
    else
        return false;
}

function btnImportresult_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
}


function btnPrintRequsition_Clicked(sender, args) {
    var btnSaved;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    btnSaved = document.getElementById('btnOrderSubmit');
    
    if (btnSaved.disabled == true) {
        return true;
    }
    else {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        if (btnSaved.disabled == false) {
            DisplayErrorMessage('230145');
            return false;
        }
    }
}


function btnGenerateABN_Clicked(sender, args) {

    var btnSaved;
    btnSaved = document.getElementById('btnOrderSubmit');
    if (btnSaved.disabled == true) {
        return true;
    }
    else
        if (btnSaved.disabled == false) {
            DisplayErrorMessage('230145');
            return false;
        }
}
function btnClearAll_Clicked(sender, args) {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
     if (JSON.parse(sessionStorage.getItem("EncCancel")) == true)
        sessionStorage.setItem("EncCancel", "false");
    var btnSaved;
    btnSaved = document.getElementById('btnOrderSubmit');
    if (btnSaved.disabled == false){
        if (document.getElementById('btnClearAll').value == "Clear All") {
            if (DisplayErrorMessage('230124') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                return false;
            }
        }
        else if (document.getElementById('btnClearAll').value == "Cancel") {
            if (DisplayErrorMessage('230151') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                 {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                return false;
            }
        }
    }
    else
    {
        if (document.getElementById('btnClearAll').value == "Cancel") {
            if (DisplayErrorMessage('230124') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        if (document.getElementById('btnClearAll').value == "Clear All") {
            if (DisplayErrorMessage('230124') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
       
}
function OnClientClosePlan(sender, eventArgs) {

    var btnordersubmit = document.getElementById('btnOrderSubmit')//$find("btnOrderSubmit");
    if (btnordersubmit.disabled == true) {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }
}
function txtCity_OnKeyPress(sender, args) {
    document.getElementById('btnSearch').disabled = false; 
    document.getElementById('btnOk').disabled = false; 
    if ((args._keyCode > 64 && args._keyCode < 91) || (args._keyCode > 96 && args._keyCode < 123)) {
        args._cancel = false;
    }
    else {
        args._cancel = true;
    }
    if (args._keyCode == 13) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        __doPostBack('btnSearch');
    }

}
function txtkeyUp() {
    $find('btnSearch').set_enabled(true);
    $find('btnOk').set_enabled(true); 
}
function txtZip_OnKeyPress(sender, args) {
    $find('btnSearch').set_enabled(true);
    $find('btnOk').set_enabled(true);
    if ((args._keyCode > 47 && args._keyCode < 58)) {
        args._cancel = false;
    }
    else {
        args._cancel = true;
    }
    if (args._keyCode == 13) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        __doPostBack('btnSearch');
    }
}
function Copy(data) {
    $find('btnSearch').set_enabled(true);
    $find('btnOk').set_enabled(true);
}
function txtState_OnKeyPress(sender, args) {
    $find('btnSearch').set_enabled(true);
    $find('btnOk').set_enabled(true);
    var txtbox = sender._textBoxElement
    if (txtbox.value.length < 2) {
        if ((args._keyCode > 64 && args._keyCode < 91) || (args._keyCode > 96 && args._keyCode < 123)) {
            args._cancel = false;
        }
        else {
            args._cancel = true;
        }
    }
    else {
        args._cancel = true;
    }
    if (args._keyCode == 13) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        __doPostBack('btnSearch');
    }
}
function txtNPI_OnKeyPress(sender, args) {
    $find('btnSearch').set_enabled(true);
    $find('btnOk').set_enabled(true);
    if ((args._keyCode > 47 && args._keyCode < 58) || (args._keyCode > 64 && args._keyCode < 91) || (args._keyCode > 96 && args._keyCode < 123)) {
        args._cancel = false;
    }
    else {
        args._cancel = true;
    }
    if (args._keyCode == 13) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        __doPostBack('btnSearch');
    }
}
function btnOk_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var grdLocation = $find('grdLabLocations');
    if (grdLocation._selectedIndexes.length == 0) {
        DisplayErrorMessage('230120');
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        return false;
    }
    else {
        return true;
    }
}
function CloseResultPage(oWindow, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true")// && DisplayErrorMessage('1100000') == true)
            args.set_cancel(true);
        else
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }

}

function OpenFillQuestionSet() {
    var oWnd = GetDiagnosticWindow();
    var oManager = oWnd.get_windowManager();
    if (document.getElementById("hdnSelectedBloodLead").value != "" && document.getElementById("hdnSelectedBloodLead").value != undefined) {
        var childWindow = oManager.BrowserWindow.radopen("frmOrdersQuestionSetsBloodLead.aspx", "RadWindowImportResult");
        SetRadWindowProperties(childWindow, 120, 630);
        childWindow.add_close(CloseBloodFillQuestionSet)
        {
            function CloseBloodFillQuestionSet(oWindow, args) {
                OpenFillQuestionSet();
            }
        }
    }
    if (document.getElementById("hdnSelectedAFP").value != "" && document.getElementById("hdnSelectedAFP").value != undefined) {
        var childWindow = oManager.BrowserWindow.radopen("frmOrdersQuestionSetsAFP.aspx", "RadWindowImportResult");
        SetRadWindowProperties(childWindow, 400, 850);
        childWindow.add_close(CloseAFPFillQuestionSet)
        {
            function CloseAFPFillQuestionSet(oWindow, args) {
                OpenFillQuestionSet();
            }
        }
    }
    if (document.getElementById("hdnSelectedCytology").value != "" && document.getElementById("hdnSelectedCytology").value != undefined) {
        var childWindow = oManager.BrowserWindow.radopen("frmOrdersQuestionSetsCytology.aspx", "RadWindowImportResult");
        SetRadWindowProperties(childWindow, 400, 850);
        childWindow.add_close(CloseCytologyFillQuestionSet)
        {
            function CloseCytologyFillQuestionSet(oWindow, args) {
                OpenFillQuestionSet();
            }
        }
    }
    return false;
}
function GetDiagnosticWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    if (oWindow == null) {
        oWindow = $find('RadResultWindow');
    }
    return oWindow;
}
function OpenBloodLead(OrderSubmitID) {
    var oWnd = GetDiagnosticWindow();
    var oManager = oWnd.get_windowManager();
    var childWindow = oManager.BrowserWindow.radopen("frmOrdersQuestionSetsBloodLead.aspx?OrderSubmitID=" + OrderSubmitID, "RadWindowImportResult");
    SetRadWindowProperties(childWindow, 120, 630);
    childWindow.remove_close(OnClientCloseSelectLabLocation);
    childWindow.remove_close(CloseAllProcedures);
    childWindow.remove_close(OnClientCloseDiagnosis);
    childWindow.remove_close(CloseCytology);
    childWindow.remove_close(CloseAFP);
    childWindow.add_close(CloseBloodLead);
    return false;
}
function CloseBloodLead(oWindow, args) {
   
    alert("Close Blood Lead");
    return false;
}
function OpenAFP(OrderSubmitID) {
    var oWnd = GetDiagnosticWindow();
    var oManager = oWnd.get_windowManager();
    var childWindow = oManager.BrowserWindow.radopen("frmOrdersQuestionSetsAFP.aspx?OrderSubmitID=" + OrderSubmitID, "RadWindowImportResult");
    SetRadWindowProperties(childWindow, 740, 1070);
    childWindow.remove_close(OnClientCloseSelectLabLocation);
    childWindow.remove_close(CloseAllProcedures);
    childWindow.remove_close(OnClientCloseDiagnosis);
    childWindow.remove_close(CloseCytology);
    childWindow.remove_close(CloseBloodLead);
    childWindow.add_close(CloseAFP);
    return false;

}
function CloseAFP(oWindow, args) {
  
    alert("Close AFP");
    return false;
}
function OpenCytology(OrderSubmitID) {
    var oWnd = GetDiagnosticWindow();
    var oManager = oWnd.get_windowManager();
    var childWindow = oManager.BrowserWindow.radopen("frmOrdersQuestionSetsCytology.aspx?OrderSubmitID=" + OrderSubmitID, "RadWindowImportResult");
    SetRadWindowProperties(childWindow, 550, 730);
    childWindow.remove_close(OnClientCloseSelectLabLocation);
    childWindow.remove_close(CloseAllProcedures);
    childWindow.remove_close(OnClientCloseDiagnosis);
    childWindow.add_close(CloseCytology);
    return false;
}
function CloseCytology(oWindow, args) {
    
    alert("Close Cytology");
    return false;
}
function BloodLeadClearAll() {
    var BloodLeadType =  $find('cboBloodLeadType')._text;
    var BloodLeadPurpose = $find('cboBloodLeadPurpose')._text;
    if (BloodLeadType != "" || BloodLeadPurpose != "") {
        if (DisplayErrorMessage('9093034')) {
            var CboBloodLeadType = $find('cboBloodLeadType');
            var CboBloodLeadPrupose = $find('cboBloodLeadPurpose');
            CboBloodLeadType.clearSelection();
            CboBloodLeadPrupose.clearSelection();
        }
    }
    return false;
}

function CloseBloodLead() {
    window.close();
}
function pageLoad() {
    $("#dtpCollectionDate").datetimepicker({ closeOnDateSelect: true, format: 'd-M-Y h:i A' }); 
    $("#cstdtpTestDate").datetimepicker({closeOnDateSelect: true,format: 'd-M-Y'});
}

function dtpCollectionDate_OnDateSelected(sender, args) {
    var cboLab = document.getElementById("cboLab"); //$find("cboLab");
    var selectedLab = document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text; //cboLab.get_selectedItem().get_text();
    if (selectedLab == "CMG Anc.-In House") {
        var elementRef = document.getElementById("chklstFrequentlyUsedProcedures");
        var checkBoxArray = elementRef.getElementsByTagName('input');
        for (var i = 0; i < checkBoxArray.length; i++) {
            var checkBoxRef = checkBoxArray[i];
            if (checkBoxRef.checked == true) {
                document.getElementById('btnImportresult').disabled = false; //$find("btnImportresult").set_enabled(true);
                return;
            }
            else {
                document.getElementById('btnImportresult').disabled = true;//$find("btnImportresult").set_enabled(false);
            }
        }

    }
    if (document.getElementById("hdnsaveEnable").value == "Load") {
        document.getElementById('btnOrderSubmit').disabled = true;
        document.getElementById("hdnsaveEnable").value = '';
    }
    else
        document.getElementById('btnOrderSubmit').disabled = false; 
}
function CloseQuestionSetWindow() {
    document.getElementById("btnDisableLoad").click();
}

function RefreshCloseResultPage() {
    if (document.getElementById("btnrefreshgrid") != null)
        document.getElementById("btnrefreshgrid").click();
}
function txtAddress_OnKeyPress(sender, args) {
    document.getElementById("btnSearch").disabled = false; 
    document.getElementById("btnOk").disabled = false; 
    if (args._keyCode == 13) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        __doPostBack('btnSearch');
    }
}


function WindowCloseDiagnostics() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}
function txtQuantity_OnKeyPress(sender, args) {
    EnableSaveDiagnosticOrder();
}

function CloseQuestionSet(sender, args) {
    var Save = document.getElementById("btnQuestionOk")//$find('btnQuestionOk');
    if (Save.disabled == false) {
        if (document.getElementById("hdnMessageType").value == "") {
            document.getElementById("hdnMessageType").value == "Yes";
            document.getElementById('hdnYesClick').value = "Yes";
            document.getElementById('btnQuestionOk').click();
            document.getElementById('hdnMessageType').value = "";
            self.close();
            //DisplayErrorMessage('9093041');
            //args.set_cancel(true);
           
        }
        //else if (document.getElementById("hdnMessageType").value == "Yes") {
        //    document.getElementById('hdnYesClick').value = "Yes";
        //    document.getElementById('btnQuestionOk').click();
        //    document.getElementById('hdnMessageType').value = "";
        //    self.close();
        //}
        //else if (document.getElementById("hdnMessageType").value == "No") {
        //    document.getElementById("hdnMessageType").value = "";
        //    document.getElementById("btnQuestionOk").disabled = true; 
        //    self.close();
        //}
        //else if (document.getElementById("hdnMessageType").value == "Cancel") {
        //    document.getElementById("hdnMessageType").value = "";
        //}
    }
    else {
        self.close();
    }
}

function GetUTCTimeNew() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById("hdnLocalTime").value = utc;
    document.getElementById("hdnYesClick").value = "Yes";
}

function EnableWaitCursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (document.getElementById('btnOrderSubmit') != undefined) {
        document.getElementById('btnOrderSubmit').disabled = false;
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true;
    }
    __doPostBack('chkSelectALLICD');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
}


function EnableWaitCursorcbolab() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    if (document.getElementById('btnOrderSubmit') != undefined) {
        document.getElementById('btnOrderSubmit').disabled = false;
    }
    else {
        document.getElementById('btnOrderSubmit').disabled = true;
    }
    var cbolab = document.getElementById("cboLab");
    __doPostBack('cboLab', cbolab);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    $("label").addClass('spanstyle');
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}

function WaitCursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}
function SavedSuccessfully() {
}

function chklstFrequentlyUsedProcedures_Changed() {
    if (document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text == "CMG Anc.-In House") {
        if ($("#chklstFrequentlyUsedProcedures input:checked").length > 0)
            document.getElementById("btnImportresult").disabled = false; //$find("btnImportresult").set_enabled(true);
        else
            document.getElementById("btnImportresult").disabled = true; //$find("btnImportresult").set_enabled(false);
    }
    if (document.getElementById("btnOrderSubmit") != undefined)
        document.getElementById("btnOrderSubmit").disabled = false;
    if (document.getElementById("txtQuantity").value != "") {
        if (document.getElementById("lblUnits").innerText.indexOf("*") != -1) {
            var lblunits = document.getElementById("lblUnits");
            //CAP-1501
            lblunits.innerHTML += "*";
            $(lblunits).html($(lblunits).html().replace("*", "<span class='manredforstar'>*</span>"));
            $('#lblUnits').removeClass('spanstyle');
            $('#lblUnits').addClass('MandLabelstyle');
        }
        else {
            document.getElementById("lblUnits").innerText = "Units";
            document.getElementById("lblUnits").innerHTML = "Units";
            document.getElementById("lblUnits").style.color = "Black";
            $('#lblUnits').removeClass('MandLabelstyle');
            $('#lblUnits').addClass('spanstyle');
        }
    }
}

function LabOrder_SavedSuccessfully() {
   
    DisplayErrorMessage('230150');
    RefreshNotification('Orders');
    //jira cap-544
    document.getElementById('lblCenterName').className = "spanstyle";
    document.getElementById("lblCenterName").innerHTML = "Center Name";

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    Order_AfterAutoSave();
    checksdates();
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}


function chkStat_CheckedChanged() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    var now = new Date();
    var utc = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utc;
    document.getElementById('cstdtpTestDate').disabled = document.getElementById('chkStat').checked; //$find('cstdtpTestDate').set_enabled(!(document.getElementById('chkStat').checked));
    if (document.getElementById('chkStat').checked) {
        document.getElementById('cstdtpTestDate').value = new Date(document.getElementById("hdnLocalTime").value).format('dd-MMM-yyyy');
        $('#cstdtpTestDate').addClass('nonEditabletxtbox');
        $('#cstdtpTestDate').removeClass('Editabletxtbox');
    }
    else
    {
        $('#cstdtpTestDate').removeClass('nonEditabletxtbox');
        $('#cstdtpTestDate').addClass('Editabletxtbox');
    }
    if (document.getElementById('chkStat').checked == true) {
        document.getElementById('btnOrderSubmit').disabled = false;
    }
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
function btnSelectLocations(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var cboLab = document.getElementById("cboLab");
    var objSelectLabLocation = new Array();
    objSelectLabLocation.push("LabName=" + cboLab.options[cboLab.selectedIndex].text);//cboLab.get_selectedItem().get_text()
    objSelectLabLocation.push("LabID=" + cboLab.options[cboLab.selectedIndex].value);
    var SelectLabLocationResult = openModal("frmSelectLabLocation.aspx", 590, 720, objSelectLabLocation, 'MessageWindow');
    var WindowName = $find('MessageWindow');
    WindowName.add_close(OnClientCloseLabLocation);
    return false
}
function checksdates() {
    if (document.getElementById('cstdtpTestDate') != null) {
        if (document.getElementById("hdnsaveEnable").value != "Load") {
            setTimeout(function () { document.getElementById('cstdtpTestDate').value = new Date(document.getElementById("hdnLocalTime").value).format('dd-MMM-yyyy'); }, 0);
            document.getElementById("hdnsaveEnable").value = "Load";
        }
    }
}
function btnSearchClient(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
}

function RadioClick(radioButton) {
    document.getElementById("hdnpaper").value = radioButton.id;
}


function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    oTbl.attr("data-item-original-width", oTbl.width());
    oTbl.find('thead tr td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).attr("data-item-original-width", $(this).width());
    });
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    newTbl.width(newTbl.attr('data-item-original-width'));
    newTbl.find('thead tr td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    oTbl.width(oTbl.attr('data-item-original-width'));
    oTbl.find('tbody tr:eq(0) td').each(function () {
        $(this).width($(this).attr("data-item-original-width"));
    });
    if (tblAsJQueryObject[0] != undefined) {
        if (tblAsJQueryObject[0].parentElement.parentElement.id == "GeneralQTable") {
            $("#ScrollIDGeneral").css('height', '');
            $("#ScrollIDGeneral").css('overflow-y', '');
        }
        else {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
        }
    }
}
//BugID:53431
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnSave')[0].disabled = true;
}
function warningmethod() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
function OpenPrintPDF(FaxSubject) {
    if(FaxSubject!="")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);

    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Orders";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "685px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('hdnSelectedItem').value + "&Location=DYNAMIC";

}
function chkUrgent_CheckedChanged()
{
    EnableSaveDiagnosticOrder();
}
function warningmethodCbolab(isDefaultLab) {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    //CAP-1152
    if (isDefaultLab == "False" && $('#hdnCMGAncillarySaveOrder').val() == 'false') {
        EnableSaveDiagnosticOrder();
    }
    if ($('#hdnCMGAncillarySaveOrder').val() == 'true') {
        $('#hdnCMGAncillarySaveOrder').val('false');
        document.getElementById('btnOrderSubmit').disabled = true;
    }
}