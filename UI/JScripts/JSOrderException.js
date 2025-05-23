function btn_Match_Clicked(sender, eventArgs)
{ { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();} }
function OnMatch()
{  {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();} DisplayErrorMessage('7100008'); }


function enableField(ChkValue)
    {
    var  pcontrol = document.getElementById(ChkValue);
     if(pcontrol.checked==true)
           {
             document.getElementById("chkSearchbyfacility").checked=false;
             document.getElementById("chkShowAllResults").checked=false;
             document.getElementById("pnlSearchbyfacility").disabled=true;
             document.getElementById("cboFacilityList").SelectedIndex=0;
             document.getElementById("pnlFindbyPatient").enabled=true;
           }
    }
    function Result(iResultMasterID)
    {
    
    var obj=new Array();
obj.push("Result_Master_ID="+iResultMasterID);
obj.push("Order_ID=0");
obj.push("strScreenName=ORDER EXCEPTION");
        obj.push("bMovetonextprocess=false");
//Jira CAP-1144
    //setTimeout(function(){GetRadWindow().BrowserWindow.openModal("frmLabResult.aspx",750,845,obj,"MessageWindow");},0);
        setTimeout(openNonModal("frmLabResult.aspx", 780, 1250, obj), 0);
    }
      function FindPatient()
     {
         var obj=new Array();
         var result = openModal("frmFindPatient.aspx", 251, 1200, obj, 'MessageWindow');
         var winObj=$find('MessageWindow');
         winObj.add_close(OnClientCloseOrderManagement);
     }
     function OnRowSelected(sender, eventArgs) 
     {
       var rowindex = eventArgs.get_itemIndexHierarchical();
       var accno=eventArgs.get_gridDataItem()._element.cells[0].innerHTML;
       var patName=eventArgs.get_gridDataItem()._element.cells[7].innerHTML;
       var DOB=eventArgs.get_gridDataItem()._element.cells[8].innerHTML;
       var txtGen=eventArgs.get_gridDataItem()._element.cells[9].innerHTML;
       if(accno!="&nbsp;")
       {
            $find('txtAccountNumber').set_value(accno);
            $find('txtPatientName').set_value(patName);
            $find('txtDOB').set_value(DOB);
            $find('txtGender').set_value(txtGen);
       }
    }
     function OnClientCloseOrderManagement(oWindow, args) 
    {
       var arg = args.get_argument();
       if (arg)
       {
        document.getElementById(GetClientId("hdnHumanID")).value=arg.HumanId;
        document.getElementById('InvisibleButton').click();           
         
       }
 
    }
    function GetRadWindow() 
{
     var oWindow = null;
     if (window.radWindow) oWindow = window.radWindow;
     else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
     return oWindow;
}
 function HandleOnCheck()
    {
            var target = event.target || event.srcElement;
            var chkLst = document.getElementById('chklPhysicianlist').getElementsByTagName('input');
            for(var item=0;item<chkLst.length;item++)
            {
                if(target.id!=chkLst[item].id)
                chkLst[item].checked=false;
            }
    }
    
function RadWindowClose()
{
//Jira CAP-1144
        //var oWindow = null;
        //  if (window.radWindow)
        //       oWindow = window.radWindow;
        //  else if (window.frameElement.radWindow)
        //       oWindow = window.frameElement.radWindow;
        //  if(oWindow!=null)
    //   oWindow.close();
    window.close();
}

function txtToolTip(txtname)
{
var txtTool=document.getElementById(txtname); 
if(txtTool.value.length>0)
{
txtTool.title=txtTool.value;
}
}

function Clear()
{
    var IsClearAll=DisplayErrorMessage('200005');
	if(IsClearAll==true)
	{
        return true;
    }
    return false;
}

function btnFindPatient_Clicked(sender,args)
	{
	  var obj=new Array();
	  var result = openModal("frmFindPatient.aspx", 251, 1200, obj, 'MessageWindow');
         var winObj=$find('MessageWindow');
         winObj.add_close(OnClientCloseOrderManagement);
	}
	
	function btnViewPendingOrder_Clicked(sender,args)
	{
	     var obj=new Array();
         var result=openModal("frmOrderManagement.aspx",650,1130,obj,'MessageWindow');
	}
	
	function checkRadioButton()
	{
	    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
	document.getElementById('SearchClick').click();
	}
	function check()
	{
	    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
	}
	
	function  checkOnclcik()    
	{
	if(document.getElementById('chkNoOrders').checked==true)
	   deselect();
	}
	function deselect()
 {
 
   var masterTable = $find("grdOutstandingOrders").get_masterTableView();
   var row = masterTable.get_dataItems();
   for (var i = 0; i < row.length; i++)
   {
     masterTable.get_dataItems()[i].set_selected(false);
   }
}

	function btnSearch_Clicked(sender,args)
	{
	    
	    if ($telerik.findDatePicker("frmDate")._element.value != "" && $telerik.findDatePicker("toDate")._element.value == "") {//BugID:46054
	        DisplayErrorMessage('7100015');
	        sender.set_autoPostBack(false);
	    }
	    else {
	        sender.set_autoPostBack(true);
	        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
	    }
	    
	}

function btnMatchOrders_Clicked(sender,args)
	{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
	
	}


	
	function grdUnassignedResults_OnRowClick(sender,args)
	{
	    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
	}

	function LabExcepLoad() {
	    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
	}

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args) {
    initializeAutocomplete();   
});

$(document).ready(function () {

    initializeAutocomplete();
});
var intPatientlen = -1;
function initializeAutocomplete() {
    var curleft = curtop = 0;
    var current_element = document.getElementById('txtPatientSearch');
   
    if (current_element == null) {
        current_element = document.getElementById('txtProviderSearch');
        curtop = 5;
    }
    window.setTimeout(function () {
        $('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
    }, 50);

    if (current_element && current_element.offsetParent) {
        do {
            curleft += current_element.offsetLeft;
            curtop += current_element.offsetTop;
        } while (current_element = current_element.offsetParent);
    }
    $("#imgClearPatientText").css({
        "cursor": "pointer",
        "width": "20px",
        "height": "20px"
    }).on("click", function () {
        $('#txtPatientSearch').val('').focus();
        $('#txtPatientSearch')[0].setSelectionRange(0, 0);
        $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
        intPatientlen = -1;
        arrPatient = [];
        $(".ui-autocomplete").hide();
        sessionStorage.setItem("valuepatientsearch", "");
        sessionStorage.setItem("labelpatientsearch", "")
        $("#imgClearPatientText").removeClass("disabled");
        $('#txtPatientSearch').prop('disabled', false);
    });
    if ($("#txtPatientSearch").length) {
        $("#txtPatientSearch").autocomplete({
            source: function (request, response) {
                if ($("#txtPatientSearch").val().trim().length > 2) {
                    if (intPatientlen == 0) {
                        UI_Time_Start = new Date();
                        //{ sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                        StartLoadingImage();
                        this.element.on("keydown", PreventTyping);
                        arrPatient = [];
                        var strkeyWords = $("#txtPatientSearch").val().split(' ');
                        var bMoreThanOneKeyword = (strkeyWords.length >= 2 && strkeyWords[1].trim() != "") ? true : false;
                        var account_status = "ACTIVE";
                        var patient_status = "ALIVE";
                        var patient_type = "REGULAR";
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
                                // { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                StopLoadingImage();
                                $("#txtPatientSearch").off("keydown", PreventTyping);
                                var jsonData = $.parseJSON(data.d);
                                if (jsonData.Error != undefined) {
                                    alert(jsonData.Error);
                                    return;
                                }
                                if (jsonData.Time_Taken != undefined)
                                    LogTimeString(jsonData.Time_Taken);
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
                                //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                StopLoadingImage();
                                if (xhr.status == 999)
                                    window.location = xhr.statusText;
                                else {
                                    var log = JSON.parse(xhr.responseText);
                                    console.log(log);
                                    alert("USER MESSAGE:\n" + xhr.status + "-" + xhr.statusText +
                                        ". \nCannot process request. Please Login again and retry. If issue persists, Please contact Support.\n\nEXCEPTION DETAILS: \nException Type" +
                                        log.ExceptionType + " \nMessage: " + log.Message);
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
            select: PatientSelected,
            open: function () {
                $('.ui-autocomplete.ui-menu.ui-widget').width($('#txtPatientSearch').width());
                $('.ui-autocomplete.ui-menu.ui-widget').find('li:last').css("border-bottom", "0px");
                $('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
            },
            focus: function () {
                return false;
            }
        }).on("paste", function (e) {
            intPatientlen = -1;
            arrPatient = [];
            $(".ui-autocomplete").hide();
        }).on("input", function (e) {
            $("#txtPatientSearch").css("color", "black").attr({ "data-human-id": "0", "data-human-details": "" });
            if ($("#txtPatientSearch").val().charAt(e.currentTarget.value.length - 1) == " ") {
                if (e.currentTarget.value.split(" ").length > 2)
                    intPatientlen = intPatientlen + 1;
                else
                    intPatientlen = 0;
            }
            else {
                if ($("#txtPatientSearch").val().length != 0 && intPatientlen != -1) {
                    intPatientlen = intPatientlen + 1;
                }

                if ($("#txtPatientSearch").val().length == 0 || $("#txtPatientSearch").val().indexOf(" ") == -1) {
                    intPatientlen = -1;
                    arrPatient = [];
                    $(".ui-autocomplete").hide();
                }
            }
        })

        $("#txtPatientSearch").data("ui-autocomplete")._renderItem = function (ul, item) {
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
    }
}
function LogTimeString(time_string) {
    UI_Time_Stop = new Date();

    var WS_Time = parseFloat(time_string.split(';')[0].split(':')[1].replace('s', ''));
    var DB_Time = parseFloat(time_string.split(';')[1].split(':')[1].replace('s', ''));
    var UI_Time = ((UI_Time_Stop.getTime() - UI_Time_Start.getTime()) / 1000) - WS_Time - DB_Time;
    console.log(time_string + " UI_Time :" + UI_Time + "s; Total_Time :" + (WS_Time + DB_Time + UI_Time).toString() + "s;");

}

function PreventTyping(e) {
    e.preventDefault();
    e.stopImmediatePropagation();
}

function PatientSelected(event, ui) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $(document).on("click", PreventTyping).on("keydown", PreventTyping).css('cursor', 'wait');
    var txtPatientSearch = document.getElementById("txtPatientSearch");

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
            var SelectedPatient = JSON.parse(data.d);
            var HumanDetails = SelectedPatient.HumanDetails;
            var txtPatientSearch = document.getElementById('txtPatientSearch');
            txtPatientSearch.value = SelectedPatient.DisplayString;
            txtPatientSearch.attributes['data-human-details'].value = JSON.stringify(HumanDetails);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            $(document).off("click", PreventTyping).off("keydown", PreventTyping).css('cursor', 'default');
        }
    });
    txtPatientSearch.value = ui.item.label;
    txtPatientSearch.attributes['data-human-id'].value = ui.item.value;//HumanDetails.HumanId;
    document.getElementById('hdnHumanID').value = ui.item.value;
    $('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
    debugger;
    //document.getElementById('hdnpatientsearch').value = ui.item.label;
    //document.getElementById('InvisibleButton').click();   
    sessionStorage.setItem("valuepatientsearch", ui.item.value);
    sessionStorage.setItem("labelpatientsearch", ui.item.label);
    __doPostBack('ctl00$ContentPlaceHolder1$InvisibleButton', '');
    return false;
}

function setpatientsearch(sAutosearch) {
    var txtPatientSearch = document.getElementById('txtPatientSearch');
    debugger;
    if (sessionStorage.getItem("labelpatientsearch") != undefined && sessionStorage.getItem("labelpatientsearch") != "") {
        txtPatientSearch.value = sessionStorage.getItem("labelpatientsearch");
        txtPatientSearch.attributes['data-human-id'].value = sessionStorage.getItem("valuepatientsearch");
        if (sAutosearch == "Y") {
            $('#txtPatientSearch').prop('disabled', false);
            //$('#imgClearPatientText').prop('disabled', false);
            $("#imgClearPatientText").removeClass("disabled");
            //txtPatientSearch.focus();
            //txtPatientSearch.setSelectionRange(0, 0);
            $('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
            $('#txtPatientSearch').focus()[0].scrollLeft = 0;
        }
    }
    else {
        txtPatientSearch.value = "";
        txtPatientSearch.attributes['data-human-id'].value = "";
    }
}
function FindPatientenabled(val, sPatientname) {
    var txtPatientSearch = document.getElementById('txtPatientSearch');
    debugger;
    if (val == "True") {
        $('#txtPatientSearch').prop('disabled', true);
        //$('#imgClearPatientText').prop('disabled', true);
        $("#imgClearPatientText").addClass("disabled");
        txtPatientSearch.value = sPatientname;
        txtPatientSearch.attributes['data-human-id'].value = sPatientname;
        $('#txtPatientSearch').focus()[0].setSelectionRange(0, 0);
        sessionStorage.setItem("valuepatientsearch", sPatientname);
        sessionStorage.setItem("labelpatientsearch", sPatientname);
    }
    else if (val == "Success") {
        
        $('#txtPatientSearch').prop('disabled', true);
        $("#imgClearPatientText").addClass("disabled");
        sessionStorage.setItem("valuepatientsearch", "");
        sessionStorage.setItem("labelpatientsearch", "");
        txtPatientSearch.value = "";
        txtPatientSearch.attributes['data-human-id'].value = "";
        sessionStorage.setItem('StartLoading', 'false');
        
    }
    else {
        debugger;
        $('#txtPatientSearch').prop('disabled', false);
        //$('#imgClearPatientText').prop('disabled', false);
        $("#imgClearPatientText").removeClass("disabled");
        sessionStorage.setItem("valuepatientsearch", "");
        sessionStorage.setItem("labelpatientsearch", "");
        txtPatientSearch.value = sPatientname;
        txtPatientSearch.attributes['data-human-id'].value = sPatientname;
    }

}