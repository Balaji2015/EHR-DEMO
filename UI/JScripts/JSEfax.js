$('body').on('keydown', 'input, select', function (e) {
    var self = $(this)
        , form = self.parents('form:eq(0)')
        , focusable
        , next
        ;

    if (e.keyCode == 13) {
        focusable = form.find('input,a,select,button').filter(':visible');
        next = focusable.eq(focusable.index(this));
        if (next.length) {
            next.focus();
        } else {
            form.submit();
        }
        return false;
    }
});

$('#txtRecName').keydown(
    function (event) {
        if (event.which == '13') {
            event.preventDefault();
        }
    });



$(top.window.document).find("#TabEFaxFrame")[0].style.height = "630px";
var intProviderlen = -1;
var arrProvider = []; 
var MAX_TOTAL_BYTES = "10485760";
var filelimit = bytesToSize(MAX_TOTAL_BYTES);
var uploadsInProgress = 0;
var filesSize = new Array();
var OVERSIZE_MESSAGE = "You are only allowed to add up to 20mb of files total";
var isDuplicateFile = false;
$(document).ready(function () {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var curleft = curtop = 0;
    var current_element = document.getElementById('txtPatientSearch');
    if (current_element == null) {
        current_element = document.getElementById('txtRecName');
        curtop = 5;
    }
   
    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    
    $("#txtRecName").autocomplete({
        source: function (request, response) {
            if ($("#txtRecName").val().trim().length > 2) {
                if (intProviderlen == 0) {

                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    this.element.on("keydown", PreventTyping);
                    arrProvider = [];
                    var strkeyWords = $("#txtRecName").val().split(' ');
                    var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                    var WSData;
                   
                    
                    var vurl = "";
                    var test = $("input:radio[name='chkProvider1']:checked").val()
                    if (test == "Provider") {
                        WSData = {
                            text_searched: strkeyWords[0],
                        };
                        vurl = "./frmFindReferralPhysician.aspx/GetProviderDetailsFaxByTokens"
                    }
                    else if (test == "Patient") {
                        WSData = {                            
                            text_searched: strkeyWords[0],
                            account_status: "ACTIVE",
                            patient_status: "ALIVE",
                            human_type: "REGULAR"
                        };
                        vurl = "./frmFindPatient.aspx/GetPatientDetailsByTokens"
                    }
                                      
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: vurl,
                        data: JSON.stringify(WSData),
                        dataType: "json",
                        success: function (data) {
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            $("#txtRecName").off("keydown", PreventTyping);
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
                                    results = FilterFax(jsonData.Matching_Result, request.term);
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

                    var results = FilterFax(arrProvider, request.term);
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
            $('.ui-autocomplete.ui-menu.ui-widget').width("600px!important");
            $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
            $('#txtRecName').focus();
        },
        focus: function () { return false; }
    }).on("paste", function (e) {
        intProviderlen = -1;
        arrProvider = [];
        $(".ui-autocomplete").hide();
    }).on("input", function (e) {
        $("#txtRecName").css("color", "black").attr({ "data-phy-id": "0", "data-phy-details": "" });      
        if ($("#txtRecName").val().length == 0 ) {
            intProviderlen = -1;
            arrProvider = [];
            $(".ui-autocomplete").hide();
        }
        else {
            intProviderlen = 0;
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
    $("#txtSenderName").keypress(function (e) {       
        if (e.keyCode == 64)
            return false;        
    });
    $("#txtSenderCompany").keypress(function (e) {       
        if (e.keyCode == 64)
            return false;        
    });
    $("#txtRecName").keypress(function (e) {
        if (e.keyCode == 64)
            return false;        
    });
    $("#txtRecipientcompany").keypress(function (e) {
        if (e.keyCode == 64)
            return false;        
    });
});

function FilterFax(array, terms) {
    arrayOfTerms = terms.split(" ");
    if (arrayOfTerms.length > 1 && arrayOfTerms[1].trim() != "") {
        var first_resultant = array;
        var resultant;
        for (var i = 1; i < arrayOfTerms.length; i++) {
            resultant = $.grep(first_resultant, function (item) {
                if (item.label != undefined)
                    return item.label.toUpperCase().indexOf(arrayOfTerms[i].toString().toUpperCase()) > -1;
                else if (item != undefined)
                    return item.toUpperCase().indexOf(arrayOfTerms[i].toString().toUpperCase()) > -1;
            });
            first_resultant = resultant;
        }
        return first_resultant;
    }
    else {
        return array;
    }
}


function ValidationSendFax() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById('txtSenderName').value == "") {
        DisplayErrorMessage('1011130');

        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if (document.getElementById(GetClientId("txtSenderEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtSenderEmail")).value) == false) {

        DisplayErrorMessage('320010');
        document.getElementById(GetClientId("txtSenderEmail")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if ($('#tblFaxDetails tbody tr').length == 0)
    {
        if (document.getElementById('txtRecName').value == "") {
            DisplayErrorMessage('1011131');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else if (document.getElementById('msktxtRecipientFax').value == "" || document.getElementById('msktxtRecipientFax').value == "-") {
            DisplayErrorMessage('1011132');
            document.getElementById('msktxtRecipientFax').value = "";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        else if (document.getElementById(GetClientId("txtRecipientmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtRecipientmail")).value) == false) {

            DisplayErrorMessage('320010');
            document.getElementById(GetClientId("txtRecipientmail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else {
            var hdnpriority = $('#DropDwnpriority option:selected').text() + "|" + $('#DropDwncoverpage option:selected').text();
            $('#hdnpriority').val(hdnpriority);
            if ($('#txtSenderMaskFax').val() == "-") {
                $('#txtSenderMaskFax').val("")
            }
        }
    }
    else if (document.getElementById('txtRecName').value == "" && document.getElementById('msktxtRecipientFax').value != "") {
        DisplayErrorMessage('1011131');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if(document.getElementById('txtRecName').value != "" && (document.getElementById('msktxtRecipientFax').value == "" || document.getElementById('msktxtRecipientFax').value == "-")) {
        DisplayErrorMessage('1011132');
        document.getElementById('msktxtRecipientFax').value = "";
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        var hdnpriority = $('#DropDwnpriority option:selected').text() + "|" + $('#DropDwncoverpage option:selected').text();
        $('#hdnpriority').val(hdnpriority);
        if ($('#txtSenderMaskFax').val() == "-") {
            $('#txtSenderMaskFax').val("")
        }
    }

    if (document.getElementById('txtRecName').value != "" || document.getElementById('msktxtRecipientFax').value != "") {
        var Category = '';
        if (document.getElementById("chkProvider").checked)
            Category = document.getElementById("chkProvider").value;
        else if (document.getElementById("chkpatient").checked)
            Category = document.getElementById("chkpatient").value;
        var Fax = document.getElementById("msktxtRecipientFax").value;
        if (Fax != "")
            Fax = "+1" + Fax.replace("-", "").replace("(", "").replace(")", "");
        tabContents = "<tr class='Gridbodystyle'>"
       + "<td style='width:5%;'><img src=" + "Resources/close_small_pressed.png" + " onclick='confirmMessage(this);'" + "/></td>"
       + "<td style='width:15%;'>" + Category + "</td>"
       + "<td style='width:15%;'>" + document.getElementById("txtRecName").value + "</td>"
       + "<td style='width:28%;'>" + document.getElementById("txtRecipientcompany").value + "</td>"
       + "<td style='width:10%;'>" + Fax + "</td>"
       + "<td style='width:25%;'>" + document.getElementById("txtRecipientmail").value + "</td></tr>";
        if ($('#tblFaxDetails tbody tr').length == 0)
            $("#tblFaxDetails").find('tbody').append(tabContents);
        else
            $("#tblFaxDetails").find('tbody tr:last').after(tabContents);
        clear();
    }
    var FaxRecipientDetails = new Array();

    //Reference the Table.
    var table = document.getElementById("tblFaxDetails");

    //Loop through Table Rows.
    for (var i = 1; i < table.rows.length; i++) {
        //Reference the Table Row.
        var row = table.rows[i];

        //Copy values from Table Cell to JSON object.
        var FaxRecipient = {};
        FaxRecipient.Category = row.cells[1].innerHTML;
        FaxRecipient.Name = row.cells[2].innerHTML;
        FaxRecipient.Company = row.cells[3].innerHTML;
        FaxRecipient.Fax = row.cells[4].innerHTML;
        FaxRecipient.EMail = row.cells[5].innerHTML;
        FaxRecipientDetails.push(FaxRecipient);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("hdnAddgrid").value = JSON.stringify(FaxRecipientDetails);
}

function EnableSend() {
    document.getElementById('btnSendfax').disabled = false;
}
var myPos, atPos;
function Closefax() {
    if (document.getElementById('btnSendfax').disabled == false) {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (!$(document).find('body').is('#dvdialogMenu'))
            $(document).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: 332px; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
        dvdialog = $(document).find('body').find('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';

        event.preventDefault();

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
                    $('#btnSendfax').trigger('click');
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btnFaxClose").click();
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    return;

                }
            }
        });
    } else {
        $(top.window.document).find("#btnFaxClose").click();
    }
    
        self.close();
   
}
function ActivityHistoryClick() {
    
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        url: "frmEFax.aspx/GetActivities",
        data: '{FieldValue: "' + "EFax" + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessActivity,
        error: function OnError(xhr) {
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
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    return false;

}
function OnSuccessActivity(response) {
    $(top.window.document).find('#ActiveModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#ActiveModalTitle")[0].textContent = "Activity Log";
    $($($(top.window.document).find('#ActiveModal')).find('#ActiveMdlDlg')).find('.modal-content').css('overflow-y', 'auto');
    $($($($($(top.window.document).find('#ActiveModal')).find('#ActiveMdlDlg')).find('.modal-content')).find('.modal-body')).find('#ctl00_TextBox1').val(response.d);
}
function ImageTempalteClick() {
    
    var vcoverpage = $('#DropDwncoverpage').val();
    $(top.window.document).find('#PrintimageModal').modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#PrintimageModalTitle")[0].textContent = "View Template";
    $($($(top.window.document).find('#PrintimageModal')).find('#Printimagemdldlg')).find('.modal-content').css('overflow-y', 'auto');
    
    $($($($($($(top.window.document).find('#PrintimageModal')).find('#Printimagemdldlg')).find('.modal-content')).find('.modal-body')).find('#PrintimageFrame')).attr("src", vcoverpage);
    $($($($($($(top.window.document).find('#PrintimageModal')).find('#Printimagemdldlg')).find('.modal-content')).find('.modal-body')).find('#PrintimageFrame')).contents().find('body').append($("<img/>").attr("src", vcoverpage).attr("title", "sometitle").css({ 'background-repeat': 'no-repeat' }));

    
}
function IsEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);

}



function LoadEfax()
{
    //var avoidPostBack = document.getElementById("hdnavoidPostBack").value;
    //if (avoidPostBack == "false") 
    {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        var vDMEOrder = '';
        var hdnAttachfile = document.getElementById("hdnAttachfile").value;
        var re = /%20/gi;
        var vlocation = window.location.href.split('&');
        for (var l = 0; l < vlocation.length; l++) {
            if (vlocation[l].split("DMEOrder=")[1] != undefined && vlocation[l].indexOf("DMEOrder=") > -1) {
                if (hdnAttachfile != "") {
                    vDMEOrder = hdnAttachfile;
                }
                else
                    vDMEOrder = vlocation[l].split("=")[1].replace(/%20/gi, ' ');
            }
            else if (vlocation[l].split("Result=")[1] != undefined && vlocation[l].indexOf("Result=") > -1) {
                if (vlocation[l].split("=")[1].split("|")[0] != undefined) {
                    if (hdnAttachfile != "") {
                        vDMEOrder = hdnAttachfile;
                    }
                    else
                        vDMEOrder = vlocation[l].split("=")[1].split("|")[0].replace(/%20/gi, ' ');

                    var Ordersid = 0;
                    var vResultid = 0;
                    if (vlocation[l].split("=")[1].split("|")[1] != undefined)
                        Ordersid = vlocation[l].split("=")[1].split("|")[1].replace(/%20/gi, ' ');
                    if (vlocation[l].split("=")[1].split("|")[2] != undefined)
                        vResultid = vlocation[l].split("=")[1].split("|")[2].replace(/%20/gi, ' ');
                    if (Ordersid != 0 && Ordersid != undefined && Ordersid.trim() != "") {
                        $.ajax({
                            type: "POST",
                            url: "frmEFax.aspx/OrdersList",
                            contentType: "application/json;charset=utf-8",
                            data: '{Ordersid: "' + Ordersid + '"}',
                            datatype: "json",
                            success: function success(data) {

                                var ordersubmitList = JSON.parse(data.d);
                            },
                            error: function onerror(xhr) {
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
                    else {
                    }

                }
                else {
                    if (hdnAttachfile != "") {
                        vDMEOrder = hdnAttachfile;
                    }
                    else
                        vDMEOrder = vlocation[l].split("=")[1].replace(/%20/gi, ' ');
                }
            }
        }
        if (localStorage['FaxSubject'] != undefined && localStorage['FaxSubject'] != "") {
            $('#txtSubject').val(localStorage['FaxSubject']);
            localStorage['FaxSubject'] = "";
        }
        $('#chkProvider').click(function () {
            $("#txtRecName").val('');
            $("#btnprov").attr('disabled', false);
        });
        $('#chkpatient').click(function () {
            $("#txtRecName").val('');
            $("#btnprov").attr('disabled', true);

        });
        //Input Mask for landline phone number
        $("#msktxtRecipientFax").mask("(999)999-9999");
        $("#txtSenderMaskFax").mask("(999)999-9999");

        $.ajax({
            type: "POST",
            url: "frmEFax.aspx/GetFaxload",
            contentType: "application/json;charset=utf-8",
            data: '',
            datatype: "json",
            success: function success(data) {

                var FaxLoadList = JSON.parse(data.d);
                if (FaxLoadList.SenderName != undefined && FaxLoadList.SenderName != '') {
                    $('#txtSenderName').val(FaxLoadList.Nameoftheuser);
                }
                if (FaxLoadList.SenderCompany != undefined && FaxLoadList.SenderCompany != '') {
                    $('#txtSenderCompany').val(FaxLoadList.SenderCompany);
                }
                if (FaxLoadList.txtSenderMaskFax != undefined && FaxLoadList.txtSenderMaskFax != '') {
                    var fax = '(' + FaxLoadList.txtSenderMaskFax.substring(0, 3) + ')' + FaxLoadList.txtSenderMaskFax.substring(3, 6) + '-' + FaxLoadList.txtSenderMaskFax.substring(6, 10);
                    $('#txtSenderMaskFax').val(fax);
                }
                if (FaxLoadList.Email != undefined && FaxLoadList.Email != '') {

                    $('#txtSenderEmail').val(FaxLoadList.Email);
                }
                if (vDMEOrder != undefined && vDMEOrder != '') {
                    $('#lblattach').html(vDMEOrder);
                }
                if (FaxLoadList.LookUpList != undefined && FaxLoadList.LookUpList != '') {

                    $("#DropDwnpriority option").remove();
                    var ddlDropDwnpriority = document.getElementById("DropDwnpriority");
                    var ddlDropDwncoverpage = document.getElementById("DropDwncoverpage");
                    $("#DropDwncoverpage option").remove();
                    for (var i = 0; i < FaxLoadList.LookUpList.length; i++) {
                        if (FaxLoadList.LookUpList[i].Field_Name == "Fax_Priority") {
                            var option = document.createElement('option');
                            option.text = FaxLoadList.LookUpList[i].Value;
                                option.value = FaxLoadList.LookUpList[i].Description;
                            ddlDropDwnpriority.add(option);
                        }
                        else if (FaxLoadList.LookUpList[i].Field_Name == "Fax_Cover Page") {
                            var option = document.createElement('option');
                            option.text = FaxLoadList.LookUpList[i].Value;
                            option.value = FaxLoadList.LookUpList[i].Description;
                            ddlDropDwncoverpage.add(option);
                        }
                        //Jira CAP-1416
                        //else if (FaxLoadList.LookUpList[i].Field_Name == "SIGNATURE") {
                        else if (FaxLoadList.LookUpList[i].Field_Name == "EFAX_SIGNATURE") {
                            $("#txtareaCoverpage").val(FaxLoadList.LookUpList[i].Value.replace("<Name of the user>", FaxLoadList.Nameoftheuser).replace("<ClientName>", FaxLoadList.FaciltyName).replace("<Facility Address>", FaxLoadList.FacilityAddress).replace("<Facility Phone Number>", FaxLoadList.FacilityPhoneNumber).replace("<Facility Fax Number>", FaxLoadList.FacilityFaxNumber));
                        }
                    }
                    for (var x = 0; x < ddlDropDwnpriority.length - 1 ; x++) {
                        if (ddlDropDwnpriority.options[x].value == "50")
                            ddlDropDwnpriority.selectedIndex = x;
                    }
                   
                    for (var x = 0; x < ddlDropDwncoverpage.length - 1 ; x++) {
                        if (ddlDropDwncoverpage.options[x].text == "Standard")
                            ddlDropDwncoverpage.selectedIndex = x;
                    }
                }
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

            },
            error: function onerror(xhr) {
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
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    $("span[mand=Yes]").addClass('MandLabelstyle');

    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });

    $("textarea[id *= txtDLC]").removeClass('DlcClass');

    $("textarea[id *= txtDLC]").addClass('Editabletxtbox');
    $('#dvpbdropdown').css('display','none')
}
function confirmMessage(evt) {
    if (window.confirm("Are you sure you want to delete?")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $(evt).closest('tr').remove();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        return false;
    }

}

function scrolify(tblAsJQueryObject, height) {
    if (document.getElementById('dvAdd') != undefined)
        $('#dvAdd').remove();
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
var tabContents = '';
function btnNewAddGrid() {
    if (document.getElementById('txtRecName').value == "") {
        DisplayErrorMessage('1011131');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else if (document.getElementById('msktxtRecipientFax').value == "" || document.getElementById('msktxtRecipientFax').value == "-") {
        DisplayErrorMessage('1011132');
        document.getElementById('msktxtRecipientFax').value = "";
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var Category = '';
    if (document.getElementById("chkProvider").checked)
        Category = document.getElementById("chkProvider").value;
    else if (document.getElementById("chkpatient").checked)
        Category = document.getElementById("chkpatient").value;
    var txtProviderSearch = document.getElementById("txtRecName");
    Category = txtProviderSearch.attributes['data-category'].value;
    var Fax = document.getElementById("msktxtRecipientFax").value;
    if (Fax != "")
        Fax = "+1" + Fax.replace("-", "").replace("(", "").replace(")", "");
    tabContents = "<tr class='Gridbodystyle'>"
       + "<td style='width:5%;'><img src=" + "Resources/close_small_pressed.png" + " onclick='confirmMessage(this);'" + "/></td>"
       + "<td style='width:15%;'>" + Category + "</td>"
       + "<td style='width:15%;'>" + document.getElementById("txtRecName").value + "</td>"
       + "<td style='width:28%;'>" + document.getElementById("txtRecipientcompany").value + "</td>"
       + "<td style='width:10%;'>" + Fax + "</td>"
       + "<td style='width:25%;'>" + document.getElementById("txtRecipientmail").value + "</td></tr>";
    if ($('#tblFaxDetails tbody tr').length == 0)
       $("#tblFaxDetails").find('tbody').append(tabContents);
   else
        $("#tblFaxDetails").find('tbody tr:last').after(tabContents);
    clear();

    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
   return false;
}
function clear() {
    document.getElementById("msktxtRecipientFax").value = '';
    document.getElementById("txtRecipientcompany").value = '';
    document.getElementById("txtRecName").value = '';
    document.getElementById("txtRecipientmail").value = '';
}
function ProviderSearchclear() {
    $("#txtRecName").val("");
    $("#txtRecipientcompany").val("");
    $("#msktxtRecipientFax").val("");
    $("#txtRecipientmail").val("");
}
function ProviderSelected(event, ui) {
    var ProviderDetails = JSON.parse(ui.item.val);
    var txtProviderSearch = document.getElementById("txtRecName");

    txtProviderSearch.attributes['data-phy-id'].value = ProviderDetails.ulPhyId;
    txtProviderSearch.attributes['data-phy-details'].value = JSON.stringify(ProviderDetails);
    txtProviderSearch.attributes['data-category'].value = ProviderDetails.sCategory;
    txtProviderSearch.value = ui.item.label;   
    if (ProviderDetails.sPhyFax != undefined && ProviderDetails.sPhyFax != '') {
        var fax = '(' + ProviderDetails.sPhyFax.replace("(", "").replace(")", "").substring(0, 3) + ')' + ProviderDetails.sPhyFax.replace("(", "").replace(")", "").replace(" ", "").substring(3, 6) + '-' + ProviderDetails.sPhyFax.replace("(", "").replace(")", "").replace(" ", "").replace("-", "").substring(6, 10);
        $('#msktxtRecipientFax').val(fax);
    }
    else {
        $('#msktxtRecipientFax').val('');
    }
    if (ProviderDetails.sphyEmail != undefined && ProviderDetails.sphyEmail != '') {
       
        $('#txtRecipientmail').val(ProviderDetails.sphyEmail);
    }
    else
    {
        $('#txtRecipientmail').val('');
    }
    if (ProviderDetails.sPhyCompany != undefined && ProviderDetails.sPhyCompany != '') {

        $('#txtRecipientcompany').val(ProviderDetails.sPhyCompany);
    }
    else
    {
        $('#txtRecipientcompany').val('');
    }

    
    return false;
}
function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}
function Efaxsaveend() {   
    var flag = localStorage.getItem("IsMenuEFax");
    if (flag == "Y") {
        DisplayErrorMessage('1011136');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
    else {
        DisplayErrorMessage('1011136');
        window.close();
        Closefax();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}
function OpenProvider() {
    localStorage.setItem("IsEnableGrid", "false");
    localStorage.setItem("IsEFax", "true");
    $(top.window.document).find("#TabPhysicianLibrary").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalPhysicianLibraryTitle")[0].textContent = "Address Book";
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

function fileUploadValidationFailed(sender, args) {
    var fileExtention = args.get_fileName().substring(args.get_fileName().lastIndexOf('.') + 1, args.get_fileName().length);
    if (args.get_fileName().lastIndexOf('.') != -1) {//this checks if the extension is correct
        if (sender.get_allowedFileExtensions().indexOf(fileExtention.toLowerCase()) == -1) {
            alert("File type selected is not allowed.  Valid file types are *.TIFF , *.PDF , *.PNG , *.JPG , *.GIF");
        }
    }
    else {
        alert("File type selected is not allowed.  Valid file types are *.TIFF , *.PDF , *.PNG , *.JPG , *.GIF");
    }
    sender._updateCancelButton(args.get_row());
    $telerik.$(".ruRemove", args.get_row()).click();
}

function onFileUpload(sender, args) {

    var totalBytes = 0;
    var indeximage = 0;
    var numberOfFiles = sender._uploadedFiles.length;
    if (isDuplicateFile) {

        for (var i = numberOfFiles - 1; i >= 0; i--) {
            if (sender._uploadedFiles[i].fileInfo["FileName"] == args.get_fileName()) {

                indeximage = indeximage + 1;
                break;
            }
        }
        sender.deleteFileInputAt(indeximage);
        isDuplicateFile = false;
        sender.updateClientState();
        alert("Selected File has been added already.Please Rename File.");
        return;
    }

    for (var index in filesSize) {
        totalBytes += filesSize[index];
    }

   
    indeximage = 0;
    if (totalBytes > MAX_TOTAL_BYTES) {
        if (sender._uploadedFiles.length > 1) {
            for (var i = numberOfFiles - 1; i >= 0; i--) {
                if (sender._uploadedFiles[i].fileInfo["FileName"] == args.get_fileName()) {
                    indeximage = indeximage + 1;
                    break;
                }
            }

            sender.deleteFileInputAt(indeximage);
        }
        else
            sender.deleteFileInputAt(numberOfFiles);
        sender.updateClientState();

        alert("The file exceeds the " + filelimit + " MB attachment limit")

    }

}

function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB'];
    if (bytes == 0) return '0';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2);
}

function OnFilesUploaded(sender, args) {

    if (sender._uploadedFiles.length == 0) {
        filesSize = new Array();
        uploadsInProgress = 0;

    }
    if (uploadsInProgress > 0) {
        DecrementUploadsInProgress();
    }

}

function OnProgressUpdating(sender, args) {

    filesSize[args.get_data().fileName] = args.get_data().fileSize;

}

function OnFileUploadRemoved(sender, args) {
    if (args.get_fileName() != null) {
        if (!isDuplicateFile) {
            delete filesSize[args.get_fileName()];
        }
    }
}