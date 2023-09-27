function ValidateProc() {
    DisplayErrorMessage('8407001');
    return false;
}

function OpenSearchOrders(OpeningMode) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    var now = new Date();
    var utcnew = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utcnew;
    document.getElementById("hdnOpenningMode").value = OpeningMode;
    if (document.getElementById('btnAdd').disabled == false) {
        DisplayErrorMessage('9093040');
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
    else {
        var obj = new Array();
        obj.push("OrderType=" + "DIAGNOSTIC ORDER");
        if (OpeningMode == "SCAN") {
            obj.push("LabName=" + document.getElementById('hdnLabName').value);
            obj.push("PhyId=" + document.getElementById('hdnPhyID').value);
        }
        obj.push("MyHumanID=" + document.getElementById('hdnHumanID').value);
        var result = openModal("frmSearchOrders.aspx", 750, 950, obj, "MREWindow");
        var WindowName = $find('MREWindow');
        WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
        WindowName.add_close(OnClientCloseSearchOrders);
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}
function CloseWithOK() {
    DisplayErrorMessage('230001');
    var Result = new Object();
    Result.SelectedItem = document.getElementById('hdnSelectedItem').value;
    Result.LabName = document.getElementById('hdnLabName').value;
    Result.LabID = document.getElementById('hdnLabID').value;
    Result.PhyID = document.getElementById('hdnPhyID').value;
    returnToParent(Result);
    window.close();
}

function loadmanualentry() {
    $("span[mand=Yes]").addClass('MandLabelstyle');
    $("span[mand=Yes]").each(function () {
        $(this).html($(this).html().replace("*", "<span class='manredforstar'>*</span>"));
    });
    $("[id*=pbDropdown]").addClass('pbDropdownBackground');
}

function EnableSave() {
    document.getElementById('btnAdd').disabled = false;
    document.getElementById('btnSaveandMovetonextprocess').disabled = false;
}
function Close() {
    if (document.getElementById('btnAdd').disabled == false) {
        var OpeningMode = document.getElementById("hdnOpenningMode").value;
        if (document.getElementById("hdnOpenningMode").value != "") {
            if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                document.getElementById("hdnOpenningMode").value = "";
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                document.getElementById('btnAdd').click();
                document.getElementById("hdnOpenningMode").value = "";
                var obj = new Array();
                obj.push("OrderType=" + "DIAGNOSTIC ORDER");
                if (OpeningMode == "SCAN") {
                    obj.push("LabName=" + document.getElementById('hdnLabName').value);
                    obj.push("PhyId=" + document.getElementById('hdnPhyID').value);
                }
                obj.push("MyHumanID=" + document.getElementById('hdnHumanID').value);
                var result = openModal("frmSearchOrders.aspx", 750, 950, obj, "MREWindow");
                var WindowName = $find('MREWindow');
                WindowName.setSize(950, 750);
                WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                WindowName.add_close(OnClientCloseSearchOrders);
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                document.getElementById(GetClientId("hdnMessageType")).value = ""
                document.getElementById("hdnOpenningMode").value == "";
                var obj = new Array();
                obj.push("OrderType=" + "DIAGNOSTIC ORDER");
                if (OpeningMode == "SCAN") {
                    obj.push("LabName=" + document.getElementById('hdnLabName').value);
                    obj.push("PhyId=" + document.getElementById('hdnPhyID').value);
                }
                obj.push("MyHumanID=" + document.getElementById('hdnHumanID').value);
                var result = openModal("frmSearchOrders.aspx", 750, 950, obj, "SearchWindow");
                var WindowName = $find('SearchWindow');
                WindowName.set_behaviors(-Telerik.Web.UI.WindowAutoSizeBehaviors.Close);
                WindowName.setSize(950, 750);
                WindowName.add_close(OnClientCloseSearchOrders);
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                document.getElementById(GetClientId("hdnMessageType")).value = "";
                document.getElementById("hdnOpenningMode").value == "";
                return false;
            }
        }
        else if (document.getElementById("hdnIsRowClick").value == "true") {
            var tableView = $find('grdSubComponent');
            if (tableView != null && $find('grdSubComponent').get_masterTableView() != null) {
                if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                    DisplayErrorMessage('020009');
                    return false;
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                    document.getElementById('btnAdd').click();
                    document.getElementById("btnFillGrid").click();
                    document.getElementById("btnAdd").disabled = true;
                    document.getElementById("btnSaveandMovetonextprocess").disabled = true;
                    return true;
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                    document.getElementById(GetClientId("hdnMessageType")).value = ""
                    document.getElementById("hdnIsRowClick").value = "";
                    document.getElementById("btnAdd").disabled = true;
                    var grid = $find('grdTestOrdered').get_masterTableView()
                    var Index = document.getElementById("hdnSelectedIndex").value;
                    var row = grid.get_dataItems()[Index];
                    row.set_selected(true);
                    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
                    document.getElementById("btnFillGrid").click();
                    return true;
                }
                else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                    document.getElementById(GetClientId("hdnMessageType")).value = "";
                    document.getElementById("hdnIsRowClick").value = "";
                    return false;
                }
            }
        }
        else {
            if (window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable != undefined) {
                window.parent.parent.parent.parent.theForm.ctl00_C5POBody_hdnIsSaveEnable.value = false;
            }
            if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
                DisplayErrorMessage('9093040');
                { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
                document.getElementById("btnAdd").click();
                document.getElementById("btnAdd").disabled = false;
                return false;
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
                document.getElementById(GetClientId("hdnMessageType")).value = ""
                if (document.getElementById("hdnScreenMode").value == "Indexing") {
                    $(top.window.document).find("#btnSearchProcedureClose").click();
                }
                else
                    GetRadWindow().close();
                return false;
            }
            else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
                document.getElementById(GetClientId("hdnMessageType")).value = "";
            }
        }
    }
    else if (document.getElementById("hdnScreenMode").value == "Indexing") {
        $(top.window.document).find("#btnSearchProcedureClose").click();
    }
    else {
        GetRadWindow().close();
        return false;
    }
}
function CloseOrders() {
    if (document.getElementById('btnOk').disabled == false) {
        if (window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable != undefined || window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable != null) {
            window.parent.parent.parent.parent.theForm.ctl00$IsSaveEnable.value = "false";
        }
        if (document.getElementById(GetClientId("hdnMessageType")).value == "") {
            DisplayErrorMessage('9093040');
            { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
            return false;

        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Yes") {
            document.getElementById("btnOk").click();
            return true;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "No") {
            document.getElementById(GetClientId("hdnMessageType")).value = ""
            if (document.getElementById("hdnScreenMode").value == "Indexing") {
                $(top.window.document).find("#btnSearchProcedureClose").click();
            }
            else
                GetRadWindow().close();
            return false;
        }
        else if (document.getElementById(GetClientId("hdnMessageType")).value == "Cancel") {
            document.getElementById(GetClientId("hdnMessageType")).value = "";
            return false;
        }
    }
    else if (document.getElementById("hdnScreenMode").value == "Indexing") {
        $(top.window.document).find("#btnSearchProcedureClose").click();
    }
    else {
        GetRadWindow().close();
        return false;
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
    document.getElementById('btnOk').disabled = false;
}

function btnAllProcedures_Clicked(sender, args) {
    if ((document.getElementById('cboPhysician').value == "") || (document.getElementById('cboPhysician').value == null)) {
        DisplayErrorMessage('115024');
        return false;
    }
    if ((document.getElementById('cboLab').value == " ") || (document.getElementById('cboLab').value == null)) {
        DisplayErrorMessage('230140');
        return false;
    }
    var ddlLab = $find("cboLab");
    var ddlPhysician = $find("cboPhysician");
    var obj = new Array();
    obj.push("SourceScreen=MRE");
    obj.push("SelectedLab=" + ddlLab.get_selectedItem().get_text());
    obj.push("PhyId=" + ddlPhysician.get_selectedItem().get_value());
    obj.push("selectedLabID=" + ddlLab.get_selectedItem().get_value());
    openModal("frmAllProcedures.aspx", 550, 680, obj, 'MessageWindow');
    $find('MessageWindow').add_close(OnClientCloseAllProcedures);
    //Jira #CAP-591
    $find("MessageWindow")._iframe.style.height = "100%";
    $find("MessageWindow")._iframe.style.width = "100%";
    $("#RadWindowWrapper_MessageWindow").css('top', '120px')
    $($("#RadWindowWrapper_MessageWindow")[0].childNodes[0]).css('height', '500px')
    return false;

}
function OnClientCloseAllProcedures(oWindow, args) {
    var result = args.get_argument();
    if (result) {

        if (result != null) {
            if (result != undefined && result.SelectedCPT != undefined) {
                var elementRef = document.getElementById("hdnTransferVaraible");
                elementRef.value = result.SelectedCPT;
                document.getElementById('InvisibleButton').click();
            }

        }


    }


}

function OpenViewIndexedImage(FileName, humanid) {
    var obj = new Array();
    obj.push("FileName=" + FileName);
    obj.push("Source=" + "MRE");
    obj.push("HumanId=" + humanid);
    openModal("frmImageViewer.aspx", 800, 1000, obj, "MessageWindow");

}
function ClearAll(sender, args) {
    { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); }
    if (DisplayErrorMessage('7200013') == true) {
        document.getElementById('btnClear').click();
    }
    else {
        { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }
        return false;
    }
}
function RowClick(sender, eventArgs) {

    var now = new Date();
    var utcnew = now.toUTCString();
    document.getElementById(GetClientId("hdnLocalTime")).value = utcnew;
    var tableView = $find('grdSubComponent');
    if (tableView != null && $find('grdSubComponent').get_masterTableView() != null) {
        document.getElementById("hdnSelectedIndex").value = eventArgs._itemIndexHierarchical;

        if (document.getElementById('btnAdd').disabled == false) {
            document.getElementById("hdnOpenningMode").value = "";
            document.getElementById("hdnIsRowClick").value = "true";
            eventArgs.set_cancel(true);
            //DisplayErrorMessage('020009');
            document.getElementById('btnAdd').disabled = false;
            return false;

        } else { { sessionStorage.setItem('StartLoading', 'true'); StartLoadFromPatChart(); } }

    }
}
function MREOnCommand(sender, args) {
    var CommanArgs = args.get_commandName();
    if (CommanArgs == "Del") {
        if (DisplayErrorMessage("230105")) {
            args.set_cancel(false);
        }
        else {
            args.set_cancel(true);
        }
    }
}
function OnClientCloseSearchOrders(oWindow, args) {
    var result = args.get_argument();
    if (result) {

        if (result != null) {
            document.getElementById('hdnSelectedItem').value = result.SelectedItem;
            document.getElementById('hdnLabName').value = result.LabName;
            document.getElementById('hdnLabID').value = result.LabID;
            document.getElementById('hdnPhyID').value = result.PhyID;

        }
        if (document.getElementById('hdnLabName').value != '') {
            document.getElementById('InvisibleButton').click();
        }

    }
    { sessionStorage.setItem('StartLoading', 'false'); StopLoadFromPatChart(); }

}
function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
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
        //self.close();
        $(top.window.document).find("#txtSearchProcedureInformation")[0].value = JSON.stringify(oArg.result);
        $(top.window.document).find("#btnSearchProcedureClose").click();
    }
}

function RowSelected(sender, eventArgs) {
    var grid = sender;
    var MasterTable = grid.get_masterTableView(); var row = MasterTable.get_dataItems()[eventArgs.get_itemIndexHierarchical()];
    var cell = MasterTable.getCellByColumnUniqueName(row, "ProcedureCode");
    var OrderSumbit = MasterTable.getCellByColumnUniqueName(row, "OrderSubmitID");
    document.getElementById('hdnSelectedPanel').value = cell.innerText;
    if (OrderSumbit.innnerText != undefined) {
        document.getElementById('hdnAddNewOrderSubmitID').value = OrderSumbit.innnerText;
    }
}


function AssignUTCTime() {
    var now = new Date();
    var utcnew = now.toUTCString();
    document.getElementById("hdnLocalTime").value = utcnew;
    return true;
}
function DateSelected() {
    document.getElementById('btnAdd').disabled = false;
    document.getElementById('btnSaveandMovetonextprocess').disabled = false;
}
function CancelYesNo() {
    DisplayErrorMessage('7200039');
    document.getElementById(GetClientId("hdnMessageType")).value = ""
    self.close();
}

function CallBack(e) {
    if (e.ctrlKey && e.keyCode == 86)
        document.getElementById('btnSaveandMovetonextprocess').disabled = false;
}



