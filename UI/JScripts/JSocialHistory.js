//CAP-1158
//CAP-1365
$('*[id*=_listDLC]').keypress(function (event) {
    if (event.keyCode === 10 || event.keyCode === 13) {
        event.preventDefault();
    }
});

//check the particular checkbox
function enableField(idCheckbox) {
    var divid = document.getElementById("divSocialHistoryControls");
    if (divid.scrollHeight != null && divid.scrollHeight != undefined)
        hdnDivPosition.value = divid.scrollHeight;

    var ctrlchk = document.getElementById(idCheckbox);
    if (ctrlchk.checked == true) {
        var value = null;
        if (idCheckbox.substring(3, 6) == "Yes")
            value = idCheckbox.replace("Yes", "No");
        else if (idCheckbox.substring(3, 5) == "No")
            value = idCheckbox.replace("No", "Yes");
        if (document.getElementById(value).checked == true)
            document.getElementById(value).checked = false;
        if (idCheckbox.substring(3, 6) == "Yes")
            disable(idCheckbox, "chkYes");
        else
            disable(idCheckbox, "chkNo");
    }
    else {
        if (idCheckbox.substring(3, 6) == "Yes") {
            enable(idCheckbox, "chkYes")
        }
        else {

            enable(idCheckbox, "chkNo")
        }
    }
    EnableSave();
    return;
}
function warningmethod() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
function LoadSocialHistory()
{

    
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=No]").addClass('LabelStyle');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
}
function displayalerttobacco()
{
    //CAP-1320
    PFSH_SaveUnsuccessful();
    alert("Please Pick at least one Reason Not Performed for Tobacco Use and Exposure and do not modify the selected item");
        return false
}
//check the checkbox YesorNo
function CheckChanged(chkids) {
    EnableSave();
    LoadTobaccoList();
    var chkid = document.getElementById(chkids);
    var ctrl = document.getElementsByTagName('INPUT');
    for (var i = 0; i < ctrl.length; i++) {
        if (ctrl[i].type == 'checkbox') {
            if (chkid.checked == true) {
                var value = null;
                if (ctrl[i].id.substring(3, 6) == chkid.id.substring(6, 9)) {
                    var sum = ctrl[i].id.replace("Yes", "No");
                    if (document.getElementById(sum).checked == true)
                        document.getElementById(ctrl[i].id).checked = false;
                    else
                        disable(ctrl[i].id, "chkYes");
                }
                else if (ctrl[i].id.substring(3, 5) == chkid.id.substring(6, 8)) {
                    var sum = ctrl[i].id.replace("No", "Yes");
                    if (document.getElementById(sum).checked == true)
                        document.getElementById(ctrl[i].id).checked = false;
                    else
                        disable(ctrl[i].id, "chkNo");
                }
                if (chkid.id.substring(6, 9) == "Yes") {
                    value = chkid.id.replace("Yes", "No");
                }
                if (chkid.id.substring(6, 8) == "No") {
                    value = chkid.id.replace("No", "Yes");
                }
                if (value != null) {
                    if (document.getElementById(value).checked == true) {
                        document.getElementById(value).checked = false;
                    }
                }
            }
            else {
                if (ctrl[i].id.substring(3, 6) == chkid.id.substring(6, 9)) {
                    //alert("Yes");
                    if (document.getElementById(ctrl[i].id.replace("Yes", "No")).checked == false)
                        enable(ctrl[i].id, "chkYes");
                }
                else if (ctrl[i].id.substring(3, 5) == chkid.id.substring(6, 8)) {
                    //alert("No");
                    if (document.getElementById(ctrl[i].id.replace("No", "Yes")).checked == false)
                        enable(ctrl[i].id, "chkNo");
                }
            }
        }
    }
}
function disable(ctrlId, ctrlName) {
    EnableSave();
    document.getElementById(ctrlId).checked = true;
    var crltxt = ctrlId.replace(ctrlName, "DLC") + "_txtDLC";
    var ctrlcbo = ctrlId.replace(ctrlName, "cbo");
    //Cap - 3604
    var ctrltxt = ctrlId.replace(ctrlName, "txt");
    var ctrlimg = ctrlId.replace(ctrlName, "img");

    // var combo = $find(ctrlcbo);
    var combo = document.getElementById(ctrlcbo);
    //Cap - 3604
    var txtbox = document.getElementById(ctrltxt);

    if (ctrlName == "chkYes" && ctrlId == 'chkYesOccupationIndustry') {
        txtbox.disabled = false;
        document.getElementById(ctrlimg).removeAttribute("disabled");
    }
    else if (ctrlName == "chkNo" && ctrlId == 'chkNoOccupationIndustry') {
        txtbox.disabled = true;
        document.getElementById(ctrlimg).setAttribute("disabled", "disabled");
        txtbox.value = "";
    }
    else {
        if (ctrlName == "chkYes" && ctrlId != "chkYesSexuallyActive" && ctrlId != "chkYesPregnancyStatus") {
            //combo.enable();
            combo.disabled = false;
        }
        else {
            //combo.disable();

            $("select#" + ctrlcbo).prop('selectedIndex', 0);
            combo.disabled = true;
        }
    }
    if (ctrlId == "chkNoTobaccoUseandExposure" || ctrlId == "chkYesTobaccoUseandExposure") {
        combo.disabled = false;

    }
    defaultselectionTobacca(ctrlId);

}
//Cap - 3604
function ClearTextbox(e) {
    var Idval = e.target.id.replace("img", "txt");
    document.getElementById(Idval).value = "";
    $("#txtOccupationIndustry").attr("OccupationVal", "");
    $("#txtOccupationIndustry").val = "";
    document.getElementById("txtOccupationIndustry").disabled = false;
}

//Cap - 3604
function myAutocomplete() {
    $("#txtOccupationIndustry").autocomplete({
        source: function (request, response) {
            if ($("#txtOccupationIndustry").val().trim().length > 2) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                var strkeyWords = $("#txtOccupationIndustry").val().split(' ');
                var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                arrPatient = [];
                var WSData = {
                    text_searched: strkeyWords[0]
                };

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "./frmHistorySocial.aspx/GetOccupationIndustry",
                    data: JSON.stringify(WSData),
                    dataType: "json",
                    success: function (data) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                        var jsonData = $.parseJSON(data.d);
                        if (jsonData.Error != undefined) {
                            alert(jsonData.Error);
                            return;
                        }
                        if (jsonData.Result != undefined) {
                            var no_matches = [];
                            no_matches.push(jsonData.Result);
                            response($.map(no_matches, function (item) {
                                return {
                                    label: item,
                                    val: ""
                                }
                            }));
                        }
                        else {
                            var results;
                            if (bMoreThanOneKeyword)
                                results = Filter(jsonData, request.term);
                            else
                                results = jsonData.Matching_Result;

                            arrPatient = jsonData.Matching_Result;
                            response($.map(results, function (item) {
                                return {
                                    label: item.Value,
                                    val: JSON.stringify(item.Value),
                                    value: item.Value
                                }
                            }));
                        }
                    }, error: function OnError(xhr) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        if (xhr.status == 999)
                            window.location = "/frmSessionExpired.aspx";
                        else {
                            var log = JSON.parse(xhr.responseText);
                            console.log(log);
                            alert("USER MESSAGE:\n" +
                                ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                "Message: " + log.Message);
                        }


                    }
                })

            }
        },
        minlength: 0,
        multiple: true,
        mustMatch: false,
        select: OccupationSelect,
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width("312px");
            $('.ui-autocomplete.ui-menu.ui-widget').css("left", "19.7%");
            $('.ui-autocomplete.ui-menu.ui-widget').find('li').css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" });
            //$('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $('#txtOccupationIndustry').focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {

        arrPatient = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#ctl00_C5POBody_txtPlanSearch").css("color", "black").attr({ "dataVal": "" });


    }).on("click", function (e) {

    }).on("focus", function (e) {

    })

    $("#txtOccupationIndustry").data("ui-autocomplete")._renderItem = function (ul, item) {

        if (item.label != "No matches found.") {

            var list_item = $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .append(item.label)
                .appendTo(ul);

            return list_item;
        }
        else
            return $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .addClass("disabled")
                .append(item.label)
                .appendTo(ul).on("click", function (e) {
                    e.preventDefault();
                    e.stopImmediatePropagation();
                });
    };
}

function OccupationSelect(event, ui) {
    $("#txtOccupationIndustry").val = ui.item.value;
    $("#txtOccupationIndustry").attr("OccupationVal", ui.item.value);
    document.getElementById("txtOccupationIndustry").disabled = true;
    EnableSave();
}

//CAP-1760 - Tobacco use & exposure - NO but "Light cigrette smoker" status is selected by default
//function defaultselectionTobacca(ctrlId) {
//    if(ctrlId == "chkYesTobaccoUseandExposure")
//    {
//        //var cmbbx = $find("cboTobaccoUseandExposure");
//        var cmbbx = document.getElementById("cboTobaccoUseandExposure");
//        var indexyes = -1;
//        //var cItem = cmbbx.findItemByValue("TobaccoUseandExposure-230060001-Light cigarette smoker");
//        for (var i = 0; i < cmbbx.options.length; i++)
//        {

//            if (cmbbx.options[i].value == "TobaccoUseandExposure-230060001-Light cigarette smoker") {
//                $("select#cboTobaccoUseandExposure").prop('selectedIndex', i);
//                }

//        }
//        //cItem.select();

//    }
//    else if(ctrlId == "chkNoTobaccoUseandExposure")
//    {
//        //var cmbbx = $find("cboTobaccoUseandExposure");
//        var cmbbx = document.getElementById("cboTobaccoUseandExposure");
//        var index = -1;
//        //var cItem = cmbbx.findItemByValue("TobaccoUseandExposure-160618006-Current non-smoker");
//        //cItem.select();
//        for (var i = 0; i < cmbbx.options.length; i++) {

//            if (cmbbx.options[i].value == "TobaccoUseandExposure-160618006-Current non-smoker") {
//                $("select#cboTobaccoUseandExposure").prop('selectedIndex', i);
//                }

//        }
//    }
//}
function defaultselectionTobacca(ctrlId) {
    if (ctrlId == "chkYesTobaccoUseandExposure" || ctrlId == "chkNoTobaccoUseandExposure") {
        $("select#cboTobaccoUseandExposure").prop('selectedIndex', -1);
    }
}
function enable(testId, chkName) {
    EnableSave();
    document.getElementById(testId).checked = false;
    var crltxt = testId.replace(chkName, "DLC") + "_txtDLC";
    var ctrlcbo = testId.replace(chkName, "cbo")
    var ctrltxt = testId.replace(chkName, "txt");
    var ctrlimg = testId.replace(chkName, "img");

    //var combo = $find(ctrlcbo);
    var combo = document.getElementById(ctrlcbo);
    //combo.disable();
    var txtbox = document.getElementById(ctrltxt);

    if (chkName == "chkYes" && testId == 'chkYesOccupationIndustry') {
        txtbox.disabled = true;
        document.getElementById(ctrlimg).setAttribute("disabled", "disabled");
        txtbox.value = "";
    }
    else if (chkName == "chkNo" && testId == 'chkNoOccupationIndustry') {
        txtbox.disabled = true;
        document.getElementById(ctrlimg).setAttribute("disabled", "disabled");
        txtbox.value = "";
    }
    else {
        if (combo.id == 'cboTobaccoUseandExposure' && combo.item(0).text != "") {
            var option = document.createElement("option");
            option.text = "";
            option.value = "";
            option.setAttribute("option", "Yes");
            combo.add(option, combo[0]);
        }
        combo.disabled = true;
        $("select#" + ctrlcbo).prop('selectedIndex', 0);
    }

    //combo.clearSelection();

    var TFamilyDiseaseControlD = testId.replace(chkName, "DLC") + "_listDLC";
    document.getElementById(TFamilyDiseaseControlD).style.display = "none";
    var listcontrolSocialHistory = document.getElementById(testId.replace(chkName, "DLC") + "_pbDropdown");
    if (listcontrolSocialHistory.childNodes[0] != undefined && listcontrolSocialHistory.childNodes[0].className != null)
        listcontrolSocialHistory.childNodes[0].className = "fa fa-plus margin2";
    else if (listcontrolSocialHistory.childNodes[0] != undefined && listcontrolSocialHistory.childNodes[0].nextSibling.className != null)
        listcontrolSocialHistory.childNodes[0].nextSibling.className = "fa fa-plus margin2";
}
function EnableSave(id) {
    //CAP-780:Cannot read properties of null - jsSocialHistory
    $find('btnSave')?.set_enabled(true);
    localStorage.setItem("bSave", "false");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    else
        window.parent.theForm.hdnSaveEnable.value = true;
}


function btnClearAll_Clicked(sender, args) {
    var IsClearAll = DisplayErrorMessage('180010');
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    if (IsClearAll == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
        document.getElementById('InvisibleButton').click();
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        if (sender != undefined)
            sender.set_autoPostBack(true);
        $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    }
    else {
        if (sender != undefined)
            sender.set_autoPostBack(false);
       
           
    }
    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
}

function OnClientSelectedIndex(sender, eventArgs) {
    EnableSave();
    
    //var item = eventArgs.get_item();
    //sender.set_text(item.get_text());
}
function btnSave_Clicked(sender, args) {
    //CAP-1235
    if (document.getElementById("DLC_txtDLC")!=null && document.getElementById("DLC_txtDLC")!=undefined && document.getElementById("DLC_txtDLC").value != "" && document.getElementById("DLC_txtDLC").value.length > 32767) {
        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180032');
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("chkYesSmokinghabit") != null && (document.getElementById("chkYesSmokinghabit").checked == false && document.getElementById("chkNoSmokinghabit").checked == false && document.getElementById("lblSmokinghabit").getAttribute("class") == "MandLabelstyle")) {
        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180020');
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("chkYesTobaccoUseandExposure") != null && (document.getElementById("chkYesTobaccoUseandExposure").checked == false && document.getElementById("chkNoTobaccoUseandExposure").checked == false && document.getElementById("lblTobaccoUseandExposure").getAttribute("class") == "MandLabelstyle" && document.getElementById("DLCTobaccoUseandExposure_txtDLC") != null && document.getElementById("DLCTobaccoUseandExposure_txtDLC").value == "")) {    //added by Shilpa for Reason_Not_Performed cbo
        PFSH_SaveUnsuccessful();
            DisplayErrorMessage('180020');
            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
    }
    else if (document.getElementById("chkYesTobaccoUseandExposure") != null && (document.getElementById("chkYesTobaccoUseandExposure").checked == true && document.getElementById("lblTobaccoUseandExposure").getAttribute("class") == "MandLabelstyle") && document.getElementById("cboTobaccoUseandExposure").value == "") {
        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180055');
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("chkNoTobaccoUseandExposure") != null && (document.getElementById("chkNoTobaccoUseandExposure").checked == true && document.getElementById("lblTobaccoUseandExposure").getAttribute("class") == "MandLabelstyle") && document.getElementById("cboTobaccoUseandExposure").value == "") {
        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180055');
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("chkYesDrugUse") != null && (document.getElementById("chkYesDrugUse").checked == false && document.getElementById("chkNoDrugUse").checked == false && document.getElementById("lblDrugUse").getAttribute("class") == "MandLabelstyle")) {
        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180020');
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("txtOccupationIndustry").value != undefined && document.getElementById("txtOccupationIndustry").value != "" && ($("#txtOccupationIndustry").attr("OccupationVal") == undefined || $("#txtOccupationIndustry").attr("OccupationVal") == "")) {
        PFSH_SaveUnsuccessful();
        DisplayErrorMessage('180058');
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
        else
            window.parent.theForm.hdnSaveEnable.value = false;
        //CAP-1887
        __doPostBack('btnSave', "true");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    }
}

function AddUsersKeyDown(evtobj) {
    $find('btnSave').set_enabled(true);
    localStorage.setItem("bSave", "false");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
}

function SavedSuccessfully() {
    
    localStorage.setItem("bSave", "true");
    DisplayErrorMessage('180003');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
    //CAP-2678
    localStorage.setItem('IsSaveCompleted', true);
}

function HideAllControls() {
    document.getElementById("divSocialHistory").style.display = 'none';
    document.getElementById("SummaryAlert").style.display = '';
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

function LoadcboTobacco(sender, args) {
   // var cboTobacco = $telerik.findComboBox('cboTobaccoUseandExposure');
    var cboTobacco = document.getElementById("cboTobaccoUseandExposure");
    if (cboTobacco != null) {
        var chkValue = "None";
        if (document.getElementById('chkYesTobaccoUseandExposure') != null && document.getElementById('chkYesTobaccoUseandExposure').checked == true)
            chkValue = true;
        else if (document.getElementById('chkNoTobaccoUseandExposure') != null && document.getElementById('chkNoTobaccoUseandExposure').checked == true)
            chkValue = false;
        //if (cboTobacco.get_attributes()._data.CheckedValue.toLowerCase() == "true" || cboTobacco.get_attributes()._data.CheckedValue.toLowerCase() == "false")
        //    chkValue = JSON.parse(cboTobacco.get_attributes()._data.CheckedValue.toLowerCase());
        //CAP-1874
        //if (cboTobacco.attributes.checkedvalue.value.toLowerCase() == "true" || cboTobacco.attributes.checkedvalue.value.toLowerCase() == "false")
        //    chkValue = JSON.parse(cboTobacco.attributes.checkedvalue.value.toLowerCase());
        //var cboItems = cboTobacco.get_items();
        //cboTobacco.trackChanges();
        var cboItems = cboTobacco.options;
        if (cboItems.item(0).value != "" && chkValue == "None" && (document.getElementById('chkYesTobaccoUseandExposure').checked == false || document.getElementById('chkNoTobaccoUseandExposure').checked == false)) {
            var option = document.createElement("option");
            option.text = "";
            option.value = "";
            option.setAttribute("option", "Yes");
            cboItems.add(option, cboItems[0]);
            $("select#" + cboTobacco.id).prop('selectedIndex', 0);
        }
        for (i = 0; i < cboItems.length; i++) {
            var item = cboItems.item(i);
            //if (chkValue == "None" && (item.get_attributes()._data.Option == "Yes" || item.get_attributes()._data.Option == "No"))
            if (chkValue == "None" && (item.attributes['option'].value == "Yes" || item.attributes['option'].value == "No")) {
                ////item.hide();
                item.style.display = "none";
            }
            else {
                //if (item.get_attributes()._data.Option == "Yes" && chkValue == true) {
                if (item.attributes['option'].value == "Yes" && chkValue == true) {
                    //item.show();
                    item.style.display = "block";
                }
                //else if (item.get_attributes()._data.Option == "No" && chkValue == false) {
                else if (item.attributes['option'].value == "No" && chkValue == false) {
                    //item.show();
                    item.style.display = "block";
                }
                //else if (item.get_attributes()._data.Option == "Yes" || item.get_attributes()._data.Option == "No")
                else if (item.attributes['option'].value == "Yes" || item.attributes['option'].value == "No")
                    //item.hide();
                    item.style.display = "none";
                //else if (item.get_attributes()._data.Option == undefined)
                else if (item.attributes['option'].value == undefined)
                    //item.show();
                    item.style.display = "block";
                //CAP-2143
                else if (item.attributes['option'].value == "")
                    item.style.display = "none";
            }
           
        }
        //cboTobacco.commitChanges();
    }
}

function LoadTobaccoList() {
    //var cboTobacco = $telerik.findComboBox('cboTobaccoUseandExposure');
    var cboTobacco = document.getElementById("cboTobaccoUseandExposure");
    if (cboTobacco.item(0).value == "")
    {
        $("#" + cboTobacco.id+" option[value='']").remove(); 
    }
    
    if (cboTobacco != null) {
       // cboTobacco.clearSelection();
        //var cboItems = cboTobacco.get_items();
        var cboItems = cboTobacco.options;
       // cboTobacco.trackChanges();
        var hidden = 'No', shown = 'Yes';
        if ((event.target.checked && event.target.id.indexOf("chkNo") > -1) || (event.target.checked && event.target.id.indexOf("chkAllNo") > -1)) {
            hidden = 'Yes';
            shown = 'No';
        }
        else if ((event.target.checked && event.target.id.indexOf("chkYes") > -1) || (event.target.checked && event.target.id.indexOf("chkAllYes") > -1)) {
            hidden = 'No';
            shown = 'Yes';
        }
        else {
            hidden = "Both";
        }
        for (var i = 0; i < cboItems.length; i++) {
            
            //var item = cboItems.getItem(i);
            var item = cboItems.item(i);
            
            if (hidden == "Both")
                //$(item).hide();
                item.style.display = "none";
            else {
                //if (item.get_attributes()._data.Option == hidden) {
                //CAP-1843, //CAP-2143
                if (item.attributes['option'].value == hidden || item.attributes['option'].value == "") {
                    //item.set_visible(false);
                    item.style.display = "none";
                }
                //else if (item.get_attributes()._data.Option == shown) {
                //CAP-1843
                else if (item.attributes['option'].value == shown) {
                    //item.set_visible(true);
                    item.style.display = "block";
                }
            }
        }
       

        //cboTobacco.commitChanges();
        if (event.target.id.indexOf("chkNo") > -1 || event.target.id.indexOf("chkYes") > -1)
            enableField(event.target.id);
    }


    if (document.getElementById('DLCTobaccoUseandExposure_txtDLC')!=undefined && document.getElementById('DLCTobaccoUseandExposure_txtDLC').value != "") {

        var heightnotes = document.getElementById('DLCTobaccoUseandExposure_txtDLC').value.split(',');
        var resason = document.getElementById('hdnreason').value.split('~');
        for (var i = 0; i < heightnotes.length; i++) {
            for (var j = 0; j < resason.length; j++) {
                if (resason[j].split('|')[1] == heightnotes[i].trim()) {
                    if ((heightnotes.length == 0 && i == 0) || i == heightnotes.length - 1)
                        document.getElementById('DLCTobaccoUseandExposure_txtDLC').value = document.getElementById('DLCTobaccoUseandExposure_txtDLC').value.replace(heightnotes[i].trim(), "");

                    else
                        document.getElementById('DLCTobaccoUseandExposure_txtDLC').value = document.getElementById('DLCTobaccoUseandExposure_txtDLC').value.replace(heightnotes[i].trim() + ", ", "");
                }
            }

        }
    }
}