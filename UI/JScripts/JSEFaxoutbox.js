$(document).ready(function () {
    $('#divEFaxTable').empty();
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var recipientname = 
            

    $.ajax({
        type: "POST",
        url: "frmEFax.aspx/EFaxoutboxload",
        contentType: "application/json;charset=utf-8",
        data: '',
        datatype: "json",
        success: function success(data) {
            var objdata = $.parseJSON(data.d);
            var tabContents;
            $('#divEFaxTable').empty();
            if (objdata.ActivityLogList.length > 0) {
                for (var i = 0; i < objdata.ActivityLogList.length; i++) {
                    if (objdata.ActivityLogList[i].Fax_File_Path == "" && objdata.ActivityLogList[i].Fax_Sent_File_Path == "") {
                        if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
                            else
                                tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");'  title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

                        }
                        else {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
                            else
                                tabContents = tabContents + " <tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
                        }

                    }
                    else {
                        if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
                            else
                                tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

                        }
                        else {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
                            else
                                tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
                        }
                    }

                }
            }
            if (tabContents == undefined)
                tabContents = '';
            $("#divEFaxTable").append("<table id='EFaxTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:990px;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Recipient Company</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Recipient Fax</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'>Sent Date and Time </th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Status</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Description</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            $('#EFaxTable th').addClass('header');
            //CAP - 1802
            /*scrolify($('#EFaxTable'), 635);*/            
            scrolify($('#EFaxTable'), 535);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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
});

function funalert(e) {
    alert('ActivityLog Failed id:' + e);
}
function funRetry(e) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        url: "frmEFax.aspx/EFaxoutboxRetry",
        contentType: "application/json;charset=utf-8",
        data: '{ActivityId: "' + e + '"}',
        datatype: "json",
        success: function success(data) {
            var objdata = $.parseJSON(data.d);
            var tabContents;
            $('#divEFaxTable').empty();
            if (objdata.ActivityLogList.length > 0) {
                for (var i = 0; i < objdata.ActivityLogList.length; i++) {
                    if (objdata.ActivityLogList[i].Fax_File_Path == "" && objdata.ActivityLogList[i].Fax_Sent_File_Path == "") {

                        if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
                            else
                                tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

                        }
                        else {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
                            else
                                tabContents = tabContents + " <tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='CloseOutboxImage(" + objdata.ActivityLogList[i].Id + ");'  title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
                        }

                    }
                    else {
                        if (objdata.ActivityLogList[i].Fax_Status.toUpperCase() == "FAILED") {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh'   ></i></td></tr>";
                            else
                                tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i onclick='funRetry(" + objdata.ActivityLogList[i].Id + ");' class='glyphicon glyphicon-refresh' ></i></td></tr>";

                        }
                        else {
                            if (i == 0)
                                tabContents = "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View' class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i  ></i></td></tr>";
                            else
                                tabContents = tabContents + "<tr id=" + objdata.ActivityLogList[i].Id + "><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Recipient_Name.split("|")[0] + "</td><td style='width:10%'>" + objdata.ActivityLogList[i].Fax_Recipient_Company + "</td><td style='width:11%'>" + objdata.ActivityLogList[i].Fax_Recipient_Number + "</td><td style='width:18%'>" + objdata.ActivityLogList[i].Subject + "</td><td style='width:13%'>" + objdata.ActivityLogList[i].Activity_Date_And_Time.replace("T", " ") + "</td><td style='width:15%'>" + objdata.ActivityLogList[i].Fax_Status + "</td><td style='width:17%'>" + objdata.ActivityLogList[i].Error_Description + "</td><td style='width:5%;text-align: center;'><i onclick='OpenViewerforEFoxoutBox(" + objdata.ActivityLogList[i].Id + ");' title='View'  class='glyphicon glyphicon-eye-open'></i></td><td style='width:5%;text-align: center;'><i ></i></td></tr>";
                        }
                    }

                }
                DisplayErrorMessage('1011134');
            }
            if (tabContents == undefined)
                tabContents = '';
            $("#divEFaxTable").append("<table id='EFaxTable' class='table table-bordered Gridbodystyle' style='table-layout: fixed;width:990px;'><thead class='header' style='border: 0px;width:96.7%;'><tr class='header'><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Recipient Name</th><th style='border: 1px solid #909090;text-align: center;width: 10%;'>Recipient Company</th><th style='border: 1px solid #909090;text-align: center;width: 11%;'>Recipient Fax</th><th style='border: 1px solid #909090;text-align: center;width: 18%;'>Subject</th><th style='border: 1px solid #909090;text-align: center;width: 13%;'>Sent Date and Time </th><th style='border: 1px solid #909090;text-align: center;width: 15%;'>Status</th><th style='border: 1px solid #909090;text-align: center;width: 17%;'>Description</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>View</th><th style='border: 1px solid #909090;text-align: center;width: 5%;'>Retry</th></tr></thead><tbody style='word-wrap: break-word;'>" + tabContents + "</tbody></table>");
            $('#EFaxTable th').addClass('header');
            
            //CAP - 1802
            /*scrolify($('#EFaxTable'), 635);*/           
            scrolify($('#EFaxTable'), 535);
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
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


function OpenViewerforEFoxoutBox(EFaxId) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var test = $(top.window.document).find('#ProcessModalExam').modal();
    test.modal({ backdrop: 'static', keyboard: false }, 'show');

    $(top.window.document).find('#btnCloseExam').css("display", "block");
    $(top.window.document).find('#ProcessModalExam')[0].style.marginLeft = "56px";
    $(top.window.document).find('#ProcessModalExam')[0].style.marginTop = "115px";
    $(top.window.document).find("#ProcessFrameExam")[0].style.height = "101%";
    $(top.window.document).find("#mdldlgExam")[0].style.height = "86.5%";
    $(top.window.document).find("#mdldlgExam")[0].style.width = "79%";
    $(top.window.document).find("#mdldlgExam")[0].style.marginLeft = "-8px";
    $(top.window.document).find("#mdldlgExam")[0].style.marginTop = "0px";
    $(top.window.document).find("#ProcessModalExam")[0].style.width = "121%";
    $(top.window.document).find("#ProcessModalExam")[0].style.zIndex = "50001";
    $(top.window.document).find('#ProcessFrameExam')[0].contentDocument.location.href = "frmImageViewer.aspx?Source=EFAX&ActivityId=" + EFaxId;
    $(top.window.document).find("#ModalTitleExam")[0].textContent = "Image Viewer -EFax";

    return false;
}





function CloseOutboxImage() {
    DisplayErrorMessage('1011135');
}
function scrolify(tblAsJQueryObject, height) {
    var oTbl = tblAsJQueryObject;
    var oTblDiv = $("<div id='dvAdd'/>");
    oTblDiv.css('height', height);
    oTblDiv.css('overflow', 'auto');
    oTblDiv.css('margin-top', '-20px');
    oTbl.wrap(oTblDiv);
    
    var newTbl = oTbl.clone();
    oTbl.find('thead tr').remove();
    newTbl.find('tbody tr').remove();

    oTbl.parent().parent().prepend(newTbl);
    newTbl.wrap("<div/>");
    
    if (tblAsJQueryObject[0] != undefined) {
            $("#scrollID").css('height', '');
            $("#scrollID").css('overflow-y', '');
    }
}