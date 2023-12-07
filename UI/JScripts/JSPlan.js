var FollowupRefList = [];
var ResonNotPerformedRefList = [];

function OpenPopup(KeyWord) {
    var focused = KeyWord;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}
function EnableSave() {
    if (document.getElementById('btnPrint').disabled == true) {
        return;
    }
    if (document.getElementById("radbtnCorrection") != undefined && document.getElementById("radbtnCorrection").checked == true && document.getElementById("radbtnCorrection").disabled == false) {
        if (document.getElementById('btnSave') != null)
            document.getElementById('btnSave').disabled = true;
        if (document.getElementById('btnMovetoPhyAsst') != null)
            document.getElementById('btnMovetoPhyAsst').disabled = false;
        if (document.getElementById('chkElectronicDeclaration') != null)
            document.getElementById("chkElectronicDeclaration").disabled = true;
        $('#txtCorrectionToPlan').removeClass('nonEditabletxtbox');
        $('#txtCorrectionToPlan').addClass('Editabletxtbox');

    }
    else if ((document.getElementById("radbtnAgreewithChanges") != undefined && document.getElementById("radbtnAgreewithChanges").checked == true && document.getElementById("radbtnAgreewithChanges").disabled == false) || (document.getElementById("radbtnAgreePlan") != undefined && document.getElementById("radbtnAgreePlan").checked == true && document.getElementById("radbtnAgreePlan").disabled == false)) {
        if (document.getElementById("radbtnAgreewithChanges") != undefined && document.getElementById("radbtnAgreewithChanges").checked == true && document.getElementById("radbtnAgreewithChanges").disabled == false) {
            $('#txtAddendumToPlan').removeClass('nonEditabletxtbox');
            $('#txtAddendumToPlan').addClass('Editabletxtbox');
            $('#txtCorrectionToPlan').removeClass('Editabletxtbox');
            $('#txtCorrectionToPlan').addClass('nonEditabletxtbox');
        }
        if (document.getElementById('btnSave') != null)
            document.getElementById('btnSave').disabled = false;
        if (document.getElementById('btnMovetoPhyAsst') != null)
            document.getElementById('btnMovetoPhyAsst').disabled = true;
        if (document.getElementById('chkElectronicDeclaration') != null)
            document.getElementById("chkElectronicDeclaration").disabled = false;
    } else {
        if (document.getElementById('btnSave') != null)
            document.getElementById('btnSave').disabled = false;
    }

    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;


}
function OpenPDFStatic(fileNotFound, screen, DownloadDoc, FaxSubject) {
    if (FaxSubject != "")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
        //Jira CAP-1011
    //if (document.getElementById('hdnSelectedItem').value != "") {
    //    var obj = new Array();
    //    obj.push("SI=" + document.getElementById('hdnSelectedItem').value);
    //    obj.push("Location=" + "STATIC");

    //    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 800, 1000, obj); }, 0);
    //}
    if (screen != null && screen != "") {
        openProgress(screen);
    }

    if (DownloadDoc != null && DownloadDoc != "") {
        setTimeout(function () {
            var sPath = ""
            sPath = "frmWellnessNotes.aspx?PatientDocuments=Patient_Documents&CheckedDocumnts=" + DownloadDoc;;
            $(top.window.document).find("#PlanModal").modal({ backdrop: "static", keyboard: false }, 'show');
            $(top.window.document).find("#ProcessiFrame")[0].contentDocument.location.href = sPath;
            $(top.window.document).find("#PlanModal").modal('hide');
            return false;
        }, 0);
    }
}


function SelectedIndexChanged(sender, eventArgs) {
    var txtBox = document.getElementById('txtSpellCheck').value;
    var txtValue = eventArgs.get_item()._element.innerText;
    if (txtBox == '' && txtbox.indexOf(txtValue) == -1) {
        var txtbox = txtValue;
        document.getElementById('txtSpellCheck').value = txtbox;
    } else if (txtBox.indexOf(txtValue) == -1) {
        var txtbox = txtBox + "\n*" + txtValue;
        document.getElementById('txtSpellCheck').value = txtbox;
    }
}





function wellnessnote() {
    var childWindow = $find('MsgWindow1').BrowserWindow.radopen("frmWellnessNotes.aspx?formName=WellnessNotes", "WellnessWindow");
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(850, 680);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}


function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear();
    utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var utcnew = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utcnew;
}



function OpenPrintPDF(fileNotFound) {
    if (fileNotFound != "") {
        DisplayErrorMessageList('110806', fileNotFound);
    }
    var childWindow = $find('MessageWindow');
    var obj = new Array();
    obj.push("SI=" + document.getElementById('hdnSelectedItem').value);
    obj.push("Location=" + "STATIC");
    setTimeout(function () {
        $find('MessageWindow').BrowserWindow.openModal('frmPrintPDF.aspx', 800, 1000, obj, 'MessageWindow');
    }, 0);
}



function Autosave() {
    DisplayErrorMessage('600001');
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    localStorage.setItem("bSave", "true");
}




function OpenClinicalSmry() {
    var obj = new Array();
    if (document.getElementById('hdnXmlPath').value != null) {
        var filelocation = document.getElementById('hdnXmlPath').value; // To open the generated XML in Human readable format
        window.open(filelocation, "CDA Human Readable", "", "")
    }
}


/*Html page functions*/
function PlanNew_Load() {
    $("#divPlan").load("GeneralPlan.html", LoadDocuments);
}
var physician_Name;
var role;
var currentprocess;
var DOS = "";
function LoadDocuments() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    var rows = $("#tblPlan tbody tr");
    var QuerystringValues = localStorage.getItem('QueryStr').split('&');//BugID:47526
    physician_Name = QuerystringValues[1];
    role = QuerystringValues[2];
    role = decodeURIComponent(role);
    currentprocess = QuerystringValues[3];
    if (QuerystringValues[4] != null && QuerystringValues[4] != undefined) {
        if (QuerystringValues[4].split('-').length > 1)
            DOS = QuerystringValues[4].split('-')[0];
        else
            DOS = QuerystringValues[4];
    }
    localStorage.setItem('DOS_Plan', DOS);

    for (var i = 0; i < rows.length; i++) {
        if ($(rows[i]).attr("data-physician-ids") != undefined) {
            if ($(rows[i]).attr("data-physician-ids").indexOf(physician_Name) == -1)
                rows[i].style.display = "none";
        }
    }


    // bug Id:57266

    $.get("ConfigXML/PhysicianFacilityMapping.xml", {}, function (xml) {
        $("PhyList", xml).each(function (i) {
            $(this).find("Facility").each(function (l) {
                if ($(this).attr("name").toUpperCase().indexOf("SURGERY-") >= 0) {
                    $(this).find('Physician').each(function (k) {
                        if ($(this).attr('username') == physician_Name) {

                            $('#chkSurgeryDeclaration').attr("disabled", false);
                        }

                        var QuerystringValues = localStorage.getItem('QueryStr').split('&');//BugID:47526
                        role = QuerystringValues[2];

                        if (role.toUpperCase() != 'PHYSICIAN') {
                            $('#chkSurgeryDeclaration').hide();
                            $('#ProceedwithSurgeryasPlanned').hide();
                        }
                    });
                }
            });
        });
    });

    //CAP-285 - null handling for current process variable
    if ((currentprocess ?? "").toUpperCase() != "SCRIBE_PROCESS" && (currentprocess ?? "").toUpperCase() != "AKIDO_SCRIBE_PROCESS" && (currentprocess ?? "").toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && (currentprocess ?? "").toUpperCase() != "SCRIBE_CORRECTION" && (currentprocess ?? "").toUpperCase() != "DICTATION_REVIEW" && (currentprocess ?? "").toUpperCase() != "CODER_REVIEW_CORRECTION" && (currentprocess ?? "").toUpperCase() != "PROVIDER_PROCESS" && (currentprocess ?? "").toUpperCase() != "TECHNICIAN_PROCESS" && (currentprocess ?? "").toUpperCase() != "PROVIDER_REVIEW_CORRECTION") {//CMG Ancilliary
        debugger;
        $('#btnPrint')[0].disabled = true;
        $('#btnClearall')[0].disabled = true;
        $('#btnCopyPreviousEncounter')[0].disabled = true;
        $('#pnlDocumentDetails')[0].disabled = true;
        $('#cboRelationship')[0].disabled = true;
        $('#txtGivento')[0].disabled = true;
        $('#cboIsDocumentGiven')[0].disabled = true;
        $('#btnCopyPreviousEncounter')[0].disabled = true;
        $('#pnlElectronicSignature ').find(':input').prop('disabled', true);
        $('#pnlSelectDocuments ').find(':input').prop('disabled', true);
        $('#pnlFollowup ').find(':input').prop('disabled', true);
        $('#txtPlan')[0].disabled = true;
        $("a").attr('disabled', true);
        $("a").attr('onclick', false);
        $("a").removeClass('pbDropdownBackground')
        $("a").addClass('pbDropdownBackgrounddisable')
        $("#txtNotes")[0].disabled = true;
        $('textarea').disabled = true;
        $("#txtReturnIn")[0].disabled = true;
        $("#txtRetrunWeeks")[0].disabled = true;
        $("#txtRetrunDays")[0].disabled = true;
        $("#chkPRN")[0].disabled = true;
        $("#chkAfterStudies")[0].disabled = true;
    }

    if ((currentprocess ?? "").toUpperCase() == "SCRIBE_PROCESS" || (currentprocess ?? "").toUpperCase() == "AKIDO_SCRIBE_PROCESS" || (currentprocess ?? "").toUpperCase() == "SCRIBE_CORRECTION" || (currentprocess ?? "").toUpperCase() == "SCRIBE_REVIEW_CORRECTION")
        $('#pnlElectronicSignature ').find(':input').prop('disabled', true);
    $('#btnSave')[0].disabled = true;
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    localStorage.setItem("bSave", "true");
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/LoadPlan",
        data: '',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var Plan = objdata.Plan;
            var PlanOthers = $.parseJSON(objdata.PlanOthers);
            var Doc = objdata.data;
            var EncData = objdata.EncRec;
            var lblchkElectronicDeclaration = objdata.signtxt;
            var ExstItems = objdata.objItems;
            var defualt_Value = objdata.defualt_Value;

            if (EncData != null) {
                FillEncDataInGenealPLan(EncData);
            }
            if (Doc.length != 0 && Doc != undefined) {
                for (var i = 0; i < Doc.length; i++) {
                    if (Doc[i].Relationship != "") {
                        if (Doc[i].Relationship == "Self") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 1;
                            $('#txtGivento')[0].value = Doc[i].Given_To;
                            $('#txtGivento')[0].readOnly = true;
                            $('#txtGivento')[0].disabled = true;
                            $('#txtGivento')[0].style.backgroundColor = "#ebebe4";
                        }
                        else if (Doc[i].Relationship == "Spouse") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 2;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Father") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 3;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Mother") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 4;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Brother") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 5;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Sister") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 6;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Son") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 7;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Daughter") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 8;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Aunt") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 9;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Uncle") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 10;
                            $('#txtGivento')[0].value = Doc[i].Given_To;
                        }
                        else if (Doc[i].Relationship == "Grand Father") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 11;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Grand Mother") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 12;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Other") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 13;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Nephew") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 14;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Niece") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 15;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Grand Son") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 16;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }

                        else if (Doc[i].Relationship == "Grand Daughter") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 17;
                            $('#txtGivento')[0].value = Doc[i].Given_To;

                        }
                        else if (Doc[i].Relationship == "Friend") {
                            $('#cboRelationship')[0].disabled = false;
                            $('#cboRelationship')[0].selectedIndex = 18;
                            $('#txtGivento')[0].value = Doc[i].Given_To;
                        }
                    }
                    else {
                        $('#txtGivento')[0].readOnly = true;
                        $('#txtGivento')[0].disabled = true;
                    }
                }

                var rows = $("#tblPlan tbody tr");
                for (var i = 0; i < Doc.length; i++) {
                    for (var j = 0; j < rows.length; j++) {
                        if (($(rows[j])[0].style.display) != "none") {
                            if ($(rows[j])[0].childNodes[1] != undefined) {
                                if ($(rows[j])[0].childNodes[1].textContent.trim() == Doc[i].Document_Type.trim()) {
                                    if ($(rows[j])[0].childNodes[1].childNodes[0] != undefined)
                                        $(rows[j])[0].childNodes[1].childNodes[0].checked = true;
                                }
                            }
                        }
                    }
                }

            }
            else {
                $('#pnlDocumentDetails')[0].disabled = true;
                $('#cboRelationship')[0].disabled = true;
                $('#cboRelationship')[0].style.backgroundColor = "#ebebe4";
                $('#cboRelationship')[0].selectedIndex = 0;
                $('#txtGivento')[0].value = "";
                $('#txtGivento')[0].disabled = true;
                $('#cboIsDocumentGiven')[0].disabled = true;
                $('#cboIsDocumentGiven')[0].selectedIndex = 0;
                $('#cboIsDocumentGiven')[0].style.backgroundColor = "#ebebe4";
            }
            var content = [];
            for (var i = 0; i < PlanOthers.length; i++) {
                if (content.indexOf(PlanOthers[i].Plan_Type) == -1)
                    content.push(PlanOthers[i].Plan_Type);
            }
            for (var j = 0; j < content.length; j++) {
                for (var k = 0; k < PlanOthers.length; k++) {
                    if (k == 0) {
                        $("#tblType").append('<tr class="title"><th class="rowppty"  style="background-color: rgb(235, 235, 228);"  colspan="2" style="height: 2%;">' + content[j] + '</th><th style="width:1%;display:none;"></th></tr>')
                    }
                    if (PlanOthers[k].Plan_Type == content[j]) {
                        var icd = PlanOthers[k].Plan.split("<br />").join("\n").indexOf("(ICD");
                        var ICDstring, icdend, datasource = '';
                        var color = "color:black;";
                        if (PlanOthers[k].Plan_Ref.toUpperCase() == "OTHER") {
                            color = "color:#6504d0!important;"
                        }
                        if (icd != -1) {
                            ICDstring = PlanOthers[k].Plan.split("<br />").join("\n").substring(icd);
                            if (PlanOthers[k].Plan.toUpperCase().indexOf('BMI') != -1) {
                                datasource += "BMI~";
                            }
                            icdend = ICDstring.indexOf(")")
                            datasource += ICDstring.substring(1, icdend).replace(" ", "");
                        }


                        if (PlanOthers[k].Plan_Ref.toUpperCase() == "OTHER" && PlanOthers[k].Plan_For_Plan.split("<br />").join("\n") == "" && PlanOthers[k].Plan.toUpperCase().indexOf('BMI') != -1) {

                            $("#tblType").append("<tr><td  class='rowppty spanstyle' style='word-wrap: break-word;background-color: rgb(235, 235, 228);" + color + "' id ='iditem'" + k + ">" + PlanOthers[k].Plan.split("<br />").join("\n") + "</td><td class='rowppty' style='position:relative !important;' ><div style='height:100%;min-height:45px;'><textarea class='spanstyle' onfocus='focusTab(this,event);' onkeydown='insertTab(this, event);EnableSave();' id ='idplannew" + k + "' data-src='" + datasource.trim() + "' contenteditable='true'  style='width:400px; word-wrap: break-word;height:100%;position: static;display:inline-block;resize: none;' oncontextmenu='return false;'>" + defualt_Value + "</textarea><div class='col-5-btns' style='width: 15px;float:right!important;display:inline-block;position:relative;vertical-align: middle;text-align: center;top: 23%;right: 5% '><a class='fa fa-plus pbDropdownBackground' style='margin-left: -1px;margin-right: -1px;text-decoration:none;   color: #fff; ' id='pbDropdown' align='centre'  font-bold='false' title='Drop down' onclick=callwebplan(this,'" + datasource.trim() + "','idplannew" + k + "')> </a></div></div></td><td style='width:1%;display:none;'>" + PlanOthers[k].Plan_ID + "</td></tr>");

                        }
                        else {
                            $("#tblType").append("<tr><td  class='rowppty spanstyle' style='word-wrap: break-word;background-color: rgb(235, 235, 228);" + color + "' id ='iditem'" + k + ">" + PlanOthers[k].Plan.split("<br />").join("\n") + "</td><td class='rowppty' style='position:relative !important;' ><div style='height:100%;min-height:45px;'><textarea class='spanstyle' onfocus='focusTab(this,event);' onkeydown='insertTab(this, event);EnableSave();' id ='idplannew" + k + "' data-src='" + datasource.trim() + "' contenteditable='true'  style='width:400px; word-wrap: break-word;height:100%;position: static;display:inline-block;resize: none;' oncontextmenu='return false;'>" + PlanOthers[k].Plan_For_Plan.split("<br />").join("\n") + "</textarea><div class='col-5-btns' style='width: 15px;float:right!important;display:inline-block;position:relative;vertical-align: middle;text-align: center;top: 23%;right: 5% '><a class='fa fa-plus pbDropdownBackground' style='margin-left: -1px;margin-right: -1px;text-decoration:none;   ' id='pbDropdown' align='centre'  font-bold='false' title='Drop down' onclick=callwebplan(this,'" + datasource.trim() + "','idplannew" + k + "')> </a></div></div></td><td style='width:1%;display:none;'>" + PlanOthers[k].Plan_ID + "</td></tr>");
                        }

                    }
                }
            }

            $('#txtPlan')[0].value = Plan;
            if (ChkboxDocument_Checked)

                if (currentprocess.toUpperCase() != "SCRIBE_PROCESS" && currentprocess.toUpperCase() != "AKIDO_SCRIBE_PROCESS" && currentprocess.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "SCRIBE_CORRECTION" && currentprocess.toUpperCase() != "DICTATION_REVIEW" && currentprocess.toUpperCase() != "CODER_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "PROVIDER_PROCESS" && currentprocess.toUpperCase() != "TECHNICIAN_PROCESS" && currentprocess.toUpperCase() != "PROVIDER_REVIEW_CORRECTION") {//CMG Ancilliary
                    $("a").attr('disabled', true);
                    $("a").attr('onclick', false);
                    $("a").css('backgroundColor', '#6D7777');
                    $('textarea').attr('disabled', true);
                    var planDivs = $('.editable');
                    for (var k = 0; k < planDivs.length; k++) {
                        planDivs[k].disabled = true;
                    }

                }
            if ($("#idplan0") != undefined && $("#idplan0") != null) {
                $("#idplan0").focus();
            }

            $('#' + "chkElectronicDeclaration" + "+span").html(lblchkElectronicDeclaration);

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
        }
    });

    $("textarea").addClass('spanstyle');
    $("label").addClass('spanstyle');
    $("select").addClass('spanstyle');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $('#tblPlan tr td').addClass('spanstyle');
    $("#pbDropdown").addClass('pbDropdownBackground');
    $("option").addClass('spanstyle');

}
window.addEventListener("contextmenu",
    function (e) {
        e.stopPropagation()
    }, true);
function followupKeypress() {
    event.preventDefault();

}
function InsertStarPlan() {

    if (event.keyCode == 13) {
        event.preventDefault();
        var txtarea = $('#txtPlan')[0];

        if (txtarea != null) {
            var step = parseInt(txtarea.selectionStart);
            var x = txtarea.selectionStart;
            var y = txtarea.selectionEnd;
            x = txtarea.value.substring(0, x);
            y = txtarea.value.substring(y);
            var txt = "\n" + "* ";
            $('#txtPlan')[0].value = x + txt + y;
            // event.srcElement.nextElementSibling.value = x + txt + y;
            txtarea.setSelectionRange(step + 3, step + 3);
            txtarea.focus();
        }
    }
    $('#btnSave')[0].disabled = false;
    EnableSave();
    return true;
}
function DueonClickPlan(obj) {
    return;
}

function ReturnInClickPlan(obj) {
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
    return;
}
function SavePlan() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    if (document.getElementById('pnlElectronicSignature').disabled == false) {
        if (document.getElementById('chkElectronicDeclaration').checked == false) {
            SaveUnsuccessful();
            DisplayErrorMessage('010015');
            localStorage.setItem("bSave", "false");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            AutoSaveUnsuccessful();
            return;
        }
    }

    var arry = new Array();
    var PlanList = function () {
        this.Is_Document_Given = "";
        this.Due_On = " ";
        this.ReturnDays = "";
        this.ReturnWeeks = "";
        this.ReturnMonths = "";
        this.FollowReasonNotes = "";
        this.PFSHVerified = "";
        this.ElectronicDeclrChecked = "";
        this.CheckedDocumnts = "";
        this.Givento = "";
        this.Relationship = "";
        this.TreatmntList = "";
        this.SurgeryDeclrChecked = "";
        this.PRNChecked = "";
        this.AfterStudieschecked = "";
    };
    var objPlanList = new PlanList();

    if ($('#chkElectronicDeclaration')[0].display != "none") {
        if ($('#chkElectronicDeclaration')[0].checked == true) {
            objPlanList.ElectronicDeclrChecked = "true";
        }
        else {
            objPlanList.ElectronicDeclrChecked = "false";
        }
    }

    if ($('#chkSurgeryDeclaration')[0].checked == true) {
        objPlanList.SurgeryDeclrChecked = "true";
    }
    else {
        objPlanList.SurgeryDeclrChecked = "false";
    }


    objPlanList.Is_Document_Given = $('#cboIsDocumentGiven')[0].value;

    if ($('#txtReturnIn')[0].value != "") {
        objPlanList.ReturnMonths = $('#txtReturnIn')[0].value;
    }
    else {
        objPlanList.ReturnMonths = 0;
    }
    if ($('#txtRetrunWeeks')[0].value != "") {
        objPlanList.ReturnWeeks = $('#txtRetrunWeeks')[0].value;
    }
    else {
        objPlanList.ReturnWeeks = 0;
    }
    if ($('#txtRetrunDays')[0].value != "") {
        objPlanList.ReturnDays = $('#txtRetrunDays')[0].value;
    }
    else {
        objPlanList.ReturnDays = 0;
    }
    if ($('#chkPRN')[0].checked == true) {
        objPlanList.PRNChecked = "Y";
    }
    else {
        objPlanList.PRNChecked = "N";
    }
    if ($('#chkAfterStudies')[0].checked == true) {
        objPlanList.AfterStudieschecked = "Y";
    }
    else {
        objPlanList.AfterStudieschecked = "N";
    }
    objPlanList.FollowReasonNotes = $("#txtNotes")[0].value;
    var rows = $("#tblPlan tbody tr");
    for (var i = 0; i < rows.length; i++) {
        if (($(rows[i])[0].style.display) != "none") {
            if ($(rows[i])[0].firstChild.nextSibling.childNodes[0].checked == true) {
                if (objPlanList.CheckedDocumnts == "") {
                    objPlanList.CheckedDocumnts = $(rows[i])[0].firstChild.nextSibling.childNodes[1].textContent;
                }
                else {
                    objPlanList.CheckedDocumnts += '-' + $(rows[i])[0].firstChild.nextSibling.childNodes[1].textContent;
                }
            }
        }
    }
    objPlanList.Givento = $('#txtGivento')[0].value;
    objPlanList.Relationship = $('#cboRelationship')[0].value;
    var rowscount = $("#tblType");
    for (var i = 0; i < rowscount.length; i++) {
        if (($(rows[i])[0].style.display) != "none") {
        }

    }
    var Planlistitems = new Array();
    var Plan = function () {
        this.Id = "";
        this.Plan = "";
        this.Plan_For_Plan = "";
    }
    var datarows = $("#tblType tr");
    var PlanData = "";
    var i = 0;
    // FOR BMI Field Validation on Followup bug id:46851
    for (var rc = 1; rc < datarows.length; rc++) {
        if ($(datarows[rc])[0].className != "title") {

            var planitem = new Plan();
            planitem.Plan = $(datarows[rc])[0].children[0].innerHTML.split("<br>").join("\n");
            planitem.Plan_For_Plan = $(datarows[rc])[0].children[1].children[0].children[0].value;//$(datarows[rc])[0].children[2].val();
            planitem.Id = $(datarows[rc])[0].children[2].innerText;
            Planlistitems[i] = planitem;
            i++;
        }
    }


    if ($('#txtPlan')[0].value != "") {
        PlanData += $('#txtPlan')[0].value + "\r\n";
    }
    objPlanList.TreatmntList = PlanData;
    arry[arry.length++] = objPlanList;
    localStorage.setItem("bSave", "true");
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/SavePlan",
        data: JSON.stringify({
            "data": arry,
            "PlanfromOthers": Planlistitems
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var result = $.parseJSON(data.d);
            var EncRecord = result.EncRecord;
            if (EncRecord != null) {
                FillEncDataInGenealPLan(EncRecord);
            }
            $('#btnSave')[0].disabled = true;
            Autosave();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            localStorage.setItem("bSave", "true");
            RefreshNotification('GeneralPlan');
            AutoSaveSuccessful();
        },
        error: function OnError(xhr) {
            AutoSaveUnsuccessful();
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                //CAP-798 Unexpected end of JSON input
                if (isValidJSON(xhr.responseText)) {
                    var log = JSON.parse(xhr.responseText);
                
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;
                }
                else {
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry.");

                }
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}
function ClearAll(sender, args) {

    if (DisplayErrorMessage('270002') == true) {
        var TextBoxControls = document.getElementsByTagName('INPUT');
        var ComboControls = document.getElementsByTagName('SELECT');
        var testarwe = document.getElementsByTagName('TEXTAREA');

        var str = null;
        document.getElementById('txtNotes').value = "";
        document.getElementById('txtPlan').value = "";
        for (var i = 0; i < TextBoxControls.length; i++) {
            var ID = TextBoxControls[i].id;
            if (ID.startsWith('txt') == true) {
                if (TextBoxControls[i].type == 'hidden') {
                    try {
                        var txtBoxName = TextBoxControls[i].id.replace("_ClientState", "");
                        if ($find(txtBoxName) != null) {
                            $find(txtBoxName).clear();
                        }
                    } catch (Error) { }
                } else {
                    document.getElementById(ID).value = '';
                }
            } else if (ID.startsWith('dtp') == true) {
                try {
                    var DateTime = TextBoxControls[i].id.replace("_ClientState", "");
                    if ($find(DateTime) != null) {
                        $find(DateTime).clear();
                    }
                } catch (Error) { }
            } else if (TextBoxControls[i].type == 'checkbox') {
                try {
                    TextBoxControls[i].checked = false;
                } catch (Error) { }
            }
        }
        for (var i = 0; i < ComboControls.length; i++) {
            var cboName = ComboControls[i].id;
            try {
                if (document.getElementById(cboName) != null) {
                    document.getElementById(cboName).selectedIndex = 0;
                }
            } catch (Error) { }
        }
        for (var i = 0; i < testarwe.length; i++) {
            var cboName = testarwe[i].id;
            try {
                if (document.getElementById(cboName) != null) {
                    document.getElementById(cboName).value = "";
                    $("#" + cboName).text('');
                }
            } catch (Error) { }
        }
    } else {
    }
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
}
function PlanCopyPrevious() {
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/CopyPreviousPlan",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var Plan = objdata.Plan;
            var previous = objdata.PreviousList;
            var process = objdata.Process;
            var dataFound = objdata.PreviousData;

            if (previous == '0') {
                onCopyPrevious('210010');
            }
            else if (process == false) {
                onCopyPrevious('210016');
            }
            else if (dataFound == false) {
                onCopyPrevious('170014');
            }
            else if (Plan == "") {
                onCopyPrevious('170014');
            }
            else if (Plan != "") {
                $('#txtPlan')[0].value = Plan;
                onCopyPrevious('');
            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });


}
function CopyPreviousPlan() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true" && localStorage.getItem("bSave") == "false") {
        event.preventDefault();
        event.stopPropagation();
        disableAutoSave();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        SavePlan();
        PlanCopyPrevious();
        return;
        //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialogPlan');
        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            disableAutoSave();
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            SavePlan();
        //            PlanCopyPrevious();
        //            $(dvdialog).dialog("close");
        //            return;
        //        },
        //        "No": function () {
        //            disableAutoSave();
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            PlanCopyPrevious();
        //            $(dvdialog).dialog("close");

        //        },
        //        "Cancel": function () {
        //            $(dvdialog).dialog("close");
        //            return;
        //        }
        //    }
        //});
    }
    else {
        PlanCopyPrevious();
    }
}

function btnPrintDocument() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var rows = $("#tblPlan tbody tr");
    var CheckedDocumnts = "";
    for (var i = 0; i < rows.length; i++) {
        if (($(rows[i])[0].style.display) != "none") {
            if ($(rows[i])[0].firstChild.nextSibling.childNodes[0].checked == true) {
                if (CheckedDocumnts == "") {
                    CheckedDocumnts = $(rows[i])[0].firstChild.nextSibling.childNodes[1].textContent + '|' + $(rows[i])[0].firstChild.nextSibling.childNodes[0].defaultValue;
                }
                else {
                    CheckedDocumnts += ':' + $(rows[i])[0].firstChild.nextSibling.childNodes[1].textContent + '|' + $(rows[i])[0].firstChild.nextSibling.childNodes[0].defaultValue;
                }
            }
        }
    }
    if (CheckedDocumnts == "") {
        DisplayErrorMessage('110805');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }

    var Givento = $('#txtGivento')[0].value;
    var Relationship = $('#cboRelationship')[0].value;
    var IsDocumentGIven = $('#cboIsDocumentGiven')[0].value;
    var currentSession = currentprocess
    var Data = [CheckedDocumnts, Givento, Relationship, IsDocumentGIven, currentSession];
    $.ajax({
        type: "POST",
        url: "WebServices/PlanService.asmx/PrintDocument",
        data: JSON.stringify({
            "data": Data,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var objdata = $.parseJSON(data.d);
            var SelectedDocs = objdata.SelectedItem;
            var files = objdata.Files;
            var screen = objdata.Screen;
            var summary = objdata.Summary;
            var human_id = objdata.HumanId;
            var EncId = objdata.EncId;
            var project = objdata.Project;
            var DownloadDoc = objdata.DownloadDoc;
            var FaxSubject = objdata.FaxSubject;
            if (FaxSubject != "")
                localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
            if (files != "" && SelectedDocs == "") {
                DisplayErrorMessageList('110806', files);
            }
            if (SelectedDocs != "") {
                OpenPDF_Plan(files, screen, summary, project, SelectedDocs, human_id, EncId);
                $('#cboRelationship')[0]._enabled = true;
                $('#cboIsDocumentGiven')[0]._enabled = true;
            }
            if (DownloadDoc != null && DownloadDoc != "") {
                var sPath = ""
                sPath = "frmWellnessNotes.aspx?PatientDocuments=Patient_Documents&CheckedDocumnts=" + DownloadDoc;;
                $(top.window.document).find("#PlanModal").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#ProcessiFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#PlanModal").modal('hide');

            }
            if (screen != "" && SelectedDocs == "") {
                if (project.indexOf("UCM") > -1) {
                    openCareTreat_Plan(screen, human_id, EncId);
                }
                else
                    openProgress_Plan(screen, human_id, EncId);
            }
            if (summary != "" && SelectedDocs == "" && screen == "") {
                OpenClinicalSmry_Plan();
            }


        },
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                //alert("USER MESSAGE:\n" +
                //                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                //                   "Message: " + log.Message);

                window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

            }
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function openCareTreat_Plan(screen, human_id, EncId, summary) {

    var obj = new Array();
    var now = new Date();
    date = now.getUTCFullYear() + '-' + (now.getUTCMonth() + 1) + '-' + now.getUTCDate();
    date = date.split('G')[0];
    obj.push("Date=" + date);
    if (screen == 'Care|Treat|Wellness') {
        $('#PlanModal').modal({ backdrop: 'static', keyboard: false }, 'show');
        var sPath = ""
        sPath = "frmWellnessNotes.aspx?SubMenuName=Care Note";
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                $('#PlanModal').modal('hide');
                $(top.window.document).find('#ProcessModal1').modal({ backdrop: 'static', keyboard: false }, 'show');
                var sPath = ""
                sPath = "frmWellnessNotes.aspx?SubMenuName=Treatment Notes";
                $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
                var sPath = ""
                sPath = "frmWellnessNotes.aspx?SubMenuName=Wellness Notes";
                $('#ProcessWellnessframe')[0].contentDocument.location.href = sPath;
                $(top.window.document).find('#ProcessModal1').modal('hide');

            }
        }
    }
    else if (screen == 'Care|Treat') {
        $('#PlanModal').modal({ backdrop: 'static', keyboard: false }, 'show');
        var sPath = ""
        sPath = "frmWellnessNotes.aspx?SubMenuName=Care Note";
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                $('#PlanModal').modal('hide');
                $(top.window.document).find('#ProcessModal1').modal({ backdrop: 'static', keyboard: false }, 'show');
                var sPath = ""
                sPath = "frmWellnessNotes.aspx?SubMenuName=Treatment Notes";
                $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
                $(top.window.document).find('#ProcessModal1').modal('hide');
            }
        }
    }

    else if (screen == 'Care') {
        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                var sPath = ""
                sPath = "frmWellnessNotes.aspx?SubMenuName=Care Note";
                $('#PlanModal').modal({ backdrop: 'static', keyboard: false }, 'show');
                $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
                $('#PlanModal').modal('hide');
            }
        }


    } else if (screen == 'Treat') {
        var sPath = ""
        sPath = "frmWellnessNotes.aspx?SubMenuName=Treatment Notes";
        $('#PlanModal').modal({ backdrop: 'static', keyboard: false }, 'show');
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        $('#PlanModal').modal('hide');
    }
    else if (screen == 'Wellness') {
        var sPath = ""
        sPath = "frmWellnessNotes.aspx?SubMenuName=Wellness Notes";
        $('#PlanModal').modal({ backdrop: 'static', keyboard: false }, 'show');
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        $('#PlanModal').modal('hide');
    }
    if (summary != null && summary != undefined && summary != "") {
        OpenClinicalSmry_Plan(summary);
    }
}

function OpenPDF_Plan(fileNotFound, screen, summary, project, SelectedDocs, human_id, EncId) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (fileNotFound != "") {
        DisplayErrorMessageList('110806', fileNotFound);
    }
    var obj = new Array();
    obj.push("SI=" + SelectedDocs);
    obj.push("Location=" + "STATIC");
    $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    var sPath = ""
    sPath = "frmPrintPDF.aspx?SI=" + SelectedDocs + "&Location=STATIC";
    $(top.window.document).find("#mdldlg")[0].style.width = "85%";
    $(top.window.document).find("#mdldlg")[0].style.height = "96%";
    $(top.window.document).find("#ProcessFrame")[0].style.height = "112%";
    $(top.window.document).find("#ProcessFrame")[0].style.width = "99%";
    $(top.window.document).find("#ProcessModal")[0].style.height = "";
    $(top.window.document).find("#ProcessModal")[0].style.width = "";
    $(top.window.document).find("#ProcessModal")[0].style.zIndex = "999";
    $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#ModalTitle")[0].textContent = "Print Documents";
    if (screen != null && screen != "") {
        if (project.indexOf("UCM") > -1) {
            openCareTreat_Plan(screen, human_id, EncId, summary);
        }
        else
            openProgress_Plan(screen, human_id, EncId, summary);
    }
    if (summary != "") {
        OpenClinicalSmry_Plan(summary);
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function OnClientClosePDF() {
    $("#ProcessModal1").modal('hide');
}

function openProgress_Plan(screen, human_id, EncId, summary) {

    var obj = new Array();
    var now = new Date();
    date = now.getUTCFullYear() + '-' + (now.getUTCMonth() + 1) + '-' + now.getUTCDate();
    date = date.split('G')[0];
    obj.push("Date=" + date);
    if (screen == 'Pro|Con') {
        //var sPath = ""
        //sPath = "frmConsultationNotes.aspx?Date=" + date;
        //$('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        //if (human_id != null && human_id != "" && human_id != "0") {
        //    if (EncId != null && EncId != "" && EncId != "0") {
        //        $('#PlanModal').modal('hide');
        //        var sPath = ""
        //        sPath = "frmSummaryNew.aspx?Menu=PDF";
        //        //BugID:42305
        //        $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
        //        $(top.window.document).find('#ProcessModal1').modal('hide');


        //    }
        //}

        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                var sPath = ""
                // sPath = "frmSummaryNew.aspx?Menu=PDF";
                sPath = "frmSummaryNew.aspx?Menu=PDF" + "&TabMode=true";
                //BugID:42305
                $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
                $(top.window.document).find('#ProcessModal1').modal('hide');
            }
        }

        var sPath = ""
        sPath = "frmConsultationNotes.aspx?Menu=PDF";
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        $('#PlanModal').modal('hide');
    }
    if (screen == 'Con|Wel') {
        $('#PlanModal').modal({ backdrop: 'static', keyboard: false }, 'show');
        var sPath = ""
        sPath = "frmConsultationNotes.aspx?Date=" + date;
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        StartLoadingImage();
        $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=WELLNESS NOTES" + "&Menu=True";
        $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Wellness Notes";
        StopLoadingImage();
    }
    else if (screen == 'Pro|Con|Wel') {
        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                var sPath = ""
                //sPath = "frmSummaryNew.aspx?Menu=PDF";
                sPath = "frmSummaryNew.aspx?Menu=PDF" + "&TabMode=true";
                //BugID:42305
                $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
                $(top.window.document).find('#ProcessModal1').modal('hide');


            }
        }

        var sPath = ""
        sPath = "frmConsultationNotes.aspx?Menu=PDF";
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        $('#PlanModal').modal('hide');
        StartLoadingImage();
        $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=WELLNESS NOTES" + "&Menu=True";
        $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Wellness Notes";


        StopLoadingImage();
    }

    else if (screen == 'Pro|Wel') {
        var sPath = ""
        StartLoadingImage();
        $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=WELLNESS NOTES" + "&Menu=True";
        $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Wellness Notes";


        StopLoadingImage();
        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                $('#PlanModal').modal('hide');
                var sPath = ""
                //sPath = "frmSummaryNew.aspx?Menu=PDF";
                sPath = "frmSummaryNew.aspx?Menu=PDF" + "&TabMode=true";
                //BugID:42305
                $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
                $(top.window.document).find('#ProcessModal1').modal('hide');


            }
        }
    }
    else if (screen == 'Pro') {
        if (human_id != null && human_id != "" && human_id != "0") {
            if (EncId != null && EncId != "" && EncId != "0") {
                var sPath = ""
                //sPath = "frmSummaryNew.aspx?Menu=PDF";
                sPath = "frmSummaryNew.aspx?Menu=PDF" + "&TabMode=true";
                //BugID:42305
                $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
                $(top.window.document).find('#ProcessModal1').modal('hide');


            }
        }


    } else if (screen == 'Con') {
        var sPath = ""
        sPath = "frmConsultationNotes.aspx?Menu=PDF";
        $('#ProcessiFrame')[0].contentDocument.location.href = sPath;
        $('#PlanModal').modal('hide');
    }
    else if (screen == 'Wel') {
        StartLoadingImage();
        $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=WELLNESS NOTES" + "&Menu=True";
        $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Wellness Notes";


        StopLoadingImage();
    }
    if (summary != null && summary != undefined && summary != "") {
        OpenClinicalSmry_Plan(summary);
    }
}
function OpenRecommendedmaterials_Plan() {
    $(top.window.document).find('#ProcessModal1').modal({ backdrop: 'static', keyboard: false }, 'show');
    var sPath = ""
    sPath = "frmRecommendedMaterials.aspx";
    $(top.window.document).find("#mdldlg1")[0].style.width = "74%";
    $(top.window.document).find("#ProcessFrame1")[0].style.height = "565px";
    $(top.window.document).find("#ProcessFrame1")[0].style.width = "600px";
    $(top.window.document).find("#ProcessModal1")[0].style.zIndex = "999";
    $(top.window.document).find('#ProcessFrame1')[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#ModalTitle1")[0].textContent = "Recommended Materials";
}
function OpenClinicalSmry_Plan(summary) {

    $(top.window.document).find('#ProcessPlan').modal({ backdrop: 'static', keyboard: false }, 'show');
    var sPath = ""
    sPath = "frmClinicalSummary.aspx";
    $(top.window.document).find("#mdldlgPlan")[0].style.width = "1000px";
    $(top.window.document).find("#mdldlgPlan")[0].style.height = "472px";
    $(top.window.document).find("#ProcessFrame2")[0].style.height = "465px";
    $(top.window.document).find("#ProcessFrame2")[0].style.width = "1000px";
    $(top.window.document).find("#ProcessFrame2")[0].style.zIndex = "999";
    $(top.window.document).find('#ProcessFrame2')[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#ModalTitle2")[0].textContent = "Clinical Summary";
}
var hdnFieldName = null;
function callweb(icon, List, id) {

    if (icon.className.indexOf("plus") > -1) {
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus")
        var ListValue = List;
        var typedval = "";
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: "{'fieldName':'" + ListValue + "','value':'" + typedval + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function OnSuccessPlan(response) {

                var values = response.d.split("|");
                var targetControlValue = "txtPlan";

                var innerdiv = '';
                var pos = $("#" + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:45541
                innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].attributes.getNamedItem('data-src').value + "');\">Click here to Add or Update Keywords</li>";
                for (var i = 0; i < values.length; i++) {

                    innerdiv += "<li style='text-decoration: none;cursor:default; list-style-type: none;color:black' onclick=\"fun('" + values[i].replace('"', "").replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~").replace(/\n/g, "$%^").split("$%^").join("~").split('"').join('') + "|" + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45541
                }

                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    $("<div class='Listdiv'  id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                        .css({
                            top: '30px',
                            left: '60px',
                            width: $("#" + targetControlValue).width() + 6,
                            height: '250px',
                            overflow: 'scroll',
                            position: 'absolute',
                            background: 'white',
                            bottom: '0',
                            border: '1px solid black',
                            floating: 'top'
                        })
                        .insertAfter($("#" + targetControlValue));
                }

            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus")
        $(icon.parentNode).children('div').css({ "display": "none" });
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
    }
    if (hdnFieldName != null && hdnFieldName != icon) {

        $(hdnFieldName).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFieldName = icon;
    $('#btnSave')[0].disabled = false;
    EnableSave();
}

function fun(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var value = agrulist.split("|");
    var sugglistval = $("#" + value[1]).val().trim();
    var lastChar = sugglistval.substring(sugglistval.length - 2, sugglistval.length);
    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split("*")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value[0]) {
                flag++;
            }
        }
        if (flag == 0 && sugglistval.indexOf(value[0]) == -1) {
            if (lastChar.indexOf('*') > -1) {
                $("#" + value[1]).val(sugglistval + value[0]);
            }
            else {
                $("#" + value[1]).val(sugglistval + "\n" + "*" + value[0]);
            }
        }
    }
    else {
        $("#" + value[1]).val(value[0]);
    }
    $("#" + value[1]).focus();
}
var tempArray = [];
function funPlanReason(agrulist) {
    EnableSave();
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var value = agrulist.split("|");
    var CheckArray = tempArray;
    var sugglistval = $("#" + value[1]).val().trim();
    var selectedItemID = event.currentTarget.id;
    if (selectedItemID.indexOf("FollowupList") > -1) {
        CheckArray = ResonNotPerformedRefList;
    }
    else if (selectedItemID.indexOf("ReasonNotPerformedList") > -1) {
        CheckArray = FollowupRefList;
    }
    var lastChar = sugglistval.substring(sugglistval.length - 2, sugglistval.length);
    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split(", ")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value[0]) {
                flag++;
            }
            else {
                if ((CheckArray.length > 0 && CheckArray.indexOf(subsugglistval[i]) > -1)) {
                    DisplayErrorMessage('9005');
                    CheckArray = tempArray;
                    return;
                }
            }
        }
        if (flag == 0 && sugglistval.indexOf(value[0]) == -1) {
            if (lastChar.indexOf(', ') > -1) {
                $("#" + value[1]).val(sugglistval + value[0]);
            }
            else {
                $("#" + value[1]).val(sugglistval + ", " + value[0]);
            }
        }
    }
    else {
        $("#" + value[1]).val(value[0]);
    }
    $("#" + value[1]).focus();
}

function cboRelationship_SelectedIndexChanged() {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var Data = $("cboRelationship").context.activeElement.selectedOptions[0].value
    if (Data.toUpperCase() == "SELF") {
        $('#txtGivento')[0].disabled = true;
        $('#txtGivento')[0].style.backgroundColor = "#ebebe4";
        $.ajax({
            type: "POST",
            url: "WebServices/PlanService.asmx/GetValue",
            data: JSON.stringify({
                "data": Data,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                var objdata = $.parseJSON(data.d);
                $('#txtGivento')[0].value = objdata;
                $('#txtGivento')[0].readOnly = true;
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }
                top.window.document.getElementById('ctl00_Loading').style.display = "none";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else if (Data.toUpperCase() == "") {
        $('#txtGivento')[0].value = "";
        $('#txtGivento')[0].disabled = true;
        $('#txtGivento')[0].readOnly = true;
        $('#txtGivento')[0].style.backgroundColor = "white";
    }
    else {
        $('#txtGivento')[0].disabled = false;
        $('#txtGivento')[0].value = "";
        $('#txtGivento')[0].readOnly = false;
        $('#txtGivento')[0].style.backgroundColor = "white";
    }
    //Cap - 936
    if (Data != "" && $('#cboIsDocumentGiven')[0].selectedIndex== 0) {
        $('#cboIsDocumentGiven')[0].selectedIndex = 1;
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
}

function ChkElectronicDeclaration_Checked() {
    $('#btnSave')[0].disabled = false;
    EnableSave();
}

function ChkPRN_Checked() {
    $('#btnSave')[0].disabled = false;
    EnableSave();
}

function ChkAfterStudies_Checked() {
    $('#btnSave')[0].disabled = false;
    EnableSave();
}
function ChkboxDocument_Checked() {
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
}
function OpenAddorUpdatePlan(ctrlID) {

    if (ctrlID.split('~$%')[1].indexOf("HistoryProblem") > -1) {

    }
    else if (document.getElementById(ctrlID.split('~$%')[1]).disabled == true) {

        return false;
    }

    var obj = new Array();
    obj.push("FieldName=" + ctrlID.split('~$%')[0]);
    var sPath = "";
    sPath = "frmAddorUpdateKeywords.aspx?FieldName=" + ctrlID.split('~$%')[0];
    $(top.window.document).find("#ProcessModal")[0].style.height = "575px";
    $(top.window.document).find("#ProcessModal")[0].style.width = "770px";
    $(top.window.document).find("#ProcessModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#mdldlg")[0].style.width = "86%";
    $(top.window.document).find("#ProcessFrame")[0].style.height = "511px";
    $(top.window.document).find("#ModalTitle").html("Add/Update Keywords");
    $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#ModalTitle")[0].textContent = "Add/Update Keywords";
    return false;
}
$("#tblPlan tr").click(function () {
    if ($('#btnSave')[0] != undefined && document.getElementById('btnPrint').disabled == false) {
        $('#btnSave')[0].disabled = false;
        EnableSave();
    }
    if ($("#tblPlan tr").find(':checked').length == 0) {
        $('#pnlDocumentDetails')[0].disabled = true;
        $('#cboRelationship')[0].disabled = true;
        $('#cboRelationship')[0].style.backgroundColor = "#ebebe4";
        $('#cboRelationship')[0].selectedIndex = 0;
        $('#txtGivento')[0].value = "";
        $('#txtGivento')[0].disabled = true;
        $('#cboIsDocumentGiven')[0].disabled = true;
        $('#cboIsDocumentGiven')[0].selectedIndex = 0;
        $('#cboIsDocumentGiven')[0].style.backgroundColor = "#ebebe4";
    }
    else {
        $('#pnlDocumentDetails')[0].disabled = false;
        $('#cboRelationship')[0].disabled = false;
        if ($('#cboRelationship')[0].selectedIndex == 0) {
            $.ajax({
                type: "POST",
                url: "WebServices/PlanService.asmx/GetValue",
                data: JSON.stringify({
                    "data": 'Self',
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    var objdata = $.parseJSON(data.d);
                    $('#txtGivento')[0].value = objdata;


                },
                error: function OnError(xhr) {
                    if (xhr.status == 999)
                        window.location = "/frmSessionExpired.aspx";
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        ////alert("USER MESSAGE:\n" +
                        ////            ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                        ////           "Message: " + log.Message);

                        window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                    }
                    top.window.document.getElementById('ctl00_Loading').style.display = "none";
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
            });
            $('#cboRelationship')[0].selectedIndex = 1;
            $('#cboRelationship')[0].style.backgroundColor = "white";
            $('#cboIsDocumentGiven')[0].style.backgroundColor = "white";
            $('#txtGivento')[0].readOnly = true;
            $('#txtGivento')[0].disabled = true;
            $('#txtGivento')[0].style.backgroundColor = "#ebebe4";
        }
        else if ($('#cboRelationship')[0].selectedIndex != 1) {
            $('#txtGivento')[0].style.backgroundColor = "white";
            $('#cboRelationship')[0].style.backgroundColor = "white";
            $('#cboIsDocumentGiven')[0].style.backgroundColor = "white";
        }


        $('#cboIsDocumentGiven')[0].disabled = false;
        $('#cboIsDocumentGiven')[0].selectedIndex = 1;
        // }
    }
});


$.fn.getType = function () { return this[0].tagName == "INPUT" ? this[0].type.toLowerCase() : this[0].tagName.toLowerCase(); }
var hdnFollowup = null;
function FollowupNotes(icon, List, id) {
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
    if (icon.className.indexOf("plus") > -1) {
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
                    $('#btnSave')[0].disabled = false;
                var values = response.d.split("|");
                var targetControlValue = id;
                var innerdiv = '';
                var pos = $('#' + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:45541
                innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].attributes.getNamedItem('data-src').value + "');\">Click here to Add or Update Keywords</li>";
                for (var i = 0; i < values.length; i++) {
                    innerdiv += "<li style='text-decoration: none;cursor:default; list-style-type: none;color:black' onclick=\"FollowUp('" + values[i].replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45541
                }

                var listlength = innerdiv.length;
                if (listlength > 0) {

                    if (document.getElementById(id).value != "")
                        var txtValue = document.getElementById(id).value;
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    //BugID:49036 - Decreased height for follow-up notes
                    $("<div  class='Listdiv' id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                        .css({
                            top: pos.top + $("#" + targetControlValue).height() + 7,
                            left: pos.left + 12,
                            height: '50px',
                            overflow: 'scroll',
                            position: 'fixed',
                            background: 'white',
                            bottom: '0',
                            floating: 'top',
                            width: '400px',
                            border: '1px solid #8e8e8e',
                            background: '#FFF',
                            fontFamily: 'Segoe UI",Arial,sans-serif',
                            fontSize: '12px',
                            zIndex: '17',
                            overflowX: 'auto'

                        })

                        .insertAfter($("#" + targetControlValue));
                    // }
                }
                EnableSave(); { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function OnError(xhr) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //               "Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }
            }

        });
    }
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");
    }

    if (hdnFollowup != null && hdnFollowup != icon) {

        $(hdnFollowup).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFollowup = icon;
    if (!(document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false))
        $('#btnSave')[0].disabled = false;
    EnableSave();
}
function FollowUp(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var value = agrulist.split("^");
    var sugglistval = $("#" + value[1]).val().trim();

    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split(",\t")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value[0]) {
                flag++;
            }
        }
        if (flag == 0) {
            $("#" + value[1]).val(sugglistval + ",\t" + value[0]);
        }
    }
    else {
        $("#" + value[1]).val(value[0]);
    }
}

/*For Print Documents*/

function OpenPDF_PrintDocument(fileNotFound, screen, project, summary, FaxSubject) {
    if (FaxSubject != "")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
    if (fileNotFound != "") {
        DisplayErrorMessageList('110806', fileNotFound);
    }
    var childWindow = $find('RadWindow2');
    var obj = new Array();
    obj.push("SI=" + document.getElementById('hdnSelectedItem').value);
    obj.push("Location=" + "STATIC");

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmPrintPDF.aspx?SI=" + document.getElementById('hdnSelectedItem').value + "&Location=STATIC", "RadWindow2");
            //Jira CAP-1011
            //SetRadWindowProperties(childWindow, 720, 750);
            SetRadWindowProperties(childWindow, 800, 1000);
            childWindow.add_close(EnableSave);
        }, 0);
    if (screen != null) {
        if (project.indexOf("UCM") > -1) {
            openCareTreatPlan_PrintDocument(screen, FaxSubject);

        }
        else
            openProgress_PrintDocument(screen, FaxSubject);
    }
    if (document.getElementById('hdnXmlPath').value != null && document.getElementById('hdnXmlPath').value != "") {
        var filelocation = document.getElementById('hdnXmlPath').value; // To open the generated XML in Human readable format
        window.open(filelocation, "CDA Human Readable", "", "")
    }
    OnClientClosePDF_PrintDocument();
}
function OnClientClosePDF_PrintDocument() {
    if (document.getElementById('hdnfilename').value == "Recommended Material") {
        OpenRecommendedmaterials();
        document.getElementById('hdnfilename').value = "";
    }
}
function openProgress_PrintDocument(screen, FaxSubject) {
    if (FaxSubject != "")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
    var obj = new Array();
    var date = document.getElementById(GetClientId('hdnLocalTime')).value;
    date = date.split('G')[0];
    obj.push("Date=" + date);
    obj.push("Menu=" + "PDF");

    if (screen == 'Pro|Con') {
        var objWindow = $find("RadWindow3");

        var objConsult = openModalProgress("frmConsultationNotes.aspx", 700, 750, obj, "RadWindow3");
        var win = $find("RadWindow3");
        win.hide();
        EnableSave();
        if (document.getElementById(GetClientId("hdnHumanID")) != null && document.getElementById(GetClientId("hdnHumanID")).value != "" && document.getElementById(GetClientId("hdnHumanID")).value != "0") {
            if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
                setTimeout(
                    function () {
                        var objConsult = openModalProgress("frmSummaryNew.aspx", 700, 750, obj, "RadwindowPDF");
                        var win = $find("RadwindowPDF");
                        win.hide();
                    }, 0);
            }
        }
    }
    else if (screen == 'Pro') {
        if (document.getElementById(GetClientId("hdnHumanID")) != null && document.getElementById(GetClientId("hdnHumanID")).value != "" && document.getElementById(GetClientId("hdnHumanID")).value != "0") {
            if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
                setTimeout(
                    function () {
                        //var objConsult = openModalProgress("frmSummaryNew.aspx?Menu=True", 700, 750, obj, "RadwindowPDF");
                        var objConsult = openModalProgress("frmSummaryNew.aspx?Menu=True&TabMode=true", 700, 750, obj, "RadwindowPDF");
                        var win = $find("RadwindowPDF");
                        win.hide();

                    }, 0);
            }
        }

    } else if (screen == 'Con') {
        var objConsult = openModalProgress("frmConsultationNotes.aspx", 10, 10, obj, "RadWindow3");
        var win = $find("RadWindow3");
        win.hide();
    }
    OpenClinicalSmry_PrintDocument(FaxSubject);
    EnableSave();
}

function openCareTreatPlan_PrintDocument(screen, FaxSubject) {
    if (FaxSubject != "")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
    var obj = new Array();
    var date = document.getElementById(GetClientId('hdnLocalTime')).value;
    date = date.split('G')[0];
    obj.push("Date=" + date);
    if (screen == 'Care|Treat|Wellness') {
        var objWindow = $find("RadWindow4");
        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Care Note", 700, 750, obj, "RadWindow4");
        var win = $find("RadWindow4");
        win.hide();
        EnableSave();
        if (document.getElementById(GetClientId("hdnHumanID")) != null && document.getElementById(GetClientId("hdnHumanID")).value != "" && document.getElementById(GetClientId("hdnHumanID")).value != "0") {
            if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {

                setTimeout(
                    function () {
                        var objWindow = $find("RadWindow5");
                        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Treatment Notes", 700, 750, obj, "RadWindow5");
                        var win = $find("RadWindow5");
                        win.hide();
                        EnableSave();

                    }, 0);
            }
        }
        var objWindow = $find("WellnessWindow");
        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Wellness Notes", 700, 750, obj, "WellnessWindow");
        var win = $find("WellnessWindow");
        win.hide();
        EnableSave();

    }
    if (screen == 'Care|Treat') {
        var objWindow = $find("RadWindow4");
        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Care Note", 700, 750, obj, "RadWindow4");
        var win = $find("RadWindow4");
        win.hide();
        EnableSave();
        if (document.getElementById(GetClientId("hdnHumanID")) != null && document.getElementById(GetClientId("hdnHumanID")).value != "" && document.getElementById(GetClientId("hdnHumanID")).value != "0") {
            if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
                setTimeout(
                    function () {
                        var objWindow = $find("RadWindow5");
                        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Treatment Notes", 700, 750, obj, "RadWindow5");
                        var win = $find("RadWindow5");
                        win.hide();
                        EnableSave();

                    }, 0);
            }
        }
    }
    else if (screen == 'Care') {
        if (document.getElementById(GetClientId("hdnHumanID")) != null && document.getElementById(GetClientId("hdnHumanID")).value != "" && document.getElementById(GetClientId("hdnHumanID")).value != "0") {
            if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
                setTimeout(
                    function () {
                        var objWindow = $find("RadWindow4");
                        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Care Note", 700, 750, obj, "RadWindow4");
                        var win = $find("RadWindow4");
                        win.hide();
                        EnableSave();
                    }, 0);
            }
        }

    } else if (screen == 'Treat') {
        var objWindow = $find("RadWindow5");
        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Treatment Notes", 700, 750, obj, "RadWindow5");
        var win = $find("RadWindow5");
        win.hide();
        EnableSave();
    }
    else if (screen == 'Wellness') {
        var objWindow = $find("WellnessWindow");
        var objConsult = openModalProgress("frmWellnessNotes.aspx?SubMenuName=Wellness Notes", 700, 750, obj, "WellnessWindow");
        var win = $find("WellnessWindow");
        win.hide();
        EnableSave();
    }

    OpenClinicalSmry_PrintDocument(FaxSubject);
    EnableSave();
}


function OpenClinicalSmry_PrintDocument(FaxSubject) {
    if (FaxSubject != "")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);
    if (document.getElementById('hdnXmlPath') != null && document.getElementById('hdnXmlPath') != undefined && document.getElementById('hdnXmlPath').value != "") {
        window.open(document.getElementById('hdnXmlPath').value, "CDA Human Readable", "", "");
        EnableSave();
    }
}
function WellNotesWindow_PrintDocument() {
    var childWindow = $find('WellnessWindow').BrowserWindow.radopen("frmWellnessNotes.aspx?formName=WellnessNotes", "WellnessWindow");
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(850, 680);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
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
function PrintDocumentwaitcursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();
}
function Printwaitcursor() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();
    EnableSave();
}


function Close_printDocument(sender, args) {
    if ((document.getElementById("btnSave") != null && document.getElementById("btnSave").disabled == false) || (document.getElementById("btnMovetoPhyAsst") != null && document.getElementById("btnMovetoPhyAsst").disabled == false)) {
        // { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (document.getElementById('btnMovetoPhyAsst') != null)
            document.getElementById("hdnSaveDetails").value = "true";
        //triggerServerSave = true;
        //ValidationPrintDoc();
        var obj = new Array();
        obj.push("Title=" + "Message");
        obj.push("ErrorMessages=" + "There are unsaved changes.Do you want to save them?");
        var result = openModal("frmValidationArea.aspx", 100, 300, obj, "MessageWindow");
        var WindowName = $find('MessageWindow');
        WindowName.add_close(OnClientCloseValidation);
    }
    else {
        self.close();
    }
}
function ValidationPrintDocAutosave() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();

    if (document.getElementById('radbtnAgreewithChanges') != null) {
        if (document.getElementById('radbtnAgreewithChanges').checked == true) {
            if (document.getElementById('txtAddendumToPlan').value == '') {
                DisplayErrorMessage('010012');
                document.getElementById('txtAddendumToPlan').focus();

                $('#txtAddendumToPlan').removeClass('nonEditabletxtbox');
                $('#txtAddendumToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('radbtnCorrection') != null) {
        if (document.getElementById('radbtnCorrection').checked == true) {
            if (document.getElementById('txtCorrectionToPlan').value == '') {
                DisplayErrorMessage('010022');
                document.getElementById('txtCorrectionToPlan').focus();
                $('#txtCorrectionToPlan').removeClass('nonEditabletxtbox');
                $('#txtCorrectionToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('pnlElectronicSignature').disabled == false) {
        if (document.getElementById('chkElectronicDeclaration').checked == false) {
            DisplayErrorMessage('010015');

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    if (document.getElementById('chkElectronicDeclaration').disabled == false && document.getElementById('chkElectronicDeclaration').checked == false)
        document.getElementById('hdnChkElecDeclaration').value = "true";
    else
        document.getElementById('hdnChkElecDeclaration').value = "false";

    if (triggerServerSave || (document.getElementById("hdnSaveDetails").value == "true")) {
        if (document.getElementById('btnSave') != null && document.getElementById('btnSave').disabled == false)
            __doPostBack('btnSave', '');
        else if (document.getElementById('btnMovetoPhyAsst') != null && document.getElementById('btnMovetoPhyAsst').disabled == false && document.getElementById('btnmovetoscribe') != null && document.getElementById('btnmovetoscribe') != undefined && document.getElementById('btnmovetoscribe').disabled == false) {
            alert('Please click either Move to Physician Assistant or Move to Scribe');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else if (document.getElementById('btnMovetoPhyAsst') != null && document.getElementById('btnMovetoPhyAsst').disabled == false)
            __doPostBack('btnMovetoPhyAsst', '');
        triggerServerSave = false;
    }


    return true;
}
var triggerServerSave = false;
function OnClientCloseValidation(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var result = arg;
        document.getElementById("hdnMessageType").value = result;
        if (result == "Yes") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            if (document.getElementById('btnMovetoPhyAsst') != null)
                document.getElementById("hdnSaveDetails").value = "true";
            triggerServerSave = true;
            ValidationPrintDocAutosave();

        } else if (result == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
            return false;
            oWindow.close();
            document.getElementById("HdnCopyButton").value = "";

        }
        else if (result == "No") {
            document.getElementById("hdnMessageType").value = ""
            top.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
            self.close();
            oWindow.remove_close(OnClientCloseValidation);
            oWindow.close();
            disableAutoSave();//BugID:52804

        }
    }
}
function ClosePrintDoc() {
    if (document.getElementById('DigitalSign').value == "true") {
        var Result = new Array();
        Result.MySignID = document.getElementById('MySignID').value;
        Result.SignedDate = document.getElementById('SignedDateandTime').value;
        returnToParent(Result);
    } else {
        var Result = new Array();
        Result.Close = "true";
        returnToParent(Result);
    }
    GetRadWindow().close();
}

function ClosePrintDocMovetoMyQ() {
    GetRadWindow().close();
    window.top.location.href = "frmMyQueueNew.aspx";
}
function ClosePrintDoc() {
    GetRadWindow().close();
    window.top.location.href = "frmPatientChart.aspx";
}
function returnToParent(args) {
    var oArg = new Object();
    oArg.result = args;
    var oWnd = GetRadWindow();
    if (oArg.result) {
        oWnd.close(oArg.result);
    } else {
        oWnd.close(oArg.result);
    }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}
function ValidationPrintDocmove() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();

    if (document.getElementById('radbtnAgreewithChanges') != null) {
        if (document.getElementById('radbtnAgreewithChanges').checked == true) {
            if (document.getElementById('txtAddendumToPlan').value == '') {
                DisplayErrorMessage('010012');
                document.getElementById('txtAddendumToPlan').focus();

                $('#txtAddendumToPlan').removeClass('nonEditabletxtbox');
                $('#txtAddendumToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('radbtnCorrection') != null) {
        if (document.getElementById('radbtnCorrection').checked == true) {
            if (document.getElementById('txtCorrectionToPlan').value == '') {
                DisplayErrorMessage('010022');
                document.getElementById('txtCorrectionToPlan').focus();
                $('#txtCorrectionToPlan').removeClass('nonEditabletxtbox');
                $('#txtCorrectionToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('pnlElectronicSignature').disabled == false) {
        if (document.getElementById('chkElectronicDeclaration').checked == false) {
            DisplayErrorMessage('010015');

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    if (document.getElementById('chkElectronicDeclaration').disabled == false && document.getElementById('chkElectronicDeclaration').checked == false)
        document.getElementById('hdnChkElecDeclaration').value = "true";
    else
        document.getElementById('hdnChkElecDeclaration').value = "false";


    __doPostBack('btnSave', '');




    return true;
}
function ValidationPrintDoc() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();

    if (document.getElementById('radbtnAgreewithChanges') != null) {
        if (document.getElementById('radbtnAgreewithChanges').checked == true) {
            if (document.getElementById('txtAddendumToPlan').value == '') {
                DisplayErrorMessage('010012');
                document.getElementById('txtAddendumToPlan').focus();

                $('#txtAddendumToPlan').removeClass('nonEditabletxtbox');
                $('#txtAddendumToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('radbtnCorrection') != null) {
        if (document.getElementById('radbtnCorrection').checked == true) {
            if (document.getElementById('txtCorrectionToPlan').value == '') {
                DisplayErrorMessage('010022');
                document.getElementById('txtCorrectionToPlan').focus();
                $('#txtCorrectionToPlan').removeClass('nonEditabletxtbox');
                $('#txtCorrectionToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('pnlElectronicSignature').disabled == false) {
        if (document.getElementById('chkElectronicDeclaration').checked == false) {
            DisplayErrorMessage('010015');

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    if (document.getElementById('chkElectronicDeclaration').disabled == false && document.getElementById('chkElectronicDeclaration').checked == false)
        document.getElementById('hdnChkElecDeclaration').value = "true";
    else
        document.getElementById('hdnChkElecDeclaration').value = "false";


    __doPostBack('btnMovetoPhyAsst', '');




    //return true;
}
function ValidationPrintDocscribe() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    GetUTCTime();

    if (document.getElementById('radbtnAgreewithChanges') != null) {
        if (document.getElementById('radbtnAgreewithChanges').checked == true) {
            if (document.getElementById('txtAddendumToPlan').value == '') {
                DisplayErrorMessage('010012');
                document.getElementById('txtAddendumToPlan').focus();

                $('#txtAddendumToPlan').removeClass('nonEditabletxtbox');
                $('#txtAddendumToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('radbtnCorrection') != null) {
        if (document.getElementById('radbtnCorrection').checked == true) {
            if (document.getElementById('txtCorrectionToPlan').value == '') {
                DisplayErrorMessage('010022');
                document.getElementById('txtCorrectionToPlan').focus();
                $('#txtCorrectionToPlan').removeClass('nonEditabletxtbox');
                $('#txtCorrectionToPlan').addClass('Editabletxtbox');
                if (document.getElementById('btnSave') != null)
                    document.getElementById('btnSave').disabled = true;
                if (document.getElementById('btnMovetoPhyAsst') != null)
                    document.getElementById('btnMovetoPhyAsst').disabled = true;
                document.getElementById("hdnEnableYesNo").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById('pnlElectronicSignature').disabled == false) {
        if (document.getElementById('chkElectronicDeclaration').checked == false) {
            DisplayErrorMessage('010015');

            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    if (document.getElementById('chkElectronicDeclaration').disabled == false && document.getElementById('chkElectronicDeclaration').checked == false)
        document.getElementById('hdnChkElecDeclaration').value = "true";
    else
        document.getElementById('hdnChkElecDeclaration').value = "false";

    // __doPostBack('btnmovetoscribe', '');

    return true;
}
function OpenAddOrUpdatePlan() {

    var childWindow = $find('PlanWindow').BrowserWindow.radopen("frmAddorUpdatePlan.aspx?formName=frmAddorUpdatePlan", "Plan");
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(900, 440);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}

//bugid:46445
function callwebplan(icon, List, id) {

    if (icon.className.indexOf("plus") > -1) {
        var tempArrFollowup = [];
        var tempArrReasonNOtFollowup = [];
        var fixed_ht = $(event.currentTarget.parentElement.parentElement.parentElement).position().top;
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus")
        var ListValue = List;
        var typedval = "";
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValuesPlanFollowup",
            data: "{'fieldName':'" + ListValue + "','value':'" + typedval + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function OnSuccessPlan(response) {
                FollowupRefList = tempArrFollowup;
                ResonNotPerformedRefList = tempArrReasonNOtFollowup;
                var FollowUpListValues = JSON.parse(response.d);
                var CommentsList = JSON.parse(FollowUpListValues.CommentsList);
                var FollowupList = JSON.parse(FollowUpListValues.FollowupList);
                var ReasonNotPerformed = JSON.parse(FollowUpListValues.ReasonNotPerformed);
                var values = response.d.split("|");
                var targetControlValue = id;
                var pos = $("#" + targetControlValue).position();
                var innerdiv = '';
                var FlloUpALL_count = 0;
                var FlloUpFreq_count = 0;
                var ReasNotPerALL_count = 0;
                var ReasNotPerFreq_count = 0;

                innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;font-weight:bolder;font-style: italic;cursor:default'>Comments</li>";
                innerdiv += "<li class='alinkstyle' style='text-decoration: none; list-style-type: none;font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].attributes.getNamedItem('data-src').value.replace("BMI~", "") + "');\">Click here to Add or Update Keywords</li>";

                if (CommentsList.length > 0) {
                    for (var i = 0; i < CommentsList.length; i++) {
                        innerdiv += "<li id='CommentsList" + i + "' style='text-decoration: none;cursor:default; list-style-type: none;color:black' onclick=\"funPlanReason('" + CommentsList[i].replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~") + "|" + targetControlValue + "');\">" + CommentsList[i] + "</li>";
                    }
                }
                if (FollowupList.length > 0) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;font-weight:bolder;font-style: italic;cursor:default'>Follow Up</li>";
                    for (var i = 0; i < FollowupList.length; i++) {
                        var displayStyle = "display:none;";
                        if (FollowupList[i].split('|').length > 1) {
                            if (FollowupList[i].split('|')[1].toUpperCase() == "FREQUENTLY USED") {
                                displayStyle = "display:block;";
                                FlloUpFreq_count++;
                            }
                            else {
                                FlloUpALL_count++;
                            }
                        }
                        innerdiv += "<li id='FollowupList" + i + "' style='text-decoration: none;cursor:default; list-style-type: none;color:#6504d0;" + displayStyle + "' onclick=\"funPlanReason('" + FollowupList[i].split('|')[0].replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~") + "|" + targetControlValue + "');\">" + FollowupList[i].split('|')[0] + "</li>";
                        FollowupRefList.push(FollowupList[i].split('|')[0]);
                    }
                    if (FlloUpALL_count > 0 && FlloUpFreq_count > 0)
                        innerdiv += "<li id='FollowupList_ViewMore' style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick='DisplayAll()'>Click to view more</li>";
                }
                if (ReasonNotPerformed.length > 0) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;font-weight:bolder;font-style: italic;cursor:default'>Reson Not Performed for FollowUp</li>";
                    for (var i = 0; i < ReasonNotPerformed.length; i++) {
                        var displayStyle = "display:none;";
                        if (ReasonNotPerformed[i].split('|').length > 1) {
                            if (ReasonNotPerformed[i].split('|')[1].toUpperCase() == "FREQUENTLY USED") {
                                ReasNotPerFreq_count++;
                                displayStyle = "display:block;";
                            }
                            else {
                                ReasNotPerALL_count++;
                            }
                        }
                        innerdiv += "<li id='ReasonNotPerformedList" + i + "' style='text-decoration: none;cursor:default; list-style-type: none;color:#6504d0;" + displayStyle + "' onclick=\"funPlanReason('" + ReasonNotPerformed[i].split('|')[0].replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~") + "|" + targetControlValue + "');\">" + ReasonNotPerformed[i].split('|')[0] + "</li>";
                        ResonNotPerformedRefList.push(ReasonNotPerformed[i].split('|')[0]);
                    }
                    if (ReasNotPerALL_count > 0 && ReasNotPerFreq_count > 0)
                        innerdiv += "<li id='ReasonNotPerformedList_ViewMore' style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick='DisplayAll()'>Click to view more</li>";
                }

                var listlength = innerdiv.length;
                for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                    if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                        document.getElementsByTagName("div")[i].hidden = true;
                    }
                }
                $("<div class='Listdiv'  id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                    .css({

                        //BugID:47395
                        top: fixed_ht + $("#" + targetControlValue).height() + 8,
                        left: '410px',
                        width: '390px',
                        height: '200px',
                        overflow: 'auto',
                        position: 'fixed',
                        background: 'white',
                        bottom: '0',
                        zIndex: '17',
                        border: '1px solid black',
                        floating: 'top',
                        marginBottom: '10px'

                    })

                    .insertAfter($("#" + targetControlValue));
                if (FlloUpFreq_count == 0) {
                    $("li[id^=FollowupList]").css("display", "block");
                }
                if (ReasNotPerFreq_count == 0) {
                    $("li[id^=ReasonNotPerformedList]").css("display", "block");
                }
            },
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = "/frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    // alert("USER MESSAGE:\n" +
                    // ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //"Message: " + log.Message);

                    window.location = "ErrorPage.aspx?Message=" + log.Message + "|$|" + log.StackTrace;;

                }

            }
        });
    }
    else {
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus")
        $(icon.parentNode).children('div').css({ "display": "none" });
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $("#dvTypes").css("overflow-y", "auto");
    }
    if (hdnFieldName != null && hdnFieldName != icon) {

        $(hdnFieldName).removeClass("fa fa-minus").addClass("fa fa-plus");

    }
    hdnFieldName = icon;
    $('#btnSave')[0].disabled = false;
    EnableSave();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
//bug id 46445
function funplan(agrulist, ctrl) {
    EnableSave();
    var value = agrulist.split("|");
    value[0] = value[0].trimRight("\n");//BugID:44608
    var sugglistval = $("#" + value[1]).text().trim();
    var sugglistvalcode = $("#" + value[1] + "hide").text().trim();

    if (ctrl.checked) {

        if (sugglistval == '') {
            $("#" + value[1]).text(sugglistval + value[0].split('~')[0]);
            $("#" + value[1] + "hide").text(sugglistvalcode + value[0].split('~')[1]);
        }
        else {
            if (sugglistval.indexOf(value[0].split('~')[0]) <= -1)
                $("#" + value[1]).text(sugglistval + ", " + value[0].split('~')[0]);
            $("#" + value[1] + "hide").text(sugglistvalcode + ", " + value[0].split('~')[1]);
        }
    }
    else {
        if (sugglistval.indexOf(value[0].split('~')[0] + ", ") > -1) {
            $("#" + value[1]).text(sugglistval.replace(value[0].split('~')[0] + ", ", ''));
            $("#" + value[1] + "hide").text(sugglistvalcode.replace(value[0].split('~')[1] + ", ", ''));
        }
        else if (sugglistval.indexOf(", " + value[0].split('~')[0]) > -1) {
            $("#" + value[1]).text(sugglistval.replace(", " + value[0].split('~')[0], ''));
            $("#" + value[1] + "hide").text(sugglistvalcode.replace(", " + value[0].split('~')[1], ''));
        }

        else {
            $("#" + value[1]).text(sugglistval.replace(value[0].split('~')[0], ''));
            $("#" + value[1] + "hide").text(sugglistvalcode.replace(value[0].split('~')[1], ''));
        }



    }

    $("#" + value[1]).focus();
}
function DueonClick(obj) {

    return false;
}

function ReturnInClick(obj) {
    if (document.getElementById('chkReturnIn').checked == true) {
        document.getElementById('txtReturnIn').disabled = false;
        document.getElementById('txtRetrunWeeks').disabled = false;
        document.getElementById('txtRetrunDays').disabled = false;
        if (document.getElementById('chkPRN') != null && document.getElementById('chkPRN') != undefined)
            document.getElementById('chkPRN').disabled = false;  /*change*/

        if (document.getElementById('chkAfterStudies') != null && document.getElementById('chkAfterStudies') != undefined)
            document.getElementById('chkAfterStudies').disabled = false;


        $('#txtReturnIn').removeClass('nonEditabletxtbox');
        $('#txtRetrunWeeks').removeClass('nonEditabletxtbox');
        $('#txtRetrunDays').removeClass('nonEditabletxtbox');
        $('#txtRetrunDays').addClass('Editabletxtbox');
        $('#txtRetrunWeeks').addClass('Editabletxtbox');
        $('#txtRetrunDays').addClass('Editabletxtbox');
        EnableSave();

    } else if (document.getElementById('chkReturnIn').checked == false) {
        document.getElementById('txtReturnIn').disabled = true;
        document.getElementById('txtRetrunWeeks').disabled = true;
        document.getElementById('txtRetrunDays').disabled = true;
        if (document.getElementById('chkPRN') != null && document.getElementById('chkPRN') != undefined)
            document.getElementById('chkPRN').disabled = true;
        if (document.getElementById('chkAfterStudies') != null && document.getElementById('chkAfterStudies') != undefined)
            document.getElementById('chkAfterStudies').disabled = true;


        $('#txtReturnIn').removeClass('Editabletxtbox');
        $('#txtRetrunWeeks').removeClass('Editabletxtbox');
        $('#txtRetrunDays').removeClass('Editabletxtbox');
        $('#txtReturnIn').addClass('nonEditabletxtbox');
        $('#txtRetrunWeeks').addClass('nonEditabletxtbox');
        $('#txtRetrunDays').addClass('nonEditabletxtbox');


    }
    return obj;
}

function AllowNumbers(evt) {
    EnableSave();
    var charCode = (evt.which) ? evt.which : event.keyCode;
    // if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {//For Bug ID 67747 
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        event.preventDefault();
        return false;
    }
    return true;

}
function EnableDocuments() {
    var s = false;
    var chkListModules = document.getElementById('chklstSelectDocuments');
    if (chkListModules == null) {
        return false;
    }
    if (chkListModules != null) {
        var chkListinputs = chkListModules.getElementsByTagName("input");

        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                s = true;
                break;
            }
        }
    }
    if (s == false) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnClearDrpdwn").click();//BugID:40876
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    //Cap - 936
    //else {
    //    $find('cboRelationship').enable(true);
    //    $find('cboIsDocumentGiven').enable(true);
    //}
    else {
        $find('cboRelationship').enable(true);
        if (document.getElementById("cboRelationship").value == "") {
            var cmb = $find("cboRelationship");
            if (cmb._itemData.length > 0) {
                var val = cmb._itemData[1].value;
                var cItem = cmb.findItemByText(val);
                cItem.select();
            }
        }

        $find('cboIsDocumentGiven').enable(true);
        var cmbDoc = $find("cboIsDocumentGiven");
        if (cmbDoc._itemData.length > 0) {
            var valDoc = cmbDoc._itemData[1].value;
            var cDocItem = cmbDoc.findItemByText(valDoc);
            cDocItem.select();
        }
    }


    EnableSave();
}
function OpenAddUpdateKeywords(FieldName) {
    var focused = FieldName;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
    return false;
}

function ForSaveEnable() {
    if (document.getElementById("chkElectronicDeclaration").checked == true) {
        document.getElementById("btnSave").disabled = false;
        if (document.getElementById("hdnEnableYesNo") != null) {
            document.getElementById("hdnEnableYesNo").value = "true";
        }
    }
}

function ChangeRadio() {
    if (document.getElementById("radbtnAgreePlan").checked == true && document.getElementById("radbtnAgreePlan").disabled == false) {
        document.getElementById("txtAddendumToPlan").disabled = true;
        document.getElementById("txtCorrectionToPlan").disabled = true;
        $('#txtAddendumToPlan').removeClass('Editabletxtbox');
        $('#txtAddendumToPlan').addClass('nonEditabletxtbox');
        $('#txtCorrectionToPlan').removeClass('Editabletxtbox');
        $('#txtCorrectionToPlan').addClass('nonEditabletxtbox');
        if (document.getElementById("hdnEnableYesNo") != null) {
            document.getElementById("hdnEnableYesNo").value = "true";
        }
        if (document.getElementById("btnMovetoPhyAsst") != null)
            document.getElementById("btnMovetoPhyAsst").disabled = true;

        if (document.getElementById("btnmovetoscribe") != null)

            document.getElementById("btnmovetoscribe").disabled = true;



        if (document.getElementById("btnSave") != null)
            document.getElementById("btnSave").disabled = false;
        document.getElementById("chkElectronicDeclaration").disabled = false;
    }
    if (document.getElementById("radbtnAgreewithChanges").checked == true && document.getElementById("radbtnAgreewithChanges").disabled == false) {
        document.getElementById("txtAddendumToPlan").disabled = false;
        $('#txtAddendumToPlan').removeClass('nonEditabletxtbox');
        $('#txtAddendumToPlan').addClass('Editabletxtbox');
        document.getElementById("txtCorrectionToPlan").disabled = true;
        $('#txtCorrectionToPlan').removeClass('Editabletxtbox');
        $('#txtCorrectionToPlan').addClass('nonEditabletxtbox');
        if (document.getElementById("hdnEnableYesNo") != null) {
            document.getElementById("hdnEnableYesNo").value = "true";
        }
        if (document.getElementById("btnMovetoPhyAsst") != null)
            document.getElementById("btnMovetoPhyAsst").disabled = true;

        if (document.getElementById("btnmovetoscribe") != null)
            document.getElementById("btnmovetoscribe").disabled = true;

        if (document.getElementById("btnSave") != null)
            document.getElementById("btnSave").disabled = false;
        document.getElementById("chkElectronicDeclaration").disabled = false;
    }
    if (document.getElementById("radbtnCorrection").checked == true && document.getElementById("radbtnCorrection").disabled == false) {
        document.getElementById("txtCorrectionToPlan").disabled = false;
        document.getElementById("txtAddendumToPlan").disabled = true;
        $('#txtCorrectionToPlan').removeClass('nonEditabletxtbox');
        $('#txtCorrectionToPlan').addClass('Editabletxtbox');
        $('#txtAddendumToPlan').removeClass('Editabletxtbox');
        $('#txtAddendumToPlan').addClass('nonEditabletxtbox');
        if (document.getElementById("hdnEnableYesNo") != null) {
            document.getElementById("hdnEnableYesNo").value = "true";
        }
        if (document.getElementById("btnMovetoPhyAsst") != null)
            document.getElementById("btnMovetoPhyAsst").disabled = false;

        if (document.getElementById("btnmovetoscribe") != null)
            document.getElementById("btnmovetoscribe").disabled = false;

        if (document.getElementById("btnSave") != null) Close_printDocument
        document.getElementById("btnSave").disabled = true;
        document.getElementById("chkElectronicDeclaration").disabled = true;
        document.getElementById("chkElectronicDeclaration").checked = false;
    }

}
function OnClientSelectedIndexChanged() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}


function onCopyPrevious(errorCode) {

    if (errorCode == "") {
        enableAutoSave();
    }
    else {
        DisplayErrorMessage(errorCode);
        disableAutoSave();
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function enableAutoSave() {
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    $('#btnSave')[0].disabled = false;
}
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnSave')[0].disabled = true;
}
function WindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();

    return false;
}
function Relationshiploadingstop() {
    ChangeRadio();
    ReturnInClick();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function DisplayAll() {

    var Curr_id = event.currentTarget.id;
    var idval = event.currentTarget.id.replace("_ViewMore", "");
    $("li[id^=" + idval + "]").css("display", "block");
    $("li[id=" + Curr_id + "]").css("display", "none");
}
//BugID:48018,48017
function FillEncDataInGenealPLan(EncData) {
    if (EncData.Due_On != "0001-01-01T00:00:00") {
        if (EncData.Return_In_Days == 0 && EncData.Return_In_Months == 0 && EncData.Return_In_Weeks == 0) {
            var duedate = new Date(EncData.Due_On.substring(0, 11) + EncData.Due_On.split('T')[1].split(':')[0].replace('00', '12') + EncData.Due_On.substring(13, 19));
            var d = duedate.getDate();
            var m = duedate.getMonth();
            var y = duedate.getFullYear();
            var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
            var date = d + "-" + arr[m] + "-" + y;
        }

        if (EncData.Is_PRN == "Y") {
            $('#chkPRN')[0].checked = true;
        }

        else {
            $('#chkPRN')[0].checked = false;
        }


        if (EncData.Is_After_Studies == "Y") {
            $("#chkAfterStudies")[0].checked = true;
        }
        else {
            $("#chkAfterStudies")[0].checked = false;
        }
    }
    else {
        if (EncData.Return_In_Days != 0) {
            $("#txtReturnIn")[0].disabled = false;
            $("#txtRetrunWeeks")[0].disabled = false;
            $("#txtRetrunDays")[0].disabled = false;
            $("#txtReturnIn")[0].value = EncData.Return_In_Months;
            $("#txtRetrunWeeks")[0].value = EncData.Return_In_Weeks;
            $("#txtRetrunDays")[0].value = EncData.Return_In_Days;
            if (EncData.Is_PRN == "Y") {
                $('#chkPRN')[0].checked = true;
            }

            else {
                $('#chkPRN')[0].checked = false;
            }


            if (EncData.Is_After_Studies == "Y") {
                $("#chkAfterStudies")[0].checked = true;
            }
            else {
                $("#chkAfterStudies")[0].checked = false;
            }
        }
        //CAP-1471
        if (currentprocess?.toUpperCase() != "SCRIBE_PROCESS" && currentprocess?.toUpperCase() != "AKIDO_SCRIBE_PROCESS" && currentprocess?.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && currentprocess?.toUpperCase() != "DICTATION_REVIEW" && currentprocess?.toUpperCase() != "CODER_REVIEW_CORRECTION" && currentprocess?.toUpperCase() != "PROVIDER_PROCESS" && currentprocess?.toUpperCase() != "TECHNICIAN_PROCESS" && currentprocess?.toUpperCase() != "PROVIDER_REVIEW_CORRECTION" && EncData.Return_In_Months != 0) //{//CMG Ancilliary
        {

            $("#txtReturnIn")[0].disabled = false;
            $("#txtRetrunWeeks")[0].disabled = false;
            $("#txtRetrunDays")[0].disabled = false;
            $("#txtReturnIn")[0].value = EncData.Return_In_Months;
            $("#txtRetrunWeeks")[0].value = EncData.Return_In_Weeks;
            $("#txtRetrunDays")[0].value = EncData.Return_In_Days;

            if (EncData.Is_PRN == "Y") {
                $('#chkPRN')[0].checked = true;
            }

            else {
                $('#chkPRN')[0].checked = false;
            }


            if (EncData.Is_After_Studies == "Y") {
                $("#chkAfterStudies")[0].checked = true;
            }
            else {
                $("#chkAfterStudies")[0].checked = false;
            }

        }
        if (EncData.Return_In_Weeks != 0) {

            $("#txtReturnIn")[0].disabled = false;
            $("#txtRetrunWeeks")[0].disabled = false;
            $("#txtRetrunDays")[0].disabled = false;
            $("#txtRetrunWeeks")[0].value = EncData.Return_In_Weeks;
            $("#txtReturnIn")[0].value = EncData.Return_In_Months;
            $("#txtRetrunDays")[0].value = EncData.Return_In_Days;


            if (EncData.Is_PRN == "Y") {
                $('#chkPRN')[0].checked = true;
            }

            else {
                $('#chkPRN')[0].checked = false;
            }


            if (EncData.Is_After_Studies == "Y") {
                $("#chkAfterStudies")[0].checked = true;
            }
            else {
                $("#chkAfterStudies")[0].checked = false;
            }
        }
        if (EncData.Return_In_Months != 0) {

            $("#txtReturnIn")[0].disabled = false;
            $("#txtRetrunWeeks")[0].disabled = false;
            $("#txtRetrunDays")[0].disabled = false;
            $("#txtReturnIn")[0].value = EncData.Return_In_Months;
            $("#txtRetrunWeeks")[0].value = EncData.Return_In_Weeks;
            $("#txtRetrunDays")[0].value = EncData.Return_In_Days;

            if (EncData.Is_PRN == "Y") {
                $('#chkPRN')[0].checked = true;
            }

            else {
                $('#chkPRN')[0].checked = false;
            }


            if (EncData.Is_After_Studies == "Y") {
                $("#chkAfterStudies")[0].checked = true;
            }
            else {
                $("#chkAfterStudies")[0].checked = false;
            }
        }

        if (EncData.Follow_Reason_Notes != "") {
            $("#txtNotes")[0].value = EncData.Follow_Reason_Notes;
        }
        if (EncData.Is_Document_Given != "") {
            if (EncData.Is_Document_Given == "Yes") {
                $('#cboIsDocumentGiven')[0].selectedIndex = 1;
            }
            else {
                $('#cboIsDocumentGiven')[0].selectedIndex = 2;
            }
        }


        if (EncData.Proceed_with_Surgery_Planned == "Y") {
            $('#chkSurgeryDeclaration')[0].checked = true;

        }
    }

    if (EncData.Is_PRN == "Y") {
        $('#chkPRN')[0].checked = true;
    }

    else {
        $('#chkPRN')[0].checked = false;
    }


    if (EncData.Is_After_Studies == "Y") {
        $("#chkAfterStudies")[0].checked = true;
    }
    else {
        $("#chkAfterStudies")[0].checked = false;
    }


}

//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
function DisableButtons() {
    if (document.getElementById('btnSave') != null)
        document.getElementById('btnSave').disabled = true;
    if (document.getElementById('btnMovetoPhyAsst') != null)
        document.getElementById('btnMovetoPhyAsst').disabled = true;
    if (document.getElementById("radbtnCorrection").checked == true && document.getElementById("radbtnCorrection").disabled == false) {
        document.getElementById("chkElectronicDeclaration").disabled = true;
        document.getElementById("chkElectronicDeclaration").checked = false;
    }
    $("input[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}