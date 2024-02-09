

var MAX_TOTAL_BYTES = "2097152";
var filelimit = bytesToSize(MAX_TOTAL_BYTES);
var uploadsInProgress = 0;

var filesSize = new Array();
var OVERSIZE_MESSAGE = "You are only allowed to add up to 20mb of files total";
var isDuplicateFile = false;
function DownloadClick() {
    var RadDownload = $find('RadWindow1');
    RadDownload.show();
    //BugId:49570 
    if (event != undefined && event.target.id == "btnDownload") {
        $(top.window.document).find("#hdnIsZip")[0].value = "";
    }
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    document.getElementById("RadWindow1_C_Button1").disabled = true;
    document.getElementById('RadWindow1_C_rdnPdf').checked = false;
    document.getElementById('RadWindow1_C_rdnXml').checked = false;
    return false;

}
function loadchangepassword() {
    $("span[mand=Yes]").addClass('MandLabelstyle');


    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
$(document).ready(function () {
    var URL = document.URL;
    localStorage.setItem("PatientPortal", URL);
});
var summarcheck = "";
function hidePatChart() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if ($("#dvTest")[0].style.display == "block") {
        $("#dvTest").css("display", "none");
        $("#divPatChartContainer").css("display", "none");
    }
}
function tree_add_leaf_example_click(leaf, node, pnode, tree) {
   
    $("#dvCheck li .colored").removeClass("colored");
    $(leaf[0]).addClass("colored");
    if (leaf[0].id.split('^')[0] == "Summary Of Care") {
        var PrevTab = $($(top.window.document).find("iframe")[0].contentDocument).find(".tab-pane.active");
        if (PrevTab.length != 0) {
            var prvTab = PrevTab[0].attributes.id.value;

            if (prvTab.match("tbEPrescription") != null) {

                //Jira CAP-1366
                StartRcopiaStrip();

                $.ajax({
                    type: "POST",
                    url: "frmEncounter.aspx/DownloadRcoipa",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        //Jira CAP-1567
                        document.cookie = "CeRxFlag=false";
                        document.cookie = "CeRxHumanID=";
                        //Jira CAP-1366
                        StopRcopiaStrip();
                        RcopiaErrorAlert(data.d);
                    },
                    error: function OnError(xhr) {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        //Jira CAP-1366
                        StopRcopiaStrip();
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
        var now = new Date();
        var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
        var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
        document.getElementById(GetClientId("hdnLocalTime")).value = utc;
        var sStrinh = (window.frames["EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs') != null) ? window.frames["EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs').innerHTML.split('|')[1].trim() : "";
        
        var role = document.getElementById('hdnRole').Value;
        var WSData = "{\"encounter_id\":\"" + leaf[0].id.split('^')[1] + "\"}";
        document.getElementById('hdnEncounterId').value = leaf[0].id.split('^')[1];
        $.ajax({
            type: "POST",
            url: "webfrmPatientPortal.aspx/showreport",
            data: WSData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                $('#EncounterContainer')[0].src = "frmPrintPDF.aspx?SI=" + data.d + "&Location=DYNAMIC";
            },
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

        document.getElementById(GetClientId("hdnEncounterId")).value = leaf[0].id.split('^')[1];
        sessionStorage.setItem("EncId_PatSummaryBar", leaf[0].id.split('^')[1]);
        sessionStorage.setItem("Enc_DOS", leaf[0].innerText.split(' - ')[0]);

        // }

        // RefreshNotification('Notify');
        $(leaf[0]).addClass("colored");
        if (backcolor != "") {
            backcolor.style.backgroundColor = "";
        }
        backcolor = leaf[0];
        if (window.frames["EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs') != null) {
            window.frames["EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs').innerText = "";
        }
        document.getElementById("btnDownload").disabled = false;
        document.getElementById("btnSend").disabled = false;
        document.getElementById("btndelete").disabled = true;
    }
       
    else if (leaf[0].id.split('^')[3] == "Others" || leaf[0].id.split('^')[0] == "EncountersSub") {
        document.getElementById("btnDownload").disabled = true;
        document.getElementById("btnSend").disabled = true;
        
        document.getElementById("btndelete").disabled = false;
        var QueryString = "?HumanID=" + leaf[0].id.split('^')[2] + "&Key_id=" + leaf[0].id.split('^')[1] + "&Opening_from=" + "Patient_Pane";
        if (leaf[0].id.split('^')[0] == "EncountersSub")
            QueryString += "&Doc_type=" + $(node.parents()).find("li[id=liEncounters]")[0].childNodes[0].innerHTML;
        else
            QueryString += "&Doc_type=" + $(node.offsetParent().offsetParent()).find(".leaf")[0].innerHTML;
        $('#EncounterContainer')[0].src = "frmViewResult.aspx" + QueryString;
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }
    else if (leaf[0].id.split('^')[3] == "ResultsSub") {
        document.getElementById("btndelete").disabled = true;
        document.getElementById("btnDownload").disabled = true;
        document.getElementById("btnSend").disabled = true;
       
        var QueryString = "?HumanID=" + leaf[0].id.split('^')[2] + "&Key_id=" + leaf[0].id.split('^')[1] + "&Doc_type=" + $(node.offsetParent().offsetParent()).find(".leaf")[0].innerHTML + "&Opening_from=" + "Patient_Pane";
        if (leaf[0].id.split('^').length == 5)
            QueryString += "&OrderSubmitId=" + leaf[0].id.split('^')[4];
        $('#EncounterContainer')[0].src = "frmViewResult.aspx" + QueryString;
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    }
    else if (leaf[0].id.split('^')[0] == "Phone Encounter") {
        document.getElementById("btndelete").disabled = false;
        document.getElementById("btnDownload").disabled = true;
        document.getElementById("btnSend").disabled = true;
        var sScreenName = "PatientChart";
        $('#EncounterContainer')[0].src = "frmPhoneEncounter.aspx?openingfrom=" + sScreenName + "&EncId=" + leaf[0].id.split('^')[2];
    }

    $(leaf[0]).addClass("colored");
    hidePatChart();
}
var backcolor = '';

function makeUL(array) {
    list = document.createElement('ul');


    var item = document.createElement('li');
    item.className = "node";
    item.id = "liEncounters";
    var span1 = document.createElement('span');
    span1.id = "sEncounterID";
    span1.className = "leaf";
    span1.innerHTML = "Summary Of Care";
    item.appendChild(span1);
    var span2 = document.createElement('span');
    span2.className = "node-toggle";
    item.appendChild(span2);

    var list1 = document.createElement('ul');
    var item1 = document.createElement('li');
    item1.className = "active";
    var sCheck = '';
    var nodepresent = false;

    for (var i = 0; i < array.length; i++) {
        if (array[i].split('^')[0] == "Summary Of Care") {
            var sStrinh = (window.frames["EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs') != null) ? window.frames["EncounterContainer"].contentDocument.getElementById('pnlBarGroupTabs').innerHTML.split('|')[1].trim() : "";
            var ulistEnc = document.createElement('ul');
            var listEnc = document.createElement('li');
            var spaninner = document.createElement('p');
            spaninner.className = "para";
            spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[1];
            spaninner.title = array[i].split('^')[2].split(';')[0] + " - " + array[i].split('^')[2].split(';')[1];
            spaninner.currentId = array[i].split('^')[3];
            spaninner.From = array[i].split('^')[4];
            spaninner.innerHTML = array[i].split('^')[2].split(';')[0] + " - " + array[i].split('^')[2].split(';')[1];
            if (sStrinh == array[i].split('^')[2].split(';')[0]) {
                $(spaninner).addClass("colored");
                backcolor = spaninner;
            }
            listEnc.appendChild(spaninner);
            ulistEnc.appendChild(listEnc);
            item1.appendChild(ulistEnc);
            list1.appendChild(item1);
        }


    }
    item.appendChild(list1);
    if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "EncountersSub") {
                if ($(item).children().children().find("p:contains('" + array[i].split('^')[2].split(' ')[0] + "')").length > 0) {
                    var itemsub = $(item).children().children().find("p:contains('" + array[i].split('^')[2].split(' ')[0] + "')").parent()[0];
                    itemsub.id = "subEncounters";
                    nodepresent = true;
                }
                else if (sCheck != array[i].split('^')[2].split(' ')[0]) {
                    if ($(item).children().children().find("span:contains('" + array[i].split('^')[2].split(' ')[0] + "')").length > 0) {
                        var itemsub = $(item).children().children().find("span:contains('" + array[i].split('^')[2].split(' ')[0] + "')").parent()[0];
                        nodepresent = true;
                    }
                    else {
                        var itemsub = document.createElement('li');
                        itemsub.className = "node";
                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = array[i].split('^')[2].split(' ')[0];
                        sCheck = span1.innerHTML;
                        itemsub.appendChild(span1);

                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        itemsub.appendChild(span2);
                        nodepresent = false;
                    }

                }
                itemsub.className = "node";
                var spanEnc = document.createElement('span');
                spanEnc.className = "node-toggle";
                var list1sub = document.createElement('ul');
                var item1sub = document.createElement('li');
                var spaninner = document.createElement('p');
                spaninner.className = "para";
                spaninner.id = array[i].split('^')[0] + "^" + array[i].split('^')[1] + "^" + array[i].split('^')[6];
                spaninner.innerHTML = array[i].split('^')[3];

                item1sub.appendChild(spaninner);
                list1sub.appendChild(item1sub);
                itemsub.appendChild(spanEnc);
                itemsub.appendChild(list1sub);
                if (nodepresent == false) {
                    list1.appendChild(itemsub);
                }
            }
        }
        item.appendChild(list1);
    }

   
    var itemResults = document.createElement('li');
    itemResults.id = "liResults";
    itemResults.className = "node";
    var span1 = document.createElement('span');
    span1.className = "leaf";
    span1.innerHTML = "Results";
    itemResults.appendChild(span1);

    var span2 = document.createElement('span');
    span2.className = "node-toggle";
    itemResults.appendChild(span2);



    var list1 = document.createElement('ul');
    var item1 = document.createElement('li');
    item1.className = "active";

    var resultsHeader = [];
    var resultsDate = [];
    var resultSubarray = [];
    var resltHeaderarr = [];
    var resltDatearr = [];
    var header = [];

    var mainResults = [];
    var mainResultArray = [];
    for (var i = 0; i < array.length; i++) {
        if (array[i].split('^')[0] == "Results") {
            if (mainResults.indexOf(array[i].split('^')[2]) == -1) {
                mainResults.push(array[i].split('^')[2]);
                mainResultArray.push(array[i]);
            }
        }
    }
    mainResults.sort(function (a, b) {
        return Date.parse(a) - Date.parse(b);
    });
    mainResults.reverse();
    for (var k = 0; k < mainResults.length; k++) {
        for (var i = 0; i < mainResultArray.length; i++) {
            if (mainResultArray[i].split('^')[0] == "Results" && mainResultArray[i].split('^')[2] == mainResults[k]) {
                var spaninner = document.createElement('p');
                if (mainResultArray[i].split('^')[1] == '0') {
                    spaninner.id = mainResultArray[i].split('^')[0] + "^" + mainResultArray[i].split('^')[1] + "^" + mainResultArray[i].split('^')[3] + "^" + mainResultArray[i].split('^')[5] + "^" + mainResultArray[i].split('^')[7];
                    spaninner.title = mainResultArray[i].split('^')[6];
                }
                else {
                    spaninner.id = mainResultArray[i].split('^')[0] + "^" + mainResultArray[i].split('^')[1] + "^" + mainResultArray[i].split('^')[4] + "^" + mainResultArray[i].split('^')[5];
                    spaninner.title = mainResultArray[i].split('^')[5];
                }
                spaninner.className = "para";
                spaninner.innerHTML = mainResultArray[i].split('^')[2];
                item1.appendChild(spaninner);
                list1.appendChild(item1);
            }
        }
    }
    for (var i = 0; i < array.length; i++) {
        if (array[i].split('^')[0] == "ResultsSub") {
            resultSubarray.push(array[i]);
            if (resultsHeader.indexOf(array[i].split('^')[3]) == -1) {
                resultsHeader.push(array[i].split('^')[3]);
                header.push(0);
            }
            if (resultsDate.indexOf(array[i].split('^')[2].split(' ')[0]) == -1) {
                resultsDate.push(array[i].split('^')[2].split(' ')[0]);
            }
        }
    }


    resultsDate.sort(function (a, b) {
        return Date.parse(a) - Date.parse(b);
    });
    resultsDate.reverse();


    for (var k = 0; k < resultsDate.length; k++) {
        for (var l = 0; l < resultSubarray.length; l++) {
            if (resultSubarray[l].split('^')[2].split(' ')[0] == resultsDate[k] && resltDatearr.indexOf(resultSubarray[l]) == -1)
                resltDatearr.push(resultSubarray[l]);
        }
    }
    for (var k = 0; k < resultsHeader.length; k++) {
        for (var l = 0; l < resltDatearr.length; l++) {
            if (resltDatearr[l].split('^')[3] == resultsHeader[k]) {
                if (header[k] == 0) {
                    var list1 = document.createElement('ul');
                    var item1 = document.createElement('li');
                    item1.className = "active";
                    header[k] = 1;

                    var itemsub = document.createElement('li');
                    itemsub.className = "node";
                    var span1 = document.createElement('span');
                    span1.className = "leaf";
                    span1.innerHTML = resltDatearr[l].split('^')[3].split('_')[0];
                    sCheck = span1.innerHTML;
                    itemsub.appendChild(span1);

                    var span2 = document.createElement('span');
                    span2.className = "node-toggle";
                    itemsub.appendChild(span2);
                }


                var list1sub = document.createElement('ul');
                var item1sub = document.createElement('li');
                var spaninner = document.createElement('p');
                spaninner.className = "para";
                if (resltDatearr[l].split('^').length == 9) {
                    spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[6] + "^" + resltDatearr[l].split('^')[0] + "^" + resltDatearr[l].split('^')[7];
                    spaninner.title = resltDatearr[l].split('^')[4];
                }
                else {
                    spaninner.id = resultsHeader[k] + "^" + resltDatearr[l].split('^')[1] + "^" + resltDatearr[l].split('^')[6] + "^" + resltDatearr[l].split('^')[0];
                    if (resltDatearr[l].split('^').length == 9)
                        spaninner.title = resltDatearr[l].split('^')[8];
                }
                spaninner.innerHTML = resltDatearr[l].split('^')[2];

                item1sub.appendChild(spaninner);
                list1sub.appendChild(item1sub);
                itemsub.appendChild(list1sub);
                list1.appendChild(itemsub);

                if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
                else { span2.classList.add("node-toggle"); }
                itemResults.appendChild(list1);
            }
        }
    }

    if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
    else { span2.classList.add("node-toggle"); }
    itemResults.appendChild(list1);

    var item12 = document.createElement('li');
    item12.className = "node";
    item12.id = "liPatientTask";
    var span1 = document.createElement('span');
    span1.className = "leaf";
    span1.innerHTML = "Patient Task";
    item12.appendChild(span1);



    var span2 = document.createElement('span');
    span2.className = "node-toggle";
    item12.appendChild(span2);

    var list1 = document.createElement('ul');
    var item1 = document.createElement('li');
    item1.className = "active";
    list.appendChild(item);
    list.appendChild(itemResults);

    var headers = [];
    var k = [];
    var datelst = [];
    var arrayOthers = [];
    var arrayOthersNew = [];
    if (summarcheck != undefined && summarcheck.indexOf("viewresult") <= -1) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].split('^')[0] == "Others") {
                if (headers.indexOf(array[i].split('^')[5]) == -1) {
                    headers.push(array[i].split('^')[5]);
                    k.push(0);
                }

            }
        }
        for (var i = 0; i < headers.length; i++) {
            for (var j = 0; j < array.length; j++) {
                if (array[j].split('^')[0] == "Others") {
                    if (array[j].split('^')[5] == headers[i]) {
                        arrayOthers.push(array[j]);
                    }
                }
            }
        }
        for (var t = 0; t < arrayOthers.length; t++) {
            if (datelst.indexOf(arrayOthers[t].split('^')[2].split(' ')[0]) == -1) {
                datelst.push(arrayOthers[t].split('^')[2].split(' ')[0]);
            }
        }
        datelst.sort(function (a, b) {
            return Date.parse(a) - Date.parse(b);
        });

        for (var t = 0; t < datelst.length; t++) {
            for (var i = 0; i < arrayOthers.length; i++) {
                if (datelst[t] == arrayOthers[i].split('^')[2].split(' ')[0]) {
                    arrayOthersNew.push(arrayOthers[i]);
                }
            }
        }

        for (var j = 0; j < headers.length; j++) {
            var date = [];
            for (var i = 0; i < arrayOthersNew.length; i++) {
                if (headers[j] == arrayOthersNew[i].split('^')[5]) {
                    if (k[j] == 0) {
                        var iteminner = document.createElement('li');
                        iteminner.className = "node";
                        iteminner.id = headers[j].replace(" ", "").replace(" ", "").replace(" ", "");

                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = headers[j];
                        iteminner.appendChild(span1);


                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        iteminner.appendChild(span2);

                        var list1 = document.createElement('ul');
                        var item1 = document.createElement('li');
                        item1.className = "active";
                        k[j] = 1;
                    }

                    if (date.indexOf(arrayOthersNew[i].split('^')[2].split(' ')[0]) == -1) {
                        var itemsub = document.createElement('li');
                        itemsub.className = "node";
                        var span1 = document.createElement('span');
                        span1.className = "leaf";
                        span1.innerHTML = arrayOthersNew[i].split('^')[2].split(' ')[0];
                        sCheck = span1.innerHTML;
                        itemsub.appendChild(span1);

                        var span2 = document.createElement('span');
                        span2.className = "node-toggle";
                        itemsub.appendChild(span2);
                        date.push(arrayOthersNew[i].split('^')[2].split(' ')[0]);
                    }



                    var list1sub = document.createElement('ul');
                    var item1sub = document.createElement('li');
                    var spaninner = document.createElement('p');
                    spaninner.className = "para";
                    spaninner.id = headers[j] + "^" + arrayOthersNew[i].split('^')[1] + "^" + arrayOthersNew[i].split('^')[6] + "^" + arrayOthersNew[i].split('^')[0];
                    spaninner.innerHTML = arrayOthersNew[i].split('^')[3];

                    item1sub.appendChild(spaninner);
                    list1sub.appendChild(item1sub);
                    itemsub.appendChild(list1sub);

                    list1.appendChild(itemsub);

                    if (list1.childElementCount == 0) { span2.classList.remove("node-toggle"); }
                    else { span2.classList.add("node-toggle"); }
                    iteminner.appendChild(list1);

                    list.appendChild(iteminner);
                }

            }

        }
     
    }
    return list;
}
function OpenOnlinedocuments() {
    StartLoadingImage();
    var obj = new Array();
    var screen = "PatientPortalOnlineDoumnets";
    obj.push("Screen=" + screen);
    var dateonclient = new Date;
    var Tz = (dateonclient.getTimezoneOffset());
    document.cookie = "Tz=" + Tz;
    var result = openModal("frmOnlineDocuments.aspx", 700, 1225, obj, "ctl00_ModalWindow");
    return false;
}

function deletefile() {
    var test = document.getElementById('EncounterContainer').contentDocument
    document.getElementById('hdnfilepath').value = $(test).find('form').find('input#hdnpath').val();
    document.getElementById('hdnindexid').value = $(test).find('form').find('input#hdnfileindexid').val();
    var r = confirm("Are you sure you want to delete the selected File?");
    if (r == true) {
        return true;
    } else {
        return false;
    }
}
function openGenrateLink() {

    var test = document.getElementById('EncounterContainer').contentDocument
    var filepath = $(test).find('form').find('input#hdnpath').val().split('~')[0];
    var ispasswordfile = '';
    if ($(test).find('form').find('input#hdnpath').val().split('~').length > 1)
        ispasswordfile = $(test).find('form').find('input#hdnpath').val().split('~')[1].trim();
    var path = filepath + "@" + $(test).find('form').find('input#hdnfileindexid').val();;

    if (ispasswordfile != '') {

        alert("Password was already set for this file.");
        return false;
    }
    if (path.toUpperCase().indexOf("PDF") > -1) {
        var obj = new Array();
        var screen = "PatientPortalOnlineDoumnets";
        obj.push("Path=" + path);
        var dateonclient = new Date;
        var Tz = (dateonclient.getTimezoneOffset());
        document.cookie = "Tz=" + Tz;
        var result = openModal("frmGenerateLink.aspx", 200, 500, obj, "ctl00_ModalWindow");
        return false;
    }
    else {
        alert("Generate Link can be done only for Pdf files");
        return false;
    }
}
var iCount = 0;
function CheckMe() {
    if ($("#dvTest")[0].style.display == "block") {
        $("#dvTest").css("display", "none");
        $("#divPatChartContainer").css("display", "none");
    }
    else {
        $("#divPatChartContainer").css("display", "block");
        $("#dvTest").css("display", "block");

        if ($("#divTreeview").children().length == 0)//if data is not present
        {
            jQuery(top.window.parent.document).find('#divLoadingPatChart').css('display', "block");
            $("#divLoading").css('display', "block");

            if (iCount == 0) {
                var search = $('#HumanID').val();
                var WSData = "{\"text\":\"" + search + "\"}";
                $.ajax({
                    type: "POST",
                    url: "frmDLC.aspx/SearchDescrptionWebportal",
                    data: WSData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#divTreeview').empty();
                        makeUL(data.d);
                        $('#divTreeview')[0].appendChild(list);
                        $("#divLoading").css('display', "none");
                        jQuery(top.window.parent.document).find('#divLoadingPatChart').css('display', "none");
                        //    to expand all nodes and sub nodes automatically on clicking patientchart
                        $("#dvCheck").find("li").removeClass("collapsed");
                        $("#dvCheck li").addClass("collapsed")
                        $("#liEncounters").removeClass("collapsed");
                    },
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
                iCount++;
            }
            else {
                jQuery(top.window.parent.document).find('#divLoadingPatChart').css('display', "none");
                $("#divLoading").css('display', "none");
            }

            $('#dvCheck')[0].style.display = "block";
            $('#lblPatientStrip')[0].textContent = "";
            $('#lblPatientStrip')[0].style.display = "none";

        }

        else {
           
            $("#dvCheck").find("li").removeClass("collapsed");
        }
    }

}
function ActivityHistoryClick(Human_Id, PatientName) {
    var Encounter_ID = document.getElementById("hdnEncounterId").value;
    $.ajax({
        type: "POST",
        url: "webfrmPatientPortal.aspx/GetTextboxValues",
        data: '{FieldValues: "' + Human_Id + "," + Encounter_ID + "," + PatientName + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
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
    return false;

}

function OnSuccess(response) {


    var RadDownload = $find('RadWindow2');
    RadDownload.show();
    RadDownload._resizeExtender.set_hideIframes(false);
    var TextBox = document.getElementById("RadWindow2_C_txtActivityLog");
    TextBox.value = response.d;


}

function ChangePasswordClick() {
    var obj = new Array();
    obj.push("ScreenMode=Patient Portal");
    obj.push("PatientID=" + document.getElementById("HumanID").value);
    obj.push("EmailID=" + document.getElementById("lblEmailIDActual").innerText);
    openModal("frmChangePassword.aspx", 250, 550, obj, "MessageWindow");
    return false;
}


function PatientAccountClick(human_ID, PatientName, PatientEMail) {
    var Role = document.getElementById("hdnRole").value;
    if (Role == "Representative") {
        var obj = new Array();
        obj.push("ScreenMode=PatientQuarantorAccount");
        obj.push("PatientID=" + human_ID);
        obj.push("PatientName=" + PatientName);
        obj.push("EmailID=" + PatientEMail);
        var Result = openModal("frmForgotPassword.aspx", 430, 525, obj, "MessageWindow");
        return false;
    }
    else {
        var obj = new Array();
        obj.push("ScreenMode=PatientAccount");
        obj.push("PatientID=" + human_ID);
        obj.push("PatientName=" + PatientName);
        obj.push("EmailID=" + PatientEMail);
        var Result = openModal("frmForgotPassword.aspx", 430, 525, obj, "MessageWindow");
        return false;
    }
}

function OpenForgorpassword() {
    var obj = new Array();
    obj.push("ScreenMode=PatientPortalLogin");
    openModal("frmForgotPassword.aspx", 410, 410, obj, "MessageWindow");
    return false;
}

function LoginChangePassword(IDandEmail) {

    var obj = new Array();
    obj.push("ScreenMode=Forgot Password");
    obj.push("PatientID=" + IDandEmail.split(',')[0]);
    obj.push("EmailID=" + IDandEmail.split(',')[1]);
    GetRadWindowPortal().BrowserWindow.openModal("frmChangePassword.aspx", 250, 550, obj, "RadWindow1");
    return false;
}
function GetRadWindowPortal() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement) {
        if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    }
    return oWindow;
}

function CloseSendMessage() {
    self.close();

}

function SendMessage(sFileName) {

    if (sFileName == "") {
        alert('Please Click on Preview');
        return false;
    }
    var Role = document.getElementById("hdnRole").value;
    var obj = new Array();
    obj.push("FileName=" + sFileName.split(',')[0]);
    obj.push("Encounter_ID=" + sFileName.split(',')[1]);
    obj.push("LoginEmailID=" + sFileName.split(',')[3]);
    obj.push("Role=" + Role);
    openModal("frmSendHealthRecord.aspx", 460, 700, obj, "MessageWindow");
    return false;

}
function SendMail(Humanid) {
    var Role = document.getElementById("hdnRole").value;
    var obj = new Array();
    obj.push("PatientID=" + Humanid.split(',')[0]);
    obj.push("EmailID=" + Humanid.split(',')[1]);
    obj.push("EncounterID=" + Humanid.split(',')[2]);
    obj.push("Role=" + Role);
    //openModal("frmMailBox.aspx", 490, 650, obj, "SendMailWindow");
    openModal("frmMailBox.aspx", 600, 686, obj, "SendMailWindow");

    return false;
}

function btnCancel_Clicked(sender, args) {
    self.close();
}

function EnableSave() {
    $find('btnSend').set_enabled(true);
}


function btnClearAll_Clicked(sender, args) {
    var ClearAll = null;
    var btnName = document.getElementById("btnClearAll").value;
    if (btnName == "Clear All") {
        ClearAll = DisplayErrorMessage('295002');
    }
   
    if (ClearAll == true && btnName == "Clear All") {
        $find("cboSecurityQuestion1").clearSelection();
        $find("cboSecurityQuestion2").clearSelection();
        $find("txtAnswer1").clear();
        $find("txtAnswer2").clear();
        document.getElementById("btnSave").disabled = true;
    }
    if (btnName == "Cancel") {
        var Enabled = document.getElementById("btnSave").disabled;
        if (Enabled == false) {
            if (DisplayErrorMessage('295009')) {
                CancelForgotPassword();
            }
        }
        else {
            CancelForgotPassword();
        }
    }
  
}
function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB'];
    if (bytes == 0) return '0';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2);
}

function OnFileUploadFailed(sender, args) {
    DecrementUploadsInProgress();
}
function OnFileSelected(sender, args) {

    for (var fileindex in sender._uploadedFiles) {
        if (sender._uploadedFiles[fileindex].fileInfo.FileName == args.get_fileName()) {
            isDuplicateFile = true;
        }
    }

    uploadsInProgress++;
}

function DecrementUploadsInProgress() {
    uploadsInProgress--;

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
function OnFileRemove(sender, args) {
    document.getElementById("lblerrormessage").innerHTML = "";
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
function OnFileUploaded(sender, args) {


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

function CancelForgotPassword() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement != null)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null) {
        oWindow.close();
    }
    else {
        var Result = document.getElementById("hdnHumanID").value;
        var Email = document.getElementById("hdnEmailID").value;
        if (Result != "") {
            window.location = "webfrmLogin.aspx?PatientID=" + Result + "&Email=" + Email;
        }
        else {
            window.location = "frmLogin.aspx";
        }
    }
}
function saveEnabled() {
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    $find('btnSend').set_enabled(true);
}



function sendvalidation(sender, args) {
    document.getElementById('hdnType').value = "";
    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    $find('btnSend').set_enabled(true);
  
    
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var regdirect = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z])+\.([A-Za-z]{2,4})$/i;
    var regorg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z0-9])+\.([A-Za-z0-9])+\.([A-Za-z]{2,4})$/i;
    if (regdirect.test(document.getElementById('cboFrom').value) == false)
        document.getElementById('hdnMailtype').value = 'N';
    else
        document.getElementById('hdnMailtype').value = 'Y';
    if ((document.getElementById('txtSubject').value) == "") {
        alert('Please Enter Subject');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else if ((document.getElementById('txtMessage').value) == "") {
        alert('Please enter Message');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById('rdbtnSentTo').checked) {
        if (document.getElementById('txtSentTo').value == "") {
            alert("Please enter mail address to send")
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }

        else if (regdirect.test(document.getElementById('cboFrom').value) == true && regdirect.test(document.getElementById('txtSentTo').value) == false) {
            alert('Please enter valid  Direct Email Address');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }

        else if (regdirect.test(document.getElementById('cboFrom').value) == false && regdirect.test(document.getElementById('txtSentTo').value) == true) {
            alert('Please enter valid Email Address');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }

        else if (regdirect.test(document.getElementById('cboFrom').value) == false && reg.test(document.getElementById('txtSentTo').value) == false) {
            alert('Please enter valid Email Address');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }
        else {
            sender.set_autoPostBack(true);
        }
    }
    else if (!document.getElementById('rdbtnSentTo').checked) {
        if (document.getElementById('cboProvider').value == "") {
            alert('Please Select Provider to Send a Mail');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }
        else if (regdirect.test(document.getElementById('cboFrom').value) == false && regdirect.test(document.getElementById('cboProvider').value.split(' - ')[1]) == true)
        {
            alert('Please Select  Valid Mail ID ');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }
        else if (regdirect.test(document.getElementById('cboFrom').value) == true && regdirect.test(document.getElementById('cboProvider').value.split(' - ')[1]) == false) {
            alert('Please  Select valid Direct Email Address ');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }
        else {
            sender.set_autoPostBack(true);
        }
    }
    else if (document.getElementById('cboProvider').value == "Others") {
        if (document.getElementById('txtDirectAddress').value == "") {
            alert("Please enter mail address to send")
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }
        else if (regdirect.test(document.getElementById('txtDirectAddress').value) == false &&
            regorg.test(document.getElementById('txtDirectAddress').value) == false && regdirect.test(document.getElementById('cboFrom').value) == true) {
            alert('Please enter valid Direct Email Address');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }

        else if (regdirect.test(document.getElementById('txtDirectAddress').value) == true &&
            regdirect.test(document.getElementById('cboFrom').value) == false) {
            alert('Please enter valid  Email Address');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }

        else if (regdirect.test(document.getElementById('cboFrom').value) == false && reg.test(document.getElementById('txtDirectAddress').value) == false) {
            alert('Please enter valid Email Address');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            sender.set_autoPostBack(false);
        }
        else {
            sender.set_autoPostBack(true);
        }
       
    }

    else {
        sender.set_autoPostBack(true);
    }
}


function LoginClick() {
    setTimeZone();
}

function setTimeZone() {
    var hiddenObj = document.getElementById('hdnLocalTime');
    hiddenObj.value = showTime().toString();


}
function showTime() {
    var dt = new Date();
    var LocalTime = dt.getTimezoneOffset();
    var LocalDate = dt.toLocaleDateString("en-US");

    if (LocalDate.indexOf("/") != -1) {
        var LocalDatenew = LocalDate.split('/');
        var day = ("0" + (LocalDatenew[1])).slice(-2);
        LocalDate = LocalDatenew[0] + "/" + day + "/" + LocalDatenew[2];
    }
    if (LocalDate.indexOf("-") != -1) {
        var LocalDatenew = LocalDate.split('-');
        var day = ("0" + (LocalDatenew[1])).slice(-2);
        LocalDate = LocalDatenew[0] + "/" + day + "/" + LocalDatenew[2];
    }
    document.getElementById('hdnUniversaloffset').value = createOffset(dt);
    var dt1 = new Date(); var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalDateAndTime").value = dt1.toLocaleTimeString();

    document.getElementById('hdnLocalDate').value = LocalDate;
    return LocalTime;

}


function pad(value) {
    return value < 10 ? '0' + value : value;
}
function createOffset(date) {
    var sign = (date.getTimezoneOffset() > 0) ? "-" : "+";
    var offset = Math.abs(date.getTimezoneOffset());
    var hours = pad(Math.floor(offset / 60));
    var minutes = pad(offset % 60);
    return sign + hours + "." + minutes;
}

function ChangePwdValidation(sender, args) {
    var NewPwd = document.getElementById("txtNewPassword").value;
    var CPwd = document.getElementById("txtConfirmPassword").value;
    var ScreenMode = document.getElementById("hdnScreenMode").value;
    if (ScreenMode.toUpperCase() == "CHANGE PASSWORD USER" || ScreenMode.toUpperCase() == "CHANGE PASSWORD PATIENT" || ScreenMode.toUpperCase() == "CHANGE PASSWORD QUARANTOR" || ScreenMode.toUpperCase() == "PATIENT PORTAL" && ScreenMode.toUpperCase() != "") {
        var txtEnable = document.getElementById("txtOldPassword").disabled;
        if (document.getElementById("txtOldPassword").value.length == 0 && txtEnable != true) {
            DisplayErrorMessage('050003');
            document.getElementById("txtOldPassword").focus();
            sender.set_autoPostBack(false);
        }
        if (document.getElementById("txtOldPassword").value == document.getElementById("txtNewPassword").value) {
            DisplayErrorMessage('780013');
            document.getElementById("txtNewPassword").focus();
            sender.set_autoPostBack(false);
        }
    }
    if (document.getElementById("txtNewPassword").value.length == 0) {
        DisplayErrorMessage('780010');
        document.getElementById("txtNewPassword").focus();
        sender.set_autoPostBack(false);
    }
    if (document.getElementById("txtNewPassword").value != "") {
        var regex = /^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{0,12}$/;
        if ((regex.test(document.getElementById("txtNewPassword").value)) == false) {
            DisplayErrorMessage('010019');
            document.getElementById('txtNewPassword').focus();
            sender.set_autoPostBack(false);
        }
    }
    if (document.getElementById("txtConfirmPassword").value.length == 0) {
        DisplayErrorMessage('780011');
        document.getElementById("txtConfirmPassword").focus();
        sender.set_autoPostBack(false);
    }
    if (NewPwd != CPwd) {
        DisplayErrorMessage('780012');
        document.getElementById("txtConfirmPassword").focus();
        sender.set_autoPostBack(false);
    }

}
function PatientPortalLoginValid(sender, args) {
    if (document.getElementById("UserName").value.length == 0) {
        DisplayErrorMessage('420056');
        document.getElementById("UserName").focus();
        sender.set_autoPostBack(false);
    }
    if (document.getElementById("UserName").value.length != 0 && IsEmail(document.getElementById(GetClientId("UserName")).value) == false) {
        DisplayErrorMessage('420030');
        document.getElementById("UserName").focus();
        sender.set_autoPostBack(false);
    }
    if (document.getElementById("Password").value.length == 0) {
        DisplayErrorMessage('596003');
        document.getElementById("Password").focus();
        sender.set_autoPostBack(false);
    }

    setTimeZone();
}
function IsEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);

}
function ForgotPwdValidation(sender, args) {
    var SQuestion1 = document.getElementById("cboSecurityQuestion1").value;
    var SQuestion2 = document.getElementById("cboSecurityQuestion2").value;
    if (document.getElementById("cboSecurityQuestion1").value.length == 0) {
        DisplayErrorMessage('780008');
        sender.set_autoPostBack(false);

    }
    else if (document.getElementById("txtAnswer1").value.trim().length == 0) {
        DisplayErrorMessage('780005');
        document.getElementById("txtAnswer1").focus();
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("cboSecurityQuestion2").value.length == 0) {
        DisplayErrorMessage('780009');
        sender.set_autoPostBack(false);
    }
    else if (SQuestion1 == SQuestion2) {
        DisplayErrorMessage('780007');
        sender.set_autoPostBack(false);
    }
    else if (document.getElementById("txtAnswer2").value.trim().length == 0) {
        DisplayErrorMessage('780006');
        document.getElementById("txtAnswer2").focus();
        sender.set_autoPostBack(false);
    }
}
function OpenLogin(HumanID, EMailID) {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement != null)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null) {
        ShowErrorMessageList('010011');
        setTimeout(function () {
            self.close();
        }, 1800);
        //for bug id 51268
        if (HumanID != "undefined" || HumanID != "" || HumanID != null) {
            var URL = "WebfrmLogin.aspx?PatientID=" + HumanID + "&Email=" + EMailID;
            if (window.parent != null)
                window.parent.location = URL;
            else
                window.location = URL;
        }
    }
    else {
        ShowErrorMessageList('010011');
        if (HumanID == "undefined" || HumanID == "" || HumanID == null) {
            window.location = 'frmLogin.aspx';
        }
        else {
            var URL = "WebfrmLogin.aspx?PatientID=" + HumanID + "&Email=" + EMailID;
            window.location = URL;
        }
    }

}
function OpenPatientPortalPage(HumanID, EmailID, Role) {
    var ContentURL = "";
    ContentURL = "webfrmPatientPortal.aspx?PatientID=" + HumanID + "&EmailID=" + EmailID + "&Role=" + Role;
    window.location = ContentURL;

    return false;
}
function OpenModalWindowToRegister(PatientName, EmailID, HumanID, Role) {
    var ContentURL = "";
    if (Role == "PatientRegister") {
        ContentURL = "frmForgotPassword.aspx?ScreenMode=PatientRegister" + "&PatientName=" + PatientName + "&EmailID=" + EmailID + "&PatientID=" + HumanID;
    }
    else if (Role == "GuarantorRegister") {
        ContentURL = "frmForgotPassword.aspx?ScreenMode=GuarantorRegister" + "&PatientName=" + PatientName + "&EmailID=" + EmailID + "&PatientID=" + HumanID;
    }
    else if (Role == "PatientPortal") {
        ContentURL = "frmForgotPassword.aspx?ScreenMode=PatientPortal" + "&PatientName=" + PatientName + "&EmailID=" + EmailID + "&PatientID=" + HumanID;
    }
    else if (Role == "GuarantorPortal") {
        ContentURL = "frmForgotPassword.aspx?ScreenMode=GuarantorPortal" + "&PatientName=" + PatientName + "&EmailID=" + EmailID + "&PatientID=" + HumanID;
    }
    if (ContentURL != "") {
        var oWnd = GetRadWindowPortal();
        if (oWnd == null) {
            oWnd = $find('MessageWindow');
        }
        if (oWnd != null) {
            var oManager = oWnd.get_windowManager(); var oManager = oWnd.get_windowManager();
            var childWindow = oManager.BrowserWindow.radopen(ContentURL, "MessageWindow"); var childWindow = oManager.BrowserWindow.radopen(ContentURL, "MessageWindow");
            SetRadWindowProperties(childWindow, 500, 600); SetRadWindowProperties(childWindow, 500, 600);
        }
        else {
            window.location = ContentURL;
        }
    }
    return false;
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
function ChangePswdPatientPortal(EmailID, HumanID, Role) {
    var ContentURL = "";
    if (Role == "CHANGE PASSWORD QUARANTOR") {
        ContentURL = "frmChangePassword.aspx?ScreenMode=CHANGE PASSWORD QUARANTOR" + "&EmailID=" + EmailID + "&IsLoginOpen=YES" + "&PatientID=" + HumanID;
    }
    else if (Role == "FORGOT PASSWORD PATIENT") {
        ContentURL = "frmChangePassword.aspx?ScreenMode=FORGOT PASSWORD PATIENT" + "&EmailID=" + EmailID + "&IsLoginOpen=YES" + "&PatientID=" + HumanID;
    }
    else if (Role == "FORGOT PASSWORD QUARANTOR") {

        ContentURL = "frmChangePassword.aspx?ScreenMode=FORGOT PASSWORD QUARANTOR" + "&EmailID=" + EmailID + "&PatientID=" + HumanID + "&IsLoginOpen=YES";
    }
    if (ContentURL != "") {
        setTimeout(
           function () {
               ShowErrorMessageList('295001');
               var oWnd = GetRadWindowPortal();
               if (oWnd == null) {
                   oWnd = $find('MessageWindow');
               }
               var oManager = oWnd.get_windowManager();
               var childWindow = oManager.BrowserWindow.radopen(ContentURL, "MessageWindow");
               SetRadWindowProperties(childWindow, 210, 600);
           }, 0);
    }
    return false;
}




function OpenCPassword(HumanID, EMailID, IsLogin, UserName) {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement != null)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null) {
        ShowErrorMessageList('295001');
    }
    else {
        ShowErrorMessageList('295001');
        var ScreenMode = document.getElementById("hdnScreenMode").value;
        if (UserName != "" && UserName != null) {
            window.location = "frmChangePassword.aspx?ScreenMode=" + ScreenMode + "&UserName=" + UserName + "&IsLoginOpen=" + IsLogin;
        }
        else {
            window.location = "frmChangePassword.aspx?ScreenMode=" + ScreenMode + "&EmailID=" + EMailID + "&PatientID=" + HumanID + "&IsLoginOpen=" + IsLogin;
        }
    }
}

function OpenCPasswordPatientRegister(HumanID, EMailID, IsLogin, sScreenMode) {
    ShowErrorMessageList('295001');
    var ScreenMode = document.getElementById("hdnScreenMode").value;
    var ContentURL = "frmChangePassword.aspx?ScreenMode=" + ScreenMode + "&EmailID=" + EMailID + "&PatientID=" + HumanID + "&IsLoginOpen=" + IsLogin;
    if (ContentURL != "") {
        setTimeout(
           function () {
               var oWnd = GetRadWindowPortal();
               if (oWnd == null) {
                   oWnd = $find('MessageWindow');
               }
               var oManager = oWnd.get_windowManager();
               var childWindow = oManager.BrowserWindow.radopen(ContentURL, "MessageWindow");
               SetRadWindowProperties(childWindow, 210, 600);
           }, 0);
    }
}

function ChangePwdClose(HumanID, EmailID) {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement != null)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null) {
        oWindow.close();
    }
    else {
        window.location = "webfrmLogin.aspx?PatientID=" + HumanID + "&Email=" + EmailID; //commented by naveena for bug_id 24950
    }
}
function LoginConfirmation() {
    PageMethods.Confirmation();
}
function SendDocument() {
    var RadSendDocument = $find('SendRecordWindow');
    RadSendDocument.show();

    var now = new Date();
    var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    document.getElementById("SendRecordWindow_C_btnSendDocument").disabled = true;
    document.getElementById('SendRecordWindow_C_rbtSendRecordPDF').checked = false;
    document.getElementById('SendRecordWindow_C_rbtSendRecordXML').checked = false;
    document.getElementById('SendRecordWindow_C_rbtSendRecordBoth').checked = false;
    return false;
}
function SpacebarValidation(t) {
    if (window.event.keyCode == 32) {
        window.event.preventDefault();
        alert('Space is not allowed');
        return false;
    }
}
function EnableForgotPwd() {
    document.getElementById("btnSave").disabled = false;
}
function CloseUserCPwd() {
    window.location = 'frmLogin.aspx';
}
function getEncounter() {
    var Dropdown = document.getElementById("cboEncounter");
    var EncounterID = document.getElementById("cboEncounter").options[Dropdown.selectedIndex].value;
    document.getElementById("hdnEncounterId").value = EncounterID;
}

function ToRetainValues() {
    if (document.getElementById("txtOldPassword") != null) {
        document.getElementById("txtOldPassword").value = document.getElementById("hdnOld").value;
    }
    document.getElementById("txtNewPassword").value = document.getElementById("hdnNew").value;
    document.getElementById("txtConfirmPassword").value = document.getElementById("hdnConfirm").value;
}
function tochangeValues() {
    if (document.getElementById("txtOldPassword") != null) {
        document.getElementById("hdnOld").value = document.getElementById("txtOldPassword").value;
    }
    document.getElementById("hdnNew").value = document.getElementById("txtNewPassword").value;
    document.getElementById("hdnConfirm").value = document.getElementById("txtConfirmPassword").value;
}

function deleting() {
    if (document.getElementById("txtOldPassword") != null) {
        document.getElementById("hdnOld").value = document.getElementById("txtOldPassword").value;
    }
    document.getElementById("hdnNew").value = document.getElementById("txtNewPassword").value;
    document.getElementById("hdnConfirm").value = document.getElementById("txtConfirmPassword").value;
}
function changeValues() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById("hdnToFindPostback").value == "true") {
        if (document.getElementById("txtOldPassword") != null) {
            document.getElementById("txtOldPassword").value = document.getElementById("hdnOld").value;
        }
        document.getElementById("txtNewPassword").value = document.getElementById("hdnNew").value;
        document.getElementById("txtConfirmPassword").value = document.getElementById("hdnConfirm").value;
    }
    else {
        if (document.getElementById("txtOldPassword") != null) {
            document.getElementById("hdnOld").value = document.getElementById("txtOldPassword").value;
        }
        document.getElementById("hdnNew").value = document.getElementById("txtNewPassword").value;
        document.getElementById("hdnConfirm").value = document.getElementById("txtConfirmPassword").value;
    }
}

$("body").keypress(function (event) {
    if (event.which == 13) {
        if (document.getElementById("LoginButton") != null && document.getElementById("LoginButton") != undefined)
            document.getElementById("LoginButton").click();
    }
});

function EnableBtnDownload() {
    document.getElementById("RadWindow1_C_Button1").disabled = false;
}

function EnableBtnSend() {
    document.getElementById("SendRecordWindow_C_btnSendDocument").disabled = false;
}
//BugID:49297
function openBulkAccess() {
    var obj = new Array();
    openModal("webfrmBulkAccess.aspx", 695, 1120, obj, "SendMailWindow");
    var WindowName = $find('SendMailWindow');
    WindowName.add_close(CloseAll);
    return false;
}


function CloseAll(oWindow, args) {
    oWindow.close();
    $('#EncounterContainer')[0].src = "";
    document.getElementById("btnDownload").disabled = true;
    document.getElementById("btnSend").disabled = true;
    document.getElementById("btndelete").disabled = true;
}

function OnSendClose() {
    self.close();
}
