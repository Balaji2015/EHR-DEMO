function OpenPopup(keyword) {
    var focused = keyword;
    $(top.window.document).find("#Modal").modal({ backdrop: 'static', keyboard: false }, 'show');
    $(top.window.document).find('#ProcessiFrame')[0].contentDocument.location.href = "frmAddOrUpdateKeywords.aspx?FieldName=" + focused;
    $(top.window.document).find("#ModalTtle")[0].textContent = "Add Or Update Keywords";

}
var listoptions = "";
var listvalue = "";
var showOptions = false;
var numDataFiles = 2;
var togglesImage = ["+", "-"];
var valuetest = "";
var lstCtrl = null;
var hdnFieldName = null;
var onclicktriggered = false;
var control = "";
function pbDropDown(textcontrol, ListControls, ListValue, textid) {
    listvalue = ListValue;
    valuetest = "";
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }

    control = document.getElementById(textcontrol);
    if (control != null && control.disabled != true) {
        lstCtrl = ListControls;
        if (control.style.background != "rgb(128, 128, 128)") {
            if (control.className.indexOf("pbDropdownBackgrounddisable") < 0) {
                if (control.innerHTML.indexOf("plus") != -1 || control.innerHTML == "+") {
                    if (textid == "" && ListValue == "EMAIL") {
                        $.ajax({
                            type: "POST",
                            url: "frmDLC.aspx/GetListBoxValues",
                            data: '{fieldName: "' + ListValue + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: OnSuccessMail,
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
                        document.getElementById(lstCtrl).style.zIndex = 17;
                        if (control.childNodes[0] != undefined && control.childNodes[0].className != null)
                            control.childNodes[0].className = "fa fa-minus margin2";
                        else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null)
                            control.childNodes[0].nextSibling.className = "fa fa-minus margin2";

                    }
                    else if (textid == "") {
                        $.ajax({
                            type: "POST",
                            url: "frmDLC.aspx/GetListBoxValues",
                            data: '{fieldName: "' + ListValue + '" }',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: OnSuccess,
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
                        document.getElementById(lstCtrl).style.zIndex = 17;
                        if (control.childNodes[0] != undefined && control.childNodes[0].className != null)
                            control.childNodes[0].className = "fa fa-minus margin2";
                        else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null)
                            control.childNodes[0].nextSibling.className = "fa fa-minus margin2";

                    }
                    else if (textid != "") {
                        var value = "";
                        if (textid.split(',').length > 1) {
                            var iddetails1 = textid.split(',')[0];
                            var iddetails2 = textid.split(',')[1];
                            if (document.getElementById(iddetails1).checked == true || document.getElementById(iddetails2).checked == true) {
                                value = "";
                            }
                            else {
                                value = textid.split(',')[2];
                                listoptions = value;
                            }
                        }
                        else {
                            if (textcontrol.indexOf("LastMammogram") != -1) {//BugID:47706
                                ListValue += "|" + "LAST MAMMOGRAM";
                                listvalue = ListValue;
                            }
                            else {
                                var textidnew = textid.replace("/", "").replace("-", "").replace(/\s+/g, '');;
                                if (document.getElementById(textidnew) != null && document.getElementById(textidnew) != undefined) {
                                    value = document.getElementById(textidnew).value;
                                    if (value == "")
                                        value = textid;
                                    listoptions = textid;
                                }
                            }
                        }

                        $.ajax({
                            type: "POST",
                            url: "frmDLC.aspx/GetListBoxValuesReason",

                            data: "{'fieldName':'" + ListValue + "','value':'" + value + "'}",

                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: OnSuccessReason,
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
                        document.getElementById(lstCtrl).style.zIndex = 17;
                        if (control.childNodes[0] != undefined && control.childNodes[0].className != null)
                            control.childNodes[0].className = "fa fa-minus margin2";
                        else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null)
                            control.childNodes[0].nextSibling.className = "fa fa-minus margin2";
                    }
                }
                else if (control.innerHTML.indexOf("minus") != -1 || control.innerHTML == "-") {
                    if (control.childNodes[0] != undefined && control.childNodes[0].className != null)
                        control.childNodes[0].className = "fa fa-plus margin2";
                    else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null)
                        control.childNodes[0].nextSibling.className = "fa fa-plus margin2";
                    document.getElementById(ListControls).style.display = "none";
                    hdnFieldName = null;
                    document.getElementById(textcontrol.replace("pbDropdown", "txtDLC")).focus();
                }
                if (hdnFieldName != null && hdnFieldName.split(',,')[0] != lstCtrl) {
                    document.getElementById(hdnFieldName.split(',,')[0]).style.display = "none";

                    var listcontrol = document.getElementById(hdnFieldName.split(',,')[1]);
                    if (listcontrol.childNodes[0] != undefined && listcontrol.childNodes[0].className != null)
                        listcontrol.childNodes[0].className = "fa fa-plus margin2";
                    else if (listcontrol.childNodes[0] != undefined && listcontrol.childNodes[0].nextSibling.className != null)
                        listcontrol.childNodes[0].nextSibling.className = "fa fa-plus margin2";

                }
                hdnFieldName = lstCtrl + ",," + textcontrol;
            }
            else if (control.style.background != "rgb(128, 128, 128)") {

            }
        }
        document.getElementById(textcontrol.replace("pbDropdown", "txtDLC")).focus();
    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
    return false;
}


function OnSuccessMail(response) {
    var text = response.d;
    var dropDownListRef = document.getElementById(lstCtrl);
    dropDownListRef.options.length = 0;
    var user = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("userrole").value;
    if (user.toUpperCase() == "PHYSICIAN" || user.toUpperCase() == "PHYSICIAN ASSISTANT") {
        var keywrd = document.createElement("option");
        keywrd.style.fontWeight = 'bolder';
        keywrd.style.textDecoration = 'none';
        keywrd.className = "alinkstyle";
        keywrd.style.fontStyle = 'italic';
        keywrd.style.cursor = 'default';
        keywrd.value = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("dlcvalue").value;
        dropDownListRef.options.add(keywrd);
    }
    var splitter = text.split("|");
    for (i = 0; i < splitter.length; i++) {
        if (i != splitter.length - 1) {
            var option1 = document.createElement("option");
            option1.text = splitter[i].split("^")[0];
            if (lstCtrl == "ctmDLCChief_Complaints_listDLC") {
                option1.value = splitter[i].split("^")[1];
            }
            if (lstCtrl == "DLCFood_listDLC") {//BugID:47705
                if (splitter[i].split('^')[1].trim() == "MACRA" && splitter[i].split('^')[2].trim() == "FREQUENTLY USED") {
                    option1.style.color = 'rgb(101,4,208)';
                }
            }
            dropDownListRef.options.add(option1);
        }
    }
    dropDownListRef.style.display = "block";
    dropDownListRef.className = "DLCList";
}


function OnSuccess(response) {
    var text = response.d;
    var dropDownListRef = document.getElementById(lstCtrl);
    dropDownListRef.options.length = 0;
    var user = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("userrole").value;
    if (user.toUpperCase() == "PHYSICIAN" || user.toUpperCase() == "PHYSICIAN ASSISTANT") {
        var keywrd = document.createElement("option");
        keywrd.style.fontWeight = 'bolder';
        keywrd.style.textDecoration = 'none';
        keywrd.className = "alinkstyle";
        keywrd.style.fontStyle = 'italic';
        keywrd.style.cursor = 'default';
        keywrd.text = 'Click here to Add or Update Keywords';
        keywrd.value = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("dlcvalue").value;
        dropDownListRef.options.add(keywrd);
    }
    var splitter = text.split("|");
    for (i = 0; i < splitter.length; i++) {
        if (i != splitter.length - 1) {
            var option1 = document.createElement("option");
            option1.text = splitter[i].split("^")[0];
            if (lstCtrl == "ctmDLCChief_Complaints_listDLC") {
                option1.value = splitter[i].split("^")[1];
            }
            if (lstCtrl == "DLCFood_listDLC") {//BugID:47705
                if (splitter[i].split('^')[1].trim() == "MACRA" && splitter[i].split('^')[2].trim() == "FREQUENTLY USED") {
                    option1.style.color = 'rgb(101,4,208)';
                }
            }
            dropDownListRef.options.add(option1);
        }

    }
    dropDownListRef.style.display = "block";
    dropDownListRef.className = "DLCList";
}

function OnSuccessReason(response) {
    var text = response.d;
    var dropDownListRef = document.getElementById(lstCtrl);
    dropDownListRef.options.length = 0;
    var user = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("userrole").value;
    var flag = 0;
    var showComments = true;
    var HeadingIndex = text.split('$');
    if (HeadingIndex.length <= 2) {
        showComments = false;
    }
    var splitter = text.split("|");
    for (i = 0; i < splitter.length; i++) {
        if (i != splitter.length - 1) {
            var option1 = document.createElement("option");
            if (flag == 1) {
                option1.style.color = 'rgb(101,4,208)';
                option1.setAttribute("key", "Reason");

            }
            if (flag == 2) {
                option1.style.color = 'rgb(101,4,208)';
            }
            if (splitter[i].split("^")[0].replace("$", "") == "Comments") {
                if ((flag == 1 || flag == 2) && valuetest != 'RELOAD#$&^') {
                    var keywrd = document.createElement("option");
                    keywrd.style.fontWeight = 'bolder';
                    keywrd.style.textDecoration = 'none';
                    keywrd.style.color = 'rgb(59,64,200)';
                    keywrd.style.fontStyle = 'italic';
                    keywrd.style.cursor = 'default';
                    keywrd.text = 'Click to view more';
                    dropDownListRef.options.add(keywrd);
                }
                option1.style.fontWeight = 'bolder';
                option1.style.textDecoration = 'none';
                option1.style.color = 'black';
                option1.style.fontStyle = 'normal';
                option1.style.cursor = 'default';
                option1.disabled = true;
            }

            if (splitter[i].split("^")[0].replace("$", "") == "Reason Not Performed") {
                option1.style.fontWeight = 'bolder';
                option1.style.textDecoration = 'none';
                option1.style.color = 'black';
                option1.style.fontStyle = 'normal';
                option1.style.cursor = 'default';
                option1.disabled = true;
                flag = 1;
            }
            if (splitter[i].split("^")[0].replace("$", "") == "Mammogram Tests") {//BugID:47706
                option1.style.fontWeight = 'bolder';
                option1.style.textDecoration = 'none';
                option1.style.color = 'black';
                option1.style.fontStyle = 'normal';
                option1.style.cursor = 'default';
                option1.disabled = true;
                flag = 2;
            }
            option1.text = splitter[i].split("^")[0].replace("$", "");
            if (lstCtrl == "ctmDLCChief_Complaints_listDLC") {
                option1.value = splitter[i].split("^")[1];
            }
            if ((showComments && option1.text == "Comments") || option1.text != "Comments")
                dropDownListRef.options.add(option1);
            if (splitter[i].split("^")[0].replace("$", "") == "Comments") {
                if (user.toUpperCase() == "PHYSICIAN" || user.toUpperCase() == "PHYSICIAN ASSISTANT") {
                    var keywrd = document.createElement("option");
                    keywrd.style.fontWeight = 'bolder';
                    keywrd.style.textDecoration = 'none';
                    keywrd.className = "alinkstyle";
                    keywrd.style.fontStyle = 'italic';
                    keywrd.style.cursor = 'default';
                    keywrd.text = 'Click here to Add or Update Keywords';
                    keywrd.value = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("dlcvalue").value;
                    dropDownListRef.options.add(keywrd);
                }
            }
        }

    }

    if (splitter == "") {
        if (user.toUpperCase() == "PHYSICIAN" || user.toUpperCase() == "PHYSICIAN ASSISTANT") {
            var keywrd = document.createElement("option");
            keywrd.style.fontWeight = 'bolder';
            keywrd.style.textDecoration = 'none';
            keywrd.className = "alinkstyle";
            keywrd.style.fontStyle = 'italic';
            keywrd.style.cursor = 'default';
            keywrd.text = 'Click here to Add or Update Keywords';
            keywrd.value = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("dlcvalue").value;
            dropDownListRef.options.add(keywrd);
        }
    }
    if ((flag == 1 || flag == 2) && valuetest != 'RELOAD#$&^') {
    }
    dropDownListRef.style.height = "150px";
    dropDownListRef.style.display = "block";
    dropDownListRef.className = "DLCList";

}

function licstboxclick(e) {
    var ID;
    var ID1;
    ID = e;
    ID1 = ID.replace("_listDLC", "_txtDLC");
    var txtbox = document.getElementById(ID1).value;
    var maxvalue = document.getElementById(ID1).attributes["maxlength"].value;
    var txtValue = '';
    var sMail = document.getElementById(lstCtrl.replace('_listDLC', '_txtDLC')).attributes.getNamedItem("dlcvalue").value;
    var index = document.getElementById(ID).selectedIndex;
    //Jira #CAP-193
    // CAP-288 - Prevanting innerHTML undefiend
    if (ID != null && ID != undefined && document.getElementById(ID) != null && document.getElementById(ID) != undefined && document.getElementById(ID).options.length > 0 && document.getElementById(ID).options[index] != null && document.getElementById(ID).options[index] != undefined && document.getElementById(ID).options[index]?.innerHTML?.trim() == "Click here to Add or Update Keywords") {
        var keyword = document.getElementById(ID).options[index].value;
        OpenPopup(keyword);
    }
    else if (document.getElementById(ID).options[index]?.innerHTML?.trim() == "Click to view more") {
        valuetest = "RELOAD#$&^";
        $.ajax({
            type: "POST",
            url: "frmDLC.aspx/GetListBoxValuesReason",

            data: "{'fieldName':'" + listvalue + "','value':'" + listoptions + valuetest + "'}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccessReason,
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
        document.getElementById(lstCtrl).style.zIndex = 17;
        if (control.childNodes[0] != undefined && control.childNodes[0].className != null)
            control.childNodes[0].className = "fa fa-minus margin2";
        else if (control.childNodes[0] != undefined && control.childNodes[0].nextSibling.className != null)
            control.childNodes[0].nextSibling.className = "fa fa-minus margin2";

    }
    else {
        if (document.getElementById(ID).options[index] != undefined)
            var txtValue = document.getElementById(ID).options[index].textContent;

        if (txtValue.length + txtbox.length > maxvalue) {
            return false;
        }
        else {
            if (ID.indexOf("txtPlan") == 0)//Plan If Start
            {
                var length = txtbox.length;
                var lastChar = txtbox.substring(length - 2, length);
                var y = txtValue.selectionEnd;
                if (txtbox == '' && txtbox.indexOf(txtValue) == -1) {
                    txtbox = txtValue;
                    document.getElementById(ID1).value = txtbox;
                    document.getElementById(ID1).onchange();
                    var step = document.getElementById(ID1 + '___livespell_proxy').scrollHeight;
                    document.getElementById(ID1 + '___livespell_proxy').scrollTop += step;
                }
                else if (txtbox.indexOf(txtValue) == -1) {
                    if (lastChar.trim() == "*") {
                        txtbox = txtbox.trim() + txtValue + "\n";
                    }
                    else if (lastChar.endsWith("\n")) {
                        txtbox = txtbox + '*' + txtValue + "\n";
                    }
                    else {
                        txtbox = txtbox + "\n" + '*' + txtValue + "\n";
                    }
                    document.getElementById(ID1).value = txtbox;
                    document.getElementById(ID1).onchange();
                    var step = document.getElementById(ID1 + '___livespell_proxy').scrollHeight;
                    document.getElementById(ID1 + '___livespell_proxy').scrollTop += step;
                }
                return false;
            }
            else {
                var retVal = false;
                if (e.indexOf("LastMammogram") != -1) {//BugID:47706
                    var TextBoxVal = document.getElementById('txtNotesLastMammogram_txtDLC').value;
                    var SelectedVal = $("#" + e)[0].value;
                    var Type = "";
                    $.ajax({
                        type: "POST",
                        url: "frmDLC.aspx/FindIfInMammogramTestList",
                        data: "{'SelectedText':'" + SelectedVal + "','TextBoxValue':'" + TextBoxVal + "','Type':'" + Type + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var result = response.d;
                            if (result == "showAlert") {
                                return false;
                            }
                            else {
                                if (txtbox == '' && txtbox.indexOf(txtValue) == -1) {
                                    txtbox = txtValue;
                                    document.getElementById(ID1).value = txtbox;
                                    document.getElementById(ID1).onchange();
                                }
                                else {
                                    var arr = txtbox.split(', ');
                                    for (var i = 0; i < arr.length; i++) {
                                        arr[i] = arr[i].trim();
                                    }
                                    if (arr.indexOf(txtValue) == -1) {
                                        txtbox = txtbox + ", " + txtValue;
                                        document.getElementById(ID1).value = txtbox;
                                        document.getElementById(ID1).onchange();
                                    }
                                }
                            }
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
                    if (txtbox == '' && txtbox.indexOf(txtValue) == -1) {
                        txtbox = txtValue;
                        document.getElementById(ID1).value = txtbox;
                        if (document.getElementById(ID1).onchange != null)
                            document.getElementById(ID1).onchange();
                    }
                    else {
                        var arr = txtbox.split(', ');
                        for (var i = 0; i < arr.length; i++) {
                            arr[i] = arr[i].trim();
                        }
                        if (arr.indexOf(txtValue) == -1) {
                            if (sMail == "EMAIL") {
                                if (txtbox == '') {
                                    txtbox = txtValue;
                                    document.getElementById(ID1).value = txtbox;
                                    document.getElementById(ID1).onchange();
                                }
                            }
                            else {
                                txtbox = txtbox + ", " + txtValue;
                                document.getElementById(ID1).value = txtbox;
                                document.getElementById(ID1).onchange();
                            }

                        }
                    }
                }

                if (e.toUpperCase().indexOf("HEIGHT") > -1 || e.toUpperCase().indexOf("WEIGHT") > -1) {
                    if (document.getElementById(ID).options[index] != undefined)
                        var txtValue = document.getElementById(ID).options[index].textContent;
                    if (document.getElementById(ID).selectedOptions[0].attributes.length > 0) {
                        var txtboxnew = "";
                        if (document.getElementById('txtNotesBMI_txtDLC') != undefined) {
                            txtboxnew = document.getElementById('txtNotesBMI_txtDLC').value
                            if (txtboxnew == '' && txtboxnew.indexOf(txtValue) == -1) {
                                document.getElementById('txtNotesBMI_txtDLC').value = txtValue;
                                document.getElementById(ID1).onchange();
                            }
                            else {
                                var flag = 0;
                                for (var i = 0; i < txtboxnew.split(',').length; i++) {
                                    if (txtboxnew.split(',')[i] == txtValue) {
                                        flag = 0;
                                    }
                                    else {
                                        flag = 1;
                                    }
                                }
                                if (flag) {
                                    txtboxnew = txtboxnew + ", " + txtValue;
                                    document.getElementById('txtNotesBMI_txtDLC').value = txtboxnew;
                                    document.getElementById(ID1).onchange();
                                }
                            }
                        }
                    }
                }
            }
            if (ID1 == "ctmDLCChief_Complaints_txtDLC") {
                var txtboxHPI = document.getElementById("ctmDLCHPI_Notes_txtDLC").value;
                var HPItxtValue = document.getElementById(ID).options[index].value;
                if (txtboxHPI == '' && txtboxHPI.indexOf(HPItxtValue) == -1) {
                    txtboxHPI = HPItxtValue;
                    document.getElementById("ctmDLCHPI_Notes_txtDLC").value = txtboxHPI;
                }
                else if (txtboxHPI.indexOf(HPItxtValue) == -1) {
                    txtboxHPI = txtboxHPI + ", " + HPItxtValue;
                    document.getElementById("ctmDLCHPI_Notes_txtDLC").value = txtboxHPI;
                }
            }
            if (document.getElementById(ID1).value.indexOf('??') != -1) {
                document.getElementById(ID1).setSelectionRange(document.getElementById(ID1).value.indexOf('??'), document.getElementById(ID1).value.indexOf('??') + 2);
            }
            var dlctxtbox = document.getElementById(ID1);
            var val = dlctxtbox.value
            dlctxtbox.value = "";
            dlctxtbox.value = val;
            dlctxtbox.scrollTop = dlctxtbox.scrollHeight;
        }
    }
}
var bFirst = true;
var bFocus = true;
function insertTab(textbox, event) {
    if (textbox.value.indexOf('??') != -1) {
        var keyChar = event.keyCode ? event.keyCode : event.charCode ? event.charCode : event.which;
        if (keyChar == 9 && !event.shiftKey && !event.ctrlKey && !event.altKey) {
            var oS = textbox.scrollTop;
            if (textbox.setSelectionRange) {
                var sStart = textbox.selectionStart;
                var sEnd = textbox.selectionEnd;
                var sString = textbox.value;
                //if (sStart == 0){
                //    sStart = sString.indexOf('??');
                //}
                //else
                {
                    var sFindvalue = sString.replace(sString.substring(0, (sStart + 2)), '')
                    var sNextvalue = sFindvalue.indexOf('??');
                    if (sNextvalue == -1) {
                        bFocus = true;
                        return true;
                    }
                    sStart = (sStart + 2) + sNextvalue;
                }
                //sStart = sString.indexOf('??');
                if (sStart != -1) {
                    sEnd = sStart + 2;
                    textbox.setSelectionRange(sStart, sEnd);
                    textbox.focus();
                }
            }
            textbox.scrollTop = oS;
            if (event.preventDefault) {
                event.preventDefault();
            }
            bFirst = true;
            return false;
        }
        else if (event.which === 9 && event.shiftKey) {
            var oS = textbox.scrollTop;
            if (textbox.setSelectionRange) {
                var sStart = textbox.selectionStart;
                var sEnd = textbox.selectionEnd;
                var sString = textbox.value;
                if (sStart == 0) {
                    bFocus = true;
                    return true;
                }


                //if (bFirst) {
                //    sStart = sString.lastIndexOf("??");
                //    bFirst = false;
                //    if (sStart != -1) {
                //        sEnd = sStart + 2;
                //        textbox.setSelectionRange(sStart, sEnd);
                //        textbox.focus();
                //    }
                //}
                //else 
                {
                    var sFindvalue = sString.replace(sString.substring((sStart) + 2, sString.length), '');

                    //sFindvalue = sFindvalue.replace(sFindvalue.substring(sFindvalue.length - 2, sFindvalue.length));
                    //sFindvalue =sString.replace(sString.substring(sStart, sString.length),'');
                    sFindvalue = sFindvalue.substring(0, sFindvalue.length - 2);//sFindvalue.replace(sFindvalue.substring(sFindvalue.length, sString.length - 2), '');
                    sStart = sFindvalue.lastIndexOf("??");
                    if (sStart == -1) {
                        bFocus = true;
                        return true;
                    }
                    if (sStart != -1) {
                        sEnd = sStart + 2;
                        textbox.setSelectionRange(sStart, sEnd);
                        textbox.focus();
                    }
                }

            }
            textbox.scrollTop = oS;
            if (event.preventDefault) {
                event.preventDefault();
            }
            return false;
        }
    }
    return true;
}

function focusTab(textbox, event) {
    if (bFocus) {
        if (textbox.value.indexOf('??') != -1) {
            {
                bFocus = false;
                textbox.setSelectionRange(0, 0);
                var oS = textbox.scrollTop;
                if (textbox.setSelectionRange) {
                    var sStart = textbox.selectionStart;
                    var sEnd = textbox.selectionEnd;
                    var sString = textbox.value;
                    sStart = sString.indexOf("??");
                    if (sStart != -1) {
                        sEnd = sStart + 2;
                        textbox.setSelectionRange(sStart, sEnd);
                        textbox.focus();
                    }

                    //if (sStart == 0)
                    //{
                    //    bFocus = true;
                    //    return true;
                    //}
                    //if (bFocus) {
                    //    sStart = sString.lastIndexOf("??");
                    //    bFocus = false;
                    //    if (sStart != -1) {
                    //        sEnd = sStart + 2;
                    //        textbox.setSelectionRange(sStart, sEnd);
                    //        textbox.focus();
                    //    }
                    //}
                    //else {
                    //    var sFindvalue = sString.replace(sString.substring((sStart) + 2, sString.length), '');
                    //    sFindvalue = sFindvalue.substring(0, sFindvalue.length - 2);
                    //    sStart = sFindvalue.lastIndexOf("??");
                    //    if (sStart == -1)
                    //    {
                    //        bFocus = true;
                    //        return true;
                    //    }
                    //    if (sStart != -1) {
                    //        sEnd = sStart + 2;
                    //        textbox.setSelectionRange(sStart, sEnd);
                    //        textbox.focus();
                    //    }
                    //}

                }
                textbox.scrollTop = oS;
                if (event.preventDefault) {
                    event.preventDefault();
                }
                return false;
            }
        }
        textbox.focus();
        var len = textbox.value.length;
        textbox.setSelectionRange(len, len);
        bFocus = true;
        return true;
    }
    textbox.focus();
    var len = textbox.value.length;
    textbox.setSelectionRange(len, len);
    bFocus = true;
}

