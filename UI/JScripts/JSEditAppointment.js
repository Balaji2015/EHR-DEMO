var intProviderlen = -1;
var arrProvider = [];
var flag = 0;
function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}
function chkchange() {
    if (document.getElementById("chkSelfReferred").checked)
        document.getElementById("hdbselref").value = 'Y';
    else
        document.getElementById("hdbselref").value = 'N';
}
$(document).ready(function () {

    //Jira #CAP-366 - Avoid Save while pressing the Enter Key
    $("#dtpStartTime_dateInput").keydown(function (event) {
        if (event.keyCode == 13 && event.target.id.indexOf("dtpStartTime") > -1) {
            event.preventDefault();
            return false;
        }
    });
    $("#dtpApptDate_dateInput").keydown(function (event) {
        if (event.keyCode == 13 && event.target.id.indexOf("dtpApptDate") > -1) {
            event.preventDefault();
            return false;
        }
    });
    $("#ddlDuration").keydown(function (event) {
        if (event.keyCode == 13 && event.target.id.indexOf("ddlDuration") > -1) {
            event.preventDefault();
            return false;
        }
    });

    var curleft = curtop = 0;
    var current_element = document.getElementById('txtPatientSearch');
    if (current_element == null) {
        current_element = document.getElementById('txtProviderSearch');
        curtop = 5;
    }
    window.setTimeout(function () {
        $('#txtProviderSearch').focus();
    }, 50);

    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }

    if ($("#txtProviderSearch").length > 0) {
        $("#txtProviderSearch").autocomplete({
            source: function (request, response) {
                 if ($("#txtProviderSearch").val().trim().length > 2) {
                    if (intProviderlen == 0) {

                        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                        this.element.on("keydown", PreventTyping);
                        arrProvider = [];
                        var strkeyWords = $("#txtProviderSearch").val().split(' ');
                        var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                        var sIsMenuLevel = "";
                        var WSData = {
                            text_searched: strkeyWords[0],
                            IsMenulevel: sIsMenuLevel,
                        };
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "./frmFindReferralPhysician.aspx/GetProviderDetailsByTokens",
                            data: JSON.stringify(WSData),
                            dataType: "json",
                            success: function (data) {
                                flag = 0;
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                $("#txtProviderSearch").off("keydown", PreventTyping);
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
                                            val: "0"
                                        }
                                    }));
                                }
                                else {
                                    var results;
                                    if (bMoreThanOneKeyword)
                                        results = Filter(jsonData.Matching_Result, request.term);
                                    else
                                        results = jsonData.Matching_Result;

                                    arrProvider = jsonData.Matching_Result;
                                    response($.map(results, function (item) {
                                        return {
                                            label: item.label,
                                            val: JSON.stringify(item.value),
                                            value: item.value.ulPhyId
                                        }
                                    }));
                                }
                            },
                            error: function OnError(xhr) {
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

                        });
                    }
                    else if (intProviderlen != -1) {

                        var results = Filter(arrProvider, request.term);
                        response($.map(results, function (item) {
                            return {
                                label: item.label,
                                val: JSON.stringify(item.value),
                                value: item.value.ulPhyId
                            }
                        }));
                    }
                }
            },
            minlength: 0,
            multiple: true,
            mustMatch: false,
            select: ProviderSelected,
            open: function () {
                $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtProviderSearch').width());
                $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
                $('#txtProviderSearch').focus();
            },
            focus: function () { return false; }
        }).on("paste", function (e) {
            intProviderlen = -1;
            arrProvider = [];
            $(".ui-autocomplete").hide();
        }).on("input", function (e) {
            $("#txtProviderSearch").css("color", "black").attr({ "data-phy-id": "0", "data-phy-details": "" });

            if ($("#txtProviderSearch").val().charCodeAt(e.currentTarget.value.length - 1) == 10) {
                e.preventDefault();
                $("#txtProviderSearch").val(e.currentTarget.value.substr(0, e.currentTarget.value.length - 1));
                return false;
            }
            if ($("#txtProviderSearch").val().charAt(e.currentTarget.value.length - 1) == " ") {
                if (e.currentTarget.value.split(" ").length > 2)
                    intProviderlen = intProviderlen + 1;
                else
                    intProviderlen = 0;
            }
            else {
                if ($("#txtProviderSearch").val().length != 0 && intProviderlen != -1) {
                    intProviderlen = intProviderlen + 1;
                }

                if ($("#txtProviderSearch").val().length == 0 || $("#txtProviderSearch").val().indexOf(" ") == -1) {
                    intProviderlen = -1;
                    arrProvider = [];
                    $(".ui-autocomplete").hide();
                }
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            if (item.label != "No matches found.") {
                if (flag == 0) {
                    flag = 1;
                    $("<li class='alinkstyle'>")
                        .attr({ "data-value": "Add", "data-val": "Add" }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                        .append("Click to add Physician").addClass('alinkstyle').css("font-style", "italic")
                        .appendTo(ul).on("click", function (e) {
                            e.preventDefault();
                            e.stopImmediatePropagation();
                            var obj = new Array();
                            obj.push("Title=" + "PROVIDER LIBRARY");
                            localStorage.removeItem("IsEFax");
                            localStorage.setItem("IsEnableGrid", "false");
                            var result = openModal("frmPhysicianLibray.aspx", 330, 750, obj, "MessageWindow");
                            var WindowName = $find('MessageWindow');

                            return false;
                            //Jira #Cap#193 - Screen Throwing error message
                        }).on("mouseover", function (e) {
                            e.preventDefault();
                            return false;
                        });;;;
                    return $("<li>")
                        .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                        .append(item.label)
                        .appendTo(ul);
                }
                if (flag != 0) {
                    return $("<li>")
                        .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                        .append(item.label)
                        .appendTo(ul);
                }
            }
            else {
                if (flag == 0) {
                    flag = 1;
                    $("<li class='alinkstyle'>")
                        .attr({ "data-value": "Add", "data-val": "Add" }).css({ "border-bottom": "1px solid #ccc", "font-style": "italic", "margin-bottom": "3px", "padding-bottom": "3px" })
                        .append("Click to add Physician").addClass('alinkstyle')
                        .appendTo(ul).on("click", function (e) {
                            var obj = new Array();
                            obj.push("Title=" + "PROVIDER LIBRARY");
                            localStorage.removeItem("IsEFax");
                            localStorage.setItem("IsEnableGrid", "false");
                            var result = openModal("frmPhysicianLibray.aspx", 330, 750, obj, "RadWindow1");
                            var WindowName = $find('MessageWindow');
                            return false;
                            //Jira #Cap#193 - Screen Throwing error message
                        }).on("mouseover", function (e) {
                            e.preventDefault();
                            return false;
                        });;;
                    return $("<li>")
                        .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                        .addClass("disabled")
                        .append(item.label)
                        .appendTo(ul).on("click", function (e) {
                            e.preventDefault();
                            e.stopImmediatePropagation();
                        });
                }
                if (flag != 0) {
                    return $("<li>")
                        .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                        .addClass("disabled")
                        .append(item.label)
                        .appendTo(ul).on("click", function (e) {
                            e.preventDefault();
                            e.stopImmediatePropagation();
                        });
                }
            }
        };
    }
    if (document.getElementById("chkSelfReferred") != null) {
        if (document.getElementById("hdnrenprovidersearch") != null) {
            if (document.getElementById("chkSelfReferred").checked == true) {
                document.getElementById("txtProviderSearch").value = "";
                document.getElementById("txtProviderSearch").disabled = true;
                if (document.getElementById("imgEditProvider")?.style?.display != undefined && document.getElementById("imgEditProvider")?.style?.display != null) {
                    document.getElementById("imgEditProvider").style.display = "none";
                }
            }
            else {
                document.getElementById("txtProviderSearch").value = document.getElementById("hdnrenprovidersearch").value;
                if (document.getElementById("imgClearProviderText") != null) {
                    var cLength = document.getElementById("imgClearProviderText").attributes.length;
                    var IsDisabledProviderSearch = "true";
                    for (var i = 0; i < cLength; i++) {
                        if (document.getElementById("imgClearProviderText").attributes[i].name == "onclick") {
                            IsDisabledProviderSearch = "false";
                            break;
                        }
                    }
                }
                if (document.getElementById("txtProviderSearch").value != "" && document.getElementById("hdnrenprovidersearch").value != "| NPI: | Facility: | Address:| Phone No:| Fax No:") {
                    document.getElementById("txtProviderSearch").disabled = true;
                    if (document?.getElementById("imgEditProvider")?.style != undefined && document?.getElementById("imgEditProvider")?.style != null) { document.getElementById("imgEditProvider").style.display = "none"; }
                }
                else {
                    //Jira #CAP-158 -  Not able to navigate tab
                    if (document.getElementById("hdnCurrentProcess").value != "" && document.getElementById("hdnCurrentProcess").value.toUpperCase() == "SCHEDULED") {
                        if (IsDisabledProviderSearch == "true")//change
                        {
                            document.getElementById("txtProviderSearch").disabled = true;
                            if (document?.getElementById("imgEditProvider")?.style != undefined && document?.getElementById("imgEditProvider")?.style != null) { document.getElementById("imgEditProvider").style.display = "none"; }
                        }
                        else {
                            document.getElementById("txtProviderSearch").disabled = false;//have to change
                            if (document?.getElementById("imgEditProvider")?.style != undefined && document?.getElementById("imgEditProvider")?.style != null) { document.getElementById("imgEditProvider").style.display = "block"; }
                        }
                    }
                    document.getElementById("txtProviderSearch").value = "";
                }
            }

        }
    }
    else {
        if (document.getElementById("hdnpcpprovidersearch") != null && document.getElementById("hdnrenprovidersearch").value != "| NPI: | Facility: | Address:| Phone No:| Fax No:") {
            document.getElementById("txtProviderSearch").value = document.getElementById("hdnpcpprovidersearch").value;
            //Jira #CAP-158 -  Not able to navigate tab
            if (document.getElementById("hdnCurrentProcess").value != "" && document.getElementById("hdnCurrentProcess").value.toUpperCase() == "SCHEDULED") {
                if (document.getElementById("hdnpcpprovidersearch").value != "") {
                    document.getElementById("txtProviderSearch").disabled = true;
                    if (document?.getElementById("imgEditProvider")?.style != undefined && document?.getElementById("imgEditProvider")?.style != null) { document.getElementById("imgEditProvider").style.display = "none"; }
                }
                else {
                    document.getElementById("txtProviderSearch").disabled = false;
                    if (document?.getElementById("imgEditProvider")?.style != undefined && document?.getElementById("imgEditProvider")?.style != null) { document.getElementById("imgEditProvider").style.display = "block"; }
                }
            }
        }
    }
    //Jira CAP-2216
    if (document.getElementById("hdnEnableProviderSearch")?.value != undefined
        && document.getElementById("hdnEnableProviderSearch")?.value != null
        && document.getElementById("hdnEnableProviderSearch")?.value != "") {
        EnableProviderSearch(document.getElementById("hdnEnableProviderSearch").value.toLowerCase());
    }

});

function ProviderSelected(event, ui) {
    var ProviderDetails = JSON.parse(ui.item.val);
    var txtProviderSearch = document.getElementById("txtProviderSearch");
    var vLableVal;
    if (JSON.parse(ui.item.val).sPhySuffix != '') {
        vLableVal = JSON.parse(ui.item.val).sPhyshortName + "(" + JSON.parse(ui.item.val).sPhySuffix + ")" + " | " +
            "NPI:" + JSON.parse(ui.item.val).sPhyNPI + " | " +
            "Facility:" + JSON.parse(ui.item.val).sPhyFacility + " | " +
            "Address: " + JSON.parse(ui.item.val).sPhyAddress + ", " +
            JSON.parse(ui.item.val).sPhyCity + "," +
            JSON.parse(ui.item.val).sPhyState + " " +
            JSON.parse(ui.item.val).sPhyZip + " | " +
            "Phone No:" + JSON.parse(ui.item.val).sPhyPhone + " | " +
            "Fax No:" + JSON.parse(ui.item.val).sPhyFax;
    }
    else {
        vLableVal = JSON.parse(ui.item.val).sPhyshortName + " | " +
            "NPI:" + JSON.parse(ui.item.val).sPhyNPI + " | " +
            "Facility:" + JSON.parse(ui.item.val).sPhyFacility + " | " +
            "Address: " + JSON.parse(ui.item.val).sPhyAddress + ", " +
            JSON.parse(ui.item.val).sPhyCity + "," +
            JSON.parse(ui.item.val).sPhyState + " " +
            JSON.parse(ui.item.val).sPhyZip + " | " +
            "Phone No:" + JSON.parse(ui.item.val).sPhyPhone + " | " +
            "Fax No:" + JSON.parse(ui.item.val).sPhyFax;
    }
    //Cap - 1989
    document.getElementById("hdnCategory").value = ProviderDetails.sCategory;
    txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
    //Cap - 1989
    if (document.getElementById("chkSelfReferred") != null && document.getElementById("chkSelfReferred").checked == false) {
        document.getElementById('hdnRefEditPhyId').value = ProviderDetails.ulPhyId;
    }
    else {
        document.getElementById('hdnpcpEditPhyId').value = ProviderDetails.ulPhyId;
    }
    
    txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(ProviderDetails);
    txtProviderSearch.value = vLableVal;
    
    var provider = "";

    provider = JSON.parse(ui.item.val).sPhyName + "|  NPI: " +
        JSON.parse(ui.item.val).sPhyNPI + "| Facility: " +
        JSON.parse(ui.item.val).sPhyFacility + "|  Address:" +
        JSON.parse(ui.item.val).sPhyAddress + ", " + JSON.parse(ui.item.val).sPhyCity + "," + JSON.parse(ui.item.val).sPhyState + " "
        + JSON.parse(ui.item.val).sPhyZip + "| Phone No:" +
        JSON.parse(ui.item.val).sPhyPhone + "| Fax No:" +
        JSON.parse(ui.item.val).sPhyFax


    if (provider == "| NPI: | Facility: | Address:| Phone No:| Fax No:") {
        provider = "";
    }

    if (JSON.parse(ui.item.val).ulPhyId != "" && document.getElementById("chkSelfReferred") == null) {
        document.getElementById("hdnpcpprovider").value = provider;//Result.sPhyName + "|" + Result.sPhyAddress + "|" + Result.sPhyPhone + "|" + Result.sPhyFax + "|" + Result.sPhyNPI.replace("&nbsp;", "") + "|" + Result.sPhyFacility.replace("&nbsp;", "");
        document.getElementById("hdnpcpprovidersearch").value = ui.item.label;
    }
    else if (JSON.parse(ui.item.val).ulPhyId != "" && document.getElementById("chkSelfReferred") != null) {
        document.getElementById("hdnrenprovider").value = provider;// Result.sPhyName + "|" + Result.sPhyAddress + "|" + Result.sPhyPhone + "|" + Result.sPhyFax + "|" + Result.sPhyNPI.replace("&nbsp;", "") + "|" + Result.sPhyFacility.replace("&nbsp;", "");
        document.getElementById("hdnrenprovidersearch").value = ui.item.label;
    }


    $("#txtProviderSearch").attr("disabled", "disabled");
    EnableSaveButton(this);

    return false;
}
function ProviderSearchclear() {
    //Jira #cap-185 - Auto Save warning message not received in Edit Appointment screen
    EnableSaveButton(this);
    document.getElementById("imgEditProvider").style.display = "block";
    $("#txtProviderSearch").attr("disabled", false);
    $("#txtProviderSearch").val("");
    if (document.getElementById("chkSelfReferred") == null) {
        document.getElementById("hdnpcpprovidersearch").value = "";
        document.getElementById("hdnpcpprovider").value = "";
        document.getElementById("hdnRefEditPhyId").value = "";
    }
    else {
        document.getElementById("hdnrenprovider").value = "";
        document.getElementById("hdnrenprovidersearch").value = "";
        document.getElementById("hdnpcpEditPhyId").value = "";
    }

}
$('#btnSave').click(function () {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    //CAP-820 Cannot set properties of undefined
    if (window?.parent?.parent?.theForm?.ctl00_hdnAppoinment?.value != undefined && window?.parent?.parent?.theForm?.ctl00_hdnAppoinment?.value != null) {
        window.parent.parent.theForm.ctl00_hdnAppoinment.value = true;
    }

    if (DateValidattion("dtpApptDate")) {
        DisplayErrorMessage('380006');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    var txtDuration = document.getElementById("ddlDuration").value;
    var sFacility = document.getElementById("hdnFacility").value;
    if (document.getElementById("ddlDuration").value != "") {
        if (sFacility != "true") {
            if (txtDuration > 480) {
                DisplayErrorMessage('110078');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }

    if (DateValidattion("dtpApptDate") == false) {
        DisplayErrorMessage('110074');
        document.getElementById("dtpApptDate").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlPhysicianName").value == "") {
        alert("Please select Provider Name");
        document.getElementById("ddlPhysicianName").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlPhysicianName").value.length == 0) {
        DisplayErrorMessage('110064');
        document.getElementById("ddlPhysicianName").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (sFacility == "true") {
        if (document.getElementById("cboOrder").value.length == 0) {
            DisplayErrorMessage('110086');
            document.getElementById("cboOrder").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (sFacility == "true") {
        if (document.getElementById("cboOrder").value.length > 0) {
            //Cap - 2505
            //if (document.getElementById("txtProviderSearch").value.trim() == "") {
            if (document.getElementById("txtProviderSearch").value.trim() == "" || document.getElementById("hdnrenprovidersearch").value.trim()=="") {
                DisplayErrorMessage('110087');
                document.getElementById("cboOrder").focus();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById("ddlVisitType").value.length == 0) {
        DisplayErrorMessage('110006');
        document.getElementById("ddlVisitType").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlDuration").value.length == 0) {
        DisplayErrorMessage('110007');
        document.getElementById("ddlDuration").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlVisitType").value.toUpperCase() == "CONSULT") {
        if (document.getElementById("txtProviderSearch").value.trim() == "") {
            DisplayErrorMessage('110050');
            document.getElementById("ddlVisitType").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    var dt = new Date();
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + ("0" + now.getUTCDate()).slice(-2) + '/' + now.getUTCFullYear();
    var minutes;
    var seconds;
    if (now.getUTCMinutes() < 10) {
        minutes = '0' + now.getUTCMinutes();
    }
    else {
        minutes = now.getUTCMinutes();
    }
    if (now.getUTCSeconds() < 10) {
        seconds = '0' + now.getUTCSeconds();
    }
    else {
        seconds = now.getUTCSeconds();

    }
    utc += ' ' + now.getUTCHours() + ':' + minutes + ':' + seconds;
    document.getElementById("hdnLocalTime").value = utc;
    if (AppointmentPastDateValidation("dtpApptDate") == false) {

        if (window.confirm("Do you want to create an appointment in the past?") == true) { return true }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

});
function showTime() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.parent.parent.theForm.ctl00_hdnAppoinment.value = true;
    if (DateValidattion("dtpApptDate")) {
        DisplayErrorMessage('380006');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    var txtDuration = document.getElementById("ddlDuration").value;
    var sFacility = document.getElementById("hdnFacility").value;
    if (document.getElementById("ddlDuration").value != "") {
        if (sFacility != "true") {
            if (txtDuration > 480) {
                DisplayErrorMessage('110078');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById("txtPatientDOB").value == "01-Jan-0001") {
        DisplayErrorMessage('110084');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (document.getElementById("txtPatientName").value.length == 0) {
        DisplayErrorMessage('110004');
        document.getElementById("txtPatientName").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("txtPatientAccountNumber").value.length == 0) {
        DisplayErrorMessage('110013');
        document.getElementById("txtPatientAccountNumber").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("dtpApptDate").value.length == 0) {
        DisplayErrorMessage('110005');
        document.getElementById("dtpApptDate").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (DateValidattion("dtpApptDate") == false) {
        DisplayErrorMessage('110074');
        document.getElementById("dtpApptDate").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlPhysicianName").value == "") {
        alert("Please select Provider Name");
        document.getElementById("ddlPhysicianName").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlPhysicianName").value.length == 0) {
        DisplayErrorMessage('110064');
        document.getElementById("ddlPhysicianName").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (sFacility == "true") {
        if (document.getElementById("cboOrder").value.length == 0) {
            DisplayErrorMessage('110086');
            document.getElementById("cboOrder").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (sFacility == "true") {
        if (document.getElementById("cboOrder").value.length > 0) {
            //Cap - 2505
            //if (document.getElementById("txtReferringProvider").value.length == 0) {
            if (document.getElementById("txtReferringProvider").value.length == 0 || document.getElementById("hdnrenprovidersearch").value.trim() == "") {
                DisplayErrorMessage('110087');
                document.getElementById("cboOrder").focus();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    if (document.getElementById("ddlVisitType").value.length == 0) {
        DisplayErrorMessage('110006');
        document.getElementById("ddlVisitType").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlDuration").value.length == 0) {
        DisplayErrorMessage('110007');
        document.getElementById("ddlDuration").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    if (document.getElementById("ddlVisitType").value.toUpperCase() == "CONSULT") {
        if (document.getElementById("txtReferringProvider").value.length == 0) {
            DisplayErrorMessage('110050');
            document.getElementById("ddlVisitType").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    var dt = new Date();
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + ("0" + now.getUTCDate()).slice(-2) + '/' + now.getUTCFullYear();
    var minutes;
    var seconds;
    if (now.getUTCMinutes() < 10) {
        minutes = '0' + now.getUTCMinutes();
    }
    else {
        minutes = now.getUTCMinutes();
    }
    if (now.getUTCSeconds() < 10) {
        seconds = '0' + now.getUTCSeconds();
    }
    else {
        seconds = now.getUTCSeconds();

    }
    utc += ' ' + now.getUTCHours() + ':' + minutes + ':' + seconds;
    document.getElementById("hdnLocalTime").value = utc;
    if (AppointmentPastDateValidation("dtpApptDate") == false) {

        if (window.confirm("Do you want to create an appointment in the past?") == true) { return true }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

}
function OpenRereralPhysician() {

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            var childWindow = oWnd.BrowserWindow.radopen("frmFindReferralPhysician.aspx", "MessageWindow");
            setRadWindowProperties(childWindow, 256, 930);
            childWindow.add_close(function FindReferralPhysicianClick(oWindow, args) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                var Result = args.get_argument();
                if (Result != null) {

                    if (Result.ulPhyId != "") {
                        document.getElementById("HdnPCPID").value = Result.ulPhyId;
                    }
                    if (Result.ulPhyId != "" && document.getElementById("hdnEditApptPhyID").value == Result.ulPhyId && document.getElementById("chkSelfReferred") != null) {
                        document.getElementById("chkSelfReferred").checked = true;
                    }
                    else if (Result.ulPhyId != "" && document.getElementById("hdnEditApptPhyID").value != Result.ulPhyId && document.getElementById("chkSelfReferred") != null) {
                        document.getElementById("chkSelfReferred").checked = false;
                    }
                    if (Result.ulPhyId != "" && document.getElementById("chkSelfReferred") == null) {
                        document.getElementById("HdnPcpPhy").value = Result.sPhyName + "|" + Result.sPhyAddress + "|" + Result.sPhyPhone + "|" + Result.sPhyFax + "|" + Result.sPhyNPI.replace("&nbsp;", "") + "|" + Result.sPhyFacility.replace("&nbsp;", "");
                    }
                    else if (Result.ulPhyId != "" && document.getElementById("chkSelfReferred") != null) {
                        document.getElementById("HdnRefPhy").value = Result.sPhyName + "|" + Result.sPhyAddress + "|" + Result.sPhyPhone + "|" + Result.sPhyFax + "|" + Result.sPhyNPI.replace("&nbsp;", "") + "|" + Result.sPhyFacility.replace("&nbsp;", "");
                    }

                    document.getElementById("btnSave").disabled = false;

                }
                childWindow.remove_close(FindReferralPhysicianClick);

            });
        }, 0);


    return false;
}
function EnableSaveButton(ctrl) {
    if (ctrl != undefined && ctrl.readOnly != true) {
        document.getElementById("btnSave").disabled = false;
        var dt = new Date();
        var now = new Date();

        var utc = (now.getUTCMonth() + 1) + '/' + ("0" + now.getUTCDate()).slice(-2) + '/' + now.getUTCFullYear();
        var minutes;
        var seconds;
        if (now.getUTCMinutes() < 10) {
            minutes = '0' + now.getUTCMinutes();
        }
        else {
            minutes = now.getUTCMinutes();
        }
        if (now.getUTCSeconds() < 10) {
            seconds = '0' + now.getUTCSeconds();
        }
        else {
            seconds = now.getUTCSeconds();

        }
        utc += ' ' + now.getUTCHours() + ':' + minutes + ':' + seconds;
        document.getElementById("hdnLocalTime").value = utc;
    }
}
function ConfirmPastAppointment() {
    if (window.confirm("Do you want to create an appointment in the past?") == true) { document.getElementById("btnConfirmAppointment").click(); }
    else { return false; }
}
function OpenFindAuth() {
    var PatientName = document.getElementById("txtPatientName").value;
    var PatientType = document.getElementById("txtHumanType").value;
    var DOB = document.getElementById("txtPatientDOB").value;
    var AccNO = document.getElementById("txtPatientAccountNumber").value;
    var EncounterID = document.getElementById("hdnEncounterID").value;
    var obj = new Array();
    obj.push("PatientName=" + PatientName);
    obj.push("parentscreen=EditAppointMents");
    obj.push("PatientDOB=" + DOB);
    obj.push("PatienType=" + PatientType);
    obj.push("AccNo=" + AccNO);
    obj.push("EncounterID=" + EncounterID);
    openModal("frmFindAuthorization.aspx", 620, 1320, obj, "ModalWindow");
    var WindowName = $find('ModalWindow');
    WindowName.add_close(ShowBlockDaysClick)
    {
        function ShowBlockDaysClick(oWindow, args) {
            var Result = args.get_argument();
            if (Result != null) {
                document.getElementById("txtAuthorizationNo").value = Result.AuthID;
            }

        }
    }
    return false;
}
function CloseWindow() {
    document.getElementById("hdnPhysicianID").value = document.getElementById("hdnEditApptPhyID").value;
    var result = new Object();
    result.PhysicianID = document.getElementById("hdnPhysicianID").value;
    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    returnToParent(result);
}
function CloseWindowForConfirmAppointment() {
    DisplayErrorMessage('110019');
    document.getElementById("hdnPhysicianID").value = document.getElementById("hdnEditApptPhyID").value;
    var result = new Object();
    result.PhysicianID = document.getElementById("hdnPhysicianID").value;
    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    returnToParent(result);
}
function Exit() {
    if (document.getElementById("btnSave").disabled == false) {
        if (DisplayErrorMessage('220211') == true) {
            document.getElementById("btnSave").click();
        }
        else {
            returnToParent(null);
        }
    }
    else {
        returnToParent(null);
    }
}
function Exits() {
    if (document.getElementById("btnSave").disabled == false) {
        if (document.getElementById("hdnMessageType").value == "") {
            DisplayErrorMessage('1104001');
            return false;
        }
        else if (document.getElementById("hdnMessageType").value == "Yes") {
            showTime();
            document.getElementById("btnSave").click();

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
        self.close();
    }
}
function Exits1() {
    if (document.getElementById("hdnConfirmBlockAppointment").value == "C") {
        ConfirmBlockAppointment();
        return;
    }
    DisplayErrorMessage('110019');
    document.getElementById("hdnMessageType").value = "";
    CloseWindow();
}

function ConfirmPreviousAppointment() {
    if (window.confirm("You are booking this patient for multiple encounters on the same day. Would you like to continue with this action?") == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        sessionStorage.setItem('btnConfirmClick', 'true');
        document.getElementById("btnConfirmAppointment").click();
    } else {
        if ($(".ui-dialog").is(":visible")) {
            $(dvdialog).dialog("close");
        }
        returnToParent(null);
    }
}
function ConfirmOverwriteAppointment() {
    if (window.confirm("The specified Time has an appointment already.Do you want to continue?") == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        sessionStorage.setItem('btnConfirmClick', 'true');
        document.getElementById("btnConfirmAppointment").click();
    } else {

        return false;
    }
}
function ConfirmBlockAppointment(str) {
    var sType = str;
    if (sType == undefined) {

        if (DisplayErrorMessage('110049')) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            sessionStorage.setItem('btnConfirmClick', 'true');
            document.getElementById("btnConfirmAppointment").click();

        }
        else {
            return false;
        }

    }
    else {

        if (window.confirm("The Specified Slot is blocked for " + sType + ".Do you want to continue?") == true) {
            document.getElementById("btnConfirmAppointment").click();
            DisplayErrorMessage('110019');
            CloseWindow();
        }
        else {
            return false;
        }

    }
}
function OpenDemographics() {

    {
        var Result = args.get_argument();
        if (Result) {
            var HumanId = Result.HumanId;
            if (HumanId != "0") {
                var obj = new Array();
                obj.push("HumanId=" + HumanId);
                obj.push("ScreenName=Demographics");
                var result = openModal("frmPatientDemographics.aspx", 1230, 1130, obj, "ctl00_ModalWindow");
                return false;


            }
        }
    }

}
function OpenPatientDemographics() {
    var EncounterID = document.getElementById("hdnEncounterID").value;
    var details = document.getElementById('divPatientstrip').innerHTML;
    var demographicdetails = details.split("|");
    var humanId = demographicdetails[4].split(":")[1].trim();

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + humanId + "&EncounterId=" + EncounterID, "MessageWindow");
            setRadWindowPropertiesDemographics(childWindow, 1230, 1130);

            childWindow.add_close(function PatientDemographicsClick(oWindow, args) {
                //var Result = args.get_argument();
                //document.getElementById("btnHumanDetailUpdate").click();
                setPAtientDetails(humanId);
                PcpPrimaryDefault(humanId);
                //  document.getElementById("txtProviderSearch").value = document.getElementById("hdnrenprovidersearch").value;

            });
        }, 0);

    return false;
}


function setPAtientDetails(humanid) {
    $.ajax({
        type: "POST",
        url: "frmEditAppointment.aspx/GetHumanDetails",
        data: JSON.stringify({
            "humanid": humanid,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {

            var objdata = $.parseJSON(data.d);
            document.getElementById('divPatientstrip').innerText = objdata;
        }
    });
}

function PcpPrimaryDefault(humanid) {
    var EncounterId = document.getElementById("hdnEncounterID").value;
    $.ajax({
        type: "POST",
        url: "frmEditAppointment.aspx/PcpPrimaryDefault",
        data: JSON.stringify({
            "humanid": humanid,
            "EncounterId": EncounterId,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {

            var objdata = $.parseJSON(data.d);
            //Cap - 1234
            var TabVal = document.getElementById("hdnTabRefPcpChange").value
            if (TabVal != undefined && TabVal != null && TabVal == "PCP") {
                document.getElementById("txtProviderSearch").value = objdata.split("&")[1] ?? "";
            }
            document.getElementById("hdnpcpprovider").value = objdata.split("&")[1] ?? "";
            document.getElementById("hdnpcpprovidersearch").value = objdata.split("&")[1] ?? "";
            document.getElementById("hdnpcpEditPhyId").value = objdata.split("&")[0] ?? "";
        }
    });
}


function OpenPatientTask() {

    var patientsex = document.getElementById("hdnPatientSex").value;
    var status = document.getElementById("hdnPatientStatus").value;
    var Parentscreen = document.getElementById("hdnParentscreen").value;
    var EncounterId = document.getElementById("hdnEncounterID").value;


    var str = document.getElementById('divPatientstrip').innerHTML;
    var patientdetails = str.split("|");
    var humanid = patientdetails[4].split(":")[1].trim();

    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmViewPatientTask.aspx?patientdob=" + patientdetails[1].trim() + "&AccountNum=" + humanid + "&gender=" + patientdetails[3].trim() + "&status=" + status + "&patientname=" + patientdetails[0].trim() + "&patientType=" + patientdetails[7].split(":")[1].trim() + "&ParentScreen=" + Parentscreen + "&EncounterID=" + EncounterId, "MessageWindow");

            setRadWindowProperties(childWindow, 940, 1070);


        }, 0);

    return false;

}
function LibraryClick() {
    var PhysicianID = document.getElementById("hdnPhysicianID").value;
    var FieldName = "PURPOSE OF VISIT";
    window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + FieldName + "&PhyID=" + PhysicianID, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:645px;scroll:yes;");

}
function librarynotes() {
    var PhysicianID = document.getElementById("hdnPhysicianID").value;
    var FieldName = "APPOINTMENT NOTES";
    window.showModalDialog("frmAddorUpdateKeywords.aspx?FieldName=" + FieldName + "&PhyID=" + PhysicianID, null, "center:yes;resizable:yes;dialogHeight:445px;dialogWidth:645px;scroll:yes;");
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    document.getElementById("btnSave").disabled = false;
    return true;
}
function DateValidattion(dateToValidate) {
    //CAP-1463
    var datePicker = $find(dateToValidate);
    var splitdate = datePicker?.get_dateInput()?.get_selectedDate()?.format("dd-MMM-yyyy");
    var dt1 = new Date();
    var dd = new Date();
    var month = new Array();
    switch (splitdate?.split('-')[1]) {
        case "Jan":
            x = 0;
            break;
        case "Feb":
            x = 1;
            break;
        case "Mar":
            x = 2;
            break;
        case "Apr":
            x = 3;
            break;
        case "May":
            x = 4;
            break;
        case "Jun":
            x = 5;
            break;
        case "Jul":
            x = 6;
            break;
        case "Aug":
            x = 7;
            break;
        case "Sep":
            x = 8;
            break;
        case "Oct":
            x = 9;
            break;
        case "Nov":
            x = 10;
            break;
        case "Dec":
            x = 11;
            break;
        case splitdate.split('-')[1]:
            return false;
            break;

    }


    dd.setFullYear(splitdate?.split('-')[2], x, splitdate?.split('-')[0]);
    if (isNaN(dd)) {
        return false;
    }
    if (splitdate?.split('-')[0] > 31) {
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
function TimeValidation(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
}

function OpenAuthorization(str) {
    if (str == undefined) {
        if (confirm("“One or More Auth Available for this patient") == true) {
            callAuth();
        }
        else {
            return false;
        }
    }
    else {
        if (confirm("“More than one Auth Available for this patient") == true) {
            callAuth();
        }
        else {
            return false;
        }
    }
}
function callAuth() {
    var PatientName = document.getElementById("txtPatientName").value;
    var PatientType = document.getElementById("txtHumanType").value;
    var DOB = document.getElementById("txtPatientDOB").value;
    var AccNO = document.getElementById("txtPatientAccountNumber").value;
    var EncounterID = document.getElementById("hdnEncounterID").value;
    var obj = new Array();
    obj.push("PatientName=" + PatientName);
    obj.push("parentscreen=EditAppointMents");
    obj.push("PatientDOB=" + DOB);
    obj.push("PatienType=" + PatientType);
    obj.push("AccNo=" + AccNO);
    obj.push("EncounterID=" + EncounterID);
    openModal("frmFindAuthorization.aspx", 720, 1120, obj, "ModalWindow");

    var WindowName = $find('ModalWindow');
    WindowName.add_close(FindAuthorizationClick)
    {
        function FindAuthorizationClick(oWindow, args) {
            var Result = args.get_argument();
            if (Result != null) {
                document.getElementById("txtAuthorizationNo").value = Result.AuthNo;
                document.getElementById("hdnAuthSelectId").value = Result.AuthID;
            }
        }
    }
}

function CancelAppointment() {
    DisplayErrorMessage('110079');
    returnToParent(null);
}

function PhoneEncounterCancel() {
    //CAP-892 From Patient communication screen when cancelled a Phone encounter, shows Multiple tab restrict access error but the encounter gets cancelled
    DisplayErrorMessage('110094');
    window.top.location.href = "frmPatientChart.aspx?allowmultipletab=true";// returnToParent(null);
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
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
        self.close();
    }
}
function ShowLoading() {
    document.getElementById('divLoading').style.display = "block";
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
function AppointmentPastDateValidation(dateToValidate) {
    var splitdate = $find(dateToValidate)._dateInput._text;
    var dt1 = new Date();
    var dd = new Date();
    var month = new Array();
    switch (splitdate.split('-')[1]) {
        case "Jan":
            x = 0;
            break;
        case "Feb":
            x = 1;
            break;
        case "Mar":
            x = 2;
            break;
        case "Apr":
            x = 3;
            break;
        case "May":
            x = 4;
            break;
        case "Jun":
            x = 5;
            break;
        case "Jul":
            x = 6;
            break;
        case "Aug":
            x = 7;
            break;
        case "Sep":
            x = 8;
            break;
        case "Oct":
            x = 9;
            break;
        case "Nov":
            x = 10;
            break;
        case "Dec":
            x = 11;
            break;
        case splitdate.split('-')[1]:
            return false;
            break;

    }


    dd.setFullYear(splitdate.split('-')[2], x, splitdate.split('-')[0]);
    if (isNaN(dd)) {
        return false;
    }
    if (parseInt(splitdate.split('-')[0]) > 31) {
        return false;
    }
    if ((dd.getFullYear() < dt1.getFullYear())) {
        return false;
    } else if (dd.getMonth() < dt1.getMonth() && (dd.getFullYear() <= dt1.getFullYear())) {
        return false;
    } else if (dd.getDate() < dt1.getDate() && (dd.getMonth() <= dt1.getMonth()) && (dd.getFullYear() <= dt1.getFullYear())) {
        return false;
    } else {
        return true;
    }
}


function CloseCancelAppmntWindow() {
    var result = new Object();
    result.Close = "Y";
    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    if (document.getElementById('hdnPhoneEncounter').value != "Y")
        returnToParent(result);
    else {
        self.close();//returnToParent(null);
        return false;
    }
}

function setRadWindowPropertiesDemographics(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();
}

function ShowLoading() {
    document.getElementById('divLoading').style.display = "block";
}
function OpenFindAvailableSlot() {
    var FacilityName = $find("cboFacility");
    var AppointmentDate = $find("dtpApptDate");
    var AppointmentTime = $find("dtpStartTime");
    var SlotLength = document.getElementById(GetClientId('ddlDuration')).value;
    var PhysicianID = document.getElementById(GetClientId('hdnPhysicianID')).value;
    var obj = new Array();
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var childWindow = oWnd.BrowserWindow.radopen("frmFindAvailableAppointments.aspx?FacilityName=" + FacilityName.get_selectedItem().get_text() + "&AppointmentDate=" + AppointmentDate.get_dateInput()._text + "&AppointmentTime=" + AppointmentTime._timeView.getTime().format("HH:mm tt") + "&SlotLength=" + SlotLength + "&PhysicianID=" + PhysicianID, "MessageWindow");
            setRadWindowProperties(childWindow, 820, 1180);
            childWindow.add_close(function FindAvailableSlotClick(oWindow, args) {
                var Result = args.get_argument();
                if (Result != "undefined" && Result != null && Result.Screen != "GoTOSlot") {
                    document.getElementById("hdnApptDate").value = Result.Appdate;
                    document.getElementById("hdnApptTime").value = Result.Apptime;
                    document.getElementById("hdnProviderName").value = Result.Provider;
                    document.getElementById("hdnFacilityName").value = Result.facility;
                    document.getElementById(GetClientId("btnAppointmentSlot")).click();

                }
                else {
                    if (Result != "undefined" && Result != null) {
                        var result = new Object();
                        result.Selecteddate = Result.Appdate;
                        result.PhyID = Result.PhyID;
                        result.Facility = Result.facility;
                        result.Provider = Result.Provider;
                    }
                    if (window.opener) {
                        window.opener.returnValue = result;
                    }
                    window.returnValue = result;
                    returnToParent(result);
                    return true;
                }
            });

        }, 0);


}

function singleclick(chk) {
    var chkList = chk.parentNode.parentNode.parentNode;
    var chks = chkList.getElementsByTagName("input");

    for (var i = 0; i < chks.length; i++) {

        if (chks[i] != chk && chk.checked) {

            chks[i].checked = false;

        }

    }
}

function SlotValueSelected() {
    var grdAppointmentList = $find('grdAppointmentList');
    var dtpApptDate = $find('dtpApptDate');
    var index = parseInt(document.getElementById("hdnSelectedIndex").value);
    var MasterTable = grdAppointmentList.get_masterTableView();
    row = MasterTable.get_dataItems()[index];
    var result = new Object();
    result.Appdate = MasterTable.getCellByColumnUniqueName(row, "AppointmentDate").innerHTML;
    result.Apptime = MasterTable.getCellByColumnUniqueName(row, "Time").innerHTML;
    result.facility = MasterTable.getCellByColumnUniqueName(row, "Facility").innerHTML;
    result.Provider = MasterTable.getCellByColumnUniqueName(row, "Provider").innerHTML;
    result.Screen = "";

    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    returnToParent(result);
    return false;
}
function WillingCancelClose() {
    var result = new Object();
    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    returnToParent(result)
}

function GoToSlot() {
    var grdAppointmentList = $find('grdAppointmentList');
    var index = parseInt(document.getElementById("hdnSelectedIndex").value);
    var MasterTable = grdAppointmentList.get_masterTableView();
    row = MasterTable.get_dataItems()[index];
    var result = new Object();
    result.Appdate = MasterTable.getCellByColumnUniqueName(row, "AppointmentDate").innerHTML;
    result.Apptime = MasterTable.getCellByColumnUniqueName(row, "Time").innerHTML;
    result.facility = MasterTable.getCellByColumnUniqueName(row, "Facility").innerHTML;
    result.Provider = MasterTable.getCellByColumnUniqueName(row, "Provider").innerHTML;
    result.Screen = "GoTOSlot";
    result.PhyID = document.getElementById("hdnPhysicianID").value;
    if (window.opener) {
        window.opener.returnValue = result;
    }
    window.returnValue = result;
    returnToParent(result);
    return false;
}


function EnableDLC(ctrl) {
    var Control = document.getElementById(GetClientId("txtPurposeofVisit"));
    if (ctrl.checked) {
        document.getElementById(GetClientId("DLC_txtDLC")).disabled = false;
        document.getElementById(GetClientId("DLC_txtDLC")).value = "";
        document.getElementById(GetClientId("DLC_pbDropdown")).disabled = false;
        document.getElementById(GetClientId("DLC_pbClear")).disabled = false;
    }
    else {

        document.getElementById(GetClientId("DLC_txtDLC")).disabled = true;
        document.getElementById(GetClientId("DLC_pbDropdown")).disabled = true;
        document.getElementById(GetClientId("DLC_pbClear")).disabled = true;
    }
    EnableSaveButton(ctrl);
}




function btnClose_Clicked() {
    if (document.getElementById("btnSave").disabled == false) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (window.GetRadWindow() != null) {
            var winName = window.GetRadWindow()._name;
            if ($(top.window.document).find("iframe[name='" + winName + "']")[0] != undefined) {
                if (!$($(top.window.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').is('#dvdialogMenu'))
                    $($(top.window.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                        '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
                dvdialog = $($(top.window.document).find("iframe[name='" + winName + "']")[0].contentDocument).find('body').find('#dvdialogMenu');
            }
            //Jira CAP-2895
            if ($(top.window.document)?.find("iframe[name='ModalWindow']")[0]?.contentWindow?.document != undefined
                && $($(top.window.document).find("iframe[name='ModalWindow']")[0].contentWindow.document)?.find("iframe[name='ModalWindowMngt']") != undefined
                && $($(top.window.document).find("iframe[name='ModalWindow']")[0].contentWindow.document).find("iframe[name='ModalWindowMngt']")[0]?.contentWindow != undefined)
            {
                $($($(top.window.document).find("iframe[name='ModalWindow']")[0].contentWindow.document).find("iframe[name='ModalWindowMngt']")[0]?.contentWindow.document).find("body").append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                    '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
                dvdialog = $($($(top.window.document).find("iframe[name='ModalWindow']")[0].contentWindow.document).find("iframe[name='ModalWindowMngt']")[0]?.contentWindow.document).find("body").find('#dvdialogMenu');
            }
            //Jira CAP-2895 - End
            else {
                //Jira #CAP-773 - Check undefind and null to the $(top.window.document).find("iframe")[0]
                if ($(top.window.document).find("iframe") != undefined && $(top.window.document).find("iframe") != null && $(top.window.document).find("iframe")[0] != undefined && $(top.window.document).find("iframe")[0] != null) {
                    if (!$($(top.window.document).find("iframe")[0].contentDocument).find('body').is('#dvdialogMenu'))
                        $($(top.window.document).find("iframe")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                            '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
                    dvdialog = $($(top.window.document).find("iframe")[0].contentDocument).find('body').find('#dvdialogMenu');
                }
            }
        }
        else {
            //Jira #CAP-773 - Check undefind and null to the $(top.window.document).find("iframe")[0]
            if ($(top.window.document).find("iframe") != undefined && $(top.window.document).find("iframe") != null && $(top.window.document).find("iframe")[0] != undefined && $(top.window.document).find("iframe")[0] != null) {
                if (!$($(top.window.document).find("iframe")[0].contentDocument).find('body').is('#dvdialogMenu'))
                    $($(top.window.document).find("iframe")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                        '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
                dvdialog = $($(top.window.document).find("iframe")[0].contentDocument).find('body').find('#dvdialogMenu');
            }
        }
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

                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    $(dvdialog).dialog("close");
                    sessionStorage.setItem("AutoSave_OrderMenu", "true");


                    $('#btnSave').trigger('click');

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
        returnToParent(null);
    }
}

function EnableSaveButtononunload() {
    $("#btnSave").removeAttr("disabled");
}
function tabReferringProvAndPCP_TabSelected(sender, args) {
    if (document.getElementById("chkSelfReferred") != null) {

        $('#imgClearProviderText').css("top", "224px !important");
        if (document.getElementById("chkSelfReferred").checked) {
        }
        else {
        }
    }
    else {
        $('#imgClearProviderText').css("top", "215px !important");
    }
    if (document.getElementById("btnSave") != undefined && document.getElementById("btnSave") != null)
        if (document.getElementById("btnSave").disabled == false)
            document.getElementById("hdnbtnsave").value = false;
        else
            document.getElementById("hdnbtnsave").value = true;
}
function EditAppointmentLoad() {
    if (sessionStorage.getItem('btnConfirmClick') == null) { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    else
        sessionStorage.removeItem('btnConfirmClick')


    var stoptransfer = new Date();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

    console.log(document.getElementById("hdnTimeTaken").value);//+ "UI Script Execution Time : " + ui_time + "s;");
    $("textarea[id *= txtDLC]").removeClass('DlcClass');

    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}
function ddlVisitType_onChange(sender, eventArgs) {
    EnableSaveButton(sender._element);
}
function OpenImageAndLaborder() {
    //var details = document.getElementById('divPatientstrip').innerHTML;
    //var demographicdetails = details.split("|");
    //var AccNO = demographicdetails[4].split(":")[1].trim();


    //var EncounterID = document.getElementById("hdnEncounterID").value;
    //var obj = new Array();
    //obj.push("HumanID=" + AccNO);
    //obj.push("EncounterID=" + EncounterID);
    //obj.push("PhysicianID=" + "0");
    //obj.push("FromAppointmnet=" + "Y");
    //var oBrowserWnd = GetRadWindow().BrowserWindow;
    //var childWindow = oBrowserWnd.radopen("frmImageAndLabOrder.aspx?HumanID=" + AccNO + "&EncounterID=" + EncounterID + "&PhysicianID=" + "0" + "&FromAppointmnet="+"Y", "MessageWindow");
    //setTimeout(   
    //    function()
    //    {

    //        childWindow.SetModal(true);
    //        childWindow.remove_close(OnClientCloseFindOrder);
    //        childWindow.set_visibleStatusbar(false);
    //        childWindow.setSize(1220, 700);
    //        childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close|Telerik.Web.UI.WindowBehaviors.Move);
    //        childWindow.set_iconUrl("Resources/16_16.ico"); 
    //        childWindow.set_keepInScreenBounds(true);
    //        childWindow.set_centerIfModal(true);
    //        childWindow.set_reloadOnShow(true);
    //        childWindow.center();
    //        childWindow.add_close(OnClientCloseFindOrder);
    //    },0);
    //return false;
    setTimeout(function () {
        var details = document.getElementById('divPatientstrip').innerHTML;
        var demographicdetails = details.split("|");
        var AccNO = demographicdetails[4].split(":")[1].trim();


        var EncounterID = document.getElementById("hdnEncounterID").value;
        var obj = new Array();
        obj.push("HumanID=" + AccNO);
        obj.push("EncounterID=" + EncounterID);
        obj.push("PhysicianID=" + "0");
        obj.push("FromAppointmnet=" + "Y");
        var oWnd = GetRadWindow();
        var childWindow = oWnd.BrowserWindow.radopen("frmImageAndLabOrder.aspx?HumanID=" + AccNO + "&EncounterID=" + EncounterID + "&PhysicianID=" + "0" + "&FromAppointmnet=" + "Y" + "&FromAppointmentFacility=" + encodeURIComponent(document.getElementById("hdnFacilityName").value), "MessageWindow");
        setRadWindowProperties(childWindow, 700, 1220);
        childWindow.remove_close(OnClientCloseFindOrder);
        childWindow.add_close(OnClientCloseFindOrder);
    }, 0);
    return false;
}
function OnClientCloseFindOrder(oWindow, args) {
    var arg = args.get_argument();
    if (arg != undefined) {
        document.getElementById("hdnOrderSubmitdata").value = arg.Orders;
        document.getElementById("btnOrderCreate").click();
    }
}

function cboOrderClientSelectedIndexChange() {
    $("#orderCombo").click();

}
function enablecontrol() {
    EnableSaveButton(this); { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).keyup(function (e) {
        if (e.which === 38 || e.which == 40) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
}
//View Patient Task Mandatory
function patienttaskload() {
    $("span[mand=Yes]").addClass('MandLabelstyle');

    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });

}


//Cap - 1989
function EditProviderDetails() {
    var EditPhyId;
    if (document.getElementById("chkSelfReferred") != null && document.getElementById("chkSelfReferred").checked == false) {
        EditPhyId = document.getElementById('hdnRefEditPhyId').value
    }
    else {
        EditPhyId = document.getElementById('hdnpcpEditPhyId').value;
    }
    
    var ProviderText = document.getElementById("txtProviderSearch").value;

    var Category = document.getElementById("hdnCategory").value;

    if (EditPhyId == '' || ProviderText == '') {
        DisplayErrorMessage('380060');
    }
    else if (Category != "NON CAPELLA USER(Physician)" && Category != "NON CAPELLA USER" && Category != "ORGANIZATION") {
        DisplayErrorMessage('380061', '', Category);
    }
    else {
        localStorage.setItem("PhyDetails", ProviderText);
        $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
        $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
        $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
        $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px";
        var sPath = "frmPhysicianLibray.aspx?EditPhyId=" + EditPhyId;
        $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
        $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
        $(top.window.document).find("#TabPhysicianLibrary").modal("show");
        $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
            var PhyTextboxName = localStorage.getItem("PhyDetails");
            if (PhyTextboxName.split("&")[4] != undefined) {
                //Cap - 2134
                document.getElementById("txtProviderSearch").value = PhyTextboxName.split("&")[4].replace("~|", "&");

                if (document.getElementById("chkSelfReferred") != null && document.getElementById("chkSelfReferred").checked == false) {
                    //Cap - 2134
                    document.getElementById("hdnrenprovider").value = PhyTextboxName.split("&")[4].replace("~|", "&");
                    document.getElementById("hdnrenprovidersearch").value = PhyTextboxName.split("&")[4].replace("~|", "&");
                    document.getElementById("hdnRefEditPhyId").value = EditPhyId; 
                }
                else {
                    //Cap - 2134
                    document.getElementById("hdnpcpprovider").value = PhyTextboxName.split("&")[4].replace("~|", "&");
                    document.getElementById("hdnpcpprovidersearch").value = PhyTextboxName.split("&")[4].replace("~|", "&");
                    document.getElementById("hdnpcpEditPhyId").value = EditPhyId; 
                }

            }
            else {
                //Cap - 2134
                document.getElementById("txtProviderSearch").value = PhyTextboxName.replace("~|", "&");
                if (document.getElementById("chkSelfReferred") != null && document.getElementById("chkSelfReferred").checked == false) {
                    //Cap - 2134
                    document.getElementById("hdnrenprovider").value = PhyTextboxName.replace("~|", "&");
                    document.getElementById("hdnrenprovidersearch").value = PhyTextboxName.replace("~|", "&");
                    document.getElementById("hdnRefEditPhyId").value = EditPhyId;
                }
                else {
                    //Cap - 2134
                    document.getElementById("hdnpcpprovider").value = PhyTextboxName.replace("~|", "&");
                    document.getElementById("hdnpcpprovidersearch").value = PhyTextboxName.replace("~|", "&");
                    document.getElementById("hdnpcpEditPhyId").value = EditPhyId; 
                }
            }


        });

        return false;
    }


}

//Jira CAP-2216
function EnableProviderSearch(EnablePrvdr)
{
    if (EnablePrvdr == 'false') {
        document.getElementById("txtProviderSearch").disabled = true;
        if (document.getElementById("imgClearProviderText")?.style?.visibility != undefined && document.getElementById("imgClearProviderText")?.style?.visibility != null) {
            document.getElementById("imgClearProviderText").style.visibility = false;
        }
        if (document.getElementById("imgEditProvider") != undefined && document.getElementById("imgEditProvider") != null) {
            document.getElementById("imgEditProvider").style.visibility = false;
            document.getElementById("imgEditProvider").style.display = "none";
        }
    }
    else if (EnablePrvdr == 'true'){
        if (document.getElementById("txtProviderSearch").value == "") {
            document.getElementById("txtProviderSearch").disabled = false;
            //Jira CAP-2348
            if (document.getElementById("imgEditProvider")?.style?.display != null && document.getElementById("imgEditProvider")?.style?.display != undefined) {
                document.getElementById("imgEditProvider").style.display = "block";
            }
        }
        else {
            document.getElementById("txtProviderSearch").disabled = true;
            //Jira CAP-2348
            if (document.getElementById("imgEditProvider")?.style?.display != null && document.getElementById("imgEditProvider")?.style?.display != undefined) {
                document.getElementById("imgEditProvider").style.display = "none";
            }
        }
        if (document.getElementById("imgClearProviderText")?.style?.visibility != undefined && document.getElementById("imgClearProviderText")?.style?.visibility != null) {
            document.getElementById("imgClearProviderText").style.visibility = true;
        }
        //if (document.getElementById("imgEditProvider") != undefined && document.getElementById("imgEditProvider") != null) {
        //    document.getElementById("imgEditProvider").style.visibility = true;
        //    document.getElementById("imgEditProvider").style.display = "none";
        //}
    }

    if (document.getElementById("hdnDisableSelfReferred") != undefined
        && document.getElementById("hdnDisableSelfReferred") != null
        && document.getElementById("hdnDisableSelfReferred").value != "") {

        if (document.getElementById("hdnDisableSelfReferred").value == 'true') {
            if (document.getElementById("chkSelfReferred")?.disabled != undefined) {
                document.getElementById("chkSelfReferred").disabled = true;
            }
        }
        else {
            if (document.getElementById("chkSelfReferred")?.disabled != undefined) {
                document.getElementById("chkSelfReferred").disabled = false;
            }
            
        }
    }

    if (document.getElementById("chkSelfReferred")?.disabled != undefined && document.getElementById("chkSelfReferred")?.checked == true) {
        document.getElementById("txtProviderSearch").disabled = true;
    }
}