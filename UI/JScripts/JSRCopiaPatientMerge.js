var intPatientlen = -1;
var arrPatient = [];

var intPatientlenMerge = -1;
var arrPatientMerge = [];

$(document).ready(function () {
    //for select keep account
    $("#imgClearKeepAccount").on("click", function () {
        $('#txtKeepAccount').val('').focus();
        $('#txtKeepAccount').attr('data-human-id', '0');
        $('#txtKeepAccount').attr('data-human-details', '');
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
        $('#btnDownload').prop('disabled', true);
    });

    $("#txtKeepAccount").autocomplete({
        source: function (request, response) {
            if ($("#txtKeepAccount").val().trim().length > 2) {
                if (intPatientlen == 0) {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    this.element.on("keydown", PreventTyping);
                    arrPatient = [];
                    var strkeyWords = $("#txtKeepAccount").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var account_status = "ACTIVE";
                    var patient_status = "ALIVE";
                    var patient_type = "ALL";
                    var WSData = {
                        text_searched: strkeyWords[0],
                        account_status: account_status,
                        patient_status: patient_status,
                        human_type: patient_type
                    };

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "./frmFindPatient.aspx/GetPatientDetailsByTokens",
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            $("#txtKeepAccount").off("keydown", PreventTyping);
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

                                arrPatient = jsonData.Matching_Result;
                                response($.map(results, function (item) {
                                    return {
                                        label: item.label,
                                        val: JSON.stringify(item.value),
                                        value: item.value.HumanId
                                    }
                                }));
                            }
                        },
                        error: function OnError(xhr) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            if (xhr.status == 999)
                                window.location = "/frmSessionExpired.aspx";
                            else {
                                if (isValidJSON(xhr.responseText)) {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                        "Message: " + log.Message);
                                }
                                else {
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry.");
                                }
                            }
                        }

                    });
                }
                else if (intPatientlen != -1) {
                    var results = Filter(arrPatient, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item.label,
                            val: JSON.stringify(item.value),
                            value: item.value.HumanId
                        }
                    }));
                }
            }
        },
        minlength: 0,
        multiple: true,
        mustMatch: false,
        select: KeepPatientSelected,
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width("890px");
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $('#txtKeepAccount').focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#txtKeepAccount").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        if ($("#txtKeepAccount").val().charAt(e.currentTarget.value.length - 1) == " ") {
            if (e.currentTarget.value.split(" ").length > 2)
                intPatientlen = intPatientlen + 1;
            else
                intPatientlen = 0;
        }
        else {
            if ($("#txtKeepAccount").val().length != 0 && intPatientlen != -1) {
                intPatientlen = intPatientlen + 1;
            }
            if ($("#txtKeepAccount").val().length == 0 || $("#txtKeepAccount").val().indexOf(" ") == -1) {
                intPatientlen = -1;
                arrPatient = [];
                $(".ui-autocomplete").hide();
            }
        }
    })

    $("#txtKeepAccount").data("ui-autocomplete")._renderItem = function (ul, item) {
        if (item.label != "No matches found.") {
            var HumanDetails = $.parseJSON(item.val);
            var list_item = $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .append(item.label)
                .appendTo(ul);
            if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                list_item.addClass("inactive");
            if (HumanDetails.Status.toUpperCase() == "DECEASED")
                list_item.addClass("deceased");
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

    //for select merge account
    $("#imgCleartxtMergeAccount").on("click", function () {
        $('#txtMergeAccount').val('').focus();
        $('#txtMergeAccount').attr('data-human-id', '0');
        $('#txtMergeAccount').attr('data-human-details', '');
        intPatientlenMerge = -1;
        arrPatientMerge = [];
        $(".ui-autocomplete").hide();
        $('#btnDownload').prop('disabled', true);
    });

    $("#txtMergeAccount").autocomplete({
        source: function (request, response) {
            if ($("#txtMergeAccount").val().trim().length > 2) {
                if (intPatientlenMerge == 0) {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    this.element.on("keydown", PreventTyping);
                    arrPatientMerge = [];
                    var strkeyWords = $("#txtMergeAccount").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var account_status = "ACTIVE";
                    var patient_status = "ALIVE";
                    var patient_type = "ALL";
                    var WSData = {
                        text_searched: strkeyWords[0],
                        account_status: account_status,
                        patient_status: patient_status,
                        human_type: patient_type
                    };

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "./frmFindPatient.aspx/GetPatientDetailsByTokens",
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            $("#txtMergeAccount").off("keydown", PreventTyping);
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

                                arrPatientMerge = jsonData.Matching_Result;
                                response($.map(results, function (item) {
                                    return {
                                        label: item.label,
                                        val: JSON.stringify(item.value),
                                        value: item.value.HumanId
                                    }
                                }));
                            }
                        },
                        error: function OnError(xhr) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            if (xhr.status == 999)
                                window.location = "/frmSessionExpired.aspx";
                            else {
                                if (isValidJSON(xhr.responseText)) {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                                        "Message: " + log.Message);
                                }
                                else {
                                    alert("USER MESSAGE:\n" +
                                        ". Cannot process request. Please Login again and retry.");
                                }
                            }
                        }

                    });
                }
                else if (intPatientlenMerge != -1) {
                    var results = Filter(arrPatientMerge, request.term);
                    response($.map(results, function (item) {
                        return {
                            label: item.label,
                            val: JSON.stringify(item.value),
                            value: item.value.HumanId
                        }
                    }));
                }
            }
        },
        minlength: 0,
        multiple: true,
        mustMatch: false,
        select: MergePatientSelected,
        open: function () {
            $('.ui-autocomplete.ui-menu.ui-widget').width("880px");
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $('#txtMergeAccount').focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intPatientlenMerge = -1;
        arrPatientMerge = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#txtMergeAccount").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        if ($("#txtMergeAccount").val().charAt(e.currentTarget.value.length - 1) == " ") {
            if (e.currentTarget.value.split(" ").length > 2)
                intPatientlenMerge = intPatientlenMerge + 1;
            else
                intPatientlenMerge = 0;
        }
        else {
            if ($("#txtMergeAccount").val().length != 0 && intPatientlenMerge != -1) {
                intPatientlenMerge = intPatientlenMerge + 1;
            }
            if ($("#txtMergeAccount").val().length == 0 || $("#txtMergeAccount").val().indexOf(" ") == -1) {
                intPatientlenMerge = -1;
                arrPatientMerge = [];
                $(".ui-autocomplete").hide();
            }
        }
    })

    $("#txtMergeAccount").data("ui-autocomplete")._renderItem = function (ul, item) {
        if (item.label != "No matches found.") {
            var HumanDetails = $.parseJSON(item.val);
            var list_item = $("<li>")
                .attr({ "data-value": item.value, "data-val": item.val }).css({ "border-bottom": "1px solid #ccc", "font-size": "11px", "margin-bottom": "3px", "padding-bottom": "3px" })
                .append(item.label)
                .appendTo(ul);
            if (HumanDetails.Account_Status.toUpperCase() == "INACTIVE")
                list_item.addClass("inactive");
            if (HumanDetails.Status.toUpperCase() == "DECEASED")
                list_item.addClass("deceased");
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
});

function KeepPatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtKeepAccount = document.getElementById("txtKeepAccount");

    var WSData = {
        HumanID: ui.item.value,
        FullDetails: ui.item.label
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/GetHumanDetails",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            $('#btnDownload').prop('disabled', false);
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtKeepAccount = document.getElementById('txtKeepAccount');
            txtKeepAccount.value = SelectedPatient.DisplayString;
            txtKeepAccount.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
        }
    });
    txtKeepAccount.value = ui.item.label;
    txtKeepAccount.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    return false;
}

function MergePatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtMergeAccount = document.getElementById("txtMergeAccount");

    var WSData = {
        HumanID: ui.item.value,
        FullDetails: ui.item.label
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "./frmFindPatient.aspx/GetHumanDetails",
        data: JSON.stringify(WSData),
        dataType: "json",
        success: function (data) {
            $('#btnDownload').prop('disabled', false);
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtMergeAccount = document.getElementById('txtMergeAccount');
            txtMergeAccount.value = SelectedPatient.DisplayString;
            txtMergeAccount.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
        }
    });
    txtMergeAccount.value = ui.item.label;
    txtMergeAccount.attributes['data-human-id'].value = ui.item.value;
    return false;
}

function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}

function Filter(array, terms) {
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 1 && arrayOfTerms[1].trim() != "") {
        var first_resultant = array;
        var resultant;
        for (var i = 1; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {
                return item.label.toLowerCase().indexOf(arrayOfTerms[i].toLowerCase()) > -1;
            });
            first_resultant = resultant;
        }
        return first_resultant;
    }
    else {
        return array;
    }
}

function download() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    var humanIdKeep = $('#txtKeepAccount').attr('data-human-id');
    var humanIdMerge = $('#txtMergeAccount').attr('data-human-id');
    if (humanIdKeep == "0" || humanIdKeep == 0) {
        DisplayErrorMessage('10113601');
        return;
    }
    if (humanIdMerge == "0" || humanIdMerge == 0) {
        DisplayErrorMessage('10113602');
        return;
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    StartRcopiaStrip();
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "frmRCopiaPatientMerge.aspx/DownloadRcoipa",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ dtClientDate: utc, ulHumanIDKeep: humanIdKeep, ulHumanIDMerge: humanIdMerge }),
            dataType: "json",
            async: true,
            success: function (data) {
                StopRcopiaStrip();
                if (data.d == "Success") {
                    DisplayErrorMessage('10113603');
                    $("#imgClearKeepAccount").click();
                    $("#imgCleartxtMergeAccount").click();
                    $('#btnDownload').prop('disabled', true);
                } else if (data.d != "") {
                    alert(data.d);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
            error: function (result) {
                StopRcopiaStrip();
                sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
            }
        });
    }, 1000);
}

function patientMergeClose() {
    if ($("#btnDownload").is(":disabled") == false) {
        var Continue = DisplayErrorMessage('220019');
        if (Continue == true) {
            $(top.window.document).find("#btnRCopiaPatientMergeClose").click();
        }
        else if (Continue == false) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    }
    else {
        $(top.window.document).find("#btnRCopiaPatientMergeClose").click();
    }
};