function OpenViewerMaximized(sDocType, sdocdate, sdocPhy) {
    //var test = $(top.window.document).find('#ProcessModalExam').modal();
    //test.modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#ProcessModalExam").modal({ backdrop: 'static', keyboard: false }, 'show');
    //$(top.window.document).find('#ProcessModalExam')[0].style.marginLeft = "5px";
    //$(top.window.document).find('#ProcessModalExam')[0].style.marginTop = "77px";
    $(top.window.document).find("#ProcessFrameExam")[0].style.height = "100%";
    $(top.window.document).find("#mdldlgExam")[0].style.height = "98%";
    $(top.window.document).find("#mdldlgExam")[0].style.width = "86%";
    $(top.window.document).find("#mdldlgExam")[0].style.marginLeft = "100px";
    //$(top.window.document).find("#ProcessModalExam")[0].style.width = "120%";
    //$(top.window.document).find("#ProcessModalExam")[0].style.zIndex = "5000";
    $(top.window.document).find('#ProcessFrameExam')[0].contentDocument.location.href = "frmImageViewer.aspx?Source=EXAM&DocType=" + sDocType + "&DocDate=" + sdocdate + "&DocPhy=" + sdocPhy;
    $(top.window.document).find("#ModalTitleExam")[0].textContent = "Image Viewer - " + sDocType + " ( " + $(window.top.document).find('#ctl00_C5POBody_lblPatientStrip').text() + " )";;

    return false;
}
function OpenViewerforCompare(sDocType, sdocdate, sdocPhy) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(top.window.document).find('#ProcessModalCompare').modal({ backdrop: 'static', keyboard: false }, 'show');
    //$(top.window.document).find('#ProcessModalCompare')[0].style.marginLeft = "10px";
    //$(top.window.document).find('#ProcessModalCompare')[0].style.marginTop = "65px";
    $(top.window.document).find("#ProcessFrameCompare")[0].style.height = "100%";
    $(top.window.document).find("#mdldlgCompare")[0].style.height = "98%";
    $(top.window.document).find("#mdldlgCompare")[0].style.width = "90%";
    //$(top.window.document).find("#ProcessModalCompare")[0].style.width = "1250px";
    //$(top.window.document).find("#ProcessModalCompare")[0].style.zIndex = "5000";
    $(top.window.document).find('#ProcessFrameCompare')[0].contentDocument.location.href = "frmImageCompare.aspx?Source=EXAM&DocType=" + sDocType + "&DocDate=" + sdocdate + "&DocPhy=" + sdocPhy;
    $(top.window.document).find("#ModalTitleCompare")[0].textContent = "Image Compare - " + sDocType + " ( " + $(window.top.document).find('#ctl00_C5POBody_lblPatientStrip').text() + " )";

    return false;
}

function downloadURI(uri, filename) {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    var link = document.createElement('a'); link.download = filename; link.href = uri; link.click();
}



Telerik.Web.UI.RadButton.prototype._click = Telerik.Web.UI.RadButton.prototype.click;
function dtpTestTakenDate_OnPopupClosing(sender, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    $find('btnSave').set_enabled(true);
}
function cboGroupType_DropDownClosed(sender, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    $find('btnSave').set_enabled(true);
}
function OnCommand(sender, args) {

    if (args.get_commandName() == "ViewRow" || args.get_commandName() == "DeleteRow") { { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } }
}

Telerik.Web.UI.RadButton.prototype.click = function () {
    this._click();
}
function openAddorUpdate() {
    var library_button = document.getElementById("hdnLibraryCheck");
    if ($find("btnSave")._enabled == true)
        library_button.value = "true";
    else
        library_button.value = "false";

    var focused = "EXAM GROUP";
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
    $(top.window.document).find("#Modal").one("hidden.bs.modal", function () {
        CloseExamPhotos();
    });
   
    return false;
}
function CloseExamPhotos(oWindow, args) {
    document.getElementById('btnLibrary').click();
}
function ReloadOnClientClose(oWindow, args) { { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } document.getElementById('LibraryButton').click(); }


function btnClear_Clicked(sender, args) {
    var ArguOne = "clear";
    if (sender == undefined || sender.get_text() == "Clear") {
        ArguOne = "clear";
    } else {
        ArguOne = "cancel";
    }
    if (DisplayErrorMessage("270108", '', ArguOne)) {
        document.getElementById('btnClearAll').click();
    }



}

function OnClientBeforeClose(sender, args) {
    returnToParent();
}

function chkShowAllPhysicians_CheckedChanged(checkStatus) {
    var chkstatus;
    if (checkStatus == undefined) {
        chkstatus = document.getElementById("chkShowAllPhysicians").checked;
    }
    else
        chkstatus = checkStatus.firstChild.checked;
    var val = [];
    if (chkstatus) {
        var usedNames = {};
        $("#cboPhysicianName > option").each(function () {
            if (usedNames[this.text]) {
                $(this).css('display', 'none');
            } else {
                usedNames[this.text] = this.value;
                $(this).css('display', 'block');
            }
        });
    }
    else {
        $("#cboPhysicianName > option").each(function () {
            var option = $(this);
            if (val.indexOf(option[0].text) == -1) {
                val.push(option[0].text);
                if (option.attr('default') == 'true' || option.attr('default') == '') { option.css('display', 'block'); } else { option.css('display', 'none'); }
            }
        });
    }
}

function getDropdownListSelectedText() {
    var DropdownList = document.getElementById("cboPhysicianName");
    $find('btnSave').set_enabled(true);
    var SelectedIndex = DropdownList.selectedIndex;
    document.getElementById("hdnindex").value = SelectedIndex;
    document.getElementById("hdnLocalPhy").value = DropdownList.value + '~' + DropdownList.selectedOptions[0].textContent.split('-')[0];
    var hdnval = DropdownList.value + '~' + DropdownList.selectedOptions[0].textContent.split('-')[0];

    $.ajax({
        type: "POST",
        url: "frmExamPhotos.aspx/loadcboExamValues",
        data: JSON.stringify({
            "data": hdnval,
        }),
        contentType: "application/json;charset=utd-8",
        async: true,
        success: function (data) {
            var jsondata = $.parseJSON(data.d);
            var cboExam = $telerik.findComboBox("cboGroupType").get_items();
            var len = cboExam.get_count();
            for (var i = len - 1; i >= 0 ; i--) {
                $telerik.findComboBox("cboGroupType").get_items().remove($telerik.findComboBox("cboGroupType").get_items().getItem(i));
            }
            for (var i = 0; i < jsondata.length ; i++) {
                var comboItem = new Telerik.Web.UI.RadComboBoxItem();
                comboItem.set_text(jsondata[i]);
                $telerik.findComboBox("cboGroupType").get_items().add(comboItem);
            }
            if ($telerik.findComboBox("cboGroupType").get_items().get_count() > 0) {
                $telerik.findComboBox("cboGroupType").get_items().getItem(0).select();
            }
        },
        error: function () { }
    });
}


function ShowEditForm(Param) {
    var test = Param.split(",");
    var obj = new Array();
    obj.push("HumanId=" + test[0]);
    obj.push("Screen=" + test[1]);
    obj.push("DocumentType=" + test[2]);
    obj.push("DocumentDate=" + test[3]);
    openModal("frmOnlineDocuments.aspx", 600, 800, obj, "RadViewer");
    return false;

}


function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
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
function UpdateKeyWords() {
    var btnUpdateKeywords = document.getElementById(GetClientId('btnUpdateKeywords'));
    btnUpdateKeywords.click();
}

function ShowLoading()
{ top.window.document.getElementById('ctl00_Loading').style.display = 'block'; }
function LoadForm() { ShowLoading(); }

function btnSave_Clicked(sender, args) {

    if ($find("cboGroupType") != null) {
        var test = $find("cboGroupType")._text;
        sender.set_autoPostBack(true);
        var date = $find("dtpTestTakenDate");
        var image = $find("UploadImage");

        var dateVar = new Date();

        if (test == "") {
            SaveUnsuccessful();
            DisplayErrorMessage('270105');
            AutoSaveUnsuccessful();
            sender.set_autoPostBack(false);

        }

        else if (date.get_selectedDate() == null) {
            SaveUnsuccessful();
            DisplayErrorMessage('270115');
            AutoSaveUnsuccessful();
            sender.set_autoPostBack(false);

        }
        else if (date.get_selectedDate() == "") {
            SaveUnsuccessful();
            DisplayErrorMessage('270115');
            AutoSaveUnsuccessful();
            sender.set_autoPostBack(false);
        }

        else if (date.get_selectedDate() > dateVar) {
            SaveUnsuccessful();
            DisplayErrorMessage('270106');
            AutoSaveUnsuccessful();
            sender.set_autoPostBack(false);
        }

        else {
            //CAP-1811
            $('#btnSave').trigger('click');
            sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
        }
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";

        if (document.getElementById("cboPhysicianName") != null && document.getElementById("cboPhysicianName").selectedOptions[0].innerHTML == "") {
            SaveUnsuccessful();
            DisplayErrorMessage('110003');
            AutoSaveUnsuccessful();
            sender.set_autoPostBack(false);
        }

    }
    else if (document.getElementById("hdnFormType").value == "Result Upload") {
        document.getElementById('divWaitLoading').style.display = 'block';
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


function btnView_Clicked(sender, args) {
  
    if (document.URL.split('?').length > 1) {
        var sQuery = document.URL.split('?')[1].split('&');
        var Index = "-1", Sign = "True";
        for (var i = 0; i < sQuery.length; i++) {
            if (sQuery[i].indexOf("IndexOrderID") > -1)
                Index = sQuery[i].split("=")[1];
            else
                if (sQuery[i].indexOf("ElectronicSign") > -1)
                    Sign = sQuery[i].split("=")[1];
        }
        if (sender != undefined) {
            if (sender.get_element().attributes.resultUpload == undefined)
                sender.get_element().attributes.resultUpload = "FALSE";
            if ((Index == "" || (Index == "0" && Sign == "False")) && (sender.get_element().attributes.resultUpload.value != "TRUE")) {
                DisplayErrorMessage("7090010");
                return;
            }
        }
    }

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    $(top.window.document).find("#TabModal").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalTitle")[0].textContent = "View Result";
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "90%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "95%";

    var sPath = "frmViewResult.aspx?Screen=ResultView" + "&HumanId=" + document.getElementById("hdnHumanId").value + "&OrderSubmitId=" + document.getElementById("hdnOrderSubmitId").value + "&CurrentProcess=" + document.getElementById("hdnCurrentProcess").value + "&MoveToMA=" + document.getElementById("hdnMoveToMAID").value + "&ResultId=0";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = sPath;
    return false;
}


function WinClose() {
    var oWnd = GetRadWindow();
    oWnd.close();
}
function UploadImage_FileUploading(sender, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
        $find('btnSave').set_enabled(true);
    }
    else {
        $find('btnSave').set_enabled(true);
    }
}

function UploadImage_FileUploadRemoved(sender, args) {
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    }
}
function testmethod() {
    document.getElementById('Invisiblebtn').click();

}
function ExamPhotos_Load() {
  
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("span[mand=Yes]").addClass('MandLabelstyle');
}
function CloseOnlineDocument() {
    document.getElementById("btnDisableOnload").click();
}


function SavedSuccessfully() {
   
    DisplayErrorMessage('270001');
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    SavedSuccessfully_NowProceed(screen_name);
    //CAP-2678
    localStorage.setItem("IsSaveCompleted", true);
}
var IsMove = false;
function MoveToNextProcess(sender, args) {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    if (!$(top.window.document).find('body').is('#dvdialogMenu'))
        $(top.window.document).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
            '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">Do you want to Move To Next Process?</p></div>');
    dvdialog = $(top.window.document).find('body').find('#dvdialogMenu');
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

                IsMove = true;

                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

                $(dvdialog).dialog("close");
                self.close();
                $('#btnMoveToNextProcess').trigger('click');
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
function disableAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    if ($('#btnSave') != null && $('#btnSave')!="undefined")
       if($('#btnSave').disabled!=undefined)
            $('#btnSave')[0].disabled = true;
}