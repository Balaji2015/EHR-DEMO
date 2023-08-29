
function todaysdate(date) {

    var monthNames = ["Jan", "Feb", "Mar", "Apr",
                      "May", "Jun", "Jul", "Aug",
                      "Sep", "Oct", "Nov", "Dec"];

    var day = date.getDate();

    var monthIndex = date.getMonth();
    var monthName = monthNames[monthIndex];

    var year = date.getFullYear();

    return day + "-" + monthName + "-" + year
}
function showTime() {

    var dt = new Date(); document.getElementById("hdnBSave").value = false; var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalTime").value = utc; return dt.toUTCString();
    var dtLocal = new Date();
    document.getElementById("hdnPCTime").value = dtLocal.toLocaleDateString();
}
function WaitCursor() {
    ShowLoading();
    showTime();
    return true;
}

function IsEmail(email) {
    var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return expr.test(email);

}
function setcurrentdate() {
    $('#lblcollection').html($('#lblcollection').html().replace("Today", todaysdate(new Date())));
    document.getElementById('hdndate').value = $('#lblcollection').html();

}
function Loading() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById('btnAdd').disabled = false;
    // document.getElementById(GetClientId("ddlVoucher")).value = document.getElementById(GetClientId("hdnEncounterID")).value;
    return true;
}
function bindauthtable(objdata, IsAuth) {
    var vchk = "";
    var tabContents = "";
    var icd = "";
    if (objdata != null && objdata != undefined && objdata.length > 0) {
        for (var i = 0; i < objdata.length; i++) {
            icd = "";
            for (var j = 0; j < objdata[i][4].split(',').length; j++) {
                if (icd == "") {
                    if (objdata[i][4].split(',')[j] != "") {
                        icd = objdata[i][4].split(',')[j];
                    }
                }

                else {
                    if (objdata[i][4].split(',')[j] != "") {
                        icd = icd + "," + objdata[i][4].split(',')[j];
                    }
                }
            }
            if (localStorage.getItem("PatientSummary") == 'Y') {
                vchk = "<input type='checkbox' disabled />"
            }

            else if (IsAuth == "Y") {
                vchk = "<input type='checkbox' disabled checked />"
            }
            else {
                vchk = "<input type='checkbox' onclick='AutoSave(this);'/>"
            }
            if (IsAuth == "N") {
                if (parseInt(objdata[i][2]) - parseInt(objdata[i][3]) > 0) {
                    if (parseInt(objdata[i][2]) - parseInt(objdata[i][3]) <= 2) {
                        if (tabContents == "") {
                            tabContents = "<tr style='color:red'><td style='width:3%'>" + vchk + "</td>" +
                                "<td style='width:18%;text-align: center;'>" + objdata[i][0] + "</td>" +
                            "<td style='width:5%'>" + objdata[i][1] + "</td>" +
                     "<td style='width:18%;text-align: center;'>" + objdata[i][2] + "</td>" +
                     "<td style='width:18%;text-align: center;''>" + objdata[i][3] + "</td>" +
                     "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                     "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                     "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +

                     "<td style='width:10%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][9] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][10] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][11] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][12] + "</td>" +
                      "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +


                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"
                        }
                        else {
                            tabContents = tabContents + "<tr style='color:red'><td style='width:3%'>" + vchk + "</td>" +
                                "<td style='width:18%;text-align: center;'>" + objdata[i][0] + "</td>" +
                            "<td style='width:5%;text-align: center;'>" + objdata[i][1] + "</td>" +
                     "<td style='width:18%;text-align: center;'>" + objdata[i][2] + "</td>" +
                     "<td style='width:18%;text-align: center;'>" + objdata[i][3] + "</td>" +
                      "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                     "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                     "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][9] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][10] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][11] + "</td>" +
                     "<td style='width:10%;text-align: center;'>" + objdata[i][12] + "</td>" +
                        "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"
                        }


                    }

                    else {
                        if (tabContents == "") {
                            tabContents = "<tr><td style='width:21%'>" + vchk + "</td>" +
                                "<td style='width:73%;text-align: center;'>" + objdata[i][0] + "</td>" +
                            "<td style='width:25%;text-align: center;'>" + objdata[i][1] + "</td>" +
                     "<td style='width:65%;text-align: center;'>" + objdata[i][2] + "</td>" +
                     "<td style='width:51%;text-align: center;'>" + objdata[i][3] + "</td>" +
                       "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                     "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                     "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                     "<td style='width:63%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +
                     "<td style='width:56%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                     "<td style='width:33%;text-align: center;'>" + objdata[i][9] + "</td>" +
                     "<td style='width:56%;text-align: center;'>" + objdata[i][10] + "</td>" +
                     "<td style='width:47%;text-align: center;'>" + objdata[i][11] + "</td>" +
                     "<td style='width:43%;text-align: center;'>" + objdata[i][12] + "</td>" +
                   "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"
                        }
                        else {
                            tabContents = tabContents + "<tr><td style='width:21%'>" + vchk + "</td>" +
                                "<td style='width:73%;text-align: center;'>" + objdata[i][0] + "</td>" +
                            "<td style='width:25%;text-align: center;'>" + objdata[i][1] + "</td>" +
                     "<td style='width:65%;text-align: center;'>" + objdata[i][2] + "</td>" +
                     "<td style='width:51%;text-align: center;'>" + objdata[i][3] + "</td>" +
                       "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                     "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                     "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                     "<td style='width:63%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +
                     "<td style='width:56%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                     "<td style='width:33%;text-align: center;'>" + objdata[i][9] + "</td>" +
                     "<td style='width:56%;text-align: center;'>" + objdata[i][10] + "</td>" +
                     "<td style='width:47%;text-align: center;'>" + objdata[i][11] + "</td>" +
                     "<td style='width:43%;text-align: center;'>" + objdata[i][12] + "</td>" +
                       "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"



                        }
                    }
                }

            }
            else {
                if (parseInt(objdata[i][2]) - parseInt(objdata[i][3]) <= 2) {
                    if (tabContents == "") {
                        tabContents = "<tr style='color:red'><td style='width:3%'>" + vchk + "</td>" +
                            "<td style='width:18%;text-align: center;'>" + objdata[i][0] + "</td>" +
                        "<td style='width:5%'>" + objdata[i][1] + "</td>" +
                 "<td style='width:18%;text-align: center;'>" + objdata[i][2] + "</td>" +
                 "<td style='width:18%;text-align: center;''>" + objdata[i][3] + "</td>" +
                 "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                 "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                 "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +

                 "<td style='width:10%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][9] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][10] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][11] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][12] + "</td>" +
                   "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"
                    }
                    else {
                        tabContents = tabContents + "<tr style='color:red'><td style='width:3%'>" + vchk + "</td>" +
                            "<td style='width:18%;text-align: center;'>" + objdata[i][0] + "</td>" +
                        "<td style='width:5%;text-align: center;'>" + objdata[i][1] + "</td>" +
                 "<td style='width:18%;text-align: center;'>" + objdata[i][2] + "</td>" +
                 "<td style='width:18%;text-align: center;'>" + objdata[i][3] + "</td>" +
                  "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                 "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                 "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][9] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][10] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][11] + "</td>" +
                 "<td style='width:10%;text-align: center;'>" + objdata[i][12] + "</td>" +
                 "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"
                    }


                }

                else {
                    if (tabContents == "") {
                        tabContents = "<tr><td style='width:21%'>" + vchk + "</td>" +
                            "<td style='width:73%;text-align: center;'>" + objdata[i][0] + "</td>" +
                        "<td style='width:25%;text-align: center;'>" + objdata[i][1] + "</td>" +
                 "<td style='width:65%;text-align: center;'>" + objdata[i][2] + "</td>" +
                 "<td style='width:51%;text-align: center;'>" + objdata[i][3] + "</td>" +
                   "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                 "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                 "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                 "<td style='width:63%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +
                 "<td style='width:56%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                 "<td style='width:33%;text-align: center;'>" + objdata[i][9] + "</td>" +
                 "<td style='width:56%;text-align: center;'>" + objdata[i][10] + "</td>" +
                 "<td style='width:47%;text-align: center;'>" + objdata[i][11] + "</td>" +
                 "<td style='width:43%;text-align: center;'>" + objdata[i][12] + "</td>" +
                  "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"
                    }
                    else {
                        tabContents = tabContents + "<tr><td style='width:21%'>" + vchk + "</td>" +
                            "<td style='width:73%;text-align: center;'>" + objdata[i][0] + "</td>" +
                        "<td style='width:25%;text-align: center;'>" + objdata[i][1] + "</td>" +
                 "<td style='width:65%;text-align: center;'>" + objdata[i][2] + "</td>" +
                 "<td style='width:51%;text-align: center;'>" + objdata[i][3] + "</td>" +
                   "<td style='width:12%;text-align: center;'>" + icd + "</td>" +
                 "<td style='width:5%;text-align: center;'>" + objdata[i][5] + "</td>" +
                 "<td style='width:20%;text-align: center;'>" + objdata[i][6] + "</td>" +
                 "<td style='width:63%;text-align: center;'>" + objdata[i][7].split('|')[0] + "</td>" +
                 "<td style='width:56%;text-align: center;'>" + objdata[i][8].split('|')[0] + "</td>" +
                 "<td style='width:33%;text-align: center;'>" + objdata[i][9] + "</td>" +
                 "<td style='width:56%;text-align: center;'>" + objdata[i][10] + "</td>" +
                 "<td style='width:47%;text-align: center;'>" + objdata[i][11] + "</td>" +
                 "<td style='width:43%;text-align: center;'>" + objdata[i][12] + "</td>" +
                  "<td style='width:49%;text-align: center;'>" + objdata[i][14] + "</td>" +
                     "<td style='width:41%;text-align: center;'>" + objdata[i][15] + "</td>" +
                      "<td style='width:10%;text-align: center;'>" + objdata[i][13] + "</td>" +

                   "<td style='display:none;text-align: center;'>" + objdata[i][16] + "</td>" +
                     "<td style='display:none;text-align: center;'>" + objdata[i][17] + "</td>" +
                            "<td style='display:none;text-align: center;'>" + objdata[i][18] + "</td>"



                    }
                }

            }
        }

    }
    if (tabContents == "") {
        tabContents = "<tr><td style='width:3%' colspan='18'>No Data Found</td></tr>"

    }
    $("#authtable").empty();
    $("#authtable").append("<table id=AuthCPtTable class='table table-bordered Gridbodystyle'   style='width:1300px;table-layout:fixed;overflow:auto;height:9%'> <thead class='header' ><tr class='header' > <th class='header' style='border: 1px solid #909090; width: 21%;'>Sel</th>" +
        "<th class='header'style='width: 73%;'>Authorization#</th>" +
    "<th class='header'style='border: 1px solid #909090; width: 26%;'>CPT</th>" +
    "<th class='header'style='border: 1px solid #909090;width: 65%;'>Approved Qty</th>" +
    "<th class='header'style='width: 51%;'>Used Qty</th>" +
    "<th class='header'style='border: 1px solid #909090;text-align: center;width: 62%;'>Diagnosis</th>" +
        "<th class='header'style='border: 1px solid #909090;width: 60%;'>Valid From</th>" +
        "<th class='header'style='border: 1px solid #909090;width: 47%;'>Valid To</th>" +
        "<th class='header'style='border: 1px solid #909090;text-align: center;width: 63%;' >Refered from</th>" +
        "<th class='header'style='border: 1px solid #909090;text-align: center;width: 56%;'>Referred To</th>" +
        "<th class='header'style='border: 1px solid #909090;text-align: center;width: 33%;'>Facility</th>" +
            "<th class='header'style='border: 1px solid #909090;text-align: center;width: 56%;'>Payer Name</th>" +
            "<th class='header'style='border: 1px solid #909090;text-align: center;width: 47%;'>Plan Name</th>" +
            "<th class='header'style='border: 1px solid #909090;text-align: center;width: 43%;'>PCP Name</th>" +
            "<th class='header'style='border: 1px solid #909090;text-align: center;width: 23%;'>POS</th>" +
                "<th class='header'style='border: 1px solid #909090;text-align: center;width: 49%;'>Category</th>" +
                    "<th class='header'style='border: 1px solid #909090;text-align: center;width: 41%;'>Comments</th>" +
                        "</tr></thead><tbody style='word-wrap: break-word;' class='Gridbodystyle'>" + tabContents + "</tbody></table>");
}

function OpenEV() {
    
    var planid = 0;
    var policyholderid = 0;
    if ($('#grdExistingPolicies tr.aspSelectedRow') != undefined && $('#grdExistingPolicies tr.aspSelectedRow').length > 0 && planid != null) {
        planid = $('#grdExistingPolicies tr.aspSelectedRow')[0].cells[2].innerText;
        policyholderid = $('#grdExistingPolicies tr.aspSelectedRow')[0].cells[1].innerText;
    }

    if (document.getElementById(GetClientId('txtPatientAccountNo')).value.trim() != undefined && document.getElementById(GetClientId('txtPatientAccountNo')).value.trim() != "" && document.getElementById(GetClientId('txtPatientAccountNo')).value.trim() != "0") {
        setTimeout(
function () {
    var oWnd = GetRadWindow();
    var childWindow = oWnd.BrowserWindow.radopen("frmPerformEV.aspx?HumanId=" + document.getElementById(GetClientId('txtPatientAccountNo')).value + "&InsPlanId=" + planid + "&ScreenMode=ManualEv", "RadOnlineWindow");
    setRadWindowPropertiesEV(childWindow, 610, 1200);
    childWindow.add_close(RefereshInsuranceList);
}, 0);
        return false;
    }
    else {
        DisplayErrorMessage('1011161');
        return false;
    }
}

function RefereshInsuranceList() {
    document.getElementById('Button2').click();
}
function openPdf() {

    var planid = 0;
    var policyholderid = 0;
    if ($('#grdExistingPolicies tr.aspSelectedRow') != undefined && $('#grdExistingPolicies tr.aspSelectedRow').length > 0 && planid != null) {
        planid = $('#grdExistingPolicies tr.aspSelectedRow')[0].cells[2].innerText;
        policyholderid = $('#grdExistingPolicies tr.aspSelectedRow')[0].cells[1].innerText;

        var humanid = document.getElementById('txtPatientAccountNo').value;
        //For Bug Id:63721
        var humanlastname = document.getElementById('txtPatientLastName').value;
        var humanfirstname = document.getElementById('txtPatientFirstName').value;
        var humanevdate = document.getElementById('txtEligibilityVerificationDate').value;
        var sFaxSubject = "Eligibility verification_" + humanlastname + "_" + humanfirstname + "_" + humanevdate;
        if (planid != "" && planid != "0" && humanid != "" & humanid != "0") {
            $.ajax({
                type: "POST",
                url: "frmQuickPatientCreate.aspx/LoadEVDetails",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    "PlanData": humanid + "|" + planid + "|" + policyholderid
                }),
                dataType: "json",
                async: true,
                success: function (data) {
                    var objdata = $.parseJSON(data.d);
                    if (objdata != "" && objdata != null && objdata != undefined && objdata.length > 0 && objdata[0][1] != null) {
                        var filepath = objdata[0][1];
                        var obj = new Array();
                        obj.push("SI=" + filepath);
                        obj.push("Location=" + "EV");
                        obj.push("Human_ID=" + humanid);
                        obj.push("FaxSubject=" + sFaxSubject);
                        var result = openModal("frmPrintPDF.aspx", 575, 1000, obj, "MessageWindow");

                    }
                    else {
                        alert("No Files Available");
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
                        return false;
                    }
                }
            });
        }
    }
    else {
        alert("No Files Available");
    }
    return false;
}
function setCPtvalue() {
    localStorage.setItem("Authdata", "");
    if (localStorage.getItem("cpt") != undefined && localStorage.getItem("cpt") != null)
        document.getElementById('hdnCPT').value = localStorage.getItem("cpt");
}

function SaveAuthEncounter() {
    var data = "";
    for (var i = 0; i < $('#AuthCPtTable > tbody  > tr').length; i++) {
        if (!($('#AuthCPtTable > tbody  > tr')[i].childNodes[0].childNodes[0].disabled) && $('#AuthCPtTable > tbody  > tr')[i].childNodes[0].childNodes[0].checked)
            if (data == "") {
                data = $('#AuthCPtTable > tbody  > tr')[i].childNodes[17].innerText.trim() + "~" +
                $('#AuthCPtTable > tbody  > tr')[i].childNodes[18].innerText.trim() + "~" +
                $('#AuthCPtTable > tbody  > tr')[i].childNodes[19].innerText.trim() + "~" + document.getElementById('hdnEncounterID').value;
            }
            else {
                data = data + "|" + $('#AuthCPtTable > tbody  > tr')[i].childNodes[17].innerText.trim() + "~" +
               $('#AuthCPtTable > tbody  > tr')[i].childNodes[18].innerText.trim() + "~" +
               $('#AuthCPtTable > tbody  > tr')[i].childNodes[19].innerText.trim() + "~" + document.getElementById('hdnEncounterID').value;
            }
    }

    if (data != "") {
        $.ajax({
            type: "POST",
            url: "frmQuickPatientCreate.aspx/SaveAuthorization",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "AuthData": data,
            }),
            dataType: "json",
            async: true,
            success: function (data) {
                DisplayErrorMessage('380018');
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

}
function WaitCursorend() {
    document.getElementById('divLoading').style.display = "none";
}
function CloseWindow() {
    if (document.getElementById("btnSave") != null) {
        if (document.getElementById("btnSave").disabled == false) {
            if (document.getElementById("hdnMessageType").value == "") {
                DisplayErrorMessage('380049');
            }
            else if (document.getElementById("hdnMessageType").value == "Yes") {
                document.getElementById("btnSave").click();
                document.getElementById("hdnMessageType").value = "";
                return false;
            }
            else if (document.getElementById("hdnMessageType").value == "No") {
                document.getElementById("hdnMessageType").value = ""
                returnToParent(null);
            }
            else if (document.getElementById("hdnMessageType").value == "Cancel") {
                document.getElementById("hdnMessageType").value = "";
            }
        }
        else {
            returnToParent(null);
        }
    }
    else
        returnToParent(null);
    //$(top.window.document).find("#btnQuickPatientClose").click();
    return false;
}

function CheckMultiPayment() {
    if (document.getElementById("chkMultiplePayments") != null && document.getElementById("chkMultiplePayments").checked == true)
    { document.getElementById("btnAdd").disabled = false; document.getElementById("btnClear").disabled = false; document.getElementById("hdnBSave").value = true; document.getElementById("btnSave").disabled = false; }
    else
    { document.getElementById("btnAdd").disabled = true; document.getElementById("btnClear").disabled = true; document.getElementById("hdnBSave").value = true; }
}
function AutoSave(ctrl) {
    if (document.getElementById("hdnScreenMode").value != "COLLECT COPAY") {
        if (ctrl != undefined && ctrl.readOnly != true) {
            document.getElementById("hdnBSave").value = true;
            //Jira #CAP-193
            if (document.getElementById("btnSave") != null && document.getElementById("btnSave") != undefined)
                document.getElementById("btnSave").disabled = false;
        }
    }
}


function ShowMessage() {
    if (document.getElementById("btnClear").value == "Clear All") {
        var ErrorMessage1 = window.confirm("Are you sure you want to clear all the fields in Payment information?");
        if (ErrorMessage1 == true) {
            PaymentInformationClear();
            document.getElementById("txtPaymentAmount").value = "0.00";
            document.getElementById("txtRecOnAcc").value = "0.00";
            document.getElementById("txtRefundAmount").value = "0.00";
            document.getElementById("txtPastDue").value = "0.00";
            document.getElementById("btnAdd").disabled = true;
            document.getElementById("cboMethodOfPayment").selectedIndex = "0";
            document.getElementById("txtCheckNo").value = '';

            if (document.getElementById("cboRelation").value == "Patient") {
                document.getElementById("txtpaidBy").value = document.getElementById("txtPatientLastName").value + "," + document.getElementById("txtPatientFirstName").value;////divPatientstrip.innerHTML.split('|')[0];  // document.getElementById("hdnPatientName").value;
                $('#txtpaidBy').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            }
            $('#spanPaymentNotes').removeClass('MandLabelstyle');
            $('#spanPaymentNotes').addClass('spanstyle');
            $('#spanPatientNotestar').css('visibility', 'hidden');
            document.getElementById("btnAdd").value = "Add";
            $('#txtRecOnAcc').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtRefundAmount').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtpaidBy').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtPaymentAmount').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtCheckNo').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtAuthNo').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#cboRelation').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#dtpCheckDate').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtPaymentNote').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#cboMethodOfPayment').addClass('Editabletxtbox').removeClass('nonEditabletxtbox');
            document.getElementById("cboMethodOfPayment").disabled = false;
            document.getElementById("txtRecOnAcc").disabled = true;
            document.getElementById("txtRefundAmount").disabled = true;
            document.getElementById("txtpaidBy").disabled = true;
            document.getElementById("txtPaymentAmount").disabled = true;
            document.getElementById("txtCheckNo").disabled = true;
            document.getElementById("txtAuthNo").disabled = true;
            document.getElementById("cboRelation").disabled = true;
            document.getElementById("dtpCheckDate").disabled = true;
            document.getElementById("txtPaymentNote").disabled = true;
            document.getElementById("dtpCheckDate").value = '';
            $('#spanCheck').removeClass('MandLabelstyle');
            $('#spanCheck').addClass('spanstyle');
            $('#spanCheckStar').css('visibility', 'hidden');
            return true;
        }
    }
    else {
        var ErrorMessage2 = window.confirm("Are you sure you want to Cancel?");
        if (ErrorMessage2 == true) {
            PaymentInformationClear();
            document.getElementById("txtPaymentAmount").value = "0.00";
            document.getElementById("txtRecOnAcc").value = "0.00";
            document.getElementById("txtRefundAmount").value = "0.00";
            document.getElementById("txtPastDue").value = "0.00";
            document.getElementById("txtCheckNo").value = '';
            document.getElementById("btnAdd").disabled = true;
            document.getElementById("cboMethodOfPayment").selectedIndex = "0";
            if (document.getElementById("cboRelation").value == "Patient") {
                document.getElementById("txtpaidBy").value = document.getElementById("txtPatientLastName").value + "," + document.getElementById("txtPatientFirstName").value;   //document.getElementById("hdnPatientName").value;
                $('#txtpaidBy').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            }
            $('#cboMethodOfPayment').addClass('Editabletxtbox').removeClass('nonEditabletxtbox');
            document.getElementById("cboMethodOfPayment").disabled = false;
            $('#spanPaymentNotes').removeClass('MandLabelstyle');
            $('#spanPaymentNotes').addClass('spanstyle');
            $('#spanPatientNotestar').css('visibility', 'hidden');
            return true;
        }
    }
}


function PaymentInformationClear() {
    document.getElementById("cboRelation").selectedIndex = "0";
    document.getElementById("txtpaidBy").value = '';

    document.getElementById("txtAuthNo").value = '';
    document.getElementById("txtPaymentAmount").value = "0.00";
    document.getElementById("txtCheckNo").value = '';
    document.getElementById("txtRefundAmount").value = "0.00";

    document.getElementById("txtRecOnAcc").value = "0.00";
    document.getElementById("txtPaymentNote").value = '';
    document.getElementById("txtPastDue").value = "0.00";

}

function Exit(args) {
    DisplayErrorMessage('380018');
    var Results = args.replace("&apos", "'");
    var Result = new Object();
    var lname = Results.split('|')[1];
    var fname = Results.split('|')[2];
    var mname = Results.split('|')[3].length > 1 ? Results.split('|')[3] : "";
    var patname = lname + "," + fname + " " + mname;
    PatientName = patname;
    var obj = new Array();
    Result.Human_id = Results.split('|')[0];
    Result.PatientName = patname;
    Result.PatientDOB = Results.split('|')[4];
    Result.Encounter_Provider_ID = Results.split('|')[5];
    Result.Cell_Phone = Results.split('|')[6];
    Result.Home_Phone = Results.split('|')[7];
    Result.HumanType = Results.split('|')[8];
    Result.PatientGender = Results.split('|')[9];
    Result.IsNewPatient = "TRUE";
    if (document.getElementById("hdnScreenMode").value.toUpperCase() == "FIND PATIENT") {
        Result.IsQuickPatient = "TRUE";
    }
    if (document.getElementById("hdnScreenMode").value.toUpperCase() == "CHECKEDIN") {
        returnToParent(null);
    }

    $(top.window.document).find("#txtPatientInformation")[0].value = JSON.stringify(Result);
    $(top.window.document).find("#btnQuickPatientClose").click();
    $(top.window.document).find("#btnFindPatientClose").click();

    if (window.opener)
    { window.opener.returnValue = Result; } window.returnValue = Result;
    returnToParent(Result);
}
function jsFormatSSN(asSSNControl) {
    var re = /\D/g; var lvCurrentSSNControlID = asSSNControl.id; var lvSSNControl = lvCurrentSSNControlID.substring(lvCurrentSSNControlID.lastIndexOf("_") + 1); var lvParent; var lvCompare; var lvRequired; if (lvSSNControl == "msktxtSSN")
    { lvParent = document.getElementById("msktxtSSN"); }
    var lvNumber = lvParent.value.replace(re, ""); var lvLength = lvNumber.length; if (lvLength > 3 && lvLength < 6)
    { var lvSegmentA = lvNumber.slice(0, 3); var lvSegmentB = lvNumber.slice(3, 5); lvParent.value = lvSegmentA + "-" + lvSegmentB; }
    else {
        if (lvLength > 5)
        { var lvSegmentA = lvNumber.slice(0, 3); var lvSegmentB = lvNumber.slice(3, 5); var lvSegmentC = lvNumber.slice(5, 9); lvParent.value = lvSegmentA + "-" + lvSegmentB + "-" + lvSegmentC; }
        else {
            if (lvLength < 1)
            { lvParent.value = ""; }
            lvParent.value = lvNumber;
        }
    }
}
function FormatPhone(event, txtbox) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false; else {
        if (txtbox.value.length == 0)
            txtbox.value += "("; else if (txtbox.value.length == 4)
                txtbox.value += ")"; else if (txtbox.value.length == 5)
                    txtbox.value += " "
                else if (txtbox.value.length == 8)
                    txtbox.value += "-"; else if (txtbox.value.length == 13)
                        return false
    }
}
function FormatZipCode(txtbox) {
    var charCode = (txtbox.which) ? txtbox.which : txtbox.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false; if (txtbox.value.length == 5)
            txtbox.value += "-"; else
            return
}
function ConfirmHumanDuplicate() {
    if (window.confirm("Patient with the same Name , Date of Birth and Sex exists in the system. Do you want to create a new patient?") == true) {
        ShowLoading();
        document.getElementById('hdnDupsendmail').value = 'true';
        document.getElementById("btnCheckDuplicate").click();
    }
    else { return false; }
}

function ValidatePatientInformation() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var dt = new Date(); document.getElementById("hdnBSave").value = false; var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalTime").value = utc;
    var dtLocal = new Date();
    document.getElementById("hdnPCTime").value = dtLocal.toLocaleDateString();
    document.getElementById("hdnValidation").value = "true";
    if (document.getElementById("txtPatientLastName").readOnly == false) {
        if (document.getElementById("txtPatientLastName").value.length == 0) {

            DisplayErrorMessage('380010');
            window.setTimeout(function () {
                document.getElementById('txtPatientLastName').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("txtPatientFirstName").value.length == 0) {


            DisplayErrorMessage('380011');
            window.setTimeout(function () {
                document.getElementById('txtPatientFirstName').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("dtpPatientDOB").value == "__-___-____") {

            DisplayErrorMessage('380012');
            window.setTimeout(function () {
                document.getElementById('dtpPatientDOB').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (DOBValidation("dtpPatientDOB") == false) {

            DisplayErrorMessage('380002');
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("cboPatientSex").value.length == 0) {


            DisplayErrorMessage('380013');
            window.setTimeout(function () {
                document.getElementById('cboPatientSex').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtCellPhno").value.length != 0 && PhNoValid("msktxtCellPhno") == false && document.getElementById("msktxtCellPhno").value != "(___) ___-____" && document.getElementById("msktxtCellPhno").RadInputValidationValue.length < 14) {

            DisplayErrorMessage('380031');
            window.setTimeout(function () {
                document.getElementById('msktxtCellPhno').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtCellPhno").RadInputValidationValue.length > 0 && document.getElementById("msktxtCellPhno").RadInputValidationValue.length < 14) {

            DisplayErrorMessage('380031');
            window.setTimeout(function () {
                document.getElementById('msktxtCellPhno').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtHomePhno").value.length != 0 && PhNoValid("msktxtHomePhno") == false && document.getElementById("msktxtHomePhno").value != "(___) ___-____") {

            DisplayErrorMessage('380032');
            window.setTimeout(function () {
                document.getElementById('msktxtHomePhno').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtHomePhno").RadInputValidationValue.length > 0 && document.getElementById("msktxtHomePhno").RadInputValidationValue.length < 14) {

            DisplayErrorMessage('380032');
            window.setTimeout(function () {
                document.getElementById('msktxtHomePhno').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtSSN").value.length != 0 && document.getElementById("msktxtSSN").value != "___-__-____") {
            var str = document.getElementById("msktxtSSN").value;
            if (str.replace(/_/gi, "").length < 11) {

                DisplayErrorMessage('380039');
                window.setTimeout(function () {
                    document.getElementById('msktxtSSN').focus();
                }, 0);
                document.getElementById("hdnValidation").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }

        if (document.getElementById("txtMail").value.length != 0) {

            if (IsEmail(document.getElementById("txtMail").value) == false) {
                DisplayErrorMessage('320010');
                document.getElementById(GetClientId("txtMail")).focus();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }

        }
        if (document.getElementById("msktxtZipcode").value == "_____-____") {

            DisplayErrorMessage('380044');
            window.setTimeout(function () {
                document.getElementById('msktxtZipcode').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtZipcode").value.length != 0 && document.getElementById("msktxtZipcode").value != "_____-____") {
            var str = document.getElementById("msktxtZipcode").value;
            if (str.replace(/_/gi, "").length != 6 && str.replace(/_/gi, "").length != 10) {

                DisplayErrorMessage('380037');
                window.setTimeout(function () {
                    document.getElementById('msktxtZipcode').focus();
                }, 0);
                document.getElementById("hdnValidation").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        if (document.getElementById("gbPaymentInformation") != null && document.getElementById("txtPaymentAmount").readOnly == false && document.getElementById("txtPaymentAmount").value.length == 0) {


            DisplayErrorMessage('380020');
            window.setTimeout(function () {
                document.getElementById('txtPaymentAmount').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

    if (document.getElementById("gbEligibilityVerification") != null && document.getElementById("chkEligibilityVerified").checked == true) {
        if (document.getElementById("ddlPayerName").value.length == 0) {

            DisplayErrorMessage('380014');
            window.setTimeout(function () {
                document.getElementById('ddlPayerName').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("ddlPlanName").value.length == 0) {


            DisplayErrorMessage('380028');
            window.setTimeout(function () {
                document.getElementById('ddlPlanName').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("ddlPayerName").value == "OTHER" && document.getElementById("ddlPlanName").value == "OTHER") {
            if (document.getElementById("txtOtherPayerName").value.length == 0) {

                DisplayErrorMessage('380045');
                window.setTimeout(function () {
                    document.getElementById('txtOtherPayerName').focus();
                }, 0);
                document.getElementById("hdnValidation").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            if (document.getElementById("txtOtherPlan").value.length == 0) {

                DisplayErrorMessage('380046');
                window.setTimeout(function () {
                    document.getElementById('txtOtherPlan').focus();
                }, 0);
                document.getElementById("hdnValidation").value = "false";
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }

        if (document.getElementById("txtPolicyHolderID").value.length == 0) {


            DisplayErrorMessage('380015');
            window.setTimeout(function () {
                document.getElementById('txtPolicyHolderID').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("dtpEffectiveStartDate").value == "__-___-____") {
            DisplayErrorMessage('380016');
            window.setTimeout(function () {
                document.getElementById('dtpEffectiveStartDate').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        if (ValidateEffStartDate("dtpPatientDOB", "dtpEffectiveStartDate") == false) {


            DisplayErrorMessage('380003');
            window.setTimeout(function () {
                document.getElementById('dtpEffectiveStartDate').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (ValidateEffStartDate("dtpEffectiveStartDate", "dtpTerminationDate") == false && document.getElementById("dtpTerminationDate").value != "__-___-____") {


            DisplayErrorMessage('380005');
            window.setTimeout(function () {
                document.getElementById('dtpTerminationDate').focus();
            }, 0);
            document.getElementById("hdnValidation").value = "false";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById("pnlAuthorization") != null) {

        if (document.getElementById("txtauthValidfrom") != null && document.getElementById("txtauthvalidTo") != null && (document.getElementById("txtauthValidfrom").value != "__-___-____" && document.getElementById("txtauthvalidTo").value == "__-___-____") ||
            (document.getElementById("txtauthvalidTo").value != "__-___-____" && document.getElementById("txtauthValidfrom").value == "__-___-____")) {

            alert('Please Enter both Authorization Valid from and valid to date ');


            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("txtauthValidfrom") != null && document.getElementById("txtauthvalidTo") != null &&

            (ValidateEffStartDate("txtauthValidfrom", "txtauthvalidTo") == false && document.getElementById("txtauthValidfrom").value != "__-___-____"
            && document.getElementById("txtauthvalidTo").value != "__-___-____")) {


            alert('Authorization valid to  date  should be greater than Authorization valid from  date');


            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    if (document.getElementById("cboMethodOfPayment") != null) {
        if (document.getElementById("cboMethodOfPayment").value.length != 0) {
            if (document.getElementById("btnAdd").value == "Update") {
                DisplayErrorMessage('380050');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            if (document.getElementById("gbPaymentInformation") != null) {
                if (document.getElementById("txtCheckNo").readOnly == false && document.getElementById("txtCheckNo").value.length == 0) {

                    DisplayErrorMessage('380021');
                    window.setTimeout(function () {
                        document.getElementById('txtCheckNo').focus();
                    }, 0);
                    document.getElementById("hdnValidation").value = "false";
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
                }
                if (document.getElementById("txtPaymentAmount").readOnly == false) {
                    if (document.getElementById("txtPaymentAmount").value.length == 0) {

                        DisplayErrorMessage('380020');
                        window.setTimeout(function () {
                            document.getElementById('txtPaymentAmount').focus();
                        }, 0);
                        document.getElementById("hdnValidation").value = "false";
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                    else if (parseFloat(document.getElementById("txtPaymentAmount").value < parseFloat(0))) {

                        DisplayErrorMessage('380020');
                        window.setTimeout(function () {
                            document.getElementById('txtPaymentAmount').focus();
                        }, 0);
                        document.getElementById("hdnValidation").value = "false";
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                        return false;
                    }
                }

            }
            if (document.getElementById("gbPaymentInformation") != null && document.getElementById("chkMultiplePayments") != null && document.getElementById("chkMultiplePayments").checked == true) {
                if (document.getElementById("gbPaymentInformation") != null) {
                    if (document.getElementById("btnAdd").value == "Update") {
                        DisplayErrorMessage('380050');
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }
                    if (document.getElementById("txtCheckNo").readOnly == false && document.getElementById("txtCheckNo").value.length == 0) {

                        DisplayErrorMessage('380021');
                        window.setTimeout(function () {
                            document.getElementById('txtCheckNo').focus();
                        }, 0);
                        document.getElementById("hdnValidation").value = "false";
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }

                    if (document.getElementById(GetClientId("txtEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtEmail")).value) == false) {
                        DisplayErrorMessage('295014');
                        document.getElementById(GetClientId("txtEmail")).focus();
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                        return false;
                    }


                    if (document.getElementById("txtPaymentAmount").readOnly == false) {
                        if (document.getElementById("txtPaymentAmount").value.length == 0) {
                            DisplayErrorMessage('380020');
                            window.setTimeout(function () {
                                document.getElementById('txtPaymentAmount').focus();
                            }, 0);
                            document.getElementById("hdnValidation").value = "false";
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        }
                        else if (parseFloat(document.getElementById("txtPaymentAmount").value < parseFloat(0))) {

                            DisplayErrorMessage('380020');
                            window.setTimeout(function () {
                                document.getElementById('txtPaymentAmount').focus();
                            }, 0);
                            document.getElementById("hdnValidation").value = "false";
                            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                            return false;
                        }
                    }

                }
            }
        }
    }
    SaveAuthEncounter();
}

function validatemail() {
    if (document.getElementById(GetClientId("txtEmail")).value.length != 0 && IsEmail(document.getElementById(GetClientId("txtEmail")).value) == false) {

        DisplayErrorMessage('420030');
        document.getElementById(GetClientId("txtEmail")).focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}



function DOBValidation(dateToValidate) {
    var splitdate = document.getElementById(dateToValidate).value;
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
    if ((dd.getFullYear() > dt1.getFullYear())) {
        return false;
    }
    else if (dd.getMonth() > dt1.getMonth() && (dd.getFullYear() >= dt1.getFullYear())) {
        return false;
    }
    else if (dd.getDate() > dt1.getDate() && (dd.getMonth() >= dt1.getMonth()) && (dd.getFullYear() >= dt1.getFullYear())) {
        return false;
    }
    else {
        return true;
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
function ValidateEffStartDate(EffDate, TermDate) {
    //var splitEffDatedate = $find(EffDate)._text;
    //var splitTermDate = $find(TermDate)._text;
    // CAT-286 - Prevanting undefind error for splitEffDatedate and splitTermDate.
    var splitEffDatedate = '';
    if ($find(EffDate) != null) {
        splitEffDatedate = $find(EffDate)._text ?? "";
    }
    var splitTermDate = '';
    if ($find(TermDate) != null) {
        splitTermDate = $find(TermDate)._text ?? "";
    }
    var EffDatedate = new Date();
    var TermDate = new Date();
    var m = getMonth(splitEffDatedate.split('-')[1]);
    if (m == 55) {
        return false;
    }
    EffDatedate.setFullYear(splitEffDatedate.split('-')[2], m, splitEffDatedate.split('-')[0]);
    if (isNaN(EffDatedate)) {
        return false;
    }
    var n = getMonth(splitTermDate.split('-')[1]);
    if (n == 55) {
        return false;
    }
    TermDate.setFullYear(splitTermDate.split('-')[2], n, splitTermDate.split('-')[0]);
    if (isNaN(TermDate)) {
        return false;
    }
    if (parseInt(splitEffDatedate.split('-')[0]) > 31) {
        return false;
    }
    if (parseInt(splitTermDate.split('-')[0]) > 31) {
        return false;
    }
    if ((EffDatedate.getFullYear() > TermDate.getFullYear())) {
        return false;
    }
    else if (EffDatedate.getMonth() > TermDate.getMonth() && (EffDatedate.getFullYear() >= TermDate.getFullYear())) {
        return false;
    }
    else if (EffDatedate.getDate() > TermDate.getDate() && (EffDatedate.getMonth() >= TermDate.getMonth()) && (EffDatedate.getFullYear() >= TermDate.getFullYear())) {
        return false;
    }
    else {
        return true;
    }
}

function getMonth(Month) {
    var month = new Array();
    switch (Month) {
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
        case Month:
            x = 55;
            break;
    }
    return x;
}

function AmountValidation(value, sMessage) {
    if (parseFloat(value) > parseFloat(9999999.99)) {
        DisplayErrorMessage('380019', '', sMessage);
        return false;
    }
    else {
        return true;
    }
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    if (document.getElementById("hdnScreenMode").value != "COLLECT COPAY") {
        document.getElementById("hdnBSave").value = true;
        //CAP-795 Cannot set properties of null
        if (document?.getElementById("btnSave") != undefined && document?.getElementById("btnSave") != null) {
            document.getElementById("btnSave").disabled = false;
        }
        return true;
    }
}

function onPaymentTypeChange() {
    document.getElementById("hdnBSave").value = true; document.getElementById("btnSave").disabled = false;
    if (document.getElementById("cboMethodOfPayment").value.length == 0) {
        paymentinformationdisableall();
        PaymentInformationClearAll();
    }
    else if (document.getElementById("cboMethodOfPayment").value.toUpperCase() == "CASH") {
        paymentinformationdisableall();

        paymentamountenable();
    }
    else if (document.getElementById("cboMethodOfPayment").value.toUpperCase() == "CHECK") {
        paymentamountenable();
        ChangeLabelStyle("lblCheckNo", false);
        TextBoxColorChange("txtCheckNo", true);
        TextBoxColorChange("txtAuthNo", true);
        document.getElementById("dtpCheckDate").disabled = false;



    }
    else if (document.getElementById("cboMethodOfPayment").value.toUpperCase() == "CREDIT CARD" || document.getElementById("cboMethodOfPayment").value.toUpperCase() == "DEBIT CARD") {
        paymentinformationdisableall();
        paymentamountenable();
        ChangeLabelStyle("lblCheckNo", false);
        TextBoxColorChange("txtCheckNo", true);
        TextBoxColorChange("txtAuthNo", false);
    }
    else {
        paymentamountenable();
        ChangeLabelStyle("lblCheckNo", false);
        TextBoxColorChange("txtCheckNo", true);
        TextBoxColorChange("txtAuthNo", true);

        DateTimePickerColorChangeForWindows("dtpCheckDate", true);
    }
}

function paymentinformationdisableall() {
    TextBoxColorChange("txtCheckNo", false);
    TextBoxColorChange("txtAuthNo", false);
    TextBoxColorChange("txtPaymentAmount", false);
    DateTimePickerColorChange("dtpCheckDate", false);
    DateTimePickerColorChangeForWindows("dtpCheckDate", false);
    ChangeLabelStyle("lblPaymentAmount", true);
    ChangeLabelStyle("lblCheckNo", true);
    TextBoxColorChange("txtPaymentNote", false);
}

function TextBoxColorChange(txtbox, bToNormal) {
    if (bToNormal == false) {
        document.getElementById(txtbox).readOnly = true;
        document.getElementById(txtbox).style.backgroundColor = "#BFDBFF";


    }
    else {
        document.getElementById(txtbox).readOnly = false;
        document.getElementById(txtbox).style.backgroundColor = "white";


    }
}

function ComboBoxColorChange() {
    if (bToNormal == false) {
        combobox.Enabled = false;
        combobox.CssClass = "nonEditabletxtbox";

    }
    else {
        combobox.Enabled = true;
        combobox.CssClass = "Editabletxtbox";

    }
}


function PaymentInformationClearAll() {
    document.getElementById("txtAuthNo").value = "";
    document.getElementById("txtPaymentAmount").value = "";
    document.getElementById("txtCheckNo").value = "";


    document.getElementById("txtPaymentNote").value = "";
    document.getElementById("txtPastDue").value = "";
}

function paymentamountenable() {
    TextBoxColorChange("txtPaymentAmount", true);
    ChangeLabelStyle("lblPaymentAmount", false);
    TextBoxColorChange("txtPaymentNote", true);
}

function ChangeLabelStyle(lbl, bIstoNormal) {
    if (bIstoNormal == true) {
        document.getElementById(lbl).value = document.getElementById(lbl).innerText.replace('*', "");
        document.getElementById(lbl).style.color = "black";
    }
    else {
        var str = document.getElementById(lbl).innerText;
        if (str.indexOf('*') == -1)
            document.getElementById(lbl).innerText = document.getElementById(lbl).innerText + "*";
        document.getElementById(lbl).style.color = "red";
    }
}
function DateTimePickerColorChangeForWindows(datetimepicker, bToNormal) {
    if (document.getElementById(datetimepicker).id != "dtpPatientDOB" && document.getElementById(datetimepicker).id != "dtpEffectiveStartDate") {
        if (bToNormal == false) {

            document.getElementById(datetimepicker).disabled = true;
            document.getElementById(datetimepicker).value = "";
        }
        else {

            document.getElementById(datetimepicker).disabled = false;
            if (document.getElementById(datetimepicker).id == "dtpCheckDate") {
                document.getElementById(datetimepicker).value = "";
            }
        }
    }
    else {
        if (bToNormal == false) {

            document.getElementById(datetimepicker).disabled = false;
        }
        else {

            document.getElementById(datetimepicker).disabled = false;
        }
    }
}

function DateTimePickerColorChange(datetimepicker, bToNormal) {
    if (bToNormal == false) {

        document.getElementById(datetimepicker).disabled = true;

    }
    else {

        document.getElementById(datetimepicker).disabled = false;

    }
}

function AddPaymentValidation() {
    if (document.getElementById("cboMethodOfPayment").value.length == 0) {
        return false;

    }
    if (document.getElementById("gbPaymentInformation") != null) {
        if (document.getElementById("txtCheckNo").readOnly == false && document.getElementById("txtCheckNo").value.length == 0) {


            DisplayErrorMessage('380021');
            document.getElementById("txtCheckNo").focus();
            return false;
        }
        if (document.getElementById("txtPaymentAmount").readOnly == false) {
            if (document.getElementById("txtPaymentAmount").value.length == 0) {


                DisplayErrorMessage('380020');
                document.getElementById("txtPaymentAmount").Focus();
                return false;
            }
            else if (parseFloat(document.getElementById("txtPaymentAmount").value) <= Convert.ToDecimal(0)) {


                DisplayErrorMessage('380020');
                document.getElementById("txtPaymentAmount").Focus();
                return false;
            }
        }

    }

}

function OpenUploadDocuments() {
    var now = new Date();
    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
    document.getElementById(GetClientId("hdnLocalTime")).value = utc;
    setTimeout(
         function () {
             var dateonclient = new Date;
             var Tz = (dateonclient.getTimezoneOffset());
             document.cookie = "Tz=" + Tz;
             var oWnd = GetRadWindow();
             localStorage.setItem("IndexingScreenMode", "Bulk Scanning and Fax");
             var childWindow = oWnd.BrowserWindow.radopen("frmIndexing.aspx?HumanId=" + document.getElementById("hdnHumanID").value + "&Screen=Appointments" + "&Title=Upload Documents" + "&CurrentTime=" + utc + "&ScreenMode=Bulk Scanning and Fax", "RadOnlineWindow");
             setRadWindowProperties(childWindow, 880, 1200);
         }, 0);
    return false;
}

function AllowAmount(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46) {
        return false
    }
    if (document.getElementById(evt.id).value.indexOf('.') != -1 && charCode == 46)
        return false;
    if (document.getElementById(evt.id).value == '' && charCode == 46) {
        return false;
    }
    AutoSave(evt);
    return true;
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


function enableAdd() {
    document.getElementById('btnAdd').disabled = false;

}
function SendEmailValidation() {

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    if (document.getElementById("txtMail").value.length == 0) {
        DisplayErrorMessage('380038');
        document.getElementById("txtMail").focus();
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }

    if (document.getElementById("txtMail").value.length != 0) {

        if (IsEmail(document.getElementById("txtMail").value) == false) {
            DisplayErrorMessage('320010');
            document.getElementById(GetClientId("txtMail")).focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

    }

    if (document.getElementById("txtPatientLastName").readOnly == false) {
        if (document.getElementById("txtPatientLastName").value.length == 0) {

            DisplayErrorMessage('380010');
            document.getElementById("txtPatientLastName").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("txtPatientFirstName").value.length == 0) {


            DisplayErrorMessage('380011');
            document.getElementById("txtPatientFirstName").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("dtpPatientDOB").value == "__-___-____") {


            DisplayErrorMessage('380012');
            document.getElementById("dtpPatientDOB").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (DOBValidation("dtpPatientDOB") == false) {

            DisplayErrorMessage('380002');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("cboPatientSex").value.length == 0) {


            DisplayErrorMessage('380013');
            document.getElementById("cboPatientSex").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtCellPhno").value.length != 0 && PhNoValid("msktxtCellPhno") == false && document.getElementById("msktxtCellPhno").value != "(___) ___-____") {


            DisplayErrorMessage('380031');
            document.getElementById("msktxtCellPhno").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtHomePhno").value.length != 0 && PhNoValid("msktxtHomePhno") == false && document.getElementById("msktxtHomePhno").value != "(___) ___-____") {


            DisplayErrorMessage('380032');
            document.getElementById("msktxtHomePhno").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        if (document.getElementById("msktxtSSN").value.length != 0 && document.getElementById("msktxtSSN").value != "___-__-____") {
            var str = document.getElementById("msktxtSSN").value;
            if (str.replace(/_/gi, "").length < 11) {


                DisplayErrorMessage('380039');
                document.getElementById("msktxtSSN").focus();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }

        if (document.getElementById("msktxtZipcode").value == '_____-____') {
            DisplayErrorMessage('380044');
            document.getElementById("msktxtZipcode").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false
        }

        if (document.getElementById("msktxtZipcode").value.length == 0) {
            DisplayErrorMessage('380044');
            document.getElementById("msktxtZipcode").focus();
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false
        }
        if (document.getElementById("msktxtZipcode").value.length != 0 && document.getElementById("msktxtZipcode").value != "_____-____") {
            var str = document.getElementById("msktxtZipcode").value;
            if (str.replace(/_/gi, "").length != 6 && str.replace(/_/gi, "").length != 10) {

                DisplayErrorMessage('380037');
                document.getElementById("msktxtZipcode").focus();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                return false;
            }
        }
    }
}
var dtLocal = new Date();
document.getElementById("hdnPCTime").value = dtLocal.toLocaleDateString();

function ShowLoading() {
    document.getElementById('divLoading').style.display = "block";
}

function dtpPatientDOB_OnDateSelected(sender, args) {
    AutoSave(sender);
}
function dtpCheckDate_OnDateSelected(sender, args) {
    AutoSave(sender);
}

function dtpTerminationDate_OnDateSelected(sender, args) {
    AutoSave(sender);
}
function dtpEffectiveStartDate_OnDateSelected(sender, args) {
    AutoSave(sender);
} function setRadWindowProperties(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

}

function setRadWindowPropertiesEV(childWindow, height, width) {
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(width, height);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

}

function AllowAlphabet(e) {
    AutoSave(e);
    isIE = document.all ? 1 : 0

    keyEntry = event.keyCode;
    if (((keyEntry >= '65') && (keyEntry <= '90')) || ((keyEntry >= '97') && (keyEntry <= '122')) || (keyEntry == '46') || (keyEntry == '32') || keyEntry == '45')
        return true;
    else {
        alert('Please Enter Only Character values.');
        return false;
    }
}

function NumberOnly(txtbox) {
    AutoSave(txtbox);
    var e = event || evt;
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function QPCDateValidation(sender, args) {
    var EnteredDateLength = parseInt(args._newValue.replace("-", "").replace("-", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").length);
    if (EnteredDateLength != 9 && EnteredDateLength > 0) {
        alert("Please Enter the Date Fully.")
        sender.clear();
        document.getElementById(sender._clientID).focus();
        return false;
    }
    if (EnteredDateLength == 9) {


        validatedate(document.getElementById(sender._clientID).value, document.getElementById(sender._clientID));
        if (sender._clientID != "dtpCheckDate") {
            DOBValidationWithTwo(document.getElementById(sender._clientID).value, document.getElementById(GetClientId('dtpPatientDOB')), sender);
        }
        if (sender._clientID == "dtpEffectiveStartDate") {
            validatetermdate();
        }
        if (sender._clientID == "dtpPatientDOB") {
            AutoSave(sender);
        }

    }
    $(document.getElementById(sender._clientID)).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(sender._clientID).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(sender._clientID)).click(function () {
        $(document.getElementById(sender._clientID)).focus();
    });


}
function QPCTermDateValidation(sender, args) {
    var EnteredDateLength = parseInt(args._newValue.replace("-", "").replace("-", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").replace("_", "").length);
    if (EnteredDateLength != 9 && EnteredDateLength > 0) {
        alert("Please Enter the Date Fully.")
        sender.clear();
        document.getElementById(sender._clientID).focus();
        return false;
    }
    if (EnteredDateLength == 9) {
        termdatevalidationDateValidation(document.getElementById(sender._clientID).value, document.getElementById(GetClientId('dtpPatientDOB')), sender);
    }


}
function validatedate(inputText, ControlId) {
    var FormatDDMMMYYYY = /(\d+)-([^.]+)-(\d+)/;
    if (inputText.match(FormatDDMMMYYYY)) {
        var DateMonthYear = inputText.split('-');
        lopera2 = DateMonthYear.length;
        var DateInput = parseInt(DateMonthYear[0]);
        var Year = parseInt(DateMonthYear[2]);
        var Month = "";
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        var ListofMonth = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
        if (ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) != -1) {

            Month = ListofMonth.indexOf(DateMonthYear[1].toUpperCase()) + 1;

            if (Month == 1 || Month > 2) {
                if (DateInput > ListofDays[Month - 1]) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId.id)).clear();
                    $find(GetClientId(ControlId.id)).focus(true);
                    return false;
                }
            }

            if (Month == 2) {
                var lyear = false;
                if ((!(Year % 4) && Year % 100) || !(Year % 400)) {
                    lyear = true;
                }
                if ((lyear == false) && (DateInput >= 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId.id)).clear();
                    $find(GetClientId(ControlId.id)).focus(true);
                    return false;

                }
                if ((lyear == true) && (DateInput > 29)) {
                    alert('Invalid date format!');
                    $find(GetClientId(ControlId.id)).clear();
                    $find(GetClientId(ControlId.id)).focus(true);
                    return false;
                }
            }

            var CurrentDate = new Date();
            var CurrentYear = CurrentDate.getFullYear();

            if (ControlId.id != 'dtpPatientDOB' && ControlId.id != 'dtpCheckDate') {
                if (Year > CurrentYear) {
                    alert("Effective Start Date can't be future Date");
                    $find(GetClientId(ControlId.id)).clear();
                    $find(GetClientId(ControlId.id)).focus(true);
                    return false;
                }
            }
            else {
                if (document.getElementById("dtpPatientDOB").value != '__-___-____') {
                    var Dobdate = parseMyDate(document.getElementById("dtpPatientDOB").value);
                }

                if (Dobdate > CurrentDate) {
                    alert("The Date of Birth you have entered is in the future. Please enter a valid day, month, and year.");
                    $find(GetClientId(ControlId.id)).clear();
                    $find(GetClientId(ControlId.id)).focus(true);
                    return false;
                }

            }
        }

        else {
            alert('Invalid date format!');
            $find(GetClientId(ControlId.id)).clear();
            $find(GetClientId(ControlId.id)).focus(true);
            return false;
        }
    }
}
function Addcursor() {
    showTime();
    return true;
}
//function confirmMessage() {
//    var now = new Date(); var then = now.getDay() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear(); then += ' ' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds(); var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds(); document.getElementById("hdnLocalTime").value = utc;
//    if (window.confirm("Do you Want to delete the payment?")) {
//        return true;
//    }
//    else {
//        return false;
//    }
//}
function CalAmt(ctrl) {
    if (document.getElementById("hdnScreenMode").value != "COLLECT COPAY") {
        if (ctrl != undefined && ctrl.readOnly != true) {
            //CAP-795 Cannot set properties of null
            if (document?.getElementById("btnSave") != undefined && document?.getElementById("btnSave") != null) {
                document.getElementById("hdnBSave").value = true;
                document.getElementById("btnSave").disabled = false;
            }
            AmtCalc();
        }
    }
}
function AmtCalc() {
    //if (document.getElementById("btnAdd").innerText != "Update") {
    //    var PaymentAmount = 0;
    //    var RecAmt = 0;
    //    var RefundAmount = 0;
    //    var totalAmount = 0;
    //    if (document.getElementById("hdnTotalPayment").value != "") {
    //        totalAmount = parseFloat(document.getElementById("hdnTotalPayment").value);
    //    }
    //    if (document.getElementById("txtPaymentAmount").value != "") {
    //        PaymentAmount = parseFloat(document.getElementById("txtPaymentAmount").value);
    //    }
    //    if (document.getElementById("txtRefundAmount").value != "") {
    //        RefundAmount = parseFloat(document.getElementById("txtRefundAmount").value);
    //    }
    //    if (document.getElementById("txtRecOnAcc").value != "")
    //        RecAmt = parseFloat(document.getElementById("txtRecOnAcc").value);
    //    var Pay = ((PaymentAmount + RecAmt) - (RefundAmount));
    //    document.getElementById("txtTotalAmount").value = (totalAmount + Pay).toString();
    //}
}
function setTotalPayment()
{
    document.getElementById("txtTotalAmount").value = document.getElementById("hdnTotalPayment").value;
}

function DOBValidationWithTwo(inputText, ControlId, sender) {
    if (ControlId.value != '__-___-____' && ControlId.value != undefined) {
        var DOB = parseMyDate(ControlId.value);
    }
    if (inputText != '__-___-____' && inputText != undefined) {
        var INPUTDate = parseMyDate(inputText);
    }

    if (DOB > INPUTDate) {
        alert("Effective Start Date can't Be lesser then DOB");
        document.getElementById("dtpEffectiveStartDate").focus();
        sender.clear();
        return false;
    }




}

function termdatevalidationDateValidation(inputText, ControlId, sender) {
    var DOB = parseMyDate(ControlId.value);
    var INPUTDate = parseMyDate(inputText);
    if (document.getElementById(GetClientId('dtpEffectiveStartDate')).value != '__-___-____') {
        var effsdate = parseMyDate(document.getElementById(GetClientId('dtpEffectiveStartDate')).value);
    }
    if (DOB > INPUTDate) {
        alert("Termination Date can't Be lesser then DOB");
        sender.focus();
        sender.clear();
        return false;
    }
    else if (effsdate > INPUTDate) {
        alert("Termination Date can't Be lesser then Effcetive start Date");
        sender.focus();
        sender.clear();
        return false;
    }
}





function DefaultCopay() {
    var copay = document.getElementById('txtPaymentAmount').value;
    if (copay == "" || parseFloat(copay) == 0)
        document.getElementById('txtPaymentAmount').value = '0.00'
}

function parseMyDate(s) {
    var m = ['jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec'];
    var match = s.match(/(\d+)-([^.]+)-(\d+)/);
    var date = match[1];
    var monthText = match[2];
    var year = match[3];
    var month = m.indexOf(monthText.toLowerCase());
    return new Date(year, month, date);
}

function Validatealphanumaeric() {
    var textvalue = document.getElementById('txtPolicyHolderID').value;
    var myRegEx = /^[-0-9a-z]*$/i;
    var isValid = !(myRegEx.test(textvalue));


    if (!textvalue.match(myRegEx)) {

        DisplayErrorMessage('8008');
        document.getElementById('txtPolicyHolderID').value = "";
        document.getElementById('txtPolicyHolderID').focus();
        return false;
    }
    else {

        return true;
    }

}

function Validatealphanumaericforpayer() {
    var textvalue = document.getElementById('txtOtherPayerName').value;
    var myRegEx = /^[-0-9a-z t]*$/i;
    var isValid = !(myRegEx.test(textvalue));


    if (!textvalue.match(myRegEx)) {

        DisplayErrorMessage('8009');
        document.getElementById('txtOtherPayerName').value = "";
        document.getElementById('txtOtherPayerName').focus();
        return false;
    }
    else {

        return true;
    }

}
function ValidatealphanumaericforOtherPlan() {
    var textvalue = document.getElementById('txtOtherPlan').value;
    var myRegEx = /^[-0-9a-z t]*$/i;
    var isValid = !(myRegEx.test(textvalue));


    if (!textvalue.match(myRegEx)) {

        DisplayErrorMessage('80010');
        document.getElementById('txtOtherPlan').value = "";
        document.getElementById('txtOtherPlan').focus();
        return false;
    }
    else {

        return true;
    }

}

function DefaultCopayforrec() {
    var copay = document.getElementById('txtRecOnAcc').value;
    if (copay == "" || parseFloat(copay) == 0)
        document.getElementById('txtRecOnAcc').value = '0.00';
}

function DefaultCopayforrefund() {
    var copay = document.getElementById('txtRefundAmount').value;
    if (copay == "" || parseFloat(copay) == 0)
        document.getElementById('txtRefundAmount').value = '0.00';
}

function validatetermdate() {
    if (document.getElementById('dtpEffectiveStartDate').value != "__-___-____" && document.getElementById('dtpTerminationDate').value != "__-___-____") {
        var startdate = parseMyDate(document.getElementById('dtpEffectiveStartDate').value);
        var termdate = parseMyDate(document.getElementById('dtpTerminationDate').value);

        if (startdate > termdate) {
            DisplayErrorMessage('380005');
        }
    }
}

function funEV() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    showTime();
}
function Payer(ctrl) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    AutoSave(ctrl);
}

function warningmethod() {


    $("span[mand=Yes]").each(function () {
        if ($(this).html().indexOf("*")) {
            $("span[mand=Yes]").addClass('MandLabelstyle');
            $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
        }
    });
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}



function OpenAuthorization() {
    if (document.getElementById(GetClientId('txtPatientAccountNo')).value.trim() != undefined && document.getElementById(GetClientId('txtPatientAccountNo')).value.trim() != "" && document.getElementById(GetClientId('txtPatientAccountNo')).value.trim() != "0") {
        setTimeout(
function () {
    var oWnd = GetRadWindow();
    var childWindow = oWnd.BrowserWindow.radopen("frmAuthorization.aspx?Human_Id=" + document.getElementById(GetClientId('txtPatientAccountNo')).value, "RadOnlineWindow");
    setRadWindowProperties(childWindow, 880, 1200);
    childWindow.add_close(CloseAuthScreen);
}, 0);
        return false;
    }
    else {
        DisplayErrorMessage('1011161');
        return false;
    }
}
function CloseAuthScreen(oWindow, args) {
    setCPtvalue();
}
$(document).ready(function () {
    //document.getElementById("Button1").style.visibility = "hidden";
    var vresult = window.location.href;
    if (vresult.split('&')[4] != undefined) {
        if (vresult.split('&')[4].split('sScreenMode=')[1] == "PATIENT%20SUMMARY") {
            $('#btnAdd').prop("disabled", true);
            localStorage.setItem("PatientSummary", "Y");
            $("#cboPatientSuffix").prop("disabled", true);
            $('#cboPatientSuffix').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
        }
    }
    $(document.getElementById(GetClientId("dtpPatientDOB"))).datepicker({
        dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, maxDate: new Date(), yearRange: "-120:+0",
        onSelect: function (selected, evnt) {
            $telerik.findMaskedTextBox(GetClientId("dtpPatientDOB")).set_value(selected);
            AutoSave();
        }
    });
    $(document.getElementById(GetClientId("dtpPatientDOB"))).click(function () {
        $(document.getElementById(GetClientId("dtpPatientDOB"))).focus();
    });

});

//Cap - 669
function btnEditName_Click() {   
    $(document.getElementById("dtpPatientDOB")).click(function () {
        $(document.getElementById("dtpPatientDOB")).focus();
    });
    setTimeout(function () {
        $("#dtpPatientDOB").datepicker(
            {
                dateFormat: 'dd-M-yy', changeYear: true, changeMonth: true, maxDate: new Date(), yearRange: "-120:+0",
                onSelect: function (selected, evnt) {
                    $telerik.findMaskedTextBox("dtpPatientDOB").set_value(selected);
                    AutoSave();
                }
            }
        );
    }, 300);
    
}



function ClickUploadControl() {
    $("#fileupload").click();
}

function openPatInsurancewindowScreen() {
   // #GitlabId - 3601
    HumanId = document.getElementById(GetClientId("txtPatientAccountNo")).value;
    setTimeout(
        function () {
            var oWnd = GetRadWindow();
            var oManager = oWnd.get_windowManager();
            var childWindow = oManager.BrowserWindow.radopen("frmPatientDemographics.aspx?HumanId=" + HumanId, 1230, 1130, "ModalWindow");
            SetRadWindowProperties(childWindow, 1230, 1130);
            childWindow.add_close(OpenPatIns);
            childWindow.remove_close(AddGuarantorClick);
            childWindow.remove_close(ViewGaurantorClick);
            childWindow.remove_close(OpenAddInsForNewPatient);
            childWindow.remove_close(FindPatientClick);
            childWindow.remove_close(CloseWorksetClick);    
        }, 0);
    //Commented By Deepak #GitlabId-3601
  //  var NoofPolicy = $('#grdExistingPolicies tr.aspSelectedRow').length;
  //  HumanId = document.getElementById(GetClientId("txtPatientAccountNo")).value;
  //  if (parseInt(NoofPolicy) > 0) {
  //      if (HumanId) {
  //          var obj = new Array();
  //          obj.push("HumanId=" + HumanId);
  //          obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
  //          setTimeout(
  //          function () {
  //              var oWnd = GetRadWindow();
  //              var oManager = oWnd.get_windowManager();
  //              var childWindow = oManager.BrowserWindow.radopen("frmPatientInsurancePolicyMaintenance.aspx?HumanId=" + HumanId + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
  //              SetRadWindowProperties(childWindow, 590, 1160);
  //              childWindow.add_close(OpenPatIns);
  //              childWindow.remove_close(AddGuarantorClick);
  //              childWindow.remove_close(ViewGaurantorClick);
  //              childWindow.remove_close(OpenAddInsForNewPatient);
  //              childWindow.remove_close(FindPatientClick);
  //              childWindow.remove_close(CloseWorksetClick);
  //          }, 0);
  //      }
  //  }
  //  else {
  //      ulpatientid = document.getElementById(GetClientId("hdnPatientID")).value
  //      objhumanid = document.getElementById(GetClientId("hdnHumanID")).value
  //      txtPatientlastname = document.getElementById(GetClientId("txtPatientLastName")).value
  //      txtPatientfirstname = document.getElementById(GetClientId("txtPatientFirstName")).value
  //      txtExternalAccNo = document.getElementById(GetClientId("txtExternalAccNo")).value
  //      if (parseInt(ulpatientid) == 0) {
  //          var obj = new Array();
  //          obj.push("HumanId=" + objhumanid);
  //          obj.push("InsuranceType=" + true);
  //          obj.push("LastName=" + txtPatientlastname);
  //          obj.push("FirstName=" + txtPatientfirstname);
  //          obj.push("ExAccountNo=" + txtExternalAccNo);
  //          obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
  //          setTimeout(
  //          function () {
  //              var oWnd = GetRadWindow();
  //              var oManager = oWnd.get_windowManager();
  //              var childWindow = oManager.BrowserWindow.radopen("frmAddInsurancePolicies.aspx?HumanId=" + objhumanid + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname + "&FirstName=" + txtPatientfirstname + "&ExAccountNo=" + txtExternalAccNo + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
  //              SetRadWindowProperties(childWindow, 850, 1140);
  //              childWindow.add_close(OpenAddInsForNewPatient);
  //              childWindow.remove_close(OpenPatIns);
  //              childWindow.remove_close(AddGuarantorClick);
  //              childWindow.remove_close(ViewGaurantorClick);
  //              childWindow.remove_close(FindPatientClick);
  //              childWindow.remove_close(CloseWorksetClick);
  //          }, 0);
  //      }
  //      else {
  //          var obj = new Array();
  //          obj.push("HumanId=" + HumanId);
  //          obj.push("InsuranceType=" + true);
  //          obj.push("LastName=" + txtPatientlastname);
  //          obj.push("FirstName=" + txtPatientfirstname);
  //          obj.push("ExAccountNo=" + txtExternalAccNo);
  //          obj.push("PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value);
  //          setTimeout(
  //function () {
  //    var oWnd = GetRadWindow();
  //    var oManager = oWnd.get_windowManager();
  //    var childWindow = oManager.BrowserWindow.radopen("frmAddInsurancePolicies.aspx?HumanId=" + HumanId + "&InsuranceType=" + true + "&LastName=" + txtPatientlastname + "&FirstName=" + txtPatientfirstname + "&ExAccountNo=" + txtExternalAccNo + "&PatientType=" + document.getElementById(GetClientId("hdnPatientType")).value + "&EncounterId=" + document.getElementById(GetClientId("hdnEncounterID")).value, "ctl00_DemographicsModalWindow");
  //    SetRadWindowProperties(childWindow, 850, 1140);
  //    childWindow.add_close(OpenAddInsForNewPatient);
  //    childWindow.remove_close(OpenPatIns);
  //    childWindow.remove_close(AddGuarantorClick);
  //    childWindow.remove_close(ViewGaurantorClick);
  //    childWindow.remove_close(FindPatientClick);
  //    childWindow.remove_close(CloseWorksetClick);
  //}, 0);
  //      }
  //  }
    return false;
}

function OpenAddInsForNewPatient(oWindow, args) {
    document.getElementById(GetClientId("hdnBtnLoadInsurance")).click();
    document.getElementById(GetClientId("btnloadgrid")).click();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}

function OpenPatIns(oWindow, args) {

    document.getElementById(GetClientId("hdnBtnLoadInsurance")).click();
    document.getElementById(GetClientId("btnloadgrid")).click();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}


function FindPatientClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("hdnPatientID")).value = Result.HumanId;
        document.getElementById(GetClientId("btnFindpatientClick")).click();
    }
}

function CloseWorksetClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result != null) {
        returnToParent(args);
    }
}


function AddGuarantorClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("hdnGuarantorID")).value = Result.HumanID;
        document.getElementById(GetClientId("btnSave")).disabled = false; 
        document.getElementById(GetClientId("hdnSaveFlag")).value = true;
        document.getElementById(GetClientId("btnAddGuarantorRefresh")).click();
    }
}

function ViewGaurantorClick(oWindow, args) {
    var Result = args.get_argument();
    if (Result) {
        document.getElementById(GetClientId("hdnGuarantorIdForView")).value = Result.GuarantorId;
    }
    document.getElementById(GetClientId("hdnBtnLoadInsurance")).click();

}


function SelectGaurantorClick(oWindow, args) {
    var Result = args.get_argument();
    var name = Result.PatientName;
    var dob = Result.PatientDOB;
    var sex = Result.PatientGender;
    var status = Result.Status;
    var cell_phone = Result.Cell_Phone;
    var home_phone = Result.Home_Phone;
    var zipcode = Result.ZipCode;
    var email = Result.EMail;
    if (Result.Address != null)
        var address = Result.Address;

    if (Result) {

        document.getElementById(GetClientId("hdnGuarantorID")).value = Result.HumanId;
        if (name != undefined && name.split(' ')[0].split(',')[1] != undefined)
            document.getElementById(GetClientId("txtGuarantorFirstName")).value = name.split(' ')[0].split(',')[1]
        if (name != undefined && name.split(' ')[0] != undefined)
            document.getElementById(GetClientId("txtGuarantorLastName")).value = name.split(',')[0];
        if (name != undefined && name.split(' ')[1] != undefined)
            document.getElementById(GetClientId("txtGuarantorMiddleName")).value = name.split(' ')[1];
        if (dob != undefined && dob.split(' ')[0] != undefined)
            document.getElementById(GetClientId("dtpGuarantorDOB")).value = dob.split(' ')[0];
        if (sex != undefined)
            document.getElementById(GetClientId("ddlGuarantorSex")).value = Result.PatientGender;
        if (status != undefined)
            document.getElementById(GetClientId("ddlPatientStatus")).value = Result.Status;
        if (cell_phone != undefined)
            document.getElementById(GetClientId("msktxtGuarantorCellNo")).value = Result.Cell_Phone;
        if (home_phone != undefined)
            document.getElementById(GetClientId("msktxtGuarantorHomeNo")).value = Result.Home_Phone;
        if (address != undefined && address.split(',')[0] != undefined)
            document.getElementById(GetClientId("txtGuarantorAddress")).value = address.split(',')[0];
        if (address != undefined && address.split(',')[0] != undefined)
            document.getElementById(GetClientId("txtGuarantorAddressLine2")).value = address.split(',')[0];
        if (address != undefined && address.split(',')[1] != undefined)
            document.getElementById(GetClientId("txtGuarantorCity")).value = address.split(',')[1];
        if (address != undefined && address.split(',')[2] != undefined)
            document.getElementById(GetClientId("ddlGuarantorState")).value = address.split(',')[2];
        if (zipcode != undefined)
            document.getElementById(GetClientId("msktxtGuarantorZipCode")).value = Result.ZipCode;
        if (email != undefined)
            document.getElementById(GetClientId("txtGuaEmail")).value = Result.EMail;


        document.getElementById(GetClientId("btnSave")).disabled = false;
        document.getElementById(GetClientId("hdnSaveFlag")).value = true;
        document.getElementById(GetClientId("btnAddGuarantorRefresh")).click();

    }

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

function onlyDotsAndNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode == 46) {
        AutoSave(this);
        return true;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    AutoSave(this);
    return true;
}

function onlyNumbers(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    AutoSave(this);
    return true;
}

function ClosesaveWindow() {
    if (document.getElementById("btnAdd") != null) {
        if (document.getElementById("btnAdd").disabled == false) {

            if (document.getElementById("hdnMessageType").value == "") {
                DisplayErrorMessage('380049');
            }
            else if (document.getElementById("hdnMessageType").value == "Yes") {
                document.getElementById("btnAdd").click();
                document.getElementById("hdnMessageType").value = "";
                return false;
            }
            else if (document.getElementById("hdnMessageType").value == "No") {
                document.getElementById("hdnMessageType").value = ""
                returnToParent(null);
            }
            else if (document.getElementById("hdnMessageType").value == "Cancel") {
                document.getElementById("hdnMessageType").value = "";
            }

        }
        else {
            returnToParent(null);
        }
    }
    else
        returnToParent(null);
    return false;
}


function btnPrintRecipt_Window() {
    document.getElementById(GetClientId("hdnEncounterID")).value = document.getElementById(GetClientId("hdnEncounterID")).value;

}

function PrintReciptClick() {
    document.getElementById(GetClientId("btnRefresh")).click();
}


function OpenPrintRecipt_Window(FaxSubject) {
    var hum_id = document.getElementById("hdnHumID").value

    setTimeout(
       function () {
           var oWnd = GetRadWindow();
           var oManager = oWnd.get_windowManager();
           var childWindow = oManager.BrowserWindow.radopen("frmPrintPDF.aspx?&SI=" + document.getElementById("hdnFileName").value + "&Location=DYNAMIC&ButtonName=Print Receipt&PageTitle=Print Receipt" + "&FaxSubject=" + FaxSubject, "ctl00_DemographicsModalWindow");
           SetRadWindowProperties(childWindow, 750, 1050);

       }, 0);
}

function btnClearAll_Clicked(sender, args) {
    var IsClearAll = DisplayErrorMessage('180010');

}


function OnClosePatientPayementsAccountClick(oWindow, args) {
    oWindow.Close();
}

function ShowMessageforPatientPayment() {
    if (document.getElementById("btnClear").value == "Clear All") {
        var ErrorMessage1 = window.confirm("Are you sure you want to clear all the fields in Payment information?");
        if (ErrorMessage1 == true) {
            PaymentInformationClear();
            document.getElementById("txtPaymentAmount").value = "0.00";
            document.getElementById("txtRecOnAcc").value = "0.00";
            document.getElementById("txtRefundAmount").value = "0.00";
            document.getElementById("txtPastDue").value = "0.00";
            document.getElementById("btnAdd").disabled = true;
            document.getElementById("txtCheckNo").value = '';
            document.getElementById("cboMethodOfPayment").selectedIndex = "0";

            if (document.getElementById("cboRelation").value == "Patient") {
                document.getElementById("txtpaidBy").value = divPatientstrip.innerHTML.split('|')[0];
                $('#txtpaidBy').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            }
            $('#spanPaymentNotes').removeClass('MandLabelstyle');
            $('#spanPaymentNotes').addClass('spanstyle');
            $('#spanPatientNotestar').css('visibility', 'hidden');
            document.getElementById("btnAdd").value = "Add";
            $('#txtRecOnAcc').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtRefundAmount').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtpaidBy').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtPaymentAmount').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtCheckNo').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtAuthNo').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#cboRelation').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#dtpCheckDate').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#txtPaymentNote').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            $('#cboMethodOfPayment').addClass('Editabletxtbox').removeClass('nonEditabletxtbox');
            document.getElementById("cboMethodOfPayment").disabled = false;
            document.getElementById("txtRecOnAcc").disabled = true;
            document.getElementById("txtRefundAmount").disabled = true;
            document.getElementById("txtpaidBy").disabled = true;
            document.getElementById("txtPaymentAmount").disabled = true;
            document.getElementById("txtCheckNo").disabled = true;
            document.getElementById("txtAuthNo").disabled = true;
            document.getElementById("cboRelation").disabled = true;
            document.getElementById("dtpCheckDate").disabled = true;
            document.getElementById("txtPaymentNote").disabled = true;
            document.getElementById("dtpCheckDate").value = '';
            $('#spanCheck').removeClass('MandLabelstyle');
            $('#spanCheck').addClass('spanstyle');
            $('#spanCheckStar').css('visibility', 'hidden');
            //return false;
            return true;

        }
        else {
            return false;
        }
    }
    else {
        var ErrorMessage2 = window.confirm("Are you sure you want to Cancel?");
        if (ErrorMessage2 == true) {
            PaymentInformationClear();
            document.getElementById("txtPaymentAmount").value = "0.00";
            document.getElementById("txtRecOnAcc").value = "0.00";
            document.getElementById("txtRefundAmount").value = "0.00";
            document.getElementById("txtPastDue").value = "0.00";
            document.getElementById("btnAdd").disabled = true;
            document.getElementById("txtCheckNo").value = '';
            document.getElementById("cboMethodOfPayment").selectedIndex = "0";
            if (document.getElementById("cboRelation").value == "Patient") {
                document.getElementById("txtpaidBy").value = divPatientstrip.innerHTML.split('|')[0];
                $('#txtpaidBy').addClass('nonEditabletxtbox').removeClass('Editabletxtbox');
            }
            $('#cboMethodOfPayment').addClass('Editabletxtbox').removeClass('nonEditabletxtbox');
            document.getElementById("cboMethodOfPayment").disabled = false;
            $('#spanPaymentNotes').removeClass('MandLabelstyle');
            $('#spanPaymentNotes').addClass('spanstyle');
            $('#spanPatientNotestar').css('visibility', 'hidden');
           // document.getElementById("btnAdd").value = "Add";
             return true;
        }

        else {
            return false;
        }

    }
}


function savesuccessfully() {
    DisplayErrorMessage('380034');
    return false;
}

var sFromDate = "";
var sToDate = "";
var pFromDate = "";
var pToDate = "";
if ($('#dtpFromDate').val() != undefined && $('#dtpFromDate').val() != "") {
    sFromDate = new Date($('#dtpFromDate').val()).dateFormat("Y-m-d");
    pFromDate = "From Date: " + new Date($('#dtpFromDate').val()).dateFormat("d-M-Y");
}
else {
    sFromDate = "2010-01-01";
    pFromDate = "From Date: 01-Jan-2010";
}
if ($('#dtpToDate').val() != undefined && $('#dtpToDate').val() != "") {
    sToDate = new Date($('#dtpToDate').val()).dateFormat("Y-m-d");
    pToDate = "To Date: " + new Date($('#dtpToDate').val()).dateFormat("d-M-Y");
}
else {
    sToDate = "2050-12-31";
    pToDate = "To Date: 31-Dec-2050";
}


function OpenFinancialReport() {
    var sPatientID = '';
    var PatientName = '';
    var FacilityName = document.getElementById("hdnFacilityRole").value.replace("#", "%23");
    if (document.getElementById("divPatientstrip").innerText != "" && document.getElementById("divPatientstrip").innerText != undefined)
        sPatientID = document.getElementById("divPatientstrip").innerHTML.split('|')[4].split(':')[1].trim();
    if (document.getElementById("divPatientstrip").innerText != "" && document.getElementById("divPatientstrip").innerText != undefined)
        PatientName = document.getElementById("divPatientstrip").innerHTML.split('|')[0].trim();
    //var sParameters = "FromDate: " + sFromDate + " , " + "ToDate: " + sToDate + " , " + "Facility: " + FacilityName;
    var sParameters = "(Patient Payments)";

    $.ajax({
        type: "POST",
        url: "frmPatientPayment.aspx/OpenFinancialReport",
        data: JSON.stringify({
            "strPatient": PatientName,
            "strParameter": sParameters,
            "strFromDate": sFromDate,
            "strToDate": sToDate,
            "strPatientID": sPatientID,
            "strFacility": FacilityName
        }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        success: function (path) {
            debugger;
            var ReportLoadList = JSON.parse(path.d);
            sBIRTReportUrl = ReportLoadList.BIRTUrl;
            sDBConnection = ReportLoadList.DBConnection;
            //var reportname = "PATIENT_FINANCIAL_STATEMENT_REPORT";
            //ReportUrl = sBIRTReportUrl + "_" + reportname + ".rptdesign" + sDBConnection;
           
            //var test = ReportUrl + "&ReportName=PATIENT TRANSACTIONS" + "&PatientName=" + PatientName + "&Parameters=" + sParameters + "&FromDate=" + sFromDate + "&ToDate=" + sToDate + "&PatientID=" + sPatientID + "&FacilityName=" + FacilityName + "&Arc=" + "Y" + "&__title=" + "PATIENT TRANSACTIONS";
            var test = sBIRTReportUrl + sDBConnection;;
            $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', "");
            $(top.window.document).find('#ProcessiFrameReport')[0].style.height = "126%";
            $(top.window.document).find("#ModalReport").modal({ backdrop: 'static', keyboard: false }, 'show');
            $(top.window.document).find("#mdlcontentReport")[0].style.width = "153%";
            $(top.window.document).find("#mdlcontentReport")[0].style.height = "58%";
            $(top.window.document).find("#mdldialogReport")[0].style.height = "128%";
            $(top.window.document).find("#mdlcontentReport").css({ "margin-left": "-27%", "margin-top": "15%" });
            $(top.window.document).find("#ProcessiFrameReport")[0].style.border = "1px solid #D0D0D0";
            $($(top.window.document).find('#ProcessiFrameReport')[0]).attr('src', test);
            $(top.window.document).find("#ModalReportTtle")[0].textContent = "";
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
    return false;
}

