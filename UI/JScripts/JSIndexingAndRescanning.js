function mouseDown(e) {
    try {
        if (2 == event.button || 3 == event.button) return !1
    } catch (e) {
        if (3 == e.which) return !1
    }
}

function updateItems(e) { }

function pageLoad() {
    $("#dtpDocumentDate").datetimepicker({
        timepicker: !1,
        format: "d-M-Y",
        closeOnDateSelect: !0
    }).prop("readonly", !0), $("#dtpSpecCollection").datetimepicker({
        timepicker: !0,
        format: "d-M-Y  g:i A",
        formatTime: "g:i A",
        closeOnDateSelect: !0,
        onSelectDate: function (e, t) {
            $("#dtpDocumentDate").val(e.dateFormat("d-M-Y"))
        }
    });
}

function clearall() {
    var e = $("#btnClearAll").val();
    //CAP-1516
    localStorage.setItem('IsClearAllClick', 'Yes');
    if ("Reset" == e) {
        if (!DisplayErrorMessage("1105005")) {
            document.getElementById("hdnUpdateMode").value = "";
            return !1;
        }
        document.getElementById("hdnUpdateMode").value = "Reset",
        $(".sys").find("input[type=text],input[type=number]").each(function () {
            removeAttr("value")
        }),
        $("#txtSelectedPages").val("");
        $("#dtpSpecCollection").val("");
        $("#OrdersPanel :input").attr("disabled", !0);
        var t = document.getElementById("btnSave");
        t.disabled = !0;
        var t = document.getElementById("btnResetFields");
        t.click();
        CheckAll();
        document.getElementById("hdnIsEditgrid").value = "";
        //document.getElementById("ddEncPhyName").selectedIndex = 0;
        $("#ddEncPhyName").empty();
        $('#ddEncPhyName').val("");
    } else {
        if (!DisplayErrorMessage("1105006")) {
            document.getElementById("hdnUpdateMode").value = "";
            return !1;
        }
        document.getElementById("hdnUpdateMode").value = "Reset";
        $(".sys").find("input[type=text],input[type=number]").each(function () {
            removeAttr("value")
        });
        $("#txtSelectedPages").val("");
        $("#dtpSpecCollection").val("");
        $("#OrdersPanel :input").attr("disabled", !0);
        //CAP-969
        document.getElementById("btnMoveToNextProcess").disabled = false;
        $("#btnClearAll").val("Reset");
        $("#btnSave").val("Add");
        $("#btnSave")[0].disabled = true;
        //Cap - 1139
        document.getElementById("hdnIsEditgrid").value = "";
        document.getElementById("cboIs_Interperated").selectedIndex = 0;
        document.getElementById("cboDocumentType").selectedIndex = 9;
        var cboDocumentType = document.getElementById("cboDocumentType");
        __doPostBack('cboDocumentType', cboDocumentType);
        $("#cboDocumentSubType").empty();
        LoadSubDocType();
        $("#cboStandingOrders").empty();
        $("#cboPhysician").empty();
        $("#cboOrderPhysician").empty();
        $("#cboLab").empty();
        //Cap - 1139
        //document.getElementById("hdnIsEditgrid").value = "";
        $("#dOrder").removeAttr("data-target");
        $("#dOrder").removeClass("panel-headingIndexing");
        $("#dOrder").addClass("panel-headingdisable LabelStyle");
        $("#dOrderCollapse")[0].classList.remove("in");
        $('#ddEncPhyName').val("");
        $("#ddEncPhyName").empty();
        //document.getElementById("ddEncPhyName").selectedIndex = 0;
        CheckAll();
    }
    return true;
}

function btnCurrentPage_Clicked() {
    var e = document.getElementById("PageBox"),
        t = document.getElementById("txtSelectedPages");
    val = t.value, null == val || "" == val.trim() ? null != e.value && "" != e.value ? t.value = e.value : t.value = "1" : null != e.value && "" != e.value ? t.value = val + "," + e.value : t.value = val + ",1"
}

function btnFindPatient_Clicked() {
    //CAP-592
    var oWnd = GetRadWindow();
    var childWindow = oWnd.BrowserWindow.radopen("frmFindPatient.aspx", "RadWindow1");
    childWindow.SetModal(true);
    childWindow.set_visibleStatusbar(false);
    childWindow.setSize(1200, 251);
    childWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Close);
    childWindow.set_iconUrl("Resources/16_16.ico");
    childWindow.set_keepInScreenBounds(true);
    childWindow.set_centerIfModal(true);
    childWindow.center();

    childWindow.add_close(function PatientDemographicsClick(oWindow, args) {
        if ($(top.window.document).find("#txtPatientInformation")[0].value != null
            && $(top.window.document).find("#txtPatientInformation")[0].value != undefined
            && $(top.window.document).find("#txtPatientInformation")[0].value != "") {

            var n = JSON.parse($(top.window.document).find("#txtPatientInformation")[0].value);
            var a = "";
            var i = $("#PatientDetails");

            if (n.HumanId != undefined) {
                a = n.HumanId;
            }
            else if (n.Human_id != undefined) {
                a = n.Human_id;
            }
            i.val(n.PatientName + " | " + n.PatientDOB + " | " + n.PatientGender + "| Acc #:" + a), i.removeClass("patientPaneEnabled"), "0" != a &&
                (i.removeClass("patientPaneEnabled"), i.addClass("patientPaneDisabled"), document.getElementById("hdnHumanID").value = a, document.getElementById("PageBox").value = "1", document.getElementById("cboDocumentType").disabled = !1, document.getElementById("btnMoveToNextProcess").disabled = !1)
        }

        var e = $("#btnClearAll").val(),
            t = document.getElementById("btnSave");
        $("#txtSelectedPages").val(""),
            $("#dtpSpecCollection").val(""),
            $("#OrdersPanel :input").attr("disabled", !0),
            t.disabled = !0, $("#btnClearAll").val("Reset"), t.value = "Add";

        document.getElementById("cboIs_Interperated").selectedIndex = 0;
        document.getElementById("cboDocumentType").selectedIndex = 9;
        var cboDocumentType = document.getElementById("cboDocumentType");
        __doPostBack('cboDocumentType', cboDocumentType);
        $("#cboDocumentSubType").empty();
        LoadSubDocType();
        $("#cboStandingOrders").empty();
        $("#cboPhysician").empty();
        $("#cboOrderPhysician").empty();
        $("#cboLab").empty();

        $("#dOrder").removeAttr("data-target");
        $("#dOrder").removeClass("panel-headingIndexing");
        $("#dOrder").addClass("panel-headingdisable LabelStyle");
        $("#dOrderCollapse")[0].classList.remove("in");
        CheckAll();
    });
}

function SetRadWindowProperties(e, t, n) {
    e.SetModal(!0), e.set_visibleStatusbar(!1), e.setSize(n, t), e.set_behaviors(Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move), e.set_iconUrl("Resources/16_16.ico"), e.set_keepInScreenBounds(!0), e.set_centerIfModal(!0), e.center()
}



function btnSave_Clicked() {
    //CAP-1516
    localStorage.removeItem('IsClearAllClick');
    var e = $("#PatientDetails")[0].className;
    if ("patientPaneEnabled" == e) return DisplayErrorMessage("390006"), $("#hdnPageState").val(""), !1;
    if ($("#tbFilesBody tr.highlight").length == 0) {
        document.getElementById('divLoading').style.display = "none";
        StopLoadOnUploadFile();
        DisplayErrorMessage("114017");
        return false;
    }
    var t = $("#cboDocumentType :selected").text();
    if ("" == t) return DisplayErrorMessage("115043"), document.getElementById('divLoading').style.display = "none", $("#hdnPageState").val(""), !1;
    var n = $("#cboDocumentSubType :selected").text();
    if ("" == n) return DisplayErrorMessage("115044"), document.getElementById('divLoading').style.display = "none", $("#hdnPageState").val(""), !1;
    if (t.toUpperCase() == "ENCOUNTERS") {
        //Cap - 1290
        //if ($("#ddEncPhyName")[0].options.length == 1) return DisplayErrorMessage("115063"), document.getElementById('divLoading').style.display = "none", !1;
        if ($("#ddEncPhyName")[0].options.length == 1) return DisplayErrorMessage("115063"), document.getElementById("IsClickDirectUpload").value = "No", document.getElementById('divLoading').style.display = "none", !1;
        var vEncPhyName = $("#ddEncPhyName :selected").text();
        //Cap - 1290
        //if ("" == vEncPhyName) return DisplayErrorMessage("115064"), document.getElementById('divLoading').style.display = "none", !1;
        if ("" == vEncPhyName) return DisplayErrorMessage("115064"), document.getElementById("IsClickDirectUpload").value = "No", document.getElementById('divLoading').style.display = "none", !1;

    }
    var a = $("#txtSelectedPages").val(),
    i = parseInt($("#hdnPagecount")[0].value, 10);
    if ($("#rdbPageRange")[0].checked == true) {
        //CAP-1140
        var aNew = a.indexOf(',') >= 0 || a.indexOf('-') >= 0 ? a : parseInt(a || 0);
        if (aNew == 0) return DisplayErrorMessage("115045"), $("#hdnPageState").val(""), !1;
        if (aNew != 0) {
            for (var r = a.split(","), l = 0; l < r.length; l++) {
                if (r[l].indexOf("-") > -1) {
                    var o = r[l].split("-");
                    if (1 == o.length) return DisplayErrorMessage("115045"), $("#hdnPageState").val(""), !1;
                    if ("" == o[0] || "" == o[1]) return DisplayErrorMessage("115045"), $("#hdnPageState").val(""), !1;
                    if (o[0].parseInt > i || o[1].parseInt > i) return DisplayErrorMessage("115045"), $("#hdnPageState").val(""), !1;
                    if (o[0].parseInt > o[1].parseInt) return DisplayErrorMessage("115045"), $("#hdnPageState").val(""), !1;
                    if (o[1] > i) return alert("Entered page number exceeds the total no of pages"), $("#hdnPageState").val(""), !1;
                }
                else if (parseInt(r[l]) > i)
                    return alert("Entered page number exceeds the total no of pages"), $("#hdnPageState").val(""), !1;
            }
            //CAP-1221
            var regex = /\b0+\b(?!\d)/g; // regular expression to match one or more "0" characters not followed by any digit
            if (regex.test(a)) {
                return DisplayErrorMessage("115045"), $("#hdnPageState").val(""), !1;
            }
        }
    }
    var d = $("#cboStandingOrders");
    if (!d[0].disabled) {
        if ("" == d.val())
            return alert("Please select any of the outstanding order or create a new one"), $("#hdnPageState").val(""), !1;
        if ("Paper Order" != d.val()) {
            var c = $("#cboLab :selected").text();
            if ("" == c) return alert("Please select the Lab"), $("#hdnPageState").val(""), !1;
        }
        var so = $("#cboOrderPhysician :selected").text();
        if ("" == so) return alert("Please select the Ordering Physician."), $("#hdnPageState").val(""), !1;
        var s = $("#cboPhysician :selected").text();
        if ("" == s) return alert("Please select the Review Physician."), $("#hdnPageState").val(""), !1;
    }

    if ($("#tbFilesBody tr.highlight").length > 0)
        document.getElementById("hdnsourceFile").value = $("#tbFilesBody tr.highlight")[0].cells[2].innerText;

    return ShowLoading(), $("#hdnPageState").val("default"), !0
}

function txtSelectedPages_OnKeyPress() {
    var e = event.charCode;
    return e > 47 && 58 > e ? !0 : 45 == e || 44 == e ? !0 : (alert("Invalid Page Number(s)"), event.preventDefault(), !1)
}

function openImageViewer() {

}

function DeleteGrid() {
    var e = DisplayErrorMessage("115008");
    if (1 == e) {
        var t = document.getElementById("btnInvisible");
        t.click()
    } else args._cancel = !0
}

function ShowLoading() {
    document.getElementById("divLoading").style.display = "block"
}
function seteditflag() {
    document.getElementById("hdnIsEditgrid").value = "true";
}
function changeDocuments() {

}

function PatientClick() {
    ShowLoading()
}

function ProcedureClick() {
    ShowLoading()
}

function MoveToNextProcess(e) {
    var t = DisplayErrorMessage("115019", "Indexing", e);
    1 == t && (ShowLoading(), document.getElementById("InvisibleButton").click())
}

function CloseAndDisplayAlert() {

    DisplayErrorMessage("115018");
    var e = GetRadWindow();
    e.close();

}

function GetEndLocalTime() {
    document.getElementById(GetClientId("hdnEndLocalTime")).value = GetLocalTime(), document.getElementById(GetClientId("hdnNextDateAndTime")).value = GetLocalTime()
}

function btnMoveToNextProcess_Clicked() {
    //CAP-1516
    if (localStorage.getItem('IsClearAllClick') == null && localStorage.getItem('IsClearAllClick') == "") {
        localStorage.setItem('IsSaveClickedSucessfull', '');
    }
    localStorage.removeItem('IsClearAllClick');
    if (document.getElementById("hdnIsMyScan").value == "" && document.getElementById("grdIndexing").rows.length == 1) {
        var e = $("#PatientDetails")[0].className;
        if ("patientPaneEnabled" == e) return DisplayErrorMessage("390006"), document.getElementById('divLoading').style.display = "none", $("#hdnPageState").val(""), !1;
        var t = $("#cboDocumentType :selected").text();
        if ("" == t) return DisplayErrorMessage("115043"), document.getElementById('divLoading').style.display = "none", $("#hdnPageState").val(""), !1;
        var n = $("#cboDocumentSubType :selected").text();
        if ("" == n) return DisplayErrorMessage("115044"), document.getElementById('divLoading').style.display = "none", $("#hdnPageState").val(""), !1;
    }

    if (document.getElementById("grdIndexing").rows.length > 1) {
        if (DisplayErrorMessage("1105004") == true) {
            if ($("#tbFilesBody tr.highlight").length == 0) {
                document.getElementById('divLoading').style.display = "none";
                StopLoadOnUploadFile();
                DisplayErrorMessage("114017");
                return false;
            }
            else {
                if (document.getElementById("hdnIsMyScan").value != "" && document.getElementById("hdnIsMyScan").value == "true") {
                    if ($("#PatientDetails")[0] != null && $("#PatientDetails")[0] != undefined)
                        document.getElementById("hdnHumanID").value = $("#PatientDetails")[0].value.split(':')[1].split('|')[0].trim();

                    document.getElementById("hdnsourceFile").value = $("#tbFilesBody tr.highlight")[0].cells[2].innerText;
                    //For myscan direct upload take only grid line items.
                    ShowLoading();
                    document.getElementById("btnHDNMoveToNextProcess").click();
                    localStorage.setItem('IsSaveClickedSucessfull', "");

                }
                else {
                    if (localStorage.getItem('IsSaveClickedSucessfull') == null || localStorage.getItem('IsSaveClickedSucessfull') == "") {
                        ShowLoading();
                        document.getElementById("IsClickDirectUpload").value = "Yes";
                        document.getElementById("btnSave").disabled = false;
                        $('#btnSave')[0].click();
                        document.getElementById("btnSave").disabled = true;
                    }
                }
            }
            if (document.getElementById("hdnIsMyScan").value != "true") {
                if (localStorage.getItem('IsSaveClickedSucessfull') != null && localStorage.getItem('IsSaveClickedSucessfull') != "" && localStorage.getItem('IsSaveClickedSucessfull') == "Success") {
                    ShowLoading();
                    document.getElementById("btnHDNMoveToNextProcess").click();
                    localStorage.setItem('IsSaveClickedSucessfull', "");
                }
            }
        }
        else {
            return !1;
        }
    }
    else {//This part for direct move to next process//For Bug Id 58541 
        if ($("#tbFilesBody tr.highlight").length == 0) {
            document.getElementById('divLoading').style.display = "none";
            StopLoadOnUploadFile();
            DisplayErrorMessage("114017");
            return false;
        }
        else {
            if (localStorage.getItem('IsSaveClickedSucessfull') == null || localStorage.getItem('IsSaveClickedSucessfull') == "") {
                document.getElementById("IsClickDirectUpload").value = "Yes";
                document.getElementById("btnSave").disabled = false;
                $('#btnSave')[0].click();
                document.getElementById("btnSave").disabled = true;
            }
        }
        if (localStorage.getItem('IsSaveClickedSucessfull') == "Success") {
            ShowLoading();
            document.getElementById("btnHDNMoveToNextProcess").click();
            localStorage.setItem('IsSaveClickedSucessfull', "");
        }
    }
    document.getElementById("hdnIsEditgrid").value = "";

//Jira #CAP-889
    RemoveItem(document.URL, "ScanId");
}
function ClickMovetoNextProcess() {
    document.getElementById("IsClickDirectUpload").value = "No";

    document.getElementById("btnHDNMoveToNextProcess").click();
}
function GetRadWindow() {
    var e = null;
    return window.radWindow ? e = window.radWindow : window.frameElement.radWindow && (e = window.frameElement.radWindow), e
}

function btnSearchCpt_Clicked() {

    var e = document.getElementById("cboLab");
    if (null == e.value || 0 == e.value) return DisplayErrorMessage("115033"), !1;
    //var t = document.getElementById("cboPhysician");
    //if (null == t.value || "0" == t.value) return DisplayErrorMessage("115024"), !1;
    var t = document.getElementById("cboOrderPhysician");
    if (null == t.value || "0" == t.value) return DisplayErrorMessage("115065"), !1;

    var n = e.options[e.selectedIndex].value;
    $(top.window.document).find("#txtSearchProcedureInformation")[0].value = "";
    $(top.window.document).find("#TabSearchProcedure").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalSearchProcedureTitle")[0].textContent = "Search Procedure";
    $(top.window.document).find("#TabmdldlgSearchProcedure")[0].style.width = "900px";
    $(top.window.document).find("#TabmdldlgSearchProcedure")[0].style.height = "895px";
    var sPath = "frmSearchOrders.aspx?Screen_Name=Indexing&OrderType=DIAGNOSTIC ORDER&PhyID=" + t.value + "&LabID=" + n;
    $(top.window.document).find("#TabSearchProcedureFrame")[0].style.height = "895px";
    $(top.window.document).find("#TabSearchProcedureFrame")[0].contentDocument.location.href = sPath;
    $(top.window.document).find("#TabSearchProcedure").on('hide.bs.modal', function (e) {
        if ($(top.window.document).find("#txtSearchProcedureInformation")[0].value != null &&
            $(top.window.document).find("#txtSearchProcedureInformation")[0].value != undefined
            && $(top.window.document).find("#txtSearchProcedureInformation")[0].value != "") {
            var n = JSON.parse($(top.window.document).find("#txtSearchProcedureInformation")[0].value);
            document.getElementById(GetClientId("hdnProcedure")).value = n.SelectedItem;
            var i = document.getElementById(GetClientId("btnIVProcedure"));
            i.click()
        }
        $(top.window.document).find("#TabSearchProcedure").modal({ backdrop: "", keyboard: false }, 'hide');
    });

}

function OnClose(e, t) {
    var n = t.get_argument();
    if (e.setUrl("about:blank"), n) {
        var a = n.SelectedItem;
        if ("" != a) {
            document.getElementById(GetClientId("hdnProcedure")).value = a;
            var i = document.getElementById(GetClientId("btnIVProcedure"));
            i.click()
        }
    }
}

function validatedate(e, t) {
    var n = /(\d+)-([^.]+)-(\d+)/;
    if (e.match(n)) {
        var a = e.split("-");
        lopera2 = a.length;
        var i = parseInt(a[0]),
            r = parseInt(a[2]),
            l = "",
            o = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31],
            d = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
        if (-1 == d.indexOf(a[1].toUpperCase())) return alert("Invalid date format!"), $find(GetClientId(t)).clear(), $find(GetClientId(t)).focus(!0), !1;
        if (l = d.indexOf(a[1].toUpperCase()) + 1, (1 == l || l > 2) && i > o[l - 1]) return alert("Invalid date format!"), $find(GetClientId(t)).clear(), $find(GetClientId(t)).focus(!0), !1;
        if (2 == l) {
            var c = !1;
            if ((r % 4 || !(r % 100)) && r % 400 || (c = !0), 0 == c && i >= 29) return alert("Invalid date format!"), $find(GetClientId(t)).clear(), $find(GetClientId(t)).focus(!0), !1;
            if (1 == c && i > 29) return alert("Invalid date format!"), $find(GetClientId(t)).clear(), $find(GetClientId(t)).focus(!0), !1
        }
        var s = new Date,
            u = s.getFullYear();
        if (r > u) return alert("DOB cannot be future date. Please Enter the Valid Year."), $find(GetClientId(t)).clear(), $find(GetClientId(t)).focus(!0), !1
    }
}

function CloseWindow() {
    var e = GetRadWindow();
    e.close()
}

function ChangePg(e) {
    //next Image
    if (parseInt(document.getElementById("PageBox").value) <= parseInt($("#PageLabel")[0].innerText.replace("/", "").trim()) && e.title.toUpperCase() == "NEXT IMAGE") {
        var ePageBox = $("#PageLabel")[0].innerText,
            tvalue = $("#PageBox"),
            n = tvalue.val();
        if ($("#PageLabel")[0].innerText.replace("/", "").trim() != "1" && parseInt(document.getElementById("PageBox").value) < $("#PageLabel")[0].innerText.replace("/", "").trim()) {
            n++;
            $("#PageBox")[0].value = n;
        }
    }//previous Image
    else if (parseInt(document.getElementById("PageBox").value) <= parseInt($("#PageLabel")[0].innerText.replace("/", "").trim()) && e.title.toUpperCase() == "PREVIOUS IMAGE") {
        var ePageBox = $("#PageBox"),
           tvalue = ePageBox.val();
        if ($("#PageLabel")[0].innerText.replace("/", "").trim() != "1" && tvalue != "1") {
            tvalue--;
            $("#PageBox")[0].value = tvalue;
        }
    }
    Src = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Pg=" + $("#PageBox").val() + "&Height=" + GetBigSrc("Height") + "&Width=" + GetBigSrc("Width");
    SrcBig = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Pg=" + $("#PageBox").val() + "&Height=1000&Width=1000";
    SrcRevert = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Pg=" + $("#PageBox").val() + "&Height=600&Width=600";
    SrcNavigate = "ViewImg.aspx?View=1&FilePath=" + GetBigSrc("FilePath") + "&Height=600&Width=600";
    document.getElementById("_imgBig").style.opacity = 0;
    document.getElementById("_imgBig").style.transition = "opacity 4s";
    document.getElementById("_imgBig").src = Src;
    document.getElementById("_imgBig").style.opacity = 1;
    document.getElementById("zoomin").onclick = function () {
        ChangePg(this), document.getElementById("_imgBig").src = SrcBig + "&Rotate" + GetBigSrc("Rotate")
    };
    document.getElementById("zoomout").onclick = function () {
        ChangePg(this), document.getElementById("_imgBig").src = SrcRevert
    }
}

function print() {
    var e = document.getElementById("_imgBig");
    printWindow = window.open(), printWindow.document.write("<div style='width:70%;margin:0'>"), printWindow.document.write("<img id='img' src='" + e.src + '\'onload="javascript:window.print();"/>'), printWindow.document.write("</div>"), printWindow.setTimeout(function () {
        printWindow.close()
    }, 1e4)
}

function GetBigSrc(e) {
    var t = document.getElementById("_imgBig").src;
    for (gy = t.split("&"), i = 0; i < gy.length; i++)
        if (ft = gy[i].split("="), ft[0] == e) return ft[1]
}

function SelectOrders() {

    var e = $("#PatientDetails")[0].className;
    if ("patientPaneEnabled" == e) {
        alert("Please Select Patient first, to get outstanding orders"), event.preventDefault();
        $("#cboDocumentType").val();
        return !1
    }

    var exists = false;
    $('#cboStandingOrders option').each(function () {
        if (this.value.indexOf("|") == -1 && this.value != "Paper Order") {

            $("#cboStandingOrders option[value='" + this.value + "']").remove();
        }
    });
    var cookies = document.cookie.split(';');
    var CFacilityName = "";
    var re = /%20/gi;
    for (var l = 0; l < cookies.length; l++) {
        if (cookies[l].indexOf("CFacilityName") > -1)
            CFacilityName = cookies[l].split("=")[1];
    }
    CFacilityName = CFacilityName.replace(re, ' ');

    var t = document.getElementById("cboStandingOrders"),
        n = document.getElementById("cboPhysician"),
        a = document.getElementById("cboLab"),
        i = document.getElementById("dtpSpecCollection"),
        r = document.getElementById("cboIs_Interperated"),
        l = t.options[t.selectedIndex].value,
        o = document.getElementById("chkShowAll"),
        d = document.getElementById("btnSearchCpt"),
        orpsa = document.getElementById("chkOrderingPhyShowAll"),
        orp = document.getElementById("cboOrderPhysician");

    if (l == "Paper Order") {
        orpsa.disabled = false;
        $("#slabMandatory").removeClass("manredforstar");
        $("#Labspan").removeClass("MandLabelstyle");
        // if (CFacilityName == "CMG LAB AND ANCILLARY 1866 #101")
        if (CFacilityName ==$("#hdnIsAncillary").val())
        {
            $('#chkShowAll')[0].checked = true;
        }
        else
        {
            $('#chkShowAll')[0].checked = false;
        }
       
        if ($("#slabMandatory")[0] != undefined)
            $("#slabMandatory")[0].style.visibility = "hidden";

    }
    else {
        orpsa.disabled = true;
        $("#slabMandatory").addClass("manredforstar");
        if ($("#slabMandatory")[0] != undefined)
            $("#slabMandatory")[0].style.visibility = "visible";
        $("#Labspan").addClass("MandLabelstyle");
        // if (CFacilityName == "CMG LAB AND ANCILLARY 1866 #101")
        if (CFacilityName == $("#hdnIsAncillary").val())
        {
            $('#chkShowAll')[0].checked = true;
        }
        else {
            $('#chkShowAll')[0].checked = false;
        }

    }
    if ("Paper Order" != l) {
        var c = l.split("|");
        // n.value = c[0], n.disabled = !0, a.value = c[1], a.disabled = !0;
        orp.value = c[0], orp.disabled = !0, a.value = c[1], a.disabled = !0;
        //if (n.value == "") {
        //    $('select[name="cboPhysician"]').find('option[value="4387"]').attr("selected", "selected");
        //    n.value = 4387;
        //}
        n.disabled = false;
        n.selectedIndex = 0;
        o.disabled = false;
        var s = t.options[t.selectedIndex].text;
        s.split("|");
        return t.disabled = !1, d.disabled = !0, 4 == c.length ? (i.value = c[3], i.disabled = !0) : (i.value = "", i.disabled = !0), r.disabled = !1, t.title = t.options[t.selectedIndex].text, !1
    }
    n.disabled = false;
    n.selectedIndex = 0;
    
    //i.disabled = !1, n.disabled = !1, a.disabled = !1, n.selectedIndex = 0, a.selectedIndex = 0, d.disabled = !1, r.disabled = !1, o.disabled = !1, $("#cboPhysician > option").each(function () {
    i.disabled = !1, orp.disabled = !1, a.disabled = !1, orp.selectedIndex = 0, a.selectedIndex = 0, d.disabled = !1, r.disabled = !1, o.disabled = !1, $("#cboOrderPhysician > option").each(function () {
        var e = $(this);
        "true" == e.attr("default") ? e.css("display", "block") : e.css("display", "none")
    });
    //if (l == "Paper Order") {
    //    $('select[name="cboPhysician"]').find('option[value="4387"]').attr("selected", "selected");
    //    n.value = 4387;
    //}
    var u;
    return u = getLocalTime(), $("#dtpSpecCollection").datetimepicker({
        timepicker: !0,
        format: "d-M-Y  g:i A",
        formatTime: "g:i A",
        maxDate: 0,
        value: u
    }), !0
}

function getLocalTime() {
    var e = new Date,
        t = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"),
        n = e.getDate(),
        a = e.getMinutes(),
        i = e.getHours(),
        r = t[e.getMonth()],
        l = e.getFullYear();
    10 > n && (n = "0" + n), 10 > r && (r = "0" + r);
    var o = "AM",
        d = i;
    d > 12 && (d = i - 12, o = "PM", 10 > d && (d = "0" + d)), 10 > a && (a = "0" + a), 0 == d && (d = 12);
    var e = n + "-" + r + "-" + l + " " + d + ":" + a + " " + o;
    return e
}

function getLocalDate() {
    var e = new Date,
        t = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"),
        n = e.getDate(),
        a = e.getMinutes(),
        i = e.getHours(),
        r = t[e.getMonth()],
        l = e.getFullYear();
    10 > n && (n = "0" + n), 10 > r && (r = "0" + r);
    var o = "AM",
        d = i;
    d > 12 && (d = i - 12, o = "PM", 10 > d && (d = "0" + d)), 10 > a && (a = "0" + a), 0 == d && (d = 12);
    var e = n + "-" + r + "-" + l;
    return e
}

function confirmDelete() {
    if (confirm("Are you sure you want to delete this?")) {
        ShowLoading();
        $("#txtSelectedPages").val("");
        $("#dtpSpecCollection").val("");
        $("#OrdersPanel :input").attr("disabled", !0);
        document.getElementById("cboIs_Interperated").selectedIndex = 0;
        document.getElementById("cboDocumentType").selectedIndex = 9;
        var cboDocumentType = document.getElementById("cboDocumentType");
        __doPostBack('cboDocumentType', cboDocumentType);
        $("#cboDocumentSubType").empty();
        LoadSubDocType();
        $("#cboStandingOrders").empty();
        $("#cboPhysician").empty();
        $("#cboOrderPhysician").empty();
        $("#cboLab").empty();
        $("#dOrder").removeAttr("data-target");
        $("#dOrder").removeClass("panel-headingIndexing");
        $("#dOrder").addClass("panel-headingdisable LabelStyle");
        $("#dOrderCollapse")[0].classList.remove("in");
        $('#rdbAll')[0].checked = true;
        PageRangeAllchange();
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}

function viewImage(e) {


    var t = document.getElementById("grdIndexing"),
        n = t.rows[e.parentNode.parentNode.rowIndex].cells[9].innerText;
    $(top.window.document).find("#txtViewImageIndexingInformation")[0].value = "";
    $(top.window.document).find("#TabViewImageIndexing").modal({ backdrop: "static", keyboard: false }, 'show');
    $(top.window.document).find("#TabModalViewImageIndexingTitle")[0].textContent = "Image Viewer -Indexing";
    $(top.window.document).find("#TabmdldlgViewImageIndexing")[0].style.width = "975px";
    $(top.window.document).find("#TabmdldlgViewImageIndexing")[0].style.height = "860px";
    $(top.window.document).find("#TabViewImageIndexingFrame")[0].style.height = "895px";
    $(top.window.document).find("#TabViewImageIndexingFrame")[0].contentDocument.location.href = "frmImageViewer.aspx?FileName=" + encodeURIComponent(n) + "&Source=INDEX";
    return false;


}

function showAllPhy(e) {
    e.firstChild.checked ? $("#cboPhysician > option").each(function () {
        $(this).css("display", "block")
    }) : $("#cboPhysician > option").each(function () {
        var e = $(this);
        "true" == e.attr("default") || "" == e.attr("default") ? e.css("display", "block") : e.css("display", "none")
    })
}

function showAllOrderingPhy(e) {
    e.firstChild.checked ? $("#cboOrderPhysician > option").each(function () {
        $(this).css("display", "block")
    }) : $("#cboOrderPhysician > option").each(function () {
        var e = $(this);
        "true" == e.attr("default") || "" == e.attr("default") ? e.css("display", "block") : e.css("display", "none")
    })
}

function btnClose_Clicked() {
    if (document.getElementById("btnSave").disabled && document.getElementById("btnMoveToNextProcess").disabled) {

        self.close();
        return false;
    } else {
        $("body").append("<div id='dvdialogMenu' style='min-height: 65px !important; width: auto; max-height: none; height: auto; display: none;'>" +
                               "<p style='font-family: Verdana,Arial,sans-serif; font-size: 12.5px;'>There are unsaved changes.Do you want to save them?</p></div>")
        dvdialog = $('#dvdialogMenu');
        myPos = "center center";
        atPos = 'center center';
        event.preventDefault();

        $(dvdialog).dialog({
            modal: true,
            title: "Capella EHR",
            position: {
                my: 'left' + " " + 'center',
                at: 'center' + " " + 'center + 100px'

            },
            buttons: {
                "Yes": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    if (!document.getElementById("btnSave").disabled)
                        document.getElementById("btnSave").click();
                    else
                        document.getElementById("btnMoveToNextProcess").click();
                     //Jira #CAP-889
                    //"" != $("#hdnPageState").val() && (DisplayErrorMessage("1007001"), self.close())
                    //Jira #CAP-593
                    //if ($("#hdnPageState").val()!="" && DisplayErrorMessage("1007001")) {
                    if ($("#hdnPageState").val() != "") {
                        DisplayErrorMessage("1007001");
                        //OnClientCloseWindow();
                        //self.close();
                    }

                    return false;
                },
                "No": function () {
                    $(dvdialog).dialog("close");
                    $(dvdialog).remove();
                    self.close();
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
}



function OnClientCloseValidation(e, t) {
    var n = t.get_argument();
    if (e.remove_close(OnClientCloseValidation), n) {
        var a = n;
        if ("Yes" == a) {
            if (!document.getElementById("btnSave").disabled)
                document.getElementById("btnSave").click();
            else
                document.getElementById("btnMoveToNextProcess").click();
            "" != $("#hdnPageState").val() && (DisplayErrorMessage("1007001"), self.close())
        } else if ("Cancel" == a);
        else if ("No" == a) {
            var i = GetRadWindow();
            i.close()
        }
    }
}

function GetRadWindow() {
    var e = null;
    return window.radWindow ? e = window.radWindow : window.frameElement.radWindow && (e = window.frameElement.radWindow), e
}

function Close() {
    var e = GetRadWindow();
    return e.argument = null, e.close(), !1
}

$(document).ready(function () {

    $("#hdnScreenMode")[0].value = "Bulk Scanning and Fax";
    if ($("#hdnScreenMode")[0].value == "Transactional Scanning") {
        ToggleButton("Transactional Scanning", "false");
    }
    else if ($("#hdnScreenMode")[0].value == "Bulk Scanning and Fax") {
        ToggleButton("Bulk Scanning and Fax", "false");
    }
    document.getElementById("btnSave").disabled = true;
    document.getElementById("txtSelectedPages").disabled = true;
    $("#txtSelectedPages").val("");
    if (document.getElementById("hdnIsMyScan").value == "" && $("#btnFindPatient")[0].disabled == false)
        document.getElementById("btnMoveToNextProcess").disabled = true;
    if ($('#rdbRemoteDir')[0].checked == false) {
        $("#dtpScannedDate")[0].disabled = true;
        $("#ddlSourceType")[0].disabled = true;
        $("#btnFindDocuments")[0].disabled = true;
        //temporary
        $("#ddSelectedFacility")[0].disabled = true;
    }
    else {
        $("#dtpScannedDate")[0].disabled = false;
        $("#ddlSourceType")[0].disabled = false;
        $("#btnFindDocuments")[0].disabled = false;
        $("#ddSelectedFacility")[0].disabled = false;
    }

    var e = 0;
    jQuery.fn.rotate = function (e) {
        $("#_imgBig").css({
            "-webkit-transform": "rotate(" + e + "deg)"
        })
    }, $("#leftrotate").click(function () {
        e -= 90, $("#leftrotate").rotate(e)
    }), $("#zoomin").click(function () {
        var e = 10,
            t = parseInt($("#_imgBig").width());
        $("#_imgBig").width(t + e + "px");
        var n = parseInt($("#_imgBig").height());
        $("#_imgBig").height(n + e + "px")
    }), $("#zoomout").click(function () {
        var e = 10,
            t = parseInt($("#_imgBig").width());
        $("#_imgBig").width(t - e + "px");
        var n = parseInt($("#_imgBig").height());
        $("#_imgBig").height(n - e + "px")
    }), $("#revert").click(function () {
        $("#_imgBig").css("width", ""), $("#_imgBig").css("height", "")
    }), $("#rotateright").click(function () {
        e += 90, $("#rotateright").rotate(e)
    }), $("#_imgBig").css("opacity", "1");



    if (document.getElementById('hdnIsMyScan').value == "true") {
        OnLoadGridMyscan();
    }
    else {
        OnLoadGrid("");

    }

});



function ViewPDF() {
    document.getElementById('imgControls').style.display = "none";
    document.getElementById('PDFholder').style.display = "block";
    document.getElementById('bigImagePDF').style.display = "block";
    document.getElementById('imgholder').style.display = "none";
    $('#bigImagePDF').show();
}

function displayAlert() {
    DisplayErrorMessage("115056");
    document.getElementById('divLoading').style.display = "none";
}
function displaySelectedPagesAlert() {
    DisplayErrorMessage("115045");
    document.getElementById('divLoading').style.display = "none";
}

function StopLoadOnUploadFile() {
    sessionStorage.setItem('StartLoading', 'false');
    StopLoadFromPatChart();
}


function rdbDirectoryChange() {
    if ($('#rdbRemoteDir')[0].checked == false) {
        $("#dtpScannedDate")[0].disabled = true;
        $("#ddlSourceType")[0].disabled = true;
        $("#btnFindDocuments")[0].disabled = true;
        $("#ddSelectedFacility")[0].disabled = true;
        document.getElementById("_imgBig").src = "";
        document.getElementById("bigImagePDF").src = "";
        document.getElementById('fileThumbs').innerHTML = "";
        document.getElementById("PageLabel").innerText = "/ 0";
        document.getElementById("btnSave").disabled = true;
        document.getElementById("btnMoveToNextProcess").disabled = true;
        $('#rdbAll')[0].checked = true;
    }
    else {
        $("#dtpScannedDate")[0].disabled = false;
        $("#ddlSourceType")[0].disabled = false;
        $("#btnFindDocuments")[0].disabled = false;
        $("#ddSelectedFacility")[0].disabled = false;
        document.getElementById("_imgBig").src = "";
        document.getElementById("bigImagePDF").src = "";
        document.getElementById('fileThumbs').innerHTML = "";
        document.getElementById("PageLabel").innerText = "/ 0";
        document.getElementById("btnSave").disabled = true;
        document.getElementById("btnMoveToNextProcess").disabled = true;
        $('#rdbAll')[0].checked = true;
    }
}



function PageRangeAllchange() {
    if ($('#rdbAll')[0].checked == true) {
        document.getElementById("btnSave").disabled = true;
        document.getElementById("txtSelectedPages").disabled = true;
        $("#txtSelectedPages").val("");
        document.getElementById("btnMoveToNextProcess").disabled = false;
    }
    else if ($('#rdbPageRange')[0].checked == true) {
        document.getElementById("btnSave").disabled = false;
        document.getElementById("txtSelectedPages").disabled = false;
        document.getElementById("btnMoveToNextProcess").disabled = true;
    }
}

function ClickClear() {
    $('#btnclearAfterupload')[0].click();
    $('#rdbAll')[0].checked = true;
    document.getElementById("btnSave").disabled = !1;
    document.getElementById("txtSelectedPages").disabled = true;
    $("#txtSelectedPages").val("");
    document.getElementById("btnMoveToNextProcess").disabled = false;
    $("#PatientDetails").val("");
}



function DisabledSelection() {
    if (document.getElementById("grdIndexing").rows.length > 1) {
        $('#fileThumbs')[0].disabled = true;
        $('#SelectDir').prop("disabled", true);
        document.getElementById('fileThumbs').style.pointerEvents = 'none';
        $("#fileThumbs").children().attr("disabled", "disabled");
    }
}


function EnabledSelection() {
    $('#fileThumbs')[0].disabled = false;
    $('#SelectDir').prop("disabled", false);
    $("#fileThumbs").children().removeAttr("disabled");
    document.getElementById('fileThumbs').style.pointerEvents = 'auto';
    $("#dtpScannedDate")[0].disabled = false;
    $("#ddlSourceType")[0].disabled = false;
    $("#btnFindDocuments")[0].disabled = false;
    $("#ddSelectedFacility")[0].disabled = false;
    var t = document.getElementById("btnFindDocuments");
    t.click();
}

function CheckAll() {
    $('#rdbAll')[0].checked = true;
    document.getElementById("btnSave").disabled = true;
    document.getElementById("txtSelectedPages").disabled = true;
    $("#txtSelectedPages").val("");
    document.getElementById("btnMoveToNextProcess").disabled = false;
}

function deletefiles(filename) {
    filename = filename.replace(/\\+/g, '/').replace(/\/+/g, '/');
    if (filename.indexOf("/") == 0)
        filename = '/' + filename;
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        async: true,
        url: "frmIndexing.aspx/SourceImageDelete",
        data: '{fileName: "' + filename + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "Success") {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                alert(data.d);
                return false;
            }
            else {
                if (confirm("Are you sure you want to delete the file Permanently?")) {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    document.getElementById("hdnfilenamedelete").value = filename;
                    document.getElementById("btnhdnloadfile").click();
                }
                else {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
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
        }
    });
    return false;
}



function DeleteThumbnails(FullFilePath, FirstFileName, FirstFilePath) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    FullFilePath = FullFilePath.replace(/\\+/g, '/').replace(/\/+/g, '/');
    if (FullFilePath.indexOf("/") == 0)
        FullFilePath = '/' + FullFilePath;
    $.ajax({
        type: "POST",
        async: true,
        url: "frmIndexing.aspx/SourceImageDelete",
        data: '{fileName: "' + FullFilePath + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "Success") {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                alert(data.d);
                return false;
            }
            else {
                if (confirm("Are you sure you want to delete the selected page from the file permanently?")) {
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    document.getElementById("hdnfilenamedelete").value = FullFilePath;
                    if ($("#PageLabel")[0].innerText.replace("/", "").trim() == "1") {
                        //delete source file from folder if the image have only one page
                        document.getElementById("btnhdnloadfile").click();
                    }
                    else {
                        document.getElementById("hdndeletePgNo").value = document.getElementById("PageBox").value;

                        $.ajax({
                            type: "POST",
                            async: true,
                            url: "frmIndexing.aspx/DeleteThumbnail",
                            data: '{ImagePath: "' + document.getElementById('hdnfilenamedelete').value + '",delPgNo: "' + document.getElementById("hdndeletePgNo").value + '"}',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {

                                if (data.d.indexOf("CheckFileNotFoundException")) {
                                    alert(data.d.Split('~')[1]);
                                }

                                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                                GridOpenFile(FirstFileName, FirstFilePath.replace(FirstFileName, ""), "");
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
                            }
                        });
                    }
                }
                else {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
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
        }
    });
    return false;
}


function ToggleButton(sMode, IsRdbClick) {
    if (sMode == "Transactional Scanning") {
        $("#dRemoteDir").removeAttr("data-target");
        $("#dRemoteDir").removeClass("panel-headingIndexing");
        $("#dRemoteDir").addClass("panel-headingdisable");

        $("#dLocalDir").attr("data-target", "#dLocalDirCollapse");
        $("#dLocalDir").removeClass("panel-headingdisable");
        $("#dLocalDir").addClass("panel-headingIndexing");

        $("#dRemoteDirCollapse")[0].classList.remove("in");
        //By default Open accordion window 
        $("#dLocalDirCollapse")[0].classList.add("in");
        if ($('#dRemoteDirCollapse')[0].classList.contains('in') == false) {
            $("#dtpScannedDate")[0].disabled = true;
            $("#ddlSourceType")[0].disabled = true;
            $("#btnFindDocuments")[0].disabled = true;
            $("#ddSelectedFacility")[0].disabled = true;
        }
        else {
            $("#dtpScannedDate")[0].disabled = false;
            $("#ddlSourceType")[0].disabled = false;
            $("#btnFindDocuments")[0].disabled = false;
            $("#ddSelectedFacility")[0].disabled = false;
        }
        $("#btnMovetoNonMedicalFolder").css('visibility', 'hidden');
    }
    else if (sMode == "Bulk Scanning and Fax") {
        if ($('#rdbRemoteDir')[0].checked == true) {


            //By default Open accordion window 
            $("#dRemoteDir").attr("data-target", "#dRemoteDirCollapse");
            $("#dRemoteDirCollapse")[0].classList.add("in");

            //Remove Local Directory
            $('#rdbLocalDir')[0].checked = false;
            $("#dLocalDir").removeAttr("data-target");
            $("#dLocalDirCollapse")[0].classList.remove("in");

            //enable non medical folder
            if ($("#ddlSourceType")[0].value == "FAX") {
                $("#btnMovetoNonMedicalFolder").show();
                $("#btnMovetoNonMedicalFolder").css('visibility', 'visible');
            }
            else
                $("#btnMovetoNonMedicalFolder").css('visibility', 'hidden');
        }
        else if ($('#rdbLocalDir')[0].checked == true) {

            //By default Open accordion window 
            $("#rdbLocalDir").attr("data-target", "#dLocalDirCollapse");
            $("#dLocalDirCollapse")[0].classList.add("in");

            //Remove Local Directory
            $('#dRemoteDir')[0].checked = false;
            $("#dRemoteDir").removeAttr("data-target");
            $("#dRemoteDirCollapse")[0].classList.remove("in");


            $("#btnMovetoNonMedicalFolder").css('visibility', 'hidden');
        }
        //Enable the controls
        $("#dtpScannedDate")[0].disabled = false;
        $("#ddlSourceType")[0].disabled = false;
        $("#btnFindDocuments")[0].disabled = false;
        $("#ddSelectedFacility")[0].disabled = false;
    }

    if (IsRdbClick == "true") {
        document.getElementById("_imgBig").src = "";
        document.getElementById("bigImagePDF").src = "";
        document.getElementById("hdnFileName").value = "";
        document.getElementById('fileThumbs').innerHTML = "";
        document.getElementById("PageLabel").innerText = "/ 0";
        document.getElementById("PageBox").value = "0";
        document.getElementById("btnSave").disabled = true;
        document.getElementById("btnMoveToNextProcess").disabled = false;

        document.getElementById("cboDocumentType").selectedIndex = 9;
        $("#cboDocumentSubType").empty();
        LoadSubDocType();

        $("#grdIndexing tr.Editabletxtbox").remove();
        $("#cboStandingOrders").empty();
        $("#cboPhysician").empty();
        $("#cboOrderPhysician").empty();
        $("#cboLab").empty();
        $("#dOrder").removeAttr("data-target");
        $("#dOrder").removeClass("panel-headingIndexing");
        $("#dOrder").addClass("panel-headingdisable LabelStyle");
        $("#dOrderCollapse")[0].classList.remove("in");

        if ($('#rdbRemoteDir')[0].checked == true && $("#ddlSourceType")[0].value == "FAX") {
            $("#btnMovetoNonMedicalFolder").show();
            $("#btnMovetoNonMedicalFolder").css('visibility', 'visible');
        }
        else {
            $("#btnMovetoNonMedicalFolder").css('visibility', 'hidden');
        }
    }
}


function MovetoNonMedicalFolder() {
    if ($("#tbFilesBody tr.highlight").length == 0) {
        alert("Please Select a file.");
        return false;
    }
    var filename = $("#tbFilesBody tr.highlight")[0].cells[2].innerText;
    filename = filename.replace(/\\+/g, '/').replace(/\/+/g, '/');

    if (filename.indexOf("/") == 0)
        filename = '/' + filename;

    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    $.ajax({
        type: "POST",
        async: true,
        url: "frmIndexing.aspx/SourceImageDelete",
        data: '{fileName: "' + filename + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d != "Success") {
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                alert("Selected File is already being indexed. So the selected file cannot be move to Non medical folder.");
                return false;
            }
            else {
                if (confirm("Are you sure you want to move this file to Non Medical folder permanently?")) {
                    var u = document.getElementById("fileThumbs");
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    document.getElementById("hdnfilenamedelete").value = filename;
                    document.getElementById("hdnMoventoNonmedicalFolder").click();
                }
                else {
                    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
                }
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
        }
    });
    return false;
}


function ChangeDocdate() {
    $("#dtpDocumentDate").val($("#dtpScannedDate").val());
}

function CheckFileIncomplete() {
    var e = DisplayErrorMessage("115061");
    if (1 == e) {
        var t = document.getElementById("btnInvisible");
        t.click();
    } else {


    }
}



function HideOrders() {
    $("#cboStandingOrders")[0].disabled = true;
    $("#OrdersPanel")[0].disabled = true;
    $("#dtpSpecCollection")[0].disabled = true;
    $("#btnSearchCpt")[0].disabled = true;
    $("#chkShowAll")[0].checked = false;
    $("#dtpSpecCollection")[0].value = "";
    $("#spanoutstandorder").addClass("spanstyle");
    $("#spanoutstandorder").css('visibility', 'hidden');
    $("#Labspan").addClass("spanstyle");
    $("#slabMandatory").css('visibility', 'hidden');
    $("#spanphy").addClass("spanstyle");
    $("#spanphystar").css('visibility', 'hidden');
    $("#dOrder").removeAttr("data-target");
    $("#dOrder").removeClass("panel-headingIndexing");
    $("#dOrder").addClass("panel-headingdisable LabelStyle");
}

function VisibleOrders() {
    $("#cboStandingOrders")[0].disabled = false;
    $("#OrdersPanel")[0].disabled = false;
    $("#dtpSpecCollection")[0].disabled = false;
    $("#btnSearchCpt")[0].disabled = false;
    $("#chkShowAll")[0].checked = false;

    //By default Open accordion window 
    $("#dOrder").attr("data-target", "#dOrderCollapse");
    $("#dOrderCollapse")[0].classList.add("in");
    $("#dOrder").addClass("panel-headingIndexing LabelStyle");
}


function OnLoadGrid(lastindexfilename) {
    var isremote, typeofscan, sselecteddate, sfacility = "";
    if ($('#rdbRemoteDir')[0].checked == true) {
        isremote = "true";
    }
    else {
        isremote = "false";
    }
    typeofscan = $("#ddlSourceType")[0].value;
    sselecteddate = $("#dtpScannedDate").val();
    sfacility = $("#ddSelectedFacility")[0].value;
    var FirstFileName, FirstFilePath;
    $.ajax({
        type: "POST",
        async: true,
        url: "frmIndexing.aspx/LoadGrid",
        data: '{IsRemote: "' + isremote + '",TypeofScan: "' + typeofscan + '",sSelectedDate: "' + sselecteddate + '",sFacility: "' + sfacility + '",sLastIndexFile: "' + lastindexfilename + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            StopLoadOnUploadFile();
            $('#tblFiles tr').removeClass("highlight");
            var tabContents = "";
            var objdata = $.parseJSON(data.d);
            var vdelete = "<td></td>";
            $("#tblFiles tr").empty();
            //CAP-280 - Null handling if objdata is null or undefined
            if (objdata?.FilesList?.length??0 > 0) {
                for (var i = 0; i < objdata.FilesList.length; i++) {

                    if (objdata.IsDeleteEnable != null && objdata.IsDeleteEnable != "" && objdata.IsDeleteEnable == "true") {
                        vdelete = "<td title='Delete' style='width: 3%;'><img style='width: 12px;' src='Resources/Delete-Blue.png' onclick=\"deletefiles('" + objdata.FilePath + objdata.FilesList[i].Scanned_File_Name + "');\" /></td>";
                    }

                    if (i == 0) {
                        tabContents = "<tr style='cursor: pointer;' class='highlight'>" + vdelete + "<td style='width: 97%' onclick=\"GridOpenFile('" + objdata.FilesList[i].Scanned_File_Name + "','" + objdata.FilePath + "','IsPendingCheckTrue',this);\">" + objdata.FilesList[i].Scanned_File_Name + "</td>" +
                                                   "<td style='display:none'>" + objdata.FilePath + objdata.FilesList[i].Scanned_File_Name + "</td>" +
                                                  "</tr>";
                        FirstFileName = objdata.FilesList[i].Scanned_File_Name;
                        FirstFilePath = objdata.FilePath;
                    }
                    else {
                        tabContents = tabContents + "<tr style='cursor: pointer;'>" + vdelete + "<td style='width: 97%' onclick=\"GridOpenFile('" + objdata.FilesList[i].Scanned_File_Name + "','" + objdata.FilePath + "','IsPendingCheckTrue',this);\">" + objdata.FilesList[i].Scanned_File_Name + "</td>" +
                                                  "<td style='display:none'>" + objdata.FilePath + objdata.FilesList[i].Scanned_File_Name + "</td>" +
                                                 "</tr>";
                    }
                }
            }
            if (tabContents == "") {
                tabContents = "<tr style='color:red'><td colspan='2'>" + objdata.IsNotification + "</td></tr>";
                $("#tbFilesBody").append(tabContents);
            }
            else {
                $("#tbFilesBody").append(tabContents);
                GridOpenFile(FirstFileName, FirstFilePath, "");

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
        }
    });
}

function OnLoadGridMyscan() {
    StopLoadOnUploadFile();

    var tabContents = "<tr class='highlight'> <td style='display:none'></td> <td style='width: 97%'\">"
        + document.getElementById("hdnFileName").value +
        "</td><td style='display:none'>"
        + document.getElementById("hdnfilepath").value + "\\" + document.getElementById("hdnFileName").value +
        "</td></tr>";
    $("#tbFilesBody").append(tabContents);
    GridOpenFile(document.getElementById("hdnFileName").value, document.getElementById("hdnfilepath").value, "");
}


function GridOpenFile(SelectedFileName, sFilePath, IsPendingCheck, isClick) {
    //Cap - 1141
    if (document.getElementById("grdIndexing").rows.length <= 1)
     localStorage.setItem('IsSaveClickedSucessfull', "");
    sFilePath = sFilePath.split("\\").join("/");
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (document.getElementById("hdnIsMyScan").value == "" && document.getElementById("grdIndexing").rows.length > 1 && IsPendingCheck != "") {
        if (confirm("File upload Pending! Are you sure to proceed to the next file? The pending file can be found in My Scan.")) {
            if (isClick != undefined && isClick != null)
                select(isClick.parentNode);
            $("#grdIndexing tr.Editabletxtbox").remove();

            $.ajax({
                type: "POST",
                async: true,
                url: "frmIndexing.aspx/PendingFile",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    OnLoadGrid($("#hdnsourceFile").val());
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
                }
            });

        }
        else {
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;
        }
    }
    else {
        if (isClick != undefined && isClick != null)
            select(isClick.parentNode);
        if (document.getElementById("hdnIsMyScan").value == "")
            $("#dtpDocumentDate").val($("#dtpScannedDate").val());
        $('#rdbAll')[0].checked = true;
        document.getElementById("btnSave").disabled = true;
        document.getElementById("txtSelectedPages").disabled = true;
        $("#txtSelectedPages").val("");
        document.getElementById("btnMoveToNextProcess").disabled = false;
        var sFullpath = sFilePath + "/" + SelectedFileName;
        //Open File
        $.ajax({
            type: "POST",
            async: true,
            url: "frmIndexing.aspx/OpenGridFile",
            data: '{filename: "' + SelectedFileName + '",filepath: "' + sFilePath + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                sFilePath = sFilePath + "/" + SelectedFileName;
                var Pagecount = data.d;
                $("#hdnPagecount")[0].value = Pagecount;
                //pop():return the last item from an array of values
                if (SelectedFileName.split('.').pop().toUpperCase() != "PDF") {
                    HidePdf();
                    $("#PageBox")[0].value = "1";
                    $("#PageLabel")[0].innerText = "/" + Pagecount;
                    document.getElementById("next").setAttribute('onClick', 'ChangePg(this)');
                    document.getElementById("prev").setAttribute('onClick', 'ChangePg(this)');

                    //Enable delete icon for tiff pages(Remote directory)
                    document.getElementById('deletethumbnail').style.display = "none";
                    if ($('#rdbRemoteDir')[0].checked == true) {

                        if (sFullpath != null) {
                            var extension = new Array,
                                len = sFullpath.split('.').length;
                            extension = sFullpath.split('.')[parseInt(len - 1)];
                            if (extension.toUpperCase() == "TIFF" || extension.toUpperCase() == "TIF") {
                                document.getElementById('deletethumbnail').style.display = "block";

                                document.getElementById("deletethumbnail").setAttribute('onclick', 'DeleteThumbnails("' + sFullpath + '","' + SelectedFileName + '","' + sFilePath + '")');
                            }
                        }

                    }
                    StopLoadOnUploadFile();
                    //CAP-964 //CAP-590
                    sFilePath = sFilePath.replaceAll("%", "%25");
                    sFilePath = sFilePath.replaceAll("#", "%23");
                    sFilePath = sFilePath.replaceAll('"', "%22");
                    sFilePath = sFilePath.replaceAll("<", "%3C");
                    sFilePath = sFilePath.replaceAll(">", "%3E");
                    sFilePath = sFilePath.replaceAll("|", "%7C");
                    document.getElementById("_imgBig").src = "ViewImg.aspx?View=1&FilePath=" + sFilePath + "&Pg=1&Height=650&Width=550";
                }
                else {
                    ViewPDF();
                    StopLoadOnUploadFile();
                    if (sFilePath.indexOf("////") != -1) {
                        sFilePath = sFilePath.replaceAll(/\/\//g, "/");
                    }
                    //CAP-964 //CAP-590
                    sFilePath = sFilePath.replaceAll(document.getElementById("hdnScanningLocal").value.replaceAll(/\\/g, "/"), document.getElementById("hdnPDFurl").value)
                    //Url encoding for special characters.
                    sFilePath = sFilePath.replaceAll("%", "%25");
                    sFilePath = sFilePath.replaceAll("#", "%23");
                    sFilePath = sFilePath.replaceAll('"', "%22");
                    sFilePath = sFilePath.replaceAll("<", "%3C");
                    sFilePath = sFilePath.replaceAll(">", "%3E");
                    sFilePath = sFilePath.replaceAll("|", "%7C");

                    document.getElementById("bigImagePDF").src = sFilePath;
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
            }
        });
    }
}


function select(e) {
    var existingSelectedItem = $("#tbFilesBody tr.highlight");
    if (existingSelectedItem.length > 0) { existingSelectedItem.removeClass("highlight"); }
    $(e).toggleClass("highlight");
}

function HidePdf() {
    document.getElementById('imgControls').style.display = "block";
    document.getElementById('PDFholder').style.display = "none";
    document.getElementById('bigImagePDF').style.display = "none";
    document.getElementById('imgholder').style.display = "block";
}

function LoadSubDocType() {
    $.ajax({
        type: "GET",
        url: "ConfigXML/Doctype.xml",
        dataType: "xml",
        async: false,
        cache: false,
        success: function (xml) {

            $(xml).find('DocElement').each(function () {
                var name_text = $(this)[0].attributes[0].nodeValue;
                if (name_text == "Patient Documents") {
                    var PhyEmptyOption = document.createElement("option");
                    PhyEmptyOption.text = "";
                    PhyEmptyOption.value = "0";
                    $("#cboDocumentSubType")[0].options.add(PhyEmptyOption);
                    var description = $(this)[0];
                    for (var i = 0; i < description.children.length; i++) {
                        var PhyOption = document.createElement("option");
                        PhyOption.text = description.children[i].attributes[0].value;
                        PhyOption.value = description.children[i].attributes[0].value;
                        $("#cboDocumentSubType")[0].options.add(PhyOption);
                    }
                    document.getElementById("cboDocumentSubType").selectedIndex = 5;
                }
            });
        }
    });
}