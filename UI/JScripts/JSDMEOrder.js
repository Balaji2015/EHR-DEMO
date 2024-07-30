function warningmethod() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
function EnableWaitCursorcbolab() {
    EnableSaveDMEOrder();
    var cbolab = document.getElementById("cboLab");
    __doPostBack('cboLab', cbolab);
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
}
function Numeric_OnKeyPress(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
        return false;
    EnableSaveDMEOrder();
    return true;
}
function closepopup() {
    if (!$('#btnsave').attr("disabled")) {
        //CAP-1798
        //sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
        //saveorder();

        //sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
        //return false;
        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'> <p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';

        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: myPos,
                at: atPos
            },
            buttons: {
                "Yes": function () {
                    sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart();
                    saveorder();
                    sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart();
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btnCloseMed").click();
                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    $(top.window.document).find("#btnCloseMed").click();
                    return false;
                },
                "Cancel": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    return false;
                }
            }
        });
    }
    else {
        $(top.window.document).find("#btnCloseMed").click();

        return false;
    }
}
$(top.window.document).find("#ProcessModalMed").on("hidden.bs.modal", function () {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

});
function saveorder() {
    
    var rows = $('#tabMed tbody >tr');
    var columns;
    var inputdata = "";
    var quantity = 0;
    var flag = 0;
    for (var i = 0; i < rows.length; i++) {
        columns = $(rows[i]).find('td');
        if (columns != undefined) {
            if ($(columns[1])[0].childNodes[0].value != "")
                quantity = $(columns[1])[0].childNodes[0].value;
            var order_id = $(columns[6]).html();
            var priotauth = $(columns[2])[0].childNodes[0].value.toString();
            var Beyond = $(columns[3])[0].childNodes[0].value.toString();
            var Custom = $(columns[4])[0].childNodes[0].value.toString();
            var Justification = $(columns[5])[0].childNodes[0].childNodes[0].value.toString();
            if ((Custom == "Yes" || Beyond == "Yes") && Justification == "")
           {
               alert('Please enter Justification for the lab procedure ' + $(columns[0])[0].childNodes[0].nodeValue)
               flag = 1;
               break;
             
           }
           else{
               var data = quantity + "|" + order_id + "|" + priotauth + "|" + Beyond + "|" + Custom + "|" + Justification;

               if (i == 0 && inputdata == "")
                   inputdata = data
               else
                   inputdata = inputdata + "~" + data;
           }
        }

    }
    if (flag == 0) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        if (inputdata != "") {
            $.ajax({
                type: "POST",
                url: "frmDMEOrder.aspx/updateOrderQuantity",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({
                    "Quantity": inputdata,
                }),
                datatype: "json",
                success: function success(data) {
                    $('#btnsave').attr("disabled", "disabled");
                    DisplayErrorMessage('110020');
                    $(top.window.document).find("#btnCloseMed")[0].click();

                },
                error: function onerror(xhr) {
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
        }
    }
}
function fun(agrulist) {
    agrulist = agrulist.split("~").join("\n");//BugID:45541
    var value = agrulist.split("|");
    var sugglistval = $("#" + value[1]).val().trim();
    var lastChar = sugglistval.substring(sugglistval.length - 2, sugglistval.length);
    if (sugglistval != " " && sugglistval != "") {
        var subsugglistval = sugglistval.split("*")
        var len = subsugglistval.length;
        var flag = 0
        for (var i = 0; i < len; i++) {
            if (subsugglistval[i] == value[0]) {
                flag++;
            }
        }
        if (flag == 0 && sugglistval.indexOf(value[0]) == -1) {
            if (lastChar.indexOf('*') > -1) {
                $("#" + value[1]).val(sugglistval + value[0]);
            }
            else {
                $("#" + value[1]).val(sugglistval + "\n" + "*" + value[0]);
            }
        }
    }
    else {
        $("#" + value[1]).val(value[0]);
    }
    $("#" + value[1]).focus();
}

function callweb(icon, List, id) {

    if (icon.className.indexOf("plus") > -1) {
        $(icon).removeClass("fa fa-plus").addClass("fa fa-minus")
        var ListValue = List;
        var typedval = "";
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValues",
            data: "{'fieldName':'" + ListValue + "','value':'" + typedval + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function OnSuccessPlan(response) {

                var values = response.d.split("|");
                var targetControlValue = id;

                var innerdiv = '';
                var pos = $("#" + targetControlValue).position();
                $("#" + targetControlValue).attr("onkeydown", "insertTab(this, event)");//BugID:45541
                innerdiv += "<li style='text-decoration: none; list-style-type: none;color:rgb(59,64,200);font-weight:bolder;font-style: italic;cursor:default' onclick=\"OpenPopup('" + $('#' + targetControlValue)[0].attributes.getNamedItem('data-src').value + "');\">Click here to Add or Update Keywords</li>";
                for (var i = 0; i < values.length; i++) {

                    innerdiv += "<li style='text-decoration: none;cursor:default; list-style-type: none;color:black' onclick=\"fun('" + values[i].replace(/'/g, "\\'").split("\r\n").join("\n").split("<br />").join("~") + "|" + targetControlValue + "');\">" + values[i] + "</li>";//BugID:45541
                }

                var listlength = innerdiv.length;
                if (listlength > 0) {
                    for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
                        if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                            document.getElementsByTagName("div")[i].hidden = true;
                        }
                    }
                    $("<div class='Listdiv'  id='" + "sg" + targetControlValue + "'tabindex='0'/>").html(innerdiv)
                      .css({
                          top: pos.top + $("#" + targetControlValue).height() + 8,
                          left: '718px',
                          width: $("#" + targetControlValue).width() + 6,
                          height: '95px',
                          overflow: 'scroll',
                          position: 'absolute',
                          background: 'white',
                          bottom: '0',
                          border: '1px solid black',
                          floating: 'top'
                      })
                        .insertAfter($("#" + targetControlValue));
                }

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
    }
    else {
        $(icon).removeClass("fa fa-minus").addClass("fa fa-plus")
        $(icon.parentNode).children('div').css({ "display": "none" });
        for (var i = 0; i < document.getElementsByTagName("div").length; i++) {
            if (document.getElementsByTagName("div")[i].id.indexOf("sg") > -1) {
                document.getElementsByTagName("div")[i].hidden = true;
            }
        }
    }
   
    enablesave();
}
function OpenPopup(KeyWord) {
    var focused = KeyWord;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";
}
function NotesTextChanged() {
    if ($find('btnAdd') != null) {
        $find('btnAdd').set_enabled(true);
        enablesave();
    }
    else {
        document.getElementById('btnAdd').disabled = false;
        enablesave();
    }
}
function NotesGetKeyPress() {
    if ($find('btnAdd') != null) {
        $find('btnAdd').set_enabled(true);

        enablesave();
    }
    else {
        document.getElementById('btnAdd').disabled = false;

        enablesave();
    }
}
function BindMedication() {
    var procedure = localStorage.getItem("procedure");
    var tbody = $("#tabMed");
    $("#tabMed").empty();
    var tr = '<thead class="Gridheaderstyle"><tr><th>Lab Procedure</th><th>Quantity</th><th>Prior Auth Req?</th><th>Beyond Qty Limit?</th><th>Custom Item</th><th>Justification</th>';
    tbody.append(tr);
    if (procedure != "") {
        var Qty = "";
        var tr;
        $.ajax({
            type: "POST",
            url: "frmDMEOrder.aspx/GetQuantity",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({
                "Quantity": procedure,
            }),
            datatype: "json",
            success: function success(data) {
                var objdata = $.parseJSON(data.d);
                for (var i = 0; i < objdata.length; i++) {
                   

                    if (objdata[i].Quantity != "" && objdata[i].Quantity != "0") {
                        tr = "<tr style='height:30px'><td style='width:30%'>" +objdata[i].Lab_Procedure +"-"+objdata[i].Lab_Procedure_Description + "</td><td  style='width:10%'><input type='textbox' class='Editabletxtbox' onkeypress='return isNumberKey(event);' onkeyup='enablesave();' value='" + objdata[i].Quantity + "'/></td> <td  style='width:10%'><select style='width:98%' id ='" + "Auth" + objdata[i].Id + "' onchange='enablesave()'><option value=''></option><option value='Yes'>Yes</option><option value='No'>No</option></select></td><td style='width:10%'><select onchange='enablesave()' style='width:98%' id ='" + "Limit" + objdata[i].Id + "'><option value=''></option><option value='Yes'>Yes</option><option value='No'>No</option></select></td><td style='width:10%'><select onchange='enablesave()' style='width:98%' id ='" + "Custom" + objdata[i].Id + "'><option value=''></option><option value='Yes'>Yes</option><option value='No'>No</option></select></td><td style='width:30%'><div style='height:30px;'><textarea class='editable' onkeydown='enablesave()' id ='" + "jus" + objdata[i].Id + "' data-src='DME-Quantity' contenteditable='true'  style='width:225px; word-wrap: break-word;height:100%;position: static;display:inline-block;resize: none;' oncontextmenu='return false'>" + objdata[i].Justification + "</textarea><div class='col-5-btns' style='width: 17px;float:right!important;display:inline-block;position:relative;vertical-align: middle;text-align: center;top: 15%;right: 5% '><a class='fa fa-plus' style='margin-left: -1px;margin-right: -1px;text-decoration:none;   padding: 4px 4px 4px 4px !important; background: #6DABF7; color: #fff;font-size: 12px; border-radius: 2px;' id='pbDropdow' align='centre'  font-bold='false' title='Drop down' onclick=callweb(this,'DME-Quantity" + "','" + "jus" + objdata[i].Id + "')> </a></div></div></td><td style='display:none'>" + objdata[i].Id + "</td>";

                    }
                    else

                        tr = "<tr style='height:30px'><td style='width:30%'>" + objdata[i].Lab_Procedure + "-" + objdata[i].Lab_Procedure_Description + "</td><td  style='width:10%'><input type='textbox' class='Editabletxtbox' onkeypress='return isNumberKey(event);' onkeyup='enablesave();' value='1.00'/></td> <td  style='width:10%'><select style='width:98%' id ='" + "Auth" + objdata[i].Id + "' onchange='enablesave()'><option value=''></option><option value='Yes'>Yes</option><option value='No'>No</option></select></td><td style='width:10%'><select onchange='enablesave()' style='width:98%' id ='" + "Limit" + objdata[i].Id + "'><option value=''></option><option value='Yes'>Yes</option><option value='No'>No</option></select></td><td style='width:10%'><select onchange='enablesave()' style='width:98%' id ='" + "Custom" + objdata[i].Id + "'><option value=''></option><option value='Yes'>Yes</option><option value='No'>No</option></select></td><td style='width:30%'><div style='height:30px;'><textarea class='editable' onkeydown='enablesave()' id ='" + "jus" + objdata[i].Id + "' data-src='DME-Quantity' contenteditable='true'  style='width:225px; word-wrap: break-word;height:100%;position: static;display:inline-block;resize: none;' oncontextmenu='return false'>" + objdata[i].Justification + "</textarea><div class='col-5-btns' style='width: 17px;float:right!important;display:inline-block;position:relative;vertical-align: middle;text-align: center;top: 15%;right: 5% '><a class='fa fa-plus' style='margin-left: -1px;margin-right: -1px;text-decoration:none;   padding: 4px 4px 4px 4px !important; background: #6DABF7; color: #fff;font-size: 12px; border-radius: 2px;' id='pbDropdow' align='centre'  font-bold='false' title='Drop down' onclick=callweb(this,'DME-Quantity" + "','" + "jus" + objdata[i].Id + "')> </a></div></div></td><td style='display:none'>" + objdata[i].Id + "</td>";
                    tbody.append(tr);

                    $("#Auth" + objdata[i].Id + " >option[value='" + objdata[i].Prior_Auth_Req + "']").attr('selected', true);
                    $("#Limit" + objdata[i].Id + " >option[value='" + objdata[i].Beyond_Qty_Limit + "']").attr('selected', true);
                    $("#Custom" + objdata[i].Id + " >option[value='" + objdata[i].Custom_Item + "']").attr('selected', true);


                }
                $("[id*=pbDropdow]").addClass('pbDropdownBackground');

            },
            error: function onerror(xhr) {
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



       
    }
    else {
        $('#btnsave').attr("disabled", "disabled");
    }
}
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;


}
var OrderIdLst = [];
function OpenMedication_dosageDME() {
    
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
        if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value == "true") {
            document.getElementById("btnAdd").click();
            return true;
            //if (DisplayErrorMessage('230159')) {
            //    document.getElementById("btnAdd").click();
            //    return true;

            //}
            //else {
            //    return false;
            //}
        }
    }
    var order_id = 1544140;
    var order_id2 = 1544141;
    OrderIdLst.push(order_id);
    OrderIdLst.push(order_id2);
    procedure = "";
    $("input[type=checkbox]:checked").each(function () {

        if ($('#' + this.id)[0].parentElement.attributes["orderid"] != undefined && $('#' + this.id)[0].parentElement.attributes["orderid"] != null) {
            if (procedure == "")
                procedure = $('#' + this.id)[0].parentElement.attributes["orderid"].value
            else {
                procedure = procedure + "~" + $('#' + this.id)[0].parentElement.attributes["orderid"].value;
            }
        }
    });

    localStorage.setItem("procedure", procedure);
    $(top.window.document).find("#ProcessModalMed")[0].style.height = "75%";
    $(top.window.document).find("#ProcessModalMed")[0].style.width = "95%";
    $(top.window.document).find("#ProcessModalMed").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#mdldlgMed")[0].style.width = "86%";
    $(top.window.document).find("#ProcessFrameMed")[0].style.height = "275px";
    $(top.window.document).find("#ModalTitleMed").html("DME Order - Edit Quantity");
    $(top.window.document).find("#ModalTitleMed")[0].textContent = "DME Order - Edit Quantity";
    $(top.window.document).find('#ProcessFrameMed')[0].src = "HtmlEditQuantityDME.html?version=" + sessionStorage.getItem("ScriptVersion");
    $(top.window.document).find("#ProcessModalMed").one("hidden.bs.modal", function () {
        CloseEditQuanity();
    });
    return false;
}
function CloseEditQuanity(oWindow, args) {
    disableDMEAutoSave();
    if (document.getElementById('btnAdd') != undefined) {
        document.getElementById('btnAdd').value = "Add";
    }
    if (document.getElementById('btnClearAll') != undefined) {
        document.getElementById('btnClearAll').value = "Clear All";
    }
    if (document.getElementById('btnEditQuantity') != undefined) {
        document.getElementById('btnEditQuantity').disabled = true;
    }
    document.getElementById('btnClear').click();
}
function enablesave() {
    $('#btnsave').attr("disabled", false);
}
function tblSelectProcedure_Clicked(sender, args) {
    var cboLab = document.getElementById("cboLab"); //$find("cboLab");
    if (cboLab.options[cboLab.selectedIndex].text == undefined) {
        DisplayErrorMessage('230140');
        return;
    }
    if (cboLab.options[cboLab.selectedIndex].text == "") {
        DisplayErrorMessage('230140');
        return;
    }
    var btnOrderSubmit = document.getElementById('btnAdd');
    if (btnOrderSubmit.value != undefined && btnOrderSubmit.value.toUpperCase() == "UPDATE") {
        DisplayErrorMessage('230142');
        return;
    }
    var phyID = document.getElementById("hdnPhysicianID").value;
    var proType = document.getElementById("hdnProcedureType").value;
    var phyID = document.getElementById("hdnPhysicianID").value;

    var selectedLabID = cboLab.value;
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "85%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "73%";
    $(top.window.document).find("#TabModalTitle")[0].textContent = "Manage Frequently Used Procedures";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find(".modal-body")[8].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmLabProcedureManage.aspx?ulMyPhysicianID=" + phyID + "&procedureList=LabCorp" + "&procedureType=DME PROCEDURE" + "&selectedLabID=" + selectedLabID + "&SelectedLab=" + cboLab.options[cboLab.selectedIndex].text + "&IsAllProcedure=" + true;
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
        OnClientCloseLabProcedureManager();
    });

}

function OnClientCloseLabProcedureManager(sender, eventArgs) {
    var hdnvariable = document.getElementById("hdnManageFreqUsed");
    hdnvariable.value = "true";
    document.getElementById('InvisibleButton').click();
}
function btnAllProcedures_Clicked(sender, args) {
    var selectedLab = document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text;
    var cboLab = document.getElementById("cboLab");//$find("cboLab");
    if (document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text == undefined) { //if (cboLab.get_selectedItem() == undefined) {
        DisplayErrorMessage('230140');
        return;
    }
    if (document.getElementById("cboLab").options[document.getElementById("cboLab").selectedIndex].text.trim() == "") {//cboLab.get_selectedItem().get_text().trim() == "") {
        DisplayErrorMessage('230140');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return;
    }
    var phyID = document.getElementById("hdnPhysicianID").value;
    var proType = document.getElementById("hdnProcedureType").value;
    var selectedLabID = cboLab.value;
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "680px";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "525px";
    $(top.window.document).find("#TabModalTitle")[0].textContent = "All Procedures";
    $(top.window.document).find("#TabFrame")[0].style.height = "462px";
    $(top.window.document).find(".modal-body")[8].style.height = "100%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmAllProcedures.aspx?ulMyPhysicianID=" + phyID + "&procedureList=LabCorp" + "&procedureType=DME PROCEDURE" + "&selectedLabID=" + selectedLabID + "&SelectedLab=" + selectedLab + "&IsAllProcedure=" + true;
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function () {
        CloseAllProcedures();
    });
    return;
}
function CloseAllProcedures(oWindow, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var arg = sessionStorage.getItem("AllProc_SelectCPT");
    if (arg != undefined) {
        var elementRef = document.getElementById("hdnTransferVaraible");
        elementRef.value = arg;
        __doPostBack('btnAllProcedures');
    }
    else { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function FormLoad() {
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function EnableWaitCursor() {
    __doPostBack('chkSelectALLICD');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
}
function OpenSpecialityDiagonsisImageandLab() {
    
    $(top.window.document).find("#TabModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    if (document.getElementById('hdnUserName') != null)
        $(top.window.document).find("#TabModalTitle")[0].textContent = "All Diagnosis" + " - " + document.getElementById('hdnUserName').value;
    else
        $(top.window.document).find("#TabModalTitle")[0].textContent = "All Diagnosis";
    $(top.window.document).find("#Tabmdldlg")[0].style.width = "54%";
    $(top.window.document).find("#Tabmdldlg")[0].style.height = "67%";
    $(top.window.document).find("#TabFrame")[0].style.height = "100%";
    $(top.window.document).find(".modal-body")[8].style.height = "92.5%";
    $(top.window.document).find("#TabFrame")[0].contentDocument.location.href = "frmSpecialityDiagnosis.aspx?sourceScreen=" + "ORDERS";
    $(top.window.document).find("#TabModal").one("hidden.bs.modal", function (e) {
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        OnClientCloseDiagnosis(null, e);
    });
    return false;
}
function OnClientCloseDiagnosis(oWindow, args) {
    if (oWindow != null) {
        oWindow.remove_close(OnClientCloseSelectLabLocation);
    }
    else {
        $(top.window.document).find("#TabModal").on('hidden', function (e) {
            OnClientCloseSelectLabLocation(null, args);
        });
    }
    document.getElementById("btninvisibleDiagnosis").click();
    EnableSaveDMEOrder();
}
function OnClientCloseSelectLabLocation(oWindow, args) {
    if (oWindow != null) {
        oWindow.remove_close(OnClientCloseDiagnosis);
    }
    else {
        $(top.window.document).find("#TabModal").on('hidden', function (e) {
            OnClientCloseDiagnosis();
        });
    }
    if (args != null) {
        var arg = args;
        if (arg) {
            var elementRef = document.getElementById("txtLocation");
            elementRef.value = arg;
            EnableSaveDMEOrder();
        }
    }

    if (oWindow != null) {
        oWindow.remove_close(OnClientCloseSelectLabLocation);
    }
    else {
        $(top.window.document).find("#TabModal").on('hidden', function (e) {
            OnClientCloseSelectLabLocation();
        });
    }
}
var editIndex = -1;
function grdOrders_OnCommand(sender, args) {
    if (args != undefined)
        editIndex = args.get_commandArgument();

    if (args != undefined) {
        var CommanArgs = args.get_commandName();
        if (CommanArgs == "Del") {
            if (DisplayErrorMessage("230105")) {
                args.set_cancel(false);
            }
            else {
                args.set_cancel(true);
            }
        }
        if (CommanArgs == "EditC") {
        }
    }
    else {
        if (DisplayErrorMessage("230105")) {
            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
            document.getElementById("hdnRowIndex").value = editIndex;
            editIndex = -1;
            document.getElementById("btnDelete").click();
        }
        else { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    }
}
function dtpLastseenbyPhysician_OnDateSelected()
{
    EnableSaveDMEOrder();
}
function pageLoad() {
    $("#dtpLastseenbyPhysician").datetimepicker({  maxDate: 0, closeOnDateSelect: true, format: 'd-M-Y' });
}
function OpenPDFImage(FaxSubject) {
    //Jira CAP-1996 - Start
    //CAP-2329
    if (FaxSubject != undefined)
        FaxSubject = FaxSubject.replaceAll("$|~|$", "'");
    //Jira CAP-1996 - End
    if (FaxSubject!="")
        localStorage['FaxSubject1'] = JSON.stringify(FaxSubject);;
    $(top.window.document).find("#PrintPDFModal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find("#PrintPDFModalTitle")[0].textContent = "Orders";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.width = "900px";
    $(top.window.document).find("#PrintPDFmdldlg")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].style.height = "750px";
    $(top.window.document).find("#PrintPDFFrame")[0].contentDocument.location.href = "frmPrintPDF.aspx?SI=" + document.getElementById('hdnSelectedItem').value + "&Location=DYNAMIC" + "&FromOrder=Y";
}
function btnPrintRequsition_Clicked(sender, args) {
    var btnSaved;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    btnSaved = document.getElementById('btnAdd');
    if (btnSaved.disabled == true) {
        return true;
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        if (btnSaved.disabled == false) {
            DisplayErrorMessage('230145');
            return false;
        }
    }
}
function btnSelectLocations(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
}
function OpenLabLocationScreen(LabID, LabName) {
    var objSelectLabLocation = new Array();
    objSelectLabLocation.push("LabName=" + LabName);
    objSelectLabLocation.push("LabID=" + LabID);
    var SelectLabLocationResult = openModal("frmSelectLabLocation.aspx", 590, 720, objSelectLabLocation, 'MessageWindow');
    var WindowName = $find('MessageWindow');
    WindowName.add_close(OnClientCloseLabLocation);
}
function OnClientCloseLabLocation(oWindow, args) {
    var arg = args.get_argument();
    if (arg) {
        var SelectedLab = arg.selectedLabText;
        var elementRef = document.getElementById("txtLocation");
        if (SelectedLab != undefined)
            elementRef.value = SelectedLab;
    }
    oWindow.remove_close(OnClientCloseLabLocation);
}
function btnAdd_Clicked(sender) {
    
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    return true;
}
function btnClearAll_Clicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var btnSaved;
    btnSaved = document.getElementById('btnAdd');
    if (btnSaved.disabled == false) {
        if (document.getElementById('btnClearAll').value == "Clear All") {
            if (DisplayErrorMessage('230124') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        else if (document.getElementById('btnClearAll').value == "Cancel") {
            if (DisplayErrorMessage('230151') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
    }
    else {
        if (document.getElementById('btnClearAll').value == "Cancel") {
            if (DisplayErrorMessage('230124') == true) {
                document.getElementById('btnClear').click();
                return true;
            }
            else {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                return false;
            }
        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }

}
function SelectItemsUnderHeader(controlInstance) {
    Array.prototype.contains = function (k) {
        for (var p in this)
            if (this[p] === k)
                return true;
        return false;
    }

    var target = event.target || event.srcElement;
    var chks = controlInstance.getElementsByTagName("input");
    var checkedHeaders = new Array();
    var targetState;
    if (target.type == 'checkbox') {
        if (target.parentNode.getAttribute("IsHeader") == "true") {
            checkedHeaders.push(target.parentNode.getAttribute("RespectiveHeader"));
            targetState = target.checked;
        }
    }

    for (var i = 0; i < chks.length; i++) {
        if (checkedHeaders.contains(chks[i].parentNode.getAttribute("RespectiveHeader"))) {
            chks[i].checked = targetState;
        }
    }
}
function Testw(chkBox) {
    if (chkBox.checked)
        EnableSaveDMEOrder();
}
function LabOrder_SavedSuccessfully() {

    DisplayErrorMessage('230150');
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        top.window.document.getElementById('ctl00_C5POBody_hdnIsSaveEnable').value = "false";
    localStorage.setItem("bSave", "true");
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    Order_AfterAutoSave();
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
}
function disableDMEAutoSave() {
    localStorage.setItem("bSave", "true");
    window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "false";
    $('#btnAdd')[0].disabled = true;
}
function ChklstAssessmentEnable() {
    if (document.getElementById('chkSelectALLICD').disabled == false) {
        EnableSaveDMEOrder();
    }
    else {
        document.getElementById('btnAdd').disabled = true; // $find('btnOrderSubmit').set_enabled(false);
    }
}
function chklstFrequentlyUsedProcedures_Changed() {
    EnableSaveDMEOrder();
}

function EnableSaveDMEOrder() {
    if (document.getElementById('btnAdd') != undefined) {
        document.getElementById('btnAdd').disabled = false;
    }
    else {
        document.getElementById('btnAdd').disabled = true;
    }
    if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != null || window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined)
        window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = "true";
    localStorage.setItem("bSave", "false");
}

function chkMoveToMA_Click() {
    var chkMoveToMA = document.getElementById('chkMoveToMA');
    var lblDatelastseen = document.getElementById('lblDatelastseen');
    var lblDurationofneedforDME = document.getElementById('lblDurationofneedforDME');
    var lblDurationofneedforsupplies = document.getElementById('lblDurationofneedforsupplies');
    if (chkMoveToMA.checked) {
        lblDatelastseen.innerText = lblDatelastseen.innerText.replace('*', ' ').trim();
        lblDatelastseen.style.color = "black";
        $('#lblDatelastseen').removeClass('MandLabelstyle');
        lblDurationofneedforDME.innerText = lblDurationofneedforDME.innerText.replace('*', ' ').trim();
        lblDurationofneedforDME.style.color = "black";
        $('#lblDurationofneedforDME').removeClass('MandLabelstyle');
        lblDurationofneedforsupplies.innerText = lblDurationofneedforsupplies.innerText.replace('*', ' ').trim();
        lblDurationofneedforsupplies.style.color = "black";
        $('#lblDurationofneedforsupplies').removeClass('MandLabelstyle');
    }
    else {
        if (lblDatelastseen.innerText != "Date last seen by Physician*") {
            lblDatelastseen.innerText += "*";
            lblDatelastseen.innerHTML = lblDatelastseen.innerText;
        }
        $('#lblDatelastseen').addClass('MandLabelstyle');
        $(lblDatelastseen).html($(lblDatelastseen).html().replace("*", "<span class='manredforstar'>*</span>"));
        if (lblDurationofneedforDME.innerText != "Duration of need for DME*"){
            lblDurationofneedforDME.innerText += "*";
            lblDurationofneedforDME.innerHTML = lblDurationofneedforDME.innerText;
        }
        $('#lblDurationofneedforDME').addClass('MandLabelstyle');
        $(lblDurationofneedforDME).html($(lblDurationofneedforDME).html().replace("*", "<span class='manredforstar'>*</span>"));
        if (lblDurationofneedforsupplies.innerText != "Duration of need for supplies*") {
            lblDurationofneedforsupplies.innerText += "*";
            lblDurationofneedforsupplies.innerHTML = lblDurationofneedforsupplies.innerText;
        }
        $('#lblDurationofneedforsupplies').addClass('MandLabelstyle');
        $(lblDurationofneedforsupplies).html($(lblDurationofneedforsupplies).html().replace("*", "<span class='manredforstar'>*</span>"));
    }
    EnableSaveDMEOrder();
}

function MoveToNextProcessClicked(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById('btnAdd').disabled == false) {
        event.preventDefault();
        event.stopPropagation();
        { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        sessionStorage.setItem("autoMovetonxtProc", "true");
        document.getElementById('hdnType').value = "Yes";
        top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        __doPostBack('btnMoveToNextProcess');
        return true;
        //if (!$($(top.window.document).find('iframe')[0].contentDocument).find("body").is('#dvdialogMenu'))
        //    $($(top.window.document).find('iframe')[0].contentDocument).find("body").append('<div id="dvdialogMenu" style="min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;">' +
        //    '<p style="font-family: Verdana,Arial,sans-serif; font-size: 13.5px;">There are unsaved changes.Do you want to save the them?</p></div>');
        //dvdialog = $($(top.window.document).find('iframe')[0].contentDocument).find("body").find('#dvdialogMenu');
        //event.preventDefault();
        //event.stopPropagation();
        //{ sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

        //$(dvdialog).dialog({
        //    modal: true,
        //    title: "Capella -EHR",
        //    position: {
        //        my: 'left' + " " + 'center',
        //        at: 'center' + " " + 'center + 100px'

        //    },
        //    buttons: {
        //        "Yes": function () {
        //            { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
        //            sessionStorage.setItem("autoMovetonxtProc", "true");
        //            document.getElementById('hdnType').value = "Yes";
        //            top.window.document.getElementById('ctl00_Loading').style.display = 'none';
        //            __doPostBack('btnMoveToNextProcess');
        //            $(dvdialog).dialog("close");
        //            return true;
        //        },
        //        "No": function () {
        //            document.getElementById('hdnType').value = "";
        //            __doPostBack('btnMoveToNextProcess');
        //            $(dvdialog).dialog("close");
        //            self.close();
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //        },
        //        "Cancel": function () {
        //            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        //            document.getElementById('hdnType').value = "";
        //            $(dvdialog).dialog("close");

        //            return false;
        //        }
        //    }
        //});
    }
    else {
        return true;
    }

}


function WindowCloseDiagnostics() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    if (oWindow != null)
        oWindow.close();
    window.top.location.href = "frmMyQueueNew.aspx";//BugID:42368
}
function warningmethod() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
}
