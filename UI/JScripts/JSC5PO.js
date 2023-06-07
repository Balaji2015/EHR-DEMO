window.top.setInterval(function () {
    //CAP 312 cannot read properties of undefined (reading style) SOURCE
    if ($(top.window.document).find("#CheckAlert") != undefined && $(top.window.document).find("#CheckAlert") != null && $(top.window.document).find("#CheckAlert")[0] != undefined && $(top.window.document).find("#CheckAlert")[0] != null)
        $(top.window.document).find("#CheckAlert")[0].style.display = "none";
}, 5000);

var popup;
function btnOK() {
    var Selected = $("input:checked");
    var SelectedObj = [];
    for (var i = 0; i < Selected.length; i++) {
        var Sobj = { 'ICD': $(Selected[0]).parent().next()[0].innerText, 'Description': $(Selected[0]).parent().next().next()[0].innerText };
        SelectedObj.push(Sobj);
    }
}

var EMRPhoneEnounter = "";
function OpenModal(data) {

    var itemValue = data;
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    document.getElementById(GetClientId("hdnLocalDate")).value = utc;
    if (itemValue == "") {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            //DisplayErrorMessage('230006', 'Patient Chart Close');
        }
    }
    if (itemValue == "Logout") {
        StartLoadingImage();
        if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
            if (window.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {

                //DisplayErrorMessage('1100000', 'LogOut');
            }
        }
        document.getElementById(GetClientId("btnlogout")).click();
    }
    if (itemValue == "File" || itemValue == "Appointments" || itemValue == "EMR" || itemValue == "RCM" || itemValue == "Admin" || itemValue == "Patient" || itemValue == "Exchange" || itemValue == "Utilities" || itemValue == "Reports" || itemValue == "Help") {

    }
    if (itemValue == "Open Patient Chart") {
        StartLoadingImage();
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").css("background-color", "rgb(109, 119, 119)");
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").addClass("not-active");
        var obj = new Array();
        obj.push("ScreenName=Open Patient Chart");

        var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
        var WindowName = $find('ctl00_RadWindow2');
        //CAP-278 - Prevent to call add_close method if windowname is null or undefined
        WindowName?.add_close(OnOpendPatientChartClick);
    }
    else if (itemValue.toUpperCase() == "ORDER DASH BOARD") {
        StartLoadingImage();
        var obj = new Array();
        var resultLabDashBoard = openModal("frmLabDashBoard.aspx", 630, 650, obj, "ctl00_ModalWindow");
    }
    else if (itemValue.toUpperCase() == "SCANNED DOCUMENTS") {
        StartLoadingImage();
        var obj = new Array();
        obj.push("AccountNum=" + Humanid);
        var dateonclient = new Date;
        var Tz = (dateonclient.getTimezoneOffset());
        document.cookie = "Tz=" + Tz;
        var resultLabDashBoard = openModal("frmLoadScannedDocuments.aspx", 700, 1225, obj, "ctl00_ModalWindow");
        $find('ctl00_ModalWindow').add_close(function () {
            $('#btnScan')[0].click();
        });

    }

    else if (itemValue == "Manage Problem List") {

        var bCancel = false;
        var dvdialog;
        var vassessment = sessionStorage.getItem("TabAssesment")
        if (vassessment == "ASSESSMENT") {
            var test = $($(top.window.document).find('#ctl00_C5POBody_EncounterContainer')[0].contentDocument).find('.clsIframe').contents()[7].all.namedItem('btnSave');
            if (test != null && test != undefined && test.disabled == false) {
                $(top.window.document).find("body").append("<div id='dvdialog' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
                                  "<p style='font-family: Verdana,Arial,sans-serif; font-size: 13.5px;'>There are unsaved changes.Do you want to save them?</p></div>");
                dvdialog = window.parent.parent.parent.parent.document.getElementsByTagName('div').namedItem('dvdialog');
                $(function () {
                    $(dvdialog).dialog({
                        modal: true,
                        title: "Capella EHR",
                        position: {
                            my: 'left' + " " + 'center',
                            at: 'center' + " " + 'center + 100px'

                        },
                        buttons: {
                            "Yes": function () {
                                $(dvdialog).dialog("close");
                                $(dvdialog).remove();
                                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

                                sessionStorage.setItem("EncAutoSave", "true");
                                if (vassessment == "ASSESSMENT") {

                                    $($(top.window.document).find('#ctl00_C5POBody_EncounterContainer')[0].contentDocument).find('.clsIframe').contents()[7].all.namedItem('btnSave').click();

                                    setTimeout("", 5000)
                                    if (localStorage.getItem("Assauto") == "Y") {
                                        sessionStorage.setItem('TabAssesment', "");
                                        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') != null && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "" && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "0") {
                                            $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
                                            var sPath = ""
                                            sPath = "htmlManageProblemList.html?version=" + sessionStorage.getItem("ScriptVersion");
                                            $(top.window.document).find("#mdldlg")[0].style.width = "1050px";
                                            $(top.window.document).find("#ProcessModal")[0].style.width = "";
                                            $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
                                            $(top.window.document).find("#ModalTitle")[0].textContent = "Manage Problem List";
                                        }
                                        else {
                                            var obj = new Array();
                                            obj.push("ScreenName=Open Patient Chart");

                                            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
                                            var WindowName = $find('ctl00_RadWindow2');
                                            WindowName.add_close(OnClientCloseManageProblemList);
                                        }

                                    }
                                }
                                return;
                            },
                            "No": function () {
                                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                                localStorage.setItem("bSave", "true");
                                sessionStorage.setItem("EncCancel", "false");
                                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                                $(dvdialog).dialog("close");
                                $(dvdialog).remove();
                                sessionStorage.setItem('TabAssesment', "");
                                if (document.getElementById('ctl00_C5POBody_hdnHumanNo') != null && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "" && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "0") {
                                    $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
                                    var sPath = ""
                                    sPath = "htmlManageProblemList.html?version=" + sessionStorage.getItem("ScriptVersion");
                                    $(top.window.document).find("#mdldlg")[0].style.width = "1050px";
                                    $(top.window.document).find("#ProcessModal")[0].style.width = "";
                                    $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
                                    $(top.window.document).find("#ModalTitle")[0].textContent = "Manage Problem List";
                                }
                                else {
                                    var obj = new Array();
                                    obj.push("ScreenName=Open Patient Chart");

                                    var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
                                    var WindowName = $find('ctl00_RadWindow2');
                                    WindowName?.add_close(OnClientCloseManageProblemList);
                                }
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            },
                            "Cancel": function () {
                                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

                                bCancel = true;
                                sessionStorage.setItem("EncCancel", "true");
                                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
                                $(dvdialog).dialog("close");
                                $(dvdialog).remove();
                                //PrevTab.tab('show');
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                return;

                            }
                        }
                    });

                });


            }
            else {
                StartLoadingImage();
                if (document.getElementById('ctl00_C5POBody_hdnHumanNo') != null && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "" && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "0") {
                    $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
                    var sPath = ""
                    sPath = "htmlManageProblemList.html?version=" + sessionStorage.getItem("ScriptVersion");
                    $(top.window.document).find("#mdldlg")[0].style.width = "1050px";
                    $(top.window.document).find("#ProcessModal")[0].style.width = "";
                    $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
                    $(top.window.document).find("#ModalTitle")[0].textContent = "Manage Problem List";
                }
                else {
                    var obj = new Array();
                    obj.push("ScreenName=Open Patient Chart");

                    var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
                    var WindowName = $find('ctl00_RadWindow2');
                    WindowName?.add_close(OnClientCloseManageProblemList);
                }
            }
        }
        else {
            StartLoadingImage();
            if (document.getElementById('ctl00_C5POBody_hdnHumanNo') != null && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "" && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "0") {
                $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
                var sPath = ""
                sPath = "htmlManageProblemList.html?version=" + sessionStorage.getItem("ScriptVersion");
                $(top.window.document).find("#mdldlg")[0].style.width = "1050px";
                $(top.window.document).find("#ProcessModal")[0].style.width = "";
                $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#ModalTitle")[0].textContent = "Manage Problem List";
            }
            else {
                var obj = new Array();
                obj.push("ScreenName=Open Patient Chart");

                var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
                var WindowName = $find('ctl00_RadWindow2');
                WindowName?.add_close(OnClientCloseManageProblemList);
            }
        }
    }
    else if (itemValue == "Potential Diagnosis") {

        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') != null && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "" && document.getElementById('ctl00_C5POBody_hdnHumanNo').value != "0") {
            $(top.window.document).find('#ProcessModal').modal({ backdrop: 'static', keyboard: false }, 'show');
            var sPath = ""
            sPath = "htmlPotentialDiagnosis.html?version=" + sessionStorage.getItem("ScriptVersion");
            $(top.window.document).find("#mdldlg")[0].style.width = "970px";
            $(top.window.document).find("#ProcessModal")[0].style.width = "";
            $(top.window.document).find('#ProcessFrame')[0].contentDocument.location.href = sPath;
            $(top.window.document).find("#ModalTitle")[0].textContent = "Manage Potential Diagnosis";
            $(top.window.document).find('#btnClose').hide();
        }
        else {
            var obj = new Array();
            obj.push("ScreenName=Open Patient Chart");

            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
            var WindowName = $find('ctl00_RadWindow2');
            WindowName?.add_close(OnClientClosePotentialDiagnosis);
        }
    }

    else if (itemValue == "MyQ-Archive") {
        StartLoadingImage();
        var obj = new Array();
        var id = new Object();
        var result = openModal("frmMyQueueArchive.aspx", 660, 1290, obj, "ctl00_ModalWindow");
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    else if (itemValue == "SCHEDULER") {

        StartLoadingImage();
        window.location.href = "frmAppointments.aspx?hdnSourceScreen=Menu";
        return false;
    }
    else if (itemValue == "Block Days and Time") {

        StartLoadingImage();
        var obj = new Array();
        var result = openModal("frmBlockDays.aspx", 540, 1230, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
        WindowName?.add_close(function CloseBlockDays(oWindow, args) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        });
    }
    else if (itemValue == "View Transaction") {
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {

            var Result = openModal("frmFindPatient.aspx", 251, 1200, null, 'ctl00_ModalWindow');
            var WindowName = $find('ctl00_ModalWindow');
            WindowName?.add_close(OnOpenViewTransactionClick);
        }
        else {

            var Humanid = document.getElementById('ctl00_C5POBody_hdnHumanNo').value;
            var obj = new Array();
            obj.push("AccountNum=" + Humanid);
            var result = openModal("frmViewTransaction.aspx", 750, 1255, obj, "ctl00_ModalWindow");
        }
    }
    else if (itemValue == "Add or Update Frequently Used Keywords") {

        var inventoryscreen = itemValue;
        var obj = new Array();
        var result = openModal("frmAddorUpdateFrequentlyUsedKeywords.aspx", 560, 670, obj, "ctl00_ModalWindow");

    }

    else if (itemValue == "Print SuperBill") {

        var result = openModal("frmPrintSuperBill.aspx", 150, 700, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "Find Patient") {
        StartLoadingImage();
        var obj = new Array();
        obj.push("ScreenName=Menu Find Patient");
        var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow2");
        var WindowName = $find('ctl00_RadWindow2');
        WindowName?.add_close(OnClientCloseFindPatient);

    }
    else if (itemValue == "PQRI") {
        StartLoadingImage();

        var obj = new Array();
        var HumanID = new Object();
        var EncounterID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = "";
            document.getElementById(GetClientId("hdnEncounterId")).value = "";
            HumanID = document.getElementById(GetClientId("hdnHumanID")).value;
            EncounterID = document.getElementById(GetClientId("hdnEncounterId")).value;
        }
        else {
            HumanID = document.getElementById(GetClientId("hdnHumanID")).value;
            EncounterID = document.getElementById(GetClientId("hdnEncounterId")).value;
        }
        if (EncounterID == undefined || EncounterID == "" || EncounterID == 0) {
            StopLoadingImage();
            DisplayErrorMessage('110035');
        }
        else {
            obj.push("MyHumanID=" + HumanID);
            obj.push("MyEncounterID=" + EncounterID);
            obj.push("ScreenName=PQRI");
            var pqri = openModal("frmPQRI.aspx", 220, 810, obj, "ct100_ModalWindow");
            var windowName = $find('ct100_ModalWindow');
            windowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
        }
    }
    else if (itemValue == "About") {

        StartLoadingImage();
        var result = openModal("frmMessage.aspx?ScreenMode=About", 150, 450, null, "ctl00_ModalWindow");

    }
    else if (itemValue == "Knowledge Center") {

        StartLoadingImage();
        var result = openModal("Documents//KnowledgeCentre//MACRA Knowledge Center.pdf", 635, 1060, null, "ctl00_ModalWindow");
        StopLoadingImage();
    }
    else if (itemValue == "Manual Result Entry") {

        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName?.add_close(OnClientCloseMRE);

        }
        else {
            obj.push("MyHumanID=" + ID);
            var MRE = openModal("frmManualResultEntry.aspx", 850, 1160, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
        }



    }
    else if (itemValue == "Enter Vitals") {
        StartLoadingImage();
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            var now = new Date();
            nowDate = new Date();
            nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();
            nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
            document.getElementById(GetClientId("hdnSystemTime")).value = nowDate;
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            $find("ctl00_ModalWindow").add_close(OnClientCloseVitals);

        }
        else {


            var nowDate = new Date();
            nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear(); nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
            var obj = new Array();
            obj.push("Date=" + nowDate);


            var result = openModal("frmImportVitals.aspx", 850, 1227, obj, "ctl00_MessageWindow");

            var WindowName = $find('ctl00_MessageWindow');
            WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.None);
        }
    }
    else if (itemValue == "Create Order") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            $find("ctl00_ModalWindow").add_close(OnClientCloseCreateOrder);
        }
        else {
            var result = openModal("frmOrdersPatientBar.aspx", 810, 1237, obj, "ctl00_ModalWindow");
            if (document.getElementById('frmOrdersPatientBar') != null && document.getElementById('frmOrdersPatientBar') != undefined) {
                document.getElementById('frmOrdersPatientBar').style.setProperty("top", "30px");
                document.getElementById('frmOrdersPatientBar').style.setProperty("left", "6px");
            }
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.None);
        }
    }

    else if (itemValue == "PFSH") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_PFSHWindow");
            var WindowName = $find('ctl00_PFSHWindow');
            WindowName?.add_close(OnClientClosePFSH);
        }
        else {
            if ($(top.window.document).find("iframe")[0].contentDocument.URL.indexOf("frmEncounter") > -1) {
                StopLoadingImage();
                DisplayErrorMessage("180051");
            }
            else {
                window.parent.location.href = "frmPatientChart.aspx?HumanID=" + ID + "&ScreenName=PFSH" + "&openingfrom=Menu";
                result = openModal("HtmlPFSH.html?MyHumanID=" + ID + "&openingfrom=Menu", 760, 1060, obj, "ctl00_PFSHWindow");
                var PFSHWindow = $find('ctl00_PFSHWindow');
                PFSHWindow.moveTo(105, 160);
                PFSHWindow.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
            }
        }
    }
    else if (itemValue == "Phone Encounter") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            EMRPhoneEnounter = "Yes";
            var WindowName = $find('ctl00_ModalWindow');
            WindowName?.add_close(OnClientClosePhoneEncounter);
        }
        else {
            obj.push("openingfrom=" + "Menu");
            obj.push("MyHumanID=" + ID);
            var result = openModal("HtmlPhoneEncounter.html", 800, 1230, obj, "ctl00_ModalWindow");

            var Window = GetRadWindow();
            if (Window != undefined && Window != null)
                Window.add_close(function ClosePhoneEnc(oWindow, args) {
                    window.parent.location.href = "frmPatientChart.aspx";
                });
        }
    }

    else if (itemValue == "Lab Exception") {
        StartLoadingImage();
        var obj = new Array();
        var result = openModal("frmLabException.aspx", 900, 1250, obj, "ctl00_ModalWindow");
    }
    else if (itemValue == "Change Password") {
        StartLoadingImage();
        var obj = new Array();
        obj.push("ScreenMode=CHANGE PASSWORD USER");
        var result = openModal("frmChangePassword.aspx", 238, 560, obj, "ctl00_ModalWindow");
        var Window = $find('ctl00_ModalWindow');
        Window.add_close(function OnCloseChangePassword(oWindow, args) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        });


    }
    else if (itemValue == "Feedback for Coding Exception") {
        StartLoadingImage();
        QEncounterID = document.getElementById(GetClientId("hdnQencounterId")).value;
        if (QEncounterID != "") {
            $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
            $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Feedback for Coding Exception";
            $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
            $(top.window.document).find("#TabmdldlgException")[0].style.height = "800px";
            var sPath = ""
            var patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
            sPath = "frmException.aspx?formName=" + "Feedback for Coding Exception" + "&PatientName=" + patientName;
            $(top.window.document).find("#TabExceptionFrame")[0].style.height = "725px";
            $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
            $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
            });
            return false;
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('8511009');
        }
    }


    else if (itemValue == "Create Coding Exception") {
        StartLoadingImage();
        var flag = localStorage.getItem("CodingException");

        QEncounterID = document.getElementById(GetClientId("hdnQencounterId")).value;
        var Physiain = document.getElementById(GetClientId("pnlBarGroupTabs")).value;
        var AddendumID = document.getElementById(GetClientId("hdnAddendumID")).value
        if (QEncounterID != "" && flag != QEncounterID) {
            StopLoadingImage();
            alert(' Coding Exception cannot be raised for the selected encounter! ');
            return false;
        }
        else if (QEncounterID != "") {
            $(top.window.document).find("#TabException").modal({ backdrop: "static", keyboard: false }, 'show');
            $(top.window.document).find("#TabModalExceptionTitle")[0].textContent = "Create Coding Exception";
            $(top.window.document).find("#TabmdldlgException")[0].style.width = "950px";
            $(top.window.document).find("#TabmdldlgException")[0].style.height = "700px";
            var sPath = ""
            var patientName = $("[id*='lblPatientStrip']")[0].innerHTML.split('|')[0].trim();
            sPath = "frmException.aspx?formName=" + "Create Coding Exception" + "&PatientName=" + patientName + "&AddendumID=" + AddendumID;
            $(top.window.document).find("#TabExceptionFrame")[0].style.height = "655px";
            $(top.window.document).find("#TabExceptionFrame")[0].contentDocument.location.href = sPath;
            $(top.window.document).find("#TabException").one("hidden.bs.modal", function (e) {
            });
            return false;
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('8511009');
        }
    }

    else if (itemValue == "View Batch Details") {
        var obj = new Array();
        var result = openModal("frmFindBatch.aspx", 800, 1150, obj, "ctl00_ModalWindow")
        return false;
    }

    else if (itemValue == "View Patient Charges") {

        return false;
    }
    else if (itemValue == "View Coding History") {
        var result = openModal("frmCodingHistory.aspx", 750, 1100, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "View Claims") {
        var result = openModal("frmViewClaims.aspx", 750, 1100, null, "ctl00_ModalWindow");
        return false;
    }

    else if (itemValue == "Patient Communication") {
        StartLoadingImage();

        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            var Result = openModal("frmFindPatient.aspx", 251, 1200, null, 'ctl00_ModalWindow');
            var WindowName = $find('ctl00_ModalWindow');

            WindowName?.add_close(OnOpenPatientCommunicationClick);


        }
        else {
            var obj = new Array();
            obj.push("IsMYQ=" + "N");
            var result = openModal("frmPatientCommunication.aspx", 810, 1050, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName?.add_close(OnClosePatientCommunicationClick);
            return false;

        }
    }
    else if (itemValue == "Patient Documents") {

        $(top.window.document).find("#TabPatientDocuments").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalPatientDocumentsTitle")[0].textContent = "Patient Documents";
        $(top.window.document).find("#TabmdldlgPatientDocuments")[0].style.width = "400px";
        $(top.window.document).find("#TabmdldlgPatientDocuments")[0].style.height = "600px";

        var sPath = ""
        sPath = "frmPatientDocuments.aspx";
        $(top.window.document).find('#TabPatientDocumentsFrame')[0].src = sPath;
        $($($(top.window.document).find('#TabPatientDocuments')).find('#TabmdldlgPatientDocuments')).find('.modal-content').css('overflow-y', 'auto');

    }
    else if (itemValue == "View Patient Task") {
        StartLoadingImage();
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            var Result = openModal("frmFindPatient.aspx", 251, 1200, null, 'ctl00_ModalWindow');
            var WindowName = $find('ctl00_ModalWindow');

            WindowName?.add_close(OnOpenViewPatientTaskClick);

        }

        else {
            var Humanid = document.getElementById('ctl00_C5POBody_hdnHumanNo').value;
            var obj = new Array();
            obj.push("AccountNum=" + Humanid);
            var result = openModal("frmViewPatientTask.aspx", 1000, 1100, obj, "ctl00_ModalWindow");

        }
    }
    else if (itemValue == "ACO") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowNam?.add_close(OnClientCloseACO);
        }
        else {
            $.ajax({
                type: "POST",
                url: "frmRCopiaToolbar.aspx/CheckACOEligiblity",
                data: "{'strHumanID':'" + ID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    document.getElementById(GetClientId("hdnAcoEligibility")).value = response.d;
                    var ACOEligibility = document.getElementById(GetClientId("hdnAcoEligibility")).value;
                    if (ACOEligibility == "Y" || ACOEligibility == "ACO") {
                        var result = openModal("frmACOValidation.aspx", 150, 550, obj, "ctl00_ModalWindow");
                    }
                    else {
                        StopLoadingImage();
                        DisplayErrorMessage('1091010');
                    }
                },
                error: function OnError(xhr) {
                    StopLoadingImage();
                    if (xhr.status == 999)
                        window.location = xhr.statusText;
                    else {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                    }
                }
            });



        }
    }
    else if (itemValue == "Patient Payments Account") {
        StartLoadingImage();

        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            var Result = openModal("frmFindPatient.aspx", 251, 1200, null, 'ctl00_ModalWindow');
            var WindowName = $find('ctl00_ModalWindow');

            WindowName?.add_close(OpenPatientPayementsAccountClick);


        }
        else {
            var obj = new Array();
            obj.push("IsMYQ=" + "N");
            //var result = openModal("frmPatientPayment.aspx", 450, 1300, obj, "ctl00_ModalWindow");
            //var result = openModal("frmPatientPayment.aspx", 512, 1047, obj, "ctl00_ModalWindow");
            var result = openModal("frmPatientPayment.aspx", 450, 1047, obj, "ctl00_ModalWindow");

            var WindowName = $find('ctl00_ModalWindow');
            WindowName?.add_close(OnClosePatientPayementsAccountClick);
            return false;

        }
    }

    else if (itemValue == "View Patient Messages") {
        StartLoadingImage();
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            var Result = openModal("frmFindPatient.aspx", 251, 1200, null, 'ctl00_ModalWindow');
            var WindowName = $find('ctl00_ModalWindow');

            WindowName?.add_close(OnOpenViewMessageClick);

        }
        else {
            var Humanid = document.getElementById('ctl00_C5POBody_hdnHumanNo').value;
            var obj = new Array();
            obj.push("AccountNum=" + HumanID);
            var result = openModal("frmViewMessage.aspx", 500, 1210, null, "ctl00_ModalWindow");
            return false;
        }
    }

    else if (itemValue == "Mail box") {
        StartLoadingImage();
        var obj = new Array();
        obj.push("Role=" + "Provider");
        var result = openModal("frmMailBox.aspx", 600, 900, obj, "ctl00_ModalWindow");
        return false;
    }



    else if (itemValue == "EFax") {
        localStorage.setItem("IsMenuEFax", "Y");
        $(top.window.document).find("#TabFax").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalEFaxTitle")[0].textContent = "Efax";
        $(top.window.document).find("#TabmdldlgEFax")[0].style.width = "1050px";
        $(top.window.document).find("#TabmdldlgEFax")[0].style.height = "715px";
        var sPath = ""
        sPath = "htmlEFAXTabs.html";
        $(top.window.document).find("#TabEFaxFrame")[0].style.height = "500px";
        $(top.window.document).find("#TabEFaxFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabFax").one("hidden.bs.modal", function (e) {
        });



    }
    else if (itemValue == "Reset Password") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmResetPassword.aspx", 40, 350, obj, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "Medicare Fee Schedule") {
        args.set_cancel(true);
        var result = openModal("frmMedicareFeeSchedule.aspx", 135, 1020, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "Flat Rate") {
        var result = openModal("frmNonMedicareFeeSchedule.aspx", 580, 1150, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "Charge Master Rules") {
        var result = openModal("frmAddEditRules.aspx", 588, 1040, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "Confidential Morbidity Report") {
        StartLoadingImage();
        var obj = new Array();
        obj.push("SI=" + "Confidential Morbidity Report");
        obj.push("Location=" + "STATIC");
        var result = openModal("frmPrintPDF.aspx", 750, 900, obj, "ctl00_ModalWindow");

    }
    else if (itemValue == "Print Demographics Sheet") {
        var obj = new Array();
        obj.push("SI=" + "Demo_Sheet");
        obj.push("Location=" + "STATIC");
        var result = openModal("frmPrintPDF.aspx", 750, 900, obj, "ctl00_ModalWindow");

    }
    else if (itemValue == "Find Authorization") {

        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            {
                var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.add_close(OnOpenFindAuthorizationClick);

            }

        }
        else {
            var obj = new Array();
            obj.push("AccNo=" + Result.HumanId);
            var Humanid = document.getElementById('ctl00_C5POBody_hdnHumanNo').value;
            var result = openModal("frmFindAuthorization.aspx", 500, 1350, obj, "ctl00_ModalWindow")

        }
    }
    else if (itemValue == "Demographics") {
        StartLoadingImage();
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            var Result = openModal("frmFindPatient.aspx", 251, 1200, null, 'ctl00_ModalWindow');
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnOpenDemographicsClick);


        }
        else {

            var Humanid = document.getElementById('ctl00_C5POBody_hdnHumanNo').value;
            var obj = new Array();
            obj.push("HumanId=" + Humanid);
            obj.push("ScreenName=Demographics");
            var result = openModal("frmPatientDemographics.aspx", 1230, 1182, obj, "ctl00_ModalWindow");
            //audit log entry for Encounter based Access
            CreateAuditLogEntryForTransactions("ACCESS", "Human", Humanid);//BugID:49685
            var Window = $find('ctl00_ModalWindow');
            Window.add_close(function CloseDemoGraphics(oWindow, args) {
                window.location.href = "frmPatientChart.aspx"
            });
        }
    }
    else if (itemValue == "Manage Authorization") {
        localStorage.setItem("IsEdit", "false");
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            {
                var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.add_close(OnOpenAuthorizationClick);
            }
        }
        else {
            var result = openModal("frmAuthorization.aspx", 800, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
        }
    }

    else if (itemValue == "Perform EV") {
        localStorage.setItem("IsEdit", "false");
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            {
                window.setTimeout(function () {
                    var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_RadWindow1");
                    var evWindowname = $find('ctl00_RadWindow1');
                    evWindowname.add_close(OnOpenPerformEVClick);
                }, 50);
            }
        }
        else {
            var obj = new Array();
            var result = openModalPerformEV("frmPerformEV.aspx", 620, 1200, obj, "ctl00_RadWindow1");
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        }
    }
    else if (itemValue == "EV Summary") {
        if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
            {
                var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.add_close(OnOpenEVSummaryClick);
            }
        }
        else {
            var obj = new Array();
            var Humanid = document.getElementById('ctl00_C5POBody_hdnHumanNo').value;
            obj.push("EncounterID=0");
            obj.push("humanID=" + Humanid);
            obj.push("EncStatus=''");
            obj.push("bShowPat=true");
            obj.push("sScreenMode=EVSUMMARY");
            openModal("frmQuickpatientcreate.aspx", 730, 1020, obj, "ctl00_ModalWindow");
        }
    }

    else if (itemValue == "Allocate") {
        var obj = new Array();
        obj.push("ScreenName=ALLOCATE");
        var result = openModal("frmAllocate.aspx", 560, 1250, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.add_close(function TimeSlotFindAllAppointmentsClick(oWindow, args) {

            window.location.href = window.location.href;
            var BrowserName = "";
            BrowserName = navigator.appName;
            var browser = navigator.userAgent;
            if (browser.indexOf("Chrome") != -1) {
                window.opener.location.reload();
            }
            if (BrowserName == "Microsoft Internet Explorer") {
                window.location.reload(true);
            }
            else if (browser.indexOf("Firefox") != -1) {
                window.opener.location.reload();
            }
        });

    }
    else if (itemValue == "Allocate Exception/Call") {
        var inventoryscreen = itemValue;
        var obj = new Array();
        obj.push("ScreenName=" + itemValue);
        var result = openModal("frmAllocateExceptionCall.aspx", 600, 1260, obj, "ctl00_ModalWindow");
    }
    else if (itemValue == "Edit Allocate Exception/Call") {
        var inventoryscreen = itemValue;
        var obj = new Array();
        obj.push("ScreenName=" + itemValue);
        var result = openModal("frmAllocateExceptionCall.aspx", 600, 1260, obj, "ctl00_ModalWindow");
    }

    else if (itemValue == "Load") {
        var result = openModal("frmLoadFiles.aspx", 630, 1250, null, "ctl00_ModalWindow");

    }
    else if (itemValue == "Inventory") {
        var inventoryscreen = itemValue;
        var obj = new Array();
        obj.push("inventoryscreen=" + itemValue);
        var result = openModal("frmInventory.aspx", 530, 1260, obj, "ctl00_ModalWindow");

    }
    else if (itemValue == "Inventory Review") {
        var inventoryscreen = itemValue;
        var obj = new Array();
        obj.push("inventoryscreen=" + itemValue);
        var result = openModal("frmInventory.aspx", 660, 1260, obj, "ctl00_ModalWindow");
    }

    else if (itemValue == "Update Inventory") {
        var result = openModal("frmUpdateInventory.aspx", 600, 1260, null, "ctl00_ModalWindow");
    }
    else if (itemValue == "Update Payment Inventory") {
        var result = openModal("frmUpdatePaymentPostingInventory.aspx", 720, 1050, null, "ctl00_ModalWindow");
    }
    else if (itemValue == "Edit Allocate") {
        var result = openModal("frmAllocate.aspx?ScreenName=EDIT ALLOCATE", 560, 1250, null, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.add_close(function TimeSlotFindAllAppointmentsClick(oWindow, args) {

            window.location.href = window.location.href;
            var BrowserName = "";
            BrowserName = navigator.appName;
            var browser = navigator.userAgent;
            if (browser.indexOf("Chrome") != -1) {
                window.opener.location.reload();
            }
            if (BrowserName == "Microsoft Internet Explorer") {
                window.location.reload(true);
            }
            else if (browser.indexOf("Firefox") != -1) {
                window.opener.location.reload();
            }
        });

    }

    else if (itemValue == "Add EOB") {
        var result = openModal("frmAddEOB.aspx", 660, 1020, null, "ctl00_ModalWindow");
        return false;
    }

    else if (itemValue == "Create Sub Batch") {
        var result = openModal("frmCreateSubBatches.aspx", 650, 800, null, "ctl00_ModalWindowCreateSubBatch");
        return false;
    }

    else if (itemValue == "Patient Reminder") {
        StartLoadingImage();
        var MRE = openModal("frmCreatePatientRemainder.aspx", 750, 950, null, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.set_behaviors();
        WindowName.add_beforeClose(OnClientCloseCreateReminderAutoSave);
        return false;
    }

    else if (itemValue == "Convert to ICD 10") {
        var result = openModal("frmICD10Conversion.aspx?sourceScreen=UTILITIES", 570, 890, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "E-Prescription") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ERXWindow");
            $find('ctl00_ERXWindow').add_close(OnClientCloseEPRES);
        }
        else {
            OpenERXFromMENU();
        }

    }
    else if (itemValue == "Order Management") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmOrderManagement.aspx", 670, 1200, obj, "ctl00_OrderManagement");
        var WindowName = $find('ctl00_OrderManagement');
        WindowName.add_close(OnClientCloseOrderManagement);
        return false;
    }

    else if (itemValue.toUpperCase() == "EHR MEASURE CALCULATION") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmEHRMeasureCalculation.aspx", 750, 980, obj, "ctl00_ModalWindow");
        return false;
    }
        //BugID:49829
    else if (itemValue.toUpperCase() == "PATIENT LIST") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmGeneratePatientLists.aspx", 750, 1180, obj, "ctl00_ModalWindow");
        return false;
    }

    else if (itemValue.toUpperCase() == "PQRI MEASURE") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmPQRIMeasure.aspx", 770, 1240, obj, "ctl00_ModalWindow");
        return false;
    }

    else if (itemValue.toUpperCase() == "REPORT GENERATOR") {

        var cookies = document.cookie.split(';');
        var CUserRole = "";
        var CUserName = "";
        var CFacilityName = "";
        var sCurrPhyId = "";
        var CLegalOrg = "";
        var CUserCarrier = "";
        var re = /%20/gi;
        for (var l = 0; l < cookies.length; l++) {
            if (cookies[l].indexOf("CUserRole") > -1)
                CUserRole = cookies[l].split("=")[1];
            if (cookies[l].indexOf("CUserName") > -1)
                CUserName = cookies[l].split("=")[1];
            if (cookies[l].indexOf("CFacilityName") > -1)
                CFacilityName = cookies[l].split("=")[1];
            if (cookies[l].indexOf("CurrPhyId") > -1)
                sCurrPhyId = cookies[l].split("=")[1];
            if (cookies[l].indexOf("CLegalOrg") > -1)
                CLegalOrg = cookies[l].split("=")[1];
            if (cookies[l].indexOf("CUserCarrier") > -1)
                CUserCarrier = cookies[l].split("=")[1];
        }
        CFacilityName = CFacilityName.replace(re, ' ');

        $(top.window.document).find('#ReportModal').modal({ backdrop: 'static', keyboard: false }, 'show');

        var sPath = ""
        var veportpath = "";

        if (window.location.protocol == "https:") {
            veportpath = sessionStorage.getItem("ReportPath");
        }
        else {
            veportpath = sessionStorage.getItem("ReportPathhttp");
        }
        sPath = veportpath + "htmlReportGenerator.htm?CUserRole=" + CUserRole + "&CUserName=" + CUserName + "&CFacilityName=" + CFacilityName + "&ProjectType=" + CLegalOrg + "&CurrPhyId=" + sCurrPhyId + "&UserCarrier=" + CUserCarrier;
        //sPath = veportpath + "htmlReportGenerator.htm?CUserRole=" + CUserRole + "&CUserName=" + CUserName + "&CFacilityName=" + CFacilityName + "&ProjectType=" + sessionStorage.getItem("Projname") + "&CurrPhyId=" + sCurrPhyId;
        //sPath = sessionStorage.getItem("ReportPath") + "htmlReportGenerator.htm?CUserRole=" + CUserRole + "&CUserName=" + CUserName + "&CFacilityName=" + CFacilityName + "&ProjectType=" + sessionStorage.getItem("Projname") + "&CurrPhyId=" + sCurrPhyId;

        $(top.window.document).find('#ReportFrame')[0].src = sPath;
        $(top.window.document).find("#ReportModalTitle")[0].textContent = "Report Generator";
        $($($(top.window.document).find('#ReportModal ')).find('#ReportMdlDlg')).find('.modal-content').css('overflow-y', 'auto');

    }
    else if (itemValue.toUpperCase() == "OFFICE MANAGER QUEUE") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmOfficeManagerQueue.aspx", 820, 1140, obj, "ctl00_ModalWindow");
        return false;
    }

    else if (itemValue.toUpperCase() == "MANAGE CDS") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmManageCDSS.aspx", 620, 450, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_ModalWindow');
        WindowName.set_behaviors();
        return false;
    }
    else if (itemValue.toUpperCase() == "CARRIER LIBRARY") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmSelectPayer.aspx", 480, 880, obj, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue.toUpperCase() == "PROVIDER LIBRARY") {
        localStorage.removeItem("IsEFax");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        localStorage.setItem("IsEnableGrid", "true");
        $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
        $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
        $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "95%";
        var sPath = "frmPhysicianLibray.aspx";
        $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "95%";
        $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabPhysicianLibrary").modal("show");
        $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
        });
        return false;
    }
    else if (itemValue.toUpperCase() == "PLAN LIBRARY") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmPayerLibrary.aspx", 720, 1050, obj, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue.toUpperCase() == "INTEGRITY") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmIntegrity.aspx", 230, 780, obj, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue.toUpperCase() == "ENCRYPTION") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmEncryption.aspx", 230, 750, obj, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue.toUpperCase() == "DECRYPTION") {
        StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmDecryption.aspx", 230, 750, obj, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue.toUpperCase() == "UPDATE PROCESS\\OWNER") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnClientCloseAdminModule);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var Result = openModal("frmWFObjectManager.aspx", 660, 1100, obj, "ctl00_ModalWindow");
        }
    }
    else if (itemValue.toUpperCase() == "WELLNESS NOTES") {

        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
            StartLoadingImage();
            $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=WELLNESS NOTES" + "&Menu=True";
            $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Wellness Notes";
            var DateTime = new Date();
            var strYear = DateTime.getFullYear();
            var strMonth = DateTime.getMonth() + 1;
            var strDay = DateTime.getDate();
            var strHours = DateTime.getHours();
            var strMinutes = DateTime.getMinutes();
            var strSeconds = DateTime.getSeconds();
            if (strMonth.toString().length == 1)
                strMonth = "0" + strMonth;
            if (strDay.toString().length == 1)
                strDay = "0" + strDay;
            if (strMinutes.toString().length == 1)
                strMinutes = "0" + strMinutes;
            var testStieng = strHours.toString() + ":" + strMinutes.toString() + ":" + strSeconds.toString();
            var timeString = testStieng.toString();
            var H = +timeString.substr(0, 2);
            var h = H % 12 || 12;
            var ampm = H < 12 ? "AM" : "PM";
            if (h.toString().length == 1)
                h = "0" + h;
            timeString = h + timeString.substr(2, 6) + ampm;
            document.getElementById(GetClientId("hdnLocalTime")).value = strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", "");
            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId("hdnLocalTime")).value);//document.getElementById(GetClientId("hdnLocalTime")).value
            StopLoadingImage();
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('110063')
        }

    } else if (itemValue == "Care Note") {
        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
            StartLoadingImage();
            $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=CARE NOTE" + "&Menu=True";
            $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Care Notes";
            var DateTime = new Date();
            var strYear = DateTime.getFullYear();
            var strMonth = DateTime.getMonth() + 1;
            var strDay = DateTime.getDate();
            var strHours = DateTime.getHours();
            var strMinutes = DateTime.getMinutes();
            var strSeconds = DateTime.getSeconds();
            if (strMonth.toString().length == 1)
                strMonth = "0" + strMonth;
            if (strDay.toString().length == 1)
                strDay = "0" + strDay;
            if (strMinutes.toString().length == 1)
                strMinutes = "0" + strMinutes;
            var testStieng = strHours.toString() + ":" + strMinutes.toString() + ":" + strSeconds.toString();
            var timeString = testStieng.toString();
            var H = +timeString.substr(0, 2);
            var h = H % 12 || 12;
            var ampm = H < 12 ? "AM" : "PM";
            if (h.toString().length == 1)
                h = "0" + h;
            timeString = h + timeString.substr(2, 6) + ampm;
            document.getElementById(GetClientId("hdnLocalTime")).value = strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", "");
            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId("hdnLocalTime")).value);//document.getElementById(GetClientId("hdnLocalTime")).value

            StopLoadingImage();
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('110063')
        }
    }
    else if (itemValue == "Treatment Notes") {
        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
            StartLoadingImage();
            $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmWellnessNotes.aspx?SubMenuName=TREATMENT NOTES" + "&Menu=True";
            $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Treatment Notes";
            var DateTime = new Date();
            var strYear = DateTime.getFullYear();
            var strMonth = DateTime.getMonth() + 1;
            var strDay = DateTime.getDate();
            var strHours = DateTime.getHours();
            var strMinutes = DateTime.getMinutes();
            var strSeconds = DateTime.getSeconds();
            if (strMonth.toString().length == 1)
                strMonth = "0" + strMonth;
            if (strDay.toString().length == 1)
                strDay = "0" + strDay;
            if (strMinutes.toString().length == 1)
                strMinutes = "0" + strMinutes;
            var testStieng = strHours.toString() + ":" + strMinutes.toString() + ":" + strSeconds.toString();
            var timeString = testStieng.toString();
            var H = +timeString.substr(0, 2);
            var h = H % 12 || 12;
            var ampm = H < 12 ? "AM" : "PM";
            if (h.toString().length == 1)
                h = "0" + h;
            timeString = h + timeString.substr(2, 6) + ampm;
            document.getElementById(GetClientId("hdnLocalTime")).value = strYear + "" + strMonth + "" + strDay + " " + timeString.replace(":", "").replace(":", "");
            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId("hdnLocalTime")).value);//document.getElementById(GetClientId("hdnLocalTime")).value

            StopLoadingImage();
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('110063')
        }
    }

    else if (itemValue == "Report an Error") {
        var result = openModal("frmErrorReport.aspx", 810, 1090, null, "ctl00_ModalWindow");
    }
    else if (itemValue == "Export or Print Claims") {
        var result = openModal("frmElectronicClaims.aspx", 935, 1250, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "View Claims") {
        var result = openModal("frmViewClaims.aspx", 660, 650, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue == "Patient Reminder Report") {
        StartLoadingImage();
        var result = openModal("frmPatientReminderReport.aspx", 620, 1200, null, "ctl00_ModalWindow");
        return false;
    }
    else if (itemValue.toUpperCase() == "CLINICAL SUMMARY") {

        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            DisplayErrorMessage('110059');
        }
        else {
            var obj = new Array();
            obj.push("formName=" + "Clinical Summary");
            var result = openModal("frmClinicalSummary.aspx", 400, 750, obj, "ctl00_ModalWindow");
        }
    }
    else if (itemValue.toUpperCase() == "IMPORT") {
        StartLoadingImage();

        var obj = new Array();
        obj.push("DialogMode=" + "Open");
        var result = openModal("frmBrowse.aspx", 500, 687, obj, "ctl00_ModalWindow");

        var obj = new Array();
        obj.push("DialogMode=" + "Open");
        var result = openModal("frmBrowse.aspx", 500, 687, obj, "ctl00_ModalWindow");

        var Window = $find('ctl00_ModalWindow');
        Window.set_behaviors(Telerik.Web.UI.WindowBehaviors.None)

    }
    else if (itemValue.toUpperCase() == "BULK EXPORT") {
        StartLoadingImage();
        var obj = new Array();
        var result = openModal("frmBulkExport.aspx", 610, 700, obj, "ctl00_ModalWindow");
    }
    else if (itemValue.toUpperCase() == "EXPORT") {
        StartLoadingImage();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            var result = openModal("frmPatientdata.aspx", 450, 930, null, "ctl00_ModalWindow");
            return false;
        }
        else if (document.getElementById(GetClientId("hdnEncounterId")) == null || document.getElementById(GetClientId("hdnEncounterId")).value == "" || document.getElementById(GetClientId("hdnEncounterId")).value == "0") {
            StopLoadingImage();
            DisplayErrorMessage("110069");
        }
        else {
            var obj = new Array();
            obj.push("formName=" + "Clinical Summary");
            var result = openModal("frmClinicalSummary.aspx", 520, 1000, obj, "ctl00_ModalWindow");
        }
    }


    else if (itemValue.toUpperCase() == "PROGRESS NOTES") {

        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {

            StartLoadingImage();
            $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmSummaryNew.aspx?Menu=PDF";
            $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Progress Notes";


            StopLoadingImage();
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('110063')
        }

    }
    else if (itemValue.toUpperCase() == "PROGRESS NOTES PDF") {

        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {

            StartLoadingImage();
            $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmSummaryNew.aspx?Menu=PDF";
            $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Progress Notes";


            StopLoadingImage();
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('110063')
        }

    }
    else if (itemValue.toUpperCase() == "PROGRESS NOTES FAX") {

        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {

            StartLoadingImage();
            $(top.window.document).find('#ProcessiFrameNotes')[0].contentDocument.location.href = "frmSummaryNew.aspx?Menu=FAX";
            $(top.window.document).find("#ModalTtleNotes")[0].textContent = "Progress Notes";


            StopLoadingImage();
        }
        else {
            StopLoadingImage();
            DisplayErrorMessage('110063')
        }

    }
    else if (itemValue.toUpperCase() == "CONSULTATION NOTES") {
        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {

            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId('hdnLocalTime')).value);
            obj.push("Menu=" + "PDF");
            var objACO = openModalProgress("frmConsultationNotes.aspx", 500, 500, obj, "ctl00_ModalWindow");
            var win = $find('ctl00_ModalWindow');
            win.hide();
        }
        else {
            DisplayErrorMessage('110063')
        }

    }
    else if (itemValue.toUpperCase() == "CONSULTATION NOTES PDF") {
        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {


            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId('hdnLocalTime')).value);
            obj.push("Menu=" + "PDF");
            var objACO = openModalProgress("frmConsultationNotes.aspx", 500, 500, obj, "ctl00_ModalWindow");
            var win = $find('ctl00_ModalWindow');
            win.hide();
        }
        else {
            DisplayErrorMessage('110063')
        }

    }
    else if (itemValue.toUpperCase() == "CONSULTATION NOTES FAX") {
        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {

            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId('hdnLocalTime')).value);
            obj.push("Menu=" + "FAX");
            var objACO = openModalProgress("frmConsultationNotes.aspx", 500, 500, obj, "ctl00_ModalWindow");
            var win = $find('ctl00_ModalWindow');
            win.hide();
        }
        else {
            DisplayErrorMessage('110063')
        }

    }
    else if (itemValue == "Spiritual Notes") {
        if (document.getElementById(GetClientId("hdnEncounterId")) != null && document.getElementById(GetClientId("hdnEncounterId")).value != "" && document.getElementById(GetClientId("hdnEncounterId")).value != "0") {
            var obj = new Array();
            obj.push("Date=" + document.getElementById(GetClientId('hdnLocalTime')).value);
            var objACO = openModalProgress("frmSpiritualNotes.aspx", 500, 500, obj, "ctl00_ModalWindow");
            var win = $find('ctl00_ModalWindow');
            win.hide();
            top.document.getElementsByClassName("TelerikModalOverlay")[0].attributes[2].value = "display: none;";
        }
        else {
            DisplayErrorMessage('110063')
        }
    }

    else if (itemValue == "Clinical Reconciliation") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = ""
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnClientCloseOpenReconcillation);
        }
        else {
            var Result = openModal("frmClinicalInformation.aspx", 900, 1240, obj, "ctl00_ModalWindow");

            var Window = GetRadWindow();
            Window.add_close(function CloseClinicalReconc(oWindow, args) {
                window.parent.location.href = "frmPatientChart.aspx";
            });
        }


    }
    else if (itemValue == "Amendment") {
        StartLoadingImage();
        var obj = new Array();
        var ID = new Object();
        if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
            document.getElementById(GetClientId("hdnHumanID")).value = "";
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }
        else {
            ID = document.getElementById(GetClientId("hdnHumanID")).value;
        }


        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/OpenAmendment",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var result = response.d;
                if (result != "") {
                    if (result == "OpenAddendumForm") {
                        openModal("frmAddendum.aspx?", 477, 1134, null, "ctl00_ModalWindow");
                    }
                    else {
                        StopLoadingImage();
                        DisplayErrorMessage(result);
                    }
                }
            },
            error: function OnError(xhr) {
                StopLoadingImage();
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }
            }
        });

    }
    else if (itemValue.toUpperCase() == "IMMUNIZATION REGISTRIES") {
        if (document.getElementById(GetClientId("hdnEncounterId")) == null || document.getElementById(GetClientId("hdnEncounterId")).value == "" || document.getElementById(GetClientId("hdnEncounterId")).value == "0")
            DisplayErrorMessage("110068");
        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=ImmunizationRegistry", 520, 665, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=ImmunizationRegistry", 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "IMMUNIZATION BULK EXPORT") {
        var objACO = openModalProgress("frmImmunizationBulkExport.aspx", 320, 520, null, "ctl00_ModalWindow");
    }
    else if (itemValue.toUpperCase() == "IMMUNIZATION REGISTRIES QUERY") {
        var ID = new Object();
        ID = document.getElementById(GetClientId("hdnHumanID")).value;

        if (ID == undefined || ID == "") {
            StartLoadingImage();

            var obj = new Array();
            obj.push("ScreenName=Open Patient Chart");

            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnImmunizationRegistryQueryClick);
        }

        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=ImmunizationRegistryQuery" + "&HumanID=" + ID, 520, 1090, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=ImmunizationRegistryQuery" + "&HumanID=" + ID, 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "IMMUNIZATION REGISTRIES QUERY HISTORY") {
        var ID = new Object();
        ID = document.getElementById(GetClientId("hdnHumanID")).value;

        if (ID == undefined || ID == "") {
            StartLoadingImage();

            var obj = new Array();
            obj.push("ScreenName=Open Patient Chart");

            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnImmunizationRegistryQueryHistoryClick);
        }
        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=ImmunizationRegistryQueryHistory" + "&HumanID=" + ID, 520, 1090, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=ImmunizationRegistryQueryHistory" + "&HumanID=" + ID, 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "SYNDROMIC SURVEILLANCE|ADMIT / VISIT NOTIFICATION") {
        if (document.getElementById(GetClientId("hdnEncounterId")) == null || document.getElementById(GetClientId("hdnEncounterId")).value == "" || document.getElementById(GetClientId("hdnEncounterId")).value == "0")
            DisplayErrorMessage("110067");
        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=SyndromicSurveillance|ADMIT / VISIT NOTIFICATION", 520, 665, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=SyndromicSurveillance|ADMIT / VISIT NOTIFICATION WITH ACKNOWLEDGEMENT", 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "SYNDROMIC SURVEILLANCE|DISCHARGE / END VISIT") {
        if (document.getElementById(GetClientId("hdnEncounterId")) == null || document.getElementById(GetClientId("hdnEncounterId")).value == "" || document.getElementById(GetClientId("hdnEncounterId")).value == "0")
            DisplayErrorMessage("110067");
        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=SyndromicSurveillance|DISCHARGE / END VISIT", 520, 665, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=SyndromicSurveillance|DISCHARGE / END VISIT WITH ACKNOWLEDGEMENT", 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "SYNDROMIC SURVEILLANCE|REGISTER PATIENT") {
        if (document.getElementById(GetClientId("hdnEncounterId")) == null || document.getElementById(GetClientId("hdnEncounterId")).value == "" || document.getElementById(GetClientId("hdnEncounterId")).value == "0")
            DisplayErrorMessage("110067");
        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=SyndromicSurveillance|REGISTER PATIENT", 520, 665, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=SyndromicSurveillance|REGISTER PATIENT WITH ACKNOWLEDGEMENT", 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "SYNDROMIC SURVEILLANCE|UPDATE PATIENT INFORMATION") {
        if (document.getElementById(GetClientId("hdnEncounterId")) == null || document.getElementById(GetClientId("hdnEncounterId")).value == "" || document.getElementById(GetClientId("hdnEncounterId")).value == "0")
            DisplayErrorMessage("110067");
        else {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=SyndromicSurveillance|UPDATE PATIENT INFORMATION", 520, 665, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=SyndromicSurveillance|UPDATE PATIENT INFORMATION WITH ACKNOWLEDGEMENT", 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
    else if (itemValue.toUpperCase() == "IMPORT CAT FILE") {
        //StartLoadingImage();
        var obj = new Array();
        var Result = openModal("frmImportCQM.aspx", 201, 850, obj, "");
        
        
    }

}


function GetClientId(strid) {
    var count = document.forms[0].length; var i = 0; var eleName; for (i = 0; i < count; i++)
    { eleName = document.forms[0].elements[i].id; pos = eleName.indexOf(strid); if (pos >= 0) break; }
    return eleName;
}
function OpenPatientTask() {


    var obj = new Array();

    var result = openModal("frmViewPatientTask.aspx", 1100, 1050, obj, "ctl00_ModalWindow");


}

function openLoadScanDocuments() {
    var oWnd = radopen("frmLoadScannedDocuments.aspx", "RadWindow1");

}

function OfficeManagerQueue() {
    args.set_cancel(true);
    var obj = new Array();
    var Result = openModal("frmOfficeManagerQueue.aspx", 1520, 1050, obj, "ctl00_ModalWindow");
    return false;
}
function UpdateProcessOwner() {
    args.set_cancel(true);
    var obj = new Array();
    var Result = openModal("frmWFObjectManager.aspx", 550, 1070, obj, "ctl00_ModalWindow");
    return false;
}
function OpenERX(MyType) {
    var obj = new Array();
    obj.push("MyType=" + MyType);
    obj.push("HumanID=" + "0");
    obj.push("EncID=" + "0");
    obj.push("PrescriptionID=" + "0");
    obj.push("IsMoveButton=" + "false");
    obj.push("IsMoveCheckbox=" + "false");
    obj.push("IsPrescriptiontobePushed=" + "N");
    obj.push("openingFrom=" + "Menu");
    obj.push("IsSentToRCopia=" + "Y");
    obj.push("LocalTime=" + document.getElementById(GetClientId('hdnLocalTime')).value);

    var result = openModal("frmRCopiaWebBrowser.aspx", 1100, 960, obj, "ctl00_ModalWindow");
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
    WindowName.add_close(LoadRcopiaCount);//BugID:54514
    return false;
}

function OnClientButtonClicked(sender, args) {
    GetUTCTime();

    if (args._item.get_value() == "tsRcopiarefill") {
        OpenERX('MESSAGE');
        args.set_cancel(true);
    }
    else if (args._item.get_value() == "tsRcopiarx_pending") {
        OpenERX('REPORT');
        args.set_cancel(true);
    }
        /*for medication review status */
    else if (args._item.get_value() == "tsRcopiarx_review_status") {
        OpenERX('REVIEW');
        args.set_cancel(true);

    }

    else if (args._item.get_value() == "PhoneEncounter") {
        args.set_cancel(true);
        var obj = new Array();
        var ID = new Object();
        ID = document.getElementById(GetClientId("hdnHumanID")).value;

        if (ID == undefined || ID == "") {

            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnClientClosePhoneEncounter);
        }
        else {
            obj.push("openingfrom=" + "Menu");
            obj.push("MyHumanID=" + ID);
            var result = openModal("HtmlPhoneEncounter.html", 800, 1230, obj, "ctl00_ModalWindow");
            var Window = $find('ctl00_ModalWindow');
            Window.add_close(function ClosePhoneEnc(oWindow, args) {
                window.location.href = "frmPatientChart.aspx"
            });
        }
        return false;

    }
    else if (args._item.get_value() == "PatientTask") {
        args.set_cancel(true);
        var obj = new Array();
        var ID = new Object();
        ID = document.getElementById(GetClientId("hdnHumanID")).value;
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnClientPatientCommunication);
        }
        else {
            var result = openModal("frmPatientCommunication.aspx", 810, 1050, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
        }
        return false;
    }
    else if (args._item.get_value() == "tsPatient_chart_summary") {

        if ($find("ctl00_C5POBody_lblPatientStrip") != null) {
            args.set_cancel(true);
            var obj = new Array();
            var Result = openModal("frmPatientChartSummary.aspx", 650, 960, null, "ctl00_ModalWindow");
            return false;
        }
        else {
            DisplayErrorMessage('140002');
            args.set_cancel(true);
        }
    }
}


function OnClientClickedSubMenu(data) {
    GetUTCTime();
    if (data == "MyQ") {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        window.location.href = "frmMyQueueNew.aspx";
    }
    if (data == "Appointments") {
        window.location.href = "frmAppointments.aspx?hdnSourceScreen=Menu";
    }
    else if (data == "tsRcopiarefill") {
        OpenERX('MESSAGE');
    }
    else if (data == "tsRcopiarx_pending") {
        OpenERX('REPORT');
    }
        /*for medication review status */
    else if (data == "tsRcopiarx_review_status") {
        OpenERX('REVIEW');

    }

    else if (data == "PhoneEncounter") {
        //var obj = new Array();
        //var ID = new Object();
        //ID = document.getElementById(GetClientId("hdnHumanID")).value;

        //if (ID == undefined || ID == "") {

        //    var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
        //    var WindowName = $find('ctl00_ModalWindow');
        //    WindowName.add_close(OnClientClosePhoneEncounter);
        //}
        //else {
        //    obj.push("openingfrom=" + "Menu");
        //    obj.push("MyHumanID=" + ID);
        //    var result = openModal("HtmlPhoneEncounter.html", 800, 1115, obj, "ctl00_ModalWindow");
        //    var Window = $find('ctl00_ModalWindow');
        //    Window.add_close(function ClosePhoneEnc(oWindow, args) {
        //        window.location.href = "frmPatientChart.aspx"
        //    });
        //}
        {
            StartLoadingImage();
            var obj = new Array();
            var ID = new Object();
            if ($find("ctl00_C5POBody_grdPatientQueue") != null) {
                document.getElementById(GetClientId("hdnHumanID")).value = ""
                ID = document.getElementById(GetClientId("hdnHumanID")).value;
            }
            else {
                ID = document.getElementById(GetClientId("hdnHumanID")).value;
            }
            if (ID == undefined || ID == "") {
                var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
                EMRPhoneEnounter = "Yes";
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.add_close(OnClientClosePhoneEncounter);
            }
            else {
                obj.push("openingfrom=" + "Menu");
                obj.push("MyHumanID=" + ID);
                var result = openModal("HtmlPhoneEncounter.html", 800, 1230, obj, "ctl00_ModalWindow");

                var Window = GetRadWindow();
                if (Window != undefined && Window != null)
                    Window.add_close(function ClosePhoneEnc(oWindow, args) {
                        window.parent.location.href = "frmPatientChart.aspx";
                    });
            }
        }
        return false;

    }
    else if (data == "PatientTask") {
        var obj = new Array();
        var ID = new Object();
        ID = document.getElementById(GetClientId("hdnHumanID")).value;
        if (ID == undefined || ID == "") {
            var result = openModal("frmFindPatient.aspx", 251, 1200, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            WindowName.add_close(OnClientPatientCommunication);
        }
        else {
            var result = openModal("frmPatientCommunication.aspx?IsMYQ=N", 810, 1050, obj, "ctl00_ModalWindow");
            var WindowName = $find('ctl00_ModalWindow');
            //CAP-302 - handle null value
            WindowName?.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
        }
        return false;
    }
    else if (data == "UploadDocuments") {

        OpenIndexing('Bulk Scanning and Fax');
    }
    else if (data == "tsPatient_chart_summary") {

        if ($find("ctl00_C5POBody_lblPatientStrip") != null) {
            args.set_cancel(true);
            var obj = new Array();
            var Result = openModal("frmPatientChartSummary.aspx", 650, 960, null, "ctl00_ModalWindow");
            return false;
        }
        else {
            DisplayErrorMessage('140002');
            args.set_cancel(true);
        }
    }
    else if (data == "tsbrowse") {//BugID:48547
        StartLoadingImage();
        var obj = new Array();
        obj.push("DialogMode=" + "Open");
        var result = openModal("frmBrowse.aspx", 500, 687, obj, "ctl00_ModalWindow");
        var Window = $find('ctl00_ModalWindow');
        Window.set_behaviors(Telerik.Web.UI.WindowBehaviors.None)
        return false;
    }
    else if (data == "tsMailbox") {//BugID:48547
        StartLoadingImage();
        var obj = new Array();
        obj.push("Role=" + "Provider");
        var result = openModal("frmMailBox.aspx", 494, 720, obj, "ctl00_ModalWindow");
        return false;
    }

    else if (data == "tsImported") {
        $(top.window.document).find("#TabImport").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalImportTitle")[0].textContent = "Abstracted";
        $(top.window.document).find("#TabmdldlgImport")[0].style.width = "1210px";
        $(top.window.document).find("#TabmdldlgImport")[0].style.height = "675px";
        $($($(top.window.document).find('#TabImport ')).find('#TabmdldlgImport')).find('.modal-content').css('overflow-y', 'auto');
        var sPath = ""
        sPath = "frmImportedPatients.aspx";
        $(top.window.document).find("#TabImportFrame")[0].style.height = "578px";
        $(top.window.document).find("#TabImportFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabImport").one("hidden.bs.modal", function (e) {
        });
        return false;
    }
    else if (data == "ReviewStatus") {//Added for Provider_Review PhysicianAssistant WorkFlow Change. Implementation of CA Rule for Provider Review
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($(top.window.document).find("#PhysicianReviewStatusModal") != undefined) {
            $(top.window.document).find("#PhysicianReviewStatusModal").modal({ backdrop: 'static', keyboard: false }, 'show');
            $(top.window.document).find('#PhysicianReview_Modal')[0].contentDocument.location.href = "htmlPhysicianReviewStatus.html?version=" + sessionStorage.getItem("ScriptVersion");
        }
        return false;
    }
}
function CloseReviewStatusModal() {
    $("#ReviewStatusModal")[0].style.display = "none";

}
function ReportGenerator() {
    args.set_cancel(true);
    var obj = new Array();
    var Result = openModal("frmMenus.aspx", 415, 425, obj, "ctl00_ModalWindow");
    return false;

}
function CreateOrder() {
    var obj = new Array();
    if (document.getElementById('ctl00_C5POBody_hdnHumanNo') == null) {
        var result = openModal("frmFindPatient.aspx", 251, 1200, obj);
        if (result != undefined && result.HumanId != undefined) {
            obj.push("HumanID=" + result.HumanId);
            obj.push("ScreenMode=Menu");
            document.getElementById(GetClientId("hdnHumanID")).value = result.HumanId;
            var ResultOrdersPatientBar = openModal("frmOrdersPatientBar.aspx", 810, 1227, obj);
        }
    }
    else {
        var ResultOrdersPatientBar = openModal("frmOrdersPatientBar.aspx", 810, 1227, obj);

    }
}


function openOnlineDocuments(args, screen) {
    args.set_cancel(true);
    var obj = new Array();
    obj.push("Screen=" + screen);
    obj.push("CurrentTime=" + document.getElementById(GetClientId("hdnLocalDate")).value);
    obj.push("ScreenMode=Bulk Scanning and Fax");
    localStorage.setItem("IndexingScreenMode", "Bulk Scanning and Fax");
    if (screen == "LocalDocuments")
        var result = openModal("frmIndexing.aspx", 880, 1200, obj, "ctl00_ModalWindow");
    else
        var result = openModal("frmIndexing.aspx", 710, 1200, obj, "ctl00_ModalWindow");

}

function openAddendum() {
    var obj = new Array();
    var result = openModal("frmAddendum.aspx", 600, 1500, obj, "ctl00_ModalWindow");
}
function FindPatient() {
    var obj = new Array();
    var result = openModal("frmFindPatient.aspx", 251, 1200, obj);
    if (result != undefined) {
        var elementRef = document.getElementById(GetClientId("hdnPatientValues"));
        elementRef.value = "sPatientName=" + result.PatientName + "$" + "sPatientDOB=" + result.PatientDOB + "$" + "sHumanId=" + result.HumanId;
    }
}

function OpenERXFromMENU() {
    var obj = new Array();
    obj.push("MyType=" + "GENERAL");
    obj.push("HumanID=" + document.getElementById(GetClientId("hdnHumanID")).value);
    obj.push("EncID=" + "0");
    obj.push("PrescriptionID=" + "0");
    obj.push("IsMoveButton=" + "true");
    obj.push("IsMoveCheckbox=" + "false");
    obj.push("IsPrescriptiontobePushed=" + "N");
    obj.push("openingFrom=" + "Menu");
    obj.push("IsSentToRCopia=" + "Y");
    obj.push("LocalTime=" + document.getElementById(GetClientId('hdnLocalTime')).value);
    var result = openModal("frmRCopiaWebBrowser.aspx", 635, 1060, obj, 'ctl00_ModalWindow');
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
    WindowName.add_close(TriggerDownloadRcopia);
    return false;
}

function TriggerDownloadRcopia(oWindow, args) {

    $.ajax({
        type: "POST",
        url: "frmEncounter.aspx/DownloadRcoipa",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            redirectToCCEprescription();
            reloadSummaryEprescription();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function (result) {
        }
    });
    //loadRcopia();
}


function RefreshRcopia() {
    if (document.getElementById("tsRefill") != null && document.getElementById("tsRefill") != undefined)
        document.getElementById("tsRefill").innerText = "REFILL:Loading...";

    else if (document.getElementById("ctl00_tsRefill") != null && document.getElementById("ctl00_tsRefill") != undefined)
        document.getElementById("ctl00_tsRefill").innerText = "REFILL:Loading...";

    if (document.getElementById("tsRx_Pending") != null && document.getElementById("tsRx_Pending") != undefined)
        document.getElementById("tsRx_Pending").innerText = "RX_PENDING:Loading...";

    else if (document.getElementById("ctl00_tsRx_Pending") != null && document.getElementById("ctl00_tsRx_Pending") != undefined)
        document.getElementById("ctl00_tsRx_Pending").innerText = "RX_PENDING:Loading...";

    loadRcopia();

}

function LoadRcopiaCount(oWindow, args) {
    loadRcopia();
    oWindow.remove_close(LoadRcopiaCount);
}
function OrderDashBoard() {

    var obj = new Array();
    var resultLabDashBoard = openModal("frmLabDashBoard.aspx", 630, 650, obj, "ctl00_ModalWindow");
}

function OnClientCloseMPR(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0") {
            var obj = new Array();
            obj.push("MyHumanID=" + HumanId);
            var MRE = openModal("frmManageProblemList.aspx", 720, 1225, obj, "ctl00_RadWindow2");
        }
    }
}
function OnClientCloseFindPatient(oWindow, args) {
    var overlay = top.document.getElementsByClassName("TelerikModalOverlay").length;
    for (var i = 0; i < overlay; i++) {
        top.document.getElementsByClassName("TelerikModalOverlay")[i].attributes[2].value = "display: none;";
    }
}

function OnClientCloseMRE(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0") {
            document.getElementById(GetClientId("hdnHumanID")).value = HumanId;
            window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenName=MRE";

        }
    }

}
function OnClientPatientCommunication(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0") {
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("AccountNum=" + HumanId);
                obj.push("PatientName=" + arg.PatientName);
                obj.push("PatientDOB=" + arg.PatientDOB);
                if (arg.HumanType != undefined && arg.HumanType != "" && arg.HumanType != "N")
                    obj.push("HumanType=" + arg.HumanType + " | " + arg.Aco_Eligible);
                else
                    obj.push("HumanType=" + arg.HumanType);
                obj.push("IsMYQ=" + "N");
                var MRE = openModal("frmPatientCommunication.aspx", 810, 1050, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
            }, 50);
        }
    }
}

function OnClientClosePFSH(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        var dob = arg.PatientDOB;
        if (HumanId != "0") {
            if ($(top.window.document).find("iframe")[0].contentDocument.URL.indexOf("frmEncounter") > -1) {
                StopLoadingImage();
                DisplayErrorMessage("180051");
            }
            else {
                var obj = new Array();
                window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenName=PFSH" + "&openingfrom=Menu";
                var PFSH = openModal("HtmlPFSH.html?MyHumanID=" + HumanId + "&openingfrom=Menu&DOB=" + dob, 710, 1050, obj, "ctl00_ModalWindow");
            }
        }
    }

}

function OnClientCloseVitals(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        var now = new Date();
        nowDate = new Date();
        nowDate = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();
        nowDate += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        document.getElementById(GetClientId("hdnSystemTime")).value = nowDate;

        if (HumanId != "0") {
            document.getElementById(GetClientId("hdnHumanID")).value = HumanId;
            window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenName=Enter Vitals" + "&Date=" + nowDate + "&openingfrom=Menu";
        }
    }

}
function OnClientCloseManageProblemList(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;

        if (HumanId != "0") {
            document.getElementById(GetClientId("hdnHumanID")).value = HumanId;
            window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenName=Manage Problem List" + "&openingfrom=Menu";
        }
    }

}
function OnClientCloseVitalsAutoSave(oWindow, args) {

    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
        if (document.getElementById("hdnMessageType").value == "") {
            DisplayErrorMessage('1100000');
            args.set_cancel(true);
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            self.close();
        }
        else if (document.getElementById("hdnMessageType").value == "No") {
            document.getElementById("hdnMessageType").value = ""
            self.close();
        }
        else if (document.getElementById("hdnMessageType").value == "Cancel") {
            document.getElementById("hdnMessageType").value = "";
        }
    }
    else {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
        self.close();
    }

}

function OnClientCloseCreateReminderAutoSave(oWindow, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable.value == "true" && DisplayErrorMessage('1100000') == true)
        args.set_cancel(true);
    else
        window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable.value = "false";

}

function OnClientCloseOrders(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        var dob = arg.PatientDOB;
        if (HumanId != "0") {
            var obj = new Array();
            obj.push("HumanID=" + HumanId);
            var createOrderWin = openModal("frmOrdersPatientBar.aspx", 810, 1237, obj, "ctl00_ModalWindow");
        }
    }
    oWindow.remove_close(OnClientCloseOrders);

}

function OnOpendPatientChartClick(oWindow, args) {
    var arg = args.get_argument();

    //inserLog("0",arg.HumanId, "Openning HumanXML from Menulevel PatientChart");
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0" && HumanId != "" && HumanId != "undefined") {
            document.getElementById(GetClientId("hdnHumanID")).value = HumanId;
            sessionStorage.removeItem("EncId_PatSummaryBar");
            sessionStorage.removeItem("Enc_DOS");
            window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenMode=Menu" + "&openingfrom=Menu";
            $("#ctl00_mnuEMR_smnuException_smnuCreateException").css("background-color", "rgb(109, 119, 119)");
            $("#ctl00_mnuEMR_smnuException_smnuCreateException").addClass("not-active");
        }
    }
    else {
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").css("background-color", "#4f94cd");
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").removeClass("not-active");
    }
}

function OnImmunizationRegistryQueryClick(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0") {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=ImmunizationRegistryQuery" + "&HumanID=" + HumanId, 520, 1090, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=ImmunizationRegistryQuery" + "&HumanID=" + HumanId, 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
}
function OnImmunizationRegistryQueryHistoryClick(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0") {
            StartLoadingImage();
            var objACO = openModal("frmExchange.aspx?TabName=ImmunizationRegistryQueryHistory" + "&HumanID=" + HumanId, 520, 1090, null, "ctl00_ModalWindow");
            var obj = new Array();
            var objACO = openModalProgress("frmImmunizationRegistry.aspx?TabName=ImmunizationRegistryQueryHistory" + "&HumanID=" + HumanId, 500, 500, null, "RadExchangeWindow");
            var win = $find("RadExchangeWindow");
            win.hide();
        }
    }
}

function OnOpenDemographicsClick(oWindow, args) {

    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;

        if (HumanId != "0") {
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("HumanId=" + HumanId);
                obj.push("ScreenName=Demographics");
                var result = openModal("frmPatientDemographics.aspx", 1230, 1130, obj, "ctl00_ModalWindow");
                oWindow.remove_close(OnOpenDemographicsClick);
            }, 50);
            return false;
        }
    }
}



function OnOpenPatientCommunicationClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        if (HumanId != "0") {
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("AccountNum=" + HumanId);
                obj.push("PatientName=" + Result.PatientName);
                obj.push("PatientDOB=" + Result.PatientDOB);
                obj.push("HumanType=" + Result.HumanType);
                obj.push("IsMYQ=" + "N");

                var result = openModal("frmPatientCommunication.aspx", 810, 1050, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
            }, 50);
        }
    }
}


function OpenPatientPayementsAccountClick(oWindow, args) {
    var Result = args.get_argument();
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    if (Result) {
        var humanid = Result.HumanId;
        if (humanid != "0") {
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("HumanId=" + humanid);
                obj.push("PatientName=" + Result.patientName);
                obj.push("PatientDOB=" + Result.PatientDOB);
                obj.push("HumanType=" + Result.HumanType);
                obj.push("IsMYQ=" + "N");
                // var result = openModal("frmPatientPayment.aspx", 450, 1300, obj, "ctl00_ModalWindow");
                //var result = openModal("frmPatientPayment.aspx", 512, 1047, obj, "ctl00_ModalWindow");

                var result = openModal("frmPatientPayment.aspx", 450, 1047, obj, "ctl00_ModalWindow");


                var WindowName = $find('ctl00_ModalWindow');
                WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
            }, 50);
        }
    }

}



//code added by balaji.TJ
function OnClosePatientCommunicationClick(oWindow, args) {
    oWindow.Close();
}

function OnClosePatientPayementsAccountClick(oWindow, args) {
    oWindow.Close();
}

function OnOpenViewPatientTaskClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        if (HumanId != "0") {
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("AccountNum=" + HumanId);
                var result = openModal("frmViewPatientTask.aspx", 920, 1100, obj, "ctl00_ModalWindow");
            }, 50);
        }
    }
}
function OnOpenViewMessageClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        if (HumanId != "0") {
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("AccountNum=" + HumanId);
                var result = openModal("frmViewMessage.aspx", 500, 1210, obj, "ctl00_ModalWindow");
            }, 50);
        }
    }
}
function OnClientCloseEPRES(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        if (HumanId != "0") {
            document.getElementById(GetClientId("hdnERXHumanId")).value = HumanId;
            document.getElementById(GetClientId("Button2")).click();
        }

    }
}
function OnClientCloseCreateOrder(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        if (HumanId != "0") {
            window.parent.document.getElementById('ctl00_hdnHumanID').value = HumanId;

            var height = window.screen.availHeight;
            document.cookie = "Resolution=" + height;
            document.getElementById(GetClientId("Button3")).click();
        }
    }
}
function GetUTCTime() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
}
function OnOpenFindAuthorizationClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        if (HumanId != "0") {
            var obj = new Array();
            obj.push("AccNo=" + HumanId);
            var result = openModal("frmFindAuthorization.aspx", 600, 1250, obj, "ctl00_ModalWindow");

        }

    }
}
function OnClientCloseACO(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        $.ajax({
            type: "POST",
            url: "frmRCopiaToolbar.aspx/CheckACOEligiblity",
            data: "{'strHumanID':'" + HumanId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                document.getElementById(GetClientId("hdnAcoEligibility")).value = response.d;
                var ACO_eligiblity = document.getElementById(GetClientId("hdnAcoEligibility")).value;
                var dob = arg.PatientDOB;
                if (HumanId != "0" && (ACO_eligiblity == 'Y' || ACOEligibility == "ACO")) {
                    var obj = new Array();
                    obj.push("HumanID=" + HumanId);
                    document.getElementById(GetClientId("hdnHumanID")).value = HumanId;
                    window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&IsAcoEligible=" + ACO_eligiblity + "&ScreenMode=Menu&OpenACO=Y" + "&openingfrom=Menu";;
                    setInterval(function () { ShowACOScreen(ACO_eligiblity) }, 800);


                }
                else {
                    DisplayErrorMessage('1091010');
                }
            },
            error: function OnError(xhr) {
                StopLoadingImage();
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                }
            }
        });

    }
    oWindow.remove_close(OnClientCloseACO);


}

function OnClientClosePhoneEncounter(oWindow, args) {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        if (HumanId != "0") {
            setTimeout(
            function () {

                if (EMRPhoneEnounter == "Yes") {
                    window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenName=PhoneEncounter" + "&openingfrom=Menu";
                }
                else {
                    var result = openModal("HtmlPhoneEncounter.html?openingfrom=Menu&MyHumanID=" + HumanId + "&LoadPatientChart=False", 800, 1230, obj, "ctl00_ModalWindow");
                    var WindowName = $find('ctl00_ModalWindow');
                    WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                }

            }, 0);
        }
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
function ShowACOScreen() {

    var objACO = openModal("frmACOValidation.aspx", 150, 500, obj, "ctl00_ModalWindow");

}
function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

}

function OnClientCloseOrderManagement(oWindow, args) {
    oWindow.remove_close(OnClientCloseOrderManagement);
}

function OnClientCloseAdminModule(oWindow, args) {

    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        var dob = arg.PatientDOB;
        var PatientName = arg.PatientName;
        if (HumanId != "0") {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            window.setTimeout(function () {
                var obj = new Array();
                obj.push("HumanID=" + HumanId);
                obj.push("PatientName=" + PatientName);
                obj.push("PatientDOB=" + dob);
                var Result = openModal("frmWFObjectManager.aspx", 660, 1100, obj, "ctl00_ModalWindow");
                var WindowName = $find('ctl00_ModalWindow');
                WindowName.add_close(function OnCloseWfObjectManager(oWindow, args) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                });
            }, 50);
        }
    }
    oWindow.remove_close(OnClientCloseAdminModule);

}

function OnClientCloseOpenReconcillation(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;
        var dob = arg.PatientDOB;
        var PatientName = arg.PatientName;
        if (HumanId != "0") {
            var obj = new Array();
            obj.push("HumanID=" + HumanId);
            obj.push("PatientName=" + PatientName);
            obj.push("PatientDOB=" + dob);
            var Result = openModal("frmClinicalInformation.aspx", 900, 1200, obj, "ctl00_ModalWindow");
        }
    }
    oWindow.remove_close(OnClientCloseOpenReconcillation);

}


function OnOpenViewTransactionClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        var HumanId = Result.HumanId;
        window.setTimeout(function () {
            if (HumanId != "0") {
                var obj = new Array();
                obj.push("AccountNum=" + HumanId);
                var result = openModal("frmViewTransaction.aspx", 750, 1255, obj, "ctl00_ModalWindow");
            }
        }, 50);
    }
}


function BeforeCloseForManagePbmList(oWindow, args) {

    if (window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable.value == "true" && DisplayErrorMessage('1100000') == true)
        args.set_cancel(true);
    else
        window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable.value = "false";
}

function BeforeCloseForCreateOrder(oWindow, args) {
    var obtn = radWindow.get_contentFrame().contentWindow.$find('btnOrderSubmit');
    if (obtn != null) {
        if (obtn._enabled) {
            var IsClearAll = DisplayErrorMessage('280014');
            if (IsClearAll) {
                var butt = radWindow.get_contentFrame().contentWindow.$find('btnOrderSubmit');
                butt.click();
            }
            else {
                var Result = new Object();
                var WindowName = $find('RadWindowImportResult');
                WindowName.Close();

            }
        }
    }
}
//function CheckNloadRcopia() {

//    if (sessionStorage.getItem("RxCount") == null || sessionStorage.getItem("RxCount") == undefined) {

//        loadRcopia();
//    }
//    else {
//        if (document.getElementById("tsRefill") != undefined && document.getElementById("tsRx_Pending") != undefined && document.getElementById("tsRx_Need_Signing") != undefined) {
//            if (sessionStorage.getItem("RxCount") == "") {
//                document.getElementById("tsRefill").style.display = "none";
//                document.getElementById("tsRx_Pending").style.display = "none";
//                document.getElementById("tsRx_Need_Signing").style.display = "none";
//                document.getElementById("tsRx_Change").style.display = "none";

//            }
//            else {
//                var rx_values = sessionStorage.getItem("RxCount").split("$:$");
//                if (rx_values.length >= 3) {
//                    document.getElementById("tsRefill").innerText = rx_values[0];
//                    document.getElementById("tsRx_Pending").innerText = rx_values[1]; 
//                    document.getElementById("tsRx_Need_Signing").innerText = rx_values[2];
//                    document.getElementById("tsRx_Change").innerText = rx_values[3];

//                }
//                else {
//                    document.getElementById("tsRefill").innerText = "Refill : 0";
//                    document.getElementById("tsRx_Pending").innerText = "Rx_Pending : 0";
//                    document.getElementById("tsRx_Need_Signing").innerText = "Rx_Need_Signing : 0";
//                    document.getElementById("tsRx_Change").innerText = "RxChange : 0";

//                }
//            }

//        }
//    }
//}

function loadRcopia() {

    $.ajax({
        type: "POST",
        url: "frmRCopiaToolbar.aspx/LoadRCopiaNotification",
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessRCopia,
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = xhr.statusText;
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

function loadImpportedPatient() {
    if (sessionStorage.getItem("importCount") == null || sessionStorage.getItem("importCount") == undefined) {
        $.ajax({
            type: "POST",
            url: "frmRCopiaToolbar.aspx/LoadImportedPatients",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessImport,
            error: function OnError(xhr) {
                if (xhr.status == 999)
                    window.location = xhr.statusText;
                else {
                    if (xhr.responseText != null && xhr.responseText.trim() != '' && xhr.responseText != undefined) {
                        var log = JSON.parse(xhr.responseText);
                        console.log(log);
                        alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                    }
                    else {
                        alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        });
    }
    else {
        if (document.getElementById("tsImported") != undefined) {
            document.getElementById("tsImported").style.display = "block";
            document.getElementById("tsImported").innerText = "ABSTRACTED: " + sessionStorage.getItem("importCount");
        }
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }


    }
}
//BugID:48547
function loadMailClinicalInfoCount() {
    document.getElementById("tsbrowse").style.display = "block";
    document.getElementById("tsMailbox").style.display = "block";

    if (sessionStorage.getItem("MailClinicalCnt") == null || sessionStorage.getItem("MailClinicalCnt") == undefined) {
        $.ajax({
            type: "POST",
            url: "frmRCopiaToolbar.aspx/LoadMailClinicalInfo",
            data: "{'type':'All'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessMailClinicalInfo,
            error: function OnError(xhr) {
                sessionStorage.setItem("MailClinicalCnt", "0:0");
                document.getElementById("tsbrowse").innerText = "CCD : 0";
                document.getElementById("tsMailbox").innerText = "MAIL : 0";

                if (xhr.status == 999)
                    window.location = xhr.statusText;
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
    else {
        var Mail_Clinical_count = sessionStorage.getItem("MailClinicalCnt");
        document.getElementById("tsbrowse").innerText = "CCD : " + Mail_Clinical_count.split(':')[0];
        document.getElementById("tsMailbox").innerText = "MAIL : " + Mail_Clinical_count.split(':')[1];



    }
}

$(document).ready(function () {
    $('li  div').addClass('navhover');
    $("#falogout").css("display", "block");

    //The given forms include JSC5PO as scripts, to avoid executing the ready function check the URL.
    if (document.URL.indexOf('frmOrderManagement') < 0 && document.URL.indexOf('frmCheckOut') < 0 && document.URL.indexOf('frmLabResult') < 0) {
        StartLoadingImage();
        var MenuItems = $("#MainMenu li");
        for (var i = 0; i < MenuItems.length; i++) {
            if (MenuItems[i].attributes.length > 1) {
                if (MenuItems[i].attributes[1].value == "disabled") {
                    MenuItems[i].style.backgroundColor = "#6D7777";
                    $(MenuItems[i]).find('a').addClass("disabledMenu");
                    MenuItems[i].children[0].setAttribute("onclick", "return false");
                }
            }
        }

        //CheckNloadRcopia();//BugID:54514
        var UserRole = "";
        if (document.getElementById(GetClientId("hdnuserrole")) != undefined && document.getElementById(GetClientId("hdnuserrole")).value != "") {
            UserRole = document.getElementById(GetClientId("hdnuserrole")).value.trim().toUpperCase();
        }
        // Mail and CCD shortcut menus need not show count 
        //if (UserRole != "" && (UserRole == "PHYSICIAN" || UserRole == "PHYSICIAN ASSISTANT"))
        //    loadMailClinicalInfoCount();//BugID:48547
        //loadImpportedPatient();

        if ((document.URL.indexOf("frmPatientChart") > -1 && (document.URL.indexOf("ScreenMode=Menu") > -1 || document.URL.indexOf("Source=WindowItem") > -1 ||
            document.URL.substr(document.URL.indexOf('frmPatientChart.aspx'), document.URL.length) == "frmPatientChart.aspx")))
            StopLoadingImage();

    }
});
function OnSuccessImport(response) {
    var responseValues = response.d;
    var rxValues = "";
    if (document.getElementById("tsImported") != null && document.getElementById("tsImported") != undefined) {
        document.getElementById("tsImported").style.display = "block";

        document.getElementById("tsImported").innerText = "ABSTRACTED: " + responseValues;

        sessionStorage.setItem("importCount", responseValues);
    }
    else {
        $(top.window.document).find("#tsImported")[0].style.display = "block";
        $(top.window.document).find("#tsImported")[0].innerText = "ABSTRACTED: " + responseValues;
        sessionStorage.setItem("importCount", responseValues);
    }
}
function OnSuccessRCopia(response) {
    var responseValues = response.d.split('#$%');
    var rxValues = "";

    if (responseValues == "") {
        if (document.getElementById("tsRefill") != null && document.getElementById("tsRefill") != undefined)
            document.getElementById("tsRefill").style.display = "none";
        if (document.getElementById("ctl00_tsRefill") != null && document.getElementById("ctl00_tsRefill") != undefined)
            document.getElementById("ctl00_tsRefill").style.display = "none";

        document.getElementById("tsRx_Pending").style.display = "none";
        document.getElementById("tsRx_Need_Signing").style.display = "none";
        document.getElementById("tsRx_Change").style.display = "none";
    }
    else if (responseValues != null && responseValues.length >= 3) {
        if (document.getElementById("tsRefill") != null && document.getElementById("tsRefill") != undefined)
            document.getElementById("tsRefill").innerText = responseValues[0];

        else if (document.getElementById("ctl00_tsRefill") != null && document.getElementById("ctl00_tsRefill") != undefined)
            document.getElementById("ctl00_tsRefill").innerText = responseValues[0];

        if (document.getElementById("tsRx_Pending") != null && document.getElementById("tsRx_Pending") != undefined)
            document.getElementById("tsRx_Pending").innerText = responseValues[1];

        else if (document.getElementById("ctl00_tsRx_Pending") != null && document.getElementById("ctl00_tsRx_Pending") != undefined)
            document.getElementById("ctl00_tsRx_Pending").innerText = responseValues[1];

        if (document.getElementById("tsRx_Need_Signing") != null && document.getElementById("tsRx_Need_Signing") != undefined)
            document.getElementById("tsRx_Need_Signing").innerText = responseValues[2];

        else if (document.getElementById("ctl00_tsRx_Need_Signing") != null && document.getElementById("ctl00_tsRx_Need_Signing") != undefined)
            document.getElementById("ctl00_tsRx_Need_Signing").innerText = responseValues[2];


        if (document.getElementById("tsRx_Change") != null && document.getElementById("tsRx_Change") != undefined)
            document.getElementById("tsRx_Change").innerText = responseValues[3];

        else if (document.getElementById("ctl00_tsRx_Change") != null && document.getElementById("ctl00_tsRx_Change") != undefined)
            document.getElementById("ctl00_tsRx_Change").innerText = responseValues[3];

        if (document.getElementById("tsRefill") != null && document.getElementById("tsRefill") != undefined)
            rxValues = document.getElementById("tsRefill").innerText + "$:$" + document.getElementById("tsRx_Pending").innerText + "$:$" + document.getElementById("tsRx_Need_Signing").innerText + "$:$" + document.getElementById("tsRx_Change").innerText;
        else if (document.getElementById("ctl00_tsRefill") != null && document.getElementById("ctl00_tsRefill") != undefined)
            rxValues = document.getElementById("ctl00_tsRefill").innerText + "$:$" + document.getElementById("ctl00_tsRx_Pending").innerText + "$:$" + document.getElementById("ctl00_tsRx_Need_Signing").innerText + "$:$" + document.getElementById("ctl00_tsRx_Change").innerText;

    }
    else {
        if (document.getElementById("tsRefill") != null && document.getElementById("tsRefill") != undefined) {
            document.getElementById("tsRefill").innerText = "Refill : 0";
            document.getElementById("tsRx_Pending").innerText = "Rx_Pending : 0";
            document.getElementById("tsRx_Need_Signing").innerText = "Rx_Need_Signing : 0";
            document.getElementById("tsRx_Change").innerText = "RxChange : 0";


            rxValues = document.getElementById("tsRefill").innerText + "$:$" + document.getElementById("tsRx_Pending").innerText + "$:$" + document.getElementById("tsRx_Need_Signing").innerText + "$:$" + document.getElementById("tsRx_Change").innerText;

        }
        else if (document.getElementById("ctl00_tsRefill") != null && document.getElementById("ctl00_tsRefill") != undefined) {

            document.getElementById("ctl00_tsRefill").innerText = "Refill : 0";
            document.getElementById("ctl00_tsRx_Pending").innerText = "Rx_Pending : 0";
            document.getElementById("ctl00_tsRx_Need_Signing").innerText = "Rx_Need_Signing : 0";
            document.getElementById("ctl00_tsRx_Change").innerText = "RxChange : 0";


            rxValues = document.getElementById("ctl00_tsRefill").innerText + "$:$" + document.getElementById("ctl00_tsRx_Pending").innerText + "$:$" + document.getElementById("ctl00_tsRx_Need_Signing").innerText + "$:$" + document.getElementById("ctl00_tsRx_Change").innerText;

        }
    }
    sessionStorage.setItem("RxCount", rxValues);
}
//BugID:48547
function OnSuccessMailClinicalInfo(response) {
    var result = $.parseJSON(response.d);
    document.getElementById("tsbrowse").innerText = "CCD : " + result.FileCount;
    document.getElementById("tsMailbox").innerText = "MAIL : " + result.MailCount;

    sessionStorage.setItem("MailClinicalCnt", result.FileCount + ":" + result.MailCount);
}

function chkFavCPT_Click() {
    $(top.window.document).find("#CPTok")[0].disabled = false;
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}
function chkFavICD_Click() {
    $(top.window.document).find("#ok")[0].disabled = false;
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}

function CheckEncountersForMenuPFSH(humanID, str) {
    var ID = { HumanID: humanID };
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/CheckEncounterForPFSH",
        data: JSON.stringify(ID),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;
            if (result != "") {
                if (result.toUpperCase() == "NO_NEW_APPOINTMENTS") {
                    StopLoadingImage();
                    DisplayErrorMessage('180052');
                }
                else if (result.indexOf("OpenPFSHForEnc") > -1) {
                    var EncID = result.split('=')[1];
                    if (str == "CHART")
                        window.parent.location.href = "frmPatientChart.aspx?HumanID=" + humanID + "&ScreenName=PFSH" + "&openingfrom=Menu&EncounterID=" + EncID;
                    else
                        if (str == "MODAL") {
                            var obj = new Array();
                            result = openModal("HtmlPFSH.html?MyHumanID=" + humanID + "&openingfrom=Menu&EncounterID=" + EncID + "&version=" + sessionStorage.getItem("ScriptVersion"), 760, 1185, obj, "ctl00_PFSHWindow");
                            var PFSHWindow = $find('ctl00_PFSHWindow');
                            if (PFSHWindow != null) {
                                PFSHWindow.moveTo(40, 160);
                                PFSHWindow.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                            }
                        }
                }
            }
        },
        error: function OnError(xhr) {
            StopLoadingImage();
            if (xhr.status == 999)
                window.location = xhr.statusText;
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

function OnClientClosePotentialDiagnosis(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var HumanId = arg.HumanId;

        if (HumanId != "0") {
            document.getElementById(GetClientId("hdnHumanID")).value = HumanId;
            window.parent.location.href = "frmPatientChart.aspx?HumanID=" + HumanId + "&ScreenName=Potential Diagnosis" + "&openingfrom=Menu";
        }
    }

}

function CloseOkWindow() {
    alert("Check");
    $(top.window.document).find('#divErrorMessage').modal('hide');
    return false;
}

function HideLoadIcdon() {

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    return false;
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
        if (inputargument.lenght != 0) {
            PageName = PageName + "?";
        }
    }

    window.open("popup1.aspx", "mywindow", "location=1,status=1,scrollbars=1,width=400,height=400")
    var result = window.open(PageName + Argument, '', "Height=" + height + ",Width=" + width + ",resizable=yes,scrollbars=yes");
    if (result == undefined) { result = window.returnValue; }
    return result;
}
if (popup != undefined) {
    if (false == popup.closed) {

        popup.close();
    }
}
function maximizeICD() {

    $(top.window.document).find("#ICDModal").css({ "margin-top": "5%", "margin-left": "3%", "width": "95%", "margin-bottom": "0%" });
    $(top.window.document).find("#btnMinimizeViewResultICD").css({ "display": "block" });
    $(top.window.document).find("#btnMaximizeViewResultICD").css({ "display": "none" });
    $(top.window.document).find("#divBody").css({ "display": "block" });
    $(top.window.document).find("#divICDFooter").css({ "display": "block" });


    $(top.window.document).find("#main").css({ "position": "relative" });
    $(top.window.document).find("#divFormView").css({ "position": "absolute" });

}
function maximizeCPT() {

    $(top.window.document).find("#CPTModal").css({ "margin-top": "5%", "margin-left": "3%", "width": "95%", "margin-bottom": "0%" });
    $(top.window.document).find("#btnMinimizeViewResultCPT").css({ "display": "block" });
    $(top.window.document).find("#btnMaximizeViewResultCPT").css({ "display": "none" });
    $(top.window.document).find("#divCPTBody").css({ "display": "block" });
    $(top.window.document).find("#divCPTFooter").css({ "display": "block" });

    //to disable access to main parts of page 
    $(top.window.document).find("#main").css({ "position": "relative" });
    $(top.window.document).find("#divCPTFormView").css({ "position": "absolute", "height": "108%!important" });

}
function minimizeICD() {

    $(top.window.document).find("#btnMinimizeViewResultICD").css({ "display": "none" });
    $(top.window.document).find("#btnMaximizeViewResultICD").css({ "display": "block" });
    $(top.window.document).find("#divBody").css({ "display": "none" });
    $(top.window.document).find("#divICDFooter").css({ "display": "none" });
    $(top.window.document).find("#ICDModal").css({ "margin-top": "850px", "margin-left": "0%", "width": "30%", "margin-bottom": "0%" });
    //to access main parts of page 
    $(top.window.document).find("#main").css({ "position": "absolute" });
    $(top.window.document).find("#divFormView").css({ "position": "static" });
    //to access backdrop of modal
    var activeFrame = $($($(top.window.document).find("iframe[id=ctl00_C5POBody_EncounterContainer]")[0].contentDocument.activeElement).find(".tab-pane.active")[0].firstElementChild);
    $(activeFrame[0].contentDocument.activeElement).find(".modal-backdrop").css({ "position": "static" });
}
function minimizeCPT() {

    $(top.window.document).find("#btnMinimizeViewResultCPT").css({ "display": "none" });
    $(top.window.document).find("#btnMaximizeViewResultCPT").css({ "display": "block" });
    $(top.window.document).find("#divCPTBody").css({ "display": "none" });
    $(top.window.document).find("#divCPTFooter").css({ "display": "none" });
    $(top.window.document).find("#CPTModal").css({ "margin-top": "850px", "margin-left": "0%", "width": "30%", "margin-bottom": "0%" });
    //to access main parts of page 
    $(top.window.document).find("#main").css({ "position": "absolute" });
    $(top.window.document).find("#divCPTFormView").css({ "position": "static", "height": "108%!important" });
    //to access backdrop of modalh

    var activeFrame = $($($(top.window.document).find("iframe[id=ctl00_C5POBody_EncounterContainer]")[0].contentDocument.activeElement).find(".tab-pane.active")[0].firstElementChild);
    $(activeFrame[0].contentDocument.activeElement).find(".modal-backdrop").css({ "position": "static" });
}
$(top.window.document).find("#btnICDClose").on('click', function () {
    maximizeICD();
});
$(top.window.document).find("#btnCPTClose").on('click', function () {
    maximizeCPT();
});
function OpenIndexing(Mode) {
    StartLoadingImage();
    var cookies = document.cookie.split(';');
    var CUserRole = "";
    var CUserName = "";
    var CFacilityName = "";
    var sCurrPhyId = "";
    var re = /%20/gi;
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CUserRole") > -1)
            CUserRole = cookies[l].split("=")[1];
        if (cookies[l].indexOf("CUserName") > -1)
            CUserName = cookies[l].split("=")[1];
        if (cookies[l].indexOf("CFacilityName") > -1)
            CFacilityName = cookies[l].split("=")[1];
        if (cookies[l].indexOf("CurrPhyId") > -1)
            sCurrPhyId = cookies[l].split("=")[1];
    }
    CFacilityName = CFacilityName.replace(re, ' ');
    var HumanID = "";
    HumanID = document.getElementById(GetClientId("hdnHumanID")).value;

    // if (CFacilityName == "CMG LAB AND ANCILLARY 1866 #101") 
    if (CFacilityName == document.getElementById(GetClientId("hdnIsAncillary")).value)
    {
        var Continue = DisplayErrorMessage('172507');
        if (Continue != undefined && Continue == true) {
            var obj = new Array();
            var screen = "OnlineDocuments";
            obj.push("Screen=" + screen);
            obj.push("HumanId=" + HumanID);
            obj.push("ScreenMode=" + Mode);
            localStorage.setItem("IndexingScreenMode", Mode);
            var dateonclient = new Date;
            var Tz = (dateonclient.getTimezoneOffset());
            document.cookie = "Tz=" + Tz;
            var result = openModal("frmIndexing.aspx", 710, 1200, obj, "ctl00_ModalWindow");
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        }
    }
    else {
        var obj = new Array();
        var screen = "OnlineDocuments";
        obj.push("Screen=" + screen);
        obj.push("HumanId=" + HumanID);
        obj.push("ScreenMode=" + Mode);
        localStorage.setItem("IndexingScreenMode", Mode);
        var dateonclient = new Date;
        var Tz = (dateonclient.getTimezoneOffset());
        document.cookie = "Tz=" + Tz;
        var result = openModal("frmIndexing.aspx", 710, 1200, obj, "ctl00_ModalWindow");
    }

}

function OnOpenAuthorizationClick(oWindow, args) {

    var HumanId = "";
    var arg = args.get_argument();
    if (arg) {
        HumanId = arg.HumanId;

    }
    if (HumanId != "") {

        obj = new Array();
        obj.push("Human_Id=" + HumanId);
        var result = openModal("frmAuthorization.aspx", 800, 1200, obj, "ctl00_RadWindow1");
        var WindowName = $find('ctl00_RadWindow1');
        localStorage.setItem("AuthScreenMode", "Menu");

    }
    return false;
}


function OnOpenPerformEVClick(oWindow, args) {

    var HumanId = "";
    var arg = args.get_argument();
    if (arg) {
        HumanId = arg.HumanId;

    }
    if (HumanId != "") {

        obj = new Array();
        obj.push("HumanId=" + HumanId);
        var result = openModalPerformEV("frmPerformEV.aspx", 620, 1200, obj, "ctl00_RadWindow1");
        var WindowName = $find('ctl00_RadWindow1');
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    }
    return false;
}
function OnOpenEVSummaryClick(oWindow, args) {

    var HumanId = "";
    var arg = args.get_argument();
    if (arg) {
        HumanId = arg.HumanId;

    }
    if (HumanId != "") {
        var obj = new Array();
        obj.push("EncounterID=0");
        obj.push("humanID=" + HumanId);
        obj.push("EncStatus=''");
        obj.push("bShowPat=true");
        obj.push("sScreenMode=EVSUMMARY");
        var result = openModal("frmQuickpatientcreate.aspx", 730, 1020, obj, "ctl00_ModalWindow");
        var WindowName = $find('ctl00_RadWindow1');
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }
    return false;
}


function reloadSummaryEprescription() {
    var enc_id = sessionStorage.getItem("EncId_PatSummaryBar");
    var enc_DOS = sessionStorage.getItem("Enc_DOS");
    //sessionStorage.removeItem("EncId_PatSummaryBar");
    //sessionStorage.removeItem("Enc_DOS");
    $.ajax({
        type: "POST",
        url: "frmRCopiaToolbar.aspx/LoadPatientSummaryBar",
        // data: JSON.stringify({ EncID: "", Enc_DOS: "" }),
        data: JSON.stringify({ EncID: enc_id, Enc_DOS: enc_DOS }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessSummaryBarEprescription,
        error: function OnError(xhr) {
            if (xhr.status == 999)
                window.location = xhr.statusText;
            else {
                var log = JSON.parse(xhr.responseText);
                console.log(log);
                alert("USER MESSAGE:\n" +
                                    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                   "Message: " + log.Message);
            }
        }

    });

    RefreshNotification("Notify");
}

function OnSuccessSummaryBarEprescription(response) {
    var regex = /<BR\s*[\/]?>/gi;

    if (response != null) {

        top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = response.d[0];
        top.window.document.getElementById("ctl00_C5POBody_lblCheifComplaints").innerHTML = response.d[1];
        top.window.document.getElementById("ctl00_C5POBody_lblProblemList").innerHTML = response.d[2];
        top.window.document.getElementById("ctl00_C5POBody_lblVitals").innerHTML = response.d[3];
        top.window.document.getElementById("ctl00_C5POBody_lblMedication").innerHTML = response.d[4];
        if (response.d[5].replace("Allergies :<br/>", "").length != 0)
            top.window.document.getElementById("Allergies_tooltp").innerText = response.d[5].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Allergies_tooltp").innerText = "";
        if (response.d[6].replace("Chief Complaints :<br/><br/>", "").length != 0)
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = response.d[6].replace(regex, "\n").split("&#xA;").join("\n") + "\n";
        else
            top.window.document.getElementById("CheifComplaints_tooltp").innerText = "";
        if (response.d[7].replace("Problem List :<br/>", "").length != 0)
            top.window.document.getElementById("ProblemList_tooltp").innerText = response.d[7].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("ProblemList_tooltp").innerText = "";
        if (response.d[8].replace("Vitals :<br/>", "").length != 0)
            top.window.document.getElementById("Vitals_tooltp").innerText = response.d[8].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Vitals_tooltp").innerText = "";
        if (response.d[9].replace("Medication :<br/>", "").length != 0)
            top.window.document.getElementById("Medication_tooltp").innerText = response.d[9].replace(regex, "\n") + "\n";
        else
            top.window.document.getElementById("Medication_tooltp").innerText = "";
        RefreshOverallSummaryTooltip();

    }
    var sDtls = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText;
    document.cookie = "Human_Details=Last_Name:" + sDtls.split('|')[0].split(',')[0] + "|First_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[0] +
        "|Middle_Name:" + sDtls.split('|')[0].split(',')[1].split(' ')[1] + "|DOB:" + sDtls.split('|')[1] + "|Sex:" + sDtls.split('|')[3] + "|" +
        window.parent.document.getElementsByTagName('fieldset')[0].innerText.split('|')[1] + "|" + window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[1] + "|" +
        window.parent.parent.document.all("ctl00_C5POBody_lblVitals").innerText.split('\n')[2];

}

function redirectToCCEprescription() {
    var value = window.location.search.toString();
    var HumanID = "";
    var enc_id = sessionStorage.getItem("EncId_PatSummaryBar");
    //CAP-300 & CAP-326 - Declare variable.
    var openingFrom = "";
    if (value != "" && value.indexOf('&') > -1) {
        HumanID = value.split('&')[0].split('=')[1];
        openingFrom = value.split('&')[1].split('=')[1];
    }
    if (openingFrom == "Menu") {//Form open patient chart
        if (enc_id != null) {
            $('#ctl00_C5POBody_EncounterContainer')[0].src = "frmSummaryNew.aspx?EncounterId=" + enc_id;
        }
        //Blank the patientcart and remove selected encounter color in tree view
        //$('#ctl00_C5POBody_EncounterContainer')[0].src = "";
        //$("#ctl00_C5POBody_dvCheck li .colored").removeClass("colored");       
    }
    else { //for tab level
        if ($($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a") != null && $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a") != undefined && $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0] != null && $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0]!=undefined)
          $($(window.top.document).find('iframe[id=ctl00_C5POBody_EncounterContainer]')[0].contentDocument).find("ul li a")[0].click();
    }
}