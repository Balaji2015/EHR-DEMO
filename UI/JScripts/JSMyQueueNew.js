var Role;

var isproviderReview = false;

function loadGeneralQueue() {
    document.getElementById("divGeneralQ").style.display = "";
    document.getElementById("divMyQ").style.display = "none";
    $('#MyQTable').empty();
    $('#GeneralQTable').empty();
    $('#RefreshQ').css("background-color", "");
    $('#btnChkOut').css("background-color", "");
    $('#MoveTo').css("background-color", "");
    $('#MoveTo')[0].innerText = "Move To My Encounters";
    $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
    if (Role == "Medical Assistant" || Role == "Office Manager") {
        $('#Exam')[0].style.visibility = "visible";
        $('#lblEr')[0].style.visibility = "visible";
    }
    else {
        $('#Exam')[0].style.visibility = "hidden";
        $('#lblEr')[0].style.visibility = "hidden";
    }
    $('#btnChkOut')[0].style.visibility = "visible";
    $('#Processenc')[0].style.display = "none";
    $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

    $("#btnEnc").removeClass("default");
    $("#btnEnc").addClass("btncolorMyQ");
    $("#btnGeneral").removeClass("default");
    $("#btnGeneral").addClass("btncolorMyQ");
    $("#btnMyQ").removeClass("btncolorMyQ");
    $("#btnMyQ").addClass("default");

    $("#btnAmendmnt").removeClass("btncolorMyQ");
    $("#btnAmendmnt").addClass("default");
    $("#btnOrder").removeClass("btncolorMyQ");
    $("#btnOrder").addClass("default");

    $('#btnChkOut')[0].style.display = "";//BugID:48827
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.setTimeout(GenQLoad, 300);
}
$(document).ready(function () {
    localStorage.setItem("PrevSubTab", "");

    if ($("#MovetoNxtProcess") != null)
        $("#MovetoNxtProcess")[0].disabled = true;
    if (sessionStorage.getItem('Next_Order_Index') != undefined) { sessionStorage.removeItem('Next_Order_Index'); }
    if (sessionStorage.getItem('CloseNotification') != undefined) { sessionStorage.removeItem('CloseNotification'); }
    if (sessionStorage.getItem('bCCSave') != undefined) { sessionStorage.removeItem('bCCSave'); }
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    document.getElementById("divMyQ").style.display = "";
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $('#MyQTable').empty();
    $("#ProcessModal").modal('hide');
    var MyShowAllmyQueue = localStorage.getItem('MyShowAll');
    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    if (MyShowAllmyQueue == "Checked") {
        $("#chkMyShowAll")[0].checked = true;
        Showall = "Checked";
        loadMyenc();
    }
    else if (MyShowAll == "Checked") {
        $("#chkShowAll")[0].checked = true;
        Showall = "Checked";
        loadGeneralQueue();
    }
    else {
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/MyEncounterLoad",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {

                var objdata = $.parseJSON(data.d);
                Role = objdata.role;
                var Ancillary = objdata.Ancillary;
                if (objdata.role != "Medical Assistant" && objdata.role != "Front Office" && objdata.role != "Surgery Coordinator" && objdata.role != "Scribe") {//BugID:53790
                    $('#MyQTable').empty();
                    var tabContents = "";
                    let disableOverallSelect = true;
                    if (objdata.data.length > 0) {

                        for (var i = 0; i < objdata.data.length; i++) {
                            var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                            let disabled = "disabled='true'";
                            if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
                                disabled = "";
                                disableOverallSelect = false;
                            }
                            if (i == 0) {
                                if (Ancillary != "true")
                                    tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                            }
                            else {
                                if (Ancillary != "true")
                                    tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";

                            }
                        }
                        if (Ancillary != "true")
                            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                        else
                            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");

                    }
                    else {
                        if (Ancillary != "true")
                            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
                        else
                            $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");

                    }
                    //$('#EncounterTable th').addClass('header');
                    $("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";
                    $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.count[0].My_Task_Count + ")";
                    $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.count[0].My_Order_Count + ")";
                    $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.count[0].My_Scan_Count + ")";
                    $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.count[0].My_Presc_Count + ")";
                    $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.count[0].My_Amendmnt_Count + ")";

                    localStorage.setItem("Myorderscount", objdata.count[0].My_Order_Count);
                    if (objdata.EncounterCount != null && objdata.EncounterCount != undefined) {
                        $("#ctl00_C5POBody_lblcount").css('font-size', '11px');
                        $("#ctl00_C5POBody_lblcount")[0].innerHTML = 'Total encounters to be signed are<span style="color:red;"> ' + objdata.EncounterCount + '</span>. To view current as well as more than 7 days old encounters, click on "ShowAll".'

                    }
                    else
                        $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";

                    $("#btnMyEnc").removeClass("default");
                    $("#btnMyEnc").addClass("btncolorMyQ");
                    $("#btnMyQ").addClass("btncolorMyQ");
                    $("#MovetoNxtProcess").css("display", "inline-block");
                    //$('#EncounterTable th').addClass('header');
                    SortTableHeader('MyQ');
                    RowClick();
                    if (disableOverallSelect) {
                        disableSelectAllMove();
                    }
                }
                else {
                    $("#chkMyShowAll")[0].checked = false;
                    $("#chkShowAll")[0].checked = false;
                    var ShowAll = localStorage.getItem('ShowallGeneralqueue');
                    if (ShowAll == "Checked") {
                        $("#chkShowAll")[0].checked = true;
                    }
                    var MyShowAll = localStorage.getItem('MyShowAll');
                    if (MyShowAll == "Checked") {
                        $("#chkMyShowAll")[0].checked = true;
                    }
                    document.getElementById("divGeneralQ").style.display = "";
                    document.getElementById("divMyQ").style.display = "none";
                    $('#MyQTable').empty();
                    $('#GeneralQTable').empty();
                    $('#RefreshQ').css("background-color", "");
                    $('#btnChkOut').css("background-color", "");
                    $('#MoveTo').css("background-color", "");
                    $('#Processenc').css("background-color", "");
                    $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
                    $('#lblEr')[0].style.visibility = "visible";
                    $('#Exam')[0].style.visibility = "visible";
                    $('#btnChkOut')[0].style.visibility = "visible";
                    $('#Processenc')[0].style.visibility = "visible";
                    $('#Processenc')[0].style.width = "134px";
                    $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
                    $('#GeneralQTable').empty();
                    var tabContents; var eRoomList;
                    var objdata = $.parseJSON(data.d);
                    if (objdata.data.length > 0) {
                        for (var i = 0; i < objdata.data.length; i++) {
                            var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                            if (i == 0)
                                tabContents = "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:11%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:11%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                            else
                                tabContents = tabContents + "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:11%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:11%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        }
                        $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    }
                    else {
                        $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead></table>");
                    }
                    $("#btnEnc")[0].innerText = "Encounters Q " + "(" + objdata.data.length + ")";

                    $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.count[0].Order_Count + ")";
                    $("#btnAmendmnt")[0].innerText = "Amendment Q" + "(" + objdata.count[0].Amendmnt_Count + ")";

                    localStorage.setItem("GenralOrderCount", objdata.count[0].Order_Count);
                    $("#btnEnc").addClass("btncolorMyQ");
                    $("#btnGeneral").addClass("btncolorMyQ");
                    if (objdata.dataEroom != undefined && objdata.dataEroom.length > 0) {
                        if ($('select#Exam option').length == 0) { $.each(objdata.dataEroom, function (i, item) { $('#Exam').append($('<option>', { value: objdata.dataEroom[i], text: objdata.dataEroom[i] })); }); }
                    }
                    else {
                        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    }
                    RowClick();
                    //$('#EncounterTable th').addClass('header');
                    SortTableHeader('GeneralQ');

                }
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

    $('#btnChkOut').click(function () {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 1) {
            $('#GeneralQTable tr').removeClass("highlight");
            $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#GeneralQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            alert("Please Select one encounter to check out");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else if ($('#GeneralQTable').children().find('.highlight').length > 0) {
            var currentProcess = $('#EncounterTable > tbody > tr.highlight > td:nth-child(8)')[0].outerText.trim();
            if (currentProcess == 'CHECK_OUT') {
                var HumanId = $('#EncounterTable > tbody > tr.highlight > td:nth-child(3)')[0].outerText.trim();
                var EncounterId = $('#EncounterTable > tbody > tr.highlight > td:nth-child(14)')[0].outerText.trim();
                var PhysicianId = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].outerText.trim();
                if (HumanId != undefined && HumanId != '' && EncounterId != undefined && EncounterId != '' && PhysicianId != undefined && PhysicianId != '') {
                    var obj = new Array();
                    $('#GeneralQTable td').find('input[type=checkbox]:checked')[0].checked = false;;
                    $('#GeneralQTable tr').removeClass("highlight");
                    var Result = openRadWindow("frmCheckOut.aspx?HumanID=" + HumanId + "&EncounterID=" + EncounterId + "&PhysicianId=" + PhysicianId, 800, 1260, obj, 'RadWindowCheckout');

                    return false;
                }
            }
            else {
                alert('Selected status does not permit Check Out');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
        }
        else {
            alert("Select an encounter");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        }
    });
    $('#Processenc').click(function () {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 1) {
            $('#GeneralQTable tr').removeClass("highlight");
            $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            $('#GeneralQTable th').find('input[type=checkbox]:checked').each(function () {
                $(this).prop('checked', false);
            });
            alert("Please select one encounter to process");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
        else if ($('#GeneralQTable').children().find('.highlight').length > 0) {
            var Showall = '';
            $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
            var currentProcess = $('#EncounterTable > tbody > tr.highlight > td:nth-child(8)')[0].outerText.trim();
            //Jira #CAP-706
            //if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS') {
            if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS' || currentProcess == "AKIDO_SCRIBE_PROCESS" || currentProcess == 'AKIDO_REVIEW_CODING') {
                var objType = $('#EncounterTable > tbody > tr.highlight > td:nth-child(16)')[0].outerText.trim();
                //Jira #CAP-706
                //if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'TECHNICIAN_PROCESS' || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS') {
                if (currentProcess == "SCRIBE_CORRECTION" || currentProcess == 'SCRIBE_PROCESS' || currentProcess == "SCRIBE_REVIEW_CORRECTION" || currentProcess == 'MA_PROCESS' || currentProcess == 'TECHNICIAN_PROCESS' || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'CHECK_OUT' || currentProcess == 'SURGERY_COORDINATOR_PROCESS' || currentProcess == "AKIDO_SCRIBE_PROCESS" || currentProcess == 'AKIDO_REVIEW_CODING') {
                    var encounterID = $('#EncounterTable > tbody > tr.highlight > td:nth-child(14)')[0].outerText.trim();
                }
                var apptdt = $('#EncounterTable > tbody > tr.highlight > td:nth-child(2)')[0].outerText.trim();
                var humanid = $('#EncounterTable > tbody > tr.highlight > td:nth-child(3)')[0].outerText.trim();
                var physcianid = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].outerText.trim();
                var date = $('#EncounterTable > tbody > tr.highlight > td:nth-child(17)')[0].outerText.trim();
                var ExamRoom = document.getElementById("Exam").value;
                var now = new Date();
                var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
                if (currentProcess == "CHECK_OUT") {
                    if (humanid != undefined && humanid != '' && encounterID != undefined && encounterID != '' && physcianid != undefined && physcianid != '') {
                        var obj = new Array();
                        $('#GeneralQTable td').find('input[type=checkbox]:checked')[0].checked = false;
                        $('#GeneralQTable tr').removeClass("highlight");
                        var Result = openRadWindow("frmCheckOut.aspx?HumanID=" + humanid + "&EncounterID=" + encounterID + "&PhysicianId=" + physcianid, 800, 1260, obj, 'RadWindowCheckout');
                        return false;
                    }
                }
                else {
                    var Data = [humanid, encounterID, physcianid, currentProcess, objType, date, utc, ExamRoom, Showall, apptdt];

                    $.ajax({
                        type: "POST",
                        url: "frmMyQueueNew.aspx/ProcessGenEncounter",
                        data: JSON.stringify({
                            "data": Data,
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            //jira #cap190 - Workflow issue - Current process PROVIDER_PROCESS current owner MA
                            if (data.d == 'UNKNOWN') {
                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                DisplayErrorMessage('1400013');
                                return false;
                            }
                            sessionStorage.setItem("EnablePFSHMenu", "true");
                            localStorage.setItem("CodingException", encounterID);
                            //Jira #CAP-709
                            localStorage.setItem("CurrentProcess", currentProcess);
                            sessionStorage.setItem("EncId_PatSummaryBar", encounterID);
                            //sessionStorage.setItem("Enc_DOS", date);
                            sessionStorage.setItem("Enc_DOS", data.d);
                            window.location = "frmPatientChart.aspx?hdnLocalTime=" + utc + "&Notification_Type=All";//BugID:52556,52627
                            $('#GeneralQTable td').find('input[type=checkbox]:checked').removeAttr('checked');
                            $('#GeneralQTable tr').removeClass("highlight");

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
            else {
                alert('Selected encounter cannot be processed.');
                $('#GeneralQTable td').find('input[type=checkbox]:checked').removeAttr('checked');
                $('#GeneralQTable tr').removeClass("highlight");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        else {
            alert("Please select an encounter to process.");
            $('#GeneralQTable td').find('input[type=checkbox]:checked').removeAttr('checked');
            $('#GeneralQTable tr').removeClass("highlight");
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }

        return false;
    });
    $('#Processenctr').click(function () {
        if ($('#MyQTable').children().find('.highlight').length > 0 && $('#MyQTable').children().find('.highlight')[0].classList.length == 1) {
            var currentProcessscnt = 0;
            $('#MyQTable tr.highlight').each(function (i, row) {
                var $row = $(row);
                if ($row.find('td:nth-child(8)')[0] != undefined) {
                    var currentProcesss = $row.find('td:nth-child(8)')[0].outerText.trim();
                    if (currentProcesss == "PROVIDER_REVIEW" || currentProcesss == "PROVIDER_REVIEW_2")
                        currentProcessscnt++;
                }
            });
            if (currentProcessscnt > 1) {
                alert("Please select one encounter to process");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable tr').removeClass("highlight");
                return false;
            }
            else {
                MyQclick();
            }
        }
        else {
            var currentProcessscnt = 0;
            $('#MyQTable tr.highlight').each(function (i, row) {
                var $row = $(row);
                if ($row.find('td:nth-child(8)')[0] != undefined) {
                    var currentProcesss = $row.find('td:nth-child(8)')[0].outerText.trim();
                    if (currentProcesss == "PROVIDER_REVIEW" || currentProcesss == "PROVIDER_REVIEW_2")
                        currentProcessscnt++;
                }
            });
            if (currentProcessscnt > 1) {
                alert("Please select one encounter to process");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                $('#MyQTable td').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable th').find('input[type=checkbox]:checked').each(function () {
                    $(this).prop('checked', false);
                });
                $('#MyQTable tr').removeClass("highlight");
                return false;
            }
            else {
                alert("Please select an encounter to process.");
            }
        }
    });
    $("th").hover(function () {
        $("th").css('cursor', 'pointer');
    });
});
function checkboxclick(evt) {
    if (evt.checked)
        $(evt.parentNode.parentElement).addClass("highlight");
    else
        $(evt.parentNode.parentElement).removeClass("highlight");
}
function selectAll(evt) {
    if (evt.checked) {
        $('#GeneralQTable td').find('input[type=checkbox]').prop('checked', 'checked');
        $('#GeneralQTable tr').addClass("highlight")
        return false;
    }
    else {
        $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
            $(this).prop('checked', false);
        });
        $('#GeneralQTable tr').removeClass("highlight")
        return false;
    }
}
function MyQclick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    localStorage.setItem('PhysicianCPT', '');
    localStorage.setItem('PhysicianICD', '');
    if ($('#MyQTable').children().find('.highlight').length > 0) {
        var currRow = $('#MyQTable').children().find('.highlight');
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").css("background-color", "#4f94cd");
        $("#ctl00_mnuEMR_smnuException_smnuCreateException").removeClass("not-active");
        if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Encounters") > -1 && $('#RefreshMyQ').is(":visible")) {
            var Showall = '';
            $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
            var apptdt = $(currRow)[0]?.children[1]?.innerText.trim();
            //var Curprocess = $(currRow)[0].children[7].innerText.trim();
            var Curprocess = '';
            var human_id = $(currRow)[0]?.children[2]?.innerText.trim();
            var PhyID = '';
            var date = '';
            var encounter_id = '';
            var objtype = '';
            //CAP 314  Uncaught Type Error: Cannot read properties of undefined (reading inner Text)    
            if ($(currRow)[0]?.children[6]?.innerText.trim() == 'TECHNICIAN_PROCESS' && $(currRow)[0]?.children[6]?.innerText.trim() != undefined && $(currRow)[0]?.children[6]?.innerText.trim() != null) {
                Curprocess = $(currRow)[0]?.children[6]?.innerText.trim()
                PhyID = $(currRow)[0]?.children[11]?.innerText.trim();
                date = $(currRow)[0]?.children[13]?.innerText.trim();
                encounter_id = $(currRow)[0]?.children[10]?.innerText.trim();
                objtype = $(currRow)[0]?.children[12]?.innerText.trim();
            }
            else {
                Curprocess = $(currRow)[0]?.children[7]?.innerText.trim();
                PhyID = $(currRow)[0]?.children[14]?.innerText.trim();
                date = $(currRow)[0]?.children[16]?.innerText.trim();
                encounter_id = $(currRow)[0]?.children[13]?.innerText.trim();
                objtype = $(currRow)[0]?.children[15]?.innerText.trim();
            }

            //CAP-618 find all headers from the table.
            var headerList = [];
            $("#MyQTable thead tr th").each(function () {
                headerList.push($(this).text());
            });

            //CAP-314 alert message if any parameter missing
            if (headerList?.includes('Test Details') == false && (Curprocess == undefined || Curprocess == null || Curprocess == '')) {
                alert("Curprocess is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            //CAP-618 alert message if has Test Details.
            else if (headerList?.includes('Test Details') == true && (Curprocess == undefined || Curprocess == null || Curprocess == '')) {
                alert("Please relogin using default facility to access these encounters in My Q.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (PhyID == undefined || PhyID == null || PhyID == '') {
                alert("PhyID is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (date == undefined || date == null || date == '') {
                alert("Date is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (encounter_id == undefined || encounter_id == null || encounter_id == '') {
                alert("Encounter_ID is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (objtype == undefined || objtype == null || objtype == '') {
                alert("ObjType is undefined. Please relogin and try again.");
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }

            var now = new Date();
            var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();


            var Data = [human_id, encounter_id, Facility, PhyID, Curprocess, objtype, date, utc, apptdt];
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessEncounter",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    localStorage.setItem("CodingException", encounter_id);
                    //J/ira #CAP-709
                    localStorage.setItem("CurrentProcess", Curprocess);
                    sessionStorage.setItem("EnablePFSHMenu", "true");
                    sessionStorage.setItem("EncId_PatSummaryBar", encounter_id);
                    sessionStorage.setItem("Enc_DOS", date);
                    window.location = "frmPatientChart.aspx?hdnLocalTime=" + utc + "&Process=" + "MA_Process_Encounter" + "&EncounterDate=" + date + "&EncounterID=" + encounter_id + "&HumanID=" + human_id + "";
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
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Tasks") > -1 && $('#RefreshMyQ').is(":visible")) {
            if ($("#chkMyTask14")[0].checked) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            var human_id = $(currRow)[0].children[1].innerText.trim();
            var Facility = $(currRow)[0].children[6].innerText.trim();
            var MessageId = $(currRow)[0].children[8].innerText.trim();
            var Data = [human_id, Facility];
            var sPersonname = '';
            for (var l = 0; l < cookies.length; l++) {
                if (cookies[l].indexOf("CPersonName") > -1) {
                    sPersonname = cookies[l].split("=")[1];
                }
            }
            if (sPersonname != $(currRow)[0].children[5].innerText.trim()) {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessTask",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    var obj = new Array();
                    jQuery('body').css('cursor', 'default');
                    //for BugID:38984- to open task with patientchart as background.

                    var Result = openRadWindow("frmPatientCommunication.aspx?AccountNum=" + human_id + "&openingfrom=Task&parentscreen=" + "MyQ" + "&MessageID=" + MessageId, 550, 1050, obj, 'ModalWindow');
                    var windowName = $find('ModalWindow');
                    windowName.add_close(OnClientCloseWindow);
                    windowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    return false;
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
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Scan") > -1 && $('#RefreshMyQ').is(":visible")) {
            var now = new Date();
            var utc = now.getTimezoneOffset();
            var sPath = "";
            var ScanID = $(currRow)[0].children[5].innerText;
            if (ScanID != null && ScanID != "") {

                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                var obj = new Array();
                openRadWindow("frmIndexing.aspx?ScanId=" + ScanID + "&CurrentZone=" + utc, 700, 1225, obj, 'IndexWindow');
                $find('IndexWindow').set_behaviors(Telerik.Web.UI.WindowBehaviors.None);
                //$find('IndexWindow').add_close(function () { $('#RefreshMyQ').click(); });
                $find('IndexWindow').add_close(OnClientCloseWindow);
                return false;
            }
        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Orders") > -1 && $('#RefreshMyQ').is(":visible")) {
            var CurrentProcess = $(currRow)[0].children[8].innerText.trim();
            sessionStorage.setItem("Next_Order_Index", $('#MyQTable tr').index($(currRow)));//BugID:41027 -- move to next result
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessOrder",
                data: JSON.stringify({
                    "CurrentProcess": CurrentProcess,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
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
            var sPath = "";
            var orderType = $(currRow)[0].children[14].innerText.replace("INTERNAL", "").trim();
            var ObjType = $(currRow)[0].children[14].innerText;

            // ---region BugID:42368---
            var fromMyOrderQ = "true";
            var OrderSubmitID = "";
            var File_Ref_ID = "";//BugID:43099
            var HumanID = $(currRow)[0].children[2].innerText.trim();
            var EncounterID = $(currRow)[0].children[11].innerText.trim();
            var Physician_ID = $(currRow)[0].children[12].innerText.trim();
            if (orderType == "IMMUNIZATION ORDER" || orderType == "REFERRAL ORDER")
                OrderSubmitID = $(currRow)[0].children[13].innerText.trim();
            else
                OrderSubmitID = $(currRow)[0].children[17].innerText.trim();
            var ResultMasterID = $(currRow)[0].children[19].innerText.trim();
            File_Ref_ID = $(currRow)[0].children[20].innerText.trim();
            var LabID = $(currRow)[0].children[15].innerText.trim();
            var vDescription = $(currRow)[0].children[6].innerText.trim();

            if (CurrentProcess == "MA_REVIEW" && (orderType == "DIAGNOSTIC ORDER" || orderType == "IMAGE ORDER" || orderType == "DME ORDER")) {
                var obj = new Array();
                var Result;
                if (orderType == "DIAGNOSTIC ORDER" || orderType == "IMAGE ORDER")
                    Result = openRadWindow("frmOrdersList.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[17].innerText + "&ScreenMode=MyQ&Openingfrom=MyorderQueue", 665, 1232, obj, 'MessageWindow');
                else if (orderType == "DME ORDER")
                    Result = openRadWindow("frmDMEOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[17].innerText + "&ScreenMode=MyQ&Openingfrom=MyorderQueue", 665, 1232, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');

                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (ObjType.toUpperCase() == "DIAGNOSTIC_RESULT") {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "95%";//BugID:47602
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&ObjType=" + ObjType + "&ResultMasterID=" + ResultMasterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&CurrentProcess=" + CurrentProcess + "&Opening_from=OrdersQ&Openingfrom=MyorderQueue&Result_OBR_Date=" + $(currRow)[0].children[0].innerText.trim().split(' ')[0];//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "RESULT_REVIEW" || orderType == "DICTATION_REVIEW") {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "97%";//BugID:47602
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&OrderSubmitId=" + OrderSubmitID + "&EncounterId=" + EncounterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&Opening_from=OrdersQ&Openingfrom=MyorderQueue&CurrentProcess=" + CurrentProcess + "&File_Ref_ID=" + File_Ref_ID + "&Description=" + vDescription;//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "RESULT_REVIEW" || orderType == "IMMUNIZATION ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmImmunization.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianID=" + $(currRow)[0].children[12].innerText + "&LabId=" + $(currRow)[0].children[15].innerText + "&Openingfrom=MyorderQueue&Screen=MyQ", 800, 1135, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (CurrentProcess == "MA_RESULTS") {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                $(top.window.document).find("#TabViewResult").modal({ backdrop: "static", keyboard: false }, 'show');
                $(top.window.document).find("#TabViewResultTitle")[0].textContent = "View Result";
                $(top.window.document).find("#TabViewResultdlg")[0].style.width = "97%";//BugID:47602
                $(top.window.document).find("#TabViewResultdlg")[0].style.height = "95%";
                var sPath = "frmViewResult.aspx?HumanID=" + HumanID + "&OrderSubmitId=" + OrderSubmitID + "&EncounterId=" + EncounterID + "&PhysicianId=" + Physician_ID + "&LabId=" + LabID + "&Opening_from=OrdersQ" + "&MA=True&Openingfrom=MyorderQueue&CurrentProcess=" + CurrentProcess + "&File_Ref_ID=" + File_Ref_ID;//BugID:42368
                $(top.window.document).find("#TabViewResultFrame")[0].style.height = "100%";
                $(top.window.document).find("#TabViewResultFrame")[0].contentDocument.location.href = sPath;
                $(top.window.document).find("#TabViewResult").one("hidden.bs.modal", function (e) {
                    OnClientCloseWindow();
                });
                return false;
            }
            else if (CurrentProcess == "MA_REVIEW" && orderType == "REFERRAL ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmReferralOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText + "&Openingfrom=MyorderQueue&ScreenMode=Myqueue", 750, 1230, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
            else if (CurrentProcess == "PHYSICIAN_VERIFY" && orderType == "REFERRAL ORDER") {
                var obj = new Array();
                var Result = openRadWindow("frmReferralOrder.aspx?HumanID=" + $(currRow)[0].children[2].innerText + "&EncounterID=" + $(currRow)[0].children[11].innerText + "&PhysicianId=" + $(currRow)[0].children[12].innerText + "&OrderSubmitId=" + $(currRow)[0].children[13].innerText + "&Openingfrom=MyorderQueue&ScreenMode=Myqueue", 830, 1230, obj, 'MessageWindow');
                var windowName = $find('MessageWindow');
                windowName.add_close(OnClientCloseWindow);
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }

            // ---end region BugID:42368---
        }
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Amendment") > -1 && $('#RefreshMyQ').is(":visible")) {
            var human_id = $(currRow)[0].children[2].innerText.trim();
            var encounter_id = $(currRow)[0].children[9].innerText.trim();
            var PhyID = $(currRow)[0].children[10].innerText.trim();
            var Curprocess = $(currRow)[0].children[5].innerText.trim();
            var objtype = $(currRow)[0].children[11].innerText.trim();
            var AddendumID = $(currRow)[0].children[12].innerText.trim();
            var Data = [human_id, encounter_id, PhyID, Curprocess, objtype];
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/ProcessAddendum",
                data: JSON.stringify({
                    "data": Data,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                    localStorage.setItem("CodingException", encounter_id);
                    //Jira #CAP-709
                    localStorage.setItem("CurrentProcess", Curprocess);
                    sessionStorage.setItem("EncId_PatSummaryBar", encounter_id);
                    sessionStorage.setItem("Enc_DOS", "");
                    window.location = "frmPatientChart.aspx?isOpenAddendum=true" + "&currentAddendumId=" + AddendumID;
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
        else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Prescription") > -1 && $('#RefreshMyQ').is(":visible")) {
            var human_id = $(currRow)[0].children[1].innerText.trim();
            var encounter_id = $(currRow)[0].children[6].innerText.trim();
            var prescriptionId = $(currRow)[0].children[7].innerText.trim();
            var Data = [human_id, encounter_id];
            var obj = new Array();
            var Result = openRadWindow("frmRCopiaWebBrowser.aspx?MyType=GENERAL&HumanID=" + human_id + "&EncID=" + encounter_id + "&PrescriptionID=" + prescriptionId +
                "&IsMoveButton=true&IsMoveCheckbox=false&IsPrescriptiontobePushed=N&openingFrom=Queue&IsSentToRCopia=Y", 630, 1200, obj, 'MessageWindow');
            var windowName = $find('MessageWindow');
            windowName.add_close(OnClientCloseWindow);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    else
        return false;
}
function OnClientCloseWindow() {
    //Jira #CAP-889
    //chkShowAllClick();
    var removeList = sessionStorage.getItem('MyQRemoveIdList');
    var btnid = $('#divMyQTab .btncolorMyQ')[0].id;
    if (removeList != "") {
        var removearry = removeList.split(",");
        for (let i = 0; i < removearry.length; i++) {
            if (btnid.indexOf("Order") > -1) {
                $('#MyQTable tr td#' + removearry[i].split("~")[1] + ':contains(' + removearry[i].split("~")[0] + ')').parent().remove();
            }
            else if (btnid == "btnMyTask") {
                $('#MyQTable tr').find('td:eq(8):contains(' + removearry[i].split("~")[0] + ')').parent().remove();
            }
            else if (btnid == "btnMyScan") {
                $('#MyQTable tr').find('td:eq(5):contains(' + removearry[i].split("~")[0] + ')').parent().remove();
            }
            else if (btnid =='btnMyPres')
            {
                $('#MyQTable tr').find('td:eq(7):contains(' + removearry[i].split("~")[0] + ')').parent().remove();
            }
            else {
                chkShowAllClick();
            }

        }

        var numberofEncounters = "";
        if ($('#dvAdd').find("#EncounterTable tbody").length > 0) {
             numberofEncounters = $('#dvAdd').find("#EncounterTable tbody").children().length;
        }
        else {
            numberofEncounters = 0;
        }
        if (btnid != undefined && numberofEncounters != undefined) {
            document.getElementById(btnid).innerText = document.getElementById(btnid).innerText.split("(")[0] + "(" + numberofEncounters + ")";
        }
    }

}

function GenQLoad() {
    var sShowall = '';
    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    if (MyShowAll == "Checked") {
        $("#chkShowAll")[0].checked = true;
        loadenc();
    }
    else {
        $("#chkShowAll")[0].checked = false;
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadEncounter",
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#GeneralQTable').empty();
                var tabContents; var eRoomList;
                var objdata = $.parseJSON(data.d);
                if (objdata.data.length > 0) {
                    for (var i = 0; i < objdata.data.length; i++) {
                        var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                        if (i == 0)
                            tabContents = "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:7%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:13%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        else
                            tabContents = tabContents + "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:7%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:13%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                    }

                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                }
                else {
                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead></table>");
                }
                if (objdata.role == 'Medical Assistant' || objdata.role == 'Physician' || objdata.role == 'Coder' || objdata.role == 'Office Manager') {
                    $('#Processenc').css("background-color", "");
                    $('#Processenc')[0].style.display = "inline-block";
                    $('#Processenc')[0].style.visibility = "visible";
                }
                $("#btnEnc")[0].innerText = "Encounters Q " + "(" + objdata.data.length + ")";

                $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.count[0].Order_Count + ")";

                $("#btnAmendmnt")[0].innerText = "Amendment Q" + "(" + objdata.count[0].Amendmnt_Count + ")";


                localStorage.setItem("GenralOrderCount", objdata.count[0].Order_Count);

                $("#btnEnc").addClass("btncolorMyQ");
                $("#btnGeneral").addClass("btncolorMyQ");
                if (objdata.dataEroom.length > 0) {
                    if ($('select#Exam option').length == 0) { $.each(objdata.dataEroom, function (i, item) { $('#Exam').append($('<option>', { value: objdata.dataEroom[i], text: objdata.dataEroom[i] })); }); }
                }
                else {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
                SortTableHeader('GeneralQ');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
}
function MyQLoad() {
    var MyShowAll = localStorage.getItem('MyShowAll');
    if (MyShowAll == "Checked") {
        Showall = "Checked";
        $("#chkMyShowAll")[0].checked = true;
        loadMyenc();
    }
    else {
        Showall = "Unchecked";
        $("#chkMyShowAll")[0].checked = false;
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/EncounterLoad",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                var Ancillary = objdata.Ancillary;
                let disableOverallSelect = true;
                if (objdata.data.length > 0) {
                    for (var i = 0; i < objdata.data.length; i++) {
                        var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                        let disabled = "disabled='true'";
                        if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
                            disabled = "";
                            disableOverallSelect = false;
                        }
                        if (i == 0) {
                            if (Ancillary != "true")
                                tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                            else
                                tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        }
                        else {
                            if (Ancillary != "true")
                                tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                            else
                                tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        }
                    }
                    if (Ancillary != "true")
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    else
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");

                }
                else {
                    if (Ancillary != "true")
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
                    else
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
                }
                $("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";
                $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.count[0].My_Task_Count + ")";
                $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.count[0].My_Order_Count + ")";
                $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.count[0].My_Scan_Count + ")";
                $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.count[0].My_Presc_Count + ")";
                $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.count[0].My_Amendmnt_Count + ")";
                localStorage.setItem("Myorderscount", objdata.count[0].My_Order_Count);
                if (objdata.EncounterCount != null && objdata.EncounterCount != undefined) {
                    $("#ctl00_C5POBody_lblcount").css('font-size', '11px');
                    $("#ctl00_C5POBody_lblcount")[0].innerHTML = 'Total encounters to be signed are <span style="color:red;">' + objdata.EncounterCount + '</span>. To view current as well as more than 7 days old encounters, click on "ShowAll".'
                }
                else
                    $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                $("#btnMyEnc").removeClass("default");
                $("#btnMyEnc").addClass("btncolorMyQ");
                $("#btnMyQ").removeClass("default");
                $("#btnMyQ").addClass("btncolorMyQ");
                //$('#EncounterTable th').addClass('header');
                SortTableHeader('MyQ');
                RowClick();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

                if (disableOverallSelect) {
                    disableSelectAllMove();
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


}
function ShowMyQTabs(sender) {
    $("#chkMyShowAll")[0].checked = false;
    $("#chkShowAll")[0].checked = false;
    $(":button:not(#btnGeneralQcount):not(#btnMyQcount)").css("background-color", "transparent");
    if (sender.innerText == "General Q") {
        document.getElementById("divGeneralQ").style.display = "";
        document.getElementById("divMyQ").style.display = "none";
        $('#MyQTable').empty();
        $('#GeneralQTable').empty();
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#MoveTo')[0].innerText = "Move To My Encounters";
        $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
        if (Role == "Medical Assistant" || Role == "Office Manager") {
            $('#Exam')[0].style.visibility = "visible";
            $('#lblEr')[0].style.visibility = "visible";
        }
        else {
            $('#Exam')[0].style.visibility = "hidden";
            $('#lblEr')[0].style.visibility = "hidden";
        }
        $('#btnChkOut')[0].style.visibility = "visible";
        $('#Processenc')[0].style.display = "none";
        $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

        $("#btnEnc").removeClass("default");
        $("#btnEnc").addClass("btncolorMyQ");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $("#btnMyQ").removeClass("btncolorMyQ");
        $("#btnMyQ").addClass("default");

        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");

        $('#btnChkOut')[0].style.display = "";//BugID:48827
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        window.setTimeout(GenQLoad, 300);
    }
    else {
        document.getElementById("divGeneralQ").style.display = "none";
        document.getElementById("divMyQ").style.display = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";

        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Encounter";
        $('#MyQTable').empty();
        $("#btnMyEnc").removeClass("default");
        $("#btnMyEnc").addClass("btncolorMyQ");
        $("#btnMyQ").removeClass("default");
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnGeneral").removeClass("btncolorMyQ");
        $("#btnGeneral").addClass("default");


        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");


        $('#btnChangeExamRoom').css("display", "inline-block");
        $('#btnChangeExamRoom').css("background-color", "");
        $('#MovetoNxtProcess').css("display", "inline-block");
        $('#MovetoNxtProcess').css("background-color", "");
        $('#RefreshMyQ').css("background-color", "");
        $('#RefreshMyQ')[0].innerText = "Refresh My Encounters";//BugID:48827
        $('#chkMyTask14').css("display", "none");
        $('#lbl14days').css("display", "none");
        $('#chkOpenTask').css("display", "none");
        $('#lblOpenTask').css("display", "none");
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        window.setTimeout(MyQLoad, 300);
    }
}

function loadMyenc() {
    $("#chkMyShowAll")[0].disabled = false;
    $('#MyQTable').empty();
    var showallchecked = localStorage.getItem('MyShowAll');
    if (showallchecked == "Checked") {
        Showall = "Checked";
        $("#chkMyShowAll")[0].checked = true;
    }
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/chkShowAllMyEncounter",
        data: JSON.stringify({
            "sShowall": Showall,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents = "";
            var objdata = $.parseJSON(data.d);
            var Ancillary = objdata.Ancillary;
            let disableOverallSelect = true;
            if (data.d != "[]") {
                for (var i = 0; i < objdata.data.length; i++) {
                    var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                    let disabled = "disabled='true'";
                    if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
                        disabled = "";
                        disableOverallSelect = false;
                    }
                    if (i == 0) {
                        if (Ancillary != "true")
                            tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        else
                            tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                    }
                    else {
                        if (Ancillary != "true")
                            tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        else
                            tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                    }
                }
                if (Ancillary != "true")
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");

            }
            else {
                if (Ancillary != "true")
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
            }
            //$("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";

            //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
            //$("#btnMyOrder")[0].innerText = "My Orders " + "(" + localStorage.getItem("Myorderscount") + ")";
            //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
            //$("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
            //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";


            if (objdata.EncounterCount != null && objdata.EncounterCount != undefined) {
                $("#ctl00_C5POBody_lblcount").css('font-size', '11px');
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = 'Total encounters to be signed are <span style="color:red;">' + objdata.EncounterCount + '</span>. To view current as well as more than 7 days old encounters, click on "ShowAll".'
            }

            else
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
            SortTableHeader('MyQ');
            RowClick();
            //$('#EncounterTable th').addClass('header');
            if (disableOverallSelect) {
                disableSelectAllMove();
            }
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

function loadMytask() {
    var myOpenTask = localStorage.getItem('MyOpenTask');
    if (myOpenTask == "Checked") {
        $("#chkOpenTask")[0].checked = true;
        $("#chkMyTask14")[0].checked = false;
        $("#chkMyTask14")[0].disabled = true;
    } else {
        $("#chkOpenTask")[0].checked = false;
    }

    var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";

    var url = "LoadMyTask";
    var data = JSON.stringify({ "sShowall": Showall, "sOpenTask": OpenTask });
    var myTask14 = localStorage.getItem('MyTask14');
    if (myTask14 == "Checked") {
        $("#chkMyTask14")[0].checked = true;
        $("#chkMyShowAll")[0].checked = false;
        $("#chkOpenTask")[0].checked = false;
        $("#chkMyShowAll")[0].disabled = true;
        $("#chkOpenTask")[0].disabled = true;
        url = "LoadMyTaskCompleted";
        data = JSON.stringify({ "sShowall": Showall });
        Showall = "";
    }

    var showallchecked = localStorage.getItem('MyShowAllMyTask');
    if (showallchecked == "Checked") {
        Showall = "Checked";
        $("#chkMyShowAll")[0].checked = true;
        $("#chkMyTask14")[0].checked = false;
        $("#chkMyTask14")[0].disabled = true;
    } else {
        Showall = "Unchecked"
    }

    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/" + url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
                        Msg_Date_And_Time = "";
                    else
                        Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
                    if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
                        Modified_Date_Time = "";
                    else
                        Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
                    if (i == 0) {
                        //Jira #CAP-119
                        //tabContents = "<tr  style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                        tabContents = "<tr  style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                    }
                    else {
                        //Jira #CAP-119
                        //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                        tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                    }
                }
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-break: break-all;' >" + tabContents + "</tbody></table>");
            }
            else {
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'  ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
            }
            //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)"; 
            $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
            SortTableHeader('MyQTask');
            //$('#EncounterTable th').addClass('header');
            RowClick();
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
function loadMyorder() {
    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadMyOrder",
        data: JSON.stringify({
            "sShowall": Showall,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
                    //Added File_Reference_number in table for ViewResult BugID:43099
                    if (i == 0) {
                        if (objdata[i].Reason_For_Referral != "") {
                            if (objdata[i].Referred_to != "") {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'  id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                            else {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                        }
                        else {
                            if (objdata[i].Referred_to != "") {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";

                            }
                            else {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                        }
                    }
                    else {
                        if (objdata[i].Reason_For_Referral != "") {
                            if (objdata[i].Referred_to != "") {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = tabContents + "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                            else {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = tabContents + "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                        }
                        else {
                            if (objdata[i].Referred_to != "") {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = tabContents + "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                            else {
                                if (objdata[i].Is_Abnormal == "Yes")
                                    tabContents = tabContents + "<tr style='color:red;'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;' id='Test_Date'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;' id='Encounter_ID'>" + objdata[i].Encounter_ID + "</td><td style='display:none;' id='Physician_ID'>" + objdata[i].Physician_ID + "</td><td style='display:none;' id='Order_ID'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;' id='Lab_ID'>" + objdata[i].Lab_ID + "</td><td style='display:none;' id='Lab_Location_ID'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;' id='Order_Submit_ID'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;' id='ResultMasterID'>" + objdata[i].ResultMasterID + "</td><td style='display:none;' id='File_Reference_No'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                            }
                        }
                    }
                }
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:8%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Description</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Lab/Referred to</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;text-align: center;display:none;'>Referred to Facility</th><th style='border: 1px solid #909090;display:none;'>Result_Master_ID</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Narrative Interpretation</th></tr></thead><tbody  style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:8%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Description</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Lab/Referred to</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;text-align: center;display:none;'>Referred to Facility</th><th style='border: 1px solid #909090;display:none;'>Result_Master_ID</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Narrative Interpretation</th></tr></thead></table>");
            $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.length + ")";

            localStorage.setItem("Myorderscount", objdata.length);
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "Note:All abnormal results are in <span style='color:red'> RED</span> color font.";
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            SortTableHeader('MyQOrder');
            //$('#EncounterTable th').addClass('header');
            RowClick();
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
function loadMyscan() {
    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadMyScan",
        data: JSON.stringify({
            "sShowall": Showall,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (i == 0)
                        tabContents = "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + + "</td></tr>";
                    else
                        tabContents = tabContents + "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + "</td></tr>";
                }
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.length + ")";
            }
            else
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead></table>");
            //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
            SortTableHeader('MyQScan');
            RowClick();
            //$('#EncounterTable th').addClass('header');
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
function loadMyprescription() {
    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadMyPrescription",
        data: JSON.stringify({
            "sShowall": Showall,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (i == 0)
                        tabContents = "<tr><td style='width:20%'>" + ConvertDate(objdata[i].Prescription_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:20%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:20%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:20%'>" + objdata[i].Current_Process + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td  style='display:none;'>" + objdata[i].Prescription_Id + "</td><td  style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td></tr>";
                    else
                        tabContents = tabContents + "<tr><td style='width:20%'>" + ConvertDate(objdata[i].Prescription_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:20%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:20%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:20%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td  style='display:none;'>" + objdata[i].Prescription_Id + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td></tr>";
                }
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:20%'>Prescription Date & Time</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PrescriptionID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:20%'>Prescription Date & Time</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PrescriptionID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><tr></thead></table>");
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
            //$("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
            $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.length + ")";
            SortTableHeader('MyQPrescription');
            RowClick();
            //$('#EncounterTable th').addClass('header');
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
function loadMyAmendment() {
    $("#chkMyShowAll")[0].disabled = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadMyAmendment",
        data: JSON.stringify({
            "sShowall": Showall,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (i == 0) {
                        tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    } else {
                        tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    }
                }
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");

            //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
            $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.length + ")";
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
            SortTableHeader('MyQAmendment');
            //$('#EncounterTable th').addClass('header');
            RowClick();
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
function loadenc() {
    $("#chkMyShowAll")[0].disabled = false;
    var sShowall = '';
    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
    if (MyShowAll == "Checked")
        sShowall = MyShowAll;
    else
        sShowall = "Unchecked";
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadEncounterTabClick",
        data: JSON.stringify({
            "sShowall": sShowall,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);

            if (objdata.data.length > 0) {
                for (var i = 0; i < objdata.data.length; i++) {
                    var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                    if (i == 0)
                        tabContents = "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:7%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:11%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:11%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                    else
                        tabContents = tabContents + "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:7%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:11%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:11%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                }

                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else {
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead></table>");
            }
            //$("#btnEnc")[0].innerText = "Encounters Q " + "(" + objdata.data.length + ")";

            //$("#btnOrder")[0].innerText = "Orders Q " + "(" + localStorage.getItem("GenralOrderCount") + ")";

            //$("#btnAmendmnt")[0].innerText = "Amendment Q" + "(*)";

            if (objdata.role == 'Medical Assistant' || objdata.role == 'Physician' || objdata.role == 'Coder') {
                $('#Processenc').css("background-color", "");
                $('#Processenc')[0].style.display = "inline-block";
                $('#Processenc')[0].style.visibility = "visible";
            }
            //$('#EncounterTable th').addClass('header');
            SortTableHeader('GeneralQ');
            RowClick();
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
function loadorder() {
    $("#chkMyShowAll")[0].disabled = false;
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadOrder",
        data: JSON.stringify({
            "sShowall": "Unchecked",
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != null && data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
                    if (i == 0) {
                        if (objdata[i].Reason_For_Referral != "") {
                            if (objdata[i].Referred_to != "")
                                //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                        else {
                            if (objdata[i].Referred_to != "")
                                //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                //tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                    }
                    else {
                        if (objdata[i].Reason_For_Referral != "") {
                            if (objdata[i].Referred_to != "")
                                //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                        else {
                            if (objdata[i].Referred_to != "")
                                //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                //tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                    }
                }
                //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;;width:10%'>Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;;width:10%'>Patient Name</th><th style='border: 1px solid #909090;;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;;width:10%'>Description</th><th style='border: 1px solid #909090;;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;;width:10%'>Current Process</th><th style='border: 1px solid #909090;;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");
            $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.length + ")";

            localStorage.setItem("GenralOrderCount", objdata.length);

            SortTableHeader('GeneralQOrder');;
            //$('#EncounterTable th').addClass('header');
            RowClick();
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
function loadamend() {
    $("#chkMyShowAll")[0].disabled = false;
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadAmend",
        data: JSON.stringify({
            "sShowall": "Unchecked",
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (i == 0) {
                        //GitLab#2246
                        //tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    } else {
                        //GitLab#2246
                        //tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    }
                }
                //GitLab#2246
                /// $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
            //GitLab#2246
            //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
            //$("#btnAmendmnt")[0].innerText = "Amendment Q " + "(*)";
            $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";

            SortTableHeader('GeneralQAmendment');
            //$('#EncounterTable th').addClass('header');
            RowClick();
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
function chkMyTask14Click(sender) {
    $("#chkOpenTask")[0].checked = false;
    $("#chkMyShowAll")[0].checked = false;
    localStorage.setItem('MyOpenTask', "");
    localStorage.setItem('MyShowAllMyTask', "");
    localStorage.setItem('MyTask14', $("#chkMyTask14")[0].checked ? "Checked" : "Unchecked");
    if ($("#chkMyShowAll") != null) {
        if ($("#chkMyTask14")[0].checked) {
            $("#chkOpenTask")[0].disabled = true;
            $("#chkMyShowAll")[0].disabled = true;
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/LoadMyTaskCompleted",
                data: JSON.stringify({
                    "sShowall": "",
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    $('#MyQTable').empty();
                    var tabContents;
                    var objdata = $.parseJSON(data.d);
                    if (data.d != "[]") {
                        for (var i = 0; i < objdata.length; i++) {
                            if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
                                Msg_Date_And_Time = "";
                            else
                                Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
                            if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
                                Modified_Date_Time = "";
                            else
                                Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
                            if (i == 0) {
                                //Jira #CAP-119
                                //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                                tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                            }
                            else {
                                //Jira #CAP-119
                                //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                                tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                            }
                        }
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    }
                    else
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
                    //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
                    $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
                    $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                    SortTableHeader('MyQTask');
                    //$('#EncounterTable th').addClass('header');
                    RowClick();
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
        else {
            $("#chkOpenTask")[0].disabled = false;
            $("#chkMyShowAll")[0].disabled = false;
            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/LoadMyTask",
                data: JSON.stringify({
                    "sShowall": "",
                    "sOpenTask": ""
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    $('#MyQTable').empty();
                    var tabContents;
                    var objdata = $.parseJSON(data.d);
                    if (data.d != "[]") {
                        for (var i = 0; i < objdata.length; i++) {
                            if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
                                Msg_Date_And_Time = "";
                            else
                                Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
                            if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
                                Modified_Date_Time = "";
                            else
                                Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
                            if (i == 0) {
                                //Jira #CAP-119
                                //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                                tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";

                            } else {
                                //Jira #CAP-119
                                //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                                tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";

                            }
                        }
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    }
                    else
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
                    //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
                    $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
                    $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                    SortTableHeader('MyQTask');
                    //$('#EncounterTable th').addClass('header');
                    RowClick();
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
    }
}

function chkOpenTaskClick() {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $("#chkMyTask14")[0].checked = false;
    $("#chkMyTask14")[0].disabled = false;
    var myOpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";
    localStorage.setItem('MyOpenTask', myOpenTask);
    if ($("#chkOpenTask")[0].checked) {
        $("#chkMyTask14")[0].disabled = true;
    }

    var Showall = $("#chkMyShowAll")[0].checked ? "Checked" : "Unchecked";
    var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/LoadMyTask",
        data: JSON.stringify({
            "sShowall": Showall,
            "sOpenTask": OpenTask,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#MyQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
                        Msg_Date_And_Time = "";
                    else
                        Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
                    if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
                        Modified_Date_Time = "";
                    else
                        Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
                    if (i == 0) {
                        //Jira #CAP-119
                        //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                        tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                    }
                    else {
                        //Jira #CAP-119
                        //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + Modified_Date_Time + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                        tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                    }
                }
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
            //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
            $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
            $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
            SortTableHeader('MyQTask');
            //$('#EncounterTable th').addClass('header');
            RowClick();
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

function ChangeTableForTabs(sender) {

    //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";

    //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
    //$("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
    //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
    //$("#btnAmendmnt")[0].innerText = "Amendment Q" + "(*)";
    $(":button:not(#btnGeneralQcount):not(#btnMyQcount)").css("background-color", "transparent");
    var Showall = "";
    $("#chkMyShowAll")[0].checked = false;
    $("#chkShowAll")[0].checked = false;
    $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    $('#RefreshMyQ').css("background-color", "");
    $('#Processenctr').css("background-color", "");
    $('#btnChangeExamRoom').css("display", "none");
    $('#chkMyTask14').css("display", "none");
    $('#lbl14days').css("display", "none");
    $('#chkOpenTask').css("display", "none");
    $('#lblOpenTask').css("display", "none");
    if (sender.innerText.indexOf("My Encounter") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyEnc").removeClass("default");
        $("#btnMyEnc").addClass("btncolorMyQ");

        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");

        $('#btnChangeExamRoom').css("display", "inline-block");
        $('#btnChangeExamRoom').css("background-color", "");
        $('#MovetoNxtProcess').css("display", "inline-block");
        $('#MovetoNxtProcess').css("background-color", "");
        $('#RefreshMyQ')[0].innerText = "Refresh My Encounters";
        $('#Processenctr')[0].innerText = "Process Encounter";
        window.setTimeout(loadMyenc, 300);
    }
    else if (sender.innerText.indexOf("My Task") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

        $("#btnMyTask").removeClass("default");
        $("#btnMyTask").addClass("btncolorMyQ");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");

        $('#chkMyTask14').css("display", "");
        $('#lbl14days').css("display", "");
        $('#chkOpenTask').css("display", "");
        $('#lblOpenTask').css("display", "");
        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Tasks";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Task";
        window.setTimeout(loadMytask, 300);
    }
    else if (sender.innerText.indexOf("My Order") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyOrder").removeClass("default");
        $("#btnMyOrder").addClass("btncolorMyQ");

        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");


        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Orders";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Order";
        window.setTimeout(loadMyorder, 300);
    }
    else if (sender.innerText.indexOf("My Scan") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyScan").removeClass("default");
        $("#btnMyScan").addClass("btncolorMyQ");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");
        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");

        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Scan";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Scan";
        window.setTimeout(loadMyscan, 300);
    }
    else if (sender.innerText.indexOf("My Prescription") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyPres").removeClass("default");
        $("#btnMyPres").addClass("btncolorMyQ");
        $("#btnMyAmendmnt").removeClass("btncolorMyQ");
        $("#btnMyAmendmnt").addClass("default");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");
        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");

        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Prescription";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Prescription";
        window.setTimeout(loadMyprescription, 300);
    }
    else if (sender.innerText.indexOf("My Amendment") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $("#btnMyAmendmnt").removeClass("default");
        $("#btnMyAmendmnt").addClass("btncolorMyQ");
        $("#btnMyEnc").removeClass("btncolorMyQ");
        $("#btnMyEnc").addClass("default");
        $("#btnMyTask").removeClass("btncolorMyQ");
        $("#btnMyTask").addClass("default");
        $("#btnMyOrder").removeClass("btncolorMyQ");
        $("#btnMyOrder").addClass("default");
        $("#btnMyScan").removeClass("btncolorMyQ");
        $("#btnMyScan").addClass("default");
        $("#btnMyPres").removeClass("btncolorMyQ");
        $("#btnMyPres").addClass("default");

        $('#MovetoNxtProcess').css("display", "none");
        $('#MyQTable').empty();
        $('#RefreshMyQ')[0].innerText = "Refresh My Amendment";
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Amendment";
        window.setTimeout(loadMyAmendment, 300);
    }
    else if ((sender.innerText.indexOf("Encounter")) > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnEnc").removeClass("default");
        $("#btnEnc").addClass("btncolorMyQ");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
        if (Role == "Medical Assistant" || Role == "Office Manager") {
            $('#Exam')[0].style.visibility = "visible";
            $('#lblEr')[0].style.visibility = "visible";
        }
        else {
            $('#Exam')[0].style.visibility = "hidden";
            $('#lblEr')[0].style.visibility = "hidden";
        }
        $('#btnChkOut')[0].style.visibility = "visible";
        $('#btnChkOut')[0].style.display = "";
        $('#MoveTo')[0].innerText = "Move To My Encounters";
        $('#Processenc')[0].style.display = "none";
        window.setTimeout(loadenc, 300);
    }
    else if (sender.innerText.indexOf("Order") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnOrder").removeClass("default");
        $("#btnOrder").addClass("btncolorMyQ");
        $("#btnEnc").removeClass("btncolorMyQ");
        $("#btnEnc").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $("#btnAmendmnt").removeClass("btncolorMyQ");
        $("#btnAmendmnt").addClass("default");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Orders Q";
        $('#MoveTo')[0].innerText = "Move To My Orders";
        $('#btnChkOut')[0].style.display = "none";
        $('#lblEr')[0].style.display = "none";
        $('#Exam')[0].style.display = "none";
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        window.setTimeout(loadorder, 300);
    }
    else if (sender.innerText.indexOf("Amendment") > -1) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        $('#GeneralQTable').empty();
        $("#btnAmendmnt").removeClass("default");
        $("#btnAmendmnt").addClass("btncolorMyQ");
        $("#btnOrder").removeClass("btncolorMyQ");
        $("#btnOrder").addClass("default");
        $("#btnEnc").removeClass("btncolorMyQ");
        $("#btnEnc").addClass("default");
        $("#btnScan").removeClass("btncolorMyQ");
        $("#btnScan").addClass("default");
        $("#btnGeneral").removeClass("default");
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#RefreshQ')[0].innerText = "Refresh Amendment Q";
        $('#MoveTo')[0].innerText = "Move To My Amendment";
        $('#btnChkOut')[0].style.display = "none";
        $('#lblEr')[0].style.display = "none";
        $('#Exam')[0].style.display = "none";
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        window.setTimeout(loadamend, 300);
    }
}

$('#ProcessModal').on('hidden.bs.modal', function () {
    chkShowAllClick();
});

function shwllclck() {
    $('#MyQTable').empty();
    //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";

    //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
    //$("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
    //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
    //$("#btnAmendmnt")[0].innerText = "Amendment Q" + "(*)";
    $(":button:not(#btnGeneralQcount):not(#btnMyQcount)").css("background-color", "transparent");
    $('#btnChangeExamRoom').css("display", "none");
    $("#chkMyTask14")[0].checked = false;
    $("#chkMyShowAll")[0].disabled = false;
    if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Encounters") > -1 && $('#RefreshMyQ').is(":visible")) {

        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $('#RefreshMyQ').css("background-color", "");
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Encounter";
        $('#btnChangeExamRoom').css("display", "inline-block");
        $('#btnChangeExamRoom').css("background-color", "");
        $('#MovetoNxtProcess').css("display", "inline-block");
        $('#MovetoNxtProcess').css("background-color", "");
        $("#btnMyEnc").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyTask").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        if ($("#MovetoNxtProcess") != null)
            $("#MovetoNxtProcess")[0].disabled = true;
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        localStorage.setItem("MyShowAll", Showall);
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/chkShowAllMyEncounter",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents = "";
                var objdata = $.parseJSON(data.d);
                var Ancillary = objdata.Ancillary;
                let disableOverallSelect = true;
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.data.length; i++) {
                        var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                        var dt = ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " "));
                        let disabled = "disabled='true'";
                        if (objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW" || objdata.data[i].Current_Process.toUpperCase() == "PROVIDER_REVIEW_2") {
                            disabled = "";
                            disableOverallSelect = false;
                        }

                        if (i == 0) {
                            if (Ancillary != "true")
                                tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                            else
                                tabContents = "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        }
                        else {
                            if (Ancillary != "true")
                                tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:12%'>" + objdata.data[i].PhyName + "</td><td style='width:8%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:8%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                            else
                                tabContents = tabContents + "<tr><td style='width:4%;text-align: center;'><input type='checkbox' onclick='MyQcheckboxclick(this)' class='myQChkbx' " + disabled + "/></td><td style='width:12%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:12%'>" + objdata.data[i].Current_Process + "</td><td style='width:12%'>" + objdata.data[i].Test_Details + "</td><td style='width:12%'>" + objdata.data[i].Ordering_Physician + "</td><td style='width:10%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        }
                    }
                    if (Ancillary != "true")
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    else
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                }
                else {
                    if (Ancillary != "true")
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'   style='table-layout: fixed;'><thead class='header'  style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
                    else
                        $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' ' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox' class='myQChkbxAll' onclick='MyQselectAll(this)'></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Test Details</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>eSuperbill Status</th></tr></thead></table>");
                }
                $("#btnMyEnc")[0].innerText = "My Encounters " + "(" + objdata.data.length + ")";

                SortTableHeader('MyQ');
                //$('#EncounterTable th').addClass('header');
                RowClick();
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                if (disableOverallSelect) {
                    disableSelectAllMove();
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
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Tasks") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyTask").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $('#RefreshMyQ')[0].innerText = "Refresh My Tasks";
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Task";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        localStorage.setItem('MyShowAllMyTask', Showall);

        var myOpenTask = localStorage.getItem('MyOpenTask');
        if (myOpenTask == "Checked") {
            $("#chkOpenTask")[0].checked = true;
        }

        var OpenTask = $("#chkOpenTask")[0].checked ? "Checked" : "Unchecked";

        if ($("#chkMyShowAll")[0].checked) {
            $("#chkMyTask14")[0].checked = false;
            $("#chkMyTask14")[0].disabled = true;
        } else {
            $("#chkMyTask14")[0].checked = false;
            $("#chkMyTask14")[0].disabled = false;
        }

        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadMyTask",
            data: JSON.stringify({
                "sShowall": Showall,
                "sOpenTask": OpenTask
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        if (objdata[i].Msg_Date_And_Time == "0001-01-01T00:00:00")
                            Msg_Date_And_Time = "";
                        else
                            Msg_Date_And_Time = ConvertDate(objdata[i].Msg_Date_And_Time.replace("T", " "));
                        if (objdata[i].Modified_Date_Time == "0001-01-01T00:00:00")
                            Modified_Date_Time = "";
                        else
                            Modified_Date_Time = ConvertDate(objdata[i].Modified_Date_Time.replace("T", " "));
                        if (i == 0) {
                            //Jira #CAP-119
                            //tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                            tabContents = "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";

                        }
                        else {
                            //Jira #CAP-119
                            //tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                            tabContents = tabContents + "<tr style='height:51px'><td style='width:6%'>" + objdata[i].Priority + "</td><td style='width:7%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:11%'>" + Msg_Date_And_Time.split(' ')[0] + "</td><td style='width:11%' title='" + objdata[i].Message_Notes.replace(/[\r\n]+/gm, "").replace('<br />', " ") + "'>" + objdata[i].Message_Description + "</td><td style='width:11%'>" + objdata[i].Assigned_To + "</td><td style='width:11%'>" + objdata[i].Created_By + "</td><td style='width:11%'>" + '' + "</td><td style='display:none'>" + objdata[i].Message_ID + "</td><td style='display:none'>" + objdata[i].Version + "</td></tr>";
                        }
                    }
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%''>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
                }
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:6%'>Priority</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Date</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Message Description</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Assigned To</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Owner</th><th style='border: 1px solid #909090;text-align: center;width:11%'>Completed Date Time</th><th style='border: 1px solid #909090;display:none;'>TaskID</th><th style='border: 1px solid #909090;display:none;'>Version</th></tr></thead></table>");
                //$("#btnMyTask")[0].innerText = "My Tasks " + "(*)";
                $("#btnMyTask")[0].innerText = "My Tasks " + "(" + objdata.length + ")";
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                SortTableHeader('MyQTask');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Scan") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyScan").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Scan";
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        $('#RefreshMyQ').css("background-color", "");
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadMyScan",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        if (i == 0)
                            tabContents = "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + + "</td></tr>";
                        else
                            tabContents = tabContents + "<tr><td style='width:16%'>" + objdata[i].Scanned_File_Name + "</td><td style='width:16%'>" + objdata[i].No_of_Pages + "</td><td style='width:16%'>" + ConvertDate(objdata[i].Scanned_Date.replace("T", " ")) + "</td><td style='width:16%'>" + objdata[i].Facility_Name + "</td><td style='width:16%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Scan_ID + "</td><td style='display:none;'>" + objdata[i].Human_ID + "</td></tr>";
                    }
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    $("#btnMyScan")[0].innerText = "My Scan " + "(" + objdata.length + ")";
                }
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle'style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:16%'>File Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>No of Pages</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Scan Date</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:16%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>Scan_ID</th><th style='border: 1px solid #909090;display:none;'>Human_ID</th></tr></thead></table>");
                //$("#btnMyScan")[0].innerText = "My Scan " + "(*)";
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                SortTableHeader('MyQScan');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Orders") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyOrder").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $('#RefreshMyQ')[0].innerText = "Refresh My Orders";
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Order";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadMyOrder",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
                        if (i == 0) {
                            if (objdata[i].Reason_For_Referral != "") {
                                if (objdata[i].Referred_to != "") {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                } else {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%' >" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                }
                            }
                            else {
                                if (objdata[i].Referred_to != "") {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                }
                                else {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                }
                            }
                        }
                        else {
                            if (objdata[i].Reason_For_Referral != "") {
                                if (objdata[i].Referred_to != "") {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = tabContents + "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td  style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                } else {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = tabContents + "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                }
                            }
                            else {
                                if (objdata[i].Referred_to != "") {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = tabContents + "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                }
                                else {
                                    if (objdata[i].Is_Abnormal == "Yes")
                                        tabContents = tabContents + "<tr style='color:red'><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                    else
                                        tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:6%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td><td style='display:none;'>" + objdata[i].ResultMasterID + "</td><td style='display:none;'>" + objdata[i].File_Reference_No + "</td><td style='width:10%'>" + objdata[i].Is_Narrative + "</td></tr>";
                                }
                            }
                        }
                    }
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:8%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Description</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Lab/Referred to</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;text-align: center;display:none;'>Referred to Facility</th><th style='border: 1px solid #909090;display:none;'>Result_Master_ID</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Narrative Interpretation</th></tr></thead><tbody  style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                }
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:8%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Description</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Lab/Referred to</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;text-align: center;display:none;'>Referred to Facility</th><th style='border: 1px solid #909090;display:none;'>Result_Master_ID</th><th style='border: 1px solid #909090;text-align: center;width:10%'>Narrative Interpretation</th></tr></thead></table>");
                $("#btnMyOrder")[0].innerText = "My Orders " + "(" + objdata.length + ")";
                localStorage.setItem("Myorderscount", objdata.length);
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "Note:All abnormal results are in <span style='color:red'> RED</span> color font.";
                SortTableHeader('MyQOrder');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Amendment") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyAmendmnt").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyPres").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Amendment";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadMyAmendment",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        if (i == 0)
                            tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        else
                            tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    }
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    $("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(" + objdata.length + ")";
                }
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
                //$("#btnMyAmendmnt")[0].innerText = "My Amendment " + "(*)";
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                SortTableHeader('MyQAmendment');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
    else if (document.getElementById("RefreshMyQ").innerText.indexOf("Refresh My Prescription") > -1 && $('#RefreshMyQ').is(":visible")) {
        document.getElementById("divMyQ").style.display = "";
        $('#MyQTable').empty();
        $("#btnMyPres").css({ "background-color": "#bfdbff" });
        $("#btnMyQ").addClass("btncolorMyQ");
        $("#btnMyScan").css({ "background-color": "transparent" });
        $("#btnMyAmendmnt").css({ "background-color": "transparent" });
        $("#btnMyOrder").css({ "background-color": "transparent" });
        $("#btnMyEnc").css({ "background-color": "transparent" });
        $("#btnMyTask").css({ "background-color": "transparent" });
        $('#Processenctr').css("background-color", "");
        $('#Processenctr')[0].innerText = "Process Prescription";
        $('#RefreshMyQ').css("background-color", "");
        var Showall = "";
        $("#chkMyShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadMyPrescription",
            data: JSON.stringify({
                "sShowall": Showall,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#MyQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        if (i == 0)
                            tabContents = "<tr><td style='width:20%'>" + ConvertDate(objdata[i].Prescription_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:20%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:20%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:20%'>" + objdata[i].Current_Process + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td  style='display:none;'>" + objdata[i].Prescription_Id + "</td><td  style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td></tr>";
                        else
                            tabContents = tabContents + "<tr><td style='width:20%'>" + ConvertDate(objdata[i].Prescription_Date.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:20%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:20%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:20%'>" + objdata[i].Current_Process + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td  style='display:none;'>" + objdata[i].Prescription_Id + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td></tr>";
                    }
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:20%'>Prescription Date & Time</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PrescriptionID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    $("#btnMyPres")[0].innerText = "My Prescription " + "(" + objdata.length + ")";
                }
                else
                    $("#MyQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:20%'>Prescription Date & Time</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:20%'>Current Process</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PrescriptionID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><tr></thead></table>");
                // $("#btnMyPres")[0].innerText = "My Prescription " + "(*)";
                $("#ctl00_C5POBody_lblcount")[0].innerHTML = "";
                SortTableHeader('MyQPrescription');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Encounters Q") > -1 && $('#RefreshQ').is(":visible")) {
        document.getElementById("divGeneralQ").style.display = "";
        $('#GeneralQTable').empty();
        $("#btnEnc").addClass("btncolorMyQ");
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ')[0].innerText = "Refresh Encounters Q";
        var ShowallGeneral = "";
        $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
        var MyShowAll = localStorage.setItem('ShowallGeneralqueue', ShowallGeneral);
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#Processenc')[0].style.display = "none";

        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadEncounterTabClick",
            data: JSON.stringify({
                "sShowall": ShowallGeneral,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#GeneralQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (objdata.data.length > 0) {
                    for (var i = 0; i < objdata.data.length; i++) {
                        var is_submitted = (objdata.data[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                        if (i == 0)
                            tabContents = "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:7%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:11%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:11%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                        else
                            tabContents = tabContents + "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:11%'>" + ConvertDate(objdata.data[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata.data[i].Human_ID + "</td><td style='width:7%'>" + objdata.data[i].External_Account_Number + "</td><td style='width:10%'>" + objdata.data[i].Last_Name + "," + objdata.data[i].First_Name + " " + objdata.data[i].MI + "</td><td style='width:7%'>" + DOBConvert(objdata.data[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:8%'>" + objdata.data[i].Type_Of_Visit + "</td><td style='width:11%'>" + objdata.data[i].Current_Process + "</td><td style='width:8%'>" + objdata.data[i].Facility_Name + "</td><td style='width:11%'>" + objdata.data[i].PhyName + "</td><td style='width:7%'>" + objdata.data[i].Carrier_Name + "</td><td style='width:7%'>" + objdata.data[i].Insurance_Plan_Name + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata.data[i].Encounter_ID + "</td><td style='display:none'>" + objdata.data[i].Physician_ID + "</td><td style='display:none'>" + objdata.data[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata.data[i].Date_of_Service + "</td></tr>";
                    }

                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                }
                else {
                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Type of Visit</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Carrier</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Pri. Plan</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead></table>");
                }
                $("#btnEnc")[0].innerText = "Encounters Q " + "(" + objdata.data.length + ")";


                if (objdata.role == 'Medical Assistant' || objdata.role == 'Physician' || objdata.role == 'Coder' || objdata.role == 'Office Manager') {
                    $('#Processenc').css("background-color", "");
                    $('#Processenc')[0].style.display = "inline-block";
                    $('#Processenc')[0].style.visibility = "visible";
                }
                //$('#EncounterTable th').addClass('header');
                RowClick();
                SortTableHeader('GeneralQ');
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
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Orders Q") > -1 && $('#RefreshQ').is(":visible")) {
        document.getElementById("divGeneralQ").style.display = "";
        $('#GeneralQTable').empty();
        $("#btnOrder").css({ "background-color": "#bfdbff" });
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ')[0].innerText = "Refresh Orders Q";
        var ShowallGeneral = "";
        $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadOrder",
            data: JSON.stringify({
                "sShowall": ShowallGeneral,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#GeneralQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
                        if (i == 0) {
                            if (objdata[i].Reason_For_Referral != "") {
                                if (objdata[i].Referred_to != "")
                                    tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            }
                            else {
                                if (objdata[i].Referred_to != "")
                                    tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                else
                                    tabContents = "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            }
                        }
                        else {
                            if (objdata[i].Reason_For_Referral != "") {
                                if (objdata[i].Referred_to != "")
                                    tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Reason_For_Referral + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            }
                            else {
                                if (objdata[i].Referred_to != "")
                                    tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:10%'>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td style='display:none;'>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td style='width:10%'>" + objdata[i].Human_ID + "</td><td style='width:10%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:10%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:10%'>" + objdata[i].Procedure_Ordered + "</td><td style='width:10%'>" + objdata[i].PhyName + "</td><td style='width:10%'>" + objdata[i].Current_Process + "</td><td style='width:10%'>" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            }
                        }
                    }
                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;width:10%'>Acct. #</th><th style='border: 1px solid #909090;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;width:10%'>Patient Name</th><th style='border: 1px solid #909090;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;width:10%'>Description</th><th style='border: 1px solid #909090;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;width:10%'>Current Process</th><th style='border: 1px solid #909090;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                }
                else
                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;width:10%'>Order Date</th><th style='border: 1px solid #909090;display:none;'>Test Date</th><th style='border: 1px solid #909090;width:10%'>Acct. #</th><th style='border: 1px solid #909090;width:10%'>Ext. Acct. #</th><th style='border: 1px solid #909090;width:10%'>Patient Name</th><th style='border: 1px solid #909090;width:10%'>Patient DOB</th><th style='border: 1px solid #909090;width:10%'>Description</th><th style='border: 1px solid #909090;width:10%'>Ordering Physician</th><th style='border: 1px solid #909090;width:10%'>Current Process</th><th style='border: 1px solid #909090;width:10%'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none'>Referred to Facility</th></tr></thead></table>");
                $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.length + ")";

                localStorage.setItem("GenralOrderCount", objdata.length);
                SortTableHeader('GeneralQOrder');
                //$('#EncounterTable th').addClass('header');
                RowClick();

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
    else if (document.getElementById("RefreshQ").innerText.indexOf("Refresh Amendment Q") > -1 && $('#RefreshQ').is(":visible")) {
        $('#GeneralQTable').empty();
        $("#btnAmendmnt").css({ "background-color": "#bfdbff" });
        $("#btnGeneral").addClass("btncolorMyQ");
        $('#RefreshQ')[0].innerText = "Refresh Amendment Q";
        var ShowallGeneral = "";
        $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
        $('#RefreshQ').css("background-color", "");
        $('#btnChkOut').css("background-color", "");
        $('#MoveTo').css("background-color", "");
        $('#Processenc').css("background-color", "");
        $('#Processenc')[0].style.display = "none";
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/LoadAmend",
            data: JSON.stringify({
                "sShowall": ShowallGeneral,
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                $('#GeneralQTable').empty();
                var tabContents;
                var objdata = $.parseJSON(data.d);
                if (data.d != "[]") {
                    for (var i = 0; i < objdata.length; i++) {
                        if (i == 0) {
                            //GitLab#2246
                            //tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                            tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        }
                        else {
                            //GitLab#2246
                            //tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                            tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        }
                    }
                    //GitLab#2246
                    //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                    $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";
                }
                else
                    $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
                //GitLab#2246
                //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
                //$("#btnAmendmnt")[0].innerText = "Amendment Q " + "(*)";
                SortTableHeader('GeneralQAmendment');
                //$('#EncounterTable th').addClass('header');
                RowClick();
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
}

function chkShowAllClick(sender) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    window.setTimeout(shwllclck, 2000);
}
function openRadWindow(fromname, height, width, inputargument, RadWindowName) {
    var Argument = "";
    var PageName = fromname;
    if (inputargument != undefined) {
        for (var i = 0; i < inputargument.length; i++) {
            if (i != 0) {
                Argument = Argument + "&" + inputargument[i];
            } else {
                Argument = inputargument[i];
            }
        }
        if (inputargument.lenght != 0 && inputargument.lenght != undefined) {
            PageName = PageName + "?";
        }
    }
    var result = radopen(PageName + Argument, RadWindowName);
    result.SetModal(true);
    result.set_visibleStatusbar(false);
    result.setSize(width, height);
    result.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close);
    result.set_iconUrl("Resources/16_16.ico");
    result.set_keepInScreenBounds(true);
    result.set_centerIfModal(true);
    result.center();
}
$('#btnChangeExamRoom').click(function (evt) {
    var cells = $('#MyQTable').children().find('.highlight').children();
    if (cells.length > 0) {
        openRadWindow("frmRoomIn.aspx?AccountNumber=" + cells[2].innerText + "&PatientName=" + cells[4].innerText + "&PatientDOB=" + cells[5].innerText +
            "&AssignedPhysician=" + cells[9].innerText + "&AppointmentDateTime=" +
            cells[1].innerText + "&TypeOfVisit=" + '' + "&ExamRoom=" + cells[14].innerText + "&EnounterID=" + cells[13].innerText, 255, 1050, "", 'MessageWindow');
        var windowName = $find('MessageWindow');
        return false;
    }
    else {
        alert("Please select an encounter to process.");
    }
});


function RefreshMyQueue() {
    $('#RefreshMyQ').click();
}

function RowClick() {

    $("#MyQTable tr").click(function () {
        var existingSelectedItem = $("#MyQTable tr.highlight");
        var currentprocesscnt = 0;
        if (isproviderReview == false) {
            if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
        }
        //$('#MyQTable tr.highlight').each(function (i, row) {
        //    var $row = $(row);
        //    var currentProcesss = '';
        //    if ($row.find('td:nth-child(7)')[0] != undefined) {
        //        currentProcesss = $row.find('td:nth-child(7)')[0].outerText.trim();
        //        if (currentProcesss != "PROVIDER_REVIEW" || currentProcesss != "PROVIDER_REVIEW_2")
        //            currentprocesscnt++;
        //    }
        //});
        //if (currentprocesscnt == 0) {
        //    if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
        //}
        isproviderReview = false;
        $(this).toggleClass("highlight");

        //if ($(this).find('input[type="checkbox"]:enabled').length > 0) {
        //    var existingSelectedItem = $("#MyQTable tr.highlight");
        //    if (existingSelectedItem.length > 0) {
        //        if ($(this).find('input[type="checkbox"]:enabled')[0].checked == true) {
        //            $(this).removeClass("highlight");
        //            $(this).find('input[type="checkbox"]:enabled')[0].checked = false;
        //        }
        //        else {
        //            $(this).addClass("highlight");
        //            $(this).find('input[type="checkbox"]:enabled')[0].checked = true;
        //        }
        //    }
        //}

    });
    $("#MyQTable tr").dblclick(function () {
        if (event.target.tagName != 'TH' && event.target.type != 'checkbox') {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            MyQclick();
        }
//Jira #CAP-889
        sessionStorage.setItem('MyQRemoveIdList', '');
    });
    $("#GeneralQTable tr").dblclick(function () {
        //CAP-346 - Prevent Undefined.
        if (event?.target?.tagName != undefined && document.getElementById("RefreshQ")?.innerText?.indexOf("Refresh Encounters Q") != undefined) {
            if (event.target.tagName != 'TH' && document.getElementById("RefreshQ").innerText.indexOf("Refresh Encounters Q") > -1) {
                { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                if ($(this)[0]?.children[0]?.children[0]?.checked != undefined) {
                    $(this)[0].children[0].children[0].checked = true;
                    checkboxclick($(this)[0].children[0].children[0]);
                }
                $('#Processenc').click();
            }
        }
//Jira #CAP-889
        sessionStorage.setItem('MyQRemoveIdList', '');
    });
    $("#GeneralQTable tr").click(function () {

        if (event.target.tagName != 'INPUT') {
            if ($(this)[0].children[0].children[0].checked) {
                $(this).removeClass("highlight");
                $(this)[0].children[0].children[0].checked = false;
            }
            else {
                $(this).toggleClass("highlight");
                $(this)[0].children[0].children[0].checked = true;
            }

        }

    });
}
function movetoEnc() {

    var currentProcess = "";
    var inputData = new Array();
    var flag = 0;
    var tablelength = $('#GeneralQTable tr:has(td)').length;
    if ($('#GeneralQTable tr.highlight').length > 0) {
        $('#GeneralQTable tr.highlight').each(function (i, row) {


            // reference all the stuff you need first
            var $row = $(row);


            if ($row.find('td:nth-child(8)')[0] != undefined) {
                currentProcess = $row.find('td:nth-child(8)')[0].outerText.trim();


                if (currentProcess == 'MA_PROCESS' || currentProcess == 'REVIEW_CODING' || currentProcess == 'AKIDO_REVIEW_CODING' || currentProcess == 'TECHNICIAN_PROCESS'
                    || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'SURGERY_COORDINATOR_PROCESS' || currentProcess == 'SCRIBE_PROCESS' || currentProcess == 'AKIDO_SCRIBE_PROCESS') {
                    var objType = $row.find('td:nth-child(16)')[0].outerText.trim();
                    var ExamRoom = document.getElementById("Exam").value;
                    var now = new Date();
                    var utc = (now.getUTCMonth() + 1) + '/' + now.getUTCDate() + '/' + now.getUTCFullYear(); utc += ' ' + now.getUTCHours() + ':' + now.getUTCMinutes() + ':' + now.getUTCSeconds();
                    if (currentProcess == 'MA_PROCESS' || currentProcess == 'TECHNICIAN_PROCESS' || currentProcess == 'READING_PROVIDER_PROCESS' || currentProcess == 'SCRIBE_PROCESS' || currentProcess == 'AKIDO_SCRIBE_PROCESS') {
                        var encounterID = $row.find('td:nth-child(14)')[0].outerText.trim();
                        var humanID = $row.find('td:nth-child(3)')[0].outerText.trim();//BugID:52865
                        inputData.push(Showall + "~" + currentProcess + "~" + encounterID + "~" + utc + "~" + objType + "~" + ExamRoom + "~" + humanID);
                    }
                    else {
                        var encounterID = $row.find('td:nth-child(14)')[0].outerText.trim();
                        inputData.push(Showall + "~" + currentProcess + "~" + encounterID + "~" + utc + "~" + objType);
                    }
                }

                else {

                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    alert('Selected encounter cannot be moved from General Queue.');
                    flag = 1;
                    $('#GeneralQTable td').find('input[type=checkbox]:checked').each(function () {
                        $(this).prop('checked', false);
                    });
                    $('#GeneralQTable th').find('input[type=checkbox]:checked').each(function () {
                        $(this).prop('checked', false);
                    });
                    $('#GeneralQTable tr').removeClass("highlight")
                    return false;
                }

            }
        });
        if (inputData.length > 0 && flag == 0) {

            $.ajax({
                type: "POST",
                url: "frmMyQueueNew.aspx/MoveToMyEncounters",
                data: JSON.stringify({
                    "data": inputData,
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (data) {
                    var sShowall = '';
                    var MyShowAll = localStorage.getItem('ShowallGeneralqueue');
                    if (MyShowAll == "Checked") {
                        $("#chkShowAll")[0].checked = true;
                        loadenc();
                    }
                    else {
                        $('#GeneralQTable').empty();
                        var tabContents;
                        var objdata = $.parseJSON(data.d);

                        if (objdata.length > 0) {
                            for (var i = 0; i < objdata.length; i++) {
                                var is_submitted = (objdata[i].Is_EandM_Submitted.toUpperCase() == 'Y') ? "Submitted" : "Not Submitted";
                                if (i == 0)
                                    tabContents = "<tr> <td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:12%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:13%'>" + objdata[i].Current_Process + "</td><td style='width:12%'>" + objdata[i].PhyName + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata[i].Encounter_ID + "</td><td style='display:none'>" + objdata[i].Physician_ID + "</td><td style='display:none'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata[i].Date_of_Service + "</td></tr>";
                                else
                                    tabContents = tabContents + "<tr><td style='width:4%'><input type='checkbox' onclick='checkboxclick(this)''/></td><td style='width:12%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:10%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:8%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:13%'>" + objdata[i].Current_Process + "</td><td style='width:12%'>" + objdata[i].PhyName + "</td><td style='width:9%;vertical-align: middle;padding-left:25px;'>" + is_submitted + "</td><td style='display:none'>" + objdata[i].Encounter_ID + "</td><td style='display:none'>" + objdata[i].Physician_ID + "</td><td style='display:none'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none'>" + objdata[i].Date_of_Service + "</td></tr>";
                            }

                            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                        }
                        else {
                            $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 4%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Appt. Date & Time</th><th style='border: 1px solid #909090;text-align: center;width: 6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width: 8%;'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width: 12%;'>Assigned Physician</th><th style='border: 1px solid #909090;text-align: center;width: 9%;'>eSuperbill Status</th></tr></thead></table>");
                        }
                        $("#btnEnc")[0].innerText = "Encounters Q " + "(" + objdata.length + ")";

                        SortTableHeader('GeneralQ');
                        //$('#EncounterTable th').addClass('header');
                        RowClick();
                    }
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
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        alert('Selected encounter cannot be moved from General Queue.');
    }
}
function movetoamend() {
    //GitLab#2246
    //var objID = $('#EncounterTable > tbody > tr.highlight > td:nth-child(16)')[0].innerText;
    //var objtype = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].innerText;
    //GitLab#2246
    var inputData = new Array();
    $('#EncounterTable > tbody > tr.highlight').each(function (i, row) {

        var $row = $(row);

        inputData.push($row.find('td:nth-child(13)')[0].innerText + "~" + $row.find('td:nth-child(14)')[0].innerText);
    });
    //GitLab#2246
    //var Data = [objtype, objID];
    // this parameter is change in GitLab#2246 data: JSON.stringify({
    //"data": inputData,
    //    }),
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/MoveToMyAmendment",
        data: JSON.stringify({
            "data": inputData,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    if (i == 0) {
                        //GitLab#2246
                        //tabContents = "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        tabContents = "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td style='display:none;' >" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    } else {
                        //GitLab#2246
                        //tabContents = tabContents + "<tr><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                        tabContents = tabContents + "<tr><td style='width:56px'><input type='checkbox' onclick='checkboxclick(this)'/></td><td style='width:9%'>" + ConvertDate(objdata[i].Appt_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:6%'>" + objdata[i].Medical_Record_Number + "</td><td style='width:6%'>" + objdata[i].Human_ID + "</td><td style='width:7%'>" + objdata[i].External_Account_Number + "</td><td style='width:9%'>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td style='width:9%'>" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td style='width:9%'>" + objdata[i].Facility_Name + "</td><td style='width:9%'>" + objdata[i].Current_Process + "</td><td style='width:9%'>" + ConvertDate(objdata[i].Addendum_Created_Date_Time.replace("T", " ")) + "</td><td style='width:9%'>" + objdata[i].Addendum_Created_By + "</td><td style='width:9%'>" + objdata[i].Addendum_Signed_By + "</td><td  style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Addendum_ID + "</td><td style='display:none;'>" + objdata[i].Current_Owner + "</td></tr>";
                    }
                }
                //GitLab#2246
                //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%;'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%;'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width: 3%;'>Select<input type='checkbox'  onclick='selectAll(this)'/></th><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
            //GitLab#2246
            //$("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;text-align: center;width:9%'>Appt. Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Addendum Date</th><th style='border: 1px solid #909090;text-align: center;width:6%'>MRN #</th><th style='border: 1px solid #909090;text-align: center;width:6%'>Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:7%'>Ext. Acct. #</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Patient DOB</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Facility Name</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Current Process</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created Date</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Created By</th><th style='border: 1px solid #909090;text-align: center;width:9%'>Signed By</th><th style='border: 1px solid #909090;display:none;'>EncounterID</th><th style='border: 1px solid #909090;display:none;'>PhysicianID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>AddendumID</th><th style='border: 1px solid #909090;display:none;'>Current Owner</th></tr></thead></table>");
            // $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(*)";
            $("#btnAmendmnt")[0].innerText = "Amendment Q " + "(" + objdata.length + ")";
            $("#GeneralQTable tr").click(function () {
                var existingSelectedItem = $("#GeneralQTable tr.highlight"); if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
                $(this).toggleClass("highlight");
            });
            //$('#EncounterTable th').addClass('header');
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
function movetoorder() {
    var EncounterID = $('#EncounterTable > tbody > tr.highlight > td:nth-child(12)')[0].innerText;
    var OrderID = $('#EncounterTable > tbody > tr.highlight > td:nth-child(14)')[0].innerText;
    var ObjectType = $('#EncounterTable > tbody > tr.highlight > td:nth-child(15)')[0].innerText;
    var CurrentProcess = $('#EncounterTable > tbody > tr.highlight > td:nth-child(9)')[0].innerText;
    var ShowallGeneral = "";
    $("#chkShowAll")[0].checked ? ShowallGeneral = "Checked" : ShowallGeneral = "Unchecked";
    sShowall = ShowallGeneral;
    var Data = [EncounterID, OrderID, ObjectType, sShowall, CurrentProcess];
    $.ajax({
        type: "POST",
        url: "frmMyQueueNew.aspx/MoveToMyOrder",
        data: JSON.stringify({
            "data": Data,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#GeneralQTable').empty();
            var tabContents;
            var objdata = $.parseJSON(data.d);
            if (data.d != "[]") {
                for (var i = 0; i < objdata.length; i++) {
                    var orderType = objdata[i].EHR_Obj_Type.replace("INTERNAL", "").trim();
                    if (i == 0) {
                        if (objdata[i].Reason_For_Referral != "") {
                            if (objdata[i].Referred_to != "")
                                tabContents = "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                tabContents = "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                        else {
                            if (objdata[i].Referred_to != "")
                                tabContents = "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                tabContents = "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td style='display:none;' >" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                    }
                    else {
                        if (objdata[i].Reason_For_Referral != "") {
                            if (objdata[i].Referred_to != "")
                                tabContents = tabContents + "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                tabContents = tabContents + "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Reason_For_Referral + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                        else {
                            if (objdata[i].Referred_to != "")
                                tabContents = tabContents + "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td>" + objdata[i].Referred_to + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                            else
                                tabContents = tabContents + "<tr><td>" + ConvertDate(objdata[i].Created_Date_And_Time.replace("T", " ")) + "</td><td>" + ConvertDate(objdata[i].Test_Date.replace("T", " ")) + "</td><td>" + objdata[i].Human_ID + "</td><td >" + objdata[i].External_Account_Number + "</td><td>" + objdata[i].Last_Name + "," + objdata[i].First_Name + " " + objdata[i].MI + "</td><td >" + DOBConvert(objdata[i].DOB.replace("T00:00:00", "")) + "</td><td >" + objdata[i].Procedure_Ordered + "</td><td>" + objdata[i].PhyName + "</td><td >" + objdata[i].Current_Process + "</td><td >" + objdata[i].Lab_Name + "</td><td style='display:none;'>" + objdata[i].Lab_Loc_Name + "</td><td style='display:none;'>" + objdata[i].Encounter_ID + "</td><td style='display:none;'>" + objdata[i].Physician_ID + "</td><td style='display:none;'>" + objdata[i].Order_ID + "</td><td style='display:none;'>" + objdata[i].EHR_Obj_Type + "</td><td style='display:none;'>" + objdata[i].Lab_ID + "</td><td style='display:none;'>" + objdata[i].Lab_Location_ID + "</td><td style='display:none;'>" + objdata[i].Order_Submit_ID + "</td><td  style='display:none;'>" + objdata[i].Referred_to_Facility + "</td></tr>";
                        }
                    }
                }
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;'>Order Date</th><th style='border: 1px solid #909090;'>Test Date</th><th style='border: 1px solid #909090;'>Acct. #</th><th style='border: 1px solid #909090;'>Ext. Acct. #</th><th style='border: 1px solid #909090;'>Patient Name</th><th style='border: 1px solid #909090;'>Patient DOB</th><th style='border: 1px solid #909090;'>Description</th><th style='border: 1px solid #909090;'>Ordering Physician</th><th style='border: 1px solid #909090;'>Current Process</th><th style='border: 1px solid #909090;'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            }
            else
                $("#GeneralQTable").append("<table id=EncounterTable class='table table-bordered Gridbodystyle' style='table-layout: fixed;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header' ><th style='border: 1px solid #909090;'>Order Date</th><th style='border: 1px solid #909090;'>Test Date</th><th style='border: 1px solid #909090;'>Acct. #</th><th style='border: 1px solid #909090;'>Ext. Acct. #</th><th style='border: 1px solid #909090;'>Patient Name</th><th style='border: 1px solid #909090;'>Patient DOB</th><th style='border: 1px solid #909090;'>Description</th><th style='border: 1px solid #909090;'>Ordering Physician</th><th style='border: 1px solid #909090;'>Current Process</th><th style='border: 1px solid #909090;'>Lab</th><th style='border: 1px solid #909090;display:none;'>Lab Location</th><th style='border: 1px solid #909090;display:none;'>Encounter_ID</th><th style='border: 1px solid #909090;display:none;'>Physician_ID</th><th style='border: 1px solid #909090;display:none;'>Order_ID</th><th style='border: 1px solid #909090;display:none;'>ObjType</th><th style='border: 1px solid #909090;display:none;'>LabID</th><th style='border: 1px solid #909090;display:none;'>LocationID</th><th style='border: 1px solid #909090;display:none;'>Order_Submit_ID</th><th style='border: 1px solid #909090;display:none;'>Referred to Facility</th></tr></thead></table>");
            $("#btnOrder")[0].innerText = "Orders Q " + "(" + objdata.length + ")";

            localStorage.setItem("GenralOrderCount", objdata.length);
            //$('#EncounterTable th').addClass('header');
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
function MoveToClick(sender) {

    var Showall = '';
    $("#chkShowAll")[0].checked ? Showall = "Checked" : Showall = "Unchecked";
    if (document.getElementById("MoveTo").innerText.indexOf("Move To My Encounters") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {
            window.setTimeout(movetoEnc, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an encounter to process.");
        }
    }
    else if (document.getElementById("MoveTo").innerText.indexOf("Move To My Amendment") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {

            window.setTimeout(movetoamend, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an encounter to process.");
        }
    }
    else if (document.getElementById("MoveTo").innerText.indexOf("Move To My Orders") > -1 && $('#RefreshQ').is(":visible")) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if ($('#GeneralQTable').children().find('.highlight').length > 0) {

            window.setTimeout(movetoorder, 300);
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            alert("Please select an encounter to process.");
        }
    }
}
function ConvertDate(utcDate) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var now = new Date(utcDate + ' UTC');
    var then = '';
    if (utcDate == '0001-01-01 00:00:00')
        then = '01-01-0001';
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

function DOBConvert(DOB) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = DOB.split('-');
    if (SplitDOB[1].substring(0, 1) == "0")
        SplitDOB[1] = SplitDOB[1].slice(-1);
    return SplitDOB[2] + "-" + monthNames[parseInt(SplitDOB[1]) - 1] + "-" + SplitDOB[0];
}
function DateConversion(date) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var SplitDOB = date.split('-');
    return SplitDOB[2] + "" + ((monthNames.indexOf(SplitDOB[1]) < 10) ? (0 + "" + monthNames.indexOf(SplitDOB[1])) : (monthNames.indexOf(SplitDOB[1]))) + "" + SplitDOB[0];
}
function SortCol(Col, tdArray) {
    var isSame = true;
    if (Col == 'd') {
        for (i = 0; i < tdArray.length; i++) {
            if (tdArray[i].innerText != "") {
                var date = tdArray[i].innerText.split(' ')[0];
                date = DateConversion(date);
                if (tdArray[i].innerText.split(' ')[1] != undefined) {
                    //CAP-281 - null handling in array
                    var t1 = tdArray[i]?.innerText?.split(' ')[1]?.replace(':', '');
                    var t2 = tdArray[i]?.innerText?.split(' ')[2]?.replace('A', 0).replace('P', 1).replace('M', '');
                    tdArray[i].innerText = date + "" + t1 + "" + t2;
                }
                else
                    tdArray[i].innerText = date;
            }
            else
                tdArray[i].innerText = "0000000000000";
        }
        tdArray.sort(function (p, n) {
            var pData = parseInt($(p).text().trim());
            var nData = parseInt($(n).text().trim());
            if (pData != nData)
                isSame = false;
            return pData - nData;
        });
        for (i = 0; i < tdArray.length; i++) {
            if (tdArray[i].innerText != "0000000000000") {
                var y = tdArray[i].innerText.substring(0, 4);
                var m = tdArray[i].innerText.substring(4, 6);
                var d = tdArray[i].innerText.substring(6, 8);
                var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                if (m.substring(0, 1) == "0")
                    m = m.substring(1, 2);
                m = monthNames[m];
                if (tdArray[i].innerText.substring(8, 10) != "") {
                    var t1 = tdArray[i].innerText.substring(8, 10) + ":" + tdArray[i].innerText.substring(10, 12);
                    if (tdArray[i].innerText.substring(12, 13) == "0")
                        var t2 = "AM";
                    else
                        if (tdArray[i].innerText.substring(12, 13) == "1")
                            var t2 = "PM";
                    tdArray[i].innerText = d + "-" + m + "-" + y + " " + t1 + " " + t2;
                }
                else
                    tdArray[i].innerText = d + "-" + m + "-" + y;
            }
            else
                tdArray[i].innerText = "";
        }
    }
    if (Col == 'i') {
        tdArray.sort(function (p, n) {
            var pData = parseInt($(p).text().trim());
            var nData = parseInt($(n).text().trim());
            if (pData != nData)
                isSame = false;
            return pData - nData;
        });
    }
    else
        if (Col == 's') {
            tdArray.sort(function (p, n) {
                var pData = $(p).text().trim().replace(/<[^>]+>/g, '');;
                var nData = $(n).text().trim().replace(/<[^>]+>/g, '');;
                if (pData != nData)
                    isSame = false;
                if (pData < nData)
                    return 1;
                else if (pData > nData)
                    return -1;
                else
                    return 0;
            });
        }
    var sArray = [tdArray, isSame];

    return sArray;
}
function SortTableHeader(s) {
    var Header;
    if (s == 'MyQ' || s == 'MyQTask' || s == 'MyQOrder' || s == 'MyQScan' || s == 'MyQPrescription' || s == 'MyQAmendment')
        Header = $("#MyQTable th");
    else
        if (s == 'GeneralQOrder' || s == 'GeneralQ' || s == 'GeneralQScan' || s == 'GeneralQAmendment')
            Header = $("#GeneralQTable th");
    for (var i = 0; i < Header.length; i++)
        Header[i].title = "Click here to sort";

    scrolify($('#EncounterTable'), 600);
    $("#MyQTable th").click(function () {
        var table = document.createElement("table");
        switch (s) {
            case 'MyQ': billQ = ['', 'd', 'i', 'i', 's', 'd', 's', 's', 's', 's', 's', 's']; break;
            case 'MyQTask': billQ = ['i', 'i', 's', 'd', 'd', 's', 's', 's', 's', 'd']; break;
            case 'MyQOrder': billQ = ['d', 'n', 'i', 'i', 's', 'd', 's', 's', 's', 's', 's', 's', 'n', 'n', 'n', 'n', 'n', 'n', 'n', 'n', 'n', 's']; break;
            case 'MyQScan': billQ = ['s', 'i', 'd', 's', 's', 's']; break;
            case 'MyQPrescription': billQ = ['d', 's', 'i', 'i', 's', 'd', 's']; break;
            case 'MyQAmendment': billQ = ['d', 'd', 's', 'i', 'i', 's', 'd', 's', 's', 'd', 's', 's']; break;
        }

        var columnIndex = $(this).index();
        var Col = billQ[columnIndex];
        var tdArray = $("#MyQTable tbody tr td:nth-child(" + (columnIndex + 1) + ")");
        var isSame = true;
        sArray = SortCol(Col, tdArray);
        tdArray = sArray[0];
        isSame = sArray[1];
        if ($(this)[0].className == "rgSortedAsc") {
            if (isSame == false) {
                for (i = tdArray.length - 1; i >= 0; i--) {
                    var row = tdArray[i].parentElement;
                    if (row[0] == undefined)
                        table.appendChild(row);
                    else
                        table.appendChild(row[0]);
                }
                $("#MyQTable tbody").empty();
                $("#dvAdd table tbody").append(table.rows);
            }

            $("#MyQTable").find("tr th").removeClass('rgSortedDesc');
            $("#MyQTable").find("tr th").removeClass('rgSortedAsc');
            $("#MyQTable").find("tr td").removeClass('ColSorted');
            $(this).addClass('rgSortedDesc');
        }
        else if ($(this)[0].className == "") {
            tdArray.each(function () {
                var row = $(this).parent();
                if (row[0] == undefined)
                    table.appendChild(row);
                else
                    table.appendChild(row[0]);
            });
            $("#MyQTable tbody").empty();
            $("#dvAdd table tbody").append(table.rows);
            $("#MyQTable").find("tr th").removeClass('rgSortedDesc');
            $("#MyQTable").find("tr th").removeClass('rgSortedAsc');
            $("#MyQTable").find("tr td").removeClass('ColSorted');
            $(this).addClass('rgSortedAsc');
        }
        else {
            if (isSame == false) {
                tdArray.each(function () {
                    var row = $(this).parent();
                    if (row[0] == undefined)
                        table.appendChild(row);
                    else
                        table.appendChild(row[0]);
                });
                $("#MyQTable tbody").empty();
                $("#dvAdd table tbody").append(table.rows);
            }
            $("#MyQTable").find("tr th").removeClass('rgSortedDesc');
            $("#MyQTable").find("tr th").removeClass('rgSortedAsc');
            $("#MyQTable").find("tr td").removeClass('ColSorted');
            $(this).addClass('rgSortedAsc');
        }
        $("#MyQTable").find("tbody tr td:nth-child(" + (columnIndex + 1) + ")").addClass('ColSorted');
    });
    $("#GeneralQTable th").click(function () {
        if (event.target.tagName != 'INPUT') {
            var table = document.createElement("table");
            switch (s) {
                case 'GeneralQ': billQ = ['', 'd', 'i', 'i', 's', 'd', 's', 's', 's', 's', 's', 's']; break;
                case 'GeneralQOrder': billQ = ['d', 'd', 'i', 'i', 's', 'd', 's', 's', 's', 's']; break;
                case 'GeneralQScan': billQ = ['s', 'i', 'd', 's', 's', 's']; break;
                case 'GeneralQAmendment': billQ = ['d', 'd', 's', 'i', 'i', 's', 'd', 's', 's', 'd', 's', 's']; break;
            }
            var columnIndex = $(this).index();
            var Col = billQ[columnIndex];
            var tdArray = $("#GeneralQTable tbody tr td:nth-child(" + (columnIndex + 1) + ")");
            var isSame = true;
            sArray = SortCol(Col, tdArray);
            tdArray = sArray[0];
            isSame = sArray[1];
            if ($(this)[0].className == "rgSortedAsc") {
                if (isSame == false) {
                    for (i = tdArray.length - 1; i >= 0; i--) {
                        var row = tdArray[i].parentElement;
                        if (row[0] == undefined)
                            table.appendChild(row);
                        else
                            table.appendChild(row[0]);
                    }
                    $("#GeneralQTable tbody").empty();
                    $("#dvAdd table tbody").append(table.rows);
                }
                $("#GeneralQTable").find("tr th").removeClass('rgSortedDesc');
                $("#GeneralQTable").find("tr th").removeClass('rgSortedAsc');
                $("#GeneralQTable").find("tr td").removeClass('ColSorted');
                $(this).addClass('rgSortedDesc');
            }
            else if ($(this)[0].className == "") {
                tdArray.each(function () {
                    var row = $(this).parent();
                    if (row[0] == undefined)
                        table.appendChild(row);
                    else
                        table.appendChild(row[0]);
                });
                $("#GeneralQTable tbody").empty();
                $("#dvAdd table tbody").append(table.rows);
                $("#GeneralQTable").find("tr th").removeClass('rgSortedDesc');
                $("#GeneralQTable").find("tr th").removeClass('rgSortedAsc');
                $("#GeneralQTable").find("tr td").removeClass('ColSorted');
                $(this).addClass('rgSortedAsc');
            }
            else {
                if (isSame == false) {
                    for (i = tdArray.length - 1; i >= 0; i--) {
                        var row = tdArray[i].parentElement;
                        if (row[0] == undefined)
                            table.appendChild(row);
                        else
                            table.appendChild(row[0]);
                    }
                    $("#GeneralQTable tbody").empty();
                    $("#dvAdd table tbody").append(table.rows);
                }
                $("#GeneralQTable").find("tr th").removeClass('rgSortedDesc');
                $("#GeneralQTable").find("tr th").removeClass('rgSortedAsc');
                $("#GeneralQTable").find("tr td").removeClass('ColSorted');
                $(this).addClass('rgSortedAsc');
            }
            $("#GeneralQTable").find("tbody tr td:nth-child(" + (columnIndex + 1) + ")").addClass('ColSorted');
        }
    });


}
function MyQcheckboxclick(evt) {
    if (evt.checked) {
        //$(evt).closest("tr").addClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = false;
        // isproviderReview = true;
    }
    if ($("input:checkbox[class=myQChkbx]:enabled:checked").length == 0) {
        //$(evt).closest("tr").removeClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = true;
        //isproviderReview = false;
    }
}
function MyQselectAll(chkbxAll) {
    if (chkbxAll.checked) {
        $("input:checkbox[class=myQChkbx]:enabled").prop("checked", "true");
        //$('#MyQTable td').find('input[type=checkbox]').prop('checked', 'checked');
        $("input:checkbox[class=myQChkbx]:enabled").each(function () {
            $(this).prop('checked', true);
            $(this).closest("tr").addClass("highlight");
        });
        //$('#MyQTable tr').addClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = false;
        isproviderReview = true;

        return false;
    }
    else {
        //$("input:checkbox[class=myQChkbx]:enabled").removeProp("checked");
        // $("input:checkbox[class=myQChkbx]:enabled").removeProp("checked");

        $("input:checkbox[class=myQChkbx]:enabled").each(function () {
            $(this).prop('checked', false);
            $(this).closest("tr").removeClass("highlight");
        });
        //$('#MyQTable tr').removeClass("highlight");
        $("#MovetoNxtProcess")[0].disabled = true;
        isproviderReview = false;

        return false;
    }
}
function PerformMovetoNextProcess() {
    let lstHumanID = [];
    let lstEncounterID = [];

    if ($("input:checkbox[class=myQChkbx]:enabled:checked").length > 0) {

        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        let chkLstenctoMove = $("input:checkbox[class=myQChkbx]:enabled:checked");
        for (let i = 0; i < chkLstenctoMove.length; i++) {
            lstHumanID.push($(chkLstenctoMove[i].parentElement.parentElement)[0].cells[2].innerText);
            lstEncounterID.push($(chkLstenctoMove[i].parentElement.parentElement)[0].cells[13].innerText);
        }
        $.ajax({
            type: "POST",
            url: "frmMyQueueNew.aspx/PerformMovetoNextProcess",
            data: "{ 'HumanIDlst': '" + lstHumanID + "','EncIDlst': '" + lstEncounterID + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (data) {
                RefreshMyQueue();
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
    else {
        alert("Please select an encounter to process.");
    }
}
function disableSelectAllMove() {
    $('.myQChkbxAll')[0].disabled = true;

}

