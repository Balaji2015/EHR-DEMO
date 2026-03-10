function GetObjects() {
    document.getElementById("divLoading").style.display = "block";
}
function cboObjectTypeUpdateProcess() {
    var SelectItem = document.getElementById("cboFirstObjType");
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}

function cboObjectTypeUpdateOwner() {
    var SelectItem = document.getElementById("cboFirstObjType");

    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}
function WindowClose() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
}
function Clear() {
    var IsClearAll = DisplayErrorMessage('200005');
    if (IsClearAll == true) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        ClearAll();
        $("#divWfObject").empty();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return true;
    }
    return false;
}

function ClearAll() {
    document.getElementById("txtReasonForChange").value = "";
    $("#cboPreviousProcess").empty();
    $("#cboUpdateOwner").empty();
    document.getElementById("checkkShowAllOwner").checked = false;
    document.getElementById("rbUpdateOwner").checked = false;
    document.getElementById("rbUpdateProcess").checked = false;
    rbPanel.Disabled();
}

function cboUpdateCurrentProcess() {
    var SelectItem = document.getElementById("cboFirstCurrentProcess");
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}

function cboOwnerCurrentProcess() {
    var SelectItem = document.getElementById("cboCurrentProcess");
    if (SelectItem != null && SelectItem != " ") {
        document.getElementById("divLoading").style.display = "block";
        return true;
    }
    else {
        return false;
    }
}


function Checking(args) {

    var rdprocess = document.getElementById("rbUpdateProcess");
    var rdOwner = document.getElementById("rbUpdateOwner");
    if (rdprocess.checked) {
        var controls = document.getElementById("<%=pnlUpdateProcess.ClientID%>").getElementsByTagName("input");
        for (var i = 0; i < controls.length; i++)
            controls[i].disabled = false;
        var controls1 = document.getElementById("<%=pnlUpdateOwner.ClientID%>").getElementsByTagName("input");
        for (var i = 0; i < controls1.length; i++)
            controls1[i].disabled = true;
    }
}
 
function CheckingOwner(args) {

    var rdprocess = document.getElementById("rbUpdateProcess");
    var rdOwner = document.getElementById("rbUpdateOwner");
    if (rdOwner.checked) {
        var controls = document.getElementById("<%=pnlUpdateOwner.ClientID%>").getElementsByTagName("input");
        for (var i = 0; i < controls.length; i++)
            controls[i].disabled = false;
        var controls1 = document.getElementById("<%=pnlUpdateProcess.ClientID%>").getElementsByTagName("input");

        for (var i = 0; i < controls1.length; i++)
            controls1[i].disabled = true;
    }
}


function btnClose_Clicked()
{
    if (document.getElementById("btnUpdateProcess").disabled == false) {
         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
        if (!$($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').is('#dvdialogMenu'))
            $($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
                '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save them?</p></div>');
        dvdialog = $($(top.window.document).find("iframe[name='ctl00_ModalWindow']")[0].contentDocument).find('body').find('#dvdialogMenu');
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
                    
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
                    $(dvdialog).dialog("close");
                    sessionStorage.setItem("AutoSave_OrderMenu", "true");
                    if (document.getElementById("txtReasonForChange").value=="")
                    {
                        DisplayErrorMessage('700003');
                         {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
                        return false;
                    }
                    else {
                        document.getElementById("hdnMessageType").value = "Yes";
                        $('#btnUpdateProcess').trigger('click');
                    }
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
        self.close();
    }

   
}
function loadwfobject() {
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}
function btnGetobjectsClick()
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    FillGrid();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}

function btnUpdateOwnerClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    let ssSelectedRow = GetSelectedRow();
    if (ssSelectedRow.length === 0) {
        DisplayErrorMessage('700001');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }

    if (document.getElementById("cboUpdateOwner").value === "") {
        DisplayErrorMessage('700011');
        document.getElementById("cboUpdateOwner").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }
    let sUpdateOwner = [document.getElementById("cboUpdateOwner").options[document.getElementById("cboUpdateOwner").selectedIndex]?.text ?? "", document.getElementById("cboUpdateOwner").value];
    if (sUpdateOwner.length === 0) {
        DisplayErrorMessage('700011');
        document.getElementById("cboUpdateOwner").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }
    var WSData = JSON.stringify({ sSelectedRow: ssSelectedRow, sCboUpdateOwner: sUpdateOwner, stxtReasonForChange: document.getElementById("txtReasonForChange").value });
    $.ajax({
        type: "POST",
        url: '/frmWFObjectManager.aspx/btnUpdateOwner_Click',
        data: WSData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var cbodata = data.d;
            if (cbodata.indexOf("DisplayErrorMessage") > -1) {
                DisplayErrorMessage(cbodata.split("-")[1]);
            }
            if (cbodata.indexOf("return") > -1) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return;
            }
            FillGrid();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                //CAP-792
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
function btnUpdateProcessClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    let ssSelectedRow = GetSelectedRow();
    if (ssSelectedRow.length === 0) {
        DisplayErrorMessage('700001');
        NotSaved();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }

    if (document.getElementById("txtReasonForChange").value == "") {
        DisplayErrorMessage('700003');
        NotSaved();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("txtReasonForChange").focus();
        return;
    }

    var cbopreviousprocess = document.getElementById("cboPreviousProcess").options[document.getElementById("cboPreviousProcess").selectedIndex]?.text ?? "";
    if (cbopreviousprocess == "START" || cbopreviousprocess == "") {
        DisplayErrorMessage('700007');
        NotSaved();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        document.getElementById("cboPreviousProcess").focus();
        return;
    }


    var WSData = JSON.stringify({
        sSelectedRow: ssSelectedRow,
        shdnMessageType: document.getElementById("hdnMessageType").value,
        stxtReasonForChange: document.getElementById("txtReasonForChange").value,
        scboPreviousProcess: document.getElementById("cboPreviousProcess").options[document.getElementById("cboPreviousProcess").selectedIndex]?.text ?? ""
    });
    document.getElementById("hdnMessageType").value = "";

    $.ajax({
        type: "POST",
        url: '/frmWFObjectManager.aspx/btnUpdateProcess_Click',
        data: WSData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var cbodata = data.d;
            if (cbodata.indexOf("DisplayErrorMessage") > -1) {
                DisplayErrorMessage(cbodata.split("-")[1]);
                if ((cbodata.indexOf("NotSaved") > -1)) {
                    NotSaved();
                }
                if ((cbodata.indexOf("SaveSuccess") > -1)) {
                    SaveSuccess();
                }
                if ((cbodata.indexOf("ClearAll") > -1)) {
                    ClearAll();
                }
                if ((cbodata.indexOf("return") > -1)) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return;
                }
            }
            FillGrid();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                //CAP-792
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

function SaveSuccess() {
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
    self.close();
}
function NotSaved() {
    document.getElementById("btnUpdateProcess").disabled == false;
     {sessionStorage.setItem('StartLoading', 'false');StopLoadFromPatChart();}
}
function grdAdminModule_OnCommand(sender, args)
{
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();}
    return true;
}

function rbUpdateProcessOnClick()
{
    if (document.getElementById("txtReasonForChange").disabled == true)
    {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        rbUpdateProcess_CheckedChanged();
    }
}
function rbUpdateOwnerOnClick()
{
    if (document.getElementById("btnUpdateOwner").disabled == true)
    {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        rbUpdateOwner_CheckedChanged();
    }
}
 //Jira #CAP-195
function chkShowAllOwner() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    rbUpdateOwner_CheckedChanged();
}
class rbPanel {
    static EnableUpdateProcess() {
        
        document.getElementById("txtReasonForChange").disabled = false;
        document.getElementById("cboPreviousProcess").disabled = false;
        document.getElementById("btnUpdateProcess").disabled = false;
    }
    static DisabledUpdateProcess() {
        
        document.getElementById("txtReasonForChange").disabled = true;
        document.getElementById("cboPreviousProcess").disabled = true;
        document.getElementById("btnUpdateProcess").disabled = true;
    }
    static EnableUpdateOwner() {
        
        document.getElementById("cboUpdateOwner").disabled = false;
        document.getElementById("checkkShowAllOwner").disabled = false;
        document.getElementById("btnUpdateOwner").disabled = false;
    }
    static DisabledUpdateOwner() {
        
        document.getElementById("cboUpdateOwner").disabled = true;
        document.getElementById("checkkShowAllOwner").disabled = true;
        document.getElementById("btnUpdateOwner").disabled = true;
    }

    static Enable()
    {
        document.getElementById("rbUpdateProcess").disabled = false;
        document.getElementById("rbUpdateOwner").disabled = false;
        rbPanel.EnableUpdateProcess();
        rbPanel.EnableUpdateOwner();
    }
    static Disabled() {
        document.getElementById("rbUpdateProcess").disabled = true;
        document.getElementById("rbUpdateOwner").disabled = true;
        rbPanel.DisabledUpdateProcess();
        rbPanel.DisabledUpdateOwner();
    }
}




function FillGrid() {
    document.getElementById("txtReasonForChange").value = "";
    $("#cboPreviousProcess").children().remove();
    $("#cboUpdateOwner").children().remove();
    rbPanel.Disabled();
    $('#divWfObject').empty();
    $("#divWfObject").append(`
    <table id="grdAdminModuleWfobjwct" class='table table-bordered Gridbodystyle' style='' runat='server'>
    <thead class='header' style='border: 0px;'>
    <tr class='header' >
    <th style='border: 1px solid #909090;text-align: center;width:7%;'>Encounter ID</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%;'>Date of Service</th>
    <th style='display:none;'>Patient Acc#</th>
    <th style='display:none;'>Patient Name</th>
    <th style='display:none;'>Patient DOB</th>
    <th style='border: 1px solid #909090;text-align: center;width:10%;'>Appointment Provider Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%'>Encounter Provider Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:11%;'>Current Process</th>
    <th style='border: 1px solid #909090;text-align: center;width:6%;'>Current Owner</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%;'>Appointment Date</th>
    <th style='border: 1px solid #909090;text-align: center;width:10%;'>Object Type</th>
    <th style='border: 1px solid #909090;text-align: center;width:7%;'>MA Name</th>
    <th style='border: 1px solid #909090;text-align: center;width:5%;'>Batch Status</th>
    <th style='border: 1px solid #909090;text-align: center;width:9%;'>Facility Name</th>
    </tr>
    </thead>
</table>`);
    data = JSON.stringify({ "PatientName": document.getElementById("txtPatientName").value, "DOB": document.getElementById("txtDOB").value, "AccountNo": document.getElementById("txtAccountNo").value });
    var dataTable = new DataTable('#grdAdminModuleWfobjwct', {
        serverSide: false,
        lengthChange: false,
        searching: true,
        processing: false,
        scrollCollapse: true,
        scrollY: '177px',
        ordering: true,
        autowidth: false,
        order: [],
        pageLength: 15,
        language: {
            search: "",
            searchPlaceholder: "Appointment Date, Encounter Provider Name or Current Process",
            infoFiltered: ""
        },
        dom: '<"top"ipf>rt<"bottom"l><"clear">', // Counter (i) and Pagination (p) at the top

        ajax: {
            url: '/frmWFObjectManager.aspx/LoadGrid',
            contentType: "application/json",
            type: "GET",
            dataType: "JSON",
            deferRender: true,
            data: function (d) {
                d.extra_search = data;
                return d;
            },
            dataSrc: function (json) {
                //var objdata = json.d;
                var objdata = json.d;
                objdata.data = Decompress(objdata.data);
                if (objdata.data.indexOf("DisplayErrorMessage") > -1) {
                    DisplayErrorMessage(objdata.data.split("-")[1]);
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return;
                }
                json.data = objdata.data;
                return json.data;
            },
            error: function (xhr, error, code) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (xhr.status == 999)
                    window.location = "frmSessionExpired.aspx";
                else {
                    var log = JSON.parse(xhr.responseText);
                    console.log(log);
                    //alert("USER MESSAGE:\n" +
                    //    ". Cannot process request. Please Login again and retry. \nEXCEPTION DETAILS: \n" +
                    //    "Message: " + log.Message);
                    ScriptErrorLogEntry(log.Message, "", "", document.URL, log.StackTrace, true);
                }
            }
        },
        columns: [
            { data: 'Encounter_id', searchable: false, switch: '7%', sClass: "word-break" },
            {
                data: 'Date_of_service', searchable: false, sClass: "word-break", type: 'date', switch: '9%', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    dt1 = ConvertDate(dt1.replaceAll("T", " "));
                    //var dt2 = dt1.split(' ');
                    //if (dt2.length > 0) {
                    //    if (dt2.indexOf("01-01-0001") > -1) {
                    //        return "";
                    //    }
                    //}
                    return dt1;
                }
            },
            { data: 'Human_ID', searchable: false, sClass: "dataTables_empty" },
            { data: 'PatientName', searchable: false, sClass: "dataTables_empty" },
            {
                data: 'PatientDOB', searchable: false, sClass: "dataTables_empty", type: 'date', render: function (data, type, row) {
                    return DOBConvert(data.replace("T00:00:00", ""))
                }
            },
            { data: 'Appointment_Provider_Name', searchable: false, sWidth: '10%', sClass: "word-break" },
            { data: 'Encounter_Provider_Name', searchable: true, sClass: "word-break", sWidth: '11%' },
            { data: 'Current_Process', searchable: true, sClass: "word-break", sWidth: '11%' },
            { data: 'Current_Owner', searchable: false, sClass: "word-break", sWidth: '6%' },
            {
                data: 'Appointment_Date', searchable: true, sClass: "word-break", sWidth: '9%', type: 'date', render: function (data, type, row) {
                    var dt1 = data.replaceAll("/", "").replaceAll("Date(", "").replaceAll(")", "");
                    dt1 = ConvertDate(dt1.replaceAll("T", " "));
                    //var dt2 = dt1.split(' ');
                    //if (dt2.length > 0) {
                    //    if (dt2.indexOf("01-01-0001") > -1) {
                    //        return "";
                    //    }
                    //}
                    return dt1;
                }
            },
            { data: 'Obj_Type', searchable: false, sClass: "word-break", sWidth: '10%' },
            { data: 'Assigned_Med_Asst_User_Name', searchable: false, sClass: "word-break", sWidth: '7%' },
            { data: 'Batch_Status', searchable: false, sClass: "word-break", sWidth: '5%' },
            { data: 'Facility_Name', searchable: false, sClass: "word-break", sWidth: '9%' },
        ],
        initComplete: function (settings, json) {
            $("#grdAdminModuleWfobjwct_filter input")[0].classList.add('searchicon');
        }

    });
    $('#grdAdminModuleWfobjwct_filter').css({
        'float': 'left',
        'text-align': 'left',
        'margin-left': '30px',
    });

    $('#grdAdminModuleWfobjwct_info').css({
        'min-width': '180px'
    });

    dataTable.on('page.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        rbPanel.Disabled();
    });
    dataTable.on('search.dt', function () {
        dataTable.$('tr.highlight').removeClass('highlight');
        rbPanel.Disabled();
    });

    $('#grdAdminModuleWfobjwct tbody').on('click', 'tr', function () {
        $(this).parent().children().removeClass("highlight")
        $(this)[0].classList.add('highlight');
        grdAdminModule_OnCommand();
        document.getElementById("txtReasonForChange").value = "";
        $("#cboPreviousProcess").children().remove();
        $("#cboUpdateOwner").children().remove();
        rbPanel.Disabled();
        document.getElementById("rbUpdateOwner").disabled = false;
        document.getElementById("checkkShowAllOwner").checked = false;
        document.getElementById("rbUpdateOwner").checked = false;
        document.getElementById("rbUpdateProcess").disabled = false;
        document.getElementById("rbUpdateProcess").checked = false;
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    });
}

$(document).ready(function () {
    rbPanel.Disabled();
    if (document.getElementById("txtPatientName").value != "") {
        FillGrid();
    }
});
function GetSelectedRow() {
    var SelectedRow = $("#grdAdminModuleWfobjwct tr.highlight td");
    var Header = $("#grdAdminModuleWfobjwct thead tr");
    let ssSelectedRow = [];
    SelectedRow.each((i, row) => {
        dheader = $("#grdAdminModuleWfobjwct thead tr")[0].children[i].textContent;
        ssSelectedRow.push('"'+dheader + '":"' + row.textContent+'"')
    });

    return ssSelectedRow;
}
function rbUpdateProcess_CheckedChanged() {
    document.getElementById("pnlUpdateProcess").disabled = false;
    document.getElementById("pnlUpdateOwner").disabled = true;

    rbPanel.EnableUpdateProcess();
    rbPanel.DisabledUpdateOwner();

    $("#cboPreviousProcess").empty();
    
    let ssSelectedRow = GetSelectedRow();

    var WSData = JSON.stringify({ sSelectedRow: ssSelectedRow });
    $.ajax({
        type: "POST",
        url: '/frmWFObjectManager.aspx/rbUpdateProcess_CheckedChanged',
        data: WSData,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $("#cboPreviousProcess").empty();
            var cbodata = $.parseJSON(data.d);
            cbodata.forEach((option) => { $("#cboPreviousProcess").append("<option>" + option + "</option>") });
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                //CAP-792
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
function rbUpdateOwner_CheckedChanged() {
    document.getElementById("pnlUpdateProcess").disabled = true;
    document.getElementById("pnlUpdateOwner").disabled = false;

    rbPanel.DisabledUpdateProcess();
    rbPanel.EnableUpdateOwner();

    $("#cboUpdateOwner").empty();
    let ssSelectedRow = GetSelectedRow();
    var WSData = { sSelectedRow: ssSelectedRow, checkkShowAllOwner: document.getElementById("checkkShowAllOwner").checked };
    $.ajax({
        type: "POST",
        url: '/frmWFObjectManager.aspx/rbUpdateOwner_CheckedChanged',
        data: JSON.stringify(WSData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $("#cboUpdateOwner").empty();
            var cbodata = $.parseJSON(data.d);
            cbodata.forEach((option) => {
                $("#cboUpdateOwner").append("<option value="+option.Item2+">" + option.Item1 + "</option>")
            });
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        },
        error: function OnError(xhr) {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            if (xhr.status == 999)
                window.location = "/frmSessionExpired.aspx";
            else {
                //CAP-792
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

function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    //CAP-3378
    //var now = new Date(utcDate + ' UTC');
    var now = new Date(utcDate.toString().replace(' ', 'T') + 'Z');
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-Jan-0001 12:00 AM';
    else
        then = ('0' + now.getDate().format("dd")).slice(-2) + '-' + monthNames[now.getMonth()] + '-' + now.getFullYear();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = ('0' + hours).slice(-2) + ':' + minutes + ' ' + ampm;
    if (utcDate != '0001-01-01 00:00:00')
        then += ' ' + strTime;
    return then;
}

function Decompress(data) {
    // Decode the Base64 string
    const binaryString = window.atob(data);
    // Convert binary string to byte array
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    // Use pako to decompress the byte array
    const decompressed = pako.inflate(bytes, { to: 'string' });
    return JSON.parse(decompressed);
}

function DOBConvert(DOB) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    //return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
    return SplitDOB[2] + "-" + SplitDOB[1] + "-" + SplitDOB[0];
}
function PageLoadValidation() {
    DisplayErrorMessage('200035');
}