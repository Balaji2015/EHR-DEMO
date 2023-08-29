
function HistoryDrug_Load() {    
    if (window.parent.parent.theForm.hdnSaveButtonID != undefined)
        window.parent.parent.theForm.hdnSaveButtonID.value = "btnSave,RadMultiPage1";
    top.window.document.getElementById('ctl00_Loading').style.display = "none";
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
    $("input[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    sessionStorage.setItem("Drug_UserRole", document.cookie.split("UserRoll=")[1].split(";")[0]);
    $('#btnAdd')[0].disabled = true;
    $("#tbodytblDrugAllergy th").addClass('tblDrugAllergyHeader')
    $.ajax({
        type: "POST",
        url: "WebServices/DrugAllergyService.asmx/LoadDrugAllergies",
        data: '',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: "false",
        success: function (data) {
            if (data != '') {
                var objdata = $.parseJSON(data.d);
                var tbody = $("#tbodytblDrugAllergy");
                $("#tbodytblDrugAllergy").empty();
                var DrugAllergylst = objdata.DrugAllergyList;
                var currProcess = objdata.CurrentProcess;

                if (currProcess != "SCRIBE_PROCESS" && currProcess != "AKIDO_SCRIBE_PROCESS" && currentprocess.toUpperCase() != "SCRIBE_REVIEW_CORRECTION" && currentprocess.toUpperCase() != "SCRIBE_CORRECTION" && currProcess != "DICTATION_REVIEW" && currProcess != "PHYSICIAN_CORRECTION" && currProcess != "PROVIDER_PROCESS" && currProcess != "MA_REVIEW" && currProcess != "MA_PROCESS" && currProcess != "") {//BugID:44637
                    $('#btnAdd')[0].disabled = true;
                    $('#btnclear')[0].disabled = true;
                    $('#mainContainer ').find(':input').prop('disabled', true);
                    $('#mainContainer ').find('textarea').prop('disabled', true);
                    $('a').attr("disabled", true);
                    $('a').attr("onclick", "return false;");
                    $('a').css("background-color", "#6D7777 !important");

                    $('#mainContainer ').find('select').prop('disabled', true);
                    for (var i = 0; i < DrugAllergylst.length; i++) {
                        var tr;
                        if (DrugAllergylst[i].Status.toUpperCase() == "ACTIVE") {
                            tr = '<tr style="background-color:#e0dede;"><td  style="width: 5%;padding-left:12px!important;"><img src="Resources/edit disabled.png" /></td><td style="width: 5%;padding-left:12px!important;"><img  src="Resources/Delete-Grey.png" /></td><td style="width: 30%;">' + DrugAllergylst[i].Drug_Allergy_Name + '</td><td style="width: 30%;">' + DrugAllergylst[i].Reaction + '</td><td style="width: 30%;">' + DrugAllergylst[i].Notes + '</td><td style="display:none">' + DrugAllergylst[i].Id + '</td></tr>';
                            tbody.append(tr);
                        }
                       
                    }
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                } else {
                    for (var i = 0; i < DrugAllergylst.length; i++) {
                        var tr;
                        if (DrugAllergylst[i].Status.toUpperCase() == "ACTIVE") {
                            tr = '<tr><td title="Edit" style="width: 5%;padding-left:12px!important;"><img src="Resources/edit.gif" onclick="Update(this);"/></td><td title="Delete" style="width: 5%;padding-left:12px!important;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width: 30%;">' + DrugAllergylst[i].Drug_Allergy_Name + '</td><td style="width: 30%;">' + DrugAllergylst[i].Reaction + '</td><td style="width: 30%;">' + DrugAllergylst[i].Notes + '</td><td style="display:none">' + DrugAllergylst[i].Id + '</td></tr>';
                            tbody.append(tr);
                        }
                       
                    }

                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }

            }
            scrolify($('#tblDrugAllergy'), 200);
            $('#tblDrugAllergy th').addClass('Gridheaderstyle');
            $('#tblDrugAllergy tr:odd').addClass('Gridtroddstyle');
            $('#tblDrugAllergy tr:even').addClass('Gridtrevenstyle');
            
            FunFrequencyused();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
function EnableSave()
{
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = true;
    localStorage.setItem("bSave", "false");
    $('#btnAdd')[0].disabled = false;
}
function OpenPopup(Keyword) {
    var focused = Keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}
function GetReaction(icon, List) {

    $('#btnAdd')[0].disabled = false;
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('.fa-minus').addClass('fa-plus').removeClass('fa-minus');
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");

        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#btnAdd')[0].disabled = false;
                var values = response.d.split("|");
                var targetControlValue = $(icon).parent().siblings().prop('id');
                var innerdiv = '';
                var txtPos = $('#' + targetControlValue).position();
                var pos = $('#' + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:44642
                var topPosition = txtPos.top;
                if (sessionStorage.getItem("Drug_UserRole").toUpperCase() == "PHYSICIAN" || sessionStorage.getItem("Drug_UserRole").toUpperCase() == "PHYSICIAN ASSISTANT") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"AddItem('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";
                }
                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    //CAP-804 Syntax error, unrecognized expression
                    $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                      .css({
                          top: topPosition + $(".actcmpt").height() + 11,
                          left: pos.left,
                          width: pos.width,
                          height: '100px',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '521px',
                          border: '1px solid #8e8e8e',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto',
                          'border-radius': '2px'
                      }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                }
                EnableSave();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
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
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");      
    }
}
function GetNotesList(icon, List) {
    $('#btnAdd')[0].disabled = false;
    if (icon.className.indexOf("plus") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('.fa-minus').addClass('fa-plus').removeClass('fa-minus');
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus");

        var ListValue = List;
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: '{fieldName: "' + ListValue + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#btnAdd')[0].disabled = false;
                var values = response.d.split("|");
                var targetControlValue = $(icon).parent().siblings().prop('id');
                var innerdiv = '';
                var txtPos = $('#' + targetControlValue).position();
                var pos = $('#' + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:44642
                var topPosition = txtPos.top;
                if (sessionStorage.getItem("Drug_UserRole").toUpperCase() == "PHYSICIAN" || sessionStorage.getItem("Drug_UserRole").toUpperCase() == "PHYSICIAN ASSISTANT") {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].name + "');\">Click here to Add or Update Keywords</li>";
                }
                for (var i = 0; i < values.length ; i++) {
                    innerdiv += "<li style='text-decoration: none; list-style-type: none;color:black;cursor:default' onclick=\"AddItem('" + values[i].split("\r\n").join("\n").split("\n").join("~") + "^" + targetControlValue + "');\">" + values[i] + "</li>";
                }
                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    //CAP-804 Syntax error, unrecognized expression
                    $("<div id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                      .css({
                          top: topPosition + $(".actcmpt").height() + 3,
                          left: pos.left,
                          width: pos.width,
                          height: '100px',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          floating: 'top',
                          width: '521px',
                          border: '1px solid #8e8e8e',
                          fontFamily: 'Segoe UI",Arial,sans-serif',
                          fontSize: '12px',
                          zIndex: '17',
                          overflowX: 'auto',
                          'border-radius': '2px'                        
                      }).insertAfter($("#" + targetControlValue?.trim() + ".actcmpt"));
                }
                EnableSave();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            },
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
    else {
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus");
    }
}
var bBool = false;
var bcheck = true;
var intCPTLength = -1;
var drugarry = [];
var drugarryUnique = [];
$(document).ready(function () {
    $("#mDrugName").autocomplete({
        source: function (request, response) {           
            if (intCPTLength == 0 && bcheck && bBool == false) {
                bBool = true;
                drugarry = [];
                drug_dts = [];
                drugs = [];
                $.ajax({
                    type: "POST",
                    url: "frmRxHistory.aspx/LoadDrugsDetails",
                    data: '{fieldName: "' + $("#mDrugName").val() + '"}',
                    contentType: "application/json;charset=utf-8",
                    datatype: "json",
                    success: function success(data) {
                        var jsonData = JSON.parse(data.d);
                        if (jsonData.length == 0) {
                            jsonData.push('No matches found.')
                            response($.map(jsonData, function (item) {
                                return {
                                    label: item
                                }
                            }));
                        }
                        else {
                            for (var i = 0; i < jsonData.length; i++) {
                                drug_dts[i] = "Drug_Name:" + jsonData[i].split('~')[0] + "$Route_of_Admin:" + jsonData[i].split('~')[1] + "$Strength:" + jsonData[i].split('~')[2];
                                drugarry.push(jsonData[i].split('~')[0]);
                            }
                            for (i = 0; i < drugarry.length; i++) {
                                if (drugarryUnique.indexOf(drugarry[i].toUpperCase()) === -1) {
                                    drugarryUnique.push(drugarry[i].toUpperCase());
                                }
                            }
                            if (drugarryUnique.length != 0) {
                                var results = PossibleCombination(drugarryUnique, request.term);
                                if (results.length == 0) {
                                    results.push('No matches found.')
                                    response($.map(results, function (item) {
                                        return {
                                            label: item
                                        }
                                    }));
                                }
                                else {
                                    response($.map(results, function (item) {
                                        return {
                                            label: item
                                        }
                                    }));
                                }
                            }
                        }
                        $("#mDrugName").focus();
                        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                    },
                    error: function onerror(xhr) {
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
            if ($("#mDrugName").val().length > 3) {
                if (drugarryUnique.length != 0) {
                    var results = PossibleCombination(drugarryUnique, request.term);
                    if (results.length == 0) {
                        results.push('No matches found.')
                        response($.map(results, function (item) {
                            return {
                                label: item
                            }
                        }));
                    }
                    else {
                        response($.map(results, function (item) {
                            return {
                                label: item
                            }
                        }));
                    }
                }
            }
        },
        minlength: 2,
        multiple: true,
        mustMatch: false,
        open: function () {
            $('.ui-menu').width(520);
            $('.ui-menu').height(200);
            $(".ui-autocomplete").find('a:contains("No matches found.")').on("click", function (e) {
                e.preventDefault();
                e.stopImmediatePropagation();
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            if (ui.item.label != "No matches found.") {
                bBool = false;
                $('#mDrugName').val(ui.item.label);
                EnableSave();
            }
            else {
                bBool = false;
                $('#mDrugName').val("");
            }
        }
    }).on("paste", function (e) {
        intCPTLength = -1;
        drugarry = [];
        drugarryUnique = [];
        $(".ui-autocomplete").hide();
    })
        .on("keydown", function (e) {
        if (e.which == 8) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if ($("#mDrugName").val().length <= 3)
                bBool = false;
            else
                bBool = true;
            $("#mDrugName").focus();
            bcheck = false;
        }
        else if (e.which == 46) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bBool = false;
            bcheck = false;
        }
        else {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            bcheck = true;
        }

    }).on("input", function (e) {
        if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if ($("#mDrugName").val().length >= 3) {
            if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (!bBool)
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            intCPTLength = 0;
        }
        else if ($("#mDrugName").val().length != 0 && intCPTLength != -1) {
            intCPTLength = intCPTLength + 1;
        }
        if ($("#mDrugName").val().length < 3) {
            intCPTLength = -1;
            drugarry = [];
            drugarryUnique = [];
            $(".ui-autocomplete").hide();
            bBool = false;
        }
    });
    $("#btnAdd").click(function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($("#mDrugName").val().trim() != "") {
            var DrugName = $("#mDrugName").val();
            var Reaction = $("#Reactiontxtarea").val();
            var Status = "ACTIVE";
            var Notes = $("#genralnotes").val();
            var AllergyID = 0;
            if ($("#btnAdd")[0].innerText.toUpperCase() == "UPDATE") {
                AllergyID = sessionStorage.getItem("EditDrugItemID");
            }
            var WSData = [];
            WSData.push(AllergyID, DrugName, Status, Reaction, Notes);
            $.ajax({
                type: "POST",
                url: "WebServices/DrugAllergyService.asmx/SaveDrugAllergies",
                data: JSON.stringify({ Drug_Data : WSData }),
                contentType: "application/json;charset=utf-8",
                datatype: "json",
                success: function success(data){
                    var objdata = $.parseJSON(data.d);
                    var DrugAllergylst = objdata.DrugAllergyList;
                    var currProcess = objdata.CurrentProcess;
                    $('#DrugAllergyTable').empty();
                    var tableContents = "";
                    for (var i = 0; i < DrugAllergylst.length; i++) {
                        if (DrugAllergylst[i].Status.toUpperCase() == "ACTIVE")
                        {
                            tableContents += '<tr><td title="Edit" style="width: 5%;padding-left:12px!important;"><img src="Resources/edit.gif" onclick="Update(this);"/></td><td title="Delete" style="width: 5%;padding-left:12px!important;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width: 30%;">' + DrugAllergylst[i].Drug_Allergy_Name + '</td><td style="width: 30%;">' + DrugAllergylst[i].Reaction + '</td><td style="width: 30%;">' + DrugAllergylst[i].Notes + '</td><td style="display:none">' + DrugAllergylst[i].Id + '</td></tr>';
                        }
                    }
                    $('#DrugAllergyTable').append("<table id='tblDrugAllergy' style='width:100%;text-align:left;' class='table table-bordered table-condensed Gridbodystyle'><thead id='DrugHead' style='background-color:#cacaca;width:100%!important;'><tr><th style='width:5%!important;'>Edit</th><th style='width:5%!important;'>Del</th><th style='width:30%!important;'>Drug Name</th><th style='width:30%!important;'>Reaction</th><th style='width:30%!important;'>Notes</th></tr></thead><tbody id='tbodytblDrugAllergy' style='width:100%!important'>" + tableContents + "</tbody></table>");
                    $("#btnAdd")[0].innerText = "Add";
                    $("#btnclear")[0].innerText = "Clear All";
                    $("#mDrugName").val("");
                    $("#Reactiontxtarea").val("");
                    $("#genralnotes").val("");
                    scrolify($('#tblDrugAllergy'), 200);
                    $('#tblDrugAllergy th').addClass('Gridheaderstyle');
                    $('#tblDrugAllergy tr:odd').addClass('Gridtroddstyle');
                    $('#tblDrugAllergy tr:even').addClass('Gridtrevenstyle');

                    var AllergyListTooltip = objdata.Tooltip;
                    var regex = /<BR\s*[\/]?>/gi;
                    top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = AllergyListTooltip[0];
                    top.window.document.getElementById("Allergies_tooltp").innerText = AllergyListTooltip[1].replace(regex, "\n") + "\n";
                    RefreshOverallSummaryTooltip();
                    SavedSuccessfully();
                    if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                },
                error: function onerror(xhr) {
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
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            DisplayErrorMessage('3101');
            $("#mDrugName").focus();
        }
    });

    $("#btnclear").click(function () {
        var IsClearAll = null;
        if ($('#btnclear')[0].innerHTML == "Clear All")
            IsClearAll = DisplayErrorMessage('180010');
        else
            if ($('#btnclear')[0].innerHTML == "Cancel")
                IsClearAll = DisplayErrorMessage('180049');
        if (IsClearAll != undefined && IsClearAll == true) {
            $("#btnAdd")[0].innerText = "Add";
            $("#btnclear")[0].innerText = "Clear All";
            $("#mDrugName").val("");
            $("#Reactiontxtarea").val("");
            $("#genralnotes").val("");
            window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            localStorage.setItem("bSave", "true");
        }
    });
});


function AddItem(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BugID:44641
    var sugglistval;
    var control;
    var value = agrulist.split("^");
    if (value.length > 2) {
        //CAP-804 Syntax error, unrecognized expression
        control = value[5]?.trim();
        sugglistval = $("#" + control + ".actcmpt").val().trim();
        var selectedvalue = value[0] + ',' + value[1] + ',' + value[2] + ',' + value[3] + ',' + value[4];
        if (sugglistval != " " && sugglistval != "") {
            var subsugglistval = sugglistval;
            var len = subsugglistval.length;
            var flag = 0;
            if (subsugglistval == selectedvalue) {
                flag++;
            }
            if (flag == 0) {
                $("#" + control + ".actcmpt").val(sugglistval + "," + selectedvalue);
            }
        }
        else {
            $("#" + control + ".actcmpt").val(selectedvalue);
        }
    }
    else {
        //CAP-804 Syntax error, unrecognized expression
        sugglistval = $("#" + value[1]?.trim() + ".actcmpt").val().trim();
        if (sugglistval != " " && sugglistval != "") {
            var subsugglistval = sugglistval.split(",")
            var len = subsugglistval.length;
            var flag = 0
            for (var i = 0; i < len; i++) {
                if (subsugglistval[i] == value[0]) {
                    flag++;
                }
            }
            if (flag == 0) {
                $("#" + value[1]?.trim() + ".actcmpt").val(sugglistval + "," + value[0]);
            }
        }
        else {
            $("#" + value[1]?.trim() + ".actcmpt").val(value[0]);
        }
    }
}
function PossibleCombination(array, txtValue) {
    aICDDesc = txtValue.split(" ");
    var ResultArray = array;
    var SearchValues;
    for (var i = 0; i < aICDDesc.length; i++) {
        SearchValues = $.grep(ResultArray, function (ResulttxtDesc) {
            return ResulttxtDesc.toLowerCase().indexOf(aICDDesc[i].toLowerCase()) > -1;
        });
        ResultArray = SearchValues;
    }
    return ResultArray;
}
var DelId = 0;
function Delete(item) {    
    if (item != null)
        DelId = item.parentElement.parentElement.cells[5].innerText;
    if (DisplayErrorMessage('110021') == true) {        
        $.ajax({
            type: "POST",
            url: "WebServices/DrugAllergyService.asmx/DeleteDrugAllergies",
            data: JSON.stringify({ DrugAllergyID: DelId }),
            contentType: "application/json;charset=utf-8",
            datatype: "json",
            success: function success(data) {
                var objdata = $.parseJSON(data.d);
                var DrugAllergylst = objdata.DrugAllergyList;
                var currProcess = objdata.CurrentProcess;
                $('#DrugAllergyTable').empty();
                var tableContents = "";
                for (var i = 0; i < DrugAllergylst.length; i++) {
                    if (DrugAllergylst[i].Status.toUpperCase() == "ACTIVE") {
                        tableContents += '<tr><td title="Edit" style="width: 5%;padding-left:12px!important;"><img src="Resources/edit.gif" onclick="Update(this);"/></td><td title="Delete" style="width: 5%;padding-left:12px!important;"><img  src="Resources/Delete-Blue.png" onclick="Delete(this);" /></td><td style="width: 30%;">' + DrugAllergylst[i].Drug_Allergy_Name + '</td><td style="width: 30%;">' + DrugAllergylst[i].Reaction + '</td><td style="width: 30%;">' + DrugAllergylst[i].Notes + '</td><td style="display:none">' + DrugAllergylst[i].Id + '</td></tr>';
                    }
                }
                $('#DrugAllergyTable').append("<table id='tblDrugAllergy' style='width:100%;text-align:left;' class='table table-bordered table-condensed Gridbodystyle'><thead id='DrugHead' style='background-color:#cacaca;width:100%!important;'><tr><th style='width:5%!important;'>Edit</th><th style='width:5%!important;'>Del</th><th style='width:30%!important;'>Drug Name</th><th style='width:30%!important;'>Reaction</th><th style='width:30%!important;'>Notes</th></tr></thead><tbody id='tbodytblDrugAllergy' style='width:100%!important'>" + tableContents + "</tbody></table>");
                $("#btnAdd")[0].innerText = "Add";
                $("#btnclear")[0].innerText = "Clear All";
                $("#mDrugName").val("");
                $("#Reactiontxtarea").val("");
                $("#genralnotes").val("");
                scrolify($('#tblDrugAllergy'), 200);
                localStorage.setItem("bSave", "false");
                var AllergyListTooltip = objdata.Tooltip;
                var regex = /<BR\s*[\/]?>/gi;
                top.window.document.getElementById("ctl00_C5POBody_lblAllergies").innerHTML = AllergyListTooltip[0];
                top.window.document.getElementById("Allergies_tooltp").innerText = AllergyListTooltip[1].replace(regex, "\n") + "\n";
                RefreshOverallSummaryTooltip();
                if (jQuery(top.window.parent.parent.parent.parent.parent.parent.document.body).find('#resultLoading').css('display') == 'block')
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

            },
            error: function onerror(xhr) {
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
}
function Update(item) {
    $("#mDrugName").val($(item).parent().parent()[0].childNodes[2].textContent);
    $("#Reactiontxtarea").val($(item).parent().parent()[0].childNodes[3].textContent);
    $("#genralnotes").val($(item).parent().parent()[0].childNodes[4].textContent);
    $("#btnAdd")[0].innerText = "Update";
    $("#btnAdd").removeClass("disabled");
    $("#btnAdd")[0].disabled = false;
    $("#btnclear")[0].innerText = "Cancel";
    sessionStorage.setItem("EditDrugItemID", $(item).parent().parent()[0].childNodes[5].textContent);
    EnableSave();
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
        $("#ScrollStatic").css('height', '');
        $("#ScrollStatic").css('overflow-y', '');
    }
}
function openAddorUpdate() {    
    var fieldName = "Frequency Used Drugs";
    $(top.window.document).find("#Modal").modal({ backdrop: "static" });
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + fieldName;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";

    $(top.window.document).find("#Modal").on('hidden.bs.modal', function () {
    })
}
function FunFrequencyused()
{
    $.ajax({
        type: "POST",
        url: "WebServices/DrugAllergyService.asmx/LoadFrequencyDrugs",
        data: '',
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: "false",
        success: function (data) {
            if (data!= '') {
                var objdata = $.parseJSON(data.d);
                var vFrequencylist = $("#Frequencylist");
                $("#Frequencylist").empty();
                for (var i = 0; i < objdata.length; i++) {
                    var option = document.createElement("OPTION");
                    option.innerHTML = objdata[i];
                    option.value = objdata[i];
                    $("#Frequencylist")[0].options.add(option);
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
function FrequencySelect()
{
    $("#mDrugName").val("");
    var vfrq = $("#Frequencylist")[0].value;
    $("#mDrugName").val(vfrq);
    $("#btnAdd")[0].disabled = false;
    EnableSave();
}

function SavedSuccessfully() {
    localStorage.setItem("bSave", "true");
    DisplayErrorMessage('180602');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null && window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
    else
        window.parent.theForm.hdnSaveEnable.value = false;
    PFSH_AfterAutoSave();
}

