
function OpenPDF() {
    var obj = new Array();
    obj.push("SI=" + document.getElementById('hdnSelectedItem').value);
    obj.push("Location=" + "DYNAMIC");
    setTimeout(function () { GetRadWindow().BrowserWindow.openModal('frmPrintPDF.aspx', 835, 900, obj, "mdl"); }, 0);
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

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function openLink(urls) {
    var temp = urls.split(';');
    var IsLonic = 0;
    if (temp.length > 1) {
        for (var i = 0; i < temp.length ; i++) {
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
function OpenFindAllAppointments() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var FindHumanID = 0;
    if ($('#txtPatientInformation').val() != ""  && $('#txtPatientInformation').val() != undefined) {
        FindHumanID = $('#txtPatientInformation').val().split('|')[4].split('#:')[1].trim();
    }
    else
    {
        //FindHumanID = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|')[4].split(':')[1].trim();
        if (window.parent.parent.document.getElementsByName('lblPatientStrip')[0] != undefined &&
        window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|').length > 3)
            FindHumanID = window.parent.parent.document.getElementsByName('lblPatientStrip')[0].innerText.split('|')[4].split(':')[1].trim();
    }
    var obj = new Array();
    obj.push("HumanID=" + FindHumanID);
    obj.push("IsFindPatientRequired=N");
    openModal("frmFindAllAppointments.aspx", 460, 900, obj, "ctl00_ModalWindow");
    var WindowName = $find('ctl00_ModalWindow');
    WindowName.add_close(FindAllAppointmentClick);//checked
    return false;
}
function FindAllAppointmentClick(oWindow, args) {
    oWindow.close();
}
//START -- BugID:41027 -- move to next result
function ViewNextResult() {
    if (sessionStorage.getItem("Next_Order_Index") != null && sessionStorage.getItem("Next_Order_Index") != undefined) {
        var next_index = sessionStorage.getItem("Next_Order_Index");
       var result_index = GetNextViewResult(next_index);
       if (result_index == 0) {
           End();
           StopLoadingImage();
           return;
       }
       var currRow = $($(top.window.document).find("table[id=EncounterTable] tbody")[0]).find("tr:eq('" + result_index + "')");
        if ($(currRow).length > 0) {
            var CurrentProcess = $(currRow)[0].children[8].innerText.trim().trim();
            var orderType = $(currRow)[0].children[14].innerText.trim().replace("INTERNAL", "").trim();
            var ObjType = $(currRow)[0].children[14].innerText.trim();
            if ((ObjType.toUpperCase() == "DIAGNOSTIC_RESULT") || (CurrentProcess == "RESULT_REVIEW" || orderType == "DICTATION_REVIEW") || (CurrentProcess == "MA_RESULTS"))

                var sPath = "";
            
            var fromMyOrderQ = "true";
            var OrderSubmitID = "";
            var File_Ref_ID = "";
            var HumanID = $(currRow)[0].children[2].innerText.trim();
            var EncounterID = $(currRow)[0].children[11].innerText.trim();
            var Physician_ID = $(currRow)[0].children[12].innerText.trim();
            if (orderType == "IMMUNIZATION ORDER" || orderType == "REFERRAL ORDER")
                OrderSubmitID = $(currRow)[0].children[13].innerText.trim();
            else
                OrderSubmitID = $(currRow)[0].children[17].innerText.trim();
            var ResultMasterID = $(currRow)[0].children[19].innerText.trim();
            File_Ref_ID = $(currRow)[0].children[20].innerText.trim();
            var LabID = $(currRow)[0].children[15].innerText.trim();

            if (CurrentProcess == "MA_REVIEW" && (orderType == "DIAGNOSTIC ORDER" || orderType == "IMAGE ORDER")) {
                var obj = new Array();
                var Result = openRadWindow("frmOrdersList.aspx?HumanID=" + $(currRow)[0].children[2].innerText.trim() + "&EncounterID=" + $(currRow)[0].children[11].innerText.trim() + "&PhysicianId=" + $(currRow)[0].children[12].innerText.trim() + "&OrderSubmitId=" + $(currRow)[0].children[17].innerText.trim() + "&ScreenMode=" + "MyQ&Openingfrom=MyorderQueue", 665, 1232, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');

                windowName.add_close(OnClientCloseWindow);
                StopLoadingImage();
                return false;
            }
            else if (ObjType.toUpperCase() == "DIAGNOSTIC_RESULT") {
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "95%";
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&ObjType=" + ObjType + "&ResultMasterID=" + ResultMasterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&CurrentProcess=" + CurrentProcess + "&Opening_from=OrdersQ&Openingfrom=MyorderQueue";//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "RESULT_REVIEW" || orderType == "DICTATION_REVIEW") {
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "97%";
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&OrderSubmitId=" + OrderSubmitID + "&EncounterId=" + EncounterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&Opening_from=OrdersQ&Openingfrom=MyorderQueue&CurrentProcess=" + CurrentProcess + "&File_Ref_ID=" + File_Ref_ID;//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "RESULT_REVIEW" || orderType == "IMMUNIZATION ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmImmunization.aspx?HumanID=" + $(currRow)[0].children[2].innerText.trim() + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText.trim() + "&EncounterID=" + $(currRow)[0].children[11].innerText.trim() + "&PhysicianID=" + $(currRow)[0].children[12].innerText.trim() + "&LabId=" + $(currRow)[0].children[15].innerText.trim() + "&Openingfrom=MyorderQueue&Screen=MyQ", 800, 1135, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                StopLoadingImage();
                return false;
            }
            else if (CurrentProcess == "MA_RESULTS") {
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "97%";
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&OrderSubmitId=" + OrderSubmitID + "&EncounterId=" + EncounterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&Opening_from=OrdersQ" + "&MA=True&Openingfrom=MyorderQueue&CurrentProcess=" + CurrentProcess + "&File_Ref_ID=" + File_Ref_ID;//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "MA_REVIEW" && orderType == "REFERRAL ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmReferralOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText.trim() + "&EncounterID=" + $(currRow)[0].children[11].innerText.trim() + "&PhysicianId=" + $(currRow)[0].children[12].innerText.trim() + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText.trim() + "&Openingfrom=MyorderQueue&ScreenMode=Myqueue", 750, 1088, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                StopLoadingImage();
                return false;
            }
            else if (CurrentProcess == "PHYSICIAN_VERIFY" && orderType == "REFERRAL ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmReferralOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText.trim() + "&EncounterID=" + $(currRow)[0].children[11].innerText.trim() + "&PhysicianId=" + $(currRow)[0].children[12].innerText.trim() + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText.trim() + "&Openingfrom=MyorderQueue&ScreenMode=Myqueue", 830, 1088, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                StopLoadingImage();
                return false;
            }

        }
        else {
            End();
            StopLoadingImage();
        }
    }
    else {
        End();
        StopLoadingImage();
    }
}

function GetNextViewResult(next_index) {
    var trlength;
    if ($($(top.window.document).find("table[id=EncounterTable] tbody")).find("tr").length != undefined && $($(top.window.document).find("table[id=EncounterTable] tbody")).find("tr").length > 0) {

        trlength = $($(top.window.document).find("table[id=EncounterTable] tbody")).find("tr").length;
        
        for (var i = next_index; i < trlength; i++) {
            var currRow = $($(top.window.document).find("table[id=EncounterTable] tbody")[0]).find("tr:eq('" + i + "')");
            if ($(currRow).length > 0) {
                var CurrentProcess = $(currRow)[0].children[8].innerText.trim();
                var orderType = $(currRow)[0].children[15].innerText.replace("INTERNAL", "").trim();
                var ObjType = $(currRow)[0].children[15].innerText.trim();
                if ((ObjType.toUpperCase() == "DIAGNOSTIC_RESULT") || (CurrentProcess == "RESULT_REVIEW" || orderType == "DICTATION_REVIEW") || (CurrentProcess == "MA_RESULTS")) {
                    sessionStorage.setItem("Next_Order_Index", parseInt(i) + 1);
                    return i;
                }
                else {
                    continue;
                }
            }
        }
    }
        return 0;
}

//END -- BugID:41027 -- move to next result