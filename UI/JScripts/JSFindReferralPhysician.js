function OpenAddPhysician() {
    localStorage.removeItem("IsEFax");
    localStorage.setItem("IsEnableGrid", "false");
    $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Provider Library";
    $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.width = "850px";
    $(top.window.document).find("#TabmdldlgPhysicianLibrary")[0].style.height = "440px"; //"715px";
    var sPath = "frmPhysicianLibray.aspx";
    $(top.window.document).find("#TabPhysicianLibraryFrame")[0].style.height = "275px"; //"495px";
    $(top.window.document).find("#TabPhysicianLibraryFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabPhysicianLibrary").modal("show");
    $(top.window.document).find("#TabPhysicianLibrary").one("hidden.bs.modal", function (e) {
    });
    return false;
}
function CloseAddPhysician(event) {
    if (document.getElementById("hdnAddPhysician").value == "Yes") {
        closeAddPhyscianAfterSave();
    }
    else if (document.getElementById("hdnAddPhysician").value == "") {
        self.close();
    }
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
function closeAddPhyscianAfterSave(oWindow, args) {
    if (document.getElementById("hdnAddPhysician").value == "Yes") {
        DisplayErrorMessage('110019');
        var Result = new Object();
        Result.ulPhyId = document.getElementById("hdnPhyID").value;
        Result.sPhyName = document.getElementById("txtLastName").value + ' ' + document.getElementById("txtFirstName").value;
        Result.sPhyNPI = document.getElementById("txtNPI").value;
        Result.sPhySpecialty = document.getElementById("ddlSpecialty").value//sPhySpecialty;
        Result.sPhyFacility = document.getElementById("ddlFacility").value;
        Result.sPhyAddress = document.getElementById("txtAddress").value;
        Result.sPhyFax = document.getElementById("msktxtFaxNumber").value;
        Result.sPhyPhone = document.getElementById("msktxtPhoneNum").value;
        if (window.opener) {
            window.opener.returnValue = Result;
        }
        window.returnValue = Result;
        returnToParent(Result);
    }
    else {
        var Result = args.get_argument();
        if (Result != null)
            returnToParent(Result);
    }
    var winAddPatient = GetRadWindow();
    var width = 860;
    var height = 150;
    winAddPatient.setSize(width, height);
}

function validationInformation() {
    if (document.getElementById(GetClientId("msktxtPhoneNum")).value.length != 0 && PhNoValid(GetClientId("msktxtPhoneNum")) == false && document.getElementById(GetClientId("msktxtPhoneNum")).value != "(___) ___-____") {

        DisplayErrorMessage('420005');
        document.getElementById(GetClientId("msktxtPhoneNum")).focus();
        return false;
    }

    else if (document.getElementById(GetClientId("msktxtFaxNumber")).value.length != 0 && PhNoValid(GetClientId("msktxtFaxNumber")) == false && document.getElementById(GetClientId("msktxtFaxNumber")).value != "(___) ___-____") {

        DisplayErrorMessage('420013');
        document.getElementById(GetClientId("msktxtFaxNumber")).focus();
        return false;
    }   
}
function PhNoValid(sphno) {
    var s = document.getElementById(sphno).value;
    sReplace = s.replace(/_/gi, "");
    if (sReplace.length < 14) {
        return false;
    }
    else {
        return true;
    }



}

function ProviderSelected(event, ui) {
    //Cap - 1989
    if (document.getElementById("txtProviderSearch").value != "" && document.getElementById("txtProviderSearch").value != "| NPI: | Facility: | Address:| Phone No:| Fax No:") {
        document.getElementById("txtProviderSearch").disabled = true;
    }
    else {
        document.getElementById("txtProviderSearch").disabled = false;//have to change
        document.getElementById("txtProviderSearch").value = "";
    }

    var ProviderDetails = JSON.parse(ui.item.val);
    var txtProviderSearch = document.getElementById("txtProviderSearch");
    //Cap - 1989
    document.getElementById("hdnCategory").value = ProviderDetails.sCategory;
    document.getElementById('hdnEditPhysicianId').value = ProviderDetails.ulPhyId;
    txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
    txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(ProviderDetails);
    document.getElementById('btnOk').disabled = false;
    txtProviderSearch.value = ui.item.label;
    return false;
}

function OnLoadPhysician() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}


var intProviderlen = -1;
var arrProvider = [];
$(document).ready(function () {
    var IsMenuLevel = "";
    if (document.URL.indexOf("IsMenuLevel") > -1) {
        IsMenuLevel = document.URL.slice(document.URL.indexOf("IsMenuLevel"), document.URL.length).split("&")[0].split("=")[1]
    }
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
    $("#imgClearProviderText").css({
        "position": "absolute",
        "right": "22px",
        "top": "41px",
        "cursor": "pointer",
        "width": "10px",
        "height": "10px"
    }).on("click", function () {
        $('#txtProviderSearch').val('').focus();
        intProviderlen = -1;
        arrProvider = [];
        $(".ui-autocomplete").hide();
        txtProviderSearch.attributes['data-phy-id'].value = '';
        txtProviderSearch.attributes['data-phy-details'].value = '';
        document.getElementById('btnOk').disabled = true;
        document.getElementById("txtProviderSearch").disabled = false;
    });
    $("#txtProviderSearch").autocomplete({
        source: function (request, response) {
            if ($("#txtProviderSearch").val().trim().length > 2) {
                if (intProviderlen == 0) {

                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
                    this.element.on("keydown", PreventTyping);
                    arrProvider = [];
                    var strkeyWords = $("#txtProviderSearch").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var WSData = {
                        text_searched: strkeyWords[0],
                        IsMenulevel: IsMenuLevel,
                    };
                    document.getElementById('btnAddPhysician').disabled = false;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "./frmFindReferralPhysician.aspx/GetProviderDetailsByTokens",
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
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
                             {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
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
        if (item.label != "No matches found.")
            return $("<li>")
              .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
              .append(item.label)
              .appendTo(ul);
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
   
});

//Cap - 1989
function EditProviderDetails() {
    var EditPhyId = document.getElementById("hdnEditPhysicianId").value;
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
                var querystring;
                document.getElementById("txtProviderSearch").value = PhyTextboxName.split("&")[4];
                if (EditPhyId != null && EditPhyId != undefined) { 
                    let dataval = EditPhyId.toString();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "./frmFindReferralPhysician.aspx/GetProviderDetailsByPhyId",
                        data: JSON.stringify({ 'vPhyId': dataval }),
                        dataType: "json",
                        success: function (data) {
                           
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

                                var  results = jsonData.Matching_Result;

                                txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(results[0].value);

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
            }
            else {
                document.getElementById("txtProviderSearch").value = PhyTextboxName;
                
            }
        });

        return false;
    }


}


